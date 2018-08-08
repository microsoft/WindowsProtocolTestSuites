#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
#############################################################################

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Protocol.xml")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
try { Stop-Transcript -ErrorAction SilentlyContinue } catch {} # Ignore Stop-Transcript error messages
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function ExitCode()
{ 
    return $MyInvocation.ScriptLineNumber 
}

#----------------------------------------------------------------------------
# Check input parameters
#----------------------------------------------------------------------------
if(!(Test-Path "$workingDir"))
{
    Write-Error.ps1 "WorkingDir $workingDir does not exist."
    exit ExitCode
}

if(!(Test-Path "$protocolConfigFile"))
{
    Write-Error.ps1 "protocolConfigFile $protocolConfigFile does not exist."
    exit ExitCode
}

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
[xml]$config = Get-Content "$protocolConfigFile"
if($config -eq $null)
{
    Write-Error.ps1 "protocolConfigFile $protocolConfigFile is not a valid XML file."
    exit ExitCode
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$password = $config.lab.core.password
if([System.String]::IsNullOrEmpty($password)) 
{
    $password = "Password01!"
}

#----------------------------------------------------------------------------
# Create and active test accounts
#----------------------------------------------------------------------------
Write-Info.ps1 "Create and active test accounts"
if((gwmi win32_computersystem).partofdomain -eq $true)
{
    $adminDN = dsquery user -name Administrator

    Write-Info.ps1 "Create user account: nonadmin"
    $nonadminDN = $adminDN.Replace("Administrator","nonadmin")
    CMD /C dsadd user $nonadminDN  -pwd $password -desc testaccount -disabled no -mustchpwd no -pwdneverexpires yes 2>&1 | Write-Info.ps1

    Write-Info.ps1 "Create group: AzGroup01"
    $azGroup01DN = $adminDN.Replace("Administrator","AzGroup01")
    CMD /C dsadd group $azGroup01DN 2>&1 | Write-Info.ps1

    Write-Info.ps1 "Create user account: AzUser01, and add to AzGroup01"
    $azUser01DN = $adminDN.Replace("Administrator","AzUser01")
    CMD /C dsadd user $azUser01DN  -pwd $password -desc testaccount -memberof $azGroup01DN -disabled no -mustchpwd no -pwdneverexpires yes 2>&1 | Write-Info.ps1

    Write-Info.ps1 "Enable Guest account"
    CMD /C net.exe user Guest /active:yes /Domain 2>&1 | Write-Info.ps1
    CMD /C net.exe user Guest $password 2>&1 | Write-Info.ps1

    Write-Info.ps1 "Setting password never expires"
    dsquery user -samid * | dsmod user -pwdneverexpires yes -mustchpwd no 2>&1 | Write-Info.ps1
    dsquery user -samid *| dsget user -samid -pwdneverexpires 2>&1 | Write-Info.ps1
}
else
{
    Write-Info.ps1 "Add nonadmin account"
    CMD /C net.exe user nonadmin $password /ADD 2>&1 | Write-Info.ps1

    Write-Info.ps1 "Enable Guest account"
    CMD /C net.exe user Guest /active:yes 2>&1 | Write-Info.ps1
    CMD /C net.exe user Guest $password 2>&1 | Write-Info.ps1

    Write-Info.ps1 "Setting password never expires"
    Get-WmiObject -Class Win32_UserAccount | where {$_.Domain -eq $env:ComputerName}  | foreach {$_.PasswordExpires = $false;$_.Put()}
    Get-WmiObject -Class Win32_UserAccount | where {$_.Domain -eq $env:ComputerName} | ft Caption,PasswordExpires   
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed create test accounts."
Stop-Transcript
exit 0