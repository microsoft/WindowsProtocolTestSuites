# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Builds the Workgroup DSC package and publishes it as a GitHub Release asset so
    the "Deploy to Azure" button can consume it with no local pre-step.

.DESCRIPTION
    The Azure Portal "Deploy to Azure" button has no place to run the local
    Build-DscPackage step that deploy.ps1 performs. This script produces the same
    package once, ahead of time, and publishes it as a versioned GitHub Release
    asset that main.bicep defaults `dscPackageZipUrl` to.

    Hosting model (SFI-friendly -- no anonymous Azure Storage):
      * DSC package (a build binary, not committed) -> GitHub Release asset:
          https://github.com/<Repo>/releases/download/<Tag>/Workgroup-Package.zip
      * ARM template -> the committed azuredeploy.json is served directly via
        raw.githubusercontent.com at the SAME tag. The button points there, so the
        committed file IS the hosted template (no separate upload, no Bicep->JSON
        drift). Pinning both to the same <Tag> keeps template and package in lockstep.

    SECURITY: the published package is PUBLIC, so it must never contain real
    credentials. Config.json is baked with the default IP topology but with a
    placeholder password token (-PasswordToken). The bicep Custom Script Extension
    substitutes the real @secure() adminPassword into Config.json ON the VM, at
    deploy time, from protectedSettings (encrypted, never returned by ARM reads).
    ResultsUpload.json is intentionally omitted (it would embed a write SAS token).

.PARAMETER Tag
    GitHub release tag to publish to (e.g. 'fileserver-workgroup-deploy-button-v1').
    Both the package asset and the raw template URL are pinned to this tag. Bump it
    for every content change; old templates stay pinned to their old package.

.PARAMETER Target
    Commit-ish the release tag should point at (branch name or SHA). Must contain
    an azuredeploy.json consistent with the package. Defaults to the repo's default
    branch HEAD when omitted.

.PARAMETER SkipUpload
    Build the zip and print the target URLs without touching GitHub. Useful for
    offline validation / CI dry-runs.

.EXAMPLE
    gh auth login
    ./Publish-DscPackage.ps1 -Tag fileserver-workgroup-deploy-button-v1
    # -> https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/fileserver-workgroup-deploy-button-v1/Workgroup-Package.zip

.EXAMPLE
    ./Publish-DscPackage.ps1 -SkipUpload -OutputZipPath .\Workgroup-Package.zip
#>
[CmdletBinding(SupportsShouldProcess = $true)]
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'PasswordToken',
    Justification = 'Not a credential -- a placeholder token baked into the public Config.json and replaced on-VM')]
param(
    [Parameter(Mandatory = $false)]
    [string]$Repo = "microsoft/WindowsProtocolTestSuites",

    [Parameter(Mandatory = $false)]
    [string]$Tag = "fileserver-workgroup-deploy-button-v1",

    [Parameter(Mandatory = $false)]
    [string]$Target = "",

    # Repo-relative path of the committed template (served via raw.githubusercontent
    # at -Tag; also the file this script expects to have been compiled from main.bicep).
    [Parameter(Mandatory = $false)]
    [string]$TemplateRepoPath = "TestSuites/FileServer/azure-automation/workgroup-bicep/azuredeploy.json",

    [Parameter(Mandatory = $false)]
    [string]$AssetName = "Workgroup-Package.zip",

    [Parameter(Mandatory = $false)]
    [string]$AdminUsername = "testadmin",

    # Placeholder token baked into the public Config.json in place of every password.
    # The bicep Custom Script Extension replaces it with the real password on the VM.
    [Parameter(Mandatory = $false)]
    [string]$PasswordToken = "#{ADMIN_PASSWORD}#",

    # Default IP topology -- MUST match the main.bicep parameter defaults.
    [Parameter(Mandatory = $false)]
    [string]$SutExternal1Ip = "192.168.1.11",

    [Parameter(Mandatory = $false)]
    [string]$SutExternal2Ip = "192.168.2.11",

    [Parameter(Mandatory = $false)]
    [string]$DriverExternal1Ip = "192.168.1.111",

    [Parameter(Mandatory = $false)]
    [string]$DriverExternal2Ip = "192.168.2.111",

    [Parameter(Mandatory = $false)]
    [ValidateSet("Windows", "Linux")]
    [string]$DriverOSType = "Windows",

    [Parameter(Mandatory = $false)]
    [string]$DscFolderPath = "DSC",

    [Parameter(Mandatory = $false)]
    [string]$OutputZipPath = "",

    [Parameter(Mandatory = $false)]
    [switch]$SkipUpload
)

$ErrorActionPreference = "Stop"

