#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Get-IADGroup.ps1
## Purpose:        Retrieve all groups in a domain or container that match the specified conditions.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################
 
function Get-IADGroup {
  param(
    [string]$Name = "*",
    [DirectoryServices.DirectoryEntry]$SearchRoot = $(Throw "Must input a SearchRoot for directory Search."),
    [int]$PageSize = 1000,
    [int]$SizeLimit = 0,
    [string]$SearchScope = "SubTree",
    [string]$GroupScope,
    [string]$GroupType
  )

  if($SearchScope -notmatch '^(Base|OneLevel|Subtree)$') {
    Throw "SearchScope Value must be one of: 'Base','OneLevel or 'Subtree'"
  }

  if($GroupScope) {
    if($GroupScope -notmatch '^(Universal|Global|DomainLocal)$') {
      Throw "GroupScope Value must be one of: 'Universal', 'Global' or 'DomainLocal'"
    }
  }

  if($GroupType) {
    if($GroupType -notmatch '^(Security|Distribution)$') {
      Throw "GroupType Value must be one of: 'Security' or 'Distribution'"
    }
  }

  $resolve = "(|(sAMAccountName=$Name)(cn=$Name)(name=$Name))"

  $parameters = $GroupScope,$GroupType
  switch (,$parameters)
  {
    @('Universal','Distribution') {$filter = "(&(objectcategory=group)(sAMAccountType=268435457)(grouptype:1.2.840.113556.1.4.804:=8)$resolve)"}
    @('Universal','Security') {$filter = "(&(objectcategory=group)(sAMAccountType=268435456)(grouptype:1.2.840.113556.1.4.804:=-2147483640)$resolve)"}
    @('Global','Distribution') {$filter = "(&(objectcategory=group)(sAMAccountType=268435457)(grouptype:1.2.840.113556.1.4.804:=2)$resolve)"}
    @('Global','Security') {$filter = "(&(objectcategory=group)(sAMAccountType=268435456)(grouptype:1.2.840.113556.1.4.803:=-2147483646)$resolve)"}
    @('DomainLocal','Distribution') {$filter = "(&(objectcategory=group)(sAMAccountType=536870913)(grouptype:1.2.840.113556.1.4.804:=4)$resolve)"}
    @('DomainLocal','Security') {$filter = "(&(objectcategory=group)(sAMAccountType=536870912)(grouptype:1.2.840.113556.1.4.804:=-2147483644)$resolve)"}
    @('Global','') {$filter = "(&(objectcategory=group)(grouptype:1.2.840.113556.1.4.804:=2)$resolve)"}
    @('DomainLocal','') {$filter = "(&(objectcategory=group)(grouptype:1.2.840.113556.1.4.804:=4)$resolve)"}
    @('Universal','') {$filter = "(&(objectcategory=group)(grouptype:1.2.840.113556.1.4.804:=8)$resolve)"}
    @('','Distribution') {$filter = "(&(objectCategory=group)(!groupType:1.2.840.113556.1.4.803:=2147483648)$resolve)"}
    @('','Security') {$filter = "(&(objectcategory=group)(groupType:1.2.840.113556.1.4.803:=2147483648)$resolve)"}
    default {$filter = "(&(objectcategory=group)$resolve)"}
  }

  $searcher = New-Object System.DirectoryServices.DirectorySearcher $filter 
  $searcher.SearchRoot = $SearchRoot
  $searcher.SearchScope = $SearchScope
  $searcher.SizeLimit = $SizeLimit
  $searcher.PageSize = $PageSize
  $searcher.FindAll() | Foreach-Object { $_.GetDirectoryEntry() }
}