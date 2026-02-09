# Windows Protocol Test Suites - Copilot Instructions

## Project Overview

Windows Protocol Test Suites provide interoperability testing for Windows open specifications across multiple protocol families (File Services, Identity Management, RDP, Kerberos, etc.). The codebase follows a consistent multi-suite architecture with shared infrastructure.

## Architecture

### Core Components

- **ProtoSDK**: Protocol library with message encoding/decoding and send/receive methods for each supported protocol
- **TestSuites**: Test suite implementations (11+ suites: FileServer, RDP Client/Server, Kerberos, ADFamily, BranchCache, SMBD, etc.)
- **ProtocolTestManager**: GUI tool for configuring and running test suites
- **CommonScripts**: Shared PowerShell scripts for environment deployment
- **Adapters**: Bridge between test cases and System Under Test (SUT), implementing [ManagedAdapterBase](../ProtoSDK)

### Test Suite Structure (Per Suite)

Each test suite follows this pattern (e.g., `TestSuites/FileServer/src/`):

```
[TestSuiteName]/
  ├── Adapter/         # Protocol adapters (inherit ManagedAdapterBase)
  ├── TestSuite/       # Test cases ([TestClass] with [TestMethod])
  ├── Plugin/          # Configuration UI plugin (XML, docs, data)
  ├── Setup/Scripts/   # Environment deployment scripts
  ├── Data/            # Test data files
  ├── [Protocol]*/     # One subdirectory per protocol (SMB2, DFSC, RSVD, etc.)
  └── build.ps1        # Suite-specific build script
```

## Build System

### Build Pattern

- **Language**: .NET 8.0 SDK (cross-platform: Windows, Linux, macOS)
- **Target Framework**: `net8.0` across all projects
- **Package Manager**: NuGet (Microsoft.Protocols.TestTools 2.6.1)
- **Build Tool**: PowerShell scripts (`build.ps1` in each suite)

### Build Commands

```powershell
# Per-suite build (from TestSuite/[Name]/src/)
.\build.ps1                    # Release mode, outputs to drop/TestSuites/[SuiteName]/
.\build.ps1 -Configuration Debug -OutDir "custom/path"

# Output structure after build
drop/TestSuites/[SuiteName]/
  ├── Bin/              # Compiled DLLs (test suite, adapters, ProtoSDK)
  ├── Batch/            # Test execution scripts (.ps1, .sh)
  ├── Scripts/          # Environment setup scripts
  ├── Plugin/           # GUI configuration plugin
  ├── Utils/            # Utility tools
  └── .version          # Assembly version info
```

### Key Dependencies

- Microsoft.Protocols.TestTools 2.6.1
- MSTest.TestFramework/Adapter 3.9+
- Microsoft.NET.Test.Sdk 17.14+
- Protocol-specific ProtoSDK packages (referenced via ProjectReference)

## ProtoSDK Architecture

### Overview

ProtoSDK is the shared protocol library containing message definitions, codecs (encoding/decoding), and transport mechanisms for all protocols. Each protocol family has its own folder (e.g., `MS-SMB2/`, `MS-RDPBCGR/`) and typically exposes:

- **Message Classes**: Data structures representing protocol messages (e.g., `PduMarshaller`, `StructureDefinitions`)
- **Codec Methods**: `Encode()` and `Decode()` for binary serialization
- **Transport/Channel**: `Send()` and `Receive()` methods for network communication
- **Context Classes**: Maintain protocol state (e.g., `ClientSecurityContext`, `ServerConnection`)

### Key ProtoSDK Modules

