# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#############################################################################
##
## Microsoft Windows Powershell Sripting
## File         :   PostConfig-SUT.ps1
## Requirements :   Windows Powershell 3.0
## Supported OS :   Windows Server 2012R2
##
##############################################################################

Param
(
    [string]$WorkingPath = (split-path -parent ([System.IO.Path]::GetDirectoryName($myInvocation.MyCommand.Definition)))
)

$param                   = @{}
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$LogFileFullPath         = "$ScriptFileFullPath.log"
$SignalFileFullPath      = "$env:SystemDrive\PostScript.Completed.signal"

# Check WorkingPath
if($WorkingPath -eq "$env:SystemDrive\")
{
    Write-ConfigLog "Make path to C:\Temp to run script" -ForegroundColor Yellow
    $WorkingPath = $WorkingPath + "Temp"
    Write-ConfigLog "WorkingPath is : $WorkingPath" -ForegroundColor Yellow
    $SignalFileFullPath = "$WorkingPath\post.finished.signal"
}

$configPath				 = "$WorkingPath\protocol.xml"

# Switch to the working path
Write-ConfigLog "Switching to $WorkingPath..." -ForegroundColor Yellow
Push-Location $WorkingPath

[xml]$Content = Get-Content $configPath
$sutSetting = $Content.lab.servers.vm | Where-Object {$_.role -eq "SUT"}
$coreSetting = $Content.lab.core

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

Function Complete-Configure
{
    # Write signal file
    Write-ConfigLog "Write signal file`: $SignalFileFullPath to hard drive."
    cmd /C ECHO CONFIG FINISHED > $SignalFileFullPath

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
Function Init-Environment()
{
    # Start logging
    Start-ConfigLog

    # Start executing the script
    Write-ConfigLog "Executing [$ScriptName]..." -ForegroundColor Cyan
}

#------------------------------------------------------------------------------------------
# Function: Config-Environment
# Control the overall workflow of all configuration phases
#------------------------------------------------------------------------------------------
Function Config-Environment
{
	[string] $domain = $sutSetting.domain
	[string] $userName = $coreSetting.username
	[string] $userPwd = $coreSetting.password

    # Start configure
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    .\Set-AutoLogon -Domain $domain -Username $userName -Password $userPwd -Count 999

    # Turn off UAC
    Write-ConfigLog "Turn off UAC..." -ForegroundColor Yellow
    Set-ItemProperty -path  HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System -name "EnableLUA" -value "0"

    Write-Host "Enable WinRM"
    if(Test-WSMan -ComputerName $sutSetting.ip){
        Write-Host "WinRM is running"
    }else{
        .\Enable-WinRM.ps1
    }
    

	# Enable Remote Desktop
	(Get-WmiObject Win32_TerminalServiceSetting -Namespace root\cimv2\TerminalServices).SetAllowTsConnections(1,1) | Out-Null
	(Get-WmiObject -Class "Win32_TSGeneralSetting" -Namespace root\cimv2\TerminalServices -Filter "TerminalName='RDP-tcp'").SetUserAuthenticationRequired(0) | Out-Null
	Get-NetFirewallRule -DisplayName "Remote Desktop*" | Set-NetFirewallRule -enabled true

	# Configure Network detection on RDP Server
    Set-ItemProperty -path "HKLM:\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services" -name "SelectNetworkDetect" -value "0"
    
    # This value can enable the group policy: "Require use of specific security layer for remote (RDP) connections" to "Negotiate".
    Set-ItemProperty -path "HKLM:\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services" -name "SecurityLayer" -value "1" -Type DWord
}


#------------------------------------------------------------------------------------------
# Main Function
#------------------------------------------------------------------------------------------
Function Main
{
    # Initialize configure environment
    Init-Environment

    # Start configure
    Config-Environment

    # Complete configure
    Complete-Configure
}

Main

Pop-Location