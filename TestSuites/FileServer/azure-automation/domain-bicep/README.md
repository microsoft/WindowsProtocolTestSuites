# File Server Protocol Test Suite - Domain Environment Deployment

This folder contains Bicep templates and deployment scripts for creating a complete domain environment for the File Server Protocol Test Suite on Azure.

## Architecture Overview

The domain environment consists of:

- **Domain Controller (DC01)**: Windows Server with Active Directory Domain Services, DNS, and Routing
- **Driver Computer (Client01)**: Windows 11 Enterprise or Ubuntu Linux for running test cases
- **SUT Computer (Node01)**: Windows Server as the System Under Test with File Server role
- **Network Infrastructure**: VNet with dual subnets, Azure Bastion for secure access

The driver computer supports both Windows and Linux. Custom Azure VM images can be used for any VM by providing a resource ID.

## Deployment Strategy

The deployment follows a **two-phase approach** to ensure the Domain Controller is fully configured before deploying domain-joined computers:

### Phase 1: Network + Domain Controller
- Virtual Network with dual subnets (External1, External2)
- Network Security Groups with required firewall rules (SMB, WinRM, RPC, DNS, Kerberos, LDAP)
- Azure Bastion for secure remote access
- Domain Controller (DC01) with AD DS, DNS

### Phase 2: Driver + SUT (after DC ready)
- Driver Computer (Client01) - automatically joins domain
- SUT Computer (Node01) - automatically joins domain, File Server role installed
- DNS configured to point to DC01

### Pre-flight Validation

Before creating any Azure resources, deploy.ps1 validates:
- **VM size availability** with automatic fallback to alternatives if the preferred size is capacity-constrained
- **Regional vCPU quota** to catch quota limits before a long deployment rolls back
- **OS image availability** to verify marketplace images exist in the target region

### On-VM Configuration Flow

Each VM uses a **CustomScriptExtension** that downloads a DSC package zip and runs an orchestrator script (`Deploy-DC.ps1`, `Deploy-Driver.ps1`, or `Deploy-SUT.ps1`). These orchestrators use **registry-based step tracking** with deferred reboots:

1. **Step 0 -> 1**: DSC configuration + domain join (or DC promotion) -> scheduled reboot
2. **Step 1 -> 2** (and beyond): Post-reboot DSC re-apply + imperative configuration (tools, accounts, shares, etc.)

The **TKFRSAR** scheduled task re-runs the orchestrator after each reboot. It includes a repeating 5-minute trigger as a safety net in case the initial startup run fails (e.g., AD services not yet ready). A **reboot circuit breaker** (max 3 reboots) prevents infinite reboot loops.

Each orchestrator writes a **signal file** on completion (e.g., `Deploy-DC.Completed.signal`). The deploy script polls the DC's signal file via `Invoke-AzVMRunCommand` before launching Phase 2.

## Prerequisites

- Azure PowerShell modules: `Az.Accounts`, `Az.Resources`, `Az.Storage`, `Az.Compute`
- Azure subscription with appropriate permissions

## Quick Start

1. **Navigate to the domain-bicep folder**:
   ```powershell
   cd domain-bicep
   ```

2. **Deploy the full domain environment**:
   ```powershell
   $password = Read-Host -Prompt "Enter the admin password" -AsSecureString

   .\deploy.ps1 `
     -SubscriptionId "your-subscription-id" `
     -ResourceGroupName "fileserver-domain-test" `
     -AdminPassword $password
   ```

   Location, VM sizes, OS versions, and all other settings are read from the bicepparam files.

3. **Connect to VMs**: Use Azure Bastion in the portal to RDP/SSH into any VM.

## Resuming a Deployment

If Phase 1 completed but Phase 2 failed, resume without redeploying the network and DC:

```powershell
$password = ConvertTo-SecureString "YourSecurePassword123!" -AsPlainText -Force

.\deploy.ps1 `
  -SubscriptionId "your-subscription-id" `
  -ResourceGroupName "fileserver-domain-test" `
  -AdminPassword $password `
  -SkipPhase1
