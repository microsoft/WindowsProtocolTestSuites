# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
)

Write-Host ==========================================
Write-Host     Start to build MS-SMBD test suite
Write-Host ==========================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../"

if ([string]::IsNullOrEmpty($OutDir)) {
    $OutDir = "$TestSuiteRoot/drop/TestSuites/MS-SMBD"
}

$OutDir = [System.IO.Path]::GetFullPath($OutDir)
$BinDir = Join-Path $OutDir "Bin"

$vsPath = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
if (-not $vsPath) {
    Write-Host "Error: Could not find Visual Studio installation." -ForegroundColor Red
    exit 1
}

$msbuildPath = Join-Path $vsPath "MSBuild\Current\Bin\MSBuild.exe"
if (-not (Test-Path $msbuildPath)) {
    $msbuildPath = Join-Path $vsPath "MSBuild\15.0\Bin\MSBuild.exe"
}

Write-Host "Using MSBuild from: $msbuildPath"
# -----------------------------

if (!(Test-Path -Path $TestSuiteRoot/ProtoSDK/RDMA/include/ndspi.h)) {
    Write-Host "WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndspi.h does not exist..."
    exit 1
}

if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

$PluginDir = "$OutDir/Plugin"
New-Item -ItemType Directory $PluginDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-SMBD/src/Plugin/SMBDPlugin/*.xml" -Destination $PluginDir -Recurse -Force
$TargetDir = "$PluginDir/doc"; New-Item -ItemType Directory $TargetDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-SMBD/src/Plugin/SMBDPlugin/Docs/*" -Destination $TargetDir -Recurse -Force
$ScriptDir = "$PluginDir/script"; New-Item -ItemType Directory $ScriptDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-SMBD/src/Plugin/SMBDPlugin/Detector/*.ps1" -Destination $ScriptDir -Recurse -Force
New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-SMBD/src/Batch/*" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.ps1" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-SMBD/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

Write-Host "Building solution using MSBuild..."
$SolutionPath = "$TestSuiteRoot/TestSuites/MS-SMBD/src/MS-SMBD_Server.sln"

& $msbuildPath $SolutionPath `
    /p:Configuration=$Configuration `
    /p:Platform="Any CPU" `
    /p:OutDir="$BinDir\" `
    /m /t:Restore,Build 

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build MS-SMBD test suite using MSBuild" -ForegroundColor Red
    exit 1
}

Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$BinDir/" -Force

Write-Host ==========================================================
Write-Host          Build MS-SMBD test suite successfully
Write-Host ==========================================================