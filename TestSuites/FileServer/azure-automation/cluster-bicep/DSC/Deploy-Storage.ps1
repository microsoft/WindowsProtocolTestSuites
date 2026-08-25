# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Deterministic phased deployment for the Cluster Storage server.

.DESCRIPTION
    Phase 0 installs disruptive file/iSCSI target features, coalesces an
    optional hostname change, and persists one planned reboot. Phase 1 proves
    that reboot, applies non-disruptive convergence, repairs the target and
    exact four-LUN layout, then publishes verified Storage readiness.
#>

[CmdletBinding()]
param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder = $PSScriptRoot
$scriptsPath = Join-Path $dscFolder 'Scripts'
$featureMofFolder = Join-Path $dscFolder 'MOF\Storage-Features'
$convergenceMofFolder = Join-Path $dscFolder 'MOF\Storage'
$logFile = Join-Path $dscFolder 'Deploy-Storage.log'
$heartbeatFile = Join-Path $dscFolder 'Deploy-Storage.heartbeat.json'
$configFile = Join-Path $WorkingPath 'Config.json'
$phaseRegistryName = 'StorageDeployPhase'
$signalFile = Join-Path $dscFolder 'Deploy-Storage.Completed.signal'
$signalPattern = '^STORAGE READY; SchemaVersion=1\.0;'

$env:Path += ";$scriptsPath"
Push-Location $scriptsPath
Start-Transcript -Path $logFile -Append -Force | Out-Null
$transcriptStopped = $false

function Stop-StorageDeploymentTranscript {
    if (-not $transcriptStopped) {
        $script:transcriptStopped = $true
        Stop-Transcript | Out-Null
        Pop-Location
    }
}

function Test-InstalledStorageFeatures {
    foreach ($featureName in @('File-Services', 'FS-iSCSITarget-Server')) {
        $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
        if ($null -eq $feature -or $feature.InstallState -ne 'Installed') {
            return $false
        }
    }
    return $true
}

function Test-RequiredStorageFeatureState {
    $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
        -Name 'StorageFeatureBundleAttempted' -ErrorAction SilentlyContinue
    return ($null -ne $marker -and (Test-InstalledStorageFeatures))
}

function Test-RequiredStorageConvergenceState {
    if (-not (Test-RequiredStorageFeatureState)) { return $false }
    $service = Get-CimInstance Win32_Service -Filter "Name='WinTarget'" `
        -ErrorAction SilentlyContinue
    if ($null -eq $service -or $service.StartMode -ne 'Auto' -or
        $service.State -ne 'Running') {
        return $false
    }
    $enabledFirewall = Get-NetFirewallProfile -ErrorAction SilentlyContinue |
        Where-Object { $_.Enabled -eq $true }
    if (@($enabledFirewall).Count -gt 0) { return $false }
    try {
        return [bool](Test-DscConfiguration -Path $convergenceMofFolder -ErrorAction Stop)
    }
    catch {
        return $false
    }
}

function Test-RequiredStorageReadyState {
    $output = @(& (Join-Path $scriptsPath 'Test-StorageReadiness.ps1') `
        -ConfigureFile $configFile *>&1)
    $lastResult = Get-LastMeaningfulDeploymentOutput -Output $output
    return (Test-DeploymentSuccessValue -Value $lastResult)
}

.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan
.\Write-Info.ps1 '  Storage -- Deterministic Phased Deployment               ' -ForegroundColor Cyan
.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan

. "$dscFolder\Deploy-CommonHelpers.ps1"

