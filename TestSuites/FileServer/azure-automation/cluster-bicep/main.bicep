var location = resourceGroup().location

@description('Environment resource-name prefix')
param environmentPrefix string = 'fstest-cluster'

param adminUsername string = 'testadmin'

@secure()
param adminPassword string

@description('Published, manifest-validated Cluster-Package.zip URL')
@minLength(1)
@secure()
param clusterPackageZipUrl string = 'https://github.com/anamikoye/wpts-deploy-test/releases/download/cluster-test-v1/Cluster-Package.zip'

param domainName string = 'contoso.com'
param domainNetBiosName string = 'CONTOSO'
param clusterName string = 'Cluster01'
param generalFSName string = 'GeneralFS'
param scaleOutFSName string = 'ScaleoutFS'
param infraFSName string = 'InfraFS'

param vnetAddressPrefix string = '192.168.0.0/16'
param bastionSubnetPrefix string = '192.168.0.0/26'
param external1SubnetPrefix string = '192.168.1.0/24'
param external2SubnetPrefix string = '192.168.2.0/24'

@allowed([
  'Basic'
  'Standard'
])
param bastionSku string = 'Basic'

@description('Domain Controller VM size')
@allowed([
  'Standard_B4ms'
  'Standard_D2s_v5'
  'Standard_D2as_v5'
  'Standard_D4s_v5'
  'Standard_D4as_v5'
  'Standard_D4s_v6'
])
param dcVmSize string = 'Standard_B4ms'

@description('Storage server VM size')
@allowed([
  'Standard_B4ms'
  'Standard_F4s_v2'
  'Standard_D4s_v5'
  'Standard_D4as_v5'
  'Standard_D4s_v6'
  'Standard_D4as_v6'
])
param storageVmSize string = 'Standard_B4ms'

@description('VM size used by both Cluster nodes')
@allowed([
  'Standard_B4ms'
  'Standard_B8ms'
  'Standard_D8ls_v5'
  'Standard_D8s_v5'
  'Standard_D8as_v5'
  'Standard_D8s_v6'
  'Standard_D8as_v6'
])
param clusterNodeVmSize string = 'Standard_B8ms'

@description('Driver computer VM size')
@allowed([
  'Standard_B4ms'
  'Standard_F4as_v6'
  'Standard_F4s_v2'
  'Standard_D4s_v5'
  'Standard_D4as_v5'
  'Standard_D4s_v6'
])
param driverVmSize string = 'Standard_B4ms'

@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param dcOsVersion string = '2025-datacenter-azure-edition'

@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param storageOsVersion string = '2022-datacenter-g2'

@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param clusterNodeOsVersion string = '2025-datacenter-azure-edition'

@allowed([
  'win11-25h2-pro'
  'win11-25h2-ent'
  'win11-24h2-ent'
  'win11-23h2-pro'
  'win11-23h2-ent'
  'win10-22h2-pro'
  'win10-22h2-ent'
])
param driverOsVersion string = 'win11-25h2-ent'

param dcCustomImageId string = ''
param storageCustomImageId string = ''
param clusterNodeCustomImageId string = ''
param driverCustomImageId string = ''

param dcExternal1Ip string = '192.168.1.10'
param dcExternal2Ip string = '192.168.2.10'
param storageExternal1Ip string = '192.168.1.50'
param node01External1Ip string = '192.168.1.11'
param node01External2Ip string = '192.168.2.11'
param node02External1Ip string = '192.168.1.12'
param node02External2Ip string = '192.168.2.12'
param driverExternal1Ip string = '192.168.1.111'
param driverExternal2Ip string = '192.168.2.111'
param clusterExternal1Ip string = '192.168.1.100'
param clusterExternal2Ip string = '192.168.2.100'
param generalFSExternal1Ip string = '192.168.1.200'
param generalFSExternal2Ip string = '192.168.2.200'

param clusterExternal1ProbePort int = 59998
param clusterExternal2ProbePort int = 59999
param generalFSExternal1ProbePort int = 60000
param generalFSExternal2ProbePort int = 60001

