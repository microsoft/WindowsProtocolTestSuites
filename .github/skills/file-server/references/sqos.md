# SQOS Protocol Reference

This document provides guidance for writing Storage Quality of Service test cases.

## Required Using Statements

```csharp
using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

SQOS tests validate the implementation of:
- **MS-SQOS**: Storage Quality of Service Protocol

Used for managing storage QoS policies for virtual hard disks (VHDX files).

## Directory Structure

```
SQOS/
└── TestSuite/
    ├── SqosTestBase.cs      # Base class for SQOS tests (inherits CommonTestBase)
    ├── SqosTestConfig.cs    # SQOS-specific configuration
    ├── SqosBasic.cs         # BVT and basic functional tests
    └── SqosNegative.cs      # Negative/error condition tests
```

## Test Categories

```csharp
[TestCategory(TestCategories.Bvt)]       // Basic verification tests
[TestCategory(TestCategories.Sqos)]      // All SQOS tests
[TestCategory(TestCategories.NonSmb)]    // Non-SMB protocol tests
[TestCategory(TestCategories.UnexpectedFields)]   // Invalid field tests
[TestCategory(TestCategories.OutOfBoundary)]      // Boundary condition tests
[TestCategory(TestCategories.Compatibility)]      // Compatibility tests
```

## Key Classes

### SqosClient
**File**: `ProtoSDK/MS-SQOS/Client/SqosClient.cs`

The primary client class for SQOS operations:

```csharp
public sealed class SqosClient : IDisposable
{
    // Constructor
    public SqosClient(TimeSpan timeout);
    
    // Connection - establishes SMB2 connection and opens VHD file
    public uint ConnectToVHD(
        string serverName,
        IPAddress serverIP,
        string domain,
        string userName,
        string password,
        SecurityPackageType securityPackage,
        bool useServerToken,
        string shareName,
        string vhdName);
    
    // I/O Operations
    public uint Read(ulong offset, uint length, out byte[] data);
    public uint Write(ulong offset, byte[] data);
    
    // SQOS Operations
    public uint SendAndReceiveSqosPacket(SqosRequestPacket request, out SqosResponsePacket response);
    
    // Cleanup
    public void Close();
    public void Disconnect();
}
```

### SqosTestBase
**File**: `TestSuites/FileServer/src/SQOS/TestSuite/SqosTestBase.cs`

Base class for all SQOS tests:

```csharp
public class SqosTestBase : CommonTestBase
{
    protected SqosClient client;
    protected uint treeId;
    
    public SqosTestConfig TestConfig { get; }
    
