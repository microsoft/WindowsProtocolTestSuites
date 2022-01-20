# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This method is used to trigger RDP client to close all RDP connection to server for clean up.

# Run task to simulate a client initiated disconnect request
try
{
	$result = Invoke-Command -HostName $PtfProp_SUTName -UserName $ptfprop_SUTUserName -ScriptBlock {taskkill /F /IM mstsc.exe}
	return 0
}
catch
{
	return -1
}