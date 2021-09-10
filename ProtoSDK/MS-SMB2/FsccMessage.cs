// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// File information classes are numerical values (specified by the Level column in the following table) 
    /// that specify what information for a file is to be queried or set
    /// </summary>
    public enum FileInformationClasses
    {
        /// <summary>
        /// This information class is used to query the access rights of a file.
        /// </summary>
        FileAccessInformation = 8,

        /// <summary>
        /// The buffer alignment required by the underlying device.
        /// </summary>
        FileAlignmentInformation = 17,

        /// <summary>
        /// This information class is used to query a collection of file information structures.
        /// </summary>
        FileAllInformation = 18,

        /// <summary>
        /// This information class is used to query alternate name information for a file.
        /// </summary>
        FileAlternateNameInformation = 21,

        /// <summary>
        /// This information class is used to query for attribute and reparse tag information for a file.
        /// </summary>
        FileAttributeTagInformation = 35,

        /// <summary>
        /// This information class is used to query or set file information.
        /// </summary>
        FileBasicInformation = 4,

        /// <summary>
        /// This information class is used to query compression information for a file
        /// </summary>
        FileCompressionInformation = 28,

        /// <summary>
        /// This information class is used to mark a file for deletion. 
        /// </summary>
        FileDispositionInformation = 13,

        /// <summary>
        /// This information class is used to query for the size of the extended attributes (EA) for a file.
        /// </summary>
        FileEaInformation = 7,

        /// <summary>
        /// This information class is used to query or set extended attribute (EA) information for a file.
        /// </summary>
        FileFullEaInformation = 15,

        /// <summary>
        /// This information class is used to query NTFS hard links to an existing file.
        /// </summary>
        FileHardLinkInformation = 46,

        /// <summary>
        /// This information class is used to query transactional visibility information for the files in a directory
        /// </summary>
        FileIdGlobalTxDirectoryInformation = 50,

        /// <summary>
        /// This information class is used to query for the file system's 8-byte file reference number for a file.
        /// </summary>
        FileInternalInformation = 6,

        /// <summary>
        /// This information class is used to query or set the mode of the file.
        /// </summary>
        FileModeInformation = 16,

        /// <summary>
        /// This information class is used to query for information on a network file open.
        /// </summary>
        FileNetworkOpenInformation = 34,

        /// <summary>
        /// Windows file systems do not implement this file information class; 
        /// the server will fail it with STATUS_NOT_SUPPORTED.
        /// </summary>
        FileNormalizedNameInformation = 48,

        /// <summary>
        /// This information class is used to query or set information on a named pipe that is not
        /// specific to one end of the pipe or another.
        /// </summary>
        FilePipeInformation = 23,

        /// <summary>
        /// This information class is used to query information on a named pipe 
        /// that is associated with the end of the pipe that is being queried.
        /// </summary>
        FilePipeLocalInformation = 24,

        /// <summary>
        /// This information class is used to query or set information on a named pipe 
        /// that is associated with the client end of the pipe that is being queried.
        /// </summary>
        FilePipeRemoteInformation = 25,

        /// <summary>
        /// This information class is used to query or set the position of the file pointer within a file.
        /// </summary>
        FilePositionInformation = 14,

        /// <summary>
        /// The information class is used to query quota information.
        /// </summary>
        FileQuotaInformation = 32,

        /// <summary>
        /// This information class is used to rename a file
        /// </summary>
        FileRenameInformation = 10,

        /// <summary>
        /// This information class is used to query or set reserved bandwidth for a file handle.
        /// </summary>
        FileSfioReserveInformation = 44,

        /// <summary>
        /// This information class is used to query file information
        /// </summary>
        FileStandardInformation = 5,

        /// <summary>
        /// This information class is used to query file link information
        /// </summary>
        FileStandardLinkInformation = 54,

        /// <summary>
        /// This information class is used to enumerate the data streams for a file.
        /// </summary>
        FileStreamInformation = 22,

        /// <summary>
        /// This information class is used to set end-of-file information for a file.
        /// </summary>
        FileEndOfFileInformation = 20,
    }

    /// <summary>
    /// File system information classes are numerical values 
    /// (specified by the Level column in the following table) that specify what information
    /// on a particular instance of a file system on a volume is to be queried.
    /// </summary>
    public enum FileSystemInformationClasses
    {
        /// <summary>
        /// This information class is used to query attribute information for a file system.
        /// </summary>
        FileFsAttributeInformation = 5,

        /// <summary>
        /// This information class is used to query device information associated with a file system volume.
        /// </summary>
        FileFsDeviceInformation = 4,

        /// <summary>
        /// This information class is used to query sector size information for a file system volume.
        /// </summary>
        FileFsFullSizeInformation = 7,

        /// <summary>
        /// This information class is used to query or set the object ID for a file system data element.
        /// </summary>
        FileFsObjectIdInformation = 8,

        /// <summary>
        /// This information class is used to query sector size information for a file system volume.
        /// </summary>
        FileFsSizeInformation = 3,

        /// <summary>
        /// This information class is used to query information on a volume on which a file system is mounted.
        /// </summary>
        FileFsVolumeInformation = 1
    }

    /// <summary>
    /// This information class is used to query or set information on a named pipe 
    /// that is associated with the client end of the pipe that is being queried
    /// </summary>
    public struct FilePipeRemoteInformation
    {
        /// <summary>
        /// A LARGE_INTEGER that MUST contain the maximum amount of time counted 
        /// in 100-nanosecond intervals that will elapse before transmission of 
        /// data from the client machine to the server.
        /// </summary>
        public ulong CollectDataTime;

        /// <summary>
        /// A ULONG that MUST contain the maximum size in bytes of data that will 
        /// be collected on the client machine before transmission to the server.
        /// </summary>
        public uint MaximumCollectionCount;
    }

    /// <summary>
    /// A 32-bit unsigned integer referring to the current state of the pipe
    /// </summary>
    public enum Named_Pipe_State_Value
    {
        /// <summary>
        /// The specified named pipe is in the disconnected state
        /// </summary>
        FILE_PIPE_DISCONNECTED_STATE = 0x01,

        /// <summary>
        /// The specified named pipe is in the listening state
        /// </summary>
        FILE_PIPE_LISTENING_STATE = 0x02,

        /// <summary>
        /// The specified named pipe is in the connected state.
        /// </summary>
        FILE_PIPE_CONNECTED_STATE = 0x03,

        /// <summary>
        /// The specified named pipe is in the closing state.
        /// </summary>
        FILE_PIPE_CLOSING_STATE = 0x04
    }

    /// <summary>
    /// The FSCTL_PIPE_PEEK response returns data from the pipe server's output buffer in the FSCTL output buffer
    /// </summary>
    public struct FSCTL_PIPE_PEEK_Reply
    {
        /// <summary>
        /// A 32-bit unsigned integer referring to the current state of the pipe
        /// </summary>
        public Named_Pipe_State_Value NamedPipeState;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the size, in bytes, of the data available to read from the pipe
        /// </summary>
        public uint ReadDataAvailable;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the number of messages available
        /// in the pipe if the pipe has been created as a message-type pipe
        /// </summary>
        public uint NumberOfMessages;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the length of the first message
        /// available in the pipe if the pipe has been created as a message-type pipe. 
        /// Otherwise, this field is 0
        /// </summary>
        public uint MessageLength;

        /// <summary>
        /// A byte buffer of preview data from the pipe.
        /// The length of the buffer is indicated by the value of the ReadDataAvailable field
        /// </summary>
        public byte[] Data;
    }

    /// <summary>
    /// The FSCTL_FILE_LEVEL_TRIM operation informs the underlying storage medium that the contents 
    /// of the given range of the file no longer needs to be maintained. This message allows the storage 
    /// medium to manage its space more efficiently.
    /// </summary>
    public struct FSCTL_FILE_LEVEL_TRIM_INPUT
    {
        /// <summary>
        /// This field is used for byte range locks to uniquely identify different consumers of byte range
        /// locks on the same thread. Typically, this field is used only by remote protocols such as SMB or SMB2
        /// </summary>
        public uint Key;

        /// <summary>
        /// A count of how many Offset, Length pairs follow in the data item
        /// </summary>
        public uint NumRanges;

        /// <summary>
        /// An array of zero or more FILE_LEVEL_TRIM_RANGE (section 2.3.69.1) data elements. 
        /// The NumRanges field contains the number of FILE_LEVEL_TRIM_RANGE data elements in the array
        /// </summary>
        [Size("NumRanges")]
        public FSCTL_FILE_LEVEL_TRIM_RANGE[] Ranges;
    }

    /// <summary>
    /// 
    /// </summary>
    public struct FSCTL_FILE_LEVEL_TRIM_RANGE
    {
        /// <summary>
        /// A 64-bit unsigned integer that contains a byte offset 
        /// into the given file at which to start the trim request
        /// </summary>
        public ulong Offset;
        
        /// <summary>
        /// A 64-bit unsigned integer that contains the length, 
        /// in bytes, of how much of the file to trim, starting at Offset
        /// </summary>
        public ulong Length;
    }

    public struct FSCTL_FILE_LEVEL_TRIM_OUTPUT
    {
        /// <summary>
        /// A 32-bit unsigned integer identifying the number of input ranges that were processed
        /// </summary>
        public uint NumRangesProcessed;
    }

    public struct FSCTL_GET_INTEGRITY_INFO_OUTPUT
    {
        public FSCTL_GET_INTEGRITY_INFO_OUTPUT_CHECKSUMALGORITHM ChecksumAlgorithm;
        public FSCTL_GET_INTEGRITY_INFO_OUTPUT_RESERVED Reserved;
        public FSCTL_GET_INTEGRITY_INFO_OUTPUT_FLAGS Flags;
        public uint ChecksumChunkSizeInBytes;
        public uint ClusterSizeInBytes;
    }

    public enum FSCTL_GET_INTEGRITY_INFO_OUTPUT_CHECKSUMALGORITHM : ushort
    {
        /// <summary>
        /// The file or directory is not configured to use integrity.
        /// </summary>
        CHECKSUM_TYPE_NONE = 0,
        
        /// <summary>
        /// The file or directory is configured to use a CRC64 checksum to provide integrity.
        /// </summary>
        CHECKSUM_TYPE_CRC64 = 0x0002,
    }

    public enum FSCTL_GET_INTEGRITY_INFO_OUTPUT_RESERVED : ushort
    {
        V1 = 0
    }

    [Flags]
    public enum FSCTL_GET_INTEGRITY_INFO_OUTPUT_FLAGS : uint
    {
        /// <summary>
        /// Indicates that checksum enforcement is not currently enabled on the target file
        /// </summary>
        FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF = 0x00000001
    }

    /// <summary>
    /// The FSCTL_SET_INTEGRITY_INFORMATION Request message requests that the server 
    /// set the integrity state of the file or directory associated with the handle on which this FSCTL was invoked
    /// </summary>
    public struct FSCTL_SET_INTEGRIY_INFO_INPUT
    {
        public FSCTL_SET_INTEGRITY_INFO_INPUT_CHECKSUMALGORITHM ChecksumAlgorithm;
        public FSCTL_SET_INTEGRITY_INFO_INPUT_RESERVED Reserved;
        public FSCTL_SET_INTEGRITY_INFO_INPUT_FLAGS Flags;
    }

    public enum FSCTL_SET_INTEGRITY_INFO_INPUT_CHECKSUMALGORITHM : ushort
    {
        /// <summary>
        /// The file or directory should be set to not use integrity
        /// </summary>
        CHECKSUM_TYPE_NONE = 0,
        
        /// <summary>
        /// The file or directory should be set to provide integrity using a CRC64 checksum. 
        /// </summary>
        CHECKSUM_TYPE_CRC64 = 0x0002,
        
        /// <summary>
        /// The integrity status of the file or directory should be unchanged.
        /// </summary>
        CHECKSUM_TYPE_UNCHANGED = 0xFFFF,
    }

    public enum FSCTL_SET_INTEGRITY_INFO_INPUT_RESERVED : ushort
    {
        V1 = 0
    }

    [Flags]
    public enum FSCTL_SET_INTEGRITY_INFO_INPUT_FLAGS : uint
    {
        /// <summary>
        /// When set, if a checksum does not match, the associated I/O operation will not be failed.
        /// </summary>
        FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF = 0x00000001
    }

    public struct FSCTL_OFFLOAD_READ_INPUT
    {
        public uint Size;
        public FSCTL_OFFLOAD_READ_INPUT_FLAGS Flags;
        public uint TokenTimeToLive;
        public uint Reserved;
        public ulong FileOffset;
        public ulong CopyLength;
    }

    [Flags]
    public enum FSCTL_OFFLOAD_READ_INPUT_FLAGS : uint
    {
        NONE = 0,
    }

    public struct FSCTL_OFFLOAD_READ_OUTPUT
    {
        public uint Size;
        public FSCTL_OFFLOAD_READ_INPUT_FLAGS Flag;
        public ulong TransferLength;
        public STORAGE_OFFLOAD_TOKEN Token;
    }

    [Flags]
    public enum FSCTL_OFFLOAD_READ_OUTPUT_FLAGS : uint
    {
        NONE = 0,
        OFFLOAD_READ_FLAG_ALL_ZERO_BEYOND_CURRENT_RANGE = 0x00000001,
    }

    public struct STORAGE_OFFLOAD_TOKEN
    {
        [ByteOrder(EndianType.BigEndian)]
        public FSCTL_OFFLOAD_WRITE_INPUT_TOKEN_TYPE TokenType;
        [ByteOrder(EndianType.BigEndian)]
        public ushort Reserved;
        [ByteOrder(EndianType.BigEndian)]
        public ushort TokenIdLength;
        [StaticSize(504)]
        public byte[] TokenId;
    }

    public enum FSCTL_OFFLOAD_WRITE_INPUT_TOKEN_TYPE : uint
    {
        STORAGE_OFFLOAD_TOKEN_TYPE_ZERO_DATA = 0xFFFF0001,
    }

    public struct FSCTL_OFFLOAD_WRITE_INPUT
    {
        public uint Size;
        public FSCTL_OFFLOAD_WRITE_INPUT_FLAGS Flags;
        public ulong FileOffset;
        public ulong CopyLength;
        public ulong TransferOffset;
        public STORAGE_OFFLOAD_TOKEN Token;
    }

    [Flags]
    public enum FSCTL_OFFLOAD_WRITE_INPUT_FLAGS : uint
    {
        NONE = 0,
    }

    public struct FSCTL_OFFLOAD_WRITE_OUTPUT
    {
        public uint Size;
        public FSCTL_OFFLOAD_WRITE_OUTPUT_FLAGS Flags;
        public ulong LengthWritten;
    }

    [Flags]
    public enum FSCTL_OFFLOAD_WRITE_OUTPUT_FLAGS : uint
    {
        NONE = 0,
    }
	
	public struct FSCTL_DUPLICATE_EXTENTS_TO_FILE_Request
    {
        public FILEID SourceFileId;
        public long SourceFileOffset;
        public long TargetFileOffset;
        public long ByteCount;
    }

    public struct FSCTL_DUPLICATE_EXTENTS_TO_FILE_Response
    {

    }

    /// <summary>
    /// A 32-bit unsigned integer that contains zero or more of the following flag values. 
    /// Flag values not specified SHOULD be set to 0 and MUST be ignored.
    /// </summary>
    [Flags]
    public enum FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request_Flags_Values : UInt32
    {
        /// <summary>
        /// None.
        /// </summary>
        NONE = 0x00000000,

        /// <summary>
        /// Indicates that duplication is atomic from source point of view.
        /// </summary>
        DUPLICATE_EXTENTS_DATA_EX_SOURCE_ATOMIC = 0x00000001
    }

    /// <summary>
    /// FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX Request
    /// </summary>
    public struct FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request
    {
        /// <summary>
        /// A 64-bit unsigned integer value that specifies the size of the structure, in bytes. 
        /// This field MUST be set to 0x30.
        /// </summary>
        public ulong StructureSize;

        /// <summary>
        /// An SMB2_FILEID structure, that is an identifier of the open to the source file.
        /// </summary>
        public FILEID SourceFileId;

        /// <summary>
        /// A 64-bit signed integer that contains the file offset, in bytes, 
        /// of the start of a range of bytes in a source file from which the data is to be copied. 
        /// The value of this field MUST be greater than or equal to 0x0000000000000000 and MUST be aligned to a logical cluster boundary.
        /// </summary>
        public long SourceFileOffset;

        /// <summary>
        /// A 64-bit signed integer that contains the file offset, in bytes, 
        /// of the start of a range of bytes in a target file to which the data is to be copied. 
        /// The value of this field MUST be greater than or equal to 0x0000000000000000 and MUST be aligned to a logical cluster boundary.
        /// </summary>
        public long TargetFileOffset;

        /// <summary>
        /// A 64-bit signed integer that contains the number of bytes to copy from source to target. 
        /// The value of this field MUST be greater than or equal to 0x0000000000000000 and MUST be aligned to a logical cluster boundary.
        /// </summary>
        public long ByteCount;

        /// <summary>
        /// A 32-bit unsigned integer that contains zero or more of the following flag values. 
        /// Flag values not specified SHOULD be set to 0 and MUST be ignored.
        /// </summary>
        public FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request_Flags_Values Flags;

        /// <summary>
        /// This field SHOULD be set to zero and MUST be ignored.
        /// </summary>
        public UInt32 Reserved;
    }

    /// <summary>
    /// FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX Reply
    /// </summary>
    public struct FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Response
    {

    }

    
    #region REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER    

    /// <summary>
    /// A value representing the operations for REFS_STREAM_SNAPSHOT_MANAGEMENT request. The value 
    /// MUST be one of the following:
    /// </summary>
    [Flags()]
    public enum RefsStreamSnapshotOperation_Values : uint
    {
        /// <summary>
        /// All requests with this operational code MUST 
        /// be failed by the server.
        /// </summary>
        REFS_STREAM_SNAPSHOT_OPERATION_INVALID = 0x00000000,

        /// <summary>
        /// This request message requests the server 
        /// create a new snapshot of the UNICODE name 
        /// contained within NameAndInputBuffer, 
        /// saving a point-in-time view of the data 
        /// stream represented by the handle the 
        /// request is being sent on.
        /// </summary>
        REFS_STREAM_SNAPSHOT_OPERATION_CREATE = 0x00000001,

        /// <summary>
        /// This request message requests the server 
        /// return a list of all snapshots of the set 
        /// containing the data stream represented by 
        /// the handle the request is being sent on, and 
        /// matching a given regular expression query 
        /// string contained in NameAndInputBuffer.
        /// </summary>
        REFS_STREAM_SNAPSHOT_OPERATION_LIST = 0x00000002,

        /// <summary>
        /// This request message requests the server 
        /// return a list of all metadata extents that have 
        /// incurred modifying operations between the 
        /// data stream represented by the handle the 
        /// request is being sent on, and the data 
        /// stream represented by the UNICODE name 
        /// contained in NameAndInputBuffer. The data 
        /// stream represented by the handle must be of 
        /// a newer creation time than the data stream 
        /// represented by the UNICODE name.
        /// </summary>
        REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS = 0x00000003,

        /// <summary>
        /// This request message requests the server 
        /// revert the data stream represented by the 
        /// handle the request is being sent on to a 
        /// point-in-time snapshot view represented by 
        /// the UNICODE name contained within 
        /// NameAndInputBuffer.
        /// </summary>
        REFS_STREAM_SNAPSHOT_OPERATION_REVERT = 0x00000004,

        /// <summary>
        /// This request message requests the server 
        /// create a shadow data stream on the data 
        /// stream represented by the handle the 
        /// request is being sent on.
        /// </summary>
        REFS_STREAM_SNAPSHOT_OPERATION_SET_SHADOW_BTREE = 0x00000005,

        /// <summary>
        /// This request message requests the server 
        /// remove a shadow data stream on the data 
        /// stream represented by the handle the 
        /// request is being sent on.
        /// </summary>
        REFS_STREAM_SNAPSHOT_OPERATION_CLEAR_SHADOW_BTREE = 0x00000006,

        /// <summary>
        /// The maximum operational code supported by
        /// the server. All operational codes larger than 
        /// this numerical value will be failed.
        /// </summary>
        REFS_STREAM_SNAPSHOT_OPERATION_MAX = 0x00000006,
    }
    
    /// <summary>
    /// The FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request message requests that the server 
    /// perform a specific stream snapshot operation on a given data stream contained in a file. The operation 
    /// performed is dependent on the value defined in REFS_STREAM_SNAPSHOT_OPERATION. The request 
    /// message takes the form of a REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER structure.
    /// The REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER is as follows.
    /// </summary>
    public partial struct REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER
    {
        /// <summary>
        /// This field specifies the operation and MUST contain one of the following values in RefsStreamSnapshotOperation_Values
        /// </summary>
        public RefsStreamSnapshotOperation_Values Operation;

        /// <summary>
        /// An unsigned integer representing the length in bytes of the 
        /// unicode name contained within NameAndInputBuffer field. If no such name is present in the 
        /// message, then this value is set to zero.
        /// </summary>
        [StaticSize(2)]
        public ushort SnapshotNameLength;

        /// <summary>
        /// An unsigned integer representing the length in bytes of 
        /// the operational control structure present in the message and contained within 
        /// NameAndInputBuffer field. If no such control structure is present in the message, then this 
        /// value is set to zero.
        /// </summary>
        [StaticSize(2)]
        public ushort OperationInputBufferLength;

        /// <summary>
        /// This field MUST be set to zero and MUST be ignored.
        /// </summary>
        [StaticSize(16)]
        public byte[] Reserved;

        /// <summary>
        /// An array of bytes optionally containing a unicode name as well as 
        /// an operational control buffer. When a unicode name is present, it is located immediately within the 
        /// first byte of NameAndInputBuffer. When an operational control buffer is present, it is located at 
        /// the next quad aligned boundary past the end of the unicode name. If no such unicode name is 
        /// present, then the operational control buffer is located at the first byte of NameAndInputBuffer.
        /// </summary>
        [Size("SnapshotNameLength + OperationInputBufferLength")]
        public byte[] NameAndInputBuffer;
    }

    #endregion
    
    #region REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER

    /// <summary>
    /// The REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER is as follows:
    /// </summary>
    public partial struct REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER
    {
        /// <summary>
        /// A signed integer representing the starting VCN for which to perform the 
        /// request on.
        /// </summary>
        public long StartingVcn;

        /// <summary>
        /// An unsigned integer representing flags to modify the behavior of the request. This 
        /// field must be set to zero.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// This field MUST be set to zero and MUST be ignored.
        /// </summary>
        public uint Reserved;
    }

    #endregion

    #region REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER
    /// FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT Reply returns the result of the FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request.

    /// <summary>
    /// The REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER is as follows:
    /// </summary>
    public partial struct REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER
    {
        /// <summary>
        /// An unsigned integer representing the number of entries contained within the 
        /// Entries field.
        /// </summary>
        public int EntryCount;

        /// <summary>
        /// An unsigned integer representing the total number of 
        /// bytes to fully satisfy the request. This value is accurate upon returning STATUS_SUCCESS as well 
        /// as STATUS_BUFFER_OVERFLOW
        /// </summary>
        public uint BufferSizeRequiredForQuery;

        /// <summary>
        /// This field MUST be set to zero and MUST be ignored.
        /// </summary>
        public uint Reserved;

        /// <summary>
        ///  An array of REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER_ENTRY structs.
        /// </summary>
        [Size("EntryCount")]
        public REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER_ENTRY[] Entries;
    }

    /// <summary>
    /// The REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER_ENTRY is as follows:
    /// </summary>
    public partial struct REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER_ENTRY
    {
        /// <summary>
        /// An unsigned integer representing the offset in bytes to the next 
        /// REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER_ENTRY structure. When this value is zero 
        /// there are no more entries in the array.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        /// A unsigned integer representing the length of the UNICODE name 
        /// contained in SnapshotName in bytes.
        /// </summary>
        public ushort SnapshotNameLength;

        /// <summary>
        /// An unsigned integer representing a FILETIME structure 
        /// containing the creation time of the snapshot.
        /// </summary>
        public ulong SnapshotCreationTime;

        /// <summary>
        /// An unsigned integer representing the End-Of-File marker of the data stream 
        /// represented by this entry.
        /// </summary>
        public ulong StreamSize;

        /// <summary>
        /// An unsigned integer representing the size in bytes used by the 
        /// data owned by the data stream represented by this entry. 
        /// </summary>
        public ulong StreamAllocationSize;

        /// <summary>
        /// This field MUST be set to zero and MUST be ignored.
        /// </summary>
        public ulong Reserved;

        /// <summary>
        /// An array of WCHARs, as specified in [MS-DTYP] section 2.2.60, 
        /// representing the UNICODE name for the snapshot representing this entry. The size of the array is 
        /// defined in the SnapshotNameLength field.
        /// </summary>
        [Size("SnapshotNameLength")]
        public byte[] SnapshotName;
    }

    #endregion

    #region REFS_STREAM_SNAPSHOT_QUERY_DELTAS_OUTPUT_BUFFER

    /// <summary>
    /// The REFS_STREAM_SNAPSHOT_QUERY_DELTAS_OUTPUT_BUFFER is as follows:
    /// </summary>
    public partial struct REFS_STREAM_SNAPSHOT_QUERY_DELTAS_OUTPUT_BUFFER
    {
        /// <summary>
        /// An unsigned integer representing the number of REFS_STREAM_EXTENT 
        /// structs contained in the Extents field.
        /// </summary>
        public uint ExtentCount;

        /// <summary>
        /// This field MUST be set to zero and MUST be ignored.
        /// </summary>
        public ulong Reserved;

        /// <summary>
        /// An array of REFS_STREAM_EXTENT structs.
        /// </summary>
        public REFS_STREAM_EXTENT Extents;
    }

    /// <summary>
    /// The REFS_STREAM_EXTENT is as follows:
    /// </summary>
    public partial struct REFS_STREAM_EXTENT
    {
        /// <summary>
        /// A signed integer representing a VCN within a data stream. This value will always be 
        /// greater than zero
        /// </summary>
        public ulong Vcn;

        /// <summary>
        /// A signed integer representing the LCN mapping to Vcn in a data stream. This value 
        /// will always be greater than zero.
        /// </summary>
        public ulong Lcn;

        /// <summary>
        /// A signed integer representing the contiguous length in clusters for which the VCN 
        /// to LCN mapping holds. This value will always be greater than zero.
        /// </summary>
        public ulong Length;

        /// <summary>
        /// A value representing the properties for this VCN to LCN mapping. The value 
        /// MUST be one of the following:
        /// </summary>
        public RefsStreamExtentProperties_Values Properties;
    }  

    /// <summary>
    /// A value representing the properties for this VCN to LCN mapping. The value 
    /// MUST be one of the following:
    /// </summary>
    [Flags()]
    public enum RefsStreamExtentProperties_Values : uint
    {
        /// <summary>
        /// The metadata extent is considered valid, where the 
        /// VCN to LCN mapping represents a written or zeroed 
        /// extent.
        /// </summary>
        REFS_STREAM_EXTENT_PROPERTY_VALID = 0x0010,

        /// <summary>
        /// The metadata extent does not map to an LCN, but 
        /// instead contains a token representation an allocation 
        /// reservation.
        /// </summary>
        REFS_STREAM_EXTENT_PROPERTY_STREAM_RESERVED = 0x0020,

        /// <summary>
        /// The metadata extent references data that is 
        /// checksumed with the CRC32 algorithm.
        /// </summary>
        REFS_STREAM_EXTENT_PROPERTY_CRC32 = 0x0080,

        /// <summary>
        /// The metadata extent references data that is 
        /// checksumed with the CRC64 algorithm.
        /// </summary>
        REFS_STREAM_EXTENT_PROPERTY_CRC64 = 0x0100,

        /// <summary>
        /// The metadata extent contains a ghosted recall buffer.
        /// </summary>
        REFS_STREAM_EXTENT_PROPERTY_GHOSTED = 0x0200,

        /// <summary>
        /// The metadata extent is a cached copy of a different 
        /// metadata extent. This extent is immutable, and the 
        /// LCN it references is not writable via this extent.
        /// </summary>
        REFS_STREAM_EXTENT_PROPERTY_READONLY = 0x0400,

        /// <summary>
        /// The metadata extent represents a sparse range within 
        /// the stream. The range represented by this extent is 
        /// analogous to a sparse hole in the stream table.
        /// </summary>
        REFS_STREAM_EXTENT_PROPERTY_SPARSE = 0x0008,
    }
    
    #endregion  

}
