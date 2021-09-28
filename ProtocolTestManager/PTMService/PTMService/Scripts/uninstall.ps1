# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$serviceName = "PTMService"

$confirmPrompt = "(Y/n)"
$denyPrompt = "(y/N)"

Write-Host "Working directory is $PSScriptRoot."

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
    Write-Host "$serviceName is not installed, exit."
    exit
}

Write-Host "Do you want to uninstall the current $serviceName installation now?"
Write-Host "The operation cannot be restored. $denyPrompt"
if (-not (Read-Confirmation -IsYesDefault $false)) {
    exit
} 

# Restore initial settings
$appSettingsPath = "$PSScriptRoot/appsettings.json"
$databasePath = "$PSScriptRoot/ptmservice.db"
$bashScriptPath = "$PSScriptRoot/run.sh"
$batchScriptPath = "$PSScriptRoot/run.bat"
$psScriptPath = "$PSScriptRoot/run.ps1"

$backupPath = "$PSScriptRoot/.bak"
$appSettingsBackupPath = "$backupPath/appsettings.json"
$databaseBackupPath = "$backupPath/ptmservice.db"
$bashScriptBackupPath = "$backupPath/run.sh"
$batchScriptBackupPath = "$backupPath/run.bat"
$psScriptBackupPath = "$backupPath/run.ps1"

Copy-Item $appSettingsBackupPath $appSettingsPath -Force -ErrorAction SilentlyContinue
Copy-Item $databaseBackupPath $databasePath -Force -ErrorAction SilentlyContinue
Copy-Item $bashScriptBackupPath $bashScriptPath -Force -ErrorAction SilentlyContinue
Copy-Item $batchScriptBackupPath $batchScriptPath -Force -ErrorAction SilentlyContinue
Copy-Item $psScriptBackupPath $psScriptPath -Force -ErrorAction SilentlyContinue

if (-not $IsLinux) {
    Remove-Item "$Home/Desktop/PTMService.lnk" -Force -ErrorAction SilentlyContinue
}

$flag = Get-Content $flagPath | ConvertFrom-Json
Write-Host "You can backup or clean up the data in the following locations:"
Write-Host "PTMService Storage Root: $($flag.storageRootPath)"
Write-Host "Database: $($flag.storageDatabasePath)"

Remove-Item $flagPath -Force -ErrorAction SilentlyContinue

Read-Host