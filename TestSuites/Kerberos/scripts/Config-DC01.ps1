#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
###############################################################################################
##
## Microsoft Windows PowerShell Scripting
## File:          Config-DC01.ps1
## Purpose:       Configure the Local Realm KDC computer for Kerberos Server test suite.
## Requirements:  Windows PowerShell 2.0
## Supported OS:  Windows Server 2012 or later versions
##
###############################################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$WorkingPath = "c:\temp"
$env:Path += ";c:\temp;c:\temp\Scripts"

#-----------------------------------------------------------------------------------------------
# Check if the script was executed
#-----------------------------------------------------------------------------------------------
$ScriptsSignalFile = "$env:HOMEDRIVE\config-dc01computer.finished.signal"
if(Test-Path -Path $ScriptsSignalFile)
{
	Write-Info.ps1 "The script execution is complete." -foregroundcolor Red
	exit 0
}

#-----------------------------------------------------------------------------------------------
# Please run as Domain Administrator
# Starting script
#-----------------------------------------------------------------------------------------------
$dataFile = "$WorkingPath\Data\ParamConfig.xml"
if(Test-Path -Path $dataFile)
{
	[xml]$configFile = Get-Content -Path $dataFile
	$domainName 	= $configFile.Parameters.LocalRealm.RealmName
}
else
{
	Write-Info.ps1 "$dataFile not found.  Will keep the default setting of all the test context info..."
}

#-----------------------------------------------------------------------------------------------
# Create $logPath if not exist
#-----------------------------------------------------------------------------------------------
$logPath = $configFile.Parameters.LogPath
if(!(Test-Path -Path $logPath))
{
	New-Item -Type Directory -Path $logPath -Force
}

#-----------------------------------------------------------------------------------------------
# Create $logFile if not exist
#-----------------------------------------------------------------------------------------------
$logFile = $WorkingPath + "\" + $MyInvocation.MyCommand.Name + ".log"
if(!(Test-Path -Path $logFile))
{
	New-Item -Type File -Path $logFile -Force
}
Start-Transcript $logFile -Append

#-----------------------------------------------------------------------------------------------
# Write value for all the parameters
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "EXECUTING [Config-DC01.ps1] ..." -foregroundcolor cyan
Write-Info.ps1 "`$logPath = $logPath"
Write-Info.ps1 "`$logFile = $logFile"
Write-Info.ps1 "`$domainName = $domainName"

#-----------------------------------------------------------------------------------------------
# Begin to config DC01 computer
#-----------------------------------------------------------------------------------------------

#-----------------------------------------------------------------------------------------------
# Turn off windows firewall
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Turn off firewall"
cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Info.ps1

#-----------------------------------------------------------------------------------------------
#Create forest trust on local side
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Create forest trust relationship on local side ..." -ForegroundColor Yellow

$LocalForest = [System.DirectoryServices.ActiveDirectory.Forest]::GetCurrentForest()

try
{
    # Build trust relationship on local forest only
    $LocalForest.CreateLocalSideOfTrustRelationship($configFile.Parameters.TrustRealm.RealmName, "Bidirectional", $configFile.Parameters.TrustPassword)
}
# If trust relationship already exists
catch [System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectExistsException]
{
    Write-Info.ps1 "Trust relationship already exists."
}
catch
{
    throw "Failed to create trust relationship. Error: " + $_.Exception.Message
}

#-----------------------------------------------------------------------------------------------
# Add AD Users Accounts on DC01 for Kerberos test suite
#-----------------------------------------------------------------------------------------------
$user = $configFile.Parameters.LocalRealm.User01.Username
$password = $configFile.Parameters.LocalRealm.User01.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName -HomeDirectory "c:\home\" -HomeDrive "c:" -ProfilePath "c:\profiles\" -ScriptPath "c:\scripts\"
$group = $configFile.Parameters.LocalRealm.User01.Group
try
{
	Remove-ADGroup -Identity $group -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $group, does not exist!" -ForegroundColor Red
}
New-ADGroup -Name $group -GroupScope Global -GroupCategory Security
$aduser = Get-ADUser -Identity $user
Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

$user = $configFile.Parameters.LocalRealm.User02.Username
$password = $configFile.Parameters.LocalRealm.User02.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -CompoundIdentitySupported $true -Department "HR" -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

