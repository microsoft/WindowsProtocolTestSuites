# RSVD Protocol Reference

This document provides guidance for writing Remote Shared Virtual Disk test cases.

## Required Using Statements

```csharp
using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.RSVD.TestSuite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

RSVD tests validate the implementation of:
- **MS-RSVD**: Remote Shared Virtual Disk Protocol

Used for accessing virtual hard disk (VHD/VHDX) files over SMB.

## Directory Structure

```
RSVD/
└── TestSuite/
    ├── RSVDTestBase.cs                      # Base class for RSVD tests
    ├── RSVDTestConfig.cs                    # RSVD-specific configuration
    ├── OpenCloseSharedVHD.cs               # Open/close shared VHD operations
    ├── ReadWriteSharedVHD.cs               # Read/write shared VHD operations
    ├── TunnelOperationToSharedVHD.cs       # Tunnel operations (SCSI, file info, etc.)
    ├── TwoClientsAccessSameSharedVHD.cs    # Multi-client access scenarios
    ├── QuerySharedVirtualDiskSupport.cs    # Query server support capabilities
    ├── CreateAndDeleteCheckpoint.cs        # Checkpoint/snapshot operations
    ├── OptimizeAndExtractVHDSet.cs         # VHD set optimization and extraction
    ├── ConvertVHDtoVHDSet.cs               # VHD to VHD set conversion
    ├── QueryVHDSetFileInfo.cs              # Query VHD set file information
    ├── SCSIPersistentReservation.cs        # SCSI persistent reservation handling
    ├── ChangTracking.cs                    # Change tracking operations
    ├── Resize.cs                           # Virtual disk resize operations
    └── MS-RSVD_ServerTestSuite.ptfconfig   # Test configuration file
```

**Note**: Unlike other FileServer test suites, RSVD test suite has NO Adapter folder. It uses `RsvdClient` directly from ProtoSDK.

## Test Base Class Pattern

All RSVD test classes inherit from `RSVDTestBase` which provides:

```csharp
[TestClass]
public class OpenCloseSharedVHD : RSVDTestBase
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
        base.TestInitialize();
        // Test-specific initialization
    }

    protected override void TestCleanup()
    {
        base.TestCleanup();  // Disconnects RsvdClient
    }
}
```

**Key inherited members from RSVDTestBase**:
- `protected RsvdClient client` - main RSVD client instance
- `protected RSVDTestConfig TestConfig` - test configuration
- `protected void OpenSharedVHD(...)` - helper method to open shared VHD
- `protected const string fileNameSuffix = ":SharedVirtualDisk"` - standard suffix for RSVD opens

## Common Test Patterns

### Test Categories

```csharp
[TestCategory(TestCategories.Bvt)]              // Built-in Verification Tests
[TestCategory(TestCategories.RsvdVersion1)]     // RSVD v1 tests
[TestCategory(TestCategories.RsvdVersion2)]     // RSVD v2 tests
[TestCategory(TestCategories.NonSmb)]           // Non-SMB related tests
[TestCategory(TestCategories.Positive)]         // Positive test cases
```

### Opening Shared Virtual Disk

**Method 1: Using inherited OpenSharedVHD helper**

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.RsvdVersion1)]
[Description("Test opening and closing shared virtual disk.")]
public void BVT_OpenCloseSharedVHD_V1()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Client opens a shared virtual disk file.");
    Smb2CreateContextResponse[] serverContextResponse;
    OpenSharedVHD(
        TestConfig.NameOfSharedVHDX,                        // File name
        RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1,      // Version
        null,                                               // openRequestId (null for auto-increment)
        true,                                               // hasInitiatorId
        null,                                               // rsvdClient (null uses default)
        out serverContextResponse,
        null);                                              // initiatorHostName

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify server context response.");
    // Verify response contexts...

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Client closes the file.");
    client.CloseSharedVirtualDisk();
}
```

**Method 2: Direct RsvdClient usage with SMB2**

