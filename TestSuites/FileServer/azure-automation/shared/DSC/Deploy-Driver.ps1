# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Orchestrator for the Driver Computer (Client01).
    Applies DSC, joins domain, reboots, then installs tools, configures PTF config,
    ForceLevel2, and schedules test execution.
    Works for Domain, Cluster, and Workgroup scenarios.

.DESCRIPTION
    Uses registry-based step tracking with deferred reboots:

    Step 0 -> 1: Pre-Domain-Join
      DSC: Hosts file, firewall, PS remoting, password never expires, multi-NIC routing.
      Imperative: Domain join via domainjoin.ps1.
      -> Deferred reboot (TKFRSAR startup task + 90s shutdown)

    Step 1 -> 2: Tools + PTF Config + RSA Keys + ForceLevel2
      DSC: Verify the persisted configuration and repair only when drifted.
      Imperative: Tools install (DotNetCore, OpenSSH, PowerShellCore, PTMService,
                  PTMCli, TestSuite, certs), cluster ptfconfig patching (if applicable),
                  RSA key copy, sshd restart, ForceLevel2 via ShareUtil.exe.
      Phase 3: Schedule test run (fire-and-forget).
      -> Finish (signal file written)

    Re-running is safe. DSC only touches drifted state and imperative steps
    have their own idempotency checks.

.PARAMETER WorkingPath
    Path to the package root folder (Domain-Package or Cluster-Package).

.PARAMETER SkipForceLevel2
    Skip the ForceLevel2 configuration. Useful when SUT is not ready yet.

.EXAMPLE
    .\Deploy-Driver.ps1
    .\Deploy-Driver.ps1 -SkipForceLevel2
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [switch]$SkipForceLevel2
)

$ErrorActionPreference = 'Stop'
$isLinuxDriver = $IsLinux -eq $true
$dscFolder    = $PSScriptRoot
$scriptsPath  = Join-Path $dscFolder 'Scripts'
$mofFolder    = Join-Path (Join-Path $dscFolder 'MOF') 'Driver'
$logFile      = Join-Path $dscFolder 'Deploy-Driver.log'

# Put Scripts folder on PATH so Write-Info.ps1, Write-Error.ps1 etc. resolve via .\
$env:Path += "$([IO.Path]::PathSeparator)$scriptsPath"
Push-Location $scriptsPath

Start-Transcript -Path $logFile -Append -Force
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "  Driver (Client01) -- DSC + Imperative Deployment         " -ForegroundColor Cyan
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
# Reboot circuit breaker + Step tracking (Windows: registry, Linux: no-op)
# ===========================================================================
$maxRebootCount = 3

if ($isLinuxDriver) {
    function Get-RebootCount { return 0 }
    function Set-RebootCount { param([int]$Count) }
    function Get-DeployStep { return 99 }   # Skip straight to tools/test on Linux
    function Set-DeployStep { param([int]$Step) }
} else {
    $rebootRegPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    $rebootRegName = 'RebootCount'
    $stepRegName   = 'DeployStep'

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
}

$currentStep = Get-DeployStep
.\Write-Info.ps1 "Current deploy step: $currentStep" -ForegroundColor DarkGray

# Load shared helpers (reboot scheduling, TKFRSAR cleanup)
. "$dscFolder\Deploy-CommonHelpers.ps1"

# Cancel any stale reboot task from a previous run so it doesn't fire
# in the middle of feature installation (0x8007045b).
if (-not $isLinuxDriver) {
    $staleReboot = Get-ScheduledTask -TaskName 'PostDeployReboot' -ErrorAction SilentlyContinue
    if ($null -ne $staleReboot) {
        Unregister-ScheduledTask -TaskName 'PostDeployReboot' -Confirm:$false
        .\Write-Info.ps1 "[OK] Cancelled stale PostDeployReboot task from previous run." -ForegroundColor Yellow
    }
}

