# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

function Read-AutomationFile {
    param([string]$RelativePath)
    return Get-Content -Path (Join-Path $root $RelativePath) -Raw
}

Describe 'Cluster deterministic deployment foundation guardrails' {
    $clusterDeployScripts = @(
        'cluster-bicep\DSC\Deploy-Storage.ps1',
        'cluster-bicep\DSC\Deploy-ClusterNode.ps1'
    )

    It 'loads the shared deterministic deployment contracts on every Cluster role' {
        foreach ($relativePath in $clusterDeployScripts) {
            $content = Read-AutomationFile $relativePath
            $content.Contains('. "$dscFolder\Deploy-CommonHelpers.ps1"') | Should Be $true
        }
    }

    It 'never continues after a Cluster reboot circuit breaker trips' {
        foreach ($relativePath in $clusterDeployScripts) {
            $content = Read-AutomationFile $relativePath
            $content.Contains('Stop-DeploymentForTerminalFailure') | Should Be $true
            $content.Contains('Reboot circuit breaker triggered. Continuing') | Should Be $false
            $content.Contains('Reboot circuit breaker triggered. Skipping rename') | Should Be $false
        }
    }

    It 'uses the shared PendingFileRenameOperations value check' {
        foreach ($relativePath in @(
            'cluster-bicep\DSC\Deploy-ClusterNode.ps1'
        )) {
            $content = Read-AutomationFile $relativePath
            $content.Contains('Test-PendingSystemReboot') | Should Be $true
            $content.Contains(
                "Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations'"
            ) | Should Be $false
        }
    }

    It 'retains deploy.ps1 as the supported local deterministic orchestrator' {
        $deployPath = Join-Path $root 'cluster-bicep\deploy.ps1'
        Test-Path -LiteralPath $deployPath -PathType Leaf | Should Be $true

        $content = Get-Content -LiteralPath $deployPath -Raw
        $content.Contains('Build-DscPackage') | Should Be $true
        $content.Contains('Wait-ForDomainController') | Should Be $true
        $content.Contains('Verify-ClusterDeployment.ps1') | Should Be $true
    }

    It 'preserves the merged Cluster operating-system matrix' {
        foreach ($relativePath in @(
            'cluster-bicep\phase1.bicep',
            'cluster-bicep\phase2.bicep',
            'cluster-bicep\modules\domain-controller.bicep',
            'cluster-bicep\modules\storage-server.bicep',
            'cluster-bicep\modules\cluster-nodes.bicep'
        )) {
            $content = Read-AutomationFile $relativePath
            $content.Contains('2019-datacenter') | Should Be $true
            $content.Contains('2022-datacenter-g2') | Should Be $true
        }
    }

    It 'uses one proven deterministic Storage reboot before convergence' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-Storage.ps1'
        $features = Read-AutomationFile 'cluster-bicep\DSC\Storage-FeatureConfiguration.ps1'
        $convergence = Read-AutomationFile 'cluster-bicep\DSC\Storage-Configuration.ps1'

        $deploy.Contains("`$phaseRegistryName = 'StorageDeployPhase'") | Should Be $true
        $deploy.Contains("Set-DeploymentRebootPending -Role 'Storage' -MaximumRebootCount 1") |
            Should Be $true
        $deploy.Contains("Confirm-DeploymentReboot -Role 'Storage'") | Should Be $true
        $deploy.Contains('Storage feature/hostname reboot') | Should Be $true
        ([regex]::Matches($deploy, 'Register-DeferredRebootAndResume').Count) |
            Should Be 1
        ([regex]::Matches($deploy, 'Invoke-VerifiedDscConfiguration').Count) |
            Should Be 2
        $deploy.Contains('Start-DscConfiguration -Path $mofFolder -Wait') |
            Should Be $false
        $features.Contains('Script StorageFeatureBundle') | Should Be $true
        $features.Contains('FS-iSCSITarget-Server') | Should Be $true
        $convergence.Contains('WindowsFeature') | Should Be $false
        $convergence.Contains('[WindowsFeature') | Should Be $false
    }

    It 'writes verified Storage readiness before persisting completion' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-Storage.ps1'
        $signalWrite = $deploy.IndexOf('Write-VerifiedDeploymentSignal -Path $signalFile')
        $signalCheck = $deploy.IndexOf(
            'Test-VerifiedDeploymentSignal -Path $signalFile',
            $signalWrite
        )
        $phaseWrite = $deploy.LastIndexOf(
            'Set-DeploymentPhase -Name $phaseRegistryName -Phase 2'
        )

        ($signalWrite -ge 0) | Should Be $true
        ($signalCheck -gt $signalWrite) | Should Be $true
        ($phaseWrite -gt $signalCheck) | Should Be $true
        $deploy.Contains('$currentPhase -ge 2 -and -not (Test-VerifiedDeploymentSignal') |
            Should Be $true
    }

    It 'uses one combined feature and domain-join reboot for both Cluster nodes' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterNode.ps1'
        $features = Read-AutomationFile 'cluster-bicep\DSC\Node-FeatureConfiguration.ps1'
        $convergence = Read-AutomationFile 'cluster-bicep\DSC\Node-Configuration.ps1'

        $deploy.Contains('$phaseRegistryName = "Cluster${NodeRole}DeployPhase"') |
            Should Be $true
        $deploy.Contains('Set-DeploymentRebootPending -Role "Cluster$NodeRole" -MaximumRebootCount 1') |
            Should Be $true
        $deploy.Contains('combined feature/domain-join reboot') | Should Be $true
        ([regex]::Matches($deploy, 'Register-DeferredRebootAndResume').Count) |
            Should Be 1
        ([regex]::Matches($deploy, 'Invoke-VerifiedDscConfiguration').Count) |
            Should Be 2
        $deploy.Contains('Test-ComputerSecureChannel -ErrorAction Stop') |
            Should Be $true
        $deploy.Contains('-Operation Prepare') | Should Be $true
        $deploy.Contains('-Operation Install') | Should Be $true
        $features.Contains('Script ClusterNodeFeatureBundle') | Should Be $true
        $features.Contains('Failover-Clustering') | Should Be $true
        $convergence.Contains('WindowsFeature') | Should Be $false
        $convergence.Contains('[WindowsFeature') | Should Be $false
        $convergence.Contains('Start-DscConfiguration -Path $OutputPath -Wait') |
            Should Be $false
    }

    It 'publishes node pre-cluster readiness before Phase 2' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterNode.ps1'
        $signalWrite = $deploy.IndexOf('Write-VerifiedDeploymentSignal -Path $preReadySignal')
        $signalCheck = $deploy.IndexOf(
            'Test-VerifiedDeploymentSignal -Path $preReadySignal',
            $signalWrite
        )
        $phaseWrite = $deploy.LastIndexOf(
            'Set-DeploymentPhase -Name $phaseRegistryName -Phase 2'
        )

        ($signalWrite -ge 0) | Should Be $true
        ($signalCheck -gt $signalWrite) | Should Be $true
        ($phaseWrite -gt $signalCheck) | Should Be $true
        $deploy.Contains('Invoke-Node01ImperativeSteps.ps1" -Step 3') |
            Should Be $false
    }

    It 'aligns the Node01 Kerberos machine key before publishing readiness' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterNode.ps1'
        $alignment = $deploy.IndexOf('Start-NodeKerberosAlignment')
        $preReady = $deploy.LastIndexOf(
            'Write-VerifiedDeploymentSignal -Path $preReadySignal'
        )

        ($alignment -ge 0) | Should Be $true
        ($preReady -gt $alignment) | Should Be $true
        $deploy.Contains('Set-KerberosMachinePasswordAlignment.ps1') |
            Should Be $true
        $deploy.Contains("-RebootScope 'KerberosAlignment'") | Should Be $true
        $deploy.Contains(
            'Node01 Kerberos machine-password alignment reboot was proven.'
        ) | Should Be $true
        $deploy.Contains('(Test-RequiredNodeKerberosAlignment)') |
            Should Be $true
    }

    It 'keeps role entry scripts as thin wrappers over the shared node orchestrator' {
        foreach ($role in @('Node01', 'Node02')) {
            $wrapper = Read-AutomationFile "cluster-bicep\DSC\Deploy-$role.ps1"
            $wrapper.Contains('Deploy-ClusterNode.ps1') | Should Be $true
            $wrapper.Contains("-NodeRole '$role'") | Should Be $true
        }
    }

    It 'blocks Node01 Cluster formation on Node02 pre-readiness' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterNode.ps1'
        $node02Wait = $deploy.IndexOf('Node02.PreClusterReady.signal')
        $formation = $deploy.IndexOf('Create-ServerFailoverEnv.ps1')
        $completeFunction = $deploy.IndexOf('function Complete-ClusterPhase')
        $storageFunction = $deploy.IndexOf('function Connect-ConfiguredStorageTarget')

        ($node02Wait -ge 0) | Should Be $true
        ($formation -gt $node02Wait) | Should Be $true
        ($completeFunction -ge 0) | Should Be $true
        ($completeFunction -lt $storageFunction) | Should Be $true
        $deploy.Contains('Wait-DeploymentCondition') | Should Be $true
        $deploy.Contains('Invoke-Command -ComputerName $node02Name') | Should Be $true
    }

    It 'requires Node02 to validate live Cluster readiness before final completion' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterNode.ps1'

        $deploy.Contains('Deploy-Cluster.Completed.signal') | Should Be $true
        $deploy.Contains('Test-ClusterReadiness.ps1') | Should Be $true
        $deploy.Contains('$finalSignal = Join-Path $dscFolder "Deploy-$NodeRole.Completed.signal"') |
            Should Be $true
        $deploy.Contains('Set-DeploymentPhase -Name $phaseRegistryName -Phase 3') |
            Should Be $true
    }

    It 'writes Cluster and node final signals before final phase persistence' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterNode.ps1'
        $clusterSignal = $deploy.IndexOf(
            'Write-VerifiedDeploymentSignal -Path $clusterReadySignal'
        )
        $nodeSignal = $deploy.IndexOf(
            'Write-VerifiedDeploymentSignal -Path $finalSignal',
            $clusterSignal
        )
        $phaseWrite = $deploy.LastIndexOf(
            'Set-DeploymentPhase -Name $phaseRegistryName -Phase 3'
        )

        ($clusterSignal -ge 0) | Should Be $true
        ($nodeSignal -gt $clusterSignal) | Should Be $true
        ($phaseWrite -gt $nodeSignal) | Should Be $true
    }

    It 'dispatches Cluster Driver deployment to the deterministic Cluster path' {
        $driver = Read-AutomationFile 'shared\DSC\Deploy-Driver.ps1'
        $clusterDriver = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterDriver.ps1'

        $driver.Contains("`$cfg.Core.Scenario -eq 'Cluster'") | Should Be $true
        $driver.Contains('Deploy-ClusterDriver.ps1') | Should Be $true
        $clusterDriver.Contains("`$phaseRegistryName = 'DriverDeployPhase'") |
            Should Be $true
        $clusterDriver.Contains("Set-DeploymentRebootPending -Role 'Driver' -MaximumRebootCount 1") |
            Should Be $true
        $clusterDriver.Contains('-AllowRebootRequired') | Should Be $true
        $clusterDriver.Contains(
            'Set-DeploymentRebootPending -Role Driver -RebootScope Convergence'
        ) | Should Be $true
        $clusterDriver.Contains(
            'Confirm-DeploymentReboot -Role Driver -RebootScope Convergence'
        ) | Should Be $true
        ($clusterDriver.IndexOf('Pre-reboot Cluster Driver tool installation') -lt
            $clusterDriver.IndexOf('Cluster Driver domain join')) | Should Be $true
        $clusterDriver.Contains('Test-ComputerSecureChannel -ErrorAction Stop') |
            Should Be $true
    }

    It 'gates Cluster test scheduling on both node completions and both ForceLevel2 shares' {
        $driver = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterDriver.ps1'
        $readiness = Read-AutomationFile `
            'cluster-bicep\DSC\Scripts\Test-ClusterDriverReadiness.ps1'
        $node01 = $driver.IndexOf('Deploy-Node01.Completed.signal')
        $node02 = $driver.IndexOf('Deploy-Node02.Completed.signal')
        $localForce = $driver.IndexOf('ForceLevel2.Local.Completed.signal')
        $clusterForce = $driver.IndexOf('ForceLevel2.Clustered.Completed.signal')
        $schedule = $driver.IndexOf("Register-ScheduledTask -TaskName 'RunFileServerTests'")

        ($node01 -ge 0) | Should Be $true
        ($node02 -gt $node01) | Should Be $true
        ($localForce -gt $node02) | Should Be $true
        ($clusterForce -gt $localForce) | Should Be $true
        ($schedule -gt $clusterForce) | Should Be $true
        $readiness.Contains('Endpoints.GeneralFS.Name') | Should Be $true
        $readiness.Contains('Endpoints.ScaleoutFS.Name') | Should Be $true
        $readiness.Contains('SMBClustered') | Should Be $true
    }

    It 'derives remote node readiness paths from the configured working path' {
        $nodeDeploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterNode.ps1'
        $driverReadiness = Read-AutomationFile `
            'cluster-bicep\DSC\Scripts\Test-ClusterDriverReadiness.ps1'

        foreach ($script in @($nodeDeploy, $driverReadiness)) {
            $script.Contains("`$remoteDscFolder = Join-Path `$WorkingPath 'DSC'") |
                Should Be $true
            $script.Contains('C:\Cluster-Package') | Should Be $false
        }
        $nodeDeploy.Contains(
            "`$node02SignalPath = Join-Path `$remoteDscFolder 'Node02.PreClusterReady.signal'"
        ) | Should Be $true
        $nodeDeploy.Contains(
            "`$node01SignalPath = Join-Path `$remoteDscFolder 'Deploy-Cluster.Completed.signal'"
        ) | Should Be $true
        $driverReadiness.Contains(
            "`$signalPath = Join-Path `$remoteDscFolder `$node.Signal"
        ) | Should Be $true
        $driverReadiness.Contains(
            "`$remoteReadinessScript = Join-Path `$remoteDscFolder 'Scripts\Test-ClusterReadiness.ps1'"
        ) | Should Be $true
        $driverReadiness.Contains(
            '-ArgumentList $remoteReadinessScript, $ConfigureFile'
        ) | Should Be $true
    }

    It 'resolves Azure Cluster endpoints through dual-subnet load-balancer DNS' {
        $driverConfig = Read-AutomationFile 'shared\DSC\Driver-Configuration.ps1'
        $dnsRecords = Read-AutomationFile 'shared\DSC\Scripts\Create-DNSRecords.ps1'

        $driverConfig.Contains('if ($isAzureCluster) { continue }') | Should Be $true
        $driverConfig.Contains('$epIp = $endpoint.IpConfig[0].Ip') | Should Be $true
        $driverConfig.Contains('$node01Ip') | Should Be $false
        $dnsRecords.Contains('foreach ($ip in $cluster.IpConfig.Ip)') | Should Be $true
        $dnsRecords.Contains('foreach ($ip in $generalfs.IpConfig.Ip)') | Should Be $true
    }

    It 'writes Driver completion before final phase persistence' {
        $driver = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterDriver.ps1'
        $signal = $driver.IndexOf('Write-VerifiedDeploymentSignal -Path $signalFile')
        $verify = $driver.IndexOf(
            'Test-VerifiedDeploymentSignal -Path $signalFile',
            $signal
        )
        $phase = $driver.LastIndexOf(
            'Set-DeploymentPhase -Name $phaseRegistryName -Phase 3'
        )

        ($signal -ge 0) | Should Be $true
        ($verify -gt $signal) | Should Be $true
        ($phase -gt $verify) | Should Be $true
    }
}
