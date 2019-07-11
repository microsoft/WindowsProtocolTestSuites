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
    * BuildTestSuites (choose this category if you want to build the test suites from source code. To build the test suites of ADFamily/MS-AZOD/MS-ADOD, you need to choose the specific test suite name and run InstallPrerequisites.ps1 again.)
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
    C:\PS>.\InstallPrerequisites.ps1 -Category 'FileServer' -ConfigPath ".\PrerequisitesConfig.xml"
    The PS script will get all tools defined under FileServer node in PrerequisitesConfig.xml, then download and install these tools.
#>

Param
(
    [parameter(Mandatory=$true, ValueFromPipeline=$true, HelpMessage="The Category is used to specify which set of tools need to be downloaded and installed, based on different test suite names, such as FileServer")]
    [String]$Category,
    [parameter(Mandatory=$false, ValueFromPipeline=$true, HelpMessage="The ConfigPath is used to specify prerequisites configure file path")]
    [String]$ConfigPath
)

# ==========================
# Global Variable 
# ==========================

# The path where the Visual Studio will be installed
$VSInstallationPath = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community"

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

# Check if NetFx3 is enabled on current machine.
# .NetFramework 3.5 is required by Wix 3.14  
Function CheckAndInstallNetFx3{
    Write-Host "Checking .NET Framework 3.5"

    $result = get-childitem -path "HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP" | Where-Object -FilterScript {$_.name -match "v3.5"}   

    if($result -ne $null){
        Write-Host ".NET Framework 3.5 is already enabled."
        return $true;
    }
    else{
        Write-Host ".NET Framework 3.5 is unenabled. Enabling this feature now."
        try{

            DISM.EXE /Online /Add-Capability /CapabilityName:NetFx3~~~~
            Add-WindowsCapability –Online -Name NetFx3~~~~
            Write-Host ".NET Framework 3.5 is already enabled."
            return $true
        }
        catch
        {
            Write-Host "Failed to install .Net Framework 3.5."  -ForegroundColor Red
            Write-Host "Please install .NET 3.5 manually after the script finished."  -ForegroundColor Yellow
            return $false
        }
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
		if($AppName -match "Microsoft Agents for Visual Studio")
        {
			#If Test Agent was not installed we also need check if Visual Studio installed.
			$app = Get-ItemProperty $regpath | .{process{if($_.DisplayName -and $_.UninstallString) { $_ } }} | Where-Object {$_.DisplayName -match "Microsoft Visual Studio \d{4} Devenv"} | Sort-Object -Property DisplayVersion -Descending | Select DisplayName, Version, DisplayVersion -First 1
			if($app){
				return $true;
			}
		}
		return $false;
    }
}

# Check if the specified Visual Studio Extension is installed.
Function CheckIfVSExtensionInstalled
{
    Param(
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
    $DllPath = Get-ChildItem -Path $VSInstallationPath\Common7\IDE\Extensions -Filter $DllName -Recurse
    if(-not $DllPath)
    {
        return $false
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
        $tool = '' | select Name,FileName,AppName,DllName,Version,URL,Arguments,InstallFileName,NeedRestart,BackwardCompatible

        $tool.Name = $item.name;
        $tool.FileName = $item.FileName;
        $tool.AppName = $item.AppName;
        $tool.Version = $item.version;
        $tool.URL = $item.url;
        $tool.InstallFileName = $item.InstallFileName;
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
        $ndTool = $tools | Where-Object{$_.Name -eq $item} | Select-Object $_
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

# Download and install prerequisite tool
Function DownloadAndInstallApplication
{
    param(        
        $AppItem,
        [string]$OutputPath
    )

    try       
    {
        Invoke-WebRequest -Uri $item.URL -OutFile $OutputPath                     
    }
    catch
    {
        try
        {                   
            (New-Object System.Net.WebClient).DownloadFile($item.URL, $OutputPath)
        }
        catch
        {          
            Write-host "Download $item.Name failed with exception: $_.Exception.Message"   -ForegroundColor Red
            Return                    
        }                              
    }       
            
    $content = "Downloading " + $AppItem.Name + " completed. Path:" + $OutputPath
    Write-Host $content

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
    $ExitCode = 0
    if ($item.Name.ToLower().Equals("vs2017community"))
    {   
        # Wait untill VS installation is finished, then install the other tools.     
        $ExitCode = (Start-Process -FilePath InstallVs2017Community.cmd -ArgumentList "$OutputPath `"$($VSInstallationPath)`"" -Wait -PassThru).ExitCode  
    }    
    elseif ($item.Name.ToLower().Contains("extension") -and $item.Name.ToLower().Contains("vs"))
    {
        # Install VS extension
        $ExitCode = (Start-Process -FILEPATH "$VSInstallationPath\Common7\IDE\vsixinstaller.exe" -ArgumentList "$OutputPath $FLAGS" -Wait -PassThru).ExitCode
    }    
    else
    {
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
$psVer = [int](Get-Host).Version.ToString().Substring(0,1)

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


#Download and install all required tools
foreach($item in $downloadList)
{
    $isInstalled = $false;
    if ($item.Name.ToLower().Contains("vs") -and $item.Name.ToLower().Contains("extension"))
    {
        $isInstalled = CheckIfVSExtensionInstalled -AppName $item.AppName -DllName $item.DllName
    }
    else
    {
        $isInstalled = CheckIfAppInstalled -AppName $item.AppName -Version $item.version -Compatible $item.BackwardCompatible
    }

    if(-not $isInstalled)
    {
        $content = "Application: " +$item.AppName + " is not installed"
    }
    
    if(-not $IsInstalled)
    {
        Write-Host $content -ForegroundColor Yellow
        
        $content = "Downloading file " + $item.Name + ". Please wait..."
        Write-Host $content
        $outputPath = $tempFolder + "\" + $item.FileName

        if($item.Name.Tolower().contains("wix314"))
        {
            $netfx3Status= CheckAndInstallNetFx3
            if(!$netfx3Status)
            {
                Write-Host ".NET 3.5 cannot be installed successfully. The Wix installation will be skipped."  -ForegroundColor Red
                Write-Host "Please install .NET 3.5 and Wix manually."  -ForegroundColor Red
                continue
            }
        }

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
            Break;
        }

        if($item.NeedRestart)
        {
            $IsNeedRestart = $true;
        }
    }
    else
    {
        if($item.AppName -match "Microsoft Agents for Visual Studio")
        {
            if($item.BackwardCompatible)
            {
                $content = $item.AppName + " or later version or Microsoft Visual Studio is already installed"
            }
            else
            {
                $content = $item.AppName + " or Microsoft Visual Studio is already installed"
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
        }
        Write-Host $content -ForegroundColor Green
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
