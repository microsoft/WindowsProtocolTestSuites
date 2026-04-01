# Workgroup File Server Test Suite - Azure Deployment

This directory contains Azure Bicep templates for deploying the File Server Protocol Test Suite in **Workgroup mode** - a simplified configuration without Active Directory Domain Services.

## Overview

The Workgroup deployment creates:
- **Client01 (Driver)**: Windows 11 or Ubuntu Linux client running the test suite
- **Node01 (SUT)**: Windows Server with File Server role

The driver computer supports both Windows and Linux. Set `driverOsType = 'Linux'` to deploy an Ubuntu driver. Custom Azure VM images can also be used for any VM by providing a resource ID.

No Domain Controller is needed for this scenario - both machines operate in workgroup mode with local accounts.

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                   Azure Resource Group                       │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────┐    │
│  │                    Virtual Network                   │    │
│  │                   192.168.0.0/16                    │    │
│  ├──────────────────┬──────────────────┬───────────────┤    │
│  │ AzureBastionSubnet│ External1 Subnet │ External2 Subnet│ │
│  │  192.168.0.0/26  │ 192.168.1.0/24   │ 192.168.2.0/24│    │
│  │                  │                  │               │    │
│  │ ┌──────────────┐ │ ┌──────────────┐ │               │    │
│  │ │   Bastion    │ │ │   Client01   │←→───────────────┤    │
│  │ │   (Access)   │ │ │  .1.111      │ │   .2.111      │    │
│  │ └──────────────┘ │ └──────────────┘ │               │    │
│  │                  │ ┌──────────────┐ │               │    │
│  │                  │ │    Node01    │←→───────────────┤    │
│  │                  │ │   .1.11      │ │    .2.11      │    │
│  │                  │ └──────────────┘ │               │    │
│  └──────────────────┴──────────────────┴───────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

## Prerequisites

- Azure subscription with appropriate permissions
- Azure CLI installed and authenticated
- Azure PowerShell modules: Az.Accounts, Az.Resources, Az.Storage, Az.Compute
- Workgroup DSC folder (included in `DSC/` subdirectory)

## Quick Start

1. **Clone and navigate to the workgroup-bicep directory**

2. **Run the deployment script**:
   ```powershell
   $adminPassword = Read-Host "Enter admin password" -AsSecureString
   $localPassword = Read-Host "Enter local user password" -AsSecureString
   .\deploy.ps1 `
       -SubscriptionId "your-subscription-id" `
       -ResourceGroupName "fs-workgroup-test" `
       -AdminPassword $adminPassword `
       -LocalUserPassword $localPassword
   ```

