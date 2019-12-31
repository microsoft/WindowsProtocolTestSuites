#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Configure-DC01.ps1
## Purpose:        Configure the Local Realm KDC computer for Kerberos Server test suite.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016, and later.
##
###########################################################################################

#------------------------------------------------------------------------------------------
# Parameters:
# Help: whether to display the help information
# Step: Current step for configuration
#------------------------------------------------------------------------------------------
Param
(
    [alias("h")]
    [switch]
    $Help,

    [string]$WorkingPath = "C:\temp"
)

$newEnvPath=$env:Path+";.\;.\scripts\"
$env:Path=$newEnvPath

#------------------------------------------------------------------------------------------
# Global Variables:
# ScriptFileFullPath: Full Path of this script file
# ScriptName:         File Name of this script file 
# SignalFileFullPath: Full Path of the completion signal file for this script file
# LogFileFullPath:    Full Path of the log file for this script file
# Parameters:         Parameters read from the config file
# DataFile:           Full Path of the kerberos config XML file
# KrbParams:          Parameters read from the DataFile
#------------------------------------------------------------------------------------------
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$SignalFileFullPath      = "$WorkingPath\Configure-DC01.finished.signal"
$LogFileFullPath         = "$ScriptFileFullPath.log"
$DataFile                = "$WorkingPath\Scripts\ParamConfig.xml"
[xml]$KrbParams          = $null
$Domain                  = ""

$ScriptsSignalFile = "$env:HOMEDRIVE\config-DC01.finished.signal"
if (Test-Path -Path $ScriptsSignalFile)
{
    Write-Host "The script execution is complete." -foregroundcolor Red
    exit 0
}

#------------------------------------------------------------------------------------------
# Function: Display-Help
# Display the help messages.
#------------------------------------------------------------------------------------------
Function Display-Help()
{
    $helpmsg = @"
Post configuration script to configure the Local Realm KDC computer for Kerberos Server test suite.

Usage:
    .\Configure-DC01.ps1 [-WorkingPath <WorkingPath>] [-h | -help]

Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg "`r`n"
    exit 0
}

#------------------------------------------------------------------------------------------
# Function: Start-ConfigLog
# Create log file and start logging
#------------------------------------------------------------------------------------------
Function Start-ConfigLog()
{
    if (!(Test-Path -Path $LogFileFullPath))
    {
        New-Item -ItemType File -path $LogFileFullPath -Force
    }
    Start-Transcript $LogFileFullPath -Append 2>&1 | Out-Null
}

#------------------------------------------------------------------------------------------
# Function: Write-ConfigLog
# Write information to log file
#------------------------------------------------------------------------------------------
Function Write-ConfigLog
{
    Param (
        [Parameter(ValueFromPipeline=$true)] $text,
        $ForegroundColor = "Green"
    )

    $date = Get-Date -f MM-dd-yyyy_HH_mm_ss
    Write-Output "[$date] $text"
}

#------------------------------------------------------------------------------------------
# Function: Read-ConfigParameters
# Read Config Parameters
#------------------------------------------------------------------------------------------
Function Read-ConfigParameters()
{
    Write-ConfigLog "Getting the parameters from Kerberos config file..." -ForegroundColor Yellow
    if(Test-Path -Path $DataFile)
    {
        [xml]$Script:KrbParams = Get-Content -Path $DataFile
        $Script:Domain = $KrbParams.Parameters.LocalRealm.RealmName
    }
    else
    {
        Write-ConfigLog "$DataFile not found. Will keep the default setting of all the test context info..."
    }
}

#------------------------------------------------------------------------------------------
# Function: Init-Environment
# Start logging, check signal file, switch to script path and read the config parameters
#------------------------------------------------------------------------------------------
Function Init-Environment()
{
    # Start logging
    Start-ConfigLog

    # Start executing the script
    Write-ConfigLog "Executing [$ScriptName]..." -ForegroundColor Cyan

    # Switch to the script path
    #Write-ConfigLog "Switching to $WorkingPath..." -ForegroundColor Yellow
    #Push-Location $WorkingPath

    # Update ParamConfig.xml
    UpdateConfigFile.ps1 -WorkingPath $WorkingPath

    # Read the config parameters
    Read-ConfigParameters
}


