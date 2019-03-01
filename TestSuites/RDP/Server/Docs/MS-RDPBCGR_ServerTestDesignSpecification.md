
# MS-RDPBCGR Server Test Design Specification

## Contents

* [Technical Document Analysis](#_Toc396468143)
    * [Technical Document Overview](#_Toc396468144)
    * [Relationship to Other Protocols](#_Toc396468145)
    * [Protocol Operations/Messages](#_Toc396468146)
    * [Protocol Properties](#_Toc396468147)
* [Test Method](#_Toc396468148)
    * [Assumptions, Scope and Constraints](#_Toc396468149)
    * [Test Approach](#_Toc396468150)
    * [Test Scenarios](#_Toc396468151)
		* [S1_Connection](#_Toc396468152)
		* [S2_AutoReconnect](#_Toc396468153)
		* [S3_Input](#_Toc396468154)
		* [S4_Output](#_Toc396468155)
		* [S5_StaticVirtualChannel](#_Toc396468156)
		* [S6_AutoDetectTest](#_Toc396468157)
		* [S7_MultitransportBootstrapping](#_Toc396468158)
		* [S8_HealthMonitoring](#_Toc396468159)		
* [Test Suite Design](#_Toc396468160)
    * [Test Suite Architecture](#_Toc396468161)
		* [System under Test (SUT)](#_Toc396468162)
		* [Test Suite Architecture](#_Toc396468163)
    * [Technical Dependencies/Considerations](#_Toc396468164)
		* [Dependencies](#_Toc396468165)
		* [Technical Difficulties](#_Toc396468166)
		* [Encryption Consideration](#_Toc396468167)
    * [Adapter Design](#_Toc396468168)
		* [Adapter Overview](#_Toc396468169)
		* [Technical Feasibility of Adapter Approach](#_Toc396468170)
		* [Adapter Abstract Level](#_Toc396468171)
* [Test Cases Design](#_Toc396468172)
    * [Traditional Test Case Design](#_Toc396468173)
    * [Test Cases Description](#_Toc396468174)
		* [BVT Test cases](#_Toc396468175)
* [Appendix](#_Toc396468176)
    * [Glossary](#_Toc396468177)
    * [Reference](#_Toc396468178)

## <a name="_Toc396468143"/>Technical Document Analysis

### <a name="_Toc396468144"/>Technical Document Overview
The Remote Desktop Protocol: Basic Connectivity and Graphics Remoting is designed to facilitate user interaction with a remote computer system by transferring graphics display information from the remote computer to the user and transporting input commands from the user to the remote computer, where the input commands are replayed on the remote computer. RDP also provides an extensible transport mechanism which allows specialized communication to take place between components on the user computer and components running on the remote computer.


### <a name="_Toc396468145"/>Relationship to Other Protocols
[MS-RDPBCGR] is based on the ITU (International Telecommunication Union) T.120 series of protocols. The T.120 standard is composed of a suite of communication and application-layer protocols that enable implementers to create compatible products and services for real-time, multipoint data connections and conferencing.

The following protocols are tunneled within an [MS-RDPBCGR] static virtual channel:

* Multiparty Virtual Channel Extension [MS-RDPEMC]

* Clipboard Virtual Channel Extension [MS-RDPECLIP]

* Audio Output Virtual Channel Extension [MS-RDPEA]

* Remote Programs Virtual Channel Extension [MS-RDPERP]

* Dynamic Channel Virtual Channel Extension [MS-RDPEDYC]

* File System Virtual Channel Extension [MS-RDPEFS]

* Serial Port Virtual Channel Extension [MS-RDPESP]

* Print Virtual Channel Extension [MS-RDPEPC]

* Smart Card Virtual Channel Extension [MS-RDPESC]

### <a name="_Toc396468146"/>Protocol Operations/Messages
There are 50 protocol data units (PDUs) described by [MS-RDPBCGR] protocol, and they can be classified into the following message flows:

| &#32;| &#32; |
| -------------| ------------- |
|  **Message Flows** |  **Number of PDUs**|
| Connection Sequence| 23|
| Deactivation-Reactivation Sequence| 1|
| Disconnection Sequences| 3|
| Automatic Reconnection| 1|
| Server Error Reporting and Status Updates| 2|
| Static Virtual Channels| 1|
| Keyboard and Mouse Input| 4|
| Basic Server Output| 4|
| Logon Notifications| 1|
| Controlling Server Graphics Output| 2|
| Display Update Notifications| 1|
| Server Redirection| 2|
| Network Characteristics Detection| 2|
| Multitransport Bootstrapping| 2|
| Connection Health Monitoring| 1|

The Connection Sequence is one of the most important message flows; it exchanges client and server settings and negotiates common settings to use for the duration of the connection so that the input, graphics, and other data can be exchanged and processed between the client and server. The Connection Sequence is described in following figure (Figure 1-1). All message exchanges in this diagram are strictly sequential, except where noted in the text that follows.

![image1.png](./image/RDP_ServerTestDesignSpecification/image1.png)

Figure 1-1: Remote Desktop Protocol (RDP) connection sequence

### <a name="_Toc396468147"/>Protocol Properties

* RPCBCGR is a block protocol and is based on TCP.
* In RDPBCGR, the client and the server roles are as mentioned in the protocol document. For example, in a normal connection sequence the client sends the requests and the server responds. After the normal connection sequence, the client sends the keyboard input and the server sends basic output concurrently.
* In the RDP connection phase, the PDUs exchanged between the client and server are tightly coupled by a sequence that makes the protocol synchronous in this phase. For example, Connection Initialization, Basic Setting Exchange, Channel Connection, and Capabilities Negotiation are synchronous. However, the Connection Finalization is asynchronous, which means there is no need for server to send out a response until a specific request is received.  And after the RDP connection is established, most of the PDUs are sent asynchronously, few of which need a response from the sender. For example, the Keyboard and Mouse Input PDUs are sent from the client and do not require any response from server.
* RDPBCGR is dependent on protocols like X.224 and T.125. The protocol document specifies that the RDP packets are encapsulated in an X224 and TPKT header.
* MS-RDPBCGR supports external security protocols like TLS, CredSSP, and Standard RDP Security, which is specified in this protocol.
* The client to server data is always encrypted, and the protocol supports multiple encryption levels.
* The RSA algorithm is used to encrypt the Client Random Number (32 bytes) for transmission to the server, and the RC4 algorithm is extensively used for encrypting the data.
* In addition to encryption, RDP compresses virtual channel data and some data in PDU’s that are sent from client to server.
* ASN.1 encoding is used to encode and decode the structures.

## <a name="_Toc396468148"/>Test Method

### <a name="_Toc396468149"/>Assumptions, Scope and Constraints
**Assumptions:**

* The RDP server machine should be configurable; in order to test some specific features or requirements, the test suite will try to trigger the server to act with specific configuration settings, such as capability set support.
**Scope:**

* The protocol server endpoint (RDP server) playing the server role will be tested. For Windows, the Remote Desktop Service (TermService) is the server endpoint.
* The protocol client endpoint (RDP client) playing the client role is out of scope.
* The virtual channel is out of scope because the MS-RDPBCGR uses the I/O channel as the data transport.
* Testing X.224 and T.125 is out of scope.
* For Windows, the System Under Test (SUT) will be both Server SKUs and Client SKUs.
* External protocols are out of scope.
* Compression is out of scope.

**Constraint:**
There is no constraint for this Test Suite.

### <a name="_Toc396468150"/>Test Approach
Recommended Test Approach
Traditional testing is chosen as the test approach in MS-RDPBCGR.
 Test Approach Comparison
Table 11 illustrates the comparison of test approaches for the MS-RDPBCGR test suite.

|  **Factor**|  **Model-Based (MBT)**|  **Traditional**|  **Best Choice**|
| -------------| -------------| -------------| ------------- |
|  **Stateful**| After the RDP connection is established, most of the PDUs sent by both the server and the client are asynchronous; most of them do not require a response, so MBT cannot cover all PDUs in models.| Traditional Testing can handle this situation easily.| Traditional|
|  **Simple logic**| The logic is not complex. MBT will require more initial effort and will be more difficult to maintain.| Traditional testing will be less effort.| Traditional|
|  **Large Number of Test Cases**| Based on the newest test suite development process, the number of test cases is not expected to be too large.  But MBT may generate a lot of “garbage” test cases.| It is easy to create useful test cases with Traditional Testing. It can reduce the number of cases and the cost of sustaining.  | Traditional|
|  **Simple combinations of parameters**| Parameter combination is not complex. The only case to be considered is the support for Capability Sets, which will be addressed with the use of configuration files.  | It is easy to cover all combinations of parameters with Traditional Testing.| Traditional|

***Table 11 Test Approach Comparison***
Reasons for choosing Traditional Testing

* The protocol is not completely stateful.

* The logic of this protocol is simple.

* Only 14 out of 50 PDUs in the connection/disconnection sequences are sent sequentially.

* The combinations of parameters are not complex. Capability sets are the exception, which will be addressed through the use of configuration files.

### <a name="_Toc396468151"/>Test Scenarios

|  **Scenario**|  **Priority**|  **Test Approach**|  **Description**|
| -------------| -------------| -------------| ------------- |
| S1_Connection| 0| Traditional| This scenario is used to verify connection and disconnection sequences.|
| S2_AutoReconnect| 0| Traditional| This scenario is used to verify auto-reconnect sequences.|
| S3_Input| 0| Traditional| This scenario is used to verify slow-path and fast-path input PDUs.|
| S4_Output| 0| Traditional| This scenario is used to verify fast-path output PDUs.|
| S5_StaticVirtualChannel| 0| Traditional| This scenario is used to verify virtual channel PDUs.|
| S6_AutoDetect| 0| Traditional| This scenario is used to verify auto-detect PDUs|
| S7_MultitransportBootstrapping| 0| Traditional| This scenario is used to verify Multitransport initialization PDUs|
| S8_HealthMonitoring| 1| Traditional| This scenario is used to Verify Heartbeat PDU|

**Table 21 Test Suite Scenarios**

#### <a name="_Toc396468152"/>S1_Connection
Preconditions:

N/A.

Typical Sequence:

The typical scenario sequence is the following:

* The client initiates the connection by sending the server a Class 0 X.224 Connection Request PDU.

* The server responds with a Class 0 X.224 Connection Confirm PDU to the client.

* The client sends the MCS Connect Initial PDU containing a Generic Conference Control (GCC) Conference Create Request to the server.

* The server responds with an MCS Connect Response PDU containing a GCC Conference Create Response to the client.

* The client sends an MCS Erect Domain Request PDU to the server.

* The client then sends the MCS Attach User Request PDU to the server.

* The server responds with an MCS Attach User Confirm PDU containing the User Channel ID.

* The client then sends multiple MCS Channel Join Request PDUs to the server.

* The server confirms each channel with an MCS Channel Join Confirm PDU.

* The client sends a Security Exchange PDU to the server.

* The client sends the Client info PDU to the server.

* The server sends a Server License Error PDU – Valid Client to the client.

* The server sends a Server Demand Active PDU to the client.

* The client responds by sending a Client Confirm Active PDU to the server.

* The following PDUs will be sent, in order, by the client after sending the Client Confirm Active PDU:

	* Client Synchronize PDU
	* Client Control PDU - Cooperate
	* Client Control PDU - Request Control
	* Client Persistent Key List PDU
	* Client Font List PDU

* After receiving the Client Confirm Active PDU, the server will respond with the following PDUs:

	* Server Synchronize PDU
	* Server Control PDU – Cooperate
	* Server Control PDU - Granted Control
	* Server Font Map PDU

* The server or client initiates a disconnection sequence when the following messages are exchanged:

	* If initiated by server:
		* The server sends a Deactivate All PDU to the client.
		* The server sends an MCS Disconnect Provider Ultimatum PDU to the client.

	* If initiated by client:
		* The client sends a Shutdown Request PDU to the server.
		* The server may respond with a Shutdown Request Denied PDU.
		* The client may respond with an MCS Disconnect Provider Ultimatum PDU if it received a Shutdown Request Denied PDU and it decides to proceed with the disconnection.

The Connection Sequence is also described in Figure 1-1, section [1.3](#_Protocol_Operations/Messages).
 Scenario Testing:
This scenario tests the following 27 messages:

* Class 0 X.224 Connection Request PDU
* Class 0 X.224 Connection Confirm PDU
* MCS Connect Initial PDU With a Generic Conference Control (GCC) Conference Create Request
* MCS Connect Response PDU With a GCC Conference Create Response
* MCS Erect Domain Request PDU
* MCS Attach User Request PDU
* MCS Attach User Confirm PDU
* MCS Channel Join Request PDU
* MCS Channel Join Confirm PDU
* Security Exchange PDU
* Client Info PDU
* Server License Error PDU – Valid Client
* Server Demand Active PDU
* Client Confirm Active PDU
* Client Synchronize PDU
* Client Control PDU - Cooperate
* Client Control PDU - Request Control
* Client Persistent Key List PDU
* Client Font List PDU
* Server Synchronize PDU
* Server Control PDU - Cooperate
* Server Control PDU - Granted Control
* Server Font Map PDU
* Deactivate All PDU
* MCS Disconnect Provider Ultimatum PDU
* Shutdown Request PDU
* Shutdown Request Denied PDU

#### <a name="_Toc396468153"/>S2_AutoReconnect
Preconditions:

* There was a disconnection due to a network error, and the client is trying to reconnect.

* The server has sent a Save Session Info PDU to the client before the disconnection and the Automatic Reconnection Cookie is still available.

Typical Sequence:
The typical scenario sequence is as follows:

* The client initializes a connection sequence.
* In the Secure Settings Exchange phase of the connection sequence, the client sends a cryptographically-modified version of the cookie to the server in the Client Info PDU.
* If server grants the auto-reconnection request, it will continue the connection without requiring the client to resend user credentials.
* If server denies the auto-reconnection request, it will responds with a Server Auto-Reconnect Status PDU.
* The client can continue the connection by providing credentials.

Scenario Testing:

* Client Info PDU with the Client Auto-Reconnect Packet

#### <a name="_Toc396468154"/>S3_Input

Preconditions:

N/A

Typical Sequence:

The typical scenario sequence is the following:
* Slow-path Input
	* The client builds an RDP connection to the server.
	* The client sends Slow-Path Input Event PDUs carrying various input events to server.
* Fast-path Input
	* The client builds an RDP connection to the server.
	* The client sends Fast-Path Input Event PDUs carrying various input events to the server.

Scenario Testing:

Slow-Path Input Event PDUs with the following input events:

* Keyboard Event
* Unicode Keyboard Event
* Mouse Event
* Extended Mouse Event
* Synchronize Event

Fast-Path Input Event PDUs with the following input events:

* Keyboard Event
* Unicode Keyboard Event
* Mouse Event
* Extended Mouse Event
* Synchronize Event

#### <a name="_Toc396468155"/>S4_Output
Preconditions:

The RDP connection has been established.

Typical Sequence:

The typical scenario sequence is as follows:

* The client builds an RDP connection to the server.

* The client sends Fast-Path Input Event PDUs carrying various input events to the server.

* Server sends Server Fast-Path Update PDU

Scenario Testing:

Server Fast-Path Update PDU with structures:

* Fast-Path Bitmap Update
* Fast-Path Palette Update
* Fast-Path Synchronize Update
* Fast-Path Surface Commands Update
* Fast-Path System Pointer Hidden Update
* Fast-Path System Pointer Default Update
* Fast-Path Pointer Position Update
* Fast-Path Color Pointer Update
* Fast-Path Cached Pointer Update
* Fast-Path New Pointer Update

#### <a name="_Toc396468156"/>S5_StaticVirtualChannel

Preconditions:

N/A

Typical Sequence:

The typical scenario sequence is as follows:

* The client builds an RDP connection to the server.
* The client sends Virtual Channel PDUs to the server.
* The server disconnects.

Scenario Testing:

* Virtual Channel PDU

#### <a name="_Toc396468157"/>S6_AutoDetectTest
Preconditions:

N/A

Typical Sequence:

The typical scenario sequence is the following:

* Connect-Time Network Characteristics Detection

* The client initiates an RDP connection to the server, and completes the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase and Secure Setting Exchange Phase.

* The server sends a Server Auto-Detect Request PDU with RDP_RTT_REQUEST

* The client responses a Client Auto-Detect Response PDU with RDP_RTT_RESPONSE

* The server sends a Server Auto-Detect Request PDU with RDP_BW_START

* The server sends several Server Auto-Detect Request PDUs with RDP_BW_PAYLOAD

* The server sends a Server Auto-Detect Request PDU with RDP_BW_STOP

* The client responses a Client Auto-Detect Response PDU with RDP_BW_RESULTS

* The server sends a Server Auto-Detect Request PDU with RDP_NETCHAR_RESULT

Continuous Network Characteristics Detection

* The client builds an RDP connection to the server.

* The server sends a Security Header with RDP_RTT_REQUEST

* The client responses a Client Auto-Detect Response PDU with RDP_RTT_RESPONSE

* The server sends a Security Header with RDP_BW_START

* The server sends a Security Header with RDP_BW_STOP

* The client responses a Client Auto-Detect Response PDU with RDP_BW_RESULTS

Network Characteristics Sync after Auto Reconnection

* The client builds an RDP connection to the server. During Optional Connect-Time Auto-Detection phase, the server detects the Network Characteristics and sends the result to the client.

* A disconnection occurred due to a network error, and the client is trying to reconnect.

* During Optional Connect-Time Auto-Detection phase of the connection, the server sends a Server Auto-Detect Request PDU with RDP_RTT_REQUEST

* The client responses a Client Auto-Detect Response PDU with RDP_NETCHAR_SYNC

Scenario Testing:
Server Auto-Detect Request PDUs with the following structures:

* RTT Measure Request (RDP_RTT_REQUEST)

* Bandwidth Measure Start (RDP_BW_START)

* Bandwidth Measure Payload (RDP_BW_PAYLOAD)

* Bandwidth Measure Stop (RDP_BW_STOP)

* Network Characteristics Result (RDP_NETCHAR_RESULT)

Client Auto-Detect Response PDUs with the following structures:

* RTT Measure Response (RDP_RTT_RESPONSE)

* Bandwidth Measure Results (RDP_BW_RESULTS)

* Network Characteristics Sync (RDP_NETCHAR_SYNC)

#### <a name="_Toc396468158"/>S7_MultitransportBootstrapping
Preconditions:

N/A

Typical Sequence:

The typical scenario sequence is the following:

* The client initiates an RDP connection to the server, and completes the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase.

* The server sends two Server Initiate Multitransport Request PDUs to initial reliable and lossy UDP connection

Scenario Testing:

* Server Initiate Multitransport Request PDU

#### <a name="_Toc396468159"/>S8_HealthMonitoring
Preconditions:

N/A

Typical Sequence:

The typical scenario sequence is as follows:

* The client and server build an RDP connection to the server.

* The server sends Server Heartbeat PDU periodically, which specified period of each send, how many missed heartbeat will trigger warning, and how many missed heartbeat will trigger a reconnection.

Scenario Testing:

* Server Heartbeat PDU

## <a name="_Toc396468160"/>Test Suite Design

### <a name="_Toc396468161"/>Test Suite Architecture

#### <a name="_Toc396468162"/>System under Test (SUT)
* From the third party point of view, the SUT is a component that implements MS-RDPBCGR Server.
* From the Windows implementation point of view, the SUT is the Remote Desktop Service (TermService).

#### <a name="_Toc396468163"/>Test Suite Architecture
Figure 31 illustrates the architecture of the MS-RDPBCGR test suite for server endpoint testing.

![image2.png](./image/RDP_ServerTestDesignSpecification/image2.png)
 _Figure 31 RDP Server Test Suite Architecture_

### <a name="_Toc396468164"/>Technical Dependencies/Considerations

#### <a name="_Toc396468165"/>Dependencies
There are no dependencies.

#### <a name="_Toc396468166"/>Technical Difficulties
There are no technical difficulties.

#### <a name="_Toc396468167"/>Encryption Consideration

* When using the RDP standard security mechanism, MS-RDPBCGR messages are encrypted; however, you can turn off server-side encryption by setting the encryption level to low.

* When using external security protocols, such as TLS and CredSSP, the MS-RDPBCGR transport is encrypted; this encryption cannot be turned off.

### <a name="_Toc396468168"/>Adapter Design

#### <a name="_Toc396468169"/>Adapter Overview
The MS-RDPBCGR Server Test Suite implements a protocol adapter. The protocol adapter is used to receive messages from the SUT and to send messages to the SUT. The protocol adapter is built upon the protocol test suite library, which is implemented with managed code.

#### <a name="_Toc396468170"/>Technical Feasibility of Adapter Approach
The protocol adapter uses the protocol SDK library to generate protocol messages, which are sent to SUT. Protocol adapter compiler (PAC) is used in the MS-RDPBCGR protocol test suite library. The protocol adapter uses the protocol test suite library to consume protocol messages which are received from the SUT. The MS-RDPBCGR SDK library will parse and decode the received messages and send them to protocol adapter.

#### <a name="_Toc396468171"/>Adapter Abstract Level
Protocol Adapter
Protocol adapter defined tens of interfaces. These interfaces can be summarized as following:

* Send Message interfaces
	* Send interface for each client-to-server message.
* Receive Message events
	* Receive event for each server-to-client message.
* Other interfaces
	* **EstablishRDPConnection**: Establish a RDP connection with RDP Server
	* **ChannelJoinRequestAndConfirm**: Complete the channel join sequence
	* **GenerateStaticVirtualChannelTraffics**: Generate static virtual channel traffics
	* **GenerateSlowPathInputs**: Send Client Input Event PDUs with all kinds of input events
	* **GenerateFastPathInputs**: Send Client Fast-Path Input Event PDU with all kinds of input events
	* **ExpectFastpathOutputs**: Expect and verifies fast-path output events during a specific timespan
	* **ProcessAutoDetectSequence**: Process the network auto-detect sequence

## <a name="_Toc396468172"/>Test Cases Design

### <a name="_Toc396468173"/>Traditional Test Case Design
The Traditional test approach is used to design all test cases. Currently, only BVT test cases are designed to cover main in-scope testable requirements.
The following table shows the number of test cases for each scenario.

| &#32;| &#32;| &#32;| &#32;| &#32; |
| -------------| -------------| -------------| -------------| ------------- |
|  **Scenario**|  **Test cases**|  **BVT**|  **P0**|  **P1**|
| S1_Connection| 8| 8| 8| 0|
| S2_AutoReconnect| 1| 1| 1| 0|
| S3_Input| 2| 2| 2| 0|
| S4_Output| 1| 1| 1| 0|
| S5_StaticVirtualChannel| 1| 1| 1| 0|
| S6_AutoDetect| 1| 1| 1| 0|
| S7_MultitransportBootstrapping| 1| 1| 1| 0|
| S8_HealthMonitoring| 1| 1| 1| 0|


### <a name="_Toc396468174"/>Test Cases Description
The test suite is a synthetic RDP client. In the following descriptions, all instances of the term “Test Suite” can be understood as the RDP client.

**Common prerequisites for all test cases:**

* The RDP service is started and listening the service port which serves the RDP server.

* The test suite knows the IP address and port number on which RDP server is listening.

**Common cleanup requirements:**

* The test suite disconnects all RDP connections if there any.

* The SUT deletes all data caches from previous RDP connections.   

The common prerequisites and cleanup requirements are not listed in any of the test cases. Only prerequisites and cleanup requirements unique to the test case are listed in the corresponding test case descriptions.

#### <a name="_Toc396468175"/>BVT Test cases

#####S1_Connection_ConnectionInitiation_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S1_Connection_ConnectionInitiation_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case tests RDP server can correctly process Client X.224 Connection Request PDU with all security protocols supported and responses a correct  Server X.224 Connection Confirm PDU|
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Start RDP connection to SUT by sending a Client X.224 Connection Request PDU.|
| | routingToken and cookie are not present|
| | rdpNegReq is present and requestedProtocols field is set with all flags.|
| | rdpCorrelationInfo is not present|
| | Waite for Server X.224 Connection Confirm PDU with RDP_NEG_RSP structure from SUT.|
| | Verify the received Server X.224 Connection Confirm PDU. |
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#####S1_Connection_BasicSettingExchange_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S1_Connection_BasicSettingExchange_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case tests the SUT can process the valid Client MCS Connect Initial PDU with GCC Conference Create Request correctly and responses a valid Server MCS Connect Response PDU with GCC Conference Create Response.|
|  **Prerequisites**|  |
|  **Test Execution Steps**| Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase. |
| | Test Suite continues the connection sequence by sending a valid Client MCS Connect Initial PDU with GCC Conference Create Request.|
| | All fields of TS_UD_CS_CORE are valid|
| | All fields of  TS_UD_CS_SEC are valid|
| | All fields of  TS_UD_CS_NET are valid, at least contains one channel|
| | TS_UD_CS_CLUSTER is present, and all fields are valid|
| | TS_UD_CS_MCS_MSGCHANNEL, TS_UD_CS_MULTITRANSPORT are present   |
| | The test suite expect a Server MCS Connect Response PDU with GCC Conference Create Response.|
| | The test suite verifies the received Server MCS Connect Response PDU with GCC Conference Create Response.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S1_Connection_ChannelConnection_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S1_Connection_ChannelConnection_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case is used to verify that SUT can process Channel Connection phase correctly.|
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate an RDP connection to SUT and complete the Connection Initiation phase and Basic Setting Exchange phase.|
| | Test Suite sends a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.|
| | Test Suite expects a Server MCS Attach User Confirm PDU from SUT.|
| | Test Suite verifies the received MCS Attach User Confirm PDU.|
| | Test Suite start the channel join sequence. Test suite use the MCS Channel Join Request PDU to join the user channel obtained from the Attach User Confirm PDU, the I/O channel and all of the static virtual channels obtained from the Server Network Data structure.|
| | Test Suite expects and verifies a Server MCS Channel Join Confirm PDU respectively for each MCS Channel Join Request PDU.|
| | After all the channels created, the Test suite close the connection.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S1_Connection_SecurityExchange_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S1_Connection_SecurityExchange_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case is used to verify SUT can process RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.|
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, and Channel Connection phase.|
| | If Standard RDP Security mechanisms are being employed, Test Suite sends a Client Security Exchange PDU to SUT.|
| | Test Suite sends SUT a Client Info PDU to SUT.|
| | Test Suite expects a Server License Error PDU from SUT.|
| | Test Suite verifies the Server License Error PDU received.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S1_Connection_CapabilityExchange_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S1_Connection_CapabilityExchange_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case is used to verify SUT can process the Capability Exchange phase successfully.|
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase.|
| | Test Suite expects a Server Demand Active PDU from SUT. When received, Test Suite verifies this PDU.|
| | Test Suite sends a Client confirm Active PDU to SUT. |
| | Test Suite expects SUT continues the connection by sending a Server Synchronize PDU.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S1_Connection_ConnectionFinalization_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S1_Connection_ConnectionFinalization_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case is used to verify SUT can process the Connection Finalization phase successfully. |
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate an RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase, Licensing phase, and Capabilities Exchange phase.|
| | Test Suite continues the connection by sending the following PDUs sequentially:|
| | Client Synchronize PDU|
| | Client Control PDU - Cooperate|
| | Client Control PDU - Request Control|
| | Client Persistent Key List PDU(optional)|
| | Client Font List PDU|
| | Test Suite expects and verifies the following PDUs one by one from SUT sequentially:|
| | Server Synchronize PDU|
| | Server Control PDU – Cooperate|
| | Server Control PDU - Granted Control|
| | Server Font Map PDU|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S1_Connection_Disconnection_PositiveTest_ClientInitiated_UserLogon

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S1_Connection_Disconnection_PositiveTest_ClientInitiated_UserLogon|
|  **Priority**| P0|
|  **Description** | This test case is used to verify the messages and behaviors of the disconnection sequence initiated by SUT, after user logon.  |
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate and complete an RDP connection with RDP server (SUT).|
| | Test suite expects a Server Save Session Info PDU, and verify it.|
| | Test suite sends a Shutdown Request PDU to initiate a disconnection sequence.|
| | Test Suite expects a Shutdown Request Denied PDU.|
| | Test Suite sends an MCS Disconnect Provider Ultimatum PDU and closes the connection. |
|  **Requirements Covered**|  |
|  **Cleanup**| N/A|

#### S1_Connection_Disconnection_PositiveTest_ClientInitiated_UserNotLogon

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S1_Connection_Disconnection_PositiveTest_ClientInitiated_UserNotLogon|
|  **Priority**| P1|
|  **Description** | This test case is used to verify the messages and behaviors of the disconnection sequence initiated by SUT, before user logon.  |
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate and complete an RDP connection with RDP server (SUT). During connection, don’t set auto logon flag in Client Info PDU.|
| | Test suite sends a Shutdown Request PDU to initiate a disconnection sequence.|
| | Expect SUT close the connection. |
|  **Requirements Covered**|  |
|  **Cleanup**| N/A|

#### S2_AutoReconnect_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S3_AutoReconnect**| |
|  **Test ID**| S2_AutoReconnect_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case is used to ensure SUT can process the Auto-Reconnection sequence successfully. |
|  **Prerequisites**| SUT supports Auto-Reconnection.|
|  **Test Execution Steps**| Initiate and complete an RDP connection with SUT. In Capability Exchange phase, Test Suite sets the AUTORECONNECT_SUPPORTED (0x0008) flag within the extraFlags field of the General Capability Set in Server Demand Active PDU.|
| | Test Suite expects a Save Session Info PDU with a notification type of either INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10). |
| | Test Suite expects SUT a Save Session Info PDU with a notification type of Logon Info Extended which presents a Server Auto-Reconnect Packet.|
| | Test suite close the connection.|
| | Test suite initiate a new connection and start an Auto-Reconnect sequence. During the reconnection sequence, Test suite sends a Client Auto-Reconnect Packet in the Client Info PDU.|
| | Test suite expects a Save Session Info PDU with a notification type of either INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) again to notify the that the user has logged on .|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S3_Input_PositiveTest_SlowPathInputs

| &#32;| &#32; |
| -------------| ------------- |
|  **S4_SlowPathInput**| |
|  **Test ID**| S3_Input_PositiveTest_SlowPathInputs|
|  **Priority**| P0|
|  **Description** | This test case is used to ensure SUT can process slow-path input message correctly. |
|  **Prerequisites**|  |
|  **Test Execution Steps**| Initiate and complete an RDP connection with SUT. In Capability Exchange phase, Test Suite advised not support fast-path input in TS_INPUT_CAPABILITYSET.|
| | Test suite sends several Client Input Event PDUs, each PDU contains a TS_INPUT_EVENT structure as following:|
| | Synchronize Event|
| | Unused Event|
| | Keyboard Event|
| | Unicode Keyboard Event|
| | Mouse Event|
| | Extended Mouse Event|
| | Test suite sends these Client Input Event PDUs again to make sure the RDP server can process these structures|
| | Test suite close the connection.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S3_Input_PositiveTest_FastPathInputs

| &#32;| &#32; |
| -------------| ------------- |
|  **S5_FastPathInput**| |
|  **Test ID**| S3_Input_PositiveTest_FastPathInputs|
|  **Priority**| P0|
|  **Description** | This test case is used to ensure SUT can process fast-path input message correctly. |
|  **Prerequisites**|  |
|  **Test Execution Steps**| Initiate and complete an RDP connection with SUT. In Capability Exchange phase, Test Suite advised its support fast-path input in TS_INPUT_CAPABILITYSET.|
| | Test suite sends several Client Fast-Path Input Event PDUs, each PDU contains a TS_FP_INPUT_EVENT structure as following:|
| | Fast-Path Keyboard Event|
| | Fast-Path Mouse Event|
| | Fast-Path Extended Mouse Event|
| | Fast-Path Synchronize Event|
| | Fast-Path Unicode Keyboard Event|
| | Test suite sends these Client Fast-path Input Event PDUs again to make sure the RDP server can process these structures|
| | Test suite close the connection.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S4_Output_PositiveTest_FastPathOutput

| &#32;| &#32; |
| -------------| ------------- |
|  **S7_FastPathOutput**| |
|  **Test ID**| S4_Output_PositiveTest_FastPathOutput|
|  **Priority**| P0|
|  **Description** | This test case is used to ensure SUT can send fast-path output message correctly. |
|  **Prerequisites**|  |
|  **Test Execution Steps**| Initiate and complete an RDP connection with SUT. In Capability Exchange phase, Test Suite advised support fast-path output in TS_GENERAL_CAPABILITYSET.|
| | Test suite sends several Client Fast-Path Input Event PDUs, each PDU contains a TS_FP_INPUT_EVENT structure as following:|
| | Fast-Path Keyboard Event|
| | Fast-Path Mouse Event|
| | Fast-Path Extended Mouse Event|
| | Fast-Path Synchronize Event|
| | Fast-Path Unicode Keyboard Event|
| | The sequence and contents of these structure are configured|
| | Test suite wait for 20 secondes, and verify each Server Fast-Path Update PDU received. The Server Fast-Path Update PDU should include:|
| | Fast-Path Orders Update |
| | Fast-Path Bitmap Update|
| | Fast-Path Palette Update|
| | Fast-Path Synchronize Update|
| | Fast-Path System Pointer Hidden Update|
| | Fast-Path System Pointer Default Update|
| | Fast-Path Pointer Position Update|
| | Fast-Path Color Pointer Update|
| | Fast-Path Cached Pointer Update|
| | Fast-Path New Pointer Update|
| | Test suite close the connection.|
| | Test suite logs which structures are verified.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S5_StaticVirtualChannel_PositiveTest_CompressionNotSupported

| &#32;| &#32; |
| -------------| ------------- |
|  **S9_StaticVirtualChannel**| |
|  **Test ID**| S5_StaticVirtualChannel_PositiveTest_CompressionNotSupported|
|  **Priority**| P0|
|  **Description** | This test case is used to verify the uncompressed Static Virtual Channel PDU. |
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate and complete an RDP connection to RDP Server (SUT). In Capability Exchange phase, Test Suite sets the flags field to VCCAPS_NO_COMPR (0x00000000) flag in Virtual Channel Capability Set.|
| | After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of either: the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10). |
| | Trigger SUT to send some Static Virtual Channel PDUs.|
| | Test Suite verifies the received Virtual Channel PDUs and expects these PDUs are valid and not compressed.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S6_AutoDetect_PositiveTest_ConnectTimeAutoDetect

| &#32;| &#32; |
| -------------| ------------- |
|  **S10_ServerRedirection**| |
|  **Test ID**| S6_AutoDetect_PositiveTest_ConnectTimeAutoDetect|
|  **Priority**| P0|
|  **Description** | This test case is used to ensure SUT can complete the optional auto detect phase successfully. |
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase. Verify that the server support auto detect and advise the client’s support of auto detect.|
| | During connect time auto detect phase, test suite expects and verifies a  Server Auto-Detect Request PDU:|
| | If the request contains a RDP_RTT_REQUEST, the test suite reply a Client Auto-Detect Response PDU with RDP_RTT_RESPONSE, then repeat step 2.|
| | If the request contains a RDP_BW_START, the test suite start a bandwidth count, then repeat step 2.|
| | If the request contains a RDP_BW_PAYLOAD, the test suite add bytes to bandwidth count, then repeat step 2.|
| | If the request contains a RDP_BW_STOP, the test suite reply a Client Auto-Detect Response PDU with RDP_BW_RESULTS, then repeat step 2.|
| | If the request contains a RDP_NETCHAR_RESULT, stop the test run.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S7_MultitransportBootstrapping_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S1_Connection**| |
|  **Test ID**| S7_MultitransportBootstrapping_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case is used to ensure SUT can send Initiate Multitransport Request PDU to bootstrap UDP connection. |
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase. Indicate support for both reliable and lossy multitransport in basic setting exchange phase.|
| | Test Suite expects a Server Initiate Multitransport Request PDU with requestedProtocol set to INITITATE_REQUEST_PROTOCOL_UDPFECR (0x01). When received, Test Suite verifies this PDU.|
| | Test Suite expects a Server Initiate Multitransport Request PDU with requestedProtocol set to INITITATE_REQUEST_PROTOCOL_UDPFECL (0x02). When received, Test Suite verifies this PDU.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|

#### S8_HealthMonitoring_PositiveTest

| &#32;| &#32; |
| -------------| ------------- |
|  **S13_HealthMonitoring**| |
|  **Test ID**| S8_HealthMonitoring_PositiveTest|
|  **Priority**| P0|
|  **Description** | This test case is used to verify SUT can send Heartbeat PDU periodically to notify the connection exist. |
|  **Prerequisites**| N/A|
|  **Test Execution Steps**| Initiate and complete an RDP connection to RDP Server (SUT). In basic exchange phase, notify the support of Heartbeat PDU in earlyCapabilityFlags of core data|
| | Test suite expects Server Heartbeat PDU several times, verify the message each received|
| | Calculate the period for Heartbeat PDU sent and log it.|
|  **Requirements Covered**| N/A|
|  **Cleanup**| N/A|


## <a name="_Toc396468176"/>Appendix

### <a name="_Toc396468177"/>Glossary
**SUT**: System under Test. In this spec, it’ indicates the MS-RDPBCGR server implementation.
**Test Suite**: The synthetic RDP client which is used to test against SUT.

### <a name="_Toc396468178"/>Reference

* Technical Document: MS-RDPBCGR.pdf
