#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           IsUnjoinDomainSuccess.ps1
## Purpose:        Use local admin user account to setup a PowerShell Remote Session
##                 and check if the client computer has unjoined domain successfully.
## Version:        1.1 (2 Mar, 2012)
##
##############################################################################

[string]$clientComputerName  = $PtfProp_ClientIP
[string]$clientAdminUserName = $PtfProp_ClientAdminUsername
[string]$clientAdminPwd      = $PtfProp_ClientAdminPwd
[string]$scriptPath          = $PtfProp_ClientScriptPath

#-------------------------------------------------------------------------------------#
# Create $logFile if not exist
#-------------------------------------------------------------------------------------#
[string]$logPath = $PtfProp_DriverLogPath
if ($logPath -eq $null -or $LogPath -eq "")
{
	$logPath = "..\Logs"
}
[string]$logFile = $logPath + "\$env:MS_ADOD_TESTCASENAME.log"
if (!(Test-Path -Path $logFile))
{
	$null = New-Item -Type File -Path $logFile -Force
}

#-------------------------------------------------------------------------------------#
# Print execution information
#-------------------------------------------------------------------------------------#
echo "================================================================" |Out-File $logFile -Append
echo "Started Transcript at $(Get-Date)." |Out-File $logFile -Append
echo "EXECUTING [IsUnjoinDomainSuccess.ps1]." | Out-File $logFile -Append
echo "`$clientComputerName  = $clientComputerName" | Out-File $logFile -Append
echo "`$clientAdminUserName = $clientAdminUserName" | Out-File $logFile -Append
echo "`$clientAdminPwd      = $clientAdminPwd" | Out-File $logFile -Append
echo "`$scriptPath          = $scriptPath" | Out-File $logFile -Append
echo "`$global:userName     = $global:userName" | Out-File $logFile -Append
echo "`$global:password     = $global:password" | Out-File $logFile -Append

#-------------------------------------------------------------------------------------#
# When exceptions trapped, stop the script and return null
#-------------------------------------------------------------------------------------#
trap
{
	$_ | Out-File $logFile -Append
	Throw "EXECUTE [IsUnjoinDomainSuccess.ps1] FAILED. For more information, please see $logFile."
}

#-------------------------------------------------------------------------------------#
# Check parameters
#-------------------------------------------------------------------------------------#
echo "Check parameters..." | Out-File $logFile -Append
if ($clientComputerName -eq $null -or $clientComputerName -eq "")
{
	Throw "Parameter `$clientComputerName NOT found."
}
if ($clientAdminUserName -eq $null -or $clientAdminUserName -eq "")
{
	echo "Parameter `$clientAdminUserName NOT found. Try to use `$global:userName." | Out-File $logFile -Append
	if ($global:userName -eq $null -or $global:userName -eq "")
	{
		Throw "Parameter `$global:userName NOT found."
	}
	$clientAdminUserName = $global:userName
}
if ($clientAdminPwd -eq $null -or $clientAdminPwd -eq "")
{
	echo "Parameter `$clientAdminPwd NOT found. Try to use `$global:password." | Out-File $logFile -Append
	if ($global:password -eq $null -or $global:password -eq "")
	{
		Throw "Parameter `$global:password NOT found."
	}
	$clientAdminPwd = $global:password
}
if ($scriptPath -eq $null -or $scriptPath -eq "")
{
	Throw "Parameter `$scriptPath NOT found."
}

#-------------------------------------------------------------------------------------#
# Setup a remote session to the client computer.
# If unjoin domain succeeded, logon using COMPUTERNAME\USERNAME to setup remote session.
#-------------------------------------------------------------------------------------#
$RemoteSession = .\SetupRemoteSession.ps1 $clientComputerName $clientComputerName\$clientAdminUserName $clientAdminPwd
if($RemoteSession -eq $null -or $RemoteSession -eq "")
{
	Throw "New PowerShell Remote Session to $clientComputerName Failed."
}

#-------------------------------------------------------------------------------------#
# Check if client computer has unjoined domain.
#-------------------------------------------------------------------------------------#
echo "Check if client computer has unjoined domain..." | Out-File $logFile -Append
$objSys = Invoke-Command -Session $RemoteSession -ScriptBlock { Get-Wmiobject -Class "Win32_ComputerSystem" } -ErrorAction Stop
$objSys | Out-File $logFile -Append

echo "Remove remote session..." | Out-File $logFile -Append
Remove-PSSession -Session $RemoteSession -ErrorAction Stop

#-------------------------------------------------------------------------------------#
# Ending script
#-------------------------------------------------------------------------------------#
echo "EXECUTE [IsUnjoinDomainSuccess.ps1] FINISHED." | Out-File $logFile -Append
if($objSys.PartOfDomain -eq $true) 
{
	return $false
}
else 
{
	return $true
}