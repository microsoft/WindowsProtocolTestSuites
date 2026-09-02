# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path
$storageReadinessScript = Join-Path $root 'cluster-bicep\DSC\Scripts\Test-StorageReadiness.ps1'

foreach ($commandName in @('Get-IscsiServerTarget', 'Get-IscsiVirtualDisk')) {
    if (-not (Get-Command $commandName -ErrorAction SilentlyContinue)) {
        Set-Item -Path "Function:\global:$commandName" -Value { }
    }
}

function Read-AutomationFile {
    param([string]$RelativePath)
    return Get-Content -Path (Join-Path $root $RelativePath) -Raw
}

Describe 'Deterministic Cluster Storage readiness' {
    It 'places new virtual disks on the Storage data disk and preserves legacy C mappings' {
        $steps = Read-AutomationFile 'cluster-bicep\DSC\Invoke-StorageImperativeSteps.ps1'

        $steps.Contains("'C:\StorageData'") | Should Be $true
        $steps.Contains("'C:\iSCSIVirtualDisks'") | Should Be $true
        $steps.Contains("Location -match 'LUN 0") | Should Be $true
        $steps.Contains('Initialize-Disk') | Should Be $true
        $steps.Contains('Add-PartitionAccessPath') | Should Be $true
        $steps.Contains('already exists -- skipping creation') | Should Be $false
    }

    It 'repairs the configured target without destructive disk operations' {
        $steps = Read-AutomationFile 'cluster-bicep\DSC\Invoke-StorageImperativeSteps.ps1'

        $steps.Contains('New-IscsiServerTarget') | Should Be $true
        $steps.Contains('Set-IscsiServerTarget') | Should Be $true
        $steps.Contains('New-IscsiVirtualDisk') | Should Be $true
        $steps.Contains('Add-IscsiVirtualDiskTargetMapping') | Should Be $true
        $steps.Contains('Remove-Iscsi') | Should Be $false
        $steps.Contains('Clear-Disk') | Should Be $false
        $steps.Contains('IQN:*') | Should Be $true
        $steps.Contains('IPAddress:') | Should Be $true
    }

    It 'requires exact target, service, listener, disk, and mapping postconditions' {
        $readiness = Read-AutomationFile 'cluster-bicep\DSC\Scripts\Test-StorageReadiness.ps1'

        $readiness.Contains('Get-IscsiServerTarget') | Should Be $true
        $readiness.Contains('Get-IscsiVirtualDisk') | Should Be $true
        $readiness.Contains('Get-NetTCPConnection') | Should Be $true
        $readiness.Contains('WinTarget') | Should Be $true
        $readiness.Contains('3260') | Should Be $true
        foreach ($name in @('disk1.vhdx', 'disk2.vhdx', 'disk3.vhdx', 'diskq.vhdx')) {
            $readiness.Contains($name) | Should Be $true
        }
        $readiness.Contains('$mappingPaths.Count -ne $diskSpecs.Count') |
            Should Be $true
    }

    It 'does not create an unbounded recurring Storage checker task' {
        $status = Read-AutomationFile 'cluster-bicep\DSC\Scripts\Check-StorageStatus.ps1'

        $status.Contains('schtasks /Create') | Should Be $false
        $status.Contains('Test-StorageReadiness.ps1') | Should Be $true
    }
}

