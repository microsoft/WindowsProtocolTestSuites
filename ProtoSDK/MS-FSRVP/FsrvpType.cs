// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;


namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsrvp
{
    /// <summary>
    /// MS-FSRVP 2.2.1.1 FSSAGENT_SHARE_MAPPING_1
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct FSSAGENT_SHARE_MAPPING_1
    {
        /// <summary>
        /// The GUID of the shadow copy set.
        /// </summary>
        public Guid ShadowCopySetId;
        /// <summary>
        /// The GUID of the shadow copy.
        /// </summary>
        public Guid ShadowCopyId;
        /// <summary>
        /// The name of the share in UNC format.
        /// </summary>
        public string ShareNameUNC;
        /// <summary>
        /// The name of the share exposing the shadow copy of the base share (identified by ShareName).
        /// </summary>
        public string ShadowCopyShareName;
        /// <summary>
        /// The time at which the shadow copy of the share is created.
        /// </summary>
        public long CreationTimestamp;
    };

    /// <summary>
    /// 2.2.3.1 FSSAGENT_SHARE_MAPPING
    /// The FSSAGENT_SHARE_MAPPING union contains mapping information of a share to its shadow copy based on the level value.
    /// 
    /// </summary>
    public struct FSSAGENT_SHARE_MAPPING
    {
        /// <summary>
        /// Indicates whether ShareMapping1 is NULL.
        /// </summary>
        public bool ShareMapping1IsNull;

        /// <summary>
        /// A pointer to an FSSAGENT_SHARE_MAPPING_1 structure, as specified in section 2.2.1.1
        /// </summary>
        public FSSAGENT_SHARE_MAPPING_1 ShareMapping1;
    };

    /// <summary>
    /// The information level of the share mapping data.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsrvpLevel : uint
    {
        /// <summary>
        /// FSSAGENT_SHARE_MAPPING_1
        /// </summary>
        FSRVP_LEVEL_1 = 1,
    }

    /// <summary>
    /// MS-FSRVP 2.2.2.1 SHADOW_COPY_ATTRIBUTES
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum FsrvpShadowCopyAttributes : uint
    {
        /// <summary>
        /// The shadow copy is persistent across reboots of the server.
        /// </summary>
        FSRVP_ATTR_PERSISTENT = 0x00000001,
        /// <summary>
        /// The shadow copy is created as read-only. Client is not allowed to modify its contents.
        /// </summary>
        FSRVP_ATTR_NO_AUTO_RECOVERY = 0x00000002,
        /// <summary>
        /// The shadow copy is not automatically deleted when all the references to the shadow copy are released.
        /// </summary>
        FSRVP_ATTR_NO_AUTO_RELEASE = 0x00000008,
        /// <summary>
        /// The shadow copy is created without any application-specific participation.
        /// </summary>
        FSRVP_ATTR_NO_WRITERS = 0x00000010,
        /// <summary>
        /// The shadow copy is being created to provide a consistent view of data that will be exposed via a file share.
        /// </summary>
        FSRVP_ATTR_FILE_SHARE = 0x04000000,
        /// <summary>
        /// The shadow copy is created as read-write. Client is allowed to modify its contents between the 
        /// ExposeShadowCopySet and RecoveryCompleteShadowCopySet operations, after which the shadow copy becomes 
        /// permanently read-only.
        /// </summary>
        FSRVP_ATTR_AUTO_RECOVERY = 0x00400000,
    }

    /// <summary>
    /// MS-FSRVP 2.2.2.2 CONTEXT_VALUES
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsrvpContextValues : uint
    {
        /// <summary>
        /// Specifies an auto-release, non-persistent shadow copy.
        /// </summary>
        FSRVP_CTX_BACKUP = 0x00000000,
        /// <summary>
        /// Specifies a persistent, non-auto-release shadow copy. 
        /// It is a combination of following shadow copy attributes:
        /// ATTR_PERSISTENT
        /// ATTR_NO_AUTO_RELEASE
        /// </summary>
        FSRVP_CTX_APP_ROLLBACK = 0x00000009,
        /// <summary>
        /// Specifies an auto-release, non-persistent shadow copy created without writer involvement. 
        /// It is a combination of following shadow copy attributes: 
        /// ATTR_NO_WRITERS
        /// </summary>
        FSRVP_CTX_FILE_SHARE_BACKUP = 0x00000010,
        /// <summary>
        /// Specifies a persistent, non-auto-release shadow copy without writer involvement.
        /// It is a combination of following shadow copy attributes:
        /// ATTR_PERSISTENT
        /// ATTR_NO_AUTO_RELEASE
        /// ATTR_NO_WRITERS
        /// </summary>
        FSRVP_CTX_NAS_ROLLBACK = 0x00000019,
    }

    /// <summary>
    /// MS-FSRVP 2.2.2.3 SHADOW_COPY_COMPATIBILITY_VALUES
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum FsrvpShadowCopyCompatibilityValues : uint
    {
        /// <summary>
        /// The provider managing the shadow copies for the specified path does not support
        /// defragmentation operations on the object store containing the path.
        /// </summary>
        FSRVP_DISABLE_DEFRAG = 0x00000001,
        /// <summary>
        /// The provider managing the shadow copies for the specified path does not support
        /// content index operations on the object store containing the path.
        /// </summary>
        FSRVP_DISABLE_CONTENTINDEX = 0x00000002,
    }

    /// <summary>
    /// Protocol versions supported by a server.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsrvpVersionValues : uint
    {
        /// <summary>
        /// Version 1 of the FSRVP protocol.
        /// </summary>
        FSRVP_RPC_VERSION_1 = 0x00000001,
    }

    /// <summary>
    /// MS-FSRVP 2.2.3 Error Codes
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsrvpErrorCode : uint
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        FSRVP_SUCCESS = 0,
        /// <summary>
        /// The wait for a shadow copy commit or expose operation has timed out.
        /// </summary>
        FSRVP_E_WAIT_TIMEOUT = 0x00000102,
        /// <summary>
        /// A method call was invalid because of the state of the server. 
        /// (For example, calling AddToShadowCopySet (opnum 3) before StartShadowCopySet (opnum 2)).
        /// </summary>
        FSRVP_E_BAD_STATE = 0x80042301,
        /// <summary>
        /// The object store which contains the share to be shadow copied is not supported by the server.
        /// </summary>
        FSRVP_E_VOLUME_NOT_SUPPORTED = 0x8004230C,
        /// <summary>
        /// A call was made to either SetContext (Opnum 1) or StartShadowCopySet (Opnum 2) while 
        /// the creation of another shadow copy set is in progress.
        /// </summary>
        FSRVP_E_SHADOW_COPY_SET_IN_PROGRESS = 0x80042316,
        /// <summary>
        /// The caller does not have the permissions to perform the operation.
        /// </summary>
        FSRVP_E_ACCESSDENIED = 0x80070005,
        /// <summary>
        /// One or more arguments are invalid.
        /// </summary>
        FSRVP_E_INVALIDARG = 0x80070057,
        /// <summary>
        /// The wait for a shadow copy commit or expose operation has failed.
        /// </summary>
        FSRVP_E_WAIT_FAILED = 0xFFFFFFFF,
        /// <summary>
        /// The file store which contains the share to be shadow copied is not supported by the server.
        /// </summary>
        FSRVP_E_NOT_SUPPORTED = 0x8004230C,
        /// <summary>
        /// The specified object already exists.
        /// </summary>
        FSRVP_E_OBJECT_ALREADY_EXISTS = 0x8004230D,
        /// <summary>
        /// The specified object does not exist.
        /// </summary>
        FSRVP_E_OBJECT_NOT_FOUND = 0x80042308,
        /// <summary>
        /// The specified context value is invalid.
        /// </summary>
        FSRVP_E_UNSUPPORTED_CONTEXT = 0x8004231B,
    }

    /// <summary>
    /// Opnum for FSRVP interface
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FSRVP_OPNUM : ushort
    {
        /// <summary>
        /// The opnum for GetSupportedVersion method
        /// </summary>
        GetSupportedVersion = 0,

        /// <summary>
        /// The opnum for SetContext method
        /// </summary>
        SetContext = 1,

        /// <summary>
        /// The opnum for StartShadowCopySet method
        /// </summary>
        StartShadowCopySet = 2,

        /// <summary>
        /// The opnum for AddToShadowCopySet method
        /// </summary>
        AddToShadowCopySet = 3,

        /// <summary>
        /// The opnum for CommitShadowCopySet method
        /// </summary>
        CommitShadowCopySet = 4,

        /// <summary>
        /// The opnum for ExposeShadowCopySet method
        /// </summary>
        ExposeShadowCopySet = 5,

        /// <summary>
        /// The opnum for RecoveryCompleteShadowCopySet method
        /// </summary>
        RecoveryCompleteShadowCopySet = 6,

        /// <summary>
        /// The opnum for AbortShadowCopySet method
        /// </summary>
        AbortShadowCopySet = 7,

        /// <summary>
        /// The opnum for IsPathSupported method
        /// </summary>
        IsPathSupported = 8,

        /// <summary>
        /// The opnum for IsPathShadowCopied method
        /// </summary>
        IsPathShadowCopied = 9,

        /// <summary>
        /// The opnum for GetShareMapping method
        /// </summary>
        GetShareMapping = 10,

        /// <summary>
        /// The opnum for DeleteShareMapping method
        /// </summary>
        DeleteShareMapping = 11,

        /// <summary>
        /// The opnum for PrepareShadowCopySet method
        /// </summary>
        PrepareShadowCopySet = 12,
    }

    /// <summary>
    /// The status of the shadow copy set.
    /// </summary>
    public enum FsrvpStatus : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// StartShadowCopySet is called
        /// </summary>
        Started = 1,
        /// <summary>
        /// AddToShadowCopySet is called
        /// </summary>
        Added = 2,
        /// <summary>
        /// PrepareShadowCopySet is called
        /// </summary>
        CreateInProgress = 3,
        /// <summary>
        /// CommitShadowCopySet is called
        /// </summary>
        Committed = 4,
        /// <summary>
        /// ExposeShadowCopySet is called
        /// </summary>
        Exposed = 5,
        /// <summary>
        /// RecoveryCompleteShadowCopySet is called
        /// </summary>
        Recovered = 6,
    }
}
