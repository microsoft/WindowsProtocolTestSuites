# SMB2/SMB3 Protocol Reference

This document provides detailed guidance for writing SMB2/SMB3 protocol test cases.

## Required Using Statements

```csharp
using System;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

SMB2 (Server Message Block 2) is the main file sharing protocol tested by the FileServer test suite. The test suite covers:

- **MS-SMB2**: Server Message Block Protocol Versions 2 and 3
- Dialects: SMB 2.0.2, SMB 2.1, SMB 3.0, SMB 3.0.2, SMB 3.1.1

## Directory Structure

```
SMB2/
├── Adapter/
│   └── SMB2TestConfig.cs           # SMB2-specific configuration properties
└── TestSuite/
    ├── SMB2TestBase.cs             # Base class for all SMB2 tests
    ├── SMB2CreateContextResponseChecker.cs  # Create context response validators
    ├── Basic/
    │   └── SMB2Basic.cs            # Basic operations (CREATE, READ, WRITE, etc.)
    ├── AppInstanceId/              # Application Instance ID feature tests
    ├── Compound/                   # Compound request tests
    ├── Compression/                # SMB compression tests
    ├── CreateClose/                # File create/close scenarios
    ├── DurableHandle/              # Durable handle v1 and v2 tests
    ├── Encryption/                 # SMB encryption (signing/sealing) tests
    ├── HVRS/                       # Hyper-V virtual disk tests
    ├── IOCTL/                      # IOCTL operation tests
    ├── Leasing/                    # Lease operation tests
    ├── MultipleChannel/            # Multi-channel and multi-NIC tests
    ├── Negotiate/                  # Protocol negotiation and dialect tests
    ├── OpLock/                     # Opportunistic lock tests
    ├── Replay/                     # Request replay/resilience tests
    ├── ResilientHandle/            # Resilient handle tests
    ├── SessionMgmt/                # Session management tests
    ├── Signing/                    # Message signing/verification tests
    └── TreeMgmt/                   # Tree connect/disconnect tests
```

## Test Base Class Pattern

All SMB2 test classes inherit from `SMB2TestBase`, which extends `CommonTestBase`:

```csharp
[TestClass]
public class SMB2Basic : SMB2TestBase
{
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
        base.TestInitialize();  // Initializes testConfig as SMB2TestConfig
        // Test-specific initialization
    }

    protected override void TestCleanup()
    {
        base.TestCleanup();  // Cleans up client connections
    }
}
```

**Key inherited members from SMB2TestBase**:
- `protected SMB2TestConfig TestConfig` - SMB2 test configuration
- `protected Smb2FunctionalClient client` - main SMB2 client
- `protected void CheckCreateContextResponses(...)` - validate create context responses
- `protected void ReadIpAddressesFromTestConfig(...)` - read multi-NIC addresses

## Common Test Patterns

### Test Categories

```csharp
[TestCategory(TestCategories.Bvt)]              // Built-in Verification Tests
[TestCategory(TestCategories.Smb2002)]          // SMB 2.0.2 tests
[TestCategory(TestCategories.Smb21)]            // SMB 2.1 tests
[TestCategory(TestCategories.Smb30)]            // SMB 3.0 tests
[TestCategory(TestCategories.Smb302)]           // SMB 3.0.2 tests
[TestCategory(TestCategories.Smb311)]           // SMB 3.1.1 tests
[TestCategory(TestCategories.Negotiate)]        // Protocol negotiation
[TestCategory(TestCategories.CreateClose)]      // File create/close
[TestCategory(TestCategories.QueryDir)]         // Query directory
[TestCategory(TestCategories.QueryInfo)]        // Query file information
[TestCategory(TestCategories.QueryAndSetFileInfo)]  // Query and set info
[TestCategory(TestCategories.ChangeNotify)]     // Change notifications
[TestCategory(TestCategories.LockUnlock)]       // File locks
[TestCategory(TestCategories.DurableHandle)]    // Durable handles
[TestCategory(TestCategories.Leasing)]          // Leasing operations
[TestCategory(TestCategories.Encryption)]       // Encryption/signing
[TestCategory(TestCategories.Compression)]      // SMB compression
[TestCategory(TestCategories.DomainRequired)]   // Requires domain
[TestCategory(TestCategories.OutOfBoundary)]    // Boundary condition tests
[TestCategory(TestCategories.Positive)]         // Positive test cases
```

### Basic File Operations

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Smb2002)]
[TestCategory(TestCategories.CreateClose)]
[Description("Test file create and close operations.")]
public void BVT_SMB2Basic_CreateClose()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Setup connection.");
    client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
    client.ConnectToServer(TestConfig.UnderlyingTransport, 
        TestConfig.SutComputerName, TestConfig.SutIPAddress);
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Negotiate SMB dialect.");
    client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Session setup and tree connect.");
    client.SessionSetup(
        TestConfig.DefaultSecurityPackage,
        TestConfig.SutComputerName,
        TestConfig.AccountCredential,
        TestConfig.UseServerGssToken);
    
    uint treeId;
    string uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
    client.TreeConnect(uncSharePath, out treeId);
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Create a file.");
    FILEID fileId;
    Smb2CreateContextResponse[] serverCreateContexts;
    string fileName = "TestFile_" + Guid.NewGuid().ToString();
    
    client.Create(
        treeId,
        fileName,
        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
        out fileId,
        out serverCreateContexts,
        accessMask: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE);
    
    BaseTestSite.Assert.IsTrue(fileId.Persistent != 0 || fileId.Volatile != 0,
        "File should be created with valid file ID");
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Close file and tree.");
    client.Close(treeId, fileId);
    client.TreeDisconnect(treeId);
    client.LogOff();
    client.Disconnect();
}
```

