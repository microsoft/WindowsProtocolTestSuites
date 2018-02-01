// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdprfx;

namespace Microsoft.Protocols.TestSuites.Rdpegfx
{
    class RdpegfxRfxProgCodecBlockManagerDecorator : RFX_PROGRESSIVE_BlockManager
    {
        #region Fields

        /// <summary>
        /// Test type to decide block data details.
        /// </summary>
        private RdpegfxNegativeTypes bmTestType;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor.
        /// </summary>
        public RdpegfxRfxProgCodecBlockManagerDecorator(RdpegfxNegativeTypes testType)
        {
            bmTestType = testType;
        }

        /// <summary>
        /// Create sync block if needed.
        /// </summary>
        /// <param name="bSync">This is used to indicate if sync block needed.</param>
        public override void CreateSyncBlock(bool bSync)
        {
            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_TooSmallBlockType)
            {
                RFX_Progressive_SYNC smallTypeBlcok = new RFX_Progressive_SYNC(RFX_Progressive_CONST.SYNC_VERSION);
                smallTypeBlcok.blockType -= 1;   // Set block type to 0xccbf.
                blkList.Add(smallTypeBlcok);
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_TooBigBlockType)
            {
                RFX_Progressive_SYNC bigTypeBlcok = new RFX_Progressive_SYNC(RFX_Progressive_CONST.SYNC_VERSION);
                bigTypeBlcok.blockType += 8;   // Set block type to 0xccc8.
                blkList.Add(bigTypeBlcok);
            }

