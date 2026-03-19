// Workgroup File Server Test Suite - Main Deployment Template
// Deploys network infrastructure and workgroup computers (Driver + SUT) in a single deployment

@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix')
param environmentPrefix string = 'fstest'

@description('Admin username for VMs')
param adminUsername string = 'testadmin'

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Driver computer VM size')
param driverVmSize string = 'Standard_F4as_v6'

@description('SUT computer VM size')
param sutVmSize string = 'Standard_D8ls_v5'

@description('Driver computer OS type')
@allowed([
  'Windows'
  'Linux'
])
param driverOsType string = 'Windows'

@description('Driver computer Windows OS version (win10-* or win11-* SKU)')
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

@description('Driver computer Linux OS version (Ubuntu SKU)')
@allowed([
  'server'
  'server-arm64'
])
param driverLinuxOsVersion string = 'server'

@description('Custom image resource ID for driver VM (overrides marketplace image when set)')
param driverCustomImageId string = ''

@description('SUT computer OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param sutOsVersion string = '2025-datacenter-azure-edition'

@description('Custom image resource ID for SUT VM (overrides marketplace image when set). Must be a Windows image.')
param sutCustomImageId string = ''

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

@description('Driver computer External1 IP address')
param driverExternal1Ip string = '192.168.1.111'

@description('Driver computer External2 IP address')
param driverExternal2Ip string = '192.168.2.111'

@description('SUT computer External1 IP address')
param sutExternal1Ip string = '192.168.1.11'

@description('SUT computer External2 IP address')
param sutExternal2Ip string = '192.168.2.11'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Auto-shutdown time (HHmm in UTC)')
param autoShutdownTime string = '2000'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('URL to the DSC package zip file in Azure Storage (contains DSC/ folder, Config.json, Tools.json)')
param dscPackageZipUrl string = ''

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

// Deploy workgroup computers (Driver + SUT)
module computers 'modules/workgroup-computers.bicep' = {
  name: '${environmentPrefix}-computers-deployment'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    driverVmSize: driverVmSize
    sutVmSize: sutVmSize
    driverOsType: driverOsType
    driverOsVersion: driverOsVersion
    driverLinuxOsVersion: driverLinuxOsVersion
    driverCustomImageId: driverCustomImageId
    sutOsVersion: sutOsVersion
    sutCustomImageId: sutCustomImageId
    external1SubnetId: network.outputs.external1SubnetId
    external2SubnetId: network.outputs.external2SubnetId
    driverExternal1Ip: driverExternal1Ip
    driverExternal2Ip: driverExternal2Ip
    sutExternal1Ip: sutExternal1Ip
    sutExternal2Ip: sutExternal2Ip
    enableAutoShutdown: enableAutoShutdown
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    dscPackageZipUrl: dscPackageZipUrl
  }
}

// Outputs
output vnetId string = network.outputs.vnetId
output vnetName string = network.outputs.vnetName
output bastionFqdn string = network.outputs.bastionFqdn
output driverVmId string = computers.outputs.driverVmId
output sutVmId string = computers.outputs.sutVmId
output driverVmName string = computers.outputs.driverVmName
output sutVmName string = computers.outputs.sutVmName
output driverPrivateIps array = computers.outputs.driverPrivateIps
output sutPrivateIps array = computers.outputs.sutPrivateIps
output keyVaultName string = diskEncryptionVault.outputs.keyVaultName
output keyVaultId string = diskEncryptionVault.outputs.keyVaultId
output keyVaultUrl string = diskEncryptionVault.outputs.keyVaultUrl
