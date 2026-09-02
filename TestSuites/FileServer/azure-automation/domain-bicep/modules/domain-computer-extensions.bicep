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

@description('Run FileServer tests automatically after configuration')
param enableTestAutoRun bool

@description('URL to Domain-Package.zip file in Azure Storage')
param domainPackageZipUrl string

var driverIsLinux = driverOsType == 'Linux'
var packageHost = empty(domainPackageZipUrl) ? '' : split(domainPackageZipUrl, '/')[2]
var driverLinuxBootstrap = replace(replace(replace(replace(replace(replace(replace(replace(
  loadTextContent('../../shared/scripts/cse-bootstrap.sh'),
  '__SCENARIO__', 'domain'),
  '__ROLE__', 'driver'),
  '__PACKAGE_NAME__', 'Domain-Package'),
  '__DEPLOY_SCRIPT__', 'Deploy-Driver.ps1'),
  '__PACKAGE_URL__', domainPackageZipUrl),
  '__PACKAGE_HOST__', packageHost),
  '__PASSWORD_B64__', base64(adminPassword)),
  '__ENABLE_TEST_AUTORUN__', string(enableTestAutoRun))
var driverCommandToExecute = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-domain-driver\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Domain-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File \'C:\\Temp\\wpts-domain-driver\\cse-bootstrap.ps1\' -Scenario \'domain\' -Role \'driver\' -PackageName \'Domain-Package\' -DeployScript \'Deploy-Driver.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -EnableTestAutoRun \'${enableTestAutoRun}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'
var sutCommandToExecute = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-domain-sut\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Domain-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File \'C:\\Temp\\wpts-domain-sut\\cse-bootstrap.ps1\' -Scenario \'domain\' -Role \'sut\' -PackageName \'Domain-Package\' -DeployScript \'Deploy-SUT.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'

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
      fileUris: [
        domainPackageZipUrl
      ]
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
      fileUris: [
        domainPackageZipUrl
      ]
      commandToExecute: sutCommandToExecute
    }
  }
}
