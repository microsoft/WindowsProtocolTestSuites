# Server Failover Reference

This document provides guidance for writing server failover test cases.

## Required Using Statements

```csharp
using System;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Swn;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

Server failover tests validate cluster failover behavior:
- Transparent failover with persistent handles
- Witness protocol (MS-SWN) notifications
- Session and connection recovery

## Directory Structure

```
ServerFailover/
├── Adapter/
│   ├── ServerFailoverTestConfig.cs      # Configuration properties (cluster nodes, shares, timeouts)
│   └── ISutControlAdapter.cs            # Interface for cluster node control (enable/disable/move)
└── TestSuite/
    ├── ServerFailoverTestBase.cs        # Base class for all failover tests
    ├── FileServerFailoverBasic.cs       # Basic failover with durable handles
    ├── FileServerFailoverDurableHandleV2.cs  # DurableHandleV2 failover scenarios
    ├── FileServerFailoverWithLeasing.cs # Failover with leasing
    ├── FileServerFailoverWithLock.cs    # Failover with file locks
    ├── FileServerFailoverWithEncryption.cs   # Failover with encryption
    ├── FileServerFailoverExtendedTest.cs    # Extended failover scenarios
    ├── AsymmetricShare.cs               # Asymmetric share failover scenarios
    ├── FSRVP/                           # File Server Remote VSS Provider tests
    │   └── VSSOperateShadowCopySet.cs   # Shadow copy operations
    ├── SWN/                             # Server Witness Notification (MS-SWN) tests
    │   ├── SWNRegistration.cs           # SWN registration tests
    │   ├── SWNGetInterfaceList.cs       # Get interface list operations
    │   ├── SWNAsyncNotification.cs      # Async notification handling
    │   ├── SWNTestUtility.cs            # SWN utility methods
    └── ServerFailoverTestSuite.ptfconfig # Test configuration file
```

## Test Categories

```csharp
[TestCategory(TestCategories.Bvt)]                  // Built-in Verification Tests
[TestCategory(TestCategories.Smb311)]               // SMB 3.1.1 tests
[TestCategory(TestCategories.DomainRequired)]       // Domain environment required
[TestCategory(TestCategories.ClusterRequired)]      // Cluster environment required
[TestCategory(TestCategories.PersistentHandle)]     // Persistent handle tests
[TestCategory(TestCategories.Leasing)]              // Lease operations
[TestCategory(TestCategories.Encryption)]           // SMB encryption tests
```

## Test Base Class Pattern

All ServerFailover tests inherit from `ServerFailoverTestBase`, which extends `SMB2TestBase`:

```csharp
[TestClass]
public class FileServerFailover : ServerFailoverTestBase
{
    [ClassInitialize()]
    public static void ClassInitialize(TestContext testContext)
    {
        TestClassBase.Initialize(testContext);
        SWNTestUtility.BaseTestSite = BaseTestSite;  // For SWN tests
    }

    [ClassCleanup()]
    public static void ClassCleanup()
    {
        TestClassBase.Cleanup();
    }

    protected override void TestInitialize()
    {
        base.TestInitialize();  // Initializes sutController
        // Test-specific initialization
    }

