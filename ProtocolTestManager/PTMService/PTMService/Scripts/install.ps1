# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$serviceName = "PTMService"
$psExe = if ($IsLinux) { "pwsh" } else { "powershell" }

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
if (Test-Path $flagPath) {
    Write-Host "$serviceName is successfully installed, do you want to start it? $confirmPrompt"
    if (Read-Confirmation) {
        & $psExe "$PSScriptRoot/run.ps1"
        exit
    }
    else {
        exit
    }
}

# Backup initial app settings and database.
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

if ((-not (Test-Path $appSettingsPath)) -or (-not (Test-Path $databasePath))) {
    Write-Error "Your $serviceName distribution is corrupted, please get a new package form the source."
    exit -1
}
else {
    if (-not (Test-Path $backupPath)) {
        mkdir $backupPath
        Copy-Item $appSettingsPath $appSettingsBackupPath
        Copy-Item $databasePath $databaseBackupPath
        Copy-Item $bashScriptPath $bashScriptBackupPath
        Copy-Item $batchScriptPath $batchScriptBackupPath
        Copy-Item $psScriptPath $psScriptBackupPath
    }
}

# Start initial configuration.
$installationId = -join ((65..90) + (97..122) | Get-Random -Count 5 | ForEach-Object { [char]$_ })
Write-Host "Your current installation ID is $installationId."
Write-Host "Do you want to specify another value as the current installation ID? $denyPrompt"
if (Read-Confirmation -IsYesDefault $false) {
    Write-Host "Please enter your installation ID:"
    $installationId = Read-Host
}

$storagePath = "$PSScriptRoot/$installationId"
$storageDatabasePath = "$storagePath/ptmservice.db"

mkdir $storagePath
Copy-Item $databasePath $storageDatabasePath

function Read-UserDefinedValue {
    param (
        [string]$Name,
        [string]$DefaultValue
    )

    Write-Host "The default value for $Name is $DefaultValue."
    Write-Host "Do you want to specify another $Name value? $denyPrompt"
    if (Read-Confirmation -IsYesDefault $false) {
        $destExists = $false
        while (-not $destExists) {
            Write-Host "Please enter your value for $Name`:"
            $userDefinedValue = (Read-Host).Trim("`"")
            if (Test-Path $userDefinedValue) {
                $destExists = $true
                return $userDefinedValue
            }
            else {
                Write-Warning "Path $userDefinedValue does not exist please specify a correct value."
            }
        }
    }
    else {
        return $DefaultValue
    }
}

$storageRootPath = Read-UserDefinedValue -Name "PTMServiceStorageRoot" -DefaultValue $storagePath
$storageDatabasePath = "$storageRootPath/ptmservice.db"
if (-not (Test-Path $storageDatabasePath)) {
    Write-Host "Copy the initial database file to $storageDatabasePath."
    Copy-Item $databasePath $storageDatabasePath
}

$appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
$appSettings.ConnectionStrings.Database = "Data Source = `"$storageDatabasePath`""
$appSettings.PTMServiceStorageRoot = $storageRootPath

[System.IO.File]::WriteAllText($appSettingsPath, ($appSettings | ConvertTo-Json))

if (-not $IsLinux) {
    Write-Host "Do you want to create a desktop shortcut to $serviceName`? $confirmPrompt"
    if (Read-Confirmation) {
        $wsh = New-Object -ComObject WScript.Shell
        $shortcut = $wsh.CreateShortcut("$Home/Desktop/PTMService.lnk")
        $shortcut.TargetPath = "$PSScriptRoot/run.bat"
        $shortcut.WorkingDirectory = "$PSScriptRoot"
        $shortcut.Save()
    }
}

$flag = @{
    installationId      = $installationId
    storageRootPath     = $storageRootPath
    storageDatabasePath = $storageDatabasePath
}

[System.IO.File]::WriteAllText($flagPath, ($flag | ConvertTo-Json))

# Modify run scripts.
$bashScript = [System.IO.File]::ReadAllText($bashScriptPath)
$batchScript = [System.IO.File]::ReadAllText($batchScriptPath)
$scriptRootPath = $PSScriptRoot.Replace("\", "/")
$bashScript = $bashScript.Replace("cd ./", "cd `"$scriptRootPath`"")
$batchScript = $batchScript.Replace("cd ./", "cd `"$scriptRootPath`"")

[System.IO.File]::WriteAllText($bashScriptPath, $bashScript)
[System.IO.File]::WriteAllText($batchScriptPath, $batchScript)

Write-Host "Do you want to change the $serviceName URLs? $confirmPrompt"
if (Read-Confirmation) {
    $defaultHost = "localhost"
    $defaultHttpPort = 5000
    $defaultHttpsPort = 5001

    Write-Host "The default host for $serviceName is $defaultHost."
    Write-Host "Do you want to specify another value for $serviceName host? $confirmPrompt"
    if (Read-Confirmation) {
        Write-Host "Please enter your value for $serviceName host:"
        $userDefinedValue = Read-Host
        $defaultHost = $userDefinedValue
    }

    Write-Host "The default HTTP port for $serviceName is $defaultHttpPort."
    Write-Host "Do you want to specify another value for $serviceName HTTP port? $confirmPrompt"
    if (Read-Confirmation) {
        while ($true) {
            Write-Host "Please enter your value for $serviceName HTTP port (0 - 65535):"
            [int]$userDefinedValue = Read-Host
            if (($userDefinedValue -lt 0) -or ($userDefinedValue -gt 65535)) {
                Write-Warning "Your HTTP port value exceeds the valid range, please specify another value."
                continue
            }
            else {
                $defaultHttpPort = $userDefinedValue
                break
            }
        }
    }

    Write-Host "The default HTTPS port for $serviceName is $defaultHttpsPort."
    Write-Host "Do you want to specify another value for $serviceName HTTPS port? $confirmPrompt"
    if (Read-Confirmation) {
        while ($true) {
            Write-Host "Please enter your value for $serviceName HTTPS port (0 - 65535):"
            [int]$userDefinedValue = Read-Host
            if (($userDefinedValue -lt 0) -or ($userDefinedValue -gt 65535)) {
                Write-Warning "Your HTTPS port value exceeds the valid range, please specify another value."
                continue
            }
            elseif ($userDefinedValue -eq $defaultHttpPort) {
                Write-Warning "Your HTTPS port value conflicts with the HTTP port value, please specify another value."
                continue
            }
            else {
                $defaultHttpsPort = $userDefinedValue
                break
            }
        }
    }

    $envSetter = "`$env:ASPNETCORE_URLS = `"http://$defaultHost`:$defaultHttpPort;https://$defaultHost`:$defaultHttpsPort`""
    $placeholder = "# `$env:ASPNETCORE_URLS = `"http://localhost:5000;https://localhost:5001`""

    # Modify run script PowerShell entry point.
    $psScript = [System.IO.File]::ReadAllText($psScriptPath)
    $psScript = $psScript.Replace($placeholder, $envSetter)

    [System.IO.File]::WriteAllText($psScriptPath, $psScript)
}

Write-Host "Do you want to start $serviceName now? $confirmPrompt"
if (Read-Confirmation) {
    & $psExe "$PSScriptRoot/run.ps1"
}