# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path
$summaryScript = Join-Path $root 'shared\DSC\Scripts\Write-TestRunSummary.ps1'
$helpersModule = Join-Path $root 'shared\Deploy-Helpers.psm1'
Import-Module $helpersModule -Force

Describe 'Complete FileServer test-run reporting' {
    BeforeEach {
        $testRoot = Join-Path $env:TEMP "TestRunSummary-$([guid]::NewGuid().ToString('N'))"
        $results = Join-Path $testRoot 'TestResults'
        New-Item -ItemType Directory -Path $results -Force | Out-Null

        @'
<TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
  <Results>
    <UnitTestResult testName="PassingTest" outcome="Passed" duration="00:00:01" />
    <UnitTestResult testName="ConfigSensitiveFailure" outcome="Failed" duration="00:00:02">
      <Output><ErrorInfo><Message>Expected share was missing</Message><StackTrace>at Setup.Check()</StackTrace></ErrorInfo></Output>
    </UnitTestResult>
  </Results>
  <ResultSummary outcome="Failed"><Counters total="2" executed="2" passed="1" failed="1" error="0" timeout="0" aborted="0" inconclusive="0" notExecuted="0" /></ResultSummary>
</TestRun>
'@ | Set-Content (Join-Path $results 'suite-one.trx') -Encoding UTF8

        @'
<TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
  <Results><UnitTestResult testName="SecondPassingTest" outcome="Passed" duration="00:00:01" /></Results>
  <ResultSummary outcome="Completed"><Counters total="1" executed="1" passed="1" failed="0" error="0" timeout="0" aborted="0" inconclusive="0" notExecuted="0" /></ResultSummary>
</TestRun>
'@ | Set-Content (Join-Path $results 'suite-two.trx') -Encoding UTF8
    }

    AfterEach {
        Remove-Item $testRoot -Recurse -Force -ErrorAction SilentlyContinue
    }

    It 'aggregates every TRX and records every failed test with diagnostic context' {
        $jsonPath = Join-Path $testRoot 'test.summary.json'
        $textPath = Join-Path $testRoot 'test.summary.txt'
        $summary = & $summaryScript -TestResultDirectory $results -Scenario Workgroup `
            -ContextName Win2025_Workgroup_NonCluster_SMB311 -ExecutionExitCode 1 `
            -OutputJsonPath $jsonPath -OutputTextPath $textPath

        $summary.Classification | Should Be 'TestFailures'
        $summary.TrxFileCount | Should Be 2
        $summary.Counters.Total | Should Be 3
        $summary.Counters.Passed | Should Be 2
        $summary.Counters.Failed | Should Be 1
        $summary.PassedTestCount | Should Be 2
        $summary.InconclusiveTestCount | Should Be 0
        $summary.FailedTestCount | Should Be 1
        $summary.FailedTests.Count | Should Be 1
        $summary.InconclusiveTests.Count | Should Be 0
        $summary.FailedTests[0].TestName | Should Be 'ConfigSensitiveFailure'
        $summary.FailedTests[0].TrxFile | Should Be 'suite-one.trx'
        $summary.FailedTests[0].ErrorMessage | Should Match 'Expected share was missing'
        Test-Path $jsonPath | Should Be $true
        (Get-Content $textPath -Raw) | Should Match 'ConfigSensitiveFailure'
        (Get-Content $textPath -Raw) | Should Match 'Setup.Check'
    }

    It 'classifies missing TRX output as infrastructure or configuration failure' {
        $jsonPath = Join-Path $testRoot 'test.summary.json'
        $textPath = Join-Path $testRoot 'test.summary.txt'
        Remove-Item (Join-Path $results '*.trx') -Force

        $summary = & $summaryScript -TestResultDirectory $results -Scenario Workgroup `
            -ContextName test -ExecutionExitCode -1 -OutputJsonPath $jsonPath `
            -OutputTextPath $textPath

        $summary.Classification | Should Be 'InfrastructureOrConfigurationFailure'
        $summary.TrxFileCount | Should Be 0
    }

        It 'reports inconclusive and not-executed tests separately from failures' {
            $jsonPath = Join-Path $testRoot 'test.summary.json'
            $textPath = Join-Path $testRoot 'test.summary.txt'
            Remove-Item (Join-Path $results '*.trx') -Force
                @'
<TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
    <Results>
        <UnitTestResult testName="PassingTest" outcome="Passed" duration="00:00:01" />
        <UnitTestResult testName="SkippedByEnvironment" outcome="NotExecuted" duration="00:00:00">
            <Output><ErrorInfo><Message>Environment prerequisite was unavailable</Message></ErrorInfo></Output>
        </UnitTestResult>
    </Results>
    <ResultSummary outcome="Completed"><Counters total="2" executed="1" passed="1" failed="0" error="0" timeout="0" aborted="0" inconclusive="0" notExecuted="0" /></ResultSummary>
</TestRun>
'@ | Set-Content (Join-Path $results 'suite-two.trx') -Encoding UTF8

                $summary = & $summaryScript -TestResultDirectory $results -Scenario Workgroup `
                        -ContextName test -ExecutionExitCode 0 -OutputJsonPath $jsonPath `
                        -OutputTextPath $textPath

                $summary.Classification | Should Be 'Inconclusive'
                $summary.PassedTestCount | Should Be 1
                $summary.InconclusiveTestCount | Should Be 1
                $summary.FailedTestCount | Should Be 0
                @($summary.InconclusiveTests | Where-Object TestName -eq 'SkippedByEnvironment').Count |
                        Should Be 1
                (Get-Content $textPath -Raw) |
                    Should Match 'Reported outcomes: passed=1, inconclusive=1, failed=0'
        }

            It 'distinguishes a missing invocation result from assertion failures' {
                $jsonPath = Join-Path $testRoot 'test.summary.json'
                $textPath = Join-Path $testRoot 'test.summary.txt'
                @{
                    TestContainer = 'MissingSuite.dll'
                    TrxResultFileName = 'missing-suite.trx'
                    TestCaseFilter = ''
                    Status = 'Completed'
                    ExitCode = -1
                    ResultFileCreated = $false
                    StartedAtUtc = [DateTime]::UtcNow.AddMinutes(-1).ToString('o')
                    CompletedAtUtc = [DateTime]::UtcNow.ToString('o')
                } | ConvertTo-Json | Set-Content (Join-Path $results 'missing-suite.execution.json') -Encoding UTF8

                $summary = & $summaryScript -TestResultDirectory $results -Scenario Workgroup `
                    -ContextName test -ExecutionExitCode 0 -RequireExecutionManifests `
                    -OutputJsonPath $jsonPath -OutputTextPath $textPath

                $summary.Classification | Should Be 'MixedTestAndInfrastructureFailures'
                $summary.TestInvocationCount | Should Be 1
                $summary.ExecutionIssues.Count | Should Be 1
                $summary.ExecutionIssues[0].TrxResultFileName | Should Be 'missing-suite.trx'
                $summary.ExecutionIssues[0].Reason | Should Match 'MissingTrx'
                (Get-Content $textPath -Raw) | Should Match 'MissingSuite.dll'
            }

            It 'classifies malformed TRX as an infrastructure reporting failure' {
                $jsonPath = Join-Path $testRoot 'test.summary.json'
                $textPath = Join-Path $testRoot 'test.summary.txt'
                Remove-Item (Join-Path $results '*.trx') -Force
                '<TestRun><broken>' | Set-Content (Join-Path $results 'malformed.trx') -Encoding UTF8

                $summary = & $summaryScript -TestResultDirectory $results -Scenario Workgroup `
                    -ContextName test -ExecutionExitCode 0 -OutputJsonPath $jsonPath `
                    -OutputTextPath $textPath

                $summary.Classification | Should Be 'InfrastructureOrConfigurationFailure'
                $summary.FailedTests.Count | Should Be 0
                $summary.ExecutionIssues.Count | Should Be 1
                $summary.ExecutionIssues[0].Reason | Should Match 'InvalidTrx'
            }

            It 'reports a timed-out invocation as infrastructure evidence' {
                $jsonPath = Join-Path $testRoot 'test.summary.json'
                $textPath = Join-Path $testRoot 'test.summary.txt'
                @{
                    TestContainer = 'HungSuite.dll'
                    TrxResultFileName = 'hung-suite.trx'
                    TestCaseFilter = ''
                    Status = 'TimedOut'
                    ExitCode = -1
                    ResultFileCreated = $false
                    StartedAtUtc = [DateTime]::UtcNow.AddMinutes(-30).ToString('o')
                    CompletedAtUtc = [DateTime]::UtcNow.ToString('o')
                    TimeoutMinutes = 30
                } | ConvertTo-Json | Set-Content (Join-Path $results 'hung-suite.execution.json') -Encoding UTF8

                $summary = & $summaryScript -TestResultDirectory $results -Scenario Domain `
                    -ContextName test -ExecutionExitCode 0 -RequireExecutionManifests `
                    -OutputJsonPath $jsonPath -OutputTextPath $textPath

                $summary.Classification | Should Be 'MixedTestAndInfrastructureFailures'
                @($summary.ExecutionIssues | Where-Object Reason -eq 'IncompleteInvocation:TimedOut').Count |
                    Should Be 1
                (Get-Content $textPath -Raw) | Should Match 'HungSuite.dll'
                (Get-Content $textPath -Raw) | Should Match 'IncompleteInvocation:TimedOut'
            }
}

