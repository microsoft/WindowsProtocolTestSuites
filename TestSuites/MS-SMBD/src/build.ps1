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
$buildToolPath = &"$TestSuiteRoot/common/setBuildTool.ps1"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/TestSuites/MS-SMBD"
}

if (!(Test-Path -Path $TestSuiteRoot/ProtoSDK/RDMA/include/ndspi.h)) {
    Write-Host "WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndspi.h does not exist, it can be extracted from NetworkDirect_DDK.zip in HPC Pack 2008 R2 SDK with Service Pack 2  @ https://www.microsoft.com/en-us/download/details.aspx?id=26645"
    exit 1
}
if (!(Test-Path -Path $TestSuiteRoot/ProtoSDK/RDMA/include/ndstatus.h)) {
    Write-Host "WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndstatus.h does not exist, it can be extracted from NetworkDirect_DDK.zip in HPC Pack 2008 R2 SDK with Service Pack 2  @ https://www.microsoft.com/en-us/download/details.aspx?id=26645"
    exit 1
}

if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}

New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-SMBD/src/Batch/*" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item  "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.ps1" -Destination "$OutDir/Batch/" -Recurse -Force

Copy-Item  "$TestSuiteRoot/TestSuites/MS-SMBD/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

cmd /c "`"$buildToolPath`" `"$TestSuiteRoot/TestSuites/MS-SMBD/src/MS-SMBD_Server.sln`" /t:Clean;Rebuild /p:OutDir=`"$OutDir/Bin`" /p:Configuration=`"$Configuration`""

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build MS-SMBD test suite"
    exit 1
}

Write-Host ==========================================================
Write-Host          Build MS-SMBD test suite successfully            
Write-Host ==========================================================