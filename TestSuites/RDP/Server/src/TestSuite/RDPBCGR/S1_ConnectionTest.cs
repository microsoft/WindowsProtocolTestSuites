// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpele;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify that RDP server can correctly process Client X.224 Connection Request PDU and response a correct Server X.224 Connection Confirm PDU")]
        public void S1_Connection_ConnectionInitiation_PositiveTest()
        {
            #region Test Steps
            // 1. Establish transport connection with RDP Server (SUT)
            // 2. Send a Client X.224 Connection Request PDU to SUT.
            // 3. Waite for Server X.224 Connection Confirm PDU with RDP_NEG_RSP structure from SUT.
            // 4. Verify the received Server X.224 Connection Confirm PDU.
            #endregion Test Steps

            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client X.224 Connection Request PDU with all security protocols supported to SUT.");
            rdpbcgrAdapter.SendClientX224ConnectionRequest(NegativeType.None, testConfig.requestProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server X224 Connection Confirm with RDP Negotiation Response.");
            Server_X_224_Connection_Confirm_Pdu confirmPdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
            if (confirmPdu == null)
            {
                this.Site.Log.Add(LogEntryKind.Comment, "Timeout when receiving Server X224 Connection Confirm with RDP Negotiation Response, try to receive Server X.224 Connection Confirm PDU with RDP Negotiation Failure");
                Server_X_224_Negotiate_Failure_Pdu negotiateFailurePdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Negotiate_Failure_Pdu>(testConfig.timeout);
                this.Site.Assert.IsNotNull(negotiateFailurePdu, "RDP Server MUST response a Server X224 Connection Confirm PDP after receiving a Client X224 Connection Request PDU.");

                if (negotiateFailurePdu.rdpNegFailure.failureCode == failureCode_Values.SSL_NOT_ALLOWED_BY_SERVER)
                {
                    this.Site.Log.Add(LogEntryKind.Comment, "Received a Server X.224 Connection Confirm PDU with RDP Negotiation Failure, whose failureCode field is SSL_NOT_ALLOWED_BY_SERVER. Reconnect to the RDP Server and send Client X.224 Connection Request PDU only support RDP protocol.");
                    rdpbcgrAdapter.Disconnect();

                    testConfig.transportProtocol = EncryptedProtocol.Rdp;

                    rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

                    testConfig.requestProtocol = requestedProtocols_Values.PROTOCOL_RDP_FLAG;

                    rdpbcgrAdapter.SendClientX224ConnectionRequest(NegativeType.None, testConfig.requestProtocol);
                    this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server X224 Connection Confirm with RDP Negotiation Response.");
                    confirmPdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
                    this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server X224 Connection Confirm PDP after receiving a Client X224 Connection Request PDU.");
                }
                else
                {
                    Site.Assert.Fail("Received a Server X.224 Connection Confirm PDU with RDP Negotiation Failure, whose failureCode field is {0}.", negotiateFailurePdu.rdpNegFailure.failureCode);
                }

            }

            #endregion Test Code
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify that the SUT can process the valid Client MCS Connect Initial PDU with GCC Conference Create Request correctly and response a valid Server MCS Connect Response PDU with GCC Conference Create Response.")]
        public void S1_Connection_BasicSettingExchange_PositiveTest()
        {
            #region Test Steps
            // 1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase.
            // 2. Test Suite continues the connection sequence by sending a valid Client MCS Connect Initial PDU with GCC Conference Create Request.
            // 3. The test suite expect a Server MCS Connect Response PDU with GCC Conference Create Response.
            // 4. The test suite verifies the received Server MCS Connect Response PDU with GCC Conference Create Response.
            #endregion Test Steps

            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            #region Connection Initiation

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client X.224 Connection Request PDU to SUT, supported security protocol is {0}.", testConfig.requestProtocol.ToString());
            rdpbcgrAdapter.SendClientX224ConnectionRequest(NegativeType.None, testConfig.requestProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server X224 Connection Confirm.");
            Server_X_224_Connection_Confirm_Pdu confirmPdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server X224 Connection Confirm PDU after receiving a Client X224 Connection Request PDU.");

            #endregion Connection Initiation

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            bool supportMultitransportReliable = false;
            bool supportMultitransportLossy = false;
            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Connect Initial PDU with GCC Conference Create Request to SUT, supportMultitransportReliable is {0}, supportMultitransportLossy is {1}.", supportMultitransportReliable, supportMultitransportLossy);
            rdpbcgrAdapter.SendClientMCSConnectInitialPDU(NegativeType.None, SVCNames, false, false, false, supportMultitransportReliable, supportMultitransportLossy, false);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Connect Response PDU with GCC Conference Create Response.");
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response response = rdpbcgrAdapter.ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server MCS Connect Response after receiving a Client MCS Connect Initial PDU.");

            #endregion Test Code
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify that SUT can process Channel Connection phase correctly.")]
        public void S1_Connection_ChannelConnection_PositiveTest()
        {
            #region Test Steps
            //1. Initiate an RDP connection to SUT and complete the Connection Initiation phase and Basic Setting Exchange phase.
            //2. Test Suite sends a Client MCS Erect Domain Request PDU and a Client MCS Attach User Request PDU.
            //3. Test Suite expects a Server MCS Attach User Confirm PDU from SUT.
            //4. Test Suite verifies the received MCS Attach User Confirm PDU.
            //5. Test Suite start the channel join sequence. Test suite use the MCS Channel Join Request PDU to join the user channel obtained from the Attach User Confirm PDU, the I/O channel and all of the static virtual channels obtained from the Server Network Data structure.
            //6. Test Suite expects and verifies a Server MCS Channel Join Confirm PDU respectively for each MCS Channel Join Request PDU.
            //7. After all the channels created, the Test suite close the connection.
            #endregion Test Steps

            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            #region Connection Initiation

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client X.224 Connection Request PDU to SUT, supported security protocol is {0}.", testConfig.requestProtocol.ToString());
            rdpbcgrAdapter.SendClientX224ConnectionRequest(NegativeType.None, testConfig.requestProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server X224 Connection Confirm.");
            Server_X_224_Connection_Confirm_Pdu confirmPdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server X224 Connection Confirm PDU after receiving a Client X224 Connection Request PDU.");

            #endregion Connection Initiation

            #region Basic Setting Exchange

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            bool supportMultitransportReliable = false;
            bool supportMultitransportLossy = false;
            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Connect Initial PDU with GCC Conference Create Request to SUT, supportMultitransportReliable is {0}, supportMultitransportLossy is {1}.", supportMultitransportReliable, supportMultitransportLossy);
            rdpbcgrAdapter.SendClientMCSConnectInitialPDU(NegativeType.None, SVCNames, false, false, false, supportMultitransportReliable, supportMultitransportLossy, false);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Connect Response PDU with GCC Conference Create Response.");
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response response = rdpbcgrAdapter.ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server MCS Connect Response after receiving a Client MCS Connect Initial PDU.");

            #endregion Basic Setting Exchange

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Erect Domain Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSErectDomainRequest(NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Attach User Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSAttachUserRequest(NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Attach User Confirm PDU.");
            Server_MCS_Attach_User_Confirm_Pdu attachuserConfirm = rdpbcgrAdapter.ExpectPacket<Server_MCS_Attach_User_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(attachuserConfirm, "RDP Server MUST response a Server MCS Attach User Confirm PDU after receiving a Client MCS Attach User Request PDU.");

            this.Site.Log.Add(LogEntryKind.Comment, "The test suite proceeds to join the user channel, the input/output (I/O) channel, and all of the static virtual channels.");
            rdpbcgrAdapter.ChannelJoinRequestAndConfirm();

            #endregion Test Code
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify SUT can process RDP Security Commencement phase, Secure Setting Exchange phase and Licensing phase.")]
        public void S1_Connection_SecurityExchange_PositiveTest()
        {
            #region Test Steps
            //1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, and Channel Connection phase.
            //2. If Standard RDP Security mechanisms are being employed, Test Suite sends a Client Security Exchange PDU to SUT.
            //3. Test Suite sends SUT a Client Info PDU to SUT.
            //4. Test Suite expects a Server License Error PDU from SUT.
            //5.Test Suite verifies the Server License Error PDU received.
            #endregion Test Steps

            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            #region Connection Initiation

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client X.224 Connection Request PDU to SUT, supported security protocol is {0}.", testConfig.requestProtocol.ToString());
            rdpbcgrAdapter.SendClientX224ConnectionRequest(NegativeType.None, testConfig.requestProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server X224 Connection Confirm.");
            Server_X_224_Connection_Confirm_Pdu confirmPdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server X224 Connection Confirm PDU after receiving a Client X224 Connection Request PDU.");

            #endregion Connection Initiation

            #region Basic Setting Exchange

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            bool supportMultitransportReliable = false;
            bool supportMultitransportLossy = false;
            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Connect Initial PDU with GCC Conference Create Request to SUT, supportMultitransportReliable is {0}, supportMultitransportLossy is {1}.", supportMultitransportReliable, supportMultitransportLossy);
            rdpbcgrAdapter.SendClientMCSConnectInitialPDU(NegativeType.None, SVCNames, false, false, false, supportMultitransportReliable, supportMultitransportLossy, false);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Connect Response PDU with GCC Conference Create Response.");
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response response = rdpbcgrAdapter.ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server MCS Connect Response after receiving a Client MCS Connect Initial PDU.");

            #endregion Basic Setting Exchange

            #region Channel Connection

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Erect Domain Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSErectDomainRequest(NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Attach User Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSAttachUserRequest(NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Attach User Confirm PDU.");
            Server_MCS_Attach_User_Confirm_Pdu attachuserConfirm = rdpbcgrAdapter.ExpectPacket<Server_MCS_Attach_User_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(attachuserConfirm, "RDP Server MUST response a Server MCS Attach User Confirm PDU after receiving a Client MCS Attach User Request PDU.");

            this.Site.Log.Add(LogEntryKind.Comment, "The test suite proceeds to join the user channel, the input/output (I/O) channel, and all of the static virtual channels.");
            rdpbcgrAdapter.ChannelJoinRequestAndConfirm();

            #endregion Channel Connection

            if (testConfig.requestProtocol == requestedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.Site.Log.Add(LogEntryKind.Comment, "Standard RDP Security mechanisms are being employed, Test Suite sends a Client Security Exchange PDU to SUT.");
                rdpbcgrAdapter.SendClientSecurityExchangePDU(NegativeType.None);
            }

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Info PDU.");
            rdpbcgrAdapter.SendClientInfoPDU(NegativeType.None, CompressionType.PACKET_COMPR_TYPE_RDP61, false);
            rdpbcgrAdapter.ProcessLicenseSequence(testConfig.timeout);

            #endregion Test Code
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify SUT can process the Capability Exchange phase successfully.")]
        public void S1_Connection_CapabilityExchange_PositiveTest()
        {
            #region Test Steps
            //1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase.
            //2. Test Suite expects a Server Demand Active PDU from SUT. When received, Test Suite verifies this PDU.
            //3. Test Suite sends a Client confirm Active PDU to SUT.
            //4. Test Suite expects SUT continues the connection by sending a Server Synchronize PDU.
            #endregion Test Steps

            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            #region Connection Initiation

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client X.224 Connection Request PDU to SUT, supported security protocol is {0}.", testConfig.requestProtocol.ToString());
            rdpbcgrAdapter.SendClientX224ConnectionRequest(NegativeType.None, testConfig.requestProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server X224 Connection Confirm.");
            Server_X_224_Connection_Confirm_Pdu confirmPdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server X224 Connection Confirm PDU after receiving a Client X224 Connection Request PDU.");

            #endregion Connection Initiation

            #region Basic Setting Exchange

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            bool supportMultitransportReliable = false;
            bool supportMultitransportLossy = false;
            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Connect Initial PDU with GCC Conference Create Request to SUT, supportMultitransportReliable is {0}, supportMultitransportLossy is {1}.", supportMultitransportReliable, supportMultitransportLossy);
            rdpbcgrAdapter.SendClientMCSConnectInitialPDU(NegativeType.None, SVCNames, false, false, false, supportMultitransportReliable, supportMultitransportLossy, false);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Connect Response PDU with GCC Conference Create Response.");
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response response = rdpbcgrAdapter.ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server MCS Connect Response after receiving a Client MCS Connect Initial PDU.");

            #endregion Basic Setting Exchange

            #region Channel Connection

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Erect Domain Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSErectDomainRequest(NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Attach User Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSAttachUserRequest(NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Attach User Confirm PDU.");
            Server_MCS_Attach_User_Confirm_Pdu attachuserConfirm = rdpbcgrAdapter.ExpectPacket<Server_MCS_Attach_User_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(attachuserConfirm, "RDP Server MUST response a Server MCS Attach User Confirm PDU after receiving a Client MCS Attach User Request PDU.");

            this.Site.Log.Add(LogEntryKind.Comment, "The test suite proceeds to join the user channel, the input/output (I/O) channel, and all of the static virtual channels.");
            rdpbcgrAdapter.ChannelJoinRequestAndConfirm();

            #endregion Channel Connection

            #region Security Commencement phase, Secure Setting Exchange Phase and Licensing phase

            if (testConfig.requestProtocol == requestedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.Site.Log.Add(LogEntryKind.Comment, "Standard RDP Security mechanisms are being employed, Test Suite sends a Client Security Exchange PDU to SUT.");
                rdpbcgrAdapter.SendClientSecurityExchangePDU(NegativeType.None);
            }

            #endregion Security Commencement phase, Secure Setting Exchange Phase and Licensing phase

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Info PDU.");
            rdpbcgrAdapter.SendClientInfoPDU(NegativeType.None, CompressionType.PACKET_COMPR_TYPE_RDP61, false);
            rdpbcgrAdapter.ProcessLicenseSequence(testConfig.timeout);

            Server_Demand_Active_Pdu demandActivePdu = rdpbcgrAdapter.ExpectPacket<Server_Demand_Active_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(demandActivePdu, "If the Licensing phase of the RDP Connection Sequence is successfully completed, RDP server must send a Server Demand Active PDU.");

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Confirm Active PDU.");
            rdpbcgrAdapter.SendClientConfirmActivePDU(NegativeType.None, true, true, true, true, true, true);

            Server_Synchronize_Pdu synchronizePdu = rdpbcgrAdapter.ExpectPacket<Server_Synchronize_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(synchronizePdu, "RDP Server MUST send a Server Synchronize PDU after receiving a Client Confirm Active PDU.");

            #endregion Test Code
        }


        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify SUT can process the Connection Finalization phase successfully.")]
        public void S1_Connection_ConnectionFinalization_PositiveTest()
        {
            #region Test Steps
            //1. Initiate an RDP connection and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange phase, Licensing phase, and Capabilities Exchange phase.
            //2. Test Suite continues the connection by sending the following PDUs sequentially:
            //    Client Synchronize PDU
            //    Client Control PDU - Cooperate
            //    Client Control PDU - Request Control
            //    Client Persistent Key List PDU(optional)
            //    Client Font List PDU
            //3. Test Suite expects and verifies the following PDUs one by one from SUT sequentially:
            //    Server Synchronize PDU
            //    Server Control PDU – Cooperate
            //    Server Control PDU - Granted Control
            //    Server Font Map PDU
            #endregion Test Steps

            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            #region Connection Initiation

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client X.224 Connection Request PDU to SUT, supported security protocol is {0}.", testConfig.requestProtocol.ToString());
            rdpbcgrAdapter.SendClientX224ConnectionRequest(NegativeType.None, testConfig.requestProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server X224 Connection Confirm.");
            Server_X_224_Connection_Confirm_Pdu confirmPdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server X224 Connection Confirm PDU after receiving a Client X224 Connection Request PDU.");

            #endregion Connection Initiation

            #region Basic Setting Exchange

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            bool supportMultitransportReliable = false;
            bool supportMultitransportLossy = false;
            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Connect Initial PDU with GCC Conference Create Request to SUT, supportMultitransportReliable is {0}, supportMultitransportLossy is {1}.", supportMultitransportReliable, supportMultitransportLossy);
            rdpbcgrAdapter.SendClientMCSConnectInitialPDU(NegativeType.None, SVCNames, false, false, false, supportMultitransportReliable, supportMultitransportLossy, false);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Connect Response PDU with GCC Conference Create Response.");
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response response = rdpbcgrAdapter.ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server MCS Connect Response after receiving a Client MCS Connect Initial PDU.");

            #endregion Basic Setting Exchange

            #region Channel Connection

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Erect Domain Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSErectDomainRequest(NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Attach User Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSAttachUserRequest(NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Attach User Confirm PDU.");
            Server_MCS_Attach_User_Confirm_Pdu attachuserConfirm = rdpbcgrAdapter.ExpectPacket<Server_MCS_Attach_User_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(attachuserConfirm, "RDP Server MUST response a Server MCS Attach User Confirm PDU after receiving a Client MCS Attach User Request PDU.");

            this.Site.Log.Add(LogEntryKind.Comment, "The test suite proceeds to join the user channel, the input/output (I/O) channel, and all of the static virtual channels.");
            rdpbcgrAdapter.ChannelJoinRequestAndConfirm();

            #endregion Channel Connection

            #region Security Commencement phase, Secure Setting Exchange Phase and Licensing phase

            if (testConfig.requestProtocol == requestedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.Site.Log.Add(LogEntryKind.Comment, "Standard RDP Security mechanisms are being employed, Test Suite sends a Client Security Exchange PDU to SUT.");
                rdpbcgrAdapter.SendClientSecurityExchangePDU(NegativeType.None);
            }

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Info PDU.");
            rdpbcgrAdapter.SendClientInfoPDU(NegativeType.None, CompressionType.PACKET_COMPR_TYPE_RDP61, false);
            rdpbcgrAdapter.ProcessLicenseSequence(testConfig.timeout);

            #endregion Security Commencement phase, Secure Setting Exchange Phase and Licensing phase

            #region Capabilities Exchange phase

            Server_Demand_Active_Pdu demandActivePdu = rdpbcgrAdapter.ExpectPacket<Server_Demand_Active_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(demandActivePdu, "If the Licensing phase of the RDP Connection Sequence is successfully completed, RDP server must send a Server Demand Active PDU.");

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Confirm Active PDU.");
            rdpbcgrAdapter.SendClientConfirmActivePDU(NegativeType.None, true, true, true, true, true, true);

            #endregion Capabilities Exchange phase

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Synchronize PDU.");
            rdpbcgrAdapter.SendClientSynchronizePDU();

            Server_Synchronize_Pdu synchronizePdu = rdpbcgrAdapter.ExpectPacket<Server_Synchronize_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(synchronizePdu, "RDP Server MUST send a Server Synchronize PDU after receiving a Client Confirm Active PDU.");

            Server_Control_Pdu_Cooperate controlPdu = rdpbcgrAdapter.ExpectPacket<Server_Control_Pdu_Cooperate>(testConfig.timeout);
            this.Site.Assert.IsNotNull(controlPdu, "RDP Server MUST send a Server Control PDU – Cooperate during Connection Finalization phase");

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Control PDU - Cooperate.");
            rdpbcgrAdapter.SendClientControlCooperatePDU();

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Control PDU - Request Control.");
            rdpbcgrAdapter.SendClientControlRequestPDU();

            Server_Control_Pdu_Granted_Control grantedControlPdu = rdpbcgrAdapter.ExpectPacket<Server_Control_Pdu_Granted_Control>(testConfig.timeout);
            this.Site.Assert.IsNotNull(grantedControlPdu, "RDP Server MUST send a Server Control PDU – Granted Control during Connection Finalization phase");

            if (rdpbcgrAdapter.IsBitmapCacheHostSupport)
            {
                this.Site.Log.Add(LogEntryKind.Comment, "Bitmap Cache Host is supported, send a Client Persistent Key List PDU.");
                rdpbcgrAdapter.SendClientPersistentKeyListPDU();
            }

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Font List PDU.");
            rdpbcgrAdapter.SendClientFontListPDU();

            Server_Font_Map_Pdu fontMapPdu = rdpbcgrAdapter.ExpectPacket<Server_Font_Map_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(fontMapPdu, "RDP Server MUST send a Server Font Map PDU during Connection Finalization phase");

            #endregion Test Code
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify the messages and behaviors of the disconnection sequence initiated by SUT, after user logon.")]
        public void S1_Connection_Disconnection_PositiveTest_ClientInitiated_UserLogon()
        {
            #region Test Steps
            //1. Initiate and complete an RDP connection with RDP server (SUT).
            //2. Test suite expects a Server Save Session Info PDU to notify user logon, and verify it.
            //3. Test suite sends a Shutdown Request PDU to initiate a disconnection sequence.
            //4. Test Suite expects a Shutdown Request Denied PDU.
            #endregion Test Steps

            #region Test Code
            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            rdpbcgrAdapter.EstablishRDPConnection(testConfig.requestProtocol,
                SVCNames,
                CompressionType.PACKET_COMPR_TYPE_RDP61,
                false, // Is reconnect
                true  // Is Auto logon
                );

            this.Site.Log.Add(LogEntryKind.Comment, "Wait RDP server to notify user logon.");
            rdpbcgrAdapter.WaitForLogon(testConfig.timeout);

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Shutdown Request PDU.");
            rdpbcgrAdapter.SendClientShutdownRequestPDU();

            this.Site.Log.Add(LogEntryKind.Comment, "Expect a Server Shutdown Request Denied PDU from SUT.");
            Server_Shutdown_Request_Denied_Pdu requestDenied = rdpbcgrAdapter.ExpectPacket<Server_Shutdown_Request_Denied_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(requestDenied, "Timeout when receiving Server Shutdown Request Denied PDU, RDP Server should response a Server Shutdown Request Denied PDU if receiving a Client Shutdown Request PDU after user logon.");

            #endregion Test Code
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify the messages and behaviors of the disconnection sequence initiated by SUT, before user logon.")]
        public void S1_Connection_Disconnection_PositiveTest_ClientInitiated_UserNotLogon()
        {
            #region Test Steps
            //1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, and Channel Connection phase.
            //2. Test suite sends a Shutdown Request PDU to initiate a disconnection sequence.
            //3. Expect SUT close the connection.
            #endregion Test Steps

            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            rdpbcgrAdapter.EstablishRDPConnection(testConfig.requestProtocol, SVCNames, CompressionType.PACKET_COMPR_TYPE_RDP61);

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Shutdown Request PDU.");
            rdpbcgrAdapter.SendClientShutdownRequestPDU();

            this.Site.Log.Add(LogEntryKind.Comment, "Expect RDP server drop this connection.");
            bool disconnected = rdpbcgrAdapter.ExpectDisconnetion(testConfig.timeout);
            Site.Assert.IsTrue(disconnected, "SUT should disconnect the connection when receiving Server Shutdown Request before logon.");
            #endregion Test Code
        }
    }
}