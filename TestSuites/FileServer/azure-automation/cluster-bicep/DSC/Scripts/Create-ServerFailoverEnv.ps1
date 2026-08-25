# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Creates or repairs the FileServer failover Cluster without replacing
    healthy identities, roles, resources, or owned disks.
#>

[CmdletBinding()]
param(
    [string]$WorkingPath = (Split-Path (Split-Path $PSScriptRoot -Parent) -Parent),
    [string]$ConfigureFile = "$WorkingPath\Config.json",
    [string]$HeartbeatPath,
    [switch]$NoTranscript
)

$ErrorActionPreference = 'Stop'
$dscFolder = Split-Path $PSScriptRoot -Parent
$logFile = "$PSCommandPath.log"
$transcriptStarted = $false

Push-Location $PSScriptRoot
if (-not $NoTranscript) {
    Start-Transcript -Path $logFile -Append -Force | Out-Null
    $transcriptStarted = $true
}

. (Join-Path $dscFolder 'Deploy-CommonHelpers.ps1')

function Write-ClusterHeartbeat {
    param([string]$Checkpoint, [datetime]$StartedAt)
    if (-not $HeartbeatPath) { return }
    Write-DeploymentHeartbeat -Phase 'ClusterFormation' `
        -Operation 'Create or repair failover Cluster' `
        -StartedAt $StartedAt -HeartbeatPath $HeartbeatPath `
        -LastCheckpoint $Checkpoint
}

function Set-ClusterParameterValue {
    param(
        [Parameter(Mandatory)]$Resource,
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)]$Value
    )
    try {
        $Resource | Set-ClusterParameter -Name $Name -Value $Value -ErrorAction Stop |
            Out-Null
    }
    catch {
        $Resource | Set-ClusterParameter -Name $Name -Value $Value -Create `
            -ErrorAction Stop | Out-Null
    }
}

function Get-ClusterNetworkForAddress {
    param([string]$Address)
    $octets = $Address.Split('.')
    if ($octets.Count -ne 4) { return $null }
    $prefix = "$($octets[0]).$($octets[1]).$($octets[2])."
    return Get-ClusterNetwork | Where-Object {
        "$($_.Address)".StartsWith($prefix)
    } | Select-Object -First 1
}

