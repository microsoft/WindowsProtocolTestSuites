# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Deterministic phased deployment for the Domain Controller.

.DESCRIPTION
    Phase 0 installs disruptive roles and coalesces an optional hostname change
    into the foundation reboot. Phase 1 promotes the server and performs the
    mandatory promotion reboot. Phase 2 proves stable AD operation, provisions
    directory state and tools, and applies post-promotion DSC convergence.
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder = $PSScriptRoot
$scriptsPath = "$dscFolder\Scripts"
$featureMofFolder = "$dscFolder\MOF\DC-Features"
$convergenceMofFolder = "$dscFolder\MOF\DC"
$logFile = "$dscFolder\Deploy-DC.log"
$heartbeatFile = "$dscFolder\Deploy-DC.heartbeat.json"
$configFile = "$WorkingPath\Config.json"
$registryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
$phaseRegistryName = 'DcDeployPhase'
$foundationPendingName = 'DcFoundationRebootPending'
$foundationBootTimeName = 'DcFoundationPreRebootBootTimeUtc'
$foundationRebootCountName = 'DcFoundationRebootCount'
$promotionPendingName = 'DcPromotionRebootPending'
$promotionBootTimeName = 'DcPromotionPreRebootBootTimeUtc'
$promotionRebootCountName = 'DcPromotionRebootCount'
$toolsJobTimeoutSeconds = 3600

$env:Path += ";$scriptsPath"
Push-Location $scriptsPath
Start-Transcript -Path $logFile -Append -Force

function Stop-DeploymentTranscript {
    Pop-Location
    Stop-Transcript
}

function Get-RegistryValue {
    param([string]$Name, $DefaultValue)
    $value = Get-ItemProperty -Path $registryPath -Name $Name -ErrorAction SilentlyContinue
    if ($null -eq $value) { return $DefaultValue }
    return $value.$Name
}

function Set-RegistryValue {
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

function Test-NewBoot {
    param([string]$BootTimeRegistryName)
    $recorded = [datetime](Get-RegistryValue -Name $BootTimeRegistryName -DefaultValue '1900-01-01T00:00:00Z')
    $current = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime
    return $current.ToUniversalTime() -gt $recorded.ToUniversalTime()
}

function Test-IsDomainController {
    $domainRole = [int](Get-CimInstance Win32_ComputerSystem).DomainRole
    return $domainRole -in @(4, 5)
}

function Test-RequiredDcFeatures {
    $missing = @()
    $marker = Get-ItemProperty -Path $registryPath `
        -Name 'DcFeatureBundleAttempted' -ErrorAction SilentlyContinue
    if ($null -eq $marker) { $missing += 'feature-bundle-marker' }
    foreach ($featureName in $requiredDcFeatures) {
        $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
        if ($null -eq $feature -or $feature.InstallState -ne 'Installed') {
            $missing += "feature:$featureName"
        }
    }
    if ($missing.Count -gt 0) {
        .\Write-Info.ps1 "[WARN] Required DC feature state is incomplete: $($missing -join ', ')" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-RequiredDcConvergenceState {
    $missing = @()
    if (-not (Test-RequiredDcFeatures)) { $missing += 'feature-bundle' }
    $enabledFirewall = Get-NetFirewallProfile -ErrorAction SilentlyContinue |
        Where-Object { $_.Enabled -eq $true }
    if ($null -ne $enabledFirewall -and @($enabledFirewall).Count -gt 0) {
        $missing += 'firewall-enabled'
    }
    $remoteAccess = Get-Service RemoteAccess -ErrorAction SilentlyContinue
    if ($null -eq $remoteAccess -or $remoteAccess.StartType -ne 'Automatic') {
        $missing += 'remote-access-autostart'
    }
    if ($missing.Count -gt 0) {
        .\Write-Info.ps1 "[WARN] Required DC convergence state is incomplete: $($missing -join ', ')" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-RequiredDcReadyState {
    if (-not (Test-IsDomainController)) { return $false }
    if (-not (Test-RequiredDcConvergenceState)) { return $false }
    if (-not (Test-Path $toolsSignal) -or -not (Test-Path $adOperationalSignal)) {
        return $false
    }
    try {
        $domain = Get-ADDomain -ErrorAction Stop
        if ($null -eq $domain) { return $false }
    }
    catch { return $false }
    return (Test-Path "\\$env:COMPUTERNAME\SYSVOL") -and
           (Test-Path "\\$env:COMPUTERNAME\NETLOGON")
}

function Get-DcRepairPhase {
    if ((Test-IsDomainController) -and
        (Test-RequiredDcFeatures) -and
        -not (Test-PendingSystemReboot)) {
        return 2
    }
    return 0
}

function Start-ToolsPreparationJob {
    if ((Test-Path $toolsPreparedSignal) -or (Test-Path $toolsSignal)) { return $null }
    .\Write-Info.ps1 'Starting parallel DC tool package preparation...' -ForegroundColor Cyan
    $jobLog = "$scriptsPath\InstallMSIAndTools.prepare.job.log"
    return Start-Job -ScriptBlock {
        param($installer, $scriptsDirectory, $preparedSignal, $log)
        Set-Location $scriptsDirectory
        $env:Path += ";$scriptsDirectory"
        $output = @(& $installer -Role 'DC' -Operation Prepare `
            -PreparedSignalFile $preparedSignal -NoTranscript *>&1)
        $output | Out-File -FilePath $log -Force
        if ($output.Count -eq 0 -or $output[-1] -ne $true) {
            throw 'Required DC package preparation failed.'
        }
    } -ArgumentList $toolsInstaller, $scriptsPath, $toolsPreparedSignal, $jobLog
}

