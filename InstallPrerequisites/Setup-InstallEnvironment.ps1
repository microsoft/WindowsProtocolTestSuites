# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param
(
    # The virtual hard disk for creating a server VM
    [string]$VHDName = "18362.1.amd64fre.19h1_release.190318-1202_server_serverdatacenter_en-us_vl_VS2017.vhd",
    # The path
    [string]$VHDPath = "\\pet-storage-04\PrototestRegressionShare\VHDShare"
)

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent

Write-Host "=============================================="
Write-Host "InvocationPath: $InvocationPath"
Write-Host "=============================================="

function Main {
    if(!(Test-Path $VHDName\$VHDPath)){
        Write-Host "Cannot find the VHD on $VHDName\$VHDPath" Exit
    }
    Write-Host "Workspace $(Pipeline.Workspace)"
    Write-Host "test script"
}

Main