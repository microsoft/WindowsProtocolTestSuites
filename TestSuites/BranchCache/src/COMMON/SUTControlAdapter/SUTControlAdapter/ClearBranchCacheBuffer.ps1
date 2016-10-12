######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$ServerComputerName = $computerName,
[string]$usr = $user,
[string]$pwd = $password
)
	
	$serviceObj = get-service -computername $ServerComputerName PeerDistSvc
	Stop-Service -inputObject $serviceObj -force

	$CommandLine = "netsh branchcache flush"
	.\RemoteExecuteCommand.ps1 $ServerComputerName "$CommandLine" $usr $pwd
	sleep 1

	#del /s /q /f C:\Windows\ServiceProfiles\NetworkService\AppData\Local\PeerDistRepub\*.* 
	
	While($serviceObj.Status -ne "Running")
	{
		Start-Service -inputObject $serviceObj
		sleep 1
	}

return $true