| Module                | Purpose                                                                         |
| --------------------- | ------------------------------------------------------------------------------- |
| **Common**            | Shared base classes, utilities, logging infrastructure                          |
| **Messages**          | Generic message handling patterns and base types                                |
| **TransportStack**    | Network transport implementations (TCP, UDP, named pipes, RDP virtual channels) |
| **Sspi/SspiLib**      | Security Support Provider Interface (NTLM, Kerberos, Negotiate authentication)  |
| **CryptoLib**         | Cryptographic operations (AES-CCM, AES-GCM, encryption/decryption)              |
| **Asn1Base**          | ASN.1 encoding/decoding infrastructure for LDAP, Kerberos                       |
| **MS-RPCE**           | Remote Procedure Call (RPC) over HTTP/SMB transport                             |
| **FileAccessService** | File system operations abstraction (used by FileServer test suite)              |
| **KerberosLib**       | Kerberos ticket validation and cryptographic operations                         |
| **Claim**             | Claims and authorization data structures                                        |

### Protocol-Specific Folders

Each MS-[PROTO] folder contains:

```
MS-SMB2/
  ├── [ProtocolName].csproj       # Package definition
  ├── Packets/                     # Message classes (SMB2Packet, Smb2Pdu, etc.)
  ├── Messages/                    # Request/response message definitions
  ├── Context/                     # Connection/session context (maintains state)
  ├── [ProtocolName]Client.cs      # High-level client API
  ├── [ProtocolName]Server.cs      # High-level server API
  └── Enums/Constants.cs           # Protocol constants and enumerations
```

### Usage Pattern in Adapters

```csharp
public partial class SmbAdapter : ManagedAdapterBase
{
    private Smb2Client client;
    private Smb2Credential credential;

    public override void Initialize(ITestSite testSite)
    {
        client = new Smb2Client(testSite);
        // Protocol initialization
    }

    public void SendMessage(Smb2Packet packet)
    {
        byte[] encoded = packet.GetBytes();
        client.SendBytes(encoded);
    }

    public Smb2Packet ReceiveMessage()
    {
        byte[] data = client.ReceiveBytes();
        Smb2Packet packet = Smb2Packet.FromBytes(data);
        return packet;
    }
}
```

### Common Patterns

**Message Encoding**: Most protocols use `GetBytes()` to serialize and `FromBytes()` to deserialize:

```csharp
byte[] encoded = messageObject.GetBytes();
MessageType decoded = MessageType.FromBytes(encoded);
```

**Context State**: Protocol contexts track connection/session state:

```csharp
var context = new ClientSecurityContext();
context.Initialize(credential);  // One-time setup
var token = context.GetToken();   // Per-operation token
```

**Event Instrumentation**: ProtoSDK uses EventSource for diagnostics. Event provider names follow pattern `Microsoft-WindowsProtocolsTestSuite-[Protocol]` (e.g., `Microsoft-WindowsProtocolsTestSuite-Kerberos`). Collect traces via: `dotnet-trace collect --providers Microsoft-WindowsProtocolsTestSuite-[Protocol] --process-id <pid>`

### Extending ProtoSDK

When adding new protocol support:

1. Create `ProtoSDK/MS-[PROTO]/` folder
2. Define message structures in `Packets/` subdirectory
3. Implement `Encode()` and `Decode()` methods
4. Add transport `Send()`/`Receive()` wrappers
5. Reference from adapter csproj via `ProjectReference`
6. Build succeeds only if all referenced types are resolvable

## TestSuite Structure and Patterns

### Configuration Management

Each test suite requires configuration files and classes to define SUT properties and test settings:

**PTFConfig Files** (XML-based):

- `[Suite].ptfconfig`: Runtime configuration with adapter definitions and properties
- `[Suite].deployment.ptfconfig`: Environment setup configuration
- Located in `TestSuite/` folder, copied to output via `CopyToOutputDirectory:Always`

**TestConfig Classes**:

- Inherit `TestConfigBase` from Common adapters (FileServer) or custom base (RDP, Kerberos, BranchCache)
- Load properties from `GetProperty("Group", "PropertyName")` method
- Example: [SMB2TestConfig.cs](../TestSuites/FileServer/src/SMB2/Adapter/SMB2TestConfig.cs)
- Properties include: SUT names/IPs, shares, credentials, feature flags, timeouts
- Pattern: Lazy-load configuration with cached fields to avoid repeated parsing
- **Protocol-Specific Configs**:
  - RDP: Includes `transportProtocol`, `rdpServerVersion`, `EncryptionLevel`, `supportCompression` (client capabilities)
  - Kerberos: Includes realm configuration, KDC addresses, proxy settings, cross-realm support
  - ADFamily: Uses `EnvironmentConfig` enum with Machine/User/Domain stores (WritableDC1, WritableDC2, RODC references)
  - BranchCache: Includes hosted cache server, content server, HTTP/SMB hash paths

