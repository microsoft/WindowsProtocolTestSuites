###########################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
###########################################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Create-DNSRecords.ps1
## Purpose:        Create static DNS records for Cluster and GeneralFS.
## Requirements:   Windows Powershell 5.0
## Supported OS:   Windows Server 2012 R2, Windows Server 2016, and later.
## Input parameter is 
##      workingDir              :  The working directory for the script execution
##      protocolConfigFile      :  Path to the XML configuration file for the current test environment
## Process:
##  1. Read the host names and IP addresses of Cluster and GeneralFS from XML configuration file.
##  2. Add static DNS records to resolve host names to host IP addresses.
###########################################################################################

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

function Add-DnsRecord
{
    param(
        [string]$DNSZone,
        [string]$HostName,
        [string]$HostIPv4Address
    )

    Write-Info.ps1 "Add a new DNS record for $HostName to resolve $HostName.$DNSZone to $HostIPv4Address"
    Add-DnsServerResourceRecordA -Name $HostName -ZoneName $DNSZone -AllowUpdateAny -IPv4Address $HostIPv4Address
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
# Define DNS zone
$domainController = $config.lab.servers.vm | Where-Object { $_.role -eq "DC" } | Select-Object -First 1
$dnsZone = $domainController.domain

# Define hosts in cluster environment
$cluster = $config.lab.ha.cluster
$generalfs = $config.lab.ha.generalfs

#----------------------------------------------------------------------------
# Create new DNS records for hosts
#----------------------------------------------------------------------------
# Do not create DNS records for 12R2 environment
$osMajorVer = [System.Environment]::OSVersion.Version.Major
if ($osMajorVer -ge 10)
{
    Add-DnsRecord -DNSZone $dnsZone -HostName $cluster.name -HostIPv4Address $cluster.ip
    Add-DnsRecord -DNSZone $dnsZone -HostName $generalfs.name -HostIPv4Address $generalfs.ip
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0