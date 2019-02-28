// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrTestSuite
    {
        [TestMethod]
        [Priority(1)]
        [TestCategory("BVT")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("RDSTLSAuthentication")]
        [Description("This test case is used to ensure SUT can process server redirection through RDSTLS protocol successfully by returning redirection GUID obtained in server redirection PDU.")]
        public void BVT_RDSTLSAuthentication_PositiveTest_ServerRedirectionWithPasswordCredentials()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process server redirection through RDSTLS protocol successfully and the redirection GUID is just the one obtained in server redirection PDU.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	Test Suite sends SUT a Server Redirection PDU which contains the redirection GUID, certificate and credential.
            3.	Test Suite expects SUT terminate the current connection.
            4.	Test Suite then expects SUT initiate a new connection. Expectation:
                a.	In Client X.224 Connection Request PDU, the requested protocols includes RDSTLS.
                b.  In RDSTLS authentication PDU, the redirection GUID and credentials are expected to be those sent by Test Suite in Step 2.
                c.	In Client Cluster Data within Client MCS Connect Initial PDU, the RefirectedSessionID field is expected to be set to the SessionID sent by Test Suite in Step 2.
                d.	In Client Info PDU, it's expected that SUT sets the credentials to that sent by Test Suite in step 2.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assume.IsTrue(isClientSupportServerRedirection, "SUT should support server redirection.");

            if (transportProtocol != EncryptedProtocol.NegotiationTls)
            {
                // Make this test case inconclusive if the chosen protocol is not negotiation TLS
                this.TestSite.Assume.Inconclusive("The transport protocol of RDSTLS authentication should be negotiation TLS.");
            }

            TriggerFirstConnectionAndRedirectWithRDSTLSAuthentication();

            ExpectSecondConnectionWithRDSTLSAuthentication();

            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("RDSTLSAuthentication")]
        [Description("This test case is used to ensure SUT can handle auto-reconnect through RDSTLS protocol successfully after server redirection by returning cookie obtained in Server Save Session Info PDU.")]
        public void S11_RDSTLSAuthentication_PositiveTest_ServerRedirectionAndAutoReconnectWithCookie()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process server redirection through RDSTLS protocol successfully and the redirection GUID is just the one obtained in server redirection PDU.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. 
            2.	Test Suite sends SUT a Server Redirection PDU which contains the redirection GUID, certificate and credential.
            3.	Test Suite expects SUT terminate the current connection.
            4.	Test Suite then expects SUT initiate a new connection. Expectation:
                a.	In Client X.224 Connection Request PDU, the requested protocols includes RDSTLS.
                b.  In RDSTLS authentication PDU, the redirection GUID and credentials are expected to be those sent by Test Suite in Step 2.
                c.	In Client Cluster Data within Client MCS Connect Initial PDU, the RefirectedSessionID field is expected to be set to the SessionID sent by Test Suite in Step 2.
                d.	In Client Info PDU, it's expected that SUT sets the credentials to that sent by Test Suite in step 2.
            5. Test Suite sends SUT with the auto-reconnect cookie in Server Save Session Info PDU.
            6. Test Suite then triggers a network failure and expects SUT reconnect through RDSTLS authentication. Expectation:
                a.	In Client X.224 Connection Request PDU, the requested protocols includes RDSTLS.
                b.  In RDSTLS authentication PDU, the auto-reconnect cookie is expected to that sent by Test Suite in Step 5.
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assume.IsTrue(isClientSupportServerRedirection, "SUT should support server redirection.");

            this.TestSite.Assume.IsTrue(isClientSuportAutoReconnect, "SUT should support auto-reconnect.");

            if (transportProtocol != EncryptedProtocol.NegotiationTls)
            {
                // Make this test case inconclusive if the chosen protocol is not negotiation TLS
                this.TestSite.Assume.Inconclusive("The transport protocol of RDSTLS authentication should be negotiation TLS.");
            }

            TriggerFirstConnectionAndRedirectWithRDSTLSAuthentication();

            ExpectSecondConnectionWithRDSTLSAuthentication();

            // Send cookie for auto-reconnect
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.AutoReconnectCookie, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            int iResult = this.sutControlAdapter.TriggerClientAutoReconnect(this.TestContext.TestName);
            TestSite.Assume.IsTrue(iResult >= 0, "SUT Control Adapter: TriggerClientAutoReconnect should be successful: {0}.", iResult);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.AutoReconnection);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            #region Register Verification Methods.
            this.rdpbcgrAdapter.X224ConnectionRequest += new X224ConnectioRequestHandler(this.CheckNegotiationFlag);
            this.rdpbcgrAdapter.RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieReceived += new RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieHandler(this.CheckRDSTLSAuthentication_VerifyCookie);
            #endregion

            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocols_Values.PROTOCOL_RDSTLS, enMethod, enLevel, true, true, rdpServerVersion);

            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #region Unregister Verification Methods.
            this.rdpbcgrAdapter.X224ConnectionRequest -= new X224ConnectioRequestHandler(this.CheckNegotiationFlag);
            this.rdpbcgrAdapter.RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieReceived -= new RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookieHandler(this.CheckRDSTLSAuthentication_VerifyCookie);
            #endregion

            #endregion
        }

        private void TriggerFirstConnectionAndRedirectWithRDSTLSAuthentication()
        {
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Redirection PDU to SUT.");
            this.rdpbcgrAdapter.SendServerRedirectionPduRDSTLS();

            #endregion

            RDPClientTryDropConnection("Server Redirection PDU through RDSTLS protocol.");
        }

        private void ExpectSecondConnectionWithRDSTLSAuthentication()
        {
            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Redirection);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            #region Register Verification Methods.
            this.rdpbcgrAdapter.X224ConnectionRequest += new X224ConnectioRequestHandler(this.CheckNegotiationFlag);
            this.rdpbcgrAdapter.RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsReceived += new RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsHandler(this.CheckRDSTLSAuthenticationFields);
            this.rdpbcgrAdapter.McsConnectRequest += new McsConnectRequestHandler(this.CheckRDSTLSAuthentication_VerifySessionId);
            this.rdpbcgrAdapter.ClientInfoRequest += new ClientInfoRequestHandler(this.CheckRDSTLSAuthentication_VerifyClientInfoFields);
            #endregion

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocols_Values.PROTOCOL_RDSTLS, enMethod, enLevel, true, false, rdpServerVersion);

            #region Unregister Verification Methods.
            this.rdpbcgrAdapter.X224ConnectionRequest -= new X224ConnectioRequestHandler(this.CheckNegotiationFlag);
            this.rdpbcgrAdapter.RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsReceived -= new RDSTLS_AuthenticationRequestPDUwithPasswordCredentialsHandler(this.CheckRDSTLSAuthenticationFields);
            this.rdpbcgrAdapter.McsConnectRequest -= new McsConnectRequestHandler(this.CheckRDSTLSAuthentication_VerifySessionId);
            this.rdpbcgrAdapter.ClientInfoRequest -= new ClientInfoRequestHandler(this.CheckRDSTLSAuthentication_VerifyClientInfoFields);
            #endregion

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);
        }

        private void CheckRDSTLSAuthentication_VerifyClientInfoFields(Client_Info_Pdu clientInfoPdu)
        {
            // user name
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_UserName, clientInfoPdu.infoPacket.UserName.Trim('\0'), "User name should equal to that set in server redirection PDU.");

            // domain
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_Domain, clientInfoPdu.infoPacket.Domain.Trim('\0'), "Domain should equal to that set in server redirection PDU.");

        }

        private void CheckRDSTLSAuthentication_VerifySessionId(Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mcsConnectRequest)
        {
            this.TestSite.Assert.AreEqual<uint>(RdpbcgrTestData.Test_Redirection_SessionId, mcsConnectRequest.mcsCi.gccPdu.clientClusterData.RedirectedSessionID, "Session ID should equal to that set in server redirection PDU.");
        }

        private void CheckRDSTLSAuthenticationFields(RDSTLS_AuthenticationRequestPDUwithPasswordCredentials pdu)
        {
            // redirection GUID
            var redirectionGuid = RdpbcgrUtility.EncodeUnicodeStringToBytes(RdpbcgrTestData.Test_RedirectionGuid);
            bool checkRedirectionGuid = redirectionGuid.SequenceEqual(pdu.RedirectionGuid);
            this.TestSite.Assert.AreEqual<bool>(true, checkRedirectionGuid, "Redirection PDU should equal to that set in server redirection PDU.");

            // user name
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_UserName, pdu.UserName, "User name should equal to that set in server redirection PDU.");

            // domain
            this.TestSite.Assert.AreEqual<string>(RdpbcgrTestData.Test_Domain, pdu.Domain, "Domain should equal to that set in server redirection PDU.");

            // password
            var password = RdpbcgrUtility.EncodeUnicodeStringToBytes(RdpbcgrTestData.Test_Password);
            bool checkPassword = password.SequenceEqual(pdu.Password);
            this.TestSite.Assert.AreEqual<bool>(true, checkPassword, "Password should equal to that set in server redirection PDU.");
        }

        private void CheckNegotiationFlag(Client_X_224_Connection_Request_Pdu x224Request)
        {
            this.TestSite.Assert.AreEqual<bool>(true, x224Request.rdpNegData.requestedProtocols.HasFlag(requestedProtocols_Values.PROTOCOL_RDSTLS), @"The request protocol for redirected connection should include the RDSTLS protocol.");
        }

        private void CheckRDSTLSAuthentication_VerifyCookie(RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie pdu)
        {
            bool ret = Enumerable.SequenceEqual(RdpbcgrTestData.Test_ArcRadndomBits, pdu.AutoReconnectCookie);
            TestSite.Assert.IsTrue(ret, "Auto-reconnect cookie should equal to that set in Server Save Session Info PDU.");
        }

    }
}