```csharp
[TestMethod]
[TestCategory(TestCategories.RsvdVersion1)]
public void OpenSharedVHD_WithDurableHandle()
{
    Smb2FunctionalClient smb2Client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
    
    // Standard SMB2 connection
    smb2Client.ConnectToServer(...);
    smb2Client.Negotiate(...);
    smb2Client.SessionSetup(...);
    uint treeId;
    smb2Client.TreeConnect(TestConfig.ShareContainingSharedVHD, out treeId);
    
    // Create with SVHDX context
    Guid initiatorId = Guid.NewGuid();
    FILEID fileId;
    Smb2CreateContextResponse[] contexts;
    
    smb2Client.Create(
        treeId,
        TestConfig.NameOfSharedVHDX + ":SharedVirtualDisk",
        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
        out fileId,
        out contexts,
        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
        new Smb2CreateContextRequest[]
        {
            new Smb2CreateSvhdxOpenDeviceContext
            {
                Version = (uint)RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1,
                OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER,
                InitiatorHostName = Environment.MachineName,
                InitiatorHostNameLength = (ushort)(Environment.MachineName.Length * 2),
                InitiatorId = initiatorId,
                HasInitiatorId = true
            }
        });

    // Perform operations...
    smb2Client.Close(treeId, fileId);
    smb2Client.TreeDisconnect(treeId);
    smb2Client.LogOff();
}
```

### Read/Write Shared Virtual Disk

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.RsvdVersion1)]
[Description("Test read operations on shared virtual disk.")]
public void BVT_ReadSharedVHD()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Client opens a shared virtual disk file.");
    OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Client reads file content.");
    byte[] payload;
    uint status = client.Read(
        0,      // Offset
        512,    // Length in bytes
        out payload);
    
    BaseTestSite.Assert.AreEqual(
        (uint)Smb2Status.STATUS_SUCCESS,
        status,
        "Read from shared VHD should succeed");

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Client closes the file.");
    client.CloseSharedVirtualDisk();
}

[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.RsvdVersion1)]
[Description("Test write operations on shared virtual disk.")]
public void BVT_WriteSharedVHD()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Client opens a shared virtual disk file.");
    OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Client writes file content.");
    byte[] payload = new byte[512];
    new Random().NextBytes(payload);
    
    uint status = client.Write(
        0,        // Offset
        payload); // Data to write
    
    BaseTestSite.Assert.AreEqual(
        (uint)Smb2Status.STATUS_SUCCESS,
        status,
        "Write to shared VHD should succeed");

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Client closes the file.");
    client.CloseSharedVirtualDisk();
}
```

### Tunnel Operations (SCSI, File Info, etc.)

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.RsvdVersion1)]
[Description("Test tunnel file info operation.")]
public void BVT_TunnelGetFileInfoToSharedVHD()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Client opens a shared virtual disk file.");
    OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Client sends tunnel GET_FILE_INFO operation.");
    byte[] payload = client.CreateTunnelFileInfoRequest();
    
    SVHDX_TUNNEL_OPERATION_HEADER? header;
    SVHDX_TUNNEL_FILE_INFO_RESPONSE? response;
    
    uint status = client.TunnelOperation<SVHDX_TUNNEL_FILE_INFO_RESPONSE>(
        false,  // Sync operation (false = synchronous, true = asynchronous)
        RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_GET_FILE_INFO_OPERATION,
        ++RequestIdentifier,
        payload,
        out header,
        out response);

    BaseTestSite.Assert.AreEqual(
        (uint)Smb2Status.STATUS_SUCCESS,
        status,
        "Tunnel operation should succeed");

    // Verify response
    BaseTestSite.Assert.IsNotNull(response, "Response should not be null");
    BaseTestSite.Assert.AreEqual(
        TestConfig.ServerServiceVersion,
        response.Value.ServerVersion,
        "Server version should match config");

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Client closes the file.");
    client.CloseSharedVirtualDisk();
}

[TestMethod]
[TestCategory(TestCategories.RsvdVersion1)]
[Description("Test tunnel SCSI operation.")]
public void TunnelSCSIToSharedVHD()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Client opens a shared virtual disk file.");
    OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Client sends SCSI READ CAPACITY command.");
    
    // Simulate SCSI READ CAPACITY (opcode 0x25)
    byte[] cdbBuffer = new byte[RsvdConst.RSVD_CDB_GENERIC_LENGTH];
    cdbBuffer[0] = 0x25;  // READ CAPACITY operation code
    byte[] dataBuffer = new byte[8];

    byte[] payload = client.CreateTunnelScsiRequest(
        RsvdConst.SVHDX_TUNNEL_SCSI_REQUEST_LENGTH,
        (byte)cdbBuffer.Length,
        (byte)RsvdConst.RSVD_SCSI_SENSE_BUFFER_SIZE,
        true,
        SRB_FLAGS.SRB_FLAGS_QUEUE_ACTION_ENABLE |
        SRB_FLAGS.SRB_FLAGS_DISABLE_SYNCH_TRANSFER |
        SRB_FLAGS.SRB_FLAGS_DATA_IN);

    SVHDX_TUNNEL_OPERATION_HEADER? header;
    SVHDX_TUNNEL_SCSI_RESPONSE? response;
    
    uint status = client.TunnelOperation<SVHDX_TUNNEL_SCSI_RESPONSE>(
        false,
        RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_SCSI_OPERATION,
        ++RequestIdentifier,
        payload,
        out header,
        out response);

    BaseTestSite.Assert.AreEqual(
        (uint)Smb2Status.STATUS_SUCCESS,
        status,
        "SCSI tunnel operation should succeed");

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Client closes the file.");
    client.CloseSharedVirtualDisk();
}
```

