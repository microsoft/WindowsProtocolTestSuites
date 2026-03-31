# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Create-DNSRecords.ps1
# Purpose:        Create static DNS records for all machines and endpoints
#                 (Cluster, GeneralFS, and individual machine A records).
# Requirements:   Windows Powershell 5.0
# Supported OS:   Windows Server 2012 R2, Windows Server 2016, and later.
# Input parameter is 
#      workingDir              :  The working directory for the script execution
#      protocolConfigFile      :  Path to the XML configuration file for the current test environment
# Process:
#  1. Read the host names and IP addresses from JSON configuration file.
#  2. Add static DNS records for all machines (including workgroup members).
#  3. Add static DNS records for endpoint virtual names (Cluster, GeneralFS).
##############################################################################

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
        .\Write-Error.ps1 "No Config file found."
        exit 1
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
    # Endpoint virtual names (Cluster, GeneralFS)
    foreach ($ip in $cluster.IpConfig.Ip) {
        Add-DnsRecord -DNSZone $dnsZone -HostName $cluster.Name -HostIPv4Address $ip
    }
    foreach ($ip in $generalfs.IpConfig.Ip) {
        Add-DnsRecord -DNSZone $dnsZone -HostName $generalfs.Name -HostIPv4Address $ip
    }

    # Machine A records — ensures workgroup machines (e.g., Storage) are resolvable via DNS
    foreach ($prop in $config.Machines.PSObject.Properties) {
        $machine = $prop.Value
        $name    = $machine.ComputerName
        if ($null -eq $name -or $null -eq $machine.IpConfig -or $machine.IpConfig.Count -eq 0) {
            continue
        }
        # Use the first IP for DNS (primary NIC)
        $ip = $machine.IpConfig[0].Ip
        if ([string]::IsNullOrWhiteSpace($ip)) {
            continue
        }
        # Skip if a record already exists (domain-joined machines register automatically)
        $existing = Get-DnsServerResourceRecord -ZoneName $dnsZone -Name $name -ErrorAction SilentlyContinue
        if ($null -eq $existing) {
            Add-DnsRecord -DNSZone $dnsZone -HostName $name -HostIPv4Address $ip
        }
        else {
            .\Write-Info.ps1 "DNS record for $name already exists, skipping."
        }
    }
}
