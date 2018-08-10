// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class Encryption : SMB2TestBase
    {
        /// <summary>
        /// Specifies the type of how to enable encryption in client
        /// </summary>
        public enum EnableEncryptionType
        {
            /// <summary>
            /// Encryption is enabled on the whole session
            /// </summary>
            EnableEncryptionPerSession,

            /// <summary>
            /// Encryption is enabled on the specific share
            /// </summary>
            EnableEncryptionPerShare
        }

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
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Encryption)]
        [Description("This test case is designed to test whether server can handle per share encryption correctly.")]
        public void BVT_Encryption_PerShareEncryptionEnabled()
        {
            NegotiateWithoutContext();
            PostNegotiateOperations(EnableEncryptionType.EnableEncryptionPerShare, connectEncryptedShare: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Encryption)]
        [Description("This test case is designed to test whether server can handle encryption request under global encryption enabled.")]
        public void BVT_Encryption_GlobalEncryptionEnabled()
        {
            NegotiateWithoutContext();
            PostNegotiateOperations(EnableEncryptionType.EnableEncryptionPerSession, connectEncryptedShare: false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Encryption)]
        [Description("This case is to ensure server could handle encrypted requests correctly with dialect 3.11, SMB2_ENCRYPTION_CAPABILITIES context.")]
        public void BVT_Encryption_SMB311()
        {
            NegotiateWithEncryptionCapabilitiesContext(EncryptionAlgorithm.ENCRYPTION_AES128_GCM, sendCipherArray: true);
            PostNegotiateOperations(EnableEncryptionType.EnableEncryptionPerSession, connectEncryptedShare: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Encryption)]
        [Description("This case is to ensure server could handle encrypted requests correctly with dialect 3.11, SMB2_ENCRYPTION_CAPABILITIES context and AES-128-CCM as encryption algorithm.")]
        public void BVT_Encryption_SMB311_CCM()
        {
            NegotiateWithEncryptionCapabilitiesContext(EncryptionAlgorithm.ENCRYPTION_AES128_CCM);
            PostNegotiateOperations(EnableEncryptionType.EnableEncryptionPerSession, connectEncryptedShare: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Encryption)]
        [Description("This case is to ensure server could handle encrypted requests correctly with dialect 3.11, SMB2_ENCRYPTION_CAPABILITIES context and AES-128-GCM as encryption algorithm.")]
        public void BVT_Encryption_SMB311_GCM()
        {
            NegotiateWithEncryptionCapabilitiesContext(EncryptionAlgorithm.ENCRYPTION_AES128_GCM);
            PostNegotiateOperations(EnableEncryptionType.EnableEncryptionPerSession, connectEncryptedShare:true);
        }

        #endregion

        private void NegotiateWithoutContext()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE with the capability GLOBAL_CAP_ENCRYPTION.");
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_ENCRYPTION
                );
        }
        
        private void NegotiateWithEncryptionCapabilitiesContext(EncryptionAlgorithm cipherId, bool sendCipherArray = false)
        {
            if (cipherId == EncryptionAlgorithm.ENCRYPTION_NONE)
            {
                throw new ArgumentException("CipherId should be either AES-128-CCM or AES-128-GCM.");
            }

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION);
            TestConfig.CheckEncryptionAlgorithm(cipherId);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends NEGOTIATE request with dialect 3.11, SMB2_ENCRYPTION_CAPABILITIES context. {0} is as the preferred cipher algorithm. ", cipherId);
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Server should reply NEGOTIATE response with dialect 3.11, SMB2_ENCRYPTION_CAPABILITIES context and {0} as cipher algorithm. ", cipherId);
            PreauthIntegrityHashID[] preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
            EncryptionAlgorithm[] encryptionAlgs = null;
            if (sendCipherArray)
            {
                encryptionAlgs = new EncryptionAlgorithm[] { 
                    cipherId, 
                    cipherId == EncryptionAlgorithm.ENCRYPTION_AES128_CCM? EncryptionAlgorithm.ENCRYPTION_AES128_GCM : EncryptionAlgorithm.ENCRYPTION_AES128_CCM };
            }
            else
            {
                encryptionAlgs = new EncryptionAlgorithm[] { cipherId };
            }

            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                TestConfig.RequestDialects,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_ENCRYPTION,
                preauthHashAlgs: preauthHashAlgs,
                encryptionAlgs: encryptionAlgs);

            if (sendCipherArray)
            {
                BaseTestSite.Assert.IsTrue(
                    TestConfig.SupportedEncryptionAlgorithmList.Contains(client.SelectedCipherID),
                    "[MS-SMB2] 3.3.5.4 The server MUST set Connection.CipherId to one of the ciphers in the client's " +
                        "SMB2_ENCRYPTION_CAPABILITIES Ciphers array in an implementation-specific manner.");
            }
            else
            {
                BaseTestSite.Assert.AreEqual(cipherId, client.SelectedCipherID, "The selected Cipher Id should be {0}", cipherId);
            }
        }

        /// <summary>
        /// Operations after Negotiate, from Session Setup to Log off.
        /// </summary>
        private void PostNegotiateOperations(EnableEncryptionType enableEncryptionType, bool connectEncryptedShare)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request and expects response.");
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            if (enableEncryptionType == EnableEncryptionType.EnableEncryptionPerSession)
            {
                // After calling this method, client will send encrypted message after session setup
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client enables global encryption.");
                client.EnableSessionSigningAndEncryption(enableSigning: false, enableEncryption: true);
            }

            string uncSharepath =
                Smb2Utility.GetUncPath(TestConfig.SutComputerName, connectEncryptedShare ? TestConfig.EncryptedFileShare : TestConfig.BasicFileShare);
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

                    if (connectEncryptedShare)
                    {
                        BaseTestSite.Assert.AreEqual(
                        ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA,
                        ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA & response.ShareFlags,
                        "Server should set SMB2_SHAREFLAG_ENCRYPT_DATA for ShareFlags field in TREE_CONNECT response");
                    }
                    else
                    {
                        BaseTestSite.Assert.AreNotEqual(
                            ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA,
                            ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA & response.ShareFlags,
                            "Server should not set SMB2_SHAREFLAG_ENCRYPT_DATA for ShareFlags field in TREE_CONNECT response");
                    }
                });

            if (enableEncryptionType == EnableEncryptionType.EnableEncryptionPerShare)
            {
                // After calling this method, client will send encrypted message after tree connect.
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client enables per share encryption: TreeId=0x{0:x}", treeId);
                client.SetTreeEncryption(treeId, true);
            }

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrypted CREATE request and expects success.");
            client.Create(
                treeId,
                GetTestFileName(uncSharepath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts);
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrpyted WRITE request and expects success.");
            client.Write(treeId, fileId, content);

            string actualContent;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrypted READ request and expects success.");
            client.Read(treeId, fileId, 0, (uint)content.Length, out actualContent);

            BaseTestSite.Assert.IsTrue(
                content.Equals(actualContent),
                "File content read should be identical to that has been written.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }
    }
}
