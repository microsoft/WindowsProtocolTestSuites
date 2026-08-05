@description('Resource group location')
param location string = resourceGroup().location

@description('Environment name prefix')
param environmentPrefix string = 'fstest'

@description('Admin password injected into the package Config.json')
@secure()
param adminPassword string

@description('Driver computer OS type')
@allowed([
  'Windows'
  'Linux'
])
param driverOsType string = 'Windows'

@description('URL to Domain-Package.zip file in Azure Storage')
param domainPackageZipUrl string

module memberConfiguration 'modules/domain-computer-extensions.bicep' = {
  name: '${environmentPrefix}-computers-configuration'
  params: {
    location: location
    driverVmName: '${environmentPrefix}-client01'
    sutVmName: '${environmentPrefix}-node01'
    adminPassword: adminPassword
    driverOsType: driverOsType
    domainPackageZipUrl: domainPackageZipUrl
  }
}