    protected override void TestCleanup()
    {
        // Clean up SWN clients, handles, etc.
        base.TestCleanup();  // Calls RestoreServer()
    }
}
```

**Key inherited members from ServerFailoverTestBase**:
- `protected ISutControlAdapter sutController` - cluster control operations (enable/disable/move nodes)
- `protected ServerFailoverTestConfig TestConfig` - test configuration with cluster properties
- `protected Smb2FunctionalClient beforeFailover` - client for pre-failover state
- `protected string currentAccessNode` - current cluster node name
- `protected void FailoverClusterNode(string clusterNode)` - trigger failover
- `protected void RestoreServer()` - restore cluster to original state

## Common Test Patterns

### Basic Failover with Persistent Handle

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Smb311)]
[Description("Test transparent failover with persistent handle.")]
public void BVT_FileServerFailover_PersistentHandle()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create SMB2 client and open file with persistent handle.");
    var client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
    client.ConnectToServer(
        TestConfig.UnderlyingTransport,
        TestConfig.ClusteredFileServerName,
        TestConfig.ClusterIPAddress);

    client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
    client.SessionSetup(...);
    uint treeId;
    client.TreeConnect(TestConfig.ClusteredFileShare, out treeId);

    FILEID fileId;
    Guid createGuid = Guid.NewGuid();
    Smb2CreateContextResponse[] contexts;
    
    client.Create(
        treeId,
        fileName,
        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
        out fileId,
        out contexts,
        createContexts: new Smb2CreateContextRequest[]
        {
            new Smb2CreateDurableHandleRequestV2
            {
                CreateGuid = createGuid,
                Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT
            }
        });

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify persistent handle granted.");
    var durableResponse = contexts.OfType<Smb2CreateDurableHandleResponseV2>().First();
    BaseTestSite.Assert.IsTrue(
        (durableResponse.Flags & CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT) != 0,
        "Server should grant persistent handle");

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Get current cluster node.");
    AssignCurrentAccessNode(TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer, TestConfig.ClusterIPAddress);

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Trigger failover on current node.");
    FailoverClusterNode(currentAccessNode);

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Reconnect and recover persistent handle.");
    var client2 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
    client2.ConnectToServer(
        TestConfig.UnderlyingTransport,
        TestConfig.ClusteredFileServerName,
        TestConfig.ClusterIPAddress);
    
    client2.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
    client2.SessionSetup(...);
    uint treeId2;
    client2.TreeConnect(TestConfig.ClusteredFileShare, out treeId2);

    FILEID fileId2;
    Smb2CreateContextResponse[] contexts2;
    
    client2.Create(
        treeId2,
        fileName,
        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
        out fileId2,
        out contexts2,
        createContexts: new Smb2CreateContextRequest[]
        {
            new Smb2CreateDurableHandleRequestV2
            {
                CreateGuid = createGuid,
                Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT
            }
        });

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Verify handle recovery after failover.");
    var durableResponse2 = contexts2.OfType<Smb2CreateDurableHandleResponseV2>().First();
    BaseTestSite.Assert.IsTrue(
        (durableResponse2.Flags & CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT) != 0,
        "Persistent handle should be recovered");

    // Cleanup
    client2.Close(treeId2, fileId2);
    client2.TreeDisconnect(treeId2);
    client2.LogOff();
}
```

### SWN Registration and Witness Notification

```csharp
[TestMethod]
[TestCategory(TestCategories.Smb311)]
[TestCategory(TestCategories.DomainRequired)]
[Description("Test SWN registration on cluster.")]
public void SWN_Registration_OnCluster()
{
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create SWN client for interface list.");
    var swnClientForInterface = new SwnClient();
    
    BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Get SWN witness list from file server.");
    swnClientForInterface.SwnBind(
        TestConfig.ClusteredFileServerName,
        TestConfig.Timeout);
    
    var witnessList = swnClientForInterface.SwnGetInterfaceList(TestConfig.Timeout);
    BaseTestSite.Assert.IsNotNull(witnessList, "Server should return witness list");
    BaseTestSite.Assert.IsTrue(
        witnessList.Count > 0,
        "Witness list should contain at least one witness");

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Register as witness client with witness server.");
    var swnClientForWitness = new SwnClient();
    swnClientForWitness.SwnBind(
        witnessList[0].Ipv4Addr,
        TestConfig.Timeout);

    IntPtr pContext = IntPtr.Zero;
    WITNESS_RESOURCE witnessResource = new WITNESS_RESOURCE
    {
        ResourceName = TestConfig.ClusteredFileServerName,
        AccountName = TestConfig.DomainName + "\\" + TestConfig.UserName
    };

    uint registerStatus = swnClientForWitness.WitnessrRegister(
        witnessResource,
        out pContext,
        TestConfig.Timeout);

    BaseTestSite.Assert.AreEqual(
        0U,
        registerStatus,
        "SWN registration should succeed");

    BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Cleanup - unregister witness.");
    if (pContext != IntPtr.Zero)
    {
        swnClientForWitness.WitnessrUnRegister(pContext);
    }
    
    swnClientForInterface.SwnUnbind(TestConfig.Timeout);
    swnClientForWitness.SwnUnbind(TestConfig.Timeout);
}
```

## ServerFailoverTestConfig Properties

