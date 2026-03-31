# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Imperative steps for the Domain Controller (DC01) that cannot be
    expressed in DSC. Run AFTER DSC has been applied.

.DESCRIPTION
    Handles (in order):
    1. DC Promotion (Install-ADDSForest) + tools install  <- REQUIRES REBOOT
    2. Test account creation (AD users, groups, Guest)
    3. CBAC objects (claim types, resource properties, central access
       rules & policies)
    4. GPO import for claims
    5. DNS record creation for all machines + endpoints
    6. DC status checker scheduled task
    7. Tool installation (PowerShellCore, OpenSSH)
    8. RemoteAccess service start + configuration

    IMPORTANT: This script supports a -Step parameter.
      Step 1 = Features already installed by DSC, promote DC, reboot.
      Step 2 = Post-reboot: test accounts, CBAC, GPO, DNS, tools, services.

.PARAMETER WorkingPath
    Path to the Domain-Package folder.

.PARAMETER Step
    Which step to execute (1 or 2). Deploy-DC.ps1 orchestrates these.

.EXAMPLE
    .\Invoke-DcImperativeSteps.ps1 -Step 1 -WorkingPath D:\Domain-Package
    # (reboots)
    .\Invoke-DcImperativeSteps.ps1 -Step 2 -WorkingPath D:\Domain-Package
#>

param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [ValidateSet(1, 2)]
    [int]$Step = 1,
    [string]$ConfigureFile = "$WorkingPath\Config.json"
)

$ErrorActionPreference = 'Continue'
$scriptsPath = "$PSScriptRoot\Scripts"
$env:Path += ";$scriptsPath"
Push-Location $scriptsPath

[string]$logFile = "$PSScriptRoot\Invoke-DcImperativeSteps.log"
Start-Transcript -Path $logFile -Append -Force

# Section success tracking
$promoteOk = $false
$accountsOk = $false
$cbacOk = $false
$gpoOk = $false
$dnsOk = $false
$toolsOk = $false

# ===========================================================================
# Load config
# ===========================================================================
$config = $null
if (Test-Path $ConfigureFile) {
    try { $config = Get-Content -Path $ConfigureFile -Raw | ConvertFrom-Json }
    catch { Write-Warning "Failed to parse Config.json: $_" }
}

if ($null -eq $config) {
    .\Write-Error.ps1 "Config.json not loaded. Cannot proceed."
    Stop-Transcript; Pop-Location; return $false
}

$systemDrive = $env:SystemDrive

