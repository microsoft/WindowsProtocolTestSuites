# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Verify-Deployment.ps1
# Polls all VMs in a deployment resource group for completion signal files.
# Works for Domain, Cluster, and Workgroup scenarios by auto-detecting VMs.

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory=$false)]
    [string]$SubscriptionId,

    [Parameter(Mandatory=$false)]
    [ValidateSet('Auto', 'Workgroup', 'Domain', 'Cluster')]
    [string]$Scenario = 'Auto',

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, 1440)]
    [int]$TimeoutMinutes = 60,

    [Parameter(Mandatory=$false)]
    [ValidateRange(5, 300)]
    [int]$PollIntervalSeconds = 30,

    [Parameter(Mandatory=$false)]
    [ValidateRange(15, 600)]
    [int]$ProbeTimeoutSeconds = 120,

    [Parameter(Mandatory=$false)]
    [datetime]$NotBeforeUtc,

    [Parameter(Mandatory=$false)]
    [switch]$WaitForTests,

    [Parameter(Mandatory=$false)]
    [switch]$DeferTestFailure,

    [Parameter(Mandatory=$false)]
    [string[]]$ExpectedRoles,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, 1440)]
    [int]$TestTimeoutMinutes = 120,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, 1440)]
    [int]$TestStallTimeoutMinutes = 70,

    [Parameter(Mandatory=$false)]
    [string]$ResultsStorageAccountName,

    [Parameter(Mandatory=$false)]
    [string]$ResultsContainerName = 'test-results'
)

$ErrorActionPreference = 'Stop'

$helpersPath = Join-Path $PSScriptRoot '..\Deploy-Helpers.psm1'
Import-Module $helpersPath -Force
Import-AzureModules | Out-Host
if ($SubscriptionId) {
    Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
}

function Invoke-BoundedVmRunCommand {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string]$ResourceGroupName,
        [Parameter(Mandatory)] [string]$VMName,
        [Parameter(Mandatory)] [string]$CommandId,
        [Parameter(Mandatory)] [string]$ScriptString,
        [Parameter(Mandatory)] [int]$TimeoutSeconds
    )

    $probeJob = $null
    $azPowerShellFailure = $null
    try {
        $probeJob = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
            -VMName $VMName -CommandId $CommandId -ScriptString $ScriptString `
            -AsJob -ErrorAction Stop
        $completedProbe = Wait-Job -Job $probeJob -Timeout $TimeoutSeconds
        if ($null -eq $completedProbe) {
            Stop-Job -Job $probeJob -ErrorAction SilentlyContinue
            throw [TimeoutException]::new("Run Command probe on '$VMName' exceeded ${TimeoutSeconds}s.")
        }
        if ($probeJob.State -ne 'Completed') {
            throw "Run Command probe on '$VMName' ended in state '$($probeJob.State)'."
        }

        $result = @($probeJob | Receive-Job -ErrorAction Stop)
        $messages = foreach ($item in $result) {
            foreach ($value in @($item.Value)) {
                if ($value.Message) { $value.Message }
            }
        }
        return ($messages -join "`n")
    } catch {
        $azPowerShellFailure = $_
    } finally {
        if ($null -ne $probeJob) {
            Remove-Job -Job $probeJob -Force -ErrorAction SilentlyContinue
        }
    }

    $azCommand = Get-Command az -ErrorAction SilentlyContinue
    if (-not $azCommand) {
        throw $azPowerShellFailure
    }

    Write-Warning "Az PowerShell Run Command failed on '$VMName': $($azPowerShellFailure.Exception.Message) Trying one bounded Azure CLI probe."
    $effectiveSubscriptionId = if ($SubscriptionId) {
        $SubscriptionId
    } else {
        (Get-AzContext).Subscription.Id
    }
    $cliScriptPath = Join-Path ([IO.Path]::GetTempPath()) "wpts-probe-$([guid]::NewGuid().ToString('N')).ps1"
    $cliJob = $null
    try {
        $ScriptString | Set-Content -LiteralPath $cliScriptPath -Encoding UTF8
        $cliJob = Start-Job -ArgumentList @(
            $azCommand.Source,
            $effectiveSubscriptionId,
            $ResourceGroupName,
            $VMName,
            $CommandId,
            $cliScriptPath
        ) -ScriptBlock {
            param($azPath, $subscription, $resourceGroup, $vm, $command, $scriptPath)
            $output = @(& $azPath vm run-command invoke `
                --subscription $subscription --resource-group $resourceGroup `
                --name $vm --command-id $command --scripts "@$scriptPath" `
                --only-show-errors --output json 2>&1)
            [pscustomobject]@{
                ExitCode = $LASTEXITCODE
                Output = ($output -join [Environment]::NewLine)
            }
        }
        $completedCliJob = Wait-Job -Job $cliJob -Timeout $TimeoutSeconds
        if ($null -eq $completedCliJob) {
            Stop-Job -Job $cliJob -ErrorAction SilentlyContinue
            throw [TimeoutException]::new("Azure CLI Run Command probe on '$VMName' exceeded ${TimeoutSeconds}s.")
        }
        if ($cliJob.State -ne 'Completed') {
            throw "Azure CLI Run Command probe on '$VMName' ended in state '$($cliJob.State)'."
        }
        $cliResult = $cliJob | Receive-Job -ErrorAction Stop
        if ($cliResult.ExitCode -ne 0) {
            throw "Azure CLI Run Command probe on '$VMName' failed with exit code $($cliResult.ExitCode): $($cliResult.Output)"
        }
        $cliResponse = $cliResult.Output | ConvertFrom-Json -ErrorAction Stop
        return (@($cliResponse.value) | ForEach-Object { $_.message } | Where-Object { $_ }) -join "`n"
    } finally {
        if ($cliJob) {
            Remove-Job -Job $cliJob -Force -ErrorAction SilentlyContinue
        }
        Remove-Item -LiteralPath $cliScriptPath -Force -ErrorAction SilentlyContinue
    }
}

function New-SignalProbeScript {
    param(
        [Parameter(Mandatory)] [string]$SignalPath,
        [Parameter(Mandatory)] [bool]$LinuxTarget,
        [Parameter(Mandatory)] [long]$NotBeforeEpoch
    )

    if ($LinuxTarget) {
        return @"
if [ ! -f '$SignalPath' ]; then echo 'SIGNAL_PENDING'; exit 0; fi
modified=`$(date -u -r '$SignalPath' +%s)
if [ "`$modified" -lt '$NotBeforeEpoch' ]; then echo "SIGNAL_STALE|`$modified"; exit 0; fi
echo "SIGNAL_READY|`$modified"
"@
    }

    $escapedPath = $SignalPath.Replace("'", "''")
    return @"
`$signal = Get-Item -LiteralPath '$escapedPath' -ErrorAction SilentlyContinue
if (-not `$signal) {
    `$resumeTask = Get-ScheduledTask -TaskName 'ResumeDeployment' -ErrorAction SilentlyContinue
    `$resumeInfo = if (`$resumeTask) { Get-ScheduledTaskInfo -TaskName 'ResumeDeployment' -ErrorAction SilentlyContinue } else { `$null }
    # 0x800710E0 is emitted while Task Scheduler queues/refuses a competing
    # demand start; 0x41300/1/3 are ready, running, and not-yet-run states.
    `$nonTerminalTaskResults = @(0, 267008, 267009, 267011, 2147946720)
    if (`$resumeInfo -and `$resumeTask.State -ne 'Running' -and
        `$resumeInfo.LastTaskResult -notin `$nonTerminalTaskResults) {
        `$lastRunEpoch = ([DateTimeOffset]`$resumeInfo.LastRunTime.ToUniversalTime()).ToUnixTimeSeconds()
        if (`$lastRunEpoch -ge $NotBeforeEpoch) {
            Write-Output "SIGNAL_FAILED|ResumeDeployment|`$(`$resumeInfo.LastTaskResult)|`$lastRunEpoch"
            exit 0
        }
    }
    Write-Output 'SIGNAL_PENDING'
    exit 0
}
`$modified = ([DateTimeOffset]`$signal.LastWriteTimeUtc).ToUnixTimeSeconds()
if (`$modified -lt $NotBeforeEpoch) { Write-Output "SIGNAL_STALE|`$modified"; exit 0 }
Write-Output "SIGNAL_READY|`$modified"
"@
}

