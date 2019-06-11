###########################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
###########################################################################################

###########################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-CDC.ps1
## Purpose:        Configure Domain Controller in Child Domain for Active Directory Family Test Suite.
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
Post configuration script to configure Domain Controller in Child Domain for Active Directory Family Test Suite.

Usage:
    .\Config-CDC.ps1 [-Step <step>] [-h | -help]

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
    if(Test-Path -Path $SignalFileFullPath){
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
# Function: Remove-ScriptFromTask
# Remove the sceduled task to execute the script next step
#------------------------------------------------------------------------------------------
Function Remove-ScriptFromTask
{
    # remove the schedule task to execute the script next step after restart
    .\RestartAndRunFinish.ps1
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
# 3.  Turn off firewall before DC promotion
# 3.  Set autologon
# 4.  [MS-ADTS-Security] Turn off UAC
# 5.  Promote Domain Controller as a Child Domain Domain Controller
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
    # Turn off UAC
    Write-ConfigLog "Turn off UAC..." -ForegroundColor Yellow
    .\Turnoff-UAC.ps1
	
	if(Test-Path "retrycount.txt")
    {
        Remove-Item "retrycount.txt"
    }
}

Function Config-Phase2
{
	Write-ConfigLog "Entering Phase 2..."
    $retryCount = 1
    if(Test-Path "retrycount.txt")
    {
        [int]$retryCount = Get-Content "retrycount.txt"
    }
    
    while($retryCount -lt 5)
    {
        Try
        {
	        # Wait for computer to be stable
            Start-Sleep -s 300
    
            # Promote Domain Controller
            Write-ConfigLog "Promoting this computer to a child domain domain controller..." -ForegroundColor Yellow
            .\WaitFor-ComputerReady.ps1 -computerName $Parameters["primarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
            .\WaitFor-ComputerReady.ps1 -computerName $Parameters["secondarydc"] -usr $Parameters["username"] -pwd $Parameters["password"]
            $DomainNetbios = $Parameters["domain"].ToString().Split('.')[0].ToUpper()

            $promoteResult = .\PromoteChildDomain.ps1 -NewDomainName $DomainNetbios -ParentDomainName $Parameters["parentdomain"] -AdminUser $Parameters["username"] -AdminPwd $Parameters["password"] -ErrorAction Stop 
			
			if($promoteResult.Success -eq "True")
			{
				break
			}else
			{
				$retryCount++
				$retryCount | Out-File "retrycount.txt"
				Restart-Computer -Force
			}
        }Catch
        {
			$retryCount++
            $retryCount | Out-File "retrycount.txt"
            Restart-Computer -Force
        }
    }
	
	if($retryCount -ge 5)
	{
		Write-ConfigLog "Promote Child Domain failed after retry 5 times."
		throw "Promote Child Domain failed after retry 5 times."
	}
	
	# Set autologon
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    .\Set-AutoLogon.ps1 -Domain $Parameters["domain"] -Username $Parameters["username"] -Password $Parameters["password"]
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase3
# Configure the environment phase 3:
# 1.  Turn off firewall
# 2.  Set domain admin account password cannot be changed and never expires
# 3.  Set password change, netlogon, create object rights in registry key and group policy: 
#      (a) [MS-DRSR][MS-FRS2] disable auto password change (Registry Key)
#      (b) [MS-NRPC] refuse password change request (Registry Key)
#      (c) [MS-NRPC] configure netlogon service to depend on the DNS service (Registry Key)
#      (d) [MS-DRSR] disable auto password change (Group Policy)
#      (e) [MS-ADTS-LDAP] set only domain admins group can create computer object (Group Policy)
# 4.  [MS-FRS2] Install DFS replication role service
# 5.  Get and log operating system version
# 6.  Get and log netbiosName from Domain Service domain object <DomainNc>
# 7.  [MS-ADTS-LDAP] Set msDS-AllowToActOnBehalfOfOtherIdentity (A2DF) attribute on Domain Service object cn=<CDC>,ou=domain controllers,<ChildDomainNc>: msDS-AllowToActOnBehalfOfOtherIdentity: <PDC>
# 8.  Register Windbg dbgsrv for debugging purpose
#------------------------------------------------------------------------------------------
Function Config-Phase3()
{
    Write-ConfigLog "Entering Phase 3..."

    # Turn off firewall
    Write-ConfigLog "Turn off firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Set domain admin account password cannot be changed and never expires
    Write-ConfigLog "Setting domain admin account password cannot be changed and never expires..." -ForegroundColor Yellow
    .\Set-AdUserPwdPolicy.ps1 -Domain $Parameters["domain"] -Name $Parameters["username"] -Password $Parameters["password"]

    # Set password change, netlogon, rights in registry key and group policy
    Write-ConfigLog "Setting password change, netlogon, create object rights in registry key and group policy..." -ForegroundColor Yellow
    .\Set-NetlogonRegKeyAndPolicy.ps1 -IsDc

    # Install DFS replication role service
    Write-ConfigLog "Installing DFS replication role service..." -ForegroundColor Yellow
    .\Install-DfsManagement-Feature.ps1 -DfsReplRole

    # Get OS Version
    Write-ConfigLog "Getting Operating System Version..." -ForegroundColor Yellow
    $OsVersion = .\Get-OsVersion.ps1 -log

    # Get and log netbiosName from Domain Service domain object <DomainNc>
    Write-ConfigLog "Getting Domain Netbios name..." -ForegroundColor Yellow
    .\Get-DomainNetbiosName.ps1 -Domain $Parameters["domain"] -log

    # Set msDS-AllowToActOnBehalfOfOtherIdentity (A2DF) attribute on Domain Service
    Write-ConfigLog "Setting msDS-AllowToActOnBehalfOfOtherIdentity (A2DF) attribute on Domain Service object cn=<CDC>,ou=domain controllers,<ChildDomainNc>: msDS-AllowToActOnBehalfOfOtherIdentity: <PDC>..." -ForegroundColor Yellow
    .\Set-MsDsAllowToActOnBehalfOfOtherIdentity.ps1 -DelegateHostFqdn $Parameters["primarydc"] -LocalHostName $Parameters["name"]

    # Register Windbg dbgsrv
    if($EnableDebugging)
    {
        Write-ConfigLog "Registering Windbg dbgsrv..." -ForegroundColor Yellow
        .\Register-DbgSrv.ps1
    }
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
        2 { Config-Phase2; Proceed-ScriptWithRestart; }
		3 { Config-Phase3; Proceed-ScriptWithRestart; }
        4 { Complete-Configure; }
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