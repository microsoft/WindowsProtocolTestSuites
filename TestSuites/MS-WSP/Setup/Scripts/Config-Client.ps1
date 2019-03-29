#############################################################################
##
## Microsoft Windows Powershell Sripting
## File:           Config-Client.ps1
## Author:         v-mankuc
## Purpose:        Configure client for MS-WSP test suite.
## Version:        1.0 (06 Oct, 2008)
## Requirements:   Windows Powershell 2.0 CTP2,Hyper-V
## Supported OS:   Windows Xp, Windows Vista
##
##############################################################################

Param(
[String]$ToolsPath, 
[String]$ScriptsPath, 
[String]$TestSuitePath,
[String]$LogPath,
[String]$ServerOS,

[String]$IPVersion,
[String]$WorkgroupDomain,
[string]$userNameInVM,
[string]$userPwdInVM,
[string]$domainInVM
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
$logFile = $logPath+"\Config-Client.ps1.log"
Start-Transcript $logFile -force

Write-Host "EXECUTING [Config-Client.ps1] ..." -foregroundcolor cyan
Write-Host "`$toolsPath       = $toolsPath"
Write-Host "`$scriptsPath     = $scriptsPath"
Write-Host "`$testSuitePath   = $testSuitePath"
Write-Host "`$logFile         = $logFile"
Write-Host "`$serverOS        = $serverOS"
Write-Host "`$IPVersion       = $IPVersion"
Write-Host "`$workgroupDomain = $workgroupDomain"
Write-Host "`$userNameInVM    = $userNameInVM"
Write-Host "`$userPwdInVM     = $userPwdInVM"
Write-Host "`$domainInVM      = $domainInVM"

Write-Host "Put current dir as $scriptsPath."
pushd $scriptsPath

$dataPath = $scriptsPath.Substring(0, $scriptsPath.Length - $scriptsPath.LastIndexOf('\')) + "Data"
$myToolsPath = $scriptsPath.Substring(0, $scriptsPath.Length - $scriptsPath.LastIndexOf('\')) + "MyTools"

Write-Host "Enable FileSharing & Stop Firewall..."  #Note: Don't run Verify-NetworkEnvironment.ps1, because Client should not join to domain
.\Config-FileSharing.ps1
netsh.exe firewall set opmode mode=DISABLE profile=ALL 2>&1 | Write-Host



#-----------------------------------------------------
# Begin to config Client
#-----------------------------------------------------
Write-Host  "Begin to config client..."        

#------------------------------------------------------
# Configurate Client
#------------------------------------------------------
$SERVERcomputer      = "SUT01"
$computerName        = $env:COMPUTERNAME
$UsernameinDomain    = "contoso\administrator"
$UsernameinWorkgroup = "192.168.0.1\administrator"

Write-Host "Copying ptfconfig file..." -foregroundcolor yellow

xcopy /y "\\$SERVERcomputer\Data\MS-WSP_ServerTestSuite.deployment.ptfconfig" "$testSuitePath\" 2>&1 | Write-Host
$OSArchitechture = .\Get-OSArchitechture.ps1 $computerName $userNameInVM $userPwdInVM 

Write-Host "Modifying the Ptfconfig..."

.\TurnOff-FileReadonly.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig  

.\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "QueryPath" "file://SUT01/Data"

If ( $IPVersion -eq "IPv4")
{
       .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerComputerName" "192.168.0.1"
}
else
{
       .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerComputerName" "2008::c1"
}

Write-Host "Enabling Windows search...."
$clientOSVersion = .\Get-OSVersion.ps1
if ($clientOSVersion.Equals("VISTA")) 
{
     
       cmd /c net start "Windows Search"

       if($OSArchitechture -eq "x64")
       {

             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientOffset" "64"
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientVersion" "65794"
             if($ServerOS -eq "Win7")
             {
                 .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientVersion" "67328"
             }
       }
       else
       {

             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientOffset" "32"
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientVersion" "258"
       }

}
else # XP
{
       cmd.exe /c "$myToolsPath\WindowsSearch-KB940157-XP-x86-enu.exe" /q

      
       if($OSArchitechture -eq "x64")
       {
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientOffset" "64"
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientVersion" "65801"

       }
       else
       {
            
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientOffset" "32"
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ClientVersion" "265"
       }
}

#-----------------------------------------------------
# Finished to config Client
#-----------------------------------------------------
popd
Write-Host  "Write signal file: config.finished.signal to system drive."
cmd /C ECHO  CONFIG FINISHED>$env:HOMEDRIVE\config.finished.signal

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-Client.ps1] FINISHED (NOT VERIFIED)."
Stop-Transcript

exit 0
