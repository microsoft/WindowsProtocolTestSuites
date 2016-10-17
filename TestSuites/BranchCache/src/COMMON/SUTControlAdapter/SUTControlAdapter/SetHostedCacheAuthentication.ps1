######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$isAuthentication = $isAuthentication,
[string]$ComputerName = $sutComputer,
[string]$usr = $userName,
[string]$pwd = $password)

#region

function SetHostedCacheAuthentication(
$isAuthentication,
$ComputerName,
$usr,
$pwd)
{
	$commandLine = "netsh BranchCache set service mode = HOSTEDSERVER clientauthentication = " 
    if($isAuthentication -eq $true)
	{
		$commandLine = $commandLine + "DOMAIN"
	}

	if($isAuthentication -eq $false)
	{
		$commandLine = $commandLine + "NONE"
	}

	.\RemoteExecuteCommand.ps1 $ComputerName "$commandLine" $usr $pwd

	# wait for the client authentication set succeed
	sleep 2
}

#endregion

SetHostedCacheAuthentication $isAuthentication $ComputerName $usr $pwd 