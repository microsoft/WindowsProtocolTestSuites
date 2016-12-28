#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$retVal = $true
try
{
	$scriptPath = Split-Path $MyInvocation.MyCommand.Definition
	Push-Location $scriptPath
	$remoteServerName = .\ReadPtfConfig.ps1 'WritableDC1.NetbiosName'

	#----------------------------------------------------------------------------
	# Stop Active Directory Domain Control Service
	#----------------------------------------------------------------------------
	$serviceObj = get-service -computername $remoteServerName DNS
	Stop-Service -inputObject $serviceObj -force
	while ($serviceObj.Status -ne "Stopped")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName DNS
	}
	
	$serviceObj = get-service -computername $remoteServerName kdc
	Stop-Service -inputObject $serviceObj -force
	while ($serviceObj.Status -ne "Stopped")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName kdc
	}
	
	$serviceObj = get-service -computername $remoteServerName ismServ
	Stop-Service -inputObject $serviceObj -force
	while ($serviceObj.Status -ne "Stopped")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName ismServ
	}
	
	$serviceObj = get-service -computername $remoteServerName DFSR
	Stop-Service -inputObject $serviceObj -force
	while ($serviceObj.Status -ne "Stopped")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName DFSR
	}

	$serviceObj = get-service -computername $remoteServerName NTDS
	Stop-Service -inputObject $serviceObj -force
	while ($serviceObj.Status -ne "Stopped")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName NTDS
	}

	$serviceObj.Close()
	$retVal = $true
}
catch
{
	Write-Error "Exception in stop Active Directory Domain Control"
	$retVal = $false
}
return $retVal
