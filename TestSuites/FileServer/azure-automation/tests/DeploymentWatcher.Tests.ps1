# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$helpersPath = Join-Path $here '..\shared\Deploy-Helpers.psm1'
Import-Module $helpersPath -Force

Describe 'ARM deployment reconciliation' {
    InModuleScope Deploy-Helpers {
        It 'continues polling ARM when the local Az job has failed' {
            $script:deploymentPollCount = 0
            Mock Get-AzResourceGroupDeployment {
                $script:deploymentPollCount++
                [pscustomobject]@{
                    DeploymentName = 'test-deployment'
                    ProvisioningState = if ($script:deploymentPollCount -lt 2) { 'Running' } else { 'Succeeded' }
                }
            }
            Mock Get-AzResourceGroupDeploymentOperation { @() }
            Mock Start-Sleep {}

            $failedLocalJob = [pscustomobject]@{ State = 'Failed' }
            $result = Watch-Deployment -ResourceGroupName 'test-rg' `
                -DeploymentName 'test-deployment' -Job $failedLocalJob `
                -PollIntervalSeconds 1 -TimeoutMinutes 1

            $result.ProvisioningState | Should Be 'Succeeded'
            $script:deploymentPollCount | Should BeGreaterThan 1
        }
    }
}
