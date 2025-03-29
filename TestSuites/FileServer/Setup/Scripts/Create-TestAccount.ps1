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

function StartService($serviceName)
{
    $service = Get-Service -Name $serviceName
    $retryTimes = 0
    while($service.Status -ne "Running" -and $retryTimes -lt 6)
    {
        .\Write-Info.ps1 "Start $serviceName service."
        Start-Service -InputObj $service -ErrorAction Continue
        Sleep 10
        $retryTimes++ 
        $service = Get-Service -Name $serviceName
    }

    if($retryTimes -ge 6)
    {
        Write-Error.ps1 "Start $serviceName service failed within 1 minute."
    }
    else
    {
        .\Write-Info.ps1 "Service $serviceName is Running."
    }
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
$isDomainEnv = (gwmi win32_computersystem).partofdomain

#----------------------------------------------------------------------------
# Start required services
#----------------------------------------------------------------------------
if($isDomainEnv -eq $true)
{
    .\Write-Info.ps1 "Check and start Active Directory Domain Services"
    StartService "NTDS"

    .\Write-Info.ps1 "Check and start Active Directory Web Services"
    StartService "ADWS"
}
else
{
    .\Write-Info.ps1 "Workgroup env, skip checking Active Directory Services"
}

#----------------------------------------------------------------------------
# Create CBAC ENV
#----------------------------------------------------------------------------
$domainName = (Get-WmiObject win32_computersystem).Domain

# Retry to wait until the ADWS can respond to PowerShell commands correctly
if($isDomainEnv -eq $true)
{
    $retryTimes = 0
    $domain = $null
    while ($retryTimes -lt 30) {
        $domain = Get-ADDomain $domainName
        if ($domain -ne $null) {
            break;
        }
        else {
            Start-Sleep 10
            $retryTimes += 1
        }
    }

    if ($domain -eq $null) {
        .\Write-Error.ps1 "Failed to get correct responses from the ADWS service after strating it for 5 minutes."
    }
}
else
{
    .\Write-Info.ps1 "Workgroup env, skip checking ADWS Services"
}

#----------------------------------------------------------------------------
# Create and active test accounts
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Create and active test accounts"
if($isDomainEnv -eq $true)
{
    $domainAdmin = $config.lab.core.username

    $adminDN = dsquery user -name $domainAdmin

    foreach($g in $azgroups)
    {        
        .\Write-Info.ps1 "Create group: $($g.Group.GroupName)"
        $azGroupDN = $g.Group.GroupName 
        New-ADGroup -Name $azGroupDN -GroupScope Global -GroupCategory Security
    }

    foreach($user in $users.User)
    {        
        .\Write-Info.ps1 "Create user: $($user.Username)"
        $domainDN = "DC=" + $domainName.Replace(".", ",DC=")
        $userDN = "CN=$($user.Username),CN=Users,$domainDN"
        dsadd user "$userDN" -pwd $user.Password -canchpwd no -display "$($user.Username)" -disabled no -pwdneverexpires yes | .\Write-Info.ps1
	      
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
    dsquery user -samid * | dsget user -samid -pwdneverexpires 2>&1 | .\Write-Info.ps1
}
else
{
    foreach($g in $azgroups)
    {
        .\Write-Info.ps1 "Create group: $($g.Group.GroupName)"
        $azGroupDN = $g.Group.GroupName
        CMD /C net.exe localgroup $azGroupDN /ADD 2>&1 | .\Write-Info.ps1
    }

    foreach($user in $users.User)
    {        
        .\Write-Info.ps1 "Create user account: $($user.Username)"
        CMD /C net.exe user $user.Username $user.Password /ADD 2>&1 | .\Write-Info.ps1
        if($user.Group -ne $null)
        {
            CMD /C net.exe localgroup $user.Group $user.Username /ADD 2>&1 | .\Write-Info.ps1
        }
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