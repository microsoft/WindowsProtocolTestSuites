# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This method is used to trigger RDP client to close all RDP connection to server for clean up.

# Run task to simulate a client initiated disconnect request

$userPwdInTCEn = ConvertTo-SecureString $ptfprop_SUTUserPassword -AsPlainText -Force
$Credential = New-Object System.Management.Automation.PSCredential($ptfprop_SUTUserName,$userPwdInTCEn)

$sessionM = $null;

try
{
	$sessionM = New-PSSession -ComputerName $ptfprop_SUTName -Credential $Credential -ErrorAction SilentlyContinue
}
catch
{
}

try
{
	if ($sessionM -eq $null) {
		$result = Invoke-Command -HostName $PtfProp_SUTName -UserName $ptfprop_SUTUserName -ScriptBlock {taskkill /F /IM mstsc.exe}
	}
	else
	{
		$result = Invoke-Command -Session $sessionM -ScriptBlock {taskkill /F /IM mstsc.exe}

		Remove-PSSession $sessionM
	}
	
	return 0
}
catch
{
	return -1
}
