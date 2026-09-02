# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Disruptive feature-only DSC configuration for the Cluster Storage server.
#>

Configuration StorageFeatureConfiguration {
    Import-DscResource -ModuleName PSDesiredStateConfiguration

    $requiredFeatures = @(
        'File-Services',
        'FS-iSCSITarget-Server'
    )

    Node 'localhost' {
        Script StorageFeatureBundle {
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
                    -Name 'StorageFeatureBundleAttempted' `
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
                    throw "Storage feature installation failed: $($result.ExitCode)"
                }
                if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
                    New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
                }
                Set-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'StorageFeatureBundleAttempted' -Value 1 -Type DWord -Force
            }
        }
    }
}

function Invoke-StorageFeatureDsc {
    param(
        [string]$OutputPath = "$PSScriptRoot\MOF\Storage-Features"
    )

    StorageFeatureConfiguration -OutputPath $OutputPath
    Write-Host "Storage feature MOF compiled to '$OutputPath'." -ForegroundColor Green
}
