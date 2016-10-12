######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$ServerComputerName = $remoteComputerName,
[string]$service = $serviceName,
[string]$usr = $user,
[string]$pwd = $password
)

try
{
	## Get service object
	$serviceObj = get-service -computername $ServerComputerName $service

        ## Start service
	Restart-Service -inputObject $serviceObj 

	While($serviceObj.Status -ne "Running")
	{
		$serviceObj = get-service -computername $ServerComputerName $service
	}
}
catch
{
	"Start $service service failed!"
}
finally
{
	$serviceObj.Close()

	[System.GC]::Collect();
	[System.GC]::WaitForPendingFinalizers();
	[System.GC]::Collect();
}

return $true