```

When resuming, the script will:
1. **Verify DC readiness** -- checks the DC signal file before proceeding (skip with `-SkipDCReadyCheck` if you're certain the DC is ready)
2. **Retrieve Phase 1 outputs** -- reads subnet IDs and DC IP from the previous Phase 1 deployment
3. **Reuse the uploaded package** -- searches existing storage accounts in the resource group for a previously uploaded `Domain-Package.zip` and generates a fresh SAS URL

You can also provide a package URL directly:
```powershell
.\deploy.ps1 ... -SkipPhase1 -DscPackageZipUrl "https://storage.blob.core.windows.net/..."
```

### Deploy Phase 1 Only

To deploy just the network and DC (e.g., to manually verify DC setup before continuing):

```powershell
.\deploy.ps1 ... -SkipPhase2
```

## Script Parameters

### Required Parameters

| Parameter | Description |
|-----------|-------------|
| `SubscriptionId` | Azure subscription ID |
| `ResourceGroupName` | Target resource group name |
| `AdminPassword` | SecureString password for VM admin accounts |

### Optional Parameters

| Parameter | Default | Description |
|-----------|---------|-------------|
| `Phase1ParametersFile` | `parameters/phase1.bicepparam` | Bicep parameter file for Phase 1 |
| `Phase2ParametersFile` | `parameters/phase2.bicepparam` | Bicep parameter file for Phase 2 |
| `DscFolderPath` | `DSC` | Local DSC folder to package and upload |
| `DscPackageZipUrl` | *(empty)* | Pre-uploaded package URL (skips packaging/upload) |
| `DCReadyTimeoutMinutes` | `45` | How long to wait for DC configuration |
| `SkipPhase1` | `$false` | Skip Phase 1 and resume from Phase 2 |
| `SkipPhase2` | `$false` | Deploy Phase 1 only |
| `SkipDCReadyCheck` | `$false` | Skip DC readiness verification when resuming |
| `StorageAccountName` | *(auto-generated)* | Name of the storage account for DSC package upload |
| `ValidateOnly` | `$false` | Run pre-flight validation only without deploying resources |
| `SkipDiskEncryption` | `$false` | Skip Azure Disk Encryption on deployed VMs |

## Template Parameters

Edit [parameters/phase1.bicepparam](parameters/phase1.bicepparam) and [parameters/phase2.bicepparam](parameters/phase2.bicepparam) to customize:

| Parameter | File | Default | Description |
|-----------|------|---------|-------------|
| `location` | Both | `West US 2` | Azure region |
| `environmentPrefix` | Both | `fstest-domain` | Prefix for resource names |
| `domainName` | Phase 1 | `contoso.com` | Active Directory domain name |
| `domainNetBiosName` | Phase 1 | `CONTOSO` | Domain NetBIOS name |
| `vnetAddressPrefix` | Phase 1 | `192.168.0.0/16` | Virtual network address space |
| `dcVmSize` | Phase 1 | `Standard_D2s_v5` | DC VM size (auto-fallback if unavailable) |
| `dcOsVersion` | Phase 1 | `2025-datacenter-azure-edition` | DC OS version |
| `dcCustomImageId` | Phase 1 | *(empty)* | Custom image for DC (overrides marketplace) |
| `driverOsType` | Phase 2 | `Windows` | Driver OS: `Windows` or `Linux` |
| `driverVmSize` | Phase 2 | `Standard_F4as_v6` | Driver VM size (auto-fallback) |
| `sutVmSize` | Phase 2 | `Standard_D8ls_v5` | SUT VM size (auto-fallback) |
| `dcExternal1Ip` | Phase 1 | `192.168.1.10` | DC primary IP address |
| `driverExternal1Ip` | Phase 2 | `192.168.1.111` | Driver computer IP |
| `sutExternal1Ip` | Phase 2 | `192.168.1.11` | SUT computer IP |
| `driverCustomImageId` | Phase 2 | *(empty)* | Custom image for driver |
| `sutCustomImageId` | Phase 2 | *(empty)* | Custom image for SUT |

## Network Configuration

Following the FileServer Protocol Test Suite requirements:

```
External1 Network: 192.168.1.0/24
+-- DC01:     192.168.1.10  (Domain Controller)
+-- Node01:   192.168.1.11  (SUT)
+-- Client01: 192.168.1.111 (Driver)

External2 Network: 192.168.2.0/24
+-- DC01:     192.168.2.10  (Domain Controller)
+-- Node01:   192.168.2.11  (SUT)
+-- Client01: 192.168.2.111 (Driver)

