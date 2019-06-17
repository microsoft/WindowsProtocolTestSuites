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
    .\Set-Parameter.ps1 $settingFile LogFile $logFile "If no log file path specified, this value should be used."
}
else
{
    Write-Host "$settingFile not found. Will keep the default setting of all the test context info..."
}

$DropConnectionForInvalidRequest = "true"
$SutOsVersion = Invoke-Command -ComputerName $tcComputerName -ScriptBlock {""+[System.Environment]::OSVersion.Version.Major.ToString() + "." + [System.Environment]::OSVersion.Version.Minor.ToString()}
$SutOsBuildNumber = Invoke-Command -ComputerName $tcComputerName -ScriptBlock {[System.Environment]::OSVersion.Version.Build}

if([double]$SutOSVersion -ge "10.0")
{
    
    if([double] $SutOsBuildNumber -eq "15063")
    {
        $RDPVersion = "10.3"
    }

    if([double] $SutOsBuildNumber -ge "15063")
    {
        $DropConnectionForInvalidRequest = "false"
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
Restart-Service WinRM

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
       
   }
}

#-----------------------------------------------------
# Modify PTF Config File
#-----------------------------------------------------
$binPath         = $scriptsPath + "\..\Bin"
$dataPath        = $scriptsPath + "\..\Data"
$DepPtfConfig    = "$binPath\RDP_ClientTestSuite.deployment.ptfconfig"

Write-Host  "TurnOff FileReadonly for $DepPtfConfig..."
.\TurnOff-FileReadonly.ps1 $DepPtfConfig

Write-Host "Begin to update RDP_ClientTestSuite.deployment.ptfconfig..."
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerPort"            $listeningPort
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.IpVersion"             $ipVersion
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Protocol"     $securityProtocol
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Negotiation"  $negotiationBased
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "CertificatePath"           "$env:HOMEDRIVE\$certFileName.pfx"
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "CertificatePassword"       $certPwd
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "SUTName"                   $tcComputerName
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "SUTUserPassword"           $userPwdInTC
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "SUTSystemDrive"            $tcSystemDrive
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerDomain"          $domainName
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerUserName"        $CredSSPUser
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerUserPassword"    $CredSSPPwd
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Version"               $RDPVersion

if ($osVersion.ToUpper() -eq "NONWINDOWS")
{
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "IsWindowsImplementation"      "false"
	
    # Update default RDP security protocol as RDP/Low/128bit for NonWindows AutoTest
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Protocol" "RDP"    
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "DropConnectionForInvalidRequest" "true"
    
}
else
{
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "IsWindowsImplementation"      "true"
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "DropConnectionForInvalidRequest" $DropConnectionForInvalidRequest
}

if ($securityProtocol.ToUpper() -eq "RDP")
{
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Encryption.Level" "Low" 
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Encryption.Method" "128bit" 
}
else
{
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Encryption.Level" "None" 
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Encryption.Method" "None" 
}

if ($workgroupDomain.ToUpper() -eq "DOMAIN")
{
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "SUTUserName"      "$domainName\$userNameInTC"
}
else
{
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "SUTUserName"      "$userNameInTC"
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerDomain" "$driverComputerName"
}

Write-Host  "TurnOff FileReadonly for $DepPtfConfig due to Execution Console..."
.\TurnOff-FileReadonly.ps1 $DepPtfConfig

#-----------------------------------------------------
# Modify ClientLocal.testsettings file
#-----------------------------------------------------
Write-Host  "Modify ClientLocal.testsettings fileg..."
$settingFile = "$binPath\ClientLocal.testsettings"
[XML]$settingXml = Get-Content $settingFile
$depFiles = $settingXml.GetElementsByTagName("DeploymentItem")
foreach ($depFile in $depFiles)
{
    $fileOrg = $depFile.filename
    $idx     = $fileOrg.LastIndexOf("\")
    $fileNew = ".\" + $fileOrg.SubString($idx + 1)
    Write-Host "Change $fileOrg to $fileNew."
    $depFile.filename = $fileNew
}
$settingXml.Save("$settingFile")

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