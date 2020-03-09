// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    ///  An ACCESS_MASK is a 32-bit set of flags that are used
    ///  to encode the user rights to an object.  An access
    ///  mask is used both for encoding the rights to an object
    ///  assigned to a principal and for encoding the desired
    ///  access when opening an object. The lower 16 bits are
    ///  used for object-specific user rights. A file object
    ///  would encode, for example, Read Access, Write Access,
    ///  and so forth. A registry key object would encode Create
    ///  Subkey, Read Value, etc.The upper 16 bits are user
    ///  rights that are common to all objects, or are generic
    ///  rights that can be mapped to object-specific user rights
    ///  by the object itself.
    /// </summary>
    public partial struct _ACCESS_MASK
    {

        /// <summary>
        ///   </summary>
        public uint ACCESS_MASK;

        public static _ACCESS_MASK Zero
        {
            get
            {
                return new _ACCESS_MASK { ACCESS_MASK = 0 };
            }
        }
    }

    /// <summary>
    ///  The SMB2 TREE_CONNECT Response packet is sent by the
    ///  server when an SMB2 TREE_CONNECT request is processed
    ///  successfully by the server. The server MUST set the
    ///  TreeId  of the newly created tree connect in the SMB
    ///  2.0 Protocol header of the response. This response
    ///  is composed of an SMB2 Packet Header that is followed
    ///  by this response structure:
    /// </summary>
    public partial struct TREE_CONNECT_Response
    {

        /// <summary>
        ///  The server MUST set this field to 16, indicating the
        ///  size of the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The type of share being accessed. This field MUST contain
        ///  one of the following values:
        /// </summary>
        public ShareType_Values ShareType;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        public byte Reserved;

        /// <summary>
        ///  The offline caching properties for this share.  For
        ///  more information, see [OFFLINE]. This field MUST contain
        ///  one of the following values:
        /// </summary>
        [StaticSize(4)]
        public ShareFlags_Values ShareFlags;

        /// <summary>
        ///  Indicates various capabilities for this share. This
        ///  field MUST be constructed by using the following values:
        /// </summary>
        [StaticSize(4)]
        public Share_Capabilities_Values Capabilities;

        /// <summary>
        ///  Contains the maximal access for the user that establishes
        ///  the tree connect on the share based on the share's
        ///  permissions. This value takes the form as specified
        ///  in section.
        /// </summary>
        [StaticSize(4)]
        public _ACCESS_MASK MaximalAccess;
    }

    /// <summary>
    /// ShareType_Values
    /// </summary>
    public enum ShareType_Values : byte
    {
        /// <summary>
        ///  Physical disk share.
        /// </summary>
        SHARE_TYPE_DISK = 0x01,

        /// <summary>
        ///  Named pipe share.
        /// </summary>
        SHARE_TYPE_PIPE = 0x02,

        /// <summary>
        ///  Printer share.
        /// </summary>
        SHARE_TYPE_PRINT = 0x03,
    }

    /// <summary>
    /// The Flags field in TreeConnect_request, which indicates how to process the operation
    /// </summary>
    [Flags()]
    public enum TreeConnect_Flags : ushort
    {
        SMB2_SHAREFLAG_NONE = 0x0000,
        /// <summary>
        /// When set, indicates that the client has previously connected to the specified cluster share
        /// using the SMB dialect of the connection on which the request is received.
        /// </summary>
        SMB2_SHAREFLAG_CLUSTER_RECONNECT = 0x0001,

        /// <summary>
        /// When set, indicates that the client can handle synchronous share redirects
        /// via a Share Redirect error context response as specified in section 2.2.2.2.2.
        /// </summary>
        SMB2_SHAREFLAG_REDIRECT_TO_OWNER = 0x0002,

        /// <summary>
        /// When set, indicates that a tree connect request extension,
        /// as specified in section 2.2.9.1, is present,
        /// starting at the Buffer field of this tree connect request.
        /// </summary>
        SMB2_SHAREFLAG_EXTENSION_PRESENT = 0x0004,
    }

    /// <summary>
    /// ShareFlags_Values
    /// </summary>
    [Flags()]
    public enum ShareFlags_Values : uint
    {
        /// <summary>
        ///  The client MAY cache files that are explicitly selected
        ///  by the user for offline use.
        /// </summary>
        SHAREFLAG_MANUAL_CACHING = 0x00000000,

        /// <summary>
        ///  The client MAY automatically cache files that are used
        ///  by the user for offline access.
        /// </summary>
        SHAREFLAG_AUTO_CACHING = 0x00000010,

        /// <summary>
        ///  The client MAY automatically cache files that are used
        ///  by the user for offline access, and MAY use those files
        ///  in an offline mode even if the share is available.
        /// </summary>
        SHAREFLAG_VDO_CACHING = 0x00000020,

        /// <summary>
        ///  Offline caching MUST NOT occur.
        /// </summary>
        SHAREFLAG_NO_CACHING = 0x00000030,

        /// <summary>
        ///  The specified share is present in a DFS tree structure.
        /// </summary>
        SHAREFLAG_DFS = 0x00000001,

        /// <summary>
        ///  The specified share is the root volume in a DFS tree
        ///  structure.
        /// </summary>
        SHAREFLAG_DFS_ROOT = 0x00000002,

        /// <summary>
        ///  The specified share disallows exclusive file opens that
        ///  deny reads to an open file.
        /// </summary>
        SHAREFLAG_RESTRICT_EXCLUSIVE_OPENS = 0x00000100,

        /// <summary>
        ///  Shared files in the specified share can be forcibly
        ///  deleted.
        /// </summary>
        SHAREFLAG_FORCE_SHARED_DELETE = 0x00000200,

        /// <summary>
        ///  Clients are allowed to cache the namespace of the specified
        ///  share.
        /// </summary>
        SHAREFLAG_ALLOW_NAMESPACE_CACHING = 0x00000400,

        /// <summary>
        ///  The server will filter directory entries based on the
        ///  access permissions of the client.
        /// </summary>
        SHAREFLAG_ACCESS_BASED_DIRECTORY_ENUM = 0x00000800,

        /// <summary>
        /// The server will not issue exclusive caching rights on this share.
        /// </summary>
        SHAREFLAG_FORCE_LEVELII_OPLOCK = 0x00001000,

        /// <summary>
        /// The share supports hash generation for branch cache retrieval of data.
        /// </summary>
        SHAREFLAG_ENABLE_HASH_V1 = 0x00002000,

        /// <summary>
        /// The share supports v2 hash generation for branch cache retrieval of data.
        /// </summary>
        SHAREFLAG_ENABLE_HASH_V2 = 0x00004000,

        /// <summary>
        /// If set, the server supports the encryption of remote file access messages on this share. 
        /// </summary>
        SHAREFLAG_ENCRYPT_DATA = 0x00008000,

        /// <summary>
        /// If set, the share supports identity remoting.
        /// The client can request remoted identity access for the share via the SMB2_REMOTED_IDENTITY_TREE_CONNECT context
        /// as specified in section 2.2.9.2.1.
        /// </summary>
        SHAREFLAG_IDENTITY_REMOTING = 0x00040000,
    }

    /// <summary>
    /// Capabilities_Values
    /// </summary>
    [Flags()]
    public enum Capabilities_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates that the client supports the Distributed
        ///  File System (DFS).
        /// </summary>
        GLOBAL_CAP_DFS = 0x00000001,

        /// <summary>
        /// When set, indicates that the client supports leasing.
        /// </summary>
        GLOBAL_CAP_LEASING = 0x00000002,

        /// <summary>
        /// When set, indicates that the client supports multi-credit operations.
        /// </summary>
        GLOBAL_CAP_LARGE_MTU = 0x00000004,

        /// <summary>
        /// When set, indicates that the client supports establishing multiple channels for a single session.
        /// </summary>
        GLOBAL_CAP_MULTI_CHANNEL = 0x00000008,

        /// <summary>
        /// When set, indicates that the client supports persistent handles.
        /// </summary>
        GLOBAL_CAP_PERSISTENT_HANDLES = 0x00000010,

        /// <summary>
        /// When set, indicates that the client supports directory leasing.
        /// </summary>
        GLOBAL_CAP_DIRECTORY_LEASING = 0x00000020,

        /// <summary>
        /// When set, indicates that the client supports encryption.
        /// </summary>
        GLOBAL_CAP_ENCRYPTION = 0x00000040,

    }

    /// <summary>
    /// Capabilities values for share in TREE_CONNECT response
    /// </summary>
    [Flags()]
    public enum Share_Capabilities_Values : uint
    {
        NONE = 0,

        /// <summary>
        /// The specified share is present in a DFS tree structure. 
        /// The server MUST set the SMB2_SHARE_CAP_DFS bit in the Capabilities field if the per-share property Share.IsDfs is TRUE.
        /// </summary>
        SHARE_CAP_DFS = 0x00000008,

        /// <summary>
        /// The specified share is continuously available. This flag is only valid for the SMB 3.0 dialect.
        /// </summary>
        SHARE_CAP_CONTINUOUS_AVAILABILITY = 0x00000010,

        /// <summary>
        /// The specified share is present on a server configuration 
        /// which facilitates faster recovery of durable handles. 
        /// This flag is only valid for the SMB 3.0 dialect.
        /// </summary>
        SHARE_CAP_SCALEOUT = 0x00000020,

        /// <summary>
        /// The specified share is present on a server configuration 
        /// which provides monitoring of the availability of share through the Witness service specified in [MS-SWN]. 
        /// This flag is only valid for the SMB 3.0 dialect.
        /// </summary>
        SHARE_CAP_CLUSTER = 0x00000040,

        /// <summary>
        ///  The specified share is present on a server configuration that allows dynamic changes in the ownership 
        ///  of the share. This flag is only valid for the SMB 3.02 dialect.
        /// </summary>
        SHARE_CAP_ASYMMETRIC = 0x00000080,

        /// <summary>
        /// The specified share is present on a server configuration that supports synchronous share level redirection
        /// via a Share Redirect error context response (section 2.2.2.2.2).
        /// This flag is not valid for SMB 2.0.2, 2.1, 3.0, and 3.0.2 dialects.
        /// </summary>
        SHARE_CAP_REDIRECT_TO_OWNER = 0x00000100,
    }

    /// <summary>
    ///  The SMB2 TREE_DISCONNECT Request packet is sent by the
    ///  client to request that the tree connect that is specified
    ///  in the TreeId within the SMB2 header be disconnected.
    ///   This request is composed of an SMB2 header, as specified
    ///  in section , that is followed by this variable-length
    ///  request structure:
    /// </summary>
    public partial struct TREE_DISCONNECT_Request
    {

        /// <summary>
        ///  The client MUST set this field to 4, indicating the
        ///  size of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public TREE_DISCONNECT_Request_Reserved_Values Reserved;
    }

    /// <summary>
    /// TREE_DISCONNECT_Request_Reserved_Values
    /// </summary>
    public enum TREE_DISCONNECT_Request_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 TREE_DISCONNECT Response packet is sent by
    ///  the server to confirm that an SMB2 TREE_DISCONNECT
    ///  Request was successfully processed. This response
    ///  is composed of an SMB2 header, as specified in section
    ///  , that is followed by this request structure:
    /// </summary>
    public partial struct TREE_DISCONNECT_Response
    {

        /// <summary>
        ///    The server MUST set this field to 4, indicating the
        ///  size of the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;
    }

    /// <summary>
    ///  When the server receives an SMB2 CREATE Request, it
    ///  MUST create a file if the file does not exist; otherwise,
    ///  it MUST open the existing file. In case of a named
    ///  pipe or printer, the server MUST create a new file.This
    ///  request is composed of an SMB2 header, as specified
    ///  in section , that is followed by this request structure:
    /// </summary>
    public partial struct CREATE_Request
    {

        /// <summary>
        ///  The client MUST set this field  to 57, indicating the
        ///  size of the request structure, not including the header.
        ///   The client MUST set it to this value regardless of
        ///  how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The Quality of Service (QoS) security flags. This field
        ///  MUST be constructed by using the following values:
        /// </summary>
        public SecurityFlags_Values SecurityFlags;

        /// <summary>
        ///  The requested oplock level. This field MUST contain
        ///  one of the following values:-based clients never use
        ///  exclusive oplocks.  Because there are no situations
        ///  where the client would want an exclusive oplock  where
        ///  it would not also want an SMB2_OPLOCK_LEVEL_BATCH,
        ///  it always requests an SMB2_OPLOCK_LEVEL_BATCH.
        /// </summary>
        public RequestedOplockLevel_Values RequestedOplockLevel;

        /// <summary>
        ///  This field specifies the impersonation level of the
        ///  application that is issuing the create request. This
        ///  field MUST contain one of the following values:
        /// </summary>
        [StaticSize(4)]
        public ImpersonationLevel_Values ImpersonationLevel;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The client MAY set this to any value, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public CREATE_Request_SmbCreateFlags SmbCreateFlags;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The client MAY set this to any value, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong Reserved;

        /// <summary>
        ///  The level of access that is wanted, as specified in
        ///  section.
        /// </summary>
        [StaticSize(4)]
        public AccessMask DesiredAccess;

        /// <summary>
        ///  The attributes of the file being created, as specified
        ///  in [MS-FSCC] section 2.6. This field is used by the
        ///  server while creating a new file or named_pipe. For
        ///  a printer, all attributes except FILE_ATTRIBUTE_DIRECTORY
        ///  are valid.
        /// </summary>
        [StaticSize(4)]
        public File_Attributes FileAttributes;

        /// <summary>
        ///  Specifies the sharing mode for the open. This field
        ///  MUST be constructed by using the following values:
        /// </summary>
        [StaticSize(4)]
        public ShareAccess_Values ShareAccess;

        /// <summary>
        ///  Defines the action the server MUST take if the file
        ///  that is specified in the name field already exists.
        ///   For opening named pipes, this field MUST be ignored.
        ///  For other files, this field MUST contain one of the
        ///  following values:
        /// </summary>
        [StaticSize(4)]
        public CreateDisposition_Values CreateDisposition;

        /// <summary>
        ///  Specifies the options to be applied when creating or
        ///  opening the file.  Combinations of the bit positions
        ///  listed below are valid, unless otherwise noted. This
        ///  field MUST be constructed by using the following values:
        /// </summary>
        [StaticSize(4)]
        public CreateOptions_Values CreateOptions;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the file name. The file name is relative
        ///  to the share that is found by the TreeId  in the SMB2
        ///  header.  If no name is provided, indicating an open
        ///  of the root directory of the share, this field MUST
        ///  be 0.
        /// </summary>
        [StaticSize(2)]
        public ushort NameOffset;

        /// <summary>
        ///  The length of the file name, in bytes.  Each individual
        ///  file name component MUST NOT exceed 255 Unicode characters
        ///  and the full path name MUST NOT exceed 32760 Unicode
        ///  characters. If no file name is provided, this field
        ///  MUST be set to 0.
        /// </summary>
        [StaticSize(2)]
        public ushort NameLength;

        /// <summary>
        ///  The offset, in bytes, to the first SMB2_CREATE_CONTEXT
        ///  structure in the request.  If no SMB2_CREATE_CONTEXTs
        ///  are being sent, this value MUST be 0.
        /// </summary>
        [StaticSize(4)]
        public uint CreateContextsOffset;

        /// <summary>
        ///  The length, in bytes, of all the SMB2_CREATE_CONTEXT
        ///  structures that are sent in this request.
        /// </summary>
        [StaticSize(4)]
        public uint CreateContextsLength;
    }

    [Flags]
    public enum CREATE_Request_SmbCreateFlags : ulong
    {
        SMB2_CREATE_FLAG_REPLAY = 0x0000000000000001
    }

    /// <summary>
    /// SecurityFlags_Values
    /// </summary>
    [Flags()]
    public enum SecurityFlags_Values : byte
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates that the security tracking mode
        ///  is dynamic.  When not set, indicates that the security
        ///  tracking mode is static.
        /// </summary>
        SECURITY_DYNAMIC_TRACKING = 0x01,

        /// <summary>
        ///  When set, indicates that only the enabled aspects of
        ///  the client security context are available to the server.
        /// </summary>
        SECURITY_EFFECTIVE_ONLY = 0x02,
    }

    /// <summary>
    /// RequestedOplockLevel_Values
    /// </summary>
    public enum RequestedOplockLevel_Values : byte
    {

        /// <summary>
        ///  No oplock is requested.
        /// </summary>
        OPLOCK_LEVEL_NONE = 0x00,

        /// <summary>
        ///  A level II oplock is requested.
        /// </summary>
        OPLOCK_LEVEL_II = 0x01,

        /// <summary>
        ///  An exclusive oplock is requested.
        /// </summary>
        OPLOCK_LEVEL_EXCLUSIVE = 0x08,

        /// <summary>
        ///  A batch oplock is requested.
        /// </summary>
        OPLOCK_LEVEL_BATCH = 0x09,

        /// <summary>
        /// A lease is requested. If set, the request packet MUST contain an 
        /// SMB2_CREATE_REQUEST_LEASE create context. This value is only valid 
        /// for the SMB 2.1 dialect.
        /// </summary>
        OPLOCK_LEVEL_LEASE = 0xFF,
    }

    /// <summary>
    /// ImpersonationLevel_Values
    /// </summary>
    public enum ImpersonationLevel_Values : uint
    {

        /// <summary>
        ///  The client is anonymous to the server. The server process
        ///  MAY impersonate the client, but the impersonation token
        ///  MUST NOT contain any information about the client.
        ///  The server process MUST NOT obtain identification
        ///  information about the client.
        /// </summary>
        Anonymous = 0x00000000,

        /// <summary>
        ///  The server MAY obtain the client's identity, and the
        ///  server MAY impersonate the client to perform access
        ///  control list (ACL) checks.
        /// </summary>
        Identification = 0x00000001,

        /// <summary>
        ///  The server MAY impersonate the client's security context
        ///  while acting on behalf of the client. The server MAY
        ///  access local resources as the client.
        /// </summary>
        Impersonation = 0x00000002,

        /// <summary>
        ///  The server MAY impersonate the client's security context
        ///  while acting on behalf of the client.  During impersonation,
        ///  the client's credentials (both local and network) MAY
        ///  be passed to any number of machines.
        /// </summary>
        Delegate = 0x00000003,
    }

    /// <summary>
    /// ShareAccess_Values
    /// </summary>
    [Flags()]
    public enum ShareAccess_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates that other opens are allowed to
        ///  read this file while this open is present.   This bit
        ///  must not be set for a named pipe or a printer file.
        ///  Each open creates a new instance of a named pipe. 
        ///  Likewise, opening a printer file always creates a new
        ///  file.
        /// </summary>
        FILE_SHARE_READ = 0x00000001,

        /// <summary>
        ///  When set, indicates that other opens are allowed to
        ///  write this file while this open is present.    This
        ///  bit must not be set for a named pipe or a printer file.
        ///  Each open creates a new instance of a named pipe. 
        ///  Likewise, opening a printer file always creates a new
        ///  file.
        /// </summary>
        FILE_SHARE_WRITE = 0x00000002,

        /// <summary>
        ///  When set, indicates that other opens are allowed to
        ///  delete or rename this file while this open is present.
        ///       This bit must not be set for a named pipe or a
        ///  printer file. Each open creates a new instance of a
        ///  named pipe.  Likewise, opening a printer file always
        ///  creates a new file.
        /// </summary>
        FILE_SHARE_DELETE = 0x00000004,
    }

    /// <summary>
    /// CreateDisposition_Values
    /// </summary>
    public enum CreateDisposition_Values : uint
    {

        /// <summary>
        ///  If the file already exists, supersede it by the specified
        ///  file.  Otherwise, create the file.
        /// </summary>
        FILE_SUPERSEDE = 0x00000000,

        /// <summary>
        ///  If the file already exists, return success; otherwise,
        ///  fail the operation. MUST NOT be set for a printer object.
        /// </summary>
        FILE_OPEN = 0x00000001,

        /// <summary>
        ///  If the file already exists, fail the operation; otherwise,
        ///  create the file.
        /// </summary>
        FILE_CREATE = 0x00000002,

        /// <summary>
        ///  Open the file if it already exists; otherwise, create
        ///  the file.
        /// </summary>
        FILE_OPEN_IF = 0x00000003,

        /// <summary>
        ///  Overwrite the file if it already exists; otherwise,
        ///  fail the operation. MUST NOT be set for a printer object.
        /// </summary>
        FILE_OVERWRITE = 0x00000004,

        /// <summary>
        ///  Overwrite the file if it already exists; otherwise,
        ///  create the file.
        /// </summary>
        FILE_OVERWRITE_IF = 0x00000005,
    }

    /// <summary>
    /// CreateOptions_Values
    /// </summary>
    [Flags()]
    public enum CreateOptions_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The file being created or opened is a directory file.
        ///   With this flag, the CreateDisposition   field MUST
        ///  be set to FILE_CREATE or FILE_OPEN_IF.  With this flag,
        ///  only the following CreateOptions values are valid:
        ///  FILE_WRITE_THROUGH, and FILE_OPEN_FOR_BACKUP_INTENT.
        /// </summary>
        FILE_DIRECTORY_FILE = 0x00000001,

        /// <summary>
        ///  The server MUST propagate writes to this open to persistent
        ///  storage before returning success to the client on write
        ///  operations.
        /// </summary>
        FILE_WRITE_THROUGH = 0x00000002,

        /// <summary>
        ///  A hint indicating that accesses to the file will be
        ///  sequential. This flag value is incompatible with the
        ///  FILE_RANDOM_ACCESS value, which indicates that the
        ///  accesses to the file can be random.
        /// </summary>
        FILE_SEQUENTIAL_ONLY = 0x00000004,

        /// <summary>
        ///  The server or underlying object store SHOULD NOT cache
        ///  data at intermediate layers and SHOULD allow it to
        ///  flow through to persistent storage.
        /// </summary>
        FILE_NO_INTERMEDIATE_BUFFERING = 0x00000008,

        /// <summary>
        ///  This bit SHOULD be set to 0 and MUST be ignored by the server.
        /// </summary>
        FILE_SYNCHRONOUS_IO_ALERT = 0x00000010,

        /// <summary>
        ///  This bit SHOULD be set to 0 and MUST be ignored by the server.
        /// </summary>
        FILE_SYNCHRONOUS_IO_NONALERT = 0x00000020,

        /// <summary>
        ///  The file being opened MUST NOT be a directory file or
        ///  this call MUST be failed. This flag MUST NOT be used
        ///  with FILE_DIRECTORY_FILE.
        /// </summary>
        FILE_NON_DIRECTORY_FILE = 0x00000040,

        /// <summary>
        /// This bit SHOULD be set to 0 and MUST be ignored by the server.
        /// </summary>
        FILE_COMPLETE_IF_OPLOCKED = 0x00000100,

        /// <summary>
        ///  If the extended attributes on an existing file being
        ///  opened indicate that the caller must understand extended
        ///  attributes (EAs) to properly interpret the file, the
        ///  server MUST fail this request because the caller does
        ///  not understand how to deal with EAs.
        /// </summary>
        FILE_NO_EA_KNOWLEDGE = 0x00000200,

        /// <summary>
        /// This indicates that the application intends to read or write at random 
        /// offsets using this handle, so the server SHOULD optimize for random access.
        /// However, the server MUST accept any access pattern. This flag value is 
        /// incompatible with the FILE_SEQUENTIAL_ONLY value. If both FILE_RANDOM_ACCESS
        /// and FILE_SEQUENTIAL_ONLY are set, then FILE_SEQUENTIAL_ONLY is ignored.
        /// </summary>
        FILE_RANDOM_ACCESS = 0x00000800,

        /// <summary>
        /// The file MUST be automatically deleted when the last open request on this file
        /// is closed. When the flag is set, it is implicitly assumed that the caller is 
        /// requesting DELETE access to the file even if the DELETE flag in the DesiredAccess
        /// field is not set by the caller.
        /// </summary>
        FILE_DELETE_ON_CLOSE = 0x00001000,

        /// <summary>
        /// This bit SHOULD be set to 0 and the server MUST fail the request 
        /// with a STATUS_INVALID_PARAMETER error if this bit is set
        /// </summary>
        FILE_OPEN_BY_FILE_ID = 0x00002000,

        /// <summary>
        ///  The file is being opened for backup intent.  That is,
        ///  it is being opened or created for the purposes of either
        ///  a backup or a restore operation. Thus, the server MAY
        ///  make appropriate checks to ensure that the caller is
        ///  capable of overriding whatever security checks have
        ///  been placed on the file to allow a backup or restore
        ///  operation to occur. The server MAY choose to check
        ///  for certain access rights to the file before checking
        ///  the DesiredAccess field.
        /// </summary>
        FILE_OPEN_FOR_BACKUP_INTENT = 0x00004000,

        /// <summary>
        /// The file cannot be compressed.
        /// </summary>
        FILE_NO_COMPRESSION = 0x00008000,

        /// <summary>
        /// This bit SHOULD be set to 0 and the server MUST fail the
        /// request with a STATUS_INVALID_PARAMETER error if this bit is set
        /// </summary>
        FILE_RESERVE_OPFILTER = 0x00100000,

        /// <summary>
        /// If the file or directory being opened is a reparse point,
        /// open the reparse point itself rather than the target that
        /// the reparse point references.
        /// </summary>
        FILE_OPEN_REPARSE_POINT = 0x00200000,

        /// <summary>
        /// In an HSM (Hierarchical Storage Management) environment, this flag means
        /// the file should not be recalled from tertiary storage such as tape. 
        /// The recall can take several minutes. The caller can specify this flag
        /// to avoid those delays
        /// </summary>
        FILE_OPEN_NO_RECALL = 0x00400000,

        /// <summary>
        /// Open file to query for free space. The client SHOULD set this to 0 
        /// and the server MUST ignore it.
        /// </summary>
        FILE_OPEN_FOR_FREE_SPACE_QUERY = 0x00800000,
    }

    /// <summary>
    ///  The SMB2 Access Mask Encoding in SMB2 is a 4-byte bit
    ///  field value that contains an array of flags.  Each
    ///  access mask MUST be a combination of zero or more of
    ///  the bit positions that are shown below. 
    /// </summary>
    public partial struct Access_Mask_Encoding
    {

        /// <summary>
        ///  For a file, pipe, or printer, the value MUST be constructed
        ///  by using the following values (for a printer, the value
        ///  MUST have at least one of the following: FILE_WRITE_DATA,
        ///  FILE_APPEND_DATA, or GENERIC_WRITE).
        /// </summary>
        public AccessMask File_Pipe_Printer_Access_Mask;

        /// <summary>
        ///  For a directory, the value MUST be constructed by using
        ///  the following values:
        /// </summary>
        public Directory_Access_Mask_Values Directory_Access_Mask;
    }

    /// <summary>
    /// File_Pipe_Printer_Access_Mask_Values
    /// </summary>
    [Flags()]
    public enum AccessMask : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  This value indicates the right to read data from the
        ///  file or named pipe.
        /// </summary>
        FILE_READ_DATA = 0x00000001,

        /// <summary>
        ///  This value indicates the right to write data into the
        ///  file or named pipe.
        /// </summary>
        FILE_WRITE_DATA = 0x00000002,

        /// <summary>
        ///  This value indicates the right to write data into the
        ///  file or named pipe beyond its current file size.
        /// </summary>
        FILE_APPEND_DATA = 0x00000004,

        /// <summary>
        ///  This value indicates the right to read the extended
        ///  attributes of the file or named pipe.
        /// </summary>
        FILE_READ_EA = 0x00000008,

        /// <summary>
        ///  This value indicates the right to write or change the
        ///  extended attributes to the file or named pipe.
        /// </summary>
        FILE_WRITE_EA = 0x00000010,

        /// <summary>
        ///  This value indicates the right to execute the file.
        /// </summary>
        FILE_EXECUTE = 0x00000020,

        /// <summary>
        ///  This value indicates the right to read the attributes
        ///  of the file.
        /// </summary>
        FILE_READ_ATTRIBUTES = 0x00000080,

        /// <summary>
        ///  This value indicates the right to change the attributes
        ///  of the file.
        /// </summary>
        FILE_WRITE_ATTRIBUTES = 0x00000100,

        /// <summary>
        ///  This value indicates the right to delete the file.
        /// </summary>
        DELETE = 0x00010000,

        /// <summary>
        ///  This value indicates the right to read the security
        ///  descriptor for the file or named pipe.
        /// </summary>
        READ_CONTROL = 0x00020000,

        /// <summary>
        ///  This value indicates the right to change the discretionary
        ///  access control list (DACL) in the security descriptor
        ///  for the file or named pipe.  For the DACL data structure,
        ///  see ACL in [MS-DTYP].
        /// </summary>
        WRITE_DAC = 0x00040000,

        /// <summary>
        ///  This value indicates the right to change the owner in
        ///  the security descriptor for the file or named pipe.
        /// </summary>
        WRITE_OWNER = 0x00080000,

        /// <summary>
        ///  This value SHOULD be set to 0 by the sender and MUST
        ///  be ignored by the receiver.
        /// </summary>
        SYNCHRONIZE = 0x00100000,

        /// <summary>
        ///  This value indicates the right to read or change the
        ///  system access control list (SACL) in the security descriptor
        ///  for the file or named pipe.  For  the SACL data structure,
        ///  see ACL in [MS-DTYP].
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0x01000000,

        /// <summary>
        ///  This value indicates that the client is requesting an
        ///  open to the file with the highest level of access the
        ///  client has on this file.  If no access is granted for
        ///  the client on this file, the server MUST fail the open
        ///  with STATUS_ACCESS_DENIED.
        /// </summary>
        MAXIMAL_ACCESS = 0x02000000,

        /// <summary>
        ///  This value indicates a request for all the access flags
        ///  that are previously listed except MAXIMAL_ACCESS and
        ///  ACCESS_SYSTEM_SECURITY.
        /// </summary>
        GENERIC_ALL = 0x10000000,

        /// <summary>
        ///  This value indicates a request for the following combination
        ///  of access flags listed above: FILE_READ_ATTRIBUTES|
        ///  FILE_EXECUTE| SYNCHRONIZE| READ_CONTROL.
        /// </summary>
        GENERIC_EXECUTE = 0x20000000,

        /// <summary>
        ///  This value indicates a request for the following combination
        ///  of access flags listed above: FILE_WRITE_DATA| FILE_APPEND_DATA|
        ///   FILE_WRITE_ATTRIBUTES| FILE_WRITE_EA| SYNCHRONIZE|
        ///  READ_CONTROL.
        /// </summary>
        GENERIC_WRITE = 0x40000000,

        /// <summary>
        ///  This value indicates a request for the following combination
        ///  of access flags listed above: FILE_READ_DATA| FILE_READ_ATTRIBUTES|
        ///  FILE_READ_EA| SYNCHRONIZE| READ_CONTROL.
        /// </summary>
        GENERIC_READ = 0x80000000,


        /// <summary>
        ///  This value indicates the right to enumerate the contents
        ///  of the directory.
        /// </summary>
        FILE_LIST_DIRECTORY = 0x00000001,

        /// <summary>
        ///  This value indicates the right to create a file under
        ///  the directory.
        /// </summary>
        FILE_ADD_FILE = 0x00000002,

        /// <summary>
        ///  This value indicates the right to add a sub-directory
        ///  under the directory.
        /// </summary>
        FILE_ADD_SUBDIRECTORY = 0x00000004,

        // The following are for directory
        /// <summary>
        ///  This value indicates the right to traverse this directory
        ///  if the server enforces traversal checking.
        /// </summary>
        FILE_TRAVERSE = 0x00000020,

        /// <summary>
        ///  This value indicates the right to delete the files and
        ///  directories within this directory.
        /// </summary>
        FILE_DELETE_CHILD = 0x00000040,
    }

    /// <summary>
    /// Directory_Access_Mask_Values
    /// </summary>
    [Flags()]
    public enum Directory_Access_Mask_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  This value indicates the right to enumerate the contents
        ///  of the directory.
        /// </summary>
        FILE_LIST_DIRECTORY = 0x00000001,

        /// <summary>
        ///  This value indicates the right to create a file under
        ///  the directory.
        /// </summary>
        FILE_ADD_FILE = 0x00000002,

        /// <summary>
        ///  This value indicates the right to add a sub-directory
        ///  under the directory.
        /// </summary>
        FILE_ADD_SUBDIRECTORY = 0x00000004,

        /// <summary>
        ///  This value indicates the right to read the extended
        ///  attributes of the directory.
        /// </summary>
        FILE_READ_EA = 0x00000008,

        /// <summary>
        ///  This value indicates the right to write or change the
        ///  extended attributes of the directory.
        /// </summary>
        FILE_WRITE_EA = 0x00000010,

        /// <summary>
        ///  This value indicates the right to traverse this directory
        ///  if the server enforces traversal checking.
        /// </summary>
        FILE_TRAVERSE = 0x00000020,

        /// <summary>
        ///  This value indicates the right to delete the files and
        ///  directories within this directory.
        /// </summary>
        FILE_DELETE_CHILD = 0x00000040,

        /// <summary>
        ///  This value indicates the right to read the attributes
        ///  of the directory.
        /// </summary>
        FILE_READ_ATTRIBUTES = 0x00000080,

        /// <summary>
        ///  This value indicates the right to change the attributes
        ///  of the directory.
        /// </summary>
        FILE_WRITE_ATTRIBUTES = 0x00000100,

        /// <summary>
        ///  This value indicates the right to delete the directory.
        /// </summary>
        DELETE = 0x00010000,

        /// <summary>
        ///  This value indicates the right to read the security
        ///  descriptor for the directory.
        /// </summary>
        READ_CONTROL = 0x00020000,

        /// <summary>
        ///  This value indicates the right to change the DACL in
        ///  the security descriptor for the directory.  For the
        ///  DACL data structure, see  ACL in [MS-DTYP].
        /// </summary>
        WRITE_DAC = 0x00040000,

        /// <summary>
        ///  This value indicates the right to change the owner in
        ///  the security descriptor for the directory.
        /// </summary>
        WRITE_OWNER = 0x00080000,

        /// <summary>
        ///  This value SHOULD be set to 0 by the sender and MUST
        ///  be ignored by the receiver.
        /// </summary>
        SYNCHRONIZE = 0x00100000,

        /// <summary>
        ///  This value indicates the right to read or change the
        ///  SACL in the security descriptor for the directory.
        ///  For the SACL data structure, see ACL in [MS-DTYP].
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0x01000000,

        /// <summary>
        ///  This value indicates that the client is requesting an
        ///  open to the directory with the highest level of access
        ///  the client has on this directory.  If no access is
        ///  granted for the client on this directory, the server
        ///  MUST fail the open with STATUS_ACCESS_DENIED.
        /// </summary>
        MAXIMAL_ACCESS = 0x02000000,

        /// <summary>
        ///  This value indicates a request for all the access flags
        ///  that are listed above except MAXIMAL_ACCESS and ACCESS_SYSTEM_SECURITY.
        /// </summary>
        GENERIC_ALL = 0x10000000,

        /// <summary>
        ///  This value indicates a request for the following access
        ///  flags listed above: FILE_READ_ATTRIBUTES| FILE_TRAVERSE|
        ///  SYNCHRONIZE| READ_CONTROL.
        /// </summary>
        GENERIC_EXECUTE = 0x20000000,

        /// <summary>
        ///  This value indicates a request for the following access
        ///  flags listed above: FILE_ADD_FILE| FILE_ADD_SUBDIRECTORY|
        ///  FILE_WRITE_ATTRIBUTES| FILE_WRITE_EA| SYNCHRONIZE|
        ///  READ_CONTROL.
        /// </summary>
        GENERIC_WRITE = 0x40000000,

        /// <summary>
        ///  This value indicates a request for the following access
        ///  flags listed above: FILE_LIST_DIRECTORY| FILE_READ_ATTRIBUTES|
        ///  FILE_READ_EA| SYNCHRONIZE| READ_CONTROL.
        /// </summary>
        GENERIC_READ = 0x80000000,
    }

    /// <summary>
    ///  The SMB2_CREATE_CONTEXT structure is used by the SMB2
    ///  CREATE Request and the SMB2 CREATE Response to encode
    ///  additional flags and attributes: in requests to specify
    ///  how the CREATE request MUST be processed, and in responses
    ///  to specify how the CREATE request was in fact processed.There
    ///  is no required ordering when multiple create context
    ///  structures are used. The server MUST support receiving
    ///  the contexts in any order.Each structure takes the
    ///  following form:
    /// </summary>
    public partial struct CREATE_CONTEXT
    {

        /// <summary>
        ///  The offset from the beginning of this create context
        ///  to the beginning of a subsequent create context. This
        ///  field MUST be set to 0 if there are no subsequent contexts.
        /// </summary>
        [StaticSize(4)]
        public uint Next;

        /// <summary>
        ///  The offset from the beginning of this structure to its
        ///  name value. The name is represented as four or more
        ///  ASCII characters and MUST be one of the values provided
        ///  in the following table. The structure name indicates
        ///  what information is encoded by the data payload. The
        ///  following values are the valid create context values.
        ///   The names are case-sensitive.  More details are provided
        ///  for each of these values in the following subsections.
        /// </summary>
        [StaticSize(2)]
        public ushort NameOffset;

        /// <summary>
        ///  The length, in bytes, of the create context name.
        /// </summary>
        [StaticSize(2)]
        public ushort NameLength;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   This value MUST be set to 0 by the sender, and ignored
        ///  by the receiver.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  The offset, in bytes, from the beginning of this structure
        ///  to the data payload.
        /// </summary>
        [StaticSize(2)]
        public ushort DataOffset;

        /// <summary>
        ///  The length, in bytes, of the data. The format of the
        ///  data is determined by the type of SMB2_CREATE_CONTEXT
        ///  request, as outlined in the following sections. The
        ///  type is indicated in the NameOffset field in the message.
        /// </summary>
        [StaticSize(4)]
        public uint DataLength;

        /// <summary>
        ///  A variable-length buffer that contains the name and
        ///  data fields, as defined by NameOffset, NameLength,
        ///  DataOffset, and DataLength.
        /// </summary>
        [Size("Next > 0 ? (Next - NameOffset) : " // has Next.
            + "(DataOffset > 0 ? (DataOffset - NameOffset + DataLength):" // last entry, 
            + "(NameLength + DataLength))")]
        public byte[] Buffer;

        /// <summary>
        /// The structure name indicates what information is encoded by the data payload
        /// Returned as a byte array, and will be marshalled according to string or guid
        /// </summary>
        public byte[] Name
        {
            get
            {
                return Buffer.Skip(NameOffset - 16).Take((int)NameLength).ToArray();
            }
        }

        /// <summary>
        /// The information encoded, which is indicated by structure name
        /// </summary>
        public byte[] Data
        {
            get
            {
                return Buffer.Skip(DataOffset - 16).Take((int)DataLength).ToArray();
            }
        }
    }

    /// <summary>
    /// The SMB2_CREATE_EA_BUFFER context is specified on an SMB2 CREATE Request (section 2.2.13) 
    /// when the client is applying extended attributes as part of creating a new file. 
    /// </summary>
    public partial struct CREATE_EA_BUFFER
    {
        public FileFullEaInformation[] FileFullInformations;
    }

    /// <summary>
    /// The SMB2_CREATE_SD_BUFFER context is specified on an SMB2 CREATE Request 
    /// when the client is applying a security descriptor to a newly created file. 
    /// </summary>
    public partial struct CREATE_SD_BUFFER
    {
        public _SID SID;
    }

    /// <summary>
    ///  The SMB2_CREATE_DURABLE_HANDLE_REQUEST context is specified
    ///  on an SMB2 CREATE Request when the client is requesting
    ///  that the open be durable (that is, that it allow reestablishment
    ///  after disconnect). It MUST NOT be used for pipes. 
    ///  The format of the Data field of this SMB2_CREATE_CONTEXT
    ///  MUST be as follows:
    /// </summary>
    public partial struct CREATE_DURABLE_HANDLE_REQUEST
    {

        /// <summary>
        ///  A 16-byte field that is currently not used and MUST
        ///  be treated as reserved for future use. This value
        ///  MUST be set to 0 by the client and ignored by the server.
        /// </summary>
        [StaticSize(16)]
        public Guid DurableRequest;
    }

    /// <summary>
    /// The SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context is only valid for the SMB 3.x dialect family. 
    /// The SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context is specified in an SMB2 CREATE request 
    /// when the client requests the server to mark the open as durable or persistent. 
    /// </summary>
    public partial struct CREATE_DURABLE_HANDLE_REQUEST_V2
    {
        [StaticSize(4)]
        public uint Timeout;

        [StaticSize(4)]
        public CREATE_DURABLE_HANDLE_REQUEST_V2_Flags Flags;

        [StaticSize(8)]
        public ulong Reserved;

        [StaticSize(16)]
        public Guid CreateGuid;
    }

    [Flags]
    public enum CREATE_DURABLE_HANDLE_REQUEST_V2_Flags : uint
    {
        DHANDLE_FLAG_PERSISTENT = 0x00000002
    }

    /// <summary>
    /// DurableRequest_Values
    /// </summary>
    public enum DurableRequest_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 CREATE_DURABLE_HANDLE_RECONNECT context is
    ///  specified when the client is attempting to reestablish
    ///  a durable open as specified in section.
    /// </summary>
    public partial struct CREATE_DURABLE_HANDLE_RECONNECT
    {

        /// <summary>
        ///  An SMB2_FILEID structure, as specified in section ,
        ///  for the open that is being reestablished.
        /// </summary>
        [StaticSize(16)]
        public FILEID Data;
    }

    /// <summary>
    /// The SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 context is specified 
    /// when the client is attempting to reestablish a durable open as specified in section 3.2.4.4. 
    /// The SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 context is valid only for the SMB 3.x dialect family.
    /// </summary>
    public partial struct CREATE_DURABLE_HANDLE_RECONNECT_V2
    {

        /// <summary>
        ///  An SMB2_FILEID structure, as specified in section ,
        ///  for the open that is being reestablished.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;

        [StaticSize(16)]
        public Guid CreateGuid;

        [StaticSize(4)]
        public CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags Flags;
    }

    [Flags]
    public enum CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags
    {
        DHANDLE_FLAG_PERSISTENT = 0x00000002
    }

    /// <summary>
    /// The SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST context is specified on an SMB2 CREATE Request 
    /// when the client is requesting the server to retrieve maximal access information as part of processing the open.
    /// </summary>
    public partial struct CREATE_QUERY_MAXIMAL_ACCESS_REQUEST
    {
        public _FILETIME Timestamp;
    }

    /// <summary>
    /// The SMB2_CREATE_APP_INSTANCE_ID context is specified on an SMB2 CREATE Request 
    /// when the client is supplying an identifier provided by an application. 
    /// The SMB2_CREATE_APP_INSTANCE_ID context is only valid for the SMB 3.x dialect family.
    /// </summary>
    public partial struct CREATE_APP_INSTANCE_ID
    {
        [StaticSize(2)]
        public ushort StructureSize;

        [StaticSize(2)]
        public ushort Reserved;

        [StaticSize(16)]
        public Guid AppInstanceId;
    }

    /// <summary>
    /// The SMB2_CREATE_APP_INSTANCE_VERSION context is specified on an SMB2 CREATE Request 
    /// when the client is supplying a version for the app instance identifier provided by an application. 
    /// The SMB2_CREATE_APP_INSTANCE_VERSION context is only valid for the SMB 3.1.1 dialect.
    /// </summary>
    public partial struct CREATE_APP_INSTANCE_VERSION
    {
        /// <summary>
        /// The client MUST set this field to 24, indicating the size of this structure
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. This field MUST be set to zero.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        /// For 8-byte alignment.
        /// </summary>
        [StaticSize(4)]
        public uint Padding;

        /// <summary>
        /// An unsigned 64 bit integer containing the most significant value of the version.
        /// </summary>
        [StaticSize(8)]
        public UInt64 AppInstanceVersionHigh;

        /// <summary>
        /// An unsigned 64 bit integer containing the least significant value of the version.
        /// </summary>
        [StaticSize(8)]
        public UInt64 AppInstanceVersionLow;
    }

    /// <summary>
    ///  The SMB2 CREATE_ALLOCATION_SIZE context is specified
    ///  on an SMB2 CREATE Request when the client is setting
    ///  the initial allocation size of a newly created file.
    ///   The Data field of the SMB2_CREATE_CONTEXT MUST be
    ///  as follows:
    /// </summary>
    public partial struct CREATE_ALLOCATION_SIZE
    {

        /// <summary>
        ///  The size, in bytes, that the newly created file MUST
        ///  have reserved on disk.
        /// </summary>
        [StaticSize(8)]
        public ulong AllocationSize;
    }

    /// <summary>
    ///  The SMB2_CREATE_TIMEWARP_TOKEN context is specified
    ///  on an SMB2 CREATE Request when the client is requesting
    ///  the server to open a version of the file at a previous
    ///  point in time. The Data field of the SMB2_CREATE_CONTEXT
    ///  MUST contain the following structure:
    /// </summary>
    public partial struct CREATE_TIMEWARP_TOKEN
    {

        /// <summary>
        ///  A FILETIME as specified in FILETIME.The time stamp of
        ///  the version of the file that MUST be opened.  If no
        ///  version of this file exists at this time stamp, the
        ///  operation MUST be failed.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME Timestamp;
    }

    /// <summary>
    /// The SMB2_CREATE_REQUEST_LEASE context is specified on an SMB2 CREATE Request (section 2.2.13)
    /// packet when the client is requesting the server to return a lease. This is only valid for the 
    /// SMB 2.1 dialect.
    /// </summary>
    public partial struct CREATE_REQUEST_LEASE
    {
        /// <summary>
        ///  A unique key that identifies the owner of the lease.
        /// </summary>
        [StaticSize(16)]
        public Guid LeaseKey;

        /// <summary>
        ///  The requested lease state. This field MUST be constructed as a combination of the following 
        ///  values.
        /// </summary>
        [StaticSize(4)]
        public LeaseStateValues LeaseState;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The client MUST set this to 0, and the
        /// server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint LeaseFlags;

        /// <summary>
        /// This field MUST NOT be used and MUST be treated as reserved. The client MUST set this to 0,
        /// and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong LeaseDuration;
    }

    /// <summary>
    /// The SMB2_CREATE_QUERY_ON_DISK_ID context is specified on an SMB2 CREATE Request (section 2.2.13)
    /// when the client is requesting that the server return an identifier for the open file. 
    /// </summary>
    public partial struct CREATE_QUERY_ON_DISK_ID
    {
    }

    /// <summary>
    /// The SMB2_CREATE_REQUEST_LEASE_V2 context is specified on an SMB2 CREATE Request
    /// when the client is requesting the server to return a lease on a file or a directory. 
    /// This is valid only for the SMB 3.x dialect family. 
    /// </summary>
    public partial struct CREATE_REQUEST_LEASE_V2
    {
        /// <summary>
        ///  A unique key that identifies the owner of the lease.
        /// </summary>
        [StaticSize(16)]
        public Guid LeaseKey;

        /// <summary>
        ///  The requested lease state. This field MUST be constructed as a combination of the following 
        ///  values.
        /// </summary>
        [StaticSize(4)]
        public LeaseStateValues LeaseState;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The client MUST set this to 0, and the
        /// server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint LeaseFlags;

        /// <summary>
        /// This field MUST NOT be used and MUST be treated as reserved. The client MUST set this to 0,
        /// and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong LeaseDuration;

        [StaticSize(16)]
        public Guid ParentLeaseKey;

        [StaticSize(4)]
        public uint Epoch;
    }

    /// <summary>
    /// The SVHDX_OPEN_DEVICE_CONTEXT packet is sent by the client to open the shared virtual disk.
    /// </summary>
    public partial struct SVHDX_OPEN_DEVICE_CONTEXT
    {
        /// <summary>
        /// The version of the create context. It MUST be set to the highest supported version of the protocol.
        /// </summary>
        [StaticSize(4)]
        public uint Version;

        /// <summary>
        /// A Boolean value, where zero represents FALSE and nonzero represents TRUE. 
        /// </summary>
        [StaticSize(1)]
        public bool HasInitiatorId;

        /// <summary>
        /// This field MUST be set to zero when sent and MUST be ignored on receipt.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        /// A GUID that optionally identifies the initiator of the open request.
        /// </summary>
        [StaticSize(16)]
        public Guid InitiatorId;

        /// <summary>
        /// Reserved. The client SHOULD set this field to 0x00000000, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Flags;

        /// <summary>
        /// This field is used to indicate which component has originated or issued the operation. 
        /// This field MUST be set to 0x00000001.
        /// </summary>
        [StaticSize(4)]
        public uint OriginatorFlags;

        /// <summary>
        /// A 64-bit value assigned by the client for an outgoing request. The server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong OpenRequestId;

        /// <summary>
        /// The length, in bytes, of the InitiatorHostName, including the null termination. 
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorHostNameLength;

        /// <summary>
        /// A 126-byte buffer containing a null-terminated Unicode UTF-16 string that specifies the computer name on which the initiator resides.
        /// </summary>
        [StaticSize(126)]
        public byte[] InitiatorHostName;
    }

    /// <summary>
    /// The SVHDX_OPEN_DEVICE_CONTEXT_V2 packet is sent by the client to open the shared virtual disk.
    /// </summary>
    public partial struct SVHDX_OPEN_DEVICE_CONTEXT_V2
    {
        /// <summary>
        /// The version of the create context. It MUST be set to 0x00000002.
        /// </summary>
        [StaticSize(4)]
        public uint Version;

        /// <summary>
        /// A Boolean value, where zero represents FALSE and nonzero represents TRUE. 
        /// </summary>
        [StaticSize(1)]
        public bool HasInitiatorId;

        /// <summary>
        /// This field MUST be set to zero when sent and MUST be ignored on receipt.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        /// A GUID that optionally identifies the initiator of the open request.
        /// </summary>
        [StaticSize(16)]
        public Guid InitiatorId;

        /// <summary>
        /// Reserved. The client SHOULD set this field to 0x00000000, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Flags;

        /// <summary>
        /// This field is used to indicate which component has originated or issued the operation. 
        /// This field MUST be set to 0x00000001.
        /// </summary>
        [StaticSize(4)]
        public uint OriginatorFlags;

        /// <summary>
        /// A 64-bit value assigned by the client for an outgoing request. The server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong OpenRequestId;

        /// <summary>
        /// The length, in bytes, of the InitiatorHostName, including the null termination. 
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorHostNameLength;

        /// <summary>
        /// A 126-byte buffer containing a null-terminated Unicode UTF-16 string that specifies the computer name on which the initiator resides.
        /// </summary>
        [StaticSize(126)]
        public byte[] InitiatorHostName;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        [StaticSize(4)]
        public uint VirtualDiskPropertiesInitialized;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        [StaticSize(4)]
        public uint ServerServiceVersion;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        [StaticSize(4)]
        public uint VirtualSectorSize;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        [StaticSize(4)]
        public uint PhysicalSectorSize;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        [StaticSize(8)]
        public ulong VirtualSize;
    }

    /// <summary>
    /// The requested lease state.
    /// </summary>
    [Flags()]
    public enum LeaseStateValues
    {
        /// <summary>
        /// No lease is requested.
        /// </summary>
        SMB2_LEASE_NONE = 0x00,

        /// <summary>
        /// A read caching lease is requested.
        /// </summary>
        SMB2_LEASE_READ_CACHING = 0x01,

        /// <summary>
        /// A handle caching lease is requested.
        /// </summary>
        SMB2_LEASE_HANDLE_CACHING = 0x02,

        /// <summary>
        /// A write caching lease is requested.
        /// </summary>
        SMB2_LEASE_WRITE_CACHING = 0x04,
    }

    /// <summary>
    ///  The SMB2 CREATE Response packet is sent by the server
    ///  to notify the client of the status of its SMB2 CREATE
    ///  Request. This response is composed of an SMB2 header,
    ///  as specified in section , followed by this response
    ///  structure:
    /// </summary>
    public partial struct CREATE_Response
    {

        /// <summary>
        ///  The server MUST set this field to 89, indicating the
        ///  size of the request structure, not including the header.
        ///   The server MUST set this field to this value regardless
        ///  of how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The oplock level that is granted to the client for this
        ///  open. This field MUST contain one of the following
        ///  values:-based clients never use exclusive oplocks.
        ///   Because there are no situations where it would want
        ///  an exclusive oplock  where it would not also want an
        ///  SMB2_OPLOCK_LEVEL_BATCH, it always requests an SMB2_OPLOCK_LEVEL_BATCH.
        /// </summary>
        public OplockLevel_Values OplockLevel;

        [StaticSize(1)]
        public CREATE_RESPONSE_Flags Flags;

        /// <summary>
        ///  The action taken in establishing the open. This field
        ///  MUST contain one of the following values:
        /// </summary>
        [StaticSize(4)]
        public CreateAction_Values CreateAction;

        /// <summary>
        ///  A FILETIME, as specified in FILETIME.The time when the
        ///  file was created.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME CreationTime;

        /// <summary>
        ///  A FILETIME, as specified in FILETIME.The time the file
        ///  was last accessed.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A FILETIME, as specified in FILETIME.The time when data
        ///  was last written to the file.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME LastWriteTime;

        /// <summary>
        ///  A FILETIME, as specified in FILETIME.The time when the
        ///  file was last modified.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME ChangeTime;

        /// <summary>
        ///  The size, in bytes, of the data that is allocated to
        ///  the file.
        /// </summary>
        [StaticSize(8)]
        public ulong AllocationSize;

        /// <summary>
        ///  The size, in bytes, of the file.
        /// </summary>
        [StaticSize(8)]
        public ulong EndofFile;

        /// <summary>
        ///  The attributes of the file. The valid flags are as
        ///  specified in [MS-FSCC] section 2.6.
        /// </summary>
        [StaticSize(4)]
        public File_Attributes FileAttributes;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved2;

        /// <summary>
        ///  The SMB2_FILEID, as specified in section.The
        ///  identifier of the open to a file or pipe that was established.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the array of the SMB2_CREATE_CONTEXT responses
        ///  that are contained in this response.  If none are being
        ///  returned in the response, this value MUST be 0.  These
        ///  values are specified in section.
        /// </summary>
        [StaticSize(4)]
        public uint CreateContextsOffset;

        /// <summary>
        ///  The length, in bytes, of the SMB2_CREATE_CONTEXT responses
        ///  that are contained in this response.
        /// </summary>
        [StaticSize(4)]
        public uint CreateContextsLength;
    }

    [Flags]
    public enum CREATE_RESPONSE_Flags : byte
    {
        CREATE_FLAG_REPARSEPOINT = 0x01
    }

    /// <summary>
    /// OplockLevel_Values
    /// </summary>
    public enum OplockLevel_Values : byte
    {
        /// <summary>
        ///  No oplock was granted.
        /// </summary>
        OPLOCK_LEVEL_NONE = 0x00,

        /// <summary>
        ///  A level II oplock was granted.
        /// </summary>
        OPLOCK_LEVEL_II = 0x01,

        /// <summary>
        ///  An exclusive oplock was granted.
        /// </summary>
        OPLOCK_LEVEL_EXCLUSIVE = 0x08,

        /// <summary>
        ///  A batch oplock was granted.
        /// </summary>
        OPLOCK_LEVEL_BATCH = 0x09,

        /// <summary>
        /// A lease is requested. If set, the request packet MUST contain an 
        /// SMB2_CREATE_REQUEST_LEASE create context. This value is only valid 
        /// for the SMB 2.1 dialect.
        /// </summary>
        OPLOCK_LEVEL_LEASE = 0xFF,
    }

    /// <summary>
    /// CreateAction_Values
    /// </summary>
    public enum CreateAction_Values : uint
    {

        /// <summary>
        ///  An existing file was deleted and a new file was created
        ///  in its place.
        /// </summary>
        FILE_SUPERSEDED = 0x00000000,

        /// <summary>
        ///  An existing file was opened.
        /// </summary>
        FILE_OPENED = 0x00000001,

        /// <summary>
        ///  A new file was created.
        /// </summary>
        FILE_CREATED = 0x00000002,

        /// <summary>
        ///  An existing file was overwritten.
        /// </summary>
        FILE_OVERWRITTEN = 0x00000003,

        /// <summary>
        ///  The CreateDisposition field of the SMB2 CREATE Request
        ///  was set to FILE_CREATE, and the operation failed because
        ///  the file existed.
        /// </summary>
        FILE_EXISTS = 0x00000004,

        /// <summary>
        ///  The CreateDisposition field of the SMB2 CREATE Request
        ///  was set to FILE_OPEN, and the operation failed because
        ///  the file does not exist.
        /// </summary>
        FILE_DOES_NOT_EXIST = 0x00000005,
    }

    /// <summary>
    /// Reserved2_Values
    /// </summary>
    public enum Reserved2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,

        /// <summary>
        /// Possible value.
        /// </summary>
        V2 = 7536755
    }

    /// <summary>
    ///  The SMB2 FILEID is used to represent an open to a file.
    /// </summary>
    public partial struct FILEID
    {

        /// <summary>
        ///  A file handle that remains persistent when an open is
        ///  reconnected after being lost on a disconnect, as specified
        ///  in section. The server MUST return this file handle
        ///  as part of an SMB2 CREATE Response.
        /// </summary>
        [StaticSize(8)]
        public ulong Persistent;

        /// <summary>
        ///  A file handle that MAY be changed when an open is reconnected
        ///  after being lost on a disconnect, as specified in section
        ///  .    The server MUST return this file handle as part
        ///  of an SMB2 CREATE Response. This value MUST NOT change
        ///  unless a reconnection is performed.
        /// </summary>
        [StaticSize(8)]
        public ulong Volatile;

        public static FILEID Zero
        {
            get
            {
                return new FILEID { Persistent = 0, Volatile = 0 };
            }
        }

        public static FILEID Invalid
        {
            get
            {
                return new FILEID { Persistent = 0xFFFFFFFFFFFFFFFF, Volatile = 0xFFFFFFFFFFFFFFFF };
            }
        }

        public override string ToString()
        {
            return string.Format("{{ Persistent: 0x{0:x16}, Volatile: 0x{1:x16} }}", Persistent, Volatile);
        }
    }

    /// <summary>
    ///  If the server succeeds in opening a durable handle to
    ///  a file as requested by the client via the SMB2_CREATE_DURABLE_HANDLE_REQUEST
    ///  , it MUST send an  SMB2 CREATE_DURABLE_HANDLE_RESPONSE
    ///  back to the client to inform the client that the handle
    ///  is durable.If the server does not mark it for durable
    ///  operation   or the server does not implement durable
    ///  handles,  then it MUST ignore this request. 
    /// </summary>
    public partial struct CREATE_DURABLE_HANDLE_RESPONSE
    {

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public CREATE_DURABLE_HANDLE_RESPONSE_Reserved_Values Reserved;
    }

    public partial struct CREATE_DURABLE_HANDLE_RESPONSE_V2
    {

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Timeout;

        [StaticSize(4)]
        public CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags Flags;
    }

    [Flags]
    public enum CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags : uint
    {
        DHANDLE_FLAG_PERSISTENT = 0x00000002
    }

    /// <summary>
    /// CREATE_DURABLE_HANDLE_RESPONSE_Reserved_Values
    /// </summary>
    public enum CREATE_DURABLE_HANDLE_RESPONSE_Reserved_Values : ulong
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 CREATE_QUERY_MAXIMAL_ACCESS returns an SMB2_CREATE_CONTEXT
    ///  in the response with the Name that is identified by
    ///  SMB2_CREATE_QUERY_MAXIMAL ACCESS as specified in SMB2_CREATE_CONTEXT
    ///  Request Values, if the server attempts to query maximal
    ///  access as part of processing a create request.
    /// </summary>
    public partial struct CREATE_QUERY_MAXIMAL_ACCESS_RESPONSE
    {

        /// <summary>
        ///  The resulting status code of the attempt to query maximal
        ///  access. The MaximalAccess field is valid only if QueryStatus
        ///  is STATUS_SUCCESS. The status code MUST be one of
        ///  those defined in [MS-ERREF].
        /// </summary>
        [StaticSize(4)]
        public uint QueryStatus;

        /// <summary>
        ///  The maximal access that the user who is described by
        ///  SessionId has on the file or named pipe that was opened.
        ///   This is an access mask value, as specified in section
        ///  .
        /// </summary>
        [StaticSize(4)]
        public _ACCESS_MASK MaximalAccess;
    }

    public partial struct CREATE_QUERY_ON_DISK_ID_RESPONSE
    {
        [StaticSize(32)]
        public byte[] DiskIDBuffer;
    }

    /// <summary>
    /// The server responds with a lease that is granted for this open. 
    /// </summary>
    public partial struct CREATE_RESPONSE_LEASE
    {
        /// <summary>
        ///  A unique key that identifies the owner of the lease.
        /// </summary>
        [StaticSize(16)]
        public Guid LeaseKey;

        /// <summary>
        ///  The granted lease state.
        /// </summary>
        [StaticSize(4)]
        public LeaseStateValues LeaseState;

        /// <summary>
        /// If the server implements SMB 2.1, this field MUST be set to 0 
        /// or the following value. Otherwise, it is unused and MUST be 
        /// treated as reserved; the server MUST set this to 0, and the 
        /// client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public LeaseFlagsValues LeaseFlags;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The server 
        /// MUST set this to 0, and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong LeaseDuration;
    }

    public partial struct CREATE_RESPONSE_LEASE_V2
    {
        /// <summary>
        ///  A unique key that identifies the owner of the lease.
        /// </summary>
        [StaticSize(16)]
        public Guid LeaseKey;

        /// <summary>
        ///  The granted lease state.
        /// </summary>
        [StaticSize(4)]
        public LeaseStateValues LeaseState;

        /// <summary>
        /// If the server implements SMB 2.1, this field MUST be set to 0 
        /// or the following value. Otherwise, it is unused and MUST be 
        /// treated as reserved; the server MUST set this to 0, and the 
        /// client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public LeaseFlagsValues Flags;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The server 
        /// MUST set this to 0, and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong LeaseDuration;

        /// <summary>
        /// A unique key that identifies the owner of the lease for the parent directory.
        /// </summary>
        public Guid ParentLeaseKey;

        [StaticSize(2)]
        /// <summary>
        /// A 16-bit unsigned integer incremented by the server on a lease state change.
        /// </summary>
        public ushort Epoch;

        [StaticSize(2)]
        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The server SHOULD<49>; set this to 0, and the client MUST ignore it on receipt.
        /// </summary>
        public ushort Reserved;
    }

    /// <summary>
    /// possible Values of LeaseFlags.
    /// </summary>
    [Flags]
    public enum LeaseFlagsValues
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// A break for the lease identified by the lease key is in progress.
        /// </summary>
        LEASE_FLAG_BREAK_IN_PROGRESS = 0x02,

        /// <summary>
        /// When set, indicates that the ParentLeaseKey is set.
        /// </summary>
        SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET = 0x04,
    }

    /// <summary>
    ///  The SMB2 CLOSE Request packet is used by the client
    ///  to close an instance of a file that was opened previously
    ///  with a successful SMB2 CREATE Request. This request
    ///  is composed of an SMB2 header, as specified in section
    ///  , followed by this request structure:
    /// </summary>
    public partial struct CLOSE_Request
    {

        /// <summary>
        ///  The client MUST set this field to 24, indicating the
        ///  size of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  A Flags field, which indicates how the operation MUST
        ///  be processed. This field MUST be constructed by using
        ///  the following values:
        /// </summary>
        [StaticSize(2)]
        public Flags_Values Flags;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public CLOSE_Request_Reserved_Values Reserved;

        /// <summary>
        ///  An SMB2_FILEID structure, as specified in section.The
        ///  identifier of the open to a file, or of a named pipe
        ///  that is being closed.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;
    }

    /// <summary>
    /// Flags_Values
    /// </summary>
    [Flags()]
    public enum Flags_Values : ushort
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  If set, the server MUST set the attribute fields in
        ///  the response, as specified in section , to valid values.
        ///   If not set, the client MUST NOT use the values that
        ///  are returned in the response.
        /// </summary>
        CLOSE_FLAG_POSTQUERY_ATTRIB = 0x0001,
    }

    /// <summary>
    /// CLOSE_Request_Reserved_Values
    /// </summary>
    public enum CLOSE_Request_Reserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 CLOSE Response packet is sent by the server
    ///  to indicate that an SMB2 CLOSE Request was processed
    ///  successfully. This response is composed of an SMB2
    ///  header, as specified in section , followed by this
    ///  response structure:
    /// </summary>
    public partial struct CLOSE_Response
    {

        /// <summary>
        ///  The client MUST set this field to 60, indicating the
        ///  size of the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at present, and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public CLOSE_Response_Flags_Values Flags;

        /// <summary>
        ///  Unused at present, and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public CLOSE_Response_Reserved_Values Reserved;

        /// <summary>
        ///  A FILETIME, as specified in FILETIME.The time when the
        ///  file was created.  If the SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB
        ///  flag in the SMB2 CLOSE Request was set, this field
        ///  MUST be set to the value that is returned by the attribute
        ///  query. If the flag is not set, the field SHOULD be
        ///  set to zero and MUST NOT be checked on receipt.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME CreationTime;

        /// <summary>
        ///  A FILETIME, as specified in FILETIME.The time when the
        ///  file was last accessed.  If the SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB
        ///  flag in the SMB2 CLOSE Request was set, this field
        ///  MUST be set to the value that is returned by the attribute
        ///  query. If the flag is not set, this field MUST  be
        ///  set to zero.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A FILETIME, as specified in FILETIME.The time when data
        ///  was last written to the file.  If the SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB
        ///  flag in the SMB2 CLOSE Request was set, this field
        ///  MUST be set to the value that is returned by the attribute
        ///  query. If the flag is not set, this field MUST  be
        ///  set to zero.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME LastWriteTime;

        /// <summary>
        ///  A FILETIME, as specified in FILETIME.The time when the
        ///  file was last modified.  If the SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB
        ///  flag in the SMB2 CLOSE Request was set, this field
        ///  MUST be set to the value that is returned by the attribute
        ///  query. If the flag is not set, this field MUST  be
        ///  set to zero.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME ChangeTime;

        /// <summary>
        ///  The size, in bytes,  of the data that is allocated to
        ///  the file.  If the SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB
        ///  flag in the SMB2 CLOSE Request was set, this field
        ///  MUST be set to the value that is returned by the attribute
        ///  query. If the flag is not set, this field MUST  be
        ///  set to zero.
        /// </summary>
        [StaticSize(8)]
        public ulong AllocationSize;

        /// <summary>
        ///  The size, in bytes, of the file.  If the SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB
        ///  flag in the SMB2 CLOSE Request was set, this field
        ///  MUST be set to the value that is returned by the attribute
        ///  query. If the flag is not set, this field MUST  be
        ///  set to zero.
        /// </summary>
        [StaticSize(8)]
        public ulong EndofFile;

        /// <summary>
        ///  The attributes of the file.  If the SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB
        ///  flag in the SMB2 CLOSE Request was set, this field
        ///  MUST be set to the value that is returned by the attribute
        ///  query. If the flag is not set, this field MUST  be
        ///  set to zero.  For more information about valid flags,
        ///  see [MS-FSCC] section 2.6.
        /// </summary>
        [StaticSize(4)]
        public File_Attributes FileAttributes;
    }

    /// <summary>
    /// CLOSE_Response_Flags_Values
    /// </summary>
    [Flags()]
    public enum CLOSE_Response_Flags_Values : ushort
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    /// CLOSE_Response_Reserved_Values
    /// </summary>
    public enum CLOSE_Response_Reserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 FLUSH Request packet is sent by a client to
    ///  request that a server flushes all cached file information
    ///  for a specified open of a file to the persistent store
    ///  that backs the file. It is also sent for a specified
    ///  open of a named pipe to be completed when all data
    ///  that is written to the named pipe has been read by
    ///  the server side of the pipe. This request is composed
    ///  of an SMB2 header, as specified in section , followed
    ///  by this request structure:
    /// </summary>
    public partial struct FLUSH_Request
    {

        /// <summary>
        ///  The client MUST set this field to 24, indicating the
        ///  size of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public Reserved1_Values Reserved1;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public FLUSH_Request_Reserved2_Values Reserved2;

        /// <summary>
        ///  The SMB2_FILEID, as specified in section.The
        ///  client MUST set this field to the identifier of the
        ///  open to a file or named pipe that is being flushed.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;
    }

    /// <summary>
    /// Reserved1_Values
    /// </summary>
    public enum Reserved1_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// FLUSH_Request_Reserved2_Values
    /// </summary>
    public enum FLUSH_Request_Reserved2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 FLUSH Response packet is sent by the server
    ///  to confirm that an SMB2 FLUSH Request was successfully
    ///  processed. This response is composed of an SMB2 header,
    ///  as specified in section , followed by this request
    ///  structure:
    /// </summary>
    public partial struct FLUSH_Response
    {

        /// <summary>
        ///  The server MUST set this field to 4, indicating the
        ///  size of the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this field to 0, and the client
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public FLUSH_Response_Reserved_Values Reserved;
    }

    /// <summary>
    /// FLUSH_Response_Reserved_Values
    /// </summary>
    public enum FLUSH_Response_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 READ Request packet is sent by the client to
    ///  request a read operation on the file that is specified
    ///  by the FileId. This request is composed of an SMB2
    ///  header, as specified in section , followed by this
    ///  request structure:
    /// </summary>
    public partial struct READ_Request
    {

        /// <summary>
        ///  The client MUST set this field to 49, indicating the
        ///  size of the request structure, not including the header.
        ///   The client MUST set it to this value regardless of
        ///  how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The requested offset, in bytes, to place the data read
        ///  in the SMB2 READ Response. This value is provided
        ///  to optimize data placement on the client and is not
        ///  binding on the server.
        /// </summary>
        public byte Padding;

        /// <summary>
        ///  For the SMB 2.002, 2.1 and 3.0 dialects, this field MUST NOT
        ///  be used and MUST be reserved. The client MUST set this field to 0, 
        ///  and the server MUST ignore it on receipt. 
        ///  For the SMB 3.02 dialect, this field MUST contain zero or more of 
        ///  the following values SMB2_READFLAG_READ_UNBUFFERED 0x01.
        /// </summary>
        public READ_Request_Flags_Values Flags;

        /// <summary>
        ///  The length, in bytes, of the data to read from the specified
        ///  file or pipe.
        /// </summary>
        [StaticSize(4)]
        public uint Length;

        /// <summary>
        ///  The offset, in bytes, into the file or pipe from which
        ///  the data MUST be read.
        /// </summary>
        [StaticSize(8)]
        public ulong Offset;

        /// <summary>
        ///  The SMB2_FILEID, as specified in section.The identifier
        ///  of the file or pipe on which to perform the read.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;

        /// <summary>
        ///  The minimum number of bytes to be read for this operation
        ///  to be successful.  If fewer than the minimum number
        ///  of bytes are read by the server, the server MUST return
        ///  an error rather than the bytes read.
        /// </summary>
        [StaticSize(4)]
        public uint MinimumCount;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public Channel_Values Channel;

        /// <summary>
        ///  The number of subsequent bytes that the client intends
        ///  to read from the file after this operation completes.
        ///   This value is provided to facilitate read-ahead caching,
        ///  and is not binding on the server.
        /// </summary>
        [StaticSize(4)]
        public uint RemainingBytes;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort ReadChannelInfoOffset;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort ReadChannelInfoLength;
    }

    /// <summary>
    /// READ_Request_Flags_Values
    /// </summary>
    public enum READ_Request_Flags_Values : byte
    {
        ZERO = 0x00,

        /// <summary>
        ///  The server or underlying object store SHOULD NOT cache the read data at intermediate layers.
        /// </summary>
        SMB2_READFLAG_READ_UNBUFFERED = 0x01,

        /// <summary>
        /// The server is requested to compress the read response when responding to the request. 
        /// This flag is not valid for the SMB 2.0.2, 2.1, 3.0 and 3.0.2 dialects.
        /// </summary>
        SMB2_READFLAG_REQUEST_COMPRESSED = 0x02,
    }

    /// <summary>
    /// Channel_Values
    /// </summary>
    public enum Channel_Values : uint
    {
        /// <summary>
        ///  No channel information is present in the request. The WriteChannelInfoOffset and WriteChannelInfoLength fields
        ///  MUST be set to zero by the client and MUST be ignored by the server.
        /// </summary>
        CHANNEL_NONE = 0x00000000,

        /// <summary>
        /// One or more SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures as specified in [MS-SMBD] section 2.2.3.1 are present 
        /// in the channel information specified by WriteChannelInfoOffset and WriteChannelInfoLength fields.
        /// </summary>
        CHANNEL_RDMA_V1 = 0x00000001,

        /// <summary>
        /// This value is valid only for the SMB 3.02 dialect. One or more SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures as specified 
        /// in [MS-SMBD] section 2.2.3.1 are present in the channel information specified by the ReadChannelInfoOffset 
        /// and ReadChannelInfoLength fields. The server is requested to perform remote invalidation when responding to 
        /// the request as specified in [MS-SMBD] section 3.1.4.2.
        /// </summary>
        CHANNEL_RDMA_V1_INVALIDATE = 0x00000002
    }

    [StructLayout(LayoutKind.Explicit, Size = 52)]
    public partial struct Transform_Header
    {
        [FieldOffset(0)]

        public uint ProtocolId;

        [FieldOffset(4)]

        private ulong Signature1;

        [FieldOffset(12)]

        private ulong Signature2;

        public byte[] Signature
        {
            get
            {
                return BitConverter.GetBytes(Signature1).Concat(BitConverter.GetBytes(Signature2)).ToArray();
            }
            set
            {
                Signature1 = BitConverter.ToUInt64(value, 0);
                Signature2 = BitConverter.ToUInt64(value, 8);
            }
        }

        [FieldOffset(20)]
        public Guid Nonce;


        [FieldOffset(36)]

        public uint OriginalMessageSize;

        [FieldOffset(40)]

        public ushort Reserved;

        // In the SMB 3.1.1 dialect, this field is interpreted as the Flags field, which indicates how the SMB2 message was transformed.
        [FieldOffset(42)]

        public TransformHeaderFlags Flags;

        // In the SMB 3.0 and SMB 3.0.2 dialects, this field is interpreted as the EncryptionAlgorithm field, which contains the algorithm used for encrypting the SMB2 message. 
        [FieldOffset(42)]

        public EncryptionAlgorithm EncryptionAlgorithm;

        [FieldOffset(44)]

        public ulong SessionId;
    }

    [Flags]
    public enum TransformHeaderFlags : ushort
    {
        None = 0,
        Encrypted = 0x0001
    }

    public enum EncryptionAlgorithm : ushort
    {
        ENCRYPTION_NONE = 0,
        ENCRYPTION_AES128_CCM = 0x0001,
        ENCRYPTION_AES128_GCM = 0x0002,
        ENCRYPTION_INVALID = 0xFFFF
    }

    public enum PreauthIntegrityHashID : ushort
    {
        HashAlgorithm_NONE = 0,
        SHA_512 = 1,
        HashAlgorithm_Invalid = 0xFFFF
    }

    /// <summary>
    /// The SMB2 COMPRESSION_TRANSFORM_HEADER is used by the client or server when sending compressed messages.
    /// This optional header is only valid for the SMB 3.1.1 dialect.
    /// </summary>
    public struct Compression_Transform_Header
    {
        /// <summary>
        /// The protocol identifier. The value MUST be (in network order) 0xFC, 'S', 'M', and 'B'.
        /// </summary>
        public uint ProtocolId;

        /// <summary>
        /// The size, in bytes, of the uncompressed data segment.
        /// </summary>
        public uint OriginalCompressedSegmentSize;

        /// <summary>
        /// This field MUST contain one of the algorithms used to compress the SMB2 message except "NONE".
        /// </summary>
        public CompressionAlgorithm CompressionAlgorithm;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The sender MUST set this to 0, and the receiver MUST ignore it.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The offset, in bytes, from the end of this structure to the start of compressed data segment.
        /// </summary>
        public uint Offset;
    }

    /// <summary>
    ///  The SMB2 Packet Header packet is the header of all SMB
    ///  2.0 Protocol packets.  If the SMB2_FLAGS_ASYNC_COMMAND
    ///  is set in Flags, the header takes the following form:
    /// </summary>
    public partial struct Packet_Header_ASync
    {

        /// <summary>
        ///  The protocol identifier. The value must be (in network
        ///  order) 0xFE, 'S', 'M', and 'B'.
        /// </summary>
        [StaticSize(4)]
        public uint ProtocolId;

        /// <summary>
        ///  MUST be set to 64, which is the size, in bytes, of the
        ///  SMB2 header structure.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The sender MUST set this to 0, and the receiver MUST
        ///  ignore it.
        /// </summary>
        [StaticSize(2)]
        public ushort CreditCharge;

        /// <summary>
        ///  The status code for the response. The client MUST set
        ///  this to 0 and the server MUST ignore it on receipt.
        ///   For a list of valid status codes, see [MS-ERREF] section
        ///  4.
        /// </summary>
        [StaticSize(4)]
        public uint Status;

        /// <summary>
        ///  The command code of this packet. This field MUST contain
        ///  one of the following valid commands: SMB2
        ///  commands Code SMB2 NEGOTIATE 0x0000 SMB2 SESSION_SETUP0x0001 SMB2
        ///  LOGOFF0x0002SMB2 TREE_CONNECT0x0003SMB2 TREE_DISCONNECT0x0004SMB2
        ///  CREATE0x0005SMB2 CLOSE0x0006SMB2 FLUSH0x0007 SMB2
        ///  READ0x0008SMB2 WRITE0x0009SMB2 LOCK0x000ASMB2 IOCTL0x000BSMB2
        ///  CANCEL0x000CSMB2 ECHO0x000DSMB2 QUERY_DIRECTORY0x000ESMB2
        ///  CHANGE_NOTIFY 0x000FSMB2 QUERY_INFO0x0010SMB2
        ///  SET_INFO0x0011SMB2 OPLOCK_BREAK0x0012
        /// </summary>
        [StaticSize(2)]
        public Smb2Command Command;

        /// <summary>
        ///  On a request, this field indicates the number of credits
        ///  the client is requesting.  On a response, it indicates
        ///  the number of credits granted to the client. If a client
        ///  does not want more credits, it MUST set this field
        ///  to 1.
        /// </summary>
        [StaticSize(2)]
        public ushort CreditRequest_47_Response;

        /// <summary>
        ///  A Flags field, which indicates how the operation must
        ///  be processed. This field MUST be constructed by using
        ///  the following values:
        /// </summary>
        [StaticSize(4)]
        public Packet_Header_ASync_Flags_Values Flags;

        /// <summary>
        ///  For a compounded request, this numeric value MUST be
        ///  set to the offset, in bytes, from the beginning of
        ///  this SMB 2.0 Protocol header to the start of the subsequent
        ///  SMB 2.0 Protocol header.  If this is not a compounded
        ///  request, this value MUST be 0.
        /// </summary>
        [StaticSize(4)]
        public uint NextCommand;

        /// <summary>
        ///  A value that identifies a message request and response
        ///  uniquely across all messages that are sent on the same
        ///  SMB 2.0 Protocol transport connection.
        /// </summary>
        [StaticSize(8)]
        public ulong MessageId;

        /// <summary>
        ///  A unique identification number that is created by the
        ///  server to handle operations asynchronously, as specified
        ///  in section.
        /// </summary>
        [StaticSize(8)]
        public ulong AsyncId;

        /// <summary>
        ///  Uniquely identifies the established session for the
        ///  command. This MUST be 0 for requests that do not have
        ///  a user context that is associated with them. This
        ///  MUST be 0 for the first SMB2 SESSION SETUP request
        ///  for a specified security principal. The following
        ///  SMB 2.0 Protocol commands do not require the SessionId
        ///  to be set to a nonzero value received from a previous
        ///  SMB2 SESSION_SETUP response.  SessionId SHOULD be set
        ///  to 0 for the following commands:SMB2 NEGOTIATE requestSMB2
        ///  NEGOTIATE responseSMB2 SESSION_SETUP requestSMB2 ECHO
        ///  requestSMB2 ECHO response
        /// </summary>
        [StaticSize(8)]
        public ulong SessionId;

        /// <summary>
        ///  The 16-byte signature of the message, if SMB2_FLAGS_SIGNED
        ///  is set in the Flags field of the SMB 2.0 Protocol header.
        ///   If the message is not signed, this field MUST be 0.
        /// </summary>
        [StaticSize(8)]
        private ulong Signature1;
        [StaticSize(8)]
        private ulong Signature2;

        public byte[] Signature
        {
            get
            {
                return BitConverter.GetBytes(Signature1).Concat(BitConverter.GetBytes(Signature2)).ToArray();
            }
            set
            {
                Signature1 = BitConverter.ToUInt64(value, 0);
                Signature2 = BitConverter.ToUInt64(value, 8);
            }
        }
    }

    /// <summary>
    /// Status_Values
    /// </summary>
    public enum Status_Values : int
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Packet_Header_ASync_Flags_Values
    /// </summary>
    [Flags()]
    public enum Packet_Header_ASync_Flags_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates the message is a response rather
        ///  than a request. This MUST be set on responses sent
        ///  from the server to the client, and MUST NOT be set
        ///  on requests sent from the client to the server.
        /// </summary>
        FLAGS_SERVER_TO_REDIR = 0x00000001,

        /// <summary>
        ///  When set, indicates that this is an asynchronously processed
        ///  command. For asynchronous requests, see section. For
        ///  asynchronous responses, see section.
        /// </summary>
        FLAGS_ASYNC_COMMAND = 0x00000002,

        /// <summary>
        ///  When set, indicates that this command is a related operation
        ///  in a compounded request chain.
        /// </summary>
        FLAGS_RELATED_OPERATIONS = 0x00000004,

        /// <summary>
        ///  When set, indicates that this packet has been signed.
        /// </summary>
        FLAGS_SIGNED = 0x00000008,

        /// <summary>
        ///  When set, indicates that this command is a Distributed
        ///  File System (DFS) operation.
        /// </summary>
        FLAGS_DFS_OPERATIONS = 0x10000000,
    }

    /// <summary>
    ///  The SMB2 Packet Header - SYNC packet is the header of
    ///  all SMB 2.0 Protocol packets. If the SMB2_FLAGS_ASYNC_COMMAND
    ///  is not set in Flags, the header takes the following
    ///  form:
    /// </summary>
    public partial struct Packet_Header
    {

        /// <summary>
        ///  The protocol identifier. The value must be (in network
        ///  order) 0xFE, 'S', 'M', and 'B'.
        /// </summary>
        [StaticSize(4)]
        public uint ProtocolId;

        /// <summary>
        ///  This MUST be set to 64, which is the size, in bytes,
        ///  of the SMB2 header structure.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The sender MUST set this to 0, and the receiver MUST
        ///  ignore it.
        /// </summary>
        [StaticSize(2)]
        public ushort CreditCharge;

        /// <summary>
        ///  The status code for the response. The client MUST set
        ///  this to 0 and the server MUST ignore it on receipt.
        ///   For a list of valid status codes, see [MS-ERREF] section
        ///  4.
        /// </summary>
        [StaticSize(4)]
        public uint Status;

        /// <summary>
        ///  The command code of this packet. This field MUST contain
        ///  one of the following valid commands: SMB2
        ///  commands Code SMB2 NEGOTIATE0x00SMB2
        ///  SESSION_SETUP0x01SMB2 LOGOFF0x02SMB2 TREE_CONNECT0x03SMB2
        ///  TREE_DISCONNECT0x04SMB2 CREATE0x05SMB2 CLOSE0x06SMB2
        ///  FLUSH0x07SMB2 READ0x08SMB2 WRITE0x09SMB2 LOCK0x0ASMB2
        ///  IOCTL0x0BSMB2 CANCEL0x0CSMB2 ECHO0x0DSMB2 QUERY_DIRECTORY0x0ESMB2
        ///  CHANGE_NOTIFY0x0FSMB2 QUERY_INFO0x10SMB2 SET_INFO0x11SMB2
        ///  OPLOCK_BREAK0x12
        /// </summary>
        [StaticSize(2)]
        public Smb2Command Command;

        /// <summary>
        ///  On a request, this field indicates the number of credits
        ///  the client is requesting.  On a response, it indicates
        ///  the number of credits that are granted to the client.
        /// </summary>
        [StaticSize(2)]
        public ushort CreditRequestResponse;

        /// <summary>
        ///  A Flags field, which indicates how the operation must
        ///  be processed. This field MUST be constructed by using
        ///  the following values:
        /// </summary>
        [StaticSize(4)]
        public Packet_Header_Flags_Values Flags;

        /// <summary>
        ///  For a compounded request, this value MUST be set to
        ///  the offset, in bytes, from the beginning of this SMB
        ///  2.0 Protocol header to the start of the subsequent
        ///  SMB 2.0 Protocol header.  If this is not a compounded
        ///  request, this value MUST be 0.
        /// </summary>
        [StaticSize(4)]
        public uint NextCommand;

        /// <summary>
        ///  A value that identifies a message request and response
        ///  uniquely across all messages that are sent on the same
        ///  SMB 2.0 Protocol transport connection.
        /// </summary>
        [StaticSize(8)]
        public ulong MessageId;

        /// <summary>
        ///  The client-side identification of the process that is
        ///  issuing the request.
        /// </summary>
        [StaticSize(4)]
        public uint ProcessId;

        /// <summary>
        ///  Uniquely identifies the tree connect for the command.
        ///   This MUST be 0 for the SMB2 TREE_CONNECT request.
        ///    The TreeId MAY be any unsigned 32-bit integer that
        ///  is received from a previous SMB2 TREE_CONNECT response.
        ///   The following SMB 2.0 Protocol commands do not require
        ///  the TreeId to be set to a nonzero value received from
        ///  a previous SMB2 TREE_CONNECT response.  TreeId SHOULD
        ///  be set to 0 for the following commands:SMB2 NEGOTIATE
        ///  requestSMB2 NEGOTIATE response SMB2 SESSION_SETUP requestSMB2
        ///  SESSION_SETUP responseSMB2 LOGOFF requestSMB2 LOGOFF
        ///  responseSMB2 ECHO requestSMB2 ECHO response
        /// </summary>
        [StaticSize(4)]
        public uint TreeId;

        /// <summary>
        ///  Uniquely identifies the established session for the
        ///  command. This MUST be 0 for requests that do not have
        ///  a user context that is associated with them. This
        ///  MUST be 0 for the first SMB2 SESSION_SETUP request
        ///  for a specified security principal. The following
        ///  SMB 2.0 Protocol commands do not require the SessionId
        ///  to be set to a nonzero value received from a previous
        ///  SMB2 SESSION_SETUP response.  SessionId SHOULD be set
        ///  to 0 for the following commands:SMB2 NEGOTIATE requestSMB2
        ///  NEGOTIATE responseSMB2 SESSION_SETUP requestSMB2 ECHO
        ///  requestSMB2 ECHO response
        /// </summary>
        [StaticSize(8)]
        public ulong SessionId;

        /// <summary>
        ///  The 16-byte signature of the message, if SMB2_FLAGS_SIGNED
        ///  is set in the Flags field of the SMB 2.0 Protocol header.
        ///   If the message is not signed, this field MUST be 0.
        /// </summary>
        [StaticSize(8)]
        private ulong Signature1;
        [StaticSize(8)]
        private ulong Signature2;

        public byte[] Signature
        {
            get
            {
                return BitConverter.GetBytes(Signature1).Concat(BitConverter.GetBytes(Signature2)).ToArray();
            }
            set
            {
                Signature1 = BitConverter.ToUInt64(value, 0);
                Signature2 = BitConverter.ToUInt64(value, 8);
            }
        }
    }

    /// <summary>
    /// Packet_Header_Flags_Values
    /// </summary>
    [Flags()]
    public enum Packet_Header_Flags_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates the message is a response, rather
        ///  than a request. This MUST be set on responses sent
        ///  from the server to the client and MUST NOT be set on
        ///  requests sent from the client to the server.
        /// </summary>
        FLAGS_SERVER_TO_REDIR = 0x00000001,

        /// <summary>
        ///  When set, indicates that this is an asynchronously processed
        ///  command. For asynchronous requests, see section. For
        ///  asynchronous responses, see section.
        /// </summary>
        FLAGS_ASYNC_COMMAND = 0x00000002,

        /// <summary>
        ///  When set, indicates that this command is a related operation
        ///  in a compounded request chain.
        /// </summary>
        FLAGS_RELATED_OPERATIONS = 0x00000004,

        /// <summary>
        ///  When set, indicates that this packet has been signed.
        /// </summary>
        FLAGS_SIGNED = 0x00000008,

        /// <summary>
        ///  When set, indicates that this command is a Distributed
        ///  File System (DFS) operation.
        /// </summary>
        FLAGS_DFS_OPERATIONS = 0x10000000,

        /// <summary>
        /// This flag is only valid for the SMB 3.0 dialect. When set, 
        /// it indicates that this command is a replay operation.The client MUST ignore this bit on receipt.
        /// </summary>
        FLAGS_REPLAY_OPERATION = 0x20000000,
    }

    /// <summary>
    /// Error Context in error response
    /// </summary>
    public struct Error_Context
    {
        /// <summary>
        /// The length, in bytes, of the ErrorData field
        /// </summary>        
        [StaticSize(4)]
        public uint ErrorDataLength;

        /// <summary>
        /// An identifier for the error context
        /// </summary>        
        [StaticSize(4)]
        public Error_Id ErrorId;

        /// <summary>
        /// Variable-length error data
        /// </summary>                 
        [Size("ErrorDataLength")]
        public ErrorData_Format ErrorData;
    }

    /// <summary>
    /// An identifier for the error context.
    /// </summary>
    public enum Error_Id : uint
    {
        /// <summary>
        /// Unless otherwise specified, all errors defined in the [MS-SMB2] protocol use this error ID.
        /// </summary>
        ERROR_ID_DEFAULT = 0x00000000,

        /// <summary>
        /// The ErrorContextData field contains a share redirect message described in section 2.2.2.2.2.
        /// </summary>
        ERROR_ID_SHARE_REDIRECT = 0x72645253,
    }

    /// <summary>
    /// Error Data format struct (when ErrorContextCount !=0)
    /// </summary>
    public struct ErrorData_Format
    {
        /// <summary>
        /// If the error code in the header of the response is set to STATUS_STOPPED_ON_SYMLINK,
        /// this field MUST contain a Symbolic Link Error Response as specified in section 2.2.2.2.1.
        /// </summary>
        public Symbolic_Link_Error_Response SymbolicLinkErrorResponse;

        /// <summary>
        /// If the error code in the header of the response is set to STATUS_BAD_NETWORK_NAME,
        /// and the ErrorId in the SMB2 Error Context response is set to SMB2_ERROR_ID_SHARE_REDIRECT,
        /// this field MUST contain a Share Redirect Error Response as specified in section 2.2.2.2.2.
        /// </summary>
        public Share_Redirect_Error_Context_Response ShareRedirectErrorContextResponse;

        /// <summary>
        /// If the error code in the header of the response is STATUS_BUFFER_TOO_SMALL,
        /// this field MUST be set to a 4-byte value indicating the minimum required buffer length.
        /// </summary>
        public byte[] BufferTooSmallErrorResponse;
    }

    /// <summary>
    ///  The SMB2 ERROR Response packet is sent by the server
    ///  to respond to a request that has failed or encountered
    ///  an error. This response is composed of an SMB2 Packet
    ///  Header followed by this response structure:
    /// </summary>        
    public partial struct ERROR_Response_packet
    {
        /// <summary>
        ///  The server MUST set this field to 9, indicating the
        ///  size of the response structure, not including the header.
        ///   The server MUST set it to this value regardless of
        ///  how long ErrorData[] actually is in the request being
        ///  sent.
        /// </summary>                
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        /// This field MUST be set to 0 for SMB dialects other than 3.1.1
        /// For the SMB dialect 3.1.1, if this field is nonzero,
        /// the ErrorData field MUST be formatted as a variable-length array of SMB2 ERROR Context structures containing ErrorContextCount entries
        /// </summary>                
        [StaticSize(1)]
        public byte ErrorContextCount;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. 
        /// The server MUST set this to 0, and the client MUST ignore it on receipt.
        /// </summary>                
        [StaticSize(1)]
        public byte Reserved;

        /// <summary>
        ///  The number of bytes of data contained in ErrorData[].
        /// </summary>                
        [StaticSize(4)]
        public uint ByteCount;

        [Size("ByteCount  == 0 ? 1: ByteCount ")]
        public byte[] ErrorData;

        [Size("ErrorContextCount")]
        public Error_Context[] ErrorContextErrorData;
    }

    /// <summary>
    /// ERROR_Response_packet_Reserved_Values
    /// </summary>
    public enum ERROR_Response_packet_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 READ Response packet is sent in response to
    ///  an SMB2 READ Request packet. This response is composed
    ///  of an SMB2 header, as specified in section , followed
    ///  by this response structure:
    /// </summary>
    public partial struct READ_Response
    {
        /// <summary>
        ///  The server MUST set this field to 17, indicating the
        ///  size of the response structure, not including the header.
        ///   This value MUST be used regardless of how large Buffer[]
        ///  is in the actual response.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the header
        ///  to the data read being returned in this response.
        /// </summary>
        public byte DataOffset;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        public byte Reserved;

        /// <summary>
        ///  The length, in bytes, of the data read being returned
        ///  in this response. The minimum size is 1 byte.
        /// </summary>
        [StaticSize(4)]
        public uint DataLength;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this field to 0, and the client
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint DataRemaining;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved2;
    }

    /// <summary>
    ///  The SMB2 WRITE Request packet is sent by the client
    ///  to write data to the file or named pipe on the server.
    ///   This request is composed of an SMB2 header, as specified
    ///  in section , followed by this request structure:
    /// </summary>
    public partial struct WRITE_Request
    {

        /// <summary>
        ///  The server MUST set this field to 49, indicating the
        ///  size of the request structure, not including the header.
        ///   The server MUST set it to this value regardless of
        ///  how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The offset, in bytes, from the header where the data
        ///  to be written is located in the request.
        /// </summary>
        [StaticSize(2)]
        public ushort DataOffset;

        /// <summary>
        ///  The length of the data being written, in bytes.
        /// </summary>
        [StaticSize(4)]
        public uint Length;

        /// <summary>
        ///  The offset, in bytes, of where to write the data in
        ///  the destination file.
        /// </summary>
        [StaticSize(8)]
        public ulong Offset;

        /// <summary>
        ///  SMB2_FILEID, as specified in section.		The identifier
        ///  of the file or pipe on which to perform the write.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public Channel_Values Channel;

        /// <summary>
        ///  The number of subsequent bytes the client intends to
        ///  write to the file after this operation completes. 
        ///  This value is provided to facilitate write caching
        ///  and is not binding on the server.
        /// </summary>
        [StaticSize(4)]
        public uint RemainingBytes;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort WriteChannelInfoOffset;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort WriteChannelInfoLength;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public WRITE_Request_Flags_Values Flags;
    }

    /// <summary>
    /// WRITE_Request_Flags_Values
    /// </summary>
    [Flags()]
    public enum WRITE_Request_Flags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        None = 0,

        /// <summary>
        /// The write request should be written to persistent storage before the response
        /// is sent regardless of how the file was opened. This value is only supported for
        /// the SMB 2.1 dialect
        /// </summary>
        SMB2_WRITEFLAG_WRITE_THROUGH = 0x00000001,

        /// <summary>
        /// The server or underlying object store SHOULD NOT cache the write data at intermediate
        /// layers and SHOULD allow it to flow through to persistent storage. This bit is only 
        /// valid for the SMB 3.02 dialect.
        /// </summary>
        SMB2_WRITEFLAG_WRITE_UNBUFFERED = 0x00000002,
    }

    /// <summary>
    ///  The SMB2 WRITE Response packet is sent by the server
    ///  to write data to the file or named pipe on the server.
    ///   This response is composed of an SMB2 header, as specified
    ///  in section , followed by this response structure:
    /// </summary>
    public partial struct WRITE_Response
    {

        /// <summary>
        ///  The server MUST set this field to 17, indicating the
        ///  size of the response structure, not including the header.
        ///   This value MUST be used regardless of how large Buffer[]
        ///  is in the actual response.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  The number of bytes written.
        /// </summary>
        [StaticSize(4)]
        public uint Count;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Remaining;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort WriteChannelInfoOffset;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort WriteChannelInfoLength;
    }

    public enum OplockLeaseBreakStructureSize : ushort
    {
        OplockBreakNotification = 24,
        LeaseBreakNotification = 44,
        OplockBreakAcknowledgment = 24,
        LeaseBreakAcknowledgment = 36,
        OplockBreakResponse = 24,
        LeaseBreakResponse = 36
    }

    /// <summary>
    ///  The SMB2 OPLOCK_BREAK Notification Packet is sent by
    ///  the server when the underlying object store indicates
    ///  that an opportunistic lock (oplock) is being broken,
    ///   representing a change in the oplock level. This response
    ///  is composed of an SMB2 header, as specified in section
    ///  , followed by this notification structure:
    /// </summary>
    public partial struct OPLOCK_BREAK_Notification_Packet
    {
        /// <summary>
        ///  The server MUST set this to 24, indicating the size
        ///  of the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The server MUST set this to the maximum value of the
        ///  OplockLevel that the server will accept for an acknowledgment
        ///  from the client.  Because SMB2_OPLOCK_LEVEL_BATCH is
        ///  the highest oplock  level, and it is being broken 
        ///  to a lower level, the server will never send a break
        ///  from SMB2_OPLOCK_LEVEL_BATCH to SMB2_OPLOCK_LEVEL_BATCH.
        ///  Thus this field MUST contain one of the following values:-based
        ///  clients never use exclusive oplocks.  Because there
        ///  are no situations where it would want an exclusive
        ///  oplock where it would not also want an SMB2_OPLOCK_LEVEL_BATCH,
        ///  it always requests an SMB2_OPLOCK_LEVEL_BATCH.
        /// </summary>
        public OPLOCK_BREAK_Notification_Packet_OplockLevel_Values OplockLevel;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        public OPLOCK_BREAK_Notification_Packet_Reserved_Values Reserved;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public OPLOCK_BREAK_Notification_Packet_Reserved2_Values Reserved2;

        /// <summary>
        ///  The SMB2_FILEID, as specified in section.The
        ///  identifier of the file or pipe which is either having
        ///  its oplock broken, or its oplock break acknowledged.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;
    }

    /// <summary>
    /// OPLOCK_BREAK_Notification_Packet_OplockLevel_Values
    /// </summary>
    public enum OPLOCK_BREAK_Notification_Packet_OplockLevel_Values : byte
    {

        /// <summary>
        ///  No oplock is available.
        /// </summary>
        OPLOCK_LEVEL_NONE = 0x00,

        /// <summary>
        ///  A level II oplock is available.
        /// </summary>
        OPLOCK_LEVEL_II = 0x01,
    }

    /// <summary>
    /// OPLOCK_BREAK_Notification_Packet_Reserved_Values
    /// </summary>
    public enum OPLOCK_BREAK_Notification_Packet_Reserved_Values : byte
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// OPLOCK_BREAK_Notification_Packet_Reserved2_Values
    /// </summary>
    public enum OPLOCK_BREAK_Notification_Packet_Reserved2_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }



    #region Lease Break Notification

    /// <summary>
    /// LEASE_BREAK_Notification_Packet_Flags_Values
    /// </summary>
    public enum LEASE_BREAK_Notification_Packet_Flags_Values : uint
    {
        None = 0x0,

        /// <summary>
        /// Possible value.
        /// </summary>
        SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED = 0x01,
    }

    /// <summary>
    /// LEASE_BREAK_Notification_Packet_BreakReason_Values
    /// </summary>
    public enum LEASE_BREAK_Notification_Packet_BreakReason_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// LEASE_BREAK_Notification_Packet_AccessMaskHint_Values
    /// </summary>
    public enum LEASE_BREAK_Notification_Packet_AccessMaskHint_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// LEASE_BREAK_Notification_Packet_ShareMaskHint_Values
    /// </summary>
    public enum LEASE_BREAK_Notification_Packet_ShareMaskHint_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }


    /// <summary>
    /// The SMB2 Lease Break Notification packet is sent by the server when the underlying 
    /// object store indicates that a lease is being broken, representing a change in the lease state. 
    /// This is only valid on the SMB 2.1 dialect. This response is composed of an SMB2 header, 
    /// as specified in section 2.2.1, followed by this notification structure:
    /// </summary>
    public partial struct LEASE_BREAK_Notification_Packet
    {
        /// <summary>
        ///  The server MUST set this to 44, indicating the size of the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        /// A 16-bit unsigned integer indicating a lease state change by the server.
        /// This field is only valid for a server implementing the SMB 3.0 dialect.
        /// </summary>
        [StaticSize(2)]
        public ushort NewEpoch;

        /// <summary>
        ///  The field MUST be constructed using the following values.
        /// </summary>
        [StaticSize(4)]
        public LEASE_BREAK_Notification_Packet_Flags_Values Flags;

        /// <summary>
        /// A unique key which identifies the owner of the lease.
        /// </summary>
        [StaticSize(16)]
        public Guid LeaseKey;

        /// <summary>
        /// The current lease state of the open. This field MUST be constructed using the following values.
        /// </summary>
        [StaticSize(4)]
        public LeaseStateValues CurrentLeaseState;

        /// <summary>
        /// The new lease state for the open. This field MUST be constructed using the SMB2_LEASE_NONE 
        /// or above values.
        /// </summary>
        [StaticSize(4)]
        public LeaseStateValues NewLeaseState;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The server MUST set this to 0, 
        /// and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint BreakReason;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The server MUST set this to 0, 
        /// and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint AccessMaskHint;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The server MUST set this to 0, 
        /// and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint ShareMaskHint;
    }
    #endregion

    /// <summary>
    ///  The SMB2 OPLOCK_BREAK Acknowledgment packet is sent
    ///  by the client in response to an SMB2 OPLOCK_BREAK notification
    ///  packet sent by the server. The server responds to
    ///  an oplock break acknowledgment with an SMB2 OPLOCK_BREAK
    ///  response. The client MUST NOT send an oplock break
    ///  acknowledgment for an oplock break from level II to
    ///  none. A break from level II MUST always transition
    ///  to none. Thus, the client does not have to send a
    ///  request to the server because there is no question
    ///  how the transition was made.
    /// </summary>
    public partial struct OPLOCK_BREAK_Acknowledgment
    {
        /// <summary>
        ///  The client MUST set this to 24, indicating the size
        ///  of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The resulting oplock level. This MUST be at least as
        ///  permissive as the level that is specified by the server
        ///  in its initial oplock break notification packet.  For
        ///  example, if the server specifies an SMB2_OPLOCK_LEVEL_II,
        ///  the client can respond with an SMB2_OPLOCK_LEVEL_II
        ///  or an SMB2_OPLOCK_LEVEL_NONE. Because SMB2_OPLOCK_LEVEL_BATCH
        ///  is the highest oplock  level, the server will never
        ///  send a break from SMB2_OPLOCK_LEVEL_BATCH to SMB2_OPLOCK_LEVEL_BATCH.
        ///  Thus this field MUST contain one of the following values:-based
        ///  clients never use exclusive oplocks.  There are no
        ///  situations where an exclusive oplock would be used
        ///  instead of using a SMB2_OPLOCK_LEVEL_BATCH.
        /// </summary>
        public OPLOCK_BREAK_Acknowledgment_OplockLevel_Values OplockLevel;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        public OPLOCK_BREAK_Acknowledgment_Reserved_Values Reserved;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public OPLOCK_BREAK_Acknowledgment_Reserved2_Values Reserved2;

        /// <summary>
        ///  An SMB2_FILEID that identifies  the file or pipe on
        ///  which the oplock break is being acknowledged.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;
    }

    /// <summary>
    /// OPLOCK_BREAK_Acknowledgment_OplockLevel_Values
    /// </summary>
    public enum OPLOCK_BREAK_Acknowledgment_OplockLevel_Values : byte
    {

        /// <summary>
        ///  The client has lowered its oplock level for this file
        ///  to none.
        /// </summary>
        OPLOCK_LEVEL_NONE = 0x00,

        /// <summary>
        ///  The client has lowered its oplock level for this file
        ///  to level II.
        /// </summary>
        OPLOCK_LEVEL_II = 0x01,
    }

    /// <summary>
    /// OPLOCK_BREAK_Acknowledgment_Reserved_Values
    /// </summary>
    public enum OPLOCK_BREAK_Acknowledgment_Reserved_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// OPLOCK_BREAK_Acknowledgment_Reserved2_Values
    /// </summary>
    public enum OPLOCK_BREAK_Acknowledgment_Reserved2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    #region LEASE BREAK Acknowledgment

    /// <summary>
    /// The SMB2 Lease Break Acknowledgment packet is sent by the client in response to an SMB2
    /// Lease Break Notification packet sent by the server. This request is valid only in the SMB 2.1 dialect.
    /// The server responds to a lease break acknowledgment with an SMB2 Lease Break Response.
    /// </summary>
    public partial struct LEASE_BREAK_Acknowledgment
    {
        /// <summary>
        ///  The client MUST set this to 36, indicating the size of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  This field MUST NOT be used and MUST be reserved. The client MUST set this to 0, 
        ///  and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  The field MUST be constructed using the following values.
        /// </summary>
        [StaticSize(4)]
        public uint Flags;

        /// <summary>
        /// A unique key which identifies the owner of the lease.
        /// </summary>
        [StaticSize(16)]
        public Guid LeaseKey;

        /// <summary>
        /// The current lease state of the open. This field MUST be constructed using the following values.
        /// </summary>
        [StaticSize(4)]
        public LeaseStateValues LeaseState;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The client MUST set this to 0, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong LeaseDuration;
    }
    #endregion

    /// <summary>
    ///  The SMB2 OPLOCK_BREAK Response packet is sent by the
    ///  server in response to an oplock break acknowledgment
    ///  from the client. This response is composed of an SMB2
    ///  header, as specified in section , followed by this
    ///  response structure:
    /// </summary>
    public partial struct OPLOCK_BREAK_Response
    {
        /// <summary>
        ///  The server MUST set this to 24, indicating the size
        ///  of the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The server MUST set this value to the OplockLevel value
        ///  in the SMB2 OPLOCK BREAK acknowledgment that is sent
        ///  by the client as specified in section.  Because SMB2_OPLOCK_LEVEL_BATCH
        ///  is the highest oplock  level, the server will never
        ///  send a break from SMB2_OPLOCK_LEVEL_BATCH to SMB2_OPLOCK_LEVEL_BATCH.
        ///  Thus this field MUST contain one of the following values:-based
        ///  clients never use exclusive oplocks.  There are no
        ///  situations where an exclusive oplock would be used
        ///  instead of using a SMB2_OPLOCK_LEVEL_BATCH.
        /// </summary>
        public OPLOCK_BREAK_Response_OplockLevel_Values OplockLevel;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        public OPLOCK_BREAK_Response_Reserved_Values Reserved;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public OPLOCK_BREAK_Response_Reserved2_Values Reserved2;

        /// <summary>
        ///  A SMB2_FILEID, as specified in section.		The identifier
        ///  of the file or pipe, which MUST be the same as the
        ///  FileId in the client's oplock break acknowledgment
        ///  packet as specified in section.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;
    }

    /// <summary>
    /// OPLOCK_BREAK_Response_OplockLevel_Values
    /// </summary>
    public enum OPLOCK_BREAK_Response_OplockLevel_Values : byte
    {

        /// <summary>
        ///  No oplock is available.
        /// </summary>
        OPLOCK_LEVEL_NONE = 0x00,

        /// <summary>
        ///  A level II oplock is available.
        /// </summary>
        OPLOCK_LEVEL_II = 0x01,
    }

    /// <summary>
    /// OPLOCK_BREAK_Response_Reserved_Values
    /// </summary>
    public enum OPLOCK_BREAK_Response_Reserved_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// OPLOCK_BREAK_Response_Reserved2_Values
    /// </summary>
    public enum OPLOCK_BREAK_Response_Reserved2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    #region LEASE BREAK Response

    /// <summary>
    /// The SMB2 Lease Break Acknowledgment packet is sent by the client in response to an SMB2
    /// Lease Break Notification packet sent by the server. This request is valid only in the SMB 2.1 dialect.
    /// The server responds to a lease break acknowledgment with an SMB2 Lease Break Response.
    /// </summary>
    public partial struct LEASE_BREAK_Response
    {
        /// <summary>
        ///  The client MUST set this to 36, indicating the size of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  This field MUST NOT be used and MUST be reserved. The client MUST set this to 0, 
        ///  and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  The field MUST be constructed using the following values.
        /// </summary>
        [StaticSize(4)]
        public uint Flags;

        /// <summary>
        /// A unique key which identifies the owner of the lease.
        /// </summary>
        [StaticSize(16)]
        public Guid LeaseKey;

        /// <summary>
        /// The current lease state of the open. This field MUST be constructed using the following values.
        /// </summary>
        [StaticSize(4)]
        public LeaseStateValues LeaseState;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The client MUST set this to 0, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong LeaseDuration;
    }
    #endregion

    /// <summary>
    ///  The SMB2 LOCK Request packet is sent by the client to
    ///  either lock or unlock portions of a file.  Several
    ///  different segments of the file can be affected with
    ///  a single  		 	SMB2 LOCK Request packet, but they all
    ///  must be within the same file.
    /// </summary>
    public partial struct LOCK_Request
    {

        /// <summary>
        ///  The client MUST set this to 48, indicating the size
        ///  of an SMB2_LOCK response with a single SMB2_LOCK structure.
        ///   This value is set regardless of the number of locks
        ///  that are sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  MUST be set to the number of SMB2_LOCK  structures that
        ///  are contained in the Locks[] array. The lock count
        ///  MUST be greater than or equal to 1.
        /// </summary>
        [StaticSize(2)]
        public ushort LockCount;

        /// <summary>
        ///  In the SMB 2.002 dialect, this field is unused and MUST be treated as reserved. 
        ///  The client MUST set this to 0, and the server MUST ignore it on receipt. In 
        ///  the SMB 2.1 dialect, this field indicates a value that identifies a lock or 
        ///  unlock request uniquely across all lock or unlock requests that are sent on 
        ///  the same file.
        /// </summary>
        [StaticSize(4)]
        public uint LockSequence;

        /// <summary>
        ///  An SMB2_FILEID that identifies the file on which
        ///  to perform the byte range locks or unlocks.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;
    }

    /// <summary>
    ///  The SMB2 LOCK_ELEMENT Structure packet is used by the
    ///  SMB2 LOCK Request packet to indicate segments of files
    ///  that should be locked or unlocked.
    /// </summary>
    public partial struct LOCK_ELEMENT
    {

        /// <summary>
        ///  The starting offset, in bytes, in the destination file
        ///  from where the range being locked or unlocked starts.
        /// </summary>
        [StaticSize(8)]
        public ulong Offset;

        /// <summary>
        ///  The length, in bytes, of the range being locked or unlocked.
        /// </summary>
        [StaticSize(8)]
        public ulong Length;

        /// <summary>
        ///  The description of how the range is being locked or
        ///  unlocked and how the operation must behave. This field
        ///  takes the following format:
        /// </summary>
        [StaticSize(4)]
        public LOCK_ELEMENT_Flags_Values Flags;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public LOCK_ELEMENT_Reserved_Values Reserved;
    }

    /// <summary>
    /// LOCK_ELEMENT_Flags_Values
    /// </summary>
    [Flags()]
    public enum LOCK_ELEMENT_Flags_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The range MUST be locked shared, allowing other opens
        ///  to read from or take a shared lock on the range.  Other
        ///  opens MUST NOT be allowed to write within the range.
        ///  Other locks can be requested and taken on this range.
        /// </summary>
        LOCKFLAG_SHARED_LOCK = 0x01,

        /// <summary>
        ///  The range MUST be locked exclusive, not allowing other
        ///  opens to read, write, or lock within the range.
        /// </summary>
        LOCKFLAG_EXCLUSIVE_LOCK = 0x02,

        /// <summary>
        ///  The range MUST be unlocked from a previous lock taken
        ///  on this range. The unlock range MUST be identical to
        ///  the lock range. Sub-ranges cannot be unlocked.
        /// </summary>
        LOCKFLAG_UNLOCK = 0x04,

        /// <summary>
        ///  The lock operation MUST fail immediately if it conflicts
        ///  with an existing lock, instead of waiting for the range
        ///  to become available.
        /// </summary>
        LOCKFLAG_FAIL_IMMEDIATELY = 0x10,
    }

    /// <summary>
    /// LOCK_ELEMENT_Reserved_Values
    /// </summary>
    public enum LOCK_ELEMENT_Reserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 LOCK Response packet is sent by a server in
    ///  response to an SMB2 LOCK Request packet. This response
    ///  is composed of an SMB2 header, as specified in section
    ///  , followed by this request structure:
    /// </summary>
    public partial struct LOCK_Response
    {

        /// <summary>
        ///  The server MUST set this to 4, indicating the size of
        ///  the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;
    }

    /// <summary>
    ///  The SMB2 ECHO Request packet is sent by a client to
    ///  determine whether a server is processing requests.
    ///   This request is composed of an SMB2 header, as specified
    ///  in section , followed by this request structure:
    /// </summary>
    public partial struct ECHO_Request
    {

        /// <summary>
        ///  The client MUST set this to 4, indicating the size of
        ///  the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;
    }

    /// <summary>
    ///  The SMB2 ECHO Response packet.
    /// </summary>
    public partial struct ECHO_Response
    {

        /// <summary>
        ///  The server MUST set this to 4, indicating the size of
        ///  the response structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;
    }

    /// <summary>
    ///  The Symbolic Link Error Response is used to describe
    ///  the target path that the client MUST follow when a
    ///  symbolic link is encountered on create. This structure
    ///  is contained in the ErrorData section of the SMB2 ERROR
    ///  Response. This structure MUST NOT be returned in an
    ///  SMB2 ERROR Response unless the Status code in the header
    ///  of that response is set to STATUS_STOPPED_ON_SYMLINK.
    ///   The structure has the following format:
    /// </summary>
    public partial struct Symbolic_Link_Error_Response
    {
        /// <summary>
        /// The length, in bytes, of the response including the variable-length 
        /// portion and excluding SymLinkLength.
        /// </summary>
        [StaticSize(4)]
        public uint SymLinkLength;

        /// <summary>
        /// The server MUST set this field to 0x4C4D5953.
        /// </summary>
        [StaticSize(4)]
        public SymLinkErrorTag_Values SymLinkErrorTag;

        /// <summary>
        ///  The type of link encountered. The server MUST set this
        ///  field to 0xA000000C.
        /// </summary>
        [StaticSize(4)]
        public ReparseTag_Values ReparseTag;

        /// <summary>
        ///  The length, in bytes, of the variable-length portion
        ///  of the symbolic link error response plus the size of
        ///  the static portion, not including ReparseTag, ReparseDataLength,
        ///  and Reserved. The server MUST set this to the size
        ///  of PathBuffer[], in bytes, plus 12.  (12 is the size
        ///  of SubstituteNameOffset, SubstituteNameLength, PrintNameOffset,
        ///  PrintNameLength, and Flags.)
        /// </summary>
        [StaticSize(2)]
        public ushort ReparseDataLength;

        /// <summary>
        ///  The length, in bytes, of the unparsed portion of the path. 
        ///  The unparsed portion of the path is the length of the path following the symbolic link.
        /// </summary>
        [StaticSize(2)]
        public ushort UnparsedPathLength;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the error
        ///  response of the substitute name string in the PathBuffer
        ///  array. The substitute name is the name the client
        ///  MUST use to access this file if it wants to follow
        ///  the symbolic link.
        /// </summary>
        [StaticSize(2)]
        public ushort SubstituteNameOffset;

        /// <summary>
        ///  The length, in bytes, of the substitute name string.
        ///   SubstituteNameLength does not include space for the
        ///  UNICODE_NULL character.
        /// </summary>
        [StaticSize(2)]
        public ushort SubstituteNameLength;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the error
        ///  response of the print name string in the PathBuffer
        ///  array. The print name is the user friendly name the
        ///  client MUST return to the application if it requests
        ///  the name of the file being opened when the symbolic
        ///  link was followed.  
        /// </summary>
        [StaticSize(2)]
        public ushort PrintNameOffset;

        /// <summary>
        ///  The length, in bytes, of the print name string.  PrintNameLength
        ///  does not include space for the UNICODE_NULL character.
        /// </summary>
        [StaticSize(2)]
        public ushort PrintNameLength;

        /// <summary>
        ///  The server MAY set this field to any value. The client
        ///  MUST ignore it.
        /// </summary>
        [StaticSize(4)]
        public uint Flags;

        /// <summary>
        ///  A buffer that contains the Unicode string for both the
        ///  substitute name and the print name, as described by
        ///  SubstituteNameOffset, SubstituteNameLength, PrintNameOffset,
        ///  and PrintNameLength. The string MUST be a Unicode
        ///  path to the target of the symbolic link.For an absolute
        ///  target that is on a remote machine, the server MUST
        ///  return the path in the format \??\UNC\server\share\...
        ///  where server is replaced by the target server name,
        ///  share is replaced by the target share name, and ...
        ///  is replaced by the remainder of the path to the target.For
        ///  an absolute target that is on the local disk of the
        ///  client machine, the server MUST return the path in
        ///  the form \??\C:\... where C: is the drive mount point
        ///  on the local system and ... is replaced by the remainder
        ///  of the path to the target.For a relative target, the
        ///  server MUST return a path that does not start with
        ///  \. The path can contain either "." or ".." to refer
        ///  to the current or parent directory, and may contain
        ///  multiple elements.  
        /// </summary>
        [Size("SubstituteNameLength + PrintNameLength")]
        public byte[] PathBuffer;
    }


    /// <summary>
    /// Enum of SymLinkErrorTag field in Symbolic_Link_Error_Response
    /// </summary>
    public enum SymLinkErrorTag_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x4C4D5953,
    }


    /// <summary>
    /// Enum of ReparseTag field in Symbolic_Link_Error_Response
    /// </summary>
    public enum ReparseTag_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0xa000000c,
    }

    /// <summary>
    /// Servers which negotiate SMB 3.1.1 or higher can return this error context
    /// to a client in response to a tree connect request
    /// with the SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER bit set
    /// in the Flags field of the SMB2 TREE_CONNECT request.
    /// </summary>
    public partial struct Share_Redirect_Error_Context_Response
    {
        /// <summary>
        /// This field MUST be set to the size of the structure.
        /// </summary>
        [StaticSize(4)]
        public uint StructureSize;

        /// <summary>
        /// This field MUST be set to 3.
        /// </summary>
        [StaticSize(4)]
        public uint NotificationType;

        /// <summary>
        /// The offset from the start of this structure to the ResourceName field.
        /// </summary>
        [StaticSize(4)]
        public uint ResourceNameOffset;

        /// <summary>
        /// The length of the share name provided in the ResourceName field, in bytes.
        /// </summary>
        [StaticSize(4)]
        public uint ResourceNameLength;

        /// <summary>
        /// This field MUST be set to zero.
        /// </summary>
        [StaticSize(2)]
        public ushort Flags;

        /// <summary>
        /// This field MUST be set to zero.
        /// </summary>
        [StaticSize(2)]
        public ushort TargetType;

        /// <summary>
        /// The number of MOVE_DST_IPADDR structures in the IPAddrMoveList field.
        /// </summary>
        [StaticSize(4)]
        public uint IPAddrCount;

        /// <summary>
        /// Array of MOVE_DST_IPADDR structures, as specified in section 2.2.2.2.2.1.
        /// </summary>
        [Size("IPAddrCount")]
        public Move_Dst_IpAddr[] IPAddrMoveList;

        /// <summary>
        /// Name of the share as a counted Unicode string, as specified in [MS-DTYP] section 2.3.10.
        /// </summary>
        [Size("ResourceNameLength")]
        public byte[] ResourceName;
    }

    /// <summary>
    /// The MOVE_DST_IPADDR structure is used in Share Redirect Error Context Response
    /// to indicate the destination IP address.
    /// </summary>
    public struct Move_Dst_IpAddr
    {
        /// <summary>
        /// This field indicates the type of destination IP address.
        /// </summary>
        [StaticSize(4)]
        public Move_Dst_IpAddr_Type Type;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved.
        /// The server SHOULD set this field to zero, and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved;

        /// <summary>
        /// This field is interpreted in different ways depending on the type of IP address passed in.
        /// If the value of the Type field is MOVE_DST_IPADDR_V4, this field is the IPv4Address field followed by Reserved2 fields.
        /// If the value of the Type field is MOVE_DST_IPADDR_V6, this field is the IPv6Address field.
        /// </summary>
        [StaticSize(16)]
        public byte[] IPv6Address;
    }

    /// <summary>
    /// Enum of Type field in Move_Dst_IpAddr
    /// </summary>
    public enum Move_Dst_IpAddr_Type : uint
    {
        /// <summary>
        /// The type of destination IP address in this structure is IPv4 address.
        /// The fields after Reserved field in this structure are interpreted as IPv4Address
        /// followed by Reserved2 as described below.
        /// </summary>
        MOVE_DST_IPADDR_V4 = 0x00000001,

        /// <summary>
        /// The type of destination IP address in this structure is IPv6 address.
        /// </summary>
        MOVE_DST_IPADDR_V6 = 0x00000002
    }

    /// <summary>
    ///  The SMB2 NEGOTIATE Request packet is used by the client
    ///  to notify the server what dialects of the SMB 2.0 Protocol
    ///  the client understands. This request is composed of
    ///  an SMB 2.0 Protocol header, as specified in section
    ///  , followed by this request structure:
    /// </summary>    
    [StructLayout(LayoutKind.Explicit, Size = 36)]
    public partial struct NEGOTIATE_Request
    {
        /// <summary>
        ///  The client MUST set this field to 36, indicating the
        ///  size of a NEGOTIATE request. This is not the size of
        ///  the structure with a single dialect in the Dialects[]
        ///  array. This value MUST be set regardless of the number
        ///  of dialects sent.
        /// </summary>
        [FieldOffset(0)]
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The number of dialects that are contained in the Dialects[]
        ///  array. This value MUST be greater than 0.
        /// </summary>
        [FieldOffset(2)]
        [StaticSize(2)]
        public ushort DialectCount;

        /// <summary>
        ///  The security mode field MUST be constructed by using
        ///  the following values:
        /// </summary>
        [FieldOffset(4)]
        [StaticSize(2)]
        public SecurityMode_Values SecurityMode;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [FieldOffset(6)]
        [StaticSize(2)]
        public NEGOTIATE_Request_Reserved_Values Reserved;

        /// <summary>
        ///  Specifies protocol capabilities for the client. This
        ///  field MUST be constructed by using the following values:
        /// </summary>
        [FieldOffset(8)]
        [StaticSize(4)]
        public Capabilities_Values Capabilities;

        /// <summary>
        ///  Unused at the present, and MUST be treated as reserved.
        ///  The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [FieldOffset(12)]
        [StaticSize(16)]
        public System.Guid ClientGuid;

        /// <summary>
        /// This field is interpreted in different ways depending on the SMB2 Dialects field:
        /// If the Dialects field does not contain 0x0311, this field is interpreted as ClientStartTime,
        /// Otherwise, this field is interpreted as the NegotiateContextOffset, NegotiateContextCount, and Reserved2 fields.
        /// </summary>
        [FieldOffset(28)]
        [StaticSize(8)]
        public _FILETIME ClientStartTime;

        /// <summary>
        /// This field is interpreted in different ways depending on the SMB2 Dialects field
        /// If the Dialects field contains 0x0311, this field is interpreted as NegotiateContextOffset        
        /// </summary>
        [FieldOffset(28)]
        [StaticSize(4)]
        public uint NegotiateContextOffset;

        /// <summary>
        /// This field is interpreted in different ways depending on the SMB2 Dialects field
        /// If the Dialects field contains 0x0311, this field is interpreted as NegotiateContextCount        
        /// </summary>
        [FieldOffset(32)]
        [StaticSize(2)]
        public ushort NegotiateContextCount;

        /// <summary>
        /// This field is interpreted in different ways depending on the SMB2 Dialects field
        /// If the Dialects field contains 0x0311, this field is interpreted as Reserved2        
        /// </summary>
        [FieldOffset(34)]
        [StaticSize(2)]
        public ushort Reserved2;
    }

    /// <summary>
    /// SecurityMode_Values
    /// </summary>
    [Flags()]
    public enum SecurityMode_Values : ushort
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates that security signatures are enabled
        ///  on the client.
        /// </summary>
        NEGOTIATE_SIGNING_ENABLED = 0x0001,

        /// <summary>
        ///  When set, indicates that security signatures are required
        ///  by the client.
        /// </summary>
        NEGOTIATE_SIGNING_REQUIRED = 0x0002,
    }

    /// <summary>
    /// NEGOTIATE_Request_Reserved_Values
    /// </summary>
    public enum NEGOTIATE_Request_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// The SMB2_NEGOTIATE_CONTEXT structure is used by the SMB2 NEGOTIATE Request and 
    /// the SMB2 NEGOTIATE Response to encode additional properties.
    /// </summary>
    public struct SMB2_NEGOTIATE_CONTEXT_Header
    {
        /// <summary>
        /// Specifies the type of context in the Data field.
        /// </summary>
        public SMB2_NEGOTIATE_CONTEXT_Type_Values ContextType;

        /// <summary>
        /// The length, in bytes, of the context data.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. 
        /// This value MUST be set to 0 by the client, and MUST be ignored by the server.
        /// </summary>
        public uint Reserved;

    }

    /// <summary>
    /// The possible types of context in the Data field of SMB2_NEGOTIATE_CONTEXT.
    /// </summary>
    public enum SMB2_NEGOTIATE_CONTEXT_Type_Values : ushort
    {
        /// <summary>
        /// The Data field contains a list of preauthentication integrity hash functions 
        /// as well as an optional salt value, as specified in section 2.2.3.1.1.
        /// </summary>
        SMB2_PREAUTH_INTEGRITY_CAPABILITIES = 0x0001,

        /// <summary>
        /// The Data field contains a list of encryption algorithms, as specified in section 2.2.3.1.2.
        /// </summary>
        SMB2_ENCRYPTION_CAPABILITIES = 0x0002,

        /// <summary>
        /// The Data field contains a list of compression algorithms.
        /// </summary>
        SMB2_COMPRESSION_CAPABILITIES = 0x0003,

        /// <summary>
        /// The Data field contains the server name to which the client connects.
        /// </summary>
        SMB2_NETNAME_NEGOTIATE_CONTEXT_ID = 0x0005,
    }

    /// <summary>
    /// The SMB2_PREAUTH_INTEGRITY_CAPABILITIES context is specified in an SMB2 NEGOTIATE request 
    /// by the client to indicate which preauthentication integrity hash algorithms the client 
    /// supports and to optionally supply a preauthentication integrity hash salt value. 
    /// </summary>
    public struct SMB2_PREAUTH_INTEGRITY_CAPABILITIES
    {
        /// <summary>
        /// Header.
        /// </summary>
        public SMB2_NEGOTIATE_CONTEXT_Header Header;

        /// <summary>
        /// The number of hash algorithms in the HashAlgorithms array. This value MUST be greater than zero.
        /// </summary>
        public ushort HashAlgorithmCount;

        /// <summary>
        /// The size, in bytes, of the Salt field.
        /// </summary>
        public ushort SaltLength;

        /// <summary>
        /// An array of HashAlgorithmCount 16-bit integer IDs specifying the supported 
        /// preauthentication integrity hash functions. 
        /// The following IDs are defined.
        /// </summary>
        [Size("HashAlgorithmCount")]
        public PreauthIntegrityHashID[] HashAlgorithms;

        /// <summary>
        /// A buffer containing the salt value of the hash.
        /// </summary>
        [Size("SaltLength")]
        public byte[] Salt;

        /// <summary>
        /// Get the data length
        /// </summary>
        /// <returns>The data length of this context.</returns>
        public int GetDataLength()
        {
            int dataLength = sizeof(ushort) + sizeof(ushort); // HashAlgorithmCount + SaltLength
            if (HashAlgorithms != null) dataLength += sizeof(PreauthIntegrityHashID) * HashAlgorithms.Length;
            if (Salt != null) dataLength += Salt.Length;
            return dataLength;
        }
    }

    /// <summary>
    /// The SMB2_ENCRYPTION_CAPABILITIES context is specified in an SMB2 NEGOTIATE request 
    /// by the client to indicate which encryption algorithms the client supports.
    /// </summary>
    public struct SMB2_ENCRYPTION_CAPABILITIES
    {
        /// <summary>
        /// Header.
        /// </summary>
        public SMB2_NEGOTIATE_CONTEXT_Header Header;

        /// <summary>
        /// The number of ciphers in the Ciphers array. This value MUST be greater than zero.
        /// </summary>
        public ushort CipherCount;

        /// <summary>
        /// An array of CipherCount 16-bit integer IDs specifying the supported encryption algorithms. 
        /// </summary>
        [Size("CipherCount")]
        public EncryptionAlgorithm[] Ciphers;

        /// <summary>
        /// Get the data length
        /// </summary>
        /// <returns>The data length of this context.</returns>
        public int GetDataLength()
        {
            int dataLength = sizeof(ushort); // CipherCount
            if (Ciphers != null) dataLength += Ciphers.Length * sizeof(EncryptionAlgorithm);
            return dataLength;
        }
    }

    public enum CompressionAlgorithm : short
    {
        /// <summary>
        /// No compression
        /// </summary>
        NONE = 0x0000,

        /// <summary>
        /// LZNT1 compression algorithm
        /// </summary>
        LZNT1 = 0x0001,

        /// <summary>
        /// LZ77 compression algorithm
        /// </summary>
        LZ77 = 0x0002,

        /// <summary>
        /// LZ77+Huffman compression algorithm
        /// </summary>
        LZ77Huffman = 0x0003,

        /// <summary>
        /// Not a real compression algorithm value, SHOULD be unsupported
        /// </summary>
        Unsupported = 0x00FF,
    }

    /// <summary>
    /// The SMB2_COMPRESSION_CAPABILITIES context is specified in an SMB2 NEGOTIATE request by the client
    /// to indicate which compression algorithms the client supports.
    /// </summary>
    public struct SMB2_COMPRESSION_CAPABILITIES
    {
        /// <summary>
        /// Header.
        /// </summary>
        public SMB2_NEGOTIATE_CONTEXT_Header Header;

        /// <summary>
        /// The number of elements in CompressionAlgorithms array.
        /// </summary>
        public ushort CompressionAlgorithmCount;

        /// <summary>
        /// The sender MUST set this to 0, and the receiver MUST ignore it on receipt.
        /// </summary>
        public ushort Padding;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved.
        /// The sender MUST set this to 0, and the receiver MUST ignore it on receipt.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// An array of 16-bit integer IDs specifying the supported compression algorithms.
        /// These IDs MUST be in order of preference from most to least.
        /// </summary>
        [Size("CompressionAlgorithmCount")]
        public CompressionAlgorithm[] CompressionAlgorithms;

        public int GetDataLength()
        {
            int dataLength = Marshal.SizeOf(CompressionAlgorithmCount) + Marshal.SizeOf(Padding) + Marshal.SizeOf(Reserved);
            if (CompressionAlgorithms != null)
            {
                dataLength += CompressionAlgorithms.Length * sizeof(CompressionAlgorithm);
            }
            return dataLength;
        }
    }

    /// <summary>
    /// The SMB2_NETNAME_NEGOTIATE_CONTEXT_ID context is specified in an SMB2 NEGOTIATE request to indicate the server name the client connects to.
    /// The server MUST ignore this context.
    /// </summary>
    public class SMB2_NETNAME_NEGOTIATE_CONTEXT_ID
    {
        /// <summary>
        /// Header.
        /// </summary>
        public SMB2_NEGOTIATE_CONTEXT_Header Header;

        /// <summary>
        /// A null-terminated Unicode string containing the server name and specified by the client application.
        /// </summary>
        public char[] NetName;

        /// <summary>
        /// Unmarshal SMB2_NETNAME_NEGOTIATE_CONTEXT_ID from input byte array.
        /// </summary>
        /// <param name="data">Input byte array containing SMB2_NETNAME_NEGOTIATE_CONTEXT_ID.</param>
        /// <param name="consumedLen">Offset of byte array.</param>
        /// <returns>SMB2_NETNAME_NEGOTIATE_CONTEXT_ID which is unmarshaled.</returns>
        internal static SMB2_NETNAME_NEGOTIATE_CONTEXT_ID Unmarshal(byte[] data, ref int consumedLen)
        {
            var result = new SMB2_NETNAME_NEGOTIATE_CONTEXT_ID();

            result.Header = TypeMarshal.ToStruct<SMB2_NEGOTIATE_CONTEXT_Header>(data, ref consumedLen);

            if (result.Header.DataLength % sizeof(char) != 0)
            {
                throw new InvalidOperationException("DataLength is invalid!");
            }

            result.NetName = new char[result.Header.DataLength / sizeof(char)];
            for (int i = 0; i < result.NetName.Length; i++)
            {
                result.NetName[i] = TypeMarshal.ToStruct<char>(data, ref consumedLen);
            }

            return result;
        }

        /// <summary>
        /// Marshal SMB2_NETNAME_NEGOTIATE_CONTEXT_ID into byte array.
        /// </summary>
        /// <returns>Byte array containing marshaled result.</returns>
        public byte[] Marshal()
        {
            var result = new List<byte>();

            result.AddRange(TypeMarshal.ToBytes(Header));

            result.AddRange(NetName.SelectMany(x => TypeMarshal.ToBytes(x)));

            return result.ToArray();
        }

        /// <summary>
        /// Get the data length
        /// </summary>
        /// <returns>The data length of this context.</returns>
        public ushort GetDataLength()
        {            
            return (ushort)NetName.Length;
        }
    }


    /// <summary>
    ///  The SMB2 CANCEL Request packet is sent by the client
    ///  to cancel a previously sent message on the same SMB2
    ///  transport connection. The MessageId of the request
    ///  to be canceled MUST be set in the SMB2 header of the
    ///  request. This request is composed of an SMB2 header,
    ///  as specified in section , followed by this request
    ///  structure:
    /// </summary>
    public partial struct CANCEL_Request
    {

        /// <summary>
        ///  The client MUST set this field to 4, indicating the
        ///  size of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;
    }

    /// <summary>
    ///  The SMB2 IOCTL Request packet is sent by a client to
    ///  issue an implementation-specific file system control
    ///  or device control (FSCTL/IOCTL) command across the
    ///  network.  For a list of  IOCTL operations, see section
    ///   and [MS-FSCC] section 2.3.This request is composed
    ///  of an SMB2 header, as specified in section , followed
    ///  by this request structure:
    /// </summary>
    public partial struct IOCTL_Request
    {

        /// <summary>
        ///  The client MUST set this field to 57, indicating the
        ///  size of the request structure, not including the header.
        ///   The client MUST set this field to this value regardless
        ///  of how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  The control code of the FSCTL/IOCTL method. The values
        ///  are listed in subsequent sections, and in [MS-FSCC]
        ///  section 2.3. The following values indicate SMB2-specific
        ///  processing as specified in sections  and :
        /// </summary>
        [StaticSize(4)]
        public CtlCode_Values CtlCode;

        /// <summary>
        ///  An SMB2_FILEID identifier of the file on which to perform
        ///  the command.  For certain IOCTL or FSCTL values, as
        ///  specified in section , this parameter MUST be ignored.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the input data buffer.  If no input data
        ///  is required for the FSCTL/IOCTL command being issued,
        ///  this value MUST be set to 0.
        /// </summary>
        [StaticSize(4)]
        public uint InputOffset;

        /// <summary>
        ///  The size, in bytes, of the input data.
        /// </summary>
        [StaticSize(4)]
        public uint InputCount;

        /// <summary>
        ///  The maximum number of bytes that the server can return
        ///  for the input data in the SMB2IOCTL response.
        /// </summary>
        [StaticSize(4)]
        public uint MaxInputResponse;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the output data buffer.  If no output data
        ///  is required for the FSCTL/IOCTL command being issued,
        ///  this value MUST be set to 0.
        /// </summary>
        [StaticSize(4)]
        public uint OutputOffset;

        /// <summary>
        ///  The size, in bytes, of the output data.
        /// </summary>
        [StaticSize(4)]
        public uint OutputCount;

        /// <summary>
        ///  The maximum number of bytes that the server can return
        ///  for the output data in the SMB2IOCTL response.
        /// </summary>
        [StaticSize(4)]
        public uint MaxOutputResponse;

        /// <summary>
        ///  Flags indicating how the operation must be processed.
        ///   This field MUST be constructed by using the following
        ///  values:
        /// </summary>
        [StaticSize(4)]
        public IOCTL_Request_Flags_Values Flags;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public IOCTL_Request_Reserved2_Values Reserved2;
    }

    /// <summary>
    /// CtlCode_Values
    /// </summary>
    public enum CtlCode_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        FSCTL_DFS_GET_REFERRALS = 0x00060194,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FSCTL_PIPE_PEEK = 0x0011400c,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FSCTL_PIPE_WAIT = 0x00110018,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FSCTL_PIPE_TRANSCEIVE = 0x0011c017,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FSCTL_SRV_COPYCHUNK = 0x001440f2,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FSCTL_SRV_ENUMERATE_SNAPSHOTS = 0x00144064,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FSCTL_SRV_REQUEST_RESUME_KEY = 0x00140078,

        /// <summary>
        /// Possible value.
        /// </summary>
        FSCTL_SRV_READ_HASH = 0x001441bb,

        /// <summary>
        /// Possible value.
        /// </summary>
        FSCTL_SRV_COPYCHUNK_WRITE = 0x001480F2,

        /// <summary>
        /// Possible value.
        /// </summary>
        FSCTL_SRV_NOTIFY_TRANSACTION = 0x00148118,

        /// <summary>
        /// Possible value.
        /// </summary>
        FSCTL_LMR_REQUEST_RESILIENCY = 0x001401D4,

        FSCTL_QUERY_NETWORK_INTERFACE_INFO = 0x001401FC,

        /// <summary>
        /// Overdue value, please don't use.
        /// </summary>
        FSCTL_IS_PATHNAME_VALID = 0x0009002c,

        /// <summary>
        ///  Overdue value, please don't use.
        /// </summary>
        FSCTL_GET_NTFS_VOLUME_DATA = 0x00090064,

        FSCTL_OFFLOAD_READ = 0x00094264,

        FSCTL_OFFLOAD_WRITE = 0x00098268,

        FSCTL_VALIDATE_NEGOTIATE_INFO = 0x00140204,

        FSCTL_FILE_LEVEL_TRIM = 0x00098208,

        FSCTL_GET_INTEGRITY_INFORMATION = 0x0009027C,

        FSCTL_SET_INTEGRITY_INFORMATION = 0x0009C280,

        FSCTL_DFS_GET_REFERRALS_EX = 0x000601B0,

        FSCTL_QUERY_SHARED_VIRTUAL_DISK_SUPPORT = 0x00090300,

        FSCTL_SVHDX_SYNC_TUNNEL_REQUEST = 0x00090304,

        FSCTL_SET_ZERO_DATA = 0x000980c8,

        FSCTL_DUPLICATE_EXTENTS_TO_FILE = 0x00098344,

        FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX = 0x000983E8,

        /// <summary>
        /// Control code for STORAGE_QOS_CONTROL_REQUEST
        /// </summary>
        FSCTL_STORAGE_QOS_CONTROL = 0x00090350,

        /// <summary>
        /// Control code for SVHDX_META_OPERATION_START_REQUEST
        /// </summary>
        FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST = 0x00090364
    }

    /// <summary>
    /// IOCTL_Request_Flags_Values
    /// </summary>
    [Flags()]
    public enum IOCTL_Request_Flags_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  If set, the request is an FSCTL request.  If not set,
        ///  the request is an IOCTL request.
        /// </summary>
        SMB2_0_IOCTL_IS_FSCTL = 0x00000001,
    }

    /// <summary>
    /// IOCTL_Request_Reserved2_Values
    /// </summary>
    public enum IOCTL_Request_Reserved2_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SRV_COPYCHUNK packet is sent in a SRV_COPYCHUNK_COPY
    ///  packet by the client to initiate a server-side copy
    ///  of data to describe an individual data range to copy.
    ///   It is set as the contents of the input data buffer.
    ///  This packet consists of the following:
    /// </summary>
    public partial struct SRV_COPYCHUNK
    {
        /// <summary>
        ///  The offset, in bytes, from the beginning of the source
        ///  file to where to copy the data.
        /// </summary>
        [StaticSize(8)]
        public ulong SourceOffset;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the destination
        ///  file to where the data will be copied.
        /// </summary>
        [StaticSize(8)]
        public ulong TargetOffset;

        /// <summary>
        ///  The number of bytes of data to copy.
        /// </summary>
        [StaticSize(4)]
        public uint Length;

        /// <summary>
        /// Unused and MUST be treated as reserved. SHOULD be set to zero and MUST be ignored.
        /// </summary>
        [StaticSize(4)]
        public byte[] Reserved;
    }

    /// <summary>
    ///  The SRV_COPYCHUNK_COPY packet is sent in an SMB2 IOCTL
    ///  Request by the client to initiate a server-side copy
    ///  of data. It is set as the contents of the input data
    ///  buffer. This packet consists of the following:
    /// </summary>
    public partial struct SRV_COPYCHUNK_COPY
    {
        /// <summary>
        ///  The key returned by the server that represents the source
        ///  file for the copy.
        /// </summary>
        [StaticSize(24)]
        public byte[] SourceKey;

        /// <summary>
        ///  The number of chunks of data that are to be copied.
        /// </summary>
        [StaticSize(4)]
        public uint ChunkCount;

        /// <summary>
        ///  This field MUST be set to 0 by the client, and ignored
        ///  by the server.
        /// </summary>
        [StaticSize(4)]
        public SRV_COPYCHUNK_COPY_Reserved_Values Reserved;

        /// <summary>
        ///  An array of packets describing the ranges to be copied.
        ///   This array MUST be of a length equal to ChunkCount
        ///  * size of SRV_COPYCHUNK.
        /// </summary>
        [Size("ChunkCount")]
        public SRV_COPYCHUNK[] Chunks;
    }

    /// <summary>
    /// SRV_COPYCHUNK_COPY_Reserved_Values
    /// </summary>
    public enum SRV_COPYCHUNK_COPY_Reserved_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    #region SRV_READ_HASH Request

    /// <summary>
    /// SRV_READ_HASH_Request_HashType_Values
    /// </summary>
    public enum SRV_READ_HASH_Request_HashType_Values : uint
    {
        /// <summary>
        /// Indicates the hash is requested for branch caching as described in [MS-PCCRC].
        /// </summary>
        SRV_HASH_TYPE_PEER_DIST = 0x00000001,
    }

    /// <summary>
    /// SRV_READ_HASH_Request_HashVersion_Values
    /// </summary>
    public enum SRV_READ_HASH_Request_HashVersion_Values : uint
    {
        /// <summary>
        /// Branch cache version 1.
        /// </summary>
        SRV_HASH_VER_1 = 0x00000001,

        /// <summary>
        /// Branch cache version 2. This value is only applicable for SMB dialect 2.2.
        /// </summary>
        SRV_HASH_VER_2 = 0x00000002,
    }

    /// <summary>
    /// SRV_READ_HASH_Request_HashRetrievalType_Values
    /// </summary>
    public enum SRV_READ_HASH_Request_HashRetrievalType_Values : uint
    {
        /// <summary>
        /// The FileOffset and Length fields in the SRV_READ_HASH request is relative to the Content 
        /// Information data structure specified in [MS-PCCRC] section 2.3.
        /// </summary>
        SRV_HASH_RETRIEVE_HASH_BASED = 0x00000001,

        /// <summary>
        /// The Offset field in the SRV_READ_HASH request is relative to the beginning of the file indicated by the FileId field in the IOCTL request.  
        /// This value is only applicable for SMB dialect 2.2.
        /// </summary>
        SRV_HASH_RETRIEVE_FILE_BASED = 0x00000002,
    }

    /// <summary>
    /// The REQUEST_HASH_DATA packet is sent to the server by the client in an SMB2 IOCTL Request 
    /// SRV_READ_HASH to retrieve the hash blob for a specified file. The request is valid only for 
    /// the SMB 2.1 dialect. It is set as the contents of the input data buffer. This packet consists 
    /// of the following:
    /// </summary>
    public partial struct SRV_READ_HASH_Request
    {
        /// <summary>
        /// The hash type of the request indicates what the hash is used for. This field MUST be constructed 
        /// using one of the following values:
        /// </summary>
        public SRV_READ_HASH_Request_HashType_Values HashType;

        /// <summary>
        /// The version number of the algorithm used to create the Content Information algorithm. 
        /// This field MUST be constructed using one of the following values:
        /// </summary>
        public SRV_READ_HASH_Request_HashVersion_Values HashVersion;

        /// <summary>
        /// Defines the FileOffset field relative to the Content Information. 
        /// This field MUST be constructed using one of the following values:
        /// </summary>
        public SRV_READ_HASH_Request_HashRetrievalType_Values HashRetrievalType;

        /// <summary>
        /// The maximum length, in bytes, of the Content Information data structure to be returned 
        /// in the SRV_READ_HASH response to the client.
        /// </summary>
        public uint Length;

        /// <summary>
        /// If the HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED, this value is the offset, 
        /// in bytes, from the beginning of the Content Information data structure.
        /// </summary>
        public ulong Offset;
    }

    /// <summary>
    /// All hash files storing a hash BLOB MUST start from a valid format HASH_HEADER as follows. 
    /// The format is valid only for the SMB 2.1 dialect.
    /// </summary>
    public partial struct HASH_HEADER
    {
        /// <summary>
        /// The hash type of the request indicates what the hash is used for. This field MUST be constructed 
        /// using one of the following values:
        /// </summary>
        public SRV_READ_HASH_Request_HashType_Values HashType;

        /// <summary>
        /// The version number of the algorithm used to create the Content Information algorithm. 
        /// This field MUST be constructed using one of the following values:
        /// </summary>
        public SRV_READ_HASH_Request_HashVersion_Values HashVersion;

        /// <summary>
        /// The last update time for the source file from which the hash BLOB is generated, 
        /// in FILETIME format as specified in [MS-DTYP] section 2.3.1.
        /// </summary>
        public _FILETIME SourceFileChangeTime;

        /// <summary>
        /// The length, in bytes, of the source file from which the hash BLOB is generated.
        /// </summary>
        public ulong SourceFileSize;

        /// <summary>
        /// The length, in bytes, of the hash BLOB.
        /// </summary>
        public uint HashBlobLength;

        /// <summary>
        ///  The offset of the hash BLOB, in bytes, from the beginning of the hash file.
        /// </summary>
        public uint HashBlobOffset;

        /// <summary>
        /// The flag that indicates whether the hash file is currently being updated. A nonzero value indicates True.
        /// </summary>
        public ushort Dirty;

        /// <summary>
        /// The length, in bytes, of the source file full name.
        /// </summary>
        public ushort SourceFileNameLength;

        /// <summary>
        /// A variable-length buffer that contains the source file full name, with length indicated by SourceFileNameLength.
        /// </summary>
        [Size("SourceFileNameLength")]
        public byte[] SourceFileName;
    }
    #endregion

    #region NETWORK_RESILIENCY_REQUEST Request

    /// <summary>
    /// NETWORK_RESILIENCY_Request_Reserved_Values
    /// </summary>
    public enum NETWORK_RESILIENCY_Request_Reserved_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// NETWORK_RESILIENCY_Request
    /// </summary>
    public partial struct NETWORK_RESILIENCY_Request
    {
        /// <summary>
        /// The requested time the server should hold the file open after a disconnect before 
        /// releasing it. This time is in milliseconds.
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The client MUST set this to 0, 
        /// and the server MUST ignore it on receipt.
        /// </summary>
        public uint Reserved;
    }
    #endregion

    /// <summary>
    ///  The SMB2 IOCTL Response packet is sent by the server
    ///  to transmit the results of a client SMB2 IOCTL request.
    ///   This response consists of an SMB2 header, as specified
    ///  in section , followed by this response structure:
    /// </summary>
    public partial struct IOCTL_Response
    {
        /// <summary>
        ///  The server MUST set this field to 49, indicating the
        ///  size of the response structure, not including the header.
        ///   This value MUST be used regardless of how large Buffer[]
        ///  is in the actual response.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The server MUST set this field to 0, and the client
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public IOCTL_Response_Reserved_Values Reserved;

        /// <summary>
        ///  The control code of the FSCTL/IOCTL method that was
        ///  executed.
        /// </summary>
        [StaticSize(4)]
        public uint CtlCode;

        /// <summary>
        ///  An SMB2_FILEID identifier of the file on which the command
        ///  was performed.  For certain IOCTL or FSCTL values as
        ///  specified in section , this parameter MUST be ignored.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the input data buffer.  If no input data
        ///  is required for the FSCTL/IOCTL command being issued,
        ///  then this value MUST be set to 0.
        /// </summary>
        [StaticSize(4)]
        public uint InputOffset;

        /// <summary>
        ///  The size, in bytes, of the input data.
        /// </summary>
        [StaticSize(4)]
        public uint InputCount;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the output data buffer.  If no output data
        ///  is required for the FSCTL/IOCTL command being issued,
        ///  then this value MUST be set to 0.
        /// </summary>
        [StaticSize(4)]
        public uint OutputOffset;

        /// <summary>
        ///  The size, in bytes, of the output data.
        /// </summary>
        [StaticSize(4)]
        public uint OutputCount;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The server MUST set this field to 0, and the client
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public IOCTL_Response_Flags_Values Flags;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The server MUST set this field to 0, and the client
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public IOCTL_Response_Reserved2_Values Reserved2;
    }

    /// <summary>
    /// IOCTL_Response_Reserved_Values
    /// </summary>
    public enum IOCTL_Response_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// IOCTL_Response_Flags_Values
    /// </summary>
    public enum IOCTL_Response_Flags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// IOCTL_Response_Reserved2_Values
    /// </summary>
    public enum IOCTL_Response_Reserved2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SRV_COPYCHUNK_RESPONSE packet is sent in an SMB2
    ///  IOCTL Response by the server to return the results
    ///  of a server-side copy operation. It is placed in the
    ///  Buffer field of the SMB2 IOCTL Response packet. This
    ///  packet consists of the following:
    /// </summary>
    public partial struct SRV_COPYCHUNK_RESPONSE
    {

        /// <summary>
        ///  If the operation did not fail with STATUS_INVALID_PARAMETER,
        ///  as specified in [MS-ERREF], this value MUST indicate
        ///  the number of chunks that were successfully written.
        ///   If the operation failed with STATUS_INVALID_PARAMETER,
        ///  this value MUST indicate the maximum number of chunks
        ///  that the server will accept in a single request. This
        ///  would allow the client to know how to correctly reissue
        ///  the request.
        /// </summary>
        [StaticSize(4)]
        public uint ChunksWritten;

        /// <summary>
        ///  If the operation did not fail with STATUS_INVALID_PARAMETER,
        ///  as specified in [MS-ERREF], this value MUST indicate
        ///  the number of bytes written in the last chunk that
        ///  did not successfully process (if a partial write occurred).
        ///   If the operation failed with STATUS_INVALID_PARAMETER,
        ///  this value MUST indicate the maximum number of bytes
        ///  the server will allow to be written in a single chunk.
        /// </summary>
        [StaticSize(4)]
        public uint ChunksBytesWritten;

        /// <summary>
        ///  If the operation did not fail with STATUS_INVALID_PARAMETER,
        ///  as specified in [MS-ERREF], this value MUST indicate
        ///  the total number of bytes written in the server-side
        ///  copy operation.  If the operation failed with STATUS_INVALID_PARAMETER,
        ///  this value MUST indicate the maximum number of bytes
        ///  the server will accept to copy in a single request.
        /// </summary>
        [StaticSize(4)]
        public uint TotalBytesWritten;
    }

    /// <summary>
    ///  The SRV_SNAPSHOT_ARRAY packet is returned to the client
    ///  by the server in an SMB2 IOCTL Response for the FSCTL_SRV_ENUMERATE_SNAPSHOTS
    ///  request, as specified in [MS-SMB] section 2.2.14.7.1.
    ///   This packet MUST contain all the revision time-stamps
    ///  that are associated with the particular open. This
    ///  SRV_SNAPSHOT_ARRAY is placed in the OutputBuffer field
    ///  in the SMB2 IOCTL Response. This packet consists of
    ///  the following:
    /// </summary>
    public partial struct SRV_SNAPSHOT_ARRAY
    {

        /// <summary>
        ///  The number of previous versions associated with the
        ///  volume that backs this file.
        /// </summary>
        [StaticSize(4)]
        public uint NumberOfSnapShots;

        /// <summary>
        ///  The number of previous version time stamps returned
        ///  in the SnapShots[] array.
        /// </summary>
        [StaticSize(4)]
        public uint NumberOfSnapShotsReturned;

        /// <summary>
        ///  This field specifies the length, in bytes, of the SnapShots[]
        ///  array.
        /// </summary>
        [StaticSize(4)]
        public uint SnapShotArraySize;

        /// <summary>
        ///  An array of time stamps in GMT format, as specified
        ///  by an @GMT token, which are separated by NULL characters
        ///  and terminated by two NULL characters.
        /// </summary>
        [Size("NumberOfSnapShotsReturned == 0 ? 0 : SnapShotArraySize")]
        public byte[] SnapShots;
    }

    #region SRV_REQUEST_RESUME_KEY Response

    /// <summary>
    /// SRV_REQUEST_RESUME_KEY_Response_ContextLength_Values
    /// </summary>
    public enum SRV_REQUEST_RESUME_KEY_Response_ContextLength_Values : uint
    {
        /// <summary>
        /// Possible value
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// The SRV_REQUEST_RESUME_KEY packet is returned to the client by the server in an SMB2 IOCTL 
    /// Response for the FSCTL_SRV_REQUEST_RESUME_KEY request. This SRV_REQUEST_RESUME_KEY is placed 
    /// in the Buffer field in the SMB2 IOCTL Response, and the OutputOffset and OutputCount fields 
    /// MUST be updated to describe the buffer as specified in section 2.2.32. This packet consists 
    /// of the following:
    /// </summary>
    public partial struct SRV_REQUEST_RESUME_KEY_Response
    {
        /// <summary>
        /// A 24-byte resume key generated by the server that can be subsequently used by the client to uniquely identify the 
        /// source file in an FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE request. The resume key 
        /// MUST be treated as a 24-byte opaque structure. The client that receives the 24-byte resume 
        /// key MUST not attach any interpretation to this key and MUST treat it as an opaque value.
        /// </summary>
        [StaticSize(24)]
        public byte[] ResumeKey;

        /// <summary>
        /// The length, in bytes, of the context information. This field is unused. The server SHOULD 
        /// set this field to 0 when sending a response. This field MUST be ignored by the client when 
        /// the message is received.
        /// </summary>
        [StaticSize(4)]
        public SRV_REQUEST_RESUME_KEY_Response_ContextLength_Values ContextLength;

        /// <summary>
        /// The context extended information. The length of Context MUST be greater than or equal to 4 
        /// bytes. If ContextLength is less than 4, the bytes in the Context array that are not described 
        /// by ContextLength SHOULD be initialized to 0.
        /// </summary>
        [Size("ContextLength")]
        public byte[] Context;
    }
    #endregion

    #region SRV_READ_HASH Response

    /// <summary>
    /// SRV_READ_HASH_Response_Reserved_Values
    /// </summary>
    public enum SRV_READ_HASH_Response_Reserved_Values : uint
    {
        /// <summary>
        /// Possible Value
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// The SRV_READ_HASH response is returned to the client by the server in an SMB2 IOCTL Response for 
    /// the FSCTL_SRV_READ_HASH request. This structure is placed in the Buffer field in the SMB2 IOCTL 
    /// Response, and the OutputOffset and OutputCount fields MUST be updated to describe the buffer 
    /// as specified in section 2.2.32. The SRV_READ_HASH response MUST be formatted as follows:
    /// </summary>
    public partial struct SRV_HASH_RETRIEVE_HASH_BASED
    {
        /// <summary>
        /// The offset, in bytes, from the beginning of the Content Information File to the portion retrieved. 
        /// This is equal to the Offset field in the SRV_READ_HASH request.
        /// </summary>
        public ulong Offset;

        /// <summary>
        /// The length, in bytes, of the retrieved portion of the Content Information data structure.
        /// </summary>
        public uint BufferLength;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The server MUST set this field to 0, 
        /// and the client MUST ignore it on receipt.
        /// </summary>
        public SRV_READ_HASH_Response_Reserved_Values Reserved;

        /// <summary>
        /// A variable-length buffer that contains the retrieved portion of the Content Information 
        /// data structure as specified in [MS-PCCRC] section 2.3.
        /// </summary>
        [Size("BufferLength")]
        public byte[] Buffer;
    }

    /// <summary>
    /// The SRV_READ_HASH response is returned to the client by the server in an SMB2 IOCTL Response for 
    /// the FSCTL_SRV_READ_HASH request. This structure is placed in the Buffer field in the SMB2 IOCTL 
    /// Response, and the OutputOffset and OutputCount fields MUST be updated to describe the buffer 
    /// as specified in section 2.2.32. The SRV_READ_HASH response MUST be formatted as follows:
    /// </summary>
    public partial struct SRV_HASH_RETRIEVE_FILE_BASED
    {
        /// <summary>
        /// File data offset corresponding to the start of the hash data returned.
        /// </summary>
        public ulong FileDataOffset;

        /// <summary>
        /// The length, in bytes, starting from the FileDataOffset that is covered by the hash data returned.
        /// </summary>
        public ulong FileDataLength;

        /// <summary>
        /// The length, in bytes, of the retrieved portion of the Content Information data structure.
        /// </summary>
        public uint BufferLength;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The server MUST set this field to 0, 
        /// and the client MUST ignore it on receipt.
        /// </summary>
        public SRV_READ_HASH_Response_Reserved_Values Reserved;

        /// <summary>
        /// A variable-length buffer that contains the retrieved portion of the Content Information 
        /// data structure as specified in [MS-PCCRC] section 2.3.
        /// </summary>
        [Size("BufferLength")]
        public byte[] Buffer;
    }
    #endregion

    #region VALIDATE_NEGOTIATE_INFO

    /// <summary>
    /// The VALIDATE_NEGOTIATE_INFO request packet is sent to the server by the client in an 
    /// SMB2 IOCTL Request FSCTL_VALIDATE_NEGOTIATE_INFO to request validation of a previous SMB2 NEGOTIATE.
    /// </summary>
    public partial struct VALIDATE_NEGOTIATE_INFO_Request
    {
        /// <summary>
        /// The Capabilities of the client
        /// </summary>
        public Capabilities_Values Capabilities;

        /// <summary>
        /// The ClientGuid of the client
        /// </summary>
        public Guid Guid;

        /// <summary>
        /// The SecurityMode of the client
        /// </summary>
        public SecurityMode_Values SecurityMode;

        /// <summary>
        /// The number of entries in the Dialects field
        /// </summary>
        public ushort DialectCount;

        /// <summary>
        /// The list of SMB2 dialects supported by the client. These entries SHOULD contain only the 2-byte Dialects values defined in section 2.2.3.
        /// </summary>
        [Size("DialectCount")]
        public DialectRevision[] Dialects;
    }

    /// <summary>
    /// The VALIDATE_NEGOTIATE_INFO response is returned to the client by the server in an SMB2 IOCTL response for FSCTL_VALIDATE_NEGOTIATE_INFO request. 
    /// </summary>
    public partial struct VALIDATE_NEGOTIATE_INFO_Response
    {
        /// <summary>
        /// The Capabilities of the server
        /// </summary>
        public Capabilities_Values Capabilities;

        /// <summary>
        /// The ServerGuid of the server
        /// </summary>
        public Guid Guid;

        /// <summary>
        /// The SecurityMode of the server
        /// </summary>
        public SecurityMode_Values SecurityMode;

        /// <summary>
        /// The SMB2 dialect in use by the server on the connection
        /// </summary>
        public DialectRevision Dialect;
    }


    #endregion

    #region NETWORK_INTERFACE_INFO Response
    /// <summary>
    /// The NETWORK_INTERFACE_INFO is returned to the client by the server in an SMB2 IOCTL response for FSCTL_QUERY_NETWORK_INTERFACE_INFO request.  
    /// </summary>
    public partial struct NETWORK_INTERFACE_INFO_Response
    {
        /// <summary>
        /// The offset, in bytes, from the beginning of this structure to the beginning of a subsequent 8-byte aligned network interface. 
        /// This field MUST be set to zero if there are no subsequent network interfaces.
        /// </summary>
        [StaticSize(4)]
        public uint Next;

        /// <summary>
        /// This field specifies the network interface index.
        /// </summary>
        [StaticSize(4)]
        public uint IfIndex;

        /// <summary>
        /// This field specifies the capabilities of the network interface. 
        /// </summary>
        [StaticSize(4)]
        public NETWORK_INTERFACE_INFO_Response_Capabilities Capability;

        /// <summary>
        /// The field specifies the RSS queue count.
        /// </summary>
        [StaticSize(4)]
        public uint RssQueueCount;

        /// <summary>
        /// The field specifies the speed of the network interface in bits per second.
        /// </summary>
        [StaticSize(8)]
        public ulong LinkSpeed;

        /// <summary>
        /// The field describes the network interface address as described in [MSDN-SOCKADDR_STORAGE].
        /// Refer to http://go.microsoft.com/fwlink/?LinkId=231819 
        /// </summary>
        [StaticSize(128)]
        public NETWORK_INTERFACE_INFO_Response_SockAddr_Storage AddressStorage;
    }

    /// <summary>
    /// The capabilities of NETWORK_INTERFACE_INFO_Response. 
    /// </summary>
    public enum NETWORK_INTERFACE_INFO_Response_Capabilities : uint
    {
        /// <summary>
        /// When set, specifies that the interface is RSS-capable.
        /// </summary>
        RSS_CAPABLE = 0x00000001,

        /// <summary>
        /// When set, specifies that the interface is RDMA-capable.
        /// </summary>
        RDMA_CAPABLE = 0x00000002
    }

    /// <summary>
    /// The SockAddr_Storage of NETWORK_INTERFACE_INFO_Response
    /// Refer to http://go.microsoft.com/fwlink/?LinkId=231819 
    /// </summary>
    public struct NETWORK_INTERFACE_INFO_Response_SockAddr_Storage
    {
        /// <summary>
        /// Address family of the socket, such as AF_INET. 
        /// On Windows Vista and later, the datatype for this member is defined as an ADDRESS_FAMILY.
        /// Family: 2 - IPv4, size 4; 23 - IPv6, size 16; 33 - MAC Address, size 6
        /// </summary>
        [StaticSize(2)]
        public ushort Family; //Address family.

        [StaticSize(2)]
        public ushort Port;

#pragma warning disable 0649
        [Size("(Family == 2) ? 4 : ((Family == 23) ? 16 : 6 )")]
        private byte[] address;
#pragma warning restore 0649

        public string Address
        {
            get
            {
                switch (Family)
                {
                    case 2:
                    case 23:
                        return (new System.Net.IPAddress(address)).ToString();
                    default:
                        throw new NotImplementedException("Support only IPv4 and IPv6 for now.");
                }
            }
        }
    }

    public enum NETWORK_INTERFACE_INFO_Response_SockAddr_StorageFamilyValue : ushort
    {
        IPv4 = 2,
        IPv6 = 23,
        MAC = 33,
    }
    #endregion

    /// <summary>
    ///  The SMB2 QUERY_DIRECTORY Request packet is sent by the
    ///  client to obtain a directory enumeration on an open
    ///  directory handle. This request consists of an SMB2
    ///  header, as specified in section , followed by this
    ///  request structure:
    /// </summary>
    public partial struct QUERY_DIRECTORY_Request
    {

        /// <summary>
        ///  The client MUST set this field to 33, indicating the
        ///  size of the request structure, not including the header.
        ///   The client MUST set this field to this value regardless
        ///  of how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The file information class describing the format that
        ///  data MUST be returned in.  Possible values are as specified
        ///  in [MS-FSCC] section 2.4. This field MUST contain
        ///  one of the following values:
        /// </summary>
        public FileInformationClass_Values FileInformationClass;

        /// <summary>
        ///  Flags indicating how the query directory operation MUST
        ///  be processed. This field MUST be constructed using
        ///  the following values:
        /// </summary>
        public QUERY_DIRECTORY_Request_Flags_Values Flags;

        /// <summary>
        ///  Index number received in a previous enumeration from
        ///  where to resume the enumeration. This MUST be used
        ///  if SMB2_INDEX_SPECIFIED is set in Flags.
        /// </summary>
        [StaticSize(4)]
        public uint FileIndex;

        /// <summary>
        ///  An SMB2_FILEID identifier of the directory on which
        ///  to perform the enumeration. This is returned from an
        ///  SMB2 Create Request to open a directory on the server.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the search pattern to be used for the enumeration.
        ///   This field MUST be 0 if no search pattern is provided.
        /// </summary>
        [StaticSize(2)]
        public ushort FileNameOffset;

        /// <summary>
        ///  The length, in bytes, of the search pattern. This field
        ///  MUST be 0 if no search pattern is provided.
        /// </summary>
        [StaticSize(2)]
        public ushort FileNameLength;

        /// <summary>
        ///  The maximum number of bytes the server is allowed to
        ///  return in the SMB2 QUERY_DIRECTORY Response.
        /// </summary>
        [StaticSize(4)]
        public uint OutputBufferLength;
    }

    /// <summary>
    /// FileInformationClass_Values
    /// </summary>
    public enum FileInformationClass_Values : byte
    {

        /// <summary>
        ///  Basic information about a file or directory.  Basic
        ///  information is defined as the file's name, time stamp,
        ///  and size, or its attributes.
        /// </summary>
        FileDirectoryInformation = 0x01,

        /// <summary>
        ///  Full information about a file or directory.  Full information
        ///  is defined as all the basic information plus extended
        ///  attribute size.
        /// </summary>
        FileFullDirectoryInformation = 0x02,

        /// <summary>
        ///  Full information plus volume file ID about a file or
        ///  directory. A volume file ID is defined as a unique
        ///  number assigned to a given volume.
        /// </summary>
        FileIdFullDirectoryInformation = 0x26,

        /// <summary>
        ///  Basic information plus extended attribute size and short
        ///  name about a file or directory.
        /// </summary>
        FileBothDirectoryInformation = 0x03,

        /// <summary>
        ///  FileBothDirectoryInformation plus volume ID about a
        ///  file or directory.
        /// </summary>
        FileIdBothDirectoryInformation = 0x25,
    }

    /// <summary>
    /// QUERY_DIRECTORY_Request_Flags_Values
    /// </summary>
    [Flags()]
    public enum QUERY_DIRECTORY_Request_Flags_Values : byte
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The server MUST restart the enumeration from the beginning,
        ///  but the search pattern is not changed.
        /// </summary>
        RESTART_SCANS = 0x01,

        /// <summary>
        ///  The server MUST only return the first entry of the search
        ///  results.
        /// </summary>
        RETURN_SINGLE_ENTRY = 0x02,

        /// <summary>
        ///  The server SHOULD return entries beginning at the byte
        ///  number specified by FileIndex.-based servers do not
        ///  support this value.
        /// </summary>
        INDEX_SPECIFIED = 0x04,

        /// <summary>
        ///  The server MUST restart the enumeration from the beginning,
        ///  and the search pattern MUST be changed to the provided
        ///  value.
        /// </summary>
        REOPEN = 0x10,
    }

    /// <summary>
    ///  The SMB2 QUERY_DIRECTORY Response packet is sent by
    ///  a server in response to an SMB2 QUERY_DIRECTORY Request.
    ///   This response consists of an SMB2 header, as specified
    ///  in section , followed by this response structure:
    /// </summary>
    public partial struct QUERY_DIRECTORY_Response
    {

        /// <summary>
        ///  The server MUST set this field to 9, indicating the
        ///  size of the request structure, not including the header.
        ///  The server MUST set this field to this value regardless
        ///  of how long Buffer[] actually is in the request.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the directory enumeration data being returned.
        /// </summary>
        [StaticSize(2)]
        public ushort OutputBufferOffset;

        /// <summary>
        ///  The length, in bytes, of the directory enumeration being
        ///  returned.
        /// </summary>
        [StaticSize(4)]
        public uint OutputBufferLength;
    }

    /// <summary>
    ///  The SMB2 CHANGE_NOTIFY Request packet is sent by the
    ///  client to register for change notifications on a directory.
    ///   This request consists of an SMB2 header, as specified
    ///  in section , followed by this request structure:
    /// </summary>
    public partial struct CHANGE_NOTIFY_Request
    {

        /// <summary>
        ///  The client MUST set this field to 32, indicating the
        ///  size of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Flags indicating how the operation MUST be processed.
        ///   This field MUST be constructed by using the following
        ///  values:
        /// </summary>
        [StaticSize(2)]
        public CHANGE_NOTIFY_Request_Flags_Values Flags;

        /// <summary>
        ///  The maximum number of bytes the server is allowed to
        ///  return in the SMB2 CHANGE_NOTIFY Response.
        /// </summary>
        [StaticSize(4)]
        public uint OutputBufferLength;

        /// <summary>
        ///  An SMB2_FILEID identifier of the directory to monitor
        ///  for changes.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;

        /// <summary>
        ///  Specifies the types of changes to monitor. It is valid
        ///  to choose multiple trigger conditions.  In this case,
        ///  if any condition is met, the client is notified of
        ///  the change and the CHANGE_NOTIFY operation is completed.
        ///   This field MUST be constructed using the following
        ///  values:
        /// </summary>
        [StaticSize(4)]
        public CompletionFilter_Values CompletionFilter;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public CHANGE_NOTIFY_Request_Reserved_Values Reserved;
    }

    /// <summary>
    /// CHANGE_NOTIFY_Request_Flags_Values
    /// </summary>
    [Flags()]
    public enum CHANGE_NOTIFY_Request_Flags_Values : ushort
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The request MUST monitor changes on any file or directory
        ///  contained beneath the directory specified by FileId.
        /// </summary>
        WATCH_TREE = 0x0001,
    }

    /// <summary>
    /// CompletionFilter_Values
    /// </summary>
    [Flags()]
    public enum CompletionFilter_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The client is notified if a file-name changes.
        /// </summary>
        FILE_NOTIFY_CHANGE_FILE_NAME = 0x00000001,

        /// <summary>
        ///  The client is notified if a directory name changes.
        /// </summary>
        FILE_NOTIFY_CHANGE_DIR_NAME = 0x00000002,

        /// <summary>
        ///  The client is notified if a file's attributes change.
        /// </summary>
        FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x00000004,

        /// <summary>
        ///  The client is notified if a file's size changes.
        /// </summary>
        FILE_NOTIFY_CHANGE_SIZE = 0x00000008,

        /// <summary>
        ///  The client is notified if the last write time of a file
        ///  changes.
        /// </summary>
        FILE_NOTIFY_CHANGE_LAST_WRITE = 0x00000010,

        /// <summary>
        ///  The client is notified if the last access time of a
        ///  file changes.
        /// </summary>
        FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x00000020,

        /// <summary>
        ///  The client is notified if the creation time of a file
        ///  changes.
        /// </summary>
        FILE_NOTIFY_CHANGE_CREATION = 0x00000040,

        /// <summary>
        ///  The client is notified if a file's extended attributes
        ///  (EAs) change.
        /// </summary>
        FILE_NOTIFY_CHANGE_EA = 0x00000080,

        /// <summary>
        ///  The client is notified of a file's access control list
        ///  (ACL) settings change.
        /// </summary>
        FILE_NOTIFY_CHANGE_SECURITY = 0x00000100,

        /// <summary>
        ///  The client is notified if a named stream is added to
        ///  a file.
        /// </summary>
        FILE_NOTIFY_CHANGE_STREAM_NAME = 0x00000200,

        /// <summary>
        ///  The client is notified if the size of a named stream
        ///  is changed.
        /// </summary>
        FILE_NOTIFY_CHANGE_STREAM_SIZE = 0x00000400,

        /// <summary>
        ///  The client is notified if a named stream is modified.
        /// </summary>
        FILE_NOTIFY_CHANGE_STREAM_WRITE = 0x00000800,
    }

    /// <summary>
    /// CHANGE_NOTIFY_Request_Reserved_Values
    /// </summary>
    public enum CHANGE_NOTIFY_Request_Reserved_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 CHANGE_NOTIFY Response packet is sent by the
    ///  server to transmit the results of a client's SMB2 CHANGE_NOTIFY
    ///  Request. The server MUST send this packet only if
    ///  a change occurs and MUST NOT send this packet otherwise.
    ///   This response consists of an SMB2 header, as specified
    ///  in section , followed by this response structure:
    /// </summary>
    public partial struct CHANGE_NOTIFY_Response
    {

        /// <summary>
        ///  The server MUST set this field to 9, indicating the
        ///  size of the request structure, not including the header.
        ///   The server MUST set the field to this value regardless
        ///  of how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the change information being returned.
        /// </summary>
        [StaticSize(2)]
        public ushort OutputBufferOffset;

        /// <summary>
        ///  The length, in bytes, of the change information being
        ///  returned.
        /// </summary>
        [StaticSize(4)]
        public uint OutputBufferLength;
    }

    /// <summary>
    ///  The FILE_NOTIFY_INFORMATION packet is sent in the SMB2
    ///  CHANGE_NOTIFY Response to return the changes that the
    ///  client is being notified of. The structure consists
    ///  of the following:
    /// </summary>
    public partial struct FILE_NOTIFY_INFORMATION
    {

        /// <summary>
        ///  The offset, in bytes, from the beginning of this structure
        ///  to the subsequent FILE_NOTIFY_INFORMATION structure.
        ///   If there are no subsequent structures, NextEntryOffset
        ///  MUST be 0.  NextEntryOffset MUST always be an integral
        ///  multiple of 4. The FileName array must be padded to
        ///  the next 4-byte boundary counted from the beginning
        ///  of the structure.
        /// </summary>
        [StaticSize(4)]
        public uint NextEntryOffset;

        /// <summary>
        ///  The changes that occurred on the file. This field MUST
        ///  contain one of the following values.
        /// </summary>
        [StaticSize(4)]
        public Action_Values Action;

        /// <summary>
        ///  The length, in bytes, of the file name in the FileName[]
        ///  field.
        /// </summary>
        [StaticSize(4)]
        public uint FileNameLength;

        /// <summary>
        ///  A Unicode string with the name of the file that changed.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// Action_Values
    /// </summary>
    public enum Action_Values : uint
    {

        /// <summary>
        ///  The file was added to the directory.
        /// </summary>
        FILE_ACTION_ADDED = 0x00000001,

        /// <summary>
        ///  The file was removed from the directory.
        /// </summary>
        FILE_ACTION_REMOVED = 0x00000002,

        /// <summary>
        ///  The file was modified. This may be a change to the
        ///  data or attributes of the file.
        /// </summary>
        FILE_ACTION_MODIFIED = 0x00000003,

        /// <summary>
        ///  The file was renamed, and this is the old name.
        /// </summary>
        FILE_ACTION_RENAMED_OLD_NAME = 0x00000004,

        /// <summary>
        ///  The file was renamed, and this is the new name.
        /// </summary>
        FILE_ACTION_RENAMED_NEW_NAME = 0x00000005,
    }

    /// <summary>
    ///  The SMB2 QUERY_INFO Request packet is sent by a client
    ///  to request information on a file, named pipe, or underlying
    ///  volume. This request consists of an SMB2 header, as
    ///  specified in section , followed by this request structure:
    /// </summary>
    public partial struct QUERY_INFO_Request
    {

        /// <summary>
        ///  The client MUST set this field to 41, indicating the
        ///  size of the request structure, not including the header.
        ///   The client MUST set this field to this value regardless
        ///  of how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The type of information queried. This field MUST contain
        ///  one of the following values:
        /// </summary>
        public InfoType_Values InfoType;

        /// <summary>
        ///  For file information queries, this field MUST contain
        ///  one of the following FILE_INFORMATION_CLASS values,
        ///  as specified in section  and in [MS-FSCC] section 2.4:FileBasicInformationFileStandardInformationFileInternalInformationFileEaInformationFileAccessInformationFilePositionInformationFileFullEaInformationFileModeInformationFileAlignmentInformationFileAllInformationFileAlternateNameInformationFileStreamInformationFilePipeInformationFilePipeLocalInformationFilePipeRemoteInformationFileCompressionInformationFileQuotaInformationFileNetworkOpenInformationFileAttributeTagInformationFor
        ///  file system information queries, this field MUST contain
        ///  one of the following FS_INFORMATION_CLASS values, as
        ///  specified in section  and in [MS-FSCC] section 2.5:FileFsVolumeInformationFileFsSizeInformationFileFsDeviceInformationFileFsAttributeInformationFileFsFullSizeInformationFileFsObjectIdInformationFor
        ///  quota and security, this field MUST be set to 0.
        /// </summary>
        public byte FileInfoClass;

        /// <summary>
        ///  The maximum number of bytes of information the server
        ///  can send in the response.
        /// </summary>
        [StaticSize(4)]
        public uint OutputBufferLength;

        /// <summary>
        ///  The offset to the input buffer from the beginning of
        ///  the request.  For quota requests, the input buffer
        ///  MUST contain an SMB2_QUERY_QUOTA_INFO, as specified
        ///  in section.  For other information queries, this field
        ///  MUST be set to 0.
        /// </summary>
        [StaticSize(2)]
        public ushort InputBufferOffset;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public QUERY_INFO_Request_Reserved_Values Reserved;

        /// <summary>
        ///  The length of the input buffer.  For quota requests,
        ///  this must be the length of the contained SMB2_QUERY_QUOTA_INFO
        ///  embedded in the request.  For other information queries,
        ///  this field MUST be set to 0.
        /// </summary>
        [StaticSize(4)]
        public uint InputBufferLength;

        /// <summary>
        ///  Provides additional information to the server.If security
        ///  information is being queried, this value contains a
        ///  4-byte bit field of flags indicating what security
        ///  attributes MUST be returned.  For more information
        ///  about security descriptors, see SECURITY_DESCRIPTOR
        ///  in [MS-DTYP].
        /// </summary>
        [StaticSize(4)]
        public AdditionalInformation_Values AdditionalInformation;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public QUERY_INFO_Request_Flags_Values Flags;

        /// <summary>
        ///  An  SMB2_FILEID identifier of the file or named pipe
        ///  on which to perform the query.  Queries for file system
        ///  or quota information are directed to the volume on
        ///  which the file resides.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;
    }

    /// <summary>
    /// InfoType_Values
    /// </summary>
    public enum InfoType_Values : byte
    {

        /// <summary>
        ///  The file information is requested.
        /// </summary>
        SMB2_0_INFO_FILE = 0x01,

        /// <summary>
        ///  The file system information is requested.
        /// </summary>
        SMB2_0_INFO_FILESYSTEM = 0x02,

        /// <summary>
        ///  The security information is requested.
        /// </summary>
        SMB2_0_INFO_SECURITY = 0x03,

        /// <summary>
        ///  The file system quota information is requested.
        /// </summary>
        SMB2_0_INFO_QUOTA = 0x04,
    }

    /// <summary>
    /// QUERY_INFO_Request_Reserved_Values
    /// </summary>
    public enum QUERY_INFO_Request_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// AdditionalInformation_Values
    /// </summary>
    [Flags()]
    public enum AdditionalInformation_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The client is querying the owner from the security of
        ///  the file or named pipe.
        /// </summary>
        OWNER_SECURITY_INFORMATION = 0x00000001,

        /// <summary>
        ///  The client is querying the group from the security of
        ///  the file or named pipe.
        /// </summary>
        GROUP_SECURITY_INFORMATION = 0x00000002,

        /// <summary>
        ///  The client is querying the discretionary access control list 
        ///  from the security descriptor of the file or named pipe.
        /// </summary>
        DACL_SECURITY_INFORMATION = 0x00000004,

        /// <summary>
        ///  The client is querying the system access control list
        ///  from the security of the file or named pipe.
        /// </summary>
        SACL_SECURITY_INFORMATION = 0x00000008,

        /// <summary>
        ///  The client is querying the integrity label from the
        ///  security of the file or named pipe.
        /// </summary>
        LABEL_SECURITY_INFORMATION = 0x00000010,

        /// <summary>
        ///  The client is querying the resource attribute 
        ///  from the security descriptor of the file or named pipe.
        /// </summary>
        ATTRIBUTE_SECURITY_INFORMATION = 0x00000020,

        /// <summary>
        ///  The client is querying the central access policy of the resource 
        ///  from the security descriptor of the file or named pipe. 
        /// </summary>
        SCOPE_SECURITY_INFORMATION = 0x00000040,

        /// <summary>
        ///  The client is querying the security descriptor information used for backup operation. 
        /// </summary>
        BACKUP_SECURITY_INFORMATION = 0x00010000,
    }

    /// <summary>
    /// QUERY_INFO_Request_Flags_Values
    /// </summary>
    [Flags()]
    public enum QUERY_INFO_Request_Flags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,

        /// <summary>
        /// Restart the scan for EAs from the beginning.
        /// </summary>
        SL_RESTART_SCAN = 1,

        /// <summary>
        /// Return a single EA entry in the response buffer.
        /// </summary>
        SL_RETURN_SINGLE_ENTRY = 2,

        /// <summary>
        /// The caller has specified an EA index.
        /// </summary>
        SL_INDEX_SPECIFIED = 4,
    }

    /// <summary>
    ///  The SMB2 QUERY_QUOTA_INFO packet that specifies the
    ///  quota information to return.
    /// </summary>
    public partial struct QUERY_QUOTA_INFO
    {

        /// <summary>
        ///  A Boolean value.  If ReturnSingle is any nonzero value,
        ///  the server MUST return a single value.  If zero, the
        ///  server MUST return as many entries as will fit in the
        ///  maximum output size that is indicated in the request.
        /// </summary>
        [StaticSize(1)]
        public byte ReturnSingle;

        /// <summary>
        ///  A Boolean value.  If set, the quota information MUST
        ///  be read from the beginning.  If not set, the quota
        ///  information MUST be continued from the previous enumeration
        ///  that was executed on this open.
        /// </summary>
        [StaticSize(1)]
        public byte RestartScan;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public QUERY_QUOTA_INFO_Reserved_Values Reserved;

        /// <summary>
        ///  The length, in bytes, of the SidList, which is a linked
        ///  list of FILE_GET_QUOTA_INFORMATION structures, as specified
        ///  in [MS-FSCC] section 2.4.31.1, that is pointed to by
        ///  StartSidOffset. It MUST be set to zero if the buffer
        ///  contains a single SID and does not contain a linked
        ///  list of FILE_GET_QUOTA_INFORMATION structures.
        /// </summary>
        [StaticSize(4)]
        public uint SidListLength;

        /// <summary>
        ///  The length, in bytes, of a single SID. It MUST be set
        ///  to zero if the Buffer field contains a linked list
        ///  of FILE_GET_QUOTA_INFORMATION structures, as specified
        ///  in [MS-FSCC] section 2.4.31.1.-based clients will always
        ///  request quota information by using a SidList, which
        ///  is a linked list of one or more FILE_GET_QUOTA_INFORMATION
        ///  structures.
        /// </summary>
        [StaticSize(4)]
        public uint StartSidLength;

        /// <summary>
        ///  The offset, in bytes, to a SID, from the beginning of
        ///  the Buffer field. It MUST be zero if the buffer contains
        ///  a linked list of FILE_GET_QUOTA_INFORMATION structures,
        ///  as specified in [MS-FSCC] section 2.4.31.1.
        /// </summary>
        [StaticSize(4)]
        public uint StartSidOffset;

        /// <summary>
        ///  The format of the buffer is either a single SID, as
        ///  specified in [MS-DTYP] section 2.4.2 or a linked list
        ///  of one or more FILE_GET_QUOTA_INFORMATION structures,
        ///  as specified in [MS-FSCC] section 2.4.31.1.If the buffer
        ///  contains a single SID, the SidListLength MUST be zero,
        ///  the StartSidLength MUST be the length of the SID, and
        ///  the StartSidOffset MUST be the offset to the SID.If
        ///  the buffer contains a single FILE_GET_QUOTA_INFORMATION
        ///  structure, the SidListLength MUST be the length, in
        ///  bytes, of the single FILE_GET_QUOTA_INFORMATION structure;
        ///  the StartSidLength MUST be the length, in bytes, of
        ///  a single SID that is contained in the FILE_GET_QUOTA_INFORMATION
        ///  structure; and the StartSidOffset MUST be the offset
        ///  to the single SID in the FILE_GET_QUOTA_INFORMATION
        ///  structure.  If the buffer contains a linked list of
        ///  FILE_GET_QUOTA_INFORMATION structures, the SidListLength
        ///  MUST be the length, in bytes, of the list of FILE_GET_QUOTA_INFORMATION
        ///  structures; the StartSidLength MUST be set to zero;
        ///  and the StartSidOffset MUST be set to zero.
        /// </summary>
        [Size("SidListLength")]
        public byte[] Buffer;
    }

    /// <summary>
    /// QUERY_QUOTA_INFO_Reserved_Values
    /// </summary>
    public enum QUERY_QUOTA_INFO_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 QUERY_INFO Response packet is sent by the server
    ///  in response to an SMB2 QUERY_INFO Request packet. 
    ///  This response consists of an SMB2 header, as specified
    ///  in section , followed by this response structure:
    /// </summary>
    public partial struct QUERY_INFO_Response
    {

        /// <summary>
        ///  The server MUST set this field to 9, indicating the
        ///  size of the request structure, not including the header.
        ///   The server MUST set this field to this value regardless
        ///  of how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the information being returned.
        /// </summary>
        [StaticSize(2)]
        public ushort OutputBufferOffset;

        /// <summary>
        ///  The length, in bytes, of the information being returned.
        /// </summary>
        [StaticSize(4)]
        public uint OutputBufferLength;
    }

    /// <summary>
    ///  The SMB2 SET_INFO Request packet is sent by a client
    ///  to set information on a file or underlying file system.
    ///   This request consists of an SMB2 header, as specified
    ///  in section , followed by this request structure:
    /// </summary>
    public partial struct SET_INFO_Request
    {

        /// <summary>
        ///  The client MUST set this field to 33, indicating the
        ///  size of the request structure, not including the header.
        ///   The client MUST set this field to this value regardless
        ///  of how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The type of information being set. The valid values
        ///  are:
        /// </summary>
        public SET_INFO_Request_InfoType_Values InfoType;

        /// <summary>
        ///  This field MUST contain one of the following FILE_INFORMATION_CLASS
        ///  values, as specified in section  and [MS-FSCC] section
        ///  2.4:FileBasicInformationFileRenameInformationFileLinkInformationFileDispositionInformationFilePositionInformationFileFullEaInformationFileModeInformationFileAllocationInformationFileEndOfFileInformationFilePipeInformationFileValidDataLengthInformationFileShortNameInformationFor
        ///  setting file system information, this field MUST contain
        ///  one of the following FS_INFORMATION_CLASS values, as
        ///  specified in [MS-FSCC] section 2.5:  FileFsControlInformation
        ///    FileFsObjectIdInformation For quota and security,
        ///  this field MUST be set to a value of 0.
        /// </summary>
        public byte FileInfoClass;

        /// <summary>
        ///  The length, in bytes, of the information to be set.
        /// </summary>
        [StaticSize(4)]
        public uint BufferLength;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB2
        ///  header to the information to be set.
        /// </summary>
        [StaticSize(2)]
        public ushort BufferOffset;

        /// <summary>
        ///  Unused at present and MUST be treated as reserved. 
        ///  The client MUST set this field to 0, and the server
        ///  MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public SET_INFO_Request_Reserved_Values Reserved;

        /// <summary>
        ///  Provides additional information to the server.If security
        ///  information is being set, this value MUST contain a
        ///  4-byte bit field of flags indicating what security
        ///  attributes MUST be applied.  For more information about
        ///  security descriptors, see [MS-DTYP] section 2.4.6.
        /// </summary>
        [StaticSize(4)]
        public SET_INFO_Request_AdditionalInformation_Values AdditionalInformation;

        /// <summary>
        ///  An SMB2_FILEID identifier of the file or named pipe
        ///  on which to perform the set.  Set operations for quota
        ///  information are directed to the volume on which the
        ///  file resides.
        /// </summary>
        [StaticSize(16)]
        public FILEID FileId;
    }

    /// <summary>
    /// SET_INFO_Request_InfoType_Values
    /// </summary>
    public enum SET_INFO_Request_InfoType_Values : byte
    {

        /// <summary>
        ///  The file information is being set.
        /// </summary>
        SMB2_0_INFO_FILE = 0x01,

        /// <summary>
        ///  The file system information is being set.
        /// </summary>
        SMB2_0_INFO_FILESYSTEM = 0x02,

        /// <summary>
        ///  The security information is being set.
        /// </summary>
        SMB2_0_INFO_SECURITY = 0x03,

        /// <summary>
        ///  The file system quota information is being set.
        /// </summary>
        SMB2_0_INFO_QUOTA = 0x04,
    }

    /// <summary>
    /// SET_INFO_Request_Reserved_Values
    /// </summary>
    public enum SET_INFO_Request_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// SET_INFO_Request_AdditionalInformation_Values
    /// </summary>
    [Flags()]
    public enum SET_INFO_Request_AdditionalInformation_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The client is setting the owner from the security of
        ///  the file or named pipe.
        /// </summary>
        OWNER_SECURITY_INFORMATION = 0x00000001,

        /// <summary>
        ///  The client is setting the group from the security of
        ///  the file or named pipe.
        /// </summary>
        GROUP_SECURITY_INFORMATION = 0x00000002,

        /// <summary>
        ///  The client is setting the discretionary access control
        ///  list from the security of the file or named pipe.
        /// </summary>
        DACL_SECURITY_INFORMATION = 0x00000004,

        /// <summary>
        ///  The client is setting the system access control list
        ///  from the security of the file or named pipe.
        /// </summary>
        SACL_SECURITY_INFORMATION = 0x00000008,

        /// <summary>
        ///  The client is setting the integrity label from the security
        ///  of the file or named pipe.
        /// </summary>
        LABEL_SECURITY_INFORMATION = 0x00000010,

        /// <summary>
        /// The client is setting the resource attribute in the security 
        /// descriptor of the file or named pipe.
        /// </summary>
        ATTRIBUTE_SECURITY_INFORMATION = 0x00000020,

        /// <summary>
        /// The client is setting the central access policy of the resource
        /// in the security descriptor of the file or named pipe.
        /// </summary>
        SCOPE_SECURITY_INFORMATION = 0x00000040,

        /// <summary>
        /// The client is setting the backup operation information in the security
        /// descriptor of the file or named pipe.
        /// </summary>
        BACKUP_SECURITY_INFORMATION = 0x00010000,

    }

    /// <summary>
    ///  The SMB2 NEGOTIATE Response packet is sent by the server
    ///  to notify the client of the preferred common dialect.
    ///   This response is composed of an SMB2 header, as specified
    ///  in section , followed by this response structure:
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public partial struct NEGOTIATE_Response
    {

        /// <summary>
        ///  The server MUST set this field to 65, indicating the
        ///  size of the response structure, not including the header.
        ///   The server MUST set it to this value, regardless of
        ///  how long Buffer[] actually is in the response being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        [FieldOffset(0)]
        public ushort StructureSize;

        /// <summary>
        ///  The security mode field MUST be constructed by using
        ///  the following values:
        /// </summary>
        [StaticSize(2)]
        [FieldOffset(2)]
        public NEGOTIATE_Response_SecurityMode_Values SecurityMode;

        /// <summary>
        ///  The preferred common SMB 2.0 Protocol dialect number
        ///  from the Dialect array that is sent in the SMB2 negotiate
        ///  request.
        /// </summary>
        [StaticSize(2)]
        [FieldOffset(4)]
        public DialectRevision DialectRevision;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 6.
        /// </summary>
        [StaticSize(2)]
        [FieldOffset(6)]
        public NEGOTIATE_Response_Reserved_Values Reserved;

        /// <summary>
        ///  A globally unique identifier that is generated by the
        ///  server to uniquely identity this server. This field
        ///  MUST NOT be used by a client as a secure method of
        ///  identitying a server.
        /// </summary>
        [StaticSize(16)]
        [FieldOffset(8)]
        public System.Guid ServerGuid;

        /// <summary>
        ///  Specifies protocol capabilities for the server. This
        ///  field MUST be constructed by using the following values:
        /// </summary>
        [StaticSize(4)]
        [FieldOffset(24)]
        public NEGOTIATE_Response_Capabilities_Values Capabilities;

        /// <summary>
        ///  The maximum buffer size, in bytes, that can be used
        ///  for operations that are not read or write operations.
        /// </summary>
        [StaticSize(4)]
        [FieldOffset(28)]
        public uint MaxTransactSize;

        /// <summary>
        ///  The maximum size, in bytes, of the Length in an SMB2
        ///  READ Request that the server will accept.
        /// </summary>
        [StaticSize(4)]
        [FieldOffset(32)]
        public uint MaxReadSize;

        /// <summary>
        ///  The maximum size, in bytes, of the Length in an SMB2
        ///  WRITE Request that the server will accept.
        /// </summary>
        [StaticSize(4)]
        [FieldOffset(36)]
        public uint MaxWriteSize;

        /// <summary>
        ///  The FILETIME format, as specified in [MS-DTYP] section
        ///  2.3.5.The current system time of the server.
        /// </summary>
        [StaticSize(8)]
        [FieldOffset(40)]
        public _FILETIME SystemTime;

        /// <summary>
        ///  The FILETIME format, as specified in [MS-DTYP] section
        ///  2.3.5.The time the server was started up.
        /// </summary>
        [StaticSize(8)]
        [FieldOffset(48)]
        public _FILETIME ServerStartTime;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB
        ///  2.0 Protocol header to the security buffer.
        /// </summary>
        [StaticSize(2)]
        [FieldOffset(56)]
        public ushort SecurityBufferOffset;

        /// <summary>
        ///  The length, in bytes, of the security buffer.
        /// </summary>
        [StaticSize(2)]
        [FieldOffset(58)]
        public ushort SecurityBufferLength;

        /// <summary>
        ///  If the DialectRevision field is not 0x0311, 
        ///  the server MUST set this to 0 and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        [FieldOffset(60)]
        public uint Reserved2;

        /// <summary>
        ///  If the DialectRevision field is 0x0311, then this field specifies the offset, 
        ///  in bytes, from the beginning of the SMB2 header to the first 8-byte aligned 
        ///  negotiate context in NegotiateContextList
        /// </summary>
        [StaticSize(4)]
        [FieldOffset(60)]
        public uint NegotiateContextOffset;
    }

    /// <summary>
    /// NEGOTIATE_Response_SecurityMode_Values
    /// </summary>
    [Flags()]
    public enum NEGOTIATE_Response_SecurityMode_Values : ushort
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates that security signatures are enabled
        ///  on the client.
        /// </summary>
        NEGOTIATE_SIGNING_ENABLED = 0x0001,

        /// <summary>
        ///  When set, indicates that security signatures are required
        ///  by the client.
        /// </summary>
        NEGOTIATE_SIGNING_REQUIRED = 0x0002,
    }

    /// <summary>
    /// DialectRevision_Values
    /// </summary>
    public enum DialectRevision : ushort
    {

        /// <summary>
        /// SMB 2.002 dialect revision number
        /// </summary>
        Smb2002 = 0x0202,

        /// <summary>
        /// SMB 2.1 dialect revision number
        /// </summary>
        Smb21 = 0x0210,

        /// <summary>
        /// SMB 3.0 dialect revision number
        /// </summary>
        Smb30 = 0x0300,

        /// <summary>
        /// SMB 3.02 dialect revision number
        /// </summary>
        Smb302 = 0x0302,

        /// <summary>
        /// SMB 3.1.1 dialect revision number
        /// </summary>
        Smb311 = 0x0311,

        /// <summary>
        /// SMB2 wildcard revision number
        /// </summary>
        Smb2Wildcard = 0x02ff,

        /// <summary>
        /// Unknown dialect 0xFFFF
        /// </summary>
        Smb2Unknown = 0xFFFF,
    }

    /// <summary>
    /// NEGOTIATE_Response_Reserved_Values
    /// </summary>
    public enum NEGOTIATE_Response_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 6,

        /// <summary>
        ///  Value generated from PAC config.
        /// </summary>
        WrongValueReturnedByServer = 0,
    }

    /// <summary>
    /// NEGOTIATE_Response_Capabilities_Values
    /// </summary>
    [Flags()]
    public enum NEGOTIATE_Response_Capabilities_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        GLOBAL_CAP_DFS = 0x00000001,

        GLOBAL_CAP_LEASING = 0x00000002,

        GLOBAL_CAP_LARGE_MTU = 0x00000004,

        GLOBAL_CAP_MULTI_CHANNEL = 0x00000008,

        GLOBAL_CAP_PERSISTENT_HANDLES = 0x00000010,

        GLOBAL_CAP_DIRECTORY_LEASING = 0x00000020,

        GLOBAL_CAP_ENCRYPTION = 0x00000040,
    }

    /// <summary>
    ///  The SMB2 SET_INFO Response packet is sent by the server
    ///  in response to an SMB2 SET_INFO Request to notify the
    ///  client that its request has been successfully processed.
    ///   This response consists of an SMB2 header, as specified
    ///  in section , followed by this response structure:
    /// </summary>
    public partial struct SET_INFO_Response
    {

        /// <summary>
        ///  The server MUST set this field to 2, indicating the
        ///  size of the request structure, not including the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;
    }

    /// <summary>
    ///  The SMB2 SESSION_SETUP Request packet is sent by the
    ///  client to request a new authenticated session within
    ///  a new or existing SMB 2.0 Protocol transport connection
    ///  to the server. This request is composed of an SMB
    ///  2.0 Protocol header as specified in section  followed
    ///  by this request structure:
    /// </summary>
    public partial struct SESSION_SETUP_Request
    {
        /// <summary>
        ///  The client MUST set this field to 25, indicating the
        ///  size of the request structure, not including the header.
        ///   The client MUST set it to this value regardless of
        ///  how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  The number of other transport connections that are already
        ///  established. The client MAY choose to set this field
        ///  to 0 regardless of the number of outstanding connections.
        ///  clients always set VcNumber to 0.
        /// </summary>
        public SESSION_SETUP_Request_Flags Flags;

        /// <summary>
        ///  The security mode field MUST be constructed by using
        ///  the following values:
        /// </summary>
        public SESSION_SETUP_Request_SecurityMode_Values SecurityMode;

        /// <summary>
        ///  Specifies protocol capabilities for the client. This
        ///  field MUST be constructed by using the following values:
        /// </summary>
        [StaticSize(4)]
        public SESSION_SETUP_Request_Capabilities_Values Capabilities;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public SESSION_SETUP_Request_Channel_Values Channel;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB
        ///  2.0 Protocol header to the security buffer.
        /// </summary>
        [StaticSize(2)]
        public ushort SecurityBufferOffset;

        /// <summary>
        ///  The length, in bytes, of the security buffer.
        /// </summary>
        [StaticSize(2)]
        public ushort SecurityBufferLength;

        /// <summary>
        ///  A previously established session identifier.  If this
        ///  is a reconnect, the client MUST set this value to its
        ///  previous session identifier to allow the server to
        ///  reconnect.  If this is not a reconnect, the client
        ///  MUST set this to 0.
        /// </summary>
        [StaticSize(8)]
        public ulong PreviousSessionId;
    }

    [Flags]
    public enum SESSION_SETUP_Request_Flags : byte
    {
        NONE = 0,

        SESSION_FLAG_BINDING = 0x01
    }

    /// <summary>
    /// SESSION_SETUP_Request_SecurityMode_Values
    /// </summary>
    [Flags()]
    public enum SESSION_SETUP_Request_SecurityMode_Values : byte
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates that security signatures are enabled
        ///  on the client.
        /// </summary>
        NEGOTIATE_SIGNING_ENABLED = 0x01,

        /// <summary>
        ///  When set, indicates that security signatures are required
        ///  by the client.
        /// </summary>
        NEGOTIATE_SIGNING_REQUIRED = 0x02,
    }

    /// <summary>
    /// SESSION_SETUP_Request_Capabilities_Values
    /// </summary>
    [Flags()]
    public enum SESSION_SETUP_Request_Capabilities_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When set, indicates that the client supports the Distributed
        ///  File System (DFS).
        /// </summary>
        GLOBAL_CAP_DFS = 0x00000001,

        /// <summary>
        ///  MAY be set to any value and server MUST ignore.
        /// </summary>
        GLOBAL_CAP_UNUSED1 = 0x00000002,

        /// <summary>
        ///  MAY be set to any value and server MUST ignore.
        /// </summary>
        GLOBAL_CAP_UNUSED2 = 0x00000004,

        /// <summary>
        ///  MAY be set to any value and server MUST ignore.
        /// </summary>
        GLOBAL_CAP_UNUSED3 = 0x00000008,
    }

    /// <summary>
    /// SESSION_SETUP_Request_Channel_Values
    /// </summary>
    public enum SESSION_SETUP_Request_Channel_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 SESSION_SETUP Response packet is sent by the
    ///  server in response to an SMB2 SESSION_SETUP Request
    ///  packet. This response is composed of an SMB 2.0 Protocol
    ///  header, as specified in section , that is followed
    ///  by this response structure:
    /// </summary>
    public partial struct SESSION_SETUP_Response
    {

        /// <summary>
        ///  The server MUST set this to 9, indicating the size of
        ///  the fixed part of the response structure not including
        ///  the header. The server MUST set it to this value regardless
        ///  of how long Buffer[] actually is in the response.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  A flags field that indicates additional information
        ///  about the session. This field MUST be constructed
        ///  by using the following values:
        /// </summary>
        [StaticSize(2)]
        public SessionFlags_Values SessionFlags;

        /// <summary>
        ///  The offset, in bytes, from the beginning of the SMB
        ///  2.0 Protocol header to the security buffer.
        /// </summary>
        [StaticSize(2)]
        public ushort SecurityBufferOffset;

        /// <summary>
        ///  The length, in bytes, of the security buffer.
        /// </summary>
        [StaticSize(2)]
        public ushort SecurityBufferLength;
    }

    /// <summary>
    /// SessionFlags_Values
    /// </summary>
    [Flags()]
    public enum SessionFlags_Values : ushort
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  If set, the client has been authenticated as a guest
        ///  user.
        /// </summary>
        SESSION_FLAG_IS_GUEST = 0x0001,

        /// <summary>
        ///  If set, the client has been authenticated as a NULL
        ///  user.
        /// </summary>
        SESSION_FLAG_IS_NULL = 0x0002,

        /// <summary>
        ///  If set, the server requires encryption of messages on this session,
        ///  this flag is only valid for SMB3.0 dialect
        ///  user.
        /// </summary>
        SESSION_FLAG_ENCRYPT_DATA = 0x0004,
    }

    /// <summary>
    ///  The SMB2 LOGOFF Request packet.
    /// </summary>
    public partial struct LOGOFF_Request
    {

        /// <summary>
        ///  The client MUST set this field to 4, indicating the
        ///  size of the subsequent request structure not including
        ///  the header.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The client MUST set this to 0, and the server MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public LOGOFF_Request_Reserved_Values Reserved;
    }

    /// <summary>
    /// LOGOFF_Request_Reserved_Values
    /// </summary>
    public enum LOGOFF_Request_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 LOGOFF Response packet is sent by the server
    ///  to confirm that an SMB2 LOGOFF Request was completed
    ///  successfully. This response is composed of an SMB2
    ///  header, as specified in section , followed by this
    ///  request structure:
    /// </summary>
    public partial struct LOGOFF_Response
    {

        /// <summary>
        ///  The server MUST set this field to 4, indicating the
        ///  size of the response structure, not including the header
        ///  or variable-length fields.
        /// </summary>
        [StaticSize(2)]
        public ushort StructureSize;

        /// <summary>
        ///  Unused at the present and MUST be treated as reserved.
        ///   The server MUST set this to 0, and the client MUST
        ///  ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public LOGOFF_Response_Reserved_Values Reserved;
    }

    /// <summary>
    /// LOGOFF_Response_Reserved_Values
    /// </summary>
    public enum LOGOFF_Response_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The SMB2 TREE_CONNECT Request packet is sent by a client
    ///  to request access to a particular share on the server.
    ///   This request is composed of an SMB2 Packet Header
    ///  that is followed by this request structure:
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public partial struct TREE_CONNECT_Request
    {
        /// <summary>
        ///  The client MUST set this field to 9, indicating the
        ///  size of the request structure, not including the header.
        ///   The client MUST set it to this value regardless of
        ///  how long Buffer[] actually is in the request being
        ///  sent.
        /// </summary>
        [FieldOffset(0)]
        public ushort StructureSize;

        /// <summary>
        /// This field is interpreted in different ways depending on the SMB2 dialect.
        /// In the SMB 3.1.1 dialect, this field is interpreted as the Flags field
        /// If the dialect is not 3.1.1, then this field MUST NOT be used and MUST be reserved.
        /// The client MUST set this to 0, and the server MUST ignore it on receipt.
        /// </summary>
        [FieldOffset(2)]
        public TREE_CONNECT_Request_Reserved_Values Reserved;

        /// <summary>
        /// This field is interpreted in different ways depending on the SMB2 dialect.
        /// In the SMB 3.1.1 dialect, this field is interpreted as the Flags field
        /// If the dialect is not 3.1.1, then this field MUST NOT be used and MUST be reserved.
        /// The client MUST set this to 0, and the server MUST ignore it on receipt.
        /// </summary>
        [FieldOffset(2)]
        public TreeConnect_Flags Flags;

        /// <summary>
        ///  The offset, in bytes, of the full share path name from
        ///  the beginning of the packet header.
        /// </summary>
        [FieldOffset(4)]
        public ushort PathOffset;

        /// <summary>
        ///  The length, in bytes, of the path name.
        /// </summary>
        [FieldOffset(6)]
        public ushort PathLength;
    }

    /// <summary>
    /// TREE_CONNECT_Request_Reserved_Values
    /// </summary>
    public enum TREE_CONNECT_Request_Reserved_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// If the Flags field of the SMB2 TREE_CONNECT request has the
    /// SMB2_TREE_CONNECT_FLAG_EXTENSION_PRESENT bit set,
    /// the following structure MUST be added at the beginning of the Buffer field.
    /// </summary>
    public struct TREE_CONNECT_Request_Extension
    {
        /// <summary>
        /// The offset from the start of the SMB2 TREE_CONNECT request of an array of tree connect contexts.
        /// </summary>
        [StaticSize(4)]
        public uint TreeConnectContextOffset;

        /// <summary>
        /// The count of elements in the tree connect context array.
        /// </summary>
        [StaticSize(2)]
        public ushort TreeConnectContextCount;

        /// <summary>
        /// MUST be set to zero.
        /// </summary>
        [StaticSize(10)]
        public byte[] Reserved;

        /// <summary>
        /// This field is a variable-length buffer that contains the full share path name
        /// as specified in section 2.2.9.
        /// </summary>
        public byte[] PathName;

        /// <summary>
        /// A variable length array of SMB2_TREE_CONNECT_CONTEXT structures
        /// as described in section 2.2.9.2.
        /// </summary>
        [Size("TreeConnectContextCount")]
        public Tree_Connect_Context[] TreeConnectContexts;
    }

    /// <summary>
    /// The SMB2_TREE_CONNECT_CONTEXT structure is used by the SMB2 TREE_CONNECT request and
    /// the SMB2 TREE_CONNECT response to encode additional properties.
    /// </summary>
    public struct Tree_Connect_Context
    {
        /// <summary>
        /// Specifies the type of context in the Data field.
        /// </summary>
        [StaticSize(2)]
        public Context_Type ContextType;

        /// <summary>
        /// The length, in bytes, of the Data field
        /// </summary>
        [StaticSize(2)]
        public ushort DataLength;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved.
        /// This value MUST be set to 0 by the client, and MUST be ignored by the server.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved;

        /// <summary>
        /// A variable-length field that contains the tree connect context specified by the ContextType field.
        /// </summary>
        public REMOTED_IDENTITY_TREE_CONNECT_Context data;
    }

    /// <summary>
    /// Specifies the type of context in the Data field. This field MUST be one of the following values.
    /// </summary>
    public enum Context_Type : ushort
    {
        /// <summary>
        /// This value is reserved.
        /// </summary>
        RESERVED_TREE_CONNECT_CONTEXT_ID = 0x0000,

        /// <summary>
        /// The Data field contains remoted identity tree connect context data
        /// as specified in section 2.2.9.2.1.
        /// </summary>
        REMOTED_IDENTITY_TREE_CONNECT_CONTEXT_ID = 0x0001,
    }

    /// <summary>
    /// The SMB2_REMOTED_IDENTITY_TREE_CONNECT context is specified in SMB2_TREE_CONNECT_CONTEXT structure
    /// when the ContextType is set to SMB2_REMOTED_IDENTITY_TREE_CONNECT_CONTEXT_ID.
    /// The format of the data in the Data field of this SMB2_TREE_CONNECT_CONTEXT is as follows.
    /// </summary>
    public struct REMOTED_IDENTITY_TREE_CONNECT_Context
    {
        /// <summary>
        /// A 16-bit integer specifying the type of ticket requested.
        /// The value in this field MUST be set to 0x0001.
        /// </summary>
        [StaticSize(2)]
        public ushort TicketType;

        /// <summary>
        /// A 16-bit integer specifying the total size of this structure.
        /// </summary>
        [StaticSize(2)]
        public ushort TicketSize;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the user information in the TicketInfo buffer.
        /// The user information is stored in SID_ATTR_DATA format as specified in section 2.2.9.2.1.2.
        /// </summary>
        [StaticSize(2)]
        public ushort User;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the null-terminated Unicode string
        /// containing the username in the TicketInfo field.
        /// </summary>
        [StaticSize(2)]
        public ushort UserName;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the null-terminated Unicode string
        /// containing the domain name in the TicketInfo field.
        /// </summary>
        [StaticSize(2)]
        public ushort Domain;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the information
        /// about the groups in the TicketInfo buffer.
        /// The information is stored in SID_ARRAY_DATA format as specified in section 2.2.9.2.1.3.
        /// </summary>
        [StaticSize(2)]
        public ushort Groups;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the information
        /// about the restricted groups in the TicketInfo field.
        /// The information is stored in SID_ARRAY_DATA format as specified in section 2.2.9.2.1.3.
        /// </summary>
        [StaticSize(2)]
        public ushort RestrictedGroups;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the information
        /// about the privileges in the TicketInfo field.
        /// The information is stored in PRIVILEGE_ARRAY_DATA format as specified in section 2.2.9.2.1.6.
        /// </summary>
        [StaticSize(2)]
        public ushort Privileges;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the information
        /// about the primary group in the TicketInfo field.
        /// The information is stored in SID_ARRAY_DATA format as specified in section 2.2.9.2.1.3.
        /// </summary>
        [StaticSize(2)]
        public ushort PrimaryGroup;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the information
        /// about the owner in the TicketInfo field.
        /// The information is stored in BLOB_DATA format as specified in section 2.2.9.2.1.1,
        /// where BlobData contains the SID, as specified in [MS-DTYP] section 2.4.2.2,
        /// representing the owner, and BlobSize contains the size of SID.
        /// </summary>
        [StaticSize(2)]
        public ushort Owner;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the information about the DACL,
        /// as specified in [MS-DTYP] section 2.5.2, in the TicketInfo field.
        /// Information about the DACL is stored in BLOB_DATA format
        /// as specified in section 2.2.9.2.1.1, where BlobSize contains the size of the ACL structure,
        /// as specified in [MS-DTYP] section 2.4.5, and BlobData contains the DACL data.
        /// </summary>
        [StaticSize(2)]
        public ushort DefaultDacl;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the information
        /// about the device groups in the TicketInfo field.
        /// The information is stored in SID_ARRAY_DATA format as specified in section 2.2.9.2.1.3.
        /// </summary>
        [StaticSize(2)]
        public ushort DeviceGroups;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the user claims data in the TicketInfo field.
        /// Information about user claims is stored in BLOB_DATA format
        /// as specified in section 2.2.9.2.1.1,
        /// where BlobData contains an array of CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 structures,
        /// as specified in [MS-DTYP] section 2.4.10.1, representing the claims issued to the user,
        /// and BlobSize contains the size of the user claims data.
        /// </summary>
        [StaticSize(2)]
        public ushort UserClaims;

        /// <summary>
        /// A 16-bit integer specifying the offset, in bytes,
        /// from the beginning of this structure to the device claims data in the TicketInfo field.
        /// Information about device claims is stored in BLOB_DATA format
        /// as specified in section 2.2.9.2.1.1,
        /// where BlobData contains an array of CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 structures,
        /// as specified in [MS-DTYP] section 2.4.10.1,
        /// representing the claims issued to the account of the device which the user is connected from,
        /// and BlobSize contains the size of the device claims data.
        /// </summary>
        [StaticSize(2)]
        public ushort DeviceClaims;

        /// <summary>
        /// A variable-length buffer containing the remoted identity tree connect context data,
        /// including the information about all the previously defined fields in this structure.
        /// </summary>
        public Ticket_Info TicketInfo;
    }

    public struct Ticket_Info
    {
        /// <summary>
        /// The user information is stored in SID_ATTR_DATA format as specified in section 2.2.9.2.1.2.
        /// </summary>
        public SID_ATTR_DATA User;

        /// <summary>
        /// Null-terminated Unicode string containing the username.
        /// </summary>
        public byte[] UserName;

        /// <summary>
        /// Null-terminated Unicode string containing the domain name.
        /// </summary>
        public byte[] Domain;

        /// <summary>
        /// The information is stored in SID_ARRAY_DATA format as specified in section 2.2.9.2.1.3.
        /// </summary>
        public SID_ARRAY_DATA Groups;

        /// <summary>
        /// The information is stored in SID_ARRAY_DATA format as specified in section 2.2.9.2.1.3.
        /// </summary>
        public SID_ARRAY_DATA RestrictedGroups;

        /// <summary>
        /// The information is stored in PRIVILEGE_ARRAY_DATA format as specified in section 2.2.9.2.1.6.
        /// </summary>
        public PRIVILEGE_ARRAY_DATA Privileges;

        /// <summary>
        /// The information is stored in SID_ARRAY_DATA format as specified in section 2.2.9.2.1.3.
        /// </summary>
        public SID_ARRAY_DATA PrimaryGroup;

        /// <summary>
        /// The information is stored in BLOB_DATA format as specified in section 2.2.9.2.1.1,
        /// where BlobData contains the SID, as specified in [MS-DTYP] section 2.4.2.2, representing the owner,
        /// and BlobSize contains the size of SID.
        /// </summary>
        public BLOB_DATA Owner;

        /// <summary>
        /// Information about the DACL is stored in BLOB_DATA format as specified in section 2.2.9.2.1.1,
        /// where BlobSize contains the size of the ACL structure, as specified in [MS-DTYP] section 2.4.5,
        /// and BlobData contains the DACL data.
        /// </summary>
        public BLOB_DATA DefaultDacl;

        /// <summary>
        /// The information is stored in SID_ARRAY_DATA format as specified in section 2.2.9.2.1.3.
        /// </summary>
        public SID_ARRAY_DATA DeviceGroups;

        /// <summary>
        /// Information about user claims is stored in BLOB_DATA format as specified in section 2.2.9.2.1.1,
        /// where BlobData contains an array of CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 structures,
        /// as specified in [MS-DTYP] section 2.4.10.1, representing the claims issued to the user,
        /// and BlobSize contains the size of the user claims data.
        /// </summary>
        public BLOB_DATA UserClaims;

        /// <summary>
        /// Information about device claims is stored in BLOB_DATA format as specified in section 2.2.9.2.1.1,
        /// where BlobData contains an array of CLAIM_SECURITY_ATTRIBUTE_RELATIVE_V1 structures,
        /// as specified in [MS-DTYP] section 2.4.10.1, representing the claims issued to the account of the device which the user is connected from,
        /// and BlobSize contains the size of the device claims data.
        /// </summary>
        public BLOB_DATA DeviceClaims;
    }

    public struct BLOB_DATA
    {
        /// <summary>
        /// Size of the data, in bytes, in BlobData.
        /// </summary>
        public ushort BlobSize;

        /// <summary>
        /// Blob data
        /// </summary>
        [Size("BlobSize")]
        public byte[] BlobData;
    }

    public struct SID_ATTR_DATA
    {
        /// <summary>
        /// SID, as specified in [MS-DTYP] section 2.4.2.2,
        /// information in BLOB_DATA format as specified in section 2.2.9.2.1.1.
        /// BlobSize MUST be set to the size of SID and BlobData MUST be set to the SID value.
        /// </summary>
        public BLOB_DATA SidData;

        /// <summary>
        /// Specified attributes of the SID.
        /// </summary>
        public SID_ATTR Attr;
    }

    /// <summary>
    /// Specified attributes of the SID, containing the following values.
    /// </summary>
    public enum SID_ATTR : uint
    {
        /// <summary>
        /// The SID is enabled for access checks.
        /// A SID without this attribute is ignored during an access check
        /// unless the SE_GROUP_USE_FOR_DENY_ONLY attribute is set.
        /// </summary>
        SE_GROUP_ENABLED = 0x00000004,

        /// <summary>
        /// The SID is enabled by default.
        /// </summary>
        SE_GROUP_ENABLED_BY_DEFAULT = 0x00000002,

        /// <summary>
        /// The SID is a mandatory integrity SID.
        /// </summary>
        SE_GROUP_INTEGRITY = 0x00000020,

        /// <summary>
        /// The SID is enabled for mandatory integrity checks.
        /// </summary>
        SE_GROUP_INTEGRITY_ENABLED = 0x00000040,

        /// <summary>
        /// The SID is a logon SID that identifies the logon session associated with an access token.
        /// </summary>
        SE_GROUP_LOGON_ID = 0xc0000000,

        /// <summary>
        /// The SID cannot have the SE_GROUP_ENABLED attribute cleared.
        /// </summary>
        SE_GROUP_MANDATORY = 0x00000001,

        /// <summary>
        /// The SID identifies a group account for which the user of the token is the owner of the group,
        /// or the SID can be assigned as the owner of the token or objects.
        /// </summary>
        SE_GROUP_OWNER = 0x00000008,

        /// <summary>
        /// The SID identifies a domain-local group.
        /// </summary>
        SE_GROUP_RESOURCE = 0x20000000,

        /// <summary>
        /// The SID is a deny-only SID in a restricted token.
        /// If this attribute is set, SE_GROUP_ENABLED is not set, and the SID cannot be reenabled.
        /// </summary>
        SE_GROUP_USE_FOR_DENY_ONLY = 0x00000010,
    }

    public struct SID_ARRAY_DATA
    {
        /// <summary>
        /// Number of SID_ATTR_DATA elements in SidAttrList array.
        /// </summary>
        public ushort SidAttrCount;

        /// <summary>
        /// An array with SidAttrCount number of SID_ATTR_DATA elements as specified in section 2.2.9.2.1.2.
        /// </summary>
        [Size("SidAttrCount")]
        public SID_ATTR_DATA[] SidAttrList;
    }

    public struct LUID_ATTR_DATA
    {
        /// <summary>
        /// LUID is a locally unique identifier, as specified in [MS-DTYP] section 2.3.7.
        /// </summary>
        public _LUID Luid;

        /// <summary>
        /// LUID attributes as specified in [MS-LSAD] section 2.2.5.4.
        /// </summary>
        public uint Attr;
    }

    public struct PRIVILEGE_DATA
    {
        /// <summary>
        /// BlobSize MUST be set to the size of LUID_ATTR_DATA structure
        /// </summary>
        public ushort BlobSize;

        /// <summary>
        /// BlobData MUST be set to the LUID_ATTR_DATA specified in section 2.2.9.2.1.4
        /// </summary>
        [Size("BlobSize")]
        public byte[] BlobData;
    }

    public struct PRIVILEGE_ARRAY_DATA
    {
        /// <summary>
        /// Number of PRIVILEGE_DATA elements in PrivilegeList array.
        /// </summary>
        public ushort PrivilegeCount;

        /// <summary>
        /// An array with PrivilegeCount number of PRIVILEGE_DATA elements as specified in section 2.2.9.2.1.5.
        /// </summary>
        [Size("PrivilegeCount")]
        public PRIVILEGE_DATA[] PrivilegeList;
    }

    /// <summary>
    ///  The FILETIME structure is a 64-bit value that represents
    ///  the number of 100-nanosecond intervals that have elapsed
    ///  since January 1, 1601, in Coordinated Universal Time
    ///  (UTC) format.
    /// </summary>
    public partial struct _FILETIME
    {

        /// <summary>
        ///  A 32-bit unsigned integer in little-endian format that
        ///  contains the low-order bits of the file time.
        /// </summary>
        public uint dwLowDateTime;

        /// <summary>
        ///  A 32-bit unsigned integer in little-endian format that
        ///  contains the high-order bits of the file time.
        /// </summary>
        public uint dwHighDateTime;

        public static _FILETIME Zero
        {
            get
            {
                return new _FILETIME();
            }
        }
    }

    /// <summary>
    ///  The Generic Reparse Data Buffer data element is as follows.
    /// </summary>
    public partial struct GenericReparseBuffer
    {

        /// <summary>
        ///  A 32-bit unsigned integer value containing the reparse
        ///  point tag that uniquely identifies the owner (that
        ///  is, the implementer of the filter driver associated
        ///  with this reparse tag) of the reparse point.
        /// </summary>
        [StaticSize(4)]
        public uint ReparseTag;

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size
        ///  in bytes of the reparse data in the DataBuffer member.
        /// </summary>
        [StaticSize(2)]
        public ushort ReparseDataLength;

        /// <summary>
        ///  A 16-bit field. This field is reserved. This field SHOULD
        ///  be set to 0, and MUST be ignored.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  A variable-length array of 8-bit unsigned integer values
        ///  containing reparse-specific data for the reparse point.
        ///  The format of this data is defined by the owner (that
        ///  is, the implementer of the filter driver associated
        ///  with the specified ReparseTag) of the reparse point.
        /// </summary>
        public byte DataBuffer;
    }

    /// <summary>
    ///  The Symbolic Link Reparse Data Buffer data element,
    ///  which contains information on symbolic link reparse
    ///  points, is as follows.
    /// </summary>
    public partial struct SymbolicLinkReparseBuffer
    {

        /// <summary>
        ///  A 32-bit unsigned integer value containing the reparse
        ///  point tag that uniquely identifies the owner (that
        ///  is, the implementer of the filter driver associated
        ///  with this ReparseTag) of the reparse point. This value
        ///  MUST be 0xA000000C, a reparse point tag assigned to
        ///  Microsoft.
        /// </summary>
        [StaticSize(4)]
        public SymbolicLinkReparseBuffer_ReparseTag_Values ReparseTag;

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size
        ///  in bytes of the reparse data in the PathBuffer member.
        ///  This value is the length of the data starting at the
        ///  SubstituteNameOffset field (or SubstituteNameLength
        ///  + PrintNameLength + 12).
        /// </summary>
        [StaticSize(2)]
        public ushort ReparseDataLength;

        /// <summary>
        ///  A 16-bit field. This field is not used. It SHOULD be
        ///  set to 0, and MUST be ignored.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST contain the offset
        ///  in bytes of the substitute name string in the PathBuffer
        ///  array, computed as an offset from byte 0 of PathBuffer.
        ///  Note that this offset must be divided by 2 to get the
        ///  array index.
        /// </summary>
        [StaticSize(2)]
        public ushort SubstituteNameOffset;

        /// <summary>
        ///   A 16-bit unsigned integer that contains the length
        ///  in bytes of the substitute name string. If this string
        ///  is NULL-terminated, SubstituteNameLength does not include
        ///  the Unicode NULL character.
        /// </summary>
        [StaticSize(2)]
        public ushort SubstituteNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the offset in
        ///  bytes of the print name string in the PathBuffer array,
        ///  computed as an offset from byte 0 of PathBuffer. Note
        ///  that this offset must be divided by 2 to get the array
        ///  index.
        /// </summary>
        [StaticSize(2)]
        public ushort PrintNameOffset;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the length in
        ///  bytes of the print name string. If this string is NULL-terminated,
        ///  PrintNameLength does not include the Unicode NULL character.
        /// </summary>
        [StaticSize(2)]
        public ushort PrintNameLength;

        /// <summary>
        ///  A 32-bit bit field that specifies whether the path name
        ///  given in the SubstituteName field contains a full 
        ///  path name or a path name relative to the source. A
        ///  relative path name, such as "..\locals", accesses a
        ///  directory relative to the current position in the directory
        ///  hierarchy.
        /// </summary>
        [StaticSize(4)]
        public SymbolicLinkReparseBuffer_Flags_Values Flags;

        /// <summary>
        ///  Unicode string that contains the substitute name string
        ///  and print name string. The substitute name and print
        ///  name strings can appear in any order in the PathBuffer.
        ///  To locate the substitute name and print name strings
        ///  in the PathBuffer, use the SubstituteNameOffset, SubstituteNameLength,
        ///  PrintNameOffset, and PrintNameLength members.
        /// </summary>
        [Size("PrintNameOffset - SubstituteNameOffset + PrintNameLength")]
        public byte[] PathBuffer;
    }

    /// <summary>
    /// SymbolicLinkReparseBuffer_ReparseTag_Values
    /// </summary>
    public enum SymbolicLinkReparseBuffer_ReparseTag_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        IO_REPARSE_TAG_SYMLINK = 0xa000000c,
    }

    /// <summary>
    /// SymbolicLinkReparseBuffer_Flags_Values
    /// </summary>
    [Flags()]
    public enum SymbolicLinkReparseBuffer_Flags_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  When this bit is clear (0), the path name given in the
        ///  SubstituteName field contains a full  path name.When
        ///  this bit is set (1), the given path name is relative
        ///  to the source.
        /// </summary>
        SYMLINK_FLAG_RELATIVE = 0x00000001,
    }

    /// <summary>
    ///  The Mount Point Reparse Data Buffer data element, which
    ///  contains information on mount point reparse points,
    ///  is as follows.
    /// </summary>
    public partial struct MountPointReparseBuffer
    {

        /// <summary>
        ///  A 32-bit unsigned integer value containing the reparse
        ///  point tag that uniquely identifies the owner (that
        ///  is, the implementer of the filter driver associated
        ///  with this ReparseTag) of the reparse point. This value
        ///  MUST be 0xA0000003, a reparse point tag assigned to
        ///  Microsoft.
        /// </summary>
        [StaticSize(4)]
        public MountPointReparseBuffer_ReparseTag_Values ReparseTag;

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size
        ///  in bytes of the reparse data in the PathBuffer member.
        ///  This value is the length of the data starting at the
        ///  SubstituteNameOffset field (or SubstituteNameLength
        ///  + PrintNameLength + 8).
        /// </summary>
        [StaticSize(2)]
        public ushort ReparseDataLength;

        /// <summary>
        ///  A 16-bit field. This field is not used. It SHOULD be
        ///  set to 0, and MUST be ignored.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST contain the offset
        ///  in bytes of the substitute name string in the PathBuffer
        ///  array, computed as an offset from byte 0 of PathBuffer.
        ///  Note that this offset must be divided by 2 to get the
        ///  array index.
        /// </summary>
        [StaticSize(2)]
        public ushort SubstituteNameOffset;

        /// <summary>
        ///   A 16-bit unsigned integer that contains the length
        ///  in bytes of the substitute name string. If this string
        ///  is NULL-terminated, SubstituteNameLength does not include
        ///  the Unicode NULL character.
        /// </summary>
        [StaticSize(2)]
        public ushort SubstituteNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the offset in
        ///  bytes of the print name string in the PathBuffer array,
        ///  computed as an offset from byte 0 of PathBuffer. Note
        ///  that this offset must be divided by 2 to get the array
        ///  index.
        /// </summary>
        [StaticSize(2)]
        public ushort PrintNameOffset;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the length in
        ///  bytes of the print name string. If this string is NULL-terminated,
        ///  PrintNameLength does not include the Unicode NULL character.
        /// </summary>
        [StaticSize(2)]
        public ushort PrintNameLength;

        /// <summary>
        ///  Unicode string that contains the substitute name string
        ///  and print name string. The substitute name and print
        ///  name strings can appear in any order in the PathBuffer.
        ///  To locate the substitute name and print name strings
        ///  in the PathBuffer, use the SubstituteNameOffset, SubstituteNameLength,
        ///  PrintNameOffset, and PrintNameLength members.
        /// </summary>
        public ushort PathBuffer;
    }

    /// <summary>
    /// MountPointReparseBuffer_ReparseTag_Values
    /// </summary>
    public enum MountPointReparseBuffer_ReparseTag_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        IO_REPARSE_TAG_MOUNT_POINT = 0xa0000003,
    }

    /// <summary>
    ///  The REPARSE_GUID_DATA_BUFFER data element MUST be used
    ///  by all non-Microsoft file systems, filters, and minifilters
    ///  to store data for a reparse point. Each reparse point
    ///  contains one REPARSE_GUID_DATA_BUFFER structure.Reparse
    ///  point tags are assigned to independent software vendors
    ///  (ISVs) by Microsoft. Reparse point GUIDs are assigned
    ///  by the ISV. An ISV MUST associate one GUID to each
    ///  assigned reparse point tag, and MUST always use that
    ///  GUID with that tag.For more information on reparse
    ///  points, see [REPARSE].If the high bit of the ReparseTag
    ///  element is 0, an application MUST interpret the reparse
    ///  point data as a REPARSE_GUID_DATA_BUFFER; otherwise,
    ///  it MUST be interpreted as a REPARSE_DATA_BUFFER.Each
    ///  reparse point MUST contain one REPARSE_GUID_DATA_BUFFER
    ///  structure. The REPARSE_GUID_DATA_BUFFER data element
    ///  is as follows.
    /// </summary>
    public partial struct REPARSE_GUID_DATA_BUFFER
    {

        /// <summary>
        ///  A 32-bit unsigned integer value containing the reparse
        ///  point tag that uniquely identifies the owner of the
        ///  reparse point. This tag MUST match the reparse tag
        ///  of the reparse point on which the FSCTL is being invoked.
        /// </summary>
        [StaticSize(4)]
        public uint ReparseTag;

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size
        ///  in bytes of the reparse data in the DataBuffer member.
        /// </summary>
        [StaticSize(2)]
        public ushort ReparseDataLength;

        /// <summary>
        ///  A 16-bit field. This field SHOULD be set to 0 by the
        ///  client, and MUST be ignored by the server.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the owner of
        ///  the reparse point. Reparse point GUIDs are not assigned
        ///  by Microsoft. A reparse point implementer MUST select
        ///  one GUID to be used with their assigned reparse point
        ///  tag to uniquely identity that reparse point. For more
        ///  information, see [REPARSE].
        /// </summary>
        [StaticSize(16)]
        public System.Guid ReparseGuid;

        /// <summary>
        ///  The content of this buffer is undefined to the file
        ///  system. On receipt, its   content MUST be preserved
        ///  and properly returned to the caller.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] DataBuffer;
    }

    /// <summary>
    ///  The first possible structure for the FILE_OBJECTID_BUFFER
    ///  data element follows.
    /// </summary>
    public partial struct FILE_OBJECTID_BUFFER_Type_1
    {

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the file or
        ///  directory within the volume  on which it resides. Specifically,
        ///  the same object ID can be assigned to another file
        ///  or directory on a different volume, but it MUST NOT
        ///  be assigned to another file or directory on the same
        ///  volume.
        /// </summary>
        [StaticSize(16)]
        public System.Guid ObjectId;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the volume on
        ///  which the object resided when the object identifier
        ///  was created, or zero if the volume had no object identifier
        ///  at that time. After copy operations, move operations,
        ///  or other file operations, this may not be the same
        ///  as the object identifier of the volume on which the
        ///  object presently resides.
        /// </summary>
        [StaticSize(16)]
        public System.Guid BirthVolumeId;

        /// <summary>
        ///  A 16-byte GUID value containing the object identifier
        ///  of the object at the time it was created. Copy operations,
        ///  move operations, or other file operations MAY change
        ///  the value of the ObjectId member. Therefore, the BirthObjectId
        ///  MAY not be the same as the ObjectId member at present.
        ///  Specifically, the same object ID MAY be assigned to
        ///  another file or directory on a different volume, but
        ///  it MUST NOT be assigned to another file or directory
        ///  on the same volume. The object ID is assigned at file
        ///  creation time.When a file is moved or copied from one
        ///  volume to another, the ObjectId member value changes
        ///  to a random unique value to avoid the potential for
        ///  ObjectId collisions because the object ID is not guaranteed
        ///  to be unique across volumes.
        /// </summary>
        [StaticSize(16)]
        public System.Guid BirthObjectId;

        /// <summary>
        ///  A 16-byte GUID value containing the domain identifier.
        ///  This value is unused; it SHOULD be zero, and MUST be
        ///  ignored.The NTFS file system places no constraints
        ///  on the format of the 48 bytes of information following
        ///  the ObjectId in this structure. This format of the
        ///  FILE_OBJECTID_BUFFER is used on  by the Microsoft Distributed
        ///  Link Tracking Service (see [MS-DLTW] section 3.1.6).
        /// </summary>
        [StaticSize(16)]
        public System.Guid DomainId;
    }

    /// <summary>
    ///  The second possible structure for the FILE_OBJECTID_BUFFER
    ///  data element follows.
    /// </summary>
    public partial struct FILE_OBJECTID_BUFFER_Type_2
    {

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the file or
        ///  directory within the volume on which it resides.  Specifically,
        ///  the same object ID can be assigned to another file
        ///  or directory on a different volume, but it MUST NOT
        ///  be assigned to another file or directory on the same
        ///  volume.
        /// </summary>
        [StaticSize(16)]
        public System.Guid ObjectId;

        /// <summary>
        ///  A 48-byte value containing extended data that was set
        ///  with the FSCTL_SET_OBJECT_ID_EXTENDED request. This
        ///  field contains application-specific data. places distributed
        ///  link tracking information into the ExtendedInfo field
        ///  for use by the Distributed Link Tracking protocols
        ///  (see [MS-DLTW] section 3.1.6).
        /// </summary>
        [StaticSize(48)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    ///  The FSCTL_FIND_FILES_BY_SID reply message returns the
    ///  results of the FSCTL_FIND_FILES_BY_SID request as an
    ///  array of FIND_BY_SID_OUTPUT data elements, one for
    ///  each matching file that is found.The FIND_BY_SID_OUTPUT
    ///  data element is as follows.
    /// </summary>
    public partial struct FSCTL_FIND_FILES_BY_SID_Reply
    {

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size
        ///  of the file name in bytes. This size does not include
        ///  the NULL character.
        /// </summary>
        [StaticSize(2)]
        public ushort FileNameLength;

        /// <summary>
        ///  A null-terminated Unicode string that specifies the
        ///  fully qualified path name for the file.
        /// </summary>
        public ushort FileName;
    }

    /// <summary>
    ///  The FSCTL_GET_COMPRESSION reply message returns the
    ///  results of the FSCTL_GET_COMPRESSION request as a 16-bit
    ///  unsigned integer value that indicates the current compression
    ///  state of the file or directory.The CompressionState
    ///  element is as follows.
    /// </summary>
    public partial struct FSCTL_GET_COMPRESSION_Reply
    {

        /// <summary>
        ///  One of the following standard values MUST be returned.
        /// </summary>
        [StaticSize(2)]
        public CompressionState_Values CompressionState;
    }

    /// <summary>
    /// CompressionState_Values
    /// </summary>
    public enum CompressionState_Values : ushort
    {

        /// <summary>
        ///  The file or directory is not compressed.
        /// </summary>
        COMPRESSION_FORMAT_NONE = 0x0000,

        /// <summary>
        ///  The file or directory is compressed by using the default
        ///  compression algorithm.Equivalent to COMPRESSION_FORMAT_LZNT1.
        /// </summary>
        COMPRESSION_FORMAT_DEFAULT = 0x0001,

        /// <summary>
        ///  The file or directory is compressed by using the LZNT1
        ///  compression algorithm. For more information, see [UASDC].
        /// </summary>
        COMPRESSION_FORMAT_LZNT1 = 0x0002,
    }

    /// <summary>
    ///  The FSCTL_GET_RETRIEVAL_POINTERS request message requests
    ///  that the server return a variably sized data element,
    ///  StartingVcn, that describes the location on disk of
    ///  the file or directory associated with the handle on
    ///  which this FSCTL was invoked. It is most commonly used
    ///  by defragmentation utilities. This data element describes
    ///  the mapping between virtual cluster numbers and logical
    ///  cluster numbers.The STARTING_VCN_INPUT_BUFFER data
    ///  element is as follows. 
    /// </summary>
    public partial struct FSCTL_GET_RETRIEVAL_POINTERS_Request
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the virtual cluster
        ///  number (VCN) at which to begin retrieving extents in
        ///  the file. This value MAY be rounded down to the first
        ///  VCN of the extent in which the given extent is found.
        ///  This value MUST be greater than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long StartingVcn;
    }

    /// <summary>
    ///  The FSCTL_GET_RETRIEVAL_POINTERS reply message returns
    ///  the results of the FSCTL_GET_RETRIEVAL_POINTERS request
    ///  as a variably sized data element, RETRIEVAL_POINTERS_BUFFER,
    ///  that specifies the allocation and location on disk
    ///  of a specific file.For the NTFS file system, the
    ///  FSCTL_GET_RETRIEVAL_POINTERS reply returns the extent
    ///  locations (that is, locations of allocated regions
    ///  of disk space) of nonresident data. A file system MAY
    ///  allow resident data, which is data that can be written
    ///  to disk within the file's directory record. Because
    ///  resident data requires no additional disk space allocation,
    ///  no extent locations are associated with resident data.On
    ///  an NTFS volume, very short data streams (typically
    ///  several hundred bytes) can be written to disk without
    ///  having any clusters allocated. These short streams
    ///  are sometimes called resident because the data resides
    ///  in the file's master file table (MFT) record. A resident
    ///  data stream has no retrieval pointers to return.The
    ///  RETRIEVAL_POINTERS_BUFFER data element is as follows.
    /// </summary>
    public partial struct FSCTL_GET_RETRIEVAL_POINTERS_Reply
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  EXTENTS data elements in the Extents array. This number
        ///  can be zero if there are no clusters allocated at (or
        ///  beyond) the specified StartingVcn.
        /// </summary>
        [StaticSize(4)]
        public uint ExtentCount;

        /// <summary>
        ///  Unused area for data alignment purposes.
        /// </summary>
        [StaticSize(4)]
        public uint Unused;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting virtual
        ///  cluster number (VCN) returned by the FSCTL_GET_RETRIEVAL_POINTERS
        ///  reply. This is not necessarily the VCN requested by
        ///  the FSCTL_GET_RETRIEVAL_POINTERS request, as the file
        ///  system driver might round down to the first VCN of
        ///  the extent in which the requested starting VCN is found.
        ///  This value MUST be greater than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long StartingVcn;

        /// <summary>
        ///  An array of zero or more EXTENTS data elements. For
        ///  the number of EXTENTS data elements in the array, see
        ///  ExtentCount.
        /// </summary>
        [Size("ExtentCount")]
        public EXTENTS[] Extents;
    }

    /// <summary>
    ///  The EXTENTS data element is as follows.
    /// </summary>
    public partial struct EXTENTS
    {

        /// <summary>
        ///  A 64-bit unsigned integer that contains the VCN at which
        ///  the next extent begins. This value minus either StartingVcn
        ///  (for the first Extents array element) or the NextVcn
        ///  of the previous element of the array (for all other
        ///  Extents array elements) is the length in clusters of
        ///  the current extent. The length is an input to the FSCTL_MOVE_FILE
        ///  request.
        /// </summary>
        [StaticSize(8)]
        public ulong NextVcn;

        /// <summary>
        ///  A 64-bit unsigned integer that contains the logical
        ///  cluster number (LCN) at which the current extent begins
        ///  on the volume. On the NTFS file system, a 64-bit value
        ///  of 0xFFFFFFFFFFFFFFFF indicates either a compression
        ///  unit that is partially allocated or an unallocated
        ///  region of a sparse file. For more information on sparse
        ///  files, see [SPARSE]. NTFS performs compression in 16-cluster
        ///  units. If a given 16-cluster unit compresses to fit
        ///  in, for example, 9 clusters, there will be a 7-cluster
        ///  extent of the file with an LCN of -1.
        /// </summary>
        [StaticSize(8)]
        public ulong Lcn;
    }

    /// <summary>
    ///  The FSCTL_IS_PATHNAME_VALID request message requests
    ///  that the server return whether or not the specified
    ///  path name associated with the handle on which this
    ///  FSCTL was invoked is composed of valid characters with
    ///  respect to the remote file system storage.The
    ///  data element is as follows.
    /// </summary>
    public partial struct FSCTL_IS_PATHNAME_VALID_Request
    {

        /// <summary>
        ///  An unsigned 32-bit integer that specifies the length
        ///  in bytes of the PathName data element.
        /// </summary>
        [StaticSize(4)]
        public uint PathNameLength;

        /// <summary>
        ///   A variable-length Unicode string  that specifies the
        ///  path name.
        /// </summary>
        [Size("PathNameLength")]
        public byte[] PathName;
    }

    /// <summary>
    ///  The FSCTL_LMR_GET_LINK_TRACKING_INFORMATION reply message
    ///  returns the results of the FSCTL_LMR_GET_LINK_TRACKING_INFORMATION
    ///  request. The LINK_TRACKING_INFORMATION data element
    ///  is as follows.
    /// </summary>
    public partial struct FSCTL_LMR_GET_LINK_TRACKING_INFORMATION_Reply
    {

        /// <summary>
        ///  An unsigned 32-bit integer that indicates the type of
        ///  file system on which the file is hosted on the destination
        ///  computer. This value MUST be one of the following.
        /// </summary>
        [StaticSize(4)]
        public Type_Values Type;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the volume for
        ///  the object, as obtained from the reply to an FSCTL_LMR_GET_LINK_TRACKING_INFORMATION
        ///  request, called by using the file handle of the destination
        ///  file.
        /// </summary>
        [StaticSize(16)]
        public System.Guid VolumeId;
    }

    /// <summary>
    /// Type_Values
    /// </summary>
    public enum Type_Values : uint
    {

        /// <summary>
        ///  The destination file system is NTFS.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  The destination file system is DFS. For more information,
        ///  see [MSDFS].
        /// </summary>
        V2 = 0x00000001,
    }

    /// <summary>
    ///  The FSCTL_LMR_SET_LINK_TRACKING_INFORMATION request
    ///  message sets distributed link tracking information
    ///  such as file system type, volume ID, object ID, and
    ///  destination computer's NetBIOS name for the file associated
    ///  with the handle on which this FSCTL was invoked. The
    ///  message contains a REMOTE_LINK_TRACKING_INFORMATION
    ///  data element. For more information on distributed link
    ///  tracking, see [MS-DLTW] section 3.1.6.The REMOTE_LINK_TRACKING_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FSCTL_LMR_SET_LINK_TRACKING_INFORMATION_Request
    {

        /// <summary>
        ///  Fid of the file from which to obtain link tracking information.
        ///   For Fid type, see [MS-SMB] section 2.2.14.7.
        /// </summary>
        [StaticSize(4)]
        public uint TargetFileObject;

        /// <summary>
        ///  Length of the TargetLinkTrackingInformationBuffer.
        /// </summary>
        [StaticSize(4)]
        public uint TargetLinkTrackingInformationLength;

        /// <summary>
        ///  This field is as specified in  TARGET_LINK_TRACKING_INFORMATION_Buffer.
        /// </summary>
        [Size("TargetLinkTrackingInformationLength")]
        public byte[] TargetLinkTrackingInformationBuffer;
    }

    /// <summary>
    ///  The TARGET_LINK_TRACKING_INFORMATION_Buffer data element
    ///  is as follows.
    /// </summary>
    public partial struct TARGET_LINK_TRACKING_INFORMATION_Buffer
    {

        /// <summary>
        ///  An unsigned 32-bit integer that indicates the type of
        ///  file system on which the file is hosted on the destination
        ///  computer. MUST be one of the following.
        /// </summary>
        [StaticSize(4)]
        public TARGET_LINK_TRACKING_INFORMATION_Buffer_Type_Values Type;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the volume for
        ///  the object, as obtained from the reply to an  FSCTL_LMR_GET_LINK_TRACKING_INFORMATION
        ///  request, called using the file handle of the destination
        ///  file.
        /// </summary>
        [StaticSize(16)]
        public System.Guid VolumeId;

        /// <summary>
        ///   A 16-byte GUID that uniquely identifies the destination
        ///  file or directory within the volume on which it resides,
        ///  as indicated by VolumeId.
        /// </summary>
        [StaticSize(16)]
        public System.Guid ObjectId;

        /// <summary>
        ///  A null-terminated ASCII string containing the NetBIOS
        ///  name of the destination computer, if known. For more
        ///  information, see [MS-DLTW] section 3.1.6. If not known,
        ///  this field is zero length and contains nothing.
        /// </summary>
        public byte[] NetBIOSName;
    }

    /// <summary>
    /// TARGET_LINK_TRACKING_INFORMATION_Buffer_Type_Values
    /// </summary>
    public enum TARGET_LINK_TRACKING_INFORMATION_Buffer_Type_Values : uint
    {

        /// <summary>
        ///  The destination file system is NTFS.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  The destination file system is DFS. For more information,
        ///  see [MSDFS].
        /// </summary>
        V2 = 0x00000001,
    }

    /// <summary>
    ///  The FSCTL_QUERY_ALLOCATED_RANGES request message requests
    ///  that the server scan a file or alternate stream looking
    ///  for byte ranges that may contain nonzero data, and
    ///  then return information on those ranges. Only sparse
    ///  files can have zeroed ranges known to the operating
    ///  system. For other files, the server will return only
    ///  a single range that contains the starting point and
    ///  the length requested. The message contains a FILE_ALLOCATED_RANGE_BUFFER
    ///  data element. The FILE_ALLOCATED_RANGE_BUFFER data element
    ///  is as follows.
    /// </summary>
    public partial struct FSCTL_QUERY_ALLOCATED_RANGES_Request
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file offset
        ///  in bytes of the start of a range of bytes in a file.
        ///  The value of this field MUST be greater than or equal
        ///  to 0.
        /// </summary>
        [StaticSize(8)]
        public long FileOffset;

        /// <summary>
        ///  A 64-bit signed integer that contains the size in bytes
        ///  of the range. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long Length;
    }

    /// <summary>
    ///  The FSCTL_QUERY_ALLOCATED_RANGES reply message returns
    ///  the results of the FSCTL_QUERY_ALLOCATED_RANGES request.This
    ///  message MUST return an array of zero or more FILE_ALLOCATED_RANGE_BUFFER
    ///  data elements. The number of FILE_ALLOCATED_RANGE_BUFFER
    ///  elements returned is computed by dividing the size
    ///  of the returned output buffer (from SMB, the lower-level
    ///  protocol that carries the FSCTL) by the size of the
    ///  FILE_ALLOCATED_RANGE_BUFFER element. Ranges returned
    ///  are always at least partially within the range specified
    ///  in the FSCTL_QUERY_ALLOCATED_RANGES request. Zero FILE_ALLOCATED_RANGE_BUFFER
    ///  data elements MUST be returned when the file has no
    ///  allocated ranges.Each entry in the output array contains
    ///  an offset and a length that indicates a range in the
    ///  file that may contain nonzero data. The actual nonzero
    ///  data, if any, is somewhere within this range, and the
    ///  calling application must scan further within the range
    ///  to locate it and determine if it really is valid data.
    ///  Multiple instances of valid data may exist within the
    ///  range.The FILE_ALLOCATED_RANGE_BUFFER data element
    ///  is as follows.
    /// </summary>
    public partial struct FSCTL_QUERY_ALLOCATED_RANGES_Reply
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file offset
        ///  in bytes from the start of the file; the start of a
        ///  range of bytes to which storage is allocated. If the
        ///  file is a sparse file, it can contain ranges of bytes
        ///  for which storage is not allocated; these ranges will
        ///  be excluded from the list of allocated ranges returned
        ///  by this FSCTL.Sparse files are supported by  , , ,
        ///  and . The NTFS file system rounds down the input file
        ///  offset to a 65,536-byte (64 kilobyte) boundary, rounds
        ///  up the length to a convenient boundary, and then begins
        ///  to walk through the file. Because an application using
        ///  a sparse file can choose whether or not to allocate
        ///  disk space for each sequence of 0x00-valued bytes,
        ///  the allocated ranges can contain 0x00-valued bytes.
        ///  This value MUST be greater than or equal to 0. does
        ///  not track every piece of zero (0) or nonzero data.
        ///  Because zero (0) is often a perfectly legal datum,
        ///  it would be misleading. Instead, the system tracks
        ///  ranges in which disk space is allocated. Where no disk
        ///  space is allocated, all data bytes within that range
        ///  for Length bytes from FileOffset are assumed to be
        ///  zero (0) (when data is read, NTFS returns a zero for
        ///  every byte in a sparse region). Allocated storage can
        ///  contain zero (0) or nonzero data. So all that this
        ///  operation does is return information on parts of the
        ///  file where nonzero data might be located. It is up
        ///  to the application to scan these parts of the file
        ///  in accordance with the application's data conventions.
        /// </summary>
        [StaticSize(8)]
        public long FileOffset;

        /// <summary>
        ///  A 64-bit signed integer that contains the size
        ///  in bytes of the range. This value MUST be greater than
        ///  or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long Length;
    }

    /// <summary>
    /// Reason_Values
    /// </summary>
    [Flags()]
    public enum Reason_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  A user has either changed one or more files or directory
        ///  attributes (such as read-only, hidden, archive, or
        ///  sparse) or one or more time stamps.
        /// </summary>
        USN_REASON_BASIC_INFO_CHANGE = 0x00008000,

        /// <summary>
        ///  The file or directory is closed.
        /// </summary>
        USN_REASON_CLOSE = 0x80000000,

        /// <summary>
        ///  The compression state of the file or directory is changed
        ///  from (or to) compressed.
        /// </summary>
        USN_REASON_COMPRESSION_CHANGE = 0x00020000,

        /// <summary>
        ///  The file or directory is extended (added to).
        /// </summary>
        USN_REASON_DATA_EXTEND = 0x00000002,

        /// <summary>
        ///  The data in the file or directory is overwritten.
        /// </summary>
        USN_REASON_DATA_OVERWRITE = 0x00000001,

        /// <summary>
        ///  The file or directory is truncated.
        /// </summary>
        USN_REASON_DATA_TRUNCATION = 0x00000004,

        /// <summary>
        ///  The user made a change to the extended attributes of
        ///  a file or directory. These NTFS file system attributes
        ///  are not accessible to -based applications. This USN
        ///  reason does not appear under normal system usage, but
        ///  can appear if an application or utility bypasses the
        ///  Win32 API and uses the native API to create or modify
        ///  extended attributes of a file or directory.
        /// </summary>
        USN_REASON_EA_CHANGE = 0x00000400,

        /// <summary>
        ///  The file or directory is encrypted or decrypted.
        /// </summary>
        USN_REASON_ENCRYPTION_CHANGE = 0x00040000,

        /// <summary>
        ///  The file or directory is created for the first time.
        /// </summary>
        USN_REASON_FILE_CREATE = 0x00000100,

        /// <summary>
        ///  The file or directory is deleted.
        /// </summary>
        USN_REASON_FILE_DELETE = 0x00000200,

        /// <summary>
        ///  An NTFS file system hard link is added to (or removed
        ///  from) the file or directory. An NTFS file system hard
        ///  link, similar to a POSIX hard link, is one of several
        ///  directory entries that see the same file or directory.
        /// </summary>
        USN_REASON_HARD_LINK_CHANGE = 0x00010000,

        /// <summary>
        ///  A user changes the FILE_ATTRIBUTE_NOT_CONTEXT_INDEXED
        ///  attribute. That is, the user changes the file or directory
        ///  from one in which content can be indexed to one in
        ///  which content cannot be indexed, or vice versa.
        /// </summary>
        USN_REASON_INDEXABLE_CHANGE = 0x00004000,

        /// <summary>
        ///  The one (or more) named data stream for a file is extended
        ///  (added to).
        /// </summary>
        USN_REASON_NAMED_DATA_EXTEND = 0x00000020,

        /// <summary>
        ///  The data in one (or more) named data stream for a file
        ///  is overwritten.
        /// </summary>
        USN_REASON_NAMED_DATA_OVERWRITE = 0x00000010,

        /// <summary>
        ///  One (or more) named data stream for a file is truncated.
        /// </summary>
        USN_REASON_NAMED_DATA_TRUNCATION = 0x00000040,

        /// <summary>
        ///  The object identifier of a file or directory is changed.
        /// </summary>
        USN_REASON_OBJECT_ID_CHANGE = 0x00080000,

        /// <summary>
        ///  A file or directory is renamed, and the file name in
        ///  the USN_RECORD structure is the new name.
        /// </summary>
        USN_REASON_RENAME_NEW_NAME = 0x00002000,

        /// <summary>
        ///  The file or directory is renamed, and the file name
        ///  in the USN_RECORD structure is the previous name.
        /// </summary>
        USN_REASON_RENAME_OLD_NAME = 0x00001000,

        /// <summary>
        ///  The reparse point that is contained in a file or directory
        ///  is changed, or a reparse point is added to (or deleted
        ///  from) a file or directory.
        /// </summary>
        USN_REASON_REPARSE_POINT_CHANGE = 0x00100000,

        /// <summary>
        ///  A change is made in the access rights to a file or directory.
        /// </summary>
        USN_REASON_SECURITY_CHANGE = 0x00000800,

        /// <summary>
        ///  A named stream is added to (or removed from) a file,
        ///  or a named stream is renamed.
        /// </summary>
        USN_REASON_STREAM_CHANGE = 0x00200000,
    }

    /// <summary>
    /// SourceInfo_Values
    /// </summary>
    [Flags()]
    public enum SourceInfo_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The operation provides information on a change to the
        ///  file or directory that was made by the operating system.
        ///  For example, a change journal record with this SourceInfo
        ///  value is generated when the Remote Storage system moves
        ///  data from external to local storage. This SourceInfo
        ///  value indicates that the modifications did not change
        ///  the application data in the file.
        /// </summary>
        USN_SOURCE_DATA_MANAGEMENT = 0x00000001,

        /// <summary>
        ///  The operation adds a private data stream to a file or
        ///  directory. For example, a virus detector might add
        ///  checksum information. As the virus detector modifies
        ///  the item, the system generates USN records. This SourceInfo
        ///  value indicates that the modifications did not change
        ///  the application data in the file.
        /// </summary>
        USN_SOURCE_AUXILIARY_DATA = 0x00000002,

        /// <summary>
        ///  The operation modified the file to match the content
        ///  of the same file that exists in another member of the
        ///  replica set  for the File Replication Service (FRS).
        ///  For more information on FRS, see [MS-FRS1].
        /// </summary>
        USN_SOURCE_REPLICATION_MANAGEMENT = 0x00000004,
    }

    /// <summary>
    ///  The FSCTL_SET_COMPRESSION request message requests that
    ///  the server set the compression state of the file or
    ///  directory (associated with the handle on which this
    ///  FSCTL was invoked) on a volume whose file system supports
    ///  per-stream compression. The message contains a 16-bit
    ///  unsigned integer.The CompressionState element is as
    ///  follows.
    /// </summary>
    public partial struct FSCTL_SET_COMPRESSION_Request
    {

        /// <summary>
        ///  MUST be one of the following standard values.
        /// </summary>
        [StaticSize(2)]
        public FSCTL_SET_COMPRESSION_Request_CompressionState_Values CompressionState;
    }

    /// <summary>
    /// FSCTL_SET_COMPRESSION_Request_CompressionState_Values
    /// </summary>
    public enum FSCTL_SET_COMPRESSION_Request_CompressionState_Values : ushort
    {

        /// <summary>
        ///  The file or directory is not compressed.
        /// </summary>
        COMPRESSION_FORMAT_NONE = 0x0000,

        /// <summary>
        ///  The file or directory is compressed by using the default
        ///  compression algorithm.Equivalent to COMPRESSION_FORMAT_LZNT1.
        /// </summary>
        COMPRESSION_FORMAT_DEFAULT = 0x0001,

        /// <summary>
        ///  The file or directory is compressed by  using the LZNT1
        ///  compression algorithm. For more information, see [UASDC].
        /// </summary>
        COMPRESSION_FORMAT_LZNT1 = 0x0002,
    }

    /// <summary>
    ///  The FSCTL_SET_OBJECT_ID_EXTENDED request  message requests
    ///  that the server set the extended information for the
    ///  file or directory associated with the handle on which
    ///  this FSCTL was invoked. The message contains an EXTENDED_INFO
    ///  data element. The EXTENDED_INFO data element is defined
    ///  as follows.
    /// </summary>
    public partial struct FSCTL_SET_OBJECT_ID_EXTENDED_Request
    {

        /// <summary>
        ///  A 48-byte blob containing user-defined extended data
        ///  that was passed to this FSCTL by an application. In
        ///  this situation, the user refers to the developer who
        ///  is calling this FSCTL, meaning the extended info is
        ///  opaque to NTFS; there are no rules enforced by NTFS
        ///  as to what these last 48 bytes contain. Contrast this
        ///  with the first 16 bytes of an object ID, which can
        ///  be used to open the file, so NTFS requires that they
        ///  be unique within a volume.The Microsoft Distributed
        ///  Link Tracking Service uses the last 48 bytes of the
        ///  ExtendedInfo blob to store information that helps it
        ///  locate files that are moved to different volumes or
        ///  computers within a domain.  For more information, see
        ///  [MS-DLTW] section 3.1.6.
        /// </summary>
        [StaticSize(48)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    ///  The FSCTL_SET_ZERO_DATA request message requests that
    ///  the server fill the specified range of the file (associated
    ///  with the handle on which this FSCTL was invoked) with
    ///  zeroes. The message contains a FILE_ZERO_DATA_INFORMATION
    ///  element. The FILE_ZERO_DATA_INFORMATION element
    ///  is as follows.
    /// </summary>
    public partial struct FSCTL_SET_ZERO_DATA_Request
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file offset
        ///  of the start of the range to set to zeros in bytes.
        ///  The value of this field must be greater than or equal
        ///  to 0.
        /// </summary>
        [StaticSize(8)]
        public ulong FileOffset;

        /// <summary>
        ///A 64-bit signed integer that contains the byte
        ///  offset of the first byte beyond the last zeroed byte.
        ///  The value of this field must be greater than or equal
        ///  to 0.
        /// </summary>
        [StaticSize(8)]
        public ulong BeyondFinalZero;
    }

    /// <summary>
    ///  The FSCTL_SIS_COPYFILE request message requests that
    ///  the server copy the file associated with the handle
    ///  on which this FSCTL was invoked using the Microsoft
    ///  Single-Instance Storage (SIS) filter. The message contains
    ///  an SI_COPYFILE data element. For more information on
    ///  Single-Instance Storage, see [SIS].If the SIS
    ///  filter is installed on the server, it will attempt
    ///  to copy the source file to the destination file by
    ///  creating an SIS link instead of actually copying the
    ///  file data. If necessary and allowed, the source file
    ///  is placed under SIS control before the destination
    ///  file is created.The SI_COPYFILE data element is
    ///  as follows.
    /// </summary>
    public partial struct FSCTL_SIS_COPYFILE_Request
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the size in
        ///  bytes of the SourceFileName element, including a terminating
        ///  Unicode NULL character.
        /// </summary>
        [StaticSize(4)]
        public uint SourceFileNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the size in
        ///  bytes of the DestinationFileName element, including
        ///  a terminating Unicode NULL character.
        /// </summary>
        [StaticSize(4)]
        public uint DestinationFileNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains zero or more
        ///  of the following flag values. Flag values not specified
        ///  below SHOULD be set to 0, and MUST be ignored.
        /// </summary>
        [StaticSize(4)]
        public FSCTL_SIS_COPYFILE_Request_Flags_Values Flags;

        /// <summary>
        ///  A NULL-terminated Unicode string containing the source
        ///  file name.
        /// </summary>
        public ushort SourceFileName;

        /// <summary>
        ///  A NULL-terminated Unicode string containing the destination
        ///  file name.Both the source and destination file names
        ///  must represent paths on the same volume, and the file
        ///  names are the full paths to the files, including the
        ///  share or drive letter at which each file is located.
        /// </summary>
        public ushort DestinationFileName;
    }

    /// <summary>
    /// FSCTL_SIS_COPYFILE_Request_Flags_Values
    /// </summary>
    [Flags()]
    public enum FSCTL_SIS_COPYFILE_Request_Flags_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  If this flag is set, only create the destination file
        ///  if the source file is already under SIS control. If
        ///  the source file is not under SIS control, the FSCTL
        ///  returns STATUS_OBJECT_TYPE_MISMATCH.If this flag is
        ///  not specified, place the source file under SIS control
        ///  (if it is not already under SIS control), and create
        ///  the destination file.
        /// </summary>
        COPYFILE_SIS_LINK = 0x00000001,

        /// <summary>
        ///  If this flag is set, create the destination file if
        ///  it does not exist; if it does exist, overwrite it.
        /// 			If this flag is not specified, create the destination
        ///  file if it does not exist; if it does exist, the FSCTL
        ///  returns STATUS_OBJECT_NAME_COLLISION.
        /// </summary>
        COPYFILE_SIS_REPLACE = 0x00000002,
    }

    /// <summary>
    ///  The buffer alignment required by the underlying device.The
    ///  FILE_ATTRIBUTE_INFORMATION data element is as follows.
    /// </summary>
    public partial struct FileAlignmentInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that MUST contain one
        ///  of the following values.
        /// </summary>
        [StaticSize(4)]
        public AlignmentRequirement_Values AlignmentRequirement;
    }

    /// <summary>
    /// AlignmentRequirement_Values
    /// </summary>
    public enum AlignmentRequirement_Values : uint
    {

        /// <summary>
        ///  If this value is specified, there are no alignment requirements
        ///  for the device.
        /// </summary>
        FILE_BYTE_ALIGNMENT = 0x00000000,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 2-byte boundary.
        /// </summary>
        FILE_WORD_ALIGNMENT = 0x00000001,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 4-byte boundary.
        /// </summary>
        FILE_LONG_ALIGNMENT = 0x00000003,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  an 8-byte boundary.
        /// </summary>
        FILE_QUAD_ALIGNMENT = 0x00000007,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 16-byte boundary.
        /// </summary>
        FILE_OCTA_ALIGNMENT = 0x0000000f,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 32-byte boundary.
        /// </summary>
        FILE_32_BYTE_ALIGNMENT = 0x0000001f,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 64-byte boundary.
        /// </summary>
        FILE_64_BYTE_ALIGNMENT = 0x0000003f,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 128-byte boundary.
        /// </summary>
        FILE_128_BYTE_ALIGNMENT = 0x0000007f,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 256-byte boundary.
        /// </summary>
        FILE_256_BYTE_ALIGNMENT = 0x000000ff,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 512-byte boundary.
        /// </summary>
        FILE_512_BYTE_ALIGNMENT = 0x000001ff,
    }

    /// <summary>
    ///  This information class is used to query for the size
    ///  of the extended attributes (EA) for a file. An extended
    ///  attribute is a piece of application-specific metadata
    ///  that an application can associate with a file that
    ///  is not part of the file's data. For more information
    ///  on extended attributes, see [CIFS].The FILE_EA_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileEaInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that contains the combined
        ///  length in bytes of the extended attributes (EA) for
        ///  the file. 
        /// </summary>
        [StaticSize(4)]
        public uint EaSize;
    }

    /// <summary>
    ///  This information class is used to set end-of-file information
    ///  for a file. The FILE_END_OF_FILE_INFORMATION data
    ///  element is as follows.
    /// </summary>
    public partial struct FileEndOfFileInformation
    {

        /// <summary>
        ///A 64-bit signed integer that contains the absolute
        ///  new end of file position as a byte offset from the
        ///  start of the file. EndOfFile specifies the offset from
        ///  the beginning of the file of the byte following the
        ///  last byte in the file. That is, it is the offset from
        ///  the beginning of the file at which new bytes appended
        ///  to the file will be written. The value of this field
        ///  MUST be greater than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long EndOfFile;
    }

    /// <summary>
    ///  This information class is used to query or set extended
    ///  attribute (EA) information for a file. The FILE_FULL_EA_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileFullEaInformation
    {
        /// <summary>
        ///   A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_ FULL_EA _INFORMATION entry is located, if
        ///  multiple entries are present in the buffer. This member
        ///  MUST be zero if no other entries follow this one. An
        ///  implementation MUST use this value to determine the
        ///  location of the next entry (if multiple entries are
        ///  present in a buffer), and MUST NOT assume that the
        ///  value of NextEntryOffset is the same as the size of
        ///  the current entry.
        /// </summary>
        [StaticSize(4)]
        public uint NextEntryOffset;

        /// <summary>
        ///   An 8-bit unsigned integer that contains 0 or
        ///  the following flag value. A zero value indicates that
        ///  the file does not use EAs.
        /// </summary>
        public FileFullEaInformation_Flags_Values Flags;

        /// <summary>
        ///   An 8-bit unsigned integer that contains the length,
        ///  in bytes, of the EaName array. This value does not
        ///  include the null-terminator to EaName.
        /// </summary>
        public byte EaNameLength;

        /// <summary>
        ///   A 16-bit unsigned integer that contains the length,
        ///  in bytes, of each extended attribute value in the array.
        /// </summary>
        [StaticSize(2)]
        public ushort EaValueLength;

        /// <summary>
        /// An array of 8-bit ASCII characters that contains the extended attribute name followed by 
        /// a single terminating null character byte
        /// </summary>
        [Size("EaNameLength+1")]
        public byte[] EaName;

        /// <summary>
        /// An array of bytes that contains the extended attribute value. The length of this array is specified by the EaValueLength field
        /// </summary>
        [Size("EaValueLength")]
        public byte[] EaValue;
    }

    /// <summary>
    /// FileFullEaInformation_Flags_Values
    /// </summary>
    [Flags()]
    public enum FileFullEaInformation_Flags_Values : byte
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  If this flag is set, the file to which the EA belongs
        ///  cannot be interpreted without understanding the associated
        ///  extended attributes.
        /// </summary>
        FILE_NEED_EA = 0x00000080,
    }

    /// <summary>
    ///  This information class is used to query for the file
    ///  system's 8-byte file reference number for a file.The
    ///  FILE_INTERNAL_INFORMATION data element is as follows.
    /// </summary>
    public partial struct FileInternalInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the 8-byte
        ///  file reference number for the file. This number MUST
        ///  be assigned by the file system and is unique to the
        ///  volume on which the file or directory is located. This
        ///  file reference number is the same as the file reference
        ///  number that is stored in the FileId field of the FILE_ID_BOTH_DIR_INFORMATION
        ///  and FILE_ID_FULL_DIR_INFORMATION data elements. The
        ///  value of this field MUST be greater than or equal to
        ///  0.
        /// </summary>
        [StaticSize(8)]
        public long IndexNumber;
    }

    /// <summary>
    ///  This information class is used to create an NTFS hard
    ///  link to an existing file. The FILE_LINK_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileLinkInformation
    {

        /// <summary>
        ///  An 8-bit field that is set to 1 to indicate that
        ///  if the link already exists, it should be replaced with
        ///  the new link. MUST be set to 0 if the caller wants
        ///  the link creation operation to fail if the link already
        ///  exists.
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        ///  Reserved for alignment.
        /// </summary>
        [StaticSize(7)]
        public byte[] Reserved;

        /// <summary>
        ///  A 64-bit unsigned integer that contains the file
        ///  handle for the directory where the link is to be created.
        ///  For network operations, this value MUST be zero.
        /// </summary>
        [StaticSize(8)]
        public RootDirectory_Values RootDirectory;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length
        ///  in bytes of the FileName field.
        /// </summary>
        [StaticSize(4)]
        public uint FileNameLength;

        /// <summary>
        ///   A sequence of Unicode characters containing the
        ///  name to be assigned to the newly created link. This
        ///  field might not be NULL-terminated, and MUST be handled
        ///  as a sequence of FileNameLength bytes. If the RootDirectory
        ///  member is NULL, and the link is to be created in a
        ///  different directory from the file that is being linked
        ///  to, this member specifies the full path name for the
        ///  link to be created. Otherwise, it specifies only the
        ///  file name.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// RootDirectory_Values
    /// </summary>
    public enum RootDirectory_Values : ulong
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  This information class is used to query information
    ///  on a mailslot.The FILE_MAILSLOT_QUERY_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileMailslotQueryInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that contains the maximum
        ///  size of a single message that can be written to the
        ///  mailslot in bytes. To specify that the message can
        ///  be of any size, set this value to zero.
        /// </summary>
        [StaticSize(4)]
        public uint MaximumMessageSize;

        /// <summary>
        ///A 32-bit unsigned integer that contains the quota
        ///  in bytes for the mailslot. The mailslot quota specifies
        ///  the in-memory pool quota that is reserved for writes
        ///  to this mailslot.
        /// </summary>
        [StaticSize(4)]
        public uint MailslotQuota;

        /// <summary>
        ///A 32-bit unsigned integer that contains the next
        ///  message size in bytes.
        /// </summary>
        [StaticSize(4)]
        public uint NextMessageSize;

        /// <summary>
        ///A 32-bit unsigned integer that contains the total
        ///  number of messages waiting to be read from the mailslot.
        /// </summary>
        [StaticSize(4)]
        public uint MessagesAvailable;

        /// <summary>
        ///A 64-bit signed integer that contains the time
        ///  a read operation can wait for a message to be written
        ///  to the mailslot before a time-out occurs in milliseconds.
        ///  The value of this field MUST be (-1) or greater than
        ///  or equal to 0. A value of (-1) requests that the read
        ///  wait forever for a message, without timing out. A value
        ///  of 0 requests that the read not wait and return immediately
        ///  whether a pending message is available to be read or
        ///  not.
        /// </summary>
        [StaticSize(8)]
        public long ReadTimeout;
    }

    /// <summary>
    ///  This information class is used to set information on
    ///  a mailslot.The FILE_MAILSLOT_SET_INFORMATION data
    ///  element is as follows.
    /// </summary>
    public partial struct FileMailslotSetInformation
    {

        /// <summary>
        ///A 64-bit signed integer that contains the time
        ///  a read operation can wait for a message to be written
        ///  to the mailslot before a time-out occurs in milliseconds.
        ///  The value of this field MUST be (-1) or greater than
        ///  or equal to 0. A value of (-1) requests that the read
        ///  wait forever for a message without timing out. A value
        ///  of 0 requests that the read not wait and return immediately
        ///  whether a pending message is available to be read or
        ///  not.
        /// </summary>
        [StaticSize(8)]
        public long ReadTimeout;
    }

    /// <summary>
    ///  This information class is used to query for information
    ///  on a network file open. A network file open differs
    ///  from a file open in that the handle obtained from a
    ///  network file open can be used to look up attributes
    ///  using FileNetworkOpenInformation, but it cannot be
    ///  used for reads and writes to the file. The network
    ///  file open is an optimization of file open that returns
    ///  a file handle to the caller more quickly, but the file
    ///  handle it returns cannot be used in all of the ways
    ///  that a normal file handle can be used. The FILE_NETWORK_OPEN_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileNetworkOpenInformation
    {

        /// <summary>
        ///A 64-bit signed integer that contains the time
        ///  when the file was created in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME CreationTime;

        /// <summary>
        ///A 64-bit signed integer that contains the last
        ///  time the file was accessed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME LastAccessTime;

        /// <summary>
        ///A 64-bit signed integer that contains the last
        ///  time information was written to the file in the format
        ///  of a FILETIME structure. The value of this field MUST
        ///  be greater than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME LastWriteTime;

        /// <summary>
        ///A 64-bit signed integer that contains the last
        ///  time the file was changed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME ChangeTime;

        /// <summary>
        ///A 64-bit signed integer that contains the file
        ///  allocation size in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long AllocationSize;

        /// <summary>
        ///A 64-bit signed integer that contains the absolute
        ///  new end-of-file position as a byte offset from the
        ///  start of the file. EndOfFile specifies the offset to
        ///  the byte immediately following the last valid byte
        ///  in the file. Because this value is zero-based, it actually
        ///  refers to the first free byte in the file. That is,
        ///  it is the offset from the beginning of the file at
        ///  which new bytes appended to the file will be written.
        ///  The value of this field MUST be greater than or equal
        ///  to 0.
        /// </summary>
        [StaticSize(8)]
        public long EndOfFile;

        /// <summary>
        ///A 32-bit unsigned integer that contains the file
        ///  attributes. Valid attributes are as specified in section
        ///  .
        /// </summary>
        [StaticSize(4)]
        public File_Attributes FileAttributes;
    }

    /// <summary>
    ///  The first type of data that may be returned.
    /// </summary>
    public partial struct FileObjectIdInformation_Type_1
    {

        /// <summary>
        ///A 64-bit unsigned integer that contains the file
        ///  reference number for the file. NTFS generates this
        ///  number and assigns it to the file automatically when
        ///  the file is created. The file reference number is unique
        ///  within the volume on which the file exists.
        /// </summary>
        [StaticSize(8)]
        public ulong FileReferenceNumber;

        /// <summary>
        ///A 16-byte GUID that uniquely identifies the file
        ///  or directory within the volume on which it resides.
        ///  Specifically, the same object ID can be assigned to
        ///  another file or directory on a different volume, but
        ///  it MUST NOT be assigned to another file or directory
        ///  on the same volume.
        /// </summary>
        [StaticSize(16)]
        public System.Guid ObjectId;

        /// <summary>
        ///A 16-byte GUID that uniquely identifies the volume
        ///  on which the object resided when the object identifier
        ///  was created, or zero if the volume had no object identifier
        ///  at that time. After copy operations, move operations,
        ///  or other file operations, this may not be the same
        ///  as the object identifier of the volume on which the
        ///  object presently resides.
        /// </summary>
        [StaticSize(16)]
        public System.Guid BirthVolumeId;

        /// <summary>
        ///A 16-byte GUID value containing the object identifier
        ///  of the object at the time it was created. After copy
        ///  operations, move operations, or other file operations,
        ///  this value may not be the same as the ObjectId member
        ///  at present. When a file is moved or copied from one
        ///  volume to another, the ObjectId member's value changes
        ///  to a random unique value to avoid the potential for
        ///  ObjectId collisions because the object ID is not guaranteed
        ///  to be unique across volumes.
        /// </summary>
        [StaticSize(16)]
        public System.Guid BirthObjectId;

        /// <summary>
        ///A 16-byte GUID value containing the domain identifier.
        ///  This value is unused; it SHOULD be zero, and MUST be
        ///  ignored.
        /// </summary>
        [StaticSize(16)]
        public System.Guid DomainId;
    }

    /// <summary>
    ///  The second type of data that may be returned.
    /// </summary>
    public partial struct FileObjectIdInformation_Type_2
    {

        /// <summary>
        ///A 64-bit unsigned integer that contains the file
        ///  reference number for the file. NTFS generates this
        ///  number and assigns it to the file automatically when
        ///  the file is created. The file reference number is unique
        ///  within the volume on which the file exists.
        /// </summary>
        [StaticSize(8)]
        public ulong FileReferenceNumber;

        /// <summary>
        ///A 16-byte GUID that uniquely identifies the file
        ///  or directory within the volume on which it resides.
        ///  Specifically, the same object ID can be assigned to
        ///  another file or directory on a different volume, but
        ///  it MUST NOT be assigned to another file or directory
        ///  on the same volume.
        /// </summary>
        [StaticSize(16)]
        public System.Guid ObjectId;

        /// <summary>
        ///A 48-byte blob that contains application-specific
        ///  extended information on the file object. If no extended
        ///  information has been written for this file, the server
        ///  MUST return 48 bytes of 0x00 in this field.
        /// </summary>
        [StaticSize(48)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    ///  This information class is used to query or set information
    ///  on a named pipe that is not specific to one end of
    ///  the pipe or another.The FILE_PIPE_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FilePipeInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that MUST contain one
        ///  of the following values.
        /// </summary>
        [StaticSize(4)]
        public ReadMode_Values ReadMode;

        /// <summary>
        ///A 32-bit unsigned integer that MUST contain one
        ///  of the following values.
        /// </summary>
        [StaticSize(4)]
        public CompletionMode_Values CompletionMode;
    }

    /// <summary>
    /// ReadMode_Values
    /// </summary>
    public enum ReadMode_Values : uint
    {

        /// <summary>
        ///  If this value is specified, data MUST be read from the
        ///  pipe as a stream of bytes.
        /// </summary>
        FILE_PIPE_BYTE_STREAM_MODE = 0x00000000,

        /// <summary>
        ///  If this value is specified, data MUST be read from the
        ///  pipe as a stream of messages.
        /// </summary>
        FILE_PIPE_MESSAGE_MODE = 0x00000001,
    }

    /// <summary>
    /// CompletionMode_Values
    /// </summary>
    public enum CompletionMode_Values : uint
    {

        /// <summary>
        ///  If this value is specified, blocking mode MUST be enabled.
        ///  When the pipe is being connected, read to, or written
        ///  from, the operation is not completed until there is
        ///  data to read, all data is written, or a client is connected.
        ///  Use of this mode can mean waiting indefinitely in some
        ///  situations for a client process to perform an action.
        /// </summary>
        FILE_PIPE_QUEUE_OPERATION = 0x00000000,

        /// <summary>
        ///  If this value is specified, non-blocking mode MUST be
        ///  enabled. When the pipe is being connected, read to,
        ///  or written from, the operation completes immediately.
        /// </summary>
        FILE_PIPE_COMPLETE_OPERATION = 0x00000001,
    }

    /// <summary>
    ///  This information class is used to query information
    ///  on a named pipe that is associated with the end of
    ///  the pipe that is being queried.The FILE_PIPE_LOCAL_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FilePipeLocalInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that contains the named
        ///  pipe type. MUST be one of the following.
        /// </summary>
        [StaticSize(4)]
        public NamedPipeType_Values NamedPipeType;

        /// <summary>
        ///A 32-bit unsigned integer that contains the named
        ///  pipe configuration. MUST be one of the following. 
        /// 
        /// </summary>
        [StaticSize(4)]
        public NamedPipeConfiguration_Values NamedPipeConfiguration;

        /// <summary>
        ///A 32-bit unsigned integer that contains the maximum
        ///  number of instances that can be created for this pipe.
        ///  The first instance of the pipe MUST specify this value.
        /// </summary>
        [StaticSize(4)]
        public uint MaximumInstances;

        /// <summary>
        ///A 32-bit unsigned integer that contains the number
        ///  of current named pipe instances.
        /// </summary>
        [StaticSize(4)]
        public uint CurrentInstances;

        /// <summary>
        ///A 32-bit unsigned integer that contains the inbound
        ///  quota in bytes for the named pipe.
        /// </summary>
        [StaticSize(4)]
        public uint InboundQuota;

        /// <summary>
        ///A 32-bit unsigned integer that contains the bytes
        ///  of data available to be read from the named pipe.
        /// </summary>
        [StaticSize(4)]
        public uint ReadDataAvailable;

        /// <summary>
        ///A 32-bit unsigned integer that contains outbound
        ///  quota in bytes for the named pipe.
        /// </summary>
        [StaticSize(4)]
        public uint OutboundQuota;

        /// <summary>
        ///A 32-bit unsigned integer that contains the write
        ///  quota in bytes for the named pipe.
        /// </summary>
        [StaticSize(4)]
        public uint WriteQuotaAvailable;

        /// <summary>
        ///A 32-bit unsigned integer that contains the named
        ///  pipe state that specifies the connection status for
        ///  the named pipe. MUST be one of the following.
        /// </summary>
        [StaticSize(4)]
        public NamedPipeState_Values NamedPipeState;

        /// <summary>
        ///A 32-bit unsigned integer that contains the type
        ///  of the named pipe end, which specifies whether this
        ///  is the client or the server side of a named pipe. MUST
        ///  be one of the following.
        /// </summary>
        [StaticSize(4)]
        public NamedPipeEnd_Values NamedPipeEnd;
    }

    /// <summary>
    /// NamedPipeType_Values
    /// </summary>
    public enum NamedPipeType_Values : uint
    {

        /// <summary>
        ///  If this value is specified, data MUST be read from the
        ///  pipe as a stream of bytes.
        /// </summary>
        FILE_PIPE_BYTE_STREAM_TYPE = 0x00000000,

        /// <summary>
        ///  If this flag is specified, data MUST be read from the
        ///  pipe as a stream of messages.
        /// </summary>
        FILE_PIPE_MESSAGE_TYPE = 0x00000001,
    }

    /// <summary>
    /// NamedPipeConfiguration_Values
    /// </summary>
    public enum NamedPipeConfiguration_Values : uint
    {

        /// <summary>
        ///  If this value is specified, the flow of data in the
        ///  pipe goes from client to server only.
        /// </summary>
        FILE_PIPE_INBOUND = 0x00000000,

        /// <summary>
        ///  If this value is specified, the flow of data in the
        ///  pipe goes from server to client only.
        /// </summary>
        FILE_PIPE_OUTBOUND = 0x00000001,

        /// <summary>
        ///  If this value is specified, the pipe is bi-directional;
        ///  both server and client processes can read from and
        ///  write to the pipe.
        /// </summary>
        FILE_PIPE_FULL_DUPLEX = 0x00000002,
    }

    /// <summary>
    /// NamedPipeState_Values
    /// </summary>
    public enum NamedPipeState_Values : uint
    {
        /// <summary>
        ///  Named pipe is disconnected.
        /// </summary>
        FILE_PIPE_DISCONNECTED_STATE = 0x00000001,

        /// <summary>
        ///  Named pipe is waiting to establish a connection.
        /// </summary>
        FILE_PIPE_LISTENING_STATE = 0x00000002,

        /// <summary>
        ///  Named pipe is connected.
        /// </summary>
        FILE_PIPE_CONNECTED_STATE = 0x00000003,

        /// <summary>
        ///  Named pipe is in the process of being closed.
        /// </summary>
        FILE_PIPE_CLOSING_STATE = 0x00000004,
    }

    /// <summary>
    /// NamedPipeEnd_Values
    /// </summary>
    public enum NamedPipeEnd_Values : uint
    {

        /// <summary>
        ///  This is the client end of a named pipe.
        /// </summary>
        FILE_PIPE_CLIENT_END = 0x00000000,

        /// <summary>
        ///  This is the server end of a named pipe.
        /// </summary>
        FILE_PIPE_SERVER_END = 0x00000001,
    }

    /// <summary>
    ///  The information class is used to query or set quota
    ///  information.The FILE_QUOTA_INFORMATION data element
    ///  is as follows.
    /// </summary>
    public partial struct FileQuotaInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_QUOTA_INFORMATION entry is located, if multiple
        ///  entries are present in a buffer. This member MUST be
        ///  zero if no other entries follow this one. An implementation
        ///  MUST use this value to determine the location of the
        ///  next entry (if multiple entries are present in a buffer),
        ///  and MUST NOT assume that the value of NextEntryOffset
        ///  is the same as the size of the current entry.
        /// </summary>
        [StaticSize(4)]
        public uint NextEntryOffset;

        /// <summary>
        ///A 32-bit unsigned integer that contains the length
        ///  in bytes of the SID data element.
        /// </summary>
        [StaticSize(4)]
        public uint SidLength;

        /// <summary>
        ///A 64-bit signed integer that contains the last
        ///  time the quota was changed in the format of a FILETIME
        ///  structure. This value MUST be greater than or equal
        ///  to 0.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME ChangeTime;

        /// <summary>
        ///A 64-bit signed integer that contains the amount
        ///  of quota used by this user in bytes. This value MUST
        ///  be greater than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long QuotaUsed;

        /// <summary>
        ///A 64-bit signed integer that contains the disk
        ///  quota warning threshold in bytes on this volume for
        ///  this user. This field MUST be set to a 64-bit integer
        ///  value greater than or equal to 0 to set a quota warning
        ///  threshold for this user on this volume, or to (-1)
        ///  to specify that no quota warning threshold is set for
        ///  this user.
        /// </summary>
        [StaticSize(8)]
        public long QuotaThreshold;

        /// <summary>
        ///A 64-bit signed integer that contains the disk
        ///  quota limit in bytes on this volume for this user.
        ///  This field MUST be set to a 64-bit integer value greater
        ///  than or equal to 0 to set a disk quota limit for this
        ///  user on this volume, or to (-1) to specify that no
        ///  quota limit is set for this user.
        /// </summary>
        [StaticSize(8)]
        public long QuotaLimit;

        /// <summary>
        ///  Security identifier (SID) for this user.
        /// </summary>
        public _SID Sid;
    }

    /// <summary>
    ///  This information class is used to rename a file.The
    ///  FILE_RENAME_INFORMATION data element is as follows.
    /// </summary>
    public partial struct FileRenameInformation
    {

        /// <summary>
        ///MUST be an 8-bit field that is set to 1 to indicate
        ///  that if a file with the given name already exists,
        ///  it SHOULD be replaced with the given file. If set to
        ///  0, the rename operation MUST fail if a file with the
        ///  given name already exists.
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        ///  Reserved area for alignment.
        /// </summary>
        [StaticSize(7)]
        public byte[] Reserved;

        /// <summary>
        ///A 64-bit unsigned integer that contains the file
        ///  handle for the root directory. For network operations,
        ///  this value MUST always be zero.
        /// </summary>
        [StaticSize(8)]
        public FileRenameInformation_RootDirectory_Values RootDirectory;

        /// <summary>
        ///A 32-bit unsigned integer that contains the length
        ///  in bytes of the new name for the file, including the
        ///  trailing NULL if present.
        /// </summary>
        [StaticSize(4)]
        public uint FileNameLength;

        /// <summary>
        ///A sequence of Unicode characters containing the
        ///  file name. This field MAY NOT be NULL-terminated, and
        ///  MUST be handled as a sequence of                  FileNameLength
        ///  bytes, not as a NULL-terminated string. If the RootDirectory
        ///  member is NULL, and the file is being moved to a different
        ///  directory, this member MUST specify the full path name
        ///  to be assigned to the file. Otherwise, it MUST specify
        ///  only the file name or a relative path name.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// FileRenameInformation_RootDirectory_Values
    /// </summary>
    public enum FileRenameInformation_RootDirectory_Values : ulong
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  This information class is used to query for information
    ///  on a reparse point.The FILE_REPARSE_POINT_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileReparsePointInformation
    {

        /// <summary>
        ///A 64-bit signed integer that contains the file
        ///  reference number for the file. NTFS generates this
        ///  number and assigns it to the file automatically when
        ///  the file is created. The value of this field MUST be
        ///  greater than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long FileReferenceNumber;

        /// <summary>
        ///  A 	32-bit unsigned integer value containing the
        ///  reparse point tag that uniquely identifies the owner
        ///  of the reparse point.
        /// </summary>
        [StaticSize(4)]
        public uint Tag;
    }

    /// <summary>
    ///  This information class is used to query or change the
    ///  file's short name.The FILE_NAME_INFORMATION data
    ///  element is as follows.
    /// </summary>
    public partial struct FileShortNameInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that contains the length
        ///  in bytes of the FileName field.
        /// </summary>
        [StaticSize(4)]
        public uint FileNameLength;

        /// <summary>
        ///A sequence of Unicode characters containing the
        ///  file name. This field MUST NOT begin with a path separator
        ///  character (backslash). This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    ///  This information class is used to query or set file
    ///  information.The FILE_STANDARD_INFORMATION data
    ///  element is as follows.
    /// </summary>
    public partial struct FileStandardInformation
    {

        /// <summary>
        ///A 64-bit signed integer that contains the file
        ///  allocation size in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long AllocationSize;

        /// <summary>
        ///A 64-bit signed integer that contains the absolute
        ///  new end-of-file position as a byte offset from the
        ///  start of the file. EndOfFile specifies the offset to
        ///  the byte immediately following the last valid byte
        ///  in the file. Because this value is zero-based, it actually
        ///  refers to the first free byte in the file. That is,
        ///  it is the offset from the beginning of the file at
        ///  which new bytes appended to the file will be written.
        ///  The value of this field MUST be greater than or equal
        ///  to 0.
        /// </summary>
        [StaticSize(8)]
        public long EndOfFile;

        /// <summary>
        ///A 32-bit unsigned integer that contains the number
        ///  of non-deleted links to this file.
        /// </summary>
        [StaticSize(4)]
        public uint NumberOfLinks;

        /// <summary>
        ///MUST be an 8-bit field that MUST be set to 1
        ///  to indicate that a file deletion has been requested;
        ///  otherwise, 0.
        /// </summary>
        public byte DeletePending;

        /// <summary>
        ///MUST be an 8-bit field that MUST be set to 1
        ///  to indicate that the file is a directory; otherwise,
        ///  0.
        /// </summary>
        public byte Directory;
    }

    /// <summary>
    ///  This information class is used to query alternate name
    ///  information for a file. The alternate name for a file
    ///  is its 8.3 format name (8 characters that appear before
    ///  the "." and 3 characters that appear after). This name
    ///  exists for compatibility with MS-DOS and 16-bit versions
    ///  of , which did not support longer file names or spaces
    ///  within file names. A file MAY have an alternate name
    ///  if its full name is not compliant with the restrictions
    ///  for file names under MS-DOS and 16-bit .NTFS assigns
    ///  an alternate name to a file whose full name is not
    ///  compliant with restrictions for file names under MS-DOS
    ///  and 16-bit  unless the system has been configured through
    ///  a registry entry to not generate these names to improve
    ///  performance.The FILE_NAME_INFORMATION data element
    ///  is as follows.
    /// </summary>
    public partial struct FileAlternateNameInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length in
        ///  bytes of the FileName member.
        /// </summary>
        [StaticSize(4)]
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the file
        ///  name. This field might not be NULL-terminated, and
        ///  MUST be handled as a sequence of FileNameLength bytes.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    ///  This information class is used to enumerate the streams
    ///  for a file. The FILE_STREAM_INFORMATION data element
    ///  is as follows.
    /// </summary>
    public partial struct FileStreamInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_ STREAM _INFORMATION entry is located, if
        ///  multiple entries are present in a buffer. This member
        ///  is zero if no other entries follow this one. An implementation
        ///  MUST use this value to determine the location of the
        ///  next entry (if multiple entries are present in a buffer),
        ///  and MUST NOT assume that the value of NextEntryOffset
        ///  is the same as the size of the current entry.
        /// </summary>
        [StaticSize(4)]
        public uint NextEntryOffset;

        /// <summary>
        ///A 32-bit unsigned integer that contains the length
        ///  in bytes of the stream name string.
        /// </summary>
        [StaticSize(4)]
        public uint StreamNameLength;

        /// <summary>
        ///A 64-bit unsigned integer that contains the size
        ///  in bytes of the stream.
        /// </summary>
        [StaticSize(8)]
        public ulong StreamSize;

        /// <summary>
        ///A 64-bit signed integer that contains the file
        ///  stream allocation size in bytes. Usually, this value
        ///  is a multiple of the sector or cluster size of the
        ///  underlying physical device. The value of this field
        ///  MUST be greater than or equal to 0.
        /// </summary>
        [StaticSize(8)]
        public long StreamAllocationSize;

        /// <summary>
        ///A sequence of Unicode characters containing the
        ///  name of the stream. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of StreamNameLength
        ///  bytes.
        /// </summary>
        [Size("StreamNameLength")]
        public byte[] StreamName;
    }

    /// <summary>
    ///  This information class is used to set the valid data
    ///  length information for a file. The FILE_VALID_DATA_LENGTH_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileValidDataLengthInformation
    {

        /// <summary>
        ///A 64-bit signed integer that contains the new
        ///  valid data length for the file. This parameter MUST
        ///  be a positive value that is greater than the current
        ///  valid data length, but less than or equal to the current
        ///  file size.The FILE_VALID_DATA_LENGTH_INFORMATION structure
        ///  is used to set a new valid data length for a file on
        ///  an NTFS volume. A file's valid data length is the length
        ///  in bytes of the data that has been written to the file.
        ///  This valid data extends from the beginning of the file
        ///  to the last byte in the file that has not been zeroed
        ///  or left uninitialized.
        /// </summary>
        [StaticSize(8)]
        public long FileNameLength;
    }

    /// <summary>
    ///  The information class is used to provide the list of
    ///  Security identifiers (SID) for which query quota information
    ///  is requested.
    /// </summary>
    public partial struct FileGetQuotaInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte offset
        ///  from the beginning of this entry, at which the next
        ///  FILE_GET_QUOTA_INFORMATION entry is located, if multiple
        ///  entries are present in a buffer. This member MUST be
        ///  zero if no other entries follow this one. An implementation
        ///  MUST use this value to determine the location of the
        ///  next entry (if multiple entries are present in a buffer),
        ///  and MUST NOT assume that the value of NextEntryOffset
        ///  is the same as the size of the current entry.
        /// </summary>
        [StaticSize(4)]
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length in
        ///  bytes of the Sid data element.
        /// </summary>
        [StaticSize(4)]
        public uint SidLength;

        /// <summary>
        ///  SID for this user. SID(s) are sent in little endian format
        ///  and require no packing. The format of a SID is as specified
        ///  in [MS-DTYP] section 2.4.2 
        /// </summary>
        public _SID Sid;
    }

    /// <summary>
    ///  This information class is used to query for attribute
    ///  and reparse tag information for a file. The FILE_ATTRIBUTE_TAG_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileAttributeTagInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file attributes.
        ///  Valid file attributes are as specified in section.
        /// </summary>
        [StaticSize(4)]
        public File_Attributes FileAttributes;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the reparse
        ///  point tag. If the FileAttributes member includes the
        ///  FILE_ATTRIBUTE_REPARSE_POINT attribute flag, this member
        ///  specifies the reparse tag. Otherwise, this member SHOULD
        ///  be set to 0, and MUST be ignored.
        /// </summary>
        [StaticSize(4)]
        public uint ReparseTag;
    }

    /// <summary>
    ///  This information class is used to query or set file
    ///  information.The FILE_BASIC_INFORMATION data element
    ///  is as follows.
    /// </summary>
    public partial struct FileBasicInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the time when
        ///  the file was created. All dates and times in this message
        ///  are in absolute system-time format, which is represented
        ///  as a FILETIME structure. This field should be set to
        ///  an integer value greater than or equal to 0; alternately,
        ///  it can be set to (-1) to indicate that this time field
        ///  should not be updated by the server.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  the file was accessed in the format of a FILETIME structure.
        ///  This field should be set to an integer value greater
        ///  than or equal to 0; alternately, it can be set to (-1)
        ///  to indicate that this time field should not be updated
        ///  by the server.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  information was written to the file in the format of
        ///  a FILETIME structure. This field should be set to an
        ///  integer value greater than or equal to 0; alternately,
        ///  it can be set to (-1) to indicate that this time field
        ///  should not be updated by the server.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME LastWriteTime;

        /// <summary>
        ///   A 64-bit signed integer that contains the last time
        ///  the file was changed in the format of a FILETIME structure.
        ///  This field should be set to an integer value greater
        ///  than or equal to 0; alternately, it can be set to (-1)
        ///  to indicate that this time field should not be updated
        ///  by the server.
        /// </summary>
        [StaticSize(8)]
        public _FILETIME ChangeTime;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file attributes.
        ///  Valid file attributes are specified in section.The
        ///  file system updates the values of the LastAccessTime,
        ///  LastWriteTime, and ChangeTime members as appropriate
        ///  after an I/O operation is performed on a file. However,
        ///  a driver or application can request that the file system
        ///  not update one or more of these members for I/O operations
        ///  that are performed on the caller's file handle by setting
        ///  the appropriate members to -1. The caller can set one,
        ///  all, or any other combination of these three members
        ///  to -1. Only the members that are set to -1 will be
        ///  unaffected by I/O operations on the file handle; the
        ///  other members will be updated as appropriate.
        /// </summary>
        [StaticSize(4)]
        public File_Attributes FileAttributes;

        /// <summary>
        /// Reserved
        /// </summary>
        [StaticSize(4)]
        public uint Reserved;
    }

    /// <summary>
    /// CompressionFormat_Values
    /// </summary>
    public enum CompressionFormat_Values : ushort
    {

        /// <summary>
        ///  The file or directory is not compressed.
        /// </summary>
        COMPRESSION_FORMAT_NONE = 0x0000,

        /// <summary>
        ///  The file or directory is compressed by using the default
        ///  compression algorithm.
        /// </summary>
        COMPRESSION_FORMAT_DEFAULT = 0x0001,

        /// <summary>
        ///  The file or directory is compressed by using the LZNT1
        ///  compression algorithm.
        /// </summary>
        COMPRESSION_FORMAT_LZNT1 = 0x0002,
    }

    /// <summary>
    ///  This information class is used to mark a file for deletion.
    /// The FILE_DISPOSITION_INFORMATION data element is
    ///  as follows.
    /// </summary>
    public partial struct FileDispositionInformation
    {

        /// <summary>
        ///An 8-bit field that is set to 1 to indicate that
        ///  a file SHOULD be deleted when it is closed; otherwise,
        ///  0.A file marked for deletion is not actually deleted
        ///  until all open handles for the file object have been
        ///  closed, and the link count for the file is zero.
        /// </summary>
        public byte DeletePending;
    }

    /// <summary>
    ///  This information class is used to query detailed information
    ///  of a file. The FILE_NAME_INFORMATION data element
    ///  is as follows.
    /// </summary>
    public partial struct FileNameInformation
    {

        /// <summary>
        ///A 32-bit unsigned integer that contains the length
        ///  in bytes of the FileName field.
        /// </summary>
        [StaticSize(4)]
        public uint FileNameLength;

        /// <summary>
        ///A sequence of Unicode characters containing the
        ///  file name. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    ///  This information class is used to query attribute information
    ///  for a file system. The message contains a FILE_FS_ATTRIBUTE_INFORMATION
    ///  data element. The FILE_FS_ATTRIBUTE_INFORMATION
    ///  data element is as follows.
    /// </summary>
    public partial struct FileFsAttributeInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains a bitmask of
        ///  flags that specify attributes of the specified file
        ///  system as a combination of the following flags. The
        ///  value of this field MUST be a bitwise OR of zero or
        ///  more of the following with the exception that FS_FILE_COMPRESSION
        ///  and FS_VOL_IS_COMPRESSED cannot both be set. Any flag
        ///  values not explicitly mentioned here MAY be set to
        ///  any value, and MUST be ignored.
        /// </summary>
        [StaticSize(4)]
        public FileSystemAttributes_Values FileSystemAttributes;

        /// <summary>
        ///  A 32-bit signed integer that contains the maximum file
        ///  name component length in bytes supported by the specified
        ///  file system. The value of this field MUST be greater
        ///  than 0.
        /// </summary>
        [StaticSize(4)]
        public int MaximumComponentNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length in
        ///  bytes of the file system name in the FileSystemName
        ///  field. The value of this field MUST be greater than
        ///  0.
        /// </summary>
        [StaticSize(4)]
        public uint FileSystemNameLength;

        /// <summary>
        ///  A variable-length Unicode field containing the name
        ///  of the file system. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileSystemNameLength
        ///  bytes.
        /// </summary>
        [Size("FileSystemNameLength")]
        public byte[] FileSystemName;
    }

    /// <summary>
    /// FileSystemAttributes_Values
    /// </summary>
    [Flags()]
    public enum FileSystemAttributes_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  The file system preserves the case of file names when
        ///  it places a name on disk.
        /// </summary>
        FILE_CASE_PRESERVED_NAMES = 0x00000002,

        /// <summary>
        ///  The file system supports case-sensitive file names when
        ///  looking up (searching for) file names in a directory.
        /// </summary>
        FILE_CASE_SENSITIVE_SEARCH = 0x00000001,

        /// <summary>
        ///  The file volume supports file-based compression. This
        ///  flag is incompatible with the FILE_VOLUME_IS_COMPRESSED
        ///  flag.
        /// </summary>
        FILE_FILE_COMPRESSION = 0x00000010,

        /// <summary>
        ///  The file system supports named streams.
        /// </summary>
        FILE_NAMED_STREAMS = 0x00040000,

        /// <summary>
        ///  The file system preserves and enforces access control
        ///  lists (ACLs).
        /// </summary>
        FILE_PERSISTENT_ACLS = 0x00000008,

        /// <summary>
        ///  The specified volume is read-only. This attribute is
        ///  only available on , , and .
        /// </summary>
        FILE_READ_ONLY_VOLUME = 0x00080000,

        /// <summary>
        ///  The file system supports the Encrypted File System (EFS).
        ///  NTFS version 2 supports EFS. To use EFS, the operating
        ///  system must support NTFS version 2, and the file system
        ///  on disk must be formatted using NTFS version 2.NTFS
        ///  version 2 is supported on , , , and .
        /// </summary>
        FILE_SUPPORTS_ENCRYPTION = 0x00020000,

        /// <summary>
        ///  The file system supports object identifiers.
        /// </summary>
        FILE_SUPPORTS_OBJECT_IDS = 0x00010000,

        /// <summary>
        ///  The file system supports remote storage.
        /// </summary>
        FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100,

        /// <summary>
        ///  The file system supports reparse points.
        /// </summary>
        FILE_SUPPORTS_REPARSE_POINTS = 0x00000080,

        /// <summary>
        ///  The file system supports sparse files.
        /// </summary>
        FILE_SUPPORTS_SPARSE_FILES = 0x00000040,

        /// <summary>
        ///  The file system supports Unicode in file and directory
        ///  names. This flag applies only to file and directory
        ///  names; the file system neither restricts nor interprets
        ///  the bytes of data within a file.
        /// </summary>
        FILE_UNICODE_ON_DISK = 0x00000004,

        /// <summary>
        ///  The specified volume is a compressed volume. This flag
        ///  is incompatible with the FILE_FILE_COMPRESSION flag.
        /// </summary>
        FILE_VOLUME_IS_COMPRESSED = 0x00008000,

        /// <summary>
        ///  The file system supports per-user quotas.
        /// </summary>
        FILE_VOLUME_QUOTAS = 0x00000020,

        /// <summary>
        /// The file system supports sharing logical clusters between files on the same volume.
        /// </summary>
        FILE_SUPPORTS_BLOCK_REFCOUNTING = 0x08000000,
    }

    /// <summary>
    /// FileSystemControlFlags_Values
    /// </summary>
    [Flags()]
    public enum FileSystemControlFlags_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  Content indexing is disabled.
        /// </summary>
        FILE_VC_CONTENT_INDEX_DISABLED = 0x00000008,

        /// <summary>
        ///  An event log entry will be created when the user exceeds
        ///  his or her assigned disk quota limit.
        /// </summary>
        FILE_VC_LOG_QUOTA_LIMIT = 0x00000020,

        /// <summary>
        ///  An event log entry will be created when the user exceeds
        ///  his or her assigned quota warning threshold.
        /// </summary>
        FILE_VC_LOG_QUOTA_THRESHOLD = 0x00000010,

        /// <summary>
        ///  An event log entry will be created when the volume's
        ///  free space limit is exceeded.
        /// </summary>
        FILE_VC_LOG_VOLUME_LIMIT = 0x00000080,

        /// <summary>
        ///  An event log entry will be created when the volume's
        ///  free space threshold is exceeded.
        /// </summary>
        FILE_VC_LOG_VOLUME_THRESHOLD = 0x00000040,

        /// <summary>
        ///  Quotas are tracked and enforced on the volume.
        /// </summary>
        FILE_VC_QUOTA_ENFORCE = 0x00000002,

        /// <summary>
        ///  Quotas are tracked on the volume, but they are not enforced.
        ///  Tracked quotas enable reporting on the file system
        ///  space used by system users. If both this field and
        ///  FILE_VC_QUOTA_ENFORCE are specified, FILE_VC_QUOTA_ENFORCE
        ///  is ignored.
        /// </summary>
        FILE_VC_QUOTA_TRACK = 0x00000001,

        /// <summary>
        ///  The quota information for the volume is incomplete because
        ///  it is corrupt, or the system is in the process of rebuilding
        ///  the quota information.
        /// </summary>
        FILE_VC_QUOTAS_INCOMPLETE = 0x00000100,

        /// <summary>
        ///  The file system is rebuilding the quota information
        ///  for the volume.
        /// </summary>
        FILE_VC_QUOTAS_REBUILDING = 0x00000200,
    }

    /// <summary>
    /// Characteristics_Values
    /// </summary>
    [Flags()]
    public enum Characteristics_Values : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_REMOVABLE_MEDIA = 0x00000001,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_READ_ONLY_DEVICE = 0x00000002,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_FLOPPY_DISKETTE = 0x00000004,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_WRITE_ONCE_MEDIA = 0x00000008,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_REMOTE_DEVICE = 0x00000010,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_DEVICE_IS_MOUNTED = 0x00000020,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_VIRTUAL_VOLUME = 0x00000040,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_AUTOGENERATED_DEVICE_NAME = 0x00000080,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_DEVICE_SECURE_OPEN = 0x00000100,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_CHARACTERISTIC_PNP_DEVICE = 0x00000800,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_CHARACTERISTIC_TS_DEVICE = 0x00001000,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_CHARACTERISTIC_WEBDAV_DEVICE = 0x00002000,
    }

    /// <summary>
    ///  The FSCTL_GET_NTFS_VOLUME_DATA reply message returns
    ///  the results of the FSCTL_GET_NTFS_VOLUME_DATA request
    ///  as an NTFS_VOLUME_DATA_BUFFER element. The NTFS_VOLUME_DATA_BUFFER
    ///  contains information on a volume.  For more information
    ///  on the NTFS file system, see [MSFT-NTFS]. 
    /// </summary>
    public partial struct NTFS_VOLUME_DATA_BUFFER
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the serial number
        ///  of the volume. This is a unique number assigned to
        ///  the volume media by the operating system when the volume
        ///  is formatted.
        /// </summary>
        [StaticSize(8)]
        public ulong VolumeSerialNumber;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  sectors in the specified volume.
        /// </summary>
        [StaticSize(8)]
        public ulong NumberSectors;

        /// <summary>
        ///  A 64-bit signed integer that contains the total number
        ///  of clusters in the specified volume.
        /// </summary>
        [StaticSize(8)]
        public ulong TotalClusters;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  free clusters in the specified volume.
        /// </summary>
        [StaticSize(8)]
        public ulong FreeClusters;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  reserved clusters in the specified volume. Reserved
        ///  clusters are free clusters reserved for when the volume
        ///  becomes full. Reserved clusters are released when either
        ///  the master file table grows beyond its allocated space
        ///  (the volume has a large number of small files) or the
        ///  volume becomes full (the volume has a small number
        ///  of large files).
        /// </summary>
        [StaticSize(8)]
        public ulong TotalReserved;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a sector on the specified volume.
        /// </summary>
        [StaticSize(4)]
        public uint BytesPerSector;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a cluster on the specified volume. This value
        ///  is also known as the cluster factor.
        /// </summary>
        [StaticSize(4)]
        public uint BytesPerCluster;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a file record segment.
        /// </summary>
        [StaticSize(4)]
        public uint BytesPerFileRecordSegment;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  clusters in a file record segment.
        /// </summary>
        [StaticSize(4)]
        public uint ClustersPerFileRecordSegment;

        /// <summary>
        ///  A 64-bit signed integer that contains the size of the
        ///  master file table in bytes.
        /// </summary>
        [StaticSize(8)]
        public ulong MftValidDataLength;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table.
        /// </summary>
        [StaticSize(8)]
        public ulong MftStartLcn;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table mirror.
        /// </summary>
        [StaticSize(8)]
        public ulong Mft2StartLcn;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table zone.
        /// </summary>
        [StaticSize(8)]
        public ulong MftZoneStart;

        /// <summary>
        ///  A 64-bit signed integer that contains the ending logical
        ///  cluster number of the master file table zone.
        /// </summary>
        [StaticSize(8)]
        public ulong MftZoneEnd;
    }

    /// <summary>
    /// TimeoutSpecified_Values
    /// </summary>
    public enum TimeoutSpecified_Values : byte
    {

        /// <summary>
        ///  Indicates that the server MUST wait forever (no timeout)
        ///  for the named pipe. Any value in Timeout MUST be ignored.
        /// </summary>
        FALSE = 0,

        /// <summary>
        ///  Indicates that the server MUST use the value in the
        ///  Timeout parameter.
        /// </summary>
        TRUE = 1,
    }

    /// <summary>
    /// The following attributes are defined for files and directories. They can be used in any combination 
    /// unless noted in the description of the attribute's meaning. There is no file attribute with the value 
    /// 0x00000000 because a value of 0x00000000 in the FileAttributes field means that the file 
    /// attributes for this file MUST NOT be changed when setting basic information for the file.  
    /// </summary>
    [Flags()]
    public enum File_Attributes : uint
    {
        /// <summary>
        /// All bits are not set.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// A file or directory that needs to be archived. Applications 
        /// use this attribute to mark files for backup or removal. 
        /// </summary>
        FILE_ATTRIBUTE_ARCHIVE = 0x00000020,

        /// <summary>
        /// A file or directory that is compressed. For a file, all of the 
        /// data in the file is compressed. For a directory, compression 
        /// is the default for newly created files and subdirectories.  
        /// </summary>
        FILE_ATTRIBUTE_COMPRESSED = 0x00000800,

        /// <summary>
        /// This item is a directory. 
        /// </summary>
        FILE_ATTRIBUTE_DIRECTORY = 0x00000010,

        /// <summary>
        /// A file or directory that is encrypted. For a file, all data 
        /// streams in the file are encrypted. For a directory, encryption
        /// is the default for newly created files and subdirectories.  
        /// </summary>
        FILE_ATTRIBUTE_ENCRYPTED = 0x00004000,

        /// <summary>
        /// A file or directory that is hidden. Files and directories 
        /// marked with this attribute do not appear in an ordinary directory listing. 
        /// </summary>
        FILE_ATTRIBUTE_HIDDEN = 0x00000002,

        /// <summary>
        /// A file that does not have other attributes set. This flag is 
        /// used to  clear all other flags by specifying it with no other flags set.
        /// </summary>
        FILE_ATTRIBUTE_NORMAL = 0x00000080,

        /// <summary>
        /// A file or directory that is not indexed by the content indexing service. 
        /// </summary>
        FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,

        /// <summary>
        /// The data in this file is not available immediately. 
        /// This attribute indicates that the file data is physically moved to 
        /// offline storage. This attribute is used by Remote Storage, 
        /// which is hierarchical storage management software.
        /// </summary>
        FILE_ATTRIBUTE_OFFLINE = 0x00001000,

        /// <summary>
        /// A file or directory that is read-only. For a file, applications 
        /// can read the file but cannot write to it or delete it. For a 
        /// directory, applications cannot delete it, but applications can 
        /// create and delete files from that directory. 
        /// </summary>
        FILE_ATTRIBUTE_READONLY = 0x00000001,

        /// <summary>
        /// A file or directory that has an associated reparse point. 
        /// </summary>
        FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,

        /// <summary>
        /// A file that is a sparse file. 
        /// </summary>
        FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,

        /// <summary>
        /// A file or directory that the operating system uses a part of or uses exclusively. 
        /// </summary>
        FILE_ATTRIBUTE_SYSTEM = 0x00000004,

        /// <summary>
        /// A file that is being used for temporary storage. 
        /// The operating system may choose to store this file's data in 
        /// memory rather than on mass storage, writing the data to 
        /// mass storage only if data remains in the file when the file is closed. 
        /// </summary>
        FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
    }


    /// <summary>
    /// The type of CreateContext buffer
    /// </summary>
    public enum CreateContextTypeValue
    {
        /// <summary>
        /// The data contains the extended attributes that MUST be stored on the created file
        /// </summary>
        SMB2_CREATE_EA_BUFFER,

        /// <summary>
        /// The data contains a security descriptor that MUST be stored on the created file.
        /// </summary>
        SMB2_CREATE_SD_BUFFER,

        /// <summary>
        /// The client is requesting the open to be durable (see section 3.3.5.9.6).
        /// </summary>
        SMB2_CREATE_DURABLE_HANDLE_REQUEST,

        /// <summary>
        /// The client is requesting to reconnect to a durable open after being disconnected (see section 3.3.5.9.7).
        /// </summary>
        SMB2_CREATE_DURABLE_HANDLE_RECONNECT,

        /// <summary>
        /// The data contains the required allocation size of the newly created file.
        /// </summary>
        SMB2_CREATE_ALLOCATION_SIZE,

        /// <summary>
        /// The client is requesting that the server return maximal access information.
        /// </summary>
        SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST,

        /// <summary>
        /// The client is requesting that the server open an earlier version of the file with the provided time stamp
        /// </summary>
        SMB2_CREATE_TIMEWARP_TOKEN,

        /// <summary>
        /// The client is requesting that the server return a 32-byte opaque blob that uniquely identifies the file
        /// being opened on disk
        /// </summary>
        SMB2_CREATE_QUERY_ON_DISK_ID,

        /// <summary>
        /// The client is requesting that the server return a lease.
        /// dialect.
        /// </summary>
        SMB2_CREATE_REQUEST_LEASE,

        /// <summary>
        /// The client is requesting the server to return a lease on a file or a directory. This is valid only for the SMB 3.x dialect family. 
        /// </summary>
        SMB2_CREATE_REQUEST_LEASE_V2,

        /// <summary>
        /// The client requests the server to mark the open as durable or persistent. 
        /// </summary>
        SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2,

        /// <summary>
        /// The client is attempting to reestablish a durable open as specified in section 3.2.4.4. 
        /// </summary>
        SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2,

        /// <summary>
        /// The client is supplying an identifier provided by an application. It's only valid for the SMB 3.x dialect family. 
        /// </summary>
        SMB2_CREATE_APP_INSTANCE_ID,

        /// <summary>
        /// The client is supplying a version to correspond to the application instance identifier. This value is only supported for SMB 3.1.1 dialect.
        /// </summary>
        SMB2_CREATE_APP_INSTANCE_VERSION,

        /// <summary>
        /// It's used to open the shared virtual disk file as specified in [MS-RSVD] section 2.2.5.2.
        /// </summary>
        SVHDX_OPEN_DEVICE_CONTEXT,

        /// <summary>
        /// Unkonwn name.
        /// </summary>
        UNKNOWN
    }

    /// <summary>
    /// The const strings of Create Context Name.
    /// </summary>
    public static class CreateContextNames
    {
        /// <summary>
        /// The data contains the extended attributes that MUST be stored on the created file.
        /// This value MUST NOT be set for named pipes and print files.
        /// </summary>
        public const string SMB2_CREATE_EA_BUFFER = "ExtA";

        /// <summary>
        /// The data contains a security descriptor that MUST be stored on the created file.
        /// This value MUST NOT be set for named pipes and print files.
        /// </summary>
        public const string SMB2_CREATE_SD_BUFFER = "SecD";

        /// <summary>
        /// The client is requesting the open to be durable (see section 3.3.5.9.6).
        /// </summary>
        public const string SMB2_CREATE_DURABLE_HANDLE_REQUEST = "DHnQ";

        /// <summary>
        /// The client is requesting to reconnect to a durable open after being disconnected (see section 3.3.5.9.7).
        /// </summary>
        public const string SMB2_CREATE_DURABLE_HANDLE_RECONNECT = "DHnC";

        /// <summary>
        /// The data contains the required allocation size of the newly created file.
        /// </summary>
        public const string SMB2_CREATE_ALLOCATION_SIZE = "AlSi";

        /// <summary>
        /// The client is requesting that the server return maximal access information.
        /// </summary>
        public const string SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST = "MxAc";

        /// <summary>
        /// The client is requesting that the server open an earlier version of the file with the provided time stamp.
        /// </summary>
        public const string SMB2_CREATE_TIMEWARP_TOKEN = "TWrp";

        /// <summary>
        /// The client is requesting that the server return a 32-byte opaque blob that uniquely identifies the file
        /// being opened on disk. No data is passed to the server by the client.
        /// </summary>
        public const string SMB2_CREATE_QUERY_ON_DISK_ID = "QFid";

        /// <summary>
        /// The client is requesting that the server return a lease. 
        /// </summary>
        public const string SMB2_CREATE_REQUEST_LEASE = "RqLs"; //This value is only supported for the SMB 2.1 and 3.x dialect family.

        public const string SMB2_CREATE_REQUEST_LEASE_V2 = "RqLs"; //This value is only supported for the SMB 3.x dialect family. 

        public const string SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 = "DH2Q"; //This value is only supported for the SMB 3.x dialect family.

        public const string SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 = "DH2C"; //This value is only supported for the SMB 3.x dialect family.

        public const string SMB2_CREATE_APP_INSTANCE_ID = "{6AA6BC45-A7EF-4AF7-9008-FA462E144D74}"; //This value is only supported for the SMB 3.x dialect family.

        //Convert from 0xB982D0B73B56074FA07B524A8116A010 to GUID format
        public const string SMB2_CREATE_APP_INSTANCE_VERSION = "{B7D082B9-563B-4F07-A07B-524A8116A010}"; //This value is only supported for SMB 3.1.1 dialect.   

        //Convert from 0x9CCBCF9E04C1E643980E158DA1F6EC83 to GUID format
        public const string SVHDX_OPEN_DEVICE_CONTEXT = "9ecfcb9c-c104-43e6-980e-158da1f6ec83"; //This Create Context value is not valid for the SMB 2.002, SMB 2.1, and SMB 3.0 dialects.
    }

    /// <summary>
    /// The command code of this packet
    /// </summary>
    public enum Smb2Command : ushort
    {
        /// <summary>
        /// NEGOTIATE
        /// </summary>
        NEGOTIATE = 0x00,

        /// <summary>
        /// SESSION_SETUP
        /// </summary>
        SESSION_SETUP = 0x01,

        /// <summary>
        /// LOGOFF
        /// </summary>
        LOGOFF = 0x02,

        /// <summary>
        /// TREE_CONNECT
        /// </summary>
        TREE_CONNECT = 0x03,

        /// <summary>
        /// TREE_DISCONNECT
        /// </summary>
        TREE_DISCONNECT = 0x04,

        /// <summary>
        /// CREATE 
        /// </summary>
        CREATE = 0x05,

        /// <summary>
        /// CLOSE
        /// </summary>
        CLOSE = 0x06,

        /// <summary>
        /// FLUSH
        /// </summary>
        FLUSH = 0x07,

        /// <summary>
        /// READ
        /// </summary>
        READ = 0x08,

        /// <summary>
        /// WRITE
        /// </summary>
        WRITE = 0x09,

        /// <summary>
        /// LOCK
        /// </summary>
        LOCK = 0x0A,

        /// <summary>
        /// IOCTL
        /// </summary>
        IOCTL = 0x0B,

        /// <summary>
        /// CANCEL
        /// </summary>
        CANCEL = 0x0C,

        /// <summary>
        /// ECHO
        /// </summary>
        ECHO = 0x0D,

        /// <summary>
        /// QUERY_DIRECTORY
        /// </summary>
        QUERY_DIRECTORY = 0x0E,

        /// <summary>
        /// CHANGE_NOTIFY
        /// </summary>
        CHANGE_NOTIFY = 0x0F,

        /// <summary>
        /// QUERY_INFO
        /// </summary>
        QUERY_INFO = 0x10,

        /// <summary>
        /// SET_INFO
        /// </summary>
        SET_INFO = 0x11,

        /// <summary>
        /// OPLOCK_BREAK
        /// </summary>
        OPLOCK_BREAK = 0x12,
    }

    public static class ErrefStatus
    {
        public const uint STATUS_INVALID_NETWORK_RESPONSE = 0xC00000C3;
    }

    public static class ApplicationSpecificSmb2Status
    {
        public const uint STATUS_INVALID_PARAMETER = 0xFFFF0001;
        public const uint STATUS_END_CONNECTION = 0xFFFF0002;
        public const uint STATUS_OPERATION_FAILED = 0xFFFF0003;
    }

    public static class Smb2DummyStatus
    {
        public const uint STATUS_DISCONNECT = 0xFFFFFFFF;
    }


    /// <summary>
    /// the Status of Smb2Packet. they are get from [MS-ERREF]. 
    /// </summary>
    public static class Smb2Status
    {
        #region Const Values

        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        public const uint STATUS_SUCCESS = 0x00000000;

        /// <summary>
        /// The operation that was requested is pending completion.
        /// </summary>
        public const uint STATUS_PENDING = 0x00000103;

        /// <summary>
        /// Indicates that a notify change request has been completed due to closing the handle that made the notify change request.
        /// </summary>
        public const uint STATUS_NOTIFY_CLEANUP = 0x0000010B;

        /// <summary>
        /// Indicates that a notify change request is being completed and that the information is not being returned in the caller's buffer. 
        /// The caller now needs to enumerate the files to find the changes.
        /// </summary>
        public const uint STATUS_NOTIFY_ENUM_DIR = 0x0000010C;

        /// <summary>
        /// The data was too large to fit into the specified buffer.
        /// </summary>
        public const uint STATUS_BUFFER_OVERFLOW = 0x80000005;

        /// <summary>
        /// No more entries are available from an enumeration operation.
        /// </summary>
        public const uint STATUS_NO_MORE_ENTRIES = 0x8000001A;

        /// <summary>
        /// The create operation stopped after reaching a symbolic link.
        /// </summary>
        public const uint STATUS_STOPPED_ON_SYMLINK = 0x8000002D;

        /// <summary>
        /// The token supplied to the function is invalid.
        /// </summary>
        public const uint SEC_E_INVALID_TOKEN = 0x80090308;

        /// <summary>
        /// No credentials are available in the security package.
        /// </summary>
        public const uint SEC_E_NO_CREDENTIALS = 0x8009030E;

        /// <summary>
        /// Indicates Domain Referral sent to DC Server with version less than 3
        /// </summary>
        public const uint STATUS_UNSUCCESSFUL = 0xC0000001;

        /// <summary>
        /// The specified information class is not a valid information class for the specified object.
        /// </summary>
        public const uint STATUS_INVALID_INFO_CLASS = 0xC0000003;

        /// <summary>
        /// An invalid parameter was passed to a service or function.
        /// </summary>
        public const uint STATUS_INVALID_PARAMETER = 0xC000000D;

        /// <summary>
        /// The file does not exist.
        /// </summary>
        public const uint STATUS_NO_SUCH_FILE = 0xC000000F;

        /// <summary>
        /// possible value
        /// </summary>
        public const uint STATUS_INVALID_DEVICE_REQUEST = 0xC0000010;

        /// <summary>
        /// The specified request is not a valid operation for the target device.
        /// </summary>
        public const uint STATUS_END_OF_FILE = 0xC0000011;

        /// <summary>
        /// The specified I/O request packet (IRP) cannot be disposed of because the I/O operation is not complete.
        /// </summary>
        public const uint STATUS_MORE_PROCESSING_REQUIRED = 0xC0000016;

        /// <summary>
        /// A process has requested access to an object but has not been granted those access rights.
        /// </summary>
        public const uint STATUS_ACCESS_DENIED = 0xC0000022;

        /// <summary>
        ///  The buffer is too small to contain the entry. No information has been written to the buffer.
        /// </summary>
        public const uint STATUS_BUFFER_TOO_SMALL = 0xC0000023;

        /// <summary>
        /// The object name is not found.
        /// </summary>
        public const uint STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034;

        /// <summary>
        /// The object name already exists.
        /// </summary>
        public const uint STATUS_OBJECT_NAME_COLLISION = 0xC0000035;

        /// <summary>
        /// A file cannot be opened because the share access flags are incompatible.
        /// </summary>
        public const uint STATUS_SHARING_VIOLATION = 0xC0000043;

        /// <summary>
        /// An operation involving EAs failed because the file system does not support EAs.
        /// </summary>
        public const uint STATUS_EAS_NOT_SUPPORTED = 0xC000004F;

        /// <summary>
        /// A requested read/write cannot be granted due to a conflicting file lock.
        /// </summary>
        public const uint STATUS_FILE_LOCK_CONFLICT = 0xC0000054;

        /// <summary>
        /// A requested file lock cannot be granted due to other existing locks.
        /// </summary>
        public const uint STATUS_LOCK_NOT_GRANTED = 0xC0000055;

        /// <summary>
        /// The attempted logon is invalid.
        /// This is either due to a bad username or authentication information.
        /// </summary>
        public const uint STATUS_LOGON_FAILURE = 0xC000006D;

        /// <summary>
        /// An operation failed because the disk was full.
        /// </summary>
        public const uint STATUS_DISK_FULL = 0xC000007F;

        /// <summary>
        /// Insufficient system resources exist to complete the API.
        /// </summary>
        public const uint STATUS_INSUFFICIENT_RESOURCES = 0xC000009A;

        /// <summary>
        /// The file that was specified as a target is a directory, and the caller specified that it could be anything but a directory.
        /// </summary>
        public const uint STATUS_FILE_IS_A_DIRECTORY = 0xC00000BA;

        /// <summary>
        /// The request is not supported.
        /// </summary>
        public const uint STATUS_NOT_SUPPORTED = 0xC00000BB;

        /// <summary>
        /// The network name was deleted.
        /// </summary>
        public const uint STATUS_NETWORK_NAME_DELETED = 0xC00000C9;

        /// <summary>
        /// The specified share name cannot be found on the remote server.
        /// </summary>
        public const uint STATUS_BAD_NETWORK_NAME = 0xC00000CC;

        /// <summary>
        /// No more connections can be made to this remote computer at this time because the computer has already accepted the maximum number of connections.
        /// </summary>
        public const uint STATUS_REQUEST_NOT_ACCEPTED = 0xC00000D0;

        /// <summary>
        /// An error status returned when the opportunistic lock (oplock) request is denied.
        /// </summary>
        public const uint STATUS_OPLOCK_NOT_GRANTED = 0xC00000E2;

        /// <summary>
        /// An error status returned when an invalid opportunistic lock (oplock) acknowledgment is received by a file system.
        /// </summary>
        public const uint STATUS_INVALID_OPLOCK_PROTOCOL = 0xC00000E3;

        /// <summary>
        /// A requested opened file is not a directory.
        /// </summary>
        public const uint STATUS_NOT_A_DIRECTORY = 0xC0000103;

        /// <summary>
        /// The I/O request was canceled.
        /// </summary>
        public const uint STATUS_CANCELLED = 0xC0000120;

        /// <summary>
        /// An I/O request other than close and several other special case operations
        /// was attempted using a file object that had already been closed
        /// </summary>
        public const uint STATUS_FILE_CLOSED = 0xC0000128;

        /// <summary>
        /// The device is not in a valid state to perform this request.
        /// </summary>
        public const uint STATUS_INVALID_DEVICE_STATE = 0xC0000184;

        /// <summary>
        /// The remote user session has been deleted.
        /// </summary>
        public const uint STATUS_USER_SESSION_DELETED = 0xC0000203;

        /// <summary>
        /// Indicates the attempt to insert the ID in the index failed because the ID is already in the index.
        /// </summary>
        public const uint STATUS_DUPLICATE_OBJECTID = 0xC000022A;

        /// <summary>
        /// The client session has expired; so the client must re-authenticate to continue accessing the remote resources.
        /// </summary>
        public const uint STATUS_NETWORK_SESSION_EXPIRED = 0xC000035C;

        /// <summary>
        /// Hash generation for the specified version and hash type is not enabled on server.
        /// </summary>
        public const uint STATUS_HASH_NOT_SUPPORTED = 0xC000A100;

        /// <summary>
        /// The hash requests is not present or not up to date with the current file contents.
        /// </summary>
        public const uint STATUS_HASH_NOT_PRESENT = 0xC000A101;

        /// <summary>
        /// Indicates that the directory trying to be deleted is not empty.
        /// </summary>
        public const uint STATUS_DIRECTORY_NOT_EMPTY = 0xC0000101;

        /// <summary>
        /// A file system filter on the server has not opted in for Offload Read support.
        /// </summary>
        public const uint STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED = 0xC000A2A1;

        /// <summary>
        /// Indicates failed DFSC sysvol referrals to DC or failed root referrals to DFSC server
        /// </summary>
        public const uint STATUS_NOT_FOUND = 0xC0000225;

        /// <summary>
        /// Indicates dfsc server is unavailable or DFS referrals meant for the domain are sent to the DFSC server.
        /// </summary>
        public const uint STATUS_DFS_UNAVAILABLE = 0xC000026d;

        /// <summary>
        /// Indicates the operation was successful, but no range was processed.
        /// </summary>
        public const uint STATUS_NO_RANGES_PROCESSED = 0xC0000460;

        /// <summary>
        /// Indicates the file is temporarily unavailable.
        /// </summary>
        public const uint STATUS_FILE_NOT_AVAILABLE = 0xC0000467;

        /// <summary>
        /// Indicates the server is temporarily unavailable.
        /// </summary>
        public const uint STATUS_SERVER_UNAVAILABLE = 0xC0000466;

        /// <summary>
        /// Indicates the server forced close file
        /// </summary>
        public const uint STATUS_FILE_FORCED_CLOSED = 0xC00000B6;

        /// <summary>
        /// Indicates the Hash Algorithms client provided are not supported by Server
        /// </summary>
        public const uint STATUS_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP = 0xC05D0000;

        /// <summary>
        /// Offload read operations cannot be performed on:
        /// Compressed files
        /// Sparse files
        /// Encrypted files
        /// File system metadata files
        /// </summary>
        public const uint STATUS_OFFLOAD_READ_FILE_NOT_SUPPORTED = 0xC000A2A3;

        /// <summary>
        /// The volume is write-protected and changes to it cannot be made. 
        /// </summary>
        public const uint STATUS_MEDIA_WRITE_PROTECTED = 0xC00000A2;

        #endregion

        public static string GetStatusCode(uint status)
        {
            string statusCode = "";
            switch (status)
            {
                case Smb2Status.STATUS_SUCCESS:
                    statusCode = "STATUS_SUCCESS";
                    break;

                case Smb2Status.STATUS_PENDING:
                    statusCode = "STATUS_PENDING";
                    break;

                case Smb2Status.STATUS_NOTIFY_CLEANUP:
                    statusCode = "STATUS_NOTIFY_CLEANUP";
                    break;

                case Smb2Status.STATUS_NOTIFY_ENUM_DIR:
                    statusCode = "STATUS_NOTIFY_ENUM_DIR";
                    break;

                case Smb2Status.STATUS_BUFFER_OVERFLOW:
                    statusCode = "STATUS_BUFFER_OVERFLOW";
                    break;

                case Smb2Status.STATUS_NO_MORE_ENTRIES:
                    statusCode = "STATUS_NO_MORE_ENTRIES";
                    break;

                case Smb2Status.STATUS_STOPPED_ON_SYMLINK:
                    statusCode = "STATUS_STOPPED_ON_SYMLINK";
                    break;

                case Smb2Status.SEC_E_INVALID_TOKEN:
                    statusCode = "SEC_E_INVALID_TOKEN";
                    break;

                case Smb2Status.SEC_E_NO_CREDENTIALS:
                    statusCode = "SEC_E_NO_CREDENTIALS";
                    break;

                case Smb2Status.STATUS_UNSUCCESSFUL:
                    statusCode = "STATUS_UNSUCCESSFUL";
                    break;

                case Smb2Status.STATUS_INVALID_INFO_CLASS:
                    statusCode = "STATUS_INVALID_INFO_CLASS";
                    break;

                case Smb2Status.STATUS_INVALID_PARAMETER:
                    statusCode = "STATUS_INVALID_PARAMETER";
                    break;

                case Smb2Status.STATUS_NO_SUCH_FILE:
                    statusCode = "STATUS_NO_SUCH_FILE";
                    break;

                case Smb2Status.STATUS_INVALID_DEVICE_REQUEST:
                    statusCode = "STATUS_INVALID_DEVICE_REQUEST";
                    break;

                case Smb2Status.STATUS_END_OF_FILE:
                    statusCode = "STATUS_END_OF_FILE";
                    break;

                case Smb2Status.STATUS_MORE_PROCESSING_REQUIRED:
                    statusCode = "STATUS_MORE_PROCESSING_REQUIRED";
                    break;

                case Smb2Status.STATUS_ACCESS_DENIED:
                    statusCode = "STATUS_ACCESS_DENIED";
                    break;

                case Smb2Status.STATUS_BUFFER_TOO_SMALL:
                    statusCode = "STATUS_BUFFER_TOO_SMALL";
                    break;

                case Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND:
                    statusCode = "STATUS_OBJECT_NAME_NOT_FOUND";
                    break;

                case Smb2Status.STATUS_OBJECT_NAME_COLLISION:
                    statusCode = "STATUS_OBJECT_NAME_COLLISION";
                    break;

                case Smb2Status.STATUS_EAS_NOT_SUPPORTED:
                    statusCode = "STATUS_EAS_NOT_SUPPORTED";
                    break;

                case Smb2Status.STATUS_FILE_LOCK_CONFLICT:
                    statusCode = "STATUS_FILE_LOCK_CONFLICT";
                    break;

                case Smb2Status.STATUS_LOGON_FAILURE:
                    statusCode = "STATUS_LOGON_FAILURE";
                    break;

                case Smb2Status.STATUS_DISK_FULL:
                    statusCode = "STATUS_DISK_FULL";
                    break;

                case Smb2Status.STATUS_INSUFFICIENT_RESOURCES:
                    statusCode = "STATUS_INSUFFICIENT_RESOURCES";
                    break;

                case Smb2Status.STATUS_FILE_IS_A_DIRECTORY:
                    statusCode = "STATUS_FILE_IS_A_DIRECTORY";
                    break;

                case Smb2Status.STATUS_NOT_SUPPORTED:
                    statusCode = "STATUS_NOT_SUPPORTED";
                    break;

                case Smb2Status.STATUS_NETWORK_NAME_DELETED:
                    statusCode = "STATUS_NETWORK_NAME_DELETED";
                    break;

                case Smb2Status.STATUS_BAD_NETWORK_NAME:
                    statusCode = "STATUS_BAD_NETWORK_NAME";
                    break;

                case Smb2Status.STATUS_REQUEST_NOT_ACCEPTED:
                    statusCode = "STATUS_REQUEST_NOT_ACCEPTED";
                    break;

                case Smb2Status.STATUS_LOCK_NOT_GRANTED:
                    statusCode = "STATUS_LOCK_NOT_GRANTED";
                    break;

                case Smb2Status.STATUS_INVALID_OPLOCK_PROTOCOL:
                    statusCode = "STATUS_INVALID_OPLOCK_PROTOCOL";
                    break;

                case Smb2Status.STATUS_NOT_A_DIRECTORY:
                    statusCode = "STATUS_NOT_A_DIRECTORY";
                    break;

                case Smb2Status.STATUS_CANCELLED:
                    statusCode = "STATUS_CANCELLED";
                    break;

                case Smb2Status.STATUS_FILE_CLOSED:
                    statusCode = "STATUS_FILE_CLOSED";
                    break;

                case Smb2Status.STATUS_INVALID_DEVICE_STATE:
                    statusCode = "STATUS_INVALID_DEVICE_STATE";
                    break;

                case Smb2Status.STATUS_USER_SESSION_DELETED:
                    statusCode = "STATUS_USER_SESSION_DELETED";
                    break;

                case Smb2Status.STATUS_NETWORK_SESSION_EXPIRED:
                    statusCode = "STATUS_NETWORK_SESSION_EXPIRED";
                    break;

                case Smb2Status.STATUS_HASH_NOT_SUPPORTED:
                    statusCode = "STATUS_HASH_NOT_SUPPORTED";
                    break;

                case Smb2Status.STATUS_HASH_NOT_PRESENT:
                    statusCode = "STATUS_HASH_NOT_PRESENT";
                    break;

                case Smb2Status.STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED:
                    statusCode = "STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED";
                    break;

                case Smb2Status.STATUS_DFS_UNAVAILABLE:
                    statusCode = "STATUS_DFS_UNAVAILABLE";
                    break;

                case Smb2Status.STATUS_FILE_NOT_AVAILABLE:
                    statusCode = "STATUS_FILE_NOT_AVAILABLE";
                    break;

                case Smb2Status.STATUS_DUPLICATE_OBJECTID:
                    statusCode = "STATUS_DUPLICATE_OBJECTID";
                    break;

                case Smb2Status.STATUS_SHARING_VIOLATION:
                    statusCode = "STATUS_SHARING_VIOLATION";
                    break;

                case Smb2Status.STATUS_SERVER_UNAVAILABLE:
                    statusCode = "STATUS_SERVER_UNAVAILABLE";
                    break;

                case Smb2Status.STATUS_FILE_FORCED_CLOSED:
                    statusCode = "STATUS_FILE_FORCED_CLOSED";
                    break;
                case Smb2Status.STATUS_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP:
                    statusCode = "STATUS_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP";
                    break;
                case Smb2Status.STATUS_OFFLOAD_READ_FILE_NOT_SUPPORTED:
                    statusCode = "STATUS_OFFLOAD_READ_FILE_NOT_SUPPORTED";
                    break;
                case Smb2Status.STATUS_MEDIA_WRITE_PROTECTED:
                    statusCode = "STATUS_MEDIA_WRITE_PROTECTED";
                    break;
                default:
                    statusCode = string.Format("0x{0:x}", status);
                    break;
            }
            return statusCode;
        }
    }
}
