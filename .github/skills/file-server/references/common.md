# Common Infrastructure Reference

This document describes the shared infrastructure used across all FileServer protocol tests, including base classes, configuration, SMB2 client, and SUT adapters.

## Required Using Statements

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiService;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Directory Structure

```
Common/
├── Adapter/
│   ├── Smb2FunctionalClient.cs              # High-level SMB2 client
│   ├── TestConfigBase.cs                    # Base configuration class
│   ├── TestCategories.cs                    # Test category constants
│   ├── SutProtocolControlAdapter.cs         # Protocol control adapter implementation
│   ├── SutCommonControlAdapter.cs           # Common SUT control implementation
│   ├── SutCommonControlAdapterAccessor.cs   # Accessor for SutCommonControlAdapter
│   ├── SutCommonControlDataTypes.cs         # Data types for SUT control
│   ├── ISutProtocolControlAdapter.cs        # Protocol control interface
│   ├── ISutCommonControlAdapter.cs          # Common control interface
│   ├── CommonRequirementHelper.cs           # Requirement tracking helper
│   └── [Protocol]TestConfig.cs              # Protocol-specific configs (inherit TestConfigBase)
└── TestSuite/
    ├── CommonTestBase.cs                    # Base class for all tests
    └── CommonTypes.cs                       # Shared type definitions
```

## CommonTestBase

**File**: `TestSuite/CommonTestBase.cs`

Base class for all FileServer tests inheriting from `TestClassBase`. Manages test lifecycle, auto-cleanup, and retry logic.

### Properties

```csharp
public abstract partial class CommonTestBase : TestClassBase
{
    protected TestConfigBase testConfig;                          // Config for current suite
    protected ISutProtocolControlAdapter sutProtocolController;   // SUT control adapter
    protected List<string> testDirectories = new List<string>();  // Auto-cleanup registry
    protected List<string> testFiles = new List<string>();        // Auto-cleanup registry
    
    /// <summary>
    /// Current test case name (derived from TestProperties)
    /// </summary>
    protected string CurrentTestCaseName { get; }
    
    /// <summary>
    /// Cache of loaded test assemblies for description lookup
    /// </summary>
    protected Dictionary<string, Assembly> TestcaseAssemblies { get; set; }
}
```

### Test Lifecycle Methods

```csharp
protected override void TestInitialize()
{
    base.TestInitialize();
    // Logs test case description from [Description] attribute
    LogTestCaseDescription();
    // Gets SUT control adapter from PTF framework
    sutProtocolController = BaseTestSite.GetAdapter<ISutProtocolControlAdapter>();
}

protected override void TestCleanup()
{
    // Auto-delete all directories created via CreateTestDirectory()
    foreach (var directory in testDirectories)
    {
        try
        {
            sutProtocolController.DeleteDirectory(
                Smb2Utility.GetShareName(directory),     // Extract share name
                Smb2Utility.GetFileName(directory));     // Extract relative path
        }
        catch { }  // Ignore failures (file may not exist)
    }
    
    // Auto-delete all files created via GetTestFileName()
    foreach (var fileName in testFiles)
    {
        try
        {
            sutProtocolController.DeleteFile(
                Smb2Utility.GetShareName(fileName),      // Extract share name
                Smb2Utility.GetFileName(fileName));      // Extract relative path
        }
        catch { }  // Ignore failures
    }
    
    base.TestCleanup();
}

/// <summary>
/// Called from TestInitialize to log [Description] attribute of test method
/// </summary>
protected void LogTestCaseDescription()
{
    // Loads test assembly and extracts Description attribute from current test method
    // Logs via BaseTestSite.Log.Add(LogEntryKind.Comment, attribute.Description)
}
```

### File and Directory Management

```csharp
/// <summary>
/// Register file for auto-cleanup and return unique file name
/// Name: [TestCaseName]_[GUID]
/// </summary>
protected string GetTestFileName(string share)
{
    string fileName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
    testFiles.Add(string.Format(@"{0}\{1}", share, fileName));  // Register for cleanup
    return fileName;  // Return just the filename part
}

/// <summary>
/// Manually register file for auto-cleanup (if created externally)
/// </summary>
protected void AddTestFileName(string share, string fileName)
{
    testFiles.Add(string.Format(@"{0}\{1}", share, fileName));
}

/// <summary>
/// Create test directory, register for auto-cleanup
/// Name: [TestCaseName]_[GUID] or [ParentDirectory]\[TestCaseName]_[GUID]
/// </summary>
protected string CreateTestDirectory(string uncSharePath)
{
    // Create directory and register for cleanup
    // Logs: "Create a directory {0} in the share {1}."
    return directoryName;  // Returns: TestCaseName_GUID
}

/// <summary>
/// Create test directory under specified parent directory
/// </summary>
protected string CreateTestDirectory(string server, string share, string parentDirectoryName)
{
    // Creates: \\server\share\ParentDirectory\TestCaseName_GUID
    return CreateTestDirectoryInternal(
        string.Format(@"\\{0}\{1}", server, share),
        parentDirectoryName);
}

/// <summary>
/// Create test directory on specified server/share
/// </summary>
protected string CreateTestDirectory(string server, string share)
{
    return CreateTestDirectoryInternal(
        string.Format(@"\\{0}\{1}", server, share),
        null);  // No parent directory
}

/// <summary>
/// Get directory name (register for cleanup but don't create)
/// Use when creating directory outside of protocol operations
/// </summary>
protected string GetTestDirectoryName(string share)
{
    string directoryName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
    testDirectories.Add(string.Format(@"{0}\{1}", share, directoryName));
    return directoryName;
}

/// <summary>
/// Manually register directory for auto-cleanup (if created externally)
/// </summary>
protected void AddTestDirectoryName(string share, string directoryName)
{
    testDirectories.Add(string.Format(@"{0}\{1}", share, directoryName));
}
```

