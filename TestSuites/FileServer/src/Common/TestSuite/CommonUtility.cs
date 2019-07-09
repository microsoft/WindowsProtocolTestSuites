// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite
{
    #region Common Utility
    /// <summary>
    /// File server type used in test
    /// </summary>
    public enum FileServerType
    {
        GeneralFileServer,
        ScaleOutFileServer
    }
    #endregion

    #region SWN Utility

    /// <summary>
    /// Witness type used in test
    /// </summary>
    public enum WitnessType
    {
        /// <summary>
        /// Do not use MS-SWN to monitor the failover of File Server.
        /// </summary>
        None,

        /// <summary>
        /// Use MS-SWN to monitor the failover of File Server.
        /// </summary>
        SwnWitness
    }

    /// <summary>
    /// Register type
    /// </summary>
    public enum SwnRegisterType : uint
    {
        /// <summary>
        /// Already Exists
        /// </summary>
        AlreadyExists = 1,

        /// <summary>
        /// Invalid NetName
        /// </summary>
        InvalidNetName = 2,

        /// <summary>
        /// Invalid Version
        /// </summary>
        InvalidVersion = 3,

        /// <summary>
        /// Invalid UnRegister
        /// </summary>
        InvalidUnRegister = 4,

        /// <summary>
        /// Invalid Request for asynchronous notification
        /// </summary>
        InvalidRequest = 5,

        /// <summary>
        /// Invalid IpAddress
        /// </summary>
        InvalidIpAddress = 6,

        /// <summary>
        /// Invalid share name
        /// </summary>
        InvalidShareName = 7,

        /// <summary>
        /// When the Notification Pending Timer expires, 
        /// If WitnessRegistration.IsAsyncNotifyRegistered is TRUE and WitnessRegistration.LastUseTime plus KeepAliveTimeout is earlier than the current time, 
        /// the server MUST fail the request with ERROR_TIMEOUT
        /// </summary>
        KeepAliveTimeout = 8,
    }
    #endregion

    #region SMB2 Utility

    /// <summary>
    /// Types of LEASE_BREAK_ACK request
    /// </summary>
    public enum LeaseBreakAckType
    {
        /// <summary>
        /// LeaseKey in the LEASE_BREAK_ACK is not equal to original LeaseKey
        /// </summary>
        InvalidLeaseKey,

        /// <summary>
        /// LeaseState in the LEASE_BREAK_ACK is not equal or less than that in LEASE_BREAK_Notification
        /// </summary>
        InvalidLeaseState,

        /// <summary>
        /// ClientGuid could not be found in GlobalLeaseTableList (i.e. client close the open with lease)
        /// </summary>
        InvalidClientGuid,

        /// <summary>
        /// Not invalid LEASE_BREAK_ACK
        /// </summary>
        None
    }

    /// <summary>
    /// Types of VALIDATE_NEGOTIATE_INFO request
    /// </summary>
    public enum ValidateNegotiateInfoRequestType
    {
        /// <summary>
        /// No greatest common dialect between Dialects field in VALIDATE_NEGOTIATE_INFO Request 
        /// and the server implemented dialect, or the greatest common dialect is not equal to Connection.Dialect
        /// </summary>
        InvalidDialects,

        /// <summary>
        /// The Guid field in VALIDATE_NEGOTIATE_INFO request is not equal to the ClientGuid sent in the original SMB2 NEGOTIATE request
        /// </summary>
        InvalidGuid,

        /// <summary>
        /// The SecurityMode field in VALIDATE_NEGOTIATE_INFO request is not equal to the SecurityMode sent in the original SMB2 NEGOTIATE request
        /// </summary>
        InvalidSecurityMode,

        /// <summary>
        /// The Capabilities field in VALIDATE_NEGOTIATE_INFO request is not equal to the Capabilities sent in the original SMB2 NEGOTIATE request
        /// </summary>
        InvalidCapabilities,

        /// <summary>
        /// The MaxOutputResponse field in VALIDATE_NEGOTIATE_INFO request is less than the size of a VALIDATE_NEGOTIATE_INFO Response
        /// </summary>
        InvalidMaxOutputResponse,

        /// <summary>
        /// The Dialects field in VALIDATE_NEGOTIATE_INFO request contains SMB 3.1.1 dialect
        /// </summary>
        InvalidSMB311Dialect,

        /// <summary>
        /// All fields in VALIDATE_NEGOTIATE_INFO request are valid and the request is expected to SUCCESS
        /// </summary>
        None
    }

    /// <summary>
    /// Types of file name
    /// </summary>
    public enum FileNameType
    {
        /// <summary>
        /// Indicates an normal valid file name(not symbolic link nor DFS link) which not existed on SUT
        /// </summary>
        NotExistedValidFileName,

        /// <summary>
        /// Indicates an normal valid file name(not symbolic link nor DFS link) which existed on SUT
        /// </summary>
        ExistedValidFileName,

        /// <summary>
        /// Indicates the intermediate component of the file name is a symbolic link
        /// </summary>
        SymbolicLinkInMiddle,

        /// <summary>
        /// Indicates the final component of the file name is a symbolic link
        /// </summary>
        SymbolicLinkAtLast,

        /// <summary>
        /// Indicates that the attempted open operation failed due to the presence of a symbolic link in the target path name
        /// </summary>
        InvalidSymbolicLink
    }
    #endregion

    #region FSRVP Utility
    /// <summary>
    /// ShadowCopy object.
    /// </summary>
    public struct FsrvpClientShadowCopy
    {
        /// <summary>
        /// The GUID of the shadow copy assigned by the client.
        /// </summary>
        public Guid clientShadowCopyId;
        /// <summary>
        /// The GUID of the shadow copy returned by the server.
        /// </summary>
        public Guid serverShadowCopyId;
        /// <summary>
        /// The name of the share in UNC format.
        /// </summary>
        public string shareName;
        /// <summary>
        /// The name of the share exposing the shadow copy of the base share identified by ShareNameUNC, in UNC format.
        /// </summary>
        public string exposedName;
        /// <summary>
        /// The time at which the shadow copy of the share is created. This MUST be a 64-bit integer value containing
        /// the number of 100-nanosecond intervals since January 1, 1601 (UTC).
        /// </summary>
        public long CreationTimestamp;
    }

    /// <summary>
    /// Share paths type.
    /// </summary>
    public enum FsrvpSharePathsType : ulong
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Test ShareName with different share paths on multiple nodes.
        /// </summary>
        OnDifferentNode = 1,
        /// <summary>
        /// Test ShareName with different share paths on the cluster and the owner node.
        /// </summary>
        OnClusterAndOwnerNode = 2,
        /// <summary>
        /// Test ShareName with different share paths on the cluster and the non-owner node.
        /// </summary>
        OnClusterAndNonOwnerNode = 3,
    }

    #endregion
}
