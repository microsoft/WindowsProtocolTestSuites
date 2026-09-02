# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$AutomationRoot,

    [Parameter(Mandatory)]
    [uri]$FileServerAssetUrl,

    [Parameter(Mandatory)]
    [ValidatePattern('^[A-Fa-f0-9]{64}$')]
    [string]$FileServerAssetSha256,

    [string]$FileServerAssetVersion = '',

    [Parameter(Mandatory)]
    [uri]$PtmServiceAssetUrl,

    [Parameter(Mandatory)]
    [ValidatePattern('^[A-Fa-f0-9]{64}$')]
    [string]$PtmServiceAssetSha256,

    [string]$PtmServiceAssetVersion = '',

    [Parameter(Mandatory)]
    [uri]$PtmCliAssetUrl,

    [Parameter(Mandatory)]
    [ValidatePattern('^[A-Fa-f0-9]{64}$')]
    [string]$PtmCliAssetSha256,

    [string]$PtmCliAssetVersion = ''
)

$ErrorActionPreference = 'Stop'

foreach ($url in @($FileServerAssetUrl, $PtmServiceAssetUrl, $PtmCliAssetUrl)) {
    if ($url.Scheme -ne 'https') {
        throw "Release asset URLs must use HTTPS: $url"
    }
}

function Set-PropertyValue {
    param(
        [Parameter(Mandatory)]
        [psobject]$Object,

        [Parameter(Mandatory)]
        [string]$Name,

        [Parameter(Mandatory)]
        [string]$Value
    )

    $property = $Object.PSObject.Properties[$Name]
    if ($property) {
        $property.Value = $Value
    }
    else {
        Add-Member -InputObject $Object -NotePropertyName $Name -NotePropertyValue $Value
    }
}

$assetPins = @{
    FileServer = @{
        Url = $FileServerAssetUrl.AbsoluteUri
        SHA256 = $FileServerAssetSha256.ToLowerInvariant()
        Version = $FileServerAssetVersion
    }
    PTMService = @{
        Url = $PtmServiceAssetUrl.AbsoluteUri
        SHA256 = $PtmServiceAssetSha256.ToLowerInvariant()
        Version = $PtmServiceAssetVersion
    }
    PTMCli = @{
        Url = $PtmCliAssetUrl.AbsoluteUri
        SHA256 = $PtmCliAssetSha256.ToLowerInvariant()
        Version = $PtmCliAssetVersion
    }
}

$scenarios = @('workgroup-bicep', 'domain-bicep', 'cluster-bicep')
foreach ($scenario in $scenarios) {
    $toolsPath = Join-Path $AutomationRoot "$scenario\DSC\Scripts\Tools.json"
    if (-not (Test-Path -LiteralPath $toolsPath -PathType Leaf)) {
        throw "Tools.json was not found for $scenario at $toolsPath"
    }

    $configuration = Get-Content -LiteralPath $toolsPath -Raw | ConvertFrom-Json
    $counts = @{
        FileServer = 0
        PTMService = 0
        PTMCli = 0
    }

    foreach ($role in $configuration.PSObject.Properties) {
        foreach ($tool in @($role.Value.Tools)) {
            if ($tool.name -in @('PTMService', 'PTMCli')) {
                $pin = $assetPins[$tool.name]
                Set-PropertyValue -Object $tool -Name Url -Value $pin.Url
                Set-PropertyValue -Object $tool -Name SHA256 -Value $pin.SHA256
                if ($pin.Version) {
                    Set-PropertyValue -Object $tool -Name version -Value $pin.Version
                }
                $counts[$tool.name]++
            }
        }

        foreach ($testSuite in @($role.Value.TestsuiteZips)) {
            if ($testSuite.ZipName -eq 'FileServer-TestSuite-ServerEP.zip') {
                $pin = $assetPins.FileServer
                Set-PropertyValue -Object $testSuite -Name Url -Value $pin.Url
                Set-PropertyValue -Object $testSuite -Name SHA256 -Value $pin.SHA256
                if ($pin.Version) {
                    Set-PropertyValue -Object $testSuite -Name version -Value $pin.Version
                }
                $counts.FileServer++
            }
        }
    }

    foreach ($assetName in $counts.Keys) {
        if ($counts[$assetName] -eq 0) {
            throw "$scenario does not contain a $assetName release asset entry."
        }
    }

    $configuration | ConvertTo-Json -Depth 20 |
        Set-Content -LiteralPath $toolsPath -Encoding UTF8
    Write-Host "Pinned release assets in $toolsPath"
}
