#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Config-DriverComputer.ps1
## Purpose:        Configure Driver Computer for MS-ADOD OD test suite.
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
    $driverLogPath         = .\Get-Parameter.ps1 $settingFile driverLogPath
    $localCapFilePath      = .\Get-Parameter.ps1 $settingFile localCapFilePath
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
    $RemoteIP              = .\Get-Parameter.ps1 $settingFile clientIP
    $driverLogFile         = $driverLogPath + "\Config-DriverComputer.ps1.log"
    .\Set-Parameter.ps1 $settingFile driverLogFile $driverlogFile "If no log file path specified, this value should be used."
}
else
{
    Write-Host "$settingFile not found. Will keep the default setting of all the test context info..."
}

#-----------------------------------------------------
# Create $logPath if not exist
#-----------------------------------------------------
if (!(Test-Path -Path $driverLogPath))
{
    New-Item -Type Directory -Path $driverLogPath -Force
}

#-----------------------------------------------------
# Create $logFile if not exist
#-----------------------------------------------------
if (!(Test-Path -Path $driverLogFile))
{
    New-Item -Type File -Path $driverLogFile -Force
}
Start-Transcript $driverLogFile -Append

#-----------------------------------------------------
# Write value for all the parameters
#-----------------------------------------------------
Write-Host "EXECUTING [Config-DriverComputer.ps1] ..." -foregroundcolor cyan
Write-Host "`$scriptsPath           = $scriptsPath"
Write-Host "`$driverLogPath         = $driverLogPath"       
Write-Host "`$driverLogFile         = $driverLogFile"
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
Write-Host "`$RemoteIP              = $RemoteIP"

#-----------------------------------------------------
# Begin to config Driver Computer
#-----------------------------------------------------

#-------------------------------------
# Turn off window firewall
#-------------------------------------
Write-Host "Turn off firewall"
cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Host

#-----------------------------------------------------
# Start the Windows Remote Management Service
#-----------------------------------------------------
Write-Host "Enable Windows Remote Management Service..."
Enable-PSRemoting -Force
Write-Host "Start to connect to $clientIP use $clientIP\$clientAdminUserName"

#check if Remote machine is in TrustedHosts
$originalValue = (get-item WSMan:\localhost\Client\TrustedHosts).value
[string]$originalValue = $originalValue.Replace("*","");
    
if([String]$originalValue.Contains($RemoteIP) -eq $false)
{
	if($originalValue.Length -gt 0){
		$originalValue = $originalValue + ",$RemoteIP"
	}else{
		$originalValue = "$RemoteIP"
	}
    
    Write-Host "Add $RemoteIP to Trusted Hosts"
    set-item WSMan:\localhost\Client\TrustedHosts -Value $originalValue -force
}
$remoteSession = .\New-RemoteSession $clientIP "$clientIP\$clientAdminUserName" $clientAdminUserPwd

#-----------------------------------------------------
# Get Information from Client Computer
#-----------------------------------------------------
$clientInfoFile = "$env:HOMEDRIVE\MSIInstalled.signal"
$clientInfo = Invoke-Command -Session $remoteSession -ScriptBlock {Param ($fileName) Get-Content "$fileName"} -ArgumentList $clientInfoFile
$clientScriptPath = $clientInfo
Remove-PSSession -Session $remoteSession

#-----------------------------------------------------
# Modify PTF Config File
#-----------------------------------------------------
$binPath         = "$scriptsPath\..\Bin"
$dataPath        = "$scriptsPath\..\Data"
$DepPtfConfig    = "$binPath\MS-ADOD_ODTestSuite.deployment.ptfconfig"

Write-Host  "TurnOff FileReadonly for $DepPtfConfig..."
.\TurnOff-FileReadonly.ps1 $DepPtfConfig

Write-Host "Begin to update MS-ADOD_ODTestSuite.deployment.ptfconfig..."
if($fullDomainName -ne $null -and $fullDomainName -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "FullDomainName"        $fullDomainName
  if($domainAdminUserName -ne $null -and $domainAdminUserName -ne "")
  {
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "DomainAdminUserName"   $domainAdminUserName
  }
  if($domainAdminUserPwd -ne $null -and $domainAdminUserPwd -ne "")
  {
    .\Modify-ConfigFileNode.ps1 $DepPtfConfig "DomainAdminPwd"        $domainAdminUserPwd
  }
}
if($pdcOperatingSystem -ne $null -and $pdcOperatingSystem -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "PDCOperatingSystem"      $pdcOperatingSystem
}

if($pdcComputerName -ne $null -and $pdcComputerName -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "PDCComputerName"         "$pdcComputerName.$fullDomainName"
}
if($pdcIP -ne $null -and $pdcIP -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "PDCIP"                   $pdcIP
}
if($clientOperatingSystem -ne $null -and $clientOperatingSystem -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "ClientOperatingSystem"    $clientOperatingSystem
}
if($clientComputerName -ne $null -and $clientComputerName -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "ClientComputerName"    $clientComputerName
}
if($clientIP -ne $null -and $clientIP -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "ClientIP"    $clientIP
}
if($clientAdminUserName -ne $null -and $clientAdminUserName -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "ClientAdminUserName"   $clientAdminUserName
}
if($clientAdminUserPwd -ne $null -and $clientAdminUserPwd -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "ClientAdminPwd"        $clientAdminUserPwd
}
if($clientScriptPath -ne $null -and $clientScriptPath -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "ClientScriptPath"        $clientScriptPath
}
if($clientLogPath -ne $null -and $clientLogPath -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "ClientLogPath"        $clientLogPath
}
if($driverLogPath -ne $null -and $driverLogPath -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "DriverLogPath"        $driverLogPath
}
if($localCapFilePath -ne $null -and $localCapFilePath -ne "")
{
  .\Modify-ConfigFileNode.ps1 $DepPtfConfig "LocalCapFilePath"        $localCapFilePath
}

#-----------------------------------------------------
# Modify ODLocalTestRun.testrunconfig file
#-----------------------------------------------------
Write-Host  "Modify ODLocalTestRun.testrunconfig file..."
$settingFile = "$binPath\ODLocalTestRun.testrunconfig"
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
# Finished to config driver computer
#-----------------------------------------------------
Pop-Location
Write-Host "Write signal file: ConfigScript.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-DriverComputer.ps1] FINISHED (NOT VERIFIED)."

Stop-Transcript

exit 0
