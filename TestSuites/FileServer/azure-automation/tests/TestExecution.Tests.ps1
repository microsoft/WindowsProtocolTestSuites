# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

Describe 'FileServer test execution' {
    It 'defaults generated deployment configuration to automatic test execution' {
        $generator = Join-Path $root 'shared\Generate-ConfigJson.ps1'
        $defaultPath = Join-Path $TestDrive 'default-config.json'
        $disabledPath = Join-Path $TestDrive 'disabled-config.json'

        & $generator -Scenario Workgroup -AdminUsername testadmin `
            -AdminPassword 'Password04!' -OutputPath $defaultPath | Out-Null
        & $generator -Scenario Workgroup -AdminUsername testadmin `
            -AdminPassword 'Password04!' -EnableTestAutoRun $false `
            -OutputPath $disabledPath | Out-Null

        (Get-Content $defaultPath -Raw | ConvertFrom-Json).TestExecution.AutoRun |
            Should Be $true
        (Get-Content $disabledPath -Raw | ConvertFrom-Json).TestExecution.AutoRun |
            Should Be $false
    }

    It 'patches the persisted test autorun setting idempotently' {
        $patcher = Join-Path $root 'shared\DSC\Scripts\Set-ConfigTestExecution.ps1'
        $configPath = Join-Path $TestDrive 'patch-config.json'
        '{"Core":{"Scenario":"Workgroup"}}' |
            Set-Content -LiteralPath $configPath -Encoding UTF8 -NoNewline

        & $patcher -EnableTestAutoRun false -ConfigPaths $configPath | Out-Null
        (Get-Content $configPath -Raw | ConvertFrom-Json).TestExecution.AutoRun |
            Should Be $false

        & $patcher -EnableTestAutoRun true -ConfigPaths $configPath | Out-Null
        (Get-Content $configPath -Raw | ConvertFrom-Json).TestExecution.AutoRun |
            Should Be $true
    }

    It 'propagates the default-enabled autorun setting through every deployment path' {
        foreach ($relativePath in @(
            'workgroup-bicep\main.bicep',
            'domain-bicep\main.bicep',
            'domain-bicep\phase2.bicep',
            'cluster-bicep\main.bicep',
            'cluster-bicep\phase2.bicep'
        )) {
            (Get-Content (Join-Path $root $relativePath) -Raw).Contains(
                'param enableTestAutoRun bool = true') | Should Be $true
        }

        foreach ($relativePath in @(
            'workgroup-bicep\parameters\workgroup.bicepparam',
            'domain-bicep\parameters\phase2.bicepparam',
            'cluster-bicep\parameters\phase2.bicepparam'
        )) {
            (Get-Content (Join-Path $root $relativePath) -Raw).Contains(
                'param enableTestAutoRun = true') | Should Be $true
        }

        $sharedDriver = Get-Content (
            Join-Path $root 'shared\DSC\Deploy-Driver.ps1') -Raw
        $clusterDriver = Get-Content (
            Join-Path $root 'cluster-bicep\DSC\Deploy-ClusterDriver.ps1') -Raw
        $sharedDriver.Contains('if (-not $testAutoRun)') | Should Be $true
        $clusterDriver.Contains('if ($testAutoRun)') | Should Be $true
        $clusterDriver.Contains('-SkipTestTaskCheck:(-not $testAutoRun)') |
            Should Be $true
    }

    It 'discovers net8 test cases through the dotnet test runner' {
        $scriptPath = Join-Path $root 'shared\DSC\Scripts\Execute-TestCaseByContext.ps1'
        $content = Get-Content -LiteralPath $scriptPath -Raw

        $content.Contains("-ArgumentList @('vstest', `$resolvedDll, '/ListTests')") |
            Should Be $true
        $content.Contains('-TimeoutSeconds 300') | Should Be $true
        $content.Contains('The following Tests are available:') | Should Be $true
        $content.Contains('"Name=$_"') | Should Be $true
        $content.Contains('"FullyQualifiedName=$_"') | Should Be $false
        $content.Contains('[System.Reflection.Assembly]::LoadFrom') | Should Be $false
        $content.Contains('$asm.GetTypes()') | Should Be $false
    }

    It 'surfaces discovery failures instead of returning an empty test list' {
        $scriptPath = Join-Path $root 'shared\DSC\Scripts\Execute-TestCaseByContext.ps1'
        $content = Get-Content -LiteralPath $scriptPath -Raw

        $content.Contains('Test discovery failed for') | Should Be $true
        $content.Contains('Test discovery timed out for') | Should Be $true
        $content.Contains('Test discovery returned no recognizable test list') |
            Should Be $true
        $content.Contains('Test discovery returned no test cases') | Should Be $true
    }

    It 'passes test filters through temporary runsettings files' {
        $scriptPath = Join-Path $root 'shared\DSC\Scripts\Invoke-VstestInvocation.ps1'
        $content = Get-Content -LiteralPath $scriptPath -Raw

        $content.Contains('[Security.SecurityElement]::Escape($TestCaseFilter)') |
            Should Be $true
        $content.Contains('<TestCaseFilter>$escapedFilter</TestCaseFilter>') |
            Should Be $true
        $content.Contains('$arguments += "/Settings:$runSettingsPath"') |
            Should Be $true
        $content.Contains(
            'Remove-Item -LiteralPath $runSettingsPath -Force') |
            Should Be $true
        $content.Contains('$arguments += "/TestCaseFilter:$TestCaseFilter"') |
            Should Be $false
    }

    It 'summarizes TRX outcomes instead of streaming exception-shaped skip output' {
        $scriptPath = Join-Path $root 'shared\DSC\Scripts\Invoke-VstestInvocation.ps1'
        $content = Get-Content -LiteralPath $scriptPath -Raw

        $content.Contains("'.console.log'") | Should Be $true
        $content.Contains('$counters.inconclusive + [int]$counters.notExecuted') |
            Should Be $true
        $content.Contains('skipped/inconclusive=$skipped') | Should Be $true
        $content.Contains('$processResult.StandardOutput') | Should Be $true
        $content.Contains('StandardOutput = $shellOutput -join') | Should Be $true
        $content.Contains("StandardError = ''") | Should Be $true
    }

    It 'temporarily trusts only the Workgroup SUT for WinRM platform detection' {
        $scriptPath = Join-Path $root 'shared\DSC\Scripts\Invoke-TestRun.ps1'
        $content = Get-Content -LiteralPath $scriptPath -Raw

        $capture = $content.IndexOf('$originalTrustedHosts =')
        $addHost = $content.IndexOf('$trustedHosts + $SutComputerName', $capture)
        $newSession = $content.IndexOf(
            'New-CimSession -ComputerName $SutComputerName',
            $addHost)
        $restore = $content.IndexOf(
            'Set-Item -Path $trustedHostsPath -Value $originalTrustedHosts',
            $newSession)

        $content.Contains("'WSMan:\localhost\Client\TrustedHosts'") | Should Be $true
        ($capture -ge 0) | Should Be $true
        ($addHost -gt $capture) | Should Be $true
        ($newSession -gt $addHost) | Should Be $true
        ($restore -gt $newSession) | Should Be $true
        $content.Contains(
            'Could not restore WinRM TrustedHosts after SUT platform detection') |
            Should Be $true
        $content.Contains("Set-Item -Path `$trustedHostsPath -Value '*'") |
            Should Be $false
    }
}
