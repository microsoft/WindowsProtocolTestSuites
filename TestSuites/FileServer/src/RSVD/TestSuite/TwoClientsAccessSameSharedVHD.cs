// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.RSVD.TestSuite
{
    [TestClass]
    public class TwoClientsAccessSameSharedVHD : RSVDTestBase
    {
        #region Fields

        /// <summary>
        /// The second Rsvd client
        /// The instance is created when needed.
        /// </summary>
        private RsvdClient secondClient;
        #endregion
        #region Test Suite Initialization

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        protected override void TestCleanup()
        {
            if (secondClient != null)
            {
                try
                {
                    secondClient.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect second client: {0}", ex.ToString());
                }
                secondClient.Dispose();
            }

            base.TestCleanup();
        }

        [TestMethod]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Check if server handles Read request to a shared virtual disk file from two clients correctly.")]
        public void TwoClientsReadSameSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	The first client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, 0);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	The first client reads file content and expects success.");
            byte[] payload;
            uint status = client.Read(0, 512, out payload);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Read content of shared virtual disk file from the first client should succeed, actual status: {0}",
                GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	The second client opens a shared virtual disk file and expects success.");
            this.secondClient = new RsvdClient(TestConfig.Timeout);
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, 0, rsvdClient: secondClient);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	The second client reads file content and expects success.");
            status = secondClient.Read(0, 512, out payload);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Read content of shared virtual disk file from the second client should succeed, actual status: {0}",
                GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5.	The first client closes the file.");
            client.CloseSharedVirtualDisk();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6.	The second client closes the file.");
            secondClient.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Check if server handles Write request to a shared virtual disk file from two clients correctly.")]
        public void TwoClientsWriteSameSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	The first client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, 0);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	The first client writes file content and expects success.");
            byte[] payload = new byte[512];
            uint status = client.Write(0, payload);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Write content of shared virtual disk file from the first client should succeed, actual status: {0}",
                GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	The second client opens a shared virtual disk file and expects success.");
            this.secondClient = new RsvdClient(TestConfig.Timeout);
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, 0, rsvdClient: secondClient);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	The second client writes file content and expects success.");
            status = secondClient.Write(0, payload);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Write content of shared virtual disk file from the second client should succeed, actual status: {0}",
                GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5.	The first client closes the file.");
            client.CloseSharedVirtualDisk();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6.	The second client closes the file.");
            secondClient.CloseSharedVirtualDisk();
        }
    }
}