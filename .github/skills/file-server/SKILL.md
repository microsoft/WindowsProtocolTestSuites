---
name: file-server
description: ALWAYS LOAD THIS SKILL when working with FileServer, SMB, SMB2, SMB3, CIFS, file sharing, MS-SMB2, MS-FSCC, MS-FSA, MS-DFSC, MS-FSRVP, MS-RSVD, MS-SQOS, or any file server protocol test implementation. This skill assists with writing FileServer protocol test cases, implementing new test scenarios, discovering reusable libraries, adapters, and transport implementations (TCP, QUIC, NetBIOS, RDMA). CRITICAL--Before implementing any test, first classify the domain by analyzing concepts (File*, Fs*, FileSystemAttributes = FSA; NEGOTIATE, SESSION_SETUP, SMB commands = SMB2). Test scenarios from MS-SMB2 that involve File*/Fs* concepts belong to the FSA test suite. Provides test case patterns, storage locations, ProtoSDK usage, and test execution guidance. Protocols under FileServer/src include Auth, Common, DFSC, FSA, FSAModel, FSRVP, RSVD, ServerFailover, SMB2, SMB2Model, and SQOS.
license: MIT
metadata:
  author: Microsoft
  version: "1.2"
---

# File Server Test Suite Skill

## MANDATORY WORKFLOW (Follow In Order)

**You MUST complete these steps IN ORDER. Do not skip ahead.**

1. ☐ **CLASSIFY DOMAIN FIRST** - Analyze concepts in the scenario (see Test Scenario Domain Classification)
2. ☐ **LOAD THE CORRECT REFERENCE** - Based on domain classification
3. ☐ **THEN search for existing tests** - Only in the correct domain folder
4. ☐ **Implement or modify** - Using the domain-specific patterns

> **CRITICAL STOP**: Do NOT use `grep_search` or `file_search` until Step 1 is complete.
> 
> **MS-SMB2 Document ≠ SMB2 Domain**: If the user mentions "MS-SMB2 section X.X.X", DO NOT assume SMB2 domain.
> First extract the CONCEPTS from the scenario text. If you see `File*`, `Fs*`, `FileSystemAttributes`,
> `FileFsAttributeInformation`, or any MS-FSCC structure names → it's **FSA domain**, regardless of the
> MS-SMB2 section number.

---

## Common Mistakes to Avoid

| Mistake | Why It's Wrong | Correct Approach |
|---------|---------------|------------------|
| Searching for tests before classifying domain | grep may find tests in wrong domain (e.g., SMB2 duplicates) | Always classify domain FIRST based on concepts |
| Assuming MS-SMB2 section = SMB2 domain | MS-SMB2 often references MS-FSCC structures which are FSA domain | Check the CONCEPTS, not the document source |
| Finding a test and assuming it's correct location | Duplicate tests may exist in wrong domains | Verify test location matches domain classification |

---

This skill helps you write FileServer protocol test cases by:
1. **Classifying the domain** - Determine FSA vs SMB2 vs other based on concepts
2. **Discovering reusable components** - Libraries, adapters, and transports you MUST reuse
3. **Providing test patterns** - How to structure and where to store test cases
4. **Guiding test execution** - Environment configuration and execution methods

## Protocol-Specific References

For detailed guidance on specific protocols, load the appropriate reference based on **domain classification** (see Section 4):

| Protocol | Reference | Use When (Domain Indicators) |
|----------|-----------|------------------------------|
| FSA | [references/fsa.md](references/fsa.md) | Concepts: `File*`, `Fs*`, `FileSystemAttributes`, `FileFsAttributeInformation`, `FileBasicInformation`, file system queries, MS-FSCC structures |
| SMB2/SMB3 | [references/smb2.md](references/smb2.md) | Concepts: `NEGOTIATE`, `SESSION_SETUP`, `TREE_CONNECT`, `CREATE`, `CLOSE`, SMB commands, dialects, signing, encryption, compounding |
| FSA Model | [references/fsamodel.md](references/fsamodel.md) | Writing FSA model-based/state-machine tests |
| DFSC | [references/dfsc.md](references/dfsc.md) | DFS, referral, namespace concepts |
| FSRVP | [references/fsrvp.md](references/fsrvp.md) | VSS, shadow copy, snapshot, backup concepts |
| RSVD | [references/rsvd.md](references/rsvd.md) | Virtual disk, VHD, VHDX, shared virtual disk concepts |
| SQOS | [references/sqos.md](references/sqos.md) | Storage QoS, policy, bandwidth, IOPS concepts |
| Auth | [references/auth.md](references/auth.md) | Authentication, Kerberos, NTLM, credentials concepts |
| Failover | [references/serverfailover.md](references/serverfailover.md) | Failover, cluster, witness, persistent handle concepts |
| Common | [references/common.md](references/common.md) | Understanding shared infrastructure |

