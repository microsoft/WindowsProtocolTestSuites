# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Orchestrator for the secondary cluster node (Node02).
    Applies DSC, joins domain, installs features, connects iSCSI,
    polls for Node01 cluster readiness, then configures node shares.

.DESCRIPTION
    Step 0 -> 1: Pre-Domain-Join
      DSC-Lite: Hosts file, firewall, multi-NIC routing.
      Imperative: Domain join.
      -> Deferred reboot

    Step 1 -> 2: Features + Tools + iSCSI
      DSC-Full: Windows features, SMB shares (subset), registry.
      Tools: Background job.
      Imperative: iSCSI target connection.
      -> Conditional reboot (features)

    Step 2 -> 3: Poll for Cluster + Node02 Config
      Poll Node01's cluster readiness (Get-Cluster).
      Imperative: Node02-specific shares, cluster status check.
      -> Finish (signal file written)

.PARAMETER WorkingPath
    Path to the Cluster-Package root folder.

.EXAMPLE
    .\Deploy-Node02.ps1
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder    = $PSScriptRoot
$scriptsPath  = "$dscFolder\Scripts"
$mofFolder    = "$dscFolder\MOF\Node02"
$logFile      = "$dscFolder\Deploy-Node02.log"

$env:Path += ";$scriptsPath"
Push-Location $scriptsPath

Start-Transcript -Path $logFile -Append -Force
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "  Cluster Node02 -- DSC + Imperative Deployment            " -ForegroundColor Cyan
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
$maxRebootCount = 4

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

$signalFile = "$dscFolder\Deploy-Node02.Completed.signal"
if (Test-Path $signalFile) {
    .\Write-Info.ps1 "[OK] Node02 deployment already completed (signal file exists)." -ForegroundColor Green
    Remove-ResumeTask
    Pop-Location; Stop-Transcript; return
}

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

