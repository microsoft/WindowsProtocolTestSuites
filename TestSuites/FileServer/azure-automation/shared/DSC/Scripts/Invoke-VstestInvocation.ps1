# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$TestContainer,

    [Parameter(Mandatory)]
    [string]$TrxResultFileName,

    [string]$TestCaseFilter = '',

    [Parameter(Mandatory)]
    [string]$ResultDirectory,

    [Parameter(Mandatory)]
    [ValidateRange(1, 1440)]
    [int]$TimeoutMinutes,

    [string]$WorkingDirectory
)

$resultPath = Join-Path $ResultDirectory $TrxResultFileName
$manifestName = [IO.Path]::GetFileNameWithoutExtension($TrxResultFileName) + '.execution.json'
$manifestPath = Join-Path $ResultDirectory $manifestName
$consoleLogPath = Join-Path $ResultDirectory (
    [IO.Path]::GetFileNameWithoutExtension($TrxResultFileName) + '.console.log'
)
$manifest = [ordered]@{
    TestContainer = $TestContainer
    TrxResultFileName = $TrxResultFileName
    TestCaseFilter = $TestCaseFilter
    Status = 'Started'
    ExitCode = $null
    ResultFileCreated = $false
    StartedAtUtc = [DateTime]::UtcNow.ToString('o')
    CompletedAtUtc = $null
    TimeoutMinutes = $TimeoutMinutes
    ErrorMessage = ''
}
$manifest | ConvertTo-Json | Set-Content -LiteralPath $manifestPath -Encoding UTF8

$arguments = @(
    'vstest'
    (Resolve-Path -LiteralPath $TestContainer -ErrorAction Stop).Path
    "/ResultsDirectory:$ResultDirectory"
    "/Logger:trx;LogFileName=$TrxResultFileName"
)
$runSettingsPath = $null
if (-not [string]::IsNullOrEmpty($TestCaseFilter)) {
    $runSettingsPath = Join-Path ([IO.Path]::GetTempPath()) `
        "wpts-$([guid]::NewGuid().ToString('N')).runsettings"
    $escapedFilter = [Security.SecurityElement]::Escape($TestCaseFilter)
    @"
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <RunConfiguration>
    <TestCaseFilter>$escapedFilter</TestCaseFilter>
  </RunConfiguration>
</RunSettings>
"@ | Set-Content -LiteralPath $runSettingsPath -Encoding UTF8
    $arguments += "/Settings:$runSettingsPath"
}

try {
    $processResult = & (Join-Path $PSScriptRoot 'Invoke-BoundedProcess.ps1') `
        -FilePath 'dotnet' -ArgumentList $arguments `
        -TimeoutSeconds ($TimeoutMinutes * 60) -WorkingDirectory $WorkingDirectory
}
finally {
    if ($runSettingsPath) {
        Remove-Item -LiteralPath $runSettingsPath -Force -ErrorAction SilentlyContinue
    }
}

$manifest['Status'] = if (-not $processResult.Started) {
    'StartFailed'
} elseif ($processResult.TimedOut) {
    'TimedOut'
} else {
    'Completed'
}
$manifest['ExitCode'] = $processResult.ExitCode
$manifest['ResultFileCreated'] = Test-Path -LiteralPath $resultPath
$manifest['CompletedAtUtc'] = $processResult.CompletedAtUtc
$manifest['ErrorMessage'] = $processResult.ErrorMessage
$manifest | ConvertTo-Json | Set-Content -LiteralPath $manifestPath -Encoding UTF8

$rawConsole = @(
    "=== STDOUT ==="
    "$($processResult.StandardOutput)"
    "=== STDERR ==="
    "$($processResult.StandardError)"
) -join [Environment]::NewLine
$rawConsole | Set-Content -LiteralPath $consoleLogPath -Encoding UTF8

$shellOutput = New-Object System.Collections.Generic.List[string]
if ($manifest['ResultFileCreated']) {
    try {
        [xml]$trx = Get-Content -LiteralPath $resultPath -Raw -ErrorAction Stop
        $counters = $trx.TestRun.ResultSummary.Counters
        $total = [int]$counters.total
        $passed = [int]$counters.passed
        $failed = [int]$counters.failed + [int]$counters.error +
            [int]$counters.timeout + [int]$counters.aborted
        $skipped = [int]$counters.inconclusive + [int]$counters.notExecuted
        $shellOutput.Add(
            "Test result '$TrxResultFileName': total=$total, passed=$passed, " +
            "failed=$failed, skipped/inconclusive=$skipped."
        )

        $failedResults = @($trx.TestRun.Results.UnitTestResult | Where-Object {
            "$($_.outcome)" -in @('Failed', 'Error', 'Timeout', 'Aborted')
        })
        foreach ($result in @($failedResults | Select-Object -First 10)) {
            $message = "$($result.Output.ErrorInfo.Message)" -split '\r?\n' |
                Select-Object -First 1
            if ([string]::IsNullOrWhiteSpace($message)) {
                $message = 'No failure message was recorded.'
            }
            $shellOutput.Add("  FAIL $($result.testName): $message")
        }
        if ($failedResults.Count -gt 10) {
            $shellOutput.Add(
                "  ... $($failedResults.Count - 10) additional failures are recorded in the TRX."
            )
        }
    }
    catch {
        $shellOutput.Add(
            "Test invocation completed, but '$TrxResultFileName' could not be summarized: " +
            "$($_.Exception.Message)"
        )
    }
} else {
    $shellOutput.Add(
        "Test invocation did not create '$TrxResultFileName' (status=$($manifest['Status']), " +
        "exitCode=$($processResult.ExitCode))."
    )
}
$shellOutput.Add("Detailed VSTest console: $consoleLogPath")

[pscustomobject]@{
    Status = $manifest['Status']
    ExitCode = $processResult.ExitCode
    ResultFileCreated = $manifest['ResultFileCreated']
    ResultPath = $resultPath
    ManifestPath = $manifestPath
    TimedOut = $processResult.TimedOut
    ProcessTerminated = $processResult.ProcessTerminated
    StandardOutput = $shellOutput -join [Environment]::NewLine
    StandardError = ''
    ConsoleLogPath = $consoleLogPath
    ErrorMessage = $processResult.ErrorMessage
}
