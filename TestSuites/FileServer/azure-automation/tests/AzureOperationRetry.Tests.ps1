# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$helpersPath = Join-Path $here '..\shared\Deploy-Helpers.psm1'
Import-Module $helpersPath -Force

Describe 'Azure control-plane retry handling' {
    It 'retries the observed HttpClient timeout and returns the eventual result' {
        $script:attempts = 0

        $result = Invoke-AzureOperationWithRetry -OperationName 'List compute SKUs' `
            -MaxAttempts 3 -InitialDelaySeconds 0 -MaximumDelaySeconds 0 -Operation {
                $script:attempts++
                if ($script:attempts -lt 3) {
                    throw 'The request was canceled due to the configured HttpClient.Timeout of 100 seconds elapsing.'
                }
                'sku-result'
            }

        $result | Should Be 'sku-result'
        $script:attempts | Should Be 3
    }

    It 'retries when the Azure management endpoint fails to respond' {
        $script:attempts = 0

        $result = Invoke-AzureOperationWithRetry -OperationName 'List compute SKUs' `
            -MaxAttempts 2 -InitialDelaySeconds 0 -MaximumDelaySeconds 0 -Operation {
                $script:attempts++
                if ($script:attempts -eq 1) {
                    throw 'A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond. (management.azure.com:443)'
                }
                'sku-result'
            }

        $result | Should Be 'sku-result'
        $script:attempts | Should Be 2
    }

    It 'does not retry a permanent authorization failure' {
        $script:attempts = 0

        {
            Invoke-AzureOperationWithRetry -OperationName 'List compute SKUs' `
                -MaxAttempts 5 -InitialDelaySeconds 0 -MaximumDelaySeconds 0 -Operation {
                    $script:attempts++
                    throw 'AuthorizationFailed: The client does not have authorization to perform action.'
                }
        } | Should Throw 'AuthorizationFailed'

        $script:attempts | Should Be 1
    }

    It 'stops after the configured number of transient attempts with context' {
        $script:attempts = 0

        {
            Invoke-AzureOperationWithRetry -OperationName 'List compute SKUs' `
                -MaxAttempts 3 -InitialDelaySeconds 0 -MaximumDelaySeconds 0 -Operation {
                    $script:attempts++
                    throw 'StatusCode: 429 TooManyRequests'
                }
        } | Should Throw 'List compute SKUs failed after 3 attempts'

        $script:attempts | Should Be 3
    }
}

Describe 'Deployment configuration merging' {
    It 'preserves explicit false and zero values while defaulting absent keys' {
        $resolved = Resolve-DeploymentConfig -Params @{
            enableTestAutoRun = $false
            probePort = 0
        } -Defaults @{
            enableTestAutoRun = $true
            probePort = 59998
            location = 'West US 2'
        }

        $resolved.enableTestAutoRun | Should Be $false
        $resolved.probePort | Should Be 0
        $resolved.location | Should Be 'West US 2'
    }
}

Describe 'Azure subscription context validation' {
    InModuleScope Deploy-Helpers {
        It 'reauthenticates when the cached subscription context has an expired token' {
            Mock Get-AzContext {
                [pscustomobject]@{
                    Subscription = [pscustomobject]@{ Id = 'sub-id'; Name = 'Test Subscription' }
                }
            }
            Mock Set-AzContext { }
            Mock Clear-AzContext { }
            Mock Connect-AzAccountFromAzureCli { $false }
            Mock Connect-AzAccount { }
            $script:probeCount = 0
            Mock Invoke-AzRestMethod {
                $script:probeCount++
                if ($script:probeCount -eq 1) { throw 'SharedTokenCacheCredential authentication unavailable.' }
                [pscustomobject]@{ StatusCode = 200 }
            }

            Connect-AzureSubscription -SubscriptionId 'sub-id'

            Assert-MockCalled Clear-AzContext -Times 1 -Scope It
            Assert-MockCalled Connect-AzAccount -Times 1 -Scope It
            $script:probeCount | Should Be 2
        }

        It 'reuses a cached context only after a live management-plane probe succeeds' {
            Mock Get-AzContext {
                [pscustomobject]@{
                    Subscription = [pscustomobject]@{ Id = 'sub-id'; Name = 'Test Subscription' }
                }
            }
            Mock Set-AzContext { }
            Mock Clear-AzContext { }
            Mock Connect-AzAccountFromAzureCli { $false }
            Mock Connect-AzAccount { }
            Mock Invoke-AzRestMethod { [pscustomobject]@{ StatusCode = 200 } }

            Connect-AzureSubscription -SubscriptionId 'sub-id'

            Assert-MockCalled Connect-AzAccount -Times 0 -Scope It
            Assert-MockCalled Invoke-AzRestMethod -Times 1 -Scope It
        }

        It 'reuses an authenticated Azure CLI session before interactive login' {
            Mock Get-AzContext {
                [pscustomobject]@{
                    Subscription = [pscustomobject]@{ Id = 'sub-id'; Name = 'Test Subscription' }
                }
            }
            Mock Clear-AzContext { }
            Mock Connect-AzAccountFromAzureCli { $true }
            Mock Connect-AzAccount { }
            $script:probeCount = 0
            Mock Invoke-AzRestMethod {
                $script:probeCount++
                if ($script:probeCount -eq 1) { throw 'SharedTokenCacheCredential authentication unavailable.' }
                [pscustomobject]@{ StatusCode = 200 }
            }

            Connect-AzureSubscription -SubscriptionId 'sub-id'

            Assert-MockCalled Connect-AzAccountFromAzureCli -Times 1 -Scope It
            Assert-MockCalled Connect-AzAccount -Times 0 -Scope It
            $script:probeCount | Should Be 2
        }

        It 'passes the CLI token only to a process-scoped Az login' {
            $moduleSource = Get-Content (Get-Module Deploy-Helpers).Path -Raw
            $moduleSource.Contains('-AccessToken $cliContext.AccessToken') | Should Be $true
            $moduleSource.Contains('-AccountId $cliContext.AccountId') | Should Be $true
            $moduleSource.Contains('-Tenant $cliContext.TenantId') | Should Be $true
            $moduleSource.Contains('-Subscription $SubscriptionId') | Should Be $true
            $moduleSource.Contains('-Scope Process') | Should Be $true
        }
    }
}

