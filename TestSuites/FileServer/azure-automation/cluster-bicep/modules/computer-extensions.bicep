@description('Resource group location')
param location string

param node01VmName string
param node02VmName string
param driverVmName string

@secure()
param adminPassword string

@secure()
param clusterPackageZipUrl string

@secure()
param configJsonBase64 string

param forceUpdateTag string

var node01Command = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-cluster-node01\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Cluster-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File (Join-Path $stage \'cse-bootstrap.ps1\') -Scenario \'cluster\' -Role \'node01\' -PackageName \'Cluster-Package\' -DeployScript \'Deploy-Node01.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -ConfigJsonBase64 \'${configJsonBase64}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'
var node02Command = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-cluster-node02\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Cluster-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File (Join-Path $stage \'cse-bootstrap.ps1\') -Scenario \'cluster\' -Role \'node02\' -PackageName \'Cluster-Package\' -DeployScript \'Deploy-Node02.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -ConfigJsonBase64 \'${configJsonBase64}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'
var driverCommand = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-cluster-driver\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Cluster-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File (Join-Path $stage \'cse-bootstrap.ps1\') -Scenario \'cluster\' -Role \'driver\' -PackageName \'Cluster-Package\' -DeployScript \'Deploy-Driver.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -ConfigJsonBase64 \'${configJsonBase64}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'

resource node01Vm 'Microsoft.Compute/virtualMachines@2023-03-01' existing = {
  name: node01VmName
}

resource node02Vm 'Microsoft.Compute/virtualMachines@2023-03-01' existing = {
  name: node02VmName
}

resource driverVm 'Microsoft.Compute/virtualMachines@2023-03-01' existing = {
  name: driverVmName
}

resource node01Extension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = {
  name: 'ConfigureClusterNode01'
  parent: node01Vm
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
      commandToExecute: node01Command
    }
  }
}

resource node02Extension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = {
  name: 'ConfigureClusterNode02'
  parent: node02Vm
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
      commandToExecute: node02Command
    }
  }
}

resource driverExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = {
  name: 'ConfigureClusterDriver'
  parent: driverVm
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
      commandToExecute: driverCommand
    }
  }
}

output node01ExtensionId string = node01Extension.id
output node02ExtensionId string = node02Extension.id
output driverExtensionId string = driverExtension.id
