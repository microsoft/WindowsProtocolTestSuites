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
        [Description(@"This test case is used to ensure SUT can complete the optional auto detect phase successfully.")]
        public void S6_AutoDetect_PositiveTest_ConnectTimeAutoDetect()
        {
            #region Test Steps
            //1. Initiate an RDP connection to RDP server (SUT) and complete the Connection Initiation phase, Basic Setting Exchange phase, Channel Connection phase, RDP Security Commencement phase, Secure Setting Exchange Phase. Verify that the server support auto detect and advise the client’s support of auto detect.
            //2. During connect time auto detect phase, test suite expects and verifies a Server Auto-Detect Request PDU:
            //    If the request contains a RDP_RTT_REQUEST, the test suite reply a Client Auto-Detect Response PDU with RDP_RTT_RESPONSE, then repeat step 2.
            //    If the request contains a RDP_BW_START, the test suite start a bandwidth count, then repeat step 2.
            //    If the request contains a RDP_BW_PAYLOAD, the test suite add bytes to bandwidth count, then repeat step 2.
            //    If the request contains a RDP_BW_STOP, the test suite reply a Client Auto-Detect Response PDU with RDP_BW_RESULTS, then repeat step 2.
            //    If the request contains a RDP_NETCHAR_RESULT, stop the test run.
            #endregion Test Steps

            #region Test Code
            this.Site.Log.Add(LogEntryKind.Comment, "To test auto detect, the RDP server should be configured to support auto-detect.");

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
            rdpbcgrAdapter.SendClientMCSConnectInitialPDU(NegativeType.None, SVCNames, true, true, true, supportMultitransportReliable, supportMultitransportLossy, false);

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

            #region Security Commencement phase / Secure Setting Exchange Phase

            if (testConfig.requestProtocol == requestedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                this.Site.Log.Add(LogEntryKind.Comment, "Standard RDP Security mechanisms are being employed, Test Suite sends a Client Security Exchange PDU to SUT.");
                rdpbcgrAdapter.SendClientSecurityExchangePDU(NegativeType.None);
            }

            this.Site.Log.Add(LogEntryKind.Comment, "Send a Client Info PDU.");
            rdpbcgrAdapter.SendClientInfoPDU(NegativeType.None, CompressionType.PACKET_COMPR_TYPE_RDP61, false);
            #endregion Security Commencement phase / Secure Setting Exchange Phase

            this.Site.Log.Add(LogEntryKind.Comment, "Process network auto detect phase.");
            rdpbcgrAdapter.ProcessAutoDetectSequence();

            #endregion Test Code
        }
    }
}