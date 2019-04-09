#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           New-IADGroup.ps1
## Purpose:        Create a new group account with the specified options.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################

function New-IADGroup {
  param(
    [string]$Name = $(Throw "Please enter a group name."),
    [DirectoryServices.DirectoryEntry]$SearchRoot = $(Throw "Must input a SearchRoot"),
    [DirectoryServices.DirectoryEntry]$ParentContainer = $(Throw "Must input a ParentContainer for directory object."),
    [string]$GroupScope,
    [string]$GroupType,
    [string]$Description
  )

  if($GroupType -ne "" -or $GroupType) {
    if($GroupType -notmatch '^(Security|Distribution)$') {
      Throw "GroupType value must be one of: 'Security' or 'Distribution'"
    }
  }

  if($GroupScope -ne "" -or $GroupScope) {
    if($GroupScope -notmatch '^(Universal|Global|DomainLocal)$') {
      Throw "GroupScope value must be one of: 'Universal', 'Global' or 'DomainLocal'"
    }
  }

  switch ($GroupScope) {
    "Global" {$GroupTypeAttr = 2}
    "DomainLocal" {$GroupTypeAttr = 4}
    "Universal" {$GroupTypeAttr = 8}
  }

  # modify group type attribute if the group is security enabled
  if ($GroupType -eq 'Security')
  {
    $GroupTypeAttr = $GroupTypeAttr -bor 0x80000000
  }

  $ExistName = Get-IADGroup -Name $Name -SearchRoot $SearchRoot
  if($ExistName -ne $null) {
    echo "Failed: Group with the name $Name already exists in the domain." | out-file "C:\MS-ADOD.log.txt" -append
    return $null
  }
  else
  {
    $NewGroup = $ParentContainer.Children.Add("CN="+$Name, "group")
    $NewGroup.InvokeSet("displayName", $Name)
    $NewGroup.InvokeSet("sAMAccountName",$Name)
    $NewGroup.InvokeSet("grouptype",$GroupTypeAttr)
    if ($Description) {
      $NewGroup.put("description",$Description)
    }

    $NewGroup.InvokeSet("info","Created $(Get-Date) by $env:userdomain\$env:username")
    $NewGroup.CommitChanges();
    return $NewGroup 
  }
}