$user = $configFile.Parameters.LocalRealm.User03.Username
$password = $configFile.Parameters.LocalRealm.User03.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountNotDelegated $true -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

$user = $configFile.Parameters.LocalRealm.User04.Username
$password = $configFile.Parameters.LocalRealm.User04.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -ServicePrincipalNames "abc/$user" -TrustedForDelegation $true -UserPrincipalName $user@$domainName

$user = $configFile.Parameters.LocalRealm.User05.Username
$password = $configFile.Parameters.LocalRealm.User05.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user

$user = $configFile.Parameters.LocalRealm.User06.Username
$password = $configFile.Parameters.LocalRealm.User06.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName
Set-ADAccountControl -Identity $user -DoesNotRequirePreAuth $true

$user = $configFile.Parameters.LocalRealm.User07.Username
$password = $configFile.Parameters.LocalRealm.User07.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $false -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

$user = $configFile.Parameters.LocalRealm.User08.Username
$password = $configFile.Parameters.LocalRealm.User08.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -AccountExpirationDate 1/1/2011 -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

$user = $configFile.Parameters.LocalRealm.User09.Username
$password = $configFile.Parameters.LocalRealm.User09.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Get-ADDefaultDomainPasswordPolicy | Set-ADDefaultDomainPasswordPolicy -ComplexityEnabled $false -MinPasswordLength 6 -MaxPasswordAge 30.0 -LockoutThreshold 1 -LockoutObservationWindow 1.0 -LockoutDuration 365.0
gpupdate /force
Write-Info.ps1 "Creating new user account $user"
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName
$wrongPassword = "1234567"
$wrongPwd = ConvertTo-SecureString $wrongPassword -AsPlainText -Force
$wrongCred = New-Object -TypeName System.Management.Automation.PSCredential -Argument $user, $wrongPwd
try
{
	Set-ADUser -Identity $user -Credential $wrongCred
}
catch
{
	Write-Info.ps1 "Successfully Locked the $user account" -ForegroundColor green
}
Get-ADDefaultDomainPasswordPolicy | Set-ADDefaultDomainPasswordPolicy -ComplexityEnabled $true -MinPasswordLength 7 -MaxPasswordAge 42.0 -LockoutThreshold 0
gpupdate /force

$user = $configFile.Parameters.LocalRealm.User10.Username
$password = $configFile.Parameters.LocalRealm.User10.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName
$hours = New-object byte[] 21
$replaceHashTable = New-Object HashTable
$replaceHashTable.Add("logonHours", $hours)
$replaceHashTable.Add("description", "This user is set to be always out of logon hours")
Set-ADUser -Identity $user -Replace $replaceHashTable

$user = $configFile.Parameters.LocalRealm.User11.Username
$password = $configFile.Parameters.LocalRealm.User11.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -ChangePasswordAtLogon $true -DisplayName $user -Enabled $true -SamAccountName $user -UserPrincipalName $user@$domainName
$aduser = Get-ADUser -Identity $user
Set-ADAccountControl -Identity $user -DoesNotRequirePreAuth $true

$user = $configFile.Parameters.LocalRealm.User12.Username
$password = $configFile.Parameters.LocalRealm.User12.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $false -ChangePasswordAtLogon $true -DisplayName $user -Enabled $true -SamAccountName $user -UserPrincipalName $user@$domainName
Set-ADAccountControl -Identity $user -DoesNotRequirePreAuth $true

$user = $configFile.Parameters.LocalRealm.User13.Username
$password = $configFile.Parameters.LocalRealm.User13.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName
$group = $configFile.Parameters.LocalRealm.User13.Group
try
{
	Remove-ADGroup -Identity $group -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $group, does not exist!" -ForegroundColor Red
}
New-ADGroup -Name $group -GroupScope Global -GroupCategory Security
$aduser = Get-ADUser -Identity $user
Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

#user14 is for DES downgrade protection
$user = $configFile.Parameters.LocalRealm.User14.Username
$password = $configFile.Parameters.LocalRealm.User14.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType DES -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName
Set-ADAccountControl $user -UseDESKeyOnly $true

