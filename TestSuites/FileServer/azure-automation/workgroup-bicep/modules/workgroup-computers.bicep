// Workgroup Computers Module
// Deploys Driver (Client01) and SUT (Node01) VMs in workgroup mode

// No parameter defaults in this module: defaults live only in the entry-point
// template (main.bicep) and the bicepparam file, so the deployment paths
// cannot drift.

@description('Resource group location')
param location string

@description('Environment name prefix')
param environmentPrefix string

@description('Admin username for VMs')
param adminUsername string

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Driver computer VM size')
param driverVmSize string

@description('SUT computer VM size')
param sutVmSize string

@description('Driver computer OS type')
@allowed([
  'Windows'
  'Linux'
])
param driverOsType string

@description('Driver computer Windows OS version (win10-* or win11-* SKU)')
@allowed([
  'win11-25h2-pro'
  'win11-25h2-ent'
  'win11-24h2-ent'
  'win11-23h2-pro'
  'win11-23h2-ent'
  'win10-22h2-pro'
  'win10-22h2-ent'
])
param driverOsVersion string

@description('Driver computer Linux OS version (Ubuntu SKU)')
@allowed([
  'server'
  'server-arm64'
])
param driverLinuxOsVersion string

@description('Custom image resource ID for driver VM (overrides marketplace image when set)')
param driverCustomImageId string

@description('SUT computer OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param sutOsVersion string

@description('Custom image resource ID for SUT VM (overrides marketplace image when set). Must be a Windows image.')
param sutCustomImageId string

@description('External1 subnet ID')
param external1SubnetId string

@description('External2 subnet ID')
param external2SubnetId string

@description('Driver computer External1 IP address')
param driverExternal1Ip string

@description('Driver computer External2 IP address')
param driverExternal2Ip string

@description('SUT computer External1 IP address')
param sutExternal1Ip string

@description('SUT computer External2 IP address')
param sutExternal2Ip string

@description('Enable auto-shutdown')
param enableAutoShutdown bool

@description('Auto-shutdown time (HHmm in UTC)')
param autoShutdownTime string

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string

@description('URL to the DSC package zip file in Azure Storage (contains DSC/ folder, Config.json, Tools.json)')
param dscPackageZipUrl string

// Variables
var driverIsLinux = driverOsType == 'Linux'

// Windows CSEs download the package through protected fileUris, extract it to a
// staging directory, and invoke the generic bootstrap shipped at package root.
// This keeps commandToExecute below Windows' command-line limit. Linux keeps
// native CustomScript v2 script delivery.
var packageHost = empty(dscPackageZipUrl) ? '' : split(dscPackageZipUrl, '/')[2]
var driverLinuxBootstrap = replace(replace(replace(replace(replace(replace(replace(
  loadTextContent('../../shared/scripts/cse-bootstrap.sh'),
  '__SCENARIO__', 'workgroup'),
  '__ROLE__', 'driver'),
  '__PACKAGE_NAME__', 'Workgroup-Package'),
  '__DEPLOY_SCRIPT__', 'Deploy-Driver.ps1'),
  '__PACKAGE_URL__', dscPackageZipUrl),
  '__PACKAGE_HOST__', packageHost),
  '__PASSWORD_B64__', base64(adminPassword))
var driverCommandToExecute = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-workgroup-driver\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Workgroup-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File \'C:\\Temp\\wpts-workgroup-driver\\cse-bootstrap.ps1\' -Scenario \'workgroup\' -Role \'driver\' -PackageName \'Workgroup-Package\' -DeployScript \'Deploy-Driver.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'
var sutCommandToExecute = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-workgroup-sut\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Workgroup-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File \'C:\\Temp\\wpts-workgroup-sut\\cse-bootstrap.ps1\' -Scenario \'workgroup\' -Role \'sut\' -PackageName \'Workgroup-Package\' -DeployScript \'Deploy-SUT.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'

var driverWindowsOffer = startsWith(driverOsVersion, 'win10-') ? 'Windows-10' : 'Windows-11'
var driverImageRef = !empty(driverCustomImageId) ? {
  id: driverCustomImageId
} : driverIsLinux ? {
  publisher: 'Canonical'
  offer: 'ubuntu-24_04-lts'
  sku: driverLinuxOsVersion
  version: 'latest'
} : {
  publisher: 'MicrosoftWindowsDesktop'
  offer: driverWindowsOffer
  sku: driverOsVersion
  version: 'latest'
}
var sutIsHotpatch = contains(sutOsVersion, 'azure-edition')

var sutImageRef = !empty(sutCustomImageId) ? {
  id: sutCustomImageId
} : {
  publisher: 'MicrosoftWindowsServer'
  offer: 'WindowsServer'
  sku: sutOsVersion
  version: 'latest'
}
var driverVmName = '${environmentPrefix}-client01'
var sutVmName = '${environmentPrefix}-node01'
var driverNic1Name = '${driverVmName}-nic1'
var driverNic2Name = '${driverVmName}-nic2'
var sutNic1Name = '${sutVmName}-nic1'
var sutNic2Name = '${sutVmName}-nic2'

// Driver VM Network Interfaces (no DNS override - use Azure DNS)
resource driverNic1 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: driverNic1Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external1-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: driverExternal1Ip
          subnet: {
            id: external1SubnetId
          }
        }
      }
    ]
    enableIPForwarding: false
  }
}

