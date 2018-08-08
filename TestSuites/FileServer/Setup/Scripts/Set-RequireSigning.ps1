#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
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
# Set SMB2 RequireSecuritySignature
#----------------------------------------------------------------------------
Write-Info.ps1 "Set SMB2 RequireSecuritySignature to TRUE."
Set-SmbServerConfiguration -RequireSecuritySignature $true -Confirm:$false

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed set SMB2 RequireSecuritySignature."
Stop-Transcript
exit 0