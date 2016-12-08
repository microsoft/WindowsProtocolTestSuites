#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
####################################################################################
##
## 	Microsoft Windows Powershell Scripting
##  File:		CleanupCachedTickets.ps1
##	Purpose:	To purge the tickets cached in current workstation.
##	Version: 	1.1 (18 Jan, 2012)
####################################################################################

#----------------------------------------------------------------------------
# Get working directory and log file path
#----------------------------------------------------------------------------
$workingDir=$MyInvocation.MyCommand.path
$workingDir =Split-Path $workingDir
$runningScriptName=$MyInvocation.MyCommand.Name
$logFile="$workingDir\$runningScriptName.log"

#----------------------------------------------------------------------------
# Purge the tickets in current computer with command line
#----------------------------------------------------------------------------
echo "-----------------$runningScriptName Log----------------------" > $logFile
echo "Run the klist purge command." >> $logFile
cmd /c klist -li 0x3e7 purge >> $logFile
cmd /c klist purge >> $logFile

#----------------------------------------------------------------------------
# Check the out put information
#----------------------------------------------------------------------------

$fileContents= Get-Content $logFile

if ([regex]::IsMatch($fileContents, "\s*Ticket\(s\)\s+purged!\Z")-eq $true){
	echo "Ticket(s) purged successfully." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
	return $true
}
else{
	echo "Ticket(s) haven't been purged." >> $logFile
	echo "-----------------$runningScriptName Log Done----------------------" >> $logFile
	return $false
}

