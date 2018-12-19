#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Add-DFSFolder.ps1
## Purpose:        Add a DFS folder.
## Version:        1.1 (26 Jun, 2008)
##
##############################################################################

param(
[Parameter(Mandatory=$true)]
[string]$nsPath,
[Parameter(Mandatory=$true)]
[string]$folderPath
)

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Add-DFSFolder.ps1] ..." -foregroundcolor cyan
Write-Host "`$nsPath = $nsPath"
Write-Host "`$folderPath = $folderPath"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: Add a DFS folder. "
    Write-host "Parm1: The NS path. (Required)"
    Write-host "Parm2: The folder path. (Required)"
    Write-host
    Write-host "Example: .\Add-DFSFolder.ps1 \\pt3test014\test\a \\pt3wtt004\test"
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
# Add DFS folder
#----------------------------------------------------------------------------
cmd.exe /c dfscmd /map $nsPath $folderPath /restore 2>&1 | Write-Host

#----------------------------------------------------------------------------
# Print exit information
#----------------------------------------------------------------------------
Write-Host "Verifying [Add-DFSFolder.ps1] ..." -foregroundcolor Yellow
Write-Host "EXECUTE [Add-DFSFolder.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor Yellow
