using '../phase1.bicep'

// Phase 1: Network, Domain Controller, Storage Server
// This parameter file is used by deploy.ps1 for Phase 1 deployment

// Environment Configuration
param location = 'West US 2'
param environmentPrefix = 'fstest-cluster'
param adminUsername = 'testadmin'
// Note: adminPassword must be provided at deployment time via command line

// VM Sizes
param dcVmSize = 'Standard_D4s_v5'
param storageVmSize = 'Standard_D4s_v5'

// OS Versions
param dcOsVersion = '2025-datacenter-azure-edition'
param storageOsVersion = '2022-datacenter-g2'

// Custom Images (empty = use marketplace images above)
param dcCustomImageId = ''
param storageCustomImageId = ''

// Network Configuration
param vnetAddressPrefix = '192.168.0.0/16'
param bastionSubnetPrefix = '192.168.0.0/26'
param external1SubnetPrefix = '192.168.1.0/24'
param external2SubnetPrefix = '192.168.2.0/24'
param bastionSku = 'Basic'

// IP Addresses - External1 Network (192.168.1.0/24)
param dcExternal1Ip = '192.168.1.10'
param storageExternal1Ip = '192.168.1.50'

// IP Addresses - External2 Network (192.168.2.0/24)
param dcExternal2Ip = '192.168.2.10'

// Domain Configuration
param domainName = 'contoso.com'
param domainNetBiosName = 'CONTOSO'

// Auto-shutdown Configuration
param enableAutoShutdown = true
param autoShutdownTime = '20:00'
param autoShutdownTimeZone = 'UTC'

// Disk Encryption
param enableDiskEncryption = true

// Cluster Package URL (optional - will be uploaded automatically if local files exist)
param clusterPackageZipUrl = ''

param adminPassword = ''
