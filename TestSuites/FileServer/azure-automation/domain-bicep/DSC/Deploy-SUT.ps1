# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Orchestrator for the Domain SUT (Node01).
    Applies DSC, joins domain, reboots, installs features/tools, configures environment.

.DESCRIPTION
    Uses registry-based step tracking with deferred reboots (matching workgroup patterns):

    Step 0 -> 1: Pre-Domain-Join
      DSC-Lite: Hosts file + firewall only (features need domain context later).
      Imperative: Domain join via domainjoin.ps1.
      -> Deferred reboot (TKFRSAR startup task + 90s shutdown)

    Step 1 -> 2: Features + Tools
      DSC-Full: Windows features (batched), Hyper-V, SMB shares, registry,
                FSRM, SMB signing, password never expires, computer password.
      Tools: PowerShellCore, OpenSSH, WAC (background job concurrent with DSC).
      -> Deferred reboot if pending (Hyper-V feature typically needs one)

    Step 2 -> 3: Environment Setup
      Imperative: Disk partitioning (ReFS K:, FAT32 J:), data disk shares,
                  symbolic links, FSA environment, DFS namespaces, QUIC certs.
      -> Finish (signal file written)

    Re-running is safe. DSC only touches drifted state and imperative steps
    have built-in idempotency checks.

.PARAMETER WorkingPath
    Path to the Domain-Package root folder.

.EXAMPLE
    .\Deploy-SUT.ps1
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder    = $PSScriptRoot
$scriptsPath  = "$dscFolder\Scripts"
$mofFolder    = "$dscFolder\MOF\SUT"
$logFile      = "$dscFolder\Deploy-SUT.log"

# Put Scripts folder on PATH so Write-Info.ps1, Write-Error.ps1 etc. resolve via .\
$env:Path += ";$scriptsPath"
Push-Location $scriptsPath

Start-Transcript -Path $logFile -Append -Force
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "  Domain SUT (Node01) -- DSC + Imperative Deployment       " -ForegroundColor Cyan
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "WorkingPath : $WorkingPath" -ForegroundColor DarkGray
.\Write-Info.ps1 "DSCFolder   : $dscFolder"  -ForegroundColor DarkGray
.\Write-Info.ps1 ""

# ===========================================================================
# Pre-flight validation
# ===========================================================================
$configFile = "$WorkingPath\Config.json"
$cfg = $null
if (Test-Path $configFile) {
    try { $cfg = Get-Content -Path $configFile -Raw | ConvertFrom-Json }
    catch { Write-Warning "Could not parse Config.json: $_" }
}
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

$currentStep = Get-DeployStep
.\Write-Info.ps1 "Current deploy step: $currentStep" -ForegroundColor DarkGray

# Load shared helpers (reboot scheduling, TKFRSAR cleanup)
. "$dscFolder\Deploy-CommonHelpers.ps1"

# Cancel any stale reboot task from a previous run so it doesn't fire
# in the middle of feature installation (0x8007045b).
$staleReboot = Get-ScheduledTask -TaskName 'PostDeployReboot' -ErrorAction SilentlyContinue
if ($null -ne $staleReboot) {
    Unregister-ScheduledTask -TaskName 'PostDeployReboot' -Confirm:$false
    .\Write-Info.ps1 "[OK] Cancelled stale PostDeployReboot task from previous run." -ForegroundColor Yellow
}

$signalFile = "$dscFolder\Deploy-SUT.Completed.signal"
if (Test-Path $signalFile) {
    .\Write-Info.ps1 "[OK] SUT deployment already completed (signal file exists)." -ForegroundColor Green
    Remove-ResumeTask
    Pop-Location; Stop-Transcript; return
}

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$phase1Ok = $false
$phase3Ok = $false

