# SMB2 Model-Based Testing Reference

This document provides guidance for SMB2 model-based tests using state machine-driven testing.

## Required Using Statements

```csharp
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

SMB2 Model tests use state machine-driven testing to systematically explore SMB2 protocol behaviors and state transitions. The testing framework:

- **Generates test cases** from protocol models (CORD files)
- **Tests state combinations** that might not be discovered through manual testing
- **Validates state transitions** across multiple operations
- **Coverage**: Negotiate, CreateClose, Oplock, Leasing, DurableHandle, Replay, Encryption, Signing, SessionMgmt, TreeMgmt, and more

## Directory Structure

```
SMB2Model/
├── Model/                          # State machine models (CORD language)
│   ├── [Feature]/                  # Feature-specific model directories
│   │   ├── Adapter/                # Feature adapter interfaces
│   │   └── Model.cord              # State machine definition
│   ├── ModelDataTypes.cs           # State enumerations and types
│   ├── ModelHelper.cs              # Helper methods for state machine
│   └── Common.cord                 # Shared model definitions
├── Adapter/                        # Implementation adapters
│   ├── [Feature]/
│   │   ├── [Feature]Adapter.cs     # Feature adapter implementation
│   │   ├── I[Feature]Adapter.cs    # Feature adapter interface
│   │   └── [Feature]Config.cs      # Feature configuration
│   ├── ModelManagedAdapterBase.cs  # Base adapter class
│   ├── SMB2ModelTestConfig.cs      # Test configuration properties
│   ├── ModelCommonDataTypes.cs     # Common data types and enums
│   └── ModelUtility.cs             # Utility methods for adapters
└── TestSuite/                      # Auto-generated test cases
    ├── [Feature]TestCase.cs        # Generated test cases from models
    └── *Scenario.cord              # Model-to-test mapping files
```

### Model Subdirectories (Features)

| Directory | Purpose |
|-----------|---------|
| **AppInstanceId** | Application instance ID negotiation |
| **CreateClose** | File creation and close operations |
| **Oplock** | Opportunistic locking behavior and conflicts |
| **Leasing** | Lease-related state transitions |
| **Handle** | Durable and resilient handle management |
| **Replay** | Request replay and idempotency |
| **Encryption** | Encryption/signing state management |
| **Signing** | Message signing verification |
| **SessionMgmt** | Session management and reconnection |
| **TreeMgmt** | Tree management and connection states |
| **CreditMgmt** | Credit/flow control management |
| **MixedOplockLease** | Combined oplock and lease behaviors |
| **Conflict** | Conflict detection and resolution |
| **ResilientHandle** | Resilient handle operations |
| **ValidateNegotiateInfo** | Validate negotiate info context |

## Test Base Class Pattern

Model-based tests are **auto-generated** from CORD (model definition language) files. The test class structure is:

```csharp
[TestClass]
public partial class CreateCloseTestCase : PtfTestClassBase
{
    // Auto-generated delegates for model actions
    public delegate void ReadConfigDelegate(CreateCloseConfig c);
    public delegate void CreateResponseDelegate(ModelSmb2Status status, CreateCloseConfig config);
    public delegate void CloseResponseDelegate(ModelSmb2Status status, QueryResponseStatus queryStatus);
    
    // Auto-generated adapter instance
    private ICreateCloseAdapter ICreateCloseAdapterInstance;
    
    // Auto-generated variables for model state
    private IVariable<CreateCloseConfig> config;
    private IVariable<ModelSmb2Status> status;
    private IVariable<int> queryStatus;
    
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
    
    [TestInitialize()]
    public void TestInitialize()
    {
        this.InitializeTestClass();
    }

    [TestCleanup()]
    public void TestCleanup()
    {
        this.CleanupTestClass();
    }
}
```

**Key points**:
- Test classes are **PARTIAL** - first part is auto-generated, second part contains manual test methods
- Tests inherit `PtfTestClassBase` instead of `SMB2TestBase`
- Adapters are injected via delegates
- State variables use `IVariable<T>` for model state tracking
- Test methods follow pattern: read config → actions → verify response

## Adapter Implementation Pattern

Feature-specific adapters implement the model-to-protocol translation:

```csharp
public class CreateCloseAdapter : ModelManagedAdapterBase, ICreateCloseAdapter
{
    private CreateCloseConfig createCloseConfig;
    private Smb2FunctionalClient testClient;
    private uint treeId;
    private FILEID fileID;
    
