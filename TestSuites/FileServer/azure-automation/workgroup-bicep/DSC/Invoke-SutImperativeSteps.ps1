# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Imperative steps for SUT that cannot be expressed in DSC.
    Run this AFTER the DSC configuration has been applied.

.DESCRIPTION
    Handles:
    - Disk partitioning (ReFS K:, FAT32 J:) via diskpart
    - ReFS/FAT32 SMB shares on data disk
    - Symbolic links and mount points
    - Shadow copies (VSS)
    - DFS namespace creation (dfsutil/dfscmd)
    - QUIC certificate mapping (Azure Edition only)
    - 8dot3 short name enable

    This script is idempotent -- it checks for existing state before acting.

.PARAMETER WorkingPath
    Path to the Workgroup-Package folder (default: script location).

.PARAMETER ConfigureFile
    Path to Config.json (default: WorkingPath\Config.json).

.EXAMPLE
    .\Invoke-SutImperativeSteps.ps1 -WorkingPath C:\Workgroup-Package
#>

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'Password originates from Azure deployment config; no interactive prompt available.')]
param(
    [string]$WorkingPath = (Split-Path $PSScriptRoot -Parent),
    [string]$ConfigureFile = "$WorkingPath\Config.json"
)

$ErrorActionPreference = 'Stop'
$parentPath = $WorkingPath
$scriptsPath = "$PSScriptRoot\Scripts"
$env:Path += ";$parentPath;$scriptsPath"
Push-Location $scriptsPath

[string]$logFile = "$PSScriptRoot\Invoke-SutImperativeSteps.log"
Start-Transcript -Path $logFile -Append -Force

$config = $null
if (Test-Path $ConfigureFile) {
    try { $config = Get-Content -Path $ConfigureFile -Raw | ConvertFrom-Json }
    catch { Write-Warning "Failed to parse config file: $_" }
}

$systemDrive = $env:SystemDrive

$accountsOk  = $false
$diskOk      = $false
$dataShareOk = $false
$symlinkOk   = $false
$fsaOk       = $false
$dfsOk       = $false
$quicOk      = $false

# ===========================================================================
# TLS Cipher Suite Configuration
# ===========================================================================
try {
    .\Write-Info.ps1 "Configuring TLS cipher suites..." -ForegroundColor Yellow
    $tlsResult = & "$scriptsPath\Configure-TlsCipherSuites.ps1"
    if ($tlsResult) {
        .\Write-Info.ps1 "[OK] TLS cipher suites configured" -ForegroundColor Green
    }
} catch {
    .\Write-Info.ps1 "[WARN] TLS cipher suite config failed: $($_.Exception.Message)" -ForegroundColor Yellow
}

# ===========================================================================
# 0. LOCAL TEST ACCOUNTS -- nonadmin, Guest, AzGroup01/AzUser01
#    In domain scenarios these are created on the DC by Create-TestAccount.ps1.
#    In workgroup mode there is no DC, so we create them locally on the SUT.
#    Create-TestAccount.ps1 already handles workgroup mode (uses net.exe
#    instead of AD cmdlets). It reads ParamConfig.json for the user/group list.
# ===========================================================================

try {
    $createAccountScript = "$scriptsPath\Create-TestAccount.ps1"
    if (Test-Path $createAccountScript) {
        .\Write-Info.ps1 "Creating local test accounts via Create-TestAccount.ps1..." -ForegroundColor Yellow
        $result = & $createAccountScript -workingDir $scriptsPath -protocolConfigFile $ConfigureFile
        if (-not $result) {
            .\Write-Info.ps1 "[WARN] Create-TestAccount.ps1 returned failure -- check Create-TestAccount.ps1.log" -ForegroundColor Yellow
        } else {
            .\Write-Info.ps1 "[OK] Local test accounts created" -ForegroundColor Green
            $accountsOk = $true
        }
    } else {
        .\Write-Info.ps1 "[WARN] Create-TestAccount.ps1 not found at $createAccountScript" -ForegroundColor Yellow
    }
}
catch {
    .\Write-Info.ps1 "[FAIL] Local test account creation failed: $($_.Exception.Message)" -ForegroundColor Red
}

# ===========================================================================
# 1. DISK PARTITIONING -- ReFS (K:) and FAT32 (J:)
# ===========================================================================

