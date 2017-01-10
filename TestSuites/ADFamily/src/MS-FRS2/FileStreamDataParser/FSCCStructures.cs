// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;
using BKUPParser;

namespace FileStreamDataParser
{
    #region structs

    /// <summary>
    /// Represents the Replicated File Structure
    /// </summary>
    public struct ReplicatedFileStructure
    {
        /// <summary>
        /// Signature of the data buffer.
        /// </summary>
        public Signature signature;

        /// <summary>
        /// Dictionary of File Streams which compose the file data
        /// </summary>
        public Dictionary<int, StreamData> streamdata;
    }

    /// <summary>
    /// Signature Structure.
    /// </summary>
    public struct Signature
    {
        /// <summary>
        /// Byte array corresponding to block signature.
        /// </summary>
        public byte[] BlockSignature;

        /// <summary>
        /// Compressed Size of data in the data buffer.
        /// </summary>
        public UInt32 BlockCompressedSize;

        /// <summary>
        /// Uncompressed Size of data in the data buffer.
        /// </summary>
        public UInt32 BlockUncompressedSize;
    }

    /// <summary>
    /// Stream Data Structure
    /// </summary>
    public struct StreamData
    {
        /// <summary>
        /// Stream Header
        /// </summary>
        public MarshalBlockHeader Header;
                
        /// <summary>
        /// Metadata Stream Structure.
        /// </summary>
        public MetaDataStream MetaData;

        /// <summary>
        /// Security Stream Structure.
        /// </summary>
        public SecurityDataStream SecurityData;

        /// <summary>
        /// Compression Stream Structure.
        /// </summary>
        public CompressionDataStream CompressionData;
        
        /// <summary>
        /// Reparse Stream Structure.
        /// </summary>
        public ReparseDataStream ReparseData;

        /// <summary>
        /// Flat Stream Structure.
        /// </summary>
        public FlatDataStream FlatData;
    }
    
    /// <summary>
    /// Marshal Block Header Structure.
    /// </summary>
    public struct MarshalBlockHeader
    {
        /// <summary>
        /// Stream Type.
        /// </summary>
        public UInt32 streamType;

        /// <summary>
        /// Size of stream block.
        /// </summary>
        public UInt32 blockSize;

        /// <summary>
        /// Flags indicate whether EndOfStream reached or not.
        /// </summary>
        public UInt32 flags;
    }

    /// <summary>
    /// Meta Data Stream Structure.
    /// </summary>
    public struct MetaDataStream
    {
        /// <summary>
        /// Version, must be 3.
        /// </summary>
        public UInt32 Version;

        /// <summary>
        /// Reserved1 Field.
        /// </summary>
        public byte[] Reserved1;

        /// <summary>
        /// File Basic Information Structure
        /// </summary>
        public FileBasicInformation FileBasicInfo;

        /// <summary>
        /// Security Descriptor Control value
        /// </summary>
        public UInt16 sdControl;

        /// <summary>
        /// Reserved2 Field.
        /// </summary>
        public byte[] Reserved2;

        /// <summary>
        /// primaryDataStreamSize value
        /// </summary>
        public UInt64 primaryDataStreamSize;

        /// <summary>
        /// Reserved3 Field.
        /// </summary>
        public byte[] Reserved3;        
    }

    /// <summary>
    /// Security Data Stream Structure
    /// </summary>
    public struct SecurityDataStream
    {
        /// <summary>
        /// byte array corresponding to security data
        /// </summary>
        public byte[] securityData;
    }

    /// <summary>
    /// Compression Data Stream Structure
    /// </summary>
    public struct CompressionDataStream
    {
        /// <summary>
        /// Compression Format
        /// </summary>
        public CompressionFormat_Values CompressionFormat;
    }    

    /// <summary>
    /// Reparse Data Stream Structure.
    /// </summary>
    public struct ReparseDataStream
    {
        /// <summary>
        /// Reparse Tag Value
        /// </summary>
        public UInt32 ReparseTag;

        /// <summary>
        /// Reparse GUID Data Buffer Structure.
        /// </summary>
        public REPARSE_GUID_DATA_BUFFER GUID_DATA_BUFFER;

        /// <summary>
        /// Reparse Data Buffer Structure.
        /// </summary>
        public REPARSE_DATA_BUFFER DATA_BUFFER;
    }

