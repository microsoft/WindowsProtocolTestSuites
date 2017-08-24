// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class SMB2TestBase : CommonTestBase
    {
        public SMB2TestConfig TestConfig
        {
            get
            {
                return testConfig as SMB2TestConfig;
            }
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            testConfig = new SMB2TestConfig(BaseTestSite);

            BaseTestSite.DefaultProtocolDocShortName = "MS-SMB2";

            BaseTestSite.Log.Add(LogEntryKind.Debug, "SecurityPackage for authentication: " + TestConfig.DefaultSecurityPackage);
        }

        protected void ReadIpAddressesFromTestConfig(out List<IPAddress> clientIps, out List<IPAddress> serverIps)
        {
            clientIps = new List<IPAddress>();
            serverIps = new List<IPAddress>();
            clientIps.Add(TestConfig.ClientNic1IPAddress);
            clientIps.Add(TestConfig.ClientNic2IPAddress);
            serverIps.Add(TestConfig.SutIPAddress);
            serverIps.Add(TestConfig.SutAlternativeIPAddress);
        }

        protected void CheckCreateContextResponses(Smb2CreateContextResponse[] createContextResponse, params BaseResponseChecker[] checkers)
        {
            CheckCreateContextResponsesExistence(createContextResponse, true, checkers);
        }

        protected void CheckCreateContextResponsesNotExist(Smb2CreateContextResponse[] createContextResponse, params BaseResponseChecker[] checkers)
        {
            CheckCreateContextResponsesExistence(createContextResponse, false, checkers);
        }

        private void CheckCreateContextResponsesExistence(Smb2CreateContextResponse[] createContextResponse, bool shouldExist, params BaseResponseChecker[] checkers)
        {
            foreach (var checker in checkers)
            {
                bool found = false;
                if (createContextResponse != null)
                {
                    foreach (var response in createContextResponse)
                    {
                        if (response.GetType() == checker.ResponseType)
                        {
                            checker.Check(response);
                            found = true;
                            break;
                        }
                    }
                }
                if (shouldExist && !found)
                {
                    BaseTestSite.Assert.Fail("The expected response {0} does not exist.", checker.ResponseType);
                }

                if (!shouldExist && found)
                {
                    BaseTestSite.Assert.Fail("The response {0} should not exist.", checker.ResponseType);
                }
            }
        }

        #region HRVS

        /// <summary>
        /// Suffix of the .vhdx file name
        /// </summary>
        protected const string fileNameSuffix = ":SharedVirtualDisk";
        protected Smb2FunctionalClient smb2Functionalclient;
        protected RsvdClient rsvdClient;

        protected override void TestCleanup()
        {
            if (smb2Functionalclient != null)
            {
                try
                {
                    smb2Functionalclient.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
            }

            if (rsvdClient != null)
            {
                try
                {
                    rsvdClient.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
                rsvdClient.Dispose();
            }
            base.TestCleanup();
        }

        public void CheckHvrsCapability(bool flag, string statement)
        {
            if(!flag)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, statement);
                Site.Assert.Inconclusive("Test case is not applicable in this server.");
            }
        }

        protected void ConnectToVHD(out uint treeId, out FILEID fileId)
        {
            smb2Functionalclient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.ShareServerName, TestConfig.ShareServerIP);
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "Client creates an Open to a VHDX file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE");
            smb2Functionalclient.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled);
            smb2Functionalclient.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.ShareServerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            smb2Functionalclient.TreeConnect(Smb2Utility.GetUncPath(TestConfig.ShareServerName, TestConfig.ShareName), out treeId);

            Smb2CreateContextResponse[] response;
            smb2Functionalclient.Create(treeId, TestConfig.NameOfSqosVHD, CreateOptions_Values.NONE, out fileId, out response, RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);
        }

        /// <summary>
        /// Open the shared virtual disk file.
        /// </summary>
        /// <param name="fileName">The virtual disk file name to be used</param>
        /// <param name="requestId">OpenRequestId, same with other operations' request id</param>
        /// <param name="hasInitiatorId">If the SVHDX_OPEN_DEVICE_CONTEXT contains InitiatorId</param>
        /// <param name="client">The instance of rsvd client. NULL stands for the default client</param>
        protected void OpenSharedVHD(string fileName, ulong? requestId = null, bool hasInitiatorId = true, RsvdClient client = null)
        {
            if (client == null)
                client = this.rsvdClient;

            client.Connect(
                TestConfig.ShareServerName,
                TestConfig.ShareServerIP,
                TestConfig.DomainName,
                TestConfig.UserName,
                TestConfig.UserPassword,
                TestConfig.DefaultSecurityPackage,
                TestConfig.UseServerGssToken,
                TestConfig.ShareName);

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
            else if (TestConfig.ServerServiceVersion == (uint)0x00000002)
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
            uint status = client.OpenSharedVirtualDisk(
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
        /// <param name="client">The instance of rsvd client. NULL stands for the default client</param>
        /// <returns>Return a snapshot id</returns>
        protected Guid CreateSnapshot(ref ulong requestId, RsvdClient client = null)
        {
            if (client == null)
            {
                client = this.rsvdClient;
            }

            SVHDX_META_OPERATION_START_REQUEST startRequest = new SVHDX_META_OPERATION_START_REQUEST();
            startRequest.TransactionId = System.Guid.NewGuid();
            startRequest.OperationType = Operation_Type.SvhdxMetaOperationTypeCreateSnapshot;
            startRequest.Padding = 0;
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
            payload = client.CreateTunnelMetaOperationStartCreateSnapshotRequest(
                startRequest,
                createsnapshot);

            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
            status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
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

            VHDSet_InformationType setFileInforType = VHDSet_InformationType.SvhdxVHDSetInformationTypeSnapshotEntry;
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
        /// <param name="client">The instance of rsvd client. NULL stands for the default client</param>
        protected void DeleteSnapshot(ref ulong requestId, Guid snapshotId, RsvdClient client = null)
        {
            if (client == null)
            {
                client = this.rsvdClient;
            }

            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST deleteRequest = new SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST();

            deleteRequest.SnapshotId = snapshotId;
            deleteRequest.PersistReference = PersistReference_Flags.PersistReferenceFalse;
            deleteRequest.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            byte[] payload = client.CreateTunnelMetaOperationDeleteSnapshotRequest(deleteRequest);
            SVHDX_TUNNEL_OPERATION_HEADER? deleteResponse;
            uint status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
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

        protected SVHDX_TUNNEL_DISK_INFO_RESPONSE? GetVirtualDiskInfo(ref ulong requestId, RsvdClient client = null)
        {
            if (client == null)
            {
                client = this.rsvdClient;
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
        protected void VerifyTunnelOperationHeader(SVHDX_TUNNEL_OPERATION_HEADER header, RSVD_TUNNEL_OPERATION_CODE code, uint status, ulong requestId)
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
        protected void VerifyFieldInResponse<T>(string fieldName, T expected, T actual)
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
        protected string GetStatus(uint status)
        {
            if (Enum.IsDefined(typeof(RsvdStatus), status))
            {
                return ((RsvdStatus)status).ToString();
            }

            return Smb2Status.GetStatusCode(status);
        }
        #endregion
    }
}