function Stop-ToolsPreparationJob {
    param([System.Management.Automation.Job]$Job)
    if ($null -eq $Job) { return }
    if ($Job.State -in @('NotStarted', 'Running', 'Blocked')) { Stop-Job -Job $Job }
    Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
}

function Complete-ToolsPreparationJob {
    param([System.Management.Automation.Job]$Job)
    if ($null -eq $Job) { return }
    try {
        Wait-DeploymentJob -Job $Job -TimeoutSeconds $toolsJobTimeoutSeconds `
            -Phase 'ToolsPrepare' -Operation 'Parallel DC package preparation' `
            -HeartbeatPath $heartbeatFile -LastCheckpoint 'DC orchestration active' | Out-Null
    }
    finally {
        Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
    }
}

.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan
.\Write-Info.ps1 '  Domain Controller -- Deterministic Phased Deployment     ' -ForegroundColor Cyan
.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan

. "$dscFolder\Deploy-CommonHelpers.ps1"

try {
    if (-not (Test-Path $configFile)) { throw "Config.json was not found at '$configFile'." }
    $cfg = Get-Content -Path $configFile -Raw | ConvertFrom-Json
    $validateScript = "$scriptsPath\Validate-ConfigFile.ps1"
    if (Test-Path $validateScript) {
        & $validateScript -ConfigPath $configFile
        .\Write-Info.ps1 '[OK] Config.json validation passed' -ForegroundColor Green
    }
}
catch {
    .\Write-Error.ps1 "[FAIL] DC preflight failed: $($_.Exception.Message)"
    Stop-DeploymentTranscript
    throw
}

$signalFile = "$dscFolder\Deploy-DC.Completed.signal"
$adOperationalSignal = "$dscFolder\DC-ADOperational.Completed.signal"
$toolsSignal = "$scriptsPath\InstallMSIAndTools.Completed.signal"
$toolsPreparedSignal = "$scriptsPath\InstallMSIAndTools.Prepared.signal"
$toolsInstaller = "$scriptsPath\InstallMSIAndTools.ps1"
$requiredDcFeatures = @(
    'AD-Domain-Services',
    'RSAT-AD-Tools',
    'GPMC',
    'RemoteAccess',
    'RSAT-RemoteAccess'
)

$staleReboot = Get-ScheduledTask -TaskName 'PostDeployReboot' -ErrorAction SilentlyContinue
if ($null -ne $staleReboot) {
    Unregister-ScheduledTask -TaskName 'PostDeployReboot' -Confirm:$false
    .\Write-Info.ps1 '[OK] Cancelled stale PostDeployReboot task.' -ForegroundColor Yellow
}

if ((Test-Path $signalFile) -and (Test-RequiredDcReadyState)) {
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 3
    Set-RegistryValue -Name $foundationRebootCountName -Value 0 -Type DWord
    Set-RegistryValue -Name $promotionRebootCountName -Value 0 -Type DWord
    Remove-ResumeTask
    .\Write-Info.ps1 '[OK] DC deployment already completed and remains ready.' -ForegroundColor Green
    Stop-DeploymentTranscript
    return
}
if (Test-Path $signalFile) {
    .\Write-Info.ps1 '[WARN] Removing stale DC completion signal.' -ForegroundColor Yellow
    Remove-Item -Path $signalFile -Force
    $repairPhase = Get-DcRepairPhase
    Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase
}

