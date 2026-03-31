@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix')
param environmentPrefix string

@description('Admin username for VMs')
param adminUsername string = 'testadmin'

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Cluster node VM size')
param clusterNodeVmSize string = 'Standard_D8s_v5'

@description('Cluster nodes OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param clusterNodeOsVersion string = '2025-datacenter-azure-edition'

@description('Custom image resource ID for cluster node VMs (overrides marketplace image when set). Must be a Windows image.')
param clusterNodeCustomImageId string = ''

@description('External1 subnet ID')
param external1SubnetId string

@description('External2 subnet ID')
param external2SubnetId string

@description('Node01 External1 IP address')
param node01External1Ip string = '192.168.1.11'

@description('Node01 External2 IP address')
param node01External2Ip string = '192.168.2.11'

@description('Node02 External1 IP address')
param node02External1Ip string = '192.168.1.12'

@description('Node02 External2 IP address')
param node02External2Ip string = '192.168.2.12'

@description('Domain Controller External1 IP address')
param dcExternal1Ip string = '192.168.1.10'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('URL to Cluster-Package.zip file in Azure Storage')
param clusterPackageZipUrl string = ''

// Variables
var nodeIsHotpatch = contains(clusterNodeOsVersion, 'azure-edition')

var nodeImageRef = !empty(clusterNodeCustomImageId) ? {
  id: clusterNodeCustomImageId
} : {
  publisher: 'MicrosoftWindowsServer'
  offer: 'WindowsServer'
  sku: clusterNodeOsVersion
  version: 'latest'
}
var node01VmName = '${environmentPrefix}-node01'
var node02VmName = '${environmentPrefix}-node02'
var node01Nic1Name = '${node01VmName}-nic1'
var node01Nic2Name = '${node01VmName}-nic2'
var node02Nic1Name = '${node02VmName}-nic1'
var node02Nic2Name = '${node02VmName}-nic2'

// Node01 Network Interfaces
resource node01Nic1 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: node01Nic1Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external1-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: node01External1Ip
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

resource node01Nic2 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: node01Nic2Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external2-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: node01External2Ip
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

// Node02 Network Interfaces
resource node02Nic1 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: node02Nic1Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external1-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: node02External1Ip
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

resource node02Nic2 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: node02Nic2Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external2-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: node02External2Ip
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

// Node01 VM
resource node01Vm 'Microsoft.Compute/virtualMachines@2023-03-01' = {
  name: node01VmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: clusterNodeVmSize
    }
    osProfile: {
      computerName: 'Node01'
      adminUsername: adminUsername
      adminPassword: adminPassword
      windowsConfiguration: {
        enableAutomaticUpdates: nodeIsHotpatch
        provisionVMAgent: true
        winRM: {
          listeners: [
            {
              protocol: 'Http'
            }
          ]
        }
        patchSettings: {
          patchMode: nodeIsHotpatch ? 'AutomaticByPlatform' : 'Manual'
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
      imageReference: nodeImageRef
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
          id: node01Nic1.id
          properties: {
            primary: true
          }
        }
        {
          id: node01Nic2.id
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

// Node02 VM
resource node02Vm 'Microsoft.Compute/virtualMachines@2023-03-01' = {
  name: node02VmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: clusterNodeVmSize
    }
    osProfile: {
      computerName: 'Node02'
      adminUsername: adminUsername
      adminPassword: adminPassword
      windowsConfiguration: {
        enableAutomaticUpdates: nodeIsHotpatch
        provisionVMAgent: true
        winRM: {
          listeners: [
            {
              protocol: 'Http'
            }
          ]
        }
        patchSettings: {
          patchMode: nodeIsHotpatch ? 'AutomaticByPlatform' : 'Manual'
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
      imageReference: nodeImageRef
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
          id: node02Nic1.id
          properties: {
            primary: true
          }
        }
        {
          id: node02Nic2.id
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
resource node01VmShutdownSchedule 'Microsoft.DevTestLab/schedules@2018-09-15' = if (enableAutoShutdown) {
  name: 'shutdown-computevm-${node01VmName}'
  location: location
  properties: {
    status: 'Enabled'
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: autoShutdownTime
    }
    timeZoneId: autoShutdownTimeZone
    targetResourceId: node01Vm.id
    notificationSettings: {
      status: 'Disabled'
    }
  }
}

resource node02VmShutdownSchedule 'Microsoft.DevTestLab/schedules@2018-09-15' = if (enableAutoShutdown) {
  name: 'shutdown-computevm-${node02VmName}'
  location: location
  properties: {
    status: 'Enabled'
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: autoShutdownTime
    }
    timeZoneId: autoShutdownTimeZone
    targetResourceId: node02Vm.id
    notificationSettings: {
      status: 'Disabled'
    }
  }
}

// VM Extensions for cluster node configuration
resource node01VmExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!empty(clusterPackageZipUrl)) {
  name: 'ConfigureClusterNode01'
  parent: node01Vm
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
      commandToExecute: 'powershell.exe -ExecutionPolicy Unrestricted -Command "Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force; Start-Transcript -Path C:\\node01-extension-setup.log -Append; Write-Output \\"Starting Cluster Node01 Setup...\\"; New-Item -ItemType Directory -Path C:\\Cluster-Package -Force; $zipFile = Get-ChildItem -Path . -Filter *.zip | Select-Object -First 1; if ($zipFile) { Write-Output \\"Extracting $($zipFile.Name)...\\"; Expand-Archive -Path $zipFile.FullName -DestinationPath C:\\Cluster-Package -Force; Write-Output \\"Package extracted successfully\\"; Remove-Item $zipFile.FullName -Force; } else { Write-Output \\"No zip file found\\"; exit 1; }; Write-Output \\"Starting Deploy-Node01.ps1 execution...\\"; if (Test-Path C:\\Cluster-Package\\DSC\\Deploy-Node01.ps1) { & C:\\Cluster-Package\\DSC\\Deploy-Node01.ps1 -WorkingPath C:\\Cluster-Package; } else { Write-Output \\"Deploy-Node01.ps1 not found, skipping configuration\\"; }; Write-Output \\"Node01 extension setup completed\\"; Stop-Transcript"'
    }
  }
}

resource node02VmExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!empty(clusterPackageZipUrl)) {
  name: 'ConfigureClusterNode02'
  parent: node02Vm
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
      commandToExecute: 'powershell.exe -ExecutionPolicy Unrestricted -Command "Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force; Start-Transcript -Path C:\\node02-extension-setup.log -Append; Write-Output \\"Starting Cluster Node02 Setup...\\"; New-Item -ItemType Directory -Path C:\\Cluster-Package -Force; $zipFile = Get-ChildItem -Path . -Filter *.zip | Select-Object -First 1; if ($zipFile) { Write-Output \\"Extracting $($zipFile.Name)...\\"; Expand-Archive -Path $zipFile.FullName -DestinationPath C:\\Cluster-Package -Force; Write-Output \\"Package extracted successfully\\"; Remove-Item $zipFile.FullName -Force; } else { Write-Output \\"No zip file found\\"; exit 1; }; Write-Output \\"Starting Deploy-Node02.ps1 execution...\\"; if (Test-Path C:\\Cluster-Package\\DSC\\Deploy-Node02.ps1) { & C:\\Cluster-Package\\DSC\\Deploy-Node02.ps1 -WorkingPath C:\\Cluster-Package; } else { Write-Output \\"Deploy-Node02.ps1 not found, skipping configuration\\"; }; Write-Output \\"Node02 extension setup completed\\"; Stop-Transcript"'
    }
  }
}

// Outputs
output node01VmId string = node01Vm.id
output node02VmId string = node02Vm.id
output node01VmName string = node01VmName
output node02VmName string = node02VmName
output node01PrivateIps array = [
  node01External1Ip
  node01External2Ip
]
output node02PrivateIps array = [
  node02External1Ip
  node02External2Ip
]
