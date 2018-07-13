// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public partial class AppInstanceIdExtendedTest : SMB2TestBase
    {
        #region Variables
        private string fileName;
        private string testDirectory;
        private string contentWrite;
        private string contentRead;
        private string uncSharePath;
        private uint status;
        private Guid appInstanceId;
        private Smb2FunctionalClient clientForInitialOpen;
        private Smb2FunctionalClient clientForReOpen;
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

            clientForInitialOpen = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            clientForReOpen = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            appInstanceId = Guid.NewGuid();
            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            fileName = GetTestFileName(uncSharePath);
        }

        protected override void TestCleanup()
        {
            if (clientForInitialOpen != null)
            {
                try
                {
                    clientForInitialOpen.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect clientBeforeFailover: {0}", ex.ToString());
                }
            }

            if (clientForReOpen != null)
            {
                try
                {
                    clientForReOpen.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect clientAfterFailover: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion

        #region Common Method
        /// <summary>
        /// Connect share on file server
        /// </summary>
        /// <param name="serverIp">Server IP address used for connection</param>
        /// <param name="clientIp">Client IP address used for connection</param>
        /// <param name="client">Client object to initialize the connection</param>
        /// <param name="treeId">Out param tree id connected</param>
        /// <param name="enableEncryptionPerShare">True indicates encryption enabled per share, otherwise disabled</param>
        private void ConnectShare(IPAddress serverIp, IPAddress clientIp, Smb2FunctionalClient client, out uint treeId, bool enableEncryptionPerShare = false)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Connect to share {0} with following steps.", uncSharePath);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Connect to server via Nic with Ip {0}", clientIp.ToString());
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, serverIp, clientIp);

            #region Negotiate
            Capabilities_Values clientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            status = client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilities,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "CREATE should succeed.");

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });
            #endregion

            #region SESSION_SETUP
            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Global encryption disabled");
            #endregion

            #region TREE_CONNECT to share
            status = client.TreeConnect(
                uncSharePath,
                out treeId,
                (Packet_Header header, TREE_CONNECT_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TreeConnect should succeed.");

                    if (enableEncryptionPerShare)
                    {
                        BaseTestSite.Assert.AreEqual(
                            ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA,
                            ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA & response.ShareFlags,
                            "Server should set SMB2_SHAREFLAG_ENCRYPT_DATA for ShareFlags field in TREE_CONNECT response");
                    }
                });

            client.SetTreeEncryption(treeId, enableEncryptionPerShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Per share encryption for TreeId=0x{0:x} : {1}", treeId, enableEncryptionPerShare);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Finish connecting to share {0}", uncSharePath);
        }

        #endregion
    }
}
