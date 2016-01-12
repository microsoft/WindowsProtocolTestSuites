#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
#############################################################################
##
## Microsoft Windows Powershell Sripting
## File:           Config-PROXY01.ps1
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012 or later versions
##
##############################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$WorkingPath = "c:\temp"
$env:Path += ";c:\temp;c:\temp\Scripts"

#-----------------------------------------------------------------------------------------------
# Check if the script was executed
#-----------------------------------------------------------------------------------------------
$ScriptsSignalFile = "$env:HOMEDRIVE\config-proxy01.finished.signal"
if(Test-Path -Path $ScriptsSignalFile)
{
	Write-Info.ps1 "The script execution is complete." -foregroundcolor Red
	exit 0
}

#-----------------------------------------------------------------------------------------------
# Please run as Domain Administrator
# Starting script
#-----------------------------------------------------------------------------------------------
$dataFile = "$WorkingPath\Data\ParamConfig.xml"
if(Test-Path -Path $dataFile)
{
	[xml]$configFile = Get-Content -Path $dataFile
}
else
{
	Write-Info.ps1 "$dataFile not found.  Will keep the default setting of all the test context info..."
}

#-----------------------------------------------------------------------------------------------
# Create $logPath if not exist
#-----------------------------------------------------------------------------------------------
$logPath = $configFile.Parameters.LogPath
if(!(Test-Path -Path $logPath))
{
	New-Item -Type Directory -Path $logPath -Force
}

#-----------------------------------------------------------------------------------------------
# Create $logFile if not exist
#-----------------------------------------------------------------------------------------------
$logFile = $WorkingPath + "\" + $MyInvocation.MyCommand.Name + ".log"
if(!(Test-Path -Path $logFile))
{
	New-Item -Type File -Path $logFile -Force
}
Start-Transcript $logFile -Append

#-----------------------------------------------------------------------------------------------
# Write value for all the parameters
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "EXECUTING [Config-PROXY01.ps1] ..." -foregroundcolor cyan
Write-Info.ps1 "`$logPath = $logPath"
Write-Info.ps1 "`$logFile = $logFile"

#-----------------------------------------------------------------------------------------------
# Begin to config PROXY01 computer
#-----------------------------------------------------------------------------------------------

#-----------------------------------------------------------------------------------------------
# Turn off windows firewall
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Turn off firewall"
cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Info.ps1

#-----------------------------------------------------------------------------------------------
# Install Web Server
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Installing web-server feature" -ForegroundColor Yellow
Install-WindowsFeature -Name web-server -ErrorAction Stop

#-----------------------------------------------------------------------------------------------
# Modify registry key of KDC Proxy Server service (KPS)
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Modify registry key of KDC Proxy Server service (KPS)" -ForegroundColor Yellow
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
Write-Info.ps1 "Create self-signed certificate and bind" -ForegroundColor Yellow
Import-Module WebAdministration
$cert = New-SelfSignedCertificate -DnsName localhost -CertStoreLocation cert:\LocalMachine\My
Push-Location IIS:\SslBindings
$IP = "*"
$certhash = $cert.GetCertHashString()
New-WebBinding -Name "Default Web Site" -IP $IP -Port 443 -Protocol https
Get-Item cert:\LocalMachine\MY\$certhash | new-item $IP!443
Pop-Location

#-----------------------------------------------------------------------------------------------
# Finished to config PROXY01 computer
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Write signal file: config-proxy01.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile
Stop-Transcript
