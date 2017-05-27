########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

Param 
(
    [string]$VMName      = "MS-ADFSPIP-DCCA",
    [string]$LogPath     = "c:\temp\controller.log"
)
$currentDir = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

Push-Location $currentDir
Import-Module .\ADFSLib.PSM1

Start-Transcript -Path $LogPath

# Parameters from the config file
$param = @{}

Write-Host "Read DC config"
Read-VMParameters -VMName $VMName -RefParamArray ([ref]$param) -ErrorAction Stop
$param

#Write-Host "Set ethernet IP address and DNS"
#Set-EthernetSettings -IPAddress $param["ip"] -DnsServer $param["dns"]

Write-Host "Promote domain controller"
Install-DomainController -DomainName $param["domain"] -AdminPassword $param["password"] -ErrorAction Stop

# read driver computer information
$driver = @{}
$driverVMName = "MS-ADFSPIP-DRIVER"

Write-Host "Read driver config"
Read-VMParameters -VMName $driverVMName -RefParamArray ([ref]$driver) -ErrorAction Stop

Write-Host "Add DNS host and alias"
Add-AdfsDnsRecord -ZoneName $driver["domain"] -IPv4Address $driver["ip"]

Write-Host "Set auto-logon"
Set-AutoLogon -Domain $param["domain"] -Username $param["username"] -Password $param["password"]

#sleep 5
Stop-Transcript

# restart to finish promoting DC
# Restart-Computer -Force