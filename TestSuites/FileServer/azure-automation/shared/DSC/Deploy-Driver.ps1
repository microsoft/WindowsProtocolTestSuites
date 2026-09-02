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
      Imperative: Prepare/install machine-level tools, then domain join via domainjoin.ps1.
      -> Deferred domain-join reboot, or a Workgroup tool-stabilization reboot.

    Step 1 -> 2: PTF Config + RSA Keys + ForceLevel2
      DSC: Verify the persisted configuration and repair only when drifted.
      Imperative: Verify tools, apply cluster ptfconfig changes (if applicable),
                  copy RSA keys, restart sshd, and configure ForceLevel2 via ShareUtil.exe.
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
$heartbeatFile = Join-Path $dscFolder 'Deploy-Driver.heartbeat.json'

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
$testAutoRun = if ($null -eq $cfg -or $null -eq $cfg.TestExecution -or
    $null -eq $cfg.TestExecution.AutoRun) {
    $true
} else {
    [Convert]::ToBoolean("$($cfg.TestExecution.AutoRun)")
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
        Pop-Location; Stop-Transcript; throw
    }
}

if ($null -ne $cfg -and $cfg.Core.Scenario -eq 'Cluster') {
    Pop-Location
    Stop-Transcript
    & (Join-Path $dscFolder 'Deploy-ClusterDriver.ps1') `
        -WorkingPath $WorkingPath
    return
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
$maxRebootCount = 1

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

$signalFile = "$dscFolder\Deploy-Driver.Completed.signal"
$driverDscSignal = "$dscFolder\Driver-DSC.Completed.signal"
$driverToolsSignal = "$scriptsPath\InstallMSIAndTools.Completed.signal"
$driverToolsPreparedSignal = "$scriptsPath\InstallMSIAndTools.Prepared.signal"
$driverToolsInstaller = "$scriptsPath\InstallMSIAndTools.ps1"
$forceLevel2Signal = "$dscFolder\ForceLevel2.Completed.signal"
$joinRebootPendingName = 'DriverJoinRebootPending'
$joinPreBootTimeName = 'DriverJoinPreRebootBootTimeUtc'
$joinRebootCountName = 'DriverJoinRebootCount'
$toolsRebootPendingName = 'DriverToolsRebootPending'
$toolsPreBootTimeName = 'DriverToolsPreRebootBootTimeUtc'
$toolsRebootCountName = 'DriverToolsRebootCount'
$toolsRebootScheduleRetryName = 'DriverToolsRebootScheduleRetryCount'
$joinRebootScheduleRetryName = 'DriverJoinRebootScheduleRetryCount'
$toolsJobTimeoutSeconds = 3600

# Load shared helpers (reboot scheduling, TKFRSAR cleanup)
. "$dscFolder\Deploy-CommonHelpers.ps1"

function Stop-DriverRecoveryTasks {
    foreach ($taskName in @('TKFRSAR', 'Config-ForceLevel2', 'PostDeployReboot')) {
        $task = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
        if ($null -ne $task) {
            Unregister-ScheduledTask -TaskName $taskName -Confirm:$false -ErrorAction SilentlyContinue
            .\Write-Info.ps1 "[OK] Unregistered terminal recovery task '$taskName'." -ForegroundColor Yellow
        }
    }
}

function Test-PostDeployRebootCanStillRun {
    $task = Get-ScheduledTask -TaskName 'PostDeployReboot' -ErrorAction SilentlyContinue
    if ($null -eq $task -or "$($task.State)" -eq 'Disabled') {
        return $false
    }
    if ("$($task.State)" -eq 'Running') {
        return $true
    }
    $taskInfo = Get-ScheduledTaskInfo -TaskName 'PostDeployReboot' -ErrorAction SilentlyContinue
    return $null -ne $taskInfo -and $taskInfo.NextRunTime -gt (Get-Date)
}

# Cancel any stale reboot task from a previous run so it doesn't fire
# in the middle of feature installation (0x8007045b).
if (-not $isLinuxDriver) {
    $toolsRebootPending = [int](Get-DeploymentRegistryValue `
        -Name $toolsRebootPendingName -DefaultValue 0 -RegistryPath $rebootRegPath)
    $joinRebootPending = [int](Get-DeploymentRegistryValue `
        -Name $joinRebootPendingName -DefaultValue 0 -RegistryPath $rebootRegPath)
    $staleReboot = Get-ScheduledTask -TaskName 'PostDeployReboot' -ErrorAction SilentlyContinue
    if ($null -ne $staleReboot -and $toolsRebootPending -ne 1 -and $joinRebootPending -ne 1) {
        Unregister-ScheduledTask -TaskName 'PostDeployReboot' -Confirm:$false
        .\Write-Info.ps1 "[OK] Cancelled stale PostDeployReboot task from previous run." -ForegroundColor Yellow
    }

    if ($toolsRebootPending -eq 1) {
        $recordedBoot = [datetime](Get-ItemPropertyValue -Path $rebootRegPath `
            -Name $toolsPreBootTimeName -ErrorAction Stop)
        $currentBoot = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime
        if ($currentBoot.ToUniversalTime() -le $recordedBoot.ToUniversalTime()) {
            if (-not (Test-PostDeployRebootCanStillRun)) {
                $scheduleRetryCount = [int](Get-DeploymentRegistryValue `
                    -Name $toolsRebootScheduleRetryName -DefaultValue 0 `
                    -RegistryPath $rebootRegPath)
                if ($scheduleRetryCount -ge 1) {
                    Stop-DriverRecoveryTasks
                    Pop-Location
                    Stop-Transcript
                    throw 'The Driver tool-stabilization reboot task failed after one bounded reschedule attempt.'
                }
                Set-ItemProperty -Path $rebootRegPath -Name $toolsRebootScheduleRetryName `
                    -Value 1 -Type DWord -Force
                $resumePassword = $cfg.Core.Password
                $domainName = $cfg.Core.DomainName
                if ([string]::IsNullOrWhiteSpace($domainName) -or $domainName -eq 'Workgroup') {
                    $resumeUser = "$env:COMPUTERNAME\$($cfg.Core.Username)"
                } else {
                    $domainPrefix = if ($cfg.Domain -and $cfg.Domain.NetBiosName) {
                        $cfg.Domain.NetBiosName
                    } else {
                        $domainName.Split('.')[0].ToUpperInvariant()
                    }
                    $resumeUser = "$domainPrefix\$($cfg.Core.Username)"
                }
                try {
                    Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Driver.ps1" `
                        -WorkingPath $WorkingPath -DscFolder $dscFolder `
                        -RunAsUser $resumeUser -RunAsPassword $resumePassword
                }
                catch {
                    foreach ($taskName in @('TKFRSAR', 'PostDeployReboot')) {
                        Unregister-ScheduledTask -TaskName $taskName -Confirm:$false -ErrorAction SilentlyContinue
                    }
                    Pop-Location
                    Stop-Transcript
                    throw "The persisted Driver tool-stabilization reboot could not be rescheduled: $($_.Exception.Message)"
                }
            }
            .\Write-Info.ps1 '[WAIT] Driver tool-stabilization reboot is scheduled but has not occurred yet.' -ForegroundColor Yellow
            Pop-Location
            Stop-Transcript
            return
        }
        Set-ItemProperty -Path $rebootRegPath -Name $toolsRebootPendingName -Value 0 -Type DWord -Force
        Set-ItemProperty -Path $rebootRegPath -Name $toolsRebootScheduleRetryName -Value 0 -Type DWord -Force
        $remainingReasons = @(Get-PendingSystemRebootReasons)
        if ($remainingReasons.Count -gt 0) {
            Stop-DriverRecoveryTasks
            Pop-Location
            Stop-Transcript
            throw "The Driver remains pending reboot after tool stabilization. Reasons: $($remainingReasons -join ', ')"
        }
        .\Write-Info.ps1 '[OK] Driver tool-stabilization reboot completed.' -ForegroundColor Green
    }
}

function Start-DriverToolsPreparationJob {
    if ($isLinuxDriver -or
        (Test-Path $driverToolsPreparedSignal) -or
        (Test-Path $driverToolsSignal)) {
        return $null
    }
    .\Write-Info.ps1 'Starting parallel Driver tool package preparation...' -ForegroundColor Cyan
    $jobLog = "$scriptsPath\InstallMSIAndTools.prepare.job.log"
    return Start-Job -ScriptBlock {
        param($installer, $scriptsDirectory, $preparedSignal, $log)
        Set-Location $scriptsDirectory
        $env:Path += ";$scriptsDirectory"
        $output = @(& $installer -Role 'DriverComputer' -Operation Prepare `
            -PreparedSignalFile $preparedSignal -NoTranscript *>&1)
        $output | Out-File -FilePath $log -Force
        if ($output.Count -eq 0 -or $output[-1] -ne $true) {
            throw 'Required Driver package preparation failed.'
        }
    } -ArgumentList $driverToolsInstaller, $scriptsPath, $driverToolsPreparedSignal, $jobLog
}

