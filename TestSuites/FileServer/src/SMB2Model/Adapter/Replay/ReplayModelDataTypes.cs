// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Replay
{
    /// <summary>
    /// The configuration of server for replay
    /// </summary>
    public class ReplayServerConfig : CompoundValue
    {
        /// <summary>
        /// SMB 2.002 or SMB 2.1 or SMB 3.0 or SMB 3.02
        /// </summary>
        public ModelDialectRevision MaxSmbVersionSupported;
        
        /// <summary>
        /// Indicates whether the server supports persistent handles or not.
        /// </summary>
        public bool IsPersistentHandleSupported;

        public bool TreeConnect_Share_Type_Include_STYPE_CLUSTER_SOFS;

        public bool IsLeasingSupported;

        public bool IsDirectoryLeasingSupported;

        public Platform Platform;

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("MaxSmbVersionSupported: {0}{1}", Enum.GetName(typeof(ModelDialectRevision), MaxSmbVersionSupported), System.Environment.NewLine);
            s.AppendFormat("IsPersistentHandleSupported: {0}{1}", IsPersistentHandleSupported, System.Environment.NewLine);
            s.AppendFormat("TreeConnect_Share_Type_Include_STYPE_CLUSTER_SOFS: {0}{1}", TreeConnect_Share_Type_Include_STYPE_CLUSTER_SOFS, System.Environment.NewLine);
            s.AppendFormat("IsLeasingSupported: {0}{1}", IsLeasingSupported, System.Environment.NewLine);
            s.AppendFormat("IsDirectoryLeasingSupported: {0}{1}", IsDirectoryLeasingSupported, System.Environment.NewLine);
            s.AppendFormat("Platform: {0}{1}", Enum.GetName(typeof(Platform), Platform), System.Environment.NewLine);

            return s.ToString();
        }
    }

    /// <summary>
    /// An enumeration type for indicating which request message is to be replayed.
    /// </summary>
    public enum ReplayModelRequestCommand
    {
        NoRequest,
        Create,
        Read,
        Write,
        SetInfo,
        IoCtl
    }

    /// <summary>
    /// An enumeration type for indicating which version is to be used for durable handle.
    /// </summary>
    public enum ReplayModelDurableHandle
    {
        /// <summary>
        /// Not a durable handle
        /// </summary>
        NormalHandle,

        /// <summary>
        /// Durable handle v1 request with SMB2_CREATE_DURABLE_HANDLE_REQUEST context
        /// </summary>
        DurableHandleV1,

        /// <summary>
        /// Durable handle v2 request with SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context
        /// </summary>
        DurableHandleV2,

        /// <summary>
        /// Persistent handle request with SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context with SMB2_DHANDLE_FLAG_PERSISTENT flag set
        /// </summary>
        DurableHandleV2Persistent,
    }

    /// <summary>
    /// An enumeration type for indicating which oplock level is to be requested.
    /// </summary>
    public enum ReplayModelRequestedOplockLevel
    {
        OplockLevelNone,
        OplockLevelLeaseV1,
        OplockLevelLeaseV2,
        OplockLevelBatch,
        OplockLevelII,
    }

    /// <summary>
    /// An enumeration type for indicating how to set ChannelSequence in request message header.
    /// </summary>
    public enum ReplayModelChannelSequenceType
    {
        /// <summary>
        /// ChannelSequence set to 0 which is the same as Open.ChannelSequence on server
        /// </summary>
        DefaultChannelSequence,
        /// <summary>
        /// DefaultChannelSequence + 1
        /// </summary>
        ChannelSequenceIncrementOne,
        /// <summary>
        /// 0x7FFF, max allowed value for unsigned difference using 16-bit arithmetic between ChannelSequence and Open.ChannelSequence
        /// </summary>
        ChannelSequenceBoundaryValid,
        /// <summary>
        /// 0x8000, invalid value for unsigned difference using 16-bit arithmetic between ChannelSequence and Open.ChannelSequence
        /// </summary>
        InvalidChannelSequence
    }

    /// <summary>
    /// An enumeration type for indicating whether a same file name is selected or not in the second request.
    /// </summary>
    public enum ReplayModelFileName
    {
        DefaultFileName,
        AlternativeFileName
    }

    /// <summary>
    /// An enumeration type for indicating whether a same CreateGuid is selected or not in the second request.
    /// </summary>
    public enum ReplayModelCreateGuid
    {
        DefaultCreateGuid,
        AlternativeCreateGuid
    }

    /// <summary>
    /// An enumeration type for indicating how to set LeaseState.
    /// </summary>
    public enum ReplayModelLeaseState
    {
        /// <summary>
        /// Lease state is none
        /// </summary>
        LeaseStateIsNone,

        /// <summary>
        /// Lease state does not include SMB2_LEASE_HANDLE_CACHING, use R in adapter
        /// </summary>
        LeaseStateNotIncludeH,

        /// <summary>
        /// Lease state include SMB2_LEASE_HANDLE_CACHING, use RWH in adapter
        /// </summary>
        LeaseStateIncludeH
    }

    /// <summary>
    /// An enumeration type for indicating how to set LeaseKey.
    /// </summary>
    public enum ReplayModelLeaseKey
    {
        DefaultLeaseKey,
        AlternativeLeaseKey,
    }

    /// <summary>
    /// An enumeration type for indicating whether a same FileAttributes is selected or not in the second request.
    /// </summary>
    public enum ReplayModelFileAttributes
    {
        DefaultFileAttributes,
        AlternativeFileAttributes
    }

    /// <summary>
    /// An enumeration type for indicating whether a same CreateDisposition is selected or not in the second request.
    /// </summary>
    public enum ReplayModelCreateDisposition
    {
        DefaultCreateDisposition,
        AlternativeCreateDisposition
    }

    /// <summary>
    /// An enumeration type for indicating which channel is selected.
    /// </summary>
    public enum ReplayModelChannelType
    {
        MainChannel,
        AlternativeChannel
    }

    /// <summary>
    /// An enumeration type for indicating whether a CA share is selected.
    /// </summary>
    public enum ReplayModelShareType
    {
        NonCAShare,
        CAShare
    }

    /// <summary>
    /// An enumeration type for indicating whether same parameters are selected or not in the second request.
    /// </summary>
    public enum ReplayModelRequestCommandParameters
    {
        DefaultParameters,
        AlternativeParameters
    }

    /// <summary>
    /// An enumeration type for indicating how to switch the channels in the second request.
    /// </summary>
    public enum ReplayModelSwitchChannelType
    {
        /// <summary>
        /// Send request thru a main channel
        /// </summary>
        MainChannel,

        /// <summary>
        /// Send request thru an alternative channel while a main channel exists
        /// </summary>
        AlternativeChannelWithMainChannel,

        /// <summary>
        /// Send request thru a main channel after reconnection
        /// </summary>
        ReconnectMainChannel,

        /// <summary>
        /// Send request thru an alternative channel while main channel disconnected
        /// </summary>
        AlternativeChannelWithDisconnectMainChannel,

        /// <summary>
        /// Send request thru a main channel while alternative channel exists
        /// </summary>
        MainChannelWithAlternativeChannel
    }

    /// <summary>
    /// An enumeration type for indicating whether the flag SMB2_GLOBAL_CAP_PERSISTENT_HANDLES is included in the field Capabilities of the negotiate request.
    /// </summary>
    public enum ReplayModelClientSupportPersistent
    {
        ClientSupportPersistent,
        ClientNotSupportPersistent
    }

    /// <summary>
    /// An enumeration type for indicating that the file is created as a directory or a file.
    /// </summary>
    public enum ReplayModelFileType
    {
        Directory,
        NonDirectory
    }

    /// <summary>
    /// An enumeration type for indicating whether the flag SMB2_FLAGS_REPLAY_OPERATION is included in the field Flags of the request header or not.
    /// </summary>
    public enum ReplayModelSetReplayFlag
    {
        WithReplayFlag,
        WithoutReplayFlag
    }

    /// <summary>
    /// An enumeration type for indicating whether the flag SMB2_DHANDLE_FLAG_PERSISTENT is included in the field Flags of the context SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or not.
    /// </summary>
    public enum ReplayModelSetPersistentFlag
    {
        WithPersistentFlag,
        WithoutPersistentFlag
    }

    /// <summary>
    /// An enumeration type for indicating whether the server supports the IoCtl code VALIDATE_NEGOTIATE_INFO or not.
    /// </summary>
    public enum ReplayModelSupportValidateNegotiateInfo
    {
        NotSupportValidateNegotiateInfo,

        SupportValidateNegotiateInfo
    }
}
