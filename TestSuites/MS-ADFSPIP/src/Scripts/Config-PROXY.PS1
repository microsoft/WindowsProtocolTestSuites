########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################
Param
(
    [string]$VMName      = "MS-ADFSPIP-PROXY",
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

Write-Host "Install Web Application Proxy"
Install-WebApplicationProxy -ErrorAction Stop

$certDir = "..\Data\Certificates"
Write-Host "Import CA root certificate"
Import-Certificate -FilePath "$certDir\CARoot.cer" -CertStoreLocation Cert:\LocalMachine\Root -Confirm:$false

Write-Host "Import mock client certificate"
Import-Certificate -FilePath "$certDir\Client.cer" -CertStoreLocation Cert:\LocalMachine\Root -Confirm:$false

Write-Host "Import ADFS certificate"
Import-PfxCertificate -FilePath "$certDir\Adfs.pfx" -Password $(ConvertTo-SecureString -AsPlainText "123" -Force) `
					  -CertStoreLocation Cert:\LocalMachine\My -Confirm:$false

Write-Host "Import WebApp certificate"
Import-PfxCertificate -FilePath "$certDir\WebApp.pfx" -Password $(ConvertTo-SecureString -AsPlainText "123" -Force) `
					  -CertStoreLocation Cert:\LocalMachine\My -Confirm:$false

Write-Host "Enable remoting"
Enable-Remoting

# Use local administrator to logon
Write-Host "Set auto-logon"
Set-AutoLogon -Domain $param["name"] -Username $param["username"] -Password $param["password"]

Write-Host "Flush DNS"
ipconfig /flushdns

#sleep 5
Stop-Transcript

# restart the computer to finish joining domain
#Restart-Computer -Force