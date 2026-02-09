# FSA Model-Based Testing Reference

This document provides guidance for FSA model-based tests.

## Required Using Statements

```csharp
using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.FSAModel.Model;
using Microsoft.Protocols.TestSuites.FileSharing.FSAModel.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

FSA Model tests use state machine-driven testing to systematically explore file system behaviors:

- Model classes define abstract states and transitions
- Adapters translate model actions to concrete operations
- Enables comprehensive coverage of state combinations

## Directory Structure

```
FSAModel/
├── Model/                                    # State machine models for each feature
│   ├── BaseModel.cs                          # Base model with global state variables
│   ├── ModelHelper.cs                        # Helper methods and state enums
│   ├── CreateFile/
│   │   └── CreateFile.cs                     # CreateFile state machine
│   ├── OpenFile/
│   │   └── OpenFile.cs
│   ├── ReadFile/
│   │   └── ReadFile.cs
│   ├── WriteFile/
│   │   └── WriteFile.cs
│   ├── ChangeNotification/
│   │   └── ChangeNotificationModel.cs
│   ├── CloseFile/
│   │   └── CloseFile.cs
│   ├── LockAndUnlock/
│   │   └── LockAndUnlockModel.cs
│   ├── Oplock/
│   │   └── OplockModel.cs
│   ├── IOCTL/
│   │   └── [FSCTL models]
│   ├── QueryAndSetFileInformation/
│   │   └── QueryAndSetFileInformationModel.cs
│   ├── QueryAndSetFileSystemInformation/
│   │   └── QueryAndSetFileSystemInformationModel.cs
│   ├── QueryAndSetSecurityInformation/
│   │   └── QueryAndSetSecurityInformationModel.cs
│   ├── QueryAndSetQuotaInformation/
│   │   └── QueryAndSetQuotaInformationModel.cs
│   ├── QueryDirectory/
│   │   └── QueryDirectoryModel.cs
│   ├── FlushCachedData/
│   │   └── FlushCachedData.cs
│   └── BaseConfig.cord                       # Cordova model configuration
├── TestSuite/                                # Auto-generated test case classes
│   ├── ReadFileTestCase.cs                   # ReadFile test harness (auto-generated)
│   ├── WriteFileTestCase.cs                  # WriteFile test harness
│   ├── QueryFileInformationTestCase.cs       # QueryFileInformation test harness
│   ├── SetFileBasicInformationTestCase.cs    # SetFileBasicInformation test harness
│   ├── SetFileRenameInformationTestCase.cs   # SetFileRenameInformation test harness
│   ├── [Feature]TestCase.cs                  # One test harness per model feature
│   ├── MS-FSAModel_ServerModel.csproj
│   ├── MS-FSAModel_ServerModel.ptfconfig
│   └── MS-FSAModel_ServerModel.deployment.ptfconfig
└── Adapter/                                  # Protocol adapter for model operations
    └── IFSAAdapter.cs                        # Adapter interface used by test harnesses
```

**Key Structure Notes:**
- **BaseModel.cs**: Defines global FSA state variables (`fsStates`, `gOpenGrantedAccess`, `gSecurityContext`, etc.)
- **Model files**: Each feature has a model class with state transitions (CreateFile.cs, ReadFile.cs, etc.)
- **BaseConfig.cord**: Cordova model specification/configuration file
- **Auto-generated TestCase classes**: One per model, generated from Cordova
- **Test harnesses**: Use `IFSAAdapterInstance` to execute model transitions
- **State tracking**: Global state maintained across transitions (ModelProgram.fsStates)

## Model-Based Test Pattern

FSAModel tests are **auto-generated** from Cordova model specifications:

```csharp
[TestClass]
public partial class ReadFileTestCase : PtfTestClassBase
{
    #region Adapter Instances
    private IFSAAdapter IFSAAdapterInstance;
    #endregion

    #region Class Initialization and Cleanup
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
    #endregion

    #region Test Initialization and Cleanup
    protected override void TestInitialize()
    {
        this.InitializeTestManager();
        // Retrieve adapter instance for model operations
        this.IFSAAdapterInstance = 
            (IFSAAdapter)this.GetAdapter(typeof(IFSAAdapter));
    }

