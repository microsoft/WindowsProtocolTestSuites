########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           IsRemoteSessionSetup.ps1
## Purpose:        Use WinRM to Setup a powershell session from local machine to a remote computer then return if it can be connected.
## Version:        1.1 (2 Mar, 2012)
##
##############################################################################
param(
[string]$computerName,
[string]$userName,
[string]$password
)

#-------------------------------------------------------------------------------------#
# Create $logFile if not exist
#-------------------------------------------------------------------------------------#
[string]$logPath = $PtfProp_DriverLogPath
if ($logPath -eq $null -or $LogPath -eq "")
{
	$logPath = "..\Logs"
}
[string]$logFile = $logPath + "\$env:MS_AZOD_TESTCASENAME.log"
if (!(Test-Path -Path $logFile))
{
	$null = New-Item -Type File -Path $logFile -Force
}

#-------------------------------------------------------------------------------------#
# Print execution information
#-------------------------------------------------------------------------------------#
echo "================================================================" |Out-File $logFile -Append
echo "Started Transcript at $(Get-Date)." |Out-File $logFile -Append
echo "EXECUTING [RemoteSessionSetup.ps1]." | Out-File $logFile -Append
echo "`$computerName = $computerName" | Out-File $logFile -Append
echo "`$userName     = $userName" | Out-File $logFile -Append
echo "`$password     = $password" | Out-File $logFile -Append
echo "`$global:userName = $global:userName" | Out-File $logFile -Append
echo "`$global:password = $global:password" | Out-File $logFile -Append

#-------------------------------------------------------------------------------------#
# When exceptions trapped, stop the script and return null
#-------------------------------------------------------------------------------------#
trap
{
	$_ | Out-File $logFile -Append
	return $null
}

#-------------------------------------------------------------------------------------#
# Check Parameters
#-------------------------------------------------------------------------------------#
echo "Check parameters..." | Out-File $logFile -Append
if ($computerName -eq $null -or $computerName -eq "")
{
	Throw "Parameter `$computerName NOT found."
}
if ($userName -eq $null -or $userName -eq "")
{
	echo "Parameter `$userName NOT found. Try to use `$global:userName." | Out-File $logFile -Append
	if ($global:userName -eq $null -or $global:userName -eq "")
	{
		Throw "Parameter `$global:userName NOT found."
	}
	$userName = $global:userName
}
if ($password -eq $null -or $password -eq "")
{
	echo "Parameter `$password NOT found. Try to use `$global:password." | Out-File $logFile -Append
	if ($global:password -eq $null -or $global:password -eq "")
	{
		Throw "Parameter `$global:password NOT found."
	}
	$password = $global:password
}


#-------------------------------------------------------------------------------------#
# New a credential object for session setup
#-------------------------------------------------------------------------------------#
echo "New a credential with USERNAME:$userName and PASSWORD:$password..." | Out-File $logFile -Append
$pwdConverted = ConvertTo-SecureString $password -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential $userName, $pwdConverted -ErrorAction Stop

#-------------------------------------------------------------------------------------#
# Setup the remote powershell session by provided information
#-------------------------------------------------------------------------------------#
echo "Setup PowerShell Remote Session to $computerName..." | Out-File $logFile -Append
$remoteSession = New-PSSession -ComputerName $computerName -Credential $cred -ErrorAction Stop

#-------------------------------------------------------------------------------------#
# Ending script
#-------------------------------------------------------------------------------------#
echo "EXECUTE [RemoteSessionSetup.ps1] FINISHED." | Out-File $logFile -Append

#-------------------------------------------------------------------------------------#
# Return Result
#-------------------------------------------------------------------------------------#
$returnvalue = !($remoteSession -match "The client cannot connect to the destination specified in the request.")

Remove-PSSession -Session $remoteSession -ErrorAction Stop
return $returnvalue