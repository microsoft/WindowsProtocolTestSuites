// Shared module: Key Vault for Azure Disk Encryption
// Deployed conditionally when enableDiskEncryption is true

@description('Resource group location')
param location string

@description('Environment name prefix (used for unique Key Vault naming)')
param environmentPrefix string

@description('Enable Azure Disk Encryption (creates a Key Vault for encryption keys)')
param enableDiskEncryption bool = true

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = if (enableDiskEncryption) {
  name: 'kv${uniqueString(resourceGroup().id, environmentPrefix)}'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enabledForDiskEncryption: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 7
    accessPolicies: []
  }
}

output keyVaultName string = keyVault.?name ?? ''
output keyVaultId string = keyVault.?id ?? ''
output keyVaultUrl string = keyVault.?properties.?vaultUri ?? ''
