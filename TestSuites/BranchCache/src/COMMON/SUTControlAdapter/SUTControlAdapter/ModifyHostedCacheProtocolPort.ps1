######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$hostedClient = $hostedClientComputerName,
[string]$hostedServer = $hostedServerComputerName,
[string]$port = $newPort
)

####################################
# Modify Hosted Cache Client Connect Port
####################################
if($hostedClient -ne "" -and $hostedClient -ne $null)
{
	####################################
	# Update register
	####################################
	try
	{
		# Get 64 bit registry remotely from a 32 bit application.
		REG ADD "\\$hostedClient\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\PeerDist\HostedCache\Connection" /v ConnectPort /t REG_DWORD /d $port /REG:64 /f 
	}
	catch
	{
		"SUT control set excaption happened, failed to modify the hosted cache protocol port!"
	}
	finally
	{
		sleep(2)
	}
}

####################################
# Modify Hosted Cache Server Listen Port
####################################
if($hostedServer -ne "" -and $hostedServer -ne $null)
{
	####################################
	# Update register
	####################################
	try
	{
		# Get 64 bit registry remotely from a 32 bit application.
		REG ADD "\\$hostedServer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\PeerDist\HostedCache\Connection" /v ListenPort /t REG_DWORD /d $port /REG:64 /f 
	}
	catch
	{
		"SUT control set excaption happened, failed to modify the hosted cache protocol port!"
	}
	finally
	{
		sleep(2)
	}
}
