// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    /// <summary>
    /// Creates capability set collect to fill Client Confirm Active PDU
    /// </summary>
    public class RdpbcgrCapSet
    {
        /// <summary>
        /// Creates a collection which contains all mandatory and optional capability sets
        /// </summary>
        /// <returns>collection fo Capability sets.</returns>
        public Collection<ITsCapsSet> CreateCapabilitySets(
            bool supportAutoReconnect,
            bool supportFastPathInput,
            bool supportFastPathOutput,
            bool supportSurfaceCommands,
            bool supportSVCCompression,
            bool supportRemoteFXCodec)
        {
            Collection<ITsCapsSet> capabilitySets = new Collection<ITsCapsSet>();

            #region Mandatory Capability Sets

            #region Populating general Capability Set, set auto-reconnect and fast-path output support
            TS_GENERAL_CAPABILITYSET generalCapabilitySet = new TS_GENERAL_CAPABILITYSET();
            generalCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_GENERAL;
            generalCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(generalCapabilitySet);
            generalCapabilitySet.osMajorType = osMajorType_Values.OSMAJORTYPE_WINDOWS;
            generalCapabilitySet.osMinorType = osMinorType_Values.OSMINORTYPE_WINDOWS_NT;
            generalCapabilitySet.protocolVersion = protocolVersion_Values.V1;
            generalCapabilitySet.pad2octetsA = 0;
            generalCapabilitySet.generalCompressionTypes = generalCompressionTypes_Values.V1;
            generalCapabilitySet.extraFlags = extraFlags_Values.NO_BITMAP_COMPRESSION_HDR
                                            | extraFlags_Values.ENC_SALTED_CHECKSUM
                                            | extraFlags_Values.LONG_CREDENTIALS_SUPPORTED;
            // Add more flags according to parameters.
            if (supportAutoReconnect)
            {
                generalCapabilitySet.extraFlags |= extraFlags_Values.AUTORECONNECT_SUPPORTED;
            }
            if (supportFastPathOutput)
            {
                generalCapabilitySet.extraFlags |= extraFlags_Values.FASTPATH_OUTPUT_SUPPORTED;
            }
            generalCapabilitySet.updateCapabilityFlag = updateCapabilityFlag_Values.V1;
            generalCapabilitySet.remoteUnshareFlag = remoteUnshareFlag_Values.V1;
            generalCapabilitySet.generalCompressionLevel = generalCompressionLevel_Values.V1;
            generalCapabilitySet.refreshRectSupport = refreshRectSupport_Values.TRUE;
            generalCapabilitySet.suppressOutputSupport = suppressOutputSupport_Values.TRUE;
            generalCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(generalCapabilitySet);

            capabilitySets.Add(generalCapabilitySet);
            #endregion  Populating general Capability Set

            #region Populating Bitmap Capability Set
            TS_BITMAP_CAPABILITYSET bitmapCapabilitySet = new TS_BITMAP_CAPABILITYSET();
            bitmapCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_BITMAP;
            bitmapCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(bitmapCapabilitySet);
            bitmapCapabilitySet.preferredBitsPerPixel = RdpConstValue.BITMAP_CAP_BITS_PER_PIXEL_DEFAULT;
            bitmapCapabilitySet.receive1BitPerPixel = RdpConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.receive4BitsPerPixel = RdpConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.receive8BitsPerPixel = RdpConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.desktopWidth = RdpConstValue.DESKTOP_WIDTH_DEFAULT;
            bitmapCapabilitySet.desktopHeight = RdpConstValue.DESKTOP_HEIGHT_DEFAULT;
            bitmapCapabilitySet.pad2octets = 0;
            bitmapCapabilitySet.desktopResizeFlag = desktopResizeFlag_Values.TRUE;
            bitmapCapabilitySet.bitmapCompressionFlag = RdpConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.highColorFlags = 0;
            bitmapCapabilitySet.drawingFlags = drawingFlags_Values.DRAW_ALLOW_COLOR_SUBSAMPLING
                                             | drawingFlags_Values.DRAW_ALLOW_DYNAMIC_COLOR_FIDELITY
                                             | drawingFlags_Values.DRAW_ALLOW_SKIP_ALPHA
                                             | drawingFlags_Values.DRAW_UNUSED_FLAG;
            bitmapCapabilitySet.multipleRectangleSupport = RdpConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.pad2octetsB = 0;

            capabilitySets.Add(bitmapCapabilitySet);
            #endregion Populating Bitmap Capability Set

            #region Populating Order Capability Set
            TS_ORDER_CAPABILITYSET orderCapabilitySet = new TS_ORDER_CAPABILITYSET();
            orderCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_ORDER;
            orderCapabilitySet.terminalDescriptor = new byte[RdpConstValue.ORDER_CAP_TERMINAL_DESCRIPTOR];
            orderCapabilitySet.pad4octetsA = 0;
            orderCapabilitySet.desktopSaveXGranularity = RdpConstValue.ORDER_CAP_DESKTOP_X;
            orderCapabilitySet.desktopSaveYGranularity = RdpConstValue.ORDER_CAP_DESKTOP_Y;
            orderCapabilitySet.pad2octetsA = 0;
            orderCapabilitySet.maximumOrderLevel = RdpConstValue.ORD_LEVEL_1_ORDERS;
            orderCapabilitySet.numberFonts = 0;
            orderCapabilitySet.orderFlags = orderFlags_Values.COLORINDEXSUPPORT
                                          | orderFlags_Values.ZEROBOUNDSDELTASSUPPORT
                                          | orderFlags_Values.NEGOTIATEORDERSUPPORT
                                          | orderFlags_Values.ORDERFLAGS_EXTRA_FLAGS; // If not set ORDERFLAGS_EXTRA_FLAGS flag when EGFX not supported, will get Server Set Error Info PDU with ErrInfoGraphicSubsystemFailed(4399)
            orderCapabilitySet.orderSupport = RdpConstValue.ORDER_CAP_ORDER_SUPPORT_DEFAULT;
            orderCapabilitySet.textFlags = 0;
            orderCapabilitySet.orderSupportExFlags =
                orderSupportExFlags_values.ORDERFLAGS_EX_CACHE_BITMAP_REV3_SUPPORT | orderSupportExFlags_values.ORDERFLAGS_EX_ALTSEC_FRAME_MARKER_SUPPORT;
            orderCapabilitySet.pad4octetsB = 0;
            orderCapabilitySet.desktopSaveSize = RdpConstValue.ORDER_CAP_DESKTOP_SIZE_DEFAULT;
            orderCapabilitySet.pad2octetsC = 0;
            orderCapabilitySet.pad2octetsD = 0;
            orderCapabilitySet.textANSICodePage = 0;
            orderCapabilitySet.pad2octetsE = 0;
            orderCapabilitySet.lengthCapability = (ushort)(sizeof(ushort) * RdpConstValue.ORDER_CAP_USHORT_COUNT
                                                + sizeof(uint) * RdpConstValue.ORDER_CAP_UINT_COUNT
                                                + orderCapabilitySet.terminalDescriptor.Length
                                                + orderCapabilitySet.orderSupport.Length);

            capabilitySets.Add(orderCapabilitySet);
            #endregion Populating Order Capability Set

            #region Populating BitmapCache Capability Set
            TS_BITMAPCACHE_CAPABILITYSET_REV2 bitmapCacheCapabilitySet = new TS_BITMAPCACHE_CAPABILITYSET_REV2();
            bitmapCacheCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_REV2;
            bitmapCacheCapabilitySet.CacheFlags = CacheFlags_Values.ALLOW_CACHE_WAITING_LIST_FLAG
                                                | CacheFlags_Values.PERSISTENT_KEYS_EXPECTED_FLAG;
            bitmapCacheCapabilitySet.pad2 = 0;
            bitmapCacheCapabilitySet.NumCellCaches = RdpConstValue.BITMAP_CACHE_NUM_CELL_DEFAULT;
            bitmapCacheCapabilitySet.BitmapCache1CellInfo.NumEntriesAndK = RdpConstValue.BITMAP_CACHE_CELL1_VALUE;
            bitmapCacheCapabilitySet.BitmapCache2CellInfo.NumEntriesAndK = RdpConstValue.BITMAP_CACHE_CELL2_VALUE;
            bitmapCacheCapabilitySet.BitmapCache3CellInfo.NumEntriesAndK = RdpConstValue.BITMAP_CACHE_CELL3_VALUE;
            bitmapCacheCapabilitySet.BitmapCache4CellInfo.NumEntriesAndK = 0;
            bitmapCacheCapabilitySet.BitmapCache5CellInfo.NumEntriesAndK = 0;
            bitmapCacheCapabilitySet.Pad3 = RdpConstValue.BITMAP_CACHE_PAD3;
            bitmapCacheCapabilitySet.lengthCapability = (ushort)(Marshal.SizeOf(bitmapCacheCapabilitySet)
                                                      + bitmapCacheCapabilitySet.Pad3.Length
                                                      - sizeof(int));

            capabilitySets.Add(bitmapCacheCapabilitySet);
            #endregion Populating BitmapCache Capability Set

            #region Populating Pointer Capability Set
            TS_POINTER_CAPABILITYSET pointerCapabilitySet = new TS_POINTER_CAPABILITYSET();
            pointerCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_POINTER;
            pointerCapabilitySet.colorPointerFlag = colorPointerFlag_Values.TRUE;
            pointerCapabilitySet.colorPointerCacheSize = RdpConstValue.POINTER_CAP_COLOR_SIZE_DEFAULT;
            pointerCapabilitySet.pointerCacheSize = RdpConstValue.POINTER_CAP_POINTER_SIZE_DEFAULT;
            pointerCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(pointerCapabilitySet);

            capabilitySets.Add(pointerCapabilitySet);
            #endregion Populating Pointer Capability Set

            #region Populating Input Capability Set, set fast-path input support
            TS_INPUT_CAPABILITYSET inputCapabilitySet = new TS_INPUT_CAPABILITYSET();
            inputCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_INPUT;
            inputCapabilitySet.inputFlags = inputFlags_Values.INPUT_FLAG_UNICODE
                                          | inputFlags_Values.INPUT_FLAG_MOUSEX
                                          | inputFlags_Values.INPUT_FLAG_SCANCODES;
            if (supportFastPathInput)
            {
                inputCapabilitySet.inputFlags |=
                    (inputFlags_Values.INPUT_FLAG_FASTPATH_INPUT2 | inputFlags_Values.TS_INPUT_FLAG_QOE_TIMESTAMPS);
            }
            inputCapabilitySet.pad2octetsA = 0;
            inputCapabilitySet.keyboardLayout = RdpConstValue.LOCALE_ENGLISH_UNITED_STATES;
            inputCapabilitySet.keyboardType = TS_INPUT_CAPABILITYSET_keyboardType_Values.V4;
            inputCapabilitySet.keyboardSubType = 0;
            inputCapabilitySet.keyboardFunctionKey = RdpConstValue.KEYBOARD_FUNCTION_KEY_NUMBER_DEFAULT;
            inputCapabilitySet.imeFileName = string.Empty;
            inputCapabilitySet.lengthCapability = (ushort)(Marshal.SizeOf(inputCapabilitySet)
                                                - sizeof(int)
                                                + RdpConstValue.INPUT_CAP_IME_FILENAME_SIZE);

            capabilitySets.Add(inputCapabilitySet);
            #endregion Populating Input Capability Set

            #region Populating Brush Capability Set
            TS_BRUSH_CAPABILITYSET brushCapabilitySet = new TS_BRUSH_CAPABILITYSET();
            brushCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_BRUSH;
            brushCapabilitySet.brushSupportLevel = brushSupportLevel_Values.BRUSH_COLOR_8x8;
            brushCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(brushCapabilitySet);

            capabilitySets.Add(brushCapabilitySet);
            #endregion Populating Brush Capability Set

            #region Populating Glyph Cache Capability Set
            TS_GLYPHCACHE_CAPABILITYSET glyphCacheCapabilitySet = new TS_GLYPHCACHE_CAPABILITYSET();
            glyphCacheCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_GLYPHCACHE;
            glyphCacheCapabilitySet.GlyphCache = new TS_CACHE_DEFINITION[RdpConstValue.CLYPH_CACHE_CAP_CLYPH_CACHE_NUM];
            glyphCacheCapabilitySet.GlyphCache[0].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[0].CacheMaximumCellSize = RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_4;
            glyphCacheCapabilitySet.GlyphCache[1].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[1].CacheMaximumCellSize = RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_8;
            glyphCacheCapabilitySet.GlyphCache[2].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[2].CacheMaximumCellSize = RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_8;
            glyphCacheCapabilitySet.GlyphCache[3].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[3].CacheMaximumCellSize = RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_4;
            glyphCacheCapabilitySet.GlyphCache[4].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[4].CacheMaximumCellSize = RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_16;
            glyphCacheCapabilitySet.GlyphCache[5].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[5].CacheMaximumCellSize = RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_32;
            glyphCacheCapabilitySet.GlyphCache[6].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[6].CacheMaximumCellSize = RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_64;
            glyphCacheCapabilitySet.GlyphCache[7].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[7].CacheMaximumCellSize =
                RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_128;
            glyphCacheCapabilitySet.GlyphCache[8].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[8].CacheMaximumCellSize =
                RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_256;
            glyphCacheCapabilitySet.GlyphCache[9].CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_64;
            glyphCacheCapabilitySet.GlyphCache[9].CacheMaximumCellSize = 2048;

            glyphCacheCapabilitySet.FragCache = new TS_CACHE_DEFINITION();
            glyphCacheCapabilitySet.FragCache.CacheEntries = RdpConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_256;
            glyphCacheCapabilitySet.FragCache.CacheMaximumCellSize = RdpConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_256;
            glyphCacheCapabilitySet.GlyphSupportLevel = GlyphSupportLevel_Values.GLYPH_SUPPORT_ENCODE;
            glyphCacheCapabilitySet.pad2octets = 0;
            glyphCacheCapabilitySet.lengthCapability = (ushort)(sizeof(ushort)
                                                     * RdpConstValue.CLYPH_CACHE_CAP_USHORT_COUNT);

            capabilitySets.Add(glyphCacheCapabilitySet);
            #endregion Populating Glyph Cache Capability Set

            #region Populating Offscreen Bitmap Cache Capability Set
            TS_OFFSCREEN_CAPABILITYSET offscreenCapabilitySet = new TS_OFFSCREEN_CAPABILITYSET();
            offscreenCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_OFFSCREENCACHE;
            offscreenCapabilitySet.offscreenSupportLevel = offscreenSupportLevel_Values.TRUE;
            offscreenCapabilitySet.offscreenCacheSize = RdpConstValue.OFFSCREEN_CAP_MAX_CACHE_SIZE;
            offscreenCapabilitySet.offscreenCacheEntries = RdpConstValue.OFFSCREEN_CAP_CACHE_ENTRY_NUM;
            offscreenCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(offscreenCapabilitySet);

            capabilitySets.Add(offscreenCapabilitySet);
            #endregion Populating Offscreen Bitmap Cache Capability Set

            #region Populating Virtual Channel Capability Set, set SVC compression support
            TS_VIRTUALCHANNEL_CAPABILITYSET virtualCapabilitySet = new TS_VIRTUALCHANNEL_CAPABILITYSET();
            virtualCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL;
            if (supportSVCCompression)
            {
                virtualCapabilitySet.flags = TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_COMPR_SC;
            }
            else
            {
                virtualCapabilitySet.flags = TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_NO_COMPR;
            }
            virtualCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(virtualCapabilitySet);
            virtualCapabilitySet.VCChunkSize = 0;

            capabilitySets.Add(virtualCapabilitySet);
            #endregion Populating Virtual Channel Capability Set

            #region Populating Sound Capability Set
            TS_SOUND_CAPABILITYSET soundCapabilitySet = new TS_SOUND_CAPABILITYSET();
            soundCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_SOUND;
            soundCapabilitySet.soundFlags = soundFlags_Values.SOUND_BEEPS_FLAG;
            soundCapabilitySet.pad2octetsA = 0;
            soundCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(soundCapabilitySet);

            capabilitySets.Add(soundCapabilitySet);
            #endregion Populating Sound Capability Set

            #endregion

            #region Optional Capability Sets

            #region Populating Bitmap Cache Host Support Capability Set
            TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET bitmapHostsupprot =
                new TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET();
            bitmapHostsupprot.capabilitySetType = capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_HOSTSUPPORT;
            bitmapHostsupprot.cacheVersion = cacheVersion_Values.V1;
            bitmapHostsupprot.pad1 = 0;
            bitmapHostsupprot.pad2 = 0;
            bitmapHostsupprot.lengthCapability = (ushort)Marshal.SizeOf(bitmapHostsupprot);

            capabilitySets.Add(bitmapHostsupprot);
            #endregion Populating Bitmap Cache Host Support Capability Set

            #region Populating Control Capability Set
            TS_CONTROL_CAPABILITYSET controlCapabilitySet = new TS_CONTROL_CAPABILITYSET();
            controlCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_CONTROL;
            controlCapabilitySet.controlFlags = 0;
            controlCapabilitySet.remoteDetachFlag = 0;
            controlCapabilitySet.controlInterest = RdpConstValue.CONTROLPRIORITY_NEVER;
            controlCapabilitySet.detachInterest = RdpConstValue.CONTROLPRIORITY_NEVER;
            controlCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(controlCapabilitySet);

            capabilitySets.Add(controlCapabilitySet);
            #endregion Populating Control Capability Set

            
            #region Populating Windows Capability Set
            TS_WINDOWACTIVATION_CAPABILITYSET windowsCapabilitySet = new TS_WINDOWACTIVATION_CAPABILITYSET();
            windowsCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_ACTIVATION;
            windowsCapabilitySet.helpKeyFlag = 0;
            windowsCapabilitySet.helpKeyIndexFlag = 0;
            windowsCapabilitySet.helpExtendedKeyFlag = 0;
            windowsCapabilitySet.windowManagerKeyFlag = 0;
            windowsCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(windowsCapabilitySet);

            capabilitySets.Add(windowsCapabilitySet);
            #endregion Populating Windows Capability Set
            

            #region Populating Share Capability Set
            TS_SHARE_CAPABILITYSET shareCapabilitySet = new TS_SHARE_CAPABILITYSET();
            shareCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_SHARE;
            shareCapabilitySet.nodeId = 0;
            shareCapabilitySet.pad2octets = 0;
            shareCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(shareCapabilitySet);

            capabilitySets.Add(shareCapabilitySet);
            #endregion Populating Share Capability Set

            #region Populating Font Capability Set
            TS_FONT_CAPABILITYSET fontCapabilitySet = new TS_FONT_CAPABILITYSET();
            fontCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_FONT;
            fontCapabilitySet.fontSupportFlags = RdpConstValue.FONTSUPPORT_FONTLIST;
            fontCapabilitySet.pad2octets = 0;
            fontCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(fontCapabilitySet);

            capabilitySets.Add(fontCapabilitySet);
            #endregion Populating Font Capability Set

            #region Populating Multifragment Update Capability Set
            TS_MULTIFRAGMENTUPDATE_CAPABILITYSET multiFragmentCapabilitySet =
                new TS_MULTIFRAGMENTUPDATE_CAPABILITYSET();
            multiFragmentCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_MULTIFRAGMENTUPDATE;
            multiFragmentCapabilitySet.MaxRequestSize = RdpConstValue.MULTIFRAGMENT_CAP_MAX_REQUEST_SIZE;
            multiFragmentCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(multiFragmentCapabilitySet);

            capabilitySets.Add(multiFragmentCapabilitySet);
            #endregion Populating Multifragment Update Capability Set

            #region Populating Large Pointer Capability Set
            TS_LARGE_POINTER_CAPABILITYSET largePointerCapabilitySet = new TS_LARGE_POINTER_CAPABILITYSET();
            largePointerCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_LARGE_POINTER;
            largePointerCapabilitySet.largePointerSupportFlags =
                largePointerSupportFlags_Values.LARGE_POINTER_FLAG_96x96;
            largePointerCapabilitySet.lengthCapability = sizeof(ushort) + sizeof(ushort) + sizeof(ushort);

            capabilitySets.Add(largePointerCapabilitySet);
            #endregion Populating Large Pointer Capability Set

            #region Populating Desktop Composition Capability Set
            TS_COMPDESK_CAPABILITYSET desktopCapabilitySet = new TS_COMPDESK_CAPABILITYSET();
            desktopCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_COMPDESK;
            desktopCapabilitySet.CompDeskSupportLevel = CompDeskSupportLevel_Values.COMPDESK_SUPPORTED;
            desktopCapabilitySet.lengthCapability = sizeof(ushort) + sizeof(ushort) + sizeof(ushort);

            capabilitySets.Add(desktopCapabilitySet);
            #endregion Populating Desktop Composition Capability Set

            #region Surface Commands Capability Set, set surface commands support
            TS_SURFCMDS_CAPABILITYSET surfCmdsCapSet = new TS_SURFCMDS_CAPABILITYSET();
            surfCmdsCapSet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_SURFACE_COMMANDS;
            if (supportSurfaceCommands)
            {
                surfCmdsCapSet.cmdFlags = CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS;
            }
            else
            {
                surfCmdsCapSet.cmdFlags = CmdFlags_Values.None;
            }
            surfCmdsCapSet.lengthCapability = sizeof(ushort) + sizeof(ushort) + sizeof(uint) + sizeof(uint);

            capabilitySets.Add(surfCmdsCapSet);
            #endregion

            #region Bitmap Codecs Capability Set
            TS_BITMAPCODECS_CAPABILITYSET codecsCapSet = new TS_BITMAPCODECS_CAPABILITYSET();
            codecsCapSet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS;
            codecsCapSet.supportedBitmapCodecs = new TS_BITMAPCODECS();
            if (supportRemoteFXCodec)
            {
                codecsCapSet.supportedBitmapCodecs.bitmapCodecCount = 3;
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray = new TS_BITMAPCODEC[3];
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray[0] = this.CreateTS_BITMAPCODEC_NSCodec();
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray[1] = this.CreateTS_BITMAPCODEC_RemoteFX();
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray[2] = this.CreateTS_BITMAPCODEC_Image_RemoteFX();
            }
            else
            {
                codecsCapSet.supportedBitmapCodecs.bitmapCodecCount = 1;
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray = new TS_BITMAPCODEC[1];
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray[0] = this.CreateTS_BITMAPCODEC_NSCodec();
            }
            codecsCapSet.lengthCapability = (ushort)(sizeof(ushort) + sizeof(ushort) + sizeof(byte));
            foreach (TS_BITMAPCODEC codec in codecsCapSet.supportedBitmapCodecs.bitmapCodecArray)
            {
                codecsCapSet.lengthCapability += (ushort)(19 + codec.codecPropertiesLength);
            }

            capabilitySets.Add(codecsCapSet);
            #endregion

            #region TS_FRAME_ACKNOWLEDGE_CAPABILITYSET
            TS_FRAME_ACKNOWLEDGE_CAPABILITYSET frameAckCapSet = new TS_FRAME_ACKNOWLEDGE_CAPABILITYSET();
            frameAckCapSet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_FRAME_ACKNOWLEDGE;
            frameAckCapSet.lengthCapability = 8;
            frameAckCapSet.maxUnacknowledgedFrameCount = 2;

            capabilitySets.Add(frameAckCapSet);
            #endregion

            #endregion

            return capabilitySets;
        }

        #region private methods
        /// <summary>
        /// Creates Bitmap Codec structure which contains a NSCodec Capability Set
        /// </summary>
        /// <returns></returns>
        private TS_BITMAPCODEC CreateTS_BITMAPCODEC_NSCodec()
        {
            TS_BITMAPCODEC bitmapCodec = new TS_BITMAPCODEC();
            bitmapCodec.codecGUID.codecGUID1 = 0xCA8D1BB9;
            bitmapCodec.codecGUID.codecGUID2 = 0x000F;
            bitmapCodec.codecGUID.codecGUID3 = 0x154F;
            bitmapCodec.codecGUID.codecGUID4 = 0x58;
            bitmapCodec.codecGUID.codecGUID5 = 0x9F;
            bitmapCodec.codecGUID.codecGUID6 = 0xAE;
            bitmapCodec.codecGUID.codecGUID7 = 0x2D;
            bitmapCodec.codecGUID.codecGUID8 = 0x1A;
            bitmapCodec.codecGUID.codecGUID9 = 0x87;
            bitmapCodec.codecGUID.codecGUID10 = 0xE2;
            bitmapCodec.codecGUID.codecGUID11 = 0xD6;
            bitmapCodec.codecID = 1;
            bitmapCodec.codecPropertiesLength = 3;
            bitmapCodec.codecProperties = new byte[3];
            bitmapCodec.codecProperties[0] = 0x01; //fAllowDynamicFidelity 
            bitmapCodec.codecProperties[1] = 0x01; //fAllowSubsampling
            bitmapCodec.codecProperties[2] = 0x03; //colorLossLevel 
            return bitmapCodec;
        }

        /// <summary>
        /// Create Bitmap Codec structure which contains a TS_RFX_CLNT_CAPS_CONTAINER structure
        /// </summary>
        /// <returns></returns>
        private TS_BITMAPCODEC CreateTS_BITMAPCODEC_RemoteFX()
        {
            TS_RFX_ICAP rfxIcapRLGR1 = new TS_RFX_ICAP();
            rfxIcapRLGR1.version = version_Value.CLW_VERSION_1_0;
            rfxIcapRLGR1.flags = 0x00;
            rfxIcapRLGR1.entropyBits = entropyBits_Value.CLW_ENTROPY_RLGR1;

            TS_RFX_ICAP rfxIcapRLGR3 = new TS_RFX_ICAP();
            rfxIcapRLGR3.version = version_Value.CLW_VERSION_1_0;
            rfxIcapRLGR3.flags = 0x00;
            rfxIcapRLGR3.entropyBits = entropyBits_Value.CLW_ENTROPY_RLGR3;

            TS_RFX_CAPSET rfxCapSet = new TS_RFX_CAPSET();
            rfxCapSet.numIcaps = 2;
            rfxCapSet.icapsData = new TS_RFX_ICAP[] { rfxIcapRLGR1, rfxIcapRLGR3 };

            TS_RFX_CAPS rfxCaps = new TS_RFX_CAPS();
            rfxCaps.capsetsData = new TS_RFX_CAPSET[] { rfxCapSet };

            TS_RFX_CLNT_CAPS_CONTAINER rfxClnCaps = new TS_RFX_CLNT_CAPS_CONTAINER();
            rfxClnCaps.captureFlags = 0x00000001;
            rfxClnCaps.capsData = rfxCaps;


            TS_BITMAPCODEC bitmapCodec = new TS_BITMAPCODEC();
            bitmapCodec.codecGUID.codecGUID1 = 0x76772F12;
            bitmapCodec.codecGUID.codecGUID2 = 0xBD72;
            bitmapCodec.codecGUID.codecGUID3 = 0x4463;
            bitmapCodec.codecGUID.codecGUID4 = 0xAF;
            bitmapCodec.codecGUID.codecGUID5 = 0xB3;
            bitmapCodec.codecGUID.codecGUID6 = 0xB7;
            bitmapCodec.codecGUID.codecGUID7 = 0x3C;
            bitmapCodec.codecGUID.codecGUID8 = 0x9C;
            bitmapCodec.codecGUID.codecGUID9 = 0x6F;
            bitmapCodec.codecGUID.codecGUID10 = 0x78;
            bitmapCodec.codecGUID.codecGUID11 = 0x86;
            bitmapCodec.codecID = 2;
            bitmapCodec.codecProperties = rfxClnCaps.ToBytes();
            bitmapCodec.codecPropertiesLength = (ushort)bitmapCodec.codecProperties.Length;
            return bitmapCodec;
        }

        /// <summary>
        /// Create Bitmap Codec structure which contains a TS_RFX_CLNT_CAPS_CONTAINER structure
        /// </summary>
        /// <returns></returns>
        private TS_BITMAPCODEC CreateTS_BITMAPCODEC_Image_RemoteFX()
        {
            TS_RFX_ICAP rfxIcapRLGR1 = new TS_RFX_ICAP();
            rfxIcapRLGR1.version = version_Value.CLW_VERSION_1_0;
            rfxIcapRLGR1.flags = 0x02;
            rfxIcapRLGR1.entropyBits = entropyBits_Value.CLW_ENTROPY_RLGR1;

            TS_RFX_ICAP rfxIcapRLGR3 = new TS_RFX_ICAP();
            rfxIcapRLGR3.version = version_Value.CLW_VERSION_1_0;
            rfxIcapRLGR3.flags = 0x02;
            rfxIcapRLGR3.entropyBits = entropyBits_Value.CLW_ENTROPY_RLGR3;

            TS_RFX_CAPSET rfxCapSet = new TS_RFX_CAPSET();
            rfxCapSet.numIcaps = 2;
            rfxCapSet.icapsData = new TS_RFX_ICAP[] { rfxIcapRLGR1, rfxIcapRLGR3 };

            TS_RFX_CAPS rfxCaps = new TS_RFX_CAPS();
            rfxCaps.capsetsData = new TS_RFX_CAPSET[] { rfxCapSet };

            TS_RFX_CLNT_CAPS_CONTAINER rfxClnCaps = new TS_RFX_CLNT_CAPS_CONTAINER();
            rfxClnCaps.captureFlags = 0x00000001;
            rfxClnCaps.capsData = rfxCaps;


            TS_BITMAPCODEC bitmapCodec = new TS_BITMAPCODEC();
            bitmapCodec.codecGUID.codecGUID1 = 0x2744CCD4;
            bitmapCodec.codecGUID.codecGUID2 = 0x9D8A;
            bitmapCodec.codecGUID.codecGUID3 = 0x4E74;
            bitmapCodec.codecGUID.codecGUID4 = 0x80;
            bitmapCodec.codecGUID.codecGUID5 = 0x3C;
            bitmapCodec.codecGUID.codecGUID6 = 0x0E;
            bitmapCodec.codecGUID.codecGUID7 = 0xCB;
            bitmapCodec.codecGUID.codecGUID8 = 0xEE;
            bitmapCodec.codecGUID.codecGUID9 = 0xA1;
            bitmapCodec.codecGUID.codecGUID10 = 0x9C;
            bitmapCodec.codecGUID.codecGUID11 = 0x54;
            bitmapCodec.codecID = 3;
            bitmapCodec.codecProperties = rfxClnCaps.ToBytes();
            bitmapCodec.codecPropertiesLength = (ushort)bitmapCodec.codecProperties.Length;
            return bitmapCodec;
        }
        #endregion
    }
}
