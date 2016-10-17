######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$computerName = $computerName,
[string]$service = $serviceName,
[string]$usr = $user,
[string]$pwd = $password
)

try
{
	## Get service object
	$serviceObj = get-service -computername $computerName $service
        if ($serviceObj.Status -ne "Running")
	{
		## Start service
		Start-Service -inputObject $serviceObj 
	}

	While($serviceObj.Status -ne "Running")
	{
		Start-Service -inputObject $serviceObj
	    sleep 1
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
