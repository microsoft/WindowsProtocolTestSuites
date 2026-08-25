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

function Get-PendingSystemRebootReasons {
    $reasons = New-Object System.Collections.Generic.List[string]
    $pendingRenames = Get-ItemProperty `
        -Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager' `
        -Name 'PendingFileRenameOperations' -ErrorAction SilentlyContinue
    $pendingRenameOperations = if ($null -ne $pendingRenames) {
        @($pendingRenames.PendingFileRenameOperations |
            Where-Object { -not [string]::IsNullOrWhiteSpace("$_") })
    } else {
        @()
    }

    if (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\RebootPending') {
        $reasons.Add('ComponentBasedServicing')
    }
    if (Test-Path 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update\RebootRequired') {
        $reasons.Add('WindowsUpdate')
    }
    if ($pendingRenameOperations.Count -gt 0) {
        $reasons.Add("PendingFileRenameOperations:$($pendingRenameOperations.Count)")
    }
    return $reasons.ToArray()
}

function Test-PendingSystemReboot {
    return @(Get-PendingSystemRebootReasons).Count -gt 0
}

function Test-ShareUtilForceLevel2Output {
    param([object[]]$Output)
    $text = @($Output | ForEach-Object { "$_" }) -join "`n"
    if ($text -match 'SHI1005_FLAGS_FORCE_LEVELII_OPLOCK') {
        return $true
    }
    foreach ($match in [regex]::Matches($text, '(?m)(?:^|:\s*)(\d+)\s*$')) {
        [uint64]$value = 0
        if ([uint64]::TryParse($match.Groups[1].Value, [ref]$value) -and
            ($value -band 0x1000) -ne 0) {
            return $true
        }
    }
    return $false
}

function Get-DeploymentRegistryValue {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        $DefaultValue = $null,

        [string]$RegistryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    )

    $value = Get-ItemProperty -Path $RegistryPath -Name $Name -ErrorAction SilentlyContinue
    if ($null -eq $value -or $value.PSObject.Properties.Name -notcontains $Name) {
        return $DefaultValue
    }
    return $value.$Name
}

function Set-DeploymentRegistryValue {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [Parameter(Mandatory)]
        $Value,

        [ValidateSet('Binary', 'DWord', 'ExpandString', 'MultiString', 'QWord', 'String')]
        [string]$Type = 'String',

        [string]$RegistryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    )

    if (-not (Test-Path $RegistryPath)) {
        New-Item -Path $RegistryPath -Force | Out-Null
    }
    Set-ItemProperty -Path $RegistryPath -Name $Name -Value $Value -Type $Type -Force
}

function Remove-DeploymentRegistryValue {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [string]$RegistryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    )

    Remove-ItemProperty -Path $RegistryPath -Name $Name -Force -ErrorAction SilentlyContinue
}

function Get-DeploymentRoleStateNames {
    param(
        [Parameter(Mandatory)]
        [ValidatePattern('^[A-Za-z][A-Za-z0-9]*$')]
        [string]$Role,

        [ValidatePattern('^[A-Za-z0-9]*$')]
        [string]$RebootScope = ''
    )

    $rebootPrefix = "$Role$RebootScope"
    return [pscustomobject]@{
        PhaseName = "${Role}DeployPhase"
        RebootPendingName = "${rebootPrefix}RebootPending"
        PreRebootBootTimeName = "${rebootPrefix}PreRebootBootTimeUtc"
        RebootCountName = "${rebootPrefix}RebootCount"
    }
}

function Get-CurrentBootTimeUtc {
    $operatingSystem = Get-CimInstance -ClassName Win32_OperatingSystem -ErrorAction Stop
    if ($null -eq $operatingSystem -or $null -eq $operatingSystem.LastBootUpTime) {
        throw 'The operating-system boot time could not be determined.'
    }
    return ([datetime]$operatingSystem.LastBootUpTime).ToUniversalTime()
}

