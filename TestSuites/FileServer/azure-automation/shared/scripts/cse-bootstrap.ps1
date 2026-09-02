# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Custom Script Extension bootstrap shared by ALL Windows VMs across scenarios.
# It is shipped at the package root and invoked with a short command line so the
# extension never embeds this script as a large base64 command argument.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet('workgroup', 'domain', 'cluster')]
    [string]$Scenario,

    [Parameter(Mandatory)]
    [ValidateSet('dc', 'driver', 'sut', 'storage', 'node01', 'node02')]
    [string]$Role,

    [Parameter(Mandatory)]
    [string]$PackageName,

    [Parameter(Mandatory)]
    [string]$DeployScript,

    [string]$PackageUrl = '',

    [string]$PackageHost = '',

    [Parameter(Mandatory)]
    [string]$PasswordBase64,

    [string]$ConfigJsonBase64 = '',

    [string]$EnableTestAutoRun = 'true',

    [switch]$PackageAlreadyExtracted
)

$ErrorActionPreference = 'Stop'
Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force
$packagePath = "C:\$PackageName"
$deploySignalName = "$([IO.Path]::GetFileNameWithoutExtension($DeployScript)).Completed.signal"
$deploySignalPath = Join-Path $packagePath "DSC\$deploySignalName"
$hadPreviousDeploySignal = Test-Path -LiteralPath $deploySignalPath
$stagedPackagePath = if ($PackageAlreadyExtracted) {
    Split-Path $PSCommandPath -Parent
} else {
    "C:\$PackageName.bootstrap-$([guid]::NewGuid().ToString('N'))"
}
Start-Transcript -Path "C:\$Scenario-$Role-setup.log" -Append

function Complete-Bootstrap {
    param([int]$ExitCode)

    Stop-Transcript -ErrorAction SilentlyContinue
    Set-Location 'C:\'
    Remove-Item -LiteralPath $stagedPackagePath -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $PSCommandPath -Force -ErrorAction SilentlyContinue
    exit $ExitCode
}

