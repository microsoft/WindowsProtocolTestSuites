# File Server Protocol Test Suite - Failover Cluster Environment Deployment

This folder contains Bicep templates and deployment scripts for creating a complete **failover cluster environment** for the File Server Protocol Test Suite on Azure.

## Architecture Overview

The cluster environment consists of:

- **Domain Controller (DC01)**: Windows Server with Active Directory Domain Services, DNS
- **Storage Server (Storage01)**: Windows Server with iSCSI Target Server (NOT domain-joined)
- **Cluster Node01**: Windows Server with Failover Clustering and File Server roles
- **Cluster Node02**: Windows Server with Failover Clustering and File Server roles
- **Driver Computer (Client01)**: Windows 11 Enterprise or Ubuntu Linux for running test cases
- **Network Infrastructure**: VNet with dual subnets, Azure Bastion for secure access
- **iSCSI Storage**: 4 virtual disks (disk1: 10GB, disk2: 10GB, disk3: 10GB, diskq: 1GB quorum)

The driver computer supports both Windows and Linux. Custom Azure VM images can be used for any VM by providing a resource ID.

## Deployment Strategy

The deployment follows a **multi-phase approach** to ensure proper sequencing:

### Phase 1: Network Infrastructure
- Virtual Network with dual subnets (External1, External2)
- Network Security Groups with required firewall rules (SMB, iSCSI, cluster communication)
- Azure Bastion for secure remote access

### Phase 2: Domain Controller Deployment
- Deploys DC01 with Windows Server
- Installs and configures Active Directory Domain Services
- Creates `contoso.com` domain forest
- Configures DNS server

### Phase 3: Storage Server Deployment (parallel with DC)
- Deploys Storage01 (NOT domain-joined per test suite requirements)
- Installs File Server and iSCSI Target Server roles
- Creates 4 iSCSI virtual disks:
  - **disk1** (10GB) - For file server storage
  - **disk2** (10GB) - For file server storage
  - **disk3** (10GB) - For file server storage
  - **diskq** (1GB) - For cluster quorum
- Configures iSCSI target for cluster nodes

### Phase 4: Cluster Nodes Deployment (after DC and Storage)
- Deploys Node01 and Node02 **after** DC and Storage are ready
- Automatically joins computers to the domain
- Installs Failover Clustering, File Server, and related features
- Connects to iSCSI storage automatically
- Configures DNS settings to point to Domain Controller

### Phase 5: Driver Computer Deployment (after DC)
- Deploys Client01 **after** DC is ready
- Automatically joins to the domain
- Configured for test execution

## Prerequisites

- Azure PowerShell module (`Install-Module Az`)
- Azure Bicep CLI (installed automatically by the script)
- Azure subscription with appropriate permissions
- Resource group deployment rights

## Quick Start

1. **Clone and navigate to the cluster-bicep folder**:
   ```powershell
   cd cluster-bicep
   ```

2. **Deploy the cluster environment**:
   ```powershell
$password = Read-Host -Prompt "Enter your secure password" -AsSecureString

.\deploy.ps1 `
  -SubscriptionId "your-subscription-id" `
  -ResourceGroupName "fileserver-cluster-test" `
  -AdminPassword $password
   ```

3. **Connect to VMs**: Use the Azure Bastion FQDN provided in the deployment outputs

4. **Configure the Failover Cluster**: Follow the post-deployment steps below

## Resuming a Deployment

If Phase 1 completed successfully but Phase 2 failed (e.g., DC timeout, VM extension error, cluster node domain join failure), you can resume from Phase 2 without redeploying the network, DC, and storage:

```powershell
$password = ConvertTo-SecureString "YourSecurePassword123!" -AsPlainText -Force

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

### Deploy Phase 1 Only

To deploy just the network, DC, and storage (e.g., to manually verify DC setup before continuing):

```powershell
.\deploy.ps1 ... -SkipPhase2
```

## Deployment Parameters

