# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Shared helper functions for all Deploy-*.ps1 orchestrators.
    Eliminates copy-pasted reboot scheduling and cleanup blocks.
#>

function Register-DeferredRebootAndResume {
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingUsernameAndPasswordParams', '',
        Justification = 'Credentials are passed from Azure deployment config to Register-ScheduledTask which requires separate user/password strings.')]
    <#
    .SYNOPSIS
        Registers the TKFRSAR startup task to re-run the deploy script after reboot,
        then schedules a deferred reboot via PostDeployReboot.
    .PARAMETER DeployScript
        Full path to the Deploy-*.ps1 script to re-run.
    .PARAMETER WorkingPath
        WorkingPath argument passed to the deploy script.
    .PARAMETER DscFolder
        Working directory for the scheduled task action.
    .PARAMETER TaskName
        Name of the resume task (default: TKFRSAR).
    .PARAMETER RunAsUser
        Optional domain user (e.g. CONTOSO\testadmin) to run the resume task as.
        When omitted the task runs as SYSTEM (appropriate for pre-domain-join steps).
    .PARAMETER RunAsPassword
        Plain-text password for RunAsUser.  Required when RunAsUser is specified.
    .PARAMETER RebootDelaySec
        Seconds to delay before the reboot fires (default: 90).
    #>
    param(
        [Parameter(Mandatory)]
        [string]$DeployScript,

        [Parameter(Mandatory)]
        [string]$WorkingPath,

        [Parameter(Mandatory)]
        [string]$DscFolder,

        [string]$TaskName = 'TKFRSAR',

        [string]$RunAsUser,

        [string]$RunAsPassword,

        [int]$RebootDelaySec = 90,

        # Hold the resume task this long after startup so the network logon / domain
        # session is fully established before the deploy script runs. Mitigates BITS
        # 0x800704DD (ERROR_NOT_LOGGED_ON) and similar not-yet-logged-on races on a
        # fast post-reboot resume (complements the HttpClient download fallback).
        [int]$ResumeStartupDelaySec = 30
    )

    # 1. Unregister existing resume task if present
    $existing = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
    if ($null -ne $existing) {
        Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false
    }

    # 2. Register resume task (AtStartup + ~30s delay, plus a 5-min repetition fallback)
    $action = New-ScheduledTaskAction -Execute 'powershell.exe' `
        -Argument "-ExecutionPolicy Bypass -NonInteractive -File `"$DeployScript`" -WorkingPath `"$WorkingPath`"" `
        -WorkingDirectory $DscFolder
    $startupTrigger = New-ScheduledTaskTrigger -AtStartup
    if ($ResumeStartupDelaySec -gt 0) {
        # ISO-8601 duration; PT30S = 30 seconds after startup.
        $startupTrigger.Delay = "PT${ResumeStartupDelaySec}S"
    }
    $trigger = @(
        $startupTrigger,
        (New-ScheduledTaskTrigger -Once -At (Get-Date).AddMinutes(5) -RepetitionInterval (New-TimeSpan -Minutes 5))
    )
    $settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable
    if ($RunAsUser -and $RunAsPassword) {
        # Run as domain user (needed for cross-node operations like New-Cluster)
        Register-ScheduledTask -TaskName $TaskName -Action $action -Trigger $trigger `
            -Settings $settings -User $RunAsUser -Password $RunAsPassword -RunLevel Highest -Force | Out-Null
        .\Write-Info.ps1 "[OK] Startup task '$TaskName' registered (as $RunAsUser, ~${ResumeStartupDelaySec}s startup delay)." -ForegroundColor Green
    } else {
        $principal = New-ScheduledTaskPrincipal -UserId 'SYSTEM' -RunLevel Highest -LogonType ServiceAccount
        Register-ScheduledTask -TaskName $TaskName -Action $action -Trigger $trigger `
            -Principal $principal -Settings $settings -Force | Out-Null
        .\Write-Info.ps1 "[OK] Startup task '$TaskName' registered (as SYSTEM, ~${ResumeStartupDelaySec}s startup delay)." -ForegroundColor Green
    }

    # 3. Schedule deferred reboot
    $rebootTaskName = 'PostDeployReboot'
    $rebootAction   = New-ScheduledTaskAction -Execute 'shutdown.exe' -Argument '/r /t 0 /f'
    $rebootTrigger  = New-ScheduledTaskTrigger -Once -At (Get-Date).AddSeconds($RebootDelaySec)
    $rebootSettings = New-ScheduledTaskSettingsSet -StartWhenAvailable
    Register-ScheduledTask -TaskName $rebootTaskName -Action $rebootAction `
        -Trigger $rebootTrigger -Settings $rebootSettings `
        -User 'SYSTEM' -RunLevel Highest -Force | Out-Null
    .\Write-Info.ps1 "[OK] Reboot scheduled in ~${RebootDelaySec}s (task '$rebootTaskName'). Extension will exit cleanly." -ForegroundColor Green
}

function Remove-ResumeTask {
    <#
    .SYNOPSIS
        Unregisters the TKFRSAR scheduled task if it exists.
        Call this after successful deployment completion or on early-exit when signal file exists.
    .PARAMETER TaskName
        Name of the resume task (default: TKFRSAR).
    #>
    param(
        [string]$TaskName = 'TKFRSAR'
    )

    $task = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
    if ($null -ne $task) {
        Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false
        .\Write-Info.ps1 "[OK] Unregistered '$TaskName' scheduled task." -ForegroundColor Green
    }
}

function Test-PendingSystemReboot {
    $pendingRenames = Get-ItemProperty `
        -Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager' `
        -Name 'PendingFileRenameOperations' -ErrorAction SilentlyContinue
    $pendingRenameOperations = if ($null -ne $pendingRenames) {
        @($pendingRenames.PendingFileRenameOperations |
            Where-Object { -not [string]::IsNullOrWhiteSpace("$_") })
    } else {
        @()
    }

    return (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\RebootPending') -or
           (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update\RebootRequired') -or
           ($pendingRenameOperations.Count -gt 0)
}

function Get-DeploymentPhase {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [string]$RegistryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    )

    $value = Get-ItemProperty -Path $RegistryPath -Name $Name -ErrorAction SilentlyContinue
    if ($null -eq $value) { return 0 }
    return [int]$value.$Name
}

function Set-DeploymentPhase {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [Parameter(Mandatory)]
        [ValidateRange(0, 100)]
        [int]$Phase,

        [string]$RegistryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    )

    if (-not (Test-Path $RegistryPath)) {
        New-Item -Path $RegistryPath -Force | Out-Null
    }
    Set-ItemProperty -Path $RegistryPath -Name $Name -Value $Phase -Type DWord -Force
}

