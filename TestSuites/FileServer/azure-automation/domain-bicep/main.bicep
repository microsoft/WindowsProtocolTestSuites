// Domain File Server Test Suite - Combined Deployment Template
// Single-template equivalent of phase1 (network + DC) + phase2 (Driver + SUT),
// intended for the one-click "Deploy to Azure" button.
//
// Phasing is handled DECLARATIVELY: the domain-joined computers module depends on
// the domain controller, and the members' NICs use the DC as their DNS server.
// The DC finishes AD DS promotion asynchronously (reboots), but the on-VM join
// (shared/DSC/Scripts/domainjoin.ps1) already retries DNS/DC reachability and
// Add-Computer with exponential backoff, so members wait out promotion without any
// PowerShell orchestration. deploy.ps1's two-phase flow remains for CLI use.

@description('Resource group location. Defaults to the resource group\'s region so the "Deploy to Azure" button deploys where you have quota/capacity. deploy.ps1 sets this explicitly from the bicepparam file.')
@minLength(1)
param location string = resourceGroup().location

@description('Environment name prefix')
@minLength(1)
param environmentPrefix string = 'fstest'

@description('Admin username for VMs')
@minLength(1)
param adminUsername string = 'testadmin'

@description('Admin password for all VMs (also used for the domain admin / test accounts). 12+ chars; must meet Azure Windows complexity: 3 of upper/lower/digit/symbol.')
@secure()
@minLength(12)
param adminPassword string

@description('Domain name')
@minLength(1)
param domainName string = 'contoso.com'

@description('Domain NetBIOS name')
@minLength(1)
param domainNetBiosName string = 'CONTOSO'

@description('Domain Controller VM size. Defaults to a broadly-available burstable (B-series) size for the one-click button; deploy.ps1 uses a compute-optimized size from the bicepparam file.')
@minLength(1)
param dcVmSize string = 'Standard_B4ms'