### Script Parameters

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
| `ClusterPackagePath` | `D:\ISOs\...\Cluster-Package` | Local directory with configuration scripts |
| `ClusterPackageZip` | `D:\ISOs\...\Cluster-Package.zip` | Local zip file with configuration scripts |
| `ClusterPackageZipUrl` | *(empty)* | Pre-uploaded package URL (skips upload) |
| `DCReadyTimeoutMinutes` | `45` | How long to wait for DC configuration |
| `SkipPhase1` | `$false` | Skip Phase 1 and resume from Phase 2 |
| `SkipPhase2` | `$false` | Deploy Phase 1 only |
| `SkipDCReadyCheck` | `$false` | Skip DC readiness verification when resuming |
| `StorageAccountName` | *(auto-generated)* | Name of the Azure Storage Account for package upload |
| `ValidateOnly` | `$false` | Run template validation only (no deployment) |
| `SkipDiskEncryption` | `$false` | Skip Azure Disk Encryption on all VMs |

### Template Parameters

Edit [parameters/phase1.bicepparam](parameters/phase1.bicepparam) and [parameters/phase2.bicepparam](parameters/phase2.bicepparam) to customize:

**Phase 1 Parameters** ([parameters/phase1.bicepparam](parameters/phase1.bicepparam)):

| Parameter | Default | Description |
|-----------|---------|-------------|
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

**Phase 2 Parameters** ([parameters/phase2.bicepparam](parameters/phase2.bicepparam)):

| Parameter | Default | Description |
|-----------|---------|-------------|
| `driverOsType` | `Windows` | Driver OS: `Windows` or `Linux` |
| `node01External1Ip` | `192.168.1.11` | Node01 primary IP |
| `node02External1Ip` | `192.168.1.12` | Node02 primary IP |
| `driverExternal1Ip` | `192.168.1.111` | Driver computer IP |
| `driverCustomImageId` | *(empty)* | Custom image for driver (overrides marketplace) |
| `clusterNodeCustomImageId` | *(empty)* | Custom image for cluster nodes (overrides marketplace) |

## Network Configuration

Following the FileServer Protocol Test Suite requirements:

```
External1 Network: 192.168.1.0/24
├── DC01:      192.168.1.10  (Domain Controller)
├── Node01:    192.168.1.11  (Cluster Node 1)
├── Node02:    192.168.1.12  (Cluster Node 2)
├── Storage01: 192.168.1.50 (iSCSI Storage - NOT domain-joined)
└── Client01:  192.168.1.111 (Driver)

External2 Network: 192.168.2.0/24
├── DC01:      192.168.2.10  (Domain Controller)
├── Node01:    192.168.2.11  (Cluster Node 1)
├── Node02:    192.168.2.12  (Cluster Node 2)
└── Client01:  192.168.2.111 (Driver)

Bastion Network: 192.168.0.0/26
└── Azure Bastion for secure access
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
  - disk1.vhdx (10GB)
  - disk2.vhdx (10GB)
  - disk3.vhdx (10GB)
  - diskq.vhdx (1GB)

## Monitoring Deployment Completion

⚠️ **IMPORTANT**: VM extensions report "Succeeded" immediately after starting configuration scripts, but the actual configuration continues in the background with automatic restarts. **This means Azure will show the deployment as "Succeeded" before VMs are fully configured.**

### Understanding Configuration Phases

Each VM runs multi-phase configuration scripts that:
1. Install Windows features
2. Restart automatically
3. Continue configuration after restart
4. Create a completion signal file when done

**Example:** Configure_DC.ps1 runs through 4 phases with 3 automatic restarts. The Azure deployment will show "Succeeded" after phase 1 starts, but AD DS installation may take 20-30 more minutes.

### Verify Configuration Completion

Use the provided verification script to check when all VMs have completed their configuration:

```powershell
.\scripts\Verify-ClusterDeployment.ps1 -ResourceGroupName "your-rg-name"
```

**What it checks:**
- ✅ DC01: `C:\Cluster-Package\DSC\Deploy-DC.Completed.signal`
- ✅ Storage01: `C:\Cluster-Package\DSC\Deploy-Storage.Completed.signal`
- ✅ Node01: `C:\Cluster-Package\DSC\Deploy-Node01.Completed.signal`
- ✅ Node02: `C:\Cluster-Package\DSC\Deploy-Node02.Completed.signal`
- ✅ Client01: `C:\Cluster-Package\DSC\Deploy-Driver.Completed.signal`

**Options:**
```powershell
# Wait up to 90 minutes (default: 60)
.\scripts\Verify-ClusterDeployment.ps1 -ResourceGroupName "your-rg" -TimeoutMinutes 90

