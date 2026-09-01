# File Server Protocol Test Suite - Azure Automated Deployment

Automated Infrastructure-as-Code (IaC) for deploying complete File Server Protocol Test Suite environments on Azure. Each scenario deploys a ready-to-use test environment with networking, VMs, Active Directory (where applicable), file server roles, and test tooling — all configured end-to-end.

## One-Click Deploy to Azure

| Workgroup | Domain | Cluster |
|---|---|---|
| [![Deploy Workgroup to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FWindowsProtocolTestSuites%2F4.26.9.0%2FTestSuites%2FFileServer%2Fazure-automation%2Fworkgroup-bicep%2Fazuredeploy.json) | [![Deploy Domain to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FWindowsProtocolTestSuites%2F4.26.9.0%2FTestSuites%2FFileServer%2Fazure-automation%2Fdomain-bicep%2Fazuredeploy.json) | [![Deploy Cluster to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FWindowsProtocolTestSuites%2F4.26.9.0%2FTestSuites%2FFileServer%2Fazure-automation%2Fcluster-bicep%2Fazuredeploy.json) |

## Which Scenario Do I Need?

| | [Workgroup](workgroup-bicep/README.md) | [Domain](domain-bicep/README.md) | [Cluster](cluster-bicep/README.md) |
|---|---|---|---|
| **Best for** | Quick validation, CI gates, basic SMB testing | Full protocol test coverage with AD | Failover, clustered shares, iSCSI |
| **VMs deployed** | 2 (Driver + SUT) | 3 (DC + Driver + SUT) | 5 (DC + Storage + 2 Nodes + Driver) |
| **Active Directory** | No | Yes | Yes |
| **Deploy time** | ~15 min | ~30 min | ~40 min |
| **Command returns** | After VM configuration and automatic tests are verified | After VM configuration and automatic tests are verified | After cluster configuration and automatic tests are verified |
| **Post-deploy manual steps** | None | None | None (manual checks remain available for troubleshooting) |
| **Authentication** | Local accounts | Domain accounts (Kerberos, NTLM) | Domain accounts (Kerberos, NTLM) |
| **Test coverage** | SMB2, DFSC, FSA, SQOS, RSVD | Full suite (SMB2, FSA, DFSC, Auth, FSRVP, RSVD, SQOS, QUIC) | Full suite + ServerFailover, clustered shares |
| **Linux driver support** | Yes | Yes | Yes |
| **Custom VM images** | Yes | Yes | Yes |
| **Resume on failure** | Yes (`-Resume`) | Yes (`-SkipPhase1`) | Yes (`-SkipPhase1`) |

> **Not sure?** Start with **Domain** — it covers the full test suite with the simplest setup. Use **Workgroup** if you need fast, fully-automatic runs (e.g., CI). Use **Cluster** only when testing failover scenarios.

## Controlling automatic test execution

All scenarios expose `enableTestAutoRun`, which defaults to `true`. Set it to
`false` in the Azure Portal form or the scenario's `.bicepparam` file to finish
after environment configuration without starting FileServer tests. The setting
is persisted as `TestExecution.AutoRun` in `Config.json`, so resumed deployments
retain the selected behavior.

When autorun is disabled, start the prepared test plan manually on the Driver:

```powershell
pwsh -File "C:\<Scenario>-Package\DSC\Scripts\Invoke-TestRun.ps1" `
    -WorkingPath "C:\<Scenario>-Package"
```

`-SkipTestWait` is different: it detaches the local Workgroup deployment command
from test monitoring but does not disable the Driver's test run.

## Prerequisites

These apply to all three scenarios:

- **PowerShell 7**
- **Azure subscription** with permissions to create resource groups, VMs, storage accounts, and Key Vaults
- **PowerShellGet** with access to PowerShell Gallery. The scripts install missing `Az.Accounts`, `Az.Resources`, `Az.Storage`, and `Az.Compute` modules for the current user.
- **Bicep CLI or Azure CLI**. If `bicep` is missing, the scripts install it through `az bicep install` with bounded retries.

The scripts first validate a cached Az PowerShell context. If its token expired after a workstation restart, they reuse an authenticated Azure CLI session through a process-scoped ARM token. Interactive Azure authentication occurs only when neither context is usable. Tokens, credentials, and signed package URLs are not written to deployment logs.

## Release packages

The official OneClick release pipeline is
`pipelines/1es/FileServer-OneClick-Release.yml`. Run it only after the signed
FileServer, PTMService, and PTMCli archives are final. Supply the HTTPS URL and
SHA-256 hash for each signed archive.

The pipeline copies the deployment sources into an isolated staging directory,
pins those release assets in each publishable scenario's `Tools.json`, signs
the staged PowerShell files through ESRP, runs the tests under
`azure-automation/tests`, and builds these public, credential-free artifacts:

- `Workgroup-Package.zip`
- `Domain-Package.zip`
- `Cluster-Package.zip`
- `SHA256SUMS.txt`

The pipeline publishes the files as the `FileServer-OneClick-Packages` pipeline
artifact. A release owner must download that artifact, complete the clean Azure
deployment checks, and upload the unchanged ZIP files to the `4.26.9.0`
FileServer GitHub release alongside the primary test-suite assets. Do not
rebuild or replace packages after signing.

## Quick Start

### 1. Navigate to the scenario folder

```powershell
# Pick one:
cd TestSuites/FileServer/azure-automation/domain-bicep      # Most common
cd TestSuites/FileServer/azure-automation/workgroup-bicep    # Fastest
cd TestSuites/FileServer/azure-automation/cluster-bicep      # Failover testing
```

### 2. Run the deployment

**All scenarios**:

```powershell
$password = Read-Host -Prompt "Enter admin password" -AsSecureString

.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fileserver-test" `
    -AdminPassword $password
```

For Workgroup, this password is used for both the VM administrator and the
ordinary test accounts.

### 3. Wait for VM configuration to finish

Azure Portal can show an ARM deployment as `Succeeded` before guest configuration finishes (AD promotion, domain joins, feature installation). All three deployment scripts perform integrated guest and automatic-test verification. They require role completion signals, both test finalization signals, a complete summary, and at least one TRX result. After terminal test finalization, requested Azure Disk Encryption and auto-shutdown schedules are handled before the command returns. All known terminal test classifications are printed and uploaded without being conflated with deployment-orchestration failure. Missing summaries/TRX, failed uploads, incomplete guest setup, and post-test infrastructure failures still produce a nonzero exit. VM responsiveness is checked again after encryption.

For an ad-hoc recheck, use the shared verifier:

**All scenarios** — use the shared verification script:
```powershell
# From any scenario folder (or use the full path)
..\shared\scripts\Verify-Deployment.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fileserver-test" `
    -Scenario Workgroup
```

Pass `-WaitForTests` when that scenario launches tests automatically. Each Azure Run Command probe has its own timeout; transient probe failures use bounded backoff. Pass `-Scenario` explicitly for partial or failed deployments so a missing VM cannot be mistaken for a smaller successful scenario.

### 4. Connect and use

Connect to any VM via **Azure Bastion** in the portal (no public IPs are exposed). For the Workgroup scenario, tests run automatically after deployment — check results at `C:\Test\TestResults\` on the Driver VM.

## Deployment Flow

All three scenarios follow the same core pattern. Domain member infrastructure can be provisioned while the Domain Controller finishes configuring, but member guest extensions and domain join remain gated on the DC readiness signal.

### Domain Scenario

```mermaid
flowchart TB
    start([deploy.ps1]) --> preflight[Pre-flight validation<br/>VM sizes, quotas, OS images]
    preflight --> upload[Package DSC scripts<br/>Upload to Azure Storage]

    upload --> phase1

    subgraph phase1 [Phase 1 — Bicep Deployment]
        net[Core network<br/>VNet, Subnets, NSGs] --> dc[DC01<br/>Windows Server VM]
        net --> bastion[Azure Bastion]
        dc --> ext_dc[CustomScriptExtension<br/>Downloads DSC package]
    end

    ext_dc --> members[Provision Client01 + Node01<br/>infrastructure only]
    ext_dc --> poll{Poll DC readiness<br/>via Invoke-AzVMRunCommand}
    poll -- Signal file not found --> wait[Wait 30s] --> poll
    members --> gate{DC ready?}
    poll -- Deploy-DC.Completed.signal found --> gate

    subgraph phase2 [Phase 2B — Guest Configuration]
        gate --> ext_driver[Client01 CustomScriptExtension]
        gate --> ext_sut[Node01 CustomScriptExtension]
    end

    ext_driver --> verify[Verify fresh member signals<br/>and complete automatic tests]
    ext_sut --> verify
    verify --> ade{ADE enabled?}
    ade --> done([Report terminal result])
```

### Cluster Scenario

```mermaid
flowchart TB
    start([deploy.ps1]) --> preflight[Pre-flight validation]
    preflight --> upload[Package & upload DSC scripts]

    upload --> phase1

    subgraph phase1 [Phase 1 — Bicep Deployment]
        net[Network<br/>VNet, Subnets, NSGs, Bastion<br/>NAT Gateway]
        net --> dc[DC01<br/>Domain Controller]
        net --> storage[Storage01<br/>iSCSI Target Server<br/>4 virtual disks]
    end

    dc --> poll{Poll DC readiness}
    poll -- Waiting --> wait[Wait 30s] --> poll
    poll -- DC ready --> phase2

    subgraph phase2 [Phase 2 — Bicep Deployment]
        ilb[Standard internal load balancer<br/>4 dual-subnet frontends]
        node1[Node01<br/>Cluster Node]
        node2[Node02<br/>Cluster Node]
        driver[Client01<br/>Driver]
        ilb --> node1
        ilb --> node2
    end

    phase2 --> configure[Node01 forms cluster<br/>and creates clustered roles/shares]
    configure --> verify[Verify all role signals<br/>and complete automatic tests]
    verify --> ade{ADE enabled?}
    ade --> done([Report terminal result])
```

### Workgroup Scenario

```mermaid
flowchart TB
    start([deploy.ps1]) --> preflight[Pre-flight validation]
    preflight --> upload[Package & upload DSC scripts]

    upload --> deploy

    subgraph deploy [Single Bicep Deployment]
        net[Core network<br/>VNet, Subnets, NSGs]
        net --> bastion[Azure Bastion]
        net --> driver[Client01 — Driver VM]
        net --> sut[Node01 — SUT VM]
    end

    driver --> ext_driver[Background config:<br/>Tools install, RSA keys]
    sut --> ext_sut[Background config:<br/>File Server role, shares, FSRM]

    ext_driver --> test[Scheduled task fires<br/>Waits for SUT readiness<br/>Runs full test suite automatically]
    ext_sut --> test
    test --> verify[Verify fresh VM + test signals<br/>and TRX output]
    verify --> ade{ADE enabled?}
    ade -- Yes --> encrypt[Encrypt both VMs<br/>then verify VM agents]
    ade -- No --> results([Results at C:\Test\TestResults\])
    encrypt --> results
```

### On-VM Configuration Flow

Once Azure deploys a VM, its **CustomScriptExtension** downloads the DSC package and runs an orchestrator script (e.g., [`Deploy-DC.ps1`](shared/DSC/Deploy-DC.ps1), [`Deploy-SUT.ps1`](domain-bicep/DSC/Deploy-SUT.ps1)). These orchestrators use a multi-step pattern with automatic restarts:

```mermaid
flowchart LR
    ext[VM Extension<br/>downloads package] --> step0

    step0[Step 0 → 1<br/>DSC config + domain join<br/>or DC promotion]
    step0 -- deferred reboot --> step1

    step1[Step 1 → 2<br/>Post-reboot DSC drift check/repair<br/>+ imperative config]
    step1 -- optional reboot --> step2

    step2[Step 2 → 3<br/>Environment setup<br/>shares, accounts, tools]
    step2 --> signal[Write .Completed.signal]
```

Current phased orchestrators persist role-specific phase values under
`HKLM:\SOFTWARE\ProtocolTestSuites` while retaining `DeployStep` only for
backward migration. Workgroup uses `WorkgroupSutDeployPhase`; Domain uses
`DomainSutDeployPhase` and `DcDeployPhase`. The Domain SUT has one normal
feature/domain-join reboot. The DC has one foundation reboot and one mandatory
promotion reboot. The Driver verifies its one domain-join reboot and secure
channel before continuing. Unexpected extra reboots are fatal. A **TKFRSAR**
scheduled task resumes each orchestrator only across its planned boundary.

## Network Architecture

All scenarios use the same network layout (Cluster adds Storage01 and Node02):

```
Azure VNet: 192.168.0.0/16
├── AzureBastionSubnet  192.168.0.0/26    ← Secure RDP/SSH access (no public IPs)
├── External1 Subnet    192.168.1.0/24    ← Primary network
│   ├── DC01        .10     (Domain/Cluster only)
│   ├── Node01      .11     (SUT or Cluster Node 1)
│   ├── Node02      .12     (Cluster only)
│   ├── Cluster01   .100    (Cluster ILB frontend, Cluster only)
│   ├── GeneralFS   .200    (Cluster ILB frontend, Cluster only)
│   ├── Storage01   .50     (Cluster only, NOT domain-joined)
│   └── Client01    .111    (Driver — runs test cases)
└── External2 Subnet    192.168.2.0/24    ← Secondary network
    ├── DC01        .10
    ├── Node01      .11
    ├── Node02      .12     (Cluster only)
    ├── Cluster01   .100    (Cluster ILB frontend, Cluster only)
    ├── GeneralFS   .200    (Cluster ILB frontend, Cluster only)
    └── Client01    .111
```

Cluster node subnets use a NAT Gateway for deterministic outbound package and
tool downloads. Its Standard internal load balancer uses floating-IP rules and
role-specific health probes so Cluster and GeneralFS names remain reachable
after ownership moves between nodes. IP addresses are configurable in each
scenario's parameter files: [Domain](domain-bicep/parameters/),
[Cluster](cluster-bicep/parameters/), and
[Workgroup](workgroup-bicep/parameters/).

## Customization

All configuration lives in Bicep parameter files — one source of truth per scenario:

| Scenario | Parameter files |
|----------|----------------|
| Domain | [`phase1.bicepparam`](domain-bicep/parameters/phase1.bicepparam), [`phase2.bicepparam`](domain-bicep/parameters/phase2.bicepparam) |
| Cluster | [`phase1.bicepparam`](cluster-bicep/parameters/phase1.bicepparam), [`phase2.bicepparam`](cluster-bicep/parameters/phase2.bicepparam) |
| Workgroup | [`workgroup.bicepparam`](workgroup-bicep/parameters/workgroup.bicepparam) |

Common things you can change:

- **Azure region** — `param location` in the bicepparam file
- **VM sizes** — `param dcVmSize`, `param sutVmSize`, `param driverVmSize` (auto-fallback if unavailable)
- **OS versions** — `param dcOsVersion`, `param sutOsVersion`, `param driverOsVersion`
- **IP addresses** — `param dcExternal1Ip`, `param sutExternal1Ip`, `param driverExternal1Ip`
- **Domain name** — `param domainName` (default: `contoso.com`)
- **Custom VM images** — `param dcCustomImageId`, `param sutCustomImageId`, etc.
- **Linux driver** — `param driverOsType = 'Linux'` (deploys Ubuntu 24.04 LTS)
- **Auto-shutdown** — `param enableAutoShutdown`, `param autoShutdownTime`

See each scenario's README for the full parameter reference.

## Resuming Failed Deployments

Domain and Cluster deployments support resuming from Phase 2 if Phase 1 succeeded but Phase 2 failed:

```powershell
.\deploy.ps1 `
    -SubscriptionId "your-subscription-id" `
    -ResourceGroupName "fileserver-test" `
    -AdminPassword $password `
    -SkipPhase1
```

This retrieves Phase 1 outputs automatically (subnet IDs, DC IP), reuses the previously uploaded DSC package, and redeploys only Phase 2 resources.

Continuation preserves AD computer objects for VMs that still exist. Unchanged Phase 1 roles may use their existing completion signals; redeployed Phase 2 roles and test signals must be newer than the continuation start time.

If the local shell or workstation restarts, VM-side extensions, scheduled tasks, and test processes continue independently. The local `deploy.ps1` process does not checkpoint and restart itself: rerun Workgroup with `-Resume`, or Domain/Cluster with `-SkipPhase1`. An authenticated Azure CLI session can restore the Az PowerShell context without another interactive login.

You can also deploy Phase 1 only (`-SkipPhase2`) to inspect the DC before continuing, or validate templates without deploying (`-ValidateOnly`).

## Automatic Test Reliability and Results

- Every VSTest invocation has a 60-minute watchdog by default. A timed-out process tree is terminated, recorded in an `*.execution.json` manifest, and later test stages still run.
- The verifier reports a `Started` manifest as stalled after 70 minutes. Scenario deploy scripts allow 360 minutes for the complete plan; direct verifier use defaults to 120 minutes unless overridden.
- Finalization writes `test.summary.json` and `test.summary.txt`, then uploads TRX files, execution manifests, summaries, and non-secret Driver/SUT diagnostics. `test.results.upload.failed.signal` records an incomplete upload.
- Summary classifications are `Passed`, `TestFailures`, `InfrastructureOrConfigurationFailure`, or `MixedTestAndInfrastructureFailures`. These are test-run outcomes: they do not stop later stages or fail the deployment command after the complete report is printed and uploaded. Verification still fails for missing or malformed terminal evidence, failed upload, incomplete guest readiness, active stalls, or failed ADE/schedule finalization.

## Troubleshooting

Each scenario README has detailed troubleshooting for its specific issues:
- [Domain troubleshooting](domain-bicep/README.md#troubleshooting)
- [Cluster troubleshooting](cluster-bicep/README.md#troubleshooting)
- [Workgroup troubleshooting](workgroup-bicep/README.md#troubleshooting)

### Common across all scenarios

**Azure shows "Succeeded" but VMs aren't ready** — VM extensions report success immediately after starting background configuration. Check for `.Completed.signal` files (see [step 3 above](#3-wait-for-vm-configuration-to-finish)).

**Feature installation restarts WinRM** — Domain and Workgroup SUT/Driver orchestrators submit DSC asynchronously and poll fresh LCM status calls, so a transient `0x803381fa` WSMan disconnect is not treated as success. Completion signals are written only after the LCM reports `Success` and required features, commands, directories, tools, and applicable domain readiness checks pass. Imperative configuration is blocked while DSC prerequisites are incomplete instead of retrying the same broken environment setup indefinitely.

While DSC is active, Windows PowerShell 5.1 can report that `Start-DscConfiguration` is still in progress when status is queried. This is an expected busy response: the verifier suppresses it, continues bounded polling, and surfaces only a final LCM failure or timeout.

Workgroup and Domain SUTs compile disruptive features into dedicated feature
MOFs and apply shares, routing, remoting, and registry state through separate
post-reboot convergence MOFs. The DC similarly separates role features from
post-promotion convergence. Tool packages download to unique temporary files in
parallel and are promoted atomically into the cache; installation remains
serial and post-reboot. Role heartbeat JSON files record the current phase,
operation, elapsed time, deadline, and last checkpoint while DSC, downloads,
installers, readiness probes, or imperative setup are active.

**VM extension failure** — Check the bootstrap log on the VM:
| VM Role | Log file |
|---------|----------|
| Domain Controller | `C:\*-Package\DSC\Deploy-DC.log` |
| SUT / Cluster Node | `C:\*-Package\DSC\Deploy-SUT.log` or `Deploy-Node01.log` |
| Driver | `C:\*-Package\DSC\Deploy-Driver.log` |
| Extension bootstrap | `C:\*-setup.log` (e.g., `dc-extension-setup.log`) |

**Marketplace terms not accepted** — First deployment of a new OS image may require accepting terms:
```powershell
$image = Get-AzVMImage -Location 'West US 2' -PublisherName 'MicrosoftWindowsDesktop' `
    -Offer 'Windows-11' -Skus 'win11-25h2-ent' | Select -First 1
Get-AzMarketplaceTerms -Publisher 'MicrosoftWindowsDesktop' -Product 'Windows-11' -Name 'win11-25h2-ent' | Set-AzMarketplaceTerms -Accept
```

**Quota exceeded** — The deploy script validates quotas before deploying. If you hit limits, request a quota increase in the Azure portal or change VM sizes in the parameter files.

## Directory Structure

```
azure-automation/
├── README.md                          ← You are here
├── domain-bicep/                      ← Domain scenario (3 VMs)
│   ├── deploy.ps1                     # Phased deployment orchestrator
│   ├── phase1.bicep                   # Network + DC
│   ├── phase2.bicep                   # Driver + SUT
│   ├── modules/                       # Bicep modules (network, DC, computers)
│   ├── parameters/                    # Bicep parameter files
│   ├── DSC/                           # SUT-specific configuration scripts
│   └── README.md
├── cluster-bicep/                     ← Cluster scenario (5 VMs)
│   ├── deploy.ps1                     # Phased deployment orchestrator
│   ├── phase1.bicep                   # Network + DC + Storage
│   ├── phase2.bicep                   # Cluster Nodes + Driver
│   ├── modules/                       # Bicep modules (network, DC, storage, nodes, driver)
│   ├── parameters/                    # Bicep parameter files
│   ├── DSC/                           # Cluster-specific configuration scripts
│   ├── scripts/                       # Verify-ClusterDeployment.ps1
│   └── README.md
├── workgroup-bicep/                   ← Workgroup scenario (2 VMs)
│   ├── deploy.ps1                     # Single-phase deployment
│   ├── main.bicep                     # All resources in one template
│   ├── modules/                       # Bicep modules (network, computers)
│   ├── parameters/                    # Bicep parameter files
│   ├── DSC/                           # SUT-specific configuration scripts
│   └── README.md
└── shared/                            ← Components used by all scenarios
    ├── Deploy-Helpers.psm1            # Azure connection, storage, quota validation
    ├── Generate-ConfigJson.ps1        # Builds Config.json from bicepparam values
    ├── DSC/                           # DC and Driver orchestrators + shared scripts
    │   ├── Deploy-DC.ps1              # Domain Controller configuration orchestrator
    │   ├── Deploy-Driver.ps1          # Driver computer configuration orchestrator
    │   ├── DC-Configuration.ps1       # DSC: AD DS, DNS, LDAP, CBAC
    │   ├── Driver-Configuration.ps1   # DSC: hosts file, firewall, PS remoting
    │   ├── Invoke-DcImperativeSteps.ps1    # AD accounts, GPO, DNS records
    │   ├── Invoke-DriverImperativeSteps.ps1 # Domain join, tools, RSA keys
    │   └── Scripts/                   # Shared utilities (test execution, tools install, etc.)
    └── modules/
        └── disk-encryption-vault.bicep # Key Vault for Azure Disk Encryption
```

## Security Notes

These environments are for **testing only**:

- Windows Firewalls are disabled on all VMs
- NSGs allow broad intra-VNet access
- AutoLogon is enabled for scheduled task execution
- Default test accounts are created with deployment-provided passwords
- No public IPs are exposed (access is via Azure Bastion only)

Not suitable for production use.

## Related Documentation

- [Shared Components](shared/README.md) — How DSC packages are assembled, on-VM configuration pattern, function reference
- [File Server Test Suite User Guide](../docs/FileServerUserGuide.md) — Manual environment setup reference
- [File Server Test Design Specification](../docs/MS-FSA_ServerTestDesignSpecification.md) — Test case descriptions and pass criteria
- [Shared Helpers Module](shared/Deploy-Helpers.psm1) — Azure automation helper functions
- [Config.json Generator](shared/Generate-ConfigJson.ps1) — Dynamic test configuration from deployment parameters
- [Verify Deployment](shared/scripts/Verify-Deployment.ps1) — Universal deployment verification script
