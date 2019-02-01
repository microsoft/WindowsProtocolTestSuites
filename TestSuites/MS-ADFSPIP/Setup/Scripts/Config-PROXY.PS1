#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Sripting
## File         :   Config-Proxy.ps1
## Requirements :   Windows Powershell 3.0
## Supported OS :   Windows Server 2012
##
##############################################################################
Param
(
    [string]$WorkingPath = "C:\temp",                                    # Script working path
    [string]$certDir="$WorkingPath\Data\Certificates"

)

$param                   = @{}
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$ScriptPath              = Split-Path $ScriptFileFullPath
$LogFileFullPath         = "$ScriptFileFullPath.log"
$SignalFileFullPath      = "$WorkingPath\post.finished.signal"

Write-Host "Put current dir as $WorkingPath" -ForegroundColor Yellow
Push-Location $WorkingPath

#-----------------------------------------------------------------------------
# Function: Prepare
# Usage   : Start executing the script; Push directory to working directory
# Params  : 
# Remark  : 
#-----------------------------------------------------------------------------
Function Prepare()
{
    Write-Host "Executing [$ScriptName] ..." -ForegroundColor Cyan

    # Check signal file
    if(Test-Path -Path $SignalFileFullPath)
    {
        Write-Host "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    #Check working path exists or not.
    if(!(Test-Path -Path $WorkingPath))
    {
    Write-Host "Error:'$WorkingPath' was not found,Please check the working path parameter you set is right or not."  -foregroundcolor Red
    Write-Host "Warning:This script was not executed completely because of the error above." -foregroundcolor Yellow
    exit 0
    }
}

#------------------------------------------------------------------------------------------
# Function: Read-ConfigParameters
# Read Config Parameters
#------------------------------------------------------------------------------------------
Function Read-ConfigParameters()
{
    $protocolXMLPath ="$WorkingPath\Scripts\Protocol.xml"
    $VMName =  .\Scripts\GetVMNameByComputerName.ps1 -ConfigFile $protocolXMLPath
    Write-ConfigLog "Getting the parameters from config file..." -ForegroundColor Yellow
    .\Scripts\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$param) -ConfigFile $protocolXMLPath
    $param
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

Function Complete-Configure
{
    # Write signal file
    Write-ConfigLog "Write signal file`: post.finished.signal to hard drive."
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

    Import-Module .\Scripts\ADFSLib.PSM1

    # Read the config parameters
    Read-ConfigParameters
}

#------------------------------------------------------------------------------------------
# Function: Config-Environment
# Control the overall workflow of all configuration phases
#------------------------------------------------------------------------------------------
Function Config-Environment
{
    # Start configure
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    Set-AutoLogon -Domain $param["name"] -Username $param["username"] -Password $param["password"] -Count 999

    # Turn off UAC
    Write-ConfigLog "Turn off UAC..." -ForegroundColor Yellow
    Set-ItemProperty -path  HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System -name "EnableLUA" -value "0"

    Write-Host "Install Web Application Proxy"
    Install-WebApplicationProxy -ErrorAction Stop

    #Check certificates path exists or not.
    if(!(Test-Path -Path $certDir))
    {
    Write-Host "Error:certificates path '$certDir' was not found."  -foregroundcolor Red
    Write-Host "Warning:This script was not executed completely because of the error above." -foregroundcolor Yellow
    exit 0
    }

    Write-Host "Import CA root certificate"
    Import-Certificate -FilePath "$certDir\CARoot.cer" -CertStoreLocation Cert:\LocalMachine\Root -Confirm:$false

    Write-Host "Import mock client certificate"
    Import-Certificate -FilePath "$certDir\Client.cer" -CertStoreLocation Cert:\LocalMachine\Root -Confirm:$false

    Write-Host "Import ADFS certificate"
    Import-PfxCertificate -FilePath "$certDir\Adfs.pfx" -Password $(ConvertTo-SecureString -AsPlainText "123" -Force) `
					      -CertStoreLocation Cert:\LocalMachine\My -Confirm:$false

    Write-Host "Import WebApp certificate"
    Import-PfxCertificate -FilePath "$certDir\WebApp.pfx" -Password $(ConvertTo-SecureString -AsPlainText "123" -Force) `
					      -CertStoreLocation Cert:\LocalMachine\My -Confirm:$false
					  
    Write-Host "Enable remoting"
    Enable-Remoting

    Write-Host "Flush DNS"
    ipconfig /flushdns
}


#------------------------------------------------------------------------------------------
# Main Function
#------------------------------------------------------------------------------------------
Function Main
{
    Prepare

    # Initialize configure environment
    Init-Environment    

    # Start configure
    Config-Environment

    # Complete configure
    Complete-Configure
}

Main
