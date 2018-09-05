##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-AdUserPwdPolicy.ps1
## Purpose:        Set Active Directory domain adminitrator password can not be changed and never expires.
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
    [String]$Name,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Password
)

$DomainNc = "DC=" + $Domain.Replace(".", ",DC=")
cmd /c dsmod user "CN=$Name,CN=users,$DomainNc" -pwd $Password -mustchpwd no -disabled no -canchpwd no -pwdneverexpires yes 2>&1 | Write-Output