# Build and Setup Guide

## Prerequisites

### For All Test Suites

1. **.NET 8.0 SDK**
   Download: https://dotnet.microsoft.com/download/dotnet/8.0
   Required for building and running all test suites.

2. **Protocol Test Framework v2.6.1**
   Referenced as a NuGet package (`Microsoft.Protocols.TestTools` version 2.6.1).
   NuGet restore handles this automatically during build.
   Source: https://github.com/microsoft/ProtocolTestFramework/releases/tag/PTF2.6.1

### For Windows (Preferred Development Platform)

3. **Visual Studio 2022** (Community or higher)
   Required components (install via VS installer):
   - .NET SDK
   - C# and Visual Basic Roslyn compilers
   - MSVC v143 — C++ x64/x86 build tools *(ADFamily and MS-SMBD only)*
   - C++/CLI support for v143 build tools *(ADFamily and MS-SMBD only)*
   - C++ 2022 Redistributable Update *(ADFamily and MS-SMBD only)*
   - C++ core features *(ADFamily and MS-SMBD only)*
   - Windows 10 SDK (10.0.19041.0) *(ADFamily and MS-SMBD only)*

4. **Network Direct DDK** *(MS-SMBD only)*
   Download: https://www.microsoft.com/en-us/download/details.aspx?id=36043
   Extract `ndspi.h` and `ndstatus.h` into `ProtoSDK/RDMA/include/`.

### For RDP Test Suites (Windows or Linux)

5. **PowerShell Core 7**
   Download: https://github.com/PowerShell/PowerShell/releases

6. **Win32-OpenSSH** *(Windows only, for PowerShell Core remoting over SSH)*
   Download: https://github.com/PowerShell/Win32-OpenSSH/releases

### Automated Prerequisite Installation

```powershell
cd InstallPrerequisites
./InstallPrerequisites.ps1
```

This reads `PrerequisitesConfig.xml` and installs: Visual Studio 2022, .NET SDK 6.0, PowerShell Core 7, Win32-OpenSSH, and NetworkDirect DDK headers.

---

## Building a Test Suite

### Generic Pattern

```powershell
cd TestSuites/<SuiteName>/src
./build.ps1
```

Output is placed in `<repo-root>/drop/TestSuites/<SuiteName>/`.

### Per-Suite Build Commands

| Suite | Build command |
|---|---|
| FileServer | `cd TestSuites/FileServer/src && ./build.ps1` |
| RDP Client | `cd TestSuites/RDP/Client/src && ./build.ps1` |
| RDP Server | `cd TestSuites/RDP/Server/src && ./build.ps1` |
| Kerberos | `cd TestSuites/Kerberos/src && ./build.ps1` |
| MS-SMBD | `cd TestSuites/MS-SMBD/src && ./build.ps1` |
| MS-WSP | `cd TestSuites/MS-WSP/src && ./build.ps1` |
| MS-XCA | `cd TestSuites/MS-XCA/src && ./build.ps1` |
| ADFamily | `cd TestSuites/ADFamily/src && ./build.ps1` |
| BranchCache | `cd TestSuites/BranchCache/src && ./build.ps1` |
| PTMService | `cd ProtocolTestManager/PTMService && ./build.ps1` |

### Build Script Parameters

All `build.ps1` scripts accept:
- `-Configuration` — `Release` (default) or `Debug`
- `-OutDir` — output directory (default: `../../drop/TestSuites/<Suite>`)

```powershell
./build.ps1 -Configuration Debug -OutDir C:\MyOutput
```

### Linux Builds

Most suites have a `build.sh` companion script:
```bash
cd TestSuites/FileServer/src
./build.sh
```

Note: ADFamily and MS-SMBD have C++ components that require Windows to build. The C# RDMA adapter (`RdmaLinux`) can be built on Linux.

### What the Build Script Does

For FileServer (representative example):
1. Cleans the output directory
2. Copies PTM plugin XML files and docs to `drop/Plugin/`
3. Copies batch scripts to `drop/Batch/`
4. Copies setup scripts and common scripts to `drop/Scripts/`
5. Runs `dotnet publish FileServer.sln` → outputs to `drop/Bin/`
6. Copies the `.version` file

---

## Build from Visual Studio

Open the solution file directly:
```
TestSuites/FileServer/src/FileServer.sln
```
Build with `Ctrl+Shift+B`. The solution includes all projects for that suite (common adapter, per-protocol adapter, test project, plugin).

---

## NuGet Configuration

`NuGet.config` at the repo root configures the package source. The PTF NuGet package is fetched from nuget.org:
```xml
<packageSources>
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
</packageSources>
```

Some suites have their own `NuGet.config` with additional private feeds.