function New-TestProbeScript {
    param(
        [Parameter(Mandatory)] [bool]$LinuxTarget,
        [Parameter(Mandatory)] [long]$NotBeforeEpoch,
        [Parameter(Mandatory)] [int]$StallTimeoutSeconds
    )

    if ($LinuxTarget) {
        return @"
test_signal='/test/test.finished.signal'
run_signal='/test/test.run.completed.signal'
upload_failure='/test/test.results.upload.failed.signal'
if [ -f "`$upload_failure" ]; then
    upload_failure_modified=`$(date -u -r "`$upload_failure" +%s)
    if [ "`$upload_failure_modified" -ge '$NotBeforeEpoch' ]; then echo "TEST_UPLOAD_FAILED|`$(cat "`$upload_failure")"; exit 0; fi
fi
if [ ! -f "`$test_signal" ] || [ ! -f "`$run_signal" ]; then
    latest_manifest=`$(find /test/TestResults -type f -name '*.execution.json' -printf '%T@ %p\n' 2>/dev/null | sort -nr | head -1 | cut -d' ' -f2-)
    if [ -n "`$latest_manifest" ] && grep -q '"Status"[[:space:]]*:[[:space:]]*"Started"' "`$latest_manifest"; then
        manifest_modified=`$(date -u -r "`$latest_manifest" +%s)
        manifest_age=`$((`$(date -u +%s) - manifest_modified))
        if [ "`$manifest_modified" -ge '$NotBeforeEpoch' ] && [ "`$manifest_age" -ge '$StallTimeoutSeconds' ]; then
            echo "TEST_STALLED|`$(basename "`$latest_manifest")|`$manifest_age"
            exit 0
        fi
    fi
    if [ -f '/test/test.started.signal' ]; then
        started_modified=`$(date -u -r '/test/test.started.signal' +%s)
        if [ "`$started_modified" -ge '$NotBeforeEpoch' ] && ! pgrep -f '[I]nvoke-TestRun.ps1' >/dev/null 2>&1; then
            echo "TEST_TASK_FAILED|LinuxBackgroundProcess|not-running"
            exit 0
        fi
    fi
    echo "TEST_PENDING|`$(basename "`$latest_manifest" 2>/dev/null)|`$(test -n "`$latest_manifest" && stat -c %Y "`$latest_manifest" 2>/dev/null)"
    exit 0