Describe 'Storage readiness postcondition behavior' {
    BeforeEach {
        $testRoot = Join-Path $env:TEMP "StorageReadiness-$([guid]::NewGuid().ToString('N'))"
        New-Item -ItemType Directory -Path $testRoot -Force | Out-Null
        $configPath = Join-Path $testRoot 'Config.json'
        @{
            Machines = @{
                Storage = @{ iSCSITargetName = 'ClusterTarget' }
                Node01 = @{ IpConfig = @(@{ Ip = '192.168.1.11' }) }
                Node02 = @{ IpConfig = @(@{ Ip = '192.168.1.12' }) }
            }
        } | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $configPath

        $global:storageReadinessMappingPaths = foreach ($name in @(
            'disk1.vhdx', 'disk2.vhdx', 'disk3.vhdx', 'diskq.vhdx'
        )) {
            $path = Join-Path $testRoot $name
            'vhdx' | Set-Content -LiteralPath $path
            $path
        }

        $global:storageReadinessInitiators = @(
            [pscustomobject]@{ Value = 'IPAddress:192.168.1.11' },
            [pscustomobject]@{ Value = 'IPAddress:192.168.1.12' }
        )
        foreach ($initiator in $global:storageReadinessInitiators) {
            $initiator | Add-Member -MemberType ScriptMethod -Name ToString -Force -Value {
                return $this.Value
            }
        }

        Mock Get-IscsiServerTarget {
            [pscustomobject]@{
                TargetName = 'ClusterTarget'
                InitiatorIds = $global:storageReadinessInitiators
                LunMappings = @($global:storageReadinessMappingPaths | ForEach-Object {
                    [pscustomobject]@{ Path = $_ }
                })
            }
        }

        Mock Get-IscsiVirtualDisk {
            $size = if ([IO.Path]::GetFileName($Path) -eq 'diskq.vhdx') {
                [long](1GB)
            } else {
                [long](10GB)
            }
            [pscustomobject]@{ Path = $Path; Size = $size }
        }
        Mock Get-CimInstance {
            [pscustomobject]@{
                Name = 'WinTarget'
                StartMode = 'Auto'
                State = 'Running'
            }
        }

        Mock Get-NetTCPConnection {
            [pscustomobject]@{ LocalPort = 3260; State = 'Listen' }
        }
    }

    AfterEach {
        Remove-Item $testRoot -Recurse -Force -ErrorAction SilentlyContinue
        $global:storageReadinessMappingPaths = $null
        $global:storageReadinessInitiators = $null
    }

    It 'accepts the exact four-disk target layout' {
        $output = @(& $storageReadinessScript -ConfigureFile $configPath -Detailed)

        $output[-1] | Should Be $true
    }

    It 'rejects a missing LUN mapping' {
        $global:storageReadinessMappingPaths = @(
            $global:storageReadinessMappingPaths | Select-Object -First 3
        )

        $output = @(& $storageReadinessScript -ConfigureFile $configPath)

        $output[-1] | Should Be $false
    }
}

