﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.RSVD.TestSuite
{
    [TestClass]
    public class QueryVHDSetFileInfo : RSVDTestBase
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
        [TestCategory(TestCategories.RsvdVersion2)]
        [Description("Check if server supports handling tunnel operation SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_REQUEST while SetFileInfo is SvhdxSetFileInformationTypeSnapshotList.")]
        public void BVT_Query_VHDSet_FileInfo_SnapshotList()
        {
            ulong requestId = 0;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, requestId++);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends tunnel operation SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_REQUEST to server and expects success.");
            SetFile_InformationType setFileInforType = SetFile_InformationType.SvhdxSetFileInformationTypeSnapshotList;
            Snapshot_Type snapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            //SnapshotId is set to empty when SetFileInformationType is SvhdxSetFileInformationTypeSnapshotList because 
            //this message is intending to get the snapshot id list from server, when you need to get a snapshot entry, you need to pass in the specific snapshot id
            System.Guid snapshotId = System.Guid.Empty;

            // Get VHDSet info
            byte[] payload = client.CreateTunnelGetVHDSetFileInfoRequest(
                setFileInforType,
                snapshotType,
                snapshotId);
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_SNAPSHOT_LIST_RESPONSE? response;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_SNAPSHOT_LIST_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VHDSET_QUERY_INFORMATION,
                requestId,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VHDSET_QUERY_INFORMATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, requestId++);

            VerifyFieldInResponse("SetFileInformationType", SetFile_InformationType.SvhdxSetFileInformationTypeSnapshotList, response.Value.SetFileInformationType);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion2)]
        [Description("Check if server supports handling tunnel operation SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_REQUEST while SetFileInfo is SvhdxSetFileInformationTypeSnapshotEntry.")]
        public void BVT_Query_VHDSet_FileInfo_SnapshotEntry()
        {
            ulong requestId = 0;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, requestId++);

            System.Guid snapshotId = CreateSnapshot(ref requestId, client);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends tunnel operation SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_REQUEST to server and expects success.");
            SetFile_InformationType setFileInforType = SetFile_InformationType.SvhdxSetFileInformationTypeSnapshotEntry;
            Snapshot_Type snapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_SNAPSHOT_ENTRY_RESPONSE? snapshotEntryResponse;
            byte[] payload = client.CreateTunnelGetVHDSetFileInfoRequest(
                setFileInforType,
                snapshotType,
                snapshotId);
            uint status = client.TunnelOperation<SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_SNAPSHOT_ENTRY_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VHDSET_QUERY_INFORMATION,
                requestId,
                payload,
                out header,
                out snapshotEntryResponse);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VHDSET_QUERY_INFORMATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, requestId++);

            VerifyFieldInResponse("SetFileInformationType", SetFile_InformationType.SvhdxSetFileInformationTypeSnapshotEntry, snapshotEntryResponse.Value.SetFileInformationType);

            DeleteSnapshot(ref requestId, snapshotId, client);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

    }
}
