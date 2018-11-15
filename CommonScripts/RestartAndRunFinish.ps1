#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-----------------------------------------------------------------------------
# Function: RestartAndRunFinish
# Usage   : Call this script to clean up the registry entry after calling 
#           RestartAndRun.ps1.
# Remark  : This script should be called at the end of your script, if you 
#           have ever called RestartAndRun.ps1.
#-----------------------------------------------------------------------------

$private:regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
$private:regKeyName = "TKFRSAR"

if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
{
	Remove-ItemProperty -Path $regRunPath -Name $regKeyName
}
