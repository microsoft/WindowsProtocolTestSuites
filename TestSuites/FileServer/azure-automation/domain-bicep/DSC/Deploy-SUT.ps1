# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Deterministic phased deployment for the Domain SUT.

.DESCRIPTION
    Phase 0 prepares disruptive features and tools, joins the domain, and uses
    one normal member reboot for both feature servicing and domain membership.
    Phase 1 applies non-disruptive DSC convergence. Phase 2 installs prepared
    tools while the domain environment is configured and verified.
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
$registryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
$phaseRegistryName = 'DomainSutDeployPhase'
$memberRebootPendingName = 'DomainSutMemberRebootPending'
$memberPreBootTimeName = 'DomainSutPreRebootBootTimeUtc'
$memberRebootCountName = 'DomainSutMemberRebootCount'
$renameRebootPendingName = 'DomainSutRenameRebootPending'
$renamePreBootTimeName = 'DomainSutPreRenameBootTimeUtc'
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

function Test-RequiredSutFeatureState {
    $missing = @()
    $marker = Get-ItemProperty -Path $registryPath `
        -Name 'DomainSutFeatureBundleAttempted' -ErrorAction SilentlyContinue
    if ($null -eq $marker) { $missing += 'feature-bundle-marker' }
    foreach ($featureName in $requiredSutFeatures) {
        $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
        if ($null -eq $feature -or $feature.InstallState -ne 'Installed') {
            $missing += "feature:$featureName"
        }
    }
    if ($missing.Count -gt 0) {
        .\Write-Info.ps1 "[WARN] Required Domain SUT feature state is incomplete: $($missing -join ', ')" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-RequiredSutDscState {
    $missing = @()
    if (-not (Test-RequiredSutFeatureState)) { $missing += 'feature-bundle' }
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
        .\Write-Info.ps1 "[WARN] Required Domain SUT convergence state is incomplete: $($missing -join ', ')" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-RequiredSutDomainState {
    $partOfDomain = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
    $secureChannel = $false
    try { $secureChannel = Test-ComputerSecureChannel -ErrorAction Stop } catch {}
    return $partOfDomain -and $secureChannel
}

function Test-RequiredSutImperativeState {
    $missing = @()
    foreach ($path in @(
        'C:\SMBBasic\symboliclink',
        'C:\SMBBasic\sub\symboliclink2',
        'C:\FileShare\ExistingFolder',
        'C:\FileShare\ExistingFile.txt',
        'C:\DomainBased.txt'
    )) {
        if (-not (Test-Path -LiteralPath $path)) {
            $missing += "imperative-path:$path"
        }
    }
    $passwordMarker = Get-ItemProperty -Path $registryPath `
        -Name 'ComputerPasswordSet' -ErrorAction SilentlyContinue
    if ($null -eq $passwordMarker) { $missing += 'computer-password-marker' }
    foreach ($namespace in @('SMBDfs', 'Standalone')) {
        $dfsRoot = "\\$env:COMPUTERNAME\$namespace"
        & dfsutil.exe root $dfsRoot *> $null
        if ($LASTEXITCODE -ne 0) { $missing += "dfs-root:$dfsRoot" }
    }
    if ($missing.Count -gt 0) {
        .\Write-Info.ps1 "[WARN] Required Domain SUT imperative state is incomplete: $($missing -join ', ')" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-RequiredSutReadyState {
    return (Test-Path $fullDscSignal) -and
           (Test-Path $toolsSignal) -and
           (Test-RequiredSutDscState) -and
           (Test-RequiredSutDomainState) -and
           (Test-RequiredSutImperativeState)
}

function Get-DomainSutRepairPhase {
    if ((Test-RequiredSutDscState) -and (Test-RequiredSutDomainState)) {
        return 2
    }
    if ((Test-RequiredSutFeatureState) -and
        (Test-RequiredSutDomainState) -and
        -not (Test-PendingSystemReboot)) {
        return 1
    }
    return 0
}

function Start-ToolsPreparationJob {
    if ((Test-Path $toolsPreparedSignal) -or (Test-Path $toolsSignal)) { return $null }
    .\Write-Info.ps1 'Starting parallel SUT tool package preparation...' -ForegroundColor Cyan
    $jobLog = "$scriptsPath\InstallMSIAndTools.prepare.job.log"
    return Start-Job -ScriptBlock {
        param($installer, $scriptsDirectory, $preparedSignal, $log)
        Set-Location $scriptsDirectory
        $env:Path += ";$scriptsDirectory"
        $output = @(& $installer -Role 'SUT' -Operation Prepare `
            -PreparedSignalFile $preparedSignal -NoTranscript *>&1)
        $output | Out-File -FilePath $log -Force
        if ($output.Count -eq 0 -or $output[-1] -ne $true) {
            throw 'Required Domain SUT package preparation failed.'
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
            -Phase 'ToolsPrepare' -Operation 'Parallel Domain SUT package preparation' `
            -HeartbeatPath $heartbeatFile -LastCheckpoint 'Domain SUT orchestration active' | Out-Null
    }
    finally {
        Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
    }
}

.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan
.\Write-Info.ps1 '  Domain SUT -- Deterministic Phased Deployment            ' -ForegroundColor Cyan
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
    .\Write-Error.ps1 "[FAIL] Domain SUT preflight failed: $($_.Exception.Message)"
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
    'RSAT-Hyper-V-Tools',
    'FS-Resource-Manager'
)

$staleReboot = Get-ScheduledTask -TaskName 'PostDeployReboot' -ErrorAction SilentlyContinue
if ($null -ne $staleReboot) {
    Unregister-ScheduledTask -TaskName 'PostDeployReboot' -Confirm:$false
    .\Write-Info.ps1 '[OK] Cancelled stale PostDeployReboot task.' -ForegroundColor Yellow
}

if ((Test-Path $signalFile) -and (Test-RequiredSutReadyState)) {
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 3
    Set-RegistryValue -Name $memberRebootCountName -Value 0 -Type DWord
    Remove-ResumeTask
    .\Write-Info.ps1 '[OK] Domain SUT deployment already completed and remains ready.' -ForegroundColor Green
    Stop-DeploymentTranscript
    return
}
if (Test-Path $signalFile) {
    .\Write-Info.ps1 '[WARN] Removing stale Domain SUT completion signal.' -ForegroundColor Yellow
    Remove-Item -Path $signalFile -Force
    $repairPhase = Get-DomainSutRepairPhase
    Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase
}

$currentPhase = Get-DeploymentPhase -Name $phaseRegistryName
if ($currentPhase -ge 3 -and -not (Test-Path $signalFile)) {
    $repairPhase = Get-DomainSutRepairPhase
    Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase
    $currentPhase = $repairPhase
    .\Write-Info.ps1 "[WARN] Completion signal is absent; reset persisted Domain SUT Phase 3 to repair Phase $repairPhase." -ForegroundColor Yellow
}

if ([int](Get-RegistryValue -Name $renameRebootPendingName -DefaultValue 0) -eq 1) {
    if (-not (Test-NewBoot -BootTimeRegistryName $renamePreBootTimeName)) {
        Stop-DeploymentTranscript
        throw 'The Domain SUT hostname repair reboot was persisted but not observed.'
    }
    Set-RegistryValue -Name $renameRebootPendingName -Value 0 -Type DWord
    .\Write-Info.ps1 '[OK] Hostname repair reboot completed.' -ForegroundColor Green
}
if ($currentPhase -ge 1 -and
    [int](Get-RegistryValue -Name $memberRebootPendingName -DefaultValue 0) -eq 1) {
    if (-not (Test-NewBoot -BootTimeRegistryName $memberPreBootTimeName)) {
        Stop-DeploymentTranscript
        throw 'The Domain SUT member reboot was persisted but not observed.'
    }
    Set-RegistryValue -Name $memberRebootPendingName -Value 0 -Type DWord
    .\Write-Info.ps1 '[OK] Combined feature/domain-join reboot completed.' -ForegroundColor Green
}

$oldDeployStep = [int](Get-RegistryValue -Name 'DeployStep' -DefaultValue 0)
if ($currentPhase -eq 0 -and -not (Test-PendingSystemReboot)) {
    if ($oldDeployStep -ge 2 -and (Test-RequiredSutDscState) -and (Test-RequiredSutDomainState)) {
        "FULL DSC APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
            Set-Content -Path $fullDscSignal -Force
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        $currentPhase = 2
        .\Write-Info.ps1 '[OK] Migrated existing converged Domain SUT state to Phase 2.' -ForegroundColor Green
    }
    elseif ($oldDeployStep -ge 1 -and
            (Test-RequiredSutFeatureState) -and (Test-RequiredSutDomainState)) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        $currentPhase = 1
        .\Write-Info.ps1 '[OK] Migrated existing joined Domain SUT state to Phase 1.' -ForegroundColor Green
    }
}

# Hostname repair must complete before domain join. Azure-created VMs normally
# already have the configured name, so this is an exceptional custom-image path.
if ($currentPhase -eq 0) {
    $expectedName = $cfg.Machines.SUT.ComputerName
    if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
        Rename-Computer -NewName $expectedName -Force
        Set-RegistryValue -Name $renameRebootPendingName -Value 1 -Type DWord
        $bootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime.ToUniversalTime().ToString('o')
        Set-RegistryValue -Name $renamePreBootTimeName -Value $bootTime -Type String
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-SUT.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder
        Stop-DeploymentTranscript
        return
    }
}

