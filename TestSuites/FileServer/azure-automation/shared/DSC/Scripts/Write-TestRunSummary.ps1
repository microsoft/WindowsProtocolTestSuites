# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$TestResultDirectory,

    [Parameter(Mandatory)]
    [string]$Scenario,

    [Parameter(Mandatory)]
    [string]$ContextName,

    [int]$ExecutionExitCode = 0,

    [bool]$ExecutionPlanCompleted = $true,

    [switch]$RequireExecutionManifests,

    [Parameter(Mandatory)]
    [string]$OutputJsonPath,

    [Parameter(Mandatory)]
    [string]$OutputTextPath
)

$trxFiles = @(Get-ChildItem -LiteralPath $TestResultDirectory -Filter '*.trx' -File -Recurse -ErrorAction SilentlyContinue)
$counters = [ordered]@{
    Total = 0
    Executed = 0
    Passed = 0
    Failed = 0
    Error = 0
    Timeout = 0
    Aborted = 0
    Inconclusive = 0
    NotExecuted = 0
}
$failedTests = [System.Collections.Generic.List[object]]::new()
$inconclusiveTests = [System.Collections.Generic.List[object]]::new()
$passedResultCount = 0
$executionIssues = [System.Collections.Generic.List[object]]::new()
$nonPassingTrxFiles = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
$configurationPattern = '(?i)config|environment|share|path|directory|file not found|network|connection|connect|DNS|domain|credential|logon|authentication|access denied|service|timeout|unreachable|not operational|prerequisite'

