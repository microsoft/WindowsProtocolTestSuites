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

    It 'throws when the LCM reports failure' {
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
