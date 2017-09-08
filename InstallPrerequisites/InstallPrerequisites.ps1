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
    * FileServer
    * Kerberos
    * SMB
    * SMBD
    * RDP
.PARAMETER ConfigPath
    The ConfigPath is used to specify prerequisites configure file path, default value is ".\PrerequisitesConfig.xml".

.EXAMPLE
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

if(-not $ConfigPath)
{
	Write-Host "Use the default value '.\PrerequisitesConfig.xml' as ConfigPath is not set"
	$ConfigPath = ".\PrerequisitesConfig.xml"
}

# Check if NetFx3 is enabled on current machine.
# To understand why .NetFramework 3.5 is needed, please check https://github.com/wixtoolset/issues/issues/3872.  
Function CheckIfNetFx3Enabled{
    $result = Dism /online /Get-FeatureInfo /FeatureName:NetFx3
    $state = $result | Where-Object {$_.ToString().Contains("State")}

    if($state -and $state.Contains("Enabled")){
        return $true;
    }
    else{
        return $false
    }
}

# Check if the required .NET framework version is installed on current machine
Function CheckIfNet4IsInstalled{
    $isInstalled = $false

    if(-not (Test-Path ‘HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full’))
    {
        return $false
    }
    else
    {
        try
        {
            $NetVersion = (Get-ItemProperty ‘HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full’ -Name Version).Version

            if($NetVersion)
            {
                $subVersion = [int]$NetVersion.Substring(0,1)
                if($subVersion -ge 4)
                {
                    $isInstalled = $true
                }
            }
        }
        catch
        {
            $isInstalled = $false
        }
    }
    
    return $isInstalled;
}

# Check if application is installed on current machine.
Function CheckIfAppInstalled{
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
    
    if($app){
		if($Compatible){
            return ([System.Version]$app.DisplayVersion -ge [System.Version]$Version);
		}else{
			return ([System.Version]$app.DisplayVersion -eq [System.Version]$Version);
		}
    }else{
		if($AppName -match "Microsoft Agents for Visual Studio"){
			#If Test Agent was not installed we also need check if Visual Studio installed.
			$app = Get-ItemProperty $regpath | .{process{if($_.DisplayName -and $_.UninstallString) { $_ } }} | Where-Object {$_.DisplayName -match "Microsoft Visual Studio \d{4} Devenv"} | Sort-Object -Property DisplayVersion -Descending | Select DisplayName, Version, DisplayVersion -First 1
			if($app){
				return $true;
			}
		}
		return $false;
    }
}

# Mount ISO and return application path searched from ISO
Function MountISOAndGetAppPath{
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
        retun "";
    }else
    {
        return $appPath.FullName;
    }
}

