// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdprfx;

namespace Microsoft.Protocols.TestSuites.Rdpegfx
{
    /// <summary>
    /// The protocol adapter of MS-RDPEGFX client test suite.
    /// </summary>
    public interface IRdpegfxAdapter : IAdapter
    {

        /// <summary>
        /// Attach a RdpbcgrAdapter object
        /// </summary>
        /// <param name="rdpbcgrAdapter">RDPBCGR adapter</param>
        void AttachRdpbcgrAdapter(IRdpbcgrAdapter rdpbcgrAdapter);

        /// <summary>
        /// Initialize this protocol with create graphic DVC channels.
        /// </summary>
        /// <param name="rdpedycserver">RDPEDYC server instance</param>
        /// <param name="transportType">Transport type</param>
        /// <returns>True if client supports this protocol; otherwise, return false.</returns>
        bool ProtocolInitialize(RdpedycServer rdpedycserver, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP);

        /// <summary>
        /// Create graphic DVC channel.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC server instance</param>
        /// <param name="transportType">Transport type</param>
        /// <param name="channelId">The channel Id</param>
        /// <returns>True if success; otherwise, return false.</returns>
        bool CreateEGFXDvc(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType, uint? channelId = null);
        /// <summary>
        /// Set the type of current test.
        /// </summary>
        /// <param name="testType">The test type.</param>
        void SetTestType(RdpegfxNegativeTypes testType);

        /// <summary>
        /// Method to expect a Capability Advertise from client.
        /// </summary>
        /// <returns>Received capsAdv message if not NULL.</returns>
        RDPGFX_CAPS_ADVERTISE ExpectCapabilityAdvertise();

        /// <summary>
        /// Method to expect a CACHE_IMPORT_OFFER_PDU from client.
        /// </summary>
        /// <returns> Received cache import offer message if not NULL </returns>
        RDPGFX_CACHE_IMPORT_OFFER ExpectCacheImportOffer();

        /// <summary>
        /// Method to send a Capability Confirm to client.
        /// </summary>        
        /// <param name="capFlag">The valid rdpgfx_capset_version8 flag.</param>
        /// <param name="version">version of the capability</param>
        void SendCapabilityConfirm(CapsFlags capFlag, CapsVersions version = CapsVersions.RDPGFX_CAPVERSION_8);

        /// <summary>
        /// Method to send a CACHE_IMPORT_REPLY pdu to client.
        /// </summary>
        /// <param name="cacheEntries">Identify a collection of bitmap cache present on the client.</param>
        void SendCacheImportReply(RDPGFX_CACHE_ENTRY_METADATA[] cacheEntries);

        /// <summary>
        /// Method to instruct client to create a surface, and output it to client screen.
        /// </summary>
        /// <param name="w"> Width of virtual desktop.</param>
        /// <param name="h"> Height of virtual desktop.</param>
        /// <param name="monitorCount">Count of Monitors.</param>
        /// <returns>Frame Id.</returns>
        uint ResetGraphics(uint w, uint h, uint monitorCount = 1);

        /// <summary>
        /// Method to instruct client to create a surface
        /// </summary>
        /// <param name="width">Width of the surface</param>
        /// <param name="height">Height of the surface</param>
        /// <param name="pixFormat">pixel Format to fill surface</param>
        /// <param name="surfaceId">Specify a Surface ID</param>
        /// <returns>The created surface</returns>
        Surface CreateSurface(ushort width, ushort height, PixelFormat pixFormat, ushort? surfaceId = null);

        /// <summary>
        /// Method to instruct client to map a surface to output
        /// </summary>
        /// <param name="surfaceId">Surface Id</param>
        /// <param name="outputOriginX">x-coordinate of the map point</param>
        /// <param name="outputOriginY">Y-coordinate of the map point</param>
        uint MapSurfaceToOutput(ushort surfaceId, uint outputOriginX, uint outputOriginY);

