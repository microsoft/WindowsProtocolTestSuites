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
        Write-Error.ps1 "No protocol.xml found."
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
    echo "Execute Create-IscsiTarget.ps1 failed, read Create-IscsiTarget.ps1.log for detail." >> $startSignalFile
}

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
[xml]$config = Get-Content "$protocolConfigFile"
if($config -eq $null)
{
    Write-Error.ps1 "protocolConfigFile $protocolConfigFile is not a valid XML file."
    exit ExitCode
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$storageServer = $config.lab.servers.vm | Where {$_.isiscsitarget -eq "true"}
$targetname = $storageServer.iscsitargetname
if([System.String]::IsNullOrEmpty($targetname)) 
{
    $targetname = "ClusterTarget"
}

$osVersion = Get-OSVersionNumber.ps1
if([double]$osVersion -ge [double]"6.3")
{
	$vhdExtension = "vhdx" # vhdx is required for Win2012R2
}
else
{
    $vhdExtension = "vhd"
}
$iscsiDiskPath = $env:SystemDrive + "\iscsidisk"
$disk1 = "$iscsiDiskPath\disk1.$vhdExtension"
$disk2 = "$iscsiDiskPath\disk2.$vhdExtension"
$diskq = "$iscsiDiskPath\diskq.$vhdExtension"

#----------------------------------------------------------------------------
# Install Windows Feature
#----------------------------------------------------------------------------
Write-Info.ps1 "Install Windows Feature"
Add-WindowsFeature File-Services,FS-iSCSITarget-Server

#----------------------------------------------------------------------------
# Create Iscsi Target
#----------------------------------------------------------------------------
for($i=0;$i -lt 5;$i++)
{
    try
    {
        Write-Info.ps1 "Create Iscsi Target"
        $iscsiServerTarget = New-IscsiServerTarget -TargetName $targetname -ErrorAction Stop
        break
    }
    catch
    {			
        Write-Info.ps1 "Get exception: $_"
        Start-Sleep 10
    }
}

if($iscsiServerTarget -eq $null)
{
    Write-Error.ps1 "Failed to create Iscsi Server Target: $targetname within 5 retries."
    Write-ConfigFailureSignal
    exit ExitCode
}


#----------------------------------------------------------------------------
# Set Iscsi Target
#----------------------------------------------------------------------------
Write-Info.ps1 "Set Iscsi Target"
Set-IscsiServerTarget -TargetName $targetname -InitiatorId IQN:*

#----------------------------------------------------------------------------
# Create Iscsi virtual disks
#----------------------------------------------------------------------------
Write-Info.ps1 "Create Iscsi virtual disks"
New-IscsiVirtualdisk $disk1 -size 10GB
New-IscsiVirtualdisk $disk2 -size 10GB
New-IscsiVirtualdisk $diskq -size 1GB

#----------------------------------------------------------------------------
# Map Iscsi virtual disk to Iscsi Target
#----------------------------------------------------------------------------
Write-Info.ps1 "Map Iscsi virtual disk to Iscsi Target"
Add-IscsiVirtualDiskTargetMapping -TargetName $targetname -devicepath $disk1
Add-IscsiVirtualDiskTargetMapping -TargetName $targetname -devicepath $disk2
Add-IscsiVirtualDiskTargetMapping -TargetName $targetname -devicepath $diskq

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0