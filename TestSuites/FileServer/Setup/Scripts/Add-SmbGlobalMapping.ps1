#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Add-SmbGlobalMapping.ps1
## Purpose:        Add a SMB Global Mapping for an infrastructure share
## Requirements:   Windows Powershell 3.0
## Supported OS:   Windows Server 2019
## Copyright (c) Microsoft Corporation. All rights reserved.
##
##############################################################################

param(
    [string]$domain         = "contoso.com",
    [string]$username       = "administrator",
    [string]$passwd         = "Password01!",
    [string]$infraShare     = "\\InfraFS\Volume1",
    [string]$localSharePath = "Y:"
)

$build = [environment]::OSVersion.Version.Build
if ($build -ge 17609)
{
    if (Test-Path \\InfraFS\Volume1)
    {
        $fullUserName = "$domain\$username"
        $SecurePassword = ConvertTo-SecureString $passwd -AsPlainText -Force
        $creds = New-Object -TypeName PSCredential -ArgumentList $fullUserName, $SecurePassword

        Write-Info.ps1 "Add a SMB global mapping for $infraShare to $localSharePath"
        New-SmbGlobalMapping -RemotePath $infraShare -Credential $creds -LocalPath $localSharePath
    }
    else
    {
        Write-Info.ps1 "Error: the infrastructure share $infraShare does not exist."
    }
}
else
{
    Write-Info.ps1 "Warning: adding SMB global mapping can be run at build 17609 and later"
}
