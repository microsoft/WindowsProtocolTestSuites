// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{

    /// <summary>
    /// the structure of MessageId and its statuses: consumed or not.
    /// </summary>
    public class MessageIdStatus
    {
        #region fields

        private ulong messageId;
        private bool isComsumed;

        #endregion


        #region properties

        /// <summary>
        /// the message identity.
        /// </summary>
        public ulong MessageId
        {
            get
            {
                return this.messageId;
            }
            set
            {
                this.messageId = value;
            }
        }


        /// <summary>
        /// whether the messageId is used in request packet or not.
        /// </summary>
        public bool IsComsumed
        {
            get
            {
                return this.isComsumed;
            }
            set
            {
                this.isComsumed = value;
            }
        }

        #endregion


        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageIdStatus()
        {
            this.MessageId = 0;
            this.IsComsumed = false;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messageId">the message identity.</param>
        /// <param name="isComsumed">true: the messageId has been used in request packet.</param>
        public MessageIdStatus(ulong messageId, bool isComsumed)
        {
            this.MessageId = messageId;
            this.IsComsumed = isComsumed;
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public MessageIdStatus(MessageIdStatus messageIdStatus)
        {
            if (messageIdStatus != null)
            {
                this.MessageId = messageIdStatus.MessageId;
                this.IsComsumed = messageIdStatus.IsComsumed;
            }
        }

        #endregion
    }

    #region Used by FileServiceBaseTransport class

    /// <summary>
    /// The ImpersonationLevel used FileServiceBaseTransport.Connect()
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsImpersonationLevel : uint
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
    /// The FileAttribute used FileServiceBaseTransport.Connect()
    /// </summary>
    [Flags()]
    public enum FsFileAttribute
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
        /// used to clear all other flags by specifying it with no other flags set.
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
    /// The CreateDisposition used FileServiceBaseTransport.Connect()
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsCreateDisposition : uint
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
    /// The CreateOption used FileServiceBaseTransport.Connect()
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsCreateOption : uint
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
    /// This can be used in DesiredAccess parameter of FileServiceBaseTransport.Connect()
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), Flags()]
    public enum FsDirectoryDesiredAccess : uint
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
    /// This can be used in DesiredAccess parameter of FileServiceBaseTransport.Connect()
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), Flags()]
    public enum FsFileDesiredAccess : uint
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
    }

    /// <summary>
    /// Fs_Ctl code, defined in [MS-FSCC] section 2.3
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum FsCtlCode : uint
    {
        /// <summary>
        /// This message requests that the server return the object identifier for the file or directory associated   
        /// with the handle on which this FSCTL was invoked. If no object identifier exists, the server MUST create   
        /// one. 
        /// </summary>
        FSCTL_CREATE_OR_GET_OBJECT_ID = 0x900c0,

        /// <summary>
        /// This message requests that the server remove the object identifier from the file or directory associated   
        /// with the handle on which this FSCTL was invoked. The underlying object MUST NOT be deleted. If the file or 
        ///  directory has no object identifier, the request MUST be considered successful. 
        /// </summary>
        FSCTL_DELETE_OBJECT_ID = 0x900a0,

        /// <summary>
        /// This message requests that the server delete the reparse point from the file or directory associated with  
        ///  the handle on which this FSCTL was invoked. The underlying file or directory MUST NOT be deleted. 
        /// </summary>
        FSCTL_DELETE_REPARSE_POINT = 0x900ac,

        /// <summary>
        /// This message requests that the server return the statistical information of the file system such as Type,  
        ///  Version, and so on, as specified in FSCTL_FILESYSTEM_GET_STATISTICS reply, for the file or directory   
        /// associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_FILESYSTEM_GET_STATISTICS = 0x90060,

        /// <summary>
        /// The FSCTL_FIND_FILES_BY_SID request message requests that the server return a list of the files (in the  
        /// directory associated with the handle on which this FSCTL was invoked) whose owner matches the specified   
        /// security identifier (SID). This message contains a FIND_BY_SID_DATA data element. 
        /// </summary>
        FSCTL_FIND_FILES_BY_SID = 0x9008f,

        /// <summary>
        /// This message requests that the server return the current compression state of the file or directory   
        /// associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_GET_COMPRESSION = 0x9003c,

        /// <summary>
        /// This message requests that the server return information about the NTFS file system volume that contains   
        /// the file or directory that is associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_GET_NTFS_VOLUME_DATA = 0x90064,

        /// <summary>
        /// This message requests that the server return the object identifier for the file or directory associated  
        /// with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_GET_OBJECT_ID = 0x9009c,

        /// <summary>
        /// This message requests that the server return the reparse point data for the file or directory associated   
        /// with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_GET_REPARSE_POINT = 0x900a8,

        /// <summary>
        /// The FSCTL_GET_RETRIEVAL_POINTERS request message requests that the server return the virtual cluster  
        /// number to the logical cluster number mapping information that describes the location the on disk of the  
        /// extents within the file or directory associated with the handle on which this FSCTL was invoked. This  
        /// mapping  information is most commonly used by defragmentation utilities. This message contains a   
        /// STARTING_VCN_INPUT_BUFFER data element. 
        /// </summary>
        FSCTL_GET_RETRIEVAL_POINTERS = 0x90073,

        /// <summary>
        /// The FSCTL_IS_PATHNAME_VALID request message requests that the server indicate whether the specified   
        /// pathname is well-formed (of acceptable length, with no invalid characters, and so on - see section 2.1.5)  
        ///  with respect to the volume that contains the file or directory associated with the handle on which this   
        /// FSCTL was invoked. 
        /// </summary>
        FSCTL_IS_PATHNAME_VALID = 0x9002c,

        /// <summary>
        /// The FSCTL_LMR_SET_LINK_TRACKING_INFORMATION request message sets Distributed Link Tracking information  
        /// such as file system type, volume ID, object ID, and destination computer's NetBIOS name for the file or   
        /// directory associated with the handle on which this FSCTL was invoked. The message contains a   
        /// REMOTE_LINK_TRACKING_INFORMATION data element. For more information about Distributed Link Tracking, see  
        /// [MS-DLTW] section 3.1.6. 
        /// </summary>
        FSCTL_LMR_SET_LINK_TRACKING_INFORMATION = 0x1400ec,

        /// <summary>
        /// The FSCTL_PIPE_PEEK request requests that the server copy a named pipe's data into a buffer for preview   
        /// without removing it. The FSCTL_PIPE_PEEK request message is issued to invoke a reply, and does not have an 
        ///  associated data structure. 
        /// </summary>
        FSCTL_PIPE_PEEK = 0x11400c,

        /// <summary>
        /// The FSCTL_PIPE_TRANSCEIVE request is used to send and receive data from an open pipe. Any bytes in the   
        /// FSCTL input buffer are written as a binary large object (BLOB) to the input buffer of the pipe server. 
        /// </summary>
        FSCTL_PIPE_TRANSCEIVE = 0x11c017,

        /// <summary>
        /// The FSCTL_PIPE_WAIT request requests that the server wait until either a time-out interval elapses or an   
        /// instance of the specified named pipe is available for connection. 
        /// </summary>
        FSCTL_PIPE_WAIT = 0x110018,

        /// <summary>
        /// This message requests that the server return the first 0x24 bytes of sector 0 for the volume that contains 
        ///   the file or directory associated with the handle on which this FSCTL was invoked. The first 0x24 bytes 
        /// of   sector 0 are known as the FAT BIOS Parameter Block (BPB), which contains hardware-specific bootstrap  
        ///  information. 
        /// </summary>
        FSCTL_QUERY_FAT_BPB = 0x90058,

        /// <summary>
        /// The FSCTL_QUERY_ALLOCATED_RANGES request message requests that the server scan a file or alternate stream  
        ///  looking for byte ranges that may contain nonzero data, and then return information on those ranges. Only  
        /// sparse files can have zeroed ranges known to the operating system. For other files, the server will return 
        ///  only a single range that contains the starting point and the length requested. The message contains a   
        /// FILE_ALLOCATED_RANGE_BUFFER data element. 
        /// </summary>
        FSCTL_QUERY_ALLOCATED_RANGES = 0x940cf,

        /// <summary>
        /// This message requests UDF-specific volume information for the volume that contains the file or directory   
        /// associated with the handle on which this FSCTL was invoked. This FSCTL is only valid on UDF file systems.  
        /// All other File Systems will treat this as an invalid FSCTL. 
        /// </summary>
        FSCTL_QUERY_ON_DISK_VOLUME_INFO = 0x9013c,

        /// <summary>
        /// Retrieves the defect management properties of the volume that contains the file or directory associated   
        /// with the handle on which this FSCTL was invoked. This FSCTL is only valid on UDF file systems. All other  
        /// File Systems will treat this as an invalid FSCTL. 
        /// </summary>
        FSCTL_QUERY_SPARING_INFO = 0x90138,

        /// <summary>
        /// This message requests that the server return the most recent change journal USN for the file or directory  
        ///  associated with the handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_READ_FILE_USN_DATA = 0x900eb,

        /// <summary>
        /// This message requests that the server recall the file (associated with the handle on which this FSCTL was  
        ///  invoked) from storage media that Remote Storage manages. This FSCTL is not valid for directories. 
        /// </summary>
        FSCTL_RECALL_FILE = 0x90117,

        /// <summary>
        /// The FSCTL_SET_COMPRESSION request message requests that the server set the compression state of the file  
        /// or directory associated with the handle on which this FSCTL was invoked. The message contains a 16-bit  
        /// unsigned integer. 
        /// </summary>
        FSCTL_SET_COMPRESSION = 0x9c040,

        /// <summary>
        /// Sets the software defect management state for the specified file associated with the handle on which this  
        /// FSCTL was invoked. Used for UDF file systems. This FSCTL is only valid on UDF file systems. All other file 
        ///  systems will treat this as an invalid FSCTL. 
        /// </summary>
        FSCTL_SET_DEFECT_MANAGEMENT = 0x98134,

        /// <summary>
        /// The FSCTL_SET_ENCRYPTION request sets the encryption for the file or directory associated with the given   
        /// handle. The message contains an encryption buffer which indicates whether to encrypt/decrypt a file or an  
        /// individual stream. 
        /// </summary>
        FSCTL_SET_ENCRYPTION = 0x900D7,

        /// <summary>
        /// This message sets the object identifier for the file or directory associated with the handle on which this 
        ///   FSCTL was invoked. The message contains a FILE_OBJECTID_BUFFER (section 2.1.3) data element. Either a  
        /// Type 1 or a Type 2 buffer is valid. 
        /// </summary>
        FSCTL_SET_OBJECT_ID = 0x90098,

        /// <summary>
        /// The FSCTL_SET_OBJECT_ID_EXTENDED request message requests that the server set the extended information for 
        ///   the file or directory associated with the handle on which this FSCTL was invoked. The message contains 
        /// an  EXTENDED_INFO data element. 
        /// </summary>
        FSCTL_SET_OBJECT_ID_EXTENDED = 0x900bc,

        /// <summary>
        /// This message requests that the server set a reparse point on the file or directory associated with the  
        /// handle on which this FSCTL was invoked. 
        /// </summary>
        FSCTL_SET_REPARSE_POINT = 0x900a4,

        /// <summary>
        /// This message requests that the server mark the file that is associated with the handle on which this FSCTL 
        ///   was invoked as sparse. In a sparse file, large ranges of zeros (0) may not require disk allocation. 
        /// Space   for nonzero data is allocated as the file is written. The message contains a 
        /// FILE_SET_SPARSE_BUFFER   element. 
        /// </summary>
        FSCTL_SET_SPARSE = 0x900c4,

        /// <summary>
        /// The FSCTL_SET_ZERO_DATA request message requests that the server fill the specified range of the file   
        /// (associated with the handle on which this FSCTL was invoked) with zeros. The message contains a   
        /// FILE_ZERO_DATA_INFORMATION element. 
        /// </summary>
        FSCTL_SET_ZERO_DATA = 0x980c8,

        /// <summary>
        /// This message requests that the server fill the clusters of the target file with zeros when they are   
        /// deallocated. This is used to set a file to secure delete mode, which ensures that data will be zeroed upon 
        ///  file truncation or deletion. 
        /// </summary>
        FSCTL_SET_ZERO_ON_DEALLOCATION = 0x90194,

        /// <summary>
        /// The FSCTL_SIS_COPYFILE request message requests that the server use the single-instance storage (SIS)   
        /// filter to copy a file. The message contains an SI_COPYFILE data element. For more information about   
        /// single-instance storage, see [SIS]. 
        /// </summary>
        FSCTL_SIS_COPYFILE = 0x90100,

        /// <summary>
        /// This message requests that the server generate a record in the server's file system change journal stream  
        ///  for the file or directory associated with the handle on which this FSCTL was invoked, indicating that the 
        ///   file or directory was closed. This FSCTL can be called independently of the actual file close operation 
        /// to  write a USN record and cause a post of any pending USN updates for the indicated file. 
        /// </summary>
        FSCTL_WRITE_USN_CLOSE_RECORD = 0x900ef,
    }

    /// <summary>
    /// transaction rap request parameters. 
    /// </summary>
    public struct RapRequestParam
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
        /// This field combines the following fields, because each of their lengths is unknown:<para/>
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
    public struct RapRequestData
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
    public struct RapResponseParam
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
    public struct RapResponseData
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
    /// The file service rap request packet
    /// </summary>
    public class FsRapRequest
    {
        /// <summary>
        /// The request parameters
        /// </summary>
        public RapRequestParam TransParameters;

        /// <summary>
        /// The request data
        /// </summary>
        public RapRequestData transData;
    }


    /// <summary>
    /// The file service rap response packet
    /// </summary>
    public class FsRapResponse
    {
        /// <summary>
        /// it uniquely identifies this packet
        /// </summary>
        public ushort messageId;

        /// <summary>
        /// The response param
        /// </summary>
        public RapResponseParam TransParameters;

        /// <summary>
        /// The response data
        /// </summary>
        public RapResponseData TransData;
    }

    /// <summary>
    /// The type of fs endpoint
    /// </summary>
    public enum FsEndpointType
    {
        /// <summary>
        /// Tcp
        /// </summary>
        Tcp,

        /// <summary>
        /// NetBios
        /// </summary>
        NetBios
    }


    /// <summary>
    /// The endpoint of file service
    /// </summary>
    public class FsEndpoint
    {
        private FsEndpointType endpointType;
        private IPEndPoint ipEndpoint;
        private int netBiosEndpoint;


        /// <summary>
        /// Constructor
        /// </summary>
        public FsEndpoint()
        {

        }

        /// <summary>
        /// Constructor for netbios type
        /// </summary>
        public FsEndpoint(int netBiosEndpoint)
        {
            this.endpointType = FsEndpointType.NetBios;
            this.netBiosEndpoint = netBiosEndpoint;
        }


        /// <summary>
        /// Constructor for tcp type
        /// </summary>
        public FsEndpoint(IPEndPoint ipEndpoint)
        {
            this.endpointType = FsEndpointType.Tcp;
            this.ipEndpoint = ipEndpoint;
        }


        /// <summary>
        /// Tcp or NetBios
        /// </summary>
        public FsEndpointType EndpointType
        {
            get
            {
                return endpointType;
            }
            set
            {
                endpointType = value;
            }
        }


        /// <summary>
        /// if Endpoint type is tcp, this contains ip information
        /// </summary>
        public IPEndPoint IpEndpoint
        {
            get
            {
                return ipEndpoint;
            }
            set
            {
                ipEndpoint = value;
            }
        }

        /// <summary>
        /// If endpoint type is netbios, this contains sessionId
        /// </summary>
        public int NetBiosEndpoint
        {
            get
            {
                return netBiosEndpoint;
            }
            set
            {
                netBiosEndpoint = value;
            }
        }
    }

    #endregion
}
