# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Config-RSAKeys.ps1
# Purpose:        This script will copy a config file, id_rsa and id_rsa.pub under C:\id_rsa\.ssh to domain administrator's .ssh folder,
#                 and it is required for Windows node which want to remoting for configuring PowerShell Core remoting over ssh.
#                 The config file's filename is 'config', and in order to remoting without issues e.g. ignore strict host key verification or avoid unknown hosts need user to input confirm,
#                 its content should set as following,
#                 Host *
#                     StrictHostKeyChecking no
#                     UserKnownHostsFile=/dev/null
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
$driver = $config.lab.servers.vm | Where {$_.role -match "DriverComputer"}
$tool = $driver.tools| Where {$_.name -match "certs"}
$RSAKeysPath = $tool.targetFolder

$dc = $config.lab.servers.vm | Where {$_.role -match "DC"}
if($dc -eq $null)
{
    # for non-domain environments, just get username
    $userFoldername = $config.lab.core.username
}
else
{
    $dcUserName = $dc.username
    if($dcUserName -eq $null)
    {
	    $dcUserName = "Administrator"
    }

    $domainName  = $dc.domain
    if($domainName -match "\.")
    {
	    $domainName = $domainName.Split(".")[0].ToUpper()
    }

    $userFoldername = "$dcUserName.$domainName"
}


#----------------------------------------------------------------------------
# Copy authorized_keys
#----------------------------------------------------------------------------
if($RSAKeysPath -eq $null)
{
    $RSAKeysPath = "$systemDrive\id_rsa\.ssh"
}

Copy "$RSAKeysPath" "$systemDrive\Users\$userFoldername" -Recurse -Force

# restart sshd service to take affect
Restart-Service sshd

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0