function Register-StoragePlannedReboot {
    .\Write-Info.ps1 'Scheduling the single Storage feature/hostname reboot.' `
        -ForegroundColor Yellow
    Register-DeferredRebootAndResume `
        -DeployScript (Join-Path $dscFolder 'Deploy-Storage.ps1') `
        -WorkingPath $WorkingPath -DscFolder $dscFolder
}

try {
    if (-not (Test-Path -LiteralPath $configFile -PathType Leaf)) {
        throw "Config.json was not found at '$configFile'."
    }
    $config = Get-Content -LiteralPath $configFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    & (Join-Path $scriptsPath 'Validate-ConfigFile.ps1') -ConfigPath $configFile

    $expectedName = "$($config.Machines.Storage.ComputerName)"
    if ([string]::IsNullOrWhiteSpace($expectedName)) {
        throw 'Config.json does not define the Storage computer name.'
    }

    $staleReboot = Get-ScheduledTask -TaskName 'PostDeployReboot' `
        -ErrorAction SilentlyContinue
    if ($null -ne $staleReboot) {
        Unregister-ScheduledTask -TaskName 'PostDeployReboot' -Confirm:$false
    }

    if ((Test-VerifiedDeploymentSignal -Path $signalFile `
            -ExpectedContentPattern $signalPattern) -and
        (Test-RequiredStorageReadyState)) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        Remove-ResumeTask
        Remove-Item -LiteralPath $heartbeatFile -Force -ErrorAction SilentlyContinue
        .\Write-Info.ps1 '[OK] Storage deployment remains complete and ready.' `
            -ForegroundColor Green
        return
    }

    if (Test-Path -LiteralPath $signalFile -PathType Leaf) {
        .\Write-Info.ps1 '[WARN] Removing stale or unverifiable Storage signal.' `
            -ForegroundColor Yellow
        Remove-Item -LiteralPath $signalFile -Force
    }

    $currentPhase = Get-DeploymentPhase -Name $phaseRegistryName
    $oldDeployStep = [int](Get-DeploymentRegistryValue -Name 'DeployStep' -DefaultValue 0)
    if ($currentPhase -eq 0 -and $oldDeployStep -ge 1 -and
        (Test-InstalledStorageFeatures)) {
        Set-DeploymentRegistryValue -Name 'StorageFeatureBundleAttempted' `
            -Value 1 -Type DWord
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        $currentPhase = 1
        .\Write-Info.ps1 (
            "[OK] Migrated legacy Storage DeployStep $oldDeployStep to deterministic Phase 1."
        ) -ForegroundColor Green
        if (Test-PendingSystemReboot) {
            Set-DeploymentRebootPending -Role 'Storage' -MaximumRebootCount 1 | Out-Null
            Register-StoragePlannedReboot
            return
        }
        Set-DeploymentRegistryValue -Name 'StorageRebootPending' -Value 0 -Type DWord
    }

    if ($currentPhase -ge 2 -and -not (Test-VerifiedDeploymentSignal -Path $signalFile -ExpectedContentPattern $signalPattern)) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        $currentPhase = 1
        .\Write-Info.ps1 (
            '[WARN] Storage Phase 2 lacked a valid signal; reset to repair Phase 1.'
        ) -ForegroundColor Yellow
    }

    $stateNames = Get-DeploymentRoleStateNames -Role 'Storage'
    $rebootPending = [int](Get-DeploymentRegistryValue `
        -Name $stateNames.RebootPendingName -DefaultValue 0)
    if ($currentPhase -ge 1 -and $rebootPending -eq 1) {
        Confirm-DeploymentReboot -Role 'Storage' | Out-Null
        .\Write-Info.ps1 '[OK] Storage feature/hostname reboot was proven.' `
            -ForegroundColor Green
    }

    if ($currentPhase -eq 0) {
        if ($env:COMPUTERNAME -ne $expectedName) {
            Rename-Computer -NewName $expectedName -Force -ErrorAction Stop
            .\Write-Info.ps1 (
                "Storage hostname change to '$expectedName' will complete at the planned reboot."
            ) -ForegroundColor Yellow
        }

        . (Join-Path $dscFolder 'Storage-FeatureConfiguration.ps1')
        StorageFeatureConfiguration -OutputPath $featureMofFolder
        Invoke-VerifiedDscConfiguration -Path $featureMofFolder `
            -OperationName 'Storage feature configuration' `
            -PhaseName 'StorageFeatures' -HeartbeatPath $heartbeatFile `
            -Postcondition { Test-RequiredStorageFeatureState } | Out-Null

        Set-DeploymentRebootPending -Role 'Storage' -MaximumRebootCount 1 | Out-Null
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        Register-StoragePlannedReboot
        return
    }

    if ($env:COMPUTERNAME -ne $expectedName) {
        throw "Storage reboot completed without applying hostname '$expectedName'."
    }
    if (Test-PendingSystemReboot) {
        throw 'An unexpected reboot remains pending before Storage convergence.'
    }

    . (Join-Path $dscFolder 'Storage-Configuration.ps1')
    StorageConfiguration -ConfigFilePath $configFile `
        -OutputPath $convergenceMofFolder
    Invoke-VerifiedDscConfiguration -Path $convergenceMofFolder `
        -OperationName 'Storage convergence configuration' `
        -PhaseName 'StorageConvergence' -HeartbeatPath $heartbeatFile `
        -Postcondition { Test-RequiredStorageConvergenceState } | Out-Null

    $imperativeOutput = @(& (Join-Path $dscFolder 'Invoke-StorageImperativeSteps.ps1') `
        -WorkingPath $WorkingPath -ConfigureFile $configFile `
        -HeartbeatPath $heartbeatFile -NoTranscript *>&1)
    $imperativeOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
    Assert-DeploymentChildResult -Output $imperativeOutput `
        -Operation 'Storage iSCSI target convergence' -RequireTrueResult | Out-Null

    if (-not (Test-RequiredStorageReadyState)) {
        throw 'Storage readiness regressed after imperative convergence.'
    }
    if (Test-PendingSystemReboot) {
        throw 'Storage convergence requested an unexpected second normal reboot.'
    }

    $signalContent = "STORAGE READY; SchemaVersion=1.0; TimestampUtc=$((Get-Date).ToUniversalTime().ToString('o'))"
    Write-VerifiedDeploymentSignal -Path $signalFile -Content $signalContent
    if (-not (Test-VerifiedDeploymentSignal -Path $signalFile `
            -ExpectedContentPattern $signalPattern)) {
        throw 'Storage readiness signal verification failed after writing.'
    }
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
    Remove-ResumeTask
    Remove-Item -LiteralPath $heartbeatFile -Force -ErrorAction SilentlyContinue
    .\Write-Info.ps1 '[OK] Storage deployment completed with verified readiness.' `
        -ForegroundColor Green
}
catch {
    $failureMessage = "Storage deployment failed: $($_.Exception.Message)"
    .\Write-Error.ps1 $failureMessage
    Stop-DeploymentForTerminalFailure -Message $failureMessage `
        -Phase 'Storage' -Operation 'Deterministic Storage deployment' `
        -HeartbeatPath $heartbeatFile
}
finally {
    Stop-StorageDeploymentTranscript
}
