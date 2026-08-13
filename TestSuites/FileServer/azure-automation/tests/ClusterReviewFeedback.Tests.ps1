# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

Describe 'Cluster VCO repair process' {
    $clusterSetup = Get-Content `
        (Join-Path $root 'cluster-bicep\DSC\Scripts\Create-ServerFailoverEnv.ps1') -Raw

    It 'resolves repair prerequisites from the current script directory' {
        $clusterSetup.Contains('$vcoRepairScript = Join-Path $scriptPath ''Repair-ClusterVirtualComputerObjects.ps1''') |
            Should Be $true
        $clusterSetup.Contains('$processLauncher = Join-Path $scriptPath ''Invoke-ProcessAsUser.ps1''') |
            Should Be $true
        $clusterSetup.Contains('-WorkingDirectory $scriptPath') | Should Be $true
        $clusterSetup.Contains('$scriptsPath') | Should Be $false
    }

    It 'waits through the bounded launcher and rejects a nonzero exit code' {
        $clusterSetup.Contains('-WaitForExit') | Should Be $true
        $clusterSetup.Contains('-TimeoutSeconds 600') | Should Be $true
        $clusterSetup.Contains('if ($repairProcess.ExitCode -ne 0)') | Should Be $true
        $clusterSetup.Contains('Wait-Process -Id $repairProcess.ProcessId') | Should Be $false
    }
}

Describe 'Cluster resume package lookup' {
    $deploy = Get-Content (Join-Path $root 'cluster-bicep\deploy.ps1') -Raw

    It 'includes an explicitly named storage account' {
        $deploy.Contains('$_.StorageAccountName -eq $StorageAccountName') | Should Be $true
    }

    It 'requires an explicit selection when retained package blobs are ambiguous' {
        $deploy.Contains('$packageCandidates.Count -gt 1') | Should Be $true
        $deploy.Contains('Multiple reusable Cluster-Package.zip blobs were found.') |
            Should Be $true
        $deploy.Contains('Provide -StorageAccountName or -ClusterPackageZipUrl') |
            Should Be $true
    }

    It 'records package age and selects the single candidate deterministically' {
        $deploy.Contains('LastModified = $lastModified') | Should Be $true
        $deploy.Contains('Sort-Object LastModified -Descending') | Should Be $true
        $deploy.Contains('$selectedPackage.StorageAccountName') | Should Be $true
    }
}