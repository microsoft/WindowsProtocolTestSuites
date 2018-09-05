##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Create-AdUserWithGroupMembership.ps1
## Purpose:        Create a user in active directory local domain and add it to groups indicated.
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

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Name,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Password,

    [String[]]$Groups
)

# Create user
Write-Host "Adding user $Name..."
net.exe user $Name $Password /add /domain

# Set UserPrincipalName for user
Write-Host "Setting UserPrincipalName attribute for user..."
$DomainNc = "DC=" + $Domain.Replace(".", ",DC=")
$Upn = [ADSI]"LDAP://CN=$Name,CN=Users,$DomainNc"
$Upn.userPrincipalName = "$Name@$Domain"
$UPN.Setinfo()

# Add to builtin groups
Write-Host "Adding to Builtin Groups..."
$GroupCollection = {$Groups}.Invoke()
if($GroupCollection -contains "Administrators")
{
    net.exe localgroup "Administrators" $Name /add
    $GroupCollection.Remove("Administrators")
}

# Add to domain groups
Write-Host "Adding to Domain Groups..."
foreach ($Group in $GroupCollection)
{
    net.exe group $Group $Name /add
}