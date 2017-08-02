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
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case is used to verify 
                      SUT drops the connection if the length field within tpktHeader in MCS Attach User Confirm PDU is not consistent with the received data.")]
        public void S1_ConnectionTest_ChannelConnection_NegativeTest_MCSAttachUserConfirm_InvalidTPKTLength()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase and Basic Setting Exchange phase.
            //2.	Test Suite expects SUT continue the connection with sending a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.
            //3.	Test Suite responds a Server MCS Attach User Confirm PDU to SUT and set the length field of tpktHeader to an invalid value (less than 11).
            //4.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT with invalid TPK header length");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.InvalidTPKLength);
                        
            RDPClientTryDropConnection("MCS Attach User Confirm PDU with inconsistent TPK header length");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case is used to verify 
                      SUT drops the connection connection if the result field in MCS Attach User Confirm PDU is not set to rt-successful (0).")]
        public void S1_ConnectionTest_ChannelConnection_NegativeTest_MCSAttachUserConfirm_Failure()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase and Basic Setting Exchange phase.
            //2.	Test Suite expects SUT continue the connection with sending a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.
            //3.	Test Suite responds a Server MCS Attach User Confirm PDU to SUT and set the result field in MCS Attach User Confirm PDU is not set to rt-successful (0).
            //4.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT with invalid result value (not rt-successful)");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.InvalidResult);

            //Expect SUT to drop the connection
            RDPClientTryDropConnection("MCS Attach User Confirm PDU with invalid rt-successful(0)");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case is used to verify 
                      SUT drops the connection connection if the initiator field in MCS Attach User Confirm PDU is not present")]
        public void S1_ConnectionTest_ChannelConnection_NegativeTest_MCSAttachUserConfirm_InitiatorNotPresent()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase and Basic Setting Exchange phase.
            //2.	Test Suite expects SUT continue the connection with sending a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.
            //3.	Test Suite responds a Server MCS Attach User Confirm PDU to SUT and does not present initiator field.
            //4.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT and does not present initiator field");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.InvalidInitiatorField);

            //Expect SUT to drop the connection
            RDPClientTryDropConnection("MCS Attach User Confirm PDU without the initiator field");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case is used to verify 
                      SUT drops the connection if the length field within tpktHeader in MCS Channel Join Confirm PDU is not consistent with the received data.")]
        public void S1_ConnectionTest_ChannelConnection_NegativeTest_MCSChannelJoinConfirm_InvalidTPKTLength()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase and Basic Setting Exchange phase.
            //2.	Test Suite expects SUT continue the connection with sending a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.
            //3.	Test Suite verifies the received Client MCS Erect Domain Request PDU and Client MCS Attach User Request PDU, and then responds a Server MCS Attach User Confirm PDU to SUT.
            //4.	Test Suite expects SUT starts the channel join sequence. SUT should use the MCS Channel Join Request PDU to join the user channel obtained from the Attach User Confirm PDU, the I/O channel and all of the static virtual channels obtained from the Server Network Data structure.
            //5.	After Test Suite received the first MCS Channel Join Request PDU, it responds a Server MCS Channel Join Confirm PDU and set the length field within tpktHeader to an invalid value (less than the actual value).
            //6.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join request and respond a pdu with the length field within tpktHeader to an invalid value (less than the actual value) .");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.InvalidTPKLength);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case is used to verify 
                      SUT drops the connection if the channelId field is not present in MCS Channel Join Confirm PDU.")]
        public void S1_ConnectionTest_ChannelConnection_NegativeTest_MCSChannelJoinConfirm_ChannelIdNotPresent()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase and Basic Setting Exchange phase.
            //2.	Test Suite expects SUT continue the connection with sending a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.
            //3.	Test Suite verifies the received Client MCS Erect Domain Request PDU and Client MCS Attach User Request PDU, and then responds a Server MCS Attach User Confirm PDU to SUT.
            //4.	Test Suite expects SUT starts the channel join sequence. SUT should use the MCS Channel Join Request PDU to join the user channel obtained from the Attach User Confirm PDU, the I/O channel and all of the static virtual channels obtained from the Server Network Data structure.
            //5.	After Test Suite received the first MCS Channel Join Request PDU, it responds a Server MCS Channel Join Confirm PDU and does not present the channelId field.
            //6.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join request and respond a pdu without presenting the channelId field.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.InvalidEmptyChannelIdField);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case is used to verify 
                      SUT drops the connection if the result field is not set to rt-successful (0) in MCS Channel Join Confirm PDU")]
        public void S1_ConnectionTest_ChannelConnection_NegativeTest_MCSChannelJoinConfirm_Failure()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase and Basic Setting Exchange phase.
            //2.	Test Suite expects SUT continue the connection with sending a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.
            //3.	Test Suite verifies the received Client MCS Erect Domain Request PDU and Client MCS Attach User Request PDU, and then responds a Server MCS Attach User Confirm PDU to SUT.
            //4.	Test Suite expects SUT starts the channel join sequence. SUT should use the MCS Channel Join Request PDU to join the user channel obtained from the Attach User Confirm PDU, the I/O channel and all of the static virtual channels obtained from the Server Network Data structure.
            //5.	After Test Suite received the first MCS Channel Join Request PDU, it responds a Server MCS Channel Join Confirm PDU and does not set the result field to rt-successful (0).
            //6.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join request and respond a pdu without setting result field to rt-successful.");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.InvalidResult);

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case is used to verify 
                      SUT drops the connection if the channelId field is not valid in MCS Channel Join Confirm PDU.")]
        public void S1_ConnectionTest_ChannelConnection_NegativeTest_MCSChannelJoinConfirm_InvalidChannelId()
        {
            #region Test Description
            //1.	Trigger SUT to initiate a RDP connection and complete the Connection Initiation phase and Basic Setting Exchange phase.
            //2.	Test Suite expects SUT continue the connection with sending a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.
            //3.	Test Suite verifies the received Client MCS Erect Domain Request PDU and Client MCS Attach User Request PDU, and then responds a Server MCS Attach User Confirm PDU to SUT.
            //4.	Test Suite expects SUT starts the channel join sequence. SUT should use the MCS Channel Join Request PDU to join the user channel obtained from the Attach User Confirm PDU, the I/O channel and all of the static virtual channels obtained from the Server Network Data structure.
            //5.	After Test Suite received the first MCS Channel Join Request PDU, it responds a Server MCS Channel Join Confirm PDU and set the channelId field to an invalid value that not correspond with the value of the channelId field received in MCS Channel Join Request PDU.
            //6.	Test Suite expects SUT drop the connection.
            #endregion

            #region Test Implementation
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

            //Expect a Client MCS Erect Domain Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Erect Domain Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Erect_Domain_Request>(waitTime);

            //Expect a Client MCS Attach User Request PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Attach User Request PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Attach_User_Request>(waitTime);

            //Respond a Server MCS Attach User Confirm PDU
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Attach User Confirm PDU to SUT");
            this.rdpbcgrAdapter.MCSAttachUserConfirm(NegativeType.None);

            //Expect SUT start a channel join sequence
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to start the  channel join request and respond a pdu with the channelId field to an invalid value that not correspond with the value of the channelId field received in MCS Channel Join Request PDU..");
            this.rdpbcgrAdapter.ChannelJoinRequestAndConfirm(NegativeType.InvalidMismatchChannelIdField);

            #endregion
        }
    }
}
