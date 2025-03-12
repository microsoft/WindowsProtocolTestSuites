# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Create-DNSRecords.ps1
# Purpose:        Create static DNS records for Cluster and GeneralFS.
# Requirements:   Windows Powershell 5.0
# Supported OS:   Windows Server 2012 R2, Windows Server 2016, and later.
# Input parameter is 
#      workingDir              :  The working directory for the script execution
#      protocolConfigFile      :  Path to the XML configuration file for the current test environment
# Process:
#  1. Read the host names and IP addresses of Cluster and GeneralFS from XML configuration file.
#  2. Add static DNS records to resolve host names to host IP addresses.
##############################################################################

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Config.json")

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
        .\Write-Error.ps1 "No Config file found."
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

function Add-DnsRecord {
    param(
        [string]$DNSZone,
        [string]$HostName,
        [string]$HostIPv4Address
    )

    .\Write-Info.ps1 "Add a new DNS record for $HostName to resolve $HostName.$DNSZone to $HostIPv4Address"
    Add-DnsServerResourceRecordA -Name $HostName -ZoneName $DNSZone -AllowUpdateAny -IPv4Address $HostIPv4Address
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
    exit ExitCode
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
# Define DNS zone
$domainController = $config.Machines.DC
$dnsZone = $domainController.Domain

# Define hosts in cluster environment
$cluster = $config.Endpoints.Cluster
$generalfs = $config.Endpoints.GeneralFS

#----------------------------------------------------------------------------
# Create new DNS records for hosts
#----------------------------------------------------------------------------
# Do not create DNS records for 12R2 environment
$osMajorVer = [System.Environment]::OSVersion.Version.Major
if ($osMajorVer -ge 10) {
    foreach ($ip in $cluster.IpConfig.Ip) {
        Add-DnsRecord -DNSZone $dnsZone -HostName $cluster.Name -HostIPv4Address $ip
    }
    foreach ($ip in $generalfs.IpConfig.Ip) {
        Add-DnsRecord -DNSZone $dnsZone -HostName $generalfs.Name -HostIPv4Address $ip
    }
}
