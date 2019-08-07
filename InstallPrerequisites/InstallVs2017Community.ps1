# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Check or install Visual Studio 2017 Community

param (
	[ValidateSet("Check", "Install")]
	[string]$Action,
	[string]$DownloadedArtifact	# Path to the downloaded Visual Studio 2017 Community installer
)

Function Check-VS2017Community {

	Write-Host "Checking whether Visual Studio 2017 Community is installed or not..."

	if ([IntPtr]::Size -eq 4) {
		$regpath = 'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*'
	}
	else {
		$regpath = @(
			'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*'
			'HKLM:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*'
		)
	}
	
	$app = Get-ItemProperty $regpath | .{process{if($_.DisplayName -and $_.UninstallString) { $_ } }} | Where-Object {$_.DisplayName -match "Visual Studio Community 2017"} | Select DisplayName, DisplayVersion -First 1
	
	if($app)
	{
		return $true
	}
	else
	{
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
		$isInstalled = Check-VS2017Community
		return $isInstalled
	}

	"Install" {
		$result = Install-VS2017Community
		return $result
	}
}
