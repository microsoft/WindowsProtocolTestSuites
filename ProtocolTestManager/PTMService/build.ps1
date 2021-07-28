# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration = "Release",
    [string]$OutDir
) 

Write-Host ======================================
Write-Host          Start to build PTMService
Write-Host ======================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/PTMService"
}

if (Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

dotnet publish "$TestSuiteRoot/ProtocolTestManager/PTMService/PTMService.sln" -c $Configuration -o $OutDir
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build PTMService"
    exit 1
}

Write-Host ==========================================================
Write-Host          Build PTMService successfully         
Write-Host ==========================================================