function Stop-DriverToolsPreparationJob {
    param([System.Management.Automation.Job]$Job)
    if ($null -eq $Job) { return }
    if ($Job.State -in @('NotStarted', 'Running', 'Blocked')) { Stop-Job -Job $Job }
    Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
}

function Complete-DriverToolsPreparationJob {
    param([System.Management.Automation.Job]$Job)
    if ($null -eq $Job) { return }
    try {
        Wait-DeploymentJob -Job $Job -TimeoutSeconds $toolsJobTimeoutSeconds `
            -Phase 'ToolsPrepare' -Operation 'Parallel Driver package preparation' `
            -HeartbeatPath $heartbeatFile -LastCheckpoint 'Driver orchestration active' | Out-Null
    }
    finally {
        Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
    }
}

function Register-DriverToolsStabilizationReboot {
    $reasons = @(Get-PendingSystemRebootReasons)
    $reasonSummary = if ($reasons.Count -gt 0) { $reasons -join ', ' } else { 'unknown' }
    $rebootCount = [int](Get-DeploymentRegistryValue `
        -Name $toolsRebootCountName -DefaultValue 0 -RegistryPath $rebootRegPath)
    if ($rebootCount -ge 1) {
        Stop-DriverRecoveryTasks
        Pop-Location
        Stop-Transcript
        throw "The Driver remains pending reboot after its one allowed tool-stabilization reboot. Reasons: $reasonSummary"
    }

    $staleTestTask = Get-ScheduledTask -TaskName 'RunFileServerTests' -ErrorAction SilentlyContinue
    if ($null -ne $staleTestTask) {
        Unregister-ScheduledTask -TaskName 'RunFileServerTests' -Confirm:$false
    }

    Set-DeployStep -Step 1
    Set-ItemProperty -Path $rebootRegPath -Name $toolsRebootCountName -Value 1 -Type DWord -Force
    Set-ItemProperty -Path $rebootRegPath -Name $toolsRebootPendingName -Value 1 -Type DWord -Force
    Set-ItemProperty -Path $rebootRegPath -Name $toolsRebootScheduleRetryName -Value 0 -Type DWord -Force
    $bootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime.ToUniversalTime().ToString('o')
    Set-ItemProperty -Path $rebootRegPath -Name $toolsPreBootTimeName -Value $bootTime -Type String -Force
    .\Write-Info.ps1 "[WARN] Scheduling one Driver tool-stabilization reboot. Reasons: $reasonSummary" -ForegroundColor Yellow

    $resumeUser = $null
    $resumePassword = $null
    if ($null -ne $cfg -and $null -ne $cfg.Core) {
        $resumePassword = $cfg.Core.Password
        $domainName = $cfg.Core.DomainName
        if ([string]::IsNullOrWhiteSpace($domainName) -or $domainName -eq 'Workgroup') {
            $resumeUser = "$env:COMPUTERNAME\$($cfg.Core.Username)"
        } else {
            $domainPrefix = if ($cfg.Domain -and $cfg.Domain.NetBiosName) {
                $cfg.Domain.NetBiosName
            } else {
                $domainName.Split('.')[0].ToUpperInvariant()
            }
            $resumeUser = "$domainPrefix\$($cfg.Core.Username)"
        }
    }
    try {
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Driver.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder `
            -RunAsUser $resumeUser -RunAsPassword $resumePassword
    }
    catch {
        Stop-DriverRecoveryTasks
        Pop-Location
        Stop-Transcript
        throw "Could not schedule the Driver tool-stabilization reboot; persisted state will retry once: $($_.Exception.Message)"
    }
}

