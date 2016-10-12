######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

#----------------------------------------------------------------------------
# Set commandline paramter value.
#----------------------------------------------------------------------------
param(
[string]$ServerComputerName = $sutName,
[string]$usr = $userName,
[string]$pwd = $password,
[string]$scriptPath = $sutScriptPath
)

#----------------------------------------------------------------------------
# Remote execute the powershell script.
#----------------------------------------------------------------------------
.\RemoteExecuteCommand.ps1 $ServerComputerName "cmd /c powershell $sutScriptPath" $usr $pwd
sleep 2
$returnVal = Get-Content "\\$ServerComputerName\MS-PCCRTP\IsFinished.txt"

If ($returnVal -eq "True")
{
	return $true
}
else
{
	return $false
}