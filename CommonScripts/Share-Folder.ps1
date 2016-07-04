#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Share-Folder.ps1
## Purpose:        Share a folder for everyone.
## Version:        1.1 (26 Jun, 2008)
##
##############################################################################

Param(
[String]$SharePath, 
[String]$ShareName
)

#----------------------------------------------------------------------------
# Print exection information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Share-Folder.ps1]..." -foregroundcolor cyan
Write-Host "`$SharePath = $SharePath" 
Write-Host "`$ShareName = $ShareName" 

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: This script will share a folder for everyone."
    Write-host
    Write-host "Example: Share-Folder.ps1  c:\share share"
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
if ($SharePath -eq $null -or $SharePath -eq "")
{
    Throw "Parameter `$SharePath is required."
}
if ($ShareName -eq $null -or $ShareName -eq "")
{
    Throw "Parameter `$ShareName is required."
}

#----------------------------------------------------------------------------
# Share the folder for everyone with the sharename
#----------------------------------------------------------------------------
write-host "net share $ShareName=$SharePath /grant:everyone,FULL"
cmd /c "net.exe share $ShareName=$SharePath /grant:everyone,FULL" 2>&1 | Write-Host

#----------------------------------------------------------------------------
# Print verification and exit information
#----------------------------------------------------------------------------
Write-Host "Verifying [Share-Folder.ps1]..." -foregroundcolor yellow
Write-Host "EXECUTE [Share-Folder.ps1] FINISHED(NOT VERIFIED)" -foregroundcolor yellow

exit

