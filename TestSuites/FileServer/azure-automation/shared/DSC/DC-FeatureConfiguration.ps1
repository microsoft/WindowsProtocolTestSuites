# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Pre-promotion feature configuration for the Domain Controller.

.DESCRIPTION
    Installs all disruptive DC roles and management features in one DSC Script
    resource so feature servicing and an optional hostname change can share the
    first planned reboot.
#>

Configuration DcFeatureConfiguration {
    Import-DscResource -ModuleName PSDesiredStateConfiguration

    $requiredFeatures = @(
        'AD-Domain-Services',
        'RSAT-AD-Tools',
        'GPMC',
        'RemoteAccess',
        'RSAT-RemoteAccess'
    )

    Node 'localhost' {
        Script DomainControllerFeatureBundle {
            GetScript = {
                $states = foreach ($featureName in $using:requiredFeatures) {
                    $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
                    "$featureName=$($feature.InstallState)"
                }
                @{ Result = $states -join '; ' }
            }
            TestScript = {
                $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'DcFeatureBundleAttempted' -ErrorAction SilentlyContinue
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
                    -IncludeAllSubFeature -IncludeManagementTools -ErrorAction Stop
                if (-not $result.Success) {
                    throw "Required Domain Controller feature installation failed: $($result.ExitCode)"
                }
                if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
                    New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
                }
                New-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'DcFeatureBundleAttempted' -Value 1 `
                    -PropertyType DWord -Force | Out-Null
            }
        }
    }
}
