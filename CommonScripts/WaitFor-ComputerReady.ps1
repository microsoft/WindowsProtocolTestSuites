#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           WaitFor-ComputerReady.ps1
## Purpose:        This script pause the script until the specified file on remote machine is ready for accessing.
## Scenarios:      1) Wait for a computer starting up. In this case, will wait for ADMIN$
##                 2) Wait for something done in another computer (such as server config, or testing on client). 
##                   a) If "config.finished.signal" found, that means the configuration finished successfully;
##                   b) If "config.error.signal" found, that means the configuration terminated with exceptions;
##                   c) If "test.finished.signal" found, that means the testing finished successfully;
##                   d) If "test.error.signal" found, that means the testing terminated with exceptions;
## Version:        1.1 (26 June, 2008)
##
##############################################################################

Param ( 
[string]$computerName, 
[string]$usr, 
[string]$pwd,
[string]$signalFolder,          # "C:\Test\TestResults\Signals"
[string]$signalFileName,        # "config.finished.signal"
[int]$timeoutSec        = 600   # 3600
)

# Write Call Stack
if($function:EnterCallStack -ne $null)
{
	EnterCallStack "WaitFor-ComputerReady.ps1"
}

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [WaitFor-ComputerReady.ps1]." -foregroundcolor cyan
Write-Host "`$computerName   = $computerName"
Write-Host "`$usr            = $usr"
Write-Host "`$pwd            = $pwd"
Write-Host "`$signalFolder   = $signalFolder"
Write-Host "`$signalFileName = $signalFileName"
Write-Host "`$timeoutSec     = $timeoutSec"

#----------------------------------------------------------------------------
#Function: Show-ScriptUsage
#Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: This script pause the script until the specified file on remote machine is ready for accessing."
    Write-host
    Write-host "Example: WaitFor-ComputerReady.ps1 SUT01 username password c$ pagefile.sys 600"
    Write-host
}

#----------------------------------------------------------------------------
# Verify Required parameters
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# Validate parameter
#----------------------------------------------------------------------------
if ($computerName -eq $null -or $computerName -eq "")
{
    Throw "Parameter computerName is required."
}
if ($signalFolder -eq $null -or $signalFolder -eq "")
{
    Write-Host "No signal folder specified, will use ADMIN$ by default."
    $signalFolder = "ADMIN$"
}
else
{
    $signalFolder = $signalFolder.Replace(":", "$")
    Write-Host "Now the signal folder is $signalFolder"
}

#----------------------------------------------------------------------------
# Using global username/password when caller doesnot provide.
#----------------------------------------------------------------------------
if ($usr -eq $null -or $usr -eq "")
{
    $usr = $global:usr
    $pwd = $global:pwd
}
#----------------------------------------------------------------------------
# Make username prefixed with domain/computername
#----------------------------------------------------------------------------
#if ($usr.IndexOf("\") -eq -1)
#{
#    if ($global:domain  -eq $null -or $global:domain -eq "")
#    {
#       $usr = "$computerName\$usr"
#    }
#    else
#    {
#        $usr = "$global:domain\$usr"
#   }
#}
#[v-xich]: Remove this, we don't use domain account as default.

#----------------------------------------------------------------------------
# Check the signal file. 
# If it contains ".finish.", define a variable for ".error." (the error signal file)
#----------------------------------------------------------------------------
$checkErrorSignal = $False
$errorSignalFileName = ""
if ( $signalFileName.Contains(".finished.") )
{
    $checkErrorSignal =$True
    $errorSignalFileName = $signalFileName.Replace(".finished.",".error.")
}

#----------------------------------------------------------------------------
# Keep retry until timeout.
#----------------------------------------------------------------------------
$retryCount = 0
$isReady    = $false
$errorFound = $false
$connectCmd    = "net.exe use \\$computerName\$signalFolder $pwd /User:$usr 1>>$global:testResultDir\WaitFor-ComputerReady.ps1.std.log 2>>$global:testResultDir\WaitFor-ComputerReady.ps1.err.log"
$disconnectCmd = "net.exe use \\$computerName\$signalFolder /delete /y      1>>$global:testResultDir\WaitFor-ComputerReady.ps1.std.log 2>>$global:testResultDir\WaitFor-ComputerReady.ps1.err.log"

