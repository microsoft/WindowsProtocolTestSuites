// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The class define the const used in all RFX Progressive Data Blocks.
    /// </summary>
    public class RFX_Progressive_CONST
    {
        public const uint SYNC_MAGIC = 0xCACCACCA;
        public const ushort SYNC_VERSION = 0x0100;
        public const ushort CONTEXT_TILESIZE = 0x0040;
        public const byte RFX_SUBBAND_DIFFING = 0x01;
        public const byte RFX_DWT_REDUCE_EXTRAPOLATE = 0x01;
        public const byte RFX_TILE_DIFFERENCE = 0x01;
    }

    /// <summary>
    /// The base pdu of all RFX Progressive Data Blocks.
    /// </summary>
    public class RFX_Progressive_DataBlock : BasePDU
    {
        #region Fields
        /// <summary>
        /// A 16-bits unsigned integer, specify RFX progressive block type.
        /// </summary>
        public RFXProgCodecBlockType blockType;

        /// <summary>
        /// A 32-bits unsigned integer, specify RFX progressive block length.
        /// </summary>
        public uint blockLen;

        /// <summary>
        /// A 32-bits unsigned integer, specify RFX progressive pdu length that is decoded.
        /// </summary>
        public uint decodedLen;

        #endregion Fields

        #region Methods
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16((ushort)blockType);
            marshaler.WriteUInt32(blockLen);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.blockType = (RFXProgCodecBlockType) marshaler.ReadUInt16();
                this.blockLen = marshaler.ReadUInt32();

                this.decodedLen = 6;
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion Methods
    }

    /// <summary>
    /// The RFX_Progressive_SYNC block is used to  transport codec version information
    /// </summary>
    public class RFX_Progressive_SYNC : RFX_Progressive_DataBlock
    {
        #region Fields
        /// <summary>
        /// A 32-bits unsigned integer, must be set to 0xCACCACCA .
        /// </summary>
        public uint magic;

        /// <summary>
        /// A 16-bits unsigned integer, specify the version of codec. current version is 1.0(0x0100)
        /// </summary>
        public ushort version;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Constructor, create RFX_Progressive_SYNC block.
        /// </summary>
        /// <param name="codecVersion">This is used to indicate codec version.</param>
        public RFX_Progressive_SYNC(ushort codecVersion)
        {
            this.blockType = RFXProgCodecBlockType.WBT_SYNC;
            this.blockLen = 6 + 6;  // Common part 6 bytes, sync block specific: 6 bytes.
            this.magic = RFX_Progressive_CONST.SYNC_MAGIC;
            this.version = codecVersion;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.magic);
            marshaler.WriteUInt16(this.version);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            
            try
            {
                if (!base.Decode(marshaler)) return false;

                this.magic = marshaler.ReadUInt32();
                this.version = marshaler.ReadUInt16();

                this.decodedLen += 6;
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion Methods
    }

    /// <summary>
    /// The RFX_Progressive_FRAME_BEGIN block is used to mark the begining of frames in codec payload.
    /// </summary>
    public class RFX_Progressive_FRAME_BEGIN : RFX_Progressive_DataBlock
    {
        #region Fields
        /// <summary>
        /// A 32-bits unsigned integer, specify the frame index .
        /// </summary>
        public uint frameIndex;

        /// <summary>
        /// A 16-bits unsigned integer, specify the region block number fllowed by frame_begin block.
        /// </summary>
        public ushort regionCount;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Constructor, create RFX_Progressive_FRAME_BEGIN block 
        /// </summary>
        /// <param name="frmIdx">This is used to indicate frame index.</param>
        /// <param name="rgCount">This is used to indicate region block number fllowed by frame_begin block.</param>
        public RFX_Progressive_FRAME_BEGIN(uint frmIdx, ushort rgCount)
        {
            this.blockType = RFXProgCodecBlockType.WBT_FRAME_BEGIN;
            this.blockLen = 6 + 6;  // Common part 6 bytes, frame_begin block specific: 6 bytes.
            this.frameIndex = frmIdx;
            this.regionCount = rgCount;
        }
     
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.frameIndex);
            marshaler.WriteUInt16(this.regionCount);
        }
     
        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler)) return false;
                this.frameIndex = marshaler.ReadUInt32();
                this.regionCount = marshaler.ReadUInt16();

                this.decodedLen += 6;
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }  
        }
     
        #endregion Methods
     }

    /// <summary>
    /// The RFX_Progressive_FRAME_END block is used to mark the end of frame in codec payload.
    /// </summary>
    public class RFX_Progressive_FRAME_END : RFX_Progressive_DataBlock
    {
        #region Fields
        // No additional fiels
        #endregion Fields

        #region Methods
        /// <summary>
        /// Constructor, create RFX_Progressive_FRAME_END block 
        /// </summary>
        public RFX_Progressive_FRAME_END()
        {
            this.blockType = RFXProgCodecBlockType.WBT_FRAME_END;
            this.blockLen = 6 ;  // common part 6 bytes, frame_end block specific: 0 bytes
        }
     
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
        }
     
        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {          
                if (!base.Decode(marshaler)) 
                    return false;
                else 
                    return true;
        }
     
        #endregion Methods
     }

    /// <summary>
    /// The RFX_Progressive_CONTEXT block is used to provide information about compressed data.
    /// </summary>
    public class RFX_Progressive_CONTEXT : RFX_Progressive_DataBlock
    {
        #region Fields
        /// <summary>
        /// A 8-bits unsigned integer, specify the context ID, must be set to 0x00 .
        /// </summary>
        public byte ctxId;

        /// <summary>
        /// A 16-bits unsigned integer, specify the width and height of a square tile, must be 0x0040.
        /// </summary>
        public ushort tileSize;

        /// <summary>
        /// A 8-bits unsigned integer, specify the context flag.
        /// </summary>
        public byte flags;
        #endregion Fields

        #region Methods
        /// <summary>
        /// Constructor, create RFX_Progressive_CONTEXT block 
        /// </summary>
        /// <param name="flag">This is used to indicate context flags.</param>
        public RFX_Progressive_CONTEXT(byte flag)
        {
            this.blockType = RFXProgCodecBlockType.WBT_CONTEXT;
            this.blockLen = 6 + 4;  // common part 6 bytes, context block specific: 4 bytes
            this.ctxId = 0x00;
            this.tileSize = RFX_Progressive_CONST.CONTEXT_TILESIZE;
            this.flags = flag;
        }
     
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(this.ctxId);
            marshaler.WriteUInt16(this.tileSize);
            marshaler.WriteByte(this.flags);
        }
     
        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler)) return false;
                this.ctxId = marshaler.ReadByte();
                this.tileSize = marshaler.ReadUInt16();
                this.flags = marshaler.ReadByte();

                this.decodedLen += 4;
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }  
        }
     
        #endregion Methods
     }

    /// <summary>
    /// The RFX_Progressive_REGION block contains the compressed data for a set of tiles from the frame.
    /// </summary>
    public class RFX_Progressive_REGION : RFX_Progressive_DataBlock
    {
        #region Fields
        /// <summary>
        /// A 8-bits unsigned integer, specify the width and height of a square tile. Must be 0x0040.
        /// </summary>
        public byte tileSize;
        /// <summary>
        /// A 16-bits unsigned integer, specify the number of TS_RFX_RECT  structures in the rects field.
        /// </summary>
        public ushort numRects;
        /// <summary>
        /// A 8-bits unsigned integer, specify the number of TS_RFX_CODEC_QUANT structures in the quantVals field. 
        /// </summary>
        public byte numQuant;
        /// <summary>
        /// A 8-bits unsigned integer, specify the number of RFX_PROGRESSIVE_CODEC_QUANT (section 2.2.5.2.1.5.1) structures in the quantProgVals field.
        /// </summary>
        public byte numProgQuant;
        /// <summary>
        /// A 8-bits unsigned integer, specify the region flags.
        /// </summary>
        public byte flags;
        /// <summary>
        /// A 16-bits unsigned integer, specify the number of elements in the tiles field.
        /// </summary>
        public ushort numTiles;
        /// <summary>
        /// A 32-bits unsigned integer, specify the size, in bytes, of the tiles field.
        /// </summary>
        public uint tileDataSize;
        /// <summary>
        /// A variable-length array of TS_RFX_RECT structures, specifies the encoded region.
        /// </summary>
        public TS_RFX_RECT[] rects;
        /// <summary>
        /// A variable-length array of TS_RFX_CODEC_QUANT structures.
        /// </summary>
        public TS_RFX_CODEC_QUANT[] quantVals;
        /// <summary>
        /// A variable-length array of RFX_PROGRESSIVE_CODEC_QUANT structures.
        /// </summary>
        public RFX_PROGRESSIVE_CODEC_QUANT[] quantProgVals;
        /// <summary>
        /// A variable-length array of RFX_PROGRESSIVE_DATABLOCK structures. The value of the blockType field of 
        /// each block present in the array MUST be WBT_TILE_SIMPLE (0xCCC5), WBT_TILE_PROGRESSIVE_FIRST (0xCCC6), 
        /// or WBT_TILE_PROGRESSIVE_UPGRADE (0xCCC7).
        /// </summary>
        //byte[] tiles;
     
        #endregion Fields

        #region Methods
        
        /// <summary>
        /// constructor, create RFX_Progressive_REGION block 
        /// </summary>
        /// <param name="bProg">This is used to indicate if progressive encoding is enabled </param>
        /// <param name="flag">This is used to indicate region flag </param>
        /// <param name="tileDSize">This is used to indicate total size of all tiles encoded data </param>
        /// <param name="regionRects">This is used to indicate region rectangles that all tiles to fill </param>
        /// <param name="tileDataArr">This is used to indicate quant data for specific layer </param>
        public RFX_Progressive_REGION(bool bProg, byte flag, uint tileDSize, TS_RFX_RECT[] regionRects, EncodedTile[] tileDataArr)
        {
            this.blockType = RFXProgCodecBlockType.WBT_REGION;
            this.blockLen = 6;  // common part 6 bytes

            this.tileSize = (byte)RFX_Progressive_CONST.CONTEXT_TILESIZE;
            this.flags = flag;
            this.blockLen += 2;

            // Init rect.
            this.numRects = (ushort)regionRects.Count();
            this.rects = regionRects;
            this.blockLen += (uint)(2 + this.numRects * Marshal.SizeOf(this.rects[0]));

            // Init quant array.
            this.numQuant = (byte)tileDataArr[0].CodecQuantVals.Length;      // each tile share same quantity table
            this.blockLen += 1;
            this.quantVals = tileDataArr[0].CodecQuantVals;
            this.blockLen += (uint)(this.numQuant * 5);
            

            if (bProg)  // Progressive encoding.
            {
                this.numProgQuant = 1;  // Each tile share same progressive quantity table.
                this.blockLen += 1;
                this.quantProgVals = new RFX_PROGRESSIVE_CODEC_QUANT[this.numProgQuant];
                for (int i = 0; i < this.numProgQuant; i++)
                {
                    this.quantProgVals[i] = tileDataArr[i].ProgCodecQuant;
                    this.blockLen += (uint)(1 + Marshal.SizeOf(this.quantProgVals[i].yQuantValues) +
                                            Marshal.SizeOf(this.quantProgVals[i].cbQuantValues) +
                                            Marshal.SizeOf(this.quantProgVals[i].crQuantValues));
                }
            }
            else  // Non-progressive encoding.
            {
                this.numProgQuant = 0;
                this.blockLen += 1;
                this.quantProgVals = null;
            }

            this.numTiles = (ushort)tileDataArr.Count();
            this.tileDataSize = tileDSize;
            this.blockLen += (6 + tileDSize);
        }

        private byte[] Convert2Bytes(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }
     
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);

            marshaler.WriteByte(this.tileSize);
            marshaler.WriteUInt16(this.numRects);
            marshaler.WriteByte(this.numQuant);
            marshaler.WriteByte(this.numProgQuant);
            marshaler.WriteByte(this.flags);
            marshaler.WriteUInt16(this.numTiles);
            marshaler.WriteUInt32(this.tileDataSize);

            // Encode rects, in normal case, it can't be NULL
            if (rects != null)
            {
                for (int i = 0; i < rects.Count(); i++)
                {
                    byte[] arr = Convert2Bytes(rects[i]);
                    marshaler.WriteBytes(arr);
                }
            }

            // Encode quantVals, in normal case, it can't be NULL
            if (quantVals != null)
            {
                for (int i = 0; i < quantVals.Count(); i++)
                {
                    byte[] arr = Convert2Bytes(quantVals[i]);
                    marshaler.WriteBytes(arr);
                }
            }

            if (quantProgVals != null)   // progressive encoding enabled
            {
                for (int i = 0; i < quantProgVals.Count(); i++)
                {
                    byte[] arr;
                    marshaler.WriteByte(quantProgVals[i].quality);

                    arr = Convert2Bytes(quantProgVals[i].yQuantValues);
                    marshaler.WriteBytes(arr);

                    arr = Convert2Bytes(quantProgVals[i].cbQuantValues);
                    marshaler.WriteBytes(arr);

                    arr = Convert2Bytes(quantProgVals[i].crQuantValues);
                    marshaler.WriteBytes(arr);
                }
            }

        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler)) return false;
                this.tileSize = marshaler.ReadByte();
                this.numRects = marshaler.ReadUInt16();
                this.numQuant = marshaler.ReadByte();
                this.numProgQuant = marshaler.ReadByte();
                this.flags = marshaler.ReadByte();
                this.decodedLen += 6;

                this.numTiles = marshaler.ReadUInt16();
                this.tileDataSize = marshaler.ReadUInt32();
                this.decodedLen += 6;

                this.rects = new TS_RFX_RECT[this.numRects];
                for (int i = 0; i < this.numRects; i++)
                {
                    this.rects[i].x = marshaler.ReadUInt16();
                    this.rects[i].y = marshaler.ReadUInt16();
                    this.rects[i].width = marshaler.ReadUInt16();
                    this.rects[i].height = marshaler.ReadUInt16();

                    this.decodedLen += (uint)Marshal.SizeOf(this.rects[i]);
                }

                this.quantVals = new TS_RFX_CODEC_QUANT[this.numQuant];
                for (int i = 0; i < this.numQuant; i++)
                {
                    this.quantVals[i].LL3_LH3 = marshaler.ReadByte();
                    this.quantVals[i].HL3_HH3 = marshaler.ReadByte();
                    this.quantVals[i].LH2_HL2 = marshaler.ReadByte();
                    this.quantVals[i].HH2_LH1 = marshaler.ReadByte();
                    this.quantVals[i].HL1_HH1 = marshaler.ReadByte();

                    this.decodedLen += 5;
                }

                this.quantProgVals = new RFX_PROGRESSIVE_CODEC_QUANT[this.numProgQuant];
                
                for (int i = 0; i < this.numProgQuant; i++)
                {
                    int quantProgSize = Marshal.SizeOf(this.quantProgVals[i]);
                    byte[] rawData = marshaler.ReadBytes(quantProgSize);
                    GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
                    try
                    {
                        IntPtr rawDataPtr = handle.AddrOfPinnedObject();
                        this.quantProgVals[i] = (RFX_PROGRESSIVE_CODEC_QUANT)Marshal.PtrToStructure(rawDataPtr, typeof(RFX_PROGRESSIVE_CODEC_QUANT));
                    }
                    finally
                    {
                        handle.Free();
                    }
                    this.decodedLen += (uint)quantProgSize;
                }

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }  
        }
     
        #endregion Methods
     }
    /// <summary>
    /// The RFX_Progressive_TILE_SIMPLE block specifies a tile that has been compressed without progressive techniques.
    /// </summary>
    public class RFX_Progressive_TILE_SIMPLE : RFX_Progressive_DataBlock
    {
        #region Fields
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxY;
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxCb;
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxCr;
        /// <summary>
        /// A 16-bits unsigned integer, specify the x-index of the encoded tile in the screen tile grid. 
        /// </summary>
        public ushort xIdx;
        /// <summary>
        /// A 16-bits unsigned integer, specify the y-index of the encoded tile in the screen tile grid. 
        /// </summary>
        public ushort yIdx;
        /// <summary>
        /// A 8-bits bool, specify if tile difference is used. 
        /// </summary>
        public bool flags;
        /// <summary>
        /// A 16-bits integer, specify the size, in bytes, of the yData field. 
        /// </summary>
        public ushort yLen;
        /// <summary>
        /// A 16-bits integer, specify the size, in bytes, of the cbData field. 
        /// </summary>
        public ushort cbLen;
        /// <summary>
        /// A 16-bits integer, specify the size, in bytes, of the crData field. 
        /// </summary>
        public ushort crLen;
        /// <summary>
        /// A 16-bits integer, specify the size, in bytes, of the tailData field. 
        /// </summary>
        public ushort tailLen;
        /// <summary>
        /// A variable-length array of bytes, specify the compressed data for the Luma (Y) component of the tile . 
        /// </summary>
        public byte[] yData;
        /// <summary>
        /// A variable-length array of bytes, specify the compressed data for the Chroma Blue (Cb)  component of the tile . 
        /// </summary>
        public byte[] cbData;
        /// <summary>
        /// A variable-length array of bytes, specify the compressed data for the Chroma Red (Cr)  component of the tile . 
        /// </summary>
        public byte[] crData;
        /// <summary>
        /// A variable-length array of bytes, specify the tail data(only valid on windows).
        /// </summary>
        public byte[] tailData;
        #endregion Fields

        #region Methods
        /// <summary>
        /// constructor, create RFX_Progressive_TILE_SIMPLE block 
        /// </summary>
        /// <param name="quantIdx">This is used to indicate index in TS_RFX_CODEC_AUANT array of region block</param>
        /// <param name="tileIdx">This is used to indicate tile position([x, y]).</param>
        /// <param name="tileData">This is used to indicate encoded tile data</param>
        public RFX_Progressive_TILE_SIMPLE(byte quantIdx, TileIndex tileIdx, EncodedTile tileData)
        {
            this.blockType = RFXProgCodecBlockType.WBT_TILE_SIMPLE;
            this.blockLen = 6;  // common part 6 bytes

            this.quantIdxY = quantIdx;
            this.quantIdxCb = quantIdx;
            this.quantIdxCr = quantIdx;

            this.xIdx = tileIdx.X;
            this.yIdx = tileIdx.Y;

            this.flags = tileData.IsDifferenceTile;
            this.blockLen += 8;

            this.yLen = (tileData.YEncodedData == null) ? (ushort)0 : (ushort)tileData.YEncodedData.Count();
            this.yData = tileData.YEncodedData;
            this.blockLen += (uint)(2 + this.yLen);

            this.cbLen = (tileData.CbEncodedData == null) ? (ushort)0 : (ushort)tileData.CbEncodedData.Count();
            this.cbData = tileData.CbEncodedData;
            this.blockLen += (uint)(2 + this.cbLen);

            this.crLen = (tileData.CrEncodedData == null) ? (ushort)0 : (ushort)tileData.CrEncodedData.Count();
            this.crData = tileData.CrEncodedData;
            this.blockLen += (uint)(2 + this.crLen);

            //this.tailLen = 8;
            //this.tailData = new byte[8]{0x4C, 0x41, 0x01, 0x00, 0xFF, 0xFF, 0x00, 0x10};
            this.tailLen = 0;
            this.tailData = null;
            this.blockLen += (uint)(2 + this.tailLen);

        }
     
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(this.quantIdxY);
            marshaler.WriteByte(this.quantIdxCb);
            marshaler.WriteByte(this.quantIdxCr);
            marshaler.WriteUInt16(this.xIdx);
            marshaler.WriteUInt16(this.yIdx);
            marshaler.WriteBool(this.flags);
            
            marshaler.WriteUInt16(this.yLen);
            marshaler.WriteUInt16(this.cbLen);
            marshaler.WriteUInt16(this.crLen);
            marshaler.WriteUInt16(this.tailLen);

            if (this.yData != null)
                marshaler.WriteBytes(this.yData);
            if (this.cbData != null)
                marshaler.WriteBytes(this.cbData);
            if (this.crData != null)
                marshaler.WriteBytes(this.crData);
            if (this.tailData != null)
                marshaler.WriteBytes(this.tailData);

        }
     
        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler)) return false;

                this.quantIdxY = marshaler.ReadByte();
                this.quantIdxCb = marshaler.ReadByte();
                this.quantIdxCr = marshaler.ReadByte();
                this.xIdx = marshaler.ReadUInt16();
                this.yIdx = marshaler.ReadUInt16();
                this.flags = marshaler.ReadBool();
                this.decodedLen += 8;

                this.yLen = marshaler.ReadUInt16();
                this.cbLen = marshaler.ReadUInt16();
                this.crLen = marshaler.ReadUInt16();
                this.tailLen = marshaler.ReadUInt16();
                this.decodedLen += 8;

                this.yData = marshaler.ReadBytes((int)this.yLen);
                this.decodedLen += this.yLen;
                this.cbData = marshaler.ReadBytes((int)this.cbLen);
                this.decodedLen += this.cbLen;
                this.crData = marshaler.ReadBytes((int)this.crLen);
                this.decodedLen += this.crLen;
                this.tailData = marshaler.ReadBytes((int)this.tailLen);
                this.decodedLen += this.tailLen;
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }  
        }
     
        #endregion Methods
     }

    /// <summary>
    /// The RFX_Progressive_TILE_FIRST block specifies the first-pass compression of a tile with progressive techniques.
    /// </summary>
    public class RFX_Progressive_TILE_FIRST : RFX_Progressive_DataBlock
    {
        #region Fields
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxY;
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxCb;
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxCr;
        /// <summary>
        /// A 16-bits unsigned integer, specify the x-index of the encoded tile in the screen tile grid. 
        /// </summary>
        public ushort xIdx;
        /// <summary>
        /// A 16-bits unsigned integer, specify the y-index of the encoded tile in the screen tile grid. 
        /// </summary>
        public ushort yIdx;
        /// <summary>
        /// A 8-bits bool, specify if tile difference is used. 
        /// </summary>
        public bool flags;
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the RFX_PROGRESSIVE_CODEC_QUANT array 
        /// (the quantProgVals field) of the containing RFX_PROGRESSIVE_REGION block. 
        /// </summary>
        public byte progressiveQuality;
        /// <summary>
        /// A 16-bits integer, specify the size, in bytes, of the yData field. 
        /// </summary>
        public ushort yLen;
        /// <summary>
        /// A 16-bits integer, specify the size, in bytes, of the cbData field. 
        /// </summary>
        public ushort cbLen;
        /// <summary>
        /// A 16-bits integer, specify the size, in bytes, of the crData field. 
        /// </summary>
        public ushort crLen;
        /// <summary>
        /// A 16-bits integer, specify the size, in bytes, of the tailData field. 
        /// </summary>
        public ushort tailLen;
        /// <summary>
        /// A variable-length array of bytes, specify the compressed data for the Luma (Y) component of the tile . 
        /// </summary>
        public byte[] yData;
        /// <summary>
        /// A variable-length array of bytes, specify the compressed data for the Chroma Blue (Cb)  component of the tile . 
        /// </summary>
        public byte[] cbData;
        /// <summary>
        /// A variable-length array of bytes, specify the compressed data for the Chroma Red (Cr)  component of the tile . 
        /// </summary>
        public byte[] crData;
        /// <summary>
        /// A variable-length array of bytes, specify the tail data(only valid on windows).
        /// </summary>
        public byte[] tailData;
        #endregion Fields

        #region Methods
        /// <summary>
        /// constructor, create RFX_Progressive_TILE_FIRST block 
        /// </summary>
        /// <param name="quantIdx">This is used to indicate index in TS_RFX_CODEC_AUANT array of region block</param>
        /// <param name="tileIdx">This is used to indicate tile position([x, y]).</param>
        /// <param name="tileData">This is used to indicate encoded tile data</param>
        public RFX_Progressive_TILE_FIRST(byte quantIdx, TileIndex tileIdx, EncodedTile tileData)
        {
            this.blockType = RFXProgCodecBlockType.WBT_TILE_PROGRESSIVE_FIRST;
            this.blockLen = 6;  // common part 6 bytes

            this.quantIdxY = quantIdx;
            this.quantIdxCb = quantIdx;
            this.quantIdxCr = quantIdx;

            this.xIdx = tileIdx.X;
            this.yIdx = tileIdx.Y;

            this.flags = tileData.IsDifferenceTile;
            this.progressiveQuality = tileData.ProgCodecQuant.quality;
            this.blockLen += 9;

            this.yLen = (tileData.YEncodedData == null) ? (ushort)0 : (ushort)tileData.YEncodedData.Count();
            this.yData = tileData.YEncodedData;
            this.blockLen += (uint)(2 + this.yLen);

            this.cbLen = (tileData.CbEncodedData == null) ? (ushort)0 : (ushort)tileData.CbEncodedData.Count();
            this.cbData = tileData.CbEncodedData;
            this.blockLen += (uint)(2 + this.cbLen);

            this.crLen = (tileData.CrEncodedData == null) ? (ushort)0 : (ushort)tileData.CrEncodedData.Count();
            this.crData = tileData.CrEncodedData;
            this.blockLen += (uint)(2 + this.crLen);

            //this.tailLen = 8;
            //this.tailData = new byte[8] { 0x4C, 0x41, 0x01, 0x00, 0xFF, 0xFF, 0x00, 0x10 };
            this.tailLen = 0;
            this.tailData = null;
            this.blockLen += (uint)(2 + this.tailLen);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(this.quantIdxY);
            marshaler.WriteByte(this.quantIdxCb);
            marshaler.WriteByte(this.quantIdxCr);
            marshaler.WriteUInt16(this.xIdx);
            marshaler.WriteUInt16(this.yIdx);
            marshaler.WriteBool(this.flags);
            marshaler.WriteByte(this.progressiveQuality);

            marshaler.WriteUInt16(this.yLen);
            marshaler.WriteUInt16(this.cbLen);
            marshaler.WriteUInt16(this.crLen);
            marshaler.WriteUInt16(this.tailLen);

            if (this.yData != null)
                marshaler.WriteBytes(this.yData);
            if (this.cbData != null)
                marshaler.WriteBytes(this.cbData);
            if (this.crData != null)
                marshaler.WriteBytes(this.crData);
            if (this.tailData != null)
                marshaler.WriteBytes(this.tailData);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler)) return false;
                this.quantIdxY = marshaler.ReadByte();
                this.quantIdxCb = marshaler.ReadByte();
                this.quantIdxCr = marshaler.ReadByte();
                this.xIdx = marshaler.ReadUInt16();
                this.yIdx = marshaler.ReadUInt16();
                this.flags = marshaler.ReadBool();
                this.progressiveQuality = marshaler.ReadByte();
                this.decodedLen += 9;

                this.yLen = marshaler.ReadUInt16();
                this.cbLen = marshaler.ReadUInt16();
                this.crLen = marshaler.ReadUInt16();
                this.tailLen = marshaler.ReadUInt16();
                this.decodedLen += 8;

                this.yData = marshaler.ReadBytes((int)this.yLen);
                this.decodedLen += this.yLen;
                this.cbData = marshaler.ReadBytes((int)this.cbLen);
                this.decodedLen += this.cbLen;
                this.crData = marshaler.ReadBytes((int)this.crLen);
                this.decodedLen += this.crLen;
                this.tailData = marshaler.ReadBytes((int)this.tailLen);
                this.decodedLen += this.tailLen;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion Methods
    }

    /// <summary>
    /// The RFX_PROGRESSIVE_TILE_UPGRADE structure contains data required for an upgrade pass of a tile using progressive techniques. 
    /// </summary>
    public class RFX_PROGRESSIVE_TILE_UPGRADE : RFX_Progressive_DataBlock
    {
        #region Fields
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxY;
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxCb;
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the TS_RFX_CODEC_QUANT array (the quantVals field) 
        /// of the containing RFX_PROGRESSIVE_REGION (section 2.2.5.2.1.5) block.
        /// </summary>
        public byte quantIdxCr;
        /// <summary>
        /// A 16-bits unsigned integer, specify the x-index of the encoded tile in the screen tile grid. 
        /// </summary>
        public ushort xIdx;
        /// <summary>
        /// A 16-bits unsigned integer, specify the y-index of the encoded tile in the screen tile grid. 
        /// </summary>
        public ushort yIdx;
        /// <summary>
        /// A 8-bits unsigned integer, specify an index into the RFX_PROGRESSIVE_CODEC_QUANT array 
        /// (the quantProgVals field) of the containing RFX_PROGRESSIVE_REGION block. 
        /// </summary>
        public byte progressiveQuality;
        /// <summary>
        /// A 16-bits unsigned integer, specify the size, in bytes, of the ySrlData field. 
        /// </summary>
        public ushort ySrlLen;
        /// <summary>
        /// A 16-bits unsigned integer, specify the size, in bytes, of the yRawData field. 
        /// </summary>
        public ushort yRawLen;
        /// <summary>
        /// A 16-bits unsigned integer, specify the size, in bytes, of the cbSrlData field. 
        /// </summary>
        public ushort cbSrlLen;
        /// <summary>
        /// A 16-bits unsigned integer, specify the size, in bytes, of the cbRawData field. 
        /// </summary>
        public ushort cbRawLen;
        /// <summary>
        /// A 16-bits unsigned integer, specify the size, in bytes, of the crSrlData field. 
        /// </summary>
        public ushort crSrlLen;
        /// <summary>
        /// A 16-bits unsigned integer, specify the size, in bytes, of the crRawData field. 
        /// </summary>
        public ushort crRawLen;
        /// <summary>
        /// A variable-length array of bytes, specify bits for the Luma (Y) component compressed using the Simplified-RL method. 
        /// </summary>
        byte[] ySrlData;
        /// <summary>
        /// A variable-length array of bytes, specify raw bits for the Luma (Y) component. 
        /// </summary>
        byte[] yRawData;
        /// <summary>
        /// A variable-length array of bytes, specify bits for the Chroma Blue (Cb) component compressed using the Simplified-RL method. 
        /// </summary>
        byte[] cbSrlData;
        /// <summary>
        /// A variable-length array of bytes, specify raw bits for the Chroma Blue (Cb) component. 
        /// </summary>
        byte[] cbRawData;
        /// <summary>
        /// A variable-length array of bytes, specify bits for the Chroma Red (Cr) component compressed using the Simplified-RL method. 
        /// </summary>
        byte[] crSrlData;
        /// <summary>
        /// A variable-length array of bytes, specify raw bits for the Chroma  Red (Cr) component. 
        /// </summary>
        byte[] crRawData;
     
        #endregion Fields

        #region Methods
        /// <summary>
        /// Constructor, create RFX_PROGRESSIVE_TILE_UPGRADE block 
        /// </summary>
        /// <param name="quantIdx">This is used to indicate index in TS_RFX_CODEC_AUANT array of region block</param>
        /// <param name="tileIdx">This is used to indicate tile position([x, y]).</param>
        /// <param name="tileData">This is used to indicate encoded tile data which is related to quantIdx</param>
        public RFX_PROGRESSIVE_TILE_UPGRADE(byte quantIdx, TileIndex tileIdx, EncodedTile tileData)
        {
            this.blockType = RFXProgCodecBlockType.WBT_TILE_PROGRESSIVE_UPGRADE;
            this.blockLen = 6;  // Common part 6 bytes.

            this.quantIdxY = quantIdx;
            this.quantIdxCb = quantIdx;
            this.quantIdxCr = quantIdx;

            this.xIdx = tileIdx.X;
            this.yIdx = tileIdx.Y;

            this.progressiveQuality = tileData.ProgCodecQuant.quality;
            this.blockLen += 8;
            
            // Srl data. 
            this.ySrlLen = (tileData.YEncodedData == null) ? (ushort)0 : (ushort)tileData.YEncodedData.Count();
            this.ySrlData = tileData.YEncodedData;

            this.cbSrlLen = (tileData.CbEncodedData == null) ? (ushort)0 : (ushort)tileData.CbEncodedData.Count();
            this.cbSrlData = tileData.CbEncodedData;
            this.crSrlLen = (tileData.CrEncodedData == null) ? (ushort)0 : (ushort)tileData.CrEncodedData.Count();
            this.crSrlData = tileData.CrEncodedData;
            this.blockLen += (uint)(6 + this.ySrlLen + this.cbSrlLen + this.crSrlLen);
             
            // Raw data. 
            this.yRawLen = (tileData.YRawData == null) ? (ushort)0 : (ushort)tileData.YRawData.Count();
            this.yRawData = tileData.YRawData;
            this.cbRawLen = (tileData.CbRawData == null) ? (ushort)0 : (ushort)tileData.CbRawData.Count();
            this.cbRawData = tileData.CbRawData;
            this.crRawLen = (tileData.CrRawData == null) ? (ushort)0 : (ushort)tileData.CrRawData.Count();
            this.crRawData = tileData.CrRawData;
            this.blockLen += (uint)(6 + this.yRawLen + this.cbRawLen + this.crRawLen);
        }
     
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(this.quantIdxY);
            marshaler.WriteByte(this.quantIdxCb);
            marshaler.WriteByte(this.quantIdxCr);
            marshaler.WriteUInt16(this.xIdx);
            marshaler.WriteUInt16(this.yIdx);
            marshaler.WriteByte(this.progressiveQuality);

            marshaler.WriteUInt16(this.ySrlLen);
            marshaler.WriteUInt16(this.yRawLen);
            marshaler.WriteUInt16(this.cbSrlLen);
            marshaler.WriteUInt16(this.cbRawLen);
            marshaler.WriteUInt16(this.crSrlLen);
            marshaler.WriteUInt16(this.crRawLen);

            if (this.ySrlData != null)
                marshaler.WriteBytes(this.ySrlData);
            if (this.yRawData != null)
                marshaler.WriteBytes(this.yRawData);
            if (this.cbSrlData != null)
                marshaler.WriteBytes(this.cbSrlData);
            if (this.cbRawData != null)
                marshaler.WriteBytes(this.cbRawData);
            if (this.crSrlData != null)
                marshaler.WriteBytes(this.crSrlData);
            if (this.crRawData != null)
                marshaler.WriteBytes(this.crRawData);
        }
     
        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler)) return false;

                this.quantIdxY = marshaler.ReadByte();
                this.quantIdxCb = marshaler.ReadByte();
                this.quantIdxCr = marshaler.ReadByte();
                this.xIdx = marshaler.ReadUInt16();
                this.yIdx = marshaler.ReadUInt16();
                this.progressiveQuality = marshaler.ReadByte();
                this.decodedLen += 8;

                this.ySrlLen = marshaler.ReadUInt16();
                this.yRawLen = marshaler.ReadUInt16();
                this.cbSrlLen = marshaler.ReadUInt16();
                this.cbRawLen = marshaler.ReadUInt16();
                this.crSrlLen = marshaler.ReadUInt16();
                this.crRawLen = marshaler.ReadUInt16();
                this.decodedLen += 12;

                this.ySrlData = marshaler.ReadBytes((int)this.ySrlLen);
                this.decodedLen += this.ySrlLen;
                this.yRawData = marshaler.ReadBytes((int)this.yRawLen);
                this.decodedLen += this.yRawLen;
                this.cbSrlData = marshaler.ReadBytes((int)this.cbSrlLen);
                this.decodedLen += this.cbSrlLen;
                this.cbRawData = marshaler.ReadBytes((int)this.cbRawLen);
                this.decodedLen += this.cbRawLen;
                this.crSrlData = marshaler.ReadBytes((int)this.crSrlLen);
                this.decodedLen += this.crSrlLen;
                this.crRawData = marshaler.ReadBytes((int)this.crRawLen);
                this.decodedLen += this.crRawLen;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }  
        }
        #endregion Methods
    }

    /// <summary>
    /// The RFX_PROGRESSIVE_BlockManager is responsible for the construct blocks into byte array
    /// </summary>
    public class RFX_PROGRESSIVE_BlockManager : BasePDU
    {
        #region Fields

        /// <summary>
        /// a list of RFX Progressive Data Blocks. 
        /// </summary>
        protected List<RFX_Progressive_DataBlock> blkList;

        /// <summary>
        /// next available frame index of rfx progressive block
        /// </summary>
        protected ushort maxblkFrameIndex;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor 
        /// </summary>
        public RFX_PROGRESSIVE_BlockManager()
        {
            maxblkFrameIndex = 0;
            blkList = new List<RFX_Progressive_DataBlock>();
            blkList.Clear();
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            foreach (RFX_Progressive_DataBlock block in blkList)
                block.Encode(marshaler);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                //if (!base.Decode(marshaler)) return false;
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }  
        }

        /// <summary>
        /// Get frameIndex of fram_begin_block 
        /// </summary>
        public uint GetFrameIndex()
        {
            return maxblkFrameIndex++; 
        }

        /// <summary>
        /// Create sync block if needed
        /// </summary>
        /// <param name="bSync">This is used to indicate if sync block needed.</param>
        public virtual void CreateSyncBlock(bool bSync)
        {
            // Create sync block if needed and add to block list.
            if (bSync)
            {
                RFX_Progressive_SYNC sync_block = new RFX_Progressive_SYNC(RFX_Progressive_CONST.SYNC_VERSION);
                blkList.Add(sync_block);
            }
        }

        /// <summary>
        /// Create context block if needed.
        /// </summary>
        /// <param name="bContext">This is used to indicate if context block needed.</param>
        /// <param name="bSubDiff">This is used to indicate if subband_diffing flag is enabled in context block.</param>
        public virtual void CreateContextBlock(bool bContext, bool bSubDiff)
        {
            // Create context block if needed and add to block list.
            if (bContext)
            {
                byte sdFlag = Convert.ToByte(bSubDiff);
                RFX_Progressive_CONTEXT context_block = new RFX_Progressive_CONTEXT(sdFlag);
                blkList.Add(context_block);
            }
        }

        /// <summary>
        /// Create frame_begin block
        /// </summary>
        /// <param name="regionNum">This is used to indicate the number of region block followed.</param>
        public virtual void CreateFrameBeginBlock(ushort regionNum)
        {
            uint blkFrameIdx = GetFrameIndex();
            RFX_Progressive_FRAME_BEGIN frame_begin_block = new RFX_Progressive_FRAME_BEGIN(blkFrameIdx, regionNum);
            blkList.Add(frame_begin_block);
        }
             
        /// <summary>
        /// Create frame_begin and previous blocks(sync or context block) if needed
        /// </summary>
        /// <param name="bSync">This is used to indicate if sync block needed.</param>
        /// <param name="bContext">This is used to indicate if context block needed.</param>
        /// <param name="bSubDiff">This is used to indicate if subband_diffing flag is enabled in context block.</param>
        /// <param name="regionNum">This is used to indicate the number of region block followed.</param>
        public virtual void CreateBeginBlocks(bool bSync, bool bContext, bool bSubDiff, ushort regionNum)
        {
            // Clear existing blocks before create a new RFX_progressive_datablock frame.
            blkList.Clear();

            CreateSyncBlock(bSync);
            CreateContextBlock(bContext, bSubDiff);
            CreateFrameBeginBlock(regionNum);         
        }

        /// <summary>
        /// Create frame_end block.
        /// </summary>
        public virtual void CreateFrameEndBlock()
        {
            // Create frame end block.
            RFX_Progressive_FRAME_END frame_end_block = new RFX_Progressive_FRAME_END();
            blkList.Add(frame_end_block);
        }

        /// <summary>
        /// Build tile data blocks(tile simple or tile first or tile upgrade).
        /// </summary>
        /// <param name="quantIdx">This is used to indicate index in TS_RFX_CODEC_QUANT array of region block</param>
        /// <param name="tileIdx">This is used to indicate the tile index.</param>
        /// <param name="tileData">This is used to indicate the encoded tile data.</param>
        /// <param name="tileBlockType">This is used to indicate the tile data block type(simple, first, or upgrade).</param>
        public RFX_Progressive_DataBlock BuildTileDataBlock(byte quantIdx, TileIndex tileIdx, EncodedTile tileData, RFXProgCodecBlockType tileBlockType)
        {
            RFX_Progressive_DataBlock tile_block;

            if (tileBlockType == RFXProgCodecBlockType.WBT_TILE_PROGRESSIVE_FIRST)
            {
                tile_block = new RFX_Progressive_TILE_FIRST(quantIdx, tileIdx, tileData);
            }
            else if (tileBlockType == RFXProgCodecBlockType.WBT_TILE_PROGRESSIVE_UPGRADE)
            {
                tile_block = new RFX_PROGRESSIVE_TILE_UPGRADE(quantIdx, tileIdx, tileData);
            }
            else
            {
                tile_block = new RFX_Progressive_TILE_SIMPLE(quantIdx, tileIdx, tileData);
            }

            return tile_block;
        }

        /// <summary>
        /// Create region block.
        /// </summary>
        /// <param name="bReduceExtrapolate">This is used to indicate if DWT uses the "Reduce Extrapolate" method.</param>
        /// <param name="tileDict">This is used to indicate the dictionary of tile index and encoded data.</param>
        /// <param name="tileDataLength">This is used to indicate the encoded tile data length.</param>
        /// <param name="tileBlockType">This is used to indicate the tile data block type(simple, first, or upgrade).</param>
        public virtual RFX_Progressive_REGION BuildRegionBlock(bool bReduceExtrapolate, Dictionary<TileIndex, EncodedTile> TileDict, uint tileDataLength, RFXProgCodecBlockType tileBlockType)
        {
            // Create region block data.
            byte tfFlag = Convert.ToByte(bReduceExtrapolate);
            List<TS_RFX_RECT> tileRectList = new List<TS_RFX_RECT>();
            foreach (KeyValuePair<TileIndex, EncodedTile> tilePair in TileDict)
            {

                TS_RFX_RECT tileRect = new TS_RFX_RECT();
                tileRect.x = (ushort)(tilePair.Key.X * RFX_Progressive_CONST.CONTEXT_TILESIZE);
                tileRect.y = (ushort)(tilePair.Key.Y * RFX_Progressive_CONST.CONTEXT_TILESIZE);
                tileRect.width = tilePair.Key.WidthInSurface;
                tileRect.height = tilePair.Key.HeightInSurface;

                tileRectList.Add(tileRect);
            }
            TS_RFX_RECT[] tileRects = tileRectList.ToArray();
            EncodedTile[] tileDataArr = TileDict.Values.ToArray();

            bool bProg = (tileBlockType == RFXProgCodecBlockType.WBT_TILE_SIMPLE) ? false : true;

            RFX_Progressive_REGION region_block = new RFX_Progressive_REGION(bProg, tfFlag, tileDataLength, tileRects, tileDataArr);
            return region_block;
        }

        /// <summary>
        /// Create region blocks and tile data blocks(tile simple or tile first or tile upgrade).
        /// </summary>
        /// <param name="bReduceExtrapolate">This is used to indicate if DWT uses the "Reduce Extrapolate" method.</param>
        /// <param name="tileDict">This is used to indicate the dictionary of tile index and encoded data.</param>
        /// <param name="tileBlockType">This is used to indicate the tile data block type(simple, first, or upgrade).</param>
        public virtual void CreateRegionTileBlocks(bool bReduceExtrapolate, Dictionary<TileIndex, EncodedTile> tileDict, RFXProgCodecBlockType tileBlockType)
        {
            List<RFX_Progressive_DataBlock> tile_block_list = new List<RFX_Progressive_DataBlock>();
            uint tileDataLength = 0;

            foreach (KeyValuePair<TileIndex, EncodedTile> tilePair in tileDict)
            {
                // Set quantIdx is always 0 since all tiles share same quant table.
                RFX_Progressive_DataBlock tile_block = BuildTileDataBlock(0, tilePair.Key, tilePair.Value, tileBlockType);  
                tile_block_list.Add(tile_block);
                tileDataLength += tile_block.blockLen;
            }

            RFX_Progressive_REGION region_block = BuildRegionBlock(bReduceExtrapolate, tileDict, tileDataLength, tileBlockType);

            blkList.Add(region_block);
            // Add tile_data blocks after region block
            blkList.AddRange(tile_block_list);
        }

        /// <summary>
        /// Return a binary array which is encoded by Rfx Progressive Codec without region/tile data.
        /// </summary>
        /// <param name="bSync">This is used to indicate if sync block needed.</param>
        /// <param name="bContext">This is used to indicate if context block needed.</param>
        /// <param name="bSubDiff">This is used to indicate if subband_diffing flag is enabled in context block.</param>
        public byte[] PackRfxProgCodecDataBlock(bool bSync, bool bContext, bool bSubDiff)
        {
            // Clear existing blocks before create a new RFX_progressive_datablock frame.
            blkList.Clear();

            // Create frame begin block and previous sync/context block if needed.
            CreateBeginBlocks(bSync, bContext, bSubDiff, 0); // No region block here.
            CreateFrameEndBlock();

            return ToBytes();
        }

        /// <summary>
        /// Return a binary array for a tile data which is encoded by Rfx Progressive Codec.
        /// </summary>
        /// <param name="bSync">This is used to indicate if sync block needed.</param>
        /// <param name="bContext">This is used to indicate if context block needed.</param>
        /// <param name="bSubDiff">This is used to indicate if subband_diffing flag is enabled in context block.</param>
        /// <param name="bReduceExtrapolate">This is used to indicate if DWT uses the "Reduce Extrapolate" method.</param>
        /// <param name="tileDict">This is used to indicate the dictionary of tile index and encoded data.</param>
        /// <param name="tileDataType">This is used to indicate the tile data type(simple, first, or upgrade).</param>
        public byte[] PackRfxProgCodecDataBlock(bool bSync, bool bContext, bool bSubDiff, bool bReduceExtrapolate,
                                            Dictionary<TileIndex, EncodedTile> tileDict, RFXProgCodecBlockType tileDataType)
        {
            // Clear existing blocks before create a new RFX_progressive_datablock frame.
            blkList.Clear();
            ushort regionCount = 1;   // Set region count as 1 by default.

            // Create frame begin block and previous sync/context block if needed.
            CreateBeginBlocks(bSync, bContext, bSubDiff, regionCount);

            // Create region and tile data blocks.
            CreateRegionTileBlocks(bReduceExtrapolate, tileDict, tileDataType);

            // Create and add frame_end block.
            CreateFrameEndBlock();

            return ToBytes();
        }

        public byte[] ToBytes()
        {
            return PduMarshaler.Marshal(this);
        }
        #endregion

    }
}
