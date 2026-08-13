# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

Describe 'Workgroup auto-shutdown recovery' {
    $deploy = Get-Content (Join-Path $root 'workgroup-bicep\deploy.ps1') -Raw

    It 'removes schedules only when auto-shutdown was requested' {
        $removeIndex = $deploy.IndexOf('Remove-VmAutoShutdownSchedules')
        $guardIndex = $deploy.LastIndexOf('if ($autoShutdownRequested)', $removeIndex)

        ($removeIndex -ge 0) | Should Be $true
        ($guardIndex -ge 0) | Should Be $true
        ($removeIndex -gt $guardIndex) | Should Be $true
    }

    It 'restores requested schedules from the outer finally path' {
        $finallyIndex = $deploy.LastIndexOf('} finally {')
        $restoreIndex = $deploy.LastIndexOf('Enable-VmAutoShutdownSchedules')

        ($finallyIndex -ge 0) | Should Be $true
        ($restoreIndex -gt $finallyIndex) | Should Be $true
        $deploy.Contains('$autoShutdownRestored = $false') | Should Be $true
        $deploy.Contains('Failed to restore VM auto-shutdown schedules') | Should Be $true
    }
}