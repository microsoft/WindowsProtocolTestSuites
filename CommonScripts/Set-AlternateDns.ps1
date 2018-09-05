##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-AlternateDns.ps1
## Purpose:        Set alternate dns for this computer.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param 
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$alternateDns
)

$nics = Get-WmiObject -Class Win32_NetworkAdapterConfiguration -Filter IPEnabled=TRUE | where {$_.ServiceName -eq "netvsc" -or $_.ServiceName -eq "dc21x4VM"} | sort MACAddress
foreach($nic in $nics)
{
    netsh interface ipv4 add dnsservers $nic.interfaceindex $alternateDns index=2
}