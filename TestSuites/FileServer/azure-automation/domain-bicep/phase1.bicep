// Phase 1: Network Infrastructure + Domain Controller
// Deploys VNet, Bastion, NSGs, and DC01 with Active Directory Domain Services

@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix')
param environmentPrefix string = 'fstest'

@description('Admin username for VMs')
param adminUsername string = 'testadmin'

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Domain Controller VM size')
param dcVmSize string = 'Standard_D4s_v5'

@description('Domain Controller OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param dcOsVersion string = '2025-datacenter-azure-edition'

@description('Virtual network address space')
param vnetAddressPrefix string = '192.168.0.0/16'

@description('Azure Bastion subnet address prefix (must be /26 or larger)')
param bastionSubnetPrefix string = '192.168.0.0/26'

@description('External1 subnet address prefix')
param external1SubnetPrefix string = '192.168.1.0/24'

@description('External2 subnet address prefix')
param external2SubnetPrefix string = '192.168.2.0/24'

@description('Azure Bastion SKU')
@allowed([
  'Basic'
  'Standard'
])
param bastionSku string = 'Basic'

@description('Domain Controller External1 IP address')
param dcExternal1Ip string = '192.168.1.10'

@description('Domain Controller External2 IP address')
param dcExternal2Ip string = '192.168.2.10'

@description('Domain name')
param domainName string = 'contoso.com'

@description('Domain NetBIOS name')
param domainNetBiosName string = 'CONTOSO'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('Custom image resource ID for DC VM (overrides marketplace image when set). Must be a Windows image.')
param dcCustomImageId string = ''

@description('URL to Domain-Package.zip file in Azure Storage')
param domainPackageZipUrl string = ''

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

// Deploy networking infrastructure
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

// Deploy Domain Controller (depends on network for subnet IDs)
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
    domainPackageZipUrl: domainPackageZipUrl
  }
}

// Outputs consumed by Phase 2
output vnetName string = network.outputs.vnetName
output bastionFqdn string = network.outputs.bastionFqdn
output external1SubnetId string = network.outputs.external1SubnetId
output external2SubnetId string = network.outputs.external2SubnetId
output dcVmName string = domainController.outputs.dcVmName
output dcExternal1Ip string = domainController.outputs.dcExternal1Ip
output keyVaultName string = diskEncryptionVault.outputs.keyVaultName
output keyVaultId string = diskEncryptionVault.outputs.keyVaultId
output keyVaultUrl string = diskEncryptionVault.outputs.keyVaultUrl
output domainName string = domainName
output domainNetBiosName string = domainNetBiosName
