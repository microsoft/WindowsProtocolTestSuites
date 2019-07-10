###########################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
###########################################################################################

###########################################################################################
## Add AD Users Accounts for Kerberos test suite
###########################################################################################

$ldifOpFileName = ".\ldifopfile.ldif"
$queryOutFileName = ".\outputdata.ldf"
$os2012R2 = "6.3"
$dataFile = ".\ParamConfig.xml"

#------------------------------------------------------------------------------------------
# Function: CreateAndExecuteLDIF
# Append content to $ldifOpFileName and call ldifde to execute on specified $Server
#------------------------------------------------------------------------------------------
Function CreateAndExecuteLDIF()
{
    Param
    (
        [alias("c")]
        [string]
        $Content,

        [alias("s")]
        [string]
        $Server
    )
    
    $Content | Out-File -FilePath $ldifOpFileName -Force

    cmd /c ldifde -s $Server -i -f $ldifOpFileName 
   
}

Function CreateAndExecuteLDIFWithSASL()
{
    Param
    (
        [alias("c")]
        [string]
        $Content,

        [alias("s")]
        [string]
        $Server
    )

    $Content | Out-File -FilePath $ldifOpFileName -Force

    cmd /c ldifde -s $Server -i -f $ldifOpFileName -h
}

#------------------------------------------------------------------------------------------
# Function: AddUser
# Add new user to $server
#------------------------------------------------------------------------------------------
Function AddUser([string]$server, [string]$domain, [string]$userName, [string] $attributes)
{
    $ldifContent = "DN: CN=$userName,CN=Users," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: add
objectClass: user
samAccountName: {1}" -f $ldifContent, $userName

    if($attributes -ne $null)
    {
        $ldifContent = $ldifContent +"
" + $attributes
    }
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

#------------------------------------------------------------------------------------------
# Function: ChangePassword
# Change user password
#------------------------------------------------------------------------------------------
Function ChangePassword([string]$server, [string]$domain, [string]$userName, [string]$password)
{
    $ldifContent = "DN: CN=$userName,CN=Users," +"DC=" + $domain.Replace(".", ",DC=")
    $password = '"'+$password+'"'
    $plainTextBytes = [System.Text.Encoding]::Unicode.GetBytes($password);
    $base64 = [Convert]::ToBase64String($plainTextBytes);

    $ldifContent = "{0}
changetype: modify
replace: unicodePwd
unicodePwd:: {1}
-" -f $ldifContent, $base64
    
    CreateAndExecuteLDIFWithSASL -Content $ldifContent -Server $server
}

#------------------------------------------------------------------------------------------
# Function: DeleteUser
# Delete specified user
#------------------------------------------------------------------------------------------
Function DeleteUser([string]$server, [string]$domain, [string]$userName)
{
    $ldifContent = "DN: CN=$userName,CN=Users," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: delete" -f $ldifContent
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

#------------------------------------------------------------------------------------------
# Function: NewADGroup
# Create new ADGroup
#------------------------------------------------------------------------------------------
Function NewADGroup([string]$server, [string]$domain, [string]$groupName, [string]$groupType, [string] $attributes)
{
    $ldifContent = "DN: CN=$groupName,CN=Users," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: add
objectClass: group
samAccountName: {1}" -f $ldifContent, $groupName

    if($groupType -eq "Global")
    {
        $ldifContent = $ldifContent + "
groupType: -2147483646"
    }
    
    if($groupType -eq "DomainLocal")
    {
            $ldifContent = $ldifContent + "
groupType: -2147483644"
    }

    if($attributes -ne $null)
    {
        $ldifContent = $ldifContent +"
" + $attributes
    }
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

#------------------------------------------------------------------------------------------
# Function: DeleteADGroup
# Delete specified ADGroup
#------------------------------------------------------------------------------------------
Function DeleteADGroup([string]$server, [string]$domain, [string]$groupName)
{
    $ldifContent = "DN: CN=$groupName,CN=Users," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: delete" -f $ldifContent
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

#------------------------------------------------------------------------------------------
# Function: QueryObject
# Query objet from LDAP Server
#------------------------------------------------------------------------------------------
Function QueryObject([string]$server, [string]$classType, [string]$filter)
{
    Write-Host $filter
    #Read data from output file
    $content = Get-Content $queryOutFileName  #[IO.File]::ReadAllText("D:\OpenSource\WindowsProtocolTestSuitesHelper\TestSuites\Kerberos\Setup\Lab\VSTORMLITEFiles\PostScript\outputdata.ldif")
    return $content
}

#------------------------------------------------------------------------------------------
# Function: DeleteADComputer
# Delete specified ADComputer
#------------------------------------------------------------------------------------------
Function DeleteADComputer([string]$server, [string]$domain, [string]$computerName)
{
    $ldifContent = "DN: CN=$computerName,CN=Computers," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: delete" -f $ldifContent
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

#------------------------------------------------------------------------------------------
# Function: DeleteADComputer
# Create new ADComputer
#------------------------------------------------------------------------------------------
Function NewADComputer([string]$server, [string]$domain, [string]$computerName, [string] $attributes)
{
    $ldifContent = "DN: CN=$computerName,CN=Computers," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: add
objectClass: computer
userAccountControl: {1}
sAMAccountName: $computerName$" -f $ldifContent, 4096

    if($attributes -ne $null)
    {
        $ldifContent = $ldifContent +"
" + $attributes
    }
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

#------------------------------------------------------------------------------------------
# Function: ChangeComputerPassword
# Change ADCompuer password
#------------------------------------------------------------------------------------------
Function ChangeComputerPassword([string]$server, [string]$domain, [string]$computerName, [string]$password)
{
    $ldifContent = "DN: CN=$computerName,CN=Computers," +"DC=" + $domain.Replace(".", ",DC=")

    $plainTextBytes = [System.Text.Encoding]::Unicode.GetBytes('"$password"');
    $base64 = [Convert]::ToBase64String($plainTextBytes);

    $ldifContent = "{0}
changetype: modify
replace: unicodePwd
unicodePwd:: {1}
-" -f $ldifContent, $base64
    
    CreateAndExecuteLDIFWithSASL -Content $ldifContent -Server $server
}

#------------------------------------------------------------------------------------------
# Function: AddGroupMember
# Add memeber to an exists group
#------------------------------------------------------------------------------------------
Function AddGroupMember([string]$server, [string]$domain, [string]$groupName, [string]$member)
{
    $ldifContent = "DN: CN=$groupName,CN=Users," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: modify
add: member
member: {1}
-" -f $ldifContent, $member
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

Function AddComputerAttribute([string]$server, [string]$domain, [string]$computerName, [string]$attrName, [string]$attrValue)
{
    $ldifContent = "DN: CN=$computerName,CN=Computers," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: modify
add: {1}
{1}: {2}
-" -f $ldifContent, $attrName, $attrValue
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

Function UpdateAccountControl([string]$server, [string]$domain, [string]$userName, [string]$userAccountControl)
{
	$ldifContent = "DN: CN=$userName,CN=Users," +"DC=" + $domain.Replace(".", ",DC=")

    $ldifContent = "{0}
changetype: modify
replace: userAccountControl
userAccountControl: {1}
-" -f $ldifContent, $userAccountControl, $userAccountControl
    
    CreateAndExecuteLDIF -Content $ldifContent -Server $server
}

Function ConfigUsersAndComputers_DC01([string]$server, [xml]$KrbParams)
{ 
    try
    {
        $osVersion = Invoke-Command -ComputerName $server -ScriptBlock {"" + [System.Environment]::OSVersion.Version.Major + "." + [System.Environment]::OSVersion.Version.Minor} -ErrorAction Stop
        Write-Host "SUT OS version is $osVersion" -ForegroundColor Yellow
    }
    catch [System.Exception]
    {
        Write-Host "Failed to get OS version on server: " + $server + " with error message: " $_.Exception.Message -ForegroundColor Red
        exit 1
    }   
    
	$domain = $KrbParams.Parameters.LocalRealm.RealmName
	
    Write-Host "Adding AD users accounts..."
	
    $user = $KrbParams.Parameters.LocalRealm.User01.Username
    $password = $KrbParams.Parameters.LocalRealm.User01.Password

    try
    {
		DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
	
	$principalName = $user + '@' + $domain
	$userAttr = "displayName: {0}
    userPrincipalName: {1}
    homeDirectory: {2}
    homeDrive: {3}
    profilePath: {4}
    scriptPath: {5}
    msDS-SupportedEncryptionTypes: {6}" -f $user, $principalName, "c:\home\", "c:", "c:\profiles\", "c:\scripts\", 31
	
    
    try
    {
        AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	    ChangePassword -server $server -domain $domain -userName $user -password $password 
        UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048 

        $group = $KrbParams.Parameters.LocalRealm.User01.Group
	
        NewADGroup -server $server -domain $domain -groupName $group -groupType "Global"
        $member = "CN=$user,CN=Users,DC=" + $domain.Replace(".", ",DC=")
	    AddGroupMember -server $server -domain $domain -groupName $group -member $member
    }
    catch [System.Exception]
    {
        Write-Host "Can't add group memeber to $server"+ " with error message: " $_.Exception.Message -ForegroundColor Red
        exit 1
    }
    
    $user = $KrbParams.Parameters.LocalRealm.User02.Username
    $password = $KrbParams.Parameters.LocalRealm.User02.Password
    try
    {
		DeleteUser -server $server -domain $domain -userName $user 
    }
    catch [System.Exception]
    {
        Write-Host "Can't remove $user"+ " with error message: " $_.Exception.Message -ForegroundColor Red
        exit 1
    }
    
    Write-Host "Creating new user account $user"
	$principalName = $user + '@' + $domain
	$userAttr = "displayName: {0}
    department: {1}
    msDS-SupportedEncryptionTypes: {2}" -f $user, "HR", 131103
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
    
    $user = $KrbParams.Parameters.LocalRealm.User03.Username
    $password = $KrbParams.Parameters.LocalRealm.User03.Password
    try
    {
		DeleteUser -server $server -domain $domain -userName $user 
    }
    catch [System.Exception]
    {
        Write-Host "Can't remove $user"+ " with error message: " $_.Exception.Message -ForegroundColor Red
        exit 1
    }

    Write-Host "Creating new user account $user"

	$principalName = $user + '@' + $domain
	$userAttr = "displayName: {0}" -f $user
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 1114624
	
    $user = $KrbParams.Parameters.LocalRealm.User04.Username
    $password = $KrbParams.Parameters.LocalRealm.User04.Password
    try
    {
		DeleteUser -server $server -domain $domain -userName $user -ErrorAction Stop
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
	$principalName = $user + '@' + $domain
	$userAttr = "displayName: {0}
servicePrincipalName: {1}" -f $user, "abc/$user"
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 590336
	
    $user = $KrbParams.Parameters.LocalRealm.User05.Username
    $password = $KrbParams.Parameters.LocalRealm.User05.Password
    try
    {
		DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
    $principalName = $user + '@' + $domain
	$userAttr = "displayName: {0}" -f $user
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048

    $user = $KrbParams.Parameters.LocalRealm.User06.Username
    $password = $KrbParams.Parameters.LocalRealm.User06.Password
    try
    {
        DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
    $principalName = $user + '@' + $domain
	$userAttr = "displayName: {0}
userPrincipalName: {1}" -f $user, $principalName
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 4260352
	
    $user = $KrbParams.Parameters.LocalRealm.User07.Username
    $password = $KrbParams.Parameters.LocalRealm.User07.Password
    $principalName = $user + '@' + $domain
    try
    {
        DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
    $userAttr = "displayName: {0}
userPrincipalName: {1}" -f $user, $principalName
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr

    $user = $KrbParams.Parameters.LocalRealm.User08.Username
    $password = $KrbParams.Parameters.LocalRealm.User08.Password
	$principalName = $user + '@' + $domain

    try
    {
        DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }
    
    Write-Host "Creating new user account $user"
    $userAttr = "displayName: {0}
userPrincipalName: {1}
accountExpires: {2}" -f $user, $principalName, 129382848000000000
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
    
    $user = $KrbParams.Parameters.LocalRealm.User09.Username
    $password = $KrbParams.Parameters.LocalRealm.User09.Password
    $principalName = $user + '@' + $domain
    try
    {
	    Invoke-Command -ComputerName $server -ScriptBlock {"Get-ADDefaultDomainPasswordPolicy | Set-ADDefaultDomainPasswordPolicy -ComplexityEnabled $false -MinPasswordLength 6 -MaxPasswordAge 30.0 -LockoutThreshold 1 -LockoutObservationWindow 1.0 -LockoutDuration 365.0
	    gpupdate /force"}  -ErrorAction Stop
	}
    catch [System.Exception]
    {
        Write-Host "Failed to Set-ADDefaultDomainPasswordPolicy on server: " + $server + " with error message: " $_.Exception.Message -ForegroundColor Red
        exit 1
    }

    Write-Host "Creating new user account $user"

    try
    {
        DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
        Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
        exit 1
    }

    $userAttr = "displayName: {0}
userPrincipalName: {1}" -f $user, $principalName
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
	
    $wrongPassword = "1234567"
    $wrongPwd = ConvertTo-SecureString $wrongPassword -AsPlainText -Force
    try
    {
        ChangePassword -server $server -domain $domain -userName $user -password $wrongPassword 
    }
    catch
    {
       Write-Host "Failed to change password of the $user account" -ForegroundColor green
       exit 1
    }

    try
    {
        Invoke-Command -ComputerName $server -ScriptBlock {"Get-ADDefaultDomainPasswordPolicy | Set-ADDefaultDomainPasswordPolicy -ComplexityEnabled $true -MinPasswordLength 7 -MaxPasswordAge 42.0 -LockoutThreshold 0
	    gpupdate /force"} -ErrorAction Stop
    }
    catch [System.Exception]
    {
        Write-Host "Failed to Set-ADDefaultDomainPasswordPolicy on server: " + $server + " with error message: " $_.Exception.Message -ForegroundColor Red
        exit 1
    }

    $user = $KrbParams.Parameters.LocalRealm.User10.Username
    $password = $KrbParams.Parameters.LocalRealm.User10.Password
	$principalName = $user + '@' + $domain
    try
    {
        DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
    $userAttr = "displayName: {0}
userPrincipalName: {1}
description: {2}
logonHours:: {3}" -f $user, $principalName, "This user is set to be always out of logon hours", "AAAAAAAAAAAAAAAAAAAAAAAAAAAA"
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
	
    $user = $KrbParams.Parameters.LocalRealm.User11.Username
    $password = $KrbParams.Parameters.LocalRealm.User11.Password
	$principalName = $user + '@' + $domain
    try
    {
        DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }
    Write-Host "Creating new user account $user"
	$userAttr = "displayName: {0}
userPrincipalName: {1}" -f $user, $principalName
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 4194816
	
    $user = $KrbParams.Parameters.LocalRealm.User12.Username
    $password = $KrbParams.Parameters.LocalRealm.User12.Password
	$principalName = $user + '@' + $domain
    try
    {
		DeleteUser -server $server -domain $domain -userName $user
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
	$userAttr = "displayName: {0}
userPrincipalName: {1}" -f $user, $principalName
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 4194816

    $user = $KrbParams.Parameters.LocalRealm.User13.Username
    $password = $KrbParams.Parameters.LocalRealm.User13.Password
	$principalName = $user + '@' + $domain
    try
    {
		DeleteUser -server $server -domain $domain -userName $user
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }
    Write-Host "Creating new user account $user"
	$userAttr = "displayName: {0}
userPrincipalName: {1}
msDS-SupportedEncryptionTypes: {2}" -f $user, $principalName, 31
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password    
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
	
    $group = $KrbParams.Parameters.LocalRealm.User13.Group
    try
    {
		DeleteADGroup -server $server -domain $domain -groupName $group 
    }
    catch
    {
       Write-Host "Can't remove $group, does not exist!" -ForegroundColor Red
       exit 1
    }
	NewADGroup -server $server -domain $domain -groupName $group -groupType "Global"
	
    $member = "CN=$user,CN=users,DC=" + $domain.Replace(".", ",DC=")
    AddGroupMember -server $server -domain $domain -groupName $group -member $member
	
    #user14 is for DES downgrade protection
    $user = $KrbParams.Parameters.LocalRealm.User14.Username
    $password = $KrbParams.Parameters.LocalRealm.User14.Password
	$principalName = $user + '@' + $domain
    try
    {
		DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
	$userAttr = "displayName: {0}
userPrincipalName: {1}
msDS-SupportedEncryptionTypes: {2}" -f $user, $principalName, 3
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 2163200
    
    $os2012R2 = "6.3"
    if([double]$osVersion -ge [double]$os2012R2)
    {
        $user = $KrbParams.Parameters.LocalRealm.User15.Username
        $password = $KrbParams.Parameters.LocalRealm.User15.Password
        $group = $KrbParams.Parameters.LocalRealm.User15.Group
        $etype = $KrbParams.Parameters.LocalRealm.User15.SupportedEtype
        try
        {
			DeleteUser -server $server -domain $domain -userName $user
        }
        catch
        {
           Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
           exit 1
        }
        Write-Host "Creating new user account $user"
		
		$userAttr = "displayName: {0}
msDS-SupportedEncryptionTypes: 3" -f $user
		AddUser -server $server -domain $domain -userName $user -attributes $userAttr
		ChangePassword -server $server -domain $domain -userName $user -password $password
        UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
	
		## Samba not support protected user group
		$member = "CN=$user,CN=Users,DC=" + $domain.Replace(".", ",DC=")
		AddGroupMember -server $server -domain $domain -groupName $group -member $member

        $user = $KrbParams.Parameters.LocalRealm.User16.Username
        $password = $KrbParams.Parameters.LocalRealm.User16.Password
        $group = $KrbParams.Parameters.LocalRealm.User16.Group
        $etype = $KrbParams.Parameters.LocalRealm.User16.SupportedEtype
        $department = $KrbParams.Parameters.LocalRealm.User16.Department
        try
        {
			DeleteUser -server $server -domain $domain -userName $user
        }
        catch
        {
           Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
           exit 1
        }
        Write-Host "Creating new user account $user"
		$userAttr = "displayName: {0}
msDS-SupportedEncryptionTypes: 3
Department: {2}" -f $user, $etype, $department
		AddUser -server $server -domain $domain -userName $user -attributes $userAttr
		ChangePassword -server $server -domain $domain -userName $user -password $password
        UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
	
		$member = "CN=$user,CN=Users,DC=" + $domain.Replace(".", ",DC=")
		AddGroupMember -server $server -domain $domain -groupName $group -member $member

        Set-ADUser -Identity $user -AuthenticationPolicy UserRestrictedPolicy

        $user = $KrbParams.Parameters.LocalRealm.User17.Username
        $password = $KrbParams.Parameters.LocalRealm.User17.Password
        $group = $KrbParams.Parameters.LocalRealm.User17.Group
        $etype = $KrbParams.Parameters.LocalRealm.User17.SupportedEtype

        try
        {
			DeleteUser -server $server -domain $domain -userName $user
        }
        catch
        {
           Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
           exit 1
        }

        Write-Host "Creating new user account $user"
		$userAttr = "displayName: {0}
msDS-SupportedEncryptionTypes: 3" -f $user
		AddUser -server $server -domain $domain -userName $user -attributes $userAttr
		ChangePassword -server $server -domain $domain -userName $user -password $password
        UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
		
		$member = "CN=$user,CN=Users," + $domain.Replace(".", ",DC=")
		AddGroupMember -server $server -domain $domain -groupName $group -member $member

        $user = $KrbParams.Parameters.LocalRealm.User18.Username
        $password = $KrbParams.Parameters.LocalRealm.User18.Password

        try
        {
			DeleteUser -server $server -domain $domain -userName $user
        }
        catch
        {
           Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
           exit 1
        }

        Write-Host "Creating new user account $user"
		$userAttr = "displayName: {0}
msDS-SupportedEncryptionTypes: 31" -f $user
		AddUser -server $server -domain $domain -userName $user -attributes $userAttr
		ChangePassword -server $server -domain $domain -userName $user -password $password
        UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048

        $user = $KrbParams.Parameters.LocalRealm.User19.Username
        $password = $KrbParams.Parameters.LocalRealm.User19.Password
        $group = $KrbParams.Parameters.LocalRealm.User19.Group
        $department = $KrbParams.Parameters.LocalRealm.User19.Department

        try
        {
			DeleteUser -server $server -domain $domain -userName $user
        }
        catch
        {
           Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
           exit 1
        }
        Write-Host "Creating new user account $user"
		$userAttr = "displayName: {0}
msDS-SupportedEncryptionTypes: 31
Department: {2}" -f $user, $etype, $department
		AddUser -server $server -domain $domain -userName $user -attributes $userAttr
		ChangePassword -server $server -domain $domain -userName $user -password $password
        UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
		
		$member = "CN=$user,CN=Users,DC=" + $domain.Replace(".", ",DC=")
		AddGroupMember -server $server -domain $domain -groupName $group -member $member

    }

    $user = $KrbParams.Parameters.LocalRealm.User22.Username
    $password = $KrbParams.Parameters.LocalRealm.User22.Password
	$principalName = $user + '@' + $domain
    try
    {
		DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
	$userAttr = "displayName: {0}
userPrincipalName: {1}" -f $user, $principalName
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
    #-----------------------------------------------------------------------------------------------
    # Add AD Computer Accounts on DC01 for Kerberos test suite
    #-----------------------------------------------------------------------------------------------
   Write-Host "Adding AD computer accounts..."

    $userNetBiosName = $KrbParams.Parameters.LocalRealm.AuthNotRequired.NetBiosName
    $userFQDN = $KrbParams.Parameters.LocalRealm.AuthNotRequired.FQDN
    $user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
    $password = $KrbParams.Parameters.LocalRealm.AuthNotRequired.Password
    try
    {
		DeleteADComputer -server $server -domain $domain -computerName $user
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new computer account $user"
    $attrs = "servicePrincipalName: http/$userFQDN
servicePrincipalName: cifs/$userFQDN"
	NewADComputer -server $server -domain $domain -computerName $user -attributes $attrs
	ChangeComputerPassword -server $server -domain $domain -computerName $user -password $password

	Write-Host "Configure $user to Pre-AuthenticationNotRequired"
    $serverName = $KrbParams.Parameters.LocalRealm.KDC.FQDN
    $objectDN = "DC=" + $Domain.Replace(".", ",DC=")
    $userName = $KrbParams.Parameters.LocalRealm.Administrator.Username
    $password = $KrbParams.Parameters.LocalRealm.Administrator.Password
    $computerName = $user

    $userNetBiosName = $KrbParams.Parameters.LocalRealm.LocalResource01.NetBiosName
    $userFQDN = $KrbParams.Parameters.LocalRealm.LocalResource01.FQDN
    $user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
    $password = $KrbParams.Parameters.LocalRealm.LocalResource01.Password
    try
    {
		DeleteADComputer -server $server -domain $domain -computerName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new computer account $user"
	$attrs = "servicePrincipalName: host/$userFQDN"
	NewADComputer -server $server -domain $domain -computerName $user -attributes $attrs
	ChangeComputerPassword -server $server -domain $domain -computerName $user -password $password

    $userNetBiosName = $KrbParams.Parameters.LocalRealm.LocalResource02.NetBiosName
    $userFQDN = $KrbParams.Parameters.LocalRealm.LocalResource02.FQDN
    $user = $userNetBiosName.Remove($userNetBiosName.IndexOf('$'))
    $password = $KrbParams.Parameters.LocalRealm.LocalResource02.Password
    try
    {
		DeleteADComputer -server $server -domain $domain -computerName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new computer account $user"
	$attrs = "servicePrincipalName: host/$userFQDN
msDS-SupportedEncryptionTypes: 524288"
	NewADComputer -server $server -domain $domain -computerName $user -attributes $attrs
	ChangeComputerPassword -server $server -domain $domain -computerName $user -password $password

    $user = $KrbParams.Parameters.LocalRealm.ClientComputer.NetBiosName
    $password = $KrbParams.Parameters.LocalRealm.ClientComputer.Password
    Write-Host "Set $user as trusted for delegation"

    if([double]$osVersion -ge [double]$os2012R2)
    {    
        Set-ADComputer -Identity $user -Add @{'Department'="HR"}  -AuthenticationPolicy ComputerRestrictedPolicy

        $user = $KrbParams.Parameters.LocalRealm.FileShare.NetBiosName
        $password = $KrbParams.Parameters.LocalRealm.FileShare.Password
        $PWD = ConvertTo-SecureString $password -AsPlainText -Force
       Write-Host "Set $user as trusted for delegation"
        Set-ADAccountControl -Identity $user -TrustedForDelegation $true
        Set-ADComputer -Identity $user -AuthenticationPolicy ComputerRestrictedPolicy
    }
    else
    {
        AddComputerAttribute -server $server -domain $domain -computerName $user -attrName "Department" -attrValue "HR"
    }
    
    #-----------------------------------------------------------------------------------------------
    # Define Local Resource Groups
    #-----------------------------------------------------------------------------------------------
   Write-Host "Defining local resource groups..."

    $group = $KrbParams.Parameters.LocalRealm.ResourceGroup01.GroupName
    try
    {
		DeleteADGroup -server $server -domain $domain -groupName $group 
    }
    catch
    {
       Write-Host "Can't remove $group, does not exist!" -ForegroundColor Red
       exit 1
    }

	NewADGroup -server $server -domain $domain -groupName $group -groupType "DomainLocal"
	
    $user = $KrbParams.Parameters.LocalRealm.LocalResource01.Name
    $member = "CN=$user,CN=Computers,DC=" + $domain.Replace(".", ",DC=")
	AddGroupMember -server $server -domain $domain -groupName $group -member $member
    
	$user = $KrbParams.Parameters.LocalRealm.LocalResource02.Name
    $member = "CN=$user,CN=Computers,DC=" + $domain.Replace(".", ",DC=")
	AddGroupMember -server $server -domain $domain -groupName $group -member $member
   
	$userGroup = $KrbParams.Parameters.LocalRealm.User13.Group
    $member = "CN=$userGroup,CN=Users,DC=" + $domain.Replace(".", ",DC=")
	AddGroupMember -server $server -domain $domain -groupName $group -member $member

    $group = $KrbParams.Parameters.LocalRealm.ResourceGroup02.GroupName
    try
    {
		DeleteADGroup -server $server -domain $domain -groupName $group 
    }
    catch
    {
       Write-Host "Can't remove $group, does not exist!" -ForegroundColor Red
       exit 1
    }
	
	NewADGroup -server $server -domain $domain -groupName $group -groupType "DomainLocal"
	
    $user = $KrbParams.Parameters.LocalRealm.LocalResource01.Name
	$member = "CN=$user,CN=Computers,DC=" + $domain.Replace(".", ",DC=")
	AddGroupMember -server $server -domain $domain -groupName $group -member $member
	
    $user = $KrbParams.Parameters.LocalRealm.LocalResource02.Name
	$member = "CN=$user,CN=Computers,DC=" + $domain.Replace(".", ",DC=")
	AddGroupMember -server $server -domain $domain -groupName $group -member $member
	
    $userGroup = $KrbParams.Parameters.LocalRealm.User13.Group
	$member = "CN=$userGroup,CN=Users,DC=" + $domain.Replace(".", ",DC=")
	AddGroupMember -server $server -domain $domain -groupName $group -member $member
}


Function ConfigUsersAndComputers_DC02([string]$server, [xml]$KrbParams)
{
	$domain = $KrbParams.Parameters.TrustRealm.RealmName
	$user = $KrbParams.Parameters.TrustRealm.User01.Username
    $password = $KrbParams.Parameters.TrustRealm.User01.Password

    try
    {
		DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
	$principalName = $user + '@' + $domain
	$userAttr = "displayName: {0}
userPrincipalName: {1}
homeDirectory: {2}
homeDrive: {3}
profilePath: {4}
scriptPath: {5}
msDS-SupportedEncryptionTypes: {6}" -f $user, $principalName, "c:\home\", "c:", "c:\profiles\", "c:\scripts\", 31
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
	ChangePassword -server $server -domain $domain -userName $user -password $password
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
	
    $group = $KrbParams.Parameters.TrustRealm.User01.Group
    try
    {
		DeleteADGroup -server $server -domain $domain -groupName $group 
    }
    catch
    {
       Write-Host "Can't remove $group, does not exist!" -ForegroundColor Red
       exit 1
    }

	NewADGroup -server $server -domain $domain -groupName $group -groupType "Global"
	
    $member = "member: cn=$user,cn=users,DC=" + $domain.Replace(".", ",DC=")
    AddGroupMember -server $server -domain $domain -groupName $group -member $member

    $user = $KrbParams.Parameters.TrustRealm.User02.Username
    $password = $KrbParams.Parameters.TrustRealm.User02.Password
    $pwd = ConvertTo-SecureString $password -AsPlainText -Force

    try
    {
		DeleteUser -server $server -domain $domain -userName $user 
    }
    catch
    {
       Write-Host "Can't remove $user, does not exist!" -ForegroundColor Red
       exit 1
    }

    Write-Host "Creating new user account $user"
	$principalName = $user + '@' + $Domain
$userAttr = "displayName: {0}
description: {1}
msDS-SupportedEncryptionTypes: 131103
userPrincipalName: {2}" -f $user, "MS-KILE", $principalName
	AddUser -server $server -domain $domain -userName $user -attributes $userAttr
    UpdateAccountControl -server $server -domain $domain -userName $user -userAccountControl 66048
}

Function CheckAndInstallADFSFeature
{
    # Windows 10 1809 build
    $1809Build = "17763"
    # Windows 10 1903 build
    $1903Build = "18362"
    # Get running Windows build
    $WindowsBuild = (Get-WmiObject -Class Win32_OperatingSystem).BuildNumber
    # Getting executing directory - considering including the source files for RSAT to be installed from local source
    #$runningDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
     try {
        if (($WindowsBuild -eq $1809Build) -OR ($WindowsBuild -ge $1903Build)) {
            Write-Host -Verbose "Running correct Windows 10 build number for installing RSAT with Features on Demand. Build number is: $WindowsBuild"
   
            Write-Host -Verbose "Script is installing all available RSAT features"
            $Install = Get-WindowsCapability -Online | Where-Object {$_.Name -like "Rsat*" -AND $_.State -eq "NotPresent"}
            if ($Install -ne $null) {
                foreach ($Item in $Install) {
                    $RsatItem = $Item.Name

                    Write-Host -Verbose "Adding $RsatItem to Windows"
                    try 
                    {
                        Add-WindowsCapability -Online -Name $RsatItem -ErrorAction Stop
                    }
                    catch [System.Exception]
                    {
                        Write-Host "Failed to add $RsatItem to Windows: " + $_.Exception.Message -ForegroundColor Red
                        exit 1
                    }
                }
            }
            else {
                Write-Host -Verbose "All RSAT features seems to be installed already"
            }    
        }
        else
        {
            Write-Warning -Message "The script can only install RSAT feature for Windows 10 build 17763 and 18362."
            Write-Warning -Message "For other OS version, please install or enable RSAT feature manually before you run this script, otherwise you will get some failures."
        }
    }
    catch [System.Exception]
    {
        Write-Host "Failed to check and install ADFS feature:" + $_.Exception.Message -ForegroundColor Red
       
        exit 1
    }
}

# Check for administrative rights

if (-NOT([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Warning -Message "The script requires administrative previleges."
    break
}   

# Start to config DC01 remotely
if(Test-Path -Path $dataFile)
{
    [xml]$Script:Params = Get-Content -Path $dataFile 
	[xml]$configFile = Get-Content -Path $dataFile
	$LocalRealmKDCFQDN = $configFile.Parameters.LocalRealm.KDC.FQDN
        
    CheckAndInstallADFSFeature

    ConfigUsersAndComputers_DC01 -server $LocalRealmKDCFQDN -KrbParams $Script:KrbParams 

    Write-Host "Config users and computers on DC01 is finished."

    exit 0
}
else
{
    Write-Error "$dataFile is not found."
    exit 1
}    