$osVersion = Get-OSVersionNumber.ps1
$os2012R2 = "6.3"
if([double]$osVersion -ge [double]$os2012R2)
{
    #create users A2AF authentication poilcy
    $sddlstring = 'O:SYG:SYD:(XA;OICI;CR;;;WD;(@USER.ad://ext/Department=="HR"))'
    New-ADAuthenticationPolicy UserRestrictedPolicy -UserAllowedToAuthenticateFrom $sddlstring -Enforce -ComputerTGTLifetimeMins 60

    #get protected users group sid
    $AdObj = New-Object System.Security.Principal.NTAccount("Protected Users")
    $strSID = $AdObj.Translate([System.Security.Principal.SecurityIdentifier])
    $protectedUsersSid = $strSID.Value
    $sddlstring = 'O:SYG:SYD:(XA;OICI;CR;;;WD;((@USER.ad://ext/Department=="HR")||(Not_Member_of_any {SID('+$protectedUsersSid+')})))'

    #create computers A2A2 authentication policy
    New-ADAuthenticationPolicy ComputerRestrictedPolicy -ComputerAllowedToAuthenticateTo $sddlstring -Enforce -ComputerTGTLifetimeMins 60

    #create test silo  related users
    $user = $configFile.Parameters.LocalRealm.User15.Username
    $password = $configFile.Parameters.LocalRealm.User15.Password
    $pwd = ConvertTo-SecureString $password -AsPlainText -Force
    $group = $configFile.Parameters.LocalRealm.User15.Group
    $etype = $configFile.Parameters.LocalRealm.User15.SupportedEtype
    try
    {
	    Remove-ADUser -Identity $user -Confirm:$false
    }
    catch
    {
	    Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
    }
    Write-Info.ps1 "Creating new user account $user"
    New-ADUser -Name $user  -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType $etype -PasswordNeverExpires $true 
    $aduser = Get-ADUser -Identity $user
    Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

    $user = $configFile.Parameters.LocalRealm.User16.Username
    $password = $configFile.Parameters.LocalRealm.User16.Password
    $pwd = ConvertTo-SecureString $password -AsPlainText -Force
    $group = $configFile.Parameters.LocalRealm.User16.Group
    $etype = $configFile.Parameters.LocalRealm.User16.SupportedEtype
    $department = $configFile.Parameters.LocalRealm.User16.Department
    try
    {
	    Remove-ADUser -Identity $user -Confirm:$false
    }
    catch
    {
	    Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
    }
    Write-Info.ps1 "Creating new user account $user"
    New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Department $department -Enabled $true -KerberosEncryptionType $etype -PasswordNeverExpires $true 
    $aduser = Get-ADUser -Identity $user
    Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

    Set-ADUser -Identity $user -AuthenticationPolicy UserRestrictedPolicy

    $user = $configFile.Parameters.LocalRealm.User17.Username
    $password = $configFile.Parameters.LocalRealm.User17.Password
    $pwd = ConvertTo-SecureString $password -AsPlainText -Force
    $group = $configFile.Parameters.LocalRealm.User17.Group
    $etype = $configFile.Parameters.LocalRealm.User17.SupportedEtype

    try
    {
	    Remove-ADUser -Identity $user -Confirm:$false
    }
    catch
    {
	    Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
    }
    Write-Info.ps1 "Creating new user account $user"
    New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType $etype -PasswordNeverExpires $true 
    $aduser = Get-ADUser -Identity $user
    Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

    $user = $configFile.Parameters.LocalRealm.User18.Username
    $password = $configFile.Parameters.LocalRealm.User18.Password
    $pwd = ConvertTo-SecureString $password -AsPlainText -Force

    try
    {
	    Remove-ADUser -Identity $user -Confirm:$false
    }
    catch
    {
	    Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
    }
    Write-Info.ps1 "Creating new user account $user"
    New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true 

    $user = $configFile.Parameters.LocalRealm.User19.Username
    $password = $configFile.Parameters.LocalRealm.User19.Password
    $pwd = ConvertTo-SecureString $password -AsPlainText -Force
    $group = $configFile.Parameters.LocalRealm.User19.Group
    $department = $configFile.Parameters.LocalRealm.User19.Department

    try
    {
	    Remove-ADUser -Identity $user -Confirm:$false
    }
    catch
    {
	    Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
    }
    Write-Info.ps1 "Creating new user account $user"
    New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Department $department -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256  -PasswordNeverExpires $true 
    $aduser = Get-ADUser -Identity $user
    Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

}

