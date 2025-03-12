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
        Write-Error.ps1 "No Config file found."
        exit ExitCode
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
    .\Write-Info.ps1 "Execute Create-ServerFailoverEnv.ps1 failed, read Create-ServerFailoverEnv.ps1.log for detail." >> $startSignalFile
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
    for($i=0;$i -lt 10;$i++)
    {
        try
        {
		    .\Write-Info.ps1 "Test TCP connection to computer: $computerName"
            Test-Connection -ComputerName $computerName -ErrorAction Stop
			
			.\Write-Info.ps1 "Test WMI connection to computer: $computerName"
            $wmiObj = Get-WmiObject Win32_ComputerSystem -Computername $computerName -ErrorAction Stop
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
        exit ExitCode
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
    Write-Error.ps1 "Failed to parse config file: $_"
    exit ExitCode
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
    exit ExitCode
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
    
    $partition = Get-Partition | Where-Object {$_.DiskNumber -eq $diskNumber}
    if($null -eq $partition)
    {
        $diskpartscript=@()
        
        .\Write-Info.ps1 "Online and format Disk $diskNumber"
        $diskpartscript += "select disk $diskNumber"
        $diskpartscript += "ATTRIBUTES DISK CLEAR READONLY"
        $diskpartscript += "online disk noerr" 
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
    $filterStr = "Name -like `"$clusterName`" -or Name -like `"$($config.Endpoints.GeneralFS.Name)`" -or Name -like `"$($config.Endpoints.ScaleoutFS.Name)`""
    Get-ADComputer -Filter $filterStr -Credential $creds | Remove-ADComputer -Credential $creds -Confirm:$false
}
catch {
    Write-Warning "Clean Cluster failed"
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
        exit ExitCode
    }

    .\Write-Info.ps1 "Set cluster quorum"
    $disks = Get-Disk | Where-Object { $_.FriendlyName -match "MSFT Virtual HD" }
    $Script:diskResourcesDict = @{}
    foreach ($disk in $disks) {
        if ($diskLabelsDict[$disk.Number] -match "Q\d") {
            $diskResource = Add-ClusterDisk $disk
            while ($diskResource.State -ne "Online") {
                $diskResource = Start-ClusterResource -Name $diskResource.Name
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
            while ($diskResource.State -ne "Online") {
                $diskResource = Start-ClusterResource -Name $diskResource.Name
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
    exit ExitCode
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

    $SMBGeneralDisk.Name = "SMBGeneralDisk"    
}

.\Write-Info.ps1 "Adding Scaleout disk"
$csv = Get-ClusterSharedVolume
if($null -eq $csv)
{
    .\Write-Info.ps1 "Pick one disk from available storage for scaleout disk"
    $clusterResources = Get-ClusterResource | Where-Object {$_.OwnerGroup -eq "Available Storage" -and $_.ResourceType -eq "Physical Disk" -and $_.Name -ne "SMBGeneralDisk"}
    $scaleoutDisk = $clusterResources | Select-Object -First 1
    .\Write-Info.ps1 "Add the disk as cluster shared volume"
    $scaleoutDisk | Add-ClusterSharedVolume
    Start-Sleep 10
    $csv = Get-ClusterSharedVolume
}

.\Write-Info.ps1 "Update SMBScaleOutDisk name"
$csv.Name = "SMBScaleOutDisk"

#----------------------------------------------------------------------------
# Create GeneralFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Add ClusterFileServerRole"
$fileServerGroup = Get-ClusterGroup | Where-Object {$_.Name -eq  $config.Endpoints.GeneralFS.Name}
if($null -eq $fileServerGroup)
{
	Add-ClusterFileServerRole -Name $config.Endpoints.GeneralFS.Name -Storage "SMBGeneralDisk" -StaticAddress $generalFsIps
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
            $retryTime -= 10
            .\Write-Info.ps1 "Retry to get general disk volume"
        }
    } while ($null -eq $drive -and $retryTime -gt 0)

    if($retryTime -le 0)
    {
        .\Write-Info.ps1 "Does not found general disk volume"
        Write-ConfigFailureSignal
        exit ExitCode
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
    Set-WmiInstance -input $drive -Arguments @{DriveLetter="$driveLetter";}
    Start-Sleep 10

    # Create share folders
    .\Write-Info.ps1 "Create share folder: $driveLetter\SMBClustered"
    CreateShareFolder "$driveLetter\SMBClustered"
    $generalfsShare1 = Get-SmbShare | Where-Object {$_.Name -eq "SMBClustered" -and $_.ScopeName.ToLower() -eq $config.Endpoints.GeneralFS.Name.ToLower()}
    if($null -eq $generalfsShare1)
    {
        New-SMBShare -name "SMBClustered" -Path "$driveLetter\SMBClustered" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache 
	}

    .\Write-Info.ps1 "Create share folder: $driveLetter\SMBClusteredEncrypted"
    CreateShareFolder "$driveLetter\SMBClusteredEncrypted"
    $generalfsShare2 = Get-SmbShare | Where-Object {$_.Name -eq "SMBClusteredEncrypted" -and $_.ScopeName.ToLower() -eq $config.Endpoints.GeneralFS.Name.ToLower()}
    if($null -eq $generalfsShare2)
    {
	    New-SMBShare -name "SMBClusteredEncrypted" -Path "$driveLetter\SMBClusteredEncrypted" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache -EncryptData $true			
    }
}

#----------------------------------------------------------------------------
# Modify IP resource of GeneralFS to make traffic go over load balancer on Azure
#----------------------------------------------------------------------------
$isAzureCluster = ($config.Core.regressiontype -match "Azure") -and ($config.Core.Scenario -match "Cluster")
if ($isAzureCluster) {
    $clusterNetworkName = (Get-ClusterNetwork)[0].Name
    $ipResourceName = (Get-ClusterResource | Where-Object { ($_.ResourceType -eq "IP Address") -and ($_.OwnerGroup -eq $config.Endpoints.GeneralFS.Name) })[0].Name
    $lbIP = $config.$config.Endpoints.GeneralFS.IpConfig[0].Ip
    $params = @{
        "Address" = "$lbIP"
        "ProbePort" = "59999"
        "SubnetMask" = "255.255.255.255"
        "Network" = "$clusterNetworkName"
        "OverrideAddressMatch" = 1
        "EnableDhcp" = 0
    }

    Get-ClusterResource -Name $ipResourceName | Set-ClusterParameter -Multiple $params

    # Take the IP resource offline and bring it online again
    Stop-ClusterResource -Name $ipResourceName
    Start-ClusterResource -Name $ipResourceName

    # Start GeneralFS role
    Start-ClusterGroup -Name $config.Endpoints.GeneralFS.Name
}

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
            New-SMBShare -name "SMBClustered" -Path "$systemDrive\ClusterStorage\Volume1\SMBClustered" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache
        }

        .\Write-Info.ps1 "Create share folder: $systemDrive\clusterstorage\volume1\SMBClusteredForceLevel2"
        # Note: Create SMBClusteredForceLevel2 for Oplock model
        CreateShareFolder "$systemDrive\clusterstorage\volume1\SMBClusteredForceLevel2"
        $ClusteredForceLevel2 = Get-SmbShare | Where-Object {$_.Name -eq "SMBClusteredForceLevel2" -and $_.ScopeName.ToLower() -eq $config.Endpoints.ScaleoutFS.Name.ToLower()}
        if($null -eq $ClusteredForceLevel2)
        {
            New-SMBShare -name "SMBClusteredForceLevel2" -Path "$systemDrive\ClusterStorage\Volume1\SMBClusteredForceLevel2" -FullAccess "$domainAdmin"
        }
    }
    catch
    {
        Start-Sleep 10
        $retryTime -= 10
        $catchIssue = $true
    }

} while ($catchIssue -eq $true -and $retryTime -gt 0)

if($retryTime -le 0)
{
    .\Write-Error.ps1 "Failed to add shared folders to ScaleoutFS role."
    Write-ConfigFailureSignal
    exit ExitCode
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
            exit ExitCode
        }
        $infraShare = $clusterAvailableResources | Select-Object -First 1
        $infraShare | Add-ClusterSharedVolume
        .\Write-Info.ps1 "Check the availability of infrastructure share. \\$infraFsName\Volume1 should be available."
        if (!(Test-Path "\\$infraFsName\Volume1"))
        {
            .\Write-Error.ps1 "Failed to add infrastructure share."
            Write-ConfigFailureSignal
            exit ExitCode
        }
    }
}

#----------------------------------------------------------------------------
# Update FailoverThreshold for Cluster Group and File Server role
# so that they can exceed more than 1 failure tolerance during a short period
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Update FailoverThreshold for Cluster Group and File Server role"

$clusgp = Get-ClusterGroup | Where-Object {$_.Name -eq "Cluster Group"}
$clusgp.FailoverThreshold = 1024

$clusgp = Get-ClusterGroup | Where-Object {$_.Name -eq $config.Endpoints.GeneralFS.Name}
$clusgp.FailoverThreshold = 1024

$clusgp = Get-ClusterGroup | Where-Object {$_.Name -eq $config.Endpoints.ScaleoutFS.Name}
$clusgp.FailoverThreshold = 1024

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Completed setup cluster failover ENV."
Pop-Location
Stop-Transcript
exit 0