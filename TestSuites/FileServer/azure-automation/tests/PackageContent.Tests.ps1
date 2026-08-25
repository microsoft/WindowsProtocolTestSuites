# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path
$contracts = Join-Path $root 'shared\DSC\Scripts\Package-Contracts.ps1'
. $contracts

function New-SyntheticDscPackage {
    param(
        [Parameter(Mandatory)]
        [string]$Scenario,

        [Parameter(Mandatory)]
        [string]$Path
    )

    New-Item -ItemType Directory -Path $Path -Force | Out-Null
    foreach ($relativePath in (Get-DscPackageRequiredPaths -Scenario $Scenario)) {
        $fullPath = Join-Path $Path ($relativePath -replace '/', '\')
        $parent = Split-Path -Path $fullPath -Parent
        if (-not (Test-Path $parent)) {
            New-Item -ItemType Directory -Path $parent -Force | Out-Null
        }
        if ($relativePath -in @('Tools.json', 'DSC/Scripts/Tools.json')) {
            $tools = @{}
            foreach ($role in (Get-DscPackageRequiredToolRoles -Scenario $Scenario)) {
                $tools[$role] = @{ Tools = @(); TestsuiteZips = @() }
            }
            $tools | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $fullPath
        }
        else {
            "content for $relativePath" | Set-Content -LiteralPath $fullPath
        }
    }
}

Describe 'DSC package manifest contracts' {
    BeforeEach {
        $testRoot = Join-Path $env:TEMP "DscPackageContract-$([guid]::NewGuid().ToString('N'))"
    }

    AfterEach {
        Remove-Item $testRoot -Recurse -Force -ErrorAction SilentlyContinue
    }

    It 'requires every Cluster role and the shared deterministic contracts' {
        $required = @(Get-DscPackageRequiredPaths -Scenario Cluster)

        ($required -contains 'DSC/Deploy-DC.ps1') | Should Be $true
        ($required -contains 'DSC/Deploy-Storage.ps1') | Should Be $true
        ($required -contains 'DSC/Storage-FeatureConfiguration.ps1') | Should Be $true
        ($required -contains 'DSC/Deploy-Node01.ps1') | Should Be $true
        ($required -contains 'DSC/Deploy-Node02.ps1') | Should Be $true
        ($required -contains 'DSC/Deploy-ClusterNode.ps1') | Should Be $true
        ($required -contains 'DSC/Node-FeatureConfiguration.ps1') | Should Be $true
        ($required -contains 'DSC/Deploy-Driver.ps1') | Should Be $true
        ($required -contains 'DSC/Deploy-CommonHelpers.ps1') | Should Be $true
        ($required -contains 'cse-bootstrap.ps1') | Should Be $true
        ($required -contains 'DSC/Scripts/Package-Contracts.ps1') | Should Be $true
        ($required -contains 'DSC/Scripts/Test-StorageReadiness.ps1') | Should Be $true
        ($required -contains 'DSC/Scripts/Test-NodeFoundationReadiness.ps1') |
            Should Be $true
        ($required -contains 'DSC/Scripts/Test-ClusterReadiness.ps1') |
            Should Be $true
        ($required -contains 'DSC/Invoke-ClusterEnvironmentSteps.ps1') |
            Should Be $true
        ($required -contains 'DSC/Deploy-ClusterDriver.ps1') | Should Be $true
        ($required -contains 'DSC/Scripts/Configure-ForceLevel2.ps1') |
            Should Be $true
        ($required -contains 'DSC/Scripts/Test-ClusterDriverReadiness.ps1') |
            Should Be $true
        ($required -contains 'DSC/Scripts/Set-KerberosMachinePasswordAlignment.ps1') |
            Should Be $true
        ($required -contains 'Config.json') | Should Be $true
        ($required -contains 'Tools.json') | Should Be $true
    }

    It 'requires the shared Kerberos alignment helper in Domain packages' {
        $required = @(Get-DscPackageRequiredPaths -Scenario Domain)

        ($required -contains 'DSC/Scripts/Set-KerberosMachinePasswordAlignment.ps1') |
            Should Be $true
    }

    It 'keeps Cluster-only Driver scripts out of Domain and Workgroup contracts' {
        foreach ($scenario in @('Domain', 'Workgroup')) {
            $required = @(Get-DscPackageRequiredPaths -Scenario $scenario)

            ($required -contains 'DSC/Deploy-ClusterDriver.ps1') | Should Be $false
            ($required -contains 'DSC/Scripts/Test-ClusterDriverReadiness.ps1') |
                Should Be $false
        }
    }

    It 'creates and validates a hash manifest for a complete Cluster package' {
        New-SyntheticDscPackage -Scenario Cluster -Path $testRoot

        New-DscPackageManifest -PackageRoot $testRoot -Scenario Cluster `
            -SourceRevision 'test-revision' | Out-Null

        Test-DscPackageManifest -PackageRoot $testRoot `
            -ExpectedScenario Cluster | Should Be $true

        $manifest = Get-Content -LiteralPath (Join-Path $testRoot 'PackageManifest.json') `
            -Raw | ConvertFrom-Json
        $manifest.SchemaVersion | Should Be '1.0'
        $manifest.SourceRevision | Should Be 'test-revision'
        @($manifest.Files).Count | Should BeGreaterThan 5
        @($manifest.Files | Where-Object {
            "$($_.SHA256)" -cne "$($_.SHA256)".ToLowerInvariant()
        }).Count | Should Be 0
    }

    It 'rejects a package whose content changed after manifest generation' {
        New-SyntheticDscPackage -Scenario Cluster -Path $testRoot
        New-DscPackageManifest -PackageRoot $testRoot -Scenario Cluster `
            -SourceRevision 'test-revision' | Out-Null
        'tampered' | Add-Content -LiteralPath (Join-Path $testRoot 'DSC\Deploy-Node01.ps1')

        { Test-DscPackageManifest -PackageRoot $testRoot `
                -ExpectedScenario Cluster -ThrowOnFailure } |
            Should Throw 'hash mismatch'
    }

    It 'rejects a package for the wrong deployment scenario' {
        New-SyntheticDscPackage -Scenario Cluster -Path $testRoot
        New-DscPackageManifest -PackageRoot $testRoot -Scenario Cluster `
            -SourceRevision 'test-revision' | Out-Null

        { Test-DscPackageManifest -PackageRoot $testRoot `
                -ExpectedScenario Domain -ThrowOnFailure } |
            Should Throw 'scenario'
    }
}

Describe 'Package assembly integration' {
    It 'writes and validates the manifest before compressing a package' {
        $helpers = Get-Content -Path (Join-Path $root 'shared\Deploy-Helpers.psm1') -Raw
        $manifestWrite = $helpers.IndexOf('New-DscPackageManifest')
        $manifestCheck = $helpers.IndexOf('Test-DscPackageManifest')
        $compress = $helpers.IndexOf('Compress-Archive', $manifestCheck)

        ($manifestWrite -ge 0) | Should Be $true
        ($manifestCheck -gt $manifestWrite) | Should Be $true
        ($compress -gt $manifestCheck) | Should Be $true
    }

    It 'rebuilds supplied Cluster zips through the shared overlay and validation path' {
        $deploy = Get-Content -Path (Join-Path $root 'cluster-bicep\deploy.ps1') -Raw
        $existingZip = $deploy.IndexOf('if (Test-Path $ClusterPackageZip)')
        $sharedBuild = $deploy.IndexOf('Build-DscPackage', $existingZip)

        ($existingZip -ge 0) | Should Be $true
        ($sharedBuild -gt $existingZip) | Should Be $true
        $deploy.Contains('Compress-Archive -Path (Join-Path $tempExtractPath') |
            Should Be $false

        $helpers = Get-Content -Path (Join-Path $root 'shared\Deploy-Helpers.psm1') -Raw
        $helpers.Contains("Copy-Item -Path (Join-Path `$DscFolderPath '*')") |
            Should Be $true
    }

    It 'keeps private results configuration out of public packages' {
        $publish = Get-Content -Path (Join-Path $root 'shared\Publish-DscPackage.ps1') -Raw
        $helpers = Get-Content -Path (Join-Path $root 'shared\Deploy-Helpers.psm1') -Raw

        $publish.Contains('-PublicPackage') | Should Be $true
        $helpers.Contains('[switch]$PublicPackage') | Should Be $true
        $helpers.Contains('if ($ResultsUploadConfig -and -not $PublicPackage)') |
            Should Be $true
    }
}

Describe 'Pinned deployment tool packages' {
    It 'pins required tools in every scenario and validates configured SHA-256 values' {
        $expectedHashes = @{
            PowerShellCore = '61b31ec847d4fdc4d39050f4f650968c55acf6f16cb66b1f00bf05db4e946559'
            DotNetCore = 'c5c6709149fa9ef3a49873257999c3f5d5d7d2894b0e9490653e7f9f768c4f74'
            'Win32-OpenSSH' = 'ec8144a107014740ec3ce16ec51710398fc390fca5344931c1506e7cc2e181f3'
            PTMService = 'afc675c6f48de4f835a1f4b2a0f75479b473eeed861109f4e30aa788898d4263'
            PTMCli = '7f77bfc1a821a665c015c33bda4709e4ac8e6d9268023d8dfdc1f7b4e9922ff9'
            FileServerTestSuite = '4700a2b35a410a38f165943aaebb72a269171b9c8d61f7214dd7478796c5e6e8'
        }

        foreach ($scenario in @('cluster-bicep', 'domain-bicep', 'workgroup-bicep')) {
            $toolsPath = Join-Path $root "$scenario\DSC\Scripts\Tools.json"
            $config = Get-Content -LiteralPath $toolsPath -Raw | ConvertFrom-Json
            $items = @()
            foreach ($role in $config.PSObject.Properties) {
                $items += @($role.Value.Tools)
                $items += @($role.Value.TestsuiteZips)
            }

            foreach ($item in @($items | Where-Object {
                $_.name -notin @('WMF', 'WindowsAdminCenter')
            })) {
                "$($item.SHA256)" | Should Match '^[a-fA-F0-9]{64}$'
            }

            foreach ($name in $expectedHashes.Keys) {
                $matching = @($items | Where-Object { $_.name -eq $name })
                if ($name -in @('PTMService', 'PTMCli', 'FileServerTestSuite')) {
                    $matching.Count | Should BeGreaterThan 0
                }
                foreach ($item in $matching) {
                    ("$($item.SHA256)").ToLowerInvariant() |
                        Should Be $expectedHashes[$name]
                }
            }

            $testSuite = @($items | Where-Object {
                $_.ZipName -eq 'FileServer-TestSuite-ServerEP.zip'
            })
            $testSuite.Count | Should BeGreaterThan 0
            foreach ($item in $testSuite) {
                $item.Url | Should Match '/releases/download/4\.26\.8\.1/'
                $item.version | Should Be '4.26.8.1'
            }

            $ptmService = @($items | Where-Object { $_.name -eq 'PTMService' })
            foreach ($item in $ptmService) {
                $item.Url | Should Match '/releases/download/4\.26\.8\.0/'
                $item.version | Should Be '4.26.8.0'
            }
        }
    }
}
