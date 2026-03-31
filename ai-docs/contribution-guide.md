# Contribution Guide for AI Contributors

## Overview

This guide is written for AI contributors (and new engineers) who want to make changes to the Windows Protocol Test Suites repository. It covers the complete workflow from setting up a branch to opening a pull request, with concrete guidance on where to put each type of change.

---

## Workflow

### Step 1: Start from main

Always start from the current state of `main`:

```bash
git checkout main
git pull origin main
```

### Step 2: Create a feature branch

Branch naming convention: `ai-work/<short-descriptive-name>` (kebab-case).

```bash
git checkout -b ai-work/<appropriate-name>
```

Examples:
- `ai-work/fix-smb2-negotiate-wildcard`
- `ai-work/add-rdp-egfx-codec-tests`
- `ai-work/update-fileserver-config-docs`
- `ai-work/add-kerberos-claims-test`

### Step 3: Make the change

Follow the conventions documented below. Work only within the scope of your task.

### Step 4: Build the affected suite

```powershell
cd TestSuites/<AffectedSuite>/src
./build.ps1
```

Verify the build succeeds before committing.

### Step 5: Commit

Write clear, focused commit messages:
- `Fix SMB2 negotiate response handling for wildcard dialect`
- `Add Encryption_RejectUnencryptedAccess test case`
- `Update FileServer ptfconfig for RDMA transport option`

```bash
git add <specific files>
git commit -m "Your message"
```

Do **not** use `git add -A` or `git add .` — stage specific files to avoid accidentally including generated files, binaries, or sensitive data.

### Step 6: Open a Pull Request

```bash
gh pr create --base main \
  --title "Fix SMB2 negotiate wildcard handling" \
  --body "$(cat <<'EOF'
## Summary
- Fixed dialect selection logic when server responds with Smb2Wildcard
- Added assertion for Smb2002 fallback path

## Test plan
- [x] Ran BVT_Negotiate_Compatible_Wildcard — passes
- [x] Ran full Negotiate test category — all pass

Generated with [Claude Code](https://claude.com/claude-code)
EOF
)"
```

---

## Where to Put Different Types of Changes

### New test case (existing protocol)

1. Navigate to the test suite for the relevant protocol. Example for a new SMB2 encryption test:
   `TestSuites/FileServer/src/SMB2/TestSuite/Encryption/`

2. Find the existing test class for that feature area, or create a new `.cs` file in the same directory.

3. Add the method with correct `[TestCategory]` attributes.

4. Update the Test Design Specification in `TestSuites/FileServer/docs/` (per CONTRIBUTING.md).

5. Build: `cd TestSuites/FileServer/src && ./build.ps1`

### New ProtoSDK feature (existing protocol)

Navigate to `ProtoSDK/MS-<PROTOCOL>/`. Add or modify:
- PDU structure in `DataTypes/` or `Packets/` (SMB2), or as a new `BasePDU` subclass (RDP)
- Client method in `Client/Smb2Client.cs` or equivalent
- Message constants in the consts file
- Decoder logic in the decoder class

ProtoSDK has no test infrastructure. Validate by running existing test cases that exercise the modified code path.

### New configuration property

1. Add the property to `<Suite>.ptfconfig` in the appropriate `<Group>`:
   ```xml
   <Group name="MyFeature">
     <Property name="MyNewProperty" value="DefaultValue" />
   </Group>
   ```

2. Parse it in the corresponding `TestConfig` class (e.g., `SMB2TestConfig.cs`):
   ```csharp
   public string MyNewProperty => Site.Properties["MyNewProperty"];
   ```

3. Update the User Guide per CONTRIBUTING.md.

### New adapter method

1. Add the method signature to the adapter interface (e.g., `ISutProtocolControlAdapter.cs`).
2. Implement it in the managed adapter class (e.g., `SutProtocolControlAdapter.cs`).
3. For PowerShell adapters: add a script file with the method name to `SutCommonControlAdapter/` directory.

### New PTM detector plugin capability

1. Modify the relevant plugin in `ProtocolTestManager/Plugins/<Suite>Plugin/`.
2. Add the property to the plugin's XML descriptor.
3. Implement the detection logic in the `IValueDetector` implementation.

### Documentation update

- Test Design Specifications: `TestSuites/<Suite>/docs/` (if exists) or look for `Docs/` capitalized
- User Guides: same `docs/` / `Docs/` directory
- AI-friendly docs: `ai-docs/` (this directory)

---

## Code Conventions

### C# Style

The codebase follows standard C# Microsoft conventions with some specifics:

**Namespace naming:**
- Test suites: `Microsoft.Protocols.TestSuites.<Family>.<Protocol>.TestSuite`
  Example: `Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite`
- ProtoSDK: `Microsoft.Protocols.TestTools.StackSdk.<Protocol>`
  Example: `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2`
- PTM: `Microsoft.Protocols.TestManager.PTMService.*`

