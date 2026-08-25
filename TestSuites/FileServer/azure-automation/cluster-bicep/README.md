# File Server Protocol Test Suite - Failover Cluster Environment Deployment

This folder contains Bicep templates and deployment scripts for creating a complete **failover cluster environment** for the File Server Protocol Test Suite on Azure.

> **Choosing a scenario?** See the [top-level azure-automation README](../README.md) for a comparison of Domain vs. Cluster vs. Workgroup.

## One-Click Deploy ("Deploy to Azure" button)

[`main.bicep`](main.bicep), compiled to [`azuredeploy.json`](azuredeploy.json),
provides the single-template Portal deployment. Enter an admin password and the
template deploys both infrastructure phases, configures all five VMs, forms the
cluster, and waits for live Cluster and Driver readiness.

The template preserves the phased safety gates by using managed deployment
scripts. It applies optional Azure Disk Encryption before each group of VM
configuration extensions, waits for verified DC and Storage completion before
deploying the domain members, and reports success only after Node01, Node02, and
the Driver pass their readiness checks.

The Portal exposes curated VM-size dropdowns with burstable defaults for broad
regional availability and lower onboarding cost: `Standard_B4ms` for DC,
Storage, and Driver, and `Standard_B8ms` for each Cluster node. Approved D/F
series alternatives are available when the defaults are constrained or higher
sustained throughput is required. Long test runs may still be slower than the
compute-oriented defaults used by `deploy.ps1`.

The template's public package contains no credentials. It validates the package
manifest before applying the generated topology override, then injects the
deployment password from encrypted Custom Script Extension
`protectedSettings`.

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
│  │                │ │  Cluster01     .100│←→──── .100       │      │
│  │                │ │  GeneralFS     .200│←→──── .200       │      │
│  │                │ │  Standard ILB VIPs │ │                 │      │
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

The node subnets use a NAT Gateway for deterministic outbound downloads. A
Standard internal load balancer supplies Cluster01 and GeneralFS frontends on
both subnets. Floating-IP rules and four health probes follow clustered role
ownership, so the Driver resolves virtual names through DNS rather than pinning
them to Node01.

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

`enableTestAutoRun` in `parameters/phase2.bicepparam` defaults to `true`. Set it
to `false` to configure the complete cluster without scheduling tests on
Client01.

### 3. Wait for deployment to complete

The script deploys in two Bicep phases with progress output:

1. **Phase 1** (~5 min): Network + DC + Storage VMs
2. **DC readiness poll** (~15-25 min): Script waits for AD DS promotion to finish
3. **Phase 2** (~10-15 min): Cluster Nodes + Driver VM

After Phase 2, the command remains attached while VMs join the domain, connect iSCSI, form the cluster, and create clustered roles/shares. With `enableTestAutoRun = true`, it also runs automatic tests and requires a terminal test summary. With autorun disabled, it completes after all five role signals. For an optional ad-hoc recheck:

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

- Username: `CONTOSO\testadmin`
- Password: the password you provided during deployment

### 5. Inspect the Failover Cluster

Node01 forms the cluster and creates GeneralFS, ScaleoutFS, InfraFS, disks, and shares automatically. Connect through Bastion only for inspection or troubleshooting.

### Deployment Timeline

| What | Duration | Notes |
|------|----------|-------|
| Phase 1 (Network + DC + Storage VMs) | ~5 min | Bicep deployment |
| DC configuration | ~20-30 min | AD DS promotion, multiple restarts |
| Storage configuration | ~5-10 min | iSCSI target, virtual disks (parallel with DC) |
| Phase 2 (Nodes + Driver VMs) | ~10-15 min | Bicep deployment |
| Node configuration | ~15-25 min | Domain join, features, iSCSI connection |
| Driver configuration | ~10-15 min | Domain join, tools install |
| Automatic test execution | Varies | Each invocation is bounded to 60 min; complete-plan wait defaults to 360 min |
| **Total (automated)** | **Environment-dependent** | Includes cluster formation and terminal test verification |

## Deployment Strategy

The deployment uses a **two-phase Bicep approach** to ensure proper sequencing:

**Phase 1** (`phase1.bicep`) deploys:
- Virtual Network with dual subnets (External1, External2)
- NAT Gateway for Cluster node outbound connectivity
- Network Security Groups with required firewall rules (SMB, iSCSI, cluster communication)
- Azure Bastion for secure remote access
- DC01 — installs AD DS, creates `contoso.com` domain, configures DNS
- Storage01 — installs iSCSI Target Server, creates 4 virtual disks (NOT domain-joined)

