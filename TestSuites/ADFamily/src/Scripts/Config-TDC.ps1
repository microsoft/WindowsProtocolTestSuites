#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Config-TDC.ps1
## Purpose:        Configure Trusted DC for Active Directory test suites.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2
##
##############################################################################
Param
(
    [alias("h")][switch]$help,
    [string]$VMName = "AD_TDC", # The Virtual Machine's name
    [int]$Step      = 1         # Current step for configuration
)

if($help)
{
$helpmsg = @"
Post script to config Trusted DC.

Usage:
    .\Config-TDC.ps1 [-VMName <vmname>] [-Step <step>] [-h | -help]

VMName: The name of the VM to be created. The default value is AD_TDC.
Step: Current step for configuration. The default value is 1.
help(h) : Display this help message.

"@
    Write-Output $helpmsg
    return
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

    Write-Log "Executing [$ScriptName]" -ForegroundColor Cyan

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
    Start-Transcript $LogFile -Force -Append 2>&1 | Out-Null
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
        Write-Output "`r`n$date Error in line $line.`r`n"
        Write-Output "**********************"
        Write-Output "Dump local variables"
        Write-Output "**********************"
        Format-Table Name,Value -wrap -autosize -inputobject $vars
        Stop-Transcript
        throw "Error in line $line."
    }
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
# Set Alternate DNS 
#-----------------------------------------------------------------------------
Function SetAlternateDNS
{
	$nics = Get-WmiObject -Class Win32_NetworkAdapterConfiguration -Filter IPEnabled=TRUE | where {$_.ServiceName -eq "netvsc" -or $_.ServiceName -eq "dc21x4VM"} | sort MACAddress
    foreach($nic in $nics){
        netsh interface ipv4 add dnsservers $nic.interfaceindex $ParamArray["alternateDNS"] index=2
    }
}

Function AddToLocalAdmin([string]$domainName, [string]$userName)
{
	Write-Log "Adding $domainName/$userName to local administrators group"
    $group = [ADSI]("WinNT://"+$env:COMPUTERNAME+"/administrators,group")
	$group.add("WinNT://$domainName/$userName,user")
    CheckReturnValue	
}

#-----------------------------------------------------------------------------
# Phase1: SetNetworkConfiguration; PromoteTDC
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
    .\WaitFor-ComputerReady.ps1 -computerName $ParamArray["trusttargetserver"] -usr $ParamArray["trusttargetuser"] -pwd $ParamArray["trusttargetpwd"]
    .\PromoteDomainController.ps1 -DomainName $ParamArray["domain"] -AdminPwd $ParamArray["password"]

    # [MS-ADTS-Security]
    Write-Log "Setting security level" -ForegroundColor Yellow  
    $objWMIService = new-object -comobject WbemScripting.SWbemLocator
    $objWMIService.Security_.ImpersonationLevel = 3
    $objWMIService.Security_.AuthenticationLevel = 6
        
    sleep 10
}

#-----------------------------------------------------------------------------
# Phase2: SetAdminAccount
#-----------------------------------------------------------------------------
Function Phase2
{
    Write-Log "Entering Phase2..."
    Sleep 10
    # Turn off firewall
    cmd /c netsh advfirewall set allprofile state off 2>&1 | Write-Output

    # Configure the Netlogon service to depend on the DNS service
    reg add HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\Netlogon /v DependOnService /t REG_MULTI_SZ /d LanmanWorkstation\0LanmanServer\0DNS /f

    # Create forest trust on local side [MS-ADTS-Schema][MS-NRPC]
    Write-Log "Create forest trust relationship on local side" -ForegroundColor Yellow
    .\CreateLocalSideForestTrust.ps1 -TargetForestName $ParamArray["trusttargetdomain"] -TrustPassword $ParamArray["trustpassword"]    

    # Create forest trust on remote side [MS-NRPC]
    Write-Log "Create forest trust relationship on remote side ..." -ForegroundColor Yellow
    
    # Trigger Phase 3 on Primary DC [MS-NRPC]
    Write-Log "Trigger Phase 3 on Primary DC ..." -ForegroundColor Yellow
    .\WaitFor-ComputerReady.ps1 -computerName $ParamArray["trusttargetserver"] -usr $ParamArray["trusttargetuser"] -pwd $ParamArray["trusttargetpwd"]
    $SecurePwd = ConvertTo-SecureString $ParamArray["trusttargetpwd"] -AsPlainText -Force
    $cred = New-Object System.Management.Automation.PSCredential $ParamArray["trusttargetuser"], $SecurePwd
    $remoteSession = New-PSSession -ComputerName $ParamArray["trusttargetserver"] -Credential $cred
    Invoke-Command -Session $remoteSession `
                   -ArgumentList $ScriptPath `
                   -ScriptBlock {
                        Param
                        (
                            $WorkingPathT
                        )

                        & "$WorkingPathT\Config-PDC.ps1" -Step 3
                   }

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

    Sleep 30
    .\WaitFor-ComputerReady.ps1 -computerName $ParamArray["trusttargetserver"] -usr $ParamArray["trusttargetuser"] -pwd $ParamArray["trusttargetpwd"]
    # Verify the bidirectional trust
    $localforest = [System.DirectoryServices.ActiveDirectory.Forest]::getCurrentForest() 
    $strRemoteForest = $ParamArray["trusttargetdomain"]
    $strRemoteUser = $ParamArray["trusttargetuser"] 
    $strRemotePassword = $ParamArray["trustpassword"]
    $remoteContext = New-Object System.DirectoryServices.ActiveDirectory.DirectoryContext("Forest", $strRemoteForest,$strRemoteUser,$strRemotePassword) 
    $remoteForest = [System.DirectoryServices.ActiveDirectory.Forest]::getForest($remoteContext)     
    $returnVal = $localForest.VerifyTrustRelationship($remoteForest,"Bidirectional")
    Write-Log "successfully verified trust relationship"
    AddToLocalAdmin $ParamArray["trusttargetdomain"] $ParamArray["username"]
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
        2 { Phase2; Finish; }
        default
        {
            Write-Log "Fail to execute the script" -ForegroundColor Red
            break
        }
    }
}

Main
