// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.RSVD.TestSuite
{
    [TestClass]
    public class QueryShareVirtualDiskSupport : RSVDTestBase
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
        [Description("Check if server handles SHARED_VIRTUAL_DISK_SUPPORT request to a shared virtual disk file correctly.")]
        public void BVT_QuerySharedVirtualDiskSupport()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file successfully.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "2.	Client sends SVHDX_SHARED_VIRTUAL_DISK_SUPPORT_REQUEST to server and expects success.");

            SVHDX_SHARED_VIRTUAL_DISK_SUPPORT_RESPONSE? response;
            uint status = client.QuerySharedVirtualDiskSupport(out response);
            uint sharedVirtualDiskSupport = TestConfig.ServerServiceVersion == (uint)ServerServiceVersion.ProtocolVersion1 ? (uint)SharedVirtualDiskSupported.SharedVirtualDiskSupported : (uint)SharedVirtualDiskSupported.SharedVirtualDiskSnapshotsSupported;

            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Query Shared Virtual Disk Support should succeed, actual status is {0}",
                GetStatus(status));
            BaseTestSite.Assert.AreEqual(
                sharedVirtualDiskSupport,
                response.Value.SharedVirtualDiskSupport,
                "If ServerServiceVersion is equal to RSVD protocol version 1(0x00000001)," +
                "the server MUST set SharedVirtualDiskSupport to SharedVirtualDiskSupported." +
                "Otherwise the server MUST set SharedVirtualDiskSupport to SharedVirtualDiskVer2OperationsSupported." +
                "The ServerServiceVersion actual value is {0}, the SharedVirtualDiskSupport actual value is {1}.",
                TestConfig.ServerServiceVersion,
                response.Value.SharedVirtualDiskSupport);
            BaseTestSite.Assert.AreEqual(
                HandleState.HandleStateShared,
                response.Value.SharedVirtualDiskHandleState,
                "SharedVirtualDiskHandleState should be HandleStateShared, actual is {0}",
                response.Value.SharedVirtualDiskHandleState);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }
    }
}