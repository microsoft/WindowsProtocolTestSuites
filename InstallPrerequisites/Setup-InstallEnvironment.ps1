# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param
(
    [string]$Workspace,
    # The virtual hard disk for creating a server VM
    [string]$VHDName = "18362.1.amd64fre.19h1_release.190318-1202_server_serverdatacenter_en-us_vl_VS2017.vhd",
    # The path
    [string]$VHDPath = "\\pet-storage-04\PrototestRegressionShare\VHDShare"
)

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$WinteropProtocolTesting = "$Workspace\..\..\WinteropProtocolTesting"
Write-Host "=============================================="
Write-Host "InvocationPath: $InvocationPath"
Write-Host "Workspace： $Workspace"
Write-Host "WinteropProtocolTesting: $WinteropProtocolTesting"
Write-Host "=============================================="

function Get-VHD {
    if(!(Test-Path $VHDPath\$VHDName)){
        Write-Host "Cannot find the VHD on $VHDPath\$VHDName" Exit
    }
    Write-Host "Copy VHD from $VHDPath\$VHDName to $WinteropProtocolTesting\VM\InstallPrerequisites..."
    if(!(Test-Path "$WinteropProtocolTesting\VM\InstallPrerequisites")) {
        mkdir "$WinteropProtocolTesting\VM\InstallPrerequisites"
    }
    Copy-Item "$VHDPath\$VHDName" -Destination "$WinteropProtocolTesting\VM\InstallPrerequisites\" -Force
    Rename-Item "$WinteropProtocolTesting\VM\InstallPrerequisites\$VHDName" -NewName "InstallPrerequisites.vhd" -Force
    Write-Host "Copy VHD finished"
}

function Main {
    Get-VHD
}

Main