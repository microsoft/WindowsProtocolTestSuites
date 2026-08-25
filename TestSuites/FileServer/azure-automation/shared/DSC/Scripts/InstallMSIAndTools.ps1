# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet('SUT', 'DriverComputer', 'DC', 'Node01', 'Node02', 'Storage')]
    [string]$Role,

    [string]$ToolsPath = (Join-Path $PSScriptRoot 'Tools'),

    [string]$ConfigureFile = (Join-Path $PSScriptRoot 'Tools.json'),

    [string]$SignalFile = (Join-Path $PSScriptRoot 'InstallMSIAndTools.Completed.signal'),

    [string]$LogDirectory = $PSScriptRoot,

    [ValidateSet('Auto', 'Online', 'Offline')]
    [string]$ConnectivityMode = 'Auto',

    [ValidateRange(1, 7200)]
    [int]$InstallTimeoutSeconds = 1800,

    [ValidateSet('All', 'Prepare', 'Install')]
    [string]$Operation = 'All',

    [string]$PreparedSignalFile = (Join-Path $PSScriptRoot 'InstallMSIAndTools.Prepared.signal'),

    [ValidateRange(1, 16)]
    [int]$DownloadThrottleLimit = 4,

    [switch]$AllowRebootRequired,

    [switch]$NoTranscript
)

$ErrorActionPreference = 'Stop'
$successExitCodes = @(0, 1641, 3010, 2359302)
$transcriptStarted = $false

function Write-ToolMessage {
    param(
        [string]$Message,
        [ConsoleColor]$ForegroundColor = [ConsoleColor]::Gray
    )

    $writer = Join-Path $PSScriptRoot 'Write-Info.ps1'
    if (Test-Path $writer) {
        & $writer $Message -ForegroundColor $ForegroundColor
    } else {
        Write-Host $Message -ForegroundColor $ForegroundColor
    }
}

function Test-InternetConnection {
    try {
        $response = Invoke-WebRequest -Uri 'https://www.microsoft.com' -UseBasicParsing -TimeoutSec 10
        return $response.StatusCode -eq 200
    } catch {
        return $false
    }
}

function Test-RequiredItem {
    param($Item)
    $requiredProperty = $Item.PSObject.Properties['Required']
    return $null -eq $requiredProperty -or [bool]$requiredProperty.Value
}

function Test-ItemApplicable {
    param($Item, [string]$CurrentOSBuild)

    if ($Item.NotSupportedOSBuilds) {
        $excluded = @("$($Item.NotSupportedOSBuilds)" -split ',' | ForEach-Object { $_.Trim() })
        if ($excluded -contains $CurrentOSBuild) { return $false }
    }
    if ($Item.SupportedOSBuilds) {
        $included = @("$($Item.SupportedOSBuilds)" -split ',' | ForEach-Object { $_.Trim() })
        if ($included -notcontains $CurrentOSBuild) { return $false }
    }
    return $true
}

function Test-ConfiguredItemAlreadyInstalled {
    param($Item)

    $paths = @($Item.ExistingInstallPaths | Where-Object {
        -not [string]::IsNullOrWhiteSpace("$_")
    })
    $services = @($Item.ExistingServiceNames | Where-Object {
        -not [string]::IsNullOrWhiteSpace("$_")
    })
    if ($paths.Count -eq 0 -and $services.Count -eq 0) {
        return $false
    }

    foreach ($path in $paths) {
        $expandedPath = [Environment]::ExpandEnvironmentVariables("$path")
        if (-not (Test-Path -LiteralPath $expandedPath)) {
            return $false
        }
    }
    foreach ($serviceName in $services) {
        if ($null -eq (Get-Service -Name "$serviceName" -ErrorAction SilentlyContinue)) {
            return $false
        }
    }
    return $true
}

function Get-ItemFileName {
    param($Item)
    foreach ($property in @('MSIName', 'EXEName', 'ZipName')) {
        if ($Item.$property) { return "$($Item.$property)" }
    }
    throw "Tool '$($Item.name)' does not define MSIName, EXEName, or ZipName."
}