#------------------------------------------------------------------------------------------
# Function: Complete-Configure
# Write signal file, stop the transcript logging and remove the scedule task
#------------------------------------------------------------------------------------------
Function Complete-Configure
{
    # Write signal file
    Write-ConfigLog "Write signal file`: post.finished.signal to hard drive."
    cmd /C ECHO CONFIG FINISHED > $SignalFileFullPath

    # Ending script
    Write-ConfigLog "Config finished."
    Write-ConfigLog "EXECUTE [$ScriptName] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    # remove the schedule task to execute the script next step after restart
    RestartAndRunFinish.ps1
}


#------------------------------------------------------------------------------------------
# Function: Config-DC01
# Configure the environment DC01:
# Triggered by remote trusted domain: <AP01>
#  * Change AP01 computer password
#------------------------------------------------------------------------------------------
Function Config-DC01()
{
	Write-ConfigLog "Begin to config DC01 computer"
	$domainName 	= $KrbParams.Parameters.LocalRealm.RealmName
	#-----------------------------------------------------------------------------------------------
	# Create forest trust on local side
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Create forest trust relationship on local side ..." -ForegroundColor Yellow

	$LocalForest = [System.DirectoryServices.ActiveDirectory.Forest]::GetCurrentForest()

	try
	{
		# Build trust relationship on local forest only
		$LocalForest.CreateLocalSideOfTrustRelationship($KrbParams.Parameters.TrustRealm.RealmName, "Bidirectional", $KrbParams.Parameters.TrustPassword)
	}
	# If trust relationship already exists
	catch [System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectExistsException]
	{
		Write-ConfigLog "Trust relationship already exists."
	}
	catch
	{
		throw "Failed to create trust relationship. Error: " + $_.Exception.Message
	}

	#-----------------------------------------------------------------------------------------------
	# Enable the Ticket-Granting Ticket (TGT) delegation on DC01
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Enable the Ticket-Granting Ticket (TGT) delegation on DC01" -ForegroundColor Yellow

	try {
		$trustRealmeName = $KrbParams.Parameters.TrustRealm.RealmName
		$trustRealmeAdmin = $KrbParams.Parameters.TrustRealm.Administrator.Username
		$trustRealmeAdminPassword = $KrbParams.Parameters.TrustRealm.Administrator.Password
		cmd /c "netdom trust $domainName /domain:$trustRealmeName /ud:$trustRealmeAdmin /pd:$trustRealmeAdminPassword /EnableTgtDelegation:yes"
	}
	catch {
		throw "Failed to enable TGT delegation on DC01. Error: " + $_.Exception.Message 
	}

	#-----------------------------------------------------------------------------------------------
	# Add AD Users Accounts on DC01 for Kerberos test suite
	#-----------------------------------------------------------------------------------------------
	$user = $KrbParams.Parameters.LocalRealm.User01.Username
	$password = $KrbParams.Parameters.LocalRealm.User01.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName -HomeDirectory "c:\home\" -HomeDrive "c:" -ProfilePath "c:\profiles\" -ScriptPath "c:\scripts\"
	$group = $KrbParams.Parameters.LocalRealm.User01.Group
	try
	{
		Remove-ADGroup -Identity $group -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $group, does not exist!" -ForegroundColor Red
	}
	New-ADGroup -Name $group -GroupScope Global -GroupCategory Security
	$aduser = Get-ADUser -Identity $user
	Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

	$user = $KrbParams.Parameters.LocalRealm.User02.Username
	$password = $KrbParams.Parameters.LocalRealm.User02.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -CompoundIdentitySupported $true -Department "HR" -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

	$user = $KrbParams.Parameters.LocalRealm.User03.Username
	$password = $KrbParams.Parameters.LocalRealm.User03.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountNotDelegated $true -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

	$user = $KrbParams.Parameters.LocalRealm.User04.Username
	$password = $KrbParams.Parameters.LocalRealm.User04.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -ServicePrincipalNames "abc/$user" -TrustedForDelegation $true -UserPrincipalName $user@$domainName

	$user = $KrbParams.Parameters.LocalRealm.User05.Username
	$password = $KrbParams.Parameters.LocalRealm.User05.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user

	$user = $KrbParams.Parameters.LocalRealm.User06.Username
	$password = $KrbParams.Parameters.LocalRealm.User06.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName
	Set-ADAccountControl -Identity $user -DoesNotRequirePreAuth $true

	$user = $KrbParams.Parameters.LocalRealm.User07.Username
	$password = $KrbParams.Parameters.LocalRealm.User07.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $false -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

	$user = $KrbParams.Parameters.LocalRealm.User08.Username
	$password = $KrbParams.Parameters.LocalRealm.User08.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -AccountExpirationDate 1/1/2011 -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

	$user = $KrbParams.Parameters.LocalRealm.User09.Username
	$password = $KrbParams.Parameters.LocalRealm.User09.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Get-ADDefaultDomainPasswordPolicy | Set-ADDefaultDomainPasswordPolicy -ComplexityEnabled $false -MinPasswordLength 6 -MaxPasswordAge 30.0 -LockoutThreshold 1 -LockoutObservationWindow 1.0 -LockoutDuration 365.0
	gpupdate /force
	Write-ConfigLog "Creating new user account $user"
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
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
		Write-ConfigLog "Successfully Locked the $user account" -ForegroundColor green
	}
	Get-ADDefaultDomainPasswordPolicy | Set-ADDefaultDomainPasswordPolicy -ComplexityEnabled $true -MinPasswordLength 7 -MaxPasswordAge 42.0 -LockoutThreshold 0
	gpupdate /force

	$user = $KrbParams.Parameters.LocalRealm.User10.Username
	$password = $KrbParams.Parameters.LocalRealm.User10.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName
	$hours = New-object byte[] 21
	$replaceHashTable = New-Object HashTable
	$replaceHashTable.Add("logonHours", $hours)
	$replaceHashTable.Add("description", "This user is set to be always out of logon hours")
	Set-ADUser -Identity $user -Replace $replaceHashTable

	$user = $KrbParams.Parameters.LocalRealm.User11.Username
	$password = $KrbParams.Parameters.LocalRealm.User11.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -ChangePasswordAtLogon $true -DisplayName $user -Enabled $true -SamAccountName $user -UserPrincipalName $user@$domainName
	$aduser = Get-ADUser -Identity $user
	Set-ADAccountControl -Identity $user -DoesNotRequirePreAuth $true

	$user = $KrbParams.Parameters.LocalRealm.User12.Username
	$password = $KrbParams.Parameters.LocalRealm.User12.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $false -ChangePasswordAtLogon $true -DisplayName $user -Enabled $true -SamAccountName $user -UserPrincipalName $user@$domainName
	Set-ADAccountControl -Identity $user -DoesNotRequirePreAuth $true

	$user = $KrbParams.Parameters.LocalRealm.User13.Username
	$password = $KrbParams.Parameters.LocalRealm.User13.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName
	$group = $KrbParams.Parameters.LocalRealm.User13.Group
	try
	{
		Remove-ADGroup -Identity $group -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $group, does not exist!" -ForegroundColor Red
	}
	New-ADGroup -Name $group -GroupScope Global -GroupCategory Security
	$aduser = Get-ADUser -Identity $user
	Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

	#user14 is for DES downgrade protection
	$user = $KrbParams.Parameters.LocalRealm.User14.Username
	$password = $KrbParams.Parameters.LocalRealm.User14.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
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
		$user = $KrbParams.Parameters.LocalRealm.User15.Username
		$password = $KrbParams.Parameters.LocalRealm.User15.Password
		$pwd = ConvertTo-SecureString $password -AsPlainText -Force
		$group = $KrbParams.Parameters.LocalRealm.User15.Group
		$etype = $KrbParams.Parameters.LocalRealm.User15.SupportedEtype
		try
		{
			Remove-ADUser -Identity $user -Confirm:$false
		}
		catch
		{
			Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
		}
		Write-ConfigLog "Creating new user account $user"
		New-ADUser -Name $user  -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType $etype -PasswordNeverExpires $true 
		$aduser = Get-ADUser -Identity $user
		Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

		$user = $KrbParams.Parameters.LocalRealm.User16.Username
		$password = $KrbParams.Parameters.LocalRealm.User16.Password
		$pwd = ConvertTo-SecureString $password -AsPlainText -Force
		$group = $KrbParams.Parameters.LocalRealm.User16.Group
		$etype = $KrbParams.Parameters.LocalRealm.User16.SupportedEtype
		$department = $KrbParams.Parameters.LocalRealm.User16.Department
		try
		{
			Remove-ADUser -Identity $user -Confirm:$false
		}
		catch
		{
			Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
		}
		Write-ConfigLog "Creating new user account $user"
		New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Department $department -Enabled $true -KerberosEncryptionType $etype -PasswordNeverExpires $true 
		$aduser = Get-ADUser -Identity $user
		Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

		Set-ADUser -Identity $user -AuthenticationPolicy UserRestrictedPolicy

		$user = $KrbParams.Parameters.LocalRealm.User17.Username
		$password = $KrbParams.Parameters.LocalRealm.User17.Password
		$pwd = ConvertTo-SecureString $password -AsPlainText -Force
		$group = $KrbParams.Parameters.LocalRealm.User17.Group
		$etype = $KrbParams.Parameters.LocalRealm.User17.SupportedEtype

		try
		{
			Remove-ADUser -Identity $user -Confirm:$false
		}
		catch
		{
			Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
		}
		Write-ConfigLog "Creating new user account $user"
		New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType $etype -PasswordNeverExpires $true 
		$aduser = Get-ADUser -Identity $user
		Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

		$user = $KrbParams.Parameters.LocalRealm.User18.Username
		$password = $KrbParams.Parameters.LocalRealm.User18.Password
		$pwd = ConvertTo-SecureString $password -AsPlainText -Force

		try
		{
			Remove-ADUser -Identity $user -Confirm:$false
		}
		catch
		{
			Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
		}
		Write-ConfigLog "Creating new user account $user"
		New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true 

		$user = $KrbParams.Parameters.LocalRealm.User19.Username
		$password = $KrbParams.Parameters.LocalRealm.User19.Password
		$pwd = ConvertTo-SecureString $password -AsPlainText -Force
		$group = $KrbParams.Parameters.LocalRealm.User19.Group
		$department = $KrbParams.Parameters.LocalRealm.User19.Department

		try
		{
			Remove-ADUser -Identity $user -Confirm:$false
		}
		catch
		{
			Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
		}
		Write-ConfigLog "Creating new user account $user"
		New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Department $department -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256  -PasswordNeverExpires $true 
		$aduser = Get-ADUser -Identity $user
		Add-ADGroupMember -Identity $group -Members $aduser.DistinguishedName

	}

	$user = $KrbParams.Parameters.LocalRealm.User22.Username
	$password = $KrbParams.Parameters.LocalRealm.User22.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADUser -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new user account $user"
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $false -PasswordNeverExpires $true -DisplayName $user -Enabled $true -SamAccountName $user -UserPrincipalName $user@$domainName

	#-----------------------------------------------------------------------------------------------
	# Add AD Computer Accounts on DC01 for Kerberos test suite
	#-----------------------------------------------------------------------------------------------
	$userNetBiosName = $KrbParams.Parameters.LocalRealm.AuthNotRequired.NetBiosName
	$userFQDN = $KrbParams.Parameters.LocalRealm.AuthNotRequired.FQDN
	$user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
	$password = $KrbParams.Parameters.LocalRealm.AuthNotRequired.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADComputer -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new computer account $user"
	New-ADComputer -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -ServicePrincipalNames @("cifs/$userFQDN", "http/$userFQDN")
	Write-ConfigLog "Configure $user to Pre-AuthenticationNotRequired"
	$serverName = $KrbParams.Parameters.LocalRealm.KDC.FQDN
	$objectDN = "DC=" + $domainName.Replace(".", ",DC=")
	$userName = $KrbParams.Parameters.LocalRealm.Administrator.Username
	$password = $KrbParams.Parameters.LocalRealm.Administrator.Password
	$computerName = $user

	# Get variables of the file
	#.\Get-IADSearchRoot.ps1
	$root = Get-IADSearchRoot.ps1 -ServerName $serverName -objectDN $objectDN -Username $userName -Password $password
	#. Get-IADComputer.ps1
	$computer = Get-IADComputer -Name $computerName -SearchRoot $root
	#. Set-IADComputer.ps1
	$null = $computer|Set-IADComputer -PreAuthenticationNotRequired

	$userNetBiosName = $KrbParams.Parameters.LocalRealm.LocalResource01.NetBiosName
	$userFQDN = $KrbParams.Parameters.LocalRealm.LocalResource01.FQDN
	$user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
	$password = $KrbParams.Parameters.LocalRealm.LocalResource01.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADComputer -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new computer account $user"
	New-ADComputer -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -ServicePrincipalNames @("host/$userFQDN")

	$userNetBiosName = $KrbParams.Parameters.LocalRealm.LocalResource02.NetBiosName
	$userFQDN = $KrbParams.Parameters.LocalRealm.LocalResource02.FQDN
	$user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
	$password = $KrbParams.Parameters.LocalRealm.LocalResource02.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	try
	{
		Remove-ADComputer -Identity $user -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $user, does not exist!" -ForegroundColor Red
	}
	Write-ConfigLog "Creating new computer account $user"
	New-ADComputer -Name $user -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true -ServicePrincipalNames @("host/$userFQDN")
	Set-ADComputer -Identity $user -Add @{'msDS-SupportedEncryptionTypes'=0x80000}

	$user = $KrbParams.Parameters.LocalRealm.ClientComputer.NetBiosName
	$password = $KrbParams.Parameters.LocalRealm.ClientComputer.Password
	$PWD = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Set $user as trusted for delegation"
	Set-ADAccountControl -Identity $user -TrustedForDelegation $true

	if([double]$osVersion -ge [double]$os2012R2)
	{    
		Set-ADComputer -Identity $user -Add @{'Department'="HR"}  -AuthenticationPolicy ComputerRestrictedPolicy

		$user = $KrbParams.Parameters.LocalRealm.FileShare.NetBiosName
		$password = $KrbParams.Parameters.LocalRealm.FileShare.Password
		$PWD = ConvertTo-SecureString $password -AsPlainText -Force
		Write-ConfigLog "Set $user as trusted for delegation"
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
	$group = $KrbParams.Parameters.LocalRealm.ResourceGroup01.GroupName
	try
	{
		Remove-ADGroup -Identity $group -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $group, does not exist!" -ForegroundColor Red
	}
	New-ADGroup -Name $group -GroupScope DomainLocal -GroupCategory Security
	$user = $KrbParams.Parameters.LocalRealm.LocalResource01.Name
	$adcomputer = Get-ADComputer -Identity $user
	Add-ADGroupMember -Identity $group -Members $adcomputer.DistinguishedName
	$user = $KrbParams.Parameters.LocalRealm.LocalResource02.Name
	$adcomputer = Get-ADComputer -Identity $user
	Add-ADGroupMember -Identity $group -Members $adcomputer.DistinguishedName
	$userGroup = $KrbParams.Parameters.LocalRealm.User13.Group
	$adgroup = Get-ADGroup -Identity $userGroup
	Add-ADGroupMember -Identity $group -Members $adgroup.DistinguishedName

	$group = $KrbParams.Parameters.LocalRealm.ResourceGroup02.GroupName
	try
	{
		Remove-ADGroup -Identity $group -Confirm:$false
	}
	catch
	{
		Write-ConfigLog "Can't remove $group, does not exist!" -ForegroundColor Red
	}
	New-ADGroup -Name $group -GroupScope DomainLocal -GroupCategory Security
	$user = $KrbParams.Parameters.LocalRealm.LocalResource01.Name
	$adcomputer = Get-ADComputer -Identity $user
	Add-ADGroupMember -Identity $group -Members $adcomputer.DistinguishedName
	$user = $KrbParams.Parameters.LocalRealm.LocalResource02.Name
	$adcomputer = Get-ADComputer -Identity $user
	Add-ADGroupMember -Identity $group -Members $adcomputer.DistinguishedName
	$userGroup = $KrbParams.Parameters.LocalRealm.User13.Group
	$adgroup = Get-ADGroup -Identity $userGroup
	Add-ADGroupMember -Identity $group -Members $adgroup.DistinguishedName

	#-----------------------------------------------------------------------------------------------
	# Define Claim Type
	#-----------------------------------------------------------------------------------------------
	$claim = $KrbParams.Parameters.LocalRealm.ClaimType.DisplayName
	$value01 = $KrbParams.Parameters.LocalRealm.ClaimType.Value01
	$value02 = $KrbParams.Parameters.LocalRealm.ClaimType.Value02
	do{
		$sugVal1 = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value01, $value01,"");
		$sugVal2 = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value02, $value02,"");
		Write-ConfigLog "Creating new Claim Type $claim"
		New-ADClaimType -AppliesToClasses @("User", "Computer") -DisplayName $claim -SourceAttribute $claim -SuggestedValues $sugVal1,$sugVal2 -ID "ad://ext/$claim"
		$tmp = Get-ADClaimType $claim
	}while($tmp -eq $null)
	Write-ConfigLog "New Claim Type Created Successfully. DisplayName $claim" -ForegroundColor Green

	#-----------------------------------------------------------------------------------------------
	# Define Resource Property
	#-----------------------------------------------------------------------------------------------
	$resource = $KrbParams.Parameters.LocalRealm.ResourceProperties.DisplayName
	$valuetype = $KrbParams.Parameters.LocalRealm.ResourceProperties.ValueType
	$value01 = $KrbParams.Parameters.LocalRealm.ResourceProperties.Value01
	$value02 = $KrbParams.Parameters.LocalRealm.ResourceProperties.Value02
	$value03 = $KrbParams.Parameters.LocalRealm.ResourceProperties.Value03
	$lbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value01, $value01,"");
	$hbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value02, $value02,"");
	$mbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value03, $value03,"");
	Write-ConfigLog "Creating new Resource Property $resource"
	New-ADResourceProperty -DisplayName $resource -IsSecured $true -ResourcePropertyValueType $valuetype -ProtectedFromAccidentalDeletion $true -SuggestedValues $lbi,$hbi,$mbi
	Add-ADResourcePropertyListMember "Global Resource Property List" -Members $resource

	#-----------------------------------------------------------------------------------------------
	# Create Central Access Rules
	#-----------------------------------------------------------------------------------------------
	$rules = $KrbParams.Parameters.LocalRealm.Rules.Name
	$type = Get-ADClaimType $claim
	$property = Get-ADResourceProperty $resource
	$value01 = $KrbParams.Parameters.LocalRealm.Rules.Value01
	$value02 = $KrbParams.Parameters.LocalRealm.Rules.Value02
	$condition = "(@RESOURCE." + $property.Name + " == `"" + $value01 + "`")"
	$acl = "O:SYG:SYD:AR(A;;FA;;;OW)(A;;FA;;;BA)(A;;FA;;;SY)(XA;;FA;;;AU;(@USER."+ $type.Name + " Any_of {`"" + $value02 + "`"}))"
	Write-ConfigLog "Creating new Access Rules $rules"
	New-ADCentralAccessRule -Name $rules -ProtectedFromAccidentalDeletion $true -ResourceCondition $condition -CurrentAcl $acl

	#-----------------------------------------------------------------------------------------------
	# Create Central Access Policies
	#-----------------------------------------------------------------------------------------------
	$policy = $KrbParams.Parameters.LocalRealm.Policy.Name
	Write-ConfigLog "Creating new Access Policy $policy"
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
	$user = $KrbParams.Parameters.LocalRealm.Administrator.Username
	$password = $KrbParams.Parameters.LocalRealm.Administrator.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd

	#-----------------------------------------------------------------------------------------------
	# Change Computer Account Password
	#-----------------------------------------------------------------------------------------------
	$user = $KrbParams.Parameters.LocalRealm.ClientComputer.NetBiosName
	$password = $KrbParams.Parameters.LocalRealm.ClientComputer.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd

	$user = $KrbParams.Parameters.LocalRealm.WebServer.NetBiosName
	$password = $KrbParams.Parameters.LocalRealm.WebServer.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd

	$user = $KrbParams.Parameters.LocalRealm.FileShare.NetBiosName
	$password = $KrbParams.Parameters.LocalRealm.FileShare.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd

	#-----------------------------------------------------------------------------------------------
	# The other domain supports Kerberos AES Encryption
	#-----------------------------------------------------------------------------------------------
	ksetup.exe /SetEncTypeAttr $KrbParams.Parameters.TrustRealm.RealmName AES256-CTS-HMAC-SHA1-96, AES128-CTS-HMAC-SHA1-96

	#-----------------------------------------------------------------------------------------------
	# Configure Group Policy for Claims
	#-----------------------------------------------------------------------------------------------
	Write-Host "Extract GPOBackup files"
	#C:\temp\Scripts\Extract-ZipFile.ps1 -ZipFile C:\temp\Scripts\Dc01GPO.zip -Destination C:\temp\Scripts\Dc01GPO
    &"$WorkingPath\Scripts\Extract-ZipFile.ps1" -ZipFile "$WorkingPath\Scripts\Dc01GPO.zip" -Destination "$WorkingPath\Scripts\Dc01GPO"

	Write-Host "Update Group Policy"
	$currDomainName = (Get-WmiObject win32_computersystem).Domain
	$currDomain = Get-ADDomain $currDomainName
	if($currDomain.name -ne "contoso") {
		Get-ChildItem -Path "$WorkingPath\Scripts\Dc01GPO" -exclude *.pol -File -Recurse | ForEach-Object {
			$content =($_|Get-Content)
			if ($content | Select-String -Pattern 'contoso') {
				$content = $content -replace 'contoso',$currDomain.name   
				[IO.File]::WriteAllText($_.FullName, ($content -join "`r`n"))
			}
		}
	}
	Write-Host "Configurating Group Policy"
	Import-GPO -BackupId 3830DC6A-0AB3-42DF-ADF4-DDCCBC65D86F -TargetName "Default Domain Policy" -Path "$WorkingPath\Scripts\Dc01GPO" -CreateIfNeeded
    	
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
			Write-ConfigLog "Set MinimumPasswordAge to be 0" -ForegroundColor Yellow
			$newline = "MinimumPasswordAge = 0"
			$lines = $lines.Replace($line,$newline)
		}
		if($line.Contains("PasswordHistorySize"))
		{
			Write-ConfigLog "Set PasswordHistorySize to be 0" -ForegroundColor Yellow
			$newline = "PasswordHistorySize = 0"
			$lines = $lines.Replace($line,$newline)
		}
	}
	Set-Content $policyFilePath $lines

	cmd /c gpupdate /force 2>&1 | Write-ConfigLog
}



#------------------------------------------------------------------------------------------
# Main Function
#------------------------------------------------------------------------------------------
Function Main
{
    # Display help information
    if($Help)
    {
        Display-Help
        return
    }

    # Initialize configure environment
    Init-Environment

    Config-DC01
	
	Complete-Configure
}

Main