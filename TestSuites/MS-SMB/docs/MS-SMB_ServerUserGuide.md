#MS-SMB Server Test Suite User Guide

## <a name="1"/>Contents

* [Introduction](#2)
* [Quick Start Checklist](#3)
* [Requirements](#4)
    * [Network Infrastructure](#4.1)
    * [Environment](#4.2)
    * [Driver Computer](#4.3)
    * [System Under Test (SUT)](#4.4)
    * [Domain Controller (DC)](#4.5)
    * [Software](#4.6)
* [Network Setup](#5)
    * [Workgroup Environment](#5.1)
    * [Domain Environment](#5.2)
    * [Verify Connectivity from the Driver Computer](#5.3)
* [Computer Setup](#6)    
    * [Set Up the Domain Controller](#6.1)
    * [Set Up the Driver Computer](#6.2)
    * [Set Up the SUT](#6.3)
    * [Installed Files and Folders](#6.4)
* [Configuration](#7)
    * [Configuring Windows-based Computers](#7.1)
    	* [Configure the Driver Computer](#7.1.1)
		* [Configure the SUT](#7.1.2)
		* [Configure the DC](#7.1.3)
	* [Configuring Computers that are Not Based on Windows](#7.2)
		* [Configure a DC that is Not Windows-based](#7.2.1)
		* [Configure a SUT that is Not Windows-based](#7.2.2)
	* [Configuring the Test Suite](#7.3)
* [Running Test Cases](#8)
    * [Run All Test Cases](#8.1)
    * [Run Test Cases by Category](#8.2)
    * [Check Test Results](#8.3)
* [Debugging Test Cases](#9)

## <a name="2"/>Introduction
This guide provides information about how to install, configure, and run MS-SMB Test Suite and its environment. This suite of tools is designed to test implementations of Server Message Block protocol, 
as specified in the Microsoft document [MS-SMB]: Server Message Block Protocol Specification. This guide provides information about using this test suite on the Microsoft? Windows? operating system and on operating systems that are not Windows based. 
This suite of tools tests only the protocol implementation behaviors that are observed on the wire. 
For detailed information about the design of this test suite, see [MS-SMB_ServerTestDesignSpecification](./MS-SMB_ServerTestDesignSpecification.md). 
 
## <a name="3"/>Quick Start Checklist
The following checklist summarizes the steps you need to complete to get the test suite up and running. The checklist also provides references to documentation that can help you get started. 

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

	For workgroup environment, omit tasks that are related to the setup and configuration of domain controllers.
	
 __Check__ | __Task__  | __Topic__
 ----------------|----------|-----------
                 |Download the test suite for the protocol implementation |For a list of the files that the download package contains, see [Installed Files and Folders](#6.4).
                 |Confirm that your test environment and computers meet the requirements of the test suite|For information about the requirements of the test suite, see [Requirements](#4). 
                 |Install the software prerequisites|For information about software that must be installed on the computers in your test environment before the test suite is installed, see [Software](#4.6).
                 |Set up the driver computer|See [Set Up the Driver Computer](#6.2).
                 |Set up the system under test (SUT) |See [Set Up the SUT](#6.3).
                 |Set up the domain controller (DC)|See [Set Up the Domain Controller](#6.1)
				 |Set up the network|See [Network Setup](#5).
				 |Verify the connection from the driver computer to the SUT and other computers|See [Verify Connectivity from the Driver Computer](#5.3).
				 |Configure the SUT|See [Configure the SUT](#7.1.2) or [Configure a SUT that is Not Windows-based](#7.2.2).
				 |Configure the driver computer|See [Configure the Driver Computer](#7.1.1).
				 |Configure the test suite settings|See [Configuring the Test Suite](#7.3).
                 
## <a name="4"/>Requirements 
This section describes the requirements for the test environment that are used to run this test suite.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

	The requirements in this section apply only to the Windows-based computers in the test environment. 
	Note that the driver computer must use a Windows-based operating system.
	
![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

	Workgroup environment do not require a domain controller.

### <a name="4.1"/>Network Infrastructure
* A test network is required to connect the test computer systems.
* It must consist of an isolated hub or switch.
* It must not be connected to a production network or used for any other business or personal communications or operations.
* It must not be connected to the internet.
* IP addresses must be assigned for a test network.
* Computer names should be assigned in a test network infrastructure.
* User credentials used on the system must be dedicated to the test network infrastructure.
* Details including computer IP addresses, names and credentials are saved in log files.

### <a name="4.2"/>Environment
Run this test suite in a workgroup environment that contains the following computers, physical or virtual: 

* A driver computer running Microsoft Windows 8.1 or later versions.
* A computer set up as a Windows-based SUT running Windows Server 2012 R2 or later versions, or a computer set up as a SUT that is not based on the Windows operating system.

Run this test suite in a domain environment that contains the following computers, physical or virtual: 

* A driver computer running Microsoft Windows 8.1 or later versions.
* A computer set up as a Windows-based SUT running Windows Server 2012 R2 or later versions, or a computer set up as a SUT that is not based on the Windows operating system.
* A computer set up as a domain controller (DC) running Windows Server 2012 R2 or later versions, or a computer set up as DC that is not based on the Windows operating system service.

### <a name="4.3"/>Driver Computer 
The minimum requirements for the driver computer are as follows. 

 __Requirement__ | __Description__ 
 ----------------|----------
 Operating system|Microsoft Windows 8.1 or later versions
 Memory|2 GB RAM
 Disk space|60 GB 
### <a name="4.6"/>Software
 
 All of the following software must be installed on the driver computer before the installation of this test suite. 
 
**Required Software**

All common softwares listed in [prerequisites](https://github.com/microsoft/WindowsProtocolTestSuites#prerequisites) for running Windows Protocol Test Suites.

* **Windows PowerShell 3.0 or later**

    **Windows PowerShell 3.0 or later** is required.

**Optional Software**

* **Microsoft® Message Analyzer**

    **Microsoft® Message Analyzer** (MMA) is listed here as an optional tool because the test cases of themselves neither perform live captures or capture verifications during execution. However, MMA can be helpful with debugging test case results, by analyzing ETL files that are generated by the Test Cases, that is, if you enable the the Automatic Network Capturing feature in the Protocol Test Manager (PTM) during test case configuration. The Automatic Network Capturing feature is further described in the [PTF User Guide](https://github.com/Microsoft/ProtocolTestFramework/blob/staging/docs/PTFUserGuide.md#-automatic-network-capturing).

    ![](./images/note.png)Note

    November 25 2019 - Microsoft Message Analyzer (MMA) has been retired and removed from public-facing sites on microsoft.com. A private MMA build is available for testing purposes; to request it, send an email to [getmma@microsoft.com](mailto:getmma@microsoft.com).

### <a name="4.4"/>System Under Test (SUT)
 The minimum requirements for the SUT are as follows.
 
  __Requirement__ | __Description__ 
 ----------------|----------
 Operating system|Microsoft Windows Server 2012 R2, Enterprise Edition or later versions, or a SUT implementation that is not based on the Windows operating system 
 Memory|1 GB RAM
 Disk space|60 GB 
 
#### Required Software
* __Printer Driver(Printer Driver for PCL)__

    You can download Printer Driver from below website
    
    [From here](https://support.brother.com/g/b/midlink_os.aspx?c=us&lang=en&prod=hl5350dn_us_as&site=pc&type3=434&orgc=us&orglang=en&orgprod=hl5350dn_us_as&targetpage=18)
 
### <a name="4.5"/>Domain Controller (DC)
  
   __Requirement__ | __Description__ 
 ----------------|----------
 Operating system|Microsoft Windows Server 2012 R2, Enterprise Edition or later versions, or a DC implementation that is not based on the Windows operating system
 Services|Active Directory Domain Services (AD DS)
 Memory|1 GB RAM
 Disk space|60 GB 



## <a name="5"/>Network Setup
Run this test suite in a workgroup or domain environment using either physical or virtual machines. This section describes the test environment using physical computers. 

For information about configuring a virtual machine, see [https://docs.microsoft.com/en-us/virtualization/hyper-v-on-windows/quick-start/create-virtual-machine](https://docs.microsoft.com/en-us/virtualization/hyper-v-on-windows/quick-start/create-virtual-machine). 
The configuration of virtual machines for use with this test suite is out of the scope of this guide. 

### <a name="5.1"/>Workgroup Environment

The workgroup environment requires interactions between the following computers.

* The driver computer runs the test cases by sending requests over the wire in the form of protocol messages. 
* The SUT runs an implementation of the protocol that is being tested. The SUT responds to the requests that the driver computer sends.

The following figure shows the workgroup environment. 

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/workgroup.png)

Machine Name|IPv4| Subnet Mask|Default Gateway|DNS Server
----------------|--------|----------------|-------------------|--------------
Driver|	192.168.1.111|	255.255.255.0|	|
SUT	|192.168.1.11	|255.255.255.0	|	|

### <a name="5.2"/>Domain Environment

The domain environment requires interactions between the following computers and server roles. 

* The driver computer runs the test cases by sending requests over the wire in the form of protocol messages. 
* The SUT runs an implementation of the protocol that is being tested. The SUT responds to the requests that the driver computer sends.
* The DC provides functionality that is required to test the protocol implementation. Specifically, the DC hosts Active Directory Domain Services (AD DS) and supports the protocol implementation.

The following figure shows the domain environment. 

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/domain.png)

Machine Name|IPv4| Subnet Mask|Default Gateway|DNS Server
----------------|--------|----------------|-------------------|--------------
DC|	192.168.1.1|	255.255.255.0|	|127.0.0.1
Driver|192.168.1.111|255.255.255.0|192.168.1.1|192.168.1.1
SUT|192.168.1.11|255.255.255.0|192.168.1.1|192.168.1.1

### <a name="5.3"/>Verify Connectivity from the Driver Computer

After you install the environment, verify the connection from the driver computer to the SUT, and between all other computers in the test environment. 
The following provides a general list of steps you can use to check for connectivity between two Windows-based computers. For further information, see the administration guide for your operating system.

To check the connection from the driver computer

1.  Disable active firewalls in the test environment.
2.  Click the Start button, and then click Run. 
3.  In the Run dialog box, type cmd and then click OK.
4.  At the command prompt, type ping followed by the hostname or IP address of the SUT, and then press Enter. The following example checks the connection to a SUT named "SUT01":

		> ping SUT01
5.  Repeat these steps until you confirm connectivity between all computers in the test environment.

Do not proceed with the configuration of the test suite until connectivity is confirmed. Any issues with network connectivity must be resolved before you configure the test suite.

## <a name="6"/>Computer Setup 

This section explains how to set up the computers for the test environment.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

	For workgroup environment, omit tasks that are related to the setup and configuration of domain controllers.
		
### <a name="6.1"/>Set Up the Domain Controller		

This section provides information about how to set up a DC for use with this test suite.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

	Please ignore this step if testing under workgroup environment
		
__To set up a Windows-based DC__

1.  Install Active Directory Domain Services manually 
2.  Promote to Domain Controller: Domain name is contoso.com and Administrator's password is "Password01!"

To set up a DC that is not based on the Windows operating system, see [Configure a DC that is Not Windows-based](#7.2.1).

### <a name="6.2"/>Set Up the Driver Computer

This section describes how to set up the driver computer.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/important.png) Important

	Microsoft Visual Studio 2017, Protocol Test Framework, and Spec Explorer must be installed on the driver 
	computer before you run the test suite installer.
		
__To set up the driver computer__

1.	Join the computer to domain (ignore this step if it's a workgroup environment).
2.	Copy the test suite package to the driver computer.
3.	Extract all the files from the package, but do not install them.
4.	Install the required and optional software described earlier.
5.	Run the MS-SMB-TestSuite-ServerEP.msi installer on the driver computer.
6.	When options are prompted, select the option, Install Test Suite on Driver Computer.

### <a name="6.3"/>Set Up the SUT

This section provides information about how to set up a SUT for use with this test suite.

__To set up a Windows-based SUT__

1.	Join the computer to domain (ignore this step if it's a workgroup environment).
2.	Copy MS-SMB-TestSuite-ServerEP.msi installer on the Windows-based SUT.
3.	Run the MS-SMB-TestSuite-ServerEP.msi installer on the Windows-based SUT.
4.	When options are prompted, select the option, Install and configure Windows System Under Test (SUT).

To set up a SUT that is not based on the Windows operating system, see [Configure a SUT that is Not Windows-based](#7.2.2).

### <a name="6.4"/>Installed Files and Folders

The installation process adds the following folders and files to the driver computer at C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\\.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

	This path may vary based on your installation location.
	The <version#> placeholder indicates the installed build of the test suite.

File or Folder|	Description
--------------|-------------
Batch|Command files you can use to run individual test cases or all test cases.
Bin|Test suite binaries and configuration files.
Scripts   |Scripts that are used to set up and configure the driver computer and the Windows-based SUT, and the Windows-based DC.
LICENSE.rtf|The End User License Agreement.

The installation process adds the following files and folders to the Windows-based SUT at C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\\.

File or Folder|	Description
--------------|-------------
Scripts|Scripts that are used to set up and configure the driver computer and the Windows-based SUT, and the Windows-based DC.
LICENSE.rtf|The End User License Agreement.

## <a name="7"/>Configuration

This section explains how to configure the test environment.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

	For workgroup environment, omit tasks that are related to the setup and configuration of domain controllers.
	
### <a name="7.1"/>Configuring Windows-based Computers

This section explains how to configure computers for a Windows-based test environment. 
For general information about configuring computers that are not based on Windows, see [Configuring Computers that are Not Based on Windows](#7.2).

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

	For domain environment, add the driver computer and SUT to the existing domain that is created on the DC 
	before starting to configure Windows-based Computers.
	
### <a name="7.1.1"/>Configure the Driver Computer

This section provides a general list of steps that you can use to configure the driver computer in a Windows-based test environment. For specific information about how to complete these steps, see the administration guide for your operating system.

__To configure the driver computer__

1.	Log on to the driver computer as domain administrator for domain environment; log on as local administrator for workgroup environment. 
2.	Go to C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Scripts, and open the ParamConfig.xml file.
3.	Edit the properties as shown in the following table.

    Property	|Description
    ------------|-------------
    domainInVM|	The domain Name, the value will be ignored in Workgroup test environment. The default value is "contoso.com"
    IPVersion| The IP version used in the protocol. The default value is "IPv4"
    logFile| The file path for storing the logs during configuration. The default value is "..\TestResults\Config-Server.ps1.log"
    logPath| The path to store log file. The default value is "..\TestResults"
    serverComputerName|	The computer name of SUT. The default value is "SUT01"
    sutOS| The OS version of SUT. The default value is "Win8Server"
    userNameInVM| The local administrator account that is used to log on to the VMs. The default value is "administrator".
    userPwdInVM| The password that is used to log on to the local administrator account. The default value is "Password01!"
    workgroupDomain| The test environment. If it is in domain environment, set the value to "Domain"; if it is in workgroup environment, set the value to "Workgroup". The default value is "Domain".
4.	Start Windows? PowerShell? by right-clicking on the Windows PowerShell icon, and then click Run as Administrator or, from a Windows PowerShell command window, type:
Start-process powershell -verb runAs
5.	At the command prompt, type Set-ExecutionPolicy Unrestricted -F, and press Enter.
6.	Type cd C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Scripts, and press Enter.
7.	Type .\Config-Client.ps1, and press Enter.

### <a name="7.1.2"/>Configure the SUT

This section provides a general list of steps that you can use to configure the SUT in a Windows-based test environment. For specific information about how to complete these steps, see the administration guide for your operating system.

__To configure the SUT__

1.	Log on to the SUT as domain administrator for domain environment; log on as local administrator for workgroup environment.
2.	Go to C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Scripts, and open the ParamConfig.xml file. 
3.	Edit the properties as shown in the following table.

    Property| Description
    --------|-------------
    domainInVM|	The domain Name, the value will be ignored in Workgroup test environment. The default value is "contoso.com"
    IPVersion| The IP version used in the protocol. The default value is "IPv4"
    logFile| The file path for storing the logs during configuration. The default value is "..\TestResults\Config-Server.ps1.log"
    logPath| The path to store log file. The default value is "..\TestResults"
    serverComputerName|	The computer name of SUT. The default value is "SUT01"
    sutOS| The OS version of SUT, for Non-windows implementation, set it to "NonWindows". The default value is "Win8Server"
    userNameInVM| The local administrator account that is used to log on to the VMs. The default value is "administrator".
    userPwdInVM| The password that is used to log on to the local administrator account. The default value is "Password01!"
    workgroupDomain| The test environment. If it is in domain environment, set the value to "Domain"; if it is in workgroup environment, set the value to "Workgroup". The default value is "Domain".
4.	Start Windows PowerShell by right-clicking on the Windows PowerShell icon, and then click Run as Administrator or, from a Windows PowerShell command window, type:
	Start-process powershell -verb runAs
5.	At the command prompt, type Set-ExecutionPolicy Unrestricted -F, and press __Enter__.
6.	Type cd C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Scripts, and press __Enter__.
7.	Type .\Config-Server.ps1 and press __Enter__.

### <a name="7.1.3"/>Configure the DC

For domain environment, we need to enable Guest account in DC.

1.	Open "Server Manager" > Tools > Active Directory Users and Computers.
2.	Open tree to Active Directory Users and Computers > Contoso.com > Users
3.	Right-click on "Guest" account, select "All Tasks" > Enable Account.
4.	After enable, right-click on the account, select "All Tasks" > Reset Password.
5.	In the "Reset Password" dialog, set the password to the one we use for test, by default, it is "Password01!"

## <a name="7.2"/>Configuring Computers that are Not Based on Windows

This guide provides only basic information about configuring the test environment for computers that are not running Windows-based operating systems. 

* For domain environment, join all computers to the domain of the domain controller.
* Disable active firewalls on all computers.

### <a name="7.2.1"/>Configure a DC that is Not Windows-based

This section provides general instructions for the configuration of a DC that runs an operating system other than the Windows operating system.
* Install directory service on DC (This is not required if testing under workgroup environment)
* Promote it to domain controller
* Enable "Guest" account and set password to "Password01!"

### <a name="7.2.2"/>Configure a SUT that is Not Windows-based

This section provides basic information about the configuration of an SUT that runs an operating system other than the Windows operating system. 

For information about how to configure a Windows-based SUT, see [Configure the SUT](#7.1.2). For detailed instructions about how to complete the tasks that this process requires, see the administration guide for your operating system. 

__To configure the SUT__

* Disable active firewalls.
* Turn on file and printer sharing.
* Enable the local guest account and set password to "Password01!"
* Install DFS management, create a standalone namespace "DFSNamespace".
* Create a NTFS disk.
* Create a FAT disk (Default disk is NTFS disk, if the Server OS already has a FAT disk, please skip this step).
* Create directories and files on the NTFS disk to be shared and used by the MS-SMB test suite:
	* Create two folders named "Sharefolder1" and "Sharefolder2" under the shared root directory which was created as a NTFS disk
	* Set folders "Sharefolder1" and "Sharefolder2" share property to "Everyone" and grant "Read/Write" permission
	* Create a .txt file named "ExistTest.txt" with any content at the root of "Sharefolder1" and "Sharefolder2"
	* Create one folder named "QuotaShare" under the shared root directory which was created as a NTFS disk 
	* Set folder "QuotaShare" share property to local and domain (ignore this if it's a workgroup environment) "Guest" user and grant "Full Control" permission
* Create directories and files on the FAT disk to be shared and used by the MS-SMB test suite:
	* Create two folders named "Sharefolder3" and "Sharefolder4" under the shared root directory which was created as a FAT disk.
	* Set folders "Sharefolder3" and "Sharefolder4" share property to "Everyone" and grant "Read/Write" permission
* Create a shadow copy for the NTFS disk and then change content of "ExistTest.txt" under the folders "Sharefolder1" and "Sharefolder2". Make sure "ExistTest.txt" has a previous version.
* Create a physically nonexistent printer and select the printer's model as "Brother Color Type3 Class Driver", share this printer with share name "SMBPrinter".

## <a name="7.3"/>Configuring the Test Suite

This test suite is installed with default configuration settings. You may need to change these settings if you use a customized test environment or if you customize your test runs. 
You can configure the test suite for various purposes including, for example, to:

* Define the settings of the test environment, including computer names and IP addresses.
* Define the basic options used in the test suite, for example, the protocol version or the version of the target operating system.
* Define the folders and formats used for output from test runs.
* Define scripts to run before and after each test run.
* Set time limits on discrete test tasks and for test runs.

To change configuration settings, edit the MS-SMB_ServerTestSuite.deployment.ptfconfig file. You can find this file in the directory C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\\Bin.

## <a name="8"/>Running Test Cases

This test suite includes command files that you can use to complete some basic test cases. Each test case verifies the protocol implementation based on a given scenario. 
You can find and run these test cases in the following directory: 

	C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Batch
	
You can run these command files at the command prompt, or by selecting and clicking one or more of the files from the directory.
For test environments that are not Windows based 
Before you run test cases, be sure to complete the following tasks: 

* Implement all the features that are listed in C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Bin\NonWindows\
* Copy and replace all files from C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Bin\NonWindows\to C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Bin\

### <a name="8.1"/>Run All Test Cases

Use the steps below to run all test cases.

* From the desktop of the driver computer, double-click the Run MS-SMB Server-EP Test Cases shortcut. This shortcut is created during the installation process.
Alternatively, go to C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Batch, and double-click the RunAllTestCases.cmd file.

### <a name="8.2"/>Run Test Cases by Category 

Use the steps below to run test cases by category.

* From the directory C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Batch, double-click the RunTestCasesByCategory.cmd file.

### <a name="8.3"/>Check Test Results

Test suite generates test result files in different paths based on the way how test case is executed.

For running test case with batch: C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\\Batch\TestResults

For running test case with Visual Studio: C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\\Source\Server\TestCode\TestResults

For further information about logging in the Protocol Test Framework (PTF), see the PTF User Guide in the PTF installation directory

## <a name="9"/>Debugging Test Cases

You can use the Visual Studio solution (.sln) file included with this test suite to debug additional test cases that you create for your protocol implementation. 

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/note.png) Note

* Copy MS-SMB_ServerTestSuite.deployment.ptfconfig from C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\\Bin to C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\\Source\Server\TestCode\TestSuite and replace the original file
* While using Microsoft Visual Studio 2017 or above to run test cases, test suite may throw exception with message of "Cannot get test site". To solve this issue, please select the test settings file under test settings menu.

	![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/testsite.png) 

__To debug a test case__

1.	On the driver computer, use Microsoft? Visual Studio? 2017 or above to open the following solution file:
	C:\MicrosoftProtocolTests\MS-SMB\Server-Endpoint\<version#>\Source\Server\TestCode\MS-SMB_Server.sln
2.	In Visual Studio, in the Solution Explorer window, right-click the Solution 'MS-SMB_Server', and select Build Solution.
3.	When you build the test project, the tests appear in Test Explorer. If Test Explorer is not visible, choose Test on the Visual Studio menu, choose Windows, and then choose Test Explorer.
4.	Select your test cases from Test Explorer and run or debug them.  

 

