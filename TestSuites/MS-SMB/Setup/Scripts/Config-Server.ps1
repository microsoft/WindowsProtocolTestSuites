#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-Server.ps1
## Purpose:        Configure sut for MS-SMB test suite.
## Requirements:   Windows Powershell 2.0 CTP2
## Supported OS:   Windows Server 2008, Windows Server 2008 R2, and Windows 8 Server
##
##############################################################################
$ScriptsSignalFile = "$ENV:HOMEDRIVE\config.finished.signal"
if (Test-Path -Path $ScriptsSignalFile)
{
	Write-Host "The script execution is complete." -foregroundcolor Red
	exit 0
}
[String]$existH = "false"
[string]$scriptsPath = Get-Location
pushd $scriptsPath
$configFile  = "$scriptsPath\ParamConfig.xml"
if(Test-Path -Path $configFile)
{
	$toolsPath       = .\Get-Parameter.ps1 $configFile toolsPath
	$logPath         = .\Get-Parameter.ps1 $configFile logPath
	$IPVersion       = .\Get-Parameter.ps1 $configFile IPVersion
	$workgroupDomain = .\Get-Parameter.ps1 $configFile workgroupDomain	
	$userNameInVM    = .\Get-Parameter.ps1 $configFile userNameInVM
	$userPwdInVM     = .\Get-Parameter.ps1 $configFile userPwdInVM
    $runonREFS       = .\Get-Parameter.ps1 $configFile runonREFS
	$domainInVM      = .\Get-Parameter.ps1 $configFile domainInVM
}
$logFile         = $logPath+"\Config-Server.ps1.log"
$dataPath        = "..\Data"
$myToolsPath     = "..\ToolsPath"
Write-Host "Begin to config server ..." -foregroundcolor yellow

$osMajor = [System.Environment]::OSVersion.Version.Major
$osMinor = [System.Environment]::OSVersion.Version.Minor
$serverOSVersion = "$osMajor.$osMinor"

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
.\Set-Parameter.ps1 $configFile logFile $logFile
.\Set-Parameter.ps1 $configFile dataPath $dataPath
.\Set-Parameter.ps1 $configFile myToolsPath $myToolsPath
.\Write-Log.ps1 "Put current dir as $scriptsPath." Debug $logFile
.\Write-Log.ps1 "`$configFile         = $configFile"  Client
.\Write-Log.ps1 "`$logFile            = $logFile"    Client
.\Write-Log.ps1 "`$ScriptsPath        = $ScriptsPath" Client
.\Write-Log.ps1 "`$logPath            = $logPath"     Client
.\Write-Log.ps1 "`$dataPath           = $dataPath" Client
.\Write-Log.ps1 "`$IPVersion          = $IPVersion" Client
.\Write-Log.ps1 "`$workgroupDomain    = $workgroupDomain" Client

#-----------------------------------------------------
# Begin to config server
#-----------------------------------------------------
.\Write-Log.ps1 "Begin to config server ..."
	
Write-Host "Turn off firewall"
cmd /c netsh.exe advfirewall set allprofiles state off
    
Write-Host "Turn on file and printer sharing..."
.\Config-FileSharing on

Write-Host "Install DFS Components and config DFS namespace ..." Client
$serverName = "$ENV:ComputerName"    
Write-Host "Start to install FS-DFS"
Import-Module Servermanager

if([double]$serverOsVersion -ge [double]$os2012)
{
	Add-WindowsFeature FS-DFS-Namespace
	Add-WindowsFeature FS-DFS-Replication
}
else
{
	Add-WindowsFeature FS-DFS	
}

Write-Host "Start DFS service..." 
cmd /C sc start dfs
.\Create-Folder.ps1 $env:homedrive\DFSNameSpace
.\Share-Folder.ps1 $env:homedrive\DFSNameSpace DFSNameSpace
.\Create-DFSNamespace.ps1 \\$serverName\DFSNameSpace
	
#-----------------------------------------------------
# Create disk H with FAT/FAT32/ReFS file system
#-----------------------------------------------------
if(([double]$serverOsVersion -ge [double]$os2012) -and ($runonREFS -eq $true))
{
    .\Write-Log.ps1 "Create a ReFS disk" Client
}
else
{
	.\Write-Log.ps1 "Create a FAT32 disk" Client	
}

