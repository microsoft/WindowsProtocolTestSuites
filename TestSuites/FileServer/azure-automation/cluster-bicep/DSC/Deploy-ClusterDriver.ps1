# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
$dscFolder = $PSScriptRoot
$scriptsPath = Join-Path $dscFolder 'Scripts'
$mofFolder = Join-Path $dscFolder 'MOF\Driver'
$configFile = Join-Path $WorkingPath 'Config.json'
$logFile = Join-Path $dscFolder 'Deploy-Driver.log'
$heartbeatFile = Join-Path $dscFolder 'Deploy-Driver.heartbeat.json'
$phaseRegistryName = 'DriverDeployPhase'
$signalFile = Join-Path $dscFolder 'Deploy-Driver.Completed.signal'
$signalPattern = '^DRIVER COMPLETE; SchemaVersion=1\.0;'
$driverDscSignal = Join-Path $dscFolder 'Driver-DSC.Completed.signal'
$toolsSignal = Join-Path $scriptsPath 'InstallMSIAndTools.Completed.signal'
$toolsPreparedSignal = Join-Path $scriptsPath 'InstallMSIAndTools.Prepared.signal'
$toolsInstaller = Join-Path $scriptsPath 'InstallMSIAndTools.ps1'
$node01FinalSignalName = 'Deploy-Node01.Completed.signal'
$node02FinalSignalName = 'Deploy-Node02.Completed.signal'
$localForceLevel2Signal = Join-Path $dscFolder 'ForceLevel2.Local.Completed.signal'
$clusteredForceLevel2Signal = Join-Path $dscFolder 'ForceLevel2.Clustered.Completed.signal'
$toolsJobTimeoutSeconds = 3600

$env:Path += ";$scriptsPath"
Push-Location $scriptsPath
Start-Transcript -Path $logFile -Append -Force | Out-Null
$transcriptStopped = $false

. "$dscFolder\Deploy-CommonHelpers.ps1"

function Stop-ClusterDriverTranscript {
    if (-not $transcriptStopped) {
        $script:transcriptStopped = $true
        Stop-Transcript | Out-Null
        Pop-Location
    }
}

function Test-DriverDomainState {
    $system = Get-CimInstance Win32_ComputerSystem -ErrorAction SilentlyContinue
    if ($null -eq $system -or -not $system.PartOfDomain -or
        "$($system.Domain)" -ine "$($config.Core.DomainName)") {
        return $false
    }
    try {
        return [bool](Test-ComputerSecureChannel -ErrorAction Stop)
    }
    catch {
        return $false
    }
}

