# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Orchestrator for the Workgroup SUT (Node01).
    Compiles & applies DSC, installs tools, then runs imperative steps.

.DESCRIPTION
    This replaces Configure_SUT.ps1 for the Workgroup scenario with a
    DSC-first approach:

    Phase 1 -- DSC (declarative, idempotent)
      Windows features (batched), firewall off, PS remoting, hosts file,
      SMB shares, registry keys, FSRM classification, computer password,
      password never expires, SMB require-signing.

    Phase 2 -- Tools install (existing InstallMSIAndTools.ps1 as background job)
      Downloads & installs PowerShellCore, OpenSSH, Windows Admin Center.

    Phase 3 -- Imperative steps (idempotent checks built-in)
      Disk partitioning (ReFS K:, FAT32 J:), symbolic links, mount points,
      shadow copies, DFS namespaces, QUIC certificate mapping.

    Re-running is safe. DSC only touches drifted state, the tools install
    checks its signal file, and imperative steps check for existing state.

.PARAMETER WorkingPath
    Path to the Workgroup-Package root folder.

.EXAMPLE
    .\Deploy-SUT.ps1
    .\Deploy-SUT.ps1 -WorkingPath D:\ISOs\ManualSetup\Packages\Workgroup-Package
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
.\Write-Info.ps1 "  Workgroup SUT (Node01) -- DSC + Imperative Deployment    " -ForegroundColor Cyan
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
# Pre-check: Validate hostname (rename if needed, schedule reboot)
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
# Phase 1: DSC -- Compile and Apply
# ===========================================================================
.\Write-Info.ps1 "---- Phase 1: DSC Configuration ----" -ForegroundColor Yellow
$phase1 = [System.Diagnostics.Stopwatch]::StartNew()

try {
    . "$dscFolder\SUT-Configuration.ps1"

    $configFile = "$WorkingPath\Config.json"
    .\Write-Info.ps1 "Compiling SUT DSC configuration (config: $configFile)..." -ForegroundColor Cyan
    SutConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder

    .\Write-Info.ps1 "Applying SUT DSC configuration (this may take several minutes)..." -ForegroundColor Yellow
    Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
    $phase1Ok = $true
    .\Write-Info.ps1 "[OK] DSC applied in $([math]::Round($phase1.Elapsed.TotalSeconds))s" -ForegroundColor Green
}
catch {
    .\Write-Error.ps1 "[FAIL] DSC failed: $($_.Exception.Message)"
    .\Write-Info.ps1 "Attempting to continue with imperative steps..." -ForegroundColor Yellow
}
$phase1.Stop()
.\Write-Info.ps1 ""