### Retry Logic

```csharp
/// <summary>
/// Retry operation returning uint status until success (status == 0) or timeout
/// Logs retry attempts and duration
/// Throws InvalidOperationException if timeout or connection closed
/// </summary>
protected uint DoUntilSucceed(
    Func<uint> func,           // Operation returning uint status (0 = success)
    TimeSpan timeout,          // Maximum retry duration
    string format,             // Log message format string
    params object[] args)      // Log message parameters
{
    DateTime endTime = DateTime.Now.Add(timeout);
    bool isUnderlyingConnectionClosed = false;
    uint retryCount = 0;
    uint result = 1;
    
    // Retries until:
    // - result == 0 (success)
    // - DateTime.Now >= endTime (timeout)
    // - Connection closed exception (stops immediately)
    
    // Logs: "Retry {N}" for each retry
    // Logs: "Throw an exception: {message}"
    // Logs: "Retry {succeed|fail} after retry duration: {duration}"
    
    if (result != 0)
        throw new InvalidOperationException(
            String.Format("Retry failed. The last exception is: {0}", lastException));
    
    return result;
}

/// <summary>
/// Retry operation returning bool until success (bool == true) or timeout
/// </summary>
protected bool DoUntilSucceed(
    Func<bool> func,
    TimeSpan timeout,
    string format,
    params object[] args)
{
    // Same retry logic as above, but for bool-returning operations
}

/// <summary>
/// Retry operation (Action) until success or timeout
/// </summary>
protected void DoUntilSucceed(
    Action func,
    TimeSpan timeout,
    string format,
    params object[] args)
{
    // Same retry logic as above, but for void operations
}
```

### Helper Methods

```csharp
/// <summary>
/// Load test assembly from disk by assembly name
/// Caches loaded assemblies in TestcaseAssemblies dictionary
/// </summary>
protected Assembly GetTestcaseAssembly(string assemblyName)
{
    // Loads: BaseTestSite.TestAssemblyName + ".dll"
    // Caches in TestcaseAssemblies[assemblyName]
}
```

## TestConfigBase

**File**: `Adapter/TestConfigBase.cs`

Base configuration class inheriting from nothing (plain class). All test suite configs inherit from this. Properties are loaded from ptfconfig XML files via `GetProperty()`.

### Key Properties

#### Transport & Authentication

```csharp
public class TestConfigBase
{
    // Transport configuration
    public Smb2TransportType UnderlyingTransport { get; }    // Tcp, NetBios, Quic, Rdma
    public ushort TransportPort { get; }                     // Connection port
    
    // Authentication credentials
    public AccountCredential AccountCredential { get; }           // Admin user from ptfconfig
    public AccountCredential NonAdminAccountCredential { get; }   // Non-admin user
    public AccountCredential GuestAccountCredential { get; }      // Guest user
    public SecurityPackageType DefaultSecurityPackage { get; }    // Kerberos, Ntlm, Negotiate
    public SecureString SecurePassword { get; }                   // Secure password wrapper
    public bool UseServerGssToken { get; }                        // Use server GSS token
    
    // Server information
    public string SutComputerName { get; }                   // Server name (DNS/NetBIOS)
    public IPAddress SutIPAddress { get; }                   // Server IP (parsed/resolved)
    public Platform Platform { get; }                        // Windows version (enum)
    public string DomainName { get; }                        // Domain name (can be null)
    public string DCServerName { get; }                      // Domain controller name
    public int KDCPort { get; }                              // Kerberos KDC port
}
```

#### Protocol & Feature Support

```csharp
    // SMB dialect support
    public bool IsSMB1NegotiateEnabled { get; }                // Allow SMB 2.002 negotiation
    public DialectRevision MaxSmbVersionSupported { get; }     // Max server supports
    public DialectRevision MaxSmbVersionClientSupported { get; } // Max client requests
    public DialectRevision[] RequestDialects { get; }          // Dialects to request
    
    // Leasing & oplock support
    public bool IsLeasingSupported { get; }
    public bool IsDirectoryLeasingSupported { get; }
    
    // Durable handles & multi-channel
    public bool IsMultiChannelCapable { get; }
    public bool IsPersistentHandlesSupported { get; }
    public bool IsMultiCreditSupported { get; }
    
    // Compression support (parsed from config)
    public List<CompressionAlgorithm> SupportedCompressionAlgorithmList { get; }
    public bool IsChainedCompressionSupported { get; }
    
    // Encryption support (parsed from config)
    public bool IsEncryptionSupported { get; }
    public List<EncryptionAlgorithm> SutSupportedEncryptionAlgorithmList { get; }
    public List<SigningAlgorithm> SupportedSigningAlgorithmList { get; }
    public bool IsRDMATransformSupported { get; }
```

