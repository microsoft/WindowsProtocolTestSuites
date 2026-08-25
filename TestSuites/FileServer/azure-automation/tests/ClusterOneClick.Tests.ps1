# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

function Read-AutomationFile {
    param([string]$RelativePath)
    return Get-Content -Path (Join-Path $root $RelativePath) -Raw
}

Describe 'Cluster one-click deployment contract' {
    BeforeAll {
        $main = Read-AutomationFile 'cluster-bicep\main.bicep'
        $serviceExtensions = Read-AutomationFile `
            'cluster-bicep\modules\service-extensions.bicep'
        $computerExtensions = Read-AutomationFile `
            'cluster-bicep\modules\computer-extensions.bicep'
    }

    It 'uses burstable defaults for portal deployments' {
        $main.Contains("param dcVmSize string = 'Standard_B4ms'") | Should Be $true
        $main.Contains("param storageVmSize string = 'Standard_B4ms'") | Should Be $true
        $main.Contains("param clusterNodeVmSize string = 'Standard_B8ms'") | Should Be $true
        $main.Contains("param driverVmSize string = 'Standard_B4ms'") | Should Be $true
    }

    It 'pins a public manifest-validated package and keeps credentials protected' {
        $main.Contains(
            'https://github.com/anamikoye/wpts-deploy-test/releases/download/cluster-test-v1/Cluster-Package.zip'
        ) | Should Be $true
        $main.Contains("Password: '#{ADMIN_PASSWORD}#'") | Should Be $true

        foreach ($module in @($serviceExtensions, $computerExtensions)) {
            $module.Contains('protectedSettings:') | Should Be $true
            $module.Contains('-ConfigJsonBase64') | Should Be $true
            $module.Contains('-PasswordBase64') | Should Be $true
        }
    }

    It 'blocks Phase 2 on verified DC and Storage readiness' {
        $phase1Wait = $main.IndexOf("resource waitPhase1")
        $phase2 = $main.IndexOf("module phase2 'phase2.bicep'")

        ($phase1Wait -ge 0) | Should Be $true
        ($phase2 -gt $phase1Wait) | Should Be $true
        $main.Contains('Deploy-DC.Completed.signal') | Should Be $true
        $main.Contains('Deploy-Storage.Completed.signal') | Should Be $true
        $main.Contains('Test-StorageReadiness.ps1') | Should Be $true
        $phase2Block = $main.Substring($phase2, $main.IndexOf(
            "resource phase2Encryption",
            $phase2
        ) - $phase2)
        $phase2Block.Contains('waitPhase1') | Should Be $true
    }

    It 'encrypts before launching configuration extensions' {
        $phase1Encryption = $main.IndexOf("resource phase1Encryption")
        $serviceExtensionsIndex = $main.IndexOf("module serviceExtensions")
        $phase2Encryption = $main.IndexOf("resource phase2Encryption")
        $computerExtensionsIndex = $main.IndexOf("module computerExtensions")

        ($serviceExtensionsIndex -gt $phase1Encryption) | Should Be $true
        ($computerExtensionsIndex -gt $phase2Encryption) | Should Be $true
        $main.Contains('    phase1Encryption') | Should Be $true
        $main.Contains('    phase2Encryption') | Should Be $true
    }

    It 'does not emit empty Key Vault arguments when disk encryption is disabled' {
        ([regex]::Matches(
            $main,
            '-EnableEncryption \$\{enableDiskEncryption \? 1 : 0\}'
        ).Count) | Should Be 2
        ([regex]::Matches(
            $main,
            "enableDiskEncryption \? phase1\.outputs\.keyVaultUrl : 'unused'"
        ).Count) | Should Be 2
        ([regex]::Matches(
            $main,
            "enableDiskEncryption \? phase1\.outputs\.keyVaultId : 'unused'"
        ).Count) | Should Be 2
    }

    It 'passes VM names as separate deployment-script arguments' {
        $main.Contains('[string]$VmName1') | Should Be $true
        $main.Contains('[string]$VmName2') | Should Be $true
        $main.Contains("[string]`$VmName3 = ''") | Should Be $true
        $main.Contains('-VmNames') | Should Be $false
        ([regex]::Matches($main, '-VmName1 "')).Count | Should Be 2
        ([regex]::Matches($main, '-VmName2 "')).Count | Should Be 2
        ([regex]::Matches($main, '-VmName3 "')).Count | Should Be 1
    }

    It 'returns success only after live Cluster and Driver readiness' {
        $main.Contains('Deploy-Node01.Completed.signal') | Should Be $true
        $main.Contains('Deploy-Node02.Completed.signal') | Should Be $true
        $main.Contains('Deploy-Driver.Completed.signal') | Should Be $true
        $main.Contains('Test-ClusterReadiness.ps1') | Should Be $true
        $main.Contains('Test-ClusterDriverReadiness.ps1') | Should Be $true
        $main.Contains('output deploymentReady bool = waitFinal.properties.outputs.complete') |
            Should Be $true
    }
}
