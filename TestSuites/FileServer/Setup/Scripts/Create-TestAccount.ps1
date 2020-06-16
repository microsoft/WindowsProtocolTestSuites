# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Protocol.xml", $parameterConfigFile = "$workingDir\Scripts\ParamConfig.xml")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

Push-Location $workingDir
#----------------------------------------------------------------------------
# if working dir is not exists. it will use scripts path as working path
#----------------------------------------------------------------------------
if(!(Test-Path "$workingDir"))
{
    $workingDir = $scriptPath
}

if(!(Test-Path "$protocolConfigFile"))
{
    $protocolConfigFile = "$workingDir\Scripts\Protocol.xml"
    if(!(Test-Path "$protocolConfigFile")) 
    {
        .\Write-Error.ps1 "No protocol.xml found."
        exit ExitCode
    }
}

if(!(Test-Path "$parameterConfigFile"))
{
    $parameterConfigFile = "$workingDir\Scripts\ParamConfig.xml"
    if(!(Test-Path "$parameterConfigFile")) 
    {
        .\Write-Error.ps1 "No ParamConfig.xml found."
        exit ExitCode
    }
}
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
# Get content from protocol config files
#----------------------------------------------------------------------------
[xml]$config = Get-Content "$protocolConfigFile"
if($config -eq $null)
{
    .\Write-Error.ps1 "Protocol config file $protocolConfigFile is not a valid XML file."
    exit ExitCode
}

[xml]$params = Get-Content "$parameterConfigFile"
if($params -eq $null)
{
    .\Write-Error.ps1 "Param Config file $parameterConfigFile is not a valid XML file."
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

$azgroups = $params.Parameters.Groups
$users =  $params.Parameters.Users

#----------------------------------------------------------------------------
# Create and active test accounts
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Create and active test accounts"
if((gwmi win32_computersystem).partofdomain -eq $true)
{
    $domainAdmin = $config.lab.core.username

    $adminDN = dsquery user -name $domainAdmin

    foreach($g in $azgroups)
    {        
        .\Write-Info.ps1 "Create group: $g.Group.GroupName"
        $azGroupDN = $g.Group.GroupName 
        New-ADGroup -Name $azGroupDN -GroupScope Global -GroupCategory Security
    }

     foreach($user in $users.User)
    {        
        .\Write-Info.ps1 "Create user: $user.Username"
        $pwd = ConvertTo-SecureString $user.Password -AsPlainText -Force
        New-ADUser -Name $user.Username -AccountPassword $pwd -CannotChangePassword $true -DisplayName $user -Enabled $true  -PasswordNeverExpires $true 
	      
        if($user.Group -ne $null)
        {
            $aduser = Get-ADUser -Identity $user.Username
            Add-ADGroupMember -Identity $user.Group -Members $aduser
        }      
    }

    .\Write-Info.ps1 "Enable Guest account"
    CMD /C net.exe user Guest /active:yes /Domain 2>&1 | .\Write-Info.ps1
    CMD /C net.exe user Guest $password 2>&1 | .\Write-Info.ps1

    .\Write-Info.ps1 "Setting password never expires"
    dsquery user -samid * | dsmod user -pwdneverexpires yes -mustchpwd no 2>&1 | .\Write-Info.ps1
    dsquery user -samid *| dsget user -samid -pwdneverexpires 2>&1 | .\Write-Info.ps1
}
else
{
    foreach($user in $users.User)
    {        
        .\Write-Info.ps1 "Create user account:$user.Username"
        CMD /C net.exe user $user.Username $user.Password /ADD 2>&1 | .\Write-Info.ps1   
    }

    .\Write-Info.ps1 "Enable Guest account"
    CMD /C net.exe user Guest /active:yes 2>&1 | .\Write-Info.ps1
    CMD /C net.exe user Guest $password 2>&1 | .\Write-Info.ps1

    .\Write-Info.ps1 "Setting password never expires"
    Get-WmiObject -Class Win32_UserAccount | where {$_.Domain -eq $env:ComputerName}  | foreach {$_.PasswordExpires = $false;$_.Put()}
    Get-WmiObject -Class Win32_UserAccount | where {$_.Domain -eq $env:ComputerName} | ft Caption,PasswordExpires   
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Completed create test accounts."
Pop-Location
Stop-Transcript
exit 0