# ===========================================================================
# Pre-check: Validate hostname (rename if needed)
# ===========================================================================
if (Test-Path $configFile) {
    try {
        $cfg = Get-Content -Path $configFile -Raw | ConvertFrom-Json
        $expectedName = $cfg.Machines.SUT.ComputerName
        if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
            $currentRebootCount = Get-RebootCount
            if ($currentRebootCount -ge $maxRebootCount) {
                .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered ($currentRebootCount >= $maxRebootCount). Skipping rename reboot -- continuing with hostname '$env:COMPUTERNAME'." -ForegroundColor Red
            } else {
                Set-RebootCount -Count ($currentRebootCount + 1)
                .\Write-Info.ps1 "Renaming computer from $env:COMPUTERNAME to $expectedName (reboot $($currentRebootCount + 1)/$maxRebootCount)..." -ForegroundColor Yellow
                Rename-Computer -NewName $expectedName -Force

                .\Write-Info.ps1 "Scheduling Deploy-SUT.ps1 to re-run after reboot..." -ForegroundColor Yellow
                Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-SUT.ps1" `
                    -WorkingPath $WorkingPath -DscFolder $dscFolder

                Pop-Location
                Stop-Transcript
                return
            }
        }
    } catch {
        .\Write-Info.ps1 "[WARN] Could not validate hostname: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# ===========================================================================
# Step 0 -> 1: Hosts file + Domain Join
# ===========================================================================
if ($currentStep -lt 1) {
    # DSC-Lite: Just hosts file and firewall (full DSC needs domain context)
    .\Write-Info.ps1 "---- Phase 1a: DSC Lite (hosts + firewall) ----" -ForegroundColor Yellow
    $phase1sw = [System.Diagnostics.Stopwatch]::StartNew()
    try {
        . "$dscFolder\SUT-Configuration.ps1"
        .\Write-Info.ps1 "Compiling SUT DSC configuration (config: $configFile)..." -ForegroundColor Cyan
        SutConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
        .\Write-Info.ps1 "Applying SUT DSC configuration (pre-join, partial)..." -ForegroundColor Yellow
        Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
        .\Write-Info.ps1 "[OK] DSC Lite applied in $([math]::Round($phase1sw.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Info.ps1 "[WARN] DSC had partial failures (expected pre-domain-join): $($_.Exception.Message)" -ForegroundColor Yellow
        .\Write-Info.ps1 "Continuing -- will re-apply after domain join." -ForegroundColor Yellow
    }
    $phase1sw.Stop()
    .\Write-Info.ps1 ""

    # Imperative Step 1 -- Domain Join
    .\Write-Info.ps1 "---- Phase 1b: Domain Join ----" -ForegroundColor Yellow
    try {
        & "$dscFolder\Invoke-SutImperativeSteps.ps1" -Step 1 -WorkingPath $WorkingPath
        .\Write-Info.ps1 "[OK] Domain join complete." -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Domain join failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; return
    }

    # Schedule deferred reboot + resume
    Set-DeployStep -Step 1
    .\Write-Info.ps1 "Scheduling deferred reboot and resume..." -ForegroundColor Yellow
    # After domain join, resume task must run as domain admin for AD operations (e.g. domain-based DFS)
    $domainNetBios = if ($cfg.Domain -and $cfg.Domain.NetBiosName) { $cfg.Domain.NetBiosName } else { $cfg.Core.DomainName.Split('.')[0].ToUpper() }
    $domainAdminUser = "$domainNetBios\$($cfg.Core.Username)"
    $domainAdminPass = $cfg.Core.Password
    Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-SUT.ps1" `
        -WorkingPath $WorkingPath -DscFolder $dscFolder `
        -RunAsUser $domainAdminUser -RunAsPassword $domainAdminPass

    Pop-Location
    Stop-Transcript
    return
}

# ===========================================================================
# Step 1 -> 2: Full DSC (features, shares, registry) + Tools
# ===========================================================================
if ($currentStep -eq 1) {
    Start-Sleep -Seconds 10  # Post-reboot stabilization

    # -- Tools install (background job) --
    .\Write-Info.ps1 "---- Phase 2a: Tools Install (background) ----" -ForegroundColor Yellow
    $phase2 = [System.Diagnostics.Stopwatch]::StartNew()
    $toolsSignal = "$scriptsPath\InstallMSIAndTools.Completed.signal"
    $toolsJob    = $null

    if (Test-Path $toolsSignal) {
        .\Write-Info.ps1 "[OK] Tools already installed (signal file exists)." -ForegroundColor Green
    }
    else {
        .\Write-Info.ps1 "Starting tools install as background job..." -ForegroundColor Cyan
        $toolsInstaller = "$scriptsPath\InstallMSIAndTools.ps1"
        $toolsScriptsDir = $scriptsPath
        $toolsJobLog = "$scriptsPath\InstallMSIAndTools.job.log"
        $toolsJob = Start-Job -ScriptBlock {
            param($wp, $installer, $sd, $jl)
            Set-Location $sd
            $env:Path += ";$sd"
            & $installer -Role 'SUT' *> $jl
        } -ArgumentList $WorkingPath, $toolsInstaller, $toolsScriptsDir, $toolsJobLog
    }
    $phase2.Stop()

    # -- Full DSC (features + shares + registry) --
    .\Write-Info.ps1 "---- Phase 2b: DSC Full (features, shares, registry) ----" -ForegroundColor Yellow
    $phase2b = [System.Diagnostics.Stopwatch]::StartNew()
    try {
        . "$dscFolder\SUT-Configuration.ps1"
        .\Write-Info.ps1 "Compiling SUT DSC configuration (full, config: $configFile)..." -ForegroundColor Cyan
        SutConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
        .\Write-Info.ps1 "Applying full SUT DSC configuration..." -ForegroundColor Yellow
        Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
        $phase1Ok = $true
        .\Write-Info.ps1 "[OK] Full DSC applied in $([math]::Round($phase2b.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Full DSC failed: $($_.Exception.Message)"
        .\Write-Info.ps1 "Continuing with remaining steps..." -ForegroundColor Yellow
    }
    $phase2b.Stop()
    .\Write-Info.ps1 ""

    # -- Wait for tools if still running --
    if ($null -ne $toolsJob) {
        $phase2.Start()
        .\Write-Info.ps1 "Waiting for tools install to complete..." -ForegroundColor Yellow
        $toolsJob | Wait-Job | Remove-Job -Force
        $phase2.Stop()
        if (Test-Path $toolsSignal) {
            .\Write-Info.ps1 "[OK] Tools installed in $([math]::Round($phase2.Elapsed.TotalSeconds))s" -ForegroundColor Green
        } else {
            .\Write-Info.ps1 "[WARN] Tools job completed but signal file not found -- check InstallMSIAndTools.ps1.log" -ForegroundColor Yellow
        }
    }

    # Check if a reboot is pending (e.g., from Hyper-V feature installation)
    $rebootPending = (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\RebootPending') -or
                     (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update\RebootRequired') -or
                     (Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations')
    if ($rebootPending) {
        $currentRebootCount = Get-RebootCount
        if ($currentRebootCount -ge $maxRebootCount) {
            .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered ($currentRebootCount >= $maxRebootCount). Skipping pending-reboot -- continuing without reboot." -ForegroundColor Red
            Set-DeployStep -Step 2
        } else {
            Set-RebootCount -Count ($currentRebootCount + 1)
            Set-DeployStep -Step 2
            .\Write-Info.ps1 "[WARN] A reboot is required to complete feature installation (reboot $($currentRebootCount + 1)/$maxRebootCount)." -ForegroundColor Yellow
            .\Write-Info.ps1 'Scheduling Deploy-SUT.ps1 to re-run after reboot...' -ForegroundColor Yellow
            $domainNetBios = if ($cfg.Domain -and $cfg.Domain.NetBiosName) { $cfg.Domain.NetBiosName } else { $cfg.Core.DomainName.Split('.')[0].ToUpper() }
            $domainAdminUser = "$domainNetBios\$($cfg.Core.Username)"
            $domainAdminPass = $cfg.Core.Password
            Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-SUT.ps1" `
                -WorkingPath $WorkingPath -DscFolder $dscFolder `
                -RunAsUser $domainAdminUser -RunAsPassword $domainAdminPass

            Pop-Location
            Stop-Transcript
            return
        }
    } else {
        .\Write-Info.ps1 'No reboot required after feature install.' -ForegroundColor Green
        Set-DeployStep -Step 2
    }
}

# ===========================================================================
# Step 2 -> 3: Environment Setup (disks, DFS, QUIC, FSA)
# ===========================================================================
$currentStep = Get-DeployStep
if ($currentStep -ge 2 -and -not (Test-Path $signalFile)) {
    Start-Sleep -Seconds 5  # Post-reboot stabilization

    .\Write-Info.ps1 "---- Phase 3: Environment Setup ----" -ForegroundColor Yellow
    $phase3sw = [System.Diagnostics.Stopwatch]::StartNew()
    try {
        & "$dscFolder\Invoke-SutImperativeSteps.ps1" -Step 3 -WorkingPath $WorkingPath
        $phase3Ok = $true
        .\Write-Info.ps1 "[OK] Environment setup complete in $([math]::Round($phase3sw.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Environment setup failed: $($_.Exception.Message)"
    }
    $phase3sw.Stop()

    # -- Summary --
    Set-DeployStep -Step 3
    Set-RebootCount -Count 0

    $stopwatch.Stop()
    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
    .\Write-Info.ps1 "  SUT Deployment Complete" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Total time : $([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min" -ForegroundColor Cyan
    .\Write-Info.ps1 "    Environment: $([math]::Round($phase3sw.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan

    if ($phase3Ok) {
        "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
        .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
        Remove-ResumeTask
    } else {
        .\Write-Info.ps1 "[WARN] Signal file NOT written -- deployment incomplete" -ForegroundColor Yellow
    }

    $cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"
    if (Test-Path $cleanupScript) {
        & $cleanupScript
    }
}

Pop-Location
Stop-Transcript
