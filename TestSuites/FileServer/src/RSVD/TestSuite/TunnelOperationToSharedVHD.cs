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
    public class TunnelOperationToSharedVHD : RSVDTestBase
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
        [Description("Check if server supports handling tunnel operation RSVD_TUNNEL_GET_FILE_INFO_OPERATION.")]
        public void BVT_TunnelGetFileInfoToSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends tunnel operation RSVD_TUNNEL_GET_FILE_INFO_OPERATION to server and expects success.");
            byte[] payload = client.CreateTunnelFileInfoRequest();
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_FILE_INFO_RESPONSE? response;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_FILE_INFO_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_GET_FILE_INFO_OPERATION,
                ++RequestIdentifier,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_GET_FILE_INFO_OPERATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);

            VerifyFieldInResponse("ServerVersion", TestConfig.ServerServiceVersion, response.Value.ServerVersion);
            VerifyFieldInResponse("SectorSize", TestConfig.SectorSize, response.Value.SectorSize);
            VerifyFieldInResponse("PhysicalSectorSize", TestConfig.PhysicalSectorSize, response.Value.PhysicalSectorSize);
            VerifyFieldInResponse("VirtualSize", TestConfig.VirtualSize, response.Value.VirtualSize);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");

            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports handling tunnel operation RSVD_TUNNEL_SCSI_OPERATION.")]
        public void BVT_TunnelSCSIToSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");

            // Simulate an SCSI command: READ CAPACITY
            // The READ CAPACITY command provides a means for the application client to request information regarding the capacity of the block device.
            // Details see [SBC-2]
            byte[] cdbBuffer = new byte[RsvdConst.RSVD_CDB_GENERIC_LENGTH];
            cdbBuffer[0] = 0x25; // Operation Code of READ CAPACITY is 25h
            byte[] dataBuffer = new byte[8];
            byte[] payload = client.CreateTunnelScsiRequest(
                RsvdConst.SVHDX_TUNNEL_SCSI_REQUEST_LENGTH,
                (byte)cdbBuffer.Length, 
                (byte)RsvdConst.RSVD_SCSI_SENSE_BUFFER_SIZE, 
                true,
                SRB_FLAGS.SRB_FLAGS_QUEUE_ACTION_ENABLE |
                SRB_FLAGS.SRB_FLAGS_DISABLE_SYNCH_TRANSFER |
                SRB_FLAGS.SRB_FLAGS_DATA_IN |
                SRB_FLAGS.SRB_FLAGS_NO_QUEUE_FREEZE |
                SRB_FLAGS.SRB_FLAGS_PORT_DRIVER_ALLOCSENSE, 
                (byte)dataBuffer.Length, 
                cdbBuffer,
                dataBuffer);

            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_SCSI_RESPONSE? response;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_SCSI_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_SCSI_OPERATION,
                ++RequestIdentifier,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_SCSI_OPERATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);
            VerifyFieldInResponse("SCSIStatus", 0, response.Value.ScsiStatus); // Status code 0 indicates that the device has completed the task successfully.
            VerifyFieldInResponse("SrbStatus", SRB_STATUS.SRB_STATUS_SUCCESS, response.Value.SrbStatus);
            VerifyFieldInResponse("DataIn", true, response.Value.DataIn); // the CDB buffer specified is to receive data from the server.
            VerifyFieldInResponse("Length", RsvdConst.SVHDX_TUNNEL_SCSI_REQUEST_LENGTH, response.Value.Length);// the size of the SVHDX_TUNNEL_SCSI_REQUEST structure excluding the DataBuffer field
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");

            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports handling tunnel operation RSVD_TUNNEL_CHECK_CONNECTION_STATUS_OPERATION.")]
        public void BVT_TunnelCheckConnectionStatusToSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends tunnel operation RSVD_TUNNEL_CHECK_CONNECTION_STATUS_OPERATION to server and expects success.");
            byte[] payload = client.CreateTunnelCheckConnectionStatusRequest();
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_OPERATION_HEADER? response;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_CHECK_CONNECTION_STATUS_OPERATION,
                ++RequestIdentifier,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_CHECK_CONNECTION_STATUS_OPERATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");

            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports handling tunnel operation RSVD_TUNNEL_SRB_STATUS_OPERATION.")]
        public void BVT_TunnelSRBStatusToSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success with no initiator id specified.");
            // Open the shared virtual disk file with no initiator id. Thus read request will fail with status key returned.
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, hasInitiatorId:false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends a Read request and expects error since initiator id is not specified when opening.");

            byte[] readContent;
            uint status = client.Read(0, 512, out readContent);
            BaseTestSite.Assert.AreNotEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Read file with an invalid offset should not succeed, actual status: {0}",
                status);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client extracts StatusKey from error code, construct SVHDX_SHARED_VIRTUAL_DISK_SUPPORT_REQUEST, sends to server and expects success.");
            byte statusKey = (byte)(status & (uint)(~RsvdStatus.STATUS_SVHDX_ERROR_STORED));
            byte[] payload = client.CreateTunnelSrbStatusRequest(statusKey);
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_SRB_STATUS_RESPONSE? response;
            status = client.TunnelOperation<SVHDX_TUNNEL_SRB_STATUS_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_SRB_STATUS_OPERATION,
                ++RequestIdentifier,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_SRB_STATUS_OPERATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);
            VerifyFieldInResponse("StatusKey", statusKey, response.Value.StatusKey); // The server MUST set this field to the status key value received in the request.

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	Client closes the file.");

            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports handling tunnel operation RSVD_TUNNEL_GET_DISK_INFO_OPERATION.")]
        public void BVT_TunnelGetDiskInfoToSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends tunnel operation RSVD_TUNNEL_GET_DISK_INFO_OPERATION to server and expects success.");
            byte[] payload = client.CreateTunnelDiskInfoRequest();
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_DISK_INFO_RESPONSE? response;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_DISK_INFO_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_GET_DISK_INFO_OPERATION,
                ++RequestIdentifier,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_GET_DISK_INFO_OPERATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);

            VerifyFieldInResponse("DiskFormat", DISK_FORMAT.VIRTUAL_STORAGE_TYPE_DEVICE_VHDX, response.Value.DiskFormat);
            VerifyFieldInResponse("DiskType", TestConfig.DiskType, response.Value.DiskType);
            VerifyFieldInResponse("IsMounted", TestConfig.IsMounted, response.Value.IsMounted);
            VerifyFieldInResponse("Is4kAligned", TestConfig.Is4kAligned, response.Value.Is4kAligned);
            VerifyFieldInResponse("FileSize", TestConfig.FileSizeInMB, response.Value.FileSize);
            VerifyFieldInResponse("Reserved", 0, response.Value.Reserved);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");

            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports handling tunnel operation RSVD_TUNNEL_VALIDATE_DISK_OPERATION.")]
        public void BVT_TunnelValidateDiskToSharedVHD()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends tunnel operation RSVD_TUNNEL_VALIDATE_DISK_OPERATION to server and expects success.");
            byte[] payload = client.CreateTunnelValidateDiskRequest();
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_VALIDATE_DISK_RESPONSE? response;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_VALIDATE_DISK_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VALIDATE_DISK_OPERATION,
                ++RequestIdentifier,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VALIDATE_DISK_OPERATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);
            VerifyFieldInResponse("IsValidDisk", true, response.Value.IsValidDisk);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");

            client.CloseSharedVirtualDisk();
        }
    }
}