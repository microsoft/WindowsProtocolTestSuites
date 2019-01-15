##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Install-AdLds-Instance.ps1
## Purpose:        Install instance for Active Directory Lightweight Directory Service.
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

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [int]$PortNum,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [int]$SslPortNum,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$NewApplicationPartition,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Domain,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Username,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$Password,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [int]$ForestFuncLvl,
    
    [switch]$Log
)

$DomainNc = "DC=" + $Domain.Replace(".", ",DC=")
$NewApplicationPartitionToCreate = $NewApplicationPartition + "," + $DomainNc

$AccountUserName = $Domain + "\" + $Username

$ImportLdifFiles = '"MS-AdamSyncMetadata.LDF" "MS-ADLDS-DisplaySpecifiers.LDF" "MS-AZMan.LDF" "MS-InetOrgPerson.LDF" "MS-User.LDF" "MS-UserProxy.LDF" "MS-UserProxyFull.LDF"'
if($ForestFuncLvl -ge 6)
{
    $ImportLdifFiles += ' "MS-MembershipTransitive.LDF" "MS-ParentDistname.LDF" "MS-ReplValMetadataExt.LDF" "MS-SecretAttributeCARs.LDF" "MS-SetOwnerBypassQuotaCARs.LDF"'
}

.\InstallADLDS.ps1 -InstallType Unique `
                   -InstanceName $InstanceName `
                   -LocalLDAPPortToListenOn $PortNum `
                   -LocalSSLPortToListenOn $SslPortNum `
                   -NewApplicationPartitionToCreate $NewApplicationPartitionToCreate `
                   -ServiceAccount $AccountUserName `
                   -ServicePassword $Password `
                   -Administrator $AccountUserName `
                   -SourceUserName $AccountUserName `
                   -SourcePassword $Password `
                   -ImportLdifFiles $ImportLdifFiles

if($Log)
{
    # Log AD LDS info in TXT [MS-ADTS-Security]
    $PortNum > "$env:SystemDrive\port.txt"
    $SslPortNum >"$env:SystemDrive\sslport.txt"
}

# Wait for computer to be stable
Sleep 10