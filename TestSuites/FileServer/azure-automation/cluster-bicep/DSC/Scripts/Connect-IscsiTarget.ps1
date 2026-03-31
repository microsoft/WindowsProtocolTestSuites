# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param($workingDir = $PSScriptRoot, $protocolConfigFile = "$workingDir\Config.json")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath"

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
        exit (ExitCode)
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
    "Execute Connect-IscsiTarget.ps1 failed, read Connect-IscsiTarget.ps1.log for detail." | Out-File -FilePath $startSignalFile -Append
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
    exit (ExitCode)
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$storageServer = $config.Machines.Storage
$targetname = $storageServer.iSCSITargetName

if($null -ne $storageServer)
{
    $iscsiServerName = $storageServer.ComputerName
    $targetName = $storageServer.iSCSITargetName
    $iscsiTargetIp = $storageServer.IpConfig.Ip
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
.\Write-Info.ps1 "Check if iscsi target server is connectable: $iscsiServerName"
for($i=0;$i -lt 60;$i++)
{
    try
    {
        .\Write-Info.ps1 "Test TCP connection to computer: $iscsiServerName"
        Test-Connection -ComputerName $iscsiServerName -ErrorAction Stop
        break
    }
    catch
    {
        .\Write-Info.ps1 "Get exception: $_"
        Start-Sleep 10
    }
}

if($i -ge 60)
{
    .\Write-Error.ps1 "$iscsiServerName cannot be connected within 10 minutes."
    Write-ConfigFailureSignal
    exit (ExitCode)
}

#----------------------------------------------------------------------------
# Set msiscsi service to Automatic start
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Set msiscsi service to Automatic start"
Set-service msiscsi -StartupType Automatic -status Running
Start-Sleep 10

.\Write-Info.ps1 "Start msiscsi service."
$service = Get-Service -Name msiscsi
$retryTimes = 0
while($service.Status -ne "Running" -and $retryTimes -lt 5)
{
    .\Write-Info.ps1 "msiscsi service is not runing, try to start it..."
    Start-Service -InputObj $service -ErrorAction Continue
    Start-Sleep 10
    $retryTimes++
    $service = Get-Service -Name msiscsi
}

if($retryTimes -ge 5)
{
    .\Write-Error.ps1 "Start msiscsi service failed within 5 retries."
    Write-ConfigFailureSignal
    exit (ExitCode)
}

#----------------------------------------------------------------------------
# Discover Iscsi Target
#----------------------------------------------------------------------------
for($i=0;$i -lt 5;$i++)
{
    .\Write-Info.ps1 "Discover Iscsi Target"
    iscsicli qaddtargetportal $iscsiservername 2>&1 | .\Write-Info.ps1
    if($LastExitCode -eq 0)
    {
        break
    }
    else
    {
        .\Write-Error.ps1 "Discover Iscsi Target failed."
    }
}

if($i -ge 5)
{
    .\Write-Error.ps1 "Discover Iscsi Target failed within 5 retries."
    Write-ConfigFailureSignal
    exit (ExitCode)
}

#----------------------------------------------------------------------------
# Quick login Target
#----------------------------------------------------------------------------
$iqnTarget = "iqn.1991-05.com.microsoft:$iscsiservername-$targetname-target"

# Check if already logged in (idempotency for re-runs)
$existingSession = Get-IscsiSession -ErrorAction SilentlyContinue | Where-Object { $_.TargetNodeAddress -eq $iqnTarget }
if ($null -ne $existingSession) {
    .\Write-Info.ps1 "Already logged in to iSCSI target $iqnTarget. Skipping login." -ForegroundColor Green
}
else {
    for($i=0;$i -lt 5;$i++)
    {
        .\Write-Info.ps1 "Quick login Target"
        iscsicli QloginTarget $iqnTarget 2>&1 | .\Write-Info.ps1
        if($LastExitCode -eq 0)
        {
            break
        }
        else
        {
            .\Write-Error.ps1 "Login Target failed."
        }
    }

    if($i -ge 5)
    {
        .\Write-Error.ps1 "Login Target failed within 5 retries."
        Write-ConfigFailureSignal
        exit (ExitCode)
    }
}

#----------------------------------------------------------------------------
# PersistentLoginTarget
#----------------------------------------------------------------------------
# Check if persistent login already exists (idempotency for re-runs)
$persistentTarget = iscsicli ListPersistentTargets 2>&1 | Select-String $iqnTarget
if ($null -ne $persistentTarget) {
    .\Write-Info.ps1 "Persistent login already exists for $iqnTarget. Skipping." -ForegroundColor Green
}
else {
    for($i=0;$i -lt 5;$i++)
    {
        .\Write-Info.ps1 "PersistentLoginTarget"
        iscsicli PersistentLoginTarget $iqnTarget T * * * * * * * * * * * * * * * 0 2>&1 | .\Write-Info.ps1
        if($LastExitCode -eq 0)
        {
            break
        }
        else
        {
            .\Write-Error.ps1 "Persistent Login Target failed."
        }
    }

    if($i -ge 5)
    {
        .\Write-Error.ps1 "Persistent Login Target failed within 5 retries."
        Write-ConfigFailureSignal
        exit (ExitCode)
    }
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0