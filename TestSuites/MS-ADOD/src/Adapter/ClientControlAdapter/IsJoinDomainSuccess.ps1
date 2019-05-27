#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           IsJoinDomainSuccess.ps1
## Purpose:        Use domain admin user account to setup a PowerShell Remote Session
##                 and check if the client computer has joined domain successfully.
## Version:        1.1 (2 Mar, 2012)
##
##############################################################################

[string]$clientComputerName  = $PtfProp_ClientIP
[string]$clientAdminUserName = $PtfProp_ClientAdminUsername
[string]$clientAdminPwd      = $PtfProp_ClientAdminPwd
[string]$scriptPath          = $PtfProp_ClientScriptPath
[string]$fullDomainName      = $PtfProp_FullDomainName
[string]$domainAdminUserName = $PtfProp_DomainAdminUsername
[string]$domainAdminPwd      = $PtfProp_DomainAdminPwd

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
echo "EXECUTING [IsJoinDomainSuccess.ps1]." | Out-File $logFile -Append
echo "`$clientComputerName  = $clientComputerName" | Out-File $logFile -Append
echo "`$clientAdminUserName = $clientAdminUserName" | Out-File $logFile -Append
echo "`$clientAdminPwd      = $clientAdminPwd" | Out-File $logFile -Append
echo "`$scriptPath          = $scriptPath" | Out-File $logFile -Append
echo "`$fullDomainName      = $fullDomainName" | Out-File $logFile -Append
echo "`$domainAdminUserName = $domainAdminUsername" | Out-File $logFile -Append
echo "`$domainAdminPwd      = $domainAdminPwd" | Out-File $logFile -Append
echo "`$global:userName     = $global:userName" | Out-File $logFile -Append
echo "`$global:password     = $global:password" | Out-File $logFile -Append

#-------------------------------------------------------------------------------------#
# When exceptions trapped, stop the script and return null
#-------------------------------------------------------------------------------------#
trap
{
	$_ | Out-File $logFile -Append
	Throw "EXECUTE [IsJoinDomainSuccess.ps1] FAILED. For more information, please see $logFile."
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
if ($fullDomainName -eq $null -or $fullDomainName -eq "")
{
	Throw "Parameter `$fullDomainName NOT found."
}
if ($domainAdminUserName -eq $null -or $domainAdminUserName -eq "")
{
	echo "Parameter `$domainAdminUserName NOT found. Try to use `$global:userName." | Out-File $logFile -Append
	if ($global:userName -eq $null -or $global:userName -eq "")
	{
		Throw "Parameter `$global:userName NOT found."
	}
	$domainAdminUserName = $global:userName
}
if ($domainAdminPwd -eq $null -or $domainAdminPwd -eq "")
{
	echo "Parameter `$domainAdminPwd NOT found. Try to use `$global:password." | Out-File $logFile -Append
	if ($global:password -eq $null -or $global:password -eq "")
	{
		Throw "Parameter `$global:password NOT found."
	}
	$domainAdminPwd = $global:password
}

#-------------------------------------------------------------------------------------#
# Setup a remote session to the client computer.
# If join domain succeeded, logon using DOMAIN\USERNAME to setup remote session.
#-------------------------------------------------------------------------------------#
echo "Setup PowerShell Remote Session to $clientComputerName..." | Out-File $logFile -Append
$RemoteSession = .\SetupRemoteSession.ps1 $clientComputerName $fullDomainName\$domainAdminUserName $domainAdminPwd
if($RemoteSession -eq $null -or $RemoteSession -eq "")
{
	Throw "New PowerShell Remote Session to $clientComputerName Failed."
}

#-------------------------------------------------------------------------------------#
# Check if client computer has joined domain.
#-------------------------------------------------------------------------------------#
echo "Check if client computer has joined domain..." | Out-File $logFile -Append
$objSys = Invoke-Command -Session $RemoteSession -ScriptBlock { Get-Wmiobject -Class "Win32_ComputerSystem" } -ErrorAction Stop

echo "Expected domain:$fullDomainName." | Out-File $logFile -Append
if($objSys.PartOfDomain -eq $true) 
{
	$objClientDomain = Invoke-Command -Session $RemoteSession -ScriptBlock { [System.DirectoryServices.ActiveDirectory.Domain]::GetCurrentDomain() }
	echo "Client computer has joined domain:$objClientDomain." | Out-File $logFile -Append
	if($objClientDomain.Name -eq $fullDomainName) 
	{
		echo "Check Succeeded." | Out-File $logFile -Append
		echo "Remove remote session..." | Out-File $logFile -Append
		Remove-PSSession -Session $RemoteSession -ErrorAction Stop
		echo "EXECUTE [IsJoinDomainSuccess.ps1] FINISHED." | Out-File $logFile -Append
		return $true
	}
}
else
{
  echo "Client computer has not joined domain." | Out-File $logFile -Append
}

echo "Check Failed." | Out-File $logFile -Append
echo "Remove remote session..." | Out-File $logFile -Append
Remove-PSSession -Session $RemoteSession -ErrorAction Stop
echo "EXECUTE [IsJoinDomainSuccess.ps1] FINISHED." | Out-File $logFile -Append
return $false