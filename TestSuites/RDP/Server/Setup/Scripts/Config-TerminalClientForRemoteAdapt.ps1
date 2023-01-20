# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
[String]$scriptsPath  = [System.IO.Path]::GetDirectoryName($myInvocation.MyCommand.Definition)
)

Push-Location $scriptsPath
$dataPath = "$scriptsPath\..\Data"

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
$settingFile = "$scriptsPath\ParamConfig.xml"
if(Test-Path -Path $settingFile)
{    
    $logPath            = .\Get-Parameter.ps1 $settingFile logPath
    $logFile            = $logPath + "\Config-TerminalClientForRemoteAdapt.ps1.log"
    $userNameInTC       = .\Get-Parameter.ps1 $settingFile userNameInTC
    $userPwdInTC        = .\Get-Parameter.ps1 $settingFile userPwdInTC
	$credSSPUser        = .\Get-Parameter.ps1 $settingFile CredSSPUser
    $credSSPPwd         = .\Get-Parameter.ps1 $settingFile CredSSPPwd
    $domainName         = .\Get-Parameter.ps1 $settingFile domainName
    $dcComputerName     = .\Get-Parameter.ps1 $settingFile dcComputerName
    $tcComputerName     = .\Get-Parameter.ps1 $settingFile tcComputerName
    $driverComputerName = .\Get-Parameter.ps1 $settingFile driverComputerName
    $listeningPort      = .\Get-Parameter.ps1 $settingFile RDPListeningPort
    $ipVersion          = .\Get-Parameter.ps1 $settingFile ipVersion
    $osVersion          = .\Get-Parameter.ps1 $settingFile osVersion
    $workgroupDomain    = .\Get-Parameter.ps1 $settingFile workgroupDomain
    $compressionInTC    = .\Get-Parameter.ps1 $settingFile compressionInTC
    .\Set-Parameter.ps1 $settingFile LogFile $logFile "If no log file path specified, this value should be used."
}
else
{
    Write-Host "$settingFile not found. Will keep the default setting of all the test context info..."
}

#-----------------------------------------------------
# Create $logPath if not exist
#-----------------------------------------------------
if (!(Test-Path -Path $logPath))
{
    New-Item -Type Directory -Path $logPath -Force
}

#-----------------------------------------------------
# Create $logFile if not exist
#-----------------------------------------------------
if (!(Test-Path -Path $logFile))
{
    New-Item -Type File -Path $logFile -Force
}
Start-Transcript $logFile -Append

#-----------------------------------------------------
# Write value for all the parameters
#-----------------------------------------------------
Write-Host "EXECUTING [Config-TerminalClientForRemoteAdapt.ps1] ..." -foregroundcolor cyan
Write-Host "`$scriptsPath        = $scriptsPath"
Write-Host "`$logPath            = $logPath"       
Write-Host "`$logFile            = $logFile"
Write-Host "`$userNameInTC       = $userNameInTC" 
Write-Host "`$userPwdInTC        = $userPwdInTC"
Write-Host "`$credSSPUser        = $credSSPUser" 
Write-Host "`$credSSPPwd         = $credSSPPwd"
Write-Host "`$domainName         = $domainName" 
Write-Host "`$dcComputerName     = $dcComputerName"
Write-Host "`$tcComputerName     = $tcComputerName" 
Write-Host "`$driverComputerName = $driverComputerName"
Write-Host "`$listeningPort      = $listeningPort"
Write-Host "`$ipVersion          = $ipVersion"     
Write-Host "`$osVersion          = $osVersion" 
Write-Host "`$workgroupDomain    = $workgroupDomain"
Write-Host "`$compressionInTC    = $compressionInTC"

#-----------------------------------------------------
# Begin to config terminal client
#-----------------------------------------------------

#-------------------------------------
# Turn off window firewall
#-------------------------------------
Write-Host "Turn off firewall"
cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Host

#-----------------------------------------------------
# Get IP address of the SUT computer
#-----------------------------------------------------
$tcComputerIP = (Test-Connection $tcComputerName -Count 1 | Select -ExpandProperty IPV4Address).IPAddressToString

#-----------------------------------------------------
# Enable Powershell Remoting
#-----------------------------------------------------
if(Test-WSMan -ComputerName $tcComputerIP){
    Write-Host "WinRM is running"
}else{
    Set-NetConnectionProfile -NetworkCategory Private -ErrorAction Ignore
    Enable-PSRemoting -Force
    Set-Item wsman:\localhost\client\trustedhosts *  -Force
    Restart-Service WinRM -Force
}

#-----------------------------------------------------
# Get IP address of the driver computer
#-----------------------------------------------------
$ipV4 = Test-Connection $driverComputerName -Count 1 | Select -ExpandProperty IPV4Address
$driverComputerIP = $ipV4.IPAddressToString

