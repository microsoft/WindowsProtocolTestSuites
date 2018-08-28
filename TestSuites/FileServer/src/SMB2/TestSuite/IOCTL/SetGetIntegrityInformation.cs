// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class SetGetIntegrityInformation : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        private uint status;
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
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlSetGetIntegrityInformation)]
        [Description("This test case is designed to test whether server can handle IOCTL FSCTL_GET_INTEGRITY_INFORMATION and FSCTL_SET_INTEGRITY_INFORMATION.")]
        public void BVT_SetGetIntegrityInfo()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION, CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION);
            #endregion

            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client creates a file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE");
            status = client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Negotiation should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });

            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            uint treeId;
            string uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.FileShareSupportingIntegrityInfo);
            status = client.TreeConnect(uncSharePath, out treeId);

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            status = client.Create(
                treeId,
                GetTestFileName(uncSharePath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);

            FSCTL_GET_INTEGRITY_INFO_OUTPUT getIntegrityInfo;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends IOCTL request with FSCTL_GET_INTEGRITY_INFORMATION.");
            status = client.GetIntegrityInfo(treeId, fileId, out getIntegrityInfo);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Integrity info returned in FSCTL_GET_INTEGRITY_INFO request: ChecksumAlgorithm {0}, Flags {1}, ChecksumChunkSizeInBytes {2}, ClusterSizeInBytes {3}", 
                getIntegrityInfo.ChecksumAlgorithm, getIntegrityInfo.Flags, getIntegrityInfo.ChecksumChunkSizeInBytes, getIntegrityInfo.ClusterSizeInBytes);

            FSCTL_SET_INTEGRIY_INFO_INPUT setIntegrityInfo;
            setIntegrityInfo.ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFO_INPUT_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64;
            setIntegrityInfo.Flags = FSCTL_SET_INTEGRITY_INFO_INPUT_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF;
            setIntegrityInfo.Reserved = FSCTL_SET_INTEGRITY_INFO_INPUT_RESERVED.V1;
            byte[] buffer = TypeMarshal.ToBytes<FSCTL_SET_INTEGRIY_INFO_INPUT>(setIntegrityInfo);
            
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Attempt to set integrity info with ChecksumAlgrithm {0}, Flags {1}",
                setIntegrityInfo.ChecksumAlgorithm, setIntegrityInfo.Flags);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends IOCTL request with FSCTL_SET_INTEGRITY_INFORMATION after changed the value of the following fields in FSCTL_SET_INTEGRIY_INFO_INPUT: ChecksumAlgorithm, Flags, Reserved.");
            status = client.SetIntegrityInfo(treeId, fileId, buffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends IOCTL request with FSCTL_GET_INTEGRITY_INFORMATION.");
            status = client.GetIntegrityInfo(treeId, fileId, out getIntegrityInfo);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Current ChecksumAlgorithm is " + getIntegrityInfo.ChecksumAlgorithm);
            BaseTestSite.Assert.AreEqual(
                (ushort)setIntegrityInfo.ChecksumAlgorithm,
                (ushort)getIntegrityInfo.ChecksumAlgorithm,
                "ChecksumAlgorithm field after set should be {0}, actual value is {1}", setIntegrityInfo.ChecksumAlgorithm, getIntegrityInfo.ChecksumAlgorithm);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Current Flags is " + getIntegrityInfo.Flags);
            BaseTestSite.Assert.AreEqual(
                (uint)setIntegrityInfo.Flags,
                (uint)getIntegrityInfo.Flags,
                "Flags field after set should be {0}, actual value is {1}", setIntegrityInfo.Flags, getIntegrityInfo.Flags);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF.");
            status = client.Close(treeId, fileId);
            status = client.TreeDisconnect(treeId);
            status = client.LogOff();
        }
    }
}
