# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
	Install prerequisite tools
.DESCRIPTION
	This Powershell script is used to download and install prerequisite software dependencies for test suites. 
	It contains one parameter "Category", which is the name of test suite and you can find it in PrerequisitesConfig.xml.
.PARAMETER Category
	The Category is used to specify which set of tools need to be downloaded and installed, based on different test suite names, such as "FileServer", Categories are defined in PrerequisitesConfig.xml
	and you can update this configure file to achieve your requirement.
	Currently we support the following Categories:
	* BuildTestSuites (choose this category if you want to build the test suites/PTM from source code. To build the test suites of ADFamily/MS-AZOD/MS-ADOD, you need to choose the specific test suite name and run InstallPrerequisites.ps1 again.)
	* FileServer
	* Kerberos
	* SMB
	* SMBD
	* RDP
	* BranchCache
	* ADFamily
	* AZOD
	* ADFSPIP
	* ADOD
.PARAMETER ConfigPath
	The ConfigPath is used to specify prerequisites configure file path, default value is ".\PrerequisitesConfig.xml".

.EXAMPLE
	When you want to run the File Server test suite, type the command below:
	C:\PS>.\InstallPrerequisites.ps1 -Category FileServer -ConfigPath ".\PrerequisitesConfig.xml"
	The PS script will get all tools defined under FileServer node in PrerequisitesConfig.xml, then download and install these tools.
#>

Param
(
	[parameter(Mandatory=$true, ValueFromPipeline=$true, HelpMessage="If you want to run a specific test suite, please input the test suite name as Category, such as FileServer. If you want to build test suites or PTM, input BuildTestSuites as Category")]
	[String]$Category,
	[parameter(Mandatory=$false, ValueFromPipeline=$true, HelpMessage="The ConfigPath is used to specify prerequisites configure file path")]
	[String]$ConfigPath
)

Function CheckInternetConnection{
  
	Try
	{
		$status = Test-Connection -ComputerName "www.microsoft.com" -count 5 -Quiet
		return ($status -ne $false) -and ($status -ne $null)
	}
	Catch
	{
		return $false
	}
}

Function CheckCategory{

	Write-Host "Reading Prerequisites Configure file..."
	[xml]$toolXML = Get-Content -Path $ConfigPath 

	# Check if Category exists.
	$CategoryItem = $toolXML.Dependency.$Category.tool;
	if(-not ($CategoryItem))
	{
		$error = "Category $Category is not acceptable.";
		Write-Host $error  -ForegroundColor Red
		exit
	}
}


# Check if application is installed on current machine.
Function CheckIfAppInstalled
{
	Param (
		[string]$AppName,	# Application Name
		[string]$Version,	# Application Version
		[bool]$Compatible	# Is support backward compatible
	)

	#check if the required software is installed on current machine
	if ([IntPtr]::Size -eq 4) {
		$regpath = 'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*'
	}
	else {
		$regpath = @(
			'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*'
			'HKLM:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*'
		)
	}
	
	$app = Get-ItemProperty $regpath | .{process{if($_.DisplayName -and $_.UninstallString) { $_ } }} | Where-Object {$_.DisplayName -match $AppName} | Select DisplayName, DisplayVersion -First 1
	
	if($app)
	{
		if($Compatible)
		{
			return ([System.Version]$app.DisplayVersion -ge [System.Version]$Version);
		}
		else
		{
			return ([System.Version]$app.DisplayVersion -eq [System.Version]$Version);
		}
	}
	else
	{
		return $false;
	}
}

# Check if the specified Visual Studio Extension is installed.
Function CheckIfVSExtensionInstalled
{
	Param(
		$VSInstallationPaths,
		[string]$AppName, # App name
		[string]$DllName  # The Dll name of the extension. It can be found in the .vsix file if you change it to .zip and unzip it.
	)

	# The DllName should not be empty, otherwise we cannot check if the extension is installed or not.
	if ($DllName -eq $null -or $DllName -eq "")
	{
		Write-Host "DllName of $AppName in PrerequisitesConfig.xml should not be empty if the tool is a VS extension." -ForegroundColor Yellow
		return $false # Install the extension anyway
	}

	# Search the dll name in the extension folder of VS installation path.
	# If the dll name can be found, then the extension is installed.  

	foreach ($path in $VSInstallationPaths)
	{
		if (($AppName -match "2017" -and $path -match "2017") -or ($AppName -match "2019" -and $path -match "2019"))
		{
			$DllPath = Get-ChildItem -Path $path\Common7\IDE\Extensions -Filter $DllName -Recurse
			if(-not $DllPath)
			{
				Write-Host "$AppName is not installed in $path" -ForegroundColor Yellow
				return $false
			}
			else 
			{
				Write-Host "$AppName is already installed in $path"	
			}
		}
	}

	return $true
}

