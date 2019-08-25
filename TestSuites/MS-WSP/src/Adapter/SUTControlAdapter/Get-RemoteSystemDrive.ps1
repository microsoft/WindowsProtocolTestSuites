#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Get-RemoteSystemDrive.ps1
## Author:         v-zhil
## Purpose:        Get the system drive letter of a remote or local computer for powershell adapter
## Version:        1.0 (18 May, 2008)
## Requirements:   Windows Powershell 2.0 CTP2
## Supported OS:   Windows 2008 Server, Windows 2003 Server, Windows Vista, Windows XP
##
##############################################################################

param(
[string]$computerName,
[string]$userName,
[string]$password
)

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    

}

#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# Verify required parameters
#----------------------------------------------------------------------------
if ($computerName -eq $null -or $computerName -eq "")
{
    Throw "Parameter computerName is required."
}

#----------------------------------------------------------------------------
# Using global username/password when caller doesnot provide.
#----------------------------------------------------------------------------
if ($userName -eq $null -or $userName -eq "")
{
    $userName = $global:usr
    $password = $global:pwd
}
#----------------------------------------------------------------------------
# Make username prefixed with domain/computername
#----------------------------------------------------------------------------
if ($userName.IndexOf("\") -eq -1)
{
    if ($global:domain  -eq $null -or $global:domain -eq "")
    {
        $userName = "$computerName\$userName"
    }
    else
    {
        $userName = "$global:domain\$userName"
    }
}
#----------------------------------------------------------------------------
# Convert the password to a SecureString
#----------------------------------------------------------------------------
$securePwd  = New-Object System.Security.SecureString
for ($i = 0; $i -lt $password.Length; $i++)
{
    $securePwd.AppendChar($password[$i]);
}
$credential = New-Object System.Management.Automation.PSCredential($userName, $securePwd) 

#----------------------------------------------------------------------------
# Wait the computer is started up
#----------------------------------------------------------------------------
$disconnectCmd = "net.exe use \\$computerName\IPC$ /delete /y      1>>netusesuc.tmp.log 2>>netuseerr.tmp.log"
$connectCmd    = "net.exe use \\$computerName\IPC$ $password /User:$userName 1>>netusesuc.tmp.log 2>>netuseerr.tmp.log"
cmd /c $disconnectCmd 
cmd.exe /c $connectCmd
if ($lastExitCode -ne 0)
{
    cmd /c $disconnectCmd 
    .\WaitFor-ComputerReady.ps1 $computerName  $userName $password 
}
cmd /c $disconnectCmd 

#----------------------------------------------------------------------------
# Wait the computer RPCServer is online
#----------------------------------------------------------------------------
$waitTimeout = 600
$osObj = $null
$retryCount = 0
for (; $retryCount -lt $waitTimeout/2; $retryCount++ ) 
{
    $osObj = get-wmiobject win32_operatingsystem -computer $computerName -Credential $credential 
    if($osObj -ne $null)
    {
        break;  
    }
    
    $NoNewLineIndicator = $True
    if ( $retryCount % 60 -eq 59 )
    {
       $NoNewLineIndicator = $False
    }
    
    Start-Sleep -s 2  # Sleep for 2 seconds [System.Threading.Thread]::Sleep(2000)
}
if ($osObj -eq $null)
{
    Throw "Connect to remote computer $computerName  failed."
}

#----------------------------------------------------------------------------
# Get the system drive letter
#----------------------------------------------------------------------------
$systemDrive = $osObj.SystemDrive;
if (($systemDrive -ne $null) -and ($systemDrive -ne ""))
{

}
else
{
    throw "Cannot get the system drive for $computerName"
}

#----------------------------------------------------------------------------
# Print Exit information
#----------------------------------------------------------------------------


return $systemDrive