// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;


namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The manage class for surfaces
    /// </summary>
    public class SurfaceManager
    {
        ushort maxId = 0;   // next surface ID to be allocated
        Dictionary<ushort, Surface> surfList = new Dictionary<ushort, Surface>();  // surfList includes the previous and current surfaces for RFX progressive encoding

        /// <summary>
        /// Create a new surface
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Heigth</param>
        /// <param name="surfaceId">Specify the surface ID, if this value is null, the method will use a self-generated ID</param>
        /// <returns></returns>
        public Surface CreateSurface(ushort w, ushort h, ushort? surfaceId = null)
        {
            if (surfList.Count >= 0xff)  // too much surface is created!
            {
                return null;
            }

            if (surfaceId == null)
            {
                surfaceId = maxId;
                maxId++;
                while (surfList.ContainsKey(maxId))
                {
                    maxId++;
                }
            }
            Surface newSuf = new Surface(surfaceId.Value, w, h);
            surfList.Add(newSuf.Id, newSuf);                    

            return newSuf;
        }

        /// <summary>
        /// Delete a surface
        /// </summary>
        /// <param name="surfaceId">ID of surface to be deleted</param>
        public void DeleteSurface(ushort surfaceId)
        {
            if (surfList.ContainsKey(surfaceId))
            {
                surfList.Remove(surfaceId);
            }
        }

        public void Reset()
        {
            maxId = 0;
            surfList.Clear();
        }
    }

    /// <summary>
    /// Represents a rect area.
    /// </summary>
    public class Surface
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the ID that MUST be assigned to the surface once it has been created.
        /// </summary>
        public ushort Id;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the width of the surface.
        /// </summary>
        public ushort Width;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the width of the surface.
        /// </summary>
        public ushort Height;

        /// <summary>
        /// Last Frame
        /// </summary>
        public SurfaceFrame LastFrame;

        /// <summary>
        /// Current Frame
        /// </summary>
        public SurfaceFrame CurrentFrame;

        TileIndex[] pendingUpdateIndexs;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The specified surface id.</param>
        /// <param name="w">Surface width</param>
        /// <param name="h">Surface height</param>
        public Surface(ushort id, ushort w, ushort h)
        {
            Id = id;
            Width = w;
            Height = h;
            LastFrame = null;
        }

        /// <summary>
        /// Get the tile indexes which covers the specified area
        /// </summary>
        /// <param name="rect">The rect area</param>
        /// <returns>The tile indexes cover the specified area</returns>
        public TileIndex[] GetIndexesInRect(Rectangle[] rects)
        {
            List<TileIndex> indexList = new List<TileIndex>();
            foreach (Rectangle rect in rects)
            {
                int leftXIndex = rect.X / RdpegfxTileUtils.TileSize;
                int topYIndex = rect.Y / RdpegfxTileUtils.TileSize;
                int rightXIndex = (rect.X + rect.Width) / RdpegfxTileUtils.TileSize;
                int bottomYIndex = (rect.Y + rect.Height) / RdpegfxTileUtils.TileSize;


                for (int x = leftXIndex; x <= rightXIndex; x++)
                {
                    for (int y = topYIndex; y <= bottomYIndex; y++)
                    {
                        TileIndex index = new TileIndex((ushort)x, (ushort)y, (rect.X + rect.Width), (rect.Y + rect.Height));
                        if (!indexList.Contains(index) && IsIndexInScope(index))
                        {
                            indexList.Add(index);
                        }
                    }
                }
            }
            return indexList.ToArray();
        }


        /// <summary>
        /// Update surface to new bitmap
        /// </summary>
        /// <param name="bmp">Bitmap which used to update surface</param>
        public void UpdateFromBitmap(Bitmap bmp)
        {
            if (pendingUpdateIndexs != null && LastFrame != null)
            {
                foreach (TileIndex index in pendingUpdateIndexs)
                {
                    LastFrame.UpdateTileRgb(index, CurrentFrame.GetRgbTile(index));
                }
            }
            else
            {
                LastFrame = CurrentFrame;
            }
            CurrentFrame = SurfaceFrame.GetFromImage(Id, bmp);
            pendingUpdateIndexs = CurrentFrame.GetAllIndexes();
        }

        /// <summary>
        /// Update surface from the changed area of the bitmap frame
        /// </summary>
        /// <param name="bmp">Bitmap which used to update surface</param>
        /// <param name="changedArea">The changed areas</param>
        public void UpdateFromBitmap(Bitmap bmp, Rectangle[] changedAreas)
        {
            if (pendingUpdateIndexs != null && LastFrame !=null)
            {
                foreach (TileIndex index in pendingUpdateIndexs)
                {
                    LastFrame.UpdateTileRgb(index, CurrentFrame.GetRgbTile(index));
                }
            }
            else
            {
                LastFrame = CurrentFrame;
            }
            pendingUpdateIndexs = this.GetIndexesInRect(changedAreas);
            CurrentFrame = SurfaceFrame.GetFromImage(Id, bmp, pendingUpdateIndexs);
        }

        /// <summary>
        /// Get all the tile indexes in this surface
        /// </summary>
        /// <returns>TileIndex array.</returns>
        public TileIndex[] GetAllIndexes()
        {
            List<TileIndex> indexList = new List<TileIndex>();
            for (int xIndex = 0; xIndex * RdpegfxTileUtils.TileSize < this.Width; xIndex++)
            {
                for (int yIndex = 0; yIndex * RdpegfxTileUtils.TileSize < this.Height; yIndex++)
                {
                    TileIndex tIndex = new TileIndex((ushort)xIndex, (ushort)yIndex, this.Width, this.Height);

                    indexList.Add(tIndex);
                }
            }
            return indexList.ToArray();
        }

        /// <summary>
        /// Check if a given index in the scope of this surface
        /// </summary>
        /// <param name="index">Specified</param>
        /// <returns></returns>
        public bool IsIndexInScope(TileIndex index)
        {
            return (index.X * RdpegfxTileUtils.TileSize < this.Width) && (index.Y * RdpegfxTileUtils.TileSize < this.Height);
        }

        /// <summary>
        /// Get the different tiles of this surface
        /// </summary>
        /// <param name="bRgb">If true, check if Rgb data exist for specified frame; otherwise, check if Dwt data exist.</param>
        /// <returns>The array of different tiles.</returns>
        public TileIndex[] GetDiffIndexes(bool bRgb)
        {
            if (this.pendingUpdateIndexs != null && this.pendingUpdateIndexs.Length <= RdpegfxTileUtils.TileDiffMinCount)
            {
                return this.pendingUpdateIndexs;
            }

            if (CurrentFrame == null) return null;
            if (LastFrame == null) return this.pendingUpdateIndexs;
            List<TileIndex> diffIndexList = new List<TileIndex>();
            for (int xIndex = 0; xIndex * RdpegfxTileUtils.TileSize < this.Width; xIndex++)
            {
                for (int yIndex = 0; yIndex * RdpegfxTileUtils.TileSize < this.Height; yIndex++)
                {
                    TileIndex tIndex = new TileIndex((ushort)xIndex, (ushort)yIndex, this.Width, this.Height);
                    if(CurrentFrame.IsIndexInScope(tIndex, bRgb))
                    {
                        //diffIndexList.Add(tIndex);
                        if (LastFrame != null && LastFrame.IsIndexInScope(tIndex, bRgb))
                        {
                            if (bRgb)
                            {
                                RgbTile prvRgb = LastFrame.GetRgbTile(tIndex);
                                RgbTile curRgb = CurrentFrame.GetRgbTile(tIndex);
                                if (!curRgb.EqualsWith(prvRgb)) diffIndexList.Add(tIndex);
                            }
                            else
                            {
                                DwtTile prvDwt = LastFrame.GetDwt(tIndex);
                                DwtTile curDwt = CurrentFrame.GetDwt(tIndex);
                                if (!curDwt.EqualsWith(prvDwt)) diffIndexList.Add(tIndex);
                            }
                        }
                        else
                        {
                            diffIndexList.Add(tIndex);
                        }
                    }
                }
            }
            return diffIndexList.ToArray();
            
        }

        /// <summary>
        /// Encode the surface with Progressive Codec
        /// </summary>
        /// <param name="quality">The target encoded quality.</param>
        /// <param name="bProg">Indicates if encode progressively</param>
        /// <param name="bSubDiff">Indicates if sub-diffing with last frame of this surface</param>
        /// <param name="bReduceExtrapolate">Indicates if use Reduce Extrapolate method in DWT step.</param>
        /// <returns>The dictionary of tile index and encoded tile datas.</returns>
        public Dictionary<TileIndex, EncodedTile[]> ProgressiveEncode(ImageQuality_Values quality, bool bProg, bool bSubDiff, bool bReduceExtrapolate, bool ignoreUnchangedTile=true)
        {
            Dictionary<TileIndex, EncodedTile[]> encodedTileDic = new Dictionary<TileIndex, EncodedTile[]>();
            TS_RFX_CODEC_QUANT quant = RdpegfxTileUtils.GetCodecQuant(quality);
            TileIndex[] tileIndexArr;
            if(!ignoreUnchangedTile)
            {
                tileIndexArr = GetAllIndexes();
            }
            else
            {
                tileIndexArr = GetDiffIndexes(true);
            }

            foreach (TileIndex index in tileIndexArr)
            {
                RfxProgressiveCodecContext codecContext = new RfxProgressiveCodecContext(
                    new TS_RFX_CODEC_QUANT[]{quant}, 
                    0, // quantization index of Y, set this paramter to 0 since only one quantization value in the array
                    0, // quantization index of Cb
                    0, // quantization index of Cr
                    bProg,//progressive
                    bSubDiff,//sub-diffing
                    bReduceExtrapolate);//reduce extrapolate
                TileState tState = new TileState(this, index);
                encodedTileDic.Add(index,RfxProgressiveEncoder.EncodeTile(codecContext, tState));
            }
            return encodedTileDic;
        }

        /// <summary>
        /// Decode the encoded data to surface
        /// </summary>
        /// <param name="tileDic">The dictionary of tile index and encoded tile data</param>
        public void ProgressiveDecode(Dictionary<TileIndex, EncodedTile> tileDic)
        {
            if (this.CurrentFrame == null)
            {
                this.CurrentFrame = SurfaceFrame.GetFromImage(this.Id, new Bitmap(this.Width, this.Height));
            }

            foreach (TileIndex index in tileDic.Keys)
            {
                TileState tState = new TileState(this, index);
                RfxProgressiveDecoder.DecodeTile(tileDic[index], tState);
            }
        }

        /// <summary>
        /// Render to bitmap with Rgb data.
        /// </summary>
        /// <returns>Surface bitmap.</returns>
        public Bitmap RgbToBitmap()
        {
            if (CurrentFrame != null)
            {
                return CurrentFrame.RgbToImage();
            }
            else
            {
                return new Bitmap(Width, Height);
            }
        }

        /// <summary>
        /// Render to bitmap with DWT data.
        /// </summary>
        /// <returns>Surface bitmap.</returns>
        public Bitmap DwtToBitmap()
        {
            if (CurrentFrame != null)
            {
                return CurrentFrame.DwtToImage();
            }
            else
            {
                return new Bitmap(Width, Height);
            }
        }

        /// <summary>
        /// Get the bitmap of a tile
        /// </summary>
        /// <param name="ti">Tile index</param>
        /// <returns>Tile bitmap</returns>
        public Bitmap GetTileBitmap(TileIndex ti)
        {
            RgbTile tl = CurrentFrame.GetRgbTile(ti);
            if (tl != null)
            {
                return tl.ToImage();
            }
            return null;
        }

        /// <summary>
        /// Get the specified RgbTile
        /// </summary>
        /// <param name="index">The index of tile in this surface</param>
        /// <returns>A RgbTile</returns>
        public RgbTile GetRgbTile(TileIndex index)
        {
            return CurrentFrame.GetRgbTile(index);
        }
    }

    /// <summary>
    /// Represents a frame state of a surface
    /// </summary>
    public class SurfaceFrame
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the ID that MUST be assigned to the surface once it has been created.
        /// </summary>
        public ushort SurfaceId;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the width of the surface.
        /// </summary>
        public ushort Width;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the width of the surface.
        /// </summary>
        public ushort Height;

        /// <summary>
        /// Stores the coefficients after DWT and Quantization of tiles in this surface.
        /// </summary>
        private Dictionary<TileIndex, DwtTile> dwtQDic;

        private Dictionary<TileIndex, RgbTile> rgbTileDic;

        private Dictionary<TileIndex, DwtTile> triStateDic;

        public SurfaceFrame(ushort id, ushort w, ushort h)
        {
            SurfaceId = id;
            Width = w;
            Height = h;
            dwtQDic = new Dictionary<TileIndex, DwtTile>();
            rgbTileDic = new Dictionary<TileIndex, RgbTile>();
            triStateDic = new Dictionary<TileIndex, DwtTile>();
        }

        /// <summary>
        /// Update the RGB data of the specified tile.
        /// </summary>
        /// <param name="index">The index of the tile.</param>
        /// <param name="dwtQ">The RGB data.</param>
        public void UpdateTileRgb(TileIndex index, RgbTile rgbT)
        {
            if (index.X * RdpegfxTileUtils.TileSize >= this.Width || index.Y * RdpegfxTileUtils.TileSize >= this.Height) return;
            lock (rgbTileDic)
            {
                if (rgbTileDic.ContainsKey(index))
                {
                    rgbTileDic[index] = rgbT;
                }
                else
                {
                    rgbTileDic.Add(index, rgbT);
                }
            }
        }

        /// <summary>
        /// Update the coefficients after DWT and Quantization of specified tile.
        /// </summary>
        /// <param name="index">The index of the tile.</param>
        /// <param name="dwtQ">The DwtQ coefficients.</param>
        public void UpdateTileDwtQ(TileIndex index, DwtTile dwtQ)
        {
            if (index.X * RdpegfxTileUtils.TileSize >= this.Width || index.Y * RdpegfxTileUtils.TileSize >= this.Height) return;
            lock (dwtQDic)
            {
                if (dwtQDic.ContainsKey(index))
                {
                    dwtQDic[index] = dwtQ;
                }
                else
                {
                    dwtQDic.Add(index, dwtQ);
                }
            }
        }

        /// <summary>
        /// Update the tri-state of a tile
        /// </summary>
        /// <param name="index">The tile index</param>
        /// <param name="stat">The tri-state of the specified tile</param>
        public void UpdateTriState(TileIndex index, DwtTile stat)
        {
            if (index.X * RdpegfxTileUtils.TileSize >= this.Width || index.Y * RdpegfxTileUtils.TileSize >= this.Height) return;
            lock (triStateDic)
            {
                if (triStateDic.ContainsKey(index))
                {
                    triStateDic[index] = stat;
                }
                else
                {
                    triStateDic.Add(index, stat);
                }
            }
        }

        /// <summary>
        /// Get the coefficients for the same tile of last frame after DWT and Quantization.
        /// </summary>
        /// <param name="index">The index of the tile.</param>
        public DwtTile GetDwt(TileIndex index)
        {
            if (dwtQDic.ContainsKey(index))
            {
                return dwtQDic[index];
            }
            else
            {
                return null;
            }
        }

        public DwtTile GetTriState(TileIndex index)
        {
            if (this.triStateDic.ContainsKey(index))
            {
                return triStateDic[index];
            }
            else
            {
                return null;
            }
        }

        public RgbTile GetRgbTile(TileIndex index)
        {
            if (rgbTileDic.ContainsKey(index))
            {
                return rgbTileDic[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Convert Dwts to an image object
        /// </summary>
        /// <returns>The image returned from this surface</returns>
        public Bitmap DwtToImage()
        {
            Bitmap surfImg = new Bitmap(this.Width, this.Height);
            Graphics gSurf = Graphics.FromImage(surfImg);
            foreach (TileIndex index in this.dwtQDic.Keys)
            {
                DwtTile dwtQ = dwtQDic[index];
                gSurf.DrawImage(dwtQ.ToImage(), index.X * RdpegfxTileUtils.TileSize, index.Y * RdpegfxTileUtils.TileSize);
            }
            gSurf.Dispose();
            return surfImg;
        }

        /// <summary>
        /// Write the Rgb data of this surface to a bitmap
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap RgbToImage()
        {
            Bitmap surfImg = new Bitmap(this.Width, this.Height);
            Graphics gSurf = Graphics.FromImage(surfImg);
            SolidBrush bgBrush = new SolidBrush(Color.FromArgb(0, Color.Black));
            gSurf.FillRectangle(bgBrush, new Rectangle(0, 0, surfImg.Width, surfImg.Height));
            foreach (TileIndex index in this.rgbTileDic.Keys)
            {
                RgbTile rgbT = rgbTileDic[index];
                gSurf.DrawImage(rgbT.ToImage(), index.X * RdpegfxTileUtils.TileSize, index.Y * RdpegfxTileUtils.TileSize);
            }
            gSurf.Dispose();
            return surfImg;
        }

        /// <summary>
        /// Get all the tile indexes in this surface
        /// </summary>
        /// <returns>TileIndex array.</returns>
        public TileIndex[] GetAllIndexes()
        {
            List<TileIndex> indexList = new List<TileIndex>();
            for (int xIndex = 0; xIndex * RdpegfxTileUtils.TileSize < this.Width; xIndex++)
            {
                for (int yIndex = 0; yIndex * RdpegfxTileUtils.TileSize < this.Height; yIndex++)
                {
                    TileIndex tIndex = new TileIndex((ushort)xIndex, (ushort)yIndex, this.Width, this.Height);

                    //  set tile width and height
                    if ((xIndex + 1) * RdpegfxTileUtils.TileSize > this.Width)
                        tIndex.WidthInSurface = (byte)(this.Width - xIndex * RdpegfxTileUtils.TileSize);
                    else
                        tIndex.WidthInSurface = RdpegfxTileUtils.TileSize;

                    if ((yIndex + 1) * RdpegfxTileUtils.TileSize > this.Height)
                        tIndex.HeightInSurface = (byte)(this.Height - yIndex * RdpegfxTileUtils.TileSize);
                    else
                        tIndex.HeightInSurface = RdpegfxTileUtils.TileSize;
                    indexList.Add(tIndex);
                }
            }
            return indexList.ToArray();
        }

        /// <summary>
        /// Check if a given index in the scope of this surface
        /// </summary>
        /// <param name="index">Specified</param>
        /// <param name="bRgb">If true, check if Rgb data exist for specified frame; otherwise, check if Dwt data exist.</param>
        /// <returns></returns>
        public bool IsIndexInScope(TileIndex index, bool bRgb)
        {
            if (bRgb) return rgbTileDic.ContainsKey(index);
            else return dwtQDic.ContainsKey(index);
        }

        /// <summary>
        /// Generate a surface from a bitmap
        /// </summary>
        /// <param name="sId">The surface Id</param>
        /// <param name="bm">The original bitmap</param>
        /// <returns>A surface instance</returns>
        public static SurfaceFrame GetFromImage(ushort sId, Bitmap bitmap)
        {
            SurfaceFrame surf = new SurfaceFrame(sId, (ushort)bitmap.Width, (ushort)bitmap.Height);
            for (int xIndex = 0; xIndex * RdpegfxTileUtils.TileSize < bitmap.Width; xIndex++)
            {
                for (int yIndex = 0; yIndex * RdpegfxTileUtils.TileSize < bitmap.Height; yIndex++)
                {
                    TileIndex tIndex = new TileIndex((ushort)xIndex, (ushort)yIndex, bitmap.Width, bitmap.Height);

                    RgbTile rgbTl = RgbTile.GetFromImage(bitmap, xIndex * RdpegfxTileUtils.TileSize, yIndex * RdpegfxTileUtils.TileSize);
                    surf.rgbTileDic.Add(tIndex, rgbTl);
                }
            }
            return surf;
        }

        /// <summary>
        /// Generate a surface from a bitmap, only update the specified tiles
        /// </summary>
        /// <param name="sId">The surface Id</param>
        /// <param name="bm">The original bitmap</param>
        /// <param name="changedTileIndexs">The indexes of the changed tiles</param>
        /// <returns>A surface instance</returns>
        public static SurfaceFrame GetFromImage(ushort sId, Bitmap bitmap, TileIndex[] changedTileIndexs)
        {
            SurfaceFrame surf = new SurfaceFrame(sId, (ushort)bitmap.Width, (ushort)bitmap.Height);

            foreach (TileIndex tIndex in changedTileIndexs)
            {
                RgbTile rgbTl = RgbTile.GetFromImage(bitmap, tIndex.X * RdpegfxTileUtils.TileSize, tIndex.Y * RdpegfxTileUtils.TileSize);
                surf.rgbTileDic.Add(tIndex, rgbTl);
            }
            return surf;
        }
    }
}
