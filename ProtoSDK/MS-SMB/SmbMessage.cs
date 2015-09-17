// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the key for ServerSendSequenceNumber
    /// </summary>
    public struct ServerSendSequenceNumberKey
    {
        /// <summary>
        /// If set to a non-zero value, this field represents the high-order
        /// bytes of a process identifier (PID). It is combined with the PIDLow field below to form a full PID.
        /// </summary>
        public ushort PidHigh;

        /// <summary>
        /// The lower 16-bits of the PID.
        /// </summary>
        public ushort PidLow;

        /// <summary>
        /// A multiplex identifier (MID).
        /// </summary>
        public ushort Mid;

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">
        /// Another object to compare to.
        /// </param>
        /// <returns>
        /// true if obj and this instance are the same type and represent the same value;<para/>
        /// otherwise, false.
        /// </returns>
        [SuppressMessage("Microsoft.Usage", "CA2231:OverloadOperatorEqualsOnOverridingValueTypeEquals")]
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ServerSendSequenceNumberKey))
            {
                return false;
            }

            ServerSendSequenceNumberKey key = (ServerSendSequenceNumberKey)obj;

            return this.PidHigh == key.PidHigh && this.PidLow == key.PidLow && this.Mid == key.Mid;
        }


        /// <summary>
        /// get hash code of object
        /// </summary>
        /// <returns>the hash code of object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// The session key state. This can be either Unavailable or Available.
    /// </summary>
    public enum SessionKeyStateValue
    {
        /// <summary>
        /// invalid.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// session key is unvailable
        /// </summary>
        Unavailable = 0x01,

        /// <summary>
        /// session key is available
        /// </summary>
        Available = 0x02,
    }

    /// <summary>
    /// the transport type of smb
    /// </summary>
    internal enum TransportType
    {
        /// <summary>
        /// Direct TCP Transport
        /// </summary>
        TCP,

        /// <summary>
        /// NetBIOS over TCP Transport
        /// </summary>
        NetBIOS,
    }

    /// <summary>
    /// Indicates the state of the Session
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// the session is expired.
        /// </summary>
        Expired,

        /// <summary>
        /// the session is in process.
        /// </summary>
        InProcess,

        /// <summary>
        /// the session is complete
        /// </summary>
        Complete,
    }

    /// <summary>
    /// Indicates the state of session key.
    /// </summary>
    public enum SessionKeyState
    {
        /// <summary>
        /// the session key is available
        /// </summary>
        Available,

        /// <summary>
        /// the session key is unavailable.
        /// </summary>
        Unavailable,
    }

    /// <summary>
    /// this enum is used to extend the TransSubCommand, 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum TransSubCommandExtended : int
    {
        /// <summary>
        /// 2.2.12.23 TRANS_MAILSLOT_WRITE Request 
        /// </summary>
        TRANS_EXT_MAILSLOT_WRITE = 0xff,

        /// <summary>
        /// SmbTransRapRequestPacket 
        /// </summary>
        TRANS_EXT_RAP = 0xfe
    }

    /// <summary>
    /// the CreateOptions in subcommand of NtTransact
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NtTransactCreateOptions : uint
    {
        /// <summary>
        /// The file being created or opened is a directory file. With this option, 
        /// the CreateDisposition field MUST be set to FILE_CREATE, FILE_OPEN, or FILE_OPEN_IF.
        /// When this bit field is set, other compatible CreateOptions include only the following: 
        /// FILE_WRITE_THROUGH, FILE_OPEN_FOR_BACKUP_INTENT, and FILE_OPEN_BY_FILE_ID.
        /// </summary>
        FILE_DIRECTORY_FILE = 0x00000001,

        /// <summary>
        /// Applications that write data to the file MUST actually transfer the data into the
        /// file before any write request is considered complete. If FILE_NO_INTERMEDIATE_BUFFERING is set,
        /// the server MUST assume that FILE_WRITE_THROUGH is set in the create request.
        /// </summary>
        FILE_WRITE_THROUGH = 0x00000002,

        /// <summary>
        /// This option indicates that access to the file MAY be sequential.
        /// The server MAY use this information to influence its caching and read-ahead strategy for this file. 
        /// The file MAY in fact be accessed randomly,
        /// but the server MAY optimize its caching and read-ahead policy for sequential access.
        /// </summary>
        FILE_SEQUENTIAL_ONLY = 0x00000004,

        /// <summary>
        /// The file SHOULD NOT be cached or buffered in an internal buffer by the server. 
        /// This option is incompatible when the FILE_APPEND_DATA bit field is set in the DesiredAccess field.
        /// </summary>
        FILE_NO_INTERMEDIATE_BUFFERING = 0x00000008,

        /// <summary>
        /// This flag MUST be ignored by the server, and clients SHOULD set this to 0.
        /// </summary>
        FILE_SYNCHRONOUS_IO_ALERT = 0x00000010,

        /// <summary>
        /// This flag MUST be ignored by the server, and clients SHOULD set this to 0.
        /// </summary>
        FILE_SYNCHRONOUS_IO_NONALERT = 0x00000020,

        /// <summary>
        /// If the file being opened is a directory, 
        /// the server MUST fail the request with STATUS_FILE_IS_A_DIRECTORY in
        /// the Status field of the SMB header in the server response. 
        /// </summary>
        FILE_NON_DIRECTORY_FILE = 0x00000040,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option MUST be ignored by the server. 
        /// </summary>
        FILE_CREATE_TREE_CONNECTION = 0x00000080,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option MUST be ignored by the server. 
        /// </summary>
        FILE_COMPLETE_IF_OPLOCKED = 0x00000100,

        /// <summary>
        /// The application that initiated the client's request does not understand extended attributes (EAs).
        /// If the EAs on an existing file being opened indicate that the caller SHOULD understand EAs to 
        /// correctly interpret the file, the server SHOULD fail this request with STATUS_ACCESS_DENIED in
        /// the Status field of the SMB header in the server response. 
        /// </summary>
        FILE_NO_EA_KNOWLEDGE = 0x00000200,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option MUST be ignored if received by the server.
        /// </summary>
        FILE_OPEN_FOR_RECOVERY = 0x00000400,

        /// <summary>
        /// Indicates that access to the file MAY be random. 
        /// The server MAY use this information to influence its caching and read-ahead strategy for this file.
        /// This is a hint to the server that sequential read-ahead operations MAY NOT be appropriate on the file. 
        /// </summary>
        FILE_RANDOM_ACCESS = 0x00000800,

        /// <summary>
        /// The file SHOULD be automatically deleted when the last 
        /// open request on this file is closed. When this option is set, the DesiredAccess
        /// field MUST include the DELETE flag. This option is often used for temporary files.
        /// </summary>
        FILE_DELETE_ON_CLOSE = 0x00001000,

        /// <summary>
        /// Opens a file based on the FileId. If this option is set,
        /// the server MUST fail the request with STATUS_NOT_SUPPORTED in the
        /// Status field of the SMB header in the server response. 
        /// </summary>
        FILE_OPEN_BY_FILE_ID = 0x00002000,

        /// <summary>
        /// The file is being opened or created for the purposes of either a backup or
        /// a restore operation. Thus, the server MAY make appropriate checks to ensure that the caller
        /// is capable of overriding whatever security checks have been placed on the file to allow a
        /// backup or restore operation to occur. The server MAY choose to check for certain access rights 
        /// to the file before checking the DesiredAccess field. 
        /// </summary>
        FILE_OPEN_FOR_BACKUP_INTENT = 0x00004000,

        /// <summary>
        /// When a new file is created, the file MUST not be compressed even it is on a compressed volume. 
        /// The flag MUST be ignored when opening an existing file.
        /// </summary>
        FILE_NO_COMPRESSION = 0x00008000,

        /// <summary>
        /// This flag SHOULD be ignored by the server.
        /// </summary>
        FILE_OPEN_REQUIRING_OPLOCK = 0x00010000,

        /// <summary>
        /// This flag SHOULD be ignored by the server.
        /// </summary>
        FILE_DISALLOW_EXCLUSIVE = 0x00020000,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option MUST be ignored if received 
        /// by the server. 
        /// </summary>
        FILE_RESERVE_OPFILTER = 0x00100000,

        /// <summary>
        /// If the file or directory being opened is a reparse point, this option requests that the server 
        /// open the reparse point itself, rather than the target to which the reparse point points.
        /// </summary>
        FILE_OPEN_REPARSE_POINT = 0x00200000,

        /// <summary>
        /// In a hierarchical storage management environment, this option 
        /// requests that the file SHOULD NOT be recalled from
        /// tertiary storage such as tape. A file recall can take up 
        /// to several minutes in a hierarchical storage management environment. 
        /// The clients can specify this option to avoid such delays.
        /// </summary>
        FILE_OPEN_NO_RECALL = 0x00400000,

        /// <summary>
        /// This option SHOULD NOT be sent by the clients, and this option
        /// MUST be ignored if received by the server.
        /// </summary>
        FILE_OPEN_FOR_FREE_SPACE_QUERY = 0x00800000
    }

    /// <summary>
    /// rename from FunctionCode to NtTransFunctionCode. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum NtTransFunctionCode : int
    {
        /// <summary>
        /// Access previous versions of a file. 
        /// </summary>
        FSCTL_SRV_ENUMERATE_SNAPSHOTS = 0x00144064,

        /// <summary>
        /// Retrieve an opaque file reference for server-side data movement. 
        /// </summary>
        FSCTL_SRV_REQUEST_RESUME_KEY = 0x00140078,

        /// <summary>
        /// Use for server-side data movement. 
        /// </summary>
        FSCTL_SRV_COPYCHUNK = 0x001440F2
    }

    /// <summary>
    /// rename from NTSubCommands to NtTransSubCommand. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum SmbNtTransSubCommand : int
    {
        /// <summary>
        /// An SMB_COM_NT_TRANSACTION (section 2.2.14) request with an NT_TRANSACT_QUERY_QUOTA subcommand code is sent 
        /// by a client to query quota information from a server. 
        /// </summary>
        NT_TRANSACT_QUERY_QUOTA = 0x0007,

        /// <summary>
        /// An SMB_COM_NT_TRANSACTION (section 2.2.14) request with an NT_TRANSACT_SET_QUOTA subcommand code is sent 
        /// by a client to set quota information on a server. 
        /// </summary>
        NT_TRANSACT_SET_QUOTA = 0x0008
    }

    /// <summary>
    /// This field determines the information contained in the response.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FindInformationLevel : ushort
    {
        /// <summary>
        /// None
        /// </summary>
        NONE = 0x0000,

        /// <summary>
        /// Return standard information about the file(s). See the details below for the list of fields.
        /// </summary>
        SMB_INFO_STANDARD = 0x0001,

        /// <summary>
        /// Return the standard information about the file(s) including the size of the extended attribute list.
        /// </summary>
        SMB_INFO_QUERY_EA_SIZE = 0x0002,

        /// <summary>
        /// Return the standard information about the file(s) including specific extended attributes provided in the request. The requested extended attributes are provided in the Trans_Data. See below for details.
        /// </summary>
        SMB_INFO_QUERY_EAS_FROM_LIST = 0x0003,

        /// <summary>
        /// Return standard information about the file(s) in the directory. Provides support for 64 bit values for important fields.
        /// </summary>
        SMB_FIND_FILE_DIRECTORY_INFO = 0x0101,

        /// <summary>
        /// Return the same information as the above along with the size of the extended attribute list of the file(s).
        /// </summary>
        SMB_FIND_FILE_FULL_DIRECTORY_INFO = 0x0102,

        /// <summary>
        /// Return the names of the file(s).
        /// </summary>
        SMB_FIND_FILE_NAMES_INFO = 0x0103,

        /// <summary>
        /// Return a combination of the data from SMB_FIND_FILE_FULL_DIRECTORY_INFO and SMB_FIND_FILE_NAMES_INFO.
        /// </summary>
        SMB_FIND_FILE_BOTH_DIRECTORY_INFO = 0x0104,

        /// <summary>
        /// Return information of file and directory
        /// </summary>
        SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO = 0x0105,

        /// <summary>
        /// Return information of file and directory
        /// </summary>
        SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO = 0x0106
    }

    /// <summary>
    /// Level (2 bytes):  A USHORT value (as specified in [MS-DTYP] section 2.2.54) that describes  the information 
    /// level being queried for the pipe. The only supported value  is 0x0001. If the server receives any other value, 
    /// it MUST fail the request  with STATUS_INVALID_PARAMETER. 
    /// </summary>
    public enum NamedPipeInformationLevel : int
    {
        /// <summary>
        /// The only supported value is 0x0001. 
        /// </summary>
        VALID = 0x0001,

        /// <summary>
        /// The invalid value. 
        /// </summary>
        INVALID = 0x0000
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_OPEN_ANDX Response 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_OPEN_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 19 
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value MUST be set to 0xFF if there are no 
        /// additional SMB command responses in the server response packet. 
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, and the client MUST ignore this field 
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to the start of the 
        /// WordCount field in the next SMB command response in this packet. This field is valid only if the 
        /// AndXCommand field is not set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored by the client 
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// A valid FID representing the open instance of the file. 
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The actual file system attributes of the file. If none of the attribute bytes are set, the file attributes 
        /// MUST refer to a regular file 
        /// </summary>
        public SmbFileAttributes FileAttrs;

        /// <summary>
        /// 32-bit integer time value of last modification to the file 
        /// </summary>
        public UTime LastWriteTime;

        /// <summary>
        /// The number of bytes in the file. This field is advisory and MAY be used. 
        /// </summary>
        public uint DataSize;

        /// <summary>
        /// A 16-bit field that shows granted access rights to the file 
        /// </summary>
        public AccessRightsValue GrantedAccess;

        /// <summary>
        /// A 16-bit field that shows the resource type opened. 
        /// </summary>
        public ResourceTypeValue FileType;

        /// <summary>
        /// A 16-bit field that shows the status of the named pipe if the resource type opened is a named pipe 
        /// </summary>
        public SMB_NMPIPE_STATUS DeviceState;

        /// <summary>
        /// A 16-bit field that shows the results of the open operation 
        /// </summary>
        public OpenResultsValues Action;

        /// <summary>
        /// An optional 32-bit server file identifier that SHOULD uniquely identify the opening of the file on the  
        /// server. 
        /// </summary>
        public uint ServerFid;

        /// <summary>
        /// This field MUST be zero 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The maximum access rights that this user has on this file. This field MUST be encoded in the ACCESS_MASK  
        /// format, as specified in [CIFS] section 3.7. 
        /// </summary>
        public uint MaximalAccessRights;

        /// <summary>
        /// The maximum access rights that the guest account has on this file. This field MUST be encoded in the  
        /// ACCESS_MASK format, as specified in [CIFS] section 3.7. Note that the notion of a guest account is 
        /// somewhat implementation specific. Implementations that do not support the notion of a guest account MUST 
        /// set this  field to zero. 
        /// </summary>
        public uint GuestMaximalAccessRights;
    }

    /// <summary>
    /// A 16-bit field of bit flags. For completeness, all flags are listed in the  following table with their 
    /// symbolic constants. Any values not listed are  considered reserved, SHOULD be set to 0 by the client, and MUST 
    /// be ignored  by the server. 
    /// </summary>
    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum OpenFlags : int
    {
        /// <summary>
        /// If set, the client is requesting extended information in the response. 
        /// </summary>
        SMB_OPEN_EXTENDED_RESPONSE = 0x0010,

        /// <summary>
        /// If set, the client is requesting a batch oplock. This flag is specified in [CIFS] section 5.8 as bit 2. 
        /// </summary>
        SMB_OPEN_OPBATCH = 0x0004,

        /// <summary>
        /// If set, the client is requesting an oplock. This flag is specified in  [CIFS] section 5.8 as bit 1. 
        /// </summary>
        SMB_OPEN_OPLOCK = 0x0002,

        /// <summary>
        /// If set, the client is requesting additional info in the response. The server MUST set DataSize, 
        /// FileAttributes, GrantedAccess, FileType, and  DeviceState in the response. If not set, the server MUST set 
        /// these  fields to 0. This flag is specified in [CIFS] section 5.8 as bit 0. 
        /// </summary>
        SMB_OPEN_QUERY_INFORMATION = 0x0001
    }

    /// <summary>
    /// A set of flags that modify the client request. Unused bit fields SHOULD be  set to 0 by the client when 
    /// sending a request and MUST be ignored when received  by the server. The Flags field in the 
    /// SMB_COM_NT_CREATE_ANDX request MUST be  used as follows. 
    /// </summary>
    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum CreateFlags : int
    {
        /// <summary>
        /// If set, the client is requesting an oplock. This flag is as specified in [CIFS] section 4.2.1. 
        /// </summary>
        NT_CREATE_REQUEST_OPLOCK = 0x00000002,

        /// <summary>
        /// If set, the client is requesting a batch oplock. This flag is as specified in [CIFS] section 4.2.1. 
        /// </summary>
        NT_CREATE_REQUEST_OPBATCH = 0x00000004,

        /// <summary>
        /// If set, the client indicates that the parent directory of  the target is opened. If the target does exist, 
        /// it might  require a delete access check for the parent directory. 
        /// </summary>
        NT_CREATE_OPEN_TARGET_DIR = 0x00000008,

        /// <summary>
        /// If set, the client is requesting extended information  in the response. 
        /// </summary>
        NT_CREATE_REQUEST_EXTENDED_RESPONSE = 0x00000010
    }

    /// <summary>
    /// A set of options that modify the SMB_COM_TREE_CONNECT_ANDX request. The entire  flag set is given here with 
    /// its symbolic constants. Any combination of the following  flags is valid. Any values not given as follows are 
    /// considered reserved. The client  MUST set them to 0, and the server MUST ignore them. 
    /// </summary>
    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum TreeConnectFlags : int
    {
        /// <summary>
        /// If set, the tree connect specified by the TID in the SMB header of the request  SHOULD be disconnected 
        /// when the server sends the response. If this tree  disconnect fails, the error SHOULD be ignored. This is 
        /// called bit 0, as  specified in [CIFS] section 4.1.4. 
        /// </summary>
        TREE_CONNECT_ANDX_DISCONNECT_TID = 0x0001,

        /// <summary>
        /// If set, the client is requesting signing key protection, as specified in  section 3.2.4.2.4. 
        /// </summary>
        TREE_CONNECT_ANDX_EXTENDED_SIGNATURES = 0x0004,

        /// <summary>
        /// If set, the client is requesting extended information on the  SMB_COM_TREE_CONNECT_ANDX response. 
        /// </summary>
        TREE_CONNECT_ANDX_EXTENDED_RESPONSE = 0x0008
    }

    /// <summary>
    /// Capabilities. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum Capabilities : int
    {
        /// <summary>
        /// The server supports SMB_COM_READ_RAW and SMB_COM_WRITE_RAW requests. 
        /// </summary>
        CAP_RAW_MODE = 0x00000001,

        /// <summary>
        /// The server supports SMB_COM_READ_MPX and SMB_COM_WRITE_MPX requests. 
        /// </summary>
        CAP_MPX_MODE = 0x00000002,

        /// <summary>
        /// The server supports 16-bit Unicode characters. 
        /// </summary>
        CAP_UNICODE = 0x00000004,

        /// <summary>
        /// The server supports large files with 64-bit offsets. 
        /// </summary>
        CAP_LARGE_FILES = 0x00000008,

        /// <summary>
        /// The server supports the SMB packets particular to the NTLM 0.12 dialect. 
        /// </summary>
        CAP_NT_SMBS = 0x00000010,

        /// <summary>
        /// The server supports the use of Microsoft remote procedure call (RPC) for  remote API calls that otherwise 
        /// would use the legacy Remote Administration  Protocol, as specified in [MS-RAP]. 
        /// </summary>
        CAP_RPC_REMOTE_APIS = 0x00000020,

        /// <summary>
        /// The server is capable of responding with 32-bit status codes in the  Status field of the SMB header (for 
        /// more information, see 2.2.1).  CAP_STATUS32 is also sometimes referred to as CAP_NT_STATUS. 
        /// </summary>
        CAP_STATUS32 = 0x00000040,

        /// <summary>
        /// The server supports level II opportunistic locks (oplocks). 
        /// </summary>
        CAP_LEVEL_II_OPLOCKS = 0x00000080,

        /// <summary>
        /// The server supports the SMB_COM_LOCK_AND_READ command requests. 
        /// </summary>
        CAP_LOCK_AND_READ = 0x00000100,

        /// <summary>
        /// The server supports the TRANS2_FIND_FIRST2, TRANS2_FIND_NEXT2, and  FIND_CLOSE2 command requests. 
        /// </summary>
        CAP_NT_FIND = 0x00000200,

        /// <summary>
        /// The server is aware of the DFS Referral Protocol, as specified in  [MS-DFSC] and can respond to Microsoft 
        /// DFS referral requests. For  more information, see [MSDFS]. 
        /// </summary>
        CAP_DFS = 0x00001000,

        /// <summary>
        /// The server supports Windows NT information-level requests, as specified in section 2.2.13. This allows 
        /// the client to pass native Windows NT  structures (as specified in [MS-FSCC]) in QUERY and SET operations,  
        /// as specified in section 2.2.13. 
        /// </summary>
        CAP_INFOLEVEL_PASSTHRU = 0x00002000,

        /// <summary>
        /// The server supports large read operations. This capability affects  the maximum size, in bytes, of the 
        /// server buffer for sending an  SMB_COM_READ_ANDX response to the client. When this capability is set by the 
        /// server (and set by the client in the  SMB_COM_SESSION_SETUP_ANDX request (section 2.2.4)),  the maximum 
        /// server buffer size for sending data can be up to 65,535  bytes rather than the MaxBufferSize field. 
        /// Therefore, the server can  send a single SMB_COM_READ_ANDX response to the client up to this size.  When 
        /// signing is active on a connection, clients MUST limit read  lengths to the "MaxBufferSize" negotiated by 
        /// the server irrespective  of the value of the CAP_LARGE_READX flag. 
        /// </summary>
        CAP_LARGE_READX = 0x00004000,

        /// <summary>
        /// The server supports large write operations. This capability affects  the maximum size, in bytes, of the 
        /// server buffer for receiving an  SMB_COM_WRITE_ANDX client request. When this capability is set by  the 
        /// server (and set by the client in the SMB_COM_SESSION_SETUP_ANDX  request (section 2.2.4)), the maximum 
        /// server buffer size can be up  to 65,535 bytes rather than the MaxBufferSize field. Therefore, a  client 
        /// can send a single SMB_COM_WRITE_ANDX request up to this size.  When signing is active on a connection, 
        /// clients MUST limit write  lengths to the "MaxBufferSize" negotiated by the server irrespective  of the 
        /// value of the CAP_LARGE_WRITEX flag. 
        /// </summary>
        CAP_LARGE_WRITE = 0x00008000,

        /// <summary>
        /// The server supports the FSCTL_SRV_REQUEST_RESUME_KEY (as specified in sections 2.2.14.7.2 and 2.2.14.8.2) 
        /// FSCTL that was sent as an  SMB_COM_NT_TRANSACTION_REQUEST with an NT_TRANSACT_IOCTL subcommand,  as 
        /// specified in section 2.2.14.7. 
        /// </summary>
        CAP_LWIO = 0x00010000,

        /// <summary>
        /// The server supports UNIX extensions. For more information, see [SNIA]. 
        /// </summary>
        CAP_UNIX = 0x00800000,

        /// <summary>
        /// The server supports compressed SMB packets. 
        /// </summary>
        CAP_COMPRESSED_DATA = 0x02000000,

        /// <summary>
        /// The server supports reauthentication, if required, as specified in  section 3.1.5.1. 
        /// </summary>
        CAP_DYNAMIC_REAUTH = 0x20000000,

        /// <summary>
        /// The server supports persistent handles. 
        /// </summary>
        CAP_PERSISTENT_HANDLES = 0x40000000,

        /// <summary>
        /// The server supports extended security for authentication, as  specified in section 3.2.4.2.3. This bit is 
        /// used in conjunction  with the SMB_FLAGS2_EXTENDED_SECURITY Flags2 field in the SMB  header (bit 11), as 
        /// specified in SMB Header Extensions and Changes. 
        /// </summary>
        CAP_EXTENDED_SECURITY = unchecked((int)0x80000000)
    }

    /// <summary>
    /// the package name of security 
    /// </summary>
    public enum SmbSecurityPackage
    {
        /// <summary>
        /// Microsoft Negotiate is a security support provider (SSP) that acts as an application layer between SSPI 
        /// and the other SSPs. When an application calls into SSPI to log on to a network, it can specify an SSP to 
        /// process the request. If the application specifies Negotiate, Negotiate analyzes the request and picks the 
        /// best SSP to handle the request based on customer-configured security policy. 
        /// </summary>
        Negotiate = 0,

        /// <summary>
        /// The Kerberos protocol defines how clients interact with a network authentication service. Clients obtain 
        /// tickets from the Kerberos Key Distribution Center (KDC), and they present these tickets to servers when 
        /// connections are established. 
        /// </summary>
        Kerberos = 1,

        /// <summary>
        /// Windows NT Challenge/Response (NTLM) is the authentication protocol used on networks that include systems 
        /// running the Windows NT operating system and on stand-alone systems. NTLM stands for Windows NT LAN 
        /// Manager, a name chosen to distinguish this more advanced challenge/response-based protocol from its weaker 
        /// predecessor LAN Manager (LM). 
        /// </summary>
        NTLM = 2,
    }

    /// <summary>
    /// the verson of implicit ntlm 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum ImplicitNtlmVersion
    {
        /// <summary>
        /// transport password as plain-text. the NEGOTIATE_ENCRYPT_PASSWORDS of SecurityMode in negotiate  response 
        /// should set to 0. 
        /// </summary>
        PlainTextPassword = 0x01,

        /// <summary>
        /// using ntlm v1 
        /// </summary>
        NtlmVersion1 = 0x02,

        /// <summary>
        /// using ntlm v2. w2k8 is not support. only 2k3 is available. 
        /// </summary>
        NtlmVersion2 = 0x03,
    }

    /// <summary>
    /// Message signing policy. 
    /// </summary>
    [FlagsAttribute]
    public enum SignState : int
    {
        /// <summary>
        /// none requirement about sign 
        /// </summary>
        NONE = 0x00,

        /// <summary>
        /// sign is disabled 
        /// </summary>
        DISABLED = 0x01,

        /// <summary>
        /// sign is enabled 
        /// </summary>
        ENABLED = 0x02,

        /// <summary>
        /// sign is required. 
        /// </summary>
        REQUIRED = 0x04,

        /// <summary>
        /// unless required, sign is disabled. 
        /// </summary>
        DISABLED_UNLESS_REQUIRED = 0x08
    }

    /// <summary>
    /// the const string definition for dialect names
    /// </summary>
    public sealed class DialectNameString
    {
        /// <summary>
        /// constructor
        /// </summary>
        private DialectNameString()
        {
        }


        /// <summary>
        /// PC NETWORK PROGRAM 1.0
        /// </summary>
        public const string PCNET1 = "PC NETWORK PROGRAM 1.0";

        /// <summary>
        /// XENIX CORE
        /// </summary>
        public const string XENIXCORE = "XENIX CORE";

        /// <summary>
        /// PCLAN1.0
        /// </summary>
        public const string PCLAN1 = "PCLAN1.0";

        /// <summary>
        /// MICROSOFT NETWORKS 1.03
        /// </summary>
        public const string MSNET103 = "MICROSOFT NETWORKS 1.03";

        /// <summary>
        /// MICROSOFT NETWORKS 3.0
        /// </summary>
        public const string MSNET30 = "MICROSOFT NETWORKS 3.0";

        /// <summary>
        /// LANMAN1.0
        /// </summary>
        public const string LANMAN10 = "LANMAN1.0";

        /// <summary>
        /// Windows for Workgroups 3.1a
        /// </summary>
        public const string WFW10 = "Windows for Workgroups 3.1a";

        /// <summary>
        /// DOS LM1.2X002
        /// </summary>
        public const string DOSLANMAN12 = "DOS LM1.2X002";

        /// <summary>
        /// LM1.2X002
        /// </summary>
        public const string LANMAN12 = "LM1.2X002";

        /// <summary>
        /// DOS LM1.2X002
        /// </summary>
        public const string DOSLANMAN20 = "DOS LM1.2X002";

        /// <summary>
        /// DOS LANMAN2.1
        /// </summary>
        public const string DOSLANMAN21 = "DOS LANMAN2.1";

        /// <summary>
        /// LANMAN2.1
        /// </summary>
        public const string LANMAN21 = "LANMAN2.1";

        /// <summary>
        /// NT LM 0.12
        /// </summary>
        public const string NTLANMAN = "NT LM 0.12";

        /// <summary>
        /// SMB 2.002 or SMB 2.???
        /// </summary>
        public const string SMB2 = "SMB 2.002";
    }

    /// <summary>
    /// Message status code returned from the server. 
    /// </summary>
    /// <remarks>rename from MessageStatus to SmbStatus this enum is copy from "enum for adapter" </remarks>
    public enum SmbStatus : int
    {
        /// <summary>
        /// The client request is successful. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// An invalid SMB client request is received by the server. The possible values for this field are as  
        /// specified in [CIFS] section 7. 
        /// </summary>
        STATUS_INVALID_SMB = unchecked((int)0x00010002),

        /// <summary>
        /// The client request to the server contains an invalid Uid value. The possible values for this field  are as 
        /// specified in [CIFS] section 7. 
        /// </summary>
        STATUS_SMB_BAD_UID = unchecked((int)0x005B0002),

        /// <summary>
        /// {Buffer Overflow} The data was too large to fit into the specified buffer. 
        /// </summary>
        STATUS_BUFFER_OVERFLOW = unchecked((int)0x80000005),

        /// <summary>
        /// {No More Files} No more files were found that match the file specification. 
        /// </summary>
        STATUS_NO_MORE_FILES = unchecked((int)0x80000006),

        /// <summary>
        /// {Not Implemented} The requested operation is not implemented. 
        /// </summary>
        STATUS_NOT_IMPLEMENTED = unchecked((int)0xC0000002),

        /// <summary>
        /// The parameter specified in the request is not valid. 
        /// </summary>
        STATUS_INVALID_PARAMETER = unchecked((int)0xC000000D),

        /// <summary>
        /// A device that does not exist was specified. 
        /// </summary>
        STATUS_NO_SUCH_DEVICE = unchecked((int)0xC000000E),

        /// <summary>
        /// The specified request is not a valid operation for the target device. 
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = unchecked((int)0xC0000010),

        /// <summary>
        /// The specified input/output (I/O) request packet (IRP) cannot be disposed of because the I/O operation is 
        /// not complete. 
        /// </summary>
        STATUS_MORE_PROCESSING_REQUIRED = unchecked((int)0xC0000016),

        /// <summary>
        /// The client did not have the required permission needed for the operation. 
        /// </summary>
        STATUS_ACCESS_DENIED = unchecked((int)0xC0000022),

        /// <summary>
        /// {Buffer Too Small} The buffer is too small to contain the entry. No information has been written to the 
        /// buffer. 
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = unchecked((int)0xC0000023),

        /// <summary>
        /// The object name is not found. 
        /// </summary>
        STATUS_OBJECT_NAME_NOT_FOUND = unchecked((int)0xC0000034),

        /// <summary>
        /// The object name already exists. 
        /// </summary>
        STATUS_OBJECT_NAME_COLLISION = unchecked((int)0xC0000035),

        /// <summary>
        /// The path to the directory specified was not found. This error is also returned on a create request if the  
        /// operation requires creating more than one new directory level for the path specified. 
        /// </summary>
        STATUS_OBJECT_PATH_NOT_FOUND = unchecked((int)0xC000003A),

        /// <summary>
        /// A specified impersonation level is invalid. Also used to indicate that a required impersonation level was  
        /// not provided. 
        /// </summary>
        STATUS_BAD_IMPERSONATION_LEVEL = unchecked((int)0xC00000A5),

        /// <summary>
        /// {Device Timeout} The specified I/O operation on %hs was not completed before the time-out period expired. 
        /// </summary>
        STATUS_IO_TIMEOUT = unchecked((int)0xC00000B5),

        /// <summary>
        /// The file that was specified as a target is a directory, and the caller specified that it could be anything 
        ///  but a directory. 
        /// </summary>
        STATUS_FILE_IS_A_DIRECTORY = unchecked((int)0xC00000BA),

        /// <summary>
        /// The client request is not supported. 
        /// </summary>
        STATUS_NOT_SUPPORTED = unchecked((int)0xC00000BB),

        /// <summary>
        /// The network name specified by the client has been deleted on the server. This error is returned if the  
        /// client specifies an incorrect Tid or the share on the server represented by the Tid was deleted. 
        /// </summary>
        STATUS_NETWORK_NAME_DELETED = unchecked((int)0xC00000C9),

        /// <summary>
        /// The user session specified by the client has been deleted on the server. This error is returned by the  
        /// server if the client sends an incorrect Uid. 
        /// </summary>
        STATUS_USER_SESSION_DELETED = unchecked((int)0xC0000203),

        /// <summary>
        /// The client's session has expired; therefore, the client MUST re-authenticate to continue accessing  remote 
        /// resources. 
        /// </summary>
        STATUS_NETWORK_SESSION_EXPIRED = unchecked((int)0xC000035C),

        /// <summary>
        /// The client request received by the server contains an invalid Tid value. The possible values for this  
        /// field are as specified in [CIFS] section 7. 
        /// </summary>
        STATUS_SMB_BAD_TID = unchecked((int)0x00050002),

        /// <summary>
        /// The client request received by the server contains an unknown SMB command code. The possible values for  
        /// this field are as specified in [CIFS] section 7. 
        /// </summary>
        STATUS_SMB_BAD_COMMAND = unchecked((int)0x00160002),

        /// <summary>
        /// The client has requested too many Uid values from the server or the client already has a SMB session  
        /// setup with this Uid value. The possible values for this field are as specified in [CIFS] section 7. 
        /// </summary>
        STATUS_SMB_TOO_MANY_UIDS = unchecked((int)0xC000205A),

        /// <summary>
        /// The client request received by the server is for a non-standard SMB operation (for example, an  
        /// SMB_COM_READ_MPX request on a non-disk share). The client SHOULD send another request with a different  
        /// SMB command to perform this operation. The possible values for this field are as specified in [CIFS]  
        /// section 7. 
        /// </summary>
        STATUS_SMB_USE_STANDARD = unchecked((int)0x00FB0002),

        /// <summary>
        /// The create operation stopped after reaching a symbolic link.
        /// </summary>
        STATUS_STOPPED_ON_SYMLINK = unchecked((int)0x8000002D),
    }

    /// <summary>
    /// Flags2 (2 bytes): The Flags2 field contains individual bit flags that,  depending on the negotiated SMB 
    /// dialect, indicate various client and server capabilities. This field is as specified in [CIFS] sections 2.4.2 
    /// and 3.1.2. Unused bit fields SHOULD  be set to 0 by the sender when sending a response and MUST be ignored 
    /// when received by  the receiver. This field may be constructed using the following possible values. 
    /// </summary>
    [FlagsAttribute]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum SmbHeader_Flags2_Values : int
    {
        /// <summary>
        /// If set in a client request for directory enumeration, the server may return long names  (that is, names 
        /// that are not 8.3 names) in the response to this request. If not set in  a client request for directory 
        /// enumeration, the server MUST return only 8.3 Name in the  response to this request. This flag indicates 
        /// that in a direct enumeration request, paths  are not restricted to 8.3 names by the server. This bit field 
        /// SHOULD be set to 1 when  NTLM1.2X002 or later is negotiated for the SMB dialect. This bit field is called 
        /// bit 0,  as specified in [CIFS] section 3.1.2. 
        /// </summary>
        SMB_FLAGS2_KNOWS_LONG_NAMES = 0x0001,

        /// <summary>
        /// If set in a client request, the client is aware of extended attributes. The client MUST  set this bit if 
        /// the client is aware of extended attributes. In response to a client request  with this flag set, a server 
        /// MAY include extended attributes in the response. This bit  field SHOULD be set to 1 when NTLM1.2X002 or 
        /// later is negotiated for the SMB dialect. This bit field is called bit 1, as specified in [CIFS] section 
        /// 3.1.2. 
        /// </summary>
        SMB_FLAGS2_KNOWS_EAS = 0x0002,

        /// <summary>
        /// If set, the client is requesting signing (if not active) or the message being sent is  signed, as 
        /// specified in section 3.1.4.1. This bit is used on the SMB header of an  SMB_COM_SESSION_SETUP_ANDX client 
        /// request (section 2.2.4) to indicate that the client  supports signing and the server can choose to enforce 
        /// signing on the connection based  on its configuration. If the server wants to turn on signing for this 
        /// connection,  it MUST set this flag and also sign the SMB_COM_SESSION_SETUP_ANDX response (section 2.2.5)— 
        /// after which all traffic on the connection MUST be signed. On the SMB header of other SMB  client requests, 
        /// the setting of this bit indicates that the packet has been signed. This bit field SHOULD be set to 1 when 
        /// NTLM 0.12 or later is negotiated for the SMB  dialect. 
        /// </summary>
        SMB_FLAGS2_SMB_SECURITY_SIGNATURE = 0x0004,

        /// <summary>
        /// If set by the client, the client is requesting compressed data for an SMB_COM_READ_ANDX  request. If 
        /// cleared by the server, the server is notifying the client that the data was  written uncompressed. 
        /// </summary>
        SMB_FLAGS2_COMPRESSED = 0x0008,

        /// <summary>
        /// This flag MUST be set by the client on the first SMB_COM_SESSION_SETUP_ANDX request  (section 2.2.4) sent 
        /// to a server that supports extended security, if the client requires  all further communication with this 
        /// server to be signed. If the server does not support  signing, it MUST disconnect the client by closing the 
        /// underlying transport connection.  Clients and servers MUST ignore this value for all other 
        /// requests/responses. If the  client receives a non-signed response from the server, it MUST disconnect the 
        /// underlying  transport connection immediately. This bit field SHOULD be set to 1 when NTLM 0.12 or  later 
        /// is negotiated for the SMB dialect. 
        /// </summary>
        SMB_FLAGS2_SMB_SECURITY_SIGNATURE_REQUIRED = 0x0010,

        /// <summary>
        /// If set, the path contained in the message contains long names; otherwise, the paths are  restricted to 8.3 
        /// names. This bit field SHOULD be set to 1 when NTLM 0.12 or later is  negotiated for the SMB dialect. 
        /// </summary>
        SMB_FLAGS2_IS_LONG_NAME = 0x0040,

        /// <summary>
        /// If set, the path in the request MUST contain an @GMT token (that is, a Previous Version  token), as 
        /// specified in section 3.2.4.3.1. This bit field SHOULD be set to 1 only when  NTLM 0.12 or later is 
        /// negotiated for the SMB dialect. 
        /// </summary>
        SMB_FLAGS2_REPARSE_PATH = 0x0400,

        /// <summary>
        /// Indicates that the client or server supports SPNEGO authentication, as specified in  section 3.2.4.2.3 for 
        /// client behavior and sections 3.3.5.2 and 3.3.5.3 for server  behavior. This bit field SHOULD be set to 1 
        /// when NTLM 0.12 or later is negotiated for  the SMB dialect  and the client or server supports 
        /// extended security. 
        /// </summary>
        SMB_FLAGS2_EXTENDED_SECURITY = 0x0800,

        /// <summary>
        /// all of the following conditions are met:     1. The server supports Distributed File System (DFS) (as 
        /// indicated by the CAP_DFS      flag specified in section 2.2.3). 2. NTLM 0.12 or later is negotiated for 
        /// the SMB      dialect.     3. The share is a DFS share. (as indicated by the SMB_SHARE_IS_IN_DFS flag 
        /// specified      in section 2.2.7).     4. The operation is targeted at a DFS namespace as indicated by the 
        /// application via      the higher-level action specified in section 3.2.4.2). The client MUST set the flag 
        /// for a SMB_COM_TREE_CONNECT_ANDX request (section 2.2.6) when  conditions 1, 2, and 4 in the preceding list 
        /// are met. For other commands, if a valid TID  is used the flag MUST be set if it was set in the 
        /// SMB_COM_TREE_CONNECT_ANDX request based  on the preceding statement, and if the SMB_COM_TREE_CONNECT_ANDX 
        /// response had the  SMB_SHARE_IS_IN_DFS bit set (condition 3 in the preceding list). 
        /// </summary>
        SMB_FLAGS2_DFS = 0x1000,

        /// <summary>
        /// If set in a client request, a read may be permitted if the client does not have read  permission but does 
        /// have execute permission. This flag is useful only on a read request. This bit field SHOULD be set to 1 
        /// when NTLM1.2X002 or later is negotiated for the SMB  dialect. This bit field is called bit 13, as 
        /// specified in [CIFS] section 3.1.2. 
        /// </summary>
        SMB_FLAGS2_PAGING_IO = 0x2000,

        /// <summary>
        /// If set in a server response, the returned error code MUST be a 32-bit error code in  Status field. 
        /// Otherwise, the Status.DosError.ErrorClass and Status.DosError.Error  fields MUST contain the MS-DOS-style 
        /// error information. When passing 32-bit error  codes is negotiated (see SMB_COM_NEGOTIATE Server Response 
        /// Extension and  SMB_COM_SESSION_SETUP_ANDX Client Request Extension), this flag MUST be set for every SMB.  
        /// This bit field SHOULD be set to 1 when NTLM 0.12 or later is negotiated for the SMB dialect. This bit 
        /// field is called bit 14, as specified in [CIFS] section 3.1.2. 
        /// </summary>
        SMB_FLAGS2_NT_STATUS = 0x4000,

        /// <summary>
        /// If set in a client request or server response, any fields that contain strings in this  SMB message MUST 
        /// be encoded as an array of 16-bit Unicode characters. Otherwise,  these fields MUST be encoded as an array 
        /// of OEM characters. This bit field SHOULD be  set to 1 when NTLM 0.12 or later is negotiated for the SMB 
        /// dialect. This bit field is  called bit 15, as specified in [CIFS] section 3.1.2. 
        /// </summary>
        SMB_FLAGS2_UNICODE = 0x8000
    }

    /// <summary>
    /// The Flags field contains individual flags, as specified in [CIFS] sections  2.4.2 and 3.1.1. The extensions in 
    /// this document do not change the use of  this field. 
    /// </summary>
    [FlagsAttribute]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum SmbHeader_Flags_Values : int
    {
        /// <summary>
        /// When set (returned) from the server in  the SMB_COM_NEGOTIATE response SMB, this bit indicates that the 
        /// server  supports the "sub dialect" consisting of the LockandRead and WriteandUnlock protocols defined 
        /// later in this document. 
        /// </summary>
        LOCK_AND_READ = 0x01,

        /// <summary>
        /// When on (on an SMB request being sent to the server), the client guarantees that there is a receive buffer 
        /// posted such that a send without acknowledgment can be used by the server to respond to the client's 
        /// request. 
        /// </summary>
        NO_ACK = 0x02,

        /// <summary>
        /// Reserved (must be zero). 
        /// </summary>
        RESERVED = 0x04,

        /// <summary>
        /// When on, all pathnames in this SMB must be treated as caseless.  When off, the pathnames are case 
        /// sensitive. 
        /// </summary>
        CASE_INSENSITIVE = 0x08,

        /// <summary>
        /// When on (in  SMB_COM_SESSION_SETUP_ANDX  defined later in this document), all paths sent to the server by 
        /// the client are already canonicalized. This means that file/directory names are in upper case, are valid 
        /// characters, . and .. have been removed, and single backslashes are used as separators. 
        /// </summary>
        CANONICALIZED = 0x10,

        /// <summary>
        /// When on (in  SMB_COM_OPEN, SMB_COM_CREATE and SMB_COM_CREATE_NEW), this indicates that the client is 
        /// requesting that the file be "opportunistically" locked if this process is the only process which has the 
        /// file open at the time of the open request.  If the server "grants" this oplock request, then this bit 
        /// should remain set in the corresponding response SMB to indicate to the client that the oplock request was 
        /// granted.  See the discussion of "oplock" in the sections defining the SMB_COM_OPEN_ANDX and 
        /// SMB_COM_LOCKING_ANDX protocols later in this document (this bit has the same function as bit 1 of Flags if 
        /// the SMB_COM_OPEN_ANDX SMB). 
        /// </summary>
        OPLOCK = 0x20,

        /// <summary>
        /// When on (in core protocols SMB_COM_OPEN_ANDX,SMB_COM_CREATE and SMB_COM_CREATE_NEW), this indicates that 
        /// the server should notify the client on any action which can modify the file (delete, setattrib, rename, 
        /// etc.) by another client.  If not set, the server need only notify the client about another open request by 
        /// a different client.  See the discussion of "oplock" in the sections defining the SMB_COM_OPEN_ANDX  and 
        /// SMB_COM_LOCKING_ANDX SMBs later in this document (this bit has the same function as bit 2 of smb_flags of 
        /// the SMB_COM_OPEN_ANDX SMB).  Bit6 only has meaning if bit5 is set.. 
        /// </summary>
        OPLOCK_NOTIFY = 0x40,

        /// <summary>
        /// When on, this SMB is being sent from the server in response to a client request. The Command     PROGRAM 
        /// 1.0 field usually contains the same value in a protocol request from the client to the server as in the 
        /// matching response from the server to the client. This bit unambiguously distinguishes the command request 
        /// from the command response. 
        /// </summary>
        FROM_SERVER = 0x80
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SESSION_SETUP_ANDX Request 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// The word count for this request MUST be 0x0C  (12) because there are twelve 16-bit WORDs between  the 
        /// WordCount and the ByteCount fields. 
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The secondary SMB command in the packet. This value  MUST be set to 0xFF if there are no additional SMB  
        /// commands in the client request packet. Information  about compounded requests is specified in  [CIFS] 
        /// section  3.12. 
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when  this request is sent, and the server MUST ignore this  value 
        /// when the message is received. Information about  compounded requests is specified in  [CIFS] section  
        /// 3.12. 
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field  MUST be set to the offset in bytes  from the start of the SMB header to the start of the  
        /// WordCount field in the next SMB command in this packet. The field is  valid only if the AndXCommand field 
        /// is  not set to 0xFF. Information about compounded requests  is specified in  [CIFS] section 3.12. 
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// The maximum size, in bytes, of the client buffer for sending and receiving SMB messages. This is the size 
        ///  of the largest message that the server SHOULD send to the client. This is the size of the buffer used  
        /// for the SMB message from the start of the SMB header to the end of the packet. This size is not the size  
        /// of the complete network packet because it does not include the size needed for the underlying transport  
        /// and the SMB transport header. The only exceptions where  this buffer size can be exceeded are the 
        /// SMB_COM_READ_ANDX  command if the client and server both support the CAP_LARGE_READX capability, and the 
        /// SMB_COM_WRITE_ANDX command if the  client and server both support the CAP_LARGE_WRITEX capability (see 
        /// the Capabilities field below). Defaults to a MaxBufferSize value of 16,644 bytes on server versions. Defaults
        /// to  a MaxBufferSize value of 4,356 bytes on client versions. SMB clients and servers use the minimum value of 
        /// the MaxBufferSize field in the SMB_COM_NEGOTIATE response and the MaxBufferSize field in the 
        /// SMB_COM_SESSION_SETUP_ANDX request to set the size of the maximum buffer used for sending and receiving SMB 
        /// messages for both the client and server. 
        /// </summary>
        public ushort MaxBufferSize;

        /// <summary>
        /// Maximum pending multiplexed requests supported  by the client. 
        /// </summary>
        public ushort MaxMpxCount;

        /// <summary>
        /// The number of this virtual connection between  the client and the server. This field SHOULD be set  to a 
        /// value of 0 for the first virtual connection between  the client and the server and SHOULD be set to a 
        /// unique  non-zero value for additional virtual connections using  this SMB connection.-based SMB servers 
        /// set a limit  for the MaxNumberVcs field in the SMB_COM_NEGOTIATE  response to 0x01, but do not enforce 
        /// this limit. This  allows an SMB client to establish more virtual circuits  than allowed by the 
        /// MaxNumberVcs field value. Because  this limit is not enforced on , SMB clients MAY ignore  this limit and 
        /// attempt to establish more than the number  of virtual circuits allowed by this value. The  behavior  of 
        /// the SMB server allows a client to exceed this limit,  but other server implementations MAY enforce this 
        /// limit  and not allow this to occur. 
        /// </summary>
        public ushort VcNumber;

        /// <summary>
        /// The client MUST set this to be equal to the SessionKey  in the SMB_COM_NEGOTIATE response. and later 
        /// servers  ignore this field. 
        /// </summary>
        public uint SessionKey;

        /// <summary>
        /// This value MUST specify the length in bytes of  the variable-length SecurityBlob contained within the  
        /// request. 
        /// </summary>
        public ushort SecurityBlobLength;

        /// <summary>
        /// An unused value that SHOULD be set to 0 when sending  this message. The server MUST ignore this field when 
        ///  receiving this message. 
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A   set of client capabilities. These flags are  a subset of those specified in section  for the server  
        /// capabilities returned in the SMB_COM_NEGOTIATE response. The possible client capabilities MAY only 
        /// include a  combination of the following: CAP_DYNAMIC_REAUTH, CAP_EXTENDED_SECURITY,  CAP_LEVEL_II_OPLOCKS, 
        /// CAP_NT_SMBS, CAP_NT_STATUS, and  CAP_UNICODE. Servers MUST NOT check for Kerberos ticket  expiry if the 
        /// client does not support the CAP_DYNAMIC_REAUTH  capability. and later clients set the CAP_DYNAMIC_REAUTH  
        /// capability bit to indicate to the server that the client  supports re-authentication when the Kerberos 
        /// ticket  for the session expires.  and later servers always  perform re-authentication when the Kerberos 
        /// ticket  for the session expires, irrespective of whether the  client has set the CAP_DYNAMIC_REAUTH flag 
        /// or not. 
        /// </summary>
        public uint Capabilities;
    }

    /// <summary>
    /// the SMB_Data struct of SMB_COM_SESSION_SETUP_ANDX Request 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data
    {
        /// <summary>
        /// This field MUST be the number of data bytes in  the Data buffer in this packet. If SMB_FLAGS2_UNICODE  is 
        /// set in the Flags2 field of the SMB header of the  request, this field has a minimum value of 4. If 
        /// SMB_FLAGS2_UNICODE  is not set, this field has a minimum value of 2. This  field is the total length of 
        /// the combined SecurityBlob,  NativeOS, and NativeLANMan fields. 
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This field MUST be the authentication token sent to the server. 
        /// </summary>
        public byte[] SecurityBlob;

        /// <summary>
        /// Padding bytes. If Unicode support has been enabled and SMB_FLAGS2_UNICODE is set in SMB_Header.Flags2, 
        /// this field MUST contain zero or one NULL bytes as needed to ensure that the AccountName string is aligned 
        /// on a 16-bit boundary. This also forces alignment of subsequent strings without additional padding 
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// A string representing the native operating system of the CIFS client. If SMB_FLAGS2_UNICODE is set in the 
        /// Flags2 field of the SMB header of the request, this string MUST be a null-terminated array of 16-bit 
        /// Unicode characters. Otherwise, this string MUST be  a null-terminated array of OEM characters. If this 
        /// string consists of Unicode characters, this field MUST be aligned to start on  a 2-byte boundary from the 
        /// start of the SMB header. 
        /// </summary>
        public byte[] NativeOS;

        /// <summary>
        /// A string that represents the native LAN manager type of the client. If SMB_FLAGS2_UNICODE is set in the 
        /// Flags2 field of the SMB header of the request, this string MUST be a null-terminated array of 16-bit 
        /// Unicode characters. Otherwise, this string MUST be a null-terminated array of OEM characters. If this 
        /// string consists of Unicode characters, this field MUST be aligned to start on a 2-byte boundary from the 
        /// start of the SMB header. 
        /// </summary>
        public byte[] NativeLanMan;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_SESSION_SETUP_ANDX Response 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 4. 
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value  MUST be set to 0xFF if there are no 
        /// additional SMB command responses in the server response packet. 
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent,  and the client MUST ignore this 
        /// field. 
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of  the SMB header to the start of the 
        /// WordCount field in  the next SMB command response in this packet. This field is valid  only if the 
        /// AndXCommand field is not set to 0xFF.  If AndXCommand is 0xFF, this field MUST be ignored by the client. 
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// A 16-bit field. The two lowest order bits have been defined 
        /// </summary>
        public ActionValues Action;

        /// <summary>
        /// This value MUST specify the length in bytes of the variable-length SecurityBlob contained within the  
        /// response. 
        /// </summary>
        public ushort SecurityBlobLength;
    }

    /// <summary>
    /// the SMB_Data struct of SMB_COM_SESSION_SETUP_ANDX Response 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data
    {
        /// <summary>
        /// USHORT 
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// This value MUST contain the authentication token being returned to the client. 
        /// </summary>
        public byte[] SecurityBlob;

        /// <summary>
        /// Padding bytes. If Unicode support has been enabled, this field MUST contain zero or  one null bytes as 
        /// needed to ensure that the NativeOS field, which follows, is aligned  on a 16-bit boundary 
        /// </summary>
        public byte[] Pad;

        /// <summary>
        /// A string that represents the native operating system of the server. If SMB_FLAGS2_UNICODE is set in the 
        /// Flags2 field  of the SMB header of the response, the string MUST be a null-terminated array of 16-bit 
        /// Unicode characters. Otherwise,  the string MUST be a null-terminated array of OEM characters.  If the 
        /// string consists of Unicode characters, this field  MUST be aligned to start on a 2-byte boundary from the 
        /// start of the SMB header. 
        /// </summary>
        public byte[] NativeOS;

        /// <summary>
        /// A string that represents the native LAN Manager type of the server. If SMB_FLAGS2_UNICODE is set in the 
        /// Flags2 field of the SMB header of the response, the string MUST be a null-terminated  array of 16-bit 
        /// Unicode characters. Otherwise, the string MUST be a null-terminated array of OEM characters. If the string  
        /// consists of Unicode characters, this field MUST be aligned  to start on a 2-byte boundary from the start 
        /// of the SMB header. 
        /// </summary>
        public byte[] NativeLanMan;

        /// <summary>
        /// A string representing the primary domain or workgroup name of the server.  If SMB_FLAGS2_UNICODE is set in 
        /// the Flags2 field of the SMB header of the response, the string MUST be a null-terminated array of 16-bit 
        /// Unicode characters. Otherwise, the string MUST be a null-terminated array of OEM characters. If the string 
        /// consists of Unicode characters, this field MUST be aligned to start on a 2-byte boundary from the start of 
        /// the SMB header. 
        /// </summary>
        public byte[] PrimaryDomain;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 6. The length in two-byte words of the remaining SMB_Parameters.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value MUST be set to
        /// 0xFF if there are no additional SMB command responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, and the client MUST
        /// ignore this field.
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header
        /// to the start of the WordCountfield in the next SMB command response in this packet.
        /// This field is valid only if the AndXCommand field is not 
        /// set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored by the client. 
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// The number of bytes written to the file
        /// </summary>
        public ushort Count;

        /// <summary>
        /// This field is valid when writing to named pipes or I/O devices. This field indicates 
        /// the number of bytes remaining to be read after the requested write was completed. 
        /// If the client wrote to a disk file, this field MUST be set to -1 (0xFFFF). 
        /// </summary>
        public ushort Available;

        /// <summary>
        /// This field contains the two most significant bytes of the count of bytes written. If the number of bytes
        /// written is greater than or equal to 0x00010000( 64 kilobytes), then the server MUST set the two least
        /// significant bytes of the length in the Count field of the request and the two most significant bytes of the
        /// length in the CountHigh field.
        /// </summary>
        public ushort CountHigh;

        /// <summary>
        /// This field MUST be 0.
        /// </summary>
        public ushort Reserved;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_READ_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_READ_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be 12
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. This value MUST be set to 0xFF 
        /// if there are no additional SMB command responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent, and the client MUST
        /// ignore this field
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to
        /// the start of the WordCount field in the next SMB command response in this packet.
        /// This field is valid only if the AndXCommand field is  not set to 0xFF. If AndXCommand
        /// is 0xFF, this field MUST be ignored by the client
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// This field is valid when reading from named pipes or I/O devices. This field indicates 
        /// the number of bytes remaining to be read after the requested read was completed. If the
        /// client read from a disk file, this field MUST be set to -1 (0xFFFF). 
        /// </summary>
        public ushort Available;

        /// <summary>
        /// Reserved and SHOULD be 0
        /// </summary>
        public ushort DataCompactionMode;

        /// <summary>
        /// This field MUST be 0
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The number of data bytes included in the response. This field MAY be 0. If this value
        /// is less than the value in the Request.SMB_Parameters.MaxCountOfBytesToReturn field it 
        /// indicates that the read operation has reached the end of the file (EOF).
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// The offset in bytes from the header of the read data
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// If the data read is greater than or equal to 0x00010000 bytes (64KB) in length, then the server MUST set
        /// the two least-significant bytes of the length in the DataLength field of the response and the two
        /// most-significant bytes of the length in the DataLengthHigh field. Otherwise, this field MUST be set to
        /// zero.
        /// </summary>
        public ushort DataLengthHigh;

        /// <summary>
        /// This field MUST be set to zero by the server and MUST be ignored by the client. The last 4 words are
        /// reserved in order to make the SMB_COM_READ_ANDX response the same size as the SMB_COM_WRITE_ANDX response.
        /// </summary>
        [StaticSize(4, StaticSizeMode.Elements)]
        public ushort[] Reserved2;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_WRITE_ANDX Request
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_WRITE_ANDX_Request_SMB_Parameters
    {
        /// <summary>
        /// This field MUST be either 12 or 14.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet.
        /// This value MUST be set to 0xFF if there are no additional SMB commands in the client request packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this request is sent, and the server MUST ignore this
        /// value when the message is received
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the SMB header to the start of
        /// the WordCount field in the next SMB command in this packet. This field is valid only if the
        /// AndXCommand field is not set to 0xFF. If AndXCommand is 0xFF, this field MUST be ignored by the server.
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// This field MUST be a valid FID indicating the file to which the data SHOULD be written.
        /// </summary>
        public ushort FID;

        /// <summary>
        /// If WordCount is 12 this field represents a 32-bit offset, measured in bytes, of where the write
        /// SHOULD start relative to the beginning of the file. 
        /// If WordCount is 14 this field represents the lower 32 bits of a 64-bit offset.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// This field represents the amount of time, in milliseconds, that a server MUST wait before sending
        /// a response. It is used only when writing to a named pipe or I/O device and does not apply when
        /// writing to a disk file. Support for this field is optional.
        /// Two values have special meaning in this field: 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// A 16-bit field containing flags
        /// </summary>
        public WriteAndxWriteMode WriteMode;

        /// <summary>
        /// This field is an advisory field telling the server approximately how many bytes are to be
        /// written to this file before the next non-write operation. It SHOULD include the number of bytes
        /// to be written by this request. The server MAY either ignore this field or use it to perform 
        /// optimizations.
        /// </summary>
        public ushort Remaining;

        /// <summary>
        /// This field contains the two most significant bytes of the length of the data to write to the file.
        /// If the number of bytes to be written is greater than or equal to 0x00010000( 64 kilobytes), then the 
        /// client MUST set the two least significant bytes of the length in the DataLength field of the request
        /// and the two most significant bytes of the length in the DataLengthHigh field.
        /// </summary>
        public ushort DataLengthHigh;

        /// <summary>
        /// This field is the number of bytes included in the SMB_Data that are to be written to the file.
        /// </summary>
        public ushort DataLength;

        /// <summary>
        /// The offset in bytes from the start of the SMB header to the start of the data that is to be written to 
        /// the file. Specifying this offset allows a client to efficiently align the data buffer.
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// This field is optional. If WordCount is 12 this field is not included in the request.
        /// If WordCount is 14 this field represents the upper 32 bits of a 64-bit offset, measured in
        /// bytes, of where the write SHOULD start relative to the beginning of the file
        /// </summary>
        public uint OffsetHigh;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_ErrorResponse_SMB_Parameters
    {
        /// <summary>
        /// the word count of bytecount
        /// </summary>
        public byte WordCount;

    }

    /// <summary>
    /// the SMB_Data struct of SMB_COM_ErrorResponse_SMB_SMB_Data 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_ErrorResponse_SMB_SMB_Data
    {
        /// <summary>
        /// field MUST be greater than or equal to 2 
        /// </summary>
        public ushort ByteCount;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 17 
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The index of the dialect selected by the server from the list presented in the request. Dialect entries 
        /// are numbered starting  with zero, so a DialectIndex value of zero indicates the first entry in the list. 
        /// If the server does not support any of the  listed dialects, it MUST return a DialectIndex of 0XFFFF. 
        /// </summary>
        public ushort DialectIndex;

        /// <summary>
        /// An 8-bit field, indicating the security modes supported or REQUIRED by the server 
        /// </summary>
        public SecurityModes SecurityMode;

        /// <summary>
        /// The maximum number of outstanding SMB operations the server supports. This value includes existing 
        /// OpLocks,  the NT_TRANSACT_NOTIFY_CHANGE subcommand, and any other command  that are pending on the server. 
        /// If the negotiated MaxMpxCount is one,  then OpLock support MUST be disabled for this session. The 
        /// MaxMpxCount MUST be greater than zero. This parameter has no specific  relationship to the 
        /// SMB_COM_READ_MPX and SMB_COM_WRITE_MPX commands. 
        /// </summary>
        public ushort MaxMpxCount;

        /// <summary>
        /// The maximum number of virtual circuits that MAY be established between the client and the server as part 
        /// of the same SMB session. 
        /// </summary>
        public ushort MaxNumberVcs;

        /// <summary>
        /// The maximum size, in bytes, of the largest SMB message the server can receive. This is the size of the 
        /// largest SMB message that the client MAY send to the server. SMB message size includes the size of the  SMB 
        /// header, parameter, and data blocks. This size does not include any  transport-layer framing or other 
        /// transport-layer data. The server MUST provide a MaxBufferSize of 1024 bytes (1Kbyte) or larger. If 
        /// CAP_RAW_MODE is negotiated, then the SMB_COM_WRITE_RAW command can bypass the MaxBufferSize limit. 
        /// Otherwise, SMB messages sent to the server  MUST have a total size less than or equal to the MaxBufferSize 
        /// value. This includes AndX chained messages. 
        /// </summary>
        public uint MaxBufferSize;

        /// <summary>
        /// The maximum raw buffer size, in bytes, available on the server. This value specifies the maximum message 
        /// size which the client MUST not exceed when sending an SMB_COM_WRITE_RAW client request, and the maximum 
        /// message size that the server MUST not exceed when sending an  SMB_COM_READ_RAW response. This value is 
        /// only significant if CAP_RAW_MODE is negotiated. 
        /// </summary>
        public uint MaxRawSize;

        /// <summary>
        /// A unique token identifying the SMB connection. This value is generated by the server for each SMB 
        /// connection. If the client wishes to create an additional virtual circuit and attach it to the same SMB 
        /// connection, the client MUST provide the SessionKey in the  SMB_COM_SESSION_SETUP_ANDX. This allows 
        /// multiple transport-level connections to be bound together to form a single logical SMB connection. 
        /// </summary>
        public uint SessionKey;

        /// <summary>
        /// A 32-bit field providing a set of server capability indicators. This bit field is used to indicate to the 
        /// client which  features are supported by the server. Any value not listed in  the following table is 
        /// unused. The server MUST set the unused bits to 0 in a response, and the client MUST ignore these bits 
        /// </summary>
        public Capabilities Capabilities;

        /// <summary>
        /// The number of 100-nanosecond intervals that have elapsed since  January 1, 1601, in Coordinated Universal 
        /// Time (UTC) format 
        /// </summary>
        public FileTime SystemTime;

        /// <summary>
        /// A signed 16-bit signed integer that represents the server's time zone, in minutes, from UTC. The time zone 
        /// of the server  MUST be expressed in minutes, plus or minus, from UTC. 
        /// </summary>
        public short ServerTimeZone;

        /// <summary>
        /// If the CAP_EXTENDED_SECURITY bit is set, the server MUST set this value to zero and clients MUST ignore  
        /// this value. When extended security is not used, the SMB_COM_NEGOTIATE server response (section 2.2.2) is  
        /// as specified as follows and in [CIFS] section 4.1.1. 
        /// </summary>
        public byte EncryptionKeyLength;
    }

    /// <summary>
    /// the SMB_Data struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data
    {
        /// <summary>
        /// field MUST be greater than or equal to 2 
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// The GUID generated by the server to uniquely identify this server. This field SHOULD NOT be used by a  
        /// client as a secure method of identifying a server because it can easily be faked. A client may use this  
        /// information to detect if connections to different textual names resolve to the same target server when TCP 
        ///  port 445 is used as a transport. This knowledge can then be used to appropriately set the VcNumber field  
        /// in the SMB_COM_SESSION_SETUP_ANDX request (section 2.2.4), as specified in [CIFS] section 4.1.2. 
        /// </summary>
        public Guid ServerGuid;

        /// <summary>
        /// A security binary large object (BLOB) that SHOULD contain an authentication token as produced by the GSS 
        /// protocol (as specified in section 3.2.4.2.3 and [RFC4178]).
        /// </summary>
        public byte[] SecurityBlob;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION Request 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_Request_SMB_Parameters_File
    {
        /// <summary>
        /// This field MUST be Words.SetupCount (see below) plus 14. This value represents the total number of 
        /// parameter words and MUST be greater than or equal to 14 
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of transaction parameter bytes the client expects to send to the server for this request. 
        /// Parameter bytes for a transaction are carried within the SMB_Data.Trans_Parameters field of the 
        /// SMB_COM_TRANSACTION request. If the size of all of the REQUIRED SMB_Data. Trans_Parameters for a given 
        /// transaction causes the request to exceed the MaxBufferSize established during session setup, then the 
        /// client MUST NOT send all of the parameters in  one request. The client MUST break up the parameters and 
        /// send additional requests using the SMB_COM_TRANSACTION_SECODARY command to send the additional parameters. 
        /// Any single  request MUST not exceed the MaxBufferSize established during session setup. The client 
        /// indicates to the server to expect additional parameters, and thus at least one  
        /// SMB_COM_TRANSACTION_SECONDARY, by setting ParameterCount (see below) to be less than 
        /// TotalParameterCount.See SMB_COM_TRANSACTION_SECONDARY for more information 
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of transaction data bytes the client expects to send to the server for this request. Data 
        /// bytes of a transaction are carried within the SMB_Data.Trans_Data field of the  SMB_COM_TRANSACTION 
        /// request. If the size of all of the REQUIRED SMB_Data.Trans_Data  for a given transaction causes the 
        /// request to exceed the MaxBufferSize established during  session setup, then the client MUST not send all 
        /// of the data in one request. The client MUST break up the data and send additional requests using the 
        /// SMB_COM_TRANSACTION_SECODARY  command to send the additional data. Any single request MUST NOT exceed the 
        /// MaxBufferSize established during session setup. The client indicates to the server to expect additional 
        /// data, and thus at least one SMB_COM_TRANSACTION_SECONDARY, by setting DataCount (see below) to be less 
        /// than TotalDataCount. See SMB_COM_TRANSACTION_SECONDARY for more information 
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The maximum number of SMB_Data.Trans_Parameters bytes that the client accepts in the transaction response. 
        /// The server MUST NOT return more than this number of bytes in the SMB_Data.Trans_Parameters field of the 
        /// response. 
        /// </summary>
        public ushort MaxParameterCount;

        /// <summary>
        /// The maximum number of SMB_Data.Trans_Data bytes that the client accepts  in the transaction response. The 
        /// server MUST NOT return more than this number of bytes in the SMB_Data.Trans_Data field 
        /// </summary>
        public ushort MaxDataCount;

        /// <summary>
        /// The maximum number of bytes that the client accepts in the Setup field of the transaction response. The 
        /// server MUST NOT return more than this number of bytes in the Setup field. 
        /// </summary>
        public byte MaxSetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field with 
        /// MaxSetupCount to form a USHORT. If MaxSetupCount is defined as a USHORT, the high order byte MUST be zero 
        /// </summary>
        public byte Reserved1;

        /// <summary>
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to 
        /// zero by the client sending the request, and MUST be ignored by the server receiving the  request. The 
        /// client MAY set either or both of the following bit flags 
        /// </summary>
        public TransSmbParametersFlags Flags;

        /// <summary>
        /// The value of this field MUST be the maximum number of milliseconds the server SHOULD wait for completion 
        /// of the transaction before generating a timeout and returning a response to the client. The client SHOULD 
        /// set this to 0 to indicate that no time-out is expected. A value of zero indicates that the server SHOULD 
        /// return an error if the resource is not immediately available. If the operation does not complete within 
        /// the specified time, the server MAY abort the request and send a failure response. 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// Reserved. This field MUST be zero in the client request. The server MUST ignore the contents  of this 
        /// field. 
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// The number of transaction parameter bytes the client is sending to the server in this request. Parameter 
        /// bytes for a transaction are carried within the SMB_Data.Trans_Parameters field of the SMB_COM_TRANSACTION 
        /// request. If the transaction request fits within a single SMB_COM_TRANSACTION request (the request size 
        /// does not exceed MaxBufferSize), then this value SHOULD be equal to TotalParameterCount. Otherwise, the sum 
        /// of the ParameterCount values in the primary and secondary transaction request messages MUST be equal to 
        /// the smallest TotalParameterCount value reported to the server. If the value of this field is less than the 
        /// value of TotalParameterCount, then at least one SMB_COM_TRANSACTION_SECONDARY message MUST be used to 
        /// transfer the remaining transaction SMB_Data.Trans_Parameters bytes. The ParameterCount field MUST be used 
        /// to determine the number of transaction SMB_Data.Trans_Parameters bytes contained within the 
        /// SMB_COM_TRANSACTION message. 
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// This field MUST contain the number of bytes from the start of the SMB Header to the start of the 
        /// SMB_Data.Trans_Parameters field. Server implementations MUST use this value to locate the transaction 
        /// parameter block within the request. 
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The number of transaction data bytes the client is sending to the server in this request. Data bytes for a 
        /// transaction are carried within the SMB_Data.Trans_Data field of the  SMB_COM_TRANSACTION request. If the 
        /// transaction request fits within a single SMB_COM_TRANSACTION  request (the request size does not exceed 
        /// MaxBufferSize), then this value SHOULD be equal to TotalDataCount. Otherwise, the sum of the DataCount 
        /// values in the primary and secondary  transaction request messages MUST be equal to the smallest 
        /// TotalDataCount value reported  to the server. If the value of this field is less than the value of 
        /// TotalDataCount, then at least one SMB_COM_TRANSACTION_SECONDARY message MUST be used to transfer the 
        /// remaining  transaction SMB_Data.Trans_Data bytes. The DataCount field MUST be used to determine  the 
        /// number of transaction SMB_Data.Trans_Data bytes contained within the SMB_COM_TRANSACTION message. 
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// This field MUST be the number of bytes from the start of the SMB Header of the request to the  start of 
        /// the SMB_Data.Trans_Data field.  Server implementations MUST use this value to locate the transaction data 
        /// block within the request 
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// This field MUST be the number of setup words that are included in the transaction request. 
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field  with 
        /// SetupCount to form a USHORT. If SetupCount is defined as a USHORT, the high order byte MUST be zero 
        /// </summary>
        public byte Reserved3;

        /// <summary>
        /// The first setup word for this request. This field MUST be set to TRANS_SET_NMPIPE_STATE, the subcommand to 
        ///  request. For a list of named pipe subcommands and values, see section 2.2.12. 
        /// </summary>
        public ushort Subcommand;

        /// <summary>
        /// The second setup word for this request. This field MUST be the SMB file identifier for the named pipe that 
        ///  is having its state changed. This field MUST be set to a valid Fid from a server response for a previous  
        /// SMB command to open or create a named pipe. These commands include: SMB_COM_OPEN, SMB_COM_CREATE,  
        /// SMB_COM_CREATE_TEMPORARY, SMB_COM_CREATE_NEW, SMB_COM_OPEN_ANDX, SMB_COM_NT_CREATE_ANDX, and  
        /// SMB_COM_NT_TRANSACT with subcommand NT_TRANSACT_CREATE. 
        /// </summary>
        public ushort Fid;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TREE_CONNECT_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// The value of this field MUST be 7.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet. 
        /// This value MUST be set to 0xFF if there are no additional SMB command 
        /// responses in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent,
        /// and the client MUST ignore this field
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of 
        /// the SMB header to the start of the WordCount field in the 
        /// next SMB command response in this packet. This field is valid 
        /// only if the AndXCommand field is not set to 0xFF. If AndXCommand 
        /// is 0xFF, this field MUST be ignored by the client
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// A 16-bit field. The following OptionalSupport field flags are defined. 
        /// Any combination of the following flags MUST be supported.
        /// All undefined values are considered reserved. The server SHOULD set
        /// them to 0, and the client MUST ignore them
        /// </summary>
        public ushort OptionalSupport;

        /// <summary>
        /// This field MUST specify the maximum rights that the user has to this share based on the security enforced by 
        /// the share. This value is as specified in the ACCESS_MASK format, as specified in [CIFS] section 3.7.
        /// </summary>
        public uint MaximalShareAccessRights;

        /// <summary>
        /// This field MUST specify the maximum rights that the guest account has on this share based on the security 
        /// enforced by the share. This value is as specified in the ACCESS_MASK format, as specified in [CIFS] section
        /// 3.7. Note that the notion of a guest account is implementation-specific. Implementations that do not 
        /// support the notion of a guest account MUST set this field to zero, implying no access.
        /// </summary>
        public uint GuestMaximalShareAccessRights;
    }

    /// <summary>
    /// the Trans_Data struct of TRANS_MAILSLOT_WRITE Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS_MAILSLOT_WRITE_Response_Trans_Data
    {
        /// <summary>
        /// The Parameter buffer contains a single USHORT (as specified in [MS-DTYP] section 2.2.53). If the SMB 
        /// connection does not support 32-bit status codes (see the SMB_COM_NEGOTIATE server response in 
        /// SMB_COM_NEGOTIATE Server Response Extension (section 2.2.3) and the SMB_COM_SESSION_SETUP_ANDX client 
        /// request in SMB_COM_SESSION_SETUP_ANDX Client Request Extension (section 2.2.4)), OperationStatus MUST be 
        /// set to the same value as the Status.DosError.Error field of the SMB header of the response. If the 
        /// connection does support 32-bit status codes, OperationStatus MUST be set to 0xffff.
        /// </summary>
        public ushort OperationStatus;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_NT_CREATE_ANDX Response
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters
    {
        /// <summary>
        /// The word count for this response MUST be 0x2A (42). WordCount in this case is not used as the count of 
        /// parameter words but is just a number.
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The command code for the next SMB command in the packet.
        /// This value MUST be set to 0xFF if there are no additional SMB command responses
        /// in the server response packet.
        /// </summary>
        public SmbCommand AndXCommand;

        /// <summary>
        /// A reserved field. This MUST be set to 0 when this response is sent,
        /// and the client MUST ignore this field
        /// </summary>
        public byte AndXReserved;

        /// <summary>
        /// This field MUST be set to the offset in bytes from the start of the 
        /// SMB header to the start of the WordCount field in the next SMB
        /// command response in this packet. This field is valid only if the
        /// AndXCommand field is not set to 0xFF. If AndXCommand is 0xFF, 
        /// this field MUST be ignored by the client
        /// </summary>
        public ushort AndXOffset;

        /// <summary>
        /// The oplock level granted to the client process
        /// </summary>
        public OplockLevelValue OplockLevel;

        /// <summary>
        /// A FID representing the file or directory that was created or opened
        /// </summary>
        public ushort FID;

        /// <summary>
        /// The CreationOption (see above) that that the server took as a result of the command.
        /// </summary>
        public uint CreationAction;

        /// <summary>
        /// A 64 bit integer value representing the time the file was created. 
        /// The time value is a signed 64 bit integer representing either 
        /// an absolute time or a time interval. Times are specified in units 
        /// of 100ns. A positive value expresses an absolute time, where the
        /// base time (the 64- bit integer with value 0) is the beginning of
        /// the year 1601 AD in the Gregorian calendar. A negative value expresses
        /// a time interval relative to some base time, usually the current time.
        /// </summary>
        public Cifs.FileTime CreateTime;

        /// <summary>
        /// The time the file was last accessed encoded in the same format as CreateTime (see above).
        /// </summary>
        public Cifs.FileTime LastAccessTime;

        /// <summary>
        /// The time the file was last written, encoded in the same format as CreateTime (see above).
        /// </summary>
        public Cifs.FileTime LastWriteTime;

        /// <summary>
        /// The time the file was last changed, encoded in the same format as CreateTime (see above).
        /// </summary>
        public Cifs.FileTime LastChangeTime;

        /// <summary>
        /// A 32 bit value composed of encoded file attribute values and file access behavior flag values. 
        /// See Request.SMB_Parameters.ExtFileAttributes above for the encoding.
        /// This value provides the attributes the server assigned
        /// to the file or directory as a result of the command.
        /// </summary>
        public uint ExtFileAttributes;

        /// <summary>
        /// The number of bytes allocated to the file by the server. 
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// The end of file offset value. 
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// The file type. This field MUST be interpreted as follows
        /// </summary>
        public FileTypeValue ResourceType;

        /// <summary>
        /// A 16-bit field that contains the state of the named pipe
        /// if the FID represents a named pipe instance. This value MUST be 
        /// any combination of the following bit values. Unused bit fields
        /// SHOULD be set to 0 by the server when sending a response and MUST 
        /// be ignored when received by the client
        /// </summary>
        public SMB_NMPIPE_STATUS NMPipeStatus_or_FileStatusFlags;

        /// <summary>
        /// If the returned FID represents a directory then the server MUST
        /// set this value to a non-zero value. If the FID is not a directory 
        /// then the server MUST set this value to 0 (FALSE).
        /// </summary>
        public byte Directory;

        /// <summary>
        /// A unique value that identifies the volume on which the file resides. This field SHOULD contain zero if the 
        /// underlying file system does not support volume GUIDs.
        /// </summary>
        public byte[] VolumeGUID;

        /// <summary>
        /// This field MUST be a 64-bit opaque value that uniquely identifies this file on a volume. This field MUST 
        /// contain zero if the underlying file system does not support unique FileId numbers on a volume. If the 
        /// underlying file system does support unique FileId numbers, this value SHOULD be set to the unique FileId 
        /// for this file.
        /// </summary>
        public byte[] FileId;

        /// <summary>
        /// Maximum access rights that the user opening the file has for this file. This value MUST be specified 
        /// according to the ACCESS_MASK format, as specified in [CIFS] section 3.7.
        /// </summary>
        public byte[] MaximalAccessRights;

        /// <summary>
        /// The maximum access rights that the guest account has when opening this file. This value MUST be specified 
        /// according to the ACCESS_MASK format, as specified in [CIFS] section 3.7. Note that the notion of a guest account 
        /// is somewhat implementation-specific. Implementations that do not support the notion of a guest account MUST set 
        /// this field to 0.
        /// </summary>
        public byte[] GuestMaximalAccessRights;
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION Request 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION_Request_SMB_Parameters_Mailslot
    {
        /// <summary>
        /// This field MUST be Words.SetupCount (see below) plus 14. This value represents the total number of 
        /// parameter words and MUST be greater than or equal to 14 
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of transaction parameter bytes the client expects to send to the server for this request. 
        /// Parameter bytes for a transaction are carried within the SMB_Data.Trans_Parameters field of the 
        /// SMB_COM_TRANSACTION request. If the size of all of the REQUIRED SMB_Data. Trans_Parameters for a given 
        /// transaction causes the request to exceed the MaxBufferSize established during session setup, then the 
        /// client MUST NOT send all of the parameters in  one request. The client MUST break up the parameters and 
        /// send additional requests using the SMB_COM_TRANSACTION_SECODARY command to send the additional parameters. 
        /// Any single  request MUST not exceed the MaxBufferSize established during session setup. The client 
        /// indicates to the server to expect additional parameters, and thus at least one  
        /// SMB_COM_TRANSACTION_SECONDARY, by setting ParameterCount (see below) to be less than 
        /// TotalParameterCount.See SMB_COM_TRANSACTION_SECONDARY for more information 
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of transaction data bytes the client expects to send to the server for this request. Data 
        /// bytes of a transaction are carried within the SMB_Data.Trans_Data field of the  SMB_COM_TRANSACTION 
        /// request. If the size of all of the REQUIRED SMB_Data.Trans_Data  for a given transaction causes the 
        /// request to exceed the MaxBufferSize established during  session setup, then the client MUST not send all 
        /// of the data in one request. The client MUST break up the data and send additional requests using the 
        /// SMB_COM_TRANSACTION_SECODARY  command to send the additional data. Any single request MUST NOT exceed the 
        /// MaxBufferSize established during session setup. The client indicates to the server to expect additional 
        /// data, and thus at least one SMB_COM_TRANSACTION_SECONDARY, by setting DataCount (see below) to be less 
        /// than TotalDataCount. See SMB_COM_TRANSACTION_SECONDARY for more information 
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The maximum number of SMB_Data.Trans_Parameters bytes that the client accepts in the transaction response. 
        /// The server MUST NOT return more than this number of bytes in the SMB_Data.Trans_Parameters field of the 
        /// response. 
        /// </summary>
        public ushort MaxParameterCount;

        /// <summary>
        /// The maximum number of SMB_Data.Trans_Data bytes that the client accepts  in the transaction response. The 
        /// server MUST NOT return more than this number of bytes in the SMB_Data.Trans_Data field 
        /// </summary>
        public ushort MaxDataCount;

        /// <summary>
        /// The maximum number of bytes that the client accepts in the Setup field of the transaction response. The 
        /// server MUST NOT return more than this number of bytes in the Setup field. 
        /// </summary>
        public byte MaxSetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field with 
        /// MaxSetupCount to form a USHORT. If MaxSetupCount is defined as a USHORT, the high order byte MUST be zero 
        /// </summary>
        public byte Reserved1;

        /// <summary>
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to 
        /// zero by the client sending the request, and MUST be ignored by the server receiving the  request. The 
        /// client MAY set either or both of the following bit flags 
        /// </summary>
        public TransSmbParametersFlags Flags;

        /// <summary>
        /// The value of this field MUST be the maximum number of milliseconds the server SHOULD wait for completion 
        /// of the transaction before generating a timeout and returning a response to the client. The client SHOULD 
        /// set this to 0 to indicate that no time-out is expected. A value of zero indicates that the server SHOULD 
        /// return an error if the resource is not immediately available. If the operation does not complete within 
        /// the specified time, the server MAY abort the request and send a failure response. 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// Reserved. This field MUST be zero in the client request. The server MUST ignore the contents  of this 
        /// field. 
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// The number of transaction parameter bytes the client is sending to the server in this request. Parameter 
        /// bytes for a transaction are carried within the SMB_Data.Trans_Parameters field of the SMB_COM_TRANSACTION 
        /// request. If the transaction request fits within a single SMB_COM_TRANSACTION request (the request size 
        /// does not exceed MaxBufferSize), then this value SHOULD be equal to TotalParameterCount. Otherwise, the sum 
        /// of the ParameterCount values in the primary and secondary transaction request messages MUST be equal to 
        /// the smallest TotalParameterCount value reported to the server. If the value of this field is less than the 
        /// value of TotalParameterCount, then at least one SMB_COM_TRANSACTION_SECONDARY message MUST be used to 
        /// transfer the remaining transaction SMB_Data.Trans_Parameters bytes. The ParameterCount field MUST be used 
        /// to determine the number of transaction SMB_Data.Trans_Parameters bytes contained within the 
        /// SMB_COM_TRANSACTION message. 
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// This field MUST contain the number of bytes from the start of the SMB Header to the start of the 
        /// SMB_Data.Trans_Parameters field. Server implementations MUST use this value to locate the transaction 
        /// parameter block within the request. 
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The number of transaction data bytes the client is sending to the server in this request. Data bytes for a 
        /// transaction are carried within the SMB_Data.Trans_Data field of the  SMB_COM_TRANSACTION request. If the 
        /// transaction request fits within a single SMB_COM_TRANSACTION  request (the request size does not exceed 
        /// MaxBufferSize), then this value SHOULD be equal to TotalDataCount. Otherwise, the sum of the DataCount 
        /// values in the primary and secondary  transaction request messages MUST be equal to the smallest 
        /// TotalDataCount value reported  to the server. If the value of this field is less than the value of 
        /// TotalDataCount, then at least one SMB_COM_TRANSACTION_SECONDARY message MUST be used to transfer the 
        /// remaining  transaction SMB_Data.Trans_Data bytes. The DataCount field MUST be used to determine  the 
        /// number of transaction SMB_Data.Trans_Data bytes contained within the SMB_COM_TRANSACTION message. 
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// This field MUST be the number of bytes from the start of the SMB Header of the request to the  start of 
        /// the SMB_Data.Trans_Data field.  Server implementations MUST use this value to locate the transaction data 
        /// block within the request 
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// This field MUST be the number of setup words that are included in the transaction request. 
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field  with 
        /// SetupCount to form a USHORT. If SetupCount is defined as a USHORT, the high order byte MUST be zero 
        /// </summary>
        public byte Reserved3;

        /// <summary>
        /// The first setup word for this request. This field MUST be set to TRANS_SET_NMPIPE_STATE, the subcommand to 
        ///  request. For a list of named pipe subcommands and values, see section 2.2.12. 
        /// </summary>
        public ushort Subcommand;

        /// <summary>
        /// The second setup word, the numeric priority of the message being written to the mailslot, MUST be in the  
        /// range of 0 to 9. The larger the value, the higher the priority. 
        /// </summary>
        public ushort Priority;

        /// <summary>
        /// The third setup word and the class of the mailslot request. This value MUST be set to one of the following 
        /// values. 
        /// </summary>
        public SmbTransMailslotClass Class;
    }

    /// <summary>
    /// The third setup word and the class of the mailslot request.This value MUST be set to one of the following 
    /// values. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SmbTransMailslotClass : ushort
    {
        /// <summary>
        /// A first-class mailslot is reliable and guarantees the delivery of the message. This class may transmit 
        /// messages of up to 65,535 bytes. Messages to class 1 mailslots MUST NOT be broadcast. 
        /// </summary>
        Class1 = 0x0001,

        /// <summary>
        /// A second-class mailslot is unreliable and does not guarantee delivery. This class may transmit messages up 
        /// to a maximum length that depends on the configuration of the server, but will never be less than 360 
        /// bytes. Messages to class 2 mailslot may be broadcast, which allows a message to be sent to a particular 
        /// mailslot  on all systems. 
        /// </summary>
        Class2 = 0x0002
    }

    /// <summary>
    /// the SMB_Parameters struct of SMB_COM_TRANSACTION2 Request 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_COM_TRANSACTION2_Request_SMB_Parameters
    {
        /// <summary>
        /// The value of Words.SetupCount plus 14. This value represents the total number of SMB parameter words  and 
        /// MUST be greater than or equal to 14 
        /// </summary>
        public byte WordCount;

        /// <summary>
        /// The total number of SMB_COM_TRANSACTION2 parameter bytes to be sent in this transaction request. This 
        /// value MAY be reduced in any or all subsequent SMB_COM_TRANSACTION2_SECONDARY requests that are part of the 
        /// same transaction. This value represents transaction parameter bytes, not SMB parameter words. Transaction 
        /// parameter bytes are carried within in the SMB_Data block of the SMB_COM_TRANSACTION2 request. 
        /// </summary>
        public ushort TotalParameterCount;

        /// <summary>
        /// The total number of SMB_COM_TRANSACTION2 data bytes to be sent in this transaction request. This value MAY 
        /// be reduced in any or all subsequent SMB_COM_TRANSACTION2_SECONDARY requests that are part of the same 
        /// transaction. This value represents transaction  data bytes, not SMB data bytes. 
        /// </summary>
        public ushort TotalDataCount;

        /// <summary>
        /// The maximum number of parameter bytes that the client will accept in the transaction reply. The server 
        /// MUST NOT return more than this number of parameter bytes. 
        /// </summary>
        public ushort MaxParameterCount;

        /// <summary>
        /// USHORT The maximum number of data bytes that the client will accept in the transaction reply. The server 
        /// MUST NOT return more than this number of data bytes. 
        /// </summary>
        public ushort MaxDataCount;

        /// <summary>
        /// Maximum number of setup bytes that the client will accept in the transaction reply. The server MUST NOT 
        /// return more than this number of setup bytes. 
        /// </summary>
        public byte MaxSetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field with 
        /// MaxSetupCount to form a USHORT. If MaxSetupCount is defined as a USHORT, the high order byte MUST be zero. 
        /// </summary>
        public byte Reserved1;

        /// <summary>
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to 
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. 
        /// </summary>
        public ushort Flags;

        /// <summary>
        /// The number of milliseconds the server SHOULD wait for completion of the transaction before generating a 
        /// timeout. A value of zero indicates that the operation MUST NOT block. 
        /// </summary>
        public uint Timeout;

        /// <summary>
        /// Reserved. This field MUST be zero in the client request. The server MUST ignore the contents of this field 
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// The number of transaction parameter bytes being sent in this SMB message.  If the transaction fits within 
        /// a single SMB_COM_TRANSACTION2 request, then this value MUST be equal to TotalParameterCount. Otherwise, 
        /// the sum of the ParameterCount values in the primary and secondary transaction  request messages MUST be 
        /// equal to the smallest TotalParameterCount value reported to the server. If the value of this field is less 
        /// than the value of TotalParameterCount, then at least one SMB_COM_TRANSACTION2_SECONDARY message MUST be 
        /// used to transfer the remaining parameter bytes. The ParameterCount field MUST be used to determine the 
        /// number of transaction parameter bytes contained within the SMB_COM_TRANSACTION2 message 
        /// </summary>
        public ushort ParameterCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the  transaction parameter bytes. This MUST be 
        /// the number of bytes from the start of the SMB message to the start of the SMB_Data.Bytes.Parameters field. 
        ///  Server implementations MUST use this value to locate the  transaction parameter block within the SMB 
        /// message 
        /// </summary>
        public ushort ParameterOffset;

        /// <summary>
        /// The number of transaction data bytes being sent in this SMB message.  If the transaction fits within a 
        /// single SMB_COM_TRANSACTION2 request, then this value MUST be equal to TotalDataCount. Otherwise, the sum  
        /// of the DataCount values in the primary and secondary transaction  request messages MUST be equal to the 
        /// smallest TotalDataCount  value reported to the server. If the value of this field is less than the value 
        /// of TotalDataCount, then at least one SMB_COM_TRANSACTION2_SECONDARY message MUST be used to transfer the 
        /// remaining data bytes. 
        /// </summary>
        public ushort DataCount;

        /// <summary>
        /// The offset, in bytes, from the start of the SMB_Header to the transaction data bytes. This MUST be the 
        /// number of bytes from the start of the SMB message to the start of the SMB_Data.Bytes.Data field. Server 
        /// implementations MUST use this value to locate the transaction data block within the SMB message. 
        /// </summary>
        public ushort DataOffset;

        /// <summary>
        /// The number of setup words that are included in the transaction request. 
        /// </summary>
        public byte SetupCount;

        /// <summary>
        /// A padding byte. This field MUST be zero. Existing CIFS implementations MAY combine this field with 
        /// SetupCount to form a USHORT.  If SetupCount is defined as a USHORT, the high order byte MUST be zero 
        /// </summary>
        public byte Reserved3;

        /// <summary>
        /// First setup word for this request.For a list of subcommands and values, see section 2.2.13. 
        /// </summary>
        public ushort Subcommand;
    }

    /// <summary>
    /// the Trans2_Parameters struct of TRANS2_SET_FILE_SYSTEM_INFORMATION Request 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters
    {
        /// <summary>
        /// This field MUST contain a valid FID returned from a previously successful SMB open command. 
        /// </summary>
        public ushort FID;

        /// <summary>
        /// This field determines the information contained in the response. See TRANS2_SET_PATH_INFORMATION for 
        /// complete details 
        /// </summary>
        public QueryFSInformationLevel InformationLevel;
    }

    /// <summary>
    /// the Trans2_Data struct of TRANS2_SET_FILE_SYSTEM_INFORMATION Request 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Data
    {
        /// <summary>
        /// it is different according to the set path information level. 
        /// </summary>
        public Object Data;
    }

    /// <summary>
    /// An SMB_COM_NT_TRANSACTION request with an NT_TRANSACT_RENAME  subcommand code is sent by a client to request 
    /// that  a file or directory be renamed on the server. The NT_TRANSACT_RENAME  subcommand code is listed in 
    /// [CIFS] section 6.3, but  the request format is not defined in [CIFS].The TotalParameterCount  MUST be greater 
    /// than or equal to 5 with the Parameter  block containing the following structure. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_RENAME_Request_NT_Trans_Parameters
    {
        /// <summary>
        /// The SMB file identifier of the file to rename. 
        /// </summary>
        public ushort Fid;

        /// <summary>
        /// The flags for the rename, defined below. 
        /// </summary>
        public ushort RenameFlags;

        /// <summary>
        /// padding for new name. 
        /// </summary>
        public byte[] Pad1;

        /// <summary>
        /// The name that the file is being renamed to.If SMB_FLAGS2_UNICODE is set in the Flags2 field of the SMB  
        /// header of the request, the NewName field MUST be a null-terminated array of 16-bit Unicode characters.  
        /// Otherwise, the NewName field MUST be a null-terminated array of OEM characters. If the NewName string  
        /// consists of Unicode characters, this field MUST be aligned to start on a 2-byte boundary from the start of 
        /// the SMB header. 
        /// </summary>
        public byte[] NewName;
    }

    /// <summary>
    /// An SMB_COM_NT_TRANSACTION request with an NT_TRANSACT_QUERY_QUOTA  subcommand code is sent by a client to 
    /// query quota  information from a server.The TotalParameterCount field  MUST be equal to 16, and the parameter 
    /// block in the  client request MUST be encoded with the following parameter  block encoding fields. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_QUERY_QUOTA_Request_NT_Trans_Parameters
    {

        /// <summary>
        /// The SMB file identifier of the target directory. 
        /// </summary>
        public ushort Fid;

        /// <summary>
        /// Indicates only a single entry is to be returned instead  of filling the entire buffer. 
        /// </summary>
        public byte ReturnSingleEntry;

        /// <summary>
        /// Indicates that the scan of the quota information is  to be restarted. 
        /// </summary>
        public byte RestartScan;

        /// <summary>
        /// Supplies the length in bytes of the SidList (see below),  or 0 if there is no SidList. 
        /// </summary>
        public uint SidListLength;

        /// <summary>
        /// Supplies the length in bytes of the StartSid (see below),  or 0 if there is no StartSid. MUST be ignored 
        /// by the  receiver if SidListLength is non-zero. 
        /// </summary>
        public uint StartSidLength;

        /// <summary>
        /// Supplies the offset, in bytes, to the StartSid in the  Parameter buffer. 
        /// </summary>
        public uint StartSidOffset;
    }

    /// <summary>
    /// An SMB_COM_NT_TRANSACTION response for an NT_TRANSACT_QUERY_QUOTA  subcommand MUST be sent by a server in 
    /// reply to a client  NT_TRANSACT_QUERY_QUOTA subcommand request when the  request is successful.The 
    /// TotalParameterCount MUST  be 4, and the parameter block in the server response  MUST contain a 32-bit unsigned 
    /// integer value indicating  the length, in bytes, of the returned quota information.The  TotalDataCount MUST 
    /// indicate the length of the Data  buffer, and that buffer MUST contain the quota information  defined below. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Parameters
    {
        /// <summary>
        /// the quota data size. 
        /// </summary>
        public uint QuotaDataSize;
    }

    /// <summary>
    /// An SMB_COM_NT_TRANSACTION response for an NT_TRANSACT_QUERY_QUOTA  subcommand MUST be sent by a server in 
    /// reply to a client  NT_TRANSACT_QUERY_QUOTA subcommand request when the  request is successful.The 
    /// TotalParameterCount MUST  be 4, and the parameter block in the server response  MUST contain a 32-bit unsigned 
    /// integer value indicating  the length, in bytes, of the returned quota information.The  TotalDataCount MUST 
    /// indicate the length of the Data  buffer, and that buffer MUST contain the quota information  defined below. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data
    {
        /// <summary>
        /// An offset to the start of the subsequent entry from  the start of this entry, or 0 for the final entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// The length, in bytes, of the security identifier. 
        /// </summary>
        public uint SidLength;

        /// <summary>
        /// This value  MUST be the time the quota was last changed,  in TIME format. 
        /// </summary>
        public ulong ChangeTime;

        /// <summary>
        /// The amount of quota, in bytes, used by this user. This  field is formatted as a LARGE_INTEGER, as 
        /// specified in [CIFS] section 2.4.2. 
        /// </summary>
        public ulong QuotaUsed;

        /// <summary>
        /// The quota warning limit, in bytes, for this user. This  field is formatted as a LARGE_INTEGER, as 
        /// specified in [CIFS] section 2.4.2. 
        /// </summary>
        public ulong QuotaThreshold;

        /// <summary>
        /// The quota limit, in bytes, for this user. This field  is formatted as a LARGE_INTEGER, as specified in 
        /// [CIFS]  section 2.4.2. 
        /// </summary>
        public ulong QuotaLimit;

        /// <summary>
        /// The security identifier of this user. For more information,  see [MS-DTYP] section 2.4.2. Note that [CIFS] 
        /// sections  4.3.4, 4.3.4.7, 4.3.5, and 4.3.5.6 use Sid as the field  name for a search handle. In 
        /// [XOPEN-SMB], the search  handle field is called a findfirst_dirhandle or findnext_dirhandle.  These are 
        /// better field names for a search handle. 
        /// </summary>
        [Size("SidLength")]
        public byte[] Sid;
    }

    /// <summary>
    /// An SMB_COM_NT_TRANSACTION request with an NT_TRANSACT_SET_QUOTA  subcommand code is sent by a client to set 
    /// quota information  on a server.The TotalParameterCount MUST be set to  2, and the Parameter block in the 
    /// client request MUST contain the 2-byte file identifier for the target directory.The  TotalDataCount MUST 
    /// contain the length of the Data  buffer, and the Data buffer in the client request MUST contain the quota 
    /// information to be applied to the  system as defined below. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Parameters
    {
        /// <summary>
        /// 2-byte file identifier for the target directory. 
        /// </summary>
        public ushort Fid;
    }

    /// <summary>
    /// An SMB_COM_NT_TRANSACTION request with an NT_TRANSACT_SET_QUOTA  subcommand code is sent by a client to set 
    /// quota information  on a server.The TotalParameterCount MUST be set to  2, and the Parameter block in the 
    /// client request MUST contain the 2-byte file identifier for the target directory.The  TotalDataCount MUST 
    /// contain the length of the Data  buffer, and the Data buffer in the client request MUST contain the quota 
    /// information to be applied to the  system as defined below. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Data
    {
        /// <summary>
        /// An offset to the start of the subsequent entry from  the start of this entry, or 0 for the final entry. 
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// The length, in bytes, of the security identifier. 
        /// </summary>
        public uint SidLength;

        /// <summary>
        /// This value MUST be the time the quota was last changed,  in TIME format. 
        /// </summary>
        public ulong ChangeTime;

        /// <summary>
        /// The amount of quota, in bytes, used by this user. This  field is formatted as a LARGE_INTEGER, as 
        /// specified in [CIFS] section 2.4.2. 
        /// </summary>
        public ulong QuotaUsed;

        /// <summary>
        /// The quota warning limit, in bytes, for this user. This  field is formatted as a LARGE_INTEGER, as 
        /// specified in [CIFS] section 2.4.2. 
        /// </summary>
        public ulong QuotaThreshold;

        /// <summary>
        /// The quota limit, in bytes, for this user. This field  is formatted as a LARGE_INTEGER, as specified in 
        /// [CIFS]  section 2.4.2. 
        /// </summary>
        public ulong QuotaLimit;

        /// <summary>
        /// The security identifier of this user. For details,  see [MS-DTYP] section 2.4.2. Note that [CIFS] sections 
        ///  4.3.4, 4.3.4.7, 4.3.5, and 4.3.5.6 use Sid as the field  name for a search handle. In [XOPEN-SMB], the 
        /// search  handle field is called a findfirst_dirhandle or findnext_dirhandle.  These are better field names 
        /// for a search handle. 
        /// </summary>
        [Size("SidLength")]
        public byte[] Sid;
    }

    /// <summary>
    /// This is the response format for a successful NT_TRANSACT_IOCTL request executed with an 
    /// FSCTL_SRV_ENUMERATE_SNAPSHOTS FunctionCode, as specified in section 2.2.14.7.1. There are no output parameters 
    /// (TotalParameterCount MUST be 0), but the output data buffer MUST contain the structure with the following 
    /// server data block encoding, and TotalDataCount SHOULD indicate the length of this structure. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data
    {
        /// <summary>
        /// This value MUST be the number of snapshots for the volume. 
        /// </summary>
        public uint NumberOfSnapShots;

        /// <summary>
        /// This value MUST be the number of snapshots being returned in this packet. 
        /// </summary>
        public uint NumberOfSnapShotsReturned;

        /// <summary>
        /// The size, in bytes, needed for the array. 
        /// </summary>
        public uint SnapShotArraySize;

        /// <summary>
        /// A concatenated set of snapshot names. Each snapshot name MUST be formatted as a null-terminated array of  
        /// 16-bit Unicode characters. The concatenated list MUST be terminated by two 16-bit Unicode null characters. 
        /// Each string in the array MUST be an array of 16-bit Unicode characters in the format of an @GMT token;  
        /// that is, @GMT-YYYY.MM.DD-HH.MM.SS. If all of the snapshots cannot fit into the callers' buffer, based on  
        /// the MaxDataCount of the request, NumberOfSnapShotsReturned MUST be less than NumberOfSnapShots.  
        /// SnapShotArraySize MUST contain the size required to receive the entire array, not including the size of 
        /// the enumeration structure. 
        /// </summary>
        [Size("SnapShotArraySize")]
        public byte[] snapShotMultiSZ;
    }

    /// <summary>
    /// This is the response format for a successful NT_TRANSACT_IOCTL request executed with the  
    /// FSCTL_SRV_REQUEST_RESUME_KEY FunctionCode, as specified in FSCTL_SRV_REQUEST_RESUME_KEY Request (section  
    /// 2.2.14.7.2). There are no output parameters (TotalParameterCount MUST be zero). However, the output data 
    /// buffer MUST contain the structure with the following server data block encoding, and TotalDataCount MUST 
    /// indicate the length of this structure. 
    /// </summary>
    public struct NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data
    {
        /// <summary>
        /// A 24-byte resume key generated by the SMB server that can be subsequently used by the client to uniquely  
        /// identify the open source file in a FSCTL_SRV_COPYCHUNK request. The resume key MUST be treated as a 
        /// 24-byte opaque structure. The SMB client that receives the 24-byte copychunk resume key MUST NOT attach 
        /// any  interpretation to this key and MUST treat it as an opaque value. For more information, see section 
        /// 3.3.1.4. 
        /// </summary>
        [Size("24")]
        public byte[] ResumeKey;

        /// <summary>
        /// The length, in bytes, of the context information. This field is unused. A server SHOULD set this field to  
        /// zero when sent. This field MUST be ignored by the client when the message is received. 
        /// </summary>
        public uint ContextLength;

        /// <summary>
        /// The context extended information. 
        /// </summary>
        [Size("ContextLength")]
        public byte[] Context;
    }

    /// <summary>
    /// This FSCTL is used for server-side data movement, as specified in section 3.1.6. The request is sent as an  
    /// NT_TRANSACT_IOCTL request (section 2.2.14.7). The 32-bit FunctionCode for this call is 0x001440F2. The input  
    /// data buffer contains the structure defined in the list following the table. TotalDataCount field MUST be equal 
    ///  to or greater than 0x34 (52). The request MUST allow for at least 0x000C (12) bytes of response data. The  
    /// MaxDataCount field MUST be equal to or greater than 0x000C (12). The response structure is as specified in  
    /// section 2.2.14.8.3. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data
    {
        /// <summary>
        /// A 24-byte server resume copychunk resume key for a source file in a server-side file copy operation. This  
        /// MUST be an opaque file identifier returned from a previous FSCTL_SRV_REQUEST_RESUME_KEY server response. 
        /// </summary>
        [Size("24")]
        public byte[] CopyChunkResumeKey;

        /// <summary>
        /// The number of entries in the copychunk list. MUST NOT be 0. 
        /// </summary>
        public uint ChunkCount;

        /// <summary>
        /// Reserved. This field SHOULD be set to zero when sent. This field MUST be ignored by the server when the  
        /// message is received. 
        /// </summary>
        public uint Unused;

        /// <summary>
        /// A concatenated list of copychunk blocks. This field is as specified in section 2.2.14.7.3.1. 
        /// </summary>
        [Size("ChunkCount")]
        public NT_TRANSACT_COPY_CHUNK_List[] List;
    }

    /// <summary>
    /// This is the response format for an NT_TRANSACT_IOCTL request executed with the FSCTL_SRV_COPYCHUNK 
    /// FunctionCode, as specified in FSCTL_SRV_COPYCHUNK Request. There are no output parameters (TotalParameterCount 
    /// MUST be 0), but the output data MUST contain the following structure of server data block encoding fields, and 
    /// MaxDataCount MUST indicate the length of this buffer.
    /// </summary>
    public struct NT_TRANSACT_COPY_CHUNK_Response_NT_Trans_Data
    {
        /// <summary>
        /// The maximum number of chunks that can be written in a single request. This value MUST be the number of 
        /// chunks successfully processed by the server.
        /// </summary>
        public uint ChunksWritten;

        /// <summary>
        /// The maximum number of bytes that can be written in an individual chunk. This value MUST be the number 
        /// of bytes written to the target file.
        /// </summary>
        public uint ChunkBytesWritten;

        /// <summary>
        ///The maximum number of bytes across all chunks that can be written in a single request. This value MUST 
        ///be the total number of bytes written to the target file.
        /// </summary>
        public uint TotalBytesWritten;
    }

    /// <summary>
    /// The List packet is a concatenated list of copychunk blocks. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NT_TRANSACT_COPY_CHUNK_List
    {
        /// <summary>
        /// The offset, in bytes, in the source file to copy from. This field is formatted as a LARGE_INTEGER, as  
        /// specified in [CIFS] section 2.4.2. 
        /// </summary>
        public ulong SourceOffset;

        /// <summary>
        /// The offset, in bytes, in the target file to copy to. This field is formatted as a LARGE_INTEGER, as  
        /// specified in [CIFS] section 2.4.2. 
        /// </summary>
        public ulong DestinationOffset;

        /// <summary>
        /// The number of bytes to copy from the source file to the target file. The length is the size of a single 
        /// chunk or copychunk block. A copychunk FSCTL can have a list of such chunks. 
        /// </summary>
        public uint Length;

        /// <summary>
        /// Unused at the present and MUST be treated as reserved. SHOULD be set to zero and MUST be ignored. 
        /// </summary>
        public uint Reserved;
    }

    /// <summary>
    /// transaction rap request parameters. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANSACTION_Rap_Request_Trans_Parameters
    {
        /// <summary>
        /// The operation code for the particular operation. For more information on valid operation codes, see 2.5.4.
        /// </summary>
        public ushort RapOPCode;

        /// <summary>
        /// This value MUST be a null-terminated ASCII descriptor string. The server SHOULD validate that the ParamDesc
        /// value passed by the client matches what is specified by the RAPOpcode. The following table specifies the
        /// descriptor character and the notation for each data type.
        /// </summary>
        public byte[] ParamDesc;

        /// <summary>
        /// (Optional) If this value is specified, it MUST be a null-terminated ASCII descriptor string that describes
        /// the contents of the data returned to the client. Certain RAPOpcodes specify a DataDesc field; for a list
        /// of Remote Administration Protocol commands that specify a DataDesc field, see section 2.5.5.
        /// </summary>
        public byte[] DataDesc;

        /// <summary>
        /// This field combines the following fields, because each of their length is unknown:<para/>
        /// RAPParams: Remote Administration Protocol command-specific parameters, as specified in sections 2.5.5, 2.5.6, 2.5.7,
        /// 2.5.8, and 2.5.9.<para/>
        /// AuxDesc: (Optional) If this value is specified, it MUST be a null-terminated ASCII descriptor string that describes
        /// auxiliary data returned to the client. If no AuxDesc field is specified for the Remote Administration
        /// Protocol command, this field MUST NOT be present. For the origin of the descriptor string values, see
        /// section 4.2.
        /// </summary>
        public byte[] RAPParamsAndAuxDesc;
    }

    /// <summary>
    /// transaction rap request data. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANSACTION_Rap_Request_Trans_Data
    {
        /// <summary>
        /// Additional data for the Remote Administration Protocol request. This field MUST be present in the
        /// NetPrintJobSetInfoRequest command. This field cannot be present in any other command.
        /// </summary>
        public byte[] RAPInData;
    }

    /// <summary>
    /// transaction rap response parameters. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANSACTION_Rap_Response_Trans_Parameters
    {
        /// <summary>
        /// This MUST be a 16-bit unsigned integer. It contains a Win32 error code representing the result of the
        /// Remote Administration Protocol command. The following table lists error codes that have particular meaning
        /// to the Remote Administration Protocol, as indicated in this specification.
        /// </summary>
        public ushort Win32ErrorCode;

        /// <summary>
        /// This field MUST contain a 16-bit signed integer, which a client MUST subtract from the string offset
        /// contained in the low 16 bits of a variable-length field in the Remote Administration Protocol response
        /// buffer. This is to derive the actual byte offset from the start of the response buffer for that field.
        /// </summary>
        public ushort Converter;

        /// <summary>
        /// If present, this structure MUST contain the response information for the Remote Administration Protocol
        /// command in the corresponding Remote Administration Protocol request message. Certain RAPOpcodes require
        /// a RAPOutParams structure; for Remote Administration Protocol commands that require a RAPOutParams
        /// structure, see sections 2.5.5, 2.5.6, 2.5.7, 2.5.8, and 2.5.9.
        /// </summary>
        public byte[] RAPOutParams;
    }

    /// <summary>
    /// transaction rap response data. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANSACTION_Rap_Response_Trans_Data
    {
        /// <summary>
        /// This is the response data for the Remote Administration Protocol operation. The content of the RAPOutData
        /// structure varies according to the Remote Administration Protocol command and the parameters of each Remote
        /// Administration Protocol command. See Remote Administration Protocol responses for each Remote
        /// Administration Protocol command in sections 2.5.5, 2.5.6, 2.5.7, 2.5.8, and 2.5.9.
        /// </summary>
        public byte[] RAPOutData;
    }

    /// <summary>
    /// the state of named pipe that is used in the SmbTransPeekNmpipeResponsePacket.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NamedPipePeekState : ushort
    {
        /// <summary>
        /// Named pipe was disconnected by server.
        /// </summary>
        DisconnectedByServer = 0x0001,

        /// <summary>
        /// Named pipe is listening.
        /// </summary>
        Listening = 0x0002,

        /// <summary>
        /// Named pipe connection to the server is okay.
        /// </summary>
        Okay = 0x0003,

        /// <summary>
        /// Server end of Named pipe is closed.
        /// </summary>
        Closed = 0x0004
    }

    /// <summary>
    /// the query information level of SMB_FIND_FILE_BOTH_DIRECTORY_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FIND_FILE_BOTH_DIRECTORY_INFO
    {
        /// <summary>
        /// This field contains the offset in bytes from this entry in the list to
        /// the next entry in the list. If there are no additional entries the
        /// value MUST be zero (0).
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// This field MUST contain the byte offset of the file within the
        /// parent directory. This member is undefined for file systems, such as NTFS,
        /// in which the position of a file within the parent directory is not
        /// fixed and can be changed at any time to maintain sort order
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// This field contains the date and time when the file was created.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// This field contains the date and time when the file was last accessed.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// This field contains the date and time when data was last written to the file.
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// This field contains the date and time when the file was last changed. 
        /// </summary>
        public FileTime LastChangeTime;

        /// <summary>
        /// Absolute new end-of-file position as a byte offset from the start 
        /// of the file. EndOfFile specifies the byte offset to the end of the file.
        /// Because this value is zero-based, it actually refers to the first free
        /// byte in the file. In other words, EndOfFile is the offset to the
        /// byte immediately following the last valid byte in the file. 
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// This field contains the file allocation size, in bytes. Usually,
        /// this value is a multiple of the sector or cluster size of the underlying physical device
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// ULONG This field contains file attribute information flags encoded as follows.
        /// </summary>
        public SmbFileAttributes32 FileAttributes;

        /// <summary>
        /// This field MUST contain the length of the FileName field in bytes.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field MUST contain the length of the ExtendedAttributeList in bytes.
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// This field MUST contain the length of the ShortName in bytes.
        /// </summary>
        public byte ShortNameLength;

        /// <summary>
        /// field is reserved and MUST be zero (0).
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// This field MUST contain the 8.3 name of the file in Unicode format.
        /// </summary>
        [Size("24")]
        public byte[] ShortName;

        /// <summary>
        /// This field contains the long name of the file.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// the query information level of SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FIND_FILE_ID_FULL_DIRECTORY_INFO
    {
        /// <summary>
        /// This field contains the offset in bytes from this entry in the list to
        /// the next entry in the list. If there are no additional entries the
        /// value MUST be zero (0).
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// This field MUST contain the byte offset of the file within the
        /// parent directory. This member is undefined for file systems, such as NTFS,
        /// in which the position of a file within the parent directory is not
        /// fixed and can be changed at any time to maintain sort order
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// This field contains the date and time when the file was created.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// This field contains the date and time when the file was last accessed.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// This field contains the date and time when data was last written to the file.
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// This field contains the date and time when the file was last changed. 
        /// </summary>
        public FileTime LastChangeTime;

        /// <summary>
        /// Absolute new end-of-file position as a byte offset from the start 
        /// of the file. EndOfFile specifies the byte offset to the end of the file.
        /// Because this value is zero-based, it actually refers to the first free
        /// byte in the file. In other words, EndOfFile is the offset to the
        /// byte immediately following the last valid byte in the file. 
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// This field contains the file allocation size, in bytes. Usually,
        /// this value is a multiple of the sector or cluster size of the underlying physical device
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// ULONG This field contains file attribute information flags encoded as follows.
        /// </summary>
        public SmbFileAttributes32 FileAttributes;

        /// <summary>
        /// This field MUST contain the length of the FileName field in bytes.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field MUST contain the length of the ExtendedAttributeList in bytes.
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// A LARGE_INTEGER that serves as an internal file system identifier. This number MUST be unique for each file
        /// on a given volume. If a remote file system does not support unique FileId values, then the FileId field
        /// MUST be set to zero.
        /// </summary>
        public ulong FileId;

        /// <summary>
        /// This field contains the long name of the file.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }

    /// <summary>
    /// the query information level of SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO
    {
        /// <summary>
        /// This field contains the offset in bytes from this entry in the list to
        /// the next entry in the list. If there are no additional entries the
        /// value MUST be zero (0).
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// This field MUST contain the byte offset of the file within the
        /// parent directory. This member is undefined for file systems, such as NTFS,
        /// in which the position of a file within the parent directory is not
        /// fixed and can be changed at any time to maintain sort order
        /// </summary>
        public uint FileIndex;

        /// <summary>
        /// This field contains the date and time when the file was created.
        /// </summary>
        public FileTime CreationTime;

        /// <summary>
        /// This field contains the date and time when the file was last accessed.
        /// </summary>
        public FileTime LastAccessTime;

        /// <summary>
        /// This field contains the date and time when data was last written to the file.
        /// </summary>
        public FileTime LastWriteTime;

        /// <summary>
        /// This field contains the date and time when the file was last changed. 
        /// </summary>
        public FileTime LastChangeTime;

        /// <summary>
        /// Absolute new end-of-file position as a byte offset from the start 
        /// of the file. EndOfFile specifies the byte offset to the end of the file.
        /// Because this value is zero-based, it actually refers to the first free
        /// byte in the file. In other words, EndOfFile is the offset to the
        /// byte immediately following the last valid byte in the file. 
        /// </summary>
        public ulong EndOfFile;

        /// <summary>
        /// This field contains the file allocation size, in bytes. Usually,
        /// this value is a multiple of the sector or cluster size of the underlying physical device
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        /// ULONG This field contains file attribute information flags encoded as follows.
        /// </summary>
        public SmbFileAttributes32 FileAttributes;

        /// <summary>
        /// This field MUST contain the length of the FileName field in bytes.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// This field MUST contain the length of the ExtendedAttributeList in bytes.
        /// </summary>
        public uint EaSize;

        /// <summary>
        /// This field MUST contain the length of the ShortName in bytes.
        /// </summary>
        public byte ShortNameLength;

        /// <summary>
        /// field is reserved and MUST be zero (0).
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// This field MUST contain the 8.3 name of the file in Unicode format.
        /// </summary>
        [Size("24")]
        public byte[] ShortName;

        /// <summary>
        /// A 16-bit unsigned integer that is used to maintain 64-bit alignment. This member MUST be 0x0000.
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// A LARGE_INTEGER that serves as an internal file system identifier. This number MUST be unique for each file
        /// on a given volume. If a remote file system does not support unique FileId values, then the FileId field
        /// MUST be set to zero.
        /// </summary>
        public ulong FileId;

        /// <summary>
        /// This field contains the long name of the file.
        /// </summary>
        [Size("FileNameLength")]
        public byte[] FileName;
    }
}
