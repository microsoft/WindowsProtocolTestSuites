########################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.

## Microsoft Windows Powershell Scripting
## File:           Create-FSAEnv.ps1
## Purpose:        Create ShareFolder and relevant files in SUT for MS-FSA test suite
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012, Windows Server 2012 R2
## 
#########################################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function ExitCode()
{ 
    return $MyInvocation.ScriptLineNumber 
}

# Create FAT32 Volume for MS-FSA test suite 
function CreateFAT32VolumeForFSA()
{
    Write-Info.ps1 "Create Volume for SMBFAT32Share"
    $volume = gwmi -Class Win32_volume | where {$_.FileSystem -eq "FAT32" -and $_.Label -eq "FAT32"}
    if ($volume -eq $null)
    {
        Write-Info.ps1 "Create Volume for SMBFAT32Share"
        # Get disk and partition
        $disk = Get-WmiObject -Class Win32_DiskDrive | sort DeviceID | Select-Object -first 1
        $diskNum = $disk.Index
        For ($i = 1; $i -le 20; $i++) {
            $partition = Get-Partition -DiskNumber $diskNum -PartitionNumber $i
            if ($partition -eq $null) {
                $partitionId = $i - 1
                break
            }
        }
        $newPartitionId = $partitionId + 1

        Write-Info.ps1 "Create partition using diskpart.exe"
        $diskPartCmd = @()
        $diskPartCmd += "select disk $diskNum"
        # assign the left 2000MB for FAT32
        $diskPartCmd += "create partition logical size=2000"
        $diskPartCmd += "select partition $newPartitionId"
        Write-Info.ps1 "FAT32 in Windows vNext, Windows Server vNext, and beyond uses a default cluster size of 4 KB. FAT32 also supports a 32-KB cluster size."
        Write-Info.ps1 "For current test suite, we use default 4-KB cluster size."
        $diskPartCmd += "format fs=FAT32 quick label=FAT32"
        $diskPartCmd += "assign letter=J"
        $diskPartCmd | diskpart.exe
    }
}

# Create share folder for MS-FSA test suite
function CreateShareFolderForFSA()
{
    Param(
    [String]$Path = "C:",
    [String]$FolderName = "FileShare"
    )
    
    #-----------------------------------------------------
    # Verify parameters
    #-----------------------------------------------------
    $Path = $Path.Trim()
    $FolderName = $FolderName.Trim()
    if([System.String]::IsNullOrEmpty($FolderName))
    {
    	Write-Info.ps1 "Folder Name could not be null or empty." -ForegroundColor Red
		exit ExitCode
    }
    $sharefolderPath = "$Path\$FolderName"
    
    #-----------------------------------------------------
    # Create Share Folder
    #-----------------------------------------------------
    Create-SMBShare.ps1 -name "$FolderName" -Path "$sharefolderPath" -FullAccess "BUILTIN\Administrators"

    #-----------------------------------------------------
    # Create ExistingFolder, ExistingFile.txt and link.txt
    #-----------------------------------------------------
	Write-Info.ps1 "Create Directory $sharefolderPath\ExistingFolder"
    New-Item -Type Directory -Path $sharefolderPath\ExistingFolder -Force
    
	Write-Info.ps1 "Create file $sharefolderPath\ExistingFile.txt"
    New-Item -type file -Path $sharefolderPath\ExistingFile.txt -Force
	
	Write-Info.ps1 "mklink $sharefolderPath\link.txt $sharefolderPath\ExistingFile.txt"
    cmd /C "mklink $sharefolderPath\link.txt $sharefolderPath\ExistingFile.txt" 2>&1 | Write-Info.ps1
    
    #-----------------------------------------------------
    # Create MountPoint
    #-----------------------------------------------------
	Write-Info.ps1 "Create MountPoint"
    New-Item -Type Directory -Path $sharefolderPath\MountPoint -Force
    mountvol $path /l > mountvol_info.txt
    $volumeInfo = Get-Content mountvol_info.txt
    try 
    {
		Write-Info.ps1 "mountvol $sharefolderPath\MountPoint $volumeInfo"
    	cmd /C "mountvol $sharefolderPath\MountPoint $volumeInfo" 2>&1 | Write-Info.ps1
    }
    catch
    {
    	Write-Info.ps1 "Create MountPoint failed." -ForegroundColor Red
    }
    
    #-----------------------------------------------------
    # Enable Short Name
    #-----------------------------------------------------
	Write-Info.ps1 "Enable Short Name"
    try
    {
		Write-Info.ps1 "fsutil 8dot3name set $Path 0"
    	cmd /C "fsutil 8dot3name set $Path 0" 2>&1 | Write-Info.ps1
    }
    catch
    {
    	Write-Info.ps1  "Enable Short Name failed." -ForegroundColor Red
    }
    #-----------------------------------------------------
    # Create Shadow Copy as Previous Version for ExistingFile.txt
    #-----------------------------------------------------
	Write-Info.ps1 "Create Shadow Copy as Previous Version for ExistingFile.txt"
    for($i = 1; $i -le 3; $i++)
    {
    	dir >> $sharefolderPath\ExistingFile.txt
    	vssadmin.exe Create Shadow /For=$Path /AutoRetry=2 2>&1 | Write-Info.ps1
    }
}

# Main entry
#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Create share folder for MS-FSA test suite
#----------------------------------------------------------------------------
CreateFAT32VolumeForFSA

#----------------------------------------------------------------------------
# Create share folder for MS-FSA test suite
#----------------------------------------------------------------------------
CreateShareFolderForFSA -Path "C:" -FolderName "FileShare"
# This script assumes Create-SMB2Env.ps1 has been run and created K: solume for REFS file system.
CreateShareFolderForFSA -Path "K:" -FolderName "SMBReFSShare"
# This script assumes Create-SMB2Env.ps1 has been run and created F: solume for FAT32 file system.
CreateShareFolderForFSA -Path "J:" -FolderName "SMBFAT32Share"

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed setup FSA ENV."
Stop-Transcript
exit 0

