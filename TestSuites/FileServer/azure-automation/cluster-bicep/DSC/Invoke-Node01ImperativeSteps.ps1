# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Imperative steps for the primary cluster node (Node01).
    Run AFTER DSC has been applied.

.DESCRIPTION
    Step 1: Domain join (requires reboot)
    Step 2: iSCSI target connection
    Step 3: Cluster creation + environment setup
      - Initialize RAW disks
      - Create failover cluster (Create-ServerFailoverEnv.ps1)
      - Create SMB2, DFSC, FSA, Auth, QUIC environments
      - ForceLevel2 oplock
      - PTF config patching
      - Cluster status check

.PARAMETER WorkingPath
    Path to the Cluster-Package folder.

.PARAMETER Step
    Which step to execute (1, 2, or 3).

.EXAMPLE
    .\Invoke-Node01ImperativeSteps.ps1 -Step 1 -WorkingPath C:\Cluster-Package
    .\Invoke-Node01ImperativeSteps.ps1 -Step 2 -WorkingPath C:\Cluster-Package
    .\Invoke-Node01ImperativeSteps.ps1 -Step 3 -WorkingPath C:\Cluster-Package
#>

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'Passwords originate in the private deployment Config.json and must be converted for AD cmdlets; no interactive prompt is available.')]
param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [ValidateSet(1, 2, 3)]
    [int]$Step = 1,
    [string]$ConfigureFile = "$WorkingPath\Config.json"
)

$ErrorActionPreference = 'Stop'
$scriptsPath = "$PSScriptRoot\Scripts"
$env:Path += ";$WorkingPath;$scriptsPath"
Push-Location $scriptsPath

[string]$logFile = "$PSScriptRoot\Invoke-Node01ImperativeSteps.log"
Start-Transcript -Path $logFile -Append -Force

$config = $null
if (Test-Path $ConfigureFile) {
    try { $config = Get-Content -Path $ConfigureFile -Raw | ConvertFrom-Json }
    catch { Write-Warning "Failed to parse Config.json: $_" }
}

$systemDrive = $env:SystemDrive