$currentPhase = Get-DeploymentPhase -Name $phaseRegistryName
if ($currentPhase -ge 3 -and -not (Test-Path $signalFile)) {
    $repairPhase = Get-DcRepairPhase
    Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase
    $currentPhase = $repairPhase
    .\Write-Info.ps1 "[WARN] Completion signal is absent; reset persisted DC Phase 3 to repair Phase $repairPhase." -ForegroundColor Yellow
}

if ($currentPhase -ge 1 -and
    [int](Get-RegistryValue -Name $foundationPendingName -DefaultValue 0) -eq 1) {
    if (-not (Test-NewBoot -BootTimeRegistryName $foundationBootTimeName)) {
        Stop-DeploymentTranscript
        throw 'The DC foundation reboot was persisted but not observed.'
    }
    Set-RegistryValue -Name $foundationPendingName -Value 0 -Type DWord
    .\Write-Info.ps1 '[OK] DC foundation reboot completed.' -ForegroundColor Green
}
if ($currentPhase -ge 2 -and
    [int](Get-RegistryValue -Name $promotionPendingName -DefaultValue 0) -eq 1) {
    if (-not (Test-NewBoot -BootTimeRegistryName $promotionBootTimeName)) {
        Stop-DeploymentTranscript
        throw 'The DC promotion reboot was persisted but not observed.'
    }
    Set-RegistryValue -Name $promotionPendingName -Value 0 -Type DWord
    .\Write-Info.ps1 '[OK] DC promotion reboot completed.' -ForegroundColor Green
}

# Migrate deployments created by the previous DeployStep/Phase1a state machine.
$oldDeployStep = [int](Get-RegistryValue -Name 'DeployStep' -DefaultValue 0)
$oldFeaturesApplied = [int](Get-RegistryValue -Name 'Phase1aDscApplied' -DefaultValue 0)
if ($currentPhase -eq 0) {
    if (Test-IsDomainController) {
        $repairPhase = Get-DcRepairPhase
        if ($repairPhase -eq 2) {
            Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
            $currentPhase = 2
            .\Write-Info.ps1 '[OK] Migrated existing promoted DC state to Phase 2.' -ForegroundColor Green
        } else {
            .\Write-Info.ps1 '[WARN] Existing promoted DC requires feature repair before convergence.' -ForegroundColor Yellow
        }
    }
    elseif ($oldDeployStep -ge 1) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        Set-RegistryValue -Name $promotionPendingName -Value 1 -Type DWord
        $bootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime.ToUniversalTime().ToString('o')
        Set-RegistryValue -Name $promotionBootTimeName -Value $bootTime -Type String
        Set-RegistryValue -Name $promotionRebootCountName -Value 1 -Type DWord
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-DC.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder
        Stop-DeploymentTranscript
        return
    }
    elseif ($oldFeaturesApplied -eq 1 -and
            (Test-RequiredDcFeatures) -and -not (Test-PendingSystemReboot)) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        $currentPhase = 1
        .\Write-Info.ps1 '[OK] Migrated existing DC feature state to Phase 1.' -ForegroundColor Green
    }
}

$toolsPreparationJob = Start-ToolsPreparationJob

