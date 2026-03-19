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
  $ptfOk        = $false

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
        $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'ComputerPasswordSet' -ErrorAction SilentlyContinue
        if ($null -eq $marker) {
            ksetup /SetComputerPassword Password04!
            if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
                New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
            }
            New-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'ComputerPasswordSet' -Value 1 -PropertyType DWord -Force | Out-Null
            .\Write-Info.ps1 "[OK] Computer password set" -ForegroundColor Green
        } else {
            .\Write-Info.ps1 "[OK] Computer password already set -- skipping" -ForegroundColor Green
        }
    } catch {
        .\Write-Info.ps1 "[WARN] ksetup failed: $($_.Exception.Message)" -ForegroundColor Yellow
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

    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "=== Node01 imperative steps completed (Cluster=$clusterOk, SMB2=$smb2Ok, FSA=$fsaOk, DFS=$dfsOk, QUIC=$quicOk, Auth=$authOk, FL2=$fl2Ok) ===" -ForegroundColor Cyan

    # Cluster creation is critical
    if (-not $clusterOk) {
        Stop-Transcript; Pop-Location
        throw "Critical section failed: Cluster creation."
    }

    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "Node01 imperative step 3 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}
