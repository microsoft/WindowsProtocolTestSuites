// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Replay;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.Model.Replay
{
    /// <summary>
    /// Lease object used in replay model
    /// </summary>
    public class ReplayLease
    {
        /// <summary>
        /// Indicates that the current state of the lease. 
        /// </summary>
        public ReplayModelLeaseState LeaseState;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leaseState">LeaseState value set in the lease object</param>
        public ReplayLease(ReplayModelLeaseState leaseState)
        {
            LeaseState = leaseState;
        }

    }

    /// <summary>
    /// Open object used in replay model
    /// </summary>
    public class ReplayOpen
    {
        /// <summary>
        /// Indicates whether there is a reference to the connection or not.
        /// </summary>
        public bool ConnectionIsNotNull;

        /// <summary>
        /// A Boolean that indicates whether this open has requested durable operation.
        /// </summary>
        public bool IsDurable;

        /// <summary>
        /// A Boolean that indicates whether this open is persistent.
        /// </summary>
        public bool IsPersistent;

        /// <summary>
        /// A numerical value that indicates the number of outstanding requests issued with ChannelSequence equal to Open.ChannelSequence.
        /// </summary>
        public int OutstandingRequestCount;

        /// <summary>
        /// A numerical value that indicates the number of outstanding requests issued with ChannelSequence less than Open.ChannelSequence.
        /// </summary>
        public int OutstandingPreRequestCount;

        /// <summary>
        /// Open.Lease
        /// </summary>
        public ReplayLease Lease;

        /// <summary>
        /// A Boolean that indicates whether Open.CreateGuid is null or not.
        /// </summary>
        public bool CreateGuidIsNotNull;

        /// <summary>
        /// Indicates that the current oplock level for this open.
        /// </summary>
        public ReplayModelRequestedOplockLevel OplockLevel;

        /// <summary>
        /// A Boolean that indicates whether Open.OplockState is Held or not.
        /// </summary>
        public bool OplockStateIsHeld;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ReplayOpen()
        {
            ConnectionIsNotNull = true;
            IsDurable = false;
            IsPersistent = false;
            OutstandingRequestCount = 0;
            OutstandingPreRequestCount = 0;
            Lease = null;
            CreateGuidIsNotNull = false;
            OplockLevel = ReplayModelRequestedOplockLevel.OplockLevelNone;
            OplockStateIsHeld = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modelDurableHandle">Type of durable handle for this open</param>
        /// <param name="isPersistent">Indicate if the open is persistent</param>
        /// <param name="oplockLevel">Indicate the oplock level</param>
        public ReplayOpen(ReplayModelDurableHandle modelDurableHandle, bool isPersistent, ReplayModelRequestedOplockLevel oplockLevel)
        {
            ConnectionIsNotNull = true;

            if (modelDurableHandle != ReplayModelDurableHandle.NormalHandle)
            {
                IsDurable = true;

                if (modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2Persistent)
                {
                    IsPersistent = isPersistent;
                }
            }

            OutstandingRequestCount = 0;
            OutstandingPreRequestCount = 0;
            Lease = null;
            CreateGuidIsNotNull = false;
            OplockLevel = oplockLevel;
        }
    }

    /// <summary>
    /// This models behavior of Replay for CREATE, WRITE, READ, IOCTL, SET_INFO operations
    /// Assumptions/Restrictions/Notes:
    /// 1. Only models behavior for server that implements 3.x dialect family or higher due to dependency on multiple channel capability
    /// 2. Keep open target to non-directory file
    /// 3. Restrict clientMaxSupportDialect to 3.x family, scenarios with non 3.x dialect family to be covered in traditional cases
    /// 4. Keep using same negotiated dialect when prepare a new open and reconnect to an existing open
    /// </summary>
    public class ReplayModel
    {
        #region Fields
        /// <summary>
        /// The state of the model.
        /// </summary>
        public static ModelState State = ModelState.Uninitialized;

        /// <summary>
        /// Configuration related to replay.
        /// </summary>
        public static ReplayServerConfig Config;

        /// <summary>
        /// The request which the model is handling.
        /// </summary>
        public static ModelSMB2Request Request;

        /// <summary>
        /// Indicates which request message will be replayed.
        /// </summary>
        public static ReplayModelRequestCommand ModelRequestCommand;

        /// <summary>
        /// Indicates that the open in the server.
        /// </summary>
        public static ReplayOpen Open;

        /// <summary>
        /// Main channel.
        /// </summary>
        public static ModelReplayChannel MainChannel;

        /// <summary>
        /// Alternative channel.
        /// </summary>
        public static ModelReplayChannel AlternativeChannel;

        /// <summary>
        /// When the first open is set to null, its value will be stored in this field.
        /// </summary>
        public static ReplayOpen LastOpen;

        #endregion

        #region Rules
        #region Config
        /// <summary>
        /// The call for reading configuration.
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
        }

        /// <summary>
        /// The return for reading configuration.
        /// </summary>
        /// <param name="c">The configuration related to replay.</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(ReplayServerConfig c)
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
            Condition.IsNotNull(c);

            Config = c;

            Open = null;
            LastOpen = null;
            MainChannel = null;
            AlternativeChannel = null;
            ModelRequestCommand = ReplayModelRequestCommand.NoRequest;

            State = ModelState.Initialized;
        }
        #endregion

        #region Test Create

        /// <summary>
        /// Prepare the first create.
        /// </summary>
        /// <param name="maxSmbVersionClientSupported">Max version that the client supports.</param>
        /// <param name="clientPersistentCapability">Indicates whether capabilities of Negotiate request includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES or not.</param>
        /// <param name="modelDurableHandle">Indicates the type of file handle.</param>
        /// <param name="shareType">Indicates whether to connect a CAShare or NonCAShare.</param>
        /// <param name="requestedOplockLevel">Indicates requested oplock level.</param>
        /// <param name="leaseState">Indicates requested lease state.</param>
        [Rule]
        public static void PrepareCreate(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelClientSupportPersistent clientPersistentCapability,
            ReplayModelDurableHandle modelDurableHandle,
            ReplayModelShareType shareType,
            ReplayModelRequestedOplockLevel requestedOplockLevel,
            ReplayModelLeaseState leaseState)
        {
            Condition.IsNull(MainChannel);

            DialectRevision negotiateDialect = ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported,
                Config.MaxSmbVersionSupported);

            Combination.NWise(2, maxSmbVersionClientSupported, clientPersistentCapability, modelDurableHandle, shareType,
                requestedOplockLevel, leaseState);

            //No lease state if no lease requested
            Condition.IfThen(requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 && 
                requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelLeaseV2,
                leaseState == ReplayModelLeaseState.LeaseStateIsNone);

            //Do not set lease state to none if requesting a lease
            Condition.IfThen(requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 || 
                requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2,
                leaseState != ReplayModelLeaseState.LeaseStateIsNone);

            MainChannel = new ModelReplayChannel(negotiateDialect, shareType, 
                clientPersistentCapability == ReplayModelClientSupportPersistent.ClientSupportPersistent);

            if (Config.TreeConnect_Share_Type_Include_STYPE_CLUSTER_SOFS &&
                requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelBatch)
            {
                // Replay will always use 3.x family
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.9: If Connection.Dialect belongs to the SMB 3.x dialect family TreeConnect.Share.Type" +
                    " includes STYPE_CLUSTER_SOFS and the RequestedOplockLevel is SMB2_OPLOCK_LEVEL_BATCH," +
                    " the server MUST set RequestedOplockLevel to SMB2_OPLOCK_LEVEL_II");

                requestedOplockLevel = ReplayModelRequestedOplockLevel.OplockLevelII;
            }

            Open = new ReplayOpen();

            if (requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1)
            {
                // Replay will always use 3.x family
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.9: If Connection.Dialect is \"2.100\" or belongs to the \"3.x\" dialect family, and the DataLength field equals 0x20," +
                    " the server MUST attempt to acquire a lease on the open from the underlying object store as described in section 3.3.5.9.8");
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.9.8: The server MUST set Open.OplockState to Held");

                Open.Lease = new ReplayLease(leaseState);
                Open.OplockStateIsHeld = true;
            }

            if (requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2)
            {
                // Replay will always use 3.x family
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.9: If Connection.Dialect belongs to the \"3.x\" dialect family, and the DataLength field equals 0x34," +
                    " the server MUST attempt to acquire a lease on the open from the underlying object store, as described in section 3.3.5.9.11");
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.9.11: The server MUST set Open.OplockState to Held");

                Open.Lease = new ReplayLease(leaseState);
                Open.OplockStateIsHeld = true;
            }

            Open.OplockLevel = requestedOplockLevel;
            State = ModelState.Connected;

            switch (modelDurableHandle)
            {
                case ReplayModelDurableHandle.DurableHandleV1:
                {
                    if (requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelBatch &&
                        !((requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                           requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2) && //TODO:TDI, TD does not mention skipping the section if contains SMB2_CREATE_REQUEST_LEASE_V2 without H
                          leaseState == ReplayModelLeaseState.LeaseStateIncludeH))
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9.6: If the RequestedOplockLevel field in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH and" +
                            " the create request does not include an SMB2_CREATE_REQUEST_LEASE create context with a LeaseState field that includes the SMB2_LEASE_HANDLE_CACHING bit value," +
                            " the server MUST ignore this create context and skip this section.");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Create context is ignored and skipped due to above conditions");
                        ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    }
                    else
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9.6: In the \"Successful Open Initialization\" phase, the server MUST set Open.IsDurable to TRUE." +
                            " This permits the client to use Open.DurableFileId to request a reopen of the file on a subsequent request as specified in section 3.3.5.9.7." +
                            " The server MUST also set Open.DurableOwner to a security descriptor accessible only by the user represented by Open.Session.SecurityContext.");

                        Open.IsDurable = true;

                        ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is set to TRUE");
                    }
                    break;
                }
                case ReplayModelDurableHandle.DurableHandleV2:
                case ReplayModelDurableHandle.DurableHandleV2Persistent:
                {
                    if (modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2 &&
                        requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelBatch &&
                        !((requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                           requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2) &&
                          leaseState == ReplayModelLeaseState.LeaseStateIncludeH))
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9.10: If the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of this create context," +
                            " if RequestedOplockLevel in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH, and if the create request does not include" +
                            " a SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context with a LeaseState field that includes SMB2_LEASE_HANDLE_CACHING," +
                            " the server MUST ignore this create context and skip this section");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Create context is ignored and skipped due to above conditions");
                        ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    }
                    else
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9.10: The server MUST set Open.CreateGuid to the CreateGuid in SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2.");

                        Open.CreateGuidIsNotNull = true;

                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9.10: In the \"Successful Open Initialization\" phase, the server MUST set Open.IsDurable to TRUE." +
                            " The server MUST also set Open.DurableOwner to a security descriptor accessible only by the user represented by Open.Session.SecurityContext.");

                        Open.IsDurable = true;

                        ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is set to TRUE");

                        if (modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2Persistent &&
                            shareType == ReplayModelShareType.CAShare &&
                            clientPersistentCapability == ReplayModelClientSupportPersistent.ClientSupportPersistent) //assume connection dialect always 3.x
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: If the SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the request, TreeConnect.Share.IsCA is TRUE," +
                                " and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES, the server MUST set Open.IsPersistent to TRUE");

                            Open.IsPersistent = true;

                            ModelHelper.Log(LogType.TestInfo, "Open.IsPersistent is set to TRUE");
                        }
                    }
                    break;
                }
                case ReplayModelDurableHandle.NormalHandle:
                {
                    //Do nothing for NormalHandle
                    break;
                }
                default:
                    break;
            }
        }

        /// <summary>
        /// Create request.
        /// </summary>
        /// <param name="maxSmbVersionClientSupported">Max version that the client supports.</param>
        /// <param name="shareType">Indicates whether to connect a CAShare or not.</param>
        /// <param name="clientPersistentCapability">Indicates whether capabilities of Negotiate request includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES or not.</param>
        /// <param name="switchChannelType">Indicates the type of switching channel.</param>
        /// <param name="channelSequence">Indicates the type of session channel sequence.</param>
        /// <param name="isSetReplayFlag">Indicates whether a replay flag is set or not.</param>
        /// <param name="modelDurableHandle">Indicates the type of file handle.</param>
        /// <param name="requestedOplockLevel">Indicates requested oplock level.</param>
        /// <param name="leaseState">Indicates requested lease state.</param>
        /// <param name="fileName">Indicates whether to create a file with the same file name or not.</param>
        /// <param name="createGuid">Indicates whether to create a file with the same CreateGuid or not.</param>
        /// <param name="fileAttributes">Indicates whether to create a file with the same FileAttributes or not.</param>
        /// <param name="createDisposition">Indicates whether to create a file with the same CreateDisposition or not.</param>
        /// <param name="leaseKey">Indicates requested lease key.</param>
        [Rule]
        public static void CreateRequest(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelShareType shareType,
            ReplayModelClientSupportPersistent clientPersistentCapability,
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
            Condition.IsTrue(State == ModelState.Connected || State == ModelState.Initialized);
            Condition.IsNull(Request);

            Combination.NWise(2, maxSmbVersionClientSupported, shareType, clientPersistentCapability, switchChannelType, channelSequence, isSetReplayFlag, modelDurableHandle,
                requestedOplockLevel, leaseState, fileName, createGuid, fileAttributes, createDisposition, leaseKey);

            Combination.Isolated(channelSequence == ReplayModelChannelSequenceType.InvalidChannelSequence);
            Combination.Isolated(channelSequence == ReplayModelChannelSequenceType.ChannelSequenceBoundaryValid);

            //No lease for non durable handle 
            Condition.IfThen(modelDurableHandle == ReplayModelDurableHandle.NormalHandle,
                requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 &&
                requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelLeaseV2);

            //No lease state or lease key(default) if not requesting lease
            Condition.IfThen(
                requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 &&
                requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelLeaseV2,
                leaseState == ReplayModelLeaseState.LeaseStateIsNone && leaseKey == ReplayModelLeaseKey.DefaultLeaseKey);

            //Ensure valid lease state if requesting lease
            Condition.IfThen(
                requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2,
                leaseState != ReplayModelLeaseState.LeaseStateIsNone);

            if (State == ModelState.Initialized)
            {
                Condition.IsTrue(switchChannelType == ReplayModelSwitchChannelType.MainChannel);

                DialectRevision negotiateDialect = ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported,
                    Config.MaxSmbVersionSupported);

                MainChannel = new ModelReplayChannel(negotiateDialect, shareType,
                    clientPersistentCapability == ReplayModelClientSupportPersistent.ClientSupportPersistent);
            }
            else // There's already an open exist
            {
                //Open eixsts
                Condition.IsTrue(Open != null);

                // Ensure same dialect
                Condition.IsTrue(ModelUtility.GetDialectRevision(maxSmbVersionClientSupported) == MainChannel.Connection_NegotiateDialect);

                // Ensure connect to same share
                Condition.IsTrue(shareType == MainChannel.Connection_Session_TreeConnect_Share_IsCA);

                switch (switchChannelType)
                {
                    case ReplayModelSwitchChannelType.MainChannel:
                    {
                        Condition.IsTrue((clientPersistentCapability ==
                                          ReplayModelClientSupportPersistent.ClientSupportPersistent) ==
                                         MainChannel.Connection_ClientCapabilities_SupportPersistent);
                        break;
                    }
                    case ReplayModelSwitchChannelType.ReconnectMainChannel:
                    {
                        MainChannel.Connection_ClientCapabilities_SupportPersistent =
                            (clientPersistentCapability == ReplayModelClientSupportPersistent.ClientSupportPersistent);
                        break;
                    }
                    case ReplayModelSwitchChannelType.AlternativeChannelWithDisconnectMainChannel:
                    case ReplayModelSwitchChannelType.AlternativeChannelWithMainChannel:
                    case ReplayModelSwitchChannelType.MainChannelWithAlternativeChannel:
                    {
                        AlternativeChannel = new ModelReplayChannel(MainChannel.Connection_NegotiateDialect, shareType,
                            clientPersistentCapability == ReplayModelClientSupportPersistent.ClientSupportPersistent);
                        break;
                    }
                }

                #region Handle state changes when there's connection lost or reconnection
                if (switchChannelType == ReplayModelSwitchChannelType.AlternativeChannelWithDisconnectMainChannel ||
                    switchChannelType == ReplayModelSwitchChannelType.ReconnectMainChannel)
                {
                    bool isPreserved = false;

                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.7.1: The server MUST iterate over the Session.OpenTable and determine whether each Open is to be preserved for reconnect." +
                        " If any of the following conditions is satisfied, it indicates that the Open is to be preserved for reconnect");
                    ModelHelper.Log(
                        LogType.TestInfo,
                        "Open.OplockLevel is {0}, Open.OplockState is {1}, Open.IsDurable {2}", Open.OplockLevel,
                        Open.OplockStateIsHeld ? "Held" : "not Held", Open.IsDurable);
                    if (Open.Lease != null)
                    {
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "LeaseState is {0}", Open.Lease.LeaseState);
                    }

                    // TDI: TD does not states the open is preserved when multi channels exist.
                    // todo: TDI # TBD
                    if (switchChannelType == ReplayModelSwitchChannelType.AlternativeChannelWithDisconnectMainChannel)
                    {
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "TD does not states the open is preserved when multi channels exist.");
                        isPreserved = true;
                    }
                    else if (Open.OplockLevel == ReplayModelRequestedOplockLevel.OplockLevelBatch &&
                             Open.OplockStateIsHeld &&
                             Open.IsDurable) // Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH and Open.OplockState is equal to Held, and Open.IsDurable is TRUE");

                        isPreserved = true;

                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Open is to be preserved for reconnect");
                    }
                    else if (((Open.OplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                               Open.OplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2) && //TODO: TDI for Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_LEASE_V2
                              Open.Lease != null &&
                              Open.Lease.LeaseState == ReplayModelLeaseState.LeaseStateIncludeH &&
                              Open.OplockStateIsHeld &&
                              Open.IsDurable)) // Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_LEASE and contains SMB2_LEASE_HANDLE_CACHING
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_LEASE, Lease.LeaseState contains SMB2_LEASE_HANDLE_CACHING," +
                            " Open.OplockState is equal to Held, and Open.IsDurable is TRUE.");

                        isPreserved = true;

                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Open is to be preserved for reconnect");
                    }
                    // "The server supports leasing and Open.IsResilient is TRUE" covered by resilient model

                    if (isPreserved)
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "If the Open is to be preserved for reconnect, perform the following actions:");
                        ModelHelper.Log(
                            LogType.Requirement,
                            "Set Open.Connection to NULL");
                        //Skip requirement for resilient handle which covered by resilient model

                        Open.ConnectionIsNotNull = false;
                    }
                    else
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "If the Open is not to be preserved for reconnect, the server MUST close the Open as specified in section 3.3.4.17");

                        LastOpen = Open;
                        Open = null;

                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Open is set to NULL");
                    }
                }
                #endregion

            }

            ModelReplayChannel channel = GetChannel(switchChannelType);

            ModelReplayCreateRequest replayCreateRequest = 
                new ModelReplayCreateRequest(channel, switchChannelType, channelSequence, modelDurableHandle, requestedOplockLevel, fileName,
                    createGuid, fileAttributes, createDisposition, leaseState, isSetReplayFlag, leaseKey);

            Request = replayCreateRequest;
            State = ModelState.Connected;
        }

        /// <summary>
        /// Create response.
        /// </summary>
        /// <param name="status">The status in the response.</param>
        /// <param name="durableHandleResponse">The durable handle context in the response.</param>
        /// <param name="c">Config.</param>
        [Rule]
        public static void CreateResponse(
            ModelSmb2Status status,
            ReplayModelDurableHandle durableHandleResponse,
            ReplayServerConfig c)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNotNull(Request);

            ModelReplayCreateRequest createRequest = ModelHelper.RetrieveOutstandingRequest<ModelReplayCreateRequest>(ref Request);

            bool openIsFound = false;

            if (PreCheckChannelSequence(ReplayModelRequestCommand.Create, createRequest.channel.Connection_NegotiateDialect, ref createRequest.channelSequence,
                createRequest.isSetReplayFlag == ReplayModelSetReplayFlag.WithReplayFlag, status))
            {
                do
                {
                    // TDI: TD does not state this.
                    // todo: TDI# TBD
                    if (createRequest.switchChannelType == ReplayModelSwitchChannelType.ReconnectMainChannel && 
                        LastOpen != null &&
                        LastOpen.IsPersistent &&
                        LastOpen.Lease.LeaseState == ReplayModelLeaseState.LeaseStateNotIncludeH &&
                        createRequest.fileName == ReplayModelFileName.DefaultFileName &&
                        createRequest.leaseState == ReplayModelLeaseState.LeaseStateIncludeH &&
                        createRequest.leaseKey == ReplayModelLeaseKey.DefaultLeaseKey)
                    {
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "SwitchChannelType is {0}, LastOpen is NOT null, LastOpen is persistent, LastOpen.Lease is {1}",
                            createRequest.switchChannelType, LastOpen.Lease.LeaseState);
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Parameters in create request is fileName {0}, leaseState {1}, leaseKey {2}", createRequest.fileName, createRequest.leaseState, createRequest.leaseKey);
                        ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_SHARING_VIOLATION);
                        break;
                    }

                    #region 3.3.6.1 Handle Oplock Break Acknowledgment Timer Event

                    // TDI: TD does not state this.
                    // todo: TDI# TBD
                    if (createRequest.switchChannelType == ReplayModelSwitchChannelType.ReconnectMainChannel ||
                        createRequest.switchChannelType == ReplayModelSwitchChannelType.AlternativeChannelWithDisconnectMainChannel)
                    {
                        //TODO: More comments
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "When switchChannelType == AlternativeChannelWithDisconnectMainChannel or switchChannelType == ReconnectMainChannel (i.e. experiencing connection drop)," +
                            " the server will send oplock/lease break notification to the client, but currently test cases do not send acknowledgement request, so the oplock break acknowledgment timer always expires.");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "createRequest.switchChannelType is {0}", createRequest.switchChannelType);
                        ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                        if (Open != null &&
                            Open.Lease != null &&
                            Open.IsPersistent &&
                            createRequest.fileName == ReplayModelFileName.DefaultFileName)
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.6.1: When the oplock break acknowledgment timer expires, the server MUST scan for oplock breaks that have not been acknowledged by the client within the configured time." +
                                " It does this by enumerating all opens in the GlobalOpenTable. For each open, if Open.OplockState is Breaking and Open.OplockTimeout is earlier than the current time," +
                                " the server MUST acknowledge the oplock break to the underlying object store represented by Open.LocalOpen, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE, and set Open.OplockState to None");

                            Open.OplockLevel = ReplayModelRequestedOplockLevel.OplockLevelNone;

                            ModelHelper.Log(
                                LogType.Requirement,
                                "For each lease, if Lease.Breaking is TRUE and Lease.LeaseBreakTimeout is earlier than the current time," +
                                " the server MUST acknowledge the lease break to the underlying object store represented by the opens in Lease.LeaseOpens, and set Lease.LeaseState to NONE");

                            Open.Lease.LeaseState = ReplayModelLeaseState.LeaseStateIsNone;

                            // TDI:
                            // todo: TDI# TBD
                            if (createRequest.createGuid == ReplayModelCreateGuid.DefaultCreateGuid)
                            {
                                Open.ConnectionIsNotNull = true;
                            }
                            else
                            {
                                LastOpen = Open;
                                Open = null;
                            }
                        }
                    }

                    #endregion

                    if (createRequest.fileName == ReplayModelFileName.DefaultFileName &&
                        Open != null &&
                        Open.IsPersistent &&
                        !Open.ConnectionIsNotNull &&
                        Open.OplockLevel != ReplayModelRequestedOplockLevel.OplockLevelBatch &&
                        ((Open.OplockLevel != ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 &&
                          Open.OplockLevel != ReplayModelRequestedOplockLevel.OplockLevelLeaseV2) ||
                         (Open.Lease != null && Open.Lease.LeaseState != ReplayModelLeaseState.LeaseStateIncludeH)))
                    {
                        // Replay will always use 3.x family
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9: If Connection.Dialect belongs to the SMB 3.x dialect family and the request does not contain SMB2_CREATE_DURABLE_HANDLE_RECONNECT Create Context" +
                            " or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, the server MUST look up an existing open in the GlobalOpenTable" +
                            " where Open.FileName matches the file name in the Buffer field of the request. If an Open entry is found, and if all the following conditions are satisfied," +
                            " the server MUST fail the request with STATUS_FILE_NOT_AVAILABLE");
                        
                        ModelHelper.Log(
                            LogType.Requirement,
                            "\tOpen.IsPersistent is TRUE");
                        ModelHelper.Log(
                            LogType.Requirement,
                            "\tOpen.Connection is NULL");
                        ModelHelper.Log(
                            LogType.Requirement,
                            "\tOpen.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH");
                        ModelHelper.Log(
                            LogType.Requirement,
                            "\tOpen.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_LEASE or Open.Lease.LeaseState does not include SMB2_LEASE_HANDLE_CACHING");

                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Connection.Dialect is {0}, Open is found from GlobalOpenTable, Open.IsPersistent is {1}, Open.Connection is{2}NULL, Open.OplockLevel is {3}, Open.Lease.LeaseState is {4}",
                            createRequest.channel.Connection_NegotiateDialect, Open.IsPersistent,
                            Open.ConnectionIsNotNull ? " NOT " : " ", Open.OplockLevel, Open.Lease.LeaseState);

                        ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_FILE_NOT_AVAILABLE);
                        break;
                    }

                    Condition.IsTrue(Config == c);
                    Condition.IsTrue(Config.TreeConnect_Share_Type_Include_STYPE_CLUSTER_SOFS == c.TreeConnect_Share_Type_Include_STYPE_CLUSTER_SOFS);

                    if (Config.TreeConnect_Share_Type_Include_STYPE_CLUSTER_SOFS &&
                        createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelBatch)
                    {
                        // Replay will always use 3.x family
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9: If Connection.Dialect belongs to the SMB 3.x dialect family TreeConnect.Share.Type includes STYPE_CLUSTER_SOFS" +
                            " and the RequestedOplockLevel is SMB2_OPLOCK_LEVEL_BATCH, the server MUST set RequestedOplockLevel to SMB2_OPLOCK_LEVEL_II");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Connection.Dialect is {0}, TreeConnect.Share.Type includes STYPE_CLUSTER_SOFS and the RequestedOplockLevel is SMB2_OPLOCK_LEVEL_BATCH",
                            createRequest.channel.Connection_NegotiateDialect);

                        createRequest.requestedOplockLevel = ReplayModelRequestedOplockLevel.OplockLevelII;
                    }

                    #region Handle Lease 3.3.5.9.8 and 3.3.5.9.11

                    if (Open != null)
                    {
                        if (Open.Lease != null &&
                            (createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                             createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2) &&
                            createRequest.leaseKey == ReplayModelLeaseKey.DefaultLeaseKey &&
                            createRequest.fileName != ReplayModelFileName.DefaultFileName)
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.8: The server MUST attempt to locate a Lease by performing a lookup in the LeaseTable.LeaseList using the LeaseKey" +
                                " in the SMB2_CREATE_REQUEST_LEASE as the lookup key. If a lease is found but Lease.Filename does not match the file name for the incoming request," +
                                " the request MUST be failed with STATUS_INVALID_PARAMETER");
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.11: The server MUST attempt to locate a Lease by performing a lookup in the LeaseTable.LeaseList using the LeaseKey" +
                                " in the SMB2_CREATE_REQUEST_LEASE_V2 as the lookup key. If a lease is found but Lease.Filename does not match the file name for the incoming request," +
                                " the request MUST be failed with STATUS_INVALID_PARAMETER");

                            ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                            Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                            break;
                        }

                        // At this point, execution of create continues as described in 3.3.5.9 until the Oplock Acquisition phase.
                    }

                    #endregion

                    #region 3.3.5.9.10   Handling the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 Create Context

                    if (createRequest.modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2 || 
                        createRequest.modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2Persistent)
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9.10: Handling the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 Create Context");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "createRequest.modelDurableHandle is {0}", createRequest.modelDurableHandle);

                        bool hasLeaseCreateContext =
                            (createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                            createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2);

                        ModelHelper.Log(
                            LogType.TestInfo,
                            "createRequest.requestedOplockLevel is {0}", createRequest.requestedOplockLevel);

                        if (createRequest.modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2 &&
                            createRequest.requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelBatch &&
                            !((createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                               createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2) &&
                              createRequest.leaseState == ReplayModelLeaseState.LeaseStateIncludeH))
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: If the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of this create context," +
                                " if RequestedOplockLevel in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH, and if the create request does not include" +
                                " a SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context with a LeaseState field that includes SMB2_LEASE_HANDLE_CACHING," +
                                " the server MUST ignore this create context and skip this section");
                            ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                            break;
                        }

                        // If ClientGuid is different, alternative channel cannot be create.
                        if (Open != null)
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: The server MUST locate the Open in GlobalOpenTable where Open.CreateGuid matches the CreateGuid in the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context," +
                                " and Open.ClientGuid matches the ClientGuid of the connection that received this request");

                            openIsFound = (createRequest.createGuid == ReplayModelCreateGuid.DefaultCreateGuid && Open.CreateGuidIsNotNull);

                            ModelHelper.Log(
                                LogType.TestInfo,
                                "Open is not NULL and Open could{0}be located", openIsFound ? " " : " not ");
                        }

                        if (!openIsFound)
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: If an Open is not found, the server MUST continue the create process specified in the \"Open Execution\" Phase, and perform the following additional steps:");
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: The server MUST set Open.CreateGuid to the CreateGuid in SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2.");

                            if (Open != null)
                            {
                                LastOpen = Open;
                            }

                            Open = new ReplayOpen();

                            ModelHelper.Log(
                                LogType.Requirement,
                                "In the \"Successful Open Initialization\" phase, the server MUST set Open.IsDurable to TRUE." +
                                " The server MUST also set Open.DurableOwner to a security descriptor accessible only by the user represented by Open.Session.SecurityContext.");

                            Open.IsDurable = true;

                            ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is set to TRUE");

                            if ( createRequest.modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2Persistent &&
                                createRequest.channel.Connection_Session_TreeConnect_Share_IsCA == ReplayModelShareType.CAShare &&
                                createRequest.channel.Connection_ClientCapabilities_SupportPersistent) // assume connection dialect always 3.x
                            {
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    " If the SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the request, TreeConnect.Share.IsCA is TRUE, and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES," +
                                    " the server MUST set Open.IsPersistent to TRUE.");

                                Open.IsPersistent = true;

                                ModelHelper.Log(LogType.TestInfo, "Open.IsPersistent is set to {0}", Open.IsPersistent);
                            }

                            // 3.3.5.9.8   Handling the SMB2_CREATE_REQUEST_LEASE Create Context
                            // 3.3.5.9.11  Handling the SMB2_CREATE_REQUEST_LEASE_V2 Create Context
                            if (createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                                createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2)
                            {
                                ModelHelper.Log(
                                    LogType.TestInfo,
                                    "Requested OplockLevel is {0}, Open.leaseState is {1}", createRequest.requestedOplockLevel, createRequest.leaseState);

                                Open.Lease = new ReplayLease(createRequest.leaseState);
                            }
                        }
                        else // Open is found
                        {
                            if (createRequest.isSetReplayFlag == ReplayModelSetReplayFlag.WithoutReplayFlag)
                            {
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "3.3.5.9.10: If an Open is found and the SMB2_FLAGS_REPLAY_OPERATION bit is not set in the SMB2 header," +
                                    " the server MUST fail the request with STATUS_DUPLICATE_OBJECTID.");

                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "Open is found and SMB2_FLAGS_REPLAY_OPERATION bit is not set in the SMB2 header");

                                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                                Condition.IsTrue(status == ModelSmb2Status.STATUS_DUPLICATE_OBJECTID);
                                break;
                            }

                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: If an Open is found and the SMB2_FLAGS_REPLAY_OPERATION bit is set in the SMB2 header, the server MUST verify the following:");

                            if (!Open.IsDurable ||
                                (Open.Lease == null && hasLeaseCreateContext) || // TODO: TDI, TD does not mention Open.Lease is NULL but request has lease context
                                (Open.Lease != null && !hasLeaseCreateContext) || // Open is NOT NULL but request does not have lease context
                                (Open.Lease != null && hasLeaseCreateContext &&
                                 createRequest.leaseKey != ReplayModelLeaseKey.DefaultLeaseKey))
                            {
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "The server MUST fail the create request with STATUS_ACCESS_DENIED in the following cases:");
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "\tOpen.IsDurable is FALSE");
                                // "Open.DurableOwner is not the user represented by Open.Session.SecurityContext." not covered
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "\tIf Open.Lease is not NULL and Open.Lease.LeaseKey is not equal to the LeaseKey specified in the SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 Create Context");

                                if (!Open.IsDurable)
                                {
                                    ModelHelper.Log(
                                        LogType.TestInfo,
                                        "Open.IsDurable is FALSE");
                                }
                                else if (Open.Lease == null && hasLeaseCreateContext)
                                {
                                    ModelHelper.Log(
                                        LogType.TestInfo,
                                        "Open.Lease is NULL but request has lease context");
                                }
                                else if (Open.Lease != null && !hasLeaseCreateContext)
                                {
                                    ModelHelper.Log(
                                        LogType.TestInfo,
                                        "Open is NOT NULL but request does not have lease context");
                                }
                                else
                                {
                                    ModelHelper.Log(
                                        LogType.TestInfo,
                                        "If Open.Lease is not NULL and Open.Lease.LeaseKey is not equal to the LeaseKey specified in the request");
                                }

                                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                                Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                                break;
                            }

                            if (createRequest.fileAttributes != ReplayModelFileAttributes.DefaultFileAttributes)
                            {
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "If Open.FileAttributes does not match the FileAttributes field of the SMB2 CREATE request, the server MUST fail the request with STATUS_INVALID_PARAMETER.");

                                // TODO: TDI, TDI#?
                                //Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                                break;
                            }

                            if (createRequest.createDisposition != ReplayModelCreateDisposition.DefaultCreateDisposition)
                            {
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "If Open.CreateDisposition does not match the CreateDisposition field of the SMB2 CREATE request, the server MUST fail the request with STATUS_INVALID_PARAMETER");
                                // TDI: TDI#?
                                //Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                                //break;
                            }

                            if (Open.IsPersistent && createRequest.modelDurableHandle != ReplayModelDurableHandle.DurableHandleV2Persistent)
                            {
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "If Open.IsPersistent is TRUE and the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 Create Context, the server MUST fail the request with STATUS_INVALID_PARAMETER");
                                // TDI: TDI#?
                                //Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                                //break;
                            }
                        }

                        if (createRequest.modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2 &&
                            Open.OplockLevel != ReplayModelRequestedOplockLevel.OplockLevelBatch && (
                                !hasLeaseCreateContext || (
                                    hasLeaseCreateContext && (
                                        Open.Lease.LeaseState == ReplayModelLeaseState.LeaseStateIsNone ||
                                        Open.Lease.LeaseState == ReplayModelLeaseState.LeaseStateNotIncludeH
                                        )
                                    )
                                )
                            )
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: The server MUST skip the construction of the SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 create context if the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of the request and if neither of the following conditions are met:");
                            ModelHelper.Log(
                                LogType.Requirement,
                                "Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH");
                            ModelHelper.Log(
                                LogType.Requirement,
                                "Open.Lease.LeaseState has SMB2_LEASE_HANDLE_CACHING bit set");
                            // Handle normal open
                            break;
                        }

                        // TDI: TD does not state this. TDI#?
                        if (createRequest.modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2Persistent &&
                            createRequest.requestedOplockLevel != ReplayModelRequestedOplockLevel.OplockLevelBatch && (
                                !hasLeaseCreateContext || (
                                    hasLeaseCreateContext && (
                                        createRequest.leaseState == ReplayModelLeaseState.LeaseStateIsNone ||
                                        createRequest.leaseState == ReplayModelLeaseState.LeaseStateNotIncludeH
                                        )
                                    )
                                )
                            )
                        {
                            // Handle normal open
                            break;
                        }

                        #region Handle Lease

                        if (Open != null)
                        {
                            if ((createRequest.requestedOplockLevel ==
                                 ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                                 createRequest.requestedOplockLevel ==
                                 ReplayModelRequestedOplockLevel.OplockLevelLeaseV2) && Open.Lease != null &&
                                Open.Lease.LeaseState == ReplayModelLeaseState.LeaseStateNotIncludeH &&
                                createRequest.leaseState == ReplayModelLeaseState.LeaseStateIncludeH)
                            {
                                ModelHelper.Log(
                                    LogType.Requirement,
                                    "3.3.5.9.8 & 3.3.5.9.11: If the lease state requested is a superset of Lease.LeaseState and Lease.Breaking is FALSE," +
                                    " the server MUST request promotion of the lease state from the underlying object store to the new caching state.");

                                ModelHelper.Log(
                                    LogType.TestInfo,
                                    "Assume that Lease.Breaking is FALSE as no lease break happens");

                                Open.Lease.LeaseState = ReplayModelLeaseState.LeaseStateIncludeH;

                                ModelHelper.Log(
                                    LogType.TestInfo,
                                    "Open.Lease.LeaseState is {0}", Open.Lease.LeaseState);
                            }
                        }

                        #endregion

                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9.10: The server MUST construct the create response from Open, as specified in the \"Response Construction\" phase," +
                            " with the following additional steps, and send the response to client");

                        if (createRequest.modelDurableHandle == ReplayModelDurableHandle.DurableHandleV2 &&
                            (Open.OplockLevel == ReplayModelRequestedOplockLevel.OplockLevelBatch ||
                             (Open.Lease != null && Open.Lease.LeaseState != ReplayModelLeaseState.LeaseStateIncludeH)))
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: The server MUST skip the construction of the SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 create context " +
                                "if the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of the request and if neither of the following conditions are met:");
                            ModelHelper.Log(
                                LogType.Requirement,
                                "Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH");
                            ModelHelper.Log(
                                LogType.Requirement,
                                "Open.Lease.LeaseState has SMB2_LEASE_HANDLE_CACHING bit set");

                            Condition.IsTrue(durableHandleResponse == ReplayModelDurableHandle.NormalHandle);
                            break;
                        }

                        if (Open.IsPersistent)
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.10: If Open.IsPersistent is TRUE, the server MUST set the SMB2_DHANDLE_FLAG_PERSISTENT bit in the Flags field.");
                            ModelHelper.Log(
                                LogType.TestInfo,
                                "Open.IsPersistent is TRUE");

                            Condition.IsTrue(durableHandleResponse == ReplayModelDurableHandle.DurableHandleV2Persistent);
                            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
                        }
                        else
                        {
                            ModelHelper.Log(
                                LogType.TestInfo,
                                "Open.IsPersistent is FALSE");

                            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
                        }
                    }

                    #endregion

                } while (false);

                // Create an open
                if (status == ModelSmb2Status.STATUS_SUCCESS && Open == null)
                {
                    Open = new ReplayOpen();
                }

                if (createRequest.modelDurableHandle == ReplayModelDurableHandle.DurableHandleV1)
                {
                    if (!(createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelBatch ||
                          ((createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV1 ||
                            createRequest.requestedOplockLevel == ReplayModelRequestedOplockLevel.OplockLevelLeaseV2) && //TODO:TDI, TD does not mention skipping the section if contains SMB2_CREATE_REQUEST_LEASE_V2 without H
                           createRequest.leaseState == ReplayModelLeaseState.LeaseStateIncludeH)))
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.9.6: If the RequestedOplockLevel field in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH" +
                            " and the create request does not include an SMB2_CREATE_REQUEST_LEASE create context with a LeaseState field" +
                            " that includes the SMB2_LEASE_HANDLE_CACHING bit value, the server MUST ignore this create context and skip this section");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "RequestedOplockLevel is {0}, LeaseState is {1}", createRequest.requestedOplockLevel,
                            createRequest.leaseState);

                        Condition.IsFalse(durableHandleResponse == ReplayModelDurableHandle.DurableHandleV1);
                    }
                    else
                    {
                        if (Open != null)
                        {
                            ModelHelper.Log(
                                LogType.Requirement,
                                "3.3.5.9.6 In the \"Successful Open Initialization\" phase, the server MUST set Open.IsDurable to TRUE.");

                            Open.IsDurable = true;

                            ModelHelper.Log(
                                LogType.TestInfo,
                                "Open.IsDurable is set to TRUE");
                        }
                    }
                }
            }

            // 3.3.4.1 Sending Any Outgoing Message
            PostCheckChannelSequence(ReplayModelRequestCommand.Create, createRequest.channel.Connection_NegotiateDialect,
                createRequest.channelSequence);

            Condition.IfThen(status != ModelSmb2Status.STATUS_SUCCESS, durableHandleResponse == ReplayModelDurableHandle.NormalHandle);
        }

        #endregion

        #region Test READ/WRITE/SET_INFO/IOCTL
        /// <summary>
        /// Prepare the first file operation.
        /// </summary>
        /// <param name="maxSmbVersionClientSupported">Max version that the client supports.</param>
        /// <param name="requestCommand">Request command.</param>
        [Rule]
        public static void PrepareFileOperation(
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelRequestCommand requestCommand)
        {
            // Condition.IsTrue(maxSmbVersionClientSupported != ModelDialectRevision.Smb302);
            Condition.IsTrue(State == ModelState.Initialized);
            Condition.IsNull(MainChannel);
            Condition.IsNull(Open);
            Condition.IsTrue(ModelRequestCommand == ReplayModelRequestCommand.NoRequest);
            Condition.IsTrue(requestCommand != ReplayModelRequestCommand.Create && requestCommand != ReplayModelRequestCommand.NoRequest);

            MainChannel =
                new ModelReplayChannel(ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported,
                    Config.MaxSmbVersionSupported));

            Open = new ReplayOpen();
            ModelRequestCommand = requestCommand;

            State = ModelState.Connected;
        }

        /// <summary>
        /// File operation request.
        /// </summary>
        /// <param name="switchChannelType">Indicates the type of switching channel.</param>
        /// <param name="maxSmbVersionClientSupported">Max version that the client supports.</param>
        /// <param name="requestCommand">Request command.</param>
        /// <param name="channelSequence">Indicates the type of session channel sequence.</param>
        /// <param name="isSetReplayFlag">Indicates whether a replay flag is set or not.</param>
        /// <param name="requestCommandParameters">Indicates whether to operate the file with the same parameters or not.</param>
        [Rule]
        public static void FileOperationRequest(
            ReplayModelSwitchChannelType switchChannelType,
            ModelDialectRevision maxSmbVersionClientSupported,
            ReplayModelRequestCommand requestCommand,
            ReplayModelChannelSequenceType channelSequence,
            ReplayModelSetReplayFlag isSetReplayFlag,
            ReplayModelRequestCommandParameters requestCommandParameters)
        {
            Condition.IsNull(Request);
            Condition.IfThen(ModelRequestCommand != ReplayModelRequestCommand.NoRequest, ModelRequestCommand == requestCommand);
            Condition.IsTrue(requestCommand != ReplayModelRequestCommand.Create && requestCommand != ReplayModelRequestCommand.NoRequest);
            Condition.IsTrue(switchChannelType != ReplayModelSwitchChannelType.ReconnectMainChannel);

            Combination.NWise(2, switchChannelType, maxSmbVersionClientSupported, requestCommand, channelSequence, isSetReplayFlag, 
                requestCommandParameters);
            
            ModelReplayChannel channel = null;
            if (State == ModelState.Initialized)
            {
                Condition.IsTrue(switchChannelType == ReplayModelSwitchChannelType.MainChannel);
                Condition.IsTrue(requestCommandParameters == ReplayModelRequestCommandParameters.DefaultParameters);

                MainChannel =
                    new ModelReplayChannel(ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported,
                        Config.MaxSmbVersionSupported));

                Open = new ReplayOpen();
                channel = MainChannel;
            }
            else
            {
                Condition.IsNotNull(MainChannel);
                Condition.IsNotNull(Open);
                Condition.IsTrue(maxSmbVersionClientSupported == ModelUtility.GetModelDialectRevision(MainChannel.Connection_NegotiateDialect));

                if (switchChannelType == ReplayModelSwitchChannelType.MainChannel)
                {
                    channel = MainChannel;
                }
                else
                {
                    AlternativeChannel = new ModelReplayChannel(MainChannel.Connection_NegotiateDialect);
                    channel = AlternativeChannel;
                }
            }

            ModelReplayFileOperationRequest operationRequest = new ModelReplayFileOperationRequest(channel, switchChannelType, 
                maxSmbVersionClientSupported, requestCommand, channelSequence, isSetReplayFlag, requestCommandParameters);

            Request = operationRequest;

            State = ModelState.Connected;
        }

        /// <summary>
        /// File operation response.
        /// </summary>
        /// <param name="status">The status in the response.</param>
        /// <param name="c">Config.</param>
        [Rule]
        public static void FileOperationResponse(ModelSmb2Status status, ReplayServerConfig c)
        {
            Condition.IsTrue(State == ModelState.Connected);

            Condition.IsTrue(Config == c);

            Condition.IsNotNull(Request);

            ModelReplayFileOperationRequest operationRequest = ModelHelper.RetrieveOutstandingRequest<ModelReplayFileOperationRequest>(ref Request);

            // 3.3.5.2.10 Verifying the Channel Sequence Number
            PreCheckChannelSequence(operationRequest.requestCommand,
                operationRequest.channel.Connection_NegotiateDialect,
                ref operationRequest.channelSequence,
                operationRequest.isSetReplayFlag == ReplayModelSetReplayFlag.WithReplayFlag,
                status);

            // 3.3.4.1 Sending Any Outgoing Message
            PostCheckChannelSequence(operationRequest.requestCommand,
                operationRequest.channel.Connection_NegotiateDialect,
                operationRequest.channelSequence);
        }
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Handle session channel sequence after receiving request.
        /// 3.3.5.2.10   Verifying the Channel Sequence Number
        /// </summary>
        /// <param name="requestCommand">Request command.</param>
        /// <param name="negotiateDialect">Negotiated dialect.</param>
        /// <param name="channelSequence">Session channel sequence.</param>
        /// <param name="isSetReplayFlag">Indicates whether a replay flag is set or not.</param>
        /// <param name="status">The status that server returns.</param>
        /// <returns></returns>
        private static bool PreCheckChannelSequence(
            ReplayModelRequestCommand requestCommand,
            DialectRevision negotiateDialect,
            ref ReplayModelChannelSequenceType channelSequence,
            bool isSetReplayFlag,
            ModelSmb2Status status)
        {
            if (negotiateDialect == DialectRevision.Smb2002 || negotiateDialect == DialectRevision.Smb21 ||
                requestCommand == ReplayModelRequestCommand.Create)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.2.10: If Connection.Dialect is equal to \"2.002\" or \"2.100\", or the command request does not include FileId," +
                    " this section MUST be skipped");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.Dialect is {0}. The request command is {1}", negotiateDialect, requestCommand);
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                return true;
            }

            if (!isSetReplayFlag)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.2.10: If the SMB2_FLAGS_REPLAY_OPERATION bit is not set in the Flags field of the SMB2 Header:");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);

                if (channelSequence == ReplayModelChannelSequenceType.DefaultChannelSequence)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "If ChannelSequence in the SMB2 Header is equal to Open.ChannelSequence, the server MUST increment Open.OutstandingRequestCount by 1.");

                    Open.OutstandingRequestCount += 1;

                    ModelHelper.Log(
                        LogType.TestInfo,
                        "Open.OutstandingRequestCount is {0} after increment", Open.OutstandingRequestCount);
                }
                else if (channelSequence == ReplayModelChannelSequenceType.ChannelSequenceIncrementOne ||
                          channelSequence == ReplayModelChannelSequenceType.ChannelSequenceBoundaryValid)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "Otherwise, if the unsigned difference using 16-bit arithmetic between ChannelSequence and Open.ChannelSequence is less than or equal to 0x7FFF," +
                        " the server MUST increment Open.OutstandingPreRequestCount by Open.OutstandingRequestCount, and MUST set Open.OutstandingRequestCount to 1." +
                        " The server MUST set Open.ChannelSequence to ChannelSequence in the SMB2 Header");
                    ModelHelper.Log(
                        LogType.TestInfo,
                        "ChannelSequence is {0}", channelSequence);

                    Open.OutstandingPreRequestCount += Open.OutstandingRequestCount;
                    Open.OutstandingRequestCount = 1;
                    channelSequence = ReplayModelChannelSequenceType.DefaultChannelSequence;

                    ModelHelper.Log(
                        LogType.TestInfo,
                        "Open.OutstandingPreRequestCount is {0} after increment by Open.OutstandingRequestCount with value {1}",
                        Open.OutstandingPreRequestCount, Open.OutstandingRequestCount);
                }
                else
                {
                    if (requestCommand == ReplayModelRequestCommand.Write ||
                        requestCommand == ReplayModelRequestCommand.SetInfo ||
                        requestCommand == ReplayModelRequestCommand.IoCtl)
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "Otherwise, the server MUST fail SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "requestCommand is {0}", requestCommand);

                        Condition.IsTrue(status == ModelSmb2Status.STATUS_FILE_NOT_AVAILABLE);
                        return false;
                    }
                }
            }
            else
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.2.10: If the SMB2_FLAGS_REPLAY_OPERATION bit is set in the Flags field of the SMB2 Header:");

                if (channelSequence == ReplayModelChannelSequenceType.DefaultChannelSequence)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "If ChannelSequence in the SMB2 Header is equal to Open.ChannelSequence and the following:");

                    if (Open.OutstandingPreRequestCount == 0)
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "If ChannelSequence in the SMB2 Header is equal to Open.ChannelSequence and Open.OutstandingPreRequestCount is equal to zero, the server MUST increment Open.OutstandingRequestCount by 1");

                        Open.OutstandingRequestCount += 1;

                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Open.OutstandingRequestCount is {0} after increment", Open.OutstandingRequestCount);
                    }
                }
                else if ((channelSequence == ReplayModelChannelSequenceType.ChannelSequenceIncrementOne ||
                          channelSequence == ReplayModelChannelSequenceType.ChannelSequenceBoundaryValid) &&
                         Open.OutstandingPreRequestCount == 0)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "Otherwise, if the unsigned difference using 16-bit arithmetic between ChannelSequence and Open.ChannelSequence is less than or equal to 0x7FFF and Open.OutstandingPreRequestCount is equal to zero," +
                        " the server MUST increment Open.OutstandingPreRequestCount by Open.OutstandingRequestCount and MUST set Open.OutstandingRequestCount to 1." +
                        " The server MUST set Open.ChannelSequence to ChannelSequence in the SMB2 Header.");

                    Open.OutstandingPreRequestCount += Open.OutstandingRequestCount;
                    Open.OutstandingRequestCount = 1;
                    channelSequence = ReplayModelChannelSequenceType.DefaultChannelSequence;

                    ModelHelper.Log(
                        LogType.TestInfo,
                        "Open.OutstandingPreRequestCount is {0} after increment by Open.OutstandingRequestCount with value {1}",
                        Open.OutstandingPreRequestCount, Open.OutstandingRequestCount);
                }
                else
                {
                    if (requestCommand == ReplayModelRequestCommand.Write ||
                        requestCommand == ReplayModelRequestCommand.SetInfo ||
                        requestCommand == ReplayModelRequestCommand.IoCtl)
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "Otherwise, the server MUST fail SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "requestCommand is {0}", requestCommand);
                        ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_FILE_NOT_AVAILABLE);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 3.3.4.1 Sending Any Outgoing Message
        /// </summary>
        /// <param name="requestCommand">Request command.</param>
        /// <param name="negotiateDialect">Negotiated dialect.</param>
        /// <param name="channelSequence">Session channel sequence.</param>
        private static void PostCheckChannelSequence(
            ReplayModelRequestCommand requestCommand,
            DialectRevision negotiateDialect,
            ReplayModelChannelSequenceType channelSequence)
        {
            //TODO: TD does not mention this
            if (requestCommand == ReplayModelRequestCommand.Create)
            {
                ModelHelper.Log(
                    LogType.TestInfo,
                    "If the command request does not include FileId, this section MUST be skipped");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.Dialect is {0}, request command is {1}", negotiateDialect, requestCommand);

                return;
            }

            if (channelSequence == ReplayModelChannelSequenceType.DefaultChannelSequence)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.4.1: For the command requests which include FileId, if Connection.Dialect belongs to the SMB 3.x dialect family and ChannelSequence is equal to Open.ChannelSequence," +
                    " the server MUST decrement Open.OutstandingRequestCount by 1. ");

                Open.OutstandingRequestCount -= 1;

                ModelHelper.Log(
                    LogType.TestInfo,
                    "Open.OutstandingRequestCount is {0} after decrement", Open.OutstandingRequestCount);
            }
            else
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.4.1: Otherwise, the server MUST decrement Open.OutstandingPreRequestCount by 1. ");

                Open.OutstandingPreRequestCount -= 1;

                ModelHelper.Log(
                    LogType.TestInfo,
                    "Open.OutstandingPreRequestCount is {0} after decrement", Open.OutstandingPreRequestCount);
            }
        }

        /// <summary>
        /// Get channel by switchChannelType.
        /// </summary>
        /// <param name="switchChannelType">The type of switching channel.</param>
        /// <returns></returns>
        private static ModelReplayChannel GetChannel(ReplayModelSwitchChannelType switchChannelType)
        {
            ModelReplayChannel channel;
            switch (switchChannelType)
            {
                case ReplayModelSwitchChannelType.AlternativeChannelWithMainChannel:
                    channel = AlternativeChannel;
                    break;
                case ReplayModelSwitchChannelType.ReconnectMainChannel:
                    channel = MainChannel;
                    AlternativeChannel = null;
                    break;
                case ReplayModelSwitchChannelType.AlternativeChannelWithDisconnectMainChannel:
                    channel = AlternativeChannel;
                    MainChannel = null;
                    break;
                case ReplayModelSwitchChannelType.MainChannelWithAlternativeChannel:
                    channel = MainChannel;
                    break;
                default: // ReplayModelSwitchChannelType.MainChannel
                    channel = MainChannel;
                    break;
            }
            return channel;
        }
        #endregion
    }
}
