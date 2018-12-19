##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Create-ManagedServiceAccount.ps1
## Purpose:        Create a managed service account in active directory local domain
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param 
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$AccountName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$AccountPassword,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Computer
)

Write-Host "Creating Managed Service Account"
$ManagedServiceAccountName = $AccountName
$ManagedServiceAccountPwd = $AccountPassword
Write-Host "Account name`: $ManagedServiceAccountName"
Write-Host "Account password`: $ManagedServiceAccountPwd"

Add-KdsRootKey -EffectiveTime ((Get-Date).AddHours(-10))
New-ADServiceAccount -Name $ManagedServiceAccountName -RestrictToSingleComputer -AccountPassword $(ConvertTo-SecureString -AsPlainText $ManagedServiceAccountPwd -Force) -Enabled $true
$ServiceAccount = Get-ADServiceAccount -Identity $ManagedServiceAccountName
$ComputerInstance = Get-ADComputer -Identity $Computer
Add-ADComputerServiceAccount -Identity $ComputerInstance -ServiceAccount $ServiceAccount