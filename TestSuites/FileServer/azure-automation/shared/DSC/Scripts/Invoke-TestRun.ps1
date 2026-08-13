# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Runs the FileServer test suite via Execute-TestCaseByContext.ps1 after both
    Driver and SUT deployments complete.

.DESCRIPTION
    This script is launched automatically by a scheduled task that fires ~30s
    after the Driver deployment completes (no login required). It:

    1. Reads Config.json to extract SUT name, IPs, and credentials.
    2. Waits for SUT readiness by polling for the Deploy-SUT.Completed.signal file
       via SMB admin share.
    3. Derives the context name (e.g., Win2025_Workgroup_NonCluster_SMB311).
    4. Calls Execute-TestCaseByContext.ps1, which handles:
       - Context-aware ptfconfig patching for all 9 ptfconfig files
       - SMB dialect capability filtering per Windows version
       - SMB2Model test sharding (4 parallel shards)
       - Per-filesystem FSA runs (NTFS/REFS/FAT32)
       - dotnet vstest with parallel execution
       - Named TRX outputs per component/filesystem
    5. Execute-TestCaseByContext.ps1 writes test.finished.signal on completion.

.PARAMETER WorkingPath
    Path to the Workgroup-Package root folder.

.PARAMETER Filter
    Optional test filter expression (e.g., "TestCategory=BVT" or test name).

.PARAMETER TestSuitePath
    Path to the installed test suite. Defaults to C:\FileServer-TestSuite-ServerEP.

.EXAMPLE
    .\Invoke-TestRun.ps1
    .\Invoke-TestRun.ps1 -Filter "TestCategory=BVT"
#>

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'Password originates from Azure deployment config; no interactive prompt available.')]
param(
    [string]$WorkingPath = (Split-Path (Split-Path $PSScriptRoot -Parent) -Parent),
    [string]$Filter,
    [string]$TestSuitePath = $(if ($IsLinux) { '/opt/FileServer-TestSuite-ServerEP' } else { 'C:\FileServer-TestSuite-ServerEP' }),
    [ValidateRange(1, 1440)]
    [int]$TestInvocationTimeoutMinutes = 60
)

$ErrorActionPreference = 'Stop'
$isLinuxDriver = $IsLinux -eq $true
$scriptsPath = $PSScriptRoot
$dscFolder   = Split-Path $PSScriptRoot -Parent
$logFile     = Join-Path $dscFolder 'Invoke-TestRun.log'

# ===========================================================================
# Helper function: Query SUT OS and map to context prefix
# ===========================================================================

