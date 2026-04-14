# File Server Protocol Test Suite - Failover Cluster Environment Deployment

This folder contains Bicep templates and deployment scripts for creating a complete **failover cluster environment** for the File Server Protocol Test Suite on Azure.

> **Choosing a scenario?** See the [top-level azure-automation README](../README.md) for a comparison of Domain vs. Cluster vs. Workgroup.

## Architecture Overview

```
┌──────────────────────────────────────────────────────────────────────┐
│                        Azure Resource Group                          │
├──────────────────────────────────────────────────────────────────────┤
│  ┌────────────────────────────────────────────────────────────┐      │
│  │                     Virtual Network                        │      │
│  │                    192.168.0.0/16                           │      │
│  ├────────────────┬────────────────────────┬─────────────────┤      │
│  │  Bastion       │  External1 Subnet      │ External2 Subnet│      │
│  │ 192.168.0.0/26 │  192.168.1.0/24        │ 192.168.2.0/24  │      │
│  │                │                        │                 │      │
│  │ ┌────────────┐ │ ┌────────────────────┐ │                 │      │
│  │ │  Bastion   │ │ │  DC01          .10 │←→──── .10        │      │
│  │ │  (Access)  │ │ │  Domain Controller │ │                 │      │
│  │ └────────────┘ │ └────────────────────┘ │                 │      │
│  │                │ ┌────────────────────┐ │                 │      │
│  │                │ │  Storage01     .50 │ │  (no External2) │      │
│  │                │ │  iSCSI Target      │ │                 │      │
│  │                │ │  NOT domain-joined │ │                 │      │
│  │                │ └────────────────────┘ │                 │      │
│  │                │ ┌────────────────────┐ │                 │      │
│  │                │ │  Node01        .11 │←→──── .11        │      │
│  │                │ │  Cluster Node 1    │ │                 │      │
│  │                │ └────────────────────┘ │                 │      │
│  │                │ ┌────────────────────┐ │                 │      │
│  │                │ │  Node02        .12 │←→──── .12        │      │
│  │                │ │  Cluster Node 2    │ │                 │      │
│  │                │ └────────────────────┘ │                 │      │
│  │                │ ┌────────────────────┐ │                 │      │
│  │                │ │  Client01     .111 │←→──── .111       │      │
│  │                │ │  Driver (Tests)    │ │                 │      │
│  │                │ └────────────────────┘ │                 │      │
│  └────────────────┴────────────────────────┴─────────────────┘      │
│                                                                      │
│  iSCSI Storage (on Storage01):                                       │
│    disk1 (10GB) ── disk2 (10GB) ── disk3 (10GB) ── diskq (1GB)     │
└──────────────────────────────────────────────────────────────────────┘
```

**5 VMs total:**
- **Domain Controller (DC01)**: Windows Server with Active Directory Domain Services, DNS
- **Storage Server (Storage01)**: Windows Server with iSCSI Target Server (NOT domain-joined)
- **Cluster Node01**: Windows Server with Failover Clustering and File Server roles
- **Cluster Node02**: Windows Server with Failover Clustering and File Server roles
- **Driver Computer (Client01)**: Windows 11 Enterprise or Ubuntu Linux for running test cases

The driver computer supports both Windows and Linux. Custom Azure VM images can be used for any VM by providing a resource ID.

## Prerequisites

- Azure PowerShell modules: `Az.Accounts`, `Az.Resources`, `Az.Storage`, `Az.Compute`
- Azure subscription with permissions to create resource groups, VMs, storage accounts, and Key Vaults
- Bicep CLI (installed automatically by the script if missing)

## Quick Start

### 1. Navigate to the cluster-bicep folder

```powershell
cd cluster-bicep
```

### 2. Deploy the cluster environment

```powershell
$password = Read-Host -Prompt "Enter your secure password" -AsSecureString

.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fileserver-cluster-test" `
    -AdminPassword $password