function Test-DriverDscState {
    try {
        return (Test-VerifiedDeploymentSignal -Path $driverDscSignal `
            -ExpectedContentPattern '^DRIVER DSC READY;') -and
            [bool](Test-DscConfiguration -Path $mofFolder -ErrorAction Stop)
    }
    catch {
        return $false
    }
}

function Test-DriverToolState {
    return (Test-VerifiedDeploymentSignal -Path $toolsSignal `
        -ExpectedContentPattern '^Completed ') -and
        (Test-Path "$env:ProgramFiles\PowerShell\7\pwsh.exe" -PathType Leaf) -and
        (Test-Path "$env:SystemDrive\FileServer-TestSuite-ServerEP\Bin\.version" -PathType Leaf)
}

function Start-DriverPreparation {
    if ((Test-Path $toolsPreparedSignal) -or (Test-Path $toolsSignal)) {
        return $null
    }
    $jobLog = Join-Path $scriptsPath 'InstallMSIAndTools.Driver.prepare.job.log'
    return Start-Job -ScriptBlock {
        param($installer, $scriptsDirectory, $preparedSignal, $log)
        Set-Location $scriptsDirectory
        $output = @(& $installer -Role DriverComputer -Operation Prepare `
            -PreparedSignalFile $preparedSignal -NoTranscript *>&1)
        $output | Out-File -LiteralPath $log -Force
        if ($output.Count -eq 0 -or $output[-1] -ne $true) {
            throw 'Required Driver package preparation failed.'
        }
    } -ArgumentList $toolsInstaller, $scriptsPath, $toolsPreparedSignal, $jobLog
}

function Complete-DriverPreparation {
    param([System.Management.Automation.Job]$Job)
    if ($null -eq $Job) { return }
    try {
        Wait-DeploymentJob -Job $Job -TimeoutSeconds $toolsJobTimeoutSeconds `
            -Phase ToolsPrepare -Operation 'Cluster Driver package preparation' `
            -HeartbeatPath $heartbeatFile -LastCheckpoint 'Driver baseline/domain phase' |
            Out-Null
    }
    finally {
        Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
    }
}

function Register-DriverReboot {
    param([switch]$AsSystem)
    $parameters = @{
        DeployScript = (Join-Path $dscFolder 'Deploy-Driver.ps1')
        WorkingPath = $WorkingPath
        DscFolder = $dscFolder
    }
    if (-not $AsSystem) {
        $domainNetBios = if ($config.Domain.NetBiosName) {
            "$($config.Domain.NetBiosName)"
        } else {
            "$($config.Core.DomainName)".Split('.')[0].ToUpperInvariant()
        }
        $parameters['RunAsUser'] = "$domainNetBios\$($config.Core.Username)"
        $parameters['RunAsPassword'] = $config.Core.Password
    }
    Register-DeferredRebootAndResume @parameters
}

function Remove-StaleTestTask {
    $task = Get-ScheduledTask -TaskName 'RunFileServerTests' `
        -ErrorAction SilentlyContinue
    if ($null -ne $task) {
        Unregister-ScheduledTask -TaskName 'RunFileServerTests' -Confirm:$false
    }
}

function Register-VerifiedTestTask {
    $pwsh = (Get-Command pwsh.exe -ErrorAction SilentlyContinue).Source
    if (-not $pwsh) { $pwsh = "$env:ProgramFiles\PowerShell\7\pwsh.exe" }
    if (-not (Test-Path -LiteralPath $pwsh -PathType Leaf)) {
        throw "pwsh.exe was not found at '$pwsh'."
    }
    $testRunScript = Join-Path $scriptsPath 'Invoke-TestRun.ps1'
    if (-not (Test-Path -LiteralPath $testRunScript -PathType Leaf)) {
        throw "Invoke-TestRun.ps1 was not found at '$testRunScript'."
    }

    $domainNetBios = if ($config.Domain.NetBiosName) {
        "$($config.Domain.NetBiosName)"
    } else {
        "$($config.Core.DomainName)".Split('.')[0].ToUpperInvariant()
    }
    $taskUser = "$domainNetBios\$($config.Core.Username)"
    $action = New-ScheduledTaskAction -Execute $pwsh `
        -Argument "-NoProfile -File `"$testRunScript`" -WorkingPath `"$WorkingPath`""
    $trigger = New-ScheduledTaskTrigger -Once -At (Get-Date).AddSeconds(30)
    $settings = New-ScheduledTaskSettingsSet -StartWhenAvailable `
        -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries
    Register-ScheduledTask -TaskName 'RunFileServerTests' -Action $action `
        -Trigger $trigger -Settings $settings -User $taskUser `
        -Password $config.Core.Password -RunLevel Highest -Force -ErrorAction Stop |
        Out-Null
}

try {
    $config = Get-Content -LiteralPath $configFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    if ("$($config.Core.Scenario)" -ne 'Cluster') {
        throw 'Deploy-ClusterDriver.ps1 is only valid for the Cluster scenario.'
    }
    $testAutoRun = if ($null -eq $config.TestExecution -or
        $null -eq $config.TestExecution.AutoRun) {
        $true
    } else {
        [Convert]::ToBoolean("$($config.TestExecution.AutoRun)")
    }
    & (Join-Path $scriptsPath 'Validate-ConfigFile.ps1') -ConfigPath $configFile
    $expectedName = "$($config.Machines.DriverComputer.ComputerName)"

    if ((Test-Path $toolsSignal) -and -not (Test-DriverToolState)) {
        Remove-Item $toolsSignal -Force
    }
    if ((Test-Path $toolsPreparedSignal) -and
        -not (Test-VerifiedDeploymentSignal -Path $toolsPreparedSignal `
            -ExpectedContentPattern '^Prepared ')) {
        Remove-Item $toolsPreparedSignal -Force
    }

    $currentPhase = Get-DeploymentPhase -Name $phaseRegistryName
    if ($currentPhase -ge 3) {
        $readyOutput = @(& (Join-Path $scriptsPath 'Test-ClusterDriverReadiness.ps1') `
            -WorkingPath $WorkingPath -ConfigureFile $configFile *>&1)
        $ready = $readyOutput.Count -gt 0 -and $readyOutput[-1] -eq $true
        if ((Test-VerifiedDeploymentSignal -Path $signalFile `
                -ExpectedContentPattern $signalPattern) -and $ready) {
            Remove-ResumeTask
            return
        }
        Remove-Item $signalFile -Force -ErrorAction SilentlyContinue
        Remove-StaleTestTask
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        $currentPhase = 2
    }

    $renameNames = Get-DeploymentRoleStateNames -Role Driver -RebootScope Rename
    if ([int](Get-DeploymentRegistryValue -Name $renameNames.RebootPendingName `
            -DefaultValue 0) -eq 1) {
        Confirm-DeploymentReboot -Role Driver -RebootScope Rename | Out-Null
    }
    $convergenceRebootNames = Get-DeploymentRoleStateNames `
        -Role Driver -RebootScope Convergence
    if ([int](Get-DeploymentRegistryValue `
            -Name $convergenceRebootNames.RebootPendingName -DefaultValue 0) -eq 1) {
        Confirm-DeploymentReboot -Role Driver -RebootScope Convergence | Out-Null
    }

    $oldStep = [int](Get-DeploymentRegistryValue -Name DeployStep -DefaultValue 0)
    if ($currentPhase -eq 0 -and $oldStep -ge 1 -and
        (Test-DriverDomainState) -and -not (Test-PendingSystemReboot)) {
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        Set-DeploymentRegistryValue -Name DriverRebootPending -Value 0 -Type DWord
        $currentPhase = 1
    }

    $stateNames = Get-DeploymentRoleStateNames -Role Driver
    if ($currentPhase -ge 1 -and
        [int](Get-DeploymentRegistryValue -Name $stateNames.RebootPendingName `
            -DefaultValue 0) -eq 1) {
        Confirm-DeploymentReboot -Role Driver | Out-Null
    }

    if ($currentPhase -eq 0 -and $env:COMPUTERNAME -ne $expectedName) {
        Rename-Computer -NewName $expectedName -Force -ErrorAction Stop
        Set-DeploymentRebootPending -Role Driver -RebootScope Rename `
            -MaximumRebootCount 1 | Out-Null
        Register-DriverReboot -AsSystem
        return
    }

    if ($currentPhase -eq 0) {
        $preparationJob = Start-DriverPreparation
        try {
            . (Join-Path $dscFolder 'Driver-Configuration.ps1')
            DriverConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
            Invoke-VerifiedDscConfiguration -Path $mofFolder `
                -OperationName 'Cluster Driver baseline configuration' `
                -PhaseName DriverBaseline -HeartbeatPath $heartbeatFile `
                -Postcondition {
                    [bool](Test-DscConfiguration -Path $mofFolder -ErrorAction Stop)
                } | Out-Null
            Write-VerifiedDeploymentSignal -Path $driverDscSignal `
                -Content "DRIVER DSC READY; TimestampUtc=$((Get-Date).ToUniversalTime().ToString('o'))"

            Complete-DriverPreparation -Job $preparationJob
            $preparationJob = $null
            if (-not (Test-Path $toolsPreparedSignal) -and
                -not (Test-Path $toolsSignal)) {
                throw 'Cluster Driver packages were not prepared before domain join.'
            }
            if (-not (Test-Path $toolsSignal)) {
                $installOutput = @(& $toolsInstaller -Role DriverComputer `
                    -Operation Install -PreparedSignalFile $toolsPreparedSignal `
                    -AllowRebootRequired -NoTranscript *>&1)
                Assert-DeploymentChildResult -Output $installOutput `
                    -Operation 'Pre-reboot Cluster Driver tool installation' `
                    -RequireTrueResult | Out-Null
            }
            if (-not (Test-DriverToolState)) {
                throw 'Cluster Driver tools are incomplete before the planned domain-join reboot.'
            }

            $joinOutput = @(& (Join-Path $scriptsPath 'domainjoin.ps1') `
                -protocolConfigFile $configFile -NoTranscript *>&1)
            Assert-DeploymentChildResult -Output $joinOutput `
                -Operation 'Cluster Driver domain join' -RequireTrueResult | Out-Null
            if (-not (Test-Path `
                    'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters')) {
                New-Item `
                    -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters' `
                    -Force | Out-Null
            }
            Set-ItemProperty `
                -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters' `
                -Name DisablePasswordChange -Value 1 -Type DWord -Force

        }
        finally {
            if ($null -ne $preparationJob) {
                Stop-Job -Job $preparationJob -ErrorAction SilentlyContinue
                Remove-Job -Job $preparationJob -Force -ErrorAction SilentlyContinue
            }
        }

        Set-DeploymentRebootPending -Role 'Driver' -MaximumRebootCount 1 |
            Out-Null
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 1
        Register-DriverReboot
        return
    }

    if ($env:COMPUTERNAME -ne $expectedName) {
        throw "Driver hostname '$env:COMPUTERNAME' does not match '$expectedName'."
    }
    if (-not (Test-DriverDomainState)) {
        Test-ComputerSecureChannel -ErrorAction Stop | Out-Null
        throw 'Cluster Driver domain membership or secure channel is incomplete.'
    }
    if (Test-PendingSystemReboot) {
        Set-DeploymentRebootPending -Role Driver -RebootScope Convergence `
            -MaximumRebootCount 1 | Out-Null
        .\Write-Info.ps1 (
            'A post-domain-join reboot remains pending. Scheduling one bounded convergence reboot.'
        ) -ForegroundColor Yellow
        Register-DriverReboot
        return
    }

    if ($currentPhase -eq 1) {
        if (-not (Test-Path $toolsPreparedSignal) -and -not (Test-Path $toolsSignal)) {
            $prepareOutput = @(& $toolsInstaller -Role DriverComputer `
                -Operation Prepare -PreparedSignalFile $toolsPreparedSignal `
                -NoTranscript *>&1)
            Assert-DeploymentChildResult -Output $prepareOutput `
                -Operation 'Cluster Driver package preparation' `
                -RequireTrueResult | Out-Null
        }

        if (-not (Test-DriverDscState)) {
            . (Join-Path $dscFolder 'Driver-Configuration.ps1')
            DriverConfiguration -ConfigFilePath $configFile -OutputPath $mofFolder
            Invoke-VerifiedDscConfiguration -Path $mofFolder `
                -OperationName 'Cluster Driver convergence repair' `
                -PhaseName DriverConvergence -HeartbeatPath $heartbeatFile `
                -Postcondition {
                    [bool](Test-DscConfiguration -Path $mofFolder -ErrorAction Stop)
                } | Out-Null
            Write-VerifiedDeploymentSignal -Path $driverDscSignal `
                -Content "DRIVER DSC READY; TimestampUtc=$((Get-Date).ToUniversalTime().ToString('o'))"
        }

        $imperativeOutput = @(& (Join-Path $dscFolder 'Invoke-DriverImperativeSteps.ps1') `
            -Step 2 -WorkingPath $WorkingPath -ConfigureFile $configFile `
            -SkipForceLevel2 -HeartbeatPath $heartbeatFile -NoTranscript *>&1)
        Assert-DeploymentChildResult -Output $imperativeOutput `
            -Operation 'Cluster Driver tools, PTF, and RSA configuration' `
            -RequireTrueResult | Out-Null
        if (-not (Test-DriverToolState)) {
            throw 'Cluster Driver tool postconditions are incomplete.'
        }
        Set-DeploymentPhase -Name $phaseRegistryName -Phase 2
        $currentPhase = 2
    }

    Remove-StaleTestTask
    Wait-DeploymentCondition -Condition {
        & (Join-Path $scriptsPath 'Test-ClusterDriverReadiness.ps1') `
            -WorkingPath $WorkingPath -ConfigureFile $configFile `
            -SkipForceLevel2Check -SkipTestTaskCheck
    } -TimeoutSeconds 7200 -PollIntervalSeconds 30 `
        -Phase ClusterDriverGate -Operation 'Wait for both nodes and Cluster endpoints' `
        -HeartbeatPath $heartbeatFile -LastCheckpoint 'Driver tools/PTF/RSA ready' |
        Out-Null

    $forceOutput = @(& (Join-Path $scriptsPath 'Configure-ForceLevel2.ps1') `
        -WorkingPath $WorkingPath -ConfigureFile $configFile `
        -HeartbeatPath $heartbeatFile -NoTranscript *>&1)
    Assert-DeploymentChildResult -Output $forceOutput `
        -Operation 'Local and clustered ForceLevel2 configuration' `
        -RequireTrueResult | Out-Null

    # Both required ForceLevel2 signals must exist before scheduling tests.
    foreach ($forceSignal in @(
        $localForceLevel2Signal,
        $clusteredForceLevel2Signal
    )) {
        if (-not (Test-VerifiedDeploymentSignal -Path $forceSignal `
                -ExpectedContentPattern '^FORCELEVEL2 READY; SchemaVersion=1\.0;')) {
            throw "ForceLevel2 signal '$forceSignal' is invalid."
        }
    }

    if ($testAutoRun) {
        Register-VerifiedTestTask
    } else {
        Remove-StaleTestTask
        .\Write-Info.ps1 (
            '[SKIP] Automatic FileServer test execution is disabled in Config.json.'
        ) -ForegroundColor Yellow
    }
    $readyOutput = @(& (Join-Path $scriptsPath 'Test-ClusterDriverReadiness.ps1') `
        -WorkingPath $WorkingPath -ConfigureFile $configFile `
        -SkipTestTaskCheck:(-not $testAutoRun) -Detailed *>&1)
    if ($readyOutput.Count -eq 0 -or $readyOutput[-1] -ne $true) {
        Remove-StaleTestTask
        throw 'Cluster Driver readiness regressed after test task registration.'
    }

    Write-VerifiedDeploymentSignal -Path $signalFile `
        -Content "DRIVER COMPLETE; SchemaVersion=1.0; TimestampUtc=$((Get-Date).ToUniversalTime().ToString('o'))"
    if (-not (Test-VerifiedDeploymentSignal -Path $signalFile `
            -ExpectedContentPattern $signalPattern)) {
        throw 'Driver completion signal verification failed.'
    }
    Set-DeploymentPhase -Name $phaseRegistryName -Phase 3
    Remove-ResumeTask
    Remove-Item $heartbeatFile -Force -ErrorAction SilentlyContinue
}
catch {
    Remove-StaleTestTask
    $failureMessage = "Cluster Driver deployment failed: $($_.Exception.Message)"
    .\Write-Error.ps1 $failureMessage
    Stop-DeploymentForTerminalFailure -Message $failureMessage `
        -Phase Driver -Operation 'Deterministic Cluster Driver deployment' `
        -HeartbeatPath $heartbeatFile
}
finally {
    Stop-ClusterDriverTranscript
}