function Assert-PackageIntegrity {
    param(
        $Item,
        [Parameter(Mandatory)]
        [string]$Path
    )

    $file = Get-Item -LiteralPath $Path -ErrorAction SilentlyContinue
    if ($null -eq $file -or $file.PSIsContainer -or $file.Length -le 0) {
        throw "Package is missing or empty: $(Split-Path $Path -Leaf)"
    }

    if ($Item.SHA256) {
        $expectedHash = "$($Item.SHA256)".Trim().ToLowerInvariant()
        if ($expectedHash -notmatch '^[a-f0-9]{64}$') {
            throw "Configured SHA256 is invalid for $(Split-Path $Path -Leaf)."
        }
        $actualHash = (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash.ToLowerInvariant()
        if ($actualHash -ne $expectedHash) {
            throw "SHA256 mismatch for $(Split-Path $Path -Leaf): expected $expectedHash, actual $actualHash."
        }
    }

    if ([System.IO.Path]::GetExtension($Path) -ieq '.zip') {
        Add-Type -AssemblyName System.IO.Compression.FileSystem
        $archive = $null
        try {
            $archive = [System.IO.Compression.ZipFile]::OpenRead($Path)
            $entryNames = @($archive.Entries | ForEach-Object {
                $_.FullName -replace '\\', '/'
            })
            if ($entryNames.Count -eq 0) {
                throw 'ZIP archive contains no entries.'
            }
            foreach ($expectedEntry in @($Item.ExpectedEntries)) {
                if ([string]::IsNullOrWhiteSpace("$expectedEntry")) { continue }
                $normalizedExpected = "$expectedEntry" -replace '\\', '/'
                if (@($entryNames | Where-Object { $_ -like $normalizedExpected }).Count -eq 0) {
                    throw "ZIP archive is missing expected entry '$normalizedExpected'."
                }
            }
        }
        catch {
            throw "ZIP validation failed for $(Split-Path $Path -Leaf): $($_.Exception.Message)"
        }
        finally {
            if ($null -ne $archive) { $archive.Dispose() }
        }
    }
}

function Get-RequiredFile {
    param(
        $Item,
        [string]$Destination,
        [bool]$OfflineMode
    )

    if (Test-Path -LiteralPath $Destination -PathType Leaf) {
        try {
            Assert-PackageIntegrity -Item $Item -Path $Destination
            return
        }
        catch {
            $integrityError = $_.Exception.Message
            Remove-Item -LiteralPath $Destination -Force -ErrorAction SilentlyContinue
            if ($OfflineMode) {
                throw "Cached package failed integrity validation: $integrityError"
            }
            Write-ToolMessage "[WARN] Removing invalid cached package: $integrityError" Yellow
        }
    }
    if ($OfflineMode) {
        throw "Required package is not baked into Tools and outbound download is unavailable: $(Split-Path $Destination -Leaf)"
    }
    if (-not $Item.Url) {
        throw "Package is missing and no download URL is configured: $(Split-Path $Destination -Leaf)"
    }

    $downloaded = Get-RemoteFile -Url "$($Item.Url)" -OutputPath $Destination
    if (-not $downloaded -or -not (Test-Path -LiteralPath $Destination -PathType Leaf) -or
        (Get-Item -LiteralPath $Destination).Length -eq 0) {
        throw "Download did not produce a non-empty file: $(Split-Path $Destination -Leaf)"
    }
    try {
        Assert-PackageIntegrity -Item $Item -Path $Destination
    }
    catch {
        Remove-Item -LiteralPath $Destination -Force -ErrorAction SilentlyContinue
        throw
    }
}

function Assert-ExitCode {
    param([int]$ExitCode, [string]$Operation)
    if ($ExitCode -eq 1641) {
        throw "$Operation initiated a reboot (exit code $ExitCode) before deployment state could be persisted."
    }
    if ($script:Operation -eq 'Install' -and $ExitCode -eq 3010 -and -not $AllowRebootRequired) {
        throw "$Operation requested a reboot (exit code $ExitCode) without an explicitly planned reboot phase."
    }
    if ($successExitCodes -notcontains $ExitCode) {
        throw "$Operation exited with code $ExitCode."
    }
}

function Add-NoRestartArgument {
    param([string]$Arguments)
    if ($Arguments -match '(?i)(^|\s)[/-]norestart(\s|$)') {
        return $Arguments
    }
    return "$Arguments /norestart".Trim()
}

function Stop-ToolProcessTree {
    param([int]$ProcessId)

    $children = Get-CimInstance Win32_Process -Filter "ParentProcessId=$ProcessId" -ErrorAction SilentlyContinue
    foreach ($child in $children) {
        Stop-ToolProcessTree -ProcessId ([int]$child.ProcessId)
    }
    Stop-Process -Id $ProcessId -Force -ErrorAction SilentlyContinue
}

function Invoke-ToolProcess {
    param(
        [string]$FilePath,
        [string]$Arguments,
        [string]$Operation,
        [string]$StandardInput,
        [string]$StandardOutputPath,
        [string]$StandardErrorPath
    )

    $startInfo = New-Object System.Diagnostics.ProcessStartInfo
    $startInfo.FileName = $FilePath
    $startInfo.Arguments = $Arguments
    $startInfo.UseShellExecute = $false
    $startInfo.CreateNoWindow = $true
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true
    $startInfo.RedirectStandardInput = $null -ne $StandardInput

    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $startInfo
    try {
        if (-not $process.Start()) {
            throw "$Operation could not start '$FilePath'."
        }

        $stdoutTask = $process.StandardOutput.ReadToEndAsync()
        $stderrTask = $process.StandardError.ReadToEndAsync()
        if ($null -ne $StandardInput) {
            $process.StandardInput.Write($StandardInput)
            $process.StandardInput.Close()
        }

        if (-not $process.WaitForExit($InstallTimeoutSeconds * 1000)) {
            Stop-ToolProcessTree -ProcessId $process.Id
            $process.WaitForExit()
            throw "$Operation exceeded the $InstallTimeoutSeconds-second timeout."
        }
        $process.WaitForExit()

        $stdout = $stdoutTask.GetAwaiter().GetResult()
        $stderr = $stderrTask.GetAwaiter().GetResult()
        if ($StandardOutputPath) {
            Set-Content -LiteralPath $StandardOutputPath -Value $stdout -Force
        }
        if ($StandardErrorPath) {
            Set-Content -LiteralPath $StandardErrorPath -Value $stderr -Force
        }
        if ($stdout) {
            $stdout.TrimEnd() -split '\r?\n' | ForEach-Object { Write-ToolMessage $_ }
        }
        if ($stderr) {
            $stderr.TrimEnd() -split '\r?\n' | ForEach-Object { Write-ToolMessage $_ Yellow }
        }

        Assert-ExitCode -ExitCode $process.ExitCode -Operation $Operation
    } finally {
        $process.Dispose()
    }
}

function Invoke-ConfiguredItem {
    param(
        $Item,
        [bool]$OfflineMode,
        [string]$CurrentOSBuild
    )

    $label = if ($Item.name) { "$($Item.name)" } else { "$($Item.ZipName)" }
    if (-not (Test-ItemApplicable -Item $Item -CurrentOSBuild $CurrentOSBuild)) {
        Write-ToolMessage "[SKIP] $label does not apply to OS build $CurrentOSBuild." Yellow
        return
    }
    if (Test-ConfiguredItemAlreadyInstalled -Item $Item) {
        Write-ToolMessage "[OK] $label is already installed; skipping package application." Green
        return
    }

    $fileName = Get-ItemFileName -Item $Item
    $itemPath = Join-Path $ToolsPath $fileName
    Get-RequiredFile -Item $Item -Destination $itemPath -OfflineMode $OfflineMode

    if ($Item.MSIName) {
        $arguments = "/i `"$itemPath`""
        $arguments += if ($Item.ArgumentList) { " $($Item.ArgumentList)" } else { ' /quiet' }
        $arguments = Add-NoRestartArgument -Arguments $arguments
        Invoke-ToolProcess -FilePath 'msiexec.exe' -Arguments $arguments `
            -Operation "MSI install for $label"
    } elseif ($Item.EXEName) {
        $arguments = "$($Item.ArgumentList)"
        if ($AllowRebootRequired -or
            [System.IO.Path]::GetExtension($itemPath) -eq '.msu') {
            $arguments = Add-NoRestartArgument -Arguments $arguments
        }
        if ([System.IO.Path]::GetExtension($itemPath) -eq '.msu') {
            Invoke-ToolProcess -FilePath 'wusa.exe' `
                -Arguments "`"$itemPath`" $arguments" `
                -Operation "Executable install for $label"
        } else {
            Invoke-ToolProcess -FilePath $itemPath -Arguments $arguments `
                -Operation "Executable install for $label"
        }
        if ($Item.InstallWaitSeconds) {
            Start-Sleep -Seconds ([int]$Item.InstallWaitSeconds)
        }
    } else {
        $targetFolder = [Environment]::ExpandEnvironmentVariables("$($Item.targetFolder)")
        if ([string]::IsNullOrWhiteSpace($targetFolder)) {
            throw "ZIP item '$label' does not define targetFolder."
        }
        Expand-Archive -LiteralPath $itemPath -DestinationPath $targetFolder -Force
        if (-not (Test-Path -LiteralPath $targetFolder -PathType Container)) {
            throw "ZIP extraction for '$label' did not create '$targetFolder'."
        }

        if ($Item.installScript) {
            $scriptPath = Join-Path $targetFolder "$($Item.installScript)"
            if (-not (Test-Path -LiteralPath $scriptPath -PathType Leaf)) {
                throw "Install script for '$label' was not found at '$scriptPath'."
            }

            $arguments = "-NoProfile -ExecutionPolicy Bypass -File `"$scriptPath`""
            if ($Item.installScriptArgs) { $arguments += " $($Item.installScriptArgs)" }
            $stdinValue = if ($Item.name -eq 'PTMService') {
                "N`nN`nN`nN`nN`nN`nN`n"
            } else {
                "N`n"
            }
            $safeLabel = $label -replace '[^A-Za-z0-9_.-]', '_'
            $stdoutFile = Join-Path $LogDirectory "$safeLabel.install.stdout.log"
            $stderrFile = Join-Path $LogDirectory "$safeLabel.install.stderr.log"
            Invoke-ToolProcess -FilePath 'powershell.exe' -Arguments $arguments `
                -Operation "Install script for $label" -StandardInput $stdinValue `
                -StandardOutputPath $stdoutFile -StandardErrorPath $stderrFile
        }
    }

    if ($Item.PostInstallPath) {
        $postInstallPath = [Environment]::ExpandEnvironmentVariables("$($Item.PostInstallPath)")
        if (-not (Test-Path -LiteralPath $postInstallPath)) {
            throw "Post-install condition for '$label' was not met: '$postInstallPath' does not exist."
        }
    }
    Write-ToolMessage "[OK] $label completed." Green
}

function Invoke-ParallelPreparation {
    param(
        [object[]]$Items,
        [bool]$OfflineMode,
        [string]$CurrentOSBuild
    )

    if (Test-Path -LiteralPath $PreparedSignalFile) {
        Remove-Item -LiteralPath $PreparedSignalFile -Force
    }

    $failures = New-Object System.Collections.Generic.List[string]
    $pending = New-Object System.Collections.Generic.List[object]
    $destinations = @{}

    foreach ($item in $Items) {
        if ($null -eq $item -or -not (Test-ItemApplicable -Item $item -CurrentOSBuild $CurrentOSBuild)) {
            continue
        }

        $label = if ($item.name) { "$($item.name)" } else { "$($item.ZipName)" }
        $fileName = Get-ItemFileName -Item $item
        $destination = Join-Path $ToolsPath $fileName
        $destinationKey = $destination.ToLowerInvariant()
        if ($destinations.ContainsKey($destinationKey)) {
            $failures.Add("$label`: duplicate package destination '$destination'.")
            continue
        }
        $destinations[$destinationKey] = $true

        $required = Test-RequiredItem -Item $item
        if (Test-Path -LiteralPath $destination -PathType Leaf) {
            try {
                Assert-PackageIntegrity -Item $item -Path $destination
                Write-ToolMessage "[OK] Package already prepared: $fileName" Green
                continue
            }
            catch {
                $integrityError = $_.Exception.Message
                Remove-Item -LiteralPath $destination -Force -ErrorAction SilentlyContinue
                if ($OfflineMode) {
                    if ($required) {
                        $failures.Add("$label`: Cached package failed integrity validation: $integrityError")
                        Write-ToolMessage "[FAIL] $label`: $integrityError" Red
                    }
                    else {
                        Write-ToolMessage "[WARN] Optional tool '$label' failed integrity validation: $integrityError" Yellow
                    }
                    continue
                }
                Write-ToolMessage "[WARN] Removing invalid cached package for '$label': $integrityError" Yellow
            }
        }

        if ($OfflineMode) {
            $message = "Required package is not baked into Tools and outbound download is unavailable: $fileName"
            if ($required) {
                $failures.Add("$label`: $message")
                Write-ToolMessage "[FAIL] $label`: $message" Red
            } else {
                Write-ToolMessage "[WARN] Optional tool '$label' was not prepared: $message" Yellow
            }
            continue
        }
        if (-not $item.Url) {
            $message = "Package is missing and no download URL is configured: $fileName"
            if ($required) {
                $failures.Add("$label`: $message")
                Write-ToolMessage "[FAIL] $label`: $message" Red
            } else {
                Write-ToolMessage "[WARN] Optional tool '$label' was not prepared: $message" Yellow
            }
            continue
        }

        $pending.Add([pscustomobject]@{
            Label = $label
            Url = "$($item.Url)"
            Destination = $destination
            Required = $required
            Item = $item
        })
    }

    $downloadScript = Join-Path $PSScriptRoot 'Get-RemoteFile.ps1'
    $deadline = (Get-Date).AddSeconds($InstallTimeoutSeconds)
    for ($offset = 0; $offset -lt $pending.Count; $offset += $DownloadThrottleLimit) {
        $lastIndex = [Math]::Min($pending.Count - 1, $offset + $DownloadThrottleLimit - 1)
        $batch = @($pending[$offset..$lastIndex])
        $jobs = @()
        $jobItems = @{}

        foreach ($entry in $batch) {
            Write-ToolMessage "Preparing package in parallel: $($entry.Label)" Cyan
            $job = Start-Job -ScriptBlock {
                param($url, $destination, $scriptPath, $workingDirectory)
                Set-Location $workingDirectory
                . $scriptPath
                Get-ChildItem -Path "$destination.download.*" -File -ErrorAction SilentlyContinue |
                    Remove-Item -Force -ErrorAction SilentlyContinue
                $temporaryPath = "$destination.download.$PID"
                Remove-Item -LiteralPath $temporaryPath -Force -ErrorAction SilentlyContinue
                try {
                    $downloaded = Get-RemoteFile -Url $url -OutputPath $temporaryPath
                    if (-not $downloaded -or
                        -not (Test-Path -LiteralPath $temporaryPath -PathType Leaf) -or
                        (Get-Item -LiteralPath $temporaryPath).Length -eq 0) {
                        throw "Download did not produce a non-empty file."
                    }
                    Move-Item -LiteralPath $temporaryPath -Destination $destination -Force
                    [pscustomobject]@{ Success = $true; Error = $null }
                }
                catch {
                    Remove-Item -LiteralPath $temporaryPath -Force -ErrorAction SilentlyContinue
                    [pscustomobject]@{ Success = $false; Error = $_.Exception.Message }
                }
            } -ArgumentList $entry.Url, $entry.Destination, $downloadScript, $PSScriptRoot
            $jobs += $job
            $jobItems[$job.Id] = $entry
        }

        $nextHeartbeat = Get-Date
        while (@($jobs | Where-Object { $_.State -in @('NotStarted', 'Running', 'Blocked') }).Count -gt 0) {
            if ((Get-Date) -ge $deadline) {
                foreach ($job in $jobs | Where-Object { $_.State -in @('NotStarted', 'Running', 'Blocked') }) {
                    Stop-Job -Job $job
                }
                break
            }
            if ((Get-Date) -ge $nextHeartbeat) {
                $remaining = @($jobs | Where-Object { $_.State -in @('NotStarted', 'Running', 'Blocked') }).Count
                Write-ToolMessage "[HEARTBEAT] Preparing packages; ActiveDownloads=$remaining; DeadlineUtc=$($deadline.ToUniversalTime().ToString('o'))" DarkGray
                $nextHeartbeat = (Get-Date).AddSeconds(60)
            }
            Wait-Job -Job $jobs -Any -Timeout 10 | Out-Null
        }

        foreach ($job in $jobs) {
            $entry = $jobItems[$job.Id]
            $result = @(Receive-Job -Job $job -ErrorAction SilentlyContinue |
                Where-Object { $null -ne $_.PSObject.Properties['Success'] } |
                Select-Object -Last 1)
            $succeeded = $job.State -eq 'Completed' -and $result.Count -eq 1 -and $result[0].Success
            $integrityError = $null
            if ($succeeded) {
                try {
                    Assert-PackageIntegrity -Item $entry.Item -Path $entry.Destination
                }
                catch {
                    $integrityError = $_.Exception.Message
                    Remove-Item -LiteralPath $entry.Destination -Force -ErrorAction SilentlyContinue
                    $succeeded = $false
                }
            }
            if ($succeeded) {
                Write-ToolMessage "[OK] Package prepared: $(Split-Path $entry.Destination -Leaf)" Green
            } else {
                $reason = if ($integrityError) {
                    $integrityError
                } elseif ($result.Count -eq 1 -and $result[0].Error) {
                    $result[0].Error
                } elseif ($job.State -in @('NotStarted', 'Running', 'Blocked', 'Stopped')) {
                    "Download exceeded the $InstallTimeoutSeconds-second preparation timeout."
                } else {
                    "$($job.ChildJobs[0].JobStateInfo.Reason)"
                }
                if ($entry.Required) {
                    $failures.Add("$($entry.Label)`: $reason")
                    Write-ToolMessage "[FAIL] $($entry.Label)`: $reason" Red
                } else {
                    Write-ToolMessage "[WARN] Optional tool '$($entry.Label)' was not prepared: $reason" Yellow
                }
            }
            Remove-Job -Job $job -Force
        }
    }

    if ($failures.Count -gt 0) {
        Write-ToolMessage "Required package preparation failed:`n - $($failures -join "`n - ")" Red
        return $false
    }

    "Prepared $(Get-Date -Format 'o')" | Set-Content -LiteralPath $PreparedSignalFile -Force
    Write-ToolMessage '[OK] All required packages are prepared.' Green
    return $true
}

if ($Operation -ne 'Prepare' -and (Test-Path -LiteralPath $SignalFile)) {
    Remove-Item -LiteralPath $SignalFile -Force
}
if (-not (Test-Path -LiteralPath $ToolsPath)) {
    New-Item -ItemType Directory -Path $ToolsPath -Force | Out-Null
}
if (-not (Test-Path -LiteralPath $LogDirectory)) {
    New-Item -ItemType Directory -Path $LogDirectory -Force | Out-Null
}

if (-not $NoTranscript) {
    Start-Transcript -Path (Join-Path $LogDirectory 'InstallMSIAndTools.ps1.log') `
        -Append -Force | Out-Null
    $transcriptStarted = $true
}

try {
    . (Join-Path $PSScriptRoot 'Get-RemoteFile.ps1')

    $offlineMode = if ($Operation -eq 'Install') {
        $true
    } else {
        switch ($ConnectivityMode) {
            'Online' { $false }
            'Offline' { $true }
            default { -not (Test-InternetConnection) }
        }
    }
    if ($offlineMode) {
        Write-ToolMessage '[WARN] No outbound internet detected. Required packages must be baked into DSC\Scripts\Tools.' Yellow
    }

    $config = Get-Content -LiteralPath $ConfigureFile -Raw | ConvertFrom-Json
    $roleConfig = $config.$Role
    if (-not $roleConfig) {
        throw "Cannot find Tools configuration for role '$Role'."
    }

    $currentOSBuild = [System.Environment]::OSVersion.Version.Build.ToString()
    $items = @($roleConfig.Tools) + @($roleConfig.TestsuiteZips)

    if ($Operation -eq 'Prepare') {
        return (Invoke-ParallelPreparation -Items $items -OfflineMode $offlineMode `
            -CurrentOSBuild $currentOSBuild)
    }

    if ($Operation -eq 'Install' -and -not (Test-Path -LiteralPath $PreparedSignalFile)) {
        throw "Prepared package signal was not found: $PreparedSignalFile"
    }

    $failures = New-Object System.Collections.Generic.List[string]
    $installOfflineMode = $offlineMode -or $Operation -eq 'Install'
    foreach ($item in $items) {
        if ($null -eq $item) { continue }
        $required = Test-RequiredItem -Item $item
        try {
            Invoke-ConfiguredItem -Item $item -OfflineMode $installOfflineMode `
                -CurrentOSBuild $currentOSBuild
        } catch {
            $label = if ($item.name) { "$($item.name)" } else { "$($item.ZipName)" }
            if ($required) {
                $failures.Add("$label`: $($_.Exception.Message)")
                Write-ToolMessage "[FAIL] $label`: $($_.Exception.Message)" Red
            } else {
                Write-ToolMessage "[WARN] Optional tool '$label' failed: $($_.Exception.Message)" Yellow
            }
        }
    }

    if ($failures.Count -gt 0) {
        Write-ToolMessage "Required tool installation failed:`n - $($failures -join "`n - ")" Red
        return $false
    }

    "Completed $(Get-Date -Format 'o')" | Set-Content -LiteralPath $SignalFile -Force
    Write-ToolMessage '[OK] All required tools completed.' Green
    return $true
} catch {
    Write-ToolMessage "[FAIL] Tool installation could not complete: $($_.Exception.Message)" Red
    return $false
} finally {
    if ($transcriptStarted) {
        Stop-Transcript | Out-Null
    }
}
