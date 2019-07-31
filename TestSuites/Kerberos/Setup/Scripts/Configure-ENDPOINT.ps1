#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Configure-ENDPOINT.ps1
## Purpose:        Configure the driver computer for Kerberos Server test suite.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016, and later.
##
###########################################################################################

#------------------------------------------------------------------------------------------
# Parameters:
# Help: whether to display the help information
# Step: Current step for configuration
#------------------------------------------------------------------------------------------
Param
(
    [alias("h")]
    [switch]
    $Help,

    [string]$WorkingPath = "C:\temp" 
)
$newEnvPath=$env:Path+";.\;.\scripts\"
$env:Path=$newEnvPath

#------------------------------------------------------------------------------------------
# Global Variables:
# ScriptFileFullPath: Full Path of this script file
# ScriptName:         File Name of this script file 
# SignalFileFullPath: Full Path of the completion signal file for this script file
# LogFileFullPath:    Full Path of the log file for this script file
# Parameters:         Parameters read from the config file
# DataFile:           Full Path of the kerberos config XML file
#------------------------------------------------------------------------------------------
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$SignalFileFullPath      = "$WorkingPath\Configure-ENDPOINT.finished.signal"
$LogFileFullPath         = "$ScriptFileFullPath.log"
$DataFile                = "$WorkingPath\Scripts\ParamConfig.xml"
[xml]$KrbParams          = $null

#------------------------------------------------------------------------------------------
# Function: Display-Help
# Display the help messages.
#------------------------------------------------------------------------------------------
Function Display-Help()
{
    $helpmsg = @"
Post configuration script to configure the Local Realm Driver computer for Kerberos Server test suite.

Usage:
    .\Configure-ENDPOINT.ps1 [-WorkingPath <WorkingPath>] [-h | -help]

Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg "`r`n"
    exit 0
}

#------------------------------------------------------------------------------------------
# Function: Start-ConfigLog
# Create log file and start logging
#------------------------------------------------------------------------------------------
Function Start-ConfigLog()
{
    if (!(Test-Path -Path $LogFileFullPath))
    {
        New-Item -ItemType File -path $LogFileFullPath -Force
    }
    Start-Transcript $LogFileFullPath -Append 2>&1 | Out-Null
}

#------------------------------------------------------------------------------------------
# Function: Write-ConfigLog
# Write information to log file
#------------------------------------------------------------------------------------------
Function Write-ConfigLog
{
    Param (
        [Parameter(ValueFromPipeline=$true)] $text,
        $ForegroundColor = "Green"
    )

    $date = Get-Date -f MM-dd-yyyy_HH_mm_ss
    Write-Output "[$date] $text"
}

#------------------------------------------------------------------------------------------
# Function: Read-ConfigParameters
# Read Config Parameters
#------------------------------------------------------------------------------------------
Function Read-ConfigParameters()
{
	if(Test-Path -Path $DataFile)
    {
        [xml]$Script:KrbParams = Get-Content -Path $DataFile
    }
    else
    {
        Write-ConfigLog "$DataFile not found. Will keep the default setting of all the test context info..."
    }
}

#------------------------------------------------------------------------------------------
# Function: Init-Environment
# Start logging, check signal file, switch to script path and read the config parameters
#------------------------------------------------------------------------------------------
Function Init-Environment()
{
    # Start logging
    Start-ConfigLog
	
	    # Check completion signal file. If signal file exists, exit with 0
    if (Test-Path -Path $SignalFileFullPath)
    {
        Write-ConfigLog "The script execution has been completed." -ForegroundColor Red
        exit 0
    }
	
	# Start executing the script
    Write-ConfigLog "Executing [$ScriptName]..." -ForegroundColor Cyan
	
	# Switch to the script path
	Write-ConfigLog "Switching to $WorkingPath..." -ForegroundColor Yellow
	#Push-Location $WorkingPath

    # Check completion signal file. If signal file exists, exit with 0
    if (Test-Path -Path $SignalFileFullPath) 
    {
        Write-ConfigLog "The script execution has been completed." -ForegroundColor Red
        exit 0
    }

    # Read the config parameters
    Read-ConfigParameters
}

#------------------------------------------------------------------------------------------
# Function: Complete-Configure
# Write signal file, stop the transcript logging and remove the scedule task
#------------------------------------------------------------------------------------------
Function Complete-Configure
{
    # Write signal file
    Write-ConfigLog "Write signal file`: post.finished.signal to hard drive."
    cmd /C ECHO CONFIG FINISHED > $SignalFileFullPath

    # Ending script
    Write-ConfigLog "Config finished."
    Write-ConfigLog "EXECUTE [$ScriptName] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    # remove the schedule task to execute the script next step after restart
    RestartAndRunFinish.ps1
}

