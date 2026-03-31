// Workgroup File Server Test Suite - Parameters
// Customize these values for your workgroup deployment

using '../main.bicep'

// Environment
param location = 'West US 2'
param environmentPrefix = 'fstest'

// Admin credentials (password overridden at deployment time)
param adminUsername = 'testadmin'
param adminPassword = ''

// VM sizes
param driverVmSize = 'Standard_F4as_v6'
param sutVmSize = 'Standard_D8ls_v5'

// OS versions
param driverOsType = 'Windows'
param driverOsVersion = 'win11-25h2-ent'
param driverLinuxOsVersion = 'server'
param sutOsVersion = '2025-datacenter-azure-edition'

// Custom Images (empty = use marketplace images above)
param driverCustomImageId = ''
param sutCustomImageId = ''

// Network configuration
param vnetAddressPrefix = '192.168.0.0/16'
param bastionSubnetPrefix = '192.168.0.0/26'
param external1SubnetPrefix = '192.168.1.0/24'
param external2SubnetPrefix = '192.168.2.0/24'
param bastionSku = 'Basic'

// Driver Computer (Client01) IP addresses
param driverExternal1Ip = '192.168.1.111'
param driverExternal2Ip = '192.168.2.111'

// SUT (Node01) IP addresses
param sutExternal1Ip = '192.168.1.11'
param sutExternal2Ip = '192.168.2.11'

// Auto-shutdown configuration
param enableAutoShutdown = true
param autoShutdownTime = '2000'
param autoShutdownTimeZone = 'UTC'

// Disk Encryption
param enableDiskEncryption = true

// DSC Package URL (optional - the local DSC/ folder is packaged and uploaded automatically)
param dscPackageZipUrl = ''