## Test Scenario Domain Classification (CRITICAL - READ FIRST)

**Before implementing any test scenario, you MUST determine its domain based on the CONCEPTS involved, NOT the document section it came from.**

> **CRITICAL STOP**: Do NOT use `grep_search` or `file_search` until Step 1 is complete.
> Finding tests via grep does not validate their correct location.

### Step 1: Identify the Domain by Concepts

Analyze the test scenario for these keyword patterns:

| If scenario mentions... | Domain | Reference to Load |
|------------------------|--------|-------------------|
| `File*`, `Fs*`, `FileSystemAttributes`, `FileFsAttributeInformation`, `FileBasicInformation`, `FileStandardInformation`, `FileInformation*`, `FsInfo*`, `FsControl*`, file system queries, file attributes, directory information | **FSA** | [references/fsa.md](references/fsa.md) |
| `NEGOTIATE`, `SESSION_SETUP`, `TREE_CONNECT`, `CREATE`, `CLOSE`, `READ`, `WRITE`, `IOCTL`, `LOCK`, `CANCEL`, SMB commands, dialects, signing, encryption, compounding, multichannel, durable handles, leasing, oplocks | **SMB2** | [references/smb2.md](references/smb2.md) |
| DFS, referral, namespace, domain-based DFS, standalone DFS | **DFSC** | [references/dfsc.md](references/dfsc.md) |
| VSS, shadow copy, snapshot, backup | **FSRVP** | [references/fsrvp.md](references/fsrvp.md) |
| Virtual disk, VHD, VHDX, shared virtual disk | **RSVD** | [references/rsvd.md](references/rsvd.md) |
| Storage QoS, policy, bandwidth, IOPS | **SQOS** | [references/sqos.md](references/sqos.md) |
| Authentication, Kerberos, NTLM, credentials | **Auth** | [references/auth.md](references/auth.md) |
| Failover, cluster, witness, persistent handles | **Failover** | [references/serverfailover.md](references/serverfailover.md) |

### Step 2: Understand the Protocol Document vs Test Domain Relationship

**IMPORTANT**: The MS-SMB2 document describes SMB2 server behavior, but many sections reference **MS-FSCC** (File System Control Codes) structures. When the test scenario involves:

- **File system information classes** (e.g., `FileFsAttributeInformation`, `FileFsVolumeInformation`) → **FSA domain**
- **File information classes** (e.g., `FileBasicInformation`, `FileStandardInformation`) → **FSA domain**
- **FSCTL operations** (e.g., `FSCTL_GET_INTEGRITY_INFORMATION`) → **FSA domain**
- **SMB2 command behavior** (e.g., `NEGOTIATE`, `SESSION_SETUP`, compound requests) → **SMB2 domain**

**Example**: MS-SMB2 section 3.3.5.20.2 describes clearing bits in `FileFsAttributeInformation`. Even though it's in the SMB2 spec, the concepts (`FileSystemAttributes`, `FileFsAttributeInformation`) are **FSA domain** because they test file system behavior exposed through SMB2.

### Step 3: Check Test Location Based on Domain

| Domain | Test Location | Base Class |
|--------|---------------|------------|
| **FSA** | `TestSuites/FileServer/src/FSA/TestSuite/` | `PtfTestClassBase` |
| **SMB2** | `TestSuites/FileServer/src/SMB2/TestSuite/[Feature]/` | `SMB2TestBase` |
| **DFSC** | `TestSuites/FileServer/src/DFSC/TestSuite/` | `DFSCTestBase` |
| **FSRVP** | `TestSuites/FileServer/src/FSRVP/TestSuite/` | `SMB2TestBase` |
| **RSVD** | `TestSuites/FileServer/src/RSVD/TestSuite/` | `RSVDTestBase` |
| **SQOS** | `TestSuites/FileServer/src/SQOS/TestSuite/` | `SqosTestBase` |
| **Auth** | `TestSuites/FileServer/src/Auth/TestSuite/` | `AuthenticationTestBase` / `AuthorizationTestBase` |
| **Failover** | `TestSuites/FileServer/src/ServerFailover/TestSuite/` | `ServerFailoverTestBase` |

