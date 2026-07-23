# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Imperative steps for the Domain SUT (Node01).
    Run AFTER DSC configuration has been applied.

.DESCRIPTION
    Supports a -Step parameter (orchestrated by Deploy-SUT.ps1):

    Step 1 -- Pre-reboot:
      Domain join (requires DC to be ready), then reboot.

    Step 3 -- Post-features-reboot:
      Disk partitioning (ReFS K:, FAT32 J:), symbolic links, mount points,
      shadow copies, DFS namespaces (standalone + domain-based),
      QUIC cert mapping, ForceLevel2.

    NOTE: Tools installation (step 2 in the deployment flow) is handled
    directly by Deploy-SUT.ps1 via a background job running
    InstallMSIAndTools.ps1, not through this script.

.PARAMETER WorkingPath
    Path to the Domain-Package folder.

.PARAMETER Step
    Which step to execute (1 or 3).

.EXAMPLE
    .\Invoke-SutImperativeSteps.ps1 -Step 1   # domain join -> reboot
    .\Invoke-SutImperativeSteps.ps1 -Step 3   # shares, DFS, QUIC, FSA
#>

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'Password originates from Azure deployment config; no interactive prompt available.')]
param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [ValidateSet(1, 3)]
    [int]$Step = 1,
    [string]$ConfigureFile = "$WorkingPath\Config.json"
)

$ErrorActionPreference = 'Stop'
$scriptsPath = "$PSScriptRoot\Scripts"
$env:Path += ";$WorkingPath;$scriptsPath"
Push-Location $scriptsPath

[string]$logFile = "$PSScriptRoot\Invoke-SutImperativeSteps.log"
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
        # Capture only the script's final return value (see domainjoin.ps1): stray
        # success-stream output makes a multi-element array truthy even on a failed join.
        $result = & "$scriptsPath\domainjoin.ps1" | Select-Object -Last 1
        if ($result -ne $true) {
            .\Write-Error.ps1 "Domain join failed."
            Stop-Transcript; Pop-Location; return $false
        }
        .\Write-Info.ps1 "[OK] Domain join complete. REBOOT REQUIRED." -ForegroundColor Green
    }
    Stop-Transcript; Pop-Location; return $true
  }
  catch {
    .\Write-Error.ps1 "SUT imperative step 1 failed: $($_.Exception.Message)"
    Stop-Transcript; Pop-Location; throw
  }
}

