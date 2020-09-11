# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
    [string]$Configuration="Release",
    [string]$OutDir
)

Write-Host ======================================
Write-Host          Start to build PtmCli
Write-Host ======================================

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$OutDir
dotnet publish "$InvocationPath/PtmCli.sln" -c $Configuration -o $OutDir
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build PTMCli"
    exit 1
}

Write-Host ==========================================================
Write-Host          Build PtmCli successfully         
Write-Host ==========================================================