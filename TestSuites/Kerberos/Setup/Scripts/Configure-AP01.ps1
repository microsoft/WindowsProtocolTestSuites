#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Configure-AP01.ps1
## Purpose:        Configure the Local Realm Application Server computer for Kerberos
##                 Server test suite.
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
# KrbParams:          Parameters read from the DataFile
#------------------------------------------------------------------------------------------
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$SignalFileFullPath      = "$WorkingPath\Configure-AP01.finished.signal"
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
Post configuration script to configure the Local Realm Application Server computer for Kerberos Server test suite.

Usage:
    .\Configure-AP01.ps1 [-WorkingPath <WorkingPath>] [-h | -help]

WorkingPath: Working folder. The default value is C:\temp.
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
    Write-ConfigLog "Getting the parameters from Kerberos config file..." -ForegroundColor Yellow
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
	# Switch to the script path
    Write-ConfigLog "Switching to $WorkingPath..." -ForegroundColor Yellow
	
    # Start logging
    Start-ConfigLog

    # Start executing the script
    Write-ConfigLog "Executing [$ScriptName]..." -ForegroundColor Cyan

    # Update ParamConfig.xml
    UpdateConfigFile.ps1 -WorkingPath $WorkingPath

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

#------------------------------------------------------------------------------------------
# Function: Config-AP01
# Configure the environment:
# Triggered by endpoint computer: <ENDPOINT>
#  * Enable Windows authentication and disable Anonymous authentication
#  * Update resource property
#  * Update group policies
#  * Set central access policy on a share folder
#  * Enable compound identity to the file server
#------------------------------------------------------------------------------------------
Function Config-AP01()
{
	#-----------------------------------------------------------------------------------------------
	# Change Computer Account Password
	# WebServer and the FileShare are on the same host
	# Uncomment the FileShare part when they are separated
	#-----------------------------------------------------------------------------------------------
	$password = $KrbParams.Parameters.LocalRealm.WebServer.Password
	ksetup /setcomputerpassword $password

	#disable password change
	Set-ItemProperty -path HKLM:\SYSTEM\CurrentControlSet\services\Netlogon\Parameters -name DisablePasswordChange -value 1

	#-----------------------------------------------------------------------------------------------
	# Install ADDS feature
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Installing ADDS..."
	Install-WindowsFeature -Name AD-Domain-Services -IncludeAllSubFeature

	#-----------------------------------------------------------------------------------------------
	# Enable Windows Authentication and disable Anonymous Authentication
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Enable Windows Authentication and disable Anonymous Authentication..."
	Set-WebConfigurationProperty -filter /system.WebServer/security/authentication/AnonymousAuthentication -name enabled -value false
	Set-WebConfigurationProperty -filter /system.WebServer/security/authentication/windowsAuthentication -name enabled -value true
	$root = $KrbParams.Parameters.LocalRealm.WebServer.wwwroot
	$user = $KrbParams.Parameters.LocalRealm.WebServer.user
	$rights = $KrbParams.Parameters.LocalRealm.WebServer.Rights
	$permission = $KrbParams.Parameters.LocalRealm.WebServer.Permission
	$ar = New-Object  System.Security.Accesscontrol.FileSystemAccessRule($user,$rights,"ContainerInherit, ObjectInherit","None",$permission)
	Write-ConfigLog "Setting Windows Authentication..."
	$objACL = (Get-Item $root).GetAccessControl("Access")

	$objACL.SetAccessRule($ar)
	Set-Acl -Path $root -AclObject $objACL

	#-----------------------------------------------------------------------------------------------
	# Update resource property
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Update resource property"
	Update-FSRMClassificationpropertyDefinition

	#-----------------------------------------------------------------------------------------------
	# Update Group Policies
	#-----------------------------------------------------------------------------------------------
	Write-ConfigLog "Update group policy"
	gpupdate /force

	#-----------------------------------------------------------------------------------------------
	# Set Central Access Policy on a share folder
	#-----------------------------------------------------------------------------------------------
	$shareFolder = "C:\share01"
	mkdir $shareFolder
	New-SmbShare -Name Share -Path $shareFolder -FullAccess Everyone
	$property = ($KrbParams.Parameters.LocalRealm.FileShare.FsrmProperty) + "_*"
	Write-ConfigLog "`$property = $property"

	$value = $KrbParams.Parameters.LocalRealm.FileShare.Value
	$id = Get-FsrmClassificationPropertyDefinition $property
	Write-ConfigLog "$id.Name"

	$cls = New-Object -ComObject Fsrm.FsrmClassificationManager
	$cls.SetFileProperty($shareFolder,$id.Name,$value)
	$policy = $KrbParams.Parameters.LocalRealm.FileShare.Policy
	$acl = (Get-Item $shareFolder).GetAccessControl("Access")
	Set-Acl $shareFolder $acl $policy

	#-----------------------------------------------------------------------------------------------
	# Enable compound identity to the file server
	#-----------------------------------------------------------------------------------------------
	
	# Enable compound identity for file server 
    # This command will be run every 30 minutes to make sure the configuration does not expire

    $FileServerName= $KrbParams.Parameters.LocalRealm.FileShare.NetBiosName 

    $TaskName ="EnableCompoundIdentity"
    
    $Command = "Set-ADComputer -Identity $FileServerName -CompoundIdentitySupported 1" 
    
    $Task = "PowerShell $Command"

    # Create task
    cmd /c schtasks /Create /RL HIGHEST /RU Administrators /SC minute /MO 30 /ST 00:00 /TN $TaskName /TR $Task /IT /F
    Sleep 10
    
    # Run task
    cmd /c schtasks /Run /TN $TaskName  

	# For 2012R2, need to set the policy
	$OsVersion = Get-OSVersionNumber.ps1
	$0S2012R2 = "6.3"

	if([double]$SutOSVersion -ge [double]$0S2012R2)
	{
		Set-ADComputer -Identity $FileServerName  -AuthenticationPolicy ComputerRestrictedPolicy
	}
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

    # Complete configure
    Config-AP01
	
	Complete-Configure
}

Main
