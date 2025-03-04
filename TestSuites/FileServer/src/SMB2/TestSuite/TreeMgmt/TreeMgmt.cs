﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.TreeMgmt
{
    [TestClass]
    public class TreeMgmt : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        private string sharePath;
        private SutCommonControlAdapterAccessor sutCommonControlAdapterAccessor;
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
            sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            sutCommonControlAdapterAccessor = new SutCommonControlAdapterAccessor(BaseTestSite);
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
        [TestCategory(TestCategories.Tree)]
        [Description("This test case is designed to test whether server can handle TREE_CONNECT and TREE_DISCONNECT requests correctly.")]
        public void BVT_TreeMgmt_TreeConnectAndDisconnect()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: NEGOTIATE; SESSION_SETUP");
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request");
            uint treeId;
            client.TreeConnect(sharePath, out treeId,
                (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                       "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    BaseTestSite.Assert.AreNotEqual<uint>(header.TreeId, uint.MaxValue, "The SMB2 server MUST reserve -1 for invalid TreeId.");
                });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_DISCONNECT request.");
            client.TreeDisconnect(treeId);
        }
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Tree)]
        [Description("This test case is designed to test whether server can disconnect the connection when Connection.Dialect is 3.1.1 and the TreeConnect request is not signed or not encrypted.")]
        public void BVT_TreeMgmt_SMB311_Disconnect_NoSignedNoEncryptedTreeConnect()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);

            if (TestConfig.IsServerSigningRequired)
                BaseTestSite.Assert.Inconclusive("Test case is only applicable when security signature is not required by the server " +
                    "because if signing or encryption is required in server, the server will fail the TreeConnect with STATUS_ACCESS_DENIED, which is not the purpose of this case.");
            if (TestConfig.IsGlobalEncryptDataEnabled && TestConfig.IsGlobalRejectUnencryptedAccessEnabled)
                BaseTestSite.Assert.Inconclusive("Test case is only applicable when encryption is not required by the server " +
                    "because if signing or encryption is required in server, the server will fail the TreeConnect with STATUS_ACCESS_DENIED, which is not the purpose of this case.");
            #endregion 

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE request without NEGOTIATE_SIGNING_REQUIRED and GLOBAL_CAP_ENCRYPTION set.");

            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NONE,
                Capabilities_Values.NONE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request without NEGOTIATE_SIGNING_REQUIRED flag set.");
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken,
                SESSION_SETUP_Request_SecurityMode_Values.NONE);

            string uncSharepath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request which is not signed or not encrypted and expects server disconnects the connection.");

            try
            {
                client.EnableSessionSigningAndEncryption(enableSigning: false, enableEncryption: false);
                // Trigger Server Disconnect event
                client.TreeConnect(
                    Packet_Header_Flags_Values.NONE,
                    uncSharepath,
                    out treeId,
                    checker: (Packet_Header header, TREE_CONNECT_Response response) =>
                    {
                        BaseTestSite.Assert.AreNotEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "TREE_CONNECT should NOT succeed.");
                    });

                // Check if server is still responding
                client.Echo(treeId);
            }
            catch
            {
            }

            // Check if server is disconnected
            BaseTestSite.Assert.IsTrue(client.Smb2Client.IsServerDisconnected,
                "[MS-SMB2] 3.3.5.7 If Connection.Dialect is \"3.1.1\" and Session.IsAnonymous and Session.IsGuest are set to FALSE" +
                " and the request is not signed or not encrypted, then the server MUST disconnect the connection. " +
                "Server did {0}disconnect the connection.", client.Smb2Client.IsServerDisconnected ? "" : "not ");
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Tree)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test server can handle a TreeConnect request with flag SMB2_SHAREFLAG_CLUSTER_RECONNECT successfully.")]
        public void TreeMgmt_SMB311_CLUSTER_RECONNECT()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: NEGOTIATE; SESSION_SETUP");
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request with flag SMB2_SHAREFLAG_CLUSTER_RECONNECT and expects STATUS_SUCCESS.");
            uint treeId;
            client.TreeConnect(sharePath, out treeId,
                (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                       "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                },
                TreeConnect_Flags.SMB2_SHAREFLAG_CLUSTER_RECONNECT);
            client.TreeDisconnect(treeId);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Tree)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test server can handle a TreeConnect request with flag SMB2_SHAREFLAG_EXTENSION_PRESENT successfully.")]
        public void TreeMgmt_SMB311_TREE_CONNECT_EXTENSION_PRESENT()
        {
            #region Check Applicability
            TestConfig.CheckPlatform(Platform.WindowsServer2019);
            TestConfig.CheckDialect(DialectRevision.Smb311);
            #endregion

            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: CONNECT; NEGOTIATE; SESSION_SETUP");
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            //Use domain credential to do session setup (e.g. contoso.com\administrator)
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            string infraSharePath = string.Format(@"\\{0}\{1}", TestConfig.ClusteredInfrastructureFileServerName, TestConfig.InfrastructureRootShare);
            uint treeId;

            string anotherUserName = TestConfig.NonAdminUserName;
            _WindowsIdentity identity = sutCommonControlAdapterAccessor.GetWindowsIdentity(anotherUserName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request with extension context and expects success");
            //Use another domain account(e.g. contoso\nonadmin) as an idenity passed in tree connect extension
            client.TreeConnect(
                infraSharePath,
                out treeId,
                (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                       "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    BaseTestSite.Assert.IsTrue(
                        response.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_IDENTITY_REMOTING),
                        "The share should support identity remoting, actually server returns {0}.", response.ShareFlags.ToString());
                },
                TreeConnect_Flags.SMB2_SHAREFLAG_EXTENSION_PRESENT,
                identity);

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client impersonates another log in user to send CREATE request and expects success.");
            client.Create(
                treeId,
                GetTestFileName(infraSharePath),
                CreateOptions_Values.FILE_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                       "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                }
                );

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client.Close(treeId, fileId);

            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Tree)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test server responds with SHAREFLAG_COMPRESS_DATA to a TreeConnect request for a share that supports compression.")]
        public void TreeMgmt_SMB311_COMPRESS_DATA()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);
            TestConfig.CheckPlatform(Platform.WindowsServer2022);
            if (string.IsNullOrEmpty(testConfig.CompressedFileShare))
                Assert.Inconclusive("This test requires a share with compression enabled");
            #endregion

            var compressedSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.CompressedFileShare);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: NEGOTIATE; SESSION_SETUP");

            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request and expects STATUS_SUCCESS with flag SMB2_SHAREFLAG_COMPRESS_DATA.");
            uint treeId;

            client.TreeConnect(compressedSharePath, out treeId,
                (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                       "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));

                    BaseTestSite.Assert.IsTrue(
                        response.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_COMPRESS_DATA),
                        "The share should support compress data, actually server returns {0}.", response.ShareFlags.ToString());
                });
            client.TreeDisconnect(treeId);
        }
    }
}
