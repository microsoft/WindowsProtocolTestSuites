# DFSC Protocol Reference

This document provides guidance for writing DFS (Distributed File System) referral protocol test cases.

## Required Using Statements

```csharp
using System;
using System.Net;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.DFSC.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

DFSC (Distributed File System: Referral Protocol) tests validate the implementation of:
- **MS-DFSC**: Distributed File System (DFS): Referral Protocol

## Directory Structure

```
DFSC/
├── Adapter/
│   ├── DfscAdapter.cs          # DFS referral operations adapter
│   ├── DFSCTestConfig.cs       # DFSC-specific configuration
│   └── IFSCAdapter.cs          # Adapter interface
├── TestSuite/
│   ├── RootReferralToDC.cs                          # Root referrals to domain controllers
│   ├── RootAndLinkReferralToDFSServer.cs            # Root and link referrals to DFS servers
│   ├── LinkReferralToDC.cs                          # Link referrals to DCs
│   ├── LinkReferralToDFSServer.cs                   # Link referrals to DFS servers
│   ├── DomainReferralToDC.cs                        # Domain namespace referrals
│   ├── DCReferralToDC.cs                            # DC referrals to other DCs
│   ├── SysvolReferralToDC.cs                        # SYSVOL share referrals
│   ├── PathNormalization.cs                         # Path normalization tests
│   ├── DFSCTestBase.cs                              # Base class for DFSC tests
│   ├── DFSCTestUtility.cs                           # Test helper utilities
│   ├── DFSCTestSuite.ptfconfig                      # Runtime configuration
│   ├── DFSCTestSuite.deployment.ptfconfig           # Environment setup
│   └── bin/obj/                                      # Build outputs
└── MS-DFSC/
    ├── DfscClient.cs                                 # DFSC protocol client
    └── [Protocol message types]                     # Message structures
```

**Key Structure Notes:**
- **RootReferralToDC.cs**: Tests root referrals with V1, V2, V3, V4 variations (referral entry types)
- **PathNormalization.cs**: Path handling and normalization validation
- **DFSCTestBase**: Common test initialization and `CheckDomainName()` validation
- **DFSCTestUtility**: Helper methods (`SendAndReceiveDFSReferral()`, `VerifyReferralResponse()`)
- Test classes follow pattern: `[ReferralType]Referral[Target]` (e.g., RootReferralToDC, LinkReferralToDFSServer)
- All tests inherit from `DFSCTestBase` which extends `CommonTestBase`

## Test Base Class

```csharp
public class DFSCTestBase : CommonTestBase
{
    protected DfscClient client;           // DFSC protocol client
    protected DFSCTestUtility utility;     // Test helpers
    protected DFSCTestConfig TestConfig;   // DFSC-specific config
    
    public override void TestInitialize()
    {
        // Creates DfscClient for sending referral requests
        // Initializes DFSCTestUtility
        // Protocol: "MS-DFSC"
    }
    
    protected void CheckDomainName()
    {
        // Validates domain name format (regex: domain.com)
        // Required for DomainRequired tests
        // Throws Inconclusive if domain validation fails
    }
}
```

**Test Setup Notes:**
- `DfscClient` uses SMB protocol to send DFSC referral requests via IOCTL
- `DFSCTestUtility` wraps protocol operations: `SendAndReceiveDFSReferral()`, `VerifyReferralResponse()`
- Tests typically interact with DFS roots/links configured on the SUT

## Test Configuration

DFSC tests extend `DFSCTestConfig` which adds:

```csharp
public enum RootTargetType { NetBios, FQDN }  // Root referral format

// Key properties:
public RootTargetType RootTargetType { get; }           // How to format root paths
public string LinkTarget { get; }                        // \NetBiosName\Share
public string RootTargetDomain { get; }                 // Domain root path
public string RootTargetStandalone { get; }             // Standalone root path
public string ValidRootPathDomain { get; }             // Complete domain root path
public string ValidRootPathStandalone { get; }         // Complete standalone root
public string ValidLinkPathDomain { get; }             // Link within domain namespace
public string ValidLinkPathStandalone { get; }         // Link within standalone namespace
public string DomainNamespace { get; }                  // DFS namespace name
public string StandaloneNamespace { get; }             // Standalone namespace name
public string DFSServerName { get; }                    // DFS server hostname
public string DomainFQDNName { get; }                   // Domain FQDN
```

**Configuration Notes:**
- `RootTargetType` determines path format: NetBios = `\\server\namespace`, FQDN = `\\server.domain.com\namespace`
- Use domain paths for `DomainRequired` tests, standalone paths for standalone DFS tests
- `LinkTarget` is the referral destination when requesting link paths

## Test Categories

```csharp
[TestCategory(TestCategories.Dfsc)]
[TestCategory(TestCategories.DomainRequired)]
[TestCategory(TestCategories.NonSmb)]
[TestCategory(TestCategories.Positive)]
[TestCategory(TestCategories.UnexpectedFields)]
```

## Common Patterns

### Referral Entry Type Variations

Tests validate each referral version (V1, V2, V3, V4):

```csharp
[TestMethod]
[TestCategory(TestCategories.Dfsc)]
[TestCategory(TestCategories.DomainRequired)]
[TestCategory(TestCategories.NonSmb)]
[TestCategory(TestCategories.Positive)]
public void RootReferralV4ToDC()
{
    CheckDomainName();
    ValidRootReferral(ReferralEntryType_Values.DFS_REFERRAL_V4);
}

