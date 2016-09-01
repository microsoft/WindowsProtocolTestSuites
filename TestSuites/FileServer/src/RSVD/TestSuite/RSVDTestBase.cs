// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;

namespace Microsoft.Protocols.TestSuites.FileSharing.RSVD.TestSuite
{
    /// <summary>
    /// Test Base for all RSVD test cases
    /// </summary>
    public abstract class RSVDTestBase : CommonTestBase
    {
        #region Variables
        /// <summary>
        /// Default Rsvd client
        /// The instance is created when the test case is initialized
        /// </summary>
        protected RsvdClient client;

        /// <summary>
        /// Suffix of the shared virtual disk file name
        /// </summary>
        protected const string fileNameSuffix = ":SharedVirtualDisk";

        /// <summary>
        /// An unsigned 64-bit value assigned by the client for an outgoing request. 
        /// </summary>
        protected ulong RequestIdentifier;

        #endregion

        public RSVDTestConfig TestConfig
        {
            get
            {
                return testConfig as RSVDTestConfig;
            }
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            testConfig = new RSVDTestConfig(BaseTestSite);

            BaseTestSite.DefaultProtocolDocShortName = "MS-RSVD";

            client = new RsvdClient(TestConfig.Timeout);
            RequestIdentifier = 0; // Follow windows client behaviour. Initialize it to zero.

            // Copy the data used in test cases to the share of the SUT, e.g. the shared virtual disk files.
            sutProtocolController.CopyFile(TestConfig.FullPathShareContainingSharedVHD, @"data\*.*");
        }

        protected override void TestCleanup()
        {
            try
            {
                client.Disconnect();
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect client: {0}", ex.ToString());
            }
            client.Dispose();

            base.TestCleanup();
        }

        /// <summary>
        /// Open the shared virtual disk file.
        /// </summary>
        /// <param name="fileName">The virtual disk file name to be used</param>
        /// <param name="version">Version of the open device context</param>
        /// <param name="openRequestId">OpenRequestId, same with other operations' request id</param>
        /// <param name="hasInitiatorId">If the SVHDX_OPEN_DEVICE_CONTEXT contains InitiatorId</param>
        /// <param name="rsvdClient">The instance of rsvd client. NULL stands for the default client</param>
        /// <param name="initiatorHostName">It specifies the computer name on which the initiator resides</param>
        public void OpenSharedVHD(
            string fileName,
            RSVD_PROTOCOL_VERSION version, 
            ulong? openRequestId = null, 
            bool hasInitiatorId = true, 
            RsvdClient rsvdClient = null,
            string initiatorHostName = null)
        {
            Smb2CreateContextResponse[] serverContextResponse;
            OpenSharedVHD(fileName, version, openRequestId, hasInitiatorId, rsvdClient, out serverContextResponse, initiatorHostName);
        }

