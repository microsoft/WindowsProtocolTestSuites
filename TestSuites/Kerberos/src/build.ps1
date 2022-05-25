# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
)

Write-Host =============================================
Write-Host     Start to build Kerberos Test Suite
Write-Host =============================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../"

if ([string]::IsNullOrEmpty($OutDir)) {
    $OutDir = "$TestSuiteRoot/drop/TestSuites/Kerberos"
}

$CommonScripts ="Change-DomainUserPassword.ps1","Check-ReturnValue.ps1","Disable_Firewall.ps1","Get-DomainControllerParameters.ps1","Get-OSVersionNumber.ps1","Get-Parameter.ps1","GetVMNameByComputerName.ps1","GetVmParameters.ps1","Install-ADDS.ps1","Install-FSRM.ps1","Install-IIS.ps1","Join-Domain.ps1","PromoteDomainController.ps1","RestartAndRun.bat","RestartAndRun.ps1","RestartAndRunFinish.ps1","Set-AutoLogon.ps1","Set-ExecutionPolicy-Unrestricted.ps1","Set-NetworkConfiguration.ps1","Set-Parameter.ps1","TurnOff-FileReadonly.ps1","Write-Info.ps1"

if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item "$TestSuiteRoot/TestSuites/Kerberos/src/Batch/*.sh" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item  "$TestSuiteRoot/TestSuites/Kerberos/src/Batch/*" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.*" -Destination "$OutDir/Batch/" -Recurse -Force

New-Item -ItemType Directory $OutDir/Scripts -Force
Copy-Item  "$TestSuiteRoot/TestSuites/Kerberos/Setup/Scripts/*" -Destination "$OutDir/Scripts/" -Recurse -Force
foreach ($curr in $CommonScripts) {
    Copy-Item  "$TestSuiteRoot/CommonScripts/$curr" -Destination "$OutDir/Scripts/" -Recurse -Force
}

Copy-Item  "$TestSuiteRoot/TestSuites/Kerberos/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force
dotnet publish "$TestSuiteRoot/TestSuites/Kerberos/src/Kerberos_Server.sln" -c $Configuration -p:Platform=x86 -o $OutDir/Bin

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build Kerberos test suite"
    exit 1
}

Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$OutDir/Bin/" -Force

Write-Host ==============================================
Write-Host    Build Kerberos test suite successfully
Write-Host ==============================================