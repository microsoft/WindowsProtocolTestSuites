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
if (-not [string]::IsNullOrEmpty($TestCaseFilter)) {
    $arguments += "/TestCaseFilter:$TestCaseFilter"
}

$processResult = & (Join-Path $PSScriptRoot 'Invoke-BoundedProcess.ps1') `
    -FilePath 'dotnet' -ArgumentList $arguments `
    -TimeoutSeconds ($TimeoutMinutes * 60) -WorkingDirectory $WorkingDirectory

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

[pscustomobject]@{
    Status = $manifest['Status']
    ExitCode = $processResult.ExitCode
    ResultFileCreated = $manifest['ResultFileCreated']
    ResultPath = $resultPath
    ManifestPath = $manifestPath
    TimedOut = $processResult.TimedOut
    ProcessTerminated = $processResult.ProcessTerminated
    StandardOutput = $processResult.StandardOutput
    StandardError = $processResult.StandardError
    ErrorMessage = $processResult.ErrorMessage
}
