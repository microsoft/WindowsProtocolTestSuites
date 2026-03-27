# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param($workingDir = $PSScriptRoot, $protocolConfigFile = "$workingDir\Config.json")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath"
Push-Location $workingDir
#----------------------------------------------------------------------------
# if working dir is not exists. it will use scripts path as working path
#----------------------------------------------------------------------------
if(!(Test-Path "$workingDir"))
{
    $workingDir = $scriptPath
}

if(!(Test-Path "$protocolConfigFile"))
{
    $protocolConfigFile = "$workingDir\Config.json"
    if(!(Test-Path "$protocolConfigFile")) 
    {
        .\Write-Error.ps1 "No Config file found."
        exit 1
    }
}

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function ExitCode()
{ 
    return $MyInvocation.ScriptLineNumber 
}

function Write-ConfigFailureSignal()
{
    $startSignalFile = "$workingDir\Config_" + $env:COMPUTERNAME + "_FailureSignal.log"
    "Execute Create-ServerFailoverEnv.ps1 failed, read Create-ServerFailoverEnv.ps1.log for detail." | Out-File -FilePath $startSignalFile -Append
}

function CreateShareFolder($fullPath)
{
    if(!(Test-Path $fullPath))
    {
        CMD /C "MKDIR $fullPath" 2>&1 | .\Write-Info.ps1
    }
    CMD /C "icacls $fullPath /grant $domainAdmin`:(OI)(CI)(F)" 2>&1 | .\Write-Info.ps1
}