        /// <summary>
        /// Open the shared virtual disk file.
        /// </summary>
        /// <param name="fileName">The virtual disk file name to be used</param>
        /// <param name="version">Version of the open device context</param>
        /// <param name="openRequestId">OpenRequestId, same with other operations' request id</param>
        /// <param name="hasInitiatorId">If the SVHDX_OPEN_DEVICE_CONTEXT contains InitiatorId</param>
        /// <param name="rsvdClient">The instance of rsvd client. NULL stands for the default client</param>
        /// <param name="serverContextResponse">The create context response returned by server</param>
        /// <param name="initiatorHostName">It specifies the computer name on which the initiator resides</param>
        public void OpenSharedVHD(
            string fileName,
            RSVD_PROTOCOL_VERSION version, 
            ulong? openRequestId, 
            bool hasInitiatorId, 
            RsvdClient rsvdClient,
            out Smb2CreateContextResponse[] serverContextResponse,
            string initiatorHostName)
        {
            if (rsvdClient == null)
                rsvdClient = this.client;

            rsvdClient.Connect(
                TestConfig.FileServerNameContainingSharedVHD,
                TestConfig.FileServerIPContainingSharedVHD,
                TestConfig.DomainName,
                TestConfig.UserName,
                TestConfig.UserPassword,
                TestConfig.DefaultSecurityPackage,
                TestConfig.UseServerGssToken,
                TestConfig.ShareContainingSharedVHD);

            Smb2CreateContextRequest[] contexts = null;
            string tempInitiatorHostName = initiatorHostName ?? TestConfig.InitiatorHostName;
            if (version == RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1)
            {
                contexts = new Smb2CreateContextRequest[]
                {
                    new Smb2CreateSvhdxOpenDeviceContext 
                    {
                        Version = (uint)version,
                        OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER, 
                        InitiatorHostName = tempInitiatorHostName,
                        InitiatorHostNameLength = (ushort)(tempInitiatorHostName.Length * 2),
                        OpenRequestId = openRequestId == null ? RequestIdentifier : openRequestId.Value,
                        InitiatorId = hasInitiatorId ? Guid.NewGuid():Guid.Empty,
                        HasInitiatorId = hasInitiatorId
                    }
                };
            }
            else if (version == RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2)
            {
                contexts = new Smb2CreateContextRequest[]
                {
                    new Smb2CreateSvhdxOpenDeviceContextV2
                    {
                        Version = (uint)version,
                        OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER, 
                        InitiatorHostName = TestConfig.InitiatorHostName,
                        InitiatorHostNameLength = (ushort)(TestConfig.InitiatorHostName.Length * 2),
                        OpenRequestId = openRequestId == null ? RequestIdentifier : openRequestId.Value,
                        InitiatorId = hasInitiatorId ? Guid.NewGuid():Guid.Empty,
                        HasInitiatorId = hasInitiatorId,
                        VirtualDiskPropertiesInitialized = 0,
                        ServerServiceVersion = 0,
                        VirtualSize = 0,
                        PhysicalSectorSize = 0,
                        VirtualSectorSize = 0
                    }
                };
            }
            else
            {
                throw new ArgumentException("The ServerServiceVersion {0} is not supported.", "Version");
            }

            CREATE_Response response;
            uint status = rsvdClient.OpenSharedVirtualDisk(
                            fileName + fileNameSuffix,
                            FsCreateOption.FILE_NO_INTERMEDIATE_BUFFERING,
                            contexts,
                            out serverContextResponse,
                            out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Open shared virtual disk file should succeed, actual status: {0}",
                GetStatus(status));
        }


        protected void CreateSnapshot(Guid snapshotId)
        {
            SVHDX_META_OPERATION_START_REQUEST startRequest = new SVHDX_META_OPERATION_START_REQUEST();
            startRequest.TransactionId = Guid.NewGuid();
            startRequest.OperationType = Operation_Type.SvhdxMetaOperationTypeCreateSnapshot;
            SVHDX_META_OPERATION_CREATE_SNAPSHOT createsnapshot = new SVHDX_META_OPERATION_CREATE_SNAPSHOT();
            createsnapshot.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            createsnapshot.Flags = Snapshot_Flags.SVHDX_SNAPSHOT_DISK_FLAG_ENABLE_CHANGE_TRACKING;
            createsnapshot.Stage1 = Stage_Values.SvhdxSnapshotStageInitialize;
            createsnapshot.SnapshotId = snapshotId;
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

            // Add spaces in the beginning of the log to be align with the last test step. Since the steps in this function are only sub steps.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "	Client sends the second tunnel operation SVHDX_META_OPERATION_START_REQUEST to create a snapshot and expects success.");
            createsnapshot.Flags = Snapshot_Flags.SVHDX_SNAPSHOT_FLAG_ZERO;
            createsnapshot.Stage1 = Stage_Values.SvhdxSnapshotStageBlockIO;
            createsnapshot.Stage2 = Stage_Values.SvhdxSnapshotStageSwitchObjectStore;
            createsnapshot.Stage3 = Stage_Values.SvhdxSnapshotStageUnblockIO;
            createsnapshot.Stage4 = Stage_Values.SvhdxSnapshotStageFinalize;
            payload = client.CreateTunnelMetaOperationStartCreateSnapshotRequest(
                startRequest,
                createsnapshot);
            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
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

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "   Verify the snapshot has been created after the operation.");

