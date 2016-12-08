#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
####################################################################################
##
## 	Microsoft Windows Powershell Scripting
##  File:		AccessShareFolder.ps1
##	Purpose:	To access a shared folder with specified credential.
##	Version: 	1.1 (18 Jan, 2012)
####################################################################################

param(
		[string]$uncPath,
		[string]$userName,
		[string]$password,
		[string]$domainName
		
	)

#----------------------------------------------------------------------------
#Replace the / with \, because //win8as/sharefolder will not be recognized by test-path
#----------------------------------------------------------------------------
$uncPath = $uncPath.Replace("/","\")

#----------------------------------------------------------------------------
# Get working directory and log file path
#----------------------------------------------------------------------------
$workingDir=$MyInvocation.MyCommand.path
$workingDir =Split-Path $workingDir
$runningScriptName=$MyInvocation.MyCommand.Name
$logFile="$workingDir\$runningScriptName.log"
$signalFile="$workingDir\$runningScriptName.signal"


#----------------------------------------------------------------------------
# Creat the log file
#----------------------------------------------------------------------------
echo "-----------------$runningScriptName Log----------------------" > $logFile
echo "UNCPath  = $uncPath" >> $logFile
echo "userName = $userName" >> $logFile
echo "password = $password" >> $logFile
echo "domainName  = $domainName" >> $logFile

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    echo "-----------------$runningScriptName Log----------------------" > $logFile
    echo "Usage: This script is to access the shared folder." >> $logFile 
    echo "Example: $runningScriptName UNCpath username password domainname"	>> $logFile    
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
# Check the required parameters
#----------------------------------------------------------------------------

if ($uncPath -eq $null -or $uncPath -eq "")
{
	echo "Error: The required UNC path is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
	Throw "UNC path cannot be blank." 
}

if ($userName -eq $null -or $userName -eq "" )
{
	echo "Error: The required username is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
	Throw"The username cannot be blank."	
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

#----------------------------------------------------------------------------
# Check the existence of the UNC path
#----------------------------------------------------------------------------
function Check-UNCPath($path){
	$isExist =$false
	try{
		echo "Check the existence of the UNC path: $uncPath" >> $logFile

		$isExist= Test-Path -Path $path -PathType Container 
	}
	catch  [system.Exception]{
	 	$tryError =$_.Exception
		echo $tryError >> $logFile
		echo "Failed when check UNCPath existence." >> $logFile
		Throw "Error in function Check-UNCPath." 
	}
	return $isExist
}

#$isUNCPathExist=Check-UNCPath ($uncPath)

#if ( $isUNCPathExist -eq $false){
#	echo "Error: The UNC path $uncPath is invalid. Please double check your inputs." >> $logFile
#	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
#	Throw "Error: The UNC path $uncPath is invalid. Please double check your inputs."
#}

#echo "The UNC path $uncPath exists." >> $logFile

#----------------------------------------------------------------------------
# Mount the unc path to a local drive, this will invoke SMB message
#----------------------------------------------------------------------------
$fullusername="$domainName\$userName"

echo "Run the net use command to mount the unc path to a local drive." >> $logFile
net use $uncPath  $password /user:$fullusername >> $logFile

echo "create testfile.txt to the share folder" >> $logFile
New-Item $uncPath\testfile.txt -type file -force -value "This is a test file."

echo "Run net use /delete." >> $logFile
net use $uncPath /delete >> $logFile

# Sleep works heres. 
# However, it's difficult to maintain for all cases in a ready msi.
# Suggest move the sleep to testsuite code, with a ptf config property.
# start-sleep 20

echo "done" > $signalFile
echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
