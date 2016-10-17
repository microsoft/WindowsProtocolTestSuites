######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$remoteComputer = $computerName,
[string]$usr = $user,
[string]$pwd = $password
)

$CommandLine = "taskkill /f /im iexplore.exe"
.\RemoteExecuteCommand.ps1 $remoteComputer "$CommandLine" $usr $pwd
	start-sleep -s 2

#REM * Cleaning Current User's TIF Folder * 

#DEL /S /Q /F "%USERPROFILE%\AppData\Local\Microsoft\Windows\Temporary Internet Files\*.*" 

$CommandLine = "rundll32.exe inetcpl.cpl,ClearMyTracksByProcess 4351"
.\RemoteExecuteCommand.ps1 $remoteComputer "$CommandLine" $usr $pwd
start-sleep -s 2

return $true