            // Create sync block if needed and add to block list.
            if (bSync && bmTestType != RdpegfxNegativeTypes.RfxProgCodec_SyncBlock_IsNotFirst)
            {
                RFX_Progressive_SYNC sync_block = new RFX_Progressive_SYNC(RFX_Progressive_CONST.SYNC_VERSION);
                if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_SyncBlock_IncorrectLen)
                {
                    sync_block.blockLen += 1;   // Change blockLen into a value other than 12.
                }
                if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_SyncBlock_IncorrectMagic)
                {
                    sync_block.magic -= 1;   // Change magic into a value other than 0xCACCACCA.
                }
                if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_SyncBlock_IncorrectVersion)
                {
                    sync_block.version += 1;   // Change version into a value other than 0x0100.
                }

                blkList.Add(sync_block);

                if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_DuplicatedSyncBlock)
                {
                    RFX_Progressive_SYNC sync_block2 = new RFX_Progressive_SYNC(RFX_Progressive_CONST.SYNC_VERSION);
                    blkList.Add(sync_block2);
                }                
            }
        }

        /// <summary>
        /// Create context block if needed.
        /// </summary>
        /// <param name="bContext">This is used to indicate if context block needed.</param>
        /// <param name="bSubDiff">This is used to indicate if subband_diffing flag is enabled in context block.</param>
        public override void CreateContextBlock(bool bContext, bool bSubDiff)
        {
            // Create context block if needed and add to block list.
            if (bContext)
            {
                byte sdFlag = Convert.ToByte(bSubDiff);
                RFX_Progressive_CONTEXT context_block = new RFX_Progressive_CONTEXT(sdFlag);

                if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_ContextBlock_IncorrectLen)
                {
                    context_block.blockLen += 1; // Set block length to other than 10.
                }
                if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_ContextBlock_IncorrectContextId)
                {
                    context_block.ctxId = 0xff;  // Set context Id to other than 0.
                }
                if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_ContextBlock_IncorrectTileSize)
                {
                    context_block.tileSize += 1;  // Set tile size to other than 64.
                }

                blkList.Add(context_block);
            }
        }

        /// <summary>
        /// Create frame_begin block.
        /// </summary>
        /// <param name="regionNum">This is used to indicate the number of region block followed.</param>
        public override void CreateFrameBeginBlock(ushort regionNum)
        {            

            uint blkFrameIdx = GetFrameIndex();
            RFX_Progressive_FRAME_BEGIN frame_begin_block = new RFX_Progressive_FRAME_BEGIN(blkFrameIdx, regionNum);

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_FrameBeginBlock_IncorrectLen)
            {
                frame_begin_block.blockLen += 1;    // Set frame begin block length to other than 12.
            }

            if ((bmTestType == RdpegfxNegativeTypes.RfxProgCodec_FrameBeginBlock_IncorrectRegionBlockNumber) ||
                (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlockNumberMismatch))
            {
                frame_begin_block.regionCount += 1; // Set regionCount larger than input parameter.
            }

            blkList.Add(frame_begin_block);

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_SyncBlock_IsNotFirst)
            {
                base.CreateSyncBlock(true);
            }
        }

        /// <summary>
        /// Create frame_begin and previous blocks(sync or context block) if needed.
        /// </summary>
        /// <param name="bSync">This is used to indicate if sync block needed.</param>
        /// <param name="bContext">This is used to indicate if context block needed.</param>
        /// <param name="bSubDiff">This is used to indicate if subband_diffing flag is enabled in context block.</param>
        /// <param name="regionNum">This is used to indicate the number of region block followed.</param>
        public override void CreateBeginBlocks(bool bSync, bool bContext, bool bSubDiff, ushort regionNum)
        {
            // Clear existing blocks before create a new RFX_progressive_datablock frame.
            blkList.Clear();

            CreateSyncBlock(bSync);

            if (bmTestType != RdpegfxNegativeTypes.RfxProgCodec_ContextBlock_AfterFrameBeginBlock)
            {
                CreateContextBlock(bContext, bSubDiff);
            }

            if ((bmTestType != RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_BeforeFrameBeginBlock) &&
                (bmTestType != RdpegfxNegativeTypes.RfxProgCodec_MissedFrameBeginBlock))
            {                    
                CreateFrameBeginBlock(regionNum);
            }

            if ((bmTestType == RdpegfxNegativeTypes.RfxProgCodec_DuplicatedFrameBeginBlock) ||
                (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_NestedFrameBlock))
            {
                CreateFrameBeginBlock(regionNum);
            }

            // Negative case context block is after frame begin block.
            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_ContextBlock_AfterFrameBeginBlock)
            {
                CreateContextBlock(bContext, bSubDiff);
            }
        }

        /// <summary>
        /// Create frame_end block.
        /// </summary>
        public override void CreateFrameEndBlock()
        {
            if ((bmTestType != RdpegfxNegativeTypes.RfxProgCodec_MissedFrameEndBlock) &&
                (bmTestType != RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_AfterFrameEndBlock))
            {
                // Create frame end block.
                RFX_Progressive_FRAME_END frame_end_block = new RFX_Progressive_FRAME_END();
                if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_FrameEndBlock_IncorrectLength)
                {
                    frame_end_block.blockLen -= 1; // Set frame end block length to other than 6.
                }

                blkList.Add(frame_end_block);
            }

            if ((bmTestType == RdpegfxNegativeTypes.RfxProgCodec_NestedFrameBlock) ||
                (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_DuplicatedFrameEndBlock))
            {
                // Create a frame end block.
                base.CreateFrameEndBlock();
            }
        }

        /// <summary>
        /// Create region block based on test type.
        /// </summary>
        /// <param name="bReduceExtrapolate">This is used to indicate if DWT uses the "Reduce Extrapolate" method.</param>
        /// <param name="tileDict">This is used to indicate the dictionary of tile index and encoded data.</param>
        /// <param name="tileDataLength">This is used to indicate the encoded tile data length.</param>
        /// <param name="tileBlockType">This is used to indicate the tile data block type(simple, first, or upgrade).</param>
        public override RFX_Progressive_REGION BuildRegionBlock(bool bReduceExtrapolate, Dictionary<TileIndex, EncodedTile> tileDict, uint tileDataLength, RFXProgCodecBlockType tileBlockType)
        {
            RFX_Progressive_REGION region_block = base.BuildRegionBlock(bReduceExtrapolate, tileDict, tileDataLength, tileBlockType);
            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectLen)
            {
                region_block.blockLen += 1; // Set block length into incorrect value.
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectTileSize)
            {
                region_block.tileSize += 1; // Set tile size in region block is other than 64.
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectRectsNumber)
            {
                region_block.numRects += 1;  // Set numRects larger 1 than TS_RFX_RECT number in region_block.rects field.
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_ZeroRectsNumber)
            {
                // Set Rect number is zero
                region_block.numRects = 0;
                region_block.rects = null;
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectQuantNumber)
            {
                region_block.numQuant += 1; // Set numQuant larger 1 than TS_RFX_CODEC_QUANT number in region_block.quantVals field.
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_ZeroQuantNumber)
            {
                // Set quant data number is zero.
                region_block.numQuant = 0;
                region_block.quantVals = null;
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectProgQuantNumber)
            {
                region_block.numProgQuant += 1; // Set numProgQuant larger 1 than RFX_RPROGRESSIVE_CODEC_QUANT number in quantProgVals field.
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectTileBlockNumber)
            {
                region_block.numTiles += 1;  // Set numTiles larger 1 than tile block number in tiles field.
            }

            return region_block;
        }

        /// <summary>
        /// Create region blocks and tile data blocks(tile simple or tile first or tile upgrade) based on test type.
        /// </summary>
        /// <param name="bReduceExtrapolate">This is used to indicate if DWT uses the "Reduce Extrapolate" method.</param>
        /// <param name="tileDict">This is used to indicate the dictionary of tile index and encoded data.</param>
        /// <param name="tileBlockType">This is used to indicate the tile data block type(simple, first, or upgrade).</param>
        public override void CreateRegionTileBlocks(bool bReduceExtrapolate, Dictionary<TileIndex, EncodedTile> tileDict, RFXProgCodecBlockType tileBlockType)
        {
            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_AfterFrameEndBlock)
            {
                base.CreateFrameEndBlock();
            }

            List<RFX_Progressive_DataBlock> tile_block_list = new List<RFX_Progressive_DataBlock>();
            uint tileDataLength = 0;
            RFX_Progressive_DataBlock tile_block = new RFX_Progressive_DataBlock();

            foreach (KeyValuePair<TileIndex, EncodedTile> tilePair in tileDict)
            {
                // Set quantIdx is always 0 since all tiles share same quant table.
                tile_block = BuildTileDataBlock(0, tilePair.Key, tilePair.Value, tileBlockType);
                tile_block_list.Add(tile_block);
                tileDataLength += tile_block.blockLen;
            }

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_InvalidTileBlockType)
            {
                // Change type of last block 
                tile_block.blockType = (RFXProgCodecBlockType)0xffff; // Set invalid block type to 0xffff.
            }

            RFX_Progressive_REGION region_block = BuildRegionBlock(bReduceExtrapolate, tileDict, tileDataLength, tileBlockType);

            blkList.Add(region_block);

            if (bmTestType == RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_BeforeFrameBeginBlock)
            {
                CreateFrameBeginBlock(1);   // Set region count as 1 since encoded image data exists.
            }

            // Add tile_data blocks after region block.
            blkList.AddRange(tile_block_list);
        }
        #endregion
    }
}
