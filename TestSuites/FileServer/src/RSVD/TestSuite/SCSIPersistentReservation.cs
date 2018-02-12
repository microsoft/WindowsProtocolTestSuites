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
    /// <summary>
    /// This scenario is used to test SCSI Persistent Reservation which is using RSVD as transport.
    /// </summary>
    [TestClass]
    public class SCSIPersistentReservation : RSVDTestBase
    {
        private RsvdClient secondClient;
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

        protected override void TestInitialize()
        {
            base.TestInitialize();

            secondClient = new RsvdClient(TestConfig.Timeout);
        }

        protected override void TestCleanup()
        {
            try
            {
                secondClient.Disconnect();
            }
            catch (System.Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect second client: {0}", ex.ToString());
            }

            secondClient.Dispose();

            base.TestCleanup();
        }
        #endregion


        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports SCSI persistent reservation service actions: Register and Reserve, by tunnel operation RSVD_TUNNEL_SCSI_OPERATION.")]
        public void BVT_TunnelSCSIPersistentReserve_RegisterAndReserve()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "2.	Client sends Register service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");

            System.Random random = new System.Random();
            byte[] key = new byte[8];
            random.NextBytes(key);

            byte scsiStatus;
            SendAndReceiveSCSICommand(
                this.client,
                10, // The size of SCSI Persistent Reserve Out command is 10
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.REGISTER_AND_IGNORE_EXISTING_KEY, 0),
                CreateDataBuffer_PersistentReserveOut(0, System.BitConverter.ToUInt64(key, 0)),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3.	Client sends Reserve service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                this.client,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.RESERVE, PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE.WriteExclusiveRegistrantsOnly),
                CreateDataBuffer_PersistentReserveOut(System.BitConverter.ToUInt64(key, 0), 0),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server can handle SCSI Persistent Reservation Conflict by tunnel operation RSVD_TUNNEL_SCSI_OPERATION.")]
        public void BVT_TunnelSCSIPersistentReserve_ReserveConflict()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	The first client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	The second client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, 0, true, secondClient, "client02");

            System.Random random = new System.Random();
            byte[] firstReservationkey = new byte[8];
            random.NextBytes(firstReservationkey);

            byte[] secondReservationKey = new byte[8];
            random.NextBytes(secondReservationKey);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3.	The first client sends Register service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            byte scsiStatus;
            SendAndReceiveSCSICommand(
                this.client,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.REGISTER_AND_IGNORE_EXISTING_KEY, 0),
                CreateDataBuffer_PersistentReserveOut(0, System.BitConverter.ToUInt64(firstReservationkey, 0)),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "4.	The first client sends Reserve service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                this.client,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.RESERVE, PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE.WriteExclusiveRegistrantsOnly),
                CreateDataBuffer_PersistentReserveOut(System.BitConverter.ToUInt64(firstReservationkey, 0), 0),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "5.	The second client sends Register service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                secondClient,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.REGISTER_AND_IGNORE_EXISTING_KEY, 0),
                CreateDataBuffer_PersistentReserveOut(0, System.BitConverter.ToUInt64(secondReservationKey, 0)),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "6.	The second client sends Reserve service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and " +
                "expects server returns Reservation Conflict.");
            SendAndReceiveSCSICommand(
                secondClient,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.RESERVE, PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE.WriteExclusiveRegistrantsOnly),
                CreateDataBuffer_PersistentReserveOut(System.BitConverter.ToUInt64(secondReservationKey, 0), 0),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 24, scsiStatus);  // Status code 24 indicates Reservation Conflict

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7.	The first client closes the file.");
            client.CloseSharedVirtualDisk();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "8.	The second client closes the file.");
            secondClient.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports SCSI persistent reservation service actions: Reserve and Release, by tunnel operation RSVD_TUNNEL_SCSI_OPERATION.")]
        public void BVT_TunnelSCSIPersistentReserve_ReserveAndRelease()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	The first client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	The second client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, 0, true, secondClient, "client02");

            System.Random random = new System.Random();
            byte[] firstReservationkey = new byte[8];
            random.NextBytes(firstReservationkey);

            byte[] secondReservationKey = new byte[8];
            random.NextBytes(secondReservationKey);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3.	The first client sends Register service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            byte scsiStatus;
            SendAndReceiveSCSICommand(
                this.client,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.REGISTER_AND_IGNORE_EXISTING_KEY, 0),
                CreateDataBuffer_PersistentReserveOut(0, System.BitConverter.ToUInt64(firstReservationkey, 0)),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "4.	The first client sends Reserve service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                this.client,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.RESERVE, PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE.WriteExclusiveRegistrantsOnly),
                CreateDataBuffer_PersistentReserveOut(System.BitConverter.ToUInt64(firstReservationkey, 0), 0),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "5.	The first client sends Release service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                this.client,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.RELEASE, PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE.WriteExclusiveRegistrantsOnly),
                CreateDataBuffer_PersistentReserveOut(System.BitConverter.ToUInt64(firstReservationkey, 0), 0),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "6.	The second client sends Register service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                secondClient,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.REGISTER_AND_IGNORE_EXISTING_KEY, 0),
                CreateDataBuffer_PersistentReserveOut(0, System.BitConverter.ToUInt64(secondReservationKey, 0)),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "7.	The second client sends Reserve service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                secondClient,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.RESERVE, PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE.WriteExclusiveRegistrantsOnly),
                CreateDataBuffer_PersistentReserveOut(System.BitConverter.ToUInt64(secondReservationKey, 0), 0),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "8.	The first client closes the file.");
            client.CloseSharedVirtualDisk();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "9.	The second client closes the file.");
            secondClient.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports SCSI persistent reservation service action: Preempt, by tunnel operation RSVD_TUNNEL_SCSI_OPERATION.")]
        public void BVT_TunnelSCSIPersistentReserve_Preempt()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	The first client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	The second client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, 0, true, secondClient, "client02");

            System.Random random = new System.Random();
            byte[] firstReservationkey = new byte[8];
            random.NextBytes(firstReservationkey);

            byte[] secondReservationKey = new byte[8];
            random.NextBytes(secondReservationKey);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3.	The first client sends Register service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            byte scsiStatus;
            SendAndReceiveSCSICommand(
                this.client,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.REGISTER_AND_IGNORE_EXISTING_KEY, 0),
                CreateDataBuffer_PersistentReserveOut(0, System.BitConverter.ToUInt64(firstReservationkey, 0)),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "4.	The first client sends Reserve service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                this.client,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.RESERVE, PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE.WriteExclusiveRegistrantsOnly),
                CreateDataBuffer_PersistentReserveOut(System.BitConverter.ToUInt64(firstReservationkey, 0), 0),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "5.	The second client sends Register service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                secondClient,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.REGISTER_AND_IGNORE_EXISTING_KEY, 0),
                CreateDataBuffer_PersistentReserveOut(0, System.BitConverter.ToUInt64(secondReservationKey, 0)),
                out scsiStatus);
            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "6.	The second client sends Preempt service action of SCSI Persistent Reserve Out command by tunnel operation RSVD_TUNNEL_SCSI_OPERATION to server and expects success.");
            SendAndReceiveSCSICommand(
                secondClient,
                10,
                CreateCDBBuffer_PersistentReserveOut(PERSISTENT_RESERVE_OUT_SERVICE_ACTION.PREEMPT, PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE.WriteExclusiveRegistrantsOnly),
                CreateDataBuffer_PersistentReserveOut(System.BitConverter.ToUInt64(secondReservationKey, 0), System.BitConverter.ToUInt64(firstReservationkey, 0)),
                out scsiStatus);

            VerifyFieldInResponse("SCSIStatus", 0, scsiStatus); // Status code 0 indicates that the device has completed the task successfully.

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7.	The first client closes the file.");
            client.CloseSharedVirtualDisk();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "8.	The second client closes the file.");
            secondClient.CloseSharedVirtualDisk();
        }


        private void SendAndReceiveSCSICommand(RsvdClient rsvdClient, byte cdbLength, byte[] cdbBuffer, byte[] dataBuffer, out byte scsiStatus)
        {
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_SCSI_RESPONSE? response;

            byte[] payload = rsvdClient.CreateTunnelScsiRequest(
                RsvdConst.SVHDX_TUNNEL_SCSI_REQUEST_LENGTH,
                cdbLength,
                (byte)RsvdConst.RSVD_SCSI_SENSE_BUFFER_SIZE,
                false,
                SRB_FLAGS.SRB_FLAGS_DATA_OUT,
                (byte)dataBuffer.Length,
                cdbBuffer,
                dataBuffer);

            uint status = rsvdClient.TunnelOperation<SVHDX_TUNNEL_SCSI_RESPONSE>(
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
            VerifyFieldInResponse("DataIn", false, response.Value.DataIn); // the CDB buffer specified is to receive data from the server.
            VerifyFieldInResponse("Length", 36, response.Value.Length);// the size of the SVHDX_TUNNEL_SCSI_REQUEST structure excluding the DataBuffer field; This field MUST be set to 36.

            scsiStatus = response.Value.ScsiStatus;
        }

        private byte[] CreateDataBuffer_PersistentReserveOut(ulong reservationKey, ulong ServiceActionReservationKey)
        {
            PERSISTENT_RESERVE_OUT_PARAMETER_LIST persistentReserveOutParameterList = new PERSISTENT_RESERVE_OUT_PARAMETER_LIST();
            persistentReserveOutParameterList.ReservationKey = reservationKey;
            persistentReserveOutParameterList.ServiceActionReservationKey = ServiceActionReservationKey;
            return TypeMarshal.ToBytes<PERSISTENT_RESERVE_OUT_PARAMETER_LIST>(persistentReserveOutParameterList);
        }

        private byte[] CreateCDBBuffer_PersistentReserveOut(
            PERSISTENT_RESERVE_OUT_SERVICE_ACTION ServiceAction,
            PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE ScopeAndType)
        {
            // cdbBuffer has a fixed length.
            // There could be several zeros in the end of the buffer because the real CDB info is not so long.
            PERSISTENT_RESERVE_OUT persistentReserveOut = new PERSISTENT_RESERVE_OUT();
            persistentReserveOut.OperationCode = OPERATION_CODE.PERSISTENT_RESERVE_OUT;
            persistentReserveOut.ServiceAction = ServiceAction;
            persistentReserveOut.ParameterListLength = 24; // The parameter list shall be 24 bytes in length
            persistentReserveOut.ScopeAndType = ScopeAndType;

            byte[] cdb = TypeMarshal.ToBytes<PERSISTENT_RESERVE_OUT>(persistentReserveOut);
            byte[] cdbBuffer = new byte[RsvdConst.RSVD_CDB_GENERIC_LENGTH];
            cdb.CopyTo(cdbBuffer, 0);
            return cdbBuffer;
        }
    }
}