Function Config-Driver
{
	$endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\Kerberos\Server-Endpoint"
	$version = Get-ChildItem $endPointPath | where {$_.Name -match "\d+\.\d+\.\d+\.\d+"} | Sort-Object Name -descending | Select-Object -first 1        
	$dataFile = "$WorkingPath\Scripts\ParamConfig.xml"

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
		$LocalRealmKDCIPv4Address = $configFile.Parameters.LocalRealm.KDC.IPv4Address
	
		$LocalRealmClientFQDN = $configFile.Parameters.LocalRealm.ClientComputer.FQDN
		$LocalRealmClientNetBiosName = $configFile.Parameters.LocalRealm.ClientComputer.NetBiosName
		$LocalRealmClientPassword = $configFile.Parameters.LocalRealm.ClientComputer.Password
		$LocalRealmClientIPv4Address = $configFile.Parameters.LocalRealm.ClientComputer.IPv4Address
		$LocalRealmClientDefaultServiceName = $configFile.Parameters.LocalRealm.ClientComputer.DefaultServiceName
		$LocalRealmClientServiceSalt = $configFile.Parameters.LocalRealm.ClientComputer.ServiceSalt
		
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

		$LocalRealmWebServerFQDN = $configFile.Parameters.LocalRealm.WebServer.FQDN
		$LocalRealmWebServerNetBiosName = $configFile.Parameters.LocalRealm.WebServer.NetBiosName
		$LocalRealmWebServerPassword = $configFile.Parameters.LocalRealm.WebServer.Password
		$LocalRealmWebServerIPv4Address = $configFile.Parameters.LocalRealm.WebServer.IPv4Address
		$LocalRealmWebServerwwwroot = $configFile.Parameters.LocalRealm.WebServer.wwwroot
		$LocalRealmWebServerUser = $configFile.Parameters.LocalRealm.WebServer.user
		$LocalRealmWebServerRights = $configFile.Parameters.LocalRealm.WebServer.Rights
		$LocalRealmWebServerPermission = $configFile.Parameters.LocalRealm.WebServer.Permission
		$LocalRealmWebServerDefaultServiceName = $configFile.Parameters.LocalRealm.WebServer.DefaultServiceName
		$LocalRealmWebServerServiceSalt = $configFile.Parameters.LocalRealm.WebServer.ServiceSalt
		$LocalRealmWebServerHttpServiceName = $configFile.Parameters.LocalRealm.WebServer.HttpServiceName
		$LocalRealmWebServerHttpUri = $configFile.Parameters.LocalRealm.WebServer.HttpUri
		
		$LocalRealmFileShareFQDN = $configFile.Parameters.LocalRealm.FileShare.FQDN
		$LocalRealmFileShareNetBiosName = $configFile.Parameters.LocalRealm.FileShare.NetBiosName
		$LocalRealmFileSharePassword = $configFile.Parameters.LocalRealm.FileShare.Password
		$LocalRealmFileShareIPv4Address = $configFile.Parameters.LocalRealm.FileShare.IPv4Address
		$LocalRealmFileShareFsrmProperty = $configFile.Parameters.LocalRealm.FileShare.FsrmProperty
		$LocalRealmFileSharePolicy = $configFile.Parameters.LocalRealm.FileShare.Policy
		$LocalRealmFileShareValue = $configFile.Parameters.LocalRealm.FileShare.Value
		$LocalRealmFileShareDefaultServiceName = $configFile.Parameters.LocalRealm.FileShare.DefaultServiceName
		$LocalRealmFileShareServiceSalt = $configFile.Parameters.LocalRealm.FileShare.ServiceSalt
		$LocalRealmFileShareSmb2ServiceName = $configFile.Parameters.LocalRealm.FileShare.Smb2ServiceName
		
		$LocalRealmLdapServerFQDN = $configFile.Parameters.LocalRealm.LdapServer.FQDN
		$LocalRealmLdapServerNetBiosName = $configFile.Parameters.LocalRealm.LdapServer.NetBiosName
		$LocalRealmLdapServerPassword = $configFile.Parameters.LocalRealm.LdapServer.Password
		$LocalRealmLdapServerIPv4Address = $configFile.Parameters.LocalRealm.LdapServer.IPv4Address
		$LocalRealmLdapServerDefaultServiceName = $configFile.Parameters.LocalRealm.LdapServer.DefaultServiceName
		$LocalRealmLdapServerServiceSalt = $configFile.Parameters.LocalRealm.LdapServer.ServiceSalt
		$LocalRealmLdapServerLdapServiceName = $configFile.Parameters.LocalRealm.LdapServer.LdapServiceName
	
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
		$TrustRealmKDCFQDN = $configFile.Parameters.TrustRealm.KDC.FQDN
		$TrustRealmKDCNetBiosName = $configFile.Parameters.TrustRealm.KDC.NetBiosName
		$TrustRealmKDCPassword = $configFile.Parameters.TrustRealm.KDC.Password
		$TrustRealmKDCIPv4Address = $configFile.Parameters.TrustRealm.KDC.IPv4Address
		$TrustRealmKDCDefaultServiceName = $configFile.Parameters.TrustRealm.KDC.DefaultServiceName
		
		$TrustRealmWebServerFQDN = $configFile.Parameters.TrustRealm.WebServer.FQDN
		$TrustRealmWebServerNetBiosName = $configFile.Parameters.TrustRealm.WebServer.NetBiosName
		$TrustRealmWebServerPassword = $configFile.Parameters.TrustRealm.WebServer.Password
		$TrustRealmWebServerIPv4Address = $configFile.Parameters.TrustRealm.WebServer.IPv4Address
		$TrustRealmWebServerwwwroot = $configFile.Parameters.TrustRealm.WebServer.wwwroot
		$TrustRealmWebServerUser = $configFile.Parameters.TrustRealm.WebServer.user
		$TrustRealmWebServerRights = $configFile.Parameters.TrustRealm.WebServer.Rights
		$TrustRealmWebServerPermission = $configFile.Parameters.TrustRealm.WebServer.Permission
		$TrustRealmWebServerDefaultServiceName = $configFile.Parameters.TrustRealm.WebServer.DefaultServiceName
		$TrustRealmWebServerServiceSalt = $configFile.Parameters.TrustRealm.WebServer.ServiceSalt
		$TrustRealmWebServerHttpServiceName = $configFile.Parameters.TrustRealm.WebServer.HttpServiceName
		$TrustRealmWebServerHttpUri = $configFile.Parameters.TrustRealm.WebServer.HttpUri
		
		$TrustRealmFileShareFQDN = $configFile.Parameters.TrustRealm.FileShare.FQDN
		$TrustRealmFileShareNetBiosName = $configFile.Parameters.TrustRealm.FileShare.NetBiosName
		$TrustRealmFileSharePassword = $configFile.Parameters.TrustRealm.FileShare.Password
		$TrustRealmFileShareIPv4Address = $configFile.Parameters.TrustRealm.FileShare.IPv4Address
		$TrustRealmFileShareFsrmProperty = $configFile.Parameters.TrustRealm.FileShare.FsrmProperty
		$TrustRealmFileSharePolicy = $configFile.Parameters.TrustRealm.FileShare.Policy
		$TrustRealmFileShareValue = $configFile.Parameters.TrustRealm.FileShare.Value
		$TrustRealmFileShareDefaultServiceName = $configFile.Parameters.TrustRealm.FileShare.DefaultServiceName
		$TrustRealmFileShareServiceSalt = $configFile.Parameters.TrustRealm.FileShare.ServiceSalt
		$TrustRealmFileShareSmb2ServiceName = $configFile.Parameters.TrustRealm.FileShare.Smb2ServiceName

		$TrustRealmLdapServerFQDN = $configFile.Parameters.TrustRealm.LdapServer.FQDN
		$TrustRealmLdapServerNetBiosName = $configFile.Parameters.TrustRealm.LdapServer.NetBiosName
		$TrustRealmLdapServerPassword = $configFile.Parameters.TrustRealm.LdapServer.Password
		$TrustRealmLdapServerIPv4Address = $configFile.Parameters.TrustRealm.LdapServer.IPv4Address
		$TrustRealmLdapServerDefaultServiceName = $configFile.Parameters.TrustRealm.LdapServer.DefaultServiceName
		$TrustRealmLdapServerServiceSalt = $configFile.Parameters.TrustRealm.LdapServer.ServiceSalt
		$TrustRealmLdapServerLdapServiceName = $configFile.Parameters.TrustRealm.LdapServer.LdapServiceName

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
		Write-ConfigLog "$dataFile not found.  Will keep the default setting of all the test context info..."
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
	Write-ConfigLog "EXECUTING [Config-Driver.ps1] ..." -foregroundcolor cyan
	Write-ConfigLog "`$logPath = $logPath"
	Write-ConfigLog "`$logFile = $logFile"
	Write-ConfigLog "`$TrustPassword =$TrustPassword"
	Write-ConfigLog "`$UseProxy =$UseProxy"
	Write-ConfigLog "`$KKDCPServerUrl =$KKDCPServerUrl"
	Write-ConfigLog "`$LocalRealmName = $LocalRealmName"
	Write-ConfigLog "`$LocalRealmKDCFQDN = $LocalRealmKDCFQDN"
	Write-ConfigLog "`$LocalRealmKDCNetBiosName = $LocalRealmKDCNetBiosName"
	Write-ConfigLog "`$LocalRealmKDCPassword = $LocalRealmKDCPassword"
	Write-ConfigLog "`$LocalRealmClientFQDN = $LocalRealmClientFQDN"
	Write-ConfigLog "`$LocalRealmClientNetBiosName = $LocalRealmClientNetBiosName"
	Write-ConfigLog "`$LocalRealmClientPassword = $LocalRealmClientPassword"
	Write-ConfigLog "`$LocalRealmAuthNotRequiredFQDN = $LocalRealmAuthNotRequiredFQDN"
	Write-ConfigLog "`$LOcalRealmAuthNotRequiredNetBiosName = $LocalRealmAuthNotRequiredNetBiosName"
	Write-ConfigLog "`$LocalRealmAuthNotRequiredPassword = $LocalRealmAuthNotRequiredPassword"
	Write-ConfigLog "`$LocalRealmAuthNotRequiredDefaultServiceName = $LocalRealmAuthNotRequiredDefaultServiceName"
	Write-ConfigLog "`$LocalRealmAuthNotRequiredServiceSalt = $LocalRealmAuthNotRequiredServiceSalt"
	Write-ConfigLog "`$LocalRealmLocalResource01FQDN = $LocalRealmLocalResource01FQDN"
	Write-ConfigLog "`$LocalRealmLocalResource01NetBiosName = $LocalRealmLocalResource01NetBiosName"
	Write-ConfigLog "`$LocalRealmLocalResource01Password = $LocalRealmLocalResource01Password"
	Write-ConfigLog "`$LocalRealmLocalResource01DefaultServiceName = $LocalRealmLocalResource01DefaultServiceName"
	Write-ConfigLog "`$LocalRealmLocalResource01ServiceSalt = $LocalRealmLocalResource01ServiceSalt"
	Write-ConfigLog "`$LocalRealmLocalResource02FQDN = $LocalRealmLocalResource02FQDN"
	Write-ConfigLog "`$LocalRealmLocalResource02NetBiosName = $LocalRealmLocalResource02NetBiosName"
	Write-ConfigLog "`$LocalRealmLocalResource02Password = $LocalRealmLocalResource02Password"
	Write-ConfigLog "`$LocalRealmLocalResource02DefaultServiceName = $LocalRealmLocalResource02DefaultServiceName"
	Write-ConfigLog "`$LocalRealmLocalResource02ServiceSalt = $LocalRealmLocalResource02ServiceSalt"
	Write-ConfigLog "`$LocalRealmWebServerFQDN =  $LocalRealmWebServerFQDN"
	Write-ConfigLog "`$LocalRealmWebServerNetBiosName = $LocalRealmWebServerNetBiosName"
	Write-ConfigLog "`$LocalRealmWebServerPassword = $LocalRealmWebServerPassword"
	Write-ConfigLog "`$LocalRealmWebServerwwwroot = $LocalRealmWebServerwwwroot"
	Write-ConfigLog "`$LocalRealmWebServerUser = $LocalRealmWebServeruser"
	Write-ConfigLog "`$LocalRealmWebServerRights = $LocalRealmWebServerRights"
	Write-ConfigLog "`$LocalRealmWebServerPermission = $LocalRealmWebServerPermission"
	Write-ConfigLog "`$LocalRealmFileShareFQDN = $LocalRealmFileShareFQDN"
	Write-ConfigLog "`$LocalRealmFileShareNetBiosName = $LocalRealmFileShareNetBiosName"
	Write-ConfigLog "`$LocalRealmFileSharePassword = $LocalRealmFileSharePassword"
	Write-ConfigLog "`$LocalRealmFileShareFsrmProperty = $LocalRealmFileShareFsrmProperty"
	Write-ConfigLog "`$LocalRealmFileSharePolicy = $LocalRealmFileSharePolicy"
	Write-ConfigLog "`$LocalRealmFileShareValue = $LocalRealmFileShareValue"
	Write-ConfigLog "`$LocalRealmLdapServerFQDN = $LocalRealmLdapServerFQDN"
	Write-ConfigLog "`$LocalRealmLdapServerNetBiosName = $LocalRealmLdapServerNetBiosName"
	Write-ConfigLog "`$LocalRealmLdapServerPassword = $LocalRealmLdapServerPassword"
	Write-ConfigLog "`$LocalRealmResourceGroup01Name = $LocalRealmResourceGroup01Name"
	Write-ConfigLog "`$LocalRealmResourceGroup02Name = $LocalRealmResourceGroup02Name"
	Write-ConfigLog "`$LocalRealmAdministratorUsername = $LocalRealmAdministratorUsername"
	Write-ConfigLog "`$LocalRealmAdministratorPassword = $LocalRealmAdministratorPassword"
	Write-ConfigLog "`$LocalRealmUser01Username = $LocalRealmUser01Username"
	Write-ConfigLog "`$LocalRealmUser01Password = $LocalRealmUser01Password"
	Write-ConfigLog "`$LocalRealmUser01Group = $LocalRealmUser01Group"
	Write-ConfigLog "`$LocalRealmUser02Username = $LocalRealmUser02Username"
	Write-ConfigLog "`$LocalRealmUser02Password = $LocalRealmUser02Password"
	Write-ConfigLog "`$LocalRealmUser03Username = $LocalRealmUser03Username"
	Write-ConfigLog "`$LocalRealmUser03Password = $LocalRealmUser03Password"
	Write-ConfigLog "`$LocalRealmUser04Username = $LocalRealmUser04Username"
	Write-ConfigLog "`$LocalRealmUser04Password = $LocalRealmUser04Password"
	Write-ConfigLog "`$LocalRealmUser05Username = $LocalRealmUser05Username"
	Write-ConfigLog "`$LocalRealmUser05Password = $LocalRealmUser05Password"
	Write-ConfigLog "`$LocalRealmUser06Username = $LocalRealmUser06Username"
	Write-ConfigLog "`$LocalRealmUser06Password = $LocalRealmUser06Password"
	Write-ConfigLog "`$LocalRealmUser07Username = $LocalRealmUser07Username"
	Write-ConfigLog "`$LocalRealmUser07Password = $LocalRealmUser07Password"
	Write-ConfigLog "`$LocalRealmUser08Username = $LocalRealmUser08Username"
	Write-ConfigLog "`$LocalRealmUser08Password = $LocalRealmUser08Password"
	Write-ConfigLog "`$LocalRealmUser09Username = $LocalRealmUser09Username"
	Write-ConfigLog "`$LocalRealmUser09Password = $LocalRealmUser09Password"
	Write-ConfigLog "`$LocalRealmUser10Username = $LocalRealmUser10Username"
	Write-ConfigLog "`$LocalRealmUser10Password = $LocalRealmUser10Password"
	Write-ConfigLog "`$LocalRealmUser11Username = $LocalRealmUser11Username"
	Write-ConfigLog "`$LocalRealmUser11Password = $LocalRealmUser11Password"
	Write-ConfigLog "`$LocalRealmUser12Username = $LocalRealmUser12Username"
	Write-ConfigLog "`$LocalRealmUser12Password = $LocalRealmUser12Password"
	Write-ConfigLog "`$LocalRealmUser13Username = $LocalRealmUser13Username"
	Write-ConfigLog "`$LocalRealmUser13Password = $LocalRealmUser13Password"
	Write-ConfigLog "`$LocalRealmUser13Group = $LocalRealmUser13Group"
	Write-ConfigLog "`$TrustRealmName = $TrustRealmName"
	Write-ConfigLog "`$TrustRealmKDCNetBiosName = $TrustRealmKDCNetBiosName"
	Write-ConfigLog "`$TrustRealmKDCPassword = $TrustRealmKDCPassword"
	Write-ConfigLog "`$TrustRealmWebServerNetBiosName = $TrustRealmWebServerNetBiosName"
	Write-ConfigLog "`$TrustRealmWebServerPassword = $TrustRealmWebServerPassword"
	Write-ConfigLog "`$TrustRealmWebServerwwwroot = $TrustRealmWebServerwwwroot"
	Write-ConfigLog "`$TrustRealmWebServerUser = $TrustRealmWebServeruser"
	Write-ConfigLog "`$TrustRealmWebServerRights = $TrustRealmWebServerRights"
	Write-ConfigLog "`$TrustRealmWebServerPermission = $TrustRealmWebServerPermission"
	Write-ConfigLog "`$TrustRealmFileShareNetBiosName = $TrustRealmFileShareNetBiosName"
	Write-ConfigLog "`$TrustRealmFileSharePassword = $TrustRealmFileSharePassword"
	Write-ConfigLog "`$TrustRealmFileShareFsrmProperty = $TrustRealmFileShareFsrmProperty"
	Write-ConfigLog "`$TrustRealmFileSharePolicy = $TrustRealmFileSharePolicy"
	Write-ConfigLog "`$TrustRealmFileShareValue = $TrustRealmFileShareValue"
	Write-ConfigLog "`$TrustRealmLdapServerNetBiosName = $TrustRealmLdapServerNetBiosName"
	Write-ConfigLog "`$TrustRealmLdapServerPassword = $TrustRealmLdapServerPassword"
	Write-ConfigLog "`$TrustRealmAdministratorUsername = $TrustRealmAdministratorUsername"
	Write-ConfigLog "`$TrustRealmAdministratorPassword = $TrustRealmAdministratorPassword"
	Write-ConfigLog "`$TrustRealmUser01Username = $TrustRealmUser01Username"
	Write-ConfigLog "`$TrustRealmUser01Password = $TrustRealmUser01Password"
	Write-ConfigLog "`$TrustRealmUser01Group = $TrustRealmUser01Group"
	Write-ConfigLog "`$TrustRealmUser02Username = $TrustRealmUser02Username"
	Write-ConfigLog "`$TrustRealmUser02Password = $TrustRealmUser02Password"

	#-----------------------------------------------------------------------------------------------
	# Begin to config Driver computer
	#-----------------------------------------------------------------------------------------------

	#-----------------------------------------------------------------------------------------------
	# Turn off windows firewall
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Turn off firewall"
	cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-ConfigLog

	#-----------------------------------------------------------------------------------------------
	# Modify PTF Config File
	#-----------------------------------------------------------------------------------------------
	$binPath = "$endPointPath\$version\Bin"
	$DepPtfConfig = "$binPath\Kerberos_ServerTestSuite.deployment.ptfconfig"

	Write-ConfigLog "TurnOff FileReadonly for $DepPtfConfig..."
	TurnOff-FileReadonly.ps1 $DepPtfConfig
	
	Write-ConfigLog "Begin to update Kerberos_ServerTestSuite.deployment.ptfconfig..."

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
	if($LocalRealmKDCIPv4Address -ne $null -and $LocalRealmKDCIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.KDC01.IPv4Address" $LocalRealmKDCIPv4Address
	}
	if($LocalRealmKDCPassword -ne $null -and $LocalRealmKDCPassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.KDC01.Password" $LocalRealmKDCPassword
	}
	if($LocalRealmClientFQDN -ne $null -and $LocalRealmClientFQDN -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.ClientComputer.FQDN" $LocalRealmClientFQDN 
	}
	if($LocalRealmClientNetBiosName -ne $null -and $LocalRealmClientNetBiosName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.ClientComputer.NetBiosName" $LocalRealmClientNetBiosName 
	}
	if($LocalRealmClientPassword -ne $null -and $LocalRealmClientPassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.ClientComputer.Password" $LocalRealmClientPassword
	}
	if($LocalRealmClientIPv4Address -ne $null -and $LocalRealmClientIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.ClientComputer.IPv4Address" $LocalRealmClientIPv4Address
	}
	if($LocalRealmClientDefaultServiceName -ne $null -and $LocalRealmClientDefaultServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.ClientComputer.DefaultServiceName" $LocalRealmClientDefaultServiceName
	}
	if($LocalRealmClientServiceSalt -ne $null -and $LocalRealmClientServiceSalt -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.ClientComputer.ServiceSalt" $LocalRealmClientServiceSalt
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
	if($LocalRealmWebServerFQDN -ne $null -and $LocalRealmWebServerFQDN -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.FQDN" $LocalRealmWebServerFQDN
	}
	if($LocalRealmWebServerNetBiosName -ne $null -and $LocalRealmWebServerNetBiosName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.NetBiosName" $LocalRealmWebServerNetBiosName
	}
	if($LocalRealmWebServerPassword -ne $null -and $LocalRealmWebServerPassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.Password" $LocalRealmWebServerPassword
	}
	if($LocalRealmWebServerIPv4Address -ne $null -and $LocalRealmWebServerIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.IPv4Address" $LocalRealmWebServerIPv4Address
	}
	if($LocalRealmWebServerDefaultServiceName -ne $null -and $LocalRealmWebServerDefaultServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.DefaultServiceName" $LocalRealmWebServerDefaultServiceName
	}
	if($LocalRealmWebServerServiceSalt-ne $null -and $LocalRealmWebServerServiceSalt -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.ServiceSalt" $LocalRealmWebServerServiceSalt
	}
	if($LocalRealmWebServerHttpServiceName -ne $null -and $LocalRealmWebServerHttpServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.HttpServiceName" $LocalRealmWebServerHttpServiceName
	}
	if($LocalRealmWebServerHttpUri -ne $null -and $LocalRealmWebServerHttpUri -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.WebServer01.HttpUri" $LocalRealmWebServerHttpUri
	}
	if($LocalRealmFileShareFQDN -ne $null -and $LocalRealmFileShareFQDN -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.FQDN" $LocalRealmFileShareFQDN
	}
	if($LocalRealmFileShareNetBiosName -ne $null -and $LocalRealmFileShareNetBiosName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.NetBiosName" $LocalRealmFileShareNetBiosName
	}
	if($LocalRealmFileSharePassword -ne $null -and $LocalRealmFileSharePassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.Password" $LocalRealmFileSharePassword
	}
	if($LocalRealmFileShareIPv4Address -ne $null -and $LocalRealmFileShareIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.IPv4Address" $LocalRealmFileShareIPv4Address
	}
	if($LocalRealmFileShareDefaultServiceName -ne $null -and $LocalRealmFileShareDefaultServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.DefaultServiceName" $LocalRealmFileShareDefaultServiceName
	}
	if($LocalRealmFileShareServiceSalt -ne $null -and $LocalRealmFileShareServiceSalt -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.ServiceSalt" $LocalRealmFileShareServiceSalt
	}
	if($LocalRealmFileShareSmb2ServiceName -ne $null -and $LocalRealmFileShareSmb2ServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.Smb2ServiceName" $LocalRealmFileShareSmb2ServiceName
	}

	$OS2012 = "6.2"
	$SUTOSVersion = Invoke-Command -ComputerName "$LocalRealmKDCFQDN" -ScriptBlock {"" + [System.Environment]::OSVersion.Version.Major + "." + [System.Environment]::OSVersion.Version.Minor}
	Write-ConfigLog "SUT OS version is $SUTOSVersion" -ForegroundColor Yellow
	if($SUTOSVersion -eq $OS2012)
	{
		Write-ConfigLog "Smb2Dialect is change to Smb30" -ForegroundColor Yellow
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.FileServer01.Smb2Dialect" "Smb30"
	}

	if($LocalRealmLdapServerFQDN -ne $null -and $LocalRealmLdapServerFQDN -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.FQDN" $LocalRealmLdapServerFQDN
	}
	if($LocalRealmLdapServerNetBiosName -ne $null -and $LocalRealmLdapServerNetBiosName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.NetBiosName" $LocalRealmLdapServerNetBiosName
	}
	if($LocalRealmLdapServerPassword -ne $null -and $LocalRealmLdapServerPassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.Password" $LocalRealmLdapServerPassword
	}
	if($LocalRealmLdapServerIPv4Address -ne $null -and $LocalRealmLdapServerIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.IPv4Address" $LocalRealmLdapServerIPv4Address
	} 
	if($LocalRealmLdapServerDefaultServiceName -ne $null -and $LocalRealmLdapServerDefaultServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.DefaultServiceName" $LocalRealmLdapServerDefaultServiceName
	} 
	if($LocalRealmLdapServerServiceSalt -ne $null -and $LocalRealmLdapServerServiceSalt -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.ServiceSalt" $LocalRealmLdapServerServiceSalt
	} 
	if($LocalRealmLdapServerLdapServiceName -ne $null -and $LocalRealmLdapServerLdapServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "LocalRealm.LdapServer01.LdapServiceName" $LocalRealmLdapServerLdapServiceName
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
	if($TrustRealmKDCFQDN -ne $null -and $TrustRealmKDCFQDN -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.KDC01.FQDN" $TrustRealmKDCFQDN
	}
	if($TrustRealmKDCNetBiosName -ne $null -and $TrustRealmKDCNetBiosName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.KDC01.NetBiosName" $TrustRealmKDCNetBiosName
	}
	if($TrustRealmKDCPassword -ne $null -and $TrustRealmKDCPassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.KDC01.Password" $TrustRealmKDCPassword
	}
	if($TrustRealmKDCIPv4Address -ne $null -and $TrustRealmKDCIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.KDC01.IPv4Address" $TrustRealmKDCIPv4Address
	}
	if($TrustRealmKDCDefaultServiceName -ne $null -and $TrustRealmKDCDefaultServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.KDC01.DefaultServiceName" $TrustRealmKDCDefaultServiceName
	}
	if($TrustRealmWebServerFQDN -ne $null -and $TrustRealmWebServerFQDN -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.FQDN" $TrustRealmWebServerFQDN
	}
	if($TrustRealmWebServerNetBiosName -ne $null -and $TrustRealmWebServerNetBiosName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.NetBiosName" $TrustRealmWebServerNetBiosName
	}
	if($TrustRealmWebServerPassword -ne $null -and $TrustRealmWebServerPassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.Password" $TrustRealmWebServerPassword
	}
	if($TrustRealmWebServerIPv4Address -ne $null -and $TrustRealmWebServerIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.IPv4Address" $TrustRealmWebServerIPv4Address
	}
	if($TrustRealmWebServerDefaultServiceName -ne $null -and $TrustRealmWebServerDefaultServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.DefaultServiceName" $TrustRealmWebServerDefaultServiceName
	}
	if($TrustRealmWebServerServiceSalt -ne $null -and $TrustRealmWebServerServiceSalt -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.ServiceSalt" $TrustRealmWebServerServiceSalt
	}
	if($TrustRealmWebServerHttpServiceName -ne $null -and $TrustRealmWebServerHttpServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.HttpServiceName" $TrustRealmWebServerHttpServiceName
	}
	if($TrustRealmWebServerHttpUri -ne $null -and $TrustRealmWebServerHttpUri -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.WebServer01.HttpUri" $TrustRealmWebServerHttpUri
	}
	if($TrustRealmFileShareFQDN -ne $null -and $TrustRealmFileShareFQDN -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.FQDN" $TrustRealmFileShareFQDN
	}
	if($TrustRealmFileShareNetBiosName -ne $null -and $TrustRealmFileShareNetBiosName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.NetBiosName" $TrustRealmFileShareNetBiosName
	}
	if($TrustRealmFileSharePassword -ne $null -and $TrustRealmFileSharePassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.Password" $TrustRealmFileSharePassword
	}
	if($TrustRealmFileShareIPv4Address -ne $null -and $TrustRealmFileShareIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.IPv4Address" $TrustRealmFileShareIPv4Address
	}
	if($TrustRealmFileShareDefaultServiceName -ne $null -and $TrustRealmFileShareDefaultServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.DefaultServiceName" $TrustRealmFileShareDefaultServiceName
	}
	if($TrustRealmFileShareServiceSalt -ne $null -and $TrustRealmFileShareServiceSalt -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.ServiceSalt" $TrustRealmFileShareServiceSalt
	}
	if($TrustRealmFileShareSmb2ServiceName -ne $null -and $TrustRealmFileShareSmb2ServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.FileServer01.Smb2ServiceName" $TrustRealmFileShareSmb2ServiceName
	}
	if($TrustRealmLdapServerFQDN -ne $null -and $TrustRealmLdapServerFQDN -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.FQDN" $TrustRealmLdapServerFQDN
	}
	if($TrustRealmLdapServerNetBiosName -ne $null -and $TrustRealmLdapServerNetBiosName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.NetBiosName" $TrustRealmLdapServerNetBiosName
	}
	if($TrustRealmLdapServerPassword -ne $null -and $TrustRealmLdapServerPassword -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.Password" $TrustRealmLdapServerPassword
	}
	if($TrustRealmLdapServerIPv4Address -ne $null -and $TrustRealmLdapServerIPv4Address -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.IPv4Address" $TrustRealmLdapServerIPv4Address
	}
	if($TrustRealmLdapServerDefaultServiceName -ne $null -and $TrustRealmLdapServerDefaultServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.DefaultServiceName" $TrustRealmLdapServerDefaultServiceName
	}
	if($TrustRealmLdapServerServiceSalt -ne $null -and $TrustRealmLdapServerServiceSalt -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.ServiceSalt" $TrustRealmLdapServerServiceSalt
	}
	if($TrustRealmLdapServerLdapServiceName -ne $null -and $TrustRealmLdapServerLdapServiceName -ne "")
	{
		Modify-ConfigFileNodeWithGroup.ps1 $DepPtfConfig "TrustedRealm.LdapServer01.LdapServiceName" $TrustRealmLdapServerLdapServiceName
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
	Write-ConfigLog "Copy the updated ptfconfig file to $ptfPath" -ForegroundColor Yellow
	copy $DepPtfConfig $ptfPath

	#-----------------------------------------------------------------------------------------------
	# Change Computer Account Password
	#-----------------------------------------------------------------------------------------------
	$password = $configFile.Parameters.LocalRealm.ClientComputer.Password
	ksetup /setcomputerpassword $password

	#disable password change
	Set-ItemProperty -path HKLM:\SYSTEM\CurrentControlSet\services\Netlogon\Parameters -name DisablePasswordChange -value 1
}

#------------------------------------------------------------------------------------------
# Main Function
#------------------------------------------------------------------------------------------
Function Main
{
    # Display help information
    if($Help) 
    {
        Display-Help
        return
    }

    # Initialize configure environment
    Init-Environment
	
    # Update ParamConfig.xml
	UpdateConfigFile.ps1 -WorkingPath $WorkingPath
	
	Config-Driver
	
	Complete-Configure
}

Main
