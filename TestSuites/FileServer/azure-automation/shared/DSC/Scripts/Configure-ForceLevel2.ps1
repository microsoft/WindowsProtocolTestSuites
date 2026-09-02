# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [string]$WorkingPath = (Split-Path (Split-Path $PSScriptRoot -Parent) -Parent),
    [string]$ConfigureFile = "$WorkingPath\Config.json",
    [string]$HeartbeatPath,
    [ValidateRange(60, 3600)]
    [int]$TimeoutSeconds = 1200,
    [switch]$NoTranscript
)

$ErrorActionPreference = 'Stop'
$dscFolder = Split-Path $PSScriptRoot -Parent
$logFile = Join-Path $dscFolder 'Configure-ForceLevel2.log'
$transcriptStarted = $false
if (-not $NoTranscript) {
    Start-Transcript -Path $logFile -Append -Force | Out-Null
    $transcriptStarted = $true
}
. (Join-Path $dscFolder 'Deploy-CommonHelpers.ps1')

function Test-ShareForceLevel2 {
    param(
        [string]$ShareUtil,
        [string]$Server,
        [string]$Share
    )
    $output = @(& $ShareUtil $Server $Share 2>&1)
    return ($LASTEXITCODE -eq 0 -and
        (Test-ShareUtilForceLevel2Output -Output $output))
}

try {
    $config = Get-Content -LiteralPath $ConfigureFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    $toolsPath = Join-Path $WorkingPath 'Tools.json'
    if (-not (Test-Path -LiteralPath $toolsPath -PathType Leaf)) {
        $toolsPath = Join-Path $PSScriptRoot 'Tools.json'
    }
    $tools = Get-Content -LiteralPath $toolsPath -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    $testSuiteRoot = [Environment]::ExpandEnvironmentVariables(
        "$($tools.DriverComputer.TestsuiteZips[0].targetFolder)"
    )
    $shareUtil = Join-Path $testSuiteRoot 'Utils\ShareUtil.exe'
    if (-not (Test-Path -LiteralPath $shareUtil -PathType Leaf)) {
        throw "ShareUtil.exe was not found at '$shareUtil'."
    }

    $targets = [System.Collections.Generic.List[object]]::new()
    $targets.Add([pscustomobject]@{
        Name = 'Local'
        Server = "$($config.Machines.Node01.ComputerName)"
        Share = 'ShareForceLevel2'
        Signal = (Join-Path $dscFolder 'ForceLevel2.Local.Completed.signal')
    })
    if ("$($config.Core.Scenario)" -eq 'Cluster') {
        $targets.Add([pscustomobject]@{
            Name = 'Clustered'
            Server = "$($config.Endpoints.ScaleoutFS.Name)"
            Share = 'SMBClusteredForceLevel2'
            Signal = (Join-Path $dscFolder 'ForceLevel2.Clustered.Completed.signal')
        })
    }

    foreach ($target in $targets) {
        Remove-Item -LiteralPath $target.Signal -Force -ErrorAction SilentlyContinue
        Wait-DeploymentCondition -Condition {
            Test-Path -LiteralPath "\\$($target.Server)\$($target.Share)" `
                -PathType Container -ErrorAction SilentlyContinue
        } -TimeoutSeconds $TimeoutSeconds -PollIntervalSeconds 10 `
            -Phase 'ForceLevel2' `
            -Operation "Wait for \\$($target.Server)\$($target.Share)" `
            -HeartbeatPath $HeartbeatPath -LastCheckpoint 'Cluster endpoints ready' |
            Out-Null

        Wait-DeploymentCondition -Condition {
            & $shareUtil $target.Server $target.Share SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true 2>&1 | Out-Null
            if ($LASTEXITCODE -ne 0) { return $false }
            return (Test-ShareForceLevel2 -ShareUtil $shareUtil `
                -Server $target.Server -Share $target.Share)
        } -TimeoutSeconds $TimeoutSeconds -PollIntervalSeconds 10 `
            -Phase 'ForceLevel2' `
            -Operation "Set and verify ForceLevel2 on $($target.Server)\$($target.Share)" `
            -HeartbeatPath $HeartbeatPath -LastCheckpoint "$($target.Name) share reachable" |
            Out-Null

        Write-VerifiedDeploymentSignal -Path $target.Signal `
            -Content "FORCELEVEL2 READY; SchemaVersion=1.0; Target=$($target.Name); TimestampUtc=$((Get-Date).ToUniversalTime().ToString('o'))"
    }
    return $true
}
finally {
    if ($transcriptStarted) { Stop-Transcript | Out-Null }
}
