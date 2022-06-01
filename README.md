# Windows Protocol Test Suites

**Windows Protocol Test Suites** provide interoperability testing against the implementation of Windows open specifications including File Services, Identity Management, Remote Desktop and etc.

Originally developed for in-house testing of the Microsoft Open Specifications, Microsoft Protocol Test Suites have been used extensively during Plugfests and interoperability labs to test against partner implementations.
A Test Suite evaluates whether a protocol or protocol family implementation meets certain interoperability requirements.
Test Suites do not cover every protocol requirement and in no way certify an implementation, even if all tests pass. 
However, each test suite provides users with a useful indication of interoperability.

* **File Server Family Test Suite**. It is designed to test implementations of file server protocol family including [MS-SMB2], [MS-DFSC], [MS-SWN], [MS-FSRVP], [MS-FSA], [MS-FSCC], [MS-RSVD] and [MS-SQOS].
* **RDP Client Family Test Suite**. It provides interoperability testing for client implementation of RDP family protocols including [MS-RDPBCGR], [MS-RDPEDISP], [MS-RDPEDYC], [MS-RDPEGFX], [MS-RDPEGT], [MS-RDPEI], [MS-RDPEMT], [MS-RDPEUDP], [MS-RDPEUSB], [MS-RDPEVOR] and [MS-RDPRFX].
* **RDP Server Family Test Suite**. It provides interoperability testing for server implementation of RDP family protocols including [MS-RDPBCGR], [MS-RDPEDYC], [MS-RDPEMT] and [MS-RDPELE].  
* **Kerberos Server Test Suite**. It is designed to test server implementations of Kerberos protocols including [MS-KILE], [MS-KKDCP] and [MS-PAC].
* **SMBD Server Test Suite**. It is designed to test the implementations of SMB2&3 direct (RDMA) protocol, as specified in [MS-SMBD] and [MS-SMB2].
* **Branch Cache Test Suite**. It is designed to test the implementations of [MS-PCCRTP], [MS-PCCRR], [MS-PCHC] and [MS-PCCRC] protocol.
* **AZOD Test Suite**. It is designed to test the implementations of [MS-AZOD] protocol.
* **ADFamily Test Suite**. It is designed to test the implementations of the Active Directory protocols including [MS-ADA1], [MS-ADA2], [MS-ADA3], [MS-ADLS], [MS-ADSC], [MS-ADTS], [MS-APDS], [MS-DRSR], [MS-FRS2], [MS-LSAD], [MS-LSAT], [MS-SAMR] and [MS-NRPC]. 
* **ADFSPIP Client Test Suite**. It is designed to test the implementations of ADFS Proxy and Web Application Proxy integration, as described in [MS-ADFSPIP].
* **ADOD Test Suite**. It is designed to test the implementations of [MS-ADOD] protocol.

## Components
Windows Protocol Test Suites contain below components:

* **CommonScripts**. Common scripts used by each test suite. Normally they're used to deploy the environment.
* **ProtoSDK**. The protocol library used by each test suite. It provides the data structures of the protocol messages, the methods to encode and decode the messages, the methods to send and receive messages and etc.
* **TestSuites**. All Test Suites code and documents are saved here and categorized by folder representing each test suite.
* **ProtocolTestManager**. A tool to help you configure and run test suites.

