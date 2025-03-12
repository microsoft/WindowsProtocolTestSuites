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
        exit ExitCode
    }
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
    Write-ConfigFailureSignal
    exit ExitCode
}

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force


function RetryCopyFile($sourceFile, $destFilePath, $destFileName)
{
    #-----------------------------------------------------
    # Retry to copy file from source to dest
    #-----------------------------------------------------
    $retryTimes = 0
    while(!( Test-Path("$destFilePath"+"$destFileName")) -and $retryTimes -lt 10)
    {
        .\Write-Info.ps1 "Copy $sourceFile to $destFilePath"
        CMD /C copy $sourceFile $destFilePath
        Start-Sleep 20
        $retryTimes++ 
    }

    if($retryTimes -eq 10)
    {
        .\Write-Info.ps1 "Retried $retryTimes to copy $sourceFile to $destFilePath failed."
    }
    else
    {
        .\Write-Info.ps1 "Retried $retryTimes to copy $sourceFile to $destFilePath succeeded."
    }
}

function CreateAndCopyVHDX($vhdName)
{
    #-----------------------------------------------------
    # Create VHD
    #-----------------------------------------------------
    $vhdFullPath = "$workingDir\$vhdName.vhdx"
    .\Write-Info.ps1 "Create expandable VHD for $vhdName, maximun size 1024MB"
    "create vdisk file=$vhdFullPath maximum=1024 type=expandable" | diskpart


    #-----------------------------------------------------
    # Retry to copy VHD to scalout file server
    #-----------------------------------------------------
	$scaleoutfsName = $config.Endpoints.ScaleoutFS.Name
    RetryCopyFile "$vhdFullPath" "\\$scaleoutfsName\SMBClustered\" "$vhdName.vhdx"

    #----------------------------------------------------------------------------
    # Ending
    #----------------------------------------------------------------------------
    .\Write-Info.ps1 "Completed copy $vhdName.vhdx."
}

CreateAndCopyVHDX("sqos")

$minIops = 100
$maxIops = 200
$maxBandwidthInKB = 1600
$maxBandwidth = 1024*$maxBandwidthInKB

$policy = New-StorageQosPolicy -Name Desktop -PolicyType Dedicated -MinimumIops $minIops -MaximumIops $maxIops -MaximumIOBandwidth $maxBandwidth
if($null -ne $policy)
{
    .\Write-Info.ps1 "Create Storage QoS policy: Desktop - File Path - C:\SqosPolicyId.txt" -ForegroundColor Cyan
    Set-Content -Path "C:\SqosPolicyId.txt" -Value $policy.PolicyId
}

Stop-Transcript
exit 0