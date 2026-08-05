# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Injects the real credential into a publicly published Config.json on the VM.

.DESCRIPTION
    The "Deploy to Azure" button consumes a PUBLIC DSC package whose Config.json
    ships with a placeholder password token instead of a real credential (public
    packages must never embed secrets). This script runs on the VM, from the
    Custom Script Extension's protectedSettings (encrypted, never returned by ARM),
    and replaces every occurrence of the token with the real password.

    The password is passed BASE64-ENCODED so it survives cmd.exe / bash / PowerShell
    quoting unharmed regardless of which special characters it contains (base64 uses
    only [A-Za-z0-9+/=]).

    Safe no-op for the local deploy.ps1 path, where Config.json already holds the
    real password and the token is absent -- nothing is replaced.

.PARAMETER PasswordBase64
    UTF-8 password, base64-encoded. If empty, the script is a no-op.

.PARAMETER Token
    Placeholder string to replace. Must match Publish-DscPackage.ps1 -PasswordToken.

.PARAMETER ConfigPaths
    Config.json files to patch. Defaults to the package-root and DSC\Scripts copies.
#>
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'PasswordBase64',
    Justification = 'Base64-encoded transport of an already-secured password; decoded only in memory to patch Config.json')]
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [AllowEmptyString()]
    [string]$PasswordBase64,

    [Parameter(Mandatory = $false)]
    [string]$Token = '#{ADMIN_PASSWORD}#',

    [Parameter(Mandatory = $false)]
    [string[]]$ConfigPaths
)

$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrEmpty($PasswordBase64)) {
    Write-Output "Set-ConfigCredential: no password supplied; nothing to inject."
    return
}

if (-not $ConfigPaths -or $ConfigPaths.Count -eq 0) {
    $root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)  # ...\<pkg>\ (parent of DSC)
    $ConfigPaths = @(
        (Join-Path $root 'Config.json')
        (Join-Path $PSScriptRoot 'Config.json')
    )
}

$password = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($PasswordBase64))

# The token sits inside a JSON string value ("...token..."), so the replacement
# must be JSON-escaped (handles " \ and control chars) or it would corrupt the
# file. ConvertTo-Json yields a quoted, escaped string; strip the outer quotes.
$jsonQuoted = $password | ConvertTo-Json
$passwordJson = $jsonQuoted.Substring(1, $jsonQuoted.Length - 2)

$patched = 0
foreach ($path in $ConfigPaths) {
    if (-not (Test-Path $path)) {
        Write-Output "Set-ConfigCredential: skipping (not found) $path"
        continue
    }

    $content = Get-Content -Path $path -Raw
    if ($content -notmatch [regex]::Escape($Token)) {
        Write-Output "Set-ConfigCredential: token absent in $path (already real credential); skipping."
        continue
    }

    # Literal (non-regex) replacement of the token with the JSON-escaped password.
    $updated = $content.Replace($Token, $passwordJson)
    Set-Content -Path $path -Value $updated -Encoding UTF8 -NoNewline
    $patched++
    Write-Output "Set-ConfigCredential: injected credential into $path"
}

Write-Output "Set-ConfigCredential: patched $patched file(s)."