### Query and Set File Information

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Smb2002)]
[TestCategory(TestCategories.QueryAndSetFileInfo)]
[Description("Test QUERY and SET file information operations.")]
public void BVT_SMB2Basic_QueryAndSet_FileInfo()
{
    // Setup
    client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
    // ... connect, negotiate, session setup, tree connect ...
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create file.");
    client.Create(treeId, fileName,
        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
        out fileId, out serverCreateContexts,
        accessMask: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE);
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query file basic information.");
    byte[] outputBuffer;
    client.QueryFileAttributes(treeId,
        (byte)FileInformationClasses.FileBasicInformation,
        QUERY_INFO_Request_Flags_Values.SL_RESTART_SCAN,
        fileId, new byte[0], out outputBuffer);
    
    FileBasicInformation fileBasicInfo = 
        TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
    BaseTestSite.Assert.IsNotNull(fileBasicInfo,
        "FileBasicInformation should be returned");
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Modify and set file information.");
    FileBasicInformation modifiedInfo = fileBasicInfo;
    modifiedInfo.LastAccessTime = Smb2Utility.ConvertToFileTime(DateTime.UtcNow);
    
    byte[] inputBuffer = TypeMarshal.ToBytes<FileBasicInformation>(modifiedInfo);
    client.SetFileAttributes(treeId,
        (byte)FileInformationClasses.FileBasicInformation,
        fileId, inputBuffer);
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Cleanup.");
    client.Close(treeId, fileId);
    // ... tree disconnect, logoff ...
}
```

### File Locking Operations

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Smb2002)]
[TestCategory(TestCategories.LockUnlock)]
[Description("Test LOCK and UNLOCK operations.")]
public void BVT_SMB2Basic_LockAndUnlock()
{
    // Setup and create file...
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Lock byte range in file.");
    LOCK_ELEMENT[] locks = new LOCK_ELEMENT[1];
    locks[0].Offset = 0;
    locks[0].Length = 1024;  // Lock first 1KB
    locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK;
    
    uint status = client.Lock(treeId, 0, fileId, locks);
    BaseTestSite.Assert.AreEqual((uint)Smb2Status.STATUS_SUCCESS, status,
        "Lock should succeed");
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Attempt write to locked range.");
    byte[] writeData = new byte[512];
    status = client.Write(treeId, fileId, writeData, 0,
        checker: (header, response) =>
        {
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_FILE_LOCK_CONFLICT,
                header.Status,
                "Write to locked range should fail");
        });
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Unlock the range.");
    locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;
    status = client.Lock(treeId, 1, fileId, locks);
    BaseTestSite.Assert.AreEqual((uint)Smb2Status.STATUS_SUCCESS, status,
        "Unlock should succeed");
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Now write should succeed.");
    status = client.Write(treeId, fileId, writeData, 0);
    BaseTestSite.Assert.AreEqual((uint)Smb2Status.STATUS_SUCCESS, status,
        "Write should succeed after unlock");
}
```

