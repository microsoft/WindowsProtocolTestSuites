# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Deterministic phased deployment for the Workgroup SUT.

.DESCRIPTION
    Phase 0 installs every disruptive Windows feature and prepares tool packages.
    One planned reboot coalesces feature and computer-rename requirements.
    Phase 1 applies only non-disruptive DSC convergence resources.
    Phase 2 runs imperative environment setup while prepared tools install.
    Phase 3 is persisted only after concrete readiness postconditions pass.
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder = $PSScriptRoot
$scriptsPath = "$dscFolder\Scripts"
$featureMofFolder = "$dscFolder\MOF\SUT-Features"
$convergenceMofFolder = "$dscFolder\MOF\SUT"
$logFile = "$dscFolder\Deploy-SUT.log"
$heartbeatFile = "$dscFolder\Deploy-SUT.heartbeat.json"
$configFile = "$WorkingPath\Config.json"
$phaseRegistryName = 'WorkgroupSutDeployPhase'
$plannedRebootName = 'WorkgroupSutPlannedReboot'
$plannedBootTimeName = 'WorkgroupSutPreRebootBootTimeUtc'
$plannedRebootCountName = 'WorkgroupSutPlannedRebootCount'
$registryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
$toolsJobTimeoutSeconds = 3600

$env:Path += ";$scriptsPath"
Push-Location $scriptsPath
Start-Transcript -Path $logFile -Append -Force

function Stop-DeploymentTranscript {
    Pop-Location
    Stop-Transcript
}

function Get-DeploymentRegistryValue {
    param([string]$Name, $DefaultValue)
    $value = Get-ItemProperty -Path $registryPath -Name $Name -ErrorAction SilentlyContinue
    if ($null -eq $value) { return $DefaultValue }
    return $value.$Name
}

function Set-DeploymentRegistryValue {
    param(
        [string]$Name,
        $Value,
        [ValidateSet('DWord', 'String')]
        [string]$Type
    )
    if (-not (Test-Path $registryPath)) {
        New-Item -Path $registryPath -Force | Out-Null
    }
    Set-ItemProperty -Path $registryPath -Name $Name -Value $Value -Type $Type -Force
}

