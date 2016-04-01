// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_SYNC message MUST be the first message in any encoded stream.
    /// The decoder MUST examine this message to determine whether the protocol version is supported.
    /// </summary>
    public struct TS_RFX_SYNC : IMarshalable
    {
        /// <summary>
        /// A TS_RFX_BLOCKT (section 2.2.2.1.1) structure.
        /// The blockType field MUST be set to WBT_SYNC (0xCCC0).
        /// </summary>
        public TS_RFX_BLOCKT BlockT;

        /// <summary>
        /// This field MUST be set to WF_MAGIC (0xCACCACCA).
        /// </summary>
        public uint magic;

        /// <summary>
        /// Indicates the version number. This field MUST be set to WF_VERSION_1_0 (0x0100).
        /// </summary>
        public ushort version;

        public byte[] ToBytes()
        {
            List<byte> dataBuffer = new List<byte>();
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)(this.BlockT.blockType)));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.BlockT.blockLen));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.magic));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.version));
            return dataBuffer.ToArray();
        }
    }
}
