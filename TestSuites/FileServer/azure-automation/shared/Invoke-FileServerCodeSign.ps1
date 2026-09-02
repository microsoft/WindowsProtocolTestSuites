# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[CmdletBinding()]
param(
    [string]$AccessToken = $env:SYSTEM_ACCESSTOKEN,

    [Parameter(Mandatory)]
    [uri]$CollectionUri,

    [Parameter(Mandatory)]
    [string]$Project,

    [int]$CodeSignDefinitionId = 56330,

    [ValidateRange(0, [int]::MaxValue)]
    [int]$ExistingCodeSignBuildId = 0,

    [Parameter(Mandatory)]
    [string]$TestSuitesBranch,

    [Parameter(Mandatory)]
    [ValidatePattern('^[0-9a-fA-F]{40}$')]
    [string]$TestSuitesCommit,

    [string]$CodeSignBranch = 'main',

    [ValidatePattern('^$|^[0-9a-fA-F]{40}$')]
    [string]$CodeSignCommit = '',

    [Parameter(Mandatory)]
    [uri]$FileServerAssetUrl,

    [Parameter(Mandatory)]
    [string]$FileServerAssetVersion,

    [Parameter(Mandatory)]
    [string]$SignerSubject,

    [Parameter(Mandatory)]
    [string]$OutputDirectory,

    [ValidateRange(1, 360)]
    [int]$TimeoutMinutes = 180,

    [ValidateRange(1, 300)]
    [int]$PollIntervalSeconds = 30
)

$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($AccessToken)) {
    throw 'An Azure DevOps access token is required through SYSTEM_ACCESSTOKEN.'
}
if ($FileServerAssetUrl.Scheme -ne 'https') {
    throw "The FileServer release asset URL must use HTTPS: $FileServerAssetUrl"
}

function Get-NormalizedBranch {
    param([Parameter(Mandatory)][string]$Branch)

    if ($Branch.StartsWith('refs/')) {
        return $Branch
    }
    return "refs/heads/$Branch"
}

$tokenBytes = [System.Text.Encoding]::UTF8.GetBytes(":$AccessToken")
$authString = [System.Convert]::ToBase64String($tokenBytes)
$headers = @{ Authorization = "Basic $authString" }
$collection = $CollectionUri.AbsoluteUri.TrimEnd('/') + '/'
$escapedProject = [uri]::EscapeDataString($Project)
$buildApi = "${collection}${escapedProject}/_apis/build"
$testSuitesCommit = $TestSuitesCommit.ToLowerInvariant()
$codeSignCommit = $CodeSignCommit.ToLowerInvariant()

$buildId = $ExistingCodeSignBuildId
if ($buildId -eq 0) {
    $queueBody = [ordered]@{
        definition = @{ id = $CodeSignDefinitionId }
        sourceBranch = Get-NormalizedBranch -Branch $CodeSignBranch
        parameters = (@{
            'build.testSuiteName' = 'FileServer'
            'build.testSuitesBranch' = $TestSuitesBranch
        } | ConvertTo-Json -Compress)
        templateParameters = @{
            testSuitesCommit = $testSuitesCommit
        }
    }
    if ($codeSignCommit) {
        $queueBody.sourceVersion = $codeSignCommit
    }

    $queueUrl = "$buildApi/builds?api-version=7.1"
    $queuedBuild = Invoke-RestMethod -Uri $queueUrl -Method Post -Headers $headers `
        -ContentType 'application/json' -Body ($queueBody | ConvertTo-Json -Depth 10) `
        -ErrorAction Stop
    if ([int]$queuedBuild.id -le 0) {
        throw 'The FileServer codesign pipeline did not return a valid build ID.'
    }
    if ($codeSignCommit -and
        "$($queuedBuild.sourceVersion)".ToLowerInvariant() -ne $codeSignCommit) {
        throw "Codesign build $($queuedBuild.id) did not queue helper commit '$codeSignCommit'."
    }
    $buildId = [int]$queuedBuild.id
}

