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
        [TestCategory("BVT")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to ensure SUT can send Initiate Multitransport Request PDU to bootstrap UDP connection.")]
        public void S7_MultitransportBootstrapping_PositiveTest()
        {
            #region Test Steps
            //1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase and Licensing phase. Indicate support for both reliable and lossy multitransport in basic setting exchange phase.
            //2. Test Suite expects a Server Initiate Multitransport Request PDU with requestedProtocol set to INITITATE_REQUEST_PROTOCOL_UDPFECR (0x01). When received, Test Suite verifies this PDU.
            //3. Test Suite expects a Server Initiate Multitransport Request PDU with requestedProtocol set to INITITATE_REQUEST_PROTOCOL_UDPFECL (0x02). When received, Test Suite verifies this PDU.
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
            bool supportMultitransportReliable = true;
            bool supportMultitransportLossy = true;
            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Connect Initial PDU with GCC Conference Create Request to SUT, supportMultitransportReliable is {0}, supportMultitransportLossy is {1}.", supportMultitransportReliable, supportMultitransportLossy);
            rdpbcgrAdapter.SendClientMCSConnectInitialPDU(NegativeType.None, SVCNames, false, false, false, supportMultitransportReliable, supportMultitransportLossy, false);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Connect Response PDU with GCC Conference Create Response.");
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response response = rdpbcgrAdapter.ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server MCS Connect Response after receiving a Client MCS Connect Initial PDU.");
            bool serverSupportUDPFECR = false;
            bool serverSupportUDPFECL = false;
            if (response.mcsCrsp.gccPdu.serverMultitransportChannelData != null)
            {
                if (response.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR))
                {
                    serverSupportUDPFECR = true;
                }
                if (response.mcsCrsp.gccPdu.serverMultitransportChannelData.flags.HasFlag(MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL))
                {
                    serverSupportUDPFECL = true;
                }
            }
            this.Site.Assume.IsTrue(serverSupportUDPFECR || serverSupportUDPFECL, "To run test case for multitransport, the RDP server should be configured to support RDP-UDP: reliable, lossy, or both.");

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

            bool receivedServerInitiateMultitransportRequestForReliable = false;
            bool receivedServerInitiateMultitransportRequestForLossy = false;
            int expectedServerInitiateMultitransportRequestNum = 1;
            if (serverSupportUDPFECR && serverSupportUDPFECL)
            {
                expectedServerInitiateMultitransportRequestNum = 2;
            }

            for (int i = 0; i < expectedServerInitiateMultitransportRequestNum; i++)
            {
                Server_Initiate_Multitransport_Request_PDU request = rdpbcgrAdapter.ExpectPacket<Server_Initiate_Multitransport_Request_PDU>(testConfig.timeout);
                Site.Assert.IsNotNull(request, "RDP Server MUST send Server_Initiate_Multitransport_Request_PDU packet to initiate multiple transport.");
                if (request.requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR)
                {
                    receivedServerInitiateMultitransportRequestForReliable = true;
                }
                else if (request.requestedProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
                {
                    receivedServerInitiateMultitransportRequestForLossy = true;
                }
            }

            if (serverSupportUDPFECR)
            {
                Site.Assert.IsTrue(receivedServerInitiateMultitransportRequestForReliable,
               "RDP Server MUST send Server_Initiate_Multitransport_Request_PDU packets for reliable multitransport.");
            }
            if (serverSupportUDPFECL)
            {
                Site.Assert.IsTrue(receivedServerInitiateMultitransportRequestForLossy,
               "RDP Server MUST send Server_Initiate_Multitransport_Request_PDU packets for lossy multitransport.");
            }

            #endregion Test Code
        }
    }
}