#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Create-DFSNamespace.ps1
## Purpose:        Create a DFS namespace and add a DFS folder.
## Version:        1.1 (26 Jun, 2008)
##
##############################################################################

param(
[string]$nsPath
)

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Create-DFSNamespace.ps1] ..." -foregroundcolor cyan
Write-Host "`$nsPath = $nsPath"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: Create a DFS namespace and add a DFS folder."
    Write-host "Parm1: The NS path. (Required)"
    Write-host
    Write-host "Example: .\Create-DFSNamespace.ps1 \\pt3test014\testr"
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
if ($nsPath -eq $null -or $nsPath -eq "")
{
    Throw "Parameter `$nsPath is required."
}

#----------------------------------------------------------------------------
# Create a DFS namespace and add a DFS folder
#----------------------------------------------------------------------------
cmd.exe /c dfsutil root addstd $nsPath 2>&1 | Write-Host

#----------------------------------------------------------------------------
# Print exit information
#----------------------------------------------------------------------------
Write-Host "Verifying [Create-DFSNamespace.ps1] ..." -foregroundcolor Yellow
Write-Host "EXECUTE [Create-DFSNamespace.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor Yellow
