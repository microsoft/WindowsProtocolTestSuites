#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Protocol.xml")

#----------------------------------------------------------------------------
# Create Working Dir and put Protocol.xml into it
#----------------------------------------------------------------------------
Create-WorkingDir.ps1 $workingDir $protocolConfigFile

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function ExitCode()
{ 
    return $MyInvocation.ScriptLineNumber
}

#----------------------------------------------------------------------------
# Check input parameters
#----------------------------------------------------------------------------
if(!(Test-Path "$workingDir"))
{
    Write-Error.ps1 "WorkingDir $workingDir does not exist."
    exit ExitCode
}

if(!(Test-Path "$protocolConfigFile"))
{
    Write-Error.ps1 "protocolConfigFile $protocolConfigFile does not exist."
    exit ExitCode
}

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
Write-Info.ps1 "Get content from protocol config file"
[xml]$config = Get-Content "$protocolConfigFile"
if($config -eq $null)
{
    Write-Error.ps1 "protocolConfigFile $protocolConfigFile is not a valid XML file."
    exit ExitCode
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$fullAccessAccount = "BUILTIN\Administrators"
$systemDrive = $ENV:SystemDrive


#----------------------------------------------------------------------------
# Create Share Folders
#----------------------------------------------------------------------------
Write-Info.ps1 "$systemDrive\FileShare"
Create-SMBShare.ps1 -name "FileShare" -Path "$systemDrive\FileShare" -FullAccess "$fullAccessAccount"  -CachingMode BranchCache

Write-Info.ps1 "$systemDrive\SMBBasic"
Create-SMBShare.ps1 -name "SMBBasic" -Path "$systemDrive\SMBBasic" -FullAccess "$fullAccessAccount"  -CachingMode BranchCache

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0