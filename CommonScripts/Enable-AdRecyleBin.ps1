##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Enable-AdRecycleBin.ps1
## Purpose:        Enable the recyle bin feature for Active Directory Domain Service.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
	[string]$Domain
)

$DomainNc = "DC=" + $Domain.Replace(".", ",DC=")
Enable-ADOptionalFeature -Identity "CN=Recycle Bin Feature,CN=Optional Features,CN=Directory Service,CN=Windows NT,CN=Services,CN=Configuration,$DomainNc" -Scope ForestOrConfigurationSet -Target $Domain -Confirm:$false