function Set-AzureClusterIpResources {
    param(
        [Parameter(Mandatory)][string]$GroupName,
        [Parameter(Mandatory)][object[]]$IpConfigurations
    )

    $group = Get-ClusterGroup -Name $GroupName -ErrorAction Stop
    $ipResources = @(Get-ClusterResource | Where-Object {
        $_.OwnerGroup -eq $group.Name -and $_.ResourceType -eq 'IP Address'
    })
    $configuredResources = New-Object System.Collections.Generic.List[object]

    for ($index = 0; $index -lt $IpConfigurations.Count; $index++) {
        $ipConfiguration = $IpConfigurations[$index]
        $address = "$($ipConfiguration.Ip)"
        $probePort = [int]$ipConfiguration.ProbePort
        $resource = $ipResources | Where-Object {
            $currentAddress = ($_ | Get-ClusterParameter -Name Address `
                -ErrorAction SilentlyContinue).Value
            "$currentAddress" -eq $address
        } | Select-Object -First 1

        if ($null -eq $resource) {
            $resource = $ipResources | Where-Object {
                $currentAddress = ($_ | Get-ClusterParameter -Name Address `
                    -ErrorAction SilentlyContinue).Value
                "$($_.State)" -ne 'Online' -and
                "$currentAddress" -in @('', '0.0.0.0')
            } | Select-Object -First 1
        }
        if ($null -eq $resource) {
            $resourceName = "$GroupName IP Address $($index + 1)"
            $resource = Add-ClusterResource -Name $resourceName `
                -ResourceType 'IP Address' -Group $group.Name -ErrorAction Stop
            $ipResources += $resource
        }

        Stop-ClusterResource -Name $resource.Name -Wait 30 `
            -ErrorAction SilentlyContinue | Out-Null
        $network = Get-ClusterNetworkForAddress -Address $address
        if ($null -eq $network) {
            throw "No Cluster network matches Azure virtual IP '$address'."
        }
        $resource | Set-ClusterParameter -Multiple @{
            Address = $address
            SubnetMask = '255.255.255.255'
            Network = $network.Name
            EnableDhcp = 0
            ProbePort = $probePort
            OverrideAddressMatch = 1
        } -ErrorAction Stop | Out-Null
        Start-ClusterResource -Name $resource.Name -Wait 60 -ErrorAction Stop |
            Out-Null
        $configuredResources.Add($resource)
    }

    $networkName = Get-ClusterResource | Where-Object {
        $_.OwnerGroup -eq $group.Name -and
        $_.ResourceType -in @('Network Name', 'Distributed Network Name')
    } | Select-Object -First 1
    if ($null -eq $networkName) {
        throw "Group '$GroupName' has no supported network-name resource."
    }
    Set-ClusterParameterValue -Resource $networkName `
        -Name RegisterAllProvidersIP -Value 1
    Set-ClusterParameterValue -Resource $networkName `
        -Name HostRecordTTL -Value 60
    $dependency = (@($configuredResources |
        ForEach-Object { "[$($_.Name)]" })) -join ' or '
    if ($networkName.ResourceType -eq 'Network Name') {
        Set-ClusterResourceDependency -Resource $networkName.Name `
            -Dependency $dependency -ErrorAction Stop
    }
    Start-ClusterResource -Name $networkName.Name -Wait 60 -ErrorAction Stop |
        Out-Null
}

function Get-ClusterOwnedDiskGuids {
    $guids = New-Object System.Collections.Generic.HashSet[string](
        [System.StringComparer]::OrdinalIgnoreCase
    )
    if (-not (Test-Path 'HKLM:\Cluster')) {
        return ,$guids
    }
    foreach ($resource in @(Get-ClusterResource -ErrorAction Stop |
        Where-Object { $_.ResourceType -eq 'Physical Disk' })) {
        $value = ($resource | Get-ClusterParameter -Name DiskIdGuid `
            -ErrorAction SilentlyContinue).Value
        if ($value) { [void]$guids.Add("$value".Trim('{}')) }
    }
    foreach ($resource in @(Get-ClusterSharedVolume -ErrorAction SilentlyContinue)) {
        $disk = Get-DiskForClusterResource -Resource $resource
        if ($null -ne $disk -and $disk.Guid) {
            [void]$guids.Add("$($disk.Guid)".Trim('{}'))
        }
    }
    return ,$guids
}

function Prepare-UnownedIscsiDisks {
    $ownedGuids = Get-ClusterOwnedDiskGuids
    $disks = @(Get-Disk -ErrorAction Stop |
        Where-Object { $_.BusType -eq 'iSCSI' } |
        Sort-Object Size, UniqueId)
    if ($disks.Count -ne 4) {
        throw "Exactly four iSCSI disks are required; found $($disks.Count)."
    }

    $dataIndex = 0
    foreach ($disk in $disks) {
        $diskGuid = "$($disk.Guid)".Trim('{}')
        $owned = $diskGuid -and $ownedGuids.Contains($diskGuid)
        if ($disk.PartitionStyle -eq 'RAW' -and $owned) {
            throw "Cluster-owned disk $($disk.Number) is RAW; refusing to format it."
        }
        if ($disk.PartitionStyle -ne 'RAW') { continue }

        if ($disk.IsOffline) {
            Set-Disk -Number $disk.Number -IsOffline $false -ErrorAction Stop
        }
        if ($disk.IsReadOnly) {
            Set-Disk -Number $disk.Number -IsReadOnly $false -ErrorAction Stop
        }
        Initialize-Disk -Number $disk.Number -PartitionStyle GPT -ErrorAction Stop
        $partition = New-Partition -DiskNumber $disk.Number -UseMaximumSize `
            -ErrorAction Stop
        if ([long]$disk.Size -lt [long](2GB)) {
            $label = 'ClusterQuorum'
        }
        else {
            $dataIndex++
            $label = "ClusterData$dataIndex"
        }
        $partition | Format-Volume -FileSystem NTFS -NewFileSystemLabel $label `
            -Confirm:$false -ErrorAction Stop | Out-Null
    }
    return @(Get-Disk | Where-Object { $_.BusType -eq 'iSCSI' } |
        Sort-Object Size, UniqueId)
}

