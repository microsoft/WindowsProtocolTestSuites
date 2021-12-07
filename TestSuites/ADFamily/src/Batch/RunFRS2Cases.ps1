# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    [switch]$DryRun = $false # If set, just list all test cases instead of running tests actually.
)

$invocationPath =  Split-Path $MyInvocation.MyCommand.Definition -Parent

$script = Join-Path $invocationPath "RunTestCasesByFilter.ps1"

$Filter="TestCategory=MS-FRS2"

$cmd = "$script -Filter `$Filter -DryRun:`$(`$DryRun.IsPresent)"

Invoke-Expression $cmd