# --- Resolve paths -----------------------------------------------------------
$sharedPath = Join-Path $PSScriptRoot "..\shared"
$helperModule = Join-Path $sharedPath "Deploy-Helpers.psm1"
$generateScript = Join-Path $sharedPath "Generate-ConfigJson.ps1"
$sharedDscPath = Join-Path $sharedPath "DSC"

$DscFolderPath = if ([System.IO.Path]::IsPathRooted($DscFolderPath)) {
    $DscFolderPath
} else {
    Join-Path $PSScriptRoot $DscFolderPath
}

foreach ($p in @($helperModule, $generateScript, $sharedDscPath, $DscFolderPath)) {
    if (-not (Test-Path $p)) { throw "Required path not found: $p" }
}

Import-Module $helperModule -Force

# Public consumer URLs, both pinned to $Tag.
$packageUrl = "https://github.com/$Repo/releases/download/$Tag/$AssetName"
$templateUrl = "https://raw.githubusercontent.com/$Repo/$Tag/$TemplateRepoPath"
$buttonUrl = "https://portal.azure.com/#create/Microsoft.Template/uri/" +
    [System.Uri]::EscapeDataString($templateUrl)

Write-Output @"
==================================================================
 Publish Workgroup DSC Package (GitHub Release)
==================================================================
  Repo            : $Repo
  Tag             : $Tag
  Package (asset) : $packageUrl
  Template (raw)  : $templateUrl
  Password fields : baked as placeholder token '$PasswordToken'
                    (real password injected on-VM by the CSE)
==================================================================
"@

# Consistency check (encodes a hard-won lesson: the CSE 404s when the deployed
# template's dscPackageZipUrl doesn't match the asset actually published). Compare
# this run's asset URL against main.bicep's committed default.
$mainBicep = Join-Path $PSScriptRoot 'main.bicep'
if (Test-Path $mainBicep) {
    $m = Select-String -Path $mainBicep -Pattern "param dscPackageZipUrl string = '([^']+)'" |
        Select-Object -First 1
    if ($m) {
        $bakedUrl = $m.Matches[0].Groups[1].Value
        if ($bakedUrl -eq $packageUrl) {
            Write-Output "[OK] main.bicep default dscPackageZipUrl matches this asset URL.`n"
        } else {
            Write-Warning ("main.bicep's dscPackageZipUrl default is:`n    $bakedUrl`n" +
                "but this run publishes:`n    $packageUrl`n" +
                "The deployed template must reference the asset you publish. For a real wpts " +
                "release, update main.bicep + azuredeploy.json to this URL; for a test repo, " +
                "override dscPackageZipUrl at deploy time or edit that repo's azuredeploy.json.`n")
        }
    }
}

# --- Build the package -------------------------------------------------------
if (-not $OutputZipPath) {
    $OutputZipPath = Join-Path $env:TEMP "Workgroup-Package-$Tag.zip"
}
$OutputZipPath = if ([System.IO.Path]::IsPathRooted($OutputZipPath)) {
    $OutputZipPath
} else {
    Join-Path (Get-Location) $OutputZipPath
}

$configJsonParams = @{
    Scenario          = 'Workgroup'
    AdminUsername     = $AdminUsername
    AdminPassword     = $PasswordToken     # placeholder -- replaced on-VM
    LocalUserPassword = $PasswordToken     # placeholder -- replaced on-VM (reuses admin password)
    SutExternal1Ip    = $SutExternal1Ip
    SutExternal2Ip    = $SutExternal2Ip
    DriverExternal1Ip = $DriverExternal1Ip
    DriverExternal2Ip = $DriverExternal2Ip
    DriverOSType      = $DriverOSType
}

Write-Output "`nAssembling package (no ResultsUpload.json -- public package)..."
New-DscPackageZip `
    -DscFolderPath $DscFolderPath `
    -SharedDscPath $sharedDscPath `
    -Scenario 'Workgroup' `
    -ConfigJsonParams $configJsonParams `
    -GenerateConfigScript $generateScript `
    -OutputZipPath $OutputZipPath

Write-Output "[OK] Package built: $OutputZipPath"

# Public-package hardening: Deploy-Helpers.psm1 documents ParamConfig.json as
# containing test-account credentials, and Install-DscPackageAssets bakes it into
# every package. A GitHub Release asset is public + permanent, so strip it here.
# The VM re-fetches it at deploy time from the same source (Create-TestAccount.ps1),
# so functionality is unchanged. (deploy.ps1's private transient blob keeps it baked.)
try {
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zipUpd = [System.IO.Compression.ZipFile]::Open($OutputZipPath, [System.IO.Compression.ZipArchiveMode]::Update)
    try {
        $paramEntries = @($zipUpd.Entries | Where-Object { $_.FullName -match '(?i)(^|/)ParamConfig\.json$' })
        foreach ($e in $paramEntries) { $e.Delete() }
        Write-Output "[OK] Public hardening: removed $($paramEntries.Count) ParamConfig.json entr$(if ($paramEntries.Count -eq 1) {'y'} else {'ies'}) from the package."
    } finally {
        $zipUpd.Dispose()
    }
} catch {
    throw "SAFETY: failed to remove ParamConfig.json from the public package: $($_.Exception.Message)"
}

