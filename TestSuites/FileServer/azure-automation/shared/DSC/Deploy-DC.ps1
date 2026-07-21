# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Orchestrator for the DC (DC01).
    Applies DSC, promotes DC, reboots, then completes post-promotion tasks.
    Works for both Domain and Cluster scenarios.

.DESCRIPTION
    Uses registry-based step tracking with deferred reboots:

    Step 0 -> 1: Pre-Promotion
      DSC: AD-Domain-Services, RemoteAccess features, firewall, hosts file,
           password never expires, multi-NIC routing.
      Imperative: DC promotion (Install-ADDSForest), tools install.
      -> Deferred reboot (TKFRSAR startup task + 90s shutdown)

    Step 1 -> 2: Post-Promotion
      DSC: Re-apply to pick up post-promote registry keys (LDAP signing,
           CBAC/Armor, service auto-start, NTDS/ADWS dependencies).
      Imperative: Test accounts, CBAC objects, GPO import, DNS records,
                  DC status checker task, RemoteAccess service start.
      -> Finish (signal file written)

    Re-running is safe. DSC only touches drifted state and imperative steps
    have their own idempotency checks.

.PARAMETER WorkingPath
    Path to the package root folder (Domain-Package or Cluster-Package).

.EXAMPLE
    .\Deploy-DC.ps1
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder    = $PSScriptRoot
$scriptsPath  = "$dscFolder\Scripts"
$mofFolder    = "$dscFolder\MOF\DC"
$logFile      = "$dscFolder\Deploy-DC.log"

# Put Scripts folder on PATH so Write-Info.ps1, Write-Error.ps1 etc. resolve via .\
$env:Path += ";$scriptsPath"
Push-Location $scriptsPath

Start-Transcript -Path $logFile -Append -Force
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "  DC (DC01) -- DSC + Imperative Deployment                 " -ForegroundColor Cyan
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "WorkingPath : $WorkingPath" -ForegroundColor DarkGray
.\Write-Info.ps1 "DSCFolder   : $dscFolder"  -ForegroundColor DarkGray
.\Write-Info.ps1 ""

# ===========================================================================
# Pre-flight validation
# ===========================================================================
$configFile = "$WorkingPath\Config.json"
$validateScript = "$scriptsPath\Validate-ConfigFile.ps1"
if (Test-Path $validateScript) {
    .\Write-Info.ps1 "Validating Config.json..." -ForegroundColor Cyan
    try {
        & $validateScript -ConfigPath $configFile
        .\Write-Info.ps1 "[OK] Config.json validation passed" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Config.json validation failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; return
    }
}

$toolsJsonPath = "$WorkingPath\Tools.json"
if (Test-Path $toolsJsonPath) {
    .\Write-Info.ps1 "Tools.json found at: $toolsJsonPath" -ForegroundColor DarkGray
} else {
    $toolsJsonPath = "$scriptsPath\Tools.json"
    if (Test-Path $toolsJsonPath) {
        .\Write-Info.ps1 "Tools.json found at fallback: $toolsJsonPath" -ForegroundColor DarkGray
    } else {
        .\Write-Info.ps1 "[WARN] Tools.json not found at $WorkingPath\Tools.json or $scriptsPath\Tools.json" -ForegroundColor Yellow
    }
}

# ===========================================================================
# Reboot circuit breaker
# ===========================================================================
$rebootRegPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
$rebootRegName = 'RebootCount'
$maxRebootCount = 3

function Get-RebootCount {
    $val = Get-ItemProperty -Path $rebootRegPath -Name $rebootRegName -ErrorAction SilentlyContinue
    if ($val) { return [int]$val.$rebootRegName } else { return 0 }
}

function Set-RebootCount {
    param([int]$Count)
    if (-not (Test-Path $rebootRegPath)) {
        New-Item -Path $rebootRegPath -Force | Out-Null
    }
    Set-ItemProperty -Path $rebootRegPath -Name $rebootRegName -Value $Count -Type DWord -Force
}

# ===========================================================================
# Step tracking (registry-based)
# ===========================================================================
$stepRegName = 'DeployStep'

function Get-DeployStep {
    $val = Get-ItemProperty -Path $rebootRegPath -Name $stepRegName -ErrorAction SilentlyContinue
    if ($val) { return [int]$val.$stepRegName } else { return 0 }
}

function Set-DeployStep {
    param([int]$Step)
    if (-not (Test-Path $rebootRegPath)) {
        New-Item -Path $rebootRegPath -Force | Out-Null
    }
    Set-ItemProperty -Path $rebootRegPath -Name $stepRegName -Value $Step -Type DWord -Force
}