$user = $configFile.Parameters.LocalRealm.User22.Username
$password = $configFile.Parameters.LocalRealm.User22.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADUser -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new user account $user"
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $false -PasswordNeverExpires $true -DisplayName $user -Enabled $true -SamAccountName $user -UserPrincipalName $user@$domainName

#-----------------------------------------------------------------------------------------------
# Add AD Computer Accounts on DC01 for Kerberos test suite
#-----------------------------------------------------------------------------------------------
$userNetBiosName = $configFile.Parameters.LocalRealm.AuthNotRequired.NetBiosName
$userFQDN = $configFile.Parameters.LocalRealm.AuthNotRequired.FQDN
$user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
$password = $configFile.Parameters.LocalRealm.AuthNotRequired.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADComputer -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new computer account $user"
New-ADComputer -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -ServicePrincipalNames @("cifs/$userFQDN", "http/$userFQDN")
Write-Info.ps1 "Configure $user to Pre-AuthenticationNotRequired"
$serverName = $configFile.Parameters.LocalRealm.KDC.FQDN
$objectDN = "DC=" + $domainName.Replace(".", ",DC=")
$userName = $configFile.Parameters.LocalRealm.Administrator.Username
$password = $configFile.Parameters.LocalRealm.Administrator.Password
$computerName = $user

# Get variables of the file
. Get-IADSearchRoot.ps1
$root = Get-IADSearchRoot -ServerName $serverName -objectDN $objectDN -Username $userName -Password $password
. Get-IADComputer.ps1
$computer = Get-IADComputer -Name $computerName -SearchRoot $root
. Set-IADComputer.ps1
$null = $computer|Set-IADComputer -PreAuthenticationNotRequired

$userNetBiosName = $configFile.Parameters.LocalRealm.LocalResource01.NetBiosName
$userFQDN = $configFile.Parameters.LocalRealm.LocalResource01.FQDN
$user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
$password = $configFile.Parameters.LocalRealm.LocalResource01.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADComputer -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new computer account $user"
New-ADComputer -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -ServicePrincipalNames @("host/$userFQDN")

$userNetBiosName = $configFile.Parameters.LocalRealm.LocalResource02.NetBiosName
$userFQDN = $configFile.Parameters.LocalRealm.LocalResource02.FQDN
$user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
$password = $configFile.Parameters.LocalRealm.LocalResource02.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
try
{
	Remove-ADComputer -Identity $user -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $user, does not exist!" -ForegroundColor Red
}
Write-Info.ps1 "Creating new computer account $user"
New-ADComputer -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -ServicePrincipalNames @("host/$userFQDN")
Set-ADComputer -Identity $user -Add @{'msDS-SupportedEncryptionTypes'=0x80000}

$user = $configFile.Parameters.LocalRealm.ClientComputer.NetBiosName
$password = $configFile.Parameters.LocalRealm.ClientComputer.Password
$PWD = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Set $user as trusted for delegation"
Set-ADAccountControl -Identity $user -TrustedForDelegation $true

if([double]$osVersion -ge [double]$os2012R2)
{    
    Set-ADComputer -Identity $user -Add @{'Department'="HR"}  -AuthenticationPolicy ComputerRestrictedPolicy

    $user = $configFile.Parameters.LocalRealm.FileShare.NetBiosName
    $password = $configFile.Parameters.LocalRealm.FileShare.Password
    $PWD = ConvertTo-SecureString $password -AsPlainText -Force
    Write-Info.ps1 "Set $user as trusted for delegation"
    Set-ADAccountControl -Identity $user -TrustedForDelegation $true
    Set-ADComputer -Identity $user -AuthenticationPolicy ComputerRestrictedPolicy
}
else
{
    Set-ADComputer -Identity $user -Add @{'Department'="HR"} 
}

#-----------------------------------------------------------------------------------------------
# Define Local Resource Groups
#-----------------------------------------------------------------------------------------------
$group = $configFile.Parameters.LocalRealm.ResourceGroup01.GroupName
try
{
	Remove-ADGroup -Identity $group -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $group, does not exist!" -ForegroundColor Red
}
New-ADGroup -Name $group -GroupScope DomainLocal -GroupCategory Security
$user = $configFile.Parameters.LocalRealm.LocalResource01.Name
$adcomputer = Get-ADComputer -Identity $user
Add-ADGroupMember -Identity $group -Members $adcomputer.DistinguishedName
$user = $configFile.Parameters.LocalRealm.LocalResource02.Name
$adcomputer = Get-ADComputer -Identity $user
Add-ADGroupMember -Identity $group -Members $adcomputer.DistinguishedName
$userGroup = $configFile.Parameters.LocalRealm.User13.Group
$adgroup = Get-ADGroup -Identity $userGroup
Add-ADGroupMember -Identity $group -Members $adgroup.DistinguishedName