```

### 3. Wait for deployment to complete

The script deploys in two Bicep phases with progress output:

1. **Phase 1** (~5 min): Network + DC + Storage VMs
2. **DC readiness poll** (~15-25 min): Script waits for AD DS promotion to finish
3. **Phase 2** (~10-15 min): Cluster Nodes + Driver VM

After Phase 2, VMs continue configuring in the background (domain join, iSCSI connection, feature installation). Use the verification script to monitor progress:

```powershell
.\scripts\Verify-ClusterDeployment.ps1 -ResourceGroupName "fileserver-cluster-test"
```

This polls all 5 VMs for their completion signal files and displays status:
```
[15.2 min] Checking VM configuration status...
  Checking fstest-cluster-dc01 (Domain Controller)...
    ✅ COMPLETE - Signal file found
  Checking fstest-cluster-storage01 (Storage Server)...
    ✅ COMPLETE - Signal file found
  Checking fstest-cluster-node01 (Cluster Node 1)...
    ⏳ IN PROGRESS - Signal file not found yet
```

**Expected signal files** (in `C:\Cluster-Package\DSC\`):
- `Deploy-DC.Completed.signal` — DC01
- `Deploy-Storage.Completed.signal` — Storage01
- `Deploy-Node01.Completed.signal` — Node01
- `Deploy-Node02.Completed.signal` — Node02
- `Deploy-Driver.Completed.signal` — Client01

### 4. Connect to VMs

Connect via **Azure Bastion** in the portal (no public IPs are exposed).

- Username: `CONTOSO\Administrator`
- Password: the password you provided during deployment

### 5. Configure the Failover Cluster

After all VMs finish configuration (all signal files present), connect to **Node01** via Bastion and follow the [Post-Deployment Cluster Setup](#post-deployment-cluster-setup) section below.

### Deployment Timeline

| What | Duration | Notes |
|------|----------|-------|
| Phase 1 (Network + DC + Storage VMs) | ~5 min | Bicep deployment |
| DC configuration | ~20-30 min | AD DS promotion, multiple restarts |
| Storage configuration | ~5-10 min | iSCSI target, virtual disks (parallel with DC) |
| Phase 2 (Nodes + Driver VMs) | ~10-15 min | Bicep deployment |
| Node configuration | ~15-25 min | Domain join, features, iSCSI connection |
| Driver configuration | ~10-15 min | Domain join, tools install |
| **Total (automated)** | **~30-40 min** | |
| Post-deployment cluster setup | ~15 min | Manual steps on Node01 |

## Deployment Strategy

The deployment uses a **two-phase Bicep approach** to ensure proper sequencing:

**Phase 1** (`phase1.bicep`) deploys:
- Virtual Network with dual subnets (External1, External2)
- Network Security Groups with required firewall rules (SMB, iSCSI, cluster communication)
- Azure Bastion for secure remote access
- DC01 — installs AD DS, creates `contoso.com` domain, configures DNS
- Storage01 — installs iSCSI Target Server, creates 4 virtual disks (NOT domain-joined)

**Phase 2** (`phase2.bicep`) deploys after the DC is ready:
- Node01 and Node02 — join domain, install Failover Clustering and File Server, connect to iSCSI storage
- Client01 — joins domain, installs test tools

The script waits between phases by polling the DC's signal file via `Invoke-AzVMRunCommand`.

## Resuming a Deployment

If Phase 1 completed successfully but Phase 2 failed (e.g., DC timeout, VM extension error, cluster node domain join failure), you can resume from Phase 2 without redeploying the network, DC, and storage:

```powershell
.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fileserver-cluster-test" `
    -AdminPassword $password `
    -SkipPhase1
```