        /// <summary>
        /// Method to instruct client to map a surface to scaled output
        /// </summary>
        /// <param name="surfaceId">Surface Id</param>
        /// <param name="outputOriginX">x-coordinate of the map point</param>
        /// <param name="outputOriginY">Y-coordinate of the map point</param>
        /// <param name="targetWidth">targetWidth of the output </param>
        /// <param name="targetHeight">targetHeight of the output</param>
        uint ScaledOutput(ushort surfaceId, uint outputOriginX, uint outputOriginY, uint targetWidth, uint targetHeight);

        /// <summary>
        /// Method to create a surface and map the surface to output.
        /// </summary>
        /// <param name="rect">The left-top, right-bottom position of the surface.</param>
        /// <param name="pixFormat">The pixel format filled in surface.</param>
        /// <param name="surfaceId">Specify a Surface ID</param>
        /// <returns>The created surface.</returns>
        Surface CreateAndOutputSurface(RDPGFX_RECT16 rect, PixelFormat pixFormat, ushort? surfaceId = null);
              
        /// <summary>
        /// Method to solidfill a surface with color.
        /// </summary>
        /// <param name="surf">The surface to be filled.</param>
        /// <param name="color">The color to fill the surface.</param>
        /// <param name="rects">The rectangles to be filled in the surface.</param>
        /// <param name="frameId">Specify the frame Id.</param>
        /// <returns>Frame Id.</returns>
        uint SolidFillSurface(Surface surf, RDPGFX_COLOR32 color, RDPGFX_RECT16[] rects, uint? frameId = null);

        /// <summary>
        /// Method to send nested frames.
        /// </summary>
        /// <param name="surf">The surface that the frames belong to.</param>
        /// <param name="colors">The color to fill the surface.</param>
        /// <param name="rects">The rectangles to be filled in the surface.</param>
        void SendNestedFrames(Surface surf, RDPGFX_COLOR32[] colors, RDPGFX_RECT16[] rects);

        /// <summary>
        /// Method to implement SurfaceToCache functionality.
        /// </summary>
        /// <param name="surf">The surface to be filled.</param>
        /// <param name="cacheRect">The rectangle to be cached on the surface.</param>
        /// <param name="cacheKey">The cacheKey of rectangle bitmap data on client.</param>
        /// <param name="cacheSlot">Specify a cacheslot</param>
        /// <param name="fillColor">The color that rectangle to be filled.</param>
        uint CacheSurface(Surface surf, RDPGFX_RECT16 cacheRect, ulong cacheKey, ushort? cacheSlot, RDPGFX_COLOR32? fillColor = null);

        /// <summary>
        /// Method to implement CacheToSurface functionality.
        /// </summary>
        /// <param name="surf">The surface to be filled.</param>
        /// <param name="cacheSlot">Cache slot of bitmap</param>
        /// <param name="destPoints">This is used to specify destination points of source rectangle bitmap to be copied</param>
        /// <returns>Frame Id</returns>
        uint FillSurfaceByCachedBitmap(Surface surf, ushort cacheSlot, RDPGFX_POINT16[] destPoints);

        /// <summary>
        /// Method to implement SurfaceToCache and CacheToSurface functionality.
        /// </summary>
        /// <param name="surf">The surface to be filled.</param>
        /// <param name="cacheRect">The rectangle to be cached on the surface.</param>
        /// <param name="cacheKey">The cacheKey of rectangle bitmap data on client.</param>
        /// <param name="destPoints">This is used to specify destination points of source rectangle bitmap to be copied </param>
        /// <param name="cacheSlot">Specify a cacheslot</param>
        /// <param name="fillColor">The color that rectangle to be filled.</param>
        /// <returns>Frame Id.</returns>
        uint FillSurfaceByCachedBitmap(Surface surf, RDPGFX_RECT16 cacheRect, ulong cacheKey, RDPGFX_POINT16[] destPoints, ushort? cacheSlot, RDPGFX_COLOR32? fillColor = null);

