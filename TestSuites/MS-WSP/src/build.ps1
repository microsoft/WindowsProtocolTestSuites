# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration = "Release",
    [string]$OutDir
) 

Write-Host ======================================
Write-Host          Start to build MS-WSP
Write-Host ======================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/TestSuites/MS-WSP"
}

$CommonScripts = @()

if (Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

$PluginDir = "$OutDir/Plugin"
New-Item -ItemType Directory $PluginDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-WSP/src/Plugin/WSPServerPlugin/*.xml" -Destination $PluginDir -Recurse -Force

$TargetDir = "$PluginDir/doc"
New-Item -ItemType Directory $TargetDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-WSP/src/Plugin/WSPServerPlugin/Docs/*" -Destination $TargetDir -Recurse -Force

New-Item -ItemType Directory "$OutDir/Batch" -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-WSP/src/Batch/*.sh" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/TestSuites/MS-WSP/src/Batch/*.ps1" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.*" -Destination "$OutDir/Batch/" -Recurse -Force

New-Item -ItemType Directory "$OutDir/Scripts" -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-WSP/Setup/Scripts/*" -Destination "$OutDir/Scripts/" -Recurse -Force
foreach ($curr in $CommonScripts) { 
    Copy-Item  "$TestSuiteRoot/CommonScripts/$curr" -Destination "$OutDir/Scripts/" -Recurse -Force
}

New-Item -ItemType Directory "$OutDir/Data" -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-WSP/Setup/Data/*" -Destination "$OutDir/Data" -Recurse -Force

Copy-Item  "$TestSuiteRoot/TestSuites/MS-WSP/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

dotnet publish "$TestSuiteRoot/TestSuites/MS-WSP/src/MS-WSP_Server.sln" -c $Configuration --property:PublishDir=$OutDir/Bin
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build MS-WSP test suite"
    exit 1
}

Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$OutDir/Bin/" -Force

Write-Host ==========================================================
Write-Host          Build MS-WSP test suite successfully         
Write-Host ==========================================================