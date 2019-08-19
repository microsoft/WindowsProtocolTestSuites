# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Check or install Visual Studio 2017 Community

param (
	[ValidateSet("Check", "Install")]
	[string]$Action,
	[string]$DownloadedArtifact	# Path to the downloaded Visual Studio 2017 Community installer
)

Function Check-VS2017OrLater {

	Write-Host "Checking whether Visual Studio 2017 or later is installed or not..."
	
	if ([IntPtr]::Size -eq 4)  # 32-bit
	{
		$VSWherePath = "${env:ProgramFiles}\Microsoft Visual Studio\Installer\vswhere.exe"
	}
	else # 64-bit
	{
		$VSWherePath = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
	}

	$VSWherePathExisted = Test-Path -Path $VSWherePath

	if ($VSWherePathExisted -eq $false)
	{
		Write-Host "Visual Studio 2017 or later is not installed in your computer."	-ForegroundColor Yellow
		return $false
	}

	$VSDisplayName = cmd /c "`"$VSWherePath`" -latest -format value -property displayname"

	if ($VSDisplayName)
	{
		Write-Host "$VSDisplayName is already installed." 
		return $true
	}
	else
	{
		Write-Host "Visual Studio 2017 or later is not installed in your computer." -ForegroundColor Yellow
		return $false				
	}
}

$currentPath = Split-Path -Parent $MyInvocation.MyCommand.Definition

# The path where the Visual Studio will be installed
$VSInstallationPath = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community"

Function Install-VS2017Community {
	try {
		$ExitCode = (Start-Process -FilePath "$currentPath\InstallVs2017Community.cmd" -ArgumentList "$DownloadedArtifact `"$VSInstallationPath`"" -Wait -PassThru).ExitCode
		if($ExitCode -eq 0) {
			return $true
		}
		else {
			return $false
		}
	}
	catch {
		Write-Host "Install Visual Studio 2017 Community failed: $_" -ForegroundColor Red
		return $false
	}
}

switch($Action) {
	"Check" {
		$isInstalled = Check-VS2017OrLater
		return $isInstalled
	}

	"Install" {
		$result = Install-VS2017Community
		return $result
	}
}