@description('Domain Controller OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param dcOsVersion string = '2025-datacenter-azure-edition'

@description('Custom image resource ID for DC VM (overrides marketplace image when set). Must be a Windows image.')
param dcCustomImageId string = ''

@description('Driver computer VM size. Defaults to a broadly-available burstable (B-series) size for the one-click button.')
@minLength(1)
param driverVmSize string = 'Standard_B4ms'

@description('SUT computer VM size. Defaults to a broadly-available burstable (B-series) size for the one-click button.')
@minLength(1)
param sutVmSize string = 'Standard_B4ms'

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

@description('SUT computer OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param sutOsVersion string = '2025-datacenter-azure-edition'

@description('Custom image resource ID for SUT VM (overrides marketplace image when set). Must be a Windows image.')
param sutCustomImageId string = ''

@description('Virtual network address space')
@minLength(1)
param vnetAddressPrefix string = '192.168.0.0/16'

@description('Azure Bastion subnet address prefix (must be /26 or larger)')
@minLength(1)
param bastionSubnetPrefix string = '192.168.0.0/26'

@description('External1 subnet address prefix')
@minLength(1)
param external1SubnetPrefix string = '192.168.1.0/24'

@description('External2 subnet address prefix')
@minLength(1)
param external2SubnetPrefix string = '192.168.2.0/24'

@description('Azure Bastion SKU')
@allowed([
  'Basic'
  'Standard'
])
param bastionSku string = 'Basic'

@description('Domain Controller External1 IP address')
@minLength(1)
param dcExternal1Ip string = '192.168.1.10'

@description('Domain Controller External2 IP address')
@minLength(1)
param dcExternal2Ip string = '192.168.2.10'

@description('Driver computer External1 IP address')
@minLength(1)
param driverExternal1Ip string = '192.168.1.111'

@description('Driver computer External2 IP address')
@minLength(1)
param driverExternal2Ip string = '192.168.2.111'

@description('SUT computer External1 IP address')
@minLength(1)
param sutExternal1Ip string = '192.168.1.11'

@description('SUT computer External2 IP address')
@minLength(1)
param sutExternal2Ip string = '192.168.2.11'

@description('Enable auto-shutdown. Default false for the domain lab: auto-shutdown DEALLOCATES VMs, and a deallocate/restart of a domain-joined member can collide with machine-account password handling. Members set Netlogon\\DisablePasswordChange to stay safe, but keeping the lab running by default avoids the deallocation churn entirely. Opt in explicitly to save cost.')
param enableAutoShutdown bool = false

@description('Auto-shutdown time (HH:mm in UTC)')
@minLength(1)
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
@minLength(1)
param autoShutdownTimeZone string = 'UTC'

@description('URL to the Domain DSC package zip (contains DSC/ folder, Config.json, Tools.json). Defaults to the public GitHub Release asset that the "Deploy to Azure" button consumes; the on-VM Custom Script Extension injects the real admin password into the package\'s placeholder Config.json at deploy time. deploy.ps1 overrides this with a freshly built, credential-baked package.')
param domainPackageZipUrl string = 'https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/fileserver-domain-deploy-button-v1/Domain-Package.zip'

@description('Enable Azure Disk Encryption (creates a Key Vault for encryption keys). Defaults OFF for the one-click button: ADE is applied by deploy.ps1 as a post-deploy step (which the Portal button cannot run), so for the button the vault would be created but never used. Managed disks are platform-encrypted at rest regardless. deploy.ps1 sets this true via the bicepparam file.')
param enableDiskEncryption bool = false

// Phase 1 (vault + network + Bastion + DC) and Phase 2 (Driver + SUT) are the
// same templates deploy.ps1 deploys, composed here as modules so the one-click
// button and the two-phase CLI path share a single wiring and single set of
// module defaults. Phase 2 consumes Phase 1's outputs, which also gives the
// members their DC dependency: they deploy only after the whole of Phase 1
// (including the DC's Custom Script Extension) has returned, then their on-VM
// domain join retries until AD DS promotion is done.
module phase1 'phase1.bicep' = {
  name: '${environmentPrefix}-phase1'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    dcVmSize: dcVmSize
    dcOsVersion: dcOsVersion
    dcCustomImageId: dcCustomImageId
    domainName: domainName
    domainNetBiosName: domainNetBiosName
    vnetAddressPrefix: vnetAddressPrefix
    bastionSubnetPrefix: bastionSubnetPrefix
    external1SubnetPrefix: external1SubnetPrefix
    external2SubnetPrefix: external2SubnetPrefix
    bastionSku: bastionSku
    dcExternal1Ip: dcExternal1Ip
    dcExternal2Ip: dcExternal2Ip
    enableAutoShutdown: enableAutoShutdown
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    domainPackageZipUrl: domainPackageZipUrl
    enableDiskEncryption: enableDiskEncryption
  }
}

module phase2 'phase2.bicep' = {
  name: '${environmentPrefix}-phase2'
  params: {
    location: location
    environmentPrefix: environmentPrefix
    adminUsername: adminUsername
    adminPassword: adminPassword
    driverVmSize: driverVmSize
    sutVmSize: sutVmSize
    driverOsType: driverOsType
    driverOsVersion: driverOsVersion
    driverLinuxOsVersion: driverLinuxOsVersion
    driverCustomImageId: driverCustomImageId
    sutOsVersion: sutOsVersion
    sutCustomImageId: sutCustomImageId
    external1SubnetId: phase1.outputs.external1SubnetId
    external2SubnetId: phase1.outputs.external2SubnetId
    driverExternal1Ip: driverExternal1Ip
    driverExternal2Ip: driverExternal2Ip
    sutExternal1Ip: sutExternal1Ip
    sutExternal2Ip: sutExternal2Ip
    dcExternal1Ip: phase1.outputs.dcExternal1Ip
    dcExternal2Ip: phase1.outputs.dcExternal2Ip
    enableAutoShutdown: enableAutoShutdown
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    domainPackageZipUrl: domainPackageZipUrl
  }
}

// Outputs
output vnetId string = phase1.outputs.vnetId
output vnetName string = phase1.outputs.vnetName
output bastionFqdn string = phase1.outputs.bastionFqdn
output dcVmName string = phase1.outputs.dcVmName
output driverVmName string = phase2.outputs.driverVmName
output sutVmName string = phase2.outputs.sutVmName
output driverPrivateIps array = phase2.outputs.driverPrivateIps
output sutPrivateIps array = phase2.outputs.sutPrivateIps
output keyVaultName string = phase1.outputs.keyVaultName
output keyVaultId string = phase1.outputs.keyVaultId
output keyVaultUrl string = phase1.outputs.keyVaultUrl
output domainName string = domainName
output domainNetBiosName string = domainNetBiosName