foreach ($trxFile in $trxFiles) {
    try {
        [xml]$document = Get-Content -LiteralPath $trxFile.FullName -Raw -ErrorAction Stop
        $namespace = [System.Xml.XmlNamespaceManager]::new($document.NameTable)
        $namespace.AddNamespace('t', 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010')

        $counterNode = $document.SelectSingleNode('//t:ResultSummary/t:Counters', $namespace)
        if ($counterNode) {
            foreach ($mapping in @{
                Total = 'total'; Executed = 'executed'; Passed = 'passed'; Failed = 'failed'
                Error = 'error'; Timeout = 'timeout'; Aborted = 'aborted'
                Inconclusive = 'inconclusive'; NotExecuted = 'notExecuted'
            }.GetEnumerator()) {
                $value = $counterNode.GetAttribute($mapping.Value)
                if ($value -match '^\d+$') { $counters[$mapping.Key] += [int]$value }
            }
            $trxTotal = [int]$counterNode.GetAttribute('total')
            $trxPassed = [int]$counterNode.GetAttribute('passed')
            if ($trxTotal -ne $trxPassed) {
                [void]$nonPassingTrxFiles.Add($trxFile.Name)
            }
        }

        foreach ($result in @($document.SelectNodes('//t:UnitTestResult[not(@outcome="Passed")]', $namespace))) {
            [void]$nonPassingTrxFiles.Add($trxFile.Name)
            $message = "$($result.Output.ErrorInfo.Message)".Trim()
            $failureStackTrace = "$($result.Output.ErrorInfo.StackTrace)".Trim()
            $diagnosticHint = if ("$message $failureStackTrace" -match $configurationPattern) {
                'PotentialConfigurationOrEnvironmentIssue'
            } else {
                'TestBehaviorFailure'
            }
            $testResult = [pscustomobject]@{
                TrxFile = $trxFile.Name
                TestName = "$($result.testName)"
                Outcome = "$($result.outcome)"
                Duration = "$($result.duration)"
                DiagnosticHint = $diagnosticHint
                ErrorMessage = $message
                StackTrace = $failureStackTrace
            }
            if ($testResult.Outcome -in @('Inconclusive', 'NotExecuted')) {
                $inconclusiveTests.Add($testResult)
            } else {
                $failedTests.Add($testResult)
            }
        }
        $passedResultCount += @($document.SelectNodes('//t:UnitTestResult[@outcome="Passed"]', $namespace)).Count
    } catch {
        $executionIssues.Add([pscustomobject]@{
            ManifestFile = ''
            TestContainer = ''
            TrxResultFileName = $trxFile.Name
            ExitCode = $null
            Reason = "InvalidTrx:$($_.Exception.Message)"
        })
    }
}

$executionManifests = @(Get-ChildItem -LiteralPath $TestResultDirectory -Filter '*.execution.json' -File -ErrorAction SilentlyContinue)
foreach ($manifestFile in $executionManifests) {
    try {
        $manifest = Get-Content -LiteralPath $manifestFile.FullName -Raw -ErrorAction Stop | ConvertFrom-Json -ErrorAction Stop
        $resultPath = Join-Path $TestResultDirectory "$($manifest.TrxResultFileName)"
        $reason = if ($manifest.Status -ne 'Completed') {
            "IncompleteInvocation:$($manifest.Status)"
        } elseif (-not (Test-Path -LiteralPath $resultPath)) {
            'MissingTrx'
        } elseif ([int]$manifest.ExitCode -ne 0 -and -not $nonPassingTrxFiles.Contains("$($manifest.TrxResultFileName)")) {
            "NonZeroExitWithoutFailedResult:$($manifest.ExitCode)"
        } else { $null }

        if ($reason) {
            $executionIssues.Add([pscustomobject]@{
                ManifestFile = $manifestFile.Name
                TestContainer = "$($manifest.TestContainer)"
                TrxResultFileName = "$($manifest.TrxResultFileName)"
                ExitCode = $manifest.ExitCode
                Reason = $reason
            })
        }
    } catch {
        $executionIssues.Add([pscustomobject]@{
            ManifestFile = $manifestFile.Name
            TestContainer = ''
            TrxResultFileName = ''
            ExitCode = $null
            Reason = "InvalidManifest:$($_.Exception.Message)"
        })
    }
}
if ($RequireExecutionManifests -and $executionManifests.Count -eq 0) {
    $executionIssues.Add([pscustomobject]@{
        ManifestFile = ''
        TestContainer = ''
        TrxResultFileName = ''
        ExitCode = $null
        Reason = 'NoExecutionManifests'
    })
}

$passedTestCount = [math]::Max($passedResultCount, $counters.Passed)
$inconclusiveTestCount = [math]::Max(
    $inconclusiveTests.Count,
    $counters.Inconclusive + $counters.NotExecuted)
$failedTestCount = [math]::Max(
    $failedTests.Count,
    $counters.Failed + $counters.Error + $counters.Timeout + $counters.Aborted)
$hasTestFailures = $failedTests.Count -gt 0 -or $failedTestCount -gt 0
$hasInconclusiveTests = $inconclusiveTests.Count -gt 0 -or $inconclusiveTestCount -gt 0
$hasInfrastructureFailures = -not $ExecutionPlanCompleted -or $trxFiles.Count -eq 0 -or $executionIssues.Count -gt 0
$classification = if ($hasTestFailures -and $hasInfrastructureFailures) {
    'MixedTestAndInfrastructureFailures'
} elseif ($hasInfrastructureFailures) {
    'InfrastructureOrConfigurationFailure'
} elseif ($hasTestFailures) {
    'TestFailures'
} elseif ($hasInconclusiveTests) {
    'Inconclusive'
} elseif ($ExecutionExitCode -ne 0) {
    'InfrastructureOrConfigurationFailure'
} else {
    'Passed'
}

$summary = [pscustomobject]@{
    GeneratedAtUtc = [DateTime]::UtcNow.ToString('o')
    Scenario = $Scenario
    ContextName = $ContextName
    Classification = $classification
    ExecutionExitCode = $ExecutionExitCode
    ExecutionPlanCompleted = $ExecutionPlanCompleted
    TrxFileCount = $trxFiles.Count
    TestInvocationCount = $executionManifests.Count
    Counters = [pscustomobject]$counters
    PassedTestCount = $passedTestCount
    InconclusiveTestCount = $inconclusiveTestCount
    FailedTestCount = $failedTestCount
    PotentialConfigurationOrEnvironmentFailures = @($failedTests | Where-Object DiagnosticHint -eq 'PotentialConfigurationOrEnvironmentIssue').Count
    FailedTests = @($failedTests)
    InconclusiveTests = @($inconclusiveTests)
    ExecutionIssues = @($executionIssues)
}

$outputDirectory = Split-Path -Parent $OutputJsonPath
if ($outputDirectory) { New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null }
$summary | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $OutputJsonPath -Encoding UTF8

$lines = [System.Collections.Generic.List[string]]::new()
$lines.Add('FileServer Test Run Summary')
$lines.Add("Generated UTC: $($summary.GeneratedAtUtc)")
$lines.Add("Scenario: $Scenario")
$lines.Add("Context: $ContextName")
$lines.Add("Classification: $classification")
$lines.Add("Execution exit code: $ExecutionExitCode")
$lines.Add("Execution plan completed: $ExecutionPlanCompleted")
$lines.Add("TRX files: $($trxFiles.Count)")
$lines.Add("Test invocations: $($executionManifests.Count)")
$lines.Add("Counters: total=$($counters.Total), executed=$($counters.Executed), passed=$($counters.Passed), failed=$($counters.Failed), error=$($counters.Error), timeout=$($counters.Timeout), aborted=$($counters.Aborted), inconclusive=$($counters.Inconclusive), notExecuted=$($counters.NotExecuted)")
$lines.Add("Reported outcomes: passed=$passedTestCount, inconclusive=$inconclusiveTestCount, failed=$failedTestCount")
$lines.Add("Potential configuration/environment failures: $($summary.PotentialConfigurationOrEnvironmentFailures)")
$lines.Add('')
if ($executionIssues.Count -gt 0) {
    $lines.Add("Execution issues ($($executionIssues.Count)):")
    foreach ($issue in $executionIssues) {
        $lines.Add("- $($issue.TestContainer) -> $($issue.TrxResultFileName): $($issue.Reason) (exit $($issue.ExitCode))")
    }
    $lines.Add('')
}
if ($failedTests.Count -eq 0) {
    $lines.Add('No failed tests were reported.')
} else {
    $lines.Add("Failed tests ($($failedTests.Count)):")
    foreach ($failure in $failedTests) {
        $lines.Add('')
        $lines.Add("[$($failure.Outcome)] $($failure.TestName)")
        $lines.Add("TRX: $($failure.TrxFile)")
        $lines.Add("Diagnostic hint: $($failure.DiagnosticHint)")
        $lines.Add("Message: $($failure.ErrorMessage)")
        if ($failure.StackTrace) { $lines.Add("Stack: $($failure.StackTrace)") }
    }
}
if ($inconclusiveTests.Count -eq 0) {
    $lines.Add('No inconclusive tests were reported.')
} else {
    $lines.Add('')
    $lines.Add("Inconclusive tests ($($inconclusiveTests.Count)):")
    foreach ($test in $inconclusiveTests) {
        $lines.Add('')
        $lines.Add("[$($test.Outcome)] $($test.TestName)")
        $lines.Add("TRX: $($test.TrxFile)")
        $lines.Add("Diagnostic hint: $($test.DiagnosticHint)")
        $lines.Add("Message: $($test.ErrorMessage)")
        if ($test.StackTrace) { $lines.Add("Stack: $($test.StackTrace)") }
    }
}
$lines | Set-Content -LiteralPath $OutputTextPath -Encoding UTF8

return $summary
