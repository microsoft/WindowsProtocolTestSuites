#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------
# Function: SetAutoLogon
# Usage   : Set the computer to be able to auto logon by the providing username 
#           and pasword.
#-----------------------------------------------------------------------------

Param 
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$domain,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$user,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$pwd
)

try
{
    Set-ItemProperty -path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name "AutoAdminLogon" -Value "1" -Force -ErrorAction Stop
    Set-ItemProperty -path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name "DefaultUserName" -Value $user -Force -ErrorAction Stop
    Set-ItemProperty -path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name "DefaultDomainName" -Value $domain -Force -ErrorAction Stop
    Set-ItemProperty -path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name "DefaultPassword" -Value $pwd -Force -ErrorAction Stop
    set-itemproperty -path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -name "AltDefaultUserName" -value "$user" -Force -ErrorAction Stop
	set-itemproperty -path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -name "AltDefaultDomainName" -value "$domain" -Force -ErrorAction Stop
}
catch
{
    throw "Unable to set auto logon with the given username and password. Error happened: "+$_.Exception.Message
}