Describe 'Deterministic Cluster node foundation readiness' {
    It 'requires domain trust, convergence, tools, SSH, and the exact iSCSI target' {
        $readiness = Read-AutomationFile `
            'cluster-bicep\DSC\Scripts\Test-NodeFoundationReadiness.ps1'

        $readiness.Contains('Test-ComputerSecureChannel') | Should Be $true
        $readiness.Contains('InstallMSIAndTools.Completed.signal') | Should Be $true
        $readiness.Contains(
            '[uint32]$value.$($registryCheck.Name) -ne [uint32]$registryCheck.Value'
        ) | Should Be $true
        $readiness.Contains('[int]$value.$($registryCheck.Name)') | Should Be $false
        $readiness.Contains('Get-IscsiSession') | Should Be $true
        $readiness.Contains('Get-IscsiTarget') | Should Be $true
        $readiness.Contains('BusType -eq ''iSCSI''') | Should Be $true
        $readiness.Contains('$iscsiDisks.Count -ne 4') | Should Be $true
        $readiness.Contains('Get-Service sshd') | Should Be $true
        $readiness.Contains('Get-SmbShare') | Should Be $true
        $readiness.Contains('Get-NetFirewallProfile') | Should Be $true
    }

    It 'connects only to the configured Storage target with bounded waits' {
        $deploy = Read-AutomationFile 'cluster-bicep\DSC\Deploy-ClusterNode.ps1'

        $deploy.Contains('Wait-DeploymentCondition') | Should Be $true
        $deploy.Contains('$expectedTargetAddress') | Should Be $true
        $deploy.Contains('Connect-IscsiTarget -NodeAddress $expectedTargetAddress') |
            Should Be $true
        $deploy.Contains('foreach ($target in $targets)') | Should Be $false
    }
}

Describe 'Non-destructive Cluster formation and live readiness' {
    It 'repairs Cluster membership, roles, storage, and Azure IP resources' {
        $formation = Read-AutomationFile `
            'cluster-bicep\DSC\Scripts\Create-ServerFailoverEnv.ps1'
        $readiness = Read-AutomationFile `
            'cluster-bicep\DSC\Scripts\Test-ClusterReadiness.ps1'

        $formation.Contains('Add-ClusterNode') | Should Be $true
        $formation.Contains('Get-ClusterAvailableDisk') | Should Be $true
        $formation.Contains("Test-Path 'HKLM:\Cluster'") | Should Be $true
        ([regex]::Matches($formation, 'return ,\$guids').Count) | Should Be 2
        $formation.Contains('Get-ClusterNode -Cluster $clusterName') | Should Be $false
        $formation.Contains('Get-Cluster -Name $clusterName') | Should Be $false
        $formation.Contains("PSObject.Properties['SharedVolumeInfo']") |
            Should Be $true
        $formation.Contains('Get-ClusterDiskResource') | Should Be $true
        $formation.Contains('System.Collections.Generic.HashSet[string]') |
            Should Be $true
        $diskInfoStart = $formation.IndexOf('function Get-PhysicalDiskInfos')
        $csvEnumeration = $formation.IndexOf(
            '$resources = @(Get-ClusterSharedVolume -ErrorAction SilentlyContinue)',
            $diskInfoStart
        )
        $physicalEnumeration = $formation.IndexOf(
            '$resources += @(Get-ClusterResource',
            $diskInfoStart
        )
        ($diskInfoStart -ge 0) | Should Be $true
        ($csvEnumeration -gt $diskInfoStart) | Should Be $true
        ($physicalEnumeration -gt $csvEnumeration) |
            Should Be $true
        $formation.Contains('$seenDisks.Add($diskIdentity)') | Should Be $true
        $formation.Contains('Set-ClusterParameter -Multiple') | Should Be $true
        $formation.Contains("@('', '0.0.0.0')") | Should Be $true
        $formation.Contains("'Distributed Network Name'") | Should Be $true
        $readiness.Contains('$ownedDiskNames = @(') | Should Be $true
        $readiness.Contains('@($physicalDisks.Name)') | Should Be $true
        $readiness.Contains('@($csvs.Name)') | Should Be $true
        $readiness.Contains('if ($ownedDiskNames.Count -ne 4)') |
            Should Be $true
        $formation.Contains("'Infrastructure File Server'") | Should Be $true
        $readiness.Contains("ResourceType -eq 'Distributed Network Name'") |
            Should Be $true
        $readiness.Contains('-Server $dnsServer') | Should Be $true
        $readiness.Contains('New-CimSession -ComputerName $ownerNode') |
            Should Be $true
        $formation.Contains("BusType -eq 'iSCSI'") | Should Be $true
        $formation.Contains('ProbePort') | Should Be $true
        $formation.Contains('OverrideAddressMatch') | Should Be $true
        $formation.Contains('255.255.255.255') | Should Be $true
        $formation.Contains('Set-ClusterResourceDependency') | Should Be $true
    }

    It 'never deletes Cluster identities or cleans/formats owned disks on rerun' {
        $formation = Read-AutomationFile `
            'cluster-bicep\DSC\Scripts\Create-ServerFailoverEnv.ps1'

        $formation.Contains('Remove-ADObject') | Should Be $false
        $formation.Contains('Remove-DnsServerResourceRecord') | Should Be $false
        $formation.Contains('clean') | Should Be $false
        $formation.Contains('Clear-Disk') | Should Be $false
        $formation.Contains('Remove-Cluster') | Should Be $false
    }

    It 'checks nodes, resources, quorum, roles, shares, and Azure probe parameters' {
        $readiness = Read-AutomationFile `
            'cluster-bicep\DSC\Scripts\Test-ClusterReadiness.ps1'

        foreach ($text in @(
            'Get-ClusterNode',
            'Get-ClusterResource',
            'Get-ClusterQuorum',
            'Get-ClusterSharedVolume',
            'Get-SmbShare',
            'ProbePort',
            'OverrideAddressMatch',
            'GeneralFS',
            'ScaleoutFS'
        )) {
            $readiness.Contains($text) | Should Be $true
        }
    }
}

Describe 'Dual-subnet Cluster load balancer contract' {
    It 'deploys a Standard ILB before Cluster nodes and attaches every node NIC' {
        $phase2 = Read-AutomationFile 'cluster-bicep\phase2.bicep'
        $nodes = Read-AutomationFile 'cluster-bicep\modules\cluster-nodes.bicep'
        $loadBalancer = Read-AutomationFile `
            'cluster-bicep\modules\cluster-load-balancer.bicep'
        $network = Read-AutomationFile 'cluster-bicep\modules\network.bicep'

        $loadBalancer.Contains("name: 'Standard'") | Should Be $true
        ([regex]::Matches($loadBalancer, "name: '.*-frontend'").Count) |
            Should Be 4
        $loadBalancer.Contains('enableFloatingIP: true') | Should Be $true
        $loadBalancer.Contains("protocol: 'All'") | Should Be $true
        $phase2.IndexOf("module clusterLoadBalancer 'modules/cluster-load-balancer.bicep'") |
            Should BeGreaterThan -1
        ($phase2.IndexOf('module clusterNodes') -gt
            $phase2.IndexOf("module clusterLoadBalancer")) | Should Be $true
        $phase2.Contains('backendPoolId: clusterLoadBalancer.outputs.backendPoolId') |
            Should Be $true
        ([regex]::Matches($nodes, 'loadBalancerBackendAddressPools').Count) |
            Should Be 4
        $network.Contains('Microsoft.Network/natGateways') | Should Be $true
        $network.Contains('natGateway:') | Should Be $true
        $network.Contains('natGateway.id') | Should Be $true
    }
}

