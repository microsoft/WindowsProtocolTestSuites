# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet('Node01', 'Node02')]
    [string]$NodeRole,

    [string]$ConfigureFile = (Join-Path (Split-Path $PSScriptRoot -Parent) '..\Config.json'),

    [string]$MofPath = (Join-Path (Split-Path $PSScriptRoot -Parent) "MOF\$NodeRole"),

    [switch]$Detailed
)

$ErrorActionPreference = 'Stop'
. (Join-Path (Split-Path $PSScriptRoot -Parent) 'Deploy-CommonHelpers.ps1')
$failures = New-Object System.Collections.Generic.List[string]

function Add-NodeReadinessFailure {
    param([string]$Message)
    $failures.Add($Message)
    if ($Detailed) { Write-Warning $Message }
}

try {
    $config = Get-Content -LiteralPath $ConfigureFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
}
catch {
    Add-NodeReadinessFailure "Config.json could not be loaded: $($_.Exception.Message)"
    return $false
}

$node = $config.Machines.$NodeRole
if ($null -eq $node) {
    Add-NodeReadinessFailure "Config.json is missing Machines.$NodeRole."
    return $false
}

if ($env:COMPUTERNAME -ne "$($node.ComputerName)") {
    Add-NodeReadinessFailure (
        "Computer name '$env:COMPUTERNAME' does not match '$($node.ComputerName)'."
    )
}

$computerSystem = Get-CimInstance Win32_ComputerSystem -ErrorAction SilentlyContinue
if ($null -eq $computerSystem -or -not $computerSystem.PartOfDomain -or
    "$($computerSystem.Domain)" -ine "$($config.Core.DomainName)") {
    Add-NodeReadinessFailure 'Cluster node domain membership is incomplete.'
}
try {
    if (-not (Test-ComputerSecureChannel -ErrorAction Stop)) {
        Add-NodeReadinessFailure 'Cluster node secure channel validation returned false.'
    }
}
catch {
    Add-NodeReadinessFailure "Cluster node secure channel failed: $($_.Exception.Message)"
}

if (Test-PendingSystemReboot) {
    Add-NodeReadinessFailure 'A reboot remains pending.'
}

$requiredFeatures = @(
    'Failover-Clustering',
    'RSAT-Clustering-PowerShell',
    'File-Services',
    'FS-BranchCache',
    'FS-VSS-Agent',
    'BranchCache',
    'FS-DFS-Namespace',
    'RSAT-DFS-Mgmt-Con',
    'FS-Resource-Manager',
    'RSAT-AD-PowerShell'
)
foreach ($featureName in $requiredFeatures) {
    $feature = Get-WindowsFeature -Name $featureName -ErrorAction SilentlyContinue
    if ($null -eq $feature -or $feature.InstallState -ne 'Installed') {
        Add-NodeReadinessFailure "Required feature '$featureName' is not installed."
    }
}

try {
    if (-not (Test-DscConfiguration -Path $MofPath -ErrorAction Stop)) {
        Add-NodeReadinessFailure 'Node DSC convergence reports drift.'
    }
}
catch {
    Add-NodeReadinessFailure "Node DSC convergence could not be checked: $($_.Exception.Message)"
}

$enabledFirewall = Get-NetFirewallProfile -ErrorAction SilentlyContinue |
    Where-Object { $_.Enabled -eq $true }
if (@($enabledFirewall).Count -gt 0) {
    Add-NodeReadinessFailure 'One or more Windows Firewall profiles remain enabled.'
}

$listener = Get-WSManInstance -ResourceURI winrm/config/listener -Enumerate `
    -ErrorAction SilentlyContinue | Where-Object { $_.Transport -eq 'HTTP' }
if ($null -eq $listener) {
    Add-NodeReadinessFailure 'PowerShell remoting HTTP listener is missing.'
}

$smbConfiguration = Get-SmbServerConfiguration -ErrorAction SilentlyContinue
if ($null -eq $smbConfiguration -or -not $smbConfiguration.RequireSecuritySignature) {
    Add-NodeReadinessFailure 'SMB server signing is not required.'
}

foreach ($registryCheck in @(
    @{ Path = 'HKLM:\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters'; Name = 'AsymmetryMode'; Value = 2 },
    @{ Path = 'HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem'; Name = 'NtfsDisableLastAccessUpdate'; Value = 0 },
    @{ Path = 'HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem'; Name = 'RefsDisableLastAccessUpdate'; Value = 0 },
    @{ Path = 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters'; Name = 'DisablePasswordChange'; Value = 1 }
)) {
    $value = Get-ItemProperty -Path $registryCheck.Path `
        -Name $registryCheck.Name -ErrorAction SilentlyContinue
    if ($null -eq $value -or
        $value.PSObject.Properties.Name -notcontains $registryCheck.Name -or
        [uint32]$value.$($registryCheck.Name) -ne [uint32]$registryCheck.Value) {
        Add-NodeReadinessFailure "Registry state '$($registryCheck.Name)' is incorrect."
    }
}