# Safety net: fail CLOSED. The package must contain a Config.json, and it must carry
# only the placeholder token (never a real credential). Any missing/unreadable
# Config.json or a leaked credential aborts the publish.
$configLeakCheck = $null
try {
    $zip = [System.IO.Compression.ZipFile]::OpenRead($OutputZipPath)
    try {
        $entry = $zip.Entries | Where-Object { $_.FullName -eq 'Config.json' } | Select-Object -First 1
        if (-not $entry) { throw "Config.json is missing from the package." }
        $stream = $null; $reader = $null
        try {
            $stream = $entry.Open()
            $reader = New-Object System.IO.StreamReader($stream)
            $configLeakCheck = $reader.ReadToEnd()
        } finally {
            if ($reader) { $reader.Dispose() }
            if ($stream) { $stream.Dispose() }
        }
    } finally {
        $zip.Dispose()
    }
} catch {
    throw "SAFETY: could not verify Config.json. Refusing to publish. Details: $($_.Exception.Message)"
}
if ([string]::IsNullOrEmpty($configLeakCheck) -or ($configLeakCheck -notmatch [regex]::Escape($PasswordToken))) {
    throw "SAFETY: Config.json in the package does not contain the placeholder token '$PasswordToken'. Refusing to publish -- it may contain a real credential."
}
Write-Output "[OK] Verified Config.json contains only the placeholder token (no baked credential)."

if ($SkipUpload) {
    Write-Output "`n-SkipUpload set. Package kept at: $OutputZipPath"
    Write-Output "Would publish asset to: $packageUrl"
    Write-Output "Deploy to Azure button URL:"
    Write-Output "  $buttonUrl"
    return $packageUrl
}

# --- Publish as a GitHub Release asset ---------------------------------------
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    throw "GitHub CLI 'gh' not found. Install it (https://cli.github.com) and run 'gh auth login' first."
}
& gh auth status 2>$null
if ($LASTEXITCODE -ne 0) { throw "Not authenticated with GitHub. Run 'gh auth login' first." }

if ($PSCmdlet.ShouldProcess($packageUrl, "Publish GitHub release asset")) {
    # Create the release if it does not already exist (idempotent).
    & gh release view $Tag --repo $Repo 2>$null | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Output "Creating release '$Tag'..."
        $createArgs = @('release', 'create', $Tag, '--repo', $Repo,
            '--title', "FileServer Workgroup Deploy-to-Azure package ($Tag)",
            '--notes', "Pre-built, credential-free Workgroup DSC package for the one-click Deploy to Azure button. Template served from azuredeploy.json at this tag.")
        if ($Target) { $createArgs += @('--target', $Target) }
        $createOut = & gh @createArgs 2>&1
        if ($LASTEXITCODE -ne 0) {
            throw "gh release create failed (exit $LASTEXITCODE):`n$($createOut -join "`n")`n" +
                  "Common causes: the repo '$Repo' has no commit on '$Target' yet (push first), the branch/SHA in -Target does not exist, or your gh token lacks 'repo' scope. Verify with: gh repo view $Repo"
        }
    } else {
        Write-Output "Release '$Tag' already exists; uploading asset (clobber)."
    }

    # gh names the release asset after the uploaded file's basename (it ignores any
    # rename), so upload from a copy named exactly $AssetName to guarantee the asset
    # matches $packageUrl (a stable Workgroup-Package.zip, not the tag-suffixed build
    # artifact name).
    $assetPath = Join-Path (Split-Path $OutputZipPath -Parent) $AssetName
    if ($assetPath -ne $OutputZipPath) { Copy-Item $OutputZipPath $assetPath -Force }

    $uploadOut = & gh release upload $Tag $assetPath --repo $Repo --clobber 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "gh release upload failed (exit $LASTEXITCODE):`n$($uploadOut -join "`n")"
    }

    Write-Output "`n[OK] Published asset: $packageUrl"
    Write-Output "Template (raw, same tag): $templateUrl"
    Write-Output "Deploy to Azure button URL:"
    Write-Output "  $buttonUrl"
    Write-Output "Next: ensure the dscPackageZipUrl default in main.bicep + azuredeploy.json and the README button URL all reference tag '$Tag'."
}

return $packageUrl
