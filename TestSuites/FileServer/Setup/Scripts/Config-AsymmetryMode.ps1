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
# Set AsymmetryMode
#----------------------------------------------------------------------------
$osVersion = Get-OSVersionNumber.ps1
if ([double]$osVersion -ge [double]"6.3")
{
    Write-Info.ps1 "Set AsymmetryMode for Windows Server 2012 R2 or later version."
    REG ADD HKLM\System\CurrentControlSet\Services\LanmanServer\Parameters /v AsymmetryMode /t REG_DWORD /d 2 /f
}
else
{
    Write-Info.ps1 "Do not set AsymmetryMode for Windows Server 2012 or lower version."
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Complete Config-AsymmetrModel.ps1"
Stop-Transcript
exit 0