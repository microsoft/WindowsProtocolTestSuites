// Phase 2: Driver Computer (Client01) + SUT Computer (Node01)
// Deployed after Domain Controller is fully configured and ready for domain joins

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
param driverVmSize string = 'Standard_D4s_v5'

@description('SUT computer VM size')
param sutVmSize string = 'Standard_D4s_v5'

@description('Driver computer OS type')
@allowed([
  'Windows'
  'Linux'
])
param driverOsType string = 'Windows'

@description('Driver computer Windows OS version')
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

@description('External1 subnet ID (from Phase 1 output)')
param external1SubnetId string

@description('External2 subnet ID (from Phase 1 output)')
param external2SubnetId string

@description('Driver computer External1 IP address')
param driverExternal1Ip string = '192.168.1.111'

@description('Driver computer External2 IP address')
param driverExternal2Ip string = '192.168.2.111'

@description('SUT computer External1 IP address')
param sutExternal1Ip string = '192.168.1.11'

@description('SUT computer External2 IP address')
param sutExternal2Ip string = '192.168.2.11'

@description('Domain Controller External1 IP address (from Phase 1 output)')
param dcExternal1Ip string

@description('Domain Controller External2 IP address (from Phase 1 output)')
param dcExternal2Ip string

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('URL to Domain-Package.zip file in Azure Storage')
param domainPackageZipUrl string = ''

// Deploy domain-joined computers
module domainComputers 'modules/domain-computers.bicep' = {
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
    external1SubnetId: external1SubnetId
    external2SubnetId: external2SubnetId
    driverExternal1Ip: driverExternal1Ip
    driverExternal2Ip: driverExternal2Ip
    sutExternal1Ip: sutExternal1Ip
    sutExternal2Ip: sutExternal2Ip
    dcExternal1Ip: dcExternal1Ip
    dcExternal2Ip: dcExternal2Ip
    enableAutoShutdown: enableAutoShutdown
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    domainPackageZipUrl: domainPackageZipUrl
  }
}

// Outputs
output driverVmName string = domainComputers.outputs.driverVmName
output sutVmName string = domainComputers.outputs.sutVmName
output driverPrivateIps array = domainComputers.outputs.driverPrivateIps
output sutPrivateIps array = domainComputers.outputs.sutPrivateIps
