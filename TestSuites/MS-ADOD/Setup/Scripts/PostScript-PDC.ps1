#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Sripting
## File:           PostScript-PDC.ps1
## Purpose:        Configure PDC for MS-ADOD test suite
## Requirements:   Windows PowerShell 2.0
## Supported OS:   Windows Server 2012 or later versions
##
##############################################################################

Param
(
    [string]$WorkingPath = (split-path -parent ([System.IO.Path]::GetDirectoryName($myInvocation.MyCommand.Definition))),
    [int]   $Step        = 1                                             # For rebooting, indicate which step the script is running
)

#-----------------------------------------------------------------------------
# Global variables
#-----------------------------------------------------------------------------
$ScriptsSignalFile      = "$env:SystemDrive\PostScript.Completed.signal"      # Config signal file
$CurrentScriptPath      = $MyInvocation.MyCommand.Definition                  # Current Working Path
$LogPath                = "$env:HOMEDRIVE\Logs"                               # Log Path
$LogFile                = "$LogPath\PostScript-PDC.ps1.log"                   # Log File

# Check WorkingPath
if($WorkingPath -eq "$env:SystemDrive\")
{
    Write-Host "Make path to C:\Temp to run script" -ForegroundColor Yellow
    $WorkingPath = $WorkingPath + "Temp"
    Write-Host "WorkingPath is : $WorkingPath" -ForegroundColor Yellow
    $ScriptsSignalFile = "$WorkingPath\post.finished.signal"
}
[string]$configPath	 	= "$WorkingPath\protocol.xml"

Write-Host "Put current dir as $WorkingPath" -ForegroundColor Yellow
Push-Location $WorkingPath

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
}

#-----------------------------------------------------------------------------
# Function: SetLog
# Usage   : Create Log File
# Params  :
# Remark  :
#-----------------------------------------------------------------------------
Function SetLog(){

    if(!(Test-Path -Path $LogPath)){
        New-Item -ItemType Directory -Path $LogPath -Force
    }

    if(!(Test-Path -Path $LogFile)){
        New-Item -ItemType File -path $LogFile -Force
    }
    Start-Transcript $LogFile -Append 2>&1 | Out-Null
}

#-----------------------------------------------------------------------------
# Restart and Resume
#-----------------------------------------------------------------------------
Function RestartAndResume
{
    $NextStep = $Step + 1

    .\RestartAndRun.ps1 -ScriptPath $CurrentScriptPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
}

#-----------------------------------------------------------------------------
# Remove Controller.ps1 run step registry key
#-----------------------------------------------------------------------------
Function RemoveControllerRegistryKey
{
    $ControllerPhase = (Get-ItemProperty -Path HKLM:\Software\Microsoft\Windows\CurrentVersion\Run -Name Install).Install
    Write-Host "Remove Controller.ps1 registry key: $ControllerPhase"
    Remove-ItemProperty -Path HKLM:\Software\Microsoft\Windows\CurrentVersion\Run -Name Install
}

