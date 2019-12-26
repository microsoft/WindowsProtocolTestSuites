// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// File type defined in [MS-FSA] 3.1.1.3
    /// file type
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// data
        /// </summary>
        DataFile,

        /// <summary>
        /// directory
        /// </summary>
        DirectoryFile
    }

    /// <summary>
    /// Stream type defined in [MS-FSA] 3.1.1.5
    /// stream type
    /// </summary>
    public enum StreamType
    {
        /// <summary>
        /// data
        /// </summary>
        DataStream,

        /// <summary>
        /// directory
        /// </summary>
        DirectoryStream
    }

    /// <summary>
    /// File attributes defined in [MS-SMB2] section 2.2.13
    /// The following attributes are defined for files and directories. They can be used in any combination 
    /// unless noted in the description of the attribute's meaning. There is no file attribute with the value 
    /// 0x00000000 because a value of 0x00000000 in the FileAttributes field means that the file attributes for 
    /// this file MUST NOT be changed when setting basic information for the file.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum FileAttribute : uint
    {
        /// <summary>
        /// A file or directory that is read-only. For a file, applications can read the 
        /// file but cannot write to it or delete it. For a directory, applications cannot delete it, 
        /// but applications can create and delete files from that directory
        /// </summary>
        READONLY = 0x00000001,

        /// <summary>
        /// A file or directory that is hidden. Files and directories marked with this attribute do 
        /// not appear in an ordinary directory listing.
        /// </summary>
        HIDDEN = 0x00000002,

        /// <summary>
        /// A file or directory that the operating system uses a part of or uses exclusively.
        /// </summary>
        SYSTEM = 0x00000004,

        /// <summary>
        /// This item is a directory.
        /// </summary>
        DIRECTORY = 0x00000010,

        /// <summary>
        /// A file or directory that needs to be archived. Applications use this attribute to mark files for backup or removal.
        /// </summary>
        ARCHIVE = 0x00000020,

        /// <summary>
        /// A file that does not have other attributes set. This flag is used to clear all other 
        /// flags by specifying it with no other flags set. This flag MUST be ignored if other flags are set
        /// </summary>
        NORMAL = 0x00000080,

        /// <summary>
        /// A file that is being used for temporary storage. The operating system may choose to store this
        /// file's data in memory rather than on mass storage, writing the data to mass storage only if data 
        /// remains in the file when the file is closed.
        /// </summary>
        TEMPORARY = 0x00000100,

        /// <summary>
        /// A file that is a sparse file
        /// </summary>
        SPARSE_FILE = 0x00000200,

        /// <summary>
        /// A file or directory that has an associated reparse point
        /// </summary>
        REPARSE_POINT = 0x00000400,

        /// <summary>
        /// A file or directory that is compressed. For a file, all of the data in the file is compressed. 
        /// For a directory, compression is the default for newly created files and subdirectories
        /// </summary>
        COMPRESSED = 0x00000800,

        /// <summary>
        /// The data in this file is not available immediately. This attribute indicates that the file data 
        /// is physically moved to offline storage. This attribute is used by Remote Storage, which is hierarchical 
        /// storage management software.
        /// </summary>
        OFFLINE = 0x00001000,

        /// <summary>
        /// A file or directory that is not indexed by the content indexing service
        /// </summary>
        NOT_CONTENT_INDEXED = 0x00002000,

        /// <summary>
        /// A file or directory that is encrypted. For a file, all data streams in the file are encrypted. 
        /// For a directory, encryption is the default for newly created files and subdirectories
        /// </summary>
        ENCRYPTED = 0x00004000,

        /// <summary>
        /// not valid values for a file object as specified in MS-SMB2 section 2.2.13
        /// </summary>
        NOT_VALID_VALUE = 0x0008,

        /// <summary>
        /// A file or directory that is configured with integrity support. 
        /// For a file, all data streams in the file have integrity support. 
        /// For a directory, integrity support is the default for newly created files and subdirectories, unless the caller specifies otherwise
        /// </summary>
        INTEGRITY_STREAM = 0x00008000,

        /// <summary>
        /// A file or directory that is configured to be excluded from the data integrity scan. 
        /// For a directory configured with FILE_ATTRIBUTE_NO_SCRUB_DATA, 
        /// the default for newly created files and subdirectories is to inherit the FILE_ATTRIBUTE_NO_SCRUB_DATA attribute
        /// </summary>
        NO_SCRUB_DATA = 0x00020000,
    }

    /// <summary>
    /// Reparse tag defined in [MS-FSCC] section 2.1.2.1
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ReparseTag : uint
    {
        /// <summary>
        /// Initialize
        /// </summary>
        Initialize,
        /// <summary>
        /// Used for mount point.
        /// </summary>
        MOUNT_POINT = 0xA0000003,

        /// <summary>
        /// Obsolete. Used by legacy Hierarchical-Storage-Manager product.
        /// </summary>
        HSM = 0xC0000004,

        /// <summary>
        /// Obsolete. Used by legacy Hierarchical-Storage-Manager product
        /// </summary>
        HSM2 = 0x80000006,

        /// <summary>
        /// Home server drive extender, as defined in [MS-FSCC] section 2.1.2.1
        /// </summary>
        DRIVER_EXTENDER = 0x80000005,

        /// <summary>
        /// Used by Single Instance Storage filter driver. Server-side interpretation only, not meaningful over the wire.
        /// </summary>
        SIS = 0x80000007,

        /// <summary>
        /// Used by the DFS filter. The DFS is described in the Distributed File System (DFS): 
        /// Referral Protocol Specification [MS-DFSC]. Server-side interpretation only, not meaningful over the wire
        /// </summary>
        DFS = 0x8000000A,

        /// <summary>
        /// Used by the DFS filter. The DFS is described in [MS-DFSC]. Server-side interpretation only, not meaningful over the wire.
        /// </summary>
        DFSR = 0x80000012,

        /// <summary>
        /// Used by filter manager test harness
        /// </summary>
        FILTER_MANAGER = 0x8000000B,

        /// <summary>
        /// Used for symbolic link support.
        /// </summary>
        SYMLINK = 0xA000000C,

        /// <summary>
        /// Reserved reparse tag value.
        /// </summary>
        IO_REPARSE_TAG_RESERVED_ZERO = 0x00000000,

        /// <summary>
        /// Reserved reparse tag value.
        /// </summary>
        IO_REPARSE_TAG_RESERVED_ONE = 0x00000001,

        /// <summary>
        /// If ReparseTag is a non-Microsoft Reparse Tag
        /// </summary>
        NON_MICROSOFT_RANGE_TAG,

        /// <summary>
        /// If ReparseTag is empty
        /// </summary>
        EMPTY,

        /// <summary>
        /// Not equal to OpenFileReparseTag.
        /// </summary>
        NotEqualOpenFileReparseTag
    }

    /// <summary>
    /// File access defined in [MS-SMB2] section 2.2.13.1
    /// The access granted for this open as specified in [MS-SMB2] section 2.2.13.1
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags]
    public enum FileAccess : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// This value indicates the right to read data from the file or named pipe.
        /// </summary>
        FILE_READ_DATA = 0x00000001,

        /// <summary>
        /// This value indicates the right to write data into the file or named 
        /// pipe beyond the end of the file. 
        /// </summary>
        FILE_WRITE_DATA = 0x00000002,

        /// <summary>
        /// This value indicates the right to append data into the file or named pipe. 
        /// </summary>
        FILE_APPEND_DATA = 0x00000004,

        /// <summary>
        /// This value indicates the right to read the extended attributes of the file or named pipe. 
        /// </summary>
        FILE_READ_EA = 0x00000008,

        /// <summary>
        /// This value indicates the right to write or change the extended attributes to the file or named pipe. 
        /// </summary>
        FILE_WRITE_EA = 0x00000010,

        /// <summary>
        /// This value indicates the right to execute the file.
        /// </summary>
        FILE_EXECUTE = 0x00000020,

        /// <summary>
        /// This value indicates the right to read the attributes of the file. 
        /// </summary>
        FILE_READ_ATTRIBUTES = 0x00000080,

        /// <summary>
        /// This value indicates the right to change the attributes of the file.
        /// </summary>
        FILE_WRITE_ATTRIBUTES = 0x00000100,

        /// <summary>
        /// This value indicates the right to delete the file.
        /// </summary>
        DELETE = 0x00010000,

        /// <summary>
        /// This value indicates the right to read the security descriptor for the
        /// file or named pipe. 
        /// </summary>
        READ_CONTROL = 0x00020000,

        /// <summary>
        /// This value indicates the right to change the discretionary access 
        /// control list (DACL) in the security descriptor for the file or named 
        /// pipe. For the DACL data structure, see ACL in [MS-DTYP].
        /// </summary>
        WRITE_DAC = 0x00040000,

        /// <summary>
        /// This value indicates the right to change the owner in the security 
        /// descriptor for the file or named pipe. 
        /// </summary>
        WRITE_OWNER = 0x00080000,

        /// <summary>
        /// SMB2 clients set this flag to any value.<37> 
        /// SMB2 servers SHOULD<38> ignore this flag. 
        /// </summary>
        SYNCHRONIZE = 0x00100000,

        /// <summary>
        /// This value indicates the right to read or change the system access 
        /// control list (SACL) in the security descriptor for the file or named 
        /// pipe. For the SACL data structure, see ACL in [MS-DTYP].<39> 
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0x01000000,

        /// <summary>
        /// This value indicates that the client is requesting an open to the file 
        /// with the highest level of access the client has on this file. If no 
        /// access is granted for the client on this file, the server MUST fail the 
        /// open with STATUS_ACCESS_DENIED.
        /// </summary>
        MAXIMUM_ALLOWED = 0x02000000,

        /// <summary>
        /// This value indicates a request for all the access flags that are 
        /// previously listed except MAXIMUM_ALLOWED and ACCESS_SYSTEM_SECURITY. 
        /// </summary>
        GENERIC_ALL = 0x10000000,

        /// <summary>
        ///This value indicates a request for the following combination of 
        ///access flags listed above: FILE_READ_ATTRIBUTES| FILE_EXECUTE| SYNCHRONIZE| READ_CONTROL. 
        /// </summary>
        GENERIC_EXECUTE = 0x20000000,

        /// <summary>
        /// This value indicates a request for the following combination of 
        /// access flags listed above: FILE_WRITE_DATA| FILE_APPEND_DATA
        /// FILE_WRITE_ATTRIBUTES| FILE_WRITE_EA| SYNCHRONIZE| READ_CONTROL. 
        /// </summary>
        GENERIC_WRITE = 0x40000000,

        /// <summary>
        /// This value indicates a request for the following combination of 
        /// access flags listed above: FILE_READ_DATA| FILE_READ_ATTRIBUTES| FILE_READ_EA| SYNCHRONIZE| 
        /// READ_CONTROL. 
        /// </summary>
        GENERIC_READ = 0x80000000,

        /// <summary>
        /// This value indicates the right to enumerate the contents of the directory. 
        /// </summary>
        FILE_LIST_DIRECTORY = 0x00000001,

        /// <summary>
        /// This value indicates the right to create a file under the directory. 
        /// </summary>
        FILE_ADD_FILE = 0x00000002,

        /// <summary>
        /// This value indicates the right to add a sub-directory under the directory.
        /// </summary>
        FILE_ADD_SUBDIRECTORY = 0x00000004,

        /// <summary>
        /// This value indicates the right to traverse this directory if the server 
        /// enforces traversal checking. 
        /// </summary>
        FILE_TRAVERSE = 0x00000020,

        /// <summary>
        /// This value indicates the right to delete the files and directories within this directory. 
        /// </summary>
        FILE_DELETE_CHILD = 0x00000040,

        /// <summary>
        /// If any of the bits in the mask 0x0CE0FE00 are set, the operation MUST be failed with STATUS_ACCESS_DENIED
        /// </summary>
        INVALID_ACCESS_MASK = 0x0CE0FE00 

    }

    /// <summary>
    /// File mode type defined in [MS-FSCC] 2.4.24
    /// This information class is used to query or set the mode of the file
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum FileMode : uint
    {
        /// <summary>
        /// When set, any system services, file system drivers(FSDs), and drivers that write data to the 
        /// file must actually transfer the data into the file before any requested write operation is considered complete.
        /// </summary>
        WRITE_THROUGH = 0x00000002,

        /// <summary>
        /// When set, all accesses to the file will be sequential
        /// </summary>
        SEQUENTIAL_ONLY = 0x00000004,

        /// <summary>
        /// When set, the file cannot be cached or buffered in a driver's internal buffers.
        /// </summary>
        NO_INTERMEDIATE_BUFFERING = 0x00000008,

        /// <summary>
        /// When set, all operations on the file are performed synchronously. Any wait on
        /// behalf of the caller is subject to premature termination from alerts. 
        /// This flag also causes the I/O system to maintain the file position context.
        /// </summary>
        SYNCHRONOUS_IO_ALERT = 0x00000010,

        /// <summary>
        /// When set, all operations on the file are performed synchronously. Wait requests in the 
        /// system to synchronize I/O queuing and completion are not subject to alerts. This flag 
        /// also causes the I/O system to maintain the file position context.
        /// </summary>
        SYNCHRONOUS_IO_NONALERT = 0x00000020,

        /// <summary>
        /// When set, delete the file when the last handle to the file is closed.
        /// </summary>
        DELETE_ON_CLOSE = 0x00001000,
    }

    /// <summary>
    /// This enum was defined in [MS-LSAD] section 3.1.1.2.1.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SecurityContext : uint
    {
        /// <summary>
        /// It is not used
        /// </summary>
        None = 0,

        /// <summary>
        /// Restore files and directories.
        /// </summary>
        SERestoreName = 0x00000011,

        /// <summary>
        /// Back up files and directories.
        /// </summary>
        SEBackupName = 0x000000012,

        /// <summary>
        /// Manage auditing and security log.
        /// </summary>
        SESecurityName = 0x00000013,

        /// <summary>
        /// Manage the files access on a volume. 
        /// </summary>
        SEManageVolumeAccess,
    }

    /// <summary>
    /// Defined in [MS-SMB2] section 2.2.13
    /// A bitmask indicate sharing access for the open file, as specified in [MS-SMB2] section 2.2.13
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum ShareAccess : uint
    {
        /// <summary>
        /// This value indicates that this file is allowed to be read while it is present. 
        /// This bit MUST NOT be set for a named pipe or a printer file. Each open creates a new instance 
        /// of a named pipe. Likewise, opening a printer file always creates a new file
        /// </summary>
        FILE_SHARE_READ = 0x00000001,

        /// <summary>
        /// This value indicates that this file is allowed to be written while it is present. 
        /// This bit MUST NOT be set for a named pipe or a printer file. Each open creates a new instance of 
        /// a named pipe. Likewise, opening a printer file always creates a new file.
        /// </summary>
        FILE_SHARE_WRITE = 0x00000002,

        /// <summary>
        /// This value indicates that this file is allowed to be deleted or renamed while it 
        /// is present. This bit MUST NOT be set for a named pipe or a printer file. Each open creates a new 
        /// instance of a named pipe. Likewise, opening a printer file always creates a new file
        /// </summary>
        FILE_SHARE_DELETE = 0x00000004,

        /// <summary>
        /// not valid values for a file object as specified in MS-SMB2 section 2.2.13
        /// </summary>
        NOT_VALID_VALUE = 0x00000008
    }

    /// <summary>
    /// Defined in [MS-SMB2] section 2.2.13
    /// The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags]
    public enum CreateDisposition : uint
    {
        /// <summary>
        /// If the file already exists, supersede it. Otherwise, create the file. 
        /// This value SHOULD NOT be used for a printer object.
        /// </summary>
        SUPERSEDE = 0x00000000,

        /// <summary>
        /// If the file already exists, return success; otherwise, fail the operation. MUST NOT be used for a printer object.
        /// </summary>
        OPEN = 0x00000001,

        /// <summary>
        /// If the file already exists, fail the operation; otherwise, create the file.
        /// </summary>
        CREATE = 0x00000002,

        /// <summary>
        /// Open the file if it already exists; otherwise, create the file. This value
        /// SHOULD NOT be used for a printer object.
        /// </summary>
        OPEN_IF = 0x00000003,

        /// <summary>
        /// Overwrite the file if it already exists; otherwise, fail the operation. MUST NOT be used for a printer object.
        /// </summary>
        OVERWRITE = 0x00000004,

        /// <summary>
        /// Overwrite the file if it already exists; otherwise, create the file. 
        /// This value SHOULD NOT be used for a printer object.
        /// </summary>
        OVERWRITE_IF = 0x00000005,

        /// <summary>
        /// not valid values for a file object as specified in MS-SMB2 section 2.2.13
        /// </summary>
        NOT_VALID_VALUE = 0x00000010
    }

    /// <summary>
    /// Defined in [MS-SMB2] section 2.2.13
    /// A bitmask of options for the open, as specified in [MS-SMB2] section 2.2.13.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum CreateOptions : uint
    {
        /// <summary>
        /// This value indicate file is a directory file. With this flag, the 
        /// CreateDisposition field MUST be set to FILE_CREATE or FILE_OPEN_IF. With this flag, 
        /// only the following CreateOptions values are valid: FILE_WRITE_THROUGH, and FILE_OPEN_FOR_BACKUP_INTENT.
        /// </summary>
        DIRECTORY_FILE = 0x00000001,

        /// <summary>
        /// The server MUST propagate writes to this open to persistent storage before 
        /// return success to the client on write operations.
        /// </summary>
        WRITE_THROUGH = 0x00000002,

        /// <summary>
        /// This indicates that the application intends to read or write at sequential 
        /// offsets using this handle, so the server SHOULD optimize for sequential access. 
        /// However, the server MUST accept any access pattern. This flag value is 
        /// incompatible with the FILE_RANDOM_ACCESS value.
        /// </summary>
        SEQUENTIAL_ONLY = 0x00000004,

        /// <summary>
        /// The server or underlying object store SHOULD NOT cache data at intermediate 
        /// layers and SHOULD allow it to flow through to persistent storage.
        /// </summary>
        NO_INTERMEDIATE_BUFFERING = 0x00000008,

        /// <summary>
        /// This bit SHOULD be set to 0 and MUST be ignored by the server.
        /// </summary>
        SYNCHRONOUS_IO_ALERT = 0x00000010,

        /// <summary>
        /// This bit SHOULD be set to 0 and MUST be ignored by the server.
        /// </summary>
        SYNCHRONOUS_IO_NONALERT = 0x00000020,

        /// <summary>
        /// The file being opened MUST NOT be a directory file or this call
        /// MUST be failed. This flag MUST NOT be used with FILE_DIRECTORY_FILE.
        /// </summary>
        NON_DIRECTORY_FILE = 0x00000040,

        /// <summary>
        /// This bit SHOULD be set to 0 and MUST be ignored by the server.
        /// </summary>
        COMPLETE_IF_OPLOCKED = 0x00000100,

        /// <summary>
        /// The caller does not understand how to handle extended attributes. 
        /// If extended attributes are associated with the file being opened, the server MUST fail this request.
        /// </summary>
        NO_EA_KNOWLEDGE = 0x00000200,

        /// <summary>
        /// This indicates that the application intends to read or write at random offsets 
        /// using this handle, so the server SHOULD optimize for random access. However, the server MUST 
        /// accept any access pattern. This flag value is incompatible with the FILE_SEQUENTIAL_ONLY value.
        /// </summary>
        RANDOM_ACCESS = 0x00000800,

        /// <summary>
        /// The file SHOULD be automatically deleted when the last open request on this file is closed.
        /// When this option is set, the DesiredAccess field MUST include the DELETE flag. This option 
        /// is often used for temporary files. See file deletion semantics in [FBSO].
        /// </summary>
        DELETE_ON_CLOSE = 0x00001000,

        /// <summary>
        /// This bit SHOULD be set to 0 and the server MUST fail the request 
        /// with a STATUS_INVALID_PARAMETER error if this bit is set.
        /// </summary>
        OPEN_BY_FILE_ID = 0x00002000,

        /// <summary>
        /// The file is being opened for backup intent. That is, it is being opened or created for the 
        /// purposes of either a backup or a restore operation. Thus, the server MAY make appropriate 
        /// checks to ensure that the caller is capable of overriding whatever security checks have 
        /// been placed on the file to allow a backup or restore operation to occur. The server MAY 
        /// choose to check for certain access rights to the file before checking the DesiredAccess field.
        /// </summary>
        OPEN_FOR_BACKUP_INTENT = 0x00004000,

        /// <summary>
        /// The file cannot be compressed.
        /// </summary>
        NO_COMPRESSION = 0x00008000,

        /// <summary>
        /// This bit SHOULD be set to 0 and the server MUST fail the request with 
        /// a STATUS_INVALID_PARAMETER error if this bit is set.
        /// </summary>
        RESERVE_OPFILTER = 0x00100000,

        /// <summary>
        /// If the file or directory being opened is a reparse point, open the reparse 
        /// point itself rather than the target that the reparse point references.
        /// </summary>
        OPEN_REPARSE_POINT = 0x00200000,

        /// <summary>
        /// In an HSM (Hierarchical Storage Management) environment, this flag means the 
        /// file should not be recalled from tertiary storage such as tape. The recall can 
        /// take several minutes. The caller can specify this flag to avoid those delays.
        /// </summary>
        OPEN_NO_RECALL = 0x00400000,

        /// <summary>
        /// Open file to query for free space. The client SHOULD set this to 0 and the server MUST ignore it.
        /// </summary>
        OPEN_FOR_FREE_SPACE_QUERY = 0x00800000,

        /// <summary>
        /// not valid values for a file object as specified in MS-SMB2 section 2.2.13
        /// </summary>
        NOT_VALID_VALUE = 0x01000000
    }

    /// <summary>
    /// Defined in [MS-SMB2] section 2.2.14
    /// A code defining the action taken by the open operation, 
    /// as specified in [MS-SMB2] section 2.2.14 for the CreateAction field
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CreateAction : uint
    {
        /// <summary>
        /// An existing file was deleted and a new file was created in its place.
        /// </summary>
        SUPERSEDED = 0x00000000,

        /// <summary>
        /// An existing file was opened.
        /// </summary>
        OPENED = 0x00000001,

        /// <summary>
        /// A new file was created.
        /// </summary>
        CREATED = 0x00000002,

        /// <summary>
        /// An existing file was overwritten.
        /// </summary>
        OVERWRITTEN = 0x00000003,

        /// <summary>
        /// null
        /// </summary>
        NULL
    }

    /// <summary>
    /// Defined in [MS-ERREF] 2.3
    /// An NTSTATUS code that specifies the result
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum MessageStatus : uint
    {
        /// <summary>
        /// The operation completed successfully
        /// </summary>
        SUCCESS = 0x00000000,

        /// <summary>
        /// The operation that was requested is pending completion.
        /// </summary>
        PENDING = 0x00000103,

        /// <summary>
        /// The I/O request was canceled
        /// </summary>
        CANCELLED = 0xC0000120,

        /// <summary>
        /// {Still Busy} The specified I/O request packet (IRP) cannot be disposed of because the I/O operation is not complete.
        /// </summary>
        MORE_PROCESSING_REQUIRED = 0xC0000016,

        /// <summary>
        /// An invalid parameter was passed to a service or function
        /// </summary>
        INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// An invalid parameter was passed to a service or function as the first argument.
        /// </summary>
        INVALID_PARAMETER_1 = 0xC00000EF,

        /// <summary>
        /// An invalid parameter was passed to a service or function as the second argument.
        /// </summary>
        INVALID_PARAMETER_2 = 0xC00000F0,

        /// <summary>
        /// An invalid parameter was passed to a service or function as the third argument.
        /// </summary>
        INVALID_PARAMETER_3 = 0xC00000F1,

        /// <summary>
        /// An invalid parameter was passed to a service or function as the fourth argument.
        /// </summary>
        INVALID_PARAMETER_4 = 0xC00000F2,

        /// <summary>
        /// The specified request is not a valid operation for the target device
        /// </summary>
        INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// The request is not supported
        /// </summary>
        NOT_SUPPORTED = 0xC00000BB,

        /// <summary>
        /// {Incorrect Volume} The destination file of a rename request is located on
        /// a different device than the source of the rename request.
        /// </summary>
        NOT_SAME_DEVICE = 0xC00000D4,

        /// <summary>
        /// {Wrong Type} There is a mismatch between the type of object that is required by the requested operation and the type of object that is specified in the request.
        /// </summary>
        OBJECT_TYPE_MISMATCH = 0xC0000024,

        /// <summary>
        /// No more connections can be made to this remote computer at this time because the computer has 
        /// already accepted the maximum number of connections.
        /// </summary>
        REQUEST_NOT_ACCEPTED = 0xC00000D0,

        /// <summary>
        /// The volume must be upgraded to enable this feature.
        /// </summary>
        VOLUME_NOT_UPGRADED = 0xC000029C,

        /// <summary>
        /// {Access Denied} A process has requested access to an object but has not been granted those access rights
        /// </summary>
        ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// The remote user session has been deleted.
        /// </summary>
        USER_SESSION_DELETED = 0xC0000203,

        /// <summary>
        /// The client session has expired; so the client must re-authenticate to continue accessing the remote resources
        /// </summary>
        NETWORK_SESSION_EXPIRED = 0xC000035B,

        /// <summary>
        /// {Network Name Not Found} The specified share name cannot be found on the remote server.
        /// </summary>
        BAD_NETWORK_NAME = 0xC00000CC,

        /// <summary>
        /// The network name was deleted.
        /// </summary>
        NETWORK_NAME_DELETED = 0xC00000C9,

        /// <summary>
        /// The create operation stopped after reaching a symbolic link
        /// </summary>
        STOPPED_ON_SYMLINK = 0x8000002D,

        /// <summary>
        /// The object name is not found.
        /// </summary>
        OBJECT_NAME_NOT_FOUND = 0xC0000034,

        /// <summary>
        /// {Path Not Found} The path %hs does not exist.
        /// </summary>
        OBJECT_PATH_NOT_FOUND = 0xC000003A,

        /// <summary>
        /// The object name already exists.
        /// </summary>
        OBJECT_NAME_COLLISION = 0xC0000035,

        /// <summary>
        /// The object name is invalid.
        /// </summary>
        OBJECT_NAME_INVALID = 0xC0000033,

        /// <summary>
        /// An I/O request other than close and several other special case operations was 
        /// attempted using a file object that had already been closed
        /// </summary>
        FILE_CLOSED = 0xC0000128,

        /// <summary>
        /// Hash generation for the specified version and hash type is not enabled on server.
        /// </summary>
        HASH_NOT_SUPPORTED = 0xC000A088,

        /// <summary>
        /// The hash requests is not present or not up to date with the current file contents
        /// </summary>
        HASH_NOT_PRESENT = 0xC000A100,

        /// <summary>
        /// Insufficient system resources exist to complete the API
        /// </summary>
        INSUFFICIENT_RESOURCES = 0xC000009A,

        /// <summary>
        /// An operation was attempted to a volume after it was dismounted
        /// </summary>
        VOLUME_DISMOUNTED = 0xC000026E,

        /// <summary>
        /// A required attribute is missing from a directory for which encryption was requested.
        /// Defined in [MS-FSCC] section 2.3.48
        /// </summary>
        FILE_CORRUPT_ERROR = 0xC0000102,

        /// <summary>
        /// A notify change request is being completed and the information is not being returned in 
        /// the caller's buffer. The caller now needs to enumerate the files to find the changes
        /// </summary>
        NOTIFY_ENUM_DIR = 0x0000010C,

        /// <summary>
        /// {Inconsistent EA List} The extended attribute (EA) list is inconsistent
        /// </summary>
        EA_LIST_INCONSISTENT = 0x80000014,

        /// <summary>
        /// The specified information record length does not match the length that is required for the specified information class.
        /// </summary>
        INFO_LENGTH_MISMATCH = 0xC0000004,

        /// <summary>
        /// An EA operation failed because the name or EA index is invalid.
        /// </summary>
        NONEXISTENT_EA_ENTRY = 0xC0000051,

        /// <summary>
        /// {Invalid Mapping} An attempt was made to create a view for a section that is bigger than the section.
        /// </summary>
        INVALID_VIEW_SIZE = 0xC000001F,

        /// <summary>
        /// A non-close operation has been requested of a file object that has a delete pending.
        /// </summary>
        DELETE_PENDING = 0xC0000056,

        /// <summary>
        /// The buffer supplied to a function was too small
        /// </summary>
        BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// The file name is too long.
        /// </summary>
        BUFFER_OVERFLOW = 0x80000005,

        /// <summary>
        /// There is not enough space on the disk.
        /// </summary>
        DISK_FULL = 0xC000007F,

        /// <summary>
        /// The oplock request is denied
        /// </summary>
        LOCK_NOT_GRANTED = 0xC0000055,

        /// <summary>
        /// A requested read/write cannot be granted due to a conflicting file lock
        /// </summary>
        FILE_LOCK_CONFLICT = 0xC0000054,

        /// <summary>
        /// This indicates that a notify change request has been completed due to closing the handle which made the notify change request.
        /// </summary>
        NOTIFY_CLEANUP = 0x0000010B,

        /// <summary>
        /// {Invalid Parameter} The specified information class is not a valid information class for the specified object.
        /// </summary>
        INVALID_INFO_CLASS = 0xC0000003,

        /// <summary>
        /// {File Not Found} The file %hs does not exist.
        /// </summary>
        NO_SUCH_FILE = 0xC000000F,

        /// <summary>
        /// The file that was specified as a target is a directory, and the caller specified that it could be anything but a directory.
        /// </summary>
        FILE_IS_A_DIRECTORY = 0xC00000BA,

        /// <summary>
        /// The end-of-file marker has been reached. There is no valid data in the file beyond this marker
        /// </summary>
        END_OF_FILE = 0xC0000011,

        /// <summary>
        /// The device is not in a valid state to perform this request.
        /// </summary>
        INVALID_DEVICE_STATE = 0xC0000184,

        /// <summary>
        /// An invalid oplock acknowledgment was received by the system
        /// </summary>
        INVALID_OPLOCK_PROTOCOL = 0xC00000E3,

        /// <summary>
        /// The mounted file system does not support extended attributes.
        /// </summary>
        EAS_NOT_SUPPORTED = 0xC000004F,

        /// <summary>
        /// {Write Protect Error} The disk cannot be written to because it is write-protected. 
        /// Remove the write protection from the volume %hs in drive %hs.
        /// </summary>
        MEDIA_WRITE_PROTECTED = 0xC00000A2,

        /// <summary>
        /// The list of RPC servers available for auto-handle binding has been exhausted
        /// </summary>
        NO_MORE_ENTRIES = 0x8000001A,

        /// <summary>
        /// A requested opened file is not a directory.
        /// </summary>
        NOT_A_DIRECTORY = 0xC0000103,

        /// <summary>
        /// The file that was specified as a target is a directory, and the caller 
        /// specified that it could be anything but a directory
        /// </summary>
        IS_A_DIRECTORY = 0xC00000BA,

        /// <summary>
        /// An attempt has been made to remove a file or directory that cannot be deleted
        /// </summary>
        CANNOT_DELETE = 0xC0000121,

        /// <summary>
        /// A reparse should be performed by the Object Manager because the name of 
        /// the file resulted in a symbolic link
        /// </summary>
        REPARSE = 0x00000104,

        /// <summary>
        /// A file cannot be opened because the share access flags are incompatible
        /// </summary>
        SHARING_VIOLATION = 0xC0000043,

        /// <summary>
        /// The instruction at 0x%08lx referenced memory at 0x%08lx. The memory could not be %s.
        /// </summary>
        ACCESS_VIOLATION = 0xC0000005,

        /// <summary>
        /// There are no more files
        /// </summary>
        NO_MORE_FILES = 0x80000006,

        /// <summary>
        /// A requested file lock operation cannot be processed due to an invalid byte range.
        /// </summary>
        INVALID_LOCK_RANGE = 0xc00001A1,

        /// <summary>
        /// The range specified in NtUnlockFile was not locked
        /// </summary>
        RANGE_NOT_LOCKED = 0xC000007E,

        /// <summary>
        /// The Windows I/O reparse tag passed for the NTFS reparse point is invalid
        /// </summary>
        IO_REPARSE_TAG_INVALID = 0xC0000276,

        /// <summary>
        /// The user data passed for the NTFS reparse point is invalid
        /// </summary>
        IO_REPARSE_DATA_INVALID = 0xC0000278,

        /// <summary>
        /// The Windows I/O reparse tag does not match the one that is in the NTFS reparse point.
        /// </summary>
        IO_REPARSE_TAG_MISMATCH = 0xC0000277,

        /// <summary>
        /// The reparse attribute cannot be set because it is incompatible with an existing attribute.
        /// </summary>
        REPARSE_ATTRIBUTE_CONFLIC = 0xC00002B2,

        /// <summary>
        /// No system quota limits are specifically set for this account
        /// </summary>
        NO_QUOTAS_FOR_ACCOUNT = 0x00000516,

        /// <summary>
        /// The supplied user buffer is not valid for the requested operation
        /// </summary>
        INVALID_USER_BUFFER = 0x000006F8,

        /// <summary>
        /// An object ID was not found in the file
        /// </summary>
        OBJECTID_NOT_FOUND = 0xC00002F0,

        /// <summary>
        /// The file or directory is not a reparse point, in MS-FSCC 2.3.6
        /// </summary>
        NOT_A_REPARSE_POINT = 0xC0000275,

        /// <summary>
        /// An invalid HANDLE was specified
        /// </summary>
        INVALID_HANDLE = 0xC0000008,

        /// <summary>
        /// Compression is disabled for this volume
        /// </summary>
        COMPRESSION_DISABLED = 0xC0000426,

        /// <summary>
        /// A duplicate name exists on the network
        /// </summary>
        DUPLICATE_NAME = 0xC00000BD,

        /// <summary>
        /// Indicates that the directory trying to be deleted is not empty
        /// </summary>
        DIRECTORY_NOT_EMPTY = 0xC0000101,

        /// <summary>
        /// An I/O request other than close was performed on a file after it was deleted, which can only happen 
        /// to a request that did not complete before the last handle was closed via NtClose
        /// </summary>
        FILE_DELETED = 0xC0000123,

        /// <summary>
        /// The volume change journal is not active.se.
        /// </summary>
        JOURNAL_NOT_ACTIVE = 0xC00002B8,

        /// <summary>
        /// The file for which EAs were requested has no EAs.
        /// </summary>
        NO_EAS_ON_FILE = 0xC0000052,

        /// <summary>
        /// The reparse attribute cannot be set because it is incompatible with an existing attribute
        /// </summary>
        REPARSE_ATTRIBUTE_CONFLICT = 0xC00002B1,

        /// <summary>
        /// An error status returned when the opportunistic lock (oplock) request is denied.
        /// </summary>
        OPLOCK_NOT_GRANTED = 0xC00000E2,

        /// <summary>
        /// {Illegal EA} The specified extended attribute (EA) name contains at least one illegal character
        /// </summary>
        INVALID_EA_NAME = 0x80000013,

        /// <summary>
        /// An attempt was made to create more links on a file than the file system supports.
        /// </summary>
        TOO_MANY_LINKS = 0xC0000265,

        /// <summary>
        /// The specified quota list is internally inconsistent with its descriptor
        /// </summary>
        QUOTA_LIST_INCONSISTENT = 0xC0000266,

        /// <summary>
        /// A required privilege is not held by the client.
        /// </summary>
        PRIVILEGE_NOT_HELD = 0xC0000061,

        /// <summary>
        /// Short names are not enabled on this volume.
        /// </summary>
        SHORT_NAMES_NOT_ENABLED_ON_VOLUME = 0xC000019F,

        /// <summary>
        /// Indicates a particular security ID may not be assigned as the owner of an object
        /// </summary>
        INVALID_OWNER = 0xC000005A,

        /// <summary>
        /// STATUS_VOLUME_NOT_UPGRADED
        /// </summary>
        STATUS_VOLUME_NOT_UPGRADED = 0xC000029C,

        /// <summary>
        /// STATUS_OPLOCK_SWITCHED_TO_NEW_HANDLE in MS-FSA 3.1.5.17.1
        /// </summary>
        OPLOCK_SWITCHED_TO_NEW_HANDLE,

        /// <summary>
        /// STATUS_CANNOT_GRANT_REQUESTED_OPLOCK. in MS-FSA 3.1.5.18
        /// </summary>
        CANNOT_GRANT_REQUESTED_OPLOCK,

        /// <summary>
        /// If Open.File.ExtendedAttributesLength becomes greater than 64 KB - 5 bytes, 
        /// the object store MUST fail the operation with STATUS_EA_TOO_LARGE. in [MS-FSA] section 3.1.5.14.5
        /// </summary>
        STATUS_EA_TOO_LARGE = 0xC0000050,

        /// <summary>
        /// The requested operation is not implemented.
        /// </summary>
        STATUS_NOT_IMPLEMENTED = 0xC0000002,

        /// <summary>
        /// The object path component was not a directory object.
        /// </summary>
        STATUS_OBJECT_PATH_SYNTAX_BAD = 0xC000003B,

        /// <summary>
        /// A file system filter on the server has not opted in for Offload Read support.
        /// </summary>
        STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED = 0xC000A2A1,

        /// <summary>
        /// A file system filter on the server has not opted in for Offload Write support.
        /// </summary>
        STATUS_OFFLOAD_WRITE_FLT_NOT_SUPPORTED = 0xC000A2A2,

        /// <summary>
        /// Offload read operations cannot be performed on:
        /// Compressed files
        /// Sparse files
        /// Encrypted files
        /// File system metadata files
        /// </summary>
        STATUS_OFFLOAD_READ_FILE_NOT_SUPPORTED = 0xC000A2A3,

        /// <summary>
        /// Offload write operations cannot be performed on:
        /// Compressed files
        /// Sparse files
        /// Encrypted files
        /// File system metadata files
        /// </summary>
        STATUS_OFFLOAD_WRITE_FILE_NOT_SUPPORTED = 0xC000A2A4
    }

    /// <summary>
    /// Defined in [MS-FSCC] section 2.4
    /// The type of information being queried, as specified in [MS-FSCC] section 2.4
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum FileInfoClass : uint
    {
        /// <summary>
        /// null
        /// </summary>
        NONE = 0,

        FILE_DIRECTORY_INFORMATION = 1,

        FILE_FULL_DIR_INFORMATION = 2,

        FILE_BOTH_DIR_INFORMATION = 3,

        FILE_BASIC_INFORMATION = 4,

        FILE_STANDARD_INFORMATION = 5,

        FILE_INTERNAL_INFORMATION = 6,

        FILE_EA_INFORMATION = 7,

        FILE_ACCESS_INFORMATION = 8,

        FILE_NAME_INFORMATION = 9,

        FILE_RENAME_INFORMATION = 10,

        FILE_LINK_INFORMATION = 11,

        FILE_NAMES_INFORMATION = 12,

        FILE_DISPOSITION_INFORMATION = 13,

        FILE_POSITION_INFORMATION = 14,

        FILE_FULLEA_INFORMATION = 15,

        FILE_MODE_INFORMATION = 16,

        FILE_ALIGNMENT_INFORMATION = 17,

        FILE_ALL_INFORMATION = 18,

        FILE_ALLOCATION_INFORMATION = 19,

        FILE_ENDOFFILE_INFORMATION = 20,

        FILE_ALTERNATENAME_INFORMATION = 21,

        FILE_STREAM_INFORMATION = 22,

        FILE_PIPE_INFORMATION = 23,

        FILE_PIPELOCAL_INFORMATION = 24,

        FILE_PIPEREMOTE_INFORMATION = 25,

        FILE_COMPRESSION_INFORMATION = 28,

        FILE_OBJECTID_INFORMATION = 29,

        FILE_QUOTA_INFORMATION = 32,

        FILE_REPARSE_POINT_INFORMATION = 33,

        FILE_NETWORKOPEN_INFORMATION = 34,

        FILE_ATTRIBUTETAG_INFORMATION = 35,

        FILE_ID_BOTH_DIR_INFORMATION = 37,

        FILE_ID_FULL_DIR_INFORMATION = 38,

        FILE_VALIDDATALENGTH_INFORMATION = 39,

        FILE_SHORTNAME_INFORMATION = 40,

        FILE_SFIO_RESERVE_INFORMATION = 44,

        FILE_LINKS_INFORMATION = 46,

        FileNormalizedNameInformation = 48,

        FILE_ID_GLOBAL_TX_DIR_INFORMATION = 50,

        FILE_STANDARD_LINK_INFORMATION = 54,

        FileIdInformation = 59,

        /// <summary>
        /// If FileInformationClass is not defined in [MS-FSCC] section 2.4
        /// </summary>
        NOT_DEFINED_IN_FSCC

    }

    /// <summary>
    /// not defined in TD, and it was self-defined follow by the subtitle of [MS-FSA] 3.1.5.12
    /// file sysytem infomation
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FileSystemInfoClass : uint
    {
        /// <summary>
        /// Undefined Information class code in section 2.5 File System Information Classes
        /// Use for negative testing.
        /// </summary>
        Zero = 0,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsVolumeInformation = 1,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsLabelInformation = 2,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsSizeInformation = 3,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsDevice_Information = 4,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsAttribute_Information = 5,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsControlInformation = 6,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsFullSize_Information = 7,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsObjectId_Information = 8,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsDriverPath_Information = 9,

        /// <summary>
        /// as the title of the subsection of 3.1.5.12
        /// </summary>
        File_FsSectorSizeInformation = 11,

        /// <summary>
        /// Artificial information class for negative testing.
        /// </summary>
        NOT_DEFINED_IN_FSCC = 12
    }

    /// <summary>
    /// Defined in [MS-FSCC] section 2.4.3
    /// set to one of the alignment requirement values specified in [MS-FSCC] section
    /// 2.4.3 based on the characteristics of the device on which the File is stored.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum AlignmentRequirement : uint
    {
        /// <summary>
        /// If this value is specified, there are no alignment requirements for the device
        /// </summary>
        FileByteAlignment = 0x00000000,

        /// <summary>
        /// If this value is specified, data MUST be aligned on a 2-byte boundary
        /// </summary>
        FileWordAlignment = 0x00000001,

        /// <summary>
        /// If this value is specified, data MUST be aligned on a 4-byte boundary
        /// </summary>
        FileLongAlignment = 0x00000003,

        /// <summary>
        /// If this value is specified, data MUST be aligned on an 8-byte boundary.
        /// </summary>
        FileQuadAlignment = 0x00000007,

        /// <summary>
        /// If this value is specified, data MUST be aligned on a 16-byte boundary.
        /// </summary>
        FileOctaAlignment = 0x0000000f,

        /// <summary>
        /// If this value is specified, data MUST be aligned on a 32-byte boundary.
        /// </summary>
        File32ByteAlignment = 0x0000001f,

        /// <summary>
        /// If this value is specified, data MUST be aligned on a 64-byte boundary.
        /// </summary>
        File64ByteAlignment = 0x0000003f,

        /// <summary>
        /// If this value is specified, data MUST be aligned on a 128-byte boundary.
        /// </summary>
        File128ByteAlignment = 0x0000007f,

        /// <summary>
        /// If this value is specified, data MUST be aligned on a 256-byte boundary.
        /// </summary>
        File256ByteAlignment = 0x000000ff,

        /// <summary>
        /// If this value is specified, data MUST be aligned on a 512-byte boundary.
        /// </summary>
        File512ByteAlignment = 0x000001ff,

        /// <summary>
        /// It is not used
        /// </summary>
        None = 0
    }

    /// <summary>
    /// Defined in [MS-FSCC] in section 2.3.51
    /// A 32-bit unsigned integer value that indicates the operation to be performed.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum EncryptionOperation : uint
    {
        /// <summary>
        /// This operation requests encryption of the specified file or directory. 
        /// </summary>
        FILE_SET_ENCRYPTION = 0x00000001,

        /// <summary>
        /// This operation requests removal of encryption from the specified file or directory. 
        /// It MUST fail if any streams for the file are marked encrypted. 
        /// </summary>
        FILE_CLEAR_ENCRYPTION = 0x00000002,

        /// <summary>
        /// This operation requests encryption of the specified stream. 
        /// </summary>
        STREAM_SET_ENCRYPTION = 0x00000003,

        /// <summary>
        /// This operation requests encryption of the specified file or directory. 
        /// </summary>
        STREAM_CLEAR_ENCRYPTION = 0x00000004,

        /// <summary>
        /// is not one of the predefined values in [MS-FSCC] section 2.3.47.
        /// </summary>
        NOT_VALID_IN_FSCC,

        /// <summary>
        /// It is not used
        /// </summary>
        None = 0
    }

    /// <summary>
    /// this enum was not defined in TD, it was self-defined 
    /// according to [MS-FSCC] 2.3
    /// FsControl type
    /// </summary>
    public enum FsControlRequestType
    {
        /// <summary>
        /// Sets the object identifier for the file or directory 
        /// associated with the handle on which this FSCTL was invoked, as specified in [MS-FSCC] 2.3.
        /// </summary>
        SET_OBJECT_ID = 0x90098,

        /// <summary>
        /// Requests that the server set the extended information for the file or directory 
        /// associated with the handle on which this FSCTL was invoked, as specified in [MS-FSCC] 2.3.
        /// </summary>
        SET_OBJECT_ID_EXTENDED = 0x900bc,

        /// <summary>
        /// Requests that the server indicate whether the specified pathname is well-formed 
        /// (of acceptable length, with no invalid characters, and so on - see section 2.1.5) 
        /// with respect to the volume that contains the file or directory 
        /// associated with the handle on which this FSCTL was invoked, as specified in [MS-FSCC] 2.3.
        /// </summary>
        IS_PATHNAME_VALID = 0x9002c,

        /// <summary>
        /// as title of the subsection in 3.1.5.9 
        /// </summary>
        //LMR_GET_LINK_TRACKING_INFORMATION = 0x1400e8,

        /// <summary>
        /// as title of the subsection in 3.1.5.9 
        /// </summary>
        //LMR_SET_LINK_TRACKING_INFORMATION = 0x1400ec,

        /// <summary>
        /// Requests that the server return the first 0x24 bytes of sector 0 for the volume 
        /// that contains the file or directory associated with the handle on which this FSCTL was invoked.
        /// </summary>
        QUERY_FAT_BPB = 0x90058,

        /// <summary>
        /// Requests UDF-specific volume information for the volume that contains the file or directory
        /// associated with the handle on which this FSCTL was invoked.
        /// </summary>
        QUERY_ON_DISK_VOLUME_INFO = 0x9013c,

        /// <summary>
        /// Retrieves the defect management properties of the volume 
        /// that contains the file or directory associated with the handle on which this FSCTL was invoked.
        /// </summary>
        QUERY_SPARING_INFO = 0x90138,

        /// <summary>
        /// Requests that the server recall the file (associated with the handle on which this FSCTL was invoked)
        /// from storage media that Remote Storage manages. 
        /// </summary>
        RECALL_FILE = 0x90117,

        /// <summary>
        /// as title of the subsection in 3.1.5.9 
        /// </summary>
        SET_SHORT_NAME_BEHAVIOR = 0x901b4,

        /// <summary>
        /// Requests that the server fill the clusters of the target file with zeros when they are deallocated
        /// </summary>
        SET_ZERO_ON_DEALLOCATION = 0x90194,

        /// <summary>
        /// requests that the server generate a record in the server's file system change journal stream 
        /// for the file or directory associated with the handle on which this FSCTL was invoked, 
        /// indicating that the file or directory was closed.
        /// </summary>
        WRITE_USN_CLOSE_RECORD = 0x900ef,

        /// <summary>
        /// as title of the subsection in 3.1.5.9 
        /// </summary>
        GET_NTFS_VOLUME_DATA = 0x90064,

        /// <summary>
        /// Requests that the server return the current compression state of the file or directory 
        /// associated with the handle on which this FSCTL was invoked.
        /// </summary>
        FSCTL_GET_COMPRESSION,

        /// <summary>
        /// as title of the subsection in 3.1.5.9 
        /// </summary>
        FSCTL_FILESYSTEM_GET_STATISTICS,

        /// <summary>
        /// Requests that the server return the statistical information of the file system 
        /// such as Type, Version, and so on, as specified in FSCTL_FILESYSTEM_GET_STATISTICS reply,
        /// for the file or directory associated with the handle on which this FSCTL was invoked.
        /// </summary>
        FSCTL_SET_SHORT_NAME_BEHAVIOR,

        /// <summary>
        /// Requests a Byte-Range Lock
        /// </summary>
        RequestsbyteRangeLock,

        /// <summary>
        /// Requests an Unlock of a Byte-Range
        /// </summary>
        RequestsunlockByteRange,

        None = 0


    }

    /// <summary>
    /// Defined in [MS-FSCC] section 2.4.9
    /// A 16-bit unsigned integer that contains the compression format. The actual compression operation 
    /// associated with each of these compression format values is implementation-dependent.
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CompressionState : uint
    {
        /// <summary>
        /// The file or directory is not compressed
        /// </summary>
        CompressionFormatNone = 0x0000,

        /// <summary>
        /// The file or directory is compressed by using the default compression algorithm
        /// </summary>
        CompressionFormatDefault = 0x0001,

        /// <summary>
        /// The file or directory is compressed by using the LZNT1 compression algorithm
        /// </summary>
        CompressionFormatLznt1 = 0x0002,

        /// <summary>
        /// Reserved for future use
        /// </summary>
        AllOtherValues
    }

    /// <summary>
    /// A set of privilege names, as specified in [MS-LSAD] section 3.1.1.2.1, 
    /// representing the privileges held by the user.
    /// </summary>
    [Flags]
    public enum PrivilegeSet
    {
        /// <summary>
        /// Back up files and directories. 
        /// </summary>
        SeBackupPrivilege = 0x0001,

        /// <summary>
        /// Restore files and directories. 
        /// </summary>
        SeRestorePrivilege = 0x0002,

        /// <summary>
        /// Create symbolic links. 
        /// </summary>
        SeCreateSymbolicLinkPrivilege = 0x0004,

        /// <summary>
        /// Manage the files on a volume. 
        /// </summary>
        SeManageVolumePrivilege = 0x0008,

        /// <summary>
        /// Manage auditing and security log. 
        /// </summary>
        SeSecurityPrivilege = 0x0010,

        /// <summary>
        /// Take ownership of files or other objects. 
        /// </summary>
        SeTakeOwnershipPrivilege = 0x0020,

        /// <summary>
        /// It is not used
        /// </summary>
        None = 0
    }

    /// <summary>
    /// SecurityContext
    /// </summary>
    public struct SSecurityContext
    {
        /// <summary>
        /// A set of privilege names, as specified in [MS-LSAD] section 3.1.1.2.1, 
        /// representing the privileges held by the user.
        /// </summary>
        public PrivilegeSet privilegeSet;

        /// <summary>
        /// if SecurityContext.SIDs contains the well-known SID BUILTIN_ADMINISTRATORS as defined 
        /// in [MS-DTYP] section 2.4.2.2.
        /// </summary>
        public bool isSecurityContextSIDsContainWellKnown;

        /// <summary>
        /// True: if the object store implements encryption used in 3.1.5.1.2
        /// </summary>
        public bool isImplementsEncryption;
    }

    /// <summary>
    /// Defined in [MS-FSA] section 3.1.5.17
    /// The type of oplock being requested.
    /// </summary>
    public enum OpType
    {
        /// <summary>
        /// Corresponds to SMB2_OPLOCK_LEVEL_II as described in [MS-SMB2] section 2.2.13.
        /// </summary>
        LEVEL_TWO,

        /// <summary>
        /// Corresponds to SMB2_OPLOCK_LEVEL_EXCLUSIVE as described in [MS-SMB2] section 2.2.13.
        /// </summary>
        LEVEL_ONE,

        /// <summary>
        /// Corresponds to SMB2_OPLOCK_LEVEL_BATCH as described in [MS-SMB2] section 2.2.13.
        /// </summary>
        LEVEL_BATCH,

        /// <summary>
        /// (Corresponds to SMB2_OPLOCK_LEVEL_LEASE as described in [MS-SMB2] section 2.2.13.) 
        /// If this oplock type is specified, the server MUST additionally provide the RequestedOploclLevel parameter
        /// </summary>
        LEVEL_GRANULAR
    }

    /// <summary>
    /// Defined in [MS-FSA] section 3.1.1.10 per oplock
    /// The current state of the oplock, expressed as a combination of one or more flags
    /// </summary>
    [Flags]
    public enum OplockState
    {
        /// <summary>
        /// indicates that this Oplock does not represent a currently granted or breaking oplock. 
        /// This is semantically equivalent to the Oplock object being entirely absent from a Stream. This flag always appears alone.
        /// </summary>
        NO_OPLOCK = 0x00000001,

        /// <summary>
        /// Indicates that this Oplock represents a Level 1 (also called Exclusive) oplock.
        /// </summary>
        LEVEL_ONE_OPLOCK = 0x00000002,

        /// <summary>
        /// Indicates that this Oplock represents a Batch oplock
        /// </summary>
        BATCH_OPLOCK = 0x00000004,

        /// <summary>
        /// Indicates that this Oplock represents a Level 2 (shared) oplock.
        /// </summary>
        LEVEL_TWO_OPLOCK = 0x00000008,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that can be held by exactly one client at 
        /// a time. This flag always appears in combination with other flags that indicate the actual 
        /// oplock level. For example, (READ_CACHING|WRITE_CACHING|EXCLUSIVE) represents a read-caching 
        /// and write-caching oplock, which can be held by only one client at a time.
        /// </summary>
        EXCLUSIVE = 0x00000010,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that is currently breaking from either Level 1
        /// or Batch to Level 2; the oplock has broken but the break has not yet been acknowledged.
        /// </summary>
        BREAK_TO_TWO = 0x00000020,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that is currently breaking from either Level 1 
        /// or Batch to None (that is, no oplock); the oplock has broken but the break has not yet been acknowledged.
        /// </summary>
        BREAK_TO_NONE = 0x00000040,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that is currently breaking from either Level 1 or Batch 
        /// to None (that is, no oplock), and was previously breaking from Level 1 or Batch to Level 2; the oplock
        /// has broken but the break has not yet been acknowledged.
        /// </summary>
        BREAK_TO_TWO_TO_NONE = 0x00000080,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that provides caching of reads; 
        /// this provides the SMB 2.1 read caching lease (see [MS-SMB2] section 2.2.13.2.8).
        /// </summary>
        READ_CACHING = 0x00000100,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that provides caching of handles; 
        /// this provides the SMB 2.1 handle caching lease (see [MS-SMB2] section 2.2.13.2.8).
        /// </summary>
        HANDLE_CACHING = 0x00000200,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that provides caching of writes;
        /// this provides the SMB 2.1 write caching lease (see [MS-SMB2] section 2.2.13.2.8).
        /// </summary>
        WRITE_CACHING = 0x00000400,

        /// <summary>
        /// Always appears together with READ_CACHING and HANDLE_CACHING. Indicates that this
        /// Oplock represents an oplock on which at least one client has been granted a 
        /// read-caching oplock, and at least one other client has been granted a read-caching and handle-caching oplock.
        /// </summary>
        MIXED_R_AND_RH = 0x00000800,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that is currently breaking to an oplock that 
        /// provides caching of reads; the oplock has broken but the break has not yet been acknowledged.
        /// </summary>
        BREAK_TO_READ_CACHING = 0x00001000,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that is currently breaking to an oplock that 
        /// provides caching of writes; the oplock has broken but the break has not yet been acknowledged.
        /// </summary>
        BREAK_TO_WRITE_CACHING = 0x00002000,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that is currently breaking to an oplock that 
        /// provides caching of handles; the oplock has broken but the break has not yet been acknowledged.
        /// </summary>
        BREAK_TO_HANDLE_CACHING = 0x00004000,

        /// <summary>
        /// Indicates that this Oplock represents an oplock that is currently breaking to None 
        /// (that is, no oplock); the oplock has broken but the break has not yet been acknowledged.
        /// </summary>
        BREAK_TO_NO_CACHING = 0x00008000,

        /// <summary>
        /// empty
        /// </summary>
        Empty
    }

    /// <summary>
    /// defined in [MS-FSA] section 3.1.5.18
    /// A combination of zero or more of the following flags, which are only given 
    /// for LEVEL_GRANULAR Type Oplocks
    /// </summary>
    /// Disable warning CA1008 is to avoid changes in model.
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags]
    public enum RequestedOplockLevel
    {
        /// <summary>
        /// null
        /// </summary>
        ZERO = 0x00000000,

        /// <summary>
        /// RequestedOplockLevel: READ_CACHING
        /// </summary>
        READ_CACHING = 0x00000001,

        /// <summary>
        /// RequestedOplockLevel: HANDLE_CACHING
        /// </summary>
        HANDLE_CACHING = 0x00000002,

        /// <summary>
        /// RequestedOplockLevel: WRITE_CACHING
        /// </summary>
        WRITE_CACHING = 0x00000004
    }

    /// <summary>
    /// Defined in [MS-FSA] section 3.1.5.17
    ///  The type of oplock the requested oplock has been broken to
    /// </summary>
    public enum NewOplockLevel
    {
        /// <summary>
        /// invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// LEVEL_NONE
        /// </summary>
        LEVEL_NONE,

        /// <summary>
        /// LEVEL_TWO
        /// </summary>
        LEVEL_TWO,

        /// <summary>
        /// READ_CACHING
        /// </summary>
        READ_CACHING,

        /// <summary>
        /// WRITE_CACHING
        /// </summary>
        WRITE_CACHING,

        /// <summary>
        /// HANDLE_CACHING
        /// </summary>
        HANDLE_CACHING,

        /// <summary>
        /// READ_CACHING|WRITE_CACHING
        /// </summary>
        READ_WRITE_CACHING,

        /// <summary>
        /// READ_CACHING|HANDLE_CACHING
        /// </summary>
        READ_HANDLE_CACHING,

        /// <summary>
        /// WRITE_CACHING|HANDLE_CACHING
        /// </summary>
        WRITE_HANDLE_CACHING,

        /// <summary>
        /// READ_CACHING|WRITE_CACHING|HANDLE_CACHING
        /// </summary>
        READ_WRITE_HANDEL_CACHING
    }

    /// <summary>
    /// SUT platformType
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// Windows server
        /// </summary>
        Windows,

        /// <summary>
        /// Not windows server
        /// </summary>
        NoneWindows
    }

    /// <summary>
    /// FSStates: used to control all the process.
    /// </summary>
    public enum FSStates
    {
        /// <summary>
        /// Ready to initial
        /// </summary>
        ReadyInitial,

        /// <summary>
        /// had initialized
        /// </summary>
        FinsihInitial,

        /// <summary>
        /// error, indicates the logic is no corresponding to TD
        /// </summary>
        Error,

        /// <summary>
        /// complete successfully
        /// </summary>
        END
    }

    /// <summary>
    /// used to control the get sut info process
    /// </summary>
    public enum SutOSInfo
    {
        /// <summary>
        /// ready
        /// </summary>
        ReadyGetSutInfo,

        /// <summary>
        /// Request to get sut info
        /// </summary>
        RequestGetSutInfo,

        /// <summary>
        /// finish
        /// </summary>
        FinishGetSutInfo
    }

    /// <summary>
    /// this enum was not defined in TD, it was self-defined to use in [MS-FSA] 3.1.5.5.2
    /// the state of SidList in 3.1.5.5.2
    /// </summary>
    public enum SidListState
    {
        /// <summary>
        /// empty
        /// </summary>
        Empty,

        /// <summary>
        /// not empty
        /// </summary>
        NotEmpty,

        /// <summary>
        /// SidList has more than one entries
        /// </summary>
        HasMoreThanOneEntry,

        /// <summary>
        /// SidList has zero or one entry
        /// </summary>
        HasZeroorOneEntry,

        /// <summary>
        /// SidList.Length (0 is a valid length) is not a multiple of 4
        /// </summary>
        NotMultipleofFour,

        /// <summary>
        /// NotEmpty and NotMultipleofFour
        /// </summary>
        NotEmpty_NotMultipleofFour,

        /// <summary>
        /// If SidList contains a single entry Sid
        /// </summary>
        //ContainsASingleEntrySid,

        /// <summary>
        /// HasZeroorOneEntry and ContainsASingleEntrySid
        /// </summary>
        //HasZeroorOneEntry_ContainsASingleEntrySid,

        /// <summary>
        /// skip
        /// </summary>
        Skip
    }

    /// <summary>
    /// An implementation-specific identifier that is unique for each outstanding 
    /// IO operation. See [MS-CIFS] section 3.3.5.51
    /// </summary>
    public enum IORequest
    {
        /// <summary>
        /// Server Requests an Open of a File
        /// </summary>
        Open,

        /// <summary>
        /// Server Requests a Read
        /// </summary>
        Read,

        /// <summary>
        /// Server Requests a Write
        /// </summary>
        Write,

        /// <summary>
        /// Server Requests Closing an Open
        /// </summary>
        CloseOpen,

        /// <summary>
        /// Server Requests Querying a Directory
        /// </summary>
        QueryDirectory,

        /// <summary>
        /// Server Requests Flushing Cached Data
        /// </summary>
        FlushingCachedData,

        /// <summary>
        /// Server Requests a Byte-Range Lock
        /// </summary>
        ByteRangeLock,

        /// <summary>
        /// Server Requests an Unlock of a Byte-Range
        /// </summary>
        UnlockByteRange,

        /// <summary>
        /// Server Requests an FsControl Request
        /// </summary>
        FSControl,

        /// <summary>
        /// Server Requests Change Notifications for a Directory
        /// </summary>
        ChangeNotifications,

        /// <summary>
        /// Server Requests a Query of File Information
        /// </summary>
        QueryFileInformation,

        /// <summary>
        /// Server Requests a Query of File System Information
        /// </summary>
        QueryFileSystemInformation,

        /// <summary>
        /// Server Requests a Query of Security Information
        /// </summary>
        QuerySecurityInformation,

        /// <summary>
        /// 3.1.5.14 Server Requests Setting of File Information
        /// </summary>
        SettingFileInformation,

        /// <summary>
        /// 3.1.5.15 Server Requests Setting of File System Information
        /// </summary>
        SettingFileSystemInformation,

        /// <summary>
        /// 3.1.5.16 Server Requests Setting of Security Information
        /// </summary>
        SettingSecurityInformation,

        /// <summary>
        /// 3.1.5.17 Server Requests an Oplock
        /// </summary>
        Oplock,

        /// <summary>
        /// 3.1.5.18 Server Acknowledges an Oplock Break
        /// </summary>
        AcknowledgesOplockBreak,

        /// <summary>
        /// 3.1.5.19 Server Requests Canceling an Operation
        /// </summary>
        CancelingOperation
    }

    /// <summary>
    /// This enum was not defined in TD, it was self-defined to use in [MS-FSA] 3.1.5.5.4
    /// The state of OutBufferSize in subsection of section 3.1.5.5.4
    /// </summary>
    public enum OutBufferSmall
    {
        /// <summary>
        /// If OutputBufferSize is smaller than FieldOffset( FILE_BOTH_DIR_INFORMATION.FileName )
        /// </summary>
        FileBothDirectoryInformation,

        /// <summary>
        /// If OutputBufferSize is smaller than FieldOffset( FILE_DIRECTORY_INFORMATION.FileName )
        /// </summary>
        FileDirectoryInformation,

        /// <summary>
        /// If OutputBufferSize is smaller than FieldOffset( FILE_FULL_DIR_INFORMATION.FileName ), the 
        /// operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH
        /// </summary>
        FileFullDirectoryInformation,

        /// <summary>
        /// If OutputBufferSize is smaller than FieldOffset( FILE_ID_BOTH_DIR_INFORMATION.FileName )
        /// </summary>
        FileIdBothDirectoryInformation,

        /// <summary>
        /// If OutputBufferSize is smaller than FieldOffset( FILE_ID_FULL_DIR_INFORMATION.FileName )
        /// </summary>
        FileIdFullDirectoryInformation,

        /// <summary>
        /// If OutputBufferSize is smaller than FieldOffset( FILE_NAMES_INFORMATION.FileName ), the 
        /// operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH
        /// </summary>
        FileNamesInformation,

        /// <summary>
        /// null
        /// </summary>
        None
    }

    /// <summary>
    /// Input buffer compression state
    /// </summary>
    public enum InputBufferCompressionState
    {
        COMPRESSION_FORMAT_NONE,
        NotPrefinedValue
    }

    /// <summary>
    /// File system
    /// </summary>
    public enum FileSystem
    {
        /// <summary>
        ///  FAT file system
        /// </summary>
        FAT = 0,

        /// <summary>
        /// FAT32 file system
        /// </summary>
        FAT32 = 1,

        /// <summary>
        /// NTFS file system
        /// </summary>
        NTFS = 2,

        /// <summary>
        /// UDFS file system
        /// </summary>
        UDFS = 3,

        /// <summary>
        /// CDFS file system
        /// </summary>
        CDFS = 4,

        /// <summary>
        /// EXFAT file system
        /// </summary>
        EXFAT = 5,

        /// <summary>
        /// Resilient File System (ReFS)
        /// </summary>
        REFS = 6,

        /// <summary>
        /// Cluster Shared Volume file system (CSVFS)
        /// </summary>
        CSVFS = 7,

        /// <summary>
        /// None of the above file systems
        /// </summary>
        OTHERFS = 8,
    }

    /// <summary>
    /// The transport used to connect to SUT.
    /// </summary>
    public enum Transport
    {
        /// <summary>
        /// SMB transport
        /// </summary>
        SMB = 0,

        /// <summary>
        /// SMB2 transport
        /// </summary>
        SMB2 = 1,

        /// <summary>
        /// SMB3 transport
        /// </summary>
        SMB3 = 2
    }

    /// <summary>
    /// An array of bytes containing a single SI_COPYFILE structure indicating the source and destination files to copy
    /// </summary>
    public enum InputBuffer
    {
        /// <summary>
        /// A string
        /// </summary>
        SourceFileName,

        /// <summary>
        /// A string
        /// </summary>
        DestinationFileName
    }

    /// <summary>
    /// Per Open
    /// </summary>
    public struct Open
    {
        /// <summary>
        /// The Link through which File is opened. Link MUST be an element of File.LinkList.
        /// </summary>
        public string Link;

        /// <summary>
        /// The Stream that is opened. Stream MUST be an element of File.StreamList.
        /// </summary>
        public string Stream;

        /// <summary>
        /// A GUID value
        /// </summary>
        public Guid OplockKey;

        /// <summary>
        /// The Open that represents the root of the share
        /// </summary>
        public string RootOpen;

    }

    /// <summary>
    /// fileNameStatus used in 3.1.5.1
    /// </summary>
    public enum FileNameStatus
    {
        /// <summary>
        /// If set, PathName contains a trailing backslash.
        /// </summary>
        BackslashName,

        /// <summary>
        /// If set, PathName is valid as specified in [MS-FSCC] section 2.1.5
        /// </summary>
        NotPathNameValid,

        /// <summary>
        /// If set, if any StreamTypeNamei is "$INDEX_ALLOCATION" and the corresponding 
        /// StreamNamei has a value other than an empty string or "$I30". Describe in MS-FSA 3.1.5.1.
        /// </summary>
        StreamTypeNameIsINDEX_ALLOCATION,

        /// <summary>
        /// If set, search ParentFile.DirectoryList for a link where Link.Name or Link.ShortName 
        //matches FileNamei, If no such link is found
        /// </summary>
        isprefixLinkNotFound,

        /// <summary>
        /// If set, Open.File is not null.
        /// </summary>
        OpenFileNotNull,

        /// <summary>
        /// If set, PathName contains a trailing backslash.
        /// </summary>
        PathNameTraiblack,

        /// <summary>
        /// File name is null
        /// </summary>
        FileNameNull,

        /// <summary>
        /// Stream name is null
        /// </summary>
        StreamNameNull,

        /// <summary>
        /// Path name is valid.
        /// </summary>
        PathNameValid,

        /// <summary>
        /// Normal pathname
        /// </summary>
        Normal,

        /// <summary>
        /// Other
        /// </summary>
        Other
    }

    /// <summary>
    /// StreamTypeNameToOpen used in 3.1.5.1
    /// </summary>
    public enum StreamTypeNameToOpen
    {
        /// <summary>
        /// "$INDEX_ALLOCATION"
        /// you can consider it as DirectoryStream.
        /// </summary>
        INDEX_ALLOCATION,

        /// <summary>
        /// "$DATA"
        /// you can consider it as DataStream.
        /// </summary>
        DATA,

        /// <summary>
        /// other value and not empty
        /// </summary>
        Other,

        /// <summary>
        /// null
        /// </summary>
        NULL
    }

    /// <summary>
    /// FileNamePattern used in 3.1.5.5
    /// </summary>
    public enum FileNamePattern
    {
        /// <summary>
        /// Skip.
        /// </summary>
        Skip,

        /// <summary>
        /// FileNamePattern is empty
        /// </summary>
        Empty,

        /// <summary>
        /// FileNamePattern is not empty
        /// </summary>
        NotEmpty,

        /// <summary>
        /// NotEmpty and LengthIsNotAMultipleOf4
        /// </summary>
        NotEmpty_LengthIsNotAMultipleOf4,

        /// <summary>
        /// FileNamePattern.Length (0 is a valid length) is not a multiple of 4
        /// </summary>
        LengthIsNotAMultipleOf4,

        /// <summary>
        /// If FileNamePattern is not a valid filename component as described in [MS-FSCC] section 2.1.5, 
        /// with the exceptions that wildcard characters described in section 3.1.4.3 are permitted and the 
        /// strings "." and ".." are permitted
        /// </summary>
        NotValidFilenameComponent,

        /// <summary>
        /// "*"
        /// </summary>
        IndicateAll,

        /// <summary>
        /// Length is less than the size of an ObjectId
        /// </summary>
        LengthIsLessThanTheSizeOfAnObjectId
    }

    /// <summary>
    /// It indicates the Stream in 3.1.1.5
    /// </summary>
    public struct Stream
    {
        /// <summary>
        /// It indicates null, just a signal.
        /// </summary>
        public bool IsNull;
    }

    /// <summary>
    /// ByteRangeLock in 3.1.5.7
    /// </summary>
    public struct ByteRangeLock
    {
        /// <summary>
        /// A 64-bit unsigned integer containing the starting offset, in bytes.
        /// </summary>
        public long LockOffset;

        /// <summary>
        /// A 64-bit unsigned integer containing the length, in bytes. This value MAY be zero
        /// </summary>
        public long LockLength;

        /// <summary>
        /// A Boolean indicating whether the range is to be locked exclusively (TRUE) or 
        /// shared (FALSE)
        /// </summary>
        public bool IsExclusive;

        // Disable warning CA1801 because parameter cannot be deleted.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public ByteRangeLock(long lockOffset, long lockLength, bool isExclusive)
        {
            this.IsExclusive = false;
            this.LockLength = -1;
            this.LockOffset = -1;
        }
    }

    /// <summary>
    ///  Indicate buffer size
    /// </summary>
    public enum BufferSize
    {
        /// <summary>
        /// Less than FILE_OBJECTID_BUFFER.
        /// </summary>
        LessThanFILE_OBJECTID_BUFFER,

        /// <summary>
        /// Less than FILE_NAME_INFORMATION
        /// </summary>
        LessThanFILE_NAME_INFORMATION,

        /// <summary>
        /// Less than NTFS_VOLUME_DATA_BUFFER
        /// </summary>
        LessThanNTFS_VOLUME_DATA_BUFFER,

        /// <summary>
        /// Less than two bytes.
        /// </summary>
        LessThanTwoBytes,

        /// <summary>
        /// Less than sizeof(FILESYSTEM_STATISTICS)
        /// </summary>
        LessThanSizeOf_FILESYSTEM_STATISTICS,

        /// <summary>
        /// Less than total size of Statistics.
        /// </summary>
        LessThanTotalSizeOfStatistics,

        /// <summary>
        /// Less than 0x24
        /// </summary>
        LessThan0x24,

        /// <summary>
        /// Less than FILE_QUERY_ON_DISK_VOL_INFO_BUFFER.
        /// </summary>
        LessThanFILE_QUERY_ON_DISK_VOL_INFO_BUFFER,

        /// <summary>
        /// Less than FILE_QUERY_SPARING_BUFFER.
        /// </summary>
        LessThanFILE_QUERY_SPARING_BUFFER,

        /// <summary>
        /// Less than size of USN.
        /// </summary>
        LessThanSizeofUsn,

        /// <summary>
        /// Less than STARTING_VCN_INPUT_BUFFER.
        /// </summary>
        LessThanSTARTING_VCN_INPUT_BUFFER,

        /// <summary>
        /// Less than FILE_ALLOCATED_RANGE_BUFFER.
        /// </summary>
        LessThanFILE_ALLOCATED_RANGE_BUFFER,

        /// <summary>
        /// Less than REPARSE_DATA_BUFFER.
        /// </summary>
        LessThanREPARSE_DATA_BUFFER,

        /// <summary>
        /// Less than REPARSE_GUID_DATA_BUFFER.
        /// </summary>
        LessThanREPARSE_GUID_DATA_BUFFER,

        /// <summary>
        /// OutLessThanFILE_ALLOCATED_RANGE_BUFFER
        /// </summary>
        OutLessThanFILE_ALLOCATED_RANGE_BUFFER,

        /// <summary>
        /// Less than USN_RECORD.
        /// </summary>
        LessThanUSN_RECORD,

        /// <summary>
        /// Less than one byte.
        /// </summary>
        LessThanOneBytes,

        /// <summary>
        /// Less than ENCRYPTION_BUFFER.
        /// </summary>
        LessThanENCRYPTION_BUFFER,

        /// <summary>
        /// Not equal to FILE_OBJECTID_BUFFER.
        /// </summary>
        NotEqualFILE_OBJECTID_BUFFER,

        /// <summary>
        /// Not equal to 48 bytes.
        /// </summary>
        NotEqual48Bytes,

        /// <summary>
        /// Less than 8 bytes.
        /// </summary>
        LessThan8Bytes,

        /// <summary>
        /// Not equal to ReparseDataLength plus 8.
        /// </summary>
        NotEqualReparseDataLengthPlus8,

        /// <summary>
        /// Not equal to ReparseDataLength plus 24.
        /// </summary>
        NotEqualReparseDataLengthPlus24,

        /// <summary>
        /// Less than RETRIEVAL_POINTERS_BUFFER.
        /// </summary>
        LessThanRETRIEVAL_POINTERS_BUFFER,

        /// <summary>
        /// Less than FILE_ZERO_DATA_INFORMATION.
        /// </summary>
        LessThanFILE_ZERO_DATA_INFORMATION,

        /// <summary>
        /// Less than RecordLength.
        /// </summary>
        LessThanRecordLength,

        /// <summary>
        /// Less than SI_COPYFILE.
        /// </summary>
        LessThanSI_COPYFILE,

        /// <summary>
        /// Proper buffer size.
        /// </summary>
        BufferSizeSuccess
    }

    /// <summary>
    ///  Enum the inputBuffer used in section 3.1.5.9.15.
    ///  It is an array of bytes containing a single FILE_ALLOCATED_RANGE_BUFFER structure indicating
    ///  the range to query for allocation, as specified in [MS-FSCC] section 2.3.32.
    /// </summary>
    public enum BufferLength
    {
        /// <summary>
        /// Length of FILE_ALLOCATED_RANGE_BUFFER equal 0.
        /// </summary>
        EqualZero,
        /// <summary>
        /// Length of FILE_ALLOCATED_RANGE_BUFFER is not less than size of FILE_ALLOCATED_RANGE_BUFFER.
        /// </summary>
        BufferLengthSuccess,
        /// <summary>
        /// Length of FILE_ALLOCATED_RANGE_BUFFER is less than 0.
        /// </summary>
        LessThanZero,
        /// <summary>
        /// Length of FILE_ALLOCATED_RANGE_BUFFER is more than long.MaxValue.
        /// </summary>
        MoreThanMAXLONGLONG,
        /// <summary>
        /// FileOffset of FILE_ALLOCATED_RANGE_BUFFER is less than 0.
        /// </summary>
        FileOffsetLessThanZero
    }

    /// <summary>
    /// InputBuffer_FSCTL_SET_ZERO_DATA
    /// </summary>
    public enum InputBuffer_FSCTL_SET_ZERO_DATA
    {
        /// <summary>
        /// BufferSuccess
        /// </summary>
        BufferSuccess,

        /// <summary>
        /// FileOffsetLessThanZero
        /// </summary>
        FileOffsetLessThanZero,

        /// <summary>
        /// BeyondFinalZeroLessThanZero
        /// </summary>
        BeyondFinalZeroLessThanZero,

        /// <summary>
        /// FileOffSetGreatThanBeyondFinalZero
        /// </summary>
        FileOffsetGreatThanBeyondFinalZero
    }

    public enum InputBufferFSCTL_SIS_COPYFILE
    {
        /// <summary>
        /// For initial
        /// </summary>
        Initial,

        /// <summary>
        /// InputBuffer.SourceFileName ) + InputBuffer.SourceFileNameLength + InputBuffer.DestinationFileNameLength is > InputBufferSize
        /// </summary>
        InputBufferSizeLessThanOtherPlus,

        /// <summary>
        /// DestinationFileNameLength LargeThan MAXUSHORT
        /// </summary>
        DestinationFileNameLengthLargeThanMAXUSHORT,

        /// <summary>
        /// FlagsNotContainCOPYFILE_SIS_LINKAndCOPYFILE_SIS_REPLACE
        /// </summary>
        FlagsNotContainCOPYFILE_SIS_LINKAndCOPYFILE_SIS_REPLACE,

        /// <summary>
        /// DestinationFileNameLengthLessThanZero
        /// </summary>
        DestinationFileNameLengthLessThanZero
    }

    /// <summary>
    /// A SECURITY_INFORMATION data type as defined in [MS-DTYP] section 2.4.7.
    /// </summary>
    /// Disable warning CA1027 is to avoid changes in model.
    [SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum SecurityInformation
    {
        /// <summary>
        /// None.
        /// </summary>
        None,
        /// <summary>
        /// The owner identifier of the object is being referenced.
        /// </summary>
        OWNER_SECURITY_INFORMATION = 0x00000001,

        /// <summary>
        /// The primary group identifier of the object is being referenced.
        /// </summary>
        GROUP_SECURITY_INFORMATION = 0x00000002,

        /// <summary>
        /// The DACL of the object is being referenced.
        /// </summary>
        DACL_SECURITY_INFORMATION = 0x00000004,

        /// <summary>
        /// The SACL of the object is being referenced.
        /// </summary>
        SACL_SECURITY_INFORMATION = 0x00000008,

        /// <summary>
        /// The mandatory integrity label is being referenced.
        /// </summary>
        LABEL_SECURITY_INFORMATION = 0x00000010,
    }

    /// <summary>
    /// Indicate output buffer size
    /// </summary>
    public enum OutputBufferSize
    {
        /// <summary>
        /// Output buffer size is less than the requested size.
        /// </summary>
        LessThan,

        /// <summary>
        /// Output buffer size is not less than the requested size.
        /// </summary>
        NotLessThan
    }

    /// <summary>
    /// Indicate input buffer size
    /// </summary>
    public enum InputBufferSize
    {
        /// <summary>
        /// Input buffer size is less than the requested size.
        /// </summary>
        LessThan,

        /// <summary>
        /// Input buffer size is not less than the requested size.
        /// </summary>
        NotLessThan
    }
    /// <summary>
    /// Indicate byte count
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ByteCount : uint
    {
        /// <summary>
        /// Not set.
        /// </summary>
        NotSet,

        /// <summary>
        /// Zero.
        /// </summary>
        Zero,

        /// <summary>
        /// Size of FILE_ACCESS_INFORMATION.
        /// </summary>
        SizeofFILE_ACCESS_INFORMATION,

        /// <summary>
        /// Size of FILE_ALIGNMENT_INFORMATION.
        /// </summary>
        SizeofFILE_ALIGNMENT_INFORMATION,

        /// <summary>
        /// field offset of FILE_ALL_INFORMATION_NameInformation add NameInformationLength.
        /// </summary>
        FieldOffsetFILE_ALL_INFORMATION_NameInformationAddNameInformationLength,

        /// <summary>
        /// Field offset of FILE_NAME_INFORMATION_FileName add OutputBuffer_FileNameLength.
        /// </summary>
        FieldOffsetFILE_NAME_INFORMATION_FileNameAddOutputBuffer_FileNameLength,

        /// <summary>
        /// Size of FILE_ATTRIBUTE_TAG_INFORMATION.
        /// </summary>
        SizeofFILE_ATTRIBUTE_TAG_INFORMATION,

        /// <summary>
        /// Size of FILE_BASIC_INFORMATION
        /// </summary>
        SizeofFILE_BASIC_INFORMATION,

        /// <summary>
        /// Size of FILE_COMPRESSION_INFORMATION.
        /// </summary>
        SizeofFILE_COMPRESSION_INFORMATION,

        /// <summary>
        /// Size of FILE_EA_INFORMATION.
        /// </summary>
        SizeofFILE_EA_INFORMATION,

        /// <summary>
        /// Size of FILE_FULL_EA_INFORMATION.
        /// </summary>
        SizeofFILE_FULL_EA_INFORMATION,

        /// <summary>
        /// Size of FILE_INTERNAL_INFORMATION.
        /// </summary>
        SizeofFILE_INTERNAL_INFORMATION,

        /// <summary>
        /// Size of FILE_MODE_INFORMATION.
        /// </summary>
        SizeofFILE_MODE_INFORMATION,

        /// <summary>
        /// Size of FILE_NETWORK_OPEN_INFORMATION.
        /// <summary>
        SizeofFILE_NETWORK_OPEN_INFORMATION,

        /// <summary>
        /// SizeofFILE_STANDARD_INFORMATION.
        /// </summary>
        SizeofFILE_STANDARD_INFORMATION
    }

    /// <summary>
    /// Indicate output buffer
    /// </summary>
    public struct OutputBuffer
    {
        /// <summary>
        /// Access flag.
        /// </summary>
        public FileAccess AccessFlags;

        /// <summary>
        /// Revision.
        /// <summary>
        public uint Revision;
    }

    public enum EA
    {
        /// <summary>
        /// EA name which format is not well.
        /// </summary>
        EANameNotWellForm,

        /// <summary>
        /// EA flags are invalid.
        /// </summary>
        EAFlagsInvalid,

        /// <summary>
        /// Name of EA exists.
        /// </summary>
        EANameExist,

        /// <summary>
        /// EA length is not zero.
        /// </summary>
        EAValueLengthNotZero,
    }

    /// <summary>
    /// Indicate input buffer filename length
    /// </summary>
    public enum InputBufferFileNameLength
    {
        /// <summary>
        /// If InputBuffer.FileNameLength is equal to zero.
        /// </summary>
        EqualTo_Zero,

        /// <summary>
        /// If InputBuffer.FileNameLength is an odd number
        /// </summary>
        OddNumber,

        /// <summary>
        //If InputBuffer.FileNameLength is greater than 
        //InputBufferLength minus the byte offset into the FILE_RENAME_INFORMATION InputBuffer 
        //of the InputBuffer.FileName field (that is, the total length of InputBuffer as given 
        //in InputBufferLength is insufficient to contain the fixed-size fields of InputBuffer 
        //plus the length of InputBuffer.FileName)
        /// </summary>
        Greater
    }

    public enum StreamName
    {
        /// <summary>
        /// Name of stream contains invalid letter.
        /// </summary>
        ContainsInvalid,

        /// <summary>
        /// Name of stream contains wildcard.
        /// </summary>
        ContainsWildcard,

        /// <summary>
        /// Name of stream is case insensitive.
        /// </summary>
        IsCaseInsensitiveMatch,

        /// <summary>
        /// Name of stream has more than 255 Unicode.
        /// </summary>
        IsMore255Unicode,

        /// <summary>
        /// Length of stream name is zero.
        /// </summary>
        ZeroLength,

        /// <summary>
        /// Length of stream name is not zero.
        /// </summary>
        LengthNotZero,
    }

    public enum StreamTypeName
    {
        /// <summary>
        /// Name of stream contains invalid letter.
        /// </summary>
        ContainsInvalid,

        /// <summary>
        /// Name of stream contains wildcard.
        /// </summary>
        ContainsWildcard,

        /// <summary>
        // Length of stream name is zero.
        /// </summary>
        ZeroLength,

        /// <summary>
        /// Name of stream is data.
        /// </summary>
        IsData,

        /// <summary>
        /// Name of stream is index allocation.
        /// </summary>
        IsIndexAllocation
    }

    /// <summary>
    /// The oplock type being requested in 3.1.5.17.1
    /// </summary>
    public enum RequestedOplock
    {
        /// <summary>
        /// Corresponds to SMB2_OPLOCK_LEVEL_II as described in [MS-SMB2] section 2.2.13.
        /// </summary>
        LEVEL_TWO,

        /// <summary>
        /// Corresponds to SMB2_OPLOCK_LEVEL_EXCLUSIVE as described in [MS-SMB2] section 2.2.13.
        /// </summary>
        LEVEL_ONE,

        /// <summary>
        /// Corresponds to SMB2_OPLOCK_LEVEL_BATCH as described in [MS-SMB2] section 2.2.13.
        /// </summary>
        LEVEL_BATCH,

        /// <summary>
        /// (Corresponds to SMB2_OPLOCK_LEVEL_LEASE as described in [MS-SMB2] section 2.2.13.) 
        /// If this oplock type is specified, the server MUST additionally provide the RequestedOploclLevel parameter
        /// </summary>
        LEVEL_GRANULAR,

        /// <summary>
        /// READ_CACHING
        /// </summary>
        READ_CACHING,

        /// <summary>
        /// HANDLE_CACHING
        /// </summary>
        HANDLE_CACHING,

        /// <summary>
        /// WRITE_CACHING
        /// </summary>
        WRITE_CACHING,

        /// <summary>
        /// READ_CACHING and HANDLE_CACHING
        /// </summary>
        READ_HANDLE,

        /// <summary>
        /// READ_CACHING and WRITE_CACHING
        /// <summary>
        READ_WRITE,

        /// <summary>
        /// READ_CACHING, HANDLE_CACHING and WRITE_CACHING
        /// <summary>
        READ_WRITE_HANDLE
    }


    /// <summary>
    /// OwnerSid
    /// </summary>
    public enum OwnerSid
    {
        /// <summary>
        /// If InputBuffer.OwnerSid is not present
        /// </summary>
        InputBufferOwnerSidNotPresent,

        /// <summary>
        /// If InputBuffer.OwnerSid is not a valid owner SID 
        /// </summary>
        InputBufferOwnerSidNotValid,

        /// <summary>
        /// If Open.File.SecurityDescriptor.Owner is NULL,
        /// </summary>
        OpenFileSecDesOwnerIsNull

    }

    /// <summary>
    /// Indicate input buffer time
    /// </summary>
    public enum InputBufferTime
    {
        /// <summary>
        /// InputBuffer.CreationTime is less than -1.
        /// </summary>
        CreationTimeLessthanM1,

        /// <summary>
        /// If InputBuffer.LastAccessTime is less than -1.
        /// </summary>
        LastAccessTimeLessthanM1,

        /// <summary>
        /// If InputBuffer.LastWriteTime is less than -1.
        /// </summary>
        LastWriteTimeLessthanM1,

        /// <summary>
        /// If InputBuffer.ChangeTime is less than -1.
        /// </summary>
        ChangeTimeLessthanM1
    }

    /// <summary>
    /// Indicate input buffer filename
    /// </summary>
    public enum InputBufferFileName
    {
        /// <summary>
        /// The first character of InputBuffer.FileName is '\'
        /// </summary>
        StartWithBackSlash,

        /// <summary>
        /// The first character of InputBuffer.FileName is ':'
        /// </summary>
        StartWithColon,

        /// <summary>
        /// The last character of NewStreamName is ":".
        /// </summary>
        EndWithColon,

        /// <summary>
        /// ':' occur in InputBuffer.FileName more than three times
        /// </summary>
        ColonOccurMoreThanThreeTimes,

        /// <summary>
        /// InputBuffer.FileName contains invalid character
        /// </summary>
        ContainsInvalid,

        /// <summary>
        /// InputBuffer.FileName contains wildcard
        /// </summary>
        ContainsWildcard,

        /// <summary>
        /// Indicate InputBuffer.FileName is case sensitive or not
        /// </summary>
        IsCaseInsensitiveMatch,

        /// <summary>
        /// Indicate the number of InputBuffer.FileName is more than 255 or not
        /// </summary>
        IsMore255Unicode,

        /// <summary>
        /// Indicate the length of InputBuffer.FileName is zero
        /// </summary>
        LengthZero,

        /// <summary>
        /// Indicate the length of InputBuffer.FileName is not zero
        /// </summary>
        LengthNotZero,

        /// <summary>
        /// Indicate InputBuffer.FileName is data
        /// </summary>
        isData,

        /// <summary>
        /// Indicate InputBuffer.FileName is index allocation
        /// </summary>
        isIndexAllocation,

        /// <summary>
        /// Indicate InputBuffer.FileName is empty
        /// </summary>
        Empty,

        /// <summary>
        /// Indicate InputBuffer.FileName is equals Open.Link.ShortName
        /// </summary>
        EqualLinkShortName,

        /// <summary>
        /// Indicate InputBuffer.FileName is invalid
        /// </summary>
        NotValid,

        /// <summary>
        /// Indicate InputBuffer.FileName is valid
        /// </summary>
        Valid
    }

    /// <summary>
    /// Indicate EA input buffer
    /// </summary>
    public enum EainInputBuffer
    {
        /// <summary>
        /// If Ea.EaName is not well-formed as per [MS-FSCC] 2.4.15,
        /// </summary>
        EaNameNotWellForm,

        /// <summary>
        /// If Ea.Flags does not contain a valid set of flags as per [MS-FSCC] 2.4.15,
        /// </summary>
        EaFlagsInvalid,

        /// <summary>
        /// If Ea.EaName exists in the Open.File.ExtendedAttributes
        /// </summary>
        EaNameExistinOpenFileExtendedAttribute,

        /// <summary>
        /// If Ea.EaValueLength is NOT zero,
        /// </summary>
        EaValueLengthNotZero,
    }

    /// <summary>
    /// Input buffer current byte offset
    /// </summary>
    public enum InputBufferCurrentByteOffset
    {
        /// <summary>
        /// InputBuffer.CurrentByteOffset is less than 0.
        /// </summary>
        LessThanZero,

        /// <summary>
        /// InputBuffer.CurrentByteOffset is not an integer multiple of Open.File.Volume.SectorSize.
        ///</summary>
        NotValid,

        /// <summary>
        /// InputBuffer.CurrentByteOffset is valid.
        /// </summary>
        Valid
    }

    /// <summary>
    /// Indicate if all entries from ChangeNotifyEntry.NotifyEventList fit in OutputBufferSize bytes
    /// </summary>
    public enum ChangeNotifyEntryType
    {
        /// <summary>
        /// All entries from ChangeNotifyEntry.NotifyEventList fit in OutputBufferSize bytes.
        /// </summary>
        AllEntriesFitInOutputBufferSize,

        /// <summary>
        /// Not all entries from ChangeNotifyEntry.NotifyEventList fit in OutputBufferSize bytes
        /// </summary>
        NotAllEntriesFitInOutputBufferSize
    }

    /// <summary>
    /// Indicate if the stream is found or not.
    /// </summary>
    public enum StreamFoundType
    {
        /// <summary>
        /// Indicate that the stream is found.
        /// </summary>
        StreamIsFound,

        /// <summary>
        /// Indicate that the stream is NOT found.
        /// </summary>
        StreamIsNotFound
    }

    /// <summary>
    /// Indicate if the file is SymbolicLink.
    /// </summary>
    public enum SymbolicLinkType
    {
        /// <summary>
        /// Indicate that the file is SymbolicLink.
        /// </summary>
        IsSymbolicLink,

        /// <summary>
        /// Indicate that the file is NOT SymbolicLink.
        /// </summary>
        IsNotSymbolicLink
    }

    /// <summary>
    /// Indicate if Open.File.Volume is readonly or not.
    /// </summary>
    public enum OpenFileVolumeType
    {
        /// <summary>
        /// Indicate that the Open.File.Volume is read only.
        /// </summary>
        OpenFileVolumeIsReadOnly,

        /// <summary>
        /// Indicate that the Open.File.Volume is NOT read only.
        /// </summary>
        OpenFileVolumeIsNotReadOnly
    }

    /// <summary>
    /// Indicate if the file is set to delete pending.
    /// </summary>
    public enum FileDispositionType
    {
        /// <summary>
        /// Indicate that the file is set to delete pending.
        /// </summary>
        IsDeletePending,

        /// <summary>
        /// Indicate that the file is NOT set to delete pending.
        /// </summary>
        IsNotDeletePending
    }

    /// <summary>
    /// Indicate if InputBuffer.AllocationSize is greater than the maximum file size allowed by the object store.
    /// </summary>
    public enum AllocationSizeType
    {
        /// <summary>
        /// Indicate that InputBuffer.AllocationSize is greater than the maximum file size allowed by the object store.
        /// </summary>
        AllocationSizeIsGreaterThanMaximum,

        /// <summary>
        /// Indicate that InputBuffer.AllocationSize is NOT greater than the maximum file size allowed by the object store.
        /// </summary>
        AllocationSizeIsNotGreaterThanMaximum
    }

    /// <summary>
    /// Indicate if DestinationDirectory.Volume is equal to Open.File.Volume.
    /// </summary>
    public enum DirectoryVolumeType
    {
        /// <summary>
        /// Indicate that DestinationDirectory.Volume is equal to Open.File.Volume.
        /// </summary>
        DestDirVolumeEqualToOpenFileVolume,

        /// <summary>
        /// Indicate that DestinationDirectory.Volume is NOT equal to Open.File.Volume.
        /// </summary>
        DestDirVolumeNotEqualToOpenFileVolume
    }

    /// <summary>
    /// Indicate if DestinationDirectory is the same as Open.Link.ParentFile.
    /// </summary>
    public enum DestinationDirectoryType
    {
        /// <summary>
        /// Indicate that DestinationDirectory is the same as Open.Link.ParentFile.
        /// </summary>
        DestDirIsSameAsOpenLinkParentFile,

        /// <summary>
        /// Indicate that DestinationDirectory is NOT same as Open.Link.ParentFile.
        /// </summary>
        DestDirIsNotSameAsOpenLinkParentFile
    }

    /// <summary>
    /// Indicate if NewLinkName is case-sensitive
    /// </summary>
    public enum NewLinkNameFormatType
    {
        /// <summary>
        /// NewLinkName is case-sensitive
        /// </summary>
        NewLinkNameIsCaseSensitive,

        /// <summary>
        /// NewLinkName is NOT case-sensitive
        /// </summary>
        NewLinkNameIsNotCaseSensitive
    }

    /// <summary>
    /// Indicate if NewLinkName matched TargetLink.ShortName
    /// </summary>
    public enum NewLinkNameMatchType
    {
        /// <summary>
        /// Indicate if NewLinkName matched TargetLink.ShortName
        /// </summary>
        NewLinkNameMatchTargetLinkShortName,

        /// <summary>
        /// Indicate if NewLinkName NOT matched TargetLink.ShortName
        /// </summary>
        NewLinkNameNotMatchTargetLinkShortName
    }

    /// <summary>
    /// Indicate if replace target file if exists.
    /// </summary>
    public enum ReplacementType
    {
        /// <summary>
        /// Indicate to replace target file if exists.
        /// </summary>
        ReplaceIfExists,

        /// <summary>
        /// Indicate NOT to replace target file if exists.
        /// </summary>
        NotReplaceIfExists
    }

    /// <summary>
    /// Indicate if TargetLink is deleted or not.
    /// </summary>
    public enum TargetLinkDeleteType
    {
        /// <summary>
        /// Indicate if TargetLink is deleted.
        /// </summary>
        TargetLinkIsDeleted,

        /// <summary>
        /// Indicate that TargetLink is NOT deleted.
        /// </summary>
        TargetLinkIsNotDeleted
    }

    /// <summary>
    /// Indicate if there is an oplock to be broken
    /// </summary>
    public enum OplockBreakStatusType
    {
        /// <summary>
        /// Indicate that there is an oplock to be broken
        /// </summary>
        HasOplockBreak,

        /// <summary>
        /// Indicate that there is NO oplock to be broken
        /// </summary>
        HasNoOplockBreak
    }
    
    /// <summary>
    /// Indicate if TargetLink.File.OpenList contains an Open with a Stream matching the current Stream.
    /// </summary>
    public enum TargetLinkFileOpenListType
    {
        /// <summary>
        /// Indicate that TargetLink.File.OpenList contains an Open with a Stream matching the current Stream.
        /// </summary>
        TargetLinkFileOpenListContainMatchedOpen,

        /// <summary>
        /// Indicate that TargetLink.File.OpenList does NOT contain any Open with a Stream matching the current Stream.
        /// </summary>
        TargetLinkFileOpenListNotContainMatchedOpen
    }

    /// <summary>
    /// Indicate if Open.File equals to File.Volume.RootDirectory
    /// </summary>
    public enum FileRootDirectoryType
    {
        /// <summary>
        /// Indicate that Open.File equals to File.Volume.RootDirectory
        /// </summary>
        FileEqualToRootDirectory,

        /// <summary>
        /// Indicate that Open.File does NOT equal to File.Volume.RootDirectory
        /// </summary>
        FileNotEqualToRootDirectory
    }

    /// <summary>
    /// ByteCount of File System Information Class
    /// </summary>
    /// Disable warning CA1028 because System.Int32 cannot match the enumeration design according to actual Model logic.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsInfoByteCount : uint
    {
        /// <summary>
        /// Zero.
        /// </summary>
        Zero,

        /// <summary>
        /// ByteCount set to FieldOffset(FILE_FS_VOLUME_INFORMATION.VolumeLabel) + BytesToCopy.
        /// </summary>
        FieldOffset_FILE_FS_VOLUME_INFORMATION_VolumeLabel_Add_BytesToCopy,

        /// <summary>
        /// ByteCount set to sizeof(FILE_FS_SIZE_INFORMATION).
        /// </summary>
        SizeOf_FILE_FS_SIZE_INFORMATION,

        /// <summary>
        /// ByteCount set to sizeof(FILE_FS_DEVICE_INFORMATION).
        /// </summary>
        SizeOf_FILE_FS_DEVICE_INFORMATION,

        /// <summary>
        /// ByteCount set to FieldOffset(FILE_FS_ATTRIBUTE_INFORMATION.FileSystemName)+ BytesToCopy.
        /// </summary>
        FieldOffset_FILE_FS_ATTRIBUTE_INFORMATION_FileSystemName_Add_BytesToCopy,

        /// <summary>
        /// ByteCount set to sizeof(FILE_FS_CONTROL_INFORMATION).
        /// </summary>
        SizeOf_FILE_FS_CONTROL_INFORMATION,

        /// <summary>
        /// ByteCount set to sizeof(FILE_FS_FULL_SIZE_INFORMATION).
        /// </summary>
        SizeOf_FILE_FS_FULL_SIZE_INFORMATION,

        /// <summary>
        /// ByteCount set to sizeof(FILE_FS_OBJECTID_INFORMATION).
        /// </summary>
        SizeOf_FILE_FS_OBJECTID_INFORMATION,

        /// <summary>
        /// ByteCount set to the size of the FILE_FS_SECTOR_SIZE_INFORMATION structure.
        /// </summary>
        SizeOf_FILE_FS_SECTOR_SIZE_INFORMATION,

        /// <summary>
        /// Unexpected size not defined in above.
        /// </summary>
        Unexpected_SIZE
    }

    /// <summary>
    /// Indicate if enumeration should be restarted from the beginning of the directory.
    /// </summary>
    public enum QueryDirectoryScanType
    {
        /// <summary>
        /// Indicate that enumeration should be restarted from the beginning of the directory.
        /// </summary>
        RestartScan,

        /// <summary>
        /// Indicate that enumeration should NOT be restarted from the beginning of the directory.
        /// </summary>
        NotRestartScan
    }

    /// <summary>
    /// Indicate if there is a matched file according to FileNamePattern.
    /// </summary>
    public enum QueryDirectoryFileNameMatchType
    {
        /// <summary>
        /// Indicate that there is a matched file according to FileNamePattern.
        /// </summary>
        FileNamePatternMatched,

        /// <summary>
        /// Indicate that there is NO matched file according to FileNamePattern.
        /// </summary>
        FileNamePatternNotMatched
    }

    /// <summary>
    /// Indicate if OutputBuffer is enough to contain Query Directory Response data.
    /// </summary>
    public enum QueryDirectoryOutputBufferType
    {
        /// <summary>
        /// Indicate that OutputBuffer is enough to contain Query Directory Response data.
        /// </summary>
        OutputBufferIsEnough,

        /// <summary>
        /// Indicate that OutputBuffer is NOT enough to contain Query Directory Response data.
        /// </summary>
        OutputBufferIsNotEnough
    }    
}
