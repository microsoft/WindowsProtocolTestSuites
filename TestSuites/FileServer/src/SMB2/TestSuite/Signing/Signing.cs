// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class Signing : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        #endregion

        #region Initialization and Cleanup

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

        protected override void TestInitialize()
        {
            base.TestInitialize();

            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
        }

        protected override void TestCleanup()
        {
            client.Disconnect();
            base.TestCleanup();
        }

        #endregion

        #region Test cases

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Signing)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE and SESSION_SETUP requests with NEGOTIATE_SIGNING_REQUIRED set.")]
        public void BVT_Signing()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb2002);
            testConfig.CheckServerEncrypt();
            TestConfig.CheckSigning();
            #endregion
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE request with NEGOTIATE_SIGNING_REQUIRED flag set.");
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request with NEGOTIATE_SIGNING_REQUIRED flag set.");
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);

            string uncSharepath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request.");
            client.TreeConnect(
                uncSharepath,
                out treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: TREE_DISCONNECT; LOG_OFF");
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Signing)]
        [Description("This test case is designed to test whether server set the Signature field to zero in Encrypted message.")]
        public void Signing_VerifySignatureWhenEncrypted()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE with the capability GLOBAL_CAP_ENCRYPTION.");
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_ENCRYPTION
                );

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request and expects response.");
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED);

            string uncSharepath =
                Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.EncryptedFileShare);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT to share: {0}", uncSharepath);
            client.TreeConnect(
                uncSharepath,
                out treeId,
                (Packet_Header header, TREE_CONNECT_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TreeConnect should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    BaseTestSite.Assert.AreEqual(
                    ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA,
                    ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA & response.ShareFlags,
                    "Server should set SMB2_SHAREFLAG_ENCRYPT_DATA for ShareFlags field in TREE_CONNECT response");

                });

            // After calling this method, client will send encrypted message after tree connect.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client enables per share encryption: TreeId=0x{0:x}", treeId);
            client.SetTreeEncryption(treeId, true);

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrypted CREATE request and expects success.");
            client.Create(
                treeId,
                GetTestFileName(uncSharepath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                null,
                shareAccess: ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.IsTrue(
                        !header.Signature.Any(e => e != 0),
                        "[MS-SMB2] 3.3.4.1.1 If the server encrypts the message, as specified in section 3.1.4.3, the server MUST set the Signature field of the SMB2 header to zero, actually the Signature field is [{0}].", string.Join(", ", header.Signature));
                });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }
        #endregion
    }
}
