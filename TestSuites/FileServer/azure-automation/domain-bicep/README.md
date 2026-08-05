# File Server Protocol Test Suite - Domain Environment Deployment

This folder contains Bicep templates and deployment scripts for creating a complete domain environment for the File Server Protocol Test Suite on Azure.

> **Choosing a scenario?** See the [top-level azure-automation README](../README.md) for a comparison of Domain vs. Cluster vs. Workgroup.

## One-Click Deploy ("Deploy to Azure" button)

For a demo/onboarding environment with **defaults**, deploy straight from the Azure Portal — no local clone, no PowerShell, no `deploy.ps1`:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FWindowsProtocolTestSuites%2Ffileserver-domain-deploy-button-v1%2FTestSuites%2FFileServer%2Fazure-automation%2Fdomain-bicep%2Fazuredeploy.json)

> The button points at [`azuredeploy.json`](azuredeploy.json) served from `raw.githubusercontent.com` at the **`fileserver-domain-deploy-button-v1`** tag, and the template pulls its DSC package from the GitHub **Release** asset at the same tag. It goes live once a maintainer cuts that release (see [Publishing the public package](#publishing-the-public-package-deploy-to-azure-button)).

The Portal renders a form from [`main.bicep`](main.bicep) (compiled to [`azuredeploy.json`](azuredeploy.json)) — the single-template equivalent of the two-phase `deploy.ps1` flow. Enter an **admin password** and deploy; the DC, SUT, and Driver come up, the members domain-join, and tests run automatically.

**How the two phases collapse into one click.** `deploy.ps1` deploys the DC, waits for AD DS promotion, then deploys the members. The button can't run that imperative wait, so `main.bicep` handles it **declaratively**: the members' module `dependsOn` the DC, their NICs use the DC as DNS, and the on-VM domain join ([`../shared/DSC/Scripts/domainjoin.ps1`](../shared/DSC/Scripts/domainjoin.ps1)) already **retries DNS/DC reachability and `Add-Computer` with exponential backoff** — so the members wait out DC promotion on their own.

**How credentials stay safe.** The button consumes a **public**, pre-built DSC package whose `Config.json` ships with a placeholder token (`#{ADMIN_PASSWORD}#`) instead of a password. At deploy time each VM's Custom Script Extension — from the extension's *encrypted* `protectedSettings` — injects the real admin password (base64-encoded, then JSON-escaped) into `Config.json` via [`../shared/DSC/Scripts/Set-ConfigCredential.ps1`](../shared/DSC/Scripts/Set-ConfigCredential.ps1) before the DC/member deploy scripts run. The credential-bearing bootstrap runs in a separate PowerShell process so transcript headers contain only the script path, then deletes itself on success or failure.

**Scope & limitations (defaults only):**
- The baked `Config.json` is valid for the **default IP topology and domain** (`contoso.com` / `CONTOSO`) only. Changing IP/domain parameters in the form will not update the peer values inside the package. For custom topologies, use `deploy.ps1` (which rebuilds the package).
- The package embeds a **fixed test-suite drop** (a snapshot) — appropriate for demo/onboarding, not for testing an arbitrary build.
- Marketplace **image terms** for the SUT/Driver/DC images may need to be accepted once per subscription (see the [top-level README](../README.md)).
- **VM sizes default to burstable (B-series)** (`Standard_B4ms`) for broad regional availability. If a size is capacity-constrained, pick a different **Region** (the template deploys into the resource group's region) or change the VM sizes in the form. Unlike `deploy.ps1`, the button **cannot auto-retry across SKUs**.
- **Disk encryption is off by default for the button** (`enableDiskEncryption=false`). Azure Disk Encryption (ADE) is applied by `deploy.ps1` as a *post-deploy* step, which the Portal button cannot run — so for the button the Key Vault would be created but never used. Managed disks are platform-encrypted at rest regardless. If you specifically want ADE and your subscription has the required Key Vault permissions, set `enableDiskEncryption` to **true** in the form.

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
│  │ │   Bastion    │ │ │    DC01      │←→───────────────┤    │
│  │ │   (Access)   │ │ │   .1.10      │ │    .2.10      │    │
│  │ └──────────────┘ │ └──────────────┘ │               │    │
│  │                  │ ┌──────────────┐ │               │    │
│  │                  │ │   Node01     │←→───────────────┤    │
│  │                  │ │  (SUT) .1.11 │ │    .2.11      │    │
│  │                  │ └──────────────┘ │               │    │
│  │                  │ ┌──────────────┐ │               │    │
│  │                  │ │  Client01    │←→───────────────┤    │
│  │                  │ │(Driver).1.111│ │   .2.111      │    │
│  │                  │ └──────────────┘ │               │    │
│  └──────────────────┴──────────────────┴───────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

- **Domain Controller (DC01)**: Windows Server with Active Directory Domain Services, DNS, and Routing
- **SUT Computer (Node01)**: Windows Server as the System Under Test with File Server role
- **Driver Computer (Client01)**: Windows 11 Enterprise or Ubuntu Linux for running test cases

The driver computer supports both Windows and Linux. Custom Azure VM images can be used for any VM by providing a resource ID.

## Prerequisites

- Azure PowerShell modules: `Az.Accounts`, `Az.Resources`, `Az.Storage`, `Az.Compute`
- Azure subscription with permissions to create resource groups, VMs, storage accounts, and Key Vaults
- Bicep CLI (installed automatically by the script if missing)

## Quick Start

### 1. Navigate to the domain-bicep folder

```powershell
cd domain-bicep
```

### 2. Deploy the full domain environment

```powershell
$password = Read-Host -Prompt "Enter the admin password" -AsSecureString

.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fileserver-domain-test" `
    -AdminPassword $password
```

Location, VM sizes, OS versions, and all other settings are read from the bicepparam files.

### 3. Wait for deployment to complete

The script overlaps independent work while preserving the domain-readiness gate:

1. **Phase 1**: Core network, Bastion, and Domain Controller deployment. Bastion no longer blocks the DC.
2. **Phase 2A**: Driver + SUT infrastructure provisions while AD DS promotion continues.
3. **DC readiness poll**: The script waits for `Deploy-DC.Completed.signal`.
4. **Phase 2B**: Driver + SUT guest extensions start only after the DC is ready.
5. **Disk encryption**: Driver and SUT encryption runs concurrently; DC encryption remains ordered before member domain joins because ADE may reboot the DC.

After Phase 2 finishes, VMs continue configuring in the background (domain join, feature installation, test environment setup). Azure Portal will show "Succeeded" before this completes.

**Check completion** by looking for signal files:
```powershell
# Check all VMs at once (replace with your resource group name)
$rg = "fileserver-domain-test"
foreach ($vm in (Get-AzVM -ResourceGroupName $rg)) {
    $result = Invoke-AzVMRunCommand -ResourceGroupName $rg -VMName $vm.Name `
        -CommandId 'RunPowerShellScript' `
        -ScriptString "Get-ChildItem C:\Domain-Package\DSC\*.signal -ErrorAction SilentlyContinue | Select -ExpandProperty Name"
    Write-Output "$($vm.Name): $($result.Value[0].Message)"
}
```

Expected signal files when complete:
- DC01: `Deploy-DC.Completed.signal`
- Node01: `Deploy-SUT.Completed.signal`
- Client01: `Deploy-Driver.Completed.signal`

### 4. Connect and use

Connect to any VM via **Azure Bastion** in the portal (no public IPs are exposed).

- For domain-joined VMs (e.g., DC01, Node01):
  - Username: `CONTOSO\testadmin`
  - Password: the password you provided during deployment

- For non-domain-joined VMs (e.g., Linux Driver):
  - Use the local admin credentials set during deployment

### Deployment Timeline

| What | Duration | Notes |
|------|----------|-------|
| Phase 1 (core network + DC VM + Bastion) | ~5 min | DC and Bastion start together after core networking |
| DC configuration + Phase 2A member infrastructure | ~15-25 min | AD DS promotion overlaps Driver/SUT provisioning |
| Phase 2B member guest configuration | ~1 min | Extensions start after the DC readiness signal |
| SUT configuration | ~10-15 min | Domain join, File Server role, shares |
| Driver configuration | ~10-15 min | Domain join, tools install; DSC re-applies only when drifted |
| **Total** | **~25-35 min** | Actual time varies by image, region, and AD promotion |

## Resuming a Deployment

If Phase 1 completed but Phase 2 failed, resume without redeploying the network and DC:

```powershell
.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fileserver-domain-test" `
    -AdminPassword $password `
    -SkipPhase1
```

When resuming, the script will:
1. **Verify DC readiness** — checks the DC signal file before proceeding (skip with `-SkipDCReadyCheck` if you're certain the DC is ready)
2. **Retrieve Phase 1 outputs** — reads subnet IDs and DC IP from the previous Phase 1 deployment
3. **Reuse the uploaded package** — searches existing storage accounts in the resource group for a previously uploaded `Domain-Package.zip` and generates a fresh SAS URL

You can also provide a package URL directly:
```powershell
.\deploy.ps1 ... -SkipPhase1 -DscPackageZipUrl "https://storage.blob.core.windows.net/..."
```

To deploy just the network and DC (e.g., to verify DC setup before continuing):
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

#### Optional

| Parameter | Default | Description |
|-----------|---------|-------------|
| `Phase1ParametersFile` | `parameters/phase1.bicepparam` | Bicep parameter file for Phase 1 |
| `Phase2ParametersFile` | `parameters/phase2.bicepparam` | Bicep parameter file for Phase 2 |
| `DscFolderPath` | `DSC` | Local DSC folder to package and upload |
| `DscPackageZipUrl` | *(empty)* | Pre-uploaded package URL (skips packaging/upload) |
| `StorageAccountName` | *(auto-generated)* | Name of the storage account for DSC package upload |
| `DCReadyTimeoutMinutes` | `45` | How long to wait for DC configuration |
| `SkipPhase1` | `$false` | Skip Phase 1 and resume from Phase 2 |
| `SkipPhase2` | `$false` | Deploy Phase 1 only |
| `SkipDCReadyCheck` | `$false` | Skip DC readiness verification when resuming |
| `ValidateOnly` | `$false` | Run pre-flight validation only without deploying resources |
| `SkipDiskEncryption` | `$false` | Skip Azure Disk Encryption on deployed VMs |

### Template Parameters (bicepparam files)

Edit these files to customize the environment. All template configuration lives here — the bicepparam files are the single source of truth.

**Phase 1** — [`parameters/phase1.bicepparam`](parameters/phase1.bicepparam):

| Parameter | Default | Description |
|-----------|---------|-------------|
| `location` | `West US 2` | Azure region |
| `environmentPrefix` | `fstest-domain` | Prefix for resource names |
| `domainName` | `contoso.com` | Active Directory domain name |
| `domainNetBiosName` | `CONTOSO` | Domain NetBIOS name |
| `vnetAddressPrefix` | `192.168.0.0/16` | Virtual network address space |
| `dcVmSize` | `Standard_D2s_v5` | DC VM size (auto-fallback if unavailable) |
| `dcOsVersion` | `2025-datacenter-azure-edition` | DC OS version |
| `dcExternal1Ip` | `192.168.1.10` | DC primary IP address |
| `dcCustomImageId` | *(empty)* | Custom image for DC (overrides marketplace) |

**Phase 2** — [`parameters/phase2.bicepparam`](parameters/phase2.bicepparam):

| Parameter | Default | Description |
|-----------|---------|-------------|
| `driverOsType` | `Windows` | Driver OS: `Windows` or `Linux` |
| `driverVmSize` | `Standard_F4as_v6` | Driver VM size (auto-fallback) |
| `sutVmSize` | `Standard_D8ls_v5` | SUT VM size (auto-fallback) |
| `driverExternal1Ip` | `192.168.1.111` | Driver computer IP |
| `sutExternal1Ip` | `192.168.1.11` | SUT computer IP |
| `driverCustomImageId` | *(empty)* | Custom image for driver |
| `sutCustomImageId` | *(empty)* | Custom image for SUT |

> **Note:** Additional parameters such as `adminUsername`, `enableAutoShutdown`, `autoShutdownTime`, and `enableDiskEncryption` are available. See the bicepparam files for the full list.

## Domain Configuration

The deployment creates:

- **Domain**: `contoso.com` (configurable)
- **Domain Admin**: `CONTOSO\testadmin` (uses deployment password)
- **Test accounts**: Created by `Create-TestAccount.ps1` (non-admin users, groups)
- **Guest Account**: Enabled for testing
- **CBAC**: Claim types, resource properties, central access rules/policies
- **GPO**: Claims-based access control policy imported

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

## How On-VM Configuration Works

Each VM uses a **CustomScriptExtension** driven by a single shared bootstrap script ([`scripts/cse-bootstrap.ps1`](scripts/cse-bootstrap.ps1) for Windows, [`scripts/cse-bootstrap.sh`](scripts/cse-bootstrap.sh) for the Linux driver). The Bicep modules `loadTextContent()` the bootstrap at compile time, substitute per-role tokens (role, deploy script, package URL, base64 admin password), and carry it base64-encoded inside the extension's **encrypted `protectedSettings`**. The bootstrap downloads the DSC package zip (members wait for DC-provided DNS first), injects the credential, and runs the role's orchestrator script ([`Deploy-DC.ps1`](../shared/DSC/Deploy-DC.ps1), [`Deploy-Driver.ps1`](../shared/DSC/Deploy-Driver.ps1), or [`Deploy-SUT.ps1`](DSC/Deploy-SUT.ps1)). These orchestrators use **registry-based step tracking** with deferred reboots:

1. **Step 0 → 1**: DSC configuration + domain join (or DC promotion) → scheduled reboot
2. **Step 1 → 2** (and beyond): Post-reboot DSC re-apply + imperative configuration (tools, accounts, shares, etc.)

The **TKFRSAR** scheduled task re-runs the orchestrator after each reboot. It includes a repeating 5-minute trigger as a safety net in case the initial startup run fails (e.g., AD services not yet ready). A **reboot circuit breaker** (max 3 reboots) prevents infinite reboot loops.

Each orchestrator writes a **signal file** on completion (e.g., `Deploy-DC.Completed.signal`). The deploy script polls the DC's signal file via `Invoke-AzVMRunCommand` before launching Phase 2.

### Pre-flight Validation

Before creating any Azure resources, deploy.ps1 validates:
- **VM size availability** with automatic fallback to alternatives if the preferred size is capacity-constrained
- **Regional vCPU quota** to catch quota limits before a long deployment rolls back
- **OS image availability** to verify marketplace images exist in the target region

## Security Notes

> **Warning**: This environment is designed for testing purposes only:
> - All Windows Firewalls are **disabled**
> - Network security groups allow broad access within the VNet
> - AutoLogon is enabled on all VMs for scheduled task execution
> - Not suitable for production use

## Publishing the public package ("Deploy to Azure" button)

> **Maintainers only.** The [one-click button](#one-click-deploy-deploy-to-azure-button) consumes two public artifacts, both pinned to the **`fileserver-domain-deploy-button-v1`** tag on the public GitHub repo:
> - **Package** → a GitHub **Release asset** `Domain-Package.zip`
> - **Template** → the committed [`azuredeploy.json`](azuredeploy.json) served via `raw.githubusercontent.com/.../<tag>/...` — the committed file *is* the hosted template, so there is no separate template upload and no Bicep→JSON drift.
>
> This avoids anonymous Azure Storage entirely (SFI-friendly). Re-publish whenever the DSC scripts, `Config.json` shape, or `main.bicep` change.

[`Publish-DscPackage.ps1`](Publish-DscPackage.ps1) is a thin wrapper over the shared publisher [`../shared/Publish-DscPackage.ps1`](../shared/Publish-DscPackage.ps1) (the same one the Workgroup scenario uses). It builds the Domain package with a **credential-free** `Config.json` (password field carries the `#{ADMIN_PASSWORD}#` token), strips `ParamConfig.json`, and publishes the Release asset.

**Prerequisites:** the [GitHub CLI](https://cli.github.com) (`gh auth login`) with permission to create releases on `microsoft/WindowsProtocolTestSuites`.

```powershell
# 1. Compile the template (keeps azuredeploy.json in sync with main.bicep)
bicep build main.bicep --outfile azuredeploy.json

# 2. Commit azuredeploy.json and push, so the raw template at the tag is consistent
#    with the package it points to.

# 3. Cut the release + upload the package asset (idempotent; --clobber re-uploads)
gh auth login
.\Publish-DscPackage.ps1 -Tag fileserver-domain-deploy-button-v1 -Target <branch-or-sha>
#  asset -> https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/fileserver-domain-deploy-button-v1/Domain-Package.zip
#  raw   -> https://raw.githubusercontent.com/microsoft/WindowsProtocolTestSuites/fileserver-domain-deploy-button-v1/TestSuites/FileServer/azure-automation/domain-bicep/azuredeploy.json
#  -> prints the Deploy to Azure button URL
```

Validate the package offline first (no GitHub calls) with `-SkipUpload`. The publisher prints an `[OK]`/warning comparing its asset URL against `main.bicep`'s `domainPackageZipUrl` default, so a mismatch is caught before you deploy. To test end-to-end against a **public** personal repo, pass `-Repo <you>/<testrepo> -TemplateRepoPath <path-to-azuredeploy.json-in-that-repo>` and point the template's `domainPackageZipUrl` at your asset.

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
| Node01 | `C:\Domain-Package\DSC\Scripts\InstallMSIAndTools.ps1.log` | Required tool installation and per-tool failures |
| Node01 | `C:\Domain-Package\DSC\Scripts\*.install.stderr.log` | Standard error from ZIP-based child installers |
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
   - Azure Portal: VM → Extensions → ConfigureDomain* → View detailed status
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

## Known Issues

1. **"Trust relationship failed" on a domain member**: Every domain member (DC promotes AD; Driver/SUT/cluster nodes join) sets `Netlogon\DisablePasswordChange=1` so its machine-account password is never auto-rotated out of sync with AD, and the DC deliberately does **not** set `RefusePasswordChange` (which would break secure channels for any member that still rotates). The SUT additionally runs `ksetup /SetComputerPassword` to pin `Password04!` for the Auth suite. If you still see "trust relationship between this workstation and the primary domain failed", it is almost always because a VM was **deallocated** (e.g. auto-shutdown — now off by default, see below) mid-rotation; check the member deploy log (`C:\Domain-Package\DSC\Invoke-SutImperativeSteps.log` on the SUT) for the ksetup/DisablePasswordChange output.

2. **Auto-shutdown disabled by default**: Auto-shutdown is OFF by default for the domain lab because it DEALLOCATES VMs, and a deallocate/restart of a domain-joined member can collide with machine-account password handling (a source of "trust relationship failed" errors). Domain members also set `Netlogon\DisablePasswordChange` to stay safe. Set `enableAutoShutdown = true` in your bicepparam file (and check `autoShutdownTime`) if you want VMs to shut down automatically to save cost.

3. **Azure Disk Encryption**: Off by default for the one-click **button** (`enableDiskEncryption=false`; ADE is a post-deploy step the Portal cannot run — see "Scope & limitations" above). The **`deploy.ps1`** path enables it by default (via the bicepparam file) and applies it to all VMs after deployment, which adds time and requires Key Vault permissions. Use `-SkipDiskEncryption` with `deploy.ps1` to skip it. Managed disks are platform-encrypted at rest regardless.

## File Structure

```
domain-bicep/
├── deploy.ps1                       # Phased deployment orchestrator
├── main.bicep                       # One-click button entry point (composes phase1 + phase2)
├── azuredeploy.json                 # Compiled main.bicep (what the Deploy-to-Azure button deploys)
├── phase1.bicep                     # Phase 1: Key Vault + Network + Bastion + DC
├── phase2.bicep                     # Phase 2: Driver + SUT
├── README.md                        # This file
├── modules/
│   ├── network.bicep               # VNet, subnets, NSGs, Bastion
│   ├── domain-controller.bicep     # DC VM, NICs, extension
│   └── domain-computers.bicep      # Driver + SUT VMs, NICs, extensions
├── scripts/
│   ├── cse-bootstrap.ps1           # Shared Windows CSE bootstrap (token-substituted per role)
│   ├── cse-bootstrap.sh            # Shared Linux driver CSE bootstrap
│   └── Remove-StaleComputerAccounts.ps1  # Pre-deploy DC cleanup of stale computer objects
├── parameters/
│   ├── phase1.bicepparam           # Phase 1 parameters (single source of truth)
│   ├── phase2.bicepparam           # Phase 2 parameters
│   └── VmSizeFallbacks.psd1        # Per-role VM size fallback lists (DC/Driver/SUT)
└── DSC/
    ├── Deploy-SUT.ps1              # SUT orchestrator (features/join → convergence → environment)
    ├── SUT-FeatureConfiguration.ps1 # DSC: disruptive File Server features
    ├── SUT-Configuration.ps1       # DSC: post-reboot shares, FSRM, registry
    ├── Invoke-SutImperativeSteps.ps1     # SUT: domain join, disks, DFS, QUIC
    └── Scripts/                    # Domain-specific scripts

../shared/
├── Deploy-Helpers.psm1             # Azure helpers (connect, storage, polling, quota)
├── Generate-ConfigJson.ps1         # Config.json generation from bicepparam values
└── DSC/
    ├── Deploy-DC.ps1               # DC orchestrator (foundation reboot → promotion reboot → convergence)
    ├── Deploy-Driver.ps1           # Driver orchestrator (baseline/join → tools/tests)
    ├── DC-FeatureConfiguration.ps1 # DSC: AD DS and RemoteAccess role features
    ├── DC-Configuration.ps1        # DSC: post-promotion routing, LDAP, CBAC, services
    ├── Driver-Configuration.ps1    # DSC: hosts, firewall, PS remoting
    ├── Invoke-DcImperativeSteps.ps1      # DC: promotion, accounts, CBAC, GPO, DNS
    ├── Invoke-DriverImperativeSteps.ps1  # Driver: domain join, tools, RSA, ForceLevel2
    └── Scripts/                    # Shared scripts (tools install, test run, validation)
```

The normal Domain reboot contract is deterministic: the SUT and Windows Driver
each use one domain-member reboot, while the DC uses one foundation reboot for
features/hostname and one mandatory promotion reboot. Tool packages are prepared
in parallel before those reboots and installed serially afterward. Additional
pending reboots are treated as failures, not silently retried. Long operations
publish `Deploy-SUT.heartbeat.json`, `Deploy-DC.heartbeat.json`, or
`Deploy-Driver.heartbeat.json`; `deploy.ps1` prints the DC heartbeat when its
readiness gate times out.

## Based on FileServer User Guide

This deployment follows the specifications in:
- **FileServerUserGuide.md** — Section 4.2 (Domain Network Environment)
- **Domain Controller Requirements** — Section 3.3.3 & 5.2.1
- **Network Architecture** — Dual NIC configuration with External1/External2
