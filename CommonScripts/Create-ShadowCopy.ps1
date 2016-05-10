#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Create-ShadowCopy.ps1
## Purpose:        Create a shadow copy.
## Version:        1.1 (26 Jun, 2008)
##
##############################################################################

param(
[string]$volumn,
[string]$maxRetryMinutes = 2
)

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Create-ShadowCopy.ps1] ..." -foregroundcolor cyan
Write-Host "`$volumn = $volumn"
Write-Host "`$maxRetryMinutes = $maxRetryMinutes"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: Accept the certificate issued by CA."
    Write-host "Parm1: Volunm info. (Required)"
    Write-host "Parm2: Max retry minutes. (Optional)"
    Write-host
    Write-host "Example: .\Create-ShadowCopy.ps1 e: 3"
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
if ($volumn -eq $null -or $volumn -eq "")
{
    Throw "Parameter `$volumn is required."
}

#----------------------------------------------------------------------------
# Accept the certificate
#----------------------------------------------------------------------------
#If there is another process creating a shadow copy, vssadmin will continue to try to create the shadow copy for MaxRetryMinutes minutes.
vssadmin.exe Create Shadow /For=$volumn /AutoRetry=$maxRetryMinutes 2>&1 | Write-Host

#----------------------------------------------------------------------------
# Print exit information
#----------------------------------------------------------------------------
Write-Host "Verifying [Create-ShadowCopy.ps1] ..." -foregroundcolor Yellow
Write-Host "EXECUTE [Create-ShadowCopy.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor Yellow

