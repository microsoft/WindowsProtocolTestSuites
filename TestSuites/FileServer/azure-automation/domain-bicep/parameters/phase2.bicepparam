using '../phase2.bicep'

// Phase 2: Driver Computer (Client01) + SUT Computer (Node01)
// This parameter file is used by deploy.ps1 for Phase 2 deployment
// Prerequisites from Phase 1: external1SubnetId, external2SubnetId, dcExternal1Ip

// Environment Configuration
param location = 'West US 2'
param environmentPrefix = 'fstest-domain'
param adminUsername = 'testadmin'
// Note: adminPassword must be provided at deployment time via command line

// VM Sizes
param driverVmSize = 'Standard_F4as_v6'
param sutVmSize = 'Standard_D8ls_v5'

// OS Versions
param driverOsType = 'Windows'
param driverOsVersion = 'win11-25h2-ent'
param driverLinuxOsVersion = 'server'
param sutOsVersion = '2025-datacenter-azure-edition'

// Custom Images (empty = use marketplace images above)
param driverCustomImageId = ''
param sutCustomImageId = ''

// IP Addresses - External1 Network (192.168.1.0/24)
param driverExternal1Ip = '192.168.1.111'
param sutExternal1Ip = '192.168.1.11'

// IP Addresses - External2 Network (192.168.2.0/24)
param driverExternal2Ip = '192.168.2.111'
param sutExternal2Ip = '192.168.2.11'

// Auto-shutdown Configuration
// Default false for the domain lab: auto-shutdown DEALLOCATES VMs, which can collide with
// domain-member machine-account password handling. Set to true to opt in and save cost.
param enableAutoShutdown = false
param enableTestAutoRun = true
param autoShutdownTime = '20:00'
param autoShutdownTimeZone = 'Eastern Standard Time'

// MUST be overridden at deployment time via command line
param adminPassword = ''

// NOTE: The following parameters are passed from Phase 1 outputs via command line
// These placeholders are required for Bicep validation but will be overridden at deployment time
param external1SubnetId = '/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/placeholder/providers/Microsoft.Network/virtualNetworks/placeholder/subnets/external1'
param external2SubnetId = '/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/placeholder/providers/Microsoft.Network/virtualNetworks/placeholder/subnets/external2'
param dcExternal1Ip = '192.168.1.10'
param dcExternal2Ip = '192.168.2.10'
