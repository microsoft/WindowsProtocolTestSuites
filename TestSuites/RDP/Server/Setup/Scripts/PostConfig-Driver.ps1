# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Sripting
# File         :   PostConfig-Driver.ps1
# Requirements :   Windows Powershell 3.0
# Supported OS :   Windows Server 2012R2 or later
#
##############################################################################

Param
(
    [string] $domain,
    [string] $userName,
    [string] $userPwd
)

$isAutomation            = $false
$WorkingPath      	     = "C:\Temp"
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$LogFileFullPath         = "$ScriptFileFullPath.log"
$SignalFileFullPath      = "$WorkingPath\post.finished.signal"
$driverName              = $env:computername
$driverIp                = (Test-Connection $driverName -count 1 | select -ExpandProperty Ipv4Address).IPAddressToString

[string]$configPath	 	 = "$WorkingPath\protocol.xml"

# Switch to the working path
if (Test-Path "$WorkingPath") {
    # Switch to the working path
    Write-Host "Switching to $WorkingPath..." -ForegroundColor Yellow
    Push-Location $WorkingPath
}

if (Test-Path "$configPath") {
    [xml]$Content = Get-Content $configPath
    $driverComputerSetting = $Content.lab.servers.vm | where { $_.role -eq "DriverComputer" }
    $coreSetting = $Content.lab.core

    if(![string]::IsNullOrEmpty($coreSetting.regressiontype) -and ($coreSetting.regressiontype -eq "Azure")){
        $SignalFileFullPath = "$env:SystemDrive\PostScript.Completed.signal"
    }

    $domain = $driverComputerSetting.domain
    $userName = $coreSetting.username
    $userPwd = $coreSetting.password
    $driverIp = $driverComputerSetting.ip
    $isAutomation = $true
}

#------------------------------------------------------------------------------------------
# Function: Start-ConfigLog
# Create log file and start logging
#------------------------------------------------------------------------------------------
Function Start-ConfigLog() {
    if (!(Test-Path -Path $LogFileFullPath)) {
        New-Item -ItemType File -path $LogFileFullPath -Force
    }
    Start-Transcript $LogFileFullPath -Append 2>&1 | Out-Null
}

#------------------------------------------------------------------------------------------
# Function: Write-ConfigLog
# Write information to log file
#------------------------------------------------------------------------------------------
Function Write-ConfigLog {
    Param (
        [Parameter(ValueFromPipeline = $true)] $text,
        $ForegroundColor = "Green"
    )

    $date = Get-Date -f MM-dd-yyyy_HH_mm_ss
    Write-Output "[$date] $text"
}

Function Complete-Configure {
    if($isAutomation)
    {
        # Write signal file
        Write-ConfigLog "Write signal file`: $SignalFileFullPath to hard drive."
        cmd /C ECHO CONFIG FINISHED > $SignalFileFullPath
    }    

    # Ending script
    Write-ConfigLog "Config finished."
    Write-ConfigLog "EXECUTE [$ScriptName] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    Restart-Computer;
}

#------------------------------------------------------------------------------------------
# Function: Init-Environment
# Start logging, check signal file, switch to script path and read the config parameters
#------------------------------------------------------------------------------------------
Function Init-Environment() {
    # Start logging
    Start-ConfigLog

    # Start executing the script
    Write-ConfigLog "Executing [$ScriptName]..." -ForegroundColor Cyan
}

#------------------------------------------------------------------------------------------
# Function: Config-Environment
# Control the overall workflow of all configuration phases
#------------------------------------------------------------------------------------------
Function Config-Environment {
    # Start configure
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    .\Set-AutoLogon -Domain $domain -Username $userName -Password $userPwd -Count 999

    # Turn off UAC
    Write-ConfigLog "Turn off UAC..." -ForegroundColor Yellow
    Set-ItemProperty -path  HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System -name "EnableLUA" -value "0"

    # Write-Host "Enable WinRM"
    Write-Host "Enable WinRM"
    if (Test-WSMan -ComputerName $driverIp) {
        Write-Host "WinRM is running"
    }
    else {
        .\Enable-WinRM.ps1
    }
}

#------------------------------------------------------------------------------------------
# Main Function
#------------------------------------------------------------------------------------------
Function Main {
    # Initialize configure environment
    Init-Environment

    # Start configure
    Config-Environment

    # Complete configure
    Complete-Configure
}

Main

Pop-Location