##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Get-AdLdsInstanceId.ps1
## Purpose:        Get domain objectGuid and log it to system drive.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$InstanceName,

    [switch]$log
)

$ConfigNcInRegkey = Get-ItemProperty -Path HKLM:\SYSTEM\ControlSet001\Services\ADAM_$InstanceName\Parameters -name "Configuration NC"
$ConfigNc = $ConfigNcInRegkey."Configuration NC"
$InstanceId = $ConfigNc.Split(",")[1] #instance id (cn={xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx})
Write-Host "The instance id of $InstanceName`: $InstanceId"

if($log)
{
    # Log Domain objectGuid in the system drive
    $InstanceId > "$env:SystemDrive\ldsInstanceId.txt"
}

return $InstanceId