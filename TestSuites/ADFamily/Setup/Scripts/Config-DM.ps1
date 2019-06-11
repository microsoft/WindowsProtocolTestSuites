###########################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
###########################################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-DM.ps1
## Purpose:        Configure Domain Member Server in Local Domain for Active Directory Family Test Suite.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016, and later.
##
###########################################################################################

#------------------------------------------------------------------------------------------
# Parameters:
# Help: whether to display the help information
# Step: Current step for configuration
#------------------------------------------------------------------------------------------
Param
(
    [alias("h")]
    [switch]
    $Help,

    [switch]
    $EnableDebugging,
    
    [int]
    $Step = 1,

    [String]
    $ConfigFile = "c:\temp\Protocol.xml"
)

#------------------------------------------------------------------------------------------
# Global Variables:
# ScriptFileFullPath: Full Path of this script file
# ScriptName:         File Name of this script file 
# ScriptPath:         File Path of this script file
# SignalFileFullPath: Full Path of the completion signal file for this script file
# LogFileFullPath:    Full Path of the log file for this script file
# Parameters:         Parameters read from the config file
#------------------------------------------------------------------------------------------
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$ScriptPath              = Split-Path $ScriptFileFullPath
$SignalFileFullPath      = "$ScriptPath\post.finished.signal"
$LogFileFullPath         = "$ScriptFileFullPath.log"
$Parameters              = @{}
$IsAzure                 = $false

try {
    [xml]$content = Get-Content $ConfigFile -ErrorAction Stop
    $currentCore = $content.lab.core
    if(![string]::IsNullOrEmpty($currentCore.regressiontype) -and ($currentCore.regressiontype -eq "Azure")){
        $IsAzure = $true;
        $SignalFileFullPath = "C:\PostScript.Completed.signal"
    }
}
catch {
    
}

#------------------------------------------------------------------------------------------
# Function: Display-Help
# Display the help messages.
#------------------------------------------------------------------------------------------
Function Display-Help()
{
    $helpmsg = @"
Post configuration script to configure Domain Member Server in Local Domain for Active Directory Family Test Suite.

Usage:
    .\Config-DM.ps1 [-Step <step>] [-h | -help]

Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg "`r`n"
    exit 0
}

#------------------------------------------------------------------------------------------
# Function: Start-ConfigLog
# Create log file and start logging
#------------------------------------------------------------------------------------------
Function Start-ConfigLog()
{
    if(!(Test-Path -Path $LogFileFullPath)){
        New-Item -ItemType File -path $LogFileFullPath -Force
    }
    Start-Transcript $LogFileFullPath -Append 2>&1 | Out-Null
}

#------------------------------------------------------------------------------------------
# Function: Write-ConfigLog
# Write information to log file
#------------------------------------------------------------------------------------------
Function Write-ConfigLog
{
    Param (
        [Parameter(ValueFromPipeline=$true)] $text,
        $ForegroundColor = "Green"
    )

    $date = Get-Date -f MM-dd-yyyy_HH_mm_ss
    Write-Output "[$date] $text"
}

#------------------------------------------------------------------------------------------
# Function: Read-ConfigParameters
# Read Config Parameters
#------------------------------------------------------------------------------------------
Function Read-ConfigParameters()
{
    Write-ConfigLog "Getting the parameters from config file..." -ForegroundColor Yellow
    if($IsAzure)
    {
        .\GetVmParametersByComputerName.ps1 -RefParamArray ([ref]$Parameters)
    } else {
    $VMName = .\GetVMNameByComputerName.ps1
        .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$Parameters)
    }
    $Parameters
}

#------------------------------------------------------------------------------------------
# Function: Init-Environment
# Start logging, check signal file, switch to script path and read the config parameters
#------------------------------------------------------------------------------------------
Function Init-Environment()
{
    # Start logging
    Start-ConfigLog

    # Start executing the script
    Write-ConfigLog "Executing [$ScriptName]..." -ForegroundColor Cyan

    # Check completion signal file. If signal file exists, exit with 0
    if(Test-Path -Path $SignalFileFullPath){
        Write-ConfigLog "The script execution has been completed." -ForegroundColor Red
        exit 0
    }

    # Switch to the script path
    Write-ConfigLog "Switching to $ScriptPath..." -ForegroundColor Yellow
    Push-Location $ScriptPath

    # Read the config parameters
    Read-ConfigParameters
}

#------------------------------------------------------------------------------------------
# Function: Proceed-ScriptWithRestart
# Restart computer and proceed the script to the next step
#------------------------------------------------------------------------------------------
Function Proceed-ScriptWithRestart
{
    $NextStep = $Step + 1

    # add a schedule task to execute the script next step after restart
    .\RestartAndRun.ps1 -ScriptPath $ScriptFileFullPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
}

#------------------------------------------------------------------------------------------
# Function: Remove-ScriptFromTask
# Remove the sceduled task to execute the script next step
#------------------------------------------------------------------------------------------
Function Remove-ScriptFromTask
{
    # remove the schedule task to execute the script next step after restart
    .\RestartAndRunFinish.ps1
}

