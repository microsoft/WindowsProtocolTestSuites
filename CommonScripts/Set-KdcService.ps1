##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Turnoff-KdcService.ps1
## Purpose:        Turn off the KDC service for this computer.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateSet('Start', 'Stop')]
    [String]$Type
)

if($Type -eq "Stop")
{
    Set-Service Kdc -StartupType "Manual"
    Stop-Service Kdc
}
else
{
    Set-service Kdc -StartupType "Automatic"
    Start-Service Kdc
}