function Test-PackagePayload {
    param([Parameter(Mandatory)][string]$RootPath)

    $contractsPath = Join-Path $RootPath 'DSC\Scripts\Package-Contracts.ps1'
    if (Test-Path -LiteralPath $contractsPath -PathType Leaf) {
        . $contractsPath
        Test-DscPackageManifest -PackageRoot $RootPath `
            -ExpectedScenario $Scenario -ThrowOnFailure | Out-Null
        Write-Output 'Package manifest and content hashes verified'
        return
    }

    if ($Scenario -eq 'cluster') {
        throw 'Package-Contracts.ps1 is missing from the Cluster package.'
    }

    foreach ($legacyRequiredPath in @(
        (Join-Path $RootPath 'Config.json'),
        (Join-Path $RootPath "DSC\$DeployScript"),
        (Join-Path $RootPath 'DSC\Scripts\InstallMSIAndTools.ps1')
    )) {
        if (-not (Test-Path -LiteralPath $legacyRequiredPath -PathType Leaf)) {
            throw "Required legacy package content is missing: '$legacyRequiredPath'."
        }
    }
}

function Stop-PackageConsumers {
    $packagePattern = [regex]::Escape($packagePath)

    foreach ($task in @(Get-ScheduledTask -ErrorAction SilentlyContinue)) {
        $actionText = @($task.Actions | ForEach-Object {
            "$($_.Execute) $($_.Arguments)"
        }) -join ' '
        if ($task.State -eq 'Running' -and $actionText -match $packagePattern) {
            Write-Output "Stopping package consumer task '$($task.TaskName)'..."
            Stop-ScheduledTask -InputObject $task -ErrorAction Stop
        }
    }

    foreach ($process in @(Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
        Where-Object {
            $_.ProcessId -ne $PID -and
            -not [string]::IsNullOrWhiteSpace($_.CommandLine) -and
            $_.CommandLine -match $packagePattern
        })) {
        Write-Output "Stopping package consumer process '$($process.Name)' (PID $($process.ProcessId))..."
        Stop-Process -Id $process.ProcessId -Force -ErrorAction Stop
    }
}

trap {
    Write-Output "Bootstrap failed: $($_.Exception.Message)"
    Complete-Bootstrap -ExitCode 1
}

Write-Output "Starting $Scenario $Role setup..."

if ($Role -eq 'driver') {
    Write-Output 'Reconciling any stale Driver test run before replacing the package...'
    $testTask = Get-ScheduledTask -TaskName 'RunFileServerTests' -ErrorAction SilentlyContinue
    if ($testTask) {
        Stop-ScheduledTask -TaskName 'RunFileServerTests' -ErrorAction SilentlyContinue
        Unregister-ScheduledTask -TaskName 'RunFileServerTests' -Confirm:$false -ErrorAction SilentlyContinue
    }

    $allProcesses = @(Get-CimInstance Win32_Process -ErrorAction SilentlyContinue)
    $staleProcessIds = [System.Collections.Generic.HashSet[int]]::new()
    foreach ($process in $allProcesses) {
        if ($process.CommandLine -match '(?i)Invoke-TestRun|Execute-TestCaseByContext|dotnet(?:\.exe)?\s+vstest|testhost(?:\.exe)?') {
            [void]$staleProcessIds.Add([int]$process.ProcessId)
        }
    }
    do {
        $addedChild = $false
        foreach ($process in $allProcesses) {
            if ($staleProcessIds.Contains([int]$process.ParentProcessId) -and
                -not $staleProcessIds.Contains([int]$process.ProcessId)) {
                [void]$staleProcessIds.Add([int]$process.ProcessId)
                $addedChild = $true
            }
        }
    } while ($addedChild)
    foreach ($processId in @($staleProcessIds)) {
        Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
    }

    Remove-Item -LiteralPath @(
        'C:\Test\test.started.signal',
        'C:\Test\test.finished.signal',
        'C:\Test\test.run.completed.signal',
        'C:\Test\test.results.upload.failed.signal'
    ) -Force -ErrorAction SilentlyContinue

    if ($hadPreviousDeploySignal) {
        if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
            New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
        }
        New-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
            -Name 'DeployStep' -Value 1 -PropertyType DWord -Force | Out-Null
        New-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
            -Name 'RebootCount' -Value 0 -PropertyType DWord -Force | Out-Null
    }
}

if ($PackageAlreadyExtracted) {
    Write-Output "Validating staged package from $stagedPackagePath..."
} else {
    # Direct/manual execution fallback. Normal CSE delivery pre-extracts the
    # downloaded package and supplies -PackageAlreadyExtracted.
    $zipFile = Get-ChildItem -Path . -Filter *.zip -ErrorAction SilentlyContinue | Select-Object -First 1
    if (-not $zipFile) {
        if ([string]::IsNullOrWhiteSpace($PackageUrl) -or [string]::IsNullOrWhiteSpace($PackageHost)) {
            throw 'PackageUrl and PackageHost are required when no staged package is supplied.'
        }
        Write-Output "Waiting for DNS resolution of $PackageHost (DNS may still be coming up)..."
        $dns = $false
        for ($i = 0; $i -lt 60; $i++) {
            try {
                Resolve-DnsName -Name $PackageHost -ErrorAction Stop | Out-Null
                $dns = $true
                break
            } catch {
                Start-Sleep -Seconds 30
            }
        }
        if (-not $dns) { throw "DNS resolution of $PackageHost failed after retries." }

        $zipPath = "C:\$PackageName.zip"
        $downloaded = $false
        for ($i = 0; $i -lt 10; $i++) {
            try {
                Start-BitsTransfer -Source $PackageUrl -Destination $zipPath -ErrorAction Stop
                $downloaded = $true
                break
            } catch {
                Write-Output "Download attempt failed: $($_.Exception.Message)"
                Start-Sleep -Seconds 30
            }
        }
        if (-not $downloaded) { throw 'Package download failed.' }
        $zipFile = Get-Item $zipPath
    }

    Write-Output "Extracting $($zipFile.Name)..."
    New-Item -ItemType Directory -Path $stagedPackagePath -Force | Out-Null
    Expand-Archive -Path $zipFile.FullName -DestinationPath $stagedPackagePath -Force
    Remove-Item $zipFile.FullName -Force
}

Test-PackagePayload -RootPath $stagedPackagePath

$stagedFullPath = [IO.Path]::GetFullPath($stagedPackagePath).TrimEnd('\')
$packageFullPath = [IO.Path]::GetFullPath($packagePath).TrimEnd('\')
if ($stagedFullPath -eq $packageFullPath) {
    throw 'The staged package path must be separate from the installed package path.'
}

Write-Output "Replacing installed package at $packagePath..."
Stop-PackageConsumers
$packageRemoved = -not (Test-Path -LiteralPath $packagePath)
for ($attempt = 1; $attempt -le 12 -and -not $packageRemoved; $attempt++) {
    try {
        Remove-Item -LiteralPath $packagePath -Recurse -Force -ErrorAction Stop
        $packageRemoved = -not (Test-Path -LiteralPath $packagePath)
    }
    catch {
        if ($attempt -eq 12) { break }
        Write-Output "Package replacement attempt $attempt failed: $($_.Exception.Message)"
        Stop-PackageConsumers
        Start-Sleep -Seconds 5
    }
}
if (-not $packageRemoved) {
    $consumers = @(Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
        Where-Object {
            -not [string]::IsNullOrWhiteSpace($_.CommandLine) -and
            $_.CommandLine -match [regex]::Escape($packagePath)
        } |
        ForEach-Object { "$($_.Name) (PID $($_.ProcessId))" })
    $consumerSummary = if ($consumers.Count -gt 0) {
        $consumers -join ', '
    } else {
        '<none detected>'
    }
    throw "Existing package path could not be removed: '$packagePath'. Consumers: $consumerSummary."
}
New-Item -ItemType Directory -Path $packagePath -Force | Out-Null
Copy-Item -Path (Join-Path $stagedPackagePath '*') -Destination $packagePath -Recurse -Force
Write-Output 'Clean package installed successfully'
Remove-Item -LiteralPath $deploySignalPath -Force -ErrorAction SilentlyContinue

if (-not [string]::IsNullOrWhiteSpace($ConfigJsonBase64)) {
    $configJson = [Text.Encoding]::UTF8.GetString(
        [Convert]::FromBase64String($ConfigJsonBase64)
    )
    $null = $configJson | ConvertFrom-Json -ErrorAction Stop
    foreach ($configPath in @(
        (Join-Path $packagePath 'Config.json'),
        (Join-Path $packagePath 'DSC\Scripts\Config.json')
    )) {
        Set-Content -LiteralPath $configPath -Value $configJson `
            -Encoding UTF8 -NoNewline -Force
    }
    Write-Output 'One-click Config.json override applied'
}

