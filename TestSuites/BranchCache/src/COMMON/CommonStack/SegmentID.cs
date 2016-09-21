// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    /// <summary>
    /// Segment ID structure.
    /// </summary>
    public struct SegmentIDStructure
    {
        /// <summary>
        /// Size, in bytes of the first SegmentID, immediately subsequent to this field. 
        /// Implementations MUST support all allowed SegmentID lengths, and MUST support 
        /// content with 32-byte SegmentIDs.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public uint SizeOfSegmentID;

        /// <summary>
        /// Public Segment Identifier for the first target segment of content (also known 
        /// as HoHoDk). See [MS-PCCRC] section 2.2 for a description of segment identifiers.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        [Size("SizeOfSegmentID")]
        public byte[] SegmentID;

        /// <summary>
        /// Sequence of bytes added (as needed) to restore 4-byte alignment, relative to 
        /// the beginning of this message. The value of each byte MUST be set to zero. 
        /// This field is 0 to 3 bytes in length, as required.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        [Size("SizeOfSegmentID % 4 == 0 ? 0 : 4- SizeOfSegmentID % 4")]
        public byte[] ZeroPad;

        /// <summary>
        /// Creates an instance of SegmentIDStructure class.
        /// </summary>
        /// <param name="segmentID">Segment ID.</param>
        public SegmentIDStructure(byte[] segmentID)
        {
            SizeOfSegmentID = (uint)segmentID.Length;
            SegmentID = segmentID;
            ZeroPad = new byte[SizeOfSegmentID % 4 == 0 ? 0 : 4 - SizeOfSegmentID % 4];
        }
    }
}
