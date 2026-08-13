# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path
$helpersPath = Join-Path $root 'shared\Deploy-Helpers.psm1'

Describe 'Auto-shutdown lifecycle' {
    $helpers = Get-Content $helpersPath -Raw

    It 'removes existing VM shutdown schedules before active deployment work' {
        $helpers.Contains("-ResourceType 'Microsoft.DevTestLab/schedules'") | Should Be $true
        $helpers.Contains('$expectedScheduleNames') | Should Be $true
        $helpers.Contains('Where-Object { $_.Name -in $expectedScheduleNames }') |
            Should Be $true
        $helpers.Contains('Remove-AzResource -ResourceId $schedule.ResourceId -Force') |
            Should Be $true
    }

    It 'creates enabled shutdown schedules only after success' {
        $helpers.Contains("`$normalizedTime = `$Time -replace ':', ''") | Should Be $true
        $helpers.Contains("status = 'Enabled'") | Should Be $true
        $helpers.Contains("dailyRecurrence = [pscustomobject]@{ time = `$normalizedTime }") |
            Should Be $true
        $helpers.Contains('New-AzResource -ResourceGroupName $ResourceGroupName') |
            Should Be $true
    }
}

Describe 'Scenario auto-shutdown ordering' {
    foreach ($scenario in @('workgroup-bicep', 'domain-bicep', 'cluster-bicep')) {
        It "defers $scenario schedules until verified success" {
            $deploy = Get-Content (Join-Path $root "$scenario\deploy.ps1") -Raw
            $removeIndex = $deploy.IndexOf('Remove-VmAutoShutdownSchedules')
            $enableIndex = $deploy.LastIndexOf('Enable-VmAutoShutdownSchedules')

            ($removeIndex -ge 0) | Should Be $true
            ($enableIndex -gt $removeIndex) | Should Be $true
            ($deploy.Contains('-VMNames @(') -or
                $deploy.Contains('$autoShutdownVmNames = @(')) | Should Be $true
            $deploy.Contains("['enableAutoShutdown'] = `$false") | Should Be $true
        }
    }
}