# Mount ISO and return application path searched from ISO
Function MountISOAndGetAppPath
{
	Param (
		[string]$AppName,
		[string]$ISOPath
	   )
	# Mount ISO and get drive letter
	$iso = Mount-DiskImage -ImagePath $ISOPath -StorageType ISO -PassThru
	$driveLetter = ($iso | Get-Volume).DriveLetter

	# Find application in ISO
	$driveLetter = $driveLetter + ":"

	$appPath = Get-ChildItem -Path $driveLetter -Filter $AppName -Recurse
	if(-not $appPath)
	{
		$content = $AppName + "cannot be found in ISO"
		Write-Host $content -ForegroundColor Red
		return "";
	}
	else
	{
		return $appPath.FullName;
	}
}

# Reject app Disk
Function UnmountDisk
{
	Param (
		[string]$AppPath
	   )

	$DriveLetter = (Get-Item $appPath).PSDrive.Name
	$ShellApplication = New-Object -ComObject Shell.Application
	Write-Host "Eject DVD Drive: "$DriveLetter
	$ShellApplication.Namespace(17).ParseName($DriveLetter+":").InvokeVerb("Eject")
}

# Get tools to be downloaded from Config file by Category
Function GetDownloadTools{
	Param(
		[string]$DpConfigPath,
		[string]$ToolCategory
	)
	   
	[xml]$toolXML = Get-Content -Path $DpConfigPath   

	$tools = New-Object System.Collections.ArrayList;

	Write-Host "Get information of all the Prerequisite tools from Configure file"
	foreach($item in $toolXML.Dependency.tools.tool)
	{
		$tool = '' | select type,Name,command,FileName,AppName,DllName,Version,URL,Arguments,InstallFileName,NeedRestart,BackwardCompatible

		$tool.Type = $item.type
		$tool.Name = $item.name
		$tool.Command = $item.command
		$tool.FileName = $item.FileName
		$tool.AppName = $item.AppName
		$tool.Version = $item.version
		$tool.URL = $item.url
		$tool.InstallFileName = $item.InstallFileName
		$tool.DllName = $item.DllName
		
		$tool.NeedRestart = $false
		$tool.BackwardCompatible = $true
		
		if($item.NeedRestart)
		{
			$tool.NeedRestart = [bool]$item.NeedRestart;
		}
		if($item.BackwardCompatible)
		{
			$tool.BackwardCompatible = [bool]$item.BackwardCompatible;
		}
		$tool.Arguments = $item.arguments;

		$index = $tools.Add($tool)
	}

	Write-Host "Get the tools to be downloaded from the specified category"
	$downloadList = New-Object System.Collections.ArrayList;
	foreach($item in $toolXML.Dependency.$ToolCategory.tool)
	{
		$ndTool = $tools | Where-Object{$_.Name -eq $item}
		$index = $downloadList.Add($ndTool)
	}

	$tools.Clear();
	return $downloadList;
}

# Create a tempoary folder under current folder, which is used to store downloaded files.
Function CreateTemporaryFolder
{
	#create temporary folder for downloading tools
	$tempPath = (get-location).ToString() + "\" + [system.guid]::newguid().ToString()
	Write-Host "Create temporary folder for downloading files"``
	$outFile = New-Item -ItemType Directory -Path $tempPath
	Write-Host "Temporary folder $outFile is created"

	return $outFile.FullName
}

# Download application
Function DownloadApplication
{
	param(
		$AppItem,
		[string]$OutputPath
	)

	try
	{
		Invoke-WebRequest -Uri $AppItem.URL -OutFile $OutputPath
	}
	catch
	{
		try
		{
			(New-Object System.Net.WebClient).DownloadFile($AppItem.URL, $OutputPath)
		}
		catch
		{
			Write-host "Download $item.Name failed with exception: $_.Exception.Message"   -ForegroundColor Red
			return
		}
	}

	$content = "Downloading " + $AppItem.Name + " completed. Path:" + $OutputPath
	Write-Host $content
}

