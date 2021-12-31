# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-Server.ps1
## Author:         v-mankuc
## Purpose:        Configure server for MS-WSP test suite.
## Version:        1.0 (06 Oct, 2008)
## Requirements:   Windows Powershell 2.0 CTP2,Hyper-V
## Supported OS:   Windows 2003 Server,Windows 2008 Server, Win7
##
##############################################################################

param(
     [string]$WorkingDir = "$env:SystemDrive\Temp", 
     [string]$ProtocolConfigFile = "$workingDir\Protocol.xml"
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
$logFile = $WorkingDir + "\Config-Server.ps1.log"
Start-Transcript $logFile -Force

Write-Host "EXECUTING [Config-Server.ps1] ..." -ForegroundColor Cyan
Write-Host "`$WorkingDir             = $WorkingDir"
Write-Host "`$ProtocolConfigFile     = $ProtocolConfigFile"

Write-Host "Put current directory as $WorkingDir."
Push-Location $WorkingDir

Write-Host  "Verifying environment..."
& .\Verify-NetworkEnvironment.ps1 "IPv4" "Workgroup"
 
#-----------------------------------------------------
# Begin to config server
#-----------------------------------------------------
Write-Host "Begin to config Server..."

Write-Host "Copy test data to the destination..."

Write-Host "Create the test folder in a default indexed location..."
$testPath = "$env:USERPROFILE\Test"
if (-not (Test-Path $testPath)) {
     New-Item -ItemType Directory -Path $testPath -Force
}

Write-Host "The test folder is located at $testPath"

if (Test-Path $ProtocolConfigFile) {
     [xml]$config = Get-Content $ProtocolConfigFile
     $server = $config.lab.servers.vm | Where-Object { $_.role -eq "Server" }
     $tsTargetFolder = $server.tools.TestsuiteZip.targetFolder
     $tsDataFolder = "$tsTargetFolder\Data"
}
else {
     $tsTargetFolder = "$env:SystemDrive\MS-WSP-TestSuite-ServerEP"
     $tsDataFolder = "$tsTargetFolder\Data"
}

Copy-Item "$tsDataFolder" -Destination $testPath -Recurse -Force

Write-Host "Modify the CreateTime of some files to meet the requirements of some test cases..."
[array]$dataFiles = Get-ChildItem "$testPath\Data\CreateQuery_Size" -Force
for ($i = 0; $i -lt $dataFiles.Length; $i++) {
     $dataFile = $dataFiles[$i]
     $dataFile.CreationTime = $dataFile.CreationTime.AddDays(-10 * $i)
     Write-Host "The CreationTime of $($dataFile.FullName) is $($dataFile.CreationTime)"
}

Write-Host "Modify the FileAttributes values of some files to meet the requirements of some test cases..."
[array]$attrFiles = Get-ChildItem "$testPath\Data\CreateQuery_CFullPropSpec\attr*.txt" -Force
for ($i = 0; $i -lt $attrFiles.Length; $i++) {
     $attrFile = $attrFiles[$i]
     if ($attrFile.Name -match "attr2.txt") {
          $attrFile.Attributes = $attrFile.Attributes -bor [System.IO.FileAttributes]::ReadOnly
          Write-Host "Make file attr2.txt ReadOnly"
     }
     elseif ($attrFile.Name -match "attr3.txt") {
          $attrFile.Attributes = $attrFile.Attributes -bor [System.IO.FileAttributes]::Hidden
          Write-Host "Make file attr2.txt Hidden"
     }
}

Write-Host "Enable and restart Windows Search service..."
$serviceName = "WSearch"
$serviceInfo = Get-Service -Name $serviceName -ErrorAction SilentlyContinue
if ($serviceInfo -eq $null) { 
     Write-Host "Install Windows Search service to the server..."
     Install-WindowsFeature Search-Service

     Start-Sleep -Seconds 30

     $serviceInfo = Get-Service -Name $serviceName -ErrorAction SilentlyContinue
}

if ($serviceInfo.Status -ne "Running") {
     Set-Service -Name $serviceName -StartupType Automatic
     Restart-Service -Name $serviceName
}

Write-Host "Make the test data folder to a share..."

Write-Host "Grant user access to shared folder..."
icacls.exe "$testPath" /grant "*S-1-1-0:(OI)(CI)(F)" 2>&1 | Write-Host

Write-Host "Share $testPath..."
$smbShare = Get-SmbShare | Where-Object { $_.Name -eq "Test" -and $_.Path -eq "$testPath" }
if ($smbShare -eq $null) {        
     New-SMBShare -Name "Test" -Path "$testPath" -FullAccess "Everyone"
}

Write-Host "Wait for reindexing..."
Start-Sleep -Seconds 30

#----------------------------------------------------------------------------
# Finished to config server
#----------------------------------------------------------------------------
Pop-Location

Write-Host "Write signal file: config.finished.signal to system drive."
Set-Content -Path "$env:SystemDrive\config.finished.signal" -Value "CONFIG FINISHED"

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-Server.ps1] FINISHED (NOT VERIFIED)."

Stop-Transcript -ErrorAction SilentlyContinue