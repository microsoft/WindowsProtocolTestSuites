#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------
# Script: InstallADLDS
# Usage : Install AD LDS on the computer.
# Remark: A restart is needed after installation.
#-----------------------------------------------------------------------------

[CmdletBinding(DefaultParameterSetName="Unique")]
Param
(
    [string]$FilePath = ".\InstallADLDSAnswerFile.txt", # Answer File Full Path
    [ValidateSet("Unique","Replica")]
    [string]$InstallType = "Unique",
    [ValidateSet("Show","Hide")]
    [string]$ShowOrHideProgressGUI = "Show",
    [string]$InstanceName = "instance01",
    [Parameter(Mandatory=$true)]
    [string]$LocalLDAPPortToListenOn = "50000",
    [Parameter(Mandatory=$true)]
    [string]$LocalSSLPortToListenOn = "50001",
    [Parameter(ParameterSetName="Unique", Mandatory=$true)]
    [string]$NewApplicationPartitionToCreate = '"CN=Application,DC=Contoso,DC=Com"',
    [string]$DataFilesPath = "C:\Program Files\Microsoft ADAM\instance01\data",
    [string]$LogFilesPath = "C:\Program Files\Microsoft ADAM\instance01\data",
    [string]$ServiceAccount = "CONTOSO\Administrator",
    [string]$AddPermissionsToServiceAccount="Yes",
    [string]$ServicePassword = "Password01!",
    [string]$Administrator = "CONTOSO\Administrator",
    [ValidateSet("Show","Hide")]
    [string]$ShowInAddRemovePrograms = "Show",
    [string]$ImportLDIFFiles = '"MS-AdamSyncMetadata.LDF" "MS-ADLDS-DisplaySpecifiers.LDF" "MS-AZMan.LDF" "MS-InetOrgPerson.LDF" "MS-User.LDF" "MS-UserProxy.LDF" "MS-UserProxyFull.LDF"',
    [string]$SourceUserName = "CONTOSO\Administrator",
    [string]$SourcePassword = "Password01!",
    [Parameter(ParameterSetName="Replica")]
    [string]$ApplicationPartitionsToReplicate,          # Only valid for replica installations
    [Parameter(ParameterSetName="Replica")]
    [string]$ReplicationDataSourcePath,                 # Only valid for replica installations
    [Parameter(ParameterSetName="Replica")]
    [string]$ReplicationLogSourcePath,                  # Only valid for replica installations
    [Parameter(ParameterSetName="Replica", Mandatory=$true)]
    [string]$SourceServer,                              # Only valid for replica installations
    [Parameter(ParameterSetName="Replica", Mandatory=$true)]
    [string]$SourceLDAPPort                             # Only valid for replica installations    
)

Write-Host "Installing AD LDS (ADAM) ..." -ForegroundColor Yellow
Add-WindowsFeature ADLDS -IncludeAllSubFeature

if(!(Test-Path -Path "$FilePath"))
{
    Throw "Must create an answer file first: $FilePath."
}

