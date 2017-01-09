#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-DM.ps1
## Purpose:        Configure Domain Member for Active Directory test suites.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2
##
##############################################################################
Param
(
    [alias("h")][switch]$help,
    [string]$VMName = "AD_DM",  # The Virtual Machine's name
    [int]$Step      = 1         # Current step for configuration
)

if($help)
{
$helpmsg = @"
Post script to config Domain Member.

Usage:
    .\Config-DM.ps1 [-VMName <vmname>] [-Step <step>] [-h | -help]

VMName: The name of the VM to be created. The default value is AD_DM.
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
# Phase1: SetNetworkConfiguration; PromoteRODC
#-----------------------------------------------------------------------------
Function Phase1
{
    Write-Log "Entering Phase1..."
    
    # Change execution policy
    Set-ExecutionPolicy Unrestricted

    # Set Network
    Write-Log "Setting network configuration" -ForegroundColor Yellow
    .\SetNetworkConfiguration.ps1 -IPAddress $ParamArray["ip"] -SubnetMask $ParamArray["subnet"] -Gateway $ParamArray["gateway"] -DNS ($ParamArray["dns"].split(';'))
    Sleep 60

    $domain   = $ParamArray["domain"]
    $username = $ParamArray["username"]
    $password = $ParamArray["password"]

    # Set Auto Logon
    Write-Log "Setting auto logon" -ForegroundColor Yellow
    .\SetAutoLogon.ps1 -domain $domain -user $username -pwd $password
        
    # Add to domain
    Write-Log "Join Domain" -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $ParamArray["primarydc"] -usr $ParamArray["username"] -pwd $ParamArray["password"]
    .\Join-Domain.ps1 "Domain" $domain $username $password $LogPath 2>&1 | Write-Output 
}

#-----------------------------------------------------------------------------
# Phase2: Set Computer Password
#-----------------------------------------------------------------------------
Function Phase2
{
    Write-Log "Entering Phase2..."

    # Turn off firewall
    cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Output

    # Disable auto password change [MS-NRPC]
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon\Parameters /v DisablePasswordChange /t REG_DWORD /d 1 /f

    # Reset computer password [MS-NRPC]
	$remoteServer = $ParamArray["primarydc"]
    $domain   = $ParamArray["domain"]
    $username = $ParamArray["username"]
    $password = $ParamArray["password"]  
    $domainNC = "DC=" + $domain.ToString().Replace(".", ",DC=")
    
    Write-Log "Set computer password"
	ksetup /SetComputerPassword $ParamArray["temppassword"]
                
	$cred=New-Object System.Management.Automation.PSCredential -ArgumentList "$domain\$username", (ConvertTo-SecureString "$password" -AsPlainText -Force) -ErrorAction Stop	
	try
    {
        Write-Log "Set computer password on PDC"
        Invoke-Command -ComputerName $remoteServer -Credential $cred -ScriptBlock {
            Param($hostName, $domainNC, $tempPassword)
		    $dcADSI=[ADSI]"LDAP://CN=$hostName,CN=Computers,$domainNC"
		    $dcADSI.SetPassword($tempPassword)
		    $dcADSI.SetInfo()
	    } -ArgumentList $ParamArray["name"], $domainNC, $ParamArray["temppassword"]
        CheckReturnValue
    }
    catch
    {
        Write-Log "Set computer password on SDC"
        $remoteServer = $ParamArray["secondarydc"]
        Invoke-Command -ComputerName $remoteServer -Credential $cred -ScriptBlock {
        Param($hostName, $domainNC, $tempPassword)
		$dcADSI=[ADSI]"LDAP://CN=$hostName,CN=Computers,$domainNC"
		$dcADSI.SetPassword($tempPassword)
		$dcADSI.SetInfo()
	    } -ArgumentList $ParamArray["name"], $domainNC, $ParamArray["temppassword"]
        CheckReturnValue
    }

}

#-----------------------------------------------------------------------------
# Phase3: Reset Computer Password
#-----------------------------------------------------------------------------
Function Phase3
{
    Write-Log "Entering Phase3..."

    # Reset computer password [MS-NRPC]
	$remoteServer = $ParamArray["primarydc"]
    $domain   = $ParamArray["domain"]
    $username = $ParamArray["username"]
    $password = $ParamArray["password"]  
    $domainNC = "DC=" + $domain.ToString().Replace(".", ",DC=")
    
    Write-Log "Set computer password again"
    ksetup /SetComputerPassword $password
            
	$cred=New-Object System.Management.Automation.PSCredential -ArgumentList "$domain\$username", (ConvertTo-SecureString $password -AsPlainText -Force) -ErrorAction Stop	
	try
    {
        Write-Log "Set computer password on PDC"
        Invoke-Command -ComputerName $remoteServer -Credential $cred -ScriptBlock {
            Param($hostName, $domainNC, $passWord)
		    $dcADSI=[ADSI]"LDAP://CN=$hostName,CN=Computers,$domainNC"
		    $dcADSI.SetPassword($password)
		    $dcADSI.SetInfo()
	    } -ArgumentList $ParamArray["name"], $domainNC, $password
        CheckReturnValue  
    }
    catch
    {
        Write-Log "Set computer password on SDC"
        $remoteServer = $ParamArray["secondarydc"]
        Invoke-Command -ComputerName $remoteServer -Credential $cred -ScriptBlock {
            Param($hostName, $domainNC, $passWord)
		    $dcADSI=[ADSI]"LDAP://CN=$hostName,CN=Computers,$domainNC"
		    $dcADSI.SetPassword($password)
		    $dcADSI.SetInfo()
	    } -ArgumentList $ParamArray["name"], $domainNC, $password
        CheckReturnValue  
    }   
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
        3 { Phase3; RestartAndResume; }
        4 { Finish;}
        default
        {
            Write-Log "Fail to execute the script" -ForegroundColor Red
            break
        }
    }
}

Main