    // Events fired by adapter for model to consume
    public event CreateResponseEventHandler CreateResponse;
    public event CloseResponseEventHandler CloseResponse;
    
    public override void Initialize(ITestSite testSite)
    {
        base.Initialize(testSite);
        // Initialize test client and config
    }
    
    public override void Reset()
    {
        if (testClient != null)
        {
            testClient.Disconnect();
            testClient = null;
        }
        base.Reset();
    }
    
    // Model action: Read configuration
    public void ReadConfig(out CreateCloseConfig c)
    {
        c = new CreateCloseConfig
        {
            MaxSmbVersionServerSupported = 
                ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
            Platform = testConfig.Platform
        };
        createCloseConfig = c;
    }
    
    // Model action: Setup connection
    public void SetupConnection(ModelDialectRevision dialect)
    {
        testClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, Site);
        testClient.ConnectToServer(testConfig.UnderlyingTransport,
            testConfig.SutComputerName, testConfig.SutIPAddress);
        
        testClient.Negotiate(
            Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(dialect)),
            testConfig.IsSMB1NegotiateEnabled);
        
        testClient.SessionSetup(testConfig.DefaultSecurityPackage,
            testConfig.SutComputerName, testConfig.AccountCredential,
            testConfig.UseServerGssToken);
        
        testClient.TreeConnect(
            Smb2Utility.GetUncPath(testConfig.SutComputerName, 
                testConfig.BasicFileShare),
            out treeId);
    }
    
    // Model action: Create file
    public void SendCreateRequest(/* parameters */)
    {
        FILEID fileId;
        Smb2CreateContextResponse[] contexts;
        uint status = testClient.Create(treeId, fileName,
            CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
            out fileId, out contexts);
        
        // Fire event to notify model of response
        CreateResponse?.Invoke((ModelSmb2Status)status, createCloseConfig);
    }
    
    // Model action: Close file
    public void SendCloseRequest()
    {
        uint status = testClient.Close(treeId, fileID);
        CloseResponse?.Invoke((ModelSmb2Status)status, 
            QueryResponseStatus.FileNotFound);
    }
}

public interface ICreateCloseAdapter : IAdapter
{
    event CreateResponseEventHandler CreateResponse;
    event CloseResponseEventHandler CloseResponse;
    
    void ReadConfig(out CreateCloseConfig c);
    void SetupConnection(ModelDialectRevision dialect);
    void SendCreateRequest(/* parameters */);
    void SendCloseRequest();
}
```

## SMB2ModelTestConfig Properties

Configuration loaded from `MS-SMB2Model_ServerTestSuite.ptfconfig`:

```csharp
public class SMB2ModelTestConfig : TestConfigBase
{
    // Connection properties (same as SMB2TestConfig)
    public string SutComputerName { get; }
    public IPAddress SutIPAddress { get; }
    
    // Model-specific shares
    public string ShareWithoutForceLevel2OrSOFS { get; }      // For oplock tests
    public string ShareWithoutForceLevel2WithSOFS { get; }    // For SOFS oplock tests
    public string ShareWithForceLevel2WithoutSOFS { get; }    // For forced level2 tests
    public string ShareWithForceLevel2AndSOFS { get; }        // For forced level2 + SOFS
    public string ScaleOutFileServerName { get; }             // For scale-out scenarios
    
    // Tree management test shares
    public string SpecialShare { get; }
    
    // AppInstanceId test shares
    public string SameWithSMBBasic { get; }                   // Share for same app instance
    public string DifferentFromSMBBasic { get; }              // Share for different app instance
    
