# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Pre-reboot feature configuration for the Workgroup SUT.

.DESCRIPTION
    Installs every disruptive SUT feature in one DSC Script resource so Windows
    cannot stop the configuration between feature resources when a reboot becomes
    pending. The orchestrator performs one planned reboot after this configuration.
#>

Configuration SutFeatureConfiguration {
    Import-DscResource -ModuleName PSDesiredStateConfiguration

    $requiredFeatures = @(
        'File-Services',
        'FS-BranchCache',
        'FS-VSS-Agent',
        'BranchCache',
        'FS-DFS-Namespace',
        'RSAT-File-Services',
        'RSAT-DFS-Mgmt-Con',
        'FS-Resource-Manager'
    )

    Node 'localhost' {
        Script WorkgroupSutFeatureBundle {
            GetScript = {
                $states = foreach ($featureName in $using:requiredFeatures) {
                    $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
                    "$featureName=$($feature.InstallState)"
                }
                @{ Result = $states -join '; ' }
            }
            TestScript = {
                $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'WorkgroupSutFeatureBundleAttempted' -ErrorAction SilentlyContinue
                if ($null -eq $marker) { return $false }

                foreach ($featureName in $using:requiredFeatures) {
                    $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
                    if ($null -eq $feature -or $feature.InstallState -ne 'Installed') {
                        return $false
                    }
                }
                return $true
            }
            SetScript = {
                $result = Install-WindowsFeature -Name $using:requiredFeatures `
                    -IncludeManagementTools -ErrorAction Stop
                if (-not $result.Success) {
                    throw "Required File Server feature installation failed: $($result.ExitCode)"
                }

                $osBuild = [System.Environment]::OSVersion.Version.Build
                if ($osBuild -lt 26100) {
                    $smb1 = Get-WindowsFeature FS-SMB1 -ErrorAction SilentlyContinue
                    if ($null -ne $smb1 -and $smb1.InstallState -ne 'Installed') {
                        $smb1Result = Install-WindowsFeature FS-SMB1 -IncludeAllSubFeature `
                            -IncludeManagementTools -ErrorAction SilentlyContinue
                        if ($null -eq $smb1Result -or -not $smb1Result.Success) {
                            Write-Warning 'FS-SMB1 installation failed because source files may be unavailable.'
                        }
                    }
                }

                try {
                    $hyperV = Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V `
                        -ErrorAction SilentlyContinue
                    if ($null -eq $hyperV -or $hyperV.State -ne 'Enabled') {
                        Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V `
                            -All -NoRestart -ErrorAction Stop | Out-Null
                    }
                }
                catch {
                    Write-Warning "Hyper-V installation failed because nested virtualization may be unavailable: $($_.Exception.Message)"
                }

                try {
                    $rsat = Get-WindowsFeature RSAT-Hyper-V-Tools -ErrorAction SilentlyContinue
                    if ($null -ne $rsat -and $rsat.InstallState -ne 'Installed') {
                        Install-WindowsFeature RSAT-Hyper-V-Tools -IncludeAllSubFeature `
                            -ErrorAction Stop | Out-Null
                    }
                }
                catch {
                    Write-Warning "RSAT Hyper-V tools installation failed: $($_.Exception.Message)"
                }

                if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
                    New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
                }
                New-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'WorkgroupSutFeatureBundleAttempted' -Value 1 `
                    -PropertyType DWord -Force | Out-Null
            }
        }
    }
}
