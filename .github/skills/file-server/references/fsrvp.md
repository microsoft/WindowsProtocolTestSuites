# FSRVP Protocol Reference

This document provides guidance for writing File Server Remote VSS Protocol test cases.

## Required Using Statements

```csharp
using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsrvp;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.FSRVP.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

FSRVP tests validate the implementation of:
- **MS-FSRVP**: File Server Remote VSS Protocol

Used for creating and managing shadow copies (snapshots) of file shares.

## Directory Structure

```
FSRVP/
├── TestSuite/
│   ├── VSSSetContext.cs                      # Shadow copy context configuration tests
│   ├── VSSOperateShadowCopySet.cs            # Shadow copy operations (create, expose, delete)
│   ├── VSSAbortShadowCopySet.cs              # Abort/cleanup operations
│   ├── MS-FSRVP_ServerTestSuite.csproj
│   ├── MS-FSRVP_ServerTestSuite.ptfconfig
│   └── MS-FSRVP_ServerTestSuite.deployment.ptfconfig
└── [Adapter files referenced from Common]
```

**Key Test Files:**
- **VSSSetContext.cs**: Tests shadow copy context values (BACKUP, APP_ROLLBACK, NAS_ROLLBACK, FILE_SHARE_BACKUP)
- **VSSOperateShadowCopySet.cs**: Tests full shadow copy lifecycle (prepare, commit, expose, recover)
- **VSSAbortShadowCopySet.cs**: Tests error handling and abort scenarios
- All test classes inherit from `SMB2TestBase` (uses SMB2 for RPC transport)

**Test Categories Used:**
- `FsrvpNonClusterRequired` - For tests NOT requiring cluster environment (most common)
- `Fsrvp` - For cluster-specific tests
- `NonSmb` - Shadow copy operations (RPC-based, not SMB file operations)

## Test Base Class

FSRVP tests inherit from `SMB2TestBase` (RPC transport over SMB2):

```csharp
public partial class VSSOperateShadowCopySet : SMB2TestBase
{
    // Inherits from SMB2TestBase for SMB2 connection management
    // Shadow copy operations use RPC protocol over SMB named pipes
    
