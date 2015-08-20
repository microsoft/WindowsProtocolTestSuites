// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// The platform test suite run on.
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Test suite run on non windows platform.
        /// </summary>
        NonWindows,

        /// <summary>
        /// Test suite run on Windows NT platform.
        /// </summary>
        WinNt,

        /// <summary>
        /// Test suite run on Windows 2000 platform.
        /// </summary>
        Win2K,

        /// <summary>
        /// Test suite run on Windows XP platform.
        /// </summary>
        WinXP,

        /// <summary>
        /// Test suite run on Windows Vista platform.
        /// </summary>
        WinVista,

        /// <summary>
        /// Test suite run on Windows 2003 platform.
        /// </summary>
        Win2K3,

        /// <summary>
        /// Test suite run on Windows 2008 platform.
        /// </summary>
        Win2K8,

        /// <summary>
        /// Test suite run on Windows 7 platform.
        /// </summary>
        Win7,

        /// <summary>
        /// Test suite run on Windows 2008 R2 platform.
        /// </summary>
        Win2K8R2,

        /// <summary>
        /// Test suite run on Windows 2003 R2 platform.
        /// </summary>
        Win2K3R2,

        /// <summary>
        /// Test suite run on Windows 2003 SP1 platform.
        /// </summary>
        Win2K3Sp1
    }


    /// <summary>
    /// File system type.
    /// </summary>
    public enum FileSystemType
    {
        /// <summary>
        /// The NTFS file system.
        /// </summary>
        Ntfs,

        /// <summary>
        /// The FAT file system.
        /// </summary>
        Fat
    }


    /// <summary>
    /// Share file Name
    /// </summary>
    public enum ShareName
    {
        /// <summary>
        /// The full path name on SUT01 of the FAT share file.
        /// </summary>
        Share1,

        /// <summary>
        /// The full path name on SUT01 of the FAT share file.
        /// </summary>
        Share2,

        /// <summary>
        /// The path name of DFS share using TreeConnect.
        /// </summary>
        DfsShare,

        /// <summary>
        /// The path name of share folder for SetQuota testing .
        /// </summary>
        QuotaShare
    }


    /// <summary>
    /// The permission user have to log on the SUT01. 
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// The user with administrator permission.
        /// </summary>
        Admin,

        /// <summary>
        /// The user with guest permission.
        /// </summary>
        Guest
    }


    /// <summary>
    /// The states used in server setup initialization and connect to a server.
    /// </summary>
    /// Disable warning CA1028 because according to the technical document, 
    /// the SmbState is an unsigned 32-bit field representing SMB state.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SmbState : uint
    {
        /// <summary>
        /// SUT (System Under Test) initial state.
        /// </summary>
        Init = 0,

        /// <summary>
        /// SUT state after tree connect request has been sent from client.
        /// </summary>
        ConnectionRequest,

        /// <summary>
        /// SUT state after tree connect response has been sent from server.
        /// </summary>
        ConnectionSuccess,

        /// <summary>
        /// SUT state after server setup request has been sent from client.
        /// </summary>
        ServerSetupRequest,

        /// <summary>
        /// SUT state after server setup response has been sent from server.
        /// </summary>
        ServerSetupSuccess,

        /// <summary>
        /// 
        /// </summary>
        CreateNamePipeAndMailslotSucceed,

        /// <summary>
        /// SUT state after negotiate request has been sent from client.
        /// </summary>
        NegotiateSent,

        /// <summary>
        /// SUT state after negotiate response has been sent from server.
        /// </summary>
        NegotiateSuccess,

        /// <summary>
        /// SUT state after session setup request has been sent from client.
        /// </summary>
        SessionSetupSent,

        /// <summary>
        /// SUT state after session setup response has been sent from server.
        /// </summary>
        SessionSetupSuccess,

        /// <summary>
        /// SUT state after tree connect request has been sent from client.
        /// </summary>
        TreeConnectSent,

        /// <summary>
        /// SUT state after tree connect response has been sent from server.
        /// </summary>
        TreeConnectSuccess,

        /// <summary>
        /// SUT final state.
        /// </summary>
        Closed,

        /// <summary>
        /// SUT end state.
        /// </summary>
        End
    }


    /// <summary>
    /// Transport type
    /// </summary>
    public enum TransportType
    {
        /// <summary>
        /// Test suite use direct TCP over either IPv4 or IPv6 as a reliable stream-oriented transport for SMB messages.
        /// </summary>
        DirectTcp,

        /// <summary>
        /// Test suite use NetBIOS over IPX (NBIPX) as a transport for SMB messages.
        /// </summary>
        NetBiosOverIpx,

        /// <summary>
        /// Test suite use NetBIOS Extended User Interface (NetBEUI) 3.0 as a transport for SMB messages.
        /// </summary>
        NetBeui
    }


    /// <summary>
    /// Dialect names
    /// </summary>
    public enum Dialect
    {
        /// <summary>
        /// PC NETWORK PROGRAM 1.0.
        /// </summary>
        PcNet1,

        /// <summary>
        /// It is sent in protocol negotiation performed by Windows NT and OS/2.
        /// </summary>
        XenixCore,

        /// <summary>
        /// It is supported by IBM Corporation in early implementations of the SMB Protocol.
        /// </summary>
        PcLan1,

        /// <summary>
        /// It consists of several minor extensions to the core protocol, including raw read and write commands 
        /// and compound commands such as SMB_COM_LOCK_AND_READ and SMB_COM_WRITE_AND_UNLOCK.
        /// </summary>
        MsNet103,

        /// <summary>
        /// It is the DOS LAN Manager 1.0 extended protocol.
        /// </summary>
        MsNet30,

        /// <summary>
        /// It is used to support OS/2 system functions and file system features.
        /// </summary>
        LanMan10,

        /// <summary>
        /// Windows for Work group 3.1a.
        /// </summary>
        Wfw10,

        /// <summary>
        /// When this dialect is selected, OS/2 error codes are translated to DOS error codes by the server 
        /// before transmission to the client.
        /// </summary>
        DosLanMan12,

        /// <summary>
        /// It is used to support for additional OS/2 commands and features to "LANMAN1.0".
        /// </summary>
        LanMan12,

        /// <summary>
        /// It is identical to the OS/2 version of the dialect except that error codes are translated.
        /// </summary>
        DosLanMan21,

        /// <summary>
        /// It represents the LAN Manager 2.0 extended protocol for OS/2.
        /// </summary>
        LanMan21,

        /// <summary>
        /// It supports Windows NT. 
        /// OS/2 LAN Manager 2.1 features are also supported.
        /// </summary>
        NtLanMan,

        /// <summary>
        /// It represents invalid dialect name.
        /// </summary>
        Invalid
    }


    /// <summary>
    /// Capabilities.
    /// </summary>
    /// Disable warning CA1028 because according to the technical document, 
    /// the Capabilities is an unsigned 32-bit field representing SMB Capabilities.
    [Flags]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Capabilities : uint
    {
        /// <summary>
        /// The server supports no capabilities.
        /// </summary>
        None = 0,

        /// <summary>
        /// The server supports SMB_COM_READ_RAW and SMB_COM_WRITE_RAW requests.
        /// Raw mode is not supported over connectionless transports.
        /// </summary>
        CapRawMode = 0x00000001,

        /// <summary>
        /// The server supports SMB_COM_READ_MPX and SMB_COM_WRITE_MPX requests.
        /// MPX mode is supported only over connectionless transports.
        /// </summary>
        CapMpxMode = 0x00000002,

        /// <summary>
        /// The server supports UTF-16LE Unicode strings.
        /// </summary>
        CapUnicode = 0x00000004,

        /// <summary>
        /// The server supports large files with 64-bit offsets.
        /// </summary>
        CapLargeFiles = 0x00000008,

        /// <summary>
        /// The server supports SMB commands particular to the NT LAN Manager dialect.
        /// </summary>
        CapNtSmbs = 0x00000010,

        /// <summary>
        /// The server supports the use of remote procedure call [MS-RPCE] for remote API calls.
        /// Similar functionality would otherwise require use of the legacy Remote Administration Protocol, 
        /// as specified in [MS-RAP].
        /// </summary>
        CapRpcRemoteApis = 0x00000020,

        /// <summary>
        /// The server is capable of responding with 32-bit status codes in the Status field of the SMB header.
        /// CAP_STATUS32 can also be referred to as CAP_NT_STATUS.
        /// </summary>
        CapStatus32 = 0x00000040,

        /// <summary>
        /// The server supports level II opportunistic locks (oplocks).
        /// </summary>
        CapLevelIIOplocks = 0x00000080,

        /// <summary>
        /// The server supports the SMB_COM_LOCK_AND_READ command requests.
        /// </summary>
        CapLockAndRead = 0x00000100,

        /// <summary>
        /// The server supports the TRANS2_FIND_FIRST2, TRANS2_FIND_NEXT2, and FIND_CLOSE2 command requests.
        /// This bit SHOULD be set if CAP_NT_SMBS is set.
        /// </summary>
        CapNtFind = 0x00000200,

        /// <summary>
        /// The server is aware of the DFS Referral Protocol, and can respond to DFS referral requests.
        /// </summary>
        CapDfs = 0x00001000,

        /// <summary>
        /// The server supports pass-through Information Levels.
        /// This allows the client to pass Information Level structures in QUERY and SET operations.
        /// </summary>
        CapInfoLevelPassThru = 0x00002000,

        /// <summary>
        /// The server supports large read operations. This capability affects the maximum size, in bytes, 
        /// of the server buffer for sending an SMB_COM_READ_ANDX response to the client.
        /// </summary>
        CapLargeReadx = 0x00004000,

        /// <summary>
        /// The server supports large write operations. This capability affects the maximum size, in bytes,
        /// of the server buffer for receiving an SMB_COM_WRITE_ANDX client request. 
        /// </summary>
        CapLargeWritex = 0x00008000,

        /// <summary>
        /// The server supports new light-weight I/O control (IOCTL) and file system control (FSCTL) operations. 
        /// These operations are accessed using the NT_TRANSACT_IOCTL subcommand.
        /// </summary>
        CapLwIo = 0x00010000,

        /// <summary>
        /// The server supports UNIX extensions.
        /// </summary>
        CapUnix = 0x00800000,

        /// <summary>
        /// Reserved but not implemented. The server supports compressed SMB packets.
        /// </summary>
        CapCompressedData = 0x02000000,

        /// <summary>
        /// The server supports re-authentication.
        /// </summary>
        CapDynamicReauth = 0x20000000,

        /// <summary>
        /// Reserved but not implemented. The server supports persistent handles.
        /// </summary>
        CapPersistentHandles = 0x40000000,

        /// <summary>
        /// The server supports extended security for authentication.
        /// This bit is used in conjunction with the SMB_FLAGS2_EXTENDED_SECURITY SMB_Header.Flags2 flag.
        /// </summary>
        CapExtendedSecurity = 0x80000000
    }


    /// <summary>
    /// Message status code returned from the server.
    /// </summary>
    /// Disable warning CA1028 because according to the technical document, 
    /// the MessageStatus is an unsigned 32-bit field representing SMB message status.
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum MessageStatus : uint
    {
        /// <summary>
        /// The client request is successful.
        /// </summary>
        Success = 0x00000000,

        /// <summary>
        /// The parameter specified in the request is not valid.
        /// </summary>
        InvalidParameter = 0xC000000D,

        /// <summary>
        /// If extended security has been negotiated, then this error code can be returned in the 
        /// SMB_COM_SESSION_SETUP_ANDX response from the server to indicate that additional authentication
        /// information is to be exchanged.
        /// </summary>
        MoreProcessingRequired = 0xC0000016,

        /// <summary>
        /// The client did not have the required permission needed for the operation.
        /// </summary>
        AccessDenied = 0xC0000022,

        /// <summary>
        /// The path to the directory specified was not found.
        /// This error is also returned on a create request if the operation requires the creation 
        /// of more than one new directory level for the path specified.
        /// </summary>
        ObjectPathNotFound = 0xC000003A,

        /// <summary>
        /// The client request is not supported.
        /// </summary>
        NotSupported = 0xC00000BB,

        /// <summary>
        /// The network name specified by the client has been deleted on the server.
        /// This error is returned if the client specifies an incorrect TID or the share on the server 
        /// represented by the TID was deleted.
        /// </summary>
        NetworkNameDeleted = 0xC00000C9,

        /// <summary>
        /// The user session specified by the client has been deleted on the server. 
        /// This error is returned by the server if the client sends an incorrect UID.
        /// </summary>
        UserSessionDeleted = 0xC0000203,

        /// <summary>
        /// The client's session has expired; therefore, 
        /// the client MUST re-authenticate to continue accessing remote resources.
        /// </summary>
        NetworkSessionExpired = 0xC000035C,

        /// <summary>
        /// An invalid SMB client request is received by the server.
        /// </summary>
        InvalidSmb = 0x00010002,

        /// <summary>
        /// The client request received by the server contains an invalid TID value.
        /// </summary>
        SmbBadTid = 0xC0002005,

        /// <summary>
        /// The client request received by the server contains an unknown SMB command code.
        /// </summary>
        SmbBadCommand = 0xC0002016,

        /// <summary>
        /// The client has requested too many UID values from the server or the 
        /// client already has an SMB session setup with this UID value.
        /// </summary>
        SmbTooManyUids = 0xC000205A,

        /// <summary>
        /// The client request to the server contains an invalid UID value.
        /// </summary>
        SmbBadUid = 0xC000205B,

        /// <summary>
        /// The client request received by the server is for a non-standard SMB operation.
        /// The client SHOULD send another request with a different SMB command to perform.
        /// </summary>
        SmbUseStandard = 0xC00020FB,

        /// <summary>
        /// The designated named pipe is in the process of being closed.
        /// </summary>
        SmbNoMoreData = 0xC00000D9,

        /// <summary>
        /// The data was too large to fit into the specified buffer.
        /// </summary>
        BufferOverFlow = 0x80000005,

        /// <summary>
        /// A device that does not exist was specified.
        /// </summary>
        NoSuchDevice = 0xC000000E,

        /// <summary>
        /// A specified impersonation level is invalid.
        /// This error is also used to indicate that a required impersonation level was not provided.
        /// </summary>
        BadImpersonationLevel = 0xC00000A5,

        /// <summary>
        /// The requested operation is not implemented.
        /// </summary>
        StatusNotImplemented = 0xC0000002,

        /// <summary>
        /// The object name is not found.
        /// </summary>
        ObjectNameNotFound = 0xc0000034,

        /// <summary>
        /// The object name already exists.
        /// </summary>
        ObjectNameNotCollision = 0xC0000035,

        /// <summary>
        /// The file that was specified as a target is a directory and 
        /// the caller specified that it could be anything but a directory.
        /// </summary>
        StatusFileIsADirectory = 0xC00000BA,

        /// <summary>
        /// The specified request is not a valid operation for the target device.
        /// </summary>
        StatusInvalidDeviceRequest = 0xC0000010,

        /// <summary>
        /// The FID is invalid.
        /// </summary>
        StatusInvalidHandle = 0xC0000008,

        /// <summary>
        /// No more files were found that match the file specification.
        /// </summary>
        StatusNoMoreFiles = 0x80000006,

        /// <summary>
        /// Indicates the exception in which this maximum buffer size MUST be exceeded
        /// </summary>
        StatusMaxBufferExceeded = 0xffffffff,

        /// <summary>
        /// Indicates the tree connect specified by the TID in the SMB header of the request is disconnected.
        /// </summary>
        StatusBadNetWorkName = 0xC00000CC,

        /// <summary>
        /// Indicates the error returned from server when dialect is LANMAN 2.0
        /// </summary>
        ErrorGenFailure = 0x22222222,

        /// <summary>
        /// Invalid SMB command.
        /// </summary>
        ErrSmbCmd = 64,

        /// <summary>
        /// Insufficient server resources to place the lock.
        /// </summary>
        StatusInsuffServerResources = 0xC0000205,

        /// <summary>
        /// Disk I/O error.
        /// </summary>
        StatusDataError = 0xC000003E,

        /// <summary>
        /// Attempt to write to a read-only file system.
        /// </summary>
        StatusMediaWriteProtected = 0xC00000A2,

        /// <summary>
        /// The MaxDataCount is too small to accept the request information.
        /// </summary>
        StatusBufferTooSmall = 0xC0000023,

        /// <summary>
        /// The UID supplied is not defined to the session.
        /// </summary>
        StatusSmbBadUid = 0x005B0002,

        /// <summary>
        /// The specified information class is not a valid information class for the specified object.
        /// </summary>
        StatusInvalidInfoClass = 0xC0000003,

        /// <summary>
        /// The parameter specified the information level is not supported.
        /// </summary>
        StatusOs2InvalidLevel = 0x007C0001,

        /// <summary>
        /// The request is failed.
        /// </summary>
        Failed
    }


    /// <summary>
    /// The action to take if a file does or does not exist.
    /// </summary>
    public enum CreateDisposition
    {
        /// <summary>
        /// FILE_SUPERSEDE
        /// If the file already exists, it SHOULD be superseded (overwritten). 
        /// If it does not already exist, then it SHOULD be created.
        /// </summary>
        FileSupersede = 0x00000000,

        /// <summary>
        /// FILE_OPEN
        /// If the file already exists, it SHOULD be opened rather than created. 
        /// If the file does not already exist, the operation MUST fail.
        /// </summary>
        FileOpen = 0x00000001,

        /// <summary>
        /// FILE_CREATE
        /// If the file already exists, the operation MUST fail. 
        /// If the file does not already exist, it SHOULD be created.
        /// </summary>
        FileCreate = 0x00000002,

        /// <summary>
        /// FILE_OPEN_IF
        /// If the file already exists, it SHOULD be opened. 
        /// If the file does not already exist, then it SHOULD be created.
        /// </summary>
        FileOpenIf = 0x00000003,

        /// <summary>
        /// FILE_OVERWRITE
        /// If the file already exists, it SHOULD be opened and truncated. 
        /// If the file does not already exist, the operation MUST fail.
        /// </summary>
        FileOverwrite = 0x00000004,

        /// <summary>
        /// FILE_OVERWRITE_IF
        /// If the file already exists, it SHOULD be opened and truncated. 
        /// If the file does not already exist, it SHOULD be created.
        /// </summary>
        FileOverwriteIf = 0x00000005
    }


    /// <summary>
    /// The action taken after create operation.
    /// </summary>
    public enum CreateAction
    {
        /// <summary>
        /// Supersede the specificed file.
        /// </summary>
        FileSuperseded,

        /// <summary>
        /// Open the specified file.
        /// </summary>
        FileOpened,

        /// <summary>
        /// Create a file.
        /// </summary>
        FileCreated,

        /// <summary>
        /// Over write the specified file.
        /// </summary>
        FileOverwritten,

        /// <summary>
        /// Check if the specified file exists.
        /// </summary>
        FileExists,

        /// <summary>
        /// Check if the specified file does not exist.
        /// </summary>
        FileDoesNotExist
    }


    /// <summary>
    /// InformationLevel.
    /// </summary>
    public enum InformationLevel
    {
        /// <summary>
        /// SMB_INFO_STANDARD
        /// Return or query creation, access, and last write timestamps, size and file attributes along with the file 
        /// name.
        /// </summary>
        SmbInfoStandard,

        /// <summary>
        /// SMB_QUERY_FS_ATTRIBUTE_INFO value is 0x105.
        /// </summary>
        SmbQueryFsAttributeInfo,

        /// <summary>
        /// SMB_QUERY_FILE_STREAM_INFO value is 0x109.
        /// </summary>
        SmbQueryFileStreamInfo,

        /// <summary>
        /// SMB_SET_FILE_BASIC_INFO value is 0x101.
        /// </summary>
        SmbSetFileBasicInfo,

        /// <summary>
        /// SMB_QUERY_FILE_ALLLOCATION_INFO value is 0x105.
        /// </summary>
        SmbQueryFileAllLocationInfo,

        /// <summary>
        /// SMB_QUERY_FILE_END_OF_FILEINFO value is 0x106.
        /// </summary>
        SmbQueryFileEndOfFileInfo,

        /// <summary>
        /// SMB_FIND_FILE_BOTH_DIRECTORY_INFO
        /// Returns a combination of the data from SMB_FIND_FILE_FULL_DIRECTORY_INFO and SMB_FIND_FILE_NAMES_INFO.
        /// </summary>
        SmbFindFileBothDirectoryInfo,

        /// <summary>
        /// SMB_FIND_FILE_FULL_DIRECTORY_INFO
        /// Returns the SMB_FIND_FILE_DIRECTORY_INFO data along with the size of a file's EAs.
        /// </summary>
        SmbFindFileIdFullDirectoryInfo,

        /// <summary>
        /// SMB_FIND_FILE_ID_BOTH_DIRECTORY_INFO value is 0x106.
        /// </summary>
        SmbFindFileIdBothDirectoryInfo,

        /// <summary>
        /// The native Windows NT information level.
        /// FILE_ACCESS_INFORMATION value is 8.
        /// </summary>
        FileAccessInformation,

        /// <summary>
        /// FILE_LINK_INFORMATION value is 11.
        /// </summary>
        FileLinkInformation,

        /// <summary>
        /// FILE_RENAME_INFORMATION value is 10.
        /// </summary>
        FileRenameInformation,

        /// <summary>
        /// FILE_ALLOCATION_INFORMATION value is 19.
        /// </summary>
        FileAllocationInformation,

        /// <summary>
        /// FILE_FS_SIZE_INFORMATION value is 3.
        /// </summary>
        FileFsSizeInformation,

        /// <summary>
        /// FILE_FS_CONTROL_INFORMATION value is 6.
        /// </summary>
        FileFsControlInformaiton,

        /// <summary>
        /// Invalid information.
        /// </summary>
        Invalid
    }


    /// <summary>
    /// The session states.
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// The session is valid.
        /// </summary>
        Valid,

        /// <summary>
        /// The session is in expired.
        /// </summary>
        Expired,

        /// <summary>
        /// The session is in progress.
        /// </summary>
        InProgress
    }


    /// <summary>
    /// The type of share. 
    /// The Service field in the SMB_COM_TREE_CONNECT_ANDX Response is matched against this element.
    /// </summary>
    public enum ShareType
    {
        /// <summary>
        /// Share is a Disk service.
        /// </summary>
        Disk,

        /// <summary>
        /// Share is a Printer service.
        /// </summary>
        Printer,

        /// <summary>
        /// Share is a Named Pipe service.
        /// </summary>
        NamedPipe,

        /// <summary>
        /// No shared service.
        /// </summary>
        CommunicationDevice
    }


    /// <summary>
    /// A state that determines whether this node signs messages. 
    /// </summary>
    public enum SignState
    {
        /// <summary>
        /// If the other party requires message signing, it MUST be used. Otherwise, message signing MUST NOT be used.
        /// </summary>
        Disabled,

        /// <summary>
        /// Message signing is enabled. If the other party enables or requires signing, it MUST be used.
        /// </summary>
        Enabled,

        /// <summary>
        /// The flag indicates that connections to parties who do not use signing MUST be disconnected.
        /// </summary>
        Required,

        /// <summary>
        /// Message signing is disabled unless the other party requires it. 
        /// </summary>
        DisabledUnlessRequired
    }


    /// <summary>
    /// MaxDataCount in FSCTL_SRV_ENUMERATE_SNAPSHOTS
    /// </summary>
    public enum MaxDataCount
    {
        /// <summary>
        /// the MaxData size is too small that the server will return InvalidParameter.
        /// </summary>
        VerySmall,

        /// <summary>
        /// the MaxData size is larger than 0x000F (15) and smaller than the total size of snapshots in server side.
        /// </summary>
        Mid,

        /// <summary>
        /// the MaxData size is larger than the total size of snapshots in server side.
        /// </summary>
        VeryLarge,
    }


    /// <summary>
    /// Commands for different message types.
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// SMB_COM_NEGOTIATE
        /// This command is used to initiate an SMB session between the client and the server.
        /// </summary>
        SmbComNegotiate,

        /// <summary>
        /// SMB_COM_SESSION_SETUP
        /// This command is used to configure an SMB session.
        /// </summary>
        SmbComSessionSetup,

        /// <summary>
        /// SMB_COM_SESSION_SETUP_ADDITIONAL
        /// </summary>
        SmbComSessionSetupAdditional,

        /// <summary>
        /// SMB_COM_TREE_CONNECT
        /// This command is used to establish a client connection to a server share.
        /// This command has been deprecated.
        /// </summary>
        SmbComTreeConnect,

        /// <summary>
        /// NT_CREATE_REQUEST
        /// </summary>
        NtCreateRequest,

        /// <summary>
        /// NT_OPEN_REQUEST
        /// </summary>
        NtOpenrequest,

        /// <summary>
        /// NT_READ_REQUEST
        /// </summary>
        NtReadRequest,

        /// <summary>
        /// NT_WRITE_REQUEST
        /// </summary>
        NtWriteRequest,

        /// <summary>
        /// NT_TRANSACT_CREATE
        /// This transaction subcommand is used to create or open a file or directory when extended attributes (EAs) or 
        /// a security descriptor (SD) need to be applied.
        /// </summary>
        NtTransactCreate,

        /// <summary>
        /// SMB_COM_TRANSACTION
        /// This command serves as the transport for the Transaction sub protocol Commands.
        /// </summary>
        SmbComTransaction,

        /// <summary>
        /// TRANS_SET_NMPIPE_STATE
        /// This subcommand of the SMB_COM_TRANSACTION allows a client to set the read mode and the blocking mode
        /// of a specified named pipe.
        /// </summary>
        TransSetNmPipeState,

        /// <summary>
        /// TRANS_QUERY_NMPIPE_STATE
        /// This subcommand of the SMB_COM_TRANSACTION allows a client to retrieve information about a specified 
        /// named pipe.
        /// </summary>
        TransQueryNmPipeState,

        /// <summary>
        /// TRANS_QUERY_NMPIPE_INFO
        /// This subcommand of the SMB_COM_TRANSACTION allows for a client to retrieve information about a specified
        /// named pipe.
        /// </summary>
        TransQueryNmPipeInfo,

        /// <summary>
        /// TRANS_PEEK_NMPIPE
        /// The TRANS_PEEK_NMPIPE subcommand of the SMB_COM_TRANSACTION is used to copy data out of a named pipe 
        /// without removing it and to retrieve information about data in a named pipe.
        /// </summary>
        TransPeekNmPipe,

        /// <summary>
        /// TRANS_RAW_WRITE_NMPIPE
        /// This command allows for a raw write to a named pipe.
        /// It is deprecated.
        /// </summary>
        TransRawWriteNmPipe,

        /// <summary>
        /// TRANS_WRITE_NMPIPE
        /// This subcommand of SMB_COM_TRANSACTION allows a client to write data to a named pipe.
        /// </summary>
        TransWriteNmPipe,

        /// <summary>
        /// TRANS_RAW_READ_NMPIPE
        /// This subcommand of the SMB_COM_TRANSACTION allows for a raw read of data from a name pipe.
        /// </summary>
        TransRawReadNmPipe,

        /// <summary>
        /// TRANS_READ_NMPIPE
        /// The TRANS_READ_NMPIPE subcommand of the SMB_COM_TRANSACTION allows a client to read data from a named pipe.
        /// </summary>
        TransReadNmPipe,

        /// <summary>
        /// TRANS_TRANSACT_NMPIPE
        /// The TRANS_TRANSACT_NMPIPE subcommand of the SMB_COM_TRANSACTION is used to execute a transacted exchange
        /// against a named pipe.
        /// </summary>
        TransTransactNmPipe,

        /// <summary>
        /// TRANS_MAILSLOT_WRITE
        /// </summary>
        TransMailSlotWrite,

        /// <summary>
        /// TRANS_CALL_NMPIPE
        /// This subcommand allows a client to open a named pipe, issue a write to the named pipe, 
        /// issue a read from the named pipe, and close the named pipe. The named pipe is opened in message mode.
        /// </summary>
        TransCallNmPipe,

        /// <summary>
        /// TRANS_WAIT_NMPIPE
        /// The TRANS_WAIT_NMPIPE subcommand of the SMB_COM_TRANSACTION allows a client to be notified when the 
        /// specified named pipe is available to be connected to.
        /// </summary>
        TransWaitNmPipe,

        /// <summary>
        /// SMB_COM_TRANSACTION2
        /// This subcommands provide support for a richer set of server-side file system semantics.
        /// </summary>
        SmbComTransaction2,

        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION
        /// This transaction is an alternative to TRANS2_QUERY_PATH_INFORMATION.
        /// </summary>
        Trans2QueryFileInformation,


        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION
        /// This transaction is used to get information about a specific file or directory.
        /// </summary>
        Trans2QueryPathInformation,

        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION
        /// This transaction is used to request information about the object store underlying a share on the server.
        /// </summary>
        Trans2QueryFsInformation,

        /// <summary>
        /// TRANS2_SET_FILE_INFORMATION
        /// This transaction is an alternative to TRANS2_SET_PATH_INFORMATION.
        /// </summary>
        Trans2SetFileInformation,

        /// <summary>
        /// TRANS2_SET_PATH_INFORMATION
        /// </summary>
        Trans2SetPathInforamtion,

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION
        /// </summary>
        Trans2SetFsInformaiton,

        /// <summary>
        /// TRANS2_SET_FS_INFOMATION_ADDITIONAL
        /// </summary>
        Trans2SetFsInforamtionAdditional,

        /// <summary>
        /// TRANS_FIND_FIRST_2
        /// </summary>
        TransFindFirst2,

        /// <summary>
        /// TRANS_FIND_NEXT_2
        /// </summary>
        TransFindNext2,

        /// <summary>
        /// TRANS_GET_DFS_REFERRAL
        /// </summary>
        TransGetDfsReferral,

        /// <summary>
        /// SMB_COM_NT_TRANSACT
        /// This subcommands extend the file system feature access offered by SMB_COM_TRANSACTION2 (section 2.2.4.46), 
        /// and also allow for the transfer of very large parameter and data blocks.
        /// </summary>
        SmbComNtTransact,

        /// <summary>
        /// NT_TRANSACT_RENAME
        /// This subcommand was reserved but not implemented.
        /// </summary>
        NtTransactRename,

        /// <summary>
        /// NT_TRANSACT_QUERY_QUOTA
        /// </summary>
        NtTransactQueryQuota,

        /// <summary>
        /// NT_TRANSACT_SET_QUOTA
        /// </summary>
        NtTransactSetQuota,

        /// <summary>
        /// NT_TRANSACT_SET_QUOTA_ADDITIONAL
        /// </summary>
        NtTransactSetQuotaAdditional,

        /// <summary>
        /// FSCTL_SRV_ENUMERATE_SNAPSHOTS
        /// </summary>
        FsctlSrvEnumerateSnapshots,

        /// <summary>
        /// FSCTL_SRV_REQUEST_RESUME_KEY
        /// </summary>
        FsctlSrvRequestResumeKey,

        /// <summary>
        /// FSCTL_SRV_CUPCHUNK
        /// </summary>
        FsctlSrvCupychunk,

        /// <summary>
        /// Invalid command type
        /// </summary>
        InvalidCommand,

        /// <summary>
        ///SMB_COM_SEND_MESSAGE
        /// </summary>
        SmbComSendMessage,

        /// <summary>
        /// SMB_COM_SEND_START_MB_MESSAGE
        /// </summary>
        SmbComSendStartMbMessage,

        /// <summary>
        /// SMB_COM_SEND_END_MB_MESSAGE
        /// </summary>
        SmbComSendEndMbMessage,

        /// <summary>
        /// SMB_COM_SEND_TEST_MB_MESSAGE
        /// </summary>
        SmbComSendTextMbMessage,

        /// <summary>
        /// Invalid FSCTL command type
        /// </summary>
        InvalidFsctlCommand,

        /// <summary>
        /// FSCC and FSCTL name
        /// </summary>
        FSCC_FSCTL_NAME,

        /// <summary>
        /// FSCC and FSCTL path information
        /// </summary>
        FSCCTRANS2_QUERY_PATH_INFORMATION,

        /// <summary>
        /// FSCC and FSCTL FS information
        /// </summary>
        FSCCTRANS2_QUERY_FS_INFORMATION
    }


    /// <summary>
    /// Enumerate the comparison result of two integers
    /// </summary>
    public enum IntegerCompare
    {
        /// <summary>
        /// Positive result of two integers.
        /// </summary>
        Larger,

        /// <summary>
        /// Negative result of two integers.
        /// </summary>
        Smaller,

        /// <summary>
        /// Zero of two integers comparison result.
        /// </summary>
        Equal
    }


    /// <summary>
    /// FSCTL command which server does not understand
    /// </summary>
    /// Disable warning CA1008 because according to technical document, 
    /// the FsctlInvalidCommand doesn't have zero value, 
    /// so it is no need to add a member that has a value of
    /// zero with a suggested name of 'None'.
    /// Disable warning CA1028 because according to the technical document, 
    /// the FsctlInvalidCommand is an unsigned 32-bit field 
    /// representing SMB Fsctl invalid commands.
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FsctlInvalidCommand : uint
    {
        /// <summary>
        /// Command FSCTL_REQUEST_OPLOCK_LEVEL_1.
        /// </summary>
        FsctlRequestOplockLevle1 = 0x00090000,

        /// <summary>
        /// Command FSCTL_REQUEST_OPLOCK_LEVEL_2.
        /// </summary>
        FsctlRequestOplockLevel2 = 0x00090004,

        /// <summary>
        /// Command FSCTL_REQUEST_BATCH_OPLOCK.
        /// </summary>
        FsctlRequestBatchOplock = 0x00090008,

        /// <summary>
        /// Command FSCTL_REQUEST_FILTER_OPLOCK.
        /// </summary>
        FsctlRequestFilterOplock = 0x0009005c,

        /// <summary>
        /// Command FSCTL_OPLOCK_BREAK_ACKNOWLEDGE.
        /// </summary>
        FsctlOplockBreakAcknowledge = 0x0009000c,

        /// <summary>
        /// Command FSCTL_OPBATCH_ACK_CLOSE_PENDING.
        /// </summary>
        FsctlOpBatchAckClosePending = 0x00090010,

        /// <summary>
        /// Command FSCTL_OPLOCK_BREAK_NOTIFY.
        /// </summary>
        FsctlOpLockBreakNotify = 0x00090014,

        /// <summary>
        /// Command FSCTL_MOVE_FILE.
        /// </summary>
        FsctlMoveFile = 0x00090074,

        /// <summary>
        /// Command FSCTL_MARK_HANDLE.
        /// </summary>
        FsctlMarkHandle = 0x000900fc,

        /// <summary>
        /// Command FSCTL_QUERY_RETRIEVAL_POINTERS.
        /// </summary>
        FsctlQueryRetrievalPointers = 0x0009003b,

        /// <summary>
        /// Command FSCTL_PIPE_ASSIGN_EVENT.
        /// </summary>
        FsctlPipeAssignEvent = 0x000110000,

        /// <summary>
        /// Command FSCTL_GET_VOLUME_BITMAP.
        /// </summary>
        FsctlGetVolumeBitmap = 0x0009006f,

        /// <summary>
        /// Command FSCTL_GET_NTFS_FILE_RECORD.
        /// </summary>
        FsctlGetNtfsFileRecord = 0x00090068,

        /// <summary>
        /// Command FSCTL_INVALIDATE_VOLUMES.
        /// </summary>
        FsctlInvalidateVolumes = 0x00090054,

        /// <summary>
        /// Command FSCTL_READ_USN_JOURNAL.
        /// </summary>
        FsctlReadUsnJournal = 0x000900bb,

        /// <summary>
        /// Command FSCTL_CREATE_USN_JOURNAL.
        /// </summary>
        FsctlCreateUsnJournal = 0x000900e7,

        /// <summary>
        /// Command FSCTL_QUERY_USN_JOURNAL.
        /// </summary>
        FsctlQueryUsnJournal = 0x000900f4,

        /// <summary>
        /// Command FSCTL_DELETE_USN_JOURNAL.
        /// </summary>
        FsctlDeleteUsnJournal = 0x000900f8,

        /// <summary>
        /// Command FSCTL_ENUM_USN_DATA.
        /// </summary>
        FsctlEnumUsnData = 0x000900b3,

        /// <summary>
        /// Command FSCTL_QUERY_DEPENDENT_VOLUME.
        /// </summary>
        FsctlQueryDependentVolume = 0x000901f0,

        /// <summary>
        /// Command FSCTL_SD_GLOBAL_CHANGE.
        /// </summary>
        FsctlSdGlobalChange = 0x000901f4,

        /// <summary>
        /// Command FSCTL_GET_BOOT_AREA_INFO.
        /// </summary>
        FsctlGetBootAreaInfo = 0x00090230,

        /// <summary>
        /// Command FSCTL_GET_RETRIEVAL_POINTER_BASE.
        /// </summary>
        FsctlGetRetrievalPointerBase = 0x00090234,

        /// <summary>
        /// Command FSCTL_SET_PERSISTENT_VOLUME_STATE.
        /// </summary>
        FsctlSetPersistentVolumeState = 0x00090238,

        /// <summary>
        /// Command FSCTL_QUERY_PERSISTENT_VOLUME_STATE.
        /// </summary>
        FsctlQueryPersistentVolumeState = 0x0009023c,

        /// <summary>
        /// Command FSCTL_REQUEST_OPLOCK.
        /// </summary>
        FsctlRequestOpLock = 0x00090240,

        /// <summary>
        /// Command FSCTL_TXFS_MODIFY_RM.
        /// </summary>
        FsctlTxFsModifyRm = 0x00098144,

        /// <summary>
        /// Command FSCTL_TXFS_QUERY_RM_INFORMATION.
        /// </summary>
        FsctlTxfsQueryRmInformation = 0x00094148,

        /// <summary>
        /// Command FSCTL_TXFS_ROLLFORW ARD_REDO.
        /// </summary>
        FsctlTxFsRollForwardRedo = 0x00098150,

        /// <summary>
        /// Command FSCTL_TXFS_ROLLFORWARD_UNDO.
        /// </summary>
        FsctlTxFsRollForwardUndo = 0x00098154,

        /// <summary>
        /// Command FSCTL_TXFS_START_RM.
        /// </summary>
        FsctlTxfsStartRm = 0x00098158,

        /// <summary>
        /// Command FSCTL_TXFS_SHUTDOWN_RM.
        /// </summary>
        FsctlTxfsShutdownRm = 0x0009815c,

        /// <summary>
        /// Command FSCTL_TXFS_READ_BACKUP_INFORMATION.
        /// </summary>
        FsctlTxfsReadBackupInformation = 0x00094160,

        /// <summary>
        /// Command FSCTL_TXFS_WRITE_BACKUP_INFORMATION.
        /// </summary>
        FsctlTxfsWriteBackupInformation = 0x00098164,

        /// <summary>
        /// Command FSCTL_TXFS_CREATE_SECONDARY_RM.
        /// </summary>
        FsctlTxfsCreateSecondaryRm = 0x00098168,

        /// <summary>
        /// Command FSCTL_TXFS_GET_METADATA_INFO.
        /// </summary>
        FsctlTxfsGetMetadataInfo = 0x0009416c,

        /// <summary>
        /// Command FSCTL_TXFS_GET_TRANSACTED_VERSION.
        /// </summary>
        FsctlTxfsGetTransactedVersion = 0x00094170,

        /// <summary>
        /// Command FSCTL_TXFS_SAVEPOINT_INFORMATION.
        /// </summary>
        FsctlTxfsSavePointInformation = 0x00098178,

        /// <summary>
        /// Command FSCTL_TXFS_CREATE_MINIVERSION.
        /// </summary>
        FsctlTxfsCreateMiniVersion = 0x0009817c,

        /// <summary>
        /// Command FSCTL_TXFS_TRANSACTION_ACTIVE.
        /// </summary>
        FsctlTxfsTransactionActive = 0x0009418c,

        /// <summary>
        /// Command FSCTL_TXFS_LIST_TRANSACTIONS.
        /// </summary>
        FsctlTxfsListTransactions = 0x000941e4,

        /// <summary>
        /// Command FSCTL_TXFS_READ_BACKUP_INFORMATION2.
        /// </summary>
        FsctlTxfsReadBackupInformation2 = 0x00094160,

        /// <summary>
        /// Command FSCTL_TXFS_WRITE_BACKUP_INFORMATION2.
        /// </summary>
        FsctlTxfsWriteBackupInformation2 = 0x00098164
    }


    /// <summary>
    ///  The state of NT_TRANSACT_SET_QUOTA Request Parameters.
    /// </summary>
    public enum NtTransSetQuotaRequestParameter
    {
        /// <summary>
        /// All Input parameters are valid
        /// </summary>
        Valid,

        /// <summary>
        /// Access is denied
        /// </summary>
        AccessDenied,

        /// <summary>
        /// Input quota information is invalid
        /// </summary>
        QuotaInfoError,

        /// <summary>
        /// Start Id contains no valid SID
        /// </summary>
        StartSidError,

        /// <summary>
        /// Sid list did not contain a valid, properly formed list
        /// </summary>
        SidListError,

        /// <summary>
        /// The number of bytes of changed data size exceeded the MaxParameterCount field in the client request
        /// </summary>
        ChangedDataSizeError,

        /// <summary>
        /// I/O error
        /// </summary>
        InputOutputError,

        /// <summary>
        /// File system is readonly
        /// </summary>
        FsReadOnly,

        /// <summary>
        /// Server is out of resource
        /// </summary>
        ServerOutOfResource,

        /// <summary>
        /// SMB is invalid
        /// </summary>
        SmbInvalid,

        /// <summary>
        /// User Id is not known to the session
        /// </summary>
        UserIdError,

        /// <summary>
        /// File Id is invalid
        /// </summary>
        FileIdErrror,

        /// <summary>
        /// Tree Id is invalid
        /// </summary>
        TreeIdError
    }


    /// <summary>
    /// The state of the TRANS2_SET_FS_INFORMATION Request parmeters.
    /// </summary>
    public enum Trans2SetFsInfoResponseParameter
    {
        /// <summary>
        /// All Input parameters are valid
        /// </summary>
        Valid,

        /// <summary>
        /// Access is denied
        /// </summary>
        AccessDenied,

        /// <summary>
        /// I/O error
        /// </summary>
        InputOutputError,

        /// <summary>
        /// File system is readonly
        /// </summary>
        FileReadOnly,

        /// <summary>
        /// Server is out of resource
        /// </summary>
        ServerOutOfResource,

        /// <summary>
        /// SMB is invalid
        /// </summary>
        SmbInvalid,

        /// <summary>
        /// User Id is not known to the session
        /// </summary>
        UserIdError,

        /// <summary>
        /// File Id is invalid
        /// </summary>
        FileIdErrror,

        /// <summary>
        /// Tree Id is invalid
        /// </summary>
        TreeIdError


    }

    /// <summary>
    /// The FSCC FSCTL Name
    /// </summary>
    /// Disable warning CA1008 because according to technical document, do not have zero value 
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum FSCCFSCTLName
    {
        /// <summary>
        /// FSCTL_CREATE_OR_GET_OBJECT_ID
        /// </summary>
        FSCTL_CREATE_OR_GET_OBJECT_ID = 0x000900c0,

        /// <summary>
        /// FSCTL_DELETE_OBJECT_ID
        /// </summary>
        FSCTL_DELETE_OBJECT_ID = 0x000900a0,

        /// <summary>
        /// FSCTL_FILESYSTEM_GET_STATISTICS
        /// </summary>
        FSCTL_FILESYSTEM_GET_STATISTICS = 0x00090060,

        /// <summary>
        /// FSCTL_GET_COMPRESSION
        /// </summary>
        FSCTL_GET_COMPRESSION = 0x0009003c,

        /// <summary>
        /// FSCTL_GET_NTFS_VOLUME_DATA
        /// </summary>
        FSCTL_GET_NTFS_VOLUME_DATA = 0x00090064,

        /// <summary>
        /// FSCTL_IS_PATHNAME_VALID
        /// </summary>
        FSCTL_IS_PATHNAME_VALID = 0x0009002c,

        /// <summary>
        /// FSCTL_QUERY_SPARING_INFO
        /// </summary>
        FSCTL_QUERY_SPARING_INFO = 0x00090138,

        /// <summary>
        /// FSCTL_READ_FILE_USN_DATA
        /// </summary>
        FSCTL_READ_FILE_USN_DATA = 0x000900eb,

        /// <summary>
        /// FSCTL_SET_COMPRESSION
        /// </summary>
        FSCTL_SET_COMPRESSION = 0x0009c040,

        /// <summary>
        /// FSCTL_SET_SPARSE
        /// </summary>
        FSCTL_SET_SPARSE = 0x000900c4,

        /// <summary>
        /// FSCTL_SET_ZERO_DATA
        /// </summary>
        FSCTL_SET_ZERO_DATA = 0x000980c8,

        /// <summary>
        /// FSCTL_SET_ZERO_ON_DEALLOCATION
        /// </summary>
        FSCTL_SET_ZERO_ON_DEALLOCATION = 0x00090194,

        /// <summary>
        /// FSCTL_WRITE_USN_CLOSE_RECORD
        /// </summary>
        FSCTL_WRITE_USN_CLOSE_RECORD = 0x900ef,
    }


    /// <summary>
    /// The FSCC TransactionQueryPathInforLevel
    /// </summary>
    public enum FSCCTransaction2QueryPathInforLevel
    {
        /// <summary>
        /// File Basic information
        /// </summary>
        FileBasicInformation,

        /// <summary>
        /// File Standard Information
        /// </summary>
        FileStandardInformation,

        /// <summary>
        /// File Internal Information
        /// </summary>
        FileInternalInformation,

        /// <summary>
        /// File Ea Information
        /// </summary>
        FileEaInformation,

        /// <summary>
        /// File Access Information
        /// </summary>
        FileAccessInformation,

        /// <summary>
        /// File Name Information 
        /// </summary>
        FileNameInformation,

        /// <summary>
        /// File Mode Information
        /// </summary>
        FileModeInformation,

        /// <summary>
        /// File Alignment Information
        /// </summary>
        FileAlignmentInformation,

        /// <summary>
        /// File Alternate Name Information
        /// </summary>
        FileAlternateNameInformation,

        /// <summary>
        /// File Stream Information
        /// </summary>
        FileStreamInformation,

        /// <summary>
        /// File Compression Information
        /// </summary>
        FileCompressionInformation,

        /// <summary>
        /// File Network Open Information
        /// </summary>
        FileNetworkOpenInformation,

        /// <summary>
        /// File Attribute Tag Information
        /// </summary>
        FileAttributeTagInformation,

        /// <summary>
        /// File Position Information
        /// </summary>
        FilePositionInformation
    }

    /// <summary>
    /// FSCC Transaction2QueryFSInforLevel
    /// </summary>
    public enum FSCCTransaction2QueryFSInforLevel
    {
        /// <summary>
        /// File Fs Volume Information
        /// </summary>
        FileFsVolumeInformation,

        /// <summary>
        /// File Fs Size Information
        /// </summary>
        FileFsSizeInformation,

        /// <summary>
        /// File Fs Device Information
        /// </summary>
        FileFsDeviceInformation,

        /// <summary>
        /// File Fs Attribute Information
        /// </summary>
        FileFsAttributeInformation
    }
}
