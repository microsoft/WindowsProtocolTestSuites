// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.SessionMgmt
{
    public struct SessionMgmtConfig
    {
        /// <summary>
        /// Max SMB2 dialect that server supports
        /// </summary>
        public ModelDialectRevision MaxSmbVersionSupported;

        public bool IsMultiChannelCapable;

        public Platform Platform;
    }

    public enum ModelConnectionId
    {
        /// <summary>
        /// Id for first established connection
        /// </summary>
        MainConnection = 0,

        /// <summary>
        /// Id for second established connection
        /// </summary>
        AlternativeConnection = 1,
    }

    public enum ModelSessionId
    {
        /// <summary>
        /// The SMB2 server MUST reserve 0 as a SessionId for which no session exists
        /// </summary>
        ZeroSessionId,

        /// <summary>
        /// The SMB2 server MUST reserve -1 as an invalid SessionId
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// Id for first established session
        /// </summary>
        MainSessionId,

        /// <summary>
        /// Id for second established session
        /// </summary>
        AlternativeSessionId

    }

    public enum ModelFlags
    {
        /// <summary>
        /// Indicates that the request is to bind an existing session to a new connection
        /// </summary>
        Binding,

        /// <summary>
        /// Do nothing
        /// </summary>
        NotBinding
    }

    public enum ModelSigned
    {
        /// <summary>
        /// SMB2_FLAGS_SIGNED bit is set in the Flags field in the header
        /// </summary>
        SignFlagSet,

        /// <summary>
        /// SMB2_FLAGS_SIGNED bit is not set in the Flags field in the header
        /// </summary>
        SignFlagNotSet
    }

    public enum ModelAllowReauthentication
    {
        /// <summary>
        ///  Indicates that reauthentication is allowed in Session Setup request
        /// </summary>
        AllowReauthentication,

        /// <summary>
        ///  Indicates that reauthentication is not allowed in Session Setup request
        /// </summary>
        NotAllowReauthentication
    }
}