Bastion Network: 192.168.0.0/26
+-- Azure Bastion for secure access
```

## Domain Configuration

The deployment creates:

- **Domain**: `contoso.com` (configurable)
- **Domain Admin**: `CONTOSO\testadmin` (uses deployment password)
- **Test accounts**: Created by `Create-TestAccount.ps1` (non-admin users, groups)
- **Guest Account**: Enabled for testing
- **CBAC**: Claim types, resource properties, central access rules/policies
- **GPO**: Claims-based access control policy imported

## Security Notes

> **Warning**: This environment is designed for testing purposes only:
> - All Windows Firewalls are **disabled**
> - Network security groups allow broad access within the VNet
> - AutoLogon is enabled on all VMs for scheduled task execution
> - Not suitable for production use

## Troubleshooting

### Log Files

| VM | Log File | Contents |
|----|----------|----------|
| DC01 | `C:\Domain-Package\DSC\Deploy-DC.log` | DC orchestrator (DSC + promotion + AD config) |
| DC01 | `C:\Domain-Package\DSC\Invoke-DcImperativeSteps.log` | AD accounts, CBAC, GPO, DNS |
| Client01 | `C:\Domain-Package\DSC\Deploy-Driver.log` | Driver orchestrator (DSC + tools + tests) |
| Client01 | `C:\Domain-Package\DSC\Invoke-DriverImperativeSteps.log` | Domain join, tools, RSA keys, ForceLevel2 |
| Node01 | `C:\Domain-Package\DSC\Deploy-SUT.log` | SUT orchestrator (DSC + features + environment) |
| Node01 | `C:\Domain-Package\DSC\Invoke-SutImperativeSteps.log` | Domain join, disks, DFS, QUIC |
| All | `C:\domain-*-setup.log` | CustomScriptExtension bootstrap log |

### Common Issues

1. **DC timeout (signal file not found)**:
   - Connect to DC01 via Bastion and check `C:\Domain-Package\DSC\Deploy-DC.log`
   - Check the deploy step: `Get-ItemProperty HKLM:\SOFTWARE\ProtocolTestSuites`
   - Manually run: `Set-Location C:\Domain-Package\DSC; .\Deploy-DC.ps1 -WorkingPath C:\Domain-Package`
   - Resume Phase 2: `.\deploy.ps1 ... -SkipPhase1`

2. **Phase 2 deployment not found (ARM validation failure)**:
   - The deployment job may have failed silently; check `Get-Job | Receive-Job`
   - Common causes: marketplace terms not accepted, VM size unavailable in zone, quota exceeded
   - Accept marketplace terms: `Get-AzVMImage -Location 'West US 2' -PublisherName 'MicrosoftWindowsDesktop' -Offer 'Windows-11' -Skus 'win11-25h2-ent' | Select -First 1 | Set-AzMarketplaceTerms -Accept`

3. **Domain join failures**:
   - Verify DC is fully configured (signal file exists on DC01)
   - Check DNS: Driver/SUT NICs should point to DC IP (192.168.1.10)
   - Review `Invoke-DriverImperativeSteps.log` or `Invoke-SutImperativeSteps.log`

4. **VM extension failures**:
   - Azure Portal: VM -> Extensions -> ConfigureDomain* -> View detailed status
   - Extension bootstrap log: `C:\domain-dc-setup.log`, `C:\domain-driver-setup.log`, `C:\domain-sut-setup.log`
   - Linux driver: `/var/log/domain-driver-setup.log`

### Checking VM State Remotely

```powershell
# Check DC readiness
$dcVm = Get-AzVM -ResourceGroupName 'myRG' | Where-Object { $_.Name -like '*-dc01' }
Invoke-AzVMRunCommand -ResourceGroupName 'myRG' -VMName $dcVm.Name `
    -CommandId 'RunPowerShellScript' `
    -ScriptString "Test-Path 'C:\Domain-Package\DSC\Deploy-DC.Completed.signal'; Get-ItemProperty 'HKLM:\SOFTWARE\ProtocolTestSuites' -EA SilentlyContinue | Select DeployStep, RebootCount; Get-Content 'C:\Domain-Package\DSC\Deploy-DC.log' -Tail 20"
