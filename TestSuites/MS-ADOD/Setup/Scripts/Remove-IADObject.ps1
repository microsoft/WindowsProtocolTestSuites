#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Remove-IADObject.ps1
## Purpose:        Remove the specified object(s) in Active Directory.
## Version:        1.0 (8 Feb, 2012)
## Requirements:   Windows PowerShell 2.0 CTP
## Supported OS:   Windows 7 or later versions
##           
##############################################################################

filter Remove-IADObject {
  param (
    [switch]$confirm
  )

  $object = $_
  if($object -is [ADSI]) {
    if($confirm) {
      $caption = "Confirm"
      $message = "Are you sure you want to perform this action?`nPerforming operation 'Remove AD Object' on Target '$($_.distinguishedName)'"
      $yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", "Continue with only the next step of the operation.";
      $no = New-Object System.Management.Automation.Host.ChoiceDescription "&No", "Skip this operation and process with the next operation.";
      $choices = [System.Management.Automation.Host.ChoiceDescription[]]($yes, $no);
      $answer = $Host.ui.PromptForChoice($caption, $message, $choices, 0)

      switch ($answer)
      {
        0 {
          $object.DeleteTree()
          return $true
        }
        1 {
          return $false
        }
      }
    }
    $object.DeleteTree()
    return $true
  }
}