    /// <summary>
    /// Reparse GUID Data Buffer Structure.
    /// </summary>
    public struct REPARSE_GUID_DATA_BUFFER
    {
        /// <summary>
        /// Length of Reparse Data 
        /// </summary>
        public UInt16 ReparseDataLength;
        
        /// <summary>
        /// Reserved Field
        /// </summary>
        public byte[] Reserved;

        /// <summary>
        /// Reparse Guid
        /// </summary>
        public Guid ReparseGuid;

        /// <summary>
        /// Byte array corresponding to Reparse Data Buffer.
        /// </summary>
        public byte[] ReparseDataBuffer;
    }

    public struct REPARSE_DATA_BUFFER
    {
        /// <summary>
        /// Length of Reparse Data 
        /// </summary>
        public UInt16 ReparseDataLength;

        /// <summary>
        /// Reserved Field
        /// </summary>
        public byte[] Reserved;

        /// <summary>
        /// SymbolicLinkReparseDataBuffer structure.
        /// </summary>
        public SymbolicLinkReparseDataBuffer symbolicLinkData;

        /// <summary>
        /// MountPointReparseDataBuffer structure
        /// </summary>
        public MountPointReparseDataBuffer mountPointData;
    }

    /// <summary>
    /// Symbolic Link Reparse Data Buffer Structure
    /// </summary>
    public struct SymbolicLinkReparseDataBuffer
    {
        /// <summary>
        /// This field gives the offset to SubstituteName
        /// field in PathBuffer.
        /// </summary>
        public UInt16 SubstituteNameOffset;

        /// <summary>
        /// Represents the length of SubstituteName
        /// </summary>
        public UInt16 SubstituteNameLength;

        /// <summary>
        /// Represents the Substitute Name
        /// </summary>
        public string SubstituteName;

        /// <summary>
        /// This field gives the offset to PrintName
        /// field in PathBuffer.
        /// </summary>
        public UInt16 PrintNameOffset;

        /// <summary>
        /// Represents the length of Print Name
        /// </summary>
        public UInt16 PrintNameLength;

        /// <summary>
        /// Represents the Print Name
        /// </summary>
        public string PrintName;

        /// <summary>
        /// Flags represent whether the path
        /// is absolute or relative.
        /// </summary>
        public UInt32 Flags;

        /// <summary>
        /// Byte Array which consists of Substitute
        /// Name and Print Name
        /// </summary>
        public byte[] PathBuffer;
    }

    /// <summary>
    /// Mount Point Reparse Data Buffer Structure
    /// </summary>
    public struct MountPointReparseDataBuffer
    {
        /// <summary>
        /// This field gives the offset to SubstituteName
        /// field in PathBuffer.
        /// </summary>
        public UInt16 SubstituteNameOffset;

        /// <summary>
        /// Represents the length of SubstituteName
        /// </summary>
        public UInt16 SubstituteNameLength;

        /// <summary>
        /// Represents the Substitute Name
        /// </summary>
        public string SubstituteName;

        /// <summary>
        /// This field gives the offset to PrintName
        /// field in PathBuffer.
        /// </summary>
        public UInt16 PrintNameOffset;

        /// <summary>
        /// Represents the length of Print Name
        /// </summary>
        public UInt16 PrintNameLength;

        /// <summary>
        /// Represents the Print Name
        /// </summary>
        public string PrintName;

        /// <summary>
        /// Byte Array which consists of Substitute
        /// Name and Print Name
        /// </summary>
        public byte[] PathBuffer;
    }

