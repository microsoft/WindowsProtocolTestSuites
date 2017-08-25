# RDP Server Test Suite User Guide

## Contents

* [Introduction](#_Toc396908219)
* [License Information](#_Toc396908220)
* [Further Assistance](#_Toc396908221)
* [Quick Start Checklist](#_Toc396908222)
* [How Do I?](#_Toc396908223)
* [Requirements](#_Toc396908224)
* [Environment](#_Toc396908225)
    * [Driver Computer](#_Toc396908226)
    * [System Under Test (SUT)](#_Toc396908227)
    * [Domain Controller (DC)](#_Toc396908228)
    * [Software](#_Toc396908229)
* [Network Setup](#_Toc396908230)
    * [Network Infrastructure](#_Toc396908231)
    * [Domain Environment](#_Toc396908232)
    * [Workgroup Environment](#_Toc396908233)
    * [Verify Connectivity from the Driver Computer](#_Toc396908234)
* [Computer Setup](#_Toc396908235)
    * [Set Up the Driver Computer](#_Toc396908236)
    * [Set Up a Windows-Based SUT](#_Toc396908237)
    * [Set Up a Windows-Based Domain Controller (DC)](#_Toc396908238)
    * [Installed Files and Folders](#_Toc396908239)
* [Configuration](#_Toc396908240)
    * [Configuring Windows-based Computers](#_Toc396908241)
    * [Configuring Computers that are Not Based on Windows](#_Toc396908242)
    * [Configuring the Test Suite](#_Toc396908243)
* [Running Test Cases](#_Toc396908244)
    * [Run All Test Cases](#_Toc396908245)
    * [Check Test Results](#_Toc396908246)
* [Debugging Test Cases](#_Toc396908247)
* [Troubleshooting](#_Toc396908248)
    * [Ping Failure](#_Toc396908249)

## <a name="_Toc396908219"/>Introduction

This guide provides information about how to install, configure, and run the RDP Server Endpoint Test Suite and its environment.

This suite of tools is designed to test implementations of the following protocols:

* _[MS-RDPBCGR]: Remote Desktop Protocol: Basic Connectivity and Graphics Remoting Specification_

This suite of tools tests only the protocol implementation behaviors that are observed on the wire. For detailed information about the design of this test suite, see [MS-RDPBCGR_ServerTestDesignSpecification](MS-RDPBCGR_ServerTestDesignSpecification.md). 

## <a name="_Toc396908220"/>License Information

For licensing information, see the End User License Agreement (EULA) that was provided with this test suite. The EULA is contained in the LICENSE.rtf file in the installation folder.

## <a name="_Toc396908221"/>Further Assistance

If you need further information about this test suite or assistance in troubleshooting issues related to this test suite, contact dochelp@microsoft.com.

## <a name="_Toc396908222"/>Quick Start Checklist

The following checklist summarizes the steps required to get the test suite up and running. The checklist also provides references to documentation that can help you get started. 

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>For workgroup environments, skip tasks that are related to the setup and configuration of DC.

|  **Check**|  **Task**|  **Topic**| 
| -------------| -------------| ------------- |
| □| Download the test suite for the protocol implementation.| For a list of the files that the download package contains, see [Installed Files and Folders](#_Toc396908239).| 
| □| Confirm that your test environment and computers meet the requirements of the test suite.| For information about the requirements of the test suite, see [Requirements](#_Toc396908224). | 
| □| Install the software prerequisites.| For information about software that must be installed on the computers in your test environment before the test suite is installed, see [Software](#_Toc396908229).| 
| □| Set up the driver computer.| See [Set Up the Driver Computer](#_Toc396908236).| 
| □| Set up the system under test (SUT).| See [Set Up the SUT](#_Toc396908237).| 
| □| Set up the Domain Controller (DC). (optional)| See [Set Up the DC](#_Toc396908238). | 
| □| Set up the network.| See [Network Setup](#_Toc396908230).| 
| □| Verify the connection from the driver computer to the SUT and other computers.| See [Verify Connectivity from the Driver Computer](#_Toc396908234).| 
| □| Configure the SUT.| See [Configuring Windows-based Computers](#_Toc396908241) or | 
| | | [Configuring Computers that are Not Based on Windows](#_Toc396908242)| 
| □| Configure the DC. (optional)| See [Configuring Computers that are Not Based on Windows](#_Toc396908242) | 
| □| Configure the driver computer.| See [Configuring Windows-based Computers](#_Toc396908241)| 
| □| Configure the test suite settings.| See [Configuring the Test Suite](#_Toc396908243).| 
| □| Run test cases to verify that the test suite is properly installed and configured| See [Running Test Cases](#_Running_Test_Cases)[](#_Toc396908244).| 

## <a name="_Toc396908223"/>How Do I?
Use the following quick reference to learn how to complete common tasks.

|  **How do I…?**|  **For more information…**| 
| -------------| ------------- |
| Set up the test environment| [Network Setup](#_Toc396908230) and [Computer Setup](#_Toc396908235)| 
| Verify the connection from the driver computer to other computers in the test environment| [Verify Connectivity from the Driver Computer](#_Toc396908234)| 
| Configure a SUT| [Configure the SUT](#_Install_and_configure) or [Configuring Computers that are Not Based on Windows](#_Toc396908242)| 
| Configure a DC| [Configuring Computers that are Not Based on Windows](#_Toc396908242)| 
| Configure the driver computer| [Configuring Windows-based Computers](#_Toc396908241)| 
| Configure the test suite settings| [Configuring the Test Suite](#_Toc396908243)| 
| Run test cases| [Run All Test Cases](#_Toc396908245)| 
| Debug my own test cases| [Debugging Test Cases](#_Toc396908247)| 
| Get the results of test runs| [Check Test Results](#_Toc396908246)| 
| Troubleshoot problems| [Troubleshooting](#_Toc396908248)| 

## <a name="_Toc396908224"/>Requirements 

This section describes the requirements for the test environment that are used to run this test suite.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>The requirements in this section apply only to the Windows-based computers in the test environment. 

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>The driver computer must use a Windows-based operating system.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>Workgroup environment does not require a domain controller.

## <a name="_Toc396908225"/>Environment

Run this test suite in a Domain environment that contains the following computers, physical or virtual: 

* A driver computer running any version of Windows which is compatible with Visual Studio 2012.

* A computer configured as the SUT (System Under Test). It can be any version of Windows or a SUT implementation that is not based on the Windows operating system.

* A computer configured as a Domain Controller (DC). If this computer is running Windows, it must be running Windows Server 2008 R2, Windows Server 2012 or Windows Server 2012 R2 or later. The DC can be on the SUT.

Run this test suite in a Workgroup environment that contains the following computers, physical or virtual: 

* A driver computer running any version of Windows which is compatible with Visual Studio 2012.

* A computer configured as the SUT (System Under Test). It  can be any version of Windows or a SUT implementation that is not based on the Windows operating system.

### <a name="_Toc396908226"/>Driver Computer 

The minimum requirements for the driver computer are as follows. 

|  **Requirement**|  **Description**| 
| -------------| ------------- |
| Operating system| Any version of Windows which is compatible with Visual Studio 2012.| 
| Memory| 2 GB RAM| 
| Disk space| 60 GB | 

### <a name="_Toc396908227"/>System Under Test (SUT)

The minimum requirements for the SUT are as follows.

|  **Requirement**|  **Description**| 
| -------------| ------------- |
| Operating system| Any version of Windows or a SUT implementation that is not based on the Windows operating system | 
| Memory| 1 GB RAM| 
| Disk space| 60 GB| 

### <a name="_Toc396908228"/>Domain Controller (DC)

The minimum requirements for the DC are as follows.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>DC is optional. Workgroup environments do not require a domain controller.

|  **Requirement**|  **Description**| 
| -------------| ------------- |
| Operating system| Windows Server 2008 R2, Windows 2012, Windows 2012 R2 or later or a directory domain service implementation that is not based on the Windows operating system| 
| Services| Directory domain service (such as the Windows Active Directory Domain Services (AD DS))| 
| Memory| 1 GB RAM| 
| Disk space| 60 GB| 

### <a name="_Toc396908229"/>Software 
All of the following software must be installed on the driver computer _before_ the installation of this test suite. 

|  **Required Software**| 
| ------------- |
| Microsoft® Visual Studio® 2012 Update 4| 
| Protocol Test Framework (build 1.0.5500.0)| 
|  **Optional Software** | 
|       Message Analyzer| 
| ![image3.png](./image/RDP_ServerUserGuide/image3.png)Note| 
|              You can download the latest Message Analyzer from below website. | 
|              [https://www.microsoft.com/en-us/download/details.aspx?id=44226](https://www.microsoft.com/en-us/download/details.aspx?id=44226)| 

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note

Visual Studio 2012 Professional is the minimum requirement.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

Visual Studio need update to Update 4.

## <a name="_Toc396908230"/>Network Setup

You can run this test suite in a workgroup or domain environment using either physical or virtual machines. This section describes the test environment using physical computers. For information about configuring a virtual machine, see [http://www.microsoft.com/virtualization/en/us/solution-appliance-test.aspx](http://www.microsoft.com/virtualization/en/us/solution-appliance-test.aspx). 

### <a name="_Toc396908231"/>Network Infrastructure

* A test network is required to connect the test computer systems

* It must consist of an isolated hub or switch

* It must not be connected to a production network or used for any other business or personal communications or operations

* It must not be connected to the internet 

* IP addresses must be assigned for a test network

* Computer names should be assigned in a test network infrastructure

* User credentials used on the system must be dedicated to the test network infrastructure

* Details including computer IP addresses, names and credentials are saved in log files

Refer to the Privacy Statement and EULA for further information.

### <a name="_Toc396908232"/>Domain Environment

The domain environment requires interactions between the following computers and server roles. Note that the domain controller, required for a domain environment, can be installed on the SUT. 

* The driver computer, which runs the test cases by sending requests over the wire in the form of protocol messages. 

* The SUT, which runs a server implementation of the protocol that is being tested. The SUT responds to the requests sent by the driver computer.

* The DC provides functionality that is required to test the protocol implementation. Specifically, the DC hosts Active Directory Domain Services (AD DS).

The following figure shows the domain environment. 

![image4.png](./image/RDP_ServerUserGuide/image4.png)

### <a name="_Toc396908233"/>Workgroup Environment

The workgroup environment requires interactions between the following computers:

* The driver computer, which runs the test cases by sending requests over the wire in the form of protocol messages. 

* The SUT, which runs a server implementation of the protocol that is being tested. The SUT responds to the requests that the driver computer sends.

The following figure shows the workgroup environment:

![image5.png](./image/RDP_ServerUserGuide/image5.png)

### <a name="_Toc396908234"/>Verify Connectivity from the Driver Computer

After you prepare the environment, verify the connection from the driver computer to the SUT, and between all other computers in the test environment. The following provides a general list of steps that you can use to check for connectivity between two Windows-based computers. For further information, see the administration guide for your operating system.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>Disable active firewalls in the test environment.

###To check the connection from the driver computer

* Click the **Start** button, and then click **Run**. 

* In the **Run** dialog box, type **cmd** and then click **OK**.

* At the command prompt, type **ping** followed by the hostname or IP address of the SUT, and then press **Enter**. The following example checks the connection to a SUT named "SUT01":
 
 &#62;  ping SUT01

* Repeat these steps until you confirm connectivity between all computers in the test environment.

Do not proceed with the configuration of the test suite until connectivity is confirmed. Any issues with network connectivity must be resolved before you configure the test suite.

## <a name="_Toc396908235"/>Computer Setup 

This section explains how to set up the computers for the test environment.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>For workgroup environments, skip tasks that are related to the setup and configuration of DC.

### <a name="_Toc396908236"/>Set Up the Driver Computer

This section describes how to set up the driver computer.

![image6.png](./image/RDP_ServerUserGuide/image6.png)
Important 

>Microsoft Visual Studio 2012 Update 4 and Protocol Test Framework (build 1.0.5500.0) must be installed on the driver computer before you run the test suite installer.

To set up the driver computer

* Copy the test suite package to the driver computer.

* Extract the files of the test suite from the package.

* Install the required and optional software described earlier.

* Run **RDP-TestSuite-ServerEP.msi** on the driver computer.

### <a name="_Toc396908237"/>Set Up a Windows-Based SUT
This section provides information about how to set up a SUT for use with this test suite.

To set up a Windows-based SUT

* Start Remote Desktop Services on the Windows-based SUT.

To set up a SUT that is not based on the Windows operating system, see [Configuring Computers that are Not Based on Windows](#_Toc396908242)**.**

### <a name="_Toc396908238"/>Set Up a Windows-Based Domain Controller (DC)
This section provides information about how to set up a DC for use with this test suite.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

DC is optional. Skip this step if the test environment is workgroup.

To set up a Windows-based DC

* Install Active Directory Domain Services.

To set up a DC that is not based on the Windows operating system, see Configuring Computers that are Not Based on Windows.

### <a name="_Toc396908239"/>Installed Files and Folders

The installation process adds the following folders and files to the driver computer at C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ _&#60; version &#35;  &#62;_ \.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>The  _&#60; version &#35;  &#62;_  placeholder indicates the installed build of the test suite.

|  **File or Folder**|  **Description**| 
| -------------| ------------- |
| Batch| Command files that you can use to run individual test cases or all test cases| 
| Bin| Test suite binaries and configuration files| 
| Docs| The version of each of the following protocol specifications used to develop this test suite:| 
| | **MS-RDPBCGR.pdf** | 
| | **RDP_ServerUserGuide.md** – A user guide that explains how to install and configure the driver computer, the SUT, and the DC, as well as how to run test cases| 
| | **Test suite design documents**, containing conceptual information about the protocol, including probable use, relationships to other protocols, message flow, state diagrams, a short list of properties, and justification for the test and adapter design choices:| 
| |  **MS-RDPBCGR_ServerTestDesignSpecification.md**| 
| LICENSE.rtf| The End User License Agreement| 


## <a name="_Toc396908240"/>Configuration

This section explains how to configure the test environment.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

>For workgroup environments, skip tasks that are related to the setup and configuration of DC.

### <a name="_Toc396908241"/>Configuring Windows-based Computers

This section explains how to configure computers for a Windows-based test environment. For general information about configuring computers that are not based on Windows, see [Configuring Computers that are Not Based on Windows](#_Toc396908242).

To configure the SUT

* Log on to the SUT as administrator.

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note

You must use the Administrator account on the SUT. If the Administrator account is disabled, you can enable it as follows: 

* In **Control Panel**, open **Administrative Tools** and then open **Computer Management**.

* In the left panel, open **Local Users and** **Groups** under **System Tools,** and then select **Users**.

* In the right panel, double click **Administrator** and then uncheck the **Account is disabled** box.

Start Remote Desktop Services

* In **Control Panel**, open **System**.

* Click **Remote Settings** on the left. Then you can see **Remote** tab on **System Properties** dialog.

* Select **Allow remote connections to this computer**, and uncheck the check box before **Allow connections only from computers running Remote Desktop with Network Level Authentication (recommend)**.

* Press **Ok** to close **System Properties**.


![image7.png](./image/RDP_ServerUserGuide/image7.png)

Configure Network detection on RDP Server.

* Start **Command Prompt**, type **gpedit.msc** and press **Enter**.

* On the **Local Group Policy Editor**, navigate to **Local Computer Policy\Computer Configuration\Administrative Templates\Windows Components\Remote Desktop Services\Remote Desktop Session Host\Connections**.

* Double click **Select network detection on the server**, on the poped up dialog, click **Enabled** and select **Use both Connect Time Detect and Continuous Network Detect**.


![image8.png](./image/RDP_ServerUserGuide/image8.png)

To configure the driver computer

* Log on to the driver computer as Administrator. 

* Follow guide in [Configure the Test Suite](#_Toc396908243) to configure the test suite.

### <a name="_Toc396908242"/>Configuring Computers that are Not Based on Windows

This guide provides only basic information about configuring the test environment for computers that are not running Windows-based operating systems. 

* For domain environments, join all computers to the domain of the domain controller.

* Disable active firewalls on all computers.

For detailed instructions about how to complete the tasks that this process requires, see the administration guide for your operating system. 

To configure the SUT

* Install and enable the server implementations of the protocols to be tested.

To configure the DC 

* Install directory domain services.

### <a name="_Toc396908243"/>Configuring the Test Suite

This test suite is installed with default configuration settings. You may need to change these settings if you use a customized test environment or if you customize your test runs. 

You can define various options for the test suite, such as the following:

* Define the settings of the test environment, including computer names and IP addresses.

* Define the basic options used in the test suite, for example, the protocol version or the version of the target operating system.

* Define the folders and formats used for output from test runs.

* Define scripts to run before and after each test run.

* Set time limits on discrete test tasks and for test runs.

To change configuration settings, edit the **RDP_ServerTestSuite.deployment.ptfconfig** file. You can find this file in the directory C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ _&#60; version &#35;  &#62;_ \Bin.
The following table describes the properties for all the test cases.

| &#32;| &#32; |
| -------------| ------------- |
|  **Property**|  **Description**| 
|  **RDP.Security.Negotiation**| Indicates if the test suite uses the “Negotiation Approach” to select the security mechanism.| 
| | The possible values are **True** and **False.** If the value is set to **False**, it indicates that the “Direct Approach” will be used.| 
| | Default value: **True**| 
|  **RDP.Security.Protocol**| The security protocol used to secure the RDP session. The possible values are **TLS**, **CredSSP** and **RDP**.| 
| | Default value: **TLS**| 
|  **RDP.ServerDomain**| Domain name of the SUT, only used in a domain environment.| 
| | Default value: **Empty string**| 
|  **RDP.ServerName**| The computer name or IP of the SUT.| 
| | Default value: **SUT01**| 
|  **RDP.ServerPort**| Listening port on SUT for Remote Desktop Services.| 
| | Default value: **3389**| 
|  **RDP.ServerUserName**| The username on SUT, which is used to logon SUT by using RDP.| 
| | Default value: **Administrator**| 
|  **RDP.ServerUserPassword**| The password of the user with **RDP.ServerUserName** on SUT.| 
| | Default value: **Password01!**| 
|  **RDP.ClientName**| The computer name or IP of the driver comptuer.| 
| | Default value: **DriverComputer**| 
|  **VerifyRdpbcgrMessages**| Indicates whether the test suite verify the RDPBCGR messages received**.**| 
| | Default value: **True**!| 
|  **VerifyShouldBehaviors**| Indicates whether the test suite verify the SHOULD behaviors in TD. | 
| | Default value: **True**| 
|  **WaitTime**| Indicates the seconds of timeout for test suite expects a message. | 
| | Default value: **20**| 

## <a name="_Toc396908244"/>Running Test Cases

This test suite includes command files that you can use to complete some basic test cases. Each test case verifies the protocol implementation based on a given scenario. 

You can find and run all test cases in the following directories: 
 C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ _&#60; version &#35;  &#62;_ \Batch

You can run these command files at the command prompt or via the graphical user interface.

### <a name="_Toc396908245"/>Run All Test Cases

Use the steps below to run all test cases.

To run all test cases

* From the desktop of the driver computer, double-click the **Run All Test Cases** shortcut.
Alternatively, go to C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ _&#60; version &#35;  &#62;_ \Batch, and double-click the **RunAllTestCases.cmd** file. 

### <a name="_Toc396908246"/>Check Test Results
Test suite generates test result files in different paths based on the way how test case is executed.
For running test case with batch: _C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ &#60; version &#35;  &#62; \Batch\TestResults_
For running test case with Visual Studio: _C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ &#60; version &#35;  &#62; \Source\Server\TestCode\TestResults_
For further information about test log settings, see the PTF User Guide in the PTF installation directory.

## <a name="_Toc396908247"/>Debugging Test Cases

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 
You can get test suite source code from github [https://github.com/Microsoft/WindowsProtocolTestSuites](https://github.com/Microsoft/WindowsProtocolTestSuites)

You can use the Visual Studio solution (.sln) file included with this test suite to debug additional test cases that you create for your protocol implementation. 

![image2.png](./image/RDP_ServerUserGuide/image2.png)
Note 

Copy _RDP_ServerTestSuite.deployment.ptfconfig_ and _RDP_ServerTestSuite.ptfconfig_ from C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ &#60; version &#35;  &#62; \Bin to C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ &#60; version &#35;  &#62; \Source\Server\TestCode\TestSuite and replace the original file.

To debug a test case

* On the driver computer, use Visual Studio to open the following solution file:
C:\MicrosoftProtocolTests\RDP\Server-Endpoint\ _&#60; version &#35;  &#62;_ \Source\Server\TestCode\RDP_Server.sln

* In the **Solution Explorer** window, right-click the **Solution** ‘**RDP_Server’**, and select **Build Solution**.

* Open the **Test Explorer** window in Visual Studio, select the names of the test cases that you want to debug.

## <a name="_Toc396908248"/>Troubleshooting

This section describes how to troubleshoot common test suite issues.

### <a name="_Toc396908249"/>Ping Failure

| &#32;| &#32; |
| -------------| ------------- |
| PROBLEM| The SUT does not respond to pings from the driver computer.| 
| CAUSE| The driver computer was not in the same network segment as the SUT, or the SUT firewall is enabled. | 
| RESOLUTION| Move the driver computer and the SUT to the same network segment or disable the SUT firewall.| 

