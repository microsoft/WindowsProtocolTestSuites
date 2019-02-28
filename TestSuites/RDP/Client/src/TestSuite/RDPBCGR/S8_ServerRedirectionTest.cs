// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("ServerRedirection")]
        [Description("This test case is used to ensure SUT can process server redirection successfully and the routingToken is not expected when the server didn't present LoadBalanceInfo in the Server Redirection PDU.")]
        public void BVT_ServerRedirection_PositiveTest_WithoutRoutingToken()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process server redirection successfully and the routingToken is not expected when the server didn’t present LoadBalanceInfo in the Server Redirection PDU.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection.
            2.	Test Suite sends SUT a Server Redirection PDU which does not present the LoadBalanceInfo field and sets the TargetNetAddress to the same machine where the Test Suite locates.
            3.	Test Suite expects SUT terminate the current connection.
            4.	Test Suite then expects SUT initiate a new connection. Expectation:
                a.	In Client X.224 Connection Request PDU, the routingToken field is expected to be not present.
                b.	In Client Cluster Data within Client MCS Connect Initial PDU, the RefirectedSessionID field is expected to be set to the SessionID sent by the server in Step 3.
                c.	In Client Info PDU, it's expected that SUT set the credentials to that sent by Test Suite in step 3.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportServerRedirection, "SUT should support server redirection.");

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region First Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Redirection PDU to SUT. The routing token is not present.");
            this.rdpbcgrAdapter.SendServerRedirectionPdu(false);

            #endregion

            RDPClientTryDropConnection("Server Redirection PDU without Routing Token");

            #region Redirect Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Redirection);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            #region Register Verification Methods.
            this.rdpbcgrAdapter.X224ConnectionRequest += new X224ConnectioRequestHandler(this.S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyX224ConnectioRequest);
            this.rdpbcgrAdapter.McsConnectRequest += new McsConnectRequestHandler(this.S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyMCSConnectInitialPDU);
            this.rdpbcgrAdapter.ClientInfoRequest += new ClientInfoRequestHandler(this.S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyClientInfoPdu);
            #endregion

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            #region Unregister Verification Methods.
            this.rdpbcgrAdapter.X224ConnectionRequest -= new X224ConnectioRequestHandler(this.S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyX224ConnectioRequest);
            this.rdpbcgrAdapter.McsConnectRequest -= new McsConnectRequestHandler(this.S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyMCSConnectInitialPDU);
            this.rdpbcgrAdapter.ClientInfoRequest -= new ClientInfoRequestHandler(this.S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyClientInfoPdu);
            #endregion

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("ServerRedirection")]
        [Description("This test case is used to ensure SUT can process server redirection successfully and the routingToken is expected when the server present LoadBalanceInfo in the Server Redirection PDU.")]
        public void S8_ServerRedirection_PositiveTest_WithRoutingToken()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process server redirection successfully and the routingToken is expected when the server present LoadBalanceInfo in the Server Redirection PDU.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	Test Suite sends SUT a Server Redirection PDU which presents the LoadBalanceInfo field and sets the TargetNetAddress to the same machine where the Test Suite locates.
            3.	Test Suite expects SUT terminate the current connection.
            4.	Test Suite then expects SUT initiate a new connection. Expectation:
                a.	In Client X.224 Connection Request PDU, the routingToken field is expected to be present.
                b.	In Client Cluster Data within Client MCS Connect Initial PDU, the RefirectedSessionID field is expected to be set to the SessionID sent by Test Suite in Step 3.
                c.	In Client Info PDU, it's expected that SUT sets the credentials to that sent by Test Suite in step 3.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportServerRedirection, "SUT should support server redirection.");

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region First Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Redirection PDU to SUT. The routing token is present.");
            this.rdpbcgrAdapter.SendServerRedirectionPdu(true);

            #endregion

            RDPClientTryDropConnection("Server Redirection PDU without Routing Token");

            #region Redirect Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Redirection);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            #region Register Verification Methods.
            this.rdpbcgrAdapter.X224ConnectionRequest += new X224ConnectioRequestHandler(this.S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyX224ConnectioRequest);
            this.rdpbcgrAdapter.McsConnectRequest += new McsConnectRequestHandler(this.S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyMCSConnectInitialPDU);
            this.rdpbcgrAdapter.ClientInfoRequest += new ClientInfoRequestHandler(this.S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyClientInfoPdu);
            #endregion

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            #region Unregister Verification Methods.
            this.rdpbcgrAdapter.X224ConnectionRequest -= new X224ConnectioRequestHandler(this.S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyX224ConnectioRequest);
            this.rdpbcgrAdapter.McsConnectRequest -= new McsConnectRequestHandler(this.S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyMCSConnectInitialPDU);
            this.rdpbcgrAdapter.ClientInfoRequest -= new ClientInfoRequestHandler(this.S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyClientInfoPdu);
            #endregion

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            #endregion
        }

        #region Verification Methods for S8_ServerRedirection_PositiveTest_WithoutRoutingToken
        private void S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyX224ConnectioRequest(Client_X_224_Connection_Request_Pdu x224Request)
        {
            this.TestSite.Assert.IsTrue(x224Request.routingToken == null || x224Request.routingToken.Length == 0, "Verify if routingToken is not present in X.224 Connection Request PDU for server redirection.");
        }

        private void S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyMCSConnectInitialPDU(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mcsConnectRequest)
        {
            this.TestSite.Assert.AreEqual<uint>(RdpbcgrTestData.Test_Redirection_SessionId,
                mcsConnectRequest.mcsCi.gccPdu.clientClusterData.RedirectedSessionID,
                "Verify if the RefirectedSessionID field within Client Cluster Data is equal to the SessionID sent by the server in Server Redirection PDU.");
        }

        private void S8_ServerRedirection_PositiveTest_WithoutRoutingToken_VerifyClientInfoPdu(Client_Info_Pdu clientInfoPdu)
        {
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_UserName.ToUpper(), clientInfoPdu.infoPacket.UserName.Trim('\0').ToUpper(), "Verify if UserName in Client Info PDU equals that sent in Server Redirection PDU.");
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_Domain.ToUpper(), clientInfoPdu.infoPacket.Domain.Trim('\0').ToUpper(), "Verify if Domain in Client Info PDU equals that sent in Server Redirection PDU.");
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_Password, clientInfoPdu.infoPacket.Password.Trim('\0'), "Verify if Password in Client Info PDU equals that sent in Server Redirection PDU.");
        }
        #endregion

        #region Verification Methods for S8_ServerRedirection_PositiveTest_WithRoutingToken
        private void S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyX224ConnectioRequest(Client_X_224_Connection_Request_Pdu x224Request)
        {
            string receivedRoutingToken = string.Empty;
            if (x224Request.routingToken != null)
            {
                receivedRoutingToken = ASCIIEncoding.ASCII.GetString(x224Request.routingToken);
            }
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_Redirection_RoutingToken, receivedRoutingToken, "Verify if routingToken is present in X.224 Connection Request PDU and same as that sent in Server Redirection PDU.");
        }

        private void S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyMCSConnectInitialPDU(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mcsConnectRequest)
        {
            this.TestSite.Assert.AreEqual<uint>(RdpbcgrTestData.Test_Redirection_SessionId,
                mcsConnectRequest.mcsCi.gccPdu.clientClusterData.RedirectedSessionID,
                "Verify if the RefirectedSessionID field within Client Cluster Data is equal to the SessionID sent by the server in Server Redirection PDU.");
        }

        private void S8_ServerRedirection_PositiveTest_WithRoutingToken_VerifyClientInfoPdu(Client_Info_Pdu clientInfoPdu)
        {
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_UserName.ToUpper(), clientInfoPdu.infoPacket.UserName.Trim('\0').ToUpper(), "Verify if UserName in Client Info PDU equals that sent in Server Redirection PDU.");
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_Domain.ToUpper(), clientInfoPdu.infoPacket.Domain.Trim('\0').ToUpper(), "Verify if Domain in Client Info PDU equals that sent in Server Redirection PDU.");
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_Password, clientInfoPdu.infoPacket.Password.Trim('\0'), "Verify if Password in Client Info PDU equals that sent in Server Redirection PDU.");
        }
        #endregion
    }
}
