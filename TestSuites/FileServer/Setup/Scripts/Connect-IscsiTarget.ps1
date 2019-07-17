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
    echo "Execute Connect-IscsiTarget.ps1 failed, read Connect-IscsiTarget.ps1.log for detail." >> $startSignalFile
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

if($storageServer -ne $null)
{
    $iscsiServerName = $storageServer.name
    $targetName = $storageServer.iscsitargetname
    $iscsiTargetIp = $storageServer.ip
}

if([System.String]::IsNullOrEmpty($iscsiServerName))
{
    $iscsiServerName = "Storage01"
}

if([System.String]::IsNullOrEmpty($targetName))
{
    $targetName = "ClusterTarget"
}

if([System.String]::IsNullOrEmpty($iscsiTargetIp))
{
    $iscsiTargetIp = $targetName
}


#----------------------------------------------------------------------------
# Set msiscsi service to Automatic start
#----------------------------------------------------------------------------
Write-Info.ps1 "Check if iscsi target server is connectable: $iscsiServerName"
for($i=0;$i -lt 60;$i++)
{
    try
    {
        Write-Info.ps1 "Test TCP connection to computer: $iscsiServerName"
        Test-Connection -ComputerName $iscsiServerName -ErrorAction Stop
        break
    }
    catch
    {
        Write-Info.ps1 "Get exception: $_"
        Start-Sleep 10
    }
}

if($i -ge 60)
{
    Write-Error.ps1 "$iscsiServerName cannot be connected within 10 minutes."
    Write-ConfigFailureSignal
    exit ExitCode
}

#----------------------------------------------------------------------------
# Set msiscsi service to Automatic start
#----------------------------------------------------------------------------
Write-Info.ps1 "Set msiscsi service to Automatic start"
Set-service msiscsi -StartupType Automatic -status Running
sleep 10

Write-Info.ps1 "Start msiscsi service."
$service = Get-Service -Name msiscsi
$retryTimes = 0
while($service.Status -ne "Running" -and $retryTimes -lt 5)
{
    Write-Info.ps1 "msiscsi service is not runing, try to start it..."
    Start-Service -InputObj $service -ErrorAction Continue
    Sleep 10
    $retryTimes++
    $service = Get-Service -Name msiscsi
}

if($i -ge 5)
{
    Write-Error.ps1 "Start msiscsi service failed within 5 retries."
    Write-ConfigFailureSignal
    exit ExitCode
}

#----------------------------------------------------------------------------
# Discover Iscsi Target
#----------------------------------------------------------------------------
for($i=0;$i -lt 5;$i++)
{
    Write-Info.ps1 "Discover Iscsi Target"
    iscsicli qaddtargetportal $iscsiservername 2>&1 | Write-Info.ps1
    if($LastExitCode -eq 0)
    {
        break
    }
    else
    {
        Write-Error.ps1 "Discover Iscsi Target failed."
    }
}

if($i -ge 5)
{
    Write-Error.ps1 "Discover Iscsi Target failed within 5 retries."
    Write-ConfigFailureSignal
    exit ExitCode
}

#----------------------------------------------------------------------------
# Quick login Target
#----------------------------------------------------------------------------
for($i=0;$i -lt 5;$i++)
{
    Write-Info.ps1 "Quick login Target"
    iscsicli QloginTarget iqn.1991-05.com.microsoft:$iscsiservername-$targetname-target 2>&1 | Write-Info.ps1
    if($LastExitCode -eq 0)
    {
        break
    }
    else
    {
        Write-Error.ps1 "Login Target failed."
    }
}

if($i -ge 5)
{
    Write-Error.ps1 "Login Target failed within 5 retries."
    Write-ConfigFailureSignal
    exit ExitCode
}

#----------------------------------------------------------------------------
# PersistentLoginTarget
#----------------------------------------------------------------------------
for($i=0;$i -lt 5;$i++)
{
    Write-Info.ps1 "PersistentLoginTarget"
    iscsicli PersistentLoginTarget iqn.1991-05.com.microsoft:$iscsiservername-$targetname-target T * * * * * * * * * * * * * * * 0 2>&1 | Write-Info.ps1
    if($LastExitCode -eq 0)
    {
        break
    }
    else
    {
        Write-Error.ps1 "Persistent Login Target failed."
    }
}

if($i -ge 5)
{
    Write-Error.ps1 "Persistent Login Target failed within 5 retries."
    Write-ConfigFailureSignal
    exit ExitCode
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0