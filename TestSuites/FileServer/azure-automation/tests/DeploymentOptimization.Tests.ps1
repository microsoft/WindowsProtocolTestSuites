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

    It 'uses one planned member reboot between Domain SUT features and convergence' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        $featureConfiguration = Read-AutomationFile 'domain-bicep\DSC\SUT-FeatureConfiguration.ps1'
        $convergenceConfiguration = Read-AutomationFile 'domain-bicep\DSC\SUT-Configuration.ps1'

        ([regex]::Matches($sutDeploy, 'Invoke-VerifiedDscConfiguration').Count) | Should Be 2
        $sutDeploy.Contains("`$phaseRegistryName = 'DomainSutDeployPhase'") | Should Be $true
        $sutDeploy.Contains("Set-DeploymentPhase -Name `$phaseRegistryName -Phase 1") | Should Be $true
        $sutDeploy.Contains("Set-DeploymentPhase -Name `$phaseRegistryName -Phase 2") | Should Be $true
        $sutDeploy.Contains("Set-DeploymentPhase -Name `$phaseRegistryName -Phase 3") | Should Be $true
        $sutDeploy.Contains('feature/domain-join reboot') | Should Be $true
        $sutDeploy.Contains('requested another normal member reboot') | Should Be $true
        $sutDeploy.Contains('Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase') |
            Should Be $true
        ([regex]::Matches($sutDeploy, 'Get-DomainSutRepairPhase').Count) | Should Be 3
        $sutDeploy.Contains('$currentPhase -ge 3 -and -not (Test-Path $signalFile)') |
            Should Be $true
        ($sutDeploy.IndexOf('Write-VerifiedDeploymentSignal -Path $signalFile') -lt
            $sutDeploy.LastIndexOf('Set-DeploymentPhase -Name $phaseRegistryName -Phase 3')) |
            Should Be $true
        $sutDeploy.Contains('Test-ComputerSecureChannel -ErrorAction Stop') | Should Be $true
        $sutDeploy.Contains('-Operation Prepare') | Should Be $true
        $sutDeploy.Contains('-Operation Install') | Should Be $true
        $featureConfiguration.Contains('Script DomainSutFeatureBundle') | Should Be $true
        $featureConfiguration.Contains("'RSAT-Hyper-V-Tools'") | Should Be $true
        $sutDeploy.Contains("'RSAT-Hyper-V-Tools'") | Should Be $true
        $convergenceConfiguration.Contains('WindowsFeature') | Should Be $false
        $convergenceConfiguration.Contains('[WindowsFeature') | Should Be $false
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
        $helpers.Contains('function Write-VerifiedDeploymentSignal') | Should Be $true
        $helpers.Contains("Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations'") |
            Should Be $false
        $helpers.Contains("if (`$statusName -eq 'Failure')") | Should Be $true
        $helpers.Contains("if (`$statusName -eq 'Success')") | Should Be $true
    }

    It 'requires verified Domain SUT convergence, tools, domain trust, and imperative state' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'

        $sutDeploy.Contains('$toolsOk = Test-Path $toolsSignal') | Should Be $true
        $sutSteps = Read-AutomationFile 'domain-bicep\DSC\Invoke-SutImperativeSteps.ps1'

        $sutDeploy.Contains('(Test-RequiredSutReadyState)') | Should Be $true
        $sutDeploy.Contains('(Test-RequiredSutDomainState)') | Should Be $true
        $sutDeploy.Contains('(Test-RequiredSutImperativeState)') | Should Be $true
        $sutDeploy.Contains('$toolsJobTimeoutSeconds = 3600') | Should Be $true
        $sutDeploy.Contains('Wait-DeploymentJob -Job $Job') | Should Be $true
        $sutDeploy.Contains("Get-Command `$commandName -ErrorAction SilentlyContinue") |
            Should Be $true
        $sutSteps.Contains('New-Item -ItemType Directory -Path (Split-Path $sl.Link -Parent)') |
            Should Be $true
        $sutSteps.Contains('Install FS-DFS-Namespace and RSAT-DFS-Mgmt-Con before Phase 3') |
            Should Be $true
        $sutSteps.Contains('Write-SutOperationHeartbeat') | Should Be $true
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

    It 'uses separate DC foundation and promotion reboots before convergence' {
        $dcDeploy = Read-AutomationFile 'shared\DSC\Deploy-DC.ps1'
        $featureConfiguration = Read-AutomationFile 'shared\DSC\DC-FeatureConfiguration.ps1'
        $convergenceConfiguration = Read-AutomationFile 'shared\DSC\DC-Configuration.ps1'

        $dcDeploy.Contains("`$phaseRegistryName = 'DcDeployPhase'") | Should Be $true
        ([regex]::Matches($dcDeploy, 'Register-DeferredRebootAndResume').Count) | Should Be 3
        $dcDeploy.Contains('DC foundation reboot was persisted but not observed') | Should Be $true
        $dcDeploy.Contains('DC promotion reboot was persisted but not observed') | Should Be $true
        $dcDeploy.Contains('A reboot remains pending before Domain Controller promotion') |
            Should Be $true
        $dcDeploy.Contains('An unexpected third DC reboot became pending after convergence') |
            Should Be $true
        $dcDeploy.Contains('Proceeding to promotion despite pending reboot') | Should Be $false
        $dcDeploy.Contains("throw 'DC post-promotion imperative step returned failure.'") |
            Should Be $true
        ([regex]::Matches($dcDeploy, 'Invoke-VerifiedDscConfiguration').Count) |
            Should Be 2
        $dcDeploy.Contains('-Operation Prepare') | Should Be $true
        $dcDeploy.Contains('-Operation Install') | Should Be $true
        $dcDeploy.Contains('DC-ADOperational.Completed.signal') | Should Be $true
        $dcDeploy.Contains('Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase') |
            Should Be $true
        ([regex]::Matches($dcDeploy, 'Get-DcRepairPhase').Count) | Should Be 4
        $dcDeploy.Contains('Existing promoted DC requires feature repair before convergence.') |
            Should Be $true
        $dcDeploy.Contains('$currentPhase -ge 3 -and -not (Test-Path $signalFile)') |
            Should Be $true
        ($dcDeploy.IndexOf('Write-VerifiedDeploymentSignal -Path $signalFile') -lt
            $dcDeploy.LastIndexOf('Set-DeploymentPhase -Name $phaseRegistryName -Phase 3')) |
            Should Be $true
        $featureConfiguration.Contains('Script DomainControllerFeatureBundle') | Should Be $true
        $convergenceConfiguration.Contains('WindowsFeature') | Should Be $false
        $convergenceConfiguration.Contains('[WindowsFeature') | Should Be $false
    }

    It 'keeps orchestrator transcript ownership across imperative child scripts' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        $dcDeploy = Read-AutomationFile 'shared\DSC\Deploy-DC.ps1'
        $driverDeploy = Read-AutomationFile 'shared\DSC\Deploy-Driver.ps1'
        $driverSteps = Read-AutomationFile 'shared\DSC\Invoke-DriverImperativeSteps.ps1'
        $createAccounts = Read-AutomationFile 'shared\DSC\Scripts\Create-TestAccount.ps1'
        $domainJoin = Read-AutomationFile 'shared\DSC\Scripts\domainjoin.ps1'

        $sutDeploy.Contains('-WorkingPath $WorkingPath -HeartbeatPath $heartbeatFile -NoTranscript') |
            Should Be $true
        $dcDeploy.Contains('-WorkingPath $WorkingPath -HeartbeatPath $heartbeatFile -NoTranscript') |
            Should Be $true
        $driverDeploy.Contains('-Step 1 -WorkingPath $WorkingPath -NoTranscript') | Should Be $true
        $driverDeploy.Contains('NoTranscript = $true') | Should Be $true
        $driverSteps.Contains('[switch]$NoTranscript') | Should Be $true
        $driverSteps.Contains('"$scriptsPath\domainjoin.ps1" -NoTranscript') | Should Be $true
        $createAccounts.Contains('[switch]$NoTranscript') | Should Be $true
        $domainJoin.Contains('[switch]$NoTranscript') | Should Be $true
        $domainJoin.Contains('if (-not $NoTranscript)') | Should Be $true
    }

    It 'blocks Driver tests and completion until ForceLevel2 is confirmed' {
        $driverDeploy = Read-AutomationFile 'shared\DSC\Deploy-Driver.ps1'
        $driverSteps = Read-AutomationFile 'shared\DSC\Invoke-DriverImperativeSteps.ps1'

        $driverDeploy.Contains('elseif (-not $fl2Done)') | Should Be $true
        $driverDeploy.Contains('[WAIT] Not scheduling tests until ForceLevel2 is confirmed.') |
            Should Be $true
        $driverDeploy.Contains('Write-VerifiedDeploymentSignal -Path $signalFile') |
            Should Be $true
        $driverSteps.Contains("Set-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'DeployStep' -Value 1") |
            Should Be $true
        $driverSteps.Contains('& `$driverDeployScript -WorkingPath `$workingPath') |
            Should Be $true
        $driverSteps.Contains('if (Test-Path `$driverSignalFile)') | Should Be $true
        $driverSteps.Contains('ForceLevel2 was already confirmed; live reconfiguration skipped.') |
            Should Be $true
    }

    It 'defines imperative heartbeat helpers at script scope' {
        foreach ($relativePath in @(
            'domain-bicep\DSC\Invoke-SutImperativeSteps.ps1',
            'shared\DSC\Invoke-DcImperativeSteps.ps1',
            'shared\DSC\Invoke-DriverImperativeSteps.ps1'
        )) {
            $tokens = $null
            $errors = $null
            $ast = [System.Management.Automation.Language.Parser]::ParseFile(
                (Join-Path $root $relativePath), [ref]$tokens, [ref]$errors)
            $errors.Count | Should Be 0

            $functions = @($ast.FindAll({
                param($node)
                $node -is [System.Management.Automation.Language.FunctionDefinitionAst]
            }, $true))
            $stopFunction = $functions | Where-Object Name -eq 'Stop-LocalTranscript'
            $heartbeatFunction = $functions | Where-Object Name -like 'Write-*OperationHeartbeat'

            $heartbeatFunction | Should Not BeNullOrEmpty
            ($heartbeatFunction.Extent.StartOffset -gt $stopFunction.Extent.EndOffset) |
                Should Be $true
        }
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
        $driverDeploy.Contains('Wait-DriverDomainReadiness') | Should Be $true
        $driverDeploy.Contains('Test-ComputerSecureChannel -ErrorAction Stop') | Should Be $true
        $driverDeploy.Contains('function Test-DriverDomainReadyState') | Should Be $true
        $driverDeploy.Contains('(Test-DriverDomainReadyState)') | Should Be $true
        $driverDeploy.Contains('Driver domain readiness verification failed') | Should Be $true
        $driverDeploy.Contains('domain-join reboot was persisted but not observed') | Should Be $true
        $driverDeploy.Contains('-Operation Prepare') | Should Be $true
        ($driverDeploy.IndexOf('$toolsPreparationJob = Start-DriverToolsPreparationJob') -lt
            $driverDeploy.IndexOf('# Pre-check: Validate hostname')) | Should Be $true
        $driverDeploy.Contains('unexpected second Driver reboot') | Should Be $true
        ($driverDeploy.IndexOf('became pending before test scheduling') -lt
            $driverDeploy.IndexOf("Register-ScheduledTask -TaskName 'RunFileServerTests'")) |
            Should Be $true
        ($driverDeploy.IndexOf('Write-VerifiedDeploymentSignal -Path $signalFile') -lt
            $driverDeploy.IndexOf('Set-DeployStep -Step 2')) | Should Be $true
        $driverDeploy.Contains('completion and tests are blocked') | Should Be $true
        $driverDeploy.Contains('if (-not $isLinuxDriver -and (Test-PendingSystemReboot))') |
            Should Be $true
        ([regex]::Matches(
            $driverDeploy,
            "Unregister-ScheduledTask -TaskName 'RunFileServerTests'"
        ).Count) | Should BeGreaterThan 3
        $driverDeploy.Contains('after its one allowed rename reboot') | Should Be $true
        $driverDeploy.Contains('Skipping rename reboot -- continuing') | Should Be $false
    }

    It 'surfaces the DC deployment heartbeat when readiness times out' {
        $deploy = Read-AutomationFile 'domain-bicep\deploy.ps1'

        $deploy.Contains('function Write-DcDeploymentHeartbeat') | Should Be $true
        $deploy.Contains('Deploy-DC.heartbeat.json') | Should Be $true
        ([regex]::Matches($deploy, 'Write-DcDeploymentHeartbeat -ResourceGroupName').Count) |
            Should Be 3
    }

    It 'uses bounded parallel jobs and aggregates member encryption failures' {
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'

        $helpers.Contains('Start-ThreadJob -ThrottleLimit $ThrottleLimit') | Should Be $true
        $helpers.Contains('$failed = @($results | Where-Object { -not $_.Success })') | Should Be $true
        $helpers.Contains('throw "Disk encryption failed for: $failedNames"') | Should Be $true
    }
}