function Get-DiskForClusterResource {
    param($Resource)
    if ($null -ne $Resource.PSObject.Properties['SharedVolumeInfo']) {
        $partitionId = "$(@($Resource.SharedVolumeInfo)[0].Partition.Name)"
        $partition = Get-Volume -ErrorAction Stop |
            Where-Object { "$($_.UniqueId)" -eq $partitionId } |
            Get-Partition -ErrorAction Stop |
            Select-Object -First 1
        if ($null -ne $partition) {
            return Get-Disk -Number $partition.DiskNumber -ErrorAction Stop
        }
        return $null
    }
    $diskGuid = ($Resource | Get-ClusterParameter -Name DiskIdGuid `
        -ErrorAction SilentlyContinue).Value
    if ($diskGuid) {
        $normalized = "$diskGuid".Trim('{}')
        $disk = Get-Disk | Where-Object {
            "$($_.Guid)".Trim('{}') -ieq $normalized
        } | Select-Object -First 1
        if ($null -ne $disk) { return $disk }
    }
    $signature = ($Resource | Get-ClusterParameter -Name DiskSignature `
        -ErrorAction SilentlyContinue).Value
    if ($null -ne $signature) {
        return Get-Disk | Where-Object {
            [uint32]$_.Signature -eq [uint32]$signature
        } | Select-Object -First 1
    }
    return $null
}

function Get-ClusterDiskResource {
    param([string]$Name)
    $resource = Get-ClusterSharedVolume -Name $Name -ErrorAction SilentlyContinue
    if ($null -ne $resource) { return $resource }
    return Get-ClusterResource -Name $Name -ErrorAction SilentlyContinue
}

function Get-PhysicalDiskInfos {
    # Prefer CSV objects when a disk appears in both APIs, then de-duplicate by
    # the underlying disk identity so reruns count each owned disk once.
    $resources = @(Get-ClusterSharedVolume -ErrorAction SilentlyContinue)
    $resources += @(Get-ClusterResource | Where-Object {
        $_.ResourceType -eq 'Physical Disk'
    })
    $seenDisks = New-Object System.Collections.Generic.HashSet[string](
        [System.StringComparer]::OrdinalIgnoreCase
    )
    $diskInfos = @()
    foreach ($resource in $resources) {
        $disk = Get-DiskForClusterResource -Resource $resource
        if ($null -ne $disk) {
            $diskIdentity = if ($disk.Guid) {
                $normalizedGuid = "$($disk.Guid)".Trim('{}')
                "Guid:$normalizedGuid"
            } elseif ($disk.UniqueId) {
                "UniqueId:$($disk.UniqueId)"
            } else {
                "Number:$($disk.Number)"
            }
            if (-not $seenDisks.Add($diskIdentity)) {
                continue
            }
            $diskInfos += [pscustomobject]@{
                Resource = $resource
                Disk = $disk
            }
        }
    }
    return @($diskInfos)
}

function Rename-ClusterResourceIfNeeded {
    param($Resource, [string]$Name)
    if ($Resource.Name -ne $Name) {
        $Resource.Name = $Name
    }
    $resource = Get-ClusterDiskResource -Name $Name
    if ($null -eq $resource) {
        throw "Cluster disk resource '$Name' was not found after rename."
    }
    return $resource
}

function Ensure-ClusterResourceOnline {
    param($Resource)
    if ($null -ne $Resource.PSObject.Properties['SharedVolumeInfo']) {
        if ("$($Resource.State)" -ne 'Online') {
            throw "Cluster Shared Volume '$($Resource.Name)' is not online."
        }
        return
    }
    if ("$($Resource.State)" -ne 'Online') {
        Start-ClusterResource -Name $Resource.Name -Wait 120 -ErrorAction Stop |
            Out-Null
    }
}

