@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix (e.g., fstest)')
param environmentPrefix string

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

// Variables
var networkSecurityGroupName = '${environmentPrefix}-cluster-nsg'
var bastionNetworkSecurityGroupName = '${environmentPrefix}-bastion-nsg'
var vnetName = '${environmentPrefix}-cluster-vnet'
var bastionName = '${environmentPrefix}-bastion'
var bastionPublicIpName = '${bastionName}-pip'

// Network Security Group for VMs
resource networkSecurityGroup 'Microsoft.Network/networkSecurityGroups@2023-04-01' = {
  name: networkSecurityGroupName
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowSMB'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '445'
          sourceAddressPrefix: vnetAddressPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 100
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowWinRM'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRanges: [
            '5985'
            '5986'
          ]
          sourceAddressPrefix: vnetAddressPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 102
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowRPCDynamic'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '49152-65535'
          sourceAddressPrefix: vnetAddressPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 103
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowDNS'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '53'
          sourceAddressPrefix: vnetAddressPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 104
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowKerberos'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '88'
          sourceAddressPrefix: vnetAddressPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 105
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowLDAP'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRanges: [
            '389'
            '636'
          ]
          sourceAddressPrefix: vnetAddressPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 106
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowBastion'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRanges: [
            '22'
            '3389'
          ]
          sourceAddressPrefix: bastionSubnetPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 107
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowiSCSI'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '3260'
          sourceAddressPrefix: vnetAddressPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 108
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowClusterCommunication'
        properties: {
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRanges: [
            '3343'
            '135'
          ]
          sourceAddressPrefix: vnetAddressPrefix
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 109
          direction: 'Inbound'
        }
      }
    ]
  }
}

// Network Security Group for Bastion
resource bastionNetworkSecurityGroup 'Microsoft.Network/networkSecurityGroups@2023-04-01' = {
  name: bastionNetworkSecurityGroupName
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowHttpsInbound'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: 'Internet'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 1000
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowGatewayManagerInbound'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: 'GatewayManager'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 1001
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowAzureLoadBalancerInbound'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: 'AzureLoadBalancer'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 1002
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowBastionHostCommunication'
        properties: {
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRanges: [
            '8080'
            '5701'
          ]
          sourceAddressPrefix: 'VirtualNetwork'
          destinationAddressPrefix: 'VirtualNetwork'
          access: 'Allow'
          priority: 1003
          direction: 'Inbound'
        }
      }
      {
        name: 'AllowSshRdpOutbound'
        properties: {
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRanges: [
            '22'
            '3389'
          ]
          sourceAddressPrefix: '*'
          destinationAddressPrefix: 'VirtualNetwork'
          access: 'Allow'
          priority: 1000
          direction: 'Outbound'
        }
      }
      {
        name: 'AllowAzureCloudOutbound'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '443'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: 'AzureCloud'
          access: 'Allow'
          priority: 1001
          direction: 'Outbound'
        }
      }
      {
        name: 'AllowBastionCommunication'
        properties: {
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRanges: [
            '8080'
            '5701'
          ]
          sourceAddressPrefix: 'VirtualNetwork'
          destinationAddressPrefix: 'VirtualNetwork'
          access: 'Allow'
          priority: 1002
          direction: 'Outbound'
        }
      }
      {
        name: 'AllowGetSessionInformation'
        properties: {
          protocol: '*'
          sourcePortRange: '*'
          destinationPortRange: '80'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: 'Internet'
          access: 'Allow'
          priority: 1003
          direction: 'Outbound'
        }
      }
    ]
  }
}

// Public IP for Bastion
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

// Virtual Network
resource vnet 'Microsoft.Network/virtualNetworks@2023-04-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetAddressPrefix
      ]
    }
    subnets: [
      {
        name: 'AzureBastionSubnet'
        properties: {
          addressPrefix: bastionSubnetPrefix
          networkSecurityGroup: {
            id: bastionNetworkSecurityGroup.id
          }
        }
      }
      {
        name: 'external1-subnet'
        properties: {
          addressPrefix: external1SubnetPrefix
          networkSecurityGroup: {
            id: networkSecurityGroup.id
          }
        }
      }
      {
        name: 'external2-subnet'
        properties: {
          addressPrefix: external2SubnetPrefix
          networkSecurityGroup: {
            id: networkSecurityGroup.id
          }
        }
      }
    ]
  }
}

// Azure Bastion
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
            id: resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, 'AzureBastionSubnet')
          }
          publicIPAddress: {
            id: bastionPublicIp.id
          }
        }
      }
    ]
  }
  dependsOn: [
    vnet
  ]
}

// Outputs
output vnetId string = vnet.id
output vnetName string = vnetName
output external1SubnetId string = resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, 'external1-subnet')
output external2SubnetId string = resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, 'external2-subnet')
output networkSecurityGroupId string = networkSecurityGroup.id
output bastionFqdn string = bastionPublicIp.properties.dnsSettings.fqdn
