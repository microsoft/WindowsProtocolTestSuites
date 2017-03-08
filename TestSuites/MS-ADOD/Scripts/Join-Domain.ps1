#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Join-Domain-Method1.ps1
## Purpose:        Join a specified domain by creating an account or by a predefined account.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7, Windows 8
##           
##############################################################################

#-------------------------------------------------------------------------
#Predefined Flags for domain joining options
#-------------------------------------------------------------------------

$JOIN_DOMAIN = 1
$ACCT_CREATE = 2
$ACCT_DELETE = 4
$WIN9X_UPGRADE = 16
$DOMAIN_JOIN_IF_JOINED = 32
$JOIN_UNSECURE = 64
$MACHINE_PASSWORD_PASSED = 128
$DEFERRED_SPN_SET = 256
$INSTALL_INVOCATION = 262144

#-------------------------------------------------------------------------
#Function: JoinDomain-PredefAccount
#Env: Windows2008R2, Samba 4 PDC
#Usage: Join Domain using a Predefined Account
#-------------------------------------------------------------------------

function JoinDomain-PredefAccount {

  param(
    [string]$domainName = $(Throw "Please enter a domain name."),
    [string]$userName = $(Throw "Please enter a full user name."),
    [string]$password = $(Throw "Please enter a password.")
  )

  echo "JoinDomain-PredefAccount Executing" | out-file "c:\MS-ADOD.log.txt" -append
  echo $domainName | out-file "c:\MS-ADOD.log.txt" -append
  echo $userName | out-file "c:\MS-ADOD.log.txt" -append
  echo $password | out-file "c:\MS-ADOD.log.txt" -append

  $regKey = "HKLM:\SYSTEM\CurrentControlSet\services\LanmanWorkstation\Parameters"
  $regKeyExist = Test-Path -path $regKey
  if($regKeyExist -eq $true) {
    Set-ItemProperty -path $regKey -name DomainCompatibilityMode -value 0 -type DWORD
    Set-ItemProperty -path $regKey -name DNSNameResolutionRequired -value 1 -type DWORD
  }

  $regKey = "HKLM:\SYSTEM\CurrentControlSet\services\Netlogon\Parameters"
  $regKeyExist = Test-Path -path $regKey
  if($regKeyExist -eq $true) {
    Set-ItemProperty -path $regKey -name RequireSignOrSeal -value 1 -type DWORD
    Set-ItemProperty -path $regKey -name RequireStrongKey -value 1 -type DWORD
  }

  $objComputer = Get-Wmiobject -Class "Win32_ComputerSystem"

  $objJoinDomain = $objComputer.JoinDomainOrWorkgroup($domainName, $password, $userName, $Null, $JOIN_DOMAIN+$DOMAIN_JOIN_IF_JOINED)
  $errorCode= $ObjJoinDomain.Returnvalue

  if($errorCode-eq 0) {
    echo "Successfully joined to the domain with predefined account." | out-file "c:\MS-ADOD.log.txt" -append
    $regKey = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon'
    $regKeyExist = Test-Path -path $regKey
    if($regKeyExist -eq $true) {
      set-ItemProperty -path $regKey -name AutoAdminLogon -value "1"
      Set-ItemProperty -path $regKey -name DefaultUserName -value $userName
      Set-ItemProperty -path $regKey -name DefaultDomainName -value $domainName
      Set-ItemProperty -path $regKey -name DefaultPassword -value $password
    }
  }
  else {
    echo "Join Domain Failed!, Error code: $errorCode." | out-file "c:\MS-ADOD.log.txt" -append
  }

  return $errorCode
}

#-------------------------------------------------------------------------
#Function: JoinDomain-CreateAccount-LDAP
#Env: Windows2008R2, Samba 4 PDC
#Usage: Join Domain by Creating an Account using LDAP
#-------------------------------------------------------------------------