function Test-RequiredSutFeatureState {
    $missing = @()
    $featureMarker = Get-ItemProperty -Path $registryPath `
        -Name 'WorkgroupSutFeatureBundleAttempted' -ErrorAction SilentlyContinue
    if ($null -eq $featureMarker) {
        $missing += 'feature-bundle-marker'
    }
    foreach ($featureName in $requiredSutFeatures) {
        $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
        if ($null -eq $feature -or $feature.InstallState -ne 'Installed') {
            $missing += "feature:$featureName"
        }
    }
    if ($missing.Count -gt 0) {
        .\Write-Info.ps1 "[WARN] Required Workgroup SUT feature state is incomplete: $($missing -join ', ')" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-RequiredSutDscState {
    $missing = @()
    if (-not (Test-RequiredSutFeatureState)) {
        $missing += 'feature-bundle'
    }
    foreach ($commandName in @('dfsutil.exe', 'dfscmd.exe')) {
        if (-not (Get-Command $commandName -ErrorAction SilentlyContinue)) {
            $missing += "command:$commandName"
        }
    }
    foreach ($path in @('C:\FileShare', 'C:\SMBBasic', 'C:\SMBBasic\sub')) {
        if (-not (Test-Path -LiteralPath $path -PathType Container)) {
            $missing += "directory:$path"
        }
    }
    foreach ($share in @(
        @{ Name = 'FileShare'; Path = 'C:\FileShare' },
        @{ Name = 'SMBBasic'; Path = 'C:\SMBBasic' }
    )) {
        $existing = Get-SmbShare -Name $share.Name -ErrorAction SilentlyContinue
        if ($null -eq $existing -or $existing.Path -ne $share.Path) {
            $missing += "share:$($share.Name)"
        }
    }
    if ($missing.Count -gt 0) {
        .\Write-Info.ps1 "[WARN] Required Workgroup SUT convergence state is incomplete: $($missing -join ', ')" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-RequiredSutImperativeState {
    $missing = @()
    $nonAdminName = if ($cfg.LocalAccounts.NonAdmin.Username) {
        $cfg.LocalAccounts.NonAdmin.Username
    } else {
        'nonadmin'
    }
    $nonAdmin = Get-LocalUser -Name $nonAdminName -ErrorAction SilentlyContinue
    if ($null -eq $nonAdmin -or -not $nonAdmin.Enabled) {
        $missing += "local-user:$nonAdminName"
    }
    foreach ($path in @(
        'C:\SMBBasic\symboliclink',
        'C:\SMBBasic\sub\symboliclink2',
        'C:\FileShare\ExistingFolder',
        'C:\FileShare\ExistingFile.txt'
    )) {
        if (-not (Test-Path -LiteralPath $path)) {
            $missing += "imperative-path:$path"
        }
    }
    foreach ($namespace in @('SMBDfs', 'Standalone')) {
        $dfsRoot = "\\$env:COMPUTERNAME\$namespace"
        & dfsutil.exe root $dfsRoot *> $null
        if ($LASTEXITCODE -ne 0) {
            $missing += "dfs-root:$dfsRoot"
        }
    }
    if ($missing.Count -gt 0) {
        .\Write-Info.ps1 "[WARN] Required Workgroup SUT imperative state is incomplete: $($missing -join ', ')" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-RequiredSutReadyState {
    return (Test-Path $fullDscSignal) -and
           (Test-Path $toolsSignal) -and
           (Test-RequiredSutDscState) -and
           (Test-RequiredSutImperativeState)
}

function Start-ToolsPreparationJob {
    if ((Test-Path $toolsPreparedSignal) -or (Test-Path $toolsSignal)) {
        return $null
    }

    .\Write-Info.ps1 'Starting parallel tool package preparation...' -ForegroundColor Cyan
    $jobLog = "$scriptsPath\InstallMSIAndTools.prepare.job.log"
    return Start-Job -ScriptBlock {
        param($installer, $scriptsDirectory, $preparedSignal, $log)
        Set-Location $scriptsDirectory
        $env:Path += ";$scriptsDirectory"
        $output = @(& $installer -Role 'SUT' -Operation Prepare `
            -PreparedSignalFile $preparedSignal -NoTranscript *>&1)
        $output | Out-File -FilePath $log -Force
        if ($output.Count -eq 0 -or $output[-1] -ne $true) {
            throw 'Required SUT tool package preparation failed.'
        }
    } -ArgumentList $toolsInstaller, $scriptsPath, $toolsPreparedSignal, $jobLog
}

function Stop-ToolsPreparationJob {
    param([System.Management.Automation.Job]$Job)
    if ($null -eq $Job) { return }
    if ($Job.State -in @('NotStarted', 'Running', 'Blocked')) {
        Stop-Job -Job $Job
    }
    Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
}

function Complete-ToolsPreparationJob {
    param([System.Management.Automation.Job]$Job)
    if ($null -eq $Job) { return }
    try {
        Wait-DeploymentJob -Job $Job -TimeoutSeconds $toolsJobTimeoutSeconds `
            -Phase 'ToolsPrepare' -Operation 'Parallel tool package preparation' `
            -HeartbeatPath $heartbeatFile -LastCheckpoint 'Deployment orchestration active' | Out-Null
    }
    finally {
        Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
    }
}

.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan
.\Write-Info.ps1 '  Workgroup SUT -- Deterministic Phased Deployment         ' -ForegroundColor Cyan
.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan
.\Write-Info.ps1 "WorkingPath : $WorkingPath" -ForegroundColor DarkGray
.\Write-Info.ps1 "DSCFolder   : $dscFolder" -ForegroundColor DarkGray

. "$dscFolder\Deploy-CommonHelpers.ps1"

try {
    if (-not (Test-Path $configFile)) {
        throw "Config.json was not found at '$configFile'."
    }
    $cfg = Get-Content -Path $configFile -Raw | ConvertFrom-Json
    $validateScript = "$scriptsPath\Validate-ConfigFile.ps1"
    if (Test-Path $validateScript) {
        .\Write-Info.ps1 'Validating Config.json...' -ForegroundColor Cyan
        & $validateScript -ConfigPath $configFile
        .\Write-Info.ps1 '[OK] Config.json validation passed' -ForegroundColor Green
    }
}
catch {
    .\Write-Error.ps1 "[FAIL] Preflight validation failed: $($_.Exception.Message)"
    Stop-DeploymentTranscript
    throw
}

