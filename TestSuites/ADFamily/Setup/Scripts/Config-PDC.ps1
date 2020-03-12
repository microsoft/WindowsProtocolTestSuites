###########################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
###########################################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-PDC.ps1
## Purpose:        Configure Primary Domain Controller in Local Domain for Active Directory Family Test Suite.
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

    [switch]
    $EnableDebugging,
    
    [int]
    $Step = 1,

    [String]
    $ConfigFile = "c:\temp\Protocol.xml"
)

#------------------------------------------------------------------------------------------
# Global Variables:
# ScriptFileFullPath: Full Path of this script file
# ScriptName:         File Name of this script file 
# ScriptPath:         File Path of this script file
# SignalFileFullPath: Full Path of the completion signal file for this script file
# LogFileFullPath:    Full Path of the log file for this script file
# Parameters:         Parameters read from the config file
#------------------------------------------------------------------------------------------
$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$ScriptPath              = Split-Path $ScriptFileFullPath
$SignalFileFullPath      = "$ScriptPath\post.finished.signal"
$LogFileFullPath         = "$ScriptFileFullPath.log"
$Parameters              = @{}
$IsAzure                 = $false

try {
    [xml]$content = Get-Content $ConfigFile -ErrorAction Stop
    $currentCore = $content.lab.core
    if(![string]::IsNullOrEmpty($currentCore.regressiontype) -and ($currentCore.regressiontype -eq "Azure")){
        $IsAzure = $true;
        $SignalFileFullPath = "C:\PostScript.Completed.signal"
    }
}
catch {
    
}
#------------------------------------------------------------------------------------------
# Function: Display-Help
# Display the help messages.
#------------------------------------------------------------------------------------------
Function Display-Help()
{
    $helpmsg = @"
Post configuration script to configure Primary Domain Controller in Local Domain for Active Directory Family Test Suite.

Usage:
    .\Config-PDC.ps1 [-Step <step>] [-h | -help]

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
    if(!(Test-Path -Path $LogFileFullPath)){
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
    Write-ConfigLog "Getting the parameters from config file..." -ForegroundColor Yellow
    if($IsAzure)
    {
        .\GetVmParametersByComputerName.ps1 -RefParamArray ([ref]$Parameters)
    } else {
    $VMName = .\GetVMNameByComputerName.ps1
        .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$Parameters)
    }
    $Parameters
}

#------------------------------------------------------------------------------------------
# Function: Init-Environment
# Start logging, check signal file, switch to script path and read the config parameters
#------------------------------------------------------------------------------------------
Function Init-Environment()
{
    # Start logging
    Start-ConfigLog

    # Start executing the script
    Write-ConfigLog "Executing [$ScriptName]..." -ForegroundColor Cyan

    # Check completion signal file. If signal file exists, exit with 0
    if (($Step -lt 3) -and (Test-Path -Path $SignalFileFullPath)){
        Write-ConfigLog "The script execution has been completed." -ForegroundColor Red
        exit 0
    }

    # Switch to the script path
    Write-ConfigLog "Switching to $ScriptPath..." -ForegroundColor Yellow
    Push-Location $ScriptPath

    # Read the config parameters
    Read-ConfigParameters
}

