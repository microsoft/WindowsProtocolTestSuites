// Phase 2: Deploy domain-joined VMs
// This phase deploys: Cluster Nodes (Node01, Node02), Driver Computer
// Prerequisites: Phase 1 must be complete and DC must be fully configured

@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix')
param environmentPrefix string = 'fstest-cluster'

@description('Admin username for VMs')
param adminUsername string = 'testadmin'

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Cluster node VM size')
param clusterNodeVmSize string = 'Standard_D8s_v5'

@description('Driver computer VM size')
param driverVmSize string = 'Standard_D4s_v5'

@description('Cluster nodes OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param clusterNodeOsVersion string = '2025-datacenter-azure-edition'

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

@description('Custom image resource ID for cluster node VMs (overrides marketplace image when set). Must be a Windows image.')
param clusterNodeCustomImageId string = ''

@description('External1 subnet ID from Phase 1')
param external1SubnetId string = ''

@description('External2 subnet ID from Phase 1')
param external2SubnetId string = ''

@description('Node01 External1 IP address')
param node01External1Ip string = '192.168.1.11'

@description('Node01 External2 IP address')
param node01External2Ip string = '192.168.2.11'

@description('Node02 External1 IP address')
param node02External1Ip string = '192.168.1.12'

@description('Node02 External2 IP address')
param node02External2Ip string = '192.168.2.12'

@description('Driver computer External1 IP address')
param driverExternal1Ip string = '192.168.1.111'

@description('Driver computer External2 IP address')
param driverExternal2Ip string = '192.168.2.111'

@description('Domain Controller External1 IP address from Phase 1')
param dcExternal1Ip string = '192.168.1.10'

@description('Cluster core virtual IP on External1')
param clusterExternal1Ip string = '192.168.1.100'

@description('Cluster core virtual IP on External2')
param clusterExternal2Ip string = '192.168.2.100'

@description('GeneralFS virtual IP on External1')
param generalFSExternal1Ip string = '192.168.1.200'

@description('GeneralFS virtual IP on External2')
param generalFSExternal2Ip string = '192.168.2.200'

@description('Cluster core External1 health probe port')
param clusterExternal1ProbePort int = 59998

@description('Cluster core External2 health probe port')
param clusterExternal2ProbePort int = 59999

@description('GeneralFS External1 health probe port')
param generalFSExternal1ProbePort int = 60000

@description('GeneralFS External2 health probe port')
param generalFSExternal2ProbePort int = 60001

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Run FileServer tests automatically after configuration')
param enableTestAutoRun bool = true

@description('URL to Cluster-Package.zip file in Azure Storage')
param clusterPackageZipUrl string = ''

module clusterLoadBalancer 'modules/cluster-load-balancer.bicep' = {
  name: '${environmentPrefix}-cluster-ilb-deployment'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    external1SubnetId: external1SubnetId
    external2SubnetId: external2SubnetId
    clusterExternal1Ip: clusterExternal1Ip
    clusterExternal2Ip: clusterExternal2Ip
    generalFSExternal1Ip: generalFSExternal1Ip
    generalFSExternal2Ip: generalFSExternal2Ip
    clusterExternal1ProbePort: clusterExternal1ProbePort
    clusterExternal2ProbePort: clusterExternal2ProbePort
    generalFSExternal1ProbePort: generalFSExternal1ProbePort
    generalFSExternal2ProbePort: generalFSExternal2ProbePort
  }
}

// Deploy Cluster Nodes (after DC is verified ready)
module clusterNodes 'modules/cluster-nodes.bicep' = {
  name: '${environmentPrefix}-cluster-nodes-deployment'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    clusterNodeVmSize: clusterNodeVmSize
    clusterNodeOsVersion: clusterNodeOsVersion
    clusterNodeCustomImageId: clusterNodeCustomImageId
    external1SubnetId: external1SubnetId
    external2SubnetId: external2SubnetId
    node01External1Ip: node01External1Ip
    node01External2Ip: node01External2Ip
    node02External1Ip: node02External1Ip
    node02External2Ip: node02External2Ip
    dcExternal1Ip: dcExternal1Ip
    backendPoolId: clusterLoadBalancer.outputs.backendPoolId
    enableAutoShutdown: enableAutoShutdown
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    clusterPackageZipUrl: clusterPackageZipUrl
  }
}

// Deploy Driver Computer (after DC is verified ready)
module driverComputer 'modules/driver-computer.bicep' = {
  name: '${environmentPrefix}-driver-deployment'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    driverVmSize: driverVmSize
    driverOsType: driverOsType
    driverOsVersion: driverOsVersion
    driverLinuxOsVersion: driverLinuxOsVersion
    driverCustomImageId: driverCustomImageId
    external1SubnetId: external1SubnetId
    external2SubnetId: external2SubnetId
    driverExternal1Ip: driverExternal1Ip
    driverExternal2Ip: driverExternal2Ip
    dcExternal1Ip: dcExternal1Ip
    enableAutoShutdown: enableAutoShutdown
    enableTestAutoRun: enableTestAutoRun
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    clusterPackageZipUrl: clusterPackageZipUrl
  }
}

// Outputs
output node01Name string = clusterNodes.outputs.node01VmName
output node02Name string = clusterNodes.outputs.node02VmName
output driverComputerName string = driverComputer.outputs.driverVmName
output node01PrivateIps array = clusterNodes.outputs.node01PrivateIps
output node02PrivateIps array = clusterNodes.outputs.node02PrivateIps
output driverPrivateIps array = driverComputer.outputs.driverPrivateIps
output clusterLoadBalancerId string = clusterLoadBalancer.outputs.loadBalancerId
