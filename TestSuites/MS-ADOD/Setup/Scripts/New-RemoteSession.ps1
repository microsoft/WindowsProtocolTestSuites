#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           New-RemoteSession.ps1
## Purpose:        This script creates a new PowerShell session on a remote machine for interactive purpose.
## Scenarios:      1) The server must have Windows Remote Management Server setup already
##                 2) The Network should be set as "Private" Network
## Version:        1.0 (15 Feb, 2012)
##
##############################################################################

Param(
[string]$computerName,
[string]$userName,         # "Administrator"
[string]$password          # "Password01!"
)

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [New-RemoteSession.ps1]." -foregroundcolor cyan
Write-Host "`$computerName   = $computerName"
Write-Host "`$userName       = $userName"
Write-Host "`$password       = $password"


#----------------------------------------------------------------------------
#Function: Show-ScriptUsage
#Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: This script creates a new PowerShell session on a remote machine for interactive purpose."
    Write-host
    Write-host "Example: New-RemoteSession.ps1 SUT01 username password"
    Write-host
}

#----------------------------------------------------------------------------
# Verify Required parameters
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# Validate parameter
#----------------------------------------------------------------------------
if ($computerName -eq $null -or $computerName -eq "")
{
    Throw "Parameter computerName is required."
}

#----------------------------------------------------------------------------
# Using default username/password when caller doesnot provide.
#----------------------------------------------------------------------------
if ($userName -eq $null -or $userName -eq "")
{
    $userName = "$computerName\Administrator"
    $password = "Password01!"
}

#----------------------------------------------------------------------------
# Create credentials object.
#----------------------------------------------------------------------------
$passwordConverted = ConvertTo-SecureString $password -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential $userName, $passwordConverted

#----------------------------------------------------------------------------
# Create new powershell session on the remote computer.
#----------------------------------------------------------------------------
$remoteSession = New-PSSession -ComputerName $computerName -Credential $cred

#----------------------------------------------------------------------------
# Print exit information. Show message of success.
#----------------------------------------------------------------------------
if($remoteSession -eq $null -or $remoteSession -eq "")
{
  Write-Host "Remote Session Setup failed." -foregroundcolor Red
  return $null
}
else
{
  Write-Host "Remote Session Setup successfully." -foregroundcolor Green
  return $remoteSession
}