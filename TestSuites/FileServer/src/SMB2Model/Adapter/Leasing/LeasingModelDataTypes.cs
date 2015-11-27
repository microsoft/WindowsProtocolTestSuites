// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using Microsoft.Modeling;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Leasing
{
    /// <summary>
    /// The configuration of server for negotiate
    /// </summary>
    public struct LeasingConfig
    {
        /// <summary>
        /// SMB 2.002 or SMB 2.1 or SMB 3.0
        /// </summary>
        public ModelDialectRevision MaxSmbVersionSupported;

        #region Create Config
        // SMB 2.1 or SMB 3.0
        /// <summary>
        /// Indicates whether the server supports leasing or not.
        /// </summary>
        public bool IsLeasingSupported;

        // SMB 3.0
        /// <summary>
        /// Indicates whether the server supports directory leasing or not.
        /// </summary>
        public bool IsDirectoryLeasingSupported;

        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "LeasingConfig State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "MaxSmbVersionSupported", this.MaxSmbVersionSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsLeasingSupported", this.IsLeasingSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "IsDirectoryLeasingSupported", this.IsDirectoryLeasingSupported.ToString());

            return outputInfo.ToString();
        }
        #endregion

    }

    /// <summary>
    /// The type of create context for leasing
    /// </summary>
    public enum LeaseContextType
    {
        /// <summary>
        /// SMB2_CREATE_REQUEST_LEASE exists.
        /// </summary>
        LeaseV1,
        /// <summary>
        /// SMB2_CREATE_REQUEST_LEASE_V2 exists.
        /// </summary>
        LeaseV2
    }

    /// <summary>
    /// The type of the lease key.
    /// </summary>
    public enum LeaseKeyType
    {
        /// <summary>
        /// Empty lease key.
        /// </summary>
        EmptyLeaseKey = 0,
        /// <summary>
        /// Valid lease key.
        /// </summary>
        ValidLeaseKey = 1,
    }

    /// <summary>
    /// The type of the parent lease key.
    /// </summary>
    public enum ParentLeaseKeyType
    {
        /// <summary>
        /// Empty parent lease key.
        /// </summary>
        EmptyParentLeaseKey = 0,
        /// <summary>
        /// Valid parent lease key.
        /// </summary>
        ValidParentLeaseKey = 1,
        /// <summary>
        /// Invalid parent lease key.
        /// </summary>
        InvalidParentLeaseKey = 2
    }

    /// <summary>
    /// Abstract create context for requesting the lease.
    /// </summary>
    public class ModelCreateContextRequest
    {
    }

    /// <summary>
    /// Abstract create context for requesting the lease V1.
    /// </summary>
    public class ModelCreateRequestLease : ModelCreateContextRequest
    {
        /// <summary>
        /// The lease key type in the request.
        /// </summary>
        public LeaseKeyType LeaseKey;
        /// <summary>
        /// The lease state in the request.
        /// </summary>
        public uint LeaseState;
        /// <summary>
        /// The lease flags in the request.
        /// </summary>
        public LeaseFlagsValues LeaseFlags;
        /// <summary>
        /// The lease duration in the request.
        /// </summary>
        public ulong LeaseDuration;
    }

    /// <summary>
    /// Abstract create context for requesting the lease V2.
    /// </summary>
    public class ModelCreateRequestLeaseV2 : ModelCreateContextRequest
    {
        /// <summary>
        /// The lease key type in the request.
        /// </summary>
        public LeaseKeyType LeaseKey;
        /// <summary>
        /// The lease state in the request.
        /// </summary>
        public uint LeaseState;
        /// <summary>
        /// The lease flags in the request.
        /// </summary>
        public LeaseFlagsValues LeaseFlags;
        /// <summary>
        /// The lease duration in the request.
        /// </summary>
        public ulong LeaseDuration;
        /// <summary>
        /// The parent lease key in the request.
        /// </summary>
        public ParentLeaseKeyType ParentLeaseKey;
        /// <summary>
        /// The epoch in the request.
        /// </summary>
        public uint Epoch;
    }

    /// <summary>
    /// The type of file operation.
    /// </summary>
    public enum FileOperation
    {
        // Operations to break READ caching lease
        
        /// <summary>
        /// The file is opened in a manner that overwrites the existing file.
        /// </summary>
        OPEN_OVERWRITE = 1,
        /// <summary>
        /// Data is written to the file.
        /// </summary>
        WRITE_DATA = 2,
        /// <summary>
        /// The file size is changed.
        /// </summary>
        SIZE_CHANGED = 3,
        /// <summary>
        /// A byte range lock is requested for the file.
        /// </summary>
        RANGE_LOCK = 4,

        // Operations to break WRITE caching lease

        /// <summary>
        /// The file is opened by a local application or via another protocol, or opened via SMB2 without providing the same ClientId, and requested access includes any flags other than FILE_READ_ATTRIBUTES, FILE_WRITE_ATTRIBUTES, and SYNCHRONIZE.
        /// The file is opened in a manner that does not overwrite the existing file.
        /// </summary>
        OPEN_WITHOUT_OVERWRITE = 5,
        /// <summary>
        /// The file is opened by a local application or via another protocol, or opened via SMB2 without providing the same ClientId, and requested access includes FILE_READ_ATTRIBUTES, FILE_WRITE_ATTRIBUTES, or SYNCHRONIZE but no other flags.
        /// Traditional case cover this.
        /// </summary>
        OPEN_WITHOUT_BREAKING_WRITE_LEASE = 6,

        // Operations to break HANDLE caching lease

        /// <summary>
        /// A file is being opened by a local application, via another protocol, or opened via SMB2 without providing the same ClientId, and this open will fail with SHARING_VIOLATION due to the existing opens that hold the lease.
        /// </summary>
        OPEN_SHARING_VIOLATION = 7,
        /// <summary>
        /// The file is opened in a manner that overwrites the existing file and this open will fail with SHARING_VIOLATION due to the existing opens that hold the lease.
        /// </summary>
        OPEN_SHARING_VIOLATION_WITH_OVERWRITE = 8,
        /// <summary>
        /// The file is being renamed by an open with a different ClientId than the lease was acquired with.
        /// </summary>
        RENAMEED = 9,
        /// <summary>
        /// The file is being deleted by an open with a different ClientId than the lease was acquired with.
        /// </summary>
        DELETED = 10,
        /// <summary>
        /// The parent directory is being renamed.
        /// </summary>
        PARENT_DIR_RENAMED = 11,
    }

    /// <summary>
    /// The type of the operator.
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// The operator which has same ClientGuid and same LeaseKey with the original client.
        /// </summary>
        SameClientId = 0,
        /// <summary>
        /// The operator which has same ClientGuid with the original client but does not request to get the same lease.
        /// </summary>
        SameClientGuidDifferentLeaseKey = 1,
        /// <summary>
        /// The operator which has different ClientGuid with the original client and does not request to get the same lease.
        /// </summary>
        SecondClient = 2,
    }

    /// <summary>
    /// Indicate whether client support directory leasing or not
    /// </summary>
    public enum ClientSupportDirectoryLeasingType
    {
        /// <summary>
        /// Indicate client support directory leasing
        /// </summary>
        ClientSupportDirectoryLeasing,

        /// <summary>
        /// Indicate client not support directory leasing
        /// </summary>
        ClientNotSupportDirectoryLeasing
    }
    
    /// <summary>
    /// Indicate connect to directory or non-directory
    /// </summary>
    public enum ConnectTargetType
    {
        /// <summary>
        /// Indicate connect to directory
        /// </summary>
        ConnectToDirectory,

        /// <summary>
        /// Indicate connect to non-directory
        /// </summary>
        ConnectToNonDirectory
    }

    /// <summary>
    /// Indicate whether lease context is included in response
    /// </summary>
    public enum ReturnLeaseContextType
    {
        /// <summary>
        /// Indicate lease context is included in response
        /// </summary>
        ReturnLeaseContextIncluded,

        /// <summary>
        /// Indicate lease context is not included in response
        /// </summary>
        ReturnLeaseContextNotIncluded
    }

    /// <summary>
    /// Indicate whether the lease key is valid or not
    /// </summary>
    public enum ModelLeaseKeyType
    {
        /// <summary>
        /// Indicate the lease key is valid
        /// </summary>
        ValidLeaseKey,

        /// <summary>
        /// Indicate the lease key is invalid
        /// </summary>
        InvalidLeaseKey
    }
}
