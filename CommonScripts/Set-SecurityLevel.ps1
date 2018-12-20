##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-SecurityLevel.ps1
## Purpose:        Set security level for this computer.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

$ObjWMIService = New-Object -comobject WbemScripting.SWbemLocator
$ObjWMIService.Security_.ImpersonationLevel = 3
$ObjWMIService.Security_.AuthenticationLevel = 6