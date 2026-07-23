# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Makes this server accept key-based PowerShell-over-SSH remoting from the driver.

.DESCRIPTION
    The FileServer Authorization/permission control adapters (GetGroupSid, GetUserSid,
    GetGroups, GetGroupMembers, GetUsers, GetUserMemberships, ...) reach the DC/SUT via
    `Invoke-Command -HostName <server> -UserName <domain>\<admin>` -- PowerShell remoting
    over SSH. If the server does not trust the driver's key, sshd falls back to an
    INTERACTIVE password prompt that the non-interactive testhost can never answer, and
    the whole Authorization test group hangs indefinitely (no SMB/KDC traffic, flat CPU).

    The Win32-OpenSSH-Certs tool (win_ssh_keys.zip) already lands an `authorized_keys` on
    the server, but Windows OpenSSH ignores a user's ~/.ssh/authorized_keys for accounts in
    the Administrators group (the domain admin `testadmin` is one) -- it only reads
    %ProgramData%\ssh\administrators_authorized_keys, and only when that file has strict
    ACLs (Administrators + SYSTEM). This script installs the trusted key into that file
    with the required ACLs (and the per-user profiles as a fallback for non-admin-override
    sshd configs), then restarts sshd. Mirrors CommonScripts\Config-AuthorizedKeys.ps1 for
    the one-click Deploy-to-Azure environment.

.PARAMETER AdminUserName
    The admin account used for remoting (Config.json Core.Username, e.g. testadmin).

.PARAMETER DomainNetBiosName
    NetBIOS domain name (e.g. CONTOSO) so the domain user profile (testadmin.CONTOSO) is
    also covered. Empty for workgroup.

.OUTPUTS
    [bool] $true when the trusted key was installed into administrators_authorized_keys.
#>
[CmdletBinding()]
param(
    # Parsed Config.json object; the admin account and NetBIOS domain are resolved from it
    # when -AdminUserName / -DomainNetBiosName are not passed explicitly.
    [Parameter(Mandatory = $false)]
    [object]$Config,

    [Parameter(Mandatory = $false)]
    [string]$AdminUserName,

    [Parameter(Mandatory = $false)]
    [string]$DomainNetBiosName
)

$ErrorActionPreference = "Stop"
$sys = $env:SystemDrive

# Resolve the admin account + NetBIOS domain from Config.json (keeps every caller a one-liner).
if ([string]::IsNullOrWhiteSpace($AdminUserName) -and $Config) { $AdminUserName = $Config.Core.Username }
if ([string]::IsNullOrWhiteSpace($DomainNetBiosName) -and $Config) {
    if ($Config.Domain -and $Config.Domain.NetBiosName) {
        $DomainNetBiosName = $Config.Domain.NetBiosName
    } elseif (-not [string]::IsNullOrWhiteSpace($Config.Core.DomainName) -and $Config.Core.DomainName -ne 'Workgroup') {
        $DomainNetBiosName = ($Config.Core.DomainName -split '\.')[0]
    }
}
if ([string]::IsNullOrWhiteSpace($AdminUserName)) {
    throw "Set-SshServerAuthorizedKeys: -AdminUserName or -Config (with Core.Username) is required."
}
if ($null -eq $DomainNetBiosName) { $DomainNetBiosName = "" }

function Write-Log([string]$msg, [string]$color = "Gray") {
    # Route to the host, never the success stream: callers cast this script's output with
    # [bool](& ...), so any stray success-stream object would make a $false return look truthy.
    & "$PSScriptRoot\Write-Info.ps1" $msg -ForegroundColor $color | Out-Host
}

# --- 1. Locate the trusted authorized_keys the driver's key lives in -----------
# Canonical source: the Win32-OpenSSH-Certs tool (win_ssh_keys.zip) extracts authorized_keys
# to %SystemDrive%\OpenSSH-Win64. Look ONLY there (deterministic). Never scan the destination
# %ProgramData%\ssh -- that would be circular on a re-run (we write administrators_authorized_
# keys there) and could select a stale/unrelated key, silently installing the wrong trust and
# reintroducing the SSH password-prompt hang. The tools install runs as a background job, so
# wait for the artifact to land, and validate it actually contains SSH public keys.
$certRoot = "$sys\OpenSSH-Win64"
$primaryKeyPath = Join-Path $certRoot "authorized_keys"

