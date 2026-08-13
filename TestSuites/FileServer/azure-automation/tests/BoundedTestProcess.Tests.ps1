# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path
$processHelper = Join-Path $root 'shared\DSC\Scripts\Invoke-BoundedProcess.ps1'

Describe 'Bounded native test process execution' {
    It 'returns output and exit code for a process that completes' {
        $pwsh = (Get-Command pwsh -ErrorAction Stop).Source
        $arguments = @('-NoProfile', '-Command', '[Console]::Out.Write("bounded-ok")')

        $result = & $processHelper -FilePath $pwsh -ArgumentList $arguments -TimeoutSeconds 10

        $result.Started | Should Be $true
        $result.TimedOut | Should Be $false
        $result.ExitCode | Should Be 0
        $result.StandardOutput | Should Be 'bounded-ok'
    }

    It 'kills a non-terminating process tree at the configured timeout' {
        $pwsh = (Get-Command pwsh -ErrorAction Stop).Source
        $arguments = @(
            '-NoProfile',
            '-Command',
            '$gate = [Threading.ManualResetEvent]::new($false); [void]$gate.WaitOne()'
        )
        $stopwatch = [Diagnostics.Stopwatch]::StartNew()

        $result = & $processHelper -FilePath $pwsh -ArgumentList $arguments -TimeoutSeconds 1
        $stopwatch.Stop()

        $result.Started | Should Be $true
        $result.TimedOut | Should Be $true
        $result.ProcessTerminated | Should Be $true
        ($stopwatch.Elapsed.TotalSeconds -lt 10) | Should Be $true
    }

    It 'drains large stdout and stderr streams without deadlocking' {
        $pwsh = (Get-Command pwsh -ErrorAction Stop).Source
        $arguments = @(
            '-NoProfile',
            '-Command',
            '$text = ''x'' * 131072; [Console]::Out.Write($text); [Console]::Error.Write($text)'
        )

        $result = & $processHelper -FilePath $pwsh -ArgumentList $arguments -TimeoutSeconds 10

        $result.TimedOut | Should Be $false
        $result.ExitCode | Should Be 0
        $result.StandardOutput.Length | Should Be 131072
        $result.StandardError.Length | Should Be 131072
    }

    It 'bounds output draining when a descendant inherits redirected handles' {
        $pwsh = (Get-Command pwsh -ErrorAction Stop).Source
        $tempRoot = Join-Path $env:TEMP "BoundedDrain-$([guid]::NewGuid().ToString('N'))"
        $childScript = Join-Path $tempRoot 'child.ps1'
        $parentScript = Join-Path $tempRoot 'parent.ps1'
        $pidFile = Join-Path $tempRoot 'child.pid'
        New-Item -ItemType Directory -Path $tempRoot -Force | Out-Null
        try {
            '[void][Threading.ManualResetEvent]::new($false).WaitOne()' |
                Set-Content -LiteralPath $childScript -Encoding UTF8
            @"
`$child = Start-Process -FilePath '$($pwsh.Replace("'", "''"))' ``
    -ArgumentList @('-NoProfile', '-File', '$($childScript.Replace("'", "''"))') ``
    -NoNewWindow -PassThru
`$child.Id | Set-Content -LiteralPath '$($pidFile.Replace("'", "''"))'
"@ | Set-Content -LiteralPath $parentScript -Encoding UTF8
            $stopwatch = [Diagnostics.Stopwatch]::StartNew()

            $result = & $processHelper -FilePath $pwsh `
                -ArgumentList @('-NoProfile', '-File', $parentScript) `
                -TimeoutSeconds 10 -OutputDrainTimeoutSeconds 1
            $stopwatch.Stop()

            $result.Started | Should Be $true
            $result.ProcessTerminated | Should Be $true
            $result.ErrorMessage | Should Match 'Redirected output did not close'
            ($stopwatch.Elapsed.TotalSeconds -lt 10) | Should Be $true
        } finally {
            if (Test-Path $pidFile) {
                Stop-Process -Id ([int](Get-Content $pidFile -Raw)) -Force -ErrorAction SilentlyContinue
            }
            Remove-Item $tempRoot -Recurse -Force -ErrorAction SilentlyContinue
        }
    }
}

