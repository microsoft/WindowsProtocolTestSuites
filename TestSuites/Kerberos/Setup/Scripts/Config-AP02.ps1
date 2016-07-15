#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
###############################################################################################
##
## Microsoft Windows PowerShell Scripting
## File:          Config-AP02.ps1
## Purpose:       Configure the Trust Realm Application Server computer for Kerberos Server test suite.
## Requirements:  Windows PowerShell 2.0
## Supported OS:  Windows Server 2012 or later versions
##
###############################################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$WorkingPath = "c:\temp"
$env:Path += ";c:\temp;c:\temp\Scripts"

#-----------------------------------------------------------------------------------------------
# Check if the script was executed
#-----------------------------------------------------------------------------------------------
$ScriptsSignalFile = "$env:HOMEDRIVE\config-ap02computer.finished.signal"
if(Test-Path -Path $ScriptsSignalFile)
{
	Write-Info.ps1 "The script execution is complete." -foregroundcolor Red
	exit 0
}

#-----------------------------------------------------------------------------------------------
# Please run as Domain Administrator
# Starting script
#-----------------------------------------------------------------------------------------------
$dataFile = "$WorkingPath\Data\ParamConfig.xml"
if(Test-Path -Path $dataFile)
{
	[xml]$configFile = Get-Content -Path $dataFile
	$domainName 	= $configFile.Parameters.LocalRealm.RealmName
}
else
{
	Write-Info.ps1 "$dataFile not found.  Will keep the default setting of all the test context info..."
}

#-----------------------------------------------------------------------------------------------
# Create $logPath if not exist
#-----------------------------------------------------------------------------------------------
$logPath = $configFile.Parameters.LogPath
if(!(Test-Path -Path $logPath))
{
	New-Item -Type Directory -Path $logPath -Force
}

#-----------------------------------------------------------------------------------------------
# Create $logFile if not exist
#-----------------------------------------------------------------------------------------------
$logFile = $WorkingPath + "\" + $MyInvocation.MyCommand.Name + ".log"
if(!(Test-Path -Path $logFile))
{
	New-Item -Type File -Path $logFile -Force
}
Start-Transcript $logFile -Append

#-----------------------------------------------------------------------------------------------
# Write value for all the parameters
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "EXECUTING [Config-AP02.ps1] ..." -foregroundcolor cyan
Write-Info.ps1 "`$logPath = $logPath"
Write-Info.ps1 "`$logFile = $logFile"
Write-Info.ps1 "`$domainName = $domainName"

#-----------------------------------------------------------------------------------------------
# Begin to config AP02 computer
#-----------------------------------------------------------------------------------------------

#-----------------------------------------------------------------------------------------------
# Turn off windows firewall
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Turn off firewall"
cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Info.ps1

#-----------------------------------------------------------------------------------------------
# Change Computer Account Password
# WebServer and the FileShare are on the same host
# Uncomment the FileShare part when they are separated
#-----------------------------------------------------------------------------------------------
$password = $configFile.Parameters.TrustRealm.WebServer.Password
ksetup /setcomputerpassword $password

#disable password change
Set-ItemProperty -path HKLM:\SYSTEM\CurrentControlSet\services\Netlogon\Parameters -name DisablePasswordChange -value 1

#-----------------------------------------------------------------------------------------------
# Install ADDS feature
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Installing ADDS."
Install-WindowsFeature -Name AD-Domain-Services -IncludeAllSubFeature

#-----------------------------------------------------------------------------------------------
# Enable Windows Authentication and disable Anonymous Authentication
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Enable Windows Authentication and disable Anonymous Authentication..."
Set-WebConfigurationProperty -filter /system.WebServer/security/authentication/AnonymousAuthentication -name enabled -value false
Set-WebConfigurationProperty -filter /system.WebServer/security/authentication/windowsAuthentication -name enabled -value true
$root = $configFile.Parameters.TrustRealm.WebServer.wwwroot
$user = $configFile.Parameters.TrustRealm.WebServer.user
$rights = $configFile.Parameters.TrustRealm.WebServer.Rights
$permission = $configFile.Parameters.TrustRealm.WebServer.Permission
$ar = New-Object  System.Security.Accesscontrol.FileSystemAccessRule($user,$rights,"ContainerInherit, ObjectInherit","None",$permission)
Write-Info.ps1 "Setting Windows Authentication..."
$objACL = (Get-Item $root).GetAccessControl("Access")

$objACL.SetAccessRule($ar)
Set-Acl -Path $root -AclObject $objACL

#-----------------------------------------------------------------------------------------------
# Update Resource Property
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Update Resource Property."
Update-FSRMClassificationpropertyDefinition

#-----------------------------------------------------------------------------------------------
# Update Group Policies
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Update group policy"
gpupdate /force

#-----------------------------------------------------------------------------------------------
# Set Central Access Policy on a share folder
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Set Central Access Policy on a share folder"
$shareFolder = "C:\share01"
mkdir $shareFolder
New-SmbShare -Name Share -Path $shareFolder -FullAccess Everyone
$property = ($configFile.Parameters.TrustRealm.FileShare.FsrmProperty) + "_*"
Write-Info.ps1 "`$property = $property"

$value = $configFile.Parameters.TrustRealm.FileShare.Value
$id = Get-FsrmClassificationPropertyDefinition $property
Write-Info.ps1 "$id.Name"

$cls = New-Object -ComObject Fsrm.FsrmClassificationManager
$cls.SetFileProperty($shareFolder,$id.Name,$value)
$policy = $configFile.Parameters.TrustRealm.FileShare.Policy
$acl = (Get-Item $shareFolder).GetAccessControl("Access")
Set-Acl $shareFolder $acl $policy

#-----------------------------------------------------------------------------------------------
# Enable compound identity to the file server
#-----------------------------------------------------------------------------------------------

$IsCompoundEnable= '$true'
$FileServerName= $configFile.Parameters.TrustRealm.FileShare.NetBiosName

# The command to run after restart
$Command= "cmd /c powershell Set-ADComputer -Identity $FileServerName -CompoundIdentitySupported $IsCompoundEnable"

#restart and run command 
Write-Info.ps1 "Computer must restart now..." -ForegroundColor Red
$regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
$regKeyName = "TKFRSAR"

# If the key has already been set, remove it
if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
{
	Remove-ItemProperty -Path $regRunPath -Name $regKeyName
}

try
{
    Set-ItemProperty -Path $regRunPath -Name $regKeyName `
                        -Value "$Command" `
                        -Force -ErrorAction Stop
}
catch
{
    throw "Unable to set registry key $regKeyName. Error happened: $_.Exception.Message"
}


#-----------------------------------------------------------------------------------------------
# Finished to config AP02 computer
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Write signal file: config-ap02computer.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile
Stop-Transcript