## RSVDTestConfig Properties

Configuration is loaded from `MS-RSVD_ServerTestSuite.ptfconfig`:

```csharp
public class RSVDTestConfig : TestConfigBase
{
    // File server information
    public string FileServerNameContainingSharedVHD { get; }
    public IPAddress FileServerIPContainingSharedVHD { get; }
    public string ShareContainingSharedVHD { get; }
    
    // Shared VHD file names
    public string NameOfSharedVHDX { get; }  // e.g., "test.vhdx"
    public string NameOfSharedVHDS { get; }  // e.g., "test.vhds"
    
    // Virtual disk properties
    public uint ServerServiceVersion { get; }
    public uint SectorSize { get; }
    public uint PhysicalSectorSize { get; }
    public ulong VirtualSize { get; }
    
    // Test configuration
    public string InitiatorHostName { get; }
    public string DomainName { get; }
    public string UserName { get; }
    public string UserPassword { get; }
}

// Access in test:
string vhdxName = TestConfig.NameOfSharedVHDX;
string serverName = TestConfig.FileServerNameContainingSharedVHD;
```

The `RsvdClient` class is the main interface for RSVD testing. Key methods:

### Connection & Lifecycle

```csharp
// Establish connection with server
public void Connect(
    string serverName,
    IPAddress serverIP,
    string domain,
    string userName,
    string password,
    string securityPackage,
    bool useServerToken,
    string shareName);

// Disconnect from server
public void Disconnect();

// Close shared virtual disk
public void CloseSharedVirtualDisk();
```

### I/O Operations

```csharp
// Read data from shared VHD
public uint Read(ulong offset, uint length, out byte[] data);

// Write data to shared VHD
public uint Write(ulong offset, byte[] data);
```

### Tunnel Operations

```csharp
// Generic tunnel operation (supports sync and async)
public uint TunnelOperation<ResponseT>(
    bool isAsync,                                    // Async vs sync
    RSVD_TUNNEL_OPERATION_CODE operationCode,       // Operation type
    ulong requestId,                                 // Request identifier
    byte[] payload,                                  // Operation payload
    out SVHDX_TUNNEL_OPERATION_HEADER? header,       // Response header
    out ResponseT? response) where ResponseT : struct;
```

### Helper Methods

```csharp
// Create tunnel file info request payload
public byte[] CreateTunnelFileInfoRequest();

// Create tunnel SCSI request payload
public byte[] CreateTunnelScsiRequest(
    uint requestLength,
    byte cdbLength,
    byte senseBufferLength,
    bool dataIn,
    SRB_FLAGS flags);

// Create meta operation request (for snapshots, optimize, extract, resize, etc.)
public byte[] CreateMetaOperationRequest(
    META_OPERATION_TYPE operationType,
    // ... additional parameters based on operation type
);
```

## Key Concepts

- **Shared VHD**: Virtual hard disk accessible by multiple clients simultaneously over SMB
- **SVHDX Context**: SMB2 create context for opening shared VHD files (`:SharedVirtualDisk` suffix)
- **Tunnel Operations**: IOCTL-based operations for VHD manipulation (file info, SCSI commands)
- **Meta Operations**: Complex operations like snapshot creation, optimization, extraction, conversion, resize
- **RsvdClient**: Direct protocol client (not an adapter) for RSVD-specific operations
- **RequestIdentifier**: Unique request ID for RSVD operations (not SMB2 message ID)

