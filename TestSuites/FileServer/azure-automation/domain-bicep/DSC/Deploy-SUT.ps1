# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Orchestrator for the Domain SUT (Node01).
    Applies DSC, joins domain, reboots, installs features/tools, configures environment.

.DESCRIPTION
    Uses registry-based step tracking with deferred reboots (matching workgroup patterns):

    Step 0 -> 1: Domain Join
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
# Persisted marker: Full DSC (features + shares + registry) is a mandatory postcondition,
# but it runs in an earlier boot than the completion-signal write, so an in-memory flag is
# lost across the reboot. Gate the completion signal on this marker so a failed Full DSC
# (e.g. missing SMB shares) does not produce a green-but-broken SUT.
$fullDscSignal = "$scriptsPath\SUT-FullDsc.Completed.signal"
if (Test-Path $signalFile) {
    .\Write-Info.ps1 "[OK] SUT deployment already completed (signal file exists)." -ForegroundColor Green
    Remove-ResumeTask
    Pop-Location; Stop-Transcript; return
}

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
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
# Step 0 -> 1: Domain Join
# ===========================================================================
if ($currentStep -lt 1) {
    # The complete SUT configuration includes features and shares that are valid only
    # after domain join. Running it here was not a "lite" pass; it applied the full MOF,
    # tolerated failures, and then repeated all resources after reboot.
    .\Write-Info.ps1 "---- Phase 1: Domain Join ----" -ForegroundColor Yellow
    try {
        # Capture the imperative result: Step 1 returns $false (not throw) on join failure,
        # so an unchecked call would advance the step on a machine that never joined.
        $joinResult = & "$dscFolder\Invoke-SutImperativeSteps.ps1" -Step 1 -WorkingPath $WorkingPath | Select-Object -Last 1
        if ($joinResult -ne $true) {
            throw "Domain join step returned failure (SUT did not join)."
        }
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
        # Clear any stale marker from a previous attempt so a FAILED apply in this run cannot
        # be mistaken for success by the completion gate (which only tests for the file).
        if (Test-Path $fullDscSignal) { Remove-Item -Path $fullDscSignal -Force -ErrorAction SilentlyContinue }
        .\Write-Info.ps1 "Applying full SUT DSC configuration..." -ForegroundColor Yellow
        Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
        "FULL DSC APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $fullDscSignal -Force
        .\Write-Info.ps1 "[OK] Full DSC applied in $([math]::Round($phase2b.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Full DSC failed: $($_.Exception.Message)"
        .\Write-Info.ps1 "Continuing with remaining steps (completion signal will be withheld until Full DSC succeeds)..." -ForegroundColor Yellow
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

    # Postcondition (domain mode): the SUT must actually be domain-joined with a working
    # secure channel before it is declared test-ready. Test-ComputerSecureChannel is used as a
    # gate (must be true to signal ready); its known false-positive bias only risks a missed
    # block, never a false block, so it is safe in this direction.
    $membershipOk = $true
    $domainMode = -not [string]::IsNullOrWhiteSpace($cfg.Core.DomainName) -and $cfg.Core.DomainName -ne 'Workgroup'
    if ($domainMode) {
        $partOfDomain = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
        $secureOk = $false
        try { $secureOk = Test-ComputerSecureChannel -ErrorAction Stop } catch { $secureOk = $false }
        $membershipOk = $partOfDomain -and $secureOk
        if (-not $membershipOk) {
            .\Write-Info.ps1 "[FAIL] Domain membership/secure-channel postcondition not met (PartOfDomain=$partOfDomain, SecureChannel=$secureOk)." -ForegroundColor Red
        }
    }

    if ($phase3Ok -and (Test-Path $fullDscSignal) -and $membershipOk) {
        "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
        .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
        Remove-ResumeTask
    } elseif (-not $membershipOk) {
        .\Write-Info.ps1 "[WARN] Signal file NOT written -- domain membership/secure channel not verified; SUT is not test-ready" -ForegroundColor Yellow
    } elseif (-not (Test-Path $fullDscSignal)) {
        .\Write-Info.ps1 "[WARN] Signal file NOT written -- Full DSC (features/shares/registry) did not succeed; SUT is not test-ready" -ForegroundColor Yellow
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
