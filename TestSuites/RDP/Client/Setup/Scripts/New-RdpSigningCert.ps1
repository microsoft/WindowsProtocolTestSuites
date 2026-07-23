# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           New-RdpSigningCert.ps1
# Purpose:        Generate (or reuse) a self-signed code-signing certificate
#                 on the SUT for signing the .RDP files emitted by
#                 Config-TerminalClient.ps1. Trusting the same certificate
#                 in LocalMachine\Root lets rdpsign-signed files validate
#                 locally, which together with the publisher-trust policies
#                 set by the caller suppresses the security dialog
#                 introduced by the April 2026 RDP security update
#                 (CVE-2026-26151).
# Returns:        The SHA1 thumbprint of the signing certificate.
#
##############################################################################

Param(
    [string]$Subject     = "CN=WPTS RDP Test Signing",
    [int]   $ValidYears  = 5
)

$ErrorActionPreference = "Stop"

#----------------------------------------------------------------------------
# Reuse an existing cert if one matches and is not near expiry
#----------------------------------------------------------------------------
$existing = Get-ChildItem Cert:\LocalMachine\My |
            Where-Object { $_.Subject -eq $Subject `
                           -and $_.HasPrivateKey `
                           -and $_.NotAfter -gt (Get-Date).AddDays(30) } |
            Sort-Object NotAfter -Descending |
            Select-Object -First 1

if ($existing)
{
    Write-Host "Reusing existing RDP signing cert (thumbprint $($existing.Thumbprint))"
    $cert = $existing
}
else
{
    Write-Host "Generating self-signed RDP signing cert: $Subject"
    $cert = New-SelfSignedCertificate `
        -Subject $Subject `
        -Type CodeSigningCert `
        -KeyUsage DigitalSignature `
        -KeyAlgorithm RSA -KeyLength 2048 `
        -HashAlgorithm SHA256 `
        -NotAfter (Get-Date).AddYears($ValidYears) `
        -CertStoreLocation Cert:\LocalMachine\My
}

#----------------------------------------------------------------------------
# Trust the cert locally so the signature chain validates on this SUT
#----------------------------------------------------------------------------
$inRoot = Get-ChildItem Cert:\LocalMachine\Root | Where-Object Thumbprint -eq $cert.Thumbprint
if (-not $inRoot)
{
    $rootStore = New-Object System.Security.Cryptography.X509Certificates.X509Store("Root","LocalMachine")
    $rootStore.Open("ReadWrite")
    # Add only the public certificate to Root. Rebuilding from RawData yields a
    # key-less X509Certificate2, so the machine Root trust store never persists
    # the signing cert's private key (Root should hold public trust anchors only).
    $publicOnlyCert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2(,$cert.RawData)
    $rootStore.Add($publicOnlyCert)
    $rootStore.Close()
    Write-Host "Added signing cert public key to LocalMachine\Root for chain validation"
}

if ([string]::IsNullOrWhiteSpace($cert.Thumbprint))
{
    throw "New-RdpSigningCert.ps1: failed to produce a valid certificate thumbprint."
}

$cert.Thumbprint