fi
test_modified=`$(date -u -r "`$test_signal" +%s)
run_modified=`$(date -u -r "`$run_signal" +%s)
if [ "`$test_modified" -lt '$NotBeforeEpoch' ] || [ "`$run_modified" -lt '$NotBeforeEpoch' ]; then echo "TEST_STALE|`$test_modified|`$run_modified"; exit 0; fi
trx_count=`$(find /test/TestResults -type f -name '*.trx' 2>/dev/null | wc -l)
failed_count=`$(find /test/TestResults -type f -name '*.trx' -exec grep -l 'outcome="Failed"' {} + 2>/dev/null | wc -l)
summary_json='/test/TestResults/test.summary.json'
summary_text='/test/TestResults/test.summary.txt'
if [ ! -f "`$summary_json" ] || [ ! -f "`$summary_text" ]; then echo 'TEST_SUMMARY_MISSING'; exit 0; fi
classification=`$(grep -o '"Classification"[[:space:]]*:[[:space:]]*"[^"]*"' "`$summary_json" | head -1 | cut -d'"' -f4)
failed_test_count=`$(grep -c '"TestName"[[:space:]]*:' "`$summary_json" 2>/dev/null || true)
summary_size=`$(wc -c < "`$summary_text")
echo "TEST_READY|`$trx_count|`$failed_count|`$classification|`$failed_test_count|`$summary_size"
"@
    }

    return @"
