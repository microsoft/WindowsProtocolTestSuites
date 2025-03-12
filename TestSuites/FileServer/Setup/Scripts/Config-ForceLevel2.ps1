# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param($workingDir = $PSScriptRoot, $protocolConfigFile = "$workingDir\Config.json", $toolsPath = "$workingDir\Tools.json")

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

$sut = $config.Machines.PSObject.Properties | Where-Object {$_.name -match "Sut" -or $_.name -match "Node01" -or $_.Value.Role -match "SUT"} | Select-Object -First 1
if($null -eq $sut) {
    Write-Error.ps1 "Failed to find SUT machine in config file."
    return $false
}
$sutComputerName = $sut.Value.ComputerName

# When the SUT is Linux OS, update the hosts file and get the ip address instead of computer name.
if( $null -ne $sut.Value.os  -and $sut.Value.os -eq "Linux"){

    $ips = $sut.Value.IpConfig
    
    $ip = $ips[0].Ip

    $sutHostString = "$ip $sutComputerName"
    $sutHostString | Out-File -FilePath "$env:windir\System32\drivers\etc\hosts" -Append -encoding ascii

    $sutComputerName = $ip

    # TODO: Ignore Forcelevel when the SUT is Linux as this tool does not support yet in Linux now.
    # After update the tool, below exit code will be removed.
    return $true
}

$tools = $null
try {
    $tools = Get-Content -Path $toolsPath -Raw | ConvertFrom-Json
}
catch {
    Write-Error.ps1 "Failed to parse tools configuration file: $_"
    return $false
}

# $endPointPath = $driver.Value.Tools.TestsuiteZip.targetFolder
$endPointPath = $tools.DriverComputer.TestsuiteZips[0].targetFolder
if(-not(Test-Path -Path $endPointPath)) {
    Write-Error.ps1 "Failed to find endpointPath - in Tools config - TestSuite target folder: $_"
    return $false
}
$binDir = "$endPointPath\Utils"
$ShareUtil = "$binDir\ShareUtil.exe"

#----------------------------------------------------------------------------
# Configure forcelevel2
#----------------------------------------------------------------------------
$retryCount = 0
for (; $retryCount -lt 10; $retryCount++) 
{
    Write-Info.ps1 "Configure forcelevel2 for share: ShareForceLevel2"
    CMD /C "$ShareUtil $sutComputerName ShareForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1 | Write-Info.ps1

    if($config.Core.DomainName -ne "WORKGROUP"){
        $scaleoutFSName = $config.Endpoints.ScaleoutFS.Name
        if((Get-WmiObject win32_computersystem).partofdomain -eq $true -and (Test-Connection -ComputerName $scaleoutFSName -Quiet))
        {
            Write-Info.ps1 "Configure forcelevel2 for share: SMBClusteredForceLevel2"
            CMD /C "$ShareUtil $scaleoutFSName SMBClusteredForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1 | Write-Info.ps1
        }
    }
    if ($lastExitCode -eq 0)
    {
        break;
    }
    Start-Sleep 5
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
return $true