# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    $workingDir = $PSScriptRoot,
    [ValidateSet("CreateCheckerTask", "StartChecker")]
    [string]$action="CreateCheckerTask"
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$scriptName = $MyInvocation.MyCommand.Path
$env:Path += ";$scriptPath"
Push-Location $workingDir
#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Create checker task
#----------------------------------------------------------------------------
if($action -eq "CreateCheckerTask")
{
    .\Write-Info.ps1 "Create checker task."
    $TaskName = "ExecuteChecker"
    $Task = "PowerShell -ExecutionPolicy Bypass -File $scriptName -action StartChecker"
    # Create a task which will auto run current script every 5 minutes with StartChecker action.
    CMD.exe /C "schtasks /Create /RU SYSTEM /SC MINUTE /MO 5 /TN `"$TaskName`" /TR `"$Task`" /IT /F"  

    .\Write-Info.ps1 "Start checker after create the checker task."
    $action = "StartChecker" 
}

#----------------------------------------------------------------------------
# Start checker
#----------------------------------------------------------------------------
if($action -eq "StartChecker")
{
    $cluster = Get-Cluster
    if($null -ne $cluster)
    {        
        $clusterNodes = Get-ClusterNode
        $clusterGroups = Get-ClusterGroup
        $clusterQuorum = Get-ClusterQuorum
        $clusterSharedVolume = Get-ClusterSharedVolume
        $clusterResources = Get-ClusterResource
        $clusterNetworks = Get-ClusterNetwork
        $clusterNetworkInterfaces = Get-ClusterNetworkInterface
        $clusterGroup = $clusterGroups | Where-Object {$_.Name -eq "Cluster Group"}
        $currentHostServer = $clusterGroup.OwnerNode
        $clusterQuorumDisk = $clusterQuorum.QuorumResource
        $physicalDisks = $clusterResources | Where-Object {$_.ResourceType -match "Physical Disk"}

        .\Write-Info.ps1 "Print out Cluster basic info."
        .\Write-Info.ps1 ("Name: " + $cluster.Name)
        .\Write-Info.ps1 ("Current Host Server: " + $currentHostServer.Name + "  Status: " + $currentHostServer.State)
        .\Write-Info.ps1 ("Quorum Disk (Witness): " + $clusterQuorumDisk.Name + "  Status: " + $clusterQuorumDisk.State)

        .\Write-Info.ps1 "Print out Cluster Roles info."
        $clusterGroups | Where-Object {$_.GroupType -match "FileServer"} | Format-Table Name,State,GroupType,OwnerNode,Priority 

        .\Write-Info.ps1 "Print out Cluster Nodes info."
        $clusterNodes | Format-Table Name,State

        .\Write-Info.ps1 "Print out Cluster Disks info." 
        $physicalDisks | Format-Table Name,State,OwnerNode
        $clusterSharedVolume | Format-Table Name,State,OwnerNode

        .\Write-Info.ps1 "Print out Cluster Networks info."
        $clusterNetworks | Format-Table Name,State,Role,Address

        .\Write-Info.ps1 "Print out Cluster network adpaters info."    
        $clusterNetworkInterfaces | Sort-Object Name | Format-Table Node,Name,State,Network

        .\Write-Info.ps1 "Print out latest Cluster Events."    
        $clusterEvents = Get-WinEvent System -ErrorAction Ignore | Where-Object {($_.ProviderName -eq "Microsoft-Windows-FailoverClustering")}
        $latestCluterEvent = $clusterEvents | Select-Object -First 1 
        $latestCluterEvent | Format-List Id,LevelDisplayName,TimeCreated,Message

    }
    else
    {
        .\Write-Error.ps1 "Cannot find any cluster."
        .\Write-Info.ps1 "Check all disks."
        $disks = Get-Disk
        if($null -ne $disks)
        {
            .\Write-Info.ps1 "Print out disk status."
            .\Write-Info.ps1 ($disks | Format-Table Number,FriendlyName,IsClustered,Size,BusType,OperationalStatus,PartitionStyle | out-string)
        }
        else
        {
            .\Write-Error.ps1 "There is no available disks."
        }

        .\Write-Info.ps1 "Check Microsoft iSCSI Initiator Service."
        $service = get-service MSiSCSI
        if($null -ne $service)
        {
            .\Write-Info.ps1 "Print out MSiSCSI servcie status."
            .\Write-Info.ps1 ($service | Format-Table Name,Status,DisplayName | out-string)
        }
        else
        {
            .\Write-Error.ps1 "There is no MSiSCSI servcie."
        }

        .\Write-Info.ps1 "Check discovered iSCSI target portal."
        $iscsiTargetPortal = Get-iscsiTargetPortal
        if($null -ne $iscsiTargetPortal)
        {
            .\Write-Info.ps1 "Print out iSCSI target portal."
            .\Write-Info.ps1 ($iscsiTargetPortal | Format-List TargetPortalAddress,TargetPortalPortNumber | out-string)
        }
        else
        {
            .\Write-Error.ps1 "There is no discovered iSCSI target portal."
        }

        .\Write-Info.ps1 "View avaialbe iSCSI targets."
        $iscsiTarget = get-iscsiTarget
        if($null -ne $iscsiTarget)
        {
            .\Write-Info.ps1 "Print out avaialbe iSCSI targets."
            .\Write-Info.ps1 ($iscsiTarget | Format-List NodeAddress,IsConnected | out-string)
        }
        else
        {
            .\Write-Error.ps1 "There is no avaialbe iSCSI targets."
        }

        .\Write-Info.ps1 "View iSCSI Connections."
        $session = Get-IscsiSession    
        if($null -ne $session)
        {           
            .\Write-Info.ps1 "Print out iSCSI Connections."
            .\Write-Info.ps1 ($session | Format-List InitiatorNodeAddress,TargetNodeAddress,IsConnected,IsPersistent | out-string)
        }
        else
        {
            .\Write-Error.ps1 "There is no iSCSI Connections."
        }

    }

    .\Write-Info.ps1 "Finish checker."
    Start-Sleep 5 # To display above messages
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Pop-Location
Stop-Transcript
exit 0