#-----------------------------------------------------
# Modify ChangeResolution.ps1 and ChangeOrientation.ps1
#-----------------------------------------------------
(Get-Content .\ChangeResolution_Template.ps1) | ForEach-Object {$_ -replace "ScriptPath", $scriptsPath} | Set-Content .\ChangeResolution.ps1
(Get-Content .\ChangeOrientation_Template.ps1) | ForEach-Object {$_ -replace "ScriptPath", $scriptsPath} | Set-Content .\ChangeOrientation.ps1

#-----------------------------------------------------
# Create Windows Tasks for SUT control adapter
#-----------------------------------------------------
Write-Host "Creating Tasks for SUT control adapter (for Windows Platform)..."
$taskUser= $userNameInTC
if ($workgroupDomain.ToUpper() -eq "DOMAIN")
{
    $taskUser = "$domainName\$taskUser"
}

Write-Host "Allow RDP connecting to unkown publisher for $driverComputerName..."
cmd /c reg add "HKCU\Software\Microsoft\Terminal Server Client" /v "AuthenticationLevelOverride" /t "REG_DWORD" /d 0 /f
cmd /c reg add "HKCU\Software\Microsoft\Terminal Server Client\LocalDevices" /v $driverComputerIP /t REG_DWORD /d 76 /F

#-----------------------------------------------------
# Edit registery.
#-----------------------------------------------------
Write-Host "Change Registry, Add a default user for Server"
New-Item -type Directory HKCU:\Software\Microsoft\"Terminal Server Client"\Servers -Force
New-Item -type Directory HKCU:\Software\Microsoft\"Terminal Server Client"\Servers\$driverComputerName -Force
if ($workgroupDomain.ToUpper() -eq "DOMAIN")
{
    $usernameHint = "$domainName\$taskUser"
}else
{
    $usernameHint = "$driverComputerName\$taskUser"
}
New-ItemProperty HKCU:\Software\Microsoft\"Terminal Server Client"\Servers\$driverComputerName UsernameHint -value $usernameHint -PropertyType string -Force

if($driverComputerIP -ne $driverComputerName)
{
    # Create registry key for IP address
	New-Item -type Directory HKCU:\Software\Microsoft\"Terminal Server Client"\Servers\$driverComputerIP -Force
	New-ItemProperty HKCU:\Software\Microsoft\"Terminal Server Client"\Servers\$driverComputerIP UsernameHint -value $usernameHint -PropertyType string -Force
}

# To avoid warning dialog
New-ItemProperty HKCU:\Software\Microsoft\"Terminal Server Client"\LocalDevices $driverComputerName -value 588 -PropertyType DWORD -Force

#-----------------------------------------------------
# Edit registery.
# Disable TLS 1.0 for client
#-----------------------------------------------------
Write-Host "Disable TLS 1.0 for client."
New-Item 'HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client' -Force | Out-Null
New-ItemProperty -path 'HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client' -name 'Enabled' -value 0 -PropertyType 'DWord' -Force | Out-Null
New-ItemProperty -path 'HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client' -name 'DisabledByDefault' -value '0xffffffff' -PropertyType 'DWord' -Force | Out-Null
Write-Host 'TLS 1.0 has been disabled.'

#-----------------------------------------------------
# Edit registery.
# Enable TLS 1.1 and TLS 1.2 for client
#-----------------------------------------------------
Write-Host "Change Registry, force client to use TLS 1.0"
New-Item -type Directory HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.1" -Force
New-Item -type Directory HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.1"\Client -Force
New-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.1"\Client Enabled -value 1 -PropertyType DWORD -Force

New-Item -type Directory HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.2" -Force
New-Item -type Directory HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.2"\Client -Force
New-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.2"\Client Enabled -value 1 -PropertyType DWORD -Force

#-----------------------------------------------------
# Save CredSSP credential to Credential Manager
#-----------------------------------------------------
cmd /c cmdkey /add:"Domain:target=TERMSRV/$driverComputerName" /user:"$driverComputerName\$credSSPUser" /pass:$credSSPPwd
if($driverComputerIP -ne $driverComputerName)
{
    # Save for IP address
	cmd /c cmdkey /add:"Domain:target=TERMSRV/$driverComputerIP" /user:"$driverComputerName\$credSSPUser" /pass:$credSSPPwd
}

#-----------------------------------------------------
# Finished to config Terminal Client
#-----------------------------------------------------
Pop-Location
Write-Host "Write signal file: configTC.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$env:HOMEDRIVE\configTC.finished.signal

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-TerminalClientForRemoteAdapt.ps1] FINISHED (NOT VERIFIED)."

# cmd /C Pause

exit 0
