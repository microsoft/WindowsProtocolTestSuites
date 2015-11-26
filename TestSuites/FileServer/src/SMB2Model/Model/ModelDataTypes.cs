// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreateClose;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreditMgmt;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Encryption;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Handle;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Leasing;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Replay;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.ResilientHandle;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.SessionMgmt;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Signing;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.TreeMgmt;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model
{
    /// <summary>
    /// The state enumeration for SMB2 model.
    /// </summary>
    public enum ModelState
    {
        /// <summary>
        /// Model is not initialized.
        /// </summary>
        Uninitialized,

        /// <summary>
        /// Model is initialized.
        /// </summary>
        Initialized,

        /// <summary>
        /// The model is ready for test.
        /// </summary>
        Connected,

        /// <summary>
        /// The connection has been disconnected.
        /// </summary>
        Disconnected,
    }

    public enum ModelSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        None,

        /// <summary>
        /// 
        /// </summary>
        InProgress,

        /// <summary>
        /// 
        /// </summary>
        Valid,
    }

    /// <summary>
    /// Abstract a connection object in the model
    /// </summary>
    public class ModelConnection
    {
        /// <summary>
        /// State of the connection
        /// </summary>
        public ModelState ConnectionState { get; set; }

        /// <summary>
        /// Dialect selected for the connection after negotiation
        /// </summary>
        public DialectRevision NegotiateDialect { get; protected set; }

        /// <summary>
        /// Indicates the session
        /// </summary>
        public ModelSession Session { get; set; }

        /// <summary>
        /// Request sent through the connection
        /// </summary>
        public ModelSMB2Request Request { get; set; }

        /// <summary>
        /// A Boolean that, if set, indicates that authentication to a non-anonymous principal has not yet been successfully performed on this connection.
        /// </summary>
        public bool ConstrainedConnection { get; set; }

        public ModelConnection(DialectRevision dialect)
        {
            this.ConnectionState = ModelState.Initialized;
            this.NegotiateDialect = dialect;
            this.Session = null;
            this.Request = null;
            this.ConstrainedConnection = false;
        }

        public override string ToString()
        {
            return string.Format("({0}: {1}, {2}: {3}, {4}: {5})",
                "ConnectionState", ConnectionState, "NegotiateDialect", NegotiateDialect, "Session", Session);
        }
    }

    /// <summary>
    /// Abstract a session object in the model
    /// </summary>
    public class ModelSession
    {
        public ModelSessionState State { get; set; }

        public DialectRevision Dialect { get; set; }

        public ModelSessionId SessionId { get; set; }

        public ModelSession()
        {
        }

        public override string ToString()
        {
            return string.Format("({0}: {1}, {2}: {3}, {4}: {5})",
                "State", State, "Dialect", Dialect, "SessionId", SessionId);
        }
    }

    public class ModelOpen
    {
        public bool IsDurable { get; set; }
        public bool IsResilient { get; set; }
        public ModelUser DurableOwner { get; set; }

        public ModelOpen(bool isDurable)
        {
            IsDurable = isDurable;
        }

        public override string ToString()
        {
            return string.Format("Open by {0}{1}{2}{3}{4}",
                DurableOwner,
                IsResilient || IsDurable? " with " : "",
                IsDurable? "Durable" : "",
                IsResilient && IsDurable? " and " : "",
                IsResilient? "Resilient" : "");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ModelOplockOpen
    {
        public OplockLevel_Values OplockLevel { get; set; }

        public OplockState OplockState { get; set; }

        public ModelOplockOpen()
        {
        }

    }

    #region Abstract SMB2 Request

    /// <summary>
    /// The class of the ModelSMB2Request includes all SMB2 request methods.
    /// </summary>
    public abstract class ModelSMB2Request
    {
        /// <summary>
        /// A value that identify a message request and response uniquely across
        /// all messages sent on the same SMB2 Protocol transport connection.
        /// </summary>
        public long messageID;

        /// <summary>
        /// Indicates that whether the command client wants to cancel is an asynchronously processed command.
        /// </summary>
        public bool isAsync;

        /// <summary>
        /// Indicates that whether cancel an asynchronously processed command.
        /// </summary>
        public bool isCanceling;

        /// <summary>
        /// The request message of the SMB2.
        /// </summary>
        /// <param name="messageID">A value that identify a message request uniquely across
        /// all messages sent on the same SMB2 Protocol transport connection.</param>
        protected ModelSMB2Request(long messageID)
        {
            this.messageID = messageID;
        }
    }

    #endregion

    #region SMB2 Requests

    #region Negotiate
    /// <summary>
    /// This class is used to Negotiate request,
    /// Negotiate request is used by the client to notify the server what dialects of the SMB Protocol the client understands.
    /// </summary>
    public class ModelComNegotiateRequest : ModelSMB2Request
    {
        public Sequence<string> Dialects;

        /// <summary>
        /// This method is used to send a negotiate request.
        /// </summary>
        /// <param name="messageID"> A value that identify a message request and response uniquely across
        /// all messages sent on the same SMB Protocol transport connection. </param>
        /// <param name="dialects"> Dialects request contains. </param>
        public ModelComNegotiateRequest(Sequence<string> dialects)
            : base(0)
        {
            this.Dialects = dialects;
        }
    }

    /// <summary>
    /// This class is used to Negotiate request,
    /// Negotiate request is used by the client to notify the server what dialects of the SMB Protocol the client understands.
    /// </summary>
    public class NegotiateRequest : ModelSMB2Request
    {
        public Sequence<DialectRevision> Dialects;

        /// <summary>
        /// This method is used to send a negotiate request.
        /// </summary>
        /// <param name="messageID"> A value that identify a message request and response uniquely across
        /// all messages sent on the same SMB Protocol transport connection. </param>
        /// <param name="dialects"> Dialects request contains. </param>
        public NegotiateRequest(Sequence<DialectRevision> dialects)
            : base(0)
        {
            this.Dialects = dialects;
        }
    }
    #endregion

    #region Leasing
    /// <summary>
    /// This class is used to Create Lease request,
    /// Create Lease request is used by the client to request either creation of or access to a file with lease.
    /// </summary>
    public class ModelCreateLeaseRequest : ModelSMB2Request
    {
        public CreateOptions_Values CreateOptions;
        public RequestedOplockLevel_Values RequestedOplockLevel;
        public LeaseContextType ContextType;
        public ModelCreateContextRequest LeaseContext;

        public ModelCreateLeaseRequest(
            CreateOptions_Values createOptions,
            RequestedOplockLevel_Values requestedOplockLevel_Values,
            ModelCreateContextRequest leaseContext,
            LeaseContextType leaseContextType)
            : base(0)
        {
            this.CreateOptions = createOptions;
            this.RequestedOplockLevel = requestedOplockLevel_Values;
            this.LeaseContext = leaseContext;
            this.ContextType = leaseContextType;
        }
    }

    /// <summary>
    /// This class is used to Lease Break Acknowledgment request,
    /// Lease Break Acknowledgment request is used by the client in response to an SMB2 Lease Break Notification packet sent by the server.
    /// </summary>
    public class ModelLeaseBreakAckRequest : ModelSMB2Request
    {
        public ModelLeaseKeyType modelLeaseKeyType;

        public uint LeaseState;

        public ModelLeaseBreakAckRequest(
            ModelLeaseKeyType modelLeaseKeyType,
            uint leaseState)
            : base(0)
        {
            this.modelLeaseKeyType = modelLeaseKeyType;
            this.LeaseState = leaseState;
        }
    }

    /// <summary>
    /// This class is used to request a file operation.
    /// </summary>
    public class ModelFileOperationRequest : ModelSMB2Request
    {
        public FileOperation Operation;

        public OperatorType OptorType;

        public ModelDialectRevision Dialect;

        public ModelFileOperationRequest(
            FileOperation operation,
            OperatorType operatorType,
            ModelDialectRevision dialect)
            : base(0)
        {
            this.Operation = operation;
            this.OptorType = operatorType;
            this.Dialect = dialect;
        }
    }

    #endregion

    #region Credit Management
    /// <summary>
    /// This class is used to request a credit operation
    /// </summary>
    public class ModelCreditOperationRequest : ModelSMB2Request
    {
        public ModelMidType midType;
        public ModelCreditCharge creditCharge;
        public ModelCreditRequestNum creditRequestNum;
        public ModelPayloadSize payloadSize;
        public ModelPayloadType payloadType;

        public ModelCreditOperationRequest(
            ModelMidType midType,
            ModelCreditCharge creditCharge,
            ModelCreditRequestNum creditRequestNum,
            ModelPayloadSize payloadSize,
            ModelPayloadType payloadType)
            : base(0)
        {
            this.midType = midType;
            this.creditCharge = creditCharge;
            this.creditRequestNum = creditRequestNum;
            this.payloadSize = payloadSize;
            this.payloadType = payloadType;
        }
    }
    #endregion

    #region Encryption

    /// <summary>
    /// This class is used to request a treeconnect operation
    /// </summary>
    public class ModelTreeConnectRequest : ModelSMB2Request
    {
        public ConnectToShareType connectToShareType;
        public ModelRequestType modelRequestType;

        public ModelTreeConnectRequest(
            ConnectToShareType connectToShareType,
            ModelRequestType modelRequestType)
            : base(0)
        {
            this.connectToShareType = connectToShareType;
            this.modelRequestType = modelRequestType;
        }
    }

    /// <summary>
    /// This class is used to request a create file operation
    /// </summary>
    public class ModelFileOperationVerifyEncryptionRequest : ModelSMB2Request
    {
        public ModelRequestType modelRequestType;

        public ModelFileOperationVerifyEncryptionRequest(
            ModelRequestType modelRequestType)
            : base(0)
        {
            this.modelRequestType = modelRequestType;
        }
    }
    #endregion

    #region Session Management

    public class ModelSessionSetupRequest : ModelSMB2Request
    {
        public ModelConnectionId connectionId;
        public ModelSessionId sessionId;
        public ModelSessionId previousSessionId;
        public bool isSigned;
        public ModelFlags flags;
        public ModelUser user;

        public ModelSessionSetupRequest(
            ModelConnectionId connectionId,
            ModelSessionId sessionId,
            ModelSessionId previousSessionId,
            bool isSigned,
            ModelFlags flags,
            ModelUser user
            )
            : base(0)
        {
            this.connectionId = connectionId;
            this.sessionId = sessionId;
            this.previousSessionId = previousSessionId;
            this.isSigned = isSigned;
            this.flags = flags;
            this.user = user;
        }
    }


    public class ModelLogOffRequest : ModelSMB2Request
    {
        public ModelConnectionId connectionId;
        public ModelSessionId sessionId;

        public ModelLogOffRequest(ModelConnectionId connectionId, ModelSessionId sessionId)
            : base(0)
        {
            this.connectionId = connectionId;
            this.sessionId = sessionId;
        }
    }

    #endregion

    #region TreeMgmt

    public class ModelTreeMgmtTreeConnectRequest : ModelSMB2Request
    {
        public ModelSharePath sharePath;

        public ModelTreeMgmtTreeConnectRequest(ModelSharePath sharePath)
            : base(0)
        {
            this.sharePath = sharePath;
        }
    }

    public class ModelTreeMgmtTreeDisconnectRequest : ModelSMB2Request
    {
        public ModelTreeId treeId;

        public ModelTreeMgmtTreeDisconnectRequest(ModelTreeId treeId)
            : base(0)
        {
            this.treeId = treeId;
        }
    }

    #endregion

    #region Resilient Handle

    public class ModelResiliencyRequest : ModelSMB2Request
    {
        public IoCtlInputCount InputCount { get; set; }
        public ResilientTimeout Timeout { get; set; }

        public ModelResiliencyRequest(
            IoCtlInputCount inputCount,
            ResilientTimeout timeout)
            : base(0)
        {
            InputCount = inputCount;
            Timeout = timeout;
        }
    }

    public class ModelReEstablishResilientOpenRequest : ModelSMB2Request
    {
        public ModelUser User { get; set; }

        public ModelReEstablishResilientOpenRequest(ModelUser user)
            : base(0)
        {
            User = user;
        }
    }

    #region Oplock
    public class ModelRequestOplockAndTriggerBreakRequest : ModelSMB2Request
    {
        public RequestedOplockLevel_Values RequestedOplockLevel;

        public ModelRequestOplockAndTriggerBreakRequest(RequestedOplockLevel_Values requestedOplockLevel)
            : base(0)
        {
            this.RequestedOplockLevel = requestedOplockLevel;
        }
    }

    public class ModelOplockBreakAcknowledgementRequest : ModelSMB2Request
    {
        public bool VolatilePortionFound;

        public bool PersistentMatchesDurableFileId;

        public OplockLevel_Values OplockLevel;

        public ModelOplockBreakAcknowledgementRequest(bool volatilePortionFound, bool persistentMatchesDurableFileId, OplockLevel_Values oplockLevel)
            :base(0)
        {
            this.VolatilePortionFound = volatilePortionFound;
            this.PersistentMatchesDurableFileId = persistentMatchesDurableFileId;
            this.OplockLevel = oplockLevel;
        }

    }

    #endregion

    #region Handle
    /// <summary>
    /// This class is used to request an open file operation
    /// </summary>
    public class ModelOpenFileRequest : ModelSMB2Request
    {
        public DurableV1RequestContext durableV1RequestContext;
        public DurableV2RequestContext durableV2RequestContext;
        public DurableV1ReconnectContext durableV1ReconnectContext;
        public DurableV2ReconnectContext durableV2ReconnectContext;
        public OplockLeaseType oplockLeaseType;
        public bool isSameLeaseKey;
        public bool isSameClient;
        public bool isSameCreateGuid;

        public ModelOpenFileRequest(
            DurableV1RequestContext durableV1RequestContext,
            DurableV2RequestContext durableV2RequestContext,
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext,
            OplockLeaseType oplockLeaseType,
            bool isSameLeaseKey,
            bool isSameClient,
            bool isSameCreateGuid)
            : base(0)
        {
            this.durableV1RequestContext = durableV1RequestContext;
            this.durableV2RequestContext = durableV2RequestContext;
            this.durableV1ReconnectContext = durableV1ReconnectContext;
            this.durableV2ReconnectContext = durableV2ReconnectContext;
            this.oplockLeaseType = oplockLeaseType;
            this.isSameLeaseKey = isSameLeaseKey;
            this.isSameClient = isSameClient;
            this.isSameCreateGuid = isSameCreateGuid;
        }
    }
    #endregion

    #endregion

    #region CreateClose
    public class ModelCreateRequest: ModelSMB2Request
    {
        public CreateFileNameType NameType;
        public CreateOptionsFileOpenReparsePointType FileOpenReparsePointType;
        public CreateOptionsFileDeleteOnCloseType FileDeleteOnCloseType;
        public CreateContextType ContextType;
        public ImpersonationLevelType ImpersonationType;

        public ModelCreateRequest(
            CreateFileNameType nameType,
            CreateOptionsFileOpenReparsePointType fileOpenReparsePointType,
            CreateOptionsFileDeleteOnCloseType fileDeleteOnCloseType,
            CreateContextType contextType,
            ImpersonationLevelType impersonationType):base(0)
        {
            NameType = nameType;
            FileOpenReparsePointType = fileOpenReparsePointType;
            FileDeleteOnCloseType = fileDeleteOnCloseType;
            ContextType = contextType;
            ImpersonationType = impersonationType;
        }
    }

    public class ModelCloseRequest: ModelSMB2Request
    {
        public CloseFlagType CloseType;
        public FileIdVolatileType VolatileType;
        public FileIdPersistentType PersistentType;

        public ModelCloseRequest(
            CloseFlagType closeType,                        
            FileIdVolatileType volatileType,                
            FileIdPersistentType persistentType):base(0)
        {
            CloseType = closeType;
            VolatileType = volatileType;
            PersistentType = persistentType;
        }
    }
    #endregion

    #region Signing
    /// <summary>
    /// This class is used to request in SigningModel
    /// </summary>
    public class SigningModelRequest : ModelSMB2Request
    {
        public SigningFlagType signingFlagType;
        public SigningEnabledType signingEnabledType;
        public SigningRequiredType signingRequiredType;

        public SigningModelRequest(
            SigningFlagType signingFlagType)
            : base(0)
        {
            this.signingFlagType = signingFlagType;
        }

        public SigningModelRequest(
            SigningFlagType signingFlagType,
            SigningEnabledType signingEnabledType,
            SigningRequiredType signingRequiredType)
            : base(0)
        {
            this.signingFlagType = signingFlagType;
            this.signingEnabledType = signingEnabledType;
            this.signingRequiredType = signingRequiredType;
        }
    }
    #endregion

    #region Replay
    public class ModelReplayChannel
    {
        /// <summary>
        /// The dialect revision after negotiation.
        /// </summary>
        public DialectRevision Connection_NegotiateDialect;

        /// <summary>
        /// The type of connected share.
        /// </summary>
        public ReplayModelShareType Connection_Session_TreeConnect_Share_IsCA;

        public bool Connection_ClientCapabilities_SupportPersistent;

        public ModelReplayChannel(DialectRevision Connection_NegotiateDialect)
        {
            this.Connection_NegotiateDialect = Connection_NegotiateDialect;
        }

        public ModelReplayChannel(DialectRevision Connection_NegotiateDialect,
            ReplayModelShareType Connection_Session_TreeConnect_Share_IsCA,
            bool Connection_ClientCapabilities_SupportPersistent)
        {
            this.Connection_NegotiateDialect = Connection_NegotiateDialect;
            this.Connection_Session_TreeConnect_Share_IsCA = Connection_Session_TreeConnect_Share_IsCA;
            this.Connection_ClientCapabilities_SupportPersistent = Connection_ClientCapabilities_SupportPersistent;
        }

    }

    public class ModelReplayCreateRequest : ModelSMB2Request
    {
        public ModelReplayChannel channel;
        public ReplayModelSwitchChannelType switchChannelType;
        public ReplayModelChannelSequenceType channelSequence;
        public ReplayModelDurableHandle modelDurableHandle;
        public ReplayModelRequestedOplockLevel requestedOplockLevel;
        public ReplayModelFileName fileName;
        public ReplayModelCreateGuid createGuid;
        public ReplayModelFileAttributes fileAttributes;
        public ReplayModelCreateDisposition createDisposition;
        public ReplayModelLeaseState leaseState;
        public ReplayModelSetReplayFlag isSetReplayFlag;
        public ReplayModelLeaseKey leaseKey;

        public ModelReplayCreateRequest(ModelReplayChannel channel,
            ReplayModelSwitchChannelType switchChannelType,
            ReplayModelChannelSequenceType channelSequence,
            ReplayModelDurableHandle modelDurableHandle,
            ReplayModelRequestedOplockLevel requestedOplockLevel,
            ReplayModelFileName fileName,
            ReplayModelCreateGuid createGuid,
            ReplayModelFileAttributes fileAttributes,
            ReplayModelCreateDisposition createDisposition,
            ReplayModelLeaseState leaseState,
            ReplayModelSetReplayFlag isSetReplayFlag,
            ReplayModelLeaseKey leaseKey)
            : base(0)
        {
            this.channel = channel;
            this.switchChannelType = switchChannelType;
            this.channelSequence = channelSequence;
            this.modelDurableHandle = modelDurableHandle;
            this.requestedOplockLevel = requestedOplockLevel;
            this.fileName = fileName;
            this.createGuid = createGuid;
            this.fileAttributes = fileAttributes;
            this.createDisposition = createDisposition;
            this.leaseState = leaseState;
            this.isSetReplayFlag = isSetReplayFlag;
            this.leaseKey = leaseKey;
        }
    }

    public class ModelReplayFileOperationRequest : ModelSMB2Request
    {
        public ModelReplayChannel channel;
        public ReplayModelSwitchChannelType switchChannelType;
        public ModelDialectRevision maxSmbVersionClientSupported;
        public ReplayModelRequestCommand requestCommand;
        public ReplayModelChannelSequenceType channelSequence;
        public ReplayModelSetReplayFlag isSetReplayFlag;
        public ReplayModelRequestCommandParameters requestCommandParameters;

        public ModelReplayFileOperationRequest(ModelReplayChannel channel,
            ReplayModelSwitchChannelType switchChannelType,
            ModelDialectRevision maxSmbVersionClientSupported, 
            ReplayModelRequestCommand requestCommand,
            ReplayModelChannelSequenceType channelSequence,
            ReplayModelSetReplayFlag isSetReplayFlag,
            ReplayModelRequestCommandParameters requestCommandParameters
            )
            : base(0)
        {
            this.channel = channel;
            this.switchChannelType = switchChannelType;
            this.requestCommand = requestCommand;
            this.channelSequence = channelSequence;
            this.isSetReplayFlag = isSetReplayFlag;
            this.requestCommandParameters = requestCommandParameters;
        }
    }

    #endregion

    #endregion
}
