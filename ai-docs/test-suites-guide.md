# Test Suites Guide

## Overview

There are approximately 2,981 `[TestMethod]`-attributed test cases spread across 375 test files in this repository. Each test suite exercises a specific protocol family against a live System Under Test (SUT).

All test suites:
- Use **MSTest v2** (`Microsoft.VisualStudio.TestTools.UnitTesting`)
- Reference **Protocol Test Framework v2.6** (`Microsoft.Protocols.TestTools`) as a NuGet package
- Use `.ptfconfig` XML files for environment configuration
- Follow a consistent class hierarchy: `TestClassBase` (PTF) → suite-specific base → protocol-specific base → test class

## Test Class Hierarchy

```
TestClassBase (PTF NuGet)
└── CommonTestBase (FileServer) / RdpTestClassBase (RDP) / TraditionTestBase (Kerberos) / etc.
    └── SMB2TestBase / FsaTestBase / RdpbcgrTestBase / etc.
        └── Negotiation / BasicFileIO / Rdpbcgr_ClientTestSuite / etc.
```

`TestClassBase` provides:
- `BaseTestSite` (the PTF `ITestSite`) — logging, assertions, adapter resolution
- `TestClassBase.Initialize(TestContext)` — must be called from `[ClassInitialize]`
- `TestClassBase.Cleanup()` — must be called from `[ClassCleanup]`
- Virtual `TestInitialize()` and `TestCleanup()` called before/after each test

## Test Suite Anatomy

Every test suite directory under `TestSuites/<Name>/src/` has:

```
src/
├── build.ps1           Build and publish script
├── build.sh            Linux build script
├── <Suite>.sln         Solution file
├── Common/             (if multi-project) Shared adapter and config types
│   ├── Adapter/        Adapter interfaces and implementations
│   └── TestSuite/      CommonTestBase and shared config
├── <Protocol>/
│   ├── TestSuite/      [TestMethod] classes
│   └── Adapter/        Protocol-specific adapter (if present)
├── Batch/              .ps1 and .sh batch runner scripts
├── Deploy/             Deployment artifacts (license, etc.)
└── Plugin/             PTM auto-detection plugin
```

---

## FileServer Test Suite

**Path:** `TestSuites/FileServer/src/`
**Solution:** `FileServer.sln`
**Protocols tested:** MS-SMB2, MS-DFSC, MS-SWN, MS-FSRVP, MS-FSA, MS-FSCC, MS-RSVD, MS-SQOS

### Structure

```
FileServer/src/
├── Common/Adapter/     Smb2FunctionalClient, ISutCommonControlAdapter, ISutProtocolControlAdapter
├── Common/TestSuite/   CommonTestBase, CommonTypes, ptfconfig
├── SMB2/TestSuite/     SMB2 test cases organized by feature area
├── SMB2/Adapter/       SMB2TestConfig
├── FSA/TestSuite/      File System Access (MS-FSA) test cases
├── DFSC/               DFS Referral test cases
├── FSRVP/              File Server Remote VSS test cases
├── RSVD/               Remote Shared Virtual Disk test cases
├── SQOS/               Storage Quality of Service test cases
├── ServerFailover/     Server failover scenarios
└── Auth/               Authentication (Kerberos, NTLM) and Authorization test cases
```

### Azure Automation

`TestSuites/FileServer/azure-automation/` contains Bicep and PowerShell deployment entry points for Domain, Workgroup, and Cluster environments. Shared infrastructure belongs under `shared/modules/`; for example, `bastion.bicep` deploys Bastion independently from core networking so VM provisioning is not serialized behind it. Domain deployment separates member infrastructure (`domain-computers.bicep`) from guest extensions (`domain-computer-extensions.bicep`), allowing VM provisioning to overlap DC configuration while keeping domain join behind the DC readiness gate. Reuse these modules rather than embedding equivalent resources in scenario network modules.

### Key Classes