#------------------------------------------------------------------------------------------
# Function: Complete-Configure
# Write signal file, stop the transcript logging and remove the scedule task
#------------------------------------------------------------------------------------------
Function Complete-Configure
{
    # Write signal file
    Write-ConfigLog "Write signal file`: post.finished.signal to hard drive."
    cmd /C ECHO CONFIG FINISHED > $SignalFileFullPath

    # Ending script
    Write-ConfigLog "Config finished."
    Write-ConfigLog "EXECUTE [$ScriptName] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    # remove the schedule task to execute the script next step after restart
    .\RestartAndRunFinish.ps1
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase1
# Configure the environment phase 1:
# 1.  Set execution policy as unrestricted
# 2.  Set network configurations, for example, ip addresses, subnet mask, gateway and dns
# 3.  Set autologon
# 4.  [MS-ADTS-Security] Turn off UAC
# 5.  Join this computer to the local domain
#------------------------------------------------------------------------------------------
Function Config-Phase1()
{
    Write-ConfigLog "Entering Phase 1..."

    # Set execution policy as unrestricted
    Write-ConfigLog "Setting execution policy..." -ForegroundColor Yellow
    .\Set-ExecutionPolicy-Unrestricted.ps1
    if(-not $IsAzure)
    {
        # Set network configurations
        Write-ConfigLog "Setting network configurations..." -ForegroundColor Yellow
        .\Set-NetworkConfiguration.ps1 -IPAddress $Parameters["ip"] -SubnetMask $Parameters["subnet"] -Gateway $Parameters["gateway"] -DNS ($Parameters["dns"].Split(';'))
    }

    # Set autologon
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    .\Set-AutoLogon.ps1 -Domain $Parameters["domain"] -Username $Parameters["username"] -Password $Parameters["password"]

    # Turn off UAC
    Write-ConfigLog "Turn off UAC..." -ForegroundColor Yellow
    .\Turnoff-UAC.ps1

    # Join Local Domain
    Write-ConfigLog "Join this computer to the local domain..." -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["primarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["secondarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]

    # Kept receiving 53, wait for DC to stabilize during domain join
    .\Join-Domain.ps1 -domainWorkgroup "Domain" -domainName $Parameters["domain"] -userName $Parameters["username"] -userPassword $Parameters["password"] -testResultsPath $ScriptPath | Write-Output
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase2
# Configure the environment phase 2:
# 1.  Turn off firewall
# 2.  Set password change, netlogon, create object rights in registry key and group policy: 
#      (a) [MS-DRSR][MS-FRS2] disable auto password change (Registry Key)
#      (b) [MS-NRPC] refuse password change request (Registry Key)
#      (c) [MS-NRPC] configure netlogon service to depend on the DNS service (Registry Key)
#      (d) [MS-DRSR] disable auto password change (Group Policy)
#      (e) [MS-ADTS-LDAP] set only domain admins group can create computer object (Group Policy)
# 3.  [MS-DRSR] Set computer password for this domain member server to the first value, which will be changed to the second value in the next phase
#------------------------------------------------------------------------------------------
Function Config-Phase2()
{
    Write-ConfigLog "Entering Phase 2..."

    # Turn off firewall
    Write-ConfigLog "Turn off firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Set password change, netlogon, rights in registry key and group policy
    Write-ConfigLog "Setting password change, netlogon, create object rights in registry key and group policy..." -ForegroundColor Yellow
    .\Set-NetlogonRegKeyAndPolicy.ps1

    # Set computer password
    Write-ConfigLog "Setting computer password to the first value, which will be changed to the second value in the next phase..." -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["primarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["secondarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\Set-AdComputerPassword.ps1 -Domain $Parameters["domain"] -Name $Parameters["name"] -Password $Parameters["temppassword"] -DcName $Parameters["primarydc"]
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase3
# Configure the environment phase 3:
# 1.  [MS-DRSR] Set computer password for this domain member server to the second value
# 2.  Get and log operating system version
# 3.  Register Windbg dbgsrv for debugging purpose
#------------------------------------------------------------------------------------------
Function Config-Phase3()
{
    Write-ConfigLog "Entering Phase 3..."

    # Set computer password
    Write-ConfigLog "Setting computer password..." -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["primarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\WaitFor-ComputerReady.ps1 -computerName $Parameters["secondarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
    .\Set-AdComputerPassword.ps1 -Domain $Parameters["domain"] -Name $Parameters["name"] -Password $Parameters["password"] -DcName $Parameters["primarydc"]

    # Get OS Version
    Write-ConfigLog "Getting Operating System Version..." -ForegroundColor Yellow
    $OsVersion = .\Get-OsVersion.ps1 -log

    # Register Windbg dbgsrv
    if($EnableDebugging)
    {
        Write-ConfigLog "Registering Windbg dbgsrv..." -ForegroundColor Yellow
        .\Register-DbgSrv.ps1
    }
}

#------------------------------------------------------------------------------------------
# Function: Config-Environment
# Control the overall workflow of all configuration phases
#------------------------------------------------------------------------------------------
Function Config-Environment
{
    # Start configure
    switch($Step)
    {
        1 { Config-Phase1; Proceed-ScriptWithRestart; }
        2 { Config-Phase2; Proceed-ScriptWithRestart; }
        3 { Config-Phase3; Proceed-ScriptWithRestart; }
        4 { Complete-Configure; }
        default
        {
            Write-ConfigLog "Fail to execute the script!" -ForegroundColor Red
            break
        }
    }
}

#------------------------------------------------------------------------------------------
# Main Function
#------------------------------------------------------------------------------------------
Function Main
{
    # Display help information
    if($Help)
    {
        Display-Help
        return
    }

    # Initialize configure environment
    Init-Environment    

    # Complete configure
    Config-Environment
}

Main