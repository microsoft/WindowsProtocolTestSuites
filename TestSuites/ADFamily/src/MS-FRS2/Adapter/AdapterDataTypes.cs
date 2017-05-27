// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Frs2DataTypes
{
    using System;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.Messages;
    using Microsoft.Protocols.TestTools.Messages.Marshaling;
    using System.Runtime.InteropServices;

    #region enum
    /// <summary>
    /// Specifies an enum which represents the authentication level used for RPC binding
    /// </summary>
    public enum RPC_C_AUTHN_LEVEL
    {
        /// <summary>
        /// Tells DCOM to choose the authentication level using its normal security blanket
        /// negotiation algorithm.
        ///This behavior occurs in Windows 2000 and later versions.
        ///In Windows NT 4.0, this value defaults to RPC_C_AUTHN_LEVEL_CONNECT.
        /// </summary>
        RPC_C_AUTHN_LEVEL_DEFAULT = 0,

        /// <summary>
        /// Performs no authentication.
        /// </summary>
        RPC_C_AUTHN_LEVEL_NONE = 1,

        /// <summary>
        /// Authenticates the credentials of the client only when the client establishes a relationship
        /// with the server.
        /// Datagram transports always use RPC_AUTHN_LEVEL_PKT instead
        /// </summary>
        RPC_C_AUTHN_LEVEL_CONNECT = 2,

        /// <summary>
        /// Authenticates only at the beginning of each remote procedure call when the server receives
        /// the request.Datagram transports use RPC_C_AUTHN_LEVEL_PKT instead. 
        /// </summary>
        RPC_C_AUTHN_LEVEL_CALL = 3,

        /// <summary>
        /// Authenticates that all data received is from the expected client. 
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT = 4,


        /// <summary>
        /// Authenticates and verifies that none of the data transferred between client and server
        /// has been modified.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT_INTEGRITY = 5,

        /// <summary>
        /// Authenticates all previous levels and encrypts the argument value of each remote
        /// procedure call.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 6
    }

    /// <summary>
    /// Specifies the authentication service used for RPC binding
    /// </summary>
    public enum RPC_C_AUTHN_SVC
    {
        /// <summary>
        /// Specifies that RPC_C_AUTHN_NONE is set
        /// </summary>
        RPC_C_AUTHN_NONE = 0,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_DCE_PRIVATE is set
        /// </summary>
        RPC_C_AUTHN_DCE_PRIVATE = 1,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_DCE_PUBLIC is set
        /// </summary>
        RPC_C_AUTHN_DCE_PUBLIC = 2,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_DEC_PUBLIC is set
        /// </summary>
        RPC_C_AUTHN_DEC_PUBLIC = 4,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_GSS_NEGOTIATE is set
        /// </summary>
        RPC_C_AUTHN_GSS_NEGOTIATE = 9,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_WINNT is set
        /// </summary>
        RPC_C_AUTHN_WINNT = 10,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_GSS_SCHANNEL is set
        /// </summary>
        RPC_C_AUTHN_GSS_SCHANNEL = 14,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_GSS_KERBEROS is set
        /// </summary>
        RPC_C_AUTHN_GSS_KERBEROS = 16,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_MSN is set
        /// </summary>
        RPC_C_AUTHN_MSN = 17,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_DPA is set
        /// </summary>
        RPC_C_AUTHN_DPA = 18,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_MQ is set
        /// </summary>
        RPC_C_AUTHN_MQ = 100,
        //RPC_C_AUTHN_DEFAULT = 0xFFFFFFFFL,
    }
    #endregion
   

    /// <summary>
    /// Enumeration that repsent RDC support.
    /// </summary>
    public enum _SupportsRDC
    {
        /// <summary>
        /// Supports Rdc Transfer
        /// </summary>
        SupportsRDC = 1,
        /// <summary>
        /// Not Supporting RDC
        /// </summary>
        notSupportsRDC = 0,
        /// <summary>
        /// Invalid Option.
        /// </summary>
        Invalid
    }
    /// <summary>
    /// Error Codes.
    /// </summary>
    public enum Error
        {
        /// <summary>
        /// Error Success
        /// </summary>
        FRS_ERROR_SUCCESS=0,
        /// <summary>
        /// Invalid ConnectionId error
        /// </summary>
        FRS_ERROR_CONNECTION_INVALID=0x00002342,
        /// <summary>
        /// Error contentSet is Read only.
        /// </summary>
        FRS_ERROR_CONTENTSET_READ_ONLY=0x00002375,
        /// <summary>
        /// Error ContentSet is not found.
        /// </summary>
        FRS_ERROR_CONTENTSET_NOT_FOUND=0x00002344
    
    }

    /// <summary>
    /// The TransportFlags enumerated type has only one flag
    /// defined, TRANSPORT_OS_ENTERPRISE.  				 				 			
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_1.xml
    //  </remarks>
    [Flags()]
    public enum _TransportFlags
    {

        /// <summary>
        ///  This flag is set by the server when the server is capable
        ///  of using similarity features of RDC.
        /// </summary>
        TRANSPORT_OS_ENTERPRISE = 1,
    }

    /// <summary>
    ///  The RDC_FILE_COMPRESSION_TYPES enumerated type identifies
    ///  the data compression algorithm used for the file transfer.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_2.xml
    //  </remarks>
    [Flags()]
    public enum _RDC_FILE_COMPRESSION_TYPES
    {

        /// <summary>
        ///  Data is not compressed. This value MUST be SENT whenever
        ///  an RDC_FILE_COMPRESSION_TYPES enum value is required.
        ///  
        /// </summary>
        RDC_UNCOMPRESSED = 0,

        /// <summary>
        ///  Not used.
        /// </summary>
        RDC_XPRESS = 1,
    }

    /// <summary>
    ///  The RDC_CHUNKER_ALGORITHM enumerated type identifies
    ///  the RDC chunking algorithm used to generate the signatures
    ///  for the file to be transferred.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_3.xml
    //  </remarks>
    [Flags()]
    public enum _RDC_CHUNKER_ALGORITHM
    {

        /// <summary>
        ///  Not used.  MUST not be sent.  If received, call MUST
        ///  fail.
        /// </summary>
        RDC_FILTERGENERIC = 0,

        /// <summary>
        ///  FilterMax chunker algorithm is used. This value MUST
        ///  be sent whenever an RDC_CHUNKER_ALGORITHM enum value
        ///  is required.
        /// </summary>
        RDC_FILTERMAX = 1,

        /// <summary>
        ///  Not used.  MUST not be sent.  If received, call MUST
        ///  fail.
        /// </summary>
        RDC_FILTERPOINT = 2,

        /// <summary>
        ///  Not used.  MUST not be sent.  If received, call MUST
        ///  fail.
        /// </summary>
        RDC_MAXALGORITHM = 3,
    }

    /// <summary>
    ///  The UPDATE_REQUEST_TYPE enumerated type specifies the
    ///  type of updates for this request.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_4.xml
    //  </remarks>
    [Flags()]
    public enum _UPDATE_REQUEST_TYPE
    {

        /// <summary>
        ///  Request all updates that pertain to a version chain
        ///  vector.
        /// </summary>
        UPDATE_REQUEST_ALL = 0,

        /// <summary>
        ///  Request only tombstone updates that pertain to a version
        ///  chain vector.
        /// </summary>
        UPDATE_REQUEST_TOMBSTONES = 1,

        /// <summary>
        ///  Request only non-tombstone updates that pertain to a
        ///  version chain vector.
        /// </summary>
        UPDATE_REQUEST_LIVE = 2,
    }

    /// <summary>
    ///  In response to a request for updates, a server MUST
    ///  use a value of the  UPDATE_STATUS enumerated type to
    ///  specify whether it was able to send all updates that
    ///  pertain to an argument version chain vector.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_5.xml
    //  </remarks>
    [Flags()]
    public enum _UPDATE_STATUS
    {

        /// <summary>
        ///  To indicate to the client to not request further updates
        ///  immediately.
        /// </summary>
       // UPDATE_STATUS_WAIT = 0,

        /// <summary>
        ///  To indicate to the client that there are potentially
        ///  more non-tombstone updates from the argument version
        ///  chain vector.
        /// </summary>
        //UPDATE_STATUS_LIVE = 1,

        /// <summary>
        ///  There are no more updates that pertain to the argument
        ///  version chain vector.  In other words, the server does
        ///  not have any updates whose versions belong to the version
        ///  chain vector passed in by the client.
        /// </summary>
        UPDATE_STATUS_DONE = 0,

        /// <summary>
        ///  There are potentially more updates (tombstone, if the
        ///  client requested tombstones; live, if the client requested
        ///  live) from the argument version chain vector.
        /// </summary>
        UPDATE_STATUS_MORE = 1,
    }

    /// <summary>
    ///  The RECORDS_STATUS enumerated type is used for an output
    ///  parameter of a Slow Sync request. It indicates whether
    ///  the server has more records in the scope of the replicated
    ///  folder over which Slow Sync is performed.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_6.xml
    //  </remarks>
    [Flags()]
    public enum _RECORDS_STATUS
    {

        /// <summary>
        ///  No more records are waiting to be transmitted on the
        ///  server.
        /// </summary>
        RECORDS_STATUS_DONE = 1,

        /// <summary>
        ///  More records are waiting to be transmitted on the server.
        /// </summary>
        RECORDS_STATUS_MORE = 2,
    }

    /// <summary>
    ///  The VERSION_REQUEST_TYPE enumerated value is used to
    ///  indicate what role the client version vector request
    ///  has.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_7.xml
    //  </remarks>
    [Flags()]
    public enum _VERSION_REQUEST_TYPE
    {

        /// <summary>
        ///  Indicates that the client requests a version vector
        ///  from the server for standard synchronization.
        /// </summary>
        REQUEST_NORMAL_SYNC = 0,

        /// <summary>
        ///  Indicates that the client requests a version vector
        ///  from the server for Slow Sync.
        /// </summary>
        REQUEST_SLOW_SYNC = 1,

        /// <summary>
        ///  Indicates that the client requests a version vector
        ///  from the server for selective single master mode.
        /// </summary>
        REQUEST_SLAVE_SYNC = 2,
    }

    /// <summary>
    ///  A clientversion vector request uses a value of VERSION_CHANGE_TYPE
    ///  to indicate whether it is requesting a version chain
    ///  vector change notification, a reduced increment, or
    ///  a full version chain vector.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_8.xml
    //  </remarks>
    [Flags()]
    public enum _VERSION_CHANGE_TYPE
    {

        /// <summary>
        ///  The client requests notification only for a change of
        ///  the server's version chain vector.
        /// </summary>
        CHANGE_NOTIFY = 0,

        /// <summary>
        ///  The client requests receiving only portions of a version
        ///  vector that have changed relative to time stamp on
        ///  the server.
        /// </summary>
        CHANGE_DELTA = 1,

        /// <summary>
        ///  The client requests to receive the full version vector
        ///  of the server.
        /// </summary>
        CHANGE_ALL = 2,
    }

    /// <summary>
    ///  The  FRS_REQUESTED_STAGING_POLICY enumerated type indicates
    ///  the staging policy for the server to use.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_2_9.xml
    //  </remarks>
    [Flags()]
    public enum _FRS_REQUESTED_STAGING_POLICY
    {

        /// <summary>
        ///  The client indicates to the server that the server is
        ///  free to use or bypass its cache.
        /// </summary>
        SERVER_DEFAULT = 1,

        /// <summary>
        ///  The client indicates to the server to store the served
        ///  content in its cache.
        /// </summary>
        STAGING_REQUIRED = 2,

        /// <summary>
        ///  The client indicates to the server to purge existing
        ///  content from its cache.
        /// </summary>
        RESTAGING_REQUIRED = 3,
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
    //   file:///D:/IDL_frs2/BKUP/_rfc_ms-bkup2_2_10.xml
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

    /// <summary>
    ///  The WIN32_STREAM_ID structure is a header that precedes
    ///  each backup stream in the NT backup file. This header
    ///  identifies the type of backup stream, its size, and
    ///  other attributes. The structure is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/BKUP/_rfc_ms-bkup2_2_2.xml
    //  </remarks>
    public partial struct WIN32_STREAM_ID
    {

        /// <summary>
        ///  A 32-bit, unsigned integer that indicates the
        ///  type of data in this backup stream. The value of this
        ///  field MUST be one of the following.
        /// </summary>
        public dwStreamId_Values dwStreamId;

        /// <summary>
        ///  A 32-bit, unsigned long integer that indicates
        ///  properties of the backup stream. The value of this
        ///  field MUST be the bitwise OR of zero or more of the
        ///  following.  Other bits are unused and MUST be 0 and
        ///  ignored on receipt.
        /// </summary>
        public dwStreamAttributes_Values dwStreamAttributes;

        /// <summary>
        ///  A 64-bit, unsigned integer that specifies the
        ///  length of the data portion of the backup stream; this
        ///  length MUST NOT include the length of the header. 
        ///  The next backup stream within the NT backup file, if
        ///  any, MUST start at Size + dwStreamNameSize bytes beyond
        ///  the end of this WIN32_STREAM_ID structure. Note that
        ///  the alternate stream name, whose size is dwStreamNameSize
        ///  bytes, is part of the header for the purposes of calculating
        ///  the position of the next WIN32_STREAM_ID structure.
        /// </summary>
        public _LARGE_INTEGER Size;

        /// <summary>
        ///  A 32-bit, unsigned integer that specifies the
        ///  length of the alternate stream name, in bytes. The
        ///  value of this field MUST be 0 for all dwStreamId values
        ///  other than ALTERNATE_DATA.  For StreamID ALTERNATE_DATA,
        ///  the value of this field MUST be in the range 065536,
        ///  and it MUST be an integral multiple of two.
        /// </summary>
        public uint dwStreamNameSize;

        /// <summary>
        ///  A Unicode string that specifies the name of the
        ///  alternate stream. This string MUST NOT be null-terminated.
        /// </summary>
        [Size("dwStreamNameSize / 2")]
        public ushort[] cStreamName;
    }
    /// <summary>
    /// dwStreamId_Values.
    /// </summary>

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

    /// <summary>
    /// DownStream Attribute Values.
    /// </summary>
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
    }


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


    /// <summary>
    ///  The Object ID Backup Stream contains a unique identifier
    ///  for a file. The structure of the data portion of this
    ///  backup stream is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/BKUP/_rfc_ms-bkup2_2_7.xml
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
    ///  The Generic Reparse Data Buffer data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_1_2_1.xml
    //  </remarks>
    public partial struct Generic_Reparse_Data_Buffer
    {

        /// <summary>
        ///  A 32-bit unsigned integer value containing the reparse
        ///  point tag that uniquely identifies the owner (that
        ///  is, the implementer of the filter driver associated
        ///  with this reparse tag) of the reparse point.
        /// </summary>
        public uint ReparseTag;

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size,
        ///  in bytes, of the reparse data in the DataBuffer member.
        /// </summary>
        public ushort ReparseDataLength;

        /// <summary>
        ///  A 16-bit field. This field is reserved. This field SHOULD
        ///  be set to 0, and MUST be ignored.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        ///  A variable-length array of 8-bit unsigned integer values
        ///  containing reparse-specific data for the reparse point.
        ///  The format of this data is defined by the owner (that
        ///  is, the implementer of the filter driver associated
        ///  with the specified ReparseTag) of the reparse point.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] DataBuffer;
    }

    /// <summary>
    ///  The Symbolic Link Reparse Data Buffer data element,
    ///  which contains information on symbolic link reparse
    ///  points, is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_1_2_2.xml
    //  </remarks>
    public partial struct Symbolic_Link_Reparse_Data_Buffer
    {

        /// <summary>
        ///  A 32-bit unsigned integer value containing the reparse
        ///  point tag that uniquely identifies the owner (that
        ///  is, the implementer of the filter driver associated
        ///  with this ReparseTag) of the reparse point. This value
        ///  MUST be 0xA000000C, a reparse point tag assigned to
        ///  Microsoft.
        /// </summary>
        public ReparseTag_Values ReparseTag;

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size,
        ///  in bytes, of the reparse data in the PathBuffer member.
        ///  This value is the length of the data starting at the
        ///  SubstituteNameOffset field (or SubstituteNameLength
        ///  + PrintNameLength + 12).
        /// </summary>
        public ushort ReparseDataLength;

        /// <summary>
        ///  A 16-bit field. This field is not used. It SHOULD be
        ///  set to 0, and MUST be ignored.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST contain the offset,
        ///  in bytes, of the substitute name string in the PathBuffer
        ///  array, computed as an offset from byte 0 of PathBuffer.
        ///  Note that this offset must be divided by 2 to get the
        ///  array index.
        /// </summary>
        public ushort SubstituteNameOffset;

        /// <summary>
        ///   A 16-bit unsigned integer that contains the length
        ///  in bytes of the substitute name string. If this string
        ///  is NULL-terminated, SubstituteNameLength does not include
        ///  the Unicode NULL character.
        /// </summary>
        public ushort SubstituteNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the offset,
        ///  in bytes, of the print name string in the PathBuffer
        ///  array, computed as an offset from byte 0 of PathBuffer.
        ///  Note that this offset must be divided by 2 to get the
        ///  array index.
        /// </summary>
        public ushort PrintNameOffset;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the length,
        ///  in bytes, of the print name string. If this string
        ///  is NULL-terminated, PrintNameLength does not include
        ///  the Unicode NULL character.
        /// </summary>
        public ushort PrintNameLength;

        /// <summary>
        ///  A 32-bit bit field that specifies whether the path name
        ///  given in the SubstituteName field contains a full 
        ///  path name or a path name relative to the source. A
        ///  relative path name, such as "..\locals", accesses a
        ///  directory relative to the current position in the directory
        ///  hierarchy.This field contains one of the values in
        ///  the table below.
        /// </summary>
        public Flags_Values Flags;

        /// <summary>
        ///  Unicode string that contains the substitute name string
        ///  and print name string. The substitute name and print
        ///  name strings can appear in any order in the PathBuffer.
        ///  To locate the substitute name and print name strings
        ///  in the PathBuffer, use the SubstituteNameOffset, SubstituteNameLength,
        ///  PrintNameOffset, and PrintNameLength members.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] PathBuffer;
    }
    /// <summary>
    /// ReparseTag Values.
    /// </summary>
    public enum ReparseTag_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        IO_REPARSE_TAG_SYMLINK = 0xA000000C,
    }


    /// <summary>
    /// Flag values.
    /// </summary>
    [Flags()]
    public enum Flags_Values : uint
    {

        /// <summary>
        ///  The path name given in the SubstituteName field contains
        ///  a full  path name.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  When this Flags value is set, the given path name is
        ///  relative to the source.
        /// </summary>
        SYMLINK_FLAG_RELATIVE = 0x00000001,
    }

    /// <summary>
    ///  The Mount Point Reparse Data Buffer data element, which
    ///  contains information about mount point reparse points,
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_1_2_3.xml
    //  </remarks>
    public partial struct Mount_Point_Reparse_Data_Buffer
    {

        /// <summary>
        ///  A 32-bit unsigned integer value containing the reparse
        ///  point tag that uniquely identifies the owner (that
        ///  is, the implementer of the filter driver associated
        ///  with this ReparseTag) of the reparse point. This value
        ///  MUST be 0xA0000003, a reparse point tag assigned to
        ///  Microsoft.
        /// </summary>
        public Mount_Point_Reparse_Data_Buffer_ReparseTag_Values ReparseTag;

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size,
        ///  in bytes, of the reparse data in the PathBuffer member.
        ///  This value is the length of the data starting at the
        ///  SubstituteNameOffset field (or SubstituteNameLength
        ///  + PrintNameLength + 8).
        /// </summary>
        public ushort ReparseDataLength;

        /// <summary>
        ///  A 16-bit field. This field is not used. It SHOULD be
        ///  set to 0, and MUST be ignored.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        ///  A 16-bit unsigned integer that MUST contain the offset,
        ///  in bytes, of the substitute name string in the PathBuffer
        ///  array, computed as an offset from byte 0 of PathBuffer.
        ///  Note that this offset must be divided by 2 to get the
        ///  array index.
        /// </summary>
        public ushort SubstituteNameOffset;

        /// <summary>
        ///   A 16-bit unsigned integer that contains the length,
        ///  in bytes, of the substitute name string. If this string
        ///  is NULL-terminated, SubstituteNameLength does not include
        ///  the Unicode NULL character.
        /// </summary>
        public ushort SubstituteNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the offset,
        ///  in bytes, of the print name string in the PathBuffer
        ///  array, computed as an offset from byte 0 of PathBuffer.
        ///  Note that this offset must be divided by 2 to get the
        ///  array index.
        /// </summary>
        public ushort PrintNameOffset;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the length,
        ///  in bytes, of the print name string. If this string
        ///  is NULL-terminated, PrintNameLength does not include
        ///  the Unicode NULL character.
        /// </summary>
        public ushort PrintNameLength;

        /// <summary>
        ///  Unicode string that contains the substitute name string
        ///  and print name string. The substitute name and print
        ///  name strings can appear in any order in PathBuffer.
        ///  To locate the substitute name and print name strings
        ///  in the PathBuffer field, use the SubstituteNameOffset,
        ///  SubstituteNameLength, PrintNameOffset, and PrintNameLength
        ///  members.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] PathBuffer;
    }
    /// <summary>
    /// Mount_Point_Reparse_Data_Buffer_ReparseTag_Values
    /// </summary>
    public enum Mount_Point_Reparse_Data_Buffer_ReparseTag_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003,
    }

    /// <summary>
    ///  The REPARSE_GUID_DATA_BUFFER data element MUST be used
    ///  by all non-Microsoft file systems, filters, and minifilters
    ///  to store data for a reparse point. Each reparse point
    ///  contains one REPARSE_GUID_DATA_BUFFER structure.Reparse
    ///  point tags are assigned to independent software vendors
    ///  (ISVs) by Microsoft. Reparse point GUIDs are assigned
    ///  by the ISV. An ISV MUST associate one GUID to each
    ///  assigned reparse point tag, and MUST always use that
    ///  GUID with that tag. For more information about reparse
    ///  points, see [REPARSE]. If the high bit of the ReparseTag
    ///  element is 0, an application MUST interpret the reparse
    ///  point data as a REPARSE_GUID_DATA_BUFFER; otherwise,
    ///  it MUST be interpreted as a REPARSE_DATA_BUFFER.Each
    ///  reparse point MUST contain one REPARSE_GUID_DATA_BUFFER
    ///  structure. The REPARSE_GUID_DATA_BUFFER data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_1_3.xml
    //  </remarks>
    public partial struct REPARSE_GUID_DATA_BUFFER
    {

        /// <summary>
        ///  A 32-bit unsigned integer value containing the reparse
        ///  point tag that uniquely identifies the owner of the
        ///  reparse point. This tag MUST match the reparse tag
        ///  of the reparse point on which the FSCTL is being invoked.
        /// </summary>
        public uint ReparseTag;

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size,
        ///  in bytes, of the reparse data in the DataBuffer member.
        /// </summary>
        public ushort ReparseDataLength;

        /// <summary>
        ///  A 16-bit field. This field SHOULD be set to 0 by the
        ///  client, and MUST be ignored by the server.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the owner of
        ///  the reparse point. Reparse point GUIDs are not assigned
        ///  by Microsoft. A reparse point implementer MUST select
        ///  one GUID to be used with their assigned reparse point
        ///  tag to uniquely identify that reparse point. For more
        ///  information, see [REPARSE].
        /// </summary>
        public System.Guid ReparseGuid;

        /// <summary>
        ///  The content of this buffer is undefined to the file
        ///  system. On receipt, its   content MUST be preserved
        ///  and properly returned to the caller.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] DataBuffer;
    }

    /// <summary>
    ///  The first possible structure for the FILE_OBJECTID_BUFFER
    ///  data element follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_1_4_1.xml
    //  </remarks>
    public partial struct FILE_OBJECTID_BUFFER_Type_1
    {

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the file or
        ///  directory within the volume  on which it resides. Specifically,
        ///  the same object ID can be assigned to another file
        ///  or directory on a different volume, but it MUST NOT
        ///  be assigned to another file or directory on the same
        ///  volume.
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the volume on
        ///  which the object resided when the object identifier
        ///  was created, or zero if the volume had no object identifier
        ///  at that time. After copy operations, move operations,
        ///  or other file operations, this may not be the same
        ///  as the object identifier of the volume on which the
        ///  object presently resides.
        /// </summary>
        public System.Guid BirthVolumeId;

        /// <summary>
        ///  A 16-byte GUID value containing the object identifier
        ///  of the object at the time it was created. Copy operations,
        ///  move operations, or other file operations MAY change
        ///  the value of the ObjectId member. Therefore, the BirthObjectId
        ///  MAY not be the same as the ObjectId member at present.
        ///  Specifically, the same object ID MAY be assigned to
        ///  another file or directory on a different volume, but
        ///  it MUST NOT be assigned to another file or directory
        ///  on the same volume. The object ID is assigned at file
        ///  creation time. When a file is moved or copied from one
        ///  volume to another, the ObjectId member value changes
        ///  to a random unique value to avoid the potential for
        ///  ObjectId collisions because the object ID is not guaranteed
        ///  to be unique across volumes.
        /// </summary>
        public System.Guid BirthObjectId;

        /// <summary>
        ///  A 16-byte GUID value containing the domain identifier.
        ///  This value is unused; it SHOULD be zero, and MUST be
        ///  ignored.The NTFS file system places no constraints
        ///  on the format of the 48 bytes of information following
        ///  the ObjectId in this structure. This format of the
        ///  FILE_OBJECTID_BUFFER is used on by the Microsoft Distributed
        ///  Link Tracking Service (see [MS-DLTW] section ).
        /// </summary>
        public System.Guid DomainId;
    }

    /// <summary>
    ///  The second possible structure for the FILE_OBJECTID_BUFFER
    ///  data element follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_1_4_2.xml
    //  </remarks>
    public partial struct FILE_OBJECTID_BUFFER_Type_2
    {

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the file or
        ///  directory within the volume on which it resides.  Specifically,
        ///  the same object ID can be assigned to another file
        ///  or directory on a different volume, but it MUST NOT
        ///  be assigned to another file or directory on the same
        ///  volume.
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        ///  A 48-byte value containing extended data that was set
        ///  with the FSCTL_SET_OBJECT_ID_EXTENDED request. This
        ///  field contains application-specific data. Places Distributed
        ///  Link Tracking information into the ExtendedInfo field
        ///  for use by the Distributed Link Tracking protocols
        ///  (see [MS-DLTW] section ).
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    ///  The FSCTL_FIND_FILES_BY_SID reply message returns the
    ///  results of the FSCTL_FIND_FILES_BY_SID request as an
    ///  array of FIND_BY_SID_OUTPUT data elements, one for
    ///  each matching file that is found. 				The FIND_BY_SID_OUTPUT
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_10.xml
    //  </remarks>
    public partial struct FSCTL_FIND_FILES_BY_SID_Reply
    {

        /// <summary>
        ///  A 16-bit unsigned integer value containing the size
        ///  of the file name in bytes. This size does not include
        ///  the NULL character.
        /// </summary>
        public ushort FileNameLength;

        /// <summary>
        ///  A null-terminated Unicode string that specifies the
        ///  fully qualified path name for the file.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    ///  The FSCTL_GET_COMPRESSION reply message returns the
    ///  results of the FSCTL_GET_COMPRESSION request as a 16-bit
    ///  unsigned integer value that indicates the current compression
    ///  state of the file or directory. The CompressionState
    ///  element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_12.xml
    //  </remarks>
    public partial struct FSCTL_GET_COMPRESSION_Reply
    {

        /// <summary>
        ///  One of the following standard values MUST be returned.
        /// </summary>
        public CompressionState_Values CompressionState;
    }

    /// <summary>
    /// Compression State Values.
    /// </summary>
    [Flags()]
    public enum CompressionState_Values : ushort
    {

        /// <summary>
        ///  The file or directory is not compressed.
        /// </summary>
        COMPRESSION_FORMAT_NONE = 0x0000,

        /// <summary>
        ///  The file or directory is compressed by using the default
        ///  compression algorithm.Equivalent to COMPRESSION_FORMAT_LZNT1.
        /// </summary>
        COMPRESSION_FORMAT_DEFAULT = 0x0001,

        /// <summary>
        ///  The file or directory is compressed by using the LZNT1
        ///  compression algorithm. For more information, see [UASDC].
        /// </summary>
        COMPRESSION_FORMAT_LZNT1 = 0x0002,

        /// <summary>
        ///  Reserved for future use by Microsoft and SHOULD NOT
        ///  be used by others.
        /// </summary>
        All_other_values,
    }

    /// <summary>
    ///  The FSCTL_GET_RETRIEVAL_POINTERS request message requests
    ///  that the server return a variably sized data element,
    ///  StartingVcn, that describes the location on disk of
    ///  the file or directory associated with the handle on
    ///  which this FSCTL was invoked. It is most commonly used
    ///  by defragmentation utilities. This data element describes
    ///  the mapping between virtual cluster numbers and logical
    ///  cluster numbers.The STARTING_VCN_INPUT_BUFFER data
    ///  element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_17.xml
    //  </remarks>
    public partial struct FSCTL_GET_RETRIEVAL_POINTERS_Request
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the virtual cluster
        ///  number (VCN) at which to begin retrieving extents in
        ///  the file. This value MAY be rounded down to the first
        ///  VCN of the extent in which the given extent is found.
        ///  This value MUST be greater than or equal to 0.
        /// </summary>
        public long StartingVcn;
    }

    /// <summary>
    ///  The FSCTL_GET_RETRIEVAL_POINTERS reply message returns
    ///  the results of the FSCTL_GET_RETRIEVAL_POINTERS request
    ///  as a variably sized data element, RETRIEVAL_POINTERS_BUFFER,
    ///  that specifies the allocation and location on disk
    ///  of a specific file. 				For the NTFS file system, the
    ///  FSCTL_GET_RETRIEVAL_POINTERS reply returns the extent
    ///  locations (that is, locations of allocated regions
    ///  of disk space) of nonresident data. A file system MAY
    ///  allow resident data, which is data that can be written
    ///  to disk within the file's directory record. Because
    ///  resident data requires no additional disk space allocation,
    ///  no extent locations are associated with resident data. On
    ///  an NTFS volume, very short data streams (typically
    ///  several hundred bytes) can be written to disk without
    ///  having any clusters allocated. These short streams
    ///  are sometimes called resident because the data resides
    ///  in the file's master file table (MFT) record. A resident
    ///  data stream has no retrieval pointers to return. The
    ///  RETRIEVAL_POINTERS_BUFFER data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_18.xml
    //  </remarks>
    public partial struct FSCTL_GET_RETRIEVAL_POINTERS_Reply
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  EXTENTS data elements in the Extents array. This number
        ///  can be zero if there are no clusters allocated at (or
        ///  beyond) the specified StartingVcn.
        /// </summary>
        public uint ExtentCount;

        /// <summary>
        ///  Unused area for data alignment purposes.
        /// </summary>
        public uint Unused;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting virtual
        ///  cluster number (VCN) returned by the FSCTL_GET_RETRIEVAL_POINTERS
        ///  reply. This is not necessarily the VCN requested by
        ///  the FSCTL_GET_RETRIEVAL_POINTERS request, as the file
        ///  system driver might round down to the first VCN of
        ///  the extent in which the requested starting VCN is found.
        ///  This value MUST be greater than or equal to 0.
        /// </summary>
        public long StartingVcn;

        /// <summary>
        ///  An array of zero or more EXTENTS data elements. For
        ///  the number of EXTENTS data elements in the array, see
        ///  ExtentCount.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] Extents;
    }

    /// <summary>
    ///  The EXTENTS data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_18_1.xml
    //  </remarks>
    public partial struct EXTENTS
    {

        /// <summary>
        ///  A 64-bit unsigned integer that contains the VCN at which
        ///  the next extent begins. This value minus either StartingVcn
        ///  (for the first Extents array element) or the NextVcn
        ///  of the previous element of the array (for all other
        ///  Extents array elements) is the length in clusters of
        ///  the current extent. The length is an input to the FSCTL_MOVE_FILE
        ///  request.
        /// </summary>
        public ulong NextVcn;

        /// <summary>
        ///  A 64-bit unsigned integer that contains the logical
        ///  cluster number (LCN) at which the current extent begins
        ///  on the volume. On the NTFS file system, a 64-bit value
        ///  of 0xFFFFFFFFFFFFFFFF indicates either a compression
        ///  unit that is partially allocated or an unallocated
        ///  region of a sparse file. For more information about
        ///  sparse files, see [SPARSE]. NTFS performs compression
        ///  in 16-cluster units. If a given 16cluster unit compresses
        ///  to fit in, for example, 9 clusters, there will be a
        ///  7cluster extent of the file with an LCN of -1.
        /// </summary>
        public ulong Lcn;
    }

    /// <summary>
    ///  The FSCTL_IS_PATHNAME_VALID request message requests
    ///  that the server return whether or not the specified
    ///  path name associated with the handle on which this
    ///  FSCTL was invoked is composed of valid characters with
    ///  respect to the remote file system storage. 				The
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_19.xml
    //  </remarks>
    public partial struct FSCTL_IS_PATHNAME_VALID_Request
    {

        /// <summary>
        ///  An unsigned 32-bit integer that specifies the length,
        ///  in bytes, of the PathName data element.
        /// </summary>
        public uint PathNameLength;

        /// <summary>
        ///   A variable-length Unicode string that specifies the
        ///  path name.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] PathName;
    }

    /// <summary>
    ///  The FSCTL_LMR_GET_LINK_TRACKING_INFORMATION reply message
    ///  returns the results of the FSCTL_LMR_GET_LINK_TRACKING_INFORMATION
    ///  request. The LINK_TRACKING_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_22.xml
    //  </remarks>
    public partial struct FSCTL_LMR_GET_LINK_TRACKING_INFORMATION_Reply
    {

        /// <summary>
        ///  An unsigned 32-bit integer that indicates the type of
        ///  file system on which the file is hosted on the destination
        ///  computer. This value MUST be one of the following:
        /// </summary>
        public Type_Values Type;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the volume for
        ///  the object, as obtained from the reply to an FSCTL_LMR_GET_LINK_TRACKING_INFORMATION
        ///  request, called by using the file handle of the destination
        ///  file.
        /// </summary>
        public System.Guid VolumeId;
    }

    /// <summary>
    /// Type values
    /// </summary>
    [Flags()]
    public enum Type_Values : uint
    {

        /// <summary>
        ///  The destination file system is NTFS.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  The destination file system is DFS. For more information,
        ///  see [MSDFS].
        /// </summary>
        V2 = 0x00000001,
    }

    /// <summary>
    ///  The FSCTL_LMR_SET_LINK_TRACKING_INFORMATION request
    ///  message sets Distributed Link Tracking information
    ///  such as file system type, volume ID, object ID, and
    ///  destination computer's NetBIOS name for the file associated
    ///  with the handle on which this FSCTL was invoked. The
    ///  message contains a REMOTE_LINK_TRACKING_INFORMATION
    ///  data element. For more information about Distributed
    ///  Link Tracking, see [MS-DLTW] section.The REMOTE_LINK_TRACKING_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_23.xml
    //  </remarks>
    public partial struct FSCTL_LMR_SET_LINK_TRACKING_INFORMATION_Request
    {

        /// <summary>
        ///  Fid of the file from which to obtain link tracking information.
        ///   For Fid type, see [MS-SMB] section.
        /// </summary>
        public uint TargetFileObject;

        /// <summary>
        ///  Length of the TargetLinkTrackingInformationBuffer.
        /// </summary>
        public uint TargetLinkTrackingInformationLength;

        /// <summary>
        ///  This field is as specified in TARGET_LINK_TRACKING_INFORMATION_Buffer.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] TargetLinkTrackingInformationBuffer;
    }

    /// <summary>
    ///  If the TargetLinkTrackingInformationLength value is
    ///  greater than or equal to 36, the TARGET_LINK_TRACKING_INFORMATION_Buffer
    ///  data element MUST be as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_23_1.xml
    //  </remarks>
    public partial struct TARGET_LINK_TRACKING_INFORMATION_Buffer_2
    {

        /// <summary>
        ///  An unsigned 32-bit integer that indicates the type of
        ///  file system on which the file is hosted on the destination
        ///  computer. MUST be one of the following:
        /// </summary>
        public TARGET_LINK_TRACKING_INFORMATION_Buffer_2_Type_Values Type;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the volume for
        ///  the object, as obtained from the reply to an  FSCTL_LMR_GET_LINK_TRACKING_INFORMATION
        ///  request, called using the file handle of the destination
        ///  file.
        /// </summary>
        public System.Guid VolumeId;

        /// <summary>
        ///   A 16-byte GUID that uniquely identifies the destination
        ///  file or directory within the volume on which it resides,
        ///  as indicated by VolumeId.
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        ///  A null-terminated ASCII string containing the NetBIOS
        ///  name of the destination computer, if known. For more
        ///  information, see [MS-DLTW] section 3.1.6. If not known,
        ///  this field is zero length and contains nothing.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] NetBIOSName;
    }

    /// <summary>
    /// Enumeration that represents TARGET_LINK_TRACKING_INFORMATION_Buffer_2_Type_Values
    /// </summary>
    [Flags()]
    public enum TARGET_LINK_TRACKING_INFORMATION_Buffer_2_Type_Values : uint
    {

        /// <summary>
        ///  The destination file system is NTFS.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  The destination file system is DFS. For more information,
        ///  see [MSDFS].
        /// </summary>
        V2 = 0x00000001,
    }

    /// <summary>
    ///  If the TargetLinkTrackingInformationLength value is
    ///  less than 36, the TARGET_LINK_TRACKING_INFORMATION_Buffer
    ///  data element MUST be as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_23_1_1_1.xml
    //  </remarks>
    public partial struct TARGET_LINK_TRACKING_INFORMATION_Buffer_1
    {

        /// <summary>
        ///  A null-terminated ASCII string containing the NetBIOS
        ///  name of the destination computer, if known. For more
        ///  information, see [MS-DLTW] section. If not known,
        ///  this field is zero length and contains nothing.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] NetBIOSName;
    }

    /// <summary>
    ///  The FSCTL_QUERY_ALLOCATED_RANGES request message requests
    ///  that the server scan a file or alternate stream looking
    ///  for byte ranges that may contain nonzero data, and
    ///  then return information on those ranges. Only sparse
    ///  files can have zeroed ranges known to the operating
    ///  system. For other files, the server will return only
    ///  a single range that contains the starting point and
    ///  the length requested. The message contains a FILE_ALLOCATED_RANGE_BUFFER
    ///  data element. The FILE_ALLOCATED_RANGE_BUFFER data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_25.xml
    //  </remarks>
    public partial struct FSCTL_QUERY_ALLOCATED_RANGES_Request
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file offset,
        ///  in bytes, of the start of a range of bytes in a file.
        ///  The value of this field MUST be greater than or equal
        ///  to 0.
        /// </summary>
        public long FileOffset;

        /// <summary>
        ///  A 64-bit signed integer that contains the size, in bytes,
        ///  of the range. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long Length;
    }

    /// <summary>
    ///  The FSCTL_QUERY_ALLOCATED_RANGES reply message returns
    ///  the results of the FSCTL_QUERY_ALLOCATED_RANGES request. This
    ///  message MUST return an array of zero or more FILE_ALLOCATED_RANGE_BUFFER
    ///  data elements. The number of FILE_ALLOCATED_RANGE_BUFFER
    ///  elements returned is computed by dividing the size
    ///  of the returned output buffer (from SMB, the lower-level
    ///  protocol that carries the FSCTL) by the size of the
    ///  FILE_ALLOCATED_RANGE_BUFFER element. Ranges returned
    ///  are always at least partially within the range specified
    ///  in the FSCTL_QUERY_ALLOCATED_RANGES request. Zero FILE_ALLOCATED_RANGE_BUFFER
    ///  data elements MUST be returned when the file has no
    ///  allocated ranges. Each entry in the output array contains
    ///  an offset and a length that indicates a range in the
    ///  file that may contain nonzero data. The actual nonzero
    ///  data, if any, is somewhere within this range, and the
    ///  calling application must scan further within the range
    ///  to locate it and determine if it really is valid data.
    ///  Multiple instances of valid data may exist within the
    ///  range.The FILE_ALLOCATED_RANGE_BUFFER data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_26.xml
    //  </remarks>
    public partial struct FSCTL_QUERY_ALLOCATED_RANGES_Reply
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file offset
        ///  in bytes from the start of the file; the start of a
        ///  range of bytes to which storage is allocated. If the
        ///  file is a sparse file, it can contain ranges of bytes
        ///  for which storage is not allocated; these ranges will
        ///  be excluded from the list of allocated ranges returned
        ///  by this FSCTL. Sparse files are supported by  , , ,
        ///  and . The NTFS file system rounds down the input file
        ///  offset to a 65,536byte (64 kilobyte) boundary, rounds
        ///  up the length to a convenient boundary, and then begins
        ///  to walk through the file. Because an application using
        ///  a sparse file can choose whether or not to allocate
        ///  disk space for each sequence of 0x00-valued bytes,
        ///  the allocated ranges can contain 0x00-valued bytes.
        ///  This value MUST be greater than or equal to 0. It does
        ///  not track every piece of zero (0) or nonzero data.
        ///  Because zero (0) is often a perfectly legal datum,
        ///  it would be misleading. Instead, the system tracks
        ///  ranges in which disk space is allocated. Where no disk
        ///  space is allocated, all data bytes within that range
        ///  for Length bytes from FileOffset are assumed to be
        ///  zero (0) (when data is read, NTFS returns a zero for
        ///  every byte in a sparse region). Allocated storage can
        ///  contain zero (0) or nonzero data. So all that this
        ///  operation does is return information on parts of the
        ///  file where nonzero data might be located. It is up
        ///  to the application to scan these parts of the file
        ///  in accordance with the application's data conventions.
        /// </summary>
        public long FileOffset;

        /// <summary>
        ///  A 64-bit signed integer that contains the size,
        ///  in bytes, of the range. This value MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long Length;
    }

    /// <summary>
    ///  The FSCTL_READ_FILE_USN_DATA reply message returns the
    ///  results of the FSCTL_READ_FILE_USN_DATA request as
    ///  a USN_RECORD. 				The USN_RECORD element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_28.xml
    //  </remarks>
    public partial struct FSCTL_READ_FILE_USN_DATA_Reply
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the total length
        ///  of the update sequence number (USN) record in bytes.
        /// </summary>
        public uint RecordLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the major version
        ///  of the change journal software for this record. For
        ///  example, if the change journal software is version
        ///  2.0, the major version number is 2.The major version
        ///  number is 2 for file systems created on , , , and .
        /// </summary>
        public ushort MajorVersion;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the minor version
        ///  of the change journal software for this record. For
        ///  example, if the change journal software is version
        ///  2.0, the minor version number is 0 (zero).The minor
        ///  version number is 0 for file systems created on , ,
        ///  , and .
        /// </summary>
        public ushort MinorVersion;

        /// <summary>
        ///  A 64-bit unsigned integer, opaque to the client, containing
        ///  the number (assigned by the file system when the file
        ///  is created) of the file or directory for which this
        ///  record notes changes. The FileReferenceNumber is an
        ///  arbitrarily assigned value (unique within the volume
        ///  on which the file is stored) that associates a journal
        ///  record with a file. This value SHOULD always be unique
        ///  within the volume on which the file is stored over
        ///  the life of the volume. Computes the file reference
        ///  number as follows: 48 bits are the index of the file's
        ///  primary record in the master file table (MFT), and
        ///  the other 16 bits are a sequence number. Therefore,
        ///  it is possible that a different file can have the same
        ///  FileReferenceNumber as a file on that volume had in
        ///  the past; however, this is an unlikely scenario.
        /// </summary>
        public ulong FileReferenceNumber;

        /// <summary>
        ///   A 64-bit unsigned integer, opaque to the client, containing
        ///  the ordinal number of the directory on which the file
        ///  or directory that is associated with this record is
        ///  located. This is an arbitrarily assigned value (unique
        ///  within the volume on which the file is stored) that
        ///  associates a journal record with a parent directory.
        /// </summary>
        public ulong ParentFileReferenceNumber;

        /// <summary>
        ///  A 64-bit signed integer, opaque to the client, containing
        ///  the USN (update sequence number) of the record. This
        ///  value is unique within the volume on which the file
        ///  is stored. This value MUST be greater than or equal
        ///  to 0. For more information, see [MSDN-CJ].
        /// </summary>
        public long Usn;

        /// <summary>
        ///  A structure containing the absolute system time in UTC
        ///  expressed as the number of 100-nanosecond intervals
        ///  since January 1, 1601 (UTC), in the format of a FILETIME
        ///  structure.
        /// </summary>
        public _FILETIME TimeStamp;

        /// <summary>
        ///  A 32-bit unsigned integer that contains flags that indicate
        ///  reasons for changes that have accumulated in this file
        ///  or directory journal record since the file or directory
        ///  was opened. When a file or directory is closed, a final
        ///  USN record is generated with the USN_REASON_CLOSE flag
        ///  set in this field. The next change, occurring after
        ///  the next open operation or deletion, starts a new record
        ///  with a new set of reason flags. A rename or move operation
        ///  generates two USN records: one that records the old
        ///  parent directory for the item and one that records
        ///  the new parent in the ParentFileReferenceNumber member.
        ///  Possible values for the reason code are as follows
        ///  (all unused bits are reserved for future use by Microsoft
        ///  and SHOULD NOT be used by others).
        /// </summary>
        public Reason_Values Reason;

        /// <summary>
        ///  A 32-bit unsigned integer that provides additional information
        ///  about the source of the change. When a thread writes
        ///  a new USN record, the source information flags in the
        ///  prior record continue to be present only if the thread
        ///  also sets those flags. Therefore, the source information
        ///  structure allows applications to filter out USN records
        ///  that are set only by a known source, for example, an
        ///  antivirus filter. This flag MUST contain one of the
        ///  following values.
        /// </summary>
        public SourceInfo_Values SourceInfo;

        /// <summary>
        ///  A 32-bit unsigned integer that contains a unique security
        ///  identifier (SID) assigned to the file or directory
        ///  associated with this record.
        /// </summary>
        public uint SecurityId;

        /// <summary>
        ///  A 32-bit unsigned integer that contains attributes for
        ///  the file or directory associated with this record.
        ///  Attributes of streams associated with the file or directory
        ///  are excluded. Valid file attributes are specified in
        ///  section.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the length of
        ///  the file or directory associated with this record in
        ///  bytes. The FileName member contains this name. Use
        ///  this member to determine file name length rather than
        ///  depending on a trailing NULL to delimit the file name
        ///  in FileName.
        /// </summary>
        public ushort FileNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the offset in
        ///  bytes of the FileName member from the beginning of
        ///  the structure.
        /// </summary>
        public ushort FileNameOffset;

        /// <summary>
        ///  A variable-length field of UNICODE characters containing
        ///  the name of the file or directory associated with this
        ///  record in Unicode format. When working with this field,
        ///  do not assume that the file name will contain a trailing
        ///  Unicode NULL character.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    /// Enumeration that represents the Reason Values.
    /// </summary>
    [Flags()]
    public enum Reason_Values : uint
    {

        /// <summary>
        ///  A user has either changed one or more files or directory
        ///  attributes (such as read-only, hidden, archive, or
        ///  sparse) or one or more time stamps.
        /// </summary>
        USN_REASON_BASIC_INFO_CHANGE = 0x00008000,

        /// <summary>
        ///  The file or directory is closed.
        /// </summary>
        USN_REASON_CLOSE = 0x80000000,

        /// <summary>
        ///  The compression state of the file or directory is changed
        ///  from (or to) compressed.
        /// </summary>
        USN_REASON_COMPRESSION_CHANGE = 0x00020000,

        /// <summary>
        ///  The file or directory is extended (added to).
        /// </summary>
        USN_REASON_DATA_EXTEND = 0x00000002,

        /// <summary>
        ///  The data in the file or directory is overwritten.
        /// </summary>
        USN_REASON_DATA_OVERWRITE = 0x00000001,

        /// <summary>
        ///  The file or directory is truncated.
        /// </summary>
        USN_REASON_DATA_TRUNCATION = 0x00000004,

        /// <summary>
        ///  The user made a change to the extended attributes of
        ///  a file or directory. These NTFS file system attributes
        ///  are not accessible to based applications. This USN
        ///  reason does not appear under normal system usage, but
        ///  can appear if an application or utility bypasses the
        ///  Win32 API and uses the native API to create or modify
        ///  extended attributes of a file or directory.
        /// </summary>
        USN_REASON_EA_CHANGE = 0x00000400,

        /// <summary>
        ///  The file or directory is encrypted or decrypted.
        /// </summary>
        USN_REASON_ENCRYPTION_CHANGE = 0x00040000,

        /// <summary>
        ///  The file or directory is created for the first time.
        /// </summary>
        USN_REASON_FILE_CREATE = 0x00000100,

        /// <summary>
        ///  The file or directory is deleted.
        /// </summary>
        USN_REASON_FILE_DELETE = 0x00000200,

        /// <summary>
        ///  An NTFS file system hard link is added to (or removed
        ///  from) the file or directory. An NTFS file system hard
        ///  link, similar to a POSIX hard link, is one of several
        ///  directory entries that see the same file or directory.
        /// </summary>
        USN_REASON_HARD_LINK_CHANGE = 0x00010000,

        /// <summary>
        ///  A user changes the FILE_ATTRIBUTE_NOT_CONTEXT_INDEXED
        ///  attribute. That is, the user changes the file or directory
        ///  from one in which content can be indexed to one in
        ///  which content cannot be indexed, or vice versa.
        /// </summary>
        USN_REASON_INDEXABLE_CHANGE = 0x00004000,

        /// <summary>
        ///  The one (or more) named data stream for a file is extended
        ///  (added to).
        /// </summary>
        USN_REASON_NAMED_DATA_EXTEND = 0x00000020,

        /// <summary>
        ///  The data in one (or more) named data stream for a file
        ///  is overwritten.
        /// </summary>
        USN_REASON_NAMED_DATA_OVERWRITE = 0x00000010,

        /// <summary>
        ///  One (or more) named data stream for a file is truncated.
        /// </summary>
        USN_REASON_NAMED_DATA_TRUNCATION = 0x00000040,

        /// <summary>
        ///  The object identifier of a file or directory is changed.
        /// </summary>
        USN_REASON_OBJECT_ID_CHANGE = 0x00080000,

        /// <summary>
        ///  A file or directory is renamed, and the file name in
        ///  the USN_RECORD structure is the new name.
        /// </summary>
        USN_REASON_RENAME_NEW_NAME = 0x00002000,

        /// <summary>
        ///  The file or directory is renamed, and the file name
        ///  in the USN_RECORD structure is the previous name.
        /// </summary>
        USN_REASON_RENAME_OLD_NAME = 0x00001000,

        /// <summary>
        ///  The reparse point that is contained in a file or directory
        ///  is changed, or a reparse point is added to (or deleted
        ///  from) a file or directory.
        /// </summary>
        USN_REASON_REPARSE_POINT_CHANGE = 0x00100000,

        /// <summary>
        ///  A change is made in the access rights to a file or directory.
        /// </summary>
        USN_REASON_SECURITY_CHANGE = 0x00000800,

        /// <summary>
        ///  A named stream is added to (or removed from) a file,
        ///  or a named stream is renamed.
        /// </summary>
        USN_REASON_STREAM_CHANGE = 0x00200000,
    }

    /// <summary>
    /// Enumeration that represents the SourceInfo Values.
    /// </summary>
    [Flags()]
    public enum SourceInfo_Values : uint
    {

        /// <summary>
        ///  The operation provides information about a change to
        ///  the file or directory that was made by the operating
        ///  system. For example, a change journal record with this
        ///  SourceInfo value is generated when the Remote Storage
        ///  system moves data from external to local storage. This
        ///  SourceInfo value indicates that the modifications did
        ///  not change the application data in the file.
        /// </summary>
        USN_SOURCE_DATA_MANAGEMENT = 0x00000001,

        /// <summary>
        ///  The operation adds a private data stream to a file or
        ///  directory. For example, a virus detector might add
        ///  checksum information. As the virus detector modifies
        ///  the item, the system generates USN records. This SourceInfo
        ///  value indicates that the modifications did not change
        ///  the application data in the file.
        /// </summary>
        USN_SOURCE_AUXILIARY_DATA = 0x00000002,

        /// <summary>
        ///  The operation modified the file to match the content
        ///  of the same file that exists in another member of the
        ///  replica set  for the File Replication Service (FRS).
        /// </summary>
        USN_SOURCE_REPLICATION_MANAGEMENT = 0x00000004,
    }

    /// <summary>
    ///  The FSCTL_SET_COMPRESSION request message requests that
    ///  the server set the compression state of the file or
    ///  directory (associated with the handle on which this
    ///  FSCTL was invoked) on a volume whose file system supports
    ///  per-stream compression. The message contains a 16-bit
    ///  unsigned integer. The CompressionState element is as
    ///  follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_31.xml
    //  </remarks>
    public partial struct FSCTL_SET_COMPRESSION_Request
    {

        /// <summary>
        ///  MUST be one of the following standard values:
        /// </summary>
        public FSCTL_SET_COMPRESSION_Request_CompressionState_Values CompressionState;
    }

    /// <summary>
    /// Enumeration that represents  FSCTL_SET_COMPRESSION_Request_CompressionState_Values 
    /// </summary>
    [Flags()]
    public enum FSCTL_SET_COMPRESSION_Request_CompressionState_Values : ushort
    {

        /// <summary>
        ///  The file or directory is not compressed.
        /// </summary>
        COMPRESSION_FORMAT_NONE = 0x0000,

        /// <summary>
        ///  The file or directory is compressed by using the default
        ///  compression algorithm.Equivalent to COMPRESSION_FORMAT_LZNT1.
        /// </summary>
        COMPRESSION_FORMAT_DEFAULT = 0x0001,

        /// <summary>
        ///  The file or directory is compressed by using the LZNT1
        ///  compression algorithm. For more information, see [UASDC].
        /// </summary>
        COMPRESSION_FORMAT_LZNT1 = 0x0002,

        /// <summary>
        ///  Reserved for future use by Microsoft, and SHOULD NOT
        ///  be used by others. This does not restrict the use of
        ///  alternative compression algorithms by others. Because
        ///  compressed data does not travel across the wire in
        ///  the course of FSCTL, FileInformation class, or FileSystemInformation
        ///  class requests or replies, an implementation can associate
        ///  any local compression mechanisms with the above values.
        ///  Particularly, this specification does not require that
        ///  a non-Microsoft implementation implement the LZNT1
        ///  compression algorithm for its file systems for interoperability.
        /// </summary>
        All_other_values,
    }

    /// <summary>
    ///  The FSCTL_SET_OBJECT_ID_EXTENDED request message requests
    ///  that the server set the extended information for the
    ///  file or directory associated with the handle on which
    ///  this FSCTL was invoked. The message contains an EXTENDED_INFO
    ///  data element. The EXTENDED_INFO data element is defined
    ///  as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_35.xml
    //  </remarks>
    public partial struct FSCTL_SET_OBJECT_ID_EXTENDED_Request
    {

        /// <summary>
        ///  A 48-byte BLOB containing user-defined extended data
        ///  that was passed to this FSCTL by an application. In
        ///  this situation, the user refers to the implementer
        ///  who is calling this FSCTL, meaning the extended info
        ///  is opaque to NTFS; there are no rules enforced by NTFS
        ///  as to what these last 48 bytes contain. Contrast this
        ///  with the first 16 bytes of an object ID, which can
        ///  be used to open the file, so NTFS requires that they
        ///  be unique within a volume. The Microsoft Distributed
        ///  Link Tracking Service uses the last 48 bytes of the
        ///  ExtendedInfo BLOB to store information that helps it
        ///  locate files that are moved to different volumes or
        ///  computers within a domain.  For more information, see
        ///  [MS-DLTW] section.
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    ///  The FSCTL_SET_ZERO_DATA request message requests that
    ///  the server fill the specified range of the file (associated
    ///  with the handle on which this FSCTL was invoked) with
    ///  zeros. The message contains a FILE_ZERO_DATA_INFORMATION
    ///  element. 				The FILE_ZERO_DATA_INFORMATION element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_41.xml
    //  </remarks>
    public partial struct FSCTL_SET_ZERO_DATA_Request
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file offset
        ///  of the start of the range to set to zeros in bytes.
        ///  The value of this field must be greater than or equal
        ///  to 0.
        /// </summary>
        public _LARGE_INTEGER FileOffset;

        /// <summary>
        ///  A 64-bit signed integer that contains the byte
        ///  offset of the first byte beyond the last zeroed byte.
        ///  The value of this field must be greater than or equal
        ///  to 0.
        /// </summary>
        public _LARGE_INTEGER BeyondFinalZero;
    }

    /// <summary>
    ///  The FSCTL_SIS_COPYFILE request message requests that
    ///  the server use the Single-Instance Storage (SIS) filter
    ///  to copy the file that is associated with the handle
    ///  on which this FSCTL was invoked. The message contains
    ///  an SI_COPYFILE data element. For more information about
    ///  Single-Instance Storage, see [SIS]. 				If the SIS
    ///  filter is installed on the server, it will attempt
    ///  to copy the source file to the destination file by
    ///  creating an SIS link instead of actually copying the
    ///  file data. If necessary and allowed, the source file
    ///  is placed under SIS control before the destination
    ///  file is created. 				The SI_COPYFILE data element is
    ///  as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_43.xml
    //  </remarks>
    public partial struct FSCTL_SIS_COPYFILE_Request
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the size, in
        ///  bytes, of the SourceFileName element, including a terminating-Unicode
        ///  NULL character.
        /// </summary>
        public uint SourceFileNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the size in
        ///  bytes of the DestinationFileName element, including
        ///  a terminating-Unicode NULL character.
        /// </summary>
        public uint DestinationFileNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains zero or more
        ///  of the following flag values. Flag values not specified
        ///  below SHOULD be set to 0, and MUST be ignored.
        /// </summary>
        public FSCTL_SIS_COPYFILE_Request_Flags_Values Flags;

        /// <summary>
        ///  A NULL-terminated Unicode string containing the source
        ///  file name.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] SourceFileName;

        /// <summary>
        ///  A NULL-terminated Unicode string containing the destination
        ///  file name. Both the source and destination file names
        ///  must represent paths on the same volume, and the file
        ///  names are the full paths to the files, including the
        ///  share or drive letter at which each file is located.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] DestinationFileName;
    }
    /// <summary>
    /// Enumeration that represents  FSCTL_SIS_COPYFILE_Request_Flags_Values 
    /// </summary>
    [Flags()]
    public enum FSCTL_SIS_COPYFILE_Request_Flags_Values : uint
    {

        /// <summary>
        ///  If this flag is set, only create the destination file
        ///  if the source file is already under SIS control. If
        ///  the source file is not under SIS control, the FSCTL
        ///  returns STATUS_OBJECT_TYPE_MISMATCH.If this flag is
        ///  not specified, place the source file under SIS control
        ///  (if it is not already under SIS control), and create
        ///  the destination file.
        /// </summary>
        COPYFILE_SIS_LINK = 0x00000001,

        /// <summary>
        ///  If this flag is set, create the destination file if
        ///  it does not exist; if it does exist, overwrite it.
        ///  							If this flag is not specified, create the destination
        ///  file if it does not exist; if it does exist, the FSCTL
        ///  returns STATUS_OBJECT_NAME_COLLISION.
        /// </summary>
        COPYFILE_SIS_REPLACE = 0x00000002,
    }

    /// <summary>
    ///  The FSCTL_FIND_FILES_BY_SID request message requests
    ///  that the server return a list of the files (in the
    ///  directory associated with the handle on which this
    ///  FSCTL was invoked) whose owner matches the specified
    ///  security identifier (SID). This message contains a
    ///  FIND_BY_SID_DATA data element. The FIND_BY_SID_DATA
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_3_9.xml
    //  </remarks>
    public partial struct FSCTL_FIND_FILES_BY_SID_Request
    {

        /// <summary>
        ///  A 32-bit unsigned integer value that indicates to restart
        ///  the search. This value MUST be 1 on first call so that
        ///  the search starts from the root. For subsequent calls,
        ///  this member SHOULD be zero so that the search resumes
        ///  at the point where it stopped.
        /// </summary>
        public uint Restart;

        /// <summary>
        ///  A SID (security identifier) (see [MS-SECO]) data element
        ///  that specifies the owner.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _SID[] SID;
    }

    /// <summary>
    ///  The buffer alignment required by the underlying device. The
    ///  FILE_ALIGNMENT_INFORMATION data element is as follows.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_1.xml
    //  </remarks>
    public partial struct FILE_ALIGNMENT_INFORMATION
    {

        /// <summary>
        ///  A 32-bit unsigned integer that MUST contain one
        ///  of the following values. 				
        /// </summary>
        public AlignmentRequirement_Values AlignmentRequirement;
    }

    /// <summary>
    /// Enumeration that represents  AlignmentRequirement_Values
    /// </summary>
    [Flags()]
    public enum AlignmentRequirement_Values : uint
    {

        /// <summary>
        ///  If this value is specified, there are no alignment requirements
        ///  for the device.
        /// </summary>
        FILE_BYTE_ALIGNMENT = 0x00000000,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 2-byte boundary.
        /// </summary>
        FILE_WORD_ALIGNMENT = 0x00000001,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 4-byte boundary.
        /// </summary>
        FILE_LONG_ALIGNMENT = 0x00000003,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  an 8-byte boundary.
        /// </summary>
        FILE_QUAD_ALIGNMENT = 0x00000007,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 16-byte boundary.
        /// </summary>
        FILE_OCTA_ALIGNMENT = 0x0000000f,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 32-byte boundary.
        /// </summary>
        FILE_32_BYTE_ALIGNMENT = 0x0000001f,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 64-byte boundary.
        /// </summary>
        FILE_64_BYTE_ALIGNMENT = 0x0000003f,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 128-byte boundary.
        /// </summary>
        FILE_128_BYTE_ALIGNMENT = 0x0000007f,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 256-byte boundary.
        /// </summary>
        FILE_256_BYTE_ALIGNMENT = 0x000000ff,

        /// <summary>
        ///  If this value is specified, data MUST be aligned on
        ///  a 512-byte boundary.
        /// </summary>
        FILE_512_BYTE_ALIGNMENT = 0x000001ff,
    }

    /// <summary>
    ///  This information class is used to query for the size
    ///  of the extended attributes (EA) for a file. An extended
    ///  attribute is a piece of application-specific metadata
    ///  that an application can associate with a file that
    ///  is not part of the file's data. For more information
    ///  about extended attributes, see [CIFS]. 				The FILE_EA_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_10.xml
    //  </remarks>
    public partial struct FileEaInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the combined
        ///  length, in bytes, of the extended attributes (EA) for
        ///  the file.
        /// </summary>
        public uint EaSize;
    }

    /// <summary>
    ///  This information class is used to set end-of-file information
    ///  for a file. 				The FILE_END_OF_FILE_INFORMATION data
    ///  element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_11.xml
    //  </remarks>
    public partial struct FileEndOfFileInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the absolute
        ///  new end of file position as a byte offset from the
        ///  start of the file. EndOfFile specifies the offset from
        ///  the beginning of the file of the byte following the
        ///  last byte in the file. That is, it is the offset from
        ///  the beginning of the file at which new bytes appended
        ///  to the file will be written. The value of this field
        ///  MUST be greater than or equal to 0.
        /// </summary>
        public long EndOfFile;
    }

    /// <summary>
    ///  This information class is used to query detailed information
    ///  for the files in a directory. 				The FILE_FULL_DIR_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_12.xml
    //  </remarks>
    public partial struct FileFullDirectoryInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_FULL_DIR_INFORMATION entry is located, if
        ///  multiple entries are present in a buffer. This member
        ///  is zero if no other entries follow this one. An implementation
        ///  MUST use this value to determine the location of the
        ///  next entry (if multiple entries are present in a buffer),
        ///  and MUST NOT assume that the value of NextEntryOffset
        ///  is the same as the size of the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset of the file within the parent directory. For
        ///  file systems such as NTFS, in which the position of
        ///  a file within the parent directory is not fixed and
        ///  can be changed at any time to maintain sort order,
        ///  this field SHOULD be set to 0, and MUST be ignored. When
        ///  using NTFS, the position of a file within the parent
        ///  directory is not fixed and can be changed at any time.
        ///  , , , and set this value to 0 for files on NTFS file
        ///  systems.
        /// </summary>
        public uint FileIndex;

        /// <summary>
        ///  A 64-bit signed integer that contains the time
        ///  when the file was created in the format of a FILETIME
        ///  structure. This value MUST be greater than or equal
        ///  to 0. 				
        /// </summary>
        public _FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was accessed in the format of a FILETIME
        ///  structure. This value MUST be greater than or equal
        ///  to 0.
        /// </summary>
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time information was written to the file in the format
        ///  of a FILETIME structure. This value MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME LastWriteTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was changed in the format of a FILETIME
        ///  structure. This value MUST be greater than or equal
        ///  to 0.
        /// </summary>
        public _FILETIME ChangeTime;

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
        public long EndOfFile;

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  allocation size, in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long AllocationSize;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file
        ///  attributes. For a list of valid file attributes, see
        ///  section.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the combined
        ///  length, in bytes, of the extended attributes (EA) for
        ///  the file.
        /// </summary>
        public uint EaSize;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    ///  This information class is used to query or set extended
    ///  attribute (EA) information for a file. 				The FILE_FULL_EA_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_13.xml
    //  </remarks>
    public partial struct FileFullEaInformation
    {

        /// <summary>
        ///  A 32-bit unsigned 4-byte aligned integer that
        ///  contains the byte offset from the beginning of this
        ///  entry, at which the next FILE_ FULL_EA _INFORMATION
        ///  entry is located, if multiple entries are present in
        ///  the buffer. This member MUST be zero if no other entries
        ///  follow this one. An implementation MUST use this value
        ///  to determine the location of the next entry (if multiple
        ///  entries are present in a buffer), and MUST NOT assume
        ///  that the value of NextEntryOffset is the same as the
        ///  size of the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  An 8-bit unsigned integer that contains one of
        ///  the following flag values.
        /// </summary>
        public FileFullEaInformation_Flags_Values Flags;

        /// <summary>
        ///  An 8-bit unsigned integer that contains the length,
        ///  in bytes, of the extended attribute name in the EaName
        ///  field. This value does not include the null-terminator
        ///  to EaName.
        /// </summary>
        public byte EaNameLength;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the length,
        ///  in bytes, of the extended attribute value in the EaName
        ///  field.
        /// </summary>
        public ushort EaValueLength;

        /// <summary>
        ///  An array of 8-bit ASCII characters that contain
        ///  the extended attribute name followed by a single null-termination
        ///  character byte and the associated value.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] EaName;
    }
    /// <summary>
    /// Enumeration that represents the Full file Information flags
    /// </summary>
    [Flags()]
    public enum FileFullEaInformation_Flags_Values : byte
    {

        /// <summary>
        ///  The file does not use EAs.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  If this flag is set, the file to which the EA belongs
        ///  cannot be interpreted without understanding the associated
        ///  extended attributes.
        /// </summary>
        FILE_NEED_EA = 0x00000080,
    }

    /// <summary>
    ///  This information class is used to query file reference
    ///  number information for the files in a directory. 				The
    ///  FILE_ID_BOTH_DIR_INFORMATION data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_14.xml
    //  </remarks>
    public partial struct FileIdBothDirectoryInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_ID_BOTH_DIR_INFORMATION entry is located,
        ///  if multiple entries are present in the buffer. This
        ///  member MUST be zero if no other entries follow this
        ///  one. An implementation MUST use this value to determine
        ///  the location of the next entry (if multiple entries
        ///  are present in a buffer), and MUST NOT assume that
        ///  the value of NextEntryOffset is the same as the size
        ///  of the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset of the file within the parent directory. For
        ///  file systems in which the position of a file within
        ///  the parent directory is not fixed and can be changed
        ///  at any time to maintain sort order, this field SHOULD
        ///  be set to 0, and MUST be ignored. When using NTFS, the
        ///  position of a file within the parent directory is not
        ///  fixed and can be changed at any time and set
        ///  this value to 0 for files on NTFS file systems.
        /// </summary>
        public uint FileIndex;

        /// <summary>
        ///  A 64-bit signed integer that contains the time
        ///  when the file was created. All dates and times are
        ///  in absolute system-time format, which is represented
        ///  as a FILETIME structure. The value of this field MUST
        ///  be greater than or equal to 0.
        /// </summary>
        public _FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was accessed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time information was written to the file in the format
        ///  of a FILETIME structure. The value of this field MUST
        ///  be greater than or equal to 0.
        /// </summary>
        public _FILETIME LastWriteTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was changed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME ChangeTime;

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
        public long EndOfFile;

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  allocation size in bytes. Usually, this value is a
        ///  multiple of the sector or cluster  size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long AllocationSize;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file
        ///  attributes. Valid attributes are as specified in section.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the combined
        ///  length, in bytes, of the extended attributes (EA) for
        ///  the file.
        /// </summary>
        public uint EaSize;

        /// <summary>
        ///  An 8-bit value containing the length, in bytes,
        ///  of the ShortName string.
        /// </summary>
        public byte ShortNameLength;

        /// <summary>
        ///  MUST be ignored by the receiver.
        /// </summary>
        public byte Reserved1;

        /// <summary>
        ///  A NULL-terminated 12-character Unicode string
        ///  containing the short (8.3) file name.
        /// </summary>
        [StaticSize(12, StaticSizeMode.Elements)]
        public ushort[] ShortName;

        /// <summary>
        ///  MUST be ignored by the receiver.
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        ///  An 8-byte file reference number for the file.
        ///  This number SHOULD be generated and assigned to the
        ///  file by the file system. Note that the FileId is not
        ///  the same as the 16-byte file object ID that was added
        ///  to NTFS for . For file systems which do not support
        ///  FileId, this field MUST be set to 0, and MUST be ignored.
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] FileId;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    ///  This information class is used to query detailed information
    ///  for the files in a directory. 				
	///  The FILE_ID_FULL_DIR_INFORMATION data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_15.xml
    //  </remarks>
    public partial struct FileIdFullDirectoryInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_ID_FULL_DIR_INFORMATION entry is located,
        ///  if multiple entries are present in a buffer. This member
        ///  MUST be zero if no other entries follow this one. An
        ///  implementation MUST use this value to determine the
        ///  location of the next entry (if multiple entries are
        ///  present in a buffer), and MUST NOT assume that the
        ///  value of NextEntryOffset is the same as the size of
        ///  the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset of the file within the parent directory. For
        ///  file systems in which the position of a file within
        ///  the parent directory is not fixed and can be changed
        ///  at any time to maintain sort order, this field SHOULD
        ///  be set to 0 and MUST be ignored. When using NTFS, the
        ///  position of a file within the parent directory is not
        ///  fixed and can be changed at any time. , , , and  set
        ///  this value to 0 for files on NTFS file systems.
        /// </summary>
        public uint FileIndex;

        /// <summary>
        ///  A 64-bit signed integer that contains the time
        ///  when the file was created in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was accessed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time information was written to the file in the format
        ///  of a FILETIME structure. The value of this field MUST
        ///  be greater than or equal to 0.
        /// </summary>
        public _FILETIME LastWriteTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was changed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME ChangeTime;

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
        public long EndOfFile;

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  allocation size in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long AllocationSize;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file
        ///  attributes. Valid attributes are as specified in section.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the combined
        ///  length, in bytes, of the extended attributes (EA) for
        ///  the file.
        /// </summary>
        public uint EaSize;

        /// <summary>
        ///  Reserved for alignment.
        /// </summary>
        public uint Reserved;

        /// <summary>
        ///  An 8-byte file reference number for the file.
        ///  This number is generated and assigned to the file by
        ///  the file system. Note that the FileId is not the same
        ///  as the 16-byte file object ID that was added to NTFS
        ///  for . For file systems which do not support FileId,
        ///   this field MUST be set to 0, and MUST be ignored.
        /// </summary>
        public _LARGE_INTEGER FileId;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    ///  This information class is used to query for the file
    ///  system's 8-byte file reference number for a file. 				The
    ///  FILE_INTERNAL_INFORMATION data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_16.xml
    //  </remarks>
    public partial struct FileInternalInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the 8-byte
        ///  file reference number for the file. This number MUST
        ///  be assigned by the file system and is unique to the
        ///  volume on which the file or directory is located. This
        ///  file reference number is the same as the file reference
        ///  number that is stored in the FileId field of the FILE_ID_BOTH_DIR_INFORMATION
        ///  and FILE_ID_FULL_DIR_INFORMATION data elements. The
        ///  value of this field MUST be greater than or equal to
        ///  0. For file systems which do not support a file reference
        ///  number, this field MUST be set to 0, and MUST be ignored.
        /// </summary>
        public long IndexNumber;
    }

    /// <summary>
    ///  This information class is used to create an NTFS hard
    ///  link to an existing file. 				The FILE_LINK_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_17.xml
    //  </remarks>
    public partial struct FileLinkInformation
    {

        /// <summary>
        ///  An 8-bit field that is set to 1 to indicate that
        ///  if the link already exists, it should be replaced with
        ///  the new link. MUST be set to 0 if the caller wants
        ///  the link creation operation to fail if the link already
        ///  exists.
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        ///  Reserved for alignment.
        /// </summary>
        [StaticSize(7, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        ///  A 64-bit unsigned integer that contains the file
        ///  handle for the directory where the link is to be created.
        ///  For network operations, this value MUST be zero.
        /// </summary>
        public RootDirectory_Values RootDirectory;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  name to be assigned to the newly created link. This
        ///  field might not be NULL-terminated, and MUST be handled
        ///  as a sequence of FileNameLength bytes. If the RootDirectory
        ///  member is NULL, and the link is to be created in a
        ///  different directory from the file that is being linked
        ///  to, this member specifies the full path name for the
        ///  link to be created. Otherwise, it specifies only the
        ///  file name.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    /// Enumeration that represents Root Directory values.
    /// </summary>
    public enum RootDirectory_Values : ulong
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  This information class is used to query information
    ///  on a mailslot. 				The FILE_MAILSLOT_QUERY_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_18.xml
    //  </remarks>
    public partial struct FileMailslotQueryInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the maximum
        ///  size of a single message that can be written to the
        ///  mailslot in bytes. To specify that the message can
        ///  be of any size, set this value to zero.
        /// </summary>
        public uint MaximumMessageSize;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the quota
        ///  in bytes for the mailslot. The mailslot quota specifies
        ///  the in-memory pool quota that is reserved for writes
        ///  to this mailslot.
        /// </summary>
        public uint MailslotQuota;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the next
        ///  message size in bytes.
        /// </summary>
        public uint NextMessageSize;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the total
        ///  number of messages waiting to be read from the mailslot.
        /// </summary>
        public uint MessagesAvailable;

        /// <summary>
        ///  A 64-bit signed integer that contains the time
        ///  a read operation can wait for a message to be written
        ///  to the mailslot before a time-out occurs in milliseconds.
        ///  The value of this field MUST be (-1) or greater than
        ///  or equal to 0. A value of (-1) requests that the read
        ///  wait forever for a message, without timing out. A value
        ///  of 0 requests that the read not wait and return immediately
        ///  whether a pending message is available to be read or
        ///  not.
        /// </summary>
        public long ReadTimeout;
    }

    /// <summary>
    ///  This information class is used to set information on
    ///  a mailslot. 				The FILE_MAILSLOT_SET_INFORMATION data
    ///  element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_19.xml
    //  </remarks>
    public partial struct FileMailslotSetInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the time
        ///  a read operation can wait for a message to be written
        ///  to the mailslot before a time-out occurs in milliseconds.
        ///  The value of this field MUST be (-1) or greater than
        ///  or equal to 0. A value of (-1) requests that the read
        ///  wait forever for a message without timing out. A value
        ///  of 0 requests that the read not wait and return immediately
        ///  whether a pending message is available to be read or
        ///  not.
        /// </summary>
        public long ReadTimeout;
    }

    /// <summary>
    ///  This information class is used to query detailed information
    ///  on the names of files in a directory. 				The FILE_NAMES_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_20.xml
    //  </remarks>
    public partial struct FileNamesInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_ NAMES _INFORMATION entry is located, if
        ///  multiple entries are present in a buffer. This member
        ///  MUST be zero if no other entries follow this one. An
        ///  implementation MUST use this value to determine the
        ///  location of the next entry (if multiple entries are
        ///  present in a buffer), and MUST NOT assume that the
        ///  value of NextEntryOffset is the same as the size of
        ///  the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset of the file within the parent directory. For
        ///  file systems in which the position of a file within
        ///  the parent directory is not fixed and can be changed
        ///  at any time to maintain sort order, this field SHOULD
        ///  be set to 0, and MUST be ignored. 					 When using
        ///  NTFS, the position of a file within the parent directory
        ///  is not fixed and can be changed at any time. , , ,
        ///  and set this value to 0 for files on NTFS file systems.
        /// </summary>
        public uint FileIndex;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
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
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_21.xml
    //  </remarks>
    public partial struct FileNetworkOpenInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the time
        ///  when the file was created in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was accessed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time information was written to the file in the format
        ///  of a FILETIME structure. The value of this field MUST
        ///  be greater than or equal to 0.
        /// </summary>
        public _FILETIME LastWriteTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was changed in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public _FILETIME ChangeTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  allocation size in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
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
        public long EndOfFile;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file
        ///  attributes. Valid attributes are as specified in section
        ///  .
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit field. This field is reserved. This field
        ///  MAY be set to any value, and MUST be ignored.
        /// </summary>
        public uint Reserved;
    }

    /// <summary>
    ///  The first type of data that may be returned.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_22_1.xml
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
        public System.Guid ObjectId;

        /// <summary>
        ///  A 16-byte GUID that uniquely identifies the volume
        ///  on which the object resided when the object identifier
        ///  was created, or zero if the volume had no object identifier
        ///  at that time. After copy operations, move operations,
        ///  or other file operations, this may not be the same
        ///  as the object identifier of the volume on which the
        ///  object presently resides.
        /// </summary>
        public System.Guid BirthVolumeId;

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
        public System.Guid BirthObjectId;

        /// <summary>
        ///  A 16-byte GUID value containing the domain identifier.
        ///  This value is unused; it SHOULD be zero, and MUST be
        ///  ignored.
        /// </summary>
        public System.Guid DomainId;
    }

    /// <summary>
    ///  The second type of data that may be returned.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_22_2.xml
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
        public System.Guid ObjectId;

        /// <summary>
        ///  A 48-byte blob that contains application-specific
        ///  extended information on the file object. If no extended
        ///  information has been written for this file, the server
        ///  MUST return 48 bytes of 0x00 in this field.
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    ///  This information class is used to query or set information
    ///  on a named pipe that is not specific to one end of
    ///  the pipe or another. 				The FILE_PIPE_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_23.xml
    //  </remarks>
    public partial struct FilePipeInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that MUST contain one
        ///  of the following values.
        /// </summary>
        public ReadMode_Values ReadMode;

        /// <summary>
        ///  A 32-bit unsigned integer that MUST contain one
        ///  of the following values.
        /// </summary>
        public CompletionMode_Values CompletionMode;
    }
    /// <summary>
    /// Enumeration that represents the ReadMode values.
    /// </summary>
    [Flags()]
    public enum ReadMode_Values : uint
    {

        /// <summary>
        ///  If this value is specified, data MUST be read from the
        ///  pipe as a stream of bytes.
        /// </summary>
        FILE_PIPE_BYTE_STREAM_MODE = 0x00000000,

        /// <summary>
        ///  If this value is specified, data MUST be read from the
        ///  pipe as a stream of messages.
        /// </summary>
        FILE_PIPE_MESSAGE_MODE = 0x00000001,
    }
    /// <summary>
    /// Enumeration that represents Completion mode values.
    /// </summary>
    [Flags()]
    public enum CompletionMode_Values : uint
    {

        /// <summary>
        ///  If this value is specified, blocking mode MUST be enabled.
        ///  When the pipe is being connected, read to, or written
        ///  from, the operation is not completed until there is
        ///  data to read, all data is written, or a client is connected.
        ///  Use of this mode can mean waiting indefinitely in some
        ///  situations for a client process to perform an action.
        /// </summary>
        FILE_PIPE_QUEUE_OPERATION = 0x00000000,

        /// <summary>
        ///  If this value is specified, non-blocking mode MUST be
        ///  enabled. When the pipe is being connected, read to,
        ///  or written from, the operation completes immediately.
        /// </summary>
        FILE_PIPE_COMPLETE_OPERATION = 0x00000001,
    }

    /// <summary>
    ///  This information class is used to query information
    ///  on a named pipe that is associated with the end of
    ///  the pipe that is being queried. 				The FILE_PIPE_LOCAL_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_24.xml
    //  </remarks>
    public partial struct FilePipeLocalInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the named
        ///  pipe type. MUST be one of the following. 				
        /// </summary>
        public NamedPipeType_Values NamedPipeType;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the named
        ///  pipe configuration. MUST be one of the following. 
        ///  				
        /// </summary>
        public NamedPipeConfiguration_Values NamedPipeConfiguration;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the maximum
        ///  number of instances that can be created for this pipe.
        ///  The first instance of the pipe MUST specify this value.
        /// </summary>
        public uint MaximumInstances;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number
        ///  of current named pipe instances.
        /// </summary>
        public uint CurrentInstances;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the inbound
        ///  quota in bytes for the named pipe.
        /// </summary>
        public uint InboundQuota;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the bytes
        ///  of data available to be read from the named pipe.
        /// </summary>
        public uint ReadDataAvailable;

        /// <summary>
        ///  A 32-bit unsigned integer that contains outbound
        ///  quota in bytes for the named pipe.
        /// </summary>
        public uint OutboundQuota;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the write
        ///  quota in bytes for the named pipe.
        /// </summary>
        public uint WriteQuotaAvailable;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the named
        ///  pipe state that specifies the connection status for
        ///  the named pipe. MUST be one of the following:
        /// </summary>
        public NamedPipeState_Values NamedPipeState;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the type
        ///  of the named pipe end, which specifies whether this
        ///  is the client or the server side of a named pipe. MUST
        ///  be one of the following. 				
        /// </summary>
        public NamedPipeEnd_Values NamedPipeEnd;
    }

    /// <summary>
    /// Enumeration that represents the NamedPipe type values.
    /// </summary>
    [Flags()]
    public enum NamedPipeType_Values : uint
    {

        /// <summary>
        ///  If this value is specified, data MUST be read from the
        ///  pipe as a stream of bytes.
        /// </summary>
        FILE_PIPE_BYTE_STREAM_TYPE = 0x00000000,

        /// <summary>
        ///  If this flag is specified, data MUST be read from the
        ///  pipe as a stream of messages.
        /// </summary>
        FILE_PIPE_MESSAGE_TYPE = 0x00000001,
    }

    /// <summary>
    /// Enumeration that represents the NamedPipe configuration values.
    /// </summary>
    [Flags()]
    public enum NamedPipeConfiguration_Values : uint
    {

        /// <summary>
        ///  If this value is specified, the flow of data in the
        ///  pipe goes from client to server only.
        /// </summary>
        FILE_PIPE_INBOUND = 0x00000000,

        /// <summary>
        ///  If this value is specified, the flow of data in the
        ///  pipe goes from server to client only.
        /// </summary>
        FILE_PIPE_OUTBOUND = 0x00000001,

        /// <summary>
        ///  If this value is specified, the pipe is bi-directional;
        ///  both server and client processes can read from and
        ///  write to the pipe.
        /// </summary>
        FILE_PIPE_FULL_DUPLEX = 0x00000002,
    }

    /// <summary>
    /// Enumeration that represents the NamedPipeState Values.
    /// </summary>
    [Flags()]
    public enum NamedPipeState_Values : uint
    {

        /// <summary>
        ///  Named pipe is disconnected.
        /// </summary>
        FILE_PIPE_DISCONNECTED_STATE = 0x00000001,

        /// <summary>
        ///  Named pipe is waiting to establish a connection.
        /// </summary>
        FILE_PIPE_LISTENING_STATE = 0x00000002,

        /// <summary>
        ///  Named pipe is connected.
        /// </summary>
        FILE_PIPE_CONNECTED_STATE = 0x00000003,

        /// <summary>
        ///  Named pipe is in the process of being closed.
        /// </summary>
        FILE_PIPE_CLOSING_STATE = 0x00000004,
    }

    /// <summary>
    /// Enumeration that NamedPipeEnd Values.
    /// </summary>
    [Flags()]
    public enum NamedPipeEnd_Values : uint
    {

        /// <summary>
        ///  This is the client end of a named pipe.
        /// </summary>
        FILE_PIPE_CLIENT_END = 0x00000000,

        /// <summary>
        ///  This is the server end of a named pipe.
        /// </summary>
        FILE_PIPE_SERVER_END = 0x00000001,
    }

    /// <summary>
    ///  The information class is used to query or set quota
    ///  information. 				The FILE_QUOTA_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_25.xml
    //  </remarks>
    public partial struct FileQuotaInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_QUOTA_INFORMATION entry is located, if multiple
        ///  entries are present in a buffer. This member MUST be
        ///  zero if no other entries follow this one. An implementation
        ///  MUST use this value to determine the location of the
        ///  next entry (if multiple entries are present in a buffer),
        ///  and MUST NOT assume that the value of NextEntryOffset
        ///  is the same as the size of the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the SID data element.
        /// </summary>
        public uint SidLength;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the quota was changed in the format of a FILETIME
        ///  structure. This value MUST be greater than or equal
        ///  to 0.
        /// </summary>
        public _FILETIME ChangeTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the amount
        ///  of quota used by this user in bytes. This value MUST
        ///  be greater than or equal to 0.
        /// </summary>
        public long QuotaUsed;

        /// <summary>
        ///  A 64-bit signed integer that contains the disk
        ///  quota warning threshold in bytes on this volume for
        ///  this user. This field MUST be set to a 64-bit integer
        ///  value greater than or equal to 0 to set a quota warning
        ///  threshold for this user on this volume, or to (-1)
        ///  to specify that no quota warning threshold is set for
        ///  this user.
        /// </summary>
        public long QuotaThreshold;

        /// <summary>
        ///  A 64-bit signed integer that contains the disk
        ///  quota limit in bytes on this volume for this user.
        ///  This field MUST be set to a 64-bit integer value greater
        ///  than or equal to 0 to set a disk quota limit for this
        ///  user on this volume, or to (-1) to specify that no
        ///  quota limit is set for this user.
        /// </summary>
        public long QuotaLimit;

        /// <summary>
        ///  Security identifier (SID) for this user.
        /// </summary>
        public _SID Sid;
    }

    /// <summary>
    ///  This information class is used to rename a file from
    ///  within the SMB protocol, as specified in [MS-SMB].
    ///  				The FILE_RENAME_INFORMATION data element is as
    ///  follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_26_1.xml
    //  </remarks>
    public partial struct FileRenameInformation_SMB
    {

        /// <summary>
        ///   					 MUST be an 8-bit field that is set to 1 to indicate
        ///  that if a file with the given name already exists,
        ///  it SHOULD be replaced with the given file. If set to
        ///  0, the rename operation MUST fail if a file with the
        ///  given name already exists.
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        ///  Reserved area for alignment.
        /// </summary>
        [StaticSize(7, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file
        ///  handle for the root directory. For network operations,
        ///  this value MUST always be zero.
        /// </summary>
        public FileRenameInformation_SMB_RootDirectory_Values RootDirectory;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length
        ///  in bytes of the new name for the file, including the
        ///  trailing NULL if present.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field MAY NOT be NULL-terminated, and
        ///  MUST be handled as a sequence of FileNameLength bytes,
        ///  not as a NULL-terminated string. If the RootDirectory
        ///  member is NULL, and the file is being moved to a different
        ///  directory, this member MUST specify the full path name
        ///  to be assigned to the file. Otherwise, it MUST specify
        ///  only the file name or a relative path name.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    /// Enumeration that represents FileRenameInformation SMB Root Directory values.
    /// </summary>
    public enum FileRenameInformation_SMB_RootDirectory_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  This information class is used to rename a file from
    ///  within the SMB2 protocol [MS-SMB2]. 				The FILE_RENAME_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_26_2.xml
    //  </remarks>
    public partial struct FileRenameInformation_SMB2
    {

        /// <summary>
        ///   					 MUST be an 8-bit field that is set to 1 to indicate
        ///  that if a file with the given name already exists,
        ///  it SHOULD be replaced with the given file. If set to
        ///  0, the rename operation MUST fail if a file with the
        ///  given name already exists.
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        ///  Reserved area for alignment.
        /// </summary>
        [StaticSize(7, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        ///  A 64-bit unsigned integer that contains the file
        ///  handle for the root directory. For network operations,
        ///  this value MUST always be zero.
        /// </summary>
        public FileRenameInformation_SMB2_RootDirectory_Values RootDirectory;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length
        ///  in bytes of the new name for the file, including the
        ///  trailing NULL if present.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field MAY NOT be NULL-terminated, and
        ///  MUST be handled as a sequence of FileNameLength bytes,
        ///  not as a NULL-terminated string. If the RootDirectory
        ///  member is NULL, and the file is being moved to a different
        ///  directory, this member MUST specify the full path name
        ///  to be assigned to the file. Otherwise, it MUST specify
        ///  only the file name or a relative path name.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }
    /// <summary>
    /// Enumeration that represents the FileRenameInformation_SMB2_RootDirectory_Values
    /// </summary>
    public enum FileRenameInformation_SMB2_RootDirectory_Values : ulong
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  This information class is used to query for information
    ///  on a reparse point. 				The FILE_REPARSE_POINT_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_27.xml
    //  </remarks>
    public partial struct FileReparsePointInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  reference number for the file. NTFS generates this
        ///  number and assigns it to the file automatically when
        ///  the file is created. The value of this field MUST be
        ///  greater than or equal to 0.
        /// </summary>
        public long FileReferenceNumber;

        /// <summary>
        ///  A  					32-bit unsigned integer value containing the
        ///  reparse point tag that uniquely identifies the owner
        ///  of the reparse point.
        /// </summary>
        public uint Tag;
    }

    /// <summary>
    ///  This information class is used to query or change the
    ///  file's short name. 				A caller changing the file's
    ///  short name MUST have SeBackupPrivilege.  SeBackupPrivilege
    ///  is specified in the Windows Behavior note of [MS-LSAD]
    ///  section 3.1.1.2.1.The FILE_NAME_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_28.xml
    //  </remarks>
    public partial struct FileShortNameInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field MUST NOT begin with a path separator
        ///  character (backslash). This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    ///  This information class is used to query or set file
    ///  information. 				The FILE_STANDARD_INFORMATION data
    ///  element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_29.xml
    //  </remarks>
    public partial struct FileStandardInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  allocation size in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
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
        public long EndOfFile;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number
        ///  of non-deleted links to this file.
        /// </summary>
        public uint NumberOfLinks;

        /// <summary>
        ///  An 8-bit field that MUST be set to 1 to indicate that
        ///  a file deletion has been requested; otherwise, 0.
        /// </summary>
        public byte DeletePending;

        /// <summary>
        ///  An 8-bit field that MUST be set to 1 to indicate that
        ///  the file is a directory; otherwise, 0.
        /// </summary>
        public byte Directory;

        /// <summary>
        ///  A 16-bit field. This field is reserved. This
        ///  field MAY be set to any value, and MUST be ignored.
        /// </summary>
        public byte Reserved;
    }

    /// <summary>
    ///  This information class is used to query alternate name
    ///  information for a file. The alternate name for a file
    ///  is its 8.3 format name (8 characters that appear before
    ///  the "." and 3 characters that appear after). This name
    ///  exists for compatibility with MS-DOS and 16-bit versions
    ///  of , which did not support longer file names or spaces
    ///  within file names. A file MAY have an alternate name
    ///  if its full name is not compliant with the restrictions
    ///  for file names under MS-DOS and 16-bit .NTFS assigns
    ///  an alternate name to a file whose full name is not
    ///  compliant with restrictions for file names under MS-DOS
    ///  and 16-bit  unless the system has been configured through
    ///  a registry entry to not generate these names to improve
    ///  performance.The FILE_NAME_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_3.xml
    //  </remarks>
    public partial struct FileAlternateNameInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length in
        ///  bytes of the FileName member.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the file
        ///  name. This field might not be NULL-terminated, and
        ///  MUST be handled as a sequence of FileNameLength bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    ///  This information class is used to enumerate the streams
    ///  for a file. 				The FILE_STREAM_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_30.xml
    //  </remarks>
    public partial struct FileStreamInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_ STREAM _INFORMATION entry is located, if
        ///  multiple entries are present in a buffer. This member
        ///  is zero if no other entries follow this one. An implementation
        ///  MUST use this value to determine the location of the
        ///  next entry (if multiple entries are present in a buffer),
        ///  and MUST NOT assume that the value of NextEntryOffset
        ///  is the same as the size of the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the stream name string.
        /// </summary>
        public uint StreamNameLength;

        /// <summary>
        ///  A 64-bit unsigned integer that contains the size,
        ///  in bytes, of the stream.
        /// </summary>
        public ulong StreamSize;

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  stream allocation size in bytes. Usually, this value
        ///  is a multiple of the sector or cluster size of the
        ///  underlying physical device. The value of this field
        ///  MUST be greater than or equal to 0.
        /// </summary>
        public long StreamAllocationSize;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  name of the stream. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of StreamNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] StreamName;
    }

    /// <summary>
    ///  This information class is used to set the valid data
    ///  length information for a file. 				The FILE_VALID_DATA_LENGTH_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_31.xml
    //  </remarks>
    public partial struct FileValidDataLengthInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the new
        ///  valid data length for the file. This parameter MUST
        ///  be a positive value that is greater than the current
        ///  valid data length, but less than or equal to the current
        ///  file size.The FILE_VALID_DATA_LENGTH_INFORMATION structure
        ///  is used to set a new valid data length for a file on
        ///  an NTFS volume. A file's valid data length is the length
        ///  in bytes of the data that has been written to the file.
        ///  This valid data extends from the beginning of the file
        ///  to the last byte in the file that has not been zeroed
        ///  or left uninitialized.
        /// </summary>
        public long FileNameLength;
    }

    /// <summary>
    ///  The information class is used to provide the list of
    ///  Security identifiers (SID) for which query quota information
    ///  is requested.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_32.xml
    //  </remarks>
    public partial struct FileGetQuotaInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte offset
        ///  from the beginning of this entry, at which the next
        ///  FILE_GET_QUOTA_INFORMATION entry is located, if multiple
        ///  entries are present in a buffer. This member MUST be
        ///  zero if no other entries follow this one. An implementation
        ///  MUST use this value to determine the location of the
        ///  next entry (if multiple entries are present in a buffer),
        ///  and MUST NOT assume that the value of NextEntryOffset
        ///  is the same as the size of the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the Sid data element.
        /// </summary>
        public uint SidLength;

        /// <summary>
        ///  SID for this user. SID(s) are sent in little endian format
        ///  and require no packing. The format of a SID is as specified
        ///  in [MS-DTYP] section 2.4.2.
        /// </summary>
        public _SID Sid;
    }

    /// <summary>
    ///  This information class is used to query for attribute
    ///  and reparse tag information for a file. 				The FILE_ATTRIBUTE_TAG_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_4.xml
    //  </remarks>
    public partial struct FileAttributeTagInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file attributes.
        ///  Valid file attributes are as specified in section.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the reparse
        ///  point tag. If the FileAttributes member includes the
        ///  FILE_ATTRIBUTE_REPARSE_POINT attribute flag, this member
        ///  specifies the reparse tag. Otherwise, this member SHOULD
        ///  be set to 0, and MUST be ignored.
        /// </summary>
        public uint ReparseTag;
    }

    /// <summary>
    ///  This information class is used to query or set file
    ///  information.The FILE_BASIC_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_5.xml
    //  </remarks>
    public partial struct FileBasicInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the time when
        ///  the file was created. All dates and times in this message
        ///  are in absolute system-time format, which is represented
        ///  as a FILETIME structure. This field should be set to
        ///  an integer value greater than or equal to 0; alternately,
        ///  it can be set to (-1) to indicate that this time field
        ///  should not be updated by the server.
        /// </summary>
        public _FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  the file was accessed in the format of a FILETIME structure.
        ///  This field should be set to an integer value greater
        ///  than or equal to 0; alternately, it can be set to (-1)
        ///  to indicate that this time field should not be updated
        ///  by the server.
        /// </summary>
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  information was written to the file in the format of
        ///  a FILETIME structure. This field should be set to an
        ///  integer value greater than or equal to 0; alternately,
        ///  it can be set to (-1) to indicate that this time field
        ///  should not be updated by the server.
        /// </summary>
        public _FILETIME LastWriteTime;

        /// <summary>
        ///   A 64-bit signed integer that contains the last time
        ///  the file was changed in the format of a FILETIME structure.
        ///  This field should be set to an integer value greater
        ///  than or equal to 0; alternately, it can be set to (-1)
        ///  to indicate that this time field should not be updated
        ///  by the server.
        /// </summary>
        public _FILETIME ChangeTime;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file attributes.
        ///  Valid file attributes are specified in section.The
        ///  file system updates the values of the LastAccessTime,
        ///  LastWriteTime, and ChangeTime members as appropriate
        ///  after an I/O operation is performed on a file. However,
        ///  a driver or application can request that the file system
        ///  not update one or more of these members for I/O operations
        ///  that are performed on the caller's file handle by setting
        ///  the appropriate members to -1. The caller can set one,
        ///  all, or any other combination of these three members
        ///  to -1. Only the members that are set to -1 will be
        ///  unaffected by I/O operations on the file handle; the
        ///  other members will be updated as appropriate.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit field. This field is reserved. This field
        ///  MAY be set to any value, and MUST be ignored.
        /// </summary>
        public uint Reserved;
    }

    /// <summary>
    ///  This information class is used to query detailed information
    ///  for the files in a directory. 				The FILE_BOTH_DIR_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_6.xml
    //  </remarks>
    public partial struct FileBothDirectoryInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte offset
        ///  from the beginning of this entry, at which the next
        ///  FILE_BOTH_DIR_INFORMATION entry is located, if multiple
        ///  entries are present in a buffer. This member is zero
        ///  if no other entries follow this one. An implementation
        ///  MUST use this value to determine the location of the
        ///  next entry (if multiple entries are present in a buffer),
        ///  and MUST NOT assume that the value of NextEntryOffset
        ///  is the same as the size of the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte offset
        ///  of the file within the parent directory. For file systems
        ///  in which the position of a file within the parent directory
        ///  is not fixed and can be changed at any time to maintain
        ///  sort order, this field SHOULD be set to 0, and MUST
        ///  be ignored. When using NTFS, the position of a file
        ///  within the parent directory is not fixed and can be
        ///  changed at any time. , , , and set this value to 0
        ///  for files on NTFS file systems.
        /// </summary>
        public uint FileIndex;

        /// <summary>
        ///   A 64-bit signed integer that contains the time when
        ///  the file was created. All dates and times are in absolute
        ///  system-time format, which is represented as a FILETIME
        ///  structure. This value MUST be greater than or equal
        ///  to 0.
        /// </summary>
        public _FILETIME CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  the file was accessed in the format of a FILETIME structure.
        ///  This value MUST be greater than or equal to 0.
        /// </summary>
        public _FILETIME LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  information was written to the file in the format of
        ///  a FILETIME structure. This value MUST be greater than
        ///  or equal to 0.
        /// </summary>
        public _FILETIME LastWriteTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  the file was changed in the format of a FILETIME structure.
        ///  This value MUST be greater than or equal to 0.
        /// </summary>
        public _FILETIME ChangeTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the absolute new
        ///  end-of-file position as a byte offset from the start
        ///  of the file. EndOfFile specifies the offset to the
        ///  byte immediately following the last valid byte in the
        ///  file. Because this value is zero-based, it actually
        ///  refers to the first free byte in the file. That is,
        ///  it is the offset from the beginning of the file at
        ///  which new bytes appended to the file will be written.
        ///  The value of this field MUST be greater than or equal
        ///  to 0. 				
        /// </summary>
        public long EndOfFile;

        /// <summary>
        ///  A 64-bit signed integer that contains the file allocation
        ///  size, in bytes. Usually, this value is a multiple of
        ///  the sector or cluster size of the underlying physical
        ///  device. The value of this field MUST be greater than
        ///  or equal to 0.
        /// </summary>
        public long AllocationSize;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file attributes.
        ///  Valid file attributes are specified in section.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///   A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field. The NULL termination
        ///  of the string, if present, is not included in the FileNameLength
        ///  count.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the combined
        ///  length, in bytes, of the extended attributes (EA) for
        ///  the file.
        /// </summary>
        public uint EaSize;

        /// <summary>
        ///  MUST be an 8-bit signed value containing the length,
        ///  in bytes, of the ShortName field. If there is a NULL
        ///  termination at the end of the string, it is not included
        ///  in the FileNameLength count. This value MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public byte ShortNameLength;

        /// <summary>
        ///  Reserved for alignment.
        /// </summary>
        public byte Reserved;

        /// <summary>
        ///  A 24-byte Unicode char field containing the short (8.3)
        ///  file name. The file name might be NULL-terminated.
        /// </summary>
        [StaticSize(12, StaticSizeMode.Elements)]
        public ushort[] ShortName;

        /// <summary>
        ///  A sequence of Unicode characters containing the file
        ///  name. This field might not be NULL-terminated, and
        ///  MUST be handled as a sequence of FileNameLength bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    ///  This information class is used to query compression
    ///  information for a file. 				The FILE_COMPRESSION_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_7.xml
    //  </remarks>
    public partial struct FileCompressionInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the size,
        ///  in bytes, of the compressed file. This value MUST be
        ///  greater than or equal to 0.
        /// </summary>
        public long CompressedFileSize;

        /// <summary>
        ///  A 16-bit unsigned integer that contains the compression
        ///  format. The actual compression operation associated
        ///  with each of these compression format values is implementation-dependent.
        ///  An implementation can associate any local compression
        ///  algorithm with the values described below because the
        ///  compressed data does not travel across the wire in
        ///  the context of FSCTL, FileInformation class, or FileSystemInformation
        ///  class requests or replies., , , and  implement only
        ///  one compression algorithm, LZNT1. For more information,
        ///  see [UASDC]. COMPRESSION_FORMAT_DEFAULT is therefore
        ///  equivalent to COMPRESSION_FORMAT_LZNT1.
        /// </summary>
        public CompressionFormat_Values CompressionFormat;

        /// <summary>
        ///  An 8-bit unsigned integer that contains the compression
        ///  unit shift, which is the number of bits by which to
        ///  left-shift a 1 bit to arrive at the compression unit
        ///  size. The compression unit size is the number of bytes
        ///  in a compression unit, that is, the number of bytes
        ///  to be compressed. This value is implementation-defined. NTFS
        ///  uses a value of 16 calculated as (4 + ClusterShift)
        ///  for the CompressionUnitShift by default. The ultimate
        ///  size of data to be compressed depends on the cluster
        ///  size set for the file system at initialization. NTFS
        ///  defaults to a 4-kilobyte cluster size, resulting in
        ///  a ClusterShift value of 12, but NTFS file systems can
        ///  be initialized with a different cluster size, so the
        ///  value may vary. The default compression unit size based
        ///  on this calculation is 64 kilobytes.
        /// </summary>
        public byte CompressionUnitShift;

        /// <summary>
        ///  An 8-bit unsigned integer that contains the compression
        ///  chunk size in bytes in log 2 format. The chunk size
        ///  is the number of bytes that the operating system's
        ///  implementation of the Lempel-Ziv compression algorithm
        ///  tries to compress at one time. This value is implementation-defined. NTFS
        ///  uses a value of 12 for the ChunkShift so that compression
        ///  chunks are 4 kilobytes in size.
        /// </summary>
        public byte ChunkShift;

        /// <summary>
        ///  An 8-bit unsigned integer that specifies, in
        ///  log 2 format, the amount of space that must be saved
        ///  by compression to successfully compress a compression
        ///  unit. If that amount of space is not saved by compression,
        ///  the data in that compression unit is stored uncompressed.
        ///  Each successfully compressed compression unit MUST
        ///  occupy at least one cluster that is less in bytes than
        ///  an uncompressed compression unit. Therefore, the cluster
        ///  shift is the number of bits by which to left shift
        ///  a 1 bit to arrive at the size of a cluster. This value
        ///  is implementation defined. The value of this field depends
        ///  on the cluster size set for the file system at initialization.
        ///  NTFS uses a value of 12 by default because the default
        ///  NTFS cluster size is 4-kilobyte bytes. If an NTFS file
        ///  system is initialized with a different cluster size,
        ///  the value of ClusterShift would be log 2 of the cluster
        ///  size for that file system.
        /// </summary>
        public byte ClusterShift;

        /// <summary>
        ///  A 24-bit reserved value. This field SHOULD be
        ///  set to 0, and MUST be ignored.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved;
    }

    /// <summary>
    /// Enumeration that represents the Compression format values.
    /// </summary>
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
    ///  This information class is used to query detailed information
    ///  for the files in a directory. 				The FILE_DIRECTORY_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_8.xml
    //  </remarks>
    public partial struct FileDirectoryInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset from the beginning of this entry, at which the
        ///  next FILE_DIRECTORY_INFORMATION entry is located, if
        ///  multiple entries are present in a buffer. This member
        ///  MUST be zero if no other entries follow this one. An
        ///  implementation MUST use this value to determine the
        ///  location of the next entry (if multiple entries are
        ///  present in a buffer), and MUST NOT assume that the
        ///  value of NextEntryOffset is the same as the size of
        ///  the current entry.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the byte
        ///  offset of the file within the parent directory. For
        ///  file systems in which the position of a file within
        ///  the parent directory is not fixed and can be changed
        ///  at any time to maintain sort order, this field SHOULD
        ///  be set to 0 and MUST be ignored. When using NTFS, the
        ///  position of a file within the parent directory is not
        ///  fixed and can be changed at any time. , , , and  set
        ///  this value to 0 for files on NTFS file systems.
        /// </summary>
        public uint FileIndex;

        /// <summary>
        ///  A 64-bit signed integer that contains the time
        ///  when the file was created. All dates and times are
        ///  in absolute system-time format, which is represented
        ///  as a FILETIME structure. This value MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was accessed in the format of a FILETIME
        ///  structure. This value MUST be greater than or equal
        ///  to 0.
        /// </summary>
        public long LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time information was written to the file in the format
        ///  of a FILETIME structure. This value MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long LastWriteTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last
        ///  time the file was changed in the format of a FILETIME
        ///  structure. This value MUST be greater than or equal
        ///  to 0.
        /// </summary>
        public long ChangeTime;

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
        public long EndOfFile;

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  allocation size, in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long AllocationSize;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file
        ///  attributes. Valid attributes are as specified in section
        ///  .
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] FileName;
    }

    /// <summary>
    ///  This information class is used to mark a file for deletion.
    ///  				The FILE_DISPOSITION_INFORMATION data element is
    ///  as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_9.xml
    //  </remarks>
    public partial struct FileDispositionInformation
    {

        /// <summary>
        ///  An 8-bit field that is set to 1 to indicate that
        ///  a file SHOULD be deleted when it is closed; otherwise,
        ///  0.A file marked for deletion is not actually deleted
        ///  until all open handles for the file object have been
        ///  closed, and the link count for the file is zero.
        /// </summary>
        public byte DeletePending;
    }

    /// <summary>
    ///  This information class is used to query detailed information
    ///  of a file. 				The FILE_NAME_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_4_filenameinformation.xml
    //  </remarks>
    public partial struct FileNameInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the FileName field.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A sequence of Unicode characters containing the
        ///  file name. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }
   

    /// <summary>
    ///  This information class is used to query attribute information
    ///  for a file system. The message contains a FILE_FS_ATTRIBUTE_INFORMATION
    ///  data element. 				The FILE_FS_ATTRIBUTE_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_1.xml
    //  </remarks>
    public partial struct FileFsAttributeInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains a bitmask of
        ///  flags that specify attributes of the specified file
        ///  system as a combination of the following flags. The
        ///  value of this field MUST be a bitwise OR of zero or
        ///  more of the following with the exception that FS_FILE_COMPRESSION
        ///  and FS_VOL_IS_COMPRESSED cannot both be set. Any flag
        ///  values not explicitly mentioned here MAY be set to
        ///  any value, and MUST be ignored.
        /// </summary>
        public FileSystemAttributes_Values FileSystemAttributes;

        /// <summary>
        ///  A 32-bit signed integer that contains the maximum file
        ///  name component length, in bytes, supported by the specified
        ///  file system. The value of this field MUST be greater
        ///  than 0.
        /// </summary>
        public int MaximumComponentNameLength;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, of the file system name in the FileSystemName
        ///  field. The value of this field MUST be greater than
        ///  0.
        /// </summary>
        public uint FileSystemNameLength;

        /// <summary>
        ///  A variable-length Unicode field containing the name
        ///  of the file system. This field might not be NULL-terminated,
        ///  and MUST be handled as a sequence of FileSystemNameLength
        ///  bytes.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileSystemName;
    }
    /// <summary>
    /// Enumeration that represents the FileSystemAttributes.
    /// </summary>
    [Flags()]
    public enum FileSystemAttributes_Values : uint
    {

        /// <summary>
        ///  The volume supports transactions. Set this flag
        ///  if the volume is formatted for NTFS 3.0 or higher.
        /// </summary>
        FILE_SUPPORTS_TRANSACTIONS = 0x00200000,

        /// <summary>
        ///  The volume supports a single sequential write, such
        ///  as for a tape-based file system.
        /// </summary>
        FILE_SEQUENTIAL_WRITE_ONCE = 0x00100000,

        /// <summary>
        ///  The specified volume is read-only. This attribute is
        ///  only available on , , and .
        /// </summary>
        FILE_READ_ONLY_VOLUME = 0x00080000,

        /// <summary>
        ///  The file system supports named streams.
        /// </summary>
        FILE_NAMED_STREAMS = 0x00040000,

        /// <summary>
        ///  The file system supports the Encrypted File System (EFS).
        ///  NTFS version 2 supports EFS. To use EFS, the operating
        ///  system must support NTFS version 2, and the file system
        ///  on disk must be formatted using NTFS version 2.NTFS
        ///  version 2 is supported on , , , and .
        /// </summary>
        FILE_SUPPORTS_ENCRYPTION = 0x00020000,

        /// <summary>
        ///  The file system supports object identifiers.
        /// </summary>
        FILE_SUPPORTS_OBJECT_IDS = 0x00010000,

        /// <summary>
        ///  The specified volume is a compressed volume. This flag
        ///  is incompatible with the FILE_FILE_COMPRESSION flag.
        /// </summary>
        FILE_VOLUME_IS_COMPRESSED = 0x00008000,

        /// <summary>
        ///  The file system supports remote storage. Remote storage
        ///  is provided by the Remote Storage service to create
        ///  virtual disk storage from a tape or other storage media.
        /// </summary>
        FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100,

        /// <summary>
        ///  The file system supports reparse points.
        /// </summary>
        FILE_SUPPORTS_REPARSE_POINTS = 0x00000080,

        /// <summary>
        ///  The file system supports sparse files.
        /// </summary>
        FILE_SUPPORTS_SPARSE_FILES = 0x00000040,

        /// <summary>
        ///  The file system supports per-user quotas.
        /// </summary>
        FILE_VOLUME_QUOTAS = 0x00000020,

        /// <summary>
        ///  The file volume supports file-based compression. This
        ///  flag is incompatible with the FILE_VOLUME_IS_COMPRESSED
        ///  flag.
        /// </summary>
        FILE_FILE_COMPRESSION = 0x00000010,

        /// <summary>
        ///  The file system preserves and enforces access control
        ///  lists (ACLs).
        /// </summary>
        FILE_PERSISTENT_ACLS = 0x00000008,

        /// <summary>
        ///  The file system supports Unicode in file and directory
        ///  names. This flag applies only to file and directory
        ///  names; the file system neither restricts nor interprets
        ///  the bytes of data within a file.
        /// </summary>
        FILE_UNICODE_ON_DISK = 0x00000004,

        /// <summary>
        ///  The file system preserves the case of file names when
        ///  it places a name on disk.
        /// </summary>
        FILE_CASE_PRESERVED_NAMES = 0x00000002,

        /// <summary>
        ///  The file system supports case-sensitive file names when
        ///  looking up (searching for) file names in a directory.
        /// </summary>
        FILE_CASE_SENSITIVE_SEARCH = 0x00000001,
    }

    /// <summary>
    ///  This information class is used to query or set quota
    ///  and content indexing control information for a file
    ///  system volume. The message contains a FILE_FS_CONTROL_INFORMATION
    ///  data element. Setting quota information requires the
    ///  caller to have permission to open a volume handle or
    ///  handle to the quota index file for write access. The
    ///  FILE_FS_CONTROL_INFORMATION data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_2.xml
    //  </remarks>
    public partial struct FileFsControlInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the minimum amount
        ///  of free disk space in bytes that is required for the
        ///  operating system's content-indexing service to begin
        ///  document filtering. This value SHOULD be set to 0,
        ///  and MUST be ignored. Sets this value to 0.
        /// </summary>
        public long FreeSpaceStartFiltering;

        /// <summary>
        ///  A 64-bit signed integer that contains the minimum amount
        ///  of free disk space in bytes that is required for the
        ///  indexing service to continue to filter documents and
        ///  merge word lists. This value SHOULD be set to 0, and
        ///  MUST be ignored. Sets this value to 0.
        /// </summary>
        public long FreeSpaceThreshold;

        /// <summary>
        ///  A 64-bit signed integer that contains the minimum amount
        ///  of free disk space in bytes that is required for the
        ///  content-indexing service to continue filtering. This
        ///  value SHOULD be set to 0, and MUST be ignored. sets
        ///  this value to 0.
        /// </summary>
        public long FreeSpaceStopFiltering;

        /// <summary>
        ///  A 64-bit signed integer that contains the default per-user
        ///  disk quota warning threshold in bytes for the volume.
        ///  This field MUST be set to a 64-bit integer value greater
        ///  than or equal to 0 to set a default quota warning threshold
        ///  per user for this volume, or to (-1) to specify that
        ///  no default quota warning threshold per user is set.
        /// </summary>
        public long DefaultQuotaThreshold;

        /// <summary>
        ///  A 64-bit signed integer that contains the default per-user
        ///  disk quota limit in bytes for the volume. This field
        ///  MUST be set to a 64-bit integer value greater than
        ///  or equal to 0 to set a default disk quota limit per
        ///  user for this volume, or to (-1) to specify that no
        ///  default quota limit per user is set.
        /// </summary>
        public long DefaultQuotaLimit;

        /// <summary>
        ///  A 32-bit unsigned integer that contains a bitmask of
        ///  flags that control quota enforcement and logging of
        ///  user-related quota events on the volume. The following
        ///  bit flags are valid in any combination. Bits not defined
        ///  below SHOULD be set to 0, and MUST be ignored. sets
        ///  flags not defined below to zero.Logging makes an entry
        ///  in the application event log.
        /// </summary>
        public FileSystemControlFlags_Values FileSystemControlFlags;
    }
    /// <summary>
    /// Enumeration that represents the File System Control flag values.
    /// </summary>
    [Flags()]
    public enum FileSystemControlFlags_Values : uint
    {

        /// <summary>
        ///  Content indexing is disabled.
        /// </summary>
        FILE_VC_CONTENT_INDEX_DISABLED = 0x00000008,

        /// <summary>
        ///  An event log entry will be created when the user exceeds
        ///  his or her assigned disk quota limit.
        /// </summary>
        FILE_VC_LOG_QUOTA_LIMIT = 0x00000020,

        /// <summary>
        ///  An event log entry will be created when the user exceeds
        ///  his or her assigned quota warning threshold.
        /// </summary>
        FILE_VC_LOG_QUOTA_THRESHOLD = 0x00000010,

        /// <summary>
        ///  An event log entry will be created when the volume's
        ///  free space limit is exceeded.
        /// </summary>
        FILE_VC_LOG_VOLUME_LIMIT = 0x00000080,

        /// <summary>
        ///  An event log entry will be created when the volume's
        ///  free space threshold is exceeded.
        /// </summary>
        FILE_VC_LOG_VOLUME_THRESHOLD = 0x00000040,

        /// <summary>
        ///  Quotas are tracked and enforced on the volume.
        /// </summary>
        FILE_VC_QUOTA_ENFORCE = 0x00000002,

        /// <summary>
        ///  Quotas are tracked on the volume, but they are not enforced.
        ///  Tracked quotas enable reporting on the file system
        ///  space used by system users. If both this field and
        ///  FILE_VC_QUOTA_ENFORCE are specified, FILE_VC_QUOTA_ENFORCE
        ///  is ignored.
        /// </summary>
        FILE_VC_QUOTA_TRACK = 0x00000001,

        /// <summary>
        ///  The quota information for the volume is incomplete because
        ///  it is corrupt, or the system is in the process of rebuilding
        ///  the quota information.
        /// </summary>
        FILE_VC_QUOTAS_INCOMPLETE = 0x00000100,

        /// <summary>
        ///  The file system is rebuilding the quota information
        ///  for the volume.
        /// </summary>
        FILE_VC_QUOTAS_REBUILDING = 0x00000200,
    }

    /// <summary>
    ///  This information class is used to query if a given driver
    ///  is in the I/O path for a file system volume. The message
    ///  contains a FILE_FS_DRIVER_PATH_INFORMATION data element.
    ///  				The FILE_FS_DRIVER_PATH_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_3.xml
    //  </remarks>
    public partial struct FileFsDriverPathInformation
    {

        /// <summary>
        ///  An unsigned character (Boolean) value that is TRUE if
        ///  the driver is in the I/O path for the file system volume;
        ///  and otherwise, FALSE.
        /// </summary>
        public byte DriverInPath;

        /// <summary>
        ///  Reserved for alignment.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] Reserved;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length of
        ///  the DriverName string.
        /// </summary>
        public uint DriverNameLength;

        /// <summary>
        ///  A variable-length Unicode field containing the name
        ///  of the driver for which to query. This sequence of
        ///  Unicode characters MUST NOT be NULL-terminated.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] DriverName;
    }

    /// <summary>
    ///  This information class is used to query sector size
    ///  information for a file system volume. The message contains
    ///  a FILE_FS_FULL_SIZE_INFORMATION data element. 				The
    ///  FILE_FS_FULL_SIZE_INFORMATION data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_4.xml
    //  </remarks>
    public partial struct FileFsFullSizeInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the total
        ///  number of allocation units on the volume that are available
        ///  to the user associated with the calling thread. The
        ///  value of this field MUST be greater than or equal to
        ///  0.In , , , and , if per-user quotas are in use, this
        ///  value may be less than the total number of allocation
        ///  units on the disk. Non-Microsoft quota management software
        ///  might display the same behavior as these versions of
        ///   if that software was implemented as a file system
        ///  filter driver, and the driver implementer opted to
        ///  set the FileFsFullSizeInformation in the same manner
        ///  as .
        /// </summary>
        public long TotalAllocationUnits;

        /// <summary>
        ///  A 64-bit signed integer that contains the total
        ///  number of free allocation units on the volume that
        ///  are available to the user associated with the calling
        ///  thread. The value of this field MUST be greater than
        ///  or equal to 0.In , , , and , if per-user quotas are
        ///  in use, this value may be less than the total number
        ///  of free allocation units on the disk.
        /// </summary>
        public long CallerAvailableAllocationUnits;

        /// <summary>
        ///  A 64-bit signed integer that contains the total
        ///  number of free allocation units on the volume. The
        ///  value of this field MUST be greater than or equal to
        ///  0.
        /// </summary>
        public long ActualAvailableAllocationUnits;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number
        ///  of sectors in each allocation unit.
        /// </summary>
        public uint SectorsPerAllocationUnit;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number
        ///  of bytes in each sector.
        /// </summary>
        public uint BytesPerSector;
    }

    /// <summary>
    ///  This information class is used to set the label for
    ///  a file system volume. The message contains a FILE_FS_LABEL_INFORMATION
    ///  data element. 				The FILE_FS_LABEL_INFORMATION data
    ///  element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_5.xml
    //  </remarks>
    public partial struct FileFsLabelInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, including the trailing NULL, if present,
        ///  of the name for the volume.
        /// </summary>
        public uint VolumeLabelLength;

        /// <summary>
        ///  A variable-length Unicode field containing the name
        ///  of the volume. The content of this field can be a NULL-terminated
        ///  string, or it can be a string padded with the space
        ///  character to be VolumeLabelLength bytes long.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] VolumeLabel;
    }

    /// <summary>
    ///  This information class is used to query or set the object
    ///  ID for a file system data element. The query MUST fail
    ///  if the file system does not support object IDs. The
    ///  Microsoft FAT File System does not support the use
    ///  of object IDs, and returns a status code of STATUS_INVALID_DEVICE_REQUEST.
    ///   The message contains a FILE_FS_OBJECTID_INFORMATION
    ///  data element. 				The FILE_FS_OBJECTID_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_6.xml
    //  </remarks>
    public partial struct FileFsObjectIdInformation
    {

        /// <summary>
        ///  A 16-byte GUID that identifies the file system volume
        ///  on the disk. This value is not required to be unique
        ///  on the system.
        /// </summary>
        public System.Guid ObjectId;

        /// <summary>
        ///  A 48-byte value containing extended information on the
        ///  file system volume. If no extended information has
        ///  been written for this file system volume, the server
        ///  MUST return 48 bytes of 0x00 in this field. It does not
        ///  write information into the ExtendedInfo field for file
        ///  systems.
        /// </summary>
        [StaticSize(48, StaticSizeMode.Elements)]
        public byte[] ExtendedInfo;
    }

    /// <summary>
    ///  This information class is used to query sector size
    ///  information for a file system volume. The message contains
    ///  a FILE_FS_SIZE_INFORMATION data element. 				The FILE_FS_SIZE_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_7.xml
    //  </remarks>
    public partial struct FileFsSizeInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the total number
        ///  of allocation units on the volume that are available
        ///  to the user associated with the calling thread. This
        ///  value MUST be greater than or equal to 0.In , , , and
        ///  , if per-user quotas are in use, this value may be
        ///  less than the total number of allocation units on the
        ///  disk. Non-Microsoft quota management software might
        ///  display the same behavior as  if that software was
        ///  implemented as a file system filter driver, and the
        ///  driver implementer opted to set the FileFsSizeInformation
        ///  in the same manner as .
        /// </summary>
        public long TotalAllocationUnits;

        /// <summary>
        ///  A 64-bit signed integer that contains the total
        ///  number of free allocation units on the volume that
        ///  are available to the user associated with the calling
        ///  thread. This value MUST be greater than or equal to
        ///  0.In , , , and , if per-user quotas are in use, this
        ///  value may be less than the total number of free allocation
        ///  units on the disk.
        /// </summary>
        public long ActualAvailableAllocationUnits;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number
        ///  of sectors in each allocation unit.
        /// </summary>
        public uint SectorsPerAllocationUnit;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number
        ///  of bytes in each sector.
        /// </summary>
        public uint BytesPerSector;
    }

    /// <summary>
    ///  This information class is used to query information
    ///  on a volume on which a file system is mounted. The
    ///  message contains a FILE_FS_VOLUME_INFORMATION data
    ///  element. 				The FILE_FS_VOLUME_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_8.xml
    //  </remarks>
    public partial struct FileFsVolumeInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the time when
        ///  the volume was created in the format of a FILETIME
        ///  structure. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long VolumeCreationTime;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the serial number
        ///  of the volume. The serial number is an opaque value
        ///  generated by the file system at format time, and is
        ///  not necessarily related to any hardware serial number
        ///  for the device on which the file system is located.
        ///  No specific format or content of this field is required
        ///  for protocol interoperation. This value is not required
        ///  to be unique.
        /// </summary>
        public uint VolumeSerialNumber;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, including the trailing NULL, if present,
        ///  of the name of the volume.
        /// </summary>
        public uint VolumeLabelLength;

        /// <summary>
        ///  A 1-byte Boolean (unsigned char) that is TRUE
        ///  (0x01) if the file system supports object-oriented
        ///  file system objects; otherwise, FALSE (0x00).This value
        ///  is TRUE for NTFS and FALSE for other file systems implemented
        ///  by .
        /// </summary>
        public SupportsObjects_Values SupportsObjects;

        /// <summary>
        ///  MUST be ignored by the receiver.
        /// </summary>
        public byte Reserved;

        /// <summary>
        ///   A variable-length Unicode field containing the name
        ///  of the volume. The content of this field can be a NULL-terminated
        ///  string or can be a string padded with the space character
        ///  to be VolumeLabelLength bytes long.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] VolumeLabel;
    }
    /// <summary>
    /// Enumeration that represents the Supports Object values.
    /// </summary>
    [Flags()]
    public enum SupportsObjects_Values : byte
    {

        /// <summary>
        ///  TRUE
        /// </summary>
        V1 = 0x01,

        /// <summary>
        ///  FALSE
        /// </summary>
        V2 = 0x00,
    }

    /// <summary>
    ///  This information class is used to query device information
    ///  associated with a file system volume. The message contains
    ///  a FILE_FS_DEVICE_INFORMATION data element.   The FILE_FS_DEVICE_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_5_9.xml
    //  </remarks>
    public partial struct FileFsDeviceInformation
    {

        /// <summary>
        ///  This identifies the type of given volume. It MUST be
        ///  one of the following:
        /// </summary>
        public DeviceType_Values DeviceType;

        /// <summary>
        ///  A bit field which identifies various characteristics
        ///  about a given volume. The following are valid bit values.
        /// </summary>
        public Characteristics_Values Characteristics;
    }
    /// <summary>
    /// Enumeration that represents the Device Types
    /// </summary>
    [Flags()]
    public enum DeviceType_Values : uint
    {

        /// <summary>
        ///  Volume resides on a CD ROM.
        /// </summary>
        FILE_DEVICE_CD_ROM = 0x00000002,

        /// <summary>
        ///  Volume resides on a disk.
        /// </summary>
        FILE_DEVICE_DISK = 0x00000007,
    }
    /// <summary>
    /// Enumeration that represents the Characteristics values.
    /// </summary>
    [Flags()]
    public enum Characteristics_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_REMOVABLE_MEDIA = 0x00000001,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_READ_ONLY_DEVICE = 0x00000002,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_FLOPPY_DISKETTE = 0x00000004,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_WRITE_ONCE_MEDIA = 0x00000008,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_REMOTE_DEVICE = 0x00000010,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_DEVICE_IS_MOUNTED = 0x00000020,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_VIRTUAL_VOLUME = 0x00000040,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_AUTOGENERATED_DEVICE_NAME = 0x00000080,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_DEVICE_SECURE_OPEN = 0x00000100,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_CHARACTERISTIC_PNP_DEVICE = 0x00000800,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_CHARACTERISTIC_TS_DEVICE = 0x00001000,

        /// <summary>
        ///  Possible value.
        /// </summary>
        FILE_CHARACTERISTIC_WEBDAV_DEVICE = 0x00002000,
    }

    /// <summary>
    ///  This information class is used to query or set the access
    ///  rights of the file. The FILE_ACCESS_INFORMATION data
    ///  element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_fileaccessinformation.xml
    //  </remarks>
    public partial struct FILE_ACCESS_INFORMATION
    {

        /// <summary>
        ///  A DWORD that MUST contain values specified in ACCESS_MASK
        ///  of [MS-DTYP].
        /// </summary>
        public _ACCESS_MASK AccessFlags;
    }

    /// <summary>
    ///  This information class is used to query a collection
    ///  of file information structures.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_fileallinformation.xml
    //  </remarks>
    public partial struct FileAllInformation
    {

        /// <summary>
        ///  A FILE_BASIC_INFORMATION structure specified in section
        ///  .
        /// </summary>
        public FileBasicInformation BasicInformation;

        /// <summary>
        ///  A FILE_STANDARD_INFORMATION structure specified in section
        ///  .
        /// </summary>
        public FileStandardInformation StandardInformation;

        /// <summary>
        ///  A FILE_INTERNAL_INFORMATION structure specified in section
        ///  .
        /// </summary>
        public FileInternalInformation InternalInformation;

        /// <summary>
        ///  A FILE_EA_INFORMATION structure specified in section
        ///  .
        /// </summary>
        public FileEaInformation EaInformation;

        /// <summary>
        ///  A FILE_ACCESS_INFORMATION structure specified in section
        ///  .
        /// </summary>
        public FILE_ACCESS_INFORMATION AccessInformation;

        /// <summary>
        ///  A FILE_POSITION_INFORMATION structure specified in section
        ///  .
        /// </summary>
        public FILE_POSITION_INFORMATION PositionInformation;

        /// <summary>
        ///  A FILE_MODE_INFORMATION structure specified in section
        ///  .
        /// </summary>
        public FILE_MODE_INFORMATION ModeInformation;

        /// <summary>
        ///  A FILE_ALIGNMENT_INFORMATION structure specified in
        ///  section.
        /// </summary>
        public FILE_ALIGNMENT_INFORMATION AlignmentInformation;

        /// <summary>
        ///  A FILE_NAME_INFORMATION structure specified in section
        ///  .
        /// </summary>
        public FileNameInformation NameInformation;
    }

    /// <summary>
    ///  This information class is used to query NTFS hard links
    ///  to an existing file. 				At least one name MUST be
    ///  returned. The FILE_LINKS_INFORMATION data element is
    ///  as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_filehardlinkinformation.xml
    //  </remarks>
    public partial struct FileHardLinkInformation
    {

        /// <summary>
        ///  A 32-bit unsigned integer that MUST contain the
        ///  number of bytes needed to hold all available names.
        ///  This field MUST NOT be 0.
        /// </summary>
        public uint BytesNeeded;

        /// <summary>
        ///  A 32-bit unsigned integer that MUST contain the number
        ///  of FILE_LINK_ENTRY_INFORMATION structures that have
        ///  been returned in the Entries field. This field MUST
        ///  return as many entries as will fit in available memory.
        ///  A value of 0 indicates that there is not enough available
        ///  memory to return any entry.   The error STATUS_BUFFER_OVERFLOW
        ///  (0x80000005) indicates that not all available entries
        ///  were returned.
        /// </summary>
        public uint EntriesReturned;

        /// <summary>
        ///  A buffer that MUST contain the returned FILE_LINK_ENTRY_INFORMATION
        ///  structures. It must be BytesNeeded in size to return
        ///  all of the available entries.
        /// </summary>
        public FILE_LINK_ENTRY_INFORMATION Entries;
    }

    /// <summary>
    ///  This information class is used to query or set the mode
    ///  of the file. The FILE_MODE_INFORMATION data element
    ///  is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_filemodeinformation.xml
    //  </remarks>
    public partial struct FILE_MODE_INFORMATION
    {

        /// <summary>
        ///  A ULONG that MUST specify on file creation or file open
        ///  how the file will subsequently be accessed.
        /// </summary>
        public Mode_Values Mode;
    }

    /// <summary>
    /// Enumeration that represents the Mode values.
    /// </summary>
    [Flags()]
    public enum Mode_Values : uint
    {

        /// <summary>
        ///  When set, any system services, file system drivers (FSDs),
        ///  and drivers that write data to the file must actually
        ///  transfer the data into the file before any requested
        ///  write operation is considered complete.
        /// </summary>
        FILE_WRITE_THROUGH = 0x00000002,

        /// <summary>
        ///  When set, all accesses to the file will be sequential.
        /// </summary>
        FILE_SEQUENTIAL_ONLY = 0x00000004,

        /// <summary>
        ///  When set, the file cannot be cached or buffered in a
        ///  driver's internal buffers.
        /// </summary>
        FILE_NO_INTERMEDIATE_BUFFERING = 0x00000008,

        /// <summary>
        ///  When set, all operations on the file are performed synchronously.
        ///  Any wait on behalf of the caller is subject to premature
        ///  termination from alerts. This flag also causes the
        ///  I/O system to maintain the file position context.
        /// </summary>
        FILE_SYNCHRONOUS_IO_ALERT = 0x00000010,

        /// <summary>
        ///  When set, all operations on the file are performed synchronously.
        ///  Wait requests in the system to synchronize I/O queuing
        ///  and completion are not subject to alerts. This flag
        ///  also causes the I/O system to maintain the file position
        ///  context.
        /// </summary>
        FILE_SYNCHRONOUS_IO_NONALERT = 0x00000020,

        /// <summary>
        ///  When set, delete the file when the last handle to the
        ///  file is closed.
        /// </summary>
        FILE_DELETE_ON_CLOSE = 0x00001000,
    }

    /// <summary>
    ///  This information class is used to query or set information
    ///  on a named pipe that is associated with the client
    ///  end of the pipe that is being queried. 				Remote information
    ///  is not available for local pipes or for the server
    ///  end of a remote pipe. The FILE_PIPE_REMOTE_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_filepiperemoteinformation.xml
    //  </remarks>
    public partial struct FilePipeRemoteInformation
    {

        /// <summary>
        ///  A LARGE_INTEGER that MUST contain the maximum amount
        ///  of time counted in 100-nanosecond intervals that will
        ///  elapse before transmission of data from the client
        ///  machine to the server.
        /// </summary>
        public _LARGE_INTEGER CollectDataTime;

        /// <summary>
        ///  A ULONG that MUST contain the maximum size in bytes
        ///  of data that will be collected on the client machine
        ///  before transmission to the server.
        /// </summary>
        public uint MaximumCollectionCount;
    }

    /// <summary>
    ///  This information class is used to query the position
    ///  of the file pointer within a file. The FILE_POSITION_INFORMATION
    ///  data element is as follows:
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_filepositioninformation.xml
    //  </remarks>
    public partial struct FILE_POSITION_INFORMATION
    {

        /// <summary>
        ///  A LARGE_INTEGER that MUST contain the offset, in bytes,
        ///  of the file pointer from the beginning of the file.
        /// </summary>
        public _LARGE_INTEGER CurrentByteOffset;
    }

    /// <summary>
    ///  The FILE_LINK_ENTRY_INFORMATION packet is used to describe
    ///  a single NTFS hard link to an existing file.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_file_link_entry_information.xml
    //  </remarks>
    public partial struct FILE_LINK_ENTRY_INFORMATION
    {

        /// <summary>
        ///  A 32-bit unsigned integer that MUST specify the
        ///  offset, in bytes, from the current FILE_LINK_ENTRY_INFORMATION
        ///  structure to the next FILE_LINK_ENTRY_INFORMATION structure.
        ///  A value of 0 indicates this is the last entry structure.
        /// </summary>
        public uint NextEntryOffset;

        /// <summary>
        ///  A 64-bit signed integer that MUST contain the FileID
        ///  of the parent directory of the given link.
        /// </summary>
        public long ParentFileId;

        /// <summary>
        ///  A 32-bit unsigned integer that MUST specify the
        ///  length, in bytes, of the FileName for the given link.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        ///  A WCHAR array whose size is given by FileNameLength
        ///  that MUST contain the Unicode string name of the given
        ///  link.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] FileName;
    }

    /// <summary>
    ///  The FSCTL_GET_NTFS_VOLUME_DATA reply message returns
    ///  the results of the FSCTL_GET_NTFS_VOLUME_DATA request
    ///  as an NTFS_VOLUME_DATA_BUFFER_REPLY element. The NTFS_VOLUME_DATA_BUFFER_REPLY
    ///  contains information on a volume.  For more information
    ///  about the NTFS file system, see [MSFT-NTFS]. 
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_fsctl_get_ntfs_volume_data_reply.xml
    //  </remarks>
    public partial struct NTFS_VOLUME_DATA_BUFFER_Reply
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the serial number
        ///  of the volume. This is a unique number assigned to
        ///  the volume media by the operating system when the volume
        ///  is formatted.
        /// </summary>
        public _LARGE_INTEGER VolumeSerialNumber;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  sectors in the specified volume.
        /// </summary>
        public _LARGE_INTEGER NumberSectors;

        /// <summary>
        ///  A 64-bit signed integer that contains the total number
        ///  of clusters in the specified volume.
        /// </summary>
        public _LARGE_INTEGER TotalClusters;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  free clusters in the specified volume.
        /// </summary>
        public _LARGE_INTEGER FreeClusters;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  reserved clusters in the specified volume. Reserved
        ///  clusters are free clusters reserved for when the volume
        ///  becomes full. Reserved clusters are released when either
        ///  the master file table grows beyond its allocated space
        ///  (the volume has a large number of small files) or the
        ///  volume becomes full (the volume has a small number
        ///  of large files).
        /// </summary>
        public _LARGE_INTEGER TotalReserved;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a sector on the specified volume.
        /// </summary>
        public uint BytesPerSector;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a cluster on the specified volume. This value
        ///  is also known as the cluster factor.
        /// </summary>
        public uint BytesPerCluster;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a file record segment.
        /// </summary>
        public uint BytesPerFileRecordSegment;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  clusters in a file record segment.
        /// </summary>
        public uint ClustersPerFileRecordSegment;

        /// <summary>
        ///  A 64-bit signed integer that contains the size of the
        ///  master file table in bytes.
        /// </summary>
        public _LARGE_INTEGER MftValidDataLength;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table.
        /// </summary>
        public _LARGE_INTEGER MftStartLcn;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table mirror.
        /// </summary>
        public _LARGE_INTEGER Mft2StartLcn;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table zone.
        /// </summary>
        public _LARGE_INTEGER MftZoneStart;

        /// <summary>
        ///  A 64-bit signed integer that contains the ending logical
        ///  cluster number of the master file table zone.
        /// </summary>
        public _LARGE_INTEGER MftZoneEnd;
    }

    /// <summary>
    ///  The FSCTL_PIPE_WAIT request requests that the server
    ///  wait  until either a time-out interval elapses or an
    ///  instance of the specified named pipe is available for
    ///  connection.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_fsctl_pipe_wait_request.xml
    //  </remarks>
    public partial struct FSCTL_PIPE_WAIT_Request
    {

        /// <summary>
        ///  A 64-bit signed integer that specifies the maximum amount
        ///  of time in units of 100 milliseconds that the function
        ///  can wait for an instance of the named pipe to be available. A
        ///  positive value expresses an absolute time at which
        ///  the base time is the beginning of the year 1601 A.D.
        ///  in the Gregorian calendar. A negative value expresses
        ///  a time interval relative to some base time, typically
        ///  the current time.
        /// </summary>
        public long Timeout;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the size, in
        ///  bytes, of the named pipe Name field.
        /// </summary>
        public uint NameLength;

        /// <summary>
        ///  An 8-bit unsigned value that specifies whether or not
        ///  the Timeout parameter will be ignored. 
        /// </summary>
        public TimeoutSpecified_Values TimeoutSpecified;

        /// <summary>
        ///  The server MUST ignore this 1-byte padding.
        /// </summary>
        public byte Padding;

        /// <summary>
        ///  A Unicode string that contains the name of the named
        ///  pipe. Name MUST not include the "\pipe\", so if the
        ///  operation was on \\server\pipe\pipename, the name would
        ///  be "pipename".    
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] Name;
    }
    /// <summary>
    /// Enumeration that represents the Timeout specified values.
    /// </summary>
    [Flags()]
    public enum TimeoutSpecified_Values : byte
    {

        /// <summary>
        ///  Indicates that the server MUST wait forever (no timeout)
        ///  for the named pipe. Any value in Timeout MUST be ignored.
        /// </summary>
        FALSE = 0,

        /// <summary>
        ///  Indicates that the server MUST use the value in the
        ///  Timeout parameter.
        /// </summary>
        TRUE = 1,
    }

    /// <summary>
    ///  The NTFS_VOLUME_DATA_BUFFER packet contains information
    ///  about a volume.  See [MSFT-NTFS] for more information
    ///  about the NTFS file system. 
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc_ntfs_volume_data_buffer.xml
    //  </remarks>
    public partial struct NTFS_VOLUME_DATA_BUFFER
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the serial number
        ///  of the volume. This is a unique number assigned to
        ///  the volume media by the operating system when the volume
        ///  is formatted.
        /// </summary>
        public _LARGE_INTEGER VolumeSerialNumber;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  sectors in the specified volume.
        /// </summary>
        public _LARGE_INTEGER NumberSectors;

        /// <summary>
        ///  A 64-bit signed integer that contains the total number
        ///  of clusters in the specified volume.
        /// </summary>
        public _LARGE_INTEGER TotalClusters;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  free clusters in the specified volume.
        /// </summary>
        public _LARGE_INTEGER FreeClusters;

        /// <summary>
        ///  A 64-bit signed integer that contains the number of
        ///  reserved clusters in the specified volume. Reserved
        ///  clusters are free clusters reserved for when the volume
        ///  becomes full. Reserved clusters are released when either
        ///  the master file table grows beyond its allocated space
        ///  (the volume has a large number of small files) or the
        ///  volume becomes full (the volume has a small number
        ///  of large files).
        /// </summary>
        public _LARGE_INTEGER TotalReserved;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a sector on the specified volume.
        /// </summary>
        public uint BytesPerSector;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a cluster on the specified volume. This value
        ///  is also known as the cluster factor.
        /// </summary>
        public uint BytesPerCluster;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  bytes in a file record segment.
        /// </summary>
        public uint BytesPerFileRecordSegment;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number of
        ///  clusters in a file record segment.
        /// </summary>
        public uint ClustersPerFileRecordSegment;

        /// <summary>
        ///  A 64-bit signed integer that contains the size of the
        ///  master file table, in bytes.
        /// </summary>
        public _LARGE_INTEGER MftValidDataLength;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table.
        /// </summary>
        public _LARGE_INTEGER MftStartLcn;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table mirror.
        /// </summary>
        public _LARGE_INTEGER Mft2StartLcn;

        /// <summary>
        ///  A 64-bit signed integer that contains the starting logical
        ///  cluster number of the master file table zone.
        /// </summary>
        public _LARGE_INTEGER MftZoneStart;

        /// <summary>
        ///  A 64-bit signed integer that contains the ending logical
        ///  cluster number of the master file table zone.
        /// </summary>
        public _LARGE_INTEGER MftZoneEnd;
    }

    /// <summary>
    ///  An ACCESS_MASK is a 32-bit set of flags that are used
    ///  to encode the user rights to an object.  An access
    ///  mask is used both to encode the rights to an object
    ///  assigned to a principal and to  encode the requested
    ///  access when opening an object. The bits with a 0 value
    ///  in the table below are used for object-specific user
    ///  rights. A file object would encode, for example, Read
    ///  Access and Write Access. A registry key object would
    ///  encode Create Subkey and  Read Value, for example. The
    ///  bits with a 0 value are reserved for use by specific
    ///  protocols that make use of the ACCESS_MASK data type.
    ///  The nature of this usage differs according to each
    ///  protocol and is implementation-specific. The bits with
    ///  a value different than 0 in the table below are user
    ///  rights that are common to all objects, or are generic
    ///  rights that can be mapped to object-specific user rights
    ///  by the object itself.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/DTYP/_rfc_ms-dtyp_access_mask.xml
    //  </remarks>
    public partial struct _ACCESS_MASK
    {

        /// <summary>
        ///   </summary>
        public uint ACCESS_MASK;
    }

    /// <summary>
    ///  The LARGE_INTEGER structure is used to represent a 64-bit
    ///  signed integer value.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/DTYP/_rfc_ms-dtyp_large_integer_union.xml
    //  </remarks>
    public partial struct _LARGE_INTEGER
    {

        /// <summary>
        ///  QuadPart member.
        /// </summary>
        public long QuadPart;
    }

    /// <summary>
    ///  The SID_IDENTIFIER_AUTHORITY structure represents the
    ///  top-level authority of a security identifier (SID).
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/DTYP/_rfc_ms-dtyp_sid_identifier_authority.xml
    //  </remarks>
    public partial struct _SID_IDENTIFIER_AUTHORITY
    {

        /// <summary>
        ///  A six-element array of 8-bit unsigned integers that
        ///  specify  the top-level authority of a  SID, RPC_SID,
        ///  and LSAPR_SID_INFORMATION.This value is generally set
        ///  to {0, 0, 0, 0, 0, 5} for SIDs that are associated
        ///  with accounts or groups. The identifier authority
        ///  value identifies the domain security authority that
        ///  issued the SID. The following identifier authorities
        ///  are predefined.
        /// </summary>
        [Inline()]
        [StaticSize(6, StaticSizeMode.Elements)]
        public Value_Values[] Value;
    }

    /// <summary>
    /// Enumeration that represents the value_values.
    /// </summary>
    [Flags()]
    public enum Value_Values : byte
    {

        /// <summary>
        ///  The authority is the NULL SID authority. It defines
        ///  only the NULL well-known-SID: S-1-0-0.
        /// </summary>
        NULL_SID_AUTHORITY = 0x00,

        /// <summary>
        ///  The authority is the World  SID authority. It only defines
        ///   the Everyone well-known-SID: S-1-1-0.
        /// </summary>
        WORLD_SID_AUTHORITY = 0x01,

        /// <summary>
        ///  The authority is the Local  SID authority. It defines
        ///  only the Local  well-known-SID: S-1-2-0.
        /// </summary>
        LOCAL_SID_AUTHORITY = 0x02,

        /// <summary>
        ///  The authority is the Creator SID authority. It defines
        ///  the Creator Owner, Creator Group, and Creator  Owner
        ///  Server well-known-SIDs: S-1-3-0, S-1-3-1, and S-1-3-2.
        ///  These SIDs are used as placeholders in an access control
        ///  list (ACL) and are replaced by the  user, group, and
        ///  machine SIDs of the security principal.
        /// </summary>
        CREATOR_SID_AUTHORITY = 0x03,

        /// <summary>
        ///  Not used.
        /// </summary>
        NON_UNIQUE_AUTHORITY = 0x04,

        /// <summary>
        ///  The authority is the security subsystem SID authority.
        ///  It defines all other SIDs in the forest.
        /// </summary>
        NT_AUTHORITY = 0x05,
    }

    /// <summary>
    ///  	The SYSTEMTIME structure is a date and time, in Coordinated
    ///  Universal Time (UTC), represented by using individual
    ///  WORD-sized structure members for the month, day, year,
    ///  day of week, hour, minute, second, and millisecond.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/DTYP/_rfc_ms-dtyp_systemtime.xml
    //  </remarks>
    public partial struct _SYSTEMTIME
    {

        /// <summary>
        ///  wYear member.
        /// </summary>
        public ushort wYear;

        /// <summary>
        ///  wMonth member.
        /// </summary>
        public ushort wMonth;

        /// <summary>
        ///  wDayOfWeek member.
        /// </summary>
        public ushort wDayOfWeek;

        /// <summary>
        ///  wDay member.
        /// </summary>
        public ushort wDay;

        /// <summary>
        ///  wHour member.
        /// </summary>
        public ushort wHour;

        /// <summary>
        ///  wMinute member.
        /// </summary>
        public ushort wMinute;

        /// <summary>
        ///  wSecond member.
        /// </summary>
        public ushort wSecond;

        /// <summary>
        ///  wMilliseconds member.
        /// </summary>
        public ushort wMilliseconds;
    }

    /// <summary>
    ///  A file range specification for RDC downloads.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_6.xml
    //  </remarks>
    public partial struct _FRS_RDC_SOURCE_NEED
    {

        /// <summary>
        ///  The offset in the marshaled source file.
        /// </summary>
        public ulong needOffset;

        /// <summary>
        ///  The number of bytes (uncompressed) to retrieve.
        /// </summary>
        public ulong needSize;
    }

    /// <summary>
    ///  Configuration parameters for the FilterMax RDC algorithm.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_7.xml
    //  </remarks>
    public partial struct _FRS_RDC_PARAMETERS_FILTERMAX
    {

        /// <summary>
        ///  See [MS-RDC] section 4.3 for the definition of the horizonSize
        ///  parameter of the FilterMax algorithm.
        /// </summary>
        public ushort horizonSize;

        /// <summary>
        ///  See [MS-RDC] section 4.3 for the definition of the hash
        ///  window size parameter of the FilterMax algorithm.
        /// </summary>
        public ushort windowSize;
    }

    /// <summary>
    ///  Configuration for the FilterPoint RDC algorithm. This
    ///  algorithm and its configuration parameters are not
    ///  used.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_8.xml
    //  </remarks>
    public partial struct _FRS_RDC_PARAMETERS_FILTERPOINT
    {

        /// <summary>
        ///  Unused.  MUST be 0 and MUST be ignored on receipt.
        /// </summary>
        public minChunkSize_Values minChunkSize;

        /// <summary>
        ///  Unused.  MUST be 0 and MUST be ignored on receipt.
        /// </summary>
        public maxChunkSize_Values maxChunkSize;
    }

    /// <summary>
    /// Enumeration that represents the minimum chunk size values.
    /// </summary>
    public enum minChunkSize_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Enumeration that represents the maximum chunk size values.
    /// </summary>
    public enum maxChunkSize_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  Blob for alternate RDC algorithms.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_9.xml
    //  </remarks>
    public partial struct _FRS_RDC_PARAMETERS_GENERIC
    {

        /// <summary>
        ///  The chunkerType MUST be RDC_FILTERMAX.
        /// </summary>
        public chunkerType_Values chunkerType;

        /// <summary>
        ///  Not used. This is a generic parameter block, which
        ///  allows for space in future protocol versions.
        /// </summary>
        [Inline()]
        [StaticSize(64, StaticSizeMode.Elements)]
        public byte[] chunkerParameters;
    }

    /// <summary>
    /// Enumeration that represents the Chunker Type values.
    /// </summary>
    public enum chunkerType_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0002,
    }

    /// <summary>
    ///  The FILETIME structure is a 64-bit value that represents
    ///  the number of 100-nanosecond intervals that have elapsed
    ///  since January 1, 1601, in Coordinated Universal Time
    ///  (UTC) format.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FSCC/_rfc_ms-fscc2_2_1_1.xml
    //  </remarks>
    public partial struct _FILETIME
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
    ///  The SID structure defines a security identifier (SID),
    ///  which is a variable-length byte array that uniquely
    ///  identifies a security principal. Each security principal
    ///  has a unique SID that is issued by a security agent.
    ///  The agent can be a local system or domain. The agent
    ///  generates the SID when the security principal is created.
    ///  The RPC marshaled version of the SID structure is defined
    ///  in section.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/DTYP/_rfc_ms-dtyp_sid.xml
    //  </remarks>
    public partial struct _SID
    {

        /// <summary>
        ///  An 8-bit unsigned integer that defines the revision
        ///  level of the SID structure. This value MUST be set
        ///  to 0x01.
        /// </summary>
        public Revision_Values Revision;

        /// <summary>
        ///  The number of elements in the SubAuthority array. The
        ///  maximum number of elements allowed is 15.
        /// </summary>
        public byte SubAuthorityCount;

        /// <summary>
        ///  A   SID_IDENTIFIER_AUTHORITY structure that contains
        ///  information that indicates the authority under which
        ///  the SID was created. It describes the entity that
        ///  created the SID and manages the account.
        /// </summary>
        public _SID_IDENTIFIER_AUTHORITY IdentifierAuthority;

        /// <summary>
        ///   A variable length array of unsigned 32-bit integers
        ///  that uniquely identifies a principal relative to the
        ///  IdentifierAuthority. Its length is determined by SubAuthorityCount.
        /// </summary>
        [Inline()]
        [Size("SubAuthorityCount")]
        public uint[] SubAuthority;
    }
    /// <summary>
    /// Enumeration that represents the Revision values.
    /// </summary>
    public enum Revision_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x01,
    }

    /// <summary>
    ///  An entry of a version chain vector.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_1.xml
    //  </remarks>
    public partial struct _FRS_VERSION_VECTOR
    {

        /// <summary>
        ///  The GUID for the database originating the versions in
        ///  the interval (low, high).
        /// </summary>
        public System.Guid dbGuid;

        /// <summary>
        ///  Lower bound for VSN interval.
        /// </summary>
        public ulong low;

        /// <summary>
        ///  Upper bound for VSN interval.
        /// </summary>
        public ulong high;
    }

    /// <summary>
    ///  Nested union of _FRS_RDC_PARAMETERS.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_10.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct @__nested_u__FRS_RDC_PARAMETERS
    {

        /// <summary>
        ///  Placeholder only to fill out the enumeration.  Never
        ///  used, because rdcChunkerAlgorithm MUST not have this
        ///  value.
        /// </summary>
        [Case("0")]
        public _FRS_RDC_PARAMETERS_GENERIC filterGeneric;

        /// <summary>
        ///  The parameters, as specified in [MS-RDC], necessary
        ///  for the FilterMax algorithm.
        /// </summary>
        [Case("1")]
        public _FRS_RDC_PARAMETERS_FILTERMAX filterMax;

        /// <summary>
        ///  Never used because rdcChunkerAlgorithm MUST not have
        ///  this value.
        /// </summary>
        [Case("2")]
        public _FRS_RDC_PARAMETERS_FILTERPOINT filterPoint;
    }

    /// <summary>
    ///  Union of RDC algorithm options.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_10.xml
    //  </remarks>
    public partial struct _FRS_RDC_PARAMETERS
    {

        /// <summary>
        ///   					SHOULD be RDC_FILTERMAX, for compatibility with
        ///  .  If the server receives an unrecognized value for
        ///  this field, the server MUST generate an error. and
        ///   use an rdcChunkerAlgorithm value of RDC_FILTERMAX.
        ///   This is the only value of rdcChunkerAlgorithm recognized
        ///  by  and .
        /// </summary>
        public ushort rdcChunkerAlgorithm;

        /// <summary>
        ///   </summary>
        [Switch("rdcChunkerAlgorithm")]
        public @__nested_u__FRS_RDC_PARAMETERS u;
    }

    /// <summary>
    ///  File information specific to RDC downloads.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_11.xml
    //  </remarks>
    public partial struct _FRS_RDC_FILEINFO
    {

        /// <summary>
        ///  An estimate for the on-disk, compressed, marshaled source
        ///  file. The server SHOULD make this estimate as accurate
        ///  as possible, but the protocol does not require that
        ///  it be exact.
        /// </summary>
        public ulong onDiskFileSize;

        /// <summary>
        ///  An estimate for the on-disk, uncompressed, unmarshaled
        ///  source file. The server SHOULD make this estimate
        ///  as accurate as possible, but the protocol does not
        ///  require that it be exact.
        /// </summary>
        public ulong fileSizeEstimate;

        /// <summary>
        ///   The current RDC version. It MUST be 1.
        /// </summary>
        public ushort rdcVersion;

        /// <summary>
        ///  The minimum version of the client-side RDC that is compatible
        ///  with the server-side RDC (rdcVersion). It MUST be
        ///  1.
        /// </summary>
        public ushort rdcMinimumCompatibleVersion;

        /// <summary>
        ///  The depth of the RDC signatures that are available for
        ///  the client to retrieve. The server MUST allow the
        ///  client to get signatures at least to this depth (using
        ///  RdcGetSignatures). The server MAY set this value to
        ///  0, indicating a non-RDC file transfer is required.
        /// </summary>
        public byte rdcSignatureLevels;

        /// <summary>
        ///  This field MUST be set to RDC_UNCOMPRESSED and MUST
        ///  be ignored on receipt.  Despite the name of this field,
        ///  data compression is always used as specified in section
        ///  .
        /// </summary>
        public _RDC_FILE_COMPRESSION_TYPES compressionAlgorithm;

        /// <summary>
        ///  The array of RDC chunker parameters used, one each for
        ///  the levels of RDCsignatures that are available. The
        ///  parameter onDiskFileSize is computed from the size
        ///  of a cached version of the marshaled, compressed file.
        ///   The parameter fileSizeEstimate is computed based on
        ///  the allocated byte ranges for the main data stream
        ///  of a file. The way that computes rdcSignatureLevels
        ///  is specified in [MS-RDC].
        /// </summary>
        [Inline()]
        [Size("rdcSignatureLevels, 1")]
        public _FRS_RDC_PARAMETERS rdcFilterParameters;
    }

    /// <summary>
    ///  An entry of an epoque vector.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_2.xml
    //  </remarks>
    public partial struct _FRS_EPOQUE_VECTOR
    {

        /// <summary>
        ///  Unused.  MUST be 0.  MUST be ignored on receipt.
        /// </summary>
        /// <summary>
        ///  Please refer to type 'machine_Guid' for the possible
        ///  values
        /// </summary>
        public System.Guid machine;

        /// <summary>
        ///  Unused.  MUST be 0.  MUST be ignored on receipt.
        /// </summary>
        /// <summary>
        ///  Please refer to type 'epoque__SYSTEMTIME' for the possible
        ///  values
        /// </summary>
        public _SYSTEMTIME epoque;
    }
    /// <summary>
    /// Class Machine Guid
    /// </summary>
    public class machine_Guid
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        public static System.Guid V1 = new System.Guid("0");
    }

    /// <summary>
    ///  A (UID, GVSN) pair.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_3.xml
    //  </remarks>
    public partial struct _FRS_ID_GVSN
    {

        /// <summary>
        ///  uidDbGuid member.
        /// </summary>
        public System.Guid uidDbGuid;

        /// <summary>
        ///  uidVersion member.
        /// </summary>
        public ulong uidVersion;

        /// <summary>
        ///  gvsnDbGuid member.
        /// </summary>
        public System.Guid gvsnDbGuid;

        /// <summary>
        ///  gvsnVersion member.
        /// </summary>
        public ulong gvsnVersion;
    }


    public partial struct FRS_UPDATE_PTR
    {

        /// <summary>
        ///  Indicates if the file exists or has been deleted. The
        ///  value MUST be either 0 or 1.
        /// </summary>

        public present_Values present;

        /// <summary>
        ///  Set if this update was tombstoned due to a name conflict.
        ///   The value MUST be either 0 or 1. This field MUST
        ///  be 0 if present is 1.
        /// </summary>
        public int nameConflict;

        /// <summary>
        ///  The file's attributes.
        /// </summary>
        public uint attributes;

        /// <summary>
        ///  The fence clock.
        /// </summary>
        public _FILETIME fence;

        /// <summary>
        ///  Logical, last change clock.
        /// </summary>
        public _FILETIME clock;

        /// <summary>
        ///  File creation time.
        /// </summary>
        public _FILETIME createTime;

        /// <summary>
        ///  The content set ID (replicated folder) that this file
        ///  belongs to.
        /// </summary>
        public System.Guid contentSetId;
        //public IntPtr contentSetId;

        /// <summary>
        ///  The SHA-1 hash of the file.
        /// </summary>
        //[Inline()]
        //[StaticSize(1, StaticSizeMode.Elements)]
        //public byte[] hash;
        private IntPtr hash;

        /// <summary>
        ///  The similarity hash of the file. The value will be
        ///  all zeros if the similarity data was not computed.
        ///   See [MS-RDC].
        /// </summary>
        //[Inline()]
        //[StaticSize(1, StaticSizeMode.Elements)]
        //public byte[] rdcSimilarity;
        private IntPtr rdcSimilarity;

        /// <summary>
        ///  The GUID portion of the file's UID.  Same as the database
        ///  GUID of the replicated folder where this file originated.
        /// </summary>
        public System.Guid uidDbGuid;
        //public IntPtr uidDbGuid;

        /// <summary>
        ///  The version sequence number portion of the file's UID.
        ///   This is assigned when the file is created.
        /// </summary>
        public ulong uidVersion;

        /// <summary>
        ///  The GUID portion of the file's GVSN.  Same as the database
        ///  GUID of the replicated folder where this file was last
        ///  updated.
        /// </summary>
        public System.Guid gvsnDbGuid;
        //public IntPtr gvsnDbGuid;

        /// <summary>
        ///  The version sequence number portion of the file's GVSN.
        ///   This is assigned when the file was last updated.
        /// </summary>
        public ulong gvsnVersion;

        /// <summary>
        ///  The GUID portion of the UID of the file's parent.  Same
        ///  as the database GUID of the replicated folder where
        ///  this file's parent originated.
        /// </summary>
        public System.Guid parentDbGuid;

        /// <summary>
        ///  The version sequence number portion of the UID of the
        ///  file's parent. This is assigned when the parent of
        ///  the file was created.
        /// </summary>
        public ulong parentVersion;

        /// <summary>
        ///  The file name, in UTF-16 form, of the file.
        /// </summary>
        //[Inline()]
        //[String()]
        //[StaticSize(261, StaticSizeMode.Elements)]
        //public ushort[] name;
        private IntPtr name;

        /// <summary>
        ///  Unused in version 0x00050000.  MUST be 0.  MUST be ignored
        ///  on receipt.  In version 0x00050001 or later, the value
        ///  of this field may be some combination of 0 or more
        ///  of the following flag bits: FRS_UPDATE_FLAG_GHOSTED_HEADER
        ///  and FRS_UPDATE_FLAG_DATA.
        /// </summary>
        public flags_Values flags;

        /// <summary>
        /// Constructor with _FRS_UPDATE
        /// </summary>
        /// <param name="frsUpdate">Input frsUpdate byte array</param>
        public FRS_UPDATE_PTR(_FRS_UPDATE frsUpdate)
        {
            attributes = frsUpdate.attributes;
            clock.dwHighDateTime = frsUpdate.clock.dwHighDateTime;
            clock.dwLowDateTime = frsUpdate.clock.dwLowDateTime;
            contentSetId = frsUpdate.contentSetId;
            createTime.dwLowDateTime = frsUpdate.createTime.dwLowDateTime;
            createTime.dwHighDateTime = frsUpdate.createTime.dwHighDateTime;
            fence.dwLowDateTime = frsUpdate.fence.dwLowDateTime;
            fence.dwHighDateTime = frsUpdate.fence.dwHighDateTime;
            flags = frsUpdate.flags;
            gvsnDbGuid = frsUpdate.gvsnDbGuid;
            gvsnVersion = frsUpdate.gvsnVersion;
            nameConflict = frsUpdate.nameConflict;
            parentDbGuid = frsUpdate.parentDbGuid;
            parentVersion = frsUpdate.parentVersion;
            uidDbGuid = frsUpdate.uidDbGuid;
            uidVersion = frsUpdate.uidVersion;
            present = frsUpdate.present;

            rdcSimilarity = Marshal.AllocHGlobal(frsUpdate.rdcSimilarity.Length);
            Marshal.Copy(frsUpdate.rdcSimilarity, 0, rdcSimilarity, frsUpdate.rdcSimilarity.Length);

            hash = Marshal.AllocHGlobal(frsUpdate.hash.Length);
            Marshal.Copy(frsUpdate.hash, 0, hash, frsUpdate.hash.Length);

            name = Marshal.AllocHGlobal(524);
            Marshal.Copy(frsUpdate.name, 0, name, frsUpdate.name.Length);
        }
    }

    /// <summary>
    ///  A structure that contains file metadata related to a
    ///  particular file being processed by Distributed File
    ///  System Replication (DFS-R).
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_4.xml
    //  </remarks>
    public partial struct _FRS_UPDATE
    {

        /// <summary>
        ///  Indicates if the file exists or has been deleted. The
        ///  value MUST be either 0 or 1.
        /// </summary>
        public present_Values present;

        /// <summary>
        ///  Set if this update was tombstoned due to a name conflict.
        ///   The value MUST be either 0 or 1. This field MUST
        ///  be 0 if present is 1.
        /// </summary>
        public int nameConflict;

        /// <summary>
        ///  The file's attributes.
        /// </summary>
        public uint attributes;

        /// <summary>
        ///  The fence clock.
        /// </summary>
        public _FILETIME fence;

        /// <summary>
        ///  Logical, last change clock.
        /// </summary>
        public _FILETIME clock;

        /// <summary>
        ///  File creation time.
        /// </summary>
        public _FILETIME createTime;

        /// <summary>
        ///  The content set ID (replicated folder) that this file
        ///  belongs to.
        /// </summary>
        public System.Guid contentSetId;

        /// <summary>
        ///  The SHA-1 hash of the file.
        /// </summary>
        [Inline()]
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] hash;

        /// <summary>
        ///  The similarity hash of the file. The value will be
        ///  all zeros if the similarity data was not computed.
        ///   See [MS-RDC].
        /// </summary>
        [Inline()]
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] rdcSimilarity;

        /// <summary>
        ///  The GUID portion of the file's UID.  Same as the database
        ///  GUID of the replicated folder where this file originated.
        /// </summary>
        public System.Guid uidDbGuid;

        /// <summary>
        ///  The version sequence number portion of the file's UID.
        ///   This is assigned when the file is created.
        /// </summary>
        public ulong uidVersion;

        /// <summary>
        ///  The GUID portion of the file's GVSN.  Same as the database
        ///  GUID of the replicated folder where this file was last
        ///  updated.
        /// </summary>
        public System.Guid gvsnDbGuid;

        /// <summary>
        ///  The version sequence number portion of the file's GVSN.
        ///   This is assigned when the file was last updated.
        /// </summary>
        public ulong gvsnVersion;

        /// <summary>
        ///  The GUID portion of the UID of the file's parent.  Same
        ///  as the database GUID of the replicated folder where
        ///  this file's parent originated.
        /// </summary>
        public System.Guid parentDbGuid;

        /// <summary>
        ///  The version sequence number portion of the UID of the
        ///  file's parent. This is assigned when the parent of
        ///  the file was created.
        /// </summary>
        public ulong parentVersion;

        /// <summary>
        ///  The file name, in UTF-16 form, of the file.
        /// </summary>
        [Inline()]
        [String()]
        [StaticSize(261, StaticSizeMode.Elements)]
        public short[] name;

        /// <summary>
        ///  Unused in version 0x00050000.  MUST be 0.  MUST be ignored
        ///  on receipt.  In version 0x00050001 or later, the value
        ///  of this field may be some combination of 0 or more
        ///  of the following flag bits: FRS_UPDATE_FLAG_GHOSTED_HEADER
        ///  and FRS_UPDATE_FLAG_DATA.
        /// </summary>
        public flags_Values flags;
    }

    /// <summary>
    /// Enumeration that represents the present flag values.
    /// </summary>
    [Flags()]
    public enum present_Values : uint
    {

        /// <summary>
        ///  File has been deleted.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  File exists.
        /// </summary>
        V2 = 0x00000001,
    }

    /// <summary>
    /// Enumeration that represents the flag values.
    /// </summary>
    [Flags()]
    public enum flags_Values : uint
    {

        /// <summary>
        ///  The update request includes all types of data.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        ///  The update request is for ghosted header only.
        /// </summary>
        FRS_UPDATE_GHOSTED_HEADER = 0x00000004,

        /// <summary>
        ///  The update request is for file data only.
        /// </summary>
        FRS_UPDATE_FLAG_DATA = 0x00000008,
    }

    /// <summary>
    ///  A structure that contains information about updates
    ///  that were not processed by a client.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_5.xml
    //  </remarks>
    public partial struct _FRS_UPDATE_CANCEL_DATA
    {

        /// <summary>
        ///  The update that could not be processed by client.
        /// </summary>
        public _FRS_UPDATE blockingUpdate;

        /// <summary>
        ///  The content set where the blocking update resides.
        /// </summary>
        public System.Guid contentSetId;

        /// <summary>
        ///  The GUID part of the GVSN of the update that could not
        ///  be processed.
        /// </summary>
        public System.Guid gvsnDatabaseId;

        /// <summary>
        ///  The GUID part of the UID from the update that could
        ///  not be processed.
        /// </summary>
        public System.Guid uidDatabaseId;

        /// <summary>
        ///  The GUID part of the parent UID from the update that
        ///  could not be processed.
        /// </summary>
        public System.Guid parentDatabaseId;

        /// <summary>
        ///  The VSN part of the GVSN of the update that could not
        ///  be processed.
        /// </summary>
        public ulong gvsnVersion;

        /// <summary>
        ///  The VSN part of the UID from the update that could not
        ///  be processed.
        /// </summary>
        public ulong uidVersion;

        /// <summary>
        ///  The VSN part of the parent UID from the update that
        ///  could not be processed.
        /// </summary>
        public ulong parentVersion;

        /// <summary>
        ///  The cause for canceling the processing of the update.
        ///   These MUST be set to one of the following values.
        /// </summary>
        public cancelType_Values cancelType;

        /// <summary>
        ///  If set, indicates that the UID field is populated.
        /// </summary>
        public int isUidValid;

        /// <summary>
        ///  If set, indicates that the parent field is populated.
        /// </summary>
        public int isParentUidValid;

        /// <summary>
        ///  If set, indicates that the blockingUpdate field is populated
        ///  with valid data.
        /// </summary>
        public int isBlockerValid;
    }

    /// <summary>
    /// Enumeration that represents the Cancel type values.
    /// </summary>
    [Flags()]
    public enum cancelType_Values : uint
    {

        /// <summary>
        ///  No reason is indicated by the client. The GVSN and
        ///  UID MUST indicate which update was not processed by
        ///  the client.
        /// </summary>
        UNSPECIFIED = 0x00000001,

        /// <summary>
        ///  The client holds a tombstone at the location where an
        ///  update inserts a child. The value of blockingUpdate
        ///  is set to the record associated with the parent; the
        ///  parent field is ignored.
        /// </summary>
        PARENT_IS_TOMBSTONE = 0x00000002,

        /// <summary>
        ///  The update is for a tombstone. The client holds a child,
        ///  whose version is known by the server. The client cannot
        ///  proceed until the child has been moved. The blockingUpdate
        ///  field is set to the child record.
        /// </summary>
        CHILD_IS_LIVE = 0x00000003,

        /// <summary>
        ///  The client does not have the parent for the (live) update
        ///  that was sent. The server should send the parent before
        ///  proceeding. The parent field (parentGuid and parentVersion)
        ///  are set to the missing parent.
        /// </summary>
        PARENT_IS_MISSING = 0x00000004,
    }

    /// <summary>
    ///  Version chain vector response payload.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_12.xml
    //  </remarks>
    public partial struct _FRS_ASYNC_VERSION_VECTOR_RESPONSE
    {

        /// <summary>
        ///  The time stamp associated with the version chain vector
        ///  on the server. The time stamp is incremented every
        ///  time a server updates its version chain vector. This
        ///  gives a way to track whether a client has the newest
        ///  version of the version chain vector known to the server.
        /// </summary>
        public ulong vvGeneration;

        /// <summary>
        ///  Number of elements in the versionVector array.
        /// </summary>
        public uint versionVectorCount;

        /// <summary>
        ///  An array of FRS_VERSION_VECTOR triples.
        /// </summary>
        [Size("versionVectorCount")]
        public _FRS_VERSION_VECTOR[] versionVector;
        // public IntPtr versionVector;
        /// <summary>
        ///  Number of elements in the epoque vector array.
        /// </summary>
        public uint epoqueVectorCount;

        /// <summary>
        ///  An array of FRS_EPOQUE vector pairs.
        /// </summary>
        [Size("epoqueVectorCount")]
        public _FRS_EPOQUE_VECTOR[] epoqueVector;
        //public IntPtr epoqueVector;
    }


    /// <summary>
    ///  Version chain vector response payload.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_12.xml
    //  </remarks>
    public partial struct _FRS_ASYNC_VERSION_VECTOR_RESPONSE_POINTER
    {

        /// <summary>
        ///  The time stamp associated with the version chain vector
        ///  on the server. The time stamp is incremented every
        ///  time a server updates its version chain vector. This
        ///  gives a way to track whether a client has the newest
        ///  version of the version chain vector known to the server.
        /// </summary>
        public ulong vvGeneration;

        /// <summary>
        ///  Number of elements in the versionVector array.
        /// </summary>
        public uint versionVectorCount;

        /// <summary>
        ///  An array of FRS_VERSION_VECTOR triples.
        /// </summary>
        //[Size("versionVectorCount")]
        //public _FRS_VERSION_VECTOR[] versionVector;
        public readonly IntPtr versionVector;

        /// <summary>
        ///  Number of elements in the epoque vector array.
        /// </summary>
        public uint epoqueVectorCount;

        /// <summary>
        ///  An array of FRS_EPOQUE vector pairs.
        /// </summary>
        //[Size("epoqueVectorCount")]
        //public _FRS_EPOQUE_VECTOR[] epoqueVector;
        public readonly IntPtr epoqueVector;
    }

    /// <summary>
    ///  Version chain vector response payload envelope.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_13.xml
    //  </remarks>
    public partial struct _FRS_ASYNC_RESPONSE_CONTEXT_POINTER
    {

        /// <summary>
        ///  Sequence number that associates the response context
        ///  with a version vector request.
        /// </summary>
        public uint sequenceNumber;

        /// <summary>
        ///  Error/success status of version vector request. The
        ///  error code can be any of the values specified in section
        ///  .
        /// </summary>
        public uint status;

        /// <summary>
        ///  Response payload, comprising a version chain vector.
        /// </summary>
        public _FRS_ASYNC_VERSION_VECTOR_RESPONSE_POINTER result;
    }

    /// <summary>
    ///  Version chain vector response payload envelope.
    /// </summary>
    //  <remarks>
    //   file:///D:/IDL_frs2/FRS2/_rfc_ms-frs22_2_1_4_13.xml
    //  </remarks>
    public partial struct _FRS_ASYNC_RESPONSE_CONTEXT
    {

        /// <summary>
        ///  Sequence number that associates the response context
        ///  with a version vector request.
        /// </summary>
        public uint sequenceNumber;

        /// <summary>
        ///  Error/success status of version vector request. The
        ///  error code can be any of the values specified in section
        ///  .
        /// </summary>
        public uint status;

        /// <summary>
        ///  Response payload, comprising a version chain vector.
        /// </summary>
        public _FRS_ASYNC_VERSION_VECTOR_RESPONSE result;
    }

    /// <summary>
    /// IFRS2Adapter Interface
    /// </summary>
    public partial interface IFRS2Adapter : IRpcAdapter
    {
       
        /// <summary>
        ///  The CheckConnectivity method is used for a client to
        ///  check whether the server is reachable and has been
        ///  configured to replicate with the server. Opnum: 0
        ///  
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="replicaSetId">
        ///  From the GUID of the replication group. It is configured
        ///  as the GUID of an object under the path msDFSR-GlobalSettings/msDFSR-ReplicationGroup.
        ///   See section for a definition of the AD schema. This
        ///  corresponds to the replication group GUID (see section
        ///  ). As with all other uses of GUIDs used in DFS-R, the
        ///  usual assumption that GUIDs are globally unique is
        ///  implied. This means that two GUIDs used in different
        ///  contexts, such as replicaSetIds for two different replication
        ///  groups, or a GUID used for a replicaSetId and one used
        ///  for a connectionId are different. The DFS-R protocol
        ///  does not require other constraints on GUIDs, such as
        ///  the specific contents of a GUID.
        /// </param>
        /// <param name="connectionId">
        ///  From the GUID of the connection. It is configured as
        ///  the GUID of an object under the path msDFSR-GlobalSettings/msDFSR-ReplicationGroup/msDFSR-Member/msDFSR-Connection.
        /// </param>
        // [DllImport(@"D:\Model\Frs2Traffic\Frs2Traffic\FRS2Stubs\frs2\Debug\frs2.dll")]
        uint CheckConnectivity(System.IntPtr bindingHandle, Guid replicaSetId, Guid connectionId);
        //System.Guid replicaSetId, System.Guid connectionId);

        /// <summary>
        ///  The EstablishConnection method establishes a logical
        ///  connection from a client to a server. A logical connection
        ///  to the server is required before most other operations
        ///  can be performed. Opnum: 1 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="replicaSetId">
        ///  From the GUID of the configured replication group. 
        ///  Same as in section.
        /// </param>
        /// <param name="connectionId">
        ///  From the GUID of the configured connection.  Same as
        ///  in CheckConnectivity.
        /// </param>
        /// <param name="downstreamProtocolVersion">
        ///  Identifies the version of the protocol implemented by
        ///  the client. The version MUST be one of: 0x00050000,
        ///  0x00050001, or 0x00050002, as specified in section
        ///  .
        /// </param>
        /// <param name="downstreamFlags">
        ///  A flags bitmask. The only one bit that is defined is
        ///  TRANSPORT_OS_ENTERPRISE. This one bit MAY be set by
        ///  the client, and all other bits MUST be clear (0).
        /// </param>
        /// <param name="upstreamProtocolVersion">
        ///  Receives the server's protocol version number.  Expected
        ///  values are the same as for downstreamProtocolVersion.
        /// </param>
        /// <param name="upstreamFlags">
        ///  A flags bitmask. The only one bit that is defined is
        ///  TRANSPORT_OS_ENTERPRISE. This one bit MAY be set by
        ///  the server, and all other bits MUST be clear.
        /// </param>
        uint EstablishConnection(System.IntPtr bindingHandle, System.Guid replicaSetId, System.Guid connectionId, uint downstreamProtocolVersion, uint downstreamFlags, out System.UInt32? upstreamProtocolVersion, out System.UInt32? upstreamFlags);

        /// <summary>
        ///  The EstablishSession method is used to establish a logical
        ///  relationship on the server for a replicated folder.
        ///  Opnum: 2 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="connectionId">
        ///  The GUID (as specified in [MS-DTYP] section) of the
        ///  configured connection; same as in the CheckConnectivity
        ///  call.
        /// </param>
        /// <param name="contentSetId">
        ///  The GUID of the configured replicated folder. It is
        ///  configured as the GUID of an object under msDFSR-GlobalSettings/msDFSR-ReplicationGroup/msDFSR-Content/msDFSR-ContentSet.
        ///   The definition of the AD schema is as specified in
        ///  section.
        /// </param>
        uint EstablishSession(System.IntPtr bindingHandle, System.Guid connectionId, System.Guid contentSetId);

        /// <summary>
        ///  The RequestUpdates method is used to obtain file metadata
        ///  in the form of updates from a server. Opnum: 3 
        /// </summary>
        /// <param name="AsyncHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="connectionId">
        ///  The globally unique identifier (GUID) of the connection
        ///  ID, which represents a specific replication partnership.
        /// </param>
        /// <param name="contentSetId">
        ///  The GUID of the content set ID (replicated folder) to
        ///  be processed by this replication connection.
        /// </param>
        /// <param name="creditsAvailable">
        ///  The number of file replication update slots available
        ///  for use in filling up the output array of updates (frsUpdate).
        /// </param>
        /// <param name="hashRequested">
        ///  The value is TRUE if the client requests the server
        ///  to compute hashes for the files indicated by the update
        ///  records; otherwise, the value is FALSE . The server
        ///  is not required to compute hashes even if client requests
        ///  so.  Recall that DFS-R represents Boolean values in
        ///  longs, as specified in  sections  and .
        /// </param>
        /// <param name="updateRequestType">
        ///  The value from the UPDATE_REQUEST_TYPE enumeration that
        ///  indicates the type of replication updates requested.
        /// </param>
        /// <param name="versionVectorDiffCount">
        ///  The number of items specified in the versionVectorDiff
        ///  parameter.
        /// </param>
        /// <param name="versionVectorDiff">
        ///  The set of FRS_VERSION_VECTOR structures that describes
        ///  version information for files to be replicated.
        /// </param>
        /// <param name="frsUpdate">
        ///  The set of FRS_UPDATE structures that describes the
        ///  update that occurred to each of the files to be replicated.
        /// </param>
        /// <param name="updateCount">
        ///  The number of items specified in the frsUpdate parameter.
        /// </param>
        /// <param name="updateStatus">
        ///  The value from the UPDATE_STATUS enumeration that describes
        ///  the status of the requested update.
        /// </param>
        /// <param name="gvsnDbGuid">
        ///  The global version number (GVSN) GUID (as specified
        ///  in [MS-DTYP] section) for the last field in the versionVectorDiff
        ///  that was processed.
        /// </param>
        /// <param name="gvsnVersion">
        ///  The version of the gvsnDbGuid.
        /// </param>
        uint RequestUpdates(System.IntPtr AsyncHandle, System.IntPtr bindingHandle, System.Guid connectionId, System.Guid contentSetId, uint creditsAvailable, int hashRequested, _UPDATE_REQUEST_TYPE updateRequestType, uint versionVectorDiffCount, [Size("versionVectorDiffCount")] _FRS_VERSION_VECTOR[] versionVectorDiff, [Inline()] [Length("*updateCount")] [Size("creditsAvailable")] out _FRS_UPDATE[] frsUpdate, out System.UInt32? updateCount, out _UPDATE_STATUS updateStatus, out System.Guid? gvsnDbGuid, out System.UInt64? gvsnVersion);

        /// <summary>
        ///  The RequestVersionVector method is used to obtain the
        ///  version chain vector persisted on a server. Opnum :
        ///  4 
        /// </summary>
        /// <param name="AsyncHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="sequenceNumber">
        ///  The sequence number for this request. The sequence
        ///  number is used to pair the version vector request with
        ///  the asynchronous response in AsyncPoll.  During a given
        ///  session, the client MUST supply a unique sequence number
        ///  for each call to this function.
        /// </param>
        /// <param name="connectionId">
        ///  The GUID (as specified in [MS-DTYP] section)  of the
        ///  connection ID that represents a specific replication
        ///  partnership.
        /// </param>
        /// <param name="contentSetId">
        ///  The GUID (as specified in [MS-DTYP] section) of the
        ///  content set ID (replicated folder) to be processed
        ///  by this replication connection.
        /// </param>
        /// <param name="requestType">
        ///  The value from the VERSION_REQUEST_TYPE enumeration
        ///  that describes the type of replication sync to perform.
        /// </param>
        /// <param name="changeType">
        ///  The value from the VERSION_CHANGE_TYPE enumeration that
        ///  indicates whether to notify change only, change delta,
        ///  or send the entire version chain vector.
        /// </param>
        /// <param name="vvGeneration">
        ///  The generation of the version chain vector for that
        ///  replicated folder on the requesting protocol. This requests
        ///  that the server return the specified version vector
        ///  information in the outstanding AsyncPoll request. The
        ///  vvGeneration parameter is used to calibrate what incarnation
        ///  of the server's version chain vector is known to the
        ///  client. The client supplies the last generation number
        ///  that it received from the server.
        /// </param>
        uint RequestVersionVector(System.IntPtr AsyncHandle, System.IntPtr bindingHandle, uint sequenceNumber, System.Guid connectionId, System.Guid contentSetId, _VERSION_REQUEST_TYPE requestType, _VERSION_CHANGE_TYPE changeType, ulong vvGeneration);

        /// <summary>
        ///  The AsyncPoll method used to register an asynchronous
        ///  callback for a server to provide version chain vectors.
        ///  Opnum: 5 
        /// </summary>
        /// <param name="AsyncHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="connectionId">
        ///  The GUID (as specified in [MS-DTYP] section) of the
        ///  connection ID that represents a specific replication
        ///  partnership.
        /// </param>
        /// <param name="response">
        ///  The FRS_ASYNC_RESPONSE_CONTEXT structure that contains
        ///  the context for the requested poll.
        /// </param>
        uint AsyncPoll(System.IntPtr AsyncHandle, System.IntPtr bindingHandle, System.Guid connectionId, out _FRS_ASYNC_RESPONSE_CONTEXT response);

        /// <summary>
        ///  The RequestRecords method used to retrieve UIDs and
        ///  GVSNs that a server persists. Opnum: 6 
        /// </summary>
        /// <param name="AsyncHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="connectionId">
        ///  The GUID (as specified in [MS-DTYP] section) of the
        ///  connection ID that represents a specific replication
        ///  partnership.
        /// </param>
        /// <param name="contentSetId">
        ///  The GUID (as specified in [MS-DTYP] section) of the
        ///  content set ID (replicated folder) to be processed
        ///  by this replication connection.
        /// </param>
        /// <param name="uidDbGuid">
        ///  The GVSN that corresponds to the first instance (creation)
        ///  of this file within the FRS replicated folder and version
        ///  of the uidDatabaseId from the array of FRS_ID_GVSN,
        ///  respectively. On the first call to this function, the
        ///  client MUST initialize these fields to all zeros. On
        ///  subsequent calls, they MUST be the last uidDatabaseId
        ///  and uidVersion processed from the returned results
        ///  in compressedRecords. This provides a cursor, so that
        ///  it can  iterate through all records on the server.
        /// </param>
        /// <param name="uidVersion">
        ///  The GVSN that corresponds to the first instance (creation)
        ///  of this file within the FRS replicated folder and version
        ///  of the uidDatabaseId from the array of FRS_ID_GVSN,
        ///  respectively. On the first call to this function, the
        ///  client MUST initialize them to all zeros. On subsequent
        ///  calls, they MUST be the last uidDatabaseId and uidVersion
        ///  processed from the returned results in compressedRecords.
        ///  This provides a  with a cursor, so that it can  iterate
        ///  through all records on the server.
        /// </param>
        /// <param name="maxRecords">
        ///  The maximum number of records that can be returned to
        ///  the client.
        /// </param>
        /// <param name="numRecords">
        ///  The number of items in the compressedRecords parameter.
        /// </param>
        /// <param name="numBytes">
        ///  The size, in bytes, of the compressedRecords parameter.
        /// </param>
        /// <param name="compressedRecords">
        ///  The data records compressed, according to the format
        ///  specified in [MS-DRSR].DFS-R uses custom marshaling
        ///  in this RPC call to compress the set of transmitted
        ///  records into a blob that conforms to the compressed
        ///  format specified by the decompression algorithm specified
        ///  in [MS-DRSR]. Note that the decompression algorithm
        ///  specified in [MS-DRSR] gives uniquely the set of compressed
        ///  representations of a source string.  Any algorithm
        ///  that computes one of the set of possible pre-images
        ///  to the decompression algorithm is an admissible compression
        ///  algorithm. The compressedRecords bytes correspond to
        ///  an array of FRS_ID_GVSN entries. The size of the array
        ///  is given by numRecords.  Thus, the uncompressed FRS_ID_GVSN
        ///  array can be obtained by using a compression engine
        ///  that implements the format (as specified in [MS-DRSR])
        ///  to have it decode numBytes from compressedRecords into
        ///  sizeof(FRS_ID_GVSN)*numRecords bytes, which can be
        ///  re-interpreted as an array of FRS_ID_GVSN entries.
        /// </param>
        /// <param name="recordsStatus">
        ///  The value from the RECORDS_STATUS enumeration that indicates
        ///  whether more update records are available.
        /// </param>
        uint RequestRecords(System.IntPtr AsyncHandle, System.IntPtr bindingHandle, System.Guid connectionId, System.Guid contentSetId, System.Guid uidDbGuid, ulong uidVersion, ref System.UInt32? maxRecords, out System.UInt32? numRecords, out System.UInt32? numBytes, [Size("*numBytes")] out byte[] compressedRecords, out _RECORDS_STATUS recordsStatus);

        /// <summary>
        ///  The UpdateCancel method is used by a client to indicate
        ///  to a server that it could not process an update.  Recall
        ///  that "client"as used in DFS-Rmeans the downstream partner,
        ///  and does not indicate the operating system version
        ///  being run. Opnum: 7 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="connectionId">
        ///  The GUID (as specified in [MS-DTYP] section) of the
        ///  connection ID that represents a specific replication
        ///  partnership.
        /// </param>
        /// <param name="cancelData">
        ///  The FRS_UPDATE_CANCEL_DATA structure that describes
        ///  an update to cancel.
        /// </param>
        uint UpdateCancel(System.IntPtr bindingHandle, System.Guid connectionId, _FRS_UPDATE_CANCEL_DATA cancelData);

        /// <summary>
        ///  The RawGetFileData method is used for to transfer successive
        ///  segments from a file. Opnum: 8 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="serverContext">
        ///  The context handle that represents the requested file
        ///  replication operation.  Use the InitializeFileTransferAsync
        ///  method of the FrsTransport interface to create a serverContext
        ///  prior to using this method.
        /// </param>
        /// <param name="dataBuffer">
        ///  The file data received from the server.
        /// </param>
        /// <param name="bufferSize">
        ///  The size, in bytes, of dataBuffer.
        /// </param>
        /// <param name="sizeRead">
        ///  The size, in bytes, of the file data returned in dataBuffer.
        /// </param>
        /// <param name="isEndOfFile">
        ///  The value is TRUE if the end of the specified file has
        ///  been reached and there is no more file data to replicate
        ///  to the client; otherwise, the value is FALSE.
        /// </param>
        uint RawGetFileData(System.IntPtr bindingHandle, ref System.IntPtr? serverContext, [Inline()] [Length("*sizeRead")] [Size("bufferSize")] out byte[] dataBuffer, uint bufferSize, out System.UInt32? sizeRead, out System.Int32? isEndOfFile);

        /// <summary>
        ///  The RdcGetSignatures method is used to obtain RDCsignature
        ///  data from a server. Opnum: 9 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="serverContext">
        ///  The context handle that represents the requested file
        ///  replication operation.  Section  , which gives the
        ///  sequencing assumptions associated with this method,
        ///  specifies using the InitializeFileTransferAsync method
        ///  of the FrsTransport interface to create a serverContext
        ///  prior to using this method.
        /// </param>
        /// <param name="level">
        ///  The recursion level. A client MUST specify a number
        ///  in the range 1 to CONFIG_RDC_MAX_LEVELS.
        /// </param>
        /// <param name="offset">
        ///  The zero-based offset, in bytes, at which to retrieve
        ///  data from the file.
        /// </param>
        /// <param name="buffer">
        ///  The file signature data received from the server.
        /// </param>
        /// <param name="length">
        ///  The size, in bytes, of buffer.
        /// </param>
        /// <param name="sizeRead">
        ///  The size, in bytes, of the file data returned in buffer.
        /// </param>
        uint RdcGetSignatures(System.IntPtr bindingHandle, System.IntPtr serverContext, byte level, ulong offset, [Inline()] [Length("*sizeRead")] [Size("length")] out byte[] buffer, uint length, out System.UInt32? sizeRead);

        /// <summary>
        ///  The RdcPushSourceNeeds method is used to register requests
        ///  for file ranges on a server. Opnum: 10 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="serverContext">
        ///  The context handle that represents the requested file
        ///  replication operation.  Section  , which gives the
        ///  sequencing assumptions associated with this method,
        ///  specifies using the InitializeFileTransferAsync method
        ///  of the FrsTransport interface to create a serverContext
        ///  prior to using this method.
        /// </param>
        /// <param name="sourceNeeds">
        ///  The pointer to a set of FRS_RDC_SOURCE_NEED structures
        ///  that indicate the offsets and lengths of file data
        ///  that must be sent from the server to the client.
        /// </param>
        /// <param name="needCount">
        ///  The number of FRS_RDC_SOURCE_NEED structures pointed
        ///  to by sourceNeeds.
        /// </param>
        uint RdcPushSourceNeeds(System.IntPtr bindingHandle, System.IntPtr serverContext, [Size("needCount")] _FRS_RDC_SOURCE_NEED[] sourceNeeds, uint needCount);

        /// <summary>
        ///  The RdcGetFileData method is used to obtain file ranges
        ///  whose requests have previously been registered on a
        ///  server. Opnum: 11 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="serverContext">
        ///  The context handle that represents the requested file
        ///  replication operation.  Section, which gives the sequencing
        ///  assumptions associated with this method, specifies
        ///  using the InitializeFileTransferAsync method of the
        ///  FrsTransport interface to create a serverContext prior
        ///  to using this method.
        /// </param>
        /// <param name="dataBuffer">
        ///  The file data received from the server. The server
        ///  MUST return at least 4 bytes, the data stream header.
        /// </param>
        /// <param name="bufferSize">
        ///  The size, in bytes, of dataBuffer. The bufferSize MUST
        ///  be at least XPRESS_RDC_MIN_GET_DATA_BUFFER_SIZE_WITH_FILE_HEADER
        ///  bytes.
        /// </param>
        /// <param name="sizeReturned">
        ///  The size, in bytes, of the file data returned in dataBuffer.
        /// </param>
        uint RdcGetFileData(System.IntPtr bindingHandle, System.IntPtr serverContext, [Inline()] [Length("*sizeReturned")] [Size("bufferSize")] out byte[] dataBuffer, uint bufferSize, out System.UInt32? sizeReturned);

        /// <summary>
        ///  The RdcClose method informs the server that the server
        ///  context information can be released. Opnum: 12 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        
        uint RdcClose(ref System.IntPtr bindingHandle);//, ref System.IntPtr? serverContext);

        /// <summary>
        ///  The InitializeFileTransferAsync method is used by a
        ///  client to start a file download. The client supplies
        ///  an update to specify which file to download. The server
        ///  provides its latest version of the update and initial
        ///  file contents. Opnum: 13 
        /// </summary>
        /// <param name="AsyncHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="connectionId">
        ///  The GUID (as specified in [MS-DTYP] section) of the
        ///  connection ID that represents a specific replication
        ///  partnership.
        /// </param>
        /// <param name="frsUpdate">
        ///   					The FRS_UPDATE structure that contains information
        ///  about the file being replicated. The fields for the
        ///  UID in frsUpdate must be set to the UID of the file
        ///  to be downloaded.  All other fields MAY have arbitrary
        ///  values.  On return, all fields of frsUpdate MUST contain
        ///  the values that are held by the server. frsUpdate comprises
        ///  either of the values provided by a server in the response
        ///  to a RequestUpdates call, or all fields are cleared
        ///  (zeroed out).
        /// </param>
        /// <param name="rdcDesired">
        ///  The value is TRUE if Remote Differential Compression
        ///  (RDC) should be used when replicating this file; otherwise,
        ///  the value is FALSE.
        /// </param>
        /// <param name="stagingPolicy">
        ///  The FRS_REQUESTED_STAGING_POLICY enumeration value that
        ///  indicates the type of staging requested.
        /// </param>
        /// <param name="serverContext">
        ///  The context handle that represents the requested file
        ///  replication operation.
        /// </param>
        /// <param name="rdcFileInfo">
        ///  The FRS_RDC_FILEINFO structure that describes the file
        ///  whose replication is in progress.
        /// </param>
        /// <param name="dataBuffer">
        ///  The file data received from the server.
        /// </param>
        /// <param name="bufferSize">
        ///  The size, in bytes, of dataBuffer.  CONFIG_TRANSPORT_MAX_BUFFER_SIZE
        ///  is 262144.
        /// </param>
        /// <param name="sizeRead">
        ///  The size, in bytes, of the file data returned in dataBuffer.
        /// </param>
        /// <param name="isEndOfFile">
        ///  The value is 					TRUE if the end of the specified file
        ///  has been reached and there is no more file data to
        ///  replicate to the client; otherwise, the value is FALSE.The
        ///  server returns information about the file currently
        ///  being replicated, and the first buffer of data from
        ///  that file (if any).
        /// </param>
        uint InitializeFileTransferAsync(System.IntPtr AsyncHandle, System.IntPtr bindingHandle, System.Guid connectionId, ref _FRS_UPDATE frsUpdate, int rdcDesired, ref _FRS_REQUESTED_STAGING_POLICY stagingPolicy, out System.IntPtr? serverContext, [StaticSize(1, StaticSizeMode.Elements)] out _FRS_RDC_FILEINFO[] rdcFileInfo, [Inline()] [Length("*sizeRead")] [Size("bufferSize")] out byte[] dataBuffer, uint bufferSize, out System.UInt32? sizeRead, out System.Int32? isEndOfFile);

        /// <summary>
        ///  The InitializeFileDataTransfer call is used by a client
        ///  to initialize a partial file transfer. The file data
        ///  is transferred over the wire in a format that is composed
        ///  of a layering:The main data stream of a file, starting
        ///  from the offset specified in the argument and extending
        ///  to the length requested by the caller. An encapsulation
        ///  of the file stream using the format generated by the
        ///  compression utility, as specified in [MS-DRSR]. Opnum
        ///  : 14 
        /// </summary>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="connectionId">
        ///  The GUID (as specified in [MS-DTYP] section) of the
        ///  connection ID that represents a specific replication
        ///  partnership.
        /// </param>
        /// <param name="frsUpdate">
        ///  The FRS_UPDATE structure containing information about
        ///  the file being replicated.
        /// </param>
        /// <param name="serverContext">
        ///  The context handle that represents the requested file
        ///  replication operation.
        /// </param>
        /// <param name="offset">
        ///  The file offset of requested data.
        /// </param>
        /// <param name="length">
        ///  The length of requested data.
        /// </param>
        /// <param name="dataBuffer">
        ///  The file data received from the server.
        /// </param>
        /// <param name="bufferSize">
        ///  The size, in bytes, of dataBuffer.
        /// </param>
        /// <param name="sizeRead">
        ///  The size, in bytes, of the file data returned in dataBuffer.
        /// </param>
        /// <param name="isEndOfFile">
        ///  The value is TRUE if the end of the specified file has
        ///  been reached and there is no more file data to replicate
        ///  to the client; otherwise, the value is FALSE.
        /// </param>
        uint InitializeFileDataTransfer(System.IntPtr bindingHandle, System.Guid connectionId, ref _FRS_UPDATE frsUpdate, out System.IntPtr? serverContext, ulong offset, ulong length, [Inline()] [Length("*sizeRead")] [Size("bufferSize")] out byte[] dataBuffer, uint bufferSize, out System.UInt32? sizeRead, out System.Int32? isEndOfFile);

        /// <summary>
        ///  The RawGetFileDataAsync method is used instead of calling
        ///  RawGetFileData multiple times to obtain file data.
        ///   As specified in [MS-RPCE], the specification for asynchronous
        ///  RPC, an RPC client pulls file data from the byte pipe
        ///  until receiving an end-of-file notification from the
        ///  pipe. Opnum: 15 
        /// </summary>
        /// <param name="AsyncHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="serverContext">
        ///  The context handle that represents the requested file
        ///  replication operation.
        /// </param>
        /// <param name="bytePipe">
        ///  The asynchronous RPC byte pipe that contains returned
        ///  file data.
        /// </param>
        uint RawGetFileDataAsync(System.IntPtr AsyncHandle, System.IntPtr bindingHandle, System.IntPtr serverContext, out System.Byte? bytePipe);

        /// <summary>
        ///  The RdcGetFileDataAsync method is used instead of calling
        ///  RawGetFileData multiple times to obtain file data.
        ///   As specified in [MS-RPCE], the specification for
        ///  asynchronous RPC, an RPCclient pulls file data from
        ///  the byte pipe until receiving an end-of-file notification
        ///  from the pipe. Opnum: 16 
        /// </summary>
        /// <param name="AsyncHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="bindingHandle">
        ///  Inserted from configuration
        /// </param>
        /// <param name="serverContext">
        ///  The context handle that represents the requested file
        ///  replication operation. 
        /// </param>
        /// <param name="bytePipe">
        ///  The asynchronous RPC byte pipe that contains returned
        ///  file data.
        /// </param>
        uint RdcGetFileDataAsync(System.IntPtr AsyncHandle, System.IntPtr bindingHandle, System.IntPtr serverContext, out System.Byte? bytePipe);
    }
}
    
