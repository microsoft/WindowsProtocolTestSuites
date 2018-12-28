#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Remove-IADGroupMember.ps1
## Purpose:        Remove a member or a list of members from a group in Active Directory.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################

filter Remove-IADGroupMember {
  param(
    [string[]]$MemberDN=$(Throw "MemberDN cannot be empty")
  )

  if($_ -is [ADSI] -and $_.psbase.SchemaClassName -eq 'group')
  {
    $group = $_

    $MemberDN | Where-Object {$_} | ForEach-Object { $group.Properties["member"].Remove($_) } 
    $group.psbase.CommitChanges()
  }
  else
  {
    Write-Warning "Wrong object type, only Group objects are allowed"
  }
}