try {
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
            .\Write-Info.ps1 "[OK] $FileSystem volume '$Label' already exists on $($volume.DriveLetter)" -ForegroundColor Green
            return
        }

        .\Write-Info.ps1 "Creating $FileSystem partition ($Label -> ${DriveLetter}:)..." -ForegroundColor Yellow

        $osDiskNum = (Get-Partition | Where-Object { $_.DriveLetter -eq $systemDrive[0] } | Select-Object -First 1).DiskNumber
        $dataDisk = Get-Disk | Where-Object { $_.Number -ne $osDiskNum -and $_.OperationalStatus -eq 'Online' -and $_.Size -gt 5GB } | Select-Object -First 1

        if ($null -eq $dataDisk) {
            Write-Warning "No data disk found. $FileSystem partition will not be created. Attach a data disk >= 10GB."
            return
        }

        $diskNum = $dataDisk.Number

        # Initialize the disk if it's RAW (brand-new Azure data disk)
        $partStyle = (Get-Disk -Number $diskNum).PartitionStyle
        if ($partStyle -eq 'RAW') {
            .\Write-Info.ps1 "  Initializing RAW disk $diskNum as GPT..." -ForegroundColor DarkGray
            Initialize-Disk -Number $diskNum -PartitionStyle GPT -ErrorAction Stop
            $partStyle = 'GPT'
        }

        $diskPartCmd = @("select disk $diskNum")
        $diskPartCmd += "create partition primary size=$SizeMB"

        $formatCmd = "format fs=$FileSystem quick label=$Label"
        if ($ClusterSize) { $formatCmd += " unit=$ClusterSize" }
        $diskPartCmd += $formatCmd
        $diskPartCmd += "assign letter=$DriveLetter"

        $diskPartCmd | diskpart.exe
        Start-Sleep -Seconds 3
        $verify = Get-CimInstance -ClassName Win32_Volume |
            Where-Object { $_.FileSystem -eq $FileSystem -and $_.Label -eq $Label }
        if ($null -eq $verify) { throw "Volume not created" }
        .\Write-Info.ps1 "[OK] $FileSystem partition created on ${DriveLetter}:" -ForegroundColor Green
    }

    New-DataPartition -FileSystem 'ReFS' -Label 'REFS' -DriveLetter 'K' -SizeMB 2000 -ClusterSize '64K'
    New-DataPartition -FileSystem 'FAT32' -Label 'FAT32' -DriveLetter 'J' -SizeMB 2000
    $diskOk = $true
}
catch {
    .\Write-Info.ps1 "[FAIL] Disk partitioning failed: $($_.Exception.Message)" -ForegroundColor Red
}

