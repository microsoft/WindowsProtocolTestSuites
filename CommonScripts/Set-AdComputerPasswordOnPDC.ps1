##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-AdComputerPasswordOnPDC.ps1
## Purpose:        Set Active Directory computer password.
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
    [String]$PDCName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Name,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Password
)

# Set password on local computer
ksetup /SetComputerPassword $Password

# Set computer account password [MS-DRSR][MS-FRS2]
$DomainNc = "DC=" + $Domain.Replace(".", ",DC=")
$AdsiObj=[ADSI]"LDAP`://$PDCName/CN=$Name,OU=Domain Controllers,$DomainNc"
Write-Host $AdsiObj.distinguishedName
# Fix me: Does not work in DM, ad module not installed
$AdsiObj.SetPassword($Password)
$AdsiObj.SetInfo()
.\Check-ReturnValue.ps1