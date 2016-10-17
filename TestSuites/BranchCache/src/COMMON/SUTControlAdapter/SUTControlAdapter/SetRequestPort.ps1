######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

param(
[string]$value = $port,
[string]$ServerComputerName = $computerName,
[string]$usr = $user,
[string]$pwd = $password
)

####################################
# Update register
####################################
try
{
	# Get 64 bit registry remotely from a 32 bit application.
	REG ADD "\\$ServerComputerName\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\PeerDist\DownloadManager\Peers\Connection" /v ConnectPort /t REG_DWORD /d $value /REG:64 /f 
}
catch
{
	"SUT control set excaption happened, failed to modify the pccrr request port!"
}
finally
{
	sleep(2)
}

return $true