$group = $configFile.Parameters.LocalRealm.ResourceGroup02.GroupName
try
{
	Remove-ADGroup -Identity $group -Confirm:$false
}
catch
{
	Write-Info.ps1 "Can't remove $group, does not exist!" -ForegroundColor Red
}
New-ADGroup -Name $group -GroupScope DomainLocal -GroupCategory Security
$user = $configFile.Parameters.LocalRealm.LocalResource01.Name
$adcomputer = Get-ADComputer -Identity $user
Add-ADGroupMember -Identity $group -Members $adcomputer.DistinguishedName
$user = $configFile.Parameters.LocalRealm.LocalResource02.Name
$adcomputer = Get-ADComputer -Identity $user
Add-ADGroupMember -Identity $group -Members $adcomputer.DistinguishedName
$userGroup = $configFile.Parameters.LocalRealm.User13.Group
$adgroup = Get-ADGroup -Identity $userGroup
Add-ADGroupMember -Identity $group -Members $adgroup.DistinguishedName

#-----------------------------------------------------------------------------------------------
# Define Claim Type
#-----------------------------------------------------------------------------------------------
$claim = $configFile.Parameters.LocalRealm.ClaimType.DisplayName
$value01 = $configFile.Parameters.LocalRealm.ClaimType.Value01
$value02 = $configFile.Parameters.LocalRealm.ClaimType.Value02
do{
    $sugVal1 = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value01, $value01,"");
    $sugVal2 = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value02, $value02,"");
    Write-Info.ps1 "Creating new Claim Type $claim"
    New-ADClaimType -AppliesToClasses @("User", "Computer") -DisplayName $claim -SourceAttribute $claim -SuggestedValues $sugVal1,$sugVal2 -ID "ad://ext/$claim"
    $tmp = Get-ADClaimType $claim
}while($tmp -eq $null)
Write-Info.ps1 "New Claim Type Created Successfully. DisplayName $claim" -ForegroundColor Green

#-----------------------------------------------------------------------------------------------
# Define Resource Property
#-----------------------------------------------------------------------------------------------
$resource = $configFile.Parameters.LocalRealm.ResourceProperties.DisplayName
$valuetype = $configFile.Parameters.LocalRealm.ResourceProperties.ValueType
$value01 = $configFile.Parameters.LocalRealm.ResourceProperties.Value01
$value02 = $configFile.Parameters.LocalRealm.ResourceProperties.Value02
$value03 = $configFile.Parameters.LocalRealm.ResourceProperties.Value03
$lbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value01, $value01,"");
$hbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value02, $value02,"");
$mbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value03, $value03,"");
Write-Info.ps1 "Creating new Resource Property $resource"
New-ADResourceProperty -DisplayName $resource -IsSecured $true -ResourcePropertyValueType $valuetype -ProtectedFromAccidentalDeletion $true -SuggestedValues $lbi,$hbi,$mbi
Add-ADResourcePropertyListMember "Global Resource Property List" -Members $resource

#-----------------------------------------------------------------------------------------------
# Create Central Access Rules
#-----------------------------------------------------------------------------------------------
$rules = $configFile.Parameters.LocalRealm.Rules.Name
$type = Get-ADClaimType $claim
$property = Get-ADResourceProperty $resource
$value01 = $configFile.Parameters.LocalRealm.Rules.Value01
$value02 = $configFile.Parameters.LocalRealm.Rules.Value02
$condition = "(@RESOURCE." + $property.Name + " == `"" + $value01 + "`")"
$acl = "O:SYG:SYD:AR(A;;FA;;;OW)(A;;FA;;;BA)(A;;FA;;;SY)(XA;;FA;;;AU;(@USER."+ $type.Name + " Any_of {`"" + $value02 + "`"}))"
Write-Info.ps1 "Creating new Access Rules $rules"
New-ADCentralAccessRule -Name $rules -ProtectedFromAccidentalDeletion $true -ResourceCondition $condition -CurrentAcl $acl

