#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-Client.ps1
## Purpose:        Configure driver computer for MS-SMB test suite.
## Requirements:   Windows Powershell 2.0 CTP2
## Supported OS:   Windows 7 and later versions
##
##############################################################################
$ScriptsSignalFile = "$env:HOMEDRIVE\config.finished.signal"
if (Test-Path -Path $ScriptsSignalFile)
{
	Write-Host "The script execution is complete." -foregroundcolor Red
	exit 0
}

[string]$scriptsPath = Get-location
pushd $scriptsPath
[string]$binPath = $scriptsPath+ "\..\bin\"
$settingFile  = "$scriptsPath\ParamConfig.xml"
if(Test-Path -Path $settingFile)
{
	$toolsPath       = .\Get-Parameter.ps1 $settingFile toolsPath
	$logPath         = .\Get-Parameter.ps1 $settingFile logPath
	$IPVersion       = .\Get-Parameter.ps1 $settingFile IPVersion
	$workgroupDomain = .\Get-Parameter.ps1 $settingFile workgroupDomain
	$servername      = .\Get-Parameter.ps1 $settingFile serverComputerName	
	$userNameInVM    = .\Get-Parameter.ps1 $settingFile userNameInVM
	$userPwdInVM     = .\Get-Parameter.ps1 $settingFile userPwdInVM
	$domainInVM      = .\Get-Parameter.ps1 $settingFile domainInVM
}

$osMajor = [System.Environment]::OSVersion.Version.Major
$osMinor = [System.Environment]::OSVersion.Version.Minor
$clientOSVersion = "$osMajor.$osMinor"

$os2008R2 = "6.1" # Win2008R2, win 7
$os2012 = "6.2"# Win2012, Win8
$os2012R2 = "6.3"# Win2012R2, WinBlue

#-----------------------------------------------------	
# Create testResults folder ...
#-----------------------------------------------------
$testResultsPath = $scriptsPath+ "\..\TestResults"
if(!(Test-Path -Path $testResultsPath))
{
	New-Item -Type Directory -Path $testResultsPath -Force
}
if(!(Test-Path -Path $logPath))
{
	New-Item -Type Directory -Path $logPath -Force
}

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
$logFile = $logPath+"\Config-Client.ps1.log"
Write-Host "EXECUTING [Config-Client.ps1] ..." -foregroundcolor cyan
Write-Host "`$toolsPath       = $toolsPath"
Write-Host "`$scriptsPath     = $scriptsPath"
Write-Host "`$binPath         = $binPath"
Write-Host "`$logFile         = $logFile"
Write-Host "`$IPVersion       = $IPVersion"
Write-Host "`$workgroupDomain = $workgroupDomain"
Write-Host "`$userNameInVM    = $userNameInVM"
Write-Host "`$userPwdInVM     = $userPwdInVM"
Write-Host "`$domainInVM      = $domainInVM"