#------------------------------------------------------------------------------------------
# Function: Proceed-ScriptWithRestart
# Restart computer and proceed the script to the next step
#------------------------------------------------------------------------------------------
Function Proceed-ScriptWithRestart
{
    $NextStep = $Step + 1

    # add a schedule task to execute the script next step after restart
    .\RestartAndRun.ps1 -ScriptPath $ScriptFileFullPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
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
    .\RestartAndRunFinish.ps1
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase1
# Configure the environment phase 1:
# 1.  Set execution policy as unrestricted
# 2.  Set network configurations, for example, ip addresses, subnet mask, gateway and dns
# 3.  Set autologon
# 4.  [MS-ADTS-Security] Turn off UAC
# 5.  Promote Domain Controller as a Primary Domain Controller
# 6.  Set alternate DNS
# 7.  Register Windbg dbgsrv for debugging purpose
#------------------------------------------------------------------------------------------
Function Config-Phase1()
{
    Write-ConfigLog "Entering Phase 1..."

    # Set execution policy as unrestricted
    Write-ConfigLog "Setting execution policy..." -ForegroundColor Yellow
    .\Set-ExecutionPolicy-Unrestricted.ps1
    if(-not $IsAzure)
    {
        # Set network configurations
        Write-ConfigLog "Setting network configurations..." -ForegroundColor Yellow
        .\Set-NetworkConfiguration.ps1 -IPAddress $Parameters["ip"] -SubnetMask $Parameters["subnet"] -Gateway $Parameters["gateway"] -DNS ($Parameters["dns"].Split(';'))
    }

    # Set autologon
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    .\Set-AutoLogon.ps1 -Domain $Parameters["domain"] -Username $Parameters["username"] -Password $Parameters["password"]

    # Turn off UAC
    Write-ConfigLog "Turn off UAC..." -ForegroundColor Yellow
    .\Turnoff-UAC.ps1

    # Promote Domain Controller
    Write-ConfigLog "Promoting this computer to a primary domain controller..." -ForegroundColor Yellow
    .\PromoteDomainController.ps1 -DomainName $Parameters["domain"] -AdminPwd $Parameters["password"] -AdminUser $Parameters["username"]

    # Set Alternate DNS
    Write-ConfigLog "Setting Alternate DNS..." -ForegroundColor Yellow
    .\Set-AlternateDns.ps1 -alternateDns $Parameters["alternateDNS"]

    # Register Windbg dbgsrv
    if($EnableDebugging)
    {
        Write-ConfigLog "Registering Windbg dbgsrv..." -ForegroundColor Yellow
        .\Register-DbgSrv.ps1
    }
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase2
# Configure the environment phase 2:
# 1.  Turn off firewall
# 2.  Set domain admin account password cannot be changed and never expires
# 3.  Set password change, netlogon, create object rights in registry key and group policy: 
#      (a) [MS-DRSR][MS-FRS2] disable auto password change (Registry Key)
#      (b) [MS-NRPC] refuse password change request (Registry Key)
#      (c) [MS-NRPC] configure netlogon service to depend on the DNS service (Registry Key)
#      (d) [MS-DRSR] disable auto password change (Group Policy)
#      (e) [MS-ADTS-LDAP] set only domain admins group can create computer object (Group Policy)
# 4.  Get and log operating system version and forest functional level
# 5.  [MS-ADTS-Security] Install Active Directory Certificate Services with EnterpriseRootCA as CAType
# 6.  [MS-ADTS-*] Install Active Directory Lightweight Directory Services
# 7.  [MS-FRS2] Install DFS Management tools
# 8.  Install IIS
# 9.  Set computer password for this primary domain controller
# 10. [MS-ADTS-Security] Create client user: userADTSSecurity for AD LDS instance
# 11. [MS-ADTS-Security] Install new AD LDS instance: instance01 under client user
# 12. [MS-ADTS-Security][MS-ADTS-Schema] Enable Optional Feature: recycle bin
# 13. [MS-ADTS-LDAP] Set msDS-Other-Settings on Domain Service object cn=directory service,cn=windows NT,cn=services,cn=configuration,<DomainNc>: ADAMAllowADAMSecurityPrincipalsInConfigPartition=1
# 14. [MS-ADTS-LDAP] Set msDS-Behavior-Version on Domain Service object cn=partitions,cn=configuration,<DomainNc>
# 15. [MS-ADTS-Security][MS-ADTS-Schema] Set msDS-AdditonalDnsHostName on Domain Service object cn=<Hostname>,ou=domain controllers,<DomainNc>
# 16. [MS-ADTS-PublishDCScenario] Get and log objectGuid from Domain Service domain object <DomainNc>
# 17. [MS-ADTS-PublishDCScenario] Get and log objectSid from Domain Service domain object <DomainNc>
# 18. Get and log netbiosName from Domain Service domain object <DomainNc>
# 19. Get and log AD LDS instance id
# 20. [MS-ADTS-Security][MS-ADTS-Schema] Set msDS-Other-Settings on Lightweight Directory Service object cn=directory service,cn=windows NT,cn=services,cn=configuration,<LdsInstanceId>: ADAMAllowADAMSecurityPrincipalsInConfigPartition=1
#------------------------------------------------------------------------------------------
Function Config-Phase2()
{
    Write-ConfigLog "Entering Phase 2..."

    # Wait for computer to be stable
    Sleep 30

    # Turn off firewall
    Write-ConfigLog "Turn off firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Set domain admin account password cannot be changed and never expires
    Write-ConfigLog "Setting domain admin account password cannot be changed and never expires..." -ForegroundColor Yellow
    .\Set-AdUserPwdPolicy.ps1 -Domain $Parameters["domain"] -Name $Parameters["username"] -Password $Parameters["password"]

    # Set password change, netlogon, rights in registry key and group policy
    Write-ConfigLog "Setting password change, netlogon, create object rights in registry key and group policy..." -ForegroundColor Yellow
    Copy-Item .\Scripts\GptTmpl.txt .\ -Force
    .\Set-NetlogonRegKeyAndPolicy.ps1 -IsDc -EnableGpConfig

    # Get OS Version and Forest Functional Level
    Write-ConfigLog "Getting Operating System Version..." -ForegroundColor Yellow
    $OsVersion = .\Get-OsVersion.ps1 -log
    Write-ConfigLog "Getting Forest Functional Level..." -ForegroundColor Yellow
    $ForestFuncLvl = .\Get-ForestFuncLvl.ps1 -log

    # Install Active Directory Certificate Services
    Write-ConfigLog "Installing Active Directory Certificate Services..." -ForegroundColor Yellow
    .\Install-AdCertificateService-Feature.ps1

    # Install AD LDS
    Write-ConfigLog "Installing Active Directory Lightweight Directory Services..." -ForegroundColor Yellow
    .\Install-AdLds-Feature.ps1

    # Install DFS Management tools 
    Write-ConfigLog "Installing DFS Management tools..." -ForegroundColor Yellow
    .\Install-DfsManagement-Feature.ps1 -DfsMgmtTools

    # Install IIS
    Write-ConfigLog "Installing IIS WEB-Server..." -ForegroundColor Yellow
    .\Install-Iis-Feature.ps1

    # Wait for computer to be stable
    Sleep 10

    # [FIXME]: this action needs host reboot to take effect, however, there is no reboot in the following
    # [FIXME]: rebooting the host, will cause the AD LDS instance not running
    # Set computer password
    Write-ConfigLog "Setting computer password..." -ForegroundColor Yellow
    .\Set-AdComputerPassword.ps1 -Domain $Parameters["domain"] -Name $Parameters["name"] -Password $Parameters["password"] -IsDc
	
    # Create client user for AD LDS instance
    Write-ConfigLog "Creating client user account for AD LDS instance ..." -ForegroundColor Yellow
    .\Create-AdUserWithGroupMembership.ps1 -Domain $Parameters["domain"] -Name $Parameters["clientuser"] -Password $Parameters["userpassword"] -Groups @("Administrators", "Domain Admins", "Domain Users", "Enterprise Admins", "Schema Admins", "Group Policy Creator Owners")
             
    # Install new AD LDS instance under client user
    Write-ConfigLog "Installing AD LDS instance under client user..." -ForegroundColor Yellow
    $LdsPortNum = .\Get-AvailablePort.ps1 $Parameters["ldsldapport"]
    $SslPortNum = .\Get-AvailablePort.ps1 $($LdsPortNum+1)
    .\Install-AdLds-Instance.ps1 -InstanceName $Parameters["ldsinstancename"] -PortNum $LdsPortNum -SslPortNum $SslPortNum -NewApplicationPartition $Parameters["ldsnewapplicationpartitiontocreate"] -Domain $Parameters["domain"] -Username $Parameters["clientuser"] -Password $Parameters["userpassword"] -ForestFuncLvl $ForestFuncLvl -Log

    # Enable Optional Feature
    Write-ConfigLog "Enabling Optional Feature..." -ForegroundColor Yellow
    .\Enable-AdRecyleBin.ps1 -Domain $Parameters["domain"]

    # Wait for computer to be stable
    Sleep 10
        
    # Set msDS-Other-Settings on Domain Service: ADAMAllowADAMSecurityPrincipalsInConfigPartition=1
    Write-ConfigLog "Setting msDS-Other-Settings on Domain Service object cn=directory service,cn=windows NT,cn=services,cn=configuration,<DomainNc>`: ADAMAllowADAMSecurityPrincipalsInConfigPartition=1..." -ForegroundColor Yellow
    .\Set-MsDsOtherSettings.ps1 -ServiceType Ds -Domain $Parameters["domain"]

    # Set msDS-Behavior-Version on Domain Service
    Write-ConfigLog "Setting msDS-Behavior-Version on Domain Service object cn=partitions,cn=configuration,<DomainNc>..." -ForegroundColor Yellow
    .\Set-MsDsBehaviorVersion.ps1 -Domain $Parameters["domain"] -ForestFuncLvl $ForestFuncLvl
          
    # Set msDS-AdditionalDnsHostName on Domain Service
    Write-ConfigLog "Setting msDS-AdditonalDnsHostName on Domain Service object cn=<Hostname>,ou=domain controllers,<DomainNc>..." -ForegroundColor Yellow
    .\Set-MsDSAdditonalDnsHostName.ps1 -Domain $Parameters["domain"] -Hostname $Parameters["name"]

    # Get and log objectGuid from Domain Service domain object <DomainNc>  
    Write-ConfigLog "Getting Domain objectGuid..." -ForegroundColor Yellow
    .\Get-DomainGuid.ps1 -Domain $Parameters["domain"] -log
    
    # Get and log objectSid from Domain Service domain object <DomainNc>  
    Write-ConfigLog "Getting Domain objectSid..." -ForegroundColor Yellow
    .\Get-DomainSid.ps1 -Domain $Parameters["domain"] -log
    
    # Get and log netbiosName from Domain Service domain object <DomainNc>  
    Write-ConfigLog "Getting Domain Netbios name..." -ForegroundColor Yellow
    .\Get-DomainNetbiosName.ps1 -Domain $Parameters["domain"] -log

    # Get AD LDS instance ID
    Write-ConfigLog "Getting AD LDS instance id..." -ForegroundColor Yellow
    $LdsInstanceId = .\Get-AdLdsInstanceId.ps1 -InstanceName $Parameters["ldsinstancename"] -log

    # Set msDS-Other-Settings on Lightweight Directory Service: ADAMAllowADAMSecurityPrincipalsInConfigPartition=1
    Write-ConfigLog "Setting msDS-Other-Settings on Lightweight Directory Service object cn=directory service,cn=windows NT,cn=services,cn=configuration,<LdsInstanceId>: ADAMAllowADAMSecurityPrincipalsInConfigPartition=1..." -ForegroundColor Yellow
    .\Set-MsDsOtherSettings.ps1 -ServiceType Lds -Domain $Parameters["domain"] -LdsPort $LdsPortNum -LdsInstanceId $LdsInstanceId -LdsUserName $Parameters["clientuser"] -LdsUserPwd $Parameters["userpassword"]
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase3
# Configure the environment phase 3:
# Triggered by remote trusted domain: <TDC>
# 1.  Create forest bidirectional trust on local side
#------------------------------------------------------------------------------------------
Function Config-Phase3()
{
    Write-ConfigLog "Enter Phase 3, triggered by trusted domain..."

    Write-ConfigLog "Creating forest trust relationship on local side..." -ForegroundColor Yellow
    .\CreateLocalSideForestTrust.ps1 -TargetForestName $Parameters["trusttargetdomain"] -TrustPassword $Parameters["trustpassword"]
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase4
# Configure the environment phase 4:
# Triggered by endpoint computer: <ENDPOINT>
# 1.  [MS-APDS] Create a managed service account
# 2.  [MS-ADTS-LDAP] Set msDS-AdditionalDnsHostName on Domain Service object cn=<Hostname>,cn=computers,<DomainNc> (endpoint needs to join domain first)
# 3.  [MS-ADTS-Security] Create a self-signed certificate for SSL and install it
# 4.  [MS-ADTS-Security] Set SSL binding for IIS website to use the self-signed certificate
#------------------------------------------------------------------------------------------
Function Config-Phase4()
{
    Write-ConfigLog "Enter Phase 4, triggered by endpoint..."

    # Create Managed Service Account
    Write-ConfigLog "Creating Managed Service Account..."
    .\Create-ManagedServiceAccount.ps1 -AccountName $Parameters["msaname"] -AccountPassword $Parameters["msapassword"] -Computer $Parameters["clientname"]

    # Set msDS-AdditionalDnsHostName (endpoint needs to join domain first)
    Write-ConfigLog "Setting msDS-AdditionalDnsHostName on Domain Service object cn=<Hostname>,cn=computers,<DomainNc>..." -ForegroundColor Yellow
    .\Set-MsDSAdditonalDnsHostName.ps1 -Domain $Parameters["domain"] -Hostname $Parameters["clientname"]

    # Create a self-signed certificate for SSL and install it
    Write-ConfigLog "Creating a self-signed certificate for SSL..."
    $CommonName = $Parameters["name"] + "." + $Parameters["domain"]
    .\Create-SelfSignedCert.ps1 -CommonName $CommonName
    Write-ConfigLog "Installing the self-signed certificate for SSL..."
    .\Install-SelfSignedCert.ps1 -CertFile "$env:SystemDrive\$CommonName.cer"

	# Set SSL binding for IIS website to use the self-signed certificate
	.\Set-IisSslBinding.ps1 -CommonName $CommonName
}

#------------------------------------------------------------------------------------------
# Function: Config-Environment
# Control the overall workflow of all configuration phases
#------------------------------------------------------------------------------------------
Function Config-Environment
{
    # Start configure
    switch($Step)
    {
        1 { Config-Phase1; Proceed-ScriptWithRestart; }
        2 { Config-Phase2; Complete-Configure; Restart-Computer; }
        3 { Config-Phase3; }
        4 { Config-Phase4; }
        default
        {
            Write-ConfigLog "Fail to execute the script!" -ForegroundColor Red
            break
        }
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
    Config-Environment
}

Main