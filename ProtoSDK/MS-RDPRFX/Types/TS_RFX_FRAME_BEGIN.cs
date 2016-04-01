// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_FRAME_BEGIN message indicates the start of a new frame
    /// for a specific channel in the encoded stream.
    /// </summary>
    public struct TS_RFX_FRAME_BEGIN : IMarshalable
    {
        /// <summary>
        /// The blockType field MUST be set to WBT_FRAME_BEGIN (0xCCC4).
        /// </summary>
        public TS_RFX_CODEC_CHANNELT CodecChannelT;

        /// <summary>
        /// Specifies the index of the frame in the current video sequence.
        /// </summary>
        public uint frameIdx;

        /// <summary>
        /// Specifies the number of TS_RFX_REGION (section 2.2.2.3.3) messages
        /// following this TS_RFX_FRAME_BEGIN message.
        /// That is, the number of regions in the frame.
        /// </summary>
        public short numRegions;

        public byte[] ToBytes()
        {
            System.Collections.Generic.List<byte> dataBuffer = new System.Collections.Generic.List<byte>();
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)this.CodecChannelT.blockType));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.CodecChannelT.blockLen));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.codecId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.channelId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.frameIdx));
            dataBuffer.AddRange(TypeMarshal.ToBytes<short>(this.numRegions));
            return dataBuffer.ToArray();
        }
    }
}
