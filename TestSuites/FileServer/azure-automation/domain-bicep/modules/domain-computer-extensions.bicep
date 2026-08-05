@description('Resource group location')
param location string

@description('Existing Driver VM name')
param driverVmName string

@description('Existing SUT VM name')
param sutVmName string

@description('Admin password injected into the package Config.json')
@secure()
param adminPassword string

@description('Driver computer OS type')
@allowed([
  'Windows'
  'Linux'
])
param driverOsType string

@description('URL to Domain-Package.zip file in Azure Storage')
param domainPackageZipUrl string

var driverIsLinux = driverOsType == 'Linux'
var packageHost = empty(domainPackageZipUrl) ? '' : split(domainPackageZipUrl, '/')[2]
var bootstrapPs = loadTextContent('../../shared/scripts/cse-bootstrap.ps1')
var driverBootstrap = replace(replace(replace(replace(replace(replace(replace(
  bootstrapPs,
  '__SCENARIO__', 'domain'),
  '__ROLE__', 'driver'),
  '__PACKAGE_NAME__', 'Domain-Package'),
  '__DEPLOY_SCRIPT__', 'Deploy-Driver.ps1'),
  '__PACKAGE_URL__', domainPackageZipUrl),
  '__PACKAGE_HOST__', packageHost),
  '__PASSWORD_B64__', base64(adminPassword))
var sutBootstrap = replace(replace(replace(replace(replace(replace(replace(
  bootstrapPs,
  '__SCENARIO__', 'domain'),
  '__ROLE__', 'sut'),
  '__PACKAGE_NAME__', 'Domain-Package'),
  '__DEPLOY_SCRIPT__', 'Deploy-SUT.ps1'),
  '__PACKAGE_URL__', domainPackageZipUrl),
  '__PACKAGE_HOST__', packageHost),
  '__PASSWORD_B64__', base64(adminPassword))
var driverLinuxBootstrap = replace(replace(replace(replace(replace(replace(replace(
  loadTextContent('../../shared/scripts/cse-bootstrap.sh'),
  '__SCENARIO__', 'domain'),
  '__ROLE__', 'driver'),
  '__PACKAGE_NAME__', 'Domain-Package'),
  '__DEPLOY_SCRIPT__', 'Deploy-Driver.ps1'),
  '__PACKAGE_URL__', domainPackageZipUrl),
  '__PACKAGE_HOST__', packageHost),
  '__PASSWORD_B64__', base64(adminPassword))
var driverCommandToExecute = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "[System.IO.File]::WriteAllText(\'C:\\domain-driver-bootstrap.ps1\', [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String(\'${base64(driverBootstrap)}\'))); & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File \'C:\\domain-driver-bootstrap.ps1\'; exit $LASTEXITCODE"'
var sutCommandToExecute = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "[System.IO.File]::WriteAllText(\'C:\\domain-sut-bootstrap.ps1\', [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String(\'${base64(sutBootstrap)}\'))); & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File \'C:\\domain-sut-bootstrap.ps1\'; exit $LASTEXITCODE"'

resource driverVm 'Microsoft.Compute/virtualMachines@2023-03-01' existing = {
  name: driverVmName
}

resource sutVm 'Microsoft.Compute/virtualMachines@2023-03-01' existing = {
  name: sutVmName
}

resource driverWinExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!driverIsLinux && !empty(domainPackageZipUrl)) {
  name: 'ConfigureDomainDriver'
  parent: driverVm
  location: location
  properties: {
    publisher: 'Microsoft.Compute'
    type: 'CustomScriptExtension'
    typeHandlerVersion: '1.10'
    autoUpgradeMinorVersion: true
    settings: {}
    protectedSettings: {
      commandToExecute: driverCommandToExecute
    }
  }
}

resource driverLinuxExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (driverIsLinux && !empty(domainPackageZipUrl)) {
  name: 'ConfigureDomainDriverLinux'
  parent: driverVm
  location: location
  properties: {
    publisher: 'Microsoft.Azure.Extensions'
    type: 'CustomScript'
    typeHandlerVersion: '2.1'
    autoUpgradeMinorVersion: true
    settings: {}
    protectedSettings: {
      script: base64(driverLinuxBootstrap)
    }
  }
}

resource sutVmExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!empty(domainPackageZipUrl)) {
  name: 'ConfigureDomainSUT'
  parent: sutVm
  location: location
  properties: {
    publisher: 'Microsoft.Compute'
    type: 'CustomScriptExtension'
    typeHandlerVersion: '1.10'
    autoUpgradeMinorVersion: true
    settings: {}
    protectedSettings: {
      commandToExecute: sutCommandToExecute
    }
  }
}
