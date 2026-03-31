# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Imperative steps for the secondary cluster node (Node02).
    Run AFTER DSC has been applied.

.DESCRIPTION
    Step 1: Domain join (requires reboot)
    Step 2: iSCSI target connection
    Step 3: Poll for Node01 cluster readiness, then create Node02 shares
            and run cluster status check.

.PARAMETER WorkingPath
    Path to the Cluster-Package folder.

.PARAMETER Step
    Which step to execute (1, 2, or 3).

.EXAMPLE
    .\Invoke-Node02ImperativeSteps.ps1 -Step 1 -WorkingPath C:\Cluster-Package
    .\Invoke-Node02ImperativeSteps.ps1 -Step 2 -WorkingPath C:\Cluster-Package
    .\Invoke-Node02ImperativeSteps.ps1 -Step 3 -WorkingPath C:\Cluster-Package
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

[string]$logFile = "$PSScriptRoot\Invoke-Node02ImperativeSteps.log"
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
    .\Write-Error.ps1 "Node02 imperative step 1 failed: $($_.Exception.Message)"
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

    .\Write-Info.ps1 "=== Node02 iSCSI connection completed ===" -ForegroundColor Cyan
    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "Node02 imperative step 2 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}

# ===========================================================================
# STEP 3: Poll for Cluster Readiness + Node02 Config
# ===========================================================================
if ($Step -eq 3) {
  $pollOk    = $false
  $sharesOk  = $false

  try {
    .\Write-Info.ps1 "---- Step 3: Poll for Cluster + Node02 Config ----" -ForegroundColor Yellow

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

    $node01Name = $config.Machines.Node01.ComputerName

    # -- Poll for Node01 cluster readiness --
    .\Write-Info.ps1 "Polling for cluster readiness on $node01Name..." -ForegroundColor Cyan
    $maxPollRetries = 120  # Up to 20 minutes (120 x 10s)
    $pollRetries = 0
    $clusterReady = $false

    while ($pollRetries -lt $maxPollRetries) {
        try {
            $cluster = Get-Cluster -ErrorAction Stop
            if ($null -ne $cluster) {
                $clusterReady = $true
                .\Write-Info.ps1 "[OK] Cluster '$($cluster.Name)' is accessible" -ForegroundColor Green
                break
            }
        } catch {
            # Cluster not ready yet
        }

        if ($pollRetries % 6 -eq 0) {
            .\Write-Info.ps1 "  Waiting for cluster... ($pollRetries / $maxPollRetries)" -ForegroundColor DarkGray
        }
        Start-Sleep -Seconds 10
        $pollRetries++
    }

    if (-not $clusterReady) {
        throw "Cluster not ready after $maxPollRetries retries (~$([math]::Round($maxPollRetries * 10 / 60)) minutes)."
    }
    $pollOk = $true

    # -- Create Node02-specific shares --
    .\Write-Info.ps1 "Creating Node02 shares..." -ForegroundColor Yellow
    try {
        $fullAccessAccount = "BUILTIN\Administrators"

        # FileShare
        $sharePath = "$systemDrive\FileShare"
        if (-not (Test-Path $sharePath)) { New-Item -ItemType Directory -Path $sharePath -Force | Out-Null }
        $existing = Get-SmbShare -Name 'FileShare' -ErrorAction SilentlyContinue
        if ($null -eq $existing) {
            New-SmbShare -Name 'FileShare' -Path $sharePath -FullAccess $fullAccessAccount -CachingMode BranchCache
            .\Write-Info.ps1 "[OK] FileShare created" -ForegroundColor Green
        } else {
            .\Write-Info.ps1 "[OK] FileShare already exists" -ForegroundColor Green
        }

        # SMBBasic
        $sharePath = "$systemDrive\SMBBasic"
        if (-not (Test-Path $sharePath)) { New-Item -ItemType Directory -Path $sharePath -Force | Out-Null }
        $existing = Get-SmbShare -Name 'SMBBasic' -ErrorAction SilentlyContinue
        if ($null -eq $existing) {
            New-SmbShare -Name 'SMBBasic' -Path $sharePath -FullAccess $fullAccessAccount -CachingMode BranchCache
            .\Write-Info.ps1 "[OK] SMBBasic created" -ForegroundColor Green
        } else {
            .\Write-Info.ps1 "[OK] SMBBasic already exists" -ForegroundColor Green
        }

        $sharesOk = $true
    } catch {
        .\Write-Info.ps1 "[FAIL] Node02 shares: $($_.Exception.Message)" -ForegroundColor Red
    }

    # -- Cluster Status Check --
    $checkScript = "$scriptsPath\Check-ClusterNodeStatus.ps1"
    if (Test-Path $checkScript) {
        .\Write-Info.ps1 "Checking cluster node status..." -ForegroundColor Yellow
        try { & $checkScript } catch { .\Write-Info.ps1 "[WARN] Status check: $($_.Exception.Message)" -ForegroundColor Yellow }
    }

    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "=== Node02 imperative steps completed (Poll=$pollOk, Shares=$sharesOk) ===" -ForegroundColor Cyan

    # Cluster poll is critical
    if (-not $pollOk) {
        Stop-Transcript; Pop-Location
        throw "Critical section failed: Cluster readiness poll."
    }

    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "Node02 imperative step 3 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}
