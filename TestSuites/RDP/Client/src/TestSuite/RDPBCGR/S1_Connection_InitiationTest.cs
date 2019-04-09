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
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case verifies that SUT drops the connection once receiving Server X.224 Connection confirm PDU with rdpNegData field of a valid RDP Negotiation Failure structure from RDP server.")]
        public void S1_ConnectionTest_ConnectionInitiation_NegativeTest_RDPNegotiationFailure()
        {
            #region Test Steps
            //1.	Trigger SUT to initiate a RDP connection with sending a Client X.224 Connection Request PDU.
            //2.	Test Suite responds a Server X.224 Connection Confirm PDU and set the rdpNegData field to a valid RDP Negotiation Failure structure.
            //3.	Test Suite expects SUT drop the connection.
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

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Negotiate Failure Confirm PDU and set the rdpNegData field to a valid RDP Negotiation Failure structure.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT with fail reason {0}", failureCode_Values.INCONSISTENT_FLAGS);
            this.rdpbcgrAdapter.Server_X_224_Negotiate_Failure(failureCode_Values.INCONSISTENT_FLAGS);

            RDPClientTryDropConnection("invalid Server X.224 Negotiate Failure Confirm PDU");
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT drops the connection once receiving a Server X.224 Connection confirm PDU with an invalid TPKT header from RDP server.")]
        public void S1_ConnectionTest_ConnectionInitiation_NegativeTest_InvalidTPKTHeader()
        {
            #region Test Steps
            //1.	Trigger SUT to initiate a RDP connection with sending a Client X.224 Connection Request PDU.
            //2.	Test Suite responds a Server X.224 Connection Confirm PDU and set the version of TPKT header to an invalid value (non 3).
            //3.	Test Suite expects SUT drop the connection.
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

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Confirm PDU with invalid TPKTHeader.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT with invalid TPKTHeader" );
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.InvalidTPKTHeader);

            RDPClientTryDropConnection("Server X.224 Confirm PDU with invalid TPKTHeader");            
            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]        
        [Description(@"This test case ensures SUT drop the connection when received a Server X.224 Connection Confirm PDU with an invalid RDP Negotiation Data..")]
        public void S1_ConnectionTest_ConnectionInitiation_NegativeTest_InvalidRDPNegData()
        {
            #region Test Steps
            //1.	Trigger SUT to initiate a RDP connection with sending a Client X.224 Connection Request PDU.
            //2.	Test Suite responds a Server X.224 Connection Confirm PDU and set the rdpNegData field to an invalid structure (neither Negotiation Response structure nor RDP Negotiation Failure structure).
            //3.	Test Suite expects SUT drop the connection.
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

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Confirm PDU with invalid TPKTHeader.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT with invalid rdpNegData");
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.InvalidRdpNegData);

            RDPClientTryDropConnection("invalid structure of rdpNegData field in Server X.224 Connection Confirm PDU");            
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT support PROTOCOL_HYBRID_EX security protocol.")]
        public void S1_ConnectionTest_ConnectionInitiation_PositiveTest_HybridEXSelected()
        {
            #region Test Steps
            //1.	Trigger SUT to initiate a RDP connection with sending a Client X.224 Connection Request PDU.
            //2.	Test Suite responds a Server X.224 Connection Confirm PDU and set the requestedProtocols field to PROTOCOL_HYBRID_EX.
            //3.	Test Suite and RDP client complete CredSSP security headshake.
            //4.    Test Suite send Early User Authorization Result PDU if it's Negotiation-Based.
            //5.    Test Suite and RDP client complete the subsequent connection phase.
            #endregion

            #region Test Implementation
            if (transportProtocol != EncryptedProtocol.NegotiationCredSsp && transportProtocol != EncryptedProtocol.DirectCredSsp)
            {
                this.TestSite.Assert.Inconclusive("This case requires using CredSSP encrypted protocol");
            }
            this.selectedProtocol = selectedProtocols_Values.PROTOCOL_HYBRID_EX;
            

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to initiate a RDP connection
            //Trigger client to initiate a RDP connection.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            #region Connection Initiation phase
            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU and set the EXTENDED_CLIENT_DATA_SUPPORTED flag.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: PROTOCOL_HYBRID_EX; Extended Client Data supported: true");
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None);
            #endregion Connection Initiation phase

            #region Send Early User Authorization Result PDU
            if (transportProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                // For Negotiation based CredSSP, send Early User Authorization Result PDU after Connection Initiation phase and CredSSP handshake 
                this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Early User Authorization Result PDU");
                this.rdpbcgrAdapter.SendEarlyUserAuthorizationResultPDU(Authorization_Result_value.AUTHZ_SUCCESS);
            }
            #endregion Send Early User Authorization Result PDU

            #region Basic Setting Exchange phase
            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            //Respond a Server MCS Connect Response PDU with GCC Conference Create Response.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server MCS Connect Response PDU to SUT. Encryption Method {0}; Encryption Level: {1}; RDP Version Code: {2}.", enMethod.ToString(), enLevel.ToString(), TS_UD_SC_CORE_version_Values.V2.ToString());
            this.rdpbcgrAdapter.Server_MCS_Connect_Response(enMethod, enLevel, rdpServerVersion, NegativeType.None);
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

            //Expect SUT start a channel join sequence.
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

            #region Licensing phase
            //Send SUT a Server License Error Pdu - Valid Client.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server License Error Pdu - Valid Client to SUT.");
            this.rdpbcgrAdapter.Server_License_Error_Pdu_Valid_Client(NegativeType.None);
            #endregion

            #region Capabilities Exchange phase
            //Set server capability.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);
            //Send a Server Demand Active PDU to SUT and advertise the support for the Bitmap Host Cache Support Capability Set.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Demand Active PDU to SUT and advertise the support for the Bitmap Host Cache Support Capability Set.");
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


            if (this.isclientSupportPersistentBitmapCache)
            {
                //Expect a Client Persistent Key List PDU.
                this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Persistent Key List PDU.");
                this.rdpbcgrAdapter.WaitForPacket<Client_Persistent_Key_List_Pdu>(waitTime);
            }

            //Expect a Client Font List PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Client Font List PDU.");
            this.rdpbcgrAdapter.WaitForPacket<Client_Font_List_Pdu>(waitTime);

            //Send a Server Font Map PDU to SUT.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Server Font Map PDU to SUT.");
            this.rdpbcgrAdapter.ServerFontMap();

            #endregion

            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the valid RDP_NEG_RSP with supporting all protocols correctly.")]
        public void S1_ConnectionTest_ConnectionInitiation_PositiveTest_FlagsOfRdpNegRsp_SupportAllProtocols()
        {
            #region Test Description
            /* 
             This test case tests:
             1) The correctness of Client X.224 Connection Request PDU.
             2) SUT can process the valid Server X.224 Connection Confirm PDU 
                with flags (EXTENDED_CLIENT_DATA_SUPPORTED | DYNVC_GFX_PROTOCOL_SUPPORTED | RESTRICTED_ADMIN_MODE_SUPPORTED) in RDP_NEG_RSP correctly.

             Test Execution Steps:
             1.	Trigger SUT to initiate a RDP connection with sending a Client X.224 Connection Request PDU.
             2.	Verify the received Client X.224 Connection Request PDU and respond a valid Server X.224 Connection Confirm PDU.
                Server should set flags = EXTENDED_CLIENT_DATA_SUPPORTED | DYNVC_GFX_PROTOCOL_SUPPORTED | RESTRICTED_ADMIN_MODE_SUPPORTED in RDP_NEG_RSP.
             3.	Test Suite expects SUT continue the connection sequence with sending a Client MCS Connect Initial PDU 
                with GCC Conference Create Request. 
            */
            #endregion

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

            //Expect SUT send a Client X.224 Connection Request PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            //Respond a Server X.224 Connection Confirm PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT. Selected protocol: {0}; Extended Client Data supported: false; RDP_NEG_RSP_flags: All except the NEGRSP_FLAG_RESERVED.", selectedProtocol.ToString());
            this.rdpbcgrAdapter.Server_X_224_Connection_Confirm(selectedProtocol, true, true, NegativeType.None, true, true, false);

            //Expect SUT send Client MCS Connect Initial PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client MCS Connect Initial PDU.");
            this.rdpbcgrAdapter.ExpectPacket<Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request>(waitTime);

            #endregion
        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case verifies that SUT can process the valid RDP_NEG_FAILURE correctly.")]
        public void S1_ConnectionTest_ConnectionInitiation_PositiveTest_RDPNegotiationFailure()
        {
            #region Test Steps
            /*
                1). Trigger SUT to initiate a RDP connection with sending a Client X.224 Connection Request PDU.
                2). Test Suite responds a Server X.224 Connection Confirm PDU and set the failureCode of RDP_NEG_FAILURE field according the requestedProtocols of Client X.224 Connection Request PDU.
                3). Test Suite expects SUT drop the connection.      
             */
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

            //Expect the transport layer connection request
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to start a transport layer connection request (TCP).");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Expect SUT send a Client X.224 Connection Request PDU.
            //this.rdpbcgrAdapter.SetNeedStoreReceivePdu();
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client X.224 Connection Request PDU");
            this.rdpbcgrAdapter.ExpectPacket<Client_X_224_Connection_Request_Pdu>(waitTime);

            failureCode_Values failureCode = GetFailureCode_RDP_NEG_FAILURE(this.rdpbcgrAdapter.SessionContext.X224ConnectionReqPdu);
            if (failureCode == failureCode_Values.NO_FAILURE)
            {
                // TODO: Error
            }

            //Respond a Server X.224 Negotiate Failure Confirm PDU and set the rdpNegData field to a valid RDP Negotiation Failure structure.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server X.224 Connection Confirm PDU to SUT with fail reason {0}", failureCode.ToString());
            this.rdpbcgrAdapter.Server_X_224_Negotiate_Failure(failureCode);

            RDPClientTryDropConnection("invalid RDP Negotiation Failure structure in Server X.224 Connection Confirm PDU");
            #endregion
        }
    }
}
