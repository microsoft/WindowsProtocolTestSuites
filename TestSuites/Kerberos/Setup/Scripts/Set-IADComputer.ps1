#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-IADComputer.ps1
## Purpose:        Modify attributes of a computer object in Active Directory.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows Powershell 2.0 CTP
## Supported OS:   Windows 7, Windows 8
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
$PRE_AUTHENTICATION_NOT_REQUIRED = 0x2000000


#=================================================================#

filter Set-IADComputer {
  param (
    [string]$sAMAccountName,
    [string]$Description,
    [string]$UserPrincipalName,
    [string]$DisplayName,
    [string]$Office,
    [string]$Department,
    [string]$Manager,
    [string]$EmployeeID,
    [string]$EmployeeNumber,
    [string]$HomeDirectory,
    [string]$HomeDrive,
    [string]$Mobile,
    [string]$Password,
    [switch]$UserMustChangePassword,
    [switch]$PasswordNeverExpires,
    [switch]$PreAuthenticationNotRequired
  )

  if($_ -is [ADSI] -and $_.psbase.SchemaClassName -eq "Computer") {
    $Computer = $_
  }
  else {
    echo "The input is not a valid computer.`nPlease check the computer account existance."
    return $null
  }

  if($UserMustChangePassword -and $PasswordNeverExpires) {
    echo "Failed: UserMustChangePassword and PasswordNeverExpires could not be selected together."
    return $null
  }

  if($sAMAccountName) {
    $Computer.InvokeSet("sAMAccountName", $sAMAccountName)
  }

  if($Description) {
    $Computer.InvokeSet("Description", $Description)
  }

  if($UserPrincipalName) {
    $Computer.InvokeSet("userPrincipalName", $UserPrincipalName)
  }

  if($DisplayName) {
    $Computer.InvokeSet("displayName", $DisplayName)
  }

  if($Office) {
    $Computer.InvokeSet("physicalDeliveryOfficeName", $Office)
  } 

  if($Department) {
    $Computer.InvokeSet("department", $Department)
  }

  if($Manager) {
    $Computer.InvokeSet("manager", $Manager)
  }

  if($EmployeeID)
  {
    $Computer.InvokeSet("employeeID", $EmployeeID)
  }

  if($EmployeeNumber)
  {
    $Computer.InvokeSet("employeeNumber", $EmployeeNumber)
  }

  if($HomeDirectory)
  {
    $Computer.InvokeSet("homeDirectory", $HomeDirectory)
  }

  if($HomeDrive)
  {
    $Computer.InvokeSet("homeDrive",$HomeDrive)
  }

  if($Mobile)
  {
    $Computer.InvokeSet("mobile", $Mobile)
  }

  #==================================================================================#
  #pay attention to this expression, setting password will return something unvisible#
  #==================================================================================#
  if($Password)
  {
    $null = $Computer.Invoke("SetPassword", $Password)
  } 

  if($UserMustChangePassword)
  {
    if ($Computer.userAccountControl[0] -band 65536)
    {
      echo "The user is set to PasswordNeverExpires, cannot set UserMustChangePassword" | out-file "C:\MS-ADOD.log.txt"
      return $null
    }
    $Computer.pwdLastSet = 0
  }

  if($PasswordNeverExpires) {
    $Computer.userAccountControl[0] = $Computer.userAccountControl[0] -bor $DONT_EXPIRE_PASSWORD
  }

  if($PreAuthenticationNotRequired) {
    $Computer.userAccountControl[0] = $Computer.userAccountControl[0] -bor $PRE_AUTHENTICATION_NOT_REQUIRED
  }

  $Computer.CommitChanges()
  return $Computer
}