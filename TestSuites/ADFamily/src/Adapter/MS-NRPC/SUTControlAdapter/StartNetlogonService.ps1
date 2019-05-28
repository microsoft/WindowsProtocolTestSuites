#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$computerName = $PTFProp_Common_WritableDC1_NetbiosName


#try
#{
	##get service object
	$serviceObj = get-service -computername $computerName Netlogon
	##restart.
	Start-Service -inputObject $serviceObj -force
	Sleep 10
	$serviceObj = get-service -computername $computerName Netlogon
	if($serviceObj.Status -ne "Running")
	{
		throw "service cannot start"
	}
	Sleep 10
#}
#catch
#{
#	"Start Netlogon Service failed!"
#}
#finally
#{
	$serviceObj.Close()

	[System.GC]::Collect();
	[System.GC]::WaitForPendingFinalizers();
	[System.GC]::Collect();
#}

New-Item -Force -ItemType directory -Path c:\temp\
$strFileName="c:\temp\changednetlogonservicestatus.txt"
If (Test-Path $strFileName)
{
    Remove-Item $strFileName
}