3. **Tests run automatically** — no login required. The Driver VM waits for SUT readiness, then runs the full test suite via `Execute-TestCaseByContext.ps1`. See [Automatic Test Execution](#automatic-test-execution) for details.

## Parameters

### Required Parameters

| Parameter | Description |
|-----------|-------------|
| `SubscriptionId` | Azure subscription ID |
| `ResourceGroupName` | Target resource group name |
| `AdminPassword` | SecureString password for testadmin account |
| `LocalUserPassword` | SecureString password for nonadmin test user |

> **Note:** The Azure region is read from `param location` in the bicepparam file (single source of truth). Edit `parameters/workgroup.bicepparam` to change it.

### Optional Parameters

| Parameter | Default | Description |
|-----------|---------|-------------|
| `ParametersFile` | `parameters/workgroup.bicepparam` | Bicep parameters file |
| `DscFolderPath` | `DSC/` (adjacent to deploy.ps1) | Path to DSC scripts folder |
| `DscPackageZipUrl` | (empty) | Direct URL to pre-built DSC package zip |
| `StorageAccountName` | (empty) | Use an existing storage account instead of creating one |
| `ValidateOnly` | `$false` | Run Bicep validation only (no deployment) |
| `SkipDiskEncryption` | `$false` | Skip post-deployment Azure Disk Encryption |
| `Resume` | `$false` | Resume a previously failed deployment (skips pre-flight checks) |

## Local Accounts

The Workgroup scenario uses local accounts:

| Account | Purpose |
|---------|---------|
| `testadmin` | Administrator account (VM admin) - password provided via `-AdminPassword` |
| `nonadmin` | Non-admin test user - password provided via `-LocalUserPassword` |
| `Guest` | Guest account (disabled by default, no password) |

## Network Configuration

| Machine | External1 IP | External2 IP |
|---------|-------------|-------------|
| Client01 | 192.168.1.111 | 192.168.2.111 |
| Node01 | 192.168.1.11 | 192.168.2.11 |

## Directory Structure

```
workgroup-bicep/
├── deploy.ps1                    # Main deployment script
├── main.bicep                    # Main template
├── README.md                     # This file
├── DSC/                          # SUT DSC configuration scripts
│   ├── Deploy-SUT.ps1            # SUT orchestrator (DSC + imperative)
│   ├── SUT-Configuration.ps1     # SUT DSC configuration
│   ├── Invoke-SutImperativeSteps.ps1     # SUT non-DSC steps
│   └── Scripts/
│       └── Config.json                   # Workgroup scenario template
│       (Shared scripts overlaid at deploy time from ../shared/DSC/Scripts/)
│   # NOTE: Driver scripts (Deploy-Driver.ps1, Driver-Configuration.ps1,
│   #   Invoke-DriverImperativeSteps.ps1) come from ../shared/DSC/ at package time.
├── modules/
│   ├── network.bicep             # VNet, NSGs, Bastion
│   └── workgroup-computers.bicep # Client01, Node01 VMs
└── parameters/
    └── workgroup.bicepparam      # Default parameters
```

## Automatic Test Execution

After deployment completes, the Driver VM automatically runs the FileServer test suite — **no login required**. A scheduled task (`RunFileServerTests`) fires ~30 seconds after `Deploy-Driver.ps1` finishes, running non-interactively with stored credentials.

### Execution flow

```
Deploy-Driver.ps1 (VM extension, runs as SYSTEM)
  Phase 1: DSC (hosts file, firewall, PS remoting)
  Phase 2: Imperative (tools install, RSA keys, ForceLevel2)
  Phase 3: Register scheduled task → fires in ~30s
  → Deploy-Driver.Completed.signal

Scheduled task → Invoke-TestRun.ps1 (runs as local testadmin user)
  1. Waits for SUT readiness (polls \\192.168.1.11\C$\...\Deploy-SUT.Completed.signal via SMB using the SUT's static IP, since DNS is not available in workgroup mode)
  2. Detects SUT OS version → derives context name (e.g., Win2025_Workgroup_NonCluster_SMB311)
  3. Calls Execute-TestCaseByContext.ps1, which:
     - Patches all 9 ptfconfig files with context-aware values from Config.json
     - Runs SMB2Model tests in 4 parallel shards
     - Runs MS-SMB2, MS-DFSC, ServerFailover, Auth, MS-SQOS, MS-RSVD tests
     - Runs FSA tests per-filesystem (NTFS, REFS, FAT32)
     - Writes TRX results to C:\Test\TestResults\
     - Writes C:\Test\test.finished.signal on completion
  4. Cleans up the scheduled task (self-unregisters)
```

### Test filter

To run a subset of tests, run `Invoke-TestRun.ps1` manually:

```powershell
C:\Workgroup-Package\DSC\Scripts\Invoke-TestRun.ps1 -Filter "TestCategory=BVT"
```

### Re-running tests

To re-run tests after a completed run, delete the signal file and run the script again:

```powershell
Remove-Item C:\Test\test.finished.signal
C:\Workgroup-Package\DSC\Scripts\Invoke-TestRun.ps1
```

## Customization

### Modify VM Sizes

Edit `parameters/workgroup.bicepparam`:
```bicep
param driverVmSize = 'Standard_F8as_v6'  // Larger for faster tests
param sutVmSize = 'Standard_D16ls_v5'    // More resources for File Server
```

### Change IP Addresses

Edit `parameters/workgroup.bicepparam`:
```bicep
param driverExternal1Ip = '192.168.1.200'
param sutExternal1Ip = '192.168.1.100'
```

### Change OS Versions

Defaults are `win11-25h2-ent` (driver) and `2025-datacenter-azure-edition` (SUT). To downgrade, edit `parameters/workgroup.bicepparam`:
```bicep
param driverOsVersion = 'win11-23h2-pro'       // downgrade from default win11-25h2-ent
param sutOsVersion = '2022-datacenter-g2'       // downgrade from default 2025-datacenter-azure-edition
```

### Use a Linux Driver

Edit `parameters/workgroup.bicepparam`:
```bicep
param driverOsType = 'Linux'
param driverLinuxOsVersion = 'server'  // Ubuntu 24.04 LTS
```

When `driverOsType = 'Linux'`, the driver VM deploys Ubuntu 24.04 LTS and installs PowerShell Core (`pwsh`) to run the same `Deploy-Driver.ps1` DSC scripts used on Windows.

### Use Custom VM Images

To use a custom Azure VM image instead of the marketplace default, set the image resource ID:
```bicep
param driverCustomImageId = '/subscriptions/.../images/my-custom-driver'
param sutCustomImageId = '/subscriptions/.../images/my-custom-sut'
```

When a custom image ID is provided, it overrides the marketplace image. Leave empty (`''`) to use the default marketplace image.

## Default Behaviors

- **Auto-shutdown** is enabled by default at 20:00 UTC. If a test run takes longer than expected, VMs may shut down mid-test. Disable with `param enableAutoShutdown = false` in the bicepparam file, or adjust the time with `param autoShutdownTime = '2300'`.
- **Azure Disk Encryption** is enabled by default. A Key Vault is created automatically for encryption keys. Skip with `-SkipDiskEncryption` if not needed or to speed up deployment.

## Comparison with Domain Deployment

| Feature | Workgroup | Domain |
|---------|-----------|--------|
| VMs | 2 (Client01, Node01) | 3 (DC01, Client01, Node01) |
| Domain Controller | No | Yes |
| Phased Deployment | No | Yes (DC ready) |
| Authentication | Local accounts | Domain accounts |
| Test Coverage | Basic SMB tests | Full test suite |

## Troubleshooting

### Check VM Extension Logs

Connect via Bastion and check:
- Driver (Windows): `C:\dsc-driver-setup.log` and `C:\Workgroup-Package\DSC\Deploy-Driver.log`
- Driver (Linux): `/var/log/dsc-driver-setup.log`
- SUT: `C:\dsc-sut-setup.log` and `C:\Workgroup-Package\DSC\Deploy-SUT.log`
- Test run: `C:\Workgroup-Package\DSC\Invoke-TestRun.log` and results in `C:\Test\TestResults\`

### Verify Workgroup Configuration

```powershell
# Check that machines are in workgroup (not domain)
Get-CimInstance Win32_ComputerSystem | Select-Object Domain, PartOfDomain
# Should show: Domain = WORKGROUP, PartOfDomain = False
```

### Test SMB Connectivity

```powershell
# From Client01, test connection to Node01
Test-NetConnection -ComputerName 192.168.1.11 -Port 445
```

### Inspect Config.json

The generated `Config.json` drives all test configuration. Verify that IPs and credentials were injected correctly:
```powershell
# On either VM, check the deployed Config.json
Get-Content C:\Workgroup-Package\DSC\Scripts\Config.json | ConvertFrom-Json | ConvertTo-Json -Depth 5
# Verify: Machines.SUT.IpConfig[0].Ip should be 192.168.1.11
# Verify: Core.Username / Core.Password match the credentials you provided
```

### WinRM Connectivity

```powershell
# From Client01, test WinRM to Node01
Test-WSMan -ComputerName 192.168.1.11
```

## Related Documentation

- [File Server Test Suite User Guide](https://github.com/microsoft/WindowsProtocolTestSuites/wiki/MS-FILESERVER---User-Guide)
- [Domain Deployment](../domain-bicep/README.md) - Full domain environment
- [Cluster Deployment](../cluster-bicep/README.md) - Clustered file server environment
- [Shared Helpers](../shared/README.md) - Common deployment functions
