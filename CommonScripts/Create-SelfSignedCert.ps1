# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Create-SelfSignedCert.ps1
# Purpose:        Create self signed certificate for server authentication.
# Requirements:   Windows Powershell 2.0
# Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
#                 Windows Server 2016 and later
#
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$CommonName
)

$MakeCertTool = (Get-ChildItem -Filter "makecert.exe" -Recurse).FullName
$ServerAuthenticationOid = "1.3.6.1.5.5.7.3.1"
$CertFile = "$env:SystemDrive\$CommonName.cer"
$CmdLine = "$MakeCertTool -r -pe -n `"CN=$CommonName`" -ss my -sr LocalMachine -a sha1 -sky exchange -eku $ServerAuthenticationOid -sp `"Microsoft RSA SChannel Cryptographic Provider`" -sy 12 $CertFile"
cmd.exe /c $CmdLine 2>&1 | Write-Output