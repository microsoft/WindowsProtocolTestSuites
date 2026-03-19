@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix')
param environmentPrefix string

@description('Admin username for VMs')
param adminUsername string = 'testadmin'

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Storage server VM size')
param storageVmSize string = 'Standard_D4s_v5'

@description('Storage server OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param storageOsVersion string = '2022-datacenter-g2'

@description('Custom image resource ID for storage VM (overrides marketplace image when set). Must be a Windows image.')
param storageCustomImageId string = ''

@description('External1 subnet ID')
param external1SubnetId string

@description('Storage server External1 IP address')
param storageExternal1Ip string = '192.168.1.50'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('URL to Cluster-Package.zip file in Azure Storage')
param clusterPackageZipUrl string = ''

// Variables
var storageIsHotpatch = contains(storageOsVersion, 'azure-edition')

var storageImageRef = !empty(storageCustomImageId) ? {
  id: storageCustomImageId
} : {
  publisher: 'MicrosoftWindowsServer'
  offer: 'WindowsServer'
  sku: storageOsVersion
  version: 'latest'
}
var storageVmName = '${environmentPrefix}-storage01'
var storageNic1Name = '${storageVmName}-nic1'

// Storage Server Network Interfaces
resource storageNic1 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: storageNic1Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external1-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: storageExternal1Ip
          subnet: {
            id: external1SubnetId
          }
        }
      }
    ]
    enableIPForwarding: false
  }
}

// Storage Server VM (Storage01) - NOT domain-joined
resource storageVm 'Microsoft.Compute/virtualMachines@2023-03-01' = {
  name: storageVmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: storageVmSize
    }
    osProfile: {
      computerName: 'Storage01'
      adminUsername: adminUsername
      adminPassword: adminPassword
      windowsConfiguration: {
        enableAutomaticUpdates: storageIsHotpatch
        provisionVMAgent: true
        winRM: {
          listeners: [
            {
              protocol: 'Http'
            }
          ]
        }
        patchSettings: {
          patchMode: storageIsHotpatch ? 'AutomaticByPlatform' : 'Manual'
          assessmentMode: 'ImageDefault'
        }
      }
    }
    storageProfile: {
      imageReference: storageImageRef
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
          caching: 'None'
        }
        {
          createOption: 'Empty'
          diskSizeGB: 32
          lun: 1
          managedDisk: {
            storageAccountType: 'Premium_LRS'
          }
          caching: 'None'
        }
      ]
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: storageNic1.id
          properties: {
            primary: true
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
resource storageVmShutdownSchedule 'Microsoft.DevTestLab/schedules@2018-09-15' = if (enableAutoShutdown) {
  name: 'shutdown-computevm-${storageVmName}'
  location: location
  properties: {
    status: 'Enabled'
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: autoShutdownTime
    }
    timeZoneId: autoShutdownTimeZone
    targetResourceId: storageVm.id
    notificationSettings: {
      status: 'Disabled'
    }
  }
}

// VM Extension for Storage Server setup (iSCSI Target configuration)
resource storageVmExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!empty(clusterPackageZipUrl)) {
  name: 'ConfigureStorageServer'
  parent: storageVm
  location: location
  properties: {
    publisher: 'Microsoft.Compute'
    type: 'CustomScriptExtension'
    typeHandlerVersion: '1.10'
    autoUpgradeMinorVersion: true
    settings: {
      fileUris: [
        clusterPackageZipUrl
      ]
    }
    protectedSettings: {
      commandToExecute: 'powershell.exe -ExecutionPolicy Unrestricted -Command "Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force; Start-Transcript -Path C:\\storage-extension-setup.log -Append; Write-Output \\"Starting Storage Server (iSCSI Target) Setup...\\"; New-Item -ItemType Directory -Path C:\\Cluster-Package -Force; $zipFile = Get-ChildItem -Path . -Filter *.zip | Select-Object -First 1; if ($zipFile) { Write-Output \\"Extracting $($zipFile.Name)...\\"; Expand-Archive -Path $zipFile.FullName -DestinationPath C:\\Cluster-Package -Force; Write-Output \\"Package extracted successfully\\"; Remove-Item $zipFile.FullName -Force; } else { Write-Output \\"No zip file found\\"; exit 1; }; Write-Output \\"Starting Deploy-Storage.ps1 execution...\\"; if (Test-Path C:\\Cluster-Package\\DSC\\Deploy-Storage.ps1) { & C:\\Cluster-Package\\DSC\\Deploy-Storage.ps1 -WorkingPath C:\\Cluster-Package; } else { Write-Output \\"Deploy-Storage.ps1 not found, skipping configuration\\"; }; Write-Output \\"Storage Server extension setup completed\\"; Stop-Transcript"'
    }
  }
}

// Outputs
output storageVmId string = storageVm.id
output storageVmName string = storageVmName
output storagePrivateIps array = [
  storageExternal1Ip
]
output storageExternal1Ip string = storageExternal1Ip
