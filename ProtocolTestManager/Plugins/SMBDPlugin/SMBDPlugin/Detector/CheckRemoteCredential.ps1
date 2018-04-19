# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to test the credential of SUT computer
# Return Value: 0/-1 indicates OK/NG

$pwdConverted = ConvertTo-SecureString $ptfpropSUTUserPassword -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential "$ptfpropDomainName\$ptfpropSUTUserName", $pwdConverted -ErrorAction Stop

$testValue = "1234567890!@#$%^&*()"
$scriptblock = {
	param($test)
	Write-Output $test
	}

try{
	$cmdOutput = Invoke-Command -ComputerName $ptfpropSUTName -credential $cred -ScriptBlock $scriptblock -ArgumentList $testValue

	if($cmdOutput -ne $testValue)
	{
		return -1 # operation failed
	}

	return 0  #operation succeed
}
catch{
	return -1 # operation failed
}