// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_TILESET message contains encoding parameters and data
    /// for an arbitrary number of encoded tiles.
    /// </summary>
    public struct TS_RFX_TILESET : IMarshalable
    {
        /// <summary>
        /// The blockType field MUST be set to WBT_EXTENSION (0xCCC7),
        /// and the codecId field MUST be set to 0x01.
        /// </summary>
        public TS_RFX_CODEC_CHANNELT CodecChannelT;

        /// <summary>
        /// Specifies the message type. This field MUST be set to CBT_TILESET (0xCAC2).
        /// </summary>
        public ushort subtype;

        /// <summary>
        /// Specifies the identifier of the TS_RFX_CONTEXT message referenced
        /// by this TileSet message. This field MUST be set to 0x0000.
        /// </summary>
        public ushort idx;

        /// <summary>
        /// Contains a collection of bit-packed property fields.
        /// </summary>
        public ushort properties;

        /// <summary>
        /// Specifies the number of TS_RFX_CODEC_QUANT (section 2.2.2.1.5) structures
        /// present in the quantVals field.
        /// </summary>
        public byte numQuant;

        /// <summary>
        /// Specifies the width and height of a tile. This field MUST be set to 0x40.
        /// </summary>
        public byte tileSize;

        /// <summary>
        /// Specifies the number of TS_RFX_TILE (section 2.2.2.3.4.1) structures
        /// present in the tiles field.
        /// </summary>
        public ushort numTiles;

        /// <summary>
        /// Specifies the size, in bytes, of the tiles field.
        /// The tiles field contains encoded data for all of the tiles that have changed.
        /// </summary>
        public uint tilesDataSize;

        /// <summary>
        /// A variable-length array of TS_RFX_CODEC_QUANT (section 2.2.2.1.5) structures.
        /// The number of structures present in the array is indicated in the numQuant field.
        /// </summary>
        public TS_RFX_CODEC_QUANT[] quantVals;

        /// <summary>
        /// A variable-length array of TS_RFX_TILE (section 2.2.2.3.4.1) structures.
        /// The number of structures present in the array is indicated in the numTiles field,
        /// while the total size, in bytes, of this field is specified by the tilesDataSize field.
        /// </summary>
        public TS_RFX_TILE[] tiles;

        public byte[] ToBytes()
        {
            List<byte> dataBuffer = new List<byte>();
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)this.CodecChannelT.blockType));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.CodecChannelT.blockLen));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.codecId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.CodecChannelT.channelId));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.subtype));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.idx));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.properties));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.numQuant));
            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.tileSize));
            dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.numTiles));
            dataBuffer.AddRange(TypeMarshal.ToBytes<uint>(this.tilesDataSize));
            for (int i = 0; i < this.quantVals.Length; i++)
            {
                dataBuffer.AddRange(TypeMarshal.ToBytes<TS_RFX_CODEC_QUANT>(this.quantVals[i]));
            }

            for (int i = 0; i < this.tiles.Length; i++)
            {
                dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>((ushort)(this.tiles[i].BlockT.blockType)));
                dataBuffer.AddRange(TypeMarshal.ToBytes<uint>((uint)(this.tiles[i].BlockT.blockLen)));
                dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.tiles[i].quantIdxY));
                dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.tiles[i].quantIdxCb));
                dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(this.tiles[i].quantIdxCr));
                dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.tiles[i].xIdx));
                dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.tiles[i].yIdx));
                dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.tiles[i].YLen));
                dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.tiles[i].CbLen));
                dataBuffer.AddRange(TypeMarshal.ToBytes<ushort>(this.tiles[i].CrLen));
                dataBuffer.AddRange(this.tiles[i].YData);
                dataBuffer.AddRange(this.tiles[i].CbData);
                dataBuffer.AddRange(this.tiles[i].CrData);
            }
            return dataBuffer.ToArray();
        }
    }
}
