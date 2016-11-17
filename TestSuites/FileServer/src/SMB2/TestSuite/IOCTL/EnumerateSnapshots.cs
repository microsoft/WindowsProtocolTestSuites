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
    public class EnumerateSnapshots : SMB2TestBase
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
        [TestCategory(TestCategories.FsctlEnumerateSnapshots)]
        [Description("This test case is designed to test basic functionality of FSCTL_SRV_ENUMERATE_SNAPSHOTS.")]
        public void BVT_EnumerateSnapshots()
        {
            uint treeId;
            FILEID fileId;

            OpenFile(true, out treeId, out fileId);

            SRV_SNAPSHOT_ARRAY snapShotArray;
            client.EnumerateSnapshots(
                treeId,
                fileId,
                out snapShotArray,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "FSCTL_SRV_ENUMERATE_SNAPSHOTS should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                });

            BaseTestSite.Assert.AreEqual(
                TestConfig.NumberOfPreviousVersions, snapShotArray.NumberOfSnapShots, "NumberOfSnapShots should be {0}.", TestConfig.NumberOfPreviousVersions);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "NumberOfSnapShotsReturned is {0}.", snapShotArray.NumberOfSnapShotsReturned);

            string[] versionArray = System.Text.Encoding.Unicode.GetString(snapShotArray.SnapShots).Split('\0');
            BaseTestSite.Assert.AreEqual(
                snapShotArray.NumberOfSnapShotsReturned + 2,
                (uint)versionArray.Length,
                "The field \"SnapShots\" should be separated by UNICODE null characters and terminated by two UNICODE null characters.");

            DateTime dt;
            for (int i = 0; i < snapShotArray.NumberOfSnapShotsReturned; ++i)
            {
                BaseTestSite.Assert.IsTrue(
                    DateTime.TryParseExact(versionArray[i], "@GMT-yyyy.MM.dd-HH.mm.ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt),
                    "This SnapShot is {0}. The format of each SnapShot should be \"@GMT-YYYY.MM.DD-HH.MM.SS\". ", versionArray[i]);
            }

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlEnumerateSnapshots)]
        [Description("This test case is designed to test basic functionality of FSCTL_SRV_ENUMERATE_SNAPSHOTS.")]
        public void EnumerateSnapshot_NoPrevioursVersions()
        {
            uint treeId;
            FILEID fileId;

            OpenFile(false, out treeId, out fileId);

            SRV_SNAPSHOT_ARRAY snapShotArray;
            client.EnumerateSnapshots(
                treeId,
                fileId,
                out snapShotArray,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "FSCTL_SRV_ENUMERATE_SNAPSHOTS should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                });

            BaseTestSite.Assert.AreEqual((uint)0, snapShotArray.NumberOfSnapShots, "NumberOfSnapShots should be zero.");
            BaseTestSite.Assert.AreEqual((uint)0, snapShotArray.NumberOfSnapShotsReturned, "NumberOfSnapShotsReturned should be zero.");
            BaseTestSite.Assert.AreEqual((uint)0, snapShotArray.SnapShotArraySize, "SnapShotArraySize should be zero.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }


        private void OpenFile(bool withPreviousVersions, out uint treeId, out FILEID fileId)
        {
            #region Check Applicability
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_SRV_ENUMERATE_SNAPSHOTS);
            #endregion

            // Parse full path to separate properties.
            string filePath = TestConfig.FilePathContainingPreviousVersions;

            string shareName = filePath.Substring(0, filePath.IndexOf(@"\"));
            string fileName;

            if (withPreviousVersions)
            {
                fileName = filePath.Substring(shareName.Length + 1);
            }
            else
            {
                fileName = string.Format("EnumerateSnapshots_{0}.txt", Guid.NewGuid());
            }

            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "Negotiate should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });

            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            client.TreeConnect(Smb2Utility.GetUncPath(TestConfig.SutComputerName, shareName), out treeId);

            Smb2CreateContextResponse[] serverCreateContexts;
            client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);
        }
    }
}
