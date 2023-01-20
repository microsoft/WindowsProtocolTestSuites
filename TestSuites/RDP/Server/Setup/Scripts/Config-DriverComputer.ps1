# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
[String]$scriptsPath  = [System.IO.Path]::GetDirectoryName($myInvocation.MyCommand.Definition)
)

Write-Host "Put current dir as $scriptsPath."
Push-Location $scriptsPath

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------

$settingFile = "$scriptsPath\ParamConfig.xml"
if(Test-Path -Path $settingFile)
{    
    $logPath            = .\Get-Parameter.ps1 $settingFile logPath
    $logFile            = $logPath + "\Config-DriverComputer.ps1.log"
    $userNameInTC       = .\Get-Parameter.ps1 $settingFile userNameInTC
    $userPwdInTC        = .\Get-Parameter.ps1 $settingFile userPwdInTC
    $CredSSPUser        = .\Get-Parameter.ps1 $settingFile CredSSPUser
    $CredSSPPwd         = .\Get-Parameter.ps1 $settingFile CredSSPPwd
    $domainName         = .\Get-Parameter.ps1 $settingFile domainName
    $dcComputerName     = .\Get-Parameter.ps1 $settingFile dcComputerName
    $tcComputerName     = .\Get-Parameter.ps1 $settingFile tcComputerName
    $driverComputerName = .\Get-Parameter.ps1 $settingFile driverComputerName
    $listeningPort      = .\Get-Parameter.ps1 $settingFile RDPListeningPort
    $ipVersion          = .\Get-Parameter.ps1 $settingFile ipVersion
	$securityProtocol   = .\Get-Parameter.ps1 $settingFile securityProtocol
	$negotiationBased   = .\Get-Parameter.ps1 $settingFile negotiationBasedApproach
    $osVersion          = .\Get-Parameter.ps1 $settingFile osVersion
	$RDPVersion         = .\Get-Parameter.ps1 $settingFile RDPVersion
    $workgroupDomain    = .\Get-Parameter.ps1 $settingFile workgroupDomain
    $tcSystemDrive      = .\Get-Parameter.ps1 $settingFile tcSystemDrive
    $agentPort          = .\Get-Parameter.ps1 $settingFile agentPort
    $agentRemoteClient  = .\Get-Parameter.ps1 $settingFile agentRemoteClient
    $compressionInTC    = .\Get-Parameter.ps1 $settingFile compressionInTC
    .\Set-Parameter.ps1 $settingFile LogFile $logFile "If no log file path specified, this value should be used."
}
else
{
    Write-Host "$settingFile not found. Will keep the default setting of all the test context info..."
}

#--Placeholder default for the windows server edition and build number being tested
[string]$winVersion = "windows server : 0"
$DropConnectionForInvalidRequest = "true"
[decimal]$SutOsVersion = Invoke-Command -ComputerName $tcComputerName -ScriptBlock { "$([System.Environment]::OSVersion.Version.Major.ToString()).$([System.Environment]::OSVersion.Version.Minor.ToString())" }
[int]$SutOsBuildNumber = Invoke-Command -ComputerName $tcComputerName -ScriptBlock { [System.Environment]::OSVersion.Version.Build }
$SutOsName = Invoke-Command -ComputerName $tcComputerName -ScriptBlock { Get-WmiObject Win32_OperatingSystem }

if ($null -eq $SutOsName) {
    Write-Host "Connect to computer $ComputerName failed."
}

if ($null -ne $SutOsName) {
    [string]$initVersion = $SutOsName.Name.split("|")[0]
    [string]$buildNumber = $SutOsName.BuildNumber.split(".")[0].split(" ")[0].split("-")[0]
    $winVersion = "$initVersion : $buildNumber"
}

