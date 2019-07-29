// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("NetcharAutoDetect")]
        [Description(@"This test case is used to ensure the SUT can complete an auto detection for rtt during connection phase")]
        public void S6_AutoDetection_ConnectTime_RTT()
        {

            #region Test Description
            /*
            Test Execution Steps:
            1.	Trigger SUT to initiate an RDP connection, and complete the Connection Initiation phase, 
                Basic Setting Exchange phase, Channel Connection phase,  RDP Security Commencement phase and Secure Setting Exchange Phase.
            2.	Test suite continues to send a Server Auto-Detect Request PDU with RDP_RTT_REQUEST.
            3.	Test suite expects SUT to send a Client Auto-Detect Response PDU with RDP_RTT_RESPONSE.
            4.  Check the received Auto-Detect Response PDU
            */
            #endregion Test Description

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

            #region Channel Connection phase
            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Channel Join Confirm PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Channel Join Confirm PDU to SUT.");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            #region RDP Security Commencement phase
            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }
            #endregion

            #region Secure Setting Exchange phase
            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);
            #endregion 
            
            #region Optional Connect-Time Auto-Detection phase
            ushort sequenceNumber = 0x0;

            //Send a Server Auto-Detect Request PDU with RDP_RTT_REQUEST.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_RTT_REQUEST to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_RTTRequest(++sequenceNumber, true);

            //Expect SUT to send a Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Auto-Detect Response PDU");
            this.rdpbcgrAdapter.WaitForPacket<Client_Auto_Detect_Response_PDU>(waitTime);

            //Check the recieved Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Checking the received Client Auto-Detect Response PDU, expect reponseType is RDP_RTT_RESPONSE, and sequenceNumber is {0}", sequenceNumber);
            this.rdpbcgrAdapter.CheckAutoDetectResponsePdu(AUTO_DETECT_RESPONSE_TYPE.RDP_RTT_RESPONSE, sequenceNumber);
                        
            #endregion

            #endregion
        }


        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("NetcharAutoDetect")]
        [Description(@"This test case is used to ensure the SUT can complete an auto detection for bandwidth during connection phase")]
        public void S6_AutoDetection_ConnectTime_bandwidth()
        {

            #region Test Description
            /*
            Test Execution Steps:
            1.	Trigger SUT to initiate an RDP connection, and complete the Connection Initiation phase, 
                Basic Setting Exchange phase, Channel Connection phase,  RDP Security Commencement phase and Secure Setting Exchange Phase.
            2.	Test suite continues to send a Server Auto-Detect Request PDU with RDP_BW_START.
            3.	Test suite continues to send a Server Auto-Detect Request PDU with RDP_BW_PAYLOAD.
            4.	Test suite continues to send a Server Auto-Detect Request PDU with RDP_BW_STOP.
            5.	Test suite expects SUT to send a Client Auto-Detect Response PDU with RDP_BW_RESULTS
            6.  Check the received Client Auto-Detect Response PDU
            */
            #endregion Test Description

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

            #region Channel Connection phase
            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Channel Join Confirm PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Channel Join Confirm PDU to SUT.");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            #region RDP Security Commencement phase
            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }
            #endregion

            #region Secure Setting Exchange phase
            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);
            #endregion 
            
            #region Optional Connect-Time Auto-Detection phase
            ushort sequenceNumber = 0x01;
            byte[] payload = new byte[payloadLength];

            //Send a Server Auto-Detect Request PDU with RDP_BW_START.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_BW_START to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_BWStart(++sequenceNumber, true);

            //Send some Server Auto-Detect Request PDU with RDP_BW_PAYLOAD.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending {0} Server Auto-Detect Request PDU with RDP_BW_PAYLOAD to SUT, and the payloadLength is {1}", payloadNum, payloadLength);
            for (int i = 0; i < payloadNum; i++)
                this.rdpbcgrAdapter.SendAutoDetectRequestPdu_BWPayload(++sequenceNumber, payloadLength, payload);

            //Send a Server Auto-Detect Request PDU with RDP_BW_STOP.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_BW_STOP to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_BWStop(++sequenceNumber, payloadLength, payload, true);

            //Expect SUT to send a Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Auto-Detect Response PDU");
            this.rdpbcgrAdapter.WaitForPacket<Client_Auto_Detect_Response_PDU>(waitTime);

            //Check the recieved Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Checking the received Client Auto-Detect Response PDU, expect reponseType is RDP_BW_RESULTS_DURING_CONNECT, and sequenceNumber is {0}", sequenceNumber);
            this.rdpbcgrAdapter.CheckAutoDetectResponsePdu(AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_DURING_CONNECT, sequenceNumber);

            
            #endregion

            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("NetcharAutoDetect")]
        [Description(@"This test case is used to ensure the SUT can complete an auto detection for rtt after connection phase")]
        public void S6_AutoDetection_Continuous_RTT()
        {

            #region Test Description
            /*
            Test Execution Steps:
            1.	Trigger SUT to initiate and complete an RDP connection.
            2.	Test suite continues to send a TS_SECURITY_HEADER with RDP_RTT_REQUEST.
            3.	Test suite expects SUT to send a Client Auto-Detect Response PDU with RDP_RTT_RESPONSE.
            4.  Check the received Client Auto-Detect Response PDU.
            */
            #endregion Test Description

            #region Test Sequence

            #region Connect Phase
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);
            
            #endregion

            #region Continuous Auto-Detection phase

            ushort sequenceNumber = 0x0;

            //Send a Server Auto-Detect Request PDU with RDP_RTT_REQUEST.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_RTT_REQUEST to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_RTTRequest(++sequenceNumber, false);

            //Expect SUT to send a Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Auto-Detect Response PDU");
            this.rdpbcgrAdapter.WaitForPacket<Client_Auto_Detect_Response_PDU>(waitTime);

            //Check the recieved Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Checking the received Client Auto-Detect Response PDU, expect reponseType is RDP_RTT_RESPONSE, and sequenceNumber is {0}", sequenceNumber);
            this.rdpbcgrAdapter.CheckAutoDetectResponsePdu(AUTO_DETECT_RESPONSE_TYPE.RDP_RTT_RESPONSE, sequenceNumber);

            
            #endregion
            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("NetcharAutoDetect")]
        [Description(@"This test case is used to ensure the SUT can complete an auto detection for bandwidth after connection phase")]
        public void S6_AutoDetection_Continuous_bandwidth()
        {

            #region Test Description
            /*
            Test Execution Steps:
            1.	Trigger SUT to initiate and complete an RDP connection.
            2.	Test suite continues to send a TS_SECURITY_HEADER with RDP_BW_START.
            3.	Test suite continues to send a TS_SECURITY_HEADER with RDP_BW_PAYLOAD.
            4.	Test suite continues to send a TS_SECURITY_HEADER with RDP_BW_STOP.
            5.	Test suite expects SUT to send a Client Auto-Detect Response PDU with RDP_BW_RESULTS.
            6.  Check the received Client Auto-Detect Response PDU.
            */
            #endregion Test Description

            #region Test Sequence

            #region Connect Phase
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            #region Continuous Auto-Detection phase
            ushort sequenceNumber = 0x01;

            //Send a Server Auto-Detect Request PDU with RDP_BW_START.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_BW_START to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_BWStart(++sequenceNumber, false);

            //Can send some PDU here, TBD

            //Send a Server Auto-Detect Request PDU with RDP_BW_STOP.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_BW_STOP to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_BWStop(++sequenceNumber, 0, null, false);

            //Expect SUT to send a Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Auto-Detect Response PDU");
            this.rdpbcgrAdapter.WaitForPacket<Client_Auto_Detect_Response_PDU>(waitTime);

            //Check the recieved Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Checking the received Client Auto-Detect Response PDU, expect reponseType is RDP_BW_RESULTS_AFTER_CONNECT, and sequenceNumber is {0}", sequenceNumber);
            this.rdpbcgrAdapter.CheckAutoDetectResponsePdu(AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT, sequenceNumber);


            #endregion

            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("NetcharAutoDetect")]
        [TestCategory("AutoReconnect")]
        [Description(@"This test case is used to ensure the SUT can Sync its network characteristic with the server after autoReconnection")]
        public void S6_AutoDetection_NetworkCharacteristicsSync_After_AutoReconnect()
        {

            #region Test Description
            /*
            Test Execution Steps:
            1.	Trigger SUT to initiate and complete an RDP connection. During Connect-Time Auto-Detection phase, 
                detect RTT and bandwidth, and send Auto-Detect Request PDU with RDP_NETCHAR_RESULT.
            2.	Trigger SUT to start an Auto-Reconnect sequence. This can be implemented by creating a short-term network failure on client side.
            3.	In the Connect-Time Auto-Detection phase of the connect sequence, Test suite send a Server Auto-Detect Request PDU with RDP_RTT_REQUEST.
            4.	Test suite expects SUT to send a  Client Auto-Detect Response PDU with RDP_BW_RESULTS with RDP_NETCHAR_SYNC.
            5.	Check the received Client Auto-Detect Response PDU.
            */
            #endregion Test Description

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region first connection phase

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

            #region Channel Connection phase
            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Channel Join Confirm PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Channel Join Confirm PDU to SUT.");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            #region RDP Security Commencement phase
            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }
            #endregion

            #region Secure Setting Exchange phase
            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);
            #endregion

            #region Optional Connect-Time Auto-Detection phase
            ushort sequenceNumber = 0x01;
            byte[] payload = new byte[payloadLength];

            //Send a Server Auto-Detect Request PDU with RDP_RTT_REQUEST.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_RTT_REQUEST to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_RTTRequest(++sequenceNumber, true);

            //Expect SUT to send a Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Auto-Detect Response PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_Auto_Detect_Response_PDU>(waitTime);

            //Send a Server Auto-Detect Request PDU with RDP_BW_START.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_BW_START to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_BWStart(++sequenceNumber, true);

            //Send some Server Auto-Detect Request PDU with RDP_BW_PAYLOAD.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending {0} Server Auto-Detect Request PDU with RDP_BW_PAYLOAD to SUT, and the payloadLength is {1}", payloadNum, payloadLength);
            for (int i = 0; i < payloadNum; i++)
                this.rdpbcgrAdapter.SendAutoDetectRequestPdu_BWPayload(++sequenceNumber, payloadLength, payload);

            //Send a Server Auto-Detect Request PDU with RDP_BW_STOP.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_BW_STOP to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_BWStop(++sequenceNumber, payloadLength, payload, true);

            //Expect SUT to send a Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Auto-Detect Response PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_Auto_Detect_Response_PDU>(waitTime);

            //Send a Server Auto-Detect Request PDU with RDP_NETCHAR_RESULT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_NETCHAR_RESULT to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_NetcharResult(++sequenceNumber);
            
            
            #endregion

            #region Licensing phase
            //Send SUT a Server License Error Pdu - Valid Client.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server License Error Pdu - Valid Client to SUT.");
            this.rdpbcgrAdapter.Server_License_Error_Pdu_Valid_Client(NegativeType.None);
            #endregion

            #region Capabilities Exchange phase
            //Set server capability and set supportServerBitmapCacheHost to false
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(false, true, true, true, true, true, true, true, true, true);
            //Send a Server Demand Active PDU to SUT and advertise the support for the Bitmap Host Cache Support Capability Set.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT and does not advertise the support for the Bitmap Host Cache Support Capability Set.");
            this.rdpbcgrAdapter.Server_Demand_Active(NegativeType.None);


            //Expect SUT respond a Client Confirm Active PDU.
            //Once the Confirm Active PDU has been sent, the client can start sending input PDUs (see section 2.2.8) to the server.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Confirm Active PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Confirm_Active_Pdu>(waitTime);
            #endregion

            #region Connection Finalization phase
            //Expect SUT send a Client Synchronize PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Synchronize PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Synchronize_Pdu>(waitTime);

            //Send a Server Synchronize PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Synchronize PDU to SUT.");
            this.rdpbcgrAdapter.ServerSynchronize();

            //Send a Server Control Cooperate PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Cooperate PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlCooperate();

            //Expect SUT send a Client Control PDU - Cooperate.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Cooperate.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Cooperate>(waitTime);

            //Expect SUT send a Client Control PDU - Request Control.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Control PDU - Request Control.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Control_Pdu_Request_Control>(waitTime);

            //Respond a Server Control Granted Control PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Control Granted Control PDU to SUT.");
            this.rdpbcgrAdapter.ServerControlGrantedControl();

            ////Expect a Client Persistent Key List PDU.
            //this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Persistent Key List PDU.");
            //this.rdpbcgrAdapter.WaitForPacket<Client_Persistent_Key_List_Pdu>(waitTime);

            //Expect a Client Font List PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Client Font List PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Font_List_Pdu>(waitTime);

            //Send a Server Font Map PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Font Map PDU to SUT.");
            this.rdpbcgrAdapter.ServerFontMap();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT with Auto-Reconnect information.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.AutoReconnectCookie, ErrorNotificationType_Values.LOGON_FAILED_OTHER);
            #endregion

            #endregion

            #region Auto-Reconnect Sequence

            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to start an Auto-Reconnect.");
            int iResult = this.sutControlAdapter.TriggerClientAutoReconnect(this.TestContext.TestName);
            TestSite.Assume.IsTrue(iResult >= 0, "SUT Control Adapter: TriggerClientAutoReconnect should be successful: {0}.", iResult);

            //Expect the transport layer connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.AutoReconnection);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: true", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, TS_UD_SC_CORE_version_Values.V2, NegativeType.None);
            #endregion

            #region Channel Connection phase
            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Channel Join Confirm PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Channel Join Confirm PDU to SUT.");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join sequence.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.None);
            #endregion

            #region RDP Security Commencement phase
            //Expects SUT continue the connection by sending a Client Security Exchange PDU 
            //(if Standard RDP Security mechanisms are being employed) or a Client Info PDU.
            if (transportProtocol == EncryptedProtocol.Rdp)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Security Exchange PDU.");
                this.rdpbcgrAdapter.ExpectPacket<Client_Security_Exchange_Pdu>(waitTime);
            }
            #endregion

            #region Secure Setting Exchange phase
            //Expect a Client Info PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send Client a Client Info PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_Info_Pdu>(waitTime);
            #endregion

            #region Optional Connect-Time Auto-Detection phase
            sequenceNumber = 0x0;


            //Send a Server Auto-Detect Request PDU with RDP_RTT_REQUEST.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Auto-Detect Request PDU with RDP_RTT_REQUEST to SUT");
            this.rdpbcgrAdapter.SendAutoDetectRequestPdu_RTTRequest(sequenceNumber, true);

            //Expect SUT to send a Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Auto-Detect Response PDU");
            this.rdpbcgrAdapter.WaitForPacket<Client_Auto_Detect_Response_PDU>(waitTime);
            
            //Check the recieved Client Auto-Detect Response PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Checking the received Client Auto-Detect Response PDU, expect reponseType is RDP_NETCHAR_SYNC, and sequenceNumber is {0}", sequenceNumber);
            this.rdpbcgrAdapter.CheckAutoDetectResponsePdu(AUTO_DETECT_RESPONSE_TYPE.RDP_NETCHAR_SYNC, sequenceNumber);

            #endregion

            #endregion
            #endregion
        }

    }
}