### Quick Domain Classification Examples

| Scenario Description | Key Concepts | Domain |
|---------------------|--------------|--------|
| "Clear FILE_SUPPORTS_USN_JOURNAL bit in FileFsAttributeInformation" | `File*`, `Fs*`, attributes | **FSA** |
| "Server SHOULD clear FileSystemAttributes bits" | `FileSystemAttributes`, `File*` | **FSA** |
| "Query FileBasicInformation and verify timestamps" | `FileBasicInformation`, `File*` | **FSA** |
| "NEGOTIATE request with signing capability" | `NEGOTIATE`, SMB command | **SMB2** |
| "Durable handle reconnect after disconnect" | Durable handle, SMB feature | **SMB2** |
| "Compound CREATE and CLOSE request" | Compounding, SMB commands | **SMB2** |

### Pre-Implementation Checklist

Before writing or modifying any test, verify:
- [ ] I identified the domain based on **concepts** (File*, Fs* = FSA; NEGOTIATE, SESSION_SETUP = SMB2)
- [ ] I did NOT assume the domain from the MS-SMB2 document section number
- [ ] I searched for existing tests **only** in the classified domain folder
- [ ] The test location matches the domain classification table above

---

## 1. Reusable Components (MUST REUSE)

### ProtoSDK Libraries

**Location**: `ProtoSDK/` - Protocol implementations you MUST reuse.

| Library | Path | Purpose |
|---------|------|---------|
| MS-SMB2 | `ProtoSDK/MS-SMB2/` | SMB2/SMB3 client/server |
| MS-DFSC | `ProtoSDK/MS-DFSC/` | DFS referral protocol |
| MS-FSCC | `ProtoSDK/MS-FSCC/` | File system control codes |
| MS-FSRVP | `ProtoSDK/MS-FSRVP/` | File Server VSS Protocol |
| MS-RSVD | `ProtoSDK/MS-RSVD/` | Remote Shared Virtual Disk |
| MS-SQOS | `ProtoSDK/MS-SQOS/` | Storage QoS Protocol |
| TransportStack | `ProtoSDK/TransportStack/` | TCP, NetBIOS transport |
| SspiLib | `ProtoSDK/SspiLib/` | Kerberos, NTLM security |

### Transport Types

**Defined in**: `ProtoSDK/MS-SMB2/CustomTypes.cs`

```csharp
public enum Smb2TransportType
{
    Tcp,      // TCP transport (port 445)
    NetBios,  // NetBIOS transport (port 139)
    Rdma,     // RDMA transport (MS-SMBD)
    Quic,     // QUIC transport (port 443)
}
```

### Test Adapters

**Location**: `TestSuites/FileServer/src/Common/Adapter/`

| Adapter | Purpose |
|---------|---------|
| `Smb2FunctionalClient` | **Primary SMB2 client** - USE THIS for all SMB2 operations |
| `TestConfigBase` | Configuration access (server, credentials, features) |
| `ISutProtocolControlAdapter` | SUT file/directory operations |

### Test Base Classes

| Domain | Base Class | Notes |
|--------|------------|-------|
| FSA | `PtfTestClassBase` | All FSA test classes use `public partial class XxxTestCases : PtfTestClassBase` |
| SMB2/SMB3 | `SMB2TestBase` | `public class Xxx : SMB2TestBase` |
| DFSC | `DFSCTestBase` | `public class Xxx : DFSCTestBase` |
| FSRVP | `SMB2TestBase` | FSRVP tests inherit from SMB2TestBase (e.g., `VSSOperateShadowCopySet : SMB2TestBase`) |
| RSVD | `RSVDTestBase` | `public class Xxx : RSVDTestBase` |
| SQOS | `SqosTestBase` | `SqosTestBase : CommonTestBase` |
| Auth | `AuthenticationTestBase` or `AuthorizationTestBase` | Both inherit from `CommonTestBase` |
| Failover | `ServerFailoverTestBase` | `ServerFailoverTestBase : SMB2TestBase` |

---

## 2. Test Case Patterns

### Where to Store Test Cases