    /// <summary>
    ///  The __FILETIME structure is a 64-bit value that represents
    ///  the number of 100-nanosecond intervals that have elapsed
    ///  since January 1, 1601, in Coordinated Universal Time
    ///  (UTC) format.
    /// </summary>
    //  <remarks>
    //   file:///E:/fscc xmls/_rfc_ms-fscc2_2_1_1.xml
    //  </remarks>
    public partial struct FILETIME
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
    }

    /// <summary>
    ///  This information class is used to query or set file
    ///  information.The FILE_BASIC_INFORMATION data element
    ///  is as follows.
    /// </summary>
    //  <remarks>
    //   file:///E:/fscc xmls/_rfc_ms-fscc2_2_4_5.xml
    //  </remarks>
    public partial struct FileBasicInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the time when
        ///  the file was created. All dates and times in this message
        ///  are in absolute system-time format, which is represented
        ///  as a FILETIME structure. A valid time for this field
        ///  is an integer greater than 0.   When setting file attributes,
        ///  a value of 0 indicates to the server that it MUST NOT
        ///  change this attribute.    When setting file attributes,
        ///  a value of -1 indicates to the server that it MUST
        ///  NOT change this attribute for all subsequent operations
        ///  on the same file handle. This field MUST NOT be set
        ///  to a value less than -1.
        /// </summary>
        [PossibleValueRange("-1", "")]
        public FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  the file was accessed in the format of a FILETIME structure.
        ///  A valid time for this field is an integer greater than
        ///  0.   When setting file attributes, a value of 0 indicates
        ///  to the server that it MUST NOT change this attribute.
        ///     When setting file attributes, a value of -1 indicates
        ///  to the server that it MUST NOT change this attribute
        ///  for all subsequent operations on the same file handle.
        ///  This field MUST NOT be set to a value less than -1.
        ///  The file system updates the values of the LastAccessTime,
        ///  LastWriteTime, and ChangeTime members as appropriate
        ///  after an I/O operation is performed on a file. However,
        ///  a driver or application can request that the file system
        ///  not update one or more of these members for I/O operations
        ///  that are performed on the caller's file handle by setting
        ///  the appropriate members to -1. The caller can set one,
        ///  all, or any other combination of these three members
        ///  to -1. Only the members that are set to -1 will be
        ///  unaffected by I/O operations on the file handle; the
        ///  other members will be updated as appropriate. This
        ///  behavior is consistent across all file system types.
        ///   Note that even though -1 can be used with the CreationTime
        ///  field, it has no effect since file creation time is
        ///  never updated in response to file system calls such
        ///  as read and write.
        /// </summary>
        [PossibleValueRange("-1", "")]
        public FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  information was written to the file in the format of
        ///  a FILETIME structure. A valid time for this field is
        ///  an integer greater than 0.   When setting file attributes,
        ///  a value of 0 indicates to the server that it MUST NOT
        ///  change this attribute.    When setting file attributes,    
        ///  a value of -1 indicates to the server that it MUST
        ///  NOT change this attribute for all subsequent operations
        ///  on the same file handle. This field MUST NOT be set
        ///  to a value less than -1. The file system updates the
        ///  values of the LastAccessTime, LastWriteTime, and ChangeTime
        ///  members as appropriate after an I/O operation is performed
        ///  on a file. However, a driver or application can request
        ///  that the file system not update one or more of these
        ///  members for I/O operations that are performed on the
        ///  caller's file handle by setting the appropriate members
        ///  to -1. The caller can set one, all, or any other combination
        ///  of these three members to -1. Only the members that
        ///  are set to -1 will be unaffected by I/O operations
        ///  on the file handle; the other members will be updated
        ///  as appropriate. This behavior is consistent across
        ///  all file system types. Note that even though -1 can
        ///  be used with the CreationTime field, it has no effect
        ///  since file creation time is never updated in response
        ///  to file system calls such as read and write.
        /// </summary>
        [PossibleValueRange("-1", "")]
        public FILETIME LastWriteTime;

        /// <summary>
        ///   A 64-bit signed integer that contains the last time
        ///  the file was changed in the format of a FILETIME structure.
        ///  A valid time for this field is an integer greater than
        ///  0.   When setting file attributes, a value of 0 indicates
        ///  to the server that it MUST NOT change this attribute.
        ///     When setting file attributes, a value of -1 indicates
        ///  to the server that it MUST NOT change this attribute
        ///  for all subsequent operations on the same file handle.
        ///  This field MUST NOT be set to a value less than -1.
        ///  The file system updates the values of the LastAccessTime,
        ///  LastWriteTime, and ChangeTime members as appropriate
        ///  after an I/O operation is performed on a file. However,
        ///  a driver or application can request that the file system
        ///  not update one or more of these members for I/O operations
        ///  that are performed on the caller's file handle by setting
        ///  the appropriate members to -1. The caller can set one,
        ///  all, or any other combination of these three members
        ///  to -1. Only the members that are set to -1 will be
        ///  unaffected by I/O operations on the file handle; the
        ///  other members will be updated as appropriate. This
        ///  behavior is consistent across all file system types.
        ///   Note that even though -1 can be used with the CreationTime
        ///  field, it has no effect since file creation time is
        ///  never updated in response to file system calls such
        ///  as read and write.
        /// </summary>
        [PossibleValueRange("-1", "")]
        public FILETIME ChangeTime;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file attributes.
        ///  Valid file attributes are specified in section.
        /// </summary>
        public uint FileAttribute;

        /// <summary>
        ///  A 32-bit field. This field is reserved. This field
        ///  can be set to any value, and MUST be ignored.
        /// </summary>
        public uint Reserved;
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
    //  <remarks>
    //   file:///E:/fscc xmls/_rfc_ms-fscc2_2_4_21.xml
    //  </remarks>
    public partial struct FileNetworkOpenInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the time
        ///  when the file was created in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [PossibleValueRange("0", "")]
        public FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was accessed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [PossibleValueRange("0", "")]
        public FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time information was written to the file in the format
        ///  of a FILETIME structure. The value of this field MUST
        ///  be greater than or equal to 0.
        /// </summary>
        [PossibleValueRange("0", "")]
        public FILETIME LastWriteTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was changed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [PossibleValueRange("0", "")]
        public FILETIME ChangeTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  allocation size in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        [PossibleValueRange("0", "9223372036854775807")]
        public long AllocationSize;

        /// <summary>
        ///  A 64-bit signed integer that contains the absolute
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
        [PossibleValueRange("0", "9223372036854775807")]
        public long EndOfFile;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file
        ///  attributes. Valid attributes are as specified in section.
        /// </summary>
        public uint FileAttribute;

        /// <summary>
        ///  A 32-bit field. This field is reserved. This field
        ///  can be set to any value, and MUST be ignored.
        /// </summary>
        public uint Reserved;
    }

    /// <summary>
    ///  The first type of data that may be returned.
    /// </summary>
    //  <remarks>
    //   file:///E:/fscc xmls/_rfc_ms-fscc2_2_4_22_1.xml
    //  </remarks>
    public partial struct FileObjectIdInformation_Type_1
    {

        /// <summary>
        ///  A 64-bit unsigned integer that contains the file
        ///  reference number for the file. NTFS generates this
        ///  number and assigns it to the file automatically when
        ///  the file is created. The file reference number is unique
        ///  within the volume on which the file exists.
        /// </summary>
        public ulong FileReferenceNumber;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the file
        ///  or directory within the volume on which it resides.
        ///  Specifically, the same object ID can be assigned to
        ///  another file or directory on a different volume, but
        ///  it MUST NOT be assigned to another file or directory
        ///  on the same volume.
        /// </summary>
        public Guid ObjectId;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the volume
        ///  on which the object resided when the object identifier
        ///  was created, or zero if the volume had no object identifier
        ///  at that time. After copy operations, move operations,
        ///  or other file operations, this may not be the same
        ///  as the object identifier of the volume on which the
        ///  object presently resides.
        /// </summary>
        public Guid BirthVolumeId;

        /// <summary>
        ///  A 16-byte GUID value containing the object identifier
        ///  of the object at the time it was created. After copy
        ///  operations, move operations, or other file operations,
        ///  this value may not be the same as the ObjectId member
        ///  at present. When a file is moved or copied from one
        ///  volume to another, the ObjectId member's value changes
        ///  to a random unique value to avoid the potential for
        ///  ObjectId collisions because the object ID is not guaranteed
        ///  to be unique across volumes.
        /// </summary>
        public Guid BirthObjectId;

        /// <summary>
        ///  A 16-byte GUID value containing the domain identifier.
        ///  This value is unused; it SHOULD be zero, and MUST be
        ///  ignored.
        /// </summary>
        public Guid DomainId;
    }

    /// <summary>
    ///  The second type of data that may be returned.
    /// </summary>
    //  <remarks>
    //   file:///E:/fscc xmls/_rfc_ms-fscc2_2_4_22_2.xml
    //  </remarks>
    public partial struct FileObjectIdInformation_Type_2
    {

        /// <summary>
        ///  A 64-bit unsigned integer that contains the file
        ///  reference number for the file. NTFS generates this
        ///  number and assigns it to the file automatically when
        ///  the file is created. The file reference number is unique
        ///  within the volume on which the file exists.
        /// </summary>
        public ulong FileReferenceNumber;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the file
        ///  or directory within the volume on which it resides.
        ///  Specifically, the same object ID can be assigned to
        ///  another file or directory on a different volume, but
        ///  it MUST NOT be assigned to another file or directory
        ///  on the same volume.
        /// </summary>
        public Guid ObjectId;

        /// <summary>
        ///  A 48-byte BLOB that contains application-specific
        ///  extended information on the file object. If no extended
        ///  information has been written for this file, the server
        ///  MUST return 48 bytes of 0x00 in this field.
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    #endregion

    #region enums

    /// <summary>
    /// This enum holds stream type values
    /// </summary>
    public enum StreamType_Values : uint
    {
        /// <summary>
        /// Enum value of MS_TYPE_META_DATA
        /// </summary>
        MS_TYPE_META_DATA = 0x00000001,

        /// <summary>
        /// Enum value of  MS_TYPE_COMPRESSION_DATA
        /// </summary>
        MS_TYPE_COMPRESSION_DATA = 0x00000002,

        /// <summary>
        /// Enum value of  MS_TYPE_REPARSE_DATA
        /// </summary>
        MS_TYPE_REPARSE_DATA = 0x00000003,

        /// <summary>
        /// Enum value of MS_TYPE_FLAT_DATA
        /// </summary>
        MS_TYPE_FLAT_DATA = 0x00000004,

        /// <summary>
        /// Enum value of MS_TYPE_SECURITY_DATA
        /// </summary>
        MS_TYPE_SECURITY_DATA = 0x00000006,

        /// <summary>
        /// Enum value of  MS_TYPE_SPARSE_DATA
        /// </summary>
        MS_TYPE_SPARSE_DATA = 0x00000007,
    }

    public enum FileAttribute : uint
    {
        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_ARCHIVE
        /// </summary>
        FILE_ATTRIBUTE_ARCHIVE = 0x00000020,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_COMPRESSED
        /// </summary>
        FILE_ATTRIBUTE_COMPRESSED = 0x00000800,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_DIRECTORY
        /// </summary>
        FILE_ATTRIBUTE_DIRECTORY = 0x00000010,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_ENCRYPTED
        /// </summary>
        FILE_ATTRIBUTE_ENCRYPTED = 0x00004000,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_HIDDEN
        /// </summary>
        FILE_ATTRIBUTE_HIDDEN = 0x00000002,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_NORMAL
        /// </summary>
        FILE_ATTRIBUTE_NORMAL = 0x00000080,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_NOT_CONTENT_INDEXED
        /// </summary>
        FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_OFFLINE
        /// </summary>
        FILE_ATTRIBUTE_OFFLINE = 0x00001000,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_READONLY
        /// </summary>
        FILE_ATTRIBUTE_READONLY = 0x00000001,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_REPARSE_POINT
        /// </summary>
        FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_SPARSE_FILE
        /// </summary>
        FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_SYSTEM
        /// </summary>
        FILE_ATTRIBUTE_SYSTEM = 0x00000004,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_TEMPORARY
        /// </summary>
        FILE_ATTRIBUTE_TEMPORARY = 0x00000100,

        /// <summary>
        /// Enum value of FILE_ATTRIBUTE_INVALID
        /// </summary>
        FILE_ATTRIBUTE_INVALID = 0x00000000
    }

    [Flags()]
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

        /// <summary>
        ///  Reserved for future use.
        /// </summary>
        All_other_values,
    }

    /// <summary>
    /// Reparse Tag Values Enum
    /// </summary>
    public enum ReparseTag_Values : uint
    {
        /// <summary>
        /// Reparse Tag corresponding to SYMLINK
        /// </summary>
        IO_REPARSE_TAG_SYMLINK = 0xA000000C,

        /// <summary>
        /// Reparse Tag corresponding to MOUNT POINT
        /// </summary>
        IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003
    }

    /// <summary>
    /// Symlink Data Flags
    /// </summary>
    public enum REPAESE_DATA_BUFFER_SYMLINK_Flags : uint
    {
        /// <summary>
        /// Path is a full path.
        /// </summary>
        SYMLINK_FLAG_FULL = 0x00000000,

        /// <summary>
        /// Path is a relative path.
        /// </summary>
        SYMLINK_FLAG_RELATIVE = 0x00000001
    }

    #endregion
}
