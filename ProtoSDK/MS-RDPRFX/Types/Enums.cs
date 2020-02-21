// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// Enum list for the negative test.
    /// </summary>
    public enum RdprfxNegativeType
    {
        /// <summary>
        /// All fields should be set to valid value.
        /// </summary>
        None,

        /// <summary>
        /// Set the blockLen field within BlockT of TS_RFX_SYNC to an invalid value.
        /// (Less than the actual block length).
        /// </summary>
        TsRfxSync_InvalidBlockLen,

        /// <summary>
        /// Set the blockLen field within CodecChannelT of TS_RFX_FRAME_BEGIN to an invalid value.
        /// (Less than the actual block length).
        /// </summary>
        TsRfxFrameBegin_InvalidBlockLen,

        /// <summary>
        /// Send client a block with a unspecified block type.
        /// </summary>
        UnspecifiedBlockType,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.1.2   	TS_RFX_CODEC_CHANNELT	codecId (1 byte):  An 8-bit, unsigned integer. Specifies the codec ID. This field MUST be set to 0x01.	
        /// The invalid value for testing: 0x00
        /// </summary>
        TsRfxCodecChannelT_InvalidCodecId,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.1.2   	TS_RFX_CODEC_CHANNELT	channelId (1 byte):  An 8-bit, unsigned integer. Specifies the channel ID. If the blockType is set to WBT_CONTEXT (0xCCC3), then channelId MUST be set to 0xFF. For all other values of blockType, channelId MUST be set to 0x00.
        /// The invalid value for testing: 0x01
        /// </summary>
        TsRfxCodecChannelT_InvalidChannelId,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.1.3	TS_RFX_CHANNELT	width (2 bytes):  A 16-bit, signed integer. Specifies the frame width of the channel. This field MUST have value in the range of 1 to 4096.	
        /// The invalid value for testing: 0
        /// </summary>
        TsRfxChannelT_InvalidWidth_TooSmall,


        /// <summary>
        /// Negative test against field:
        /// 2.2.2.1.3	TS_RFX_CHANNELT	width (2 bytes):  A 16-bit, signed integer. Specifies the frame width of the channel. This field MUST have value in the range of 1 to 4096.
        /// The invalid value for testing: 4097
        /// </summary>
        TsRfxChannelT_InvalidWidth_TooBig,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.1.3	TS_RFX_CHANNELT	height (2 bytes):  A 16-bit, signed integer. Specifies the frame height of the channel. This field MUST have a value in the range of 1 to 2048.	
        /// The invalid value for testing: 0
        /// </summary>
        TsRfxChannelT_InvalidHeight_TooSmall,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.1.3	TS_RFX_CHANNELT	height (2 bytes):  A 16-bit, signed integer. Specifies the frame height of the channel. This field MUST have a value in the range of 1 to 2048.	
        /// The invalid value for testing: 2049
        /// </summary>
        TsRfxChannelT_InvalidHeight_TooBig,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.1.4	TS_RFX_CODEC_VERSIONT	codecId (1 byte):  An 8-bit, unsigned integer. Specifies the codec ID. This field MUST be set to 0x01.	
        /// The invalid value for testing: 0x00
        /// </summary>
        TsRfxCodecVersions_InvalidCodecId,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.1.4	TS_RFX_CODEC_VERSIONT	version (2 bytes):  A 16-bit, signed integer. This field MUST be set to 0x0100.	
        /// The invalid value for testing: 0x0000
        /// </summary>
        TsRfxCodecVersions_InvalidVersion,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.2.1	TS_RFX_SYNC	magic (4 bytes):  A 32-bit, unsigned integer. This field MUST be set to WF_MAGIC (0xCACCACCA).	
        /// The invalid value for testing: 0xBBBBBBBB
        /// </summary>
        TsRfxSync_InvalidMagic,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.2.1	TS_RFX_SYNC	version (2 bytes):  A 16-bit, unsigned integer. Indicates the version number. This field MUST be set to WF_VERSION_1_0 (0x0100).	
        /// The invalid value for testing: 0x0000
        /// </summary>
        TsRfxSync_InvalidVersion,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.2.3	TS_RFX_CHANNELS	Channel (5 bytes):  A TS_RFX_CHANNELT (section /// 2.2.2.1.3) structure. The channelId field MUST be set to 0x00.	
        /// The invalid value for testing: 0x01
        /// </summary>
        TsRfxChannels_InvalidChannelId,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.2.4	TS_RFX_CONTEXT	ctxId (1 byte):  An 8-bit unsigned integer. Specifies an identifier for this context message. This field MUST be set to 0x00.	
        /// The invalid value for testing: 0x01
        /// </summary>
        TsRfxContext_InvalidCtxId,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.2.4	TS_RFX_CONTEXT	tileSize (2 bytes):  A 16-bit unsigned integer. Specifies the tile size used by the RemoteFX codec. This field MUST be set to CT_TILE_64x64 (0x0040), indicating that a tile is 64 x 64 pixels.	
        /// The invalid value for testing: 0x0080
        /// </summary>
        TsRfxContext_InvalidTileSize,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.2.4	TS_RFX_CONTEXT	cct (2 bits):  A 2-bit unsigned integer. Specifies the color conversion transform. This field MUST be set to COL_CONV_ICT (0x1) to specify the transform defined by the equations in sections 3.1.8.1.3 and 3.1.8.2.5.	
        /// The invalid value for testing: 0x0
        /// </summary>
        TsRfxContext_InvalidCct,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.2.4	TS_RFX_CONTEXT	xft (4 bits):  A 4-bit unsigned integer. Specifies the DWT. This field MUST be set to CLW_XFORM_DWT_53_A (0x1), 	
        /// The invalid value for testing: 0x0
        /// </summary>
        TsRfxContext_InvalidXft,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.2.4	TS_RFX_CONTEXT	qt (2 bits):  A 2-bit unsigned integer. Specifies the quantization type. This field MUST be set to SCALAR_QUANTIZATION (0x1)	
        /// The invalid value for testing: 0x0
        /// </summary>
        TsRfxContext_InvalidQt,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.3.3  	TS_RFX_REGION	lrf (1 bit): A 1-bit unsigned integer. This field MUST be set to 0x1.	
        /// The invalid value for testing: 0x0
        /// </summary>
        TsRfxRegion_InvalidRegionFlags,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.3.3  	TS_RFX_REGION	regionType (2 bytes):  A 16-bit, unsigned integer. Specifies the region type. This field MUST be set to CBT_REGION (0xCAC1).	
        /// The invalid value for testing: 0xBBBB
        /// </summary>
        TsRfxRegion_InvalidRegionType,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.3.4	TS_RFX_TILESET	idx (2 bytes):  A 16-bit, unsigned integer. Specifies the identifier of the TS_RFX_CONTEXT message referenced by this TileSet message. This field MUST be set to 0x0000.	
        /// The invalid value for testing: 0x0001
        /// </summary>
        TsRfxTileSet_InvalidIdx,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.3.4	TS_RFX_TILESET	lt (1 bit):  A 1-bit field that specifies whether this is the last TS_RFX_TILESET in the region. This field MUST be set to TRUE (0x1).	
        /// The invalid value for testing: 0x0
        /// </summary>
        TsRfxTileSet_InvalidLt,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.3.4	TS_RFX_TILESET	cct (2 bits):  A 2-bit unsigned integer. Specifies the color conversion transform. This field MUST be set to COL_CONV_ICT (0x1) to specify the transform defined by the equations in sections 3.1.8.1.3 and 3.1.8.2.5.	
        /// The invalid value for testing: 0x0
        /// </summary>
        TsRfxTileSet_InvalidCct,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.3.4	TS_RFX_TILESET	xft (4 bits):  A 4-bit unsigned integer. Specifies the DWT. This field MUST be set to CLW_XFORM_DWT_53_A (0x1)	
        /// The invalid value for testing: 0x0
        /// </summary>
        TsRfxTileSet_InvalidXft,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.3.4	TS_RFX_TILESET	qt (2 bits):  A 2-bit unsigned integer. Specifies the quantization type. This field MUST be set to SCALAR_QUANTIZATION (0x1).	
        /// The invalid value for testing: 0x0
        /// </summary>
        TsRfxTileSet_InvalidQt,

        /// <summary>
        /// Negative test against field:
        /// 2.2.2.3.4	TS_RFX_TILESET	tileSize (1 byte):  An 8-bit, unsigned integer. Specifies the width and height of a tile. This field MUST be set to 0x40.	
        /// The invalid value for testing: 0x80
        /// </summary>
        TsRfxTileSet_InvalidTileSize

    }
}