#### Encryption & Security

```csharp
    // Global encryption policy
    public bool IsGlobalEncryptDataEnabled { get; }           // Encrypt all connections
    public bool IsGlobalRejectUnencryptedAccessEnabled { get; } // Reject unencrypted
    public bool DisableEncryptionOverSecureTransport { get; }  // Don't encrypt over TLS
    public bool AllowNamedPipeAccessOverQUIC { get; }          // Named pipe access over QUIC
    public bool IsServerSigningRequired { get; }              // Require signing
    public bool DisableVerifySignature { get; }                // Don't verify signatures
    
    // Notification support
    public bool IsServerToClientNotificationsSupported { get; }
```

#### Network & Shares

```csharp
    // Client network configuration
    public IPAddress ClientNic1IPAddress { get; }            // Primary NIC IP
    public IPAddress ClientNic2IPAddress { get; }            // Secondary NIC IP
    
    // Test shares
    public string BasicFileShare { get; }                    // Regular file share
    public string EncryptedFileShare { get; }                // Encrypted share
    public string CompressedFileShare { get; }               // Compressed share
    public string CAShareName { get; }                       // Claims-based share
    public string CAShareServerName { get; }                 // CA server name
    public IPAddress CAShareServerIP { get; }                // CA server IP
```

#### Timeouts & Limits

```csharp
    // Timing configuration
    public TimeSpan Timeout { get; }                         // Default timeout (seconds)
    public TimeSpan LongerTimeout { get; }                   // Longer timeout
    public TimeSpan RetryInterval { get; }                   // Retry wait interval (seconds)
    public TimeSpan LeaseBreakNotificationWaitTimeout { get; } // Lease notification timeout
    public uint MaxResiliencyTimeoutInSecond { get; }        // Resilience timeout
    
    // Buffer sizing
    public int WriteBufferLengthInKb { get; }                // Write buffer size (KB)
```

#### Users & Accounts

```csharp
    public string UserName { get; }                          // Admin user name
    public string NonAdminUserName { get; }                  // Non-admin user name
    public string GuestUserName { get; }                     // Guest user name
    public string UserPassword { get; }                      // Password for all users
```

#### Support Checking

```csharp
    // Check if IOCTL code is supported
    public bool IsIoCtlCodeSupported(CtlCode_Values ioCtlCode)
    
    // Check if create context is supported
    public bool IsCreateContextSupported(CreateContextTypeValue createContext)
    
    // Platform helpers
    public bool IsWindowsPlatform { get; }                   // true if not NonWindows
    
    // Active TDI (Transport Driver Interface) list
    public List<string> ActiveTDIs { get; }                  // Active network drivers
```

### Property Retrieval Methods

```csharp
/// <summary>
/// Get property from Common group (default)
/// Properties loaded from: [TestSuite].ptfconfig with key: "Common.PropertyName"
/// </summary>
public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
{
    // Returns value from Site.Properties["Common." + propertyName]
    // If checkNullOrEmpty=true and value is null/empty:
    //   Inconclusive("The property {0} does not existed.") or
    //   Inconclusive("The value of {0} is empty.")
}

/// <summary>
/// Get property from specific group
/// Properties loaded from: [TestSuite].ptfconfig with key: "GroupName.PropertyName"
/// </summary>
public string GetProperty(string groupName, string propertyName, bool checkNullOrEmpty = true)
{
    // Returns value from Site.Properties[groupName + "." + propertyName]
}

/// <summary>
/// Parse property value to enum
/// Throws: Site.Assume.Fail if value is not valid enum member
/// </summary>
protected T ParsePropertyToEnum<T>(string propertyValue, string propertyName) where T : struct
{
    // Uses Enum.TryParse() to convert string to enum
    // Example: ParsePropertyToEnum<Smb2TransportType>("Tcp", "Transport")
}

/// <summary>
/// Parse semicolon-separated property values to list of enums
/// Example: "Aes128Ccm;Aes256Ccm;Aes128Gcm;Aes256Gcm" -> List<EncryptionAlgorithm>
/// </summary>
protected List<T> ParsePropertyToList<T>(string property, string groupName = "Common")
    where T : struct
{
    // Splits on ';', parses each value as enum T
    // Returns List<T> (empty if property is null/empty)
}
```

### Property Groups

Ptfconfig file structure groups properties by category:

