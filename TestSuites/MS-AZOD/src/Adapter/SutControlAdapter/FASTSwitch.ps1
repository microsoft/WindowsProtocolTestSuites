########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

################################################################################
## 	Microsoft Windows Powershell Scripting
##  File:		FASTSwitch.ps1
##	Purpose:	Turn on or off the cabac of the remote computer by modify registy.
##	Version: 	1.1 (12 Mar, 2013)
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
echo "EXECUTING [FASTSwitch.ps1]." |Out-File $logFile -Append
echo "`$computerName  = $computerName" |Out-File $logFile -Append
echo "`$userName = $userName" |Out-File $logFile -Append
echo "`$userPassword      = $userPassword" |Out-File $logFile -Append

#-------------------------------------------------------------------------------------#
# When exceptions trapped, stop the script and return null
#-------------------------------------------------------------------------------------#
trap
{
	$_ | Out-File $logFile -Append
	Throw "EXECUTE [FASTSwitch.ps1] FAILED. For more information, please see $logFile."
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
		# Turn on/off CBAC on remote computer
		#-------------------------------------------------------------------------------------#
		echo "Turn on/off CBAC on $computerName..." | Out-File $logFile -Append
	
		Invoke-Command -Session $RemoteSession -ScriptBlock {REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC /f}
		Invoke-Command -Session $RemoteSession -ScriptBlock {REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\parameters /f}
	
		Invoke-Command -Session $RemoteSession -ScriptBlock {param($fastswitch) REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\parameters  /v CbacAndArmorLevel /t REG_DWORD /d $fastswitch /f} -argumentlist $FASTLevel					
		Invoke-Command -Session $RemoteSession -ScriptBlock {REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\KDC\parameters  /v EnableCbacAndArmor /t REG_DWORD /d 1 /f}
		
		Invoke-Command -Session $RemoteSession -ScriptBlock {REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos /f}
		Invoke-Command -Session $RemoteSession -ScriptBlock {REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\parameters /f}
	
		Invoke-Command -Session $RemoteSession -ScriptBlock {param($cbacswitch) REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\parameters  /v EnableCbacAndArmor /t REG_DWORD /d $cbacswitch /f} -argumentlist $cbacSwitch		
		Invoke-Command -Session $RemoteSession -ScriptBlock {REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\policies\system\kerberos\parameters  /v SupportedEncryptionTypes /t REG_DWORD /d 0x7fffffff /f}
		

		
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