Describe 'VSTest invocation watchdog integration' {
    $executor = Get-Content (Join-Path $root 'shared\DSC\Scripts\Execute-TestCaseByContext.ps1') -Raw
    $vstestInvocation = Get-Content (Join-Path $root 'shared\DSC\Scripts\Invoke-VstestInvocation.ps1') -Raw
    $testRunner = Get-Content (Join-Path $root 'shared\DSC\Scripts\Invoke-TestRun.ps1') -Raw
    $verifier = Get-Content (Join-Path $root 'shared\scripts\Verify-Deployment.ps1') -Raw

    It 'bounds every VSTest invocation and continues after timeout' {
        $executor.Contains('[int]$TestInvocationTimeoutMinutes = 60') | Should Be $true
        $executor.Contains('Invoke-VstestInvocation.ps1') | Should Be $true
        $vstestInvocation.Contains("'TimedOut'") | Should Be $true
        $vstestInvocation.Contains('Invoke-BoundedProcess.ps1') | Should Be $true
        $executor.Contains('& dotnet @vstestArgs') | Should Be $false
        $testRunner.Contains('[int]$TestInvocationTimeoutMinutes = 60') | Should Be $true
        $testRunner.Contains("TestInvocationTimeoutMinutes = `$TestInvocationTimeoutMinutes") |
            Should Be $true
    }

    It 'fails verification when an invocation stalls or the fresh test task exits early' {
        $verifier.Contains('[int]$TestStallTimeoutMinutes = 70') | Should Be $true
        $verifier.Contains('TEST_STALLED') | Should Be $true
        $verifier.Contains('`$manifest_modified" -ge ''$NotBeforeEpoch''') | Should Be $true
        $verifier.Contains('$manifestStartedEpoch -ge $NotBeforeEpoch') | Should Be $true
        $verifier.Contains('TEST_TASK_FAILED') | Should Be $true
        $verifier.Contains('Automatic test invocation stalled') | Should Be $true
        $verifier.Contains('Automatic test task failed') | Should Be $true
        $verifier.Contains("if (```$testProcesses.Count -eq 0 -and ```$taskInfo") | Should Be $true
    }
}

Describe 'Credentialed test process launcher' {
    $launcherPath = Join-Path $root 'shared\DSC\Scripts\Invoke-ProcessAsUser.ps1'
    $launcher = Get-Content $launcherPath -Raw

    It 'loads the user profile and environment before native process creation' {
        $launcher.Contains('LoadUserProfile') | Should Be $true
        $launcher.Contains('CreateEnvironmentBlock') | Should Be $true
        $launcher.Contains('CreateProcessWithTokenW') | Should Be $true
        $launcher.Contains('DestroyEnvironmentBlock') | Should Be $true
        $launcher.Contains('UnloadUserProfile') | Should Be $true
        $launcher.Contains('CloseHandle') | Should Be $true
        $launcher.Contains('[switch]$KeepProfileLoaded') | Should Be $true
        $launcher.Contains('$profileLoaded -and -not $KeepProfileLoaded') | Should Be $true
        (Get-Content (Join-Path $root 'shared\DSC\Deploy-Driver.ps1') -Raw).Contains('-KeepProfileLoaded') |
            Should Be $true
    }

    It 'keeps the profile loaded through bounded synchronous execution and terminates on timeout' {
        $launcher.Contains('[switch]$WaitForExit') | Should Be $true
        $launcher.Contains('[int]$TimeoutSeconds = 600') | Should Be $true
        $launcher.Contains('WaitForSingleObject') | Should Be $true
        $launcher.Contains('TerminateProcess') | Should Be $true
        $launcher.Contains('GetExitCodeProcess') | Should Be $true
        $launcher.IndexOf('WaitForSingleObject') | Should BeLessThan $launcher.LastIndexOf('UnloadUserProfile')
    }

    It 'quotes paths and arguments through a dedicated command-line encoder' {
        $launcher.Contains('function ConvertTo-WindowsCommandLineArgument') | Should Be $true
        $launcher.Contains('$commandLineParts = @((ConvertTo-WindowsCommandLineArgument $FilePath))') |
            Should Be $true
        $launcher.Contains('$ArgumentList | ForEach-Object { ConvertTo-WindowsCommandLineArgument $_ }') |
            Should Be $true
    }
}

Describe 'Windows SMB credential connection' {
    $connector = Get-Content (Join-Path $root 'shared\DSC\Scripts\Connect-WindowsSmbShare.ps1') -Raw
    $testRun = Get-Content (Join-Path $root 'shared\DSC\Scripts\Invoke-TestRun.ps1') -Raw

    It 'does not accept a conflicting pre-existing credential session' {
        foreach ($implementation in @($connector, $testRun)) {
            $implementation.Contains('$result -eq 1219') | Should Be $true
            $implementation.Contains('requested credential session was not established') |
                Should Be $true
            $implementation.Contains('$result -notin @(0, 1219)') | Should Be $false
        }
    }
}
