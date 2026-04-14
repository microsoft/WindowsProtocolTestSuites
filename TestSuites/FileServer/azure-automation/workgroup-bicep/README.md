# File Server Protocol Test Suite - Workgroup Environment Deployment

This directory contains Azure Bicep templates for deploying the File Server Protocol Test Suite in **Workgroup mode** — a simplified configuration without Active Directory Domain Services.

> **Choosing a scenario?** See the [top-level azure-automation README](../README.md) for a comparison of Domain vs. Cluster vs. Workgroup.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                   Azure Resource Group                       │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────┐    │
│  │                    Virtual Network                   │    │
│  │                   192.168.0.0/16                     │    │
│  ├──────────────────┬──────────────────┬───────────────┤    │
│  │ AzureBastionSubnet│ External1 Subnet │External2 Subnet│   │
│  │  192.168.0.0/26  │ 192.168.1.0/24   │192.168.2.0/24 │   │
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

**2 VMs total:**
- **Client01 (Driver)**: Windows 11 or Ubuntu Linux client running the test suite
- **Node01 (SUT)**: Windows Server with File Server role

No Domain Controller is needed — both machines operate in workgroup mode with local accounts.

The driver computer supports both Windows and Linux. Custom Azure VM images can be used for any VM by providing a resource ID.

## Prerequisites

- Azure PowerShell modules: `Az.Accounts`, `Az.Resources`, `Az.Storage`, `Az.Compute`
- Azure subscription with permissions to create resource groups, VMs, storage accounts, and Key Vaults
- Bicep CLI (installed automatically by the script if missing)

## Quick Start

### 1. Navigate to the workgroup-bicep directory

```powershell
cd workgroup-bicep
```

### 2. Run the deployment

```powershell
$adminPassword = Read-Host "Enter admin password" -AsSecureString
$localPassword = Read-Host "Enter local user password" -AsSecureString

.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fs-workgroup-test" `
    -AdminPassword $adminPassword `
    -LocalUserPassword $localPassword
```

### 3. Wait for deployment and automatic tests

The script runs a single Bicep deployment (~5 min for Azure resources). After that, VMs configure themselves in the background:

1. **SUT** (~5-10 min): File Server role, SMB shares, FSRM configuration
2. **Driver** (~10-15 min): Tools install, RSA keys, test environment setup
3. **Tests run automatically** — a scheduled task on the Driver waits for SUT readiness, then executes the full test suite. No login required.

### 4. Check test results

Connect to Client01 via **Azure Bastion** in the portal and check:
```powershell
# See if tests finished
Test-Path C:\Test\test.finished.signal

# View results
Get-ChildItem C:\Test\TestResults\*.trx
```

### Deployment Timeline

| What | Duration | Notes |
|------|----------|-------|
| Bicep deployment (Network + VMs) | ~5 min | Single phase, no DC dependency |
| SUT configuration | ~5-10 min | File Server role, shares, FSRM |
| Driver configuration | ~10-15 min | Tools install, RSA keys |
| Automatic test execution | ~30-60 min | Depends on test scope |
| **Total to test results** | **~50-90 min** | Fully unattended |

## Parameters

### Required Parameters (command-line)

| Parameter | Description |
|-----------|-------------|
| `SubscriptionId` | Azure subscription ID |
| `ResourceGroupName` | Target resource group name |
| `AdminPassword` | SecureString password for testadmin account |
| `LocalUserPassword` | SecureString password for nonadmin test user |

> **Note:** The Azure region is read from `param location` in the bicepparam file (single source of truth). Edit `parameters/workgroup.bicepparam` to change it.

### Optional Parameters (command-line)

| Parameter | Default | Description |
|-----------|---------|-------------|
| `ParametersFile` | `parameters/workgroup.bicepparam` | Bicep parameters file |
| `DscFolderPath` | `DSC/` (adjacent to deploy.ps1) | Path to DSC scripts folder |
| `DscPackageZipUrl` | *(empty)* | Direct URL to pre-built DSC package zip |
| `StorageAccountName` | *(empty)* | Use an existing storage account instead of creating one |
| `ValidateOnly` | `$false` | Run Bicep validation only (no deployment) |
| `SkipDiskEncryption` | `$false` | Skip post-deployment Azure Disk Encryption |
| `Resume` | `$false` | Resume a previously failed deployment (see below) |

