# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param(
[String]$scriptsPath  = [System.IO.Path]::GetDirectoryName($myInvocation.MyCommand.Definition)
)

Write-Host "Put current dir as $scriptsPath."
pushd $scriptsPath

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
$logFile = "$env:HOMEDRIVE\Logs\Config-DriverComputer.ps1.log"

$settingFile = "$scriptsPath\ParamConfig.xml"
if(Test-Path -Path $settingFile)
{    
    $Negotiation       	= .\Get-Parameter.ps1 $settingFile Negotiation
    $Protocol        	= .\Get-Parameter.ps1 $settingFile Protocol
    $domainName        	= .\Get-Parameter.ps1 $settingFile domainName
    $ServerName         = .\Get-Parameter.ps1 $settingFile ServerName
    $ServerUserName     = .\Get-Parameter.ps1 $settingFile ServerUserName
    $ServerUserPwd     	= .\Get-Parameter.ps1 $settingFile ServerUserPwd
    $ServerPort     	= .\Get-Parameter.ps1 $settingFile ServerPort
    $ClientName 		= .\Get-Parameter.ps1 $settingFile ClientName
    $RDPVersion      	= .\Get-Parameter.ps1 $settingFile RDPVersion
}
else
{
    Write-Host "$settingFile not found. Will keep the default setting of all the test context info..."
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
Write-Host "`$logFile            = $logFile"
Write-Host "`$Negotiation        = $Negotiation" 
Write-Host "`$Protocol        	 = $Protocol"
Write-Host "`$domainName         = $domainName" 
Write-Host "`$ServerName         = $ServerName"
Write-Host "`$ServerUserName     = $ServerUserName" 
Write-Host "`$ServerUserPwd      = $ServerUserPwd"
Write-Host "`$ServerPort     	 = $ServerPort" 
Write-Host "`$ClientName 		 = $ClientName"
Write-Host "`$RDPVersion      	 = $RDPVersion"

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
# If listening port of test suite is 3389 and the Remote Desktop Service is running, edit registry to change Windows RDP service port and restart TermService
#-----------------------------------------------------
if($listeningPort -eq "3389")
{   
   if(Get-Service | Where-Object{$_.name -eq "TermService"} | where-object{$_.status -eq "Running"})
   {
       Set-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Control\"Terminal Server"\Winstations\RDP-Tcp PortNumber -value 4488 -Type DWORD
       Restart-Service TermService -F
   }
}

#-----------------------------------------------------
# Modify PTF Config File
#-----------------------------------------------------
$binPath         = $scriptsPath + "\..\Bin"
$dataPath        = $scriptsPath + "\..\Data"
$DepPtfConfig    = "$binPath\RDP_ServerTestSuite.deployment.ptfconfig"

Write-Host  "TurnOff FileReadonly for $DepPtfConfig..."
.\TurnOff-FileReadonly.ps1 $DepPtfConfig

Write-Host "Begin to update RDP_ServerTestSuite.deployment.ptfconfig..."
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Negotiation"        $Negotiation
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Security.Protocol"           $Protocol
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerDomain"        		$domainName
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerName"    				$ServerName
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerPort"                	$ServerPort
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerUserName"        		$ServerUserName
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ServerUserPassword"         	$ServerUserPwd
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.ClientName"       			$ClientName
.\Modify-ConfigFileNode.ps1 $DepPtfConfig "RDP.Version"     				$RDPVersion

#-----------------------------------------------------
# Finished to config driver computer
#-----------------------------------------------------
popd
Write-Host "Write signal file: config.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$env:HOMEDRIVE\config.finished.signal

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-DriverComputer.ps1] FINISHED (NOT VERIFIED)."

exit 0