# Check every 60 seconds (default: 30)
.\scripts\Verify-ClusterDeployment.ps1 -ResourceGroupName "your-rg" -PollIntervalSeconds 60
```

**Example Output:**
```
[15.2 min] Checking VM configuration status...
  Checking fstest-cluster-dc01 (Domain Controller)...
    ✅ COMPLETE - Signal file found
  Checking fstest-cluster-storage01 (Storage Server)...
    ✅ COMPLETE - Signal file found
  Checking fstest-cluster-node01 (Cluster Node 1)...
    ⏳ IN PROGRESS - Signal file not found yet
  ...

🎉 SUCCESS! All VMs have completed their configuration.
```

### Manual Verification

Connect to each VM via Bastion and check:

1. **View extension logs**:
   ```powershell
   # DC01
   Get-Content C:\dc-extension-setup.log -Tail 50

   # Storage01
   Get-Content C:\storage-extension-setup.log -Tail 50

   # Node01/Node02
   Get-Content C:\node01-extension-setup.log -Tail 50
   Get-Content C:\node02-extension-setup.log -Tail 50

   # Client01
   Get-Content C:\driver-extension-setup.log -Tail 50
   ```

2. **Check for signal files**:
   ```powershell
   Test-Path C:\Cluster-Package\DSC\Deploy-DC.Completed.signal          # DC01
   Test-Path C:\Cluster-Package\DSC\Deploy-Storage.Completed.signal     # Storage01
   Test-Path C:\Cluster-Package\DSC\Deploy-Node01.Completed.signal      # Node01
   Test-Path C:\Cluster-Package\DSC\Deploy-Node02.Completed.signal      # Node02
   Test-Path C:\Cluster-Package\DSC\Deploy-Driver.Completed.signal      # Client01
   ```

**Expected Timeline:**
- **Storage01**: ~5-10 minutes (fastest, no domain join)
- **DC01**: ~20-30 minutes (AD DS installation, multiple restarts)
- **Node01/Node02**: ~15-25 minutes (domain join, feature installation, iSCSI)
- **Client01**: ~10-15 minutes (domain join, basic setup)

## Post-Deployment: Cluster Configuration

After deployment completes, follow these steps to configure the failover cluster:

### Step 1: Connect to Node01 via Bastion

1. In Azure Portal, navigate to Node01 VM
2. Click "Connect" → "Bastion"
3. Enter credentials:
   - Username: `CONTOSO\Administrator`
   - Password: (the password you provided during deployment)

### Step 2: Initialize Shared Disks

On Node01, open PowerShell as Administrator:

```powershell
# View the iSCSI disks
Get-Disk | Where-Object {$_.BusType -eq "iSCSI"}

# Initialize the iSCSI disks
$iscsiDisks = Get-Disk | Where-Object {$_.BusType -eq "iSCSI" -and $_.PartitionStyle -eq "RAW"}
$iscsiDisks | ForEach-Object { Initialize-Disk -Number $_.Number -PartitionStyle GPT }

# Create volumes based on disk size
$quorumDisk = $iscsiDisks | Where-Object {$_.Size -eq 1GB}
$fsDisk01 = $iscsiDisks | Where-Object {$_.Size -eq 10GB} | Select-Object -First 1
$fsDisk02 = $iscsiDisks | Where-Object {$_.Size -eq 10GB} | Select-Object -Skip 1 -First 1

# QuorumDisk (1GB) - No drive letter needed for quorum
New-Partition -DiskNumber $quorumDisk.Number -UseMaximumSize | Format-Volume -FileSystem NTFS -NewFileSystemLabel "QuorumDisk" -Confirm:$false

# FSDisk01 (10GB)
New-Partition -DiskNumber $fsDisk01.Number -DriveLetter F -UseMaximumSize | Format-Volume -FileSystem NTFS -NewFileSystemLabel "FSDisk01" -Confirm:$false

# FSDisk02 (10GB)
New-Partition -DiskNumber $fsDisk02.Number -DriveLetter G -UseMaximumSize | Format-Volume -FileSystem NTFS -NewFileSystemLabel "FSDisk02" -Confirm:$false
```

### Step 3: Validate Cluster Configuration

```powershell
# Run cluster validation
Test-Cluster -Node Node01, Node02
```

### Step 4: Create Failover Cluster

On Node01, open Failover Cluster Manager or run PowerShell:

```powershell
# Create the cluster
New-Cluster -Name Cluster01 -Node Node01, Node02 -StaticAddress 192.168.1.100 -NoStorage

