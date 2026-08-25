# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Deterministic pre-cluster deployment shared by Node01 and Node02.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet('Node01', 'Node02')]
    [string]$NodeRole,

    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder = $PSScriptRoot
$scriptsPath = Join-Path $dscFolder 'Scripts'
$featureMofFolder = Join-Path $dscFolder "MOF\$NodeRole-Features"
$convergenceMofFolder = Join-Path $dscFolder "MOF\$NodeRole"
$logFile = Join-Path $dscFolder "Deploy-$NodeRole.log"
$heartbeatFile = Join-Path $dscFolder "Deploy-$NodeRole.heartbeat.json"
$configFile = Join-Path $WorkingPath 'Config.json'
$roleStateName = "Cluster$NodeRole"
$phaseRegistryName = "Cluster${NodeRole}DeployPhase"
$preReadySignal = Join-Path $dscFolder "$NodeRole.PreClusterReady.signal"
$preReadyPattern = "^NODE PRECLUSTER READY; SchemaVersion=1\.0; Role=$NodeRole;"
$finalSignal = Join-Path $dscFolder "Deploy-$NodeRole.Completed.signal"
$finalPattern = "^NODE COMPLETE; SchemaVersion=1\.0; Role=$NodeRole;"
$clusterReadySignal = Join-Path $dscFolder 'Deploy-Cluster.Completed.signal'
$clusterReadyPattern = '^CLUSTER READY; SchemaVersion=1\.0;'
$toolsSignal = Join-Path $scriptsPath 'InstallMSIAndTools.Completed.signal'
$toolsPreparedSignal = Join-Path $scriptsPath 'InstallMSIAndTools.Prepared.signal'
$toolsInstaller = Join-Path $scriptsPath 'InstallMSIAndTools.ps1'
$toolsJobTimeoutSeconds = 3600
$kerberosAlignmentVersion = 1
$kerberosAlignmentMarkerName = 'KerberosMachinePasswordAlignmentVersion'

$env:Path += ";$scriptsPath"
Push-Location $scriptsPath
Start-Transcript -Path $logFile -Append -Force | Out-Null
$transcriptStopped = $false

function Stop-NodeDeploymentTranscript {
    if (-not $transcriptStopped) {
        $script:transcriptStopped = $true
        Stop-Transcript | Out-Null
        Pop-Location
    }
}

function Test-InstalledNodeFeatures {
    foreach ($featureName in @(
        'Failover-Clustering',
        'RSAT-Clustering',
        'RSAT-Clustering-Mgmt',
        'RSAT-Clustering-PowerShell',
        'RSAT-Clustering-CmdInterface',
        'File-Services',
        'FS-BranchCache',
        'FS-VSS-Agent',
        'BranchCache',
        'FS-DFS-Namespace',
        'RSAT-File-Services',
        'RSAT-DFS-Mgmt-Con',
        'FS-Resource-Manager',
        'RSAT-AD-PowerShell'
    )) {
        $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
        if ($null -eq $feature -or $feature.InstallState -ne 'Installed') {
            return $false
        }
    }
    return $true
}

function Test-RequiredNodeFeatureState {
    $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
        -Name 'ClusterNodeFeatureBundleAttempted' -ErrorAction SilentlyContinue
    return ($null -ne $marker -and (Test-InstalledNodeFeatures))
}

function Test-RequiredNodeDomainState {
    $computerSystem = Get-CimInstance Win32_ComputerSystem -ErrorAction SilentlyContinue
    if ($null -eq $computerSystem -or -not $computerSystem.PartOfDomain -or
        "$($computerSystem.Domain)" -ine "$($config.Core.DomainName)") {
        return $false
    }
    try {
        return [bool](Test-ComputerSecureChannel -ErrorAction Stop)
    }
    catch {
        return $false
    }
}

