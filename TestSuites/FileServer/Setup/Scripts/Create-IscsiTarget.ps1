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
if (!(Test-Path "$workingDir")) {
    $workingDir = $scriptPath
}

if (!(Test-Path "$protocolConfigFile")) {
    $protocolConfigFile = "$workingDir\Config.json"
    if (!(Test-Path "$protocolConfigFile")) {
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
function ExitCode() { 
    return $MyInvocation.ScriptLineNumber 
}

function Write-ConfigFailureSignal() {
    $startSignalFile = "$workingDir\Config_" + $env:COMPUTERNAME + "_FailureSignal.log"
    .\Write-Info.ps1 "Execute Create-IscsiTarget.ps1 failed, read Create-IscsiTarget.ps1.log for detail." >> $startSignalFile
}

function Start-iSCSITargetOnStart {
    Set-Service -Name WinTarget -StartupType Manual
    .\Write-Info.ps1 "Set WinTarget service to Manual" -ForegroundColor Cyan

    $scriptPath = "$env:SystemDrive\Scripts"
    $scriptFile = "$scriptPath\StartISCSI.cmd"

    if (!(Test-Path $scriptPath)) {
        New-Item -ItemType Directory -Path $scriptPath -Force
    }

    @"
@echo off
timeout /t 60 /nobreak
net start WinTarget
"@ | Set-Content -Path $scriptFile -Encoding ASCII

    .\Write-Info.ps1 "Startup script created at $scriptFile" -ForegroundColor Cyan

    $taskName = "Start iSCSI Target Service"
    $taskAction = New-ScheduledTaskAction -Execute "cmd.exe" -Argument "/c `"$scriptFile`""
    $taskTrigger = New-ScheduledTaskTrigger -AtStartup
    $taskPrincipal = New-ScheduledTaskPrincipal -UserId "SYSTEM" -LogonType ServiceAccount -RunLevel Highest
    $taskSettings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable

    Register-ScheduledTask -TaskName $taskName -Action $taskAction -Trigger $taskTrigger -Principal $taskPrincipal -Settings $taskSettings -Force

    .\Write-Info.ps1 "Scheduled task '$taskName' created successfully." -ForegroundColor Green
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
# TODO: NAMI Add to config validation
$storageServer = $config.Machines.Storage
$targetname = $storageServer.iSCSITargetName
if ([System.String]::IsNullOrEmpty($targetname)) {
    $targetname = "ClusterTarget"
}

$osVersion = .\Get-OSVersionNumber.ps1
if ([double]$osVersion -ge [double]"6.3") {
    $vhdExtension = "vhdx" # vhdx is required for Win2012R2
}
else {
    $vhdExtension = "vhd"
}
$iscsiDiskPath = $env:SystemDrive + "\iscsidisk"
$disk1 = "$iscsiDiskPath\disk1.$vhdExtension"
$disk2 = "$iscsiDiskPath\disk2.$vhdExtension"
$disk3 = "$iscsiDiskPath\disk3.$vhdExtension"
$diskq = "$iscsiDiskPath\diskq.$vhdExtension"

#----------------------------------------------------------------------------
# Install Windows Feature
#----------------------------------------------------------------------------

$StorageFeatures = @("File-Services", "FS-iSCSITarget-Server")
foreach ($feature in $StorageFeatures) {
    $state = Get-WindowsFeature -Name $feature
    if ($state.InstallState -ne "Installed") {
        .\Write-Info.ps1 "Install Windows Feature: $feature"
        Add-WindowsFeature $feature
    }
}



#----------------------------------------------------------------------------
# Create Iscsi Target
#----------------------------------------------------------------------------
for ($i = 0; $i -lt 5; $i++) {
    try {
        .\Write-Info.ps1 "Create Iscsi Target"
        $iscsiServerTarget = New-IscsiServerTarget -TargetName $targetname -ErrorAction Stop
        break
    }
    catch {			
        .\Write-Info.ps1 "Get exception: $_"
        Start-Sleep 10
    }
}

if ($null -eq $iscsiServerTarget) {
    Write-Error.ps1 "Failed to create Iscsi Server Target: $targetname within 5 retries."
    Write-ConfigFailureSignal
    exit ExitCode
}


#----------------------------------------------------------------------------
# Set Iscsi Target
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Set Iscsi Target"
Set-IscsiServerTarget -TargetName $targetname -InitiatorId IQN:*

#----------------------------------------------------------------------------
# Create Iscsi virtual disks
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Create Iscsi virtual disks"
New-IscsiVirtualdisk $disk1 -size 10GB
New-IscsiVirtualdisk $disk2 -size 10GB
New-IscsiVirtualdisk $disk3 -size 10GB
New-IscsiVirtualdisk $diskq -size 1GB

#----------------------------------------------------------------------------
# Map Iscsi virtual disk to Iscsi Target
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Map Iscsi virtual disk to Iscsi Target"
Add-IscsiVirtualDiskTargetMapping -TargetName $targetname -devicepath $disk1
Add-IscsiVirtualDiskTargetMapping -TargetName $targetname -devicepath $disk2
Add-IscsiVirtualDiskTargetMapping -TargetName $targetname -devicepath $disk3
Add-IscsiVirtualDiskTargetMapping -TargetName $targetname -devicepath $diskq

Start-iSCSITargetOnStart

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0