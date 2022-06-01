# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Write-Host "Get the path to MSBuild.exe"
$vswherePath = foreach ($programPath in @($env:ProgramFiles, ${env:ProgramFiles(x86)})) {
    $pathToTest = "$programPath\Microsoft Visual Studio\Installer\vswhere.exe"
    if (Test-Path $pathToTest) {
       $pathToTest
    }
}

if($vswherePath -eq $null) {
    Write-Host "please make sure you have installed Visual Studio 2022 or later"
    exit 1
}

$vsoutput = & $vswherePath -format json
$jsonoutput = $vsoutput | ConvertFrom-Json

$vs2022Path= $jsonoutput| Where-Object installationVersion -like '17.*'
if($vs2022Path -eq $null) {
    Write-Host "please make sure you have installed Visual Studio 2022 or later"
    exit 1
}

$vsPath = $vs2022Path.installationPath
if(!(Test-Path "$vsPath\MSBuild"))
{
    Write-Host "could not find MSBuild.exe, please make sure you have installed `"C# and Visual Basic Roslyn compilers`" in Visual Studio 2022 or later."
    exit 1
}

$buildToolPath = Get-ChildItem "$vsPath\MSBuild\*" | ForEach-Object {
    if (Test-Path "$($_.FullName)\Bin\MSBuild.exe") {
        return "$($_.FullName)\Bin\MSBuild.exe"
    }
}

if($buildToolPath -eq $null) {
    Write-Host "could not find MSBuild.exe, please make sure you have installed `"C# and Visual Basic Roslyn compilers`" in Visual Studio 2022 or later."
    exit 1
}
return $buildToolPath