            DoUntilSucceed(
                () => CheckSnapshotExisted(snapshotId),
                TestConfig.Timeout,
                "Retry getting the snapshot to make sure it has been created until succeed within timeout span");
        }

        protected void DeleteSnapshot(Guid snapshotId)
        {
            SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST deleteRequest = new SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST();
            deleteRequest.SnapshotId = snapshotId;
            deleteRequest.PersistReference = PersistReference_Flags.PersistReferenceFalse;
            deleteRequest.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            byte[] payload = client.CreateTunnelMetaOperationDeleteSnapshotRequest(
                deleteRequest);
            SVHDX_TUNNEL_OPERATION_HEADER? deleteResponse;
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_DELETE_SNAPSHOT,
                ++RequestIdentifier,
                payload,
                out header,
                out deleteResponse);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_DELETE_SNAPSHOT, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);
        }

        private void CheckSnapshotExisted(Guid snapshotId)
        {
            VHDSet_InformationType setFileInforType = VHDSet_InformationType.SvhdxVHDSetInformationTypeSnapshotEntry;
            Snapshot_Type snapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_SNAPSHOT_ENTRY_RESPONSE? snapshotEntryResponse;
            byte[] payload = client.CreateTunnelGetVHDSetFileInfoRequest(
                setFileInforType,
                snapshotType,
                snapshotId);
            uint status = client.TunnelOperation<SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_SNAPSHOT_ENTRY_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VHDSET_QUERY_INFORMATION,
                ++RequestIdentifier,
                payload,
                out header,
                out snapshotEntryResponse);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VHDSET_QUERY_INFORMATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);
        }

        public SVHDX_TUNNEL_DISK_INFO_RESPONSE? GetVirtualDiskInfo()
        {
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

            return response;
        }

        /// <summary>
        /// Verify the Tunnel Operation Header
        /// </summary>
        /// <param name="header">The received Tunnel Operation Header</param>
        /// <param name="code">The operation code</param>
        /// <param name="status">The received status</param>
        /// <param name="requestId">The request ID</param>
        public void VerifyTunnelOperationHeader(SVHDX_TUNNEL_OPERATION_HEADER header, RSVD_TUNNEL_OPERATION_CODE code, uint status, ulong requestId)
        {
            BaseTestSite.Assert.AreEqual(
                code,
                header.OperationCode,
                "Operation code should be {0}, actual is {1}", code, header.OperationCode);

            BaseTestSite.Assert.AreEqual(
                status,
                header.Status,
                "Status should be {0}, actual is {1}", GetStatus(status), GetStatus(header.Status));

            BaseTestSite.Assert.AreEqual(
                requestId,
                header.RequestId,
                "The RequestId should be {0}, actual is {1}", requestId, header.RequestId);
        }

        /// <summary>
        /// Verify the specific field of response
        /// </summary>
        /// <typeparam name="T">The response type</typeparam>
        /// <param name="fieldName">The field name in response</param>
        /// <param name="expected">The expected value of the specific field</param>
        /// <param name="actual">The actual value of the specific field</param>
        public void VerifyFieldInResponse<T>(string fieldName, T expected, T actual)
        {
            BaseTestSite.Assert.AreEqual(
                expected,
                actual,
                fieldName + " should be {0}, actual is {1}", expected, actual);
        }

        /// <summary>
        /// Convert the status from uint to string
        /// </summary>
        /// <param name="status">The status in uint</param>
        /// <returns>The status in string format</returns>
        public string GetStatus(uint status)
        {
            if (Enum.IsDefined(typeof(RsvdStatus), status))
            {
                return ((RsvdStatus)status).ToString();
            }

            return Smb2Status.GetStatusCode(status);
        }
    }
}