# Configure cluster quorum
Set-ClusterQuorum -NodeAndDiskMajority "Cluster Disk 1"

# Add shared storage to cluster
Get-ClusterAvailableDisk | Add-ClusterDisk
```

### Step 5: Create Scale-Out File Server

```powershell
# Add Scale-Out File Server role
Add-ClusterScaleOutFileServerRole -Name ScaleOutFS

# Add storage to the role
Add-ClusterSharedVolume -Name "Cluster Disk 2"
Add-ClusterSharedVolume -Name "Cluster Disk 3"
```

### Step 6: Create Required Shares

```powershell
# Create SMBClustered share
New-Item -Path C:\ClusterStorage\Volume1\SMBClustered -ItemType Directory -Force
New-SmbShare -Name SMBClustered -Path C:\ClusterStorage\Volume1\SMBClustered -FullAccess Everyone -ContinuouslyAvailable $true

# Create SMBClusteredEncrypted share
New-Item -Path C:\ClusterStorage\Volume1\SMBClusteredEncrypted -ItemType Directory -Force
New-SmbShare -Name SMBClusteredEncrypted -Path C:\ClusterStorage\Volume1\SMBClusteredEncrypted -FullAccess Everyone -EncryptData $true -ContinuouslyAvailable $true

# Create SMBClusteredForceLevel2 share
New-Item -Path C:\ClusterStorage\Volume2\SMBClusteredForceLevel2 -ItemType Directory -Force
New-SmbShare -Name SMBClusteredForceLevel2 -Path C:\ClusterStorage\Volume2\SMBClusteredForceLevel2 -FullAccess Everyone -ContinuouslyAvailable $true
Set-SmbShare -Name SMBClusteredForceLevel2 -ForceLevelIIOplock $true -Confirm:$false
```

## Security Notes

⚠️ **This environment is designed for testing purposes only:**

- All Windows Firewalls are **disabled**
- Default passwords are used for test accounts
- Network security groups allow broad access within the VNet
- Storage01 is NOT domain-joined per test suite requirements
- Not suitable for production use

## Known Issues

1. **GeneralFS virtual IP not routable in Azure**: The GeneralFS clustered file server virtual IP is not directly routable in Azure networking. As a workaround, Client01 maps `GeneralFS` to Node01's IP address via the hosts file (`C:\Windows\System32\drivers\etc\hosts`).

2. **ksetup trust relationship**: The `ksetup /SetComputerPassword` command runs after domain join. AD account password synchronization is performed via `Set-ADAccountPassword` on the DC side.

3. **Auto-shutdown enabled by default**: All VMs are configured with auto-shutdown at 20:00 UTC by default. Control this with the `enableAutoShutdown` parameter in the Bicep templates.

4. **Azure Disk Encryption enabled by default**: All VMs have Azure Disk Encryption enabled, which requires Key Vault permissions in the subscription. Use the `-SkipDiskEncryption` script parameter to disable this if your subscription lacks Key Vault access or if encryption is not needed.

## Deployment Time

- **Total deployment time**: ~30-40 minutes
- **Phase 1** (Network): ~5 minutes
- **Phase 2** (Domain Controller): ~10-15 minutes (includes domain promotion)
- **Phase 3** (Storage Server): ~10 minutes (includes iSCSI configuration)
- **Phase 4** (Cluster Nodes): ~10-15 minutes (includes domain join and iSCSI connection)
- **Phase 5** (Driver Computer): ~5 minutes (includes domain join)

**Note**: Cluster configuration (post-deployment) requires an additional 15-30 minutes of manual setup.

## Troubleshooting

### Common Issues

1. **iSCSI connection failures**:
   - Verify Storage01 has created virtual disks successfully
   - Check iSCSI target is configured with correct initiator IPs
   - Review Storage01 extension logs in Azure portal

2. **Domain join failures**:
   - Check DNS configuration on cluster node VMs
   - Verify Domain Controller is fully ready
   - Review VM extension logs in Azure portal

3. **Cluster creation failures**:
   - Ensure both nodes can see shared disks
   - Run `Test-Cluster` to identify issues
   - Verify both nodes are domain-joined

4. **Template validation errors**:
   - Update Azure Bicep CLI: `az bicep upgrade`
   - Check parameter values in `.bicepparam` file

5. **DC timeout**:
   - Increase timeout: `-DCReadyTimeoutMinutes 60`
   - Check DC logs via Bastion: `C:\dc-extension-setup.log`
   - Resume Phase 2 once DC is ready: `.\deploy.ps1 ... -SkipPhase1`

6. **VM extension failures (Configure_SUT / Configure_Driver / Configure_Node)**:
   - Check extension logs via Azure Portal: VM → Extensions
   - On the VM (Windows): `C:\node01-extension-setup.log`, `C:\driver-extension-setup.log`
   - On the VM (Linux driver): `/var/log/cluster-driver-setup.log`
   - Fix issues, then resume: `.\deploy.ps1 ... -SkipPhase1`

7. **Resource conflicts**:
   - Ensure resource group name is unique
   - Check for existing resources with same names

### Monitoring Deployment

View detailed logs in Azure portal:
- Resource Group → Deployments
- Virtual Machines → Extensions (for configuration scripts)
- Activity Log for deployment progress

### Viewing Extension Logs

Connect to each VM via Bastion and check the **extension-level logs** (created by the VM extension entry point):
- DC01: `C:\dc-extension-setup.log`
- Storage01: `C:\storage-extension-setup.log`
- Node01: `C:\node01-extension-setup.log`
- Node02: `C:\node02-extension-setup.log`
- Client01: `C:\driver-extension-setup.log`

**DSC-level logs** (created by the Deploy-*.ps1 scripts inside the Cluster-Package):
- DC01: `C:\Cluster-Package\DSC\Deploy-DC.log`
- Storage01: `C:\Cluster-Package\DSC\Deploy-Storage.log`
- Node01: `C:\Cluster-Package\DSC\Deploy-Node01.log`
- Node02: `C:\Cluster-Package\DSC\Deploy-Node02.log`
- Client01: `C:\Cluster-Package\DSC\Deploy-Driver.log`

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
│   ├── Deploy-DC.ps1               # Domain Controller configuration script
│   ├── Deploy-Storage.ps1          # Storage Server configuration script
│   ├── Deploy-Node01.ps1           # Cluster Node 1 configuration script
│   ├── Deploy-Node02.ps1           # Cluster Node 2 configuration script
│   └── Deploy-Driver.ps1           # Driver computer configuration script
├── parameters/
│   ├── phase1.bicepparam           # Phase 1 parameters
│   └── phase2.bicepparam           # Phase 2 parameters
└── scripts/
    └── Verify-ClusterDeployment.ps1 # Post-deployment verification
```

