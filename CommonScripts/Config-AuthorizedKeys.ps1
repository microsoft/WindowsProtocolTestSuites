# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Config-AuthorizedKeys.ps1
# Purpose:        This script will copy authorized_keys to .ssh folder for all users,
#                 and it is required for Windows Server to configure PowerShell Core remoting over SSH.
# Version:        2.0 (26 Jan, 2021)
#
##############################################################################

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Protocol.xml")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
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
    if (!(Test-Path "$protocolConfigFile")) {
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
$hostSettings = $config.lab.servers.vm | Where-Object { $_.name -eq $hostName } | Select-Object -First 1
$certsNode = $hostSettings.tools | Where-Object { $_.name -match "Win32-OpenSSH-Certs" }
$sshPath = $certsNode.targetFolder

#----------------------------------------------------------------------------
# Copy authorized_keys
#----------------------------------------------------------------------------
if ($sshPath -eq $null) {
    $sshPath = "$systemDrive\OpenSSH-Win64"
}

Get-ChildItem "$systemDrive\Users" | Where-Object { $_.PSIsContainer } | ForEach-Object {
    $keysPath = "$($_.FullName)\.ssh"
    if (-not (Test-Path $keysPath)) {
        New-Item  $keysPath -ItemType Directory
    }

    Copy-Item "$sshPath\authorized_keys" "$keysPath\authorized_keys" -Force
}

# Restart sshd service to take effect
Restart-Service sshd

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0