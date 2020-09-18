# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    [string]$Filter = "",    # Expression used to filter test cases.For example, "TestCategory=BVT" will filter out test cases which have test category BVT. 
    [switch]$DryRun = $false # If set, just list all test cases instead of running tests actually.
)

$invocationPath =  Split-Path $MyInvocation.MyCommand.Definition -Parent

$script = Join-Path $invocationPath "RunTestCasesByBinariesAndFilter.ps1"

$binaries = @(
    "RDP_ClientTestSuite.dll"
)

$cmd = "$script -Binaries `$binaries -Filter `$Filter -DryRun:`$(`$DryRun.IsPresent)"

Invoke-Expression $cmd