Describe 'Run Command conflict recovery' {
    BeforeEach {
        $verifierPath = Join-Path $here '..\shared\scripts\Verify-Deployment.ps1'
        $tokens = $null
        $parseErrors = $null
        $verifierAst = [System.Management.Automation.Language.Parser]::ParseFile(
            $verifierPath,
            [ref]$tokens,
            [ref]$parseErrors)
        $parseErrors.Count | Should Be 0

        $functionNames = @(
            'Get-RunCommandRecoveryState',
            'Register-RunCommandProbeTimeout',
            'Reset-RunCommandProbeFailure',
            'Test-RunCommandConflict',
            'Invoke-RunCommandConflictRecovery'
        )
        foreach ($functionName in $functionNames) {
            $functionAst = $verifierAst.Find(
                {
                    param($node)
                    $node -is [System.Management.Automation.Language.FunctionDefinitionAst] -and
                    $node.Name -eq $functionName
                },
                $true)
            $null -ne $functionAst | Should Be $true
            . ([scriptblock]::Create($functionAst.Extent.Text))
        }

        $script:ResourceGroupName = 'test-rg'
        $script:RunCommandConflictThreshold = 3
        $script:RunCommandRecoveryDelaySeconds = 300
        $script:RunCommandRecoveryLimit = 1
        $script:runCommandRecoveryState = @{}
        Mock Restart-AzVM { }
    }

    It 'does not restart for a conflict without a preceding local timeout' {
        $restarted = Invoke-RunCommandConflictRecovery -VMName 'client01' `
            -FailureMessage '409 Conflict: execution is in progress' -AllowRestart $true

        $restarted | Should Be $false
        Assert-MockCalled Restart-AzVM -Times 0 -Scope It
    }

    It 'restarts once after a timed-out probe has sustained enough conflicts' {
        Register-RunCommandProbeTimeout -VMName 'client01'
        $state = Get-RunCommandRecoveryState -VMName 'client01'
        $state.FirstTimeoutUtc = [datetime]::UtcNow.AddSeconds(-301)

        1..2 | ForEach-Object {
            Invoke-RunCommandConflictRecovery -VMName 'client01' `
                -FailureMessage 'Run command extension execution is in progress (409)' `
                -AllowRestart $true | Should Be $false
        }
        Invoke-RunCommandConflictRecovery -VMName 'client01' `
            -FailureMessage 'Run command extension execution is in progress (409)' `
            -AllowRestart $true | Should Be $true

        Assert-MockCalled Restart-AzVM -Times 1 -Scope It

        Register-RunCommandProbeTimeout -VMName 'client01'
        $state.FirstTimeoutUtc = [datetime]::UtcNow.AddSeconds(-301)
        1..3 | ForEach-Object {
            Invoke-RunCommandConflictRecovery -VMName 'client01' `
                -FailureMessage '409 Conflict' -AllowRestart $true | Should Be $false
        }
        Assert-MockCalled Restart-AzVM -Times 1 -Scope It
    }

    It 'never restarts a Linux target automatically' {
        Register-RunCommandProbeTimeout -VMName 'linux01'
        $state = Get-RunCommandRecoveryState -VMName 'linux01'
        $state.FirstTimeoutUtc = [datetime]::UtcNow.AddSeconds(-301)

        1..3 | ForEach-Object {
            Invoke-RunCommandConflictRecovery -VMName 'linux01' `
                -FailureMessage '409 Conflict' -AllowRestart $false | Should Be $false
        }

        Assert-MockCalled Restart-AzVM -Times 0 -Scope It
    }
}
