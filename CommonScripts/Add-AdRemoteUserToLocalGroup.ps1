##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Add-AdRemoteUserToLocalGroups.ps1
## Purpose:        Add a user from another active directory domain to the local builtin groups indicated.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param 
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$RemoteDomain,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$RemoteUsername,

    [String[]]$LocalGroups
)

foreach ($LocalGroup in $LocalGroups)
{
    $AdministratorsGroup = [ADSI]("WinNT://$env:COMPUTERNAME/$LocalGroup,group")
    $AdministratorsGroup.add("WinNT://$RemoteDomain/$RemoteUsername,user")
}