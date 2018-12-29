##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-IisSslBinding.ps1
## Purpose:        Set SSL binding for IIS website to use the self-signed certificate
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

Write-Output "Getting the thumbprint of the certificate (Certificate Hash)..."
$CertHash = (.\Get-CertHash.ps1 -certIndex $CommonName "MY").Replace(" ","")
$AppId = [Guid]::NewGuid().ToString()
netsh http add sslcert ipport=0.0.0.0:443 certhash=$CertHash appid="{$AppId}" 2>&1 | Write-Output
cmd.exe /c "$env:windir\system32\inetsrv\appcmd.exe" set site "Default Web Site" /+"bindings.[protocol='https',bindingInformation='*:443:']" 2>&1 | Write-Output