$signalFile = "$dscFolder\Deploy-SUT.Completed.signal"
$featureSignal = "$scriptsPath\SUT-Features.Completed.signal"
$fullDscSignal = "$scriptsPath\SUT-FullDsc.Completed.signal"
$toolsSignal = "$scriptsPath\InstallMSIAndTools.Completed.signal"
$toolsPreparedSignal = "$scriptsPath\InstallMSIAndTools.Prepared.signal"
$toolsInstaller = "$scriptsPath\InstallMSIAndTools.ps1"
$requiredSutFeatures = @(
    'File-Services',
    'FS-BranchCache',
    'FS-VSS-Agent',
    'BranchCache',
    'FS-DFS-Namespace',
    'RSAT-File-Services',
    'RSAT-DFS-Mgmt-Con',
    'FS-Resource-Manager'
)

$staleReboot = Get-ScheduledTask -TaskName 'PostDeployReboot' -ErrorAction SilentlyContinue
if ($null -ne $staleReboot) {
    Unregister-ScheduledTask -TaskName 'PostDeployReboot' -Confirm:$false
    .\Write-Info.ps1 '[OK] Cancelled stale PostDeployReboot task.' -ForegroundColor Yellow
}

if ((Test-Path $signalFile) -and (Test-RequiredSutReadyState)) {
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 3
    Set-DeploymentRegistryValue -Name $plannedRebootCountName -Value 0 -Type DWord
    .\Write-Info.ps1 '[OK] SUT deployment already completed and remains ready.' -ForegroundColor Green
    Remove-ResumeTask
    Stop-DeploymentTranscript
    return
}
if (Test-Path $signalFile) {
    .\Write-Info.ps1 '[WARN] Removing stale Workgroup SUT completion signal.' -ForegroundColor Yellow
    Remove-Item -Path $signalFile -Force
}

$currentPhase = Get-DeploymentPhase -Name $phaseRegistryName
if ($currentPhase -ge 3 -and -not (Test-Path $signalFile)) {
    $repairPhase = if (Test-RequiredSutDscState) {
        2
    } elseif ((Test-RequiredSutFeatureState) -and -not (Test-PendingSystemReboot)) {
        1
    } else {
        0
    }
    Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase
    $currentPhase = $repairPhase
    .\Write-Info.ps1 "[WARN] Completion signal is absent; reset persisted Phase 3 to repair Phase $repairPhase." -ForegroundColor Yellow
}

$plannedReboot = [int](Get-DeploymentRegistryValue -Name $plannedRebootName -DefaultValue 0)
if ($currentPhase -ge 1 -and $plannedReboot -eq 1) {
    $preRebootBootTime = [datetime](Get-DeploymentRegistryValue -Name $plannedBootTimeName -DefaultValue '1900-01-01T00:00:00Z')
    $currentBootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime
    if ($currentBootTime.ToUniversalTime() -le $preRebootBootTime.ToUniversalTime()) {
        Stop-DeploymentTranscript
        throw 'Phase advancement was persisted, but the planned Workgroup SUT reboot was not observed.'
    }
    Set-DeploymentRegistryValue -Name $plannedRebootName -Value 0 -Type DWord
    .\Write-Info.ps1 '[OK] Planned feature/rename reboot completed.' -ForegroundColor Green
}

# Migrate deployments created by the previous non-phased orchestrator.
if ($currentPhase -eq 0 -and -not (Test-PendingSystemReboot)) {
    if (Test-RequiredSutDscState) {
        "FULL DSC APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
            Set-Content -Path $fullDscSignal -Force
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        $currentPhase = 2
        .\Write-Info.ps1 '[OK] Migrated existing converged SUT state to Phase 2.' -ForegroundColor Green
    }
    elseif (Test-RequiredSutFeatureState) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        $currentPhase = 1
        .\Write-Info.ps1 '[OK] Migrated existing feature state to Phase 1.' -ForegroundColor Green
    }
}

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$toolsPreparationJob = Start-ToolsPreparationJob

