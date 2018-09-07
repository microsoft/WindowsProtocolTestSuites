##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-MsDsAllowToActOnBehalfOfOtherIdentity.ps1
## Purpose:        Set msDs-AllowToActOnBehalfOfOtherIdentity attribute.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
	[string]$DelegateHostFqdn,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
	[string]$LocalHostName
)

$DelegateHostName = $DelegateHostFqdn.Split(".")[0]
$DelegateHostObject = Get-ADComputer -Identity $DelegateHostName -Server $DelegateHostFqdn
Set-ADComputer -Identity $LocalHostName -PrincipalsAllowedToDelegateToAccount $DelegateHostObject   