private void ValidRootReferral(ReferralEntryType_Values entryType, bool isEx = false, bool containSiteName = false)
{
    uint status;
    string reqPath = TestConfig.ValidRootPathDomain;
    
    // Send referral request
    // isEx=true uses FSCTL_DFS_GET_REFERRALS_EX (extended info)
    // containSiteName=true includes site name in request
    DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(
        out status, client, entryType, reqPath, true, isEx, containSiteName);
    
    // Verify response status and structure
    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status);
    utility.VerifyReferralResponse(
        ReferralResponseType.RootTarget,  // Verify root (not link) referral
        entryType,                         // Verify matches requested version
        reqPath,                           // Original request path
        TestConfig.RootTargetDomain,       // Expected target
        respPacket);
}
```

### Invalid Path Tests

```csharp
[TestMethod]
[TestCategory(TestCategories.Dfsc)]
[TestCategory(TestCategories.DomainRequired)]
[TestCategory(TestCategories.UnexpectedFields)]
public void InvalidRootReferralToDC()
{
    CheckDomainName();
    string invalidPath = string.Format(@"\{0}\{1}", 
        TestConfig.DomainFQDNName, "InvalidComponent");
    
    utility.SendAndReceiveDFSReferral(
        out var status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, 
        invalidPath, true);
    
    BaseTestSite.Assert.AreEqual(
        Smb2Status.STATUS_NO_SUCH_FILE, status);
}
```

## Test File Organization

DFSC tests are organized by referral type:
- `RootReferralToDC.cs` - Root referrals to domain controllers (v1, v2, v3, v4)
- `RootAndLinkReferralToDFSServer.cs` - Root and link referrals to DFS servers
- `LinkReferralToDC.cs` - Link referrals to DCs
- `DomainReferralToDC.cs` - Domain namespace referrals
- `PathNormalization.cs` - Path normalization tests

## Key Concepts

- **RootTargetType**: Determines if root paths use NetBIOS name or FQDN format
- **DfscClient**: Protocol client for sending referral requests via SMB IOCTL
- **DFSCTestUtility**: Helper methods:
  - `SendAndReceiveDFSReferral()` - sends request, receives response packet
  - `VerifyReferralResponse()` - validates response structure and targets
  - `Connect()` - connect to the DC or DFS server without sending a request (reuse one connection across multiple sends)
  - `SendReferralWithMaxOutput()` - send a referral over an already-connected client with an explicit `MaxOutputResponse`, returning the response and the IOCTL `OutputCount`. Use this for referral output-buffer boundary tests (e.g. `STATUS_BUFFER_OVERFLOW` when the buffer is too small; MS-SMB2 3.3.5.15.2 / Appendix A note 384). SMB2/SMB3 transport only.
- **Referral Entry Types**: V1, V2, V3, V4 format variations for backward compatibility
  - V1: Basic referral (section 7.2.1)
  - V2: Extended with TTL and flavor (section 7.2.2)
  - V3: Adds site awareness (section 7.2.3)
  - V4: Namespace and domain referrals (section 7.2.4)
- **ReferralResponseType**: Distinguishes root referrals from link referrals in verification
- **Domain Validation**: Tests requiring domain environment check domain name regex pattern
- **EX Flag**: Extended referral request (includes client info like site, cluster)

## Type Declarations (Where to Find/Add Definitions)

### DFSC Enums and Structures
**File**: `ProtoSDK/MS-DFSC/DfscMessage.cs`

Search for these type definitions (use `public enum` prefix):

| Type | Purpose |
|------|---------|
| `ReferralHeaderFlags` | Referral header flags |
| `ReferralEntryFlags_Values` | Referral entry flags |
| `ReferralEntryType_Values` | Referral entry type enum |
| `REQ_GET_DFS_REFERRAL_RequestFlags` | Request flags enum |
| `ReferralResponseType` | Response type enum |

### DFSC Packets
**Files in**: `ProtoSDK/MS-DFSC/`

| File | Purpose |
|------|---------|
| `DfscReferralRequestPacket.cs` | Standard referral request |
| `DfscReferralRequestEXPacket.cs` | Extended referral request |
| `DfscReferralResponsePacket.cs` | Referral response |

### DFS FSCTL Codes
**File**: `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`

Search for these in the `CtlCode_Values` enum:

| Constant | Purpose |
|----------|---------|
| `FSCTL_DFS_GET_REFERRALS` | Get DFS referrals |
| `FSCTL_DFS_GET_REFERRALS_EX` | Get DFS referrals (extended) |

**To add new DFSC types**: Add to `ProtoSDK/MS-DFSC/DfscMessage.cs`