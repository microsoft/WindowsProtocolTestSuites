#############################################################################
##
## Microsoft Windows Powershell Sripting
## File:           Config-DC.ps1
## Author:         v-mankuc
## Purpose:        Configure DC for MS-WSP test suite.
## Version:        1.1 (06 NOV, 2008)
## Requirements:   Windows Powershell 2.0 CTP2,Hyper-V
## Supported OS:   Windows 2003 Server,Windows 2008 Server, Win7
##
##############################################################################

Param(
[String]$toolsPath, 
[String]$scriptsPath, 
[String]$testSuitesPath,
[String]$logPath,
[String]$clientOS,

[String]$IPVersion,
[String]$workgroupDomain,
[string]$userNameInVM,
[string]$userPwdInVM,
[string]$domainInVM
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
$logFile = $logPath+"\Config-DC.ps1.log"
Start-Transcript $logFile -force

Write-Host "EXECUTING [Config-DC.ps1] ..." -foregroundcolor cyan
Write-Host "`$toolsPath       = $toolsPath"
Write-Host "`$scriptsPath     = $scriptsPath"
Write-Host "`$testSuitesPath  = $testSuitesPath"
Write-Host "`$logFile         = $logFile"
Write-Host "`$clientOS        = $clientOS"
Write-Host "`$IPVersion       = $IPVersion"
Write-Host "`$workgroupDomain = $workgroupDomain"
Write-Host "`$userNameInVM    = $userNameInVM"
Write-Host "`$userPwdInVM     = $userPwdInVM"
Write-Host "`$domainInVM      = $domainInVM"

Write-Host "Put current dir as $scriptsPath."
pushd $scriptsPath

$dataPath = $scriptsPath + "\..\Data"
$myToolsPath = $scriptsPath + "\..\MyTools"

#-----------------------------------------------------
# Begin to config DC
#-----------------------------------------------------
Write-Host "Begin to config DC..." -foregroundcolor yellow
Copy-Item $dataPath"\hosts" "$env:windir\system32\drivers\etc\hosts" -force

Write-Host "Install DC..." -foregroundcolor yellow
$osVersion = .\Get-OSVersion.ps1 SUT01 $userNameInVM $userPwdInVM 
if (($osVersion -ne $null) -and ($osVersion -eq "W2K3"))
{
   Write-Host "Server OS is W2K3"   
   cmd.exe /c dcpromo "/answer:$dataPath\DC_Install_W2K3.txt" 2>&1 | Write-Host
}
else
{
    Write-Host "Server OS is W2K8 or later"
    cmd.exe /c dcpromo "/unattend:$dataPath\DC_Install_W2K8.txt" 2>&1 | Write-Host
}

Write-Host "Restart and run Config-server.ps1 on SUT..." -foregroundcolor yellow
.\RestartAndRun.bat "cmd /C powershell.exe $scriptsPath\Config-Server.ps1 $toolsPath $scriptsPath $testSuitesPath $logPath $clientOS $IPVersion Domain $userNameInVM $userPwdInVM $domainInVM"

#-----------------------------------------------------
# Finished to config DC
#-----------------------------------------------------
popd

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-DC.ps1] FINISHED (NOT VERIFIED)."
Stop-Transcript

exit 0