```csharp
public class SMB2TestConfig : TestConfigBase
{
    public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
    {
        return GetProperty("SMB2", propertyName, checkNullOrEmpty);
    }

    public int WaitTimeoutInMilliseconds
    {
        get { return Int32.Parse(GetProperty("WaitTimeoutInMilliseconds")); }
    }
}
```

### Test Organization

Test cases follow strict MSTest patterns within protocol-specific folders:

**Test Class Structure**:

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

    protected override void TestInitialize() { }
    protected override void TestCleanup() { }

    [TestMethod]
    [TestCategory(TestCategories.Bvt)]
    [TestCategory(TestCategories.Smb2002)]
    public void BVT_SMB2Basic_TestName() { }
}
```

**Key Elements**:

- Inherit from protocol-specific base class (e.g., `SMB2TestBase`)
- `ClassInitialize` called once per class, receives `TestContext`
- `TestInitialize`/`TestCleanup` called per test method
- Multiple `[TestCategory]` attributes for filtering/grouping
- Test methods: 20-100 lines, clear arrange-act-assert flow
- Method naming: `[Category]_[Protocol]_[Scenario]`

**TestCategories** (filtering patterns):

- `Bvt`: Built-in verification tests (critical path)
- Protocol versions: `Smb2002`, `Smb21`, `Smb30`, `Smb302`, `Smb311`
- Features: `ChangeNotify`, `QueryDir`, `FileAccess`, `CreateClose`, `LockUnlock`
- Environment: `DomainRequired`, `OutOfBoundary`

### Test Execution Patterns

**Helper Methods** (in same test class):

```csharp
private void SmbClientConnect(Smb2FunctionalClient client, out uint treeId)
{
    client.ConnectToServer(TestConfig.UnderlyingTransport,
        TestConfig.SutComputerName, TestConfig.SutIPAddress);
    client.Negotiate(...);
    client.SessionSetup(...);
    client.TreeConnect(uncSharePath, out treeId);
}

private void SmbClientDisconnect(Smb2FunctionalClient client, uint treeId)
{
    client.TreeDisconnect(treeId);
    client.Logoff();
}
```

**Functional Client Pattern**:

- Use `Smb2FunctionalClient` (or protocol-specific equivalent) for high-level operations
- Methods return status codes (uint) for validation
- Event callbacks for async responses (e.g., `ChangeNotifyResponseReceived`)
- Automatic packet encoding/decoding via ProtoSDK

**Assertion Strategy**:

- Use `BaseTestSite.Assert.*()` for critical failures
- Use `BaseTestSite.Assume.*()` for prerequisites (skip if false)
- Use `BaseTestSite.Log.Add(LogEntryKind.*, "message")` for tracing
- Example: [SMB2Basic.cs](../TestSuites/FileServer/src/SMB2/TestSuite/Basic/SMB2Basic.cs)

### Multi-Protocol Suite Organization

Complex suites (e.g., FileServer) contain multiple protocol folders:

```
FileServer/src/
  ├── SMB2/          # SMB2 protocol tests (traditional)
  ├── SMB2Model/     # SMB2 model-based tests (state machine-driven)
  ├── DFSC/          # DFS referral protocol tests
  ├── FSRVP/         # File server VSS provider tests
  ├── RSVD/          # Remote shared virtual disk tests
  ├── FSA/           # File system access model (traditional)
  ├── FSAModel/      # FSA model-based tests (state machine-driven)
  ├── Common/        # Shared adapters, base classes
  └── [Protocol]/