# ===========================================================================
# Pre-check: Validate hostname
# ===========================================================================
if ($null -ne $cfg) {
    try {
        $expectedName = $cfg.Machines.Node02.ComputerName
        if (-not [string]::IsNullOrWhiteSpace($expectedName) -and $env:COMPUTERNAME -ne $expectedName) {
            $currentRebootCount = Get-RebootCount
            if ($currentRebootCount -ge $maxRebootCount) {
                .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered. Skipping rename." -ForegroundColor Red
            } else {
                Set-RebootCount -Count ($currentRebootCount + 1)
                .\Write-Info.ps1 "Renaming computer from $env:COMPUTERNAME to $expectedName..." -ForegroundColor Yellow
                Rename-Computer -NewName $expectedName -Force
                Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Node02.ps1" `
                    -WorkingPath $WorkingPath -DscFolder $dscFolder
                Pop-Location; Stop-Transcript; return
            }
        }
    } catch {
        .\Write-Info.ps1 "[WARN] Could not validate hostname: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# ===========================================================================
# Step 0 -> 1: DSC-Lite + Domain Join
# ===========================================================================
if ($currentStep -lt 1) {
    .\Write-Info.ps1 "---- Phase 1a: DSC Lite (hosts + firewall + routing) ----" -ForegroundColor Yellow
    $phase1sw = [System.Diagnostics.Stopwatch]::StartNew()
    try {
        . "$dscFolder\Node-Configuration.ps1"
        NodeConfiguration -ConfigFilePath $configFile -NodeRole 'Node02' -OutputPath $mofFolder
        Invoke-VerifiedDscConfiguration -Path $mofFolder `
            -OperationName 'Cluster Node02 pre-domain DSC' | Out-Null
        .\Write-Info.ps1 "[OK] DSC Lite applied in $([math]::Round($phase1sw.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Info.ps1 "[WARN] DSC had partial failures (expected pre-domain-join): $($_.Exception.Message)" -ForegroundColor Yellow
    }
    $phase1sw.Stop()

    .\Write-Info.ps1 "---- Phase 1b: Domain Join ----" -ForegroundColor Yellow
    try {
        & "$dscFolder\Invoke-Node02ImperativeSteps.ps1" -Step 1 -WorkingPath $WorkingPath
        .\Write-Info.ps1 "[OK] Domain join complete." -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Domain join failed: $($_.Exception.Message)"
        Pop-Location; Stop-Transcript; return
    }

    Set-DeployStep -Step 1
    # After domain join, resume task must run as domain admin for cross-node access
    $domainNetBios = if ($cfg.Domain -and $cfg.Domain.NetBiosName) { $cfg.Domain.NetBiosName } else { $cfg.Core.DomainName.Split('.')[0].ToUpper() }
    $domainAdminUser = "$domainNetBios\$($cfg.Core.Username)"
    $domainAdminPass = $cfg.Core.Password
    Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Node02.ps1" `
        -WorkingPath $WorkingPath -DscFolder $dscFolder `
        -RunAsUser $domainAdminUser -RunAsPassword $domainAdminPass
    Pop-Location; Stop-Transcript; return
}

# ===========================================================================
# Step 1 -> 2: Full DSC + Tools + iSCSI
# ===========================================================================
if ($currentStep -eq 1) {
    Start-Sleep -Seconds 10

    # Tools (background)
    .\Write-Info.ps1 "---- Phase 2a: Tools Install (background) ----" -ForegroundColor Yellow
    $toolsSignal = "$scriptsPath\InstallMSIAndTools.Completed.signal"
    $toolsJob    = $null
    $toolsJobStart = $null
    $toolsJobTimeoutSeconds = 3600

    if (Test-Path $toolsSignal) {
        .\Write-Info.ps1 "[OK] Tools already installed." -ForegroundColor Green
    }
    else {
        $toolsInstaller = "$scriptsPath\InstallMSIAndTools.ps1"
        $toolsScriptsDir = $scriptsPath
        $toolsJobLog = "$scriptsPath\InstallMSIAndTools.job.log"
        $toolsJobStart = Get-Date
        $toolsJob = Start-Job -ScriptBlock {
            param($wp, $installer, $sd, $jl)
            Set-Location $sd
            $env:Path += ";$sd"
            & $installer -Role 'Node02' *> $jl
        } -ArgumentList $WorkingPath, $toolsInstaller, $toolsScriptsDir, $toolsJobLog
    }

    # Full DSC
    .\Write-Info.ps1 "---- Phase 2b: DSC Full (features, shares, registry) ----" -ForegroundColor Yellow
    $phase2b = [System.Diagnostics.Stopwatch]::StartNew()
    $fullDscOk = $false
    try {
        . "$dscFolder\Node-Configuration.ps1"
        NodeConfiguration -ConfigFilePath $configFile -NodeRole 'Node02' -OutputPath $mofFolder
        Invoke-VerifiedDscConfiguration -Path $mofFolder `
            -OperationName 'Cluster Node02 full DSC' | Out-Null
        $fullDscOk = $true
        .\Write-Info.ps1 "[OK] Full DSC applied in $([math]::Round($phase2b.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Full DSC failed: $($_.Exception.Message)"
    }
    $phase2b.Stop()

    # Wait for tools
    if ($null -ne $toolsJob) {
        .\Write-Info.ps1 "Waiting for tools install..." -ForegroundColor Yellow
        $elapsedSeconds = [int]((Get-Date) - $toolsJobStart).TotalSeconds
        $remainingSeconds = [math]::Max(1, $toolsJobTimeoutSeconds - $elapsedSeconds)
        $completedToolsJob = Wait-Job -Job $toolsJob -Timeout $remainingSeconds
        if ($null -eq $completedToolsJob) {
            Stop-Job -Job $toolsJob -ErrorAction SilentlyContinue
            .\Write-Error.ps1 "[FAIL] Tools job exceeded ${toolsJobTimeoutSeconds}s."
        } elseif ($toolsJob.State -ne 'Completed') {
            .\Write-Error.ps1 "[FAIL] Tools job ended in state '$($toolsJob.State)'."
        }
        Remove-Job -Job $toolsJob -Force -ErrorAction SilentlyContinue
    }
    $toolsOk = Test-Path $toolsSignal

    # iSCSI connection
    .\Write-Info.ps1 "---- Phase 2c: iSCSI Connection ----" -ForegroundColor Yellow
    $iscsiOk = $false
    try {
        & "$dscFolder\Invoke-Node02ImperativeSteps.ps1" -Step 2 -WorkingPath $WorkingPath
        $iscsiOk = $true
        .\Write-Info.ps1 "[OK] iSCSI connected." -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] iSCSI connection failed: $($_.Exception.Message)"
    }

    if (-not ($fullDscOk -and $toolsOk -and $iscsiOk)) {
        Set-DeployStep -Step 1
        throw "Cluster node prerequisites are incomplete (DSC=$fullDscOk, Tools=$toolsOk, iSCSI=$iscsiOk)."
    }

    # Check for pending reboot
    $rebootPending = (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\RebootPending') -or
                     (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update\RebootRequired') -or
                     (Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations')
    if ($rebootPending) {
        $currentRebootCount = Get-RebootCount
        if ($currentRebootCount -ge $maxRebootCount) {
            .\Write-Info.ps1 "[WARN] Reboot circuit breaker triggered. Continuing." -ForegroundColor Red
            Set-DeployStep -Step 2
        } else {
            Set-RebootCount -Count ($currentRebootCount + 1)
            Set-DeployStep -Step 2
            $domainNetBios = if ($cfg.Domain -and $cfg.Domain.NetBiosName) { $cfg.Domain.NetBiosName } else { $cfg.Core.DomainName.Split('.')[0].ToUpper() }
            $domainAdminUser = "$domainNetBios\$($cfg.Core.Username)"
            $domainAdminPass = $cfg.Core.Password
            Register-DeferredRebootAndResume -DeployScript "$dscFolder\Deploy-Node02.ps1" `
                -WorkingPath $WorkingPath -DscFolder $dscFolder `
                -RunAsUser $domainAdminUser -RunAsPassword $domainAdminPass
            Pop-Location; Stop-Transcript; return
        }
    } else {
        Set-DeployStep -Step 2
    }
}

# ===========================================================================
# Step 2 -> 3: Poll for Cluster + Node02 Config
# ===========================================================================
$currentStep = Get-DeployStep
if ($currentStep -ge 2 -and -not (Test-Path $signalFile)) {
    Start-Sleep -Seconds 5

    .\Write-Info.ps1 "---- Phase 3: Poll for Cluster + Node02 Config ----" -ForegroundColor Yellow
    $phase3sw = [System.Diagnostics.Stopwatch]::StartNew()
    $phase3Ok = $false
    try {
        & "$dscFolder\Invoke-Node02ImperativeSteps.ps1" -Step 3 -WorkingPath $WorkingPath
        $phase3Ok = $true
        .\Write-Info.ps1 "[OK] Node02 config complete in $([math]::Round($phase3sw.Elapsed.TotalSeconds))s" -ForegroundColor Green
    }
    catch {
        .\Write-Error.ps1 "[FAIL] Node02 config failed: $($_.Exception.Message)"
    }
    $phase3sw.Stop()

    Set-DeployStep -Step 3
    Set-RebootCount -Count 0

    $stopwatch.Stop()
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
    .\Write-Info.ps1 "  Node02 Deployment Complete ($([math]::Round($stopwatch.Elapsed.TotalMinutes, 1)) min)" -ForegroundColor Cyan
    .\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan

    if ($phase3Ok) {
        "DEPLOY FINISHED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $signalFile -Force
        .\Write-Info.ps1 "[OK] Signal file written: $signalFile" -ForegroundColor Green
        Remove-ResumeTask
    } else {
        .\Write-Info.ps1 "[WARN] Signal file NOT written -- deployment incomplete" -ForegroundColor Yellow
    }

    $cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"
    if (Test-Path $cleanupScript) { & $cleanupScript }
}

Pop-Location
Stop-Transcript