    // Helper to connect to VHD file
    protected void ConnectToVHD();
}
```

### SqosTestConfig
**File**: `TestSuites/FileServer/src/SQOS/TestSuite/SqosTestConfig.cs`

Configuration properties for SQOS tests:

| Property | Type | Description |
|----------|------|-------------|
| `FileServerNameContainingSharedVHD` | string | Server hosting the shared VHD |
| `FileServerIPContainingSharedVHD` | IPAddress | IP address of the server |
| `ShareContainingSharedVHD` | string | Share name containing VHD |
| `NameOfSharedVHD` | string | VHD filename (default: "test.vhdx") |
| `SqosPolicyId` | Guid | QoS policy ID to use |
| `SqosInitiatorName` | string | Initiator name |
| `SqosInitiatorNodeName` | string | Node name (defaults to hostname) |
| `SqosMaximumIoRate` | ulong | Expected max IOPS |
| `SqosMinimumIoRate` | ulong | Expected min IOPS |
| `SqosBaseIoSize` | uint | Base I/O size in bytes |
| `SqosMaximumBandwidth` | ulong | Max bandwidth (v1.1 only) |
| `SqosClientDialect` | SQOS_PROTOCOL_VERSION | Protocol version (Sqos10 or Sqos11) |

## Common Test Patterns

### Set Policy to Logical Flow (BVT)

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Sqos)]
[TestCategory(TestCategories.NonSmb)]
[Description("This test case is to test if server can handle an SQOS request to set policy to a logical flow")]
public void BVT_Sqos_SetPolicy()
{
    ConnectToVHD();
    Guid logicalFlowId = Guid.NewGuid();
    Guid initiatorId = Guid.NewGuid();
    
    // Step 1: Associate Open to a logical flow
    SqosRequestPacket sqosRequest = new SqosRequestPacket(
        TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
        (ushort)TestConfig.SqosClientDialect,
        SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_LOGICAL_FLOW_ID,
        logicalFlowId,
        Guid.Empty,
        Guid.Empty,
        string.Empty,
        string.Empty);
    
    SqosResponsePacket sqosResponse;
    client.SendAndReceiveSqosPacket(sqosRequest, out sqosResponse);
    
    // Step 2: Set policy to the logical flow
    sqosRequest = new SqosRequestPacket(
        TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
        (ushort)TestConfig.SqosClientDialect,
        SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_POLICY,
        logicalFlowId,
        TestConfig.SqosPolicyId,
        initiatorId,
        TestConfig.SqosInitiatorName,
        TestConfig.SqosInitiatorNodeName);
    
    uint status = client.SendAndReceiveSqosPacket(sqosRequest, out sqosResponse);
    BaseTestSite.Assert.AreEqual(
        (uint)Smb2Status.STATUS_SUCCESS,
        status,
        "SetPolicy should succeed");
    
    // Step 3: Query status (with retry for server to compute rates)
    DoUntilSucceed(
        () => GetStatus(initiatorId, logicalFlowId),
        TestConfig.LongerTimeout,
        "Retry querying the logic flow status until succeed");
}
```

### Probe Policy

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Sqos)]
[TestCategory(TestCategories.NonSmb)]
[Description("This test case is to test if server can handle an SQOS request to probe policy to a logical flow")]
public void BVT_Sqos_ProbePolicy()
{
    ConnectToVHD();
    Guid logicalFlowId = Guid.NewGuid();
    Guid initiatorId = Guid.NewGuid();
    
    SqosRequestPacket sqosRequest = new SqosRequestPacket(
        TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
        (ushort)TestConfig.SqosClientDialect,
        SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_PROBE_POLICY,
        logicalFlowId,
        TestConfig.SqosPolicyId,
        initiatorId,
        TestConfig.SqosInitiatorName,
        TestConfig.SqosInitiatorNodeName);
    
    SqosResponsePacket sqosResponse;
    uint status = client.SendAndReceiveSqosPacket(sqosRequest, out sqosResponse);
    BaseTestSite.Assert.AreEqual(
        (uint)Smb2Status.STATUS_SUCCESS,
        status,
        "ProbePolicy should succeed");
}
```

### Update Counters

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Sqos)]
[TestCategory(TestCategories.NonSmb)]
[Description("This test case is to test if server can handle an SQOS request to update counters to a logical flow")]
public void BVT_Sqos_UpdateCounters()
{
    ConnectToVHD();
    Guid logicalFlowId = Guid.NewGuid();
    Guid initiatorId = Guid.NewGuid();
    
    // Associate and set policy first...
    
    // Perform I/O and measure latency
    byte[] payload;
    Stopwatch sw = Stopwatch.StartNew();
    client.Read(0, TestConfig.SqosBaseIoSize, out payload);
    sw.Stop();
    
    // Update counters with measured values
    SqosRequestPacket sqosRequest = new SqosRequestPacket(
        TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
        (ushort)TestConfig.SqosClientDialect,
        SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_UPDATE_COUNTERS,
        logicalFlowId,
        TestConfig.SqosPolicyId,
        initiatorId,
        TestConfig.SqosInitiatorName,
        TestConfig.SqosInitiatorNodeName,
        0,                          // limit
        0,                          // reservation
        1,                          // ioCountIncrement
        1,                          // normalizedIoCountIncrement
        (ulong)sw.ElapsedTicks,     // latencyIncrement (100-nanosecond units)
        (ulong)sw.ElapsedTicks,     // lowerLatencyIncrement
        0,                          // bandwidthLimit
        8);                         // kilobyteCountIncrement
    
    SqosResponsePacket sqosResponse;
    uint status = client.SendAndReceiveSqosPacket(sqosRequest, out sqosResponse);
    BaseTestSite.Assert.AreEqual(
        (uint)Smb2Status.STATUS_SUCCESS,
        status,
        "Update counters should succeed");
}
```

