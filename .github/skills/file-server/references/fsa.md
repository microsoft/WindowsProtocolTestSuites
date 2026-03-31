# FSA Protocol Reference

This document provides guidance for writing File System Access (FSA) test cases.

## Required Using Statements

```csharp
using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsa;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

FSA tests validate file system behavior at the API level:
- **MS-FSA**: File System Algorithms
- **MS-FSCC**: File System Control Codes

## Directory Structure

```
FSA/
├── Adapter/
│   ├── FSAAdapter.cs              # Main FSA protocol adapter
│   ├── FSATestConfig.cs           # FSA-specific configuration
│   └── IFSAAdapter.cs             # FSAAdapter interface
├── TestSuite/
│   ├── CreateFile/
│   │   ├── CreateFileTestCases.cs # CreateFile test class base
│   │   ├── CreateFile_*.cs        # Individual CreateFile tests
│   │   └── ...
│   ├── FileAccess/
│   │   ├── FileAccessTestCases.cs
│   │   └── ...
│   ├── FileInformation/
│   │   ├── FileInfoTestCases.cs   # Query/set file info tests
│   │   └── ...
│   ├── FsControlRequest/
│   │   ├── FsControlTestCases.cs
│   │   └── ...
│   ├── AlternateDataStream/
│   │   ├── AlternateDataStreamTestCases.cs
│   │   └── ...
│   ├── QuotaInformation/
│   │   ├── QuotaInfoTestCases.cs
│   │   └── ...
│   ├── QueryDirectory/
│   │   ├── QueryDirectoryTestCases.cs
│   │   └── ...
│   ├── FileSystemInformation/
│   │   ├── FsInfoTestCases.cs
│   │   └── ...
│   ├── Leasing/
│   │   ├── LeasingTestCases.cs
│   │   └── ...
│   ├── TraditionalTestCases/     # Legacy test cases
│   ├── CommonAlgorithm/          # Shared helper algorithms
│   ├── MS-FSA_ServerTestSuite.csproj
│   ├── MS-FSA_ServerTestSuite.ptfconfig
│   └── MS-FSA_ServerTestSuite.deployment.ptfconfig
└── [Additional test categories as needed]
```

**Key File Structure Notes:**
- `FSAAdapter.cs` is the main adapter implementing all FSA operations
- Each test category (CreateFile, FileInformation, etc.) has a corresponding folder with `*TestCases.cs` base class
- Individual test methods are in separate `*_*.cs` files within each category folder
- `CommonAlgorithm/` folder contains shared helper methods used across test categories
- Configuration and deployment settings in `.ptfconfig` files at suite root

## Test Base Class

FSA tests inherit from `PtfTestClassBase` and use `FSAAdapter`:

```csharp
[TestClass]
public partial class FileInfoTestCases : PtfTestClassBase
{
    private FSAAdapter fsaAdapter;
    
    [ClassInitialize()]
    public static void ClassInitialize(TestContext context)
    {
        PtfTestClassBase.Initialize(context);
    }

    [ClassCleanup()]
    public static void ClassCleanup()
    {
        PtfTestClassBase.Cleanup();
    }

    protected override void TestInitialize()
    {
        this.InitializeTestManager();
        this.fsaAdapter = new FSAAdapter();
        this.fsaAdapter.Initialize(BaseTestSite);
        this.fsaAdapter.LogTestCaseDescription(BaseTestSite);
        // Log test environment details
        BaseTestSite.Log.Add(LogEntryKind.Comment, "File System: " + fsaAdapter.FileSystem.ToString());
        BaseTestSite.Log.Add(LogEntryKind.Comment, "Transport: " + fsaAdapter.Transport.ToString());
        BaseTestSite.Log.Add(LogEntryKind.Comment, "Platform: " + fsaAdapter.TestConfig.Platform.ToString());
        this.fsaAdapter.FsaInitial();
    }