$expectedShares = if ($NodeRole -eq 'Node01') {
    @('FileShare', 'SMBBasic', 'ShareForceLevel2', 'SMBEncrypted', 'SMBCompressed')
} else {
    @('FileShare', 'SMBBasic')
}
foreach ($shareName in $expectedShares) {
    $share = Get-SmbShare -Name $shareName -ErrorAction SilentlyContinue
    if ($null -eq $share -or -not (Test-Path -LiteralPath $share.Path -PathType Container)) {
        Add-NodeReadinessFailure "Required local share '$shareName' is not ready."
    }
}

$toolsSignal = Join-Path $PSScriptRoot 'InstallMSIAndTools.Completed.signal'
if (-not (Test-VerifiedDeploymentSignal -Path $toolsSignal `
        -ExpectedContentPattern '^Completed ')) {
    Add-NodeReadinessFailure 'Required tool completion signal is missing or invalid.'
}
foreach ($toolPath in @(
    "$env:ProgramFiles\PowerShell\7\pwsh.exe",
    "$env:SystemDrive\OpenSSH-Win64\ssh.exe"
)) {
    if (-not (Test-Path -LiteralPath $toolPath -PathType Leaf)) {
        Add-NodeReadinessFailure "Required tool path '$toolPath' is missing."
    }
}

$sshd = Get-Service sshd -ErrorAction SilentlyContinue
if ($null -eq $sshd -or $sshd.Status -ne 'Running' -or
    $sshd.StartType -ne 'Automatic') {
    Add-NodeReadinessFailure 'OpenSSH service is not Automatic and Running.'
}

$storage = $config.Machines.Storage
$targetName = if ($storage.iSCSITargetName) {
    "$($storage.iSCSITargetName)"
} else {
    'ClusterTarget'
}
$expectedTargetAddress = (
    "iqn.1991-05.com.microsoft:$($storage.ComputerName)-$targetName-target"
).ToLowerInvariant()
$target = Get-IscsiTarget -ErrorAction SilentlyContinue |
    Where-Object { "$($_.NodeAddress)" -ieq $expectedTargetAddress } |
    Select-Object -First 1
if ($null -eq $target -or -not $target.IsConnected) {
    Add-NodeReadinessFailure "Configured iSCSI target '$expectedTargetAddress' is not connected."
}
$session = Get-IscsiSession -ErrorAction SilentlyContinue |
    Where-Object { "$($_.TargetNodeAddress)" -ieq $expectedTargetAddress } |
    Select-Object -First 1
if ($null -eq $session) {
    Add-NodeReadinessFailure "Persistent iSCSI session '$expectedTargetAddress' is missing."
}
$persistentTargets = @(& iscsicli.exe ListPersistentTargets 2>&1)
if (-not ($persistentTargets -match [regex]::Escape($expectedTargetAddress))) {
    Add-NodeReadinessFailure "iSCSI target '$expectedTargetAddress' is not persistent."
}
$iscsiDisks = @(Get-Disk -ErrorAction SilentlyContinue |
    Where-Object { $_.BusType -eq 'iSCSI' })
if ($iscsiDisks.Count -ne 4) {
    Add-NodeReadinessFailure (
        "Cluster node sees $($iscsiDisks.Count) iSCSI disks; expected exactly 4."
    )
}

if ($failures.Count -gt 0) {
    if ($Detailed) {
        Write-Warning "$NodeRole foundation readiness failed with $($failures.Count) issue(s)."
    }
    return $false
}

if ($Detailed) {
    Write-Output "$NodeRole foundation is ready for Cluster formation."
}
return $true
