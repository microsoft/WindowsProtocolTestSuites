# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Creates or repairs the Cluster iSCSI target without replacing healthy state.
#>

[CmdletBinding()]
param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [string]$ConfigureFile = "$WorkingPath\Config.json",
    [string]$HeartbeatPath,
    [switch]$NoTranscript
)

$ErrorActionPreference = 'Stop'
$scriptsPath = Join-Path $PSScriptRoot 'Scripts'
$logFile = Join-Path $PSScriptRoot 'Invoke-StorageImperativeSteps.log'
$transcriptStarted = $false

Push-Location $scriptsPath
if (-not $NoTranscript) {
    Start-Transcript -Path $logFile -Append -Force | Out-Null
    $transcriptStarted = $true
}

function Stop-StorageTranscript {
    if ($transcriptStarted) {
        Stop-Transcript | Out-Null
    }
    Pop-Location
}

function Write-StorageHeartbeat {
    param([string]$Checkpoint, [datetime]$StartedAt)
    if (-not $HeartbeatPath) { return }
    Write-DeploymentHeartbeat -Phase 'StorageConvergence' `
        -Operation 'Create or repair iSCSI target' `
        -StartedAt $StartedAt -HeartbeatPath $HeartbeatPath `
        -LastCheckpoint $Checkpoint
}

function Get-StorageDataRoot {
    $mountPath = 'C:\StorageData'
    $mountAccessPath = "$mountPath\"
    $existingPartition = Get-Partition -ErrorAction SilentlyContinue |
        Where-Object { @($_.AccessPaths) -contains $mountAccessPath } |
        Select-Object -First 1

    if ($null -eq $existingPartition) {
        $dataDisk = Get-Disk -ErrorAction Stop |
            Where-Object {
                -not $_.IsBoot -and -not $_.IsSystem -and
                $_.Location -match 'LUN 0(?:\s*)$'
            } |
            Select-Object -First 1
        if ($null -eq $dataDisk) {
            $dataDisk = Get-Disk -ErrorAction Stop |
                Where-Object {
                    -not $_.IsBoot -and -not $_.IsSystem -and
                    [long]$_.Size -ge [long](60GB) -and
                    "$($_.BusType)" -in @('SAS', 'SCSI', 'RAID')
                } |
                Sort-Object Number |
                Select-Object -First 1
        }
        if ($null -eq $dataDisk) {
            throw 'The Storage data disk (LUN 0 / at least 60 GB) was not found.'
        }

        if ($dataDisk.IsOffline) {
            Set-Disk -Number $dataDisk.Number -IsOffline $false -ErrorAction Stop
        }
        if ($dataDisk.IsReadOnly) {
            Set-Disk -Number $dataDisk.Number -IsReadOnly $false -ErrorAction Stop
        }
        $dataDisk = Get-Disk -Number $dataDisk.Number -ErrorAction Stop

        if ($dataDisk.PartitionStyle -eq 'RAW') {
            Initialize-Disk -Number $dataDisk.Number -PartitionStyle GPT -ErrorAction Stop
        }
        elseif ($dataDisk.PartitionStyle -ne 'GPT') {
            throw "Storage data disk $($dataDisk.Number) uses unsupported partition style '$($dataDisk.PartitionStyle)'."
        }

        $usablePartitions = @(Get-Partition -DiskNumber $dataDisk.Number `
            -ErrorAction SilentlyContinue | Where-Object { $_.Type -ne 'Reserved' })
        if ($usablePartitions.Count -gt 1) {
            throw "Storage data disk $($dataDisk.Number) has multiple usable partitions; refusing to choose one."
        }
        $existingPartition = $usablePartitions | Select-Object -First 1
        if ($null -eq $existingPartition) {
            $existingPartition = New-Partition -DiskNumber $dataDisk.Number `
                -UseMaximumSize -ErrorAction Stop
        }

        $volume = $existingPartition | Get-Volume -ErrorAction SilentlyContinue
        if ($null -eq $volume -or [string]::IsNullOrWhiteSpace("$($volume.FileSystem)")) {
            $volume = $existingPartition | Format-Volume -FileSystem NTFS `
                -NewFileSystemLabel 'ClusterIscsiData' -Confirm:$false -ErrorAction Stop
        }
        elseif ($volume.FileSystem -ne 'NTFS') {
            throw "Storage data volume uses '$($volume.FileSystem)'; refusing to reformat it."
        }

        New-Item -ItemType Directory -Path $mountPath -Force | Out-Null
        if (@($existingPartition.AccessPaths) -notcontains $mountAccessPath) {
            Add-PartitionAccessPath -DiskNumber $existingPartition.DiskNumber `
                -PartitionNumber $existingPartition.PartitionNumber `
                -AccessPath $mountAccessPath -ErrorAction Stop
        }
    }

    $root = Join-Path $mountPath 'iSCSIVirtualDisks'
    New-Item -ItemType Directory -Path $root -Force | Out-Null
    return $root
}

