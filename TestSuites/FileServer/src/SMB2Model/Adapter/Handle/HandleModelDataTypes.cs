// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Handle
{
    /// <summary>
    /// The configuration of server for handle
    /// </summary>
    public struct HandleConfig
    {
        /// <summary>
        /// SMB 2.002 or SMB 2.1 or SMB 3.0
        /// </summary>
        public ModelDialectRevision MaxSmbVersionSupported;

        /// <summary>
        /// The platform
        /// </summary>
        public Platform Platform;

        /// <summary>
        /// Indicates whether the server supports persistent handle.
        /// </summary>
        public bool IsPersistentHandleSupported;

        /// <summary>
        /// Indicates whether the server supports leasing.
        /// </summary>
        public bool IsLeasingSupported;

        /// <summary>
        /// Indicates whether the server supports directory leasing
        /// </summary>
        public bool IsDirectoryLeasingSupported;

        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "HandleConfig State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "MaxSmbVersionSupported", this.MaxSmbVersionSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "Platform", this.Platform.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsPersistentHandleSupported", this.IsPersistentHandleSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsLeasingSupported", this.IsLeasingSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsDirectoryLeasingSupported", this.IsDirectoryLeasingSupported.ToString());

            return outputInfo.ToString();
        }
    }

    /// <summary>
    /// Indicate the Handle type the client requests
    /// </summary>
    public enum ModelHandleType
    {
        /// <summary>
        /// The client requests a durable handle v1 
        /// </summary>
        DurableHandleV1,

        /// <summary>
        /// The client requests a durable handle v2
        /// </summary>
        DurableHandleV2,

        /// <summary>
        /// The client requests a persistent handle 
        /// </summary>
        PersistentHandle
    }
    
    /// <summary>
    /// Indicate whether the create context contains durableV1 request
    /// </summary>
    public enum DurableV1RequestContext
    {
        /// <summary>
        /// The create context contains DurableV1Request
        /// </summary>
        DurableV1RequestContextExist,

        /// <summary>
        /// The create context does not contain DurableV1Request
        /// </summary>
        DurableV1RequestContextNotExist
    }

    /// <summary>
    /// Indicate whether the create context contains durableV2 request and its type
    /// </summary>
    public enum DurableV2RequestContext
    {
        /// <summary>
        /// The create context contains DurableV2Request without persistent flag set
        /// </summary>
        DurableV2RequestContextExistWithoutPersistent,

        /// <summary>
        /// The create context contains DurableV2Request with persistent flag set
        /// </summary>
        DurableV2RequestContextExistWithPersistent,

        /// <summary>
        /// The create context does not contain DurableV2Request
        /// </summary>
        DurableV2RequestContextNotExist
    }

    /// <summary>
    /// Indicate whether the create context contains durableV1 reconnect
    /// </summary>
    public enum DurableV1ReconnectContext
    {
        /// <summary>
        /// The create context contains DurableV1Reconnect
        /// </summary>
        DurableV1ReconnectContextExist,

        /// <summary>
        /// The create context does not contain DurableV1Reconnect
        /// </summary>
        DurableV1ReconnectContextNotExist
    }

    /// <summary>
    /// Indicate whether the create context contains durableV2 reconnect and its type
    /// </summary>
    public enum DurableV2ReconnectContext
    {
        /// <summary>
        /// The create context contains DurableV2Reconnect without persistent flag set
        /// </summary>
        DurableV2ReconnectContextExistWithoutPersistent,

        /// <summary>
        /// The create context contains DurableV2Reconnect with persistent flag set
        /// </summary>
        DurableV2ReconnectContextExistWithPersistent,

        /// <summary>
        /// The create context does not contain DurableV2Reconnect
        /// </summary>
        DurableV2ReconnectContextNotExist
    }

    /// <summary>
    /// Indicate the type of Oplock or Lease
    /// </summary>
    public enum OplockLeaseType
    {
        /// <summary>
        /// Indicate no Oplock or Lease context
        /// </summary>
        NoOplockOrLease,

        /// <summary>
        /// Indicate lease V1 context
        /// </summary>
        LeaseV1,

        /// <summary>
        /// Indicate lease V2 context
        /// </summary>
        LeaseV2,

        /// <summary>
        /// Indicate batch oplock
        /// </summary>
        BatchOplock
    }

    /// <summary>
    /// Indicate the context type in server response
    /// </summary>
    public enum DurableHandleResponseContext: uint 
    {
        /// <summary>
        /// Indicate there is no context in server response
        /// </summary>
        NONE,

        /// <summary>
        /// Indicate the SMB2_CREATE_DURABLE_HANDLE_RESPONSE context in server response
        /// </summary>
        SMB2_CREATE_DURABLE_HANDLE_RESPONSE,

        /// <summary>
        /// Indicate the SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 context in server response,
        /// </summary>
        SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2,

        /// <summary>
        /// Indicate the SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 context in server response,
        /// SMB2_DHANDLE_FLAG_PERSISTENT is set
        /// </summary>
        SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2_WITH_PERSISTENT,
    }

    public enum LeaseResponseContext
    {
        /// <summary>
        /// Indicate there is no context in server response
        /// </summary>
        NONE,
        /// <summary>
        /// Indicate the SMB2_CREATE_RESPONSE_LEASE context in server response
        /// </summary>
        SMB2_CREATE_RESPONSE_LEASE = 8,
        
        /// <summary>
        /// Indicate the SMB2_CREATE_RESPONSE_LEASE_V2 context in server response
        /// </summary>
        SMB2_CREATE_RESPONSE_LEASE_V2 = 16
    }

    /// <summary>
    /// Indicate the tree connect target
    /// </summary>
    public enum ModelConnectTarget
    {
        /// <summary>
        /// Indicate tree connect to CAShare directory
        /// </summary>
        CAShareDirectory,

        /// <summary>
        /// Indicate tree connect to CAShare file
        /// </summary>
        CAShareFile,

        /// <summary>
        /// Indicate tree connect to non-CAShare directory
        /// </summary>
        CommonShareDirectory,

        /// <summary>
        /// Indicate tree connect to non-CAShare file
        /// </summary>
        CommonShareFile
    }

    /// <summary>
    /// Indicate whether or not connect to CA share
    /// </summary>
    public enum CAShareType
    {
        /// <summary>
        /// Indicate connect to CA share
        /// </summary>
        CAShare,

        /// <summary>
        /// Indicate not connect to CA share
        /// </summary>
        NonCAShare
    }

    /// <summary>
    /// Indicate using same or different lease key when reconnecting
    /// </summary>
    public enum LeaseKeyDifferentialType
    {
        /// <summary>
        /// Indicate using same lease key
        /// </summary>
        SameLeaseKey,

        /// <summary>
        /// Indicate using different lease key
        /// </summary>
        DifferentLeaseKey
    }

    /// <summary>
    /// Indicate using same or different client id when reconnecting
    /// </summary>
    public enum ClientIdType
    {
        /// <summary>
        /// Indicate using same client id
        /// </summary>
        SameClient,

        /// <summary>
        /// Indicate using different client id
        /// </summary>
        DifferentClient
    }

    /// <summary>
    /// Indicate using same or different file name when reconnecting
    /// </summary>
    public enum FileNameType
    {
        /// <summary>
        /// Indicate using same file name
        /// </summary>
        SameFileName,

        /// <summary>
        /// Indicate using different file name
        /// </summary>
        DifferentFileName
    }

    /// <summary>
    /// Indicate using same or different create Guid when reconnecting
    /// </summary>
    public enum CreateGuidType
    {
        /// <summary>
        /// Indicate using same create Guid
        /// </summary>
        SameCreateGuid,

        /// <summary>
        /// Indicate using different create Guid
        /// </summary>
        DifferentCreateGuid
    }

    /// <summary>
    /// Indicate whether SMB2_GLOBAL_CAP_PERSISTENT_HANDLES is set in Negotiate Request or not
    /// </summary>
    public enum PersistentBitType
    {
        /// <summary>
        /// Indicate client sets SMB2_GLOBAL_CAP_PERSISTENT_HANDLES bit in Negotiate Request
        /// </summary>
        PersistentBitSet,

        /// <summary>
        /// Indicate client does not set SMB2_GLOBAL_CAP_PERSISTENT_HANDLES bit in Negotiate Request
        /// </summary>
        PersistentBitNotSet
    }
}
