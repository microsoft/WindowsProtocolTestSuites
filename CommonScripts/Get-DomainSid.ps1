##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Get-DomainSid.ps1
## Purpose:        Get domain objectSid and log it to system drive.
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

    [switch]$log
)

$DomainSid = (Get-ADDomain -Identity $Domain).DomainSID.Value

if($log)
{
    # Log Domain objectSid in the system drive
    $DomainSid.ToString() > "$env:SystemDrive\domainSid.txt"
}

return $DomainSid