function CheckConnectivity($computerName)
{
    # Build domain admin credentials for remote WMI access (SYSTEM doesn't have remote WMI rights)
    $domainAdminPwd = New-Object SecureString
    $config.Core.Password.ToCharArray() | ForEach-Object {$domainAdminPwd.AppendChar($_)}
    $wmiCreds = New-Object System.Management.Automation.PSCredential($domainAdmin, $domainAdminPwd)

    for($i=0;$i -lt 10;$i++)
    {
        try
        {
		    .\Write-Info.ps1 "Test TCP connection to computer: $computerName"
            Test-Connection -ComputerName $computerName -ErrorAction Stop

			.\Write-Info.ps1 "Test WMI connection to computer: $computerName"
            if ($computerName -eq $env:COMPUTERNAME) {
                $wmiObj = Get-WmiObject Win32_ComputerSystem -ErrorAction Stop
            } else {
                $wmiObj = Get-WmiObject Win32_ComputerSystem -Computername $computerName -Credential $wmiCreds -ErrorAction Stop
            }
            break
        }
        catch
        {
            .\Write-Info.ps1 "Get exception: $_"
            Start-Sleep 15
        }
    }

    if($i -ge 10)
    {
        .\Write-Error.ps1 "$computerName cannot be connected within 10 retries."
        Write-ConfigFailureSignal
        exit (ExitCode)
    }
}

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
$config = $null
try {
    $config = Get-Content -Path $protocolConfigFile -Raw | ConvertFrom-Json
}
catch {
    .\Write-Error.ps1 "Failed to parse config file: $_"
    exit 1
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$domain = (Get-WmiObject win32_computersystem).Domain
$domainAdmin = $config.Core.Username
$domainAdmin = "$domain\$domainAdmin"
$systemDrive = $env:SystemDrive

$clusterName = $config.Endpoints.Cluster.Name
$clusterNodes = @()
$clusterIps = @()
$generalFsIps = @()
$infraFsName = $config.Endpoints.InfrastructureFS.Name

$clusterNodeList = $config.Machines.PSObject.Properties | Where-Object {$_.Value.isclusternode -eq "true"} | Select-Object -ExpandProperty Value
foreach ($clusterNode in $clusterNodeList)
{
    $clusterNodes += $clusterNode.ComputerName
}

$clusterIpList = $config.Endpoints.Cluster.IpConfig
foreach ($clusterip in $clusterIpList)
{
    $clusterIps += $clusterip.Ip
}

$generalFsIpList = $config.Endpoints.GeneralFS.IpConfig
foreach ($generalFsIp in $generalFsIpList)
{
    $generalFsIps += $generalFsIp.Ip
}

#----------------------------------------------------------------------------
# Install Windows Features
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Install Windows Features"
Add-WindowsFeature Failover-Clustering
Add-WindowsFeature FS-BranchCache
Add-WindowsFeature FS-VSS-Agent
Add-WindowsFeature BranchCache
Add-WindowsFeature RSAT-Clustering
Add-WindowsFeature RSAT-File-Services
Add-WindowsFeature RSAT-AD-PowerShell

#----------------------------------------------------------------------------
# Get disk ready for cluster
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Get disk ready for cluster"
$disks = Get-Disk | Where-Object {$_.FriendlyName -match "MSFT Virtual HD"} | Sort-Object Size
$diskCount = $disks.count
if($diskCount -lt 3)
{
    .\Write-Info.ps1 "There are only $diskCount disks."
    .\Write-Info.ps1 "Cluster environment requires at least 3 disks."
    Write-ConfigFailureSignal
    exit (ExitCode)
} 

.\Write-Info.ps1 "Format all disks for cluster"
$diskLabelsDict = @{}
$isQuorumDisk = $true
foreach ($disk in $disks)
{
    $diskNumber = $disk.Number
    if($isQuorumDisk)
    {
        $volumeLabel = "Q$diskNumber"
        $isQuorumDisk = $false
    }
    else
    {
        $volumeLabel = "CLUSTER_DATA$diskNumber"
    }
    $diskLabelsDict.Add($diskNumber, $volumeLabel)
    
    # Only skip formatting if the disk already has a data partition with an NTFS volume.
    # GPT-initialized disks may have a Microsoft Reserved Partition (MSR) but no usable volume.
    $partition = Get-Partition -DiskNumber $diskNumber -ErrorAction SilentlyContinue |
        Where-Object { $_.Type -notin @('Reserved', 'System', 'Unknown') -and $_.Size -gt 0 }
    if($null -eq $partition)
    {
        $diskpartscript=@()

        .\Write-Info.ps1 "Online and format Disk $diskNumber"
        $diskpartscript += "select disk $diskNumber"
        $diskpartscript += "ATTRIBUTES DISK CLEAR READONLY"
        $diskpartscript += "online disk noerr"
        $diskpartscript += "clean"
        $diskpartscript += "convert mbr noerr"
        $diskpartscript += "crea part prim noerr"
        $diskpartscript += "format fs=NTFS label=$volumeLabel quick noerr"

        $diskpartscript | diskpart.exe
    }
}

#----------------------------------------------------------------------------
# Clean Cluster
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Clean Cluster"
try {
    .\Write-Info.ps1 "Clean cluster env in single-domain environment."
    $domainAdminPwd = New-Object SecureString
    $config.Core.Password.ToCharArray() | ForEach-Object {$domainAdminPwd.AppendChar($_)}

    $creds = New-Object System.Management.Automation.PSCredential($domainAdmin,$domainAdminPwd)

    # Remove stale AD computer accounts for all cluster endpoints
    $endpointNames = @($clusterName, $config.Endpoints.GeneralFS.Name, $config.Endpoints.ScaleoutFS.Name)
    if (-not [string]::IsNullOrEmpty($infraFsName)) { $endpointNames += $infraFsName }
    $filterStr = ($endpointNames | ForEach-Object { "Name -like `"$_`"" }) -join " -or "
    .\Write-Info.ps1 "Removing stale AD objects matching: $filterStr"
    $staleObjects = Get-ADComputer -Filter $filterStr -Credential $creds -ErrorAction SilentlyContinue
    if ($null -ne $staleObjects) {
        $staleObjects | ForEach-Object {
            .\Write-Info.ps1 "  Removing AD computer: $($_.Name)"
            $_ | Remove-ADObject -Recursive -Credential $creds -Confirm:$false
        }
    }

    # Also remove stale DNS records so Add-ClusterFileServerRole / Add-ClusterScaleOutFileServerRole
    # don't fail with "network name already used". AD object removal doesn't always clean DNS quickly.
    $dcName = ($config.Machines.PSObject.Properties | Where-Object { $_.Name -match "DC" } | Select-Object -First 1).Value.ComputerName
    $zoneName = $domain
    foreach ($epName in $endpointNames) {
        if ([string]::IsNullOrEmpty($epName)) { continue }
        try {
            Invoke-Command -ComputerName $dcName -Credential $creds -ScriptBlock {
                param($name, $zone)
                $records = Get-DnsServerResourceRecord -ZoneName $zone -Name $name -ErrorAction SilentlyContinue
                if ($null -ne $records) {
                    $records | Remove-DnsServerResourceRecord -ZoneName $zone -Name $name -Force -ErrorAction SilentlyContinue
                    Write-Host "  Removed DNS records for $name in $zone"
                }
            } -ArgumentList $epName, $zoneName -ErrorAction SilentlyContinue
        }
        catch {
            .\Write-Info.ps1 "  DNS cleanup for $epName skipped: $_" -ForegroundColor Yellow
        }
    }

    # Flush local DNS cache to pick up the changes
    ipconfig /flushdns 2>&1 | Out-Null
    .\Write-Info.ps1 "Clean cluster completed."
}
catch {
    Write-Warning "Clean Cluster failed: $_"
}

#----------------------------------------------------------------------------
# Create Cluster
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Create Cluster"
$cluster = $null
try {
    $cluster = Get-cluster | Where-Object {$_.Name -eq $clusterName}    
}
catch {
    Write-Warning "Get-Cluster failed"
}

if($null -eq $cluster)
{
    .\Write-Info.ps1 "Check cluster node connectivity"
    foreach($node in $clusterNodes)
    {
        CheckConnectivity $node
    }
	
    # Create cluster
    .\Write-Info.ps1 "Create cluster"
    New-Cluster -Name $clusterName -Node $clusterNodes -StaticAddress $clusterIps -NoStorage
    Start-Sleep 20

    .\Write-Info.ps1 "Check if cluster create succeed"
    $cluster = Get-cluster | Where-Object {$_.Name -eq $clusterName}
    if($null -eq $cluster)
    {
        .\Write-Info.ps1 "Create Cluster failed."
        Write-ConfigFailureSignal
        exit (ExitCode)
    }

    # Ensure all cluster networks allow client access (ClusterAndClient role = 3).
    # In Azure, secondary NICs lack a default gateway, so the cluster classifies those
    # networks as "Cluster only" (role 1). This prevents file server roles from binding
    # to secondary subnet IPs. Setting role = 3 fixes this.
    # NOTE: Property assignments on cluster objects are no-ops in PowerShell 7 because the
    # FailoverClusters module runs via WinPSCompatSession and returns deserialized objects.
    # All mutations must go through powershell.exe (Windows PowerShell 5.1).
    .\Write-Info.ps1 "Setting all cluster networks to ClusterAndClient role"
    Get-ClusterNetwork | ForEach-Object {
        if ($_.Role -ne 3) {
            .\Write-Info.ps1 "  Network '$($_.Name)' (Address: $($_.Address)) role $($_.Role) -> 3 (ClusterAndClient)"
            powershell.exe -NoProfile -Command "(Get-ClusterNetwork -Name '$($_.Name)').Role = 3"
        } else {
            .\Write-Info.ps1 "  Network '$($_.Name)' (Address: $($_.Address)) already ClusterAndClient"
        }
    }

    .\Write-Info.ps1 "Set cluster quorum"
    $disks = Get-Disk | Where-Object { $_.FriendlyName -match "MSFT Virtual HD" }
    $Script:diskResourcesDict = @{}
    foreach ($disk in $disks) {
        if ($diskLabelsDict[$disk.Number] -match "Q\d") {
            $diskResource = Add-ClusterDisk $disk
            $diskOnlineRetry = 0
            while ($diskResource.State -ne "Online" -and $diskOnlineRetry -lt 30) {
                Start-Sleep 5
                $diskResource = Start-ClusterResource -Name $diskResource.Name -ErrorAction SilentlyContinue
                $diskOnlineRetry++
            }

            $Script:diskResourcesDict.Add($disk.Number, $diskResource.Name)
            break
        }
    }

    foreach ($disk in $disks) {
        if ($diskLabelsDict[$disk.Number] -match "Q\d") {
            $quorumDiskResourceName = $Script:diskResourcesDict[$disk.Number]
            .\Write-Info.ps1 "Set $quorumDiskResourceName as the cluster quorum disk"
            Set-ClusterQuorum -DiskWitness $quorumDiskResourceName
            break
        }
    }

    .\Write-Info.ps1 "Add other disks to cluster"
    foreach ($disk in $disks) {
        if ($diskLabelsDict[$disk.Number] -notmatch "Q\d") {
            $diskResource = Add-ClusterDisk $disk
            $diskOnlineRetry = 0
            while ($diskResource.State -ne "Online" -and $diskOnlineRetry -lt 30) {
                Start-Sleep 5
                $diskResource = Start-ClusterResource -Name $diskResource.Name -ErrorAction SilentlyContinue
                $diskOnlineRetry++
            }

            $Script:diskResourcesDict.Add($disk.Number, $diskResource.Name)
        }
    }
}

#----------------------------------------------------------------------------
# Adding storage disk to cluster
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Adding storage disk to cluster"

.\Write-Info.ps1 "Check available disk number"
$Storages = Get-ClusterResource | Where-Object {$_.ResourceType -eq "Physical Disk"}
if($Storages.Count -lt 2)
{
    .\Write-Info.ps1 "At lease 2 available storages are required for File Sharing Cluster ENV."
    Write-ConfigFailureSignal
    exit (ExitCode)
}

.\Write-Info.ps1 "Adding General disk"
$SMBGeneralDisk = Get-ClusterResource | Where-Object {$_.Name -eq "SMBGeneralDisk"}
if($null -eq $SMBGeneralDisk)
{
    .\Write-Info.ps1 "Pick one disk from available storage for general disk"
    $clusterResources = Get-ClusterResource | Where-Object {$_.OwnerGroup -eq "Available Storage" -and $_.ResourceType -eq "Physical Disk"}
    $SMBGeneralDisk = $clusterResources | Select-Object -First 1

    foreach ($diskNumber in $Script:diskResourcesDict.Keys) {
        if ($Script:diskResourcesDict[$diskNumber] -eq $SMBGeneralDisk.Name) {
            $Script:SMBGeneralDiskLabel = $diskLabelsDict[$diskNumber]
            break
        }
    }

    $originalDiskName = $SMBGeneralDisk.Name
    powershell.exe -NoProfile -Command "(Get-ClusterResource -Name '$originalDiskName').Name = 'SMBGeneralDisk'"
    .\Write-Info.ps1 "Renamed '$originalDiskName' to 'SMBGeneralDisk'"
}

# If SMBGeneralDiskLabel not set (re-run where cluster already existed), resolve it from current disk state
if ([string]::IsNullOrEmpty($Script:SMBGeneralDiskLabel)) {
    .\Write-Info.ps1 "Resolving general disk label from existing cluster resource..."
    $generalDiskResource = Get-ClusterResource -Name "SMBGeneralDisk" -ErrorAction SilentlyContinue
    if ($null -ne $generalDiskResource) {
        $diskId = ($generalDiskResource | Get-ClusterParameter -Name DiskIdGuid -ErrorAction SilentlyContinue).Value
        $clusterDisk = Get-Disk | Where-Object { $_.Guid -eq $diskId }
        if ($null -ne $clusterDisk) {
            $vol = Get-Partition -DiskNumber $clusterDisk.Number -ErrorAction SilentlyContinue | Get-Volume -ErrorAction SilentlyContinue
            if ($null -ne $vol) {
                $Script:SMBGeneralDiskLabel = $vol.FileSystemLabel
                .\Write-Info.ps1 "Resolved general disk label: $($Script:SMBGeneralDiskLabel)"
            }
        }
    }
}

.\Write-Info.ps1 "Adding Scaleout disk"
$csv = Get-ClusterSharedVolume
if($null -eq $csv)
{
    .\Write-Info.ps1 "Pick one disk from available storage for scaleout disk"
    $clusterResources = Get-ClusterResource | Where-Object {$_.OwnerGroup -eq "Available Storage" -and $_.ResourceType -eq "Physical Disk" -and $_.Name -ne "SMBGeneralDisk"}
    $scaleoutDisk = $clusterResources | Select-Object -First 1
    if ($null -eq $scaleoutDisk) {
        .\Write-Info.ps1 "No available disk for scaleout volume." -ForegroundColor Red
        Write-ConfigFailureSignal
        exit (ExitCode)
    }
    .\Write-Info.ps1 "Add the disk as cluster shared volume: $($scaleoutDisk.Name)"
    Add-ClusterSharedVolume -Name $scaleoutDisk.Name
    Start-Sleep 10
    $csv = Get-ClusterSharedVolume
}

.\Write-Info.ps1 "Update SMBScaleOutDisk name"
if ($null -ne $csv) {
    $originalCsvName = $csv.Name
    powershell.exe -NoProfile -Command "(Get-ClusterSharedVolume -Name '$originalCsvName').Name = 'SMBScaleOutDisk'"
    .\Write-Info.ps1 "Renamed CSV '$originalCsvName' to 'SMBScaleOutDisk'"
} else {
    .\Write-Info.ps1 "Warning: No cluster shared volume found. Downstream steps may fail." -ForegroundColor Yellow
}

#----------------------------------------------------------------------------
# Create GeneralFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Add ClusterFileServerRole"
$fileServerGroup = Get-ClusterGroup | Where-Object {$_.Name -eq  $config.Endpoints.GeneralFS.Name}
if($null -eq $fileServerGroup)
{
    # Try with all configured IPs first; if a secondary subnet IP fails
    # (e.g. no default gateway → cluster won't accept it as ClusterAndClient),
    # fall back to primary IP only.
    try {
        Add-ClusterFileServerRole -Name $config.Endpoints.GeneralFS.Name -Storage "SMBGeneralDisk" -StaticAddress $generalFsIps -ErrorAction Stop
    }
    catch {
        .\Write-Info.ps1 "Add-ClusterFileServerRole with all IPs failed: $_" -ForegroundColor Yellow
        if ($generalFsIps.Count -gt 1) {
            .\Write-Info.ps1 "Retrying with primary IP only ($($generalFsIps[0]))..." -ForegroundColor Yellow
            Add-ClusterFileServerRole -Name $config.Endpoints.GeneralFS.Name -Storage "SMBGeneralDisk" -StaticAddress $generalFsIps[0]
        }
        else {
            throw
        }
    }
}

#----------------------------------------------------------------------------
# Add shared folders to GeneralFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Change the owner of GeneralFS to $env:ComputerName to access the local path of shares."
Move-ClusterGroup -Name $config.Endpoints.GeneralFS.Name -Node $env:ComputerName
Start-Sleep 15
.\Write-Info.ps1 "Add shared folders to GeneralFS role"
$fileServerShare = Get-SmbShare | Where-Object {$_.Name -eq "SMBClustered" -and $_.ScopeName.ToLower() -eq $config.Endpoints.GeneralFS.Name.ToLower()}
if($null -eq $fileServerShare)
{
    .\Write-Info.ps1 "Get general disk volume"
    # Note: 
    # There are 3 disks with label prefix "CLUSTER_DATA" for general disk and cluster shared volumes.
    # The general disk's FileSystem is NTFS
    # The cluster shared volumes' FileSystem is CSVFS
    # Retry 5 minutes (30 * 10 s) in case the disk is not ready due to change cluster owner node.
    $retryTime = 30
    do
    {
        $drive = Get-WmiObject -Class Win32_volume | Where-Object {$_.FileSystem -eq "NTFS" -and $_.Label -eq $Script:SMBGeneralDiskLabel}
        if($null -eq $drive)
        {
            Start-Sleep 10
            $retryTime--
            .\Write-Info.ps1 "Retry to get general disk volume"
        }
    } while ($null -eq $drive -and $retryTime -gt 0)

    if($retryTime -le 0)
    {
        .\Write-Info.ps1 "Does not found general disk volume"
        Write-ConfigFailureSignal
        exit (ExitCode)
    }

    .\Write-Info.ps1 "Get available drive letter"
	$driveLetter = ""
	foreach ($letter in [char[]]([char]'F'..[char]'Z')) 
    { 
      	$driveLetter = $letter + ":"
        $logicaldisk = get-wmiobject win32_logicaldisk | Where-Object {$_.DeviceID -eq $driveLetter}
        if ($null -eq $logicaldisk -and (Test-Path -path $driveLetter) -eq $false)
        { 
            break
        } 
    } 
	.\Write-Info.ps1 "The available drive letter is: $driveLetter"

    .\Write-Info.ps1 "Assign drive letter to general disk volume"
    # Set-WmiInstance fails in PS7 because $drive is a deserialized WMI object.
    # Use powershell.exe to perform the assignment with a live WMI object.
    $volumeLabel = $Script:SMBGeneralDiskLabel
    powershell.exe -NoProfile -Command "
        `$vol = Get-WmiObject -Class Win32_Volume | Where-Object { `$_.FileSystem -eq 'NTFS' -and `$_.Label -eq '$volumeLabel' }
        if (`$vol) { Set-WmiInstance -InputObject `$vol -Arguments @{ DriveLetter = '$driveLetter' } }
    "
    Start-Sleep 10

    # Create share folders
    .\Write-Info.ps1 "Create share folder: $driveLetter\SMBClustered"
    CreateShareFolder "$driveLetter\SMBClustered"
    $generalfsShare1 = Get-SmbShare | Where-Object {$_.Name -eq "SMBClustered" -and $_.ScopeName.ToLower() -eq $config.Endpoints.GeneralFS.Name.ToLower()}
    if($null -eq $generalfsShare1)
    {
        New-SMBShare -name "SMBClustered" -ScopeName $config.Endpoints.GeneralFS.Name -Path "$driveLetter\SMBClustered" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache
	}

    .\Write-Info.ps1 "Create share folder: $driveLetter\SMBClusteredEncrypted"
    CreateShareFolder "$driveLetter\SMBClusteredEncrypted"
    $generalfsShare2 = Get-SmbShare | Where-Object {$_.Name -eq "SMBClusteredEncrypted" -and $_.ScopeName.ToLower() -eq $config.Endpoints.GeneralFS.Name.ToLower()}
    if($null -eq $generalfsShare2)
    {
	    New-SMBShare -name "SMBClusteredEncrypted" -ScopeName $config.Endpoints.GeneralFS.Name -Path "$driveLetter\SMBClusteredEncrypted" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache -EncryptData $true
    }
}

#----------------------------------------------------------------------------
# Azure cluster: GeneralFS virtual IPs are not routable without an Azure Load
# Balancer because Azure's SDN drops traffic to IPs not assigned to any NIC.
# Instead of setting /32 masks + ProbePort for a non-existent LB, we leave the
# default /24 masks from Add-ClusterFileServerRole and rely on hosts-file
# entries (configured by DSC HostsFileEntries) on client machines to resolve
# GeneralFS to the owning node's real IP.
#----------------------------------------------------------------------------

#----------------------------------------------------------------------------
# Create ScaleoutFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Create ScaleoutFS role"
$scaleOutGroup = Get-ClusterGroup | Where-Object {$_.Name -eq $config.Endpoints.ScaleoutFS.Name}
if($null -eq $scaleOutGroup)
{
    Add-ClusterScaleOutFileServerRole -Name $config.Endpoints.ScaleoutFS.Name
}

#----------------------------------------------------------------------------
# Add shared folders to ScaleoutFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Change the owner of ScaleOutFS and SMBScaleOutDisk to $env:ComputerName to access the local path of shares."
Move-ClusterGroup -Name $config.Endpoints.ScaleoutFS.Name -Node $env:ComputerName
Start-Sleep 5
Move-ClusterSharedVolume -Name "SMBScaleOutDisk" -Node $env:ComputerName
Start-Sleep 10

$retryTime = 30
do
{
    try
    {
        $catchIssue = $false

        .\Write-Info.ps1 "Add shared folders to ScaleoutFS role"
        .\Write-Info.ps1 "Create share folder: $systemDrive\clusterstorage\volume1\SMBClustered"
        CreateShareFolder "$systemDrive\clusterstorage\volume1\SMBClustered"
        $scaleOutShare = Get-SmbShare | Where-Object {$_.Name -eq "SMBClustered" -and $_.ScopeName.ToLower() -eq $config.Endpoints.ScaleoutFS.Name.ToLower()}
        if($null -eq $scaleOutShare)
        {
            New-SMBShare -name "SMBClustered" -ScopeName $config.Endpoints.ScaleoutFS.Name -Path "$systemDrive\ClusterStorage\Volume1\SMBClustered" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache
        }

        .\Write-Info.ps1 "Create share folder: $systemDrive\clusterstorage\volume1\SMBClusteredForceLevel2"
        # Note: Create SMBClusteredForceLevel2 for Oplock model
        CreateShareFolder "$systemDrive\clusterstorage\volume1\SMBClusteredForceLevel2"
        $ClusteredForceLevel2 = Get-SmbShare | Where-Object {$_.Name -eq "SMBClusteredForceLevel2" -and $_.ScopeName.ToLower() -eq $config.Endpoints.ScaleoutFS.Name.ToLower()}
        if($null -eq $ClusteredForceLevel2)
        {
            New-SMBShare -name "SMBClusteredForceLevel2" -ScopeName $config.Endpoints.ScaleoutFS.Name -Path "$systemDrive\ClusterStorage\Volume1\SMBClusteredForceLevel2" -FullAccess "$domainAdmin"
        }
    }
    catch
    {
        Start-Sleep 10
        $retryTime--
        $catchIssue = $true
    }

} while ($catchIssue -eq $true -and $retryTime -gt 0)

if($retryTime -le 0)
{
    .\Write-Error.ps1 "Failed to add shared folders to ScaleoutFS role."
    Write-ConfigFailureSignal
    exit (ExitCode)
}

#----------------------------------------------------------------------------
# Create infrastructure share before adding cluster shared volume
#----------------------------------------------------------------------------
$build = [environment]::OSVersion.Version.Build
if (($build -ge 17609) -and (![string]::IsNullOrEmpty($infraFsName)))
{
    $InfrastructureGroup = Get-ClusterGroup | Where-Object {$_.Name -eq $infraFsName}
    if($null -eq $InfrastructureGroup)
    {
        .\Write-Info.ps1 "Create InfraFS role"
        Add-ClusterScaleOutFileServerRole -Infrastructure -Name $infraFsName
        .\Write-Info.ps1 "Add infrastructure share"
        $clusterAvailableResources = Get-ClusterResource | Where-Object {$_.OwnerGroup -eq "Available Storage" -and $_.ResourceType -eq "Physical Disk" -and $_.Name -ne "SMBGeneralDisk"}
        if ($clusterAvailableResources.Count -lt 1)
        {
            .\Write-Error.ps1 "No available storage for infrastructure share."
            Write-ConfigFailureSignal
            exit (ExitCode)
        }
        $infraShare = $clusterAvailableResources | Select-Object -First 1
        Add-ClusterSharedVolume -Name $infraShare.Name
        .\Write-Info.ps1 "Check the availability of infrastructure share. \\$infraFsName\Volume1 should be available."
        if (!(Test-Path "\\$infraFsName\Volume1"))
        {
            .\Write-Error.ps1 "Failed to add infrastructure share."
            Write-ConfigFailureSignal
            exit (ExitCode)
        }
    }
}

#----------------------------------------------------------------------------
# Update FailoverThreshold for Cluster Group and File Server role
# so that they can exceed more than 1 failure tolerance during a short period
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Update FailoverThreshold for Cluster Group and File Server role"

foreach ($groupName in @("Cluster Group", $config.Endpoints.GeneralFS.Name, $config.Endpoints.ScaleoutFS.Name)) {
    $clusgp = Get-ClusterGroup -Name $groupName -ErrorAction SilentlyContinue
    if ($null -ne $clusgp) {
        try {
            powershell.exe -NoProfile -Command "(Get-ClusterGroup -Name '$groupName').FailoverThreshold = 1024"
            .\Write-Info.ps1 "  Set FailoverThreshold=1024 for '$groupName'"
        }
        catch {
            .\Write-Info.ps1 "  Warning: Could not set FailoverThreshold for '$groupName': $_" -ForegroundColor Yellow
        }
    } else {
        .\Write-Info.ps1 "  Warning: Cluster group '$groupName' not found, skipping FailoverThreshold" -ForegroundColor Yellow
    }
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Completed setup cluster failover ENV."
Pop-Location
Stop-Transcript
exit 0