# Reject app Disk
Function UnmountDisk{
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

    Write-Host "Reading Prerequisites Configure file..."
    [xml]$toolXML = Get-Content -Path $DpConfigPath #".\PrerequisitesConfig.xml"

    # Check if Category exists.
    $CategoryItem = $toolXML.Dependency.$ToolCategory.tool;
    if(-not ($CategoryItem))
    {
        $error = "Category $ToolCategory does not exist";
        throw $error
    }

    $tools = New-Object System.Collections.ArrayList;

    Write-Host "Get information of all the Prerequisite tools from Configure file"
    foreach($item in $toolXML.Dependency.tools.tool)
    {
        $tool = '' | select Name,FileName,AppName,Version,URL,Arguments,InstallFileName,NeedRestart,BackwardCompatible

        $tool.Name = $item.name;
        $tool.FileName = $item.FileName;
        $tool.AppName = $item.AppName;
        $tool.Version = $item.version;
        $tool.URL = $item.url;
        $tool.InstallFileName = $item.InstallFileName;
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
Function CreateTemporaryFolder{
    #create temporary folder for downloading tools
    $tempPath = (get-location).ToString() + "\" + [system.guid]::newguid().ToString()
    Write-Host "Create temporary folder for downloading files"
    $outFile = New-Item -ItemType Directory -Path $tempPath
    Write-Host "Temporary folder $outFile is created"

    return $outFile.FullName
}

# Download and install prerequisite tool
Function DownloadAndInstallApplication
{
    param(
        [int]$PSVersion,
        $AppItem,
        [string]$OutputPath
    )
    # Check if Powershell version greate than 3.0, if not then use WebClient to download file, otherwise use Invoke-WebRequest.
    if($psVersion -ge 3)
    {
        Invoke-WebRequest -Uri $AppItem.URL -OutFile $OutputPath
    }else
    {
        (New-Object System.Net.WebClient).DownloadFile($AppItem.URL, $OutputPath)
    }
            
    $content = "Downloading " + $AppItem.Name + " completed. Path:" + $OutputPath
    Write-Host $content

    # Check if the downloaded file is ISO
    if($AppItem.FileName.ToLower().EndsWith("iso"))
    {
        if($psVersion -ge 3)
        {
            Write-Host "Mounting ISO image";
            $OutputPath = MountISOAndGetAppPath -AppName $AppItem.InstallFileName -ISOPath $OutputPath
            Write-Host $OutputPath
        }
        else
        {
            $content = "Your system does not support Mount-DiskImage command. Please install " + $AppItem.AppName;

            Write-Host "Your system does not support Mount-DiskImage command. Please mount and install manually";
        }
    }
            
    # start to Install file
    $content = "Installing " + $AppItem.Name + ". Please wait..."
    Write-Host $content

    $FLAGS  = $AppItem.Arguments

    $ExitCode = (Start-Process -FILEPATH $OutputPath $FLAGS -Wait -PassThru).ExitCode
    if ($ExitCode -EQ 0)
    {
        $content = "Application " + $AppItem.Name +" is successfully installed on current machine"
        Write-Host $content -ForegroundColor Green
    }
    else
    {
        $failedList += $AppItem.Name
        $content = "Installing " + $AppItem.Name +" failed, Error Code:" + $ExitCode
        Write-Host "ERROR $ExitCode"; 
    }

    # If the file is ISO, unmount it.
    if($AppItem.FileName.ToLower().EndsWith("iso"))
    {
        UnmountDisk -AppPath $OutputPath
    }
}

# Start get all needed tools from configure file.
$downloadList = GetDownloadTools -DpConfigPath $ConfigPath -ToolCategory $Category
$tempFolder = CreateTemporaryFolder
$failedList = @();
$IsNeedRestart = $false;

# Check PowerShell version

$psVer = [int](Get-Host).Version.ToString().Substring(0,1)

# Check if .NET Framework 3.5 feature is enabled.
Write-Host "Check if .NET Framework 3.5 feature is enabled"
$net35Enabled = CheckIfNetFx3Enabled

if(-not $net35Enabled)
{
    Write-Host ".NET Framework 3.5 feature is not enabled" -ForegroundColor Yellow
    Write-Host "Starting enable .NET Framework 3.5"
    DISM /Online /Enable-Feature /FeatureName:NetFx3 /All
}
else
{
    Write-Host ".NET Framework 3.5 feature is enabled" -ForegroundColor Green
}

foreach($item in $downloadList)
{
    $isInstalled = $false;

    if($item.Name.ToLower().Equals("net40"))
    {
        $isInstalled = CheckIfNet4IsInstalled

        if(-not $isInstalled)
        {
            $content = ".NET Framework 4.0 is not installed"
        }
    }
    else
    {
        $isInstalled = CheckIfAppInstalled -AppName $item.AppName -Version $item.version -Compatible $item.BackwardCompatible
        if(-not $isInstalled)
        {
            $content = "Application: " +$item.AppName + " is not installed"
        }
    }

    if(-not $IsInstalled)
    {
        Write-Host $content -ForegroundColor Yellow
        
        $content = "Downloading file " + $item.Name + ". Please wait..."
        Write-Host $content
        $outputPath = $tempFolder + "\" + $item.FileName

        try
        {
            DownloadAndInstallApplication -PSVersion $psVer -AppItem $item -OutputPath $outputPath
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
    else{
		if($item.AppName -match "Microsoft Agents for Visual Studio"){
			if($item.BackwardCompatible){
				$content = $item.AppName + " or later version or Microsoft Visual Studio is already installed"
			}else{
				$content = $item.AppName + " or Microsoft Visual Studio is already installed"
			}
		}else{
			if($item.BackwardCompatible){
				$content = $item.AppName + " or later version is already installed"
			}else{
				$content = $item.AppName + " is already installed"
			}
		}
        
        Write-Host $content -ForegroundColor Green
    }
}

if($psVersion -ge 3)
{
    $downloadList.Clear();
}
else{
    $downloadList = @();
}

# Check if .NET Framework 3.5 is enabled after installation.
if(-not $net35Enabled)
{
    # check if .NET 3.5 is installed.
    $net35Enabled = CheckIfNetFx3Enabled
}

if(($failedList.Length -eq 0) -and ($net35Enabled)) #No failure occurs and .NETFramework 3.5 is enabled.
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
    if(-not $net35Enabled)
    {
        Write-Host "Installation of .NET Framework 3.5 failed. Please enable it manually." -ForegroundColor Red
    }

    if($failedList.Length -gt 0)
    {
        Write-Host "The following prerequisite tools are not installed. Please install them manually."
        Write-Host "You can get the setup files under "$tempFolder

        $failedList|ForEach-Object{
            Write-Host $_ -ForegroundColor Red
        }
    }
}