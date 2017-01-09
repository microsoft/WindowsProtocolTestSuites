#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

function GetPtfVariable
{
    param($name)
	$v = Get-Variable -Name ("PTFProp"+$name)
	return $v.Value
}

$PDCName = GetPtfVariable "Common.WritableDC1.NetbiosName"
$TDCName = GetPtfVariable "Common.TDC.NetbiosName"

try
{
	$PDCRemoteKey = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey("LocalMachine", $PDCName)
	$PDCNetlogonParam = $PDCRemoteKey.OpenSubKey("SYSTEM\CurrentControlSet\services\Netlogon\Parameters", $true)	
	$PDCNetlogonParam.SetValue("RestrictNTLMInDomain", 0, [Microsoft.Win32.RegistryValueKind]::DWord)
	$PDCNetlogonParam.Flush()
	$TDCRemoteKey = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey("LocalMachine", $TDCName)
	$TDCNetlogonParam = $TDCRemoteKey.OpenSubKey("SYSTEM\CurrentControlSet\services\Netlogon\Parameters", $true)	
	$TDCNetlogonParam.SetValue("RestrictNTLMInDomain", 0, [Microsoft.Win32.RegistryValueKind]::DWord)
	$TDCNetlogonParam.Flush()
}
catch
{
	"SUT control set exception value failed!"
}
finally
{
	$PDCNetlogonParam.Close()
	$PDCRemoteKey.Close()
	$TDCNetlogonParam.Close()
	$TDCRemoteKey.Close()
	sleep(2)
}
