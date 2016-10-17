// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    /// <summary>
    /// Segment descriptor structure.
    /// </summary>
    public struct SegmentDescriptor
    {
        /// <summary>
        /// BlockSize (4 bytes):  Size of each block in the segment.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint BlockSize;

        /// <summary>
        /// SegmentSize (4 bytes):  Size of data in the segment.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint SegmentSize;

        /// <summary>
        /// SizeOfContentTag (2 bytes):  Size of data in the ContentTag field.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ushort SizeOfContentTag;

        /// <summary>
        /// ContentTag (variable):  Content tag; the size of this field is 
        /// indicated by the value of the SizeOfContentTag field.
        /// </summary>
        [Size("SizeOfContentTag")]
        public byte[] ContentTag;

        /// <summary>
        /// HashAlgorithm (1 byte):  The hashing algorithm ID, 1 ?SHA 256
        /// </summary>
        public byte HashAlgorithm;

        /// <summary>
        /// SegmentHoHoDk (variable):  Segment identifier; the size of 
        /// the HoHoDk is indicated by the hashing algorithm ID.
        /// </summary>
        [StaticSize(32)]
        public byte[] SegmentHoHoDk;
    }
}
