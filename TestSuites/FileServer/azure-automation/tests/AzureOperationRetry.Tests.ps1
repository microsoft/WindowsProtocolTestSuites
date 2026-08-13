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