#-----------------------------------------------------------------------------------------------
# Create Central Access Policies
#-----------------------------------------------------------------------------------------------
$policy = $configFile.Parameters.LocalRealm.Policy.Name
Write-Info.ps1 "Creating new Access Policy $policy"
New-ADCentralAccessPolicy -Name $policy
Add-ADCentralAccessPolicyMember $policy -Members $rules

#-----------------------------------------------------------------------------------------------
# Enable FAST and Claims for this Realm
#-----------------------------------------------------------------------------------------------
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\Parameters /v CbacAndArmorLevel /t REG_DWORD /d 2 /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v EnableCbacAndArmor /t REG_DWORD /d 1 /f
REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\Parameters /v Supportedencryptiontypes /t REG_DWORD /d 0x7fffffff /f

#-----------------------------------------------------------------------------------------------
# Change User Account
#-----------------------------------------------------------------------------------------------
$user = $configFile.Parameters.LocalRealm.Administrator.Username
$password = $configFile.Parameters.LocalRealm.Administrator.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd

#-----------------------------------------------------------------------------------------------
# Change Computer Account Password
#-----------------------------------------------------------------------------------------------
$user = $configFile.Parameters.LocalRealm.ClientComputer.NetBiosName
$password = $configFile.Parameters.LocalRealm.ClientComputer.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd

$user = $configFile.Parameters.LocalRealm.WebServer.NetBiosName
$password = $configFile.Parameters.LocalRealm.WebServer.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd

$user = $configFile.Parameters.LocalRealm.FileShare.NetBiosName
$password = $configFile.Parameters.LocalRealm.FileShare.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd

#-----------------------------------------------------------------------------------------------
# The other domain supports Kerberos AES Encryption
#-----------------------------------------------------------------------------------------------
ksetup.exe /SetEncTypeAttr $configFile.Parameters.TrustRealm.RealmName AES256-CTS-HMAC-SHA1-96, AES128-CTS-HMAC-SHA1-96

#-----------------------------------------------------------------------------------------------
# Configure Group Policy for Claims
#-----------------------------------------------------------------------------------------------
Write-Host "Extract GPOBackup files"
C:\temp\Scripts\Extract-ZipFile.ps1 -ZipFile C:\temp\Scripts\Dc01GPO.zip -Destination C:\temp\Scripts\Dc01GPO

Write-Host "Configurating Group Policy"
Import-GPO -BackupId 3830DC6A-0AB3-42DF-ADF4-DDCCBC65D86F -TargetName "Default Domain Policy" -Path "C:\temp\Scripts\Dc01GPO" -CreateIfNeeded
    	
gpupdate /force 

#-----------------------------------------------------------------------------------------------
# Change the password policy for KKDCP
#-----------------------------------------------------------------------------------------------
$DomainPolicyId = (Get-GPO -Name "Default Domain Policy").id
$policyFilePath = "C:\Windows\SYSVOL\domain\Policies\{$DomainPolicyId}\MACHINE\Microsoft\Windows NT\SecEdit\GptTmpl.inf"
$lines = get-content $policyFilePath
foreach($line in $lines)
{
    if($line.Contains("MinimumPasswordAge"))
    {
        Write-Info.ps1 "Set MinimumPasswordAge to be 0" -ForegroundColor Yellow
        $newline = "MinimumPasswordAge = 0"
        $lines = $lines.Replace($line,$newline)
    }
    if($line.Contains("PasswordHistorySize"))
    {
        Write-Info.ps1 "Set PasswordHistorySize to be 0" -ForegroundColor Yellow
        $newline = "PasswordHistorySize = 0"
        $lines = $lines.Replace($line,$newline)
    }
}
Set-Content $policyFilePath $lines

cmd /c gpupdate /force 2>&1 | Write-Info.ps1

#-----------------------------------------------------------------------------------------------
# Finished to config DC01 computer
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Write signal file: config-dc01computer.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile


#-----------------------------------------------------------------------------------------------
# Ending script
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Config finished."
Write-Info.ps1 "EXECUTE [Config-DC01.ps1] FINISHED (NOT VERIFIED)."
Stop-Transcript