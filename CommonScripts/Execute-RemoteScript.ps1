##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Execute-RemoteScript.ps1
## Purpose:        Execute script on a remote machine.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$RemoteHost,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Username,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Password,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$ScriptPath,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$ScriptName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$ScriptArgs
)

$SecurePwd = ConvertTo-SecureString $Password -AsPlainText -Force
$Cred = New-Object System.Management.Automation.PSCredential $Username, $SecurePwd
$RemoteSession = New-PSSession -ComputerName $RemoteHost -Credential $Cred
Invoke-Command -Session $RemoteSession `
               -ArgumentList $ScriptPath, $ScriptName, $ScriptArgs `
               -ScriptBlock {
                  Param (
                    $ScriptPathT,
                    $ScriptNameT,
                    $ScriptArgsT
                    )
                  Pushd $ScriptPathT
                  Invoke-Expression "$ScriptNameT $ScriptArgsT"
                }