#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

# [MethodHelp(@"This method is used to restore the Netlogon service on the PDC and TDC")]
#        void RestoreNetlogonService(string pdcName, string tdcName);
#
##get service object
$serviceObj = get-service -computername $pdcName Netlogon
##Judge the status of service.
if($serviceObj.Status -eq "Paused")
{
    Resume-Service -inputObject $serviceObj
}
if($serviceObj.Status -eq "Stopped")
{
    Start-Service -inputObject $serviceObj
}
Sleep 10
do
{
	Sleep 5
	$serviceObj = get-service -computername $pdcName Netlogon
}While($serviceObj.Status -ne "Running")
Sleep 10
$serviceObj.Close()
[System.GC]::Collect();
[System.GC]::WaitForPendingFinalizers();
[System.GC]::Collect();

if($tdcName)
{
	##get service object
	$serviceObj = get-service -computername $tdcName Netlogon
	##Judge the status of service.
	if($serviceObj.Status -eq "Paused")
	{
		Resume-Service -inputObject $serviceObj
	}
	if($serviceObj.Status -eq "Stopped")
	{
		Start-Service -inputObject $serviceObj
	}	
	sleep 10
	do{
		Sleep 5
		$serviceObj = get-service -computername $tdcName Netlogon
	}While($serviceObj.Status -ne "Running")
	Sleep 10

	$serviceObj.Close()

	[System.GC]::Collect();
	[System.GC]::WaitForPendingFinalizers();
	[System.GC]::Collect();
}
New-Item -Force -ItemType directory -Path c:\temp\
$strFileName="c:\temp\changednetlogonservicestatus.txt"
If (Test-Path $strFileName)
{
    Remove-Item $strFileName
}