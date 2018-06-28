# Windows Protocol Test Suites

Originally developed for in-house testing of the Microsoft Open Specifications, Microsoft Protocol Test Suites have been used extensively during Plugfests and interoperability labs to test against partner implementations.
A Test Suite evaluates whether a protocol or protocol family implementation meets certain interoperability requirements.
Test Suites do not cover every protocol requirement and in no way certify an implementation, even if all tests pass. 
However, each test suite provides users with a useful indication of interoperability.

Windows Protocol Test Suites provide interoperability testing against the implementation of Windows open specifications including File Services, Identity Management, Remote Desktop and etc.

* **SMB1 Server Test Suite**. It covers the requirements documented by [MS-SMB], and those documented by other protocols which are related to [MS-SMB], i.e. [MS-FSCC] and [MS-CIFS].
* **File Server Family Test Suite**. It is designed to test implementations of file server protocol family including [MS-SMB2], [MS-DFSC], [MS-SWN], [MS-FSRVP], [MS-FSA], [MS-RSVD] and [MS-SQOS].
* **RDP Client Family Test Suite**. It provides interoperability testing for client implementation of RDP family protocols including [MS-RDPBCGR], [MS-RDPEDISP], [MS-RDPEDYC], [MS-RDPEGFX], [MS-RDPEGT], [MS-RDPEI], [MS-RDPEMT], [MS-RDPEUDP], [MS-RDPEUSB], [MS-RDPEVOR] and [MS-RDPRFX].
* **RDP Server Family Test Suite**. It provides interoperability testing for server implementation of RDP family protocols including [MS-RDPBCGR] and [MS-RDPEMT].  
* **Kerberos Server Test Suite**. It is designed to test server implementations of Kerberos protocols including [MS-KILE], [MS-KKDCP] and [MS-PAC].
* **SMBD Server Test Suite**. It is designed to test the implementations of SMB2&3 direct (RDMA) protocol, as specified in [MS-SMBD] and [MS-SMB2].
* **Branch Cache Test Suite**. It is designed to test the implementations of [MS-PCCRTP], [MS-PCCRR], [MS-PCHC] and [MS-PCCRC] protocol.
* **AZOD Test Suite**. It is designed to test the implementations of [MS-AZOD] protocol.
* **ADFamily Test Suite**. It is designed to test the implementations of the Active Directory protocols including [MS-ADTS], [MS-APDS], [MS-DRSR] [MS-FRS2], [MS-LSAD], [MS-LSAT], [MS-SAMR] and [MS-NRPC]. 
* **ADFSPIP Client Test Suite**. It is designed to test the implementations of ADFS Proxy and Web Application Proxy integration, as described in [MS-ADFSPIP].
* **ADOD Test Suite**. It is designed to test the implementations of [MS-ADOD] protocol.

## Components
Windows Protocol Test Suites contain 4 components:

* **CommonScripts**. Common scripts used by each test suite. Normally they're used to deploy the environment.
* **ProtocolTestManager**. A UI tool to help you configure and run test cases.
* **ProtoSDK**. The protocol library used by each test suite. It provides the data structures of the protocol messages, the methods to encode and decode the messages, the methods to send and receive messages and etc.
* **TestSuites**. All Test Suites code and documents are saved here and categorized by folder representing each test suite.


## Prerequisites
The Test Suites are developed and must be installed on a Windows platform.
You should install the following list of software in order to build Test Suites from source code.
You can use the script in **InstallPrerequisites** folder to automatically download and install these software.

* .Net Framework 4.7.1 or higher.
* [Wix toolset](http://wixtoolset.org/) v3.11 or higher.
* Visual Studio or Visual Studio Agent, version 2012, 2015 or 2017.
* [Protocol Test Framework](https://github.com/microsoft/protocoltestframework). You can use a released MSI file or build it from source code.
* [Spec Explorer](https://visualstudiogallery.msdn.microsoft.com/271d0904-f178-4ce9-956b-d9bfa4902745/). It is only required for the test suites that contain Model-Based Test cases. If you want to regenerate Model-Based Test cases, you must install Visual Studio 2012, otherwise higher versions of Visual Studio are supported.

Tips when using the script in **InstallPrerequisites** folder:

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

## Model-Based Testing

Some test suites use [Model-Based Testing](https://msdn.microsoft.com/en-us/library/ee620469.aspx):

* **SMB1 Server Test Suite**
* **File Server Family Test Suite**
* **Kerberos Server Test Suite**
* **SMBD Server Test Suite**
* **BranchCache Server Test Suite**
* **ADFamily Test Suite**
* **ADFSPIP Client Test Suite**
* **AZOD Test Suite**
* **ADOD Test Suite**

## Build

After you [clone a copy](https://help.github.com/articles/cloning-a-repository/) of this repo, you can run **build.cmd** for Protocol Test Manager and each test suite separately.

### Build Protocol Test Manager

```
cd WindowsProtocolTestSuites\ProtocolTestManager
build.cmd
```

### Build a test suite

If the test suite contains Model-Based test cases, you should follow the steps below before building it. 

* Install [Spec Explorer](https://visualstudiogallery.msdn.microsoft.com/271d0904-f178-4ce9-956b-d9bfa4902745/), 
* Install the **formodel** version of [Protocol Test Framework](https://github.com/microsoft/protocoltestframework). You can use a released MSI file or build it from source code by **formodel** option.

If the test suite does not contain Model-Based test cases, you should follow the step below before building it.

* Install the **nomodel** version of [Protocol Test Framework](https://github.com/microsoft/protocoltestframework). You can use a released MSI file or build it from source code without any options.

After that, take SMB test suite as an example
```
cd WindowsProtocolTestSuites\TestSuites\MS-SMB\src
build.cmd
```

After the build succeeds, the MSI file of each test suite should be generated in the folder *WindowsProtocolTestSuite\drop\TestSuites\\[TestSuiteName]\deploy\.*
Take SMB test suite as an example, **MS-SMB-TestSuite-ServerEP.msi** should be generated in the folder *WindowsProtocolTestSuite\drop\TestSuites\MS-SMB\deploy\.*

## Run
After the build succeeds, you could set up the test environment and install/configure/run the test suite according to its **User Guide**.
Each test suite has its own **User Guide** in the **WindowsProtocolTestSuites\TestSuites\\[TestSuiteName]\docs** folder.
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