## RSVDTestConfig Properties

Configuration is loaded from `MS-RSVD_ServerTestSuite.ptfconfig`:

```csharp
public class RSVDTestConfig : TestConfigBase
{
    // File server information
    public string FileServerNameContainingSharedVHD { get; }
    public IPAddress FileServerIPContainingSharedVHD { get; }
    public string ShareContainingSharedVHD { get; }
    
    // Shared VHD file names
    public string NameOfSharedVHDX { get; }  // e.g., "test.vhdx"
    public string NameOfSharedVHDS { get; }  // e.g., "test.vhds"
    
    // Virtual disk properties
    public uint ServerServiceVersion { get; }
    public uint SectorSize { get; }
    public uint PhysicalSectorSize { get; }
    public ulong VirtualSize { get; }
    
    // Test configuration
    public string InitiatorHostName { get; }
    public string DomainName { get; }
    public string UserName { get; }
    public string UserPassword { get; }
}

// Access in test:
string vhdxName = TestConfig.NameOfSharedVHDX;
string serverName = TestConfig.FileServerNameContainingSharedVHD;
```

## Key RSVD Types & Enums

All RSVD types are located in `ProtoSDK/MS-RSVD/Packet/RsvdMessage.cs` and related files:

| Type | Definition Location | Purpose |
|------|-------------------|---------|
| `RSVD_PROTOCOL_VERSION` | MS-RSVD/Packet/RsvdMessage.cs | Protocol version (V1, V2) |
| `RSVD_TUNNEL_OPERATION_CODE` | MS-RSVD/Packet/RsvdMessage.cs | Tunnel operation types (GET_FILE_INFO, SCSI, etc.) |
| `OriginatorFlag` | MS-RSVD/Packet/RsvdMessage.cs | Originator flags (PVHDPARSER, VIRTUAL_MACHINE, etc.) |
| `SRB_FLAGS` | MS-RSVD/Packet/RsvdMessage.cs | SCSI Request Block flags |
| `META_OPERATION_TYPE` | MS-RSVD/Packet/RsvdMessage.cs | Meta operation types (CreateSnapshot, Optimize, etc.) |
| `SNAPSHOT_TYPE` | MS-RSVD/Packet/RsvdMessage.cs | Snapshot types (DISK_SNAPSHOT, VM_SNAPSHOT) |
| `RsvdStatus` | MS-RSVD/Packet/RsvdMessage.cs | RSVD operation status codes |
| `Smb2CreateSvhdxOpenDeviceContext` | MS-RSVD/Packet/RsvdMessage.cs | SVHDX open device context structure |
| `SVHDX_TUNNEL_OPERATION_HEADER` | MS-RSVD/Packet/RsvdMessage.cs | Tunnel operation response header |
| `SVHDX_TUNNEL_FILE_INFO_RESPONSE` | MS-RSVD/Packet/RsvdMessage.cs | File info tunnel response |
| `SVHDX_TUNNEL_SCSI_RESPONSE` | MS-RSVD/Packet/RsvdMessage.cs | SCSI tunnel response |

**Usage in tests**:
```csharp
// Reference types directly from their namespace
OpenSharedVHD(fileName, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);
client.TunnelOperation<SVHDX_TUNNEL_FILE_INFO_RESPONSE>(...);
var originatorFlag = OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER;
```

## Best Practices

1. **Always use `:SharedVirtualDisk` suffix** when opening shared VHD files
2. **Set proper OriginatorFlags** (typically `SVHDX_ORIGINATOR_PVHDPARSER` for test clients)
3. **Include InitiatorId** in SVHDX open context (use `Guid.NewGuid()`)
4. **Verify response headers** after tunnel operations to check status codes
5. **Use inherited OpenSharedVHD()** helper for standard open operations
6. **Clean up resources** - call `client.CloseSharedVirtualDisk()` before test cleanup
7. **Handle both RSVD v1 and v2** - check version support in ptfconfig
8. **Request identifiers** - increment for each tunnel operation, independent of SMB2 message IDs
