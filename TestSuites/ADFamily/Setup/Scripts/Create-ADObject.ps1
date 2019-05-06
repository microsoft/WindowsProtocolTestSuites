#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Create-ADObject.ps1
## Purpose:        Create new object in an Active Directory Domain Controller
## Requirements:   Windows Powershell 2.0 
## Supported OS:   Windows Server 2008 R2 / Windows Server 8
##
##############################################################################


Param(
#DC IP address
[String]$ServerIP	= "",
#DC port, by default use 389 to connect DS
[int]$ServerPort	= 389,
#Domain name of Domain Admin
[String]$DomainName	= "",
#name of Domain Admin
[String]$AdminName	= "",
#password of Domain Admin
[String]$AdminPwd	= "",
#common name for new object
[String]$CommonName	= "",
#object class for new object
[STring]$Class		= "",
#parent path for new object
[String]$Path		= "",
#optional description for new object
[String]$Description	= "",
#optional display name for new object
[String]$DisplayName	= "",
#optional attributes for new object
[HashTable]$Attributes = @{}
)

$Err = 1
if($ServerIP -eq "" -or $CommonName -eq "" -or $Class -eq "" -or $Path -eq "")
{
  write-host "ServerIP, CommonName,Class,Path are all required parameters"
  return $Err
}

if($ServerPort -lt 1)
{
  write-host "ServerPort is invalid"
  return $Err
}

$remote = $ServerIP+":"+$ServerPort


Try
{
  if($AdminName -eq "")
  {
    if($att.count -eq 0)
    {
      new-adobject -name $CommonName -type $Class -path $Path -server $remote
    }
    else
    {
      new-adobject -name $CommonName -type $Class -path $Path -OtherAttributes $Attributes -server $remote
    }
  }
  else
  {
    if($AdminPwd -ne "")
    {
      $UPwd = $AdminPwd|ConvertTo-SecureString -asPlainText -Force
    }
    else
    {
      $UPwd = ""
    }
    $UName = $DomainName+"\"+$AdminName
    $UCredential = New-Object System.Management.Automation.PSCredential($UName,$UPwd)

    if($Attributes.count -eq 0)
    {
      new-adobject -name $CommonName -type $Class -path $Path -server $remote -credential $UCredential
    }
    else
    {
      new-adobject -name $CommonName -type $Class -path $Path -OtherAttributes $Attributes -server $remote -credential $UCredential
    }
  }
  $Err = 0
}
Catch
{
  write-host $_.Exception.message
}
return $Err
