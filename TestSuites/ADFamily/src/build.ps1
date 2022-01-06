# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
) 

Write-Host =============================================
Write-Host     Start to build ADFamily Test Suite
Write-Host =============================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../"
$buildToolPath = &"$TestSuiteRoot/common/setBuildTool.ps1"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/TestSuites/ADFamily"
}

$CommonScripts ="Add-AdRemoteUserToLocalGroup.ps1","Check-ReturnValue.ps1","Create-AdUserWithGroupMembership.ps1",
                "Create-ManagedServiceAccount.ps1","Create-SelfSignedCert.ps1","Disable_Firewall.ps1","Enable-AdRecyleBin.ps1",
                "Execute-RemoteScript.ps1","Find-PtfConfigFiles.ps1","Get-AdLdsInstanceId.ps1","Get-AvailablePort.ps1",
                "Get-DomainGuid.ps1","Get-DomainNetbiosName.ps1","Get-DomainSid.ps1","Get-ForestFuncLvl.ps1","Get-OSVersion.ps1",
                "GetVMNameByComputerName.ps1","Install-AdCertificateService-Feature.ps1","Install-AdLds-Feature.ps1",
                "Install-AdLds-Instance.ps1","Install-DfsManagement-Feature.ps1","Install-Iis-Feature.ps1","Install-SelfSignedCert.ps1",
                "Modify-PtfConfigFiles.ps1","Register-DbgSrv.ps1","Replicate-DomainNc.ps1","Set-AdComputerPassword.ps1",
                "Set-AdComputerPasswordOnPDC.ps1","Set-AdUserPwdPolicy.ps1","Set-AlternateDns.ps1","Set-AutoLogon.ps1",
                "Set-ExecutionPolicy-Unrestricted.ps1","Set-IisSslBinding.ps1","Set-KdcService.ps1","Set-MsDSAdditonalDnsHostName.ps1",
                "Set-MsDsAllowToActOnBehalfOfOtherIdentity.ps1","Set-MsDsBehaviorVersion.ps1","Set-MsDsOtherSettings.ps1",
                "Set-NetlogonRegKeyAndPolicy.ps1","Set-NetworkConfiguration.ps1","Set-SecurityLevel.ps1","Turnoff-UAC.ps1",
                "Verify-ForestTrust.ps1"

if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item "$TestSuiteRoot/TestSuites/ADFamily/src/Batch/*.sh" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item  "$TestSuiteRoot/TestSuites/ADFamily/src/Batch/*" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.*" -Destination "$OutDir/Batch/" -Recurse -Force

New-Item -ItemType Directory $OutDir/Scripts -Force
Copy-Item  "$TestSuiteRoot/TestSuites/ADFamily/Setup/Scripts/*" -Destination "$OutDir/Scripts/" -Recurse -Force
foreach ($curr in $CommonScripts) { 
    Copy-Item  "$TestSuiteRoot/CommonScripts/$curr" -Destination "$OutDir/Scripts/" -Recurse -Force
}

New-Item -ItemType Directory $OutDir/Data -Force
Copy-Item  "$TestSuiteRoot/TestSuites/ADFamily/Setup/Data/*" -Destination "$OutDir/Data/" -Recurse -Force

New-Item -ItemType Directory $OutDir/PlayLists -Force
Copy-Item  "$TestSuiteRoot/TestSuites/ADFamily/src/PlayLists/*" -Destination "$OutDir/PlayLists/" -Recurse -Force

Copy-Item  "$TestSuiteRoot/TestSuites/ADFamily/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

Write-Host $buildToolPath
Write-Host $TestSuiteRoot

cmd /c "`"$buildToolPath`" `"$TestSuiteRoot/TestSuites/ADFamily/src/AD_Server.sln`" /t:Clean;Rebuild /p:OutDir=`"$OutDir/Bin`" /p:Configuration=`"$Configuration`""


if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build ADFamily test suite"
    exit 1
}

Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$OutDir/Bin/" -Force

Write-Host ==============================================
Write-Host    Build ADFamily test suite successfully
Write-Host ==============================================