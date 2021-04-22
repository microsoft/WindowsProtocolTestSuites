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

$backupPath = "$PSScriptRoot/.bak"
$appSettingsBackupPath = "$backupPath/appsettings.json"
$databaseBackupPath = "$backupPath/ptmservice.db"
$bashScriptBackupPath = "$backupPath/run.sh"
$batchScriptBackupPath = "$backupPath/run.bat"

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
$testSuitePoolPath = "$storagePath/testsuite"
$configurationPoolPath = "$storagePath/configuration"
$testResultPoolPath = "$storagePath/testresult"

mkdir $storagePath
Copy-Item $databasePath $storageDatabasePath

function Read-UserDefinedValue {
    param (
        [string]$Name,
        [string]$DefaultValue,
        [bool]$IsKnownStorageNodeValue = $true
    )

    if ($IsKnownStorageNodeValue) {
        Write-Host "The default value for known storage node $Name is $DefaultValue."
    }
    else {
        Write-Host "The default value for database path $Name is $DefaultValue."
    }
    Write-Host "Do you want to specify another $Name value? $denyPrompt"
    if (Read-Confirmation -IsYesDefault $false) {
        $destExists = $false
        while (-not $destExists) {
            Write-Host "Please enter your value for $Name`:"
            $userDefinedValue = Read-Host
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

$storageDatabasePath = Read-UserDefinedValue -Name "Data Source" -DefaultValue $storageDatabasePath -IsKnownStorageNodeValue $false

$testSuitePoolPath = Read-UserDefinedValue -Name "testsuite" -DefaultValue $testSuitePoolPath
$configurationPoolPath = Read-UserDefinedValue -Name "configuration" -DefaultValue $configurationPoolPath
$testResultPoolPath = Read-UserDefinedValue -Name "testresult" -DefaultValue $testResultPoolPath

$appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
$appSettings.ConnectionStrings.Database = "Data Source = $storageDatabasePath"
for ($idx = 0; $idx -lt $appSettings.KnownStorageNodes.Length; $idx++) {
    switch ($appSettings.KnownStorageNodes[$idx].Name) {
        "testsuite" {
            $appSettings.KnownStorageNodes[$idx].Path = $testSuitePoolPath
        }
        "configuration" {
            $appSettings.KnownStorageNodes[$idx].Path = $configurationPoolPath
        }
        "testresult" {
            $appSettings.KnownStorageNodes[$idx].Path = $testResultPoolPath
        }
    }
}

[System.IO.File]::WriteAllText($appSettingsPath, ($appSettings | ConvertTo-Json))

if (-not $IsLinux) {
    Write-Host "Do you want to create a desktop top shortcut to $ServiceName`? $confirmPrompt"
    if (Read-Confirmation) {
        $wsh = New-Object -ComObject WScript.Shell
        $shortcut = $wsh.CreateShortcut("$Home/Desktop/PTMService.lnk")
        $shortcut.TargetPath = "$PSScriptRoot/run.bat"
        $shortcut.Save()
    }
}

$flag = @{
    installationId        = $installationId
    storageDatabasePath   = $storageDatabasePath
    testSuitePoolPath     = $testSuitePoolPath
    configurationPoolPath = $configurationPoolPath
    testResultPoolPath    = $testResultPoolPath
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

Write-Host "Do you want to start $ServiceName now? $confirmPrompt"
if (Read-Confirmation) {
    & $psExe "$PSScriptRoot/run.ps1"
}