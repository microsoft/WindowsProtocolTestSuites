# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet('Detach', 'Restore')]
    [string]$Mode,

    [string]$StateFilePath = 'C:\Windows\Temp\WptsFsaMountPoints.json'
)

$ErrorActionPreference = 'Stop'
$candidatePaths = @(
    'C:\FileShare\MountPoint',
    'K:\SMBReFSShare\MountPoint'
)

if ($Mode -eq 'Detach') {
    $mappings = @()
    if (Test-Path -LiteralPath $StateFilePath) {
        $mappings = @(Get-Content -LiteralPath $StateFilePath -Raw | ConvertFrom-Json)
    }
    foreach ($path in $candidatePaths) {
        $item = Get-Item -LiteralPath $path -Force -ErrorAction SilentlyContinue
        $isMountPoint = $item -and
            ($item.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0
        if (-not $isMountPoint) { continue }

        $volume = (& mountvol.exe $path /L 2>&1 | Out-String).Trim()
        if ($LASTEXITCODE -ne 0 -or -not $volume.StartsWith('\\?\Volume{') -or
            -not $volume.EndsWith('}\')) {
            throw "Could not resolve mount target for '$path'."
        }
        if (-not @($mappings | Where-Object Path -eq $path)) {
            $mappings += [pscustomobject]@{ Path = $path; Volume = $volume }
        }
        $stateDirectory = Split-Path -Parent $StateFilePath
        New-Item -ItemType Directory -Path $stateDirectory -Force | Out-Null
        $mappings | ConvertTo-Json | Set-Content -LiteralPath $StateFilePath -Encoding UTF8
        & mountvol.exe $path /D 2>&1 | Out-Null
        if ($LASTEXITCODE -ne 0) { throw "Could not detach '$path'." }
        Write-Output "DETACHED|$path"
    }
    return
}

if (-not (Test-Path -LiteralPath $StateFilePath)) { return }
$mappings = @(Get-Content -LiteralPath $StateFilePath -Raw | ConvertFrom-Json)
foreach ($mapping in $mappings) {
    $item = Get-Item -LiteralPath $mapping.Path -Force -ErrorAction SilentlyContinue
    $isMountPoint = $item -and
        ($item.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0
    if (-not $isMountPoint) {
        if (-not (Test-Path -LiteralPath $mapping.Path)) {
            New-Item -ItemType Directory -Path $mapping.Path -Force | Out-Null
        }
        if (-not ([string]$mapping.Volume).StartsWith('\\?\Volume{') -or
            -not ([string]$mapping.Volume).EndsWith('}\')) {
            throw "The persisted volume for '$($mapping.Path)' is invalid."
        }
        & mountvol.exe $mapping.Path $mapping.Volume 2>&1 | Out-Null
        if ($LASTEXITCODE -ne 0) { throw "Could not restore '$($mapping.Path)'." }
    }
    $item = Get-Item -LiteralPath $mapping.Path -Force -ErrorAction Stop
    if (($item.Attributes -band [IO.FileAttributes]::ReparsePoint) -eq 0) {
        throw "Restored path '$($mapping.Path)' is not a mount point."
    }
    Write-Output "MOUNT|$($mapping.Path)|Verified"
}
Remove-Item -LiteralPath $StateFilePath -Force