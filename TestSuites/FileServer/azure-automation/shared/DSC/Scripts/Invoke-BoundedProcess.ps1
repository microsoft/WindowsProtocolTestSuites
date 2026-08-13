# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$FilePath,

    [string[]]$ArgumentList = @(),

    [Parameter(Mandatory)]
    [ValidateRange(1, 86400)]
    [int]$TimeoutSeconds,

    [ValidateRange(1, 300)]
    [int]$OutputDrainTimeoutSeconds = 30,

    [string]$WorkingDirectory
)

$startedAtUtc = [DateTime]::UtcNow
$process = $null
$standardOutputTask = $null
$standardErrorTask = $null
$started = $false
$timedOut = $false
$processTerminated = $false
$exitCode = $null
$standardOutput = ''
$standardError = ''
$startError = ''

function ConvertTo-NativeCommandLineArgument {
    param([AllowEmptyString()] [string]$Value)

    if ($Value -notmatch '[\s"]') { return $Value }
    return '"' + (($Value -replace '(\\*)"', '$1$1\"') -replace '(\\+)$', '$1$1') + '"'
}

try {
    $startInfo = [Diagnostics.ProcessStartInfo]::new()
    $startInfo.FileName = $FilePath
    $startInfo.UseShellExecute = $false
    $startInfo.CreateNoWindow = $true
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true
    if ($WorkingDirectory) {
        $startInfo.WorkingDirectory = $WorkingDirectory
    }
    if ($startInfo.PSObject.Properties['ArgumentList']) {
        foreach ($argument in $ArgumentList) {
            [void]$startInfo.ArgumentList.Add($argument)
        }
    } else {
        $startInfo.Arguments = (@($ArgumentList | ForEach-Object {
            ConvertTo-NativeCommandLineArgument $_
        }) -join ' ')
    }

    $process = [Diagnostics.Process]::new()
    $process.StartInfo = $startInfo
    $started = $process.Start()
    if ($started) {
        $standardOutputTask = $process.StandardOutput.ReadToEndAsync()
        $standardErrorTask = $process.StandardError.ReadToEndAsync()
        $timedOut = -not $process.WaitForExit($TimeoutSeconds * 1000)
        if ($timedOut) {
            try {
                $killTreeMethod = $process.GetType().GetMethod('Kill', [type[]]@([bool]))
                if ($killTreeMethod) {
                    [void]$killTreeMethod.Invoke($process, @($true))
                } elseif ($env:OS -eq 'Windows_NT') {
                    & "$env:SystemRoot\System32\taskkill.exe" /PID $process.Id /T /F 2>&1 | Out-Null
                    if ($LASTEXITCODE -ne 0 -and -not $process.HasExited) {
                        $process.Kill()
                    }
                } else {
                    $process.Kill()
                }
            } catch {
                $startError = "Timed out, but process-tree termination failed: $($_.Exception.Message)"
            }
            [void]$process.WaitForExit(30000)
        }

        $processTerminated = $process.HasExited
        if ($processTerminated) {
            $exitCode = $process.ExitCode
            $drainTasks = [Threading.Tasks.Task[]]@($standardOutputTask, $standardErrorTask)
            $drained = [Threading.Tasks.Task]::WaitAll(
                $drainTasks,
                $OutputDrainTimeoutSeconds * 1000)
            if ($drained) {
                $standardOutput = $standardOutputTask.GetAwaiter().GetResult()
                $standardError = $standardErrorTask.GetAwaiter().GetResult()
            } else {
                $drainError = "Redirected output did not close within $OutputDrainTimeoutSeconds seconds after the process exited."
                $startError = if ($startError) { "$startError $drainError" } else { $drainError }
            }
        }
    }
} catch {
    $startError = $_.Exception.Message
} finally {
    if ($process) {
        $process.Dispose()
    }
}

[pscustomobject]@{
    Started = $started
    TimedOut = $timedOut
    ProcessTerminated = $processTerminated
    ExitCode = $exitCode
    StandardOutput = $standardOutput
    StandardError = $standardError
    ErrorMessage = $startError
    StartedAtUtc = $startedAtUtc.ToString('o')
    CompletedAtUtc = [DateTime]::UtcNow.ToString('o')
}
