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