function Test-LooksLikePublicKeys([string]$path) {
    if (-not (Test-Path $path)) { return $false }
    foreach ($l in (Get-Content $path -ErrorAction SilentlyContinue)) {
        if ($l -match '^\s*(ssh-(rsa|ed25519|dss)|ecdsa-sha2-\S+|sk-(ssh-ed25519|ecdsa-sha2-)\S*)\s+\S') { return $true }
    }
    return $false
}

$srcKeys = $null
for ($attempt = 1; $attempt -le 20 -and -not $srcKeys; $attempt++) {
    # Prefer the exact canonical path; fall back to a nested copy strictly under the cert root.
    if (Test-LooksLikePublicKeys $primaryKeyPath) {
        $srcKeys = $primaryKeyPath
    } elseif (Test-Path $certRoot) {
        $found = Get-ChildItem -Path $certRoot -Recurse -Filter "authorized_keys" -File -ErrorAction SilentlyContinue |
            Where-Object { Test-LooksLikePublicKeys $_.FullName } | Select-Object -First 1
        if ($found) { $srcKeys = $found.FullName }
    }
    if (-not $srcKeys) {
        Write-Log "  Valid authorized_keys not present under $certRoot yet (attempt $attempt/20); waiting 15s for the SSH-certs tool..." "DarkGray"
        Start-Sleep -Seconds 15
    }
}

if (-not $srcKeys) {
    Write-Log "[FAIL] No valid authorized_keys found under $certRoot (Win32-OpenSSH-Certs). Cannot enable PowerShell-over-SSH remoting; the Authorization test group would hang on a password prompt." "Red"
    return $false
}
Write-Log "Using trusted keys from: $srcKeys" "Gray"

# --- 2. Install into administrators_authorized_keys (the file sshd reads for -----
#        Administrators-group accounts) with the strict ACLs OpenSSH requires.
$sshProgramData = "$env:ProgramData\ssh"
if (-not (Test-Path $sshProgramData)) { New-Item -ItemType Directory -Path $sshProgramData -Force | Out-Null }
$adminKeys = Join-Path $sshProgramData "administrators_authorized_keys"

# Merge (don't clobber any existing trusted keys), then de-duplicate.
$existing = if (Test-Path $adminKeys) { Get-Content $adminKeys -ErrorAction SilentlyContinue } else { @() }
$merged = @($existing + (Get-Content $srcKeys)) |
    Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | Select-Object -Unique
Set-Content -Path $adminKeys -Value $merged -Encoding ascii -Force

# OpenSSH refuses the file unless only Administrators + SYSTEM have access.
$icaclsOutput = (& icacls $adminKeys /inheritance:r /grant "Administrators:F" /grant "SYSTEM:F" 2>&1 | Out-String).Trim()
if ($LASTEXITCODE -ne 0) {
    # icacls is native: a non-zero exit does not throw. Without this check the script would
    # log success and return $true even though OpenSSH will ignore the (wrongly-ACLed) file,
    # re-introducing the SSH password-prompt hang in the Authorization tests.
    Write-Log "[FAIL] Failed to set strict ACLs on $adminKeys (icacls exit $LASTEXITCODE). OpenSSH will ignore administrators_authorized_keys. Output: $icaclsOutput" "Red"
    return $false
}
Write-Log "[OK] administrators_authorized_keys installed with strict ACLs ($($merged.Count) key(s))" "Green"

# --- 3. Fallback: per-user authorized_keys (covers sshd configs without the ------
#        admin-override block). Best-effort; skip profiles that don't exist yet.
$userFolders = @()
if ($DomainNetBiosName) { $userFolders += "$sys\Users\$AdminUserName.$DomainNetBiosName" }
$userFolders += "$sys\Users\$AdminUserName"
foreach ($uf in ($userFolders | Select-Object -Unique)) {
    if (Test-Path $uf) {
        $uSsh = Join-Path $uf ".ssh"
        if (-not (Test-Path $uSsh)) { New-Item -ItemType Directory -Path $uSsh -Force | Out-Null }
        Copy-Item $srcKeys (Join-Path $uSsh "authorized_keys") -Force
        Write-Log "[OK] authorized_keys deployed to $uSsh" "Green"
    }
}

# --- 4. Restart sshd so the new keys take effect -------------------------------
$sshd = Get-Service sshd -ErrorAction SilentlyContinue
if ($sshd) {
    Restart-Service sshd -Force
    Write-Log "[OK] sshd restarted" "Green"
} else {
    Write-Log "[WARN] sshd service not found; keys are staged and will apply once OpenSSH starts." "Yellow"
}

return $true