# Marker so a deferred-reboot resume (still at step 0) doesn't replay the whole Phase 1a
# DSC compile+apply -- it goes straight to the pending-reboot re-check + promotion instead.
$phase1aRegName = 'Phase1aDscApplied'

function Get-Phase1aApplied {
    $val = Get-ItemProperty -Path $rebootRegPath -Name $phase1aRegName -ErrorAction SilentlyContinue
    if ($val) { return [int]$val.$phase1aRegName } else { return 0 }
}

function Set-Phase1aApplied {
    param([int]$Value)
    if (-not (Test-Path $rebootRegPath)) {
        New-Item -Path $rebootRegPath -Force | Out-Null
    }
    Set-ItemProperty -Path $rebootRegPath -Name $phase1aRegName -Value $Value -Type DWord -Force
}

$currentStep = Get-DeployStep
.\Write-Info.ps1 "Current deploy step: $currentStep" -ForegroundColor DarkGray

# Load shared helpers (reboot scheduling, TKFRSAR cleanup)
. "$dscFolder\Deploy-CommonHelpers.ps1"

$signalFile = "$dscFolder\Deploy-DC.Completed.signal"
if (Test-Path $signalFile) {
    .\Write-Info.ps1 "[OK] DC deployment already completed (signal file exists)." -ForegroundColor Green
    Remove-ResumeTask
    Pop-Location; Stop-Transcript; return
}

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