| Class | File | Purpose |
|---|---|---|
| `CommonTestBase` | `Common/TestSuite/CommonTestBase.cs` | Abstract base; sets up adapters, cleans test files/dirs |
| `SMB2TestBase` | `SMB2/TestSuite/SMB2TestBase.cs` | Adds `SMB2TestConfig`, sets `DefaultProtocolDocShortName` |
| `Smb2FunctionalClient` | `Common/Adapter/Smb2FunctionalClient.cs` | High-level SMB2 client wrapping the raw ProtoSDK `Smb2Client` |
| `TestConfigBase` | `Common/Adapter/TestConfigBase.cs` | Reads `.ptfconfig` properties; base for all config classes |
| `SMB2TestConfig` | `SMB2/Adapter/SMB2TestConfig.cs` | SMB2-specific config (dialects, timeouts, etc.) |

### SMB2 Test Directories

| Directory | What it tests |
|---|---|
| `SMB2/TestSuite/Negotiate/` | Protocol negotiation, dialect selection |
| `SMB2/TestSuite/SessionMgmt/` | Session setup, logoff, reconnect |
| `SMB2/TestSuite/TreeMgmt/` | Tree connect/disconnect |
| `SMB2/TestSuite/CreateClose/` | File/directory create and close |
| `SMB2/TestSuite/Basic/` | Read, write, query info |
| `SMB2/TestSuite/Signing/` | Message signing |
| `SMB2/TestSuite/Encryption/` | Session/share-level encryption |
| `SMB2/TestSuite/DurableHandle/` | Durable open handles (v1 and v2) |
| `SMB2/TestSuite/Leasing/` | SMB2/3 leasing |
| `SMB2/TestSuite/OpLock/` | Opportunistic locks |
| `SMB2/TestSuite/MultipleChannel/` | Multi-channel scenarios |
| `SMB2/TestSuite/Compression/` | SMB3 compression |
| `SMB2/TestSuite/Replay/` | Replay protection |
| `SMB2/TestSuite/IOCTL/` | FSCTL operations |
| `SMB2/TestSuite/HVRS/` | Hyper-V replica set |
| `SMB2/TestSuite/AppInstanceId/` | Application instance identifier |

---

## RDP Test Suites

### RDP Client Test Suite

**Path:** `TestSuites/RDP/Client/src/`
**Solution:** `RDP_Client.sln`
**Protocols tested:** MS-RDPBCGR, MS-RDPEDISP, MS-RDPEDYC, MS-RDPEGFX, MS-RDPEGT, MS-RDPEI, MS-RDPEMT, MS-RDPEUDP, MS-RDPEUSB, MS-RDPEVOR, MS-RDPRFX

The RDP Client suite acts as the **RDP server** and tests the client. It uses `RDPSUTControlAgent` (a separate agent deployed on the SUT) to trigger client connections.

**Key Classes:**
- `RdpTestClassBase` (`TestSuite/RdpTestClassBase.cs`) — base class; initializes `TestConfig`, `IRDPSUTControlAdapter`

**Test directories in `TestSuite/`:**
`RDPBCGR/`, `RDPEDISP/`, `RDPEGFX/`, `RDPEI/`, `RDPEMT/`, `RDPEUDP/`, `RDPEUSB/`, `RDPEVOR/`, `RDPRFX/`

### RDP Server Test Suite

**Path:** `TestSuites/RDP/Server/src/`
**Solution:** `RDP_Server.sln`
**Protocols tested:** MS-RDPBCGR, MS-RDPEDYC, MS-RDPEMT, MS-RDPELE

The RDP Server suite acts as the **RDP client** and tests the server.

**Key Classes:**
- `RdpTestClassBase` (`TestSuite/RdpTestClassBase.cs`) — same base class as client suite

**Test directories:** `RDPBCGR/`, `RDPEDYC/`, `RDPELE/`, `RDPEMT/`

---

## Kerberos Test Suite

**Path:** `TestSuites/Kerberos/src/`
**Solution:** `Kerberos_Server.sln`
**Protocols tested:** MS-KILE, MS-KKDCP, MS-PAC

