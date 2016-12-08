#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
####################################################################################
##
## 	Microsoft Windows Powershell Scripting
##  File:		Update-FsrmClassificationPropertyDefinitionWithLog.ps1
##	Purpose:	update Classification property definition using powershell command Update-FsrmClassificationPropertyDefinition
##	Version: 	
####################################################################################

#----------------------------------------------------------------------------
# Get working directory and log file path
#----------------------------------------------------------------------------
$workingDir=$MyInvocation.MyCommand.path
$workingDir =Split-Path $workingDir
$runningScriptName=$MyInvocation.MyCommand.Name
$logFile="$workingDir\$runningScriptName.log"

#----------------------------------------------------------------------------
#Createthe log file
#----------------------------------------------------------------------------
echo "-----------------$runningScriptName Log----------------------" | Out-File $logFile -Append
echo "Executing [$runningScriptName.ps1]."  | Out-File $logFile -Append

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    echo "----------------$runningScriptName Log----------------------" | Out-File $logFile -Append
    echo "update Classification property definition."   | Out-File $logFile -Append
    echo "Example: $runningScriptName.ps1"  | Out-File $logFile -Append	    
}
#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

Update-FsrmClassificationPropertyDefinition | Out-File $logFile -Append
#----------------------------------------------------------------------------
# print out the log
#----------------------------------------------------------------------------

echo "-----------------$runningScriptName Log Done----------------------" | Out-File $logFile -Append


