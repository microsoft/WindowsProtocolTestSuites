###########################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
###########################################################################################

#------------------------------------------------------------------------------------------
# Parameters:
# Help: whether to display the help information
# Step: Current step for configuration
#------------------------------------------------------------------------------------------
Param
(
	$WorkingPath      	 = "C:\Temp",
    [int]$Step 			 = 1
)
$ConfigFile = "c:\temp\Protocol.xml"
$Parameters              = @{}
$CurrentScriptPath 		 = $MyInvocation.MyCommand.Definition
$ScriptsSignalFile = "$WorkingPath\post.finished.signal" # Config signal file
$IsAzure                 = $false

try {
    [xml]$content = Get-Content $ConfigFile -ErrorAction Stop
    $currentCore = $content.lab.core
    if(![string]::IsNullOrEmpty($currentCore.regressiontype) -and ($currentCore.regressiontype -eq "Azure")){
        $IsAzure = $true;
        $ScriptsSignalFile = "C:\PostScript.Completed.signal"
    }
}
catch {
    
}

#-----------------------------------------------------------------------------
# Function: Prepare
# Usage   : Start executing the script; Push directory to working directory
# Params  : 
# Remark  : 
#-----------------------------------------------------------------------------
Function Prepare()
{
    Write-Host "Executing [PostScript-PDC.ps1] ..." -ForegroundColor Cyan
	
	# Check signal file
    if(Test-Path -Path $ScriptsSignalFile)
    {
        Write-Host "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    # Change to absolute path
    Write-Host "Current path is $CurrentScriptPath" -ForegroundColor Cyan
    $WorkingPath = (Get-Item $WorkingPath).FullName

    Write-Host "Put current dir as $WorkingPath" -ForegroundColor Yellow
    Push-Location $WorkingPath
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
    Write-ConfigLog "Getting the parameters from environment config file..." -ForegroundColor Yellow
    if($IsAzure)
    {
        .\GetVmParametersByComputerName.ps1 -RefParamArray ([ref]$Parameters)
    } else {
        $VMName = .\GetVMNameByComputerName.ps1
        .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$Parameters)
    }
    $Parameters
}

# Utility Function 
Function RestartAndResume
{
    $NextStep = $Step + 1
    .\RestartAndRun.ps1 -ScriptPath $CurrentScriptPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
}

Function Phase1
{
	Write-ConfigLog "Entering Phase 1..."
	
	# Set execution policy as unrestricted
    Write-ConfigLog "Setting execution policy..." -ForegroundColor Yellow
    .\Set-ExecutionPolicy-Unrestricted.ps1

	if(-not $IsAzure)
    {
        Remove-ItemProperty -Path HKLM:\Software\Microsoft\Windows\CurrentVersion\Run -Name Install;
    }

    # Promote Domain Controller
    Write-ConfigLog "Promoting this computer to a primary domain controller..." -ForegroundColor Yellow
    .\PromoteDomainController.ps1 -DomainName $Parameters["domain"] -AdminPwd $Parameters["password"]

}

Function Phase2
{
	Write-ConfigLog "Entering Phase 2..."

    # Wait for computer to be stable
    Sleep 30
	
    # Turn off firewall
    Write-ConfigLog "Disable firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Set domain administrator password
    Write-ConfigLog "Setting domain administrator password" -ForegroundColor Yellow
    .\Change-DomainUserPassword.ps1 -DomainName $Parameters["domain"] -Username $Parameters["username"] -NewPassword $Parameters["password"]

    # Set autologon
    Write-ConfigLog "Setting autologon..." -ForegroundColor Yellow
    .\Set-AutoLogon.ps1 -Domain $Parameters["domain"] -Username $Parameters["username"] -Password $Parameters["password"]
}

Function Finish
{
	# Write signal file
    Write-Host "Write signal file: config.finished.signal to system drive."
    cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

    # Ending script
    Write-Host "post Config finished."
    Write-Host "EXECUTE [Kerberos-DC01-postscript.ps1] FINISHED (NOT VERIFIED)." -ForegroundColor Green
	
    .\RestartAndRunFinish.ps1

    if(-not $IsAzure)
    {
        Set-ItemProperty -Path HKLM:\Software\Microsoft\Windows\CurrentVersion\Run -Name Install -Value "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe -command `"c:\temp\controller.ps1 -phase 4`"";
    }
    Restart-Computer -Force
}

# Main Script Starts
Function Main
{
	Prepare
	
	Read-ConfigParameters

    switch ($Step)
    {
        1 { Phase1; RestartAndResume; }
        2 { Phase2; Finish; }
    }
}

Main

