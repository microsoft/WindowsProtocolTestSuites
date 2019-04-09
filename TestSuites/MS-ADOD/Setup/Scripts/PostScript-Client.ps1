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
    [int]   $Step        = 1                                              # For rebooting, indicate which step the script is runing
)

#-----------------------------------------------------------------------------
# Global variables
#-----------------------------------------------------------------------------
$ScriptsSignalFile      = "$env:SystemDrive\PostScript.Completed.signal"        # Config signal file
$CurrentScriptPath      = $MyInvocation.MyCommand.Definition                    # Current Working Path
$LogPath                = "$env:HOMEDRIVE\Logs"                                 # Log Path
$LogFile                = "$LogPath\PostScript-Client.ps1.log"                  # Log File
[string]$configPath	 	= "$WorkingPath\protocol.xml"

Write-Host "Put current dir as $WorkingPath" -ForegroundColor Yellow
Push-Location $WorkingPath

#-----------------------------------------------------------------------------
# Prepare Work Before Script Start
#-----------------------------------------------------------------------------
Function Prepare(){

    Write-Host "Executing [PostConfig-Client.ps1] ..." -ForegroundColor Cyan

    # Check signal file
    if(Test-Path -Path $ScriptsSignalFile){
        Write-Host "The script execution is complete." -ForegroundColor Red
        exit 0
    }
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
    [xml]$content = Get-Content $configPath
    $clientSetting = $Content.lab.servers.vm | Where-Object {$_.role -eq "Client"}

    # Disable IPv6
    Write-Host "Disabling IPv6" -ForegroundColor Yellow
    .\Disable-IPv6.ps1

    if($Content.lab.core.environment -ne "Azure"){ #azure regression do not neet set network configuration.
        # Set Network
        Write-Host "Setting network configuration" -ForegroundColor Yellow    
        .\Set-NetworkConfiguration.ps1 -IPAddress $clientSetting.ip -SubnetMask $clientSetting.subnet -Gateway $clientSetting.gateway -DNS (($clientSetting.dns).split(';'))

        # Disable ICMP Redirect
        Write-Host "Disabling ICMP Redirect" -ForegroundColor Yellow
        .\Disable-ICMPRedirect.ps1

        # Set Network Location
        Write-Host "Setting network location to Private" -ForegroundColor Yellow
        .\Set-NetworkLocation.ps1 -NetworkLocation "Private"
    }

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
    Write-Host "Write signal file: PostScript.Completed.signal to system drive."
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

Pop-Location