        /// <summary>
        /// Method to implement evictCacheEntry functionality
        /// </summary>
        /// <param name="cacheSlot">Cache slot</param>
        /// <returns>Frame Id</returns>
        uint EvictCachEntry(ushort cacheSlot);

        /// <summary>
        /// Method to copy bitmap of a rectangle in surface to other position.
        /// </summary>
        /// <param name="surf">The source surface where the rectangle to be copied.</param>
        /// <param name="srcRect">The rectangle to be copied.</param>
        /// <param name="destPos">The position array that rectangle is copied to.</param>
        /// <returns>Frame Id.</returns>
        uint IntraSurfaceCopy(Surface surf, RDPGFX_RECT16 srcRect, RDPGFX_POINT16[] destPos);

        /// <summary>
        /// Method to copy bitmap of a rectangle in a surface to other position in another surface.
        /// </summary>
        /// <param name="surf">The source surface where the rectangle to be copied.</param>
        /// <param name="srcRect">The rectangle to be copied.</param>
        /// <param name="fillColor">The color of rectangle to be filled.</param>
        /// <param name="surfDest">The destination surface where the rectangle is copied to.</param>
        /// <param name="destPos">The position array that rectangle is copied to.</param>
        /// <returns>Frame Id.</returns>
        uint InterSurfaceCopy(Surface surfSrc, RDPGFX_RECT16 srcRect, RDPGFX_COLOR32 fillColor, Surface surfDest, RDPGFX_POINT16[] destPos);

        /// <summary>
        /// Method to Delete a surface.
        /// </summary>
        /// <param name="sid">The ID of surface to be deleted.</param>
        void DeleteSurface(ushort sid);

        /// <summary>
        /// Method to expect a Frame Acknowledge from client.
        /// </summary>
        void ExpectFrameAck(uint fid);

        /// <summary>
        /// Method to pack rdpegfx frame into segment header and send it.
        /// </summary>
        /// <param name="frameData">The frame data to be sent.</param>
        void SendRdpegfxFrameInSegment(byte[] frameData);

        /// <summary>
        /// Encode a bitmap data by RemoteFX codec.
        /// </summary>
        /// <param name="image"> The bitmap image to be sent. </param>
        /// <param name="opMode"> indicate Operational Mode.</param>
        /// <param name="entropy"> indicate Entropy Algorithm.</param>
        /// <param name="imgPos"> The top-left position of bitmap image relative to surface.</param>
        /// <param name="sId"> The surface Id that bitmap image is sent to. </param>
        /// <param name="pixFormat"> The pixel format of bitmap image. </param>
        /// <returns> A dictionary with frameId and byte stream frame pair. </returns>
        Dictionary<uint, byte[]> RemoteFXCodecEncode(System.Drawing.Image image, OperationalMode opMode, EntropyAlgorithm entropy,
                                            RDPGFX_POINT16 imgPos, ushort sId, PixelFormat pixFormat);

        /// <summary>
        /// Send RFX Progressive codec Pdu without image data to client.
        /// </summary>
        /// <param name="sId"> The surface Id that bitmap image is sent to. </param>
        /// <param name="pixFormat">The pixel format to draw surface.</param>
        /// <param name="hasSync">Indicates if sync block exists in FRX Progressive bitmap stream.</param>
        /// <param name="hasContext">Indicates if context block exists in FRX Progressive bitmap stream.</param>
        /// <param name="bSubDiff">Indicates if sub-diffing with last frame of this surface</param>
        /// <returns>Frame ID.</returns>
        uint SendRfxProgressiveCodecPduWithoutImage(ushort sId, PixelFormat pixFormat, bool hasSync, bool hasContext, bool bSubDiff);

