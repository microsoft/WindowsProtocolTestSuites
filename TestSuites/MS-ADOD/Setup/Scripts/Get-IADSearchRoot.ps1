#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Get-IADSearchRoot.ps1
## Purpose:        Get the search root of an Active Directory in a specified DC (Server) according to the object DN provided.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7, Windows 8
##           
##############################################################################

function Get-IADSearchRoot {
  param (
    [string]$ServerName = $(Throw "Please enter a Domain Controller Server Name."),
    [string]$ObjectDN = $(Throw "Please enter a Object Distinguished Name."),
    [string]$UserName = $(Throw "Please enter a valid username."),
    [string]$Password = $(Throw "Please enter a valid password.")
  )

  $Root = New-Object DirectoryServices.DirectoryEntry("LDAP://$ServerName/$ObjectDN", $UserName, $Password)
  $Searcher = New-Object DirectoryServices.DirectorySearcher
  $Searcher.SearchRoot = $Root
  $AdObj=$Searcher.FindAll()

  return $Root
}