`$testSignal = Get-Item -LiteralPath 'C:\Test\test.finished.signal' -ErrorAction SilentlyContinue
`$runSignal = Get-Item -LiteralPath 'C:\Test\test.run.completed.signal' -ErrorAction SilentlyContinue
`$uploadFailure = Get-Item -LiteralPath 'C:\Test\test.results.upload.failed.signal' -ErrorAction SilentlyContinue
if (`$uploadFailure) {
    `$uploadFailureModified = ([DateTimeOffset]`$uploadFailure.LastWriteTimeUtc).ToUnixTimeSeconds()
    if (`$uploadFailureModified -ge $NotBeforeEpoch) { Write-Output "TEST_UPLOAD_FAILED|`$(Get-Content `$uploadFailure.FullName -Raw)"; exit 0 }
}
if (-not `$testSignal -or -not `$runSignal) {
    `$task = Get-ScheduledTask -TaskName 'RunFileServerTests' -ErrorAction SilentlyContinue
    `$taskInfo = if (`$task) { Get-ScheduledTaskInfo -TaskName 'RunFileServerTests' -ErrorAction SilentlyContinue } else { `$null }
    `$latestManifestFile = Get-ChildItem -LiteralPath 'C:\Test\TestResults' -Filter '*.execution.json' -File -ErrorAction SilentlyContinue |
        Sort-Object LastWriteTimeUtc -Descending | Select-Object -First 1
    `$latestManifest = if (`$latestManifestFile) {
        Get-Content -LiteralPath `$latestManifestFile.FullName -Raw -ErrorAction SilentlyContinue | ConvertFrom-Json -ErrorAction SilentlyContinue
    } else { `$null }
    if (`$latestManifest -and `$latestManifest.Status -eq 'Started') {
        `$manifestStartedEpoch = ([DateTimeOffset]([DateTime]`$latestManifest.StartedAtUtc).ToUniversalTime()).ToUnixTimeSeconds()
        `$manifestAgeSeconds = [math]::Floor(([DateTime]::UtcNow - ([DateTime]`$latestManifest.StartedAtUtc).ToUniversalTime()).TotalSeconds)
        if (`$manifestStartedEpoch -ge $NotBeforeEpoch -and `$manifestAgeSeconds -ge $StallTimeoutSeconds) {
            Write-Output "TEST_STALLED|`$(`$latestManifestFile.Name)|`$manifestAgeSeconds"
            exit 0
        }
    }
    `$testProcesses = @(Get-CimInstance Win32_Process -ErrorAction SilentlyContinue |
        Where-Object { `$_.CommandLine -match 'Invoke-TestRun|Execute-TestCaseByContext|dotnet(?:\.exe)?\s+vstest|testhost(?:\.exe)?' })
    if (`$testProcesses.Count -eq 0 -and `$taskInfo -and `$task.State -ne 'Running') {
        `$lastRunEpoch = ([DateTimeOffset]`$taskInfo.LastRunTime.ToUniversalTime()).ToUnixTimeSeconds()
        if (`$lastRunEpoch -ge $NotBeforeEpoch -and `$taskInfo.LastTaskResult -ne 0) {
            Write-Output "TEST_TASK_FAILED|RunFileServerTests|`$(`$taskInfo.LastTaskResult)"
            exit 0
        }
    }
    `$startedSignal = Get-Item -LiteralPath 'C:\Test\test.started.signal' -ErrorAction SilentlyContinue
    if (-not `$task -and `$startedSignal -and `$testProcesses.Count -eq 0) {
        `$startedEpoch = ([DateTimeOffset]`$startedSignal.LastWriteTimeUtc).ToUnixTimeSeconds()
        if (`$startedEpoch -ge $NotBeforeEpoch) {
            Write-Output 'TEST_TASK_FAILED|RunFileServerTests|missing-without-process'
            exit 0
        }
    }
    Write-Output "TEST_PENDING|`$(`$task.State)|`$(`$taskInfo.LastTaskResult)|`$(`$latestManifestFile.Name)|`$(`$latestManifest.Status)"
    exit 0
}
`$testModified = ([DateTimeOffset]`$testSignal.LastWriteTimeUtc).ToUnixTimeSeconds()
`$runModified = ([DateTimeOffset]`$runSignal.LastWriteTimeUtc).ToUnixTimeSeconds()
if (`$testModified -lt $NotBeforeEpoch -or `$runModified -lt $NotBeforeEpoch) { Write-Output "TEST_STALE|`$testModified|`$runModified"; exit 0 }
`$trxFiles = @(Get-ChildItem -Path 'C:\Test\TestResults' -Filter '*.trx' -File -Recurse -ErrorAction SilentlyContinue)
`$failedFiles = @(`$trxFiles | Where-Object { Select-String -LiteralPath `$_.FullName -Pattern 'outcome="Failed"' -Quiet })
`$summaryPath = 'C:\Test\TestResults'
`$summaryJson = Join-Path `$summaryPath 'test.summary.json'
`$summaryText = Join-Path `$summaryPath 'test.summary.txt'
if (-not (Test-Path -LiteralPath `$summaryJson) -or -not (Test-Path -LiteralPath `$summaryText)) { Write-Output 'TEST_SUMMARY_MISSING'; exit 0 }
`$summary = Get-Content -LiteralPath `$summaryJson -Raw | ConvertFrom-Json
`$failedTestCount = @(`$summary.FailedTests).Count
`$summaryLength = (Get-Item -LiteralPath `$summaryText).Length
Write-Output "TEST_READY|`$(`$trxFiles.Count)|`$(`$failedFiles.Count)|`$(`$summary.Classification)|`$failedTestCount|`$summaryLength"
"@
}

function New-TestSummaryChunkProbeScript {
    param(
        [Parameter(Mandatory)] [bool]$LinuxTarget,
        [Parameter(Mandatory)] [long]$Offset,
        [Parameter(Mandatory)] [int]$ChunkSize
    )

    if ($LinuxTarget) {
        return @"
summary='/test/TestResults/test.summary.txt'
compressed=`$(mktemp)
trap 'rm -f "`$compressed"' EXIT
gzip -c "`$summary" > "`$compressed"
compressed_total=`$(wc -c < "`$compressed")
original_total=`$(wc -c < "`$summary")
data=`$(dd if="`$compressed" bs=1 skip=$Offset count=$ChunkSize 2>/dev/null | base64 | tr -d '\r\n')
echo "TEST_SUMMARY_CHUNK|$Offset|`$compressed_total|`$original_total|`$data"
"@
    }

    return @"
`$summaryPath = Join-Path 'C:\Test\TestResults' 'test.summary.txt'
`$bytes = [System.IO.File]::ReadAllBytes(`$summaryPath)
`$compressedStream = [System.IO.MemoryStream]::new()
try {
    `$gzipStream = [System.IO.Compression.GZipStream]::new(
        `$compressedStream,
        [System.IO.Compression.CompressionMode]::Compress,
        `$true)
    try { `$gzipStream.Write(`$bytes, 0, `$bytes.Length) } finally { `$gzipStream.Dispose() }
    `$compressedBytes = `$compressedStream.ToArray()
} finally {
    `$compressedStream.Dispose()
}
`$offset = [long]$Offset
`$count = [Math]::Min([int]$ChunkSize, `$compressedBytes.Length - `$offset)
`$chunk = [byte[]]::new(`$count)
[Array]::Copy(`$compressedBytes, `$offset, `$chunk, 0, `$count)
`$data = [Convert]::ToBase64String(`$chunk)
Write-Output "TEST_SUMMARY_CHUNK|`$offset|`$(`$compressedBytes.Length)|`$(`$bytes.Length)|`$data"
"@
}

function Get-RemoteTestSummaryText {
    param(
        [Parameter(Mandatory)] [string]$ResourceGroupName,
        [Parameter(Mandatory)] [psobject]$Driver,
        [Parameter(Mandatory)] [long]$SummaryLength,
        [Parameter(Mandatory)] [int]$TimeoutSeconds
    )

    # Base64 expands by 4/3; 2800 compressed bytes plus the marker remain below
    # the Run Command plugin's approximately 4 KiB stdout envelope.
    $chunkSize = 2800
    $offset = 0L
    $compressedLength = $null
    $compressedSummary = [System.IO.MemoryStream]::new()
    try {
        while ($null -eq $compressedLength -or $offset -lt $compressedLength) {
            $chunkScript = New-TestSummaryChunkProbeScript -LinuxTarget $Driver.IsLinux `
                -Offset $offset -ChunkSize $chunkSize
            $chunkOutput = $null
            for ($chunkAttempt = 1; $chunkAttempt -le 3; $chunkAttempt++) {
                try {
                    $chunkOutput = Invoke-BoundedVmRunCommand -ResourceGroupName $ResourceGroupName `
                        -VMName $Driver.VMName -CommandId $Driver.CommandId `
                        -ScriptString $chunkScript -TimeoutSeconds $TimeoutSeconds
                    break
                } catch {
                    if ($chunkAttempt -eq 3) { throw }
                    $retryDelaySeconds = [math]::Min(5 * [math]::Pow(2, $chunkAttempt - 1), 20)
                    Write-Warning "Summary chunk transport failed at offset $offset (attempt $chunkAttempt/3): $($_.Exception.Message). Retrying in ${retryDelaySeconds}s."
                    Start-Sleep -Seconds $retryDelaySeconds
                }
            }
            $match = [regex]::Match($chunkOutput, 'TEST_SUMMARY_CHUNK\|(\d+)\|(\d+)\|(\d+)\|([A-Za-z0-9+/=]*)')
            if (-not $match.Success) {
                throw "Summary chunk at offset $offset was not returned by '$($Driver.VMName)'."
            }
            $returnedOffset = [long]$match.Groups[1].Value
            $reportedCompressedLength = [long]$match.Groups[2].Value
            $reportedOriginalLength = [long]$match.Groups[3].Value
            if ($null -eq $compressedLength) { $compressedLength = $reportedCompressedLength }
            if ($returnedOffset -ne $offset -or
                $reportedCompressedLength -ne $compressedLength -or
                $reportedOriginalLength -ne $SummaryLength) {
                throw "Summary changed while being read from '$($Driver.VMName)'."
            }
            $chunkBytes = [Convert]::FromBase64String($match.Groups[4].Value)
            if ($chunkBytes.Count -eq 0) {
                throw "Summary chunk at offset $offset was empty."
            }
            $compressedSummary.Write($chunkBytes, 0, $chunkBytes.Count)
            $offset += $chunkBytes.Count
        }

        if ($compressedSummary.Length -ne $compressedLength) {
            throw "Retrieved $($compressedSummary.Length) of $compressedLength compressed summary bytes from '$($Driver.VMName)'."
        }
        $compressedSummary.Position = 0
        $gzipStream = [System.IO.Compression.GZipStream]::new(
            $compressedSummary,
            [System.IO.Compression.CompressionMode]::Decompress,
            $true)
        $summaryBytes = [System.IO.MemoryStream]::new()
        try {
            $gzipStream.CopyTo($summaryBytes)
        } finally {
            $gzipStream.Dispose()
        }
        try {
            if ($summaryBytes.Length -ne $SummaryLength) {
                throw "Decompressed $($summaryBytes.Length) of $SummaryLength summary bytes from '$($Driver.VMName)'."
            }
            return [Text.Encoding]::UTF8.GetString($summaryBytes.ToArray())
        } finally {
            $summaryBytes.Dispose()
        }
    } finally {
        $compressedSummary.Dispose()
    }
}

function Get-UploadedTestSummaryText {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string]$ResourceGroupName,
        [Parameter(Mandatory)] [string]$StorageAccountName,
        [Parameter(Mandatory)] [string]$ContainerName,
        [Parameter(Mandatory)] [string]$Scenario,
        [Parameter(Mandatory)] [datetime]$NotBeforeUtc,
        [Parameter(Mandatory)] [long]$SummaryLength
    )

    $storageAccount = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName `
        -Name $StorageAccountName -ErrorAction Stop
    $summaryBlobs = @(Get-AzStorageBlob -Container $ContainerName `
        -Context $storageAccount.Context -ErrorAction Stop | Where-Object {
            $_.Name -like "$Scenario/*/TestResults/test.summary.txt"
        } | ForEach-Object {
            $lastModified = if ($_.LastModified) {
                $_.LastModified.UtcDateTime
            } elseif ($_.ICloudBlob.Properties.LastModified) {
                $_.ICloudBlob.Properties.LastModified.UtcDateTime
            } else {
                [datetime]::MinValue
            }
            [pscustomobject]@{ Blob = $_; LastModifiedUtc = $lastModified }
        } | Where-Object { $_.LastModifiedUtc -ge $NotBeforeUtc.ToUniversalTime() } |
        Sort-Object LastModifiedUtc -Descending)

    if ($summaryBlobs.Count -eq 0) {
        throw "No fresh uploaded $Scenario summary was found in '$StorageAccountName/$ContainerName'."
    }

    $summaryBlob = $summaryBlobs[0].Blob
    $tempPath = Join-Path $env:TEMP "wpts-summary-$([guid]::NewGuid().ToString('N')).txt"
    try {
        Get-AzStorageBlobContent -Container $ContainerName -Blob $summaryBlob.Name `
            -Destination $tempPath -Context $storageAccount.Context -Force -ErrorAction Stop |
            Out-Null
        $summaryBytes = [IO.File]::ReadAllBytes($tempPath)
        if ($summaryBytes.Length -ne $SummaryLength) {
            throw "Uploaded summary length is $($summaryBytes.Length) bytes; expected $SummaryLength bytes."
        }
        Write-Host "Retrieved complete test summary directly from $StorageAccountName/$ContainerName/$($summaryBlob.Name)."
        return [Text.Encoding]::UTF8.GetString($summaryBytes)
    } finally {
        Remove-Item -LiteralPath $tempPath -Force -ErrorAction SilentlyContinue
    }
}

$scenarioDefinitions = @{
    Workgroup = @{
        Package = 'Workgroup-Package'
        Targets = @(
            @{ Pattern = '*-node01';   Signal = 'Deploy-SUT.Completed.signal';    Role = 'SUT' }
            @{ Pattern = '*-client01'; Signal = 'Deploy-Driver.Completed.signal'; Role = 'Driver Computer' }
        )
    }
    Domain = @{
        Package = 'Domain-Package'
        Targets = @(
            @{ Pattern = '*-dc01';     Signal = 'Deploy-DC.Completed.signal';     Role = 'Domain Controller' }
            @{ Pattern = '*-node01';   Signal = 'Deploy-SUT.Completed.signal';    Role = 'SUT' }
            @{ Pattern = '*-client01'; Signal = 'Deploy-Driver.Completed.signal'; Role = 'Driver Computer' }
        )
    }
    Cluster = @{
        Package = 'Cluster-Package'
        Targets = @(
            @{ Pattern = '*-dc01';      Signal = 'Deploy-DC.Completed.signal';      Role = 'Domain Controller' }
            @{ Pattern = '*-storage01'; Signal = 'Deploy-Storage.Completed.signal'; Role = 'Storage Server' }
            @{ Pattern = '*-node01';    Signal = 'Deploy-Node01.Completed.signal';  Role = 'Cluster Node 1' }
            @{ Pattern = '*-node02';    Signal = 'Deploy-Node02.Completed.signal';  Role = 'Cluster Node 2' }
            @{ Pattern = '*-client01';  Signal = 'Deploy-Driver.Completed.signal';  Role = 'Driver Computer' }
        )
    }
}

Write-Host "Verifying deployment completion..."
Write-Host "Resource Group: $ResourceGroupName"
Write-Host "Timeout: $TimeoutMinutes minutes | Poll interval: $PollIntervalSeconds seconds"
Write-Host ""

$allVms = @(Get-AzVM -ResourceGroupName $ResourceGroupName -ErrorAction Stop)
if ($allVms.Count -eq 0) {
    throw "No VMs found in resource group '$ResourceGroupName'."
}

if ($Scenario -eq 'Auto') {
    if ($allVms.Name -like '*-node02' -or $allVms.Name -like '*-storage01') {
        $Scenario = 'Cluster'
    } elseif ($allVms.Name -like '*-dc01') {
        $Scenario = 'Domain'
    } else {
        $Scenario = 'Workgroup'
    }
    Write-Warning "Scenario auto-detected as '$Scenario'. Pass -Scenario explicitly when verifying a partial or failed deployment."
}

$definition = $scenarioDefinitions[$Scenario]
$packageFolder = $definition.Package
$targetDefinitions = @($definition.Targets)
if ($ExpectedRoles) {
    $knownRoles = @($targetDefinitions | ForEach-Object { $_.Role })
    $unknownRoles = @($ExpectedRoles | Where-Object { $_ -notin $knownRoles })
    if ($unknownRoles.Count -gt 0) {
        throw "Unknown role(s) for scenario '$Scenario': $($unknownRoles -join ', ')."
    }
    $targetDefinitions = @($targetDefinitions | Where-Object { $_.Role -in $ExpectedRoles })
}
$targets = @()
foreach ($entry in $targetDefinitions) {
    $targetMatches = @($allVms | Where-Object { $_.Name -like $entry.Pattern })
    if ($targetMatches.Count -ne 1) {
        throw "Scenario '$Scenario' requires exactly one VM matching '$($entry.Pattern)' ($($entry.Role)); found $($targetMatches.Count)."
    }
    $vm = $targetMatches[0]
    $linuxVm = "$($vm.StorageProfile.OsDisk.OsType)" -eq 'Linux'
    $signalPath = if ($linuxVm) {
        "/opt/$packageFolder/DSC/$($entry.Signal)"
    } else {
        "C:\$packageFolder\DSC\$($entry.Signal)"
    }
    $targets += [pscustomobject]@{
        VMName      = $vm.Name
        SignalPath  = $signalPath
        Role        = $entry.Role
        IsLinux     = $linuxVm
        CommandId   = if ($linuxVm) { 'RunShellScript' } else { 'RunPowerShellScript' }
    }
}

Write-Host "Scenario: $Scenario"
Write-Host "Detected package folder: $packageFolder"
Write-Host "Monitoring $($targets.Count) VMs..."
Write-Host ""

$startTime = Get-Date
$timeoutTime = $startTime.AddMinutes($TimeoutMinutes)
$notBeforeEpoch = if ($PSBoundParameters.ContainsKey('NotBeforeUtc')) {
    ([DateTimeOffset]$NotBeforeUtc.ToUniversalTime()).ToUnixTimeSeconds()
} else { 0L }
$allComplete = $false
$statusList = @()
$currentPollInterval = $PollIntervalSeconds
$completedTargets = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
$configurationFailure = $null

while (-not $allComplete -and (Get-Date) -lt $timeoutTime) {
    $elapsed = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
    Write-Host "[$elapsed min] Checking VM configuration status..."

    $allComplete = $true
    $statusList = @()
    $hadProbeError = $false
    $probeEntries = @()

    foreach ($target in $targets) {
        if ($completedTargets.Contains($target.VMName)) {
            Write-Host "  $($target.VMName) ($($target.Role)): [OK] COMPLETE (cached)"
            $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'COMPLETE' }
            continue
        }
        Write-Host "  Checking $($target.VMName) ($($target.Role))..."

        try {
            $checkScript = New-SignalProbeScript -SignalPath $target.SignalPath `
                -LinuxTarget $target.IsLinux -NotBeforeEpoch $notBeforeEpoch
            $probeJob = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                -VMName $target.VMName -CommandId $target.CommandId `
                -ScriptString $checkScript -AsJob -ErrorAction Stop
            $probeEntries += [pscustomobject]@{ Target = $target; Job = $probeJob }
        } catch {
            Write-Host "    [!] ERROR submitting probe: $($_.Exception.Message)"
            $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'ERROR' }
            $allComplete = $false
            $hadProbeError = $true
        }
    }

    if ($probeEntries.Count -gt 0) {
        $remainingSeconds = [math]::Max(1, [int]($timeoutTime - (Get-Date)).TotalSeconds)
        $probeTimeout = [math]::Min($ProbeTimeoutSeconds, $remainingSeconds)
        $probeJobs = @($probeEntries | ForEach-Object { $_.Job })
        Wait-Job -Job $probeJobs -Timeout $probeTimeout | Out-Null

        foreach ($probeEntry in $probeEntries) {
            $target = $probeEntry.Target
            $probeJob = $probeEntry.Job
            try {
                if ($probeJob.State -in @('Running', 'NotStarted')) {
                    Stop-Job -Job $probeJob -ErrorAction SilentlyContinue
                    throw "Run Command probe on '$($target.VMName)' exceeded ${probeTimeout}s."
                }
                if ($probeJob.State -ne 'Completed') {
                    $reason = $probeJob.ChildJobs[0].JobStateInfo.Reason
                    throw "Run Command probe on '$($target.VMName)' ended in state '$($probeJob.State)': $reason"
                }

                $result = @($probeJob | Receive-Job -ErrorAction Stop)
                $output = @($result | ForEach-Object { @($_.Value) } |
                    ForEach-Object { $_.Message }) -join "`n"

            if ($output -match 'SIGNAL_READY\|') {
                Write-Host "    [OK] COMPLETE"
                $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'COMPLETE' }
                [void]$completedTargets.Add($target.VMName)
            } elseif ($output -match 'SIGNAL_FAILED\|([^|]+)\|([^|]+)\|([^|\r\n]+)') {
                $taskName = $Matches[1]
                $taskResult = $Matches[2]
                Write-Host "    [!] FAILED: task '$taskName' returned $taskResult"
                $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'ERROR' }
                $configurationFailure = "ResumeDeployment task failed on '$($target.VMName)' with result $taskResult."
                $allComplete = $false
            } elseif ($output -match 'SIGNAL_STALE\|') {
                Write-Host "    [..] STALE SIGNAL - waiting for this deployment run"
                $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'STALE' }
                $allComplete = $false
            } else {
                Write-Host "    ... IN PROGRESS"
                $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'IN PROGRESS' }
                $allComplete = $false
            }
            } catch {
                Write-Host "    [!] ERROR: $($_.Exception.Message)"
                $statusList += @{ VM = $target.VMName; Role = $target.Role; Status = 'ERROR' }
                $allComplete = $false
                $hadProbeError = $true
            } finally {
                Remove-Job -Job $probeJob -Force -ErrorAction SilentlyContinue
            }
        }
    }

    Write-Host ""

    if ($configurationFailure) {
        $failureError = [System.Management.Automation.ErrorRecord]::new(
            [InvalidOperationException]::new($configurationFailure),
            'DeploymentConfigurationFailed',
            [System.Management.Automation.ErrorCategory]::InvalidResult,
            $ResourceGroupName)
        $PSCmdlet.ThrowTerminatingError($failureError)
    }

    if (-not $allComplete) {
        $currentPollInterval = if ($hadProbeError) {
            [math]::Min([math]::Max($PollIntervalSeconds, $currentPollInterval * 2), 120)
        } else { $PollIntervalSeconds }
        $remainingSeconds = [math]::Max(0, [int]($timeoutTime - (Get-Date)).TotalSeconds)
        $sleepSeconds = [math]::Min($currentPollInterval, $remainingSeconds)
        if ($sleepSeconds -gt 0) {
            Write-Host "Not all VMs complete. Next check in $sleepSeconds seconds (timeout at $(Get-Date $timeoutTime -Format 'HH:mm:ss'))..."
            Write-Host ""
            Start-Sleep -Seconds $sleepSeconds
        }
    }
}