param enableAutoShutdown bool = false
@description('Run FileServer tests automatically after Cluster configuration completes')
param enableTestAutoRun bool = true
param autoShutdownTime string = '20:00'
param autoShutdownTimeZone string = 'UTC'
param enableDiskEncryption bool = true
param forceUpdateTag string = utcNow('yyyyMMddHHmmss')

var oneClickConfig = {
  Core: {
    Username: adminUsername
    Password: '#{ADMIN_PASSWORD}#'
    TestSuiteName: 'FileServer'
    Scenario: 'Cluster'
    DomainName: domainName
    RegressionType: 'Azure'
    UseAgent: 'false'
    UsePasswordForAllUsers: 'true'
  }
  TestExecution: {
    AutoRun: enableTestAutoRun
  }
  Machines: {
    DC: {
      Role: 'DC'
      HyperVName: 'DC01'
      ComputerName: 'DC01'
      IsClusterNode: 'false'
      Domain: domainName
      IpConfig: [
        {
          NIC: 'External1'
          Ip: dcExternal1Ip
          Subnet: '255.255.255.0'
          Gateway: ''
          DNSServer: '127.0.0.1'
        }
        {
          NIC: 'External2'
          Ip: dcExternal2Ip
          Subnet: '255.255.255.0'
          Gateway: ''
          DNSServer: '127.0.0.1'
        }
      ]
    }
    Storage: {
      Role: 'Storage'
      HyperVName: 'Storage01'
      ComputerName: 'Storage01'
      IsClusterNode: 'false'
      Domain: 'Workgroup'
      iSCSITarget: true
      iSCSITargetName: 'ClusterTarget'
      IpConfig: [
        {
          NIC: 'External1'
          Ip: storageExternal1Ip
          Subnet: '255.255.255.0'
          Gateway: ''
          DNSServer: ''
        }
      ]
    }
    Node01: {
      Role: 'Node01'
      HyperVName: 'Node01'
      ComputerName: 'Node01'
      IsClusterNode: 'true'
      Domain: domainName
      IpConfig: [
        {
          NIC: 'External1'
          Ip: node01External1Ip
          Subnet: '255.255.255.0'
          Gateway: dcExternal1Ip
          DNSServer: dcExternal1Ip
        }
        {
          NIC: 'External2'
          Ip: node01External2Ip
          Subnet: '255.255.255.0'
          Gateway: dcExternal2Ip
          DNSServer: dcExternal2Ip
        }
      ]
    }
    Node02: {
      Role: 'Node02'
      HyperVName: 'Node02'
      ComputerName: 'Node02'
      IsClusterNode: 'true'
      Domain: domainName
      IpConfig: [
        {
          NIC: 'External1'
          Ip: node02External1Ip
          Subnet: '255.255.255.0'
          Gateway: dcExternal1Ip
          DNSServer: dcExternal1Ip
        }
        {
          NIC: 'External2'
          Ip: node02External2Ip
          Subnet: '255.255.255.0'
          Gateway: dcExternal2Ip
          DNSServer: dcExternal2Ip
        }
      ]
    }
    DriverComputer: {
      Role: 'DriverComputer'
      HyperVName: 'Driver'
      ComputerName: 'Client01'
      IsClusterNode: 'false'
      OSType: 'Windows'
      Domain: domainName
      IpConfig: [
        {
          NIC: 'External1'
          Ip: driverExternal1Ip
          Subnet: '255.255.255.0'
          Gateway: dcExternal1Ip
          DNSServer: dcExternal1Ip
        }
        {
          NIC: 'External2'
          Ip: driverExternal2Ip
          Subnet: '255.255.255.0'
          Gateway: dcExternal2Ip
          DNSServer: dcExternal2Ip
        }
      ]
    }
  }
  Endpoints: {
    Cluster: {
      Name: clusterName
      IpConfig: [
        {
          NIC: 'External1'
          Ip: clusterExternal1Ip
          ProbePort: clusterExternal1ProbePort
        }
        {
          NIC: 'External2'
          Ip: clusterExternal2Ip
          ProbePort: clusterExternal2ProbePort
        }
      ]
    }
    GeneralFS: {
      Name: generalFSName
      IpConfig: [
        {
          NIC: 'External1'
          Ip: generalFSExternal1Ip
          ProbePort: generalFSExternal1ProbePort
        }
        {
          NIC: 'External2'
          Ip: generalFSExternal2Ip
          ProbePort: generalFSExternal2ProbePort
        }
      ]
    }
    InfrastructureFS: {
      Name: infraFSName
    }
    ScaleoutFS: {
      Name: scaleOutFSName
    }
  }
  Domain: {
    Name: domainName
    NetBiosName: domainNetBiosName
  }
}

