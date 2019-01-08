#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
try { Stop-Transcript -ErrorAction SilentlyContinue } catch {} # Ignore Stop-Transcript error messages
Start-Transcript -Path "$logFile" -Append -Force

#-----------------------------------------------------
# Begin to install BranchCache feature
#-----------------------------------------------------
Import-Module ServerManager

Write-Info.ps1 "Install BranchCache Feature"
Add-Windowsfeature BranchCache -IncludeAllSubFeature -IncludeManagementTools

#-----------------------------------------------------
# Begin to config HostedCacheServer
#-----------------------------------------------------
Write-Info.ps1 "Enable BCHostedCacheServer"
Enable-BCHostedServer

Write-Info.ps1 "Restart service: peerdistsvc"
Restart-service peerdistsvc

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed config hosted cache server."
Stop-Transcript
exit 0