resource driverNic2 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: driverNic2Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external2-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: driverExternal2Ip
          subnet: {
            id: external2SubnetId
          }
        }
      }
    ]
    enableIPForwarding: false
  }
}

// SUT VM Network Interfaces
resource sutNic1 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: sutNic1Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external1-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: sutExternal1Ip
          subnet: {
            id: external1SubnetId
          }
        }
      }
    ]
    enableIPForwarding: false
  }
}

resource sutNic2 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: sutNic2Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external2-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: sutExternal2Ip
          subnet: {
            id: external2SubnetId
          }
        }
      }
    ]
    enableIPForwarding: false
  }
}

// Driver VM (Client01)
resource driverVm 'Microsoft.Compute/virtualMachines@2023-03-01' = {
  name: driverVmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: driverVmSize
    }
    osProfile: {
      computerName: 'Client01'
      adminUsername: adminUsername
      adminPassword: adminPassword
      windowsConfiguration: driverIsLinux ? null : {
        enableAutomaticUpdates: false
        provisionVMAgent: true
        winRM: {
          listeners: [
            {
              protocol: 'Http'
            }
          ]
        }
        patchSettings: {
          patchMode: 'Manual'
          assessmentMode: 'ImageDefault'
        }
        additionalUnattendContent: [
          {
            passName: 'OobeSystem'
            componentName: 'Microsoft-Windows-Shell-Setup'
            settingName: 'AutoLogon'
            content: '<AutoLogon><Username>${adminUsername}</Username><Password><Value>${adminPassword}</Value><PlainText>true</PlainText></Password><Enabled>true</Enabled></AutoLogon>'
          }
        ]
      }
      linuxConfiguration: driverIsLinux ? {
        disablePasswordAuthentication: false
        provisionVMAgent: true
        patchSettings: {
          patchMode: 'AutomaticByPlatform'
          assessmentMode: 'AutomaticByPlatform'
        }
      } : null
    }
    storageProfile: {
      imageReference: driverImageRef
      osDisk: {
        createOption: 'FromImage'
        diskSizeGB: 128
        managedDisk: {
          storageAccountType: 'Premium_LRS'
        }
      }
      dataDisks: [
        {
          createOption: 'Empty'
          diskSizeGB: 64
          lun: 0
          managedDisk: {
            storageAccountType: 'Premium_LRS'
          }
        }
      ]
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: driverNic1.id
          properties: {
            primary: true
          }
        }
        {
          id: driverNic2.id
          properties: {
            primary: false
          }
        }
      ]
    }
    diagnosticsProfile: {
      bootDiagnostics: {
        enabled: true
      }
    }
  }
}

