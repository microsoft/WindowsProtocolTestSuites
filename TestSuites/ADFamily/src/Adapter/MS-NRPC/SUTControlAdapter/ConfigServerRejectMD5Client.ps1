#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$computerName = $PTFProp_Common_WritableDC1_NetbiosName


####################################
# Update register
####################################
try
{
	$resourceDCRemoteKey = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey("LocalMachine", $computerName)
	$resourceDCSubKey = $resourceDCRemoteKey.OpenSubKey("SYSTEM\CurrentControlSet\Services\Netlogon\Parameters", $true)
	if($rejectMD5Clients)
	{
		$resourceDCSubKey.SetValue("RejectMD5Clients", 1)
	}
        else
	{
		$resourceDCSubKey.SetValue("RejectMD5Clients", 0)
	}		
	$resourceDCSubKey.Flush()
}
catch
{
	"SUT control set exception value failed!"
}
finally
{
	$resourceDCSubKey.Close()
	$resourceDCRemoteKey.Close()
	sleep(2)
}


####################################
# Restart NetLogon Service
####################################
try
{
	##get service object
	$serviceObj = get-service -computername $computerName Netlogon
	##restart.
	Restart-Service -inputObject $serviceObj -force
}
catch
{
	"Restart Netlogon Service failed!"
}
finally
{
	$serviceObj.Close()

	[System.GC]::Collect();
	[System.GC]::WaitForPendingFinalizers();
	[System.GC]::Collect();
}
