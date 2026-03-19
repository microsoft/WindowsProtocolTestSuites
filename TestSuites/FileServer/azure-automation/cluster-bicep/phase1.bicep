// Phase 1: Deploy infrastructure and domain services
// This phase deploys: Network, Domain Controller, Storage Server
// Domain-joined VMs (Nodes, Driver) are deployed in Phase 2 after DC is ready

@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix (e.g., fstest)')
param environmentPrefix string = 'fstest-cluster'

@description('Admin username for VMs')
param adminUsername string = 'testadmin'

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Domain Controller VM size')
param dcVmSize string = 'Standard_D4s_v5'

@description('Storage server VM size')
param storageVmSize string = 'Standard_D4s_v5'

@description('Domain Controller OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param dcOsVersion string = '2025-datacenter-azure-edition'

@description('Storage server OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param storageOsVersion string = '2022-datacenter-g2'

@description('Custom image resource ID for DC VM (overrides marketplace image when set). Must be a Windows image.')
param dcCustomImageId string = ''

@description('Custom image resource ID for storage VM (overrides marketplace image when set). Must be a Windows image.')
param storageCustomImageId string = ''

@description('Virtual network address space')
param vnetAddressPrefix string = '192.168.0.0/16'

@description('Azure Bastion subnet address prefix (must be /26 or larger)')
param bastionSubnetPrefix string = '192.168.0.0/26'

@description('External1 subnet address prefix')
param external1SubnetPrefix string = '192.168.1.0/24'

@description('External2 subnet address prefix')
param external2SubnetPrefix string = '192.168.2.0/24'

@description('Domain Controller External1 IP address')
param dcExternal1Ip string = '192.168.1.10'

@description('Domain Controller External2 IP address')
param dcExternal2Ip string = '192.168.2.10'

@description('Storage server External1 IP address')
param storageExternal1Ip string = '192.168.1.50'

@description('Azure Bastion SKU')
@allowed([
  'Basic'
  'Standard'
])
param bastionSku string = 'Basic'

@description('Domain name')
param domainName string = 'contoso.com'

@description('Domain NetBIOS name')
param domainNetBiosName string = 'CONTOSO'

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('URL to Cluster-Package.zip file in Azure Storage')
param clusterPackageZipUrl string = ''

@description('Enable Azure Disk Encryption (creates a Key Vault for encryption keys)')
param enableDiskEncryption bool = true

// Key Vault for Azure Disk Encryption (conditional)
module diskEncryptionVault '../shared/modules/disk-encryption-vault.bicep' = {
  name: '${environmentPrefix}-disk-encryption-vault'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    enableDiskEncryption: enableDiskEncryption
  }
}

// Phase 1: Deploy networking infrastructure
module network 'modules/network.bicep' = {
  name: '${environmentPrefix}-network-deployment'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    vnetAddressPrefix: vnetAddressPrefix
    bastionSubnetPrefix: bastionSubnetPrefix
    external1SubnetPrefix: external1SubnetPrefix
    external2SubnetPrefix: external2SubnetPrefix
    bastionSku: bastionSku
  }
}

// Phase 1: Deploy Domain Controller
module domainController 'modules/domain-controller.bicep' = {
  name: '${environmentPrefix}-dc-deployment'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    dcVmSize: dcVmSize
    dcOsVersion: dcOsVersion
    dcCustomImageId: dcCustomImageId
    domainName: domainName
    domainNetBiosName: domainNetBiosName
    external1SubnetId: network.outputs.external1SubnetId
    external2SubnetId: network.outputs.external2SubnetId
    dcExternal1Ip: dcExternal1Ip
    dcExternal2Ip: dcExternal2Ip
    enableAutoShutdown: enableAutoShutdown
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    clusterPackageZipUrl: clusterPackageZipUrl
  }
}

// Phase 1: Deploy Storage Server (NOT domain-joined, can run in parallel with DC)
module storageServer 'modules/storage-server.bicep' = {
  name: '${environmentPrefix}-storage-deployment'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    storageVmSize: storageVmSize
    storageOsVersion: storageOsVersion
    storageCustomImageId: storageCustomImageId
    external1SubnetId: network.outputs.external1SubnetId
    storageExternal1Ip: storageExternal1Ip
    enableAutoShutdown: enableAutoShutdown
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    clusterPackageZipUrl: clusterPackageZipUrl
  }
}

// Outputs for Phase 2
output resourceGroupName string = resourceGroup().name
output vnetName string = network.outputs.vnetName
output bastionFqdn string = network.outputs.bastionFqdn
output external1SubnetId string = network.outputs.external1SubnetId
output external2SubnetId string = network.outputs.external2SubnetId
output domainControllerName string = domainController.outputs.dcVmName
output storageServerName string = storageServer.outputs.storageVmName
output domainName string = domainName
output domainNetBiosName string = domainNetBiosName
output dcExternal1Ip string = dcExternal1Ip
output dcExternal2Ip string = dcExternal2Ip
output storageExternal1Ip string = storageExternal1Ip
output dcPrivateIps array = domainController.outputs.dcPrivateIps
output storagePrivateIps array = storageServer.outputs.storagePrivateIps
output keyVaultName string = diskEncryptionVault.outputs.keyVaultName
output keyVaultId string = diskEncryptionVault.outputs.keyVaultId
output keyVaultUrl string = diskEncryptionVault.outputs.keyVaultUrl
