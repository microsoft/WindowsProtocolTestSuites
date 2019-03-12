#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Config-ClientComputer.ps1
## Purpose:        Configure Client Computer for MS-ADOD OD test suite.
## Requirements:   Windows PowerShell 2.0
## Supported OS:   Windows 7 or later versions
##
##############################################################################
Param(
    [String]$scriptsPath     = (Split-Path $MyInvocation.MyCommand.Definition -Parent)
)
$ScriptsSignalFile = "$env:HOMEDRIVE\ConfigScript.finished.signal"
if (Test-Path -Path $ScriptsSignalFile)
{
    Write-Host "The script execution is complete." -foregroundcolor Red
    exit 0
}

Write-Host "Put current dir as $scriptsPath."
Push-Location $scriptsPath

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
$settingFile = "$scriptsPath\ParamConfig.xml"
if(Test-Path -Path $settingFile)
{
    $clientLogPath         = .\Get-Parameter.ps1 $settingFile clientLogPath
    $clientLogFile         = $clientLogPath + "\Config-ClientComputer.ps1.log"
    $fullDomainName        = .\Get-Parameter.ps1 $settingFile fullDomainName
    $domainAdminUserName   = .\Get-Parameter.ps1 $settingFile domainAdminUserName
    $domainAdminUserPwd    = .\Get-Parameter.ps1 $settingFile domainAdminUserPwd
    $pdcOperatingSystem    = .\Get-Parameter.ps1 $settingFile pdcOperatingSystem
    $pdcComputerName       = .\Get-Parameter.ps1 $settingFile pdcComputerName
    $pdcIP                 = .\Get-Parameter.ps1 $settingFile pdcIP
    $clientOperatingSystem = .\Get-Parameter.ps1 $settingFile clientOperatingSystem
    $clientComputerName    = .\Get-Parameter.ps1 $settingFile clientComputerName
    $clientIP              = .\Get-Parameter.ps1 $settingFile clientIP
    $clientAdminUserName   = .\Get-Parameter.ps1 $settingFile clientAdminUserName
    $clientAdminUserPwd    = .\Get-Parameter.ps1 $settingFile clientAdminUserPwd
    $ipVersion             = .\Get-Parameter.ps1 $settingFile ipVersion
    .\Set-Parameter.ps1 $settingFile clientLogFile $clientLogFile "If no log file path specified, this value should be used."
}
else
{
    Write-Host "$settingFile not found. Will keep the default setting of all the test context info..."
}

#-----------------------------------------------------
# Create $logPath if not exist
#-----------------------------------------------------
if (!(Test-Path -Path $clientLogPath))
{
    New-Item -Type Directory -Path $clientLogPath -Force
}

#-----------------------------------------------------
# Create $logFile if not exist
#-----------------------------------------------------
if (!(Test-Path -Path $clientLogFile))
{
    New-Item -Type File -Path $clientLogFile -Force
}
Start-Transcript $clientLogFile -Append

#-----------------------------------------------------
# Write value for all the parameters
#-----------------------------------------------------
Write-Host "EXECUTING [Config-ClientComputer.ps1] ..." -foregroundcolor cyan
Write-Host "`$scriptsPath           = $scriptsPath"
Write-Host "`$clientLogPath         = $clientLogPath"       
Write-Host "`$clientLogFile         = $clientLogFile"
Write-Host "`$fullDomainName        = $fullDomainName" 
Write-Host "`$domainAdminUserName   = $domainAdminUserName"
Write-Host "`$domainAdminUserPwd    = $domainAdminUserPwd"
Write-Host "`$pdcOperatingSystem    = $pdcOperatingSystem"
Write-Host "`$pdcComputerName       = $pdcComputerName"
Write-Host "`$pdcIP                 = $pdcIP" 
Write-Host "`$clientOperatingSystem = $clientOperatingSystem" 
Write-Host "`$clientComputerName    = $clientComputerName"
Write-Host "`$clientIP              = $clientIP"
Write-Host "`$clientAdminUserName   = $clientAdminUserName"
Write-Host "`$clientAdminUserPwd    = $clientAdminUserPwd"
Write-Host "`$ipVersion             = $ipVersion"

#-----------------------------------------------------
# Begin to config Client Computer
#-----------------------------------------------------

#-------------------------------------
# Turn off window firewall
#-------------------------------------
Write-Host "Turn off firewall"
cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Host

#-----------------------------------------------------
# Start the Windows Remote Management Service
#-----------------------------------------------------
Write-Host "Enable Windows Remote Management Service on Server..."
.\Enable-RemoteSessionInServer.ps1 

#-----------------------------------------------------
# Store Useful Information from Client Computer for configuring Driver Computer
#-----------------------------------------------------
Write-Host "Get the Installation Path for Test Scripts..."
if(-not (Test-Path -Path "$env:HOMEDRIVE\MSIInstalled.signal"))
{
	$MSIScriptsFile = [System.IO.Directory]::GetFiles("$env:HOMEDRIVE\MicrosoftProtocolTests", "ParamConfig.xml", [System.IO.SearchOption]::AllDirectories)
	[string]$MSIFullPath = [System.IO.Directory]::GetParent($MSIScriptsFile)
	"$MSIFullPath" | out-file "$env:HOMEDRIVE\MSIInstalled.signal"
}
#-----------------------------------------------------
# Set Group Policy to Start Remote Management Service when startup
#-----------------------------------------------------
Write-Host "Set Group Policy for Startup Script to Enable Remote Session at every startup..."
cmd /c regedit /s .\addStartupScript.reg
$regVal = "$MSIFullPath\Enable-RemoteSessionInServer.ps1"
$regKey = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Startup\0\0"
if(Test-Path -path $regKey) {
  Set-ItemProperty -path $regKey -name Script -value $regVal
}
$regKey = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy\State\Machine\Scripts\Startup\0\0"
if(Test-Path -path $regKey) {
  Set-ItemProperty -path $regKey -name Script -value $regVal
}
$regKey = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Startup\0\0"
if(Test-Path -path $regKey) {
  Set-ItemProperty -path $regKey -name Script -value $regVal
}
$regKey = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Group Policy\State\Machine\Scripts\Startup\0\0"
if(Test-Path -path $regKey) {
  Set-ItemProperty -path $regKey -name Script -value $regVal
}

#-----------------------------------------------------
# Finished to config client computer
#-----------------------------------------------------
Pop-Location
Write-Host "Write signal file: ConfigScript.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-ClientComputer.ps1] FINISHED (NOT VERIFIED)."

Stop-Transcript

exit 0
