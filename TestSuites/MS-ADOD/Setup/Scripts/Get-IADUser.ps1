#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Get-IADSearchRoot.ps1
## Purpose:        Get all users in a domain or container with specific conditions.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################

function Get-IADUser {
  param(
    [string]$Name = "*",
    [DirectoryServices.DirectoryEntry]$SearchRoot = $(Throw "Must input a SearchRoot for directory Search."),
    [int]$PageSize = 1000,
    [int]$SizeLimit = 0,
    [string]$SearchScope = "SubTree",
    [switch]$Enabled,
    [switch]$Disabled,
    [switch]$AccountNeverExpires,
    [switch]$PasswordNeverExpires
  )

  if($SearchScope -notmatch '^(Base|OneLevel|Subtree)$') {
    Throw "SearchScope value must be one of: 'Base', 'OneLevel', 'Subtree'"
  }

  $resolve = "(|(sAMAccountName=$Name)(cn=$Name)(displayName=$Name)(givenName=$Name))"
  
  $EnabledDisabledf = ""
  if($Enabled) {
    $EnabledDisabledf = "(!userAccountControl:1.2.840.113556.1.4.803:=2)"
  }
  if($Disabled) {
    $EnabledDisabledf = "(userAccountControl:1.2.840.113556.1.4.803:=2)"
  }
  if($Enabled -and $Disabled) {
    $EnabledDisabledf = ""
  }

  if($AccountNeverExpires) {
    $AccountNeverExpiresf = "(|(accountExpires=9223372036854775807)(accountExpires=0))"
  }
  else {
    $AccountNeverExpiresf = ""
  }

  if($PasswordNeverExpires) {
    $PasswordNeverExpiresf = "(userAccountControl:1.2.840.113556.1.4.803:=65536)"
  }
  else {
    $PasswordNeverExpiresf = ""
  }

  $filter = "(&(objectCategory=Person)(objectClass=User)$EnabledDisabledf$AccountNeverExpiresf$PasswordNeverExpiresf$resolve)"
  $searcher = New-Object System.DirectoryServices.DirectorySearcher $filter
  $searcher.SearchRoot = $SearchRoot
  $searcher.SearchScope = $SearchScope
  $searcher.SizeLimit = $SizeLimit
  $searcher.PageSize = $PageSize
  $searcher.FindAll() | Foreach-Object {$_.GetDirectoryEntry()}
}