# Final report
Write-Host "======================================================="
Write-Host "FINAL STATUS"
Write-Host "======================================================="

$completeCount = 0
foreach ($s in $statusList) {
    $icon = switch ($s.Status) {
        'COMPLETE'    { '[OK]'; $completeCount++; break }
        'IN PROGRESS' { '[..]'; break }
        'STALE'       { '[<>]'; break }
        'ERROR'       { '[!!]'; break }
    }
    Write-Host "  $icon $($s.Role) ($($s.VM)): $($s.Status)"
}

$totalTime = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
Write-Host ""
Write-Host "  $completeCount/$($statusList.Count) VMs completed in $totalTime minutes"
Write-Host "======================================================="

if ($allComplete) {
    Write-Host ""
    Write-Host "All VMs have completed their configuration."

    if ($Scenario -eq 'Cluster') {
        Write-Host ""
        Write-Host "Next: Connect to Node01 via Bastion and follow the"
        Write-Host "post-deployment cluster setup steps in the README."
    }
} else {
    $message = "Timeout after $TimeoutMinutes minutes: some '$Scenario' VMs did not complete configuration. Check C:\$packageFolder\DSC\Deploy-*.log (Windows) or /opt/$packageFolder/DSC/Deploy-*.log (Linux)."
    $timeoutError = [System.Management.Automation.ErrorRecord]::new(
        [TimeoutException]::new($message),
        'DeploymentConfigurationTimeout',
        [System.Management.Automation.ErrorCategory]::OperationTimeout,
        $ResourceGroupName)
    $PSCmdlet.ThrowTerminatingError($timeoutError)
}