$disk1=Get-WmiObject -Class Win32_DiskDrive
$disk2=get-wmiobject -class Win32_LogicalDisk
for( $i=0; $i -lt $disk2.length;$i++)
{
    if($disk2[$i].name -eq "H:")
    {
        $existH="true"
        Write-Host "The disk H: has exist ..."
        break
    }
}
$Cid=1
if($disk1.Partitions -gt 1)
{
    $Cid=2
}
$Hid=$disk1.Partitions+1
if($existH -eq "false")
{
    $source="SELECT DISK 0`r`nSELECT PART $Cid`r`nsHRINK MINIMUM=101`r`nCREATE PART EXTEND SIZE=100`r`nCREATE PART LOGI`r`nSELECT PART $Hid`r`nFORMAT FS=FAT32 QUICK LABEL=`"SMBTEST`"`r`nASSIGN LETTER=H"

    if(([double]$serverOsVersion -ge [double]$os2012) -and ($runonREFS -eq $true))
	{
		$source="SELECT DISK 0`r`nSELECT PART $Cid`r`nsHRINK MINIMUM=2001`r`nCREATE PART EXTEND SIZE=2000`r`nCREATE PART LOGI`r`nSELECT PART $Hid`r`nFORMAT FS=ReFS QUICK LABEL=`"SMBTEST`"`r`nASSIGN LETTER=H"
    }
    out-file -filepath $scriptsPath\diskpart_script2k8.txt -inputobject $source -encoding ASCII
    diskpart.exe /s $scriptsPath\diskpart_script2k8.txt
    
    if(([double]$serverOsVersion -ge [double]$os2012) -and ($runonREFS -eq $true))
	{
       cmd /C fsutil USN createjournal m=5242880 a=1048576 H:
    }
}
		
#-----------------------------------------------------
# Create Share folders ...
# Share is Configured in MS-SMB_TestSuite.deployment.ptfconfig, property share
# fileName is Configured in MS-SMB_TestSuite.deployment.ptfconfig, property fileName
# f1fileName is according to fileName property
# fileName.txt is Configured in MS-SMB_TestSuite.deployment.ptfconfig, property createFileName
# f1fileName.txt is according to createFileName property
#-----------------------------------------------------

$sharefolderPath1 = "$ENV:HomeDrive\ShareFolder1"
$sharefolderPath2 = "$ENV:HomeDrive\ShareFolder2"
$homeDrive = "$ENV:HomeDrive"
$firstTwoShareFolderFileSystem = "NTFS"
$extendedDiskFileSystem = "FAT32"

if(([double]$serverOsVersion -ge [double]$os2012) -and ($runonREFS -eq $true))
{
	$sharefolderPath1 = "H:\ShareFolder1"
    $sharefolderPath2 = "H:\ShareFolder2"
    $homeDrive = "H:"
    $firstTwoShareFolderFileSystem = "ReFS"
    $extendedDiskFileSystem = "ReFS"
}

.\Write-Log.ps1 "Create share folders in $firstTwoShareFolderFileSystem disk..." Client
.\Write-Log.ps1 "1. Create folder $sharefolderPath1" Client
.\Write-Log.ps1 "2. Create folder $sharefolderPath2" Client
.\Write-Log.ps1 "3. Share folder ShareFolder1 to Everyone for full permission" Client
.\Write-Log.ps1 "4. Share folder ShareFolder2 to Everyone for full permission" Client
.\Write-Log.ps1 "5. Create file $sharefolderPath1\ExistTest.txt" Client
.\Write-Log.ps1 "6. Create file $sharefolderPath2\ExistTest.txt" Client

$disk2=get-wmiobject -class Win32_LogicalDisk
$existH="false"
for( $i=0; $i -lt $disk2.length;$i++)
{
    if($disk2[$i].name -eq "H:")
    {
    	$existH="true"
        Write-Host "The disk H: has exist ..."
        break
    }
}
    
if(($extendedDiskFileSystem -eq "ReFS") -and ($existH -eq "false"))
{     
     Write-Host "You did not create ReFS disk H manually.So ShareFoder1 and ShareFoder2 can not be created with scripts."      
}
else
{
    if(!(Test-Path -Path $sharefolderPath1))
    {
	    New-Item -Type Directory -Path $sharefolderPath1 -Force
    }
    if(!(Test-Path -Path $sharefolderPath2))
    {
	    New-Item -Type Directory -Path $sharefolderPath2 -Force
    }
    cmd /C "net share ShareFolder1=$sharefolderPath1 /grant:everyone,FULL"
    cmd /C "net share ShareFolder2=$sharefolderPath2 /grant:everyone,FULL"
    if(!(Test-Path -Path $sharefolderPath1\ExistTest.txt))
    {
	    New-Item -type file -Path $sharefolderPath1\ExistTest.txt -Force
    }
    if(!(Test-Path -Path $sharefolderPath2\ExistTest.txt))
    {
	    New-Item -type file -Path $sharefolderPath2\ExistTest.txt -Force
    }
}