    // Helper method
    public string GetProperty(string propertyName, bool checkNullOrEmpty = true);
}
```

## Model State Management

### Core State Enumerations

```csharp
// Main model states (ModelDataTypes.cs)
public enum ModelState
{
    Uninitialized,      // Initial state before setup
    Initialized,        // Model initialized
    Connected,          // Connected to server
    Disconnected        // Disconnected state
}

// Session state
public enum ModelSessionState
{
    Initial,            // Before session setup
    SetupInProgress,    // During SESSION_SETUP
    Valid,              // Session established
    SessionExpired,     // Session timed out
    SessionTerminated   // Session closed
}

// File handle state
public enum ModelHandleState
{
    NotOpened,          // File not opened
    Opened,             // File opened
    Closed,             // File closed
}

// Oplock state
public enum ModelOplockState
{
    None,               // No oplock
    LevelII,            // Level II oplock
    Exclusive,          // Exclusive oplock
    Batch,              // Batch oplock
    ReadHandle,         // Read handle lease
    WriteHandle         // Write handle lease
}
```

### Model Helper Methods

```csharp
public static class ModelHelper
{
    // Determine negotiated dialect based on client and server capabilities
    public static DialectRevision DetermineNegotiateDialect(
        ModelDialectRevision clientSupported,
        ModelDialectRevision serverSupported);
    
    // Log model state/requirements
    public static void OutputModelStateInfo(string state);
    
    // Add logs with different prefixes
    public static void Log(LogType logtype, string log, params object[] args);
    
    // Retrieve and cast outstanding request
    public static T RetrieveOutstandingRequest<T>(ref ModelSMB2Request request) 
        where T : ModelSMB2Request;
    
    // Check error codes match expectations
    public static bool CheckMustErrorCode(ModelSmb2Status status, 
        ModelSmb2Status expectedError, bool checkMustError);
}

public enum LogType
{
    Requirement,    // Logs from specification (with [MS-SMB2] prefix)
    TestInfo,       // Test execution information
    TestTag         // Tags for test classification
}
```

## Key Concepts

- **State Machine Model**: CORD language definitions of protocol state and transitions
- **Adaptation**: Adapter implements model actions using Smb2FunctionalClient
- **Auto-Generation**: PTF framework generates test methods from model/adapter
- **Exhaustive Testing**: Models explore state combinations systematically
- **Event-Driven**: Adapter fires events that notify model of operation results
- **ModelSmb2Status**: Enum wrapper for SMB2 status codes
- **ModelDialectRevision**: Enum for dialect versions (different from DialectRevision)

## Best Practices

1. **Inherit from ModelManagedAdapterBase** in adapter implementations
2. **Use Smb2FunctionalClient** in adapters for protocol operations
3. **Fire events from adapters** to notify model of responses
4. **Handle Reset() properly** - clean up SMB2 client connections
5. **Use ModelUtility** for dialect conversion between Model enums and protocol enums
6. **Keep adapters focused** on specific features (CreateClose, Oplock, etc.)
7. **Test configuration** uses ptfconfig with feature-specific shares
8. **Log with LogType** for proper test result classification
9. **Share naming** follows patterns: ForceLevel2, WithSOFS, WithoutForceLevel2, etc.
10. **Variable tracking** - model maintains state via IVariable<T> objects

## Type Declarations (Where to Find/Add Definitions)

### SMB2 Model State and Rules
**Location**: `TestSuites/FileServer/src/SMB2Model/Model/`

| File | Purpose |
|------|---------|
| `ModelConfig.cs` | Model configuration |
| `[Feature]Model.cs` | Feature-specific model classes with [Rule] methods |

### SMB2 Model Adapter
**Location**: `TestSuites/FileServer/src/SMB2Model/Adapter/`

| File | Purpose |
|------|---------|
| `Smb2ModelAdapter.cs` | Translates model actions to SMB2 operations |

### Base SMB2 Types (used in models)
**File**: `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`

Models reference the same enums as traditional tests:
- `DialectRevision` - Protocol dialects
- `CreateOptions_Values` - Create options
- `LeaseStateValues` - Lease states
- `Capabilities_Values` - Server capabilities

**To add new SMB2 model rules**: Add to model files in `TestSuites/FileServer/src/SMB2Model/Model/`