**Structure:**
```
src/
├── Adapter/        KerberosTestConfig, KerberosAdapter
├── TestSuite/
│   ├── TraditionTestBase.cs   Base class
│   ├── KILE/                  MS-KILE test cases
│   ├── KKDCP/                 MS-KKDCP (Kerberos proxy) test cases
│   ├── RC4/                   RC4 encryption tests
│   ├── Claim/                 Claims-based authorization tests
│   └── AZOD/                  Azure Object Discovery tests
```

**Key Classes:**
- `TraditionTestBase` — sets up KerberosAdapter and test site
- Playlists (`.PLAYLIST` files) define which tests to run for specific OS/domain configurations (e.g., `SingleRealm_KDC_2012R2.PLAYLIST`)

---

## MS-SMBD Test Suite

**Path:** `TestSuites/MS-SMBD/src/`
**Solution:** `MS-SMBD_Server.sln`
**Protocols tested:** MS-SMBD (SMB2 over RDMA), MS-SMB2

**Structure:**
```
src/
├── Adapter/
│   ├── SmbdAdapter.cs          Main SMBD adapter
│   ├── Smb2OverSmbdTestClient.cs
│   └── TestConfig.cs
└── TestSuite/
    ├── SmbdTestBase.cs         Base for pure SMBD tests
    ├── Smb2OverSmbdTestBase.cs Base for SMB2-over-SMBD tests
    ├── SMBD/                   SMBD connection/negotiation tests
    └── SMB2overSMBD/           SMB2 operations carried over SMBD
```

Requires an RDMA-capable NIC on both client and server. For Windows: uses the C++/CLI `RDMA` adapter in ProtoSDK. For Linux: uses the `RdmaLinux` adapter.

---

## BranchCache Test Suite

**Path:** `TestSuites/BranchCache/src/`
**Solution:** `BranchCache.sln`
**Protocols tested:** MS-PCCRTP, MS-PCCRR, MS-PCHC, MS-PCCRC

---

## ADFamily Test Suite

**Path:** `TestSuites/ADFamily/src/`
**Solution:** `AD_Server.sln`
**Protocols tested:** MS-ADA1, MS-ADA2, MS-ADA3, MS-ADLS, MS-ADSC, MS-ADTS, MS-APDS, MS-DRSR, MS-FRS2, MS-LSAD, MS-LSAT, MS-SAMR, MS-NRPC

The largest test suite in the repo. Has C++ native components for some AD protocol operations. Contains many sub-directories under `TestSuite/` corresponding to each protocol.

---

## MS-WSP Test Suite

**Path:** `TestSuites/MS-WSP/src/`
**Solution:** `MS-WSP_Server.sln`
**Protocols tested:** MS-WSP (Windows Search Protocol)

---

## MS-XCA Test Suite

**Path:** `TestSuites/MS-XCA/src/`
**Solution:** `MS-XCA.sln`
**Protocols tested:** MS-XCA (Xpress Compression Algorithm)

This is a pure algorithm test suite — it does not require a network SUT. Tests run the compression/decompression algorithms against known test vectors.

---

## MS-ADFSPIP, MS-AZOD, MS-ADOD Test Suites

These smaller suites follow the same pattern but target specific AD-related protocols. Each has `src/Adapter/`, `src/TestSuite/`, and deployment scripts.

---

## How a Test Case Is Structured

### Lifecycle

