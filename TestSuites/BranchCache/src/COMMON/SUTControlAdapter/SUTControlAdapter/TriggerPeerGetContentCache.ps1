######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$ServerComputerName = $peerServerName,
[string]$ContentServerName = $contentServerName,
[string]$path = $fileName,
[string]$usr = $user,
[string]$pwd = $password
)

#region ClearSUTCache
function ClearSUTCache(
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

#region OpenIexplore
function OpenIexplore(
	$ServerComputerName,
	$ContentServerName,
	$path,
	$usr,
	$pwd
)
{
    $iepath= "c:\Program files\Internet explorer\iexplore.exe"
    $CommandLine = "cmd /c echo `"$iepath`" http://" + $ContentServerName + "/" + $path +">c:\Welcome.bat" 
	.\RemoteExecuteCommand.ps1 $ServerComputerName $CommandLine $usr $pwd
	sleep 1
	$commandLine = "cmd /c c:\Welcome.bat"
    .\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	sleep 2
}
#endregion

ClearSUTCache $ServerComputerName $usr $pwd
OpenIexplore $ServerComputerName $ContentServerName $path $usr $pwd

return $true