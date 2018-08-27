##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Install-AdLds-Feature.ps1
## Purpose:        Install Active Directory Lightweight Directory Service.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Import-Module ServerManager
Add-WindowsFeature ADLDS -IncludeAllSubFeature -Confirm:$false