#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

# This script is used to configure test environment of test driver computer   
# to run against Linux SUT. The script should be copied to VSTORMLITE\PostScript                   
# by WTT job before it used.

param($protocolName="RDP",$endPoint="Client")

# Decide the root installation path of test suite of each protocol, it it protocol specific implemnetation
$endPointPath = "$env:SystemDrive\MicrosoftProtocolTests\$protocolName\$endPoint-Endpoint" 
$version = Get-ChildItem $endPointPath | where {$_.Name -match "\d+\.\d+\.\d+\.\d+"} | Sort-Object Name -descending | Select-Object -first 1
$installpath = "$endPointPath\$version"


# Decide Driver Computer running environment(for Linux or Windows)  
$ConfigHomeDir = $env:SystemDrive + "\temp"
$LinuxSUTConfigDir = "$ConfigHomeDir\VSTORMLITEFiles\LinuxSutAdapter"
[bool]$script:IsLinuxSUTEnv = $false
[xml]$protocolconfig = Get-Content $ConfigHomeDir\protocol.xml
foreach($vmdata in ($protocolconfig.lab.servers.vm)) {
    if($vmdata.vmtype -eq "Linux" -and $vmdata.role -eq "SUT")
    {
       $script:IsLinuxSUTEnv = $true
       break;
    }
} 

# add "c:\temp" to $env:path for pscp/plink tools 
[environment]::SetEnvironmentVariable("path", "$env:path;$ConfigHOmeDir", "machine")

copy $ConfigHomeDir\scripts\Config-DriverComputer.ps1 $installpath\Scripts\Config-DriverComputer.ps1 -Force

# If SUT is linux test env, run below configuration
if ($script:IsLinuxSUTEnv -eq $true) 
{ 
    Write-Host "Linux SUT is detected, configure SUT Adapter for Linux SUT ..." -ForegroundColor Green

    # Copy scripts to bin and sourece dir
    $sourcecodepath = $installpath + "\Source\Client\TestCode"

    if(! (Test-Path "$installpath\bin\NonWindowsSUTImplementation")) {
        New-Item -Path "$installpath\bin\NonWindowsSUTImplementation" -Type directory
    }

    if(! (Test-Path "$sourcecodepath\Adapter\NonWindowsSUTImplementation")) {
        New-Item -Path "$sourcecodepath\Adapter\NonWindowsSUTImplementation" -Type directory
    }

    $psfileItems = Get-ChildItem $LinuxSUTConfigDir -Include *.ps1 -Recurse
    foreach ($item in $psfileItems) { 
        $fname = $item.Name
         
        copy $LinuxSUTConfigDir\$fname "$installpath\bin\NonWindowsSUTImplementation\" -Force
        copy $LinuxSUTConfigDir\$fname "$installpath\bin\" -Force

        copy $LinuxSUTConfigDir\$fname "$sourcecodepath\Adapter\NonWindowsSUTImplementation\" -Force
        copy $LinuxSUTConfigDir\$fname "$sourcecodepath\Adapter\SUTControlAdapter\" -Force
    } 
    
    $shfileItems = Get-ChildItem $LinuxSUTConfigDir -Include *.sh -Recurse
    foreach ($item in $shfileItems) {
        $fname = $item.Name
        copy $LinuxSUTConfigDir\$fname "$installpath\bin\NonWindowsSUTImplementation\" -Force
        copy $LinuxSUTConfigDir\$fname "$sourcecodepath\Adapter\NonWindowsSUTImplementation\" -Force
    }   

    Write-Host "Copy SUT Adapter Scripts for Linux SUT is done" -ForegroundColor Green
}