# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path
. (Join-Path $root 'shared\DSC\Deploy-CommonHelpers.ps1')

Describe 'Verified DSC execution' {
    BeforeEach {
        Mock Start-DscConfiguration {}
        Mock Start-Sleep {}
    }

    It 'accepts only a fresh successful LCM result' {
        Mock Get-DscConfigurationStatus {
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Success'
                Error = $null
            }
        }

        $result = Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5

        $result.Status | Should Be 'Success'
        Assert-MockCalled Start-DscConfiguration -Times 1
    }

    It 'retries a transient status-probe failure with a fresh call' {
        $script:probeCount = 0
        Mock Get-DscConfigurationStatus {
            $script:probeCount++
            if ($script:probeCount -eq 1) {
                throw 'WinRM restarted'
            }
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Success'
                Error = $null
            }
        }

        $result = Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5

        $result.Status | Should Be 'Success'
        $script:probeCount | Should Be 2
    }

    It 'suppresses the expected PowerShell 5.1 busy response while DSC is active' {
        $script:probeCount = 0
        Mock Get-DscConfigurationStatus {
            $script:probeCount++
            if ($script:probeCount -eq 1) {
                Write-Error 'The Start-DscConfiguration cmdlet is in progress and must return before Get-DscConfigurationStatus can be invoked.'
                return
            }
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Success'
                Error = $null
            }
        }

        $previousPreference = $ErrorActionPreference
        try {
            $ErrorActionPreference = 'Stop'
            $result = Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5
        }
        finally {
            $ErrorActionPreference = $previousPreference
        }

        $result.Status | Should Be 'Success'
        $script:probeCount | Should Be 2
    }

    It 'does not accept a successful status from a previous run' {
        $script:probeCount = 0
        $priorStart = (Get-Date).AddMinutes(-1)
        Mock Get-DscConfigurationStatus {
            $script:probeCount++
            if ($script:probeCount -eq 1) {
                return [pscustomobject]@{
                    StartDate = $priorStart
                    Status = 'Success'
                    Error = $null
                }
            }
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Success'
                Error = $null
            }
        }

        $result = Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5

        $result.StartDate | Should BeGreaterThan $priorStart
        $script:probeCount | Should Be 2
    }

    It 'preserves source recency when fresh DSC statuses share a start time' {
        $sameStart = (Get-Date).AddSeconds(1)
        Mock Get-DscConfigurationStatus {
            @(
                [pscustomobject]@{
                    StartDate = $sameStart
                    Status = 'Success'
                    Error = $null
                },
                [pscustomobject]@{
                    StartDate = $sameStart
                    Status = 'Failure'
                    Error = @('The SendConfigurationApply function did not succeed.')
                }
            )
        }

        $result = Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5

        $result.Status | Should Be 'Success'
    }

    It 'throws when the LCM reports failure' {
        Mock Get-WinEvent { @() }
        Mock Get-DscConfigurationStatus {
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Failure'
                Error = @('resource failed')
            }
        }

        { Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5 } |
            Should Throw 'resource failed'
    }

    It 'surfaces the failed DSC resource and nested error details' {
        Mock Get-DscConfigurationStatus {
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Failure'
                Error = @([pscustomobject]@{
                    Message = 'The SendConfigurationApply function did not succeed.'
                    ErrorSource = 'LCM'
                    ErrorCode = 1
                })
                ResourcesNotInDesiredState = @([pscustomobject]@{
                    ResourceId = '[Script]EnablePSRemoting'
                    SourceInfo = 'Driver-Configuration.ps1::270::9::Script'
                    ModuleName = 'PSDesiredStateConfiguration'
                    Error = @([pscustomobject]@{
                        Message = 'Access is denied.'
                        ErrorSource = 'Set-TargetResource'
                        ErrorCode = 5
                    })
                })
            }
        }

        { Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5 } |
            Should Throw '[Script]EnablePSRemoting'
    }

    It 'waits through a generic apply failure when a fresh success follows' {
        $script:probeCount = 0
        Mock Get-WinEvent { @() }
        Mock Get-DscConfigurationStatus {
            $script:probeCount++
            if ($script:probeCount -eq 1) {
                return [pscustomobject]@{
                    StartDate = Get-Date
                    Status = 'Failure'
                    Error = @('The SendConfigurationApply function did not succeed.')
                    ResourcesNotInDesiredState = @()
                }
            }
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Success'
                Error = $null
                ResourcesNotInDesiredState = @()
            }
        }

        $result = Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5 `
            -PollIntervalSeconds 1

        $result.Status | Should Be 'Success'
        $script:probeCount | Should Be 2
    }

    It 'treats a null failed-resource collection as no failed resources' {
        $script:probeCount = 0
        Mock Get-WinEvent { @() }
        Mock Get-DscConfigurationStatus {
            $script:probeCount++
            if ($script:probeCount -eq 1) {
                return [pscustomobject]@{
                    StartDate = Get-Date
                    Status = 'Failure'
                    Error = @('The SendConfigurationApply function did not succeed.')
                    ResourcesNotInDesiredState = $null
                }
            }
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Success'
                Error = $null
                ResourcesNotInDesiredState = $null
            }
        }

        $result = Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5 `
            -PollIntervalSeconds 1

        $result.Status | Should Be 'Success'
    }

    It 'does not defer a named error accompanying the generic apply error' {
        Mock Get-WinEvent { @() }
        Mock Get-DscConfigurationStatus {
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Failure'
                Error = @(
                    'The SendConfigurationApply function did not succeed.',
                    'Access is denied.'
                )
                ResourcesNotInDesiredState = $null
            }
        }

        { Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5 } |
            Should Throw 'Access is denied'
    }

    It 'adds recent DSC operational errors when the LCM returns only a generic failure' {
        Mock Get-DscConfigurationStatus {
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Failure'
                Error = @('The SendConfigurationApply function did not succeed.')
                ResourcesNotInDesiredState = @()
            }
        }
        Mock Get-WinEvent {
            [pscustomobject]@{
                Id = 4252
                Level = 2
                Message = 'Script resource MultiNicRouting failed.'
            }
        }

        { Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5 } |
            Should Throw 'MultiNicRouting failed'
    }

    It 'throws when required postconditions are missing' {
        Mock Get-DscConfigurationStatus {
            [pscustomobject]@{
                StartDate = Get-Date
                Status = 'Success'
                Error = $null
            }
        }

        { Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5 -Postcondition { $false } } |
            Should Throw 'postconditions'
    }

    It 'writes machine-readable heartbeat progress while DSC is active' {
        $heartbeat = Join-Path $env:TEMP "dsc-heartbeat-$([guid]::NewGuid().ToString('N')).json"
        try {
            Mock Get-DscConfigurationStatus {
                [pscustomobject]@{
                    StartDate = Get-Date
                    Status = 'Success'
                    Error = $null
                }
            }

            Invoke-VerifiedDscConfiguration -Path 'C:\MOF' -TimeoutSeconds 5 `
                -HeartbeatPath $heartbeat -PhaseName 'FeaturePhase' | Out-Null

            $state = Get-Content -Path $heartbeat -Raw | ConvertFrom-Json
            $state.Phase | Should Be 'FeaturePhase'
            $state.Operation | Should Be 'DSC configuration'
            $state.TimestampUtc | Should Not BeNullOrEmpty
        }
        finally {
            Remove-Item -Path $heartbeat -Force -ErrorAction SilentlyContinue
        }
    }
}

Describe 'Deployment state helpers' {
    It 'returns the default when an optional registry value is absent' {
        Mock Get-ItemProperty {
            [pscustomobject]@{
                DeployStep = 1
            }
        }

        Get-DeploymentRegistryValue -Name 'DriverToolsRebootPending' `
            -DefaultValue 0 | Should Be 0
    }

    It 'returns an existing optional registry value' {
        Mock Get-ItemProperty {
            [pscustomobject]@{
                DriverJoinRebootPending = 1
            }
        }

        Get-DeploymentRegistryValue -Name 'DriverJoinRebootPending' `
            -DefaultValue 0 | Should Be 1
    }

    It 'detects non-empty PendingFileRenameOperations registry values' {
        Mock Test-Path { $false }
        Mock Get-ItemProperty {
            [pscustomobject]@{
                PendingFileRenameOperations = @(
                    '\??\C:\Windows\Temp\pending.tmp',
                    ''
                )
            }
        }

        Test-PendingSystemReboot | Should Be $true
        (@(Get-PendingSystemRebootReasons) -contains 'PendingFileRenameOperations:1') |
            Should Be $true
    }

    It 'ignores an empty PendingFileRenameOperations registry value' {
        Mock Test-Path { $false }
        Mock Get-ItemProperty {
            [pscustomobject]@{
                PendingFileRenameOperations = @('')
            }
        }

        Test-PendingSystemReboot | Should Be $false
        @(Get-PendingSystemRebootReasons).Count | Should Be 0
    }

    It 'writes a non-empty verified deployment signal' {
        $signal = Join-Path $env:TEMP "deployment-signal-$([guid]::NewGuid().ToString('N')).signal"
        try {
            Write-VerifiedDeploymentSignal -Path $signal -Content 'DEPLOY FINISHED'

            (Get-Item $signal).Length | Should BeGreaterThan 0
        }
        finally {
            Remove-Item -Path $signal -Force -ErrorAction SilentlyContinue
        }
    }
}
