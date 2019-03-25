##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Create-SelfSignedCert.ps1
## Purpose:        Create self signed certificate for server authentication.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$CommonName
)

$certStoreLocation = "cert:\LocalMachine\My"
$cert = New-SelfSignedCertificate -DnsName $CommonName -CertStoreLocation $certStoreLocation
Export-Certificate -Cert $cert -FilePath "$env:SystemDrive\$CommonName.cer" -Type CERT
