#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

try
{
	$resourceDCRemoteKey = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey("LocalMachine", $targetDCName)
	$resourceDCSubKey = $resourceDCRemoteKey.OpenSubKey("SYSTEM\ControlSet001\services\Netlogon\Parameters", $true)
	$resourceDCSubKey.SetValue("RestrictNTLMInDomain", $blockKey)
	$resourceDCSubKey.Flush()
}
catch
{
	"SUT control set block value failed!"
}
finally
{
	$resourceDCSubKey.Close()
	$resourceDCRemoteKey.Close()
	sleep(2)
}

