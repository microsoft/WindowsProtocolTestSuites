# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param($workingDir = $PSScriptRoot, $protocolConfigFile = "$workingDir\Config.json")


# Start Logging
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

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

# Determine our Server
$server = $config.Machines.DC

if ($null -eq $server) {
    Write-Error.ps1 "Cannot find DC configuration in config file."
    return $false
}

if (![System.String]::IsNullOrEmpty($server.domain)) {   
    $domainName = $server.domain    
}
elseif (![System.String]::IsNullOrEmpty($config.Core.DomainName)) {
    $domainName = $config.Core.DomainName
}
else {
    $domainName = "contoso.com"
}

$userName = "Administrator"

if ([System.String]::IsNullOrEmpty($server.username )) {
    if (![System.String]::IsNullOrEmpty($config.Core.Username)) {	
        $userName = $config.Core.Username
    }
}
else {
    $userName = $server.username
}

if ([System.String]::IsNullOrEmpty($server.password)) {
    
    $adminPwd = $config.Core.Password

}
else {
    $adminPwd = $server.Password
}

# Promote DC
.\Write-Info.ps1 "Promoting this computer to DC." -ForegroundColor Green
if (-not (.\PromoteDomainController.ps1 -DomainName $domainName -AdminPwd $adminPwd -AdminUser $userName)) {
    Write-Error.ps1 "Failed to promote this computer to DC."
    return $false
}

.\Write-Info.ps1 "Setting auto logon." -ForegroundColor Green
if (-not (.\Set-AutoLogon.ps1 -Domain $domainName -Username "$domainName\$userName" -Password $adminPwd)) {
    Write-Error.ps1 "Failed to set auto logon."
    return $false
}

.\Write-Info.ps1 "Promote DC completed." -ForegroundColor Green

# Stop the transcript
Stop-Transcript -ErrorAction SilentlyContinue

return $true