function Test-DeploymentBootTimeAdvanced {
    param(
        [Parameter(Mandatory)]
        [string]$RecordedBootTimeUtc,

        [datetime]$CurrentBootTimeUtc = (Get-CurrentBootTimeUtc)
    )

    $recordedBootTime = [datetime]::MinValue
    if (-not [datetime]::TryParse(
        $RecordedBootTimeUtc,
        [System.Globalization.CultureInfo]::InvariantCulture,
        [System.Globalization.DateTimeStyles]::RoundtripKind,
        [ref]$recordedBootTime
    )) {
        return $false
    }

    return $CurrentBootTimeUtc.ToUniversalTime() -gt $recordedBootTime.ToUniversalTime()
}

function Set-DeploymentRebootPending {
    param(
        [Parameter(Mandatory)]
        [ValidatePattern('^[A-Za-z][A-Za-z0-9]*$')]
        [string]$Role,

        [ValidatePattern('^[A-Za-z0-9]*$')]
        [string]$RebootScope = '',

        [Parameter(Mandatory)]
        [ValidateRange(1, 20)]
        [int]$MaximumRebootCount,

        [string]$RegistryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    )

    $names = Get-DeploymentRoleStateNames -Role $Role -RebootScope $RebootScope
    $rebootCount = [int](Get-DeploymentRegistryValue -Name $names.RebootCountName `
        -DefaultValue 0 -RegistryPath $RegistryPath)
    if ($rebootCount -ge $MaximumRebootCount) {
        throw "$Role reboot circuit breaker reached its limit of $MaximumRebootCount."
    }

    $bootTime = Get-CurrentBootTimeUtc
    Set-DeploymentRegistryValue -Name $names.PreRebootBootTimeName `
        -Value $bootTime.ToString('o') -Type String -RegistryPath $RegistryPath
    Set-DeploymentRegistryValue -Name $names.RebootCountName `
        -Value ($rebootCount + 1) -Type DWord -RegistryPath $RegistryPath
    Set-DeploymentRegistryValue -Name $names.RebootPendingName `
        -Value 1 -Type DWord -RegistryPath $RegistryPath

    return [pscustomobject]@{
        Role = $Role
        RebootScope = $RebootScope
        RebootCount = $rebootCount + 1
        PreRebootBootTimeUtc = $bootTime
    }
}

function Confirm-DeploymentReboot {
    param(
        [Parameter(Mandatory)]
        [ValidatePattern('^[A-Za-z][A-Za-z0-9]*$')]
        [string]$Role,

        [ValidatePattern('^[A-Za-z0-9]*$')]
        [string]$RebootScope = '',

        [string]$RegistryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    )

    $names = Get-DeploymentRoleStateNames -Role $Role -RebootScope $RebootScope
    $pending = [int](Get-DeploymentRegistryValue -Name $names.RebootPendingName `
        -DefaultValue 0 -RegistryPath $RegistryPath)
    if ($pending -ne 1) {
        return $false
    }

    $recordedBootTime = [string](Get-DeploymentRegistryValue `
        -Name $names.PreRebootBootTimeName -DefaultValue '' -RegistryPath $RegistryPath)
    if (-not (Test-DeploymentBootTimeAdvanced -RecordedBootTimeUtc $recordedBootTime)) {
        throw "$Role persisted a pending reboot, but a newer boot was not observed."
    }

    Set-DeploymentRegistryValue -Name $names.RebootPendingName `
        -Value 0 -Type DWord -RegistryPath $RegistryPath
    return $true
}

function Test-VerifiedDeploymentSignal {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [string]$ExpectedContentPattern,

        [string]$ExpectedVersion,

        [switch]$RemoveInvalid
    )

    $isValid = $false
    try {
        $signal = Get-Item -LiteralPath $Path -ErrorAction SilentlyContinue
        if ($null -eq $signal -or $signal.PSIsContainer -or $signal.Length -le 0) {
            return $false
        }

        $content = Get-Content -LiteralPath $Path -Raw -ErrorAction Stop
        if ([string]::IsNullOrWhiteSpace($content)) {
            return $false
        }
        if ($ExpectedContentPattern -and
            -not [regex]::IsMatch($content, $ExpectedContentPattern)) {
            return $false
        }

        if ($ExpectedVersion) {
            $actualVersion = $null
            try {
                $signalData = $content | ConvertFrom-Json -ErrorAction Stop
                foreach ($propertyName in @('SchemaVersion', 'SignalVersion', 'Version')) {
                    if ($signalData.PSObject.Properties.Name -contains $propertyName) {
                        $actualVersion = "$($signalData.$propertyName)"
                        break
                    }
                }
            }
            catch {
                $versionMatch = [regex]::Match(
                    $content,
                    '(?im)^\s*(?:SchemaVersion|SignalVersion|Version)\s*[:=]\s*[\x22\x27]?([^\x22\x27,;\s]+)'
                )
                if ($versionMatch.Success) {
                    $actualVersion = $versionMatch.Groups[1].Value
                }
            }

            if ($actualVersion -ne $ExpectedVersion) {
                return $false
            }
        }

        $isValid = $true
        return $true
    }
    catch {
        return $false
    }
    finally {
        if (-not $isValid -and $RemoveInvalid -and
            $null -ne (Get-Item -LiteralPath $Path -ErrorAction SilentlyContinue)) {
            Remove-Item -LiteralPath $Path -Force -ErrorAction SilentlyContinue
        }
    }
}

function Write-VerifiedDeploymentSignal {
    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [Parameter(Mandatory)]
        [string]$Content
    )

    if ([string]::IsNullOrWhiteSpace($Content)) {
        throw 'Deployment completion signal content must not be empty.'
    }

    $signalDirectory = Split-Path -Path $Path -Parent
    if ($signalDirectory -and -not (Test-Path -LiteralPath $signalDirectory)) {
        New-Item -ItemType Directory -Path $signalDirectory -Force | Out-Null
    }

    $temporaryPath = "$Path.tmp.$PID.$([guid]::NewGuid().ToString('N'))"
    try {
        $Content | Set-Content -LiteralPath $temporaryPath -Force -ErrorAction Stop
        Move-Item -LiteralPath $temporaryPath -Destination $Path -Force -ErrorAction Stop

        $writtenContent = Get-Content -LiteralPath $Path -Raw -ErrorAction Stop
        if ([string]::IsNullOrWhiteSpace($writtenContent) -or
            $writtenContent.Trim() -ne $Content.Trim()) {
            throw "Deployment completion signal '$Path' failed content verification."
        }
    }
    finally {
        Remove-Item -LiteralPath $temporaryPath -Force -ErrorAction SilentlyContinue
    }
}

function Get-DeploymentPhase {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [string]$RegistryPath = 'HKLM:\SOFTWARE\ProtocolTestSuites'
    )

    return [int](Get-DeploymentRegistryValue -Name $Name -DefaultValue 0 `
        -RegistryPath $RegistryPath)
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

    Set-DeploymentRegistryValue -Name $Name -Value $Phase -Type DWord `
        -RegistryPath $RegistryPath
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
        Stop-Job -Job $Job -ErrorAction SilentlyContinue
        throw "$Operation exceeded the $TimeoutSeconds-second timeout."
    }
    if ($Job.State -ne 'Completed') {
        $reason = $Job.ChildJobs[0].JobStateInfo.Reason
        $failureOutput = @(Receive-Job -Job $Job -Keep -ErrorAction SilentlyContinue 2>&1 |
            ForEach-Object { "$_" } | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
        $details = @("$reason") + $failureOutput |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
            Select-Object -Unique
        $detailText = if (@($details).Count -gt 0) { $details -join '; ' } else { 'No job failure details were returned.' }
        throw "$Operation ended in state '$($Job.State)': $detailText"
    }
    return $Job
}

function Get-LastMeaningfulDeploymentOutput {
    param([object[]]$Output)

    $meaningful = @($Output | Where-Object {
        $null -ne $_ -and
        (-not ($_ -is [string]) -or -not [string]::IsNullOrWhiteSpace($_))
    })
    if ($meaningful.Count -eq 0) {
        return $null
    }
    return $meaningful[-1]
}

function Test-DeploymentSuccessValue {
    param($Value)

    if ($Value -is [bool]) {
        return $Value
    }
    if ($Value -is [byte] -or $Value -is [int16] -or
        $Value -is [int32] -or $Value -is [int64]) {
        return ([int64]$Value -ne 0)
    }

    $text = "$Value".Trim()
    if ($text -match '^(?i:true)$') { return $true }
    if ($text -match '^(?i:false)$') { return $false }
    return $false
}

function Wait-DeploymentCondition {
    param(
        [Parameter(Mandatory)]
        [scriptblock]$Condition,

        [Parameter(Mandatory)]
        [ValidateRange(1, 86400)]
        [int]$TimeoutSeconds,

        [ValidateRange(1, 600)]
        [int]$PollIntervalSeconds = 10,

        [string]$Phase = 'Readiness',

        [Parameter(Mandatory)]
        [string]$Operation,

        [string]$HeartbeatPath,

        [string]$LastCheckpoint,

        [ValidateRange(1, 600)]
        [int]$HeartbeatIntervalSeconds = 60
    )

    $startedAt = Get-Date
    $deadline = $startedAt.AddSeconds($TimeoutSeconds)
    $nextHeartbeat = $startedAt
    $lastError = $null

    while ((Get-Date) -lt $deadline) {
        if ((Get-Date) -ge $nextHeartbeat) {
            Write-DeploymentHeartbeat -Phase $Phase -Operation $Operation `
                -StartedAt $startedAt -Deadline $deadline -HeartbeatPath $HeartbeatPath `
                -LastCheckpoint $LastCheckpoint
            $nextHeartbeat = (Get-Date).AddSeconds($HeartbeatIntervalSeconds)
        }

        try {
            $conditionOutput = @(& $Condition)
            $conditionResult = Get-LastMeaningfulDeploymentOutput -Output $conditionOutput
            if (Test-DeploymentSuccessValue -Value $conditionResult) {
                return $true
            }
            $lastError = $null
        }
        catch {
            $lastError = $_.Exception.Message
        }

        $remainingSeconds = [int][Math]::Ceiling(($deadline - (Get-Date)).TotalSeconds)
        if ($remainingSeconds -le 0) { break }
        Start-Sleep -Seconds ([Math]::Min($PollIntervalSeconds, $remainingSeconds))
    }

    $errorSuffix = if ($lastError) { " Last error: $lastError" } else { '' }
    throw "$Operation did not satisfy its readiness condition within $TimeoutSeconds seconds.$errorSuffix"
}

function Assert-DeploymentChildResult {
    param(
        [object[]]$Output,

        [Parameter(Mandatory)]
        [string]$Operation,

        [int]$ExitCode,

        [switch]$RequireTrueResult
    )

    if ($PSBoundParameters.ContainsKey('ExitCode') -and $ExitCode -ne 0) {
        throw "$Operation failed with exit code $ExitCode."
    }

    if ($RequireTrueResult) {
        $lastOutput = Get-LastMeaningfulDeploymentOutput -Output $Output
        if (-not (Test-DeploymentSuccessValue -Value $lastOutput)) {
            throw "$Operation did not return a final success result."
        }
    }

    return $true
}

function Invoke-CheckedPowerShellProcess {
    param(
        [Parameter(Mandatory)]
        [string]$ScriptPath,

        [string[]]$ArgumentList = @(),

        [string]$WorkingDirectory = (Split-Path -Path $ScriptPath -Parent),

        [Parameter(Mandatory)]
        [string]$Operation,

        [ValidateRange(1, 14400)]
        [int]$TimeoutSeconds = 1800
    )

    $scriptFile = Get-Item -LiteralPath $ScriptPath -ErrorAction SilentlyContinue
    if ($null -eq $scriptFile -or $scriptFile.PSIsContainer -or $scriptFile.Length -le 0) {
        throw "$Operation script was not found at '$ScriptPath'."
    }

    $quotedScript = '"' + $ScriptPath.Replace('"', '\"') + '"'
    $arguments = @(
        '-NoProfile',
        '-NonInteractive',
        '-ExecutionPolicy', 'Bypass',
        '-File', $quotedScript
    )
    foreach ($argument in $ArgumentList) {
        $arguments += ('"' + "$argument".Replace('"', '\"') + '"')
    }

    $process = Start-Process -FilePath 'powershell.exe' `
        -ArgumentList ($arguments -join ' ') `
        -WorkingDirectory $WorkingDirectory -PassThru
    try {
        if (-not $process.WaitForExit($TimeoutSeconds * 1000)) {
            Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
            throw "$Operation exceeded the $TimeoutSeconds-second timeout."
        }
        if ($process.ExitCode -ne 0) {
            throw "$Operation failed with exit code $($process.ExitCode)."
        }
    }
    finally {
        $process.Dispose()
    }
    return $true
}

function Stop-DeploymentForTerminalFailure {
    param(
        [Parameter(Mandatory)]
        [string]$Message,

        [string]$Phase = 'Failure',

        [string]$Operation = 'Deployment',

        [string]$HeartbeatPath,

        [datetime]$StartedAt = (Get-Date),

        [string[]]$UnsafeTaskName = @('PostDeployReboot', 'TKFRSAR')
    )

    try {
        Write-DeploymentHeartbeat -Phase $Phase -Operation $Operation `
            -StartedAt $StartedAt -HeartbeatPath $HeartbeatPath `
            -LastCheckpoint "FAILED: $Message"
    }
    catch {
        Write-Warning "Could not update deployment failure heartbeat: $($_.Exception.Message)"
    }

    foreach ($taskName in @($UnsafeTaskName | Where-Object { $_ } | Select-Object -Unique)) {
        try {
            $task = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
            if ($null -ne $task) {
                Unregister-ScheduledTask -TaskName $taskName -Confirm:$false
            }
        }
        catch {
            Write-Warning "Could not remove unsafe scheduled task '$taskName': $($_.Exception.Message)"
        }
    }

    throw $Message
}

function ConvertTo-DscDiagnosticText {
    param([object]$Value)

    if ($null -eq $Value) { return $null }
    if ($Value -is [string]) { return $Value }

    $parts = New-Object 'System.Collections.Generic.List[string]'
    foreach ($name in @('ResourceId', 'ErrorSource', 'Message', 'ErrorCode')) {
        $property = $Value.PSObject.Properties[$name]
        if ($null -ne $property -and -not [string]::IsNullOrWhiteSpace("$($property.Value)")) {
            $parts.Add("$name=$($property.Value)")
        }
    }
    $exception = $Value.PSObject.Properties['Exception']
    if ($null -ne $exception -and
        $null -ne $exception.Value -and
        -not [string]::IsNullOrWhiteSpace("$($exception.Value.Message)")) {
        $parts.Add("Exception=$($exception.Value.Message)")
    }
    if ($parts.Count -gt 0) { return $parts.ToArray() -join ', ' }
    return "$Value"
}

function Get-DscFailureDetails {
    param(
        [Parameter(Mandatory)]
        [object]$Status,

        [datetime]$Since
    )

    $details = New-Object 'System.Collections.Generic.List[string]'
    foreach ($errorValue in @($Status.Error)) {
        $text = ConvertTo-DscDiagnosticText -Value $errorValue
        if (-not [string]::IsNullOrWhiteSpace($text)) {
            $details.Add($text)
        }
    }

    foreach ($resource in @($Status.ResourcesNotInDesiredState)) {
        if ($null -eq $resource) { continue }
        $resourceParts = New-Object 'System.Collections.Generic.List[string]'
        foreach ($name in @('ResourceId', 'SourceInfo', 'ModuleName')) {
            $property = $resource.PSObject.Properties[$name]
            if ($null -ne $property -and -not [string]::IsNullOrWhiteSpace("$($property.Value)")) {
                $resourceParts.Add("$name=$($property.Value)")
            }
        }
        foreach ($errorValue in @($resource.Error)) {
            $text = ConvertTo-DscDiagnosticText -Value $errorValue
            if (-not [string]::IsNullOrWhiteSpace($text)) {
                $resourceParts.Add("Error=$text")
            }
        }
        if ($resourceParts.Count -gt 0) {
            $details.Add("FailedResource($($resourceParts.ToArray() -join ', '))")
        }
    }

    $onlyGenericApplyError = $details.Count -eq 0 -or
        ($details.Count -eq 1 -and $details[0] -match 'SendConfigurationApply function did not succeed')
    if ($onlyGenericApplyError -and $Since -ne [datetime]::MinValue) {
        $events = @(Get-WinEvent -FilterHashtable @{
            LogName = 'Microsoft-Windows-DSC/Operational'
            StartTime = $Since
        } -ErrorAction SilentlyContinue | Where-Object {
            $_.Level -le 2 -and -not [string]::IsNullOrWhiteSpace($_.Message)
        } | Select-Object -First 10)
        foreach ($event in $events) {
            $details.Add("DSCEvent(Id=$($event.Id), Message=$($event.Message))")
        }
    }

    if ($details.Count -eq 0) { return 'No LCM error details were returned.' }
    return $details.ToArray() -join '; '
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
        [int]$HeartbeatIntervalSeconds = 60,

        [ValidateRange(1, 600)]
        [int]$TransientFailureGraceSeconds = 120
    )

    $submittedAt = Get-Date
    Start-DscConfiguration -Path $Path -Verbose -Force -ErrorAction Stop

    $deadline = $submittedAt.AddSeconds($TimeoutSeconds)
    $lastStatus = $null
    $lastProbeError = $null
    $transientFailureSince = $null
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
            $freshStatuses = @(Get-DscConfigurationStatus -All `
                -ErrorAction SilentlyContinue -ErrorVariable statusErrors |
                # A prior run can finish immediately before this submission. Never accept
                # a status whose LCM start time predates the operation being verified.
                Where-Object { $_.StartDate -ge $submittedAt })
            if ($freshStatuses.Count -gt 0) {
                $latestStartDate = ($freshStatuses |
                    Measure-Object -Property StartDate -Maximum).Maximum
                # Get-DscConfigurationStatus returns newest-first. Preserve that order
                # when PowerShell 5.1 reports multiple operations in the same second;
                # Sort-Object is unstable for equal DateTime values.
                $lastStatus = $freshStatuses |
                    Where-Object { $_.StartDate -eq $latestStartDate } |
                    Select-Object -First 1
            } else {
                $lastStatus = $null
            }
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
            $details = Get-DscFailureDetails -Status $lastStatus -Since $submittedAt
            $failedResources = @($lastStatus.ResourcesNotInDesiredState |
                Where-Object { $null -ne $_ })
            $statusErrorTexts = @($lastStatus.Error |
                Where-Object { $null -ne $_ } |
                ForEach-Object { ConvertTo-DscDiagnosticText -Value $_ } |
                Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
            $isTransientApplyFailure = $failedResources.Count -eq 0 -and
                $statusErrorTexts.Count -eq 1 -and
                $statusErrorTexts[0] -match 'SendConfigurationApply function did not succeed' -and
                $details -notmatch 'DSCEvent\('
            if ($isTransientApplyFailure) {
                if ($null -eq $transientFailureSince) {
                    $transientFailureSince = Get-Date
                }
                $lastProbeError = $details
                if (((Get-Date) - $transientFailureSince).TotalSeconds -lt
                    $TransientFailureGraceSeconds) {
                    Start-Sleep -Seconds $PollIntervalSeconds
                    continue
                }
                throw "$OperationName returned only a transient DSC apply failure for $TransientFailureGraceSeconds seconds: $details"
            }
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