$testsComplete = $false
$trxCount = 0
$failedTrxCount = 0
$testClassification = ''
$failedTestCount = 0
$summaryLength = 0L
if ($WaitForTests) {
    $driver = $targets | Where-Object { $_.Role -eq 'Driver Computer' } | Select-Object -First 1
    if (-not $driver) {
        throw "Scenario '$Scenario' has no Driver Computer target for test verification."
    }

    Write-Host ""
    Write-Host "Waiting for automatic FileServer tests on $($driver.VMName)..."
    $testStart = Get-Date
    $testDeadline = $testStart.AddMinutes($TestTimeoutMinutes)
    $testPollInterval = $PollIntervalSeconds

    while (-not $testsComplete -and (Get-Date) -lt $testDeadline) {
        try {
            $remainingSeconds = [math]::Max(1, [int]($testDeadline - (Get-Date)).TotalSeconds)
            $probeTimeout = [math]::Min($ProbeTimeoutSeconds, $remainingSeconds)
            $testScript = New-TestProbeScript -LinuxTarget $driver.IsLinux `
                -NotBeforeEpoch $notBeforeEpoch `
                -StallTimeoutSeconds ($TestStallTimeoutMinutes * 60)
            $testOutput = Invoke-BoundedVmRunCommand -ResourceGroupName $ResourceGroupName `
                -VMName $driver.VMName -CommandId $driver.CommandId `
                -ScriptString $testScript -TimeoutSeconds $probeTimeout

            if ($testOutput -match 'TEST_UPLOAD_FAILED\|(.+)') {
                throw "Automatic test result upload failed on '$($driver.VMName)': $($Matches[1])"
            }
            if ($testOutput -match 'TEST_STALLED\|([^|\r\n]+)\|(\d+)') {
                $stalledMinutes = [math]::Round(([int64]$Matches[2]) / 60, 1)
                throw "Automatic test invocation stalled on '$($driver.VMName)': '$($Matches[1])' remained Started for $stalledMinutes minutes."
            }
            if ($testOutput -match 'TEST_TASK_FAILED\|([^|\r\n]+)\|([^|\r\n]+)') {
                throw "Automatic test task failed on '$($driver.VMName)': '$($Matches[1])' returned '$($Matches[2])' without final test signals."
            }
            if ($testOutput -match 'TEST_SUMMARY_MISSING') {
                throw "Automatic tests finalized on '$($driver.VMName)', but the complete test summary is missing."
            }
            if ($testOutput -match 'TEST_READY\|(\d+)\|(\d+)\|([^|\r\n]+)\|(\d+)\|(\d+)') {
                $trxCount = [int]$Matches[1]
                $failedTrxCount = [int]$Matches[2]
                $testClassification = $Matches[3]
                $failedTestCount = [int]$Matches[4]
                $summaryLength = [long]$Matches[5]
                if ($trxCount -eq 0) {
                    throw 'The test completion signal exists, but no TRX result files were found.'
                }
                $testsComplete = $true
                break
            }

            $elapsed = [math]::Round(((Get-Date) - $testStart).TotalMinutes, 1)
            Write-Host "[$elapsed min] Tests still running ($testOutput)"
            $testPollInterval = $PollIntervalSeconds
        } catch {
            if ($_.Exception.Message -like 'Automatic test result upload failed*') {
                throw
            }
            if ($_.Exception.Message -like 'Automatic test invocation stalled*') {
                throw
            }
            if ($_.Exception.Message -like 'Automatic test task failed*') {
                throw
            }
            if ($_.Exception.Message -like 'Automatic tests completed with failures*') {
                throw
            }
            Write-Warning "Test completion probe failed: $($_.Exception.Message)"
            $testPollInterval = [math]::Min([math]::Max($PollIntervalSeconds, $testPollInterval * 2), 120)
        }

        $remainingSeconds = [math]::Max(0, [int]($testDeadline - (Get-Date)).TotalSeconds)
        $sleepSeconds = [math]::Min($testPollInterval, $remainingSeconds)
        if ($sleepSeconds -gt 0) { Start-Sleep -Seconds $sleepSeconds }
    }

    if (-not $testsComplete) {
        throw "Timeout after $TestTimeoutMinutes minutes waiting for automatic tests on '$($driver.VMName)'."
    }

    $testSummaryText = $null
    if (-not [string]::IsNullOrWhiteSpace($ResultsStorageAccountName) -and
        $PSBoundParameters.ContainsKey('NotBeforeUtc')) {
        try {
            if ($SubscriptionId) {
                Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
            }
            $testSummaryText = Get-UploadedTestSummaryText -ResourceGroupName $ResourceGroupName `
                -StorageAccountName $ResultsStorageAccountName `
                -ContainerName $ResultsContainerName -Scenario $Scenario `
                -NotBeforeUtc $NotBeforeUtc -SummaryLength $summaryLength
        } catch {
            Write-Warning "Direct summary retrieval failed: $($_.Exception.Message) Falling back to bounded VM transport."
        }
    }
    if ($null -eq $testSummaryText) {
        $testSummaryText = Get-RemoteTestSummaryText -ResourceGroupName $ResourceGroupName `
            -Driver $driver -SummaryLength $summaryLength -TimeoutSeconds $ProbeTimeoutSeconds
    }
    Write-Host ""
    Write-Host "================ COMPLETE TEST SUMMARY ================"
    Write-Host $testSummaryText
    Write-Host "======================================================="

    if ($testClassification -ne 'Passed') {
        $testFailureMessage = "Automatic tests completed with failures in $failedTrxCount of $trxCount TRX files on '$($driver.VMName)'. Classification: $testClassification; failed tests: $failedTestCount. Full logs and summaries were uploaded."
        if (-not $DeferTestFailure) {
            throw $testFailureMessage
        }
        Write-Warning "$testFailureMessage Final deployment failure is deferred until post-test infrastructure handling completes."
    } else {
        Write-Host "[OK] Automatic tests completed: $trxCount TRX files, $failedTrxCount containing failed results."
    }
}

[pscustomobject]@{
    ResourceGroup          = $ResourceGroupName
    Scenario               = $Scenario
    VmConfigurationReady   = $true
    TestsComplete          = $testsComplete
    TrxFileCount           = $trxCount
    FailedTrxFileCount     = $failedTrxCount
    TestClassification     = $testClassification
    FailedTestCount        = $failedTestCount
    VerificationMinutes    = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
}
