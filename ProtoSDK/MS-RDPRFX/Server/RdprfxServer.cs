// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    public class RdprfxServer
    {
        public const int TileSize = 0x40;

        private TS_RFX_CODEC_QUANT codecQuant;

        public RdprfxServer()
        {
            initCodecQuant();
        }

        #region Encode Header Messages

        /// <summary>
        /// Method to create TS_RFX_SYNC.
        /// </summary>
        public TS_RFX_SYNC CreateTsRfxSync()
        {
            TS_RFX_SYNC rfxSync = new TS_RFX_SYNC();
            rfxSync.BlockT.blockType = blockType_Value.WBT_SYNC;
            rfxSync.BlockT.blockLen = 12;
            rfxSync.magic = 0xCACCACCA;
            rfxSync.version = 0x0100;
            return rfxSync;
        }

        /// <summary>
        /// Method to create TS_RFX_CODEC_VERSIONS.
        /// </summary>
        public TS_RFX_CODEC_VERSIONS CreateTsRfxCodecVersions()
        {
            TS_RFX_CODEC_VERSIONS rfxVersions = new TS_RFX_CODEC_VERSIONS();
            rfxVersions.BlockT.blockType = blockType_Value.WBT_CODEC_VERSIONS;
            rfxVersions.BlockT.blockLen = 10;
            rfxVersions.numCodecs = 0x01;
            rfxVersions.codecs = new TS_RFX_CODEC_VERSIONT();
            rfxVersions.codecs.codecId = 0x01;
            rfxVersions.codecs.version = 0x0100;

            return rfxVersions;
        }

        /// <summary>
        /// Method to create TS_RFX_CHANNELS.
        /// </summary>
        public TS_RFX_CHANNELS CreateTsRfxChannels()
        {
            TS_RFX_CHANNELS rfxChannels = new TS_RFX_CHANNELS();
            rfxChannels.BlockT.blockType = blockType_Value.WBT_CHANNELS;
            rfxChannels.BlockT.blockLen = 12;
            rfxChannels.numChannels = 0x01;
            rfxChannels.channels = new TS_RFX_CHANNELT[1];
            rfxChannels.channels[0].channelId = 0x00;
            rfxChannels.channels[0].width = RdprfxServer.TileSize;
            rfxChannels.channels[0].height = RdprfxServer.TileSize;

            return rfxChannels;
        }

        /// <summary>
        /// Method to create TS_RFX_CONTEXT.
        /// </summary>
        /// <param name="isImageMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        public TS_RFX_CONTEXT CreateTsRfxContext(OperationalMode opMode, EntropyAlgorithm entropy)
        {
            TS_RFX_CONTEXT rfxContext = new TS_RFX_CONTEXT();
            rfxContext.CodecChannelT = new TS_RFX_CODEC_CHANNELT();
            rfxContext.CodecChannelT.blockType = blockType_Value.WBT_CONTEXT;
            rfxContext.CodecChannelT.blockLen = 13;
            rfxContext.CodecChannelT.codecId = 0x01;
            rfxContext.CodecChannelT.channelId = 0xFF;
            rfxContext.ctxId = 0x00;
            rfxContext.tileSize = 0x0040;

            ushort flags = 0x0002;
            if (opMode == OperationalMode.VideoMode) flags = 0x0000;
            ushort cct = 0x0001;
            ushort xft = 0x0001;
            ushort et = 0x0001;
            if (entropy == EntropyAlgorithm.CLW_ENTROPY_RLGR3) et = 0x0004;
            ushort qt = 0x0001;
            ushort r = 0x0000;

            rfxContext.properties = 0x0000;
            rfxContext.properties |= flags;
            rfxContext.properties |= (ushort)(cct << 3);
            rfxContext.properties |= (ushort)(xft << 5);
            rfxContext.properties |= (ushort)(et << 9);
            rfxContext.properties |= (ushort)(qt << 13);
            rfxContext.properties |= (ushort)(r << 15);

            return rfxContext;
        }

        #endregion

        #region Encode Data Messages

        /// <summary>
        /// Method to create TS_RFX_FRAME_BEGIN.
        /// </summary>
        /// <param name="frameIdx">The frame index.</param>
        public TS_RFX_FRAME_BEGIN CreateTsRfxFrameBegin(uint frameIdx)
        {
            TS_RFX_FRAME_BEGIN rfxBegin = new TS_RFX_FRAME_BEGIN();
            rfxBegin.CodecChannelT = new TS_RFX_CODEC_CHANNELT();
            rfxBegin.CodecChannelT.blockType = blockType_Value.WBT_FRAME_BEGIN;
            rfxBegin.CodecChannelT.blockLen = 14;
            rfxBegin.CodecChannelT.codecId = 0x01;
            rfxBegin.CodecChannelT.channelId = 0x00;
            rfxBegin.frameIdx = frameIdx;
            rfxBegin.numRegions = 1;

            return rfxBegin;
        }

        /// <summary>
        /// Method to create TS_RFX_REGION.
        /// </summary>
        /// <param name="rects">Array of rects, if this parameter is null, will send a 64*64 rect </param>
        /// <param name="numRectsZero">A boolean variable to indicate whether the numRectsZero field of the TS_RFX_REGION is zero </param>
        public TS_RFX_REGION CreateTsRfxRegion(Rectangle[] rects = null, bool numRectsZero = false)
        {
            TS_RFX_REGION rfxRegion = new TS_RFX_REGION();
            rfxRegion.CodecChannelT = new TS_RFX_CODEC_CHANNELT();
            rfxRegion.CodecChannelT.blockType = blockType_Value.WBT_REGION;
            rfxRegion.CodecChannelT.codecId = 0x01;
            rfxRegion.CodecChannelT.channelId = 0x00;
            rfxRegion.regionFlags = 0x01;
            
            if (numRectsZero)
            {
                rfxRegion.numRects = 0;
                rfxRegion.CodecChannelT.blockLen = 15;
                rfxRegion.rects = new TS_RFX_RECT[0];
                rfxRegion.regionType = 0xCAC1;
                rfxRegion.numTilesets = 0x0001;

                return rfxRegion;
            }

            if (rects == null || rects.Length == 0)
            {
                rfxRegion.numRects = 1;
                rfxRegion.CodecChannelT.blockLen = (uint)(15 + 1 * 8);
                rfxRegion.rects = new TS_RFX_RECT[1];
                rfxRegion.rects[0].x = 0;
                rfxRegion.rects[0].y = 0;
                rfxRegion.rects[0].width = RdprfxServer.TileSize;
                rfxRegion.rects[0].height = RdprfxServer.TileSize;
            }
            else
            {
                rfxRegion.numRects = (ushort) rects.Length;
                rfxRegion.CodecChannelT.blockLen = (uint)(15 + rfxRegion.numRects * 8);
                rfxRegion.rects = new TS_RFX_RECT[rects.Length];
                for (int i = 0; i < rects.Length; i++)
                {
                    rfxRegion.rects[i].x = (ushort) rects[i].Left;
                    rfxRegion.rects[i].y = (ushort) rects[i].Top;
                    rfxRegion.rects[i].width = (ushort) rects[i].Width;
                    rfxRegion.rects[i].height = (ushort) rects[i].Height;
                }

            }
            rfxRegion.regionType = 0xCAC1;
            rfxRegion.numTilesets = 0x0001;

            return rfxRegion;
        }


        /// <summary>
        /// Method to create TS_RFX_TILESET.
        /// </summary>
        /// <param name="opMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="tileImage">A bitmap in Image type for a tile. The width and heigth must be less than or equals with 64.</param>
        /// <param name="codecQuantVals">Quant values array</param>
        /// <param name="quantIdxY">Index of Y component in Quant value array</param>
        /// <param name="quantIdxCb">Index of Cb component in Quant value array</param>
        /// <param name="quantIdxCr">Index of Cr component in Quant value array</param>
        /// <returns></returns>
        public TS_RFX_TILESET CreateTsRfxTileSet(OperationalMode opMode, EntropyAlgorithm entropy, Image tileImage,
            TS_RFX_CODEC_QUANT[] codecQuantVals = null, byte quantIdxY = 0, byte quantIdxCb = 0, byte quantIdxCr = 0)
        {
            if (codecQuantVals == null)
            {
                codecQuantVals = new TS_RFX_CODEC_QUANT[1];
                codecQuantVals[0] = codecQuant;
            }

            TS_RFX_TILESET rfxTileSet = new TS_RFX_TILESET();
            rfxTileSet.CodecChannelT = new TS_RFX_CODEC_CHANNELT();
            rfxTileSet.CodecChannelT.blockType = blockType_Value.WBT_EXTENSION;
            rfxTileSet.CodecChannelT.blockLen = 22 + 5;
            rfxTileSet.CodecChannelT.codecId = 0x01;
            rfxTileSet.CodecChannelT.channelId = 0x00;
            rfxTileSet.subtype = 0xCAC2;
            rfxTileSet.idx = 0x0000;

            ushort lt = 0x0001;
            ushort flags = 0x0002;
            if (opMode == OperationalMode.VideoMode) flags = 0x0000;
            ushort cct = 0x0001;
            ushort xft = 0x0001;
            ushort et = 0x0001;
            if (entropy == EntropyAlgorithm.CLW_ENTROPY_RLGR3) et = 0x0004;
            ushort qt = 0x0001;
            rfxTileSet.properties = lt;
            rfxTileSet.properties |= (ushort)(flags << 1);
            rfxTileSet.properties |= (ushort)(cct << 4);
            rfxTileSet.properties |= (ushort)(xft << 6);
            rfxTileSet.properties |= (ushort)(et << 10);
            rfxTileSet.properties |= (ushort)(qt << 14);

            rfxTileSet.numQuant = (byte)codecQuantVals.Length;
            rfxTileSet.tileSize = RdprfxServer.TileSize;
            rfxTileSet.numTiles = 1;

            rfxTileSet.quantVals = codecQuantVals;

            byte[] yData, cbData, crData;

            RemoteFXCodecContext encodingContext = new RemoteFXCodecContext(codecQuantVals, quantIdxY, quantIdxCb, quantIdxCr, entropy);
            RemoteFXEncoder.EncodeTile(tileImage, 0, 0, encodingContext);
            yData = encodingContext.YData;
            cbData = encodingContext.CbData;
            crData = encodingContext.CrData;

            rfxTileSet.tiles = new TS_RFX_TILE[1];
            rfxTileSet.tiles[0].BlockT.blockType = blockType_Value.CBT_TILE;
            rfxTileSet.tiles[0].BlockT.blockLen = (uint)(19 + yData.Length + cbData.Length + crData.Length);
            rfxTileSet.tiles[0].quantIdxY = quantIdxY;
            rfxTileSet.tiles[0].quantIdxCb = quantIdxCb;
            rfxTileSet.tiles[0].quantIdxCr = quantIdxCr;
            rfxTileSet.tiles[0].xIdx = 0;
            rfxTileSet.tiles[0].yIdx = 0;
            rfxTileSet.tiles[0].YLen = (ushort)yData.Length;
            rfxTileSet.tiles[0].CbLen = (ushort)cbData.Length;
            rfxTileSet.tiles[0].CrLen = (ushort)crData.Length;
            rfxTileSet.tiles[0].YData = yData;
            rfxTileSet.tiles[0].CbData = cbData;
            rfxTileSet.tiles[0].CrData = crData;

            rfxTileSet.tilesDataSize = rfxTileSet.tiles[0].BlockT.blockLen;
            rfxTileSet.CodecChannelT.blockLen = (uint)(22 + 5 * rfxTileSet.numQuant + rfxTileSet.tilesDataSize);

            return rfxTileSet;
        }


        /// <summary>
        /// Method to create TS_RFX_TILESET.
        /// </summary>
        /// <param name="opMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="tileImages">An array of bitmaps in Image type for a tile. The width and heigth must be less than or equals with 64.</param>
        /// <param name="positions">A TILE_POSITION array indicating the positions of each tile images</param>
        /// <param name="codecQuantVals">Quant values array</param>
        /// <param name="quantIdxYs">Index array of Y component in Quant value array</param>
        /// <param name="quantIdxCbs">Index array of Cb component in Quant value array</param>
        /// <param name="quantIdxCrs">Index array of Cr component in Quant value array</param>
        /// <returns></returns>
        public TS_RFX_TILESET CreateTsRfxTileSet(
            OperationalMode opMode,
            EntropyAlgorithm entropy,
            Image[] tileImages,
            TILE_POSITION[] positions,
            TS_RFX_CODEC_QUANT[] codecQuantVals = null,
            byte[] quantIdxYs = null,
            byte[] quantIdxCbs = null,
            byte[] quantIdxCrs = null
            )
        {
            if (codecQuantVals == null)
            {
                codecQuantVals = new TS_RFX_CODEC_QUANT[1];
                codecQuantVals[0] = codecQuant;
            }

            TS_RFX_TILESET rfxTileSet = new TS_RFX_TILESET();
            rfxTileSet.CodecChannelT = new TS_RFX_CODEC_CHANNELT();
            rfxTileSet.CodecChannelT.blockType = blockType_Value.WBT_EXTENSION;
            rfxTileSet.CodecChannelT.blockLen = 22 + 5;
            rfxTileSet.CodecChannelT.codecId = 0x01;
            rfxTileSet.CodecChannelT.channelId = 0x00;
            rfxTileSet.subtype = 0xCAC2;
            rfxTileSet.idx = 0x0000;

            ushort lt = 0x0001;
            ushort flags = 0x0002;
            if (opMode == OperationalMode.VideoMode) flags = 0x0000;
            ushort cct = 0x0001;
            ushort xft = 0x0001;
            ushort et = 0x0001;
            if (entropy == EntropyAlgorithm.CLW_ENTROPY_RLGR3) et = 0x0004;
            ushort qt = 0x0001;
            rfxTileSet.properties = lt;
            rfxTileSet.properties |= (ushort)(flags << 1);
            rfxTileSet.properties |= (ushort)(cct << 4);
            rfxTileSet.properties |= (ushort)(xft << 6);
            rfxTileSet.properties |= (ushort)(et << 10);
            rfxTileSet.properties |= (ushort)(qt << 14);

            rfxTileSet.numQuant = (byte)codecQuantVals.Length;
            rfxTileSet.tileSize = RdprfxServer.TileSize;
            rfxTileSet.numTiles = 1;

            rfxTileSet.quantVals = codecQuantVals;

            byte[] yData, cbData, crData;

            rfxTileSet.tiles = new TS_RFX_TILE[tileImages.Length];
            rfxTileSet.numTiles = (ushort) rfxTileSet.tiles.Length;

            for (int i = 0; i < tileImages.Length; i++)
            {
                byte quantIdxY = quantIdxYs == null ? (byte)0 : (quantIdxYs.Length > i ? quantIdxYs[i] : (byte)0);
                byte quantIdxCb = quantIdxCbs == null ? (byte)0 : (quantIdxCbs.Length > i ? quantIdxCbs[i] : (byte)0);
                byte quantIdxCr = quantIdxCrs == null ? (byte)0 : (quantIdxCrs.Length > i ? quantIdxCrs[i] : (byte)0);

                RemoteFXCodecContext encodingContext = new RemoteFXCodecContext(codecQuantVals, quantIdxY, quantIdxCb, quantIdxCr, entropy);
                RemoteFXEncoder.EncodeTile(tileImages[i], 0, 0, encodingContext);
                yData = encodingContext.YData;
                cbData = encodingContext.CbData;
                crData = encodingContext.CrData;

                rfxTileSet.tiles[i].BlockT.blockType = blockType_Value.CBT_TILE;
                rfxTileSet.tiles[i].BlockT.blockLen = (uint)(19 + yData.Length + cbData.Length + crData.Length);
                rfxTileSet.tiles[i].quantIdxY = quantIdxY;
                rfxTileSet.tiles[i].quantIdxCb = quantIdxCb;
                rfxTileSet.tiles[i].quantIdxCr = quantIdxCr;
                rfxTileSet.tiles[i].xIdx = positions[i].xIdx;
                rfxTileSet.tiles[i].yIdx = positions[i].yIdx;
                rfxTileSet.tiles[i].YLen = (ushort)yData.Length;
                rfxTileSet.tiles[i].CbLen = (ushort)cbData.Length;
                rfxTileSet.tiles[i].CrLen = (ushort)crData.Length;
                rfxTileSet.tiles[i].YData = yData;
                rfxTileSet.tiles[i].CbData = cbData;
                rfxTileSet.tiles[i].CrData = crData;

                rfxTileSet.tilesDataSize += rfxTileSet.tiles[i].BlockT.blockLen;
                rfxTileSet.CodecChannelT.blockLen = (uint)(22 + 5 * rfxTileSet.numQuant + rfxTileSet.tilesDataSize);
            }

            return rfxTileSet;
        }
         
        /// <summary>
        /// Method to create TS_RFX_TILESET.
        /// </summary>
        /// <param name="isImageMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="tileImage">A RgbTile. The width and heigth must be less than or equals with 64.</param>
        public TS_RFX_TILESET CreateTsRfxTileSet(OperationalMode opMode, EntropyAlgorithm entropy, RgbTile tileImage)
        {
            TS_RFX_TILESET rfxTileSet = new TS_RFX_TILESET();
            rfxTileSet.CodecChannelT = new TS_RFX_CODEC_CHANNELT();
            rfxTileSet.CodecChannelT.blockType = blockType_Value.WBT_EXTENSION;
            rfxTileSet.CodecChannelT.blockLen = 22 + 5;
            rfxTileSet.CodecChannelT.codecId = 0x01;
            rfxTileSet.CodecChannelT.channelId = 0x00;
            rfxTileSet.subtype = 0xCAC2;
            rfxTileSet.idx = 0x0000;

            ushort lt = 0x0001;
            ushort flags = 0x0002;
            if (opMode == OperationalMode.VideoMode) flags = 0x0000;
            ushort cct = 0x0001;
            ushort xft = 0x0001;
            ushort et = 0x0001;
            if (entropy == EntropyAlgorithm.CLW_ENTROPY_RLGR3) et = 0x0004;
            ushort qt = 0x0001;
            rfxTileSet.properties = lt;
            rfxTileSet.properties |= (ushort)(flags << 1);
            rfxTileSet.properties |= (ushort)(cct << 4);
            rfxTileSet.properties |= (ushort)(xft << 6);
            rfxTileSet.properties |= (ushort)(et << 10);
            rfxTileSet.properties |= (ushort)(qt << 14);

            rfxTileSet.numQuant = 1;
            rfxTileSet.tileSize = RdprfxServer.TileSize;
            rfxTileSet.numTiles = 1;

            rfxTileSet.quantVals = new TS_RFX_CODEC_QUANT[1];
            rfxTileSet.quantVals[0] = codecQuant;

            byte[] yData, cbData, crData;

            RemoteFXCodecContext encodingContext = new RemoteFXCodecContext(codecQuant, entropy);
            RemoteFXEncoder.EncodeTile(tileImage, 0, 0, encodingContext);
            yData = encodingContext.YData;
            cbData = encodingContext.CbData;
            crData = encodingContext.CrData;

            rfxTileSet.tiles = new TS_RFX_TILE[1];
            rfxTileSet.tiles[0].BlockT.blockType = blockType_Value.CBT_TILE;
            rfxTileSet.tiles[0].BlockT.blockLen = (uint)(19 + yData.Length + cbData.Length + crData.Length);
            rfxTileSet.tiles[0].quantIdxY = 0;
            rfxTileSet.tiles[0].quantIdxCb = 0;
            rfxTileSet.tiles[0].quantIdxCr = 0;
            rfxTileSet.tiles[0].xIdx = 0;
            rfxTileSet.tiles[0].yIdx = 0;
            rfxTileSet.tiles[0].YLen = (ushort)yData.Length;
            rfxTileSet.tiles[0].CbLen = (ushort)cbData.Length;
            rfxTileSet.tiles[0].CrLen = (ushort)crData.Length;
            rfxTileSet.tiles[0].YData = yData;
            rfxTileSet.tiles[0].CbData = cbData;
            rfxTileSet.tiles[0].CrData = crData;

            rfxTileSet.tilesDataSize = rfxTileSet.tiles[0].BlockT.blockLen;
            rfxTileSet.CodecChannelT.blockLen = 22 + 5 + rfxTileSet.tilesDataSize;

            return rfxTileSet;
        }

        /// <summary>
        /// Method to create TS_RFX_FRAME_END.
        /// </summary>
        public TS_RFX_FRAME_END CreateTsRfxFrameEnd()
        {
            TS_RFX_FRAME_END rfxEnd = new TS_RFX_FRAME_END();
            rfxEnd.CodecChannelT = new TS_RFX_CODEC_CHANNELT();
            rfxEnd.CodecChannelT.blockType = blockType_Value.WBT_FRAME_END;
            rfxEnd.CodecChannelT.blockLen = 8;
            rfxEnd.CodecChannelT.codecId = 0x01;
            rfxEnd.CodecChannelT.channelId = 0x00;

            return rfxEnd;
        }

        #endregion

        /// <summary>
        /// Set the default Codec Quant
        /// </summary>
        private void initCodecQuant()
        {
            codecQuant = new TS_RFX_CODEC_QUANT();
            codecQuant.LL3_LH3 = 0x66;
            codecQuant.HL3_HH3 = 0x66;
            codecQuant.LH2_HL2 = 0x77;
            codecQuant.HH2_LH1 = 0x88;
            codecQuant.HL1_HH1 = 0x98;
        }
    }
}