#-----------------------------------------------------
# Definition
#-----------------------------------------------------
function FileStringReplace
(
[string]$filePath,
[string]$origStr,
[string]$newStr
)
{
    if(!(Test-Path -Path $filePath -PathType Leaf))
    {
        throw File ($filePath) not found.
    }
    if(!$origStr)
    {
        throw String to be replaced can not be empty.
    }
    if(!$newStr)
    {
        throw New string can not be empty.
    }

    $tmpFile = ($filePath + ".tmp")
    $lines = Get-Content $filePath
    foreach($line in $lines)
    {
        $newLine = $line.Replace($origStr, $newStr)
        Add-Content $tmpFile $newLine
    }
    Remove-Item $filePath
    if($filePath.Contains("\"))
    {
       $filePath = $filePath.Remove(0, $filePath.LastIndexOf("\") + 1)
    }
    Rename-Item $tmpFile $filePath
}

#-----------------------------------------------------
# Begin to config Client
#-----------------------------------------------------
Write-Host  "Begin to config client..."   

#-----------------------------------------------------
# Turn off firewall
#-----------------------------------------------------
.\Write-Log.ps1 "Turn off firewall" Client
cmd /c netsh.exe advfirewall set allprofiles state off

#-----------------------------------------------------
# Begin to Modify deployment.ptfconfig file...
#-----------------------------------------------------
Write-Host  "Modify deployment.ptfconfig file..." 
.\Write-Log.ps1 "Config deployment.ptfconfig file" Client
.\Write-Log.ps1 "Manual Steps:" Client
.\Write-Log.ps1 "1. change value SmbClientOS to your client OS plateform(Win7 or Vista)" Client
.\Write-Log.ps1 "2. change value SutPlatformOS to your Server OS plateform(Win2K8R2 or Win2K8)" Client
.\Write-Log.ps1 "3. change value SmbTransportIpVersion to your IPv4 or IPv6. If is IPV4, change to 192.168.0.1 or if is IPv6, change to 2008::1" Client
# Get the SmbTestCasePreviousVersion according to the test environment.

if([double]$clientOsVersion -ge [double]$os2008R2)
{
    $PreviousVersion = "1,@GMT-2010.08.02-11.56.42;"
}
else
{
    $PreviousVersion = "1,@GMT-2010.08.19-10.00.09;"
}

if($IPVersion -eq "IPv6")
{
    $ipconfig = "Ipv6"
}
else
{
    $ipconfig = "Ipv4"
}

# Modify deployment.ptfconfig file
[string]$cfgfile = "$binPath\MS-SMB_ServerTestSuite.deployment.ptfconfig"
.\Modify-ConfigFileNode.ps1 $cfgfile "SmbTestCasePreviousVersion"    $PreviousVersion

if ([double]$clientOsVersion -le [double]$os2008)
{
   .\Modify-ConfigFileNode.ps1 $cfgfile "SmbClientOS"      "WinVista"
}
elseif ([double]$clientOsVersion -ge [double]$os2008R2)
{
   .\Modify-ConfigFileNode.ps1 $cfgfile "SmbClientOS"      "Win7"
}


$UserName = "SUT01\administrator"
$Password = ConvertTo-SecureString -String "Password01!" -AsPlainText -Force

$Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $UserName , $Password

#SUTOsVersion is like 6.3.9600
$SUTOsVersion =  (Get-WmiObject -comp "SUT01" -Credential $Credential -class Win32_OperatingSystem ).Version
$FirstDotIndex = $SUTOSVersion.IndexOf('.')
$SecondDotIndex = $SUTOSVersion.IndexOf('.', $FirstDotIndex+1)
if ($SecondDotIndex -eq -1)
{
	## The version has only one dot, no need to cut the tail
}
else
{
	$SUTOsVersion = $SUTOsVersion.substring(0,$SecondDotIndex)
}

if([double]$SUTOsVersion -ge [double]$os2008R2)
{
   .\Modify-ConfigFileNode.ps1 $cfgfile "SutPlatformOS"   "Win2K8R2"
}
else
{
   .\Modify-ConfigFileNode.ps1 $cfgfile "SutPlatformOS"   "Win2K8"
}

if($IPVersion -eq "IPv6")
{
   FileStringReplace $binPath\factory.xml 192.168.0.1 2008::1 
}

if ([double]$clientOsVersion -eq [double]$os2008)
{
   if([double]$SUTOsVersion -ge [double]$os2008R2)
   {
       set-content $env:systemdrive\testlist.txt "WinVista_Win2K8R2"
   }  
   else
   {
       set-content $env:systemdrive\testlist.txt "WinVista_Win2K8"
   }
}
elseif ([double]$clientOsVersion  -ge [double]$os2008R2)
{
   if([double]$SUTOsVersion -ge [double]$os2008R2)
   {
       set-content $env:systemdrive\testlist.txt "Win7_Win2K8R2"
   }   
   else
   {
       set-content $env:systemdrive\testlist.txt "Win7_Win2K8"
   }
}
else
{
       set-content $env:systemdrive\testlist.txt "Win7_Win2K8"
}


.\Modify-ConfigFileNode.ps1 $cfgfile "SutMachineName"         $servername
.\Modify-ConfigFileNode.ps1 $cfgfile "SutLoginAdminUserName"  $userNameInVM
if($workgroupDomain -eq "Domain")
{
	$userFullPathName = $domainInVM + "\" + $userNameInVM
}
else
{
	$userFullPathName = $servername + "\" + $userNameInVM
}
.\Modify-ConfigFileNode.ps1 $cfgfile "SutLoginAdminUserFullPathName"  $userFullPathName
.\Modify-ConfigFileNode.ps1 $cfgfile "SutLoginAdminPwd"               $userPwdInVM
.\Modify-ConfigFileNode.ps1 $cfgfile "SutLoginGuestPwd"               $userPwdInVM
.\Modify-ConfigFileNode.ps1 $cfgfile "SutLoginDomain"                 $domainInVM
.\Modify-ConfigFileNode.ps1 $cfgfile "SutNtfsShare1FullName"   		  "\\$servername\Sharefolder1"
.\Modify-ConfigFileNode.ps1 $cfgfile "SutNtfsShare2FullName"          "\\$servername\Sharefolder2"
.\Modify-ConfigFileNode.ps1 $cfgfile "SutNtfsQuotaShareFullName"      "\\$servername\QuotaShare"
.\Modify-ConfigFileNode.ps1 $cfgfile "SutFatShare1FullName"           "\\$servername\Sharefolder3"
.\Modify-ConfigFileNode.ps1 $cfgfile "SutFatShare2FullName"           "\\$servername\Sharefolder4"
.\Modify-ConfigFileNode.ps1 $cfgfile "SutShareDfsTreeConnect"         "\\$servername\DfsNameSpace"
.\Modify-ConfigFileNode.ps1 $cfgfile "SutSharePrinterFullName"        "\\$servername\SMBPrinter"
.\Modify-ConfigFileNode.ps1 $cfgfile "SutNamedPipeFullName"           "\\$servername\SMBPrinter"

if([double]$SUTOsVersion -ge [double]$os2012)
{
	.\Modify-ConfigFileNode.ps1 $cfgfile "SmbTransportShareAccess"        "3"
}

 Write-Host "Check new register key to disable metadata cache ..."
 REG ADD HKLM\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters\  /v FileInfoCacheLifetime /t REG_DWORD /d 0 /f
 REG ADD HKLM\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters\  /v FileNotFoundCacheLifetime /t REG_DWORD /d 0 /f
 REG ADD HKLM\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters\  /v DirectoryCacheLifetime /t REG_DWORD /d 0 /f
     
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
sleep 5

exit 0
