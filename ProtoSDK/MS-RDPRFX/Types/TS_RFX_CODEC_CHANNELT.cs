// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CODEC_CHANNELT structure is an extension of the TS_RFX_BLOCKT structure.
    /// It is present as the first field in messages that are targeted for a specific
    /// combination of codec and channel.
    /// </summary>
    public struct TS_RFX_CODEC_CHANNELT
    {
        /// <summary>
        /// A 16-bit, unsigned integer. Specifies the data block type.
        /// </summary>
        public blockType_Value blockType;

        /// <summary>
        /// Specifies the size, in bytes, of the data block. This size includes the size of
        /// the blockType, blockLen, codecId, and channelId fields, as well as all trailing data.
        /// </summary>
        public uint blockLen;

        /// <summary>
        /// Specifies the codec ID. This field MUST be set to 0x01.
        /// </summary>
        public byte codecId;

        /// <summary>
        /// Specifies the channel ID. A value between 0x00-0xFE specifies a particular channel.
        /// A channel ID of 0xFF applies the block data to all open channels.
        /// </summary>
        public byte channelId;
    }
}
