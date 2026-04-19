# Shared Components — Azure Automated Deployment

This directory contains components shared across all three deployment scenarios ([Domain](../domain-bicep/), [Cluster](../cluster-bicep/), [Workgroup](../workgroup-bicep/)).

## How DSC Packages Are Assembled

Each scenario has its own `DSC/` folder with scenario-specific scripts. At deploy time, the `deploy.ps1` script merges these with the shared scripts into a single zip package that gets uploaded to Azure Storage and downloaded by each VM.

```
Scenario-specific (e.g., domain-bicep/DSC/)     Shared (shared/DSC/)
├── Deploy-SUT.ps1                               ├── Deploy-DC.ps1
├── SUT-Configuration.ps1                        ├── Deploy-Driver.ps1
├── Invoke-SutImperativeSteps.ps1                ├── DC-Configuration.ps1
└── Scripts/                                     ├── Driver-Configuration.ps1
    ├── Config.json  (template)                  ├── Invoke-DcImperativeSteps.ps1
    └── Tools.json   (scenario-specific)         ├── Invoke-DriverImperativeSteps.ps1
                                                 └── Scripts/
                                                     ├── InstallMSIAndTools.ps1
            ↓  merged by Build-DscPackage  ↓         ├── Execute-TestCaseByContext.ps1
                                                     ├── Invoke-TestRun.ps1
Uploaded Package (e.g., Domain-Package.zip)          └── (16 more utilities)
├── Config.json          ← generated from deployment params
├── ResultsUpload.json   ← storage account for test results
├── Tools.json           ← from scenario DSC/Scripts/
└── DSC/
    ├── Deploy-DC.ps1            (from shared)
    ├── Deploy-Driver.ps1        (from shared)
    ├── Deploy-SUT.ps1           (from scenario)
    ├── SUT-Configuration.ps1    (from scenario)
    ├── DC-Configuration.ps1     (from shared)
    ├── Driver-Configuration.ps1 (from shared)
    └── Scripts/
        ├── Config.json          (copy of generated config)
        ├── Tools.json           (from scenario)
        └── (all shared utilities)
```

**Key rule:** Shared files overwrite scenario-specific files of the same name. This means `shared/DSC/` is the source of truth for DC and Driver scripts — the scenario folders should not contain copies of these.

## On-VM Configuration Pattern

Once a VM downloads the package, its **CustomScriptExtension** runs an orchestrator script (e.g., `Deploy-DC.ps1`). All orchestrators follow the same multi-step pattern:

### Registry-Based Step Tracking

Each orchestrator tracks progress in the registry at `HKLM:\SOFTWARE\ProtocolTestSuites`:

| Key | Purpose |
|-----|---------|
| `DeployStep` | Current step (0, 1, 2, 3...) — incremented after each phase |
| `RebootCount` | Number of reboots taken — circuit breaker at 3-4 max |

### Step Progression

```
Step 0 → 1:  DSC configuration + domain join (or DC promotion)
             → Schedule deferred reboot via TKFRSAR task

Step 1 → 2:  Post-reboot DSC re-apply + imperative configuration
             (tools install, accounts, shares, etc.)
             → Optional reboot if features require it

Step 2 → 3:  Environment-specific setup (test shares, certificates, etc.)
             → Write .Completed.signal file
```

### TKFRSAR Scheduled Task