var oneClickConfigBase64 = base64(string(oneClickConfig))
var encryptionScript = '''
param(
  [string]$ResourceGroupName,
  [string]$VmName1,
  [string]$VmName2,
  [string]$VmName3 = '',
  [bool]$EnableEncryption,
  [string]$KeyVaultUrl,
  [string]$KeyVaultId
)
$ErrorActionPreference = 'Stop'
foreach ($vmName in @($VmName1, $VmName2, $VmName3) | Where-Object {
  -not [string]::IsNullOrWhiteSpace($_)
}) {
  $submitted = $false
  for ($attempt = 1; $attempt -le 20 -and -not $submitted; $attempt++) {
    try {
      $null = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $vmName -ErrorAction Stop
      if ($EnableEncryption) {
        $status = Get-AzVMDiskEncryptionStatus -ResourceGroupName $ResourceGroupName `
          -VMName $vmName -ErrorAction SilentlyContinue
        if ($status.OsVolumeEncrypted -ne 'Encrypted') {
          Set-AzVMDiskEncryptionExtension -ResourceGroupName $ResourceGroupName `
            -VMName $vmName -DiskEncryptionKeyVaultUrl $KeyVaultUrl `
            -DiskEncryptionKeyVaultId $KeyVaultId -VolumeType OS `
            -SkipVmBackup -Force -ErrorAction Stop | Out-Null
        }
      }
      $submitted = $true
    }
    catch {
      if ($attempt -eq 20) { throw }
      Start-Sleep 15
    }
  }

  $deadline = (Get-Date).AddMinutes(30)
  do {
    $vm = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $vmName `
      -Status -ErrorAction SilentlyContinue
    $running = @($vm.Statuses | Where-Object Code -eq 'PowerState/running').Count -gt 0
    $agentReady = @($vm.VMAgent.Statuses | Where-Object {
      $_.Code -eq 'ProvisioningState/succeeded'
    }).Count -gt 0
    if ($running -and $agentReady) { break }
    Start-Sleep 15
  } while ((Get-Date) -lt $deadline)

  if (-not ($running -and $agentReady)) {
    throw "VM '$vmName' did not become ready after disk encryption."
  }
}
$DeploymentScriptOutputs = @{ complete = $true }
'''
var phase1ReadinessScript = '''
param([string]$ResourceGroupName, [string]$DcVmName, [string]$StorageVmName)
$ErrorActionPreference = 'Stop'

function Invoke-Probe([string]$VmName, [string]$Script) {
  $job = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
    -VMName $VmName -CommandId RunPowerShellScript -ScriptString $Script `
    -AsJob -ErrorAction Stop
  try {
    if (-not (Wait-Job $job -Timeout 120)) {
      Stop-Job $job
      return $false
    }
    $result = Receive-Job $job -ErrorAction Stop
    return (@($result.Value.Message) -join "`n") -match '(?im)^\s*True\s*$'
  }
  finally {
    Remove-Job $job -Force -ErrorAction SilentlyContinue
  }
}

$dcScript = @'
$item = Get-Item 'C:\Cluster-Package\DSC\Deploy-DC.Completed.signal' -ErrorAction SilentlyContinue
$ready = $null -ne $item -and $item.Length -gt 0
if ($ready) {
  try {
    Import-Module ActiveDirectory -ErrorAction Stop
    $ready = $null -ne (Get-ADDomain -ErrorAction Stop) -and
      (Test-Path "\\$env:COMPUTERNAME\SYSVOL") -and
      (Test-Path "\\$env:COMPUTERNAME\NETLOGON")
  }
  catch {
    $ready = $false
  }
}
Write-Output $ready
'@

$storageScript = @'
$item = Get-Item 'C:\Cluster-Package\DSC\Deploy-Storage.Completed.signal' -ErrorAction SilentlyContinue
if ($null -eq $item -or $item.Length -le 0) {
  Write-Output $false
  return
}
& 'C:\Cluster-Package\DSC\Scripts\Test-StorageReadiness.ps1' `
  -ConfigureFile 'C:\Cluster-Package\Config.json'
'@

$deadline = (Get-Date).AddHours(2)
do {
  $dcReady = Invoke-Probe $DcVmName $dcScript
  $storageReady = Invoke-Probe $StorageVmName $storageScript
  if ($dcReady -and $storageReady) { break }
  Start-Sleep 30
} while ((Get-Date) -lt $deadline)

if (-not ($dcReady -and $storageReady)) {
  throw 'Phase 1 readiness timed out.'
}
$DeploymentScriptOutputs = @{ complete = $true }
'''
var finalReadinessScript = '''
param(
  [string]$ResourceGroupName,
  [string]$Node01VmName,
  [string]$Node02VmName,
  [string]$DriverVmName
)
$ErrorActionPreference = 'Stop'

function Invoke-Probe([string]$VmName, [string]$Script) {
  $job = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
    -VMName $VmName -CommandId RunPowerShellScript -ScriptString $Script `
    -AsJob -ErrorAction Stop
  try {
    if (-not (Wait-Job $job -Timeout 180)) {
      Stop-Job $job
      return $false
    }
    $result = Receive-Job $job -ErrorAction Stop
    return (@($result.Value.Message) -join "`n") -match '(?im)^\s*True\s*$'
  }
  finally {
    Remove-Job $job -Force -ErrorAction SilentlyContinue
  }
}

$node01Script = @'
$signal = Get-Item 'C:\Cluster-Package\DSC\Deploy-Node01.Completed.signal' -ErrorAction SilentlyContinue
if ($null -eq $signal -or $signal.Length -le 0) {
  Write-Output $false
  return
}
& 'C:\Cluster-Package\DSC\Scripts\Test-ClusterReadiness.ps1' `
  -ConfigureFile 'C:\Cluster-Package\Config.json'
'@

$node02Script = @'
$signal = Get-Item 'C:\Cluster-Package\DSC\Deploy-Node02.Completed.signal' -ErrorAction SilentlyContinue
Write-Output ($null -ne $signal -and $signal.Length -gt 0)
'@

$driverScript = @'
$signal = Get-Item 'C:\Cluster-Package\DSC\Deploy-Driver.Completed.signal' -ErrorAction SilentlyContinue
if ($null -eq $signal -or $signal.Length -le 0) {
  Write-Output $false
  return
}
& 'C:\Cluster-Package\DSC\Scripts\Test-ClusterDriverReadiness.ps1' `
  -WorkingPath 'C:\Cluster-Package' `
  -ConfigureFile 'C:\Cluster-Package\Config.json'
'@

$deadline = (Get-Date).AddHours(3)
do {
  $node01Ready = Invoke-Probe $Node01VmName $node01Script
  $node02Ready = Invoke-Probe $Node02VmName $node02Script
  $driverReady = Invoke-Probe $DriverVmName $driverScript
  if ($node01Ready -and $node02Ready -and $driverReady) { break }
  Start-Sleep 30
} while ((Get-Date) -lt $deadline)

if (-not ($node01Ready -and $node02Ready -and $driverReady)) {
  throw 'Final Cluster readiness timed out.'
}
$DeploymentScriptOutputs = @{ complete = $true }
'''

