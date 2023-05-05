# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
) 

Write-Host ==========================================
Write-Host     Start to build BranchCache test suite    
Write-Host ==========================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../"
$buildToolPath = &"$TestSuiteRoot/common/setBuildTool.ps1"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/TestSuites/BranchCache"
}

$CommonScripts ="domainjoin.PS1","Check-ReturnValue.ps1","Disable_Firewall.ps1","GetVMNameByComputerName.ps1",
"GetVmParameters.ps1","PromoteDomainController.ps1","PromoteRODC.ps1","RestartAndRun.ps1","RestartAndRunFinish.ps1",
"Set-AutoLogon.ps1","Set-ExecutionPolicy-Unrestricted.ps1","Set-NetworkConfiguration.ps1","Turnoff-UAC.ps1",
"WaitFor-ComputerReady.ps1","Write-Info.ps1"

if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item "$TestSuiteRoot/TestSuites/BranchCache/src/Batch/*.sh" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/TestSuites/BranchCache/src/Batch/*.ps1" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.*" -Destination "$OutDir/Batch/" -Recurse -Force

New-Item -ItemType Directory $OutDir/Scripts -Force
Copy-Item  "$TestSuiteRoot/TestSuites/BranchCache/Scripts/*" -Destination "$OutDir/Scripts/" -Recurse -Force
foreach ($curr in $CommonScripts) { 
    Copy-Item  "$TestSuiteRoot/CommonScripts/$curr" -Destination "$OutDir/Scripts/" -Recurse -Force
}

Copy-Item  "$TestSuiteRoot/TestSuites/BranchCache/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

cmd /c "`"$buildToolPath`" `"$TestSuiteRoot/TestSuites/BranchCache/src/BranchCache.sln`" /t:Clean;Rebuild /p:OutDir=`"$OutDir/Bin`" /p:Configuration=`"$Configuration`""
#dotnet publish "$TestSuiteRoot/TestSuites/BranchCache/src/BranchCache.sln" c $Configuration -o $OutDir/Bin
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build BranchCache test suite"
    exit 1
}


Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$OutDir/Bin/" -Force

Write-Host ==========================================================
Write-Host          Build BranchCache test suite successfully         
Write-Host ==========================================================