function Write-DeploymentHeartbeat {
    param(
        [Parameter(Mandatory)]
        [string]$Phase,

        [Parameter(Mandatory)]
        [string]$Operation,

        [Parameter(Mandatory)]
        [datetime]$StartedAt,

        [string]$HeartbeatPath,

        [string]$LastCheckpoint,

        [Nullable[datetime]]$Deadline
    )

    $now = Get-Date
    $elapsedSeconds = [int]($now - $StartedAt).TotalSeconds
    $deadlineText = if ($null -ne $Deadline -and $Deadline.HasValue) {
        $Deadline.Value.ToUniversalTime().ToString('o')
    } else {
        $null
    }
    $state = [ordered]@{
        TimestampUtc = $now.ToUniversalTime().ToString('o')
        Phase = $Phase
        Operation = $Operation
        ElapsedSeconds = $elapsedSeconds
        DeadlineUtc = $deadlineText
        LastCheckpoint = $LastCheckpoint
    }

    if ($HeartbeatPath) {
        $heartbeatDirectory = Split-Path -Path $HeartbeatPath -Parent
        if ($heartbeatDirectory -and -not (Test-Path $heartbeatDirectory)) {
            New-Item -ItemType Directory -Path $heartbeatDirectory -Force | Out-Null
        }
        $temporaryPath = "$HeartbeatPath.tmp"
        $state | ConvertTo-Json -Compress | Set-Content -LiteralPath $temporaryPath -Encoding UTF8 -Force
        Move-Item -LiteralPath $temporaryPath -Destination $HeartbeatPath -Force
    }

    $message = "[HEARTBEAT] Phase=$Phase; Operation=$Operation; Elapsed=${elapsedSeconds}s"
    if ($LastCheckpoint) { $message += "; LastCheckpoint=$LastCheckpoint" }
    if ($deadlineText) { $message += "; DeadlineUtc=$deadlineText" }

    $writer = Join-Path (Get-Location) 'Write-Info.ps1'
    if (Test-Path $writer) {
        & $writer $message -ForegroundColor DarkGray
    } else {
        Write-Host $message -ForegroundColor DarkGray
    }
}

