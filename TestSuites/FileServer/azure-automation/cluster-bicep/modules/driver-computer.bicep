@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix')
param environmentPrefix string

@description('Admin username for VMs')
param adminUsername string = 'testadmin'

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Driver computer VM size')
param driverVmSize string = 'Standard_F4as_v6'

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

@description('External1 subnet ID')
param external1SubnetId string

@description('External2 subnet ID')
param external2SubnetId string

@description('Driver computer External1 IP address')
param driverExternal1Ip string = '192.168.1.111'

@description('Driver computer External2 IP address')
param driverExternal2Ip string = '192.168.2.111'

@description('Domain Controller External1 IP address')
param dcExternal1Ip string = '192.168.1.10'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Run FileServer tests automatically after configuration')
param enableTestAutoRun bool = true

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('URL to Cluster-Package.zip file in Azure Storage')
param clusterPackageZipUrl string = ''

// Variables
var driverVmName = '${environmentPrefix}-client01'
var driverNic1Name = '${driverVmName}-nic1'
var driverNic2Name = '${driverVmName}-nic2'
var driverIsLinux = driverOsType == 'Linux'
var packageHost = empty(clusterPackageZipUrl) ? '' : split(clusterPackageZipUrl, '/')[2]
var driverLinuxBootstrap = replace(replace(replace(replace(replace(replace(replace(replace(
  loadTextContent('../../shared/scripts/cse-bootstrap.sh'),
  '__SCENARIO__', 'cluster'),
  '__ROLE__', 'driver'),
  '__PACKAGE_NAME__', 'Cluster-Package'),
  '__DEPLOY_SCRIPT__', 'Deploy-Driver.ps1'),
  '__PACKAGE_URL__', clusterPackageZipUrl),
  '__PACKAGE_HOST__', packageHost),
  '__PASSWORD_B64__', base64(adminPassword)),
  '__ENABLE_TEST_AUTORUN__', string(enableTestAutoRun))
var driverCommandToExecute = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "$stage=\'C:\\Temp\\wpts-cluster-driver\'; Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue; Expand-Archive -Path \'.\\Cluster-Package.zip\' -DestinationPath $stage -Force; & powershell.exe -ExecutionPolicy Unrestricted -NoProfile -File \'C:\\Temp\\wpts-cluster-driver\\cse-bootstrap.ps1\' -Scenario \'cluster\' -Role \'driver\' -PackageName \'Cluster-Package\' -DeployScript \'Deploy-Driver.ps1\' -PasswordBase64 \'${base64(adminPassword)}\' -EnableTestAutoRun \'${enableTestAutoRun}\' -PackageAlreadyExtracted; exit $LASTEXITCODE"'
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

// Driver VM Network Interfaces
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
    dnsSettings: {
      dnsServers: [
        dcExternal1Ip
      ]
    }
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
    dnsSettings: {
      dnsServers: [
        dcExternal1Ip
      ]
    }
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

// Auto-shutdown schedule
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

// Windows VM Extension for Cluster Driver configuration
resource driverWinExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!driverIsLinux && !empty(clusterPackageZipUrl)) {
  name: 'ConfigureClusterDriver'
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
        clusterPackageZipUrl
      ]
      commandToExecute: driverCommandToExecute
    }
  }
}

// Linux VM Extension for Cluster Driver configuration
resource driverLinuxExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (driverIsLinux && !empty(clusterPackageZipUrl)) {
  name: 'ConfigureClusterDriverLinux'
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

// Outputs
output driverVmId string = driverVm.id
output driverVmName string = driverVmName
output driverPrivateIps array = [
  driverExternal1Ip
  driverExternal2Ip
]
