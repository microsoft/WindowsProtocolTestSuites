#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$retVal = $false
try
{
	$scriptPath = Split-Path $MyInvocation.MyCommand.Definition
	Push-Location $scriptPath
	$remoteServerName = .\ReadPtfConfig.ps1 'WritableDC1.NetbiosName'
	
	#----------------------------------------------------------------------------
	# Start Active Directory Domain Control Service
	#----------------------------------------------------------------------------
	$serviceObj = get-service -computername $remoteServerName NTDS
	Start-Service -inputObject $serviceObj
	while ($serviceObj.Status -ne "Running")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName NTDS
	}

    $serviceObj = get-service -computername $remoteServerName DFSR
	Start-Service -inputObject $serviceObj
	while ($serviceObj.Status -ne "Running")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName DFSR
	}

    $serviceObj = get-service -computername $remoteServerName ismServ
	Start-Service -inputObject $serviceObj
	while ($serviceObj.Status -ne "Running")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName ismServ
	}

    $serviceObj = get-service -computername $remoteServerName kdc
	Start-Service -inputObject $serviceObj
	while ($serviceObj.Status -ne "Running")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName kdc
	}

	$serviceObj = get-service -computername $remoteServerName DNS
	Start-Service -inputObject $serviceObj
	while ($serviceObj.Status -ne "Running")
	{
		Start-Sleep -s 1
		$serviceObj = get-service -computername $remoteServerName DNS
	}

	$serviceObj.Close()
	$retVal = $true
}
catch
{
	Write-Error "Exception in start Active Directory Domain Control"
	$retVal = $false
}
return $retVal
