#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

function GetPtfVariable
{
    param($name)
	$v = Get-Variable -Name ("PTFProp"+$name)
	return $v.Value
}


New-Item -Force -ItemType directory -Path c:\temp\
$strFileName="c:\temp\changednetlogonservicestatus.txt"
"DONE" >> $strFileName

if($sutType -eq "PrimaryDc")
{
    $computerName = GetPtfVariable "Common.WritableDC1.NetbiosName"
}
if($sutType -eq "TrustDc")
{
    $computerName = GetPtfVariable "Common.TDC.NetbiosName"
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