$buildUrl = '{0}/builds/{1}?api-version=7.1' -f $buildApi, $buildId
$build = Invoke-RestMethod -Uri $buildUrl -Method Get -Headers $headers `
    -ErrorAction Stop
if ($ExistingCodeSignBuildId -gt 0 -and "$($build.status)" -ne 'completed') {
    throw "Existing codesign build $buildId is not completed."
}
if ($ExistingCodeSignBuildId -eq 0) {
    $deadline = [DateTime]::UtcNow.AddMinutes($TimeoutMinutes)
    while ("$($build.status)" -ne 'completed') {
        if ([DateTime]::UtcNow -ge $deadline) {
            throw "Codesign build $buildId did not complete within $TimeoutMinutes minutes."
        }
        Start-Sleep -Seconds $PollIntervalSeconds
        $build = Invoke-RestMethod -Uri $buildUrl -Method Get -Headers $headers `
            -ErrorAction Stop
    }
}

if ("$($build.status)" -ne 'completed') {
    throw "Codesign build $buildId has status '$($build.status)', not 'completed'."
}
if ("$($build.result)" -ne 'succeeded') {
    throw "Codesign build $buildId completed with result '$($build.result)'."
}
if ([int]$build.definition.id -ne $CodeSignDefinitionId) {
    throw "Codesign build $buildId belongs to definition '$($build.definition.id)', not '$CodeSignDefinitionId'."
}
if ($codeSignCommit -and
    "$($build.sourceVersion)".ToLowerInvariant() -ne $codeSignCommit) {
    throw "Codesign build $buildId used helper commit '$($build.sourceVersion)', not '$codeSignCommit'."
}

$artifactsUrl = "$buildApi/builds/$buildId/artifacts?api-version=7.1"
$artifacts = Invoke-RestMethod -Uri $artifactsUrl -Method Get -Headers $headers `
    -ErrorAction Stop
$dropArtifact = @($artifacts.value | Where-Object {
    $_.name -eq 'drop' -and $_.resource.type -eq 'Container'
})
if ($dropArtifact.Count -ne 1) {
    throw "Codesign build $buildId did not publish exactly one final 'drop' container artifact."
}

$containerData = "$($dropArtifact[0].resource.data)"
if ($containerData -notmatch '^#/(?<Id>[0-9]+)/(?<Path>.+)$') {
    throw "Codesign build $buildId returned invalid drop container data '$containerData'."
}
$containerId = $Matches.Id
$containerPath = $Matches.Path
$escapedContainerPath = [uri]::EscapeDataString($containerPath)
$containerUrl = '{0}_apis/resources/Containers/{1}?itemPath={2}&api-version=7.1-preview.4' -f `
    $collection, $containerId, $escapedContainerPath
$containerItems = Invoke-RestMethod -Uri $containerUrl -Method Get -Headers $headers `
    -ErrorAction Stop

$archiveName = 'FileServer-TestSuite-ServerEP'
$requiredFiles = @(
    "$containerPath/$archiveName.zip",
    "$containerPath/$archiveName.provenance.json"
)
$itemsByPath = @{}
foreach ($item in @($containerItems.value)) {
    $itemsByPath["$($item.path)"] = $item
}
foreach ($requiredFile in $requiredFiles) {
    if (-not $itemsByPath.ContainsKey($requiredFile)) {
        throw "Codesign build $buildId artifact is missing '$requiredFile'."
    }
}

New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$archivePath = Join-Path $OutputDirectory "$archiveName.zip"
$provenancePath = Join-Path $OutputDirectory "$archiveName.provenance.json"
$collectionOrigin = [uri]$collection
function Invoke-ArtifactDownload {
    param(
        [Parameter(Mandatory)]
        [uri]$Uri,

        [Parameter(Mandatory)]
        [string]$OutFile
    )

    $arguments = @{
        Uri = $Uri
        OutFile = $OutFile
        ErrorAction = 'Stop'
    }
    if ($Uri.Scheme -eq $collectionOrigin.Scheme -and
        $Uri.Host -eq $collectionOrigin.Host -and
        $Uri.Port -eq $collectionOrigin.Port) {
        $arguments.Headers = $headers
    }
    Invoke-WebRequest @arguments
}

Invoke-ArtifactDownload -Uri $itemsByPath[$requiredFiles[0]].contentLocation `
    -OutFile $archivePath
