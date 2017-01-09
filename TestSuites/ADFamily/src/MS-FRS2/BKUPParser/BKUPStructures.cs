// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;

namespace BKUPParser
{
    /// <summary>
    /// Captures the MS_TYPE_FLAT_DATA structure,
    /// which contains the backup streams specified by the BKUP TD.
    /// </summary>
    public struct FlatDataStream
    {
        /// <summary>
        /// Stream ID of the Stream, must one of the valid values
        /// specified by the dwStreamId_Values enum.
        /// </summary>
        public dwStreamId_Values dwStreamId;

        /// <summary>
        /// Stream Attributes of the Stream, must one of the valid values
        /// specified by the dwStreamAttributes_Values enum.
        /// </summary>
        public dwStreamAttributes_Values dwStreamAttributes;
        
        /// <summary>
        /// Size of the Backup Stream.
        /// </summary>
        public UInt64 Size;

        /// <summary>
        /// Size of the cStreamName field.
        /// </summary>
        public UInt32 dwStreamNameSize;

        /// <summary>
        /// Byte Array equal to the size specified by the
        /// dwStreamNameSize value, represents the stream name.
        /// </summary>
        public byte[] cStreamName;

        /// <summary>
        /// string strStreamName read from the cStreamName byte array.
        /// </summary>
        public string strStreamName;

        /// <summary>
        /// Alternate Data Backup Stream Structure.
        /// </summary>
        public FlatData_ALTERNATE_DATA AlternateData;
        
        /// <summary>
        /// Data Backup Stream Structure.
        /// </summary>
        public FlatData_DATA Data;

        /// <summary>
        /// object ID Backup Stream Structure.
        /// </summary>
        public FlatData_OBJECT_ID ObjectID;

        /// <summary>
        /// Reparse Backup Stream Structure.
        /// </summary>
        public FlatData_REPARSE_DATA ReparseData;
    }

    /// <summary>
    /// Alternate Data Backup Stream Structure.
    /// </summary>
    public struct FlatData_ALTERNATE_DATA
    {
        /// <summary>
        /// byte array which represents the data
        /// in Alternate Data Backup Stream.
        /// </summary>
        public byte[] data;

        /// <summary>
        /// Alternate Data read in string format from data field.
        /// </summary>
        public string strData;

        /// <summary>
        /// Keeps the count of number of Alternate data streams.
        /// </summary>
        public UInt32 streamCount;
    }

    /// <summary>
    /// Data Backup Stream Structure.
    /// </summary>
    public struct FlatData_DATA
    {
        /// <summary>
        /// byte array which represents the data
        /// in Data Backup Stream.
        /// </summary>
        public byte[] data;

        /// <summary>
        /// File Data read in string format from data field.
        /// </summary>
        public string strData;
    }

    /// <summary>
    /// Object ID Backup Stream Structure.
    /// </summary>
    public struct FlatData_OBJECT_ID
    {
        /// <summary>
        /// Guid corresponding to Object ID
        /// </summary>
        public Guid objectID;

        /// <summary>
        /// Guid corresponding to Birth Volume ID
        /// </summary>
        public Guid birthVolumeID;

        /// <summary>
        /// Guid corresponding to Birth Object ID
        /// </summary>
        public Guid birthObjectID;

        /// <summary>
        /// Guid corresponding to Domain ID
        /// </summary>
        public Guid domainID;
    }

    /// <summary>
    /// Reparse Data Backup Stream Structure.
    /// </summary>
    public struct FlatData_REPARSE_DATA
    {
        /// <summary>
        /// Reparse Tag Value
        /// </summary>
        public UInt32 ReparseTag;

        /// <summary>
        /// Reparse GUID Data Buffer Structure.
        /// </summary>
        public FlatData_REPARSE_GUID_DATA_BUFFER GUID_DATA_BUFFER;

        /// <summary>
        /// Reparse Data Buffer Structure.
        /// </summary>
        public FlatData_REPARSE_DATA_BUFFER DATA_BUFFER;
    }

    /// <summary>
    /// Reparse GUID Data Buffer Structure.
    /// </summary>
    public struct FlatData_REPARSE_GUID_DATA_BUFFER
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

    /// <summary>
    /// Reparse Data Buffer Structure.
    /// </summary>
    public struct FlatData_REPARSE_DATA_BUFFER
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
        public FlatData_SymbolicLinkReparseDataBuffer symbolicLinkData;

