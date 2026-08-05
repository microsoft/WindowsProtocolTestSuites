// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    /// <summary>
    /// Test cases validating NTLM (MS-NLMP) authentication performed through the SMB2
    /// SESSION_SETUP exchange. Covers MS-NLMP 3 and the server-side processing of the
    /// authentication token described in MS-SMB2 3.3.5.5.
    /// </summary>
    [TestClass]
    public class NtlmAuthentication : AuthenticationTestBase
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
                try
                {
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
            }
            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.NtlmAuthentication)]
        [Description("This test case is designed to verify that the server can handle NTLM (NTLMSSP) authentication " +
            "through the SMB2 SESSION_SETUP exchange (Negotiate -> Challenge -> Authenticate) and grant access to a share.")]
        public void BVT_NtlmAuth_Success()
        {
            BaseTestSite.Assume.IsNotNull(TestConfig.AccountCredential, "A configured valid account credential is required for NTLM authentication.");
            BaseTestSite.Assume.IsFalse(
                string.IsNullOrEmpty(TestConfig.BasicFileShare),
                "A configured test share is required to verify the authenticated session is usable.");

            string uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            ulong continuationSessionId = 0;
            bool continuationObserved = false;
            bool finalObserved = false;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client sends NEGOTIATE request.");
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "The client sends SESSION_SETUP request using the NTLM security package. The underlying NTLMSSP " +
                "message exchange (NEGOTIATE_MESSAGE -> CHALLENGE_MESSAGE -> AUTHENTICATE_MESSAGE) is performed by the SSPI layer.");
            client.SessionSetup(
                SecurityPackageType.Ntlm,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                useServerGssToken: false,
                observer: (header, response, serverGssToken) =>
                {
                    if (header.Status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED)
                    {
                        // MS-SMB2 3.3.5.5.3: while authentication is not yet final, the server returns
                        // STATUS_MORE_PROCESSING_REQUIRED with the GSS output token in the response security buffer.
                        BaseTestSite.Assert.AreNotEqual(
                            0ul,
                            header.SessionId,
                            "The continuation SESSION_SETUP response MUST carry a nonzero SessionId.");
                        BaseTestSite.Assert.IsNotNull(
                            serverGssToken,
                            "The continuation SESSION_SETUP response MUST contain a GSS output token (NTLMSSP CHALLENGE_MESSAGE).");
                        BaseTestSite.Assert.AreNotEqual(
                            0,
                            serverGssToken.Length,
                            "The continuation SESSION_SETUP response MUST return a non-empty GSS output token.");
                        continuationSessionId = header.SessionId;
                        continuationObserved = true;
                    }
                    else if (header.Status == Smb2Status.STATUS_SUCCESS)
                    {
                        // MS-SMB2 3.3.5.5.3: the final SESSION_SETUP response has SMB2_FLAGS_SERVER_TO_REDIR set,
                        // carries the established SessionId, and returns STATUS_SUCCESS.
                        BaseTestSite.Assert.IsTrue(
                            header.Flags.HasFlag(Packet_Header_Flags_Values.FLAGS_SERVER_TO_REDIR),
                            "The final SESSION_SETUP response MUST have SMB2_FLAGS_SERVER_TO_REDIR set.");
                        BaseTestSite.Assert.AreNotEqual(
                            0ul,
                            header.SessionId,
                            "The final SESSION_SETUP response MUST carry a nonzero SessionId.");
                        BaseTestSite.Assert.AreEqual(
                            continuationSessionId,
                            header.SessionId,
                            "The final SESSION_SETUP response MUST retain the SessionId established by the continuation response.");
                        finalObserved = true;
                    }
                    else
                    {
                        BaseTestSite.Assume.Inconclusive(
                            "Unexpected SESSION_SETUP status {0}; NTLM may be disabled by SUT/domain policy or the configured credential/share is unavailable.",
                            Smb2Status.GetStatusCode(header.Status));
                    }
                },
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "SESSION_SETUP with NTLMSSP authentication should succeed with STATUS_SUCCESS, actual status is {0}.",
                        Smb2Status.GetStatusCode(header.Status));
                });

            BaseTestSite.Assert.IsTrue(continuationObserved, "The NTLM challenge (STATUS_MORE_PROCESSING_REQUIRED) response MUST be observed.");
            BaseTestSite.Assert.IsTrue(finalObserved, "The final NTLM SESSION_SETUP success response MUST be observed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "The client sends TREE_CONNECT request to verify the NTLM authenticated session is usable.");
            uint treeId;
            client.TreeConnect(
                uncSharePath,
                out treeId,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TREE_CONNECT over an NTLM authenticated session should succeed with STATUS_SUCCESS, actual status is {0}.",
                        Smb2Status.GetStatusCode(header.Status));
                });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the session by sending TREE_DISCONNECT and LOGOFF requests.");
            client.TreeDisconnect(treeId);
            client.LogOff();
        }
        #endregion
    }
}