.\Write-Log.ps1 "Create share folders in $extendedDiskFileSystem disk..." Client
.\Write-Log.ps1 "1. Create folder H:\ShareFolder3" Client
.\Write-Log.ps1 "2. Create folder H:\ShareFolder4" Client
.\Write-Log.ps1 "3. Share folder ShareFolder3 to Everyone for full permission" Client
.\Write-Log.ps1 "4. Share folder ShareFolder4 to Everyone for full permission" Client
.\Write-Log.ps1 "5. Create file H:\ShareFolder3\ExistTest.txt" Client
.\Write-Log.ps1 "6. Create file H:\ShareFolder4\ExistTest.txt" Client
    
if($existH -eq "false")
{
    Write-Host "You did not create $extendedDiskFileSystem disk H manually.So ShareFoder3 and ShareFolder4 can not be created with scripts."   
}
else
{
    New-Item -Type Directory -Path H:\ShareFolder3 -Force
    New-Item -Type Directory -Path H:\ShareFolder4 -Force
    cmd /C "net share ShareFolder3=H:\ShareFolder3 /grant:everyone,FULL"
    cmd /C "net share ShareFolder4=H:\ShareFolder4 /grant:everyone,FULL"
    New-Item -type file -Path H:\ShareFolder3\ExistTest.txt -Force
    New-Item -type file -Path H:\ShareFolder4\ExistTest.txt -Force
}

#-----------------------------------------------------
# Create previous versions
#-----------------------------------------------------
.\Write-Log.ps1 "Create previous versions ..." Client
.\Write-Log.ps1 "1. Enable the shadow copy of $homeDrive" Client
.\Write-Log.ps1 "2. Modify the content in $sharefolderPath1\ExistTest.txt and $sharefolderPath2\ExistTest.txt, then create 3 shadow copies" Client
for($i = 1; $i -le 3; $i++)
{
	dir >> $sharefolderPath1\ExistTest.txt
	dir >> $sharefolderPath2\ExistTest.txt
	.\Create-ShadowCopy.ps1 $homeDrive
}
Get-Date | Set-Content $sharefolderPath1\ShadowsReady

#-----------------------------------------------------
# Add and share printer SMBPrinter ...
# "smbPrinter" is configured in MS-SMB_TestSuite.deployment.ptfconfig, property printer
#-----------------------------------------------------
.\Write-Log.ps1 "Add and share printer SMBPrinter ..." Client
.\Write-Log.ps1 "Manual Steps:" Client
.\Write-Log.ps1 "1. Add a local printer" Client
.\Write-Log.ps1 "2. Share the local printer" Client

if([double]$serverOsVersion -eq [double]$os2008R2) 
{
	.\Add-Printer.ps1 "SMBPrinter" "Brother MFC-465CN"
}
elseif(([double]$serverOsVersion -ge [double]$os2012))
{
	.\Add-Printer.ps1 "SMBPrinter" "Brother Color Type3 Class Driver"
}
else
{
	.\Add-Printer.ps1 "SMBPrinter" "Apollo P-1200"
}

#-----------------------------------------------------
# Enable Guest account
#-----------------------------------------------------
.\Write-Log.ps1 "Enable Guest account..." Client
.\Write-Log.ps1 "Manual Steps:" Client
.\Write-Log.ps1 "1. Enable the Guest account" Client
.\Write-Log.ps1 "2. Set a random password for Guest account" Client
net.exe user Guest /active:yes 2>&1 | Write-Host
net.exe user Guest Password01! 2>&1 | Write-Host

#-----------------------------------------------------
# modify regedit
#-----------------------------------------------------
.\Write-Log.ps1 "Modify regedit..." Client
.\Write-Log.ps1 "Manual Steps:" Client
.\Write-Log.ps1 "1. Modify regedit..." Client
REG ADD HKLM\System\CurrentControlSet\Services\LanmanServer\Parameters\  /v NoAliasingOnFilesystem /t REG_DWORD /d 1 /f

#----------------------------------------------------------------------------
# Finished to config server
#----------------------------------------------------------------------------
.\Write-Log.ps1 "Write signal file: config.finished.signal to system drive."
cmd /C ECHO  CONFIG FINISHED>$ENV:HOMEDRIVE\config.finished.signal

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
.\Write-Log.ps1 "Config finished."
.\Write-Log.ps1 "EXECUTE [Config-Server.ps1] FINISHED (NOT VERIFIED)."
popd  

exit 0