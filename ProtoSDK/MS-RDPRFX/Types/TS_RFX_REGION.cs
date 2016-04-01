// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_REGION message contains information about the list of change rectangles
    /// on the screen for a specific channel.
    /// It also specifies the number of trailing TS_RFX_TILESET (section 2.2.2.3.4) messages.
    /// </summary>
    public struct TS_RFX_REGION : IMarshalable
    {
        /// <summary>
        /// The blockType field MUST be set to WBT_REGION (0xCCC6),
        /// and the codecId field MUST be set to 0x01.
        /// </summary>
        public TS_RFX_CODEC_CHANNELT CodecChannelT;

        /// <summary>
        /// Specifies whether this is the last region in a frame. This field must be set 0x01.
        /// </summary>
        public byte regionFlags;

        /// <summary>
        /// Specifies the number of TS_RFX_RECT (section 2.2.2.1.6) structures present in the rects field.
        /// </summary>
        public ushort numRects;

        /// <summary>
        /// A variable-length array of TS_RFX_RECT (section 2.2.2.1.6)_structures.
        /// This array defines the region.
        /// The number of rectangles in the array is specified in the numRects field.
        /// </summary>
        public TS_RFX_RECT[] rects;

        /// <summary>
        /// A 16-bit, unsigned integer. Specifies the region type. This field MUST be set to CBT_REGION (0xCAC1).
        /// </summary>
        public ushort regionType;

        /// <summary>
        /// Specifies the number of TS_RFX_TILESET (section 2.2.2.3.4) messages following
        /// this TS_RFX_REGION message. This field MUST be set to 0x0001.
        /// </summary>
        public ushort numTilesets;

        public byte[] ToBytes()
        {
            System.Collections.Generic.List<byte> dataBuffer = new System.Collections.Generic.List<byte>();
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)this.CodecChannelT.blockType));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.CodecChannelT.blockLen));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.codecId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.channelId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.regionFlags));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.numRects));
            foreach (TS_RFX_RECT rect in this.rects)
            {
                dataBuffer.AddRange(TypeMarshal.ToBytes<TS_RFX_RECT>(rect));
            }
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.regionType));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.numTilesets));
            return dataBuffer.ToArray();
        }
    }
}
