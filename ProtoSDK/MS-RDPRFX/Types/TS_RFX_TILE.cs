// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_TILE structure specifies the position of the tile on the frame
    /// and contains the encoded data for the three tile components of Y, Cb, and Cr.
    /// </summary>
    public struct TS_RFX_TILE
    {
        /// <summary>
        /// blockType field MUST be set to CBT_TILE (0xCAC3).
        /// </summary>
        public TS_RFX_BLOCKT BlockT;

        /// <summary>
        /// Specifies an index into the TS_RFX_CODEC_QUANT array
        /// provided in the TS_RFX_TILESET message.
        /// </summary>
        public byte quantIdxY;

        /// <summary>
        /// Specifies an index into the TS_RFX_CODEC_QUANT array
        /// provided in the TS_RFX_TILESET message.
        /// </summary>
        public byte quantIdxCb;

        /// <summary>
        /// Specifies an index into the TS_RFX_CODEC_QUANT array
        /// provided in the TS_RFX_TILESET message.
        /// </summary>
        public byte quantIdxCr;

        /// <summary>
        /// The X-index of the encoded tile in the screen tile grid.
        /// </summary>
        public ushort xIdx;

        /// <summary>
        /// The Y-index of the encoded tile in the screen tile grid.
        /// </summary>
        public ushort yIdx;

        /// <summary>
        /// Specifies the size, in bytes, of the YData field.
        /// </summary>
        public ushort YLen;

        /// <summary>
        /// Specifies the size, in bytes, of the CbData field.
        /// </summary>
        public ushort CbLen;

        /// <summary>
        /// Specifies the size, in bytes, of the CrData field.
        /// </summary>
        public ushort CrLen;

        /// <summary>
        /// A variable-length array. Contains the encoded data for the Y-component of the tile.
        /// The size, in bytes, of this field is specified by the YLen field.
        /// </summary>
        public byte[] YData;

        /// <summary>
        /// A variable-length array. Contains the encoded data for the Cb-component of the tile.
        /// The size, in bytes, of this field is specified by the CbLen field.
        /// </summary>
        public byte[] CbData;

        /// <summary>
        /// A variable-length array. Contains the encoded data for the Cr-component of the tile.
        /// The size, in bytes, of this field is specified by the CrLen field.
        /// </summary>
        public byte[] CrData;
    }

    /// <summary>
    /// Use to specify the position of the tile
    /// </summary>
    public struct TILE_POSITION
    {
        /// <summary>
        /// The X-index of the encoded tile in the screen tile grid.
        /// </summary>
        public ushort xIdx;

        /// <summary>
        /// The Y-index of the encoded tile in the screen tile grid.
        /// </summary>
        public ushort yIdx;
    }
}
