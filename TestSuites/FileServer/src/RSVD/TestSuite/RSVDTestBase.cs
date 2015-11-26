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
        /// Suffix of the .vhdx file name
        /// </summary>
        protected const string fileNameSuffix = ":SharedVirtualDisk";

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
        /// <param name="requestId">OpenRequestId, same with other operations' request id</param>
        /// <param name="hasInitiatorId">If the SVHDX_OPEN_DEVICE_CONTEXT contains InitiatorId</param>
        /// <param name="rsvdClient">The instance of rsvd client. NULL stands for the default client</param>
        public void OpenSharedVHD(string fileName, ulong? requestId = null, bool hasInitiatorId = true, RsvdClient rsvdClient = null)
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
            if (TestConfig.ServerServiceVersion == (uint)0x00000001)
            {
                contexts = new Smb2CreateContextRequest[]
                {
                    new Smb2CreateSvhdxOpenDeviceContext 
                    {
                        Version = TestConfig.ServerServiceVersion,
                        OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER, 
                        InitiatorHostName = TestConfig.InitiatorHostName,
                        InitiatorHostNameLength = (ushort)(TestConfig.InitiatorHostName.Length * 2),
                        OpenRequestId = requestId == null ? ((ulong)new System.Random().Next()) : requestId.Value,
                        InitiatorId = hasInitiatorId ? Guid.NewGuid():Guid.Empty,
                        HasInitiatorId = hasInitiatorId
                    }
                };
            }
            else if(TestConfig.ServerServiceVersion == (uint)0x00000002)
            {
                contexts = new Smb2CreateContextRequest[]
                {
                    new Smb2CreateSvhdxOpenDeviceContextV2
                    {
                        Version = TestConfig.ServerServiceVersion,
                        OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER, 
                        InitiatorHostName = TestConfig.InitiatorHostName,
                        InitiatorHostNameLength = (ushort)(TestConfig.InitiatorHostName.Length * 2),
                        OpenRequestId = requestId == null ? ((ulong)new System.Random().Next()) : requestId.Value,
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
            Smb2CreateContextResponse[] serverContextResponse;
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

        /// <summary>
        /// Create snapshot for the VHD set file
        /// </summary>
        /// <param name="requestId">Tunnel operation request id</param>
        /// <param name="rsvdClient">The instance of rsvd client. NULL stands for the default client</param>
        /// <returns>Return a snapshot id</returns>
        public Guid CreateSnapshot(ref ulong requestId, RsvdClient rsvdClient = null)
        {
            if (rsvdClient == null)
            {
                rsvdClient = this.client;
            }

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
            byte[] payload = rsvdClient.CreateTunnelMetaOperationStartCreateSnapshotRequest(
                startRequest,
                createsnapshot);

            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_OPERATION_HEADER? response;
            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
            uint status = rsvdClient.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                true,
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START,
                requestId++,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            createsnapshot.Flags = Snapshot_Flags.SVHDX_SNAPSHOT_FLAG_ZERO;
            createsnapshot.Stage1 = Stage_Values.SvhdxSnapshotStageBlockIO;
            createsnapshot.Stage2 = Stage_Values.SvhdxSnapshotStageSwitchObjectStore;
            createsnapshot.Stage3 = Stage_Values.SvhdxSnapshotStageUnblockIO;
            createsnapshot.Stage4 = Stage_Values.SvhdxSnapshotStageFinalize;
            createsnapshot.Stage5 = Stage_Values.SvhdxSnapshotStageInvalid;
            createsnapshot.Stage6 = Stage_Values.SvhdxSnapshotStageInvalid;
            payload = rsvdClient.CreateTunnelMetaOperationStartCreateSnapshotRequest(
                startRequest,
                createsnapshot);

            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
            status = rsvdClient.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                true,
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START,
                requestId++,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            SetFile_InformationType setFileInforType = SetFile_InformationType.SvhdxSetFileInformationTypeSnapshotEntry;
            Snapshot_Type snapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_SNAPSHOT_ENTRY_RESPONSE? snapshotEntryResponse;
            payload = client.CreateTunnelGetVHDSetFileInfoRequest(
                setFileInforType,
                snapshotType,
                createsnapshot.SnapshotId);
            status = client.TunnelOperation<SVHDX_TUNNEL_VHDSET_FILE_QUERY_INFORMATION_SNAPSHOT_ENTRY_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_VHDSET_QUERY_INFORMATION,
                requestId++,
                payload,
                out header,
                out snapshotEntryResponse);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));

            return createsnapshot.SnapshotId;
        }

        /// <summary>
        /// Delete the existing snapshots for the VHD set file
        /// </summary>
        /// <param name="requestId">Tunnel operation request id</param>
        /// <param name="snapshotId">The snapshot id to be deleted</param>
        /// <param name="rsvdClient">The instance of rsvd client. NULL stands for the default client</param>
        public void DeleteSnapshot(ref ulong requestId, Guid snapshotId, RsvdClient rsvdClient = null)
        {
            if (rsvdClient == null)
            {
                rsvdClient = this.client;
            }

            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST deleteRequest = new SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST();

            deleteRequest.SnapshotId = snapshotId;
            deleteRequest.PersistReference = PersistReference_Flags.PersistReferenceFalse;
            deleteRequest.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            byte[] payload = rsvdClient.CreateTunnelMetaOperationDeleteSnapshotRequest(deleteRequest);
            SVHDX_TUNNEL_OPERATION_HEADER? deleteResponse;
            uint status = rsvdClient.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                false,
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
        }

        public SVHDX_TUNNEL_DISK_INFO_RESPONSE? GetVirtualDiskInfo(ref ulong requestId, RsvdClient rsvdClient = null)
        {
            if (rsvdClient == null)
            {
                rsvdClient = this.client;
            }

            byte[] payload = client.CreateTunnelDiskInfoRequest();
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_DISK_INFO_RESPONSE? response;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_DISK_INFO_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_GET_DISK_INFO_OPERATION,
                requestId,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_GET_DISK_INFO_OPERATION, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, requestId++);

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
