// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CONTEXT message contains information regarding
    /// the encoding properties being used.
    /// </summary>
    public struct TS_RFX_CONTEXT : IMarshalable
    {
        /// <summary>
        /// The blockType field MUST be set to WBT_CONTEXT (0xCCC3).
        /// </summary>
        public TS_RFX_CODEC_CHANNELT CodecChannelT;

        /// <summary>
        /// Specifies an identifier for this context message 
        /// that the other messages can use to refer to this message.
        /// </summary>
        public byte ctxId;

        /// <summary>
        /// Specifies the tile size used by the RemoteFX codec.
        /// This field MUST be set to CT_TILE_64x64 (0x0040),
        /// indicating that a tile is 64 x 64 pixels.
        /// </summary>
        public ushort tileSize;

        /// <summary>
        /// Contains a collection of bit-packed property fields.
        /// </summary>
        public ushort properties;

        public byte[] ToBytes()
        {
            List<byte> dataBuffer = new List<byte>();
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)this.CodecChannelT.blockType));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.CodecChannelT.blockLen));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.codecId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.channelId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.ctxId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.tileSize));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.properties));
            return dataBuffer.ToArray();

        }
    }

    /// <summary>
    /// A 8-bit unsigned integer. Specifies the entropy algorithm.
    /// </summary>
    [Flags]
    public enum EntropyAlgorithm : byte
    {
        None = 0x00,

        /// <summary>
        /// RLGR algorithm as detailed in 3.1.8.1.7.1.
        /// </summary>
        CLW_ENTROPY_RLGR1 = 0x01,

        /// <summary>
        /// RLGR algorithm as detailed in 3.1.8.1.7.2.
        /// </summary>
        CLW_ENTROPY_RLGR3 = 0x04
    }

    /// <summary>
    /// A 8-bit unsigned integer. Specifies the entropy algorithm. This field MUST be set to one of the following values.
    /// </summary>
    [Flags]
    public enum OperationalMode : byte
    {
        /// <summary>
        /// Image Mode.
        /// </summary>
        ImageMode = 0x02,

        /// <summary>
        /// Video Mode.
        /// </summary>
        VideoMode = 0x00
    }

}
