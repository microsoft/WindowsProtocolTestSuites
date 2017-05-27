#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-SDC.ps1
## Purpose:        Configure Secondary DC for Active Directory test suites.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2
##
##############################################################################
Param
(
    [alias("h")][switch]$help,
    [string]$VMName = "AD_SDC", # The Virtual Machine's name
    [int]$Step      = 1         # Current step for configuration
)

if($help)
{
$helpmsg = @"
Post script to config Secondary DC.

Usage:
    .\Config-SDC.ps1 [-VMName <vmname>] [-Step <step>] [-h | -help]

VMName: The name of the VM to be created. The default value is AD_SDC.
Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output -NoNewLine $helpmsg "`r`n"  
    return
}

Function Write-Log
{
    Param ([Parameter(ValueFromPipeline=$true)] $text,
    $ForegroundColor = "Green"
    )

    $date = Get-Date
    Write-Output "`r`n$date $text"
}

Function CheckReturnValue()
{
    if( -not $?)
    {
	    $vars = Get-Variable
        $date = Get-Date
        $line = $MyInvocation.ScriptLineNumber.ToString()
        Write-Output "`r`n$date Error in line $line."
        Write-Output "**********************"
        Write-Output "Dump local variables"
        Write-Output "**********************"
        Format-Table Name,Value -wrap -autosize -inputobject $vars
        Stop-Transcript
        throw "Error in line $line."
    }
}

#-----------------------------------------------------------------------------
# Global function: Split file name and directory path from a full path
#-----------------------------------------------------------------------------
Function Get-SplitFileName([string]$FullPathName)
{
    $Pieces = $FullPathName.split("\") 
    $NumberOfPieces = $Pieces.Count 
    $FileName = $Pieces[$NumberOfPieces - 1] 
    $DirectoryPath = $FullPathName.Substring(0, $FullPathName.Length - $FileName.Length - 1)

    return $FileName, $DirectoryPath
}

#-----------------------------------------------------------------------------
# Global variables
#-----------------------------------------------------------------------------
$ScriptFullPath          = $MyInvocation.MyCommand.Definition                # Current Working Script Full Path
$ScriptName, $ScriptPath = Get-SplitFileName -FullPathName $ScriptFullPath   # Current Working Script Name
                                                                             # Current Working Script Path
$ScriptSignalFullPath    = "$ScriptFullPath.finished.signal"                 # Current Working Script Completion Signal File
$LogPath                 = "$ScriptPath"                                     # Current Working Script Log Path
$LogFile                 = "$LogPath\$ScriptName.log"                        # Current Working Script Log File
$ParamArray              = @{}                                               # Parameters from the config file


#-----------------------------------------------------------------------------
# Read Config Parameters
#-----------------------------------------------------------------------------
Function ReadConfig()
{
    Write-Log "Getting the parameters from config file ..." -ForegroundColor Yellow
    .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$ParamArray)
    $ParamArray
}

#-----------------------------------------------------------------------------
# Check signal file and switch to script path
#-----------------------------------------------------------------------------
Function Prepare(){

    Write-Log "Executing [$ScriptName] ..." -ForegroundColor Cyan

    # Check completion signal file. If signal file exists, exit with 0
    if(Test-Path -Path $ScriptSignalFullPath){
        Write-Log "The script execution is complete." -ForegroundColor Red
        exit 0
    }
        
    Write-Log "Switching to $ScriptPath" -ForegroundColor Yellow
    Push-Location $ScriptPath
}

#-----------------------------------------------------------------------------
# Create Log 
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

    .\RestartAndRun.ps1 -ScriptPath $ScriptFullPath `
                        -PhaseIndicator "-Step $NextStep" `
                        -AutoRestart $true
}

#-----------------------------------------------------------------------------
# Phase1: SetNetworkConfiguration; SetAutoLogon; PromoteSDC
#-----------------------------------------------------------------------------
Function Phase1
{
    Write-Log "Entering Phase1..."

    # Set Network
    Write-Log "Setting network configuration" -ForegroundColor Yellow
    .\SetNetworkConfiguration.ps1 -IPAddress $ParamArray["ip"] -SubnetMask $ParamArray["subnet"] -Gateway $ParamArray["gateway"] -DNS ($ParamArray["dns"].split(';'))
    
    # Set Auto Logon
    Write-Log "Setting auto logon" -ForegroundColor Yellow
    .\SetAutoLogon.ps1 -domain $ParamArray["domain"] -user $ParamArray["username"] -pwd $ParamArray["password"]
       
    # Promote DC
    Write-Log "Promoting this computer to DC" -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $ParamArray["replicasourcedc"] -usr $ParamArray["username"] -pwd $ParamArray["password"]
    .\PromoteDomainController.ps1 -DomainName $ParamArray["domain"] -AdminPwd $ParamArray["password"] -IsPrimary $false
    
    # Temporarily stop KDC service [MS-DRSR]
    Set-Service Kdc -StartupType Manual
}