### Change Notification Operations

```csharp
private AutoResetEvent changeNotificationReceived = new AutoResetEvent(false);
private CHANGE_NOTIFY_Response receivedChangeNotify;
private Packet_Header receivedChangeNotifyHeader;

[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Smb2002)]
[TestCategory(TestCategories.ChangeNotify)]
[Description("Test CHANGE_NOTIFY for file name changes.")]
public void BVT_SMB2Basic_ChangeNotify_ChangeFileName()
{
    // Setup first client for watching directory
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Setup and open directory for watching.");
    client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
    client1.Smb2Client.ChangeNotifyResponseReceived += OnChangeNotifyResponseReceived;
    // ... connect, negotiate, setup, tree connect ...
    
    client1.Create(treeId, testDirectory,
        CreateOptions_Values.FILE_DIRECTORY_FILE,
        out fileIdDir, out serverCreateContexts);
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Register for change notifications.");
    client1.ChangeNotify(treeId, fileIdDir,
        CompletionFilter_Values.FILE_NOTIFY_CHANGE_FILE_NAME,
        flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Create a file from second client.");
    // ... second client creates file ...
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Wait for change notification.");
    BaseTestSite.Assert.IsTrue(
        changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
        "Change notification should be received");
    
    BaseTestSite.Assert.AreEqual(
        Smb2Status.STATUS_SUCCESS,
        receivedChangeNotifyHeader.Status,
        "CHANGE_NOTIFY response should succeed");
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Verify notification details.");
    BaseTestSite.Assert.IsNotNull(
        receivedFileNotifyInfo,
        "FILE_NOTIFY_INFORMATION array should be populated");
}

private void OnChangeNotifyResponseReceived(
    FILE_NOTIFY_INFORMATION[] fileNotifyInfo,
    Packet_Header respHeader,
    CHANGE_NOTIFY_Response changeNotify)
{
    receivedChangeNotify = changeNotify;
    receivedChangeNotifyHeader = respHeader;
    receivedFileNotifyInfo = fileNotifyInfo;
    changeNotificationReceived.Set();
}
```

### Protocol Negotiation

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Smb21)]
[TestCategory(TestCategories.Negotiate)]
[Description("Test negotiation with dialect wildcard.")]
public void BVT_Negotiate_Compatible_Wildcard()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Connect and send NEGOTIATE with wildcard.");
    client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
    client.ConnectToServer(TestConfig.UnderlyingTransport,
        TestConfig.SutComputerName, TestConfig.SutIPAddress);
    
    string[] dialects = new string[] { "SMB 2.002", "SMB 2.???" };
    
    client.MultiProtocolNegotiate(dialects,
        (Packet_Header header, NEGOTIATE_Response response) =>
        {
            BaseTestSite.Log.Add(LogEntryKind.Debug,
                "Server selected dialect: {0}", response.DialectRevision);
            
            if (TestConfig.MaxSmbVersionSupported == DialectRevision.Smb2002)
            {
                BaseTestSite.Assert.AreEqual(
                    DialectRevision.Smb2002,
                    response.DialectRevision,
                    "Server should select SMB 2.002 if only 2.002 supported");
            }
            else
            {
                BaseTestSite.Assert.AreEqual(
                    DialectRevision.Smb2Wildcard,
                    response.DialectRevision,
                    "Server should respond with wildcard for compatible dialects");
            }
        });
}
```

## SMB2TestConfig Properties

Configuration is loaded from `MS-SMB2_ServerTestSuite.ptfconfig`:

```csharp
public class SMB2TestConfig : TestConfigBase
{
    // Connection properties
    public string SutComputerName { get; }
    public IPAddress SutIPAddress { get; }
    public IPAddress SutAlternativeIPAddress { get; }        // For multi-NIC tests
    
