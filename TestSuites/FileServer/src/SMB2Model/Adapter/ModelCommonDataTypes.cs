// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter
{
    /// <summary>
    /// Valid dialect revisions.
    /// </summary>
    public enum ModelDialectRevision : ushort
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
        /// SMB 3.11 dialect revision number
        /// </summary>
        Smb311 = 0x0311,

    }

    public enum ModelUser
    {
        /// <summary>
        /// Indicate to use default user for new session establishment
        /// </summary>
        DefaultUser,

        /// <summary>
        /// Indicate to use different user when bind to an existing session
        /// </summary>
        DiffUser
    }

    /// <summary>
    /// SMB2 Status code used in model for better diagnosis
    /// TODO: Consider to merge with Smb2Status class in SDK
    /// </summary>
    public enum ModelSmb2Status : uint
    {
        /// <summary>
        /// The operation completed successfully. 
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// The operation that was requested is pending completion.
        /// </summary>
        STATUS_PENDING = 0x00000103,

        /// <summary>
        /// Indicates that a notify change request has been completed due to closing the handle that made the notify change request.
        /// </summary>
        STATUS_NOTIFY_CLEANUP = 0x0000010B,

        /// <summary>
        /// Indicates that a notify change request is being completed and that the information is not being returned in the caller's buffer. 
        /// The caller now needs to enumerate the files to find the changes.
        /// </summary>
        STATUS_NOTIFY_ENUM_DIR = 0x0000010C,

        /// <summary>
        /// The data was too large to fit into the specified buffer.
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005,

        /// <summary>
        /// No more entries are available from an enumeration operation.
        /// </summary>
        STATUS_NO_MORE_ENTRIES = 0x8000001A,

        /// <summary>
        /// The create operation stopped after reaching a symbolic link.
        /// </summary>
        STATUS_STOPPED_ON_SYMLINK = 0x8000002D,

        /// <summary>
        /// The token supplied to the function is invalid.
        /// </summary>
        SEC_E_INVALID_TOKEN = 0x80090308,

        /// <summary>
        /// No credentials are available in the security package.
        /// </summary>
        SEC_E_NO_CREDENTIALS = 0x8009030E,

        /// <summary>
        /// The requested operation was unsuccessful.
        /// </summary>
        STATUS_UNSUCCESSFUL = 0xC0000001,

        /// <summary>
        /// The specified information class is not a valid information class for the specified object.
        /// </summary>
        STATUS_INVALID_INFO_CLASS = 0xC0000003,

        /// <summary>
        /// An invalid parameter was passed to a service or function.
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// The file does not exist.
        /// </summary>
        STATUS_NO_SUCH_FILE = 0xC000000F,

        /// <summary>
        /// The specified request is not a valid operation for the target device.
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// The end-of-file marker has been reached. There is no valid data in the file beyond this marker.
        /// </summary>
        STATUS_END_OF_FILE = 0xC0000011,

        /// <summary>
        /// The specified I/O request packet (IRP) cannot be disposed of because the I/O operation is not complete.
        /// </summary>
        STATUS_MORE_PROCESSING_REQUIRED = 0xC0000016,

        /// <summary>
        /// A process has requested access to an object but has not been granted those access rights.
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        ///  The buffer is too small to contain the entry. No information has been written to the buffer.
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// Object Name invalid.
        /// </summary>
        STATUS_OBJECT_NAME_INVALID = 0xC0000033,

        /// <summary>
        /// The object name is not found.
        /// </summary>
        STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034,

        /// <summary>
        /// The object name already exists.
        /// </summary>
        STATUS_OBJECT_NAME_COLLISION = 0xC0000035,

        /// <summary>
        /// A file cannot be opened because the share access flags are incompatible.
        /// </summary>
        STATUS_SHARING_VIOLATION = 0xC0000043,

        /// <summary>
        /// An operation involving EAs failed because the file system does not support EAs.
        /// </summary>
        STATUS_EAS_NOT_SUPPORTED = 0xC000004F,

        /// <summary>
        /// A requested read/write cannot be granted due to a conflicting file lock.
        /// </summary>
        STATUS_FILE_LOCK_CONFLICT = 0xC0000054,

        /// <summary>
        /// A lock request specified an invalid locking mode, or conflicted with an existing file lock.
        /// </summary>
        STATUS_LOCK_NOT_GRANTED = 0xC0000055,

        /// <summary>
        /// The attempted logon is invalid.
        /// This is either due to a bad username or authentication information.
        /// </summary>
        STATUS_LOGON_FAILURE = 0xC000006D,

        /// <summary>
        /// An operation failed because the disk was full.
        /// </summary>
        STATUS_DISK_FULL = 0xC000007F,

        /// <summary>
        /// Insufficient system resources exist to complete the API.
        /// </summary>
        STATUS_INSUFFICIENT_RESOURCES = 0xC000009A,

        /// <summary>
        /// A specified impersonation level is invalid.
        /// Also used to indicate a required impersonation level was not provided.
        /// </summary>
        STATUS_BAD_IMPERSONATION_LEVEL = 0xC00000A5,

        /// <summary>
        /// The file that was specified as a target is a directory, and the caller specified that it could be anything but a directory.
        /// </summary>
        STATUS_FILE_IS_A_DIRECTORY = 0xC00000BA,

        /// <summary>
        /// The request is not supported.
        /// </summary>
        STATUS_NOT_SUPPORTED = 0xC00000BB,

        /// <summary>
        /// The network name was deleted.
        /// </summary>
        STATUS_NETWORK_NAME_DELETED = 0xC00000C9,

        /// <summary>
        /// The specified share name cannot be found on the remote server.
        /// </summary>
        STATUS_BAD_NETWORK_NAME = 0xC00000CC,

        /// <summary>
        /// No more connections can be made to this remote computer at this time because the computer has already accepted the maximum number of connections.
        /// </summary>
        STATUS_REQUEST_NOT_ACCEPTED = 0xC00000D0,

        /// <summary>
        /// An error status returned when the opportunistic lock (oplock) request is denied.
        /// </summary>
        STATUS_OPLOCK_NOT_GRANTED = 0xC00000E2,

        /// <summary>
        /// An error status returned when an invalid opportunistic lock (oplock) acknowledgment is received by a file system.
        /// </summary>
        STATUS_INVALID_OPLOCK_PROTOCOL = 0xC00000E3,

        /// <summary>
        /// Indicates that the directory trying to be deleted is not empty.
        /// </summary>
        STATUS_DIRECTORY_NOT_EMPTY = 0xC0000101,

        /// <summary>
        /// A requested opened file is not a directory.
        /// </summary>
        STATUS_NOT_A_DIRECTORY = 0xC0000103,

        /// <summary>
        /// The I/O request was canceled.
        /// </summary>
        STATUS_CANCELLED = 0xC0000120,

        /// <summary>
        /// An I/O request other than close and several other special case operations
        /// was attempted using a file object that had already been closed
        /// </summary>
        STATUS_FILE_CLOSED = 0xC0000128,

        /// <summary>
        /// The device is not in a valid state to perform this request.
        /// </summary>
        STATUS_INVALID_DEVICE_STATE = 0xC0000184,

        /// <summary>
        /// A volume has been accessed for which a file system driver is required that has not yet been loaded.
        /// </summary>
        STATUS_FS_DRIVER_REQUIRED = 0xC000019C,

        /// <summary>
        /// The remote user session has been deleted.
        /// </summary>
        STATUS_USER_SESSION_DELETED = 0xC0000203,

        /// <summary>
        /// Indicates the attempt to insert the ID in the index failed because the ID is already in the index.
        /// </summary>
        STATUS_DUPLICATE_OBJECTID = 0xC000022A,

        /// <summary>
        /// DFS pathname not on local server.
        /// </summary>
        STATUS_PATH_NOT_COVERED = 0xC0000257,

        /// <summary>
        /// Indicates dfsc server is unavailable or DFS referrals meant for the domain are sent to the DFSC server.
        /// </summary>
        STATUS_DFS_UNAVAILABLE = 0xC000026d,

        /// <summary>
        /// The client session has expired; so the client must re-authenticate to continue accessing the remote resources.
        /// </summary>
        STATUS_NETWORK_SESSION_EXPIRED = 0xC000035C,

        /// <summary>
        /// Indicates the operation was successful, but no range was processed.
        /// </summary>
        STATUS_NO_RANGES_PROCESSED = 0xC0000460,

        /// <summary>
        /// The file is temporarily unavailable.
        /// </summary>
        STATUS_FILE_NOT_AVAILABLE = 0xC0000467,

        /// <summary>
        /// Hash generation for the specified version and hash type is not enabled on server.
        /// </summary>
        STATUS_HASH_NOT_SUPPORTED = 0xC000A100,

        /// <summary>
        /// The hash requests is not present or not up to date with the current file contents.
        /// </summary>
        STATUS_HASH_NOT_PRESENT = 0xC000A101,

        /// <summary>
        /// A file system filter on the server has not opted in for Offload Read support.
        /// </summary>
        STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED = 0xC000A2A1,
    }

    /// <summary>
    /// The level of MUST error check.
    /// </summary>
    public enum MustErrorCheckLevel
    {
        /// <summary>
        /// Ignore specified MUST error code, only check that the error code is not SUCCESS.
        /// </summary>
        IgnoreMustError = 0,

        /// <summary>
        /// Check the error code strictly.
        /// </summary>
        CheckMustError = 1,
    }

}
