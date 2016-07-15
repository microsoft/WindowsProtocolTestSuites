# Windows Protocol Test Suites

Originally developed for in-house testing of the Microsoft Open Specifications, Microsoft Protocol Test Suites have been used extensively during Plugfests and interoperability labs to test against partner implementations.
A Test Suite evaluates whether a protocol or protocol family implementation meets certain interoperability requirements.
Test Suites do not cover every protocol requirement and in no way certify an implementation, even if all tests pass. 
However, each test suite provides users with a useful indication of interoperability.

Windows Protocol Test Suites provide interoperability testing against the implementation of Windows open specifications including File Services, Identity Management, Remote Desktop and etc.

* **SMB1 Server Test Suite**. It covers the requirements documented by [MS-SMB], and those documented by other protocols which are related to [MS-SMB], i.e. [MS-FSCC] and [MS-CIFS].
* **File Server Family Test Suite**. It is designed to test implementations of file server protocol family including [MS-SMB2], [MS-DFSC], [MS-SWN], [MS-FSRVP], [MS-FSA], [MS-RSVD] and [MS-SQOS].
* **RDP Client Family Test Suite**. It provides interoperability testing for client implementation of RDP family protocols including [MS-RDPBCGR], [MS-RDPEDISP], [MS-RDPEDYC], [MS-RDPEGFX], [MS-RDPEGT], [MS-RDPEI], [MS-RDPEMT], [MS-RDPEUDP], [MS-RDPEUSB], [MS-RDPEVOR] and [MS-RDPRFX].  
* **Kerberos Server Test Suite**. It is designed to test server implementations of Kerberos protocols including [MS-KILE], [MS-KKDCP] and [MS-PAC].

## Components
Windows Protocol Test Suites contain 4 components:

* **CommonScripts**. Common scripts used by every test suite. Normally they're used to deploy the environment.
* **ProtocolTestManager**. A UI tool to help you configure and run test cases.
* **ProtoSDK**. The protocol library used by every test suite. It provides the data structures of the protocol messages, the methods to encode and decode the messages, the methods to send and receive messages and etc.
* **TestSuites**. All Test Suites code and documents are saved here and categorized by folder representing each test suite.


## Prerequisites
The Test Suites are developed and must be installed on a Windows platform.
You should install the following list of software in order to build Test Suites from source code.

* .Net framework 3.5
* [Wix toolset](http://wixtoolset.org/) v3.7 or higher
* [Visual Studio](https://www.microsoft.com/en-us/download/details.aspx?id=30682) or [Visual Studio Agent](https://www.microsoft.com/en-us/download/details.aspx?id=38186), version 2012 or higher
* [Protocol Test Framework](https://github.com/microsoft/protocoltestframework). You can use a released **ProtocolTestFrameworkInstaller.msi** or build it from source code.
* [Spec Explorer](https://visualstudiogallery.msdn.microsoft.com/271d0904-f178-4ce9-956b-d9bfa4902745/). It is only required for the test suites that contain Model Based Test cases. If you want to regenerate Model Based Test cases, you must install Visual Studio 2012, otherwise higher versions of Visual Studio are supported.

## Model-Based Testing

Some test suites use [Model-Based Testing](https://msdn.microsoft.com/en-us/library/ee620469.aspx):

* **SMB1 Server Test Suite**
* **File Server Family Test Suite**

To be able to build the above test suites, you should not only install [Spec Explorer](https://visualstudiogallery.msdn.microsoft.com/271d0904-f178-4ce9-956b-d9bfa4902745/), but also build [Protocol Test Framework](https://github.com/microsoft/protocoltestframework)
by **formodel** option and install the built **ProtocolTestFrameworkInstaller.msi**.

## Build

After you [clone a copy](https://help.github.com/articles/cloning-a-repository/) of this repo, change to your local **WindowsProtocolTestSuites** directory:

```
cd WindowsProtocolTestSuites
```

Run **buildall.cmd**

```
buildall.cmd
```

After the build succeeds, the MSI (installer package) file of every test suite should be generated in the folder *WindowsProtocolTestSuite\drop\TestSuites\\[TestSuiteName]\Installer\.*
Take SMB test suite as an example, **MS-SMB-TestSuite-ServerEP.msi** should be generated in the folder *WindowsProtocolTestSuite\drop\TestSuites\MS-SMB\Installer\.*

You can also run **build.cmd** for protocol SDK, Protocol Test Manager and every test suite separately.

To build protocol SDK
```
cd WindowsProtocolTestSuites\ProtoSDK
build.cmd
```

To build Protocol Test Manager
```
cd WindowsProtocolTestSuites\ProtocolTestManager
build.cmd
```

To build a test suite, take SMB test suite as an example
```
cd WindowsProtocolTestSuites\TestSuites\MS-SMB\src
build.cmd
```
## Run
After the build succeeds, you could set up the test environment and install/configure/run the test suite according to its **User Guide**.
Every test suite has its own **User Guide** in the **WindowsProtocolTestSuites\TestSuites\\[TestSuiteName]\docs** folder.
There're two more documents in the same folder:

* **Technical Document**. The Open Specifications documentation for protocols, published by Microsoft. It's the basis of developing Test Suites.
* **Test Design Spec**.  It provides information about the test scope and test suite design.

## Contribute

You can find contributing guide [here](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/CONTRIBUTING.md).

## License

Windows Protocol Test Suites are under the [MIT license](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/LICENSE.txt).
  

## Contact
The following resources are for Windows protocol test suite news, discussion, and support:
* View news announcements in [Open Specification Windows Protocols Forum](https://social.msdn.microsoft.com/Forums/en-US/home?forum=os_windowsprotocols).
* Discuss test suites issues [here](https://github.com/Microsoft/WindowsProtocolTestSuites/issues) on the github.
* For [Open Specifications Protocols](https://msdn.microsoft.com/en-us/library/gg685446.aspx) support, contact dochelp@microsoft.com.

## Microsoft Open Source Code of Conduct
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.