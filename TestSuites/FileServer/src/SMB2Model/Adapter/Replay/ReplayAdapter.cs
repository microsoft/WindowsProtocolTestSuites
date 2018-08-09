// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Net;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Replay
{
    public class ReplayAdapter : ModelManagedAdapterBase, IReplayAdapter
    {
        #region Fields
        private Smb2FunctionalClient smb2ClientMainChannel;
        private Smb2FunctionalClient smb2ClientAlternativeChannel;
        private ReplayServerConfig replayConfig;

        private uint treeIdMainChannel;
        private ulong sessionIdMainChannel;
        private byte[] sessionKeyMainChannel;
        private string fileNameMainChannel;
        private Guid createGuidMainChannel;
        private FILEID fileIdMainChannel;
        private FSCTL_SET_INTEGRIY_INFO_INPUT setIntegrityInfo;
        private FileEndOfFileInformation endOfFileInformation;
        private Guid leaseKeyMainChannel;
        private Guid clientGuidMainChannel;
        private IPAddress serverIpMainChannel;
        private string serverNameMainChannel;
        private string principleNameMainChannel;
        private string sharePathMainChannel;
        private DialectRevision dialectMainChannel;
        private Capabilities_Values clientCapabilitiesMainChannel;

        private string writeContent;
        private bool prepared;
        #endregion

        #region  Events
        public event CreateResponseEventHandler CreateResponse;

        public event FileOperationResponseEventHandler FileOperationResponse;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            Initialize();
        }

        public override void Reset()
        {
            if (smb2ClientMainChannel != null)
            {
                smb2ClientMainChannel.Disconnect();
                smb2ClientMainChannel = null;
            }

            if (smb2ClientAlternativeChannel != null)
            {
                smb2ClientAlternativeChannel.Disconnect();
                smb2ClientAlternativeChannel = null;
            }

            Initialize();

            base.Reset();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            smb2ClientMainChannel = null;
            smb2ClientAlternativeChannel = null;
            prepared = false;

            setIntegrityInfo.ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFO_INPUT_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64;
            setIntegrityInfo.Flags = FSCTL_SET_INTEGRITY_INFO_INPUT_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF;
            setIntegrityInfo.Reserved = FSCTL_SET_INTEGRITY_INFO_INPUT_RESERVED.V1;

            endOfFileInformation.EndOfFile = 2048;
            fileNameMainChannel = this.CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            leaseKeyMainChannel = Guid.NewGuid();
            clientGuidMainChannel = Guid.NewGuid();
            createGuidMainChannel = Guid.NewGuid();
            clientCapabilitiesMainChannel = Capabilities_Values.NONE;
        }

        private void InitializeMainChannel(
            ModelDialectRevision maxSmbVersionClientSupported,
            Guid clientGuid,
            ReplayModelShareType shareType,
            out uint treeId,
            bool isReconnect = false,
            bool isClientSupportPersistent = true)
        {
            Site.Assume.IsNull(smb2ClientMainChannel, "Expect smb2ClientMainChannel is NULL.");

            smb2ClientMainChannel = new Smb2FunctionalClient(testConfig.Timeout, testConfig, Site);
            smb2ClientMainChannel.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(OnLeaseBreakNotificationReceived);
            smb2ClientMainChannel.Smb2Client.OplockBreakNotificationReceived += new Action<Packet_Header, OPLOCK_BREAK_Notification_Packet>(OnOplockBreakNotificationReceived);
            serverIpMainChannel = (shareType == ReplayModelShareType.CAShare ? testConfig.CAShareServerIP : testConfig.SutIPAddress);
            serverNameMainChannel = (shareType == ReplayModelShareType.CAShare) ? testConfig.CAShareServerName : testConfig.SutComputerName;
            smb2ClientMainChannel.ConnectToServer(testConfig.UnderlyingTransport, serverNameMainChannel, serverIpMainChannel);
            
            DialectRevision[] dialects = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(maxSmbVersionClientSupported));
            uint status;

            #region Negotiate

            Capabilities_Values capability = isClientSupportPersistent ? 
                Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | 
                Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | 
                Capabilities_Values.GLOBAL_CAP_ENCRYPTION :
                Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | 
                Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            NEGOTIATE_Response? negotiateResponse = null;
            clientCapabilitiesMainChannel = ModelUtility.IsSmb3xFamily(maxSmbVersionClientSupported)? capability : Capabilities_Values.NONE;
            status = smb2ClientMainChannel.Negotiate(
                dialects,
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilitiesMainChannel,
                clientGuid: maxSmbVersionClientSupported == ModelDialectRevision.Smb2002 ? Guid.Empty : clientGuid,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);

                    negotiateResponse = response;
                });

            dialectMainChannel = negotiateResponse.Value.DialectRevision;
            #endregion

            #region SESSION_SETUP
            principleNameMainChannel = (shareType == ReplayModelShareType.CAShare ? testConfig.CAShareServerName : testConfig.SutComputerName);
            if (isReconnect)
            {
                status = smb2ClientMainChannel.ReconnectSessionSetup(
                    sessionIdMainChannel,
                    testConfig.DefaultSecurityPackage,
                    principleNameMainChannel,
                    testConfig.AccountCredential,
                    testConfig.UseServerGssToken);
                sessionIdMainChannel = smb2ClientMainChannel.SessionId;
                sessionKeyMainChannel = smb2ClientMainChannel.SessionKey;
            }
            else
            {
                status = smb2ClientMainChannel.SessionSetup(
                    testConfig.DefaultSecurityPackage,
                    principleNameMainChannel,
                    testConfig.AccountCredential,
                    testConfig.UseServerGssToken);
                sessionIdMainChannel = smb2ClientMainChannel.SessionId;
                sessionKeyMainChannel = smb2ClientMainChannel.SessionKey;
            }

            Site.Log.Add(
                LogEntryKind.Debug,
                "Global encryption disabled");

            #endregion

            #region TREE_CONNECT to share
            sharePathMainChannel = (shareType == ReplayModelShareType.CAShare ?
                Smb2Utility.GetUncPath(testConfig.CAShareServerName, testConfig.CAShareName) : Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare));
            status = smb2ClientMainChannel.TreeConnect(
                sharePathMainChannel,
                out treeId);
            Site.Log.Add(
                LogEntryKind.Debug,
                "Establish main channel to connect share {0}", sharePathMainChannel);

            smb2ClientMainChannel.SetTreeEncryption(treeId, false);
            #endregion
        }

        private void InitializeAlternativeChannel(
            Guid clientGuid,
            uint treeId,
            bool isClientSupportPersistent = true)
        {
            Site.Assume.IsNull(smb2ClientAlternativeChannel, "Expect smb2ClientAlternativeChannel is NULL.");

            testConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);

            smb2ClientAlternativeChannel = new Smb2FunctionalClient(testConfig.Timeout, testConfig, Site);
            smb2ClientAlternativeChannel.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(OnLeaseBreakNotificationReceived);
            smb2ClientAlternativeChannel.Smb2Client.OplockBreakNotificationReceived += new Action<Packet_Header, OPLOCK_BREAK_Notification_Packet>(OnOplockBreakNotificationReceived);
            smb2ClientAlternativeChannel.ConnectToServer(testConfig.UnderlyingTransport, serverNameMainChannel, serverIpMainChannel);
            uint status;

            #region Negotiate

            Capabilities_Values capability = isClientSupportPersistent ?
                Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | 
                Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | 
                Capabilities_Values.GLOBAL_CAP_ENCRYPTION :
                Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | 
                Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            NEGOTIATE_Response? negotiateResponse = null;
            status = smb2ClientAlternativeChannel.Negotiate(
                new DialectRevision[] { dialectMainChannel },
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: ModelUtility.IsSmb3xFamily(dialectMainChannel) ? capability : Capabilities_Values.NONE,
                clientGuid: dialectMainChannel == DialectRevision.Smb2002 ? Guid.Empty : clientGuid,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);

                    negotiateResponse = response;
                });
            Site.Assert.AreEqual(
                dialectMainChannel,
                negotiateResponse.Value.DialectRevision,
                "DialectRevision {0} is expected", dialectMainChannel);

            #endregion

            #region SESSION_SETUP
            status = smb2ClientAlternativeChannel.AlternativeChannelSessionSetup(
                smb2ClientMainChannel,
                testConfig.DefaultSecurityPackage,
                principleNameMainChannel,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);

            Site.Log.Add(
                LogEntryKind.Debug,
                "Global encryption disabled");

            #endregion
        }

        /// <summary>
        /// Handle the lease break notification.
        /// </summary>
        /// <param name="respHeader">The SMB2 header included in the notification.</param>
        /// <param name="leaseBreakNotify">Lease break notification payload in the notification.</param>
        private void OnLeaseBreakNotificationReceived(Packet_Header respHeader, LEASE_BREAK_Notification_Packet leaseBreakNotify)
        {
            Smb2FunctionalClient client = null;
            if (smb2ClientMainChannel != null)
            {
                client = smb2ClientMainChannel;
            }
            else if (smb2ClientAlternativeChannel != null)
            {
                client = smb2ClientAlternativeChannel;
            }

            if (client != null)
            {
                Site.Log.Add(LogEntryKind.Debug, "Receive a lease break notification and will send lease break acknowledgment.");
                client.LeaseBreakAcknowledgment(
                    treeIdMainChannel,
                    leaseKeyMainChannel,
                    leaseBreakNotify.NewLeaseState);
            }
        }

        /// <summary>
        /// Handle the oplock break notification.
        /// </summary>
        /// <param name="respHeader">The SMB2 header included in the notification.</param>
        /// <param name="oplockBreakNotify">Oplock break notification payload in the notification.</param>
        private void OnOplockBreakNotificationReceived(Packet_Header respHeader, OPLOCK_BREAK_Notification_Packet oplockBreakNotify)
        {
            Smb2FunctionalClient client = null;
            if (smb2ClientMainChannel != null)
            {
                client = smb2ClientMainChannel;
            }
            else if (smb2ClientAlternativeChannel != null)
            {
                client = smb2ClientAlternativeChannel;
            }

            if (client != null)
            {
                Site.Log.Add(LogEntryKind.Debug, "Receive an oplock break notification and will send oplock break acknowledgment.");
                client.OplockAcknowledgement(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    (OPLOCK_BREAK_Acknowledgment_OplockLevel_Values)oplockBreakNotify.OplockLevel,
                    checker: (responseHeader, response) => { }
                    );
            }
        }

        private void FillParameters(ReplayModelLeaseState leaseState, 
            ReplayModelRequestedOplockLevel requestedOplockLevel, 
            ReplayModelDurableHandle modelDurableHandle, 
            Guid createGuid,
            Guid leaseKey,
            out LeaseStateValues requestLeaseState, 
            out RequestedOplockLevel_Values oplockLevel, 
            out Smb2CreateContextRequest[] contexts)
        {
            #region Fill lease state
            switch (leaseState)
            {
                case ReplayModelLeaseState.LeaseStateNotIncludeH:
                    requestLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;
                    break;
                case ReplayModelLeaseState.LeaseStateIncludeH:
                    requestLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
                    break;
                default:
                    requestLeaseState = LeaseStateValues.SMB2_LEASE_NONE;
                    break;
            }
            #endregion

            #region Fill oplockLevel and lease context
            contexts = new Smb2CreateContextRequest[] { };
            switch (requestedOplockLevel)
            {
                case ReplayModelRequestedOplockLevel.OplockLevelLeaseV1:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);

                    oplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE;
                    contexts = new Smb2CreateContextRequest[]
                    {
                        new Smb2CreateRequestLease
                        {
                            LeaseKey = leaseKey,
                            LeaseState = requestLeaseState,
                            LeaseFlags = (uint)LeaseFlagsValues.NONE
                        }
                    };
                    break;
                case ReplayModelRequestedOplockLevel.OplockLevelLeaseV2:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);

                    oplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE;
                    contexts = new Smb2CreateContextRequest[]
                    {
                        new Smb2CreateRequestLeaseV2
                        {
                            LeaseKey = leaseKey,
                            LeaseState = requestLeaseState,
                            LeaseFlags = (uint)LeaseFlagsValues.NONE
                        }
                    };
                    break;
                case ReplayModelRequestedOplockLevel.OplockLevelBatch:
                    oplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH;
                    break;
                case ReplayModelRequestedOplockLevel.OplockLevelII:
                    oplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_II;
                    break;
                default:
                    oplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE;
                    break;
            }
            #endregion

            #region Fill handle context
            switch (modelDurableHandle)
            {
                case ReplayModelDurableHandle.DurableHandleV1:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST);

                    contexts = Smb2Utility.Append<Smb2CreateContextRequest>(contexts,
                            new Smb2CreateDurableHandleRequest
                            {
                            });
                    break;
                case ReplayModelDurableHandle.DurableHandleV2:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);

                    contexts = Smb2Utility.Append<Smb2CreateContextRequest>(contexts,
                            new Smb2CreateDurableHandleRequestV2
                            {
                                CreateGuid = createGuid,
                                Timeout = 120 * 1000,
                            });
                    break;
                case ReplayModelDurableHandle.DurableHandleV2Persistent:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);

                    contexts = Smb2Utility.Append<Smb2CreateContextRequest>(contexts,
                            new Smb2CreateDurableHandleRequestV2
                            {
                                CreateGuid = createGuid,
                                Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                                Timeout = 120 * 1000,
                            });
                    break;
                default:
                    break;
            }
            #endregion
        }

        private ReplayModelDurableHandle ConvertHandle(Smb2CreateContextResponse[] serverCreateContexts)
        {
            ReplayModelDurableHandle responseHandle = ReplayModelDurableHandle.NormalHandle;
            if (serverCreateContexts != null)
            {
                foreach (var context in serverCreateContexts)
                {
                    if (context is Smb2CreateDurableHandleResponse)
                    {
                        responseHandle = ReplayModelDurableHandle.DurableHandleV1;
                        break;
                    }
                    else if (context is Smb2CreateDurableHandleResponseV2)
                    {
                        Smb2CreateDurableHandleResponseV2 handleV2 = context as Smb2CreateDurableHandleResponseV2;
                        if (handleV2.Flags == CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT)
                        {
                            responseHandle = ReplayModelDurableHandle.DurableHandleV2Persistent;
                        }
                        else
                        {
                            responseHandle = ReplayModelDurableHandle.DurableHandleV2;
                        }
                        break;
                    }
                }
            }

            return responseHandle;
        }

        private void FillChannelSequence(Smb2FunctionalClient client, ReplayModelChannelSequenceType channelSequence)
        {
            switch (channelSequence)
            {
                case ReplayModelChannelSequenceType.ChannelSequenceIncrementOne:
                    client.SessionChannelSequence = 1;
                    break;
                case ReplayModelChannelSequenceType.ChannelSequenceBoundaryValid:
                    client.SessionChannelSequence = 0x7FFF;
                    break;
                case ReplayModelChannelSequenceType.InvalidChannelSequence:
                    client.SessionChannelSequence = 0x8000;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Actions
        public void ReadConfig(out ReplayServerConfig c)
        {
            writeContent = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);

            c = new ReplayServerConfig
            {
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
                IsDirectoryLeasingSupported = testConfig.IsDirectoryLeasingSupported,
                IsLeasingSupported = testConfig.IsLeasingSupported,
                IsPersistentHandleSupported = testConfig.IsPersistentHandlesSupported,
                TreeConnect_Share_Type_Include_STYPE_CLUSTER_SOFS = Boolean.Parse(testConfig.GetProperty("ShareTypeInclude_STYPE_CLUSTER_SOFS")),
                Platform = testConfig.Platform,
            };

            replayConfig = c;

            testConfig.CheckDialect(DialectRevision.Smb30);

            Site.Log.Add(LogEntryKind.Debug, c.ToString());
        }

        #region Test Create

        public void PrepareCreate(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelClientSupportPersistent isClientSupportPersistent,
            ReplayModelDurableHandle modelDurableHandle,
            ReplayModelShareType shareType,
            ReplayModelRequestedOplockLevel requestedOplockLevel,
            ReplayModelLeaseState leaseState)
        {
            InitializeMainChannel(maxSmbVersionClientSupported, clientGuidMainChannel, shareType, out treeIdMainChannel, false, isClientSupportPersistent == ReplayModelClientSupportPersistent.ClientSupportPersistent);

            Smb2CreateContextRequest[] contexts;
            RequestedOplockLevel_Values oplockLevel;
            LeaseStateValues requestLeaseState;

            FillParameters(leaseState, requestedOplockLevel, modelDurableHandle, createGuidMainChannel,
                leaseKeyMainChannel, out requestLeaseState, out oplockLevel, out contexts);

            Smb2CreateContextResponse[] serverCreateContexts;
            smb2ClientMainChannel.Create(
                treeIdMainChannel,
                fileNameMainChannel,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdMainChannel,
                out serverCreateContexts,
                oplockLevel,
                contexts
            );

            AddTestFileName(this.sharePathMainChannel, fileNameMainChannel);

            ReplayModelDurableHandle preparedHandle = ConvertHandle(serverCreateContexts);
            Site.Assert.IsTrue(modelDurableHandle == preparedHandle, "Prepared handle should be {0}, actual is {1}", modelDurableHandle, preparedHandle);

            prepared = true;
        }

        public void CreateRequest(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelShareType shareType,
            ReplayModelClientSupportPersistent isClientSupportPersistent,
            ReplayModelSwitchChannelType switchChannelType,
            ReplayModelChannelSequenceType channelSequence,
            ReplayModelSetReplayFlag isSetReplayFlag,
            ReplayModelDurableHandle modelDurableHandle,
            ReplayModelRequestedOplockLevel requestedOplockLevel,
            ReplayModelLeaseState leaseState,
            ReplayModelFileName fileName,
            ReplayModelCreateGuid createGuid,
            ReplayModelFileAttributes fileAttributes,
            ReplayModelCreateDisposition createDisposition,
            ReplayModelLeaseKey leaseKey)
        {
            Smb2FunctionalClient client = null;

            switch (switchChannelType)
            {
                case ReplayModelSwitchChannelType.MainChannel:
                {
                    if (smb2ClientMainChannel == null)
                    {
                        InitializeMainChannel(maxSmbVersionClientSupported, clientGuidMainChannel, shareType,
                            out treeIdMainChannel, false,
                            isClientSupportPersistent == ReplayModelClientSupportPersistent.ClientSupportPersistent);
                    }
                    break;
                }
                case ReplayModelSwitchChannelType.ReconnectMainChannel:
                {
                    Site.Assume.IsNotNull(smb2ClientMainChannel, "Main channel is expected to exist.");
                    smb2ClientMainChannel.Disconnect();
                    smb2ClientMainChannel = null;
                    InitializeMainChannel(maxSmbVersionClientSupported, clientGuidMainChannel, shareType,
                        out treeIdMainChannel, true,
                        isClientSupportPersistent == ReplayModelClientSupportPersistent.ClientSupportPersistent);
                    break;
                }
                default: //AlternativeChannelWithMainChannel, AlternativeChannelWithDisconnectMainChannel, MainChannelWithAlternativeChannel
                {
                    InitializeAlternativeChannel(
                        clientGuidMainChannel,
                        treeIdMainChannel,
                        isClientSupportPersistent == ReplayModelClientSupportPersistent.ClientSupportPersistent);

                    if (switchChannelType == ReplayModelSwitchChannelType.AlternativeChannelWithDisconnectMainChannel)
                    {
                        smb2ClientMainChannel.Disconnect();
                        smb2ClientMainChannel = null;
                    }
                    break;
                }
            }

            if (switchChannelType == ReplayModelSwitchChannelType.AlternativeChannelWithDisconnectMainChannel ||
                switchChannelType == ReplayModelSwitchChannelType.AlternativeChannelWithMainChannel)
            {
                client = smb2ClientAlternativeChannel;
            }
            else
            {
                client = smb2ClientMainChannel;
            }

            Smb2CreateContextRequest[] contexts;
            RequestedOplockLevel_Values oplockLevel;
            LeaseStateValues requestLeaseState;

            FillParameters(leaseState,
                requestedOplockLevel, 
                modelDurableHandle, 
                createGuid == ReplayModelCreateGuid.DefaultCreateGuid ? createGuidMainChannel : Guid.NewGuid(), 
                leaseKey == ReplayModelLeaseKey.DefaultLeaseKey ? leaseKeyMainChannel : Guid.NewGuid(),
                out requestLeaseState, 
                out oplockLevel, 
                out contexts);

            FillChannelSequence(client, channelSequence);

            Smb2CreateContextResponse[] serverCreateContexts;

            string realFileName = fileName == ReplayModelFileName.DefaultFileName ?
                fileNameMainChannel : this.CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            ulong createRequestId;
            client.CreateRequest(
                    treeIdMainChannel,
                    realFileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    (isSetReplayFlag == ReplayModelSetReplayFlag.WithReplayFlag ? Packet_Header_Flags_Values.FLAGS_REPLAY_OPERATION : Packet_Header_Flags_Values.NONE) | (testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE),
                    out createRequestId,
                    oplockLevel,
                    contexts,
                    createDisposition: createDisposition == ReplayModelCreateDisposition.DefaultCreateDisposition ? CreateDisposition_Values.FILE_OPEN_IF : CreateDisposition_Values.FILE_OVERWRITE_IF,
                    fileAttributes: fileAttributes == ReplayModelFileAttributes.DefaultFileAttributes ? File_Attributes.NONE : File_Attributes.FILE_ATTRIBUTE_TEMPORARY
                 );

            AddTestFileName(sharePathMainChannel, realFileName);

            uint status = client.CreateResponse(
                    createRequestId,
                    out fileIdMainChannel,
                    out serverCreateContexts,
                    checker: (header, response) =>
                    {
                    }
                 );

            CreateResponse(
                (ModelSmb2Status)status,
                ConvertHandle(serverCreateContexts),
                replayConfig);
        }

        #endregion

        #region Test READ/WRITE/SET_INFO/IOCTL

        public void PrepareFileOperation(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelRequestCommand requestCommand)
        {
            if (requestCommand == ReplayModelRequestCommand.IoCtl)
            {
                testConfig.CheckIOCTL(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY);
            }

            InitializeMainChannel(
                maxSmbVersionClientSupported,
                clientGuidMainChannel,
                ReplayModelShareType.NonCAShare,
                out treeIdMainChannel);

            uint status = 0;

            #region Create
            Smb2CreateContextResponse[] serverCreateContexts = null;

            status = smb2ClientMainChannel.Create(
                treeIdMainChannel,
                fileNameMainChannel,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdMainChannel,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                null);
            AddTestFileName(sharePathMainChannel, fileNameMainChannel);
            #endregion

            if (requestCommand == ReplayModelRequestCommand.Write)
            {
                #region Write
                status = smb2ClientMainChannel.Write(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    writeContent);
                #endregion
            }
            else if (requestCommand == ReplayModelRequestCommand.SetInfo)
            {
                #region SetInfo
                byte[] buffer = TypeMarshal.ToBytes<FileEndOfFileInformation>(endOfFileInformation);

                status = smb2ClientMainChannel.SetFileAttributes(
                    treeIdMainChannel,
                    (byte)FileInformationClasses.FileEndOfFileInformation,
                    fileIdMainChannel,
                    buffer);
                #endregion
            }
            else if (requestCommand == ReplayModelRequestCommand.IoCtl)
            {
                #region IOCtl
                Packet_Header ioCtlHeader;
                IOCTL_Response ioCtlResponse;
                byte[] inputInResponse;
                byte[] outputInResponse;
                status = smb2ClientMainChannel.ResiliencyRequest(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    0,
                    (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                    out ioCtlHeader,
                    out ioCtlResponse,
                    out inputInResponse,
                    out outputInResponse,
                    checker: (header, response) =>
                    {
                        // do nothing, skip the exception
                    }
                    );
                #endregion
            }
            else if (requestCommand == ReplayModelRequestCommand.Read)
            {
                #region Read
                #region Prepare data for read
                status = smb2ClientMainChannel.Write(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    writeContent);
                #endregion

                string data;
                status = smb2ClientMainChannel.Read(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    0,
                    (uint)testConfig.WriteBufferLengthInKb * 1024,
                    out data);
                #endregion
            }

            prepared = true;
        }

        public void FileOperationRequest(
            ReplayModelSwitchChannelType switchChannelType,
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelRequestCommand requestCommand,
            ReplayModelChannelSequenceType channelSequence,
            ReplayModelSetReplayFlag isReplay,
            ReplayModelRequestCommandParameters requestCommandParameters)
        {
            if (requestCommand == ReplayModelRequestCommand.IoCtl)
            {
                testConfig.CheckIOCTL(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY);
            }

            uint status = Smb2Status.STATUS_SUCCESS;
            Smb2FunctionalClient client = null;

            #region Switch channel
            switch (switchChannelType)
            {
                case ReplayModelSwitchChannelType.MainChannel:
                    if (smb2ClientMainChannel == null)
                    {
                        InitializeMainChannel(
                            maxSmbVersionClientSupported,
                            clientGuidMainChannel,
                            ReplayModelShareType.NonCAShare,
                            out treeIdMainChannel);

                        #region Create
                        Smb2CreateContextResponse[] serverCreateContexts = null;

                        status = smb2ClientMainChannel.Create(
                            treeIdMainChannel,
                            fileNameMainChannel,
                            CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                            out fileIdMainChannel,
                            out serverCreateContexts,
                            RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                            null,
                            shareAccess: ShareAccess_Values.NONE);
                        AddTestFileName(this.sharePathMainChannel, fileNameMainChannel);

                        #endregion
                    }
                    client = smb2ClientMainChannel;
                    break;
                case ReplayModelSwitchChannelType.AlternativeChannelWithMainChannel:
                    InitializeAlternativeChannel(
                        clientGuidMainChannel,
                        treeIdMainChannel);
                    client = smb2ClientAlternativeChannel;
                    break;
                case ReplayModelSwitchChannelType.AlternativeChannelWithDisconnectMainChannel:
                    InitializeAlternativeChannel(
                        clientGuidMainChannel,
                        treeIdMainChannel);
                    smb2ClientMainChannel.Disconnect();
                    smb2ClientMainChannel = null;
                    client = smb2ClientAlternativeChannel;
                    break;
                case ReplayModelSwitchChannelType.MainChannelWithAlternativeChannel:
                    InitializeAlternativeChannel(
                        clientGuidMainChannel,
                        treeIdMainChannel);
                    client = smb2ClientMainChannel;
                    break;
                default:
                    Site.Assume.Fail("Unknown ReplayModelSwitchChannelType {0}.", switchChannelType);
                    break;
            }
            #endregion

            #region Prepare data for read
            if (switchChannelType == ReplayModelSwitchChannelType.MainChannel && !prepared && requestCommand == ReplayModelRequestCommand.Read)
            {
                status = smb2ClientMainChannel.Write(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    writeContent);
            }
            #endregion

            FillChannelSequence(client, channelSequence);

            if (requestCommand == ReplayModelRequestCommand.Write)
            {
                #region Write
                status = client.Write(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    requestCommandParameters == ReplayModelRequestCommandParameters.DefaultParameters ? writeContent : Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb),
                    checker: (header, response) => 
                    {
                    },
                    isReplay: isReplay == ReplayModelSetReplayFlag.WithReplayFlag);
                #endregion
            }
            else if (requestCommand == ReplayModelRequestCommand.SetInfo)
            {
                #region SetInfo
                if (requestCommandParameters == ReplayModelRequestCommandParameters.AlternativeParameters)
                {
                    endOfFileInformation.EndOfFile = 512;
                }
                byte[] buffer = TypeMarshal.ToBytes<FileEndOfFileInformation>(endOfFileInformation);

                status = client.SetFileAttributes(
                    treeIdMainChannel,
                    (byte)FileInformationClasses.FileEndOfFileInformation,
                    fileIdMainChannel,
                    buffer,
                    checker: (header, response) =>
                    {
                    },
                    isReplay: isReplay == ReplayModelSetReplayFlag.WithReplayFlag);
                #endregion
            }
            else if (requestCommand == ReplayModelRequestCommand.IoCtl)
            {
                #region IOCtl
                Packet_Header ioCtlHeader;
                IOCTL_Response ioCtlResponse;
                byte[] inputInResponse;
                byte[] outputInResponse;
                status = client.ResiliencyRequest(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    (uint)(requestCommandParameters == ReplayModelRequestCommandParameters.DefaultParameters ? 0 : 2000),
                    (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                    out ioCtlHeader,
                    out ioCtlResponse,
                    out inputInResponse,
                    out outputInResponse,
                    checker: (header, response) =>
                    {
                        // do nothing, skip the exception
                    }
                    );
                #endregion
            }
            else if (requestCommand == ReplayModelRequestCommand.Read)
            {
                #region Read
                string data;
                status = client.Read(
                    treeIdMainChannel,
                    fileIdMainChannel,
                    0,
                    requestCommandParameters == ReplayModelRequestCommandParameters.DefaultParameters ? (uint)testConfig.WriteBufferLengthInKb * 1024 : 512,
                    out data,
                    isReplay: isReplay == ReplayModelSetReplayFlag.WithReplayFlag);
                #endregion
            }

            FileOperationResponse((ModelSmb2Status)status, replayConfig);
        }

        #endregion

        #endregion
    }
}