## Shared Components

This deployment uses shared components from `../shared/`:
- **Deploy-Helpers.psm1** — Azure connection, storage, password handling, DC readiness polling
- **Generate-ConfigJson.ps1** — Generates Config.json dynamically for both Domain and Cluster scenarios with actual deployment IPs

## Based on FileServer User Guide

This deployment follows the specifications in:
- **FileServerUserGuide.md** - Sections 4.2 (Domain Network Environment) and 5.3.12-5.3.13 (Cluster Setup)
- **Storage Server Requirements** - Section 3.3.4 & 5.2.3
- **Cluster Configuration** - Sections 5.3.12 (SAN Storage) and 5.3.13 (Cluster Setup)
- **Network Architecture** - Dual NIC configuration with External1/External2

## Multi-Platform and Custom Image Support

### Linux Driver

To deploy an Ubuntu Linux driver computer, edit `parameters/phase2.bicepparam`:
```bicep
param driverOsType = 'Linux'
param driverLinuxOsVersion = 'server'  // Ubuntu 24.04 LTS
```

When `driverOsType = 'Linux'`, the driver VM deploys Ubuntu 24.04 LTS and installs PowerShell Core (`pwsh`) to run the same `Configure_Driver.ps1` scripts used on Windows.

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

## Key Differences from Domain Scenario

1. **Additional VM**: Storage01 (iSCSI Target Server) - NOT domain-joined
2. **Two Cluster Nodes**: Node01 and Node02 instead of single SUT
3. **iSCSI Storage**: Automatic provisioning of shared storage for cluster
4. **Cluster Features**: Additional roles and features installed on cluster nodes
5. **Network Rules**: Additional NSG rules for iSCSI (port 3260) and cluster communication

## Support

For issues with the File Server Protocol Test Suite itself, refer to:
- [File Server Test Suite Documentation](../docs/FileServerUserGuide.md)
- Original test suite setup instructions for manual configuration details
- [Domain Deployment (for comparison)](../domain-bicep/README.md)
