# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param($workingDir = $PSScriptRoot)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath"

#----------------------------------------------------------------------------
# if working dir is not exists. it will use scripts path as working path
#----------------------------------------------------------------------------
if(!(Test-Path "$workingDir"))
{
    $workingDir = $scriptPath
}

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
# Define common variables
#----------------------------------------------------------------------------
$fullAccessAccount = "BUILTIN\Administrators"
$systemDrive = $ENV:SystemDrive


#----------------------------------------------------------------------------
# Create Share Folders
#----------------------------------------------------------------------------
.\Write-Info.ps1 "$systemDrive\FileShare"
.\Create-SMBShare.ps1 -name "FileShare" -Path "$systemDrive\FileShare" -FullAccess "$fullAccessAccount"  -CachingMode BranchCache

.\Write-Info.ps1 "$systemDrive\SMBBasic"
.\Create-SMBShare.ps1 -name "SMBBasic" -Path "$systemDrive\SMBBasic" -FullAccess "$fullAccessAccount"  -CachingMode BranchCache

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0