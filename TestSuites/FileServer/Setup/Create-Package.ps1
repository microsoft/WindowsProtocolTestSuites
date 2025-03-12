# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param
(
    [Parameter(Mandatory = $true)]
    [ValidateSet("Workgroup", "Domain", "Cluster")]
    [string]$Scenario,
    [Parameter(Mandatory = $true)]
    [string]$ConfigPath
)

$sourceFolders = @(".\Scripts")
$zipFilePath = ".\Packages\$Scenario-Package.zip"
$pullConfigs = $false

if (Test-Path $zipFilePath) {
    Remove-Item $zipFilePath
}

if (-not(Test-Path (Split-Path -Path $zipFilePath -Parent))) {
    New-Item -Path (Split-Path -Path $zipFilePath -Parent) -ItemType Directory -Force > $null
}

switch ($Scenario) {
    "Workgroup" {
        $sourceFolders += ".\Workgroup"
        $pullConfigs = $false
    }
    "Domain" {
        $sourceFolders += ".\Domain"
        $pullConfigs = $true
    }
    "Cluster" {
        $sourceFolders += ".\Cluster"
        $pullConfigs = $true
    }
}

if (-not(. ".\$Scenario\Validate-ConfigFile.ps1" -ConfigPath $ConfigPath)) {
    Write-Output "Invalid Configuration File provided"
    exit 1
}

# Create a new zip file
$scripts = $sourceFolders[0]
Compress-Archive -Path $scripts\* -DestinationPath $zipFilePath -Force

# Add the contents of the second folder to the zip file
foreach ($folder in $sourceFolders[1..($sourceFolders.Length - 1)]) {
    Compress-Archive -Path $folder\* -Update -DestinationPath $zipFilePath
}

if ($pullConfigs) {
    Write-Output "Pulling Configs"
    try {
        $configs = ".\Configs"
        if (-not (Test-Path -Path $configs)) {
            New-Item -ItemType Directory -Force -Path $configs | Out-Null
        }
        Start-BitsTransfer -Source "https://ptsresources.z13.web.core.windows.net/Configs/ParamConfig.json" -Destination "$configs\ParamConfig.json"
        Compress-Archive -Path "$configs\*" -Update -DestinationPath $zipFilePath
        Remove-Item -Path $configs -Recurse -Force
    }
    catch {
        Write-Output "Failed to pull Configs: $_"
    }
}

Compress-Archive -Path $ConfigPath -Update -DestinationPath $zipFilePath

Write-Output "Package created at $zipFilePath"