function Wait-DeploymentJob {
    param(
        [Parameter(Mandatory)]
        [System.Management.Automation.Job]$Job,

        [Parameter(Mandatory)]
        [ValidateRange(1, 86400)]
        [int]$TimeoutSeconds,

        [Parameter(Mandatory)]
        [string]$Phase,

        [Parameter(Mandatory)]
        [string]$Operation,

        [string]$HeartbeatPath,

        [string]$LastCheckpoint,

        [ValidateRange(5, 600)]
        [int]$HeartbeatIntervalSeconds = 60
    )

    $startedAt = Get-Date
    $deadline = $startedAt.AddSeconds($TimeoutSeconds)
    $nextHeartbeat = $startedAt

    while ($Job.State -in @('NotStarted', 'Running', 'Blocked') -and (Get-Date) -lt $deadline) {
        if ((Get-Date) -ge $nextHeartbeat) {
            Write-DeploymentHeartbeat -Phase $Phase -Operation $Operation `
                -StartedAt $startedAt -Deadline $deadline -HeartbeatPath $HeartbeatPath `
                -LastCheckpoint $LastCheckpoint
            $nextHeartbeat = (Get-Date).AddSeconds($HeartbeatIntervalSeconds)
        }
        Wait-Job -Job $Job -Timeout ([Math]::Min(10, $HeartbeatIntervalSeconds)) | Out-Null
    }

    if ($Job.State -in @('NotStarted', 'Running', 'Blocked')) {
        Stop-Job -Job $Job
        throw "$Operation exceeded the $TimeoutSeconds-second timeout."
    }
    if ($Job.State -ne 'Completed') {
        $reason = $Job.ChildJobs[0].JobStateInfo.Reason
        throw "$Operation ended in state '$($Job.State)': $reason"
    }
    return $Job
}

function Invoke-VerifiedDscConfiguration {
    <#
    .SYNOPSIS
        Applies DSC and waits for a verified LCM success result.
    .DESCRIPTION
        Start-DscConfiguration -Wait keeps one WSMan client session open. Feature
        installation can restart WinRM, invalidating that session even while the LCM
        continues locally. Submit asynchronously, then poll with fresh calls so a
        transient WinRM restart cannot become either a false failure or false success.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [string]$OperationName = 'DSC configuration',

        [ValidateRange(1, 7200)]
        [int]$TimeoutSeconds = 3600,

        [ValidateRange(1, 300)]
        [int]$PollIntervalSeconds = 10,

        [scriptblock]$Postcondition,

        [string]$HeartbeatPath,

        [string]$PhaseName = 'DSC',

        [ValidateRange(5, 600)]
        [int]$HeartbeatIntervalSeconds = 60
    )

    $submittedAt = Get-Date
    Start-DscConfiguration -Path $Path -Verbose -Force -ErrorAction Stop

    $deadline = $submittedAt.AddSeconds($TimeoutSeconds)
    $lastStatus = $null
    $lastProbeError = $null
    $nextHeartbeat = $submittedAt

    while ((Get-Date) -lt $deadline) {
        if ((Get-Date) -ge $nextHeartbeat) {
            Write-DeploymentHeartbeat -Phase $PhaseName -Operation $OperationName `
                -StartedAt $submittedAt -Deadline $deadline -HeartbeatPath $HeartbeatPath `
                -LastCheckpoint 'DSC submitted to the Local Configuration Manager'
            $nextHeartbeat = (Get-Date).AddSeconds($HeartbeatIntervalSeconds)
        }
        $statusErrors = @()
        try {
            # Windows PowerShell 5.1 reports an expected non-terminating error while
            # the submitted configuration is still active:
            # "Start-DscConfiguration cmdlet is in progress..."
            # Explicitly suppress the error stream so a normal feature installation
            # does not flood the deployment transcript with terminating-error records.
            $lastStatus = Get-DscConfigurationStatus -All `
                -ErrorAction SilentlyContinue -ErrorVariable statusErrors |
                # A prior run can finish immediately before this submission. Never accept
                # a status whose LCM start time predates the operation being verified.
                Where-Object { $_.StartDate -ge $submittedAt } |
                Sort-Object StartDate -Descending |
                Select-Object -First 1
            if ($statusErrors.Count -gt 0) {
                $lastProbeError = @($statusErrors | ForEach-Object { $_.Exception.Message }) -join '; '
            } else {
                $lastProbeError = $null
            }
        }
        catch {
            $lastProbeError = $_.Exception.Message
            Start-Sleep -Seconds $PollIntervalSeconds
            continue
        }

        if ($null -eq $lastStatus) {
            Start-Sleep -Seconds $PollIntervalSeconds
            continue
        }

        $statusName = "$($lastStatus.Status)"
        if ($statusName -eq 'Failure') {
            $details = @($lastStatus.Error | ForEach-Object { "$_" }) -join '; '
            if ([string]::IsNullOrWhiteSpace($details)) { $details = 'No LCM error details were returned.' }
            throw "$OperationName failed in the DSC Local Configuration Manager: $details"
        }

        if ($statusName -eq 'Success') {
            if ($null -ne $Postcondition -and -not (& $Postcondition)) {
                throw "$OperationName reported Success, but its required postconditions were not met."
            }
            return $lastStatus
        }

        Start-Sleep -Seconds $PollIntervalSeconds
    }

    $statusSummary = if ($null -ne $lastStatus) { "$($lastStatus.Status)" } else { 'not available' }
    $probeSummary = if ($lastProbeError) { " Last status probe error: $lastProbeError" } else { '' }
    throw "$OperationName did not reach a verified Success state within $TimeoutSeconds seconds (last status: $statusSummary).$probeSummary"
}
