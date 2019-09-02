# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Check or install Visual Studio 2017 Community

param (
	[ValidateSet("Check", "Install")]
	[string]$Action,
	[string]$DownloadedArtifact	# Path to the downloaded Visual Studio 2017 Community installer
)

Function CheckAndModify-VS2017OrLater {

	Write-Host "Checking whether Visual Studio 2017 or later is installed or not..."
	
	if ([IntPtr]::Size -eq 4)  # 32-bit
	{
		$VSWherePath = "${env:ProgramFiles}\Microsoft Visual Studio\Installer\vswhere.exe"
		$VsInstallerPath = "${env:ProgramFiles}\Microsoft Visual Studio\Installer\vs_installer.exe"
	}
	else # 64-bit
	{
		$VSWherePath = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
		$VsInstallerPath = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vs_installer.exe"
	}

	$VSWherePathExisted = Test-Path -Path $VSWherePath

	if ($VSWherePathExisted -eq $false)
	{
		Write-Host "Visual Studio 2017 or later is not installed in your computer."	-ForegroundColor Yellow
		return $false
	}

	[string[]]$VSInstallationPaths = cmd /c "`"$VSWherePath`" -format value -property installationpath"
	[string[]]$VSDisplayNames = cmd /c "`"$VSWherePath`" -format value -property displayname"

	if ($VSInstallationPaths -eq $null)
	{
		Write-Host "Visual Studio 2017 or later is not installed in your computer." -ForegroundColor Yellow
		return $false			
	}

	for($i = 0; $i -lt $VSInstallationPaths.Count; $i++)
	{
		$VSDisplayName = $VSDisplayNames[$i]
		Write-Host "$VSDisplayName is already installed. Modifying the components..." 
		$VSInstallationPath = $VSInstallationPaths[$i]
		$ExitCode = (Start-Process -FilePath "$currentPath\InstallVisualStudio.cmd" -ArgumentList "`"$VsInstallerPath`" modify `"$VSInstallationPath`"" -Wait -PassThru).ExitCode	
		if($ExitCode -eq 0) {
			Write-Host "The necessary Visual Studio components are installed successfully." -ForegroundColor Green
		}
		else 
		{
			Write-Host "Failed to install the necessary components for $VSDisplayName. ExitCode is $ExitCode" -ForegroundColor Yellow
		}	
	}

	return $true
}

$currentPath = Split-Path -Parent $MyInvocation.MyCommand.Definition

# The path where the Visual Studio will be installed
$VSInstallationPath = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community"

Function Install-VisualStudio {
	try {
		$ExitCode = (Start-Process -FilePath "$currentPath\InstallVisualStudio.cmd" -ArgumentList "$DownloadedArtifact  `"`"  `"$VSInstallationPath`"" -Wait -PassThru).ExitCode
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
		$isInstalled = CheckAndModify-VS2017OrLater
		return $isInstalled
	}

	"Install" {
		$result = Install-VisualStudio
		return $result
	}
}