### Template Parameters (bicepparam file)

Edit [`parameters/workgroup.bicepparam`](parameters/workgroup.bicepparam) to customize the environment:

| Parameter | Default | Description |
|-----------|---------|-------------|
| `location` | `West US 2` | Azure region |
| `environmentPrefix` | `fstest-wg` | Prefix for resource names |
| `driverVmSize` | `Standard_F4as_v6` | Driver VM size (auto-fallback) |
| `sutVmSize` | `Standard_D8ls_v5` | SUT VM size (auto-fallback) |
| `driverOsType` | `Windows` | Driver OS: `Windows` or `Linux` |
| `driverExternal1Ip` | `192.168.1.111` | Driver IP address |
| `sutExternal1Ip` | `192.168.1.11` | SUT IP address |
| `driverCustomImageId` | *(empty)* | Custom image for driver (overrides marketplace) |
| `sutCustomImageId` | *(empty)* | Custom image for SUT (overrides marketplace) |
| `enableAutoShutdown` | `true` | Auto-shutdown VMs at scheduled time |
| `autoShutdownTime` | `2000` | Shutdown time in 24h format (UTC) |

## Local Accounts

The Workgroup scenario uses local accounts (no domain):

| Account | Purpose |
|---------|---------|
| `testadmin` | Administrator account (VM admin) — password provided via `-AdminPassword` |
| `nonadmin` | Non-admin test user — password provided via `-LocalUserPassword` |
| `Guest` | Guest account (disabled by default, no password) |

## Resuming a Failed Deployment

If the deployment fails partway through (e.g., VM extension error, quota issue), use `-Resume` to retry without repeating pre-flight checks or re-uploading the DSC package:

```powershell
.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fs-workgroup-test" `
    -AdminPassword $adminPassword `
    -LocalUserPassword $localPassword `
    -Resume
