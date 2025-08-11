# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    [string]$Filter = "",    # Expression used to filter test cases.For example, "TestCategory=BVT&TestCategory=Smb2OverRdmaChannel" will filter out test cases which have test category BVT and Smb2OverRdmaChannel. 
    [switch]$DryRun = $false # If set, just list all test cases instead of running tests actually.
)

$invocationPath = "/mnt/server-endpoint/Batch".ToLower()

Write-Host "Running all test cases in MS-SMBD test suite...II"
Write-Host "Path: $invocationPath"

$script = Join-Path $invocationPath "RunTestCasesByBinariesAndFilterLinux.ps1".ToLower()

$binaries = @(
    "MS-SMBD_ServerTestSuite.dll"
)

$cmd = "$script -Binaries `$binaries -Filter `$Filter -DryRun:`$(`$DryRun.IsPresent)"

Invoke-Expression $cmd
