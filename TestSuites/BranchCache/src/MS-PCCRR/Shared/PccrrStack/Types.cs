// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

    /// <summary>
    /// Protocol type
    /// </summary>
    public enum ProtocolType
    {
        /// <summary>
        /// The type of protocol is PCCRD
        /// </summary>
        PCCRD,

        /// <summary>
        /// The type of protocol is PCCRR
        /// </summary>
        PCCRR,

        /// <summary>
        /// Http download data
        /// </summary>
        HttpDownload
    }

    /// <summary>
    /// The type of content in server. Used in sut-client.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// Three or fewer consecutive blocks on the server peer 1 or hosted-cache server
        /// </summary>
        ThreeOrFewerConsecutiveBlks,

        /// <summary>
        /// More than three consecutive blocks on the server peer 1 or hosted-cache server
        /// </summary>
        MoreThanThreeConsecutiveBlks,
    }

    /// <summary>
    /// The type of message in the message body.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32",
        Justification = @"According to the technical document, the MsgType_Values is an unsigned 32-bit field.")]
    public enum MsgType_Values : uint
    {
        /// <summary>
        ///  A protocol version negotiation request. The request
        ///  declares the minimum and maximum version numbers supported
        ///  by the requesting client-role peer.This message is
        ///  never sent by windows, but it is handled by the code
        ///  if received, by responding with an MSG_NEGO_RESP message.
        /// </summary>
        MSG_NEGO_REQ = 0x00000000,

        /// <summary>
        ///  A protocol version negotiation response. It is sent
        ///  in response to any protocol version negotiation request
        ///  or to any other request with protocol version not supported
        ///  by the server-role peer.The response declares the minimum
        ///  and maximum version numbers supported by the responding
        ///  server-role peer.
        /// </summary>
        MSG_NEGO_RESP = 0x00000001,

        /// <summary>
        ///  A request for a list of block IDs of blocks in the target
        ///  segment that are possessed by the destination server-role
        ///  peer (list expressed as a block range array), and intersecting
        ///  the list of block IDs specified in the request itself.
        /// </summary>
        MSG_GETBLKLIST = 0x00000002,

        /// <summary>
        ///  A request for an array of block IDs (specified by a
        ///  block range array). Since only one block will be returned,
        ///  a MSG_GETBLKS message SHOULD specify only a single
        ///  range containing only a single block).
        /// </summary>
        MSG_GETBLKS = 0x00000003,

        /// <summary>
        ///  A response message containing a list of block IDs of
        ///  blocks in the target segment that are possessed by
        ///  the destination server-role peer (list expressed as
        ///  a block range array), and intersecting the list of
        ///  block IDs specified in the previous request from the
        ///  client-role peer.
        /// </summary>
        MSG_BLKLIST = 0x00000004,

        /// <summary>
        ///  A response message containing the (first) actual block
        ///  requested by the client-role peer via a block range
        ///  array in a MSG_BLKLIST message.
        /// </summary>
        MSG_BLK = 0x00000005,

        // TODO: These two are not documented yet, will add comment later
        MSG_GETSEGLIST = 0x00000006,
        MSG_SEGLIST = 0x00000007,
    }

    /// <summary>
    /// Encryption algorithm.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32",
        Justification = @"According to the technical document, the CryptoAlgoId_Values is an unsigned 32-bit field.")]
    public enum CryptoAlgoId_Values : uint
    {
        /// <summary>
        ///  No encryption.
        /// </summary>
        NoEncryption = 0x00000000,

        /// <summary>
        ///  AES 128-bit encryption.
        /// </summary>
        AES_128 = 0x00000001,

        /// <summary>
        ///  AES 192-bit encryption.
        /// </summary>
        AES_192 = 0x00000002,

        /// <summary>
        ///  AES 256-bit encryption.
        /// </summary>
        AES_256 = 0x00000003,
    }

    /// <summary>
    ///  A BLOCK_RANGE is an array of two integers that defines
    ///  a consecutive array of blocks.
    /// </summary>
    public partial struct BLOCK_RANGE
    {
        /// <summary>
        ///  The index of the first block in the range.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint Index;

        /// <summary>
        ///  Count of consecutive adjacent blocks in that range,
        ///  including the block at the Index location. The value
        ///  of this field MUST be greater than 0.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        [PossibleValueRange("1", "4294967295")]
        public uint Count;
    }

    /// <summary>
    ///  The GetBlocks message contains a request for blocks
    ///  of content. It is used to retrieve a set of blocks
    ///  defined by a single BLOCK_ARRAY_RANGE.
    /// </summary>
    public partial struct MSG_GETBLKS
    {
        /// <summary>
        ///  Size in bytes of the subsequent SegmentID field.
        /// </summary>
        public uint SizeOfSegmentID;

        /// <summary>
        ///  Public Segment Identifier for the target segment of
        ///  content (also known as HoHoDk). See [MS-PCCRC] for
        ///  a description of contents, segments, blocks, and identifiers.
        /// </summary>
        [Size("SizeOfSegmentID")]
        public byte[] SegmentID;

        /// <summary>
        ///  Sequence of bytes added (as needed) to restore 4-byte
        ///  alignment, relative to the beginning of this message.
        ///  The value of each byte MUST be set to zero. This field
        ///  is 0 to 3 bytes in length, as required.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] ZeroPad;

        /// <summary>
        ///  Number of items in the subsequent block range array.
        /// </summary>
        public uint ReqBlockRangeCount;

        /// <summary>
        ///  Block range array representing the blocks requested
        ///  for the target segment. Since only the first block
        ///  listed in the array will be returned by the peer, a
        ///  GetBlocks message SHOULD specify one only range containing
        ///  one only block.
        /// </summary>
        [Size("ReqBlockRangeCount")]
        public BLOCK_RANGE[] ReqBlockRanges;

        /// <summary>
        ///  Size in bytes of the subsequent DataForVrfBlock field.
        ///  This field SHOULD be zero.
        /// </summary>
        public uint SizeOfDataForVrfBlock;

        /// <summary>
        ///  Not used by the protocol. This field SHOULD be empty.
        /// </summary>
        [Size("SizeOfDataForVrfBlock")]
        public byte[] DataForVrfBlock;
    }

    /// <summary>
    ///  The transport adds the following header in front of
    ///  response-type protocol messages:
    /// </summary>
    public partial struct TRANSPORT_RESPONSE_HEADER
    {
        /// <summary>
        ///  Total message size in bytes, excluding this field.
        /// </summary>
        public uint Size;
    }

    /// <summary>
    ///  The Negotiation Request message is a request for the
    ///  minimum and maximum protocol version supported by the
    ///  target server-role peer. The message contains the minimum
    ///  and maximum protocol version supported by the requesting
    ///  client-role peer.
    /// </summary>
    public partial struct MSG_NEGO_REQ
    {
        /// <summary>
        ///  Minimum protocol version supported by the requesting
        ///  peer. The protocol version is encoded identically to
        ///  the ProtVer field defined in section .
        /// </summary>
        public ProtoVersion MinSupportedProtocolVersion;

        /// <summary>
        ///  Maximum protocol version supported by the requesting
        ///  peer. The protocol version is encoded identically to
        ///  the ProtVer field defined in section .
        /// </summary>
        public ProtoVersion MaxSupportedProtocolVersion;
    }

    /// <summary>
    ///  All Retrieval Protocol messages are prefixed by a message
    ///  header.Messages can be one of two types: request-type
    ///  or response-type. Request-type messages initiate a
    ///  communication session between two peers. Response-type
    ///  messages are sent only on response to a Request-type
    ///  one (see Protocol Details section for more details).A
    ///  request-type message can be delivered only as an HTTP
    ///  request. A response-type message can be delivered only
    ///  as an HTTP response to an incoming HTTP request.The
    ///  layout of the message header is as follows.
    /// </summary>
    public partial struct MESSAGE_HEADER
    {
        /// <summary>
        ///  Protocol version number, formed by concatenating the
        ///  protocol major version number and protocol minor version
        ///  number, encoded as follows (where MSB is Most Significant
        ///  Byte and LSB is Least Significant Byte):1st Byte (Addr:
        ///  X)2nd Byte (Addr: X+1)3rd Byte (Addr: X+2)4th Byte
        ///  (Addr: X+3)Minor version MSBMinor version LSBMajor
        ///  version MSBMajor version LSBThe major version number
        ///  is encoded in the least significant word of the protocol
        ///  version's DWORD.The minor version number is encoded
        ///  in the most significant word of the protocol version's
        ///  DWORD.Currently, the protocol version number MUST be
        ///  set to {major=1 (0x0001), minor=0 (0x0000)}.
        /// </summary>
        public ProtoVersion ProtVer;

        /// <summary>
        ///  The type of message in the message body, expressed as
        ///  a binary integer. MUST be set to one of the following
        ///  values.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public MsgType_Values MsgType;

        /// <summary>
        ///  Protocol message total size including MESSAGE_HEADER,
        ///  but not including the Transport Header.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint MsgSize;

        /// <summary>
        ///  Encryption algorithm used by the server-role peer to
        ///  encrypt data. MUST be one of the following values.windows
        ///  uses AES_128 as the default encryption algorithm. windows
        ///  supports no encryption, AES_128 and AES_256. Refer
        ///  to [FIPS197] for the AES standard.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public CryptoAlgoId_Values CryptoAlgoId;
    }

    /// <summary>
    /// The protocol version.
    /// </summary>
    public struct ProtoVersion
    {
        /// <summary>
        /// Currently, the protocol version number MUST be set to 
        /// {minor=0}.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ushort MinorVersion;

        /// <summary>
        /// Currently, the protocol version number MUST be set to 
        /// {major=1}.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ushort MajorVersion;
    }

    /// <summary>
    ///  The GetBlockList message contains a request for a download
    ///  block list. It is used when retrieving a set of blocks
    ///  defined by one or more BLOCK_ARRAY_RANGE items.
    /// </summary>
    public partial struct MSG_GETBLKLIST
    {
        /// <summary>
        ///  Size in bytes of the subsequent SegmentID field.
        /// </summary>
        public uint SizeOfSegmentID;

        /// <summary>
        ///  Public Segment Identifier for the target segment of
        ///  content (also known as HoHoDk). See [MS-PCCRC] for
        ///  a description of contents, segments, blocks, and identifiers.
        /// </summary>
        [Size("SizeOfSegmentID")]
        public byte[] SegmentID;

        /// <summary>
        ///  Sequence of bytes added (as needed) to restore 4-byte
        ///  alignment, relative to the beginning of this message.
        ///  The value of each byte MUST be set to zero. This field
        ///  is 0 to 3 bytes in length, as required.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] ZeroPad;

        /// <summary>
        ///  Number of items in the subsequent block range array.
        /// </summary>
        public uint NeededBlocksRangeCount;

        /// <summary>
        ///  Block range array listing the block IDs of the blocks
        ///  within the target segment that the client-role peer
        ///  is interested in. The server-role peer will reply with
        ///  a block range array representing the intersection between
        ///  the list of block IDs in the NeededBlockRanges array
        ///  and the block range array set of blocks within the target
        ///  segment currently available for sharing in the local
        ///  cache of the server-role peer.
        /// </summary>
        [Size("NeededBlocksRangeCount")]
        public BLOCK_RANGE[] NeededBlockRanges;
    }

    /// <summary>
    ///  MSG_BLKLIST packet.
    /// </summary>
    public partial struct MSG_BLKLIST
    {
        /// <summary>
        ///  The size, in bytes, of the subsequent SegmentId field.
        /// </summary>
        public uint SizeOfSegmentId;

        /// <summary>
        ///  The Public Segment Identifier for the target segment
        ///  of content (also known as HoHoDk). See [MS-PCCRC] for
        ///  details.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] SegmentId;

        /// <summary>
        ///  A sequence of N bytes added (only as needed) to restore
        ///  4-byte alignment, where N between 0 to 3. Each byte's value
        ///  MUST be set to zero.
        /// </summary>
        [StaticSize(3, StaticSizeMode.Elements)]
        public byte[] ZeroPad;

        /// <summary>
        ///  Number of items in the subsequent block range array.
        ///  The server MUST set the BlockRangeCount field to 0
        ///  if it does not have any of the requested block ranges.
        /// </summary>
        public uint BlockRangeCount;

        /// <summary>
        ///  A block range array describing the blocks currently
        ///  available for download from the current server-role
        ///  peer for the target segment, within the boundaries
        ///  of the list of block ranges of interest (NeededBlockRanges)
        ///  specified by the client-role peer in the previously
        ///  received GetBlockList method request.
        /// </summary>
        [Size("BlockRangeCount")]
        public BLOCK_RANGE[] BlockRanges;

        /// <summary>
        ///  The index of the first block after the block sent in
        ///  the current message, currently available for download
        ///  from this server-role peer. If no such next block is
        ///  available, this index MUST be zero.
        /// </summary>
        public uint NextBlockIndex;
    }

    /// <summary>
    ///  MSG_BLK packet.
    /// </summary>
    public partial struct MSG_BLK
    {
        /// <summary>
        ///  The size, in bytes, of the subsequent SegmentId field.
        /// </summary>
        public uint SizeOfSegmentId;

        /// <summary>
        ///  The Public Segment Identifier for the target segment
        ///  of content (also known as HoHoDk). See [MS-PCCRC] for
        ///  details.
        /// </summary>
        [Size("SizeOfSegmentID")]
        public byte[] SegmentId;

        /// <summary>
        ///  A sequence of N bytes added (only as needed) to restore
        ///  4-byte alignment, where N is between 0 to 3. Each byte's value
        ///  MUST be set to zero.
        /// </summary>
        public byte[] ZeroPad;

        /// <summary>
        ///  The index in the target segment of the block sent in
        ///  the current message.
        /// </summary>
        public uint BlockIndex;

        /// <summary>
        ///  The index of the first block after the block sent in
        ///  the current message, currently available for download
        ///  from this server-role peer. If no such next block is
        ///  available, this index MUST be zero.
        /// </summary>
        public uint NextBlockIndex;

        /// <summary>
        ///  The size, in bytes, of the subsequent Block field. The
        ///  server MUST set the SizeOfBlock field to zero if it
        ///  does not have the requested block.
        /// </summary>
        public uint SizeOfBlock;

        /// <summary>
        ///  The actual block of data, encrypted according to the
        ///  cryptographic algorithm specified in the header of
        ///  the message itself, not including the initialization
        ///  vector.
        /// </summary>
        [Size("SizeOfBlock")]
        public byte[] Block;

        /// <summary>
        ///  A sequence of N bytes added (only as needed) to restore
        ///  4-byte alignment, where N is between 0 to 3. Each byte's value
        ///  MUST be set to zero.
        /// </summary>
        public byte[] ZeroPad2;

        /// <summary>
        ///  The size, in bytes, of the subsequent VrfBlock field,
        ///  which SHOULD be zero.
        /// </summary>
        public uint SizeOfVrfBlock;

        /// <summary>
        ///  Currently not used, and SHOULD be empty.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] VrfBlock;

        /// <summary>
        ///  A sequence of N bytes added (only as needed) to restore
        ///  4-byte alignment, where N is between 0 to 3. Each byte's value
        ///  MUST be set to zero.
        /// </summary>
        public byte[] ZeroPad3;

        /// <summary>
        ///  The size, in bytes, of the subsequent IVBlock field.
        /// </summary>
        public uint SizeOfIVBlock;

        /// <summary>
        ///  The initialization vector used by the server-role peer
        ///  when encrypting the block of data (Block field) sent
        ///  with this message.
        /// </summary>
        [Size("SizeOfIVBlock")]
        public byte[] IVBlock;
    }

    /// <summary>
    ///  The complete layout of a Request type Retrieval Protocol
    ///  Message sent over HTTP is as follows.
    /// </summary>
    public partial struct REQUEST_MESSAGE
    {
        /// <summary>
        ///  Message header.
        /// </summary>
        public MESSAGE_HEADER MESSAGEHEADER;

        /// <summary>
        ///  Message body, which contains either a GetBlockList or
        ///  GetBlocks request message.
        /// </summary>
        public object MESSAGEBODY;
    }

    /// <summary>
    ///  The Negotiation Response message is the response message
    ///  containing the minimum and maximum protocol version
    ///  supported by the responding server-role peer. The message
    ///  is sent in response to a Negotiation Request message
    ///  or to any other request message with a protocol version
    ///  not supported by the server-role peer.
    /// </summary>
    public partial struct MSG_NEGO_RESP
    {
        /// <summary>
        ///  Minimum protocol version supported by the requesting
        ///  peer. The protocol version is encoded identically to
        ///  the ProtVer field defined in section .
        /// </summary>
        public ProtoVersion MinSupporteProtocolVersion;

        /// <summary>
        ///  Maximum protocol version supported by the requesting
        ///  peer. The protocol version is encoded identically to
        ///  the ProtVer field defined in section .
        /// </summary>
        public ProtoVersion MaxSupporteProtocolVersion;
    }

    /// <summary>
    ///  The complete layout of a Response type Download Protocol
    ///  Message sent over HTTP is:
    /// </summary>
    public partial struct RESPONSE_MESSAGE
    {
        /// <summary>
        ///  Transport Response Header.
        /// </summary>
        public TRANSPORT_RESPONSE_HEADER TRANSPORTRESPONSEHEADER;

        /// <summary>
        ///  Message header.
        /// </summary>
        public MESSAGE_HEADER MESSAGEHEADER;

        /// <summary>
        ///  Message body, which may contain either a MSG_BLKLIST
        ///  or MSG_BLK message.
        /// </summary>
        public object MESSAGEBODY;
    }

    /// <summary>
    /// The MSG_GETSEGLIST (GetSegmentList) message contains a request for a 
    /// download segment list. It is used when retrieving a set of segments. 
    /// </summary>
    public struct MSG_GETSEGLIST
    {
        /// <summary>
        /// Unique identifier among all outstanding GetSegmentList Requests from this peer.
        /// </summary>
        public Guid RequestID;

        /// <summary>
        /// Count of the Segment IDs in the current GetSegmentList Request.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint CountOfSegmentIDs;

        /// <summary>
        /// Segment ID list.
        /// </summary>
        [Size("CountOfSegmentIDs")]
        public SegmentIDStructure[] SegmentIDs;

        /// <summary>
        /// Size, in bytes, of the ExtensibleBlob field. Implementations MAY support 
        /// extensible blobs in MSG_GETSEGLIST messages. Implementations that do not 
        /// support extensible blobs in MSG_GETSEGLIST messages MUST set SizeOfExtensibleBlob 
        /// to zero and omit the ExtensibleBlob field.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint SizeOfExtensibleBlob;

        /// <summary>
        /// An extensible blob. See Extensible Blob (section 2.2.6) for the definition 
        /// of currently defined extensible BLOBS. Implementations MAY support extensible 
        /// blobs in MSG_GETSEGLIST messages. Implementations that do not support extensible 
        /// blobs in MSG_GETSEGLIST messages MUST set SizeOfExtensibleBlob to zero and omit 
        /// the ExtensibleBlob field. Relative indexes contained in the extensible blob are 
        /// relative to the first segment in the first SegmentRange carried by the current 
        /// MSG_GETSEGLIST message. 
        /// </summary>
        [Size("SizeOfExtensibleBlob")]
        public byte[] ExtensibleBlob;
    }

    /// <summary>
    /// The MSG_SEGLIST message is the response message containing the 
    /// segment range array describing the segments currently available 
    /// for download. This message is sent by the server-role peer in 
    /// response to a MSG_GETSEGLIST message from a requesting client-role peer.
    /// </summary>
    public struct MSG_SEGLIST
    {
        /// <summary>
        /// Unique identifier among all outstanding GetSegmentList Requests from this peer.
        /// </summary>
        public Guid RequestID;

        /// <summary>
        /// Number of items in the subsequent segment range array. The server MUST set the 
        /// SegmentRangeCount field to 0 if it does not have any of the requested segments.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint SegmentRangeCount;

        /// <summary>
        /// A segment range array describing the segments (full or partial) currently 
        /// available for download from the current server-role peer, The indexes specified 
        /// in each range in the response are the relative indexes of the segment in the 
        /// original array of segment IDs specified in the associated GetSegmentList message.
        /// </summary>
        [Size("SegmentRangeCount")]
        public BLOCK_RANGE[] SegmentRanges;

        /// <summary>
        /// Size, in bytes, of the ExtensibleBlob field. Implementations MAY support 
        /// extensible blobs in MSG_GETSEGLIST messages. Implementations that do not 
        /// support extensible blobs in MSG_GETSEGLIST messages MUST set SizeOfExtensibleBlob 
        /// to zero and omit the ExtensibleBlob field.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint SizeOfExtensibleBlob;

        /// <summary>
        /// An extensible blob. See Extensible Blob (section 2.2.6) for the definition 
        /// of currently defined extensible BLOBS. Implementations MAY support extensible 
        /// blobs in MSG_GETSEGLIST messages. Implementations that do not support extensible 
        /// blobs in MSG_GETSEGLIST messages MUST set SizeOfExtensibleBlob to zero and omit 
        /// the ExtensibleBlob field. Relative indexes contained in the extensible blob are 
        /// relative to the first segment in the first SegmentRange carried by the current 
        /// MSG_GETSEGLIST message. 
        /// </summary>
        [Size("SizeOfExtensibleBlob")]
        public byte[] ExtensibleBlob;
    }

    /// <summary>
    /// The Extensible Blob Version 1 MUST be formatted as follows.
    /// </summary>
    public struct ExtensibleBlobVersion1
    {
        /// <summary>
        /// Network-byte-order unsigned short integer that contains the version of the 
        /// extensible blob. It must be equal to 1.
        /// </summary>
        public ushort ExtensibleBlobVersion;

        /// <summary>
        /// Unit used to specify the age of the segments in the following ENCODED_SEGMENT_AGE 
        /// structures. 
        /// </summary>
        public byte SegmentAgeUnits;

        /// <summary>
        /// Count of ENCODED_SEGMENT_AGE structures encoded right after this field 
        /// (acceptable range: 0 - 255).
        /// </summary>
        public byte SegmentAgeCount;

        /// <summary>
        /// SegmentAgeCount ENCODED_SEGMENT_AGE structures.
        /// </summary>
        [Size("SegmentAgeCount")]
        public EncodedSegmentAge[] SegmentAges;

        public byte[] ToBytes()
        {
            byte[] buffer = TypeMarshal.ToBytes<ExtensibleBlobVersion1>(this);
            byte temp = buffer[0];
            buffer[0] = buffer[1];
            buffer[1] = temp;

            return buffer;
        }

        public void FromBytes(byte[] data)
        {
            byte[] buffer = new byte[data.Length];
            Array.Copy(data, buffer, data.Length);
            byte temp = buffer[0];
            buffer[0] = buffer[1];
            buffer[1] = temp;

            this = TypeMarshal.ToStruct<ExtensibleBlobVersion1>(buffer);
        }
    }

    /// <summary>
    /// An ENCODED_SEGMENT_AGE is an array of four bytes that describes the age of a segment of 
    /// data involved in a Peer Content Caching and Retrieval: Retrieval Protocol message exchange. 
    /// The age refers to the amount of time that has elapsed since the specified segment was 
    /// first created in the local BranchCache cache.
    /// </summary>
    public struct EncodedSegmentAge
    {
        /// <summary>
        /// Index of a segment among all of the segments involved in the current message exchange. 
        /// The index is relative to the first segment addressed in the message containing the 
        /// specific current ENCODED_SEGMENT_AGE structure.
        /// </summary>
        public byte SegmentIndex;

        /// <summary>
        /// Low part of the age of the segment.
        /// </summary>
        public byte SegmentAgeLowPart;

        /// <summary>
        /// Mid part of the age of the segment.
        /// </summary>
        public byte SegmentAgeMidPart;

        /// <summary>
        /// High part of the age of the segment.
        /// </summary>
        public byte SegmentAgeHighPart;

        /// <summary>
        /// Gets the age of the segment.
        /// </summary>
        public uint SegmentAge
        {
            get
            {
                return (uint)(SegmentAgeLowPart + 256 * SegmentAgeMidPart + 256 * 256 * SegmentAgeHighPart);
            }
        }
    }
}

