##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-NetlogonRegKeyAndPolicy.ps1
## Purpose:        Set password change, netlogon, and create object rights for registry key and group policy for this computer.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
## Notice:         There must be a GptTmpl.txt template file exist to apply the policy
##
##############################################################################

Param
(
    [switch]$IsDc,
        
    [switch]$EnableGpConfig
)

# Prevents the computer from changing its account password automatically
reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon\Parameters /v DisablePasswordChange /t REG_DWORD /d 1 /f

if($IsDc)
{
    # Prevents a domain controller from accepting requests from workstations to change their computer account passwords [MS-NRPC]
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon\Parameters /v RefusePasswordChange /t REG_DWORD /d 1 /f 
    
    # Configure a domain controller Netlogon service to depend on the DNS service
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon /v DependOnService /t REG_MULTI_SZ /d LanmanWorkstation\0LanmanServer\0DNS /f
}

if($EnableGpConfig)
{
    # Disable auto password change (Group Policy) [MS-DRSR] and set domain admins only to create computer object [MS-ADTS-LDAP] 
    $DomainPolicyId = (Get-GPO -Name "Default Domain Policy").id
    $PolicyFilePath = "$env:SystemDrive\Windows\SYSVOL\domain\Policies\{$DomainPolicyId}\MACHINE\Microsoft\Windows NT\SecEdit\GptTmpl.inf"
    $Template = Get-Content $(Get-Item "GptTmpl.txt")
    $DomainAdmins = Get-ADGroup -Identity "Domain Admins"
    $GpSid = $DomainAdmins.Sid
    $GpContent = $Template.Replace("%DomainAdminsGroupSid%",$GpSid)
    Set-Content $PolicyFilePath $GpContent
}