```

Each protocol folder has identical structure: `Adapter/` and `TestSuite/` subdirectories.
Shared classes in `Common/` are referenced by all protocol tests via ProjectReference.

**Model-Based Testing (FileServer only)**:

- SMB2Model and FSAModel suites use state machine-driven tests
- Organized with `Model/` and `Adapter/` directories
- Model classes track state (e.g., `ModelState.Connected`) and validate state transitions
- Adapter translates model actions into protocol operations
- Enables systematic exploration of protocol behaviors and edge cases

## Code Patterns

### Test Base Class Hierarchy

Different protocols use different base classes optimized for their testing needs:

- **FileServer**: Inherits `SMB2TestBase` (SMB2) or custom bases for other protocols
- **RDP**: Uses `RdpTestClassBase` with built-in adapter management and RDP-specific helpers (connection startup, capability negotiation)
- **Kerberos**: Uses `TraditionTestBase` with KKDCP proxy support and cross-realm configuration
- **ADFamily**: Uses `TestClassBase` with environment machine enumeration (WritableDC1, WritableDC2, RODC)
- **BranchCache**: Uses `BranchCacheTestClassBase` with content server and hosted cache management
- **MS-SMBD**: Uses `Smb2OverSmbdTestBase` for RDMA operations over SMB2

### Adapter Implementation

All adapters inherit `ManagedAdapterBase` (from Microsoft.Protocols.TestTools). Pattern:

```csharp
public partial class [ProtocolName]Adapter : ManagedAdapterBase
{
    // Initialize method called before each test
    public override void Initialize(ITestSite testSite) { }

    // Methods called by test cases
    public void ProtocolOperation() { }
}
```

Adapters are split across multiple partial class files (e.g., `RdpbcgrAdapter_ReceiveMethod.cs`).

### Test Case Organization

- Use `[TestClass]` on classes, `[TestMethod]` on test methods
- ClassInitialize receives `TestContext` parameter
- Test methods typically 20-100 lines, with clear arrange-act-assert pattern
- Tests are protocol-focused (not generic), testing specific message exchanges

### Configuration (.ptfconfig files)

- XML-based configuration per test suite (stored in TestSuite folder)
- Two variants: `[Suite].ptfconfig` (runtime config) and `[Suite].deployment.ptfconfig` (environment setup)
- Contains adapters, properties, and SUT configuration

### Instrumentation (EventSource pattern)

- Providers named: `Microsoft-WindowsProtocolsTestSuite-[ProtocolName]`
- Event methods follow: `ClassName_MethodNameAction` (e.g., `ClientSecurityContext_InitializationStarted`)
- Event IDs: 1-20 for constructors, 100+ for class methods
- Use primitives or `[EventData]` decorated types for event parameters

## Key Workflows

### Running Tests

1. **Via Batch Scripts** (post-build in `drop/TestSuites/[Suite]/Batch/`):
   - PowerShell: `RunTestCasesByBinariesAndFilter.ps1 -Filter "TestMethodName"`
   - Shell: `RunTestCasesByBinariesAndFilter.sh`

2. **Via ProtocolTestManager GUI**: Load `[Suite].ptfconfig`, select tests, run

3. **Via VSTest** (direct):
   ```powershell
   vstest.console.exe Bin/[TestAssembly].dll /Tests:TestClass.TestMethod
   ```

### PowerShell Dependency Pattern

Test suites heavily use PowerShell scripts in adapters:

- Scripts stored as `.ps1` files, copied to output via `CopyToOutputDirectory:Always` in .csproj
- Used for SUT management (user/group queries, file system ops, service control)
- Executed via managed adapter methods to remote systems via WinRM

## Important Conventions

### Naming

- Test classes: `[ProtocolAbbrev]TestSuite` (e.g., `RdpbcgrTestSuite`)
- Adapter classes: `[ProtocolAbbrev]Adapter` (e.g., `RdpemtAdapter`)
- ProjectReferences use full paths: `..\..\..\..\ProtoSDK\MS-[PROTO]\[Proto].csproj`

### Assembly Configuration

- All test projects use `GenerateAssemblyInfo=false` (shared [SharedAssemblyInfo.cs](../AssemblyInfo/SharedAssemblyInfo.cs))
- Version linked from single source: `AssemblyInfo/SharedAssemblyInfo.cs`
- Output paths: DLLs in `Bin/`, scripts in `Scripts/`, data in `Bin/Data/`

### Testing Frameworks

- MSTest (not xUnit/NUnit)
- Protocol Test Framework (PTF) 2.6.1 for adapter infrastructure
- Custom test patterns (not standard MSTest Assert patterns) for protocol validation

## References & Key Files

- [README.md](../README.md) - Overview of all test suites
- [CONTRIBUTING.md](../CONTRIBUTING.md) - CLA, code review requirements
- [ProtoSDK/README.md](../ProtoSDK/README.md) - EventSource instrumentation details
- [TestSuites/FileServer/src/build.ps1](../TestSuites/FileServer/src/build.ps1) - Build pattern reference
- [NuGet.config](../NuGet.config) - Package source configuration

## Common Tasks

**Add a new test case**: Create `[Name].cs` in `TestSuite/` folder, inherit test class pattern, use adapter methods
**Add protocol support**: Create new `[ProtocolAbbrev]/` subdirectory, add ProtoSDK reference, adapter, test cases
**Debug adapter issues**: Check `.ptfconfig` SUT properties, adapter initialization, PowerShell script paths
**Build verification**: Run suite's `build.ps1`, verify `drop/TestSuites/[Suite]/Bin/` contains expected DLLs

## Skills

Skills provide domain-specific knowledge for Copilot to assist with protocol-related tasks. They are located in `.github/skills/` with each skill in its own directory.

### Skill Directory Structure

Every skill directory **MUST** contain a `SKILL.md` file:

```
.github/skills/
  ├── [skill-name]/
  │   ├── SKILL.md           # Required: Skill instructions
  │   └── references/        # Optional: Additional resource files
  │       ├── protocol1.md
  │       └── protocol2.md