# ===========================================================================
# STEP 1: Domain Join
# ===========================================================================
if ($Step -eq 1) {
  try {
    .\Write-Info.ps1 "---- Step 1: Domain Join ----" -ForegroundColor Yellow

    $isDomain = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
    if ($isDomain) {
        .\Write-Info.ps1 "[OK] Already domain-joined -- skipping" -ForegroundColor Green
    }
    else {
        .\Write-Info.ps1 "Joining domain..." -ForegroundColor Cyan
        $result = & "$scriptsPath\domainjoin.ps1"
        if (-not $result) {
            .\Write-Error.ps1 "Domain join failed."
            Stop-Transcript; Pop-Location; return $false
        }
        .\Write-Info.ps1 "[OK] Domain join complete. REBOOT REQUIRED." -ForegroundColor Green
    }

    # Prevent Netlogon from auto-rotating this cluster node's machine-account password.
    # The DC does NOT set RefusePasswordChange, so a rotation would succeed on both
    # sides -- but an Azure deallocate/restart in the rotation window can capture an
    # inconsistent state, producing a broken "trust relationship" on the node. Disabling
    # rotation keeps the local secret and AD copy in sync. Mirrors
    # CommonScripts\Set-NetlogonRegKeyAndPolicy.ps1.
    & reg add 'HKLM\SYSTEM\CurrentControlSet\services\Netlogon\Parameters' /v DisablePasswordChange /t REG_DWORD /d 1 /f 2>&1 | .\Write-Info.ps1

    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "Node01 imperative step 1 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}

# ===========================================================================
# STEP 2: iSCSI Target Connection
# ===========================================================================
if ($Step -eq 2) {
  try {
    .\Write-Info.ps1 "---- Step 2: iSCSI Target Connection ----" -ForegroundColor Yellow

    if ($null -eq $config) {
        throw "Config.json not loaded. Cannot proceed."
    }

    $storageMachine = $config.Machines.Storage
    $storageIp = $storageMachine.IpConfig[0].Ip

    # Set msiscsi service to auto-start
    $msiscsi = Get-Service msiscsi -ErrorAction SilentlyContinue
    if ($null -ne $msiscsi) {
        if ($msiscsi.StartType -ne 'Automatic') {
            Set-Service msiscsi -StartupType Automatic
        }
        if ($msiscsi.Status -ne 'Running') {
            Start-Service msiscsi
            Start-Sleep -Seconds 3
        }
        .\Write-Info.ps1 "[OK] msiscsi service running" -ForegroundColor Green
    }

    # Test connectivity to storage
    .\Write-Info.ps1 "Testing connectivity to Storage ($storageIp)..." -ForegroundColor Cyan
    $retries = 0
    $maxRetries = 60
    while ($retries -lt $maxRetries) {
        if (Test-Connection -ComputerName $storageIp -Count 1 -Quiet -ErrorAction SilentlyContinue) {
            break
        }
        Start-Sleep -Seconds 10
        $retries++
    }
    if ($retries -ge $maxRetries) {
        throw "Storage ($storageIp) not reachable after $maxRetries retries."
    }
    .\Write-Info.ps1 "[OK] Storage reachable" -ForegroundColor Green

    # Discover iSCSI target portal
    $portal = Get-IscsiTargetPortal -TargetPortalAddress $storageIp -ErrorAction SilentlyContinue
    if ($null -eq $portal) {
        .\Write-Info.ps1 "Discovering iSCSI target portal at $storageIp..." -ForegroundColor Cyan
        New-IscsiTargetPortal -TargetPortalAddress $storageIp
    }

    # Connect to target
    $targets = Get-IscsiTarget -ErrorAction SilentlyContinue
    if ($null -ne $targets) {
        foreach ($target in $targets) {
            $session = Get-IscsiSession -ErrorAction SilentlyContinue |
                Where-Object { $_.TargetNodeAddress -eq $target.NodeAddress }
            if ($null -eq $session) {
                .\Write-Info.ps1 "Connecting to iSCSI target: $($target.NodeAddress)..." -ForegroundColor Cyan
                $connected = $false
                for ($i = 0; $i -lt 5; $i++) {
                    try {
                        Connect-IscsiTarget -NodeAddress $target.NodeAddress -IsPersistent $true -ErrorAction Stop
                        $connected = $true
                        break
                    } catch {
                        .\Write-Info.ps1 "  Retry $($i + 1)/5: $($_.Exception.Message)" -ForegroundColor Yellow
                        Start-Sleep -Seconds 10
                    }
                }
                if ($connected) {
                    .\Write-Info.ps1 "[OK] Connected to $($target.NodeAddress)" -ForegroundColor Green
                } else {
                    throw "Failed to connect to iSCSI target after 5 retries."
                }
            } else {
                .\Write-Info.ps1 "[OK] Already connected to $($target.NodeAddress)" -ForegroundColor Green
            }
        }
    }

    .\Write-Info.ps1 "=== Node01 iSCSI connection completed ===" -ForegroundColor Cyan
    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "Node01 imperative step 2 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}

# ===========================================================================
# STEP 3: Cluster Creation + Environment Setup
# ===========================================================================
if ($Step -eq 3) {
  $clusterOk    = $false
  $smb2Ok       = $false
  $fsaOk        = $false
  $dfsOk        = $false
  $quicOk       = $false
  $authOk       = $false
  $fl2Ok        = $false
  $sshKeysOk    = $false
    $computerPasswordOk = $false

  try {
    .\Write-Info.ps1 "---- Step 3: Cluster + Environment Setup ----" -ForegroundColor Yellow

    if ($null -eq $config) {
        throw "Config.json not loaded. Cannot proceed."
    }

    # -- TLS Cipher Suite Configuration --
    .\Write-Info.ps1 "Configuring TLS cipher suites..." -ForegroundColor Yellow
    try {
        $tlsResult = & "$scriptsPath\Configure-TlsCipherSuites.ps1"
        if ($tlsResult) {
            .\Write-Info.ps1 "[OK] TLS cipher suites configured" -ForegroundColor Green
        }
    } catch {
        .\Write-Info.ps1 "[WARN] TLS cipher suite config failed: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # -- Computer Password (ksetup) --
    # Must run AFTER domain join (matches pipeline's Set-ComputerPassword.ps1).
    .\Write-Info.ps1 "Setting computer password (ksetup)..." -ForegroundColor Yellow
    try {
        if (-not $config -or [string]::IsNullOrWhiteSpace($config.Core.Username) -or
            [string]::IsNullOrWhiteSpace($config.Core.Password)) {
            throw 'Config.json Core.Username and Core.Password are required to reset the AD computer password.'
        }
        $adDomain = (Get-CimInstance Win32_ComputerSystem).Domain
        $domainNetBios = if ($config.Domain -and $config.Domain.NetBiosName) {
            $config.Domain.NetBiosName
        } else {
            $adDomain.Split('.')[0].ToUpperInvariant()
        }
        $domainAdminPassword = ConvertTo-SecureString $config.Core.Password -AsPlainText -Force
        $domainCredential = [pscredential]::new(
            "$domainNetBios\$($config.Core.Username)",
            $domainAdminPassword)
        $machinePassword = ConvertTo-SecureString 'Password04!' -AsPlainText -Force
        $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'ComputerPasswordSet' -ErrorAction SilentlyContinue
        if ($null -ne $marker -and $marker.ComputerPasswordSet -eq 2) {
            try { $computerPasswordOk = Test-ComputerSecureChannel -ErrorAction Stop } catch { $computerPasswordOk = $false }
            if (-not $computerPasswordOk) {
                Remove-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
                    -Name 'ComputerPasswordSet' -ErrorAction SilentlyContinue
            }
        }

        for ($attempt = 1; $attempt -le 10 -and -not $computerPasswordOk; $attempt++) {
            .\Write-Info.ps1 "  Computer password sync attempt $attempt/10..." -ForegroundColor DarkGray
            try {
                Set-ADAccountPassword -Identity "$($env:COMPUTERNAME)$" -Reset `
                    -NewPassword $machinePassword -Credential $domainCredential `
                    -Server $adDomain -ErrorAction Stop
                $ksetupOutput = ksetup /SetComputerPassword Password04! 2>&1
                $ksetupExitCode = $LASTEXITCODE
                $ksetupOutput | .\Write-Info.ps1
                if ($ksetupExitCode -ne 0) {
                    throw "ksetup returned exit code $ksetupExitCode."
                }
                Restart-Service Netlogon -Force -ErrorAction Stop
                Start-Sleep -Seconds 5
                $computerPasswordOk = Test-ComputerSecureChannel -ErrorAction Stop
            } catch {
                .\Write-Info.ps1 "  Password sync failed: $($_.Exception.Message)" -ForegroundColor DarkGray
                $computerPasswordOk = $false
            }
            if (-not $computerPasswordOk -and $attempt -lt 10) {
                Start-Sleep -Seconds 30
            }
        }

        if (-not $computerPasswordOk) {
            throw 'Failed to synchronize the Node01 AD and local machine passwords or verify the secure channel.'
        }
        if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
            New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
        }
        New-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' `
            -Name 'ComputerPasswordSet' -Value 2 -PropertyType DWord -Force | Out-Null
        .\Write-Info.ps1 "[OK] AD and local machine passwords synchronized; secure channel verified" -ForegroundColor Green
    } catch {
        .\Write-Error.ps1 "[FAIL] Computer password setup failed: $($_.Exception.Message)"
        throw
    }

    # NOTE: Do NOT Initialize-Disk here. Create-ServerFailoverEnv.ps1 handles
    # disk preparation via diskpart (MBR + NTFS format). Pre-initializing as GPT
    # creates an MSR partition that tricks the script into skipping formatting,
    # leaving disks without NTFS volumes and breaking Set-ClusterQuorum.

    # -- Create Failover Cluster --
    .\Write-Info.ps1 "Creating failover cluster environment..." -ForegroundColor Yellow
    $failoverScript = "$scriptsPath\Create-ServerFailoverEnv.ps1"
    if (Test-Path $failoverScript) {
        $failureSignal = "$WorkingPath\Config_$($env:COMPUTERNAME)_FailureSignal.log"
        if (Test-Path $failureSignal) { Remove-Item $failureSignal -Force }

        & $failoverScript
        if (Test-Path $failureSignal) {
            throw "Create-ServerFailoverEnv.ps1 failed. Check logs for details."
        }
        $clusterOk = $true
        .\Write-Info.ps1 "[OK] Failover cluster created" -ForegroundColor Green
    } else {
        throw "Create-ServerFailoverEnv.ps1 not found at $failoverScript"
    }

    # -- Create SMB2 Environment --
    .\Write-Info.ps1 "Creating SMB2 environment..." -ForegroundColor Yellow
    try {
        $smb2Script = "$scriptsPath\Create-SMB2Env.ps1"
        if (Test-Path $smb2Script) { & $smb2Script }
        $smb2Ok = $true
        .\Write-Info.ps1 "[OK] SMB2 environment created" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[FAIL] SMB2 environment: $($_.Exception.Message)" -ForegroundColor Red
    }

    # -- Create DFSC Environment --
    .\Write-Info.ps1 "Creating DFSC environment..." -ForegroundColor Yellow
    try {
        $dfscScript = "$scriptsPath\Create-DFSCEnv.ps1"
        if (Test-Path $dfscScript) { & $dfscScript }
        $dfsOk = $true
        .\Write-Info.ps1 "[OK] DFSC environment created" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[FAIL] DFSC environment: $($_.Exception.Message)" -ForegroundColor Red
    }

    # -- Create FSA Environment --
    .\Write-Info.ps1 "Creating FSA environment..." -ForegroundColor Yellow
    try {
        $fsaScript = "$scriptsPath\Create-FSAEnv.ps1"
        if (Test-Path $fsaScript) { & $fsaScript }
        $fsaOk = $true
        .\Write-Info.ps1 "[OK] FSA environment created" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[FAIL] FSA environment: $($_.Exception.Message)" -ForegroundColor Red
    }

    # -- Create Auth Environment --
    .\Write-Info.ps1 "Creating Auth environment..." -ForegroundColor Yellow
    try {
        $authScript = "$scriptsPath\Create-AuthEnv.ps1"
        if (Test-Path $authScript) { & $authScript }
        $authOk = $true
        .\Write-Info.ps1 "[OK] Auth environment created" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[FAIL] Auth environment: $($_.Exception.Message)" -ForegroundColor Red
    }

    # -- Create QUIC Environment --
    .\Write-Info.ps1 "Creating QUIC environment..." -ForegroundColor Yellow
    try {
        $quicScript = "$scriptsPath\Create-QUICEnv.ps1"
        if (Test-Path $quicScript) { & $quicScript }
        $quicOk = $true
        .\Write-Info.ps1 "[OK] QUIC environment created" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[FAIL] QUIC environment: $($_.Exception.Message)" -ForegroundColor Red
    }

    # -- ForceLevel2 --
    .\Write-Info.ps1 "Configuring ForceLevel2..." -ForegroundColor Yellow
    try {
        $fl2Script = "$scriptsPath\Config-ForceLevel2.ps1"
        if (Test-Path $fl2Script) { & $fl2Script }
        $fl2Ok = $true
        .\Write-Info.ps1 "[OK] ForceLevel2 configured" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[WARN] ForceLevel2: $($_.Exception.Message)" -ForegroundColor Yellow
        $fl2Ok = $true  # Non-critical
    }

    # -- Copy Vhdx (if script exists) --
    $copyVhdxScript = "$scriptsPath\Copy-Vhdx.ps1"
    if (Test-Path $copyVhdxScript) {
        .\Write-Info.ps1 "Copying Vhdx..." -ForegroundColor Yellow
        try { & $copyVhdxScript } catch { .\Write-Info.ps1 "[WARN] Copy-Vhdx: $($_.Exception.Message)" -ForegroundColor Yellow }
    }

    # -- Cluster Status Check --
    $checkScript = "$scriptsPath\Check-ClusterNodeStatus.ps1"
    if (Test-Path $checkScript) {
        .\Write-Info.ps1 "Checking cluster node status..." -ForegroundColor Yellow
        try { & $checkScript } catch { .\Write-Info.ps1 "[WARN] Status check: $($_.Exception.Message)" -ForegroundColor Yellow }
    }

    # -- SSH server authorized_keys (PowerShell-over-SSH remoting for control adapters) --
    # The Authorization/permission control adapters (GetGroupSid, GetUserSid, ...) reach the
    # domain nodes/DC via `Invoke-Command -HostName` (PS over SSH). Windows OpenSSH reads
    # administrators_authorized_keys for the domain admin, which the SSH-certs tool does not
    # populate -- without it sshd falls back to a password prompt and the Authorization tests
    # hang. Install + verify the trusted key as a critical step.
    try {
        .\Write-Info.ps1 "Configuring SSH authorized_keys for PowerShell-over-SSH remoting..." -ForegroundColor Yellow
        $sshKeysOk = [bool](& "$scriptsPath\Set-SshServerAuthorizedKeys.ps1" -Config $config)
    } catch {
        .\Write-Info.ps1 "[WARN] SSH authorized_keys setup error: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "=== Node01 imperative steps completed (ComputerPassword=$computerPasswordOk, Cluster=$clusterOk, SMB2=$smb2Ok, FSA=$fsaOk, DFS=$dfsOk, QUIC=$quicOk, Auth=$authOk, FL2=$fl2Ok, SshKeys=$sshKeysOk) ===" -ForegroundColor Cyan

    # Cluster creation and SSH remoting are critical
    if (-not $clusterOk) {
        Stop-Transcript; Pop-Location
        throw "Critical section failed: Cluster creation."
    }
    if (-not $sshKeysOk) {
        Stop-Transcript; Pop-Location
        throw "Critical section failed: SSH remoting authorized_keys. The Authorization control adapters remote via PowerShell-over-SSH; without trusted keys they hang on a password prompt."
    }

    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "Node01 imperative step 3 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}