#-----------------------------------------------------------------------------
# Add back Controller.ps1 run step registry key
#-----------------------------------------------------------------------------
Function AddBackControllerRegistryKey
{
    Write-Host "Post script run finished, add back Controller.ps1 registry key"
    $ControllerRegistryPhase = "C:\Windows\System32\WindowsPowerShell\v1.0\PowerShell.exe -NoExit -Command `"c:\temp\controller.ps1 -phase 4`""
    Set-ItemProperty -Path HKLM:\Software\Microsoft\Windows\CurrentVersion\Run -Name Install -Value "$ControllerRegistryPhase"
}

#-----------------------------------------------------------------------------
# Phase1: SetNetworkConfiguration; Disable-ICMPRedirect; Disable-IPv6; PromoteDomainController
#-----------------------------------------------------------------------------
Function Phase1
{
    [xml]$content = Get-Content $configPath
    $clientSetting = $Content.lab.servers.vm | where {$_.role -eq "Client"}
    $PDCSetting = $Content.lab.servers.vm | where {$_.role -eq "DriverComputer"}

    if($Content.lab.core.environment -ne "Azure"){ #azure regression do not neet set network configuration.
        # Set Network
        Write-Host "Setting network configuration" -ForegroundColor Yellow
        .\Set-NetworkConfiguration.ps1 -IPAddress $PDCSetting.ip -SubnetMask $PDCSetting.subnet -Gateway $PDCSetting.gateway -DNS (($PDCSetting.dns).split(';'))

        # Disable ICMP Redirect
        Write-Host "Disabling ICMP Redirect" -ForegroundColor Yellow
        .\Disable-ICMPRedirect.ps1
    }


    # Disable IPv6
    Write-Host "Disabling IPv6" -ForegroundColor Yellow
    .\Disable-IPv6.ps1

     # Get Installed Script Path
    Write-Host "Getting the Installed Script Path" -ForegroundColor Yellow
    $Path = .\Get-InstalledScriptPath.ps1 -Name "MS-ADOD"

    # Set Startup Script
    Write-Host "Setting Startup Script" -ForegroundColor Yellow
    .\SetStartupScript.ps1 -Script "$Path\Enable-RemoteSessionInServer.ps1"

    # Get Installed Script Path in Client Computer
    $clientIP = $clientSetting.ip
    $clientAdmin = $content.lab.core.username
    $clientAdminPwd = $content.lab.core.password

    Write-Host "Getting Installed Script Path on Client Computer" -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $clientIP -usr $clientAdmin -pwd $clientAdminPwd
    $clientSignalPath = "\\$clientIP\C$\MSIInstalled.signal"
    $clientScriptsPath = Get-Content -Path $clientSignalPath

    # Modify PTF configure file
    Write-Host "Modify PTF configure file" -ForegroundColor Yellow
    $binPath = "$Path\..\Bin"
    $binDepPtfConfig = "$binPath\MS-ADOD_ODTestSuite.deployment.ptfconfig"
    .\Modify-ConfigFileNode.ps1 -sourceFileName $binDepPtfConfig -nodeName "ClientScriptPath" -newContent $clientScriptsPath

    # Modify Test Setting file
    Write-Host "Modify ODLocalTestRun.testrunconfig file" -ForegroundColor Yellow
    .\Modify-TestRunConfig.ps1 -TestRunConfigPath $binPath

    # Promote DC
    Write-Host "Promoting this computer to DC" -ForegroundColor Yellow
    .\PromoteDomainController.ps1 -DomainName $PDCSetting.domain -AdminPwd $content.lab.core.password

    if($Content.lab.core.environment -ne "Azure"){
        Write-Host "Enable WinRM" -ForegroundColor Yellow
        if (Test-WSMan -ComputerName $PDCSetting.ip)
        {
            Write-Host "WinRM is running"
        }
        else
        {
            .\Enable-WinRM.ps1
        }
    }
}


#-----------------------------------------------------------------------------
# Finish Script
#-----------------------------------------------------------------------------
Function Finish
{
    # Write signal file
    Write-Host "Write signal file: $ScriptsSignalFile."
    cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

    # Ending script
    Write-Host "Config finished."
    Write-Host "EXECUTE [PostScript-PDC.ps1] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    # Remove the registry key for postscript auto run and finish the postscript execution.
    .\RestartAndRunFinish.ps1
}

#-------------------------------------------------------------------------------
# Function: Main
# Remark  : Main script starts here.
#-------------------------------------------------------------------------------
Function Main()
{
    Prepare
    SetLog

    switch ($Step)
    {
        1 { RemoveControllerRegistryKey; Phase1; RestartAndResume; }
        2 { AddBackControllerRegistryKey; Finish; }
        default
        {
            Write-Host "Fail to execute the script" -ForegroundColor Red
            break
        }
    }
    shutdown /r /f
}

Main

Pop-Location