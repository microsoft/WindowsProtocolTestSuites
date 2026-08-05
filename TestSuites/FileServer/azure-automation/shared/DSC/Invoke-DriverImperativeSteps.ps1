# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Imperative steps for the Driver Computer (Client01).
    Run AFTER DSC has been applied. Works for Domain, Cluster, and Workgroup scenarios.

.DESCRIPTION
    Step 1: Domain join (requires reboot; skipped in Workgroup mode)
    Step 2: Post-reboot configuration:
      - Tool installation (DotNetCore, OpenSSH, PowerShellCore, PTMService, PTMCli, TestSuite, certs)
      - PTF config patching (cluster-specific, runs only if Config-ClusterPTFConfig.ps1 exists)
      - RSA key copy (domain-aware: copies to $adminUser.$domainNetBios .ssh folder)
      - sshd restart
      - ForceLevel2 (ShareUtil.exe against SUT/Node01 ShareForceLevel2)

.PARAMETER WorkingPath
    Path to the package folder (Domain-Package or Cluster-Package).

.PARAMETER Step
    Which step to execute (1 or 2). Deploy-Driver.ps1 orchestrates these.

.PARAMETER SkipForceLevel2
    Skip the ForceLevel2 configuration. Useful when SUT is not ready yet.

.PARAMETER NoTranscript
    Preserve the parent orchestrator transcript when invoked by Deploy-Driver.ps1.

.EXAMPLE
    .\Invoke-DriverImperativeSteps.ps1 -Step 1 -WorkingPath C:\Domain-Package
    .\Invoke-DriverImperativeSteps.ps1 -Step 2 -WorkingPath C:\Cluster-Package
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [ValidateSet(1, 2)]
    [int]$Step = 1,
    [string]$ConfigureFile = "$WorkingPath\Config.json",
    [switch]$SkipForceLevel2,
    [switch]$NoTranscript
)

$ErrorActionPreference = 'Stop'
$isLinuxDriver = $IsLinux -eq $true
# Azure CSE invokes pwsh as root without propagating HOME; downstream Join-Path $env:HOME fails with "Path is null".
if ($isLinuxDriver -and [string]::IsNullOrWhiteSpace($env:HOME)) {
    $env:HOME = '/root'
}
$scriptsPath = Join-Path $PSScriptRoot 'Scripts'
$pathSep = [IO.Path]::PathSeparator
$env:Path += "${pathSep}${WorkingPath}${pathSep}${scriptsPath}"
Push-Location $scriptsPath

[string]$logFile = "$PSScriptRoot\Invoke-DriverImperativeSteps.log"
$transcriptStarted = $false
if (-not $NoTranscript) {
    Start-Transcript -Path $logFile -Append -Force
    $transcriptStarted = $true
}

function Stop-LocalTranscript {
    if ($transcriptStarted) {
        Stop-Transcript
    }
}

$systemDrive = $env:SystemDrive

# Section success tracking
$toolsOk = $false
$ptfOk   = $false
$rsaOk   = $false
$fl2Ok   = $false

