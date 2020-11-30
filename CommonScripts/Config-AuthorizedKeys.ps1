# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Config-AuthorizedKeys.ps1
# Purpose:        This script will copy authorized_keys to domain administrator's .ssh folder,
#                 and it is required for Windows Server which want to be remoted for configuring PowerShell Core remoting over ssh.
# Version:        1.0 (27 Nov, 2020)
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
# Start logging using start-transcript cmdlet
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

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
Write-Info.ps1 "Get content from protocol config file"
[xml]$config = Get-Content "$protocolConfigFile"
if($config -eq $null)
{
    Write-Error.ps1 "protocolConfigFile $protocolConfigFile is not a valid XML file."
    exit ExitCode
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$node01 = $config.lab.servers.vm | Where {$_.role -match "ClusterNode01"}
$tool = $node01.tools| Where {$_.name -match "Win32-OpenSSH-Certs"}
$OpenSSHPath = $tool.targetFolder
$dc = $config.lab.servers.vm | Where {$_.role -match "DC"}
$dcUserName = $dc.username
$domainName  = $dc.domain
if($domainName -match "\.")
{
	$domainName = $domainName.Split(".")[0].ToUpper()
}

if($dcUserName -eq $null)
{
	$dcUserName = "Administrator"
}

#----------------------------------------------------------------------------
# Copy authorized_keys
#----------------------------------------------------------------------------
if($OpenSSHPath -eq $null)
{
    $OpenSSHPath = "$systemDrive\OpenSSH-Win64"
}

if (!(Test-Path "$systemDrive\Users\$dcUserName.$domainName\.ssh")) {
        New-Item -ItemType Directory "$systemDrive\Users\$dcUserName.$domainName\.ssh"
}

Copy "$OpenSSHPath\authorized_keys" "$systemDrive\Users\$dcUserName.$domainName\.ssh\authorized_keys" -Force

# restart sshd service to take affect
Restart-Service sshd

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0