@description('Resource group location')
param location string

param dcVmName string
param storageVmName string

@secure()
param adminPassword string

@secure()
param clusterPackageZipUrl string

@secure()
param configJsonBase64 string

param forceUpdateTag string

var dcCommand = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-cluster-dc\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Cluster-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File (Join-Path $stage \'cse-bootstrap.ps1\') -Scenario \'cluster\' -Role \'dc\' -PackageName \'Cluster-Package\' -DeployScript \'Deploy-DC.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -ConfigJsonBase64 \'${configJsonBase64}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'
var storageCommand = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-cluster-storage\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Cluster-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File (Join-Path $stage \'cse-bootstrap.ps1\') -Scenario \'cluster\' -Role \'storage\' -PackageName \'Cluster-Package\' -DeployScript \'Deploy-Storage.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -ConfigJsonBase64 \'${configJsonBase64}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'

resource dcVm 'Microsoft.Compute/virtualMachines@2023-03-01' existing = {
  name: dcVmName
}

resource storageVm 'Microsoft.Compute/virtualMachines@2023-03-01' existing = {
  name: storageVmName
}

resource dcExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = {
  name: 'ConfigureDomainController'
  parent: dcVm
  location: location
  properties: {
    publisher: 'Microsoft.Compute'
    type: 'CustomScriptExtension'
    typeHandlerVersion: '1.10'
    autoUpgradeMinorVersion: true
    forceUpdateTag: forceUpdateTag
    settings: {}
    protectedSettings: {
      fileUris: [
        clusterPackageZipUrl
      ]
      commandToExecute: dcCommand
    }
  }
}

resource storageExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = {
  name: 'ConfigureStorageServer'
  parent: storageVm
  location: location
  properties: {
    publisher: 'Microsoft.Compute'
    type: 'CustomScriptExtension'
    typeHandlerVersion: '1.10'
    autoUpgradeMinorVersion: true
    forceUpdateTag: forceUpdateTag
    settings: {}
    protectedSettings: {
      fileUris: [
        clusterPackageZipUrl
      ]
      commandToExecute: storageCommand
    }
  }
}

output dcExtensionId string = dcExtension.id
output storageExtensionId string = storageExtension.id
