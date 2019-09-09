#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Configure-DC02.ps1
## Purpose:        Configure the Trust Realm KDC computer for Kerberos Server test suite.
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
$SignalFileFullPath      = "$WorkingPath\Configure-DC02.finished.signal"
$LogFileFullPath         = "$ScriptFileFullPath.log"
$DataFile                = "$WorkingPath\Scripts\ParamConfig.xml"
[xml]$KrbParams          = $null

#------------------------------------------------------------------------------------------
# Function: Display-Help
# Display the help messages.
#------------------------------------------------------------------------------------------
Function Display-Help()
{
    $helpmsg = @"
Post configuration script to configure the Trust Realm KDC computer for Kerberos Server test suite.

Usage:
    .\Configure-DC02.ps1 [-WorkingPath <WorkingPath>] [-h | -help]

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
        $Script:Domain = $KrbParams.Parameters.TrustRealm.RealmName
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
    Write-ConfigLog "Switching to $WorkingPath..." -ForegroundColor Yellow
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
# Function: Config-DC02
# Configure the environment DC02:
# Triggered by remote trusted domain: <AP02>
#  * Change AP02 computer password
Function Config-DC02()
{
    Write-ConfigLog "Begin to config DC02 computer"
	$domainName 	= $KrbParams.Parameters.TrustRealm.RealmName

	#-----------------------------------------------------------------------------------------------
	#Create forest trust on local side
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Create forest trust relationship on local side ..." -ForegroundColor Yellow

	$LocalForest = [System.DirectoryServices.ActiveDirectory.Forest]::GetCurrentForest()
	try
	{
		# Build trust relationship on local forest only
		$LocalForest.CreateLocalSideOfTrustRelationship($KrbParams.Parameters.LocalRealm.RealmName, "Bidirectional", $KrbParams.Parameters.TrustPassword)
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
	# Add AD Users Accounts on DC02 for Kerberos test suite
	#-----------------------------------------------------------------------------------------------
	$user = $KrbParams.Parameters.TrustRealm.User01.Username
	$password = $KrbParams.Parameters.TrustRealm.User01.Password
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
	$group = $KrbParams.Parameters.TrustRealm.User01.Group
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

	$user = $KrbParams.Parameters.TrustRealm.User02.Username
	$password = $KrbParams.Parameters.TrustRealm.User02.Password
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
	New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -CompoundIdentitySupported $true -Description "MS-KILE" -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

	#-----------------------------------------------------------------------------------------------
	# Define Claim Type
	#-----------------------------------------------------------------------------------------------
	$claim = $KrbParams.Parameters.TrustRealm.ClaimType.DisplayName
	$value01 = $KrbParams.Parameters.TrustRealm.ClaimType.Value01
	$value02 = $KrbParams.Parameters.TrustRealm.ClaimType.Value02
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
	$resource = $KrbParams.Parameters.TrustRealm.ResourceProperties.DisplayName
	$valuetype = $KrbParams.Parameters.TrustRealm.ResourceProperties.ValueType
	$value01 = $KrbParams.Parameters.TrustRealm.ResourceProperties.Value01
	$value02 = $KrbParams.Parameters.TrustRealm.ResourceProperties.Value02
	$value03 = $KrbParams.Parameters.TrustRealm.ResourceProperties.Value03
	$lbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value01, $value01,"");
	$hbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value02, $value02,"");
	$mbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value03, $value03,"");
	Write-ConfigLog "Creating new Resource Property $resource"
	New-ADResourceProperty -DisplayName $resource -IsSecured $true -ResourcePropertyValueType $valuetype -ProtectedFromAccidentalDeletion $true -SuggestedValues $lbi,$hbi,$mbi
	Add-ADResourcePropertyListMember "Global Resource Property List" -Members $resource

	#-----------------------------------------------------------------------------------------------
	# Define Claims transformation policy
	#-----------------------------------------------------------------------------------------------
	New-ADClaimTransformPolicy -Description:"Claims transformation policy to deny all claims except $claim" -Name:"DenyAllClaimsExceptTestedClaimPolicy" -DenyAllExcept:$claim -Server:"$domainName"
	$trustedDomain = $KrbParams.Parameters.LocalRealm.RealmName
	Set-ADClaimTransformLink -Identity:"$trustedDomain" -Policy:"DenyAllClaimsExceptTestedClaimPolicy" -TrustRole:Trusting

	#-----------------------------------------------------------------------------------------------
	# Create Central Access Rules
	#-----------------------------------------------------------------------------------------------
	$rules = $KrbParams.Parameters.TrustRealm.Rules.Name
	$type = Get-ADClaimType $claim
	$property = Get-ADResourceProperty $resource
	$value01 = $KrbParams.Parameters.TrustRealm.Rules.Value01
	$value02 = $KrbParams.Parameters.TrustRealm.Rules.Value02
	$condition = "(@RESOURCE." + $property.Name + " == `"" + $value01 + "`")"
	$acl = "O:SYG:SYD:AR(A;;FA;;;OW)(A;;FA;;;BA)(A;;FA;;;SY)(XA;;FA;;;AU;(@USER."+ $type.Name + " Any_of {`"" + $value02 + "`"}))"
	Write-ConfigLog "Creating new Access Rules $rules"
	New-ADCentralAccessRule -Name $rules -ProtectedFromAccidentalDeletion $true -ResourceCondition $condition -CurrentAcl $acl

	#-----------------------------------------------------------------------------------------------
	# Create Central Access Policies
	#-----------------------------------------------------------------------------------------------
	$policy = $KrbParams.Parameters.TrustRealm.Policy.Name
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
	$user = $KrbParams.Parameters.TrustRealm.Administrator.Username
	$password = $KrbParams.Parameters.TrustRealm.Administrator.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd

	#-----------------------------------------------------------------------------------------------
	# Grant Allowed to Authenticate Permission on Computer Accounts
	#-----------------------------------------------------------------------------------------------
	$admin = $KrbParams.Parameters.LocalRealm.Administrator.Username
	$password = $KrbParams.Parameters.LocalRealm.Administrator.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	$cred = New-Object System.Management.Automation.PSCredential $admin,$pwd -ErrorAction stop
	$user = $KrbParams.Parameters.LocalRealm.User01.Username
	$dc = $KrbParams.Parameters.LocalRealm.KDC.FQDN
	Write-ConfigLog "Get $user in another domain on server: $dc."

	# If Get-ADUser failed, retry 15 minutes until it succeeds.
	$aduser = $null
	$RetryTimes = 90
	while($RetryTimes -ge 0)
	{
		$aduser = get-aduser -identity $user -server $dc -credential $cred
		if ($aduser -ne $null)
		{
			break;
		}
		else
		{
			Write-ConfigLog "Get-AdUser failed, retry it later"
			$RetryTimes--
			sleep 10
		}
	}

	if ($RetryTimes -le 0)
	{
		Write-ConfigLog "Cannot Get-AdUser $user in 15 minutes, quit current script"
		exit 1
	}

	$user = $KrbParams.Parameters.TrustRealm.WebServer.NetBiosName
	Write-ConfigLog "Set ACL on computer $user."
	$adcomputer = Get-ADComputer -Identity $user

	pushd AD:
	$acl = get-acl $adcomputer.distinguishedName
	$accesscontroltypeallow = [System.Security.AccessControl.AccessControlType]::Allow
	#Allowed to Authenticate GUID is 68b1d179-0d15-4d4f-ab71-46152e79a7bc
	$allowed2authguid = New-Object GUID 68b1d179-0d15-4d4f-ab71-46152e79a7bc
	$ace = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $aduser.sid,"ExtendedRight",$accesscontroltypeallow,$allowed2authguid
	$acl.AddAccessRule($ace)
	Set-Acl -AclObject $acl $adcomputer.distinguishedName
	popd
	Write-ConfigLog "Set ACL on computer $adcomputer succeeded." -ForegroundColor Green

	$user = $KrbParams.Parameters.TrustRealm.FileShare.NetBiosName
	Write-ConfigLog "Set ACL on computer $user."
	$adcomputer = Get-ADComputer -Identity $user

	pushd AD:
	$acl = get-acl $adcomputer.distinguishedName
	$accesscontroltypeallow = [System.Security.AccessControl.AccessControlType]::Allow
	$allowed2authguid = New-Object GUID 68b1d179-0d15-4d4f-ab71-46152e79a7bc
	$ace = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $aduser.sid,"ExtendedRight",$accesscontroltypeallow,$allowed2authguid
	$acl.AddAccessRule($ace)
	Set-Acl -AclObject $acl $adcomputer.distinguishedName
	popd
	Write-ConfigLog "Set ACL on computer $adcomputer succeeded." -ForegroundColor Green

	#-----------------------------------------------------------------------------------------------
	# Change Computer Account Password
	#-----------------------------------------------------------------------------------------------
	$user = $KrbParams.Parameters.TrustRealm.WebServer.NetBiosName
	$password = $KrbParams.Parameters.TrustRealm.WebServer.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd

	$user = $KrbParams.Parameters.TrustRealm.FileShare.NetBiosName
	$password = $KrbParams.Parameters.TrustRealm.FileShare.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd

	$user = $KrbParams.Parameters.TrustRealm.LdapServer.NetBiosName
	$password = $KrbParams.Parameters.TrustRealm.LdapServer.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd

	$user = $KrbParams.Parameters.TrustRealm.KDC.NetBiosName
	$password = $KrbParams.Parameters.TrustRealm.KDC.Password
	$pwd = ConvertTo-SecureString $password -AsPlainText -Force
	Write-ConfigLog "Changing user account $user's password."
	Set-ADAccountPassword -Identity $user -NewPassword $pwd
	ksetup /setcomputerpassword $password

	#-----------------------------------------------------------------------------------------------
	# Configure Group Policy for Claims
	#-----------------------------------------------------------------------------------------------
	Write-Host "Extract GPOBackup files"
	&"$WorkingPath\Scripts\Extract-ZipFile.ps1" -ZipFile "$WorkingPath\Scripts\Dc02GPO.zip" -Destination "$WorkingPath\Scripts\Dc02GPO"

	Write-Host "Update Group Policy"
	$currDomainName = (Get-WmiObject win32_computersystem).Domain
	$currDomain = Get-ADDomain $currDomainName
	if($currDomain.name -ne "contoso") {
		Get-ChildItem -Path "$WorkingPath\Scripts\Dc02GPO" -exclude *.pol -File -Recurse | ForEach-Object {
			$content =($_|Get-Content)
			if ($content | Select-String -Pattern 'contoso') {
				$content = $content -replace 'contoso',$currDomain.name   
				[IO.File]::WriteAllText($_.FullName, ($content -join "`r`n"))
			}
		}
	}
	Write-Host "Configurating Group Policy"
	Import-GPO -BackupId FC378AD4-C0A2-40D8-9072-D7D6A7B587E8 -TargetName "Default Domain Policy" -Path "$WorkingPath\Scripts\Dc02GPO" -CreateIfNeeded
    	
	gpupdate /force 
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

    # Complete configure
    Config-DC02
	
	Complete-Configure
}

Main