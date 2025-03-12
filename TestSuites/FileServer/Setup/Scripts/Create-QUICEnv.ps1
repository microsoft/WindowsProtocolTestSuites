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
        .\Write-Error.ps1 "No config file found."
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
    .\Write-Error.ps1 "Failed to parse config file: $_"
    return $false
}

#----------------------------------------------------------------------------
# Define common variables
#----------------------------------------------------------------------------

$systemDrive = $ENV:SystemDrive
$osVersion = (Get-WmiObject Win32_OperatingSystem).Caption

$sut = $config.Machines.SUT 
if ($null -eq $sut) {
   $sut = $config.Machines.Node01
}

$sutName = $sut.ComputerName
$domain = $sut.domain
$sutComputerName = $sutName
if ((-not [string]::IsNullOrEmpty($domain)) -and ($domain.ToLower() -ne "workgroup")) {
    $sutComputerName = "$sutName.$domain".ToLower()
}

if ($osVersion -notlike "*Windows*") {
    .\Write-Error.ps1 "QUIC is not supported on Linux"
    return $false
}

$osName = (Get-WMIObject Win32_OperatingSystem).Name
.\Write-Info.ps1 "OS Name: $osName"

#----------------------------------------------------------------------------
# Create SMB certificate mapping
#----------------------------------------------------------------------------
if ($osName -match "Azure Edition") {
    .\Write-Info.ps1 "Create SelfSigned Certificate: $sutName"
    $currCert = New-SelfSignedCertificate -Subject $sutName -FriendlyName "SMB over QUIC for File Servers" -KeyUsageProperty Sign -KeyUsage DigitalSignature -CertStoreLocation Cert:\LocalMachine\My -HashAlgorithm SHA256 -Provider "Microsoft Software Key Storage Provider" -KeyAlgorithm ECDSA_P256 -KeyLength 256 -DnsName @($sutComputerName, $sutName)

    $certThumbprint = $currCert.Thumbprint
    $subject = $currCert.Subject
    .\Write-Info.ps1 "Mapping SmbServer:$sutComputerName with Certificate: $certThumbprint Subject:$subject"
    New-SmbServerCertificateMapping -Name $sutComputerName -Thumbprint $certThumbprint -StoreName my -Subject $subject

    .\Write-Info.ps1 "Import the certificate to root"
    $pfxPwd = New-Object SecureString
    $config.Core.Password.ToCharArray() | ForEach-Object {$pfxPwd.AppendChar($_)}
    
    Export-PfxCertificate -Cert $currCert -FilePath "QUICCert.pfx" -Password $pfxPwd
    Import-PfxCertificate -FilePath "QUICCert.pfx" -CertStoreLocation Cert:\LocalMachine\Root -Password $pfxPwd

    .\Write-Info.ps1 "Enable SMB encryption on QUIC connection."
    Set-SmbServerConfiguration -DisableSmbEncryptionOnSecureConnection $false -Confirm:$false

    .\Write-Info.ps1 "Enable NamedPipe access on QUIC connection."
    Set-SmbServerConfiguration -RestrictNamedpipeAccessViaQuic $false -Confirm:$false
}
else {
    .\Write-Info.ps1 "QUIC is only supported on Windows Server 2022 Azure Edition and later versions."
}


#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Completed setup QUIC ENV."
Stop-Transcript
return $true