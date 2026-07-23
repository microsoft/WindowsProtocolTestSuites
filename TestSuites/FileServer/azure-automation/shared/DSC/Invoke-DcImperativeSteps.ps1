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
$domainAdminOk = $false

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

    # Wait for AD DS to be *fully operational*, not just ADWS-responsive. A freshly-promoted
    # DC answers Get-ADDomain within seconds but still reports "server is not operational" for
    # AD-object and GPO writes for minutes -- until SYSVOL/NETLOGON are shared and the DC is
    # advertising. Gate all AD-dependent work (CBAC objects, claims GPO) behind this so those
    # steps run once, reliably, instead of failing early and leaning on retries.
    .\Write-Info.ps1 "Waiting for AD DS to be fully operational (ADWS read+WRITE, SYSVOL, NETLOGON, advertising)..." -ForegroundColor Cyan
    $domainDns = (Get-CimInstance Win32_ComputerSystem).Domain
    $domain = $null
    $adOperational = $false
    $consecutive = 0
    $requiredConsecutive = 3   # must pass several times in a row -- a single Get-ADDomain
                               # success flickers True for minutes post-promotion while AD
                               # writes still throw "server is not operational".
    for ($i = 0; $i -lt 60 -and -not $adOperational; $i++) {
        $adwsOk = $false
        try { $domain = Get-ADDomain -ErrorAction Stop; $adwsOk = ($null -ne $domain) } catch { $adwsOk = $false }
        $sysvolOk   = Test-Path "\\$($env:COMPUTERNAME)\SYSVOL"
        $netlogonOk = Test-Path "\\$($env:COMPUTERNAME)\NETLOGON"
        & nltest "/dsgetdc:$domainDns" 2>&1 | Out-Null
        $advertOk = ($LASTEXITCODE -eq 0)

        # Actual AD WRITE probe: "server is not operational" specifically affects writes, so a
        # read (Get-ADDomain) passing is NOT enough -- CBAC and claims-GPO both write to AD.
        # Create then remove a throwaway object to prove the directory accepts writes right now.
        $writeOk = $false
        if ($adwsOk) {
            $probeName = "zzReadyProbe$([guid]::NewGuid().ToString('N').Substring(0,8))"
            try {
                $probePath = "CN=Users,$($domain.DistinguishedName)"
                New-ADObject -Name $probeName -Type 'contact' -Path $probePath -ErrorAction Stop
                Remove-ADObject -Identity "CN=$probeName,$probePath" -Confirm:$false -ErrorAction Stop
                $writeOk = $true
            } catch { $writeOk = $false }
        }

        if ($adwsOk -and $sysvolOk -and $netlogonOk -and $advertOk -and $writeOk) {
            $consecutive++
            if ($consecutive -ge $requiredConsecutive) { $adOperational = $true; break }
            .\Write-Info.ps1 "  AD operational check passed ($consecutive/$requiredConsecutive consecutive); confirming stability..." -ForegroundColor DarkGray
            Start-Sleep 10
        } else {
            $consecutive = 0
            .\Write-Info.ps1 "  DC not fully operational yet (ADWS=$adwsOk Write=$writeOk SYSVOL=$sysvolOk NETLOGON=$netlogonOk Advertising=$advertOk); waiting 15s..." -ForegroundColor DarkGray
            Start-Sleep 15
        }
    }
    if (-not $adOperational) {
        throw "AD DS did not become stably operational within ~15 minutes (ADWS read+write / SYSVOL / NETLOGON / advertising, $requiredConsecutive consecutive passes). CBAC and claims-GPO setup would fail; failing Step 2 so the DC does not signal readiness."
    }
    .\Write-Info.ps1 "[OK] AD DS is fully operational" -ForegroundColor Green

    # -- Account Lockout Policy -- DISABLE for domain test accounts --
    # The Auth/FileServer suites run negative-auth and rapid re-authentication cases; on a
    # Server 2025 DC the default domain policy inherits the new baseline (lockout threshold 10,
    # a Win11 22H2 change) so those cases lock the DOMAIN test accounts, and every subsequent
    # SESSION_SETUP returns 0xC0000234 (STATUS_ACCOUNT_LOCKED_OUT). Domain accounts are governed
    # by the DC's default domain policy (not the member's local policy), so pin the threshold to
    # 0 here. Idempotent; safe on older OSes (threshold was already 0).
    try {
        .\Write-Info.ps1 "Disabling domain account lockout policy..." -ForegroundColor Yellow
        Set-ADDefaultDomainPasswordPolicy -Identity $domainDns -LockoutThreshold 0 -ErrorAction Stop
        .\Write-Info.ps1 "[OK] Domain account lockout disabled (threshold 0)" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[WARN] Could not disable domain account lockout: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # NOTE: The DC does NOT set Netlogon\RefusePasswordChange. Each domain member is
    # responsible for its own machine-account password stability via DisablePasswordChange
    # (see the member imperative-step scripts). Setting RefusePasswordChange on the DC is
    # unsafe -- Microsoft warns it can break secure channels for ANY member that still
    # rotates its password, producing "trust relationship failed" errors. Keeping the
    # protection member-side only avoids that failure mode.

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
        # Mandatory: all deploy/test scripts run as this account with full domain admin
        # rights. Retry to ride out transient ADWS "server is not operational" flicker on a
        # freshly-promoted DC (same rationale as CBAC/GPO). $domainAdminOk gates readiness.
        for ($try = 1; $try -le 5 -and -not $domainAdminOk; $try++) {
            try {
                $adUser = Get-ADUser -Identity $adminUser -ErrorAction Stop
                $isDomainAdmin = (Get-ADGroupMember -Identity 'Domain Admins' -ErrorAction Stop |
                    Where-Object { $_.SamAccountName -eq $adminUser })
                if (-not $isDomainAdmin) {
                    Add-ADGroupMember -Identity 'Domain Admins' -Members $adUser -ErrorAction Stop
                    .\Write-Info.ps1 "[OK] Added '$adminUser' to Domain Admins" -ForegroundColor Green
                } else {
                    .\Write-Info.ps1 "[OK] '$adminUser' already in Domain Admins" -ForegroundColor Green
                }
                $domainAdminOk = $true
            } catch {
                .\Write-Info.ps1 "  Domain Admins attempt $try/5 failed: $($_.Exception.Message)" -ForegroundColor DarkGray
                if ($try -lt 5) { Start-Sleep 20 }
            }
        }
    }
    if (-not $domainAdminOk) {
        .\Write-Info.ps1 "[FAIL] Could not ensure '$adminUser' in Domain Admins after retries -- deploy/test scripts run as this account." -ForegroundColor Red
    }

    # -- Test Accounts -- retry to ride out transient ADWS flicker on a freshly-promoted DC
    # (same rationale as CBAC/GPO/DomainAdmin). Create-TestAccount is idempotent (handles
    # pre-existing accounts), so reruns are safe.
    .\Write-Info.ps1 "Creating test accounts..." -ForegroundColor Yellow
    for ($try = 1; $try -le 5 -and -not $accountsOk; $try++) {
        try {
            # Capture only the final return value: Create-TestAccount emits stray success-stream
            # output (net.exe, Format-Table), so a raw capture would be a truthy array that masks
            # a real failure. The script returns $true on completion and throws on hard failure.
            $accountResult = & "$scriptsPath\Create-TestAccount.ps1" | Select-Object -Last 1
            $accountsOk = ($accountResult -eq $true)
        } catch {
            .\Write-Info.ps1 "  Test accounts attempt $try/5 failed: $($_.Exception.Message)" -ForegroundColor DarkGray
            if ($try -lt 5) { Start-Sleep 20 }
        }
    }
    if ($accountsOk) {
        .\Write-Info.ps1 "[OK] Test accounts created" -ForegroundColor Green
    } else {
        .\Write-Info.ps1 "[FAIL] Create-TestAccount failed after retries -- auth/logon tests would fail." -ForegroundColor Red
    }

    # -- CBAC Objects -- Even past the readiness gate, ADWS/AD writes can briefly flicker
    # "server is not operational" on a freshly-promoted DC, so retry. Create-CbacObjectsInDC.ps1
    # is idempotent, making reruns safe.
    .\Write-Info.ps1 "Creating CBAC objects..." -ForegroundColor Yellow
    $cbacOk = $false
    for ($try = 1; $try -le 5 -and -not $cbacOk; $try++) {
        try {
            $cbacOk = [bool](& "$scriptsPath\Create-CbacObjectsInDC.ps1")
        } catch {
            .\Write-Info.ps1 "  CBAC attempt $try/5 failed: $($_.Exception.Message)" -ForegroundColor DarkGray
        }
        if (-not $cbacOk -and $try -lt 5) { Start-Sleep 20 }
    }
    if ($cbacOk) { .\Write-Info.ps1 "[OK] CBAC objects created" -ForegroundColor Green }
    else { .\Write-Info.ps1 "[FAIL] Create-CbacObjectsInDC failed after retries -- CBAC/Authorization tests would fail." -ForegroundColor Red }

    # -- GPO Import (claims) -- same transient-write retry.
    .\Write-Info.ps1 "Importing GPO for claims..." -ForegroundColor Yellow
    $gpoOk = $false
    for ($try = 1; $try -le 5 -and -not $gpoOk; $try++) {
        try {
            $gpoOk = [bool](& "$scriptsPath\Import-GPOForClaims.ps1")
        } catch {
            .\Write-Info.ps1 "  GPO import attempt $try/5 failed: $($_.Exception.Message)" -ForegroundColor DarkGray
        }
        if (-not $gpoOk -and $try -lt 5) { Start-Sleep 20 }
    }
    if ($gpoOk) { .\Write-Info.ps1 "[OK] GPO imported" -ForegroundColor Green }
    else { .\Write-Info.ps1 "[FAIL] Import-GPOForClaims failed after retries -- claims/CBAC tests would fail." -ForegroundColor Red }

    # -- DNS Forwarder (Azure recursive resolver) --
    # AD DNS on Azure cannot reliably resolve external names via root hints, so domain
    # members (which use this DC as their only DNS server) can't resolve public hosts
    # e.g. to download packages. Add the Azure platform resolver 168.63.129.16 as a
    # forwarder so external resolution works. Idempotent; safe for all domain/cluster
    # deploys (CLI and the one-click button).
    .\Write-Info.ps1 "Configuring DNS forwarder to Azure resolver (168.63.129.16)..." -ForegroundColor Yellow
    try {
        $existingFwd = Get-DnsServerForwarder -ErrorAction SilentlyContinue
        if ($existingFwd.IPAddress.IPAddressToString -notcontains '168.63.129.16') {
            Add-DnsServerForwarder -IPAddress '168.63.129.16' -PassThru -ErrorAction Stop | Out-Null
            .\Write-Info.ps1 "[OK] DNS forwarder 168.63.129.16 added" -ForegroundColor Green
        } else {
            .\Write-Info.ps1 "[OK] DNS forwarder 168.63.129.16 already present" -ForegroundColor Green
        }
    } catch {
        .\Write-Info.ps1 "[WARN] DNS forwarder config failed: $($_.Exception.Message)" -ForegroundColor Yellow
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
    # Postcondition: InstallMSIAndTools writes its Completed.signal only after all tools
    # install (PowerShellCore, OpenSSH, etc.), which PS-over-SSH remoting depends on.
    $toolsOk = Test-Path $toolsSignal
    if (-not $toolsOk) {
        .\Write-Info.ps1 "[FAIL] Tools install did not complete (no InstallMSIAndTools.Completed.signal)." -ForegroundColor Red
    }

    # -- Start services --
    .\Write-Info.ps1 "Starting services..." -ForegroundColor Yellow
    foreach ($svcName in @('sstpsvc', 'rasman', 'RemoteAccess', 'NTDS', 'ADWS')) {
        $svc = Get-Service $svcName -ErrorAction SilentlyContinue
        if ($null -ne $svc -and $svc.Status -ne 'Running') {
            Start-Service $svcName -ErrorAction SilentlyContinue
            .\Write-Info.ps1 "  Started $svcName" -ForegroundColor DarkGray
        }
    }

    # -- SSH server authorized_keys (PowerShell-over-SSH remoting for control adapters) --
    # The Authorization/permission control adapters remote to the DC via `Invoke-Command
    # -HostName` (PS over SSH) to resolve group/user SIDs. Windows OpenSSH reads
    # administrators_authorized_keys for the domain admin, which the SSH-certs tool does not
    # populate -- without it sshd falls back to an interactive password prompt and the tests
    # hang (no SMB/KDC traffic, flat CPU). Install + verify the trusted key; fail loudly if
    # it cannot be established.
    $sshKeysOk = $false
    try {
        .\Write-Info.ps1 "Configuring SSH authorized_keys for PowerShell-over-SSH remoting..." -ForegroundColor Yellow
        $sshKeysOk = [bool](& "$scriptsPath\Set-SshServerAuthorizedKeys.ps1" -Config $config)
    } catch {
        .\Write-Info.ps1 "[WARN] SSH authorized_keys setup error: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    .\Write-Info.ps1 ""
    .\Write-Info.ps1 "=== DC imperative steps (Step 2) completed (Accounts=$accountsOk, DomainAdmin=$domainAdminOk, CBAC=$cbacOk, GPO=$gpoOk, DNS=$dnsOk, Tools=$toolsOk, SshKeys=$sshKeysOk) ===" -ForegroundColor Cyan

    # Gate readiness: a DC that failed CBAC/GPO/SSH provisioning must NOT report ready, or the
    # deployment proceeds to run tests against a half-provisioned DC (empty CAPs -> CBAC
    # KeyNotFound; no PS-over-SSH -> SID lookups return null). Fail Step 2 so Deploy-DC skips
    # writing the completion signal.
    $provisioningFailures = @()
    if (-not $accountsOk)    { $provisioningFailures += 'test accounts (Create-TestAccount) -- auth/logon tests would fail' }
    if (-not $domainAdminOk) { $provisioningFailures += "Domain Admins membership for '$adminUser' -- deploy/test scripts run as this account" }
    if (-not $toolsOk)       { $provisioningFailures += 'tools install (InstallMSIAndTools) -- PS-over-SSH remoting unavailable' }
    if (-not $cbacOk)    { $provisioningFailures += 'CBAC objects (Create-CbacObjectsInDC)' }
    if (-not $gpoOk)     { $provisioningFailures += 'claims GPO (Import-GPOForClaims)' }
    if (-not $sshKeysOk) { $provisioningFailures += 'SSH remoting authorized_keys (control adapters would hang / return null)' }
    if ($provisioningFailures.Count -gt 0) {
        throw ("DC post-promotion provisioning incomplete: " + ($provisioningFailures -join '; ') +
            ". Failing Step 2 so the DC does not signal readiness and the deployment does not proceed to tests against a half-provisioned DC.")
    }
    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "DC imperative step 2 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}