```

This skips storage account creation and package upload (reuses the existing blob), then re-runs the Bicep deployment. Azure will update only the resources that changed.

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
  1. Waits for SUT readiness (polls \\192.168.1.11\C$\...\Deploy-SUT.Completed.signal
     via SMB using the SUT's static IP, since DNS is not available in workgroup mode)
  2. Detects SUT OS version → derives context name
     (e.g., Win2025_Workgroup_NonCluster_SMB311)
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

### Change OS Versions

Defaults are `win11-25h2-ent` (driver) and `2025-datacenter-azure-edition` (SUT). To change:
```bicep
param driverOsVersion = 'win11-23h2-pro'
param sutOsVersion = '2022-datacenter-g2'
```

### Use a Linux Driver

```bicep
param driverOsType = 'Linux'
param driverLinuxOsVersion = 'server'  // Ubuntu 24.04 LTS
```

When `driverOsType = 'Linux'`, the driver VM deploys Ubuntu 24.04 LTS and installs PowerShell Core (`pwsh`) to run the same `Deploy-Driver.ps1` scripts used on Windows.

### Use Custom VM Images

```bicep
param driverCustomImageId = '/subscriptions/.../images/my-custom-driver'
param sutCustomImageId = '/subscriptions/.../images/my-custom-sut'
```

When a custom image ID is provided, it overrides the marketplace image. Leave empty (`''`) to use the default.

## Default Behaviors

- **Auto-shutdown** is enabled by default at 20:00 UTC. If a test run takes longer than expected, VMs may shut down mid-test. Disable with `param enableAutoShutdown = false` in the bicepparam file, or adjust the time with `param autoShutdownTime = '2300'`.
- **Azure Disk Encryption** is enabled by default. A Key Vault is created automatically for encryption keys. Skip with `-SkipDiskEncryption` if not needed or to speed up deployment.

## Comparison with Domain Deployment

| Feature | Workgroup | Domain |
|---------|-----------|--------|
| VMs | 2 (Client01, Node01) | 3 (DC01, Client01, Node01) |
| Domain Controller | No | Yes |
| Phased Deployment | No | Yes (DC must be ready first) |
| Authentication | Local accounts | Domain accounts |
| Test Coverage | Basic SMB tests | Full test suite |
| Automatic Tests | Yes (unattended) | Yes (unattended) |
| Deploy Time | ~15 min | ~30 min |

## Troubleshooting

### Log Files

| VM | Log File | Contents |
|----|----------|----------|
| Client01 (Windows) | `C:\Workgroup-Package\DSC\Deploy-Driver.log` | Driver orchestrator |
| Client01 (Windows) | `C:\Workgroup-Package\DSC\Scripts\Invoke-TestRun.log` | Test execution |
| Client01 (Linux) | `/var/log/dsc-driver-setup.log` | Driver bootstrap |
| Node01 | `C:\Workgroup-Package\DSC\Deploy-SUT.log` | SUT orchestrator |
| All (Windows) | `C:\dsc-*-setup.log` | CustomScriptExtension bootstrap |
| Test results | `C:\Test\TestResults\*.trx` | TRX result files |

### Common Issues

1. **VM extension failure**: Check bootstrap logs on the VM (`C:\dsc-*-setup.log`) and the orchestrator logs above.

2. **Tests not running**: Verify the scheduled task exists: `Get-ScheduledTask -TaskName 'RunFileServerTests'`. If it doesn't exist, the Driver configuration hasn't completed yet — check `Deploy-Driver.log`.

3. **SUT readiness timeout**: The test runner waits for the SUT signal file via SMB. If it times out, check `Deploy-SUT.log` on Node01. Verify SMB connectivity: `Test-NetConnection -ComputerName 192.168.1.11 -Port 445`.

4. **Marketplace terms not accepted**: First deployment of a new OS image may require: 
   $terms = Get-AzMarketplaceTerms -Publisher 'MicrosoftWindowsDesktop' -Product 'Windows-11' -Name 'win11-25h2-ent'
   Set-AzMarketplaceTerms -Publisher 'MicrosoftWindowsDesktop' -Product 'Windows-11' -Name 'win11-25h2-ent' -Accept

### Verify Configuration

```powershell
# Check that machines are in workgroup (not domain)
Get-CimInstance Win32_ComputerSystem | Select-Object Domain, PartOfDomain
# Should show: Domain = WORKGROUP, PartOfDomain = False

# Test SMB connectivity from Client01 to Node01
Test-NetConnection -ComputerName 192.168.1.11 -Port 445

# Inspect the generated Config.json
Get-Content C:\Workgroup-Package\DSC\Scripts\Config.json | ConvertFrom-Json | ConvertTo-Json -Depth 5
```

## Directory Structure

```
workgroup-bicep/
├── deploy.ps1                    # Main deployment script
├── main.bicep                    # Main template (single phase)
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
    └── workgroup.bicepparam      # Default parameters (single source of truth)

../shared/                        # Components shared with Domain/Cluster
├── Deploy-Helpers.psm1           # Azure helpers (connect, storage, quota)
├── Generate-ConfigJson.ps1       # Config.json generation from bicepparam values
└── DSC/
    ├── Deploy-Driver.ps1         # Driver orchestrator
    ├── Driver-Configuration.ps1  # DSC: hosts, firewall, PS remoting
    └── Scripts/                  # Shared utilities (tools install, test run, etc.)
```

## Related Documentation

- [Top-level azure-automation README](../README.md) — Scenario comparison and shared concepts
- [Domain Deployment](../domain-bicep/README.md) — Full domain environment (3 VMs)
- [Cluster Deployment](../cluster-bicep/README.md) — Clustered file server environment (5 VMs)
- [File Server Test Suite User Guide](https://github.com/microsoft/WindowsProtocolTestSuites/wiki/MS-FILESERVER---User-Guide)