    protected override void TestCleanup()
    {
        this.fsaAdapter.Dispose();
        base.TestCleanup();
        this.CleanupTestManager();
    }
}
```

**Key FSAAdapter Properties:**
- `FileSystem` - NTFS, ReFS, FAT32, etc.
- `Transport` - TCP/UDP, SMB version
- `UncSharePath` - Path to test share
- `TestConfig` - FSA configuration and platform info
- Capability Properties: `IsIntegritySupported`, `IsCompressionSupported`, `IsSparseFileSupported`, `IsEncryptionSupported`, `IsQuotaSupported`, `IsReparsePointSupported`, etc.

## Test Categories

```csharp
[TestCategory(TestCategories.Fsa)]
[TestCategory(TestCategories.CreateFile)]
[TestCategory(TestCategories.QueryInfo)]
[TestCategory(TestCategories.FsControl)]
[TestCategory(TestCategories.AlternateDataStream)]
[TestCategory(TestCategories.QuotaInfo)]
[TestCategory(TestCategories.Leasing)]
[TestCategory(TestCategories.QueryDirectory)]
```

## Test Suite Organization

FSA tests are organized by feature area:
- `CreateFile/` - File/directory creation tests
- `FileAccess/` - File open, access rights tests
- `FileInformation/` - Query/set file attributes
- `FsControlRequest/` - FSCTL operation tests
- `AlternateDataStream/` - Alternate data stream tests
- `QuotaInformation/` - Quota query/set tests
- `QueryDirectory/` - Directory enumeration tests
- `FileSystemInformation/` - Volume property tests
- `Leasing/` - File leasing tests
- `CommonAlgorithm/` - Shared helper algorithms

## Common Test Patterns

### File Creation Test

```csharp
[TestMethod]
public void CreateFile_BasicTest()
{
    // FSAAdapter.CreateFile() maps to FSA model operations
    // Returns MessageStatus (SUCCESS, INVALID_PARAMETER, etc.)
    MessageStatus status = fsaAdapter.CreateFile(
        fileName,                           // Relative path
        FileAttribute.NORMAL,               // File attributes
        CreateOptions.NON_DIRECTORY_FILE,   // Create options
        FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE,  // Access mask
        ShareAccess.FILE_SHARE_READ,        // Share mode
        CreateDisposition.CREATE);          // Create disposition
    
    BaseTestSite.Assert.AreEqual(
        MessageStatus.SUCCESS,
        status,
        "CreateFile should succeed");
}
```

### Query File Information

```csharp
[TestMethod]
public void QueryFileInfo_BasicTest()
{
    // Open file first
    fsaAdapter.CreateFile(...);
    
    // Query file information (native FSCC structures)
    byte[] outputBuffer;
    MessageStatus status = fsaAdapter.QueryFileInformation(
        FileInformationClass.FileBasicInformation,  // Info class to query
        out outputBuffer);
    
    // Parse response using TypeMarshal
    FileBasicInformation info = 
        TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
    
    BaseTestSite.Assert.IsNotNull(info.CreationTime);
}
```

### FSCTL Operation

```csharp
[TestMethod]
public void FsControl_CompressTest()
{
    fsaAdapter.CreateFile(...);
    
    // Check FSCTL support first
    fsaAdapter.TestConfig.CheckFSCTL((uint)FsControlCommand.FSCTL_SET_COMPRESSION);
    
    // Send FSCTL request
    MessageStatus status = fsaAdapter.FsControl(
        FsControlCommand.FSCTL_SET_COMPRESSION,
        inputBuffer,
        out byte[] outputBuffer);
    
    BaseTestSite.Assert.AreEqual(
        MessageStatus.SUCCESS,
        status,
        "FSCTL_SET_COMPRESSION should succeed");
}
```

### Query Directory

```csharp
[TestMethod]
public void QueryDirectory_EnumerateTest()
{
    // Open directory
    fsaAdapter.CreateFile(directoryName, ...);
    
    // Query directory contents (returns directory entries)
    MessageStatus status = fsaAdapter.QueryDirectory(
        FileInformationClass.FileDirectoryInformation,
        pattern,  // Wildcard pattern or null for all
        out byte[] outputBuffer);
    
    // Parse results
    List<FileDirectoryInformation> entries = ParseDirectoryEntries(outputBuffer);
}
```

## Type Declarations (Where to Find/Add Definitions)

### File Information Classes
**File**: `ProtoSDK/MS-SMB2/FsccMessage.cs`

Search for `public enum FileInformationClasses` to find all file info class values.

Common file information classes for query/set operations:

```csharp
// Search for "public enum FileInformationClasses" in FsccMessage.cs
FileInformationClasses.FileBasicInformation       // Timestamps, attributes
FileInformationClasses.FileStandardInformation    // Allocation, EOF, links
FileInformationClasses.FileNameInformation        // File name
FileInformationClasses.FileAllInformation         // All basic info combined
FileInformationClasses.FileAttributeTagInformation // Attributes and reparse tag
FileInformationClasses.FileEndOfFileInformation   // End of file position
FileInformationClasses.FilePositionInformation    // Current file position
FileInformationClasses.FileRenameInformation      // Rename operations
FileInformationClasses.FileLinkInformation        // Hard link creation
FileInformationClasses.FileDispositionInformation // Delete on close
FileInformationClasses.FileFullEaInformation      // Extended attributes
FileInformationClasses.FileQuotaInformation       // Quota information
```

### FSCTL Codes
**File**: `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`

Search for `public enum CtlCode_Values` to find all FSCTL control codes.

Common file system control codes:

```csharp
// Search for "public enum CtlCode_Values" in SMB2Message.cs
CtlCode_Values.FSCTL_GET_COMPRESSION
CtlCode_Values.FSCTL_SET_COMPRESSION
CtlCode_Values.FSCTL_SET_SPARSE
CtlCode_Values.FSCTL_SET_ZERO_DATA
CtlCode_Values.FSCTL_QUERY_ALLOCATED_RANGES
CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION
CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION
CtlCode_Values.FSCTL_OFFLOAD_READ
CtlCode_Values.FSCTL_OFFLOAD_WRITE
CtlCode_Values.FSCTL_DUPLICATE_EXTENTS_TO_FILE
```

**To add new file info classes**: Add to `ProtoSDK/MS-SMB2/FsccMessage.cs`  
**To add new FSCTL codes**: Add to `CtlCode_Values` in `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`

## FSAAdapter Key Methods

All file operations return `MessageStatus` (SUCCESS, INVALID_PARAMETER, etc.):

```csharp
// File operations
MessageStatus CreateFile(string name, FileAttribute attrs, CreateOptions opts, 
    FileAccess access, ShareAccess share, CreateDisposition disp)