#-----------------------------------------------------------------------------
# Phase2: SetAdminAccount; DisablePasswordChange
#-----------------------------------------------------------------------------
Function Phase2
{
    Write-Log "Entering Phase2..."

    # Turn off firewall
    cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Output

    $domainNC = "DC=" + $ParamArray["domain"].ToString().Replace(".", ",DC=")
    $domainAdmin = $ParamArray["username"]
    $hostName = $ParamArray["name"]
    $pdcName = $ParamArray["replicasourcedc"].ToString().Split('.')[0]

    # Set Domain Admin Account    
    Write-Log "Setting Domain Admin Account" -ForegroundColor Yellow
    
    cmd /c dsmod user "CN=$domainAdmin,CN=users,$domainNC" `
                      -pwd $ParamArray["password"] -mustchpwd no -disabled no -canchpwd no `
                      -pwdneverexpires yes 2>&1 | Write-Output
    CheckReturnValue

    # Disable auto password change [MS-DRSR]
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon\Parameters /v DisablePasswordChange /t REG_DWORD /d 1 /f

    # Configure the Netlogon service to depend on the DNS service
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon /v DependOnService /t REG_MULTI_SZ /d LanmanWorkstation\0LanmanServer\0DNS /f

    # Set password for local computer
    ksetup /SetComputerPassword $ParamArray["password"]

    # Set computer account password [MS-DRSR]
    $dcADSI=[ADSI]"LDAP://$pdcName/CN=$hostName,OU=Domain Controllers,$domainNC"
    $dcADSI.SetPassword($ParamArray["password"])
    $dcADSI.SetInfo()
    CheckReturnValue

    # Replicate from PDC [MS-DRSR]
    repadmin /replicate $hostName $ParamArray["replicasourcedc"] "$domainNC"
    CheckReturnValue

    # Restart KDC [MS-DRSR]
    Set-service Kdc -StartupType "Automatic"
    Start-Service Kdc

    # Install DFS Management tools [MS-FRS2]
    Import-Module Servermanager
    Write-Log "Installing DFS Management tools"
    Add-WindowsFeature RSAT-DFS-Mgmt-Con -IncludeAllSubFeature -Confirm:$false

    # Log OS Version in TXT
    Write-Log "Getting OS Version" -ForegroundColor Yellow
    $osVersion   = .\Get-OSVersion.ps1
    if($osVersion -eq $null)
    {
        Write-Log "Unable to get OS Version and set as default value" -ForegroundColor Red
        $osVersion = "Win2012R2"
    }
    $osPath = "$env:SystemDrive\osversion.txt"
    $osVersion>>$osPath

    if(Test-Path -Path "c:\temp\scripts\TTT\tttracer.exe" -PathType Leaf)
    {
        .\scripts\registerwdbgsvr.ps1
    }

    sleep 30
}

#-----------------------------------------------------------------------------
# Finish Script
#-----------------------------------------------------------------------------
Function Finish
{
    # Write signal file
    Write-Log "Write signal file: $ScriptName.finished.signal to system drive."
    cmd /C ECHO CONFIG FINISHED > $ScriptSignalFullPath

    # Ending script
    Write-Log "Config finished."
    Write-Log "EXECUTE [$ScriptName] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    Stop-Transcript

    .\RestartAndRunFinish.ps1
}

#-----------------------------------------------------------------------------
# Main Function
#-----------------------------------------------------------------------------
Function Main(){

    Prepare
    ReadConfig
    SetLog

    switch ($Step)
    {
        1 { Phase1; RestartAndResume; }
        2 { Phase2; RestartAndResume; }
        3 { Finish;}
        default
        {
            Write-Log "Fail to execute the script" -ForegroundColor Red
            break
        }
    }
}

Main