```xml
<!-- Common.ptfconfig or [Protocol].ptfconfig -->
<Properties>
    <Group name="Common">
        <Property name="SutComputerName" value="SERVER01"/>
        <Property name="SutIPAddress" value="192.168.1.100"/>
        <Property name="UnderlyingTransport" value="Tcp"/>
        <Property name="Timeout" value="30"/>
        <!-- ... more Common properties ... -->
    </Group>
    
    <Group name="SMB2">
        <!-- SMB2-specific properties if using Smb2TestConfig -->
    </Group>
</Properties>
```

### Validation Methods

```csharp
/// <summary>
/// Assert/Inconclusive if specified dialect is not supported
/// </summary>
public void CheckDialect(DialectRevision dialect)
{
    // Checks if dialect is in MaxSmbVersionSupported or MaxSmbVersionClientSupported
    // Marks test Inconclusive if not supported
}

/// <summary>
/// Check platform requirements
/// </summary>
public void CheckPlatform(Platform platform)
{
    // Checks if current Platform matches requirement
}

/// <summary>
/// Check server capability support
/// </summary>
public void CheckCapabilities(NEGOTIATE_Response_Capabilities_Values capabilities)
{
    // Validates negotiate response capabilities
}
```

## Smb2FunctionalClient

**File**: `Adapter/Smb2FunctionalClient.cs`

High-level client wrapper around `Smb2Client` SDK providing functional methods for test cases. Manages credits, signing, encryption, and message generation.

### Initialization

```csharp
public class Smb2FunctionalClient
{
    protected Smb2Client client;              // Underlying SDK client
    protected ulong sessionId;                // Current session ID
    protected byte[] sessionKey;              // Session encryption/signing key
    protected byte[] serverGssToken;          // GSS token from server
    protected DialectRevision selectedDialect; // Negotiated dialect
    protected SortedSet<ulong> sequenceWindow; // Available message IDs
    protected uint maxBufferSize;             // Max read/write buffer size
    protected uint maxTransactSize;           // Max query/set/notify buffer size
    protected ushort creditGoal;              // Target credit level to maintain
    
    /// <summary>
    /// Create client with timeout, config, and test site
    /// </summary>
    public Smb2FunctionalClient(
        TimeSpan timeout,
        TestConfigBase testConfig,
        ITestSite baseTestSite,
        bool checkEncrypt = true)
    {
        // Initializes underlying Smb2Client with timeout
        // Sets up credit tracking and message ID generation
        // Subscribes to packet events (Sending, Received, PendingResponseReceived)
        // checkEncrypt: whether to validate that responses are actually encrypted
    }
    
    /// <summary>
    /// Get underlying Smb2Client for advanced operations
    /// </summary>
    public Smb2Client Smb2Client { get; }
}
```

### Session Properties

```csharp
// Current session information
public ulong SessionId { get; }              // Session ID from SessionSetup response
public byte[] SessionKey { get; }            // Session key for signing/encryption
public byte[] ServerGssToken { get; }        // GSS token from server response
public DialectRevision Dialect { get; }      // Selected SMB dialect

// Credit management
public ushort CreditGoal { get; set; }       // Target credits to maintain (default: 1)
public ushort Credits { get; }               // Current available credits (sequenceWindow.Count)
public SortedSet<ulong> SequenceWindow { get; } // Available message IDs

// Buffer sizes (from negotiate response)
public uint MaxBufferSize { get; }           // Max size for READ/WRITE length
public uint MaxTransactSize { get; }         // Max size for QUERY_INFO/QUERY_DIRECTORY/SET_INFO/CHANGE_NOTIFY
```

### Encryption & Signing

```csharp
// Crypto properties
public EncryptionAlgorithm SelectedCipherID { get; }      // Negotiated cipher
public SigningAlgorithm SelectedSigningAlgorithm { get; } // Negotiated signing algorithm
public PreauthIntegrityHashID SelectedPreauthIntegrityHashID { get; } // Preauth hash

/// <summary>
/// Generate session signing/encryption keys
/// Call after SessionSetup completes before encrypting
/// </summary>
public void GenerateCryptoKeys(
    bool enableSigning,
    bool enableEncryption,
    Smb2FunctionalClient previousClient = null,  // For channel binding (copy keys from previous session)
    bool isBinding = false)                       // true = new session, false = re-auth same session
{
    // If previousClient != null:
    //   Copies sessionKey/encrypt/decrypt keys from previous client
    // If isBinding == false:
    //   Uses previous session key for new session key generation
}

/// <summary>
/// Enable/disable signing and encryption session-wide
/// </summary>
public void EnableSessionSigningAndEncryption(
    bool enableSigning,
    bool enableEncryption)
{
    // Enables/disables for all future operations on this session
}

/// <summary>
/// Enable/disable encryption for specific tree connect
/// </summary>
public void SetTreeEncryption(uint treeId, bool enableEncryption)
{
    // Controls encryption for tree-specific operations
}
```

### Connection Management

