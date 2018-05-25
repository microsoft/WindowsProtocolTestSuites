// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.SessionMgmt
{
    [TestClass]
    public class SessionMgmt : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        #endregion

        #region Test Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Case Initialize and Clean up
        protected override void TestInitialize()
        {
            base.TestInitialize();
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
        }

        protected override void TestCleanup()
        {
            if (client != null)
            {
                client.Disconnect();
            }
            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Session)]
        [Description("This test case is designed to test whether server can handle a new session.")]
        public void BVT_SessionMgmt_NewSession()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client sends NEGOTIATE request.");
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client sends SESSION_SETUP request with SESSION_ID set to ZERO.");
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, TestConfig.UseServerGssToken);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Session)]
        [Description("This test case is designed to test whether server can handle reauthentication successfully.")]
        public void BVT_SessionMgmt_Reauthentication()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE request.");
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request with SESSION_ID set to ZERO.");
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, TestConfig.UseServerGssToken);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends another SESSION-SETUP request for reauthentication.");
            client.SessionSetup(
                testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE, // The second session setup should set signed flag if server supports signing to keep consistency with SDK.
                TestConfig.DefaultSecurityPackage, 
                TestConfig.SutComputerName, 
                TestConfig.AccountCredential, 
                TestConfig.UseServerGssToken,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                checker:(header, response) =>
                {
                    // <225> Section 3.3.5.5: Windows Vista SP1 and Windows Server 2008 servers fail the session setup request with STATUS_REQUEST_NOT_ACCEPTED.
                    if (TestConfig.Platform == Platform.WindowsServer2008)
                    {
                        BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_REQUEST_NOT_ACCEPTED, 
                            header.Status,
                            "Windows Vista SP1 and Windows Server 2008 should return STATUS_REQUEST_NOT_ACCEPTED. Actually it returns {0}.", 
                            Smb2Status.GetStatusCode(header.Status));
                    }
                    else if (header.Status != Smb2Status.STATUS_SUCCESS)
                    {
                        // 3.3.5.5   Receiving an SMB2 SESSION_SETUP Request
                        // 6.	If Session.State is Valid, the server SHOULD<225> process the session setup request as specified in section 3.3.5.5.2.
                        if (TestConfig.Platform == Platform.NonWindows)
                        {
                            BaseTestSite.Assert.Fail("Reauthentication is not supported in the server.");
                        }
                        else
                        {
                            throw new InvalidOperationException(string.Format("Unexpected status {0}", Smb2Status.GetStatusCode(header.Status)));
                        }
                    }
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Session)]
        [Description("This test case is designed to test whether server can handle authentication after disconnection.")]
        public void BVT_SessionMgmt_ReconnectSessionSetup()
        {
            Guid clientGuid = (TestConfig.RequestDialects.Length == 1 && TestConfig.RequestDialects[0] == DialectRevision.Smb2002) ? Guid.Empty : Guid.NewGuid();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a first client by sending NEGOTIATE request.");
            client.Negotiate(TestConfig.RequestDialects, 
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                null, // capability value will be set inside the method according to requested dialects.
                clientGuid);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends SESSION_SETUP request with SESSION_ID set to ZARO.");
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, TestConfig.UseServerGssToken);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disconnect the first client by sending DISCONNECT request.");
            client.Disconnect();

            #region Reconnect and Do Session Setup with PreviousSessionId set.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a second client by sending NEGOTIATE request.");
            Smb2FunctionalClient client2 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client2.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client2.Negotiate(TestConfig.RequestDialects, 
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                null, // capability value will be set inside the method according to requested dialects.
                clientGuid);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The second client sends SESSION_SETUP request with SESSION_ID set to the value in the previous SESSION_SETUP response.");
            client2.ReconnectSessionSetup(client, TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            #endregion

            #region Tear Down Client
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: LOG_OFF; DISCONNECT");
            client2.LogOff();
            client2.Disconnect();
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.Session)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle reconnect with different dialect.")]
        public void SessionMgmt_ReconnectWithDifferentDialect()
        {
            Guid clientGuid = Guid.NewGuid();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a first client by sending NEGOTIATE request.");
            client.Negotiate(TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                null, // capability value will be set inside the method according to requested dialects.
                clientGuid);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends SESSION_SETUP request with SESSION_ID set to ZERO.");
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, TestConfig.UseServerGssToken);

            #region Reconnect and Do Session Setup with PreviousSessionId set.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a second client by sending NEGOTIATE request.");
            Smb2FunctionalClient client2 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client2.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            // Select different dialects
            List<DialectRevision> newRequestDialects = new List<DialectRevision>();
            foreach (DialectRevision dialect in TestConfig.RequestDialects)
            {
                if (dialect != client.Dialect)
                {
                    newRequestDialects.Add(dialect);
                }
            }

            client2.Negotiate(newRequestDialects.ToArray(),
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                null, // capability value will be set inside the method according to requested dialects.
                clientGuid);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The second client sends SESSION_SETUP request with SESSION_ID set to the value in the previous SESSION_SETUP response.");
            client2.ReconnectSessionSetup(
                client,
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                false,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                checker: (Packet_Header header, SESSION_SETUP_Response response) =>
                {                    
                    if (testConfig.IsWindowsPlatform && testConfig.Platform != Platform.WindowsServer2008R2)
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_USER_SESSION_DELETED,
                            header.Status,
                            "[MS-SMB2] 3.3.5.5.3 The server MUST look up all existing connections from the client in the global ConnectionList where Connection.ClientGuid matches Session.Connection.ClientGuid. " +
                            "For any matching Connection, if Connection.Dialect is not the same as Session.Connection.Dialect, the server SHOULD<235> close the newly created Session, " +
                            "as specified in section 3.3.4.12, by providing Session.SessionGlobalId as the input parameter, and fail the session setup request with STATUS_USER_SESSION_DELETED.");
                    }
                    else if (testConfig.Platform == Platform.NonWindows)
                    {
                        BaseTestSite.Assert.AreNotEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "[MS-SMB2] 3.3.5.5.3 The server MUST look up all existing connections from the client in the global ConnectionList where Connection.ClientGuid matches Session.Connection.ClientGuid. " +
                            "For any matching Connection, if Connection.Dialect is not the same as Session.Connection.Dialect, the server SHOULD<235> close the newly created Session, " +
                            "as specified in section 3.3.4.12, by providing Session.SessionGlobalId as the input parameter, and fail the session setup request with STATUS_USER_SESSION_DELETED.");
                    }
                });
            #endregion

            #region Tear Down Client
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disconnect both clients by sending DISCONNECT request.");
            client.Disconnect();
            client2.Disconnect();
            #endregion
        }
    }
}
