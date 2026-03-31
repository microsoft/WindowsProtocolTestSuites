@description('Resource group location')
param location string = 'West US 2'

@description('Environment name prefix')
param environmentPrefix string

@description('Admin username for VMs')
param adminUsername string = 'testadmin'

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Domain Controller VM size')
param dcVmSize string = 'Standard_D2s_v3'

@description('Domain Controller OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param dcOsVersion string = '2025-datacenter-azure-edition'

@description('Custom image resource ID for DC VM (overrides marketplace image when set). Must be a Windows image.')
param dcCustomImageId string = ''

@description('Domain name')
param domainName string = 'contoso.com'

@description('Domain NetBIOS name')
param domainNetBiosName string = 'CONTOSO'


@description('External1 subnet ID')
param external1SubnetId string

@description('External2 subnet ID')
param external2SubnetId string

@description('Domain Controller External1 IP address')
param dcExternal1Ip string = '192.168.1.10'

@description('Domain Controller External2 IP address')
param dcExternal2Ip string = '192.168.2.10'

@description('Enable auto-shutdown')
param enableAutoShutdown bool = true

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string = '20:00'

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string = 'UTC'

@description('URL to Cluster-Package.zip file in Azure Storage')
param clusterPackageZipUrl string = ''

// Variables
var dcVmName = '${environmentPrefix}-dc01'
var dcNic1Name = '${dcVmName}-nic1'
var dcNic2Name = '${dcVmName}-nic2'
var dcIsHotpatch = contains(dcOsVersion, 'azure-edition')

var dcImageRef = !empty(dcCustomImageId) ? {
  id: dcCustomImageId
} : {
  publisher: 'MicrosoftWindowsServer'
  offer: 'WindowsServer'
  sku: dcOsVersion
  version: 'latest'
}

// Domain Controller Network Interfaces
resource dcNic1 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: dcNic1Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external1-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: dcExternal1Ip
          subnet: {
            id: external1SubnetId
          }
        }
      }
    ]
    enableIPForwarding: true
    // DNS will be configured by Deploy-DC.ps1 after AD DS installation
    // Initially use Azure default DNS (168.63.129.16) so extension can download packages
  }
}

resource dcNic2 'Microsoft.Network/networkInterfaces@2023-04-01' = {
  name: dcNic2Name
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'external2-config'
        properties: {
          privateIPAllocationMethod: 'Static'
          privateIPAddress: dcExternal2Ip
          subnet: {
            id: external2SubnetId
          }
        }
      }
    ]
    enableIPForwarding: true
    // DNS will be configured by Deploy-DC.ps1 after AD DS installation
    // Initially use Azure default DNS (168.63.129.16) so extension can download packages
  }
}

// Domain Controller VM (DC01)
resource dcVm 'Microsoft.Compute/virtualMachines@2023-03-01' = {
  name: dcVmName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: dcVmSize
    }
    osProfile: {
      computerName: 'DC01'
      adminUsername: adminUsername
      adminPassword: adminPassword
      windowsConfiguration: {
        enableAutomaticUpdates: dcIsHotpatch
        provisionVMAgent: true
        winRM: {
          listeners: [
            {
              protocol: 'Http'
            }
          ]
        }
        patchSettings: {
          patchMode: dcIsHotpatch ? 'AutomaticByPlatform' : 'Manual'
          assessmentMode: 'ImageDefault'
        }
      }
    }
    storageProfile: {
      imageReference: dcImageRef
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
          id: dcNic1.id
          properties: {
            primary: true
          }
        }
        {
          id: dcNic2.id
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
resource dcVmShutdownSchedule 'Microsoft.DevTestLab/schedules@2018-09-15' = if (enableAutoShutdown) {
  name: 'shutdown-computevm-${dcVmName}'
  location: location
  properties: {
    status: 'Enabled'
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: autoShutdownTime
    }
    timeZoneId: autoShutdownTimeZone
    targetResourceId: dcVm.id
    notificationSettings: {
      status: 'Disabled'
    }
  }
}

// VM Extension for Domain Controller setup
resource dcVmExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!empty(clusterPackageZipUrl)) {
  name: 'ConfigureDomainController'
  parent: dcVm
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
      commandToExecute: 'powershell.exe -ExecutionPolicy Unrestricted -Command "Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force; Start-Transcript -Path C:\\dc-extension-setup.log -Append; Write-Output \\"Starting Domain Controller Setup for Cluster Environment...\\"; New-Item -ItemType Directory -Path C:\\Cluster-Package -Force; $zipFile = Get-ChildItem -Path . -Filter *.zip | Select-Object -First 1; if ($zipFile) { Write-Output \\"Extracting $($zipFile.Name)...\\"; Expand-Archive -Path $zipFile.FullName -DestinationPath C:\\Cluster-Package -Force; Write-Output \\"Package extracted successfully\\"; Remove-Item $zipFile.FullName -Force; } else { Write-Output \\"No zip file found\\"; exit 1; }; Write-Output \\"Starting Deploy-DC.ps1 execution...\\"; if (Test-Path C:\\Cluster-Package\\DSC\\Deploy-DC.ps1) { & C:\\Cluster-Package\\DSC\\Deploy-DC.ps1 -WorkingPath C:\\Cluster-Package; } else { Write-Output \\"Deploy-DC.ps1 not found, skipping configuration\\"; }; Write-Output \\"Domain Controller extension setup completed\\"; Stop-Transcript"'
    }
  }
}

// Outputs
output dcVmId string = dcVm.id
output dcVmName string = dcVmName
output dcPrivateIps array = [
  dcExternal1Ip
  dcExternal2Ip
]
output domainName string = domainName
output domainNetBiosName string = domainNetBiosName
output dcExternal1Ip string = dcExternal1Ip
