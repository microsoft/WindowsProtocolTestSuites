using '../phase1.bicep'

// Phase 1: Network + Domain Controller
// This parameter file is used by deploy.ps1 for Phase 1 deployment

// Environment Configuration
param location = 'West US 2'
param environmentPrefix = 'fstest-domain'
param adminUsername = 'testadmin'
// Note: adminPassword must be provided at deployment time via command line

// VM Sizes
param dcVmSize = 'Standard_D2s_v5'

// OS Versions
param dcOsVersion = '2025-datacenter-azure-edition'

// Network Configuration
param vnetAddressPrefix = '192.168.0.0/16'
param bastionSubnetPrefix = '192.168.0.0/26'
param external1SubnetPrefix = '192.168.1.0/24'
param external2SubnetPrefix = '192.168.2.0/24'
param bastionSku = 'Basic'

// IP Addresses - External1 Network (192.168.1.0/24)
param dcExternal1Ip = '192.168.1.10'

// IP Addresses - External2 Network (192.168.2.0/24)
param dcExternal2Ip = '192.168.2.10'

// Domain Configuration
param domainName = 'contoso.com'
param domainNetBiosName = 'CONTOSO'

// Auto-shutdown Configuration
param enableAutoShutdown = true
param autoShutdownTime = '20:00'
param autoShutdownTimeZone = 'Eastern Standard Time'

// Disk Encryption
param enableDiskEncryption = true

// Custom Images (empty = use marketplace images above)
param dcCustomImageId = ''

// Domain Package URL (optional - will be uploaded automatically if local files exist)
param domainPackageZipUrl = ''

param adminPassword = ''