$toolsPreparationJob = Start-ToolsPreparationJob

# ===========================================================================
# Phase 0: Features, domain join, and one normal member reboot
# ===========================================================================
if ($currentPhase -eq 0) {
    .\Write-Info.ps1 '---- Phase 0: Features + Domain Join ----' -ForegroundColor Yellow
    try {
        . "$dscFolder\SUT-FeatureConfiguration.ps1"
        SutFeatureConfiguration -OutputPath $featureMofFolder
        Remove-Item -Path $featureSignal -Force -ErrorAction SilentlyContinue
        Invoke-VerifiedDscConfiguration -Path $featureMofFolder `
            -OperationName 'Domain SUT pre-reboot feature configuration' `
            -Postcondition { Test-RequiredSutFeatureState } `
            -HeartbeatPath $heartbeatFile -PhaseName 'Features' | Out-Null
        "FEATURES APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
            Set-Content -Path $featureSignal -Force

        $alreadyJoined = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
        if (-not $alreadyJoined) {
            Write-DeploymentHeartbeat -Phase 'DomainJoin' -Operation 'Joining the Domain SUT to Active Directory' `
                -StartedAt (Get-Date) -HeartbeatPath $heartbeatFile -LastCheckpoint 'Feature phase complete'
            $joinResult = & "$dscFolder\Invoke-SutImperativeSteps.ps1" -Step 1 `
                -WorkingPath $WorkingPath -HeartbeatPath $heartbeatFile -NoTranscript |
                Select-Object -Last 1
            if ($joinResult -ne $true) { throw 'Domain join step returned failure.' }
        }
    }
    catch {
        $phaseError = $_.Exception.Message
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        .\Write-Error.ps1 "[FAIL] Domain SUT pre-reboot phase failed: $phaseError"
        Stop-DeploymentTranscript
        throw
    }

    $rebootRequired = -not (Test-RequiredSutDomainState) -or (Test-PendingSystemReboot)
    if ($rebootRequired) {
        if ($null -ne $toolsPreparationJob -and $toolsPreparationJob.State -eq 'Completed') {
            try { Complete-ToolsPreparationJob -Job $toolsPreparationJob } catch {
                .\Write-Info.ps1 "[WARN] Package preparation will retry after reboot: $($_.Exception.Message)" -ForegroundColor Yellow
            }
        } else {
            Stop-ToolsPreparationJob -Job $toolsPreparationJob
            .\Write-Info.ps1 '[INFO] Paused unfinished package preparation for the member reboot.' -ForegroundColor Yellow
        }
        $toolsPreparationJob = $null

        $memberRebootCount = [int](Get-RegistryValue -Name $memberRebootCountName -DefaultValue 0)
        if ($memberRebootCount -ge 1) {
            Stop-DeploymentTranscript
            throw 'The Domain SUT requested another normal member reboot after its planned reboot.'
        }
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        Set-RegistryValue -Name 'DeployStep' -Value 1 -Type DWord
        Set-RegistryValue -Name $memberRebootCountName -Value 1 -Type DWord
        Set-RegistryValue -Name $memberRebootPendingName -Value 1 -Type DWord
        $bootTime = (Get-CimInstance Win32_OperatingSystem).LastBootUpTime.ToUniversalTime().ToString('o')
        Set-RegistryValue -Name $memberPreBootTimeName -Value $bootTime -Type String
        $domainNetBios = if ($cfg.Domain.NetBiosName) {
            $cfg.Domain.NetBiosName
        } else {
            $cfg.Core.DomainName.Split('.')[0].ToUpper()
        }
        Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-SUT.ps1" `
            -WorkingPath $WorkingPath -DscFolder $dscFolder `
            -RunAsUser "$domainNetBios\$($cfg.Core.Username)" `
            -RunAsPassword $cfg.Core.Password
        Stop-DeploymentTranscript
        return
    }
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
    Set-RegistryValue -Name 'DeployStep' -Value 1 -Type DWord
    $currentPhase = 1
}

