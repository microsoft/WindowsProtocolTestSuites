// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// Possible values of blockType field.
    /// </summary>
    public enum blockType_Value : ushort
    {
        /// <summary>
        /// A TS_RFX_SYNC (section 2.2.2.2.1) structure.
        /// </summary>
        WBT_SYNC = 0xCCC0,

        /// <summary>
        /// A TS_RFX_CODEC_VERSIONS (section 2.2.2.2.2) structure.
        /// </summary>
        WBT_CODEC_VERSIONS = 0xCCC1,

        /// <summary>
        /// A TS_RFX_CHANNELS (section 2.2.2.2.3) structure.
        /// </summary>
        WBT_CHANNELS = 0xCCC2,

        /// <summary>
        /// A TS_RFX_CONTEXT (section 2.2.2.2.4) structure.
        /// </summary>
        WBT_CONTEXT = 0xCCC3,

        /// <summary>
        /// A TS_RFX_FRAME_BEGIN (section 2.2.2.3.1) structure.
        /// </summary>
        WBT_FRAME_BEGIN = 0xCCC4,

        /// <summary>
        /// A TS_RFX_FRAME_END (section 2.2.2.3.2) structure.
        /// </summary>
        WBT_FRAME_END = 0xCCC5,

        /// <summary>
        /// A TS_RFX_REGION (section 2.2.2.3.3) structure.
        /// </summary>
        WBT_REGION = 0xCCC6,

        /// <summary>
        /// A TS_RFX_TILESET (section 2.2.2.3.4) structure.
        /// </summary>
        WBT_EXTENSION = 0xCCC7,

        /// <summary>
        /// A TS_RFX_TILE (section 2.2.2.3.4.1) structure.
        /// </summary>
        CBT_TILE = 0xCAC3,

        /// <summary>
        /// A TS_RFX_CAPS (section 2.2.1.1.1) structure.
        /// </summary>
        CBY_CAPS = 0xCBC0,

        /// <summary>
        /// A TS_RFX_CAPSET (section 2.2.1.1.1.1) structure.
        /// </summary>
        CBY_CAPSET = 0xCBC1,

        /// <summary>
        /// Invalid block type.
        /// </summary>
        InvalidType = 0xBBBB,
    }

    /// <summary>
    /// Represents the structure defined in section 2.2.2.1.1.
    /// This structure identifies the type of an encode message and
    /// specifies the size of the message.
    /// </summary>
    public struct TS_RFX_BLOCKT
    {
        /// <summary>
        /// A 16-bit, unsigned integer. Specifies the data block type.
        /// </summary>
        public blockType_Value blockType;

        /// <summary>
        /// A 32-bit, unsigned integer. Specifies the size, in bytes, of the data block.
        /// This size includes the size of the blockType and blockLen fields, as well as
        /// all trailing data.
        /// </summary>
        public uint blockLen;
    }
}
