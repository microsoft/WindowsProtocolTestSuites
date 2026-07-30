# Protocol Test Manager (PTM) Guide

## What PTM Is

Protocol Test Manager (PTM) is an ASP.NET Core 8.0 web application that provides a browser-based UI for:
- Installing and managing test suite packages
- Creating and editing test suite configurations (mapping `.ptfconfig` properties to form fields)
- Running auto-detection to discover SUT capabilities
- Selecting and executing test cases with category filters
- Viewing real-time and historical test results

PTM is the intended end-user interface for running the test suites. Command-line execution via vstest or batch scripts is also supported and is used in CI pipelines.

**Root path:** `c:\Users\jomitiran\source\repos\WindowsProtocolTestSuites\ProtocolTestManager\`

## Architecture

```
ProtocolTestManager/
├── PTMService/
│   ├── PTMService/           ASP.NET Core host + React SPA
│   ├── PTMKernelService/     Core business logic
│   ├── Abstractions/         Kernel interfaces
│   ├── Common/               Shared types and entities
│   ├── Database/             EF Core DbContext, SQLite
│   ├── Storage/              File system storage abstraction
│   └── UnitTest/             PTM unit tests
├── Plugins/                  Per-suite detection plugins
├── Kernel/                   Legacy PTM kernel (older version)
├── PtmCli/                   CLI runner
└── SampleDetector/           Example detector plugin
```

## Component Breakdown

### PTMService (Web Host)

**Path:** `ProtocolTestManager/PTMService/PTMService/`
**Project:** `PTMService.csproj`

ASP.NET Core app. Key files:
- `Program.cs` / `Startup.cs` — dependency injection setup, middleware pipeline
- `Controllers/` — REST API controllers (see below)
- `ClientApp/` — React SPA (built separately; served as static files)
- `appsettings.json` — configuration (storage paths, DB connection string)

### REST API Controllers

**Path:** `PTMService/PTMService/Controllers/`

| Controller | Endpoints | Purpose |
|---|---|---|
| `TestSuiteManagementController` | POST/PUT/DELETE `/api/TestSuiteManagement` | Install, update, remove test suite packages |
| `TestSuiteInfoController` | GET `/api/TestSuiteInfo` | Query installed test suites |
| `TestSuiteConfigurationController` | CRUD `/api/TestSuiteConfiguration` | Manage configurations (ptfconfig property sets) |
| `TestSuiteAutoDetectionController` | POST `/api/TestSuiteAutoDetection` | Trigger capability auto-detection against SUT |
| `TestSuiteRunController` | POST/GET `/api/TestSuiteRun` | Start test runs, stream results |
| `TestResultController` | GET `/api/TestResult` | Query historical test results |
| `CapabilitiesInfoController` | GET `/api/CapabilitiesInfo` | Query detected SUT capabilities |
| `CapabilitiesManagementController` | PUT `/api/CapabilitiesManagement` | Update capability configuration |

All controllers extend `PTMServiceControllerBase` and inject `IScopedServiceFactory` to get `IPTMKernelService`.

### PTMKernelService

**Path:** `ProtocolTestManager/PTMService/PTMKernelService/`
**Project:** `PTMKernelService.csproj`

The business logic layer. `PTMKernelService.cs` is the main implementation class implementing `IPTMKernelService`. It is split across partial class files:

| File | Responsibility |
|---|---|
| `PTMKernelService.cs` | Constructor, DI setup, common helpers |
| `PTMKernelService_TestSuite.cs` | Install/update/remove test suites (unzip packages, register in DB) |
| `PTMKernelService_Configuration.cs` | Create/read/update/delete configurations; write ptfconfig XML |
| `PTMKernelService_AutoDetect.cs` | Run detector plugin, collect capabilities |
| `PTMKernelService_Capabilities.cs` | Read/write capabilities configuration |
| `PTMKernelService_TestRun.cs` | Execute vstest, stream results via pipe |
| `PTMKernelService_ProfileSetup.cs` | Profile import/export |

**Key methods on `IPTMKernelService`:**

```csharp
// Test suite management
ITestSuite[] QueryTestSuites();
int InstallTestSuite(string name, string packageName, Stream package, string description);
void RemoveTestSuite(int id);

// Configuration
IConfiguration[] QueryConfigurations(int? testSuiteId);
int CreateConfiguration(string name, int testSuiteId, string description);
void UpdateConfigurationProperties(int configId, IEnumerable<ConfigurationProperty> properties);

// Auto-detection
void StartAutoDetection(int configId, DetectionCallback callback);
DetectionOutcome GetAutoDetectionResult();