### Negative Test - Invalid Protocol Version

```csharp
[TestMethod]
[TestCategory(TestCategories.Sqos)]
[TestCategory(TestCategories.NonSmb)]
[TestCategory(TestCategories.UnexpectedFields)]
[Description("This test case is to test if server can handle an SQOS request with an invalid protocol version correctly")]
public void Sqos_InvalidProtocolVersion()
{
    ConnectToVHD();
    ushort invalidProtocolVersion = 0xFFFF;
    
    SqosRequestPacket sqosRequest = new SqosRequestPacket(
        TestConfig.SqosClientDialect == SQOS_PROTOCOL_VERSION.Sqos10 ? SqosRequestType.V10 : SqosRequestType.V11,
        invalidProtocolVersion,  // Invalid version
        SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_LOGICAL_FLOW_ID,
        Guid.NewGuid(),
        TestConfig.SqosPolicyId,
        Guid.NewGuid(),
        TestConfig.SqosInitiatorName,
        TestConfig.SqosInitiatorNodeName);
    
    SqosResponsePacket sqosResponse;
    uint status = client.SendAndReceiveSqosPacket(sqosRequest, out sqosResponse);
    BaseTestSite.Assert.AreEqual(
        (uint)NtStatus.STATUS_REVISION_MISMATCH,
        status,
        "Server MUST fail the request with STATUS_REVISION_MISMATCH");
}
```

## Type Declarations (Where to Find/Add Definitions)

### SQOS Enums and Structures
**File**: `ProtoSDK/MS-SQOS/Packet/SqosMessage.cs`

| Type | Purpose |
|------|---------|
| `SQOS_PROTOCOL_VERSION` | Protocol version enum (Sqos10 = 0x0100, Sqos11 = 0x0101) |
| `SqosRequestType` | Request structure type (V10, V11) |
| `SqosResponseType` | Response structure type (V10, V11) |
| `SqosOptions_Values` | Option flags (SET_LOGICAL_FLOW_ID, SET_POLICY, PROBE_POLICY, GET_STATUS, UPDATE_COUNTERS) |
| `LogicalFlowStatus` | Flow status (StorageQoSStatusOk, InsufficientThroughput, UnknownPolicyId, etc.) |
| `STORAGE_QOS_CONTROL_Header` | Common header for request/response |
| `STORAGE_QOS_CONTROL_Request_V10` | Request structure (v1.0) |
| `STORAGE_QOS_CONTROL_Request_V11` | Request structure (v1.1 - adds BandwidthLimit, KilobyteCountIncrement) |
| `STORAGE_QOS_CONTROL_Response_V10` | Response structure (v1.0) |
| `STORAGE_QOS_CONTROL_Response_V11` | Response structure (v1.1 - adds MaximumBandwidth) |

### Request/Response Packets
**File**: `ProtoSDK/MS-SQOS/Packet/SqosRequestPacket.cs`
**File**: `ProtoSDK/MS-SQOS/Packet/SqosResponsePacket.cs`

### SQOS IOCTL Code
**File**: `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`


Search for `FSCTL_STORAGE_QOS_CONTROL` in the `CtlCode_Values` enum.

