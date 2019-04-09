#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

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

    [int]
    $Step = 1
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$ScriptFileFullPath 	 = $MyInvocation.MyCommand.Definition
$ScriptName              = [System.IO.Path]::GetFileName($ScriptFileFullPath)
$ScriptPath         	 = Split-Path $ScriptFileFullPath
$LogFileFullPath         = "$ScriptFileFullPath.log"
$SignalFileFullPath      = "$ScriptPath\CreateRODC.finished.signal"
$Parameters              = @{}

#------------------------------------------------------------------------------------------
# Function: Display-Help
# Display the help messages.
#------------------------------------------------------------------------------------------
Function Display-Help()
{
    $helpmsg = @"
Post configuration script to configure Read-Only Domain Controller in Local Domain for Active Directory Family Test Suite.

Usage:
    .\Config-RODC.ps1 [-Step <step>] [-h | -help]

Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg "`r`n"
    exit 0
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
    .\Write-Info.ps1 -logContent $text -ForegroundColor $ForegroundColor
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
# Function: Read-ConfigParameters
# Read Config Parameters
#------------------------------------------------------------------------------------------
Function Read-ConfigParameters()
{
    Write-ConfigLog "Getting the parameters from config file..." -ForegroundColor Yellow
    $VMName = .\GetVMNameByComputerName.ps1
    .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$Parameters)
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
    Write-ConfigLog "Executing [$ScriptName], Step $Step..." -ForegroundColor Cyan

    # Check completion signal file. If signal file exists, exit with 0
    if(Test-Path -Path $SignalFileFullPath){
        Write-ConfigLog "Singnal file exist, The script execution has been completed." -ForegroundColor Red
        exit 0
    }

    # Switch to the script path
    Write-ConfigLog "Switching to $ScriptPath..." -ForegroundColor Yellow
    Push-Location $ScriptPath

    # Read the config parameters
    Read-ConfigParameters
}

#------------------------------------------------------------------------------------------
# Function: Config-Phase1
# Configure the environment phase 1:
# 1.  Set execution policy as unrestricted
# 2.  Set network configurations, for example, ip addresses, subnet mask, gateway and dns
# 3.  Turn off UAC
#------------------------------------------------------------------------------------------
Function Config-Phase1()
{
    Write-ConfigLog "Entering Phase 1..."

    # Set execution policy as unrestricted
    Write-ConfigLog "Setting execution policy..." -ForegroundColor Yellow
    .\Set-ExecutionPolicy-Unrestricted.ps1

    # Set network configurations
    Write-ConfigLog "Setting network configurations..." -ForegroundColor Yellow
    .\Set-NetworkConfiguration.ps1 -IPAddress $Parameters["ip"] -SubnetMask $Parameters["subnet"] -Gateway $Parameters["gateway"] -DNS ($Parameters["dns"].Split(';'))

    # Turn off UAC
    Write-ConfigLog "Turn off UAC..." -ForegroundColor Yellow
    .\Turnoff-UAC.ps1
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
			Write-ConfigLog "Sleep 5 minutes to wait for computer to be stable..."
            Start-Sleep -s 300
    
            # Promote Domain Controller
            Write-ConfigLog "Promoting this computer to a read-only domain controller..." -ForegroundColor Yellow
            .\WaitFor-ComputerReady.ps1 -computerName $Parameters["replicasourcedc"] -usr $Parameters["username"] -pwd $Parameters["password"]

            $promoteResult = .\PromoteRODC.ps1 -DomainName $Parameters["domain"] -AdminUser $Parameters["username"] -AdminPwd $Parameters["password"] -ReplicationSourceDC $Parameters["replicasourcedc"]
			if($promoteResult.Status -eq "Error")
			{
				$retryCount++
				$retryCount | Out-File "retrycount.txt"
				Restart-Computer -Force
			}else
			{
				break
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
		Write-ConfigLog "Promote RODC failed after retry 5 times."
		throw "Promote RODC failed after retry 5 times."
	}
	
	# Set autologon
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    .\Set-AutoLogon.ps1 -Domain $Parameters["domain"] -Username $Parameters["username"] -Password $Parameters["password"]
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
# Function: Config-Environment
# Control the overall workflow of all configuration phases
#------------------------------------------------------------------------------------------
Function Config-Environment
{
	Write-ConfigLog "Entering Config-Environment, Step: $Step."
    # Start configure
    switch($Step)
    {
        1 { Config-Phase1; Proceed-ScriptWithRestart; }
        2 { Config-Phase2; Proceed-ScriptWithRestart; }
        3 { Complete-Configure; }
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