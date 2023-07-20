# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
) 

Write-Host =============================================
Write-Host     Start to build MS-XCA Test Suite
Write-Host =============================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$TestSuiteRoot = "$InvocationPath/../../../"

if ([string]::IsNullOrEmpty($OutDir)) { 
    $OutDir = "$TestSuiteRoot/drop/TestSuites/MS-XCA"
}

if(Test-Path -Path $OutDir) {
    Get-ChildItem $OutDir -Recurse | Remove-Item -Recurse -Force
}


New-Item -ItemType Directory $OutDir/Batch -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-XCA/src/Batch/*" -Destination "$OutDir/Batch/" -Recurse -Force
Copy-Item "$TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.*" -Destination "$OutDir/Batch/" -Recurse -Force


New-Item -ItemType Directory $OutDir/StaticData -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-XCA/src/TestSuite/StaticData/*.*" -Destination "$OutDir/StaticData/" -Force

New-Item -ItemType Directory $OutDir/UserData -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-XCA/src/TestSuite/UserData/*.*" -Destination "$OutDir/UserData/" -Force

New-Item -ItemType Directory $OutDir/UserData/Compression -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-XCA/src/TestSuite/UserData/Compression/*.*" -Destination "$OutDir/UserData/Compression/" -Force

New-Item -ItemType Directory $OutDir/UserData/Decompression -Force

New-Item -ItemType Directory $OutDir/UserData/Decompression/LZ77 -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-XCA/src/TestSuite/UserData/Decompression/LZ77/*.*" -Destination "$OutDir/UserData/Decompression/LZ77/" -Force

New-Item -ItemType Directory $OutDir/UserData/Decompression/LZ77Huffman -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-XCA/src/TestSuite/UserData/Decompression/LZ77Huffman/*.*" -Destination "$OutDir/UserData/Decompression/LZ77Huffman/" -Force

New-Item -ItemType Directory $OutDir/UserData/Decompression/LZNT1 -Force
Copy-Item  "$TestSuiteRoot/TestSuites/MS-XCA/src/TestSuite/UserData/Decompression/LZNT1/*.*" -Destination "$OutDir/UserData/Decompression/LZNT1/" -Force


Copy-Item  "$TestSuiteRoot/TestSuites/MS-XCA/src/Deploy/LICENSE.rtf" -Destination "$OutDir/LICENSE.rtf" -Recurse -Force

dotnet publish "$TestSuiteRoot/TestSuites/MS-XCA/XcaTestApp/XcaTestApp.sln" -c $Configuration --property:PublishDir=$OutDir/Utils/XcaTestApp
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build XcaTestApp tool"
    exit 1
}

dotnet publish "$TestSuiteRoot/TestSuites/MS-XCA/src/MS-XCA.sln" -c $Configuration --property:PublishDir=$OutDir/Bin --property:IsBuildScript=Yes

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build MS-XCA test suite"
    exit 1
}

Copy-Item "$TestSuiteRoot/AssemblyInfo/.version" -Destination "$OutDir/Bin/" -Force

Write-Host ==============================================
Write-Host    Built MS-XCA test suite successfully
Write-Host ==============================================
