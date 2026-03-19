using '../phase2.bicep'

// Phase 2: Cluster Nodes and Driver Computer
// This parameter file is used by deploy.ps1 for Phase 2 deployment
// Prerequisites from Phase 1: external1SubnetId, external2SubnetId, dcExternal1Ip, domainName, domainNetBiosName

// Environment Configuration
param location = 'West US 2'
param environmentPrefix = 'fstest-cluster'
param adminUsername = 'testadmin'
// Note: adminPassword must be provided at deployment time via command line

// VM Sizes
param clusterNodeVmSize = 'Standard_D8s_v5'
param driverVmSize = 'Standard_D4s_v5'

// OS Versions
param clusterNodeOsVersion = '2025-datacenter-azure-edition'
param driverOsVersion = 'win11-25h2-ent'

// Driver OS type and custom images (empty = use marketplace images above)
param driverOsType = 'Windows'
param driverLinuxOsVersion = 'server'
param driverCustomImageId = ''
param clusterNodeCustomImageId = ''

// IP Addresses - External1 Network (192.168.1.0/24)
param node01External1Ip = '192.168.1.11'
param node02External1Ip = '192.168.1.12'
param driverExternal1Ip = '192.168.1.111'

// IP Addresses - External2 Network (192.168.2.0/24)
param node01External2Ip = '192.168.2.11'
param node02External2Ip = '192.168.2.12'
param driverExternal2Ip = '192.168.2.111'

// Auto-shutdown Configuration
param enableAutoShutdown = true
param autoShutdownTime = '20:00'
param autoShutdownTimeZone = 'UTC'

// Cluster Package URL (optional - will be uploaded automatically if local files exist)
param clusterPackageZipUrl = ''

// MUST be overridden at deployment time via command line
param adminPassword = ''

// NOTE: The following parameters are passed from Phase 1 outputs via command line
// These placeholders are required for Bicep validation but will be overridden at deployment time
param external1SubnetId = '/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/placeholder/providers/Microsoft.Network/virtualNetworks/placeholder/subnets/external1'
param external2SubnetId = '/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/placeholder/providers/Microsoft.Network/virtualNetworks/placeholder/subnets/external2'
param dcExternal1Ip = '192.168.1.10'