    // Share names
    public string BasicFileShare { get; }
    public string SymbolicLink { get; }
    public string FileShareSupportingIntegrityInfo { get; }
    
    // File information
    public uint NumberOfPreviousVersions { get; }            // For snapshot tests
    
    // HVRS (Hyper-V) properties
    public string SharePath { get; }
    public string ShareServerName { get; }
    public bool IsOffLoadImplemented { get; }
    
    // Timeouts and dialects
    public int WaitTimeoutInMilliseconds { get; }
    public DialectRevision[] RequestDialects { get; }
    public DialectRevision MaxSmbVersionSupported { get; }
    
    // Helper methods
    public string GetProperty(string propertyName, bool checkNullOrEmpty = true);
}
```
## Error Handling Patterns

### Expected Failures

```csharp
status = client.Create(
    treeId,
    fileName,
    CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
    out fileId,
    out serverCreateContexts,
    checker: (header, response) =>
    {
        BaseTestSite.Assert.AreEqual(
            Smb2Status.STATUS_ACCESS_DENIED,
            header.Status,
            "Operation should fail with STATUS_ACCESS_DENIED");
    });
```

### Invalid Parameter Tests

```csharp
[TestMethod]
[TestCategory(TestCategories.Smb2002)]
[TestCategory(TestCategories.OutOfBoundary)]
[TestCategory(TestCategories.CreateClose)]
[Description("Test server response to invalid CREATE structure size.")]
public void InvalidCreateRequestStructureSize()
{
    // Modify packet before sending
    client1.BeforeSendingPacket(ReplacePacketByStructureSize);
    
    status = client1.Create(...,
        checker: (header, response) => { });
    
    BaseTestSite.Assert.AreEqual(
        Smb2Status.STATUS_INVALID_PARAMETER,
        status,
        "Server should return STATUS_INVALID_PARAMETER");
}

private void ReplacePacketByStructureSize(Smb2Packet packet)
{
    Smb2CreateRequestPacket request = packet as Smb2CreateRequestPacket;
    if (request == null) return;
    request.PayLoad.StructureSize += 1; // Invalid size
}
```

## Key Protocol Constants

```csharp
// Status codes (Smb2Status class)
Smb2Status.STATUS_SUCCESS
Smb2Status.STATUS_ACCESS_DENIED
Smb2Status.STATUS_FILE_LOCK_CONFLICT
Smb2Status.STATUS_INVALID_PARAMETER
Smb2Status.STATUS_CANCELLED
Smb2Status.STATUS_NOTIFY_CLEANUP

// Create options
CreateOptions_Values.FILE_NON_DIRECTORY_FILE
CreateOptions_Values.FILE_DIRECTORY_FILE
CreateOptions_Values.FILE_DELETE_ON_CLOSE

// Access masks
AccessMask.GENERIC_READ
AccessMask.GENERIC_WRITE
AccessMask.DELETE
AccessMask.WRITE_DAC

