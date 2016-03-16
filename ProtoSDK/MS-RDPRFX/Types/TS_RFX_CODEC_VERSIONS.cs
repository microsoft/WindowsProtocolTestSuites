// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CODEC_VERSIONS message indicates the version
    /// of the RemoteFX codec that is being used.
    /// </summary>
    public struct TS_RFX_CODEC_VERSIONS : IMarshalable
    {
        /// <summary>
        /// The blockType field MUST be set to WBT_CODEC_VERSIONS (0xCCC1).
        /// </summary>
        public TS_RFX_BLOCKT BlockT;

        /// <summary>
        /// Specifies the number of codec version data blocks in the codecs field.
        /// This field MUST be set to 0x01.
        /// </summary>
        public byte numCodecs;

        /// <summary>
        /// A TS_RFX_CODEC_VERSIONT (section 2.2.2.1.4) structure.
        /// The codecId field MUST be set to 0x01 and
        /// the version field MUST be set to WF_VERSION_1_0 (0x0100).
        /// </summary>
        public TS_RFX_CODEC_VERSIONT codecs;

        public byte[] ToBytes()
        {
            List<byte> dataBuffer = new List<byte>();
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)(this.BlockT.blockType)));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.BlockT.blockLen));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.numCodecs));
            dataBuffer.AddRange(TypeMarshal.ToBytes<TS_RFX_CODEC_VERSIONT>(this.codecs));
            return dataBuffer.ToArray();
        }
    }
}