| Protocol | Directory |
|----------|-----------|
| SMB2 | `TestSuites/FileServer/src/SMB2/TestSuite/[Feature]/` |
| FSA | `TestSuites/FileServer/src/FSA/TestSuite/` |
| DFSC | `TestSuites/FileServer/src/DFSC/TestSuite/` |
| FSRVP | `TestSuites/FileServer/src/FSRVP/TestSuite/` |
| RSVD | `TestSuites/FileServer/src/RSVD/TestSuite/` |
| SQOS | `TestSuites/FileServer/src/SQOS/TestSuite/` |
| Auth | `TestSuites/FileServer/src/Auth/TestSuite/` |

### Minimal Test Template

```csharp
using System;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class YourFeatureTests : SMB2TestBase
    {
        private Smb2FunctionalClient client;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            if (client != null)
            {
                try { client.Disconnect(); }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug,
                        "Disconnect exception: {0}", ex.ToString());
                }
            }
            base.TestCleanup();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [Description("Test description.")]
        public void BVT_Feature_Scenario()
        {
            // Use Smb2FunctionalClient for all SMB2 operations
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport,
                TestConfig.SutComputerName, TestConfig.SutIPAddress);

            // See protocol-specific reference for detailed patterns
        }
    }
}
```

### Test Method Naming

Format: `[Category]_[Feature]_[Scenario]`

Examples: `BVT_Negotiate_SigningEnabled`, `BVT_Encryption_EncryptedShare`

### Required Test Categories

```csharp
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

// Test type (pick one)
[TestCategory(TestCategories.Bvt)]    // Build verification
[TestCategory(TestCategories.Model)]  // Model-based

// SMB dialect (for SMB2 tests)
[TestCategory(TestCategories.Smb2002)]
[TestCategory(TestCategories.Smb311)]

// Feature category
[TestCategory(TestCategories.Negotiate)]
[TestCategory(TestCategories.Encryption)]

// Environment (if required)
[TestCategory(TestCategories.DomainRequired)]
```

---

## 3. Test Execution

### Configuration Files

| File | Purpose |
|------|---------|
| `CommonTestSuite.deployment.ptfconfig` | Environment settings (server, credentials) |
| `[Protocol]_ServerTestSuite.ptfconfig` | Protocol-specific settings |

### Key Settings in deployment.ptfconfig

```xml
<Property name="UnderlyingTransport" value="Tcp"/>  <!-- Tcp, NetBios, Quic -->
<Property name="SutComputerName" value="server.contoso.com"/>
<Property name="SutIPAddress" value="192.168.1.11"/>
<Property name="BasicFileShare" value="SMBBasic"/>
<Property name="MaxSmbVersionSupported" value="Smb311"/>
```

### Running Tests

```powershell
# From TestSuites/FileServer/src/Batch/
.\RunTestCasesByFilter.ps1 -Filter "TestCategory=BVT"
.\RunTestCasesByFilter.ps1 -Filter "TestCategory=BVT&TestCategory=Smb311"

# Dry run (list tests without running)
.\RunTestCasesByFilter.ps1 -Filter "TestCategory=BVT" -DryRun
```

### Checking Prerequisites

```csharp
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

// Check dialect
TestConfig.CheckDialect(DialectRevision.Smb311);

// Check feature (skips if unsupported)
BaseTestSite.Assume.IsTrue(TestConfig.IsEncryptionSupported,
    "Test requires encryption support");
```

## 4. Decision Tree

### Which reference should I load?

**First, classify the domain using Step 1 above, then:**

1. **FSA domain (File*, Fs*, file system concepts)?** → Load [references/fsa.md](references/fsa.md)
2. **SMB2 domain (SMB commands, protocol features)?** → Load [references/smb2.md](references/smb2.md)
3. **DFS referral tests?** → Load [references/dfsc.md](references/dfsc.md)
4. **VSS/shadow copy tests?** → Load [references/fsrvp.md](references/fsrvp.md)
5. **Virtual disk tests?** → Load [references/rsvd.md](references/rsvd.md)
6. **Storage QoS tests?** → Load [references/sqos.md](references/sqos.md)
7. **Authentication tests?** → Load [references/auth.md](references/auth.md)
8. **Failover tests?** → Load [references/serverfailover.md](references/serverfailover.md)
9. **Understanding shared infrastructure?** → Load [references/common.md](references/common.md)
