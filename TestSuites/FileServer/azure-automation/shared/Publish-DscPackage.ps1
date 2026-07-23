# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Builds a scenario DSC package (Workgroup / Domain / Cluster) and publishes it as
    a GitHub Release asset so the "Deploy to Azure" button can consume it with no
    local pre-step. Shared implementation invoked by each scenario's thin wrapper.

.DESCRIPTION
    The Azure Portal "Deploy to Azure" button has no place to run the local
    Build-DscPackage step that deploy.ps1 performs. This script produces the same
    package once, ahead of time, and publishes it as a versioned GitHub Release
    asset that the scenario's main.bicep defaults its package-URL parameter to.

    Hosting model (SFI-friendly -- no anonymous Azure Storage):
      * DSC package (a build binary, not committed) -> GitHub Release asset:
          https://github.com/<Repo>/releases/download/<Tag>/<AssetName>
      * ARM template -> the committed azuredeploy.json is served directly via
        raw.githubusercontent.com at the SAME tag. The button points there, so the
        committed file IS the hosted template (no separate upload, no Bicep->JSON
        drift). Pinning both to the same <Tag> keeps template and package in lockstep.

    SECURITY: the published package is PUBLIC, so it must never contain real
    credentials. Config.json is baked with the default IP topology but with a
    placeholder password token (-PasswordToken); the bicep Custom Script Extension
    substitutes the real @secure() adminPassword into Config.json ON the VM at deploy
    time, from protectedSettings. ParamConfig.json (test-account credentials) is
    stripped from the public asset and re-fetched on-VM. ResultsUpload.json is
    omitted (it would embed a write SAS token).

.PARAMETER Scenario
    Deployment scenario: Workgroup, Domain, or Cluster. Passed to New-DscPackageZip.

.PARAMETER ConfigJsonParams
    Hashtable of scenario-specific parameters splatted to Generate-ConfigJson.ps1
    (built by the calling wrapper; password fields must be set to -PasswordToken).

.PARAMETER DscFolderPath
    Absolute path to the scenario-specific DSC folder.

.PARAMETER MainBicepPath
    Absolute path to the scenario main.bicep, used for the asset-URL drift check.

.PARAMETER PackageUrlParamName
    Name of the main.bicep parameter holding the package URL (e.g. dscPackageZipUrl
    for Workgroup, domainPackageZipUrl for Domain). Used for the drift check.

.PARAMETER Tag
    GitHub release tag. Both the package asset and the raw template URL pin to it.

.PARAMETER Target
    Commit-ish the release tag should point at (branch name or SHA).

.PARAMETER SkipUpload
    Build the zip and print the target URLs without touching GitHub.
#>
[CmdletBinding(SupportsShouldProcess = $true)]
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'PasswordToken',
    Justification = 'Not a credential -- a placeholder token baked into the public Config.json and replaced on-VM')]
param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('Workgroup', 'Domain', 'Cluster')]
    [string]$Scenario,

    [Parameter(Mandatory = $true)]
    [hashtable]$ConfigJsonParams,

    [Parameter(Mandatory = $true)]
    [string]$DscFolderPath,

    [Parameter(Mandatory = $true)]
    [string]$MainBicepPath,

    [Parameter(Mandatory = $true)]
    [string]$AssetName,

    [Parameter(Mandatory = $true)]
    [string]$TemplateRepoPath,

    [Parameter(Mandatory = $false)]
    [string]$PackageUrlParamName = 'dscPackageZipUrl',

    [Parameter(Mandatory = $false)]
    [string]$Repo = "microsoft/WindowsProtocolTestSuites",

    [Parameter(Mandatory = $true)]
    [string]$Tag,

    [Parameter(Mandatory = $false)]
    [string]$Target = "",

    [Parameter(Mandatory = $false)]
    [string]$PasswordToken = "#{ADMIN_PASSWORD}#",

    [Parameter(Mandatory = $false)]
    [string]$OutputZipPath = "",

    [Parameter(Mandatory = $false)]
    [switch]$SkipUpload
)

$ErrorActionPreference = "Stop"

# --- Resolve paths (this script lives in shared/) ----------------------------
$helperModule = Join-Path $PSScriptRoot "Deploy-Helpers.psm1"
$generateScript = Join-Path $PSScriptRoot "Generate-ConfigJson.ps1"
$sharedDscPath = Join-Path $PSScriptRoot "DSC"