function Get-SutPlatform {
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingUsernameAndPasswordParams', '',
        Justification = 'Credentials are used to build a CimSession for workgroup scenarios; separate strings required.')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
        Justification = 'Password originates from Azure deployment config; no interactive prompt available.')]
    <#
    .SYNOPSIS
        Queries the SUT OS version via CIM and maps it to a PTF platform string.
    #>
    param(
        [string]$SutComputerName,
        [string]$Username,
        [string]$Password,
        [string]$DomainNetBiosName
    )

    try {
        $cimSession = $null
        $cimParams = @{
            ClassName   = 'Win32_OperatingSystem'
            ErrorAction = 'Stop'
        }

        # In Domain/Cluster scenarios the script runs as DOMAIN\user, so Kerberos
        # authenticates implicitly — just use -ComputerName.
        # In Workgroup scenarios there's no Kerberos; we must create a CimSession
        # with explicit credentials over NTLM (Negotiate).
        if ($DomainNetBiosName) {
            # Domain context — implicit Kerberos
            $cimParams['ComputerName'] = $SutComputerName
        } elseif ($Username -and $Password) {
            # Workgroup — explicit credentials via CimSession
            $secPwd = ConvertTo-SecureString $Password -AsPlainText -Force
            $cred = [PSCredential]::new("$SutComputerName\$Username", $secPwd)
            $sessionOpt = New-CimSessionOption -Protocol Wsman
            $cimSession = New-CimSession -ComputerName $SutComputerName -Credential $cred `
                -SessionOption $sessionOpt -ErrorAction Stop
            $cimParams['CimSession'] = $cimSession
        } else {
            $cimParams['ComputerName'] = $SutComputerName
        }

        $os = Get-CimInstance @cimParams
        $build = [int]$os.BuildNumber
        switch -Wildcard ($os.Caption) {
            '*2025*' { return 'WindowsServer2025' }
            '*2022*' { return 'WindowsServer2022' }
            '*2019*' { return 'WindowsServer2019' }
            default {
                if ($build -ge 26100) { return 'WindowsServer2025' }
                if ($build -ge 20348) { return 'WindowsServer2022' }
                if ($build -ge 17763) { return 'WindowsServer2019' }
                return 'WindowsServer2022'
            }
        }
    }
    catch {
        Write-Warning "Could not query SUT OS version: $($_.Exception.Message). Defaulting to WindowsServer2025."
        return 'WindowsServer2025'
    }
    finally {
        if ($cimSession) { Remove-CimSession $cimSession -ErrorAction SilentlyContinue }
    }
}

function Connect-WindowsSmbShare {
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingUsernameAndPasswordParams', '',
        Justification = 'The Windows network provider API requires separate username and password strings; neither is placed on a process command line.')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'CredentialPassword',
        Justification = 'The password is passed directly to the Windows network provider and never placed on a process command line.')]
    param(
        [Parameter(Mandatory)] [string]$RemotePath,
        [Parameter(Mandatory)] [string]$CredentialUser,
        [Parameter(Mandatory)] [string]$CredentialPassword
    )

    if (-not ('ProtocolTestSuites.NetworkConnection' -as [type])) {
        Add-Type -TypeDefinition @'
using System;
using System.Runtime.InteropServices;

namespace ProtocolTestSuites
{
    public static class NetworkConnection
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct NetResource
        {
            public int Scope;
            public int Type;
            public int DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        public static extern int WNetAddConnection2(
            ref NetResource netResource,
            string password,
            string userName,
            int flags);
    }
}
'@
    }

    $resource = [ProtocolTestSuites.NetworkConnection+NetResource]::new()
    $resource.Type = 1
    $resource.RemoteName = $RemotePath
    $result = [ProtocolTestSuites.NetworkConnection]::WNetAddConnection2(
        [ref]$resource,
        $CredentialPassword,
        $CredentialUser,
        0)
    # 1219 means an SMB connection already exists for this server. Keep it;
    # subsequent access is the authoritative credential check.
    if ($result -notin @(0, 1219)) {
        throw [ComponentModel.Win32Exception]::new($result)
    }
    return $result
}

function Invoke-SecureSmbClient {
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingUsernameAndPasswordParams', '',
        Justification = 'smbclient requires separate credential fields in a mode-0600 auth file that is removed in finally.')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'CredentialPassword',
        Justification = 'The password is written only to a mode-0600 temporary smbclient auth file and removed in finally.')]
    param(
        [Parameter(Mandatory)] [string]$SharePath,
        [Parameter(Mandatory)] [string]$Command,
        [Parameter(Mandatory)] [string]$CredentialUser,
        [Parameter(Mandatory)] [string]$CredentialPassword
    )

    $credentialDomain = ''
    $credentialName = $CredentialUser
    if ($CredentialUser -match '^([^\\]+)\\(.+)$') {
        $credentialDomain = $Matches[1]
        $credentialName = $Matches[2]
    }
    $authFile = Join-Path ([IO.Path]::GetTempPath()) "wpts-smb-$([guid]::NewGuid().ToString('N')).auth"
    try {
        $authLines = @(
            "username = $credentialName",
            "password = $CredentialPassword"
        )
        if ($credentialDomain) { $authLines += "domain = $credentialDomain" }
        [IO.File]::WriteAllLines($authFile, $authLines, [Text.UTF8Encoding]::new($false))
        & chmod 600 $authFile
        if ($LASTEXITCODE -ne 0) { throw "Could not restrict permissions on smbclient auth file." }

        $output = @(& smbclient $SharePath -A $authFile -c $Command 2>&1)
        $exitCode = $LASTEXITCODE
        return [pscustomobject]@{ Output = $output; ExitCode = $exitCode }
    } finally {
        Remove-Item -LiteralPath $authFile -Force -ErrorAction SilentlyContinue
    }
}

function Copy-ReadableDiagnosticFile {
    param(
        [Parameter(Mandatory)] [string]$SourcePath,
        [Parameter(Mandatory)] [string]$DestinationPath
    )

    $destinationDirectory = Split-Path -Parent $DestinationPath
    New-Item -ItemType Directory -Path $destinationDirectory -Force | Out-Null
    $sourceStream = $null
    $destinationStream = $null
    try {
        $sourceStream = [System.IO.File]::Open(
            $SourcePath,
            [System.IO.FileMode]::Open,
            [System.IO.FileAccess]::Read,
            [System.IO.FileShare]::ReadWrite)
        $destinationStream = [System.IO.File]::Open(
            $DestinationPath,
            [System.IO.FileMode]::Create,
            [System.IO.FileAccess]::Write,
            [System.IO.FileShare]::Read)
        $sourceStream.CopyTo($destinationStream)
    } finally {
        if ($destinationStream) { $destinationStream.Dispose() }
        if ($sourceStream) { $sourceStream.Dispose() }
    }
}

function Copy-DiagnosticDirectory {
    param(
        [Parameter(Mandatory)] [string]$SourceDirectory,
        [Parameter(Mandatory)] [string]$DestinationDirectory
    )

    if (-not (Test-Path -LiteralPath $SourceDirectory -ErrorAction SilentlyContinue)) { return }
    $extensions = @('.log', '.txt', '.signal', '.evtx', '.dmp')
    foreach ($file in @(Get-ChildItem -LiteralPath $SourceDirectory -File -Recurse -ErrorAction SilentlyContinue |
        Where-Object { $_.Extension.ToLowerInvariant() -in $extensions })) {
        try {
            $relativePath = $file.FullName.Substring($SourceDirectory.TrimEnd('\', '/').Length).TrimStart('\', '/')
            Copy-ReadableDiagnosticFile -SourcePath $file.FullName `
                -DestinationPath (Join-Path $DestinationDirectory $relativePath)
        } catch {
            Write-Warning "Could not collect diagnostic '$($file.FullName)': $($_.Exception.Message)"
        }
    }
}

