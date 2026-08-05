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
        ([regex]::Matches($sutDeploy, 'Invoke-VerifiedDscConfiguration').Count) | Should Be 1
        $sutDeploy.Contains('DSC Lite') | Should Be $false
    }

    It 'verifies DSC through fresh status probes after disruptive feature installation' {
        $helpers = Read-AutomationFile 'shared\DSC\Deploy-CommonHelpers.ps1'

        $helpers.Contains('Start-DscConfiguration -Path $Path -Verbose -Force -ErrorAction Stop') |
            Should Be $true
        $helpers.Contains('-ErrorAction SilentlyContinue -ErrorVariable statusErrors') |
            Should Be $true
        $helpers.Contains('$statusErrors = @()') | Should Be $true
        $helpers.Contains('function Test-PendingSystemReboot') | Should Be $true
        $helpers.Contains("-Name 'PendingFileRenameOperations'") | Should Be $true
        $helpers.Contains('$pendingRenameOperations.Count -gt 0') | Should Be $true
        $helpers.Contains("Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations'") |
            Should Be $false
        $helpers.Contains("if (`$statusName -eq 'Failure')") | Should Be $true
        $helpers.Contains("if (`$statusName -eq 'Success')") | Should Be $true
    }

    It 'requires verified tool installation before the Domain SUT reports ready' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'

        $sutDeploy.Contains('$toolsOk = Test-Path $toolsSignal') | Should Be $true
        ([regex]::IsMatch($sutDeploy, 'if \([^\r\n]*\$toolsOk[^\r\n]*\) \{\s*"DEPLOY FINISHED')) |
            Should Be $true
        $sutDeploy.Contains('Phase 3 blocked until required tools install successfully') |
            Should Be $true
    }

    It 'bounds and diagnoses the Domain SUT background tools job' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'

        $sutDeploy.Contains('$toolsJobTimeoutSeconds = 3600') | Should Be $true
        $sutDeploy.Contains('Wait-Job -Job $toolsJob -Timeout $remainingSeconds') |
            Should Be $true
        $sutDeploy.Contains("Tools job ended in state '`$(`$toolsJob.State)'") |
            Should Be $true
    }

    It 'does not advance the Domain SUT past a failed full DSC apply' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'

        $sutDeploy.Contains('$fullDscOk = $false') | Should Be $true
        $sutDeploy.Contains('$nextDeployStep = if ($fullDscOk) { 2 } else { 1 }') |
            Should Be $true
        $sutDeploy.Contains('the resume task will retry Phase 2') | Should Be $true
    }

    It 'keeps the Domain SUT resume task after an incomplete Phase 3' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'

        $sutDeploy.Contains('SUT deployment is incomplete; the resume task remains registered for retry.') |
            Should Be $true
        $sutDeploy.Contains('$cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"') |
            Should Be $false
    }

    It 'returns the Domain SUT to Phase 2 when DSC prerequisites are incomplete' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        $sutSteps = Read-AutomationFile 'domain-bicep\DSC\Invoke-SutImperativeSteps.ps1'

        $sutDeploy.Contains('resetting to Phase 2 for DSC repair') | Should Be $true
        $sutDeploy.Contains('Set-DeployStep -Step 1') | Should Be $true
        $sutDeploy.Contains("Get-Command `$commandName -ErrorAction SilentlyContinue") |
            Should Be $true
        $sutDeploy.Contains('Get-SmbShare -Name $shareName -ErrorAction SilentlyContinue') |
            Should Be $true
        $sutDeploy.Contains('Test-RequiredSutReadyState') | Should Be $true
        $sutDeploy.Contains('Test-ComputerSecureChannel -ErrorAction Stop') |
            Should Be $true
        $sutSteps.Contains('New-Item -ItemType Directory -Path (Split-Path $sl.Link -Parent)') |
            Should Be $true
        $sutSteps.Contains('Install FS-DFS-Namespace and RSAT-DFS-Mgmt-Con before Phase 3') |
            Should Be $true
    }

    It 'uses persisted pre-reboot and convergence phases for the Workgroup SUT' {
        $sutDeploy = Read-AutomationFile 'workgroup-bicep\DSC\Deploy-SUT.ps1'
        $featureConfiguration = Read-AutomationFile 'workgroup-bicep\DSC\SUT-FeatureConfiguration.ps1'
        $convergenceConfiguration = Read-AutomationFile 'workgroup-bicep\DSC\SUT-Configuration.ps1'

        ([regex]::Matches($sutDeploy, 'Invoke-VerifiedDscConfiguration').Count) | Should Be 2
        $sutDeploy.Contains("`$phaseRegistryName = 'WorkgroupSutDeployPhase'") | Should Be $true
        $sutDeploy.Contains("Set-DeploymentPhase -Name `$phaseRegistryName -Phase 1") | Should Be $true
        $sutDeploy.Contains("Set-DeploymentPhase -Name `$phaseRegistryName -Phase 2") | Should Be $true
        $sutDeploy.Contains("Set-DeploymentPhase -Name `$phaseRegistryName -Phase 3") | Should Be $true
        ([regex]::Matches($sutDeploy, 'Register-DeferredRebootAndResume').Count) | Should Be 1
        $sutDeploy.Contains('single planned reboot') | Should Be $true
        $sutDeploy.Contains('-Postcondition { Test-RequiredSutFeatureState }') |
            Should Be $true
        $sutDeploy.Contains('-Postcondition { Test-RequiredSutDscState }') |
            Should Be $true
        $sutDeploy.Contains('(Test-RequiredSutImperativeState)') | Should Be $true
        $sutDeploy.Contains('dfsutil.exe root $dfsRoot') | Should Be $true
        $sutDeploy.Contains('Continuing without reboot') | Should Be $false
        $sutDeploy.Contains('$currentPhase -ge 3 -and -not (Test-Path $signalFile)') |
            Should Be $true
        $sutDeploy.Contains('reset persisted Phase 3 to repair Phase $repairPhase') |
            Should Be $true
        ($sutDeploy.IndexOf('Set-Content -Path $signalFile -Force') -lt
            $sutDeploy.LastIndexOf('Set-DeploymentPhase -Name $phaseRegistryName -Phase 3')) |
            Should Be $true
        $sutDeploy.Contains("Test-Path `$signalFile -PathType Leaf") | Should Be $true

        ([regex]::Matches($featureConfiguration, 'Script WorkgroupSutFeatureBundle').Count) |
            Should Be 1
        $featureConfiguration.Contains('Install-WindowsFeature -Name $using:requiredFeatures') |
            Should Be $true
        $featureConfiguration.Contains('Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V') |
            Should Be $true
        $convergenceConfiguration.Contains('WindowsFeatureSet FileServerFeatures') | Should Be $false
        $convergenceConfiguration.Contains('[WindowsFeatureSet]FileServerFeatures') | Should Be $false
    }

    It 'prepares Workgroup tools in parallel and preserves the parent transcript' {
        $sutDeploy = Read-AutomationFile 'workgroup-bicep\DSC\Deploy-SUT.ps1'
        $sutSteps = Read-AutomationFile 'workgroup-bicep\DSC\Invoke-SutImperativeSteps.ps1'
        $toolInstaller = Read-AutomationFile 'shared\DSC\Scripts\InstallMSIAndTools.ps1'

        $sutDeploy.Contains('$toolsJobTimeoutSeconds = 3600') | Should Be $true
        $sutDeploy.Contains('Wait-DeploymentJob -Job $Job') | Should Be $true
        $sutDeploy.Contains("-Operation Prepare") | Should Be $true
        $sutDeploy.Contains("-Operation Install") | Should Be $true
        $sutDeploy.Contains('-HeartbeatPath $heartbeatFile -NoTranscript') | Should Be $true
        $sutSteps.Contains('[switch]$NoTranscript') | Should Be $true
        $sutSteps.Contains('[string]$HeartbeatPath') | Should Be $true
        $sutSteps.Contains("Write-DeploymentHeartbeat -Phase 'Environment'") | Should Be $true
        $sutSteps.Contains('-protocolConfigFile $ConfigureFile -NoTranscript') | Should Be $true
        $sutSteps.Contains('New-Item -ItemType Directory -Path (Split-Path $sl.Link -Parent)') |
            Should Be $true
        $sutSteps.Contains('Install FS-DFS-Namespace and RSAT-DFS-Mgmt-Con before Phase 3') |
            Should Be $true
        $toolInstaller.Contains("[ValidateSet('All', 'Prepare', 'Install')]") | Should Be $true
        $toolInstaller.Contains('Start-Job -ScriptBlock') | Should Be $true
        $toolInstaller.Contains('$temporaryPath = "$destination.download.$PID"') | Should Be $true
        $toolInstaller.Contains('Move-Item -LiteralPath $temporaryPath -Destination $destination -Force') |
            Should Be $true
    }

    It 'runs DC drift repair only after the AD readiness-gated imperative phase' {
        $dcDeploy = Read-AutomationFile 'shared\DSC\Deploy-DC.ps1'
        $imperative = $dcDeploy.IndexOf('Phase 2a: Imperative Step 2')
        $dscRepair = $dcDeploy.IndexOf('Phase 2b: DSC Re-Apply')

        ($imperative -ge 0) | Should Be $true
        ($dscRepair -gt $imperative) | Should Be $true
        $dcDeploy.Contains("throw 'DC post-promotion imperative step returned failure.'") |
            Should Be $true
        ([regex]::Matches($dcDeploy, 'Invoke-VerifiedDscConfiguration').Count) |
            Should Be 2
        $dcDeploy.Contains('Removing stale DC completion signal because required state is incomplete.') |
            Should Be $true
        $dcDeploy.IndexOf('Set-DeployStep -Step 1', $dcDeploy.IndexOf('Removing stale DC completion signal')) |
            Should BeGreaterThan $dcDeploy.IndexOf('Removing stale DC completion signal')
        $dcDeploy.Contains('$currentStep = 1') | Should Be $true
        $dcDeploy.Contains('$rebootPending = Test-PendingSystemReboot') | Should Be $true
    }

    It 'keeps orchestrator transcript ownership across imperative child scripts' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        $dcDeploy = Read-AutomationFile 'shared\DSC\Deploy-DC.ps1'
        $driverDeploy = Read-AutomationFile 'shared\DSC\Deploy-Driver.ps1'
        $driverSteps = Read-AutomationFile 'shared\DSC\Invoke-DriverImperativeSteps.ps1'
        $createAccounts = Read-AutomationFile 'shared\DSC\Scripts\Create-TestAccount.ps1'
        $domainJoin = Read-AutomationFile 'shared\DSC\Scripts\domainjoin.ps1'

        $sutDeploy.Contains('-WorkingPath $WorkingPath -NoTranscript') | Should Be $true
        $dcDeploy.Contains('-WorkingPath $WorkingPath -NoTranscript') | Should Be $true
        $driverDeploy.Contains('-Step 1 -WorkingPath $WorkingPath -NoTranscript') | Should Be $true
        $driverDeploy.Contains('NoTranscript = $true') | Should Be $true
        $driverSteps.Contains('[switch]$NoTranscript') | Should Be $true
        $driverSteps.Contains('"$scriptsPath\domainjoin.ps1" -NoTranscript') | Should Be $true
        $createAccounts.Contains('[switch]$NoTranscript') | Should Be $true
        $domainJoin.Contains('[switch]$NoTranscript') | Should Be $true
        $domainJoin.Contains('if (-not $NoTranscript)') | Should Be $true
    }

    It 'bounds each Azure DC readiness Run Command probe' {
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'

        $helpers.Contains('[int]$ProbeTimeoutSeconds = 120') | Should Be $true
        $helpers.Contains('-ScriptString $CheckScript -AsJob -ErrorAction Stop') |
            Should Be $true
        $helpers.Contains('Wait-Job -Job $probeJob -Timeout $probeWaitSeconds') |
            Should Be $true
    }

    It 'checks Driver DSC drift before deciding to repair' {
        $driverDeploy = Read-AutomationFile 'shared\DSC\Deploy-Driver.ps1'

        $driverDeploy.Contains('Driver-DSC.Completed.signal') | Should Be $true
        $driverDeploy.Contains('Test-DscConfiguration -Path $mofFolder') | Should Be $true
        $driverDeploy.Contains('re-apply skipped') | Should Be $true
        $driverDeploy.Contains('if ($driverDscReady)') | Should Be $true
        ([regex]::Matches($driverDeploy, 'Invoke-VerifiedDscConfiguration').Count) |
            Should Be 2
        $driverDeploy.Contains('Driver DSC failed; domain join and tool installation are blocked.') |
            Should Be $true
        $driverDeploy.Contains('[FAIL] Could not validate or set hostname') | Should Be $true
        $driverDeploy.Contains('Removing stale Driver completion signal') | Should Be $true
        $driverDeploy.Contains('$fl2Done -and (Test-RequiredDriverReadyState)') | Should Be $true
        $driverDeploy.Contains('Resetting to Step 1 for repair.') | Should Be $true
        $driverDeploy.Contains('Start-DscConfiguration -Path $mofFolder -Wait') | Should Be $false
    }

    It 'uses bounded parallel jobs and aggregates member encryption failures' {
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'

        $helpers.Contains('Start-ThreadJob -ThrottleLimit $ThrottleLimit') | Should Be $true
        $helpers.Contains('$failed = @($results | Where-Object { -not $_.Success })') | Should Be $true
        $helpers.Contains('throw "Disk encryption failed for: $failedNames"') | Should Be $true
    }
}
