#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

New-Item -Force -ItemType directory -Path c:\temp\
$strFileName="c:\temp\changednetlogonservicestatus.txt"
"DONE" >> $strFileName
$computerName = $PTFProp_Common_WritableDC1_NetbiosName

	##get service object
	$serviceObj = get-service -computername $computerName Netlogon
	##stop.
	Stop-Service -inputObject $serviceObj
	Sleep 10
	$serviceObj = get-service -computername $computerName Netlogon
	if($serviceObj.Status -ne "Stopped")
	{
		throw "service cannot start"
	}
	Sleep 10
	$serviceObj.Close()

	[System.GC]::Collect();
	[System.GC]::WaitForPendingFinalizers();
	[System.GC]::Collect();