Describe 'Runner completion and diagnostic artifacts' {
    $executor = Get-Content (Join-Path $root 'shared\DSC\Scripts\Execute-TestCaseByContext.ps1') -Raw
    $vstestInvocation = Get-Content (Join-Path $root 'shared\DSC\Scripts\Invoke-VstestInvocation.ps1') -Raw
    $testRunner = Get-Content (Join-Path $root 'shared\DSC\Scripts\Invoke-TestRun.ps1') -Raw
    $verifier = Get-Content (Join-Path $root 'shared\scripts\Verify-Deployment.ps1') -Raw

    It 'writes the finish signal only after the complete execution plan' {
        $lastExecution = $executor.LastIndexOf('ExecuteTestCasesArray(')
        $finishSignal = $executor.IndexOf('Write-Output "test.finished.signal"')

        ($lastExecution -ge 0) | Should Be $true
        ($finishSignal -gt $lastExecution) | Should Be $true
    }

    It 'uses unique direct TRX paths and records every invocation without stopping later stages' {
        $vstestInvocation.Contains('/Logger:trx;LogFileName=$TrxResultFileName') | Should Be $true
        $vstestInvocation.Contains('/ResultsDirectory:$ResultDirectory') | Should Be $true
        $vstestInvocation.Contains("Status = 'Started'") | Should Be $true
        $vstestInvocation.Contains("`$manifest['Status'] = if") | Should Be $true
        $vstestInvocation.Contains('.execution.json') | Should Be $true
        $executor.Contains('RenameTrxResultFile') | Should Be $false
        $executor.Contains('throw "Test invocation failed') | Should Be $false
    }

    It 'writes and uploads summary plus Driver and SUT diagnostic logs' {
        $testRunner.Contains('Write-TestRunSummary.ps1') | Should Be $true
        $testRunner.Contains('test.summary.json') | Should Be $true
        $testRunner.Contains('test.summary.txt') | Should Be $true
        $testRunner.Contains("`$driverDiagnostics = Join-Path `$diagnosticRoot 'Driver'") | Should Be $true
        $testRunner.Contains("`$sutDiagnostics = Join-Path `$diagnosticRoot 'SUT'") | Should Be $true
        $testRunner.Contains('$diagnosticFiles') | Should Be $true
    }

    It 'retrieves the complete artifact but prints only a concise terminal summary' {
        $verifier.Contains('function Get-RemoteTestSummaryText') | Should Be $true
        $verifier.Contains('function Get-UploadedTestSummaryText') | Should Be $true
        $verifier.Contains('Get-AzStorageBlobContent') | Should Be $true
        $verifier.Contains('ResultsStorageAccountName') | Should Be $true
        $verifier.Contains('Falling back to bounded VM transport') | Should Be $true
        $verifier.Contains('TEST_SUMMARY_CHUNK') | Should Be $true
        $verifier.Contains('Compression.GZipStream') | Should Be $true
        $verifier.Contains('$compressedLength') | Should Be $true
        $verifier.Contains("Join-Path ```$summaryPath 'test.summary.txt'") | Should Be $true
        $verifier.Contains('for ($chunkAttempt = 1; $chunkAttempt -le 3; $chunkAttempt++)') | Should Be $true
        $verifier.Contains('Summary chunk transport failed at offset $offset') | Should Be $true
        $verifier.Contains('Write-Host $testSummaryText') | Should Be $false
        $verifier.Contains('================ COMPLETE TEST SUMMARY ================') |
            Should Be $false
        $verifier.Contains('Write-Host "Classification: $testClassification"') |
            Should Be $true
        $verifier.Contains('Write-Host "Passed:         $passedTestCount"') |
            Should Be $true
        $verifier.Contains('Write-Host "Inconclusive:   $inconclusiveTestCount"') |
            Should Be $true
        $verifier.Contains('Write-Host "Failed:         $failedTestCount"') |
            Should Be $true
        $verifier.Contains('Detailed per-test messages and stack traces remain in') |
            Should Be $true
        $verifier.Contains('Import-AzureModules | Out-Host') | Should Be $true
        $verifier.Contains('Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host') |
            Should Be $true
        $printIndex = $verifier.IndexOf('Write-Host "Classification: $testClassification"')
        $throwIndex = $verifier.IndexOf("Automatic tests completed on '`$(`$driver.VMName)'")
        ($printIndex -ge 0) | Should Be $true
        ($throwIndex -gt $printIndex) | Should Be $true
        $verifier.Contains('[switch]$DeferTestFailure') | Should Be $true
        $verifier.Contains('Final deployment failure is deferred until post-test infrastructure handling completes') |
            Should Be $true
    }

    It 'does not instruct automated Cluster deployments to perform manual setup' {
        $verifier.Contains('Connect to Node01 via Bastion') | Should Be $false
        $verifier.Contains('post-deployment cluster setup steps') | Should Be $false
        $verifier.Contains('Automatic FileServer test execution will now be monitored.') |
            Should Be $true
    }

    It 'reports terminal deployment outcomes in past tense for every scenario' {
        foreach ($scenario in @('workgroup-bicep', 'domain-bicep', 'cluster-bicep')) {
            $deploy = Get-Content (Join-Path $root "$scenario\deploy.ps1") -Raw
            $deploy.Contains('Automatic FileServer test execution completed.') |
                Should Be $true
            $deploy.Contains('What happens next (fully automatic):') |
                Should Be $false
        }

        $clusterDeploy = Get-Content (
            Join-Path $root 'cluster-bicep\deploy.ps1') -Raw
        $clusterDeploy.Contains('Monitor progress:') | Should Be $false
        $clusterDeploy.Contains('Tests run automatically on Client01.') |
            Should Be $false

        $workgroupDeploy = Get-Content (
            Join-Path $root 'workgroup-bicep\deploy.ps1') -Raw
        $workgroupDeploy.Contains('Monitor progress:') | Should Be $false
    }
}

