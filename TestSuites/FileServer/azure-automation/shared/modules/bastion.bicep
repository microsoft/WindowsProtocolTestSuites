@description('Resource group location')
param location string

@description('Environment name prefix (e.g., fstest)')
param environmentPrefix string

@description('Azure Bastion subnet resource ID')
param bastionSubnetId string

@description('Azure Bastion SKU')
@allowed([
  'Basic'
  'Standard'
])
param bastionSku string

var bastionName = '${environmentPrefix}-bastion'
var bastionPublicIpName = '${bastionName}-pip'

resource bastionPublicIp 'Microsoft.Network/publicIPAddresses@2023-04-01' = {
  name: bastionPublicIpName
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
    dnsSettings: {
      domainNameLabel: '${bastionName}-${uniqueString(resourceGroup().id)}'
    }
  }
}

resource bastion 'Microsoft.Network/bastionHosts@2023-04-01' = {
  name: bastionName
  location: location
  sku: {
    name: bastionSku
  }
  properties: {
    ipConfigurations: [
      {
        name: 'IpConf'
        properties: {
          subnet: {
            id: bastionSubnetId
          }
          publicIPAddress: {
            id: bastionPublicIp.id
          }
        }
      }
    ]
  }
}

output bastionFqdn string = bastionPublicIp.properties.dnsSettings.fqdn
