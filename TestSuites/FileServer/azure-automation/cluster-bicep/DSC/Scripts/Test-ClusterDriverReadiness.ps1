# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'The private deployment Config.json already contains the runtime credential and readiness must convert it to SecureString for PSCredential-based Kerberos probes; no interactive prompt is available.')]
[CmdletBinding()]
param(
    [string]$WorkingPath = (Split-Path (Split-Path $PSScriptRoot -Parent) -Parent),
    [string]$ConfigureFile = "$WorkingPath\Config.json",
    [switch]$SkipForceLevel2Check,
    [switch]$SkipTestTaskCheck,
    [switch]$Detailed
)

$ErrorActionPreference = 'Stop'
$dscFolder = Split-Path $PSScriptRoot -Parent
$remoteDscFolder = Join-Path $WorkingPath 'DSC'
. (Join-Path $dscFolder 'Deploy-CommonHelpers.ps1')
$failures = New-Object System.Collections.Generic.List[string]

function Add-DriverReadinessFailure {
    param([string]$Message)
    $failures.Add($Message)
    if ($Detailed) { Write-Warning $Message }
}

try {
    $config = Get-Content -LiteralPath $ConfigureFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    $testAutoRun = if ($null -eq $config.TestExecution -or
        $null -eq $config.TestExecution.AutoRun) {
        $true
    } else {
        [Convert]::ToBoolean("$($config.TestExecution.AutoRun)")
    }
}
catch {
    Add-DriverReadinessFailure "Config.json could not be loaded: $($_.Exception.Message)"
    return $false
}

$computerSystem = Get-CimInstance Win32_ComputerSystem -ErrorAction SilentlyContinue
if ($null -eq $computerSystem -or -not $computerSystem.PartOfDomain) {
    Add-DriverReadinessFailure 'Driver is not joined to the domain.'
}
try {
    if (-not (Test-ComputerSecureChannel -ErrorAction Stop)) {
        Add-DriverReadinessFailure 'Driver secure channel returned false.'
    }
}
catch {
    Add-DriverReadinessFailure "Driver secure channel failed: $($_.Exception.Message)"
}
if (Test-PendingSystemReboot) {
    Add-DriverReadinessFailure 'Driver has a pending reboot.'
}

