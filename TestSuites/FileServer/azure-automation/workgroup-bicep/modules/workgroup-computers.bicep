// Workgroup Computers Module
// Deploys Driver (Client01) and SUT (Node01) VMs in workgroup mode

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

@description('SUT computer VM size')
param sutVmSize string = 'Standard_D8ls_v5'

@description('Driver computer OS type')
@allowed([
  'Windows'
  'Linux'
])
param driverOsType string = 'Windows'

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

@description('External1 subnet ID')
param external1SubnetId string

@description('External2 subnet ID')
param external2SubnetId string

@description('Driver computer External1 IP address')
param driverExternal1Ip string = '192.168.1.111'

@description('Driver computer External2 IP address')
param driverExternal2Ip string = '192.168.2.111'

@description('SUT computer External1 IP address')
param sutExternal1Ip string = '192.168.1.11'

@description('SUT computer External2 IP address')
param sutExternal2Ip string = '192.168.2.11'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Auto-shutdown time (HHmm in UTC)')
param autoShutdownTime string = '2000'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('URL to the DSC package zip file in Azure Storage (contains DSC/ folder, Config.json, Tools.json)')
param dscPackageZipUrl string = ''

// Variables
var driverIsLinux = driverOsType == 'Linux'
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
    settings: {
      fileUris: [
        dscPackageZipUrl
      ]
    }
    protectedSettings: {
      commandToExecute: 'powershell.exe -ExecutionPolicy Unrestricted -Command "Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force; Start-Transcript -Path C:\\dsc-driver-setup.log -Append; Write-Output \\"Starting DSC Driver Setup...\\"; New-Item -ItemType Directory -Path C:\\Workgroup-Package -Force; $zipFile = Get-ChildItem -Path . -Filter *.zip | Select-Object -First 1; if ($zipFile) { Write-Output \\"Extracting $($zipFile.Name)...\\"; Expand-Archive -Path $zipFile.FullName -DestinationPath C:\\Workgroup-Package -Force; Write-Output \\"Package extracted successfully\\"; Remove-Item $zipFile.FullName -Force; } else { Write-Output \\"No zip file found\\"; exit 1; }; Write-Output \\"Starting Deploy-Driver.ps1 (DSC + imperative)...\\"; if (Test-Path C:\\Workgroup-Package\\DSC\\Deploy-Driver.ps1) { Set-Location C:\\Workgroup-Package\\DSC; .\\Deploy-Driver.ps1 -WorkingPath C:\\Workgroup-Package; } else { Write-Output \\"Deploy-Driver.ps1 not found, skipping configuration\\"; }; Write-Output \\"Driver DSC configuration completed\\"; Stop-Transcript"'
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
    settings: {
      fileUris: [
        dscPackageZipUrl
      ]
    }
    protectedSettings: {
      commandToExecute: 'bash -c "set -e; export DEBIAN_FRONTEND=noninteractive; echo Starting DSC Driver Setup...; mkdir -p /opt/Workgroup-Package; apt-get update -qq; apt-get install -y -qq wget unzip apt-transport-https; if ! command -v pwsh >/dev/null 2>&1; then wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O /tmp/packages-microsoft-prod.deb; dpkg -i /tmp/packages-microsoft-prod.deb; rm -f /tmp/packages-microsoft-prod.deb; apt-get update -qq; apt-get install -y -qq powershell; fi; zipfile=\\$(find /var/lib/waagent/custom-script/download/0 -maxdepth 1 -name *.zip 2>/dev/null | head -1); if [ -n \\"\\$zipfile\\" ]; then echo Extracting \\$zipfile...; unzip -o \\"\\$zipfile\\" -d /opt/Workgroup-Package; rm -f \\"\\$zipfile\\"; else echo No zip file found; exit 1; fi; echo Starting Deploy-Driver.ps1 - DSC + imperative...; if [ -f /opt/Workgroup-Package/DSC/Deploy-Driver.ps1 ]; then cd /opt/Workgroup-Package/DSC; pwsh -ExecutionPolicy Unrestricted -File /opt/Workgroup-Package/DSC/Deploy-Driver.ps1 -WorkingPath /opt/Workgroup-Package; else echo Deploy-Driver.ps1 not found, skipping configuration; fi; echo Driver DSC configuration completed"'
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
    settings: {
      fileUris: [
        dscPackageZipUrl
      ]
    }
    protectedSettings: {
      commandToExecute: 'powershell.exe -ExecutionPolicy Unrestricted -Command "Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force; Start-Transcript -Path C:\\dsc-sut-setup.log -Append; Write-Output \\"Starting DSC SUT Setup...\\"; New-Item -ItemType Directory -Path C:\\Workgroup-Package -Force; $zipFile = Get-ChildItem -Path . -Filter *.zip | Select-Object -First 1; if ($zipFile) { Write-Output \\"Extracting $($zipFile.Name)...\\"; Expand-Archive -Path $zipFile.FullName -DestinationPath C:\\Workgroup-Package -Force; Write-Output \\"Package extracted successfully\\"; Remove-Item $zipFile.FullName -Force; } else { Write-Output \\"No zip file found\\"; exit 1; }; Write-Output \\"Starting Deploy-SUT.ps1 (DSC + imperative)...\\"; if (Test-Path C:\\Workgroup-Package\\DSC\\Deploy-SUT.ps1) { Set-Location C:\\Workgroup-Package\\DSC; .\\Deploy-SUT.ps1 -WorkingPath C:\\Workgroup-Package; } else { Write-Output \\"Deploy-SUT.ps1 not found, skipping configuration\\"; }; Write-Output \\"SUT DSC configuration completed\\"; Stop-Transcript"'
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
