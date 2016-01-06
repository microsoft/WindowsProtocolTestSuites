#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
###############################################################################################
##
## Microsoft Windows PowerShell Scripting
## File:          Config-DC02.ps1
## Purpose:       Configure the Trust Realm KDC computer for Kerberos Server test suite.
## Requirements:  Windows PowerShell 2.0
## Supported OS:  Windows Server 2012 or later versions
##
###############################################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$workingPath = "c:\temp"
$env:Path += ";c:\temp;c:\temp\Scripts"

#-----------------------------------------------------------------------------------------------
# Check if the script was executed
#-----------------------------------------------------------------------------------------------
$ScriptsSignalFile = "$env:HOMEDRIVE\config-dc02computer.finished.signal"
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
	$domainName 	= $configFile.Parameters.TrustRealm.RealmName
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
Write-Info.ps1 "EXECUTING [Config-DC02.ps1] ..." -foregroundcolor cyan
Write-Info.ps1 "`$logPath = $logPath"
Write-Info.ps1 "`$logFile = $logFile"
Write-Info.ps1 "`$domainName = $domainName"

#-----------------------------------------------------------------------------------------------
# Begin to config DC02 computer
#-----------------------------------------------------------------------------------------------
$dataFile = "$WorkingPath\Data\ParamConfig.xml"
[xml]$configFile = Get-Content -Path $dataFile
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
    $LocalForest.CreateLocalSideOfTrustRelationship($configFile.Parameters.LocalRealm.RealmName, "Bidirectional", $configFile.Parameters.TrustPassword)
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
# Add AD Users Accounts on DC02 for Kerberos test suite
#-----------------------------------------------------------------------------------------------
$user = $configFile.Parameters.TrustRealm.User01.Username
$password = $configFile.Parameters.TrustRealm.User01.Password
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
$group = $configFile.Parameters.TrustRealm.User01.Group
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

$user = $configFile.Parameters.TrustRealm.User02.Username
$password = $configFile.Parameters.TrustRealm.User02.Password
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
New-ADUser -Name $user -AccountPassword $pwd -CannotChangePassword $true -CompoundIdentitySupported $true -Description "MS-KILE" -DisplayName $user -Enabled $true -KerberosEncryptionType DES,RC4,AES128,AES256 -PasswordNeverExpires $true -SamAccountName $user -UserPrincipalName $user@$domainName

#-----------------------------------------------------------------------------------------------
# Define Claim Type
#-----------------------------------------------------------------------------------------------
$claim = $configFile.Parameters.TrustRealm.ClaimType.DisplayName
$value01 = $configFile.Parameters.TrustRealm.ClaimType.Value01
$value02 = $configFile.Parameters.TrustRealm.ClaimType.Value02
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
$resource = $configFile.Parameters.TrustRealm.ResourceProperties.DisplayName
$valuetype = $configFile.Parameters.TrustRealm.ResourceProperties.ValueType
$value01 = $configFile.Parameters.TrustRealm.ResourceProperties.Value01
$value02 = $configFile.Parameters.TrustRealm.ResourceProperties.Value02
$value03 = $configFile.Parameters.TrustRealm.ResourceProperties.Value03
$lbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value01, $value01,"");
$hbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value02, $value02,"");
$mbi = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($value03, $value03,"");
Write-Info.ps1 "Creating new Resource Property $resource"
New-ADResourceProperty -DisplayName $resource -IsSecured $true -ResourcePropertyValueType $valuetype -ProtectedFromAccidentalDeletion $true -SuggestedValues $lbi,$hbi,$mbi
Add-ADResourcePropertyListMember "Global Resource Property List" -Members $resource

#-----------------------------------------------------------------------------------------------
# Define Claims transformation policy
#-----------------------------------------------------------------------------------------------
New-ADClaimTransformPolicy -Description:"Claims transformation policy to deny all claims except $claim" -Name:"DenyAllClaimsExceptTestedClaimPolicy" -DenyAllExcept:$claim -Server:"$domainName"
$trustedDomain = $configFile.Parameters.LocalRealm.RealmName
Set-ADClaimTransformLink -Identity:"$trustedDomain" -Policy:"DenyAllClaimsExceptTestedClaimPolicy" -TrustRole:Trusting

#-----------------------------------------------------------------------------------------------
# Create Central Access Rules
#-----------------------------------------------------------------------------------------------
$rules = $configFile.Parameters.TrustRealm.Rules.Name
$type = Get-ADClaimType $claim
$property = Get-ADResourceProperty $resource
$value01 = $configFile.Parameters.TrustRealm.Rules.Value01
$value02 = $configFile.Parameters.TrustRealm.Rules.Value02
$condition = "(@RESOURCE." + $property.Name + " == `"" + $value01 + "`")"
$acl = "O:SYG:SYD:AR(A;;FA;;;OW)(A;;FA;;;BA)(A;;FA;;;SY)(XA;;FA;;;AU;(@USER."+ $type.Name + " Any_of {`"" + $value02 + "`"}))"
Write-Info.ps1 "Creating new Access Rules $rules"
New-ADCentralAccessRule -Name $rules -ProtectedFromAccidentalDeletion $true -ResourceCondition $condition -CurrentAcl $acl

