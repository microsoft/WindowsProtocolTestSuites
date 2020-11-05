#############################################################################
##
## Microsoft Windows Powershell Sripting
## File:           Config-Server.ps1
## Author:         v-mankuc
## Purpose:        Configure server for MS-WSP test suite.
## Version:        1.0 (06 Oct, 2008)
## Requirements:   Windows Powershell 2.0 CTP2,Hyper-V
## Supported OS:   Windows 2003 Server,Windows 2008 Server, Win7
##
##############################################################################

Param(
[String]$ToolsPath, 
[String]$ScriptsPath, 
[String]$TestSuitePath,
[String]$LogPath,
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
$logFile = $logPath+"\Config-Server.ps1.log"
Start-Transcript $logFile -force

Write-Host "EXECUTING [Config-Server.ps1] ..." -foregroundcolor cyan
Write-Host "`$toolsPath       = $toolsPath"
Write-Host "`$scriptsPath     = $scriptsPath"
Write-Host "`$TestSuitePath   = $TestSuitePath"
Write-Host "`$logFile         = $logFile"
Write-Host "`$clientOS        = $clientOS"
Write-Host "`$IPVersion       = $IPVersion"
Write-Host "`$workgroupDomain = $workgroupDomain"
Write-Host "`$userNameInVM    = $userNameInVM"
Write-Host "`$userPwdInVM     = $userPwdInVM"
Write-Host "`$domainInVM      = $domainInVM"

Write-Host "Put current dir as $scriptsPath."
pushd $scriptsPath

Write-Host  "Verifying environment..."
.\Verify-NetworkEnvironment.ps1 $IPVersion $workgroupDomain

$dataPath = $scriptsPath.Substring(0, $scriptsPath.Length - $scriptsPath.LastIndexOf('\')) + "Data"
$myToolsPath = $scriptsPath.Substring(0, $scriptsPath.Length - $scriptsPath.LastIndexOf('\')) + "MyTools"
 
#-----------------------------------------------------
# Begin to config server
#-----------------------------------------------------
Write-Host "Begin to config Server..."        

$OSVersion = .\Get-OSVersion.ps1
$sysdrive=$env:systemdrive
.\Share-Folder.ps1 %systemdrive%\test\data Data
$OSArchitechture = .\Get-OSArchitechture.ps1 $computerName $userNameInVM $userPwdInVM 

.\Turnoff-FileReadonly.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig

if(($OSversion -ne $Null) -and ($OSVersion -eq "WS7"))
{
    Write-Host "Installing File services Server and Window search..." -foregroundcolor yellow
    cmd.exe /c "servermanagercmd.exe -install FS-Search-Service -allSubFeatures" 
    cmd /c net start "Windows Search"

   .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerOffset" "64"
   .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerVersion" "67328"

  
}
else
{    $OSSYSVersion = .\Get-OSVersion.ps1 SUT01 $userNameInVM $userPwdInVM
     if ($OSSYSVersion -eq "W2K3") # W2k3    
     {
        Write-Host "Installing Windows desktop search.." -foregroundcolor yellow
        
        if($OSArchitechture -eq "x64")       
        {
             cmd.exe /c "$myToolsPath\WindowsSearch-KB940157-Srv2K3_XP-x64-enu" /q
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerOffset" "64"
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerVersion" "65801"

        }
        else
        {
             cmd.exe /c "$myToolsPath\WindowsSearch-KB940157-Srv2K3-x86-enu.exe" /q
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerOffset" "32"
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerVersion" "265"
        }      

    }
    else # W2K8
    {
    Write-Host "Installing File services Server and Window search..." -foregroundcolor yellow
    cmd.exe /c "servermanagercmd.exe -install FS-Search-Service -allSubFeatures"
    cmd /c net start "Windows Search"

    if($OSArchitechture -eq "x64")
       {
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerOffset" "64"
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerVersion" "65794"

       }
       else
       {
              
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerOffset" "32"
             .\Modify-ConfigFileNode.ps1 $TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig "ServerVersion" "258"
       }


   
    }
}


xcopy /y "$TestSuitePath\MS-WSP_ServerTestSuite.deployment.ptfconfig" "$datapath\" 2>&1 | Write-Host


#----------------------------------------------------------------------------
# Finished to config server
#----------------------------------------------------------------------------
popd
Write-Host "Write signal file: config.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$ENV:HOMEDRIVE\config.finished.signal


#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Config-Server.ps1] FINISHED (NOT VERIFIED)."
Stop-Transcript
exit 0