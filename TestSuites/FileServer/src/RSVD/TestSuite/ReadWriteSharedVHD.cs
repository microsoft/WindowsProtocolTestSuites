// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
    public class ReadWriteSharedVHD : RSVDTestBase
    {

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

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server handles Read request to a shared virtual disk file correctly.")]
        public void BVT_ReadSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file successfully.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client reads file content and expects success.");
            byte[] payload;
            uint status = client.Read(0, 512, out payload);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Read content of shared virtual disk file should succeed, actual status: {0}",
                GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server handles Write request to a shared virtual disk file correctly.")]
        public void BVT_WriteSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file successfully.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client writes file content and expects success.");

            byte[] payload = new byte[512];
            uint status = client.Write(0, payload);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Write content of shared virtual disk file should succeed, actual status: {0}",
                GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }
    }
}