# Download and install prerequisite application
Function DownloadAndInstallApplication
{
	param(
		$AppItem,
		[string]$OutputPath
	)

	DownloadApplication -AppItem $AppItem -OutputPath $OutputPath


	# Check if the downloaded file is ISO
	if($AppItem.FileName.ToLower().EndsWith("iso"))
	{
		Write-Host "Mounting ISO image";
		$OutputPath = MountISOAndGetAppPath -AppName $AppItem.InstallFileName -ISOPath $OutputPath
		Write-Host $OutputPath        
	}    
			
	# start to Install file
	
	$content = "Installing " + $AppItem.Name + ". Please wait..."
	Write-Host $content

	$FLAGS  = $AppItem.Arguments
	$ExitCode = (Start-Process -FILEPATH "$env:systemroot\system32\msiexec.exe" -ArgumentList "/i $OutputPath $FLAGS /passive" -Wait -PassThru).ExitCode
	if ($ExitCode -NE 0)
	{
		$ExitCode = (Start-Process -FILEPATH $OutputPath $FLAGS -Wait -PassThru).ExitCode
	}

	# If the file is ISO, unmount it.
	if($AppItem.FileName.ToLower().EndsWith("iso"))
	{
		UnmountDisk -AppPath $OutputPath
	}

	if ($ExitCode -EQ 0)
	{
		$content = $AppItem.Name +" is successfully installed on current machine"
		Write-Host $content -ForegroundColor Green
	}
	else
	{
		$failedList += $AppItem.Name
		$content = "Install " + $AppItem.Name +" failed, Error Code:" + $ExitCode
		Write-Host "ERROR $ExitCode"; 
	}    
}

# Find the Visual Studio installation path of a specific version
Function FindSpecificVersionOfVisualStudio
{
	param(
		$Version,
		$VSInstallationPaths
	)

	[string[]]$VSPath = $VSInstallationPaths | Where-Object{$_ -match $Version}

	# VS extension can be installed on all the same versions of the Visual studio ( For example, 2019 Enterprise, 2019 Professional ) at one time. 		
	# So only one path is enough.
	return $VSPath[0]
}

# Download and install visual studio extension
Function DownloadAndInstallVsExtension
{
	param(
		$VSInstallationPaths,
		$AppItem,
		[string]$OutputPath
	)

	DownloadApplication -AppItem $AppItem -OutputPath $OutputPath

	# start to Install file
	
	$content = "Installing " + $AppItem.Name + ". Please wait..."
	Write-Host $content

	$FLAGS  = $AppItem.Arguments

	if ($AppItem -match "2017")
	{
		$path = FindSpecificVersionOfVisualStudio -Version 2017 -VSInstallationPaths $VSInstallationPaths
	}
	elseif ($AppItem -match "2019") 
	{
		$path = FindSpecificVersionOfVisualStudio -Version 2019 -VSInstallationPaths $VSInstallationPaths	
	}
	else
	{
		$failedList += $AppItem.Name
		$content = "Install " + $AppItem.Name +" failed, we only support install Visual Studio 2017 or 2019 extensions now."
		Write-Host "ERROR $content"; 
	}

	$ExitCode = (Start-Process -FILEPATH "$path\Common7\IDE\vsixinstaller.exe" -ArgumentList "$OutputPath $FLAGS" -Wait -PassThru).ExitCode

	if ($ExitCode -NE 0)
	{
		$ExitCode = (Start-Process -FILEPATH $OutputPath $FLAGS -Wait -PassThru).ExitCode
	}

	if ($ExitCode -EQ 0)
	{
		$content = $AppItem.Name +" is successfully installed on current machine"
		Write-Host $content -ForegroundColor Green
	}
	else
	{
		$failedList += $AppItem.Name
		$content = "Install " + $AppItem.Name +" failed, Error Code:" + $ExitCode
		Write-Host "ERROR $content"; 
	}
}

