// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

    /// <summary>
    /// Indicate which hash algorithm to use.
    /// </summary>
    public enum dwHashAlgo_Values : int
    {
        /// <summary>
        /// Default value.
        /// </summary>
        none,

        /// <summary>
        ///  Use the SHA-256 hash algorithm.
        /// </summary>
        SHA256 = 0x0000800C,

        /// <summary>
        ///  Use the SHA-384 hash algorithm.
        /// </summary>
        SHA384 = 0x0000800D,

        /// <summary>
        ///  Use the SHA-512 hash algorithm.
        /// </summary>
        SHA512 = 0x0000800E
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
    public partial struct Content_Information_Data_Structure
    {
        /// <summary>
        ///  Content Information version (0x0100 is version 1.0).
        ///  The low byte is the minor version number and the high
        ///  byte is the major version number. MUST be 0x0100.
        /// </summary>
        public ushort Version;

        /// <summary>
        ///  Hash algorithm to use. MUST be one of the following
        ///  values:0x0000800C,0x0000800D,0x0000800E.
        /// </summary>
        public dwHashAlgo_Values dwHashAlgo;

        /// <summary>
        ///  Number of bytes into the first segment within the Content
        ///  Information data structure at which the content range
        ///  begins.
        /// </summary>
        public uint dwOffsetInFirstSegment;

        /// <summary>
        ///  Total number of bytes of the content range which lie
        ///  within the final segment in the Content Information
        ///  structure.
        /// </summary>
        public uint dwReadBytesInLastSegment;

        /// <summary>
        ///  The number of segments which intersect the content range
        ///  and hence are contained in the Content Information
        ///  structure.
        /// </summary>
        public uint cSegments;

        /// <summary>
        ///  Segment start offset, length, block size, Segment Hash
        ///  of Data and Segment Secret for each segment.  Each
        ///  segment description is as specified in .
        /// </summary>
        public SegmentDescription[] segments;

        /// <summary>
        ///  Count of blocks and content block hashes for each block
        ///  intersecting the content range for each segment in
        ///  the Content Information structure.  Each set of blocks
        ///  for a segment is as specified in .
        /// </summary>
        public SegmentContentBlocks[] blocks;

        public byte[] GetSegmentId(int segmentIndex)
        {
            return PccrcUtility.GetHoHoDkBytes(
                            segments[segmentIndex].SegmentSecret,
                            segments[segmentIndex].SegmentHashOfData,
                            dwHashAlgo);
        }

        /// <summary>
        /// Convert the structure to byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        public byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();
            byteList.Add((byte)this.Version);
            byteList.Add((byte)(this.Version >> 8));

            byteList.Add((byte)this.dwHashAlgo);
            byteList.Add((byte)((uint)this.dwHashAlgo >> 8));
            byteList.Add((byte)((uint)this.dwHashAlgo >> 16));
            byteList.Add((byte)((uint)this.dwHashAlgo >> 24));

            byteList.Add((byte)this.dwOffsetInFirstSegment);
            byteList.Add((byte)(this.dwOffsetInFirstSegment >> 8));
            byteList.Add((byte)(this.dwOffsetInFirstSegment >> 16));
            byteList.Add((byte)(this.dwOffsetInFirstSegment >> 24));

            byteList.Add((byte)this.dwReadBytesInLastSegment);
            byteList.Add((byte)(this.dwReadBytesInLastSegment >> 8));
            byteList.Add((byte)(this.dwReadBytesInLastSegment >> 16));
            byteList.Add((byte)(this.dwReadBytesInLastSegment >> 24));

            byteList.Add((byte)this.cSegments);
            byteList.Add((byte)(this.cSegments >> 8));
            byteList.Add((byte)(this.cSegments >> 16));
            byteList.Add((byte)(this.cSegments >> 24));

            for (int i = 0; i < this.segments.Length; i++)
            {
                byteList.AddRange(this.segments[i].ToByteArray());
            }

            for (int i = 0; i < this.blocks.Length; i++)
            {
                byteList.AddRange(this.blocks[i].ToByteArray(this.dwHashAlgo));
            }

            return byteList.ToArray();
        }
    }

    public partial struct Content_Information_Data_Structure_V2
    {
        /// <summary>
        /// Content Information minor version.  MUST be 0x00.
        /// </summary>
        public byte bMinorVersion;

        /// <summary>
        /// Content Information major version.  MUST be 0x02.
        /// </summary>
        public byte bMajorVersion;

        /// <summary>
        ///  Hash algorithm to use.
        /// </summary>
        public dwHashAlgoV2_Values dwHashAlgo;

        /// <summary>
        /// The byte offset in the content at which the start of the first 
        /// segment in the Content Information begins.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ulong ullStartInContent;

        /// <summary>
        /// The zero-based index in the content of the first segment in 
        /// the Content Information.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ulong ullIndexOfFirstSegment;

        /// <summary>
        ///  Number of bytes into the first segment within the Content
        ///  Information data structure at which the content range
        ///  begins.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint dwOffsetInFirstSegment;

        /// <summary>
        /// The length of the content range.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ulong ullLengthOfRange;

        /// <summary>
        /// Chunk type, chunk data length, and chunk data for each 
        /// chunk of Content Information.
        /// </summary>
        [StaticSize(0)]
        public ChunkDescription[] chunks;

        public int GetBlockCount()
        {
            int total = 0;
            for (int i = 0; i < chunks.Length; i++)
            {
                total += chunks[i].chunkData.Length;
            }
            return total;
        }

        public byte[] GetSegmentId(int chunkIndex, int segmentIndex)
        {
            return PccrcUtility.GetHoHoDkBytes(
                            chunks[chunkIndex].chunkData[segmentIndex].SegmentSecret,
                            chunks[chunkIndex].chunkData[segmentIndex].SegmentHashOfData,
                            dwHashAlgo);
        }

        /// <summary>
        /// Convert the structure to byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        public byte[] ToByteArray()
        {
            return null;
        }
    }

    public enum dwHashAlgoV2_Values : byte
    {
        TRUNCATED_SHA512 = 0x04
    }

    public struct ChunkDescription
    {
        /// <summary>
        /// Identifies the type and format of chunk data. MUST be set to 0x00.
        /// </summary>
        public byte bChunkType;

        /// <summary>
        /// Length of the chunk data, in bytes.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint dwChunkDataLength;

        /// <summary>
        /// One or more segment descriptions.
        /// </summary>
        [Size("dwChunkDataLength / 68")]
        public SegmentDescriptionV2[] chunkData;
    }

    /// <summary>
    /// Each SegmentDescription describes a single segment. Each segment 
    /// description chunk MUST contain at least one segment description. 
    /// A segment description chunk can contain multiple segment descriptions.
    /// Segments MUST be listed in the content information in the same order 
    /// as they appear in the content.
    /// </summary>
    public struct SegmentDescriptionV2
    {
        /// <summary>
        /// Total number of bytes in the segment, regardless of how many 
        /// of those bytes intersect the content range.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint cbSegment;

        /// <summary>
        /// The hash of the segment. The hash is of length 32 bytes if bHashAlgo 
        /// at the start of the Content Information was 0x01 = SHA-256.
        /// </summary>
        [StaticSize(32)]
        public byte[] SegmentHashOfData;

        /// <summary>
        /// Kp (see section 2.2), computed as Hash (SegmentHashofData + ServerSecret) 
        /// using the hash algorithm specified at the beginning of the 
        /// Content Information Data Structure. The hash is of length 32 bytes if 
        /// bHashAlgo at the start of the Content Information was 0x01 = SHA-256.
        /// </summary>
        [StaticSize(32)]
        public byte[] SegmentSecret;
    }

    /// <summary>
    ///  The blocks field contains a number cSegments of SegmentContentBlocks
    ///  fields. The Nth SegmentContentBlocks field corresponds
    ///  to the Nth SegmentDescription and hence the Nth content
    ///  segment. The SegmentContentBlocks field is formatted
    ///  as follows.
    /// </summary>
    public partial struct SegmentContentBlocks
    {
        /// <summary>
        ///  Number of content blocks in the segment which intersect
        ///  the content range specified at the start of the content
        ///  information.
        /// </summary>
        public uint cBlocks;

        /// <summary>
        ///  SHA-256, SHA-384 or SHA-512 hash of each content block
        ///  in the order in which the blocks appear in the segment.
        ///  The size of this field is cBlocks * (32, 48 or 64,
        ///  depending on which hash was used).
        /// </summary>
        public byte[] BlockHashes;

        /// <summary>
        /// Convert the structure to byte array.
        /// </summary>
        /// <param name="algo">The hash algorithm to use.</param>
        /// <returns>The byte array.</returns>
        public byte[] ToByteArray(dwHashAlgo_Values algo)
        {
            List<byte> byteList = new List<byte>();

            byteList.Add((byte)this.cBlocks);
            byteList.Add((byte)(this.cBlocks >> 8));
            byteList.Add((byte)(this.cBlocks >> 16));
            byteList.Add((byte)(this.cBlocks >> 24));

            byteList.AddRange(this.BlockHashes);

            return byteList.ToArray();
        }
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
    public partial struct SegmentDescription
    {
        /// <summary>
        ///  Content offset at which the start of the segment begins.
        /// </summary>
        public ulong ullOffsetInContent;

        /// <summary>
        ///  Total number of bytes in the segment, regardless of
        ///  how many of those bytes intersect the content range.
        /// </summary>
        public uint cbSegment;

        /// <summary>
        ///  Length of a content block within this segment, in bytes.
        ///  Every segment MUST use the same block size, which MUST
        ///  be 65536 bytes.
        /// </summary>
        public uint cbBlockSize;

        /// <summary>
        ///  The hash of the content block hashes of every block
        ///  in the segment, regardless of how many of those blocks
        ///  intersect the content range. The hash is of length
        ///  32 if dwHashAlgo at the start of the content information
        ///  was 0x800C = SHA-256, 48 if dwHashAlgo = 0x800D = SHA-384
        ///  or 64 if dwHashAlgo = 0x800E = SHA-512.
        /// </summary>
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
        public byte[] SegmentSecret;

        public uint BlockCount
        {
            get
            {
                return (cbSegment + cbBlockSize - 1) / cbBlockSize;
            }
        }

        /// <summary>
        /// Convert the structure to byte array.
        /// </summary>
        /// <returns>The byte array.</returns>
        public byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();

            byteList.Add((byte)this.ullOffsetInContent);
            byteList.Add((byte)(this.ullOffsetInContent >> 8));
            byteList.Add((byte)(this.ullOffsetInContent >> 16));
            byteList.Add((byte)(this.ullOffsetInContent >> 24));
            byteList.Add((byte)(this.ullOffsetInContent >> 32));
            byteList.Add((byte)(this.ullOffsetInContent >> 40));
            byteList.Add((byte)(this.ullOffsetInContent >> 48));
            byteList.Add((byte)(this.ullOffsetInContent >> 56));

            byteList.Add((byte)this.cbSegment);
            byteList.Add((byte)(this.cbSegment >> 8));
            byteList.Add((byte)(this.cbSegment >> 16));
            byteList.Add((byte)(this.cbSegment >> 24));

            byteList.Add((byte)this.cbBlockSize);
            byteList.Add((byte)(this.cbBlockSize >> 8));
            byteList.Add((byte)(this.cbBlockSize >> 16));
            byteList.Add((byte)(this.cbBlockSize >> 24));

            byteList.AddRange(this.SegmentHashOfData);

            byteList.AddRange(this.SegmentSecret);

            return byteList.ToArray();
        }
    }
}

