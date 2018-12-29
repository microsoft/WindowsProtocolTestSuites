##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Verify-ForestTrust.ps1
## Purpose:        Verify the bidirectional forest trust.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$RemoteForestName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$RemoteUser,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$RemotePassword
)

$LocalForest = [System.DirectoryServices.ActiveDirectory.Forest]::getCurrentForest()
$RemoteForestContext = New-Object System.DirectoryServices.ActiveDirectory.DirectoryContext("Forest", $RemoteForestName, $RemoteUser, $RemotePassword)
$RemoteForest = [System.DirectoryServices.ActiveDirectory.Forest]::getForest($RemoteForestContext)
$LocalForest.VerifyTrustRelationship($RemoteForest, "Bidirectional")