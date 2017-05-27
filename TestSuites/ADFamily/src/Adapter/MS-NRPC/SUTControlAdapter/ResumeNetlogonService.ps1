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

$waitingTime = GetPtfVariable "MS_NRPC.Adapter.WaitingTime"

if($sutType -eq "PrimaryDc")
{
    $computerName = GetPtfVariable "Common.WritableDC1.NetbiosName"
}
if($sutType -eq "TrustDc")
{
    $computerName = GetPtfVariable "Common.TDC.NetbiosName"
}

#try
#{
	##get service object
	$serviceObj = get-service -computername $computerName Netlogon
	##Resume service.
	Resume-Service -inputObject $serviceObj
	##The waiting time is needed if NetrLogonControl2Ex Response or NetrLogonControl2 Response is ERROR_NO_LOGON_SERVERS.
	Start-Sleep -s $waitingTime
	Sleep 10
	do
        {
	    Sleep 5
	    $serviceObj = get-service -computername $computerName Netlogon
	}While($serviceObj.Status -ne "Running")
	Sleep 10
#}
#catch
#{
#	"Resume Netlogon Service failed!"
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