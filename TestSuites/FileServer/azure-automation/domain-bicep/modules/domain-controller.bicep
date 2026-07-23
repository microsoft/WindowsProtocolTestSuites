// No parameter defaults in this module: defaults live only in the entry-point
// templates (main.bicep / phase1.bicep) and the bicepparam files, so the two
// deployment paths cannot drift.

@description('Resource group location')
param location string

@description('Environment name prefix')
param environmentPrefix string

@description('Admin username for VMs')
param adminUsername string

@description('Admin password for VMs')
@secure()
param adminPassword string

@description('Domain Controller VM size')
param dcVmSize string

@description('Domain Controller OS version')
@allowed([
  '2019-datacenter-gensecond'
  '2022-datacenter-g2'
  '2025-datacenter-azure-edition'
])
param dcOsVersion string

@description('Custom image resource ID for DC VM (overrides marketplace image when set). Must be a Windows image.')
param dcCustomImageId string

@description('Domain name')
param domainName string

@description('Domain NetBIOS name')
param domainNetBiosName string

@description('External1 subnet ID')
param external1SubnetId string

@description('External2 subnet ID')
param external2SubnetId string

@description('Domain Controller External1 IP address')
param dcExternal1Ip string

@description('Domain Controller External2 IP address')
param dcExternal2Ip string

@description('Enable auto-shutdown')
param enableAutoShutdown bool

@description('Auto-shutdown time (HH:mm in UTC)')
param autoShutdownTime string

@description('Auto-shutdown timezone')
param autoShutdownTimeZone string

@description('URL to Domain-Package.zip file in Azure Storage')
param domainPackageZipUrl string

// Variables
var dcIsHotpatch = contains(dcOsVersion, 'azure-edition')

// CSE bootstrap: the shared script (../../shared/scripts/cse-bootstrap.ps1) is
// loaded at compile time, role/package tokens are substituted, and the result
// travels base64-encoded inside the extension's ENCRYPTED protectedSettings. The
// commandToExecute materializes it to a file and runs it (the Windows
// CustomScriptExtension has no 'script' property, and inline one-liners are
// unreviewable and drift between roles).
var packageHost = empty(domainPackageZipUrl) ? '' : split(domainPackageZipUrl, '/')[2]
var dcBootstrap = replace(replace(replace(replace(replace(replace(replace(
  loadTextContent('../../shared/scripts/cse-bootstrap.ps1'),
  '__SCENARIO__', 'domain'),
  '__ROLE__', 'dc'),
  '__PACKAGE_NAME__', 'Domain-Package'),
  '__DEPLOY_SCRIPT__', 'Deploy-DC.ps1'),
  '__PACKAGE_URL__', domainPackageZipUrl),
  '__PACKAGE_HOST__', packageHost),
  '__PASSWORD_B64__', base64(adminPassword))
var dcCommandToExecute = 'powershell.exe -ExecutionPolicy Unrestricted -NoProfile -Command "[System.IO.File]::WriteAllText(\'C:\\domain-dc-bootstrap.ps1\', [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String(\'${base64(dcBootstrap)}\'))); & \'C:\\domain-dc-bootstrap.ps1\'; exit $LASTEXITCODE"'

var dcImageRef = !empty(dcCustomImageId) ? {
  id: dcCustomImageId
} : {
  publisher: 'MicrosoftWindowsServer'
  offer: 'WindowsServer'
  sku: dcOsVersion
  version: 'latest'
}
var dcVmName = '${environmentPrefix}-dc01'
var dcNic1Name = '${dcVmName}-nic1'
var dcNic2Name = '${dcVmName}-nic2'

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
    // DNS will be configured by DSC\Deploy-DC.ps1 after AD DS installation
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
    // DNS will be configured by DSC\Deploy-DC.ps1 after AD DS installation
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
        // AutoLogon is required so that scheduled tasks (TKFRSAR) run under an
        // interactive session after reboots.  The CustomScriptExtension is the sole
        // entry point for Deploy-DC.ps1 -- no FirstLogonCommands to avoid racing
        // with the extension.
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
resource dcVmExtension 'Microsoft.Compute/virtualMachines/extensions@2023-03-01' = if (!empty(domainPackageZipUrl)) {
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
        domainPackageZipUrl
      ]
    }
    protectedSettings: {
      commandToExecute: dcCommandToExecute
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
output dcExternal2Ip string = dcExternal2Ip
