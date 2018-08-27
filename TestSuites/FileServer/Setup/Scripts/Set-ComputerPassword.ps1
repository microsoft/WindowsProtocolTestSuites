#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
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
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Setup ComputerPassword
#----------------------------------------------------------------------------
Write-Info.ps1 "Setup ComputerPassword for Kerberos authentication."
ksetup /SetComputerPassword Password04!

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed set computer password."
Stop-Transcript
exit 0