MessageStatus OpenFile(string name, FileAccess access, ShareAccess share)

MessageStatus CloseHandle()  // Close current open handle

// Information operations
MessageStatus QueryFileInformation(FileInformationClass infoClass, out byte[] output)

MessageStatus SetFileInformation(FileInformationClass infoClass, byte[] input)

MessageStatus QueryFileInformation(FileInformationClass infoClass, FileInformationClass2 infoClass2)

// Directory operations
MessageStatus QueryDirectory(FileInformationClass infoClass, string pattern, out byte[] output)

// FSCTL operations
MessageStatus FsControl(FsControlCommand fsctl, byte[] input, out byte[] output)

// Lock operations
MessageStatus Lock(long offset, long length, bool isExclusive)

MessageStatus Unlock(long offset, long length)

// Lease operations
MessageStatus RequestLeasing(uint leaseState)

// Get handle state
MessageStatus GetFileHandle(string fileName, out uint fileId)
```

**Return Values:**
- `MessageStatus.SUCCESS` - Operation succeeded
- `MessageStatus.INVALID_PARAMETER` - Invalid arguments
- `MessageStatus.OBJECT_NAME_NOT_FOUND` - File/directory not found
- `MessageStatus.ACCESS_DENIED` - Insufficient access rights
- `MessageStatus.FILE_EXISTS` - File already exists
- Other NTSTATUS codes per MS-FSA specification

## Key Concepts

- **FSAAdapter**: Main protocol adapter that implements FSA operations
- **MessageStatus**: Return type for all operations (NTSTATUS values)
- **FileInformationClass**: Enum specifying what file info to query/set (FileBasicInformation, FileStandardInformation, etc.)
- **FsControlCommand**: FSCTL operation codes (FSCTL_SET_COMPRESSION, FSCTL_QUERY_ALLOCATED_RANGES, etc.)
- **CreateOptions**: File creation flags (NON_DIRECTORY_FILE, DIRECTORY_FILE, DELETE_ON_CLOSE, etc.)
- **FileAccess**: Access mask flags (GENERIC_READ, GENERIC_WRITE, DELETE, etc.)
- **ShareAccess**: Share mode flags (FILE_SHARE_READ, FILE_SHARE_WRITE, etc.)
- **Capability Properties**: FSAAdapter provides properties to check SUT capabilities:
  - `IsIntegritySupported` - Integrity streams (ReFS)
  - `IsCompressionSupported` - File compression
  - `IsSparseFileSupported` - Sparse files
  - `IsEncryptionSupported` - EFS encryption
  - `IsQuotaSupported` - Disk quotas
  - `IsReparsePointSupported` - Reparse points
  - `IsShortNameSupported` - 8.3 format names
  - And many more feature flags

## Best Practices

1. **Check capability flags** before testing unsupported features (e.g., `if (fsaAdapter.IsCompressionSupported)`)
2. **Always close handles** in TestCleanup (fsaAdapter.Dispose())
3. **Use CheckFSCTL()** before sending unsupported FSCTLs
4. **Verify MessageStatus** matches expected result
5. **Log test environment** early (FileSystem, Transport, Platform)
6. **Use TypeMarshal** to parse FSCC structures from output buffers