resource orchestrationIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: '${environmentPrefix}-orchestration'
  location: location
}

resource orchestrationContributor 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(resourceGroup().id, orchestrationIdentity.id, 'cluster-orchestration-contributor')
  properties: {
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      'b24988ac-6180-42a0-ab88-20f7382dd24c'
    )
    principalId: orchestrationIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

module phase1 'phase1.bicep' = {
  name: '${environmentPrefix}-phase1'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    dcVmSize: dcVmSize
    storageVmSize: storageVmSize
    dcOsVersion: dcOsVersion
    storageOsVersion: storageOsVersion
    dcCustomImageId: dcCustomImageId
    storageCustomImageId: storageCustomImageId
    vnetAddressPrefix: vnetAddressPrefix
    bastionSubnetPrefix: bastionSubnetPrefix
    external1SubnetPrefix: external1SubnetPrefix
    external2SubnetPrefix: external2SubnetPrefix
    dcExternal1Ip: dcExternal1Ip
    dcExternal2Ip: dcExternal2Ip
    storageExternal1Ip: storageExternal1Ip
    bastionSku: bastionSku
    domainName: domainName
    domainNetBiosName: domainNetBiosName
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    enableAutoShutdown: enableAutoShutdown
    clusterPackageZipUrl: ''
    enableDiskEncryption: enableDiskEncryption
  }
}

