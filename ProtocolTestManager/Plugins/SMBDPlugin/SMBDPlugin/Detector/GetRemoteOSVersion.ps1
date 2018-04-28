# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to get the OS version of SUT computer
# Return Value: OS version string or $null if failed

$pwdConverted = ConvertTo-SecureString $ptfpropSUTUserPassword -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential "$ptfpropDomainName\$ptfpropSUTUserName", $pwdConverted -ErrorAction Stop

$scriptblock = {
	try {
	$osInfo = Get-WmiObject -Class Win32_OperatingSystem
	return $osInfo.Version
	}
	catch {
		return $null
	}
}

try{
	$cmdOutput = Invoke-Command -ComputerName $ptfpropSUTName -credential $cred -ScriptBlock $scriptblock

	return $cmdOutput  #operation succeed
}
catch{
	return $null # operation failed
}