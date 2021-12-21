# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

New-Item -Force -ItemType directory -Path c:\temp\
$strFileName="c:\temp\changednetlogonservicestatus.txt"
"DONE" >> $strFileName
$computerName = $PTFProp_Common_WritableDC1_NetbiosName

	##get service object
	Invoke-Command -Computername $computerName -Scriptblock { $serviceObj = Get-Service Netlogon
		##stop.
		Stop-Service -inputObject $serviceObj
		Sleep 10
		$serviceObj =Invoke-Command -Computername $computerName -Scriptblock {Get-Service Netlogon} 
		if($serviceObj.Status -ne "Stopped")
		{
			throw "service cannot start"
		}
		Sleep 10
	}

	[System.GC]::Collect();
	[System.GC]::WaitForPendingFinalizers();
	[System.GC]::Collect();