When resuming, the script will:
1. **Verify DC readiness** — Checks the DC signal file before proceeding (skip with `-SkipDCReadyCheck` if you're certain the DC is ready)
2. **Retrieve Phase 1 outputs** — Automatically reads subnet IDs, DC IP, Storage IP, and domain info from the previous Phase 1 deployment
3. **Reuse the uploaded package** — Searches existing storage accounts in the resource group for a previously uploaded `Cluster-Package.zip` and generates a fresh SAS URL

You can also provide a package URL directly:
```powershell
.\deploy.ps1 ... -SkipPhase1 -ClusterPackageZipUrl "https://storage.blob.core.windows.net/..."
```

To deploy just Phase 1 (e.g., to verify DC setup before continuing):
```powershell
.\deploy.ps1 ... -SkipPhase2
```

## Deployment Parameters

### Script Parameters (command-line)

#### Required

| Parameter | Description |
|-----------|-------------|
| `SubscriptionId` | Azure subscription ID |
| `ResourceGroupName` | Target resource group name |
| `AdminPassword` | SecureString password for VM admin accounts |

> **Note:** The Azure region (location) is set in `parameters/phase1.bicepparam` via the Bicep `location` parameter, not as a script parameter.

#### Optional

| Parameter | Default | Description |
|-----------|---------|-------------|
| `Phase1ParametersFile` | `parameters/phase1.bicepparam` | Bicep parameter file for Phase 1 |
| `Phase2ParametersFile` | `parameters/phase2.bicepparam` | Bicep parameter file for Phase 2 |
| `ClusterPackagePath` | *(auto: `DSC/`)* | Local directory with configuration scripts |
| `ClusterPackageZip` | *(empty)* | Local zip file with configuration scripts |
| `ClusterPackageZipUrl` | *(empty)* | Pre-uploaded package URL (skips upload) |
| `DCReadyTimeoutMinutes` | `45` | How long to wait for DC configuration |
| `SkipPhase1` | `$false` | Skip Phase 1 and resume from Phase 2 |
| `SkipPhase2` | `$false` | Deploy Phase 1 only |
| `SkipDCReadyCheck` | `$false` | Skip DC readiness verification when resuming |
| `StorageAccountName` | *(auto-generated)* | Name of the Azure Storage Account for package upload |
| `ValidateOnly` | `$false` | Run template validation only (no deployment) |
| `SkipDiskEncryption` | `$false` | Skip Azure Disk Encryption on all VMs |

### Template Parameters (bicepparam files)

Edit these files to customize the environment. All template configuration lives here — the bicepparam files are the single source of truth.

**Phase 1** — [`parameters/phase1.bicepparam`](parameters/phase1.bicepparam):

| Parameter | Default | Description |
|-----------|---------|-------------|
| `location` | `West US 2` | Azure region |
| `environmentPrefix` | `fstest-cluster` | Prefix for resource names |
| `domainName` | `contoso.com` | Active Directory domain name |
| `domainNetBiosName` | `CONTOSO` | Domain NetBIOS name |
| `clusterName` | `Cluster01` | Failover cluster name |
| `scaleOutFSName` | `ScaleOutFS` | Scale-Out File Server name |
| `vnetAddressPrefix` | `192.168.0.0/16` | Virtual network address space |
| `dcExternal1Ip` | `192.168.1.10` | DC primary IP address |
| `storageExternal1Ip` | `192.168.1.50` | Storage server IP |
| `dcCustomImageId` | *(empty)* | Custom image for DC (overrides marketplace) |
| `storageCustomImageId` | *(empty)* | Custom image for Storage (overrides marketplace) |

**Phase 2** — [`parameters/phase2.bicepparam`](parameters/phase2.bicepparam):

| Parameter | Default | Description |
|-----------|---------|-------------|
| `driverOsType` | `Windows` | Driver OS: `Windows` or `Linux` |
| `node01External1Ip` | `192.168.1.11` | Node01 primary IP |
| `node02External1Ip` | `192.168.1.12` | Node02 primary IP |
| `driverExternal1Ip` | `192.168.1.111` | Driver computer IP |
| `driverCustomImageId` | *(empty)* | Custom image for driver (overrides marketplace) |
| `clusterNodeCustomImageId` | *(empty)* | Custom image for cluster nodes (overrides marketplace) |

## Post-Deployment Cluster Setup

After all VMs finish their background configuration (all signal files present), connect to **Node01** via Bastion and run these steps to form the failover cluster.

### Step 1: Initialize shared disks

```powershell
# View the iSCSI disks
Get-Disk | Where-Object {$_.BusType -eq "iSCSI"}

# Initialize all RAW iSCSI disks
$iscsiDisks = Get-Disk | Where-Object {$_.BusType -eq "iSCSI" -and $_.PartitionStyle -eq "RAW"}
$iscsiDisks | ForEach-Object { Initialize-Disk -Number $_.Number -PartitionStyle GPT }

# Create volumes — the 1GB disk is quorum, the 10GB disks are data
$quorumDisk = $iscsiDisks | Where-Object {$_.Size -eq 1GB}
$fsDisk01 = $iscsiDisks | Where-Object {$_.Size -eq 10GB} | Select-Object -First 1
$fsDisk02 = $iscsiDisks | Where-Object {$_.Size -eq 10GB} | Select-Object -Skip 1 -First 1

New-Partition -DiskNumber $quorumDisk.Number -UseMaximumSize |
    Format-Volume -FileSystem NTFS -NewFileSystemLabel "QuorumDisk" -Confirm:$false

New-Partition -DiskNumber $fsDisk01.Number -DriveLetter F -UseMaximumSize |
    Format-Volume -FileSystem NTFS -NewFileSystemLabel "FSDisk01" -Confirm:$false

New-Partition -DiskNumber $fsDisk02.Number -DriveLetter G -UseMaximumSize |
    Format-Volume -FileSystem NTFS -NewFileSystemLabel "FSDisk02" -Confirm:$false
```

### Step 2: Create the failover cluster

```powershell
# Validate cluster prerequisites
Test-Cluster -Node Node01, Node02

# Create the cluster (no storage yet — added in next step)
New-Cluster -Name Cluster01 -Node Node01, Node02 -StaticAddress 192.168.1.100 -NoStorage

# Add shared disks and configure quorum
Get-ClusterAvailableDisk | Add-ClusterDisk
Set-ClusterQuorum -NodeAndDiskMajority "Cluster Disk 1"
```

### Step 3: Create Scale-Out File Server and shares

```powershell
# Add Scale-Out File Server role
Add-ClusterScaleOutFileServerRole -Name ScaleOutFS

# Add storage to the role
Add-ClusterSharedVolume -Name "Cluster Disk 2"
Add-ClusterSharedVolume -Name "Cluster Disk 3"

# Create required SMB shares
New-Item -Path C:\ClusterStorage\Volume1\SMBClustered -ItemType Directory -Force
New-SmbShare -Name SMBClustered -Path C:\ClusterStorage\Volume1\SMBClustered `
    -FullAccess Everyone -ContinuouslyAvailable $true

New-Item -Path C:\ClusterStorage\Volume1\SMBClusteredEncrypted -ItemType Directory -Force
New-SmbShare -Name SMBClusteredEncrypted -Path C:\ClusterStorage\Volume1\SMBClusteredEncrypted `
    -FullAccess Everyone -EncryptData $true -ContinuouslyAvailable $true

New-Item -Path C:\ClusterStorage\Volume2\SMBClusteredForceLevel2 -ItemType Directory -Force
New-SmbShare -Name SMBClusteredForceLevel2 -Path C:\ClusterStorage\Volume2\SMBClusteredForceLevel2 `
    -FullAccess Everyone -ContinuouslyAvailable $true
Set-SmbShare -Name SMBClusteredForceLevel2 -ForceLevelIIOplock $true -Confirm:$false
```

## Domain Configuration

The deployment creates:

- **Domain**: `contoso.com` (configurable)
- **Domain Admin**: `CONTOSO\Administrator` (uses deployment password)
- **Domain User**: `CONTOSO\nonadmin` (password set by deployment configuration scripts)
- **Guest Account**: Enabled for testing

## Storage Configuration

Storage01 is configured with:

- **iSCSI Target**: `ClusterTarget` (configured in Config.json)
- **Allowed Initiators**: IQN:* (all initiators)
- **Virtual Disks**:
  - disk1.vhdx (10GB), disk2.vhdx (10GB), disk3.vhdx (10GB)
  - diskq.vhdx (1GB) — cluster quorum

## Multi-Platform and Custom Image Support

### Linux Driver

To deploy an Ubuntu Linux driver computer, edit `parameters/phase2.bicepparam`:
```bicep
param driverOsType = 'Linux'
param driverLinuxOsVersion = 'server'  // Ubuntu 24.04 LTS
```

When `driverOsType = 'Linux'`, the driver VM deploys Ubuntu 24.04 LTS and installs PowerShell Core (`pwsh`) to run the same configuration scripts used on Windows.

### Custom VM Images

To use custom Azure VM images instead of marketplace defaults, set the image resource IDs:
```bicep
// Phase 1 parameters
param dcCustomImageId = '/subscriptions/.../images/my-custom-dc'
param storageCustomImageId = '/subscriptions/.../images/my-custom-storage'

// Phase 2 parameters
param driverCustomImageId = '/subscriptions/.../images/my-custom-driver'
param clusterNodeCustomImageId = '/subscriptions/.../images/my-custom-node'
```

When a custom image ID is provided, it overrides the marketplace image for that VM. Leave empty (`''`) to use the default.

## Security Notes

> **Warning**: This environment is designed for testing purposes only:
> - All Windows Firewalls are **disabled**
> - Default passwords are used for test accounts
> - Network security groups allow broad access within the VNet
> - Storage01 is NOT domain-joined per test suite requirements
> - Not suitable for production use

## Troubleshooting

### Log Files

| VM | Log File | Contents |
|----|----------|----------|
| DC01 | `C:\Cluster-Package\DSC\Deploy-DC.log` | DC orchestrator |
| Storage01 | `C:\Cluster-Package\DSC\Deploy-Storage.log` | Storage/iSCSI orchestrator |
| Node01 | `C:\Cluster-Package\DSC\Deploy-Node01.log` | Cluster node 1 orchestrator |
| Node02 | `C:\Cluster-Package\DSC\Deploy-Node02.log` | Cluster node 2 orchestrator |
| Client01 | `C:\Cluster-Package\DSC\Deploy-Driver.log` | Driver orchestrator |
| All | `C:\*-extension-setup.log` | CustomScriptExtension bootstrap |

### Common Issues

1. **iSCSI connection failures**:
   - Verify Storage01 created virtual disks: check `C:\Cluster-Package\DSC\Deploy-Storage.log`
   - Verify iSCSI target is configured: `Get-IscsiServerTarget` on Storage01
   - Check initiator IPs: nodes must be able to reach Storage01 on port 3260

2. **Domain join failures**:
   - Check DNS configuration on cluster node VMs
   - Verify Domain Controller is fully ready (signal file exists)
   - Review VM extension logs in Azure portal

3. **Cluster creation failures**:
   - Ensure both nodes can see shared disks: `Get-Disk | Where BusType -eq iSCSI`
   - Run `Test-Cluster` to identify issues
   - Verify both nodes are domain-joined: `(Get-CimInstance Win32_ComputerSystem).PartOfDomain`

4. **DC timeout**:
   - Increase timeout: `-DCReadyTimeoutMinutes 60`
   - Check DC logs via Bastion: `C:\dc-extension-setup.log`
   - Resume Phase 2 once DC is ready: `.\deploy.ps1 ... -SkipPhase1`

5. **VM extension failures**:
   - Azure Portal: VM → Extensions → View detailed status
   - On the VM (Windows): `C:\*-extension-setup.log`
   - On the VM (Linux driver): `/var/log/cluster-driver-setup.log`
   - Fix issues, then resume: `.\deploy.ps1 ... -SkipPhase1`

6. **Template validation errors**:
   - Update Azure Bicep CLI: `az bicep upgrade`
   - Check parameter values in bicepparam files

### Manual Verification

Connect to each VM via Bastion and check:

```powershell
# Check signal file
Test-Path C:\Cluster-Package\DSC\Deploy-DC.Completed.signal        # DC01
Test-Path C:\Cluster-Package\DSC\Deploy-Storage.Completed.signal   # Storage01
Test-Path C:\Cluster-Package\DSC\Deploy-Node01.Completed.signal    # Node01
Test-Path C:\Cluster-Package\DSC\Deploy-Node02.Completed.signal    # Node02
Test-Path C:\Cluster-Package\DSC\Deploy-Driver.Completed.signal    # Client01

# Check deploy step progress
Get-ItemProperty HKLM:\SOFTWARE\ProtocolTestSuites
```

## Known Issues

1. **GeneralFS virtual IP not routable in Azure**: The GeneralFS clustered file server virtual IP is not directly routable in Azure networking. As a workaround, Client01 maps `GeneralFS` to Node01's IP address via the hosts file (`C:\Windows\System32\drivers\etc\hosts`).

2. **ksetup trust relationship**: The `ksetup /SetComputerPassword` command runs after domain join. AD account password synchronization is performed via `Set-ADAccountPassword` on the DC side.

3. **Auto-shutdown enabled by default**: All VMs are configured with auto-shutdown at 20:00 UTC. Control this with the `enableAutoShutdown` parameter in the Bicep templates.

4. **Azure Disk Encryption enabled by default**: All VMs have Azure Disk Encryption enabled, which requires Key Vault permissions. Use `-SkipDiskEncryption` to disable this if your subscription lacks Key Vault access or to speed up deployment.

## Key Differences from Domain Scenario

| Feature | Domain | Cluster |
|---------|--------|---------|
| VMs | 3 (DC + Driver + SUT) | 5 (DC + Storage + 2 Nodes + Driver) |
| Shared storage | None | 4 iSCSI virtual disks |
| Failover Clustering | No | Yes (Node01 + Node02) |
| Post-deploy manual steps | None | Cluster formation (~15 min) |
| Additional NSG rules | — | iSCSI (3260/tcp), cluster communication |

## File Structure

```
cluster-bicep/
├── phase1.bicep                     # Phase 1: Network + DC + Storage
├── phase2.bicep                     # Phase 2: Cluster Nodes + Driver
├── deploy.ps1                       # PowerShell deployment script (supports resume)
├── README.md                        # This file
├── modules/
│   ├── network.bicep              # Network infrastructure
│   ├── domain-controller.bicep    # Domain Controller VM
│   ├── storage-server.bicep       # Storage Server (iSCSI) VM
│   ├── cluster-nodes.bicep        # Cluster Node VMs (Node01, Node02)
│   └── driver-computer.bicep      # Driver Computer VM
├── DSC/
│   ├── Deploy-Storage.ps1          # Storage Server configuration script
│   ├── Deploy-Node01.ps1           # Cluster Node 1 configuration script
│   ├── Deploy-Node02.ps1           # Cluster Node 2 configuration script
│   └── Scripts/                    # Environment setup helpers
├── parameters/
│   ├── phase1.bicepparam           # Phase 1 parameters
│   └── phase2.bicepparam           # Phase 2 parameters
└── scripts/
    └── Verify-ClusterDeployment.ps1 # Post-deployment verification

../shared/                           # Components shared with Domain/Workgroup
├── Deploy-Helpers.psm1             # Azure connection, storage, quota validation
├── Generate-ConfigJson.ps1         # Config.json generation from bicepparam values
└── DSC/
    ├── Deploy-DC.ps1               # Domain Controller orchestrator
    ├── Deploy-Driver.ps1           # Driver computer orchestrator
    └── Scripts/                    # Shared utilities (tools install, test run, etc.)
```

## Based on FileServer User Guide

This deployment follows the specifications in:
- **FileServerUserGuide.md** — Sections 4.2 (Domain Network Environment) and 5.3.12-5.3.13 (Cluster Setup)
- **Storage Server Requirements** — Section 3.3.4 & 5.2.3
- **Cluster Configuration** — Sections 5.3.12 (SAN Storage) and 5.3.13 (Cluster Setup)
- **Network Architecture** — Dual NIC configuration with External1/External2
