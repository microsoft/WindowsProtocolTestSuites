# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

function Read-AutomationFile {
    param([string]$RelativePath)
    return Get-Content -Path (Join-Path $root $RelativePath) -Raw
}

Describe 'Azure deployment critical-path optimizations' {
    It 'keeps Bastion out of the core network modules' {
        $domainNetwork = Read-AutomationFile 'domain-bicep\modules\network.bicep'
        $workgroupNetwork = Read-AutomationFile 'workgroup-bicep\modules\network.bicep'
        $domainPhase1 = Read-AutomationFile 'domain-bicep\phase1.bicep'
        $workgroupMain = Read-AutomationFile 'workgroup-bicep\main.bicep'

        $domainNetwork.Contains('Microsoft.Network/bastionHosts') | Should Be $false
        $workgroupNetwork.Contains('Microsoft.Network/bastionHosts') | Should Be $false
        $domainPhase1.Contains("../shared/modules/bastion.bicep") | Should Be $true
        $workgroupMain.Contains("../shared/modules/bastion.bicep") | Should Be $true
    }

    It 'separates domain member infrastructure from guest configuration' {
        $computers = Read-AutomationFile 'domain-bicep\modules\domain-computers.bicep'
        $extensions = Read-AutomationFile 'domain-bicep\modules\domain-computer-extensions.bicep'
        $phase2 = Read-AutomationFile 'domain-bicep\phase2.bicep'

        $computers.Contains('Microsoft.Compute/virtualMachines/extensions') | Should Be $false
        $extensions.Contains('Microsoft.Compute/virtualMachines/extensions') | Should Be $true
        $phase2.Contains('param configureGuests bool = true') | Should Be $true
        $phase2.Contains("module memberConfiguration 'modules/domain-computer-extensions.bicep' = if (configureGuests)") | Should Be $true
    }

    It 'provisions member VMs before the fresh-deployment readiness wait' {
        $deploy = Read-AutomationFile 'domain-bicep\deploy.ps1'
        $infrastructure = $deploy.IndexOf("'configureGuests'] = `$false")
        $readiness = $deploy.IndexOf('if (-not $SkipPhase1 -and -not $SkipDCReadyCheck)')
        $configuration = $deploy.IndexOf("'phase2-configuration.bicep'")

        ($infrastructure -ge 0) | Should Be $true
        ($readiness -gt $infrastructure) | Should Be $true
        ($configuration -gt $readiness) | Should Be $true
        $deploy.Contains('if ($diskEncryptionRequested -and $phase1Deployment)') | Should Be $true
        $deploy.Contains('if (-not $SkipPhase1 -and $diskEncryptionRequested') | Should Be $false
        $deploy.Contains('if ($SkipPhase2) {') | Should Be $true
        $deploy.Contains('Domain Controller did not signal readiness within $DCReadyTimeoutMinutes minutes.') | Should Be $true
    }

    It 'applies the domain SUT full DSC configuration only after domain join' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        ([regex]::Matches($sutDeploy, 'Start-DscConfiguration').Count) | Should Be 1
        $sutDeploy.Contains('DSC Lite') | Should Be $false
    }

    It 'checks Driver DSC drift before deciding to repair' {
        $driverDeploy = Read-AutomationFile 'shared\DSC\Deploy-Driver.ps1'

        $driverDeploy.Contains('Driver-DSC.Completed.signal') | Should Be $true
        $driverDeploy.Contains('Test-DscConfiguration -Path $mofFolder') | Should Be $true
        $driverDeploy.Contains('re-apply skipped') | Should Be $true
        $driverDeploy.Contains('if ($driverDscReady)') | Should Be $true
    }

    It 'uses bounded parallel jobs and aggregates member encryption failures' {
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'

        $helpers.Contains('Start-ThreadJob -ThrottleLimit $ThrottleLimit') | Should Be $true
        $helpers.Contains('$failed = @($results | Where-Object { -not $_.Success })') | Should Be $true
        $helpers.Contains('throw "Disk encryption failed for: $failedNames"') | Should Be $true
    }
}
