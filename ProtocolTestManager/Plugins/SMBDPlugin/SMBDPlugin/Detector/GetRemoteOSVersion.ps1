# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to get the OS version of SUT computer
# Return Value: OS version string or $null if failed

$pwdConverted = ConvertTo-SecureString $ptfprop_SUTUserPassword -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential "$ptfprop_DomainName\$ptfprop_SUTUserName", $pwdConverted -ErrorAction Stop

$scriptblock = {
	try {
		$osInfo = Get-WmiObject -Class Win32_OperatingSystem
		$result = "" | Select-Object -Property Caption, Version
		$result.Caption = $osInfo.Caption
		$result.Version = $osInfo.Version
		return $result
	}
	catch {
		return $null
	}
}

try{
	$cmdOutput = Invoke-Command -ComputerName $ptfprop_SUTName -credential $cred -ScriptBlock $scriptblock

	return $cmdOutput  #operation succeed
}
catch{
	return $null # operation failed
}