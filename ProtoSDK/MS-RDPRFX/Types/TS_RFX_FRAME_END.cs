// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_FRAME_END message specifies the end of a frame
    /// for a specific channel in the encoded stream.
    /// </summary>
    public struct TS_RFX_FRAME_END : IMarshalable
    {
        /// <summary>
        /// The blockType field MUST be set to WBT_FRAME_END (0xCCC5).
        /// </summary>
        public TS_RFX_CODEC_CHANNELT CodecChannelT;

        public byte[] ToBytes()
        {
            List<byte> dataBuffer = new List<byte>();
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)this.CodecChannelT.blockType));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.CodecChannelT.blockLen));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.codecId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.channelId));
            return dataBuffer.ToArray();
        }
    }
}
