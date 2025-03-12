# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param($workingDir = $PSScriptRoot, $protocolConfigFile = "$workingDir\Config.json")

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath"

#----------------------------------------------------------------------------
# if working dir is not exists. it will use scripts path as working path
#----------------------------------------------------------------------------
if(!(Test-Path "$workingDir"))
{
    $workingDir = $scriptPath
}

if(!(Test-Path "$protocolConfigFile"))
{
    $protocolConfigFile = "$workingDir\Config.json"
    if(!(Test-Path "$protocolConfigFile")) 
    {
        Write-Error.ps1 "No Config file found."
        return $false
    }
}

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

function GetDfsNsNameSuffix() {
    $curFt = [DateTime]::UtcNow.ToFileTimeUtc()
    $curFtBytes = [BitConverter]::GetBytes($curFt)
    $suffix = if ([BitConverter]::IsLittleEndian) {
        "$([BitConverter]::ToUInt32($curFtBytes, 0))" 
    }
    else {
        "$([BitConverter]::ToUInt32($curFtBytes, 4))"
    }

    return $suffix
}

#----------------------------------------------------------------------------
# Get content from protocol config file
#----------------------------------------------------------------------------
$config = $null
try {
    $config = Get-Content -Path $protocolConfigFile -Raw | ConvertFrom-Json
}
catch {
    Write-Error.ps1 "Failed to parse config file: $_"
    return $false
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------
# Define full access account
$fullAccessAccount = "BUILTIN\Administrators"

# Define home drive
$homeDrive = $ENV:HomeDrive

# Define servers for DFSC target
$serverComputerName = "$ENV:ComputerName"

$clusternodes = @()

# Check if there are any machines with IsClusterNode set to true
foreach ($machine in $config.Machines.PSObject.Properties) {
    if ($machine.Value.IsClusterNode -eq "true") {
        $clusterNodes += $machine.Value.ComputerName
    }
}

if($clusterNodes.Count -gt 0)
{ # Cluster Environment
    $targetServer = $clusternodes | Where-Object {$_ -ne "$serverComputerName"}
    $targetServerName = $targetServer
}
else
{ # Non-Cluster Environment
    $targetServerName = $serverComputerName
}

#----------------------------------------------------------------------------
# Install Windows Features
#----------------------------------------------------------------------------
Write-Info.ps1 "Install Windows Features"
Import-Module Servermanager
$osVersion = Get-OSVersionNumber.ps1

if ([double]$osVersion -ge [double]"6.2")
{
	Write-Info.ps1 "OS is Windows Server 2012 or later version."
    Add-WindowsFeature FS-DFS-Namespace

    $type = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion").InstallationType
    if ($type -ne "Server Core") {
        Add-WindowsFeature RSAT-DFS-Mgmt-Con
    }
}
else
{
    Write-Info.ps1 "OS is Windows 2008 R2 or lower version."
    Add-WindowsFeature FS-DFS
}


#----------------------------------------------------------------------------
# Start DFS service
#----------------------------------------------------------------------------
Write-Info.ps1 "Start DFS service..."    
cmd /C sc start dfs 2>&1 | Write-Info.ps1

#----------------------------------------------------------------------------
# Create SMBDfs DFSC Environment
#----------------------------------------------------------------------------
Write-Info.ps1 "Create SMBDfs shared folder"
Create-SMBShare.ps1 -name "SMBDfs" -Path "$homeDrive\DFSRoots\SMBDfs" -FullAccess "$fullAccessAccount"

Write-Info.ps1 "Create SMBBasic if not exist."
Create-SMBShare.ps1 -name "SMBBasic" -Path "$homeDrive\SMBBasic" -FullAccess "$fullAccessAccount"  -CachingMode BranchCache

Write-Info.ps1 "Create DFS Namespace"
cmd.exe /c dfsutil root addstd \\$serverComputerName\SMBDfs 2>&1 | Write-Info.ps1

Write-Info.ps1 "Add share folder to DFS Namespace"
cmd.exe /c dfscmd /map \\$serverComputerName\SMBDfs\SMBDfsLink \\$serverComputerName\SMBBasic /restore 2>&1 | Write-Info.ps1

#----------------------------------------------------------------------------
# Create Standalone DFSC Environment
#----------------------------------------------------------------------------
Write-Info.ps1 "Create Standalone shared folder"
Create-SMBShare.ps1 -name "Standalone" -Path "$homeDrive\DFSRoots\Standalone" -FullAccess "$fullAccessAccount"

Write-Info.ps1 "Create Stand-alone DFS Namespace"
cmd.exe /c dfsutil root addstd \\$serverComputerName\Standalone 2>&1 | Write-Info.ps1

Write-Info.ps1 "Add Link target to Stand-alone Namespace"
cmd.exe /c dfscmd /map \\$serverComputerName\Standalone\DFSLink \\$targetServerName\FileShare /restore 2>&1 | Write-Info.ps1	

Write-Info.ps1 "Add Interink to Stand-alone Namespace"
cmd.exe /c dfscmd /map \\$serverComputerName\Standalone\Interlink \\$serverComputerName\SMBDfs\SMBDfsLink /restore 2>&1 | Write-Info.ps1

#----------------------------------------------------------------------------
# Create DomainBased DFSC Environment
#----------------------------------------------------------------------------
if ((Get-WmiObject Win32_ComputerSystem).PartOfDomain -eq $true) {
    Write-Info.ps1 "Create unique DomainBased DFS Namespace Name"
    $domainBasedNsName = "DomainBased$(GetDfsNsNameSuffix)"

    Write-Info.ps1 "Create DomainBased shared folder"
    Create-SMBShare.ps1 -name "$domainBasedNsName" -Path "$homeDrive\DFSRoots\$domainBasedNsName" -FullAccess "$fullAccessAccount"

    Write-Info.ps1 "Create Domain-based DFS Namespace"
    cmd.exe /c dfsutil root adddom \\$serverComputerName\$domainBasedNsName 2>&1 | Write-Info.ps1

    Write-Info.ps1 "Add Link target to Domain-based Namespace"
    cmd.exe /c dfscmd /map \\$serverComputerName\$domainBasedNsName\DFSLink \\$targetServerName\FileShare /restore 2>&1 | Write-Info.ps1

    Write-Info.ps1 "Add Interink to Domain-based Namespace"
    cmd.exe /c dfscmd /map \\$serverComputerName\$domainBasedNsName\Interlink \\$serverComputerName\SMBDfs\SMBDfsLink /restore 2>&1 | Write-Info.ps1
     
    Write-Info.ps1 "Write the DomainBased DFS Namespace name to a file"
    Set-Content -Path "C:\DomainBased.txt" -Value $domainBasedNsName
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
return $true