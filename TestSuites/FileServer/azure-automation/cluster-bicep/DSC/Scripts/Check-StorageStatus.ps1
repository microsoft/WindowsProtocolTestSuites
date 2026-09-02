# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [string]$ConfigureFile = (Join-Path (Split-Path $PSScriptRoot -Parent) '..\Config.json'),
    [switch]$NoTranscript
)

$ErrorActionPreference = 'Stop'
$transcriptStarted = $false
if (-not $NoTranscript) {
    Start-Transcript -Path "$PSCommandPath.log" -Append -Force | Out-Null
    $transcriptStarted = $true
}

try {
    $output = @(& (Join-Path $PSScriptRoot 'Test-StorageReadiness.ps1') `
        -ConfigureFile $ConfigureFile -Detailed)
    $output | ForEach-Object { Write-Output $_ }
    if ($output.Count -eq 0 -or $output[-1] -ne $true) {
        throw 'Storage readiness validation failed.'
    }
    return $true
}
finally {
    if ($transcriptStarted) {
        Stop-Transcript | Out-Null
    }
}