try {
    # Load configuration
    $config = $null
    if (Test-Path $ConfigureFile) {
        try { $config = Get-Content -Path $ConfigureFile -Raw | ConvertFrom-Json }
        catch { Write-Warning "Failed to parse Config.json: $_" }
    }
    if ($null -eq $config) {
        throw "Config.json not loaded from $ConfigureFile. Cannot proceed."
    }

    # Load Tools.json
    $tools = $null
    $toolsFile = "$WorkingPath\Tools.json"
    if (Test-Path $toolsFile) {
        .\Write-Info.ps1 "Loading Tools.json from WorkingPath: $toolsFile" -ForegroundColor DarkGray
    } else {
        $toolsFile = "$scriptsPath\Tools.json"
        if (Test-Path $toolsFile) {
            .\Write-Info.ps1 "Loading Tools.json from Scripts fallback: $toolsFile" -ForegroundColor DarkGray
        }
    }
    if (Test-Path $toolsFile) {
        try { $tools = Get-Content -Path $toolsFile -Raw | ConvertFrom-Json }
        catch { Write-Warning "Failed to parse Tools.json: $_" }
    }

    switch ($Step) {

        # ==================================================================
        #  STEP 1 -- Domain Join (requires reboot)
        # ==================================================================
        1 {
            .\Write-Info.ps1 '=== Driver Step 1: Domain Join ===' -ForegroundColor Cyan

            # Detect workgroup mode from Config.json
            # Note: Generate-ConfigJson sets Core.DomainName to "" for workgroup,
            # and machine entries have Domain = "Workgroup". Handle both.
            $domName = if ($config -and $config.Core) { $config.Core.DomainName } else { $null }
            $isWorkgroup = [string]::IsNullOrWhiteSpace($domName) -or $domName -eq 'Workgroup'

            if ($isLinuxDriver) {
                .\Write-Info.ps1 '[SKIP] Domain join not applicable on Linux' -ForegroundColor Yellow
            } elseif ($isWorkgroup) {
                .\Write-Info.ps1 '[SKIP] Workgroup mode -- domain join not required' -ForegroundColor Yellow
            } else {
                # Check if already domain-joined
                $isDomain = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
                if ($isDomain) {
                    .\Write-Info.ps1 '[OK] Already domain-joined -- skipping' -ForegroundColor Green
                }
                else {
                    .\Write-Info.ps1 'Joining domain...' -ForegroundColor Cyan
                    # Capture only the script's final return value. domainjoin.ps1 may emit
                    # stray success-stream output; a multi-element array is truthy even when
                    # the real result is $false, which would silently mask a failed join.
                    $joined = & "$scriptsPath\domainjoin.ps1" -NoTranscript | Select-Object -Last 1
                    if ($joined -ne $true) {
                        throw 'Domain join failed.'
                    }
                }

                # Prevent Netlogon from auto-rotating this member's machine-account
                # password. The DC does NOT set RefusePasswordChange, so a rotation
                # would succeed locally AND in AD -- but an Azure deallocate/restart in
                # the rotation window can capture an inconsistent state, producing the
                # "trust relationship between this workstation and the primary domain
                # failed" error. Disabling rotation keeps the local secret and AD copy
                # permanently in sync. Mirrors CommonScripts\Set-NetlogonRegKeyAndPolicy.ps1.
                & reg add 'HKLM\SYSTEM\CurrentControlSet\services\Netlogon\Parameters' /v DisablePasswordChange /t REG_DWORD /d 1 /f 2>&1 | .\Write-Info.ps1
            }

            .\Write-Info.ps1 'Step 1 complete.' -ForegroundColor Green
        }

        # ==================================================================
        #  STEP 2 -- Tools, PTF Config, RSA Keys, ForceLevel2 (post-reboot)
        # ==================================================================
        2 {
            .\Write-Info.ps1 '=== Driver Step 2: Tools + PTF Config + RSA Keys + ForceLevel2 ===' -ForegroundColor Cyan

            # Brief stabilization wait after reboot
            Start-Sleep -Seconds 5

            # -- Tool installation --
            try {
                $signalFile = Join-Path $scriptsPath 'InstallMSIAndTools.Completed.signal'
                if (Test-Path $signalFile) {
                    .\Write-Info.ps1 "[OK] Tools already installed (signal file exists)" -ForegroundColor Green
                } elseif ($isLinuxDriver) {
                    # Linux: install dotnet + extract test suite + certs
                    .\Write-Info.ps1 'Installing tools for Linux driver...' -ForegroundColor Yellow
                    $toolsFolder = Join-Path $scriptsPath 'Tools'

                    # 1. Install .NET SDK
                    $dotnetCmd = Get-Command dotnet -ErrorAction SilentlyContinue
                    if ($dotnetCmd) {
                        .\Write-Info.ps1 "[OK] dotnet already at $($dotnetCmd.Source)" -ForegroundColor Green
                    } else {
                        $installScript = '/tmp/dotnet-install.sh'
                        & curl -sSL 'https://dot.net/v1/dotnet-install.sh' -o $installScript
                        & bash $installScript --channel 8.0 --install-dir '/usr/share/dotnet'
                        if (-not (Test-Path '/usr/local/bin/dotnet')) {
                            & ln -sf '/usr/share/dotnet/dotnet' '/usr/local/bin/dotnet'
                        }
                        $env:DOTNET_ROOT = '/usr/share/dotnet'
                        $env:Path += ":/usr/share/dotnet"
                        .\Write-Info.ps1 "[OK] .NET SDK installed" -ForegroundColor Green
                    }

                    # 2. Extract test suite zip
                    if ($null -ne $tools) {
                        $testSuiteZip = $tools.DriverComputer.TestsuiteZips | Select-Object -First 1
                        if ($testSuiteZip) {
                            $zipName = $testSuiteZip.ZipName
                            $targetFolder = '/opt/FileServer-TestSuite-ServerEP'
                            $toolPath = Join-Path $toolsFolder $zipName
                            if (-not (Test-Path $toolPath) -and $testSuiteZip.Url) {
                                New-Item -ItemType Directory -Path $toolsFolder -Force | Out-Null
                                & curl -sSL $testSuiteZip.Url -o $toolPath
                            }
                            if (Test-Path $toolPath) {
                                New-Item -ItemType Directory -Path $targetFolder -Force | Out-Null
                                Expand-Archive -Path $toolPath -DestinationPath $targetFolder -Force
                                .\Write-Info.ps1 "[OK] Test suite extracted to $targetFolder" -ForegroundColor Green
                            }
                        }
                    }

                    # 3. Extract certs
                    if ($null -ne $tools) {
                        $certsTool = $tools.DriverComputer.Tools | Where-Object { $_.name -eq 'certs' }
                        if ($certsTool) {
                            $zipName = $certsTool.ZipName
                            $toolPath = Join-Path $toolsFolder $zipName
                            $certsTarget = Join-Path $env:HOME '.ssh'
                            if (-not (Test-Path $toolPath) -and $certsTool.Url) {
                                New-Item -ItemType Directory -Path $toolsFolder -Force | Out-Null
                                & curl -sSL $certsTool.Url -o $toolPath
                            }
                            if (Test-Path $toolPath) {
                                New-Item -ItemType Directory -Path $certsTarget -Force | Out-Null
                                Expand-Archive -Path $toolPath -DestinationPath $certsTarget -Force
                                & chmod 700 $certsTarget
                                & bash -c "chmod 600 '$certsTarget'/*"
                                .\Write-Info.ps1 "[OK] SSH certs extracted to $certsTarget" -ForegroundColor Green
                            }
                        }
                    }

                    "Completed" | Out-File -FilePath $signalFile -Force
                } else {
                    .\Write-Info.ps1 'Installing tools (DotNetCore, OpenSSH, PowerShellCore, PTMService, PTMCli, TestSuite, certs)...' -ForegroundColor Yellow
                    $result = & (Join-Path $scriptsPath 'InstallMSIAndTools.ps1') -Role 'DriverComputer'
                    if ($result -eq $true) {
                        .\Write-Info.ps1 "[OK] Tools installed successfully" -ForegroundColor Green
                    } else {
                        Write-Warning "Tool installation returned non-success"
                    }
                }
                # Postcondition: the tools signal exists only after a successful install
                # (InstallMSIAndTools writes it after all MSIs; the Linux/extract branch writes
                # it at the end of its steps). The critical-throw at end of script fires if false.
                $toolsOk = Test-Path $signalFile
            }
            catch {
                .\Write-Info.ps1 "[FAIL] Tools installation failed: $($_.Exception.Message)" -ForegroundColor Red
            }

            # -- PTF config patching (cluster-specific, no-op for domain) --
            $ptfScript = "$scriptsPath\Config-ClusterPTFConfig.ps1"
            if (Test-Path $ptfScript) {
                try {
                    .\Write-Info.ps1 'Configuring ptfconfig for cluster...' -ForegroundColor Yellow
                    & $ptfScript -workingDir $scriptsPath -protocolConfigFile $ConfigureFile
                    .\Write-Info.ps1 "[OK] Cluster ptfconfig configured" -ForegroundColor Green
                    $ptfOk = $true
                }
                catch {
                    .\Write-Info.ps1 "[FAIL] Cluster PTF config: $($_.Exception.Message)" -ForegroundColor Red
                }
            } else {
                $ptfOk = $true  # Not applicable (domain scenario)
            }

            # -- RSA key copy --
            try {
                .\Write-Info.ps1 'Configuring RSA keys...' -ForegroundColor Yellow

                if ($isLinuxDriver) {
                    # On Linux, certs were already extracted to ~/.ssh during tool install
                    $sshTargetPath = Join-Path $env:HOME '.ssh'
                    $authKeysFile  = Join-Path $sshTargetPath 'authorized_keys'
                    if (Test-Path $authKeysFile) {
                        .\Write-Info.ps1 "[OK] RSA keys already present at $sshTargetPath" -ForegroundColor Green
                    } else {
                        Write-Warning "SSH authorized_keys not found at $authKeysFile"
                    }

                    # Restart sshd via systemctl
                    $sshdActive = & systemctl is-active sshd 2>&1
                    if ($sshdActive -eq 'active' -or $sshdActive -eq 'activating') {
                        & systemctl restart sshd
                        .\Write-Info.ps1 '[OK] sshd restarted' -ForegroundColor Green
                    } else {
                        $sshActive = & systemctl is-active ssh 2>&1
                        if ($sshActive -eq 'active' -or $sshActive -eq 'activating') {
                            & systemctl restart ssh
                            .\Write-Info.ps1 '[OK] ssh restarted' -ForegroundColor Green
                        } else {
                            Write-Warning 'sshd service not found.'
                        }
                    }
                } else {
                    $hostName = [System.Net.Dns]::GetHostName()
                    if (-not $config -or -not $config.Core.Username) {
                        throw "Config.json Core.Username is required for RSA key configuration"
                    }
                    $adminUserName = $config.Core.Username
                    $domainNetBios = $null
                    if ($config -and $config.Core.DomainName -and $config.Core.DomainName -ne 'Workgroup') {
                        $domainNetBios = ($config.Core.DomainName -split '\.')[0]
                    }

                    # Resolve certs path from Tools.json
                    $certsPath = $null
                    if ($null -ne $tools) {
                        $vm = $config.Machines.PSObject.Properties | Where-Object { $_.Value.ComputerName -match $hostName }
                        if ($vm) {
                            $certsPathRaw = ($tools.($vm.Name).tools | Where-Object { $_.name -eq 'certs' } | Select-Object -First 1).targetFolder
                            if ($certsPathRaw) { $certsPath = [Environment]::ExpandEnvironmentVariables($certsPathRaw) }
                        }
                    }
                    if (-not $certsPath) { $certsPath = "$systemDrive\id_rsa\.ssh" }

                    # Domain-aware user folder
                    $userFolderName = if ($domainNetBios) { "$adminUserName.$domainNetBios" } else { $adminUserName }
                    $userFolderPath = "$systemDrive\Users\$userFolderName"
                    # Fallback to non-domain path if domain folder doesn't exist yet
                    if (-not (Test-Path $userFolderPath)) {
                        $userFolderPath = "$systemDrive\Users\$adminUserName"
                    }
                    $sshTargetPath = "$userFolderPath\.ssh"

                    if (Test-Path $certsPath) {
                        if (Test-Path $sshTargetPath) {
                            if (Test-Path "$sshTargetPath\authorized_keys") {
                                .\Write-Info.ps1 "[OK] RSA keys already present at $sshTargetPath" -ForegroundColor Green
                            } else {
                                Copy-Item "$certsPath\*" $sshTargetPath -Recurse -Force
                                .\Write-Info.ps1 "[OK] RSA keys copied to $sshTargetPath" -ForegroundColor Green
                            }
                        } else {
                            Copy-Item $certsPath $sshTargetPath -Recurse -Force
                            .\Write-Info.ps1 "[OK] RSA keys copied to $sshTargetPath" -ForegroundColor Green
                        }
                    } else {
                        Write-Warning "Certs path $certsPath not found. RSA keys not configured."
                    }

                    # Restart sshd if it exists
                    $sshdSvc = Get-Service sshd -ErrorAction SilentlyContinue
                    if ($null -ne $sshdSvc) {
                        Restart-Service sshd -Force
                        .\Write-Info.ps1 '[OK] sshd restarted' -ForegroundColor Green
                    } else {
                        Write-Warning 'sshd service not found. OpenSSH may not be installed yet.'
                    }

                    # Register scheduled task to re-copy RSA keys at logon
                    $rsaTaskName = 'Config-RSAKeys'
                    $existingRsaTask = Get-ScheduledTask -TaskName $rsaTaskName -ErrorAction SilentlyContinue
                    if ($null -eq $existingRsaTask) {
                        .\Write-Info.ps1 "Registering scheduled task '$rsaTaskName' to re-copy RSA keys at logon..." -ForegroundColor Cyan
                        $copyCmd = "if (Test-Path '$certsPath') { " +
                                   "Copy-Item '$certsPath\*' '$sshTargetPath' -Recurse -Force -ErrorAction SilentlyContinue; " +
                                   "Restart-Service sshd -Force -ErrorAction SilentlyContinue }"
                        $taskAction    = New-ScheduledTaskAction -Execute 'PowerShell.exe' -Argument "-NoProfile -ExecutionPolicy Bypass -Command `"$copyCmd`""
                        $taskTrigger   = New-ScheduledTaskTrigger -AtLogOn
                        $taskSettings  = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable
                        $rsaTaskUser = $config.Core.Username
                        $rsaTaskPass = $config.Core.Password
                        # Domain/Cluster: prefix with NetBIOS domain name (e.g., CONTOSO\testadmin)
                        $rsaDomainPrefix = if ($config.Domain -and $config.Domain.NetBiosName) {
                            $config.Domain.NetBiosName
                        } elseif (-not [string]::IsNullOrWhiteSpace($config.Core.DomainName) -and $config.Core.DomainName -ne 'Workgroup') {
                            ($config.Core.DomainName -split '\.')[0]
                        } else { '' }
                        if ($rsaDomainPrefix) { $rsaTaskUser = "$rsaDomainPrefix\$rsaTaskUser" }
                        Register-ScheduledTask -TaskName $rsaTaskName -Action $taskAction `
                            -Trigger $taskTrigger -Settings $taskSettings `
                            -User $rsaTaskUser -Password $rsaTaskPass -RunLevel Highest -Force | Out-Null
                        .\Write-Info.ps1 "[OK] Scheduled task '$rsaTaskName' registered (user=$rsaTaskUser)" -ForegroundColor Green
                    } else {
                        .\Write-Info.ps1 "[OK] Scheduled task '$rsaTaskName' already exists" -ForegroundColor Green
                    }
                }
                $rsaOk = $true
            }
            catch {
                .\Write-Info.ps1 "[FAIL] RSA key configuration failed: $($_.Exception.Message)" -ForegroundColor Red
            }

            # -- ForceLevel2 (non-fatal) --
            try {
                if ($SkipForceLevel2) {
                    .\Write-Info.ps1 'Skipping ForceLevel2 configuration (-SkipForceLevel2 specified).' -ForegroundColor DarkGray
                    $fl2Ok = $true
                }
                elseif ($isLinuxDriver) {
                    .\Write-Info.ps1 '[SKIP] ForceLevel2 -- ShareUtil.exe is Windows-only' -ForegroundColor DarkGray
                    $fl2Ok = $true
                }
                else {
                    .\Write-Info.ps1 'Configuring ForceLevel2 oplock on SUT...' -ForegroundColor Yellow

                    # Resolve SUT name (matches both domain 'Sut' and cluster 'Node01')
                    $sut = $null
                    if ($config) {
                        $sut = $config.Machines.PSObject.Properties |
                            Where-Object { $_.Name -match 'Sut|Node01' -or $_.Value.Role -match 'SUT' } |
                            Select-Object -First 1
                    }

                    if ($null -eq $sut) {
                        Write-Warning 'SUT machine not found in config. Skipping ForceLevel2.'
                        $fl2Ok = $true
                    }
                    elseif ($sut.Value.OS -eq 'Linux') {
                        .\Write-Info.ps1 '[SKIP] Linux SUT -- ForceLevel2 not supported' -ForegroundColor DarkGray
                        $fl2Ok = $true
                    }
                    else {
                        $sutComputerName = $sut.Value.ComputerName

                        # Locate ShareUtil.exe
                        $endPointPath = $null
                        if ($tools) {
                            $endPointPath = [Environment]::ExpandEnvironmentVariables($tools.DriverComputer.TestsuiteZips[0].targetFolder)
                        }
                        $ShareUtil = "$endPointPath\Utils\ShareUtil.exe"

                        if (-not (Test-Path $ShareUtil)) {
                            Write-Warning "ShareUtil.exe not found at $ShareUtil. Install test suite first."
                        }
                        else {
                            $adminUser = $config.Core.Username
                            $adminPass = $config.Core.Password
                            # Workgroup: authenticate as computername\user; Domain: domain\user
                            $isWgFL2 = [string]::IsNullOrWhiteSpace($config.Core.DomainName) -or $config.Core.DomainName -eq 'Workgroup'
                            $netUsePrefix = if ($isWgFL2) { $sutComputerName } else { ($config.Core.DomainName -split '\.')[0] }

                            # Quick check: is SUT reachable?
                            $sutReachable = Test-Connection -ComputerName $sutComputerName -Count 1 -Quiet -ErrorAction SilentlyContinue
                            $success = $false

                            if ($sutReachable) {
                                $netArgs = @('use', "\\$sutComputerName\IPC`$", "/user:$netUsePrefix\$adminUser", $adminPass)
                                $netProc = Start-Process -FilePath 'net.exe' -ArgumentList $netArgs -Wait -NoNewWindow -PassThru -ErrorAction SilentlyContinue
                                .\Write-Info.ps1 "  net use exit code: $($netProc.ExitCode)" -ForegroundColor DarkGray

                                $maxRetries = 3
                                for ($i = 0; $i -lt $maxRetries; $i++) {
                                    .\Write-Info.ps1 "  ForceLevel2 attempt $($i + 1)/$maxRetries on $sutComputerName\ShareForceLevel2..." -ForegroundColor DarkYellow
                                    try {
                                        $fl2Output = CMD /C "`"$ShareUtil`" $sutComputerName ShareForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1
                                        $fl2Output | .\Write-Info.ps1
                                    } catch {
                                        .\Write-Info.ps1 "  ShareUtil error: $($_.Exception.Message)" -ForegroundColor DarkGray
                                    }
                                    if ($LASTEXITCODE -eq 0) {
                                        $success = $true
                                        .\Write-Info.ps1 '[OK] ForceLevel2 configured' -ForegroundColor Green
                                        break
                                    }
                                    Start-Sleep -Seconds 5
                                }
                            }
                            else {
                                .\Write-Info.ps1 "SUT ($sutComputerName) is not reachable yet." -ForegroundColor Yellow
                            }

                            $fl2SignalFile = "$PSScriptRoot\ForceLevel2.Completed.signal"
                            if ($success) {
                                $fl2Ok = $true
                                "FL2 OK $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $fl2SignalFile -Force
                            }
                            else {
                                # Register a scheduled task to retry periodically until SUT is ready
                                $fl2TaskName = 'Config-ForceLevel2'
                                $existingFl2Task = Get-ScheduledTask -TaskName $fl2TaskName -ErrorAction SilentlyContinue
                                if ($null -ne $existingFl2Task) {
                                    .\Write-Info.ps1 "[OK] Scheduled task '$fl2TaskName' already registered -- will retry automatically" -ForegroundColor Green
                                }
                                else {
                                    .\Write-Info.ps1 "Registering scheduled task '$fl2TaskName' to retry every 5 minutes..." -ForegroundColor Yellow
                                    $fl2Script = @"
`$ShareUtil = '$ShareUtil'
`$ConfigPath = '$ConfigureFile'
`$logPath = '$PSScriptRoot\Config-ForceLevel2.log'
`$fl2SignalFile = '$fl2SignalFile'
Add-Content -Path `$logPath -Value "[`$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] Checking SUT readiness..."
`$cfg = Get-Content -Path `$ConfigPath -Raw | ConvertFrom-Json
`$SutName = (`$cfg.Machines.PSObject.Properties | Where-Object { `$_.Name -match 'Sut|Node01' -or `$_.Value.Role -match 'SUT' } | Select-Object -First 1).Value.ComputerName
`$AdminUser = `$cfg.Core.Username
`$AdminPass = `$cfg.Core.Password
if (-not (Test-Connection -ComputerName `$SutName -Count 1 -Quiet -ErrorAction SilentlyContinue)) {
    Add-Content -Path `$logPath -Value "[`$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] SUT not reachable. Will retry."
    exit 1
}
`$isWg = [string]::IsNullOrWhiteSpace(`$cfg.Core.DomainName) -or `$cfg.Core.DomainName -eq 'Workgroup'
`$NetUsePrefix = if (`$isWg) { `$SutName } else { (`$cfg.Core.DomainName -split '\.')[0] }
`$netArgs = @('use', "\\`$SutName\IPC`$", "/user:`$NetUsePrefix\`$AdminUser", `$AdminPass)
Start-Process -FilePath 'net.exe' -ArgumentList `$netArgs -Wait -NoNewWindow -ErrorAction SilentlyContinue
& `$ShareUtil `$SutName ShareForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true 2>&1 | Out-Null
if (`$LASTEXITCODE -eq 0) {
    Add-Content -Path `$logPath -Value "[`$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] ForceLevel2 configured successfully. Cleaning up task."
    "FL2 OK `$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath `$fl2SignalFile -Force
    Unregister-ScheduledTask -TaskName '$fl2TaskName' -Confirm:`$false
    Remove-Item -Path `$MyInvocation.MyCommand.Path -Force -ErrorAction SilentlyContinue
} else {
    Add-Content -Path `$logPath -Value "[`$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] ShareUtil failed (exit `$LASTEXITCODE). Will retry."
    exit 1
}
"@
                                    $fl2ScriptPath = "$PSScriptRoot\Config-ForceLevel2-Task.ps1"
                                    $fl2Script | Out-File -FilePath $fl2ScriptPath -Force -Encoding ASCII
                                    $fl2Action    = New-ScheduledTaskAction -Execute 'PowerShell.exe' `
                                        -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$fl2ScriptPath`""
                                    $fl2Trigger   = New-ScheduledTaskTrigger -Once -At (Get-Date).AddMinutes(5) `
                                        -RepetitionInterval (New-TimeSpan -Minutes 5)
                                    $fl2Settings  = New-ScheduledTaskSettingsSet -StartWhenAvailable -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries
                                    $fl2TaskUser = $config.Core.Username
                                    $fl2TaskPass = $config.Core.Password
                                    # Domain/Cluster: prefix with NetBIOS domain name (e.g., CONTOSO\testadmin)
                                    $fl2DomainPrefix = if ($config.Domain -and $config.Domain.NetBiosName) {
                                        $config.Domain.NetBiosName
                                    } elseif (-not [string]::IsNullOrWhiteSpace($config.Core.DomainName) -and $config.Core.DomainName -ne 'Workgroup') {
                                        ($config.Core.DomainName -split '\.')[0]
                                    } else { '' }
                                    if ($fl2DomainPrefix) { $fl2TaskUser = "$fl2DomainPrefix\$fl2TaskUser" }
                                    Register-ScheduledTask -TaskName $fl2TaskName -Action $fl2Action `
                                        -Trigger $fl2Trigger -Settings $fl2Settings `
                                        -User $fl2TaskUser -Password $fl2TaskPass -RunLevel Highest -Force | Out-Null
                                    .\Write-Info.ps1 "[OK] Scheduled task '$fl2TaskName' registered (user=$fl2TaskUser) -- will auto-configure when SUT is ready and clean up after" -ForegroundColor Green
                                }
                                # $fl2Ok stays $false -- Deploy-Driver will not write signal file
                            }
                        }
                    }
                }
            }
            catch {
                .\Write-Info.ps1 "[FAIL] ForceLevel2 configuration failed: $($_.Exception.Message)" -ForegroundColor Red
            }

            .\Write-Info.ps1 ''
            .\Write-Info.ps1 "=== Driver imperative steps completed (Tools=$toolsOk, PTF=$ptfOk, RSA=$rsaOk, FL2=$fl2Ok) ===" -ForegroundColor Cyan
        }
    }
}
catch {
    .\Write-Error.ps1 "Driver imperative step $Step failed: $_"
    Stop-LocalTranscript
    Pop-Location
    throw
}

# Tools installation is critical -- throw if it failed so the parent
# orchestrator (Deploy-Driver.ps1) can catch and report the failure.
if ($Step -eq 2 -and -not $toolsOk) {
    Stop-LocalTranscript
    Pop-Location
    throw "Critical section failed: Tools installation. Check Invoke-DriverImperativeSteps.log for details."
}

Stop-LocalTranscript
Pop-Location