Describe 'Cluster Driver and test-run gates' {
    It 'sets and verifies ForceLevel2 on local and clustered shares' {
        $force = Read-AutomationFile `
            'shared\DSC\Scripts\Configure-ForceLevel2.ps1'
        $helpers = Read-AutomationFile 'shared\DSC\Deploy-CommonHelpers.ps1'

        $force.Contains('ShareForceLevel2') | Should Be $true
        $force.Contains('SMBClusteredForceLevel2') | Should Be $true
        $force.Contains('SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true') |
            Should Be $true
        $force.Contains('SHI1005_FLAGS_FORCE_LEVELII_OPLOCK') |
            Should Be $true
        $force.Contains('Test-ShareUtilForceLevel2Output') | Should Be $true
        $helpers.Contains('($value -band 0x1000) -ne 0') | Should Be $true
        $readiness = Read-AutomationFile `
            'cluster-bicep\DSC\Scripts\Test-ClusterDriverReadiness.ps1'
        $readiness.Contains('$actualTaskUser -ine "$($config.Core.Username)"') |
            Should Be $true
        $readiness.Contains("'Auth_ServerTestSuite.deployment.ptfconfig'") |
            Should Be $true
        $readiness.Contains("'ServicePassword') -ne") | Should Be $true
        $readiness.Contains("'ServiceSaltString') -cne") | Should Be $true
        $readiness.Contains('-Authentication Kerberos') | Should Be $true
        $force.Contains('ForceLevel2.Local.Completed.signal') | Should Be $true
        $force.Contains('ForceLevel2.Clustered.Completed.signal') | Should Be $true
        $force.Contains('Wait-DeploymentCondition') | Should Be $true
    }

    It 'revalidates Cluster readiness and aborts instead of starting tests early' {
        $testRun = Read-AutomationFile 'shared\DSC\Scripts\Invoke-TestRun.ps1'
        $environment = Read-AutomationFile `
            'shared\DSC\Scripts\Validate-Environment.ps1'

        $testRun.Contains('Test-ClusterDriverReadiness.ps1') | Should Be $true
        $testRun.Contains('Cluster readiness validation failed; tests will not start') |
            Should Be $true
        $testRun.Contains("if (-not `$isLinuxDriver -and `$scenario -ne 'Cluster')") |
            Should Be $true
        $testRun.Contains('Defaulting to WindowsServer2025') | Should Be $false
        $environment.Contains(
            'GeneralFS live validation failed in a deterministic Cluster deployment.'
        ) | Should Be $true
        $environment.Contains(
            'ScaleoutFS live validation failed in a deterministic Cluster deployment.'
        ) | Should Be $true
    }
}