# ===========================================================================
# 2. DATA DISK SMB SHARES
# ===========================================================================

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
            } else {
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

# ===========================================================================
# 3. SYMBOLIC LINKS AND MOUNT POINTS
# ===========================================================================

try {
    $symlinks = @(
        @{ Link = "$systemDrive\SMBBasic\symboliclink";      Target = "$systemDrive\FileShare\" },
        @{ Link = "$systemDrive\SMBBasic\sub\symboliclink2"; Target = "$systemDrive\FileShare\" }
    )

    foreach ($sl in $symlinks) {
        if (-not (Test-Path $sl.Link)) {
            cmd /C "mklink /D `"$($sl.Link)`" `"$($sl.Target)`"" 2>&1 | .\Write-Info.ps1
        } else {
            .\Write-Info.ps1 "[OK] Symlink $($sl.Link) exists" -ForegroundColor Green
        }
    }
    $symlinkOk = $true
}
catch {
    .\Write-Info.ps1 "[FAIL] Symbolic links failed: $($_.Exception.Message)" -ForegroundColor Red
}

# ===========================================================================
# 4. FSA ENVIRONMENT -- ExistingFile, links, mount points, shadow copies
# ===========================================================================

try {
    function Set-FsaShareFolder {
        param(
            [string]$Path,
            [string]$FolderName
        )

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

        # Mount point (NTFS/ReFS only)
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

        # 8dot3 short names (NTFS only)
        if ($fileSystem -eq 'NTFS') {
            cmd /C "fsutil 8dot3name set $Path 0" 2>&1 | .\Write-Info.ps1
        }

        # Shadow copies (not FAT32)
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

# ===========================================================================
# 5. DFS NAMESPACES
# ===========================================================================

try {
    $computerName = $env:COMPUTERNAME

    $dfsSvc = Get-Service dfs -ErrorAction SilentlyContinue
    if ($null -ne $dfsSvc -and $dfsSvc.Status -ne 'Running') {
        Start-Service dfs -ErrorAction SilentlyContinue
    }

    $dfsRoot = "\\$computerName\SMBDfs"
    $dfsCheck = dfsutil root $dfsRoot 2>&1
    if ($dfsCheck -match 'error|not found|does not exist') {
        .\Write-Info.ps1 "Creating DFS namespace $dfsRoot..." -ForegroundColor Yellow
        dfsutil root addstd $dfsRoot 2>&1 | .\Write-Info.ps1
        dfscmd /map "$dfsRoot\SMBDfsLink" "\\$computerName\SMBBasic" /restore 2>&1 | .\Write-Info.ps1
    } else {
        .\Write-Info.ps1 "[OK] DFS namespace $dfsRoot exists" -ForegroundColor Green
    }

    $standaloneRoot = "\\$computerName\Standalone"
    $saCheck = dfsutil root $standaloneRoot 2>&1
    if ($saCheck -match 'error|not found|does not exist') {
        .\Write-Info.ps1 "Creating DFS namespace $standaloneRoot..." -ForegroundColor Yellow
        dfsutil root addstd $standaloneRoot 2>&1 | .\Write-Info.ps1
        dfscmd /map "$standaloneRoot\DFSLink" "\\$computerName\FileShare" /restore 2>&1 | .\Write-Info.ps1
        dfscmd /map "$standaloneRoot\Interlink" "$dfsRoot\SMBDfsLink" /restore 2>&1 | .\Write-Info.ps1
    } else {
        .\Write-Info.ps1 "[OK] DFS namespace $standaloneRoot exists" -ForegroundColor Green
    }
    $dfsOk = $true
}
catch {
    .\Write-Info.ps1 "[FAIL] DFS namespace setup failed: $($_.Exception.Message)" -ForegroundColor Red
}

# ===========================================================================
# 6. QUIC ENVIRONMENT (Azure Edition only)
# ===========================================================================

try {
    $osName = (Get-CimInstance Win32_OperatingSystem).Name
    if ($osName -match 'Azure Edition') {

        $sutName = $env:COMPUTERNAME
        $sutComputerName = $sutName  # Workgroup -- no domain suffix

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

            # Import to root store
            if ($null -eq $config -or [string]::IsNullOrEmpty($config.Core.Password)) {
                throw "Config.json Core.Password is required for QUIC certificate export"
            }
            $certPassword = ConvertTo-SecureString $config.Core.Password -AsPlainText -Force
            Export-PfxCertificate -Cert $cert -FilePath "$systemDrive\QUICCert.pfx" -Password $certPassword | Out-Null
            Import-PfxCertificate -FilePath "$systemDrive\QUICCert.pfx" -CertStoreLocation Cert:\LocalMachine\Root -Password $certPassword | Out-Null

            Set-SmbServerConfiguration -DisableSmbEncryptionOnSecureConnection $false -Confirm:$false
            Set-SmbServerConfiguration -RestrictNamedpipeAccessViaQuic $false -Confirm:$false
            .\Write-Info.ps1 "[OK] QUIC configured" -ForegroundColor Green
        } else {
            .\Write-Info.ps1 "[OK] QUIC certificate mapping already exists for $sutComputerName" -ForegroundColor Green
        }
    } else {
        .\Write-Info.ps1 "[SKIP] QUIC -- not Azure Edition" -ForegroundColor DarkGray
    }
    $quicOk = $true
}
catch {
    .\Write-Info.ps1 "[FAIL] QUIC environment setup failed: $($_.Exception.Message)" -ForegroundColor Red
}

# ===========================================================================
# Done
# ===========================================================================
.\Write-Info.ps1 ""
.\Write-Info.ps1 "=== SUT imperative steps completed (Accounts=$accountsOk, Disk=$diskOk, DataShares=$dataShareOk, Symlinks=$symlinkOk, FSA=$fsaOk, DFS=$dfsOk, QUIC=$quicOk) ===" -ForegroundColor Cyan

# Critical sections: FSA and DFS must succeed for tests to work.
# Throw if either failed so the parent orchestrator catches the failure.
$criticalFailures = @()
if (-not $accountsOk) { $criticalFailures += 'Accounts' }
if (-not $fsaOk)  { $criticalFailures += 'FSA' }
if (-not $dfsOk)  { $criticalFailures += 'DFS' }

if ($criticalFailures.Count -gt 0) {
    Stop-Transcript
    Pop-Location
    throw "Critical section(s) failed: $($criticalFailures -join ', '). Check Invoke-SutImperativeSteps.log for details."
}

Stop-Transcript
Pop-Location
