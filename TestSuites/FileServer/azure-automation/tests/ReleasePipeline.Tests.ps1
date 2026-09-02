# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Describe 'FileServer release pipeline composition' {
    BeforeAll {
        $repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..')).Path
        $oneClickPipeline = Get-Content -LiteralPath (
            Join-Path $repositoryRoot 'pipelines\1es\FileServer-OneClick-Release.yml'
        ) -Raw
        $orchestratorPipeline = Get-Content -LiteralPath (
            Join-Path $repositoryRoot 'pipelines\1es\FileServer-Release-Orchestrator.yml'
        ) -Raw
        $packageTemplate = Get-Content -LiteralPath (
            Join-Path $repositoryRoot 'pipelines\1es\templates\FileServer-OneClick-Package-Steps.yml'
        ) -Raw
    }

    It 'reuses the same OneClick packaging template in both release pipelines' {
        $templatePath = '/pipelines/1es/templates/FileServer-OneClick-Package-Steps.yml@self'
        if (-not $oneClickPipeline.Contains($templatePath) -or
            -not $orchestratorPipeline.Contains($templatePath)) {
            throw 'Both release pipelines must use the shared OneClick packaging template.'
        }
    }

    It 'pins codesign and test-suite commits in the orchestrator' {
        foreach ($requiredText in @(
            'Invoke-FileServerCodeSign.ps1',
            '-TestSuitesCommit "${{ parameters.testSuitesCommit }}"',
            '-CodeSignCommit "${{ parameters.codeSignCommit }}"',
            '-ExistingCodeSignBuildId "${{ parameters.existingCodeSignBuildId }}"',
            'codeSignCommit must be a full 40-character commit SHA',
            'FileServer-Release-Bundle'
        )) {
            if (-not $orchestratorPipeline.Contains($requiredText)) {
                throw "The orchestrator is missing '$requiredText'."
            }
        }
    }

    It 'updates staged versions and includes every ZIP in checksums' {
        foreach ($requiredText in @(
            '-FileServerAssetVersion',
            '-PtmServiceAssetVersion',
            '-PtmCliAssetVersion',
            "Get-ChildItem `$drop -Filter '*.zip'"
        )) {
            if (-not $packageTemplate.Contains($requiredText)) {
                throw "The packaging template is missing '$requiredText'."
            }
        }
    }
}
