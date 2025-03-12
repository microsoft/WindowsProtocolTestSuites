# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    [Parameter(Mandatory = $true)]
    [string]$Role,
    [Parameter(Mandatory = $true)]
    [string]$ConfigureFile = "$PSScriptRoot\Config.json",
    [Parameter(Mandatory = $true)]
    [bool]$Update = $false
)

$testDir = $PSScriptRoot

Start-Transcript -Path "$testDir\Validate-Configs.ps1.log" -Append -Force

if (-not (Test-Path -Path $ConfigureFile)) {
    .\Write-Info.ps1 "Protocol configure file $ConfigureFile does not exist." -ForegroundColor Red
    Stop-Transcript
    exit 1
}

$config = $null

try {
    $config = Get-Content -Path $ConfigureFile -Raw | ConvertFrom-Json
}
catch {
    .\Write-Info.ps1 "Failed to parse config file: $_" -ForegroundColor Red
    Stop-Transcript
    exit 1
}

$VM = $config.Machines.$Role

if ($null -eq $VM) {
    .\Write-Info.ps1 "Cannot find Vm configure for VM $Role." -ForegroundColor Red
    Stop-Transcript
    exit 1
}


$ipConfigs = $VM.IpConfig

foreach ($ipConfig in $ipConfigs) {
    $ip = $ipConfig.ip
    .\Write-Info "Checking IP Address $ip exists on this machine." -ForegroundColor Yellow

    $staticIP = Get-NetIPAddress -AddressFamily IPv4 | Where-Object { $_.IPAddress -eq $ip -and $_.PrefixOrigin -eq 'Manual' }

    if ($staticIP) {
        .\Write-Info "The static IP address $ip exists on this machine." -ForegroundColor Green
    }
    else {
        .\Write-Info "The static IP address $ip does not exist on this machine." -ForegroundColor Red
    }
}

# Check Computer Name
if ($null -eq $VM.ComputerName) {
    .\Write-Info.ps1 "Computer name is not configured." -ForegroundColor Red
    Stop-Transcript
    exit 1
}

$hostname = hostname

if ($hostname -ne $VM.ComputerName) {
    .\Write-Info.ps1 "Computer name is not set correctly." -ForegroundColor Red
    if ($Update) {
        .\Write-Info.ps1 "Setting computer name to $($VM.ComputerName)" -ForegroundColor Green
        Rename-Computer -NewName "$($VM.ComputerName)" -Force
    }
    else {        
        Stop-Transcript
        exit 1
    }
}

Stop-Transcript
exit 0