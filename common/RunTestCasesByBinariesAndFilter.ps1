# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    [array]$Binaries,        # An array containing name of the test binaries.
    [string]$Filter = "",    # Expression used to filter test cases.For example, "TestCategory=BVT&TestCategory=SMB311" will filter out test cases which have test category BVT and SMB311. 
    [switch]$DryRun = $false # If set, just list all filtered test cases instead of running tests actually.
)

$invocationPath =  Split-Path $MyInvocation.MyCommand.Definition -Parent

$rootPath = Split-Path $invocationPath -Parent

$binPath = Join-Path $rootPath "Bin"

$testResultPath = Join-Path $rootPath "TestResults"

$testBinariesWithPath = $Binaries | ForEach-Object -Process { Join-Path $binPath $_ }
    
$testBinariesString = [String]::Join(" ", $testBinariesWithPath)

if([String]::IsNullOrEmpty($Filter))
{
    $TestCaseFilter = ""
}
else
{
    $TestCaseFilter = "--filter ""$Filter"""
}

if($DryRun)
{
    $cmd = "dotnet test $testBinariesString $TestCaseFilter --list-tests"    
}
else
{
    $cmd = "dotnet test $testBinariesString $TestCaseFilter --logger trx --ResultsDirectory ""$testResultPath"""
}

Invoke-Expression $cmd
