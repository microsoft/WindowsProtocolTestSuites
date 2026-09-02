# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$root = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$scriptPath = Join-Path $root 'shared\Invoke-FileServerCodeSign.ps1'

Describe 'FileServer release codesign orchestration' {
    BeforeEach {
        $testRoot = Join-Path $env:TEMP "FileServerRelease-$([guid]::NewGuid().ToString('N'))"
        $sourceCommit = 'a' * 40
        $helperCommit = 'b' * 40
        $global:queueBody = $null
        $global:downloadHeaders = @()

        Mock Start-Sleep {}
        Mock Expand-Archive {
            New-Item -ItemType Directory -Path (Join-Path $DestinationPath 'Bin') -Force |
                Out-Null
            Set-Content -LiteralPath (Join-Path $DestinationPath 'Bin\.version') `
                -Value '1.0'
            Set-Content -LiteralPath (Join-Path $DestinationPath 'install.ps1') `
                -Value 'Write-Host test'
        }
        Mock Invoke-RestMethod {
            if ($Method -eq 'Post') {
                $global:queueBody = $Body
                return [pscustomobject]@{
                    id = 99
                    sourceVersion = $helperCommit
                }
            }
            if ($Uri -match '/build/builds/99\?') {
                return [pscustomobject]@{
                    id = 99
                    buildNumber = '20260902.2'
                    status = 'completed'
                    result = 'succeeded'
                    definition = [pscustomobject]@{ id = 56330 }
                    sourceBranch = 'refs/heads/main'
                    sourceVersion = $helperCommit
                    _links = [pscustomobject]@{
                        web = [pscustomobject]@{ href = 'https://example.test/build/99' }
                    }
                }
            }
            if ($Uri -match '/build/builds/99/artifacts\?') {
                return [pscustomobject]@{
                    value = @([pscustomobject]@{
                        name = 'drop'
                        resource = [pscustomobject]@{
                            type = 'Container'
                            data = '#/123/drop'
                        }
                    })
                }
            }
            if ($Uri -match '/_apis/resources/Containers/123\?') {
                return [pscustomobject]@{
                    value = @(
                        [pscustomobject]@{
                            path = 'drop/FileServer-TestSuite-ServerEP.zip'
                            contentLocation = 'https://example.test/archive'
                        },
                        [pscustomobject]@{
                            path = 'drop/FileServer-TestSuite-ServerEP.provenance.json'
                            contentLocation = 'https://example.test/provenance'
                        }
                    )
                }
            }
            throw "Unexpected REST request: $Method $Uri"
        }
        Mock Invoke-WebRequest {
            $global:downloadHeaders += $Headers
            if ($Uri -eq 'https://example.test/archive') {
                Set-Content -LiteralPath $OutFile -Value 'signed archive'
                return
            }
            if ($Uri -eq 'https://example.test/provenance') {
                $archivePath = Join-Path (Split-Path $OutFile -Parent) `
                    'FileServer-TestSuite-ServerEP.zip'
                $hash = (Get-FileHash -LiteralPath $archivePath -Algorithm SHA256).Hash.ToLowerInvariant()
                [ordered]@{
                    Archive = @{ SHA256 = $hash }
                    SourceBuild = @{ SourceCommit = $sourceCommit }
                    SigningBuild = @{
                        BuildId = 99
                        ExpectedSignerSubject = 'CN=Microsoft Corporation'
                    }
                } | ConvertTo-Json -Depth 5 |
                    Set-Content -LiteralPath $OutFile
                return
            }
            throw "Unexpected download request: $Uri"
        }
    }

    AfterEach {
        Remove-Item -LiteralPath $testRoot -Recurse -Force -ErrorAction SilentlyContinue
        Remove-Variable queueBody -Scope Global -ErrorAction SilentlyContinue
        Remove-Variable downloadHeaders -Scope Global -ErrorAction SilentlyContinue
    }

    It 'pins the codesign and test-suite commits and verifies the signed artifact' {
        & $scriptPath -AccessToken token -CollectionUri 'https://dev.azure.com/example/' `
            -Project Project -TestSuitesBranch release -TestSuitesCommit $sourceCommit `
            -CodeSignBranch main -CodeSignCommit $helperCommit `
            -FileServerAssetUrl 'https://github.com/example/repo/releases/download/1.0/FileServer-TestSuite-ServerEP.zip' `
            -FileServerAssetVersion '1.0' -SignerSubject 'CN=Microsoft Corporation' `
            -OutputDirectory $testRoot `
            -PollIntervalSeconds 1

        $request = $global:queueBody | ConvertFrom-Json
        $variables = $request.parameters | ConvertFrom-Json
        if ($request.sourceVersion -ne $helperCommit) {
            throw 'The helper source commit was not pinned.'
        }
        if ($request.templateParameters.testSuitesCommit -ne $sourceCommit) {
            throw 'The test-suite source commit was not pinned.'
        }
        if ($variables.'build.testSuiteName' -ne 'FileServer' -or
            $variables.'build.testSuitesBranch' -ne 'release') {
            throw 'The FileServer codesign variables are incorrect.'
        }

        $summary = Get-Content -LiteralPath (
            Join-Path $testRoot 'FileServer-Release-Orchestration.json'
        ) -Raw | ConvertFrom-Json
        if ($summary.TestSuitesSource.Commit -ne $sourceCommit) {
            throw 'The orchestration summary source commit is incorrect.'
        }
        if ($summary.CodeSignBuild.BuildId -ne 99) {
            throw 'The orchestration summary codesign build ID is incorrect.'
        }
        if ($summary.FileServerAsset.VerifiedVersion -ne '1.0') {
            throw 'The signed FileServer archive version was not verified.'
        }
        if (-not (Test-Path -LiteralPath (
            Join-Path $testRoot 'FileServer-TestSuite-ServerEP.zip'
        ))) {
            throw 'The signed FileServer ZIP was not downloaded.'
        }
        if (@($global:downloadHeaders | Where-Object { $null -ne $_ }).Count -ne 0) {
            throw 'The authorization header was sent to a cross-origin artifact URL.'
        }
    }

    It 'reuses a completed codesign build without queueing another build' {
        & $scriptPath -AccessToken token -CollectionUri 'https://dev.azure.com/example/' `
            -Project Project -ExistingCodeSignBuildId 99 `
            -TestSuitesBranch release -TestSuitesCommit $sourceCommit `
            -CodeSignBranch main -CodeSignCommit $helperCommit `
            -FileServerAssetUrl 'https://github.com/example/repo/releases/download/1.0/FileServer-TestSuite-ServerEP.zip' `
            -FileServerAssetVersion '1.0' -SignerSubject 'CN=Microsoft Corporation' `
            -OutputDirectory $testRoot

        if ($null -ne $global:queueBody) {
            throw 'Reusing a codesign build must not queue another build.'
        }
        $summary = Get-Content -LiteralPath (
            Join-Path $testRoot 'FileServer-Release-Orchestration.json'
        ) -Raw | ConvertFrom-Json
        if ($summary.CodeSignBuild.BuildId -ne 99 -or
            -not $summary.CodeSignBuild.Reused) {
            throw 'The reused codesign build was not recorded correctly.'
        }
    }
}