resource phase1Encryption 'Microsoft.Resources/deploymentScripts@2023-08-01' = {
  name: '${environmentPrefix}-phase1-encryption'
  location: location
  kind: 'AzurePowerShell'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${orchestrationIdentity.id}': {}
    }
  }
  properties: {
    azPowerShellVersion: '11.3'
    timeout: 'PT2H'
    retentionInterval: 'P1D'
    cleanupPreference: 'OnSuccess'
    arguments: '-ResourceGroupName "${resourceGroup().name}" -VmName1 "${phase1.outputs.domainControllerName}" -VmName2 "${phase1.outputs.storageServerName}" -EnableEncryption ${enableDiskEncryption ? 1 : 0} -KeyVaultUrl "${enableDiskEncryption ? phase1.outputs.keyVaultUrl : 'unused'}" -KeyVaultId "${enableDiskEncryption ? phase1.outputs.keyVaultId : 'unused'}"'
    scriptContent: encryptionScript
    forceUpdateTag: '${forceUpdateTag}-phase1-encryption'
  }
  dependsOn: [
    orchestrationContributor
  ]
}

module serviceExtensions 'modules/service-extensions.bicep' = {
  name: '${environmentPrefix}-service-extensions'
  params: {
    location: location
    dcVmName: phase1.outputs.domainControllerName
    storageVmName: phase1.outputs.storageServerName
    adminPassword: adminPassword
    clusterPackageZipUrl: clusterPackageZipUrl
    configJsonBase64: oneClickConfigBase64
    forceUpdateTag: '${forceUpdateTag}-phase1'
  }
  dependsOn: [
    phase1Encryption
  ]
}

resource waitPhase1 'Microsoft.Resources/deploymentScripts@2023-08-01' = {
  name: '${environmentPrefix}-wait-phase1'
  location: location
  kind: 'AzurePowerShell'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${orchestrationIdentity.id}': {}
    }
  }
  properties: {
    azPowerShellVersion: '11.3'
    timeout: 'PT3H'
    retentionInterval: 'P1D'
    cleanupPreference: 'OnSuccess'
    arguments: '-ResourceGroupName "${resourceGroup().name}" -DcVmName "${phase1.outputs.domainControllerName}" -StorageVmName "${phase1.outputs.storageServerName}"'
    scriptContent: phase1ReadinessScript
    forceUpdateTag: '${forceUpdateTag}-wait-phase1'
  }
  dependsOn: [
    serviceExtensions
  ]
}