function Get-GeneralDiskRoot {
    param($Resource)
    $disk = Get-DiskForClusterResource -Resource $Resource
    if ($null -eq $disk) {
        throw "Could not resolve disk for Cluster resource '$($Resource.Name)'."
    }
    $partition = Get-Partition -DiskNumber $disk.Number -ErrorAction Stop |
        Where-Object { $_.Type -notin @('Reserved', 'System') } |
        Select-Object -First 1
    if ($null -eq $partition) {
        throw "GeneralFS disk $($disk.Number) has no usable partition."
    }
    if (-not $partition.DriveLetter) {
        $usedLetters = @(Get-Volume | Where-Object { $_.DriveLetter } |
            ForEach-Object { "$($_.DriveLetter)" })
        $letter = @([char[]]([char]'F'..[char]'Z') | Where-Object {
            "$_" -notin $usedLetters
        } | Select-Object -First 1)
        if ($letter.Count -ne 1) { throw 'No drive letter is available for GeneralFS.' }
        Add-PartitionAccessPath -DiskNumber $disk.Number `
            -PartitionNumber $partition.PartitionNumber `
            -DriveLetter "$($letter[0])" -ErrorAction Stop
        $partition = Get-Partition -DiskNumber $disk.Number `
            -PartitionNumber $partition.PartitionNumber
    }
    return "$($partition.DriveLetter):\"
}

function Ensure-ScopedShare {
    param(
        [string]$Name,
        [string]$ScopeName,
        [string]$Path,
        [string]$FullAccess,
        [bool]$EncryptData = $false,
        [bool]$ContinuouslyAvailable = $true
    )
    New-Item -ItemType Directory -Path $Path -Force | Out-Null
    & icacls.exe $Path '/grant' "${FullAccess}:(OI)(CI)(F)" | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to grant '$FullAccess' access to '$Path'."
    }
    $share = Get-SmbShare -Name $Name -ScopeName $ScopeName `
        -ErrorAction SilentlyContinue
    if ($null -eq $share) {
        New-SmbShare -Name $Name -ScopeName $ScopeName -Path $Path `
            -FullAccess $FullAccess -ContinuouslyAvailable $ContinuouslyAvailable `
            -CachingMode BranchCache -EncryptData $EncryptData -ErrorAction Stop |
            Out-Null
    }
    elseif ($share.Path -ne $Path) {
        throw "Share '$ScopeName\$Name' uses '$($share.Path)', expected '$Path'."
    }
}

$startedAt = Get-Date
try {
    Import-Module FailoverClusters -ErrorAction Stop
    $config = Get-Content -LiteralPath $ConfigureFile -Raw -ErrorAction Stop |
        ConvertFrom-Json -ErrorAction Stop
    $clusterName = "$($config.Endpoints.Cluster.Name)"
    $generalFsName = "$($config.Endpoints.GeneralFS.Name)"
    $scaleOutName = "$($config.Endpoints.ScaleoutFS.Name)"
    $infraFsName = "$($config.Endpoints.InfrastructureFS.Name)"
    $clusterNodes = @($config.Machines.Node01.ComputerName,
        $config.Machines.Node02.ComputerName)
    $clusterIps = @($config.Endpoints.Cluster.IpConfig)
    $generalFsIps = @($config.Endpoints.GeneralFS.IpConfig)
    $domainAdmin = "$($config.Domain.NetBiosName)\$($config.Core.Username)"

    Write-ClusterHeartbeat -Checkpoint 'Preparing unowned iSCSI disks' `
        -StartedAt $startedAt
    $null = Prepare-UnownedIscsiDisks

    $cluster = Get-Cluster -ErrorAction SilentlyContinue
    if ($null -ne $cluster -and $cluster.Name -ne $clusterName) {
        throw "This node belongs to Cluster '$($cluster.Name)', expected '$clusterName'."
    }
    if ($null -eq $cluster) {
        New-Cluster -Name $clusterName -Node $clusterNodes `
            -StaticAddress @($clusterIps.Ip) -NoStorage -ErrorAction Stop |
            Out-Null
        $cluster = Get-Cluster -ErrorAction Stop
    }
    else {
        .\Write-Info.ps1 "Cluster '$clusterName' already exists; preserving its AD and DNS endpoint identities"
    }

    Write-ClusterHeartbeat -Checkpoint 'Repairing Cluster node membership' `
        -StartedAt $startedAt
    $memberNames = @(Get-ClusterNode | ForEach-Object { $_.Name })
    foreach ($nodeName in $clusterNodes) {
        if ($memberNames -notcontains $nodeName) {
            Add-ClusterNode -Name $nodeName -NoStorage `
                -ErrorAction Stop | Out-Null
        }
    }
    Wait-DeploymentCondition -Condition {
        $nodes = @(Get-ClusterNode -ErrorAction SilentlyContinue)
        @($nodes | Where-Object { $_.State -eq 'Up' }).Count -eq $clusterNodes.Count
    } -TimeoutSeconds 600 -PollIntervalSeconds 10 `
        -Phase 'ClusterFormation' -Operation 'Wait for both Cluster nodes Up' `
        -HeartbeatPath $HeartbeatPath -LastCheckpoint 'Membership repaired' | Out-Null

    foreach ($network in @(Get-ClusterNetwork)) {
        if ([int]$network.Role -ne 3) { $network.Role = 3 }
    }

    Write-ClusterHeartbeat -Checkpoint 'Adding available iSCSI disks' `
        -StartedAt $startedAt
    $availableDisks = @(Get-ClusterAvailableDisk -ErrorAction SilentlyContinue)
    if ($availableDisks.Count -gt 0) {
        $availableDisks | Add-ClusterDisk -ErrorAction Stop | Out-Null
    }
    $diskInfos = Get-PhysicalDiskInfos
    if ($diskInfos.Count -ne 4) {
        throw "Cluster owns $($diskInfos.Count) physical disks; expected exactly 4."
    }

    $quorumInfo = $diskInfos | Where-Object {
        [long]$_.Disk.Size -lt [long](2GB)
    } | Select-Object -First 1
    if ($null -eq $quorumInfo) { throw 'The 1-GB quorum disk was not found.' }
    $quorumResource = Rename-ClusterResourceIfNeeded `
        -Resource $quorumInfo.Resource -Name 'ClusterQuorumDisk'
    Ensure-ClusterResourceOnline -Resource $quorumResource
    Set-ClusterQuorum -DiskWitness $quorumResource.Name -ErrorAction Stop |
        Out-Null

    $dataInfos = @($diskInfos | Where-Object {
        [long]$_.Disk.Size -ge [long](2GB)
    } | Sort-Object { $_.Disk.UniqueId })
    if ($dataInfos.Count -ne 3) {
        throw "Cluster requires three data disks; found $($dataInfos.Count)."
    }

    $scaleResource = Get-ClusterDiskResource -Name 'SMBScaleOutDisk'
    $infraResource = Get-ClusterDiskResource -Name 'SMBInfraDisk'
    $reservedDataGuids = @(@($scaleResource, $infraResource) | Where-Object {
        $null -ne $_
    } | ForEach-Object {
        $reservedDisk = Get-DiskForClusterResource -Resource $_
        if ($null -ne $reservedDisk) { "$($reservedDisk.Guid)" }
    })

    $generalResource = Get-ClusterDiskResource -Name 'SMBGeneralDisk'
    if ($null -eq $generalResource) {
        $generalCandidate = $dataInfos | Where-Object {
            "$($_.Disk.Guid)" -notin $reservedDataGuids
        } | Select-Object -First 1
        if ($null -eq $generalCandidate) {
            throw 'No data disk is available for SMBGeneralDisk.'
        }
        $generalResource = Rename-ClusterResourceIfNeeded `
            -Resource $generalCandidate.Resource -Name 'SMBGeneralDisk'
    }
    Ensure-ClusterResourceOnline -Resource $generalResource
    $generalDisk = Get-DiskForClusterResource -Resource $generalResource
    if ($null -eq $generalDisk) { throw 'Could not resolve SMBGeneralDisk.' }

    if ($null -eq $scaleResource) {
        $infraDiskGuid = if ($null -ne $infraResource) {
            $disk = Get-DiskForClusterResource -Resource $infraResource
            if ($null -ne $disk) { "$($disk.Guid)" }
        } else { $null }
        $candidate = $dataInfos | Where-Object {
            "$($_.Disk.Guid)" -ine "$($generalDisk.Guid)" -and
            (-not $infraDiskGuid -or "$($_.Disk.Guid)" -ine $infraDiskGuid)
        } | Select-Object -First 1
        if ($null -eq $candidate) {
            throw 'No data disk is available for SMBScaleOutDisk.'
        }
        if ($null -eq (Get-ClusterSharedVolume -Name $candidate.Resource.Name `
                -ErrorAction SilentlyContinue)) {
            Add-ClusterSharedVolume -Name $candidate.Resource.Name -ErrorAction Stop |
                Out-Null
        }
        $scaleResource = Rename-ClusterResourceIfNeeded `
            -Resource (Get-ClusterSharedVolume -Name $candidate.Resource.Name `
                -ErrorAction Stop) `
            -Name 'SMBScaleOutDisk'
    }
    Ensure-ClusterResourceOnline -Resource $scaleResource
    if ($null -eq (Get-ClusterSharedVolume -Name $scaleResource.Name `
            -ErrorAction SilentlyContinue)) {
        Add-ClusterSharedVolume -Name $scaleResource.Name -ErrorAction Stop |
            Out-Null
    }
    $scaleDisk = Get-DiskForClusterResource -Resource $scaleResource
    if ($null -eq $scaleDisk) { throw 'Could not resolve SMBScaleOutDisk.' }

    if (-not [string]::IsNullOrWhiteSpace($infraFsName) -and
        $null -eq $infraResource) {
        $candidate = $dataInfos | Where-Object {
            "$($_.Disk.Guid)" -notin @("$($generalDisk.Guid)", "$($scaleDisk.Guid)")
        } | Select-Object -First 1
        if ($null -eq $candidate) { throw 'No data disk remains for InfraFS.' }
        if ($null -eq (Get-ClusterSharedVolume -Name $candidate.Resource.Name `
                -ErrorAction SilentlyContinue)) {
            Add-ClusterSharedVolume -Name $candidate.Resource.Name -ErrorAction Stop |
                Out-Null
        }
        $infraResource = Rename-ClusterResourceIfNeeded `
            -Resource (Get-ClusterSharedVolume -Name $candidate.Resource.Name `
                -ErrorAction Stop) `
            -Name 'SMBInfraDisk'
    }
    if ($null -ne $infraResource) {
        Ensure-ClusterResourceOnline -Resource $infraResource
    }

    Write-ClusterHeartbeat -Checkpoint 'Repairing Clustered file server roles' `
        -StartedAt $startedAt
    $generalGroup = Get-ClusterGroup -Name $generalFsName -ErrorAction SilentlyContinue
    if ($null -eq $generalGroup) {
        Add-ClusterFileServerRole -Name $generalFsName `
            -Storage $generalResource.Name -StaticAddress @($generalFsIps.Ip) `
            -ErrorAction Stop | Out-Null
    }
    elseif ($generalResource.OwnerGroup -ne $generalFsName) {
        Move-ClusterResource -Name $generalResource.Name -Group $generalFsName `
            -ErrorAction Stop | Out-Null
    }
    Move-ClusterGroup -Name $generalFsName -Node $env:COMPUTERNAME `
        -Wait 120 -ErrorAction Stop | Out-Null
    Set-AzureClusterIpResources -GroupName $generalFsName `
        -IpConfigurations $generalFsIps

    $generalRoot = Get-GeneralDiskRoot -Resource $generalResource
    Ensure-ScopedShare -Name 'SMBClustered' -ScopeName $generalFsName `
        -Path (Join-Path $generalRoot 'SMBClustered') -FullAccess $domainAdmin
    Ensure-ScopedShare -Name 'SMBClusteredEncrypted' -ScopeName $generalFsName `
        -Path (Join-Path $generalRoot 'SMBClusteredEncrypted') `
        -FullAccess $domainAdmin -EncryptData $true

    if ($null -eq (Get-ClusterGroup -Name $scaleOutName -ErrorAction SilentlyContinue)) {
        Add-ClusterScaleOutFileServerRole -Name $scaleOutName -ErrorAction Stop |
            Out-Null
    }
    Move-ClusterGroup -Name $scaleOutName -Node $env:COMPUTERNAME `
        -Wait 120 -ErrorAction Stop | Out-Null
    Move-ClusterSharedVolume -Name $scaleResource.Name -Node $env:COMPUTERNAME `
        -ErrorAction Stop | Out-Null
    $scaleCsv = Get-ClusterSharedVolume -Name $scaleResource.Name -ErrorAction Stop
    $scaleRoot = "$($scaleCsv.SharedVolumeInfo.FriendlyVolumeName)"
    Ensure-ScopedShare -Name 'SMBClustered' -ScopeName $scaleOutName `
        -Path (Join-Path $scaleRoot 'SMBClustered') -FullAccess $domainAdmin
    Ensure-ScopedShare -Name 'SMBClusteredForceLevel2' -ScopeName $scaleOutName `
        -Path (Join-Path $scaleRoot 'SMBClusteredForceLevel2') `
        -FullAccess $domainAdmin -ContinuouslyAvailable $true

    if (-not [string]::IsNullOrWhiteSpace($infraFsName)) {
        $infraGroup = Get-ClusterGroup -Name $infraFsName `
            -ErrorAction SilentlyContinue
        if ($null -eq $infraGroup) {
            $infraGroup = Get-ClusterGroup -Name 'Infrastructure File Server' `
                -ErrorAction SilentlyContinue
        }
        if ($null -eq $infraGroup) {
            Add-ClusterScaleOutFileServerRole -Infrastructure -Name $infraFsName `
                -ErrorAction Stop | Out-Null
        }
        if ($null -ne $infraResource -and
            $null -eq (Get-ClusterSharedVolume -Name $infraResource.Name `
                -ErrorAction SilentlyContinue)) {
            Add-ClusterSharedVolume -Name $infraResource.Name -ErrorAction Stop |
                Out-Null
        }
    }

    Set-AzureClusterIpResources -GroupName 'Cluster Group' `
        -IpConfigurations $clusterIps

    # Repair VCOs after role creation. This is normally a no-op, but restores
    # endpoint identities removed by an interrupted or older continuation.
    $scriptPath = $PSScriptRoot
    $protocolConfigFile = $ConfigureFile
    $vcoRepairScript = Join-Path $scriptPath 'Repair-ClusterVirtualComputerObjects.ps1'
    $processLauncher = Join-Path $scriptPath 'Invoke-ProcessAsUser.ps1'
    $vcoRepairResult = Join-Path $env:TEMP "wpts-vco-repair-$PID.result"
    if (-not (Test-Path $vcoRepairScript) -or -not (Test-Path $processLauncher)) {
        throw 'Cluster VCO repair prerequisites are missing from the deployment package.'
    }
    Remove-Item -LiteralPath $vcoRepairResult -Force -ErrorAction SilentlyContinue
    try {
        $repairProcess = & $processLauncher `
            -UserName $config.Core.Username `
            -Domain $config.Domain.NetBiosName `
            -Password $config.Core.Password `
            -FilePath "$env:SystemRoot\System32\WindowsPowerShell\v1.0\powershell.exe" `
            -ArgumentList @(
                '-NoProfile', '-NonInteractive', '-ExecutionPolicy', 'Bypass',
                '-File', $vcoRepairScript,
                '-ConfigFile', $protocolConfigFile,
                '-ResultPath', $vcoRepairResult
            ) `
            -WorkingDirectory $scriptPath `
            -WaitForExit -TimeoutSeconds 600
        if ($repairProcess.ExitCode -ne 0) {
            throw "Cluster VCO repair process exited with code $($repairProcess.ExitCode)."
        }
        if (-not (Test-Path $vcoRepairResult)) {
            throw 'Cluster VCO repair process returned no result.'
        }
        $vcoRepairOutcome = (Get-Content -LiteralPath $vcoRepairResult -Raw).Trim()
        if ($vcoRepairOutcome -notlike 'SUCCESS|*') {
            throw "Cluster VCO repair failed: $vcoRepairOutcome"
        }
        .\Write-Info.ps1 "[OK] Cluster virtual computer objects verified: $($vcoRepairOutcome.Substring(8))"
    }
    finally {
        Remove-Item -LiteralPath $vcoRepairResult -Force -ErrorAction SilentlyContinue
    }

    foreach ($groupName in @('Cluster Group', $generalFsName, $scaleOutName)) {
        $group = Get-ClusterGroup -Name $groupName -ErrorAction SilentlyContinue
        if ($null -ne $group) { $group.FailoverThreshold = 1024 }
    }

    Wait-DeploymentCondition -Condition {
        & (Join-Path $PSScriptRoot 'Test-ClusterReadiness.ps1') `
            -ConfigureFile $ConfigureFile
    } -TimeoutSeconds 600 -PollIntervalSeconds 15 `
        -Phase 'ClusterFormation' -Operation 'Wait for live Cluster readiness' `
        -HeartbeatPath $HeartbeatPath -LastCheckpoint 'Cluster roles and IPs repaired' |
        Out-Null
    return $true
}
catch {
    .\Write-Error.ps1 "Cluster formation/repair failed: $($_.Exception.Message)"
    throw
}
finally {
    if ($transcriptStarted) { Stop-Transcript | Out-Null }
    Pop-Location
}