```csharp
/// <summary>
/// Connect to server based on transport type
/// Automatically selects protocol based on Smb2TransportType
/// </summary>
public void ConnectToServer(
    Smb2TransportType transportType,   // Tcp, NetBios, Quic
    string serverName,                 // Server name (for NetBios/QUIC)
    IPAddress serverIp,                // Server IP (for TCP/QUIC)
    IPAddress clientIp = null)         // Client IP to bind to (optional)
{
    // TCP: ConnectToServerOverTCP(serverIp, clientIp)
    // NetBios: ConnectToServerOverNetbios(serverName)
    // QUIC: ConnectToServerOverQuic(serverName, serverIp, clientIp, TransportPort)
}

/// <summary>
/// Disconnect from server
/// Cleans up resources and stops notification thread
/// </summary>
public void Disconnect()
{
    // Removes event handlers and closes socket
}
```

### Core SMB2 Operations

#### Negotiate

```csharp
/// <summary>
/// Negotiate SMB dialect with optional SMB 1 compatibility
/// </summary>
public uint Negotiate(
    DialectRevision[] dialects,        // Dialects to request
    bool isSmb1NegotiateEnabled,       // Allow fallback to SMB 2.002
    SecurityMode_Values securityMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
    Capabilities_Values? capabilityValue = null,
    Guid? clientGuid = null,
    ResponseChecker<NEGOTIATE_Response> checker = null,
    bool ifHandleRejectUnencryptedAccessSeparately = false,
    bool? ifAddGLOBAL_CAP_ENCRYPTION = null,
    bool addDefaultEncryption = false,
    bool addNetNameContextID = false,
    bool addTransportCapabilities = false,
    bool ifAddGLOBAL_CAP_NOTIFICATIONS = false)
{
    // If isSmb1NegotiateEnabled: Try SMB 2.002 first via MultiProtocolNegotiate
    // Sets: selectedDialect, maxBufferSize, maxTransactSize, serverGssToken
    // Returns: status code (STATUS_SUCCESS if successful)
}

/// <summary>
/// Multi-protocol negotiate (SMB 1 and SMB 2 mixed)
/// </summary>
public uint MultiProtocolNegotiate(
    string[] dialects,  // "SMB 2.002", "SMB 2.???" etc.
    ResponseChecker<NEGOTIATE_Response> checker = null)
{
    // Sends SMB 1 negotiate containing SMB 2 dialect strings
    // Returns status code
}
```

#### Session Setup

```csharp
/// <summary>
/// Session setup with specified security package
/// </summary>
public uint SessionSetup(
    SecurityPackageType securityPackageType,      // Kerberos, Ntlm, Negotiate
    string serverName,                            // Server name for Kerberos SPN
    AccountCredential credential,                 // Username/password
    bool useServerGssToken,                       // Use GSS token from server
    uint sessionSetupFlags = 0,
    SessionFlags_Values sessionFlags = SessionFlags_Values.NONE,
    Smb2Encryption encryption = null,
    ResponseChecker<SESSION_SETUP_Response> checker = null)
{
    // Performs GSS exchange (Kerberos/NTLM)
    // Sets: sessionId, sessionKey, serverGssToken
    // Returns: status code
}
```

#### Tree Connect

```csharp
/// <summary>
/// Connect to share
/// </summary>
public uint TreeConnect(
    string uncSharePath,                    // \\server\share
    out uint treeId,
    string fileName = null,                 // Deprecated
    Smb2Encryption encryption = null,
    ResponseChecker<TREE_CONNECT_Response> checker = null)
{
    // Connects to share and allocates tree ID
    // Returns: status code
}

/// <summary>
/// Disconnect tree
/// </summary>
public uint TreeDisconnect(uint treeId)
{
    // Closes tree connection
}
```

#### Create & Close

```csharp
/// <summary>
/// Create or open file/directory
/// </summary>
public uint Create(
    uint treeId,
    string fileName,
    CreateOptions_Values createOptions,     // FILE_DIRECTORY_FILE, FILE_NON_DIRECTORY_FILE, etc.
    out FILEID fileId,
    out Smb2CreateContextResponse[] contexts,
    AccessMask accessMask = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE,
    FileAttributes_Values fileAttributes = FileAttributes_Values.None,
    ShareAccess_Values shareAccess = ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
    CreateDisposition_Values createDisposition = CreateDisposition_Values.FILE_OPEN_IF,
    Smb2CreateContextRequest[] requestContexts = null,
    ResponseChecker<CREATE_Response> checker = null)
{
    // Creates or opens file/directory
    // Sets: fileId, contexts from response
    // Returns: status code
}

/// <summary>
/// Close file/directory
/// </summary>
public uint Close(uint treeId, FILEID fileId)
{
    // Closes file handle
}

/// <summary>
/// Cancel pending request
/// </summary>
public uint Cancel()
{
    // Sends CANCEL message to abort pending request
}
```

#### Read & Write

```csharp
/// <summary>
/// Read file data
/// </summary>
public uint Read(
    uint treeId,
    FILEID fileId,
    ulong offset,
    uint length,
    out string data,
    ResponseChecker<READ_Response> checker = null)
{
    // Reads data from file starting at offset
    // Sets: data (as string)
    // Returns: status code
}

/// <summary>
/// Write file data
/// </summary>
public uint Write(
    uint treeId,
    FILEID fileId,
    string data,
    ulong offset = 0,
    ResponseChecker<WRITE_Response> checker = null)
{
    // Writes string data to file at offset
    // Returns: status code
}
```