**Test method naming convention:** `<Category>_<FeatureArea>_<Scenario>`
- BVT prefix for Basic Verification Tests: `BVT_Negotiate_Compatible_Wildcard`
- Feature prefix for non-BVT: `Encryption_RejectUnencryptedAccess`
- Negative tests often end in `_Negative` or describe the error condition

**[TestCategory] usage:**
Always include at minimum:
- Dialect/version category if protocol-version-specific (e.g., `TestCategories.Smb311`)
- Feature category (e.g., `TestCategories.Encryption`)
- `TestCategories.Bvt` for smoke tests

**Logging in test cases:**
```csharp
// Mark test steps for traceability
BaseTestSite.Log.Add(LogEntryKind.TestStep, "Step description");

// Use BaseTestSite.Assert not Assert directly
BaseTestSite.Assert.AreEqual(expected, actual, "Failure message with context: {0}", contextValue);
```

**Never** use `Console.WriteLine` or `Debug.WriteLine` in test cases. Always use `BaseTestSite.Log.Add`.

**Checker callbacks:**
Many ProtoSDK client methods accept a checker delegate to validate the response inline:
```csharp
client.Create(
    treeId, fileName, ...,
    checker: (header, response) => {
        BaseTestSite.Assert.AreEqual(
            (uint)Smb2Status.STATUS_SUCCESS, header.Status,
            "Create should succeed, got: {0}", Smb2Status.GetStatusCode(header.Status));
    });
```

**Resource cleanup:**
Always add test files and directories to the base class tracking lists so `TestCleanup` removes them:
```csharp
// In CommonTestBase subclasses:
string fileName = GetTestFileName(testConfig.SharePath);
// or
testFiles.Add(string.Format(@"{0}\{1}", share, fileName));
testDirectories.Add(string.Format(@"{0}\{1}", share, dirName));
```

### File Organization

For a new protocol area in an existing test suite, create a new subdirectory under `TestSuite/` mirroring the pattern of existing directories. For example, adding MS-SMB2 Compression tests:
```
TestSuites/FileServer/src/SMB2/TestSuite/Compression/
├── Compression.cs          Test class
└── CompressionTestConfig.cs (if new config needed)
```

For a new ProtoSDK message, add it alongside similar messages in the existing directory structure. Do not create new top-level directories in `ProtoSDK/` for protocol extensions that belong to an existing protocol.

### Error Handling in Tests

Tests should be written to distinguish between test failure (assertion failure, wrong server behavior) and test infrastructure failure (connection lost, timeout). Use:
- `BaseTestSite.Assert.Fail()` for protocol violations
- `BaseTestSite.Assume.IsTrue()` for preconditions that skip the test if not met
- Let exceptions propagate for infrastructure failures (they will be reported as test errors)

### ptfconfig XML

ptfconfig files use this XML schema: `http://schemas.microsoft.com/windows/ProtocolsTest/2007/07/TestConfig`

Properties must be in named groups:
```xml
<Group name="FeatureName">
  <Property name="PropertyName" value="DefaultValue" />
  <!-- Comment explaining the property -->
</Group>
```

Use comments to explain non-obvious properties. Boolean properties use `"true"` / `"false"` strings. Enum-valued properties should list valid values in a comment.

---

## Requirements from CONTRIBUTING.md

Before submitting a PR:

1. **Run all impacted test cases** against a compatible Windows SUT and verify they pass.
2. **Update Test Design Specification** if adding new test cases.
3. **Update User Guide** if adding new configuration properties.
4. **Include a clear PR description** explaining what changed and why.
5. **Sign the CLA** (one-time requirement): https://cla.microsoft.com/

---

## CLA Requirement

All contributors must sign the Microsoft Contributor License Agreement (CLA). This is a one-time requirement. If you have already signed the CLA for another Microsoft open-source project, you do not need to sign again.

Sign at: https://cla.microsoft.com/

The PR bot will check CLA status and block merging until it is signed.

---

## PR Review Process

After submitting a PR:
1. The CLA bot verifies your CLA status.
2. CI pipelines (Azure Pipelines) run the build and any automated validation.
3. The core team reviews the code and provides feedback.
4. Address review comments with additional commits (do not force-push).
5. Once approved and CI passes, the PR is merged by a maintainer.

---

## Checklist for a New Test Case PR

- [ ] Test method has `[TestMethod]`, `[TestCategory]`, and `[Description]` attributes
- [ ] Test method name follows `<Category>_<Feature>_<Scenario>` convention
- [ ] Uses `BaseTestSite.Assert.*` (not `Assert.*`) for all assertions
- [ ] Uses `BaseTestSite.Log.Add(LogEntryKind.TestStep, ...)` for test step logging
- [ ] Resources (test files/directories) registered for cleanup
- [ ] `TestInitialize` and `TestCleanup` properly extend base class versions
- [ ] Build succeeds (`./build.ps1`)
- [ ] Test passes against a compatible SUT
- [ ] Test Design Specification updated (if applicable)
- [ ] PR description explains the protocol behavior being tested and cites the spec section
