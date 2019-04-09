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
$sut = $config.lab.servers.vm | Where {$_.role -match "SUT" -or $_.role -match "Node01"}
$sutComputerName = $sut.name

$endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\FileServer\Server-Endpoint"
$version = Get-ChildItem $endPointPath | where {$_.Attributes -match "Directory" -and $_.Name -match "\d+\.\d+\.\d+\.\d+"} | Sort-Object Name -descending | Select-Object -first 1
$binDir = "$endPointPath\$version\bin"
$ShareUtil = "$binDir\ShareUtil.exe"

#----------------------------------------------------------------------------
# Configure forcelevel2
#----------------------------------------------------------------------------
Write-Info.ps1 "Configure forcelevel2 for share: ShareForceLevel2"
CMD /C "$ShareUtil $sutComputerName ShareForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1 | Write-Info.ps1

if($config.lab.ha){
    $scaleoutFSName = $config.lab.ha.scaleoutfs.name
    if((gwmi win32_computersystem).partofdomain -eq $true -and (Test-Connection -ComputerName $scaleoutFSName -Quiet))
    {
        Write-Info.ps1 "Configure forcelevel2 for share: SMBClusteredForceLevel2"
        CMD /C "$ShareUtil $scaleoutFSName SMBClusteredForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1 | Write-Info.ps1
    }
    sleep 5
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0