#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           New-IADUser.ps1
## Purpose:        Create a new user account with the specified options.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################

#================Flags for "userAccountControl" Property===================#
$SCRIPT                     = 0x0001
$ACCOUNTDISABLE             = 0X0002
$HOMEDIR_REQUIRED           = 0X0008
$LOCKOUT                    = 0X0010
$PASSWD_NOTREQD             = 0X0020
$PASSWD_CANT_CHANGE         = 0X0040
$ENCRYPTED_TEXT_PWD_ALLOWED = 0X0080
$TEMP_DUPLICATE_ACCOUNT     = 0X0100
$NORMAL_ACCOUNT             = 0X0200
$INTERDOMAIN_TRUST_ACCOUNT  = 0X0800
$WORKSTATION_TRUST_ACCOUNT  = 0x1000 
$SERVER_TRUST_ACCOUNT       = 0x2000 
$DONT_EXPIRE_PASSWORD       = 0x10000 
$MNS_LOGON_ACCOUNT          = 0x20000 
$SMARTCARD_REQUIRED         = 0x40000 
$TRUSTED_FOR_DELEGATION     = 0x80000 
$NOT_DELEGATED              = 0x100000 
$USE_DES_KEY_ONLY           = 0x200000 
$DONT_REQ_PREAUTH           = 0x400000 
$PASSWORD_EXPIRED           = 0x800000 
$TRUSTED_TO_AUTH_FOR_DELEGATION = 0x1000000


#=================================================================#

function New-IADUser {
  param (
    [string]$Name = $(Throw "Please enter a full user name"),
    [DirectoryServices.DirectoryEntry]$SearchRoot = $(Throw "Must input a SearchRoot"),
    [DirectoryServices.DirectoryEntry]$ParentContainer = $(Throw "Must input a ParentContainer for directory object."),
    [string]$sAMAccountName = $(Throw "Please enter a sAMAccountName."),
    [string]$Password = $(Throw "Please enter a Password for this user account."),
    [switch]$UserMustChangePassword,
    [switch]$PasswordNeverExpires,
    [switch]$EnableAccount
  )

  if($sAMAccountName -match '\s') {
    echo "Failed: sAMAccountName cannot contain spaces." | out-file "C:\MS-ADOD.log.txt" -append
    return $null
  }

  $ExistName = Get-IADUser -Name $Name -SearchRoot $SearchRoot
  if($ExistName -ne $null) {
    echo "Failed: User Account with the name $Name already exists in the domain." | out-file "C:\MS-ADOD.log.txt" -append
    return $null
  }
  $ExistName = Get-IADUser -Name $sAMAccountName -SearchRoot $SearchRoot
  if($ExistName -ne $null) {
    echo "Failed: User Account with the name $sAMAccountName already exists in the domain." | out-file "C:\MS-ADOD.log.txt" -append
    return $null
  }

  if($UserMustChangePassword -and $PasswordNeverExpires) {
    echo "Failed: UserMustChangePassword and PasswordNeverExpires could not be selected together." | out-file "C:\MS-ADOD.log.txt" -append
    return $null
  }

  $NewUser = $ParentContainer.Children.Add("CN="+$Name, "user")
  if($Name -match '\s') {
    $n = $Name.Split()
    $FirstName = $n[0]
    $LastName = "$($n[1..$n.length])"
    $NewUser.InvokeSet("sn", $LastName)
  }
  else {
    $FirstName = $Name
  }

  $NewUser.InvokeSet("givenName", $FirstName)
  $NewUser.InvokeSet("displayName", $Name)
  $Suffix = $SearchRoot.distinguishedName -replace "dc=" -replace ",","."
  $upn = "$sAMAccountName@$Suffix"
  $NewUser.InvokeSet("userPrincipalName", $upn)
  $NewUser.InvokeSet("sAMAccountName", $sAMAccountName)
  $NewUser.CommitChanges()

  #==================================================================================#
  #pay attention to this expression, setting password will return something unvisible#
  #==================================================================================#
  $null = $NewUser.Invoke("SetPassword", $Password)

  if($UserMustChangePassword) {
    $NewUser.pwdLastSet = 0
  }

  if($PasswordNeverExpires) {
    $NewUser.userAccountControl[0] = $NewUser.userAccountControl[0] -bor $DONT_EXPIRE_PASSWORD
  }

  if($EnableAccount) {
    $NewUser.InvokeSet("AccountDisabled", $false)
  }
  else {
    $NewUser.InvokeSet("AccountDisabled", $true)
  }

  if(($NewUser.userAccountControl[0] -band $LOCKOUT) -ne 0) {
    $NewUser.userAccountControl[0] = $NewUser.userAccountControl[0] -bxor $LOCKOUT
  }

  $NewUser.CommitChanges()
  return $NewUser
}