$startedAt = Get-Date
try {
    . (Join-Path $PSScriptRoot 'Deploy-CommonHelpers.ps1')
    $config = Get-Content -LiteralPath $ConfigureFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    $storage = $config.Machines.Storage
    if ($null -eq $storage) { throw 'Config.json is missing Machines.Storage.' }

    $targetName = if ($storage.iSCSITargetName) {
        "$($storage.iSCSITargetName)"
    } else {
        'ClusterTarget'
    }
    $nodeIps = @(
        $config.Machines.Node01.IpConfig[0].Ip,
        $config.Machines.Node02.IpConfig[0].Ip
    ) | Where-Object { -not [string]::IsNullOrWhiteSpace("$_") } |
        Select-Object -Unique
    if ($nodeIps.Count -ne 2) {
        throw 'Exactly two Cluster node initiator IP addresses are required.'
    }
    $initiatorIds = @($nodeIps | ForEach-Object { "IPAddress:$_" })

    $diskSpecs = @(
        @{ Name = 'disk1.vhdx'; SizeBytes = [long](10GB) },
        @{ Name = 'disk2.vhdx'; SizeBytes = [long](10GB) },
        @{ Name = 'disk3.vhdx'; SizeBytes = [long](10GB) },
        @{ Name = 'diskq.vhdx'; SizeBytes = [long](1GB) }
    )
    $legacyRoot = 'C:\iSCSIVirtualDisks'
    $dataRoot = $null

    Write-StorageHeartbeat -Checkpoint 'Loading existing target state' -StartedAt $startedAt
    $target = Get-IscsiServerTarget -TargetName $targetName -ErrorAction SilentlyContinue
    if ($null -eq $target) {
        New-IscsiServerTarget -TargetName $targetName `
            -InitiatorId $initiatorIds -ErrorAction Stop | Out-Null
        $target = Get-IscsiServerTarget -TargetName $targetName -ErrorAction Stop
    }
    else {
        $actualInitiators = @($target.InitiatorIds | ForEach-Object { "$_" })
        $initiatorsMatch = @($initiatorIds | Where-Object {
            $actualInitiators -notcontains $_
        }).Count -eq 0
        if (-not $initiatorsMatch) {
            if ($actualInitiators -contains 'IQN:*' -and "$($target.Status)" -eq 'Connected') {
                .\Write-Info.ps1 (
                    "[WARN] Preserving connected legacy IQN:* initiator policy for '$targetName'."
                ) -ForegroundColor Yellow
            }
            else {
                Set-IscsiServerTarget -TargetName $targetName `
                    -InitiatorId $initiatorIds -ErrorAction Stop | Out-Null
            }
        }
    }

    $existingMappings = @($target.LunMappings)
    $unexpectedMappings = @($existingMappings | Where-Object {
        [System.IO.Path]::GetFileName("$($_.Path)") -notin $diskSpecs.Name
    })
    if ($unexpectedMappings.Count -gt 0) {
        throw "Target '$targetName' contains unexpected mappings; refusing destructive cleanup: $($unexpectedMappings.Path -join ', ')"
    }

    foreach ($spec in $diskSpecs) {
        Write-StorageHeartbeat -Checkpoint "Repairing $($spec.Name)" -StartedAt $startedAt
        $mapping = @($existingMappings | Where-Object {
            [System.IO.Path]::GetFileName("$($_.Path)") -ieq $spec.Name
        } | Select-Object -First 1)

        if ($mapping.Count -eq 1) {
            $diskPath = "$($mapping[0].Path)"
        }
        else {
            $legacyPath = Join-Path $legacyRoot $spec.Name
            if (Test-Path -LiteralPath $legacyPath -PathType Leaf) {
                $diskPath = $legacyPath
            }
            else {
                if (-not $dataRoot) { $dataRoot = Get-StorageDataRoot }
                $diskPath = Join-Path $dataRoot $spec.Name
            }
        }

        $virtualDisk = Get-IscsiVirtualDisk -Path $diskPath -ErrorAction SilentlyContinue
        if ($null -eq $virtualDisk -and (Test-Path -LiteralPath $diskPath -PathType Leaf)) {
            Import-IscsiVirtualDisk -Path $diskPath -ErrorAction Stop | Out-Null
            $virtualDisk = Get-IscsiVirtualDisk -Path $diskPath -ErrorAction Stop
        }
        if ($null -eq $virtualDisk) {
            New-IscsiVirtualDisk -Path $diskPath `
                -SizeBytes $spec.SizeBytes -ErrorAction Stop | Out-Null
            $virtualDisk = Get-IscsiVirtualDisk -Path $diskPath -ErrorAction Stop
        }
        if ([long]$virtualDisk.Size -ne [long]$spec.SizeBytes) {
            throw "Existing virtual disk '$diskPath' has size $($virtualDisk.Size); expected $($spec.SizeBytes)."
        }

        $target = Get-IscsiServerTarget -TargetName $targetName -ErrorAction Stop
        $mapped = @($target.LunMappings | Where-Object { "$($_.Path)" -ieq $diskPath })
        if ($mapped.Count -eq 0) {
            Add-IscsiVirtualDiskTargetMapping -TargetName $targetName `
                -Path $diskPath -ErrorAction Stop | Out-Null
        }
        $existingMappings = @(
            (Get-IscsiServerTarget -TargetName $targetName -ErrorAction Stop).LunMappings
        )
    }

    Set-Service WinTarget -StartupType Automatic -ErrorAction Stop
    $service = Get-Service WinTarget -ErrorAction Stop
    if ($service.Status -ne 'Running') {
        Start-Service WinTarget -ErrorAction Stop
    }
    Wait-DeploymentCondition -Condition {
        $running = (Get-Service WinTarget -ErrorAction SilentlyContinue).Status -eq 'Running'
        $listening = $null -ne (Get-NetTCPConnection -LocalPort 3260 `
            -State Listen -ErrorAction SilentlyContinue)
        return ($running -and $listening)
    } -TimeoutSeconds 120 -PollIntervalSeconds 5 `
        -Phase 'StorageConvergence' -Operation 'Wait for iSCSI target listener' `
        -HeartbeatPath $HeartbeatPath -LastCheckpoint 'Target mappings repaired' | Out-Null

    Write-StorageHeartbeat -Checkpoint 'Verifying complete Storage readiness' -StartedAt $startedAt
    $readinessOutput = @(& (Join-Path $scriptsPath 'Test-StorageReadiness.ps1') `
        -ConfigureFile $ConfigureFile -Detailed *>&1)
    $readinessOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
    if ($readinessOutput.Count -eq 0 -or $readinessOutput[-1] -ne $true) {
        throw 'Storage postconditions are incomplete after target repair.'
    }

    .\Write-Info.ps1 '[OK] Storage target and all four LUN mappings are ready.' `
        -ForegroundColor Green
    return $true
}
catch {
    .\Write-Error.ps1 "Storage convergence failed: $($_.Exception.Message)"
    throw
}
finally {
    Stop-StorageTranscript
}
