# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Describe 'OneClick release asset pinning' {
    BeforeAll {
        $root = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
        $updateScript = Join-Path $root 'shared\Update-OneClickReleaseAssets.ps1'
    }

    It 'pins FileServer, PTMService, and PTMCli URLs and hashes in each publishable scenario' {
        $testRoot = Join-Path $env:TEMP "OneClickReleaseAssets-$([guid]::NewGuid().ToString('N'))"
        $hashes = @{
            FileServer = 'a' * 64
            PTMService = 'b' * 64
            PTMCli = 'c' * 64
        }
        $versions = @{
            FileServer = '4.26.9.0'
            PTMService = '1.1.2'
            PTMCli = '4.26.9.0'
        }

        try {
            foreach ($scenario in @('workgroup-bicep', 'domain-bicep', 'cluster-bicep')) {
                $target = Join-Path $testRoot "$scenario\DSC\Scripts"
                New-Item -ItemType Directory -Path $target -Force | Out-Null
                Copy-Item -LiteralPath (Join-Path $root "$scenario\DSC\Scripts\Tools.json") `
                    -Destination (Join-Path $target 'Tools.json')
            }

            & $updateScript -AutomationRoot $testRoot `
                -FileServerAssetUrl 'https://example.test/4.26.8.0/FileServer-TestSuite-ServerEP.zip' `
                -FileServerAssetSha256 $hashes.FileServer `
                -FileServerAssetVersion $versions.FileServer `
                -PtmServiceAssetUrl 'https://example.test/ptmservice@1.1.2/PTMService.zip' `
                -PtmServiceAssetSha256 $hashes.PTMService `
                -PtmServiceAssetVersion $versions.PTMService `
                -PtmCliAssetUrl 'https://example.test/4.26.8.0/PTMCli.zip' `
                -PtmCliAssetSha256 $hashes.PTMCli `
                -PtmCliAssetVersion $versions.PTMCli

            foreach ($scenario in @('workgroup-bicep', 'domain-bicep', 'cluster-bicep')) {
                $tools = Get-Content -LiteralPath (
                    Join-Path $testRoot "$scenario\DSC\Scripts\Tools.json"
                ) -Raw | ConvertFrom-Json
                $items = foreach ($role in $tools.PSObject.Properties) {
                    @($role.Value.Tools)
                    @($role.Value.TestsuiteZips)
                }

                $fileServer = @($items | Where-Object {
                    $_.ZipName -eq 'FileServer-TestSuite-ServerEP.zip'
                })
                $ptmService = @($items | Where-Object { $_.name -eq 'PTMService' })
                $ptmCli = @($items | Where-Object { $_.name -eq 'PTMCli' })

                if ($fileServer.Count -eq 0 -or $ptmService.Count -eq 0 -or $ptmCli.Count -eq 0) {
                    throw "$scenario is missing one or more required release assets."
                }
                if (@($fileServer | Where-Object { $_.SHA256 -ne $hashes.FileServer }).Count -gt 0) {
                    throw "$scenario contains an incorrect FileServer SHA-256."
                }
                if (@($ptmService | Where-Object { $_.SHA256 -ne $hashes.PTMService }).Count -gt 0) {
                    throw "$scenario contains an incorrect PTMService SHA-256."
                }
                if (@($ptmCli | Where-Object { $_.SHA256 -ne $hashes.PTMCli }).Count -gt 0) {
                    throw "$scenario contains an incorrect PTMCli SHA-256."
                }
                if (@($fileServer | Where-Object { $_.version -ne $versions.FileServer }).Count -gt 0) {
                    throw "$scenario contains an incorrect FileServer version."
                }
                if (@($ptmService | Where-Object { $_.version -ne $versions.PTMService }).Count -gt 0) {
                    throw "$scenario contains an incorrect PTMService version."
                }
                if (@($ptmCli | Where-Object { $_.version -ne $versions.PTMCli }).Count -gt 0) {
                    throw "$scenario contains an incorrect PTMCli version."
                }
            }

        }
        finally {
            Remove-Item -LiteralPath $testRoot -Recurse -Force -ErrorAction SilentlyContinue
        }
    }

    It 'contains correctly configured public package publishers for every scenario' {
        $publishers = @{
            Workgroup = @{
                Directory = 'workgroup-bicep'
                UrlParameter = 'dscPackageZipUrl'
            }
            Domain = @{
                Directory = 'domain-bicep'
                UrlParameter = 'domainPackageZipUrl'
            }
            Cluster = @{
                Directory = 'cluster-bicep'
                UrlParameter = 'clusterPackageZipUrl'
            }
        }

        foreach ($scenario in $publishers.Keys) {
            $configuration = $publishers[$scenario]
            $publisherPath = Join-Path $root (
                "$($configuration.Directory)\Publish-DscPackage.ps1"
            )

            if (-not (Test-Path -LiteralPath $publisherPath -PathType Leaf)) {
                throw "$scenario publisher was not found: $publisherPath"
            }

            $publisher = Get-Content -LiteralPath $publisherPath -Raw
            if (-not $publisher.Contains("-Scenario '$scenario'")) {
                throw "$scenario publisher does not pass the expected scenario name."
            }
            if (-not $publisher.Contains(
                "-PackageUrlParamName '$($configuration.UrlParameter)'"
            )) {
                throw "$scenario publisher does not pass the expected package URL parameter."
            }
        }
    }
}
