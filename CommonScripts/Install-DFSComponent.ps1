#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Install-DFSComponent.ps1
## Purpose:        Install DFS component (just used in w2k8).
## Version:        1.1 (26 Jun, 2008)
##
##############################################################################

#----------------------------------------------------------------------------
# NO PARAM
#----------------------------------------------------------------------------

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Install-DFSComponent.ps1] ..." -foregroundcolor cyan

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: Install DFS component."
    Write-host
    Write-host "Example: .\Install-DFSComponent.ps1"
    Write-host
    Write-host "Note: Only used in Windows Server 2008."
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
# Installation
#----------------------------------------------------------------------------
servermanagercmd.exe -install FS-DFS 2>&1 | Write-Host

#----------------------------------------------------------------------------
# Print exit information
#----------------------------------------------------------------------------
Write-Host "Verifying [Install-DFSComponent.ps1] ..." -foregroundcolor Yellow
Write-Host "EXECUTE [Install-DFSComponent.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor Yellow

