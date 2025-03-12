# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Update-VisualStudioLicense.ps1
## Purpose:        Update vs license file to vm in the process of setup.
## Version:        1.0 (08 Apr, 2019)
##
##############################################################################

Param
(
    [string]$WorkingPath = "C:\temp\Scripts"                  # Script working path
)

$ScriptFileFullPath      = $MyInvocation.MyCommand.Definition
$LogFileFullPath         = "$ScriptFileFullPath.log"


#==========================================================================================
# Function Definition
#==========================================================================================


#------------------------------------------------------------------------------------------
# Write a piece of information to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteInfo {
    Param(
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message,
    [string]$ForegroundColor = "White",
    [string]$BackgroundColor = "DarkBlue")

    # WinBlue issue: Start-Transcript cannot write the log printed out by Write-Host, as a workaround, use Write-output instead
    # Write-Output does not support color
    if ([Double]$Script:HostOsBuildNumber -eq [Double]"6.3") {
        ((Get-Date).ToString() + ": $Message") | Out-Host
    }
    else {
        Write-Host ((Get-Date).ToString() + ": $Message") -ForegroundColor $ForegroundColor -BackgroundColor $BackgroundColor
    }
}


#------------------------------------------------------------------------------------------
# Write a piece of error message to the screen
#------------------------------------------------------------------------------------------
Function Write-TestSuiteError {
    Param (
    [Parameter(ValueFromPipeline=$True)]
    [string]$Message,
    [switch]$Exit)

    Write-TestSuiteInfo -Message "[ERROR]: $Message" -ForegroundColor Red -BackgroundColor Black
    if ($Exit) {exit 1}
}


#------------------------------------------------------------------------------------------
# Extract zip file to destination path
#------------------------------------------------------------------------------------------
Function Extract-ZipFile {
    Param (
    [Parameter(Mandatory=$true)]
    [string]$ZipFile,
    [Parameter(Mandatory=$true)]
    [string]$Destination
    )
    # check dotnet version
    if($PSVersionTable.CLRVersion.Major -ge 4){
	[System.Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem")
	[System.IO.Compression.ZipFile]::ExtractToDirectory($ZipFile, $Destination)
    }else
    {
	$shell = New-Object -com shell.application
	$zip = $shell.NameSpace($ZipFile)
	if(-not (Test-Path -Path $Destination))
	{
    New-Item -ItemType directory -Path $Destination 
	}
	$shell.Namespace($Destination).CopyHere($zip.items(), 0x14)
    }
}


#------------------------------------------------------------------------------------------
# Check whether visual studio installed or not
#------------------------------------------------------------------------------------------
Function Check-VisualStudioInstalled {
    # Local Machine
    $Hklm="HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
    # Current User
    $Hkcu="HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall"
    # Win64 System 32 bit software
    $Win6432Node="HKLM:SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
    $UninstallPaths  = @(,$Hklm,$Hkcu)
    if([Environment]::Is64BitOperatingSystem)
    {
    $UninstallPaths  = @(,$Win6432Node,$Hklm,$Hkcu)
    }
    foreach($uninstall in $UninstallPaths){
    if(Test-Path $uninstall){
    $items=Get-ChildItem $uninstall
    foreach($item in $items){
    $DisplayName=$item.GetValue("DisplayName")
    if(($DisplayName -ne $null) -and (![string]::IsNullOrEmpty($DisplayName.Trim()))){
    if($DisplayName.ToUpper().Contains("VISUAL STUDIO"))
        {  
        return $true   
        }
    }      
    }
    }
    }
    return $false
}


#------------------------------------------------------------------------------------------
# Update expired VS license in vm
#------------------------------------------------------------------------------------------
Function Update-VisualStudioLicense {
    # Check Whether Visual Studio installed or not
    $isVsInstalled= Check-VisualStudioInstalled
    if($isVsInstalled -eq $false){
    Write-host  "No need to update Vs license file because no visual studio installed"  -ForegroundColor Yellow 
    exit
    }
    # Check Whether Vs License file exists or not
    $ZipFile="$WorkingPath\VsLicense\VisualStudio.zip"
    if(-not(Test-Path $ZipFile)){
    Write-TestSuiteError "Vs License file does not exist" -Exit
    }

    if(-not(Test-Path $env:localappdata)){
    Write-TestSuiteError "Local AppData Path does not exist" -Exit 
    }
    
    $VsLicenseDestPath="$env:localappdata\Microsoft\VSCommon\OnlineLicensing\VisualStudio"
    # Delete Expired VS License if exist
    if(Test-Path $VsLicenseDestPath){
    Remove-Item $VsLicenseDestPath -Recurse -Force
    }
    # Update Vs License file to destination
    Write-host  "Update Vs License file to destination:  $VsLicenseDestPath"  -ForegroundColor Green  
    Extract-ZipFile -ZipFile $ZipFile -Destination $VsLicenseDestPath
    Write-Host  "Update Vs License file successfully" -ForegroundColor Green
}


#==========================================================================================
# Main body
#==========================================================================================

#==========================================================================================
# Start log
#==========================================================================================
try{
if(Test-Path $LogFileFullPath){
Remove-Item -Path $LogFileFullPath -Force 
}
}catch{}
Start-Transcript -Path $LogFileFullPath -Append -Force


#==========================================================================================
# Update Visual Studio License file
#==========================================================================================
Update-VisualStudioLicense 


#==========================================================================================
# Stop log
#==========================================================================================
Stop-Transcript

