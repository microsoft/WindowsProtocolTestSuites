# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Check or install NuGet CLI

param (
    [ValidateSet("Check", "Install")]
    [string]$Action,
    [string]$DownloadedArtifact	# Path to the downloaded nuget.exe
)

Function Check-NuGet {
    $pathList = $Env:Path.Split(";")
    foreach($pathItem in $pathList) {
        $ret = Test-Path -PathType Leaf -Path "$pathItem\nuget.exe"
        if($ret) {
            Write-Host "NuGet has already been installed."	
            return $true
        }
    }
    return $false
}

Function Install-NuGet {
    try {
        $ret = Test-Path -PathType Container -Path "$($Env:ALLUSERSPROFILE)\NuGet\"
        if(!$ret) {
            New-Item -Path "$($Env:ALLUSERSPROFILE)\NuGet\" -ItemType Directory
        }
        Copy-Item -Path $DownloadedArtifact -Destination "$($Env:ALLUSERSPROFILE)\NuGet\"
        $machinePath = [Environment]::GetEnvironmentVariable("Path", "Machine")
        [Environment]::SetEnvironmentVariable("Path", "$machinePath;$($Env:ALLUSERSPROFILE)\NuGet", "Machine")
        $Env:Path += ";$($Env:ALLUSERSPROFILE)\NuGet\"
        return $true
    }
    catch {
        Write-Host "Install NuGet failed: $_" -ForegroundColor Red
        return $false
    }
}

switch($Action) {
    "Check" {
        $isInstalled = Check-NuGet
        return $isInstalled
    }

    "Install" {
        $result = Install-NuGet
        return $result
    }
}
