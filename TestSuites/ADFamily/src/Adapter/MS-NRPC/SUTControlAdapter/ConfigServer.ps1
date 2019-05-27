#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$computerName = $PTFProp_Common_WritableDC1_NetbiosName

try
{
	$resourceDCRemoteKey = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey("LocalMachine", $computerName)
	$resourceDCSubKey = $resourceDCRemoteKey.OpenSubKey("SYSTEM\CurrentControlSet\services\Netlogon\Parameters", $true)
	
	if($refusePasswordChange)
	{
		$resourceDCSubKey.SetValue("RefusePasswordChange", 1, [Microsoft.Win32.RegistryValueKind]::DWord)
	}
        else
	{
		$resourceDCSubKey.SetValue("RefusePasswordChange", 0, [Microsoft.Win32.RegistryValueKind]::DWord)
	}
	if($allowDes)
	{
		$resourceDCSubKey.SetValue("AllowNT4Crypto", 1, [Microsoft.Win32.RegistryValueKind]::DWord)
	}
        else
	{
		$resourceDCSubKey.SetValue("AllowNT4Crypto", 0, [Microsoft.Win32.RegistryValueKind]::DWord)
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
