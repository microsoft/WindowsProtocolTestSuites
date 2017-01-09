// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// this flag describes the capabilities of the DC that produced the DRS_EXTENSIONS_INT structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_EXTENSIONS_IN_FLAGS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Unused. MUST be 1 and ignored.
        /// </summary>
        DRS_EXT_BASE = 0x00000001,

        /// <summary>
        /// If present, signifies that the DC supports DRS_MSG_REPADD_V2 and a version for the server-to-server
        /// replication implementation.
        /// </summary>
        DRS_EXT_ASYNCREPL = 0x00000002,

        /// <summary>
        /// If present, signifies that the DC supports IDL_DRSRemoveDsServer and IDL_DRSRemoveDsDomain.
        /// </summary>
        DRS_EXT_REMOVEAPI = 0x00000004,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication implementation.
        /// </summary>
        DRS_EXT_MOVEREQ_V2 = 0x00000008,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication implementation.
        /// </summary>
        DRS_EXT_GETCHG_DEFLATE = 0x00000010,

        /// <summary>
        /// If present, signifies that the DC supports IDL_DRSDomainControllerInfo.
        /// </summary>
        DRS_EXT_DCINFO_V1 = 0x00000020,

        /// <summary>
        /// Unused. MUST be 1 and ignored.
        /// </summary>
        DRS_EXT_RESTORE_USN_OPTIMIZATION = 0x00000040,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server implementation of
        /// creating objects remotely.
        /// </summary>
        DRS_EXT_ADDENTRY = 0x00000080,

        /// <summary>
        /// If present, signifies that the DC supports IDL_DRSExecuteKCC.
        /// </summary>
        DRS_EXT_KCC_EXECUTE = 0x00000100,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server implementation of
        /// creating objects remotely.
        /// </summary>
        DRS_EXT_ADDENTRY_V2 = 0x00000200,

        /// <summary>
        /// If present, signifies that the DC supports link value replication, and this support is enabled.
        /// </summary>
        DRS_EXT_LINKED_VALUE_REPLICATION = 0x00000400,

        /// <summary>
        /// If present, signifies that the DC supports DRS_MSG_DCINFOREPLY_V2.
        /// </summary>
        DRS_EXT_DCINFO_V2 = 0x00000800,

        /// <summary>
        /// Unused. MUST be 1 and ignored.
        /// </summary>
        DRS_EXT_INSTANCE_TYPE_NOT_REQ_ON_MOD = 0x00001000,

        /// <summary>
        /// A server-to-server implementation-specific flag. If present, it indicates that the security provider
        /// used for the connection supports encryption through RPC.
        /// </summary>
        DRS_EXT_CRYPTO_BIND = 0x00002000,

        /// <summary>
        /// If present, signifies that the DC supports IDL_DRSGetReplInfo.
        /// </summary>
        DRS_EXT_GET_REPL_INFO = 0x00004000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication
        /// implementation that uses 128-bit encryption.
        /// </summary>
        DRS_EXT_STRONG_ENCRYPTION = 0x00008000,

        /// <summary>
        /// If present, signifies that the DC supports DRS_MSG_DCINFOREPLY_VFFFFFFFF.
        /// </summary>
        DRS_EXT_DCINFO_VFFFFFFFF = 0x00010000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server implementation of group
        /// expansion.
        /// </summary>
        DRS_EXT_TRANSITIVE_MEMBERSHIP = 0x00020000,

        /// <summary>
        /// If present, signifies that the DC supports IDL_DRSAddSidHistory.
        /// </summary>
        DRS_EXT_ADD_SID_HISTORY = 0x00040000,

        /// <summary>
        /// Unused. MUST be 1 and ignored.
        /// </summary>
        DRS_EXT_POST_BETA3 = 0x00080000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication implementation.
        /// </summary>
        DRS_EXT_GETCHGREQ_V5 = 0x00100000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server implementation of group
        /// expansion.
        /// </summary>
        DRS_EXT_GETMEMBERSHIPS2 = 0x00200000,

        /// <summary>
        /// Unused. This bit was used for a pre-release version of Windows. No released version of Windows
        /// references it. This bit can be set or unset with no change in behavior.
        /// </summary>
        DRS_EXT_GETCHGREQ_V6 = 0x00400000,

        /// <summary>
        /// If present, signifies that the DC supports application NCs.
        /// </summary>
        DRS_EXT_NONDOMAIN_NCS = 0x00800000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication implementation.
        /// </summary>
        DRS_EXT_GETCHGREQ_V8 = 0x01000000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication implementation.
        /// </summary>
        DRS_EXT_GETCHGREPLY_V5 = 0x02000000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication implementation.
        /// </summary>
        DRS_EXT_GETCHGREPLY_V6 = 0x04000000,

        /// <summary>
        /// If present, signifies that the DC supports DRS_MSG_REPVERIFYOBJ, the server-to-server implementation of
        /// creating objects remotely, and a version of the server-to-server replication implementation method.
        /// </summary>
        DRS_EXT_WHISTLER_BETA3 = 0x08000000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication implementation.
        /// </summary>
        DRS_EXT_W2K3_DEFLATE = 0x10000000,

        /// <summary>
        /// If present, signifies that the DC supports a version for the server-to-server replication implementation.
        /// </summary>
        DRS_EXT_GETCHGREQ_V10 = 0x20000000,

        /// <summary>
        /// Unused. MUST be 0 and ignored.
        /// </summary>
        DRS_EXT_RESERVED_FOR_WIN2K_OR_DOTNET_PART2 = 0x40000000,

        /// <summary>
        /// Unused. MUST be 0 and ignored.
        /// </summary>
        DRS_EXT_RESERVED_FOR_WIN2K_OR_DOTNET_PART3 = 0x80000000,
    }


    /// <summary>
    /// this flag describes the capabilities of the DC that produced the DRS_EXTENSIONS_INT structure.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_EXTENSIONS_IN_FLAGSEXT : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If present, signifies that the DC supports DRS_MSG_REPSYNC_V2, DRS_MSG_UPDREFS_V2,
        /// DRS_MSG_INIT_DEMOTIONREQ_V1, DRS_MSG_REPLICA_DEMOTIONREQ_V1, and DRS_MSG_FINISH_DEMOTIONREQ_V1.
        /// </summary>
        DRS_EXT_ADAM = 0x00000001,

        /// <summary>
        /// If present, signifies that the DC supports the DRS_FULL_REP and DRS_SELECT_SECRET_REP flags as
        /// well as InfoLevel 3 in DRS_MSG_DCINFOREQ_V1.
        /// </summary>
        DRS_EXT_LH_BETA2 = 0x00000002,

        /// <summary>
        /// If present, signifies that the DC has enabled the Recycle Bin optional feature.
        /// </summary>
        DRS_EXT_RECYCLE_BIN = 0x00000004,

        /// <summary>
        /// If present, signifies that the DC supports DRS_MSG_GETCHGREPLY_V9.
        /// </summary>
        DRS_EXT_GETCHGREPLY_V9 = 0x00000200
    }


    /// <summary>
    /// DRS_OPTIONS is a concrete type for a set of options sent to and received from various drsuapi methods.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_OPTIONS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Perform the operation asynchronously.
        /// </summary>
        DRS_ASYNC_OP = 0x00000001,

        /// <summary>
        /// Treat ERROR_DS_DRA_REF_NOT_FOUND and ERROR_DS_DRA_REF_ALREADY_EXISTS as success for calls to
        /// IDL_DRSUpdateRefs (section 4.1.26).
        /// </summary>
        DRS_GETCHG_CHECK = 0x00000002,

        /// <summary>
        /// Identifies a call to IDL_DRSReplicaSync that was generated due to a replication notification. 
        /// See [MS-ADTS] section 3.1.1.5.1.5 for more information on replication notifications.
        /// This flag is ignored by the server.
        /// </summary>
        DRS_UPDATE_NOTIFICATION = DRS_GETCHG_CHECK,

        /// <summary>
        /// Register a client DC for notifications of updates to the NC replica.
        /// </summary>
        DRS_ADD_REF = 0x00000004,

        /// <summary>
        /// Replicate from all server DCs.
        /// </summary>
        DRS_SYNC_ALL = 0x00000008,

        /// <summary>
        /// Deregister a client DC from notifications of updates to the NC replica.
        /// </summary>
        DRS_DEL_REF = DRS_SYNC_ALL,

        /// <summary>
        /// Replicate a writable replica, not a read-only partial replica or read-only full replica.
        /// </summary>
        DRS_WRIT_REP = 0x00000010,

        /// <summary>
        /// Perform replication at startup.
        /// </summary>
        DRS_INIT_SYNC = 0x00000020,

        /// <summary>
        /// Perform replication periodically.
        /// </summary>
        DRS_PER_SYNC = 0x00000040,

        /// <summary>
        /// Perform replication using SMTP as a transport.
        /// </summary>
        DRS_MAIL_REP = 0x00000080,

        /// <summary>
        /// Populate the NC replica asynchronously.
        /// </summary>
        DRS_ASYNC_REP = 0x00000100,

        /// <summary>
        /// Ignore errors.
        /// </summary>
        DRS_IGNORE_ERROR = DRS_ASYNC_REP,

        /// <summary>
        /// Inform the server DC to replicate from the client DC.
        /// </summary>
        DRS_TWOWAY_SYNC = 0x00000200,

        /// <summary>
        /// Replicate only system-critical objects.
        /// </summary>
        DRS_CRITICAL_ONLY = 0x00000400,

        /// <summary>
        /// Include updates to ancestor objects before updates to their descendants.
        /// </summary>
        DRS_GET_ANC = 0x00000800,

        /// <summary>
        /// Get the approximate size of the server NC replica.
        /// </summary>
        DRS_GET_NC_SIZE = 0x00001000,

        /// <summary>
        /// Perform the operation locally without contacting any other DC.
        /// </summary>
        DRS_LOCAL_ONLY = DRS_GET_NC_SIZE,

        /// <summary>
        /// Replicate a read-only full replica. Not a writable or partial replica.
        /// </summary>
        DRS_NONGC_RO_REP = 0x00002000,

        /// <summary>
        /// Choose the source server by network name.
        /// </summary>
        DRS_SYNC_BYNAME = 0x00004000,

        /// <summary>
        /// Allow the NC replica to be removed even if other DCs use this DC as a replication server DC.
        /// </summary>
        DRS_REF_OK = DRS_SYNC_BYNAME,

        /// <summary>
        /// Replicate all updates now, even those that would normally be filtered.
        /// </summary>
        DRS_FULL_SYNC_NOW = 0x00008000,

        /// <summary>
        /// The NC replica has no server DCs.
        /// </summary>
        DRS_NO_SOURCE = DRS_FULL_SYNC_NOW,

        /// <summary>
        /// When the flag DRS_FULL_SYNC_NOW is received in a call to IDL_DRSReplicaSync, the flag
        /// DRS_FULL_SYNC_IN_PROGRESS is sent in the associated calls to IDL_DRSGetNCChanges 
        /// until the replication cycle completes. This flag is ignored by the server.
        /// </summary>
        DRS_FULL_SYNC_IN_PROGRESS = 0x00010000,

        /// <summary>
        /// Replicate all updates in the replication request, even those that would normally be filtered.
        /// </summary>
        DRS_FULL_SYNC_PACKET = 0x00020000,

        /// <summary>
        /// This flag is specific to the Microsoft client implementation of IDL_DRSGetNCChanges. 
        /// It is used to identify whether the call was placed in the replicationQueue more than
        /// once due to implementation-specific errors. This flag is ignored by the server.
        /// </summary>
        DRS_SYNC_REQUEUE = 0x00040000,

        /// <summary>
        /// Perform the requested replication immediately; do not wait for any timeouts or delays.
        /// For information about urgent replication, see [MS-ADTS] section 3.1.1.5.1.6.
        /// </summary>
        DRS_SYNC_URGENT = 0x00080000,

        /// <summary>
        /// Requests that the server add an entry to repsTo for the client on the root object of 
        /// the NC replica that is being replicated. When repsTo is set using this flag, the 
        /// notifying client DC contacts the server DC using the service principal name that
        /// begins with "GC" (section 2.2.3.2).
        /// </summary>
        DRS_REF_GCSPN = 0x00100000,

        /// <summary>
        /// This flag is specific to the Microsoft implementation. It identifies when the 
        /// client DC should call the requested IDL_DRSReplicaSync method individually,
        /// without overlapping other outstanding calls to IDL_DRSReplicaSync. This flag 
        /// is ignored by the server.
        /// </summary>
        DRS_NO_DISCARD = 0x00100000,

        /// <summary>
        /// There is no successfully completed replication from this source server.
        /// </summary>
        DRS_NEVER_SYNCED = 0x00200000,

        /// <summary>
        /// Do not replicate attribute values of attributes that contain secret data.
        /// </summary>
        DRS_SPECIAL_SECRET_PROCESSING = 0x00400000,

        /// <summary>
        /// Perform initial replication now.
        /// </summary>
        DRS_INIT_SYNC_NOW = 0x00800000,

        /// <summary>
        /// The replication attempt is preempted by a higher priority replication request.
        /// </summary>
        DRS_PREEMPTED = 0x01000000,

        /// <summary>
        /// Force replication, even if the replication system is otherwise disabled.
        /// </summary>
        DRS_SYNC_FORCED = 0x02000000,

        /// <summary>
        /// Disable replication induced by update notifications.
        /// </summary>
        DRS_DISABLE_AUTO_SYNC = 0x04000000,

        /// <summary>
        /// Disable periodic replication.
        /// </summary>
        DRS_DISABLE_PERIODIC_SYNC = 0x08000000,

        /// <summary>
        /// Compress response messages.
        /// </summary>
        DRS_USE_COMPRESSION = 0x10000000,

        /// <summary>
        /// Do not send update notifications.
        /// </summary>
        DRS_NEVER_NOTIFY = 0x20000000,

        /// <summary>
        /// Expand the partial attribute set of the partial replica.
        /// </summary>
        DRS_SYNC_PAS = 0x40000000,

        /// <summary>
        ///  Replicate all kinds of group membership. If this flag is not
        ///  present nonuniversal group membership will not be replicated.
        /// </summary>
        DRS_GET_ALL_GROUP_MEMBERSHIP = 0x80000000,
    }


    /// <summary>
    /// DRS_MORE_GETCHGREQ_OPTIONS is a concrete type for a set of extra options sent to the IDL_DRSGetNCChanges method.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_MORE_GETCHGREQ_OPTIONS : uint
    {
        /// <summary>
        ///  Unused. MUST be zero and ignored.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Include updates to the target object of a link value before updates to the link value.
        /// </summary>
        DRS_GET_TGT = 0x00000001,
    }


    /// <summary>
    /// the ModifyField values of DRS_MSG_REPMOD
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_MSG_REPMOD_FIELDS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Updates the flags associated with the server.
        /// </summary>
        DRS_UPDATE_FLAGS = 0x00000001,

        /// <summary>
        /// Updates the transport-specific address associated with the server.
        /// </summary>
        DRS_UPDATE_ADDRESS = 0x00000002,

        /// <summary>
        /// Updates the replication schedule associated with the server.
        /// </summary>
        DRS_UPDATE_SCHEDULE = 0x00000004,
    }


    /// <summary>
    /// the flag values of DRS_MSG_CRACKREQ
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_MSG_CRACKREQ_FLAGS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If set, the call fails if the server is not a GC server.
        /// </summary>
        DS_NAME_FLAG_GC_VERIFY = 0x00000004,

        /// <summary>
        /// If set and the lookup fails on the server, referrals are returned to trusted forests
        /// where the lookup might succeed.
        /// </summary>
        DS_NAME_FLAG_TRUST_REFERRAL = 0x00000008,

        /// <summary>
        /// If set and the named object is a foreign security principal, indicate this by using
        /// the status of the lookup operation.
        /// </summary>
        DS_NAME_FLAG_PRIVATE_RESOLVE_FPOS = 0x80000000,
    }


    /// <summary>
    /// The DS_SPN_OPERATION type indicates the operation to perform.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_SPN_OPREATION : uint
    {
        /// <summary>
        /// Adds the specified values to the existing set of SPNs.
        /// </summary>
        DS_SPN_ADD_SPN_OP = 0x00000000,

        /// <summary>
        /// Removes all the existing SPNs, then adds the specified values. If the set of specified values is empty
        /// (cSPN is zero), no value is added.
        /// </summary>
        DS_SPN_REPLACE_SPN_OP = 0x00000001,

        /// <summary>
        /// Removes all the existing SPNs.
        /// </summary>
        DS_SPN_DELETE_SPN_OP = 0x00000002,
    }


    /// <summary>
    /// the flags of DRS_MSG_KCC_EXECUTE
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_MSG_KCC_EXECUTE_FLAGS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Request the KCC to run, then return immediately.
        /// </summary>
        DS_KCC_FLAG_ASYNC_OP = 0x00000001,

        /// <summary>
        /// Request the KCC to run unless there is already such a request pending.
        /// </summary>
        DS_KCC_FLAG_DAMPED = 0x00000002,
    }


    /// <summary>
    /// The response version requested by the client: 1, 2, 3, or 0xFFFFFFFF.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DRS_MSG_DCINFOREQ_INFOLEVEL : uint
    {
        /// <summary>
        /// InfoLevel 1 is a subset of the information at InfoLevel 2 
        /// </summary>
        LEVLE_1 = 1,

        /// <summary>
        /// InfoLevel 2 is a subset of the information at InfoLevel 3. 
        /// </summary>
        LEVEL_2 = 2,

        /// <summary>
        /// InfoLevel 3 includes information about the RODCs in the given domain.
        /// </summary>
        LEVEL_3 = 3,

        /// <summary>
        /// InfoLevel 0xFFFFFFFF server returns information about the active LDAP connections.
        /// </summary>
        LEVEL_F = 0xFFFFFFFF,
    }


    /// <summary>
    /// DS_REPL_INFO codes indicate the type of replication state information being requested.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DS_REPL_INFO : uint
    {
        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_NEIGHBORS = 0x00000000,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_CURSORS_FOR_NC = 0x00000001,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_METADATA_FOR_OBJ = 0x00000002,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_KCC_DSA_CONNECT_FAILURES = 0x00000003,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_KCC_DSA_LINK_FAILURES = 0x00000004,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_PENDING_OPS = 0x00000005,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_METADATA_FOR_ATTR_VALUE = 0x00000006,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_CURSORS_2_FOR_NC = 0x00000007,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_CURSORS_3_FOR_NC = 0x00000008,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_METADATA_2_FOR_OBJ = 0x00000009,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_METADATA_2_FOR_ATTR_VALUE = 0x0000000A,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_SERVER_OUTGOING_CALLS = 0xFFFFFFFA,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_UPTODATE_VECTOR_V1 = 0xFFFFFFFB,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_CLIENT_CONTEXTS = 0xFFFFFFFC,

        /// <summary>
        /// possible value
        /// </summary>
        DS_REPL_INFO_REPSTO = 0xFFFFFFFE,
    }


    /// <summary>
    /// the flag values of DRS_MSG_GETREPLINFO_REQ
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_MSG_GETREPLINFO_REQ_FLAGS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Return attribute stamps for linked values.
        /// </summary>
        DS_REPL_INFO_FLAG_IMPROVE_LINKED_ATTRS = 0x00000001,
    }


    /// <summary>
    /// The DRS_ADDSID_FLAGS type consists of bit flags that indicate how the SID is to be added to the security
    /// principal.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_ADDSID_FLAGS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// If set, the server should verify whether the client connection is secure and should return the result
        /// of the verification in the response.
        /// </summary>
        DS_ADDSID_FLAG_PRIVATE_CHK_SECURE = 0x40000000,

        /// <summary>
        /// If set, the server should append the objectSid and sIDHistory attributes of SrcPrincipal to the
        /// sIDHistory attribute of DstPrincipal, and should delete SrcPrincipal from the source domain.
        /// </summary>
        DS_ADDSID_FLAG_PRIVATE_DEL_SRC_OBJ = 0x80000000,
    }


    /// <summary>
    /// the option values of DRS_MSG_REPVERIFYOBJ
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [CLSCompliant(false)]
    public enum DRS_MSG_REPVERIFYOBJ_OPTIONS : uint
    {
        /// <summary>
        /// to expunge each object that is not verified.
        /// </summary>
        EXPUNGE = 0,

        /// <summary>
        /// to log an event that identifies each such object.
        /// </summary>
        LOG = 1,
    }


    /// <summary>
    /// the flag values of DRS_MSG_REPLICA_DEMOTIONREQ
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// MUST be set.
        /// </summary>
        DS_REPLICA_DEMOTE_TRY_ALL_SRCS = 0x00000001,
    }


    /// <summary>
    /// the operation values of DRS_MSG_FINISH_DEMOTIONREQ
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    [CLSCompliant(false)]
    public enum DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS : uint
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Undo the effects of IDL_DRSInitDemotion. If present, any other flags present are ignored.
        /// </summary>
        DS_DEMOTE_ROLLBACK_DEMOTE = 0x00000001,

        /// <summary>
        /// Mark the DC's database "demotion-complete" (this effect is outside the state model).
        /// </summary>
        DS_DEMOTE_COMMIT_DEMOTE = 0x00000002,

        /// <summary>
        /// Delete the nTDSDSA object for this DC; see RemoveADLDSServer (section 4.1.6.2.1).
        /// </summary>
        DS_DEMOTE_DELETE_CSMETA = 0x00000004,

        /// <summary>
        /// Delete any serviceConnectionPoint objects for this DC from AD DS; see RemoveADLDSSCP (section 4.1.6.2.2).
        /// </summary>
        DS_DEMOTE_UNREGISTER_SCPS = 0x00000008,

        /// <summary>
        /// Delete any AD LDS SPNs from the service account object in AD DS; see RemoveADLDSSPNs (section 4.1.6.2.3).
        /// </summary>
        DS_DEMOTE_UNREGISTER_SPNS = 0x00000010,

        /// <summary>
        /// Fail the request if the dwOperations field contains an unknown flag.
        /// </summary>
        DS_DEMOTE_OPT_FAIL_ON_UNKNOWN_OP = 0x80000000,
    }


    /// <summary>
    /// DRSR RPC interface type, DRSUAPI or DSAOP.
    /// </summary>
    public enum DrsrRpcInterfaceType
    {
        /// <summary>
        /// None
        /// </summary>
        NONE,

        /// <summary>
        /// RPC interface for drsuapi methods
        /// </summary>
        DRSUAPI,

        /// <summary>
        /// RPC interface for dsaop methods
        /// </summary>
        DSAOP
    }
}
