# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release"
) 

Write-Host ======================================
Write-Host          Start to build RDPSUTControlAgent
Write-Host ======================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../../"

dotnet publish "$TestSuiteRoot/TestSuites/RDP/RDPSUTControlAgent/CSharp/RDPSUTControlAgent.sln" -c $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build RDP SUT Control Agent"
    exit 1
}

Write-Host ==========================================================
Write-Host          Build RDPSUTControlAgent successfully 
Write-Host ==========================================================