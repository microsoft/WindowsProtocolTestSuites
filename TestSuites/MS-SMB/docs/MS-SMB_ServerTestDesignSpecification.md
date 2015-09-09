# MS-SMB Server Test Design Specification

## Contents

* [1 Document Review](#1)
    * [1.1 Introduction](#1.1)
        * [1.1.1 Overview](#1.1.1)
        * [1.1.2 Relationship to Other Protocols](#1.1.2)
		* [1.1.3 Message](#1.1.3)
	* [1.2 Protocol Properties](#1.2)
	* [1.3 Test Approach](#1.3)
	* [1.4 Adapter Method](#1.4)
* [2 Test Suite Planning](#2)
	* [2.1 Assumptions, Scope and Constraints](#2.1)
		* [2.1.1 Assumptions](#2.1.1)
		* [2.1.2 Scope](#2.1.2)
		* [2.1.3 Out of Scope](#2.1.3)
		* [2.1.4 Constraints](#2.1.4)
	* [2.2 Test Suite Approach](#2.2)
		* [2.2.1 Test Suite Overview](#2.2.1)
		* [2.2.2 Model Plan](#2.2.2)
	* [2.3 Technical Feasibility of Message Generation/Adapter Approach](#2.3)
	* [2.4 Scenarios](#2.4)
* [3 Test Suite Design](#3)
	* [3.1 Model or Test Design Description](#3.1)
		* [3.1.1 Model Design](#3.1.1)
		* [3.1.2 Scenario Machine Design](#3.1.2)
	* [3.2 Test Cases](#3.2)
	* [3.3 Adapter Design](#3.3)
		* [3.3.1 Protocol Adapter Design](#3.3.1)
		* [3.3.2 SUT Control Adapter Design](#3.3.2)
		* [3.3.3 Sequence Diagram](#3.3.3)

## <a name="1"/>1  Document Review
### <a name="1.1"/>1.1	Introduction
#### <a name="1.1.1"/>1.1.1	Overview

Common Internet File System (CIFS)  defines an open cross-platform mechanism for client systems to request file and print services from server systems over network.

Server Message Block (SMB) Protocol supports the sharing of files, print services and interprocess communication (IPC) mechanisms such as namepipe and mailslot. 
It defines extensions to the existing Common Internet File System (CIFS), and extends the CIFS with additional security, file, and disk management support. 
These extensions introduce new structures, new information levels, and extensions of CIFS request and response messages.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/sharetype.png) 

The SMB Protocol provides support for the following features:

* File & File System
	* Enumeration to previous versions of files. 
	* Access to previous versions of files. 
    * Server-side data copy method by which can be done entirely on the server-side without consuming network resources. 
    * Support native file and file system information levels in query and set operations. 
 	* Support more extended information in response to share connection and file open operations. 
* Security
	* New authentication methods, including Kerberos and NT LAN Manager (NTLM).
		* The Negotiate and Session Setup commands have been enhanced to carry opaque security tokens to support mechanisms compatible with the Generic Security Service (GSS).
    * Message signing to guarantee packet integrity. 

#### <a name="1.1.2"/>1.1.2	Relationship to Other Protocols

Figure below illustrates the relationships between MS-SMB and other protocols.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/protocol.png) 

* MS-SMB is an extension of MS-CIFS. 
* The transports of SMB can be Direct TCP (Transmission Control Protocol), NetBIOS over IPX or NetBIOS Extended User Interface (NetBEUI). 
* [MS-FSA] is algorithms, concepts and informative reference, it is not necessary to contain [MS-FSA] in the relationship diagram.

#### <a name="1.1.3"/>1.1.3	Message

##### Message Syntax

* SMB message is the payload packet encapsulated in a transport packet. 
    * Direct TCP:
    
	![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/tcp.png) 
	
	* NetBIOS over IPX:
	
    ![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/netbios.png) 
	
	* NetBIOS Extended User Interface (NetBEUI):
	
    ![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/NetBEUI.png) 

* SMB message consists of two parts:
	* SMB header, which has a fixed-length SMB protocol header
    * SMB payload, which has a variable length depending on the SMB command type in header that the message represent. 
* SMB Message Classification:

Message|Description
-----------------|----------
SMB_COM_NEGOTIATE	|Request and response messages used to handle dialect and capability negotiation between server and client.
SMB_COM_SESSION_SETUP	|Request and response messages used to session setup. They can be used to begin user authentication on an SMB connection and establish an SMB session.
SMB_COM_TREE_CONNECT	|Request and response messages used to create tree connection, which is a connection point to a share that exists on a server.
SMB_COM_NT_CREATE	|Request and response messages used to create or open a share on a server, including file, printer and IPC.
SMB_COM_OPEN	|Request and response messages used to open a share on a server, including file, printer and IPC. Those messages are obsolete as SMB_COM_NT_CREATE can do the same job.
SMB_COM_TRANSACTION	|Request and response messages used to execute a named pipe or mailslot operation on a server. (The status of the two subcommands in this message is obsolescent.)
SMB_COM_TRANSACTION2	|Request and response messages used to execute a specific operation on the server. It includes file enumeration, query and set file attribute operations, and DFS referral retrieval.
SMB_COM_NT_TRANSACTION	|Request and response messages used to specify operations on the server. It includes file open, file create, device I/O control, file rename, notify directory change, and set and query security descriptors.
SMB_COM_READ_ANDX	|Request and response messages used to read from a file or named pipe on a server. 
SMB_COM_WRITE_ANDX	|Request and response messages used to write data to a file or named pipe on a server. 

##### Message Flow

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/MessageFlow.png) 

Notice: Every successful operation must follow the three steps to create the SMB connection: negotiate, session setup and tree connect. 
After that, shares on remote server can be accessed in different operations. 
As there are more than 30 kinds of operations in SMB, we just call these operations as resource request/response instead of listing the details in the diagram. 
All of these operations follow a request/response pattern in which the client initiates all requests except oplock breaks.

Here is a sample of SMB file open operation:

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/sample.png) 

As shown in the figure above, the typical message flow between the client and server is as follows:

1.	For negotiating a dialect between server and client:
	* The client sends Negotiate Request to the server.
	* The server sends Negotiate Response to the client.
2.	For setting up a SMB session:
	* The client sends SessionSetup Request to the server.
	* The server sends SessionSetup Response to the client.
3.	For Creating a connection to a specific share:
	* The client sends TreeConnect Request to the server.
	* The server sends TreeConnect Response to the client.
4.	For Opening a file:
	* The client sends Create Request to the server.
	* The Server sends Create Response to the client.

### <a name="1.2"/>1.2	Protocol Properties

Protocol properties:

* SMB is a Client/Server protocol.
* SMB is a block, synchronous protocol.
* All data exchanged on the network is not encrypted. Clients and server can choose to sign the message or not.
* The transports of SMB can be Direct TCP, NetBIOS over IPX or NetBIOS Extended User Interface (NetBEUI). 

### <a name="1.3"/>1.3	Test Approach

__Recommended Test Approach__ 

The test suite uses a hybrid testing approach including Model Based Testing (MBT) and Traditional Testing.

__Reasons for MBT__

Test Suite Developer (TSD) adopts Model Based Testing to verify most of the requirements: 
* MS-SMB is a state-full protocol which contains 64 messages. MBT is suitable to be used as the testing method.
* Large amount of test cases are needed in the SMB test suite, and MBT can help us to generate them automatically.

__Reasons for Traditional Testing__

TSD also adopt Traditional Testing to cover cases which are hard for model to cover as there may be some test cases about time event.

### <a name="1.4"/>1.4	Adapter Method 

__Adapter Overview__

One Protocol Adapter, one Interactive Adapter and one SUT (System Under Control) Adapter are designed for the MS-SMB Test Suite. 

__Protocol Adapter__

* The protocol adapter uses managed code.
* The protocol adapter sends 10 types of messages to the SUT, receives responses from the SUT, parses the messages and capture requirements.

__Interactive Adapter__

As on some platforms, the SUT Control Adapter can’t be configured automatically, an Interactive adapter is added for avoiding this issue.
The Interactive Adapter is implemented by PowerShell scripts, and the functionalities are the same as SUT Control Adapter functionalities, which are:

* The adapter provides interfaces through which test cases can perform configurations on the server.
* The adapter provides interfaces through which disk I/O errors can be raised.
* The adapter provides interfaces through which a generic server open failure can be raised.

__SUT control adapter__

It is implemented using PowerShell Script and it is to:

* Create/delete a remote queue.
* Send messages to a remote queue, or retrieve unique lookup identifier from a remote queue.
* Create a distributed atomic transaction for receiving or moving the messages.

__Message Generation__

MS-SMB test suite generates all messages synthetically.
The ServerAdapter will invoke the protocol SDK (Software Development Kit) to generate all the messages in this protocol, and then sends it to the server. 
Then the ServerAdapter will use the protocol SDK to receive and parse the messages. 
Later the ServerAdapter will send the parsed result to relevant test case for further validation. 

## <a name="2"/>2	Test Suite Planning

### <a name="2.1"/>2.1	Assumptions, Scope and Constraints

#### <a name="2.1.1"/>2.1.1	Assumptions

The test suite assumes that all the shares (disk share, named pipe share, printer share and device share) already exist before running test suite.
WHY: All the shares can be configured according to deployment guide. So it is not necessary to configure by the test suite.

#### <a name="2.1.2"/>2.1.2	Scope

* The test suite will verify 20 SMB operations out of 23 SMB operations. For the purpose of better analyzing the protocol, the 20 SMB operations are partitioned into 6 Facets, as follows:

Facets|	SMB Operations
------|--------------------
Connection	|SMB_COM_ NEGOTIATE
Session	|SMB_COM_ SESSION_SETUP_ANDX
Tree	|SMB_COM_ TREE_CONNECT_ANDX
SMB Common Operations (4 operations)	|SMB _COM_NT_CREATE_ANDX
	|SMB_COM_OPEN_ANDX
	|SMB_COM_READ_ANDX
	|SMB_COM_WRITE_ANDX
SMB Transaction2 Operations (8 operations)	|TRANS2_FIND_FIRST2:  
	|   1. SMB_FIND_FILE_BOTH_DIRECTORY_INFO
	|   2. SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO
	|   3. SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO
	|TRANS2_FIND_NEXT2:
	|1. SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO
	|2. SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO
	|TRANS2_QUERY_FS_INFORMTION
	|TRANS2_SET_FS_INFORMATION
	|TRANS2_QUERY_PATH_INFORMATION
	|TRANS2_SET_PATH_INFORMATION
	|TRANS2_QUERY_FILE_INFORMATION
	|TRANS2_SET_FILE_INFORMATION
SMB NT Transaction Operations (5 operations)	|NT_TRANSACT_QUERY_QUOTA
	|NT_TRANSACT_SET_QUOTA
	|FSCTL_SRV_ENUMERATE_SNAPSHOTS
	|FSCTL_SRV_REQUEST_RESUME_KEY
	|FSCTL_SRV_COPYCHUNK

* The test suite is designed to cover all the requirements of MS-SMB on a generic file system. 
* The test suite is going to test requirements of [MS-FSCC] that are related to [MS-SMB].

	__Further explanation__
	
	[MS-FSCC] provides the information of structures and flags related to file system. The test suite will verify structures and flags of [MS-FSCC] that are documented explicitly in or referenced by [MS-SMB]. 

#### <a name="2.1.3"/>2.1.3	Out of Scope   

* The test suite will not verify the below operations in test suite of SMB: 
	WHY: The below operations are described only in CIFS instead of SMB, and there is no testable requirement in MS-SMB TD about them.
	
	SMB Commands|  Sub Commands
    ------------|--------------
	SMB_COM_TRANSACTION2	   |TRANS2_OPEN2 
	   |TRANS2_FSCTL 
	   |TRANS2_IOCTL2 
	   |TRANS2_FIND_NOTIFY_FIRST 
	   |TRANS2_FIND_NOTIFY_NEXT 
	   |TRANS2_CREATE_DIRECTORY 
	   |TRANS2_SESSION_SETUP 
	   |TRANS2_REPORT_DFS_INCONSISTENCY 
	SMB_COM_NT_TRANSACTION	    |NT_TRANSACT_SET_SECURITY_DESC 
	   |NT_TRANSACT_NOTIFY_CHANGE 
	   |NT_TRANSACT_QUERY_SECURITY_DESC 

* The test suite will not verify SMB_COM_TRANSACTION on named pipe and mailslot.
	WHY: The commands have been obsolescent.
	
#### <a name="2.1.4"/>2.1.4	Constraints

The test suite tests a single connection instead of multiple connections.

WHY: There is no behavior specified for multiple connections in TD. 

### <a name="2.2"/>2.2	Test Suite Approach	

#### <a name="2.2.1"/>2.2.1	Test Suite Overview

__Test Approach__

The test suite uses a hybrid testing approach including MBT and Traditional Testing.

__System Under Test (SUT)__

From the third party’s point of view, the SUT is an implementation of MS-SMB.
On a Windows platform, the SUT is SMB server.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/TestSuite.png) 

Further explanation to the above figure:

* There are two kinds of test cases, one is generated by SMB model and the other one is traditional test case.
* The SUT control adapter or interactive adapter implements create file and share file on SUT.
* The protocol adapter calls Protocol SDK to generate and send the request messages, receive and parse the response messages.
* TCP works as the transport between the test suite and SUT

#### <a name="2.2.2"/>2.2.2	Model Plan

SMB model is separated into 3 C# files:  Model, Transaction2Model and NTTransactionModel.

* Model: Covers the operations related to connections and creating files.
* Transaction2Model: Covers the operations related to SMB_COM_TRANSACTION2 Extensions.
* NTTransactionModel: Covers the Clarification and operations related to SMB_COM_NT_TRANSACTION Extensions, except the operations included in [Section 2.1.3](#2.1.3).

##### <a name="2.2.2.1"/>2.2.2.1	Model Abstraction 

__Action Abstraction__

A total of 81 actions are abstracted in the test suite.

* 75 actions are abstracted based on the 20 types of message exchange from the protocol.
	* 18 types of operations have 3 corresponding actions: a request, a response and an error response.
	* The operation SMB_COM_NEGOTIATE has 4 actions abstracted, which are NegotiateRequest, NegotiateResponse, NonExtendedNegotiateResponse and ErrorNegotiateResponse.
	* The operation SMB_COM_SESSION_SETUP_ANDX has 5 actions abstracted, which are SessionSetupRequest, NonExtendedSessionSetupRequest, SessionSetupResponse, NonExtendedSessionSetupResponse and ErrorSessionSetupResponse.
	* FsctlBadCommandRequest: This action is used to send invalid FSCTL command to server.
	* ErrorFsctlBadCommandResponse: This action is the response to the FSCTLBadCommandRequest action.
	* ErrorInvalidCommandResponse: This action is the response to the BadCommandRequest action.
	* 3 actions related to TRANS2_SET_FS_INFORMATION, which are Trans2SetFsInfoRequestAdditional,Trans2SetFsInfoResponseAdditional and ErrorTrans2SetFsInfoResponseAdditional
	* 3 actions related to NT_TRANSACT_SET_QUOTA, which are Trans2SetFsInfoRequestAdditional, Trans2SetFsInfoResponseAdditional and ErrorTrans2SetFsInfoResponseAdditional.
	* 3 actions related to Session setup, which are SessionSetupRequestAdditional,SessionSetupResponseAdditional and ErrorSessionSetupResponseAdditional.
* 6 actions are defined by the test suite:
	* SmbConnectionRequest: This action initializes the SMB underlying transport connection.
	* SmbConnectionResponse: The response to the SMBConnectionRequest action.
	* ServerSetup: This action sets up the server side environment, including message signing requirement and maximum read/write size.
	* ServerSetupResponse: The response to the ServerSetup action.
	* CreatePipeAndMailslot: This action creates a named pipe or mailslot at the server side, in order to enable client create its instances. 
	* CheckPreviousVersion: This action checks the previous version for the designated file identifier.

The 7 abstract data model classes are SmbConnection class, SmbFile class, SmbPipe class, SmbSession class, SmbShare class, SmbTree and SmbMailslot.

__SMB Helper Methods Abstraction__

There are 4 classes designed as helper methods. 

* Checker is used to check all the request and response messages.
* Parameter is used to test the overall variable definitions for environment setup.
* Update is used to update local data according to different messages.
* ModelHelper is used to capture requirements.

__SMB Data Abstraction__

In SMB models, there are 7 classes abstracted to simulate the state of both client and server. 

* SmbConnection: SmbConnection indicates the network connection between client and server while an SMB connection is active on the SMB transport
* SmbFile: SmbFile maintains necessary file information data according to all file operations. SMBFile serves as a file server image at the client side in order to verify all file operations.
* SmbPipe: SmbPipe maintains necessary named pipe data according to all named pipe operations. SMBPipe serves as a pipe server image at the client side in order to verify all named pipe operations.
* SmbMailslot: SmbMailslot maintains necessary file information data according to all mailslot operations. SMBMailslot serves as a mailslot server image at the client side in order to verify all mailslot operations.
* SmbSession: SmbSession represents the session established over a connection. Client can establish multiple sessions over one connection. 
* SmbShare: SmbShare represents the share information at server side. 
* SmbTree: SmbTree Connect represents the tree-connect information for a share. A client can establish multiple trees.

##### <a name="2.2.2.2"/>2.2.2.2	Model pattern

* Abstract Identifiers Pattern:
	* The MS-SMB model explores a set of simple types (int, enum, string and bool) as placeholders for the concrete values that will appear during protocol testing. For example, the actual values of sessionId, treeId and fileId cannot be identified at modeling time, so these variables are replaced by abstract identifiers, and the real data is stored in the protocol adapter. 
* Synchronous Protocols Pattern:
	* The MS-SMB model uses the underlying transport TCP to make the RPC calls when performing the other functionalities except event notification. This model pattern offers an appearance of a local call/return mechanism to verify the logic of the protocol, including the parameters being passed in both directions and the return values. For example, client sends SMB Create Request to server and synchronously receives the SMB Create Response from server.
* Server Initialization Pattern:
	* The MS-SMB model sets up the server, including create folder and share folder. For example, when setting up server environment, Action ServerSetup is called to configure enableSign which is used to set the server to support signature or not.
* Check OS Version Pattern
	* The MS-SMB model checks the SUT OS version on which the requirements are to be verified and stores this information as a variable for further use.            
* Check Return Value Pattern
	* The MS-SMB model verifies protocol model actions’ return values to cover the related requirements. For example, the model will verify the enumeration MessageStatus in the responses.
* Helper File Pattern:
	* The MS-SMB model adds a new file ModelHelper.cs in Model project, and designs the capture requirement method in ModelHelper.cs file.

##### <a name="2.2.2.3"/>2.2.2.3	States in Model	

* Before establishing the session: Global variable “smbState” is used for binding the model logic and state transfer.
* After establishing the session, all the abstracted data defined in connection are used for state transfer.
* Two accepting conditions are defined in model program:
	* The session is closed because of error response;
	* All the requests have received their associated responses after Tree Connection is established.
	
##### <a name="2.2.2.4"/>2.2.2.4	Distribution between Model and Adapters

The test suite will verify messages sent from the server, the model is responsible for driving adapters and verifying requirements relevant to logic, and the adapters verify the structures and values of packets.

* Model verifies if the received message is what the test suite expected. 
* Adapters verify the specific structures and concrete values of messages. 
For example, when the test suite receives Negotiate Response, the model will verify whether it is the expected value by comparing it with the expected values stored in the classes, and the protocol adapter will verify whether the ChallengeLength field is set to 0 when the CAP_EXTENDED_SECURITY bit is set.

### <a name="2.3"/>2.3	Technical Feasibility of Message Generation/Adapter Approach

__Technical Feasibility__

No technical issues are encountered in MS-SMB test suite.

__Adapter Approach__

Two adapters are used in the test suite, one is SUT Control Adapter or Interactive Adapter that is used to configure server. 
The other is Protocol Adapter that is used to cover test cases. 
Protocol Adapter is separated into 3 C# files, which are ISMBAdapter, ISMBTransaction2Adapter and ISMBNTTransactionAdapter, they construct the whole Protocol Adapter.

* ISMBAdapter.cs covers the operations in Model.cs.
* ISMBTransaction2Adapter.cs covers the operations in Transaction2Model.cs.
* ISMBNTTransactionAdapter.cs covers the operations in NTTransactionModel.cs.

__Protocol Adapter__

The protocol SDK will be used for message generation and parse for SMB test suite.

* Message Generation 
	Protocol Adapter calls the SMB SDK instead of PAC (Protocol Adapter Compiler) to generate SMB packets. Each formatted packet must satisfy all the specific requirements in SMB technical document and then sent by SendPacket function in SMB SDK.
* Message Consumption
	* When protocol adapter finishes sending SMB packets, it will receive packet from the server by calling ReceivePacket function in SMB SDK. During the process of receiving the packets, SMB SDK will also parse SMB packets and then return them to protocol adapter.  The protocol adapter will capture and verify the adapter requirements, while passing the test case requirements to the test cases to verify.
	* Besides, FSCC structures are embedded in the SMB packets received as data buffer, the parse of FSCC structures are done by FSCC SDK.  

__Interactive Adapter__

As on some platforms, the SUT Control Adapter can’t be configured automatically, an Interactive adapter is added for avoiding this issue.

The Interactive Adapter is implemented by PowerShell scripts, and the functionalities are the same as SUT Control Adapter functionalities, which are:

* The adapter provides interfaces through which test cases can perform configurations on the server.
* The adapter provides interfaces through which disk I/O errors can be raised.
* The adapter provides interfaces through which a generic server open failure can be raised.

__SUT Control Adapter__

SUT control is implemented using PowerShell Script and is responsible for 

* Creating/deleting a remote queue.
* Sending messages to a remote queue, or retrieving unique lookup identifier from a remote queue.
* Creating a distributed atomic transaction for receiving or moving the messages.

### <a name="2.4"/>2.4	Scenarios

There are total of 4 scenarios in SMB. 
Note: There are more than 300 machines, so only 2 machines are listed for each type of environment below.

No.	|Name	|Machine	|Covered Operation
----|-------|-----------|-----------------
1	|Windows 7--Windows Server 2008	|…	|…
	|	|NoSignSET_QUERY_PATHModelProgram_Win7_Win2K8	|SmbConnectionResponse
 | | |ServerSetup
 | | |NegotiateRequest
 | | |SessionSetupRequest
 | | |TreeConnectRequest
 | | |CreateRequest
 | | |Trans2SetPathInfoRequest
 | | |Trans2QueryPathInfoRequest
	| |	NoSignSET_QUERY_PATH_Win7_Win2K8	|SmbConnectionRequest
 | | |SmbConnectionResponse
 | | |ServerSetup
 | | |ServerSetupResponse
 | | |CreatePipeAndMailslot
 | | |NegotiateRequest
 | | |NegotiateResponse
 | | |SessionSetupRequest
 | | |SessionSetupResponse
 | | |TreeConnectRequest
 | | |TreeConnectResponse
 | | |CreateRequest
 | | |CreateResponse CheckPreviousVersion
 | | |Trans2SetPathInfoRequest
 | | |Trans2SetPathInfoResponse
 | | |Trans2QueryPathInfoRequest
 | | |Trans2QueryPathInfoResponse
	| |	…	|…
2	|Windows 7--Windows Server 2008 R2	|…	|…
	|	|SET_QUERY_FILEModelProgram_Win7_Win2K8R2	|SmbConnectionResponse
 | | |ServerSetup
 | | |NegotiateRequest
 | | |SessionSetupRequest
 | | |TreeConnectRequest
 | | |CreateRequest
 | | |Trans2SetFileInfoRequest
 | | |Trans2QueryFileInfoRequest
	|	|SET_QUERY_FILE_Win7_Win2K8R2	|SmbConnectionRequest
 | | |SmbConnectionResponse
 | | |ServerSetup
 | | |ServerSetupResponse
 | | |CreatePipeAndMailslot
 | | |NegotiateRequest
 | | |NegotiateResponse
 | | |SessionSetupRequest
 | | |SessionSetupResponse
 | | |TreeConnectRequest
 | | |TreeConnectResponse
 | | |CreateRequest
 | | |CreateResponse
 | | |Trans2SetFileInfoRequest
 | | |Trans2SetFileInfoResponse
 | | |Trans2QueryFileInfoRequest
 | | |Trans2QueryFileInfoResponse
 | | |Trans2QueryFileInfoRequest
 | | |Trans2QueryFileInfoResponse
	|	|…	|…
3	|Windows Vista --Windows Server 2008	|…	|…
	|	|NoSignSET_QUERY_FILEModelProgram_WinVista_Win2K8_InvalidLevel	|SmbConnectionResponse
 | | |ServerSetup
 | | |NegotiateRequest
 | | |SessionSetupRequest
 | | |TreeConnectRequest
 | | |CreateRequest
 | | |Trans2QueryFileInfoRequest
	|	|NoSignSET_QUERY_FILE_WinVista_Win2K8_InvalidLevel	|SmbConnectionRequest
 | | |SmbConnectionResponse
 | | |ServerSetup
 | | |ServerSetupResponse
 | | |CreatePipeAndMailslot
 | | |NegotiateRequest
 | | |NegotiateResponse
 | | |SessionSetupRequest
 | | |SessionSetupResponse
 | | |TreeConnectRequest
 | | |TreeConnectResponse
 | | |CreateRequest
 | | |CreateResponse
 | | |Trans2QueryFileInfoRequest
	|	|…	|…
4	|Windows Vista --Windows Server 2008 R2	|…	|…
	|	|SET_QUERY_FILEModelProgram_WinVista_Win2K8R2	|SmbConnectionResponse
 | | |ServerSetup
 | | |NegotiateRequest
 | | |SessionSetupRequest
 | | |TreeConnectRequest
 | | |CreateRequest
 | | |Trans2SetFileInfoRequest
 | | |Trans2QueryFileInfoRequest
	|	|SET_QUERY_FILE_WinVista_Win2K8R2	|SmbConnectionRequest
 | | |SmbConnectionResponse
 | | |ServerSetup
 | | |ServerSetupResponse
 | | |CreatePipeAndMailslot
 | | |NegotiateRequest
 | | |NegotiateResponse
 | | |SessionSetupRequest
 | | |SessionSetupResponse
 | | |TreeConnectRequest
 | | |TreeConnectResponse
 | | |CreateRequest
 | | |CreateResponse
 | | |Trans2SetFileInfoRequest
 | | |Trans2SetFileInfoResponse
 | | |Trans2QueryFileInfoRequest
 | | |Trans2QueryFileInfoResponse
 | | |Trans2QueryFileInfoRequest
 | | |Trans2QueryFileInfoResponse
	|	|…	|…

Common Steps:

* SmbConnectionRequest
	The client sends an SMB connection request to the server.
* SmbConnectionResponse
	The server sends an SMB connection response to the client.
* ServerSetup
	The client sends an SMB connection setup request with the server.
* ServerSetupResponse
	The server sends an SMB connection setup response to the client.
* CreatePipeAndMailslot
	The server creates named pipe and mail slot.
* NegotiateRequest
	The client sends negotiate request to the server.
* NegotiateResponse
	The server sends negotiate response to the client.
* SessionSetupRequest
	The client sends session setup request to the server.
* SessionSetupResponse
	The server sends session setup response to the client.
* TreeConnectRequest
	The client sends a tree connection request to the server.
* TreeConnectResponse
	The server sends a tree connection response to the client.
* CreateRequest
	The client sends a file creation request to the server.
* CreateResponse
	The server sends a file creation response to the client.
	
__Typical machine__

__Windows Vista --Windows Server 2008 R2: SET_QUERY_FILE_WinVista_Win2K8R2__

__Description:__ This machine is related to the environment Windows Vista --Windows Server 2008 R2. And it is used to cover the requirements in Trans2SetFileInfoRequest
Trans2SetFileInfoResponse, Trans2QueryFileInfoRequest, Trans2QueryFileInfoResponse, Trans2QueryFileInfoRequest, Trans2QueryFileInfoResponse.

__Detailed Sequence:__

1.	Common steps.
2.	The client sends a set file information request to the server.
3.	The server sends a set file information response to the client.
4.	The client sends a file information query request to the server.
5.	The server sends a file information query response to the client.
6.	The client sends a file information query request to the server.
7.	The server sends a file information query response to the client.

Note: Other machines are similar with the typical machine.

## <a name="3"/>3	Test Suite Design

### <a name="3.1"/>3.1	Model or Test Design Description

#### <a name="3.1.1"/>3.1.1	Model Design

Hybrid testing, that is MBT and traditional testing combined, is adopted as the test approach for the MS-SMB test suite. 
TSD (Test suite developer) designed one Model according to the protocol behaviors, and the scenarios designed will generate test cases to test the server behaviors. 
Model Design is based on Model Plan as specified in [section 2.2.2](#2.2.2).

__Model Class Diagram__

The MS-SMB model consists of 12 classes: 1 model class, 7 abstract data model classes, and 4 helper classes.

* The model class ModelProgram is the main class in the MS-SMB model. It defines 81 model actions, as specified in [Section 2.2.2](#2.2.2).
	* 18 types of operations have 3 corresponding actions: a request, a response and an error response.
	* The operation SMB_COM_NEGOTIATE has 4 actions abstracted, which are NegotiateRequest, NegotiateResponse, NonExtendedNegotiateResponse and ErrorNegotiateResponse.
	* The operation SMB_COM_SESSION_SETUP_ANDX has 5 actions abstracted, which are SessionSetupRequest, NonExtendedSessionSetupRequest, SessionSetupResponse, NonExtendedSessionSetupResponse and ErrorSessionSetupResponse.
	* FSCTLBadCommandRequest: This action is used to send invalid FSCTL command to server.
	* ErrorFsctlBadCommandResponse: This action is the response to the FsctlBadCommandRequest action.
	* ErrorInvalidCommandResponse: This action is the response to the BadCommandRequest action.
	* 3 actions related to TRANS2_SET_FS_INFORMATION, which are Trans2SetFsInfoRequestAdditional,Trans2SetFsInfoResponseAdditional and ErrorTrans2SetFsInfoResponseAdditional
	* 3 actions related to NT_TRANSACT_SET_QUOTA, which are Trans2SetFsInfoRequestAdditional, Trans2SetFsInfoResponseAdditional and ErrorTrans2SetFsInfoResponseAdditional.
	* 3 actions related to Session setup, which are SessionSetupRequestAdditional,SessionSetupResponseAdditional and ErrorSessionSetupResponseAdditional.
	All the above actions are implemented in the same manner: The client sends one request to the server and the server will return two kinds of response, which are successful response or failed response.  
	For example, there are 3 actions related to the operation “SMB_COM_WRITE_ANDX”, which are WriteRequest, WriteResponse and ErrorWriteResponse. 
	WriteRequest is used to send a request to server to write data to a file or a named pipe. If the operation succeeds, the WriteResponse will be called to receive the successful respone from server. 
	If the operation is failed, the ErrorWriteResponse will be called to receive the failed respone from server.
* 6 actions are defined by the test suite:
	* ServerSetup: This action sets up the server side environment, including message signing requirement and maximum read/write size.
	* ServerSetupResponse: The response to the ServerSetup action.
	* CreatePipeAndMailslot：This action creates a named pipe or mailslot at the server side, in order to enable client create its instances. 
	* CheckPreviousVersion: This action checks the previous version for the designated file identifier.
* The 7 abstract data model classes are SmbConnection class, SmbFile class, SmbPipe class, SmbSession class, SmbShare class, SmbTree and SmbMailslot as specified in section 2.2.2. 

There are 4 helper classes to the MS-SMB model. The relationship between these helper classes and the model class is illustrated in the following figure:

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/helper.png) 

Further explanation of the helper classes:

* The class Parameters helps to set up the environment and the initial values of the model.
* Each time a request or a response is processed, model must validate the messages and update the model state. The 2 classes Checker and ModelHelper guarantee that the model follows the logics specified in the protocol.
	* Checker: Each request has its own validation procedure with “Condition.isTrue()” statements. And there are common procedures which can be extracted as a uniform method. MessageChecks is used to do all common message validation. Responses are validated by method CheckReponse in this class.
	* ModelHelper is used to capture code.
* The class Update is responsible for updating all local data structures after a request is sent or a response is received.

__Global Variables__

12 global variables are designed in the model. 

* The enumeration variable “smbState” is used to indicate the initial state of SMB model.
* The class variable “connection” is used to indicate the network connection between the client and the server. As described before, there is only one connection in the Test Suite.
* The enumeration variable “smbRequest” is used to save subclass object.
* If the Boolean variable “isSendBufferSizeExceedMaxBufferSize” is set, it indicates the maximum server buffer size of bytes it writes can exceed the MaxBufferSize field. 
* If the Boolean variable “isWriteBufferSizeExceedMaxBufferSize” is set, it indicates the maximum server buffer size of bytes it writes can exceed the MaxBufferSize field.
* The int variable “quotaUsed” is used to record quota information.
* The int variable “fIdUsed” is used to define fid used for setting quota.
* The Boolean variable “isQuotaSet” is used to record whether quota has been set by client
* The string variable “resumeKey” is used to define resumeKey.
* The int variable “copychunkSourceFid” is used to set copychunkSourceFid to 0.

__Requirement Verification Strategy in the Model__

Every action in the model will handle success and error operations. If the action returns success or an expected error code, the model will move on to the next state and continue with the next action if any. 
Otherwise, if any unexpected error is returned, the test case will stop and exit. The model verifies the requirements involving logics, such as the requirements concerning return values.

#### <a name="3.1.2"/>3.1.2	Scenario Machine Design

The MS-SMB test suite contains about 320 machines in cord script to generate test cases that are utilized to cover the server requirements. 

__State Transitions__

The following diagram shows the state transitions in MS-SMB test suite.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/StateTransition.jpg) 

As shown in the figure above, the state transitions in MS-SMB test suite are implemented as follows:

1.	State variable is initialized to “Init”
2.	When “SmbConnectionRequest” is called, the state will be transited to “ConnectionRequest”.
3.	When “SmbConnectionResponse” is returned, the state will be transited to ”ConnectionSuccess”.
4.	When “ServerSetup” is called, the state will be transited to “SeverSetupRequest”.
5.	When “ServerSetupResponse” is returned, the state will be transited to” ServerSetupSuccess”.
6.	When “CreateNamePipeAndMailslot” is called, the state will be transited to “CreateNamePipeAndMailslotSuccess”.
7.	When “NegotiateRequest” is called, the state will be transited to “NegotiateSent”.
8.	If “NegotiateResponse” is returned, the state will be transited to “NegotiateSuccess”
	If “ErrorResponse” is returned, the state will be transited to “Closed”.
9.	When “SessionSetupRequest” is called, the state will be transited to “SessionSetup Success”.
10.	If “SessionSetupResponse” is returned, the state will be transited to “SessionSetup Success”.
	If “ErrorResponse” is returned, the state will be transited to “Closed”.
11.	When “TreeConnectRequest” is called, the state will be transited to “TreeConnectSent”.
12.	If “TreeConnectResponse” is returned, the state will be transited to “TreeConnectionSuccess”.
	If “ErrorResponse” is returned, the state will be transited to “Closed”.
13.	When the client sends requests for SMB operations, such as sending request for“SMB_COM_NT_CREATE_ANDX”, if the operation succeeds, the state will be returned to “TreeConnectionSuccess”, if the operation is failed, the state will be transited to “End”.

__The Principle of Scenario Machine Design__

The model actions are designed according to the 22 request types.

* Normally the MS-SMB Model sends one request and waits to receive the corresponding response. 
	For example: After sending out the ReadRequest, the model waits to receive a ReadResponse. Only after the model receives it, it will continue sending out the next request. 
	
__Sample Scenario__

The following is the message sequence of scenario03: SMB_S13_Write:

```
machine NoSignInvalidTokenTraditional_Model_Win7_Win2K8R2() : Main
{
(
	SmbConnectionRequest;
	SmbConnectionResponse;
	ServerSetup;
	ServerSetupResponse;
	CreatePipeAndMailslot;
	NegotiateRequest;
	NegotiateResponse;
	SessionSetupRequest;
	SessionSetupResponse;
    TreeConnectRequest;
    TreeConnectResponse;
    CreateRequest;
    CreateResponse;
	(
    Trans2FindFirst2RequestInvalidDirectoryToken|
    Trans2FindFirst2RequestInvalidFileToken
    );
    ErrorResponse;
    SessionClose*;
) 
|| NoSignInvalidTokenTraditional_Win7_Win2K8R2
}
```

### <a name="3.2"/>3.2	Test Cases 

#### Model-Based Testing 

The MS-SMB model generates 3467 test cases to cover the requirements in the model and in the adapter. The generated test cases also cover the protocol examples mentioned in section 4 of the protocol.

__Test Case Limiting__

In order to limit the number of the test cases, the MS-SMB Test Suite uses the strategies of scenario slicing, parameter generation, binding and long test case.

* Bindings:
	The test suite binds parameters to their possible values in order to reduce the amount of test cases. For example, the shareType parameter is bound to ShareType.DISK. Parameter bindings can avoid generating excessive test cases. The model only generates the test cases with parameter values that are necessary to cover necessary requirements. The requirement coverage will not be reduced as a result of this binding.
* Long test cases:
	The test suite uses the long test cases strategy for all the SpecExplorermachines that are used for test case generation. During test case generation, the machines that adopt long test cases attempt to explore more states in a single test case in order to limit the amount of generated test cases.
* Requirement coverage:
	The test suite uses requirement coverage rule for all the SpecExplorermachines that are used for test case generation. During test case generation, the machines that adopt enough test cases cover all the requirements and avoid superfluous cases.

#### Traditional Testing 

There are 9 test cases designed for the traditional scenario. Details about the test cases are as follows.
Common steps of all the traditional test cases:

1.	The client sends a SMB_COM_NEGOTIATE request to the server to initiate an SMB session between the client and the server.
2.	The server replies a successful SMB_COM_NEGOTIATE response.
3.	The client sends a SMB_COM_SESSION_SETUP_ANDX request to begin user authentication on an SMB connection and establish an SMB session.
4.	The server replies a SMB_COM_SESSION_SETUP_ANDX response with STATUS_MORE_PROCESSING_REQUIRED.
5.	The client begins second round trips and send SMB_COM_SESSION_SETUP_ANDX request.
6.	The server replies a successful SMB_COM_SESSION_SETUP_ANDX response.
7.	The client sends a SMB_COM_TREE_CONNECT_ANDX request to establish a tree connect to a share.
8.	The server replies a successful SMB_COM_TREE_CONNECT_ANDX response.


<empty>  |<empty>  
-------|-----
__Test ID__|TraditionalTestCase_IgnoreFields_Create_01
__Description__|The test case is to test the requirements that the client sets the unused or reserved fields of SMB_COM_NT_CREATE_ANDX and the server ignores and replies the same.
__Prerequisites__|There is a share in the sever side.
__Scenario__ |SMB_S08_Ignore
__Test Execution Steps__|1.	Execute all common steps. 
						|2.	The client sends a SMB_COM_NT_CREATE_ANDX request to create a file: 
						|- Reserved bits in ExtFileAttributes field set to 0.
						|- CreateDisposition field set to FILE_OPEN_IF.
						|- Unused bits of Flags field set to 0.
						|- Other fields set according to TD.
						|3.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
						|4.	The client sends a SMB_COM_NT_CREATE_ANDX request to open the file in step 2.
						|	- Reserved bits in ExtFileAttributes field set to 1.
						|	- CreateDisposition field set to FILE_OPEN_IF.
						|	- Unused bits of Flags field set to 1.
						|	- Other fields set according to TD and the same with step 2.
						|5.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
						|6.	Compare the responses in step 3 and step 5, if they are the same, verify R109027 and R109243.
						|7.	Disconnect the tree, session and connection.
__Cleanup__                 |N/A


<empty>  |<empty>  
-------|-----
__Test ID__|TraditionalTestCase_IgnoreFields_FIND_FIRST2_02
__Description__|The test case is to test the requirements that the client sets the unused or reserved fields of TRANS2_FIND_NEXT2 and the server ignores and replies the same.
__Prerequisites__|There is a share in the sever side.
__Scenario__ |SMB_S08_Ignore
__Test Execution Steps__|1.	Execute all common steps. 
 |2.	The client sends a SMB_COM_NT_CREATE_ANDX request to open a directory:
 |-	CreateDisposition field set to FILE_OPEN.
 |-	Other fields set according to TD.
 |3.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |4.	The client sends a TRANS2_FIND_FIRST2 request to retrieve an enumeration of files.
 |-	The bits unlisted in TD 2.2.1.2.2 of SearchAttributes field set to 0.
 |-	The Reserved bit in SearchAttributes field set to 0.
 |-	InformationLevel field set to SMB_INFO_STANDARD.
 |-	Other fields set according to TD.
 |5.	The server returns a successful TRANS2_FIND_FIRST2 response.
 |6.	The client sends a TRANS2_FIND_NEXT2 request to continue the file enumeration.
 |-	InformationLevel field set to SMB_INFO_STANDARD.
 |-	Other fields set according to TD.
 |7.	The server returns a successful TRANS2_FIND_NEXT2 response.
 |8.	The client sends a TRANS2_FIND_FIRST2 request to retrieve an enumeration of files again under the directory.
 |-	The bits unlisted in TD 2.2.1.2.2 of SearchAttributes field set to 1.
 |-	The Reserved bit in SearchAttributes field set to 1.
 |-	InformationLevel field set to SMB_INFO_STANDARD.
 |-	Other fields set according to TD and the same with step 4.
 |9.	The server returns a successful TRANS2_FIND_FIRST2 response.
 |10.	The client sends a TRANS2_FIND_NEXT2 request to continue the file enumeration.
 |-	InformationLevel field set to SMB_INFO_STANDARD.
 |-	Other fields set according to TD.
 |11.	The server returns a successful TRANS2_FIND_NEXT2 response.
 |12.	Compare the responses in step 7 and step 11, if they are the same, verify R109033 and R109056.
 |13.	Disconnect the tree, session and connection.
__Cleanup__                 |N/A


<empty>  |<empty>  
-------|-----
__Test ID__ |TraditionalTestCase_IgnoreFields_Negotiate_03
__Description__ |The test case is to test the requirements that the client sets the unused or reserved fields of SMB_COM_NEGOTIATE and the server ignores and replies the same.
__Prerequisites__|There is a share in the sever side.
__Scenario__|SMB_S08_Ignore
__Test Execution Steps__|1.	The client sends a SMB_COM_NEGOTIATE request to server to initiate an SMB session between the client and the server.
 |-	Unused bits in Flags2 field set to 0.
 |-	SMB_FLAGS2_SMB_SECURITY_SIGNATURE_REQUIRED bit in Flags2 field set to 0.
 |-	Other fields set according to TD.
 |2.	The server replies a successful SMB_COM_NEGOTIATE response.
 |3.	Disconnect.
 |4.	The client sends a SMB_COM_NEGOTIATE request again to server to initiate an SMB session between the client and the  server.
 |-	Unused bits in Flags2 field set to 1.
 |-	SMB_FLAGS2_SMB_SECURITY_SIGNATURE_REQUIRED bit in Flags2 field set to 1.
 |-	Other fields set according to TD and the same with step 1.
 |5.	The server replies a successful SMB_COM_NEGOTIATE response.
 |6.	Check the follows:
 |-	Compare all the fields of the responses in step 2 and step 5 except for unused bits in Flags2, if they are the same, verify R5277 and R5298.
 |-	Compare the value in Unused bits in Flags2 of the client request and the server response, if they are the same,  verify R108.
 |7.	Disconnect
__Cleanup__|N/A


<empty>  |<empty>  
-------|-----
__Test ID__|Traditional_TraditionalTestCase_IgnoreFields_Tree_04
__Description__|The test case is to test the requirements that the client sets the unused or reserved fields of successful SMB_COM_NT_CREATE_ANDX and the server ignores and replies the same.
__Prerequisites__|There is a share in the sever side.
__Scenario__|SMB_S08_Ignore
__Test Execution Steps__|1.	Execute Common Step 1~6.
 |2.	The client sends a SMB_COM_NT_CREATE_ANDX request to create a file:
 |-	The bits not listed in TD 2.2.4.7.1 of Flags field set to 0.
 |-	Other fields set according to TD.
 |3.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |4.	The client sends a SMB_COM_NT_CREATE_ANDX request to open the file in step 2.
 |-	The bits not listed in TD 2.2.4.7.1 of Flags field set to 1.
 |-	Other fields set according to TD and the same with step 2.
 |5.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |6.	Compare the responses in step 3 and step 5, if they are the same, verify R5596.
 |7.	Disconnect the tree, session and connection.
__Cleanup__|N/A


<empty>  |<empty>  
-------|-----
__Test ID__|TraditionalTestCase_IgnoreFields_CopyChunk_05
__Description__|The test case is to test the requirements that the client sets the unused or reserved fields of successful SMB_COM_NT_CREATE_ANDX and the server ignores and replies the same.
__Prerequisites__|There is a share in the sever side.
__Scenario__|SMB_S08_Ignore
__Test Execution Steps__|1.	Execute Common Step 1~6.
 |2.	The client sends a SMB_COM_NT_CREATE_ANDX request to create a file:
 |-	The bits not listed in TD 2.2.4.7.1 of Flags field set to 0.
 |-	Other fields set according to TD.
 |3.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |4.	The client sends a SMB_COM_NT_CREATE_ANDX request to open the file in step 2.
 |-	The bits not listed in TD 2.2.4.7.1 of Flags field set to 1.
 |-	Other fields set according to TD and the same with step 2.
 |5.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |6.	Compare the responses in step 3 and step 5, if they are the same, verify R5596.
 |7.	Disconnect the tree, session and connection.
__Cleanup__|N/A


<empty>  |<empty>  
-------|-----
__Test ID__|TraditionalTestCase_IgnoreFields_QueryQuota_06
__Description__|The test case is to test the requirements that the client sets the unused or reserved fields of successful NT_TRANSACT_QUERY_QUOTA and the server ignores and replies the same.
__Prerequisites__|There is a share in the sever side.
__Scenario__|SMB_S08_Ignore
__Test Execution Steps__|1.	Execute all common steps.
 |2.	The client sends a SMB_COM_NT_CREATE_ANDX request to open a file:
 |-	CreateDisposition field set to FILE_OPEN.
 |-	Other fields set according to TD.
 |3.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |4.	The client sends a NT_TRANSACT_QUERY_QUOTA request to query quota information.
 |-	StartSidLength field set to 0.
 |-	StartSidOffset field set to 0.
 |-	Other fields set according to TD.
 |5.	The server returns a successful NT_TRANSACT_QUERY_QUOTA response.
 |6.	The client sends a NT_TRANSACT_QUERY_QUOTA request to again query quota information.
 |-	StartSidLength field set to 0.
 |-	StartSidOffset field set to 1.
 |-	Other fields set according to TD and the same with step 4.
 |7.	The server returns a successful NT_TRANSACT_QUERY_QUOTA response.
 |8.	Compare the responses in step 5 and step 7, if they are the same, verify R109446.
 |9.	Disconnect the tree, session and connection.
__Cleanup__|N/A

<empty>  |<empty>  
-------|-----
__Test ID__|TraditionalTestCase_IgnoreFields_SET_FILE_09
__Description__|The test case is to test the requirements that the client sets the unused or reserved fields of successful TRANS2_SET_FILE_INFORMATION and the server ignores and replies the same.
__Prerequisites__|There is a share in the sever side.
__Scenario__|SMB_S08_Ignore
__Test Execution Steps__|1.	Execute all Common steps.
 |2.	The client sends a SMB_COM_NT_CREATE_ANDX request to create a named pipe.
 |-	CreateDisposition field set to FILE_OPEN_IF.
 |-	Other fields set according to TD.
 |3.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |4.	The client sends a TRANS2_SET_FILE_INFORMATION request to open the named pipe.
 |-	InformationLevel field set to FILE_LINK_INFORMATION.
 |-	Reserved field in FILE_LINK_INFORMATION structure set to 0
 |-	Other fields set according to TD.
 |5.	The server returns a successful TRANS2_SET_FILE_INFORMATION response.
 |6.	The client sends a TRANS2_SET_FILE_INFORMATION request to open the named pipe.
 |-	InformationLevel field set to FILE_LINK_INFORMATION.
 |-	Reserved field in FILE_LINK_INFORMATION structure set to 0
 |-	Other fields set according to TD.
 |7.	The server returns a successful TRANS2_SET_FILE_INFORMATION response.
 |8.	Compare the responses in step 5 and step 7, if they are the same, verify R109590.
 |9.	Disconnect the tree, session and connection.
__Cleanup__|N/A


<empty>  |<empty>  
-------|-----
__Test ID__|TraditionalTestCase_LARGE_Read_Write_10
__Description__|The test case is to test the requirements that the client Sends the SMB_WRITE_ADNX request, client and server both support the CAP_LARGE_WRITEX capability, then server returns response with more than the maximum buffer size.
__Prerequisites__|There is a share in the sever side.
__Scenario__|N/A
__Test Execution Steps__|1.	Execute all common steps.
 |In SMB_COM_SESSION_SETUP_ANDX request, set the Capabilities to 0xC000.
 |2.	The client sends a SMB_COM_NT_CREATE_ANDX request to create a named pipe.
 |-	CreateDisposition field is set to FILE_OPEN_IF.
 |-	CreateOptions field is set to FILE_OPEN_REPARSE_POINT, FILE_SEQUENTIAL_ONLY, and FILE_NON_DIRECTORY_FILE.
 |-	Other fields are set according to the TD.
 |3.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |4.	The client sends a SMB_COM_WRITE_ANDX request to write bytes to a file.
 |5.	The server returns a successful SMB_COM_WRITE_ANDX response.
 |6.	Check the count in the SmbParameters of the response, to verify requirement 9207.
 |7.	The client sends a SMB_COM_READ_ANDX request to query quota information.
 |8.	The server returns a successful SMB_COM_READ_ANDX response.
 |9.	Check the ByteCount in the SmbData of the response, to verify requirements R9957, R5402, R206932, and R106932.
 |10.	Disconnect the tree, session and connection.
__Cleanup__|N/A


<empty>  |<empty>  
-------|-----
__Test ID__|TraditionalTestCase_Disconnect_14_Case
__Description__|The test case is to test the requirements that the client sets the TREE_CONNECT_ANDX_DISCONNECT_TID bit in the Flags field of SMB_COM_TREE_CONNECT_ANDX and the server disconnects.
__Prerequisites__|There is a tree connect with specified TID in the SMB header of the request.
__Scenario__|N/A
__Test Execution Steps__|1.	Execute all common steps.
 |2.	The client sends a SMB_COM_NT_CREATE_ANDX request to create a named pipe.
 |- CreateDisposition field is set to FILE_OPEN_IF.
 |- FILE_WRITE_THROUGH, FILE_SYNCHRONOUS_IO_ALERT bits of CreateOptions field is set to 0.
 |- FILE_SYNCHRONOUS_IO_NONALERT bit of CreateOptions field is set to 1.
 |- Other fields are set according to the TD.
 |3.	The server returns a successful SMB_COM_NT_CREATE_ANDX response.
 |4.	The client sends a NT_TRANSACT_QUERY_QUOTA request to query quota information from a server.
 |- SidListLength filed is set to 0.
 |- StartSidLength field is set to 0.
 |- StartSidOffset field is set to 90.
 |- Other fields are set according to the TD.
 |5.	The server returns a successful NT_TRANSACT_QUERY_QUOTA response.
 |6.	Check the Flags field in the SMB header of NT_TRANSACT_QUERY_QUOTA request, if it contains  TREE_CONNECT_ANDX_DISCONNECT_TID bit, then verify the requirement 10357.
 |7.	Disconnect the connection.
__Cleanup__|N/A

### <a name="3.3"/>3.3	Adapter Design

The MS-SMB Test Suite implements 2 adapters: 1 protocol adapter, 1 Interactive adapter and 1 SUT control adapter.

#### <a name="3.3.1"/>3.3.1	Protocol Adapter Design

The Protocol Adapter has 43 methods and 35 events. 

* MS-SMB Adapter uses its methods to send request to the SUT and to receive the response from the SUT. 
	For example: After sending out the ReadRequest, the model waits to receive a ReadResponse. Only after the model receives it, it will continue sending out the next request. 
* MS-SMB Adapter uses its events to trigger bound methods to notify the test case that the expected message has been received. 
	For example: ReadResponse is triggered when receiving the ReadResponse message to invoke a bound method.

#### <a name="3.3.2"/>3.3.2	SUT Control Adapter Design

SUT Control Adapter or Interactive Adapter is used to create/delete a remote queue, send messages to a remote queue, 
or retrieve unique lookup identifier from a remote queue and create a distributed atomic transaction for receiving or moving the messages. Correspondingly, there are 2 methods designed. 
For details of the methods used in SUT control adapter or Interactive Adapter, please refer to [Section 2.3](#2.3).

* SUT Control Adapter Class

The diagram of MS-SMB Adapter Interface is given below.

**SUT Control Adapter Interface**

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/SutControlAdapter.png) 

**Interactive Adapter Interface**
 
![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/InteractiveAdapter.png) 

* The SUT Control Adapter or Interactive Adapter is implemented using Powershell script.

#### <a name="3.3.3"/>3.3.3	Sequence Diagram 

The figure below shows the interactions among the test cases, protocol adapter, SUT control adapter and the SUT.

![](https://github.com/Microsoft/WindowsProtocolTestSuites/blob/master/TestSuites/MS-SMB/docs/images/InnerWorkingAdapter.png) 

The diagram uses the message sequence of scenario S13: SMB_S13_Write to demonstrate the inner workings among adapters, test case and SMB server. The detailed message sequence is as follows:

1.	The test case drives SUT control adapter or Interactive Adapter to initialize the configuration of SUT.
2.	SUT control adapter or Interactive Adapter calls PowerShell scripts to configure SUT.
3.	The test case drives the protocol adapter to send an SMB Negotiate Request.
4.	The protocol adapter sends a SMB Negotiate Request to SUT.
5.	SUT returns a SMB Negotiate Response to the protocol adapter which then passes the response to the test case.
6.	The test case drives the protocol adapter to send a SMB SessionSetup Request.
7.	The protocol adapter sends a SMB SessionSetup Request to SUT.
8.	SUT returns a SMB SessionSetup Response to the protocol adapter which then passes the response to the test case.
9.	The test case drives the protocol adapter to send a SMB TreeConnect Request.
10.	The protocol adapter sends a SMB TreeConnect Request to SUT.
11.	SUT returns a SMB TreeConnect Response to the protocol adapter which then passes the response to the test case.
12.	The test case drives the protocol adapter to send a SMB Create Request.
13.	The protocol adapter sends a SMB Create Request to SUT.
14.	SUT returns a SMB Create Response to the protocol adapter which then passes the response to the test case.
15.	The test case drives the protocol adapter to send a Write Request.
16.	The protocol adapter sends a Write Request to SUT.
17.	SUT returns a Write Response to the protocol adapter which then passes the response to the test case.













	