if (-not (Test-VerifiedDeploymentSignal `
        -Path (Join-Path $dscFolder 'Driver-DSC.Completed.signal') `
        -ExpectedContentPattern '^DRIVER DSC READY;')) {
    Add-DriverReadinessFailure 'Driver DSC completion signal is invalid.'
}
try {
    if (-not (Test-DscConfiguration -Path (Join-Path $dscFolder 'MOF\Driver') `
            -ErrorAction Stop)) {
        Add-DriverReadinessFailure 'Driver DSC reports drift.'
    }
}
catch {
    Add-DriverReadinessFailure "Driver DSC could not be checked: $($_.Exception.Message)"
}

foreach ($path in @(
    (Join-Path $PSScriptRoot 'InstallMSIAndTools.Completed.signal'),
    "$env:ProgramFiles\PowerShell\7\pwsh.exe",
    "$env:SystemDrive\FileServer-TestSuite-ServerEP\Bin\.version",
    "$env:SystemDrive\FileServer-TestSuite-ServerEP\Utils\ShareUtil.exe"
)) {
    $item = Get-Item -LiteralPath $path -ErrorAction SilentlyContinue
    if ($null -eq $item -or $item.PSIsContainer -or $item.Length -le 0) {
        Add-DriverReadinessFailure "Required Driver artifact '$path' is missing."
    }
}

$sshd = Get-Service sshd -ErrorAction SilentlyContinue
if ($null -eq $sshd -or $sshd.Status -ne 'Running') {
    Add-DriverReadinessFailure 'Driver OpenSSH service is not Running.'
}
$domainNetBios = if ($config.Domain.NetBiosName) {
    "$($config.Domain.NetBiosName)"
} else {
    "$($config.Core.DomainName)".Split('.')[0].ToUpperInvariant()
}
$domainCredential = [pscredential]::new(
    "$domainNetBios\$($config.Core.Username)",
    (ConvertTo-SecureString "$($config.Core.Password)" -AsPlainText -Force)
)
$sshKeyCandidates = @(
    "$env:SystemDrive\Users\$($config.Core.Username).$domainNetBios\.ssh\id_rsa",
    "$env:SystemDrive\Users\$($config.Core.Username)\.ssh\id_rsa"
)
if (@($sshKeyCandidates | Where-Object {
    Test-Path -LiteralPath $_ -PathType Leaf
}).Count -eq 0) {
    Add-DriverReadinessFailure 'Driver RSA key was not installed in the admin profile.'
}

$testSuiteBin = "$env:SystemDrive\FileServer-TestSuite-ServerEP\Bin"
function Read-PtfProperty {
    param([string]$Path, [string]$Name)
    [xml]$xml = Get-Content -LiteralPath $Path -ErrorAction Stop
    $node = $xml.GetElementsByTagName('Property') | Where-Object {
        $_.GetAttribute('name') -eq $Name
    } | Select-Object -First 1
    if ($null -eq $node) { return $null }
    return $node.GetAttribute('value')
}
try {
    $commonPtf = Join-Path $testSuiteBin 'CommonTestSuite.deployment.ptfconfig'
    $failoverPtf = Join-Path $testSuiteBin 'ServerFailoverTestSuite.deployment.ptfconfig'
    $authPtf = Join-Path $testSuiteBin 'Auth_ServerTestSuite.deployment.ptfconfig'
    if ((Read-PtfProperty -Path $commonPtf -Name 'SutComputerName') -ne
        "$($config.Machines.Node01.ComputerName).$($config.Core.DomainName)") {
        Add-DriverReadinessFailure 'Common PTF SutComputerName is incorrect.'
    }
    if ([string]::IsNullOrWhiteSpace(
        (Read-PtfProperty -Path $commonPtf -Name 'PasswordForAllUsers')
    )) {
        Add-DriverReadinessFailure 'Common PTF password was not configured.'
    }
    if ((Read-PtfProperty -Path $failoverPtf -Name 'ClusteredFileServerName') -ne
        "$($config.Endpoints.GeneralFS.Name).$($config.Core.DomainName)") {
        Add-DriverReadinessFailure 'ServerFailover PTF GeneralFS name is incorrect.'
    }
    if ((Read-PtfProperty -Path $failoverPtf -Name 'ClusteredScaleOutFileServerName') -ne
        "$($config.Endpoints.ScaleoutFS.Name).$($config.Core.DomainName)") {
        Add-DriverReadinessFailure 'ServerFailover PTF ScaleoutFS name is incorrect.'
    }
    if (-not [string]::IsNullOrWhiteSpace(
        (Read-PtfProperty -Path $authPtf -Name 'KeytabFile')
    )) {
        Add-DriverReadinessFailure 'Auth PTF unexpectedly selects a keytab instead of the deterministic machine password.'
    }
    if ((Read-PtfProperty -Path $authPtf -Name 'ServicePassword') -ne
        'Password04!') {
        Add-DriverReadinessFailure 'Auth PTF Kerberos service password is incorrect.'
    }
    $expectedServiceSalt = (
        "$($config.Core.DomainName)".ToUpperInvariant() +
        'host' +
        "$($config.Machines.Node01.ComputerName)".ToLowerInvariant() +
        '.' +
        "$($config.Core.DomainName)".ToLowerInvariant()
    )
    if ((Read-PtfProperty -Path $authPtf -Name 'ServiceSaltString') -cne
        $expectedServiceSalt) {
        Add-DriverReadinessFailure 'Auth PTF Kerberos service salt is incorrect.'
    }
}
catch {
    Add-DriverReadinessFailure "Cluster PTF configuration could not be verified: $($_.Exception.Message)"
}

$node01 = "$($config.Machines.Node01.ComputerName)"
$node02 = "$($config.Machines.Node02.ComputerName)"
$node01Fqdn = "$node01.$($config.Core.DomainName)"
try {
    $kerberosComputerName = Invoke-Command -ComputerName $node01Fqdn `
        -Authentication Kerberos -Credential $domainCredential `
        -ScriptBlock { $env:COMPUTERNAME } `
        -ErrorAction Stop
    if ("$kerberosComputerName" -ine $node01) {
        Add-DriverReadinessFailure 'Kerberos-only Node01 remoting returned an unexpected computer identity.'
    }
}
catch {
    Add-DriverReadinessFailure (
        "Node01 did not accept a Kerberos-only service authentication: $($_.Exception.Message)"
    )
}
foreach ($node in @(
    @{ Name = $node01; Signal = 'Deploy-Node01.Completed.signal'; Role = 'Node01' },
    @{ Name = $node02; Signal = 'Deploy-Node02.Completed.signal'; Role = 'Node02' }
)) {
    try {
        $signalPath = Join-Path $remoteDscFolder $node.Signal
        $ready = Invoke-Command -ComputerName $node.Name -ScriptBlock {
            param($path, $role)
            $item = Get-Item -LiteralPath $path -ErrorAction SilentlyContinue
            if ($null -eq $item -or $item.Length -le 0) { return $false }
            $content = Get-Content -LiteralPath $path -Raw
            return $content -match "^NODE COMPLETE; SchemaVersion=1\.0; Role=$role;"
        } -ArgumentList $signalPath, $node.Role -Credential $domainCredential `
            -Authentication Kerberos -ErrorAction Stop
        if ($ready -ne $true) {
            Add-DriverReadinessFailure "$($node.Role) final completion signal is invalid."
        }
    }
    catch {
        Add-DriverReadinessFailure "$($node.Role) final completion could not be verified."
    }
}

try {
    $remoteReadinessScript = Join-Path $remoteDscFolder 'Scripts\Test-ClusterReadiness.ps1'
    $clusterOutput = @(Invoke-Command -ComputerName $node01 -ScriptBlock {
        param($readinessScript, $configFile)
        & $readinessScript -ConfigureFile $configFile
    } -ArgumentList $remoteReadinessScript, $ConfigureFile `
        -Credential $domainCredential -Authentication Kerberos -ErrorAction Stop)
    if ($clusterOutput.Count -eq 0 -or $clusterOutput[-1] -ne $true) {
        Add-DriverReadinessFailure 'Remote live Cluster readiness validation failed.'
    }
}
catch {
    Add-DriverReadinessFailure "Remote live Cluster validation failed: $($_.Exception.Message)"
}

foreach ($sharePath in @(
    "\\$($config.Endpoints.GeneralFS.Name)\SMBClustered",
    "\\$($config.Endpoints.ScaleoutFS.Name)\SMBClustered"
)) {
    $driveName = "Wpts$([guid]::NewGuid().ToString('N').Substring(0, 8))"
    try {
        New-PSDrive -Name $driveName -PSProvider FileSystem -Root $sharePath `
            -Credential $domainCredential -ErrorAction Stop | Out-Null
        if (-not (Test-Path -LiteralPath "${driveName}:\" -PathType Container `
                -ErrorAction Stop)) {
            Add-DriverReadinessFailure "Clustered share '$sharePath' is unreachable."
        }
    }
    catch {
        Add-DriverReadinessFailure (
            "Clustered share '$sharePath' is unreachable: $($_.Exception.Message)"
        )
    }
    finally {
        Remove-PSDrive -Name $driveName -Force -ErrorAction SilentlyContinue
    }
}