module phase2 'phase2.bicep' = {
  name: '${environmentPrefix}-phase2'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    clusterNodeVmSize: clusterNodeVmSize
    driverVmSize: driverVmSize
    clusterNodeOsVersion: clusterNodeOsVersion
    driverOsType: 'Windows'
    driverOsVersion: driverOsVersion
    driverLinuxOsVersion: 'server'
    driverCustomImageId: driverCustomImageId
    clusterNodeCustomImageId: clusterNodeCustomImageId
    external1SubnetId: phase1.outputs.external1SubnetId
    external2SubnetId: phase1.outputs.external2SubnetId
    node01External1Ip: node01External1Ip
    node01External2Ip: node01External2Ip
    node02External1Ip: node02External1Ip
    node02External2Ip: node02External2Ip
    driverExternal1Ip: driverExternal1Ip
    driverExternal2Ip: driverExternal2Ip
    dcExternal1Ip: dcExternal1Ip
    clusterExternal1Ip: clusterExternal1Ip
    clusterExternal2Ip: clusterExternal2Ip
    generalFSExternal1Ip: generalFSExternal1Ip
    generalFSExternal2Ip: generalFSExternal2Ip
    clusterExternal1ProbePort: clusterExternal1ProbePort
    clusterExternal2ProbePort: clusterExternal2ProbePort
    generalFSExternal1ProbePort: generalFSExternal1ProbePort
    generalFSExternal2ProbePort: generalFSExternal2ProbePort
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    enableAutoShutdown: enableAutoShutdown
    enableTestAutoRun: enableTestAutoRun
    clusterPackageZipUrl: ''
  }
  dependsOn: [
    waitPhase1
  ]
}

resource phase2Encryption 'Microsoft.Resources/deploymentScripts@2023-08-01' = {
  name: '${environmentPrefix}-phase2-encryption'
  location: location
  kind: 'AzurePowerShell'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${orchestrationIdentity.id}': {}
    }
  }
  properties: {
    azPowerShellVersion: '11.3'
    timeout: 'PT2H'
    retentionInterval: 'P1D'
    cleanupPreference: 'OnSuccess'
    arguments: '-ResourceGroupName "${resourceGroup().name}" -VmName1 "${phase2.outputs.node01Name}" -VmName2 "${phase2.outputs.node02Name}" -VmName3 "${phase2.outputs.driverComputerName}" -EnableEncryption ${enableDiskEncryption ? 1 : 0} -KeyVaultUrl "${enableDiskEncryption ? phase1.outputs.keyVaultUrl : 'unused'}" -KeyVaultId "${enableDiskEncryption ? phase1.outputs.keyVaultId : 'unused'}"'
    scriptContent: encryptionScript
    forceUpdateTag: '${forceUpdateTag}-phase2-encryption'
  }
  dependsOn: [
    orchestrationContributor
  ]
}

module computerExtensions 'modules/computer-extensions.bicep' = {
  name: '${environmentPrefix}-computer-extensions'
  params: {
    location: location
    node01VmName: phase2.outputs.node01Name
    node02VmName: phase2.outputs.node02Name
    driverVmName: phase2.outputs.driverComputerName
    adminPassword: adminPassword
    clusterPackageZipUrl: clusterPackageZipUrl
    configJsonBase64: oneClickConfigBase64
    forceUpdateTag: '${forceUpdateTag}-phase2'
  }
  dependsOn: [
    phase2Encryption
  ]
}

resource waitFinal 'Microsoft.Resources/deploymentScripts@2023-08-01' = {
  name: '${environmentPrefix}-wait-final'
  location: location
  kind: 'AzurePowerShell'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${orchestrationIdentity.id}': {}
    }
  }
  properties: {
    azPowerShellVersion: '11.3'
    timeout: 'PT4H'
    retentionInterval: 'P1D'
    cleanupPreference: 'OnSuccess'
    arguments: '-ResourceGroupName "${resourceGroup().name}" -Node01VmName "${phase2.outputs.node01Name}" -Node02VmName "${phase2.outputs.node02Name}" -DriverVmName "${phase2.outputs.driverComputerName}"'
    scriptContent: finalReadinessScript
    forceUpdateTag: '${forceUpdateTag}-wait-final'
  }
  dependsOn: [
    computerExtensions
  ]
}

output domainControllerName string = phase1.outputs.domainControllerName
output storageServerName string = phase1.outputs.storageServerName
output node01Name string = phase2.outputs.node01Name
output node02Name string = phase2.outputs.node02Name
output driverName string = phase2.outputs.driverComputerName
output clusterLoadBalancerId string = phase2.outputs.clusterLoadBalancerId
output bastionFqdn string = phase1.outputs.bastionFqdn
output deploymentReady bool = waitFinal.properties.outputs.complete
