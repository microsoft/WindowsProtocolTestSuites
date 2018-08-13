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
# Get content from protocol config file
#----------------------------------------------------------------------------
[xml]$config = Get-Content "$protocolConfigFile"
if($config -eq $null)
{
    Write-Error.ps1 "protocolConfigFile $protocolConfigFile is not a valid XML file."
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
        Write-Info.ps1 "Copy $sourceFile to $destFilePath"
        CMD /C copy $sourceFile $destFilePath
        Sleep 20
        $retryTimes++ 
    }

    if($retryTimes -eq 10)
    {
        Write-Info.ps1 "Retried $retryTimes to copy $sourceFile to $destFilePath failed."
    }
    else
    {
        Write-Info.ps1 "Retried $retryTimes to copy $sourceFile to $destFilePath succeeded."
    }
}

function CreateAndCopyVHDX($vhdName)
{
    #-----------------------------------------------------
    # Create VHD
    #-----------------------------------------------------
    $vhdFullPath = "$env:SystemDrive\Temp\$vhdName.vhdx"
    Write-Info.ps1 "Create expandable VHD for $vhdName, maximun size 1024MB"
    "create vdisk file=$vhdFullPath maximum=1024 type=expandable" | diskpart


    #-----------------------------------------------------
    # Retry to copy VHD to scalout file server
    #-----------------------------------------------------
	$scaleoutfsName = $config.lab.ha.scaleoutfs.name
    RetryCopyFile "$vhdFullPath" "\\$scaleoutfsName\SMBClustered\" "$vhdName.vhdx"

    #----------------------------------------------------------------------------
    # Ending
    #----------------------------------------------------------------------------
    Write-Info.ps1 "Completed copy $vhdName.vhdx."
}

CreateAndCopyVHDX("sqos")

Stop-Transcript
exit 0