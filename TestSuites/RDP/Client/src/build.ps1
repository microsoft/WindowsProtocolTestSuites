# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
) 

Write-Host =============================================
Write-Host     Start to build RDP Client Test Suite
Write-Host =============================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../../"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/TestSuites/RDP/Client"
}

$CommonScripts ="Disable_Firewall.ps1","Get-Parameter.ps1","Modify-ConfigFileNode.ps1","Set-Parameter.ps1",
"TurnOff-FileReadonly.ps1","Enable-WinRM.ps1"


if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

$PluginDir = "$OutDir/Plugin"
New-Item -ItemType Directory $PluginDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/RDP/Client/src/Plugin/RDPClientPlugin/*.xml" -Destination $PluginDir -Recurse -Force

$TargetDir = "$PluginDir/doc"
New-Item -ItemType Directory $TargetDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/RDP/Client/src/Plugin/RDPClientPlugin/Docs/*" -Destination $TargetDir -Recurse -Force

$TargetDir = "$PluginDir/SUTControlAdapter"
New-Item -ItemType Directory $TargetDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/RDP/Client/src/Plugin/RDPClientPlugin/SUTControlAdapter/*" -Destination $TargetDir -Recurse -Force

$TargetDir = "$PluginDir/ShellSUTControlAdapter"
New-Item -ItemType Directory $TargetDir -Force
Copy-Item "$TestSuiteRoot/TestSuites/RDP/Client/src/Plugin/RDPClientPlugin/ShellSUTControlAdapter/*" -Destination $TargetDir -Recurse -Force

New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/src/Batch/*" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.*" -Destination "$OutDir/Batch/" -Recurse -Force


New-Item -ItemType Directory $OutDir/Data -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/Setup/Data/*" -Destination "$OutDir/Data/" -Recurse -Force    


New-Item -ItemType Directory $OutDir/Scripts -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/Setup/Scripts/*" -Destination "$OutDir/Scripts/" -Recurse -Force
foreach ($curr in $CommonScripts) { 
    Copy-Item  "$TestSuiteRoot/CommonScripts/$curr" -Destination "$OutDir/Scripts/" -Recurse -Force
}

New-Item -ItemType Directory $OutDir/TestData -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/TestData/*.bmp" -Destination "$OutDir/TestData/" -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/TestData/*.xml" -Destination "$OutDir/TestData/" -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/RDPEDISP/RdpedispEnhancedAdapterImages/*.png" -Destination "$OutDir/TestData/" -Recurse -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/RDPEGFX/H264TestData/*.*" -Destination "$OutDir/TestData/" -Force
Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/RDPEGFX/H264TestData/BaseImage/*" -Destination "$OutDir/TestData/" -Recurse -Force    


Copy-Item  "$TestSuiteRoot/TestSuites/RDP/Client/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

dotnet publish "$TestSuiteRoot/TestSuites/RDP/Client/src/RDP_Client.sln" -c $Configuration --property:PublishDir=$OutDir/Bin

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build RDP Client test suite"
    exit 1
}

Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$OutDir/Bin/" -Force

Write-Host ==============================================
Write-Host    Build RDP Client test suite successfully
Write-Host ==============================================

