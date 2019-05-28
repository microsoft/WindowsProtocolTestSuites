#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

New-Item -Force -ItemType directory -Path c:\temp\
$strFileName="c:\temp\changednetlogonservicestatus.txt"
"DONE" >> $strFileName

if($sutType -eq "PrimaryDc")
{
    $computerName = $PTFProp_Common_WritableDC1_NetbiosName
}
if($sutType -eq "TrustDc")
{
    $computerName = $PTFProp_Common_TDC_NetbiosName
}


##get service object
$serviceObj = get-service -computername $computerName Netlogon
##pause service.
Suspend-Service -inputObject $serviceObj
	
do{
	Sleep 5
	$serviceObj = get-service -computername $computerName Netlogon
}While($serviceObj.Status -ne "Paused")
Sleep 10

$serviceObj.Close()
[System.GC]::Collect();
[System.GC]::WaitForPendingFinalizers();
[System.GC]::Collect();