# ===========================================================================
# Phase 0: Hostname + disruptive roles + foundation reboot
# ===========================================================================
if ($currentPhase -eq 0) {
    .\Write-Info.ps1 '---- Phase 0: Hostname + Role Features ----' -ForegroundColor Yellow
    $expectedName = $cfg.Machines.DC.ComputerName
    $renameRequested = $false
    try {
        if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
            Rename-Computer -NewName $expectedName -Force
            $renameRequested = $true
            .\Write-Info.ps1 "[OK] Requested hostname change to $expectedName." -ForegroundColor Green
        }

        . "$dscFolder\DC-FeatureConfiguration.ps1"
        DcFeatureConfiguration -OutputPath $featureMofFolder
        Invoke-VerifiedDscConfiguration -Path $featureMofFolder `
            -OperationName 'DC pre-promotion feature configuration' `
            -Postcondition { Test-RequiredDcFeatures } `
            -HeartbeatPath $heartbeatFile -PhaseName 'Features' | Out-Null
    }
    catch {
        $phaseError = $_.Exception.Message
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        .\Write-Error.ps1 "[FAIL] DC foundation phase failed: $phaseError"
        Stop-DeploymentTranscript
        throw
    }

    $foundationRebootRequired = $renameRequested -or (Test-PendingSystemReboot)
    if ($foundationRebootRequired) {
        if ($null -ne $toolsPreparationJob -and $toolsPreparationJob.State -eq 'Completed') {
            try { Complete-ToolsPreparationJob -Job $toolsPreparationJob } catch {
                .\Write-Info.ps1 "[WARN] Package preparation will retry after reboot: $($_.Exception.Message)" -ForegroundColor Yellow
            }
        } else {
            Stop-ToolsPreparationJob -Job $toolsPreparationJob
            .\Write-Info.ps1 '[INFO] Paused unfinished package preparation for the foundation reboot.' -ForegroundColor Yellow
        }
        $toolsPreparationJob = $null

        $rebootCount = [int](Get-RegistryValue -Name $foundationRebootCountName -DefaultValue 0)
        if ($rebootCount -ge 1) {
            Stop-DeploymentTranscript
            throw 'The DC requested another foundation reboot after its planned foundation reboot.'
        }
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        Set-RegistryValue -Name 'DeployStep' -Value 0 -Type DWord
        Set-RegistryValue -Name 'Phase1aDscApplied' -Value 1 -Type DWord
        Set-RegistryValue -Name $foundationRebootCountName -Value 1 -Type DWord
        Set-RegistryValue -Name $foundationPendingName -Value 1 -Type DWord
        $bootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime.ToUniversalTime().ToString('o')
        Set-RegistryValue -Name $foundationBootTimeName -Value $bootTime -Type String
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-DC.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder
        Stop-DeploymentTranscript
        return
    }
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
    $currentPhase = 1
}

# ===========================================================================
# Phase 1: Promotion and its mandatory reboot
# ===========================================================================
if ($currentPhase -eq 1) {
    if (Test-PendingSystemReboot) {
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        Stop-DeploymentTranscript
        throw 'A reboot remains pending before Domain Controller promotion.'
    }
    if (-not (Test-RequiredDcFeatures)) {
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        Stop-DeploymentTranscript
        throw 'Required Domain Controller features are incomplete before promotion.'
    }
    $expectedName = $cfg.Machines.DC.ComputerName
    if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        Stop-DeploymentTranscript
        throw "The DC foundation reboot did not establish hostname '$expectedName'."
    }

    if (-not (Test-IsDomainController)) {
        .\Write-Info.ps1 '---- Phase 1: Domain Controller Promotion ----' -ForegroundColor Yellow
        try {
            $promotionResult = & "$dscFolder\Invoke-DcImperativeSteps.ps1" -Step 1 `
                -WorkingPath $WorkingPath -HeartbeatPath $heartbeatFile -NoTranscript |
                Select-Object -Last 1
            if ($promotionResult -ne $true) { throw 'DC promotion step returned failure.' }
        }
        catch {
            $phaseError = $_.Exception.Message
            Stop-ToolsPreparationJob -Job $toolsPreparationJob
            .\Write-Error.ps1 "[FAIL] DC promotion failed: $phaseError"
            Stop-DeploymentTranscript
            throw
        }

        if ($null -ne $toolsPreparationJob -and $toolsPreparationJob.State -eq 'Completed') {
            try { Complete-ToolsPreparationJob -Job $toolsPreparationJob } catch {
                .\Write-Info.ps1 "[WARN] Package preparation will retry after promotion: $($_.Exception.Message)" -ForegroundColor Yellow
            }
        } else {
            Stop-ToolsPreparationJob -Job $toolsPreparationJob
        }
        $toolsPreparationJob = $null

        $rebootCount = [int](Get-RegistryValue -Name $promotionRebootCountName -DefaultValue 0)
        if ($rebootCount -ge 1) {
            Stop-DeploymentTranscript
            throw 'The DC requested another promotion reboot after its planned promotion reboot.'
        }
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        Set-RegistryValue -Name 'DeployStep' -Value 1 -Type DWord
        Set-RegistryValue -Name $promotionRebootCountName -Value 1 -Type DWord
        Set-RegistryValue -Name $promotionPendingName -Value 1 -Type DWord
        $bootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime.ToUniversalTime().ToString('o')
        Set-RegistryValue -Name $promotionBootTimeName -Value $bootTime -Type String
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-DC.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder
        Stop-DeploymentTranscript
        return
    }

    Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
    $currentPhase = 2
}

