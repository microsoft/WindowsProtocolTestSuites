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
        private const uint SizeOfEmptySnapShotArray = 16;
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
        [TestCategory(TestCategories.FsctlEnumerateSnapShots)]
        [Description("This test case is designed to test basic functionality of FSCTL_SRV_ENUMERATE_SNAPSHOTS.")]
        public void BVT_EnumerateSnapShots()
        {
            #region Check Applicability
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_SRV_ENUMERATE_SNAPSHOTS);
            #endregion

            uint treeId;
            FILEID fileId;
            OpenFile(out treeId, out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Enumerate snapshots by sending the first IOCTL request: FSCTL_SRV_ENUMERATE_SNAPSHOTS, with MaxOutputResponse set to 16.");
            SRV_SNAPSHOT_ARRAY snapShotArray;
            client.EnumerateSnapShots(
                treeId,
                fileId,
                SizeOfEmptySnapShotArray,
                out snapShotArray,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "FSCTL_SRV_ENUMERATE_SNAPSHOTS should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                });
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Verify SRV_SNAPSHOT_ARRAY returned in response.");
            BaseTestSite.Assert.AreEqual(
                TestConfig.NumberOfPreviousVersions, snapShotArray.NumberOfSnapShots, "NumberOfSnapShots should be {0}.", TestConfig.NumberOfPreviousVersions);
            BaseTestSite.Assert.AreEqual(
                (uint)0, snapShotArray.NumberOfSnapShotsReturned, "NumberOfSnapShotsReturned should be 0.");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Enumerate snapshots by sending the second IOCTL request: FSCTL_SRV_ENUMERATE_SNAPSHOTS, with MaxOutputResponse set to 65536.");
            client.EnumerateSnapShots(
                treeId,
                fileId,
                Smb2FunctionalClient.DefaultMaxOutputResponse,
                out snapShotArray,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "FSCTL_SRV_ENUMERATE_SNAPSHOTS should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                });

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Verify SRV_SNAPSHOT_ARRAY returned in response.");
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

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        private void OpenFile(out uint treeId, out FILEID fileId)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Open the file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE.");
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
                });

            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            client.TreeConnect(Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare), out treeId);

            Smb2CreateContextResponse[] serverCreateContexts;
            client.Create(
                treeId,
                GetTestFileName(Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare)),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);
        }
    }
}
