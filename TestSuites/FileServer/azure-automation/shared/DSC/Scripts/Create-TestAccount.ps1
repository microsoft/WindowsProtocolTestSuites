# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.


# TODO: Fetch ParamConfig from Storage Account
param($workingDir = $PSScriptRoot, $protocolConfigFile = "$workingDir\Config.json", $parameterConfigFile = "$workingDir\ParamConfig.json")

# Ensure net.exe stderr (e.g. invalid usernames with '@') does not become a
# terminating error when the caller sets $ErrorActionPreference = 'Stop'.
$ErrorActionPreference = 'Continue'

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;"

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
    $protocolConfigFile = "$workingDir\Config.json"
    if(!(Test-Path "$protocolConfigFile")) 
    {
        Write-Error.ps1 "No Config file found."
        return $false
    }
}

if(!(Test-Path "$parameterConfigFile"))
{
    $parameterConfigFile = "$workingDir\ParamConfig.json"
    if(!(Test-Path "$parameterConfigFile")) 
    {
        .\Write-Error.ps1 "No ParamConfig.json found."
        return $false
    }
}
#----------------------------------------------------------------------------
# Start logging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
try { Stop-Transcript -ErrorAction SilentlyContinue } catch {} # Ignore Stop-Transcript error messages
Start-Transcript -Path "$logFile" -Append -Force

function StartService($serviceName)
{
    $service = Get-Service -Name $serviceName
    $retryTimes = 0
    while($service.Status -ne "Running" -and $retryTimes -lt 6)
    {
        .\Write-Info.ps1 "Start $serviceName service."
        Start-Service -InputObj $service -ErrorAction Continue
        Start-Sleep 10
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
# Get content from protocol config file
#----------------------------------------------------------------------------
$config = $null
try {
    $config = Get-Content -Path $protocolConfigFile -Raw | ConvertFrom-Json
}
catch {
    Write-Error.ps1 "Failed to parse config file: $_"
    return $false
}

$params = $null
try {
    $params = Get-Content -Path $parameterConfigFile -Raw | ConvertFrom-Json
}
catch {
    Write-Error.ps1 "Failed to parse parameter config file: $_"
    return $false
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$password = $config.Core.Password
if([System.String]::IsNullOrEmpty($password))
{
    .\Write-Error.ps1 "Config.json Core.Password is empty -- cannot set Guest password."
    return $false
}

$azgroups = $params.Parameters.Groups
$users =  $params.Parameters.Users
$isDomainEnv = (Get-CimInstance Win32_ComputerSystem).PartOfDomain

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
$domainName = (Get-CimInstance Win32_ComputerSystem).Domain

# Retry to wait until the ADWS can respond to PowerShell commands correctly
if($isDomainEnv -eq $true)
{
    $retryTimes = 0
    $domain = $null
    while ($retryTimes -lt 30) {
        $domain = Get-ADDomain $domainName
        if ($null -ne $domain) {
            break;
        }
        else {
            Start-Sleep 10
            $retryTimes += 1
        }
    }

    if ($null -eq $domain) {
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
if ($isDomainEnv -eq $true)
{
    $domainAdmin = $config.Core.Username

    $adminDN = dsquery user -name $domainAdmin

    foreach($group in $azgroups)
    {        
        .\Write-Info.ps1 "Create group: $($group.Group.GroupName)"
        $azGroupDN = $group.Group.GroupName 
        New-ADGroup -Name $azGroupDN -GroupScope Global -GroupCategory Security
    }

    foreach($user in $users.User)
    {
        .\Write-Info.ps1 "Create user: $($user.Username)"
        try {
            $domainDN = "DC=" + $domainName.Replace(".", ",DC=")
            $userDN = "CN=$($user.Username),CN=Users,$domainDN"
            dsadd user "$userDN" -pwd $user.Password -canchpwd no -display "$($user.Username)" -disabled no -pwdneverexpires yes | .\Write-Info.ps1

            if($null -ne $user.Group)
            {
                $aduser = Get-ADUser -Identity $user.Username
                Add-ADGroupMember -Identity $user.Group -Members $aduser
            }
        } catch {
            .\Write-Info.ps1 "[WARN] Failed to create domain user '$($user.Username)': $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }

    .\Write-Info.ps1 "Enable Guest account"
    & net.exe user Guest /active:yes /Domain 2>&1 | .\Write-Info.ps1
    & net.exe user Guest $password 2>&1 | .\Write-Info.ps1

    .\Write-Info.ps1 "Setting password never expires"
    dsquery user -samid * | dsmod user -pwdneverexpires yes -mustchpwd no 2>&1 | .\Write-Info.ps1
    dsquery user -samid * | dsget user -samid -pwdneverexpires 2>&1 | .\Write-Info.ps1
}
else
{
    foreach($group in $azgroups)
    {
        .\Write-Info.ps1 "Create group: $($group.Group.GroupName)"
        $azGroupDN = $group.Group.GroupName
        try {
            & net.exe localgroup $azGroupDN /ADD 2>&1 | .\Write-Info.ps1
        } catch {
            .\Write-Info.ps1 "[WARN] Failed to create group '$azGroupDN': $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }

    foreach($user in $users.User)
    {
        .\Write-Info.ps1 "Create user account: $($user.Username)"
        try {
            & net.exe user $user.Username $user.Password /ADD 2>&1 | .\Write-Info.ps1
            if ($LASTEXITCODE -ne 0) {
                .\Write-Info.ps1 "[WARN] net.exe user failed for '$($user.Username)' (exit code $LASTEXITCODE)" -ForegroundColor Yellow
                continue
            }
            if($null -ne $user.Group)
            {
                & net.exe localgroup $user.Group $user.Username /ADD 2>&1 | .\Write-Info.ps1
            }
        } catch {
            .\Write-Info.ps1 "[WARN] Failed to create user '$($user.Username)': $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }

    .\Write-Info.ps1 "Enable Guest account"
    & net.exe user Guest /active:yes 2>&1 | .\Write-Info.ps1
    & net.exe user Guest $password 2>&1 | .\Write-Info.ps1

    .\Write-Info.ps1 "Setting password never expires"
    Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:ComputerName'" | ForEach-Object { Set-CimInstance -InputObject $_ -Property @{PasswordExpires = $false} }
    Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:ComputerName'" | Format-Table Caption,PasswordExpires   
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Completed create test accounts."
Pop-Location
Stop-Transcript
return $true