#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

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
    $logFile            = $logPath + "\Config-TerminalClient.ps1.log"
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
Write-Host "EXECUTING [Config-TerminalClient.ps1] ..." -foregroundcolor cyan
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
# Enable Powershell Remoting
#-----------------------------------------------------
if(Test-WSMan -ComputerName $sutSetting.ip){
    Write-Host "WinRM is running"
}else{
    Set-NetConnectionProfile -NetworkCategory Private -ErrorAction Ignore
    Enable-PSRemoting -Force
    Set-Item wsman:\localhost\client\trustedhosts *  -Force
    Restart-Service WinRM
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

if ($compressionInTC.ToUpper() -eq "YES")
{
   $compressionStr = "compression:i:1"
}else
{
   $compressionStr = "compression:i:0"
}

Write-Host "Modifying target IP address and compression of RDP files..."
if(!(Test-Path -Path "$dataPath\Base"))
{
    New-Item -Path "$dataPath\Base" -ItemType directory
}

if (Test-Path -Path "$dataPath\Base\Negotiate.RDP")
{
    Copy-Item $dataPath\Base\Negotiate.RDP $dataPath\Negotiate.RDP -Force
}else
{
    Copy-Item $dataPath\Negotiate.RDP $dataPath\Base\Negotiate.RDP -Force
}

if (Test-Path -Path "$dataPath\Base\NegotiateFullScreen.RDP")
{
    Copy-Item $dataPath\Base\NegotiateFullScreen.RDP $dataPath\NegotiateFullScreen.RDP -Force
}else
{
    Copy-Item $dataPath\NegotiateFullScreen.RDP $dataPath\Base\NegotiateFullScreen.RDP -Force
}

if (Test-Path -Path "$dataPath\Base\DirectCredSSP.RDP")
{
    Copy-Item $dataPath\Base\DirectCredSSP.RDP $dataPath\DirectCredSSP.RDP -Force
}else
{
    Copy-Item $dataPath\DirectCredSSP.RDP $dataPath\Base\DirectCredSSP.RDP -Force
}

if (Test-Path -Path "$dataPath\Base\DirectCredSSPFullScreen.RDP")
{
    Copy-Item $dataPath\Base\DirectCredSSPFullScreen.RDP $dataPath\DirectCredSSPFullScreen.RDP -Force
}else
{
    Copy-Item $dataPath\DirectCredSSPFullScreen.RDP $dataPath\Base\DirectCredSSPFullScreen.RDP -Force
}

"`nfull address:s:${driverComputerName}:${listeningPort}" | out-file "$dataPath\Negotiate.RDP" -Append -Encoding Unicode
"`nfull address:s:${driverComputerName}:${listeningPort}" | out-file "$dataPath\DirectCredSSP.RDP" -Append -Encoding Unicode
"`nfull address:s:${driverComputerName}:${listeningPort}" | out-file "$dataPath\NegotiateFullScreen.RDP" -Append -Encoding Unicode
"`nfull address:s:${driverComputerName}:${listeningPort}" | out-file "$dataPath\DirectCredSSPFullScreen.RDP" -Append -Encoding Unicode

"`n$compressionStr" | out-file "$dataPath\Negotiate.RDP" -Append -Encoding Unicode
"`n$compressionStr" | out-file "$dataPath\DirectCredSSP.RDP" -Append -Encoding Unicode
"`n$compressionStr" | out-file "$dataPath\NegotiateFullScreen.RDP" -Append -Encoding Unicode
"`n$compressionStr" | out-file "$dataPath\DirectCredSSPFullScreen.RDP" -Append -Encoding Unicode

"`nusbdevicestoredirect:s:*" | out-file "$dataPath\Negotiate.RDP" -Append -Encoding Unicode
"`nusbdevicestoredirect:s:*" | out-file "$dataPath\DirectCredSSP.RDP" -Append -Encoding Unicode
"`nusbdevicestoredirect:s:*" | out-file "$dataPath\NegotiateFullScreen.RDP" -Append -Encoding Unicode
"`nusbdevicestoredirect:s:*" | out-file "$dataPath\DirectCredSSPFullScreen.RDP" -Append -Encoding Unicode

Write-Host "Allow RDP connecting to unkown publisher for $driverComputerName..."
cmd /c reg add "HKCU\Software\Microsoft\Terminal Server Client\LocalDevices" /v $driverComputerName /t REG_DWORD /d 68 /F

Write-Host "Creating task to trigger client to initiate a RDP connection with Negotiation Approach..."
cmd /c schtasks /Create /RU $taskUser /SC Weekly /TN Negotiate_RDPConnect /TR "$dataPath\Negotiate.RDP" /IT /F

Write-Host "Creating task to trigger client to initiate a RDP connection using CredSSP security protocol with Direct Approach..."
cmd /c schtasks /Create /RU $taskUser /SC Weekly /TN DirectCredSSP_RDPConnect /TR "$dataPath\DirectCredSSP.RDP" /IT /F

Write-Host "Creating task to trigger client to initiate a full screen RDP connection with Negotiation Approach..."
cmd /c schtasks /Create /RU $taskUser /SC Weekly /TN Negotiate_FullScreen_RDPConnect /TR "$dataPath\NegotiateFullScreen.RDP" /IT /F

Write-Host "Creating task to trigger client to initiate a full screen RDP connection using CredSSP security protocol with Direct Approach..."
cmd /c schtasks /Create /RU $taskUser /SC Weekly /TN DirectCredSSP_FullScreen_RDPConnect /TR "$dataPath\DirectCredSSPFullScreen.RDP" /IT /F

Write-Host "Creating task to maximize mstsc window..."
cmd /c schtasks /Create /RU $taskUser /SC Weekly /TN MaximizeMstsc /TR "powershell $scriptsPath\MaximizeMstsc.ps1" /IT /F

Write-Host "Creating task to trigger RDP client to start a Auto-Reconnect sequence after a network interruption..."
cmd /c schtasks /Create /RU $taskUser /SC Weekly /TN TriggerNetworkFailure /TR "powershell $dataPath\TriggerNetworkFailure.ps1" /IT /F

Write-Host "Creating task to close all RDP connections of terminal client..."
cmd /c schtasks /Create /RU $taskUser /SC Weekly /TN DisconnectAll /TR "$dataPath\DisconnectAll.bat" /IT /F

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
# Force client to use TLS 1.0, not to use TLS 1.1 and TLS 1.2
#-----------------------------------------------------
Write-Host "Change Registry, force client to use TLS 1.0"
New-Item -type Directory HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.1" -Force
New-Item -type Directory HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.1"\Client -Force
New-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.1"\Client Enabled -value 0 -PropertyType DWORD -Force

New-Item -type Directory HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.2" -Force
New-Item -type Directory HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.2"\Client -Force
New-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\"TLS 1.2"\Client Enabled -value 0 -PropertyType DWORD -Force

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
Write-Host "EXECUTE [Config-TerminalClient.ps1] FINISHED (NOT VERIFIED)."

# cmd /C Pause

exit 0