#### Query & Set Info

```csharp
/// <summary>
/// Query file attributes/information
/// </summary>
public uint QueryFileAttributes(
    uint treeId,
    byte fileInformationClass,              // FileInformationClasses enum value
    QUERY_INFO_Request_Flags_Values flags,
    FILEID fileId,
    byte[] additionalInputBuffer,
    out byte[] outputBuffer,
    ResponseChecker<QUERY_INFO_Response> checker = null)
{
    // Queries file information (basic, standard, all info, names, etc.)
    // Sets: outputBuffer with response data
    // Returns: status code
}

/// <summary>
/// Set file attributes/information
/// </summary>
public uint SetFileAttributes(
    uint treeId,
    byte fileInformationClass,
    FILEID fileId,
    byte[] inputBuffer,
    ResponseChecker<SET_INFO_Response> checker = null)
{
    // Sets file information (timestamps, attributes, etc.)
    // Returns: status code
}
```

#### Lock & Unlock

```csharp
/// <summary>
/// Lock/unlock byte ranges
/// </summary>
public uint Lock(
    uint treeId,
    uint lockSequence,
    FILEID fileId,
    LOCK_ELEMENT[] locks,
    ResponseChecker<LOCK_Response> checker = null)
{
    // Locks/unlocks file ranges
    // locks[].Flags: LOCKFLAG_EXCLUSIVE_LOCK, LOCKFLAG_SHARED_LOCK, LOCKFLAG_UNLOCK
    // Returns: status code
}
```

#### Change Notify

```csharp
/// <summary>
/// Register for directory change notifications
/// Async - subscribe to Smb2Client.ChangeNotifyResponseReceived event
/// </summary>
public uint ChangeNotify(
    uint treeId,
    FILEID fileId,
    CompletionFilter_Values completionFilter,  // FILE_NOTIFY_CHANGE_FILE_NAME, etc.
    CHANGE_NOTIFY_Request_Flags_Values flags = CHANGE_NOTIFY_Request_Flags_Values.NONE,
    uint maxOutputBufferLength = 0,
    ResponseChecker<CHANGE_NOTIFY_Response> checker = null)
{
    // Registers for change notifications on directory
    // Response arrives asynchronously via event
    // Returns: status code (usually STATUS_PENDING)
}
```

#### Query Directory

```csharp
/// <summary>
/// Query directory contents
/// </summary>
public uint QueryDirectory(
    uint treeId,
    FileInformationClass_Values fileInformationClass,  // FileDirectoryInformation, etc.
    QUERY_DIRECTORY_Request_Flags_Values flags,        // RESTART_SCAN, RETURN_SINGLE_ENTRY
    uint fileIndex,
    FILEID fileId,
    out byte[] outputBuffer,
    ResponseChecker<QUERY_DIRECTORY_Response> checker = null)
{
    // Lists directory contents
    // Sets: outputBuffer with directory entries
    // Returns: status code
}
```

#### IoCtl (FSCTL)

```csharp
/// <summary>
/// Send FSCTL/IOCTL operation
/// </summary>
public uint IoCtl(
    uint treeId,
    CtlCode_Values ctlCode,                // FSCTL_VALIDATE_NEGOTIATE_INFO, etc.
    FILEID? fileId,
    byte[] inputBuffer,
    out byte[] outputBuffer,
    ResponseChecker<IOCTL_Response> checker = null)
{
    // Sends device control/file system control operation
    // Sets: outputBuffer with response data
    // Returns: status code
}
```

### Event Handlers & Callbacks

```csharp
/// <summary>
/// Callback to check response header and payload
/// Custom validation logic
/// </summary>
public delegate void ResponseChecker<T>(Packet_Header responseHeader, T response);

// Example usage:
client.QueryDirectory(
    treeId,
    FileInformationClass_Values.FileDirectoryInformation,
    flags,
    0,
    fileId,
    out outputBuffer,
    checker: (header, response) =>
    {
        if (header.Status != Smb2Status.STATUS_SUCCESS)
        {
            BaseTestSite.Assert.Fail("QueryDirectory failed: {0}", 
                Smb2Status.GetStatusCode(header.Status));
        }
    });

/// <summary>
/// Subscribe to packet sending event
/// </summary>
public event Action<Smb2Packet> RequestSent;

/// <summary>
/// Modify packets before sending (via Smb2Client events)
/// </summary>
public void BeforeSendingPacket(Action<Smb2Packet> modifier)
{
    // Example:
    // client.Smb2Client.PacketSending += (packet) => modifier(packet);
}
```

### Credit Management