Write-Host "Wait for $signalFileName from $computerName (timeout: $timeoutSec) ..." -foregroundcolor Yellow
Write-host "Disconnection: net.exe use \\$computerName\$signalFolder /delete /y"
Write-Host "Connection:    net.exe use \\$computerName\$signalFolder $pwd /User:$usr"
for (; $retryCount -lt $timeoutSec/2; $retryCount++) 
{
    #----------------------------------------------------------------------------
    # Remove the credential from cache.
    #----------------------------------------------------------------------------
    cmd /c $disconnectCmd

    #----------------------------------------------------------------------------
    # Reset client cache because computer's IP address may change after restart
    #----------------------------------------------------------------------------
    cmd /c ipconfig.exe /flushdns 2>&1 1>>$global:testResultDir\WaitFor-ComputerReady.ps1.log
    cmd /c nbtstat.exe /R         2>&1 1>>$global:testResultDir\WaitFor-ComputerReady.ps1.log

    #----------------------------------------------------------------------------
    # Setup the credential to access remote computer
    #----------------------------------------------------------------------------
    Write-Host "." -noNewLine -foregroundcolor Yellow
    cmd.exe /c $connectCmd
	
	sleep 3

    if ($lastExitCode -eq 0 -or $lastExitCode -eq 2)
    {
        #----------------------------------------------------------------------------
        # No file name specified, only wait for computer starting up
        #----------------------------------------------------------------------------
        if (($signalFileName -eq $null) -or ($signalFileName -eq ""))
        {
            $isReady = $True
            cmd /c $disconnectCmd
            break
        }

        #----------------------------------------------------------------------------
        # Check whether the file exists
        #----------------------------------------------------------------------------
        $SignalFileFullName = "\\$computerName\$signalFolder\$signalFileName"
        if ( [System.IO.File]::Exists( $SignalFileFullName ) -eq $true )
        {
            $isReady = $True
            cmd /c $disconnectCmd
            break
        }

        #----------------------------------------------------------------------------
        # Check the error signal file exists
        #----------------------------------------------------------------------------
        if ($checkErrorSignal -eq $True)
        {
            $errorSignalFileFullName = "\\$computerName\$signalFolder\$errorSignalFileName"
            if ( [System.IO.File]::Exists( $errorSignalFileFullName ) -eq $true )
            {
                $errorFound = $True
                cmd /c $disconnectCmd
                break
            }
        }
    }

    #----------------------------------------------------------------------------
    # Wait for 2 seconds and retry.
    # 1 minues for a line of progress; 
    # if lastExitCode equals 0, print white dot, else red
    #---------------------------------------------------------------------------- 
    $NoNewLineIndicator = $True
    if ( $retryCount % 30 -eq 29 )
    {
       $NoNewLineIndicator = $False
    }
    $foregroundColorIndicator = "Red"
    if ( $lastExitCode -eq 0 )
    {
        $foregroundColorIndicator = "White"
    }
    Write-host "." -NoNewLine:$NoNewLineIndicator -foregroundcolor $foregroundColorIndicator 
    
    Start-Sleep -s 2  # Sleep for 2 minutes [System.Threading.Thread]::Sleep(2000)
}

#----------------------------------------------------------------------------
# Exit from loop.
#----------------------------------------------------------------------------
Write-Host "." -foregroundcolor Green

#----------------------------------------------------------------------------
# Throw exception if error found.
#----------------------------------------------------------------------------
if ($errorFound -eq $true)
{
    Throw "An error signal file found: $errorSignalFileName."
}

#----------------------------------------------------------------------------
# Throw exception if timeout.
#----------------------------------------------------------------------------
if ($isReady -ne $true)
{
    Throw "Wait for ready failed/timeout."
}

#----------------------------------------------------------------------------
# Print exit information. Show message of success.
#----------------------------------------------------------------------------
Write-Host "Sync up successfully." -foregroundcolor Green

# Write Call Stack
if($function:ExitCallStack -ne $null)
{
	ExitCallStack "WaitFor-ComputerReady.ps1"
}

exit 0