        /// <summary>
        /// MountPointReparseDataBuffer structure
        /// </summary>
        public FlatData_MountPointReparseDataBuffer mountPointData;
    }

    /// <summary>
    /// Symbolic Link Reparse Data Buffer Structure
    /// </summary>
    public struct FlatData_SymbolicLinkReparseDataBuffer
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
    public struct FlatData_MountPointReparseDataBuffer
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
    ///  A sparse file (or a sparse named stream) is represented
    ///  both by a DATA backup stream (or an ALTERNATE_DATA
    ///  backup stream in the case of a named stream) followed
    ///  by one or more SPARSE_BLOCK backup streams. A SPARSE_BLOCK
    ///  backup stream represents a portion of a sparse file
    ///  that contains data; unallocated portions of a file
    ///  are indicated by the absence of a SPARSE_BLOCK stream
    ///  that describes that part of the file. The structure
    ///  of the data portion of this backup stream is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/BKUP/xml/_rfc_ms-bkup2_2_10.xml
    //  </remarks>
    public partial struct SparseBlockStreamStructure
    {

        /// <summary>
        ///   					An unsigned, 64-bit integer that specifies the
        ///  offset within the file stream (not in the backup stream)
        ///  of the data contained in this sparse block.
        /// </summary>
        public ulong Offset;

        /// <summary>
        ///   					The data for this allocated region of the file
        ///  stream. This field MUST be of length (Size - 8), where
        ///  Size is the value of the WIN32_STREAM_ID header's Size
        ///  field that is associated with this backup stream.
        /// </summary>
        public byte Data;
    }   

    [Flags()]
    public enum dwStreamId_Values : uint
    {

        /// <summary>
        ///  Alternative data streams.
        /// </summary>
        ALTERNATE_DATA = 0x00000004,

        /// <summary>
        ///  Standard data.
        /// </summary>
        DATA = 0x00000001,

        /// <summary>
        ///  Extended attribute data.
        /// </summary>
        EA_DATA = 0x00000002,

        /// <summary>
        ///  Hard link information.
        /// </summary>
        LINK = 0x00000005,

        /// <summary>
        ///  Object identifiers.
        /// </summary>
        OBJECT_ID = 0x00000007,

        /// <summary>
        ///  Reparse points.
        /// </summary>
        REPARSE_DATA = 0x00000008,

        /// <summary>
        ///  Security descriptor data.
        /// </summary>
        SECURITY_DATA = 0x00000003,

        /// <summary>
        ///  Data in a sparse file.
        /// </summary>
        SPARSE_BLOCK = 0x00000009,

        /// <summary>
        ///  Transactional file system.
        /// </summary>
        TXFS_DATA = 0x0000000A,
    }

    [Flags()]
    public enum dwStreamAttributes_Values : uint
    {

        /// <summary>
        ///  This backup stream has no special attributes.
        /// </summary>
        STREAM_NORMAL_ATTRIBUTE = 0x00000000,

        /// <summary>
        ///  The backup stream contains security information. This
        ///  attribute applies only to backup stream of type SECURITY_DATA.
        /// </summary>
        STREAM_CONTAINS_SECURITY = 0x00000002,

        /// <summary>
        ///  The backup stream is part of a sparse file stream. 
        ///  This attribute applies only to backup stream of type
        ///  DATA, ALTERNATE_DATA, and SPARSE_BLOCK.
        /// </summary>
        STREAM_SPARSE_ATTRIBUTE = 0x00000008,

        STREAM_COMBINED_ATTRIBUTE = 0x00000002 | 0x00000008
    }

    /// <summary>
    ///  The Object ID Backup Stream contains a unique identifier
    ///  for a file. The structure of the data portion of this
    ///  backup stream is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/BKUP/xml/_rfc_ms-bkup2_2_7.xml
    //  </remarks>
    public partial struct Object_ID_Backup_Stream_Structure
    {

        /// <summary>
        ///  A 16-byte GUID, assigned by the server on which
        ///  the file is stored, that uniquely identifies the file
        ///  or directory within the volume in which it is stored.
        /// </summary>
        public System.Guid ObjectID;

        /// <summary>
        ///  This field contains 48 bytes of implementation-defined
        ///  metadata associated with the file. This field contains
        ///  Distributed Link Tracking information for the file,
        ///  if any, if the file is stored in an NTFS file system.
        ///  The first 16 bytes contain the file's birth volume
        ///  ID, which is the volume on which the object resided
        ///  when the object ID was created, or 0 if the volume
        ///  had no object identifier at that time. The next 16
        ///  bytes contain the object ID at the time the object
        ///  was created, which MUST be unique within the volume
        ///  on which it was created. The next 16 bytes contain
        ///  the domain ID, which is set to 0. The FILE_OBJECTID_BUFFER
        ///  structure is specified in [MS-FSCC] section.
        ///  For more information about the Distributed Link Tracking
        ///  Service, see [MS-DLTCS] and [MS-DLTW].
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] Data;
    }

    /// <summary>
    /// Reparse Tag Values Enum
    /// </summary>
    public enum FlatData_ReparseTag_Values : uint
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
}
