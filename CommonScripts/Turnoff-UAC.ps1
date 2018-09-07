##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Turnoff-UAC
## Purpose:        Turn off the user account control.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Set-ItemProperty -path  HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System -name "EnableLUA" -value "0"
.\Check-ReturnValue.ps1