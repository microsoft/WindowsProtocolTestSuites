#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

param(
    $workingDir = "$env:SystemDrive\Temp",
    [ValidateSet("CreateCheckerTask", "StartChecker")]
    [string]$action="CreateCheckerTask"
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$scriptName = $MyInvocation.MyCommand.Path
$env:Path += ";$scriptPath;$scriptPath\Scripts"
Push-Location $workingDir
#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function StartService($serviceName)
{
    $service = Get-Service -Name $serviceName
    $retryTimes = 0
    while($service.Status -ne "Running" -and $retryTimes -lt 6)
    {
        .\Write-Info.ps1 "Start $serviceName service."
        Start-Service -InputObj $service -ErrorAction Continue
        Sleep 10
        $retryTimes++ 
        $service = Get-Service -Name $serviceName
    }

    if($retryTimes -ge 6)
    {
        Write-Error.ps1 "Start $serviceName service failed within 1 minute."
    }
    else
    {
        .\Write-Info.ps1 "Service $serviceName is Running."
    }
}

#----------------------------------------------------------------------------
# Create checker task
#----------------------------------------------------------------------------
if($action -eq "CreateCheckerTask")
{
    .\Write-Info.ps1 "Create checker task."
    $TaskName = "ExecuteChecker"
    $Task = "PowerShell $scriptName StartChecker"
    # Create a task which will auto run current script every 5 minutes with StartChecker action.
    CMD.exe /C "schtasks /Create /RU SYSTEM /SC MINUTE /MO 5 /TN `"$TaskName`" /TR `"$Task`" /IT /F"
        
    .\Write-Info.ps1 "Do basic config of below services."

    .\Write-Info.ps1 "NETSH ras set type lanonly lanonly IPv4"
    CMD /C NETSH ras set type lanonly lanonly IPv4 2>&1 | .\Write-Info.ps1

    .\Write-Info.ps1 "NETSH ras set conf ENABLED"
    CMD /C NETSH ras set conf ENABLED 2>&1 | .\Write-Info.ps1

    .\Write-Info.ps1 "NET stop RemoteAccess"
    CMD /C NET stop RemoteAccess 2>&1 | .\Write-Info.ps1

    StartService "sstpsvc"
    StartService "rasman"
    StartService "wanarpv6"
    CMD /C SC config RemoteAccess start=Auto 2>&1 | .\Write-Info.ps1    
    StartService "RemoteAccess" 

    .\Write-Info.ps1 "Check and start Active Directory Domain Services"
    StartService "NTDS"

    .\Write-Info.ps1 "Check and start Active Directory Web Services"
    StartService "ADWS"
}

#----------------------------------------------------------------------------
# Start checker
#----------------------------------------------------------------------------
if($action -eq "StartChecker")
{
    StartService "sstpsvc"
    StartService "rasman"
    StartService "wanarpv6"
    StartService "RemoteAccess"

    .\Write-Info.ps1 "Check and start Active Directory Domain Services"
    StartService "NTDS"

    .\Write-Info.ps1 "Check and start Active Directory Web Services"
    StartService "ADWS"

    .\Write-Info.ps1 "Finish checker."
    Sleep 5 # To display above messages
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Pop-Location
Stop-Transcript
exit 0