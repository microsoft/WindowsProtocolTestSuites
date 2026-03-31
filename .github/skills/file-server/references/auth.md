# Authentication Tests Reference

This document provides guidance for writing authentication-related test cases in the FileServer Auth test suite.

## Required Using Statements

```csharp
using System;
using System.Text.RegularExpressions;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## Overview

The FileServer Auth test suite validates:
- **Authentication**: Kerberos and NTLM protocols
- **Authorization**: Share, folder, and file-level access control
- **Claims-Based Access Control (CBAC)**: Claims-based access evaluation
- **Security Descriptors**: ACL enforcement

## Directory Structure

```
Auth/
├── TestSuite/
│   ├── AuthTestConfig.cs                          # Auth-specific configuration
│   ├── Authentication/
│   │   ├── AuthenticationTestBase.cs              # Base class for auth tests
│   │   └── KerberosAuthentication/
│   │       ├── KerberosAuthentication.cs          # Kerberos test cases
│   │       ├── KerberosFunctionalClient.cs        # Kerberos-specific client
│   │       ├── SMB2FunctionalClientForKerbAuth.cs # SMB2 client for Kerberos
│   │       └── KeyManager.cs                      # Kerberos key management
│   └── Authorization/
│       ├── AuthorizationTestBase.cs               # Base class for authorization tests
│       ├── FilePermissionTest.cs                  # File-level access tests
│       ├── FolderPermissionTest.cs                # Folder-level access tests
│       ├── SharePermissionTest.cs                 # Share-level access tests
│       └── CBACTest.cs                            # Claims-based access tests
└── Configuration files
    ├── Auth_ServerTestSuite.ptfconfig             # Runtime configuration
    └── Auth_ServerTestSuite.deployment.ptfconfig  # Deployment configuration
```

## Test Base Classes

### AuthenticationTestBase
**File**: `Authentication/AuthenticationTestBase.cs`

Base class for Kerberos authentication tests. Inherits from `CommonTestBase`.

```csharp
public class AuthenticationTestBase : CommonTestBase
{
    public AuthTestConfig TestConfig { get; }  // Gets Auth-specific config
    
    // Calls base TestInitialize and initializes AuthTestConfig
    protected override void TestInitialize() { }
}
```

### AuthorizationTestBase
**File**: `Authorization/AuthorizationTestBase.cs`

Base class for authorization (share, folder, file permissions) and CBAC tests.

```csharp
public class AuthorizationTestBase : CommonTestBase
{
    protected const string azUser01Name = "AzUser01";      // Test user
    protected const string azGroup01Name = "AzGroup01";    // Test group
    protected SutCommonControlAdapterAccessor sutCommonControlAdapterAccessor;
    
    public AuthTestConfig TestConfig { get; }
    
    // Provides methods for:
    // - AccessShare(AccountCredential, string)  // Test share access
    // - ShareExists(AccountCredential, string)  // Check if share exists
    // - QuerySecurityDescriptor(...)            // Get security descriptor
    // - CreateNewFile(...)                      // Create test files
    // - DeleteExistingFile(...)                 // Clean up files
}
```

## AuthTestConfig

**File**: `AuthTestConfig.cs`

Inherits from `TestConfigBase` and provides Auth-specific settings:

```csharp
public class AuthTestConfig : TestConfigBase
{
    // Kerberos configuration (Auth.Authentication group)
    public string KeytabFile { get; }              // Kerberos keytab file path
    public string ServicePassword { get; }         // SMB2 service principal password
    public string ServiceSaltString { get; }       // Password salt
    public SpecialUserCredential[] SpecialUsers { get; }  // Users with special characters
    
    // Authorization configuration (Auth.Authorization group)
    public string CBACShare { get; }               // Share for CBAC tests
    public string FilePermissionTestShare { get; } // Share for file permission tests
    public string FolderPermissionTestShare { get; } // Share for folder permission tests
    public string SharePermissionTestShare { get; } // Share for share permission tests
}
```

## Test Categories

```csharp
[TestCategory(TestCategories.Bvt)]
[TestCategory(TestCategories.Auth)]
[TestCategory(TestCategories.DomainRequired)]

// Authentication specific
[TestCategory(TestCategories.Authentication)]
[TestCategory(TestCategories.KerberosAuthentication)]

// Authorization specific
[TestCategory(TestCategories.Authorization)]
[TestCategory(TestCategories.FileAccessCheck)]
[TestCategory(TestCategories.FolderAccessCheck)]
[TestCategory(TestCategories.ShareAccessCheck)]

// CBAC specific
[TestCategory(TestCategories.CBAC)]
[TestCategory(TestCategories.NonSmb)]  // Non-SMB specific tests
```

## Common Test Patterns

### Kerberos Authentication

**File**: `Authentication/KerberosAuthentication/KerberosAuthentication.cs`

Tests Kerberos authentication with various scenarios:

```csharp
[TestClass]
public class KerberosAuthentication : AuthenticationTestBase
{
    private Smb2FunctionalClientForKerbAuth smb2Client;
    private KerberosFunctionalClient kerberosClient;
    private KeyManager keyManager;
    
    [ClassInitialize]
    public static void ClassInitialize(TestContext testContext)
    {
        Initialize(testContext);
        // Setup Kerberos environment
    }
    
    protected override void TestInitialize()
    {
        base.TestInitialize();
        
        // Verify domain environment
        if (!Regex.IsMatch(TestConfig.DomainName, 
            @"^(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+[a-z0-9][a-z0-9-]{0,61}[a-z0-9]$", 
            RegexOptions.IgnoreCase))
        {
            BaseTestSite.Assert.Inconclusive(
                "Kerberos Authentication test cases are not applicable in non-domain environment");
        }
        
        // Get KDC IP and service principal name
        servicePrincipalName = Smb2Utility.GetCifsServicePrincipalName(
            TestConfig.SutComputerName);
    }
    