```csharp
[TestClass]
public class Negotiation : SMB2TestBase
{
    private Smb2FunctionalClient client;

    [ClassInitialize()]
    public static void ClassInitialize(TestContext testContext)
    {
        TestClassBase.Initialize(testContext);     // PTF initialization
    }

    [ClassCleanup()]
    public static void ClassCleanup()
    {
        TestClassBase.Cleanup();
    }

    protected override void TestInitialize()
    {
        base.TestInitialize();                    // sets up testConfig, adapters
        client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
        client.ConnectToServer(TestConfig.UnderlyingTransport,
                               TestConfig.SutComputerName,
                               TestConfig.SutIPAddress);
    }

    protected override void TestCleanup()
    {
        client.Disconnect();
        base.TestCleanup();
    }

    [TestMethod]
    [TestCategory(TestCategories.Bvt)]
    [TestCategory(TestCategories.Smb21)]
    [TestCategory(TestCategories.Negotiate)]
    [Description("Verify server handles NEGOTIATE with SMB2 wildcard dialect")]
    public void BVT_Negotiate_Compatible_Wildcard()
    {
        BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send negotiate with wildcard");
        uint status = client.MultiProtocolNegotiate(
            new[] { "SMB 2.002", "SMB 2.???" },
            (header, response) => {
                BaseTestSite.Assert.AreEqual(DialectRevision.Smb2Wildcard,
                                             response.DialectRevision,
                                             "Expected wildcard dialect");
            });
    }
}
```

### Test Categories

Test methods use `[TestCategory]` attributes to classify them. Common categories (defined in `TestCategories.cs`):
- `Bvt` — Basic Verification Test; smoke test, always run
- `Smb2002`, `Smb21`, `Smb30`, `Smb302`, `Smb311` — dialect-specific
- `Negotiate`, `Signing`, `Encryption`, `DurableHandle`, etc. — feature area
- `NonSmb` — tests that exercise authentication or transport but not SMB directly

The `.PLAYLIST` files in some suites (Kerberos) define curated sets of tests for specific configurations.

### Logging

Use `BaseTestSite.Log.Add(LogEntryKind.TestStep, "message")` for structured test step logging. Available kinds: `TestStep`, `Checkpoint`, `Comment`, `Debug`, `Warning`, `CheckFailed`, `CheckSucceeded`.

Assertions use `BaseTestSite.Assert.*` (not `Assert.*` directly) so that assertion failures are captured by PTF and appear in the test log with context.

### Adapters

Adapters are retrieved from the test site:
```csharp
var adapter = BaseTestSite.GetAdapter<ISutProtocolControlAdapter>();
```

Adapter implementations are specified in `.ptfconfig`:
```xml
<Adapters>
  <Adapter xsi:type="powershell" name="ISutCommonControlAdapter" scriptdir=".\SutCommonControlAdapter" />
  <Adapter xsi:type="managed"   name="ISutProtocolControlAdapter"
           adaptertype="Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter.SutProtocolControlAdapter" />
</Adapters>
```

Type `powershell` — PTF auto-generates an adapter that delegates each method to a PowerShell script with the same name as the method.
Type `managed` — PTF instantiates the named C# class.

---

## How to Add a New Test Case

### 1. Find the right test class

Navigate to the protocol area: e.g., for a new SMB2 encryption test, go to `TestSuites/FileServer/src/SMB2/TestSuite/Encryption/`.

If a class already exists for that feature, add your method there. Otherwise, create a new class file.

### 2. Write the test method

```csharp
[TestMethod]
[TestCategory(TestCategories.Smb311)]
[TestCategory(TestCategories.Encryption)]
[Description("Verify server rejects unencrypted traffic on an encryption-required share.")]
public void Encryption_RejectUnencryptedAccess()
{
    // Arrange
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to encryption-required share without encryption.");

    // Act & Assert
    uint status = client.TreeConnect(
        TestConfig.EncryptionRequiredShare,
        out uint treeId,
        checker: (header, response) => {
            BaseTestSite.Assert.AreNotEqual((uint)Smb2Status.STATUS_SUCCESS, header.Status,
                "Server should reject unencrypted access to encryption-required share.");
        });
}
```

### 3. Update documentation

Per `CONTRIBUTING.md`: if adding a new test case, update the corresponding **Test Design Specification** (found in `TestSuites/<Suite>/docs/`).

### 4. Add to ptfconfig if new config properties needed

Add new `<Property>` elements to `<Suite>.ptfconfig` and parse them in the relevant `TestConfig` class.

### 5. Build and run

```powershell
cd TestSuites/FileServer/src
./build.ps1
```

Then run from the output `drop/TestSuites/FileServer/Bin/` directory using vstest or PTM.