# ===========================================================================
# Phase 0: Disruptive features and one planned reboot
# ===========================================================================
if ($currentPhase -eq 0) {
    .\Write-Info.ps1 '---- Phase 0: Pre-Reboot Features ----' -ForegroundColor Yellow
    $renameRequested = $false
    try {
        $expectedName = $cfg.Machines.SUT.ComputerName
        if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
            .\Write-Info.ps1 "Renaming computer from $env:COMPUTERNAME to $expectedName; reboot is deferred until features finish." -ForegroundColor Yellow
            Rename-Computer -NewName $expectedName -Force
            $renameRequested = $true
        }

        . "$dscFolder\SUT-FeatureConfiguration.ps1"
        .\Write-Info.ps1 'Compiling feature-only SUT DSC configuration...' -ForegroundColor Cyan
        SutFeatureConfiguration -OutputPath $featureMofFolder
        Remove-Item -Path $featureSignal -Force -ErrorAction SilentlyContinue
        Invoke-VerifiedDscConfiguration -Path $featureMofFolder `
            -OperationName 'Workgroup SUT pre-reboot feature configuration' `
            -Postcondition { Test-RequiredSutFeatureState } `
            -HeartbeatPath $heartbeatFile -PhaseName 'Features' | Out-Null
        "FEATURES APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
            Set-Content -Path $featureSignal -Force
        .\Write-Info.ps1 '[OK] All required disruptive features were attempted in one DSC operation.' -ForegroundColor Green
    }
    catch {
        $phaseError = $_.Exception.Message
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        .\Write-Error.ps1 "[FAIL] Pre-reboot feature phase failed: $phaseError"
        Stop-DeploymentTranscript
        throw
    }

    $rebootRequired = $renameRequested -or (Test-PendingSystemReboot)
    if ($rebootRequired) {
        if ($null -ne $toolsPreparationJob -and $toolsPreparationJob.State -eq 'Completed') {
            try {
                Complete-ToolsPreparationJob -Job $toolsPreparationJob
            }
            catch {
                .\Write-Info.ps1 "[WARN] Tool package preparation will retry after reboot: $($_.Exception.Message)" -ForegroundColor Yellow
            }
        } else {
            Stop-ToolsPreparationJob -Job $toolsPreparationJob
            .\Write-Info.ps1 '[INFO] Paused unfinished package preparation for the planned reboot; completed files remain cached.' -ForegroundColor Yellow
        }
        $toolsPreparationJob = $null

        $plannedRebootCount = [int](Get-DeploymentRegistryValue -Name $plannedRebootCountName -DefaultValue 0)
        if ($plannedRebootCount -ge 1) {
            Stop-DeploymentTranscript
            throw 'The Workgroup SUT requested another disruptive reboot after its single planned reboot.'
        }
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        $currentPhase = 1
        Set-DeploymentRegistryValue -Name $plannedRebootCountName -Value 1 -Type DWord
        Set-DeploymentRegistryValue -Name $plannedRebootName -Value 1 -Type DWord
        $bootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime.ToUniversalTime().ToString('o')
        Set-DeploymentRegistryValue -Name $plannedBootTimeName -Value $bootTime -Type String
        Write-DeploymentHeartbeat -Phase 'RebootPending' -Operation 'Waiting for the single planned reboot' `
            -StartedAt (Get-Date) -HeartbeatPath $heartbeatFile -LastCheckpoint 'Feature phase complete'
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-SUT.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder
        Stop-DeploymentTranscript
        return
    }
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
    $currentPhase = 1
}

