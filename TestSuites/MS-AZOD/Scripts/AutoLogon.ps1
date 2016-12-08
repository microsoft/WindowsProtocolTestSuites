#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
####################################################################################
##
## 	Microsoft Windows Powershell Scripting
##  File:		AutoLogon.ps1
##	Purpose:	To log into one computer automatically.
##	Version: 	1.1 (10 Feb, 2012)
####################################################################################


param(
		[string] $computerName,
		[string] $userName,
		[string] $password,
		[string] $domainName
	)
#----------------------------------------------------------------------------
# Get working directory and log file path
#----------------------------------------------------------------------------

$workingDir=$MyInvocation.MyCommand.path
$workingDir =Split-Path $workingDir
$runningScriptName=$MyInvocation.MyCommand.Name
$logFile="$workingDir\$runningScriptName.log"

#----------------------------------------------------------------------------
# Create the log file
#----------------------------------------------------------------------------
echo "-----------------$runningScriptName Log----------------------" > $logFile
echo "computername 	= $computerName" >> $logFile
echo "userName  = $userName" >> $logFile
echo "password  = $password" >> $logFile
echo "domainName  = $domainName" >> $logFile

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    echo "----------------$runningScriptName Log----------------------" > $logFile
    echo "Usage: This script is to log into one computer automatically."   >> $logFile
    echo "Example: $runningScriptName computername username password domainname"  >> $logFile	
    
}	
#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# check the required parameters
#----------------------------------------------------------------------------

if ($computerName -eq $null -or $computerName -eq "")
{
	echo "Error: The required computer name is blank." >> $logFile		
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile	
    Throw "Computer name cannot be blank."
}
if ($userName -eq $null -or $userName -eq "")
{
	echo "Error: The required userName is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
    Throw "UserName cannot be blank."
}
if ($password -eq $null -or $password -eq "")
{
	echo "Error: The required password is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
    Throw "Password cannot be blank."
}
if ($domainName -eq $null -or $domainName -eq "")
{
	echo "Error: The required domain name is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
    Throw "Domain Name cannot be blank."
}

# Edit the registry for the automatically log on
echo "Edit the registry for the automatically log on." >> $logFile

[string] $regKey='HKLM:\SOFTWARE\Microsoft\WIndows NT\CurrentVersion\Winlogon'
$regKeyExist = Test-Path -path $regKey

if($regKeyExist -eq $true){

	echo "The registry path exists. We will edit it now." >> $logFile
	set-ItemProperty -path $regKey -name AutoAdminLogon -value "1"
	set-ItemProperty -path $regKey -name DefaultUserName -value $userName
	set-ItemProperty -path $regKey -name DefaultDomainName -value $domainName
	set-ItemProperty -path $regKey -name DefaultPassword -value $passWord
	echo "Finish to edit registry." >> $logFile
}
else{
	echo "The registry path $regKey doesn't exist." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
	Throw "The registry path $regKey doesn't exist." 
}
echo "-----------------$runningScriptName Log Done----------------------" >> $logFile