# File Server Protocol Family Test Suite User Guide                                     

##Contents

* [Introduction](#1)
* [Quick Start Checklist](#2)
* [Requirements](#3)
    * [Network Infrastructure](#3.1)
    * [Environment](#3.2)
    * [Driver Computer](#3.3)
    * [System Under Test (SUT)](#3.4)
    * [Domain Controller (DC)](#3.5)
	* [SAN Storage Server](#3.6)
    * [Software](#3.7)
* [Network Setup](#4)
    * [Workgroup Environment](#4.1)
    * [Domain Environment](#4.2)
    * [Verify Connectivity from the Driver Computer](#4.3)
* [Computer Setup](#5)    
    * [Workgroup Environment](#5.1)
		* [Set up the Driver Computer](#5.1.1)
		* [Set up the SUT](#5.1.2)
			* [Common Setup](#5.1.2.1)
			* [Setup to test SMB2](#5.1.2.2)
				* [Create a share SMBBasic](#5.1.2.2.1)
				* [Create a share with Encrypt data enabled](#5.1.2.2.2)
				* [Other configuration](#5.1.2.2.3)
					* [Create a share with Oplock Force Level 2 enabled](#5.1.2.2.3.1)
					* [Create a share SameWithSMBBasic](#5.1.2.2.3.2)
					* [Create a share different from SMBBasic](#5.1.2.2.3.3)
					* [Create Symbolic Links](#5.1.2.2.3.4)
					* [Create a share on volume supporting integrity](#5.1.2.2.3.5)
					* [Modify the Signing configurations](#5.1.2.2.3.6)
			* [Setup to test DFSC](#5.1.2.3)
			* [Setup to test RSVD](#5.1.2.4)
			* [Setup to test SQOS](#5.1.2.5)
    * [Domain Environment](#5.2)
		* [Set up the Domain Controller (DC)](#5.2.1)
			* [Configure IP addresses and set the computer name to "DC01"](#5.2.1.1)
			* [Promote DC](#5.2.1.2)
			* [Create Domain User accounts ](#5.2.1.3)
			* [Setup to test Claimed Based Access Control (CBAC)](#5.2.1.4)
		* [Set up the Driver Computer](#5.2.2)
		* [Set up the SUT](#5.2.3)
			* [Common Set up ](#5.2.3.1)
			* [Setup to test SMB2](#5.2.3.2)
			* [Setup to test DFSC](#5.2.3.3)
			* [Setup to test Server Failover/FSRVP/SWN](#5.2.3.4)
			* [Setup to test RSVD](#5.2.3.5)
			* [Setup to test SQOS](#5.2.3.6)
			* [Setup to test Authorization](#5.2.3.7)
	* [Computer Setup for Windows platform](#5.3)
		* [Create share SMBBasic](#5.3.1)
		* [Set NTFS Permissions](#5.3.2)
		* [Set Share Permissions](#5.3.3)
		* [Create share SameWithSMBBasic ](#5.3.4)
		* [Create symbolic link](#5.3.5)
		* [Create share with Encrypt data enabled](#5.3.6)
		* [Enable FORCE_LEVELII_OPLOCK on share](#5.3.7)
		* [Create share on volume supporting integrity](#5.3.8)
		* [Set up a Windows-based DC](#5.3.9)
			* [Install DNS Service and Active Directory Domain Services with a specific domain](#5.3.9.1)
			* [Turn off windows firewall from Control Panel](#5.3.9.2)
			* [Install Remote Access role and configure Routing and Remote Access](#5.3.9.3)
			* [Configure and start Routing service](#5.3.9.4)
			* [Create Non-Admin user account ](#5.3.9.5)
			* [Enable Guest user account ](#5.3.9.6)
			* [Create a Domain group](#5.3.9.7)
			* [Create a Domain account that belongs to a Domain group](#5.3.9.8)
			* [Configure Claimed Based Access Control (CBAC)](#5.3.9.9)
				* [Create User Claim](#5.3.9.9.1)
				* [Create Central Access Rules](#5.3.9.9.2)
				* [Create Central Access Policies](#5.3.9.9.3)
				* [Create Users](#5.3.9.9.4)
				* [Apply Central Access Policies](#5.3.9.9.5)
		* [Set up Distributed File System (DFS)](#5.3.10)
			* [Install Roles and Features on node01](#5.3.10.1)
			* [Create share FileShare ](#5.3.10.2)
			* [Create DFS name spaces](#5.3.10.3)
				* [Create Stand-alone namespace: SMBDfs](#5.3.10.3.1)
				* [Create Stand-alone namespace: Standalone](#5.3.10.3.2)
				* [Create Domain-based namespace: DomainBased](#5.3.10.3.3)
			* [Create DFS link for the namespaces](#5.3.10.4)
				* [Add folder SMBDfsLink to namespace SMBDfs](#5.3.10.4.1)
				* [Add two folders to namespace Standalone](#5.3.10.4.2)
				* [Add two folders to DomainBased namespace](#5.3.10.4.3)
		* [Set up SAN Storage Server](#5.3.11)
			* [Configure IP addresses and set the computer name to "Storage01"](#5.3.11.1)
			* [Turn off windows firewall](#5.3.11.2)
			* [Install iSCSI Target](#5.3.11.3)
			* [Create virtual disks for failover cluster nodes](#5.3.11.4)
		* [Set up Cluster](#5.3.12)
			* [Connect to the iSCSI disks provided by the SAN Storage Server from two nodes](#5.3.12.1)
			* [Install Roles and Features on each failover cluster node](#5.3.12.2)
			* [Create failover cluster in either node](#5.3.12.3)
			* [Create file server on failover cluster](#5.3.12.4)
			* [Add Scale-out Share volume](#_Add_Scale-out_Share)
			* [Create file share for cluster](#5.3.12.5)
			* [Create file share with Oplock Force Level 2 enabled for cluster ](#5.3.12.6)
			* [Create file share with Encrypt data enabled for cluster](#5.3.12.7)
		* [Turn off Firewall](#5.3.13)
		* [Create Local Admin User Account](#5.3.14)
		* [Create a Local Non-Admin User Account](#5.3.15)
		* [Enable the Local Guest Account](#5.3.16)
		* [Reset Password to Local Users](#5.3.17)
		* [Create Asymmetric share](#5.3.18)
		* [Create the virtual hard disk file](#5.3.19)
		* [Create the virtual hard disk set file](#5.3.20)
		* [Modify the Signing configuration](#5.3.21)
		* [Manually Set up Computer Password](#5.3.22)
		* [Create an SQOS policy ](#5.3.23)
* [Installed Files and Folders](#6)
* [Configure and Run Test Cases ](#7)
    * [Configure and Run Test Cases Using Protocol Test Manager](#7.1)
    * [Configure Test Suite Manually](#7.2)
		* [Brief introduction to Configuration Settings](#7.2.1)
	* [Run Test Cases by Batch Script](#7.3)
		* [Run the BVT Test](#7.3.1)
		* [Run All Test Cases](#7.3.2)
		* [Check Test Results](#7.3.3)
    * [Check Test Results](#7.3)
* [Debugging Test Cases](#8)

##<a name="1"/>Introduction

This guide provides information about how to install, configure, and run File Sharing Test Suite in its environment. This suite is designed to test implementations of file server protocol family including Server Message Block (SMB) Protocol Version 2 and 3, File Server Remote VSS Protocol and Service Witness Protocol, The Distributed File System (DFS): Namespace Referral Protocol, Remote Shared Virtual Disk Protocol, as specified in the Microsoft document *\[MS-SMB2\], \[MS-FSRVP\] , \[MS-SWN\], \[MS-DFSC\], \[MS-FSA\], \[MS-SQOS\] and \[MS-RSVD\].* This guide provides information about using this test suite on the Microsoft® Windows® operating system and on operating systems that are not Windows based.

This suite tests only the protocol implementation behaviors that are observed on the wire. For detailed information about the design of this test suite, please see *TestDesignSpecification.*.

##<a name="2"/>Quick Start Checklist

The following checklist summarizes the steps you need to complete to get the test suite up and running. The checklist also provides references to documentation that can help you get started.

![](./image/FileServerUserGuide/image3.png)Note

For workgroup environments, omit tasks that are related to the setup and configuration of domain controllers.

| **Check** | **Task**                                                                                 | **Topic**                                                                                                                                                                                                         |
|-----------|------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| □         | Download the test suite for the protocol implementation                                  | For a list of the files that the download package contains, please see [Installed Files and Folders](#6).                                                                                                      |
| □         | Confirm that your test environment and computers meet the requirements of the test suite | For information about the requirements of the test suite, please see [Requirements](#3).                                                                                                                 |
| □         | Set up the Domain Controller (DC)                                                        | See [Set up the Domain Controller (DC)](#5.2.1).                                                                                                                                              |
| □         | Set up the system under test (SUT)                                                       | See [Set up the SUT in Workgroup Environment](#5.1.2) or [Set up the SUT in Domain Environment](#5.2.3) .                                                                                                                                      |
| □         | Set up the driver computer                                                               | See [Set up the Driver Computer](#5.1.1) or [Set up the Driver Computer](#5.2.2).                                                                                                                               |
| □         | Set up the network                                                                       | See [Network Setup](#4).                                                                                                                                                                               |
| □         | Verify the connections from the driver computer to the SUT and other computers           | See [Verify Connectivity from the Driver Computer](#4.3).                                                                                                                                                                  |
| □         | Install the software prerequisites                                                       | For information about software that must be installed on the computers in your test environment before the test suite is installed, please see [Software](#3.7). |
| □         | Configure the test suite settings                                                        | See [Configure and Run Test Cases ](#7).                                                                                                                                                                     |
| □         | Run a BVT test                                                                           | [Run the BVT Test](#7.3.1)                                                                                                                                                                             |
| □         | Run test cases                                                                           | [Run All Test Cases](#7.3.2)                                                                                                                                                                                   |
| □         | Debug my own test cases                                                                  | [Debugging Test Cases](#8)                                                                                                                                                                              |
| □         | Get the results of test runs                                                             | [Check Test Results](#7.3.3)                                                                                                                                                                             |


##<a name="3"/>Requirements 

This section lists the requirements for the test environment used to run this test suite.

###<a name="3.1"/>Network Infrastructure

-   A test network is required to connect the test computer systems.

-   It must consist of an isolated hub or switch.

-   It must not be connected to a production network or used for any other business or personal communications or operations.

-   It must not be connected to the internet.

-   IP addresses must be assigned for the test network.

-   Computer names should be assigned in the test network infrastructure.

-   User credentials used on the system must be dedicated to the test network infrastructure.

-   Network details including computer IP addresses, names and credentials will be saved in log files.

    -   Please refer to the Detailed Logging Support section.

###<a name="3.2"/>Environment

Run this test suite in a workgroup environment that contains the following computers (physical or virtual):

-   A driver computer running Microsoft® Windows 8.1 Enterprise.

-   A computer running either Microsoft® Windows Server 2016 Technical Preview 2, or a computer set up as SUT that is running non-Windows operating system.

Run this test suite in a domain environment that contains the following computers (physical or virtual):

-   A driver computer running Microsoft® Windows 8.1 Enterprise.

-   For scenarios involving cluster, two computers set up as failover cluster for a Windows-based SUT running Microsoft® Windows Server 2016 Technical Preview 2, or computers set up as cluster for SUT that is running non-Windows operating system.

    For scenarios not involving cluster, a computer running Microsoft® Windows Server 2016 Technical Preview 2, or a computer set up as SUT that is running non-Windows operating system. Either failover cluster node could be used as SUT for non-cluster involved scenarios here.

![](./image/FileServerUserGuide/image3.png)Note

For more information about scenarios involving cluster and otherwise, please check FileServerTestDesignSpecification.md section 3 "Test Suite Design".

-   A computer setup as Domain Controller (DC) running Microsoft® Windows Server 2016 Technical Preview 2 or non-Windows operating system.

-   A computer setup as SAN storage server running Microsoft® Windows Server 2016 Technical Preview 2, or device setup as SAN storage that is running non-Windows operating system.

###<a name="3.3"/>Driver Computer 

The minimum requirements for the driver computer are as follows.

 __Requirement__  | __Description__                           
------------------|-------------------------------------------
 Operating system | Microsoft Windows 8.1, Enterprise Edition 
 Memory           | 2 GB RAM                                  
 Disk space       | 60 GB

###<a name="3.4"/>System Under Test (SUT)

The minimum requirements for the SUT are as follows.

| **Requirement**  | **Description**                                                                                                                                                  |
|------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Operating system | For Windows platform: Microsoft Windows Server 2016 Technical Preview 2, Standard Edition, or a SUT implementation that is running non-Windows operating system. |
| Features         | For Windows platform: Failover Clustering |                  
| Role Services    | For Windows platform:|                                     
|                  | File Server           |                                                 
|                  | DFS Namespaces|
|                  | File Server Resource Manager|                            
|                  | File Server VSS Agent Service |     
| Memory           | 2 GB RAM |                                                 
| Disk space       | 60 GB    |                                                                                                                                                       

###<a name="3.5"/>Domain Controller (DC)

The minimum requirements for the DC are as follows.

![](./image/FileServerUserGuide/image3.png)Note

The requirements in this section apply only to the Domain environment.

| **Requirement**  | **Description**                                                                                                                                       |
|------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------|
| Operating system | For Windows platform: Microsoft Windows Server 2016 Technical Preview 2, Standard Edition, or non-Windows operating system running DC implementation. |
| Services         | For Windows platform: Active Directory Domain Services (AD DS)                                                                                        |
| Memory           | 1 GB RAM                                                                                                                                              |
| Disk space       | 60 GB                                                                                                                                                 |

###<a name="3.6"/>SAN Storage Server

The minimum requirements for the SAN storage server

![](./image/FileServerUserGuide/image3.png)Note

The requirements in this section apply only to the Windows-based computers in the test environment.

| **Requirement**  | **Description**                                                                                                                     |
|------------------|-------------------------------------------------------------------------------------------------------------------------------------|
| Operating system | Microsoft Windows Server 2016 Technical Preview 2, Standard Edition, or a SAN storage that is running non-Windows operating system. |
| Services         | File Server |                                                                                                                        
|                  | iSCSI Target Server|         
| Memory           | 1 GB RAM|                                                                                                                            
| Disk space       | 60 GB|                                                                                                                               

###<a name="3.7"/>Software 

All of the following software must be installed on the driver computer. They have some dependencies on each other, it is recommended to install them in the order listed below so that they can work correctly.

* Install Microsoft® Visual Studio® 2012 or Test Agent.

	Execution of test cases requires vstest.console.exe utility which is included in either Visual Studio or Test Agent for Visual Studio. 

	* Visual Studio provides additional debugging and test case authoring ability for you to understand and update the test better.
	
		You can download Visual Studio Ultimate 2012 Trial (30 days) from the website listed below:

		<http://www.microsoft.com/en-us/download/details.aspx?id=30678>                                                              
		You can download Visual Studio 2012 Update 4 or later from                                                                  
		<http://www.microsoft.com/en-us/download/confirmation.aspx?id=39305>
  
	* By installing Visual Studio agent, you could run the cases by Protocol Test Manager or batch scripts without installing Visual Studio. <http://www.microsoft.com/en-us/download/details.aspx?id=38186>  
	
* Install Spec Explorer (build 3.5.3146.0 or later) 
    
	Spec Explorer is a model based testing tool used by test suite to design model and generate test cases. It has some dependencies on [VC++ 9.0 redistributable package](https://www.microsoft.com/en-us/download/details.aspx?id=5582) which should be installed together.    
	
    You can download Spec Explorer from the website listed below: 

    <http://visualstudiogallery.msdn.microsoft.com/271d0904-f178-4ce9-956b-d9bfa4902745/>

* Install [Protocol Test Framework](https://github.com/microsoft/protocoltestframework).

* Install File Server Test Suite.

* Install Protocol Test Manager (optional).

    Protocol Test Manager provides a graphical UI for easily configuring and running Microsoft Windows Protocol Test Suites. Microsoft® Visual Studio® 2012 or Test Agent must be installed before install Protocol Test Manager.  
 
* Install Microsoft Message Analyzer (optional).

    You can download the latest Message Analyzer from the website listed below:                                                                    
    <http://www.microsoft.com/en-us/download/details.aspx?id=44226>    

##<a name="4"/>Network Setup

Run this test suite in a domain environment using either physical or virtual machines. This section describes the test environment using physical machines.

For information about configuring a virtual machine, please see <http://www.microsoft.com/virtualization/en/us/solution-appliance-test.aspx>. The configuration of virtual machines for use with this test suite is out of the scope of this guide.

###<a name="4.1"/>Workgroup Environment

The workgroup environment requires interactions between the following computers and server roles.

-   The driver computer runs the test cases by sending requests over the wire in the form of protocol messages.

-   The SUT runs an implementation of the protocol that is being tested. The SUT responds to the requests that the driver computer sends.

![](./image/FileServerUserGuide/image3.png)Note

The following diagram applies to both Windows based computers and Non-Windows based computers in the test environment.

![](./image/FileServerUserGuide/image4.png)

| Machine Name/Access Point | NIC       | IPv4          | Subnet Mask   |
|---------------------------|-----------|---------------|---------------|
| Client01                  | External1 | 192.168.1.111 | 255.255.255.0 |
|                           | External2 | 192.168.1.112 | 255.255.255.0 |
| Node01                    | External1 | 192.168.1.11  | 255.255.255.0 |
|                           | External2 | 192.168.1.12  | 255.255.255.0 |


###<a name="4.2"/>Domain Environment

The domain environment requires interactions between the following computers and server roles.

-   The driver computer runs the test cases by sending requests over the wire in the form of protocol messages.

-   The SUT runs an implementation of the protocol that is being tested. The SUT responds to the requests that the driver computer sends.

-   The domain controller (DC) responds to security authentication requests (logging in, checking permissions, etc.) within the domain.

-   Any additional computers and server roles are specified to provide functionality that is required to test the protocol implementation.

![](./image/FileServerUserGuide/image3.png)Note

The following diagram applies only to the Windows based computers in the test environment.

For scenarios involving cluster, driver computer in this test suite will simulate a physical machine to contact to Node01 and Node02 which are built as failover cluster for SUT. SAN storage server Storage01 is on an isolated network to provide backend storage for failover cluster.

For non-cluster scenarios, driver computer will simulate a physical machine than contacts either node which is acting as SUT.

The following figure shows the domain environment with recommended network configurations for reference.

![](./image/FileServerUserGuide/image5.png)

![](./image/FileServerUserGuide/image3.png)Note

"Cluster01" is the Failover Cluster; "GeneralFS" is a File Server role provided by the Failover Cluster.

| Machine Name/Access Point | NIC       | IPv4          | Subnet Mask   | Default Gateway | DNS Server    |
|---------------------------|-----------|---------------|---------------|-----------------|---------------|
| DC01                      | External1 | 192.168.1.1   | 255.255.255.0 | &lt;empty&gt;   | 127.0.0.1     |
|                           | External2 | 192.168.2.1   | 255.255.255.0 | &lt;empty&gt;   | 127.0.0.1     |
| Client01                  | External1 | 192.168.1.111 | 255.255.255.0 | 192.168.1.1     | 192.168.1.1   |
|                           | External2 | 192.168.2.111 | 255.255.255.0 | 192.168.2.1     | 192.168.2.1   |
| Node01                    | External1 | 192.168.1.11  | 255.255.255.0 | 192.168.1.1     | 192.168.1.1   |
|                           | External2 | 192.168.2.11  | 255.255.255.0 | 192.168.2.1     | 192.168.2.1   |
|                           | Cluster   | 10.10.0.11    | 255.255.255.0 | &lt;empty&gt;   | &lt;empty&gt; |
|                           | iSCSI     | 192.168.0.11  | 255.255.255.0 | &lt;empty&gt;   | &lt;empty&gt; |
| Node02                    | External1 | 192.168.1.12  | 255.255.255.0 | 192.168.1.1     | 192.168.1.1   |
|                           | External2 | 192.168.2.12  | 255.255.255.0 | 192.168.2.1     | 192.168.2.1   |
|                           | Cluster   | 10.10.0.12    | 255.255.255.0 | &lt;empty&gt;   | &lt;empty&gt; |
|                           | iSCSI     | 192.168.0.12  | 255.255.255.0 | &lt;empty&gt;   | &lt;empty&gt; |
| Storage01                 | iSCSI     | 192.168.0.1   | 255.255.255.0 | &lt;empty&gt;   | &lt;empty&gt; |
| Cluster01                 | External1 | 192.168.1.100 |               |                 |               |
|                           | External2 | 192.168.2.100 |               |                 |               |
| GeneralFS                 | External1 | 192.168.1.200 |               |                 |               |
|                           | External2 | 192.168.2.200 |               |                 |               |

![](./image/FileServerUserGuide/image3.png)Note

	For Non-Windows, Node01, Node02 and Storage01 could be replaced by the implementation-specific SUT.

###<a name="4.3"/>Verify Connectivity from the Driver Computer

After you install the environment, verify the connection from the driver computer to the SUT, and between all other computers in the test environment. You can perform following steps to test the connectivity between two Windows based computers. For further information, please see the administration guide for your operating system.

To check the connection from the driver computer

![](./image/FileServerUserGuide/image3.png)Note

Disable active firewalls in the test environment.

1.  Click the **Start** button, and then click **Run**.

2.  In the **Run** dialog box, type **cmd** and then click **OK**.

3.  At the command prompt, type **ping** followed by the hostname or IP address of the SUT, and then press **Enter**. The following example checks the connection to a SUT named as "NODE01":

	ping NODE01

4.  Repeat these steps until you confirm connectivity between all computers in the test environment.

Do not proceed with the configuration of the test suite until connectivity is confirmed. Any issues with network connectivity must be resolved before you configure the test suite.

##<a name="5"/>Computer Setup 

This section explains how to set up the computers for the test environment.

###<a name="5.1"/>Workgroup Environment

####<a name="5.1.1"/>Set up the Driver Computer

This section describes how to set up the driver computer.

![](./image/FileServerUserGuide/image6.png)Important

Microsoft Visual Studio 2012, Protocol Test Framework, and Spec Explorer must be installed on the driver computer before you run the test suite installer.

To set up the driver computer

1.  Configure IP address and set the computer name to "Client01".

2.  Install the required and optional software described earlier.

3.  Open Windows PowerShell as Administrator privilege, and then type following command:

    *Set-ExecutionPolicy Unrestricted*

    Execute the same command on Windows PowerShell (x86)

4.  Copy MSI package and install it on the driver computer.

5.  When options are presented, select the option, **Install Test Suite on Driver Computer**.

6.  Create a folder named **FileServerCaptureFileDirectory** under **C:\\**

    It’s used to save the capture files automatically captured by Microsoft Message Analyzer.

####<a name="5.1.2"/>Set up the SUT

#####<a name="5.1.2.1"/>Common Setup

1.  Configure IP addresses on the SUT node and set the computer name to **Node01**.

2.  Turn off firewall.

    For Windows platform, please refer to [Turn off Firewall](#5.3.13)

3.  Create local user accounts:

    - Admin User

        For Windows platform, please refer to [Create Local Admin User Account](#5.3.14)

    - Non-Admin User

        For Windows platform, please refer to [Create a Local Non-Admin User Account](#5.3.15)

4.  Enable Guest account

    For Windows platform, please refer to [Enable the Local Guest Account](#5.3.16)

5.  The password for all accounts is set to "**Password01!**"

    For Windows platform, please refer to [Reset Password to Local Users](#5.3.17)

#####<a name="5.1.2.2"/> Setup to test SMB2

######<a name="5.1.2.2.1"/> Create a share SMBBasic 

It’s basic file share to test SMB2 cases.

1.  Create a share **SMBBasic** on node01.

2.  Share with read/write access to Admin User.

For Windows platform, please refer to [Create share SMBBasic](#5.3.1).

######<a name="5.1.2.2.2"/> Create a share with Encrypt data enabled

It’s used to test Encryption feature of SMB2 protocol.

1.  Create share **SMBEncrypted** on node01.

2.  Share with read/write access to Admin User.

3.  Enable share folder with **Encrypt data** which indicates that the server requires messages to be encrypted for accessing this share.

For Windows platform, please refer to [Create share with Encrypt data enabled](#5.3.6).

######<a name="5.1.2.2.3"/> Other configuration

#######<a name="5.1.2.2.3.1"/> Create a share with Oplock Force Level 2 enabled

It’s used in some cases when testing **Oplock** feature of SMB2 protocol.

1.  Create a share **ShareForceLevel2** on node01.

2.  Share with read/write access to Admin User.

3.  Enable **FORCE\_LEVELII\_OPLOCK** on this share.

For Windows platform, please refer to [Enable FORCE_LEVELII_OPLOCK on share](#5.3.7).

#######<a name="5.1.2.2.3.2"/> Create a share SameWithSMBBasic

It’s used for testing some negative cases of **AppInstanceId** feature of SMB2. If **AppInstanceId** is not supported, this section can be ignored.

Add a share named **SameWithSMBBasic**, this share should have the same local path as **SMBBasic** share.

Summary of the negative cases:

1.  The case creates an open under the share **SMBBasic**. There’s an **AppInstanceId** in the Create Request.

2.  The case connects the share **SameWithSMBBasic** and sends a second Create Request to SUT with the same **AppInstanceId**.

3.  Verify the first open is not force to close by the second create request because the second create request connects to a different share.

For Windows platform, please refer to [Create share SameWithSMBBasic ](#5.3.4).

#######<a name="5.1.2.2.3.3"/> Create a share different from SMBBasic

It’s used for some negative cases (similar to [Create a share SameWithSMBBasic](#5.1.2.2.3.2)) when testing **AppInstanceId** feature of SMB2. If **AppInstanceId** is not supported, this section can be ignored.

1.  Create a share named **DifferentFromSMBBasic** on node01. The local path of this share should be different from **SMBBasic** share.

2.  Share with read/write access to Admin User.

#######<a name="5.1.2.2.3.4"/> Create Symbolic Links

It’s used for some negative cases when testing **Create/Close** feature of SMB2. For details, please see section 3.3.5.9 in \[MS-SMB2\].

1.  Add a **Symbolic Link** to **SMBBasic**

	In share **SMBBasic**, add a symbolic link named **Symboliclink**, which links to **SMBBasic**.

2.  Add a new folder **Sub** under **SMBBasic**, add another symbolic link named **Symboliclink2** under Sub folder, and link it to **SMBBasic**.

For Windows platform, please refer to [Create symbolic link](#5.3.5).

#######<a name="5.1.2.2.3.5"/> Create a share on volume supporting integrity

If integrity is not supported by file system, this section can be ignored.

The share is used to test IoCtl codes **FSCTL\_GET\_INTEGRITY\_INFORMATION** and **FSCTL\_SET\_INTEGRITY\_INFORMATION**.

1.  Create a share named **SMBReFSShare** on node01.

	The file system of the volume containing the specified share should support the use of **integrity**.

2.  Share with read/write access to Admin User.

For Windows platform, please refer to [Create share on volume supporting integrity](#5.3.8).

![](./image/FileServerUserGuide/image3.png)Note

Details about the two IoCtl codes can be found in \[MS-FSCC\].

#######<a name="5.1.2.2.3.6"/> Modify the Signing configurations

If Signing is not supported by file system, this section can be ignored.

The configuration is used to test the Signing function.

In order to run Signing cases, you need to do:

1.  Open the ptfconfig file whose path is as follow:

	WindowsProtocolTestSuites\\TestSuites\\FileServer\\src\\Common\\TestSuite\\CommonTestSuite.deployment.ptfconfig

2.  If SigningRequired is enabled on SUT, set the value of property "IsRequireMessageSigning" to true in the ptfconfig file mentioned above.

3.  If SigningRequired is disabled on SUT, set the value of property "IsRequireMessageSigning" to false in the ptfconfig file mentioned above.

For Windows platform on how to enable or disable SigningRequired, please refer to [Modify the Signing configuration](#5.3.21).

#####<a name="5.1.2.3"/> Setup to test DFSC

If DFS is not supported, this section can be ignored.

1.  Create DFS namespaces.

    Two Stand-alone namespaces: **SMBDfs** and **Standalone**

2.  Create a share **FileShare** on SUT.

    Share with read/write access to Admin User.

3.  Add one folder to namespace **SMBDfs.**

    Folder name is **SMBDfsLink**, link target is **\\\\node01\\FileShare**

4.  Add two folders to namespace **Standalone.**

    One is **DFSLink**, link target is **\\\\node01\\FileShare**

    The other is **Interlink**, link target is **\\\\node01\\SMBDfs\\SMBDfsLink**

For Windows platform, please refer to [Set up Distributed File System (DFS)](#5.3.10).

#####<a name="5.1.2.4"/> Setup to test RSVD

If \[MS-RSVD\] is not implemented, this section can be ignored.

Create two virtual hard disk files named **rsvd.vhdx** and **convert.vhdx**, as well as a virtual hard disk set file named **rsvd.vhds,** and put them in a share of SUT.

Testing RSVD is not applicable for workgroup environment of Windows platform.

#####<a name="5.1.2.5"/> Setup to test SQOS

If \[MS-SQOS\] is not implemented, this section can be ignored.

1.  Create a virtual hard disk file named **sqos.vhdx** and put it in a share of SUT.

2.  Create a new policy (with MinimumIoRate: 100 and MaximumIoRate: 200).

Save the policy id to the ptfconfig item: SqosPolicyId

Testing SQOS is not applicable for workgroup environment of Windows platform.

###<a name="5.2"/>Domain Environment
  
####<a name="5.2.1"/>Set up the Domain Controller (DC)

#####<a name="5.2.1.1"/> Configure IP addresses and set the computer name to "DC01"

#####<a name="5.2.1.2"/> Promote DC

Promote the computer to be a Domain Controller.

For Windows platform, please refer to [Install DNS Service and Active Directory Domain Services with a specific domain](#5.3.9.1).

#####<a name="5.2.1.3"/> Create Domain User accounts 

1.  Create a domain admin user account in Active Directory

2.  Create a domain user account named as **nonadmin** in Active Directory

    Make sure this account is not the admin account of SUT.

3.  Enable Guest account

4.  Create a domain group AzGroup01 in Active Directory

5.  Create a domain user AzUser01 within AzGroup01 in Active Directory

6.  Password of all the accounts is set to "**Password01!**"

For Windows platform, please refer to [Create Non-Admin user account](#5.3.9.5), [Enable Guest user account](#5.3.9.6), [Create a Domain group](#5.3.9.7) and [Create a Domain account that belongs to a Domain group](#5.3.9.8).

#####<a name="5.2.1.4"/> Setup to test Claimed Based Access Control (CBAC)

For Windows platform, please refer to [Configure Claimed Based Access Control (CBAC)](#5.3.9.9).

####<a name="5.2.2"/> Set up the Driver Computer

1.  Complete steps specified in [Set up the Driver Computer](#5.1.1).

2.  Join the Driver Computer to the domain provided by the DC.

####<a name="5.2.3"/> Set up the SUT

#####<a name="5.2.3.1"/> Common Set up 

1.  Complete steps specified in [Common Setup](#5.1.2.1)

2.  Join the SUT to the domain provided by the DC.

3.  Setup the computer password for SUT.

For Windows platform, please refer to [Manually Set up Computer Password](#5.3.22).

#####<a name="5.2.3.2"/> Setup to test SMB2

1.  Complete steps specified in [Setup to test SMB2](#5.1.2.2)

2.  Share all the shares with Domain Admin User.

#####<a name="5.2.3.3"/> Setup to test DFSC

If DFS is not supported, this section can be ignored.

1.  Complete steps specified in section [Setup to test DFSC](#5.1.2.3)

2.  Create one Domain-based namespace: **DomainBased**

3.  Add two folders to namespace **DomainBased**

	One is **DFSLink.** If there is node02, link target is **\\\\node02\\FileShare**, and otherwise link target is **\\\\node01\\FileShare**

	The other is **Interlink**, link target is **\\\\node01\\SMBDfs\\SMBDfsLink**

For Windows platform, please refer to [Set up Distributed File System (DFS)](#5.3.10).

#####<a name="5.2.3.4"/> Setup to test Server Failover/FSRVP/SWN

1.  Create a share named **SMBClustered**, set the property **Share.IsCA** to TRUE, set **Share.Type** to include **STYPE\_CLUSTER\_SOFS**, and make the share asymmetric.

    - **Share.IsCA** is set to TRUE when **SHI1005\_FLAGS\_ENABLE\_CA** bit in **shi1005\_flags** is set as documented in \[MS-SRVS\] section 2.2.4.29.

    - **STYPE\_CLUSTER\_SOFS** is one of "**Share Types**" documented in \[MS-SRVS\] section 2.2.2.4.

2.  Create a share named **SMBClusteredEncrypted** with the properties **Share.IsCA** and **Share.EncryptData** set to TRUE.

3.  Create a share named **SMBClusteredForceLevel2**, set the property **Share.ForceLevel2Oplock** to TRUE and set **Share.Type** to include **STYPE\_CLUSTER\_SOFS**.
 
	- **Share.ForceLevel2Oplock** is set to TRUE when **SHI1005\_FLAGS\_FORCE\_LEVELII\_OPLOCK** bit in **shi1005\_flags** is set as specified in \[MS-SRVS\] section 2.2.4.29.

	- **STYPE\_CLUSTER\_SOFS** is one of "**Share Types**" documented in \[MS-SRVS\] section 2.2.2.4.

For Windows platform, please refer to [Set up SAN Storage Server](#5.3.11) and [Set up Cluster ](#5.3.12).

#####<a name="5.2.3.5"/> Setup to test RSVD

If \[MS-RSVD\] is not implemented, this section can be ignored.

Create two virtual hard disk files named **rsvd.vhdx** and **convert.vhdx**, as well as a virtual hard disk set file named **rsvd.vhds**, put them in a share of SUT.

For Windows platform,

1.  About how to create a virtual hard disk file, please refer to [Create the virtual hard disk file](#5.3.19).

2.  About how to create a virtual hard disk set file, please refer to [Create the virtual hard disk set file](#5.3.20).

3.  Put **rsvd.vhdx**, **convert.vhdx** and **rsvd.vhds** in the share **\\\\scaleoutfs\\SMBClustered**

#####<a name="5.2.3.6"/> Setup to test SQOS

If \[MS-SQOS\] is not implemented, this section can be ignored.

1.  Create a virtual hard disk file named **sqos.vhdx** and put it in a share of SUT.

2.  Create a new policy (with MinimumIoRate: 100 and MaximumIoRate: 200)

	Save the policy id to the ptfconfig item: SqosPolicyId

For Windows platform,

1.  About how to create a virtual hard disk file, please refer to [Create the virtual hard disk file](#5.3.19).

2.  Put **sqos.vhdx** in the share [**\\\\scaleoutfs\\SMBClustered**](file:///\\scaleoutfs\SMBClustered)

3.  About how to create the new policy, please refer to section [Create an SQOS policy ](#5.3.23)

#####<a name="5.2.3.7"/> Setup to test Authorization

1.  Create a share named **AzShare**.

	| NTFS Permission  | Allow Everyone      |
	|------------------|---------------------|
	| Share Permission | Allow Domain Admins |

2.  Create a share named **AzFolder**.

	| NTFS Permission  | Allow Domain Admins |
	|------------------|---------------------|
	| Share Permission | Allow Everyone      |

3.  Create a share named **AzFile**.

	| NTFS Permission  | Allow Domain Admins |
	|------------------|---------------------|
	| Share Permission | Allow Everyone      |

4.  Create a share named **AzCBAC**.

	| NTFS Permission  | Allow Everyone |
	|------------------|----------------|
	| Share Permission | Allow Everyone |

	If **SRVS** (Server Service Remote Protocol) is not supported, please create 6 shares: **AzShare01**... **AzShare06.** Share permissions should be set according to the following table:

	| AzShare01 | O:SYG:SYD:(A;;0x1fffff;;;\[SID of AzUser01\])(A;;FA;;;BA)   |
	|-----------|-------------------------------------------------------------|
	| AzShare02 | O:SYG:SYD:(A;;0x1fffff;;; \[SID of AzGroup01\])(A;;FA;;;BA) |
	| AzShare03 | O:SYG:SYD:(D;;0x1fffff;;; \[SID of AzUser01\])(A;;FA;;;BA)  |
	| AzShare04 | O:SYG:SYD:(D;;0x1fffff;;; \[SID of AzGroup01\])(A;;FA;;;BA) |
	| AzShare05 | O:SYG:SYD:(A;;FA;;;BA)                                      |
	| AzShare06 | O:SYG:SYD:(D;;;;; \[SID of AzUser01\])(A;;FA;;;BA)          |

Steps to create the above shares are similar to creating share **SMBBasic**.

For Windows platform on how to create **SMBBasic** and configure permissions, please refer to [Create share SMBBasic](#5.3.1), [Set NTFS Permissions](#5.3.2) and [Set Share Permissions](#5.3.3).

###<a name="5.3"/> Computer Setup for Windows platform

####<a name="5.3.1"/>Create share SMBBasic

Create a folder **C:\\SMBBasic**.

Share it with name **SMBBasic** and read/write access to **node01\\administrator** (and **contoso\\administrator** if Domain Environment).

![](./image/FileServerUserGuide/image7.png)

####<a name="5.3.2"/>Set NTFS Permissions

![](./image/FileServerUserGuide/image8.png)

####<a name="5.3.3"/>Set Share Permissions

![](./image/FileServerUserGuide/image9.png)

####<a name="5.3.4"/>Create share SameWithSMBBasic 

Add another share name **SameWithSMBBasic** to **C:\\SMBBasic** folder

![](./image/FileServerUserGuide/image10.png)

####<a name="5.3.5"/>Create symbolic link

Start console window as administrator privilege:

Click **Start**-&gt;Type **cmd**-&gt;Right click **Command Prompt**-&gt;select **Run as Administrator**

![](./image/FileServerUserGuide/image11.png)

Run the following commands in console window:

1.  cd c:\\SMBBasic

2.  mklink /D Symboliclink c:\\

3.  mkdir Sub

4.  cd Sub

5.  mklink /D Symboliclink2 c:\\SMBBasic

####<a name="5.3.6"/>Create share with Encrypt data enabled

1.  Create a folder **C:\\SMBEncrypted** on node01.

2.  Share with read/write access to **node01\\administrator** (and **contoso\\administrator** if Domain Environment).

3.  Enable share folder with **Encrypt data**.

Navigate to **Server Manager** &gt; **File and Storage Services** &gt; **Share**&gt;

**SMBEncrypted** &gt; **Properties** &gt; **Settings** &gt; **Encrypt data**

![](./image/FileServerUserGuide/image12.png)

####<a name="5.3.7"/>Enable FORCE\_LEVELII\_OPLOCK on share

Please modify and run below two commands from console window:

	cd C:\MicrosoftProtocolTests\FileServer\Server-Endpoint\<version#>\Bin\
	ShareUtil.exe <computer name of the share> <share name> <flag > true

Examples:

1.  To set SHI1005\_FLAGS\_FORCE\_LEVELII\_OPLOCK flag on **\\\\node01\\ShareForceLevel2**, run the following command:
    
    *ShareUtil.exe node01 ShareForceLevel2 SHI1005\_FLAGS\_FORCE\_LEVELII\_OPLOCK true*

2.  To set SHI1005\_FLAGS\_FORCE\_LEVELII\_OPLOCK flag on **\\\\ScaleOutFS\\ SMBClusteredForceLevel2**, run the following command:

    *ShareUtil.exe ScaleOutFS SMBClusteredForceLevel2 SHI1005\_FLAGS\_FORCE\_LEVELII\_OPLOCK true*

####<a name="5.3.8"/>Create share on volume supporting integrity

Execute "diskmgmt.msc" from console window, create **ReFS** format volume.

![](./image/FileServerUserGuide/image13.png)

Right-click unallocated disk to create new simple volume. Or right-click on **Disk 0** and select "**Shrink volume**" to split a free space (5G) at first.

![](./image/FileServerUserGuide/image14.png)

Format new created volume as ReFS fie system.

Create a share in the new volume with share name: **SMBReFSShare** and share it with read/write access with **node01\\administrator** and **contoso\\administrator** (If Domain Environment).

![](./image/FileServerUserGuide/image15.png)

####<a name="5.3.9"/>Set up a Windows-based DC

#####<a name="5.3.9.1"/> Install DNS Service and Active Directory Domain Services with a specific domain

Install Active Directory domain services, and create domain (like contoso.com), set domain Administrator’s password to "**Password01!**"

#####<a name="5.3.9.2"/>Turn off windows firewall from Control Panel

Please refer to [Turn off Firewall](#5.3.13)

#####<a name="5.3.9.3"/> Install Remote Access role and configure Routing and Remote Access

From Server Manager click Add Roles and Features and add Remote Access to Server Roles.

![](./image/FileServerUserGuide/image3.png) Note

This step is for routing of two different networks which are used in multiple channel cases

![](./image/FileServerUserGuide/image16.png)

Select **Routing** from **Role Services**

![](./image/FileServerUserGuide/image17.png)

Submit Confirmation

![](./image/FileServerUserGuide/image18.png)

#####<a name="5.3.9.4"/> Configure and start Routing service

From **Server Manager** click **Tools,** select **Routing and Remote Access**.

![](./image/FileServerUserGuide/image19.png)

Right-click DC01, select **Configure and Enable Routing and Remote Access**.

![](./image/FileServerUserGuide/image20.png)

![](./image/FileServerUserGuide/image21.png)

![](./image/FileServerUserGuide/image22.png)

And then start the service.

#####<a name="5.3.9.5"/> Create Non-Admin user account 

Run the following commands in console window as administrator privilege:

	dsadd user "CN=nonadmin,CN=Users,DC=contoso,DC=com"  -pwd Password01! -desc contoso -disabled no -mustchpwd no -pwdneverexpires yes

#####<a name="5.3.9.6"/> Enable Guest user account 

Launch **Active Directory Users and Computers** form **Server Manager**.

Right Click **Guest**, Click **Enable Account** in the popup menu.

![](./image/FileServerUserGuide/image23.png)

Right Click **Guest**, Click Reset **Password…** and set the password to "**Password01!**"

![](./image/FileServerUserGuide/image24.png)

#####<a name="5.3.9.7"/> Create a Domain group

Run the following commands in console window as administrator:

	dsadd group "CN=AzGroup01,CN=Users,DC=contoso,DC=com" 

#####<a name="5.3.9.8"/> Create a Domain account that belongs to a Domain group

Run the following commands in console window as administrator:

	dsadd user "CN=AzUser01,CN=Users,DC=contoso,DC=com"  -pwd Password01! -desc contoso -memberof "CN=AzGroup01,CN=Users,DC=contoso,DC=com" -disabled no -mustchpwd no -pwdneverexpires yes

#####<a name="5.3.9.9"/> Configure Claimed Based Access Control (CBAC)

######<a name="5.3.9.9.1"/> Create User Claim

Navigate to **Server Manager** &gt; **Tools** &gt; **Active Directory Administrative Center**

![](./image/FileServerUserGuide/image25.png)

Then **Dynamic Access Control** &gt; **Claim Types**

![](./image/FileServerUserGuide/image26.png)

In the right panel, select **New** &gt; **Claim Type**

![](./image/FileServerUserGuide/image27.png)

Input **department** in the filter, and then input **Department** as display name, and then add value **IT** and **Payroll** as suggested values.

![](./image/FileServerUserGuide/image28.png)

Similarly, add another claim type: **CountryCode**, "Suggested Values" set to **156, 840** and **392**.

![](./image/FileServerUserGuide/image29.png)

######<a name="5.3.9.9.2"/> Create Central Access Rules

Navigate to **Server Manager** &gt; **Tools** &gt;**Active Directory Administrative Center** &gt; **Dynamic Access Control** &gt; **Central Access Rules**.

In the right panel, select **New** &gt; **Central Access Rule**

![](./image/FileServerUserGuide/image30.png)

Enter name: **CountryCodeEquals156Rule**

![](./image/FileServerUserGuide/image31.png)

In the **Permission** tab, click **Edit**

![](./image/FileServerUserGuide/image32.png)

Click **Add**

![](./image/FileServerUserGuide/image33.png)

Click **Select a principal**

![](./image/FileServerUserGuide/image34.png)

Input **Authenticated Users**, then click **Check Names** and **OK**

![](./image/FileServerUserGuide/image35.png)

Check **Full Control** for **Basic permissions**.

![](./image/FileServerUserGuide/image36.png)Click **Add a condition**, and select **CountryCode** **Equals** **156** and click **OK**

Back to **Create Central Access Rule** page, and click **OK.**

![](./image/FileServerUserGuide/image37.png)

Repeat above procedure to create the rest 9 rules

![](./image/FileServerUserGuide/image38.png)

Conditions of the 9 rules:

**CountryCodeAnyOf156Or840Rule**: User.CountryCode Any of {156,840}

**CountryCodeEquals156AndITDepartmentRule**: (User.CountryCode Equals 156) And (User.Department Equals "IT")

**CountryCodeEquals156OrITDepartmentRule**: (User.CountryCode Equals 156) Or (User.Department Equals "IT")

**CountryCodeGreaterThan392Rule**: User.CountryCode Greater than 392

**CountryCodeGreaterThanOrEquals392Rule**: User.CountryCode Greater than or equal to 392

**CountryCodeLessThan392Rule**: User.CountryCode Less than 392

**CountryCodeLessThanOrEquals392Rule**: User.CountryCode Less than or equal to 392

**CountryCodeNotAnyOf156Or840Rule**: User.CountryCode Not any of {156, 840}

**CountryCodeNotEquals156Rule**: User.CountryCode Not equals 156

######<a name="5.3.9.9.3"/> Create Central Access Policies

Navigate to **Server Manager** &gt; **Tools** &gt; **Active Directory Administrative Center** &gt; **Dynamic Access Control** &gt; **Create Central Access Policies**

In the right panel, select **New** &gt; **Central Access Policy**

![](./image/FileServerUserGuide/image39.png)

In the **Create Central Access Policy** page, input **CountryCodeEquals156Policy** in Name, and click **Add** to select rule **CountryCodeEquals156Rule**, then click **OK**.

![](./image/FileServerUserGuide/image40.png)

![](./image/FileServerUserGuide/image41.png)

Repeat the above procedure and add the rest 9 policies:

![](./image/FileServerUserGuide/image42.png)

######<a name="5.3.9.9.4"/> Create Users

Navigate to **Server Manager** &gt; **Tools** &gt; **ADSI Edit**

![](./image/FileServerUserGuide/image43.png)

In the right panel, right click **ADSI Edit**, and select **Connect to**

![](./image/FileServerUserGuide/image44.png)

Click **OK**

![](./image/FileServerUserGuide/image45.png)

Expand **Default naming context** &gt; **DC=contoso,DC=com**, right click **CN=Users**, select **New** &gt; **Object**

![](./image/FileServerUserGuide/image46.png)

Select **user** and click **Next** 

![](./image/FileServerUserGuide/image47.png)

Input **ITadmin01** and then **Next**

![](./image/FileServerUserGuide/image48.png)

Input **ITadmin01** and then **Next**

![](./image/FileServerUserGuide/image49.png)

Press **Finish**

![](./image/FileServerUserGuide/image50.png)

Right click the created user CN=ITadmin01, and select Properties

![](./image/FileServerUserGuide/image51.png)

Select **CountryCode**, and input **156** to it, then click **OK**

![](./image/FileServerUserGuide/image52.png)

Select **department** and enter **IT**

![](./image/FileServerUserGuide/image53.png)

Repeat the above procedure to add the 4 users as follows:

**ITmember01,** department: IT, countrycode: 392

**Payrollmember01,** department: Payroll, countrycode: 156

**Payrollmember02,** department: Payroll, countrycode: 840

**Payrollmember03,** department: Payroll, countrycode: 392

######<a name="5.3.9.9.5"/> Apply Central Access Policies

Open **Run** &gt; **gpme.msc**, select **Default Domain Policy** and click **OK**

![](./image/FileServerUserGuide/image54.png)

In the right panel, navigate to **Computer Configuration &gt; Policies &gt; Windows Settings &gt; Security Settings &gt; File System &gt; Central Access Policy**

![](./image/FileServerUserGuide/image55.png)

Right click **Central Access Policy** and select **Manage Central Access Policies…**

![](./image/FileServerUserGuide/image56.png)

Select all the 10 rules added in [Create Central Access Rules](#5.3.9.9.2), click **Add**, and then **OK**

![](./image/FileServerUserGuide/image57.png)

First, update group policy in DC01, then in Node01 by executing the following command with administrator privilege in PowerShell or CMD:

	gpupdate /force

####<a name="5.3.10"/>Set up Distributed File System (DFS)

#####<a name="5.3.10.1"/> Install Roles and Features on node01

In **Server Roles** &gt; **File And Storage Services**, select **DFS Namespaces.**

![](./image/FileServerUserGuide/image58.png)

Click **Add Features** when prompted to install **DFS Management** **Tools**.

![](./image/FileServerUserGuide/image59.png)

Click **Install** in **Confirmation** page to complete the wizard.

![](./image/FileServerUserGuide/image60.png)

#####<a name="5.3.10.2"/> Create share FileShare 

If there is node02 in the environment, create a folder **C:\\FileShare** on node02 and share it with read/write access with **node02\\administrator** and **contoso\\administrator** (If Domain Environment).

Otherwise, create a folder **C:\\FileShare** on node01 and share it with read/write access with **node01\\administrator** and **contoso\\administrator** (If Domain Environment).

#####<a name="5.3.10.3"/> Create DFS name spaces

######<a name="5.3.10.3.1"/> Create Stand-alone namespace: SMBDfs


1.  Launch **DFS Management** from **Server Manager** on node01

	![](./image/FileServerUserGuide/image61.png)

2.  Right-click **Namespace** and select **New Namespace…**.

	![](./image/FileServerUserGuide/image62.png)

3.  Specify the server that will host the namespace as **node01**.

	![](./image/FileServerUserGuide/image63.png)

4.  Specify the name of namespace **SMBDfs** and click **Edit Settings…** to set access permission to "**Administrators have full access; other users have read-only permissions**".

	![](./image/FileServerUserGuide/image64.png)

	![](./image/FileServerUserGuide/image65.png)

5.  Select Stand-alone namespace.

	![](./image/FileServerUserGuide/image66.png)

	Then click **Create** to complete the wizard.

	![](./image/FileServerUserGuide/image67.png)

######<a name="5.3.10.3.2"/> Create Stand-alone namespace: Standalone

Repeat steps 1 to 5 in [Create Stand-alone namespace: SMBDfs](#5.3.10.3.1), and create another Stand-alone namespace: **Standalone**.

######<a name="5.3.10.3.3"/> Create Domain-based namespace: **DomainBased**

Repeat steps 1 to 4 in [Create Stand-alone namespace: SMBDfs](#5.3.10.3.1), and then choose to create Domain-based namespace: **DomainBased**

![](./image/FileServerUserGuide/image68.png)

Then click **Create** to complete the wizard.

![](./image/FileServerUserGuide/image69.png)

#####<a name="5.3.10.4"/> Create DFS link for the namespaces

######<a name="5.3.10.4.1"/>  Add folder SMBDfsLink to namespace SMBDfs

Right-click the newly created namespace **\\\\NODE01\\SMBDfs** and click **New Folder…**

![](./image/FileServerUserGuide/image70.png)

Specify the folder name **SMBDfsLink** and click **Add…** to add target folder **\\\\NODE01\\FileShare**.

![](./image/FileServerUserGuide/image71.png)

![](./image/FileServerUserGuide/image72.png)

![](./image/FileServerUserGuide/image73.png)

######<a name="5.3.10.4.2"/> Add two folders to namespace Standalone

Right-click the newly created namespace **\\\\NODE01\\Standalone** and click **New Folder**

Add two folders, one is **DFSLink**, target is **\\\\node02\\FileShare**.

If there is no node02 in the environment, set the target to **\\\\node01\\FileShare**.

![](./image/FileServerUserGuide/image74.png)

![](./image/FileServerUserGuide/image75.png)

The other is **Interlink**, target is **\\\\node01\\SMBDfs\\SMBDfsLink**

![](./image/FileServerUserGuide/image76.png)

![](./image/FileServerUserGuide/image77.png)

Two folders under **Standalone** namespace in total.

![](./image/FileServerUserGuide/image78.png)

######<a name="5.3.10.4.3"/> Add two folders to DomainBased namespace

Right-click the namespace **\\\\contoso.com\\DomainBased** and click **New Folder.**

Add two folders.

One folder is **DFSLink**, target is **\\\\node02\\FileShare**.

If no node02 in environment, set the target to **\\\\node01\\FileShare**.

![](./image/FileServerUserGuide/image79.png)

![](./image/FileServerUserGuide/image80.png)

Press OK and add another folder **Interlink**, target is **\\\\node01\\SMBDfs\\SMBDfsLink**

![](./image/FileServerUserGuide/image81.png)

![](./image/FileServerUserGuide/image82.png)

Two folders under **DomainBased** namespace in total.

![](./image/FileServerUserGuide/image83.png)

####<a name="5.3.11"/>Set up SAN Storage Server

This section provides information about how to setup a SAN storage server for use with this test suite.

![](./image/FileServerUserGuide/image3.png)Note

This server may not join the Domain provided by the DC.

#####<a name="5.3.11.1"/> Configure IP addresses and set the computer name to "Storage01"

#####<a name="5.3.11.2"/> Turn off windows firewall from Control Panel

Run the following command in console window:

	netsh advfirewall set allprofiles state off

#####<a name="5.3.11.3"/> Install iSCSI Target

From **Server Manager** click **Add Roles and Features** and add **File Services** from **Server Roles**.

![](./image/FileServerUserGuide/image84.png)

Click **Next** with default settings until **Server Roles** page. Expand tree **File And Storage Services** and select **File Server** and **iSCSI Target Server** to install.

![](./image/FileServerUserGuide/image85.png)

Confirm to add required features for iSCSI Target Server.

![](./image/FileServerUserGuide/image86.png)

Click **Next** with default settings until last **Confirmation** page and click **Install**.

![](./image/FileServerUserGuide/image87.png)

#####<a name="5.3.11.4"/> Create virtual disks for failover cluster nodes.

After installation of the above step, navigate to **Server Manager** &gt; **File and Storage Services** &gt; **iSCSI Virtual Disks** to launch the New Virtual Disk wizard to create a virtual disk.

![](./image/FileServerUserGuide/image88.png)

Either select volume or type a custom path for the new virtual disk location.

![](./image/FileServerUserGuide/image89.png)

Specify the names and sizes of the virtual disks. The following table gives recommended virtual disk number and size used in this test suite.

| **Disk Name** | **Size** | **Purpose**                                                                    |
|---------------|----------|--------------------------------------------------------------------------------|
| Quorumdisk    | 1GB      | Quorum disk for a two-node cluster                                             |
| FSDisk01      | 10GB     | Storage used by file server for general use on failover cluster                |
| FSDisk02      | 10GB     | Storage used by file server for scale-out application data on failover cluster |

![](./image/FileServerUserGuide/image90.png)

![](./image/FileServerUserGuide/image91.png)

Create a new iSCSI target for failover cluster node access.

![](./image/FileServerUserGuide/image92.png)

Specify the name of the iSCSI target name.

![](./image/FileServerUserGuide/image93.png)

Specify the initiators of failover cluster nodes which need to access the virtual disk and target.

![](./image/FileServerUserGuide/image94.png)

When **Add…** is clicked there’re multiple ways for you to give the target access to a specific machine. You could select the IQNs of failover cluster nodes from the list if **Select from the initiator cache on the target server** option is available. Or add the values in specific type of the failover cluster nodes if the **Enter a value for the selected type** option is selected and a type is specified. Here, take this example with **IP Address** type.

![](./image/FileServerUserGuide/image95.png)

![](./image/FileServerUserGuide/image96.png)

Click **Next** with default settings and click **Create** in **Confirmation** page to complete the wizard.

![](./image/FileServerUserGuide/image97.png)

After the 1<sup>st</sup> virtual disk is created, right click from **iSCSI VIRTUAL DISKS** blank space to launch new "**New iSCSI Virtual Disk…**" wizard.

![](./image/FileServerUserGuide/image98.png)

Repeat the above steps to create more virtual disks as shared storages for the failover cluster use. Please note that you must select the existing iSCSI target which was just created in the above steps.

![](./image/FileServerUserGuide/image99.png)

After iSCSI target is created, you could find iSCSI target name as below when select any virtual disk.

![](./image/FileServerUserGuide/image100.png)

####<a name="5.3.12"/>Set up Cluster 

#####<a name="5.3.12.1"/> Connect to the iSCSI disks provided by the SAN Storage Server from two nodes

From **Server Manager** of each failover cluster node, click **iSCSI Initiator** and select **Yes** to start **Microsoft iSCSI service** when prompted for its first use.

![](./image/FileServerUserGuide/image101.png)

![](./image/FileServerUserGuide/image102.png)

Add the IP address of the SAN storage server when discover target portal in **Discovery** tab.

![](./image/FileServerUserGuide/image103.png)

Navigate to **Targets** tab click **Refresh** and **Connect**

![](./image/FileServerUserGuide/image104.png)

On either of the failover cluster nodes, open **Run** &gt; **DISKMGMT.MSC** to launch disk manager, 3 disks can be found if you follow the recommended disk settings. Online and initiate them as GPT or MBR disk, and then create simple volumes with NTFS format on them. Do not forget to format these three disks.

![](./image/FileServerUserGuide/image105.png)

Right click **Disk 1**, the left panel, **Online** this disk and then choose **Initialize Disk**

![](./image/FileServerUserGuide/image106.png)

Select **MBR**, and click **OK**

![](./image/FileServerUserGuide/image107.png)

Then right click **Disk 1** again, the right panel

Choose **New Simple Volume**

![](./image/FileServerUserGuide/image108.png)

Press **Next &gt;**

![](./image/FileServerUserGuide/image109.png)

**Next &gt;**

![](./image/FileServerUserGuide/image110.png)

**Next &gt;**

![](./image/FileServerUserGuide/image111.png)

**Next &gt;**

![](./image/FileServerUserGuide/image112.png)

Press **Finish**.

![](./image/FileServerUserGuide/image113.png)

Repeat the same steps to format the other two disks.

#####<a name="5.3.12.2"/> Install Roles and Features on each failover cluster node

In **Server Roles** &gt; **File And Storage Services**, select **File Server**, **File Server Resource Manager,** **File Server** **VSS Agent Service. **

![](./image/FileServerUserGuide/image114.png)

**In Features,** select **Failover Clustering**.

![](./image/FileServerUserGuide/image115.png)

Click **Add Features** when prompt to install **Failover Clustering Tools**.

![](./image/FileServerUserGuide/image116.png)

Click **Install** in **Confirmation** page to complete the wizard.

#####<a name="5.3.12.3"/> Create failover cluster in either node

Logon on either of the failover cluster nodes using a domain account which has permission to create computer object to Active Directory.

**Server Manager** &gt; **Tools** &gt; **Failover Cluster Manager** to open management console.

Create cluster using "**Create Cluster…**" wizard.

![](./image/FileServerUserGuide/image3.png)Note

Prior to create the cluster, the shared storage needs to be created and added to the failover cluster nodes. For more information about this, please check [Set up SAN Storage Server](#5.3.11)

![](./image/FileServerUserGuide/image117.png)

Input the computer name of failover cluster nodes.

![](./image/FileServerUserGuide/image118.png)

Optionally select if you want do cluster configuration validation tests. For simplicity, we just select "No" to skip such validation tests here.

![](./image/FileServerUserGuide/image119.png)

Specify the cluster name **Cluster01**, and also the IP address of the cluster (e.g. **192.168.1.100**, and **192.168.2.100**) if the cluster nodes are using static IP addresses in a subnet. You could ignore specifying the IP address in the subnet which is used for cluster internal traffic.

![](./image/FileServerUserGuide/image120.png)

![](./image/FileServerUserGuide/image121.png)

![](./image/FileServerUserGuide/image122.png)

#####<a name="5.3.12.4"/> Create file server on failover cluster

In **Failover Cluster Manager** from either failover cluster node, right click on **Roles**, select **Configure Role…**

![](./image/FileServerUserGuide/image123.png)

![](./image/FileServerUserGuide/image124.png)

Select either **File Server for general use** or **File Server for scale-out application data**. Here we select the first one.

![](./image/FileServerUserGuide/image125.png)

Specify the access point name (such as "**GeneralFS**") and IP address if the failover cluster nodes use static IP addresses in a subnet (such as "**192.168.1.200**", "**192.168.2.200**").

![](./image/FileServerUserGuide/image126.png)

Select a disk for the file server consumption.

![](./image/FileServerUserGuide/image127.png)

Click "**Next**" in following pages to complete the wizard.

![](./image/FileServerUserGuide/image128.png)

Repeat above steps when select **File Server for scale-out application data** to create file server for scale-out application data except just specifying client access point name like **ScaleOutFS** as follows.

![](./image/FileServerUserGuide/image129.png)

##### <a name="_Add_Scale-out_Share"/>Add Scale-out Share volume
To create a file share on the file server for scale-out application data, first add an existing free cluster disk (**Assigned To** is **Available Storage**) as **Cluster Shared Volumes** like below.

![image141.png](./image/FileServerUserGuide/image141.png)

#####<a name="5.3.12.5"/> Create file share for cluster

Select **Roles** from **Failover Cluster Manager**, to create a file share on file server for general use, right click one of the file server items with **Type** "**File Server**" (**GeneralFS**) and select **Add Shared Folder**

![](./image/FileServerUserGuide/image130.png)

Select "**SMB Share – Quick**"

![](./image/FileServerUserGuide/image131.png)

![](./image/FileServerUserGuide/image132.png)

Specify the share name, such as "**SMBClustered**".

![](./image/FileServerUserGuide/image133.png)

Check "**Enable continuous availability**" and "**Allow caching of share**" checkboxes.

![](./image/FileServerUserGuide/image134.png)

Give **Full Control** to the account that will access the file share.

![](./image/FileServerUserGuide/image135.png)

![](./image/FileServerUserGuide/image136.png)

![](./image/FileServerUserGuide/image137.png)

![](./image/FileServerUserGuide/image138.png)

![](./image/FileServerUserGuide/image139.png)

![](./image/FileServerUserGuide/image140.png)

Repeat above steps except for setting the share location on the scale-out file server role on **Select the Server and path for this share** page and setting the share name to **SMBClustered** on **Specify share name** page.

![](./image/FileServerUserGuide/image167.png)

#####<a name="5.3.12.6"/> Create file share with Oplock Force Level 2 enabled for cluster 

Repeat the steps in [Create file share for cluster](#5.3.12.5) except for setting the share location on the scale-out file server role on **Select the Server and path for this share** page and setting the share name to **SMBClusteredForceLevel2** on **Specify share name** page.

Enable **FORCE\_LEVELII\_OPLOCK** on this share. Please refer to [Enable FORCE_LEVELII_OPLOCK on share](#5.3.7).

#####<a name="5.3.12.7"/> Create file share with Encrypt data enabled for cluster

To create **SMBClusteredEncrypted** share on failover cluster (both **GeneralFS** and **ScaleOutFS**), repeat the steps in section [Create file share for cluster](#5.3.12.5) except select **Encrypt data access** option in **Other Settings** page.

####<a name="5.3.13"/> Turn off Firewall

Run the following command in console window:

	netsh advfirewall set allprofiles state off

####<a name="5.3.14"/> Create Local Admin User Account

Do not need to create a new one. Just use **Administrator**.

If the OS of SUT is Windows client SKU, you need to enable build-in administrator by running the following command:

	net.exe user administrator /active:yes

####<a name="5.3.15"/> Create a Local Non-Admin User Account

Run the following commands in console window as administrator privilege:

	net.exe user nonadmin Password01! /add

####<a name="5.3.16"/> Enable the Local Guest Account

Run the following commands in an elevated console window (run as administrator):

	net.exe user Guest /active:yes

If the OS of SUT is Windows client SKU, you need to remove Guest from the user right Deny log on as follows:

1.  Open the Group Policy Management Editor. To do so, click Start, then in the Start Search box, type gpedit.msc, and then press ENTER.

2.  If the User Account Control dialog box appears, confirm that the action it displays is what you want, and then click Continue.

3.  In the navigation pane, open Computer Configuration, Windows Settings, Security Settings, Local Policies, and User Rights Assignment.

4.  In the details pane, double-click "Deny log on locally" or "Deny access to this computer from the network".

5.  Select the user or group account you want to remove, and then click Remove.

6.  Click OK to save your changes to the GPO.

7.  Users that are currently logged on must log off and back on to be affected by the changed GPO setting.

####<a name="5.3.17"/> Reset Password to Local Users

Run the following commands in console window as administrator privilege:

	net.exe user administrator Password01!

####<a name="5.3.18"/> Create Asymmetric share

Execute the following command in a console window on both node01 and node02:

	REG ADD HKLM\System\CurrentControlSet\Services\LanmanServer\Parameters /v AsymmetryMode /t REG_DWORD /d 2 /f

####<a name="5.3.19"/> Create the virtual hard disk file

There are two ways to create the virtual hard disk file: PowerShell and manually.

Use "rsvd.vhdx" as an example of the virtual hard disk file name.

1.  Run PowerShell as administrator privilege, and execute the following command:

	"create vdisk file=C:\\rsvd.vhdx maximum=1024 type=expandable" | diskpart.exe

2.  Open Hyper-V manager, in the right panel, click **New** -&gt; **Hard Disk**

	Choose **VHDX** for **Disk Format**

	![](./image/FileServerUserGuide/image142.png)

	Choose **Dynamically expanding** for **Disk Type**

	![](./image/FileServerUserGuide/image143.png)

	![](./image/FileServerUserGuide/image144.png)Input the name of the virtual hard disk file in **Name** field

	Input **1** for **Size** field. And click **Finish** to create the virtual disk file.

	![](./image/FileServerUserGuide/image145.png)

####<a name="5.3.20"/> Create the virtual hard disk set file

There are two ways to create the virtual hard disk set file: PowerShell and manually.

Use "rsvd.vhds" as an example of the virtual hard disk set file name.

1.  Run PowerShell as administrator privilege, and execute the following command:

	New-VHD "c:\\rsvd.vhds" -SizeBytes 1GB -Fixed -LogicalSectorSizeBytes 512

1.  Open Hyper-V manager, in the right panel, click **New** -&gt; **Hard Disk**

	Choose **VHD Set** for **Disk Format**

	![](./image/FileServerUserGuide/image146.png)

	![](./image/FileServerUserGuide/image147.png)
	
	Choose **Fixed size** for **Disk Type**

	Input the name of the virtual hard disk set file in **Name** field

	![](./image/FileServerUserGuide/image148.png)

	Input **1** for **Size** field. And click **Finish** to create the virtual disk file.

	![](./image/FileServerUserGuide/image149.png)

####<a name="5.3.21"/> Modify the Signing configuration

Execute the following command in a console window on node01 to enable SigningRequired:

	cmd /C reg ADD "HKLM\\System\\ControlSet001\\Services\\LanmanServer\\Parameters" /f /v requiresecuritysignature /t REG\_DWORD /d 1

Execute the following command in a console window on node01 to disable SigningRequired:

	cmd /C reg DELETE "HKLM\\System\\ControlSet001\\Services\\LanmanServer\\Parameters" /f /v requiresecuritysignature

####<a name="5.3.22"/>  Manually Set up Computer Password

Run the following command in a console window:

ksetup /SetComputerPassword &lt;password&gt;

to set the password for the computer’s domain account. And restart the computer.

####<a name="5.3.23"/>Create an SQOS policy 

Run the following PowerShell command to create a MultiInstance sqos policy in the scale out file server:

	$policy = New-StorageQosPolicy -Name sqostest -PolicyType MultiInstance -MinimumIops 100 -MaximumIops 200

Get the policy id:

	$policy.PolicyId

##<a name="6"/>Installed Files and Folders

The installation process adds the following folders and files to the driver computer at C:\\MicrosoftProtocolTests\\FileS\\Server-Endpoint\\*&lt;version\#&gt;*\\.

![](./image/FileServerUserGuide/image3.png)Note

This path may vary based on your installation location.

The &lt;*version\#&gt;* placeholder indicates the installed build of the test suite.

| **File or Folder** | **Description**                                                                                                                                                                           |
|--------------------|-------------------|
| Batch              | Command files you can use to run individual test cases or all test cases.|                                                                                                           
| Bin                | Test suite binaries and configuration files. |                                                                                                                                                                                                                                                                                                                                               
| License.rtf        | The End User License Agreement.|                                                                                                                                                           


##<a name="7"/>Configure and Run Test Cases 

This section explains how to configure the test suite and how to run test cases.

###<a name="7.1"/>Configure and Run Test Cases Using Protocol Test Manager

Protocol Test Manager is a UI tool to help you to configure and run test cases.

You can follow the following steps to use Protocol Test Manager to configure the test suite:

1.  Launch the ProtocolTestManager.msi and install the Protocol Test Manager.

2.  **Select Test Suite:** Open the Protocol Test Manager from the shortcut on the desktop. Click **Configure Wizard** to configure the selected File Sharing test suite.

	![](./image/FileServerUserGuide/image150.png)

3.  After the first click, you can see the **Test Suite Introduction**, which provides some general information about File Sharing test suite:

	![](./image/FileServerUserGuide/image151.png)

4.  Click **Deployment Guide** to see how to do the configuration in Domain or Workgroup Environment

	![](./image/FileServerUserGuide/image152.png)

5.  Click **Next**, "Protocol Test Manager" supports three ways to configure the test suite. Auto-Detection, Manual Configuration and load a saved profile.

	![](./image/FileServerUserGuide/image153.png)

	(1).  **Configure the test suite with Run Auto-Detection**:

	-   Click **Run Auto-Detection** and navigate to **Auto-Detection** page.

	-   For each field in **Auto-Detection** page, you can use the value loaded from ptfconfig file, or change it by assigning an appointed value, and then click **Detect**. If SUT is in WORKGROUP, set **Domain Name** field to the same value as the **Target SUT** field.

	![](./image/FileServerUserGuide/image154.png)

	-   When the detection is finished, click **Next** to check the **Detection Result**, such as whether the SUT supports the specified capability or ioctl code. You can check the result by clicking each item.

	![](./image/FileServerUserGuide/image155.png)

	-   **Filter Test Cases**: The checked cases are selected automatically, which means that the features tested in these cases are supported by SUT. If the detection result is not supported, the feature is italic. You can also edit the selection on demand.

	![](./image/FileServerUserGuide/image156.png)

	(2).  **Configure the test suite manually**

	-   Click **Do Manual Configuration** and jump to **Filter Test Cases**.

	-   **Filter Test Cases**: Select the test cases you want to run.

	![](./image/FileServerUserGuide/image157.png)

	(3).  **Load Profile**: The Profile includes the information of previously selected test cases and the configuration.

	-   Click **Load Profile**, select an existing profile and jump to **Filter Test Cases**.

	![](./image/FileServerUserGuide/image158.png)

	-   **Filter Test Cases**: The checked cases are selected in the profile. You can also edit the selection if needed.

	![](./image/FileServerUserGuide/image159.png)


6.  **Configure Test Cases**: In this page, you can check the property values which are configured by detection result and user modification.

	![](./image/FileServerUserGuide/image160.png)

7.  **Configure Adapter**: In this page, you can change the SUT control adapter type.

	![](./image/FileServerUserGuide/image161.png)

8.  **Run Selected Test Cases**: Run all the test cases by clicking **Run All**; Select test cases and run them by clicking **Run Selected Test**. If a test case is executed, you can click this test case and the test log of the case is shown in the **Test logs** windows.

	![](./image/FileServerUserGuide/image162.png)

9.  After the test cases execution completed, you can click the hyperlink at the upper-right corner to open the Result folder of the test suite.

	![](./image/FileServerUserGuide/image163.png)

10.  **Export Results**: You can export the test cases from the Export/Import menu, you can also **Save Profile** from this menu.

	![](./image/FileServerUserGuide/image164.png)

	![](./image/FileServerUserGuide/image3.png)Note
	- You can drag the seperator between the case list and the log to adjust the width of the window.

 	- You can right-click on the test cases to uncheck all the test cases.

	![](./image/FileServerUserGuide/image165.png)

11.  Protocol Test Manager has a command line interface (ptmcli.exe) which can be used for automation test run.

	The command line interface ptmcli.exe is located in the bin folder of the Protocol Test Framework installation folder. To use PTM, you need to provide a profile using -p option.

	You can use "ptmcli.exe -h" to show more information.

###<a name="7.2"/>Configure Test Suite Manually

This test suite is installed with default configuration settings. You may need to change these settings if you use a customized test environment or if to customize your test runs.

You can configure the test suite for various purposes including, for example, to:

-   Define the settings of the test environment, including computer names and IP addresses.

-   Define the folders and formats used for output from test runs.

To change configuration settings, edit the \*.deployment.ptfconfig files including

-   CommonTestSuite.deployment.ptfconfig

-   ServerFailoverTestSuite.deployment.ptfconfig

-   MS-DFSC\_ServerTestSuite.deployment.ptfconfig

-   MS-FSRVP\_ServerTestSuite.deployment.ptfconfig

-   MS-SMB2\_ServerTestSuite.deployment.ptfconfig

-   MS-SMB2Model\_ServerTestSuite.deployment.ptfconfig

-   MS-RSVD\_ServerTestSuite.deployment.ptfconfig

-   MS-SQOS\_ServerTestSuite.deployment.ptfconfig

-   Auth\_ServerTestSuite.deployment.ptfconfig

You can find ptfconfig files at "**C:\\MicrosoftProtocolTests\\\\Server-Endpoint\\*&lt;version\#&gt;*\\Bin**" after test suite installed, which will be used when running batch scripts. 

####<a name="7.2.1"/> Brief introduction to Configuration Settings

1.  **Common Settings**

	Common settings in "CommonTestSuite.deployment.ptfconfig" are shared by all the sub test suites.

2.  **Settings for file server failover test suite**

    Settings in "ServerFailoverTestSuite.deployment.ptfconfig" are for the File server failover test suite.

3.  **Settings for DFSC server test suite**

    Settings in "MS-DFSC\_ServerTestSuite.deployment.ptfconfig" are for the DFSC server test suite.

4.  **Settings for FSRVP server test suite**

    Settings in "MS-FSRVP\_ServerTestSuite.deployment.ptfconfig" are for the FSRVP server test suite.

5.  **Settings for SMB2 server test suite**

    Settings in "MS-SMB2\_ServerTestSuite.deployment.ptfconfig" are for the SMB2 server test suite.

6.  **Settings for SMB2 server model test suite**

    Settings in "MS-SMB2Model\_ServerTestSuite.deployment.ptfconfig" are for the SMB2 server model test suite.

7.  **Settings for RSVD server test suite**

    Settings in "MS-RSVD\_ServerTestSuite.deployment.ptfconfig" are for the RSVD server test suite.

8.  **Settings for SQOS server test suite**

    Settings in "MS-SQOS\_ServerTestSuite.deployment.ptfconfig" are for the SQOS server test suite.

9.  **Settings for Auth server test suite**

    Settings in "Auth\_ServerTestSuite.deployment.ptfconfig" are for the Auth test suite.

Please refer to the corresponding \*.deployment.ptfconfig files for configuration details.

###<a name="7.3"/>Run Test Cases by Batch Script

This test suite includes command files that you can use to complete some basic test cases. Each test case verifies the protocol implementation based on a given scenario.

You can find and run these test cases in the following directory:

	C:\MicrosoftProtocolTests\FileServer\Server-Endpoint\<version#>\Batch

You can run these command files at the command prompt, or by selecting and clicking one or more of the files from the directory.

####<a name="7.3.1"/>Run the BVT Test

This test suite includes a set of basic tests called "BVT". Together these test cases perform a basic functionality test to evaluate the implementation on SUT machine. Use the following steps to run all BVT test cases.

To run the BVT test in a Domain environment

-   From the desktop of the driver computer, double-click the **Run Server-Domain\_BVTTestCases** shortcut. This shortcut is created during the installation process.

Alternatively, go to C:\\MicrosoftProtocolTests\\Server-Endpoint\\*&lt;version\#&gt;*\\Batch, and double-click the **Domain\_RunBVTTestCases.cmd** file.

To run the BVT test in a Workgroup environment

-   From the desktop of the driver computer, double-click the **Run Server-Workgroup\_BVTTestCases** shortcut. This shortcut is created during the installation process.

Alternatively, go to C:\\MicrosoftProtocolTests\\Server-Endpoint\\*&lt;version\#&gt;*\\Batch, and double-click the **Workgroup\_RunBVTTestCases.cmd** file.

####<a name="7.3.2"/> Run All Test Cases

Use the following steps to run all test cases.

To run all test cases in a Domain environment

-   From the desktop of the driver computer, double-click the **Run Server-Domain\_AllTestCases** shortcut. This shortcut is created during the installation process.

Alternatively, go to C:\\MicrosoftProtocolTests\\Server-Endpoint\\*&lt;version\#&gt;*\\Batch, and double-click the **Domain\_RunAllTestCases.cmd** file.

To run all test cases in a Workgroup environment

-   From the desktop of the driver computer, double-click the **Run Server-Workgroup\_AllTestCases** shortcut. This shortcut is created during the installation process.

Alternatively, go to C:\\MicrosoftProtocolTests\\Server-Endpoint\\*&lt;version\#&gt;*\\Batch, and double-click the **Workgroup\_RunAllTestCases.cmd** file.

####<a name="7.3.3"/> Check Test Results

Test suite generates test result files in different paths based on the way how test case is executed.

For running test case with batch: C:\\MicrosoftProtocolTests\\Server-Endpoint\\*&lt;version*\#&gt;\\Batch\\TestResults

For running test case with PTM: C:\\MicrosoftProtocolTests\\Server-Endpoint\\*&lt;version*\#&gt;\\TestResults

For further information about logging in the Protocol Test Framework (PTF), please see the *PTF User Guide* in the PTF installation directory

##<a name="8"/>Debugging Test Cases

You can use the Visual Studio solution (.sln) file included with this test suite to debug additional test cases that you create for your protocol implementation.

![](./image/FileServerUserGuide/image3.png)Note

While using Microsoft® Visual Studio® 2012 or above to run test cases, test suite may throw exception with message of "Cannot get test site". To solve this issue, please select the test settings file under test settings menu.

![](./image/FileServerUserGuide/image166.png)

To debug a test case

1.  On the driver computer, use Microsoft® Visual Studio® to open the following solution file: FileServer.sln

2.  In Visual Studio, in the Solution Explorer window, right-click the **Solution 'FileServer'**, and select **Build Solution**.

3.  When you build the test project, the tests appear in **Test Explorer**. If Test Explorer is not visible, choose **Test** on the Visual Studio menu, choose **Windows**, and then choose **Test Explorer**.

4.  Select your test cases from Test Explorer and run or debug them.
