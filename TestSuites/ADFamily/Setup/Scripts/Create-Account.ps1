#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Create-Account.ps1
## Purpose:        Create a new user account (Domain user or Local user).
## Version:        1.1 (26 June, 2008)
##       
##############################################################################

Param (
$strUserName, 
$strPassword, 
$accountType
)

Write-Host "EXECUTING [Create-Account.ps1]..." -foregroundcolor cyan
Write-Host "`$strUserName = $strUserName"
Write-Host "`$strPassword = $strPassword"
Write-Host "`$accountType = $accountType"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: This script creates a new user account.(Domain or Workgroup)"
    Write-host
    Write-host "First Parameter              `t:Account Name      : The name of the user account you want to create."
    Write-host "Second Parameter             `t:Account Password  : The password of the user."
    Write-host "Third parameter              `t:Account Type   : The type of the user account. (Domain or Workgroup)" 
    Write-host
    Write-host "Example: Create-Account.ps1 user1 Password01! Domain"
    Write-host
}

#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# Check Parameter accountType
#----------------------------------------------------------------------------
function accountTypeCheck($accountType)
{
    if (($accountType -eq "Domain") -or ($accountType -eq "Workgroup"))
    {
        return $True
    }
    else
    {
        Throw "Parameter accountType is unlegal, only Domain or Workgroup is accredited."
    }
}

#----------------------------------------------------------------------------
# Verify required parameters
#----------------------------------------------------------------------------
if ($strUserName -eq $null -or $strUserName -eq "")
{
    Throw "Parameter strUserName is required."
}
if ($strPassword -eq $null -or $strPassword -eq "")
{
    Throw "Parameter strPassword is required."
}
if ($accountType -eq $null -or $accountType -eq "")
{
    Throw "Parameter accountType is required."
}

#----------------------------------------------------------------------------
#Function: ExecuteLocalCommand
#
#Usage   : Execute command on cmd console
#----------------------------------------------------------------------------

function ExecuteLocalCommand([string] $command)
{
    cmd.exe /c $command 2>&1 | Write-Host
}

if ($accountType -eq "Domain")
{
    $strCommandAddGroup = "net.exe user $strUserName $strPassword /ADD /DOMAIN"
}
elseif ($accountType -eq "Workgroup")
{
    $strCommandAddGroup = "net.exe user $strUserName $strPassword /ADD"
}
else
{
    Throw "Unsupported account type:$accountType. The value must be Domain/Workgroup."
}

ExecuteLocalCommand($strCommandAddGroup)

#if (ExecuteLocalCommand($strCommandAddGroup) -eq "The command completed successfully.")
#{
#    Write-Host "Create domain user account successfully." -foregroundcolor Green
#}
#else
#{
#    Throw "Create domain user account failed."
#}

Write-Host "EXECUTE [Create-Account.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor yellow

return 0
