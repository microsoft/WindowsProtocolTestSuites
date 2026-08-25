# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [string]$ConfigureFile = (Join-Path (Split-Path $PSScriptRoot -Parent) '..\Config.json'),
    [switch]$Detailed
)

$ErrorActionPreference = 'Stop'
$failures = New-Object System.Collections.Generic.List[string]

function Add-ClusterReadinessFailure {
    param([string]$Message)
    $failures.Add($Message)
    if ($Detailed) { Write-Warning $Message }
}

function Test-AzureIpResources {
    param(
        [string]$GroupName,
        [object[]]$IpConfigurations
    )
    $resources = @(Get-ClusterResource | Where-Object {
        $_.OwnerGroup -eq $GroupName -and $_.ResourceType -eq 'IP Address'
    })
    foreach ($configuration in $IpConfigurations) {
        $address = "$($configuration.Ip)"
        $resource = $resources | Where-Object {
            "$((($_ | Get-ClusterParameter -Name Address -ErrorAction SilentlyContinue).Value))" -eq $address
        } | Select-Object -First 1
        if ($null -eq $resource) {
            Add-ClusterReadinessFailure "Group '$GroupName' is missing IP '$address'."
            continue
        }
        $subnetMask = ($resource | Get-ClusterParameter -Name SubnetMask `
            -ErrorAction SilentlyContinue).Value
        $probePort = ($resource | Get-ClusterParameter -Name ProbePort `
            -ErrorAction SilentlyContinue).Value
        $override = ($resource | Get-ClusterParameter -Name OverrideAddressMatch `
            -ErrorAction SilentlyContinue).Value
        if ("$subnetMask" -ne '255.255.255.255') {
            Add-ClusterReadinessFailure "IP '$address' is not configured with a /32 mask."
        }
        if ([int]$probePort -ne [int]$configuration.ProbePort) {
            Add-ClusterReadinessFailure "IP '$address' has incorrect ProbePort '$probePort'."
        }
        if ([int]$override -ne 1) {
            Add-ClusterReadinessFailure "IP '$address' does not enable OverrideAddressMatch."
        }
    }
}

function Get-ClusterScopedShare {
    param([string]$ScopeName, [string]$Name)
    $share = Get-SmbShare -Name $Name -ScopeName $ScopeName `
        -ErrorAction SilentlyContinue
    if ($null -ne $share) { return $share }

    $group = Get-ClusterGroup -Name $ScopeName -ErrorAction SilentlyContinue
    $ownerNode = "$($group.OwnerNode.Name)"
    if ([string]::IsNullOrWhiteSpace($ownerNode) -or
        $ownerNode -eq $env:COMPUTERNAME) {
        return $null
    }

    $session = $null
    try {
        $session = New-CimSession -ComputerName $ownerNode -ErrorAction Stop
        return Get-SmbShare -CimSession $session -Name $Name `
            -ScopeName $ScopeName -ErrorAction SilentlyContinue
    }
    catch {
        Add-ClusterReadinessFailure (
            "Could not query share '$ScopeName\$Name' from owner '$ownerNode': " +
            $_.Exception.Message
        )
        return $null
    }
    finally {
        if ($null -ne $session) {
            Remove-CimSession -CimSession $session -ErrorAction SilentlyContinue
        }
    }
}

try {
    Import-Module FailoverClusters -ErrorAction Stop
    $config = Get-Content -LiteralPath $ConfigureFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
}
catch {
    Add-ClusterReadinessFailure "Cluster readiness preflight failed: $($_.Exception.Message)"
    return $false
}

$clusterName = "$($config.Endpoints.Cluster.Name)"
$generalFsName = "$($config.Endpoints.GeneralFS.Name)"
$scaleOutName = "$($config.Endpoints.ScaleoutFS.Name)"
$infraFsName = "$($config.Endpoints.InfrastructureFS.Name)"

$cluster = Get-Cluster -ErrorAction SilentlyContinue
if ($null -eq $cluster -or $cluster.Name -ne $clusterName) {
    Add-ClusterReadinessFailure "Expected Cluster '$clusterName' is not accessible."
}

$expectedNodes = @($config.Machines.Node01.ComputerName,
    $config.Machines.Node02.ComputerName)
$nodes = @(Get-ClusterNode -ErrorAction SilentlyContinue)
foreach ($nodeName in $expectedNodes) {
    $node = $nodes | Where-Object { $_.Name -eq $nodeName } |
        Select-Object -First 1
    if ($null -eq $node -or "$($node.State)" -ne 'Up') {
        Add-ClusterReadinessFailure "Cluster node '$nodeName' is not Up."
    }
}

$failedResources = @(Get-ClusterResource -ErrorAction SilentlyContinue |
    Where-Object { "$($_.State)" -eq 'Failed' })
if ($failedResources.Count -gt 0) {
    Add-ClusterReadinessFailure (
        "Failed Cluster resources: $($failedResources.Name -join ', ')."
    )
}

$physicalDisks = @(Get-ClusterResource -ErrorAction SilentlyContinue |
    Where-Object { $_.ResourceType -eq 'Physical Disk' })
$csvs = @(Get-ClusterSharedVolume -ErrorAction SilentlyContinue)
$ownedDiskNames = @(
    @($physicalDisks.Name)
    @($csvs.Name)
) | Where-Object { $_ } | Select-Object -Unique
if ($ownedDiskNames.Count -ne 4) {
    Add-ClusterReadinessFailure (
        "Cluster owns $($ownedDiskNames.Count) disk resources; expected 4."
    )
}
foreach ($disk in $physicalDisks) {
    if ("$($disk.State)" -ne 'Online') {
        Add-ClusterReadinessFailure "Physical disk '$($disk.Name)' is not Online."
    }
}

$quorum = Get-ClusterQuorum -ErrorAction SilentlyContinue
if ($null -eq $quorum -or [string]::IsNullOrWhiteSpace("$($quorum.QuorumResource)")) {
    Add-ClusterReadinessFailure 'Cluster disk witness quorum is not configured.'
}

foreach ($groupName in @('Cluster Group', $generalFsName, $scaleOutName)) {
    $group = Get-ClusterGroup -Name $groupName -ErrorAction SilentlyContinue
    if ($null -eq $group -or "$($group.State)" -ne 'Online') {
        Add-ClusterReadinessFailure "Cluster group '$groupName' is not Online."
    }
}
if (-not [string]::IsNullOrWhiteSpace($infraFsName)) {
    $infraGroup = Get-ClusterGroup -Name $infraFsName -ErrorAction SilentlyContinue
    if ($null -eq $infraGroup) {
        $infraGroup = Get-ClusterGroup -Name 'Infrastructure File Server' `
            -ErrorAction SilentlyContinue
    }
    if ($null -eq $infraGroup -or "$($infraGroup.State)" -ne 'Online') {
        Add-ClusterReadinessFailure "Infrastructure group '$infraFsName' is not Online."
    }
}

$expectedCsvCount = if ([string]::IsNullOrWhiteSpace($infraFsName)) { 1 } else { 2 }
if ($csvs.Count -lt $expectedCsvCount) {
    Add-ClusterReadinessFailure (
        "Cluster has $($csvs.Count) CSVs; expected at least $expectedCsvCount."
    )
}

foreach ($share in @(
    @{ Scope = $generalFsName; Name = 'SMBClustered'; Encrypt = $false; CA = $true },
    @{ Scope = $generalFsName; Name = 'SMBClusteredEncrypted'; Encrypt = $true; CA = $true },
    @{ Scope = $scaleOutName; Name = 'SMBClustered'; Encrypt = $false; CA = $true },
    @{ Scope = $scaleOutName; Name = 'SMBClusteredForceLevel2'; Encrypt = $false; CA = $true }
)) {
    $existing = Get-ClusterScopedShare -Name $share.Name -ScopeName $share.Scope
    if ($null -eq $existing) {
        Add-ClusterReadinessFailure "Share '$($share.Scope)\$($share.Name)' is missing."
        continue
    }
    if ([bool]$existing.EncryptData -ne [bool]$share.Encrypt) {
        Add-ClusterReadinessFailure "Share '$($share.Scope)\$($share.Name)' encryption is incorrect."
    }
    if ([bool]$existing.ContinuouslyAvailable -ne [bool]$share.CA) {
        Add-ClusterReadinessFailure "Share '$($share.Scope)\$($share.Name)' CA state is incorrect."
    }
}

foreach ($network in @(Get-ClusterNetwork -ErrorAction SilentlyContinue)) {
    if ([int]$network.Role -ne 3) {
        Add-ClusterReadinessFailure "Cluster network '$($network.Name)' is not ClusterAndClient."
    }
}

Test-AzureIpResources -GroupName 'Cluster Group' `
    -IpConfigurations @($config.Endpoints.Cluster.IpConfig)
Test-AzureIpResources -GroupName $generalFsName `
    -IpConfigurations @($config.Endpoints.GeneralFS.IpConfig)

foreach ($endpoint in @($config.Endpoints.Cluster, $config.Endpoints.GeneralFS)) {
    try {
        $expectedIps = @($endpoint.IpConfig.Ip)
        if ($endpoint.Name -eq $clusterName) {
            $clusterNameResource = Get-ClusterResource | Where-Object {
                $_.OwnerGroup -eq 'Cluster Group' -and
                $_.ResourceType -eq 'Distributed Network Name'
            } | Select-Object -First 1
            if ($null -ne $clusterNameResource) {
                $expectedIps = @(
                    $config.Machines.Node01.IpConfig.Ip
                    $config.Machines.Node02.IpConfig.Ip
                )
            }
        }
        $dnsServer = "$(@($config.Machines.DC.IpConfig.Ip)[0])"
        $resolved = @(Resolve-DnsName -Name $endpoint.Name -Type A `
            -Server $dnsServer `
            -ErrorAction Stop | ForEach-Object { $_.IPAddress })
        foreach ($expectedIp in $expectedIps) {
            if ($resolved -notcontains "$expectedIp") {
                Add-ClusterReadinessFailure (
                    "DNS '$($endpoint.Name)' does not include '$expectedIp'."
                )
            }
        }
    }
    catch {
        Add-ClusterReadinessFailure "DNS '$($endpoint.Name)' could not be resolved."
    }
}

if ($failures.Count -gt 0) {
    if ($Detailed) {
        Write-Warning "Cluster readiness failed with $($failures.Count) issue(s)."
    }
    return $false
}

if ($Detailed) {
    Write-Output "Cluster '$clusterName' and clustered endpoints are ready."
}
return $true
