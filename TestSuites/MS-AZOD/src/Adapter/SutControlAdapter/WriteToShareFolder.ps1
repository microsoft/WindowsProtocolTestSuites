########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

####################################################################################
## 	Microsoft Windows Powershell Scripting
##  File:		WriteToShareFolder.ps1
##	Purpose:	To create a file to a shared folder with specified credential.
##	Version: 	1.0 (18 Jun, 2014)
####################################################################################
#param(
#		[string]$uncPath,		
#		[string]$fileName,	
#		[string]$userName,
#		[string]$password,
#		[string]$domainName,
#		[string]$logFileName
#		
#	)
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
$logFile = "$PtfProp_DriverLogPath\$logFileName"
$signalFile="$workingDir\$runningScriptName.signal"


#----------------------------------------------------------------------------
# Create the log file
#----------------------------------------------------------------------------
echo "$runningScriptName starts." 
echo "-----------------$runningScriptName Log----------------------" > $logFile
echo "UNCPath  = $uncPath" >> $logFile
echo "FileName  = $fileName" >> $logFile
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
    echo "Usage: This script is to create a file to the shared folder." >> $logFile 
    echo "Example: $runningScriptName UNCpath fileName username password domainname"	>> $logFile    
}
#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return $true
}

#----------------------------------------------------------------------------
# Check the required parameters
#----------------------------------------------------------------------------

if ($uncPath -eq $null -or $uncPath -eq "")
{
	echo "Error: The required UNC path is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
	#Throw "UNC path cannot be blank." 
    return $false
}

if ($fileName -eq $null -or $uncPath -eq "")
{
	echo "Error: The required filename is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
    return $false
}

if ($userName -eq $null -or $userName -eq "" )
{
	echo "Error: The required username is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
    return $false
}
if ($password -eq $null -or $password -eq "")
{
	echo "Error: The required password is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
    return $false
}
if ($domainName -eq $null -or $domainName -eq "")
{
	echo "Error: The required domain name is blank." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
    return $false
}

#----------------------------------------------------------------------------
# Check the existence of the UNC path
#----------------------------------------------------------------------------
function Check-UNCPath($path){
	$isExist =$false
	try{
		echo "Check the existence of the UNC path: $uncPath"
		echo "Check the existence of the UNC path: $uncPath" >> $logFile

		$isExist= Test-Path -Path $path -PathType Container 
	}
	catch  [system.Exception]{
	 	$tryError =$_.Exception
		echo $tryError >> $logFile
		echo "Failed when check UNCPath existence." >> $logFile
		Throw "Error in function Check-UNCPath." 
	}
    if(!$?)
    {
        return $false
    }
	return $isExist
}

$isUNCPathExist=Check-UNCPath ($uncPath)

if ( $isUNCPathExist -eq $false){
	echo "Error: The UNC path $uncPath is invalid. Please double check your inputs."
	echo "Error: The UNC path $uncPath is invalid. Please double check your inputs." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
    return $false
}
else
{
	echo "The UNC path: $uncPath exists."
	echo "The UNC path: $uncPath exists."  >> $logFile
}
#----------------------------------------------------------------------------
# Mount the unc path to a local drive, this will invoke SMB message
#----------------------------------------------------------------------------
$fullusername="$domainName\$userName"

$isUNCPathExist=Check-UNCPath ("X:\")

if ( $isUNCPathExist -eq $true){
	echo "Run net use X: /delete." >> $logFile
	net use X: /delete /yes


	if(!$?)
	{
		echo "Failed to run the net use X: /delete command." >> $logFile
    
	}
	else
	{
		echo "Run the net use /delete command successfully." >> $logFile
	}
}

ipconfig /flushdns
ipconfig /renew
klist purge
klist -li 0x3e7 purge

#Sleep for client sku
if($((gwmi win32_operatingsystem).ProductType) -eq "1")
{
	sleep 20
}

echo "Run the net use command to mount the unc path to a local drive." 
echo "Run the net use command to mount the unc path to a local drive." >> $logFile
try
{
    net use X: $uncPath $password  /user:$fullusername  /persistent:no
}
catch
{
	echo "Failed to run the net use command to map network drive."
    echo "Failed to run the net use command to map network drive." >> $logFile
    return $false
}

if(!$?)
{
	echo "Failed to run the net use command to map network drive." 
	echo "Failed to run the net use command to map network drive." >> $logFile
    return $false
}
else
{
	echo "Run the net use  command successfully."
    echo "Run the net use  command successfully." >> $logFile
}

echo "Create a file to the share folder with command new-item."
echo "Create a file to the share folder with command new-item." >> $logFile

$fileFullPath =  "X:\" + $fileName

New-Item  $fileFullPath -type file -force -value "This is a test file with user name $userName, password $password, and domain Name $domainName."

if(!$?)
{
	echo "Failed to create file." 
	echo "Failed to create file." >> $logFile
    return $false
}

echo "Create file successfully."
echo "Create file successfully." >> $logFile

echo "Run net use X: /del." 
echo "Run net use X: /del." >> $logFile
net use X: /del /yes >> $logFile
if(!$?)
{
	echo "Failed to run the net use X: /del /yes command." 
	echo "Failed to run the net use X: /del /yes command." >> $logFile
	net use X： /del /yes
    return $false
}

echo "Done" > $signalFile
echo "$runningScriptName ends." 
echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
return $true