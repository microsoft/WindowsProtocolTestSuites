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
├── cse-bootstrap.ps1    ← short generic Windows extension entry point
├── PackageManifest.json ← scenario, source revision, lengths, SHA-256 hashes
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

`Build-DscPackage` validates required scenario content, generates
`PackageManifest.json`, and verifies every hash before compression.
`cse-bootstrap.ps1` repeats that verification before credential injection or
role execution. Cluster packages fail closed when the contracts or manifest are
missing. Public packages exclude private parameter and results-upload files.
`InstallMSIAndTools.ps1` also verifies configured SHA-256 values and expected
ZIP entries for cached and downloaded external assets before preparation or
installation can succeed.

## On-VM Configuration Pattern

Once a VM downloads the package, its **CustomScriptExtension** runs an orchestrator script (e.g., `Deploy-DC.ps1`). All orchestrators follow the same multi-step pattern:

### Registry-Based Step Tracking

Each orchestrator tracks progress in the registry at `HKLM:\SOFTWARE\ProtocolTestSuites`:

| Key | Purpose |
|-----|---------|
| `DeployStep` | Current step (0, 1, 2, 3...) — incremented after each phase |
| `RebootCount` | Number of reboots taken — circuit breaker at 3-4 max |

Current deterministic orchestrators use role-specific phase and reboot values;
`DeployStep` remains only for migration. Cluster Storage uses one bounded
feature/hostname reboot, Cluster nodes use one combined feature/domain-join
reboot, and the Cluster Driver has a dedicated readiness-gated path.

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

The `deploy.ps1` script polls the DC's signal file between Phase 1 and Phase 2 and performs integrated final verification. [`scripts/Verify-Deployment.ps1`](scripts/Verify-Deployment.ps1) is also available for ad-hoc rechecks. Continuations accept existing signals from untouched phases but require fresh signals from redeployed roles.

### Packaged Extension Bootstrap

Windows Custom Script Extensions download the private package through `fileUris`, extract it to a short staging path, and invoke the packaged `cse-bootstrap.ps1`. This keeps extension command lines below Windows limits. Before replacing a Driver package, the bootstrap stops stale test tasks/process trees, removes stale test signals, and forces a fresh Driver completion signal and test launch. Linux Drivers use the native `cse-bootstrap.sh` flow.

### Bounded Test Execution and Reporting

- `Invoke-TestRun.ps1` launches the complete plan under the configured test account. Each VSTest invocation is bounded to 60 minutes and writes an `*.execution.json` manifest; a timeout kills the process tree but does not stop later stages.
- `Write-TestRunSummary.ps1` parses every TRX and manifest into `test.summary.json` and `test.summary.txt`. Classifications distinguish assertion failures from infrastructure/configuration failures and mixed outcomes.
- Finalization uploads TRX, manifests, summaries, and non-secret diagnostics before writing `test.run.completed.signal`. Upload failure is recorded in `test.results.upload.failed.signal`.
- The verifier detects a `Started` manifest stalled for 70 minutes. Scenario deploy scripts pass a 360-minute complete-plan timeout.

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
| `Connect-AzureSubscription` | Validates cached Az authentication, reuses an authenticated Azure CLI session when possible, then falls back to interactive login |
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
| `Deploy-ClusterDriver.ps1` | Cluster | Cluster Driver orchestrator — waits for both nodes, live Cluster readiness, tools, and ForceLevel2 shares |
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
| `Invoke-BoundedProcess.ps1` | Runs native tests with asynchronous output draining and whole-process-tree timeout termination |
| `Invoke-VstestInvocation.ps1` | Writes execution manifests and invokes VSTest through the bounded process helper |
| `Invoke-ProcessAsUser.ps1` | Launches the test runner from LocalSystem with the configured user's profile and environment |
| `Write-TestRunSummary.ps1` | Aggregates all TRX/manifests and writes complete JSON/text summaries |
| `InstallMSIAndTools.ps1` | Downloads and installs tools from Tools.json manifest |
| `Create-TestAccount.ps1` | Creates AD test accounts and groups |
| `Create-CbacObjectsInDC.ps1` | Creates Claims-Based Access Control objects in AD |
| `Import-GPOForClaims.ps1` | Imports CBAC Group Policy objects |
| `PromoteDomainController.ps1` | Promotes a server to domain controller |
| `domainjoin.ps1` | Joins a computer to the domain with retry logic |
| `Create-DNSRecords.ps1` | Creates DNS records for test endpoints |
| `Validate-ConfigFile.ps1` | Validates Config.json structure |
| `Validate-Environment.ps1` | Post-deploy environment validation |
| `Package-Contracts.ps1` | Required package content plus manifest and SHA-256 verification |
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
| [`Verify-Deployment.ps1`](scripts/Verify-Deployment.ps1) | Polls bounded VM probes and automatic tests, rejects stale signals, detects stalled/failed invocations and uploads, retrieves the complete summary, and supports explicit phase-role subsets. |

### modules/ Directory

| File | Purpose |
|------|---------|
| [`disk-encryption-vault.bicep`](modules/disk-encryption-vault.bicep) | Bicep module for Azure Key Vault used by disk encryption. Referenced by all three scenarios. |
