// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The value of this integer indicates the type of message following the header. 
    /// </summary>
    public enum PacketTypeValues : ushort
    {
        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_WIRE_TO_SURFACE_PDU_1 message.
        /// </summary>
        RDPGFX_CMDID_WIRETOSURFACE_1 = 0x0001,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_WIRE_TO_SURFACE_PDU_2 message.
        /// </summary>
        RDPGFX_CMDID_WIRETOSURFACE_2 = 0x0002,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_DELETE_ENCODING_CONTEXT_PDU message.
        /// </summary>
        RDPGFX_CMDID_DELETEENCODINGCONTEXT = 0x0003,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_SOLIDFILL_PDU message.
        /// </summary>
        RDPGFX_CMDID_SOLIDFILL = 0x0004,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_SURFACE_TO_SURFACE_PDU message.
        /// </summary>
        RDPGFX_CMDID_SURFACETOSURFACE = 0x0005,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_SURFACE_TO_CACHE_PDU message.
        /// </summary>
        RDPGFX_CMDID_SURFACETOCACHE = 0x0006,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_CACHE_TO_SURFACE_PDU  message.
        /// </summary>
        RDPGFX_CMDID_CACHETOSURFACE = 0x0007,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_EVICT_CACHE_ENTRY_PDU message.
        /// </summary>
        RDPGFX_CMDID_EVICTCACHEENTRY = 0x0008,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_CREATE_SURFACE_PDU message.
        /// </summary>
        RDPGFX_CMDID_CREATESURFACE = 0x0009,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_DELETE_SURFACE_PDU message.
        /// </summary>
        RDPGFX_CMDID_DELETESURFACE = 0x000a,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_START_FRAME_PDU message.
        /// </summary>
        RDPGFX_CMDID_STARTFRAME = 0x000b,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_END_FRAME_PDU message.
        /// </summary>
        RDPGFX_CMDID_ENDFRAME = 0x000c,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_FRAME_ACKNOWLEDGE_PDU message.
        /// </summary>
        RDPGFX_CMDID_FRAMEACKNOWLEDGE = 0x000d,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_RESET_GRAPHICS message.
        /// </summary>
        RDPGFX_CMDID_RESETGRAPHICS = 0x000e,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_MAP_SURFACE_TO_OUTPUT_PDU message.
        /// </summary>
        RDPGFX_CMDID_MAPSURFACETOOUTPUT = 0x000f,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_CACHE_IMPORT_OFFER_PDU message.
        /// </summary>
        RDPGFX_CMDID_CACHEIMPORTOFFER = 0x0010,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_CACHE_IMPORT_REPLY_PDU message.
        /// </summary>
        RDPGFX_CMDID_CACHEIMPORTREPLY = 0x0011,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_CAPS_ADVERTISE_PDU message.
        /// </summary>
        RDPGFX_CMDID_CAPSADVERTISE = 0x0012,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDP_CAPS_CONFIRM_PDU message.
        /// </summary>
        RDPGFX_CMDID_CAPSCONFIRM = 0x0013,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_MAP_SURFACE_TO_WINDOW_PDU message
        /// </summary>
        RDPGFX_CMDID_MAPSURFACETOWINDOW = 0x0015,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_QOE_FRAME_ACKNOWLEDGE_PDU message
        /// </summary>
        RDPGFX_CMDID_QOEFRAMEACKNOWLEDGE = 0x0016,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU message
        /// </summary>
        RDPGFX_CMDID_MAPSURFACETOSCALEDOUTPUT = 0x0017,

        /// <summary>
        /// Indicates that this message should be interpreted as a RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU message
        /// </summary>
        RDPGFX_CMDID_MAPSURFACETOSCALEDWINDOW = 0x0018,

    }

    /// <summary>
    /// Specifies the version of RDPEGFX implementation.
    /// </summary>
    public enum CapsVersions : uint
    {
        /// <summary>
        /// specifies the version of the capability in RDP8.0.
        /// </summary>
        RDPGFX_CAPVERSION_8 = 0x00080004,

        /// <summary>
        /// Specifies the version of the capability, which is supported in RDP 8.1
        /// </summary>
        RDPGFX_CAPVERSION_81 = 0x00080105,

        /// <summary>
        /// Specifies the version of the capability, which is supported in RDP 10
        /// </summary>
        RDPGFX_CAPVERSION_10 = 0x000A0002,

        /// <summary>
        /// Specifies the version of the capability, which is supported in RDP 10.1
        /// </summary>
        RDPGFX_CAPVERSION_101 = 0x000A0100,

        /// <summary>
        /// Specifies the version of the capability, which is supported in RDP 10.2
        /// </summary>
        RDPGFX_CAPVERSION_102 = 0x000A0200,

        /// <summary>
        /// Specifies the version of the capability, which is supported in RDP 10.3
        /// </summary>
        RDPGFX_CAPVERSION_103 = 0x000A0301,

        /// <summary>
        /// Specifies the version of the capability, which is supported in RDP 10.4
        /// </summary>
        RDPGFX_CAPVERSION_104 = 0x000A0400,

        /// <summary>
        /// Specifies the version of the capability, which is supported in RDP 10.5
        /// </summary>
        RDPGFX_CAPVERSION_105 = 0x000A0502,

        /// <summary>
        /// Specifies the version of the capability, which is supported in RDP 10.6
        /// </summary>
        RDPGFX_CAPVERSION_106 = 0x000A0600
    }

    public enum MaxCacheSlotNumber : int
    {
        Small_Cache_Size = 12800,
        Thin_Client_Cache_Size = 4096,
        Default_Cache_Size = 100,//If neither the RDPGFX_CAPS_FLAG_THINCLIENT nor the RDPGFX_CAPS_FLAG_SMALL_CACHE capability flag is specified, the bitmap cache size is assumed to be 100 MB in size
        FLAG_THINCLIENT_Cache_Size = 16, // The bitmap cache MUST be constrained to 16 MB in size (if it is used) 
        FLAG_SMALL_CACHE_Cache_Size = 16, //Indicates that the bitmap cache MUST be constrained to 16 MB in size (if it is used).

    }
    /// <summary>
    /// Flags of capability.
    /// </summary>
    public enum CapsFlags : uint
    {
        /// <summary>
        /// Indicates that the bitmap cache size is assumed to be 100 MB in size, if it is used.
        /// </summary>
        RDPGFX_CAPS_FLAG_DEFAULT = 0x0,
        /// <summary>
        /// Indicates that the bitmap cache MUST be constrained to 16 MB in size (if it is used) and 
        /// that the RemoteFX Codec ([MS-RDPRFX] section 1 to 3) MUST be used in place of the RemoteFX Progressive Codec 
        /// </summary>
        RDPGFX_CAPS_FLAG_THINCLIENT = 0x00000001,
        /// <summary>
        /// Indicates that the bitmap cache MUST be constrained to 50 MB in size (if it is used). 
        /// </summary>
        RDPGFX_CAPS_FLAG_SMALL_CACHE = 0x00000002,

        /// <summary>
        /// Indicates that the usage of the MPEG-4 AVC/H.264 Codec in YUV420p mode 
        /// is supported in the RDPGFX_WIRE_TO_SURFACE_PDU_1 (section 2.2.2.1) message..
        /// </summary>
        RDPGFX_CAPS_FLAG_AVC420_ENABLED = 0x00000010,

        /// <summary>
        /// Indicates that usage of the MPEG-4 AVC/H.264 Codec in either YUV420p or YUV444 modes 
        /// is not supported in the RDPGFX_WIRE_TO_SURFACE_PDU_1 (section 2.2.2.1) message.
        /// </summary>
        RDPGFX_CAPS_FLAG_AVC_DISABLED = 0x00000020,

        /// <summary>
        /// Indicates that the client prefers the MPEG-4 AVC/H.264 Codec in YUV444 mode. 
        /// If this flag is set, the RDPGFX_CAPS_FLAG_AVC_DISABLED flag MUST NOT be set.
        /// </summary>
        RDPGFX_CAPS_FLAG_AVC_THINCLIENT = 0x00000040

    }

    /// <summary>
    /// Pixel format of bitmap data.
    /// </summary>
    public enum PixelFormat : byte
    {
        /// <summary>
        /// Indicates 32bpp with no valid alpha (XRGB)
        /// </summary>
        PIXEL_FORMAT_XRGB_8888 = 0x20,
        /// <summary>
        /// Indicates 32bpp with valid alpha (ARGB).
        /// </summary>
        PIXEL_FORMAT_ARGB_8888 = 0x21

    }

    /// <summary>
    /// Type of bitmap codec.
    /// </summary>
    public enum CodecType : ushort
    {
        /// <summary>
        /// Indicates the bitmap data encapsulated in the bitmapData field is uncompressed.
        /// </summary>
        RDPGFX_CODECID_UNCOMPRESSED = 0x0000,
        /// <summary>
        /// Indicates the bitmap data encapsulated in the bitmapData field is compressed using the NSCodec Codec 
        /// </summary>
        RDPGFX_CODECID_NSCODEC = 0x0001,
        /// <summary>
        /// Indicates the bitmap data encapsulated in the bitmapData field is compressed using the RemoteFX Codec 
        /// </summary>
        RDPGFX_CODECID_CAVIDEO = 0x0003,
        /// <summary>
        /// Indicates the bitmap data encapsulated in the bitmapData field is compressed using the ClearCodec Codec 
        /// </summary>
        RDPGFX_CODECID_CLEARCODEC = 0x0008,
        /// <summary>
        /// Indicates the bitmap data encapsulated in the bitmapData field is compressed using the RemoteFX Progressive Codec  
        /// </summary>
        RDPGFX_CODECID_CAPROGRESSIVE = 0x0009,
        /// <summary>
        /// Indicates the bitmap data encapsulated in the bitmapData field is compressed using the Planar  Codec 
        /// </summary>
        RDPGFX_CODECID_PLANAR = 0x000A,
        /// <summary>
        /// The bitmap data encapsulated in the bitmapData field is compressed using the MPEG-4 AVC/H.264 Codec in YUV420p mode.  
        /// </summary>
        RDPGFX_CODECID_AVC420 = 0x000B,
        /// <summary>
        /// The bitmap data encapsulated in the bitmapData field is compressed using the Alpha Codec 
        /// </summary>
        RDPGFX_CODECID_ALPHA = 0x000C,
        /// <summary>
        /// The bitmap data encapsulated in the bitmapData field is compressed using the MPEG-4 AVC/H.264 Codec in YUV444 mode.
        /// </summary>
        RDPGFX_CODECID_AVC444 = 0x000E,
        /// <summary>
        /// The bitmap data encapsulated in the bitmapData field is compressed using the MPEG-4 AVC/H.264 Codec in YUV444v2 mode.
        /// </summary>
        RDPGFX_CODECID_AVC444v2 = 0x000F
    }

    /// <summary>
    /// The type of RFX progress codec block.
    /// </summary>
    public enum RFXProgCodecBlockType : ushort
    {
        /// <summary>
        /// Indicates sync block
        /// </summary>
        WBT_SYNC = 0xCCC0,
        /// <summary>
        /// Indicates frame begin block
        /// </summary>
        WBT_FRAME_BEGIN = 0xCCC1,
        /// <summary>
        /// Indicates frame end block
        /// </summary>
        WBT_FRAME_END = 0xCCC2,
        /// <summary>
        /// Indicates context block
        /// </summary>
        WBT_CONTEXT = 0xCCC3,
        /// <summary>
        /// Indicates region block
        /// </summary>
        WBT_REGION = 0xCCC4,
        /// <summary>
        /// Indicates tile simple block, used for non-progressive encoding
        /// </summary>
        WBT_TILE_SIMPLE = 0xCCC5,
        /// <summary>
        /// Indicates tile first block, used for progressive encoding
        /// </summary>
        WBT_TILE_PROGRESSIVE_FIRST = 0xCCC6,
        /// <summary>
        /// Indicates tile upgrade block, used for progressive encoding
        /// </summary>
        WBT_TILE_PROGRESSIVE_UPGRADE = 0xCCC7
    }
}
