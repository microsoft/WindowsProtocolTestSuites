########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

################################################################################
## 	Microsoft Windows Powershell Scripting
##  File:		CleanupCachedTickets.ps1
##	Purpose:	Cleanup cached kerberos tickets and dns
##	Version: 	1.1 (12 Apr, 2013)
#################################################################################

#-------------------------------------------------------------------------------------#
# Create $logFile if not exist
#-------------------------------------------------------------------------------------#
[string]$logPath = $PtfProp_DriverLogPath
if ($logPath -eq $null -or $LogPath -eq "")
{
	$logPath = "..\Logs"
}
[string]$logFile = $logPath + "\$env:MS_AZOD_TESTCASENAME.log"
# Create a new log file if it doesn't exist.
if (!(Test-Path $logFile))
{
	New-Item -Type File -Path $logFile -Force
}

#-------------------------------------------------------------------------------------#
# Print execution information
#-------------------------------------------------------------------------------------#
echo "================================================================" |Out-File $logFile -Append
echo "Started Transcript at $(Get-Date)." |Out-File $logFile -Append
echo "EXECUTING [CleanupCachedTickets.ps1]." |Out-File $logFile -Append
echo "`$computerName  = $computerName" |Out-File $logFile -Append
echo "`$userName = $userName" |Out-File $logFile -Append
echo "`$userPassword      = $userPassword" |Out-File $logFile -Append


#-------------------------------------------------------------------------------------#
# When exceptions trapped, stop the script and return null
#-------------------------------------------------------------------------------------#
trap
{
	$_ | Out-File $logFile -Append
	Throw "EXECUTE [CleanupCachedTickets.ps1] FAILED. For more information, please see $logFile."
}

#-------------------------------------------------------------------------------------#
# Check parameters
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
if ($userPassword -eq $null -or $userPassword -eq "")
{
  	echo "Parameter `$userPassword NOT found. Try to use `$global:password." | Out-File $logFile -Append
  	if ($global:password -eq $null -or $global:password -eq "")
  	{
		Throw "Parameter `$global:password NOT found."
	}
	$userPassword = $global:password
}

#-------------------------------------------------------------------------------------#
#setup remote session to the remote computer
#-------------------------------------------------------------------------------------#
echo "Setup PowerShell Remote Session to $computerName..." | Out-File $logFile -Append
$RemoteSession = . .\RemoteSessionSetup.ps1 $computerName $userName $userPassword
if($RemoteSession -eq $null -or $RemoteSession -eq "")
{
	Throw "New PowerShell Remote Session to $computerName Failed."
}
try{
		
		#-------------------------------------------------------------------------------------#
		# Clear dns cache on remote computer.
		#-------------------------------------------------------------------------------------#
		echo "Clear DNS cache on $computerName..." | Out-File $logFile -Append
		Invoke-Command -Session $RemoteSession -ScriptBlock {ipconfig /flushdns}

		#-------------------------------------------------------------------------------------#
		# Clean the ticket cached in application server
		#-------------------------------------------------------------------------------------#
		echo "Clear kerberos ticket cached on $computerName..." | Out-File $logFile -Append
		Invoke-Command -Session $RemoteSession -ScriptBlock {klist -li 0x3e7 purge}
		Invoke-Command -Session $RemoteSession -ScriptBlock {klist  purge}

		
		#-------------------------------------------------------------------------------------#
		# Remove the remote session to the remote computer
		#-------------------------------------------------------------------------------------#

		echo "Remove the remote session to the $computerName..." | Out-File $logFile -Append

		Remove-PSSession -Session $RemoteSession -ErrorAction Stop
	}
	catch
	{
		Throw "Remote Powershell session to $computerName Failed."
	}
	finally
	{
		if($RemoteSession -eq $null -or $RemoteSession -eq "")
		{
			#-------------------------------------------------------------------------------------#
			# Remove the remote session to the remote computer
			#-------------------------------------------------------------------------------------#

			echo "Remove the remote session to the $computerName..." | Out-File $logFile -Append

			Remove-PSSession -Session $RemoteSession -ErrorAction Stop
		}
		
	}