    [TestMethod]
    [TestCategory(TestCategories.Bvt)]
    [TestCategory(TestCategories.FsrvpNonClusterRequired)]
    [TestCategory(TestCategories.NonSmb)]
    [Description("Check if server supports FSRVP_CTX_BACKUP context.")]
    public void BVT_VSSSetContext_ReadonlySnapshot_BACKUP()
    {
        CheckDriverSupportsNRPC();
        List<string> shareUncPaths = new List<string>();
        shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
        
        // Test shadow copy set with BACKUP context
        TestShadowCopySet(
            (ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | 
            (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_NO_AUTO_RECOVERY,
            shareUncPaths,
            FsrvpStatus.None,
            FsrvpSharePathsType.None);
    }
}
```

**Key Helper Methods:**
- `CheckDriverSupportsNRPC()` - Validates VSS writer driver support
- `TestShadowCopySet()` - Tests full shadow copy lifecycle
- `TestInvalidSetContext()` - Tests error conditions

## Test Categories

```csharp
[TestCategory(TestCategories.Bvt)]           // Built-in verification tests
[TestCategory(TestCategories.Fsrvp)]         // Cluster-required tests
[TestCategory(TestCategories.FsrvpNonClusterRequired)]  // Non-cluster tests (most common)
[TestCategory(TestCategories.NonSmb)]        // RPC operations (not traditional SMB)
[TestCategory(TestCategories.Positive)]      // Positive test case
[TestCategory(TestCategories.UnexpectedContext)]  // Error/edge case tests
```

## Shadow Copy Context Values

Common shadow copy context settings tested:

```csharp
FsrvpContextValues.FSRVP_CTX_BACKUP              // Standard backup context (0x00000001)
FsrvpContextValues.FSRVP_CTX_APP_ROLLBACK        // Application rollback (0x0000000B)
FsrvpContextValues.FSRVP_CTX_NAS_ROLLBACK        // NAS rollback (0x00000010)
FsrvpContextValues.FSRVP_CTX_FILE_SHARE_BACKUP   // File share backup (0x00000019)

FsrvpShadowCopyAttributes.FSRVP_ATTR_NO_AUTO_RECOVERY    // Disable auto-recovery (0x00000002)
FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY       // Enable auto-recovery (0x00400000)
```

**Context determines:**
- Backup scope (full system, application, NAS share)
- Recovery behavior (auto-recovery enabled/disabled)
- Compatibility with different backup scenarios

## Test File Organization

### VSSSetContext.cs
Tests shadow copy context configuration with different context values:
- `FSRVP_CTX_BACKUP` - Read-only snapshot for backup
- `FSRVP_CTX_APP_ROLLBACK` - Application-aware rollback
- `FSRVP_CTX_NAS_ROLLBACK` - NAS-specific rollback
- `FSRVP_CTX_FILE_SHARE_BACKUP` - File share backup
- Invalid context combinations (e.g., both AUTO_RECOVERY and NO_AUTO_RECOVERY)

### VSSOperateShadowCopySet.cs
Tests full shadow copy lifecycle operations:
1. Start shadow copy set
2. Add shares to shadow copy set
3. Prepare shadow copy set
4. Commit shadow copy set
5. Expose shadow copy (make accessible via path)
6. Recover shadow copy
7. Delete shadow copy

### VSSAbortShadowCopySet.cs
Tests abort and cleanup scenarios:
- Aborting at various stages (prepare, commit, expose)
- Error recovery
- Resource cleanup

## Key Concepts

- **Shadow Copy Set**: Container for one or more shadow copies
- **Shadow Copy**: Point-in-time snapshot of a volume
- **Context**: Backup context settings (BACKUP, FILE_SHARE_BACKUP, etc.)
- **Expose**: Making shadow copy accessible via a share path

## Type Declarations (Where to Find/Add Definitions)

### FSRVP Enums and Types
**File**: `ProtoSDK/MS-FSRVP/FsrvpType.cs`

Search for these type definitions (use `public enum` prefix):

| Type | Purpose |
|------|---------|
| `FsrvpLevel` | RPC level enum |
| `FsrvpShadowCopyAttributes` | Shadow copy attribute flags |
| `FsrvpContextValues` | Context values (backup modes) |
| `FsrvpShadowCopyCompatibilityValues` | Compatibility flags |
| `FsrvpVersionValues` | Version enum |
| `FsrvpErrorCode` | Error codes enum |
| `FSRVP_OPNUM` | RPC operation numbers |
| `FsrvpStatus` | Status codes |

### FSRVP Utilities
**File**: `ProtoSDK/MS-FSRVP/FsrvpUtility.cs`

Contains helper methods for FSRVP operations.

**To add new FSRVP types**: Add to `ProtoSDK/MS-FSRVP/FsrvpType.cs`

## RPC Interface

FSRVP uses DCE/RPC for communication:

```csharp
// FSRVP interface UUID
Guid FSRVP_INTERFACE = new Guid("a8e0653c-2744-4389-a61d-7373df8b2292");

// Version
ushort FSRVP_VERSION_MAJOR = 1;
ushort FSRVP_VERSION_MINOR = 0;

// Operation numbers (defined in FsrvpType.cs)
FSRVP_OPNUM.OpGetSupportedVersion
FSRVP_OPNUM.OpSetContext
FSRVP_OPNUM.OpStartShadowCopySet
FSRVP_OPNUM.OpAddToShadowCopySet
FSRVP_OPNUM.OpCommitShadowCopySet
```

## Best Practices

1. **Check cluster requirement** at test start
2. **Clean up shadow copies** in TestCleanup
3. **Verify exposed paths** are accessible
4. **Handle timeout scenarios** for large volumes
