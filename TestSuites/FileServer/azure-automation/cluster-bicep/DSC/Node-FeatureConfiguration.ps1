# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Disruptive feature-only DSC configuration shared by Cluster Node01/Node02.
#>

Configuration NodeFeatureConfiguration {
    Import-DscResource -ModuleName PSDesiredStateConfiguration

    $requiredFeatures = @(
        'Failover-Clustering',
        'RSAT-Clustering',
        'RSAT-Clustering-Mgmt',
        'RSAT-Clustering-PowerShell',
        'RSAT-Clustering-CmdInterface',
        'File-Services',
        'FS-BranchCache',
        'FS-VSS-Agent',
        'BranchCache',
        'FS-DFS-Namespace',
        'RSAT-File-Services',
        'RSAT-DFS-Mgmt-Con',
        'FS-Resource-Manager',
        'RSAT-AD-PowerShell'
    )

    Node 'localhost' {
        Script ClusterNodeFeatureBundle {
            GetScript = {
                $state = foreach ($featureName in $using:requiredFeatures) {
                    Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue |
                        Select-Object Name, InstallState
                }
                @{ Result = ($state | Out-String) }
            }
            TestScript = {
                $marker = Get-ItemProperty `
                    -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'ClusterNodeFeatureBundleAttempted' `
                    -ErrorAction SilentlyContinue
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
                    throw "Cluster node feature installation failed: $($result.ExitCode)"
                }

                if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
                    New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
                }

                $smb1State = 'NotApplicable'
                $osBuild = [System.Environment]::OSVersion.Version.Build
                if ($osBuild -lt 26100) {
                    $smb1 = Get-WindowsFeature -Name FS-SMB1 -ErrorAction SilentlyContinue
                    if ($null -eq $smb1) {
                        $smb1State = 'Unavailable'
                    }
                    elseif ($smb1.InstallState -eq 'Installed') {
                        $smb1State = 'Enabled'
                    }
                    else {
                        try {
                            $smb1Result = Install-WindowsFeature -Name FS-SMB1 `
                                -IncludeAllSubFeature -IncludeManagementTools -ErrorAction Stop
                            $smb1State = if ($smb1Result.Success) { 'Enabled' } else { 'Failed' }
                        }
                        catch {
                            $smb1State = 'Failed'
                        }
                    }
                }

                $hyperVState = 'Unavailable'
                $hyperV = Get-WindowsOptionalFeature -Online `
                    -FeatureName Microsoft-Hyper-V -ErrorAction SilentlyContinue
                if ($null -ne $hyperV) {
                    if ($hyperV.State -eq 'Enabled') {
                        $hyperVState = 'Enabled'
                    }
                    else {
                        try {
                            Enable-WindowsOptionalFeature -Online `
                                -FeatureName Microsoft-Hyper-V -All -NoRestart `
                                -ErrorAction Stop | Out-Null
                            $hyperVState = 'Enabled'
                        }
                        catch {
                            $hyperVState = 'Unsupported'
                        }
                    }
                }
                if ($hyperVState -eq 'Enabled') {
                    try {
                        Install-WindowsFeature -Name RSAT-Hyper-V-Tools `
                            -IncludeAllSubFeature -ErrorAction Stop | Out-Null
                    }
                    catch {
                        $hyperVState = 'EnabledWithoutManagementTools'
                    }
                }

                Set-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'ClusterNodeOptionalSmb1State' -Value $smb1State `
                    -Type String -Force
                Set-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'ClusterNodeOptionalHyperVState' -Value $hyperVState `
                    -Type String -Force
                Set-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'ClusterNodeFeatureBundleAttempted' -Value 1 `
                    -Type DWord -Force
            }
        }
    }
}

function Invoke-NodeFeatureDsc {
    param([string]$OutputPath = "$PSScriptRoot\MOF\Node-Features")

    NodeFeatureConfiguration -OutputPath $OutputPath
    Write-Host "Cluster node feature MOF compiled to '$OutputPath'." -ForegroundColor Green
}