# ===========================================================================
# STEP 1: Promote DC (requires reboot after)
# ===========================================================================
if ($Step -eq 1) {
  try {
    .\Write-Info.ps1 "---- Step 1: Promote to Domain Controller ----" -ForegroundColor Yellow

    # Check if already a DC
    $isDc = $false
    try {
        $adDomain = Get-ADDomain -ErrorAction SilentlyContinue
        if ($null -ne $adDomain) { $isDc = $true }
    } catch {}

    if ($isDc) {
        .\Write-Info.ps1 "[OK] Already a Domain Controller -- skipping promotion" -ForegroundColor Green
    }
    else {
        # Resolve config
        $server = $config.Machines.DC
        $domainName = if ($server.domain) { $server.domain }
                      elseif ($config.Core.DomainName) { $config.Core.DomainName }
                      else { throw "Config.json Core.DomainName is required for DC configuration" }
        $adminUser  = if ($server.username) { $server.username }
                      elseif ($config.Core.Username) { $config.Core.Username }
                      else { throw "Config.json Core.Username is required for DC configuration" }
        $adminPwd   = if ($server.password) { $server.password }
                      elseif ($config.Core.Password) { $config.Core.Password }
                      else { throw "Config.json Core.Password is required for DC configuration" }

        .\Write-Info.ps1 "Promoting to DC for domain $domainName..." -ForegroundColor Cyan
        $result = & "$scriptsPath\PromoteDomainController.ps1" -DomainName $domainName -AdminPwd $adminPwd -AdminUser $adminUser
        if (-not $result) {
            .\Write-Error.ps1 "DC promotion failed."
            Stop-Transcript; Pop-Location; return $false
        }

        # Install tools in background before reboot
        $toolsSignal = "$scriptsPath\InstallMSIAndTools.Completed.signal"
        if (-not (Test-Path $toolsSignal)) {
            .\Write-Info.ps1 "Installing tools (background)..." -ForegroundColor Cyan
            & "$scriptsPath\InstallMSIAndTools.ps1" -Role 'DC'
        }

        .\Write-Info.ps1 "[OK] DC promotion complete. REBOOT REQUIRED." -ForegroundColor Green
    }

    $promoteOk = $true
    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "DC imperative step 1 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}

# ===========================================================================
# STEP 2: Post-reboot configuration (AD-dependent)
# ===========================================================================
if ($Step -eq 2) {
  try {
    .\Write-Info.ps1 "---- Step 2: Post-Reboot DC Configuration ----" -ForegroundColor Yellow

    # Wait for AD Web Services
    .\Write-Info.ps1 "Waiting for ADWS..." -ForegroundColor Cyan
    $retries = 0
    $domain = $null
    while ($retries -lt 30) {
        try {
            $domain = Get-ADDomain -ErrorAction Stop
            if ($null -ne $domain) { break }
        } catch {}
        Start-Sleep 10
        $retries++
    }
    if ($null -eq $domain) {
        .\Write-Error.ps1 "ADWS not responding after 5 minutes."
        Stop-Transcript; Pop-Location; return $false
    }
    .\Write-Info.ps1 "[OK] ADWS is responding" -ForegroundColor Green

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

    # -- Ensure Core.Username is a Domain Admin --
    # The Bicep adminUsername (e.g. testadmin) becomes a domain user after forest
    # promotion but is NOT automatically in Domain Admins.  All deploy scripts
    # (TKFRSAR, New-Cluster, test execution) run as this account, so it must
    # have full domain admin rights on every machine.
    $adminUser = $config.Core.Username
    if ($adminUser) {
        try {
            $adUser = Get-ADUser -Identity $adminUser -ErrorAction Stop
            $isDomainAdmin = (Get-ADGroupMember -Identity 'Domain Admins' -ErrorAction Stop |
                Where-Object { $_.SamAccountName -eq $adminUser })
            if (-not $isDomainAdmin) {
                Add-ADGroupMember -Identity 'Domain Admins' -Members $adUser
                .\Write-Info.ps1 "[OK] Added '$adminUser' to Domain Admins" -ForegroundColor Green
            } else {
                .\Write-Info.ps1 "[OK] '$adminUser' already in Domain Admins" -ForegroundColor Green
            }
        } catch {
            .\Write-Info.ps1 "[WARN] Could not add '$adminUser' to Domain Admins: $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }

    # -- Test Accounts --
    .\Write-Info.ps1 "Creating test accounts..." -ForegroundColor Yellow
    try {
        $result = & "$scriptsPath\Create-TestAccount.ps1"
        if (-not $result) {
            Write-Warning "Create-TestAccount.ps1 returned failure"
        } else {
            .\Write-Info.ps1 "[OK] Test accounts created" -ForegroundColor Green
        }
        $accountsOk = $true
    } catch {
        .\Write-Info.ps1 "[WARN] Test accounts failed: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # -- CBAC Objects --
    .\Write-Info.ps1 "Creating CBAC objects..." -ForegroundColor Yellow
    try {
        $result = & "$scriptsPath\Create-CbacObjectsInDC.ps1"
        if (-not $result) {
            Write-Warning "Create-CbacObjectsInDC.ps1 returned failure"
        } else {
            .\Write-Info.ps1 "[OK] CBAC objects created" -ForegroundColor Green
        }
        $cbacOk = $true
    } catch {
        .\Write-Info.ps1 "[WARN] CBAC objects failed: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # -- GPO Import --
    .\Write-Info.ps1 "Importing GPO for claims..." -ForegroundColor Yellow
    try {
        $result = & "$scriptsPath\Import-GPOForClaims.ps1"
        if (-not $result) {
            Write-Warning "Import-GPOForClaims.ps1 returned failure"
        } else {
            .\Write-Info.ps1 "[OK] GPO imported" -ForegroundColor Green
        }
        $gpoOk = $true
    } catch {
        .\Write-Info.ps1 "[WARN] GPO import failed: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # -- DNS Records --
    .\Write-Info.ps1 "Creating DNS records..." -ForegroundColor Yellow
    try {
        $result = & "$scriptsPath\Create-DNSRecords.ps1"
        if (-not $result) {
            Write-Warning "Create-DNSRecords.ps1 returned failure"
        } else {
            .\Write-Info.ps1 "[OK] DNS records processed" -ForegroundColor Green
        }
        $dnsOk = $true
    } catch {
        .\Write-Info.ps1 "[WARN] DNS records failed: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # -- DC Status checker task --
    .\Write-Info.ps1 "Configuring DC status checker..." -ForegroundColor Yellow
    try {
        $dcStatusResult = & "$scriptsPath\Check-DCStatus.ps1" -action 'CreateCheckerTask'
        if (-not $dcStatusResult) {
            Write-Warning "Check-DCStatus.ps1 returned failure"
        } else {
            .\Write-Info.ps1 "[OK] DC status checker configured" -ForegroundColor Green
        }
    } catch {
        .\Write-Info.ps1 "[WARN] DC status checker failed: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # -- Tools install (if not done in Step 1) --
    $toolsSignal = "$scriptsPath\InstallMSIAndTools.Completed.signal"
    if (-not (Test-Path $toolsSignal)) {
        .\Write-Info.ps1 "Installing tools..." -ForegroundColor Cyan
        & "$scriptsPath\InstallMSIAndTools.ps1" -Role 'DC'
    } else {
        .\Write-Info.ps1 "[OK] Tools already installed" -ForegroundColor Green
    }
    $toolsOk = $true

    # -- Start services --
    .\Write-Info.ps1 "Starting services..." -ForegroundColor Yellow
    foreach ($svcName in @('sstpsvc', 'rasman', 'RemoteAccess', 'NTDS', 'ADWS')) {
        $svc = Get-Service $svcName -ErrorAction SilentlyContinue
        if ($null -ne $svc -and $svc.Status -ne 'Running') {
            Start-Service $svcName -ErrorAction SilentlyContinue
            .\Write-Info.ps1 "  Started $svcName" -ForegroundColor DarkGray
        }
    }

    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "=== DC imperative steps (Step 2) completed (Accounts=$accountsOk, CBAC=$cbacOk, GPO=$gpoOk, DNS=$dnsOk, Tools=$toolsOk) ===" -ForegroundColor Cyan
    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "DC imperative step 2 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}
