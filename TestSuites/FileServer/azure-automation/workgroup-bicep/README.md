# File Server Protocol Test Suite - Workgroup Environment Deployment

This directory contains Azure Bicep templates for deploying the File Server Protocol Test Suite in **Workgroup mode** — a simplified configuration without Active Directory Domain Services.

> **Choosing a scenario?** See the [top-level azure-automation README](../README.md) for a comparison of Domain vs. Cluster vs. Workgroup.

## One-Click Deploy ("Deploy to Azure" button)

For a demo/onboarding environment with **defaults**, deploy straight from the Azure Portal — no local clone, no PowerShell, no `deploy.ps1`:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FWindowsProtocolTestSuites%2F4.26.8.0%2FTestSuites%2FFileServer%2Fazure-automation%2Fworkgroup-bicep%2Fazuredeploy.json)

> The button points at [`azuredeploy.json`](azuredeploy.json) served from `raw.githubusercontent.com` at the **`4.26.8.0`** release tag, and the template pulls its DSC package from the same GitHub **Release**. The button goes live once a maintainer publishes that release (see [Publishing the public package](#publishing-the-public-package-deploy-to-azure-button)).

The Portal renders a form from [`main.bicep`](main.bicep) (compiled to [`azuredeploy.json`](azuredeploy.json)). Enter an **admin password** and deploy; the Driver + SUT come up and tests run automatically — the same outcome as `deploy.ps1`, without the local build step.

**How credentials stay safe.** The button consumes a **public**, pre-built DSC package. That package must never contain secrets, so its `Config.json` ships with a placeholder token (`#{ADMIN_PASSWORD}#`) instead of a password. At deploy time the VM's Custom Script Extension — running from the extension's *encrypted* `protectedSettings` — injects the real admin password (passed base64-encoded, then JSON-escaped) into `Config.json` via [`../shared/DSC/Scripts/Set-ConfigCredential.ps1`](../shared/DSC/Scripts/Set-ConfigCredential.ps1). The credential-bearing bootstrap runs in a separate PowerShell process so transcript headers contain only the script path, then deletes itself on success or failure. The single admin password is reused for the local NonAdmin test account.

**Scope & limitations (step 1 — defaults only):**
- The baked `Config.json` is valid for the **default IP topology only**. Changing the IP parameters in the Portal form will not update the peer IPs inside the package. For custom topologies, use `deploy.ps1` (which rebuilds the package) until on-VM `Config.json` regeneration lands (step 2).
- The package embeds a **fixed test-suite drop** (a snapshot) — appropriate for demo/onboarding, not for testing an arbitrary build.
- Marketplace **image terms** for the SUT/Driver images may need to be accepted once per subscription (see the [top-level README](../README.md)); otherwise the deployment can fail on first use.
- **Disk encryption is off by default for the button** (`enableDiskEncryption=false`). Azure Disk Encryption (ADE) is applied by `deploy.ps1` as a *post-deploy* step, which the Portal button cannot run — so for the button the Key Vault would be created but never used. Managed disks are platform-encrypted at rest regardless. If you specifically want ADE and your subscription has the required Key Vault permissions, set `enableDiskEncryption` to **true** in the form.
- **VM sizes default to burstable (B-series):** driver `Standard_B4ms`, SUT `Standard_B8ms` — chosen for **broad regional availability** (fewest capacity errors) and low cost for demo/onboarding. Trade-off: the driver's CPU may throttle during long test runs, so runs can be slower than `deploy.ps1` (which uses compute-optimized sizes). For faster runs, change the VM sizes in the form or use `deploy.ps1`.
- **VM capacity / quota** can still vary by region. If the deployment fails preflight with `SkuNotAvailable`, `AllocationFailed`, or `ZonalAllocationFailed`, pick a different **Region** (the template deploys into the resource group's region) or change the **driver/SUT VM size** in the form. Unlike `deploy.ps1`, the one-click button **cannot auto-retry across SKUs**, so this is a manual choice.

**Publishing / updating the package + template** (maintainers): see [Publishing the public package](#publishing-the-public-package-deploy-to-azure-button) below.

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

- PowerShell 7 and PowerShellGet access to PowerShell Gallery
- Azure subscription with permissions to create resource groups, VMs, storage accounts, and Key Vaults
- Bicep CLI or Azure CLI

`deploy.ps1` installs missing `Az.Accounts`, `Az.Resources`, `Az.Storage`, and `Az.Compute` modules for the current user. If `bicep` is not on `PATH`, it installs Bicep through Azure CLI. Both install paths use bounded retries and fail with a concrete prerequisite message. A cached Azure login is reused; authentication is requested only when required.

## Quick Start

### 1. Navigate to the workgroup-bicep directory

```powershell
cd workgroup-bicep
```

### 2. Run the deployment

```powershell
$adminPassword = Read-Host "Enter admin password" -AsSecureString

.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fs-workgroup-test" `
  -AdminPassword $adminPassword
```

### 3. Wait for deployment and automatic tests

The script runs a single Bicep deployment (~5 min for Azure resources). Bastion is deployed alongside the VMs after core networking, so Bastion provisioning does not delay VM creation. The same command then remains attached while the VMs configure themselves:

1. **SUT**: Feature preparation, one planned reboot, post-reboot shares/FSRM convergence, then environment setup
2. **Driver** (~10-15 min): Tools install, RSA keys, test environment setup
3. **Tests run automatically** — a scheduled task on the Driver waits for SUT readiness, then executes the full test suite. No login required.

The command does not report success merely because ARM reports `Succeeded`. It requires fresh SUT and Driver signals, both test completion signals, and at least one TRX file. Each Run Command probe is bounded and transient probe errors use capped backoff.

When Azure Disk Encryption is enabled, it runs only after automatic tests finalize, avoiding a reboot race with DSC, tool installation, and test execution. Driver and SUT encryption operations run concurrently; afterward both VM agents must answer a fresh Run Command probe.

### 4. Check test results

Connect to Client01 via **Azure Bastion** in the portal and check:
```powershell
# See if tests finished
Test-Path C:\Test\test.finished.signal
Test-Path C:\Test\test.run.completed.signal

# View results
Get-ChildItem C:\Test\TestResults\*.trx
```

### Deployment Timeline

| What | Duration | Notes |
|------|----------|-------|
| Bicep deployment (Network + VMs) | ~5 min | Single phase, no DC dependency |
| SUT configuration | Image-dependent | Feature-only DSC, one planned reboot, convergence DSC, environment setup |
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

> **Note:** The Azure region is read from `param location` in the bicepparam file (single source of truth). Edit `parameters/workgroup.bicepparam` to change it.

### Optional Parameters (command-line)

| Parameter | Default | Description |
|-----------|---------|-------------|
| `ParametersFile` | `parameters/workgroup.bicepparam` | Bicep parameters file |
| `DscFolderPath` | `DSC/` (adjacent to deploy.ps1) | Path to DSC scripts folder |
| `DscPackageZipUrl` | *(empty)* | Direct URL to pre-built DSC package zip |
| `StorageAccountName` | *(empty)* | Use an existing storage account instead of creating one |
| `LocalUserPassword` | `AdminPassword` | Optional compatibility override; ordinary test accounts are unified to the admin password |
| `ValidateOnly` | `$false` | Run Bicep validation only (no deployment) |
| `SkipDiskEncryption` | `$false` | Skip post-deployment Azure Disk Encryption |
| `Resume` | `$false` | Resume a previously failed deployment (see below) |
| `ConfigurationTimeoutMinutes` | `90` | Maximum wait for fresh SUT and Driver completion signals |
| `TestTimeoutMinutes` | `360` | Maximum wait for the complete automatic test plan and finalization |
| `SkipTestWait` | `$false` | Return after VM configuration; must be combined with `-SkipDiskEncryption` to avoid an ADE/test reboot race |

### Template Parameters (bicepparam file)

Edit [`parameters/workgroup.bicepparam`](parameters/workgroup.bicepparam) to customize the environment:

| Parameter | Default | Description |
|-----------|---------|-------------|
| `location` | `West US 2` | Azure region |
| `environmentPrefix` | `fstest` | Prefix for resource names |
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
| `nonadmin` | Non-admin test user — uses the same password as `testadmin` so `PasswordForAllUsers` logons are consistent |
| `Guest` | Guest account — enabled by `Create-TestAccount.ps1` with the admin password (required by Guest-access test cases) |

## Resuming a Failed Deployment

If the deployment fails partway through (e.g., VM extension error, quota issue), use `-Resume` to retry without repeating pre-flight checks or re-uploading the DSC package:

```powershell
.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fs-workgroup-test" `
    -AdminPassword $adminPassword `
    -Resume
```

Resume builds and uploads a fresh private package, starts stopped VMs, clears stale deployment/test tasks, processes, and signals, and sends bounded Run Command jobs to the existing SUT and Driver. It does not redeploy unchanged Azure infrastructure. The command then applies the same fresh-signal and optional test-completion verification as an initial deployment.

If the local workstation restarts, guest setup and tests continue on the VMs, but the local script does not automatically resume its checkpoint. Rerun the same command with `-Resume`; expired Az PowerShell authentication can be restored from an authenticated Azure CLI session.

## Automatic Test Execution

After deployment completes, the Driver VM automatically runs the FileServer test suite — **no login required**. `Deploy-Driver.ps1` launches it immediately under the configured test account and registers `RunFileServerTests` as an at-startup fallback.

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
   4. Gives every native test invocation a 60-minute watchdog; a timeout is recorded and later stages continue
   5. Writes complete JSON/text summaries and uploads TRX, manifests, and non-secret diagnostics
   6. Writes C:\Test\test.run.completed.signal after upload/finalization handling
```

### Workgroup SUT phase flow

`Deploy-SUT.ps1` persists `WorkgroupSutDeployPhase` under
`HKLM:\SOFTWARE\ProtocolTestSuites`:

1. **Phase 0 — Features:** rename is requested without immediately rebooting; one DSC resource attempts all disruptive File Server, DFS, FSRM, Hyper-V, and management features. Tool packages are prepared in parallel using atomic temporary downloads.
2. **Single planned reboot:** feature and rename requirements are coalesced. The resume task verifies that a new Windows boot actually occurred.
3. **Phase 1 — Convergence:** a non-disruptive MOF creates shares and directories and configures routing, firewall, remoting, FSRM, and registry state.
4. **Phase 2 — Environment and tools:** prepared tools install serially while idempotent disk, FSA, DFS, symlink, account, and QUIC setup runs.
5. **Phase 3 — Complete:** the completion signal is written only after concrete DSC, tools, DFS, account, and filesystem postconditions pass.

Long-running operations update
`C:\Workgroup-Package\DSC\Deploy-SUT.heartbeat.json`. The file identifies the
phase, active operation, elapsed time, deadline, and last successful checkpoint.

### Test filter

To run a subset of tests, run `Invoke-TestRun.ps1` manually:

```powershell
C:\Workgroup-Package\DSC\Scripts\Invoke-TestRun.ps1 -Filter "TestCategory=BVT"
```

### Re-running tests

To re-run tests after a completed run, delete the signal file and run the script again:

```powershell
Remove-Item C:\Test\test.finished.signal, C:\Test\test.run.completed.signal
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

- **Auto-shutdown** is enabled by default at 20:00 UTC, but the deploy script removes existing schedules before configuration and restores them only after terminal test finalization and optional encryption. Known terminal test classifications are reported without failing deployment orchestration; missing output, failed upload/readiness, and post-test infrastructure failures remain fatal. Disable with `param enableAutoShutdown = false` or adjust `param autoShutdownTime = '2300'`.
- **Azure Disk Encryption** is enabled by default on the `deploy.ps1` path (via the bicepparam file); a Key Vault is created automatically for encryption keys. It runs after test finalization to avoid disruptive reboots during setup or tests. Skip with `-SkipDiskEncryption` if not needed or to speed up deployment. The one-click button defaults it **off** (see Scope & limitations above).

## Publishing the public package ("Deploy to Azure" button)

> **Maintainers only.** The [one-click button](#one-click-deploy-deploy-to-azure-button) consumes two public artifacts, both pinned to the **`4.26.8.0`** FileServer release:
> - **Package** → a GitHub **Release asset**: `https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/<tag>/Workgroup-Package.zip`
> - **Template** → the committed [`azuredeploy.json`](azuredeploy.json) served via `raw.githubusercontent.com/.../<tag>/...` — the committed file *is* the hosted template, so there is no separate template upload and no Bicep→JSON drift.
>
> This avoids anonymous Azure Storage entirely (SFI-friendly). Re-publish whenever the DSC scripts, `Config.json` shape, or `main.bicep` change.

[`Publish-DscPackage.ps1`](Publish-DscPackage.ps1) builds the Workgroup package with a **credential-free** `Config.json` (password fields carry the `#{ADMIN_PASSWORD}#` token — see [How credentials stay safe](#one-click-deploy-deploy-to-azure-button)) and publishes it as the Release asset.

**Prerequisites:** the [GitHub CLI](https://cli.github.com) (`gh auth login`) with permission to create releases on `microsoft/WindowsProtocolTestSuites`.

```powershell
# 1. Compile the template (keeps azuredeploy.json in sync with main.bicep)
bicep build main.bicep --outfile azuredeploy.json

# 2. Commit azuredeploy.json (with the tag's dscPackageZipUrl) and push, so the
#    raw template at the tag is consistent with the package it points to.

# 3. Cut the release + upload the package asset (idempotent; --clobber re-uploads)
gh auth login
.\Publish-DscPackage.ps1 -Tag 4.26.8.0 -Target <branch-or-sha>
#  asset -> https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/4.26.8.0/Workgroup-Package.zip
#  raw   -> https://raw.githubusercontent.com/microsoft/WindowsProtocolTestSuites/4.26.8.0/TestSuites/FileServer/azure-automation/workgroup-bicep/azuredeploy.json
#  -> prints the Deploy to Azure button URL
```

Validate the package offline first (no GitHub calls) with `-SkipUpload`:

```powershell
.\Publish-DscPackage.ps1 -SkipUpload -OutputZipPath .\Workgroup-Package.zip
```

The script refuses to publish if `Config.json` does not contain the placeholder token — a guard against ever leaking a real credential into a public asset.

**When bumping the tag:** publish to the new `-Tag`, then update the `dscPackageZipUrl` default in [`main.bicep`](main.bicep), recompile `azuredeploy.json`, and update the button URL in this README to the new tag. Old templates stay pinned to their old package. The publish script prints an `[OK]`/warning comparing its asset URL against `main.bicep`'s default, so a mismatch is caught before you deploy.

### Testing against your own repo (before touching the public repo)

Validate end-to-end without cutting a real wpts release: push to a **public** personal repo (private repos can't be fetched anonymously by the Portal or the VM's CSE), then point the script at it.

```powershell
# Place azuredeploy.json in your test repo (root is fine) and push it, then:
.\Publish-DscPackage.ps1 -Repo <you>/<testrepo> -Tag test-v1 -Target main -TemplateRepoPath azuredeploy.json
```

- `-TemplateRepoPath` **must match where azuredeploy.json actually lives** in the test repo (`azuredeploy.json` for a root file; the default deep path is for the real wpts repo). A mismatch gives the Portal a 404 / "template not publicly accessible".
- The template you deploy must reference **your** asset: either set `dscPackageZipUrl` in the Portal form to the printed asset URL, or edit your test repo's `azuredeploy.json` default and re-push (the script warns when they don't match).

## Comparison with Domain Deployment

| Feature | Workgroup | Domain |
|---------|-----------|--------|
| VMs | 2 (Client01, Node01) | 3 (DC01, Client01, Node01) |
| Domain Controller | No | Yes |
| Phased Deployment | Yes (persisted SUT phases, one planned reboot) | Yes (DC must be ready first) |
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
| Client01 (Linux) | `/var/log/azure/custom-script/handler.log` | Driver extension output |
| Node01 | `C:\Workgroup-Package\DSC\Deploy-SUT.log` | SUT orchestrator |
| Node01 | `C:\Workgroup-Package\DSC\Deploy-SUT.heartbeat.json` | Current SUT phase and active long-running operation |
| All (Windows) | `C:\workgroup-*-setup.log` | CustomScriptExtension bootstrap |
| Test results | `C:\Test\TestResults\*.trx` | TRX result files |

### Common Issues

1. **VM extension failure**: Check bootstrap logs on the VM (`C:\workgroup-*-setup.log`) and the orchestrator logs above.

2. **Tests not running**: Verify the scheduled task exists: `Get-ScheduledTask -TaskName 'RunFileServerTests'`. If it doesn't exist, the Driver configuration hasn't completed yet — check `Deploy-Driver.log`.

3. **SUT readiness timeout**: The test runner waits for the SUT signal file via SMB. If it times out, check `Deploy-SUT.log` and `Deploy-SUT.heartbeat.json` on Node01. Verify SMB connectivity: `Test-NetConnection -ComputerName 192.168.1.11 -Port 445`.

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
├── azuredeploy.json              # Compiled template consumed by the Deploy-to-Azure button
├── Publish-DscPackage.ps1        # Maintainer publisher for the button's package + release
├── README.md                     # This file
├── DSC/                          # SUT DSC configuration scripts
│   ├── Deploy-SUT.ps1            # SUT orchestrator (DSC + imperative)
│   ├── SUT-FeatureConfiguration.ps1 # Pre-reboot disruptive feature bundle
│   ├── SUT-Configuration.ps1     # Post-reboot convergence configuration
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
├── Deploy-Helpers.psm1           # Azure helpers (connect, storage, quota, SKU-fallback deploy)
├── Generate-ConfigJson.ps1       # Config.json generation from bicepparam values
├── scripts/
│   └── cse-bootstrap.ps1 / .sh   # Tokenized Custom Script Extension bootstrap (all scenarios)
├── parameters/
│   └── VmSizeFallbacks.psd1      # Per-role fallback VM sizes (data, not code)
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