    [TestMethod]
    [TestCategory(TestCategories.Bvt)]
    [TestCategory(TestCategories.Auth)]
    [TestCategory(TestCategories.KerberosAuthentication)]
    [TestCategory(TestCategories.DomainRequired)]
    public void BVT_Kerberos_ValidAuthentication()
    {
        // Test valid Kerberos authentication
        smb2Client = new Smb2FunctionalClientForKerbAuth(...);
        smb2Client.ConnectToServer(...);
        
        // Use Kerberos for authentication
        // Validate ticket and token exchange
    }
}
```

### File Permission Tests

**File**: `Authorization/FilePermissionTest.cs`

Tests file-level access control:

```csharp
[TestClass]
public class FilePermissionTest : AuthorizationTestBase
{
    private Smb2FunctionalClient client;
    private string FilePermissionTestShareUncPath;
    private string tempFileName;
    private _SECURITY_DESCRIPTOR baseSD;
    
    protected override void TestInitialize()
    {
        base.TestInitialize();
        
        // Verify domain environment
        if (!Regex.IsMatch(TestConfig.DomainName, @"^(?:...", RegexOptions.IgnoreCase))
        {
            BaseTestSite.Assert.Inconclusive(
                "Authentication test cases are not applicable in non-domain environment");
        }
        
        // Setup test file and security descriptor
        FilePermissionTestShareUncPath = Smb2Utility.GetUncPath(
            TestConfig.SutComputerName, TestConfig.FilePermissionTestShare);
        
        tempFileName = GetTestFileName(FilePermissionTestShareUncPath);
        CreateNewFile(FilePermissionTestShareUncPath, tempFileName);
        baseSD = QuerySecurityDescriptor(
            FilePermissionTestShareUncPath, tempFileName,
            AdditionalInformation_Values.DACL_SECURITY_INFORMATION);
        
        client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
        client.ConnectToServer(TestConfig.UnderlyingTransport,
            TestConfig.SutComputerName, TestConfig.SutIPAddress);
    }
    
    protected override void TestCleanup()
    {
        client?.Disconnect();
        if (FilePermissionTestShareUncPath != null)
        {
            DeleteExistingFile(FilePermissionTestShareUncPath, tempFileName);
        }
        base.TestCleanup();
    }
    
    [TestMethod]
    [TestCategory(TestCategories.Bvt)]
    [TestCategory(TestCategories.Auth)]
    [TestCategory(TestCategories.FileAccessCheck)]
    [TestCategory(TestCategories.DomainRequired)]
    [Description("Test whether a user can read a file when ACCESS_ALLOWED_ACE exists.")]
    public void BVT_FilePermission_AccessAllow_UserSid()
    {
        // Get user SID from domain
        _SID sid = sutCommonControlAdapterAccessor.GetUserSid(azUser01Name);
        
        // Create ACCESS_ALLOWED_ACE for file
        object ace = DtypUtility.CreateAccessAllowedAce(
            sid, DtypUtility.ACCESS_MASK_GENERIC_READ, ACE_FLAGS.None);
        
        // Set security descriptor
        _SECURITY_DESCRIPTOR newSD = // ... set with new ACE ...
        
        // Verify access
        BaseTestSite.Assert.IsTrue(
            AccessFile(TestConfig.AccountCredential, FilePermissionTestShareUncPath, tempFileName),
            "User should be able to read file with ACCESS_ALLOWED_ACE");
    }
}
```

### Share Permission Tests

**File**: `Authorization/SharePermissionTest.cs`

Tests share-level access control.

### Folder Permission Tests

**File**: `Authorization/FolderPermissionTest.cs`

Tests folder-level access control.

### CBAC Tests

**File**: `Authorization/CBACTest.cs`

Tests claims-based access control for devices and users with claims.

## Type Declarations (Where to Find/Add Definitions)

### Security Descriptor Types
**File**: `ProtoSDK/MS-DTYP/DtypUtility.cs`

Search for these utilities:

| Type | Purpose |
|------|---------|
| `_SECURITY_DESCRIPTOR` | Security descriptor structure |
| `_SID` | Security identifier |
| `_ACE` | Access control entry |
| `_ACL` | Access control list |

### ACE and ACL Utilities
**File**: `ProtoSDK/MS-DTYP/DtypUtility.cs`

| Method | Purpose |
|--------|---------|
| `CreateAccessAllowedAce()` | Create allow ACE |
| `CreateAccessDeniedAce()` | Create deny ACE |
| `GetUserSid()` | Get SID from account |
| `ToSddlString()` | Convert to SDDL format |

### Access Mask Constants
**File**: `ProtoSDK/MS-DTYP/DtypUtility.cs`

| Constant | Purpose |
|----------|---------|
| `ACCESS_MASK_GENERIC_READ` | Read permission |
| `ACCESS_MASK_GENERIC_WRITE` | Write permission |
| `ACCESS_MASK_GENERIC_EXECUTE` | Execute permission |

**To add new Auth test files**: Create in `TestSuites/FileServer/src/Auth/TestSuite/Authentication/` or `Authorization/` folder

## Best Practices

1. **Always verify domain requirement** at test start with regex validation
2. **Check share existence** before running tests
3. **Use AuthorizationTestBase helpers** for permission operations
4. **Create test files with proper cleanup** in TestCleanup
5. **Use DtypUtility** for security descriptor operations
6. **Test both positive and negative cases** (allow and deny)
7. **Verify expected error codes** (STATUS_ACCESS_DENIED, STATUS_SUCCESS)
8. **Handle special characters in usernames** via SpecialUsers config