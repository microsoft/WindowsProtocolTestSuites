// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// Defines the types of negative testing.
    /// </summary>
    public enum RdpegfxNegativeTypes : ushort
    {
        /// <summary>
        /// All fields should be set to valid value.
        /// </summary>
        None,

        /// <summary>
        /// Server set an incorrect capability version in Capability Confirm response
        /// </summary>
        Capability_Incorrect_Version,

        /// <summary>
        /// Server set an incorrect CapsDataLength in CapabilityConfirm response
        /// </summary>
        Capability_Incorrect_CapsDatalength,

        /// <summary>
        /// Server set an invalid capability flag in CapabilityConfirm response
        /// </summary>
        Capability_InvalidCapFlag,

        /// <summary>
        /// Server set an capability flag not in request in CapabilityConfirm response
        /// </summary>
        Capability_CapFlagNotInRequest,

        /// <summary>
        /// The server attempts to allocate cache slots which exceeds size upper limitation for default cache flag
        /// </summary>
        CacheManagement_Default_ExceedMaxCacheSize,

        /// <summary>
        /// Check if client can use an inexistent surface as source for cache successfully
        /// </summary>
        CacheManagement_SurfaceToCache_InexistentSurface,

        /// <summary>
        /// Check if client can copy cached bitmap data to inexistent surface
        /// </summary>
        CacheManagement_CacheToSurface_InexistentSurface,

        /// <summary>
        /// Check if client can copy cached bitmap from an inexistent cache slot to destination surface
        /// </summary>
        CacheManagement_CacheToSurface_InexistentCacheSlot,

        /// <summary>
        /// Check if client can handle a request of deleting an inexistent cache slot
        /// </summary>
        CacheManagement_Delete_InexistentCacheSlot,

        /// <summary>
        /// Attempt to copy bitmap from inexistent source surface to destination surface
        /// </summary>
        SurfaceManagement_InterSurfaceCopy_InexistentSrc,

        /// <summary>
        /// Attempt to copy bitmap from source surface to inexistent destination surface
        /// </summary>
        SurfaceManagement_InterSurfaceCopy_InexistentDest,

        /// <summary>
        /// Value of destPtsCount and the length of destPts doesn't match in SurfaceToSurface PDU
        /// </summary>
        SurfaceManagement_InterSurfaceCopy_DestPtsCount_Mismatch,

        /// <summary>
        /// Check if client can handle a message with compress flag is not 0x20
        /// </summary>
        RDP8Compression_IncorrectCompressFlag,

        /// <summary>
        /// Check if client can handle a message with compress type is not 0x04
        /// </summary>
        RDP8Compression_IncorrectCompressType,

        /// <summary>
        /// Check if client can handle a message with incorrect compressed pdu
        /// </summary>
        RDP8Compression_InvalidCompressPDU,

        /// <summary>
        /// Check if client can handle a message with incorrect pdu cmdId in RDPGFX_HEADER
        /// </summary>
        SurfaceToScreen_IncorrectPduCmdId,

        /// <summary>
        /// Check if client can handle a message with nested frames
        /// </summary>
        SurfaceManagement_NestedFrames,

        /// <summary>
        /// Check if client can handle a message with frameId mismatch in begin and end frame
        /// </summary>
        SurfaceManagement_FrameIdMismatch,

        /// <summary>
        /// The server attempts to allocate slots which exceeds slot upper limitation with default cache size
        /// </summary>
        CacheManagement_Default_ExceedMaxCacheSlot,

        /// <summary>
        /// The server attempts to allocate slots with exceeds slot upper limitation for the small cache case
        /// </summary>
        CacheManagement_SmallCache_ExceedMaxCacheSlot,

        /// <summary>
        /// The server attempts to allocate slots with exceeds slot upper limitation for the thin client case
        /// </summary>
        CacheManagement_ThinCient_ExceedMaxCacheslot,

        /// <summary>
        /// The server attempts to allocate cache slots which exceeds size upper limitation for small cache flag
        /// </summary>
        CacheManagement_SmallCache_ExceedMaxCacheSize,

        /// <summary>
        /// The server attempts to allocate cache slots which exceeds size upper limitation for thin client flag
        /// </summary>
        CacheManagement_ThinClient_ExceedMaxCacheSize,

        /// <summary>
        /// Server set an invalid capability in CapabilityConfirm response
        /// </summary>
        Capability_CapFlag_NotInRequest,

        /// <summary>
        /// Server set an invalid capability flag in CapabilityConfirm response
        /// </summary>
        Capability_Invalid_CapFlag,

        /// <summary>
        /// Create a new surface with surfaceId which is duplicated with another surface
        /// </summary>
        SurfaceManagement_CreateDuplicatedSurface,

        /// <summary>
        /// Solid fill pixel with mismatch format
        /// </summary>
        SurfaceManagement_SolidFill_FormatMismatch,

        /// <summary>
        /// Attempt to delete a surface, which has an inexistent surfaceId
        /// </summary>
        SurfaceManagement_DeleteInexistentSurface,

        /// <summary>
        /// Attempt to map bitmap of an inexistent surface to output
        /// </summary>
        SurfaceManagement_MapInexistentSurfaceToOutput,

        /// <summary>
        /// Attempt to fill solid color to an inexistent surface
        /// </summary>
        SurfaceManagement_SolidFill_ToInexistentSurface,

        /// <summary>
        /// Check if client can handle a message with incorrect pdu length in RDPGFX_HEADER
        /// </summary>
        SurfaceToScreen_Incorrect_PduLengthInHeader,

        /// <summary>
        /// Check if client can handle a uncompressed message without segment header
        /// </summary>
        Segmentation_Uncompressed_NoSegmentHeader,

        /// <summary>
        /// Check if client can handle an uncompressed message with segment header
        /// </summary>
        Segmentation_Uncompressed_WithSegmentHeader,

        /// <summary>
        /// Check if client can handle a compressed message without segment header
        /// </summary>
        Segmentation_Compressed_NoSegemntHeader,

        /// <summary>
        /// Check if client can handle a message with segment descriptor is neither 0xE0 nor 0xE1
        /// </summary>
        Segmentation_Incorrect_SegmentDescriptor,

        /// <summary>
        /// Check if client can handle a message with single segment with segmentCount field
        /// </summary>
        Segmentation_SingleSegment_WithSegmentCount,

        /// <summary>
        /// Check if client can handle a message with single segment with UncompressedSize field
        /// </summary>
        Segmentation_SingleSegment_WithUncompessedSize,

        /// <summary>
        /// Check if client can handle a message with single segment with SegmentArray field
        /// </summary>
        Segmentation_SingleSegment_WithSegmentArray,

        /// <summary>
        /// Check if client can handle a message with multiple segments without SegmentCount field
        /// </summary>
        Segmentation_MultiSegments_WithoutSegmentCount,

        /// <summary>
        /// Check if client can handle a message with multiple segments without UncompressedSize field
        /// </summary>
        Segmentation_MultiSegments_WithoutUncompressedSize,

        /// <summary>
        /// Check if client can handle a message with multiple segments without SegmentArray field
        /// </summary>
        Segmentation_MultiSegments_WithoutSegmentArray,

        // **** 500~699 is reserved for clearcodec stream negative test ****

        /// <summary>
        /// Check if client can handle a clearcodec stream with graph hit flag set but still have composite payload
        /// </summary>
        ClearCodec_GraphHitFlagSet_CompositePayloadExist = 500,

        /// <summary>
        /// Check if client can handle a clearcodec stream with run length factor is zero in residual layer
        /// </summary>
        ClearCodec_Residual_ZeroRunLengthFactor,

        /// <summary>
        /// Check if client can handle a clearcodec stream with run length factor1 is less than 0xff but run length factor2 exists in residual layer
        /// </summary>
        ClearCodec_Residual_RedundantRunLengthFactor2,

        /// <summary>
        /// Check if client can handle a clearcodec stream with run length factor2 is less than 0xffff but run length factor3 exists in residual layer
        /// </summary>
        ClearCodec_Residual_RedundantRunLengthFactor3,

        /// <summary>
        /// Check if client can handle a clearcodec stream with run length factor1 is 0xff but run length factor2 is missed in residual layer
        /// </summary>
        ClearCodec_Residual_AbsentRunLengthFactor2,

        /// <summary>
        /// Check if client can handle a clearcodec stream with run length factor2 is 0xffff but run length factor3 is missed in residual layer
        /// </summary>
        ClearCodec_Residual_AbsentRunLengthFactor3,

        /// <summary>
        /// Check if client can handle a clearcodec stream with vbar pixels in vbar cache hit structure in band layer
        /// </summary>
        ClearCodec_Band_VBarCacheHit_ShortVBarPixelsExist,

        /// <summary>
        /// Check if client can handle a clearcodec stream with vbar pixels in short vbar cache hit structure in band layer
        /// </summary>
        ClearCodec_Band_ShortVBarCacheHit_ShortVBarPixelsExist,

        /// <summary>
        /// Check if client can handle a clearcodec stream without vbar pixels in short vbar cache miss structure in band layer
        /// </summary>
        ClearCodec_Band_ShortVBarCacheMiss_IncorrectPixelNumber,

        /// <summary>
        /// Check if client can handle a clearcodec stream with incorrect palette count in subcodec layer
        /// </summary>
        ClearCodec_Band_Subcodec_IncorrectPaletteCount,

        //**** 700~899 is reserved for Rfx Progressive Codec stream negative test  ****

        /// <summary>
        /// Check if client can handle a clearcodec stream with bock type is less than 0xccc0
        /// </summary>
        RfxProgCodec_TooSmallBlockType= 700,

        /// <summary>
        /// Check if client can handle a clearcodec stream with bock type is larger than 0xccc7
        /// </summary>
        RfxProgCodec_TooBigBlockType,

        /// <summary>
        /// Check if client can handle a clearcodec stream with duplicated sync block
        /// </summary>
        RfxProgCodec_DuplicatedSyncBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with sync block is not first block
        /// </summary>
        RfxProgCodec_SyncBlock_IsNotFirst,

        /// <summary>
        /// Check if client can handle a clearcodec stream with sync block has wrong block length
        /// </summary>
        RfxProgCodec_SyncBlock_IncorrectLen,

        /// <summary>
        /// Check if client can handle a clearcodec stream with sync block has wrong magic number
        /// </summary>
        RfxProgCodec_SyncBlock_IncorrectMagic,

        /// <summary>
        /// Check if client can handle a clearcodec stream with sync block has wrong version
        /// </summary>
        RfxProgCodec_SyncBlock_IncorrectVersion,

        /// <summary>
        /// Check if client can handle a clearcodec stream with duplicated frame begin block
        /// </summary>
        RfxProgCodec_DuplicatedFrameBeginBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with missing frame begin block
        /// </summary>
        RfxProgCodec_MissedFrameBeginBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with nested frame block
        /// </summary>
        RfxProgCodec_NestedFrameBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with frame begin block has wrong length
        /// </summary>
        RfxProgCodec_FrameBeginBlock_IncorrectLen,

        /// <summary>
        /// Check if client can handle a clearcodec stream with frame begin block has wrong region block number
        /// </summary>
        RfxProgCodec_FrameBeginBlock_IncorrectRegionBlockNumber,

        /// <summary>
        /// Check if client can handle a clearcodec stream with duplicated frame end block
        /// </summary>
        RfxProgCodec_DuplicatedFrameEndBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with missing frame end block
        /// </summary>
        RfxProgCodec_MissedFrameEndBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with frame end block has wrong block length
        /// </summary>
        RfxProgCodec_FrameEndBlock_IncorrectLength,

        /// <summary>
        /// Check if client can handle a clearcodec stream with context block is after frame begin block
        /// </summary>
        RfxProgCodec_ContextBlock_AfterFrameBeginBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with context block has wrong length
        /// </summary>
        RfxProgCodec_ContextBlock_IncorrectLen,

        /// <summary>
        /// Check if client can handle a clearcodec stream with context block's context Id is not zero
        /// </summary>
        RfxProgCodec_ContextBlock_IncorrectContextId,

        /// <summary>
        /// Check if client can handle a clearcodec stream with context block has wrong tile size
        /// </summary>
        RfxProgCodec_ContextBlock_IncorrectTileSize,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block is before frame begin block
        /// </summary>
        RfxProgCodec_RegionBlock_BeforeFrameBeginBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block is after frame end block
        /// </summary>
        RfxProgCodec_RegionBlock_AfterFrameEndBlock,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has wrong block length
        /// </summary>
        RfxProgCodec_RegionBlock_IncorrectLen,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has wrong tile size
        /// </summary>
        RfxProgCodec_RegionBlock_IncorrectTileSize,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has wrong rects number
        /// </summary>
        RfxProgCodec_RegionBlock_IncorrectRectsNumber,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has zero rects number
        /// </summary>
        RfxProgCodec_RegionBlock_ZeroRectsNumber,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has wrong quant number
        /// </summary>
        RfxProgCodec_RegionBlock_IncorrectQuantNumber,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has zero quant number
        /// </summary>
        RfxProgCodec_RegionBlock_ZeroQuantNumber,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has wrong progquant number
        /// </summary>
        RfxProgCodec_RegionBlock_IncorrectProgQuantNumber,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has incorrect tile block number
        /// </summary>
        RfxProgCodec_RegionBlock_IncorrectTileBlockNumber,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block has invalid tile block 
        /// </summary>
        RfxProgCodec_RegionBlock_InvalidTileBlockType,

        /// <summary>
        /// Check if client can handle a clearcodec stream with region block number is mismatched with number in frame_begin block
        /// </summary>
        RfxProgCodec_RegionBlockNumberMismatch

    }
}