$signalFile = "$dscFolder\Deploy-Driver.Completed.signal"
$driverDscSignal = "$dscFolder\Driver-DSC.Completed.signal"
if (Test-Path $signalFile) {
    .\Write-Info.ps1 "[OK] Driver deployment already completed (signal file exists)." -ForegroundColor Green
    if (-not $isLinuxDriver) { Remove-ResumeTask }
    Pop-Location; Stop-Transcript; return
}

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$phase1Ok = $false
$phase2Ok = $false

# ===========================================================================
# Pre-check: Validate hostname (Windows only -- Linux hostname set by cloud-init)
# ===========================================================================
if (-not $isLinuxDriver -and (Test-Path $configFile)) {
    try {
        $cfg = Get-Content -Path $configFile -Raw | ConvertFrom-Json
        $expectedName = $cfg.Machines.DriverComputer.ComputerName
        if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
            $currentRebootCount = Get-RebootCount
            if ($currentRebootCount -ge $maxRebootCount) {
                .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered ($currentRebootCount >= $maxRebootCount). Skipping rename reboot -- continuing with hostname '$env:COMPUTERNAME'." -ForegroundColor Red
            } else {
                Set-RebootCount -Count ($currentRebootCount + 1)
                .\Write-Info.ps1 "Renaming computer from $env:COMPUTERNAME to $expectedName (reboot $($currentRebootCount + 1)/$maxRebootCount)..." -ForegroundColor Yellow
                Rename-Computer -NewName $expectedName -Force

                .\Write-Info.ps1 "Scheduling Deploy-Driver.ps1 to re-run after reboot..." -ForegroundColor Yellow
                Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Driver.ps1" `
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
# Step 0 -> 1: DSC + Domain Join
# ===========================================================================
if ($currentStep -lt 1) {
    .\Write-Info.ps1 "---- Phase 1a: DSC Configuration ----" -ForegroundColor Yellow
    $phase1 = [System.Diagnostics.Stopwatch]::StartNew()

    try {
        . "$dscFolder\Driver-Configuration.ps1"
        .\Write-Info.ps1 "Compiling Driver DSC configuration (config: $configFile)..." -ForegroundColor Cyan
        DriverConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
        .\Write-Info.ps1 "Applying Driver DSC configuration..." -ForegroundColor Yellow
        Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
        $phase1Ok = $true
        "DRIVER DSC APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $driverDscSignal -Force
        .\Write-Info.ps1 "[OK] DSC applied in $([math]::Round($phase1.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] DSC failed: $($_.Exception.Message)"
        .\Write-Info.ps1 "Continuing with domain join..." -ForegroundColor Yellow
    }
    $phase1.Stop()
    .\Write-Info.ps1 ""

    # Imperative Step 1 -- Domain Join
    .\Write-Info.ps1 "---- Phase 1b: Domain Join ----" -ForegroundColor Yellow
    try {
        & "$dscFolder\Invoke-DriverImperativeSteps.ps1" -Step 1 -WorkingPath $WorkingPath
        .\Write-Info.ps1 "[OK] Domain join complete." -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Domain join failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; return
    }

    # Detect workgroup mode -- no reboot needed if domain join was skipped
    $cfgForReboot = Get-Content -Path $configFile -Raw | ConvertFrom-Json
    $domNameReboot = if ($cfgForReboot.Core) { $cfgForReboot.Core.DomainName } else { $null }
    $isWorkgroup = [string]::IsNullOrWhiteSpace($domNameReboot) -or $domNameReboot -eq 'Workgroup'

    if ($isWorkgroup) {
        .\Write-Info.ps1 "Workgroup mode -- skipping reboot, proceeding to Step 2..." -ForegroundColor Yellow
        Set-DeployStep -Step 1
        # Fall through to Step 1 -> 2 below
    } else {
        # Schedule deferred reboot + resume
        Set-DeployStep -Step 1
        .\Write-Info.ps1 "Scheduling deferred reboot and resume..." -ForegroundColor Yellow
        # After domain join, resume as domain admin for cross-machine operations
        $domainNetBios = if ($cfg.Domain -and $cfg.Domain.NetBiosName) { $cfg.Domain.NetBiosName } else { $cfg.Core.DomainName.Split('.')[0].ToUpper() }
        $domainAdminUser = "$domainNetBios\$($cfg.Core.Username)"
        $domainAdminPass = $cfg.Core.Password
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Driver.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder `
            -RunAsUser $domainAdminUser -RunAsPassword $domainAdminPass

        Pop-Location
        Stop-Transcript
        return
    }
}

# ===========================================================================
# Step 1 -> 2: Tools + RSA Keys + ForceLevel2 + Test Scheduling
# ===========================================================================
$currentStep = Get-DeployStep
if ($currentStep -eq 1) {
    Start-Sleep -Seconds 5  # Post-reboot stabilization

    # Verify the successful pre-join application. Repair only when the persisted MOF
    # reports drift or the success marker is absent.
    .\Write-Info.ps1 "---- Phase 2a: DSC Drift Check ----" -ForegroundColor Yellow
    $phase2a = [System.Diagnostics.Stopwatch]::StartNew()
    $driverDscReady = $false
    try {
        $mofPath = Join-Path $mofFolder 'localhost.mof'
        $inDesiredState = $false
        if ((Test-Path $driverDscSignal) -and (Test-Path $mofPath)) {
            .\Write-Info.ps1 "Checking Driver DSC drift against the persisted MOF..." -ForegroundColor Cyan
            $inDesiredState = [bool](Test-DscConfiguration -Path $mofFolder -ErrorAction Stop)
        }

        if ($inDesiredState) {
            $driverDscReady = $true
            .\Write-Info.ps1 "[OK] Driver DSC remains in desired state; re-apply skipped." -ForegroundColor Green
        } else {
            . "$dscFolder\Driver-Configuration.ps1"
            .\Write-Info.ps1 "Driver DSC is missing or drifted; compiling repair configuration..." -ForegroundColor Yellow
            DriverConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
            Start-DscConfiguration -Path $mofFolder -Wait -Verbose -Force
            "DRIVER DSC APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $driverDscSignal -Force
            $driverDscReady = $true
            .\Write-Info.ps1 "[OK] Driver DSC repaired in $([math]::Round($phase2a.Elapsed.TotalSeconds))s" -ForegroundColor Green
        }
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Driver DSC verification/repair failed: $($_.Exception.Message)"
    }
    $phase2a.Stop()
    .\Write-Info.ps1 ""

    # Imperative Step 2 -- Tools, RSA keys, ForceLevel2
    .\Write-Info.ps1 "---- Phase 2b: Tools + RSA Keys + ForceLevel2 ----" -ForegroundColor Yellow
    $phase2b = [System.Diagnostics.Stopwatch]::StartNew()
    $imperativeArgs = @{
        Step        = 2
        WorkingPath = $WorkingPath
    }
    if ($SkipForceLevel2) {
        $imperativeArgs['SkipForceLevel2'] = $true
    }

    if ($driverDscReady) {
        try {
            & "$dscFolder\Invoke-DriverImperativeSteps.ps1" @imperativeArgs
            $phase2Ok = $true
            .\Write-Info.ps1 "[OK] Driver configuration complete in $([math]::Round($phase2b.Elapsed.TotalSeconds))s" -ForegroundColor Green
        }
        catch {
            .\Write-Error.ps1 "[FAIL] Driver configuration failed: $($_.Exception.Message)"
        }
    } else {
        .\Write-Info.ps1 "[SKIP] Driver tools and test setup blocked because DSC is not ready." -ForegroundColor Yellow
    }
    $phase2b.Stop()
    .\Write-Info.ps1 ""

    # ===========================================================================
    # Phase 3: Schedule test run (fire-and-forget)
    # ===========================================================================
    .\Write-Info.ps1 "---- Phase 3: Schedule Test Run ----" -ForegroundColor Yellow
    $phase3 = [System.Diagnostics.Stopwatch]::StartNew()

    $testRunScript = Join-Path $scriptsPath 'Invoke-TestRun.ps1'
    if (-not $phase2Ok) {
        $phase3.Stop()
        .\Write-Info.ps1 "[SKIP] Not scheduling test run -- earlier phases failed" -ForegroundColor Yellow
    } elseif (-not (Test-Path $testRunScript)) {
        $phase3.Stop()
        .\Write-Info.ps1 "[SKIP] Invoke-TestRun.ps1 not found at $testRunScript" -ForegroundColor Yellow
    } else {
        .\Write-Info.ps1 "Scheduling FileServer test run (dotnet test)..." -ForegroundColor Cyan

        $cfgJson = Get-Content -Path $configFile -Raw | ConvertFrom-Json
        $taskUser = $cfgJson.Core.Username
        $taskPassword = $cfgJson.Core.Password

        # Use NetBIOS domain name (e.g., CONTOSO\testadmin) for the scheduled task user.
        # This is more reliable for Kerberos/TGT acquisition than FQDN format (contoso.com\testadmin).
        # Prefer Domain.NetBiosName, fall back to Core.DomainName.
        $domainPrefix = if ($cfgJson.Domain -and $cfgJson.Domain.NetBiosName) {
            $cfgJson.Domain.NetBiosName
        } elseif (-not [string]::IsNullOrWhiteSpace($cfgJson.Core.DomainName) -and $cfgJson.Core.DomainName -ne 'Workgroup') {
            $cfgJson.Core.DomainName
        } else {
            ''
        }
        if ($domainPrefix) {
            $taskUser = "$domainPrefix\$taskUser"
        }
        .\Write-Info.ps1 "Task user: $taskUser (domainPrefix=$domainPrefix)" -ForegroundColor DarkGray

        if ([string]::IsNullOrWhiteSpace($taskUser)) {
            throw "Config.json Core.Username is empty -- cannot register scheduled task."
        }
        if ([string]::IsNullOrWhiteSpace($taskPassword)) {
            throw "Config.json Core.Password is empty -- cannot register scheduled task."
        }

        # Resolve pwsh.exe
        $pwshExe = (Get-Command pwsh.exe -ErrorAction SilentlyContinue).Source
        if (-not $pwshExe) {
            $pwshExe = @(
                "$env:ProgramFiles\PowerShell\7\pwsh.exe",
                "${env:ProgramFiles(x86)}\PowerShell\7\pwsh.exe"
            ) | Where-Object { Test-Path $_ } | Select-Object -First 1
        }
        if (-not $pwshExe) {
            throw "pwsh.exe not found. Ensure PowerShell 7 is installed before scheduling test execution."
        }
        .\Write-Info.ps1 "Resolved pwsh.exe: $pwshExe" -ForegroundColor DarkGray

        $action = New-ScheduledTaskAction -Execute $pwshExe `
            -Argument "-File `"$testRunScript`" -WorkingPath `"$WorkingPath`""
        $triggerStartup = New-ScheduledTaskTrigger -AtStartup
        $triggerOnce    = New-ScheduledTaskTrigger -Once -At (Get-Date).AddSeconds(30)
        $settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries `
            -StartWhenAvailable
        Register-ScheduledTask -TaskName 'RunFileServerTests' -Action $action `
            -Trigger @($triggerStartup, $triggerOnce) `
            -Settings $settings -User $taskUser -Password $taskPassword -RunLevel Highest `
            -Force -ErrorAction Stop | Out-Null

        $phase3.Stop()
        .\Write-Info.ps1 "[OK] Test run scheduled (fires in ~30s as '$taskUser') -- $([math]::Round($phase3.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    .\Write-Info.ps1 ""

    # ===========================================================================
    # Summary
    # ===========================================================================
    if ($phase2Ok) {
        Set-DeployStep -Step 2
        Set-RebootCount -Count 0
    }

    $stopwatch.Stop()
    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Driver Deployment Complete" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Total time  : $([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min" -ForegroundColor Cyan
    .\Write-Info.ps1 "    DSC       : $([math]::Round($phase2a.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "    Imperative: $([math]::Round($phase2b.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "    Test sched: $([math]::Round($phase3.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan

    # Only write signal file if all phases succeeded, including ForceLevel2.
    # ForceLevel2 writes its own signal file on success; if missing, the Driver
    # deployment is incomplete and should re-run on next boot.
    $fl2SignalFile = "$dscFolder\ForceLevel2.Completed.signal"
    $fl2Required = -not $isLinuxDriver -and -not $SkipForceLevel2
    $fl2Done = -not $fl2Required -or (Test-Path $fl2SignalFile)

    if ($phase2Ok -and $fl2Done) {
        "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
        .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
        Remove-ResumeTask
    } elseif ($phase2Ok -and -not $fl2Done) {
        .\Write-Info.ps1 "[WARN] Signal file NOT written -- ForceLevel2 not yet confirmed. Deployment will re-run on next boot." -ForegroundColor Yellow
    } else {
        .\Write-Info.ps1 "[WARN] Signal file NOT written -- deployment incomplete" -ForegroundColor Yellow
    }

    $cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"
    if (Test-Path $cleanupScript) {
        & $cleanupScript
    }
}

# ===========================================================================
# Step 2+: Re-run after ForceLevel2 retry task has completed
# ===========================================================================
# When Deploy-Driver re-runs with currentStep >= 2, it means Step 1->2 already
# completed but the signal file wasn't written because ForceLevel2 hadn't
# finished yet. Check if the Config-ForceLevel2 scheduled task has since
# written ForceLevel2.Completed.signal and finalize the deployment.
$currentStep = Get-DeployStep
if (-not $isLinuxDriver -and $currentStep -ge 2 -and -not (Test-Path $signalFile)) {
    $fl2SignalFile = "$dscFolder\ForceLevel2.Completed.signal"
    $fl2Required = -not $SkipForceLevel2
    $fl2Done = -not $fl2Required -or (Test-Path $fl2SignalFile)

    if ($fl2Done) {
        "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
        .\Write-Info.ps1 "[OK] ForceLevel2 now confirmed. Signal file written: $signalFile" -ForegroundColor Green
        Remove-ResumeTask
    } else {
        .\Write-Info.ps1 "[WARN] ForceLevel2 still not confirmed. Will check again on next boot." -ForegroundColor Yellow
    }
}

# ===========================================================================
# Linux flow: No DSC, no domain join, no reboot -- straight to tools + test
# ===========================================================================
if ($isLinuxDriver) {
    .\Write-Info.ps1 "---- Linux Driver: Tools + Test Run ----" -ForegroundColor Cyan
    $linuxPhase = [System.Diagnostics.Stopwatch]::StartNew()

    # Imperative tools install (Step 2 flow in the imperative script)
    $imperativeArgs = @{
        Step        = 2
        WorkingPath = $WorkingPath
    }
    if ($SkipForceLevel2) { $imperativeArgs['SkipForceLevel2'] = $true }

    try {
        & "$dscFolder\Invoke-DriverImperativeSteps.ps1" @imperativeArgs
        $phase2Ok = $true
        .\Write-Info.ps1 "[OK] Linux driver tools completed in $([math]::Round($linuxPhase.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Linux driver tools failed: $($_.Exception.Message)"
    }
    $linuxPhase.Stop()

    # Launch test run in background via nohup
    $testRunScript = Join-Path $scriptsPath 'Invoke-TestRun.ps1'
    if ($phase2Ok -and (Test-Path $testRunScript)) {
        $pwshExe = (Get-Command pwsh -ErrorAction SilentlyContinue).Source
        if (-not $pwshExe) { $pwshExe = '/usr/bin/pwsh' }
        $testRunLog = Join-Path $dscFolder 'Invoke-TestRun.log'
        & bash -c "nohup '$pwshExe' -File '$testRunScript' -WorkingPath '$WorkingPath' > '$testRunLog' 2>&1 &"
        .\Write-Info.ps1 "[OK] Test run launched in background (log: $testRunLog)" -ForegroundColor Green
    }

    $stopwatch.Stop()
    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Linux Driver Deployment Complete" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Total time: $([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min" -ForegroundColor Cyan
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan

    $signalFile = Join-Path $dscFolder 'Deploy-Driver.Completed.signal'
    if ($phase2Ok) {
        "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
        .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
    }
}

Pop-Location
Stop-Transcript
