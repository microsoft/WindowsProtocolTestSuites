# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
) 

Write-Host ======================================
Write-Host          Start to build FileServer
Write-Host ======================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/TestSuites/FileServer"
}

$CommonScripts ="Get-OSVersionNumber.ps1","Write-Error.ps1","Write-Info.ps1"

if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

if(!(Test-Path -Path $OutDir/Batch)) {
    New-Item -ItemType Directory $OutDir/Batch -Force
    Copy-Item  "$TestSuiteRoot/TestSuites/FileServer/src/Batch/*" -Destination "$OutDir/Batch/" -Recurse -Force
    Copy-Item "$TestSuiteRoot/common/*" -Destination "$OutDir/Batch/" -Recurse -Force
}

if(!(Test-Path -Path $OutDir/Scripts)) {
    New-Item -ItemType Directory $OutDir/Scripts -Force
    Copy-Item  "$TestSuiteRoot/TestSuites/FileServer/Setup/Scripts/*" -Destination "$OutDir/Scripts/" -Recurse -Force
    foreach ($curr in $CommonScripts) { 
        Copy-Item  "$TestSuiteRoot/CommonScripts/$curr" -Destination "$OutDir/Scripts/" -Recurse -Force
    }
}

Copy-Item  "$TestSuiteRoot/TestSuites/FileServer/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

dotnet publish "$TestSuiteRoot/TestSuites/FileServer/ShareUtil/ShareUtil.sln" -c $Configuration -o $OutDir/Utils
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build ShareUtil tool"
    exit 1
}

dotnet publish "$TestSuiteRoot/TestSuites/FileServer/src/FileServer.sln" -c $Configuration -o $OutDir/Bin
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build FileServer test suite"
    exit 1
}

Write-Host ==========================================================
Write-Host          Build FileServer test suite successfully         
Write-Host ==========================================================