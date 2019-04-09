// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;

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
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports handling tunnel operation to create and delete a VM checkpoint.")]
        public void BVT_Create_Delete_Checkpoint()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2);
            Guid snapshotId = Guid.NewGuid();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends the first tunnel operation SVHDX_META_OPERATION_START_REQUEST to create a snapshot, with Stage1 set to Initialize and expects success.");
            CreateSnapshot(snapshotId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client sends the operation SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST to delete a snapshot and expects success.");
            DeleteSnapshot(snapshotId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion2)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports querying a list of changed ranges since the designated snapshot.")]
        public void BVT_QueryVirtualDiskChanges()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2);

            Guid snapshotId = Guid.NewGuid();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends the first tunnel operation SVHDX_META_OPERATION_START_REQUEST to create a snapshot.");
            CreateSnapshot(snapshotId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the open.");
            client.CloseSharedVirtualDisk();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	Client reopens the shared virtual disk file and expects success.");

            RequestIdentifier = 0;
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5.	Client sends Write request and expects success.");
            byte[] payload = Smb2Utility.CreateRandomByteArray(512);
            uint status = client.Write(0, payload);
            BaseTestSite.Assert.AreEqual(
                (uint)0,
                status,
                "Write Status should be {0}, actual is {1}", GetStatus(0), GetStatus(status));
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6.	Client creates another snapshot.");
            Guid snapshotId2 = Guid.NewGuid();
            CreateSnapshot(snapshotId2);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7.	Client sends the tunnel operation SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST to query the changes since the first snapshot.");

            SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST queryRequest = new SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST();
            queryRequest.TargetSnapshotId = snapshotId2;
            queryRequest.LimitSnapshotId = snapshotId;
            queryRequest.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            queryRequest.ByteOffset = 0;
            queryRequest.ByteLength = 1024;

            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REPLY? response;
            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
            status = client.TunnelOperation<SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REPLY>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES,
                ++RequestIdentifier,
                client.CreateQueryVirtualDiskChangeRequest(queryRequest),
                out header,
                out response);
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);

            BaseTestSite.Assert.AreEqual((uint)1, response.Value.RangeCount, "RangeCount should be 1");
            BaseTestSite.Assert.AreEqual((ulong)0, response.Value.Ranges[0].ByteOffset, "ByteOffset should be 0");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "8.	Client deletes the two snapshots.");

            DeleteSnapshot(snapshotId);
            DeleteSnapshot(snapshotId2);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "9.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion2)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports applying a specified snapshot.")]
        public void BVT_ApplySnapshot()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Client reads 512 bytes and saves it for later comparation.");
            byte[] byteBeforeChanged = null;
            uint status = client.Read(0, 512, out byteBeforeChanged);
            BaseTestSite.Assert.AreEqual(
                (uint)0,
                status,
                "Read Status should be {0}, actual is {1}", GetStatus(0), GetStatus(status));

            Guid snapshotId = Guid.NewGuid();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client creates a snapshot.");
            CreateSnapshot(snapshotId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	Client closes the file.");
            client.CloseSharedVirtualDisk();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5.	Client reopens the shared virtual disk file and expects success.");

            RequestIdentifier = 0;
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6.	Client sends Write request to change the file and expects success.");
            byte[] payload = Smb2Utility.CreateRandomByteArray(512);
            status = client.Write(0, payload);
            BaseTestSite.Assert.AreEqual(
                (uint)0,
                status,
                "Write Status should be {0}, actual is {1}", GetStatus(0), GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7.	Client sends Apply Snapshot request to apply the previous snapshot.");

            SVHDX_APPLY_SNAPSHOT_PARAMS applySnapshot = new SVHDX_APPLY_SNAPSHOT_PARAMS();
            applySnapshot.SnapshotID = snapshotId;
            applySnapshot.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            SVHDX_META_OPERATION_START_REQUEST startRequest = new SVHDX_META_OPERATION_START_REQUEST();
            startRequest.TransactionId = Guid.NewGuid();
            startRequest.OperationType = Operation_Type.SvhdxMetaOperationTypeApplySnapshot;
            payload = client.CreateTunnelMetaOperationStartApplySnapshotRequest(startRequest, applySnapshot);
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_OPERATION_HEADER? response;
            status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                            true,//true for Async operation, false for non-async operation
                            RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START,
                            ++RequestIdentifier,
                            payload,
                            out header,
                            out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "8.	Client rereads 512 bytes and compares it with the previously saved bytes.");
            byte[] byteAfterApplied = null;
            status = client.Read(0, 512, out byteAfterApplied);
            BaseTestSite.Assert.AreEqual(
                (uint)0,
                status,
                "Read Status should be {0}, actual is {1}", GetStatus(0), GetStatus(status));

            bool equal = ArrayUtility.CompareArrays(byteBeforeChanged, byteAfterApplied);
            BaseTestSite.Assert.AreEqual(
                true,
                equal,
                "The bytes after snapshot applied should be the same with the original.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "9.	Client deletes the snapshot.");
            DeleteSnapshot(snapshotId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "10. Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion2)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if the server supports opening a shared VHD set file with using a Target Specifier.")]
        public void BVT_OpenSharedVHDSetByTargetSpecifier()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2);
            Guid snapshotId = Guid.NewGuid();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends the first tunnel operation SVHDX_META_OPERATION_START_REQUEST to create a snapshot.");
            CreateSnapshot(snapshotId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");
            client.CloseSharedVirtualDisk();

            Smb2CreateEaBuffer createEaBuffer = new Smb2CreateEaBuffer();
            FsccFileFullEaInformation eaInfo = new FsccFileFullEaInformation();
            eaInfo.EaName = RsvdConst.RSVD_TARGET_SPECIFIER_EA;
            eaInfo.EaValue = client.CreateTargetSpecifier(snapshotId);
            eaInfo.Flags = FileFullEaInformation_Flags_Values.FILE_NEED_EA;
            createEaBuffer.FileFullEaInformations = new List<FsccFileFullEaInformation>();

            createEaBuffer.FileFullEaInformations.Add(eaInfo);

            Smb2CreateContextRequest[] contexts = new Smb2CreateContextRequest[]{
                    createEaBuffer};

            CREATE_Response response;
            Smb2CreateContextResponse[] serverCreateContexts;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	Client reopens the shared virtual disk file using a Target Specifier.");

            client.Connect(
                TestConfig.FileServerNameContainingSharedVHD,
                TestConfig.FileServerIPContainingSharedVHD,
                TestConfig.DomainName,
                TestConfig.UserName,
                TestConfig.UserPassword,
                TestConfig.DefaultSecurityPackage,
                TestConfig.UseServerGssToken,
                TestConfig.ShareContainingSharedVHD);

            RequestIdentifier = 0;
            uint status = client.OpenSharedVirtualDisk(
                TestConfig.NameOfSharedVHDS + fileNameSuffix,
                FsCreateOption.FILE_NO_INTERMEDIATE_BUFFERING,
                contexts,
                out serverCreateContexts,
                out response);

            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Open shared virtual disk file should succeed, actual status: {0}",
                GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

    }
}