**To add new SQOS types**: Add to `ProtoSDK/MS-SQOS/Packet/SqosMessage.cs`

## Status Values (LogicalFlowStatus)

```csharp
StorageQoSStatusOk = 0x00000000                    // Performance within constraints
StorageQoSStatusInsufficientThroughput = 0x00000001 // Cannot meet minimum throughput
StorageQoSUnknownPolicyId = 0x00000002             // Unknown policy ID
StorageQoSStatusConfigurationMismatch = 0x00000004  // Unsupported parameters
StorageQoSStatusNotAvailable = 0x00000005          // Status not available
```

## Response Packet Fields

```csharp
// From SqosResponsePacket (after parsing response)
sqosResponse.Header.ProtocolVersion   // Protocol version
sqosResponse.Header.Options           // Should be NONE
sqosResponse.Header.LogicalFlowID     // Logical flow GUID
sqosResponse.Header.PolicyID          // Policy GUID
sqosResponse.Header.InitiatorID       // Initiator GUID
sqosResponse.TimeToLive               // Validity period in milliseconds
sqosResponse.Status                   // LogicalFlowStatus
sqosResponse.MaximumIoRate            // Max IOPS (normalized)
sqosResponse.MinimumIoRate            // Min IOPS (normalized)
sqosResponse.BaseIoSize               // Base I/O size for normalization
sqosResponse.MaximumBandwidth         // Max bandwidth KB/s (v1.1 only)
```

## Available Test Cases

### Basic Tests (SqosBasic.cs)
| Test Method | Description |
|-------------|-------------|
| `BVT_Sqos_SetPolicy` | Set policy to a logical flow |
| `BVT_Sqos_ProbePolicy` | Probe policy for a logical flow |
| `BVT_Sqos_UpdateCounters` | Update I/O counters for a logical flow |

### Negative Tests (SqosNegative.cs)
| Test Method | Description |
|-------------|-------------|
| `Sqos_InvalidProtocolVersion` | Invalid protocol version returns STATUS_REVISION_MISMATCH |
| `Sqos_InvalidOption` | Invalid option (0) returns STATUS_INVALID_PARAMETER |
| `Sqos_InvalidPolicyId` | Invalid policy ID with Limit > 0 returns STATUS_INVALID_PARAMETER |
| `Sqos_ReservationGreaterThanLimit` | Reservation > Limit returns STATUS_INVALID_PARAMETER |
| `Sqos_InvalidInitiatorNameOffset_Small` | Small offset returns STATUS_INVALID_PARAMETER |
| `Sqos_InvalidInitiatorNameOffset_Large` | Large offset returns STATUS_INVALID_PARAMETER |
| `Sqos_InvalidInitiatorNodeNameOffset_Small` | Small offset returns STATUS_INVALID_PARAMETER |
| `Sqos_InvalidInitiatorNodeNameOffset_Large` | Large offset returns STATUS_INVALID_PARAMETER |
| `Sqos_SetPolicyToNonAssociatedLogicalFlow` | Set policy without association returns STATUS_NOT_FOUND |
| `Sqos_InvalidRequestType` | V1.0 structure with V1.1 version returns STATUS_INVALID_PARAMETER |

## Best Practices

1. **Always connect to VHD first** - Use `ConnectToVHD()` before any SQOS operations
2. **Associate before SetPolicy** - Must call SET_LOGICAL_FLOW_ID before SET_POLICY
3. **Use retry for GetStatus** - Server needs time to compute rates; use `DoUntilSucceed()`
4. **Match dialect and request type** - Use V10 request type for Sqos10, V11 for Sqos11
5. **Clean up resources** - Call `client.Close()` and `client.Disconnect()` in cleanup
6. **Use valid policy IDs** - Configure `SqosPolicyId` in ptfconfig from actual QoS policies
7. **Measure latency correctly** - Latency values are in 100-nanosecond units (same as Stopwatch ticks)

