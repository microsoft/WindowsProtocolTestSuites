##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Find-PtfConfigFiles.ps1
## Purpose:        Find PtfConfig files in both \bin and source\server\testcode\testsuite folders.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
	[string]$FileName
)

$PtfConfigFiles = dir "$env:SystemDrive\MicrosoftProtocolTests" -Recurse | where{$_.Name -eq $FileName}
if($PtfConfigFiles.Length -eq 0)
{
    Write-Host "There is no PTF config files found."
}

return $PtfConfigFiles