## Prerequisites
**Windows Protocol Test Suites** are based on [.NET](https://dotnet.microsoft.com/) so they can be developed and run across different platforms.
You should install the software listed below based on your testing purpose, including their own dependencies.

1. .NET and related components

   a. For Windows, Linux and macOS, install [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0/) to build or run test suites.

   b. For those who work on Windows and prefer IDE, install [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or higher ([Visual Studio 2022 Community](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=17) recommended), together with these individual components from the installer:
 
      |Section|Individual Component in Visual Studio 2022|Run Windows Protocol Test Suites|Build Windows Protocol Test Suites from source code|
      |---|---|---|---|
      |.NET|.NET SDK|Required|Required|
      |Compilers, build tools, and runtime|C# and Visual Basic Roslyn compilers||Required|
      |Compilers, build tools, and runtime|MSVC v143 - VS 2022 C++ x64/x86 build tools (Latest)||Required<sup>[1](#footnote1)</sup>|
      |Compilers, build tools, and runtime|C++/CLI support for v143 build tools (Latest)||Required<sup>[1](#footnote1)</sup>|
      |Compilers, build tools, and runtime|C++ 2022 Redistributable Update|Required<sup>[1](#footnote1)</sup>|Required<sup>[1](#footnote1)</sup>|
      |Development Activities|C++ core features||Required<sup>[1](#footnote1)</sup>|
      |SDKs, libraries, and frameworks|Windows 10 SDK (10.0.19041.0)||Required<sup>[1](#footnote1)</sup>|      

      Note:

<a name="footnote1">[1]</a>: This individual component is required by ADFamily and MS-SMBD which have C++ code.

1. [Protocol Test Framework v2.3 (build 2.3.0)](https://github.com/Microsoft/ProtocolTestFramework/releases/tag/2.3.0)

   Protocol Test Framework is referenced by projects of ProtoSDK and TestSuites as [NuGet packages](https://www.nuget.org/packages/Microsoft.Protocols.TestTools/2.3.0).

1. [Network Direct DDK](https://www.microsoft.com/en-us/download/details.aspx?id=36043)

   From `NetworkDirect_DDK.zip` extract `ndspi.h` and `ndstatus.h` into project path `ProtoSDK\RDMA\include`. This is to build SMBD test suite.

1. [PowerShell Core](https://github.com/PowerShell/PowerShell/releases)

   This is required only when user want to use [PowerShell Core Remoting over SSH](https://github.com/microsoft/WindowsProtocolTestSuites/wiki/Run-Test-Suites-With-Enabling-PowerShell-Core-Remoting-Over-SSH).

1. [Win32-OpenSSH](https://github.com/PowerShell/Win32-OpenSSH/releases)

   This is required only when user want to use [PowerShell Core Remoting over SSH](https://github.com/microsoft/WindowsProtocolTestSuites/wiki/Run-Test-Suites-With-Enabling-PowerShell-Core-Remoting-Over-SSH) for Windows platform.

1. [WMF 5.1](https://docs.microsoft.com/en-us/powershell/scripting/windows-powershell/wmf/setup/install-configure?view=powershell-7.1)

   This is required only when user want to use PowerShell implementation on Windows Server 2012R2 for ISutCommonControlAdapter in [CommonTestSuite.ptfconfig](./TestSuites/FileServer/src/Common/TestSuite/CommonTestSuite.ptfconfig).

   a. If you choose PowerShell implementation for ISutCommonControlAdapter in a domain environment where the DC runs Windows Server 2012R2, in order to get SID from the DC, you need to install WMF 5.1 on the DC, for other Windows Server versions newer than Windows Server 2012R2, you do not need to install WMF 5.1 on the DC.

   b. If you choose PowerShell implementation for ISutCommonControlAdapter on Windows platforms (including Windows Server 2012R2 and newer versions) in workgroup environment, you do not need to install WMF 5.1 on the SUT.

   c. If you choose managed implementation for ISutCommonControlAdapter on Windows platforms (including Windows Server 2012R2 and newer versions), it will use LDAP queries to get SID and only supports domain environment.


If your work on Windows, you can use the script in `InstallPrerequisites` folder to automatically download and install these software.

Tips when using the script in `InstallPrerequisites` folder:

* To run the script, open **Windows PowerShell**, and execute the commands below in the **PowerShell Window**:

```
cd WindowsProtocolTestSuites\InstallPrerequisites
.\InstallPrerequisites.ps1
```

* If you meet errors about **Execution Policy**, make sure you run **Windows PowerShell** as **Administrator**, and type the following and enter:

```
Set-ExecutionPolicy RemoteSigned
```

You could run the command below to verify if the **Execution Policy** is correctly set:

```
Get-ExecutionPolicy
```

Then rerun the script.

## Build

After you [clone a copy](https://help.github.com/articles/cloning-a-repository/) of this repo, you can run `build.ps1` in PowerShell or `build.sh` in shell for each test suite separately after you have installed all the softwares required for build listed in [Prerequisites](#prerequisites).

For example, if you want to build FileServer test suite:

```
cd WindowsProtocolTestSuites\TestSuites\FileServer\src
build.ps1
```

After the build succeeds, the common folder structure should be generated in the folder `WindowsProtocolTestSuite\drop\TestSuites\[TestSuiteName]\`.

* `Bin`: all the built binaries including ProtoSDK, adapters and test suites.
* `Batch`: batch files (.ps1, .sh) which can be used to launch tests.
* `Scripts`: scripts which can be used to configure test environment.
* `Utils`: some utilities which can be used in tests.

## Run

Before running a test suite, you need do either of below:

* Download the test suite archive you want to run from [Releases](https://github.com/microsoft/WindowsProtocolTestSuites/releases), and extract it to some path you have access.
* Build the test suite according to [Build a test suite](#build).

On macOS, the FileServer test suite uses [AesCcm and AesGcm classes](https://docs.microsoft.com/en-us/dotnet/standard/security/cross-platform-cryptography#aes-ccm-and-aes-gcm-on-macos) which require OpenSSL, so if there is not OpenSSL 1.1 on your macOS please install OpenSSL 1.1 and set the environment variable as following before you run the FileServer test suite on macOS,
```
brew install openssl@1.1
export DYLD_LIBRARY_PATH="/usr/local/opt/openssl@1.1/lib:$DYLD_LIBRARY_PATH"
```
**Note:**
1. If `brew` is not installed on your macOS, you can install it according to [brew](https://brew.sh/).
2. If you get the error "algorithm 'aesgcm' is not supported on this platform", that means dotnet cannot load AesGcm class from libcrypto.1.1.dylib on your macOS, then you need to check whether its location is in your `DYLD_LIBRARY_PATH` environment variable or not. Once you install OpenSSL 1.1 on your macOS the crypto library location is `/usr/local/Cellar/openssl@1.1/1.1.1m/lib/libcrypto.1.1.dylib` and `/usr/local/opt/openssl` links to the `/usr/local/Cellar/openssl@1.1/1.1.1m` directory by default.

### Run test suite by batch

In the `Batch` folder under root path of the test suite, there are several scripts you can use to launch tests.

1. Run all test cases
   
   Execute `RunAllTestCases.ps1` in PowerShell, or `RunAllTestCases.sh` in shell directly.

1. Run test cases by filters

   Execute `RunTestCasesByFilter.ps1 -Filter [your filter expression]` in PowerShell, or `RunTestCasesByFilter.sh [your filter expression]` in shell directly.

   For example, you can run below command if you want to run test cases with test category `BVT` and `SMB311`:
   ```
   RunTestCasesByFilter.sh "TestCategory=BVT&TestCategory=SMB311"
   ``` 

   For more information about how to construct the filter expression, you can refer to [Filter option details](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test#filter-option-details).

1. Dry run

   If you want to list the test cases before running them actually, you could add `-DryRun` switch to `.ps1` scripts or pass a non-empty string as the last argument to `.sh` scripts.

   For example, you can run below command if you want to list test cases with test category `BVT` and `SMB311`:
   ```
   RunTestCasesByFilter.sh "TestCategory=BVT&TestCategory=SMB311" "list"
   ```

### Configure and run test suite by Protocol Test Manager

Take File Server test suite as an example, you can learn how to configure and run test suite by Protocol Test Manager referring to this [tutorial](./Doc/File%20Server%20SMB2%20Test%20Suite%20Lab%20Tutorial_v2.pdf).

## Documents

You could set up the test environment and configure the test suite according to its **User Guide**.
Each test suite has its own **User Guide** in the `WindowsProtocolTestSuites\TestSuites\[TestSuiteName]\docs` folder.
There are two more kinds of documents in the same folder:

* **Technical Documents**. The Open Specifications documentation for protocols, published by Microsoft. It's the basis of developing Test Suites.
* **Test Design Specs**.  It provides information about the test scope and test suite design.

## Contribute

You can find contributing guide [here](./CONTRIBUTING.md).

## License

Windows Protocol Test Suites are under the [MIT license](./LICENSE.txt).

## Contact
The following resources are for Windows protocol test suite news, discussion, and support:
* View news announcements in [Open Specification Windows Protocols Forum](https://social.msdn.microsoft.com/Forums/en-US/home?forum=os_windowsprotocols).
* Discuss test suites issues [here](https://github.com/Microsoft/WindowsProtocolTestSuites/issues) on the github.
* For [Open Specifications Protocols](https://msdn.microsoft.com/en-us/library/gg685446.aspx) support, contact dochelp@microsoft.com.

## Microsoft Open Source Code of Conduct
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