# ===========================================================================
# Phase 1: Joined-machine convergence
# ===========================================================================
if ($currentPhase -eq 1) {
    if (Test-PendingSystemReboot) {
        Stop-DeploymentTranscript
        throw 'A reboot remains pending before Domain SUT convergence.'
    }
    if (-not (Test-RequiredSutDomainState)) {
        Stop-DeploymentTranscript
        throw 'Domain SUT membership or secure channel is not ready after reboot.'
    }

    .\Write-Info.ps1 '---- Phase 1: Post-Reboot DSC Convergence ----' -ForegroundColor Yellow
    try {
        . "$dscFolder\SUT-Configuration.ps1"
        SutConfiguration -ConfigFilePath $configFile -OutputPath $convergenceMofFolder
        Remove-Item -Path $fullDscSignal -Force -ErrorAction SilentlyContinue
        Invoke-VerifiedDscConfiguration -Path $convergenceMofFolder `
            -OperationName 'Domain SUT post-reboot convergence configuration' `
            -Postcondition { Test-RequiredSutDscState } `
            -HeartbeatPath $heartbeatFile -PhaseName 'Convergence' | Out-Null
        "FULL DSC APPLIED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
            Set-Content -Path $fullDscSignal -Force
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        Set-RegistryValue -Name 'DeployStep' -Value 2 -Type DWord
        $currentPhase = 2
    }
    catch {
        $phaseError = $_.Exception.Message
        Stop-ToolsPreparationJob -Job $toolsPreparationJob
        .\Write-Error.ps1 "[FAIL] Domain SUT convergence failed: $phaseError"
        Stop-DeploymentTranscript
        throw
    }
}

# ===========================================================================
# Phase 2: Domain environment and controlled tool installation
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
                throw 'Required Domain SUT packages could not be prepared.'
            }
        }
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Domain SUT package preparation failed: $($_.Exception.Message)"
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
                throw 'Required Domain SUT tool installation failed.'
            }
        } -ArgumentList $toolsInstaller, $scriptsPath, $toolsPreparedSignal, $toolsInstallLog
    }

    $environmentOk = $false
    try {
        $result = & "$dscFolder\Invoke-SutImperativeSteps.ps1" -Step 3 `
            -WorkingPath $WorkingPath -HeartbeatPath $heartbeatFile -NoTranscript |
            Select-Object -Last 1
        $environmentOk = $result -eq $true -and
                         (Test-RequiredSutImperativeState) -and
                         (Test-RequiredSutDomainState)
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Domain SUT environment setup failed: $($_.Exception.Message)"
    }

    if ($null -ne $toolsInstallJob) {
        try {
            Wait-DeploymentJob -Job $toolsInstallJob -TimeoutSeconds $toolsJobTimeoutSeconds `
                -Phase 'ToolsInstall' -Operation 'Controlled Domain SUT tool installation' `
                -HeartbeatPath $heartbeatFile -LastCheckpoint 'Domain SUT convergence complete' | Out-Null
        }
        catch {
            .\Write-Error.ps1 "[FAIL] Domain SUT tool installation failed: $($_.Exception.Message)"
        }
        finally {
            Remove-Job -Job $toolsInstallJob -Force -ErrorAction SilentlyContinue
        }
    }

    $toolsOk = Test-Path $toolsSignal
    if (-not $environmentOk -or -not $toolsOk -or -not (Test-RequiredSutReadyState)) {
        Stop-DeploymentTranscript
        throw "Domain SUT Phase 2 is incomplete (Tools=$toolsOk, Environment=$environmentOk)."
    }
    if (Test-PendingSystemReboot) {
        Stop-DeploymentTranscript
        throw 'An unexpected second Domain SUT reboot became pending after convergence.'
    }

    try {
        Write-VerifiedDeploymentSignal -Path $signalFile `
            -Content "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Failed to commit Domain SUT completion signal: $($_.Exception.Message)"
        Stop-DeploymentTranscript
        throw
    }

    Set-DeploymentPhase -Name $phaseRegistryName -Phase 3
    Set-RegistryValue -Name 'DeployStep' -Value 3 -Type DWord
    Set-RegistryValue -Name $memberRebootCountName -Value 0 -Type DWord
    Remove-ResumeTask
    Remove-Item -Path $heartbeatFile -Force -ErrorAction SilentlyContinue
    .\Write-Info.ps1 '[OK] Domain SUT deployment completed with all postconditions verified.' -ForegroundColor Green
}

Stop-DeploymentTranscript