Invoke-ArtifactDownload -Uri $itemsByPath[$requiredFiles[1]].contentLocation `
    -OutFile $provenancePath

$provenance = Get-Content -LiteralPath $provenancePath -Raw |
    ConvertFrom-Json -ErrorAction Stop
$archiveHash = (Get-FileHash -LiteralPath $archivePath -Algorithm SHA256).Hash.ToLowerInvariant()
if ("$($provenance.Archive.SHA256)".ToLowerInvariant() -ne $archiveHash) {
    throw 'The downloaded signed FileServer ZIP does not match its provenance SHA-256.'
}
if ("$($provenance.SourceBuild.SourceCommit)".ToLowerInvariant() -ne $testSuitesCommit) {
    throw "The signed FileServer ZIP came from '$($provenance.SourceBuild.SourceCommit)', not '$testSuitesCommit'."
}
if ([int]$provenance.SigningBuild.BuildId -ne $buildId) {
    throw "The provenance signing build ID does not match codesign build $buildId."
}
if ("$($provenance.SigningBuild.ExpectedSignerSubject)" -ne $SignerSubject) {
    throw 'The provenance signer subject does not match the requested signer.'
}

$verificationRoot = Join-Path $env:TEMP "FileServerSignedArchive-$([guid]::NewGuid().ToString('N'))"
try {
    Expand-Archive -LiteralPath $archivePath -DestinationPath $verificationRoot
    $versionPath = Join-Path $verificationRoot 'Bin\.version'
    if (-not (Test-Path -LiteralPath $versionPath -PathType Leaf)) {
        throw 'The signed FileServer ZIP does not contain Bin\.version.'
    }
    $archiveVersion = (Get-Content -LiteralPath $versionPath -Raw).Trim()
    if ($archiveVersion -ne $FileServerAssetVersion) {
        throw "The signed FileServer version '$archiveVersion' does not match '$FileServerAssetVersion'."
    }
}
finally {
    Remove-Item -LiteralPath $verificationRoot -Recurse -Force -ErrorAction SilentlyContinue
}

$summary = [ordered]@{
    SchemaVersion = '1.0'
    GeneratedUtc = [DateTime]::UtcNow.ToString('o')
    FileServerAsset = [ordered]@{
        Version = $FileServerAssetVersion
        Url = $FileServerAssetUrl.AbsoluteUri
        SHA256 = $archiveHash
        FileName = "$archiveName.zip"
        VerifiedVersion = $archiveVersion
        ExpectedSignerSubject = $SignerSubject
    }
    TestSuitesSource = [ordered]@{
        Branch = $TestSuitesBranch
        Commit = $testSuitesCommit
    }
    CodeSignBuild = [ordered]@{
        DefinitionId = $CodeSignDefinitionId
        BuildId = $buildId
        BuildNumber = "$($build.buildNumber)"
        SourceBranch = "$($build.sourceBranch)"
        SourceCommit = "$($build.sourceVersion)".ToLowerInvariant()
        Reused = $ExistingCodeSignBuildId -gt 0
        Url = "$($build._links.web.href)"
    }
}
$summaryPath = Join-Path $OutputDirectory 'FileServer-Release-Orchestration.json'
$summary | ConvertTo-Json -Depth 10 |
    Set-Content -LiteralPath $summaryPath -Encoding UTF8

Write-Host "##vso[task.setvariable variable=FileServerAssetSha256]$archiveHash"
Write-Host "##vso[task.setvariable variable=FileServerCodeSignBuildId]$buildId"
Write-Host "Verified signed FileServer ZIP from codesign build $buildId."
Write-Host "Signed FileServer ZIP SHA-256: $archiveHash"
