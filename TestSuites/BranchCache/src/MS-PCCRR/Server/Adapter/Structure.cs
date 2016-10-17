// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Protocols.TestTools.Messages.Marshaling;

    /// <summary>
    /// The type of content in server. Used in sut-client.
    /// </summary>
    public enum CONTENTTYPE
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
    /// Indicate which hash algorithm to use.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue",
        Justification = @"According to the technical document, the DWHashAlgoValues don't have zero.")]
    public enum DWHashAlgoValues : int
    {
        /// <summary>
        ///  Use the SHA-256 hash algorithm.
        /// </summary>
        V1 = 0x0000800C,

        /// <summary>
        ///  Use the SHA-384 hash algorithm.
        /// </summary>
        V2 = 0x0000800D,

        /// <summary>
        ///  Use the SHA-512 hash algorithm.
        /// </summary>
        V3 = 0x0000800E,
    }

    /// <summary>
    /// Encryption algorithm.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32",
        Justification = @"According to the technical document, the CryptoAlgoIdValues is an unsigned 32-bit field.")]
    public enum CryptoAlgoIdValues : uint
    {
        /// <summary>
        ///  No encryption.
        /// </summary>
        V1 = 0x00000000,

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
    /// Indicate which hash algorithm to use.
    /// </summary>
    [SuppressMessage("Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue",
        Justification = @"According to the technical document, the DwHashAlgo_Values don't have zero.")]
    public enum DwHashAlgo_Values : int
    {
        /// <summary>
        ///  Use the SHA-256 hash algorithm.
        /// </summary>
        V1 = 0x0000800C,

        /// <summary>
        ///  Use the SHA-384 hash algorithm.
        /// </summary>
        V2 = 0x0000800D,

        /// <summary>
        ///  Use the SHA-512 hash algorithm.
        /// </summary>
        V3 = 0x0000800E,
    }

    /// <summary>
    ///  A BLOCK_RANGE is an array of two integers that defines
    ///  a consecutive array of blocks.
    /// </summary>
    public partial struct BLOCKRANGE
    {
        /// <summary>
        ///  The index of the first block in the range.
        /// </summary>
        public uint Index;

        /// <summary>
        ///  Count of consecutive adjacent blocks in that range,
        ///  including the block at the Index location. The value
        ///  of this field MUST be greater than 0.
        /// </summary>
        public uint Count;
    }

    /// <summary>
    ///  Content Information is a variable size data structure.
    ///  Content Information size is proportional to the length
    ///  of the content it represents. Content Information starts
    ///  with a single 2 byte WORD value representing the data
    ///  structure version. Version 1.0 of the Content Information
    ///  data structure is formatted as follows. All fields
    ///  are in host byte order.
    /// </summary>
    public partial struct ContentInformationDataStructure
    {
        /// <summary>
        ///  Content Information version (0x0100 is version 1.0).
        ///  The low byte is the minor version number and the high
        ///  byte is the major version number. MUST be 0x0100.
        /// </summary>
        public ushort Version;

        /// <summary>
        ///  Hash algorithm to use. MUST be one of the following
        ///  values:
        /// </summary>
        public DwHashAlgo_Values DWHashAlgo;

        /// <summary>
        ///  Number of bytes into the first segment within the Content
        ///  Information data structure at which the content range
        ///  begins.
        /// </summary>
        public uint DWOffsetInFirstSegment;

        /// <summary>
        ///  Total number of bytes of the content range which lie
        ///  within the final segment in the Content Information
        ///  structure.
        /// </summary>
        public uint DWReadBytesInLastSegment;

        /// <summary>
        ///  The number of segments which intersect the content range
        ///  and hence are contained in the Content Information
        ///  structure.
        /// </summary>
        public uint CSegments;

        /// <summary>
        ///  Segment start offset, length, block size, Segment Hash
        ///  of Data and Segment Secret for each segment.  Each
        ///  segment description is as specified in .
        /// </summary>
        public SegmentDescription[] Segments;

        /// <summary>
        ///  Count of blocks and content block hashes for each block
        ///  intersecting the content range for each segment in
        ///  the Content Information structure.  Each set of blocks
        ///  for a segment is as specified in .
        /// </summary>
        public SegmentContentBlocks[] Blocks;
    }

    /// <summary>
    ///  Content Information is a variable size data structure.
    ///  Content Information size is proportional to the length
    ///  of the content it represents. Content Information starts
    ///  with a single 2 byte WORD value representing the data
    ///  structure version. Version 1.0 of the Content Information
    ///  data structure is formatted as follows. All fields
    ///  are in host byte order.
    /// </summary>
    /// <remarks> 
    /// The remarks xml document
    /// .\NewTDxml\36171657-7f15-419e-973b-6612b1799117.xml
    /// </remarks>
    public partial struct SegmentInformation
    {
        /// <summary>
        ///  Content Information version (0x0100 is version 1.0).
        ///  The low byte is the minor version number and the high
        ///  byte is the major version number. MUST be 0x0100.
        /// </summary>
        public ushort Version;

        /// <summary>
        ///  Hash algorithm to use. MUST be one of the following
        ///  values:
        /// </summary>
        public DwHashAlgo_Values DwHashAlgo;

        /// <summary>
        ///  Number of bytes into the first segment within the Content
        ///  Information data structure at which the content range
        ///  begins.
        /// </summary>
        public uint DwOffsetInFirstSegment;

        /// <summary>
        ///  Total number of bytes of the content range which lie
        ///  within the final segment in the Content Information
        ///  structure.
        /// </summary>
        public uint DwReadBytesInLastSegment;

        /// <summary>
        ///  The number of segments which intersect the content range
        ///  and hence are contained in the Content Information
        ///  structure.
        /// </summary>
        public uint CSegments;

        /// <summary>
        ///  Segment start offset, length, block size, Segment Hash
        ///  of Data and Segment Secret for each segment.  Each
        ///  segment description is as specified in .
        /// </summary>
        [Size("csegments")]
        public SegmentDescription[] Segments;

        /// <summary>
        ///  Count of blocks and content block hashes for each block
        ///  intersecting the content range for each segment in
        ///  the Content Information structure.  Each set of blocks
        ///  for a segment is as specified in .
        /// </summary>
        [Size("csegments")]
        public SegmentContentBlocks[] Blocks;
    }

    /// <summary>
    ///  The blocks field contains a number cSegments of SegmentContentBlocks
    ///  fields. The Nth SegmentContentBlocks field corresponds
    ///  to the Nth SegmentDescription and hence the Nth content
    ///  segment. The SegmentContentBlocks field is formatted
    ///  as follows.
    /// </summary>
    /// <remarks>
    /// The remarks xml document
    /// .\NewTDxml\6aabffe5-f712-41fe-8f18-c992c9ba507e.xml
    /// </remarks>
    public partial struct SegmentContentBlocks
    {
        /// <summary>
        ///  Number of content blocks in the segment which intersect
        ///  the content range specified at the start of the content
        ///  information.
        /// </summary>
        public uint Cblocks;

        /// <summary>
        ///  SHA-256, SHA-384 or SHA-512 hash of each content block
        ///  in the order in which the blocks appear in the segment.
        ///  The size of this field is cBlocks * (32, 48 or 64,
        ///  depending on which hash was used).
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] BlockHashes;
    }

    /// <summary>
    ///  The segments field is composed of a number cSegments
    ///  of SegmentDescription fields. Each SegmentDescription
    ///  field corresponds to a content segment in the order
    ///  in which they appear in the original content. Every
    ///  segment except for the last segment must be exactly
    ///  32 MB in size. The content information data structure
    ///  defines the content range as described below. Content
    ///  range = {Start offset, Length}Start offset = ullOffsetInContent
    ///  + dwOffsetInFirstSegment, where ullOffsetInContent
    ///  is taken from the first SegmentDescription in the segments
    ///  field.Length =( Sum of cbSegment of all segments in
    ///  segments field except for the first segment and last
    ///  segment) + (cbSegment of first segment – dwOffsetInFirstSegment)
    ///  + dwReadBytesInLastSegmentThe content range extends
    ///  to the end of all the segments whose SegmentDescriptions
    ///  are included in the Content Information except for
    ///  the last segment, for which the number of bytes is
    ///  limited to dwReadBytesInLastSegment instead of the
    ///  total number of bytes actually present in the segment.
    /// </summary>
    /// <remarks>
    /// The remarks xml document
    /// .\NewTDxml\6aabffe5-f712-41fe-8f18-c992c9ba507e.xml
    /// </remarks>
    public partial struct SegmentDescription
    {
        /// <summary>
        ///  Content offset at which the start of the segment begins.
        /// </summary>
        public ulong UllOffsetInContent;

        /// <summary>
        ///  Total number of bytes in the segment, regardless of
        ///  how many of those bytes intersect the content range.
        /// </summary>
        public uint CbSegment;

        /// <summary>
        ///  Length of a content block within this segment, in bytes.
        ///  Every segment MUST use the same block size, which MUST
        ///  be 65536 bytes.
        /// </summary>
        public uint CbBlockSize;

        /// <summary>
        ///  The hash of the content block hashes of every block
        ///  in the segment, regardless of how many of those blocks
        ///  intersect the content range. The hash is of length
        ///  32 if dwHashAlgo at the start of the content information
        ///  was 0x800C = SHA-256, 48 if dwHashAlgo = 0x800D = SHA-384
        ///  or 64 if dwHashAlgo = 0x800E = SHA-512.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] SegmentHashOfData;

        /// <summary>
        ///  Kp (see section 2.2), computed as Hash(Segment Hash
        ///  of Data + server secret) using the hash algorithm specified
        ///  at the beginning of the Content Information data structure.
        ///  The hash is of length 32 if dwHashAlgo at the start
        ///  of the content information was 0x800C = SHA-256, 48
        ///  if dwHashAlgo = 0x800D = SHA-384 or 64 if dwHashAlgo
        ///  = 0x800E = SHA-512.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] SegmentSecret;
    }
}

