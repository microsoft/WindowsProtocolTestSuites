########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

#############################################################################
##
## Microsoft Windows Powershell Sripting
## File:           MS-AUTHSOD-AS-Postscript.ps1
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012
##
##############################################################################
Param
(
    [int]$Step = 1 # Used for rebooting
)

#----------------------------------------------------------------------------
# Get working directory and log file path
#----------------------------------------------------------------------------
$workingDir=$MyInvocation.MyCommand.path
$workingDir =Split-Path $workingDir
$runningScriptName=$MyInvocation.MyCommand.Name
$logFile="$workingDir\$runningScriptName.log"
$signalFile="$workingDir\$runningScriptName.signal"

Function Prepare()
{
    #----------------------------------------------------------------------------
    #Createthe log file
    #----------------------------------------------------------------------------
    echo "-----------------$runningScriptName Log----------------------" > $logFile
    echo "Executing [$runningScriptName.ps1]."  >> $logFile


    #----------------------------------------------------------------------------
    # Function: Show-ScriptUsage
    # Usage   : Describes the usage information and options
    #----------------------------------------------------------------------------
    function Show-ScriptUsage
    {    
        echo "----------------$runningScriptName Log----------------------" > $logFile
        echo "Usage: This script will open the cmd.exe with a domain user account."   >> $logFile
        echo "Example: $runningScriptName.ps1 username password domainname"  >> $logFile	    
    }
    #----------------------------------------------------------------------------
    # Show help if required
    #----------------------------------------------------------------------------
    if ($args[0] -match '-(\?|(h|(help)))')
    {
        Show-ScriptUsage 
        return
    }
}



Function RestartAndResume(
[Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [String]$ScriptPath,
        
        [Parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()]
        [String]$PhaseIndicator,

        [Parameter(Mandatory=$false)]
        [String]$ArgumentList,

        [Parameter(Mandatory=$false)]
        [bool]$AutoRestart = $false)
{
    

    # Only absolute path is accepted. Because relative path
    # may cause trouble after rebooting.
    if([System.IO.Path]::IsPathRooted($ScriptPath) -eq $False)
    {
        throw "Argument ScriptPath must be absolute path"
    }

    $private:regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
    $private:regKeyName = "TKFRSAR"

    # If the key has already been set, remove it
    if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
    {
	    Remove-ItemProperty -Path $regRunPath -Name $regKeyName
    }

    try
    {
        Set-ItemProperty -Path $regRunPath -Name $regKeyName `
                            -Value "cmd /c powershell $ScriptPath $PhaseIndicator $ArgumentList" `
                            -Force -ErrorAction Stop
    }
    catch
    {
        throw "Unable to restart. Error happened: $_.Exception.Message"
    }

    # Show prompt
    Write-Host "The computer is going to restart..." -ForegroundColor Yellow
    if($AutoRestart -eq $false) { Pause } # Waiting for key stoke
    else { sleep 3 } # Auto restart

    # Restart
    Restart-Computer -Force
}


Function Autologon()
{

    Write-host "set autologon account"

    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v AutoAdminLogon /t REG_SZ /d 1 /f
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultDomainName /t REG_SZ /d "contoso.com" /f
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultUserName /t REG_SZ /d "administrator" /f
    cmd /c REG ADD "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" /v DefaultPassword /t REG_SZ /d "Password01!" /f


}


#-----------------------------------------------------------------------------
# Function: RestartAndRunFinish
#-----------------------------------------------------------------------------
Function RestartAndRunFinish()
{
    $private:regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
    $private:regKeyName = "TKFRSAR"

    if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
    {
	    Remove-ItemProperty -Path $regRunPath -Name $regKeyName
    }
}



Function Phase1
{  
    echo "Autologon" >> $logFile
    Autologon

}

Function Phase2
{
    echo "phase2" > $signalFile
}

Function Finish
{    
    RestartAndRunFinish
    Write-Host "DONE" -ForegroundColor Green
    echo "Autologon" >> $logFile
    echo "Done" >> $signalFile
}

#-----------------------------------------------------------------------------
# Main Script
#-----------------------------------------------------------------------------
Function Main
{

    prepare

    switch($Step)
    {

        1 {
            Phase1
    		
            $invocation = (get-variable MyInvocation -Scope 1).Value
            echo "invocation $invocation" >> $logFile

            $NextStep=$step+1
            echo "Restartandresume" >> $logFile

            RestartAndResume -ScriptPath $invocation.MyCommand.Path `
                        -PhaseIndicator "-Step $NextStep" -AutoRestart:$true >> $logFile
        }
        2 {
            echo "call phase 2">> $logFile
            Phase2 >> $logFile
            Finish
        }
    }
}

Main



