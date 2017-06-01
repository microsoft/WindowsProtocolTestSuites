#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
###############################################################################################
##
## Microsoft Windows PowerShell Scripting
## File:          Config-Driver.ps1
## Purpose:       Configure the Local Realm Driver computer for Kerberos Server test suite.
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
$ScriptsSignalFile = "$env:HOMEDRIVE\config-drivercomputer.finished.signal"
if(Test-Path -Path $ScriptsSignalFile)
{
	Write-Info.ps1 "The script execution is complete." -foregroundcolor Red
	exit 0
}

$endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\Kerberos\Server-Endpoint"
$version = Get-ChildItem $endPointPath | where {$_.Attributes -eq "Directory" -and $_.Name -match "\d+\.\d+\.\d+\.\d+"} | Sort-Object Name -descending | Select-Object -first 1        
$dataFile = "$WorkingPath\Data\ParamConfig.xml"

#-----------------------------------------------------------------------------------------------
# Please run as Domain Administrator
# Starting script
#-----------------------------------------------------------------------------------------------

if(Test-Path -Path $dataFile)
{
	[xml]$configFile = Get-Content -Path $dataFile
	$TrustPassword = $configFile.Parameters.TrustPassword
    $UseProxy = $configFile.Parameters.UseProxy
    $KKDCPServerUrl = $configFile.Parameters.KKDCPServerUrl

	$LocalRealmName = $configFile.Parameters.LocalRealm.RealmName
	$LocalRealmKDCFQDN = $configFile.Parameters.LocalRealm.KDC.FQDN
	$LocalRealmKDCNetBiosName = $configFile.Parameters.LocalRealm.KDC.NetBiosName
	$LocalRealmKDCPassword = $configFile.Parameters.LocalRealm.KDC.Password
	
	$LocalRealmClientNetBiosName = $configFile.Parameters.LocalRealm.ClientComputer.NetBiosName
	$LocalRealmClientPassword = $configFile.Parameters.LocalRealm.ClientComputer.Password
	$LocalRealmAuthNotRequiredFQDN = $configFile.Parameters.LocalRealm.AuthNotRequired.FQDN
	$LocalRealmAuthNotRequiredNetBiosName = $configFile.Parameters.LocalRealm.AuthNotRequired.NetBiosName
	$LocalRealmAuthNotRequiredPassword = $configFile.Parameters.LocalRealm.AuthNotRequired.Password
	$LocalRealmAuthNotRequiredDefaultServiceName = $configFile.Parameters.LocalRealm.AuthNotRequired.DefaultServiceName
	$LocalRealmAuthNotRequiredServiceSalt = $configFile.Parameters.LocalRealm.AuthNotRequired.ServiceSalt
	$LocalRealmLocalResource01FQDN = $configFile.Parameters.LocalRealm.LocalResource01.FQDN
	$LocalRealmLocalResource01NetBiosName = $configFile.Parameters.LocalRealm.LocalResource01.NetBiosName
	$LocalRealmLocalResource01Password = $configFile.Parameters.LocalRealm.LocalResource01.Password
	$LocalRealmLocalResource01DefaultServiceName = $configFile.Parameters.LocalRealm.LocalResource01.DefaultServiceName
	$LocalRealmLocalResource01ServiceSalt = $configFile.Parameters.LocalRealm.LocalResource01.ServiceSalt
	$LocalRealmLocalResource02FQDN = $configFile.Parameters.LocalRealm.LocalResource02.FQDN
	$LocalRealmLocalResource02NetBiosName = $configFile.Parameters.LocalRealm.LocalResource02.NetBiosName
	$LocalRealmLocalResource02Password = $configFile.Parameters.LocalRealm.LocalResource02.Password
	$LocalRealmLocalResource02DefaultServiceName = $configFile.Parameters.LocalRealm.LocalResource02.DefaultServiceName
	$LocalRealmLocalResource02ServiceSalt = $configFile.Parameters.LocalRealm.LocalResource02.ServiceSalt

	$LocalRealmWebServerNetBiosName = $configFile.Parameters.LocalRealm.WebServer.NetBiosName
	$LocalRealmWebServerPassword = $configFile.Parameters.LocalRealm.WebServer.Password
	$LocalRealmWebServerwwwroot = $configFile.Parameters.LocalRealm.WebServer.wwwroot
	$LocalRealmWebServerUser = $configFile.Parameters.LocalRealm.WebServer.user
	$LocalRealmWebServerRights = $configFile.Parameters.LocalRealm.WebServer.Rights
	$LocalRealmWebServerPermission = $configFile.Parameters.LocalRealm.WebServer.Permission
	$LocalRealmFileShareNetBiosName = $configFile.Parameters.LocalRealm.FileShare.NetBiosName
	$LocalRealmFileSharePassword = $configFile.Parameters.LocalRealm.FileShare.Password
	$LocalRealmFileShareFsrmProperty = $configFile.Parameters.LocalRealm.FileShare.FsrmProperty
	$LocalRealmFileSharePolicy = $configFile.Parameters.LocalRealm.FileShare.Policy
	$LocalRealmFileShareValue = $configFile.Parameters.LocalRealm.FileShare.Value
	$LocalRealmLdapServerNetBiosName = $configFile.Parameters.LocalRealm.LdapServer.NetBiosName
	$LocalRealmLdapServerPassword = $configFile.Parameters.LocalRealm.LdapServer.Password
	
	$LocalRealmResourceGroup01Name = $configFile.Parameters.LocalRealm.ResourceGroup01.GroupName
	$LocalRealmResourceGroup02Name = $configFile.Parameters.LocalRealm.ResourceGroup02.GroupName

	$LocalRealmAdministratorUsername = $configFile.Parameters.LocalRealm.Administrator.Username
	$LocalRealmAdministratorPassword = $configFile.Parameters.LocalRealm.Administrator.Password
	$LocalRealmUser01Username = $configFile.Parameters.LocalRealm.User01.Username
	$LocalRealmUser01Password = $configFile.Parameters.LocalRealm.User01.Password
	$LocalRealmUser01Group = $configFile.Parameters.LocalRealm.User01.Group
	$LocalRealmUser02Username = $configFile.Parameters.LocalRealm.User02.Username
	$LocalRealmUser02Password = $configFile.Parameters.LocalRealm.User02.Password
	$LocalRealmUser03Username = $configFile.Parameters.LocalRealm.User03.Username
	$LocalRealmUser03Password = $configFile.Parameters.LocalRealm.User03.Password
	$LocalRealmUser04Username = $configFile.Parameters.LocalRealm.User04.Username
	$LocalRealmUser04Password = $configFile.Parameters.LocalRealm.User04.Password
	$LocalRealmUser05Username = $configFile.Parameters.LocalRealm.User05.Username
	$LocalRealmUser05Password = $configFile.Parameters.LocalRealm.User05.Password
	$LocalRealmUser06Username = $configFile.Parameters.LocalRealm.User06.Username
	$LocalRealmUser06Password = $configFile.Parameters.LocalRealm.User06.Password
	$LocalRealmUser07Username = $configFile.Parameters.LocalRealm.User07.Username
	$LocalRealmUser07Password = $configFile.Parameters.LocalRealm.User07.Password
	$LocalRealmUser08Username = $configFile.Parameters.LocalRealm.User08.Username
	$LocalRealmUser08Password = $configFile.Parameters.LocalRealm.User08.Password
	$LocalRealmUser09Username = $configFile.Parameters.LocalRealm.User09.Username
	$LocalRealmUser09Password = $configFile.Parameters.LocalRealm.User09.Password
	$LocalRealmUser10Username = $configFile.Parameters.LocalRealm.User10.Username
	$LocalRealmUser10Password = $configFile.Parameters.LocalRealm.User10.Password
	$LocalRealmUser11Username = $configFile.Parameters.LocalRealm.User11.Username
	$LocalRealmUser11Password = $configFile.Parameters.LocalRealm.User11.Password
	$LocalRealmUser12Username = $configFile.Parameters.LocalRealm.User12.Username
	$LocalRealmUser12Password = $configFile.Parameters.LocalRealm.User12.Password
	$LocalRealmUser13Username = $configFile.Parameters.LocalRealm.User13.Username
	$LocalRealmUser13Password = $configFile.Parameters.LocalRealm.User13.Password
	$LocalRealmUser13Group = $configFile.Parameters.LocalRealm.User13.Group	

	$TrustRealmName = $configFile.Parameters.TrustRealm.RealmName
	$TrustRealmKDCNetBiosName = $configFile.Parameters.TrustRealm.KDC.NetBiosName
	$TrustRealmKDCPassword = $configFile.Parameters.TrustRealm.KDC.Password
	$TrustRealmWebServerNetBiosName = $configFile.Parameters.TrustRealm.WebServer.NetBiosName
	$TrustRealmWebServerPassword = $configFile.Parameters.TrustRealm.WebServer.Password
	$TrustRealmWebServerwwwroot = $configFile.Parameters.TrustRealm.WebServer.wwwroot
	$TrustRealmWebServerUser = $configFile.Parameters.TrustRealm.WebServer.user
	$TrustRealmWebServerRights = $configFile.Parameters.TrustRealm.WebServer.Rights
	$TrustRealmWebServerPermission = $configFile.Parameters.TrustRealm.WebServer.Permission
	$TrustRealmFileShareNetBiosName = $configFile.Parameters.TrustRealm.FileShare.NetBiosName
	$TrustRealmFileSharePassword = $configFile.Parameters.TrustRealm.FileShare.Password
	$TrustRealmFileShareFsrmProperty = $configFile.Parameters.TrustRealm.FileShare.FsrmProperty
	$TrustRealmFileSharePolicy = $configFile.Parameters.TrustRealm.FileShare.Policy
	$TrustRealmFileShareValue = $configFile.Parameters.TrustRealm.FileShare.Value
	$TrustRealmLdapServerNetBiosName = $configFile.Parameters.TrustRealm.LdapServer.NetBiosName
	$TrustRealmLdapServerPassword = $configFile.Parameters.TrustRealm.LdapServer.Password

	$TrustRealmAdministratorUsername = $configFile.Parameters.TrustRealm.Administrator.Username
	$TrustRealmAdministratorPassword = $configFile.Parameters.TrustRealm.Administrator.Password
	$TrustRealmUser01Username = $configFile.Parameters.TrustRealm.User01.Username
	$TrustRealmUser01Password = $configFile.Parameters.TrustRealm.User01.Password
	$TrustRealmUser01Group = $configFile.Parameters.TrustRealm.User01.Group
	$TrustRealmUser02Username = $configFile.Parameters.TrustRealm.User02.Username
	$TrustRealmUser02Password = $configFile.Parameters.TrustRealm.User02.Password
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
Write-Info.ps1 "EXECUTING [Config-Driver.ps1] ..." -foregroundcolor cyan
Write-Info.ps1 "`$logPath = $logPath"
Write-Info.ps1 "`$logFile = $logFile"
Write-Info.ps1 "`$TrustPassword =$TrustPassword"
Write-Info.ps1 "`$UseProxy =$UseProxy"
Write-Info.ps1 "`$KKDCPServerUrl =$KKDCPServerUrl"
Write-Info.ps1 "`$LocalRealmName = $LocalRealmName"
Write-Info.ps1 "`$LocalRealmKDCFQDN = $LocalRealmKDCFQDN"
Write-Info.ps1 "`$LocalRealmKDCNetBiosName = $LocalRealmKDCNetBiosName"
Write-Info.ps1 "`$LocalRealmKDCPassword = $LocalRealmKDCPassword"
Write-Info.ps1 "`$LocalRealmClientNetBiosName = $LocalRealmClientNetBiosName"
Write-Info.ps1 "`$LocalRealmClientPassword = $LocalRealmClientPassword"
Write-Info.ps1 "`$LocalRealmAuthNotRequiredFQDN = $LocalRealmAuthNotRequiredFQDN"
Write-Info.ps1 "`$LOcalRealmAuthNotRequiredNetBiosName = $LocalRealmAuthNotRequiredNetBiosName"
Write-Info.ps1 "`$LocalRealmAuthNotRequiredPassword = $LocalRealmAuthNotRequiredPassword"
Write-Info.ps1 "`$LocalRealmAuthNotRequiredDefaultServiceName = $LocalRealmAuthNotRequiredDefaultServiceName"
Write-Info.ps1 "`$LocalRealmAuthNotRequiredServiceSalt = $LocalRealmAuthNotRequiredServiceSalt"
Write-Info.ps1 "`$LocalRealmLocalResource01FQDN = $LocalRealmLocalResource01FQDN"
Write-Info.ps1 "`$LocalRealmLocalResource01NetBiosName = $LocalRealmLocalResource01NetBiosName"
Write-Info.ps1 "`$LocalRealmLocalResource01Password = $LocalRealmLocalResource01Password"
Write-Info.ps1 "`$LocalRealmLocalResource01DefaultServiceName = $LocalRealmLocalResource01DefaultServiceName"
Write-Info.ps1 "`$LocalRealmLocalResource01ServiceSalt = $LocalRealmLocalResource01ServiceSalt"
Write-Info.ps1 "`$LocalRealmLocalResource02FQDN = $LocalRealmLocalResource02FQDN"
Write-Info.ps1 "`$LocalRealmLocalResource02NetBiosName = $LocalRealmLocalResource02NetBiosName"
Write-Info.ps1 "`$LocalRealmLocalResource02Password = $LocalRealmLocalResource02Password"
Write-Info.ps1 "`$LocalRealmLocalResource02DefaultServiceName = $LocalRealmLocalResource02DefaultServiceName"
Write-Info.ps1 "`$LocalRealmLocalResource02ServiceSalt = $LocalRealmLocalResource02ServiceSalt"
Write-Info.ps1 "`$LocalRealmWebServerNetBiosName = $LocalRealmWebServerNetBiosName"
Write-Info.ps1 "`$LocalRealmWebServerPassword = $LocalRealmWebServerPassword"
Write-Info.ps1 "`$LocalRealmWebServerwwwroot = $LocalRealmWebServerwwwroot"
Write-Info.ps1 "`$LocalRealmWebServerUser = $LocalRealmWebServeruser"
Write-Info.ps1 "`$LocalRealmWebServerRights = $LocalRealmWebServerRights"
Write-Info.ps1 "`$LocalRealmWebServerPermission = $LocalRealmWebServerPermission"
Write-Info.ps1 "`$LocalRealmFileShareNetBiosName = $LocalRealmFileShareNetBiosName"
Write-Info.ps1 "`$LocalRealmFileSharePassword = $LocalRealmFileSharePassword"
Write-Info.ps1 "`$LocalRealmFileShareFsrmProperty = $LocalRealmFileShareFsrmProperty"
Write-Info.ps1 "`$LocalRealmFileSharePolicy = $LocalRealmFileSharePolicy"
Write-Info.ps1 "`$LocalRealmFileShareValue = $LocalRealmFileShareValue"
Write-Info.ps1 "`$LocalRealmLdapServerNetBiosName = $LocalRealmLdapServerNetBiosName"
Write-Info.ps1 "`$LocalRealmLdapServerPassword = $LocalRealmLdapServerPassword"
Write-Info.ps1 "`$LocalRealmResourceGroup01Name = $LocalRealmResourceGroup01Name"
Write-Info.ps1 "`$LocalRealmResourceGroup02Name = $LocalRealmResourceGroup02Name"
Write-Info.ps1 "`$LocalRealmAdministratorUsername = $LocalRealmAdministratorUsername"
Write-Info.ps1 "`$LocalRealmAdministratorPassword = $LocalRealmAdministratorPassword"
Write-Info.ps1 "`$LocalRealmUser01Username = $LocalRealmUser01Username"
Write-Info.ps1 "`$LocalRealmUser01Password = $LocalRealmUser01Password"
Write-Info.ps1 "`$LocalRealmUser01Group = $LocalRealmUser01Group"
Write-Info.ps1 "`$LocalRealmUser02Username = $LocalRealmUser02Username"
Write-Info.ps1 "`$LocalRealmUser02Password = $LocalRealmUser02Password"
Write-Info.ps1 "`$LocalRealmUser03Username = $LocalRealmUser03Username"
Write-Info.ps1 "`$LocalRealmUser03Password = $LocalRealmUser03Password"
Write-Info.ps1 "`$LocalRealmUser04Username = $LocalRealmUser04Username"
Write-Info.ps1 "`$LocalRealmUser04Password = $LocalRealmUser04Password"
Write-Info.ps1 "`$LocalRealmUser05Username = $LocalRealmUser05Username"
Write-Info.ps1 "`$LocalRealmUser05Password = $LocalRealmUser05Password"
Write-Info.ps1 "`$LocalRealmUser06Username = $LocalRealmUser06Username"
Write-Info.ps1 "`$LocalRealmUser06Password = $LocalRealmUser06Password"
Write-Info.ps1 "`$LocalRealmUser07Username = $LocalRealmUser07Username"
Write-Info.ps1 "`$LocalRealmUser07Password = $LocalRealmUser07Password"
Write-Info.ps1 "`$LocalRealmUser08Username = $LocalRealmUser08Username"
Write-Info.ps1 "`$LocalRealmUser08Password = $LocalRealmUser08Password"
Write-Info.ps1 "`$LocalRealmUser09Username = $LocalRealmUser09Username"
Write-Info.ps1 "`$LocalRealmUser09Password = $LocalRealmUser09Password"
Write-Info.ps1 "`$LocalRealmUser10Username = $LocalRealmUser10Username"
Write-Info.ps1 "`$LocalRealmUser10Password = $LocalRealmUser10Password"
Write-Info.ps1 "`$LocalRealmUser11Username = $LocalRealmUser11Username"
Write-Info.ps1 "`$LocalRealmUser11Password = $LocalRealmUser11Password"
Write-Info.ps1 "`$LocalRealmUser12Username = $LocalRealmUser12Username"
Write-Info.ps1 "`$LocalRealmUser12Password = $LocalRealmUser12Password"
Write-Info.ps1 "`$LocalRealmUser13Username = $LocalRealmUser13Username"
Write-Info.ps1 "`$LocalRealmUser13Password = $LocalRealmUser13Password"
Write-Info.ps1 "`$LocalRealmUser13Group = $LocalRealmUser13Group"
Write-Info.ps1 "`$TrustRealmName = $TrustRealmName"
Write-Info.ps1 "`$TrustRealmKDCNetBiosName = $TrustRealmKDCNetBiosName"
Write-Info.ps1 "`$TrustRealmKDCPassword = $TrustRealmKDCPassword"
Write-Info.ps1 "`$TrustRealmWebServerNetBiosName = $TrustRealmWebServerNetBiosName"
Write-Info.ps1 "`$TrustRealmWebServerPassword = $TrustRealmWebServerPassword"
Write-Info.ps1 "`$TrustRealmWebServerwwwroot = $TrustRealmWebServerwwwroot"
Write-Info.ps1 "`$TrustRealmWebServerUser = $TrustRealmWebServeruser"
Write-Info.ps1 "`$TrustRealmWebServerRights = $TrustRealmWebServerRights"
Write-Info.ps1 "`$TrustRealmWebServerPermission = $TrustRealmWebServerPermission"
Write-Info.ps1 "`$TrustRealmFileShareNetBiosName = $TrustRealmFileShareNetBiosName"
Write-Info.ps1 "`$TrustRealmFileSharePassword = $TrustRealmFileSharePassword"
Write-Info.ps1 "`$TrustRealmFileShareFsrmProperty = $TrustRealmFileShareFsrmProperty"
Write-Info.ps1 "`$TrustRealmFileSharePolicy = $TrustRealmFileSharePolicy"
Write-Info.ps1 "`$TrustRealmFileShareValue = $TrustRealmFileShareValue"
Write-Info.ps1 "`$TrustRealmLdapServerNetBiosName = $TrustRealmLdapServerNetBiosName"
Write-Info.ps1 "`$TrustRealmLdapServerPassword = $TrustRealmLdapServerPassword"
Write-Info.ps1 "`$TrustRealmAdministratorUsername = $TrustRealmAdministratorUsername"
Write-Info.ps1 "`$TrustRealmAdministratorPassword = $TrustRealmAdministratorPassword"
Write-Info.ps1 "`$TrustRealmUser01Username = $TrustRealmUser01Username"
Write-Info.ps1 "`$TrustRealmUser01Password = $TrustRealmUser01Password"
Write-Info.ps1 "`$TrustRealmUser01Group = $TrustRealmUser01Group"
Write-Info.ps1 "`$TrustRealmUser02Username = $TrustRealmUser02Username"
Write-Info.ps1 "`$TrustRealmUser02Password = $TrustRealmUser02Password"

#-----------------------------------------------------------------------------------------------
# Begin to config Driver computer
#-----------------------------------------------------------------------------------------------

#-----------------------------------------------------------------------------------------------
# Turn off windows firewall
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Turn off firewall"
cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Info.ps1

#-----------------------------------------------------------------------------------------------
# Modify PTF Config File
#-----------------------------------------------------------------------------------------------
$binPath = "$endPointPath\$version\Bin"
$DepPtfConfig = "$binPath\Kerberos_ServerTestSuite.deployment.ptfconfig"

Write-Info.ps1 "TurnOff FileReadonly for $DepPtfConfig..."
.\TurnOff-FileReadonly.ps1 $DepPtfConfig
	
Write-Info.ps1 "Begin to update Kerberos_ServerTestSuite.deployment.ptfconfig..."

if($TrustPassword -ne $null -and $TrustPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.TrustPassword" $TrustPassword
}
if($UseProxy -ne $null -and $UseProxy -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "UseProxy" $UseProxy
}
if($KKDCPServerUrl -ne $null -and $KKDCPServerUrl -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "KKDCPServerUrl" $KKDCPServerUrl
}
if($LocalRealmName -ne $null -and $LocalRealmName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.RealmName" $LocalRealmName
}
if($LocalRealmKDCFQDN -ne $null -and $LocalRealmKDCFQDN -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.KDC01.FQDN" $LocalRealmKDCFQDN
}
if($LocalRealmKDCNetBiosName -ne $null -and $LocalRealmKDCNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.KDC01.NetBiosName" $LocalRealmKDCNetBiosName
}
if($LocalRealmKDCPassword -ne $null -and $LocalRealmKDCPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.KDC01.Password" $LocalRealmKDCPassword
}
if($LocalRealmClientNetBiosName -ne $null -and $LocalRealmClientNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.ClientComputer.NetBiosName" $LocalRealmClientNetBiosName 
}
if($LocalRealmClientPassword -ne $null -and $LocalRealmClientPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.ClientComputer.Password" $LocalRealmClientPassword
}
if($LocalRealmAuthNotRequiredFQDN -ne $null -and $LocalRealmAuthNotRequiredFQDN -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.AuthNotRequired.FQDN" $LocalRealmAuthNotRequiredFQDN
}
if($LocalRealmAuthNotRequiredNetBiosName -ne $null -and $LocalRealmAuthNotRequiredNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.AuthNotRequired.NetBiosName" $LocalRealmAuthNotRequiredNetBiosName
}
if($LocalRealmAuthNotRequiredPassword -ne $null -and $LocalRealmAuthNotRequiredPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.AuthNotRequired.Password" $LocalRealmAuthNotRequiredPassword
}
if($LocalRealmAuthNotRequiredDefaultServiceName -ne $null -and $LocalRealmAuthNotRequiredDefaultServiceName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.AuthNotRequired.DefaultServiceName" $LocalRealmAuthNotRequiredDefaultServiceName
}
if($LocalRealmAuthNotRequiredServiceSalt -ne $null -and $LocalRealmAuthNotRequiredServiceSalt -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.AuthNotRequired.ServiceSalt" $LocalRealmAuthNotRequiredServiceSalt
}
if($LocalRealmLocalResource01FQDN -ne $null -and $LocalRealmLocalResource01FQDN -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource01.FQDN" $LocalRealmLocalResource01FQDN
}
if($LocalRealmLocalResource01NetBiosName -ne $null -and $LocalRealmLocalResource01NetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource01.NetBiosName" $LocalRealmLocalResource01NetBiosName
}
if($LocalRealmLocalResource01Password -ne $null -and $LocalRealmLocalResource01Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource01.Password" $LocalRealmLocalResource01Password
}
if($LocalRealmLocalResource01DefaultServiceName -ne $null -and $LocalRealmLocalResource01DefaultServiceName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource01.DefaultServiceName" $LocalRealmLocalResource01DefaultServiceName
}
if($LocalRealmLocalResource01ServiceSalt -ne $null -and $LocalRealmLocalResource01ServiceSalt -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource01.ServiceSalt" $LocalRealmLocalResource01ServiceSalt
}
if($LocalRealmLocalResource02FQDN -ne $null -and $LocalRealmLocalResource02FQDN -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource02.FQDN" $LocalRealmLocalResource02FQDN
}
if($LocalRealmLocalResource02NetBiosName -ne $null -and $LocalRealmLocalResource02NetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource02.NetBiosName" $LocalRealmLocalResource02NetBiosName
}
if($LocalRealmLocalResource02Password -ne $null -and $LocalRealmLocalResource02Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource02.Password" $LocalRealmLocalResource02Password
}
if($LocalRealmLocalResource02DefaultServiceName -ne $null -and $LocalRealmLocalResource02DefaultServiceName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource02.DefaultServiceName" $LocalRealmLocalResource02DefaultServiceName
}
if($LocalRealmLocalResource02ServiceSalt -ne $null -and $LocalRealmLocalResource02ServiceSalt -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LocalResource02.ServiceSalt" $LocalRealmLocalResource02ServiceSalt
}
if($LocalRealmWebServerNetBiosName -ne $null -and $LocalRealmWebServerNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.NetBiosName" $LocalRealmWebServerNetBiosName
}
if($LocalRealmWebServerPassword -ne $null -and $LocalRealmWebServerPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.Password" $LocalRealmWebServerPassword
}
if($LocalRealmFileShareNetBiosName -ne $null -and $LocalRealmFileShareNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.NetBiosName" $LocalRealmFileShareNetBiosName
}
if($LocalRealmFileSharePassword -ne $null -and $LocalRealmFileSharePassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.Password" $LocalRealmFileSharePassword
}

$OS2012 = "6.2"
$SUTOSVersion = Invoke-Command -ComputerName "dc01" -ScriptBlock {"" + [System.Environment]::OSVersion.Version.Major + "." + [System.Environment]::OSVersion.Version.Minor}
Write-Info.ps1 "SUT OS version is $SUTOSVersion" -ForegroundColor Yellow
if($SUTOSVersion -eq $OS2012)
{
    Write-Info.ps1 "Smb2Dialect is change to Smb30" -ForegroundColor Yellow
    Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.Smb2Dialect" "Smb30"
}

if($LocalRealmLdapServerNetBiosName -ne $null -and $LocalRealmLdapServerNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.NetBiosName" $LocalRealmLdapServerNetBiosName
}
if($LocalRealmLdapServerPassword -ne $null -and $LocalRealmLdapServerPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.Password" $LocalRealmLdapServerPassword
}
if($LocalRealmResourceGroup01Name -ne $null -and $LocalRealmResourceGroup01Name -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.resourceGroups.resourceGroup01.GroupName" $LocalRealmResourceGroup01Name
}
if($LocalRealmResourceGroup02Name -ne $null -and $LocalRealmResourceGroup02Name -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.resourceGroups.resourceGroup02.GroupName" $LocalRealmResourceGroup02Name
}
if($LocalRealmAdministratorUsername -ne $null -and $LocalRealmAdministratorUsername -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.Admin.Username" $LocalRealmAdministratorUsername
}
if($LocalRealmAdministratorPassword -ne $null -and $LocalRealmAdministratorPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.Admin.Password" $LocalRealmAdministratorPassword
}
if($LocalRealmUser01Username -ne $null -and $LocalRealmUser01Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User01.Username" $LocalRealmUser01Username
}
if($LocalRealmUser01Password -ne $null -and $LocalRealmUser01Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User01.Password" $LocalRealmUser01Password
}
if($LocalRealmUser02Username -ne $null -and $LocalRealmUser02Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User02.Username" $LocalRealmUser02Username
}
if($LocalRealmUser02Password -ne $null -and $LocalRealmUser02Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User02.Password" $LocalRealmUser02Password
}
if($LocalRealmUser03Username -ne $null -and $LocalRealmUser03Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User03.Username" $LocalRealmUser03Username
}
if($LocalRealmUser03Password -ne $null -and $LocalRealmUser03Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User03.Password" $LocalRealmUser03Password
}
if($LocalRealmUser04Username -ne $null -and $LocalRealmUser04Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User04.Username" $LocalRealmUser04Username
}
if($LocalRealmUser04Password -ne $null -and $LocalRealmUser04Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User04.Password" $LocalRealmUser04Password
}
if($LocalRealmUser05Username -ne $null -and $LocalRealmUser05Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User05.Username" $LocalRealmUser05Username
}
if($LocalRealmUser05Password -ne $null -and $LocalRealmUser05Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User05.Password" $LocalRealmUser05Password
}
if($LocalRealmUser06Username -ne $null -and $LocalRealmUser06Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User06.Username" $LocalRealmUser06Username
}
if($LocalRealmUser06Password -ne $null -and $LocalRealmUser06Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User06.Password" $LocalRealmUser06Password
}
if($LocalRealmUser07Username -ne $null -and $LocalRealmUser07Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User07.Username" $LocalRealmUser07Username
}
if($LocalRealmUser07Password -ne $null -and $LocalRealmUser07Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User07.Password" $LocalRealmUser07Password
}
if($LocalRealmUser08Username -ne $null -and $LocalRealmUser08Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User08.Username" $LocalRealmUser08Username
}
if($LocalRealmUser08Password -ne $null -and $LocalRealmUser08Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User08.Password" $LocalRealmUser08Password
}
if($LocalRealmUser09Username -ne $null -and $LocalRealmUser09Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User09.Username" $LocalRealmUser09Username
}
if($LocalRealmUser09Password -ne $null -and $LocalRealmUser09Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User09.Password" $LocalRealmUser09Password
}
if($LocalRealmUser10Username -ne $null -and $LocalRealmUser10Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User10.Username" $LocalRealmUser10Username
}
if($LocalRealmUser10Password -ne $null -and $LocalRealmUser10Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User10.Password" $LocalRealmUser10Password
}
if($LocalRealmUser11Username -ne $null -and $LocalRealmUser11Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User11.Username" $LocalRealmUser11Username
}
if($LocalRealmUser11Password -ne $null -and $LocalRealmUser11Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User11.Password" $LocalRealmUser11Password
}
if($LocalRealmUser12Username -ne $null -and $LocalRealmUser12Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User12.Username" $LocalRealmUser12Username
}
if($LocalRealmUser12Password -ne $null -and $LocalRealmUser12Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User12.Password" $LocalRealmUser12Password
}
if($LocalRealmUser13Username -ne $null -and $LocalRealmUser13Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User13.Username" $LocalRealmUser13Username
}
if($LocalRealmUser13Password -ne $null -and $LocalRealmUser13Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.Users.User13.Password" $LocalRealmUser13Password
}
if($TrustRealmName -ne $null -and $TrustRealmName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.RealmName" $TrustRealmName
}
if($TrustRealmKDCNetBiosName -ne $null -and $TrustRealmKDCNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.KDC01.NetBiosName" $TrustRealmKDCNetBiosName
}
if($TrustRealmKDCPassword -ne $null -and $TrustRealmKDCPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.KDC01.Password" $TrustRealmKDCPassword
}
if($TrustRealmWebServerNetBiosName -ne $null -and $TrustRealmWebServerNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.NetBiosName" $TrustRealmWebServerNetBiosName
}
if($TrustRealmWebServerPassword -ne $null -and $TrustRealmWebServerPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.Password" $TrustRealmWebServerPassword
}
if($TrustRealmFileShareNetBiosName -ne $null -and $TrustRealmFileShareNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.NetBiosName" $TrustRealmFileShareNetBiosName
}
if($TrustRealmFileSharePassword -ne $null -and $TrustRealmFileSharePassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.Password" $TrustRealmFileSharePassword
}
if($TrustRealmLdapServerNetBiosName -ne $null -and $TrustRealmLdapServerNetBiosName -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.NetBiosName" $TrustRealmLdapServerNetBiosName
}
if($TrustRealmLdapServerPassword -ne $null -and $TrustRealmLdapServerPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.Password" $TrustRealmLdapServerPassword
}
if($TrustRealmAdministratorUsername -ne $null -and $TrustRealmAdministratorUsername -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.Users.Admin.Username" $TrustRealmAdministratorUsername
}
if($TrustRealmAdministratorPassword -ne $null -and $TrustRealmAdministratorPassword -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.Users.Admin.Password" $TrustRealmAdministratorPassword
}
if($TrustRealmUser01Username -ne $null -and $TrustRealmUser01Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.Users.User01.Username" $TrustRealmUser01Username
}
if($TrustRealmUser01Password -ne $null -and $TrustRealmUser01Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.Users.User01.Password" $TrustRealmUser01Password
}
if($TrustRealmUser02Username -ne $null -and $TrustRealmUser02Username -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.Users.User02.Username" $TrustRealmUser02Username
}
if($TrustRealmUser02Password -ne $null -and $TrustRealmUser02Password -ne "")
{
	Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.Users.User02.Password" $TrustRealmUser02Password
}

#-----------------------------------------------------------------------------------------------
# Copy updated ptfconfig file to TestSuite folder
#-----------------------------------------------------------------------------------------------
$ptfPath="$endPointPath\$version\Source\Server\TestCode\TestSuite"
Write-Info.ps1 "Copy the updated ptfconfig file to $ptfPath" -ForegroundColor Yellow
copy $DepPtfConfig $ptfPath

#-----------------------------------------------------------------------------------------------
# Change Computer Account Password
#-----------------------------------------------------------------------------------------------
$password = $configFile.Parameters.LocalRealm.ClientComputer.Password
ksetup /setcomputerpassword $password

#disable password change
Set-ItemProperty -path HKLM:\SYSTEM\CurrentControlSet\services\Netlogon\Parameters -name DisablePasswordChange -value 1

#-----------------------------------------------------------------------------------------------
# Finished to config Driver computer
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Write signal file: config-drivercomputer.finished.signal to system drive."
cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

#-----------------------------------------------------------------------------------------------
# Ending script
#-----------------------------------------------------------------------------------------------
Write-Info.ps1 "Config finished."
Write-Info.ps1 "EXECUTE [Config-Driver.ps1] FINISHED (NOT VERIFIED)."
Write-Info.ps1 "Computer must restart now..." -ForegroundColor Red

Stop-Transcript