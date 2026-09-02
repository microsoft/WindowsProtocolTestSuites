# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$automationRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$repositoryRoot = (Resolve-Path (Join-Path $automationRoot '..\..\..')).Path
$releaseVersion = (Get-Content -LiteralPath (
    Join-Path $repositoryRoot 'AssemblyInfo\.version'
) -Raw).Trim()

$scenarios = @(
    @{ Name = 'Workgroup'; Directory = 'workgroup-bicep'; Package = 'Workgroup-Package.zip'; Parameter = 'dscPackageZipUrl' }
    @{ Name = 'Domain'; Directory = 'domain-bicep'; Package = 'Domain-Package.zip'; Parameter = 'domainPackageZipUrl' }
    @{ Name = 'Cluster'; Directory = 'cluster-bicep'; Package = 'Cluster-Package.zip'; Parameter = 'clusterPackageZipUrl' }
)

Describe 'One-click release links' {
    $topLevelReadme = Get-Content -LiteralPath (
        Join-Path $automationRoot 'README.md'
    ) -Raw

    It 'pins the <Name> package and deployment buttons to the current release' `
        -TestCases $scenarios {
        param($Name, $Directory, $Package, $Parameter)

        $scenarioRoot = Join-Path $automationRoot $Directory
        $packageUrl = "https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/$releaseVersion/$Package"
        $templateUrl = "https://raw.githubusercontent.com/microsoft/WindowsProtocolTestSuites/$releaseVersion/TestSuites/FileServer/azure-automation/$Directory/azuredeploy.json"
        $buttonUrl = 'https://portal.azure.com/#create/Microsoft.Template/uri/' +
            [uri]::EscapeDataString($templateUrl)

        $main = Get-Content -LiteralPath (
            Join-Path $scenarioRoot 'main.bicep'
        ) -Raw
        $template = Get-Content -LiteralPath (
            Join-Path $scenarioRoot 'azuredeploy.json'
        ) -Raw | ConvertFrom-Json
        $readme = Get-Content -LiteralPath (
            Join-Path $scenarioRoot 'README.md'
        ) -Raw
        $tools = Get-Content -LiteralPath (
            Join-Path $scenarioRoot 'DSC\Scripts\Tools.json'
        ) -Raw | ConvertFrom-Json
        $fileServerAssets = @($tools.PSObject.Properties.Value.TestsuiteZips |
            Where-Object name -eq 'FileServerTestSuite')

        if (-not $main.Contains($packageUrl)) {
            throw "$Name main.bicep does not pin $packageUrl."
        }
        $templateParameter = $template.parameters.($Parameter)
        if ($templateParameter.type -ne 'string' -or
            $templateParameter.defaultValue -ne $packageUrl) {
            throw "$Name azuredeploy.json package parameter is incorrect."
        }
        if (-not $readme.Contains($buttonUrl) -or
            -not $topLevelReadme.Contains($buttonUrl)) {
            throw "$Name deployment button does not target $templateUrl."
        }
        if ($fileServerAssets.Count -ne 1 -or
            $fileServerAssets[0].version -ne $releaseVersion -or
            $fileServerAssets[0].Url -ne
                "https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/$releaseVersion/FileServer-TestSuite-ServerEP.zip" -or
            "$($fileServerAssets[0].SHA256)" -notmatch '^[a-f0-9]{64}$') {
            throw "$Name FileServer asset pin is not aligned with $releaseVersion."
        }
    }
}