if (Test-Path "$packagePath\DSC\Scripts\Set-ConfigCredential.ps1") {
    Write-Output 'Injecting credential into Config.json...'
    & "$packagePath\DSC\Scripts\Set-ConfigCredential.ps1" -PasswordBase64 $PasswordBase64
}

if ($Role -eq 'driver') {
    $testExecutionScript = "$packagePath\DSC\Scripts\Set-ConfigTestExecution.ps1"
    if (-not (Test-Path -LiteralPath $testExecutionScript -PathType Leaf)) {
        throw "Set-ConfigTestExecution.ps1 not found at '$testExecutionScript'."
    }
    & $testExecutionScript -EnableTestAutoRun $EnableTestAutoRun
}

if (Test-Path "$packagePath\DSC\$DeployScript") {
    Write-Output "Starting $DeployScript (DSC + imperative)..."
    Set-Location "$packagePath\DSC"
    try {
        & ".\$DeployScript" -WorkingPath $packagePath
    } catch {
        Write-Output "$DeployScript failed: $($_.Exception.Message)"
        Complete-Bootstrap -ExitCode 1
    }
} else {
    Write-Output "$DeployScript not found, skipping configuration"
    Complete-Bootstrap -ExitCode 1
}

Write-Output "$Scenario $Role extension setup completed"
Complete-Bootstrap -ExitCode 0