A scheduled task named **TKFRSAR** re-runs the orchestrator after each reboot. It has:
- A **boot trigger** to run at startup
- A **repeating 5-minute trigger** as a safety net (handles cases where AD services aren't ready at first boot)
- A **reboot circuit breaker** — if `RebootCount` exceeds the max, the orchestrator stops to prevent infinite loops

### Signal Files

Each orchestrator writes a signal file on completion:

| VM Role | Signal File |
|---------|-------------|
| Domain Controller | `Deploy-DC.Completed.signal` |
| Storage Server | `Deploy-Storage.Completed.signal` |
| SUT (Domain/Workgroup) | `Deploy-SUT.Completed.signal` |
| Cluster Node 1 | `Deploy-Node01.Completed.signal` |
| Cluster Node 2 | `Deploy-Node02.Completed.signal` |
| Driver Computer | `Deploy-Driver.Completed.signal` |

The `deploy.ps1` script polls the DC's signal file between Phase 1 and Phase 2. After deployment, use [`scripts/Verify-Deployment.ps1`](scripts/Verify-Deployment.ps1) to poll all VMs.

## File Reference

### Root Files

| File | Purpose |
|------|---------|
| [`Deploy-Helpers.psm1`](Deploy-Helpers.psm1) | PowerShell module with all shared deployment functions. Imported by every `deploy.ps1`. |
| [`Generate-ConfigJson.ps1`](Generate-ConfigJson.ps1) | Generates `Config.json` from deployment parameters. Handles all three scenarios (Domain, Cluster, Workgroup). |

### Key Functions in Deploy-Helpers.psm1

| Function | Purpose |
|----------|---------|
| `Build-DscPackage` | Assembles a DSC package from scenario-specific + shared scripts, generates Config.json, uploads to Azure Storage. Called by all three `deploy.ps1` scripts. |
| `Import-AzureModules` | Imports required Az PowerShell modules |
| `Connect-AzureSubscription` | Authenticates to Azure |
| `Get-OrCreateStorageAccount` | Finds or creates a storage account for package upload |
| `Send-BlobWithSasUrl` | Uploads a file to blob storage and returns a SAS URL |
| `Wait-ForDomainController` | Polls a DC VM's signal file via `Invoke-AzVMRunCommand` |
| `ConvertFrom-BicepParam` | Parses `.bicepparam` files into PowerShell hashtables |
| `Resolve-DeploymentConfig` | Merges parsed params with defaults into a config object |
| `Resolve-AvailableVmSize` | Finds available VM sizes with automatic fallback |
| `Test-RegionalVCpuQuota` | Validates vCPU quota before deployment |
| `Test-VmImageAvailability` | Verifies OS images exist in the target region |
| `Install-DscPackageAssets` | Downloads external assets (GPOBackup.zip, ParamConfig.json) into a package |
| `New-ResultsUploadConfig` | Creates ResultsUpload.json for test result collection |
| `Invoke-DiskEncryptionForVMs` | Applies Azure Disk Encryption to a set of VMs |
| `Watch-Deployment` | Monitors an ARM deployment with progress output |

### DSC/ Directory

Orchestrators and DSC configurations shared by all scenarios:

| File | Used By | Purpose |
|------|---------|---------|
| `Deploy-DC.ps1` | Domain, Cluster | DC orchestrator — AD DS promotion, accounts, CBAC, GPO |
| `Deploy-Driver.ps1` | All | Driver orchestrator — tools install, domain join, test setup |
| `Deploy-CommonHelpers.ps1` | All | Shared functions used by orchestrators |
| `DC-Configuration.ps1` | Domain, Cluster | DSC: AD DS role, DNS, LDAP, RemoteAccess |
| `Driver-Configuration.ps1` | All | DSC: hosts file, firewall rules, PS remoting |
| `Invoke-DcImperativeSteps.ps1` | Domain, Cluster | Post-DSC: AD accounts, CBAC objects, GPO import, DNS records |
| `Invoke-DriverImperativeSteps.ps1` | All | Post-DSC: domain join, tools, RSA keys, ForceLevel2 |

### DSC/Scripts/ Directory

Utility scripts that run on VMs during configuration:

| File | Purpose |
|------|---------|
| `Execute-TestCaseByContext.ps1` | Main test executor — patches ptfconfig, runs tests by context |
| `Invoke-TestRun.ps1` | Test run wrapper — waits for SUT, detects context, calls executor |
| `InstallMSIAndTools.ps1` | Downloads and installs tools from Tools.json manifest |
| `Create-TestAccount.ps1` | Creates AD test accounts and groups |
| `Create-CbacObjectsInDC.ps1` | Creates Claims-Based Access Control objects in AD |
| `Import-GPOForClaims.ps1` | Imports CBAC Group Policy objects |
| `PromoteDomainController.ps1` | Promotes a server to domain controller |
| `domainjoin.ps1` | Joins a computer to the domain with retry logic |
| `Create-DNSRecords.ps1` | Creates DNS records for test endpoints |
| `Validate-ConfigFile.ps1` | Validates Config.json structure |
| `Validate-Environment.ps1` | Post-deploy environment validation |
| `Set-AutoLogon.ps1` | Configures automatic logon for scheduled tasks |
| `Check-DCStatus.ps1` | Checks DC readiness (AD services, DNS) |
| `Configure-TlsCipherSuites.ps1` | Configures TLS cipher suites for testing |
| `Modify-ConfigFileNode.ps1` | Patches individual Config.json values |
| `Get-OSVersionNumber.ps1` | Detects Windows OS version for context selection |
| `RestartAndRunFinish.ps1` | Schedules a deferred reboot |
| `Write-Info.ps1` / `Write-Error.ps1` | Structured logging helpers |

### scripts/ Directory

| File | Purpose |
|------|---------|
| [`Verify-Deployment.ps1`](scripts/Verify-Deployment.ps1) | Polls all VMs in a resource group for completion. Auto-detects scenario (Domain/Cluster/Workgroup) by the VMs present. |

### modules/ Directory

| File | Purpose |
|------|---------|
| [`disk-encryption-vault.bicep`](modules/disk-encryption-vault.bicep) | Bicep module for Azure Key Vault used by disk encryption. Referenced by all three scenarios. |
