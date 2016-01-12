#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Config-FileSharing.ps1
## Purpose:        Enable/Disable the FileSharing.
## Version:        1.1 (26 Jun, 2008)
##
##############################################################################

param(
[String]$FileSharing = "on"
)

#----------------------------------------------------------------------------
# Print exection information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Config-FileSharing.ps1]..." -foregroundcolor cyan
Write-Host "`$FileSharing = $FileSharing" 

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: This script will enable/disable the FileSharing."
    Write-host
    Write-host "Example: Config-FileSharing.ps1  on"
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
# Verify required parameters
#----------------------------------------------------------------------------
if ($FileSharing -eq $null -or $FileSharing -eq "")
{
    Throw "Parameter `$FileSharing is required."
}
if($FileSharing -ne "on" -and $FileSharing -ne "off")
{
    Throw "Parameter `$FileSharing SHOULD be 'on' or 'off'."
}

#----------------------------------------------------------------------------
# Enable/Disable the Fire sharing in the Network and Sharing Center
#----------------------------------------------------------------------------
$TurnOn = $true
if($FileSharing.ToLower().Equals("off"))
{
    $TurnOn = $false
}

$objFirewall = New-Object -ComObject "HNetCfg.Fwmgr"
$objPolicy = $objFirewall.LocalPolicy.CurrentProfile
$colServices = $objPolicy.Services
$objService = $colServices.Item(0)
if($objService.Enabled -ne $TurnOn)
{
    $objService.Enabled = $TurnOn
}

#----------------------------------------------------------------------------
# Print verification and exit information
#----------------------------------------------------------------------------
Write-Host "Verifying [Config-FileSharing.ps1]..." -foregroundcolor yellow
if($TurnOn)
{
    Write-Host "Successfully turned on Fire sharing" -ForegroundColor green
}
else
{
    Write-Host "Successfully turned off Fire sharing" -ForegroundColor green
}
Write-Host "EXECUTE [Config-FileSharing.ps1] SUCCEED." -foregroundcolor green
exit