---

## Environment Configuration

Test suites require a running SUT (System Under Test). Environment configuration is split into two files:

### `<Suite>.ptfconfig` (Static Defaults)

Checked into the repo. Contains default property values and adapter declarations. Do **not** edit for per-environment settings.

Location: `TestSuites/<Suite>/src/<Protocol>/TestSuite/<Suite>.ptfconfig`

### `<Suite>.deployment.ptfconfig` (Environment Overrides)

Not checked into the repo (or checked in with placeholder values). Contains environment-specific overrides:

```xml
<TestSite xmlns="http://schemas.microsoft.com/windows/ProtocolsTest/2007/07/TestConfig">
  <Properties>
    <Group name="Common">
      <Property name="SutComputerName" value="WIN-SERVER-01" />
      <Property name="DomainName" value="contoso.com" />
      <Property name="AdminUser" value="Administrator" />
      <Property name="AdminPassword" value="Password123!" />
      <Property name="SutIPAddress" value="192.168.1.100" />
    </Group>
  </Properties>
</TestSite>
```

The `deployment.ptfconfig` is merged with the base `ptfconfig` at test run time. It overrides any properties with the same name.

### Populating deployment.ptfconfig

Use the PTM web UI auto-detection feature, or run the suite's setup scripts in `TestSuites/<Suite>/Setup/Scripts/` / `TestSuites/<Suite>/src/Deploy/`:

```powershell
cd drop/TestSuites/FileServer/Scripts
./Config-FileSharing.ps1
./Config-AuthorizedKeys.ps1
```

---

## Running Tests

### Via PTM Web UI (Recommended)

1. Build and install the test suite package via PTM.
2. Create a configuration and fill in or auto-detect SUT properties.
3. Select test cases (filter by category, e.g., `Bvt`).
4. Click Run. Results appear in real time.

### Via vstest Command Line

```powershell
# Run all BVT tests in FileServer SMB2
dotnet vstest drop/TestSuites/FileServer/Bin/MS-SMB2_ServerTestSuite.dll \
    --TestCaseFilter:"TestCategory=Bvt" \
    --Settings:drop/TestSuites/FileServer/Bin/MS-SMB2_ServerTestSuite.ptfconfig
```

### Via Batch Scripts

Each suite ships batch scripts in `Batch/` that run test groups:

```powershell
# Windows
drop/TestSuites/FileServer/Batch/RunTestCasesByBinariesAndFilter.ps1 `
    -Filter "TestCategory=Bvt" `
    -BinPath drop/TestSuites/FileServer/Bin
```

```bash
# Linux
./drop/TestSuites/FileServer/Batch/RunTestCasesByBinariesAndFilter.sh \
    --filter "TestCategory=Bvt" \
    --binpath drop/TestSuites/FileServer/Bin
```

### Via PtmCli

```bash
ptmcli run --suite FileServer --config myconfig.json --filter "TestCategory=Bvt"
```

---

## CI/CD

Azure Pipelines definitions are in `pipelines/`:

| File | Purpose |
|---|---|
| `DotNetCore-Build.yml` | Main build pipeline — builds all test suites and RDP SUT Control Agent |
| `DotNetCore-PullRequestValidation.yml` | PR validation — builds and runs quick validation |
| `DotNetCore-PTMCli-Build.yml` | Builds PtmCli |
| `DotNetCore-MSBuild-Build.yml` | MSBuild-based build (legacy) |
| `1es/FileServer-OneClick-Release.yml` | ESRP-signs and packages Workgroup, Domain, and Cluster OneClick release assets |

All pipelines run on `windows-2022` pool and reference a template repo at `AzurePipelines-main` branch for the actual build steps.

The `test.testSuiteName` variable controls which suite-specific steps are included (e.g., building the RDP SUT Control Agent is only done for RDP suites).

---

## Common Build Issues

### "ndspi.h not found"
Install the NetworkDirect DDK and copy `ndspi.h` and `ndstatus.h` to `ProtoSDK/RDMA/include/`. See Prerequisites section above.

### "MSTest runner not found"
Ensure `.NET SDK 8.0` is installed. Run `dotnet --version` to verify.

### NuGet restore fails
Check internet connectivity or configure a private NuGet feed in `NuGet.config`. The PTF package `Microsoft.Protocols.TestTools` must be resolvable from nuget.org.

### C++ build fails for ADFamily or SMBD
Ensure Visual Studio 2022 with C++ workload components is installed (see Prerequisites). MSVC v143 and C++/CLI support are required.

### Tests fail with "Connection refused"
The SUT is not reachable, or the `deployment.ptfconfig` has incorrect IP/hostname. Verify network connectivity and ptfconfig values.
