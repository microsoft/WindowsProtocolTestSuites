# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Set-RdpFileSigning.ps1
# Purpose:        Sign the specified .RDP files with rdpsign.exe and register
#                 the signing certificate as a trusted .rdp publisher via
#                 machine GPO so the April 2026 RDP security dialog
#                 (CVE-2026-26151) does not block unattended task execution.
#                 Used by Config-TerminalClient.ps1 and
#                 Config-TerminalClientForRemoteAdapt.ps1.
#
##############################################################################

Param(
    [Parameter(Mandatory = $true)]
    [string]$Subject,

    [Parameter(Mandatory = $true)]
    [string[]]$RdpFiles
)

$ErrorActionPreference = "Stop"

#----------------------------------------------------------------------------
# 1. Get (or create) the signing cert and trust it locally.
#    New-RdpSigningCert.ps1 throws if it cannot produce a valid thumbprint,
#    so we can use the return value directly.
#----------------------------------------------------------------------------
Write-Host "Generating RDP signing certificate..."
$thumbprint = .\New-RdpSigningCert.ps1 -Subject $Subject

#----------------------------------------------------------------------------
# 2. Resolve rdpsign.exe to its trusted System32 path and verify it is the
#    genuine, Authenticode-valid Windows binary. This script runs elevated and
#    writes machine policy, so a bare "rdpsign.exe" invocation could be hijacked
#    via PATH / current-directory search order (CWE-427). Bind to the absolute
#    path and refuse to run anything that is missing or not validly signed.
#----------------------------------------------------------------------------
$rdpSignPath = Join-Path $env:SystemRoot "System32\rdpsign.exe"
if (-not (Test-Path -LiteralPath $rdpSignPath))
{
    throw "rdpsign.exe not found at the expected location: $rdpSignPath"
}
$rdpSignSignature = Get-AuthenticodeSignature -FilePath $rdpSignPath
if ($rdpSignSignature.Status -ne "Valid")
{
    throw "rdpsign.exe at $rdpSignPath is not Authenticode-valid (status: $($rdpSignSignature.Status)); refusing to execute it."
}

#----------------------------------------------------------------------------
# 3. Sign each .RDP file
#----------------------------------------------------------------------------
$anySigned = $false
foreach ($rdpFile in $RdpFiles)
{
    if (-not (Test-Path $rdpFile))
    {
        Write-Host "Skipping sign for missing $rdpFile"
        continue
    }
    Write-Host "Signing $rdpFile with thumbprint $thumbprint..."
    & $rdpSignPath /sha256 $thumbprint /v $rdpFile
    if ($LASTEXITCODE -ne 0)
    {
        throw "rdpsign.exe failed for $rdpFile (exit code $LASTEXITCODE)"
    }
    $anySigned = $true
}

#----------------------------------------------------------------------------
# 4. Register the cert as a trusted .rdp publisher and revert the
#    April 2026 dialog so the existing LocalDevices whitelist still applies.
#    Only touch machine-wide policy if at least one file was actually signed:
#    the callers always generate these .RDP files immediately before signing,
#    so zero signed files means the upstream generation failed. Failing loud
#    here avoids silently leaving the April 2026 dialog in place, which would
#    block the unattended scheduled tasks and hang the test run.
#----------------------------------------------------------------------------
if (-not $anySigned)
{
    throw "No .RDP files were signed (none of the provided paths existed); refusing to apply machine policy. Check that the .RDP files were generated before signing."
}

Write-Host "Registering RDP signing cert as a trusted .rdp publisher (machine policy)..."
$tsPolicyPath       = "HKLM:\Software\Policies\Microsoft\Windows NT\Terminal Services"
$tsClientPolicyPath = "$tsPolicyPath\Client"
New-Item -Path $tsClientPolicyPath -Force | Out-Null
New-ItemProperty -Path $tsPolicyPath       -Name "AllowSignedFiles"                -Value 1                     -PropertyType DWord  -Force | Out-Null
New-ItemProperty -Path $tsPolicyPath       -Name "TrustedCertThumbprints"          -Value $thumbprint.ToUpper() -PropertyType String -Force | Out-Null
New-ItemProperty -Path $tsClientPolicyPath -Name "RedirectionWarningDialogVersion" -Value 1                     -PropertyType DWord  -Force | Out-Null