if ($SutOSVersion -ge 10.0) {
    if ($SutOsBuildNumber -ge 19041) {
        $RDPVersion = "10.8"
    }
    elseif ($SutOsBuildNumber -ge 18362) {
        $RDPVersion = "10.7"
    }
    elseif ($SutOsBuildNumber -ge 17763) {
        $RDPVersion = "10.6"
    }
    elseif ($SutOsBuildNumber -ge 17134) {
        $RDPVersion = "10.5"
    }
    elseif ($SutOsBuildNumber -ge 16299) {
        $RDPVersion = "10.4"
    }
    elseif ($SutOsBuildNumber -ge 15063) {
        $RDPVersion = "10.3"
        $DropConnectionForInvalidRequest = "false"
    }
    elseif ($SutOsBuildNumber -ge 14393) {
        $RDPVersion = "10.2"
    }
    elseif ($SutOsBuildNumber -ge 10586) {
        $RDPVersion = "10.1"
    }
    else {
        $RDPVersion = "10.0"
    }
}
elseif ($SutOsVersion -ge 6.3) {
    $RDPVersion = "8.1"
}
elseif ($SutOsVersion -ge 6.2) {
    $RDPVersion = "8.0"
}
elseif ($SutOsVersion -ge 6.1) {
    if ($SutOsBuildNumber -ge 7601) {
        $RDPVersion = "7.1"
    }
    else {
        $RDPVersion = "7.0"
    }
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
Write-Host "EXECUTING [Config-DriverComputer.ps1] ..." -foregroundcolor cyan
Write-Host "`$scriptsPath        = $scriptsPath"
Write-Host "`$logPath            = $logPath"       
Write-Host "`$logFile            = $logFile"
Write-Host "`$userNameInTC       = $userNameInTC" 
Write-Host "`$userPwdInTC        = $userPwdInTC"
Write-Host "`$CredSSPUser        = $CredSSPUser" 
Write-Host "`$CredSSPPwd         = $CredSSPPwd"
Write-Host "`$domainName         = $domainName" 
Write-Host "`$dcComputerName     = $dcComputerName"
Write-Host "`$tcComputerName     = $tcComputerName" 
Write-Host "`$driverComputerName = $driverComputerName"
Write-Host "`$listeningPort      = $listeningPort"
Write-Host "`$ipVersion          = $ipVersion"
Write-Host "`$securityProtocol   = $securityProtocol"  
Write-Host "`$negotiationBased   = $negotiationBased"       
Write-Host "`$osVersion          = $osVersion" 
Write-Host "`$RDPVersion         = $RDPVersion" 
Write-Host "`$workgroupDomain    = $workgroupDomain"
Write-Host "`$tcSystemDrive      = $tcSystemDrive"
Write-Host "`$agentPort          = $agentPort"
Write-Host "`$agentRemoteClient  = $agentRemoteClient"
Write-Host "`$compressionInTC    = $compressionInTC"

#-----------------------------------------------------
# Begin to config Driver Computer
#-----------------------------------------------------

#-------------------------------------
# Turn off window firewall
#-------------------------------------
Write-Host "Turn off firewall"
cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Host

#-----------------------------------------------------
# Enable Powershell Remoting
#-----------------------------------------------------
Set-NetConnectionProfile -NetworkCategory Private -ErrorAction Ignore
Enable-PSRemoting -Force
Set-Item wsman:\localhost\client\trustedhosts *  -Force
Restart-Service WinRM -Force

#-----------------------------------------------------
# Create a self-signed certificate and export
#-----------------------------------------------------
Write-Host "Create a self-signed certificate and export it out..."

if ($workgroupDomain.ToUpper() -eq "DOMAIN")
{
    $certCN = $driverComputerName + "." + $domainName
}
else
{
    $certCN = $driverComputerName
}
$certPwd = "Password01!"
$certFileName = $driverComputerName

if (Test-Path -Path "$env:HOMEDRIVE\$certFileName.cer")
{
    Remove-Item "$env:HOMEDRIVE\$certFileName.cer" -Force
}
if (Test-Path -Path "$env:HOMEDRIVE\$certFileName.pfx")
{
    Remove-Item "$env:HOMEDRIVE\$certFileName.pfx" -Force
}

$cert = New-SelfSignedCertificate -DnsName $certCN -CertStoreLocation "cert:\LocalMachine\My"
$securePwd = (ConvertTo-SecureString -string "$certPwd" -Force -AsPlainText)
Export-PfxCertificate -Cert $cert -Force -Password $securePwd -FilePath "$env:HOMEDRIVE\$certFileName.pfx"
Export-Certificate -Cert $cert -FilePath "$env:HOMEDRIVE\$certFileName.cer" -Type CERT

#-----------------------------------------------------
# If listening port of test suite is 3389 and the Remote Desktop Service is running, edit registry to change Windows RDP service port and restart TermService
#-----------------------------------------------------
if($listeningPort -eq "3389")
{   
   if(Get-Service | Where-Object{$_.name -eq "TermService"} | where-object{$_.status -eq "Running"})
   {
       Set-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Control\"Terminal Server"\Winstations\RDP-Tcp PortNumber -value 4488 -Type DWORD
       try {
            Restart-Service TermService -Force 
       }
       catch {
            Write-Warning  "Restart-Service TermService failed..."
       }
       
       try {
              $systemroot = get-content env:systemroot
              netsh advfirewall firewall add rule name="Remote Desktop - Custom Port" dir=in program=$systemroot\system32\svchost.exe service=termservice action=allow protocol=TCP localport=4488 enable=yes
       }
       catch {
            Write-Warning  "Enable firewall for RDP Port with 4488 failed..."
       }
   }

   # If listening port is 3389, we need to enable firewall for it.
    try {
           netsh advfirewall firewall add rule name="enable3389" dir=in action=allow protocol=TCP localport=3389 enable=yes
    }
    catch {
        Write-Warning  "Enable 3389 port firewall failed..."
    }
}

#-----------------------------------------------------
# Modify PTF Config File
#-----------------------------------------------------
$binPath         = $scriptsPath + "\..\Bin"
$dataPath        = $scriptsPath + "\..\Data"
$DepPtfConfig    = "$binPath\RDP_ServerTestSuite.deployment.ptfconfig"
$PtfConfig       = "$binPath\RDP_ServerTestSuite.ptfconfig"

Write-Host  "TurnOff FileReadonly for $PtfConfig..."
.\TurnOff-FileReadonly.ps1 $PtfConfig

$agentAddress    = "$tcComputerName" + ":" + $agentPort
Write-Host "Begin to update RDP_ServerTestSuite.ptfconfig..."
.\Modify-ConfigFileNode.ps1 $PtfConfig "AgentAddress" $agentAddress

Write-Host  "TurnOff FileReadonly for $DepPtfConfig..."
.\TurnOff-FileReadonly.ps1 $DepPtfConfig

Write-Host "Begin to update RDP_ServerTestSuite.deployment.ptfconfig..."
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerPort"            $listeningPort
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Protocol"     $securityProtocol
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Negotiation"  $negotiationBased
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerName"            $tcComputerName
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerDomain"          $domainName
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerUserName"        $CredSSPUser
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerUserPassword"    $CredSSPPwd
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Version"               $RDPVersion
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.WindowsVersion"        $winVersion

if ($osVersion.ToUpper() -eq "NONWINDOWS")
{
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "IsWindowsImplementation" "false"
	
    # Update default RDP security protocol as RDP/Low/128bit for NonWindows AutoTest
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "Protocol" "RDP"    
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "DropConnectionForInvalidRequest" "true"
    
}
else
{
    if($agentRemoteClient -ne "mstsc"){
        .\Modify-ConfigFileNode.ps1 $DepPtfConfig "IsWindowsImplementation" "false"
    }
    else
    {
        .\Modify-ConfigFileNode.ps1 $DepPtfConfig "IsWindowsImplementation" "true"
    }
}


Write-Host  "TurnOff FileReadonly for $DepPtfConfig due to Execution Console..."
.\TurnOff-FileReadonly.ps1 $DepPtfConfig

#-----------------------------------------------------
# Edit registery.
# Disable TLS 1.0 for Server
#-----------------------------------------------------
Write-Host "Disable TLS 1.0 for Server."
New-Item 'HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server' -Force | Out-Null
New-ItemProperty -path 'HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server' -name 'Enabled' -value 0 -PropertyType 'DWord' -Force | Out-Null
New-ItemProperty -path 'HKLM:\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Server' -name 'DisabledByDefault' -value '0xffffffff' -PropertyType 'DWord' -Force | Out-Null
Write-Host 'TLS 1.0 has been disabled.'

#-----------------------------------------------------
# Create task to detect whether the SUT Adapter works
#-----------------------------------------------------
Write-Host "Creating task to detect whether the SUT Adapter works and try 10 times if fails ..."
$batchPath = "$scriptsPath\..\Batch"
cmd /c schtasks /Create /SC Weekly /TN WaitForSUTControlAdapterReady /TR "powershell $scriptsPath\WaitForSUTControlAdapterReady.ps1 $tcComputerName $userNameInTC $userPwdInTC $batchPath" /IT /F

#-----------------------------------------------------
# Finished to config driver computer
#-----------------------------------------------------
Pop-Location
Write-Host "Write signal file: config.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$env:HOMEDRIVE\config.finished.signal

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-DriverComputer.ps1] FINISHED (NOT VERIFIED)."

exit 0