function Copy-RemoteDiagnostics {
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingUsernameAndPasswordParams', '',
        Justification = 'Credentials are forwarded only to secure SMB helpers that keep the password off process command lines.')]
    param(
        [Parameter(Mandatory)] [string]$Target,
        [Parameter(Mandatory)] [string]$RemoteDscDirectory,
        [Parameter(Mandatory)] [string]$DestinationDirectory,
        [Parameter(Mandatory)] [string]$CredentialUser,
        [Parameter(Mandatory)] [string]$CredentialPassword,
        [Parameter(Mandatory)] [bool]$LinuxDriver
    )

    New-Item -ItemType Directory -Path $DestinationDirectory -Force | Out-Null
    if ($LinuxDriver) {
        if (-not (Get-Command smbclient -ErrorAction SilentlyContinue)) { return }
        $remotePath = $RemoteDscDirectory.Replace('\', '/')
        $commands = "prompt OFF; recurse ON; lcd `"$DestinationDirectory`"; cd `"$remotePath`"; mget *.log; mget *.txt; mget *.signal"
        $smbInvocation = Invoke-SecureSmbClient -SharePath "//$Target/C`$" `
            -Command $commands -CredentialUser $CredentialUser `
            -CredentialPassword $CredentialPassword
        $smbInvocation.Output | ForEach-Object { Write-Verbose "smbclient diagnostics: $_" }
        return
    }

    try {
        [void](Connect-WindowsSmbShare -RemotePath "\\$Target\C`$" `
            -CredentialUser $CredentialUser -CredentialPassword $CredentialPassword)
        Copy-DiagnosticDirectory -SourceDirectory "\\$Target\C`$\$RemoteDscDirectory" `
            -DestinationDirectory $DestinationDirectory
    } catch {
        Write-Warning "Could not collect diagnostics from '$Target': $($_.Exception.Message)"
    }
}

$env:Path += "$([IO.Path]::PathSeparator)$scriptsPath"
Push-Location $scriptsPath

Start-Transcript -Path $logFile -Append -Force

$testDir = if ($isLinuxDriver) { '/test' } else { "$env:SystemDrive\Test" }
$existingSignal = Join-Path $testDir 'test.finished.signal'
$runCompleteSignal = Join-Path $testDir 'test.run.completed.signal'
$uploadFailureSignal = Join-Path $testDir 'test.results.upload.failed.signal'
if (Test-Path $existingSignal) {
    if (Test-Path $runCompleteSignal) {
        .\Write-Info.ps1 "Test run already completed (both completion signals exist). Skipping." -ForegroundColor Green
        .\Write-Info.ps1 "Delete $existingSignal and $runCompleteSignal to re-run tests." -ForegroundColor DarkGray
        Pop-Location; Stop-Transcript; return
    }

    .\Write-Info.ps1 "[WARN] Test execution finished previously, but finalization did not. Removing the stale execution signal and rerunning." -ForegroundColor Yellow
    Remove-Item -LiteralPath $existingSignal -Force
}
Remove-Item -LiteralPath $runCompleteSignal -Force -ErrorAction SilentlyContinue
Remove-Item -LiteralPath $uploadFailureSignal -Force -ErrorAction SilentlyContinue

.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "  FileServer Test Run -- Execute-TestCaseByContext          " -ForegroundColor Cyan
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "WorkingPath   : $WorkingPath" -ForegroundColor DarkGray
.\Write-Info.ps1 "TestSuitePath : $TestSuitePath" -ForegroundColor DarkGray
if (-not $isLinuxDriver) {
    .\Write-Info.ps1 "Running as    : $([System.Security.Principal.WindowsIdentity]::GetCurrent().Name)" -ForegroundColor DarkGray
    .\Write-Info.ps1 "Domain joined : $((Get-CimInstance Win32_ComputerSystem -ErrorAction SilentlyContinue).PartOfDomain)" -ForegroundColor DarkGray
} else {
    .\Write-Info.ps1 "Running as    : $(whoami 2>/dev/null)" -ForegroundColor DarkGray
}
.\Write-Info.ps1 ""

$configFile = "$WorkingPath\Config.json"
if (-not (Test-Path $configFile)) {
    .\Write-Error.ps1 "[FAIL] Config.json not found at $configFile"
    Pop-Location; Stop-Transcript; return
}

$config = Get-Content -Path $configFile -Raw | ConvertFrom-Json
$adminUser     = $config.Core.Username
$adminPassword = $config.Core.Password
$scenario      = $config.Core.Scenario

# Resolve SUT machine: Cluster uses Node01 as the primary SUT; Domain/Workgroup use "SUT"
$sutMachine = if ($scenario -eq 'Cluster' -and $config.Machines.Node01) {
    $config.Machines.Node01
} elseif ($config.Machines.SUT) {
    $config.Machines.SUT
} else {
    $config.Machines.Node01
}
$sutName       = $sutMachine.ComputerName
$sutIpConfigs  = @($sutMachine.IpConfig)
$sutExt1Ip     = $sutIpConfigs[0].Ip
$sutExt2Ip     = if ($sutIpConfigs.Count -gt 1) {
    $sutIpConfigs[1].Ip
} else { "" }
$driverIpConfigs = @($config.Machines.DriverComputer.IpConfig)
$driverExt1Ip  = $driverIpConfigs[0].Ip
$driverExt2Ip  = if ($driverIpConfigs.Count -gt 1) {
    $driverIpConfigs[1].Ip
} else { "" }
$dcName        = if ($config.Machines.DC) { $config.Machines.DC.ComputerName } else { "" }
$dcIpConfigs   = if ($config.Machines.DC -and $config.Machines.DC.IpConfig) { @($config.Machines.DC.IpConfig) } else { @() }
$dcExt1Ip      = if ($dcIpConfigs.Count -gt 0) { $dcIpConfigs[0].Ip } else { "" }
$dcExt2Ip      = if ($dcIpConfigs.Count -gt 1) { $dcIpConfigs[1].Ip } else { "" }

.\Write-Info.ps1 "Scenario    : $scenario" -ForegroundColor DarkGray
.\Write-Info.ps1 "SUT Name    : $sutName" -ForegroundColor DarkGray
.\Write-Info.ps1 "SUT IPs     : $sutExt1Ip, $sutExt2Ip" -ForegroundColor DarkGray
.\Write-Info.ps1 "Driver IPs  : $driverExt1Ip, $driverExt2Ip" -ForegroundColor DarkGray
if ($dcName) {
    .\Write-Info.ps1 "DC Name     : $dcName ($dcExt1Ip, $dcExt2Ip)" -ForegroundColor DarkGray
}
.\Write-Info.ps1 ""

.\Write-Info.ps1 "Waiting for SUT deployment to complete..." -ForegroundColor Yellow

$pollInterval    = 30  # seconds
$maxWait         = 120 # minutes
$waited          = 0

# Determine the credential prefix for SMB auth:
# Domain/Cluster mode -> DOMAIN\user; Workgroup mode -> SUTNAME\user
$domainNetBiosName = if ($config.Domain -and $config.Domain.NetBiosName) {
    $config.Domain.NetBiosName
} elseif (($scenario -eq 'Domain' -or $scenario -eq 'Cluster') -and $config.Core.DomainName) {
    # Cluster scenario: Generate-ConfigJson.ps1 doesn't create $config.Domain,
    # so derive NetBIOS name from the FQDN (e.g., "contoso.com" -> "CONTOSO")
    ($config.Core.DomainName -split '\.')[0].ToUpper()
} else { "" }

$smbCredUser = if ($domainNetBiosName) {
    "$domainNetBiosName\$adminUser"
} else {
    "$sutName\$adminUser"
}
.\Write-Info.ps1 "Auth user: $smbCredUser (scenario=$scenario)" -ForegroundColor DarkGray

# Derive the SUT working directory and signal file name per scenario
$sutWorkingDir = switch ($scenario) {
    'Domain'  { 'Domain-Package' }
    'Cluster' { 'Cluster-Package' }
    default   { 'Workgroup-Package' }
}
$sutSignalFileName = switch ($scenario) {
    'Cluster' { 'Deploy-Node01.Completed.signal' }
    default   { 'Deploy-SUT.Completed.signal' }
}

if ($isLinuxDriver) {
    # On Linux we cannot use UNC paths. Poll the SUT's admin share
    # via smbclient to check for the signal file.
    # The SUT is always Windows.  Derive its working path from the scenario
    # (Azure Bicep deploys to C:\<Scenario>-Package on the SUT).
    $sutDriveLetter = 'C'
    $signalRelPath = "$sutWorkingDir\DSC\$sutSignalFileName"
    .\Write-Info.ps1 "SUT signal (smbclient): //$sutName/${sutDriveLetter}`$ -> $signalRelPath" -ForegroundColor DarkGray

    # Ensure smbclient is available (Azure VM extensions run as root)
    if (-not (Get-Command smbclient -ErrorAction SilentlyContinue)) {
        .\Write-Info.ps1 "Installing smbclient..." -ForegroundColor DarkGray
        & sudo apt-get update -qq 2>&1 | Out-Null
        & sudo apt-get install -y -qq smbclient 2>&1 | Out-Null
    }

    while ($waited -lt ($maxWait * 60)) {
        $smbInvocation = Invoke-SecureSmbClient -SharePath "//$sutName/${sutDriveLetter}`$" `
            -Command "ls $signalRelPath" -CredentialUser $smbCredUser `
            -CredentialPassword $adminPassword
        if ($smbInvocation.ExitCode -eq 0 -and "$($smbInvocation.Output)" -notmatch 'NT_STATUS') {
            .\Write-Info.ps1 "[OK] SUT deployment signal detected." -ForegroundColor Green
            break
        }

        $waitedMin = [math]::Round($waited / 60, 1)
        .\Write-Info.ps1 "  SUT not ready yet. Waited ${waitedMin}m, polling every ${pollInterval}s..." -ForegroundColor DarkGray
        Start-Sleep -Seconds $pollInterval
        $waited += $pollInterval
    }
} else {
    # Windows: poll via UNC path and an in-process network-provider call.
    # Use IP address instead of hostname for Workgroup (no DNS server to resolve names)
    $sutTarget = if ($scenario -eq 'Workgroup') { $sutExt1Ip } else { $sutName }
    $driveLetter = if ($WorkingPath -match '^([A-Za-z]):') { $Matches[1] } else { 'C' }
    $sutSignalPath = "\\$sutTarget\${driveLetter}`$\$sutWorkingDir\DSC\$sutSignalFileName"
    .\Write-Info.ps1 "SUT signal path: $sutSignalPath" -ForegroundColor DarkGray

    $netUseAttempted = $false

    while ($waited -lt ($maxWait * 60)) {
        if (-not $netUseAttempted) {
            .\Write-Info.ps1 "  Authenticating to \\$sutTarget\${driveLetter}`$ ..." -ForegroundColor DarkGray
            try {
                $connectionResult = Connect-WindowsSmbShare `
                    -RemotePath "\\$sutTarget\${driveLetter}`$" `
                    -CredentialUser $smbCredUser -CredentialPassword $adminPassword
                .\Write-Info.ps1 "  SMB connection result: $connectionResult" -ForegroundColor DarkGray
            } catch {
                .\Write-Info.ps1 "  SMB connection failed: $($_.Exception.Message)" -ForegroundColor DarkGray
            }
            $netUseAttempted = $true
        }

        # Test-Path on UNC can throw if host is unreachable -- treat as "not ready"
        $signalFound = $false
        try { $signalFound = Test-Path $sutSignalPath -ErrorAction Stop } catch {}

        if ($signalFound) {
            .\Write-Info.ps1 "[OK] SUT deployment signal detected." -ForegroundColor Green
            break
        }

        $waitedMin = [math]::Round($waited / 60, 1)
        .\Write-Info.ps1 "  SUT not ready yet. Waited ${waitedMin}m, polling every ${pollInterval}s..." -ForegroundColor DarkGray
        Start-Sleep -Seconds $pollInterval
        $waited += $pollInterval

        # Re-authenticate every 10 minutes in case connection dropped
        if ($waited % 600 -eq 0) {
            $netUseAttempted = $false
        }
    }
}

if ($waited -ge ($maxWait * 60)) {
    .\Write-Error.ps1 "[FAIL] Timed out waiting for SUT after $maxWait minutes."
    Pop-Location; Stop-Transcript; return
}

# ===========================================================================
# Ensure ForceLevel2 is configured before running tests
# ===========================================================================
# ForceLevel2 (SHI1005_FLAGS_FORCE_LEVELII_OPLOCK on ShareForceLevel2) may not
# have been applied yet if the SUT wasn't ready when Deploy-Driver ran.  A retry
# scheduled task exists but fires every 5 minutes -- tests can start before it
# succeeds.  Now that SUT is confirmed ready, apply it inline.
if (-not $isLinuxDriver) {
    $fl2Applied = $false
    $endPointPath = $null
    $toolsJsonPath = "$WorkingPath\Tools.json"
    if (-not (Test-Path $toolsJsonPath)) {
        $toolsJsonPath = "$scriptsPath\Tools.json"
    }
    if (Test-Path $toolsJsonPath) {
        try {
            $toolsJson = Get-Content -Path $toolsJsonPath -Raw | ConvertFrom-Json
            $endPointPath = [Environment]::ExpandEnvironmentVariables($toolsJson.DriverComputer.TestsuiteZips[0].targetFolder)
        } catch { }
    }
    $shareUtilExe = if ($endPointPath) { "$endPointPath\Utils\ShareUtil.exe" } else { $null }

    if ($shareUtilExe -and (Test-Path $shareUtilExe)) {
        .\Write-Info.ps1 "Ensuring ForceLevel2 on \\$sutName\ShareForceLevel2 ..." -ForegroundColor Yellow

        # Authenticate to SUT (may already be connected from signal polling)
        try {
            [void](Connect-WindowsSmbShare -RemotePath "\\$sutName\IPC`$" `
                -CredentialUser $smbCredUser -CredentialPassword $adminPassword)
        } catch { }

        $fl2MaxRetries = 6
        for ($fl2i = 1; $fl2i -le $fl2MaxRetries; $fl2i++) {
            try {
                $fl2Out = CMD /C "`"$shareUtilExe`" $sutName ShareForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1
                $fl2Out | ForEach-Object { .\Write-Info.ps1 "  $_" -ForegroundColor DarkGray }
            } catch {
                .\Write-Info.ps1 "  ShareUtil error: $($_.Exception.Message)" -ForegroundColor DarkGray
            }
            if ($LASTEXITCODE -eq 0) {
                .\Write-Info.ps1 "[OK] ForceLevel2 configured on $sutName\ShareForceLevel2" -ForegroundColor Green
                $fl2Applied = $true
                break
            }
            .\Write-Info.ps1 "  ForceLevel2 attempt $fl2i/$fl2MaxRetries failed (exit $LASTEXITCODE). Retrying in 10s..." -ForegroundColor Yellow
            Start-Sleep -Seconds 10
        }
        if (-not $fl2Applied) {
            .\Write-Info.ps1 "[WARN] ForceLevel2 could not be applied after $fl2MaxRetries attempts. OplockOnShareWithForceLevel2 tests may fail." -ForegroundColor Yellow
        }

        # Write FL2 signal file + clean up the retry scheduled task if ForceLevel2 succeeded
        if ($fl2Applied) {
            $fl2SignalFile = Join-Path $dscFolder 'ForceLevel2.Completed.signal'
            "FL2 OK $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File -FilePath $fl2SignalFile -Force
            .\Write-Info.ps1 "  ForceLevel2 signal written: $fl2SignalFile" -ForegroundColor DarkGray

            $fl2TaskName = 'Config-ForceLevel2'
            $existingFl2Task = Get-ScheduledTask -TaskName $fl2TaskName -ErrorAction SilentlyContinue
            if ($null -ne $existingFl2Task) {
                Unregister-ScheduledTask -TaskName $fl2TaskName -Confirm:$false -ErrorAction SilentlyContinue
                .\Write-Info.ps1 "  Cleaned up '$fl2TaskName' scheduled task" -ForegroundColor DarkGray
            }
        }
    } else {
        .\Write-Info.ps1 "[WARN] ShareUtil.exe not found -- skipping ForceLevel2 pre-check" -ForegroundColor Yellow
    }
    .\Write-Info.ps1 ""
}

$platform = Get-SutPlatform -SutComputerName $sutName -Username $adminUser -Password $adminPassword -DomainNetBiosName $domainNetBiosName
.\Write-Info.ps1 "SUT Platform: $platform" -ForegroundColor DarkGray

$contextPrefix = $platform -replace 'WindowsServer', 'Win'
$scenario = $config.Core.Scenario
switch ($scenario) {
    'Workgroup' { $contextName = "${contextPrefix}_Workgroup_NonCluster_SMB311" }
    'Domain'    { $contextName = "${contextPrefix}_Domain_NonCluster_SMB311" }
    'Cluster'   { $contextName = "${contextPrefix}_Domain_Cluster_SMB311" }
    default     { $contextName = "${contextPrefix}_Workgroup_NonCluster_SMB311" }
}
.\Write-Info.ps1 "Context Name: $contextName (Scenario=$scenario)" -ForegroundColor Cyan

$executeScript = "$scriptsPath\Execute-TestCaseByContext.ps1"
if (-not (Test-Path $executeScript)) {
    .\Write-Error.ps1 "[FAIL] Execute-TestCaseByContext.ps1 not found at $executeScript"
    Pop-Location; Stop-Transcript; return
}

.\Write-Info.ps1 "" -ForegroundColor Cyan
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "  Starting test run (Execute-TestCaseByContext.ps1)" -ForegroundColor Cyan
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
.\Write-Info.ps1 "Script       : $executeScript" -ForegroundColor DarkGray
.\Write-Info.ps1 "Context      : $contextName" -ForegroundColor DarkGray
if ($Filter) {
    .\Write-Info.ps1 "Filter       : $Filter" -ForegroundColor DarkGray
}
.\Write-Info.ps1 ""

# Establish SMB sessions to the SUT (and DC in domain mode) by hostname, IP, AND FQDN.
# Windows treats \\Node01, \\192.168.1.11, and \\Node01.contoso.com as separate servers,
# so we must pre-authenticate to every variant that Execute-TestCaseByContext.ps1,
# Validate-Environment.ps1, or the test framework may access.
if (-not $isLinuxDriver) {
    $domainSuffix = if ($config.Core.DomainName) { $config.Core.DomainName } else { "" }

    $authTargets = @($sutName, $sutExt1Ip, $sutExt2Ip)
    # FQDN variant — used as SutComputerName and in share paths for Domain/Cluster
    if ($domainSuffix -and ($scenario -eq 'Domain' -or $scenario -eq 'Cluster')) {
        $authTargets += "$sutName.$domainSuffix"
    }
    if ($dcName)   { $authTargets += $dcName }
    if ($dcExt1Ip) { $authTargets += $dcExt1Ip }
    if ($dcExt2Ip) { $authTargets += $dcExt2Ip }
    if ($dcName -and $domainSuffix) { $authTargets += "$dcName.$domainSuffix" }

    # Cluster virtual names — Validate-Environment and tests access these via Windows redirector
    if ($scenario -eq 'Cluster' -and $config.Endpoints) {
        $node02Name = if ($config.Machines.Node02) { $config.Machines.Node02.ComputerName } else { "" }
        $generalFsName  = if ($config.Endpoints.GeneralFS)  { $config.Endpoints.GeneralFS.Name }  else { "" }
        $scaleOutFsName = if ($config.Endpoints.ScaleoutFS) { $config.Endpoints.ScaleoutFS.Name } else { "" }
        if ($node02Name) {
            $authTargets += $node02Name
            if ($domainSuffix) { $authTargets += "$node02Name.$domainSuffix" }
        }
        if ($generalFsName) {
            $authTargets += $generalFsName
            if ($domainSuffix) { $authTargets += "$generalFsName.$domainSuffix" }
        }
        if ($scaleOutFsName) {
            $authTargets += $scaleOutFsName
            if ($domainSuffix) { $authTargets += "$scaleOutFsName.$domainSuffix" }
        }
    }

    foreach ($target in $authTargets) {
        if (-not $target) { continue }
        .\Write-Info.ps1 "  Authenticating to \\$target\C`$ ..." -ForegroundColor DarkGray
        try {
            $connectionResult = Connect-WindowsSmbShare -RemotePath "\\$target\C`$" `
                -CredentialUser $smbCredUser -CredentialPassword $adminPassword
            .\Write-Info.ps1 "  SMB connection \\$target\C`$ result: $connectionResult" -ForegroundColor DarkGray
        } catch {
            .\Write-Info.ps1 "  SMB connection \\$target\C`$ failed: $($_.Exception.Message)" -ForegroundColor DarkGray
        }
    }

    # Verify CIM/WinRM connectivity to DC before running tests.
    # Execute-TestCaseByContext.ps1's StartService() queries the DC via Get-CimInstance
    # using Kerberos. If the Kerberos TGT isn't valid (e.g., timing, batch logon edge case),
    # the error is fatal and kills the test run. Retry to let things stabilize.
    if (($scenario -eq 'Domain' -or $scenario -eq 'Cluster') -and $dcName) {
        $cimOk = $false
        for ($attempt = 1; $attempt -le 5; $attempt++) {
            try {
                Get-CimInstance -ClassName Win32_OperatingSystem -ComputerName $dcName -ErrorAction Stop | Out-Null
                .\Write-Info.ps1 "  [OK] CIM/WinRM to $dcName verified (attempt $attempt/5)" -ForegroundColor Green
                $cimOk = $true
                break
            } catch {
                .\Write-Info.ps1 "  [WARN] CIM to $dcName failed (attempt $attempt/5): $($_.Exception.Message)" -ForegroundColor Yellow
                if ($attempt -lt 5) { Start-Sleep -Seconds 30 }
            }
        }
        if (-not $cimOk) {
            .\Write-Info.ps1 "  [WARN] CIM to $dcName still failing -- test may fail on remote service checks. Continuing..." -ForegroundColor Yellow
        }
    }
}

$testStopwatch = [System.Diagnostics.Stopwatch]::StartNew()

try {
    $testArgs = @{
        ContextName    = $contextName
        runTests       = 'true'
        enableParallel = 'true'
        TestInvocationTimeoutMinutes = $TestInvocationTimeoutMinutes
    }
    if ($Filter) {
        # Map -Filter to CategoryName or TestName
        if ($Filter -match '^TestCategory=') {
            $testArgs['CategoryName'] = ($Filter -replace '^TestCategory=', '')
        } else {
            $testArgs['TestName'] = $Filter
        }
    }

    # Reset ErrorActionPreference before calling the test infrastructure.
    # Execute-TestCaseByContext.ps1 uses Test-Path on UNC paths that may return
    # access-denied; with 'Stop' these become terminating errors instead of $false.
    # It also does Set-Location internally, so save our location for restoration.
    $ErrorActionPreference = 'Continue'
    & $executeScript @testArgs
    $exitCode = $LASTEXITCODE
}
catch {
    & "$scriptsPath\Write-Error.ps1" "[FAIL] Test run threw an exception: $($_.Exception.Message)"
    $exitCode = -1
}
finally {
    $ErrorActionPreference = 'Stop'
    # Execute-TestCaseByContext.ps1 does Set-Location $testDir (line 184) which
    # permanently changes the working directory. Restore to Scripts folder so
    # subsequent .\Write-Info.ps1 calls resolve correctly.
    Set-Location $scriptsPath
}
$testStopwatch.Stop()

$testResultDir = Join-Path $testDir 'TestResults'
$executionPlanCompleted = Test-Path -LiteralPath $existingSignal
$summaryScript = Join-Path $scriptsPath 'Write-TestRunSummary.ps1'
$summaryJsonPath = Join-Path $testResultDir 'test.summary.json'
$summaryTextPath = Join-Path $testResultDir 'test.summary.txt'
try {
    if (-not (Test-Path -LiteralPath $summaryScript)) {
        throw "Test summary script not found at $summaryScript."
    }
    $testSummary = & $summaryScript -TestResultDirectory $testResultDir -Scenario $scenario `
        -ContextName $contextName -ExecutionExitCode $exitCode `
        -ExecutionPlanCompleted $executionPlanCompleted -RequireExecutionManifests `
        -OutputJsonPath $summaryJsonPath `
        -OutputTextPath $summaryTextPath
} catch {
    $summaryError = "Unable to generate complete test summary: $($_.Exception.Message)"
    $testSummary = [pscustomobject]@{
        Classification = 'InfrastructureOrConfigurationFailure'
        FailedTests = @()
    }
    $summaryError | Set-Content -LiteralPath $summaryTextPath -Encoding UTF8
    .\Write-Info.ps1 "[WARN] $summaryError" -ForegroundColor Yellow
}

.\Write-Info.ps1 "" -ForegroundColor Cyan
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan
if ($executionPlanCompleted) {
    .\Write-Info.ps1 "  All configured test stages completed" -ForegroundColor Cyan
} else {
    .\Write-Info.ps1 "  Test execution stopped before the complete plan finished" -ForegroundColor Yellow
}
.\Write-Info.ps1 "  Classification: $($testSummary.Classification)" -ForegroundColor $(if ($testSummary.Classification -eq 'Passed') { 'Green' } else { 'Yellow' })
.\Write-Info.ps1 "  Test process exit code: $exitCode" -ForegroundColor Cyan
.\Write-Info.ps1 "  Duration: $([math]::Round($testStopwatch.Elapsed.TotalMinutes, 1)) min" -ForegroundColor Cyan
.\Write-Info.ps1 "  Results : $testResultDir" -ForegroundColor Cyan
.\Write-Info.ps1 "===========================================================" -ForegroundColor Cyan

if (Test-Path -LiteralPath $summaryTextPath) {
    Get-Content -LiteralPath $summaryTextPath | ForEach-Object {
        .\Write-Info.ps1 $_ -ForegroundColor $(if ($_ -match '^\[(Failed|Error|Timeout|Aborted)\]') { 'Red' } else { 'DarkGray' })
    }
}

# Capture every deployment/test diagnostic before upload and before the active
# transcript is closed. Config files are intentionally excluded because they
# contain credentials.
$diagnosticRoot = Join-Path $testDir 'Diagnostics'
$driverDiagnostics = Join-Path $diagnosticRoot 'Driver'
$sutDiagnostics = Join-Path $diagnosticRoot 'SUT'
Remove-Item -LiteralPath $diagnosticRoot -Recurse -Force -ErrorAction SilentlyContinue
Copy-DiagnosticDirectory -SourceDirectory $dscFolder `
    -DestinationDirectory (Join-Path $driverDiagnostics 'Deployment')
Copy-DiagnosticDirectory -SourceDirectory (Join-Path $testDir 'TestLog') `
    -DestinationDirectory (Join-Path $driverDiagnostics 'TestLog')

foreach ($machineProperty in @($config.Machines.PSObject.Properties)) {
    if ($machineProperty.Name -eq 'DriverComputer') { continue }
    $machine = $machineProperty.Value
    if (-not $machine -or -not $machine.ComputerName) { continue }
    $target = if ($scenario -eq 'Workgroup' -and $machine.IpConfig) {
        @($machine.IpConfig)[0].Ip
    } else {
        $machine.ComputerName
    }
    $roleDirectory = if ($machineProperty.Name -eq 'SUT') {
        $sutDiagnostics
    } else {
        Join-Path $diagnosticRoot $machineProperty.Name
    }
    $remoteCredentialUser = if ($domainNetBiosName) {
        "$domainNetBiosName\$adminUser"
    } else {
        "$($machine.ComputerName)\$adminUser"
    }
    Copy-RemoteDiagnostics -Target $target -RemoteDscDirectory "$sutWorkingDir\DSC" `
        -DestinationDirectory $roleDirectory -CredentialUser $remoteCredentialUser `
        -CredentialPassword $adminPassword -LinuxDriver $isLinuxDriver
}
$diagnosticFiles = @(Get-ChildItem -LiteralPath $diagnosticRoot -File -Recurse -ErrorAction SilentlyContinue)

# ===========================================================================
# Upload test results to Azure Storage (if ResultsUpload.json exists)
# ===========================================================================
$resultsUploadFile = "$WorkingPath\ResultsUpload.json"
if (Test-Path $resultsUploadFile) {
    .\Write-Info.ps1 "Uploading test results to Azure Storage..." -ForegroundColor Yellow
    try {
        $uploadConfig = Get-Content -Path $resultsUploadFile -Raw | ConvertFrom-Json
        [UriBuilder]$sasUrl = $uploadConfig.SasUrl
        $timestamp = (Get-Date).ToString('yyyyMMdd-HHmmss')
        $blobPrefix = "$scenario/$timestamp"
        # Collect files to upload: TRX files, logs, and the signal file
        $filesToUpload = @()
        if (Test-Path $testResultDir) {
            $filesToUpload += Get-ChildItem -Path $testResultDir -Recurse -File -ErrorAction SilentlyContinue
        }
        $filesToUpload += $diagnosticFiles

        $uploaded = 0
        foreach ($file in $filesToUpload) {
            $relativePath = if ($file.FullName.StartsWith($testResultDir, [StringComparison]::OrdinalIgnoreCase)) {
                "TestResults/" + $file.FullName.Substring($testResultDir.Length).TrimStart('\', '/')
            } elseif ($file.FullName.StartsWith($diagnosticRoot, [StringComparison]::OrdinalIgnoreCase)) {
                "Diagnostics/" + $file.FullName.Substring($diagnosticRoot.Length).TrimStart('\', '/')
            } else {
                "Logs/" + $file.Name
            }
            $blobName = "$blobPrefix/$relativePath"
            $blobUriBuilder = [UriBuilder]$sasUrl.Uri
            $blobUriBuilder.Path = "$($sasUrl.Path.TrimEnd('/'))/$($blobName.Replace('\', '/'))"
            $blobUrl = $blobUriBuilder.Uri.AbsoluteUri

            try {
                $headers = @{
                    'x-ms-blob-type' = 'BlockBlob'
                    'x-ms-date'      = (Get-Date).ToUniversalTime().ToString('R')
                }
                $fileStream = [System.IO.File]::Open(
                    $file.FullName,
                    [System.IO.FileMode]::Open,
                    [System.IO.FileAccess]::Read,
                    [System.IO.FileShare]::ReadWrite)
                try {
                    $memoryStream = [System.IO.MemoryStream]::new()
                    try {
                        $fileStream.CopyTo($memoryStream)
                        $fileBytes = $memoryStream.ToArray()
                    } finally {
                        $memoryStream.Dispose()
                    }
                } finally {
                    $fileStream.Dispose()
                }
                Invoke-RestMethod -Uri $blobUrl -Method Put -Headers $headers -Body $fileBytes `
                    -ContentType 'application/octet-stream' -ErrorAction Stop | Out-Null
                $uploaded++
            } catch {
                $errMsg = $_.Exception.Message
                if ($errMsg -match 'AuthenticationFailed|AuthorizationFailure|403|Server failed to authenticate') {
                    .\Write-Info.ps1 "[WARN] SAS token expired or invalid. Stopping upload." -ForegroundColor Yellow
                    break
                }
                .\Write-Info.ps1 "  Failed to upload $($file.Name): $errMsg" -ForegroundColor Yellow
            }
        }

        if ($uploaded -eq $filesToUpload.Count) {
            .\Write-Info.ps1 "[OK] Uploaded $uploaded/$($filesToUpload.Count) files to $($uploadConfig.StorageAccountName)/$($uploadConfig.ContainerName)/$blobPrefix" -ForegroundColor Green
        } else {
            "UPLOAD FAILED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') uploaded=$uploaded expected=$($filesToUpload.Count)" |
                Out-File -FilePath $uploadFailureSignal -Force
            .\Write-Info.ps1 "[WARN] Uploaded only $uploaded/$($filesToUpload.Count) files. Failure signal: $uploadFailureSignal" -ForegroundColor Yellow
        }
    } catch {
        "UPLOAD FAILED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') error=$($_.Exception.Message)" |
            Out-File -FilePath $uploadFailureSignal -Force
        .\Write-Info.ps1 "[WARN] Test results upload failed: $($_.Exception.Message)" -ForegroundColor Yellow
    }
} else {
    .\Write-Info.ps1 "No ResultsUpload.json found -- skipping test results upload" -ForegroundColor DarkGray
}

if (-not $isLinuxDriver) {
    $taskName = 'RunFileServerTests'
    $existingTask = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
    if ($null -ne $existingTask) {
        Unregister-ScheduledTask -TaskName $taskName -Confirm:$false
        .\Write-Info.ps1 "Scheduled task '$taskName' unregistered." -ForegroundColor DarkGray
    }
}

"TEST RUN FINALIZED $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" |
    Out-File -FilePath $runCompleteSignal -Force
.\Write-Info.ps1 "[OK] Test orchestration completion signal written: $runCompleteSignal" -ForegroundColor Green

Pop-Location
Stop-Transcript

if ($testSummary.Classification -ne 'Passed') {
    exit 1
}
