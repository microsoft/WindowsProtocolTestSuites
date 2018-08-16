#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Protocol.xml")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# if working dir is not exists. it will use scripts path as working path
#----------------------------------------------------------------------------
if(!(Test-Path "$workingDir"))
{
    $workingDir = $scriptPath
}

if(!(Test-Path "$protocolConfigFile"))
{
    $protocolConfigFile = "$workingDir\Protocol.xml"
    if(!(Test-Path "$protocolConfigFile")) 
    {
        Write-Error.ps1 "No protocol.xml found."
        exit ExitCode
    }
}

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function ExitCode()
{ 
    return $MyInvocation.ScriptLineNumber 
}

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
[xml]$config = Get-Content "$protocolConfigFile"
if($config -eq $null)
{
    Write-Error.ps1 "protocolConfigFile $protocolConfigFile is not a valid XML file."
    exit ExitCode
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
$fullAccessAccount = "BUILTIN\Administrators"
$systemDrive = $ENV:SystemDrive
$OSVersion = [System.Environment]::OSVersion.Version
#----------------------------------------------------------------------------
# Create Share Folders
#----------------------------------------------------------------------------
Write-Info.ps1 "Create share folder: $systemDrive\FileShare"
Create-SMBShare.ps1 -name "FileShare" -Path "$systemDrive\FileShare" -FullAccess "$fullAccessAccount"  

Write-Info.ps1 "Create share folder: $systemDrive\SMBBasic"
Create-SMBShare.ps1 -name "SMBBasic" -Path "$systemDrive\SMBBasic" -FullAccess "$fullAccessAccount"  
Write-Info.ps1 "Create symboliclink: $systemDrive\SMBBasic\symboliclink"
CMD /C "mklink /D $systemDrive\SMBBasic\symboliclink $systemDrive\Fileshare\"
CMD /C "mkdir $systemDrive\SMBBasic\sub"
CMD /C "mklink /D $systemDrive\SMBBasic\sub\symboliclink2 $systemDrive\Fileshare\"

Write-Info.ps1 "Create SameWithSMBBasic for CreateClose model"
Create-SMBShare.ps1 -Name "SameWithSMBBasic" -Path "$systemDrive\SMBBasic" -FullAccess "$fullAccessAccount" 

Write-Info.ps1 "Create DifferentFromSMBBasic for CreateClose model"
Create-SMBShare.ps1 -Name "DifferentFromSMBBasic" -Path "$systemDrive\DifferentFromSMBBasic" -FullAccess "$fullAccessAccount"  

Write-Info.ps1 "Create share folder: $systemDrive\ShareForceLevel2"
Create-SMBShare.ps1 -name "ShareForceLevel2" -Path "$systemDrive\ShareForceLevel2" -FullAccess "$fullAccessAccount"   

Write-Info.ps1 "Create share folder: $systemDrive\SMBEncrypted"
Create-SMBShare.ps1 -name "SMBEncrypted" -Path "$systemDrive\SMBEncrypted" -FullAccess "$fullAccessAccount"  -EncryptData $true

Write-Info.ps1 "Create Volume for SMBReFSShare"

$volume = gwmi -Class Win32_volume | where {$_.FileSystem -eq "REFS" -and $_.Label -eq "REFS"}
if($volume -eq $null)
{
	Write-Info.ps1 "Create Volume for SMBReFSShare"
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
	$diskPartCmd += "select partition $partitionId"
	$diskPartCmd += "shrink minimum=5120"
    # the extended partition should be enough to contain both ReFS and FAT32
	$diskPartCmd += "create partition extended size=4096"
    # assign 2000MB for ReFS, and save 2000MB for FAT32
	$diskPartCmd += "create partition logical size=2000"
	$diskPartCmd += "select partition $newPartitionId"
    if ($OSVersion.Major -ge 10)
    {
        Write-Info.ps1 "ReFS in Windows vNext, Windows Server vNext, and beyond uses a default cluster size of 4 KB. ReFS also supports a 64-KB cluster size."
        Write-Info.ps1 "For current test suite, we use 64-KB cluster size."
        $diskPartCmd += "format fs=ReFS quick label=REFS unit=64K"
    }
    else
    {
        Write-Info.ps1 "ReFS in Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 uses a fixed cluster size of 64 KB."
        $diskPartCmd += "format fs=ReFS quick label=REFS"
    }
	$diskPartCmd += "assign letter=K"
	$diskPartCmd | diskpart.exe
}

Write-Info.ps1 "Create share folder: K:\SMBReFSShare"
Create-SMBShare.ps1 -name "SMBReFSShare" -Path "K:\SMBReFSShare" -FullAccess "$fullAccessAccount"

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed setup SMB2 ENV."
Stop-Transcript
exit 0