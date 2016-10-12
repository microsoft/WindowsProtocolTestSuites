######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$ServerComputerName = $SUTName,
[string]$ContentServerName = $ContentComputerName,
[string]$path = $path,
[string]$usr = $usrInVM,
[string]$pwd = $pwdInVM
)

#region ClearSUTCatch
function ClearSUTCatch(
	$ServerComputerName,
	$usr,
	$pwd
)
{
	$CommandLine = "rundll32.exe inetcpl.cpl, ClearMyTracksByProcess 4351"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	$CommandLine = "netsh bra flush"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
}
#endregion

#region Openbitsadmin
function Openbitsadmin(
	$ServerComputerName,
	$ContentServerName,
	$path,
	$usr,
	$pwd
)
{
	$CommandLine = "cmd /c Echo bitsadmin /transfer abc http://" + $ContentServerName + "/" + $path + " $env:SystemDrive\" + $path + ">$env:SystemDrive\Welcome.cmd"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	Start-Sleep 2
	$CommandLine = "schtasks /create /s " + $ServerComputerName + " /tn Welcome /tr $env:SystemDrive\Welcome.cmd /sc WEEKLY"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	Start-Sleep 2
	$CommandLine = "schtasks /run /tn Welcome /s $ServerComputerName"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
}
#endregion

ClearSUTCatch $ServerComputerName $usr $pwd
Openbitsadmin $ServerComputerName $ContentServerName $path $usr $pwd

exit