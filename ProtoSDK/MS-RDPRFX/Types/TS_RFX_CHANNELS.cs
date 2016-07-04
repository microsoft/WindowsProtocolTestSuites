// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CHANNELS message contains the list of open channels,
    /// each of which corresponds to a monitor on the server.
    /// The decoder endpoint MUST be able to support channels with different frame dimensions.
    /// </summary>
    public struct TS_RFX_CHANNELS : IMarshalable
    {
        /// <summary>
        /// The blockType field MUST be set to WBT_CHANNELS (0xCCC2).
        /// </summary>
        public TS_RFX_BLOCKT BlockT;

        /// <summary>
        /// Specifies the number of channel data blocks in the channels field.
        /// </summary>
        public byte numChannels;

        /// <summary>
        /// A variable-length array of TS_RFX_CHANNELT (section 2.2.2.1.3) structures.
        /// </summary>
        public TS_RFX_CHANNELT[] channels;

        public byte[] ToBytes()
        {
            System.Collections.Generic.List<byte> dataBuffer = new System.Collections.Generic.List<byte>();
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)(this.BlockT.blockType)));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.BlockT.blockLen));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.numChannels));
            dataBuffer.AddRange(TypeMarshal.ToBytes<TS_RFX_CHANNELT>(this.channels[0]));
            return dataBuffer.ToArray();
        }
    }
}
