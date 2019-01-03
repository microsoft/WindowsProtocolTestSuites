// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// Enum for Image Quantity Assessment algorithms.
    /// </summary>
    public enum IQA_Algorithm
    {
        SSIM = 0,
        MSSSIM = 1,
        GSSIM = 2
    }

    /// <summary>
    /// This class is used to buffer information of the RemoteFX message before they are sent.
    /// </summary>
    public class RemoteFXContext
    {        
        public TS_RFX_REGION Region;

        public TS_RFX_TILESET TileSet;

        public EntropyAlgorithm Entropy;

    }

    /// <summary>
    /// Simulated a output screen
    /// Used as a base image to verify the output on the RDP client.
    /// </summary>
    public class SimulatedScreen
    {
        #region Internal Definition

        /// <summary>
        /// Surface
        /// </summary>
        class Surface : IDisposable
        {
            #region Vairables

            private Bitmap image;
            private ushort surfaceId;
            private ushort width;
            private ushort height;
            private List<Point> mapPoints;
            private SimulatedScreen screen;
            private PixelFormat pixelFormat;

            private Graphics imageGraphic;            

            #endregion Variables

            /// <summary>
            /// Image of this surface
            /// </summary>
            public Bitmap Image
            {
                get
                {
                    return this.image;
                }
            }

            /// <summary>
            /// Surface ID
            /// </summary>
            public ushort ID
            {
                get
                {
                    return this.surfaceId;
                }
            }

            /// <summary>
            /// Current frame
            /// This property is used for Progressive Codec decoding
            /// </summary>
            public SurfaceFrame CurrentFrame { set; get; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="screen"></param>
            /// <param name="surfaceId"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            public Surface(SimulatedScreen screen, ushort surfaceId, ushort width, ushort height, PixelFormat pixelFormat)
            {
                this.screen = screen;
                this.width = width;
                this.height = height;
                this.surfaceId = surfaceId;
                this.pixelFormat = pixelFormat;
                image = new Bitmap(width, height);
                imageGraphic = Graphics.FromImage(image);
                mapPoints = new List<Point>();
            }

            /// <summary>
            /// Map the surface to a point on the screen
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void MapToOutput(int x, int y)
            {
                mapPoints.Add(new Point(x, y));
                Graphics g = Graphics.FromImage(screen.BaseImage);
                g.DrawImage(this.image, x, y);
                // Clean up
                g.Dispose();

            }
            /// <summary>
            /// Map the surface to a scaled output
            /// </summary>
            /// <param name="x">origin x</param>
            /// <param name="y">origin y</param>
            /// <param name="w">target width</param>
            /// <param name="h">target height</param>
            public void MapToScaledOutput(int x, int y, int w, int h)
            {
                mapPoints.Add(new Point(x, y));
                Graphics g = Graphics.FromImage(screen.BaseImage);
                g.DrawImage(this.image, x, y, w, h);
                // Clean up
                g.Dispose();
            }

            /// <summary>
            /// Set a pixel in the surface.
            /// Also will update the corresponding mapped screen position.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="c"></param>
            public void SetPixel(int x, int y, Color c)
            {
                if (x < this.width && y < this.height)
                {
                    image.SetPixel(x, y, c);
                    foreach (Point p in mapPoints)
                    {
                        if (p.X + x < this.screen.BaseImage.Width && p.Y + y < this.screen.BaseImage.Height)
                        {
                            this.screen.BaseImage.SetPixel(p.X + x, p.Y + y, c);
                        }
                    }
                }
            }

            public void SolidFill(RDPGFX_COLOR32 fillPixel, RDPGFX_RECT16[] fillRects)
            {
                if (fillRects != null && fillRects.Length > 0)
                {
                    Color c = Color.Black;
                    if (this.pixelFormat == PixelFormat.PIXEL_FORMAT_ARGB_8888)
                    {
                        c = Color.FromArgb(fillPixel.XA, fillPixel.R, fillPixel.G, fillPixel.B);
                    }
                    else
                    {
                        c = Color.FromArgb(fillPixel.R, fillPixel.G, fillPixel.B);
                    }
                    Brush brush = new SolidBrush(c);
                    Rectangle[] rects = new Rectangle[fillRects.Length];
                    for (int i = 0; i < fillRects.Length; i++)
                    {
                        rects[i] = new Rectangle(fillRects[i].left, fillRects[i].top, fillRects[i].right - fillRects[i].left, fillRects[i].bottom - fillRects[i].top);
                    }
                    imageGraphic.FillRectangles(brush, rects);

                    if (mapPoints.Count > 0)
                    {
                        List<Rectangle> rectList = new List<Rectangle>();
                        foreach (Point p in mapPoints)
                        {
                            foreach(Rectangle rect in rects)
                            {
                                rectList.Add(new Rectangle(rect.Left+p.X, rect.Top+p.Y, rect.Width, rect.Height));
                            }
                        }
                        Graphics baseImageGraphics = Graphics.FromImage(screen.BaseImage);

                        baseImageGraphics.FillRectangles(brush, rectList.ToArray());
                        
                        baseImageGraphics.Dispose();
                    }

                }
            }
            /// <summary>
            /// Draw an image on the surface.
            /// Also will update the corresponding mapped screen position.
            /// </summary>
            /// <param name="srcImage"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void DrawImage(Image srcImage, ushort x, ushort y)
            {
                imageGraphic.DrawImage(srcImage, x, y);

                if (mapPoints.Count > 0)
                {
                    Graphics baseImageGraphics = Graphics.FromImage(screen.BaseImage);
                    foreach (Point p in mapPoints)
                    {
                        baseImageGraphics.DrawImage(srcImage, p.X + x, p.Y + y);
                    }
                    baseImageGraphics.Dispose();
                }
            }

            /// <summary>
            /// Draw part of an image on the surface.
            /// Also will update the corresponding mapped screen position.
            /// </summary>
            /// <param name="srcImage"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void DrawImage(Image srcImage, ushort x, ushort y, RDPGFX_RECT16 srcRect)
            {
                Rectangle rect = new Rectangle(srcRect.left, srcRect.top, srcRect.right - srcRect.left, srcRect.bottom - srcRect.top);
                imageGraphic.DrawImage(srcImage, x, y, rect, GraphicsUnit.Pixel);

                if (mapPoints.Count > 0)
                {
                    Graphics baseImageGraphics = Graphics.FromImage(screen.BaseImage);
                    foreach (Point p in mapPoints)
                    {
                        baseImageGraphics.DrawImage(srcImage, p.X + x, p.Y + y, rect, GraphicsUnit.Pixel);
                    }
                    baseImageGraphics.Dispose();
                }
            }

            /// <summary>
            /// Dispose
            /// </summary>
            public void Dispose()
            {
                RemoveFromScreen();
                imageGraphic.Dispose();
            }

            #region Private Methods

            /// <summary>
            /// Remove the Surface from the screen position mapped
            /// </summary>
            private void RemoveFromScreen()
            {
                if (mapPoints.Count > 0)
                {
                    foreach (Point p in mapPoints)
                    {
                        for (int i = p.X; i < p.X + this.width && i < screen.baseImage.Width; i++)
                        {
                            for (int j = p.Y; j < p.Y + this.height && j < screen.baseImage.Height; j++)
                            {
                                screen.baseImage.SetPixel(i, j, SimulatedScreen.OriginalColor);
                            }
                        }
                    }
                }
            }

            #endregion Private Methods

        }

        /// <summary>
        /// Item of Bitmap cache
        /// </summary>
        class CacheItem
        {
            public Bitmap Image;

            public UInt64 CacheKey;

            public ushort CacheSlot;

            public ushort Width;

            public ushort Height;

            public CacheItem(ulong cacheKey, ushort cacheSlot, Image imageSrc, RDPGFX_RECT16 rect)
            {
                this.CacheKey = cacheKey;
                this.CacheSlot = cacheSlot;
                this.Width = (ushort)(rect.right - rect.left);
                this.Height = (ushort)(rect.bottom - rect.top);
                // Create the new bitmap and associated graphics object
                this.Image = new Bitmap(this.Width, this.Height);
                Graphics graphics = Graphics.FromImage(this.Image);

                // Draw the specified section of the source bitmap to the new one
                Rectangle rectangle = new Rectangle(rect.left, rect.top, this.Width, this.Height);
                graphics.DrawImage(imageSrc, 0, 0, rectangle, GraphicsUnit.Pixel);

                // Clean up
                graphics.Dispose();

            }
        }
        #endregion Internal Definition

        #region Variable

        public const int DefaultScreenWidth = 2000;

        public const int DefaultScreenHeight = 2000;
        
        public static Color OriginalColor = Color.Black;

        /// <summary>
        /// Whether the IQA algorithm assess Y component when assess the two images
        /// </summary>
        public bool IQAAssessY;

        /// <summary>
        /// Whether the IQA algorithm assess Cb component when assess the two images
        /// </summary>
        public bool IQAAssessCb;

        /// <summary>
        /// Whether the IQA algorithm assess Cr component when assess the two images
        /// </summary>
        public bool IQAAssessCr;

        private ITestSite site;

        private int width;

        private int height;

        private Bitmap baseImage;

        private RemoteFXContext remoteFXContext;

        private Dictionary<ushort, Surface> surfaceDic;

        private Dictionary<ushort, CacheItem> bitmapCache;

        // Decompressor Glyph Storage 
        private Dictionary<ushort, Image> clearCodecGlyphStorage;

        private ImageQualityAccessment.FRIQAIndexBase iqaIndex;

        private double assessValueThreshold;

        #endregion Variable

        #region Properties

        /// <summary>
        /// Base Image which is the same as expected output on RDP Client
        /// </summary>
        public Bitmap BaseImage
        {
            get
            {
                return baseImage;
            }
        }
        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="testSite">Site of the test suite, used to save logs</param>
        /// <param name="IQAAlgorithm">IQA algorithm</param>
        /// <param name="assessValueThreshold">Assess Value Threshold</param>
        public SimulatedScreen(ITestSite testSite, IQA_Algorithm IQAAlgorithm, double assessValueThreshold)
            : this(testSite, DefaultScreenWidth, DefaultScreenHeight, IQAAlgorithm, assessValueThreshold)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="testSite">Site of the test suite, used to save logs</param>
        /// <param name="width">Width of the base image</param>
        /// <param name="height">Height of the base image</param>
        /// <param name="IQAAlgorithm">IQA algorithm</param>
        /// <param name="assessValueThreshold">Assess Value Threshold</param>
        public SimulatedScreen(ITestSite testSite, int width, int height, IQA_Algorithm IQAAlgorithm, double assessValueThreshold)
        {
            this.site = testSite;
            this.width = width;
            this.height = height;

            this.assessValueThreshold = assessValueThreshold;
            if (IQAAlgorithm == IQA_Algorithm.GSSIM)
            {
                iqaIndex = new ImageQualityAccessment.Gssim();
            }
            else if (IQAAlgorithm == IQA_Algorithm.MSSSIM)
            {
                iqaIndex = new ImageQualityAccessment.Msssim();
            }
            else
            {
                iqaIndex = new ImageQualityAccessment.Ssim();
            }

            baseImage = new Bitmap(width, height);
            FillColor(Color.Black);

            remoteFXContext = new RemoteFXContext();

            surfaceDic = new Dictionary<ushort, Surface>();
            bitmapCache = new Dictionary<ushort, CacheItem>();
            clearCodecGlyphStorage = new Dictionary<ushort, Image>();

            // Set default values for each component
            // to decide whether access this component in IQA algorithm
            this.IQAAssessY = true;
            this.IQAAssessCb = true;
            this.IQAAssessCr = true;
        }
        #endregion Constructor

        #region Public Methods

        public void Reset()
        {
            baseImage = new Bitmap(width, height);
            FillColor(Color.Black);
            surfaceDic.Clear();
            bitmapCache.Clear();
            clearCodecGlyphStorage.Clear();
            //
        }

        #region Methods for RemoteFX codec

        /// <summary>
        /// Set TS_RFX_REGION for RemoteFX codec image
        /// </summary>
        /// <param name="rfxRegion"></param>
        public void SetRemoteFXRegion(TS_RFX_REGION rfxRegion)
        {
            
            remoteFXContext.Region = rfxRegion;            
        }

        /// <summary>
        /// Set TS_RFX_TILESET for RemoteFX codec image
        /// </summary>
        /// <param name="rfxTileSet"></param>
        /// <param name="entropy"></param>
        public void SetRemoteFXTileSet(TS_RFX_TILESET rfxTileSet, EntropyAlgorithm entropy)
        {
            remoteFXContext.TileSet = rfxTileSet;
            remoteFXContext.Entropy = entropy;
        }

        /// <summary>
        /// Draw the picture on output screen
        /// SetRemoteFXRegion and SetRemoteFXTileSet must be called before calling this method
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="surfaceId"></param>
        public void RenderRemoteFXTile(ushort left, ushort top)
        {
            Bitmap image = baseImage;
            byte quantIdxY = remoteFXContext.TileSet.tiles[0].quantIdxY;
            byte quantIdxCb = remoteFXContext.TileSet.tiles[0].quantIdxCb;
            byte quantIdxCr = remoteFXContext.TileSet.tiles[0].quantIdxCr;
            RemoteFXCodecContext context = new RemoteFXCodecContext(remoteFXContext.TileSet.quantVals, quantIdxY, quantIdxCb, quantIdxCr, remoteFXContext.Entropy);
            foreach (TS_RFX_TILE tile in remoteFXContext.TileSet.tiles)
            {
                RemoteFXDecoder.DecodeTile(context, tile.YData, tile.CbData, tile.CrData);
                for (int i = 0; i < RgbTile.TileSize; i++)
                {
                    for (int j = 0; j < RgbTile.TileSize; j++)
                    {
                        int x = tile.xIdx * 64 + i;
                        int y =  tile.yIdx * 64 + j;
                        if (IsInRects(x, y, remoteFXContext.Region.rects))
                        {
                            image.SetPixel(left + x, top + y, Color.FromArgb(context.RSet[i, j], context.GSet[i, j], context.BSet[i, j]));
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Draw the picture on Surface
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="destRect"></param>
        public void RenderRemoteFXTile(ushort surfaceId, RDPGFX_RECT16 destRect)
        {

            if (surfaceDic.ContainsKey(surfaceId))
            {
                Surface sur = surfaceDic[surfaceId];

                byte quantIdxY = remoteFXContext.TileSet.tiles[0].quantIdxY;
                byte quantIdxCb = remoteFXContext.TileSet.tiles[0].quantIdxCb;
                byte quantIdxCr = remoteFXContext.TileSet.tiles[0].quantIdxCr;
                RemoteFXCodecContext context = new RemoteFXCodecContext(remoteFXContext.TileSet.quantVals, quantIdxY, quantIdxCb, quantIdxCr, remoteFXContext.Entropy);
                foreach (TS_RFX_TILE tile in remoteFXContext.TileSet.tiles)
                {
                    RemoteFXDecoder.DecodeTile(context, tile.YData, tile.CbData, tile.CrData);
                    for (int i = 0; i < RgbTile.TileSize; i++)
                    {
                        for (int j = 0; j < RgbTile.TileSize; j++)
                        {
                            int x = tile.xIdx + i;
                            int y = tile.yIdx + j;
                            if (IsInRects(x, y, remoteFXContext.Region.rects))
                            {
                                if (destRect.left + x < destRect.right && destRect.top + y < destRect.bottom)
                                {
                                    sur.SetPixel(destRect.left + x, destRect.top + y, Color.FromArgb(context.RSet[i, j], context.GSet[i, j], context.BSet[i, j]));
                                }
                            }
                        }
                    }
                }
            }

        }

        #endregion Methods for RemoteFX codec

        #region Methods for clear codec

        /// <summary>
        /// Render Clear codec image
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="pixFormat"></param>
        /// <param name="ccFlag"></param>
        /// <param name="graphIdx"></param>
        /// <param name="bmRect"></param>
        /// <param name="residualBmp"></param>
        /// <param name="bands"></param>
        /// <param name="subcodecs"></param>
        public void RenderClearCodecImage(ushort surfaceId, PixelFormat pixFormat, byte ccFlag, ushort graphIdx, RDPGFX_RECT16 bmRect, Image residualBmp,
                            Dictionary<RDPGFX_POINT16, Bitmap> bands, Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecs)
        {
            Image paint = new Bitmap(bmRect.right - bmRect.left, bmRect.bottom - bmRect.top);
            Graphics paintG = Graphics.FromImage(paint);
            if (ccFlag != 0 && ((ccFlag & ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX) != 0)
                && ((ccFlag & ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_HIT) != 0)
                && this.clearCodecGlyphStorage.ContainsKey(graphIdx))
            {
                Image srcImage = this.clearCodecGlyphStorage[graphIdx];
                Rectangle srcRect = new Rectangle(0,0, srcImage.Width, srcImage.Height);
                Rectangle destRect = new Rectangle(0, 0, bmRect.right - bmRect.left, bmRect.bottom - bmRect.top);
                paintG.DrawImage(srcImage, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {

                // Draw the first layer: residualData
                if (residualBmp != null && residualBmp.Width <= paint.Width && residualBmp.Height <= residualBmp.Height)
                {
                    paintG.DrawImage(residualBmp, 0, 0);
                }

                // Draw the second layer: bandsData 
                if (bands != null)
                {
                    foreach (RDPGFX_POINT16 pos in bands.Keys)
                    {
                        Bitmap image = bands[pos];
                        if (pos.x + image.Width <= paint.Width && pos.y + image.Height <= paint.Height)
                        {
                            paintG.DrawImage(image, pos.x, pos.y);
                        }
                    }
                }

                // Draw the third layer: subcodecData 
                if (subcodecs != null)
                {
                    foreach (RDPGFX_POINT16 pos in subcodecs.Keys)
                    {
                        Bitmap image = subcodecs[pos].bmp;
                        if (pos.x + image.Width <= paint.Width && pos.y + image.Height <= paint.Height)
                        {
                            paintG.DrawImage(image, pos.x, pos.y);
                        }
                    }
                }

                paintG.Dispose();
            }

            // Draw the image to the surface
            if (this.surfaceDic.ContainsKey(surfaceId))
            {
                Surface sur = surfaceDic[surfaceId];
                sur.DrawImage(paint, bmRect.left, bmRect.top);
            }

            // Save the image to Glyph Storage
            if (ccFlag != 0 && ((ccFlag & ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX) != 0)
                && ((ccFlag & ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_HIT) == 0))
            {
                if (this.clearCodecGlyphStorage.ContainsKey(graphIdx))
                {
                    this.clearCodecGlyphStorage.Remove(graphIdx);
                }
                this.clearCodecGlyphStorage.Add(graphIdx, paint);
            }
        }

        /// <summary>
        /// Render a batch of clear codec images
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="startGlyphIdx"></param>
        /// <param name="startGlyphPos"></param>
        /// <param name="glyphNum"></param>
        /// <param name="glyph"></param>
        public void RenderClearCodecBatch(ushort surfaceId, ushort startGlyphIdx, RDPGFX_POINT16 startGlyphPos, ushort glyphNum, Image glyph)
        {
            ushort glyphIdx = startGlyphIdx;

            if (this.surfaceDic.ContainsKey(surfaceId))
            {
                Surface sur = surfaceDic[surfaceId];
                for (int i = 0; i < glyphNum; i++)
                {
                    sur.DrawImage(glyph, (ushort)(startGlyphPos.x + i), startGlyphPos.y);
                    this.clearCodecGlyphStorage.Add(glyphIdx++, glyph);
                }
            }
        }
        #endregion Methods for clear codec

        #region Methods for progressive codec

        /// <summary>
        /// Render progressive codec image
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="tileDict"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void RenderProgressiveCodec(ushort surfaceId, Dictionary<TileIndex, EncodedTile[]> tileDict, ushort width, ushort height)
        {            
            if (surfaceDic.ContainsKey(surfaceId))
            {
                Bitmap paint = new Bitmap(width, height);
                Graphics paintGraphics = Graphics.FromImage(paint);
                if (surfaceDic[surfaceId].CurrentFrame == null)
                {
                    surfaceDic[surfaceId].CurrentFrame = new SurfaceFrame(surfaceId, width, height);
                }
                SurfaceFrame frame = surfaceDic[surfaceId].CurrentFrame;

                foreach (TileIndex index in tileDict.Keys)
                {
                    TileState state = new TileState(frame, index);
                    EncodedTile[] tiles = tileDict[index];
                    if (tiles != null)
                    {
                        for (int i = 0; i < tiles.Length; i++)
                        {
                            RfxProgressiveDecoder.DecodeTile(tiles[i], state);
                        }
                    }
                    paintGraphics.DrawImage(state.GetDwt().ToImage(), index.X * RdpegfxTileUtils.TileSize, index.Y * RdpegfxTileUtils.TileSize);
                }

                Surface sur = surfaceDic[surfaceId];
                sur.DrawImage(paint, 0, 0);

            }
        }

        #endregion Methods for progressive codec

        #region Methods for Uncompressed Image

        /// <summary>
        /// Render uncompressed image on a surface
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void RenderUncompressedImage(ushort surfaceId, Image image, ushort x, ushort y)
        {
            if (surfaceDic.ContainsKey(surfaceId))
            {
                Surface sur = surfaceDic[surfaceId];
                sur.DrawImage(image, x, y);
            }
        }

        public void RenderUncompressedImage(Image image, ushort x, ushort y)
        {
            Graphics graphic = Graphics.FromImage(baseImage);
            graphic.DrawImageUnscaled(image, x, y);
        }

        #endregion Methods for Uncompressed Image

        #region Methods for Graphic commands

        /// <summary>
        /// Create a Surface
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateSurface(ushort surfaceId, ushort width, ushort height, PixelFormat pixelFormat = PixelFormat.PIXEL_FORMAT_XRGB_8888)
        {
            Surface sur = new Surface(this, surfaceId, width, height, pixelFormat);
            this.surfaceDic.Add(surfaceId, sur);
        }

        /// <summary>
        /// Delete a Surface
        /// </summary>
        /// <param name="surfaceId"></param>
        public void DeleteSurface(ushort surfaceId)
        {
            if (this.surfaceDic.ContainsKey(surfaceId))
            {
                Surface sur = surfaceDic[surfaceId];
                surfaceDic.Remove(surfaceId);
                sur.Dispose();
            }
        }

        /// <summary>
        /// Map a Surface to output screen 
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MapSurfaceToOutput(ushort surfaceId, uint x, uint y)
        {
            if (this.surfaceDic.ContainsKey(surfaceId))
            {
                surfaceDic[surfaceId].MapToOutput((int)x, (int)y);
            }
        }

        /// <summary>
        /// Map a Surface to Scaled output screen 
        /// </summary>
        /// <param name="surfaceId">surface id</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void MapSurfaceToScaledOutput(ushort surfaceId, uint x, uint y, uint w, uint h)
        {
            if (this.surfaceDic.ContainsKey(surfaceId))
            {
                surfaceDic[surfaceId].MapToScaledOutput((int)x, (int)y, (int)w, (int)h);
            }
        }       

        /// <summary>
        /// Copy bitmap data from a source surface to a destination surface 
        /// or to replicate bitmap data within the same surface.
        /// </summary>
        /// <param name="surfaceIdSrc"></param>
        /// <param name="surfaceIdDest"></param>
        /// <param name="rectSrc"></param>
        /// <param name="destPts"></param>
        public void SurfaceToSurface(ushort surfaceIdSrc, ushort surfaceIdDest, RDPGFX_RECT16 rectSrc, RDPGFX_POINT16[] destPts)
        {
            if (surfaceDic.ContainsKey(surfaceIdSrc) && surfaceDic.ContainsKey(surfaceIdDest))
            {
                if (destPts != null && destPts.Length > 0)
                {
                    Surface surfaceSrc = surfaceDic[surfaceIdSrc];
                    Surface surfaceDest = surfaceDic[surfaceIdDest];

                    foreach (RDPGFX_POINT16 destPt in destPts)
                    {
                        surfaceDest.DrawImage(surfaceSrc.Image, destPt.x, destPt.y, rectSrc);
                    }
                }
            }
        }

        /// <summary>
        /// Copy bitmap data from a source surface to the bitmap cache
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheSlot"></param>
        /// <param name="rectSrc"></param>
        public void SurfaceToCache(ushort surfaceId, ulong cacheKey, ushort cacheSlot, RDPGFX_RECT16 rectSrc)
        {
            if(surfaceDic.ContainsKey(surfaceId))
            {
                Surface sur = surfaceDic[surfaceId];
                CacheItem cacheItem = new CacheItem(cacheKey, cacheSlot, sur.Image, rectSrc);
                bitmapCache[cacheSlot] = cacheItem;
            }
        }

        /// <summary>
        /// Evict a cache entry
        /// </summary>
        /// <param name="cacheSlot">Slot of cache to be evicted</param>
        public void EvictCacheEntry(ushort cacheSlot)
        {
            if (bitmapCache.ContainsKey(cacheSlot))
            {
                bitmapCache.Remove(cacheSlot);
            }
        }

        /// <summary>
        /// Copy bitmap data from the bitmap cache to a destination surface.
        /// </summary>
        /// <param name="cacheSlot"></param>
        /// <param name="surfaceId"></param>
        /// <param name="destPts"></param>
        public void CacheToSurface(ushort cacheSlot, ushort surfaceId, RDPGFX_POINT16[] destPts)
        {
            if (surfaceDic.ContainsKey(surfaceId) && bitmapCache.ContainsKey(cacheSlot))
            {
                if (destPts != null && destPts.Length > 0)
                {
                    Surface surfaceDest = surfaceDic[surfaceId];
                    CacheItem cacheItem = bitmapCache[cacheSlot];

                    foreach (RDPGFX_POINT16 destPt in destPts)
                    {
                        surfaceDest.DrawImage(cacheItem.Image, destPt.x, destPt.y);
                    }
                }
            }
        }

        /// <summary>
        /// Fill a collection of rectangles on a destination surface with a solid color
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="fillPixel"></param>
        /// <param name="fillRects"></param>
        public void SolidFill(ushort surfaceId, RDPGFX_COLOR32 fillPixel, RDPGFX_RECT16[] fillRects)
        {
            if (surfaceDic.ContainsKey(surfaceId))
            {
                surfaceDic[surfaceId].SolidFill(fillPixel, fillRects);
            }
        }

        #endregion Methods for Graphic commands        
        
        #region Methods for SUT display verification

        /// <summary>
        /// Compare the given image with the baseImage
        /// </summary>
        /// <param name="image"></param>
        /// <param name="shift"></param>
        /// <param name="compareRect"></param>
        /// <returns></returns>
        public bool Compare(Image image, Point shift, Rectangle compareRect, bool usingRemoteFX)
        {
            if (usingRemoteFX)
            {
                return IsSimilar(image, shift, compareRect, this.assessValueThreshold);
            }            
            return IsIdentical(image, shift, compareRect);            
        }

        #endregion Methods for SUT display verification

        #endregion Public Methods

        #region Private Methods

        private bool IsInRect(int x, int y, TS_RFX_RECT rect)
        {
            if (x >= rect.x && x < rect.x + rect.width && y >= rect.y && y < rect.y + rect.height)
                return true;
            return false;
        }

        private bool IsInRects(int x, int y, TS_RFX_RECT[] rects)
        {
            if (rects != null && rects.Length > 0)
            {
                foreach (TS_RFX_RECT rect in rects)
                {
                    if (IsInRect(x, y, rect))
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;                
        }

        /// <summary>
        /// Whether an image is identical with the baseImage
        /// This method is used to verify SUT display when not using any codec algorithm
        /// </summary>
        /// <param name="image"></param>
        /// <param name="shift"></param>
        /// <param name="compareRect"></param>
        /// <returns></returns>
        private bool IsIdentical(Image image, Point shift, Rectangle compareRect)
        {
            Bitmap bitmap = null;
            if (image is Bitmap)
            {
                bitmap = image as Bitmap;
            }
            else
            {
                bitmap = new Bitmap(image);
            }

            for (int i = compareRect.Left; i < compareRect.Right; i++)
            {
                for (int j = compareRect.Top; j < compareRect.Bottom; j++)
                {
                    if (!IsIdenticial(BaseImage.GetPixel(i, j), bitmap.GetPixel(i + shift.X, j + shift.Y)))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compare if two colors are equal, not compare the alpha value
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool IsIdenticial(Color a, Color b)
        {
            if (a.R == b.R && a.G == b.G && a.B == b.B)
                return true;
            return false;
        }

        /// <summary>
        /// Verify whether the given image is similar as the base image
        /// This method is used for SUT display verification when using some compress codec like remote FX
        /// This method used an image comparison algorithm to assess input image with a base image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="shift"></param>
        /// <param name="compareRect"></param>
        /// <param name="assessValueThreshold"></param>
        /// <returns></returns>
        private bool IsSimilar(Image image, Point shift, Rectangle compareRect, double assessValueThreshold)
        {
            Bitmap bitmap = null;
            if (image is Bitmap)
            {
                bitmap = image as Bitmap;
            }
            else
            {
                bitmap = new Bitmap(image);
            }
            
            Bitmap referenceImage = BaseImage.Clone(compareRect, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Bitmap inputImage = bitmap.Clone(new Rectangle(shift.X + compareRect.Left, shift.Y + compareRect.Top, compareRect.Width, compareRect.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            // Extend image if necessary, so as to make sure the image used for IQA assess is not smaller than size requested by SSIM/MS-SSIM/G-SSIM algorithm
            referenceImage = ExtendImage(referenceImage, ImageQualityAccessment.Ssim.MinWidth, ImageQualityAccessment.Ssim.MinHeight);
            inputImage = ExtendImage(inputImage, ImageQualityAccessment.Ssim.MinWidth, ImageQualityAccessment.Ssim.MinWidth);

            if (IQAAssessY)
            {
                // Assess Y component
                ImageQualityAccessment.AssessResult res = iqaIndex.Assess(referenceImage, inputImage, ImageQualityAccessment.UseComponent.Luma);
                double assessValueY = res.Luma;

                site.Log.Add(LogEntryKind.Comment, "SimulatedScreen: Assess Y component of Images using {0} algorithm, assess value is {1}, assess value threshold is {2}.", iqaIndex.IndexName, assessValueY, assessValueThreshold);
                if (assessValueY < assessValueThreshold)
                {
                    return false;
                }
            }

            if (IQAAssessCb)
            {
                // Assess Cb component
                ImageQualityAccessment.AssessResult res = iqaIndex.Assess(referenceImage, inputImage, ImageQualityAccessment.UseComponent.Cb);
                double assessValueCb = res.Cb;

                site.Log.Add(LogEntryKind.Comment, "SimulatedScreen: Assess Cb component of Images using {0} algorithm, assess value is {1}, assess value threshold is {2}.", iqaIndex.IndexName, assessValueCb, assessValueThreshold);
                if (assessValueCb < assessValueThreshold)
                {
                    return false;
                }
            }

            if (IQAAssessCr)
            {
                // Assess Cr component
                ImageQualityAccessment.AssessResult res = iqaIndex.Assess(referenceImage, inputImage, ImageQualityAccessment.UseComponent.Cr);
                double assessValueCr = res.Cr;

                site.Log.Add(LogEntryKind.Comment, "SimulatedScreen: Assess Cr component of Images using {0} algorithm, assess value is {1}, assess value threshold is {2}.", iqaIndex.IndexName, assessValueCr, assessValueThreshold);
                if (assessValueCr < assessValueThreshold)
                {
                    return false;
                }
            }
            return true;
        }               

        /// <summary>
        /// Fill the base image with specific color
        /// </summary>
        /// <param name="c"></param>
        private void FillColor(Color c)
        {
            Graphics baseImageGraphics = Graphics.FromImage(this.BaseImage);
            Brush brush = new SolidBrush(c);
            baseImageGraphics.FillRectangle(brush, 0, 0, BaseImage.Width, BaseImage.Height);
            baseImageGraphics.Dispose();
        }

        /// <summary>
        /// Extend a bitmap image to the size not smaller than minWidth*minHeight
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="minWidth"></param>
        /// <param name="minHeight"></param>
        /// <returns></returns>
        private Bitmap ExtendImage(Bitmap originalImage, int minWidth, int minHeight)
        {
            if (originalImage.Width >= minWidth && originalImage.Height >= minHeight)
            {
                return originalImage;
            }

            int width = Math.Max(originalImage.Width, minWidth);
            int height = Math.Max(originalImage.Height, minHeight);

            Bitmap newBitmap = new Bitmap(width, height);
            Graphics newBitmapGraphics = Graphics.FromImage(newBitmap);
            Brush brush = new SolidBrush(Color.Black);
            newBitmapGraphics.FillRectangle(brush, 0, 0, width, height);
            newBitmapGraphics.DrawImage(originalImage, 0, 0);
            newBitmapGraphics.Dispose();

            return newBitmap;
        }
        #endregion Private Methods

    }
}
