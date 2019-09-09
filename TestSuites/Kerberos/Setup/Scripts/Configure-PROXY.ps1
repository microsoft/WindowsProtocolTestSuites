#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Configure-PROXY.ps1
## Purpose:        Configure the proxy computer for Kerberos Server test suite.
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

    [string]$WorkingPath = "C:\temp"
)

$newEnvPath=$env:Path+";.\;.\scripts\"
$env:Path=$newEnvPath

#------------------------------------------------------------------------------------------
# Global Variables:
# ScriptFileFullPath: Full Path of this script file
# ScriptName:         File Name of this script file 
# SignalFileFullPath: Full Path of the completion signal file for this script file
# LogFileFullPath:    Full Path of the log file for this script file
# Parameters:         Parameters read from the config file
# DataFile:           Full Path of the kerberos config XML file
# KrbParams:          Parameters read from the DataFile
#------------------------------------------------------------------------------------------
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$SignalFileFullPath      = "$WorkingPath\Configure-PROXY.finished.signal"
$LogFileFullPath         = "$ScriptFileFullPath.log"
$DataFile                = "$WorkingPath\Scripts\ParamConfig.xml"
[xml]$KrbParams          = $null

#------------------------------------------------------------------------------------------
# Function: Display-Help
# Display the help messages.
#------------------------------------------------------------------------------------------
Function Display-Help()
{
    $helpmsg = @"
Post configuration script to configure the proxy computer for Kerberos Server test suite.

Usage:
    .\Configure-PROXY.ps1 [-WorkingPath <WorkingPath>] [-h | -help]

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
    if (!(Test-Path -Path $LogFileFullPath))
    {
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
    Write-ConfigLog "Getting the parameters from Kerberos config file..." -ForegroundColor Yellow
    if(Test-Path -Path $DataFile)
    {
        [xml]$Script:KrbParams = Get-Content -Path $DataFile
    }
    else
    {
        Write-ConfigLog "$DataFile not found. Will keep the default setting of all the test context info..."
    }
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
    if (Test-Path -Path $SignalFileFullPath)
    {
        Write-ConfigLog "The script execution has been completed." -ForegroundColor Red
        exit 0
    }

    # Switch to the script path
    Write-ConfigLog "Switching to $WorkingPath..." -ForegroundColor Yellow
    #Push-Location $WorkingPath

    # Update ParamConfig.xml
    UpdateConfigFile.ps1 -WorkingPath $WorkingPath

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
    RestartAndRun.ps1 -ScriptPath $ScriptFileFullPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
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
    RestartAndRunFinish.ps1
}

#------------------------------------------------------------------------------------------
# Function: Config-Proxy
# Configure the environment phase 1:
#  * Modify registry key of KDC Proxy Server service (KPS)
#  * Create self-signed certificate and bind
#------------------------------------------------------------------------------------------
Function Config-Proxy()
{
    #-----------------------------------------------------------------------------------------------
	# Install Web Server
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Installing web-server feature" -ForegroundColor Yellow
	Install-WindowsFeature -Name web-server -ErrorAction Stop

	#-----------------------------------------------------------------------------------------------
	# Modify registry key of KDC Proxy Server service (KPS)
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Modify registry key of KDC Proxy Server service (KPS)" -ForegroundColor Yellow
	reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\KPSSVC" /v "DisplayName" /t "REG_SZ" /d "KDC Proxy Server service (KPS)" /f
	reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\KPSSVC" /v "Start" /t "REG_DWORD" /d 0x00000002 /f
	reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\KPSSVC" /v "Type" /t "REG_DWORD" /d 0x00000120 /f
	reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\KPSSVC" /v "ObjectName" /t "REG_SZ" /d "LocalSystem" /f

	reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\KPSSVC\Settings" /v "DisallowUnprotectedPasswordAuth" /t "REG_DWORD" /d 0 /f
	reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\KPSSVC\Settings" /v "HttpsClientAuth" /t "REG_DWORD" /d 0 /f
	reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\KPSSVC\Settings" /v "HttpsUrlGroup" /t "REG_MULTI_SZ" /d "+:443" /f

	#-----------------------------------------------------------------------------------------------
	# Create self-signed certificate and bind
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Create self-signed certificate and bind" -ForegroundColor Yellow
	Import-Module WebAdministration
	$cert = New-SelfSignedCertificate -DnsName localhost -CertStoreLocation cert:\LocalMachine\My
	Push-Location IIS:\SslBindings
	$IP = "*"
	$certhash = $cert.GetCertHashString()
	New-WebBinding -Name "Default Web Site" -IP $IP -Port 443 -Protocol https
	Get-Item cert:\LocalMachine\MY\$certhash | new-item $IP!443
	Pop-Location
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
    Config-Proxy
	
	Complete-Configure
}

Main