function Test-RequiredNodeDscState {
    if (-not (Test-RequiredNodeFeatureState) -or
        -not (Test-RequiredNodeDomainState)) {
        return $false
    }
    try {
        if (-not (Test-DscConfiguration -Path $convergenceMofFolder -ErrorAction Stop)) {
            return $false
        }
    }
    catch {
        return $false
    }
    $enabledFirewall = Get-NetFirewallProfile -ErrorAction SilentlyContinue |
        Where-Object { $_.Enabled -eq $true }
    if (@($enabledFirewall).Count -gt 0) { return $false }
    $signing = Get-SmbServerConfiguration -ErrorAction SilentlyContinue
    return ($null -ne $signing -and $signing.RequireSecuritySignature)
}

function Test-RequiredNodeToolState {
    if (-not (Test-VerifiedDeploymentSignal -Path $toolsSignal `
            -ExpectedContentPattern '^Completed ')) {
        return $false
    }
    foreach ($path in @(
        "$env:ProgramFiles\PowerShell\7\pwsh.exe",
        "$env:SystemDrive\OpenSSH-Win64\ssh.exe"
    )) {
        if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
            return $false
        }
    }
    return $true
}

function Test-RequiredNodeFoundationState {
    $output = @(& (Join-Path $scriptsPath 'Test-NodeFoundationReadiness.ps1') `
        -NodeRole $NodeRole -ConfigureFile $configFile `
        -MofPath $convergenceMofFolder *>&1)
    $lastResult = Get-LastMeaningfulDeploymentOutput -Output $output
    return (Test-DeploymentSuccessValue -Value $lastResult)
}

function Test-RequiredNodeKerberosAlignment {
    if ($NodeRole -ne 'Node01') { return $true }

    $marker = [int](Get-DeploymentRegistryValue `
        -Name $kerberosAlignmentMarkerName -DefaultValue 0)
    if ($marker -ne $kerberosAlignmentVersion) { return $false }

    $netlogon = Get-ItemProperty `
        -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters' `
        -Name DisablePasswordChange -ErrorAction SilentlyContinue
    if ($null -eq $netlogon -or $netlogon.DisablePasswordChange -ne 1) {
        return $false
    }

    try {
        return [bool](Test-ComputerSecureChannel -ErrorAction Stop)
    }
    catch {
        return $false
    }
}

function Start-NodeKerberosAlignment {
    $alignmentScript = Join-Path $scriptsPath `
        'Set-KerberosMachinePasswordAlignment.ps1'
    $alignmentOutput = @(& $alignmentScript -ConfigFile $configFile *>&1)
    $alignmentOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
    Assert-DeploymentChildResult -Output $alignmentOutput `
        -Operation 'Node01 Kerberos machine-password alignment' `
        -RequireTrueResult | Out-Null

    Set-DeploymentRebootPending -Role $roleStateName `
        -RebootScope 'KerberosAlignment' -MaximumRebootCount 1 | Out-Null
    .\Write-Info.ps1 (
        'Scheduling the required Node01 reboot after Kerberos machine-password alignment.'
    ) -ForegroundColor Yellow
    Register-NodePlannedReboot
}

function Start-NodeToolsPreparation {
    if ((Test-Path -LiteralPath $toolsPreparedSignal) -or
        (Test-Path -LiteralPath $toolsSignal)) {
        return $null
    }
    $jobLog = Join-Path $scriptsPath "InstallMSIAndTools.$NodeRole.prepare.job.log"
    return Start-Job -ScriptBlock {
        param($installer, $role, $scriptsDirectory, $preparedSignal, $log)
        Set-Location $scriptsDirectory
        $output = @(& $installer -Role $role -Operation Prepare `
            -PreparedSignalFile $preparedSignal -NoTranscript *>&1)
        $output | Out-File -FilePath $log -Force
        if ($output.Count -eq 0 -or $output[-1] -ne $true) {
            throw "Required $role package preparation failed."
        }
    } -ArgumentList $toolsInstaller, $NodeRole, $scriptsPath, $toolsPreparedSignal, $jobLog
}

function Complete-NodeToolsPreparation {
    param([System.Management.Automation.Job]$Job)
    if ($null -eq $Job) { return }
    try {
        Wait-DeploymentJob -Job $Job -TimeoutSeconds $toolsJobTimeoutSeconds `
            -Phase 'ToolsPrepare' -Operation "$NodeRole package preparation" `
            -HeartbeatPath $heartbeatFile -LastCheckpoint 'Node feature/domain phase active' |
            Out-Null
    }
    finally {
        Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
    }
}

function Register-NodePlannedReboot {
    param([switch]$AsSystem)

    $rebootParameters = @{
        DeployScript = (Join-Path $dscFolder "Deploy-$NodeRole.ps1")
        WorkingPath = $WorkingPath
        DscFolder = $dscFolder
    }
    if (-not $AsSystem) {
        $domainNetBios = if ($config.Domain.NetBiosName) {
            "$($config.Domain.NetBiosName)"
        } else {
            "$($config.Core.DomainName)".Split('.')[0].ToUpperInvariant()
        }
        $rebootParameters['RunAsUser'] = "$domainNetBios\$($config.Core.Username)"
        $rebootParameters['RunAsPassword'] = $config.Core.Password
    }
    Register-DeferredRebootAndResume @rebootParameters
}

function Complete-ClusterPhase {
    if ($NodeRole -eq 'Node01') {
        $node02Name = "$($config.Machines.Node02.ComputerName)"
        $node02SignalPath = 'C:\Cluster-Package\DSC\Node02.PreClusterReady.signal'
        try {
            Wait-DeploymentCondition -Condition {
                Invoke-Command -ComputerName $node02Name -ScriptBlock {
                    param($path)
                    $item = Get-Item -LiteralPath $path -ErrorAction SilentlyContinue
                    if ($null -eq $item -or $item.Length -le 0) { return $false }
                    $content = Get-Content -LiteralPath $path -Raw -ErrorAction SilentlyContinue
                    return $content -match '^NODE PRECLUSTER READY; SchemaVersion=1\.0; Role=Node02;'
                } -ArgumentList $node02SignalPath -ErrorAction Stop
            } -TimeoutSeconds 240 -PollIntervalSeconds 15 `
                -Phase 'ClusterPrerequisites' `
                -Operation 'Wait for Node02.PreClusterReady.signal' `
                -HeartbeatPath $heartbeatFile `
                -LastCheckpoint 'Node01 pre-cluster foundation ready' | Out-Null
        }
        catch {
            .\Write-Info.ps1 (
                '[WAIT] Node02 pre-cluster readiness is not yet available; the resume task will retry.'
            ) -ForegroundColor Yellow
            return $false
        }

        $formationOutput = @(& (Join-Path $scriptsPath 'Create-ServerFailoverEnv.ps1') `
            -WorkingPath $WorkingPath -ConfigureFile $configFile `
            -HeartbeatPath $heartbeatFile -NoTranscript *>&1)
        $formationOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
        Assert-DeploymentChildResult -Output $formationOutput `
            -Operation 'Failover Cluster formation and repair' `
            -RequireTrueResult | Out-Null

        $environmentOutput = @(& (Join-Path $dscFolder 'Invoke-ClusterEnvironmentSteps.ps1') `
            -WorkingPath $WorkingPath -ConfigureFile $configFile `
            -HeartbeatPath $heartbeatFile -NoTranscript *>&1)
        $environmentOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
        Assert-DeploymentChildResult -Output $environmentOutput `
            -Operation 'Required Cluster test environment setup' `
            -RequireTrueResult | Out-Null

        $clusterOutput = @(& (Join-Path $scriptsPath 'Test-ClusterReadiness.ps1') `
            -ConfigureFile $configFile -Detailed *>&1)
        $clusterOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
        if ($clusterOutput.Count -eq 0 -or $clusterOutput[-1] -ne $true) {
            throw 'Live Cluster readiness is incomplete after formation.'
        }

        $clusterContent = "CLUSTER READY; SchemaVersion=1.0; TimestampUtc=$((Get-Date).ToUniversalTime().ToString('o'))"
        Write-VerifiedDeploymentSignal -Path $clusterReadySignal -Content $clusterContent
        if (-not (Test-VerifiedDeploymentSignal -Path $clusterReadySignal `
                -ExpectedContentPattern $clusterReadyPattern)) {
            throw 'Cluster readiness signal verification failed.'
        }
    }
    else {
        $node01Name = "$($config.Machines.Node01.ComputerName)"
        $node01SignalPath = 'C:\Cluster-Package\DSC\Deploy-Cluster.Completed.signal'
        try {
            Wait-DeploymentCondition -Condition {
                Invoke-Command -ComputerName $node01Name -ScriptBlock {
                    param($path)
                    $item = Get-Item -LiteralPath $path -ErrorAction SilentlyContinue
                    if ($null -eq $item -or $item.Length -le 0) { return $false }
                    $content = Get-Content -LiteralPath $path -Raw -ErrorAction SilentlyContinue
                    return $content -match '^CLUSTER READY; SchemaVersion=1\.0;'
                } -ArgumentList $node01SignalPath -ErrorAction Stop
            } -TimeoutSeconds 240 -PollIntervalSeconds 15 `
                -Phase 'ClusterPrerequisites' `
                -Operation 'Wait for Deploy-Cluster.Completed.signal' `
                -HeartbeatPath $heartbeatFile `
                -LastCheckpoint 'Node02 pre-cluster foundation ready' | Out-Null
        }
        catch {
            .\Write-Info.ps1 (
                '[WAIT] Node01 Cluster readiness is not yet available; the resume task will retry.'
            ) -ForegroundColor Yellow
            return $false
        }

        $clusterOutput = @(& (Join-Path $scriptsPath 'Test-ClusterReadiness.ps1') `
            -ConfigureFile $configFile -Detailed *>&1)
        $clusterOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
        if ($clusterOutput.Count -eq 0 -or $clusterOutput[-1] -ne $true) {
            throw 'Node02 live Cluster validation failed.'
        }
    }

    $finalContent = "NODE COMPLETE; SchemaVersion=1.0; Role=$NodeRole; TimestampUtc=$((Get-Date).ToUniversalTime().ToString('o'))"
    Write-VerifiedDeploymentSignal -Path $finalSignal -Content $finalContent
    if (-not (Test-VerifiedDeploymentSignal -Path $finalSignal `
            -ExpectedContentPattern $finalPattern)) {
        throw "$NodeRole final signal verification failed."
    }
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 3
    Remove-ResumeTask
    Remove-Item -LiteralPath $heartbeatFile -Force -ErrorAction SilentlyContinue
    .\Write-Info.ps1 "[OK] $NodeRole completed Cluster validation." -ForegroundColor Green
    return $true
}

function Connect-ConfiguredStorageTarget {
    $storage = $config.Machines.Storage
    $storageIp = "$($storage.IpConfig[0].Ip)"
    $targetName = if ($storage.iSCSITargetName) {
        "$($storage.iSCSITargetName)"
    } else {
        'ClusterTarget'
    }
    $expectedTargetAddress = (
        "iqn.1991-05.com.microsoft:$($storage.ComputerName)-$targetName-target"
    ).ToLowerInvariant()

    Set-Service msiscsi -StartupType Automatic -ErrorAction Stop
    if ((Get-Service msiscsi -ErrorAction Stop).Status -ne 'Running') {
        Start-Service msiscsi -ErrorAction Stop
    }

    Wait-DeploymentCondition -Condition {
        Test-NetConnection -ComputerName $storageIp -Port 3260 `
            -InformationLevel Quiet -WarningAction SilentlyContinue
    } -TimeoutSeconds 900 -PollIntervalSeconds 10 `
        -Phase 'Iscsi' -Operation "$NodeRole wait for Storage TCP 3260" `
        -HeartbeatPath $heartbeatFile -LastCheckpoint 'Node convergence complete' |
        Out-Null

    if ($null -eq (Get-IscsiTargetPortal -TargetPortalAddress $storageIp `
            -ErrorAction SilentlyContinue)) {
        New-IscsiTargetPortal -TargetPortalAddress $storageIp -ErrorAction Stop |
            Out-Null
    }

    Wait-DeploymentCondition -Condition {
        $null -ne (Get-IscsiTarget -ErrorAction SilentlyContinue |
            Where-Object { "$($_.NodeAddress)" -ieq $expectedTargetAddress } |
            Select-Object -First 1)
    } -TimeoutSeconds 600 -PollIntervalSeconds 10 `
        -Phase 'Iscsi' -Operation "$NodeRole discover configured Storage target" `
        -HeartbeatPath $heartbeatFile -LastCheckpoint 'Storage portal reachable' |
        Out-Null

    $target = Get-IscsiTarget -ErrorAction Stop |
        Where-Object { "$($_.NodeAddress)" -ieq $expectedTargetAddress } |
        Select-Object -First 1
    if (-not $target.IsConnected) {
        Connect-IscsiTarget -NodeAddress $expectedTargetAddress `
            -TargetPortalAddress $storageIp -IsPersistent $true -ErrorAction Stop |
            Out-Null
    }
    $session = Get-IscsiSession -ErrorAction SilentlyContinue |
        Where-Object { "$($_.TargetNodeAddress)" -ieq $expectedTargetAddress } |
        Select-Object -First 1
    $persistentOutput = @(& iscsicli.exe ListPersistentTargets 2>&1)
    if ($null -ne $session -and
        -not ($persistentOutput -match [regex]::Escape($expectedTargetAddress))) {
        Register-IscsiSession -SessionIdentifier $session.SessionIdentifier `
            -ErrorAction Stop | Out-Null
    }

    Wait-DeploymentCondition -Condition {
        Update-HostStorageCache -ErrorAction SilentlyContinue
        $sessionReady = $null -ne (Get-IscsiSession -ErrorAction SilentlyContinue |
            Where-Object { "$($_.TargetNodeAddress)" -ieq $expectedTargetAddress } |
            Select-Object -First 1)
        $diskCount = @(Get-Disk -ErrorAction SilentlyContinue |
            Where-Object { $_.BusType -eq 'iSCSI' }).Count
        return ($sessionReady -and $diskCount -eq 4)
    } -TimeoutSeconds 600 -PollIntervalSeconds 10 `
        -Phase 'Iscsi' -Operation "$NodeRole wait for four shared iSCSI disks" `
        -HeartbeatPath $heartbeatFile -LastCheckpoint 'Configured target connected' |
        Out-Null
}

.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan
.\Write-Info.ps1 "  $NodeRole -- Deterministic Pre-Cluster Deployment" `
    -ForegroundColor Cyan
.\Write-Info.ps1 '===========================================================' -ForegroundColor Cyan

. "$dscFolder\Deploy-CommonHelpers.ps1"

try {
    $config = Get-Content -LiteralPath $configFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    & (Join-Path $scriptsPath 'Validate-ConfigFile.ps1') -ConfigPath $configFile
    $node = $config.Machines.$NodeRole
    if ($null -eq $node) { throw "Config.json is missing Machines.$NodeRole." }
    $expectedName = "$($node.ComputerName)"

    if ((Test-Path -LiteralPath $toolsSignal -PathType Leaf) -and
        -not (Test-RequiredNodeToolState)) {
        Remove-Item -LiteralPath $toolsSignal -Force
    }
    if ((Test-Path -LiteralPath $toolsPreparedSignal -PathType Leaf) -and
        -not (Test-VerifiedDeploymentSignal -Path $toolsPreparedSignal `
            -ExpectedContentPattern '^Prepared ')) {
        Remove-Item -LiteralPath $toolsPreparedSignal -Force
    }

    $renameStateNames = Get-DeploymentRoleStateNames `
        -Role $roleStateName -RebootScope 'Rename'
    if ([int](Get-DeploymentRegistryValue -Name $renameStateNames.RebootPendingName `
            -DefaultValue 0) -eq 1) {
        Confirm-DeploymentReboot -Role $roleStateName -RebootScope 'Rename' | Out-Null
        if ($env:COMPUTERNAME -ne $expectedName) {
            throw "$NodeRole hostname repair reboot did not apply '$expectedName'."
        }
    }

    if ($NodeRole -eq 'Node01') {
        $kerberosRebootNames = Get-DeploymentRoleStateNames `
            -Role $roleStateName -RebootScope 'KerberosAlignment'
        if ([int](Get-DeploymentRegistryValue `
                -Name $kerberosRebootNames.RebootPendingName `
                -DefaultValue 0) -eq 1) {
            Confirm-DeploymentReboot -Role $roleStateName `
                -RebootScope 'KerberosAlignment' | Out-Null
            if (-not (Test-RequiredNodeDomainState)) {
                throw 'Node01 Kerberos alignment reboot completed without a healthy secure channel.'
            }
            Set-DeploymentRegistryValue -Name $kerberosAlignmentMarkerName `
                -Value $kerberosAlignmentVersion -Type DWord
            Set-DeploymentRegistryValue -Name 'ComputerPasswordSet' `
                -Value 3 -Type DWord
            Set-DeploymentRegistryValue `
                -Name $kerberosRebootNames.RebootCountName -Value 0 -Type DWord
            .\Write-Info.ps1 (
                '[OK] Node01 Kerberos machine-password alignment reboot was proven.'
            ) -ForegroundColor Green
        }
    }

    $currentPhase = Get-DeploymentPhase -Name $phaseRegistryName
    if ($currentPhase -ge 3) {
        $finalValid = Test-VerifiedDeploymentSignal -Path $finalSignal `
            -ExpectedContentPattern $finalPattern
        $clusterOutput = @(& (Join-Path $scriptsPath 'Test-ClusterReadiness.ps1') `
            -ConfigureFile $configFile *>&1)
        $clusterLive = $clusterOutput.Count -gt 0 -and $clusterOutput[-1] -eq $true
        if ($finalValid -and $clusterLive -and
            (Test-RequiredNodeKerberosAlignment)) {
            Remove-ResumeTask
            return
        }
        Remove-Item -LiteralPath $finalSignal -Force -ErrorAction SilentlyContinue
        if ($NodeRole -eq 'Node01') {
            Remove-Item -LiteralPath $clusterReadySignal -Force `
                -ErrorAction SilentlyContinue
        }
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        $currentPhase = 2
    }
    elseif (Test-Path -LiteralPath $finalSignal -PathType Leaf) {
        Remove-Item -LiteralPath $finalSignal -Force
        if ($NodeRole -eq 'Node01') {
            Remove-Item -LiteralPath $clusterReadySignal -Force `
                -ErrorAction SilentlyContinue
        }
    }
    elseif ($NodeRole -eq 'Node01' -and $currentPhase -lt 3 -and
        (Test-Path -LiteralPath $clusterReadySignal -PathType Leaf)) {
        Remove-Item -LiteralPath $clusterReadySignal -Force
    }

    $staleReboot = Get-ScheduledTask -TaskName 'PostDeployReboot' `
        -ErrorAction SilentlyContinue
    if ($null -ne $staleReboot) {
        Unregister-ScheduledTask -TaskName 'PostDeployReboot' -Confirm:$false
    }

    if ((Test-VerifiedDeploymentSignal -Path $preReadySignal `
            -ExpectedContentPattern $preReadyPattern) -and
        (Test-RequiredNodeFoundationState) -and
        (Test-RequiredNodeKerberosAlignment)) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        $currentPhase = 2
        $null = Complete-ClusterPhase
        return
    }
    if (Test-Path -LiteralPath $preReadySignal -PathType Leaf) {
        Remove-Item -LiteralPath $preReadySignal -Force
    }

    if ($currentPhase -ge 2 -and -not (Test-VerifiedDeploymentSignal -Path $preReadySignal -ExpectedContentPattern $preReadyPattern)) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        $currentPhase = 1
    }

    $oldDeployStep = [int](Get-DeploymentRegistryValue -Name 'DeployStep' -DefaultValue 0)
    if ($currentPhase -eq 0 -and $oldDeployStep -ge 1 -and
        (Test-InstalledNodeFeatures) -and
        (Test-RequiredNodeDomainState) -and
        -not (Test-PendingSystemReboot)) {
        Set-DeploymentRegistryValue -Name 'ClusterNodeFeatureBundleAttempted' `
            -Value 1 -Type DWord
        Set-DeploymentRegistryValue -Name "${roleStateName}RebootPending" `
            -Value 0 -Type DWord
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        $currentPhase = 1
    }

    $stateNames = Get-DeploymentRoleStateNames -Role $roleStateName
    if ($currentPhase -ge 1 -and
        [int](Get-DeploymentRegistryValue -Name $stateNames.RebootPendingName `
            -DefaultValue 0) -eq 1) {
        Confirm-DeploymentReboot -Role $roleStateName | Out-Null
        .\Write-Info.ps1 "[OK] $NodeRole combined feature/domain-join reboot was proven." `
            -ForegroundColor Green
    }

    if ($currentPhase -eq 0) {
        if ($env:COMPUTERNAME -ne $expectedName) {
            Rename-Computer -NewName $expectedName -Force -ErrorAction Stop
            Set-DeploymentRebootPending -Role $roleStateName `
                -RebootScope 'Rename' -MaximumRebootCount 1 | Out-Null
            .\Write-Info.ps1 (
                "Scheduling exceptional $NodeRole hostname repair reboot before domain join."
            ) -ForegroundColor Yellow
            Register-NodePlannedReboot -AsSystem
            return
        }

        $toolsPreparationJob = Start-NodeToolsPreparation
        try {
            . (Join-Path $dscFolder 'Node-FeatureConfiguration.ps1')
            NodeFeatureConfiguration -OutputPath $featureMofFolder
            Invoke-VerifiedDscConfiguration -Path $featureMofFolder `
                -OperationName "$NodeRole feature configuration" `
                -PhaseName 'NodeFeatures' -HeartbeatPath $heartbeatFile `
                -Postcondition { Test-RequiredNodeFeatureState } | Out-Null

            $joinOutput = @(& (Join-Path $scriptsPath 'domainjoin.ps1') `
                -protocolConfigFile $configFile -NoTranscript *>&1)
            $joinOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
            Assert-DeploymentChildResult -Output $joinOutput `
                -Operation "$NodeRole domain join" -RequireTrueResult | Out-Null

            if (-not (Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters')) {
                New-Item -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters' `
                    -Force | Out-Null
            }
            Set-ItemProperty `
                -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters' `
                -Name 'DisablePasswordChange' -Value 1 -Type DWord -Force

            Complete-NodeToolsPreparation -Job $toolsPreparationJob
            $toolsPreparationJob = $null
            if (-not (Test-Path -LiteralPath $toolsPreparedSignal) -and
                -not (Test-Path -LiteralPath $toolsSignal)) {
                throw "$NodeRole package preparation did not produce a verified signal."
            }
        }
        finally {
            if ($null -ne $toolsPreparationJob) {
                if ($toolsPreparationJob.State -in @('NotStarted', 'Running', 'Blocked')) {
                    Stop-Job -Job $toolsPreparationJob -ErrorAction SilentlyContinue
                }
                Remove-Job -Job $toolsPreparationJob -Force -ErrorAction SilentlyContinue
            }
        }

        Set-DeploymentRebootPending -Role "Cluster$NodeRole" -MaximumRebootCount 1 |
            Out-Null
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        .\Write-Info.ps1 "Scheduling $NodeRole combined feature/domain-join reboot." `
            -ForegroundColor Yellow
        Register-NodePlannedReboot
        return
    }

    if ($env:COMPUTERNAME -ne $expectedName) {
        throw "$NodeRole reboot completed without applying hostname '$expectedName'."
    }
    if (-not (Test-RequiredNodeDomainState)) {
        Test-ComputerSecureChannel -ErrorAction Stop | Out-Null
        throw "$NodeRole domain membership or secure channel is incomplete."
    }
    if (Test-PendingSystemReboot) {
        throw "A reboot remains pending before $NodeRole convergence."
    }

    if (-not (Test-Path -LiteralPath $toolsPreparedSignal) -and
        -not (Test-Path -LiteralPath $toolsSignal)) {
        $preparedOutput = @(& $toolsInstaller -Role $NodeRole -Operation Prepare `
            -PreparedSignalFile $toolsPreparedSignal -NoTranscript *>&1)
        Assert-DeploymentChildResult -Output $preparedOutput `
            -Operation "$NodeRole package preparation" -RequireTrueResult | Out-Null
    }

    . (Join-Path $dscFolder 'Node-Configuration.ps1')
    NodeConfiguration -ConfigFilePath $configFile -NodeRole $NodeRole `
        -OutputPath $convergenceMofFolder
    Invoke-VerifiedDscConfiguration -Path $convergenceMofFolder `
        -OperationName "$NodeRole convergence configuration" `
        -PhaseName 'NodeConvergence' -HeartbeatPath $heartbeatFile `
        -Postcondition { Test-RequiredNodeDscState } | Out-Null

    if (-not (Test-RequiredNodeToolState)) {
        $installOutput = @(& $toolsInstaller -Role $NodeRole -Operation Install `
            -PreparedSignalFile $toolsPreparedSignal -NoTranscript *>&1)
        $installOutput | ForEach-Object { .\Write-Info.ps1 "$_" }
        Assert-DeploymentChildResult -Output $installOutput `
            -Operation "$NodeRole tool installation" -RequireTrueResult | Out-Null
    }

    Connect-ConfiguredStorageTarget
    if (-not (Test-RequiredNodeFoundationState)) {
        throw "$NodeRole foundation readiness is incomplete after convergence."
    }
    if (Test-PendingSystemReboot) {
        throw "$NodeRole convergence requested an unexpected second normal reboot."
    }

    if (-not (Test-RequiredNodeKerberosAlignment)) {
        Start-NodeKerberosAlignment
        return
    }

    $signalContent = "NODE PRECLUSTER READY; SchemaVersion=1.0; Role=$NodeRole; TimestampUtc=$((Get-Date).ToUniversalTime().ToString('o'))"
    Write-VerifiedDeploymentSignal -Path $preReadySignal -Content $signalContent
    if (-not (Test-VerifiedDeploymentSignal -Path $preReadySignal `
            -ExpectedContentPattern $preReadyPattern)) {
        throw "$NodeRole pre-cluster readiness signal verification failed."
    }
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
    Remove-Item -LiteralPath $heartbeatFile -Force -ErrorAction SilentlyContinue
    .\Write-Info.ps1 (
        "[WAIT] $NodeRole pre-cluster foundation is ready; waiting for Cluster formation."
    ) -ForegroundColor Yellow
    $null = Complete-ClusterPhase
}
catch {
    $failureMessage = "$NodeRole deployment failed: $($_.Exception.Message)"
    .\Write-Error.ps1 $failureMessage
    Stop-DeploymentForTerminalFailure -Message $failureMessage `
        -Phase $NodeRole -Operation 'Deterministic Cluster node foundation' `
        -HeartbeatPath $heartbeatFile
}
finally {
    Stop-NodeDeploymentTranscript
}