# ===========================================================================
# Phase 2: Stable AD, tools, directory objects, and convergence
# ===========================================================================
if ($currentPhase -eq 2) {
    if (Test-PendingSystemReboot) {
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        Stop-DeploymentTranscript
        throw 'A reboot remains pending after Domain Controller promotion.'
    }
    if (-not (Test-IsDomainController)) {
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        Stop-DeploymentTranscript
        throw 'The promotion reboot completed, but the server is not a Domain Controller.'
    }

    .\Write-Info.ps1 '---- Phase 2: AD Provisioning + Convergence ----' -ForegroundColor Yellow
    try {
        Complete-ToolsPreparationJob -Job $toolsPreparationJob
        $toolsPreparationJob = $null
        if (-not (Test-Path $toolsPreparedSignal) -and -not (Test-Path $toolsSignal)) {
            $prepared = & $toolsInstaller -Role 'DC' -Operation Prepare `
                -PreparedSignalFile $toolsPreparedSignal -NoTranscript | Select-Object -Last 1
            if (-not $prepared -or -not (Test-Path $toolsPreparedSignal)) {
                throw 'Required DC packages could not be prepared.'
            }
        }
        if (-not (Test-Path $toolsSignal)) {
            $installed = & $toolsInstaller -Role 'DC' -Operation Install `
                -PreparedSignalFile $toolsPreparedSignal -NoTranscript | Select-Object -Last 1
            if (-not $installed -or -not (Test-Path $toolsSignal)) {
                throw 'Required DC tools could not be installed.'
            }
        }

        Remove-Item -Path $adOperationalSignal -Force -ErrorAction SilentlyContinue
        $imperativeResult = & "$dscFolder\Invoke-DcImperativeSteps.ps1" -Step 2 `
            -WorkingPath $WorkingPath -HeartbeatPath $heartbeatFile -NoTranscript |
            Select-Object -Last 1
        if ($imperativeResult -ne $true) {
            throw 'DC post-promotion imperative step returned failure.'
        }
        "AD OPERATIONAL $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
            Set-Content -Path $adOperationalSignal -Force

        . "$dscFolder\DC-Configuration.ps1"
        DcConfiguration -ConfigFilePath $configFile -OutputPath $convergenceMofFolder
        Invoke-VerifiedDscConfiguration -Path $convergenceMofFolder `
            -OperationName 'DC post-promotion convergence configuration' `
            -Postcondition { Test-RequiredDcConvergenceState } `
            -HeartbeatPath $heartbeatFile -PhaseName 'Convergence' | Out-Null
    }
    catch {
        $phaseError = $_.Exception.Message
        .\Write-Error.ps1 "[FAIL] DC post-promotion phase failed: $phaseError"
        Stop-DeploymentTranscript
        throw
    }

    if (-not (Test-RequiredDcReadyState)) {
        Stop-DeploymentTranscript
        throw 'DC post-promotion work completed, but final readiness postconditions are incomplete.'
    }
    if (Test-PendingSystemReboot) {
        Stop-DeploymentTranscript
        throw 'An unexpected third DC reboot became pending after convergence.'
    }

    try {
        Write-VerifiedDeploymentSignal -Path $signalFile `
            -Content "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Failed to commit DC completion signal: $($_.Exception.Message)"
        Stop-DeploymentTranscript
        throw
    }

    Set-DeploymentPhase -Name $phaseRegistryName -Phase 3
    Set-RegistryValue -Name 'DeployStep' -Value 2 -Type DWord
    Set-RegistryValue -Name 'Phase1aDscApplied' -Value 0 -Type DWord
    Set-RegistryValue -Name $foundationRebootCountName -Value 0 -Type DWord
    Set-RegistryValue -Name $promotionRebootCountName -Value 0 -Type DWord
    Remove-ResumeTask
    Remove-Item -Path $heartbeatFile -Force -ErrorAction SilentlyContinue

    $cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"
    if (Test-Path $cleanupScript) { & $cleanupScript }
    .\Write-Info.ps1 '[OK] DC deployment completed with all readiness postconditions verified.' -ForegroundColor Green
}

Stop-DeploymentTranscript
