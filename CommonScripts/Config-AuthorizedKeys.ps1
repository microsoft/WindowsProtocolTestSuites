# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Config-AuthorizedKeys.ps1
# Purpose:        This script will copy authorized_keys to domain or local administrator's .ssh folder,
#                 and it is required for Windows Server which want to be remoted for configuring PowerShell Core remoting over ssh.
# Version:        2.0 (7 Feb, 2021)
#
##############################################################################

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Protocol.xml", [ValidateSet("CreateTask", "StartTask")]$action = "CreateTask")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -Parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"
$systemDrive = $env:SystemDrive

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function ExitCode() { 
    return $MyInvocation.ScriptLineNumber
}

#----------------------------------------------------------------------------
# If working dir is not exists, it will use scripts path as working path
#----------------------------------------------------------------------------
if (-not (Test-Path "$workingDir")) {
    $workingDir = $scriptPath
}

if (-not (Test-Path "$protocolConfigFile")) {
    $protocolConfigFile = "$workingDir\Protocol.xml"
    if (-not (Test-Path "$protocolConfigFile")) {
        Write-Error.ps1 "No Protocol.xml found."
        exit ExitCode
    }
}

#----------------------------------------------------------------------------
# Start logging using Start-Transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
Write-Info.ps1 "Get content from protocol config file"
[xml]$config = Get-Content "$protocolConfigFile"
if ($config -eq $null) {
    Write-Error.ps1 "ProtocolConfigFile $protocolConfigFile is not a valid XML file."
    exit ExitCode
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$hostName = [System.Net.Dns]::GetHostName()
$vm = $config.lab.servers.vm | Where-Object { $_.name -match $hostName }
$sshPath = ($vm.tools.tool | Where-Object { $_.name -match "Win32-OpenSSH-Certs" } | Select-Object -First 1).targetFolder

$dc = $config.lab.servers.vm | Where-Object { $_.role -match "DC" }
$adminUserName = $config.lab.core.username
if ($dc -eq $null) {
    # for non-domain environments, just get admin user name
    $userFolderName = $adminUserName
}
else {
    $dcName = $dc.name
    if ($dcName -match $hostName) {
        # for DC, just get admin user name
        $userFolderName = $adminUserName
    }
    else {
        $domainName = $dc.domain
        $domainNetBios = $domainName.Split(".")[0].ToUpper()

        $userFolderName = "$adminUserName.$domainNetBios"
    }
}

#----------------------------------------------------------------------------
# Copy authorized_keys
#----------------------------------------------------------------------------
if ($sshPath -eq $null) {
    $sshPath = "$systemDrive\OpenSSH-Win64"
}

$userFolderPath = "$systemDrive\Users\$userFolderName"
if (Test-Path $userFolderPath) {
    $keysPath = "$userFolderPath\.ssh"
    if (-not (Test-Path $keysPath)) {
        New-Item -ItemType Directory $keysPath
    }

    Copy-Item "$sshPath\authorized_keys" "$keysPath\authorized_keys" -Force
}

if (!(Test-Path "$systemDrive\Users\$adminUserName\.ssh")) {
        New-Item -ItemType Directory "$systemDrive\Users\$adminUserName\.ssh"
}

Copy-Item "$sshPath\authorized_keys" "$systemDrive\Users\$adminUserName\.ssh\authorized_keys" -Force

# Restart sshd service to take effect
Restart-Service sshd

if ($action -eq "CreateTask") {
    $taskAction = New-ScheduledTaskAction -Execute "PowerShell" -Argument "$($MyInvocation.MyCommand.Path) -action StartTask"
    $taskTrigger = New-ScheduledTaskTrigger -AtLogOn
    $taskPrincipal = New-ScheduledTaskPrincipal "SYSTEM"
    $taskSettings = New-ScheduledTaskSettingsSet
    $task = New-ScheduledTask -Action $taskAction -Trigger $taskTrigger -Principal $taskPrincipal -Settings $taskSettings
    Register-ScheduledTask "Config-AuthorizedKeys" -InputObject $task
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0