#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           TurnOff-FileReadonly.ps1
## Purpose:        Turn off readonly attribute for a file.
## Version:        1.0 (25 May, 2008)
##
##############################################################################

param(
[string]$filePath
)

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [TurnOff-FileReadonly.ps1] ..." -foregroundcolor cyan
Write-Host "`$filePath = $filePath"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: Turn off file readonly."
    Write-host
    Write-host "Example: TurnOff-FileReadonly.ps1 `"C:\MyFile.txt`""
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
if ($filePath -eq $null -or $filePath -eq "")
{
    Throw "Parameter `$filePath is required."
}

#----------------------------------------------------------------------------
# Turn off file readonly
#----------------------------------------------------------------------------
if ((Test-Path $filePath) -eq $false)
{
    Throw "Error: Cannot get file `"$filePath`""
}
$fileObj = gci $filePath
$fileObj.IsReadOnly=$False

#----------------------------------------------------------------------------
# Print exit information
#----------------------------------------------------------------------------
Write-Host "EXECUTE [TurnOff-FileReadonly.ps1] SUCCEED." -foregroundcolor Green

