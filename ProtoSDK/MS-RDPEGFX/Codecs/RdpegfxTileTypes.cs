// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using System.Collections;
using System.Security.Cryptography;
using System.Drawing.Imaging;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// Stores the coefficients after DWT and Quantization of a tile
    /// </summary>
    public class DwtTile
    {

        /// <summary>
        /// The Y coefficients after DWT and Quantization of the tile;
        /// </summary>
        public short[] Y_DwtQ;

        /// <summary>
        /// The Y coefficients after DWT and Quantization of the tile;
        /// </summary>
        public short[] Cb_DwtQ;

        /// <summary>
        /// The Y coefficients after DWT and Quantization of the tile;
        /// </summary>
        public short[] Cr_DwtQ;

        /// <summary>
        /// Indicates if the Reduce-Extrapolate method used when DWT.
        /// </summary>
        public bool UseReduceExtrapolate;

        /// <summary>
        /// TS_RFX_CODEC_QUANT of the Tile
        /// </summary>
        public TS_RFX_CODEC_QUANT[] CodecQuantVals;

        /// <summary>
        /// Quant index of Y component
        /// </summary>
        public byte QuantIdxY;

        /// <summary>
        /// Quant index of Cb component
        /// </summary>
        public byte QuantIdxCb;

        /// <summary>
        /// Quant index of Cr component
        /// </summary>
        public byte QuantIdxCr;

        /// <summary>
        /// The Code Quant for last progressive pass
        /// </summary>
        public RFX_PROGRESSIVE_CODEC_QUANT ProgCodecQuant;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="y">Y component data</param>
        /// <param name="cb">Cb component data</param>
        /// <param name="cr">Cr component data</param>
        /// <param name="quant">Codec quantity</param>
        /// <param name="bReduceExtrapolate">Indicates if used Reduce-Extrapolate method in DWT</param>
        /// <param name="progQuant">The progressive codec quantity</param>
        public DwtTile(short[] y, short[] cb, short[] cr, TS_RFX_CODEC_QUANT[] quantVals, byte quantIdxY, byte quantIdxCb, byte quantIdxCr, bool bReduceExtrapolate, RFX_PROGRESSIVE_CODEC_QUANT progQuant = null)         
        {
            // Clone the array， make sure different DwtTitle cannot hold the same array reference
            Y_DwtQ = (short[])y.Clone(); 
            Cb_DwtQ = (short[])cb.Clone();
            Cr_DwtQ = (short[])cr.Clone();
            CodecQuantVals = quantVals;
            this.QuantIdxY = quantIdxY;
            this.QuantIdxCb = quantIdxCb;
            this.QuantIdxCr = quantIdxCr;
            UseReduceExtrapolate = bReduceExtrapolate;
            ProgCodecQuant = progQuant;

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="y">Y component data</param>
        /// <param name="cb">Cb component data</param>
        /// <param name="cr">Cr component data</param>
        public DwtTile(short[] y, short[] cb, short[] cr)
        {
            Y_DwtQ = (short[])y.Clone();
            Cb_DwtQ = (short[])cb.Clone();
            Cr_DwtQ = (short[])cr.Clone();
        }

        /// <summary>
        /// Decode and Render the tile to an image
        /// </summary>
        /// <returns>The image rendered from tile</returns>
        public Bitmap ToImage()
        {
            Bitmap tileImg = new Bitmap(RdpegfxTileUtils.TileSize, RdpegfxTileUtils.TileSize);

            RfxProgressiveCodecContext codecContext = new RfxProgressiveCodecContext(this.CodecQuantVals, this.QuantIdxY, this.QuantIdxCb, this.QuantIdxCr, UseReduceExtrapolate);
            codecContext.YComponent = new short[Y_DwtQ.Length];
            codecContext.CbComponent = new short[Cb_DwtQ.Length];
            codecContext.CrComponent = new short[Cr_DwtQ.Length];

            Y_DwtQ.CopyTo(codecContext.YComponent, 0);
            Cb_DwtQ.CopyTo(codecContext.CbComponent, 0);
            Cr_DwtQ.CopyTo(codecContext.CrComponent, 0);

            RfxProgressiveDecoder.DecodeTileFromDwtQ(codecContext);

            BitmapData bmpData = tileImg.LockBits(new Rectangle(0, 0, tileImg.Width, tileImg.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* cusor = (byte*)bmpData.Scan0.ToPointer();
                for (int y = 0; y < bmpData.Height; y++)
                {
                    for (int x = 0; x < bmpData.Width; x++)
                    {
                        cusor[0] = codecContext.BSet[x, y];
                        cusor[1] = codecContext.GSet[x, y];
                        cusor[2] = codecContext.RSet[x, y];
                        cusor += 3;
                    }
                    cusor += (bmpData.Stride - 3 * (bmpData.Width));
                }
            }
            tileImg.UnlockBits(bmpData);

            return tileImg;
        }

        /// <summary>
        /// Sub a DWT tile
        /// </summary>
        /// <param name="subTile">The DWT tile to subtract</param>
        public void Sub(DwtTile subTile)
        {
            for (int i = 0; i < this.Y_DwtQ.Length; i++)
            {
                this.Y_DwtQ[i] = (short)(this.Y_DwtQ[i] - subTile.Y_DwtQ[i]);
                this.Cb_DwtQ[i] = (short)(this.Cb_DwtQ[i] - subTile.Cb_DwtQ[i]);
                this.Cr_DwtQ[i] = (short)(this.Cr_DwtQ[i] - subTile.Cr_DwtQ[i]);
            }
        }

        /// <summary>
        /// Add a DWT tile
        /// </summary>
        /// <param name="addTile">The DWT tile to add</param>
        public void Add(DwtTile addTile)
        {
            for (int i = 0; i < this.Y_DwtQ.Length; i++)
            {
                this.Y_DwtQ[i] = (short)(this.Y_DwtQ[i] + addTile.Y_DwtQ[i]);
                this.Cb_DwtQ[i] = (short)(this.Cb_DwtQ[i] + addTile.Cb_DwtQ[i]);
                this.Cr_DwtQ[i] = (short)(this.Cr_DwtQ[i] + addTile.Cr_DwtQ[i]);
            }
        }

        /// <summary>
        /// Compare if equals with a given DWT tile
        /// </summary>
        /// <param name="cpTile">The specified DWT tile.</param>
        /// <returns>True if equals, otherwise false</returns>
        public bool EqualsWith(DwtTile cpTile)
        {
            return ShortArrayEquals(this.Y_DwtQ, cpTile.Y_DwtQ) &&
                ShortArrayEquals(this.Cb_DwtQ, cpTile.Cb_DwtQ) &&
                ShortArrayEquals(this.Cr_DwtQ, cpTile.Cr_DwtQ);
        }

        private bool ShortArrayEquals(short[] a, short[] b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
    }      

    /// <summary>
    /// Specify the index of a tile in a surface
    /// </summary>
    public struct TileIndex
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the x-index of the encoded tile in the screen tile grid. 
        /// The pixel x-coordinate is obtained by multiplying the x-index by the size of the tile.
        /// </summary>
        public ushort X;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the y-index of the encoded tile in the screen tile grid. 
        /// The pixel y-coordinate is obtained by multiplying the y-index by the size of the tile.
        /// </summary>
        public ushort Y;

        public byte WidthInSurface;  // in surface part width of a tile
        public byte HeightInSurface;  // in surface part height of a tile 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The X index</param>
        /// <param name="y">The Y index</param>
        public TileIndex(ushort x, ushort y, int totalW, int totalH)
        {
            X = x;
            Y = y;

            //  set tile width and height
            if ((x + 1) * RdpegfxTileUtils.TileSize > totalW)
                WidthInSurface = (byte)(totalW - x * RdpegfxTileUtils.TileSize);
            else
                WidthInSurface = RdpegfxTileUtils.TileSize;

            if ((y + 1) * RdpegfxTileUtils.TileSize > totalH)
                HeightInSurface = (byte)(totalH - y * RdpegfxTileUtils.TileSize);
            else
                HeightInSurface = RdpegfxTileUtils.TileSize;
        }
    }

    /// <summary>
    /// The tile data after RLGR or SRL compression
    /// </summary>
    public struct EncodedTile
    {
        /// <summary>
        /// RLGR/SRL encoded Y data
        /// </summary>
        public byte[] YEncodedData;

        /// <summary>
        /// RLGR/SRL encoded U data
        /// </summary>
        public byte[] CbEncodedData;

        /// <summary>
        /// RLGR/SRL encoded V data
        /// </summary>
        public byte[] CrEncodedData;

        /// <summary>
        /// Raw Y data
        /// </summary>
        public byte[] YRawData;

        /// <summary>
        /// Raw U data
        /// </summary>
        public byte[] CbRawData;

        /// <summary>
        /// Raw V data
        /// </summary>
        public byte[] CrRawData; 

        /// <summary>
        /// The data type of Y, Cb, Cr
        /// </summary>
        public EncodedTileType DataType;

        /// <summary>
        /// Indicates if it's a difference tile
        /// </summary>
        public bool IsDifferenceTile;

        /// <summary>
        /// Indicates if it's a difference tile
        /// </summary>
        public bool UseReduceExtrapolate;

        /// <summary>
        /// The Codec Quant used to encoded
        /// </summary>
        public TS_RFX_CODEC_QUANT[] CodecQuantVals;

        /// <summary>
        /// Quant index of Y component
        /// </summary>
        public byte QuantIdxY;

        /// <summary>
        /// Quant index of Cb component
        /// </summary>
        public byte QuantIdxCb;

        /// <summary>
        /// Quant index of Cr component
        /// </summary>
        public byte QuantIdxCr;

        /// <summary>
        /// The progressive quantization table for compressing the tile
        /// </summary>
        public RFX_PROGRESSIVE_CODEC_QUANT ProgCodecQuant;

    }

    /// <summary>
    /// This class stores DWT values for each bands in a tile
    /// </summary>
    public class DwtBands
    {
        public short[] HL1;
        public short[] LH1;
        public short[] HH1;
        public short[] HL2;
        public short[] LH2;
        public short[] HH2;
        public short[] HL3;
        public short[] LH3;
        public short[] HH3;
        public short[] LL3;

        /// <summary>
        /// Create an instance from the given DWT data
        /// </summary>
        /// <param name="data">The linearization DWT data</param>
        /// <param name="useReduceExtrapolate">Indicates if Reduce-Extrapolate method used in DWT</param>
        /// <returns>A DWTBands instance</returns>
        public static DwtBands GetFromLinearizationResult(short[] data, bool useReduceExtrapolate)
        {
            int curIdx = 0;
            DwtBands bDwt = new DwtBands();
            bDwt.HL1 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.HL1, useReduceExtrapolate)];
            bDwt.LH1 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.LH1, useReduceExtrapolate)];
            bDwt.HH1 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.HH1, useReduceExtrapolate)];
            bDwt.HL2 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.HL2, useReduceExtrapolate)];
            bDwt.LH2 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.LH2, useReduceExtrapolate)];
            bDwt.HH2 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.HH2, useReduceExtrapolate)];
            bDwt.HL3 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.HL3, useReduceExtrapolate)];
            bDwt.LH3 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.LH3, useReduceExtrapolate)];
            bDwt.HH3 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.HH3, useReduceExtrapolate)];
            bDwt.LL3 = new short[RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, useReduceExtrapolate)];


            Array.Copy(data, curIdx, bDwt.HL1, 0, bDwt.HL1.Length); curIdx += bDwt.HL1.Length;
            Array.Copy(data, curIdx, bDwt.LH1, 0, bDwt.LH1.Length); curIdx += bDwt.LH1.Length;
            Array.Copy(data, curIdx, bDwt.HH1, 0, bDwt.HH1.Length); curIdx += bDwt.HH1.Length;
            Array.Copy(data, curIdx, bDwt.HL2, 0, bDwt.HL2.Length); curIdx += bDwt.HL2.Length;
            Array.Copy(data, curIdx, bDwt.LH2, 0, bDwt.LH2.Length); curIdx += bDwt.LH2.Length;
            Array.Copy(data, curIdx, bDwt.HH2, 0, bDwt.HH2.Length); curIdx += bDwt.HH2.Length;
            Array.Copy(data, curIdx, bDwt.HL3, 0, bDwt.HL3.Length); curIdx += bDwt.HL3.Length;
            Array.Copy(data, curIdx, bDwt.LH3, 0, bDwt.LH3.Length); curIdx += bDwt.LH3.Length;
            Array.Copy(data, curIdx, bDwt.HH3, 0, bDwt.HH3.Length); curIdx += bDwt.HH3.Length;
            Array.Copy(data, curIdx, bDwt.LL3, 0, bDwt.LL3.Length); curIdx += bDwt.LL3.Length;

            return bDwt;
        }

        /// <summary>
        /// Linearization the bands
        /// </summary>
        /// <returns>Linearization result</returns>
        public short[] GetLinearizationData()
        {
            List<short> lineList = new List<short>();
            lineList.AddRange(HL1); 
            lineList.AddRange(LH1); 
            lineList.AddRange(HH1); 
            lineList.AddRange(HL2); 
            lineList.AddRange(LH2); 
            lineList.AddRange(HH2); 
            lineList.AddRange(HL3); 
            lineList.AddRange(LH3); 
            lineList.AddRange(HH3);
            lineList.AddRange(LL3);
            return lineList.ToArray();
        }
    }

    /// <summary>
    /// Represents the state of a tile in a surface
    /// </summary>
    public class TileState
    {
        /// <summary>
        /// The surface where the tile locates.
        /// </summary>
        SurfaceFrame LastFrame;

        /// <summary>
        /// The surface where the tile locates.
        /// </summary>
        SurfaceFrame NewFrame;

        /// <summary>
        /// The index of the tile in surface.
        /// </summary>
        TileIndex Index;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newFrame">The current frame contains this tile</param>
        /// <param name="lastFrame">The last frame contains this tile</param>
        /// <param name="index">The index of this tile in the surface</param>
        public TileState(SurfaceFrame newFrame, SurfaceFrame lastFrame, TileIndex index)
        {
            NewFrame = newFrame;
            LastFrame = lastFrame;
            Index = index;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="surface">The surface contains this tile</param>
        /// <param name="index">The index of this tile in surface</param>
        public TileState(Surface surface, TileIndex index)
        {
            NewFrame = surface.CurrentFrame;
            LastFrame = surface.LastFrame;
            Index = index;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newFrame">The current frame contains this tile</param>
        /// <param name="index">The index of this tile in surface</param>
        public TileState(SurfaceFrame newFrame, TileIndex index)
        {
            NewFrame = newFrame;
            Index = index;
        }
        
        /// <summary>
        /// Get the DWT data of this tile from last frame
        /// </summary>
        /// <returns>A DWT tile</returns>
        public DwtTile GetOldDwt()
        {
            if (LastFrame != null)
            {
                return LastFrame.GetDwt(Index);
            }
            return null;
        }

        /// <summary>
        /// Get the RGB data of this tile
        /// </summary>
        /// <returns>The RGB tile</returns>
        public RgbTile GetRgb()
        {
            return NewFrame.GetRgbTile(Index);
        }

        /// <summary>
        /// Update the DWT data of this tile
        /// </summary>
        /// <param name="newDwt">the DWT tile</param>
        public void UpdateDwt(DwtTile newDwt)
        {
            NewFrame.UpdateTileDwtQ(Index, newDwt);
        }

        /// <summary>
        /// Add DWT data into this tile
        /// </summary>
        /// <param name="diffDwt">the DWT tile</param>
        public void AddDwt(DwtTile diffDwt)
        {
            DwtTile orgDwt = GetDwt();
            diffDwt.Add(orgDwt);
            UpdateDwt(diffDwt);
        }

        /// <summary>
        /// Get the current DWT data of this tile
        /// </summary>
        /// <returns>a DWT tile</returns>
        public DwtTile GetDwt()
        {
            return NewFrame.GetDwt(Index);
        }

        /// <summary>
        /// Get the current tri-state of this tile
        /// </summary>
        /// <returns>a DWT tile</returns>
        public DwtTile GetTriState()
        {
            return NewFrame.GetTriState(Index);
        }

        /// <summary>
        /// Update the tri-state of this tile
        /// </summary>
        /// <param name="newDwt">a DWT tile</param>
        public void UpdateTriState(DwtTile newDwt)
        {
            NewFrame.UpdateTriState(Index, newDwt);
        }
    }

    /// <summary>
    /// Specify the type of the encoded tile data block
    /// </summary>
    public enum EncodedTileType
    {
        /// <summary>
        /// Flag that specifies it is a compression/decompression without progressive techniques.
        /// </summary>
        Simple,

        /// <summary>
        /// Flag that specifies it is a first-pass compression/decompression of a tile with progressive techniques.
        /// </summary>
        FirstPass,

        /// <summary>
        /// Flag that specifies it is an upgrade-pass compression/decompression of a tile with progressive techniques.
        /// </summary>
        UpgradePass
    }

    /// <summary>
    /// Enum of possible values for progressive chunk 
    /// </summary>
    public enum ProgressiveChunk_Values : byte
    {
        kChunk_None = 0, // Nothing has been sent yet      kChunk_25,  
        kChunk_20 = 1,
        kChunk_25 = 2,
        kChunk_50 = 3,
        kChunk_75 = 4,
        kChunk_100 = 5
    }

    /// <summary>
    /// Enum of tile components
    /// </summary>
    public enum TileComponents : byte
    {
        Y = 0,
        Cb = 1,
        Cr = 2
    }

    /// <summary>
    /// Enum of bands
    /// </summary>
    public enum BandType_Values : byte
    {
        LL3 = 0,
        HL3 = 1,
        LH3 = 2,
        HH3 = 3,
        HL2 = 4,
        LH2 = 5,
        HH2 = 6,
        HL1 = 7,
        LH1 = 8,
        HH1 = 9
    }

    /// <summary>
    /// This class specify the coordinate of a band
    /// </summary>
    public class BandRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    /// <summary>
    /// The supported values for image encoding quality
    /// </summary>
    public enum ImageQuality_Values : byte
    {
        Lossless = 0,
        High = 1,
        Midium = 2,
        Low = 3
    }
}