if (-not $SkipForceLevel2Check) {
    $tools = Get-Content -LiteralPath (Join-Path $WorkingPath 'Tools.json') -Raw |
        ConvertFrom-Json
    $testSuiteRoot = [Environment]::ExpandEnvironmentVariables(
        "$($tools.DriverComputer.TestsuiteZips[0].targetFolder)"
    )
    $shareUtil = Join-Path $testSuiteRoot 'Utils\ShareUtil.exe'
    foreach ($target in @(
        @{ Server = $node01; Share = 'ShareForceLevel2'; Signal = 'ForceLevel2.Local.Completed.signal' },
        @{ Server = "$($config.Endpoints.ScaleoutFS.Name)"; Share = 'SMBClusteredForceLevel2'; Signal = 'ForceLevel2.Clustered.Completed.signal' }
    )) {
        $signalPath = Join-Path $dscFolder $target.Signal
        if (-not (Test-VerifiedDeploymentSignal -Path $signalPath `
                -ExpectedContentPattern '^FORCELEVEL2 READY; SchemaVersion=1\.0;')) {
            Add-DriverReadinessFailure "ForceLevel2 signal '$($target.Signal)' is invalid."
            continue
        }
        $output = @(& $shareUtil $target.Server $target.Share 2>&1)
        if ($LASTEXITCODE -ne 0 -or
            -not (Test-ShareUtilForceLevel2Output -Output $output)) {
            Add-DriverReadinessFailure (
                "ForceLevel2 is not live on $($target.Server)\$($target.Share)."
            )
        }
    }
}

if (-not $SkipTestTaskCheck -and $testAutoRun) {
    $task = Get-ScheduledTask -TaskName 'RunFileServerTests' `
        -ErrorAction SilentlyContinue
    $testsFinished = Test-Path "$env:SystemDrive\Test\test.finished.signal" `
        -PathType Leaf -ErrorAction SilentlyContinue
    if ($null -eq $task -and $testsFinished) {
        # The test wrapper removes its task after successful completion.
    }
    elseif ($null -eq $task -or "$($task.State)" -eq 'Disabled') {
        Add-DriverReadinessFailure 'RunFileServerTests task is missing or disabled.'
    }
    elseif (@($task.Actions | Where-Object {
        "$($_.Arguments)" -match 'Invoke-TestRun\.ps1'
    }).Count -eq 0) {
        Add-DriverReadinessFailure 'RunFileServerTests action is incorrect.'
    }
    else {
        $expectedTaskUser = "$domainNetBios\$($config.Core.Username)"
        $actualTaskUser = "$($task.Principal.UserId)"
        if ($actualTaskUser -ine $expectedTaskUser -and
            $actualTaskUser -ine "$($config.Core.Username)") {
            Add-DriverReadinessFailure (
                "RunFileServerTests user '$actualTaskUser' does not match '$expectedTaskUser'."
            )
        }
        if (@($task.Triggers).Count -eq 0) {
            Add-DriverReadinessFailure 'RunFileServerTests has no trigger.'
        }
        if ($task.Settings -and $task.Settings.Enabled -eq $false) {
            Add-DriverReadinessFailure 'RunFileServerTests settings are disabled.'
        }
    }
}

if ($failures.Count -gt 0) {
    if ($Detailed) {
        Write-Warning "Cluster Driver readiness failed with $($failures.Count) issue(s)."
    }
    return $false
}
if ($Detailed) { Write-Output 'Cluster Driver and test gates are ready.' }
return $true
