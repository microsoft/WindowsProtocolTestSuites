// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// A base interface that implements IAdapter
    /// </summary>
    public interface IFSAAdapter : IAdapter
    {
        #region Initialize
        /// <summary>
        /// FSA initialize, it contains the following operations:
        /// 1. The client connects to server.
        /// 2. The client sets up a session with server.
        /// 3. The client connects to a share on server.
        /// </summary>
        void FsaInitial();
        #endregion

        #region 3.1.5   Higher-Layer Triggered Events

        #region 3.1.5.1   Server Requests an Open of a File

        #region 3.1.5.1.1   Creation of a New File
        /// <summary>
        /// Implement CreateFile Interface
        /// </summary>
        /// <param name="desiredFileAttribute">Desired file attribute</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        /// <param name="streamTypeNameToOpen">The name of stream type to open</param>
        /// <param name="desiredAccess">A bitmask indicating desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="shareAccess">A bitmask indicating sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="createDisposition">The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="streamFoundType">Indicate if the stream is found or not.</param>
        /// <param name="symbolicLinkType">Indicate if it is symbolic link or not.</param>
        /// <param name="openFileType">Indicate Filetype of open file</param>
        /// <param name="fileNameStatus">File name status</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        MessageStatus CreateFile(
            FileAttribute desiredFileAttribute,
            CreateOptions createOption,
            StreamTypeNameToOpen streamTypeNameToOpen,
            FileAccess desiredAccess,
            ShareAccess shareAccess,
            CreateDisposition createDisposition,
            StreamFoundType streamFoundType,
            SymbolicLinkType symbolicLinkType,
            FileType openFileType,
            FileNameStatus fileNameStatus
            );
        #endregion

        #region 3.1.5.1.2   Open of an Existing File
        /// <summary>
        /// Implement OpenExistingFile Interface
        /// </summary>
        /// <param name="shareAccess">A bitmask indicates sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="desiredAccess">A bitmask indicates desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="streamFoundType">Indicate if the stream is found or not.</param>
        /// <param name="symbolicLinkType">Indicate if it is symbolic link or not.</param>
        /// <param name="openFileType">Indicate file type of the file to be opened</param>
        /// <param name="fileNameStatus">File name status</param>
        /// <param name="existingOpenModeCreateOption">The option of existing file's create.</param>
        /// <param name="existOpenShareModeShareAccess">A bitmask indicates sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="existOpenDesiredAccess">A bitmask indicates desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="createOption">Create options</param>
        /// <param name="createDisposition">The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="streamTypeNameToOpen">The name of the stream type to be opened</param>
        /// <param name="fileAttribute">A bitmask of file attributes for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="desiredFileAttribute">A bitmask of desired file attributes for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        MessageStatus OpenExistingFile(
            ShareAccess shareAccess,
            FileAccess desiredAccess,
            StreamFoundType streamFoundType,
            SymbolicLinkType symbolicLinkType,
            FileType openFileType,
            FileNameStatus fileNameStatus,
            CreateOptions existingOpenModeCreateOption,
            ShareAccess existOpenShareModeShareAccess,
            FileAccess existOpenDesiredAccess,
            CreateOptions createOption,
            CreateDisposition createDisposition,
            StreamTypeNameToOpen streamTypeNameToOpen,
            FileAttribute fileAttribute,
            FileAttribute desiredFileAttribute);
        #endregion

        #region OpenFileinitial
        /// <summary>
        /// Implement OpenFileinitial Interface
        /// </summary>
        /// <param name="desiredAccess">A bitmask indicating desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="shareAccess">A bitmask indicating sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="createOption">Options of create</param>
        /// <param name="createDisposition">The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="fileAttribute">A bitmask of file attributes for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="isVolumeReadOnly">True: if Open.File.Volume.IsReadOnly is true</param>
        /// <param name="isCreateNewFile">True: if create a new file</param>
        /// <param name="isCaseInsensitive">True: if is case-insensitive</param>
        /// <param name="isLinkIsDeleted">True: if Link is deleted</param>
        /// <param name="isSymbolicLink">True: if Link.File.IsSymbolicLink is true</param>
        /// <param name="streamTypeNameToOpen">The type name of stream to be opened</param>
        /// <param name="openFileType">Open.File.FileType</param>
        /// <param name="fileNameStatus">File name status</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        MessageStatus OpenFileinitial(
            FileAccess desiredAccess,
            ShareAccess shareAccess,
            CreateOptions createOption,
            CreateDisposition createDisposition,
            FileAttribute fileAttribute,
            bool isVolumeReadOnly,
            bool isCreateNewFile,
            bool isCaseInsensitive,
            bool isLinkIsDeleted,
            bool isSymbolicLink,
            StreamTypeNameToOpen streamTypeNameToOpen,
            FileType openFileType,
            FileNameStatus fileNameStatus);
        #endregion

        #endregion

        #region 3.1.5.2   Server Requests a Read
        // <summary>
        /// Implement ReadFile Interface
        /// </summary>
        /// <param name="valueOffset">The absolute byte offset in the stream from which to read data.</param>
        /// <param name="valueCount">The desired number of bytes to read.</param>
        /// <param name="byteRead">The number of bytes that were read.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus ReadFile(
            long valueOffset,
            int valueCount,
            out long byteRead);

        /// <summary>
        /// Implement ReadFile Interface
        /// </summary>
        /// <param name="valueOffset">The absolute byte offset in the stream from which to read data.</param>
        /// <param name="valueCount">The desired number of bytes to read.</param>
        /// <param name="byteRead">The number of bytes that were read.</param>
        /// <param name="buffer">The buffer which contains file content read out.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus ReadFile(
            long valueOffset,
            int valueCount,
            out long byteRead,
            out byte[] outBuffer
            );
        #endregion

        #region 3.1.5.3   Server Requests a Write
        /// <summary>
        ///  Implement WriteFile Interface
        /// </summary>
        /// <param name="valueOffset">The absolute byte offset in the stream where data should be written. </param>
        /// <param name="valueCount">The number of bytes in InputBuffer to write.</param>
        /// <param name="bytesWritten">The number of bytes that were written</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        MessageStatus WriteFile(
            long valueOffset,
            long valueCount,
            out long bytesWritten);
        #endregion

        #region 3.1.5.4   Server Requests Closing an Open
        /// <summary>
        /// Implement CloseOpen Interface
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus CloseOpen();
        #endregion

        #region 3.1.5.5   Server Requests Querying a Directory

        #region 3.1.5.5.1   FileObjectIdInformation

        /// <summary>
        /// Implement QueryFileObjectIdInfo Interface
        /// </summary>
        /// <param name="fileNamePattern"> A Unicode string containing the file name pattern to match. </param>
        /// <param name="queryDirectoryScanType">Indicate whether the enumeration should be restarted.</param>
        /// <param name="queryDirectoryFileNameMatchType">The object store MUST search the volume for Files having File.ObjectId matching FileNamePattern.
        /// This parameter indicates if matches the file by FileNamePattern.</param>
        /// <param name="queryDirectoryOutputBufferType">Indicate if OutputBuffer is large enough to hold the first matching entry.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus QueryFileObjectIdInfo(
            FileNamePattern fileNamePattern,
            QueryDirectoryScanType queryDirectoryScanType,
            QueryDirectoryFileNameMatchType queryDirectoryFileNameMatchType,
            QueryDirectoryOutputBufferType queryDirectoryOutputBufferType);

        #endregion

        #region 3.1.5.5.2   FileReparsePointInformation

        /// <summary>
        /// Implement QueryFileReparsePointInformation Interface
        /// </summary>
        /// <param name="fileNamePattern"> A Unicode string containing the file name pattern to match. </param>
        /// <param name="queryDirectoryScanType">Indicate whether the enumeration should be restarted.</param>
        /// <param name="queryDirectoryFileNameMatchType">The object store MUST search the volume for Files having File.ObjectId matching FileNamePattern.
        /// This parameter indicates if matches the file by FileNamePattern.</param>
        /// <param name="queryDirectoryOutputBufferType">Indicate if OutputBuffer is large enough to hold the first matching entry.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus QueryFileReparsePointInformation(
            FileNamePattern fileNamePattern,
            QueryDirectoryScanType queryDirectoryScanType,
            QueryDirectoryFileNameMatchType queryDirectoryFileNameMatchType,
            QueryDirectoryOutputBufferType queryDirectoryOutputBufferType);

        #endregion

        #region 3.1.5.5.3   Directory Information Queries
        /// <summary>
        /// Implement QueryDirectoryInfo Interface
        /// </summary>
        /// <param name="fileNamePattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="restartScan">A boolean value indicates whether the enumeration should be restarted.</param>
        /// <param name="isNoRecordsReturned">True if no record returned.</param>
        /// <param name="isOutBufferSizeLess">True if OutputBufferSize is less than the size needed to return a single entry</param>
        /// <param name="outBufferSize">The state of OutBufferSize in subsection of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus QueryDirectoryInfo(
            FileNamePattern fileNamePattern,
            bool restartScan,
            bool isNoRecordsReturned,
            bool isOutBufferSizeLess,
            OutBufferSmall outBufferSize);
        #endregion

        #endregion

        #region 3.1.5.6   Server Requests Flushing Cached Data
        /// <summary>
        /// Implement FlushCachedData Interface
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        MessageStatus FlushCachedData();
        #endregion

        #region 3.1.5.7   Server Requests a Byte-Range Lock
        /// <summary>
        /// Implement ByteRangeLock Interface
        /// </summary>
        /// </summary>
        /// <param name="fileOffset">A 64-bit unsigned integer containing the starting offset, in bytes.</param>
        /// <param name="length">A 64-bit unsigned integer containing the length, in bytes.</param>
        /// <param name="exclusiveLock">A boolean value indicates whether the range is to be locked exclusively (true) or shared (false).</param>
        /// <param name="failImmediately">A boolean value indicates whether the lock request fails immediately.
        /// True: if the range is locked by another open. False: if it has to wait until the lock can be acquired.</param>
        /// <param name="isOpenNotEqual">True: if open is not equal</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus ByteRangeLock(
            long fileOffset,
            long length,
            bool exclusiveLock,
            bool failImmediately,
            bool isOpenNotEqual);
        #endregion

        #region 3.1.5.8   Server Requests an Unlock of a Byte-Range
        /// <summary>
        /// Implement ByteRangeUnlock Interface
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus ByteRangeUnlock();
        #endregion

        #region 3.1.5.9   Server Requests an FsControl Request

        #region 3.1.5.9.1   FSCTL_CREATE_OR_GET_OBJECT_ID
        /// <summary>
        /// Implement FsCtlCreateOrGetObjId Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isBytesReturnedSet">True: if the return status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlCreateOrGetObjId(
            BufferSize bufferSize,
            out bool isBytesReturnedSet
            );
        #endregion

        #region 3.1.5.9.2   FSCTL_DELETE_OBJECT_ID
        /// <summary>
        /// Implement FsCtlDeleteObjId Interface
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlDeleteObjId();
        #endregion

        #region 3.1.5.9.3   FSCTL_DELETE_REPARSE_POINT
        /// <summary>
        /// Implement FsCtlDeleteReparsePoint Interface
        /// </summary>
        /// <param name="reparseTag">An identifier indicates the type of the reparse point to delete, 
        /// as defined in [MS-FSCC] section 2.1.2.1.</param>
        /// <param name="reparseGuidEqualOpenGuid">True: if Open.File.ReparseGUID == ReparseGUID</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlDeleteReparsePoint(
            ReparseTag reparseTag,
            bool reparseGuidEqualOpenGuid
            );
        #endregion

        #region 3.1.5.9.6   FSCTL_FIND_FILES_BY_SID
        /// <summary>
        /// Implement FsCtlFindFilesBySID Interface
        /// </summary>
        /// <param name="openFileVolQuoInfoEmpty">True: if Open.File.Volume.QuotaInformation is empty</param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="linkOwnerSidEqualSID">True: if linkOwnerSid equals SID.</param>
        /// <param name="outputBufferLessLinkSize">True: if the outputbuffer is less than linksize.</param>
        /// <param name="isOutputBufferOffset">True: if the outputbuffer is offset</param>
        /// <param name="isBytesReturnedSet">True: if returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        MessageStatus FsCtlFindFilesBySID(
            bool openFileVolumeQuoInfoEmpty,
            BufferSize bufferSize,
            bool linkOwnerSidEqualSid,
            bool outputBufferLessLinkSize,
            bool isOutputBufferOffset,
            out bool isBytesReturnedSet);
        #endregion

        #region 3.1.5.9.11   FSCTL_GET_REPARSE_POINT
        /// <summary>
        /// Implement FsCtlGetReparsePoint Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="openFileReparseTag">Open.File.ReparseTag </param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlGetReparsePoint(
            BufferSize bufferSize,
            ReparseTag openFileReparseTag,
            out bool isBytesReturnedSet
            );
        #endregion

        #region 3.1.5.9.12   FSCTL_GET_RETRIEVAL_POINTERS
        /// <summary>
        /// Implement FsCtlGetRetrivalPoints Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isStartingNegative">True: if StartingVcnBuffer.StartingVcn is negative</param>
        /// <param name="isStartingGreatThanAllocationSize">True: if StartingVcnBuffer.StartingVcn is 
        /// greater than or equal to Open.Stream.AllocationSize divided by Open.File.Volume.ClusterSize</param>
        /// <param name="isElementsNotAllocated">True: if not all the elements in Open.Stream.ExtentList
        /// were copied into OutputBuffer.Extents.</param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlGetRetrivalPoints(
            BufferSize bufferSize,
            bool isStartingNegative,
            bool isStartingGreatThanAllocationSize,
            bool isElementsNotAllocated,
            out bool isBytesReturnedSet
            );
        #endregion

        #region 3.1.5.9.19   FSCTL_QUERY_ALLOCATED_RANGES
        /// <summary>
        /// Implement FsCtlQueryAllocatedRanges Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="inputBuffer">Indicate buffer length</param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlQueryAllocatedRanges(
            BufferSize bufferSize,
            BufferLength inputBuffer,
            out bool isBytesReturnedSet
            );
        #endregion

        #region 3.1.5.9.22   FSCTL_READ_FILE_USN_DATA
        /// <summary>
        /// Implement FsCtlReadFileUSNData Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlReadFileUSNData(
            BufferSize bufferSize,
            out bool isBytesReturnedSet
            );
        #endregion

        #region 3.1.5.9.24   FSCTL_SET_COMPRESSION
        /// <summary>
        /// Implement FsCtlSetCompression Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="compressionState">This string value indicates the InputBuffer.CompressionState.
        /// <param name="isCompressionSupportEnable">True: if compression support is enabled</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlSetCompression(
            BufferSize bufferSize,
            InputBufferCompressionState compressionState,
            bool isCompressionSupportEnable
            );
        #endregion

        #region 3.1.5.9.25   FSCTL_SET_DEFECT_MANAGEMENT
        /// <summary>
        /// Implement FsCtlSetDefectManagement Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size.</param>
        /// <param name="isOpenListContainMoreThanOneOpen">True: if openList contains more than one open</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlSetDefectManagement(
            BufferSize bufferSize,
            bool isOpenListContainMoreThanOneOpen
            );
        #endregion

        #region FsCtlForEasyRequest
        /// <summary>
        /// Implement FsCtlForEasyRequest Interface
        /// </summary>
        /// <param name="requestType">Request type</param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <param name="isOutputBufferSizeReturn">True: if return OutputBufferSize</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlForEasyRequest(
            FsControlRequestType requestType,
            BufferSize bufferSize,
            out bool isBytesReturnedSet,
            out bool isOutputBufferSizeReturn
            );
        #endregion

        #region 3.1.5.9.26   FSCTL_SET_ENCRYPTION
        /// <summary>
        /// Implement FsCtlSetEncryption Interface
        /// </summary>
        /// <param name="isIsCompressedTrue">True: if stream is compressed.</param>
        /// <param name="encryptionOperation">InputBuffer.EncryptionOperation </param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlSetEncryption(
            bool isIsCompressedTrue,
            EncryptionOperation encryptionOperation,
            BufferSize bufferSize
            );
        #endregion

        #region 3.1.5.9.28   FSCTL_SET_OBJECT_ID
        /// <summary>
        /// Implement FsCtlSetObjID Interface
        /// </summary>
        /// <param name="requestType">FsControlRequestType is self-defined to indicate control type</param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlSetObjID(
            FsControlRequestType requestType,
            BufferSize bufferSize
            );
        #endregion

        #region 3.1.5.9.32   FSCTL_SET_SPARSE
        /// <summary>
        /// Implement FsCtlSetReparsePoint Interface
        /// </summary>
        /// <param name="inputReparseTag">InputBuffer.ReparseTag</param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isReparseGuidNotEqual">True: if Open.File.ReparseGUID is not equal to InputBuffer.ReparseGUID</param>
        /// <param name="isFileReparseTagNotEqualInputBufferReparseTag">True: if Open.File.ReparseTag is not equal to 
        /// InputBuffer.ReparseTag.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlSetReparsePoint(
            ReparseTag inputReparseTag,
            BufferSize bufferSize,
            bool isReparseGuidNotEqual,
            bool isFileReparseTagNotEqualInputBufferReparseTag
            );
        #endregion

        #region 3.1.5.9.33   FSCTL_SET_ZERO_DATA
        /// <summary>
        /// Implement FsCtlSetZeroData Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="inputBuffer">InputBuffer_FSCTL_SET_ZERO_DATA</param>
        /// <param name="isDeletedTrue">True: if Open.Stream.IsDeleted</param>
        /// <param name="isConflictDetected">True: if a conflict is detected.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsCtlSetZeroData(
            BufferSize bufferSize,
            InputBuffer_FSCTL_SET_ZERO_DATA inputBuffer,
            bool isIsDeletedTrue,
            bool isConflictDetected
            );
        #endregion

        #region 3.1.5.9.35   FSCTL_SIS_COPYFILE
        /// <summary>
        /// Implement FsctlSisCopyFile Interface
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="inputBuffer">InputBufferFSCTL_SIS_COPYFILE</param>
        /// <param name="isCopyFileSisLinkTrue">True: if InputBuffer.Flags.COPYFILE_SIS_LINK is true</param>
        /// <param name="isIsEncryptedTrue">True if encrypted</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus FsctlSisCopyFile(
            BufferSize bufferSize,
            InputBufferFSCTL_SIS_COPYFILE inputBuffer,
            bool isCopyFileSisLinkTrue,
            bool isIsEncryptedTrue
            );
        #endregion

        #region FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX
        /// <summary>
        /// Implement FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX, which duplicates file extents from the same opened file.
        /// </summary>
        /// <param name="sourceFileOffset">Offset of source file.</param>
        /// <param name="targetFileOffset">Offset of target file.</param>
        /// <param name="byteCount">The number of bytes of duplicate.</param>
        /// <param name="flags">Flags for duplicate file.</param>
        /// <returns></returns>
        MessageStatus FsctlDuplicateExtentsToFileEx(
            long sourceFileOffset,
            long targetFileOffset,
            long byteCount,
            FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request_Flags_Values flags
            );
        #endregion

        #endregion

        #region 3.1.5.10   Server Requests Change Notifications for a Directory
        /// <summary>
        /// Implement ChangeNotificationForDir Interface
        /// </summary>
        /// <param name="changeNotifyEntryType">ChangeNotifyEntryType to indicate if all entries from ChangeNotifyEntry.NotifyEventList
        /// fit in OutputBufferSize bytes</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus ChangeNotificationForDir(ChangeNotifyEntryType changeNotifyEntryType);
        #endregion

        #region 3.1.5.11   Server Requests a Query of File Information

        /// <summary>
        /// Implement QueryFileInfoPart1 Interface
        /// </summary>
        /// <param name="fileInfoClass">The type of information being queried, as specified in [MS-FSCC] section 2.5.</param>
        /// <param name="outputBufferSize">Indicate output buffer size</param>
        /// <param name="byteCount">The desired number of bytes to query.</param>
        /// <param name="outputBuffer">Indicate output buffer</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus QueryFileInfoPart1(
             FileInfoClass fileInfoClass,
            OutputBufferSize outputBufferSize,
            out ByteCount byteCount,
            out OutputBuffer outputBuffer
            );

        #endregion

        #region 3.1.5.12   Server Requests a Query of File System Information
        /// <summary>
        /// Implement QueryFileSystemInfo Interface
        /// </summary>
        /// <param name="fileInfoClass">The type of information being queried, as specified in [MS-FSCC] section 2.5.</param>
        /// <param name="outputBufferSize">This value indicates size of output buffer. </param>
        /// <param name="byteCount">The number of bytes stored in OutputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus QueryFileSystemInfo(
            FileSystemInfoClass fileInfoClass,
            OutputBufferSize outputBufferSize,
            out FsInfoByteCount byteCount);
        #endregion

        #region 3.1.5.13   Server Requests a Query of Security Information
        /// <summary>
        /// Implement QuerySecurityInfo Interface
        /// </summary>
        /// <param name="securityInformation">A SECURITY_INFORMATION data type, as defined in [MS-DTYP] section 2.4.7.</param>
        /// <param name="isByteCountGreater">True: if ByteCount is greater than OutputBufferSize</param>
        /// <param name="outputBuffer">Indicate output buffer</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus QuerySecurityInfo(
            SecurityInformation securityInformation,
            bool isByteCountGreater,
            out OutputBuffer outputBuffer
            );
        #endregion

        #region 3.1.5.14   Server Requests Setting of File Information

        #region 3.1.5.14.1   FileAllocationInformation
        /// <summary>
        /// Implement SetFileAllocOrObjIdInfo Interface
        /// </summary>
        /// <param name="fileInfoClass">The type of information being applied, as specified in [MS-FSCC] section 2.4.</param>
        /// <param name="allocationSizeType">Indicates if InputBuffer.AllocationSize is greater than the maximum file size allowed by the object store.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileAllocOrObjIdInfo(
            FileInfoClass fileInfoClass,
            AllocationSizeType allocationSizeType
            );
        #endregion

        #region 3.1.5.14.2   FileBasicInformation
        /// <summary>
        /// Implement SetFileBasicInfo Interface
        /// </summary>
        /// <param name="inputBufferSize">Indicate inputBuffer size</param>
        /// <param name="inputBufferTime">Indicate inputBuffer time</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileBasicInfo(
            InputBufferSize inputBufferSize,
            InputBufferTime inputBufferTime
            );
        #endregion

        #region 3.1.5.14.3   FileDispositionInformation
        /// <summary>
        /// Implement SetFileDispositionInfo Interface
        /// </summary>
        /// <param name="fileDispositionType">Indicates if the file is set to delete pending.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileDispositionInfo(
            FileDispositionType fileDispositionType
            );
        #endregion

        #region 3.1.5.14.4   FileEndOfFileInformation
        /// <summary>
        /// Implement SetFileEndOfFileInfo Interface
        /// </summary>
        /// <param name="isEndOfFileGreatThanMaxSize">True: if InputBuffer.EndOfFile is greater than the maximum file size</param>
        /// <param name="isSizeEqualEndOfFile">True: if Open.Stream.Size is equal to InputBuffer.EndOfFile</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileEndOfFileInfo(
            bool isEndOfFileGreatThanMaxSize,
            bool isSizeEqualEndOfFile
            );
        #endregion

        #region 3.1.5.14.5   FileFullEaInformation
        /// <summary>
        /// Implement SetFileFullEaInfo Interface
        /// </summary>
        /// <param name="eAValidate">Indicate Ea Information</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileFullEaInfo(
            EainInputBuffer eAValidate
            );
        #endregion

        #region 3.1.5.14.6   FileLinkInformation
        /// <summary>
        /// Implement SetFileLinkInfo Interface
        /// </summary>
        /// <param name="inputNameInvalid">True: if InputBuffer.FileName is not valid as specified in [MS-FSCC] section 2.1.5</param>
        /// <param name="replaceIfExist">InputBuffer.ReplaceIfExists </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileLinkInfo(
            bool inputNameInvalid,
            bool replaceIfExist
            );
        #endregion

        #region 3.1.5.14.7   FileModeInformation
        /// <summary>
        /// Implement SetFileModeInfo Interface
        /// </summary>
        /// <param name="inputMode">InputBuffer.Mode </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileModeInfo(
            FileMode inputMode
            );
        #endregion

        #region 3.1.5.14.9   FilePositionInformation
        /// <summary>
        /// Implement SetFilePositionInfo Interface
        /// </summary>
        /// <param name="inputBufferSize">Indicate inputBuffer size</param>
        /// <param name="currentByteOffset">Indicate InputBuffer.CurrentByteOffset</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFilePositionInfo(
            InputBufferSize inputBufferSize,
            InputBufferCurrentByteOffset currentByteOffset
            );
        #endregion

        #region 3.1.5.14.11   FileRenameInformation
        /// <summary>
        /// Implement SetFileRenameInfo Interface
        /// </summary>
        /// <param name="inputBufferFileNameLength">InputBufferFileNameLength</param>
        /// <param name="inputBufferFileName">InputbBufferFileName</param>
        /// <param name="directoryVolumeType">Indicate if DestinationDirectory.Volume is equal to Open.File.Volume.</param>
        /// <param name="destinationDirectoryType">Indicate if DestinationDirectory is the same as Open.Link.ParentFile.</param>
        /// <param name="newLinkNameFormatType">Indicate if NewLinkName is case-sensitive</param>
        /// <param name="newLinkNameMatchType">Indicate if NewLinkName matched TargetLink.ShortName</param>
        /// <param name="replacementType">Indicate if replace target file if exists.</param>
        /// <param name="targetLinkDeleteType">Indicate if TargetLink is deleted or not.</param>
        /// <param name="oplockBreakStatusType">Indicate if there is an oplock to be broken</param>
        /// <param name="targetLinkFileOpenListType">Indicate if TargetLink.File.OpenList contains an Open with a Stream matching the current Stream.</param>
        /// <returns>An NTSTATUS code indicating the result of the operation.</returns>
        MessageStatus SetFileRenameInfo(
            InputBufferFileNameLength inputBufferFileNameLength,
            InputBufferFileName inputBufferFileName,
            DirectoryVolumeType directoryVolumeType,
            DestinationDirectoryType destinationDirectoryType,
            NewLinkNameFormatType newLinkNameFormatType,
            NewLinkNameMatchType newLinkNameMatchType,
            ReplacementType replacementType,
            TargetLinkDeleteType targetLinkDeleteType,
            OplockBreakStatusType oplockBreakStatusType,
            TargetLinkFileOpenListType targetLinkFileOpenListType
            );
        #endregion

        #region 3.1.5.14.11.1   Algorithm for Performing Stream Rename
        /// <summary>
        /// Implement StreamRename Interface
        /// </summary>
        /// <param name="newStreamNameFormat">The format of NewStreamName</param>
        /// <param name="streamTypeNameFormat">The format of StreamType</param>
        /// <param name="replacementType">Indicate if replace target file if exists.</param>
        /// <returns>An NTSTATUS code indicating the result of the operation</returns>
        MessageStatus StreamRename(
            InputBufferFileName newStreamNameFormat,
            InputBufferFileName streamTypeNameFormat,
            ReplacementType replacementType
            );
        #endregion

        #region 3.1.5.14.13   FileShortNameInformation
        /// <summary>
        /// Implement SetFileShortNameInfo Interface
        /// </summary>
        /// <param name="inputBufferFileName">InputBuffer.FileName. This value equals "Notvalid" 
        /// if InputBuffer.FileName is not a valid 8.3 file name as described in [MS-FSCC] section 2.1.5.2.1.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileShortNameInfo(
            InputBufferFileName inputBufferFileName
            );
        #endregion

        #region 3.1.5.14.14   FileValidDataLengthInformation
        ///<summary>
        /// Implement SetFileValidDataLengthInfo Interface
        ///</summary>
        ///<param name="isStreamValidLengthGreater">True: if Open.Stream.ValidDataLength is greater than InputBuffer.ValidDataLength</param>
        ///<returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetFileValidDataLengthInfo(
            bool isStreamValidLengthGreater
            );
        #endregion

        #endregion

        #region 3.1.5.15   Server Requests Setting of File System Information
        /// <summary>
        /// Implement SetFileSystemInformation Interface
        /// </summary>
        /// <param name="fileInfoClass">The type of information being applied, as specified in [MS-FSCC] section 2.5.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        MessageStatus SetFileSystemInformation(
            UInt32 fsInformationClass,
            byte[] buffer);

        /// <summary>
        /// SetFileSysInfo interface for model
        /// </summary>
        /// <param name="fileInfoClass">The type of information being applied, as specified in [MS-FSCC] section 2.5.</param>
        /// <param name="inputBufferSize">Indicate input buffer size.</param>
        /// <returns></returns>
        MessageStatus SetFileSysInfo(
            FileSystemInfoClass fileInfoClass,
            InputBufferSize inputBufferSize);
        #endregion

        #region 3.1.5.16   Server Requests Setting of Security Information
        ///<summary>
        /// Implement SetSecurityInfo Interface
        ///</summary>
        ///<param name="securityInformation">Indicate SecurityInformation</param>
        ///<param name="ownerSidEnum">Indicate InputBuffer.OwnerSid</param>
        ///<returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus SetSecurityInfo(
            SecurityInformation securityInformation,
            OwnerSid ownerSidEnum);
        #endregion

        #region 3.1.5.17   Server Requests an Oplock

        /// <summary>
        /// Implement Oplock Interface
        /// </summary>
        /// <param name="opType">Oplock type</param>
        /// <param name="openListContainStream">True: if Open.File.OpenList contains more than one 
        /// Open whose Stream is the same as Open.Stream.</param>
        /// <param name="opLockLevel">Requested oplock level</param>
        /// <param name="isOpenStreamOplockEmpty">True: if Open.Steam.Oplock is empty</param>
        /// <param name="oplockState">To specify the current state of the oplock, expressed as a combination of one or more flags</param>
        /// <param name="streamIsDeleted">True: if stream is deleted.</param>
        /// <param name="isRHBreakQueueEmpty">True: if Open.Stream.Oplock.State.RHBreakQueue is empty</param>
        /// <param name="isOplockKeyEqual">True: if ThisOpen.OplockKey == Open.OplockKey</param>
        /// <param name="isOplockKeyEqualExclusive">False: if Open.OplockKey != Open.Stream.Oplock.ExclusiveOpen.OplockKey</param>
        /// <param name="requestOplock">Request oplock</param>
        /// <param name="grantingInAck">True: if this oplock is being requested as part of an oplock break acknowledgement</param>
        /// <param name="keyEqualOnRHOplocks">True: if there is an Open on Open.Stream.Oplock.RHOplocks whose OplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnRHBreakQueue">True: if there is an Open on Open.Stream.Oplock.RHBreakQueue whose OplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnROplocks">True: if there is an Open ThisOpen on Open.Stream.Oplock.ROplocks whose OplockKey is equal to Open.OplockKey</param>
        /// <returns> An NTSTATUS code indicating the result of the operation</returns>
        MessageStatus Oplock(
            OpType opType,
            bool openListContainStream,
            RequestedOplockLevel opLockLevel,
            bool isOpenStreamOplockEmpty,
            OplockState oplockState,
            bool streamIsDeleted,
            bool isRHBreakQueueEmpty,
            bool isOplockKeyEqual,
            bool isOplockKeyEqualExclusive,
            RequestedOplock requestOplock,
            bool grantingInAck,
            bool keyEqualOnRHOplocks,
            bool keyEqualOnRHBreakQueue,
            bool keyEqualOnROplocks);

        #endregion

        #region 3.1.5.18   Server Acknowledges an Oplock Break

        /// <summary>
        /// Server Acknowledges an Oplock Break
        /// </summary>
        /// <param name="OpenStreamOplockEmpty">True: if Open.Stream.Oplock is empty</param>
        /// <param name="opType">Oplock type</param>
        /// <param name="level">Requested Oplock level</param>
        /// <param name="ExclusiveOpenEqual">True: if Open.Stream.Oplock.ExclusiveOpen is not equal to Open</param>
        /// <param name="oplockState">Oplock state</param>
        /// <param name="RHBreakQueueIsEmpty">True: if Open.Stream.Oplock.RHBreakQueue is empty</param>
        /// <param name="ThisContextOpenEqual">True: if ThisContext.Open equals Open</param>
        /// <param name="ThisContextBreakingToRead">False: if ThisContext.BreakingToRead is false</param>
        /// <param name="OplockWaitListEmpty">False: if Open.Stream.Oplock.WaitList is not empty</param>
        /// <param name="StreamIsDeleted">True: if Open.Stream.IsDeleted is true</param>
        /// <param name="GrantingInAck">True: if this oplock is being requested as part of an oplock break acknowledgement, as defined in [MS-FSA] 3.1.5.17.2</param>
        /// <param name="keyEqualOnRHOplocks">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <param name="keyEqualOnRHBreakQueue">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <param name="keyEqualOnROplocks">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <param name="requestOplock">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <returns> An NTSTATUS code indicating the result of the operation</returns>
        MessageStatus OplockBreakAcknowledge(
            bool OpenStreamOplockEmpty,
            OpType opType,
            RequestedOplockLevel level,
            bool ExclusiveOpenEqual,
            OplockState oplockState,
            bool RHBreakQueueIsEmpty,
            bool ThisContextOpenEqual,
            bool ThisContextBreakingToRead,
            bool OplockWaitListEmpty,
            bool StreamIsDeleted,
            bool GrantingInAck,
            bool keyEqualOnRHOplocks,
            bool keyEqualOnRHBreakQueue,
            bool keyEqualOnROplocks,
            RequestedOplock requestOplock
            );

        #endregion

        #region 3.1.5.19   Server Requests Canceling an Operation

        /// <summary>
        /// Implement CancelinganOperation Interface
        /// Server requests to cancel an Operation
        /// This method is called by 3.1.5.7, and the scenario is in Scenario16_ByteRangeLock
        /// </summary>
        /// <param name="ioRequest">An implementation-specific identifier that is unique for each 
        /// outstanding IO operation. See [MS-CIFS] section 3.3.5.51.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus CancelinganOperation(IORequest ioRequest);

        #endregion

        #region 3.1.5.20   Server Requests Querying Quota Information

        /// <summary>
        /// Implement QueryFileQuotaInformation Interface
        /// </summary>
        /// <param name="state">Sid list state</param>
        /// <param name="isOutBufferSizeLess">True if OutputBufferSize is less than the size of FILE_QUOTA_INFORMATION
        /// multiplied by the number of elements in SidList</param>
        /// <param name="restartScan">True if EmptyPattern is true</param>
        /// <param name="isDirectoryNotRight">True if there is no match</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        MessageStatus QueryFileQuotaInformation(
            SidListState state,
            bool isOutBufferSizeLess,
            bool restartScan,
            bool isDirectoryNotRight);

        #endregion

        #endregion

        #region Adapter Utilities

        /// <summary>
        /// Get SUT platformType.
        /// </summary>
        /// <param name="platformType">SUT OS platformType</param>
        void GetOSInfo(out PlatformType platformType);

        /// <summary>
        /// Get the system config information.
        /// </summary>
        /// <param name="securityContext">SSecurityContext</param>
        void GetSystemConfig(out SSecurityContext securityContext);

        /// <summary>
        /// To get whether implement object functionality
        /// </summary>
        /// <param name="isImplemented">True: if Object is functionality</param>
        void GetObjectFunctionality(out bool isImplemented);

        /// <summary>
        /// To get whether ObjectIDs is supported
        /// </summary>
        /// <param name="isSupported">True: if ObjectIDs is supported</param>
        void GetObjectIDsSupported(out bool isSupported);

        /// <summary>
        /// To get whether reparse points is supported
        /// </summary>
        /// <param name="isSupported">True: if ReparsePoints is supported</param>
        void GetReparsePointsSupported(out bool isSupported);

        /// <summary>
        /// To get whether open has manage.vol.privilege
        /// </summary>
        /// <param name="isSupported">True: if open has manage.vol.privilege.</param>
        void GetopenHasManageVolPrivilege(out bool isSupported);

        /// <summary>
        /// Get is restore has access
        /// </summary>
        /// <param name="isHas">True: if Restore has access.</param>
        void GetRestoreAccess(out bool isHas);

        /// <summary>
        /// To get whether is administrator
        /// </summary>
        /// <param name="isGet">True: if administrator is got</param>
        void GetAdministrator(out bool isGet);

        /// <summary>
        /// To get whether open generate shortname
        /// </summary>
        /// <param name="GenerateShortNames">True: if Open generate shortnames</param>
        void GetOpenGenerateShortNames(out bool generateShortNames);

        /// <summary>
        /// To get whether Open.File.OpenList contains more than one Open on open stream.
        /// defined in 3.1.5.17
        /// </summary>
        /// <param name="openListContains">True: if Open.ListContains</param>
        void GetIsOpenListContains(out bool openListContains);

        /// <summary>
        /// To get whether the link, through which file is opened, is found
        /// </summary>
        /// <param name="IsLinkFound">True: if Link is Found</param>
        void GetIsLinkFound(out bool isLinkFound);

        /// <summary>
        /// Get file volum size.
        /// </summary>
        /// <param name="openFileVolumeSize">The volume size of the file opened</param>
        void GetFileVolumSize(out long openFileVolumeSize);

        /// <summary> 
        /// To get whether Quotas is supported.
        /// </summary>
        /// <param name="isQuotasSupported">True: if Quotas is supported</param>
        void GetIFQuotasSupported(out bool isQuotasSupported);

        /// <summary> 
        /// To get whether ObjectIDs is supported.
        /// </summary>
        /// <param name="isObjectIDsSupported">True: if ObjectIDs is supported</param>
        void GetIFObjectIDsSupported(out bool isObjectIDsSupported);

        /// <summary>
        /// Check if requirement R507 is implemented
        /// </summary>
        /// <param name="isFlagValue">A boolean value to specify the result</param>
        void CheckIsR507Implemented(out bool isFlagValue);

        /// <summary>
        /// Check if requirement R405 is implemented
        /// </summary>
        /// <param name="isFlagValue">A boolean value to specify the result</param>
        void CheckIsR405Implemented(out bool isFlagValue);

        /// <summary>
        /// To get whether Query FileFsControlInformation is implemented.
        /// </summary>
        /// <param name="isImplemented">True: if this functionality is implemented by the object store.</param>
        void GetIfImplementQueryFileFsControlInformation(out bool isImplemented);

        /// <summary>
        /// To get whether Query FileFsObjectIdInformation is implemented.
        /// </summary>
        /// <param name="isImplemented">True: if this functionality is implemented by the object store.</param>
        void GetIfImplementQueryFileFsObjectIdInformation(out bool isImplemented);

        /// <summary>
        /// To get whether Open.File.Volume is read only.
        /// </summary>
        /// <param name="isReadOnly"></param>
        void GetIfOpenFileVolumeIsReadOnly(out bool isReadOnly);

        /// <summary>
        /// To get whether Open.File equals to Root Directory.
        /// </summary>
        /// <param name="isEqualRootDirectory"></param>
        void GetIfOpenFileEqualRootDirectory(out bool isEqualRootDirectory);

        /// <summary>        
        /// Get if Query FileObjectIdInformation is implemented        
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>        
        void GetIfImplementQueryFileObjectIdInformation(out bool isImplemented);

        /// <summary>
        /// Get if Query FileReparsePointInformation is implemented
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        void GetIfImplementQueryFileReparsePointInformation(out bool isImplemented);

        /// <summary>
        /// Get if Query FileQuotaInformation is implemented
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        void GetIfImplementQueryFileQuotaInformation(out bool isImplemented);

        /// <summary>
        /// Get if ObjectId related IoCtl requests are implemented
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        void GetIfImplementObjectIdIoCtlRequest(out bool isImplemented);

        /// <summary>
        /// Get if Open.File.Volume is NTFS file system.
        /// </summary>
        /// <param name="isNtfsFileSystem">true: if it is NTFS File System.</param>
        void GetIfNtfsFileSystem(out bool isNtfsFileSystem);

        /// <summary>
        /// Get If Open Has Manage Volume Access. 
        /// </summary>
        /// <param name="isHasManageVolumeAccess">true: if open has manage volume access.</param>
        void GetIfOpenHasManageVolumeAccess(out bool isHasManageVolumeAccess);

        /// <summary>
        /// Get if Stream Rename is supported.
        /// </summary>
        /// <param name="isSupported">true: if stream rename is supported.</param>
        void GetIfStreamRenameIsSupported(out bool isSupported);
        #endregion

    }
}