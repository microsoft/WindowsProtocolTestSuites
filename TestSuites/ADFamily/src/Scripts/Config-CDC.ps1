#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################
#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-CDC.ps1
## Purpose:        Configure Child DC for Active Directory test suites.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2
##
##############################################################################
Param
(
    [alias("h")][switch]$help,
    [string]$VMName = "AD_CDC", # The Virtual Machine's name
    [int]$Step      = 1         # Current step for configuration
)

if($help)
{
$helpmsg = @"
Post script to config Child DC.

Usage:
    .\Config-CDC.ps1 [-VMName <vmname>] [-Step <step>] [-h | -help]

VMName: The name of the VM to be created. The default value is AD_CDC.
Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg
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
# Read Config Parameters
#-----------------------------------------------------------------------------
Function ReadConfig()
{
    Write-Log "Getting the parameters from config file ..." -ForegroundColor Yellow
    .\GetVmParameters.ps1 -VMName $VMName -RefParamArray ([ref]$ParamArray)
    $ParamArray
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
# Phase1: SetNetworkConfiguration; SetAutoLogon; PromoteDomainController
#-----------------------------------------------------------------------------
Function Phase1
{
    Write-Log "Entering Phase1..."

    # Set Network
    Write-Log "Setting network configuration" -ForegroundColor Yellow
    .\SetNetworkConfiguration.ps1 -IPAddress $ParamArray["ip"] -SubnetMask $ParamArray["subnet"] -Gateway $ParamArray["gateway"] -DNS ($ParamArray["dns"].split(';'))
    
    # Turn off firewall before DC promotion
    cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Output

    # Set Auto Logon
    Write-Log "Setting auto logon" -ForegroundColor Yellow
    .\SetAutoLogon.ps1 -domain $ParamArray["domain"] -user $ParamArray["username"] -pwd $ParamArray["password"]   
        
    # Promote DC to a Child Domain DC
    Write-Log "Promoting this computer to Child Domain DC" -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $ParamArray["primarydc"] -usr $ParamArray["username"] $ParamArray["password"]
    .\WaitFor-ComputerReady.ps1 -computerName $ParamArray["secondarydc"] -usr $ParamArray["username"] $ParamArray["password"]
    sleep 10    
    $domainNameNetBios = $ParamArray["domain"].ToString().Split('.')[0].ToUpper()
    try
    {
        .\PromoteChildDomain.ps1 -NewDomainName $domainNameNetBios -ParentDomainName $ParamArray["parentdomain"] -AdminUser $ParamArray["username"] -AdminPwd $ParamArray["password"]
    }
    catch
    {
        if( -not $?)
        {
            Write-Log "Promotion failed, rerun"
            sleep 60
            .\PromoteChildDomain.ps1 -NewDomainName $domainNameNetBios -ParentDomainName $ParamArray["parentdomain"] -AdminUser $ParamArray["username"] -AdminPwd $ParamArray["password"]
        }
    }
    sleep 10
}

#-----------------------------------------------------------------------------
# Phase2: SetAdminAccount
#-----------------------------------------------------------------------------
Function Phase2
{
    Write-Log "Entering Phase2..."
    
    Write-Log "Set Domain Admin Account ..." -ForegroundColor Yellow
    $domainNC = "DC=" + $ParamArray["domain"].ToString().Replace(".", ",DC=")
    $domainAdmin = $ParamArray["username"]
    cmd /c dsmod user "CN=$domainAdmin,CN=users,$domainNC" `
                      -pwd $ParamArray["password"] -mustchpwd no -disabled no -canchpwd no `
                      -pwdneverexpires yes 2>&1 | Write-Output
    CheckReturnValue

    # Configure the Netlogon service to depend on the DNS service
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon /v DependOnService /t REG_MULTI_SZ /d LanmanWorkstation\0LanmanServer\0DNS /f

    # Install DFS Management tools [MS-FRS2]
    Write-Log "Installing DFS Management tools"
    Import-Module Servermanager
    Add-WindowsFeature FS-DFS-Replication -IncludeAllSubFeature -Confirm:$false

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

    # Set msDS-AllowToActOnBehalfOfOtherIdentity (A2DF) attribute on CDC computer object to PDC [MS-ADTS-LDAP]
    Write-Log "Set msDS-AllowToActOnBehalfOfOtherIdentity attribute on CDC computer object to PDC" -ForegroundColor Yellow
    $pdc = Get-ADComputer -Identity $ParamArray["primarydc"].Split('.')[0] -Server $ParamArray["primarydc"]
    Set-ADComputer -Identity $ParamArray["name"] -PrincipalsAllowedToDelegateToAccount $pdc    
}

#-----------------------------------------------------------------------------
# Finish Script
#-----------------------------------------------------------------------------
Function Finish
{
    # Write signal file
    Write-Log "Write signal file: config.finished.signal to system drive."
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
        2 { Phase2; Finish; }
        default
        {
            Write-Log "Fail to execute the script" -ForegroundColor Red
            break
        }
    }
}

Main
