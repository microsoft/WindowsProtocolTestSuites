# Windows Protocol Test Suites

Originally developed for in-house testing of the Microsoft Open Specifications, Microsoft Protocol Test Suites have been used extensively during plugfests and interoperability labs to test partner implementations.
A Test Suite evaluates whether a protocol implementation meets certain interoperability requirements.
Test Suites do not cover every protocol requirement and in no way certify an implementation, even if all tests pass. 
However, each test suite provides users with a useful indication of interoperability.

Windows Protocol Test Suites provide interoperability testing against an implementation of the Windows open specifications including SMB2&3, Active Directory, RDP, Kerberos and etc.

* **SMB Protocol Test Suite**. It covers requirements documented in [MS-SMB], [MS-FSCC] as well [MS-CIFS] dependencies referred to in [MS-SMB].

## Components
Windows Protocol Test Suites contain three components:

* **CommonScripts**. Common scripts used by every test suite.
* **ProtoSDK**. Protocol SDK is the protocol library used by every test suite. It provides the data structures of the protocol messages, the methods to encode and decode the messages, the methods to send and recieve messages and etc.
* **TestSuites**. All Test Suites code are saved here, categorized by folder.


## Prerequisites
The Test Suites are based on Windows platform.
You should install the following list of software in order to build Test Suites from source code.

* .Net framework 4.0 or higher
* Wix toolset v3.7 or higher
* Visual Studio or Visual Studio test agent, version 2012 or higher
* [Protocol Test Framework](https://github.com/microsoft/protocoltestframework)
* [Spec Explorer](https://visualstudiogallery.msdn.microsoft.com/271d0904-f178-4ce9-956b-d9bfa4902745/). It is only required for the test suites that contain Model Based Test cases.

## Build

After you clone a copy of this repo, change to the WindowsProtocolTestSuites directory:

```
cd WindowsProtocolTestSuites
```

Run buildall.cmd

```
buildall.cmd
```

After the build succeeds, the MSI (installer package) files of every test suite should be generated in the folder *WindowsProtocolTestSuite\drop\TestSuites\\[TestSuiteName]\Installer\.*
Take SMB test suite as an example, **MS-SMB-TestSuite-ServerEP.msi** should be generated in the folder *WindowsProtocolTestSuite\drop\TestSuites\MS-SMB\Installer\.*

You can also run **build.cmd** for protocol SDK and every test suite separately.

To build protocol SDK
```
cd WindowsProtocolTestSuites\ProtoSDK
build.cmd
```

To build a test suite, take SMB test suite as an example
```
cd WindowsProtocolTestSuite\TestSuites\MS-SMB\src
build.cmd
```
## Documentation
Every test suite contains three kinds of documents:

* **Technical Document**. The Open Specifications documentation for protocols, published by Microsoft. It's the basis of developing Test Suites.
* **Test Design Spec**.  It provides information about the test scope and test suite design.
* **User Guide**. It provides information about how to set up environment and how to install, configure, and run Test Suites. 

## Contribute

You can find contributing guide [here](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/CONTRIBUTING.md).

## License

Windows Protocol Test Suites is under the [MIT license](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/LICENSE.txt).
