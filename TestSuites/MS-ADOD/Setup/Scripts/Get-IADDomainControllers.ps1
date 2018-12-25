#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Get-IADDomainControllers.ps1
## Purpose:        Get the list of domain controllers' names.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################
 
function Get-IADDomainControllers {
  param (
    [string]$DomainName = $(Throw "Please enter a full qualified domain name."),
    [string]$UserName = $(Throw "Please enter a valid username."),
    [string]$Password = $(Throw "Please enter a password.")
  )

  $DirectoryContext = New-Object System.DirectoryServices.ActiveDirectory.DirectoryContext("domain", $DomainName, $Username, $Password)
  $DCList = [System.DirectoryServices.ActiveDirectory.Domain]::GetDomain($DirectoryContext).DomainControllers

  if($DCList.Count -eq 0) {
    echo "Unable to find a domain controller for $DomainName domain. Please check whether the domain name is correct." | out-file "$env:HOMEDRIVE\MS-ADOD.log.txt" -append
    return $null
  }

  Foreach ($DC in $DCList) {
    if($DC.Partitions.Count -eq 0) {
      echo "No Objects in $DC server" | out-file "$env:HOMEDRIVE\MS-ADOD.log.txt" -append
      continue
    }
    $Root = Get-IADSearchRoot -ServerName $DC -ObjectDN $DC.Partitions[0] -UserName $UserName -Password $Password
    if($Root -eq $null) {
      echo "DomainController $DC can not be connected or does not exist." | out-file "$env:HOMEDRIVE\MS-ADOD.log.txt" -append
    }
  }
  return $DCList
}