# Replace password in the answer file
$ansFile = Get-Item $FilePath
attrib -R $ansFile.FullName
$answer = Get-Content $ansFile.FullName
if($InstallType -ne $null)
{
    $answer[$answer.IndexOf("InstallType=")] = "InstallType=$InstallType"
}
if($ShowOrHideProgressGUI -ne $null)
{
    $answer[$answer.IndexOf("ShowOrHideProgressGUI=")] = "ShowOrHideProgressGUI=$ShowOrHideProgressGUI"
}
if($InstanceName -ne $null)
{
    $answer[$answer.IndexOf("InstanceName=")] = "InstanceName=$InstanceName"
}
if($LocalLDAPPortToListenOn -ne $null)
{
    $answer[$answer.IndexOf("LocalLDAPPortToListenOn=")] = "LocalLDAPPortToListenOn=$LocalLDAPPortToListenOn"
}
if($LocalSSLPortToListenOn -ne $null)
{
    $answer[$answer.IndexOf("LocalSSLPortToListenOn=")] = "LocalSSLPortToListenOn=$LocalSSLPortToListenOn"
}
if($DataFilesPath -ne $null)
{
    $NewDataFilesPath = $DataFilesPath.Replace("instance01", $InstanceName)
    $answer[$answer.IndexOf("DataFilesPath=")] = "DataFilesPath=$NewDataFilesPath"
} 
if($LogFilesPath -ne $null)
{
    $NewLogFilesPath = $LogFilesPath.Replace("instance01", $InstanceName)
    $answer[$answer.IndexOf("LogFilesPath=")] = "LogFilesPath=$NewLogFilesPath"
}
if($ServiceAccount -ne $null)
{
    $answer[$answer.IndexOf("ServiceAccount=")] = "ServiceAccount=$ServiceAccount"
}
if($AddPermissionsToServiceAccount -ne $null)
{
    $answer[$answer.IndexOf("AddPermissionsToServiceAccount=")] = "AddPermissionsToServiceAccount=$AddPermissionsToServiceAccount"
}
if($ServicePassword -ne $null)
{
    $answer[$answer.IndexOf("ServicePassword=")] = "ServicePassword=$ServicePassword"
}
if($Administrator -ne $null)
{
    $answer[$answer.IndexOf("Administrator=")] = "Administrator=$Administrator"
}
if($ShowInAddRemovePrograms -ne $null)
{
    $answer[$answer.IndexOf("ShowInAddRemovePrograms=")] = "ShowInAddRemovePrograms=$ShowInAddRemovePrograms"
}
if($ImportLDIFFiles -ne $null)
{   
    $answer[$answer.IndexOf("ImportLDIFFiles=")] = "ImportLDIFFiles=$ImportLDIFFiles"
}
if($SourceUserName -ne $null)
{   
    $answer[$answer.IndexOf("SourceUserName=")] = "SourceUserName=$SourceUserName"
}
if($SourcePassword -ne $null)
{   
    $answer[$answer.IndexOf("SourcePassword=")] = "SourcePassword=$SourcePassword"
}

switch($PSCmdlet.ParameterSetName)
{
    "Unique"  
    {
        if($NewApplicationPartitionToCreate -ne $null)
        {   
            $answer[$answer.IndexOf("NewApplicationPartitionToCreate=")] = "NewApplicationPartitionToCreate=$NewApplicationPartitionToCreate"
        }
        $answer[$answer.IndexOf("ApplicationPartitionsToReplicate=")] = $null
        $answer[$answer.IndexOf("ReplicationDataSourcePath=")] = $null
        $answer[$answer.IndexOf("ReplicationLogSourcePath=")] = $null
        $answer[$answer.IndexOf("SourceServer=")] = $null
        $answer[$answer.IndexOf("SourceLDAPPort=")] = $null
        break
    }
    "Replica" 
    {
        $answer[$answer.IndexOf("NewApplicationPartitionToCreate=")] = $null
        if($ApplicationPartitionsToReplicate -ne $null)
        {   
            $answer[$answer.IndexOf("ApplicationPartitionsToReplicate=")] = "ApplicationPartitionsToReplicate=$ApplicationPartitionsToReplicate"
        }
        if($ReplicationDataSourcePath -ne $null)
        {   
            $answer[$answer.IndexOf("ReplicationDataSourcePath=")] = "ReplicationDataSourcePath=$ReplicationDataSourcePath"
        }
        if($ReplicationLogSourcePath -ne $null)
        {   
            $answer[$answer.IndexOf("ReplicationLogSourcePath=")] = "ReplicationLogSourcePath=$ReplicationLogSourcePath"
        }
        if($SourceServer -ne $null)
        {   
            $answer[$answer.IndexOf("SourceServer=")] = "SourceServer=$SourceServer"
        }
        if($SourceLDAPPort -ne $null)
        {   
            $answer[$answer.IndexOf("SourceLDAPPort=")] = "SourceLDAPPort=$SourceLDAPPort"
        }
        break
    }
}

[DateTime]$time = Get-Date
$newAnsFile = $ansFile.FullName.Replace(".txt", $time.ToFileTime().ToString()+".txt")
Set-Content -Path $newAnsFile -Value $answer
attrib +R $ansFile.FullName
attrib +R $newAnsFile

Write-Host "Install an LDS Instance" -ForegroundColor Yellow
cmd.exe /c $env:SystemRoot\ADAM\adaminstall.exe /answer:$newAnsFile 2>&1 | Write-Host