# ===========================================================================
# STEP 3: Environment Configuration (post-features reboot)
# NOTE: Tools installation (step 2 in the deployment flow) is handled
# directly by Deploy-SUT.ps1 via a background job, not this script.
# ===========================================================================
if ($Step -eq 3) {
  $diskOk      = $false
  $dataShareOk = $false
  $symlinkOk   = $false
  $fsaOk       = $false
  $dfsOk       = $false
  $quicOk      = $false
  $computerPwdOk = $false
  $sshKeysOk   = $false

  try {
    .\Write-Info.ps1 "---- Step 3: Environment Configuration ----" -ForegroundColor Yellow

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

    # -- Account Lockout Policy (local) -- DISABLE on the test SUT --
    # The FileServer suites run negative-auth / rapid re-auth cases. Windows Server 2025
    # clean installs enable account lockout by default (threshold 10) -- a Win11 22H2
    # baseline change; older images shipped it disabled (0), which the suites assume.
    # Without this, tests trip the threshold and every subsequent SESSION_SETUP returns
    # 0xC0000234 (STATUS_ACCOUNT_LOCKED_OUT). This covers local SUT accounts; DOMAIN test
    # accounts are governed by the DC's default domain policy (see Invoke-DcImperativeSteps.ps1).
    try {
        .\Write-Info.ps1 "Disabling local account lockout policy (test SUT)..." -ForegroundColor Yellow
        & net accounts /lockoutthreshold:0 2>&1 | .\Write-Info.ps1
        .\Write-Info.ps1 "[OK] Local account lockout disabled (threshold 0)" -ForegroundColor Green
    } catch {
        .\Write-Info.ps1 "[WARN] Could not disable local account lockout: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # The Auth/Kerberos suite derives the file server's service key from
    # ServicePassword (Password04!) + ServiceSaltString, and that key only matches
    # the SUT if the computer-account password is Password04! on BOTH the local LSA
    # secret AND in AD. If the two diverge, the KDC issues service tickets the SUT
    # cannot decrypt: the Kerberos AP exchange then BLOCKS the SMB2 SESSION_SETUP
    # response, surfacing as a multi-hour Auth test hang instead of a clean failure.
    # ksetup /SetComputerPassword sets both sides in one operation (the pipeline's
    # canonical approach in Set-ComputerPassword.ps1) -- but only when the DC is
    # reachable. So wait for the DC, set, VERIFY the secure channel, retry, and treat
    # failure as a critical section so the problem surfaces at deploy time.
    .\Write-Info.ps1 "Setting computer password (ksetup) for Kerberos..." -ForegroundColor Yellow
    try {
        # Sync the machine-account password to the Kerberos constant Password04! on BOTH
        # the local LSA secret AND in AD. The Auth suite derives the file server's service
        # key from ServicePassword (Password04!) + salt; if AD's machine key is anything
        # else, service tickets fail the HMAC integrity check and the Kerberos AP exchange
        # blocks SMB2 SESSION_SETUP.
        #
        # IMPORTANT: do NOT gate success on Test-ComputerSecureChannel alone. It returns
        # True whenever the local secret == AD -- including when BOTH are still the OLD
        # domain-join password (ksetup's local change is pending a reboot). Trusting it
        # skips the AD-side reset, leaving AD != Password04!, which breaks Kerberos AND
        # breaks machine trust after the next reboot. So ALWAYS force AD explicitly first,
        # then the local secret, then restart Netlogon so the running LSA reloads to match.
        $secPw       = ConvertTo-SecureString 'Password04!' -AsPlainText -Force
        $adDomain    = (Get-CimInstance Win32_ComputerSystem).Domain
        $machineAcct = "$($env:COMPUTERNAME)$"

        # Idempotency: the marker is only ever written AFTER a forced AD reset + Netlogon
        # restart + live verification below, so a marker + verified channel is authoritative.
        $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'ComputerPasswordSet' -ErrorAction SilentlyContinue
        if ($null -ne $marker) {
            $secureOk = $false
            try { $secureOk = Test-ComputerSecureChannel -ErrorAction Stop } catch { $secureOk = $false }
            if ($secureOk) {
                $computerPwdOk = $true
                .\Write-Info.ps1 "[OK] Computer password already forced + verified (marker present) -- skipping" -ForegroundColor Green
            } else {
                Remove-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'ComputerPasswordSet' -ErrorAction SilentlyContinue
            }
        }

        if (-not $computerPwdOk) {
            if (-not (Get-Module -ListAvailable -Name ActiveDirectory -ErrorAction SilentlyContinue)) {
                try { Install-WindowsFeature RSAT-AD-PowerShell -ErrorAction Stop | Out-Null }
                catch { .\Write-Info.ps1 "[WARN] Could not install RSAT-AD-PowerShell: $($_.Exception.Message)" -ForegroundColor Yellow }
            }
            for ($attempt = 1; $attempt -le 10 -and -not $computerPwdOk; $attempt++) {
                .\Write-Info.ps1 "  Computer password sync attempt $attempt/10..." -ForegroundColor DarkGray

                # Wait for the DC to be reachable before touching the account.
                $dcReady = $false
                try { $dcReady = [bool](Get-ADDomainController -Discover -ErrorAction Stop) } catch { $dcReady = $false }
                if (-not $dcReady) {
                    & nltest "/dsgetdc:$adDomain" 2>&1 | Out-Null
                    $dcReady = ($LASTEXITCODE -eq 0)
                }
                if (-not $dcReady) {
                    .\Write-Info.ps1 "  DC not reachable yet; waiting 30s..." -ForegroundColor DarkGray
                    Start-Sleep -Seconds 30
                    continue
                }

                # 1) Force the AD copy to Password04! (authoritative). This is NOT a fallback:
                #    ksetup cannot be relied on to update AD, and this must run BEFORE
                #    DisablePasswordChange is set (which would block the AD update).
                try {
                    Set-ADAccountPassword -Identity $machineAcct -Reset -NewPassword $secPw -ErrorAction Stop
                } catch {
                    .\Write-Info.ps1 "  AD machine-password reset failed: $($_.Exception.Message); retrying in 30s..." -ForegroundColor DarkGray
                    Start-Sleep -Seconds 30
                    continue
                }

                # 2) Set the local LSA secret to the same value.
                $ksetupOut = ksetup /SetComputerPassword Password04! 2>&1
                $ksetupExit = $LASTEXITCODE
                $ksetupOut | .\Write-Info.ps1
                if ($ksetupExit -ne 0) {
                    .\Write-Info.ps1 "  ksetup failed (exit $ksetupExit); retrying in 30s..." -ForegroundColor DarkGray
                    Start-Sleep -Seconds 30
                    continue
                }

                # 3) Restart Netlogon so the RUNNING LSA reloads the on-disk secret (ksetup
                #    otherwise "requires a reboot to take effect"). This closes the window where
                #    running != on-disk that would break trust on the next reboot.
                try { Restart-Service Netlogon -Force -ErrorAction Stop } catch {
                    .\Write-Info.ps1 "  [WARN] Netlogon restart failed: $($_.Exception.Message)" -ForegroundColor Yellow
                }
                Start-Sleep -Seconds 5

                # 4) Verify the LIVE secure channel now that all three (AD, on-disk, running)
                #    should be Password04!.
                $secureOk = $false
                try { $secureOk = Test-ComputerSecureChannel -ErrorAction Stop } catch { $secureOk = $false }

                if ($secureOk) {
                    $computerPwdOk = $true
                    if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
                        New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
                    }
                    New-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'ComputerPasswordSet' -Value 1 -PropertyType DWord -Force | Out-Null
                    .\Write-Info.ps1 "[OK] Computer account password (Password04!) forced in AD + local, Netlogon restarted, secure channel verified" -ForegroundColor Green
                } else {
                    .\Write-Info.ps1 "  Secure channel not verified yet; retrying in 30s..." -ForegroundColor DarkGray
                    Start-Sleep -Seconds 30
                }
            }
            if (-not $computerPwdOk) {
                .\Write-Error.ps1 "Failed to force computer account password (Password04!) in AD + local / verify secure channel after 10 attempts. The Auth/Kerberos suite will hang without this."
            }
        }

        # 5) Only NOW prevent Netlogon from auto-rotating the (now-correct) password. Setting
        #    DisablePasswordChange BEFORE the sync can block the authoritative AD update above.
        if ($computerPwdOk) {
            & reg add 'HKLM\SYSTEM\CurrentControlSet\services\Netlogon\Parameters' /v DisablePasswordChange /t REG_DWORD /d 1 /f 2>&1 | .\Write-Info.ps1
        }
    } catch {
        .\Write-Info.ps1 "[WARN] Computer password setup error: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # -- SSH server authorized_keys (PowerShell-over-SSH remoting for control adapters) --
    # The Authorization/permission control adapters (GetGroupSid, GetUserSid, GetGroups, ...)
    # reach the SUT/DC via `Invoke-Command -HostName` (PS remoting over SSH). Windows OpenSSH
    # reads administrators_authorized_keys for the domain admin, which the SSH-certs tool does
    # not populate -- without it sshd falls back to an interactive password prompt and the whole
    # Authorization test group hangs (no SMB/KDC traffic, flat CPU). Install + verify as critical.
    try {
        .\Write-Info.ps1 "Configuring SSH authorized_keys for PowerShell-over-SSH remoting..." -ForegroundColor Yellow
        $sshKeysOk = [bool](& "$scriptsPath\Set-SshServerAuthorizedKeys.ps1" -Config $config)
    } catch {
        .\Write-Info.ps1 "[WARN] SSH authorized_keys setup error: $($_.Exception.Message)" -ForegroundColor Yellow
    }

    # -- Initialize RAW Azure disks --
    $rawDisks = Get-Disk | Where-Object { $_.PartitionStyle -eq 'RAW' -and $_.Size -gt 0 }
    foreach ($d in $rawDisks) {
        .\Write-Info.ps1 "  Initializing RAW disk $($d.Number) as GPT..." -ForegroundColor DarkGray
        Initialize-Disk -Number $d.Number -PartitionStyle GPT -ErrorAction Stop
    }

    # -- Disk Partitioning --
    .\Write-Info.ps1 "Setting up disk partitions..." -ForegroundColor Yellow

    function New-DataPartition {
        param(
            [string]$FileSystem,
            [string]$Label,
            [string]$DriveLetter,
            [int]$SizeMB = 2000,
            [string]$ClusterSize = ''
        )

        $volume = Get-CimInstance -ClassName Win32_Volume |
            Where-Object { $_.FileSystem -eq $FileSystem -and $_.Label -eq $Label }

        if ($null -ne $volume) {
            .\Write-Info.ps1 "[OK] $FileSystem volume '$Label' already exists" -ForegroundColor Green
            return
        }

        .\Write-Info.ps1 "Creating $FileSystem partition ($Label -> ${DriveLetter}:)..." -ForegroundColor Yellow

        $osDiskNum = (Get-Partition | Where-Object { $_.DriveLetter -eq $systemDrive[0] } | Select-Object -First 1).DiskNumber
        $dataDisk = Get-Disk | Where-Object { $_.Number -ne $osDiskNum -and $_.OperationalStatus -eq 'Online' -and $_.Size -gt 5GB } | Select-Object -First 1

        if ($null -eq $dataDisk) {
            Write-Warning "No data disk found for $FileSystem partition."
            return
        }

        $diskNum = $dataDisk.Number

        $diskPartCmd = @("select disk $diskNum")
        $diskPartCmd += "create partition primary size=$SizeMB"

        $formatCmd = "format fs=$FileSystem quick label=$Label"
        if ($ClusterSize) { $formatCmd += " unit=$ClusterSize" }
        $diskPartCmd += $formatCmd
        $diskPartCmd += "assign letter=$DriveLetter"

        try {
            $diskPartCmd | diskpart.exe
            Start-Sleep -Seconds 3
            $verify = Get-CimInstance -ClassName Win32_Volume |
                Where-Object { $_.FileSystem -eq $FileSystem -and $_.Label -eq $Label }
            if ($null -eq $verify) { throw "Volume not created" }
            .\Write-Info.ps1 "[OK] $FileSystem partition created on ${DriveLetter}:" -ForegroundColor Green
        }
        catch {
            Write-Warning "Failed to create $FileSystem partition: $($_.Exception.Message)"
        }
    }

    New-DataPartition -FileSystem 'ReFS' -Label 'REFS' -DriveLetter 'K' -SizeMB 2000 -ClusterSize '64K'
    New-DataPartition -FileSystem 'FAT32' -Label 'FAT32' -DriveLetter 'J' -SizeMB 2000
    $diskOk = $true
  }
  catch {
    .\Write-Info.ps1 "[FAIL] Disk partitioning failed: $($_.Exception.Message)" -ForegroundColor Red
  }

  # -- Data disk shares --
  try {
    $dataDiskShares = @(
        @{ Drive = 'K:\'; Name = 'SMBReFSShare'; Path = 'K:\SMBReFSShare' },
        @{ Drive = 'J:\'; Name = 'SMBFAT32Share'; Path = 'J:\SMBFAT32Share' }
    )
    foreach ($ds in $dataDiskShares) {
        if (Test-Path $ds.Drive) {
            $existing = Get-SmbShare -Name $ds.Name -ErrorAction SilentlyContinue
            if ($null -eq $existing) {
                New-Item -ItemType Directory -Path $ds.Path -Force | Out-Null
                icacls $ds.Path /grant "BUILTIN\Administrators:(OI)(CI)(F)" | Out-Null
                New-SmbShare -Name $ds.Name -Path $ds.Path -FullAccess 'BUILTIN\Administrators' -CachingMode None
                .\Write-Info.ps1 "[OK] Created share $($ds.Name)" -ForegroundColor Green
            }
            else {
                .\Write-Info.ps1 "[OK] Share $($ds.Name) already exists" -ForegroundColor Green
            }
        } else {
            Write-Warning "$($ds.Drive) not available -- skipping $($ds.Name)"
        }
    }
    $dataShareOk = $true
  }
  catch {
    .\Write-Info.ps1 "[FAIL] Data disk SMB shares failed: $($_.Exception.Message)" -ForegroundColor Red
  }

  # -- Symbolic Links --
  try {
    .\Write-Info.ps1 "Creating symbolic links..." -ForegroundColor Yellow
    $symlinks = @(
        @{ Link = "$systemDrive\SMBBasic\symboliclink";      Target = "$systemDrive\FileShare\" },
        @{ Link = "$systemDrive\SMBBasic\sub\symboliclink2"; Target = "$systemDrive\FileShare\" }
    )
    foreach ($sl in $symlinks) {
        if (-not (Test-Path $sl.Link)) {
            cmd /C "mklink /D `"$($sl.Link)`" `"$($sl.Target)`"" 2>&1 | .\Write-Info.ps1
        }
        else {
            .\Write-Info.ps1 "[OK] Symlink $($sl.Link) exists" -ForegroundColor Green
        }
    }
    $symlinkOk = $true
  }
  catch {
    .\Write-Info.ps1 "[FAIL] Symbolic links failed: $($_.Exception.Message)" -ForegroundColor Red
  }

  # -- FSA Environment --
  try {
    .\Write-Info.ps1 "Setting up FSA environment..." -ForegroundColor Yellow

    function Set-FsaShareFolder {
        param([string]$Path, [string]$FolderName)
        $shareFolder = "$Path\$FolderName"
        $driveLetter = $Path.TrimEnd(':', '\')
        $volume = Get-CimInstance -ClassName Win32_Volume | Where-Object { $_.DriveLetter -eq "${driveLetter}:" }
        $fileSystem = if ($volume) { $volume.FileSystem } else { 'Unknown' }

        .\Write-Info.ps1 "Setting up FSA for $shareFolder (filesystem: $fileSystem)..." -ForegroundColor Yellow

        New-Item -Type Directory -Path "$shareFolder\ExistingFolder" -Force | Out-Null
        if (-not (Test-Path "$shareFolder\ExistingFile.txt")) {
            New-Item -Type File -Path "$shareFolder\ExistingFile.txt" -Force | Out-Null
        }
        if ($fileSystem -ne 'FAT32' -and -not (Test-Path "$shareFolder\link.txt")) {
            cmd /C "mklink `"$shareFolder\link.txt`" `"$shareFolder\ExistingFile.txt`"" 2>&1 | .\Write-Info.ps1
        }
        if ($fileSystem -notin @('FAT32')) {
            $mpPath = "$shareFolder\MountPoint"
            if (-not (Test-Path $mpPath)) {
                New-Item -Type Directory -Path $mpPath -Force | Out-Null
                $volumeInfo = (mountvol $Path /l 2>&1) | Select-Object -First 1
                if ($volumeInfo -match '^\\\\\?\\') {
                    cmd /C "mountvol `"$mpPath`" $volumeInfo" 2>&1 | .\Write-Info.ps1
                }
            }
        }
        if ($fileSystem -eq 'NTFS') {
            cmd /C "fsutil 8dot3name set $Path 0" 2>&1 | .\Write-Info.ps1
        }
        if ($fileSystem -ne 'FAT32') {
            $shadows = vssadmin list shadows /for=${Path}\ 2>&1
            $shadowCount = ($shadows | Select-String 'Shadow Copy ID').Count
            $needed = 3 - $shadowCount
            for ($i = 0; $i -lt $needed; $i++) {
                Get-ChildItem $shareFolder >> "$shareFolder\ExistingFile.txt"
                vssadmin.exe Create Shadow /For=${Path}\ /AutoRetry=2 2>&1 | .\Write-Info.ps1
            }
            .\Write-Info.ps1 "[OK] $($shadowCount + $needed) shadow copies for ${Path}\" -ForegroundColor Green
        }
    }

    Set-FsaShareFolder -Path 'C:' -FolderName 'FileShare'
    if (Test-Path 'K:\') {
        Set-FsaShareFolder -Path 'K:' -FolderName 'SMBReFSShare'
    } else {
        Write-Warning "K: (ReFS) not available -- skipping FSA for SMBReFSShare"
    }
    if (Test-Path 'J:\') {
        Set-FsaShareFolder -Path 'J:' -FolderName 'SMBFAT32Share'
    } else {
        Write-Warning "J: (FAT32) not available -- skipping FSA for SMBFAT32Share"
    }
    $fsaOk = $true
  }
  catch {
    .\Write-Info.ps1 "[FAIL] FSA environment setup failed: $($_.Exception.Message)" -ForegroundColor Red
  }

  # -- DFS Namespaces --
  try {
    .\Write-Info.ps1 "Setting up DFS namespaces..." -ForegroundColor Yellow
    $computerName = $env:COMPUTERNAME
    $dfsSvc = Get-Service dfs -ErrorAction SilentlyContinue
    if ($null -ne $dfsSvc -and $dfsSvc.Status -ne 'Running') {
        Start-Service dfs -ErrorAction SilentlyContinue
    }

    # Standalone: SMBDfs
    $dfsRoot = "\\$computerName\SMBDfs"
    $dfsCheck = dfsutil root $dfsRoot 2>&1
    if ($dfsCheck -match 'error|not found|does not exist') {
        .\Write-Info.ps1 "Creating DFS namespace $dfsRoot..." -ForegroundColor Yellow
        dfsutil root addstd $dfsRoot 2>&1 | .\Write-Info.ps1
        dfscmd /map "$dfsRoot\SMBDfsLink" "\\$computerName\SMBBasic" /restore 2>&1 | .\Write-Info.ps1
    }
    else {
        .\Write-Info.ps1 "[OK] DFS namespace $dfsRoot exists" -ForegroundColor Green
    }

    # Standalone: Standalone
    $standaloneRoot = "\\$computerName\Standalone"
    $saCheck = dfsutil root $standaloneRoot 2>&1
    if ($saCheck -match 'error|not found|does not exist') {
        .\Write-Info.ps1 "Creating DFS namespace $standaloneRoot..." -ForegroundColor Yellow
        dfsutil root addstd $standaloneRoot 2>&1 | .\Write-Info.ps1
        dfscmd /map "$standaloneRoot\DFSLink" "\\$computerName\FileShare" /restore 2>&1 | .\Write-Info.ps1
        dfscmd /map "$standaloneRoot\Interlink" "$dfsRoot\SMBDfsLink" /restore 2>&1 | .\Write-Info.ps1
    }
    else {
        .\Write-Info.ps1 "[OK] DFS namespace $standaloneRoot exists" -ForegroundColor Green
    }

    # Domain-based (only if domain-joined)
    if ((Get-CimInstance Win32_ComputerSystem).PartOfDomain) {
        $domainBasedFile = "C:\DomainBased.txt"
        if (Test-Path $domainBasedFile) {
            $domainBasedNsName = (Get-Content $domainBasedFile -Raw).Trim()
            .\Write-Info.ps1 "[OK] Domain-based DFS already created: $domainBasedNsName" -ForegroundColor Green
        }
        else {
            $curFt = [DateTime]::UtcNow.ToFileTimeUtc()
            $curFtBytes = [BitConverter]::GetBytes($curFt)
            $suffix = if ([BitConverter]::IsLittleEndian) {
                "$([BitConverter]::ToUInt32($curFtBytes, 0))"
            } else {
                "$([BitConverter]::ToUInt32($curFtBytes, 4))"
            }
            $domainBasedNsName = "DomainBased$suffix"

            New-Item -ItemType Directory -Path "C:\DFSRoots\$domainBasedNsName" -Force | Out-Null
            $existing = Get-SmbShare -Name $domainBasedNsName -ErrorAction SilentlyContinue
            if ($null -eq $existing) {
                New-SmbShare -Name $domainBasedNsName -Path "C:\DFSRoots\$domainBasedNsName" -FullAccess 'BUILTIN\Administrators'
            }
            dfsutil root adddom "\\$computerName\$domainBasedNsName" 2>&1 | .\Write-Info.ps1
            dfscmd /map "\\$computerName\$domainBasedNsName\DFSLink" "\\$computerName\FileShare" /restore 2>&1 | .\Write-Info.ps1
            dfscmd /map "\\$computerName\$domainBasedNsName\Interlink" "$dfsRoot\SMBDfsLink" /restore 2>&1 | .\Write-Info.ps1

            Set-Content -Path $domainBasedFile -Value $domainBasedNsName
            .\Write-Info.ps1 "[OK] Domain-based DFS created: $domainBasedNsName" -ForegroundColor Green
        }
    }
    $dfsOk = $true
  }
  catch {
    .\Write-Info.ps1 "[FAIL] DFS namespace setup failed: $($_.Exception.Message)" -ForegroundColor Red
  }

  # -- QUIC Environment (Azure Edition only) --
  try {
    .\Write-Info.ps1 "Checking QUIC..." -ForegroundColor Yellow
    $osName = (Get-CimInstance Win32_OperatingSystem).Name
    if ($osName -match 'Azure Edition') {
        $sutName = $env:COMPUTERNAME
        $sut = $config.Machines.SUT
        if ($null -eq $sut) { $sut = $config.Machines.Node01 }
        $domain = if ($sut) { $sut.domain } else { '' }
        $sutComputerName = if ((-not [string]::IsNullOrEmpty($domain)) -and ($domain.ToLower() -ne 'workgroup')) {
            "$sutName.$domain".ToLower()
        } else { $sutName }

        $existingMapping = Get-SmbServerCertificateMapping -ErrorAction SilentlyContinue |
            Where-Object { $_.Name -eq $sutComputerName }
        if ($null -eq $existingMapping) {
            .\Write-Info.ps1 "Configuring QUIC certificate for $sutName..." -ForegroundColor Yellow
            $cert = New-SelfSignedCertificate -Subject $sutName `
                -FriendlyName "SMB over QUIC for File Servers" `
                -KeyUsageProperty Sign -KeyUsage DigitalSignature `
                -CertStoreLocation Cert:\LocalMachine\My `
                -HashAlgorithm SHA256 `
                -Provider "Microsoft Software Key Storage Provider" `
                -KeyAlgorithm ECDSA_P256 -KeyLength 256 `
                -DnsName @($sutComputerName, $sutName)

            New-SmbServerCertificateMapping -Name $sutComputerName `
                -Thumbprint $cert.Thumbprint -StoreName My -Subject $cert.Subject

            if (-not $config -or [string]::IsNullOrEmpty($config.Core.Password)) {
                throw "Config.json Core.Password is required for QUIC certificate export"
            }
            $certPassword = ConvertTo-SecureString $config.Core.Password -AsPlainText -Force
            Export-PfxCertificate -Cert $cert -FilePath "$systemDrive\QUICCert.pfx" -Password $certPassword | Out-Null
            Import-PfxCertificate -FilePath "$systemDrive\QUICCert.pfx" -CertStoreLocation Cert:\LocalMachine\Root -Password $certPassword | Out-Null

            Set-SmbServerConfiguration -DisableSmbEncryptionOnSecureConnection $false -Confirm:$false
            Set-SmbServerConfiguration -RestrictNamedpipeAccessViaQuic $false -Confirm:$false
            .\Write-Info.ps1 "[OK] QUIC configured for $sutComputerName" -ForegroundColor Green
        }
        else {
            .\Write-Info.ps1 "[OK] QUIC cert mapping already exists" -ForegroundColor Green
        }
    }
    else {
        .\Write-Info.ps1 "[SKIP] QUIC -- not Azure Edition" -ForegroundColor DarkGray
    }
    $quicOk = $true
  }
  catch {
    .\Write-Info.ps1 "[FAIL] QUIC environment setup failed: $($_.Exception.Message)" -ForegroundColor Red
  }

  .\Write-Info.ps1 ""
  .\Write-Info.ps1 "=== SUT imperative steps (Step 3) completed (Disk=$diskOk, DataShares=$dataShareOk, Symlinks=$symlinkOk, FSA=$fsaOk, DFS=$dfsOk, QUIC=$quicOk, ComputerPwd=$computerPwdOk, SshKeys=$sshKeysOk) ===" -ForegroundColor Cyan

  # Critical sections: FSA, DFS, the Kerberos computer-account password, and SSH remoting
  # must succeed for tests to work. A broken computer key or missing SSH key trust silently
  # hangs the Auth / Authorization suites for hours, so treat them as fatal at deploy time.
  $criticalFailures = @()
  if (-not $fsaOk)         { $criticalFailures += 'FSA' }
  if (-not $dfsOk)         { $criticalFailures += 'DFS' }
  if (-not $computerPwdOk) { $criticalFailures += 'ComputerPassword' }
  if (-not $sshKeysOk)     { $criticalFailures += 'SshRemoting' }

  if ($criticalFailures.Count -gt 0) {
    Stop-Transcript
    Pop-Location
    throw "Critical section(s) failed: $($criticalFailures -join ', '). Check Invoke-SutImperativeSteps.log for details."
  }

  Stop-Transcript; Pop-Location; return $true
}
