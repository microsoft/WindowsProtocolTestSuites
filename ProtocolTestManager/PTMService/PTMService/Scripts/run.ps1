# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$serviceName = "PTMService"
$psExe = if ($IsLinux) { "pwsh" } else { "powershell" }

$confirmPrompt = "(Y/n)"
$denyPrompt = "(y/N)"

Write-Host "Working directory is $PSScriptRoot."

# Uncomment or modify the following line to set the PTMService URLs.
# $env:ASPNETCORE_URLS = "http://localhost:5000;https://localhost:5001"

function Read-Confirmation {
    param (
        [bool]$IsYesDefault = $true
    )

    $yesPattern = "[yY][eE][sS]|[yY]"
    $noPattern = "[nN][oO]|[nN]"

    while ($true) {
        $value = Read-Host
        if ([string]::IsNullOrEmpty($value)) {
            return $IsYesDefault
        }
        elseif ($value -match $yesPattern) {
            return $true
        }       
        elseif ($value -match $noPattern) {
            return $false
        }
        else {
            Write-Warning "Matching failed, please check your input."
        }
    }
}

# Check installation flag.
$flagPath = "$PSScriptRoot/.installed"
if (-not (Test-Path $flagPath)) {
    Write-Host "$serviceName is not installed, do you want to start a new installation now? $confirmPrompt"
    if (Read-Confirmation) {
        & $psExe "$PSScriptRoot/install.ps1"
        exit
    }
    else {
        exit
    }
}

if (Test-Path $flagPath) {
    dotnet "$PSScriptRoot/PTMService.dll"
}