        /// <summary>
        /// Send bitmap data in RFX Progressive codec (one tile in one rfx_progressive_datablock frame).
        /// </summary>
        /// <param name="image"> The bitmap image to be sent. </param>
        /// <param name="surf"> The surface that bitmap image is sent to. </param>
        /// <param name="pixFormat">The pixel format to draw surface.</param>
        /// <param name="hasSync">Indicates if sync block exists in FRX Progressive bitmap stream.</param>
        /// <param name="hasContext">Indicates if context block exists in FRX Progressive bitmap stream.</param>
        /// <param name="quality">The target encoded quality.</param>
        /// <param name="bProg">Indicates if encode progressively.</param>
        /// <param name="bSubDiff">Indicates if sub-diffing with last frame of this surface.</param>
        /// <param name="bReduceExtrapolate">Indicates if use Reduce Extrapolate method in DWT step.</param>
        /// <returns> A list of layer byte stream, each layer is built by a dictionary with frameId and byte stream frame pair. </returns>
        List<Dictionary<uint, byte[]>> RfxProgressiveCodecEncode(Surface surf, System.Drawing.Image image, PixelFormat pixFormat, bool hasSync, bool hasContext,
                                                    ImageQuality_Values quality, bool bProg, bool bSubDiff, bool bReduceExtrapolate);

        /// <summary>
        /// Get a used RfxProgssiveCodec Context Id.
        /// </summary>
        /// <param name="contextId">A used context Id if exists.</param>
        /// <returns> True if codec context Id exists. </returns>
        bool GetUsedRfxProgssiveCodecContextId(ref uint contextId);

        /// <summary>
        /// Send a RDPEGFX_DELETE_ENCODING_CONTEXT_PDU to client.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="contextId">The context Id to be deleted.</param>
        /// <returns> Frame Id. </returns>
        uint DeleteRfxProgssiveCodecContextId(ushort sId, uint contextId);

        /// <summary>
        /// Send bitmap data in ClearCodec.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="ccFlag">This is used to indicate the clearcodec stream flags.</param>
        /// <param name="graphIdx">This is used to indicate the index of graph to be put in client graph cache.</param>
        /// <param name="bmRect">The rectangle of whole Image, which will be sent in clearcodec, relative to the surface. </param>
        /// <param name="residualImage"> The residual layer image to be sent. </param>
        /// <param name="bands"> The dictionary of band layer image and position. </param>
        /// <param name="subcodecs"> The dictionary of subcodec layer image, subcodecID and position. </param>
        /// <returns> Frame Id. </returns>
        uint SendImageWithClearCodec(ushort sId, PixelFormat pixFormat, byte ccFlag, ushort graphIdx, RDPGFX_RECT16 bmRect,
                                        System.Drawing.Image residualBmp, Dictionary<RDPGFX_POINT16, System.Drawing.Bitmap> bands,
                                        Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecs);

        /// <summary>
        /// Send bitmap data in H264 codec
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">The rectangle of whole Image</param>
        /// <param name="numRects">Number of rects</param>
        /// <param name="regionRects">Rect list</param>
        /// <param name="quantQualityVals">Quality list</param>
        /// <param name="avc420EncodedBitstream">encoded H264 AVC420 data stream</param>
        /// <param name="baseImage">Base Image used to verify output</param>
        /// <returns></returns>
        uint SendImageWithH264AVC420Codec(ushort sId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, uint numRects, RDPGFX_RECT16[] regionRects, RDPGFX_AVC420_QUANT_QUALITY[] quantQualityVals,
                          byte[] avc420EncodedBitstream, Image baseImage);

        /// <summary>
        /// Send bitmap data in H264 AVC420 codec
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">The rectangle of whole Image</param>
        /// <param name="avc420BitmapStream">A RFX_AVC420_BITMAP_STREAM structure for encoded information</param>
        /// <param name="baseImage">Base Image used to verify output</param>
        /// <returns></returns>
        uint SendImageWithH264AVC420Codec(ushort sId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, RFX_AVC420_BITMAP_STREAM avc420BitmapStream, Image baseImage);