```

## File Structure

```
domain-bicep/
+-- deploy.ps1                       # Phased deployment orchestrator
+-- phase1.bicep                     # Phase 1: Network + DC
+-- phase2.bicep                     # Phase 2: Driver + SUT
+-- README.md                        # This file
+-- modules/
|   +-- network.bicep               # VNet, subnets, NSGs, Bastion
|   +-- domain-controller.bicep     # DC VM, NICs, extension
|   +-- domain-computers.bicep      # Driver + SUT VMs, NICs, extensions
+-- parameters/
|   +-- phase1.bicepparam           # Phase 1 parameters (single source of truth)
|   +-- phase2.bicepparam           # Phase 2 parameters
+-- DSC/
    +-- Deploy-SUT.ps1              # SUT orchestrator (step 0->1->2->3)
    +-- SUT-Configuration.ps1       # DSC: features, shares, FSRM, registry
    +-- Invoke-SutImperativeSteps.ps1     # SUT: domain join, disks, DFS, QUIC
    +-- Scripts/                    # Domain-specific scripts (domainjoin, PromoteDC, etc.)

../shared/
+-- Deploy-Helpers.psm1             # Azure helpers (connect, storage, polling, quota)
+-- Generate-ConfigJson.ps1         # Config.json generation from bicepparam values
+-- DSC/
    +-- Deploy-DC.ps1               # DC orchestrator (step 0->1->2)
    +-- Deploy-Driver.ps1           # Driver orchestrator (step 0->1->2)
    +-- DC-Configuration.ps1        # DSC: AD DS, RemoteAccess, LDAP, CBAC
    +-- Driver-Configuration.ps1    # DSC: hosts, firewall, PS remoting
    +-- Invoke-DcImperativeSteps.ps1      # DC: promotion, accounts, CBAC, GPO, DNS
    +-- Invoke-DriverImperativeSteps.ps1  # Driver: domain join, tools, RSA, ForceLevel2
    +-- Scripts/                    # Shared scripts (tools install, test run, validation)
```

## Multi-Platform and Custom Image Support

### Linux Driver

To deploy an Ubuntu Linux driver computer, edit `parameters/phase2.bicepparam`:
```bicep
param driverOsType = 'Linux'
param driverLinuxOsVersion = 'server'  // Ubuntu 24.04 LTS
```

The Linux driver skips DSC and domain join, going straight to tools installation and test execution via `pwsh`.

### Custom VM Images

To use custom Azure VM images instead of marketplace defaults, set the image resource IDs:
```bicep
// Phase 1 parameters
param dcCustomImageId = '/subscriptions/.../images/my-custom-dc'

// Phase 2 parameters
param driverCustomImageId = '/subscriptions/.../images/my-custom-driver'
param sutCustomImageId = '/subscriptions/.../images/my-custom-sut'
```

When a custom image ID is provided, it overrides the marketplace image for that VM. Leave empty (`''`) to use the default. Non-driver VMs must use Windows images.

## Known Issues

1. **ksetup trust relationship failure on SUT**: After domain join, `Invoke-SutImperativeSteps.ps1` runs `ksetup /SetComputerPassword` to synchronize with AD. If you see "trust relationship between this workstation and the primary domain failed" errors, check the SUT deploy log at `C:\Domain-Package\DSC\Invoke-SutImperativeSteps.log` for the ksetup output.

2. **Auto-shutdown enabled by default**: All VMs are configured with auto-shutdown. Check the `enableAutoShutdown` and `autoShutdownTime` parameters in your bicepparam file if VMs shut down unexpectedly.

3. **Azure Disk Encryption enabled by default**: Disk encryption is applied to all VMs after deployment. This adds deployment time and requires Key Vault permissions. Use `-SkipDiskEncryption` to disable it.

> **Note:** The template parameters table above covers the most commonly customized settings. Additional parameters such as `adminUsername`, `enableAutoShutdown`, `autoShutdownTime`, `enableDiskEncryption`, and `domainPackageZipUrl` are available. See the [phase1.bicepparam](parameters/phase1.bicepparam) and [phase2.bicepparam](parameters/phase2.bicepparam) files for the full list of configurable values.

## Based on FileServer User Guide

This deployment follows the specifications in:
- **FileServerUserGuide.md** - Section 4.2 (Domain Network Environment)
- **Domain Controller Requirements** - Section 3.3.3 & 5.2.1
- **Network Architecture** - Dual NIC configuration with External1/External2
