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

    It 'resolves Cluster default parameter files relative to the deploy script' {
        $deploy = Read-AutomationFile 'cluster-bicep\deploy.ps1'

        $deploy.Contains('[System.IO.Path]::IsPathRooted($Phase1ParametersFile)') |
            Should Be $true
        $deploy.Contains('Join-Path $PSScriptRoot $Phase1ParametersFile') |
            Should Be $true
        $deploy.Contains('[System.IO.Path]::IsPathRooted($Phase2ParametersFile)') |
            Should Be $true
        $deploy.Contains('Join-Path $PSScriptRoot $Phase2ParametersFile') |
            Should Be $true
    }

    It 'keeps Cluster ValidateOnly free of Azure mutations' {
        $deploy = Read-AutomationFile 'cluster-bicep\deploy.ps1'
        $validateIndex = $deploy.IndexOf('if ($ValidateOnly)')
        $validateReturn = $deploy.IndexOf('    return', $validateIndex)
        $resourceGroupMutation = $deploy.IndexOf('Initialize-ResourceGroup')
        $scheduleMutation = $deploy.IndexOf('Remove-VmAutoShutdownSchedules')
        $packageMutation = $deploy.IndexOf('Get-OrCreateStorageAccount')

        ($validateIndex -ge 0) | Should Be $true
        ($validateReturn -gt $validateIndex) | Should Be $true
        ($resourceGroupMutation -gt $validateReturn) | Should Be $true
        ($scheduleMutation -gt $validateReturn) | Should Be $true
        ($packageMutation -gt $validateReturn) | Should Be $true
        $deploy.Contains('no resource group, storage account, package, or schedule was created or modified') |
            Should Be $true
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
        $deploy.Contains('if ($diskEncryptionRequested -and $phase1DeploymentResult)') | Should Be $true
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

    It 'requires verified tool installation before the Domain SUT reports ready' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        $readyState = $sutDeploy.IndexOf('function Test-RequiredSutReadyState')
        $toolsGate = $sutDeploy.IndexOf('(Test-Path $toolsSignal)', $readyState)
        $completionSignal = $sutDeploy.LastIndexOf('Write-VerifiedDeploymentSignal -Path $signalFile')

        ($readyState -ge 0) | Should Be $true
        ($toolsGate -gt $readyState) | Should Be $true
        ($completionSignal -gt $toolsGate) | Should Be $true
        $sutDeploy.Contains('Domain SUT Phase 2 is incomplete (Tools=$toolsOk, Environment=$environmentOk).') |
            Should Be $true
    }

    It 'bounds and diagnoses the Domain SUT background tools job' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'

        $sutDeploy.Contains('$toolsJobTimeoutSeconds = 3600') | Should Be $true
        $sutDeploy.Contains('Wait-DeploymentJob -Job $Job -TimeoutSeconds $toolsJobTimeoutSeconds') |
            Should Be $true
        $sutDeploy.Contains("-Phase 'ToolsPrepare' -Operation 'Parallel Domain SUT package preparation'") |
            Should Be $true
        $sutDeploy.Contains('Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue') |
            Should Be $true
    }

    It 'does not advance the Domain SUT past a failed full DSC apply' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        $convergence = $sutDeploy.LastIndexOf('Invoke-VerifiedDscConfiguration -Path $convergenceMofFolder')
        $completionSignal = $sutDeploy.LastIndexOf('Write-VerifiedDeploymentSignal -Path $signalFile')
        $phaseComplete = $sutDeploy.LastIndexOf('Set-DeploymentPhase -Name $phaseRegistryName -Phase 3')

        ($convergence -ge 0) | Should Be $true
        ($completionSignal -gt $convergence) | Should Be $true
        ($phaseComplete -gt $completionSignal) | Should Be $true
        $sutDeploy.Contains('throw "Domain SUT Phase 2 is incomplete') | Should Be $true
    }

    It 'keeps the Domain SUT resume task after an incomplete Phase 3' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        $completionSignal = $sutDeploy.LastIndexOf('Write-VerifiedDeploymentSignal -Path $signalFile')
        $removeResumeTask = $sutDeploy.LastIndexOf('Remove-ResumeTask')

        ($completionSignal -ge 0) | Should Be $true
        ($removeResumeTask -gt $completionSignal) | Should Be $true
        $sutDeploy.Contains('$cleanupScript = "$scriptsPath\RestartAndRunFinish.ps1"') |
            Should Be $false
    }

    It 'returns the Domain SUT to Phase 2 when DSC prerequisites are incomplete' {
        $sutDeploy = Read-AutomationFile 'domain-bicep\DSC\Deploy-SUT.ps1'
        $sutSteps = Read-AutomationFile 'domain-bicep\DSC\Invoke-SutImperativeSteps.ps1'

        $sutDeploy.Contains('function Get-DomainSutRepairPhase') | Should Be $true
        $sutDeploy.Contains('Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase') |
            Should Be $true
        $sutDeploy.Contains('return 2') | Should Be $true
        $sutDeploy.Contains("Get-Command `$commandName -ErrorAction SilentlyContinue") |
            Should Be $true
        $sutDeploy.Contains('Get-SmbShare -Name $share.Name -ErrorAction SilentlyContinue') |
            Should Be $true
        $sutDeploy.Contains('Test-RequiredSutReadyState') | Should Be $true
        $sutDeploy.Contains('Test-ComputerSecureChannel -ErrorAction Stop') |
            Should Be $true
        $sutSteps.Contains('New-Item -ItemType Directory -Path (Split-Path $sl.Link -Parent)') |
            Should Be $true
        $sutSteps.Contains('Install FS-DFS-Namespace and RSAT-DFS-Mgmt-Con before Phase 3') |
            Should Be $true
    }

    It 'requires verified DSC and concrete postconditions for the Workgroup SUT' {
        $sutDeploy = Read-AutomationFile 'workgroup-bicep\DSC\Deploy-SUT.ps1'
        $sutSteps = Read-AutomationFile 'workgroup-bicep\DSC\Invoke-SutImperativeSteps.ps1'

        ([regex]::Matches($sutDeploy, 'Invoke-VerifiedDscConfiguration').Count) | Should Be 2
        $sutDeploy.Contains("`$phaseRegistryName = 'WorkgroupSutDeployPhase'") |
            Should Be $true
        $sutDeploy.Contains('-Postcondition { Test-RequiredSutDscState }') | Should Be $true
        $sutDeploy.Contains('Removing stale Workgroup SUT completion signal') | Should Be $true
        $sutDeploy.Contains('(Test-RequiredSutImperativeState)') | Should Be $true
        $sutDeploy.Contains('dfsutil.exe root $dfsRoot') | Should Be $true
        $sutDeploy.Contains('Workgroup SUT deployment did not satisfy all required Phase 2 postconditions.') |
            Should Be $true
        $sutDeploy.Contains('if (-not $imperativeOk -or -not $toolsOk -or -not (Test-RequiredSutReadyState))') |
            Should Be $true
        $sutDeploy.Contains('Rename-Computer -NewName $expectedName -Force') | Should Be $true
        $sutDeploy.Contains('requested another disruptive reboot after its single planned reboot') |
            Should Be $true
        $sutDeploy.Contains('Start-DscConfiguration -Path $mofFolder -Wait') | Should Be $false
        $sutDeploy.Contains('[IO.FileAttributes]::ReparsePoint') | Should Be $true
        $sutDeploy.Contains('reparse-point:$path') | Should Be $true
        $sutSteps.Contains('$volume.DeviceID') | Should Be $true
        $sutSteps.Contains('Remove-Item -LiteralPath $mpPath -Force') | Should Be $true
        $sutSteps.Contains('Mount point verification failed') | Should Be $true
        $sutSteps.Contains('$fixtureBytes = [byte[]]::new(8192)') | Should Be $true
        $sutDeploy.Contains('fsa-data-file:$path') | Should Be $true
    }

    It 'bounds Workgroup tools and preserves the parent transcript' {
        $sutDeploy = Read-AutomationFile 'workgroup-bicep\DSC\Deploy-SUT.ps1'
        $sutSteps = Read-AutomationFile 'workgroup-bicep\DSC\Invoke-SutImperativeSteps.ps1'

        $sutDeploy.Contains('$toolsJobTimeoutSeconds = 3600') | Should Be $true
        $sutDeploy.Contains('Wait-DeploymentJob -Job $Job -TimeoutSeconds $toolsJobTimeoutSeconds') |
            Should Be $true
        $sutDeploy.Contains('-Operation Prepare') | Should Be $true
        $sutDeploy.Contains('-Operation Install') | Should Be $true
        $sutDeploy.Contains('& "$dscFolder\Invoke-SutImperativeSteps.ps1" -WorkingPath $WorkingPath') |
            Should Be $true
        $sutDeploy.Contains('-HeartbeatPath $heartbeatFile -NoTranscript') |
            Should Be $true
        $sutSteps.Contains('[switch]$NoTranscript') | Should Be $true
        $sutSteps.Contains('-protocolConfigFile $ConfigureFile -NoTranscript') | Should Be $true
        $sutSteps.Contains('New-Item -ItemType Directory -Path (Split-Path $sl.Link -Parent)') |
            Should Be $true
        $sutSteps.Contains('Install FS-DFS-Namespace and RSAT-DFS-Mgmt-Con before Phase 3') |
            Should Be $true
    }

    It 'runs DC drift repair only after the AD readiness-gated imperative phase' {
        $dcDeploy = Read-AutomationFile 'shared\DSC\Deploy-DC.ps1'
        $imperative = $dcDeploy.IndexOf('& "$dscFolder\Invoke-DcImperativeSteps.ps1" -Step 2')
        $dscRepair = $dcDeploy.IndexOf('Invoke-VerifiedDscConfiguration -Path $convergenceMofFolder', $imperative)

        ($imperative -ge 0) | Should Be $true
        ($dscRepair -gt $imperative) | Should Be $true
        $dcDeploy.Contains("throw 'DC post-promotion imperative step returned failure.'") |
            Should Be $true
        ([regex]::Matches($dcDeploy, 'Invoke-VerifiedDscConfiguration').Count) |
            Should Be 2
        $dcDeploy.Contains('Removing stale DC completion signal.') |
            Should Be $true
        $dcDeploy.Contains('function Get-DcRepairPhase') | Should Be $true
        $dcDeploy.Contains('Set-DeploymentPhase -Name $phaseRegistryName -Phase $repairPhase') |
            Should Be $true
    }

    It 'self-installs required Az modules and verifies Bicep before deployment' {
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'
        $workgroupDeploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'
        $domainDeploy = Read-AutomationFile 'domain-bicep\deploy.ps1'
        $clusterDeploy = Read-AutomationFile 'cluster-bicep\deploy.ps1'

        $helpers.Contains("`$requiredModules = @('Az.Accounts', 'Az.Resources', 'Az.Storage', 'Az.Compute')") |
            Should Be $true
        $helpers.Contains('Install-Module -Name $moduleName -Repository PSGallery -Scope CurrentUser') |
            Should Be $true
        $helpers.Contains("Azure CLI ('az') is not installed") | Should Be $true
        $helpers.Contains('& $azCommand.Source bicep install') | Should Be $true
        $helpers.Contains("Bicep CLI was installed but failed its version check") | Should Be $true
        $workgroupDeploy.Contains('Initialize-BicepCli') | Should Be $true
        $domainDeploy.Contains('Initialize-BicepCli') | Should Be $true
        $clusterDeploy.Contains('Initialize-BicepCli') | Should Be $true
    }

    It 'uses the lightweight regional VM-size endpoint for Workgroup preflight' {
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'
        $workgroupDeploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'
        $domainDeploy = Read-AutomationFile 'domain-bicep\deploy.ps1'
        $clusterDeploy = Read-AutomationFile 'cluster-bicep\deploy.ps1'

        $helpers.Contains('/providers/Microsoft.Compute/locations/$normalizedLocation/vmSizes') |
            Should Be $true
        $helpers.Contains("api-version=2024-07-01") | Should Be $true
        $workgroupDeploy.Contains('Get-RegionalVmSkuSnapshot -Location $config.location') |
            Should Be $true
        $workgroupDeploy.Contains('Get-AzComputeResourceSku') | Should Be $false
        $domainDeploy.Contains('Get-RegionalVmSkuSnapshot -Location $config.location') |
            Should Be $true
        $domainDeploy.Contains('Get-AzComputeResourceSku') | Should Be $false
        $clusterDeploy.Contains('Get-RegionalVmSkuSnapshot -Location $config.location') |
            Should Be $true
        $clusterDeploy.Contains('Get-AzComputeResourceSku') | Should Be $false
    }

    It 'bounds deployment verification probes and requires every scenario VM' {
        $verifier = Read-AutomationFile 'shared\scripts\Verify-Deployment.ps1'

        $verifier.Contains("[ValidateSet('Auto', 'Workgroup', 'Domain', 'Cluster')]") |
            Should Be $true
        $verifier.Contains('[int]$ProbeTimeoutSeconds = 120') | Should Be $true
        $verifier.Contains('-AsJob -ErrorAction Stop') | Should Be $true
        $verifier.Contains('Wait-Job -Job $probeJob -Timeout $TimeoutSeconds') |
            Should Be $true
        $verifier.Contains('Trying one bounded Azure CLI probe') | Should Be $true
        $verifier.Contains('Wait-Job -Job $cliJob -Timeout $TimeoutSeconds') |
            Should Be $true
        $verifier.Contains('--scripts "@$scriptPath"') | Should Be $true
        $verifier.Contains('Wait-Job -Job $probeJobs -Timeout $probeTimeout') |
            Should Be $true
        $verifier.Contains('[string[]]$ExpectedRoles') | Should Be $true
        $verifier.Contains('$targetDefinitions = @($targetDefinitions | Where-Object') |
            Should Be $true
        $submitIndex = $verifier.IndexOf('$probeEntries += [pscustomobject]')
        $waitIndex = $verifier.IndexOf('Wait-Job -Job $probeJobs -Timeout $probeTimeout')
        $collectIndex = $verifier.IndexOf('foreach ($probeEntry in $probeEntries)')
        ($submitIndex -ge 0) | Should Be $true
        ($waitIndex -gt $submitIndex) | Should Be $true
        ($collectIndex -gt $waitIndex) | Should Be $true
        $verifier.Contains("Scenario '`$Scenario' requires exactly one VM matching") |
            Should Be $true
        $verifier.Contains('SIGNAL_STALE') | Should Be $true
        $verifier.Contains('RunShellScript') | Should Be $true
        $verifier.Contains('TEST_READY') | Should Be $true
        $verifier.Contains('Automatic tests completed with failures in $failedTrxCount') |
            Should Be $true
        $verifier.Contains('[bool]$IsLinux') | Should Be $false
        $verifier.Contains('[bool]$LinuxTarget') | Should Be $true
        $verifier.Contains('$linuxVm') | Should Be $true

        $driverDeploy = Read-AutomationFile 'shared\DSC\Deploy-Driver.ps1'
        $driverDeploy.Contains('$isLinuxDriver = $IsLinux -eq $true') | Should Be $true
        $driverDeploy.Contains('Linux flow: No DSC, no domain join, no reboot') |
            Should Be $true

        $clusterVerifier = Read-AutomationFile 'cluster-bicep\scripts\Verify-ClusterDeployment.ps1'
        $delegateEnd = $clusterVerifier.IndexOf("`nreturn")
        $delegateBody = $clusterVerifier.Substring(0, $delegateEnd)
        $delegateBody.Contains('..\..\shared\scripts\Verify-Deployment.ps1') |
            Should Be $true
        $delegateBody.Contains("Scenario = 'Cluster'") | Should Be $true
        $delegateBody.Contains('Invoke-AzVMRunCommand') | Should Be $false
    }

    It 'preserves AD computer accounts for Domain member VMs that still exist' {
        $deploy = Read-AutomationFile 'domain-bicep\deploy.ps1'
        $cleanup = Read-AutomationFile 'domain-bicep\scripts\Remove-StaleComputerAccounts.ps1'
        $sutSteps = Read-AutomationFile 'domain-bicep\DSC\Invoke-SutImperativeSteps.ps1'

        $deploy.Contains("ComputerName = 'Client01'") | Should Be $true
        $deploy.Contains("ComputerName = 'Node01'") | Should Be $true
        $deploy.Contains('Where-Object { $_.VMName -notin $existingMemberVmNames }') |
            Should Be $true
        $deploy.Contains("-Parameter @{ ComputerNamesCsv = (`$staleComputerNames -join ',') }") |
            Should Be $true
        $cleanup.Contains('[string]$ComputerNamesCsv') | Should Be $true
        $cleanup.Contains("'C:\Domain-Package\Config.json'") | Should Be $false
        $sutSteps.Contains('-Credential $domainCredential -Server $adDomain') | Should Be $true
    }

    It 'preserves live Cluster member and endpoint computer accounts on resume' {
        $deploy = Read-AutomationFile 'cluster-bicep\deploy.ps1'
        $clusterSetup = Read-AutomationFile 'cluster-bicep\DSC\Scripts\Create-ServerFailoverEnv.ps1'
        $nodeSetup = Read-AutomationFile 'cluster-bicep\DSC\Invoke-Node01ImperativeSteps.ps1'

        foreach ($computerName in @('Client01', 'Node01', 'Node02', 'Storage01')) {
            $deploy.Contains("ComputerName = '$computerName'") | Should Be $true
        }
        $deploy.Contains("else { 'fstest' }") | Should Be $false
        @([regex]::Matches($deploy, "else \{ 'fstest-cluster' \}")).Count |
            Should BeGreaterThan 2
        $deploy.Contains('Where-Object { $_.VMName -notin $existingMemberVmNames }') |
            Should Be $true
        $deploy.Contains('$cleanupClusterEndpoints = $survivingClusterNodeCount -eq 0') |
            Should Be $true
        $deploy.Contains('Cluster VMs already exist; preserving member and endpoint computer accounts') |
            Should Be $true
        $clusterSetup.Contains("Cluster '`$clusterName' already exists; preserving its AD and DNS endpoint identities") |
            Should Be $true
        $clusterSetup.Contains('Repair-ClusterVirtualComputerObjects.ps1') | Should Be $true
        $nodeSetup.Contains('-Credential $domainCredential') | Should Be $true
        $nodeSetup.Contains('Test-ComputerSecureChannel -ErrorAction Stop') | Should Be $true
        $nodeSetup.Contains('$marker.ComputerPasswordSet -eq 2') | Should Be $true
        $nodeSetup.Contains("-Name 'ComputerPasswordSet' -Value 2") | Should Be $true
        $nodeSetup.Contains("throw 'Failed to synchronize the Node01 AD and local machine passwords") |
            Should Be $true

            $vcoRepair = Read-AutomationFile 'cluster-bicep\DSC\Scripts\Repair-ClusterVirtualComputerObjects.ps1'
            $vcoRepair.Contains('[uint32]0x0100018D') | Should Be $true
            $vcoRepair.Contains('ServicePrincipalName') | Should Be $true
            $vcoRepair.Contains('StatusDNS') | Should Be $true
            $vcoRepair.Contains('StatusKerberos') | Should Be $true
            $vcoRepair.Contains('Start-ClusterGroup') | Should Be $true
    }

    It 'requires fresh signals only from phases redeployed during continuation' {
        $domainDeploy = Read-AutomationFile 'domain-bicep\deploy.ps1'
        $clusterDeploy = Read-AutomationFile 'cluster-bicep\deploy.ps1'

        $domainDeploy.Contains("-ExpectedRoles 'Domain Controller' -TimeoutMinutes 10") |
            Should Be $true
        $domainDeploy.Contains("-ExpectedRoles @('SUT', 'Driver Computer')") |
            Should Be $true
        $clusterDeploy.Contains("-ExpectedRoles @('Domain Controller', 'Storage Server')") |
            Should Be $true
        $clusterDeploy.Contains("-ExpectedRoles @('Cluster Node 1', 'Cluster Node 2', 'Driver Computer')") |
            Should Be $true
    }

    It 'evaluates terminal test classification only after ADE and shutdown handling complete' {
        foreach ($scenario in @('workgroup-bicep', 'domain-bicep', 'cluster-bicep')) {
            $deploy = Read-AutomationFile "$scenario\deploy.ps1"
            $deferIndex = $deploy.IndexOf('DeferTestFailure')
            $encryptionIndex = $deploy.LastIndexOf('Invoke-DiskEncryptionForVMs')
            $outcomeIndex = $deploy.LastIndexOf('Complete-DeploymentTestOutcome')
            $scheduleIndex = if ($outcomeIndex -ge 0) {
                $deploy.LastIndexOf('Enable-VmAutoShutdownSchedules', $outcomeIndex)
            } else {
                -1
            }

            ($deferIndex -ge 0) | Should Be $true
            ($scheduleIndex -ge 0) | Should Be $true
            ($outcomeIndex -gt $encryptionIndex) | Should Be $true
            ($outcomeIndex -gt $scheduleIndex) | Should Be $true
            $deploy.Contains('Automatic tests reached terminal finalization') | Should Be $false
        }
    }

    It 'refreshes Azure authentication after long test waits and before finalization' {
        foreach ($scenario in @('workgroup-bicep', 'domain-bicep', 'cluster-bicep')) {
            $deploy = Read-AutomationFile "$scenario\deploy.ps1"
            $verificationIndex = $deploy.LastIndexOf('$verification =')
            if ($scenario -eq 'workgroup-bicep') {
                $verificationIndex = [math]::Max(
                    $verificationIndex,
                    $deploy.LastIndexOf('$resumeVerification ='))
            }
            $refreshIndex = $deploy.IndexOf(
                'Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host',
                $verificationIndex)
            $encryptionIndex = $deploy.IndexOf('Invoke-DiskEncryptionForVMs', $verificationIndex)
            $scheduleIndex = $deploy.LastIndexOf('Enable-VmAutoShutdownSchedules')
            $scheduleRefreshIndex = $deploy.LastIndexOf(
                'Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host')

            ($verificationIndex -ge 0) | Should Be $true
            ($refreshIndex -gt $verificationIndex) | Should Be $true
            ($encryptionIndex -lt 0 -or $refreshIndex -lt $encryptionIndex) | Should Be $true
            ($scheduleRefreshIndex -lt $scheduleIndex) | Should Be $true
        }

        $verifier = Read-AutomationFile 'shared\scripts\Verify-Deployment.ps1'
        $storageIndex = $verifier.IndexOf('Get-UploadedTestSummaryText -ResourceGroupName')
        $refreshIndex = $verifier.LastIndexOf(
            'Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host',
            $storageIndex)
        ($refreshIndex -ge 0) | Should Be $true
        ($refreshIndex -lt $storageIndex) | Should Be $true
    }

    It 'verifies Cluster configuration and tests before its only disk encryption call' {
        $deploy = Read-AutomationFile 'cluster-bicep\deploy.ps1'
        $verification = $deploy.IndexOf('$verification = & "$PSScriptRoot\scripts\Verify-ClusterDeployment.ps1"')
        $encryption = $deploy.IndexOf('Invoke-DiskEncryptionForVMs')

        ($verification -ge 0) | Should Be $true
        ($encryption -gt $verification) | Should Be $true
        $deploy.Contains('-NotBeforeUtc $operationStartUtc -WaitForTests') | Should Be $true
        $deploy.Contains('[int]$TestTimeoutMinutes = 360') | Should Be $true
        $deploy.Contains('-TestTimeoutMinutes $TestTimeoutMinutes') | Should Be $true
        ([regex]::Matches($deploy, 'Invoke-DiskEncryptionForVMs').Count) | Should Be 1
        $deploy.Contains('Wait-Job -Job $cleanupJob -Timeout 180') | Should Be $true
        $deploy.Contains("DeploymentName -like 'Cluster-Phase1-*'") | Should Be $true
        $deploy.Contains("StorageAccountName -like 'fststorage*'") | Should Be $true
        $deploy.Contains('No reusable Cluster-Package.zip was found') | Should Be $true
    }

    It 'verifies full Domain configuration and tests before member disk encryption' {
        $deploy = Read-AutomationFile 'domain-bicep\deploy.ps1'
        $verification = $deploy.IndexOf('$verification = & "$PSScriptRoot\..\shared\scripts\Verify-Deployment.ps1"')
        $lastEncryption = $deploy.LastIndexOf('Invoke-DiskEncryptionForVMs')

        ($verification -ge 0) | Should Be $true
        ($lastEncryption -gt $verification) | Should Be $true
        $deploy.Contains('-Scenario Domain -TimeoutMinutes 120') |
            Should Be $true
        $deploy.Contains('[int]$TestTimeoutMinutes = 360') | Should Be $true
        $deploy.Contains('-TestTimeoutMinutes $TestTimeoutMinutes') | Should Be $true
        $deploy.Contains('-NotBeforeUtc $operationStartUtc -WaitForTests') | Should Be $true
        $deploy.Contains('Wait-Job -Job $cleanupJob -Timeout 180') | Should Be $true
        $deploy.Contains('No reusable Domain-Package.zip was found') | Should Be $true
    }

    It 'requires verified DSC, bounded tools, and iSCSI before Cluster nodes advance' {
        foreach ($relativePath in @(
            'cluster-bicep\DSC\Deploy-Node01.ps1',
            'cluster-bicep\DSC\Deploy-Node02.ps1'
        )) {
            $nodeDeploy = Read-AutomationFile $relativePath

            ([regex]::Matches($nodeDeploy, 'Invoke-VerifiedDscConfiguration').Count) |
                Should Be 2
            $nodeDeploy.Contains('Start-DscConfiguration -Path $mofFolder -Wait') |
                Should Be $false
            $nodeDeploy.Contains('$toolsJobTimeoutSeconds = 3600') | Should Be $true
            $nodeDeploy.Contains('Wait-Job -Job $toolsJob -Timeout $remainingSeconds') |
                Should Be $true
            $nodeDeploy.Contains('$toolsOk = Test-Path $toolsSignal') | Should Be $true
            $nodeDeploy.Contains('$fullDscOk -and $toolsOk -and $iscsiOk') |
                Should Be $true
            $nodeDeploy.Contains('Cluster node prerequisites are incomplete') |
                Should Be $true
        }
    }

    It 'requires verified DSC and concrete iSCSI state before Storage reports ready' {
        $storageDeploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-Storage.ps1'

        ([regex]::Matches($storageDeploy, 'Invoke-VerifiedDscConfiguration').Count) |
            Should Be 2
        $storageDeploy.Contains('Start-DscConfiguration -Path $mofFolder -Wait') |
            Should Be $false
        $storageDeploy.Contains('function Test-RequiredStorageReadyState') | Should Be $true
        $storageDeploy.Contains("`$service.Status -ne 'Running'") | Should Be $true
        $storageDeploy.Contains('$target.LunMappings.Count -lt 4') | Should Be $true
        $storageDeploy.Contains('Removing stale Storage completion signal') | Should Be $true
        $storageDeploy.Contains('if (Test-RequiredStorageReadyState)') | Should Be $true
    }

    It 'uses shared bootstrap delivery for every Cluster VM role' {
        foreach ($relativePath in @(
            'cluster-bicep\modules\domain-controller.bicep',
            'cluster-bicep\modules\storage-server.bicep',
            'cluster-bicep\modules\cluster-nodes.bicep',
            'cluster-bicep\modules\driver-computer.bicep'
        )) {
            $module = Read-AutomationFile $relativePath
            $module.Contains('cse-bootstrap.ps1') |
                Should Be $true
            $module.Contains('Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force; Start-Transcript') |
                Should Be $false
        }

        $driverModule = Read-AutomationFile 'cluster-bicep\modules\driver-computer.bicep'
        $driverModule.Contains("loadTextContent('../../shared/scripts/cse-bootstrap.sh')") |
            Should Be $true
        $driverModule.Contains('script: base64(driverLinuxBootstrap)') | Should Be $true
        $driverModule.Contains('/var/lib/waagent/custom-script/download/0') | Should Be $false
    }

    It 'keeps every Windows CSE command below the command-line limit' {
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'
        $bootstrap = Read-AutomationFile 'shared\scripts\cse-bootstrap.ps1'
        $windowsModules = @(
            'workgroup-bicep\modules\workgroup-computers.bicep',
            'domain-bicep\modules\domain-controller.bicep',
            'domain-bicep\modules\domain-computer-extensions.bicep',
            'cluster-bicep\modules\domain-controller.bicep',
            'cluster-bicep\modules\storage-server.bicep',
            'cluster-bicep\modules\cluster-nodes.bicep',
            'cluster-bicep\modules\driver-computer.bicep'
        )

        $helpers.Contains("Join-Path `$sharedRoot 'scripts\cse-bootstrap.ps1'") |
            Should Be $true
        $helpers.Contains("Join-Path `$tempPackagePath 'cse-bootstrap.ps1'") |
            Should Be $true
        $bootstrap.Contains('[switch]$PackageAlreadyExtracted') | Should Be $true

        foreach ($relativePath in $windowsModules) {
            $module = Read-AutomationFile $relativePath
            $module.Contains("\\cse-bootstrap.ps1\'") | Should Be $true
            $module.Contains('-PackageAlreadyExtracted') | Should Be $true
            $module.Contains("fileUris: [") | Should Be $true
            $module.Contains('base64(driverBootstrap)') | Should Be $false
            $module.Contains('base64(sutBootstrap)') | Should Be $false
            $module.Contains('base64(dcBootstrap)') | Should Be $false
            $module.Contains('base64(storageBootstrap)') | Should Be $false
            $module.Contains('base64(node01Bootstrap)') | Should Be $false
            $module.Contains('base64(node02Bootstrap)') | Should Be $false

            foreach ($line in @($module -split "`r?`n" | Where-Object { $_ -match 'CommandToExecute\s*=' })) {
                ($line.Length -lt 2000) | Should Be $true
            }
        }
    }

    It 'reconciles stale Driver test processes before replacing a package' {
        $windowsBootstrap = Read-AutomationFile 'shared\scripts\cse-bootstrap.ps1'
        $linuxBootstrap = Read-AutomationFile 'shared\scripts\cse-bootstrap.sh'

        $windowsCleanup = $windowsBootstrap.IndexOf("if (`$Role -eq 'driver')")
        $windowsExtract = $windowsBootstrap.IndexOf('Expand-Archive')
        ($windowsCleanup -ge 0) | Should Be $true
        ($windowsExtract -gt $windowsCleanup) | Should Be $true
        $windowsBootstrap.Contains("Stop-ScheduledTask -TaskName 'RunFileServerTests'") |
            Should Be $true
        $windowsBootstrap.Contains("Unregister-ScheduledTask -TaskName 'RunFileServerTests'") |
            Should Be $true
        $windowsBootstrap.Contains('[System.Collections.Generic.HashSet[int]]') |
            Should Be $true
        $windowsBootstrap.Contains('Invoke-TestRun|Execute-TestCaseByContext|dotnet') |
            Should Be $true
        $windowsBootstrap.Contains('vstest|testhost') | Should Be $true
        $windowsBootstrap.Contains("'C:\Test\test.finished.signal'") | Should Be $true
        $windowsBootstrap.Contains('$hadPreviousDeploySignal = Test-Path -LiteralPath $deploySignalPath') |
            Should Be $true
        $windowsBootstrap.Contains("-Name 'DeployStep' -Value 1") | Should Be $true
        $windowsBootstrap.Contains('Remove-Item -LiteralPath $deploySignalPath') |
            Should Be $true

        $linuxCleanup = $linuxBootstrap.IndexOf('if [ ''__ROLE__'' = ''driver'' ]')
        $linuxExtract = $linuxBootstrap.IndexOf('unzip -o')
        ($linuxCleanup -ge 0) | Should Be $true
        ($linuxExtract -gt $linuxCleanup) | Should Be $true
        $linuxBootstrap.Contains("pkill -TERM -f 'Invoke-TestRun|Execute-TestCaseByContext|dotnet vstest|testhost'") |
            Should Be $true
        $linuxBootstrap.Contains('/test/test.finished.signal') | Should Be $true
        $linuxBootstrap.Contains('deploy_signal="/opt/__PACKAGE_NAME__/DSC/') |
            Should Be $true
        $linuxBootstrap.Contains('rm -f "$deploy_signal"') | Should Be $true
    }

    It 'verifies Workgroup configuration and tests before disk encryption' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'
        $deployment = $deploy.IndexOf('$deploymentResult = Invoke-DeploymentWithSkuFallback')
        $verification = $deploy.IndexOf('Invoke-WorkgroupVerification -NotBeforeUtc $deployStart', $deployment)
        $encryption = $deploy.IndexOf('Invoke-WorkgroupDiskEncryptionSafely -DeploymentOutputs $deploymentResult.Outputs', $deployment)

        ($deployment -ge 0) | Should Be $true
        ($verification -gt $deployment) | Should Be $true
        ($encryption -gt $verification) | Should Be $true
        $deploy.Contains("Scenario          = 'Workgroup'") | Should Be $true
        $deploy.Contains("`$verificationParams['WaitForTests'] = `$true") | Should Be $true
        $deploy.Contains('cannot be combined while Azure Disk Encryption is enabled') |
            Should Be $true
        $deploy.Contains('[int]$TestTimeoutMinutes = 360') | Should Be $true
        (Read-AutomationFile 'shared\scripts\Verify-Deployment.ps1').Contains('test.run.completed.signal') |
            Should Be $true
        (Read-AutomationFile 'shared\DSC\Scripts\Invoke-TestRun.ps1').Contains('test.run.completed.signal') |
            Should Be $true
    }

    It 'temporarily detaches required Workgroup FSA mount points around ADE' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'
        $mountScript = Read-AutomationFile 'shared\scripts\Set-FsaMountPointsForDiskEncryption.ps1'
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'

        $deploy.Contains('function Invoke-WorkgroupDiskEncryptionSafely') | Should Be $true
        $deploy.Contains('$resumeInfrastructureFinalizationError') | Should Be $true
        $deploy.Contains('$infrastructureFinalizationError') | Should Be $true
        $helpers.Contains("-Parameter @{ Mode = 'Detach' }") | Should Be $true
        $helpers.Contains("-Parameter @{ Mode = 'Restore' }") | Should Be $true
        $mountScript.Contains("'C:\FileShare\MountPoint'") | Should Be $true
        $mountScript.Contains("'K:\SMBReFSShare\MountPoint'") | Should Be $true
        $mountScript.Contains('& mountvol.exe $path /D') | Should Be $true
    }

    It 'completes deferred disk encryption when Workgroup resumes' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'
        $resumeStart = $deploy.IndexOf('if ($Resume)')
        $resumeVerification = $deploy.IndexOf('Invoke-WorkgroupVerification -NotBeforeUtc $resumeStart', $resumeStart)
        $resumeDeploymentLookup = $deploy.IndexOf('$resumeDeployment = Get-AzResourceGroupDeployment', $resumeVerification)
        $resumeEncryption = $deploy.IndexOf('Invoke-WorkgroupDiskEncryptionSafely -DeploymentOutputs $resumeDeployment.Outputs', $resumeDeploymentLookup)
        $resumeSchedules = $deploy.IndexOf('Enable-VmAutoShutdownSchedules', $resumeEncryption)

        ($resumeVerification -gt $resumeStart) | Should Be $true
        ($resumeDeploymentLookup -gt $resumeVerification) | Should Be $true
        ($resumeEncryption -gt $resumeDeploymentLookup) | Should Be $true
        ($resumeSchedules -gt $resumeEncryption) | Should Be $true
        $deploy.Contains('Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName') |
            Should Be $true
        $deploy.Contains('Invoke-WorkgroupVerification -NotBeforeUtc $NotBeforeUtc -VmTimeoutMinutes 20') |
            Should Be $true
        $deploy.Contains("`$_.Outputs.ContainsKey('keyVaultId')") | Should Be $true
        $deploy.Contains("`$_.Outputs.ContainsKey('keyVaultUrl')") | Should Be $true
    }

    It 'uses one required Workgroup password for unified test accounts' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'
        $localPasswordParameter = [regex]::Match(
            $deploy,
            '\[Parameter\(Mandatory=\$false\)\]\s*\[SecureString\]\$LocalUserPassword')

        $localPasswordParameter.Success | Should Be $true
        $deploy.Contains('$plainLocalUserPassword = if ($LocalUserPassword)') |
            Should Be $true
        $deploy.Contains('UnifyAccountPasswords = $true') | Should Be $true
    }

    It 'creates Workgroup accounts without interactive net user password prompts' {
        $accounts = Read-AutomationFile 'shared\DSC\Scripts\Create-TestAccount.ps1'
        $workgroupBranch = $accounts.Substring($accounts.LastIndexOf('foreach($group in $azgroups)'))

        $workgroupBranch.Contains('New-LocalUser -Name $user.Username') | Should Be $true
        $workgroupBranch.Contains('Set-LocalUser -InputObject $localUser') | Should Be $true
        $workgroupBranch.Contains('Add-LocalGroupMember -Name $user.Group -Member $localUser') |
            Should Be $true
        $workgroupBranch.Contains('& net.exe user $user.Username $user.Password /ADD') |
            Should Be $false
        $workgroupBranch.Contains('& net.exe user Guest $password') | Should Be $false
    }

    It 're-enters the Driver orchestrator after asynchronous ForceLevel2 succeeds' {
        $driverSteps = Read-AutomationFile 'shared\DSC\Invoke-DriverImperativeSteps.ps1'

        $driverSteps.Contains('`$driverDeployScript = ''$PSScriptRoot\Deploy-Driver.ps1''') |
            Should Be $true
        $driverSteps.Contains('`$workingPath = ''$WorkingPath''') | Should Be $true
        $driverSteps.Contains('& `$driverDeployScript -WorkingPath `$workingPath') |
            Should Be $true
        $driverSteps.Contains('if (Test-Path `$driverSignalFile)') | Should Be $true
    }

    It 'launches tests immediately under the configured account with a reboot fallback' {
        $driverDeploy = Read-AutomationFile 'shared\DSC\Deploy-Driver.ps1'
        $userLauncher = Read-AutomationFile 'shared\DSC\Scripts\Invoke-ProcessAsUser.ps1'

        $registerIndex = $driverDeploy.IndexOf("Register-ScheduledTask -TaskName 'RunFileServerTests'")
        $launchIndex = $driverDeploy.IndexOf('Invoke-ProcessAsUser.ps1', $registerIndex)
        $successIndex = $driverDeploy.IndexOf('[OK] Test run launched immediately', $launchIndex)

        ($registerIndex -ge 0) | Should Be $true
        ($launchIndex -gt $registerIndex) | Should Be $true
        ($successIndex -gt $launchIndex) | Should Be $true
        $driverDeploy.Contains('$triggerStartup = New-ScheduledTaskTrigger -AtStartup') |
            Should Be $true
        $driverDeploy.Contains('$triggerOnce') | Should Be $false
        $driverDeploy.Contains('Start-Process -FilePath $pwshExe') | Should Be $false
        $userLauncher.Contains('LogonUser') | Should Be $true
        $userLauncher.Contains('CreateEnvironmentBlock') | Should Be $true
        $userLauncher.Contains('CreateProcessWithTokenW') | Should Be $true
        $userLauncher.Contains('CREATE_UNICODE_ENVIRONMENT') | Should Be $true
        $userLauncher.Contains('LOGON_WITH_PROFILE') | Should Be $true
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
        $driverDeploy.Contains('domain-join reboot is scheduled but has not occurred yet') |
            Should Be $true
        $driverDeploy.Contains('-Operation Prepare') | Should Be $true
        $driverDeploy.Contains('-Operation Install') | Should Be $true
        $driverDeploy.Contains('-AllowRebootRequired') | Should Be $true
        ($driverDeploy.IndexOf('Pre-Reboot Tool Installation') -lt
            $driverDeploy.IndexOf('Phase 1c: Domain Join')) | Should Be $true
        $driverDeploy.Contains(
            'Workgroup mode -- scheduling the planned post-install stabilization reboot.'
        ) | Should Be $true
        ($driverDeploy.IndexOf('$toolsPreparationJob = Start-DriverToolsPreparationJob') -lt
            $driverDeploy.IndexOf('# Pre-check: Validate hostname')) | Should Be $true
        $driverDeploy.Contains('DriverToolsRebootPending') | Should Be $true
        $driverDeploy.Contains('DriverToolsRebootScheduleRetryCount') | Should Be $true
        $driverDeploy.Contains('DriverJoinRebootScheduleRetryCount') | Should Be $true
        $driverDeploy.Contains('Get-DeploymentRegistryValue') | Should Be $true
        ([regex]::Matches(
            $driverDeploy,
            'Get-ItemPropertyValue -Path \$rebootRegPath `\r?\n\s+-Name \$\w+ -ErrorAction SilentlyContinue'
        ).Count) | Should Be 0
        $driverDeploy.Contains('one Driver tool-stabilization reboot') | Should Be $true
        $driverDeploy.Contains('tool-stabilization reboot is scheduled but has not occurred yet') |
            Should Be $true
        $driverDeploy.Contains('Stop-DriverRecoveryTasks') | Should Be $true
        $driverDeploy.Contains("'TKFRSAR', 'Config-ForceLevel2', 'PostDeployReboot'") |
            Should Be $true
        $driverDeploy.Contains('Could not schedule the Driver tool-stabilization reboot') |
            Should Be $true
        $driverDeploy.Contains('Could not schedule the Driver domain-join reboot') |
            Should Be $true
        $driverDeploy.Contains('Could not schedule the Driver hostname-change reboot') |
            Should Be $true
        $driverDeploy.Contains('persisted Driver tool-stabilization reboot could not be rescheduled') |
            Should Be $true
        $driverDeploy.Contains('persisted Driver domain-join reboot could not be rescheduled') |
            Should Be $true
        $driverDeploy.Contains('Test-PostDeployRebootCanStillRun') | Should Be $true
        $driverDeploy.Contains('failed after one bounded reschedule attempt') | Should Be $true
        ($driverDeploy.IndexOf('Write-VerifiedDeploymentSignal -Path $signalFile') -lt
            $driverDeploy.IndexOf('Set-DeployStep -Step 2')) | Should Be $true
        $driverDeploy.Contains('if (-not $isLinuxDriver -and (Test-PendingSystemReboot))') |
            Should Be $true
        ([regex]::Matches(
            $driverDeploy,
            "Unregister-ScheduledTask -TaskName 'RunFileServerTests'"
        ).Count) | Should BeGreaterThan 2
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
        $helpers.Contains('Treating the postcondition as authoritative') | Should Be $true
    }

    It 'enforces the ARM deadline and treats ARM as authoritative over the local job' {
        $helpers = Read-AutomationFile 'shared\Deploy-Helpers.psm1'
        $deploymentFunctionStart = $helpers.IndexOf('function Invoke-DeploymentWithSkuFallback')
        $deploymentFunctionEnd = $helpers.IndexOf('function Test-RegionalVCpuQuota', $deploymentFunctionStart)
        $deploymentFunction = $helpers.Substring(
            $deploymentFunctionStart,
            $deploymentFunctionEnd - $deploymentFunctionStart)

        $helpers.Contains('throw [TimeoutException]::new("Deployment ''$DeploymentName'' did not reach a terminal state') |
            Should Be $true
        $deploymentFunction.Contains('$deployment = Watch-Deployment') |
            Should Be $true
        $deploymentFunction.Contains('Stop-Job -Job $job -ErrorAction SilentlyContinue') |
            Should Be $true
        $deploymentFunction.Contains('Remove-Job -Job $job -Force -ErrorAction SilentlyContinue') |
            Should Be $true
        $deploymentFunction.Contains('Wait-Job -Job $job') |
            Should Be $false
        $deploymentFunction.Contains('$deployment = $job | Receive-Job') |
            Should Be $false
        $helpers.Contains('ARM state is authoritative and local job state is diagnostic only') |
            Should Be $true
        $helpers.Contains('return $deployment') | Should Be $true
    }

    It 'collects bounded Workgroup resume jobs without invalid AutoRemoveJob usage' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'

        $deploy.Contains("'ResumeDeployment', 'TKFRSAR', 'PostDeployReboot', 'Config-ForceLevel2', 'RunFileServerTests'") |
            Should Be $true
        $deploy.Contains('Stop-ScheduledTask -TaskName `$taskName') | Should Be $true
        $deploy.Contains('Stop-Process -Id `$processId -Force') | Should Be $true
        $deploy.Contains("-Filter 'Deploy-*.Completed.signal'") | Should Be $true
        $deploy.Contains("Get-ChildItem 'C:\\`$packageName' -Filter '*.signal' -Recurse") |
            Should Be $false
        $deploy.Contains('Wait-Job -Job $rj.Job -Timeout 300') | Should Be $true
        $deploy.Contains('$rj.Job | Receive-Job -ErrorAction Stop') | Should Be $true
        $deploy.Contains("if (`$resumeOutput -notmatch '=== Resume setup complete ===')") |
            Should Be $true
        $deploy.Contains("-OutFile 'C:\Temp\DSC-Package.zip' -UseBasicParsing -TimeoutSec 120") |
            Should Be $true
        $deploy.Contains('Remove-Job -Job $rj.Job -Force -ErrorAction SilentlyContinue') |
            Should Be $true
        $deploy.Contains('$rj.Job | Receive-Job -AutoRemoveJob') | Should Be $false
        $deploy.Contains('for ($resumeAttempt = 1; $resumeAttempt -le 3; $resumeAttempt++)') |
            Should Be $true
        $deploy.Contains("Script = `$script") | Should Be $true
        $deploy.Contains("Retrying failed resume Run Command") | Should Be $true
    }

    It 'launches Workgroup resume through detached WMI with a startup-task fallback' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'

        $registration = $deploy.IndexOf("Register-ScheduledTask -TaskName 'ResumeDeployment'")
        $startupTrigger = $deploy.IndexOf('New-ScheduledTaskTrigger -AtStartup')
        $detachedLaunch = $deploy.IndexOf("Invoke-CimMethod -ClassName Win32_Process -MethodName Create")
        $launchCheck = $deploy.IndexOf('if (`$launchResult.ReturnValue -ne 0)')
        $successMarker = $deploy.IndexOf("Write-Output '=== Resume setup complete ==='")
        ($registration -ge 0) | Should Be $true
        ($startupTrigger -ge 0) | Should Be $true
        ($detachedLaunch -gt $registration) | Should Be $true
        ($launchCheck -gt $detachedLaunch) | Should Be $true
        ($successMarker -gt $launchCheck) | Should Be $true
        $deploy.Contains("Start-ScheduledTask -TaskName 'ResumeDeployment'") | Should Be $false
    }

    It 'synchronizes an existing Workgroup local admin password before scheduling deployment' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'

        $passwordSync = $deploy.IndexOf('Set-LocalUser -InputObject `$localAdmin -Password `$resumeAdminPassword -ErrorAction Stop')
        $scheduleResume = $deploy.IndexOf("Write-Output 'Scheduling `$(`$vc.Script)...'")
        ($passwordSync -ge 0) | Should Be $true
        ($scheduleResume -gt $passwordSync) | Should Be $true
        $deploy.Contains('`$resumeConfig.Core.Password') | Should Be $true
        $deploy.Contains('Write-Output `$resumeConfig.Core.Password') | Should Be $false
        $deploy.Contains('Set-LocalUser -InputObject `$localAdmin -Password `$resumeAdminPassword -PasswordNeverExpires') |
            Should Be $false
    }

    It 'fails fast on a fresh failed resume task and stops re-probing completed VMs' {
        $verifier = Read-AutomationFile 'shared\scripts\Verify-Deployment.ps1'

        $verifier.Contains('SIGNAL_FAILED|ResumeDeployment') | Should Be $true
        $verifier.Contains('2147946720') | Should Be $true
        $verifier.Contains("'DeploymentConfigurationFailed'") | Should Be $true
        $verifier.Contains('$completedTargets = [System.Collections.Generic.HashSet[string]]') |
            Should Be $true
        $verifier.Contains('if ($completedTargets.Contains($target.VMName))') |
            Should Be $true
    }

    It 'automatically reconciles Workgroup guests once after configuration timeout' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'

        $deploy.Contains('[int]$ConfigurationRecoveryAttempts = 1') | Should Be $true
        $deploy.Contains('[switch]$DeferAutoShutdownRestore') | Should Be $true
        $deploy.Contains("`$ErrorRecord.FullyQualifiedErrorId -match 'DeploymentConfiguration(Timeout|Failed)'") |
            Should Be $true
        $deploy.Contains("`$recoveryParams['Resume'] = `$true") | Should Be $true
        $deploy.Contains("`$recoveryParams['ConfigurationRecoveryAttempts'] = 0") |
            Should Be $true
        $deploy.Contains("`$recoveryParams['DeferAutoShutdownRestore'] = `$true") |
            Should Be $true
        $recoveryInvoke = $deploy.IndexOf('& $PSCommandPath @recoveryParams')
        $successMessage = $deploy.IndexOf("Write-Output '[OK] Automatic Workgroup reconciliation completed and passed verification.'", $recoveryInvoke)
        $recoveryReturn = $deploy.IndexOf('            return', $successMessage)
        $outerFinalization = $deploy.IndexOf('$infrastructureFinalizationError = $null', $recoveryReturn)
        ($recoveryReturn -gt $successMessage) | Should Be $true
        ($outerFinalization -gt $recoveryReturn) | Should Be $true
        $deploy.Contains('if ($autoShutdownRequested)') | Should Be $true
        $deploy.Contains('& $PSCommandPath @recoveryParams') | Should Be $true
        $deploy.Contains('Test-IsWorkgroupConfigurationFailure') | Should Be $true
    }
}
