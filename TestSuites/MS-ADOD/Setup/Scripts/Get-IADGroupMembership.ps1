#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Get-IADGroupMembership.ps1
## Purpose:        Get all group memberships of a specified account.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################

filter Get-IADGroupMembership {

  if($_ -is [ADSI] -and $_.MemberOf) {
    return $_.MemberOf
  }
}