# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Custom Script Extension bootstrap shared by ALL Windows VMs across scenarios
# (Workgroup Driver/SUT; Domain DC/Driver/SUT). The Bicep modules load this file
# with loadTextContent(), substitute the __TOKENS__, base64 it into the
# extension's encrypted protectedSettings, and the commandToExecute decodes and
# runs it on the VM.
#
# Tokens injected at compile/deploy time:
#   __SCENARIO__       workgroup | domain              (log file name, messages)
#   __ROLE__           dc | driver | sut               (log file name, messages)
#   __PACKAGE_NAME__   Workgroup-Package | Domain-Package (C:\<name> extract root)
#   __DEPLOY_SCRIPT__  Deploy-DC.ps1 | Deploy-Driver.ps1 | Deploy-SUT.ps1
#   __PACKAGE_URL__    URL of the package zip
#   __PACKAGE_HOST__   host part of the URL (DNS readiness probe)
#   __PASSWORD_B64__   base64 admin password for Set-ConfigCredential.ps1
#
# When the extension pre-downloads the package via fileUris, the zip is found in
# the CWD and used directly. Otherwise this script waits for DNS and downloads it
# itself -- needed by domain members whose only DNS server is the DC, which may
# still be promoting when the extension fires.

$ErrorActionPreference = 'Stop'
Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force
Start-Transcript -Path 'C:\__SCENARIO__-__ROLE__-setup.log' -Append

Write-Output 'Starting __SCENARIO__ __ROLE__ setup...'
New-Item -ItemType Directory -Path 'C:\__PACKAGE_NAME__' -Force | Out-Null

# Package already present? (fileUris path: the extension downloaded it into its
# working directory, which is the CWD of this script.)
$zipFile = Get-ChildItem -Path . -Filter *.zip -ErrorAction SilentlyContinue | Select-Object -First 1

if (-not $zipFile) {
    Write-Output 'Waiting for DNS resolution of __PACKAGE_HOST__ (DNS may still be coming up)...'
    $dns = $false
    for ($i = 0; $i -lt 60; $i++) {
        try {
            Resolve-DnsName -Name '__PACKAGE_HOST__' -ErrorAction Stop | Out-Null
            $dns = $true
            break
        } catch {
            Start-Sleep -Seconds 30
        }
    }
    if (-not $dns) {
        Write-Output 'DNS resolution of __PACKAGE_HOST__ failed after retries'
        Stop-Transcript
        exit 1
    }

    $zipPath = 'C:\__PACKAGE_NAME__.zip'
    $dl = $false
    for ($i = 0; $i -lt 10; $i++) {
        try {
            Start-BitsTransfer -Source '__PACKAGE_URL__' -Destination $zipPath -ErrorAction Stop
            $dl = $true
            break
        } catch {
            Write-Output "Download attempt failed: $($_.Exception.Message)"
            Start-Sleep -Seconds 30
        }
    }
    if (-not $dl) {
        Write-Output 'Package download failed'
        Stop-Transcript
        exit 1
    }
    $zipFile = Get-Item $zipPath
}

Write-Output "Extracting $($zipFile.Name)..."
Expand-Archive -Path $zipFile.FullName -DestinationPath 'C:\__PACKAGE_NAME__' -Force
Remove-Item $zipFile.FullName -Force
Write-Output 'Package extracted successfully'

if (Test-Path 'C:\__PACKAGE_NAME__\DSC\Scripts\Set-ConfigCredential.ps1') {
    Write-Output 'Injecting credential into Config.json...'
    & 'C:\__PACKAGE_NAME__\DSC\Scripts\Set-ConfigCredential.ps1' -PasswordBase64 '__PASSWORD_B64__'
}

if (Test-Path 'C:\__PACKAGE_NAME__\DSC\__DEPLOY_SCRIPT__') {
    Write-Output 'Starting __DEPLOY_SCRIPT__ (DSC + imperative)...'
    Set-Location 'C:\__PACKAGE_NAME__\DSC'
    & '.\__DEPLOY_SCRIPT__' -WorkingPath 'C:\__PACKAGE_NAME__'
} else {
    Write-Output '__DEPLOY_SCRIPT__ not found, skipping configuration'
    Stop-Transcript
    exit 1
}

Write-Output '__SCENARIO__ __ROLE__ extension setup completed'
Stop-Transcript
# Explicit success code: the launcher propagates $LASTEXITCODE, which would
# otherwise carry whatever the last native tool inside the deploy script returned.
exit 0
