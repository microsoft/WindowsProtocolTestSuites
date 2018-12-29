##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Install-AdCertificateService-Feature.ps1
## Purpose:        Install Active Directory Certificate Service.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Import-Module ServerManager
Add-WindowsFeature Adcs-Cert-Authority -confirm:$false
Install-AdcsCertificationAuthority -CAType EnterpriseRootCA -Confirm:$false