# ===========================================================================
# Pre-check: Validate hostname (rename if needed)
# ===========================================================================
if (Test-Path $configFile) {
    try {
        $cfg = Get-Content -Path $configFile -Raw | ConvertFrom-Json
        $expectedName = $cfg.Machines.DC.ComputerName
        if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
            $currentRebootCount = Get-RebootCount
            if ($currentRebootCount -ge $maxRebootCount) {
                .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered ($currentRebootCount >= $maxRebootCount). Skipping rename reboot -- continuing with hostname '$env:COMPUTERNAME'." -ForegroundColor Red
            } else {
                Set-RebootCount -Count ($currentRebootCount + 1)
                .\Write-Info.ps1 "Renaming computer from $env:COMPUTERNAME to $expectedName (reboot $($currentRebootCount + 1)/$maxRebootCount)..." -ForegroundColor Yellow
                Rename-Computer -NewName $expectedName -Force

                .\Write-Info.ps1 "Scheduling Deploy-DC.ps1 to re-run after reboot..." -ForegroundColor Yellow
                Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-DC.ps1" `
                    -WorkingPath $WorkingPath -DscFolder $dscFolder

                Pop-Location; Stop-Transcript; return
            }
        }
    } catch {
        .\Write-Info.ps1 "[WARN] Could not validate hostname: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# ===========================================================================
# Step 0 -> 1: DSC (features) + Promote DC
# ===========================================================================
if ($currentStep -lt 1) {
    if ((Get-Phase1aApplied) -eq 1) {
        # Resuming after the pending-reboot gate deferred a reboot: Phase 1a DSC already ran
        # (features installed), so skip the idempotent replay and re-check the reboot flag below.
        .\Write-Info.ps1 "---- Phase 1a: DSC already applied (resuming after pending-reboot); skipping re-apply ----" -ForegroundColor DarkGray
    }
    else {
        .\Write-Info.ps1 "---- Phase 1a: DSC Configuration (features + baseline) ----" -ForegroundColor Yellow
        $phase1 = [System.Diagnostics.Stopwatch]::StartNew()

        try {
            . "$dscFolder\DC-Configuration.ps1"
            .\Write-Info.ps1 "Compiling DC DSC configuration (config: $configFile)..." -ForegroundColor Cyan
            DcConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
            .\Write-Info.ps1 "Applying DC DSC configuration..." -ForegroundColor Yellow
            Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
            .\Write-Info.ps1 "[OK] DSC Phase 1 applied in $([math]::Round($phase1.Elapsed.TotalSeconds))s" -ForegroundColor Green
            # Mark Phase 1a done ONLY after a successful apply, so the pending-reboot resume skips
            # the idempotent replay -- but a FAILED apply leaves the marker unset so a rerun
            # re-applies DSC (repairing missing AD DS/features) before promotion is retried.
            Set-Phase1aApplied -Value 1
        }
        catch {
            .\Write-Error.ps1 "[FAIL] DSC Phase 1 failed: $($_.Exception.Message)"
            .\Write-Info.ps1 "Continuing with imperative steps..." -ForegroundColor Yellow
        }
        $phase1.Stop()
        .\Write-Info.ps1 ""
    }

    # -- Pending-reboot gate BEFORE promotion --
    # DSC Phase 1a installs AD-DS + features; a Windows servicing/feature operation can leave
    # a pending-reboot flag. Install-ADDSForest refuses to start while a reboot is pending
    # ("Role change is in progress or this computer needs to be restarted") and fails the whole
    # deploy. Mirror the SUT's guard: if a reboot is pending, defer + resume. On resume the
    # Phase1aDscApplied marker skips the DSC replay, the reboot flag is clear, and promotion
    # proceeds. Bounded by the reboot circuit breaker.
    $rebootPending = (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\RebootPending') -or
                     (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update\RebootRequired') -or
                     (Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations')
    if ($rebootPending) {
        $currentRebootCount = Get-RebootCount
        if ($currentRebootCount -ge $maxRebootCount) {
            .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered ($currentRebootCount >= $maxRebootCount). Proceeding to promotion despite pending reboot." -ForegroundColor Red
        } else {
            Set-RebootCount -Count ($currentRebootCount + 1)
            .\Write-Info.ps1 "[WARN] A reboot is pending after DSC feature install; promotion cannot start until it clears (reboot $($currentRebootCount + 1)/$maxRebootCount)." -ForegroundColor Yellow
            .\Write-Info.ps1 "Scheduling deferred reboot and resume (DSC re-applies fast, then promotion proceeds)..." -ForegroundColor Yellow
            Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-DC.ps1" `
                -WorkingPath $WorkingPath -DscFolder $dscFolder
            Pop-Location; Stop-Transcript; return
        }
    }

    # -- Imperative Step 1 (promote + tools) --
    .\Write-Info.ps1 "---- Phase 1b: Imperative Step 1 (DC Promotion) ----" -ForegroundColor Yellow
    try {
        & "$dscFolder\Invoke-DcImperativeSteps.ps1" -Step 1 -WorkingPath $WorkingPath
        .\Write-Info.ps1 "[OK] DC Promotion initiated." -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] DC Promotion failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; return
    }

    # -- Schedule deferred reboot + resume at Step 1 --
    Set-DeployStep -Step 1
    .\Write-Info.ps1 "Scheduling deferred reboot and resume..." -ForegroundColor Yellow
    Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-DC.ps1" `
        -WorkingPath $WorkingPath -DscFolder $dscFolder

    Pop-Location; Stop-Transcript; return
}

# ===========================================================================
# Step 1 -> 2: Post-Promotion: re-apply DSC + accounts + CBAC + DNS
# ===========================================================================
if ($currentStep -eq 1) {
    .\Write-Info.ps1 "---- Phase 2a: DSC Re-Apply (post-promote drift) ----" -ForegroundColor Yellow
    $phase2 = [System.Diagnostics.Stopwatch]::StartNew()
    Start-Sleep -Seconds 10  # Wait for AD DS services to stabilize

    try {
        . "$dscFolder\DC-Configuration.ps1"
        .\Write-Info.ps1 "Compiling DC DSC configuration (post-promote, config: $configFile)..." -ForegroundColor Cyan
        DcConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
        .\Write-Info.ps1 "Applying DC DSC configuration (catching drift)..." -ForegroundColor Yellow
        Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
        .\Write-Info.ps1 "[OK] DSC Phase 2 applied." -ForegroundColor Green
    }
    catch {
        .\Write-Info.ps1 "[WARN] DSC re-apply had issues: $($_.Exception.Message)" -ForegroundColor Yellow
        .\Write-Info.ps1 "Continuing -- imperative steps may cover what's needed." -ForegroundColor Yellow
    }
    $phase2.Stop()
    .\Write-Info.ps1 ""

    # -- Imperative Step 2 (accounts, CBAC, GPO, DNS, services) --
    .\Write-Info.ps1 "---- Phase 2b: Imperative Step 2 (AD objects + services) ----" -ForegroundColor Yellow
    $phase2b = [System.Diagnostics.Stopwatch]::StartNew()
    try {
        & "$dscFolder\Invoke-DcImperativeSteps.ps1" -Step 2 -WorkingPath $WorkingPath
        .\Write-Info.ps1 "[OK] Post-promotion configuration complete." -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Post-promotion steps failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; return
    }
    $phase2b.Stop()

    # -- Finish --
    Set-DeployStep -Step 2
    Set-RebootCount -Count 0
    Set-Phase1aApplied -Value 0

    "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
    .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
    Remove-ResumeTask

    $cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"
    if (Test-Path $cleanupScript) {
        & $cleanupScript
    }

    $stopwatch.Stop()
    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
    .\Write-Info.ps1 "  DC Deployment Complete" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Total time  : $([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min" -ForegroundColor Cyan
    .\Write-Info.ps1 "    DSC       : $([math]::Round($phase2.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "    Imperative: $([math]::Round($phase2b.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
}

Pop-Location
Stop-Transcript
