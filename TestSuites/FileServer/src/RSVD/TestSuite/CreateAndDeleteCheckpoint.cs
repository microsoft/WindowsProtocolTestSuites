// Copyright (c) Microsoft. All rights reserved.
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
    public class CreateAndDeleteCheckpoint : RSVDTestBase
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
        [Description("Check if server supports handling tunnel operation to create and delete a VM checkpoint.")]
        public void BVT_Create_Delete_Checkpoint()
        {
            ulong requestId = 0;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, requestId++);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends the first tunnel operation SVHDX_META_OPERATION_START_REQUEST to create a snapshot, with Stage1 set to Initialize and expects success.");
            SVHDX_META_OPERATION_START_REQUEST startRequest = new SVHDX_META_OPERATION_START_REQUEST();
            startRequest.TransactionId = System.Guid.NewGuid();
            startRequest.OperationType = Operation_Type.SvhdxMetaOperationTypeCreateSnapshot;
            startRequest.Padding = new byte[4];
            SVHDX_META_OPERATION_CREATE_SNAPSHOT createsnapshot = new SVHDX_META_OPERATION_CREATE_SNAPSHOT();
            createsnapshot.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            createsnapshot.Flags = Snapshot_Flags.SVHDX_SNAPSHOT_DISK_FLAG_ENABLE_CHANGE_TRACKING;
            createsnapshot.Stage1 = Stage_Values.SvhdxSnapshotStageInitialize;
            createsnapshot.Stage2 = Stage_Values.SvhdxSnapshotStageInvalid;
            createsnapshot.Stage3 = Stage_Values.SvhdxSnapshotStageInvalid;
            createsnapshot.Stage4 = Stage_Values.SvhdxSnapshotStageInvalid;
            createsnapshot.Stage5 = Stage_Values.SvhdxSnapshotStageInvalid;
            createsnapshot.Stage6 = Stage_Values.SvhdxSnapshotStageInvalid;
            createsnapshot.SnapshotId = System.Guid.NewGuid();
            createsnapshot.ParametersPayloadSize = (uint)0x00000000;
            createsnapshot.Padding = new byte[24];
            byte[] payload = client.CreateTunnelMetaOperationStartCreateSnapshotRequest(
                startRequest,
                createsnapshot);
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_OPERATION_HEADER? response;
            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
            uint status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                true,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START,
                requestId,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, requestId++);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client sends the second tunnel operation SVHDX_META_OPERATION_START_REQUEST to create a snapshot and expects success.");
            createsnapshot.Flags = Snapshot_Flags.SVHDX_SNAPSHOT_FLAG_ZERO;
            createsnapshot.Stage1 = Stage_Values.SvhdxSnapshotStageBlockIO;
            createsnapshot.Stage2 = Stage_Values.SvhdxSnapshotStageSwitchObjectStore;
            createsnapshot.Stage3 = Stage_Values.SvhdxSnapshotStageUnblockIO;
            createsnapshot.Stage4 = Stage_Values.SvhdxSnapshotStageFinalize;
            createsnapshot.Stage5 = Stage_Values.SvhdxSnapshotStageInvalid;
            createsnapshot.Stage6 = Stage_Values.SvhdxSnapshotStageInvalid;
            payload = client.CreateTunnelMetaOperationStartCreateSnapshotRequest(
                startRequest,
                createsnapshot);
            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
            status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                true,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START,
                requestId,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, requestId++);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify the snapshot has been created after the operation.");
            DoUntilSucceed(
                () => CheckSnapshotExisted(ref requestId, client, createsnapshot.SnapshotId),
                TestConfig.Timeout,
                "Retry getting the snapshot to make sure it has been created until succeed within timeout span");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5.	Client sends the operation SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST to delete a snapshot and expects success.");
            SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST deleteRequest = new SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST();
            deleteRequest.SnapshotId = createsnapshot.SnapshotId;
            deleteRequest.PersistReference = PersistReference_Flags.PersistReferenceFalse;
            deleteRequest.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            payload = client.CreateTunnelMetaOperationDeleteSnapshotRequest(
                deleteRequest);
            SVHDX_TUNNEL_OPERATION_HEADER? deleteResponse;
            status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_DELETE_SNAPSHOT,
                requestId,
                payload,
                out header,
                out deleteResponse);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_DELETE_SNAPSHOT, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, requestId++);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6.	Client closes the file.");
            client.CloseSharedVirtualDisk();

        }

        private void CheckSnapshotExisted(ref ulong requestId, RsvdClient client, System.Guid snapshotId)
        {
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
        }
    }
}