**Phase 2** (`phase2.bicep`) deploys after the DC is ready:
- Standard internal load balancer with Cluster01 and GeneralFS frontends on both subnets
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
3. **Refresh the package** — Builds a fresh private package when local sources are available, otherwise finds an existing `Cluster-Package.zip` and generates a fresh signed URL
4. **Preserve live identities** — Keeps physical-member and cluster endpoint AD objects while their VMs/cluster survive
5. **Verify by phase** — Accepts existing DC/Storage signals while requiring fresh Node01, Node02, Driver, and test signals

If the local shell or workstation restarts, VM-side setup and tests continue, but `deploy.ps1` does not automatically resume its local checkpoint. Rerun with `-SkipPhase1`. An authenticated Azure CLI session can restore an expired Az PowerShell context.

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
| `TestTimeoutMinutes` | `360` | Maximum wait for the complete automatic test plan and finalization |
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
| `clusterExternal1Ip` / `clusterExternal2Ip` | `.100` on each subnet | Cluster core load-balancer frontends |
| `generalFSExternal1Ip` / `generalFSExternal2Ip` | `.200` on each subnet | GeneralFS load-balancer frontends |
| `clusterExternal1ProbePort` / `clusterExternal2ProbePort` | `59998` / `59999` | Cluster core health probes |
| `generalFSExternal1ProbePort` / `generalFSExternal2ProbePort` | `60000` / `60001` | GeneralFS health probes |
| `driverCustomImageId` | *(empty)* | Custom image for driver (overrides marketplace) |
| `clusterNodeCustomImageId` | *(empty)* | Custom image for cluster nodes (overrides marketplace) |

## Automated Cluster Setup Verification

Node01 performs disk preparation, cluster creation, quorum setup, clustered role creation, and share creation. These non-destructive commands are useful for verification:

```powershell
Get-ClusterNode
Get-ClusterGroup
Get-ClusterSharedVolume
Get-SmbShare -ScopeName GeneralFS
Get-SmbShare -ScopeName ScaleoutFS
```

Do not rerun `New-Cluster`, disk initialization, or role-creation commands against an existing deployment. Resume with `-SkipPhase1` so the orchestrators reconcile state safely.

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

1. **Cluster virtual IPs require the Standard internal load balancer**: The
   Cluster01 and GeneralFS addresses use floating-IP load-balancer rules and
   probe ports. Do not replace them with direct hosts-file mappings to a
   physical node.

2. **ksetup trust relationship**: The `ksetup /SetComputerPassword` command runs after domain join. AD account password synchronization is performed via `Set-ADAccountPassword` on the DC side.

3. **Auto-shutdown enabled by default**: The deploy script removes schedules before setup/tests and restores them only after terminal test finalization and optional encryption. Known terminal test classifications are reported without failing deployment orchestration; missing output, failed upload/readiness, and post-test infrastructure failures remain fatal. Control the final schedule with `enableAutoShutdown` and `autoShutdownTime`.

4. **Azure Disk Encryption enabled by default**: ADE is applied only after cluster and automatic tests reach terminal finalization, then VM responsiveness is checked. Use `-SkipDiskEncryption` if your subscription lacks Key Vault access or to shorten deployment.

## Key Differences from Domain Scenario

| Feature | Domain | Cluster |
|---------|--------|---------|
| VMs | 3 (DC + Driver + SUT) | 5 (DC + Storage + 2 Nodes + Driver) |
| Shared storage | None | 4 iSCSI virtual disks |
| Failover Clustering | No | Yes (Node01 + Node02) |
| Post-deploy manual steps | None | None (formation is automated) |
| Additional NSG rules | — | iSCSI (3260/tcp), cluster communication |

## File Structure

```
cluster-bicep/
├── main.bicep                       # One-click, readiness-gated deployment
├── azuredeploy.json                 # Compiled one-click ARM template
├── phase1.bicep                     # Phase 1: Network + DC + Storage
├── phase1.json                      # Compiled Phase 1 ARM template
├── phase2.bicep                     # Phase 2: Cluster Nodes + Driver
├── phase2.json                      # Compiled Phase 2 ARM template
├── deploy.ps1                       # PowerShell deployment script (supports resume)
├── README.md                        # This file
├── modules/
│   ├── network.bicep              # Network infrastructure
│   ├── domain-controller.bicep    # Domain Controller VM
│   ├── storage-server.bicep       # Storage Server (iSCSI) VM
│   ├── cluster-nodes.bicep        # Cluster Node VMs (Node01, Node02)
│   ├── driver-computer.bicep      # Driver Computer VM
│   ├── service-extensions.bicep   # DC + Storage one-click configuration
│   └── computer-extensions.bicep  # Nodes + Driver one-click configuration
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
