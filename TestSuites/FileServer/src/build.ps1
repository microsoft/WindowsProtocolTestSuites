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

$PluginDir = "$OutDir/Plugin"
New-Item -ItemType Directory $PluginDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/FileServer/src/Plugin/FileServerPlugin/*.xml" -Destination $PluginDir -Recurse -Force

$TargetDir = "$PluginDir/doc"
New-Item -ItemType Directory $TargetDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/FileServer/src/Plugin/FileServerPlugin/Docs/*" -Destination $TargetDir -Recurse -Force

$TargetDir = "$PluginDir/data"
New-Item -ItemType Directory $TargetDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/FileServer/src/Plugin/FileServerPlugin/Data/*" -Destination $TargetDir -Recurse -Force

New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item "$TestSuiteRoot/TestSuites/FileServer/src/Batch/*.sh" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/TestSuites/FileServer/src/Batch/*.ps1" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.*" -Destination "$OutDir/Batch/" -Recurse -Force

New-Item -ItemType Directory $OutDir/Scripts -Force
Copy-Item  "$TestSuiteRoot/TestSuites/FileServer/Setup/Scripts/*" -Destination "$OutDir/Scripts/" -Recurse -Force
foreach ($curr in $CommonScripts) { 
    Copy-Item  "$TestSuiteRoot/CommonScripts/$curr" -Destination "$OutDir/Scripts/" -Recurse -Force
}

New-Item -ItemType Directory $OutDir/Bin/Data -Force
Copy-Item  "$TestSuiteRoot/TestSuites/FileServer/src/Data/*" -Destination "$OutDir/Bin/Data" -Recurse -Force

Copy-Item  "$TestSuiteRoot/TestSuites/FileServer/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

dotnet publish "$TestSuiteRoot/TestSuites/FileServer/ShareUtil/ShareUtil.sln" -c $Configuration --property:PublishDir=$OutDir/Utils
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build ShareUtil tool"
    exit 1
}

dotnet publish "$TestSuiteRoot/TestSuites/FileServer/src/FileServer.sln" -c $Configuration --property:PublishDir=$OutDir/Bin
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build FileServer test suite"
    exit 1
}

Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$OutDir/Bin/" -Force

Write-Host ==========================================================
Write-Host          Build FileServer test suite successfully         
Write-Host ==========================================================