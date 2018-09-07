##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-MsDsOtherSettings.ps1
## Purpose:        Set msDS-Other-Settings attribute: ADAMAllowADAMSecurityPrincipalsInConfigPartition=1.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateSet('Ds', 'Lds')]
	[string]$ServiceType,

	[string]$Domain,

    [int]$LdsPort,

    [string]$LdsInstanceId,

    [string]$LdsUserName,
    
    [string]$LdsUserPwd
)

if($ServiceType -eq 'Ds')
{
    $DomainNc = "DC=" + $Domain.Replace(".", ",DC=")
    
    $DataPath = ".\Set-MsDsOtherSettingsForDs.txt"
    if(Test-Path -Path $DataPath)
    {
        Remove-item $DataPath
    }
    $DataFile = New-Item -Type file -Path $DataPath
    "dn:cn=directory service,cn=windows NT,cn=services,cn=configuration,$DomainNc">>$DataFile
    "changetype:modify">>$DataFile
    "replace:msds-other-settings">>$DataFile
    "msds-other-settings:ADAMAllowADAMSecurityPrincipalsInConfigPartition=1">>$DataFile
    "msds-other-settings:DisableVLVSupport=0">>$DataFile
    "msds-other-settings:DynamicObjectDefaultTTL=86400">>$DataFile
    "msds-other-settings:DynamicObjectMinTTL=900">>$DataFile
    "-">>$DataFile
    cmd.exe /c ldifde -v -i -f $DataPath | Write-Output
}
else
{
    $DataPath = ".\Set-MsDsOtherSettingsForLds.txt"
    if(Test-Path -Path $DataPath)
    {
        Remove-item $DataPath
    }
    $DataFile = New-Item -Type file -Path $DataPath
    "dn:cn=directory service,cn=windows NT,cn=services,cn=configuration,$LdsInstanceId">>$DataFile
    "changetype:modify">>$DataFile
    "replace:msds-other-settings">>$DataFile
    "msds-other-settings:ADAMAllowADAMSecurityPrincipalsInConfigPartition=1">>$DataFile
    "msds-other-settings:ADAMDisableLogonAuditing=0">>$DataFile
    "msds-other-settings:ADAMDisablePasswordPolicies=0">>$DataFile
    "msds-other-settings:ADAMDisableSPNRegistration=0">>$DataFile
    "msds-other-settings:ADAMLastLogonTimestampWindow=7">>$DataFile
    "msds-other-settings:DisableVLVSupport=0">>$DataFile
    "msds-other-settings:DynamicObjectDefaultTTL=86400">>$DataFile
    "msds-other-settings:DynamicObjectMinTTL=900">>$DataFile
    "msds-other-settings:MaxReferrals=3">>$DataFile
    "msds-other-settings:ReferralRefreshInterval=5">>$DataFile
    "msds-other-settings:RequireSecureProxyBind=1">>$DataFile
    "msds-other-settings:RequireSecureSimpleBind=0">>$DataFile
    "msds-other-settings:SelfReferralsOnly=0">>$DataFile
    "-">>$DataFile
    cmd.exe /c ldifde -v -i -f $DataPath -s localhost:$LdsPort -b $LdsUserName $Domain $LdsUserPwd | Write-Output
}