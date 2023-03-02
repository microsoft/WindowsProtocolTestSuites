# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
) 

Write-Host =============================================
Write-Host     Start to build RDP Server Test Suite
Write-Host =============================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../../"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/TestSuites/RDP/Server"
}

$CommonScripts ="Disable_Firewall.ps1","Enable-WinRM.ps1","Set-AutoLogon.ps1","RestartAndRunFinish.ps1","RestartAndRun.ps1","TurnOff-FileReadonly.ps1"

if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

$PluginDir = "$OutDir/Plugin"
New-Item -ItemType Directory $PluginDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/RDP/Server/src/Plugin/RDPServerPlugin/*.xml" -Destination $PluginDir -Recurse -Force

$TargetDir = "$PluginDir/doc"
New-Item -ItemType Directory $TargetDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/RDP/Server/src/Plugin/RDPServerPlugin/Docs/*" -Destination $TargetDir -Recurse -Force

New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Server/src/Batch/*" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.*" -Destination "$OutDir/Batch/" -Recurse -Force

New-Item -ItemType Directory $OutDir/Scripts -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Server/Setup/Scripts/*" -Destination "$OutDir/Scripts/" -Recurse -Force
foreach ($curr in $CommonScripts) { 
    Copy-Item  "$TestSuiteRoot/CommonScripts/$curr" -Destination "$OutDir/Scripts/" -Recurse -Force
}

Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Server/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

dotnet publish "$TestSuiteRoot/TestSuites/RDP/Server/src/RDP_Server.sln" -c $Configuration --property:PublishDir=$OutDir/Bin

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build RDP Server test suite"
    exit 1
}

Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$OutDir/Bin/" -Force

Write-Host ==============================================
Write-Host    Build RDP Server test suite successfully
Write-Host ==============================================