```csharp
/// <summary>
/// Message ID generator (for advanced credit control)
/// </summary>
public delegate ulong MessageIdGenerator(SortedSet<ulong> sequenceWindow);

public MessageIdGenerator GenerateMessageId { get; set; }

public delegate ushort CreditChargeGenerator(uint payloadSize);

public CreditChargeGenerator GenerateCreditCharge { get; set; }

public delegate ushort CreditRequestGenerator(
    SortedSet<ulong> sequenceWindow,
    ushort creditGoal,
    ushort creditCharge);

public CreditRequestGenerator GenerateCreditRequest { get; set; }

/// <summary>
/// Default message ID generator (linear sequence)
/// </summary>
protected ulong GetDefaultMId(SortedSet<ulong> sequenceWindow)

/// <summary>
/// Default credit charge generator
/// </summary>
protected ushort GetDefaultCreditCharge(uint payloadSize)

/// <summary>
/// Default credit request generator
/// </summary>
protected ushort GetDefaultCreditRequest(
    SortedSet<ulong> sequenceWindow,
    ushort creditGoal,
    ushort creditCharge)
```

## Test Categories

**File**: `Adapter/TestCategories.cs`

Constants defining test categories for filtering and grouping. Use `[TestCategory(...)]` attributes on test methods.

### Environment & Execution

```csharp
public const string Bvt = "BVT";                    // Built-in Verification Test
public const string Model = "Model";                // Model-based tests (state machine)
public const string Failover = "Failover";          // Cluster failover tests
public const string NonSmb = "NonSmb";              // Non-SMB tests (non-protocol)
public const string DomainRequired = "DomainRequired"; // Requires domain environment
```

### SMB Dialects

```csharp
public const string Smb2002 = "Smb2002";            // SMB 2.002
public const string Smb21 = "Smb21";                // SMB 2.1
public const string Smb30 = "Smb30";                // SMB 3.0
public const string Smb302 = "Smb302";              // SMB 3.0.2
public const string Smb311 = "Smb311";              // SMB 3.1.1
```

### Protocol Families

```csharp
public const string Dfsc = "DFSC";                  // DFS referral protocol
public const string Swn = "SWN";                    // Server-side resource management
public const string Fsrvp = "FSRVP";                // File server VSS provider
public const string FsrvpNonClusterRequired = "FSRVPNonClusterRequired";
public const string RsvdVersion1 = "RSVDVersion1";  // Remote shared virtual disk
public const string RsvdVersion2 = "RSVDVersion2";
public const string Fsa = "FSA";                    // File system access model
public const string Sqos = "SQOS";                  // Storage quality of service
```

### Authentication & Authorization

```csharp
public const string Auth = "Auth";                  // General authentication/authorization
public const string Authentication = "Authentication";
public const string KerberosAuthentication = "KerberosAuthentication";
public const string Authorization = "Authorization";
public const string ShareAccessCheck = "ShareAccessCheck";
public const string FolderAccessCheck = "FolderAccessCheck";
public const string FileAccessCheck = "FileAccessCheck";
public const string CBAC = "CBAC";                  // Claims-based access control
```

### Core SMB2 Features

```csharp
public const string Negotiate = "Negotiate";        // Negotiate phase
public const string Session = "Session";            // Session setup & auth
public const string Tree = "Tree";                  // Tree connect/disconnect
public const string Credit = "Credit";              // Credit management
public const string Signing = "Signing";            // Message signing
public const string Encryption = "Encryption";      // Message encryption
public const string Compression = "Compression";    // Compression
public const string CreateClose = "CreateClose";    // Create/close operations
public const string ChangeNotify = "ChangeNotify";  // Change notifications
public const string QueryAndSetFileInfo = "QueryAndSetFileInfo";
public const string LockUnlock = "LockUnlock";      // File locking
public const string QueryDir = "QueryDir";          // Directory queries
public const string QueryInfo = "QueryInfo";        // File information queries
```

### Leasing & Handles

```csharp
public const string LeaseV1 = "LeaseV1";            // Lease oplock v1
public const string LeaseV2 = "LeaseV2";            // Lease oplock v2
public const string DirectoryLeasing = "DirectoryLeasing";
public const string DurableHandleV1BatchOplock = "DurableHandleV1BatchOplock";
public const string DurableHandleV1LeaseV1 = "DurableHandleV1LeaseV1";
public const string DurableHandleV2BatchOplock = "DurableHandleV2BatchOplock";
public const string DurableHandleV2LeaseV1 = "DurableHandleV2LeaseV1";
public const string DurableHandleV2LeaseV2 = "DurableHandleV2LeaseV2";
public const string PersistentHandle = "PersistentHandle";
public const string PersistentHandleNonClusterRequired = "PersistentHandleNonClusterRequired";
```

### Advanced Features

```csharp
public const string AppInstanceId = "AppInstanceId";
public const string AppInstanceVersion = "AppInstanceVersion";
public const string Replay = "Replay";              // Request replay
public const string MultipleChannel = "MultipleChannel";
public const string Compound = "Compound";          // Compound requests
public const string CombinedFeature = "CombinedFeature";
public const string CombinedFeatureNonClusterRequired = "CombinedFeatureNonClusterRequired";
```

### Usage Example