InModuleScope Deploy-Helpers {
Describe 'Deployment test outcome exit semantics' {
    It 'allows a passed test run to complete deployment' {
        $verification = [pscustomobject]@{
            TestsComplete = $true
            TestClassification = 'Passed'
            PassedTestCount = 12
            InconclusiveTestCount = 0
            FailedTestCount = 0
        }

        { Complete-DeploymentTestOutcome -Verification $verification } | Should Not Throw
    }

    It 'reports protocol test failures without failing deployment orchestration' {
        $verification = [pscustomobject]@{
            TestsComplete = $true
            TestClassification = 'TestFailures'
            PassedTestCount = 20
            InconclusiveTestCount = 3
            FailedTestCount = 12
        }

        $output = @(Complete-DeploymentTestOutcome -Verification $verification 3>&1) -join "`n"
        $output | Should Match 'environment and test run completed successfully'
        $output | Should Match '20 passed, 3 inconclusive, and 12 failed'
    }

    It 'reports inconclusive tests without calling them failures' {
        $verification = [pscustomobject]@{
            TestsComplete = $true
            TestClassification = 'Inconclusive'
            PassedTestCount = 100
            InconclusiveTestCount = 12
            FailedTestCount = 0
        }

        $output = @(Complete-DeploymentTestOutcome -Verification $verification 3>&1) -join "`n"
        $output | Should Match '100 passed, 12 inconclusive, and 0 failed'
        $output | Should Match 'environment prerequisites'
    }

    It 'reports infrastructure or configuration outcomes without failing deployment orchestration' {
        $verification = [pscustomobject]@{
            TestsComplete = $true
            TestClassification = 'InfrastructureOrConfigurationFailure'
            PassedTestCount = 0
            InconclusiveTestCount = 0
            FailedTestCount = 1
        }

        $output = @(Complete-DeploymentTestOutcome -Verification $verification 3>&1) -join "`n"
        $output | Should Match 'environment orchestration completed'
        $output | Should Match 'infrastructure or configuration failures'
    }

    It 'reports mixed test and infrastructure outcomes without failing deployment orchestration' {
        $verification = [pscustomobject]@{
            TestsComplete = $true
            TestClassification = 'MixedTestAndInfrastructureFailures'
            PassedTestCount = 4
            InconclusiveTestCount = 2
            FailedTestCount = 5
        }

        $output = @(Complete-DeploymentTestOutcome -Verification $verification 3>&1) -join "`n"
        $output | Should Match 'environment orchestration completed'
        $output | Should Match 'both protocol-test and infrastructure/configuration failures'
    }

    It 'does not evaluate a test outcome when test waiting was skipped' {
        $verification = [pscustomobject]@{
            TestsComplete = $false
            TestClassification = ''
            PassedTestCount = 0
            InconclusiveTestCount = 0
            FailedTestCount = 0
        }

        { Complete-DeploymentTestOutcome -Verification $verification } | Should Not Throw
    }
}
    }
