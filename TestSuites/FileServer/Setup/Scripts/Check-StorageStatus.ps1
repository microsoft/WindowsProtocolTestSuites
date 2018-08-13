#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

param(
    [ValidateSet("CreateCheckerTask", "StartChecker")]
    [string]$action="CreateCheckerTask"
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$scriptName = $MyInvocation.MyCommand.Path
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Create checker task
#----------------------------------------------------------------------------
if($action -eq "CreateCheckerTask")
{
    Write-Info.ps1 "Create checker task."
    $TaskName = "ExecuteChecker"
    $Task = "PowerShell $scriptName StartChecker"
    # Create a task which will auto run current script every 5 minutes with StartChecker action.
    CMD.exe /C "schtasks /Create /RU SYSTEM /SC MINUTE /MO 5 /TN `"$TaskName`" /TR `"$Task`" /IT /F"

    Write-Info.ps1 "Start checker after create the checker task."
    $action = "StartChecker"  
}

#----------------------------------------------------------------------------
# Start checker
#----------------------------------------------------------------------------
if($action -eq "StartChecker")
{
    $iscsiServerTarget = Get-IscsiServerTarget
    if($iscsiServerTarget -ne $null)
    {
        $iscsiServerTarget | fl TargetName,TargetIqn,Status,InitiatorIds,LastLogin,LunMappings
    }
    else
    {
        Write-Error.ps1 "Cannot find Iscsi Server Target."
    }

    Write-Info.ps1 "Finish checker."
    Sleep 5 # To display above messages
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0