        /// <summary>
        /// Send clearcodec encoded glyph in batch (make sure glyphnum + startGlyphRect.right is not bigger than surf.width).
        /// </summary>
        /// <param name="surf">This is used to indicate the target surface id.</param>
        /// <param name="startGlyphIdx">This is used to indicate the start index of graph batch to be put in client graph cache.</param>
        /// <param name="startGlyphPos">The start position of glyph batch, which will be sent in clearcodec, relative to the surface. </param>
        /// <param name="glyphNum"> The glyph number in batch. </param>
        /// <param name="glyph"> The residual layer image to be sent. </param>
        /// <returns> Frame Id. </returns>
        uint SendClearCodecGlyphInBatch(Surface surf, ushort startGlyphIdx, RDPGFX_POINT16 startGlyphPos, ushort glyphNum, System.Drawing.Image glyph);

        /// <summary>
        /// Send bitmap data in H264 AVC444 codec
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">The rectangle of whole Image</param>
        /// <param name="lcValue">Code specifies how data is encoded in the avc420EncodedBitstream1 and avc420EncodedBitstream2 fields</param>
        /// <param name="stream1NumRects">Number of rects of avc420EncodedBitstream1</param>
        /// <param name="steam1RegionRects">Rect list of avc420EncodedBitstream1</param>
        /// <param name="stream1QuantQualityVals">Quality list of avc420EncodedBitstream1</param>
        /// <param name="avc420EncodedBitstream1">encoded H264 AVC420 data stream of avc420EncodedBitstream1</param>
        /// <param name="stream2NumRects">Number of rects of avc420EncodedBitstream2</param>
        /// <param name="steam2RegionRects">Rect list of avc420EncodedBitstream2</param>
        /// <param name="stream2QuantQualityVals">Quality list of avc420EncodedBitstream2</param>
        /// <param name="avc420EncodedBitstream2">encoded H264 AVC420 data stream of avc420EncodedBitstream2</param>
        /// <param name="baseImage">Base Image used to verify output</param>
        /// <returns></returns>
        uint SendImageWithH264AVC444Codec(ushort sId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, AVC444LCValue lcValue,
            uint stream1NumRects, RDPGFX_RECT16[] steam1RegionRects, RDPGFX_AVC420_QUANT_QUALITY[] stream1QuantQualityVals, byte[] avc420EncodedBitstream1,
            uint stream2NumRects, RDPGFX_RECT16[] steam2RegionRects, RDPGFX_AVC420_QUANT_QUALITY[] stream2QuantQualityVals, byte[] avc420EncodedBitstream2,
            Image baseImage);

        /// <summary>
        /// Send bitmap data in H264 AVC444/AVC444v2 codec
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">The rectangle of whole Image</param>
        /// <param name="codec">Codec type.</param>
        /// <param name="avc444BitmapStream">An IRFX_AVC444_BITMAP_STREAM interface for encoded information</param>
        /// <param name="baseImage">Base Image used to verify output</param>
        /// <returns></returns>
        uint SendImageWithH264AVC444Codec(ushort sId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, CodecType codec, IRFX_AVC444_BITMAP_STREAM avc444BitmapStream,
            Image baseImage);

        /// <summary>
        /// Send a uncompressed bitmap data, the data segment(s) can be compressed or uncompressed according to parameter.
        /// </summary>
        /// <param name="image"> The bitmap image to be sent. </param>
        /// <param name="destLeft"> The x-coordination of bitmap image top-left position.  </param>
        /// <param name="destTop"> The y-coordination of bitmap image top-left position. </param>
        /// <param name="sId"> The surface Id that bitmap image is sent to. </param>
        /// <param name="pixFormat"> The pixel format of bitmap image. </param>
        /// <param name="compFlag"> The flag indicates whether the bitmap is compressed. </param>
        /// <param name="partSize"> The size of pure data in a single RDP8_BULK_ENCODED_DATA structure. </param>
        /// <returns> Frame Id. </returns>
        uint SendUncompressedImage(System.Drawing.Image image, ushort destLeft, ushort destTop,
                                            ushort sId, PixelFormat pixFormat, byte compFlag, uint partSize);
    }
}