Configuration is loaded from `ServerFailoverTestSuite.ptfconfig`:

```csharp
public class ServerFailoverTestConfig : TestConfigBase
{
    // Cluster configuration
    public string ClusterNode01 { get; }           // First cluster node
    public string ClusterNode02 { get; }           // Second cluster node
    public string ClusteredFileServerName { get; } // Cluster network name
    public string ClusteredScaleOutFileServerName { get; }  // Scale-out cluster name
    
    // File shares
    public string ClusteredFileShare { get; }      // Clustered file share name
    public string ClusteredEncryptedFileShare { get; }     // Encrypted share
    public string AsymmetricShare { get; }         // Asymmetric share
    public string OptimumNodeOfAsymmetricShare { get; }    // Optimum node
    public string NonOptimumNodeOfAsymmetricShare { get; } // Non-optimum node
    
    // Timeouts
    public TimeSpan FailoverTimeout { get; }       // Failover completion timeout
    
    // Domain and credentials (inherited from TestConfigBase)
    public string DomainName { get; }
    public string UserName { get; }
    public string UserPassword { get; }
}
```

## ISutControlAdapter Methods

The `ISutControlAdapter` interface provides cluster node management:

| Method | Purpose |
|--------|---------|
| `void TriggerFailover(string ipAddress)` | Disable endpoint to trigger failover (Non-Windows) |
| `void RestoreToInitialState()` | Restore all endpoints (Non-Windows) |
| `string GetClusterNodeStatus(string nodeName)` | Get cluster service status |
| `bool EnableClusterNode(string nodeName)` | Enable cluster node (reboot in Windows) |
| `bool DisableClusterNode(string nodeName)` | Disable cluster node (stop service) |
| `string GetClusterResourceOwner(string resName)` | Get current resource owner node |
| `void MoveSmbWitnessClient(string clientName, string nodeName)` | Move witness client to new node |
| `void FlushDNS()` | Flush DNS cache |

**Usage in tests**:
```csharp
// Get SUT controller adapter
sutController = BaseTestSite.GetAdapter<ISutControlAdapter>();

// Disable node to trigger failover
sutController.DisableClusterNode(currentAccessNode);
sutController.FlushDNS();
System.Threading.Thread.Sleep(1000 * 30);  // Wait for cluster stability

// Restore nodes after test
sutController.EnableClusterNode(nodeName);
```

## Type Declarations (Where to Find/Add Definitions)

### SWN (Server Witness) Enums and Types
**File**: `ProtoSDK/MS-SWN/SwnMessage.cs`

Search for these type definitions (use `public enum` prefix):

| Type | Purpose |
|------|---------|
| `SwnMessageType` | Witness message type enum |
| `SwnResourceChangeType` | Resource change type enum |
| `SwnIPAddrInfoFlags` | IP address info flags |
| `SwnNodeState` | Node state enum (Online, Offline, etc.) |
| `SwnNodeFlagsValue` | Node flags |
| `WitnessrRegisterExFlagsValue` | Register flags |
| `SwnVersion` | Protocol version enum |
| `SWN_OPNUM` | RPC operation numbers |
| `SwnErrorCode` | Error codes enum |

### SMB2 Durable/Persistent Handle Types
**File**: `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`

Search for these type definitions:

| Type | Purpose |
|------|---------|
| `DurableRequest_Values` | Durable handle request values |
| `LeaseStateValues` | Lease state for persistent handles |

**To add new SWN types**: Add to `ProtoSDK/MS-SWN/SwnMessage.cs`

## Best Practices

1. **Always verify cluster environment** before cluster failover tests
2. **Use persistent handles** for transparent failover tests
3. **Clean up SWN resources** properly in TestCleanup (unregister witness)
4. **Include FlushDNS()** before and after failover to handle DNS caching
5. **Wait for cluster stability** after node failover (30+ seconds)
6. **Test both direction failovers** between cluster nodes when applicable
7. **Verify data integrity** after failover completion
8. **Use SWN for proactive failover notification** instead of client timeout-based detection
9. **Handle asymmetric shares correctly** - verify optimum/non-optimum node behavior
10. **Restore cluster state** in TestCleanup using inherited `RestoreServer()` method
