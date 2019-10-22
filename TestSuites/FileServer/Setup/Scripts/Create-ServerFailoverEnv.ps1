#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Protocol.xml")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"
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
    $protocolConfigFile = "$workingDir\Protocol.xml"
    if(!(Test-Path "$protocolConfigFile")) 
    {
        .\Write-Error.ps1 "No protocol.xml found."
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
    echo "Execute Create-ServerFailoverEnv.ps1 failed, read Create-ServerFailoverEnv.ps1.log for detail." >> $startSignalFile
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
[xml]$config = Get-Content $protocolConfigFile
if($config -eq $null)
{
    .\Write-Error.ps1 "protocolConfigFile $protocolConfigFile is not a valid XML file."
    Write-ConfigFailureSignal
    exit ExitCode
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$domain = (Get-WmiObject win32_computersystem).Domain
$domainAdmin = $config.lab.core.username
$domainAdmin = "$domain\$domainAdmin"
$systemDrive = $env:SystemDrive

$clusterName = $config.lab.ha.cluster.name
$clusterNodes = @()
$clusterIps = @()
$generalFsIps = @()
$infraFsName = $config.lab.ha.infrastructurefs.name

$clusterNodeList = $config.lab.servers.vm | Where-Object {$_.isclusternode -eq "true"}
foreach ($clusterNode in $clusterNodeList)
{
    $clusterNodes += $clusterNode.name
}

$clusterIpList = $config.lab.ha.cluster.ip
foreach ($clusterip in $clusterIpList)
{
    $clusterIps += $clusterip
}

$generalFsIpList = $config.lab.ha.generalfs.ip
foreach ($generalFsIp in $generalFsIpList)
{
    $generalFsIps += $generalFsIp
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

.\Write-Info.ps1 "Format 3 disks for cluster"
for($i=0; $i -lt 3;$i++)
{
    if($i -eq 0)
    {
        $volumeLabel = "Q"
    }
    else
    {
        $volumeLabel = "CLUSTER_DATA"
    }
    $diskNumber = $disks[$i].Number
    $partition = Get-Partition | Where-Object {$_.DiskNumber -eq $diskNumber}
    if($partition -eq $null)
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

if($cluster -eq $null)
{
    .\Write-Info.ps1 "Check cluster node connectivity"
    foreach($node in $clusterNodes)
    {
        CheckConnectivity $node
    }
	
    # Create cluster
    .\Write-Info.ps1 "Create cluster"
    New-Cluster -Name $clusterName -Node $clusterNodes -StaticAddress $clusterIps
    Start-Sleep 20

    .\Write-Info.ps1 "Check if cluster create succeed"
    $cluster = Get-cluster | where {$_.Name -eq $clusterName}
    if($cluster -eq $null)
    {
        .\Write-Info.ps1 "Create Cluster failed."
        Write-ConfigFailureSignal
        exit ExitCode
    }    
}

#----------------------------------------------------------------------------
# Adding storage disk to cluster
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Adding storage disk to cluster"

.\Write-Info.ps1 "Check available disk number"
$Storages = Get-ClusterResource | where {$_.ResourceType -eq "Physical Disk"}
if($Storages.Count -lt 2)
{
    .\Write-Info.ps1 "At lease 2 available storages are required for File Sharing Cluster ENV."
    Write-ConfigFailureSignal
    exit ExitCode
}

.\Write-Info.ps1 "Adding General disk"
$SMBGeneralDisk = Get-ClusterResource | where {$_.Name -eq "SMBGeneralDisk"}
if($SMBGeneralDisk -eq $null)
{
    .\Write-Info.ps1 "Pick one disk from available storage for general disk"
    $clusterResources = Get-ClusterResource | where {$_.OwnerGroup -eq "Available Storage" -and $_.ResourceType -eq "Physical Disk"}
    $SMBGeneralDisk = $clusterResources | Select-Object -First 1
    $SMBGeneralDisk.Name = "SMBGeneralDisk"    
}

.\Write-Info.ps1 "Adding Scaleout disk"
$csv = Get-ClusterSharedVolume
if($csv -eq $null)
{
    .\Write-Info.ps1 "Pick one disk from available storage for scaleout disk"
    $clusterResources = Get-ClusterResource | where {$_.OwnerGroup -eq "Available Storage" -and $_.ResourceType -eq "Physical Disk" -and $_.Name -ne "SMBGeneralDisk"}
    $scaleoutDisk = $clusterResources | Select-Object -First 1
    .\Write-Info.ps1 "Add the disk as cluster shared volume"
    $scaleoutDisk | Add-ClusterSharedVolume
    sleep 10
    $csv = Get-ClusterSharedVolume
}

.\Write-Info.ps1 "Update SMBScaleOutDisk name"
$csv.Name = "SMBScaleOutDisk"

#----------------------------------------------------------------------------
# Create GeneralFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Add ClusterFileServerRole"
$fileServerGroup = Get-ClusterGroup | where {$_.Name -eq  $config.lab.ha.generalfs.name}
if($fileServerGroup -eq $null)
{
	Add-ClusterFileServerRole -Name $config.lab.ha.generalfs.name -Storage "SMBGeneralDisk" -StaticAddress $generalFsIps
}

#----------------------------------------------------------------------------
# Add shared folders to GeneralFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Change the owner of GeneralFS to $env:ComputerName to access the local path of shares."
Move-ClusterGroup -Name $config.lab.ha.generalfs.name -Node $env:ComputerName
Sleep 15
.\Write-Info.ps1 "Add shared folders to GeneralFS role"
$fileServerShare = Get-SmbShare | where {$_.Name -eq "SMBClustered" -and $_.ScopeName.ToLower() -eq $config.lab.ha.generalfs.name.ToLower()}
if($fileServerShare -eq $null)
{
    .\Write-Info.ps1 "Get general disk volume"
    # Note: 
    # There are 2 disks with label "CLUSTER_DATA" for general disk and cluster shared volume.
    # The general disk's FileSystem is NTFS
    # The cluster shared volume's FileSystem is CSVFS
    # Retry 5 minutes (30 * 10 s) in case the disk is not ready due to change cluster owner node.
    $retryTime = 30 
    do
    {
        $drive = gwmi -Class Win32_volume | where {$_.FileSystem -eq "NTFS" -and $_.Label -eq "CLUSTER_DATA"}
        if($drive -eq $null)
        {
            Sleep 10
            $retryTime -= 10
            .\Write-Info.ps1 "Retry to get general disk volume"
        }
    } while ($drive -eq $null -and $retryTime -gt 0)

    if($retryTime -le 0)
    {
        .\Write-Info.ps1 "Does not found general disk volume"
        Write-ConfigFailureSignal
        exit ExitCode
    }

    .\Write-Info.ps1 "Ger available drive letter"
	$driveLetter = ""
	foreach ($letter in [char[]]([char]'F'..[char]'Z')) 
    { 
      	$driveLetter = $letter + ":"
        $logicaldisk = get-wmiobject win32_logicaldisk | where {$_.DeviceID -eq $driveLetter}
        if ($logicaldisk -eq $null -and (Test-Path -path $driveLetter) -eq $false)
        { 
            break
        } 
    } 
	.\Write-Info.ps1 "The available drive letter is: $driveLetter"

    .\Write-Info.ps1 "Assign drive letter to general disk volume"
    Set-WmiInstance -input $drive -Arguments @{DriveLetter="$driveLetter";}
    Sleep 10

    # Create share folders
    .\Write-Info.ps1 "Create share folder: $driveLetter\SMBClustered"
    CreateShareFolder "$driveLetter\SMBClustered"
    $generalfsShare1 = Get-SmbShare | where {$_.Name -eq "SMBClustered" -and $_.ScopeName.ToLower() -eq $config.lab.ha.generalfs.name.ToLower()}
    if($generalfsShare1 -eq $null)
    {
        New-SMBShare -name "SMBClustered" -Path "$driveLetter\SMBClustered" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache 
	}

    .\Write-Info.ps1 "Create share folder: $driveLetter\SMBClusteredEncrypted"
    CreateShareFolder "$driveLetter\SMBClusteredEncrypted"
    $generalfsShare2 = Get-SmbShare | where {$_.Name -eq "SMBClusteredEncrypted" -and $_.ScopeName.ToLower() -eq $config.lab.ha.generalfs.name.ToLower()}
    if($generalfsShare2 -eq $null)
    {
	    New-SMBShare -name "SMBClusteredEncrypted" -Path "$driveLetter\SMBClusteredEncrypted" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache -EncryptData $true			
    }
}

#----------------------------------------------------------------------------
# Modify IP resource of GeneralFS to make traffic go over load balancer on Azure
#----------------------------------------------------------------------------
$isAzureCluster = ($config.lab.core.regressiontype -match "Azure") -and ($config.lab.ha.cluster -ne $null)
if ($isAzureCluster) {
    $clusterNetworkName = (Get-ClusterNetwork)[0].Name
    $ipResourceName = (Get-ClusterResource | Where-Object { ($_.ResourceType -eq "IP Address") -and ($_.OwnerGroup -eq $config.lab.ha.generalfs.name) })[0].Name
    $lbIP = $config.lab.ha.generalfs.ip
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
    Start-ClusterGroup -Name $config.lab.ha.generalfs.name
}

#----------------------------------------------------------------------------
# Create ScaleoutFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Create ScaleoutFS role"
$scaleOutGroup = Get-ClusterGroup | where {$_.Name -eq $config.lab.ha.scaleoutfs.name}
if($scaleOutGroup -eq $null)
{
    Add-ClusterScaleOutFileServerRole -Name $config.lab.ha.scaleoutfs.name
}

#----------------------------------------------------------------------------
# Add shared folders to ScaleoutFS role
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Change the owner of ScaleOutFS and SMBScaleOutDisk to $env:ComputerName to access the local path of shares."
Move-ClusterGroup -Name $config.lab.ha.scaleoutfs.name -Node $env:ComputerName
Sleep 5
Move-ClusterSharedVolume -Name "SMBScaleOutDisk" -Node $env:ComputerName
Sleep 10

$retryTime = 30
do
{
    try
    {
        $catchIssue = $false

        .\Write-Info.ps1 "Add shared folders to ScaleoutFS role"
        .\Write-Info.ps1 "Create share folder: $systemDrive\clusterstorage\volume1\SMBClustered"
        CreateShareFolder "$systemDrive\clusterstorage\volume1\SMBClustered"
        $scaleOutShare = Get-SmbShare | where {$_.Name -eq "SMBClustered" -and $_.ScopeName.ToLower() -eq $config.lab.ha.scaleoutfs.name.ToLower()}
        if($scaleOutShare -eq $null)
        {
            New-SMBShare -name "SMBClustered" -Path "$systemDrive\ClusterStorage\Volume1\SMBClustered" -FullAccess "$domainAdmin" -ContinuouslyAvailable $true -CachingMode BranchCache
        }

        .\Write-Info.ps1 "Create share folder: $systemDrive\clusterstorage\volume1\SMBClusteredForceLevel2"
        # Note: Create SMBClusteredForceLevel2 for Oplock model
        CreateShareFolder "$systemDrive\clusterstorage\volume1\SMBClusteredForceLevel2"
        $ClusteredForceLevel2 = Get-SmbShare | where {$_.Name -eq "SMBClusteredForceLevel2" -and $_.ScopeName.ToLower() -eq $config.lab.ha.scaleoutfs.name.ToLower()}
        if($ClusteredForceLevel2 -eq $null)
        {
            New-SMBShare -name "SMBClusteredForceLevel2" -Path "$systemDrive\ClusterStorage\Volume1\SMBClusteredForceLevel2" -FullAccess "$domainAdmin"
        }
    }
    catch
    {
        sleep 10
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
    $InfrastructureGroup = Get-ClusterGroup | where {$_.Name -eq $infraFsName}
    if($InfrastructureGroup -eq $null)
    {
        .\Write-Info.ps1 "Create InfraFS role"
        Add-ClusterScaleOutFileServerRole -Infrastructure -Name $infraFsName
        .\Write-Info.ps1 "Add infrastructure share"
        $clusterAvailableResources = Get-ClusterResource | where {$_.OwnerGroup -eq "Available Storage" -and $_.ResourceType -eq "Physical Disk" -and $_.Name -ne "SMBGeneralDisk"}
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

$clusgp = Get-ClusterGroup | where {$_.Name -eq "Cluster Group"}
$clusgp.FailoverThreshold = 1024

$clusgp = Get-ClusterGroup | where {$_.Name -eq $config.lab.ha.generalfs.name}
$clusgp.FailoverThreshold = 1024

$clusgp = Get-ClusterGroup | where {$_.Name -eq $config.lab.ha.scaleoutfs.name}
$clusgp.FailoverThreshold = 1024

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Completed setup cluster failover ENV."
Pop-Location
Stop-Transcript
exit 0