// SUT VM (Node01)
resource sutVm 'Microsoft.Compute/virtualMachines@2023-03-01' = {
  name: sutVmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: sutVmSize
    }
    osProfile: {
      computerName: 'Node01'
      adminUsername: adminUsername
      adminPassword: adminPassword
      windowsConfiguration: {
        enableAutomaticUpdates: sutIsHotpatch
        provisionVMAgent: true
        winRM: {
          listeners: [
            {
              protocol: 'Http'
            }
          ]
        }
        patchSettings: {
          patchMode: sutIsHotpatch ? 'AutomaticByPlatform' : 'Manual'
          assessmentMode: 'ImageDefault'
        }
        additionalUnattendContent: [
          {
            passName: 'OobeSystem'
            componentName: 'Microsoft-Windows-Shell-Setup'
            settingName: 'AutoLogon'
            content: '<AutoLogon><Username>${adminUsername}</Username><Password><Value>${adminPassword}</Value><PlainText>true</PlainText></Password><Enabled>true</Enabled></AutoLogon>'
          }
        ]
      }
    }
    storageProfile: {
      imageReference: sutImageRef
      osDisk: {
        createOption: 'FromImage'
        diskSizeGB: 128
        managedDisk: {
          storageAccountType: 'Premium_LRS'
        }
      }
      dataDisks: [
        {
          createOption: 'Empty'
          diskSizeGB: 64
          lun: 0
          managedDisk: {
            storageAccountType: 'Premium_LRS'
          }
        }
      ]
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: sutNic1.id
          properties: {
            primary: true
          }
        }
        {
          id: sutNic2.id
          properties: {
            primary: false
          }
        }
      ]
    }
    diagnosticsProfile: {
      bootDiagnostics: {
        enabled: true
      }
    }
  }
}

// Auto-shutdown schedules
resource driverVmShutdownSchedule 'Microsoft.DevTestLab/schedules@2018-09-15' = if (enableAutoShutdown) {
  name: 'shutdown-computevm-${driverVmName}'
  location: location
  properties: {
    status: 'Enabled'
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: autoShutdownTime
    }
    timeZoneId: autoShutdownTimeZone
    targetResourceId: driverVm.id
    notificationSettings: {
      status: 'Disabled'
    }
  }
}

resource sutVmShutdownSchedule 'Microsoft.DevTestLab/schedules@2018-09-15' = if (enableAutoShutdown) {
  name: 'shutdown-computevm-${sutVmName}'
  location: location
  properties: {
    status: 'Enabled'
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: autoShutdownTime
    }
    timeZoneId: autoShutdownTimeZone
    targetResourceId: sutVm.id
    notificationSettings: {
      status: 'Disabled'
    }
  }
}

// Windows VM Extension for Workgroup Driver configuration (DSC + imperative)
resource driverWinExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!driverIsLinux && !empty(dscPackageZipUrl)) {
  name: 'ConfigureWorkgroupDriver'
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
        dscPackageZipUrl
      ]
      commandToExecute: driverCommandToExecute
    }
  }
}

// Linux VM Extension for Workgroup Driver configuration (DSC + imperative)
resource driverLinuxExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (driverIsLinux && !empty(dscPackageZipUrl)) {
  name: 'ConfigureWorkgroupDriverLinux'
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

// SUT VM Extension for Workgroup SUT configuration (DSC + imperative)
resource sutVmExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!empty(dscPackageZipUrl)) {
  name: 'ConfigureWorkgroupSUT'
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
        dscPackageZipUrl
      ]
      commandToExecute: sutCommandToExecute
    }
  }
}

// Outputs
output driverVmId string = driverVm.id
output sutVmId string = sutVm.id
output driverVmName string = driverVmName
output sutVmName string = sutVmName
output driverPrivateIps array = [
  driverExternal1Ip
  driverExternal2Ip
]
output sutPrivateIps array = [
  sutExternal1Ip
  sutExternal2Ip
]