# Check if DSC triggered a pending reboot (e.g., Hyper-V feature installation).
# If so, schedule a resume and return early -- Phase 2/3 will run after reboot
# when DSC has fully completed and directories like C:\SMBBasic exist.
$rebootAfterDsc = (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\RebootPending') -or
                  (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update\RebootRequired') -or
                  (Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations')
if ($rebootAfterDsc) {
    $currentRebootCount = Get-RebootCount
    if ($currentRebootCount -ge $maxRebootCount) {
        .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered ($currentRebootCount >= $maxRebootCount). Continuing without reboot." -ForegroundColor Red
    } else {
        Set-RebootCount -Count ($currentRebootCount + 1)
        .\Write-Info.ps1 "[WARN] DSC requires a reboot to complete feature installation (reboot $($currentRebootCount + 1)/$maxRebootCount)." -ForegroundColor Yellow
        .\Write-Info.ps1 "Deferring Phase 2/3 until after reboot..." -ForegroundColor Yellow
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-SUT.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder

        Pop-Location
        Stop-Transcript
        return
    }
}

# ===========================================================================
# Phase 2: Tools Install (background job for parallelism)
# ===========================================================================
.\Write-Info.ps1 "---- Phase 2: Tools Install (background) ----" -ForegroundColor Yellow
$phase2 = [System.Diagnostics.Stopwatch]::StartNew()

$toolsSignal = "$scriptsPath\InstallMSIAndTools.Completed.signal"
$toolsJob = $null

if (Test-Path $toolsSignal) {
    $phase2.Stop()
    .\Write-Info.ps1 "[OK] Tools already installed (signal file exists)" -ForegroundColor Green
} else {
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
    # Pause the timer -- it will resume when we wait for the job after Phase 3
    $phase2.Stop()
}

# ===========================================================================
# Phase 3: Imperative Steps (runs while tools install in background)
# ===========================================================================
.\Write-Info.ps1 "---- Phase 3: Imperative Steps ----" -ForegroundColor Yellow
$phase3 = [System.Diagnostics.Stopwatch]::StartNew()

try {
    & "$dscFolder\Invoke-SutImperativeSteps.ps1" -WorkingPath $WorkingPath
    $phase3Ok = $true
    .\Write-Info.ps1 "[OK] Imperative steps completed in $([math]::Round($phase3.Elapsed.TotalSeconds))s" -ForegroundColor Green
}
catch {
    .\Write-Error.ps1 "[FAIL] Imperative steps failed: $($_.Exception.Message)"
}
$phase3.Stop()
.\Write-Info.ps1 ""

# ===========================================================================
# Wait for tools install job to finish
# ===========================================================================
if ($null -ne $toolsJob) {
    $phase2.Start()  # Resume timer (paused after job was started)
    .\Write-Info.ps1 "Waiting for tools install to complete..." -ForegroundColor Yellow
    # Wait-Job + Remove-Job instead of Receive-Job to avoid CLIXML deserialization
    # errors. The job redirects all output to a log file (*> $jl), so we don't
    # need its deserialized output. Start-Transcript and CMD /C calls inside
    # the job can leak raw text into the CLIXML stream, causing:
    # "Cannot process an element with node type 'Text'"
    $toolsJob | Wait-Job | Remove-Job -Force
    $phase2.Stop()
    if (Test-Path $toolsSignal) {
        .\Write-Info.ps1 "[OK] Tools installed in $([math]::Round($phase2.Elapsed.TotalSeconds))s" -ForegroundColor Green
    } else {
        .\Write-Info.ps1 "[WARN] Tools job completed but signal file not found -- check InstallMSIAndTools.ps1.log" -ForegroundColor Yellow
    }
}

# ===========================================================================
# Summary
# ===========================================================================
$stopwatch.Stop()
.\Write-Info.ps1 ""
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "  SUT Deployment Complete" -ForegroundColor Cyan
.\Write-Info.ps1 "  Total time : $([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min" -ForegroundColor Cyan
.\Write-Info.ps1 "    DSC      : $([math]::Round($phase1.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
.\Write-Info.ps1 "    Tools    : $([math]::Round($phase2.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
.\Write-Info.ps1 "    Imperative: $([math]::Round($phase3.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan

# Check if a reboot is pending (e.g., from Hyper-V feature installation)
$rebootPending = (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\RebootPending') -or
                 (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update\RebootRequired') -or
                 (Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations')
if ($rebootPending) {
    $currentRebootCount = Get-RebootCount
    if ($currentRebootCount -ge $maxRebootCount) {
        .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered ($currentRebootCount >= $maxRebootCount). Skipping pending-reboot -- continuing without reboot." -ForegroundColor Red
    } else {
        Set-RebootCount -Count ($currentRebootCount + 1)
        .\Write-Info.ps1 "[WARN] A reboot is required to complete feature installation (reboot $($currentRebootCount + 1)/$maxRebootCount)." -ForegroundColor Yellow
        .\Write-Info.ps1 'Scheduling Deploy-SUT.ps1 to re-run after reboot...' -ForegroundColor Yellow
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-SUT.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder

        Pop-Location
        Stop-Transcript
        return
    }
} else {
    .\Write-Info.ps1 'No reboot required.' -ForegroundColor Green
}

# Reset reboot circuit breaker on successful completion
Set-RebootCount -Count 0

if ($phase1Ok -and $phase3Ok) {
    "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
    .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
    Remove-ResumeTask
} else {
    .\Write-Info.ps1 "[WARN] Signal file NOT written -- deployment incomplete (DSC=$phase1Ok, Imperative=$phase3Ok)" -ForegroundColor Yellow
}

$cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"
if (Test-Path $cleanupScript) {
    & $cleanupScript
}

Pop-Location
Stop-Transcript