# Get the installation path of Visual Studio
Function GetVSInstallationPaths
{
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
		Write-Host "$VSWherePath is not found."	-ForegroundColor Yellow
		return $null
	}

	$VSInstallationPaths = cmd /c "`"$VSWherePath`" -format value -property installationPath"
	if ($VSInstallationPaths)
	{
		Write-Host "Installation path of Visual Studio is $VSInstallationPaths." 
		return $VSInstallationPaths			
	}
	else
	{
		Write-Host "Did not find installation path of Visual Studio." -ForegroundColor Yellow
		return $null				
	}
}


# ================================
# Script starts here
# ================================

#Check the internet connection before run the scripts
$internetConnection = CheckInternetConnection

if($internetConnection -eq $false)
{
	Write-Host "The script requires the computer connected to internet." -ForegroundColor Red
	Write-Host "Your comuter is not connected to internet."  -ForegroundColor Yellow
	Write-Host "Please check your network connection before executing the script."  -ForegroundColor Yellow
	exit
}

#Check input parameter
if(-not $ConfigPath)
{
	Write-Host "Use the default value '.\PrerequisitesConfig.xml' as ConfigPath is not set."
	$ConfigPath = ".\PrerequisitesConfig.xml"
}

CheckCategory

#Check PS version
$psVer = [int] $PSVersionTable.PSVersion.Major

if($psVer -lt 4)
{
	Write-Host "Powershell 4 or later is required for Github downloading." -ForegroundColor Red 
	Write-Host "Please install WMF 4.0 from https://www.microsoft.com/en-us/download/details.aspx?id=40855 before running this script."  -ForegroundColor Yellow
	exit
}

#Check OS version

[double] $OsVer = [System.Environment]::OSVersion.Version.Build

if($OsVer -lt "7601")
{
	Write-Host "Windows 7 SP1 and later is required for running this script." -ForegroundColor Red
	exit
}


$AllProtocols = [System.Net.SecurityProtocolType]'Ssl3,Tls,Tls11,Tls12'
[System.Net.ServicePointManager]::SecurityProtocol = $AllProtocols

# Download all required tools from configure file.
$downloadList = GetDownloadTools -DpConfigPath $ConfigPath -ToolCategory $Category
$tempFolder = CreateTemporaryFolder
$failedList = @();
$IsNeedRestart = $false;

$currentPath = Split-Path -Parent $MyInvocation.MyCommand.Definition

#Download and install all required tools
foreach($item in $downloadList)
{
	switch($item.Type) {
		"Custom" {
			$isInstalled = & "$currentPath\$($item.Command)" -Action Check
			if($isInstalled) {
				continue
			}

			$outputPath = $null

			if(-not [string]::IsNullOrEmpty($item.url)) {
				# Download artifact
				$outputPath = $tempFolder + "\" + $item.FileName
				DownloadApplication -AppItem $item -OutputPath $outputPath

				$result = & "$currentPath\$($item.Command)" -Action Install -DownloadedArtifact $outputPath
			}
			else {
				# No need to download artifact
				$result = & "$currentPath\$($item.Command)" -Action Install
			}

			if($result) {
				Write-Host "$($item.Name) is installed successfully." -ForegroundColor Green
			}
			else 
			{
				$failedList += $item.Name
				Write-Host "Failed to install $($item.Name)!" -ForegroundColor Red
				break
			}
		}
		"Installer" {
			$isInstalled = CheckIfAppInstalled -AppName $item.AppName -Version $item.version -Compatible $item.BackwardCompatible

			if(-not $isInstalled)
			{
				$content = "Application: " +$item.AppName + " is not installed"

				Write-Host $content -ForegroundColor Yellow
		
				$content = "Downloading file " + $item.Name + ". Please wait..."
				Write-Host $content
				$outputPath = $tempFolder + "\" + $item.FileName

				try
				{
					DownloadAndInstallApplication -AppItem $item -OutputPath $outputPath
				}
				catch
				{
					$failedList += $item.Name
					$IsInstalled = $false;
					$ErrorMessage = $_.Exception.Message
					Write-Host $ErrorMessage -ForegroundColor Red
					Break
				}

				if($item.NeedRestart)
				{
					$IsNeedRestart = $true;
				}
			}
			else
			{
				if($item.BackwardCompatible)
				{
					$content = $item.AppName + " or later version is already installed"
				}
				else
				{
					$content = $item.AppName + " is already installed"
				}
				Write-Host $content
			}
		}
		"VsExtension" {
			# Get VS installation path
			if (-not $VSInstallationPaths)
			{
				$VSInstallationPaths = GetVSInstallationPaths
			}

			if (-not $VSInstallationPaths)
			{
				Write-Host "VS Extension cannot be installed" -ForegroundColor red
				$failedList += $item.Name
				break
			}

			$isInstalled = CheckIfVSExtensionInstalled -VSInstallationPaths $VSInstallationPaths -AppName $item.AppName -DllName $item.DllName

			if(-not $isInstalled)
			{
				$content = "VsExtension: " +$item.AppName + " is not installed"

				Write-Host $content -ForegroundColor Yellow
		
				$content = "Downloading file " + $item.Name + ". Please wait..."
				Write-Host $content
				$outputPath = $tempFolder + "\" + $item.FileName


				try
				{
					DownloadAndInstallVsExtension -VSInstallationPaths $VSInstallationPaths -AppItem $item -OutputPath $outputPath
				}
				catch
				{
					$failedList += $item.Name
					$IsInstalled = $false
					$ErrorMessage = $_.Exception.Message
					Write-Host $ErrorMessage -ForegroundColor Red
					Break
				}

				if($item.NeedRestart)
				{
					$IsNeedRestart = $true
				}
			}
		}
	}

	
}

$downloadList.Clear();

if($failedList.Length -eq 0) #No failure occurs
{
	if(Test-Path $tempFolder)
	{
		Write-Host "Remove temporary folder"
		Remove-Item -Path $tempFolder -Recurse -Force
	}
	Write-Host "Prerequisite tools are all installed." -ForegroundColor Green
	if($IsNeedRestart)
	{
		Write-Host "Please restart your computer before build the test suites." -ForegroundColor Yellow
	}
}
else
{
	if($failedList.Length -gt 0)
	{
		Write-Host "The following prerequisite tools are not installed. Please install them manually."
		Write-Host "You can get the setup files under "$tempFolder

		$failedList|ForEach-Object{
			Write-Host $_ -ForegroundColor Red
		}
	}
}
