// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{
    [TestClass]
    public partial class RdpemtTestSuite : RdpTestClassBase
    {
        protected RdpemtAdapter rdpemtAdapter;
        protected RdpbcgrAdapter rdpbcgrAdapter;

        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            RdpTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            RdpTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            this.rdpbcgrAdapter = new RdpbcgrAdapter(testConfig);
            this.rdpbcgrAdapter.Initialize(Site);

            this.rdpemtAdapter = new RdpemtAdapter(testConfig);
            this.rdpemtAdapter.Initialize(Site);
        }

        protected override void TestCleanup()
        {
            if (rdpbcgrAdapter != null)
            {
                rdpbcgrAdapter.Reset();
            }

            if (rdpemtAdapter != null)
            {
                rdpemtAdapter.Reset();
            }

            base.TestCleanup();
        }
        #endregion


        private void StartRDPConnect()
        {
            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            #region Connection Initiation

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client X.224 Connection Request PDU to SUT, supported security protocol is {0}.", testConfig.requestProtocol.ToString());
            rdpbcgrAdapter.SendClientX224ConnectionRequest(Rdpbcgr.NegativeType.None, testConfig.requestProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server X224 Connection Confirm.");
            Server_X_224_Connection_Confirm_Pdu confirmPdu = rdpbcgrAdapter.ExpectPacket<Server_X_224_Connection_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server X224 Connection Confirm PDU after receiving a Client X224 Connection Request PDU.");

            #endregion Connection Initiation

            #region Basic Setting Exchange

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            bool supportMultitransportReliable = true;
            bool supportMultitransportLossy = true;
            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Connect Initial PDU with GCC Conference Create Request to SUT, supportMultitransportReliable is {0}, supportMultitransportLossy is {1}.", supportMultitransportReliable, supportMultitransportLossy);
            rdpbcgrAdapter.SendClientMCSConnectInitialPDU(Rdpbcgr.NegativeType.None, SVCNames, false, false, false, supportMultitransportReliable, supportMultitransportLossy, false);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Connect Response PDU with GCC Conference Create Response.");
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response response = rdpbcgrAdapter.ExpectPacket<Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response>(testConfig.timeout);
            this.Site.Assert.IsNotNull(confirmPdu, "RDP Server MUST response a Server MCS Connect Response after receiving a Client MCS Connect Initial PDU.");
            serverSupportUDPFECR = false;
            serverSupportUDPFECL = false;
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
            rdpbcgrAdapter.SendClientMCSErectDomainRequest(Rdpbcgr.NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client MCS Attach User Request PDU to SUT.");
            rdpbcgrAdapter.SendClientMCSAttachUserRequest(Rdpbcgr.NegativeType.None);

            this.Site.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Server MCS Attach User Confirm PDU.");
            Server_MCS_Attach_User_Confirm_Pdu attachuserConfirm = rdpbcgrAdapter.ExpectPacket<Server_MCS_Attach_User_Confirm_Pdu>(testConfig.timeout);
            this.Site.Assert.IsNotNull(attachuserConfirm, "RDP Server MUST response a Server MCS Attach User Confirm PDU after receiving a Client MCS Attach User Request PDU.");

            this.Site.Log.Add(LogEntryKind.Comment, "The test suite proceeds to join the user channel, the input/output (I/O) channel, and all of the static virtual channels.");
            rdpbcgrAdapter.ChannelJoinRequestAndConfirm();

            #endregion Channel Connection

            if (testConfig.requestProtocol == requestedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.Site.Log.Add(LogEntryKind.Comment, "Standard RDP Security mechanisms are being employed, Test Suite sends a Client Security Exchange PDU to SUT.");
                rdpbcgrAdapter.SendClientSecurityExchangePDU(Rdpbcgr.NegativeType.None);
            }

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Info PDU.");
            rdpbcgrAdapter.SendClientInfoPDU(Rdpbcgr.NegativeType.None, CompressionType.PACKET_COMPR_TYPE_RDP61, false);
            rdpbcgrAdapter.ProcessLicenseSequence(testConfig.timeout);
        }


        private List<Server_Initiate_Multitransport_Request_PDU> ExpectServerInitiateMultitransportRequestPDUs()
        {
            bool receivedServerInitiateMultitransportRequestForReliable = false;
            bool receivedServerInitiateMultitransportRequestForLossy = false;
            int expectedServerInitiateMultitransportRequestNum = 1;
            if (serverSupportUDPFECR && serverSupportUDPFECL)
            {
                expectedServerInitiateMultitransportRequestNum = 2;
            }

            var serverInitiateMultitransportRequestPDUs = new List<Server_Initiate_Multitransport_Request_PDU>();

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
                else
                {
                    Site.Assert.Fail("The value of requestedProtocol 0x{0:04X} is unexpected!", (ushort)request.requestedProtocol);
                }
                serverInitiateMultitransportRequestPDUs.Add(request);
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

            return serverInitiateMultitransportRequestPDUs;
        }

        private bool serverSupportUDPFECR;
        private bool serverSupportUDPFECL;
    }
}