```

### Progressive Disclosure Model

Skills use a three-step progressive disclosure model to optimize context window usage:

| Step             | When Loaded             | Token Cost            | Content                                        |
| ---------------- | ----------------------- | --------------------- | ---------------------------------------------- |
| **Frontmatter**  | Always (at startup)     | ~100 tokens per skill | `name` and `description` from YAML frontmatter |
| **Instructions** | When skill is triggered | Under 5k tokens       | `SKILL.md` body with instructions and guidance |
| **Resources**    | As needed               | Effectively unlimited | Bundled files in `references/` directory       |

This ensures only relevant content occupies the context window at any given time.

### SKILL.md Format

Each `SKILL.md` must include a YAML frontmatter with metadata, followed by instructions:

```markdown
---
name: skill-name
description: Brief description of what this skill helps with and when it will be used.
license: MIT
metadata:
  author: Microsoft
  version: "1.0"
---

# Skill Title

Instructions and guidance content here (Instructions).

## Reference Links

Link to files in references/ directory for Resources content:

- [Reference Name](references/topic.md) - When to use this reference
```

### Writing Effective Skills

**Frontmatter**:

- `name`: Short, lowercase, hyphen-separated identifier
- `description`: Concise summary (1-2 sentences) describing when this skill is useful

**Instructions**:

- Keep under 5,000 tokens
- Provide actionable guidance, patterns, and quick-reference tables
- Include links to Resources for detailed information

**Resources**:

- Place detailed protocol-specific documentation in `references/` subdirectory
- These files are loaded only when explicitly needed
- Can reference code files, detailed specifications, or examples

### Existing Skills

| Skill                    | Description                                            |
| ------------------------ | ------------------------------------------------------ |
| `file-server`            | FileServer protocol test cases, adapters, transports   |
| `kerberos`               | Kerberos protocol specification guidance               |
| `rdp`                    | RDP protocol specification guidance                    |
| `ad-family`              | AD-Family test suite guidance                          |
| `smbd`                   | MS-SMBD protocol specification guidance                |
| `generate-protocol-diff` | Generate diffs between protocol specification versions |
