#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
#############################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Kerberos-DC01-Postscript.ps1
## Requirements:   Windows PowerShell 2.0
## Supported OS:   Windows Server 2012 or later versions
##
##############################################################################
Param
(
    [int]$Step = 1
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$WorkingPath = "c:\temp"
$env:Path += ";c:\temp;c:\temp\Scripts"
[string]$VMName = "Kerberos-OSS-DC01"

# Parameters from the config file
$ParamArray = @{} 
$CurrentScriptPath = $MyInvocation.MyCommand.Definition

# Utility Function 
Function RestartAndResume
{
    $NextStep = $Step + 1
    RestartAndRun.ps1 -ScriptPath $CurrentScriptPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
}

# Phase Functions
Function Prepare
{
    # Get parameters
    Write-Info.ps1 "Trying to get parameters from config file" -ForegroundColor Yellow
    GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$ParamArray)
    $ParamArray
}

Function Phase1
{
    # Set Network
    Write-Info.ps1 "Setting network configuration" -ForegroundColor Yellow
    SetNetworkConfiguration.ps1 -IPAddress $ParamArray["ip"] -SubnetMask $ParamArray["subnet"] `
        -Gateway $ParamArray["gateway"] -DNS ($ParamArray["dns"].split(';'))

    # Promote DC
    Write-Info.ps1 "Promoting this computer to DC" -ForegroundColor Yellow
    PromoteDomainController.ps1 -DomainName $ParamArray["domain"] -AdminPwd $ParamArray["password"]
}

Function Phase2
{
    # Set domain administrator password
    Write-Info.ps1 "Setting domain administrator password" -ForegroundColor Yellow
    ChangeDomainUserPassword.ps1 -DomainName $ParamArray["domain"] `
         -Username "Administrator" -NewPassword $ParamArray["password"]

    # Use domain administrator to logon
    Write-Info.ps1 "Setting auto logon" -ForegroundColor Yellow
    SetAutoLogon.ps1 -Domain $ParamArray["domain"] -User $ParamArray["username"] -Pwd $ParamArray["password"]
}

Function Finish
{
    RestartAndRunFinish.ps1
}

# Main Script Starts
Function Main
{
    Prepare

    switch ($Step)
    {
        1 {Remove-ItemProperty -Path HKLM:\Software\Microsoft\Windows\CurrentVersion\Run -Name Install; Phase1; RestartAndResume; }
        2 { Phase2; Set-ItemProperty -Path HKLM:\Software\Microsoft\Windows\CurrentVersion\Run -Name Install -Value "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe -command `"c:\temp\controller.ps1 -phase 4`""; Finish; }
    }
    
    Sleep 5
    Restart-Computer
}

Main