```csharp
[TestMethod]
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Smb311)]
[TestCategory(TestCategories.FileAccessCheck)]
[TestCategory(TestCategories.DomainRequired)]
[Description("Verify file read access with proper permissions")]
public void BVT_FileAccess_ReadWithPermission()
{
    // Test implementation
}

// Filter tests: Run all BVT tests for SMB 3.1.1
// powershell: RunTestCasesByFilter.ps1 "TestCategory=BVT&TestCategory=Smb311"
```

## Logging

Use `BaseTestSite.Log` to record test execution steps and diagnostics:

```csharp
// Log test step (visible in test output and report)
BaseTestSite.Log.Add(
    LogEntryKind.TestStep,
    "Step {0}: Create file {1}", stepNumber, fileName);

// Log debug information (for troubleshooting)
BaseTestSite.Log.Add(
    LogEntryKind.Debug,
    "FileId: {0}, TreeId: {1}", fileId.ToString(), treeId.ToString());

// Log comment (general information)
BaseTestSite.Log.Add(
    LogEntryKind.Comment,
    "Connection established to \\\\{0}\\{1}",
    TestConfig.SutComputerName, TestConfig.BasicFileShare);

// Log warning
BaseTestSite.Log.Add(
    LogEntryKind.Warning,
    "Server returned unexpected status: {0}",
    Smb2Status.GetStatusCode(status));

// Log checkpoint (for long-running tests)
BaseTestSite.Log.Add(
    LogEntryKind.Checkpoint,
    "Checkpoint: File operations completed");
```

## Assertions & Assumptions

### Standard Assertions (Test Failure)

```csharp
// Test fails if condition is false
BaseTestSite.Assert.AreEqual(expected, actual, message);
BaseTestSite.Assert.AreNotEqual(expected, actual, message);
BaseTestSite.Assert.IsTrue(condition, message);
BaseTestSite.Assert.IsFalse(condition, message);
BaseTestSite.Assert.IsNull(obj, message);
BaseTestSite.Assert.IsNotNull(obj, message);
BaseTestSite.Assert.Fail(message);

// Example
BaseTestSite.Assert.AreEqual(
    Smb2Status.STATUS_SUCCESS,
    status,
    "Create should succeed, actually returned {0}",
    Smb2Status.GetStatusCode(status));
```

### Assumptions (Test Skip)

```csharp
// Test is inconclusive (skipped) if condition is false
BaseTestSite.Assume.IsTrue(condition, message);
BaseTestSite.Assume.IsFalse(condition, message);
BaseTestSite.Assume.IsNotNull(obj, message);
BaseTestSite.Assume.Fail(message);  // Unconditional skip

// Example: Skip if domain not available
BaseTestSite.Assume.IsTrue(
    !string.IsNullOrEmpty(TestConfig.DomainName),
    "Domain environment required for Kerberos tests");
```

### Requirement Capture

```csharp
// Capture requirement if assertion passes
BaseTestSite.CaptureRequirementIfAreEqual(
    expected, actual,
    RequirementCategory.CATEGORY.Id,
    RequirementCategory.CATEGORY.Description);

BaseTestSite.CaptureRequirementIfIsTrue(
    condition,
    RequirementCategory.CATEGORY.Id,
    RequirementCategory.CATEGORY.Description);

// Example
BaseTestSite.CaptureRequirementIfAreEqual(
    Smb2Status.STATUS_SUCCESS,
    status,
    RequirementCategory.SMB2_SECTION_3_3_5_10.Id,
    "Server responds to CLOSE with STATUS_SUCCESS");
```

## Type Declarations (Where to Find/Add Definitions)

### Core SMB2 Types
**File**: `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`

This is the primary file for SMB2 protocol types. Search for `public enum` to find definitions.

| Type Category | Search Pattern | Purpose |
|--------------|----------------|---------|
| Share types & flags | `ShareType_Values`, `ShareFlags_Values` | Share enums and values |
| Create options/flags | `CreateOptions_Values`, `CreateDisposition_Values` | File creation enums |
| Access masks | `public enum AccessMask` | File/directory access masks |
| Lease/oplock types | `LeaseStateValues`, `OplockLevel_Values` | Lease and oplock values |
| Header flags | `Packet_Header_Flags_Values` | Packet header enums |
| IOCTL/FSCTL codes | `public enum CtlCode_Values` | Control code enum |
| Change notify filters | `CompletionFilter_Values` | Notify filter flags |
| Status codes | `public static class Smb2Status` | Status codes |

### File Information Classes
**File**: `ProtoSDK/MS-SMB2/FsccMessage.cs`

Search for `public enum FileInformationClasses` to find all file info class values.

### Transport Types
**File**: `ProtoSDK/MS-SMB2/CustomTypes.cs`

| Type | Purpose |
|------|---------|
| `Smb2TransportType` | Transport enum (Tcp, NetBios, Rdma, Quic) |

### Test Categories
**File**: `TestSuites/FileServer/src/Common/Adapter/TestCategories.cs`

Contains all test category constants used for test filtering.

**To add new SMB2 types**: Add to `ProtoSDK/MS-SMB2/Packets/SMB2Message.cs`  
**To add new test categories**: Add to `TestSuites/FileServer/src/Common/Adapter/TestCategories.cs`