function Wait-DriverDomainReadiness {
    param([int]$TimeoutSeconds = 600)
    if ($isLinuxDriver) { return }
    $domainName = if ($cfg.Core) { $cfg.Core.DomainName } else { $null }
    $isWorkgroup = [string]::IsNullOrWhiteSpace($domainName) -or $domainName -eq 'Workgroup'
    if ($isWorkgroup) { return }

    $startedAt = Get-Date
    $deadline = $startedAt.AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        $partOfDomain = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
        $secureChannel = $false
        try { $secureChannel = Test-ComputerSecureChannel -ErrorAction Stop } catch {}
        Write-DeploymentHeartbeat -Phase 'DomainReadiness' `
            -Operation 'Waiting for Driver domain membership and secure channel' `
            -StartedAt $startedAt -Deadline $deadline -HeartbeatPath $heartbeatFile `
            -LastCheckpoint "PartOfDomain=$partOfDomain; SecureChannel=$secureChannel"
        if ($partOfDomain -and $secureChannel) { return }
        Start-Sleep -Seconds 10
    }
    throw "Driver domain membership and secure channel did not become ready within $TimeoutSeconds seconds."
}

function Test-RequiredDriverDscState {
    if ($isLinuxDriver) { return $true }

    $mofPath = Join-Path $mofFolder 'localhost.mof'
    if (-not (Test-Path $mofPath)) { return $false }

    try {
        return [bool](Test-DscConfiguration -Path $mofFolder -ErrorAction Stop)
    }
    catch {
        .\Write-Info.ps1 "[WARN] Driver DSC state verification failed: $($_.Exception.Message)" -ForegroundColor Yellow
        return $false
    }
}

function Test-DriverDomainReadyState {
    if ($isLinuxDriver) { return $true }
    if ($null -eq $cfg -or $null -eq $cfg.Core) { return $false }

    $domainName = $cfg.Core.DomainName
    $isWorkgroup = [string]::IsNullOrWhiteSpace($domainName) -or $domainName -eq 'Workgroup'
    if ($isWorkgroup) { return $true }

    try {
        $partOfDomain = (Get-CimInstance Win32_ComputerSystem -ErrorAction Stop).PartOfDomain
        $secureChannel = Test-ComputerSecureChannel -ErrorAction Stop
        return $partOfDomain -and $secureChannel
    }
    catch {
        .\Write-Info.ps1 "[WARN] Driver domain readiness verification failed: $($_.Exception.Message)" -ForegroundColor Yellow
        return $false
    }
}

function Test-RequiredDriverReadyState {
    $forceLevel2Ready = $isLinuxDriver -or $SkipForceLevel2 -or (Test-Path $forceLevel2Signal)
    if (-not $isLinuxDriver -and (Test-PendingSystemReboot)) {
        return $false
    }
    return (Test-RequiredDriverDscState) -and
           (Test-DriverDomainReadyState) -and
           (Test-Path $driverToolsSignal) -and
           $forceLevel2Ready
}

if ((Test-Path $signalFile) -and (Test-RequiredDriverReadyState)) {
    .\Write-Info.ps1 "[OK] Driver deployment already completed (signal file exists)." -ForegroundColor Green
    if (-not $isLinuxDriver) { Remove-ResumeTask }
    Pop-Location; Stop-Transcript; return
}
$toolsPreparationJob = $null
if ((Test-Path $signalFile) -and (-not (Test-RequiredDriverReadyState))) {
    .\Write-Info.ps1 "[WARN] Removing stale Driver completion signal because required deployment state is incomplete." -ForegroundColor Yellow
    Remove-Item -Path $signalFile -Force
    if (-not $isLinuxDriver) {
        $staleTestTask = Get-ScheduledTask -TaskName 'RunFileServerTests' -ErrorAction SilentlyContinue
        if ($null -ne $staleTestTask) {
            Unregister-ScheduledTask -TaskName 'RunFileServerTests' -Confirm:$false
        }
        Set-DeployStep -Step 1
        $currentStep = 1
    }

}
if (-not $isLinuxDriver) {
    $toolsPreparationJob = Start-DriverToolsPreparationJob
}

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
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
                Stop-DriverToolsPreparationJob -Job $toolsPreparationJob
                Stop-DriverRecoveryTasks
                throw "The Driver hostname is still '$env:COMPUTERNAME' after its one allowed rename reboot; expected '$expectedName'."
            } else {
                Set-RebootCount -Count ($currentRebootCount + 1)
                .\Write-Info.ps1 "Renaming computer from $env:COMPUTERNAME to $expectedName (reboot $($currentRebootCount + 1)/$maxRebootCount)..." -ForegroundColor Yellow
                Rename-Computer -NewName $expectedName -Force

                .\Write-Info.ps1 "Scheduling Deploy-Driver.ps1 to re-run after reboot..." -ForegroundColor Yellow
                Stop-DriverToolsPreparationJob -Job $toolsPreparationJob
                try {
                    Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Driver.ps1" `
                        -WorkingPath $WorkingPath -DscFolder $dscFolder
                }
                catch {
                    Stop-DriverRecoveryTasks
                    Set-RebootCount -Count $currentRebootCount
                    throw "Could not schedule the Driver hostname-change reboot: $($_.Exception.Message)"
                }

                Pop-Location
                Stop-Transcript
                return
            }
        }
    } catch {
        .\Write-Error.ps1 "[FAIL] Could not validate or set hostname: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; throw
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
        if (Test-Path $driverDscSignal) {
            Remove-Item -Path $driverDscSignal -Force -ErrorAction SilentlyContinue
        }
        Invoke-VerifiedDscConfiguration -Path $mofFolder `
            -OperationName 'Driver DSC configuration' `
            -Postcondition { Test-RequiredDriverDscState } `
            -HeartbeatPath $heartbeatFile -PhaseName 'DriverBaseline' | Out-Null
        "DRIVER DSC APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $driverDscSignal -Force
        .\Write-Info.ps1 "[OK] DSC applied in $([math]::Round($phase1.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        Stop-DriverToolsPreparationJob -Job $toolsPreparationJob
        .\Write-Error.ps1 "[FAIL] DSC failed: $($_.Exception.Message)"
        Pop-Location
        Stop-Transcript
        throw "Driver DSC failed; domain join and tool installation are blocked."
    }
    $phase1.Stop()
    .\Write-Info.ps1 ""

    # Install cached tools before the domain-join reboot so installer reboot
    # requirements are coalesced with the one normal Driver reboot.
    .\Write-Info.ps1 "---- Phase 1b: Pre-Reboot Tool Installation ----" -ForegroundColor Yellow
    try {
        Complete-DriverToolsPreparationJob -Job $toolsPreparationJob
        $toolsPreparationJob = $null
        if (-not (Test-Path $driverToolsPreparedSignal) -and
            -not (Test-Path $driverToolsSignal)) {
            $prepared = & $driverToolsInstaller -Role 'DriverComputer' -Operation Prepare `
                -PreparedSignalFile $driverToolsPreparedSignal -NoTranscript | Select-Object -Last 1
            if (-not $prepared -or -not (Test-Path $driverToolsPreparedSignal)) {
                throw 'Required Driver packages could not be prepared before domain join.'
            }
        }
        if (-not (Test-Path $driverToolsSignal)) {
            $installed = & $driverToolsInstaller -Role 'DriverComputer' -Operation Install `
                -PreparedSignalFile $driverToolsPreparedSignal -AllowRebootRequired `
                -NoTranscript | Select-Object -Last 1
            if (-not $installed -or -not (Test-Path $driverToolsSignal)) {
                throw 'Required Driver tools could not be installed before domain join.'
            }
        }
        .\Write-Info.ps1 '[OK] Driver tools installed before the planned domain-join reboot.' -ForegroundColor Green
    }
    catch {
        Stop-DriverToolsPreparationJob -Job $toolsPreparationJob
        .\Write-Error.ps1 "[FAIL] Pre-reboot Driver tool installation failed: $($_.Exception.Message)"
        Pop-Location
        Stop-Transcript
        throw
    }

    # Imperative Step 1 -- Domain Join
    .\Write-Info.ps1 "---- Phase 1c: Domain Join ----" -ForegroundColor Yellow
    try {
        & "$dscFolder\Invoke-DriverImperativeSteps.ps1" -Step 1 -WorkingPath $WorkingPath -NoTranscript
        .\Write-Info.ps1 "[OK] Domain join complete." -ForegroundColor Green
    }
    catch {
        Stop-DriverToolsPreparationJob -Job $toolsPreparationJob
        .\Write-Error.ps1 "[FAIL] Domain join failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; throw
    }

    # Workgroup has no domain-join reboot, so use the same bounded stabilization
    # reboot to honor any accepted installer reboot requirement before Step 2.
    $cfgForReboot = Get-Content -Path $configFile -Raw | ConvertFrom-Json
    $domNameReboot = if ($cfgForReboot.Core) { $cfgForReboot.Core.DomainName } else { $null }
    $isWorkgroup = [string]::IsNullOrWhiteSpace($domNameReboot) -or $domNameReboot -eq 'Workgroup'

    if ($isWorkgroup) {
        .\Write-Info.ps1 'Workgroup mode -- scheduling the planned post-install stabilization reboot.' -ForegroundColor Yellow
        Set-DeployStep -Step 1
        Register-DriverToolsStabilizationReboot
        Pop-Location
        Stop-Transcript
        return
    } else {
        if ($null -ne $toolsPreparationJob) {
            if ($toolsPreparationJob.State -eq 'Completed') {
                try { Complete-DriverToolsPreparationJob -Job $toolsPreparationJob } catch {
                    .\Write-Info.ps1 "[WARN] Package preparation will retry after reboot: $($_.Exception.Message)" -ForegroundColor Yellow
                }
            } else {
                Stop-DriverToolsPreparationJob -Job $toolsPreparationJob
                .\Write-Info.ps1 '[INFO] Paused unfinished package preparation for the domain-join reboot.' -ForegroundColor Yellow
            }
        }
        $toolsPreparationJob = $null

        $joinRebootCount = [int](Get-DeploymentRegistryValue `
            -Name $joinRebootCountName -DefaultValue 0 -RegistryPath $rebootRegPath)
        if ($joinRebootCount -ge 1) {
            Pop-Location
            Stop-Transcript
            throw 'The Driver requested another domain-join reboot after its planned reboot.'
        }
        # Schedule deferred reboot + resume
        Set-DeployStep -Step 1
        Set-ItemProperty -Path $rebootRegPath -Name $joinRebootCountName -Value 1 -Type DWord -Force
        Set-ItemProperty -Path $rebootRegPath -Name $joinRebootPendingName -Value 1 -Type DWord -Force
        Set-ItemProperty -Path $rebootRegPath -Name $joinRebootScheduleRetryName -Value 0 -Type DWord -Force
        $bootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime.ToUniversalTime().ToString('o')
        Set-ItemProperty -Path $rebootRegPath -Name $joinPreBootTimeName -Value $bootTime -Type String -Force
        .\Write-Info.ps1 "Scheduling deferred reboot and resume..." -ForegroundColor Yellow
        # After domain join, resume as domain admin for cross-machine operations
        $domainNetBios = if ($cfg.Domain -and $cfg.Domain.NetBiosName) { $cfg.Domain.NetBiosName } else { $cfg.Core.DomainName.Split('.')[0].ToUpper() }
        $domainAdminUser = "$domainNetBios\$($cfg.Core.Username)"
        $domainAdminPass = $cfg.Core.Password
        try {
            Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Driver.ps1" `
                -WorkingPath $WorkingPath -DscFolder $dscFolder `
                -RunAsUser $domainAdminUser -RunAsPassword $domainAdminPass
        }
        catch {
            Stop-DriverRecoveryTasks
            Pop-Location
            Stop-Transcript
            throw "Could not schedule the Driver domain-join reboot; persisted state will retry once: $($_.Exception.Message)"
        }

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
    if (-not $isLinuxDriver) {
        $joinPending = [int](Get-DeploymentRegistryValue `
            -Name $joinRebootPendingName -DefaultValue 0 -RegistryPath $rebootRegPath)
        if ($joinPending -eq 1) {
            $recordedBoot = [datetime](Get-ItemPropertyValue -Path $rebootRegPath `
                -Name $joinPreBootTimeName -ErrorAction Stop)
            $currentBoot = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime
            if ($currentBoot.ToUniversalTime() -le $recordedBoot.ToUniversalTime()) {
                Stop-DriverToolsPreparationJob -Job $toolsPreparationJob
                if (-not (Test-PostDeployRebootCanStillRun)) {
                    $scheduleRetryCount = [int](Get-DeploymentRegistryValue `
                        -Name $joinRebootScheduleRetryName -DefaultValue 0 `
                        -RegistryPath $rebootRegPath)
                    if ($scheduleRetryCount -ge 1) {
                        Stop-DriverRecoveryTasks
                        Pop-Location
                        Stop-Transcript
                        throw 'The Driver domain-join reboot task failed after one bounded reschedule attempt.'
                    }
                    Set-ItemProperty -Path $rebootRegPath -Name $joinRebootScheduleRetryName `
                        -Value 1 -Type DWord -Force
                    $domainNetBios = if ($cfg.Domain -and $cfg.Domain.NetBiosName) {
                        $cfg.Domain.NetBiosName
                    } else {
                        $cfg.Core.DomainName.Split('.')[0].ToUpperInvariant()
                    }
                    try {
                        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Driver.ps1" `
                            -WorkingPath $WorkingPath -DscFolder $dscFolder `
                            -RunAsUser "$domainNetBios\$($cfg.Core.Username)" `
                            -RunAsPassword $cfg.Core.Password
                    }
                    catch {
                        foreach ($taskName in @('TKFRSAR', 'PostDeployReboot')) {
                            Unregister-ScheduledTask -TaskName $taskName -Confirm:$false -ErrorAction SilentlyContinue
                        }
                        Pop-Location
                        Stop-Transcript
                        throw "The persisted Driver domain-join reboot could not be rescheduled: $($_.Exception.Message)"
                    }
                }
                .\Write-Info.ps1 '[WAIT] Driver domain-join reboot is scheduled but has not occurred yet.' -ForegroundColor Yellow
                Pop-Location
                Stop-Transcript
                return
            }
            Set-ItemProperty -Path $rebootRegPath -Name $joinRebootPendingName -Value 0 -Type DWord -Force
            Set-ItemProperty -Path $rebootRegPath -Name $joinRebootScheduleRetryName -Value 0 -Type DWord -Force
        }
        Wait-DriverDomainReadiness
    }

    Complete-DriverToolsPreparationJob -Job $toolsPreparationJob
    $toolsPreparationJob = $null
    if (-not (Test-Path $driverToolsPreparedSignal) -and
        -not (Test-Path $driverToolsSignal) -and
        -not $isLinuxDriver) {
        $prepared = & $driverToolsInstaller -Role 'DriverComputer' -Operation Prepare `
            -PreparedSignalFile $driverToolsPreparedSignal -NoTranscript | Select-Object -Last 1
        if (-not $prepared -or -not (Test-Path $driverToolsPreparedSignal)) {
            Pop-Location
            Stop-Transcript
            throw 'Required Driver packages could not be prepared.'
        }
    }

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
            if (Test-Path $driverDscSignal) {
                Remove-Item -Path $driverDscSignal -Force -ErrorAction SilentlyContinue
            }
            Invoke-VerifiedDscConfiguration -Path $mofFolder `
                -OperationName 'Driver DSC repair configuration' `
                -Postcondition { Test-RequiredDriverDscState } `
                -HeartbeatPath $heartbeatFile -PhaseName 'DriverRepair' | Out-Null
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
        NoTranscript = $true
        HeartbeatPath = $heartbeatFile
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

    $fl2Required = -not $isLinuxDriver -and -not $SkipForceLevel2
    $fl2Done = -not $fl2Required -or (Test-Path $forceLevel2Signal)

    if ($phase2Ok -and $fl2Done -and (Test-PendingSystemReboot)) {
        Register-DriverToolsStabilizationReboot
        Pop-Location
        Stop-Transcript
        return
    }

    # ===========================================================================
    # Phase 3: Schedule test run (fire-and-forget)
    # ===========================================================================
    .\Write-Info.ps1 "---- Phase 3: Schedule Test Run ----" -ForegroundColor Yellow
    $phase3 = [System.Diagnostics.Stopwatch]::StartNew()

    $testRunScript = Join-Path $scriptsPath 'Invoke-TestRun.ps1'
    if (-not $testAutoRun) {
        $phase3.Stop()
        $staleTestTask = Get-ScheduledTask -TaskName 'RunFileServerTests' -ErrorAction SilentlyContinue
        if ($null -ne $staleTestTask) {
            Unregister-ScheduledTask -TaskName 'RunFileServerTests' -Confirm:$false
        }
        .\Write-Info.ps1 "[SKIP] Automatic FileServer test execution is disabled in Config.json." -ForegroundColor Yellow
    } elseif (-not $phase2Ok) {
        $phase3.Stop()
        .\Write-Info.ps1 "[SKIP] Not scheduling test run -- earlier phases failed" -ForegroundColor Yellow
    } elseif (-not $fl2Done) {
        $phase3.Stop()
        $staleTestTask = Get-ScheduledTask -TaskName 'RunFileServerTests' -ErrorAction SilentlyContinue
        if ($null -ne $staleTestTask) {
            Unregister-ScheduledTask -TaskName 'RunFileServerTests' -Confirm:$false
        }
        .\Write-Info.ps1 "[WAIT] Not scheduling tests until ForceLevel2 is confirmed." -ForegroundColor Yellow
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
        $settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries `
            -StartWhenAvailable
        Register-ScheduledTask -TaskName 'RunFileServerTests' -Action $action `
            -Trigger $triggerStartup `
            -Settings $settings -User $taskUser -Password $taskPassword -RunLevel Highest `
            -Force -ErrorAction Stop | Out-Null

        $launchDomain = if ($domainPrefix) { $domainPrefix } else { $env:COMPUTERNAME }
        $launchUser = if ($domainPrefix) { $cfgJson.Core.Username } else { $taskUser }
        $testRunArguments = @(
            '-NoProfile',
            '-NonInteractive',
            '-File',
            $testRunScript,
            '-WorkingPath',
            $WorkingPath
        )
        $launchedProcess = & "$scriptsPath\Invoke-ProcessAsUser.ps1" `
            -UserName $launchUser -Domain $launchDomain -Password $taskPassword `
            -FilePath $pwshExe -ArgumentList $testRunArguments `
            -WorkingDirectory $scriptsPath -KeepProfileLoaded

        $phase3.Stop()
        .\Write-Info.ps1 "[OK] Test run launched immediately as '$taskUser' (PID $($launchedProcess.ProcessId)); AtStartup fallback registered -- $([math]::Round($phase3.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    .\Write-Info.ps1 ""

    # ===========================================================================
    # Summary
    # ===========================================================================
    $stopwatch.Stop()
    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Driver Configuration Complete" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Total time  : $([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min" -ForegroundColor Cyan
    .\Write-Info.ps1 "    DSC       : $([math]::Round($phase2a.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "    Imperative: $([math]::Round($phase2b.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "    Test sched: $([math]::Round($phase3.Elapsed.TotalSeconds))s" -ForegroundColor DarkGray
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan

    # Only write signal file if all phases succeeded, including ForceLevel2.
    # ForceLevel2 writes its own signal file on success; if missing, the Driver
    # deployment is incomplete and should re-run on next boot.
    if ($phase2Ok -and $fl2Done) {
        if (Test-PendingSystemReboot) {
            Register-DriverToolsStabilizationReboot
            Pop-Location
            Stop-Transcript
            return
        }
        Write-VerifiedDeploymentSignal -Path $signalFile `
            -Content "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
        Set-DeployStep -Step 2
        Set-RebootCount -Count 0
        Set-ItemProperty -Path $rebootRegPath -Name $joinRebootCountName -Value 0 -Type DWord -Force
        Set-ItemProperty -Path $rebootRegPath -Name $toolsRebootCountName -Value 0 -Type DWord -Force
        .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
        Remove-ResumeTask
        Remove-Item -Path $heartbeatFile -Force -ErrorAction SilentlyContinue
    } elseif ($phase2Ok -and -not $fl2Done) {
        .\Write-Info.ps1 "[WARN] Completion remains blocked. The ForceLevel2 retry task will finalize the Driver and schedule tests when the SUT share is ready." -ForegroundColor Yellow
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

    if ($fl2Done -and (Test-PendingSystemReboot)) {
        Register-DriverToolsStabilizationReboot
        Pop-Location
        Stop-Transcript
        return
    }
    if ($fl2Done -and (Test-RequiredDriverReadyState)) {
        Write-VerifiedDeploymentSignal -Path $signalFile `
            -Content "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
        .\Write-Info.ps1 "[OK] ForceLevel2 now confirmed. Signal file written: $signalFile" -ForegroundColor Green
        Remove-ResumeTask
    } elseif ($fl2Done) {
        Set-DeployStep -Step 1
        .\Write-Info.ps1 "[WARN] ForceLevel2 is complete, but Driver DSC/tools state is incomplete. Resetting to Step 1 for repair." -ForegroundColor Yellow
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
        NoTranscript = $true
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
    if (-not $testAutoRun) {
        .\Write-Info.ps1 "[SKIP] Automatic FileServer test execution is disabled in Config.json." -ForegroundColor Yellow
    } elseif ($phase2Ok -and (Test-Path $testRunScript)) {
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
        Write-VerifiedDeploymentSignal -Path $signalFile `
            -Content "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
        .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
    }
}

Pop-Location
Stop-Transcript
