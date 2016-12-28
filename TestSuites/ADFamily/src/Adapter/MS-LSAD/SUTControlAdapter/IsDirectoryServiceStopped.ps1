#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           IsDirectoryServiceStopped.ps1
## Purpose:        Get Active Directory Domain Control Service Status
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2
##
##############################################################################

$retVal = $false
try
{
	$scriptPath = Split-Path $MyInvocation.MyCommand.Definition
	Push-Location $scriptPath
	$remoteServerName = .\ReadPtfConfig.ps1 'WritableDC1.NetbiosName'

	#----------------------------------------------------------------------------
	# Get Active Directory Domain Control Service Status
	#----------------------------------------------------------------------------
	$serviceObj = get-service -computername $remoteServerName NTDS
	if ($serviceObj.Status -eq "Stopped")
	{
		$retVal = $true
	}
	else
	{
		$retVal = $false
	}

	$serviceObj.Close()
}
catch
{
	Write-Error "Exception in get Active Directory Domain Control"
	$retVal = $false
}
return $retVal