// Change notify filters
CompletionFilter_Values.FILE_NOTIFY_CHANGE_FILE_NAME
CompletionFilter_Values.FILE_NOTIFY_CHANGE_DIR_NAME
CompletionFilter_Values.FILE_NOTIFY_CHANGE_ATTRIBUTES
CompletionFilter_Values.FILE_NOTIFY_CHANGE_SIZE
CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_ACCESS
CompletionFilter_Values.FILE_NOTIFY_CHANGE_LAST_WRITE
CompletionFilter_Values.FILE_NOTIFY_CHANGE_CREATION
CompletionFilter_Values.FILE_NOTIFY_CHANGE_EA
CompletionFilter_Values.FILE_NOTIFY_CHANGE_SECURITY
CompletionFilter_Values.FILE_NOTIFY_CHANGE_STREAM_NAME
CompletionFilter_Values.FILE_NOTIFY_CHANGE_STREAM_SIZE
CompletionFilter_Values.FILE_NOTIFY_CHANGE_STREAM_WRITE
```

## Type Declarations (Where to Find/Add Definitions)

### SMB2 Message Types and Enums
**File**: `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`

Search for these type definitions (use `public enum` or `public class` prefix):

| Type | Purpose |
|------|---------|
| `ShareType_Values` | Share type enum |
| `ShareFlags_Values` | Share flags enum |
| `Capabilities_Values` | Server capabilities flags |
| `Share_Capabilities_Values` | Share capabilities flags |
| `SecurityFlags_Values` | Security flags |
| `RequestedOplockLevel_Values` | Oplock level enum |
| `ImpersonationLevel_Values` | Impersonation level enum |
| `ShareAccess_Values` | Share access flags |
| `CreateDisposition_Values` | Create disposition enum |
| `CreateOptions_Values` | Create options flags |
| `AccessMask` | File access mask enum |
| `Directory_Access_Mask_Values` | Directory access mask |
| `LeaseStateValues` | Lease state flags |
| `OplockLevel_Values` | Oplock level enum |
| `CreateAction_Values` | Create action enum |
| `LeaseFlagsValues` | Lease flags |
| `Flags_Values` | Request flags |
| `READ_Request_Flags_Values` | Read request flags |
| `Channel_Values` | Channel enum |
| `Packet_Header_Flags_Values` | Packet header flags |
| `WRITE_Request_Flags_Values` | Write request flags |
| `LOCK_ELEMENT_Flags_Values` | Lock element flags |
| `CtlCode_Values` | FSCTL control codes |
| `CompletionFilter_Values` | Change notify filters |
| `Smb2Status` (static class) | Status codes |

### File Information Classes
**File**: `ProtoSDK/MS-SMB2/FsccMessage.cs`

Search for these type definitions:

| Type | Purpose |
|------|---------|
| `public enum FileInformationClasses` | File information class enum |
| `FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request_Flags_Values` | FSCTL flags |

### Transport Types
**File**: `ProtoSDK/MS-SMB2/CustomTypes.cs`

| Type | Purpose |
|------|---------|
| `Smb2TransportType` | Transport enum (Tcp, NetBios, Rdma, Quic) |

**To add new SMB2 types**: Add to `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`  
**To add new file info classes**: Add to `ProtoSDK/MS-SMB2/FsccMessage.cs`

## Key Concepts

- **Smb2FunctionalClient**: High-level client API for SMB2 operations
- **Create Contexts**: SMB2 feature negotiation during file open (durability, leasing, integrity)
- **Durable Handles**: Handles that can survive brief disconnections
- **Leasing**: Caching hints for client optimization
- **Change Notifications**: Server notifications of file system changes
- **Request Replay**: Idempotent request handling for reliability
- **Compression**: Network compression for large transfers
- **Encryption**: SMB3 encryption (signing/sealing)
- **Multi-Channel**: Multiple connections for bandwidth aggregation
- **OpLock**: Opportunistic locking for caching hints

## Best Practices

1. **Always call Disconnect()** on client after test completes
2. **Use appropriate test categories** for filtering and dialect requirements
3. **Log test steps** with `BaseTestSite.Log.Add(LogEntryKind.TestStep, ...)`
4. **Verify dialect support** at test start with `TestConfig.CheckDialect()`
5. **Clean up resources** properly in TestCleanup
6. **Use helper methods** for file/directory creation
7. **Include checker callbacks** in operations to verify response headers
8. **Handle async operations** with events and AutoResetEvent/ManualResetEvent
9. **Test both positive and negative cases** for protocol compliance
10. **Use TypeMarshal** for serializing/deserializing structures
