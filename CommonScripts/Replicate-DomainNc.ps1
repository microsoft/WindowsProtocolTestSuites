##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Replicate-DomainNc.ps1
## Purpose:        Replicate domain naming context from one DC to another.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Domain,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$SourceHost,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$TargetHost
)

$DomainNc = "DC=" + $Domain.Replace(".", ",DC=")
repadmin /replicate $TargetHost $SourceHost $DomainNc