foreach ($p in @($helperModule, $generateScript, $sharedDscPath, $DscFolderPath, $MainBicepPath)) {
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
 Publish $Scenario DSC Package (GitHub Release)
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
# template's package URL doesn't match the asset actually published). Compare this
# run's asset URL against main.bicep's committed default for $PackageUrlParamName.
$m = Select-String -Path $MainBicepPath -Pattern "param $PackageUrlParamName string = '([^']+)'" |
    Select-Object -First 1
if ($m) {
    $bakedUrl = $m.Matches[0].Groups[1].Value
    if ($bakedUrl -eq $packageUrl) {
        Write-Output "[OK] main.bicep default $PackageUrlParamName matches this asset URL.`n"
    } else {
        Write-Warning ("main.bicep's $PackageUrlParamName default is:`n    $bakedUrl`n" +
            "but this run publishes:`n    $packageUrl`n" +
            "The deployed template must reference the asset you publish. For a real wpts " +
            "release, update main.bicep + azuredeploy.json to this URL; for a test repo, " +
            "override the package URL at deploy time or edit that repo's azuredeploy.json.`n")
    }
}

# --- Build the package -------------------------------------------------------
if (-not $OutputZipPath) {
    $OutputZipPath = Join-Path $env:TEMP "$Scenario-Package-$Tag.zip"
}
$OutputZipPath = if ([System.IO.Path]::IsPathRooted($OutputZipPath)) {
    $OutputZipPath
} else {
    Join-Path (Get-Location) $OutputZipPath
}

Write-Output "`nAssembling package (no ResultsUpload.json -- public package)..."
New-DscPackageZip `
    -DscFolderPath $DscFolderPath `
    -SharedDscPath $sharedDscPath `
    -Scenario $Scenario `
    -ConfigJsonParams $ConfigJsonParams `
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
if ([string]::IsNullOrEmpty($configLeakCheck)) {
    throw "SAFETY: Config.json in the package is empty/unreadable. Refusing to publish."
}
if ($configLeakCheck -notmatch [regex]::Escape($PasswordToken)) {
    throw "SAFETY: Config.json in the package does not contain the placeholder token '$PasswordToken'. Refusing to publish -- it may contain a real credential."
}

# The token-presence check above is necessary but not sufficient: a second password-bearing
# field could hold a real credential while the token appears elsewhere. Parse Config.json and
# reject any password/passphrase field whose value is neither the placeholder nor empty. Fail
# closed -- any parse failure or suspicious value aborts publishing the public asset.
try {
    $configObj = $configLeakCheck | ConvertFrom-Json -ErrorAction Stop
} catch {
    throw "SAFETY: Config.json is not valid JSON. Refusing to publish. Details: $($_.Exception.Message)"
}
$script:passwordFieldOffenders = New-Object 'System.Collections.Generic.List[string]'
$script:passwordPlaceholderSeen = $false
function Test-ConfigPasswordFields {
    param([object]$Obj, [string]$Path = '')
    if ($null -eq $Obj) { return }
    if ($Obj -is [pscustomobject]) {
        foreach ($prop in $Obj.PSObject.Properties) {
            $propPath = if ([string]::IsNullOrEmpty($Path)) { $prop.Name } else { "$Path.$($prop.Name)" }
            if ($prop.Name -match '(?i)(password|passphrase)$') {
                $value = $prop.Value
                if ($null -eq $value -or $value -eq '') {
                    # empty/null password fields are allowed in the public package
                } elseif ($value -is [string] -and $value -eq $PasswordToken) {
                    $script:passwordPlaceholderSeen = $true
                } else {
                    $script:passwordFieldOffenders.Add("$propPath='$value'")
                }
            }
            Test-ConfigPasswordFields -Obj $prop.Value -Path $propPath
        }
    } elseif (($Obj -is [System.Collections.IEnumerable]) -and -not ($Obj -is [string])) {
        $i = 0
        foreach ($item in $Obj) { Test-ConfigPasswordFields -Obj $item -Path "$Path[$i]"; $i++ }
    }
}
Test-ConfigPasswordFields -Obj $configObj
if ($script:passwordFieldOffenders.Count -gt 0) {
    throw "SAFETY: Config.json has password-bearing field(s) with non-placeholder values. Refusing to publish:`n  $($script:passwordFieldOffenders -join "`n  ")"
}
if (-not $script:passwordPlaceholderSeen) {
    throw "SAFETY: No password-bearing field in Config.json is set to the placeholder token '$PasswordToken'. Refusing to publish."
}
Write-Output "[OK] Verified Config.json password fields are placeholder/empty only (no baked credential)."

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
            '--title', "FileServer $Scenario Deploy-to-Azure package ($Tag)",
            '--notes', "Pre-built, credential-free $Scenario DSC package for the one-click Deploy to Azure button. Template served from azuredeploy.json at this tag.")
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
    # matches $packageUrl (a stable name, not the tag-suffixed build artifact name).
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
    Write-Output "Next: ensure the $PackageUrlParamName default in main.bicep + azuredeploy.json and the README button URL all reference tag '$Tag'."
}

return $packageUrl