    protected override void TestCleanup()
    {
        base.TestCleanup();
        this.CleanupTestManager();
    }
    #endregion

    #region Test Methods
    [TestMethod]
    [TestCategory(TestCategories.Model)]
    [TestCategory(TestCategories.Fsa)]
    [TestCategory(TestCategories.ReadFile)]
    [TestCategory(TestCategories.Positive)]
    public void ReadFileTestCaseS0()
    {
        this.Manager.BeginTest("ReadFileTestCaseS0");
        
        // Initialize FSA
        this.Manager.Comment("executing step 'call FsaInitial()'");
        this.IFSAAdapterInstance.FsaInitial();
        
        // Check feature implementation flags
        bool isR507Implemented;
        this.Manager.Comment("executing step 'call CheckIsR507Implemented(out _)'");
        this.IFSAAdapterInstance.CheckIsR507Implemented(out isR507Implemented);
        
        // Get system configuration
        SSecurityContext securityContext;
        this.Manager.Comment("executing step 'call GetSystemConfig(out _)'");
        this.IFSAAdapterInstance.GetSystemConfig(out securityContext);
        
        // Model transitions and assertions generated by Cordova
        this.Manager.Comment("reaching state 'S0'");
        // ... [auto-generated state transitions] ...
    }
    #endregion
}
```

**Key Points About Auto-Generated Tests:**
- Test class and methods are auto-generated from BaseConfig.cord model specification
- `Manager.Comment()` traces state transitions
- `Manager.BeginTest()`, `Manager.EndTest()` bracket test execution
- Adapter methods map model actions to concrete FSA operations
- State transitions follow model definition (S0, S1, S6, S9, etc. are generated states)

## FSAModel Global State Management

FSAModel maintains global FSA state across transitions:

```csharp
// From BaseModel.cs - Global FSA State
public static class ModelProgram
{
    // FSA initialization state
    public static FSStates fsStates = FSStates.ReadyInitial;
    
    // SUT platform and OS information
    public static PlatformType sutPlatForm;
    public static SutOSInfo sutOSInfo = SutOSInfo.ReadyGetSutInfo;
    
    // Feature implementation flags
    static bool isR507Implemented;
    static bool isR405Implemented;
    
    // Security context
    static SSecurityContext gSecurityContext;
    
    // Open file state
    public static FileAccess gOpenGrantedAccess;
    public static FileAccess gOpenRemainingDesiredAccess;
    // ... [additional open state fields] ...
}
```

**State Tracking During Model Execution:**
- Global variables persist across adapter method calls
- Enables validation of state transitions and constraints
- Accessible to all model feature classes for cross-feature dependencies

## Model Feature Categories

FSAModel includes state machines for these file system operations:

| Model | Purpose |
|-------|---------|
| CreateFile | File/directory creation (attributes, options, access masks) |
| OpenFile | File open/reopen operations |
| ReadFile | File read operations (offset, length, constraints) |
| WriteFile | File write operations |
| CloseFile | File handle closing |
| ChangeNotification | Directory change notifications |
| LockAndUnlock | File byte range locking (exclusive, shared, unlock) |
| Oplock | Opportunistic locking state transitions |
| IOCTL | FSCTL operations (compression, sparse, etc.) |
| QueryAndSetFileInformation | Query/set file properties |
| QueryAndSetFileSystemInformation | Volume/filesystem properties |
| QueryAndSetSecurityInformation | Security descriptor operations |
| QueryAndSetQuotaInformation | Quota information |
| QueryDirectory | Directory enumeration |
| FlushCachedData | Cache flush operations |

## Test Execution Model

Model-based tests use **Cordova** (specifications language):

1. **BaseConfig.cord** defines model specification
2. Cordova compiler generates test harness classes (ReadFileTestCase.cs, etc.)
3. Test harnesses call `IFSAAdapterInstance` methods
4. Adapter translates to actual FSA/SMB operations
5. Manager tracks state transitions for validation
