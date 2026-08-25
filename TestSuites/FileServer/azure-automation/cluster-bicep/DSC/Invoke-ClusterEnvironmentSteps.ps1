# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [string]$ConfigureFile = "$WorkingPath\Config.json",
    [string]$HeartbeatPath,
    [switch]$NoTranscript
)

$ErrorActionPreference = 'Stop'
$scriptsPath = Join-Path $PSScriptRoot 'Scripts'
$logFile = Join-Path $PSScriptRoot 'Invoke-ClusterEnvironmentSteps.log'
$transcriptStarted = $false

Push-Location $scriptsPath
if (-not $NoTranscript) {
    Start-Transcript -Path $logFile -Append -Force | Out-Null
    $transcriptStarted = $true
}
. (Join-Path $PSScriptRoot 'Deploy-CommonHelpers.ps1')

try {
    $config = Get-Content -LiteralPath $ConfigureFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    $startedAt = Get-Date
    foreach ($scriptName in @(
        'Create-SMB2Env.ps1',
        'Create-DFSCEnv.ps1',
        'Create-FSAEnv.ps1',
        'Create-AuthEnv.ps1'
    )) {
        Write-DeploymentHeartbeat -Phase 'ClusterEnvironment' `
            -Operation 'Create required FileServer environments' `
            -StartedAt $startedAt -HeartbeatPath $HeartbeatPath `
            -LastCheckpoint "Running $scriptName"
        $failureSignal = Join-Path $WorkingPath "Config_$($env:COMPUTERNAME)_FailureSignal.log"
        Remove-Item -LiteralPath $failureSignal -Force -ErrorAction SilentlyContinue
        Invoke-CheckedPowerShellProcess `
            -ScriptPath (Join-Path $scriptsPath $scriptName) `
            -WorkingDirectory $scriptsPath `
            -Operation "Required environment script $scriptName" `
            -TimeoutSeconds 1800 | Out-Null
        if (Test-Path -LiteralPath $failureSignal -PathType Leaf) {
            throw "$scriptName wrote a configuration failure signal."
        }
    }

    $productName = (Get-ComputerInfo -Property WindowsProductName `
        -ErrorAction SilentlyContinue).WindowsProductName
    if ("$productName" -match 'Azure Edition') {
        Invoke-CheckedPowerShellProcess `
            -ScriptPath (Join-Path $scriptsPath 'Create-QUICEnv.ps1') `
            -WorkingDirectory $scriptsPath `
            -Operation 'Optional SMB over QUIC environment' `
            -TimeoutSeconds 1200 | Out-Null
    }

    $sshOutput = @(& (Join-Path $scriptsPath 'Set-SshServerAuthorizedKeys.ps1') `
        -Config $config *>&1)
    Assert-DeploymentChildResult -Output $sshOutput `
        -Operation 'Cluster node SSH authorized_keys configuration' `
        -RequireTrueResult | Out-Null

    $copyVhdx = Join-Path $scriptsPath 'Copy-Vhdx.ps1'
    if (Test-Path -LiteralPath $copyVhdx -PathType Leaf) {
        Invoke-CheckedPowerShellProcess -ScriptPath $copyVhdx `
            -WorkingDirectory $scriptsPath -Operation 'Optional SQOS VHDX preparation' `
            -TimeoutSeconds 1200 | Out-Null
    }

    return $true
}
finally {
    if ($transcriptStarted) { Stop-Transcript | Out-Null }
    Pop-Location
}
