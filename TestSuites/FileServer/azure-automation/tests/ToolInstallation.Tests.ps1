# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$installer = Join-Path $here '..\shared\DSC\Scripts\InstallMSIAndTools.ps1'
$downloadScript = Join-Path $here '..\shared\DSC\Scripts\Get-RemoteFile.ps1'

Describe 'Tool installation completion' {
    BeforeEach {
        $serverJob = $null
        $testRoot = Join-Path $env:TEMP "ToolInstallation-$([guid]::NewGuid().ToString('N'))"
        $toolsPath = Join-Path $testRoot 'Tools'
        $configureFile = Join-Path $testRoot 'Tools.json'
        $signalFile = Join-Path $testRoot 'InstallMSIAndTools.Completed.signal'
        $preparedSignalFile = Join-Path $testRoot 'InstallMSIAndTools.Prepared.signal'
        $targetFolder = Join-Path $testRoot 'Installed'
        New-Item -ItemType Directory -Path $toolsPath -Force | Out-Null
    }

    AfterEach {
        if ($null -ne $serverJob) {
            Stop-Job -Job $serverJob -ErrorAction SilentlyContinue
            Remove-Job -Job $serverJob -Force -ErrorAction SilentlyContinue
        }
        Remove-Item $testRoot -Recurse -Force -ErrorAction SilentlyContinue
    }

    function Write-TestConfiguration {
        param($Item)
        @{
            DriverComputer = @{
                Tools = @($Item)
                TestsuiteZips = @()
            }
        } | ConvertTo-Json -Depth 8 | Set-Content -Path $configureFile -Encoding UTF8
    }

    function New-TestZip {
        param(
            [string]$Name = 'package.zip',
            [string]$InstallScript
        )

        $source = Join-Path $testRoot "Source-$([guid]::NewGuid().ToString('N'))"
        New-Item -ItemType Directory -Path $source -Force | Out-Null
        if ($null -ne $InstallScript) {
            $InstallScript | Set-Content -Path (Join-Path $source 'install.ps1')
        } else {
            'payload' | Set-Content -Path (Join-Path $source 'payload.txt')
        }
        $zip = Join-Path $toolsPath $Name
        Compress-Archive -Path (Join-Path $source '*') -DestinationPath $zip
        Remove-Item $source -Recurse -Force
        return $zip
    }

    function Invoke-TestInstaller {
        param(
            [string]$ConnectivityMode = 'Offline',
            [int]$InstallTimeoutSeconds = 30,
            [string]$Operation = 'All',
            [switch]$AllowRebootRequired
        )

        Push-Location (Split-Path $installer -Parent)
        try {
            return & $installer -Role DriverComputer -ToolsPath $toolsPath `
                -ConfigureFile $configureFile -SignalFile $signalFile `
                -PreparedSignalFile $preparedSignalFile -Operation $Operation `
                -LogDirectory $testRoot `
                -ConnectivityMode $ConnectivityMode `
                -InstallTimeoutSeconds $InstallTimeoutSeconds `
                -AllowRebootRequired:$AllowRebootRequired -NoTranscript
        } finally {
            Pop-Location
        }
    }

    It 'installs a baked package and treats an omitted Required flag as required' {
        New-TestZip | Out-Null
        Write-TestConfiguration @{
            name = 'BakedTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
        }

        $result = Invoke-TestInstaller

        $result | Should Be $true
        Test-Path $signalFile | Should Be $true
    }

    It 'skips package application when configured installation evidence already exists' {
        $existingPath = Join-Path $targetFolder 'existing.exe'
        New-Item -ItemType Directory -Path $targetFolder -Force | Out-Null
        'installed' | Set-Content -LiteralPath $existingPath
        Write-TestConfiguration @{
            name = 'ExistingTool'
            ZipName = 'missing.zip'
            targetFolder = $targetFolder
            ExistingInstallPaths = @($existingPath)
            ExistingServiceNames = @('EventLog')
        }

        $result = Invoke-TestInstaller

        $result | Should Be $true
        Test-Path $signalFile | Should Be $true
        Test-Path (Join-Path $toolsPath 'missing.zip') | Should Be $false
    }

    It 'rejects a baked package whose SHA-256 does not match' {
        New-TestZip | Out-Null
        Write-TestConfiguration @{
            name = 'HashMismatch'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            SHA256 = ('0' * 64)
        }

        $result = Invoke-TestInstaller

        $result | Should Be $false
        Test-Path $signalFile | Should Be $false
        Test-Path (Join-Path $toolsPath 'package.zip') | Should Be $false
    }

    It 'rejects a ZIP missing a configured expected entry' {
        $packagePath = New-TestZip
        $hash = (Get-FileHash -LiteralPath $packagePath -Algorithm SHA256).Hash.ToLowerInvariant()
        Write-TestConfiguration @{
            name = 'MissingEntry'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            SHA256 = $hash
            ExpectedEntries = @('required-file.txt')
        }

        $result = Invoke-TestInstaller

        $result | Should Be $false
        Test-Path $signalFile | Should Be $false
        Test-Path $packagePath | Should Be $false
    }

    It 'rejects an invalid baked package during preparation' {
        New-TestZip | Out-Null
        Write-TestConfiguration @{
            name = 'PrepareHashMismatch'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            SHA256 = ('f' * 64)
        }

        $result = Invoke-TestInstaller -Operation Prepare

        $result | Should Be $false
        Test-Path $preparedSignalFile | Should Be $false
        Test-Path (Join-Path $toolsPath 'package.zip') | Should Be $false
    }

    It 'fails when a required package is unavailable offline and removes a stale signal' {
        'stale' | Set-Content -Path $signalFile
        Write-TestConfiguration @{
            name = 'RequiredTool'
            ZipName = 'missing.zip'
            targetFolder = $targetFolder
        }

        $result = Invoke-TestInstaller

        $result | Should Be $false
        Test-Path $signalFile | Should Be $false
    }

    It 'does not fail completion when an explicitly optional package is unavailable' {
        Write-TestConfiguration @{
            name = 'OptionalTool'
            ZipName = 'missing.zip'
            targetFolder = $targetFolder
            Required = $false
        }

        $result = Invoke-TestInstaller

        $result | Should Be $true
        Test-Path $signalFile | Should Be $true
    }

    It 'fails when an online download does not produce the configured package' {
        Write-TestConfiguration @{
            name = 'DownloadTool'
            ZipName = 'missing.zip'
            targetFolder = $targetFolder
            Url = 'not-a-valid-url'
        }

        $result = Invoke-TestInstaller -ConnectivityMode Online

        $result | Should Be $false
        Test-Path $signalFile | Should Be $false
    }

    It 'follows an HTTP 308 redirect with the HttpClient fallback' {
        $packagePath = New-TestZip
        $packageBytes = [System.IO.File]::ReadAllBytes($packagePath)
        Remove-Item -LiteralPath $packagePath -Force

        $portProbe = New-Object System.Net.Sockets.TcpListener(
            [System.Net.IPAddress]::Loopback,
            0
        )
        $portProbe.Start()
        $port = ([System.Net.IPEndPoint]$portProbe.LocalEndpoint).Port
        $portProbe.Stop()

        $serverJob = Start-Job -ScriptBlock {
            param($Port, $PackageBytes)

            $listener = New-Object System.Net.Sockets.TcpListener(
                [System.Net.IPAddress]::Loopback,
                $Port
            )
            $listener.Start()
            'READY'
            try {
                foreach ($requestNumber in 1..2) {
                    $connection = $listener.AcceptTcpClient()
                    try {
                        $stream = $connection.GetStream()
                        $reader = New-Object System.IO.StreamReader(
                            $stream,
                            [System.Text.Encoding]::ASCII,
                            $false,
                            1024,
                            $true
                        )
                        $requestLine = $reader.ReadLine()
                        while ($reader.ReadLine()) { }
                        $path = ($requestLine -split ' ')[1]

                        if ($path -eq '/redirect') {
                            $header = "HTTP/1.1 308 Permanent Redirect`r`n" +
                                "Location: /package.zip`r`nContent-Length: 0`r`n" +
                                "Connection: close`r`n`r`n"
                            $headerBytes = [System.Text.Encoding]::ASCII.GetBytes($header)
                            $stream.Write($headerBytes, 0, $headerBytes.Length)
                        } elseif ($path -eq '/package.zip') {
                            $header = "HTTP/1.1 200 OK`r`nContent-Type: application/zip`r`n" +
                                "Content-Length: $($PackageBytes.Length)`r`nConnection: close`r`n`r`n"
                            $headerBytes = [System.Text.Encoding]::ASCII.GetBytes($header)
                            $stream.Write($headerBytes, 0, $headerBytes.Length)
                            $stream.Write($PackageBytes, 0, $PackageBytes.Length)
                        } else {
                            throw "Unexpected request path: $path"
                        }
                        $stream.Flush()
                    } finally {
                        $connection.Dispose()
                    }
                }
            } finally {
                $listener.Stop()
            }
        } -ArgumentList $port, $packageBytes

        $readyDeadline = (Get-Date).AddSeconds(10)
        do {
            $serverOutput = @(Receive-Job -Job $serverJob -Keep)
            if ($serverOutput -contains 'READY') { break }
            Start-Sleep -Milliseconds 100
        } while ((Get-Date) -lt $readyDeadline)
        ($serverOutput -contains 'READY') | Should Be $true

        $downloadPath = Join-Path $toolsPath 'redirected-package.zip'
        Push-Location (Split-Path $downloadScript -Parent)
        try {
            . $downloadScript
            $result = Get-RemoteFileViaHttpClient `
                -Url "http://127.0.0.1:$port/redirect" `
                -OutputPath $downloadPath
        } finally {
            Pop-Location
        }

        $result | Should Be $true
        Test-Path -LiteralPath $downloadPath -PathType Leaf | Should Be $true
        (Get-Item -LiteralPath $downloadPath).Length | Should BeGreaterThan 0
        { Expand-Archive -LiteralPath $downloadPath -DestinationPath $targetFolder -Force } |
            Should Not Throw
        Wait-Job -Job $serverJob -Timeout 10 | Out-Null
        $serverJob.State | Should Be 'Completed'
    }

    It 'rejects HTTPS to HTTP redirect downgrades' {
        $content = Get-Content -LiteralPath $downloadScript -Raw

        $content.Contains("`$requireHttps = `$currentUri.Scheme -eq 'https'") | Should Be $true
        $content.Contains("if (`$requireHttps -and `$nextUri.Scheme -ne 'https')") |
            Should Be $true
        $content.Contains('Refusing to follow HTTPS to HTTP redirect') | Should Be $true
    }

    It 'fails when a required ZIP install script returns a failure exit code' {
        New-TestZip -InstallScript 'exit 42' | Out-Null
        Write-TestConfiguration @{
            name = 'FailingTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            installScript = 'install.ps1'
        }

        $result = Invoke-TestInstaller

        $result | Should Be $false
        Test-Path $signalFile | Should Be $false
    }

    It 'preserves reboot-required success for legacy all-operation callers' {
        New-TestZip -InstallScript 'exit 3010' | Out-Null
        Write-TestConfiguration @{
            name = 'RebootingTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            installScript = 'install.ps1'
        }

        $result = Invoke-TestInstaller

        $result | Should Be $true
        Test-Path $signalFile | Should Be $true
    }

    It 'rejects a reboot-required exit code during install-only convergence' {
        New-TestZip -InstallScript 'exit 3010' | Out-Null
        Write-TestConfiguration @{
            name = 'RebootingTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            installScript = 'install.ps1'
        }

        Invoke-TestInstaller -Operation Prepare | Should Be $true
        Invoke-TestInstaller -Operation Install | Should Be $false

        Test-Path $signalFile | Should Be $false
    }

    It 'accepts a reboot-required exit code during an explicitly pre-reboot install' {
        New-TestZip -InstallScript 'exit 3010' | Out-Null
        Write-TestConfiguration @{
            name = 'PreRebootTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            installScript = 'install.ps1'
        }

        Invoke-TestInstaller -Operation Prepare | Should Be $true
        Invoke-TestInstaller -Operation Install -AllowRebootRequired | Should Be $true

        Test-Path $signalFile | Should Be $true
    }

    It 'suppresses installer-initiated restarts during explicitly pre-reboot installation' {
        $content = Get-Content -LiteralPath $installer -Raw

        $content.Contains('if ($ExitCode -eq 1641)') | Should Be $true
        $content.Contains('function Add-NoRestartArgument') | Should Be $true
        $content.Contains('$script:Operation -eq ''Install'' -and $ExitCode -eq 3010') |
            Should Be $true
        ([regex]::Matches(
            $content,
            '\$arguments = Add-NoRestartArgument -Arguments \$arguments'
        ).Count) | Should Be 2
    }

    It 'rejects an installer-initiated reboot even during an explicitly pre-reboot install' {
        New-TestZip -InstallScript 'exit 1641' | Out-Null
        Write-TestConfiguration @{
            name = 'SelfRebootingTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            installScript = 'install.ps1'
        }

        Invoke-TestInstaller -Operation Prepare | Should Be $true
        Invoke-TestInstaller -Operation Install -AllowRebootRequired | Should Be $false

        Test-Path $signalFile | Should Be $false
    }

    It 'accepts the Windows Update already-installed success code' {
        New-TestZip -InstallScript 'exit 2359302' | Out-Null
        Write-TestConfiguration @{
            name = 'AlreadyInstalledUpdate'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            installScript = 'install.ps1'
        }

        $result = Invoke-TestInstaller

        $result | Should Be $true
        Test-Path $signalFile | Should Be $true
    }

    It 'fails a required install script that exceeds its timeout' {
        New-TestZip -InstallScript "Start-Sleep -Seconds 5`nexit 0" | Out-Null
        Write-TestConfiguration @{
            name = 'HangingTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
            installScript = 'install.ps1'
        }

        $result = Invoke-TestInstaller -InstallTimeoutSeconds 1

        $result | Should Be $false
        Test-Path $signalFile | Should Be $false
    }

    It 'prepares baked packages without installing them' {
        New-TestZip | Out-Null
        Write-TestConfiguration @{
            name = 'PreparedTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
        }

        $result = Invoke-TestInstaller -Operation Prepare

        $result | Should Be $true
        Test-Path $preparedSignalFile | Should Be $true
        Test-Path $targetFolder | Should Be $false
        Test-Path $signalFile | Should Be $false
    }

    It 'installs only from a completed preparation phase' {
        New-TestZip | Out-Null
        Write-TestConfiguration @{
            name = 'PreparedTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
        }

        Invoke-TestInstaller -Operation Prepare | Should Be $true
        Invoke-TestInstaller -Operation Install | Should Be $true

        Test-Path $targetFolder | Should Be $true
        Test-Path $signalFile | Should Be $true
    }

    It 'rejects install-only execution without a preparation signal' {
        New-TestZip | Out-Null
        Write-TestConfiguration @{
            name = 'UnpreparedTool'
            ZipName = 'package.zip'
            targetFolder = $targetFolder
        }

        $result = Invoke-TestInstaller -Operation Install

        $result | Should Be $false
        Test-Path $signalFile | Should Be $false
    }
}
