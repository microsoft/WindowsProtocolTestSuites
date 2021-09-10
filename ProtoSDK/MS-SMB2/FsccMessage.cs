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
}
