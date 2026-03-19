# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Orchestrator for the Storage Server (Storage01).
    Applies DSC for features/firewall, creates iSCSI target, reboots to ensure
    services start cleanly.

.DESCRIPTION
    Step 0 -> 1: Features + Firewall + iSCSI Target
      DSC: File-Services, FS-iSCSITarget-Server, firewall, hosts file,
           password never expires, WinTarget auto-start.
      Imperative: iSCSI target creation (virtual disks + target mapping).
      -> Deferred reboot (always reboot to ensure services start cleanly)

    Step 1 -> 2: Post-Reboot Verification
      Verify WinTarget service is running.
      -> Finish (signal file written)

    Storage01 is a workgroup machine -- no domain join, no multi-NIC.

.PARAMETER WorkingPath
    Path to the Cluster-Package root folder.

.EXAMPLE
    .\Deploy-Storage.ps1
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder    = $PSScriptRoot
$scriptsPath  = "$dscFolder\Scripts"
$mofFolder    = "$dscFolder\MOF\Storage"
$logFile      = "$dscFolder\Deploy-Storage.log"

$env:Path += ";$scriptsPath"
Push-Location $scriptsPath

Start-Transcript -Path $logFile -Append -Force
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "  Storage (Storage01) -- DSC + Imperative Deployment       " -ForegroundColor Cyan
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
    try {
        & $validateScript -ConfigPath $configFile
        .\Write-Info.ps1 "[OK] Config.json validation passed" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Config.json validation failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; return
    }
}

# ===========================================================================
# Reboot circuit breaker + Step tracking
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
    if (-not (Test-Path $rebootRegPath)) { New-Item -Path $rebootRegPath -Force | Out-Null }
    Set-ItemProperty -Path $rebootRegPath -Name $rebootRegName -Value $Count -Type DWord -Force
}

$stepRegName = 'DeployStep'
function Get-DeployStep {
    $val = Get-ItemProperty -Path $rebootRegPath -Name $stepRegName -ErrorAction SilentlyContinue
    if ($val) { return [int]$val.$stepRegName } else { return 0 }
}
function Set-DeployStep {
    param([int]$Step)
    if (-not (Test-Path $rebootRegPath)) { New-Item -Path $rebootRegPath -Force | Out-Null }
    Set-ItemProperty -Path $rebootRegPath -Name $stepRegName -Value $Step -Type DWord -Force
}

$currentStep = Get-DeployStep
.\Write-Info.ps1 "Current deploy step: $currentStep" -ForegroundColor DarkGray

. "$dscFolder\Deploy-CommonHelpers.ps1"

$signalFile = "$dscFolder\Deploy-Storage.Completed.signal"
if (Test-Path $signalFile) {
    .\Write-Info.ps1 "[OK] Storage deployment already completed (signal file exists)." -ForegroundColor Green
    Remove-ResumeTask
    Pop-Location; Stop-Transcript; return
}

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

# ===========================================================================
# Pre-check: Validate hostname
# ===========================================================================
if (Test-Path $configFile) {
    try {
        $cfg = Get-Content -Path $configFile -Raw | ConvertFrom-Json
        $expectedName = $cfg.Machines.Storage.ComputerName
        if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
            $currentRebootCount = Get-RebootCount
            if ($currentRebootCount -ge $maxRebootCount) {
                .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered. Skipping rename." -ForegroundColor Red
            } else {
                Set-RebootCount -Count ($currentRebootCount + 1)
                .\Write-Info.ps1 "Renaming computer from $env:COMPUTERNAME to $expectedName..." -ForegroundColor Yellow
                Rename-Computer -NewName $expectedName -Force

                Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Storage.ps1" `
                    -WorkingPath $WorkingPath -DscFolder $dscFolder

                Pop-Location; Stop-Transcript; return
            }
        }
    } catch {
        .\Write-Info.ps1 "[WARN] Could not validate hostname: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# ===========================================================================
# Step 0 -> 1: DSC (features + firewall) + iSCSI Target Creation
# ===========================================================================
if ($currentStep -lt 1) {
    .\Write-Info.ps1 "---- Phase 1a: DSC Configuration (features + baseline) ----" -ForegroundColor Yellow
    $phase1 = [System.Diagnostics.Stopwatch]::StartNew()

    try {
        . "$dscFolder\Storage-Configuration.ps1"
        .\Write-Info.ps1 "Compiling Storage DSC configuration..." -ForegroundColor Cyan
        StorageConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
        .\Write-Info.ps1 "Applying Storage DSC configuration..." -ForegroundColor Yellow
        Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
        .\Write-Info.ps1 "[OK] DSC applied in $([math]::Round($phase1.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] DSC failed: $($_.Exception.Message)"
        .\Write-Info.ps1 "Continuing with imperative steps..." -ForegroundColor Yellow
    }
    $phase1.Stop()

    # Imperative: Create iSCSI target
    .\Write-Info.ps1 "---- Phase 1b: Imperative (iSCSI Target Creation) ----" -ForegroundColor Yellow
    try {
        & "$dscFolder\Invoke-StorageImperativeSteps.ps1" -Step 1 -WorkingPath $WorkingPath
        .\Write-Info.ps1 "[OK] iSCSI target created." -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] iSCSI target creation failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; return
    }

    # Always reboot to ensure WinTarget service starts cleanly
    Set-DeployStep -Step 1
    Set-RebootCount -Count ((Get-RebootCount) + 1)
    .\Write-Info.ps1 "Scheduling reboot to ensure services start cleanly..." -ForegroundColor Yellow
    Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Storage.ps1" `
        -WorkingPath $WorkingPath -DscFolder $dscFolder

    Pop-Location; Stop-Transcript; return
}

# ===========================================================================
# Step 1 -> 2: Post-Reboot Verification
# ===========================================================================
if ($currentStep -eq 1) {
    Start-Sleep -Seconds 10
    .\Write-Info.ps1 "---- Phase 2: Post-Reboot Verification ----" -ForegroundColor Yellow

    # Verify WinTarget service is running
    $winTarget = Get-Service WinTarget -ErrorAction SilentlyContinue
    if ($null -ne $winTarget) {
        if ($winTarget.Status -ne 'Running') {
            .\Write-Info.ps1 "Starting WinTarget service..." -ForegroundColor Yellow
            Start-Service WinTarget -ErrorAction SilentlyContinue
            Start-Sleep -Seconds 5
        }
        .\Write-Info.ps1 "[OK] WinTarget service status: $((Get-Service WinTarget).Status)" -ForegroundColor Green
    } else {
        .\Write-Info.ps1 "[WARN] WinTarget service not found" -ForegroundColor Yellow
    }

    # Re-apply DSC to catch drift
    try {
        . "$dscFolder\Storage-Configuration.ps1"
        StorageConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
        Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
        .\Write-Info.ps1 "[OK] DSC re-applied post-reboot." -ForegroundColor Green
    }
    catch {
        .\Write-Info.ps1 "[WARN] DSC re-apply had issues: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # Run storage status check if available
    $checkScript = "$scriptsPath\Check-StorageStatus.ps1"
    if (Test-Path $checkScript) {
        try { & $checkScript } catch { .\Write-Info.ps1 "[WARN] Storage status check: $($_.Exception.Message)" -ForegroundColor Yellow }
    }

    Set-DeployStep -Step 2
    Set-RebootCount -Count 0

    "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
    .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
    Remove-ResumeTask

    $cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"
    if (Test-Path $cleanupScript) { & $cleanupScript }

    $stopwatch.Stop()
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Storage Deployment Complete ($([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min)" -ForegroundColor Cyan
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
}

Pop-Location
Stop-Transcript
