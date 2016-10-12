######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$ServerComputerName = $SUTName,
[string]$serviceName = $Service,
[string]$ProcessName = $processName,
[string]$usr = $usrInVM,
[string]$pwd = $pwdInVM,
[string]$path = $path
)

#region EndTask
function EndTask(
	$ServerComputerName,
	$usr,
	$pwd
	)
{
	$CommandLine = "schtasks /end /s " + $ServerComputerName + " /tn Welcome"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	sleep 1
}
#endregion

#region EndRemoteProcess
function EndRemoteProcess(
	$ServerComputerName,
	$ProcessName,
	$usr,
	$pwd
	)
{
	$CommandLine = "taskkill /f /im " + $ProcessName
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	
}
#endregion

#region RestartRmoteComputerService
function RestartRmoteComputerService(
	$ServerComputerName,
	$serviceName,
	$usr,
	$pwd
	)
{
	$CommandLine = "net stop " + $serviceName  
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	sleep 3
	$CommandLine = "net start " + $serviceName  
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	sleep 2
}
#endregion

#region
function CleanCacheOnServer(
	$ServerComputerName,
	$usr,
	$pwd
	)
{
    $spath = $env:SystemDrive.replace(':', '$')
    $file = "\\" + $ServerComputerName + "\" + $spath + "\" + $path
    if(Test-Path $file)
    {
    	Remove-Item $file
    }
	sleep 2
	$CommandLine = "netsh branchcache flush"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	sleep 1
	$CommandLine = "bitsadmin /cache /clear"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	sleep 1
}
#endregion

EndTask $ServerComputerName $usr $pwd
EndRemoteProcess $ServerComputerName $ProcessName $usr $pwd
RestartRmoteComputerService $ServerComputerName $serviceName $usr $pwd
CleanCacheOnServer $ServerComputerName $usr $pwd

exit