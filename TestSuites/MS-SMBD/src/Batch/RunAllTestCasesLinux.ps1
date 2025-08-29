# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    [switch]$DryRun = $false # If set, just list all test cases instead of running tests actually.
)

$invocationPath = "/mnt/server-endpoint/Batch".ToLower()

Write-Host "Running all test cases in MS-SMBD test suite..."
Write-Host "Path: $invocationPath"

$script = Join-Path $invocationPath "RunTestCasesByFilterLinux.ps1".ToLower()

$cmd = "$script -DryRun:`$(`$DryRun.IsPresent)"

Invoke-Expression $cmd
