# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

# When the SUT is Linux OS, update the hosts file and get the ip address instead of computer name.
if( $null -ne $sut.os  -and $sut.os -eq "Linux"){

    $ip = $sut.ip

    if(($sut.ip | Measure-Object ).Count -gt 1){
        $ip = $sut.ip[0]
    }

    $sutHostString = "$ip $sutComputerName"
    $sutHostString | Out-File -FilePath "$env:windir\System32\drivers\etc\hosts" -Append -encoding ascii

    $sutComputerName = $ip

    # TODO: Ignore Forcelevel when the SUT is Linux as this tool does not support yet in Linux now.
    # After update the tool, below exit code will be removed.
    exit 0
}

$driver = $config.lab.servers.vm | Where {$_.role -match "DriverComputer"}
$endPointPath = $driver.tools.TestsuiteZip.targetFolder
if(-not(Test-Path -Path $endPointPath)) {
    $endPointPath = $sut.tools.TestsuiteZip.targetFolder
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

    if($config.lab.ha){
        $scaleoutFSName = $config.lab.ha.scaleoutfs.name
        if((gwmi win32_computersystem).partofdomain -eq $true -and (Test-Connection -ComputerName $scaleoutFSName -Quiet))
        {
            Write-Info.ps1 "Configure forcelevel2 for share: SMBClusteredForceLevel2"
            CMD /C "$ShareUtil $scaleoutFSName SMBClusteredForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1 | Write-Info.ps1
        }
    }
    if ($lastExitCode -eq 0)
    {
        break;
    }
    sleep 5
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0