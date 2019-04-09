#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Unjoin-Domain.ps1
## Purpose:        Unjoin a specified domain.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################

#-------------------------------------------------------------------------
#Function: UnjoinDomain
#Env: Windows2008R2, Samba 4 PDC, Samba 3 PDC
#Usage: Unjoin the computer from a Domain
#-------------------------------------------------------------------------
function UnjoinDomain {

  param(
    [string]$userName = $(Throw "Please enter a full qualified domain username."),
    [string]$password = $(Throw "Please enter the password for this domain user."),
    [string]$localuserName = $(Throw "Please enter a local username."),
    [string]$localpassword = $(Throw "Please the password for this local user.")
  )

  echo "UnjoinDomain Executing." | out-file "c:\MS-ADOD.log.txt" -append
  echo $UserName | out-file "c:\MS-ADOD.log.txt" -append
  echo $Password | out-file "c:\MS-ADOD.log.txt" -append

  $objComputer = Get-Wmiobject -Class "Win32_ComputerSystem"

  $objUnjoinDomain = $objComputer.UnjoinDomainOrWorkgroup($password, $userName, 4)
  $errorCode = $objUnjoinDomain.Returnvalue

  if($errorCode -eq 0) {
    echo "Successfully unjoined the domain." | out-file "c:\MS-ADOD.log.txt" -append
    $regKey = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon'
    $regKeyExist = Test-Path -path $regKey
    if($regKeyExist -eq $true) {
      set-ItemProperty -path $regKey -name AutoAdminLogon -value "1"
      Set-ItemProperty -path $regKey -name DefaultUserName -value $localuserName
      Set-ItemProperty -path $regKey -name DefaultDomainName -value ""
      Set-ItemProperty -path $regKey -name DefaultPassword -value $localpassword
    }
  }
  else {
    echo "Unjoin Domain Failed!, Error code: $errorCode." | out-file "c:\MS-ADOD.log.txt" -append
  }

  return $errorCode
}

function UnjoinDomain_method2 {

  param(
    [string]$userName = $(Throw "Please enter a full qualified domain username."),
    [string]$password = $(Throw "Please enter the password for this domain user."),
    [string]$localuserName = $(Throw "Please enter a local username."),
    [string]$localpassword = $(Throw "Please the password for this local user.")
  )

  echo "UnjoinDomain Executing." | out-file "c:\MS-ADOD.log.txt" -append
  echo $UserName | out-file "c:\MS-ADOD.log.txt" -append
  echo $Password | out-file "c:\MS-ADOD.log.txt" -append

  trap [exception]
  {
    $_ | out-file "c:\MS-ADOD.log.txt" -append
    return $false
  }
  $pwdConverted = ConvertTo-SecureString $password -AsPlainText -Force
  $cred = New-Object System.Management.Automation.PSCredential $userName, $pwdConverted -ErrorAction Stop
  Remove-Computer -Credential $cred -Force -ErrorAction Stop -WarningAction SilentlyContinue

  echo "Successfully unjoined the domain."| out-file "c:\MS-ADOD.log.txt" -append
  $regKey = 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon'
  $regKeyExist = Test-Path -path $regKey
  if($regKeyExist -eq $true) {
    set-ItemProperty -path $regKey -name AutoAdminLogon -value "1"
    Set-ItemProperty -path $regKey -name DefaultUserName -value $localuserName
    Set-ItemProperty -path $regKey -name DefaultDomainName -value ""
    Set-ItemProperty -path $regKey -name DefaultPassword -value $localpassword
  }
  return $true
}
