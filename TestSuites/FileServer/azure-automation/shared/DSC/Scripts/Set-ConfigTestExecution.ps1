# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [object]$EnableTestAutoRun = $true,

    [Parameter(Mandatory = $false)]
    [string[]]$ConfigPaths
)

$ErrorActionPreference = 'Stop'
$autoRun = if ($EnableTestAutoRun -is [bool]) {
    $EnableTestAutoRun
} else {
    [Convert]::ToBoolean("$EnableTestAutoRun")
}

if (-not $ConfigPaths -or $ConfigPaths.Count -eq 0) {
    $root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
    $ConfigPaths = @(
        (Join-Path $root 'Config.json')
        (Join-Path $PSScriptRoot 'Config.json')
    )
}

$patched = 0
foreach ($path in $ConfigPaths) {
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        Write-Output "Set-ConfigTestExecution: skipping (not found) $path"
        continue
    }

    $config = Get-Content -LiteralPath $path -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    if ($null -eq $config.TestExecution) {
        $config | Add-Member -MemberType NoteProperty -Name TestExecution -Value (
            [pscustomobject]@{ AutoRun = $autoRun }
        )
    } elseif ($null -eq $config.TestExecution.PSObject.Properties['AutoRun']) {
        $config.TestExecution |
            Add-Member -MemberType NoteProperty -Name AutoRun -Value $autoRun
    } else {
        $config.TestExecution.AutoRun = $autoRun
    }

    $config | ConvertTo-Json -Depth 20 |
        Set-Content -LiteralPath $path -Encoding UTF8 -NoNewline -Force
    $patched++
    Write-Output "Set-ConfigTestExecution: AutoRun=$autoRun in $path"
}

Write-Output "Set-ConfigTestExecution: patched $patched file(s)."
