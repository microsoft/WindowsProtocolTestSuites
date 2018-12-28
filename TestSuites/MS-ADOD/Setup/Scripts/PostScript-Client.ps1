#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Sripting
## File:           PostScript-Client.ps1
## Purpose:        Configure Client for MS-ADOD test suite.
## Requirements:   Windows PowerShell 2.0
## Supported OS:   Windows Server 2012 or later versions
##
##############################################################################

Param
(
    [string]$WorkingPath = "C:\temp",                                     # Script working path
    [bool]  $AutoReboot  = $false,                                        # Reboot when necessary without key pressing
    [int]   $Step        = 1                                              # For rebooting, indicate which step the script is runing
)

#-----------------------------------------------------------------------------
# Global variables
#-----------------------------------------------------------------------------
$ScriptsSignalFile  = "$env:HOMEDRIVE\PostConfig-SUT.ps1.finished.signal" # Config signal file
$ParamArray         = @{}                                                 # Parameters from the config file
$CurrentScriptPath  = $MyInvocation.MyCommand.Definition                  # Current Working Path
$LogPath            = "$env:HOMEDRIVE\Logs"                               # Log Path
$LogFile            = "$LogPath\PostConfig-SUT.ps1.log"                   # Log File

#-----------------------------------------------------------------------------
# Prepare Work Before Script Start
#-----------------------------------------------------------------------------
Function Prepare(){

    Write-Host "Executing [PostConfig-SUT.ps1] ..." -ForegroundColor Cyan

    # Check signal file
    if(Test-Path -Path $ScriptsSignalFile){
        Write-Host "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    # Change to absolute path
    Write-Host "Current path is $CurrentScriptPath" -ForegroundColor Cyan
    $WorkingPath = (Get-Item $WorkingPath).FullName

    Write-Host "Put current dir as $WorkingPath" -ForegroundColor Yellow
    Push-Location $WorkingPath
}

#-----------------------------------------------------------------------------
# Read Config Parameters
#-----------------------------------------------------------------------------
Function ReadConfig()
{
    $VMName =  .\GetVMNameByComputerName.ps1
    Write-Host "Getting the parameters from config file ..." -ForegroundColor Yellow
    .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$ParamArray)
    $ParamArray
}

#-----------------------------------------------------------------------------
# Create Log 
#-----------------------------------------------------------------------------
Function SetLog(){

    if(!(Test-Path -Path $LogPath)){
        New-Item -ItemType Directory -Path $LogPath -Force
    }

    if(!(Test-Path -Path $LogFile)){
        New-Item -ItemType File -path $LogFile -Force
    }
    Start-Transcript $LogFile -Append 2>&1 | Out-Null
}

#-----------------------------------------------------------------------------
# Restart and Resume
#-----------------------------------------------------------------------------
Function RestartAndResume
{
    $NextStep = $Step + 1

    .\RestartAndRun.ps1 -ScriptPath $CurrentScriptPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
}

#-----------------------------------------------------------------------------
# Phase1: SetNetworkConfiguration; DisableICMPRedirect; DisableIPv6; SetNetworkLocation; GetInstalledScriptPath; SetStartupScript
#-----------------------------------------------------------------------------
Function Phase1
{
    # Set Network
    Write-Host "Setting network configuration" -ForegroundColor Yellow    
    .\Set-NetworkConfiguration.ps1 -IPAddress $ParamArray["ip"] -SubnetMask $ParamArray["subnet"] -Gateway $ParamArray["gateway"] -DNS ($ParamArray["dns"].split(';'))

    # Disable ICMP Redirect
    Write-Host "Disabling ICMP Redirect" -ForegroundColor Yellow
    .\Disable-ICMPRedirect.ps1

    # Disable IPv6
    Write-Host "Disabling IPv6" -ForegroundColor Yellow
    .\Disable-IPv6.ps1

    # Set Network Location
    Write-Host "Setting network location to Private" -ForegroundColor Yellow
    .\Set-NetworkLocation.ps1 -NetworkLocation "Private"

    # Get Installed Script Path
    Write-Host "Getting the Installed Script Path" -ForegroundColor Yellow
    $Path = .\Get-InstalledScriptPath.ps1 -Name "MS-ADOD"

    # Set Startup Script
    Write-Host "Setting Startup Script" -ForegroundColor Yellow
    .\SetStartupScript.ps1 -Script "$Path\Enable-RemoteSessionInServer.ps1"
}

#-----------------------------------------------------------------------------
# Finish Script
#-----------------------------------------------------------------------------
Function Finish
{
    # Write signal file
    Write-Host "Write signal file: config.finished.signal to system drive."
    cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

    # Ending script
    Write-Host "Config finished."
    Write-Host "EXECUTE [PostScript-Client.ps1] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    # Cancel RestartAndRun
    .\RestartAndRunFinish.ps1
}

#-------------------------------------------------------------------------------
# Function: Main
# Remark  : Main script starts here.
#-------------------------------------------------------------------------------
Function Main()
{
    Prepare
    ReadConfig
    SetLog

    switch ($Step)
    {
        1 { Phase1; RestartAndResume; }
        2 { Finish; }
        default 
        {
            Write-Host "Fail to execute the script" -ForegroundColor Red
            break
        }
    }
}

Main