// Test execution
int CreateTestRun(int configId, IEnumerable<string> selectedTestCases);
void StartTestRun(int testRunId);
TestRunResult GetTestRunResult(int testRunId);
```

### Abstractions

**Path:** `ProtocolTestManager/PTMService/Abstractions/`

Interface definitions for all kernel entities:

| Interface | File | Description |
|---|---|---|
| `IPTMKernelService` | `Kernel/IPTMKernelService.cs` | Full kernel service contract |
| `ITestSuite` | `Kernel/ITestSuite.cs` | Installed test suite entity |
| `IConfiguration` | `Kernel/IConfiguration.cs` | Configuration entity |
| `ITestRun` | `Kernel/ITestRun.cs` | Test run state and results |
| `IAutoDetection` | `Kernel/IAutoDetection.cs` | Auto-detection result |

### Plugins (Auto-Detection)

**Path:** `ProtocolTestManager/Plugins/`

Each test suite has a corresponding detector plugin. The plugin implements `IValueDetector` (from the PTF `Microsoft.Protocols.TestManager.Detector` namespace) and provides:
- `GetDetectedProperty(string name)` — returns detected property values for the test suite configuration
- `RunDetection()` — performs actual network probing/WMI/registry queries against the SUT

PTM monitors detector progress and may treat an unresponsive detector as failed based on the service's configured monitoring behavior. Each run has isolated cancellation, terminal state, detector, and log state so a non-cooperative detector cannot affect or block a later run. Incremental log chunks include the run ID and an `IsComplete` flag; the React client rejects stale chunks and continues fetching until the detector has stopped and the log writer has flushed all content.

| Plugin | Suite |
|---|---|
| `FileServerPlugin/` | FileServer |
| `RDPServerPlugin/` | RDP Server |
| `RDPClientPlugin/` | RDP Client |
| `KerberosPlugin/` | Kerberos |
| `SMBDPlugin/` | MS-SMBD |
| `ADFamilyPlugin/` | ADFamily |
| `ADFSPIPPlugin/` | MS-ADFSPIP |
| `ADODPlugin/` | MS-ADOD |
| `AZODPlugin/` | MS-AZOD |
| `BranchCachePlugin/` | BranchCache |

**Plugin discovery:** When a test suite package is installed, PTM looks for a plugin DLL in the package's `Plugin/` subdirectory. If found, it loads it and uses it for auto-detection.

### PtmCli

**Path:** `ProtocolTestManager/PtmCli/`

A command-line interface for PTM operations. Useful for headless CI scenarios. Supports:
- Install/run test suites without the web UI
- Specify configuration via command-line arguments

## How PTM Runs Tests

1. PTM receives a `StartTestRun` request with a configuration ID and selected test case names.
2. `PTMKernelService_TestRun.cs` resolves the test suite binary path from the installed package.
3. It constructs a `dotnet vstest` (or `vstest.console.exe`) command with a filter expression derived from the selected test cases.
4. The process is started with `stdout` redirected. Output is parsed line by line to extract test pass/fail events.
5. A `PipeSink` (configured in `.ptfconfig`) sends real-time PTF log events to PTM. PTM correlates each pipe connection with the fully qualified test name, buffers logs by that name, and serves them through the per-test result endpoint while the UI polls.
6. Results are stored in the SQLite database via `TestRunResult` entities.

## How to Add Test Suite Support to PTM

To make a new test suite work in PTM:

### 1. Create a detector plugin

Create a new project under `ProtocolTestManager/Plugins/<SuiteName>Plugin/`. Implement `IValueDetector`:

```csharp
public class MySuiteDetector : IValueDetector
{
    public bool RunDetection()
    {
        // Probe SUT: ping, WMI query, registry check, etc.
        // Populate internal state with detected values
        return true;
    }

    public string GetDetectedProperty(string propertyName)
    {
        return detectedValues.TryGetValue(propertyName, out var val) ? val : string.Empty;
    }
}
```

### 2. Create a plugin descriptor XML

In the plugin project, add an XML descriptor file (see existing plugins for format) that maps test suite property names to UI labels, groups, and detection logic.

### 3. Package the test suite

The `build.ps1` script for the test suite should output:
```
drop/TestSuites/<SuiteName>/
├── Bin/                   Test suite .dll and dependencies
├── Plugin/                Detector plugin .dll + descriptor XML
├── Batch/                 Batch scripts
└── Scripts/               Environment scripts
```

The `Plugin/` subdirectory contents are what PTM loads for auto-detection.

### 4. Install in PTM

Use PTM's "Install Test Suite" UI or the PtmCli to install the package ZIP. PTM will:
- Unzip to its storage directory
- Register the test suite in the DB
- Load the plugin DLL for future auto-detection runs

## Build PTM

```powershell
cd ProtocolTestManager/PTMService
./build.ps1
```

Or build the solution:
```powershell
dotnet build ProtocolTestManager/PTMService/PTMService.sln
```

## Run PTM Locally

```powershell
cd ProtocolTestManager/PTMService/PTMService
dotnet run
```

Then navigate to `http://localhost:5000` (or the configured port).

## Key Configuration

`PTMService/PTMService/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ptm.db"
  },
  "Storage": {
    "TestSuiteStoragePath": "./TestSuiteStorage"
  }
}
```

`TestSuiteStoragePath` is where installed test suite packages are extracted.
