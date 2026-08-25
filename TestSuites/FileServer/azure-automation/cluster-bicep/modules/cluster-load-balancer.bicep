@description('Resource group location')
param location string

@description('Environment name prefix')
param environmentPrefix string

param external1SubnetId string
param external2SubnetId string
param clusterExternal1Ip string
param clusterExternal2Ip string
param generalFSExternal1Ip string
param generalFSExternal2Ip string
param clusterExternal1ProbePort int
param clusterExternal2ProbePort int
param generalFSExternal1ProbePort int
param generalFSExternal2ProbePort int

var loadBalancerName = '${environmentPrefix}-cluster-ilb'
var backendPoolName = 'cluster-node-backend'
var frontends = [
  {
    name: 'cluster-external1-frontend'
    subnetId: external1SubnetId
    address: clusterExternal1Ip
    probeName: 'cluster-external1-probe'
    probePort: clusterExternal1ProbePort
    ruleName: 'cluster-external1-rule'
  }
  {
    name: 'cluster-external2-frontend'
    subnetId: external2SubnetId
    address: clusterExternal2Ip
    probeName: 'cluster-external2-probe'
    probePort: clusterExternal2ProbePort
    ruleName: 'cluster-external2-rule'
  }
  {
    name: 'generalfs-external1-frontend'
    subnetId: external1SubnetId
    address: generalFSExternal1Ip
    probeName: 'generalfs-external1-probe'
    probePort: generalFSExternal1ProbePort
    ruleName: 'generalfs-external1-rule'
  }
  {
    name: 'generalfs-external2-frontend'
    subnetId: external2SubnetId
    address: generalFSExternal2Ip
    probeName: 'generalfs-external2-probe'
    probePort: generalFSExternal2ProbePort
    ruleName: 'generalfs-external2-rule'
  }
]

resource loadBalancer 'Microsoft.Network/loadBalancers@2023-04-01' = {
  name: loadBalancerName
  location: location
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  properties: {
    frontendIPConfigurations: [
      for frontend in frontends: {
        name: frontend.name
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: frontend.address
          privateIPAddressVersion: 'IPv4'
          subnet: {
            id: frontend.subnetId
          }
        }
      }
    ]
    backendAddressPools: [
      {
        name: backendPoolName
      }
    ]
    probes: [
      for frontend in frontends: {
        name: frontend.probeName
        properties: {
          protocol: 'Tcp'
          port: frontend.probePort
          intervalInSeconds: 5
          numberOfProbes: 2
        }
      }
    ]
    loadBalancingRules: [
      for frontend in frontends: {
        name: frontend.ruleName
        properties: {
          protocol: 'All'
          frontendPort: 0
          backendPort: 0
          enableFloatingIP: true
          idleTimeoutInMinutes: 15
          loadDistribution: 'Default'
          disableOutboundSnat: true
          frontendIPConfiguration: {
            id: resourceId(
              'Microsoft.Network/loadBalancers/frontendIPConfigurations',
              loadBalancerName,
              frontend.name
            )
          }
          backendAddressPool: {
            id: resourceId(
              'Microsoft.Network/loadBalancers/backendAddressPools',
              loadBalancerName,
              backendPoolName
            )
          }
          probe: {
            id: resourceId(
              'Microsoft.Network/loadBalancers/probes',
              loadBalancerName,
              frontend.probeName
            )
          }
        }
      }
    ]
  }
}

output loadBalancerId string = loadBalancer.id
output backendPoolId string = resourceId(
  'Microsoft.Network/loadBalancers/backendAddressPools',
  loadBalancerName,
  backendPoolName
)
