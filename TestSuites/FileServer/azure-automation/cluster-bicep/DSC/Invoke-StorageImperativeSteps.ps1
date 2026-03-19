# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Imperative steps for the Storage Server (Storage01).
    Run AFTER DSC has been applied.

.DESCRIPTION
    Step 1: Create iSCSI target with virtual disks for the failover cluster.
      - Installs iSCSI target feature (idempotent, DSC also installs it)
      - Creates iSCSI server target
      - Creates 4 virtual disks (3x data + 1x quorum)
      - Maps disks to the target
      - Ensures WinTarget service is set to auto-start

.PARAMETER WorkingPath
    Path to the Cluster-Package folder.

.PARAMETER Step
    Which step to execute (currently only 1).

.EXAMPLE
    .\Invoke-StorageImperativeSteps.ps1 -Step 1 -WorkingPath C:\Cluster-Package
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [ValidateSet(1)]
    [int]$Step = 1,
    [string]$ConfigureFile = "$WorkingPath\Config.json"
)

$ErrorActionPreference = 'Stop'
$scriptsPath = "$PSScriptRoot\Scripts"
$env:Path += ";$scriptsPath"
Push-Location $scriptsPath

[string]$logFile = "$PSScriptRoot\Invoke-StorageImperativeSteps.log"
Start-Transcript -Path $logFile -Append -Force

$config = $null
if (Test-Path $ConfigureFile) {
    try { $config = Get-Content -Path $ConfigureFile -Raw | ConvertFrom-Json }
    catch { Write-Warning "Failed to parse Config.json: $_" }
}

if ($null -eq $config) {
    .\Write-Error.ps1 "Config.json not loaded. Cannot proceed."
    Stop-Transcript; Pop-Location; return $false
}

# ===========================================================================
# STEP 1: Create iSCSI Target
# ===========================================================================
if ($Step -eq 1) {
  # -- TLS Cipher Suite Configuration --
  try {
    .\Write-Info.ps1 "Configuring TLS cipher suites..." -ForegroundColor Yellow
    $tlsResult = & "$scriptsPath\Configure-TlsCipherSuites.ps1"
    if ($tlsResult) {
        .\Write-Info.ps1 "[OK] TLS cipher suites configured" -ForegroundColor Green
    }
  } catch {
    .\Write-Info.ps1 "[WARN] TLS cipher suite config failed: $($_.Exception.Message)" -ForegroundColor Yellow
  }

  try {
    .\Write-Info.ps1 "---- Step 1: Create iSCSI Target ----" -ForegroundColor Yellow

    $storageMachine = $config.Machines.Storage
    $targetName = if ($storageMachine.iSCSITargetName) { $storageMachine.iSCSITargetName } else { 'ClusterTarget' }

    # Check if target already exists
    $existingTarget = Get-IscsiServerTarget -TargetName $targetName -ErrorAction SilentlyContinue
    if ($null -ne $existingTarget) {
        .\Write-Info.ps1 "[OK] iSCSI target '$targetName' already exists -- skipping creation" -ForegroundColor Green
        Stop-Transcript; Pop-Location; return $true
    }

    # Determine virtual disk storage path from data disks
    $systemDrive = $env:SystemDrive
    $vhdPath = "$systemDrive\iSCSIVirtualDisks"
    New-Item -ItemType Directory -Path $vhdPath -Force | Out-Null

    # Create iSCSI server target (allow all initiators)
    .\Write-Info.ps1 "Creating iSCSI target '$targetName'..." -ForegroundColor Cyan
    New-IscsiServerTarget -TargetName $targetName -InitiatorIds @("IQN:*")
    .\Write-Info.ps1 "[OK] iSCSI target created" -ForegroundColor Green

    # Create virtual disks
    $diskSpecs = @(
        @{ Name = 'disk1'; SizeBytes = 10GB },
        @{ Name = 'disk2'; SizeBytes = 10GB },
        @{ Name = 'disk3'; SizeBytes = 10GB },
        @{ Name = 'diskq'; SizeBytes = 1GB }
    )

    foreach ($spec in $diskSpecs) {
        $diskPath = "$vhdPath\$($spec.Name).vhdx"
        if (Test-Path $diskPath) {
            .\Write-Info.ps1 "[OK] Virtual disk $($spec.Name) already exists" -ForegroundColor Green
        } else {
            .\Write-Info.ps1 "Creating virtual disk $($spec.Name) ($([math]::Round($spec.SizeBytes / 1GB))GB)..." -ForegroundColor Cyan
            New-IscsiVirtualDisk -Path $diskPath -SizeBytes $spec.SizeBytes
            .\Write-Info.ps1 "[OK] Virtual disk $($spec.Name) created" -ForegroundColor Green
        }

        # Map disk to target
        $mapping = Get-IscsiServerTarget -TargetName $targetName -ErrorAction SilentlyContinue
        $alreadyMapped = $false
        if ($null -ne $mapping -and $null -ne $mapping.LunMappings) {
            foreach ($lun in $mapping.LunMappings) {
                if ($lun.Path -eq $diskPath) { $alreadyMapped = $true; break }
            }
        }
        if (-not $alreadyMapped) {
            Add-IscsiVirtualDiskTargetMapping -TargetName $targetName -Path $diskPath
            .\Write-Info.ps1 "  Mapped $($spec.Name) to target" -ForegroundColor DarkGray
        }
    }

    # Ensure WinTarget service auto-starts
    $winTarget = Get-Service WinTarget -ErrorAction SilentlyContinue
    if ($null -ne $winTarget -and $winTarget.StartType -ne 'Automatic') {
        Set-Service WinTarget -StartupType Automatic
        .\Write-Info.ps1 "[OK] WinTarget service set to Automatic" -ForegroundColor Green
    }

    # Start the service if not running
    if ($null -ne $winTarget -and $winTarget.Status -ne 'Running') {
        Start-Service WinTarget
        .\Write-Info.ps1 "[OK] WinTarget service started" -ForegroundColor Green
    }

    .\Write-Info.ps1 "=== Storage imperative steps completed ===" -ForegroundColor Cyan
    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "Storage imperative step 1 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}
