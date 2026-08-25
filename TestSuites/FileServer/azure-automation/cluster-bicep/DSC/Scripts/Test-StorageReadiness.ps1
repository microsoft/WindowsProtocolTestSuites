# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [string]$ConfigureFile = (Join-Path (Split-Path $PSScriptRoot -Parent) '..\Config.json'),
    [switch]$Detailed
)

$ErrorActionPreference = 'Stop'
$failures = New-Object System.Collections.Generic.List[string]

function Add-ReadinessFailure {
    param([string]$Message)
    $failures.Add($Message)
    if ($Detailed) { Write-Warning $Message }
}

try {
    $config = Get-Content -LiteralPath $ConfigureFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
}
catch {
    Add-ReadinessFailure "Config.json could not be loaded: $($_.Exception.Message)"
    return $false
}

$storage = $config.Machines.Storage
$targetName = if ($storage.iSCSITargetName) {
    "$($storage.iSCSITargetName)"
} else {
    'ClusterTarget'
}
$diskSpecs = @(
    @{ Name = 'disk1.vhdx'; SizeBytes = [long](10GB) },
    @{ Name = 'disk2.vhdx'; SizeBytes = [long](10GB) },
    @{ Name = 'disk3.vhdx'; SizeBytes = [long](10GB) },
    @{ Name = 'diskq.vhdx'; SizeBytes = [long](1GB) }
)

$target = Get-IscsiServerTarget -TargetName $targetName -ErrorAction SilentlyContinue
if ($null -eq $target) {
    Add-ReadinessFailure "iSCSI target '$targetName' does not exist."
}
else {
    $mappingPaths = @($target.LunMappings | ForEach-Object { "$($_.Path)" })
    if ($mappingPaths.Count -ne $diskSpecs.Count) {
        Add-ReadinessFailure (
            "Target '$targetName' has $($mappingPaths.Count) mappings; expected $($diskSpecs.Count)."
        )
    }

    foreach ($spec in $diskSpecs) {
        $path = @($mappingPaths | Where-Object {
            [System.IO.Path]::GetFileName($_) -ieq $spec.Name
        } | Select-Object -First 1)
        if ($path.Count -ne 1) {
            Add-ReadinessFailure "Target '$targetName' is missing mapping '$($spec.Name)'."
            continue
        }

        $virtualDisk = Get-IscsiVirtualDisk -Path $path[0] -ErrorAction SilentlyContinue
        if ($null -eq $virtualDisk) {
            Add-ReadinessFailure "Mapped virtual disk '$($path[0])' is not registered."
            continue
        }
        if ([long]$virtualDisk.Size -ne [long]$spec.SizeBytes) {
            Add-ReadinessFailure (
                "Virtual disk '$($spec.Name)' has size $($virtualDisk.Size); expected $($spec.SizeBytes)."
            )
        }
        if (-not (Test-Path -LiteralPath $path[0] -PathType Leaf)) {
            Add-ReadinessFailure "Virtual disk file '$($path[0])' is missing."
        }
    }

    $configuredIps = @(
        $config.Machines.Node01.IpConfig[0].Ip,
        $config.Machines.Node02.IpConfig[0].Ip
    ) | Where-Object { -not [string]::IsNullOrWhiteSpace("$_") }
    $configuredInitiators = @($configuredIps | ForEach-Object { "IPAddress:$_" })
    $actualInitiators = @($target.InitiatorIds | ForEach-Object { "$_" })
    if ($actualInitiators -notcontains 'IQN:*') {
        foreach ($initiator in $configuredInitiators) {
            if ($actualInitiators -notcontains $initiator) {
                Add-ReadinessFailure "Target '$targetName' is missing initiator '$initiator'."
            }
        }
    }
}

$service = Get-CimInstance Win32_Service -Filter "Name='WinTarget'" -ErrorAction SilentlyContinue
if ($null -eq $service) {
    Add-ReadinessFailure 'WinTarget service is missing.'
}
else {
    if ($service.StartMode -ne 'Auto') {
        Add-ReadinessFailure "WinTarget start mode is '$($service.StartMode)', expected Auto."
    }
    if ($service.State -ne 'Running') {
        Add-ReadinessFailure "WinTarget state is '$($service.State)', expected Running."
    }
}

$listener = Get-NetTCPConnection -LocalPort 3260 -State Listen -ErrorAction SilentlyContinue
if ($null -eq $listener) {
    Add-ReadinessFailure 'No iSCSI listener is active on TCP port 3260.'
}

if ($failures.Count -gt 0) {
    if ($Detailed) {
        Write-Warning "Storage readiness failed with $($failures.Count) issue(s)."
    }
    return $false
}

if ($Detailed) {
    Write-Output "Storage target '$targetName' is ready with four verified LUN mappings."
}
return $true
