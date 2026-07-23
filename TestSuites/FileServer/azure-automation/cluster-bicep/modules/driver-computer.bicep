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
var driverImageRef = !empty(driverCustomImageId) ? {
  id: driverCustomImageId
} : driverIsLinux ? {
  publisher: 'Canonical'
  offer: 'ubuntu-24_04-lts'
  sku: driverLinuxOsVersion
  version: 'latest'
} : {
  publisher: 'MicrosoftWindowsDesktop'
  offer: 'Windows-11'
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
    settings: {
      fileUris: [
        clusterPackageZipUrl
      ]
    }
    protectedSettings: {
      commandToExecute: 'powershell.exe -ExecutionPolicy Unrestricted -Command "Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force; Start-Transcript -Path C:\\driver-extension-setup.log -Append; Write-Output \\"Starting Driver Computer Setup for Cluster Testing...\\"; New-Item -ItemType Directory -Path C:\\Cluster-Package -Force; $zipFile = Get-ChildItem -Path . -Filter *.zip | Select-Object -First 1; if ($zipFile) { Write-Output \\"Extracting $($zipFile.Name)...\\"; Expand-Archive -Path $zipFile.FullName -DestinationPath C:\\Cluster-Package -Force; Write-Output \\"Package extracted successfully\\"; Remove-Item $zipFile.FullName -Force; } else { Write-Output \\"No zip file found\\"; exit 1; }; Write-Output \\"Starting Deploy-Driver.ps1 execution...\\"; if (Test-Path C:\\Cluster-Package\\DSC\\Deploy-Driver.ps1) { & C:\\Cluster-Package\\DSC\\Deploy-Driver.ps1 -WorkingPath C:\\Cluster-Package; } else { Write-Output \\"Deploy-Driver.ps1 not found, skipping configuration\\"; }; Write-Output \\"Driver Computer extension setup completed\\"; Stop-Transcript"'
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
    settings: {
      fileUris: [
        clusterPackageZipUrl
      ]
    }
    protectedSettings: {
      commandToExecute: 'bash -c "set -e; export DEBIAN_FRONTEND=noninteractive; echo Starting Driver Computer Setup for Cluster Testing...; mkdir -p /opt/Cluster-Package; apt-get update -qq; apt-get install -y -qq wget unzip apt-transport-https; if ! command -v pwsh >/dev/null 2>&1; then wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O /tmp/packages-microsoft-prod.deb; dpkg -i /tmp/packages-microsoft-prod.deb; rm -f /tmp/packages-microsoft-prod.deb; apt-get update -qq; apt-get install -y -qq powershell; fi; zipfile=\\$(find /var/lib/waagent/custom-script/download/0 -maxdepth 1 -name *.zip 2>/dev/null | head -1); if [ -n \\"\\$zipfile\\" ]; then echo Extracting \\$zipfile...; unzip -o \\"\\$zipfile\\" -d /opt/Cluster-Package; rm -f \\"\\$zipfile\\"; else echo No zip file found; exit 1; fi; echo Starting Deploy-Driver.ps1 execution...; if [ -f /opt/Cluster-Package/DSC/Deploy-Driver.ps1 ]; then pwsh -ExecutionPolicy Unrestricted -File /opt/Cluster-Package/DSC/Deploy-Driver.ps1 -WorkingPath /opt/Cluster-Package; else echo Deploy-Driver.ps1 not found, skipping configuration; fi; echo Driver Computer extension setup completed"'
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
