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

    [switch]$PackageAlreadyExtracted
)

$ErrorActionPreference = 'Stop'
Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force
$packagePath = "C:\$PackageName"
$deploySignalName = "$([IO.Path]::GetFileNameWithoutExtension($DeployScript)).Completed.signal"
$deploySignalPath = Join-Path $packagePath "DSC\$deploySignalName"
$hadPreviousDeploySignal = Test-Path -LiteralPath $deploySignalPath
$stagedPackagePath = if ($PackageAlreadyExtracted) { Split-Path $PSCommandPath -Parent } else { $null }
Start-Transcript -Path "C:\$Scenario-$Role-setup.log" -Append

function Complete-Bootstrap {
    param([int]$ExitCode)

    Stop-Transcript -ErrorAction SilentlyContinue
    Set-Location 'C:\'
    if ($stagedPackagePath) {
        Remove-Item -LiteralPath $stagedPackagePath -Recurse -Force -ErrorAction SilentlyContinue
    } else {
        Remove-Item -LiteralPath $PSCommandPath -Force -ErrorAction SilentlyContinue
    }
    exit $ExitCode
}

trap {
    Write-Output "Bootstrap failed: $($_.Exception.Message)"
    Complete-Bootstrap -ExitCode 1
}

Write-Output "Starting $Scenario $Role setup..."
New-Item -ItemType Directory -Path $packagePath -Force | Out-Null

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
    Write-Output "Installing staged package from $stagedPackagePath..."
    Copy-Item -Path (Join-Path $stagedPackagePath '*') -Destination $packagePath -Recurse -Force
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
    Expand-Archive -Path $zipFile.FullName -DestinationPath $packagePath -Force
    Remove-Item $zipFile.FullName -Force
}

Write-Output 'Package extracted successfully'
Remove-Item -LiteralPath $deploySignalPath -Force -ErrorAction SilentlyContinue

if (Test-Path "$packagePath\DSC\Scripts\Set-ConfigCredential.ps1") {
    Write-Output 'Injecting credential into Config.json...'
    & "$packagePath\DSC\Scripts\Set-ConfigCredential.ps1" -PasswordBase64 $PasswordBase64
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
