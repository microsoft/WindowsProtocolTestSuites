#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Get-IADComputer.ps1
## Purpose:        Get all computer objects in a domain according to the options specified.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7, Windows 8
##           
##############################################################################

function Get-IADComputer {
  param (
    [string]$Name = "*",
    [DirectoryServices.DirectoryEntry]$SearchRoot = $(Throw "Must input a SearchRoot for directory Search."),
    [int]$PageSize = 1000,
    [int]$SizeLimit = 0,
    [string]$SearchScope = "SubTree",
    [switch]$Enabled,
    [switch]$Disabled
  )

  if($SearchScope -notmatch '^(Base|OneLevel|Subtree)$') {
    Throw "SearchScope value must be one of: 'Base', 'OneLevel', 'Subtree'"
  }

  $resolve = "(|(sAMAccountName=$Name)(cn=$Name)(displayName=$Name)(dNSHostName=$Name)(name=$Name))"
  
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

  $filter = "(&(objectCategory=Computer)(objectClass=User)$EnabledDisabledf$resolve)"

  $searcher = New-Object System.DirectoryServices.DirectorySearcher $filter
  $searcher.SearchRoot = $SearchRoot
  $searcher.SearchScope = $SearchScope
  $searcher.SizeLimit = $SizeLimit
  $searcher.PageSize = $PageSize
  $searcher.FindAll() | Foreach-Object {$_.GetDirectoryEntry()}

}