function JoinDomain-CreateAccount-LDAP {

  param(
    [string]$domainName = $(Throw "Please enter a domain name."),
    [string]$userName = $(Throw "Please enter a full user name."),
    [string]$password = $(Throw "Please enter a password.")
  )

  echo "JoinDomain-CreateAccount-LDAP Executing" | out-file "c:\MS-ADOD.log.txt" -append
  echo $domainName | out-file "c:\MS-ADOD.log.txt" -append
  echo $userName | out-file "c:\MS-ADOD.log.txt" -append
  echo $password | out-file "c:\MS-ADOD.log.txt" -append

  $regKey = "HKLM:\SYSTEM\CurrentControlSet\services\LanmanWorkstation\Parameters"
  $regKeyExist = Test-Path -path $regKey
  if($regKeyExist -eq $true) {
    Set-ItemProperty -path $regKey -name DomainCompatibilityMode -value 0 -type DWORD
    Set-ItemProperty -path $regKey -name DNSNameResolutionRequired -value 1 -type DWORD
  }

  $regKey = "HKLM:\SYSTEM\CurrentControlSet\services\Netlogon\Parameters"
  $regKeyExist = Test-Path -path $regKey
  if($regKeyExist -eq $true) {
    Set-ItemProperty -path $regKey -name RequireSignOrSeal -value 1 -type DWORD
    Set-ItemProperty -path $regKey -name RequireStrongKey -value 1 -type DWORD
  }
  
  $objComputer = Get-Wmiobject -Class "Win32_ComputerSystem"

  $objJoinDomain = $objComputer.JoinDomainOrWorkgroup($domainName, $password, $UserName, $Null, $JOIN_DOMAIN+$ACCT_CREATE)
  $errorCode= $objJoinDomain.Returnvalue
  if($errorCode-eq 0) {
    echo "Successfully joined to the domain." | out-file "c:\MS-ADOD.log.txt" -append
    $regKey = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon'
    $regKeyExist = Test-Path -path $regKey
    if($regKeyExist -eq $true) {
      set-ItemProperty -path $regKey -name AutoAdminLogon -value "1"
      Set-ItemProperty -path $regKey -name DefaultUserName -value $userName
      Set-ItemProperty -path $regKey -name DefaultDomainName -value $domainName
      Set-ItemProperty -path $regKey -name DefaultPassword -value $password
    }
  }
  else {
    echo "Join Domain Failed!, Error code: $errorcode." | out-file "c:\MS-ADOD.log.txt" -append
  }
  return $errorcode
}


#--------------------------------------------------------------------------
#Function: JoinDomain-CreateAccount-SAMR
#Env: Samba 3 PDC
#Usage: Join Domain by Creating an Account using SAMR
#--------------------------------------------------------------------------

function JoinDomain-CreateAccount-SAMR {

  param(
    [string]$domainName = $(Throw "Please enter a domain name."),
    [string]$userName = $(Throw "Please enter a user name."),
    [string]$password = $(Throw "Please enter a password.")
  )

  $regKey = "HKLM:\SYSTEM\CurrentControlSet\services\LanmanWorkstation\Parameters"
  $regKeyExist = Test-Path -path $regKey
  if($regKeyExist -eq $true) {
    Set-ItemProperty -path $regKey -name DomainCompatibilityMode -value 1 -type DWORD
    Set-ItemProperty -path $regKey -name DNSNameResolutionRequired -value 0 -type DWORD
  }
  
  $regKey = "HKLM:\SYSTEM\CurrentControlSet\services\Netlogon\Parameters"
  $regKeyExist = Test-Path -path $regKey
  if($regKeyExist -eq $true) {
    Set-ItemProperty -path $regKey -name RequireSignOrSeal -value 1 -type DWORD
    Set-ItemProperty -path $regKey -name RequireStrongKey -value 1 -type DWORD
  }

  $objComputer = Get-Wmiobject -Class "Win32_ComputerSystem"

  $objJoinDomain = $objComputer.JoinDomainOrWorkgroup($domainName, $password, $UserName, $Null, $JOIN_DOMAIN+$ACCT_CREATE)
  $errorCode= $objJoinDomain.Returnvalue
  if($errorCode-eq 0) {
    echo "Successfully joined to the domain." | out-file "c:\MS-ADOD.log.txt" -append
    $regKey = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon'
    $regKeyExist = Test-Path -path $regKey
    if($regKeyExist -eq $true) {
      set-ItemProperty -path $regKey -name AutoAdminLogon -value "1"
      Set-ItemProperty -path $regKey -name DefaultUserName -value $userName
      Set-ItemProperty -path $regKey -name DefaultDomainName -value $domainName
      Set-ItemProperty -path $regKey -name DefaultPassword -value $password
    }
  }
  else {
    echo "Join Domain Failed!, Error code: $errorcode." | out-file "c:\MS-ADOD.log.txt" -append
  }
  return $errorcode
}