#-----------------------------------------------------------------------------------------------
# Create Central Access Policies
#-----------------------------------------------------------------------------------------------
$policy = $configFile.Parameters.TrustRealm.Policy.Name
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
$user = $configFile.Parameters.TrustRealm.Administrator.Username
$password = $configFile.Parameters.TrustRealm.Administrator.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd

#-----------------------------------------------------------------------------------------------
# Grant Allowed to Authenticate Permission on Computer Accounts
#-----------------------------------------------------------------------------------------------
$admin = $configFile.Parameters.LocalRealm.Administrator.Username
$password = $configFile.Parameters.LocalRealm.Administrator.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential $admin,$pwd -ErrorAction stop
$user = $configFile.Parameters.LocalRealm.User01.Username
$dc = $configFile.Parameters.LocalRealm.KDC.FQDN
Write-Info.ps1 "Get $user in another domain on server: $dc."

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
        Write-Info.ps1 "Get-AdUser failed, retry it later"
        $RetryTimes--
        sleep 10
    }
}

if ($RetryTimes -le 0)
{
    Write-Info.ps1 "Cannot Get-AdUser $user in 15 minutes, quit current script"
    exit 1
}

$user = $configFile.Parameters.TrustRealm.WebServer.NetBiosName
Write-Info.ps1 "Set ACL on computer $user."
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
Write-Info.ps1 "Set ACL on computer $adcomputer succeeded." -ForegroundColor Green

$user = $configFile.Parameters.TrustRealm.FileShare.NetBiosName
Write-Info.ps1 "Set ACL on computer $user."
$adcomputer = Get-ADComputer -Identity $user

pushd AD:
$acl = get-acl $adcomputer.distinguishedName
$accesscontroltypeallow = [System.Security.AccessControl.AccessControlType]::Allow
$allowed2authguid = New-Object GUID 68b1d179-0d15-4d4f-ab71-46152e79a7bc
$ace = New-Object System.DirectoryServices.ActiveDirectoryAccessRule $aduser.sid,"ExtendedRight",$accesscontroltypeallow,$allowed2authguid
$acl.AddAccessRule($ace)
Set-Acl -AclObject $acl $adcomputer.distinguishedName
popd
Write-Info.ps1 "Set ACL on computer $adcomputer succeeded." -ForegroundColor Green

#-----------------------------------------------------------------------------------------------
# Change Computer Account Password
#-----------------------------------------------------------------------------------------------
$user = $configFile.Parameters.TrustRealm.WebServer.NetBiosName
$password = $configFile.Parameters.TrustRealm.WebServer.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd

$user = $configFile.Parameters.TrustRealm.FileShare.NetBiosName
$password = $configFile.Parameters.TrustRealm.FileShare.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd

$user = $configFile.Parameters.TrustRealm.LdapServer.NetBiosName
$password = $configFile.Parameters.TrustRealm.LdapServer.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd

$user = $configFile.Parameters.TrustRealm.KDC.NetBiosName
$password = $configFile.Parameters.TrustRealm.KDC.Password
$pwd = ConvertTo-SecureString $password -AsPlainText -Force
Write-Info.ps1 "Changing user account $user's password."
Set-ADAccountPassword -Identity $user -NewPassword $pwd
ksetup /setcomputerpassword $password

#-----------------------------------------------------------------------------------------------
# Configure Group Policy for Claims
#-----------------------------------------------------------------------------------------------
Write-Host "Extract GPOBackup files"
C:\temp\Scripts\Extract-ZipFile.ps1 -ZipFile C:\temp\Scripts\Dc02GPO.zip -Destination C:\temp\Scripts\Dc02GPO

Write-Host "Configurating Group Policy"
Import-GPO -BackupId FC378AD4-C0A2-40D8-9072-D7D6A7B587E8 -TargetName "Default Domain Policy" -Path "C:\temp\Scripts\Dc02GPO" -CreateIfNeeded
    	
gpupdate /force 

#-----------------------------------------------------------------------------------------------
# Finished to config DC02 computer
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Write signal file: config-dc02computer.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile


#-----------------------------------------------------------------------------------------------
# Ending script
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Config finished."
Write-Info.ps1 "EXECUTE [Config-DC02.ps1] FINISHED (NOT VERIFIED)."
Stop-Transcript