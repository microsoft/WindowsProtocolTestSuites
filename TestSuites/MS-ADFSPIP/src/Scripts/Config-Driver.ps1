########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

Param
(
    [string]$VMName      = "MS-ADFSPIP-DRIVER",
    [string]$LogPath     = "c:\temp\controller.log"
)
$currentDir = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

Push-Location $currentDir
Import-Module .\ADFSLib.PSM1

Start-Transcript -Path $LogPath

# Parameters from the config file
$param = @{} 

Write-Host "Read config file"
Read-VMParameters -VMName $VMName -RefParamArray ([ref]$param) -ErrorAction Stop
$param

#Write-Host "Set ethernet IP address and DNS"
#Set-EthernetSettings -IPAddress $param["ip"] -DnsServer $param["dns"]

Write-Host "Join the computer to domain"
Join-Domain -Domain $param["domain"] -Username $param["username"] -Password $param["password"] -ErrorAction Stop

Write-Host "Enable remoting"
Enable-Remoting

# Use domain administrator to logon
Set-AutoLogon -Domain $param["domain"] -Username $param["username"] -Password $param["password"]

#sleep 5
Stop-Transcript

# restart the computer to finish joining domain
# Restart-Computer -Force