# ===========================================================================
# Phase 1: Post-reboot non-disruptive DSC convergence
# ===========================================================================
if ($currentPhase -eq 1) {
    if (Test-PendingSystemReboot) {
        Stop-DeploymentTranscript
        throw 'A reboot is still pending before Workgroup SUT convergence; refusing to start non-disruptive DSC.'
    }

    .\Write-Info.ps1 '---- Phase 1: Post-Reboot DSC Convergence ----' -ForegroundColor Yellow
    try {
        . "$dscFolder\SUT-Configuration.ps1"
        SutConfiguration -ConfigFilePath $configFile -OutputPath $convergenceMofFolder
        Remove-Item -Path $fullDscSignal -Force -ErrorAction SilentlyContinue
        Invoke-VerifiedDscConfiguration -Path $convergenceMofFolder `
            -OperationName 'Workgroup SUT post-reboot convergence configuration' `
            -Postcondition { Test-RequiredSutDscState } `
            -HeartbeatPath $heartbeatFile -PhaseName 'Convergence' | Out-Null
        "FULL DSC APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
            Set-Content -Path $fullDscSignal -Force
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        $currentPhase = 2
        .\Write-Info.ps1 '[OK] Post-reboot DSC convergence completed.' -ForegroundColor Green
    }
    catch {
        $phaseError = $_.Exception.Message
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        .\Write-Error.ps1 "[FAIL] Post-reboot DSC convergence failed: $phaseError"
        Stop-DeploymentTranscript
        throw
    }
}

# ===========================================================================
# Phase 2: Imperative environment and controlled tool installation
# ===========================================================================
if ($currentPhase -eq 2) {
    .\Write-Info.ps1 '---- Phase 2: Environment + Tools ----' -ForegroundColor Yellow

    try {
        Complete-ToolsPreparationJob -Job $toolsPreparationJob
        $toolsPreparationJob = $null
        if (-not (Test-Path $toolsPreparedSignal) -and -not (Test-Path $toolsSignal)) {
            $prepared = & $toolsInstaller -Role 'SUT' -Operation Prepare `
                -PreparedSignalFile $toolsPreparedSignal -NoTranscript | Select-Object -Last 1
            if (-not $prepared -or -not (Test-Path $toolsPreparedSignal)) {
                throw 'Required SUT tool packages could not be prepared.'
            }
        }
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Tool package preparation failed: $($_.Exception.Message)"
        Stop-DeploymentTranscript
        throw
    }

    $toolsInstallJob = $null
    if (-not (Test-Path $toolsSignal)) {
        $toolsInstallLog = "$scriptsPath\InstallMSIAndTools.install.job.log"
        $toolsInstallJob = Start-Job -ScriptBlock {
            param($installer, $scriptsDirectory, $preparedSignal, $log)
            Set-Location $scriptsDirectory
            $env:Path += ";$scriptsDirectory"
            $output = @(& $installer -Role 'SUT' -Operation Install `
                -PreparedSignalFile $preparedSignal -NoTranscript *>&1)
            $output | Out-File -FilePath $log -Force
            if ($output.Count -eq 0 -or $output[-1] -ne $true) {
                throw 'Required SUT tool installation failed.'
            }
        } -ArgumentList $toolsInstaller, $scriptsPath, $toolsPreparedSignal, $toolsInstallLog
    }

    $imperativeOk = $false
    try {
        & "$dscFolder\Invoke-SutImperativeSteps.ps1" -WorkingPath $WorkingPath `
            -HeartbeatPath $heartbeatFile -NoTranscript
        $imperativeOk = Test-RequiredSutImperativeState
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Imperative environment setup failed: $($_.Exception.Message)"
    }

    if ($null -ne $toolsInstallJob) {
        try {
            Wait-DeploymentJob -Job $toolsInstallJob -TimeoutSeconds $toolsJobTimeoutSeconds `
                -Phase 'ToolsInstall' -Operation 'Controlled SUT tool installation' `
                -HeartbeatPath $heartbeatFile -LastCheckpoint 'Post-reboot DSC convergence complete' | Out-Null
        }
        catch {
            .\Write-Error.ps1 "[FAIL] Tool installation failed: $($_.Exception.Message)"
        }
        finally {
            Remove-Job -Job $toolsInstallJob -Force -ErrorAction SilentlyContinue
        }
    }

    $toolsOk = Test-Path $toolsSignal
    if (-not $imperativeOk -or -not $toolsOk -or -not (Test-RequiredSutReadyState)) {
        .\Write-Info.ps1 "[FAIL] Phase 2 incomplete (Tools=$toolsOk, Imperative=$imperativeOk)." -ForegroundColor Red
        Stop-DeploymentTranscript
        throw 'Workgroup SUT deployment did not satisfy all required Phase 2 postconditions.'
    }
    if (Test-PendingSystemReboot) {
        Stop-DeploymentTranscript
        throw 'An unexpected second reboot became pending after the controlled post-reboot phase.'
    }

    try {
        "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
            Set-Content -Path $signalFile -Force
        if (-not (Test-Path $signalFile -PathType Leaf)) {
            throw "Completion signal '$signalFile' was not created."
        }
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Failed to write Workgroup SUT completion signal: $($_.Exception.Message)"
        Stop-DeploymentTranscript
        throw
    }

    Set-DeploymentPhase -Name $phaseRegistryName -Phase 3
    Set-DeploymentRegistryValue -Name $plannedRebootCountName -Value 0 -Type DWord
    Remove-ResumeTask
    Remove-Item -Path $heartbeatFile -Force -ErrorAction SilentlyContinue
    .\Write-Info.ps1 '[OK] Workgroup SUT deployment completed with all postconditions verified.' -ForegroundColor Green
}

$stopwatch.Stop()
.\Write-Info.ps1 "Total deployment time in this invocation: $([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min" -ForegroundColor Cyan
Stop-DeploymentTranscript
