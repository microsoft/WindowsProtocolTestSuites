#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           JoinDomainPredefAcct.ps1
## Purpose:        Use PowerShell Remote Session to trigger the client computer to join domain by a predefined account.
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
echo "EXECUTING [JoinDomainPredefAcct.ps1]." | Out-File $logFile -Append
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
	Throw "EXECUTE [JoinDomainCreateAcctLDAP.ps1] FAILED. For more information, please see $logFile."
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
# Since client has not joined a domain yet, the username should be COMPUTERNAME\USERANME
#-------------------------------------------------------------------------------------#
echo "Setup PowerShell Remote Session to $clientComputerName..." | Out-File $logFile -Append
$RemoteSession = .\SetupRemoteSession.ps1 $clientComputerName $clientComputerName\$clientAdminUserName $clientAdminPwd
if($RemoteSession -eq $null -or $RemoteSession -eq "")
{
	Throw "New PowerShell Remote Session to $clientComputerName Failed."
}

#-------------------------------------------------------------------------------------#
# Clear dns cache on client computer.
#-------------------------------------------------------------------------------------#
echo "Clear DNS cache on $clientComputerName..." | Out-File $logFile -Append
$null = Invoke-Command -Session $RemoteSession -ScriptBlock {ipconfig /flushdns}

#-------------------------------------------------------------------------------------#
# Import MS-ADOD PowerShell Modules on Client computer.
#-------------------------------------------------------------------------------------#
echo "Import MS-ADOD Modules on $clientComputerName..." | Out-File $logFile -Append
Invoke-Command -Session $RemoteSession -ScriptBlock {param ($pathT) . ($pathT+"\Join-Domain.ps1")} -ArgumentList $scriptPath

#-------------------------------------------------------------------------------------#
# Join Domain by a predefined computer account
#-------------------------------------------------------------------------------------#
echo "Join Domain by a predefined computer account..." | Out-File $logFile -Append
$errorcode = Invoke-Command -Session $RemoteSession -ScriptBlock {param ($domainNameT, $userNameT, $passwordT) JoinDomain-PredefAccount $domainNameT $userNameT $passwordT} -ArgumentList $fullDomainName, $fullDomainName\$domainAdminUserName, $domainAdminPwd
if($errorcode -eq 0) {
	echo "Join Domain $fullDomainName Succeeded." | Out-File $logFile -Append
	echo "Restart $clientComputerName..." | Out-File $logFile -Append
	Invoke-Command -Session $RemoteSession -ScriptBlock {Restart-Computer -force}
	echo "Remove remote session..." | Out-File $logFile -Append
	Remove-PSSession -Session $RemoteSession -ErrorAction Stop
	echo "EXECUTE [JoinDomainCreateAcctLDAP.ps1] FINISHED." | Out-File $logFile -Append
	return $true
}
else {
	echo "Join Domain $fullDomainName Failed - errorcode: $errorcode." | Out-File $logFile -Append
	echo "Remove remote session..." | Out-File $logFile -Append
	Remove-PSSession -Session $RemoteSession -ErrorAction Stop
	echo "EXECUTE [JoinDomainCreateAcctLDAP.ps1] FINISHED." | Out-File $logFile -Append
	return $false
}