#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Get-IADGroupMember.ps1
## Purpose:        Get all group members of a specified group.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################

filter Get-IADGroupMember {

  if($_ -is [ADSI] -and $_.psbase.SchemaClassName -eq 'group') {
    return $_.member
  }
}