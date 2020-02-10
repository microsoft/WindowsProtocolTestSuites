// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Security.Authentication;

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;
using Microsoft.Protocols.TestTools.StackSdk.Security;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// RDPBCGR client, receives BCGR server PDUs and sends client PDUs. 
    /// Meanwhile acts as a base protocol for upper layer protocol, such as MS-RA, MS-RDPEGDI, and etc.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
        "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1804:RemoveUnusedLocals")]
    public class RdpbcgrClient : IDisposable
    {
        #region member
        private bool disposed;
        private string serverDomain;
        private string serverName;
        private string logonName;
        private string logonPassword;
        private string clientIPAddress;
        private int serverPort;
        private int transportBufferSize;
        private StreamConfig transportConfig;
        private bool isAutoReactivate;
        protected const ushort TS_UD_CS_SEC_SecurityDataSize = 12;
        private const int SOCKET_RECEIVE_TIMEOUT = 1;
        private SslProtocols tlsVersion;

        /// <summary>
        /// A TCP transport instance, sending and receiving all the PDUs.
        /// </summary>
#if UT
        private TransportStackMock transportStack;
#else
        private RdpbcgrClientTransportStack transportStack;
#endif

        private TcpClient tcpClient;

        /// <summary>
        /// The stream to do TCP transport.
        /// </summary>
        private Stream clientStream;

        /// <summary>
        /// A class to decode received PDUs.
        /// </summary>
        private RdpbcgrDecoder decoder;

        /// <summary>
        /// The selected security protocol.
        /// </summary>
        private EncryptedProtocol encryptedProtocol;

        /// <summary>
        /// A context contains all major PDUs information.
        /// </summary>
        public RdpbcgrClientContext context;

        /// <summary>
        /// This member indicates UpdateSessionKey has completed.
        /// </summary>
        private ManualResetEvent updateSessionKeyEvent;

        /// <summary>
        /// Indicating whether client disconnects with SUT or not.
        /// </summary>
        private bool disconnected;

        /// <summary>
        /// A context contains all needed information.
        /// </summary>
        public RdpbcgrClientContext Context
        {
            get
            {
                return context;
            }
        }

        public bool Connected
        {
            get
            {
                return this.tcpClient.Connected;
            }
        }

        /// <summary>
        /// To detect whether there are packets cached in the queue of TransportStack.
        /// </summary>
        public bool IsDataAvailable
        {
            get
            {
                return transportStack.IsDataAvailable;
            }
        }

        /// <summary>
        /// The buffer size of transport stack. The default value is 20480.
        /// </summary>
        public int TransportStackBufferSize
        {
            get
            {
                return transportBufferSize;
            }
            set
            {
                transportBufferSize = value;
            }
        }

        /// <summary>
        /// If do reactivate automatically when receiving Server_Deactivate_All_PDU
        /// </summary>
        public bool IsAutoReactivation
        {
            get
            {
                return isAutoReactivate;
            }
            set
            {
                isAutoReactivate = value;
            }
        }

        public SslProtocols TlsVersion
        {
            get
            {
                return tlsVersion;
            }
            set
            {
                tlsVersion = value;
            }
        }

        /// <summary>
        /// Get all virtual channels' names and ids that have been allocated.
        /// This method can be called after receiving MCS Connect Response PDU with GCC Conference Create Response.
        /// </summary>
        /// <returns>All pairs of virtual channel Id and name.</returns>
        /// <exception> Throw FormatException if the virtual channel ids and names are not matched.</exception>
        public Dictionary<ushort, string> GetVirtualChannels()
        {
            ushort[] virtualChannelIds = context.VirtualChannelIdStore;
            CHANNEL_DEF[] virtualChannelDefines = context.VirtualChannelDefines;

            if (virtualChannelIds == null && virtualChannelDefines == null)
            {
                return null;
            }

            Dictionary<ushort, string> virtualChannels = new Dictionary<ushort, string>();

            if (virtualChannelIds != null && virtualChannelDefines != null
                && virtualChannelIds.Length == virtualChannelDefines.Length)
            {
                for (int i = 0; i < virtualChannelIds.Length; ++i)
                {
                    virtualChannels.Add(virtualChannelIds[i], virtualChannelDefines[i].name);
                }
            }
            else
            {
                throw new FormatException("The virtual channel ids and names are not matched.");
            }

            return virtualChannels;
        }
        #endregion member


        #region constructor
        /// <summary>
        /// Create a RDPBCGR client.
        /// </summary>
        /// <param name="domain">The domain of the server.</param>
        /// <param name="server">The computer name of the server.</param>
        /// <param name="userName">The user name to logon the server.</param>
        /// <param name="password">The password of the userName to logon the server.</param>
        /// <param name="clientIP">The IP address of the client computer.</param>
        /// <param name="serverPort">The port to logon the server.</param>
        public RdpbcgrClient(string domain,
                             string server,
                             string userName,
                             string password,
                             string clientIP,
                             int serverPort)
        {
            serverDomain = domain;
            serverName = server;
            logonName = userName;
            logonPassword = password;
            clientIPAddress = clientIP;
            this.serverPort = serverPort;
            transportBufferSize = ConstValue.TRANSPORT_BUFFER_SIZE;
            context = new RdpbcgrClientContext(this);
            decoder = new RdpbcgrDecoder(this, context);
            updateSessionKeyEvent = new ManualResetEvent(false);
        }
        #endregion constructor


        #region help methods
        /// <summary>
        /// Create default capability sets.
        /// User can add, remove or update the capability sets with the return value.
        /// </summary>
        /// <returns>The capability sets created.</returns>
        public Collection<ITsCapsSet> CreateCapabilitySets(
            bool supportAutoReconnect = true,
            bool supportFastPathInput = false,
            bool supportFastPathOutput = false,
            bool supportSVCCompression = false)
        {
            Collection<ITsCapsSet> capabilitySets = new Collection<ITsCapsSet>();

            #region fill capabilities
            #region Populating general Capability Set
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
            bitmapCapabilitySet.preferredBitsPerPixel = ConstValue.BITMAP_CAP_BITS_PER_PIXEL_DEFAULT;
            bitmapCapabilitySet.receive1BitPerPixel = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.receive4BitsPerPixel = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.receive8BitsPerPixel = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.desktopWidth = ConstValue.DESKTOP_WIDTH_DEFAULT;
            bitmapCapabilitySet.desktopHeight = ConstValue.DESKTOP_HEIGHT_DEFAULT;
            bitmapCapabilitySet.pad2octets = 0;
            bitmapCapabilitySet.desktopResizeFlag = desktopResizeFlag_Values.TRUE;
            bitmapCapabilitySet.bitmapCompressionFlag = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.highColorFlags = 0;
            bitmapCapabilitySet.drawingFlags = drawingFlags_Values.DRAW_ALLOW_COLOR_SUBSAMPLING
                                             | drawingFlags_Values.DRAW_ALLOW_DYNAMIC_COLOR_FIDELITY
                                             | drawingFlags_Values.DRAW_ALLOW_SKIP_ALPHA;
            bitmapCapabilitySet.multipleRectangleSupport = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.pad2octetsB = 0;

            capabilitySets.Add(bitmapCapabilitySet);
            #endregion Populating Bitmap Capability Set

            #region Populating Order Capability Set
            TS_ORDER_CAPABILITYSET orderCapabilitySet = new TS_ORDER_CAPABILITYSET();
            orderCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_ORDER;
            orderCapabilitySet.terminalDescriptor = new byte[ConstValue.ORDER_CAP_TERMINAL_DESCRIPTOR];
            orderCapabilitySet.pad4octetsA = 0;
            orderCapabilitySet.desktopSaveXGranularity = ConstValue.ORDER_CAP_DESKTOP_X;
            orderCapabilitySet.desktopSaveYGranularity = ConstValue.ORDER_CAP_DESKTOP_Y;
            orderCapabilitySet.pad2octetsA = 0;
            orderCapabilitySet.maximumOrderLevel = ConstValue.ORD_LEVEL_1_ORDERS;
            orderCapabilitySet.numberFonts = 0;
            orderCapabilitySet.orderFlags = orderFlags_Values.COLORINDEXSUPPORT
                                          | orderFlags_Values.ZEROBOUNDSDELTASSUPPORT
                                          | orderFlags_Values.NEGOTIATEORDERSUPPORT;
            orderCapabilitySet.orderSupport = ConstValue.ORDER_CAP_ORDER_SUPPORT_DEFAULT;
            orderCapabilitySet.textFlags = 0;
            orderCapabilitySet.orderSupportExFlags =
                orderSupportExFlags_values.ORDERFLAGS_EX_CACHE_BITMAP_REV3_SUPPORT;
            orderCapabilitySet.pad4octetsB = 0;
            orderCapabilitySet.desktopSaveSize = ConstValue.ORDER_CAP_DESKTOP_SIZE_DEFAULT;
            orderCapabilitySet.pad2octetsC = 0;
            orderCapabilitySet.pad2octetsD = 0;
            orderCapabilitySet.textANSICodePage = 0;
            orderCapabilitySet.pad2octetsE = 0;
            orderCapabilitySet.lengthCapability = (ushort)(sizeof(ushort) * ConstValue.ORDER_CAP_USHORT_COUNT
                                                + sizeof(uint) * ConstValue.ORDER_CAP_UINT_COUNT
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
            bitmapCacheCapabilitySet.NumCellCaches = ConstValue.BITMAP_CACHE_NUM_CELL_DEFAULT;
            bitmapCacheCapabilitySet.BitmapCache1CellInfo.NumEntriesAndK = ConstValue.BITMAP_CACHE_CELL1_VALUE;
            bitmapCacheCapabilitySet.BitmapCache2CellInfo.NumEntriesAndK = ConstValue.BITMAP_CACHE_CELL2_VALUE;
            bitmapCacheCapabilitySet.BitmapCache3CellInfo.NumEntriesAndK = ConstValue.BITMAP_CACHE_CELL3_VALUE;
            bitmapCacheCapabilitySet.BitmapCache4CellInfo.NumEntriesAndK = 0;
            bitmapCacheCapabilitySet.BitmapCache5CellInfo.NumEntriesAndK = 0;
            bitmapCacheCapabilitySet.Pad3 = ConstValue.BITMAP_CACHE_PAD3;
            bitmapCacheCapabilitySet.lengthCapability = (ushort)(TypeMarshal.ToBytes(bitmapCacheCapabilitySet).Length);
            capabilitySets.Add(bitmapCacheCapabilitySet);
            #endregion Populating BitmapCache Capability Set

            #region Populating Pointer Capability Set
            TS_POINTER_CAPABILITYSET pointerCapabilitySet = new TS_POINTER_CAPABILITYSET();
            pointerCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_POINTER;
            pointerCapabilitySet.colorPointerFlag = colorPointerFlag_Values.TRUE;
            pointerCapabilitySet.colorPointerCacheSize = ConstValue.POINTER_CAP_COLOR_SIZE_DEFAULT;
            pointerCapabilitySet.pointerCacheSize = ConstValue.POINTER_CAP_POINTER_SIZE_DEFAULT;
            pointerCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(pointerCapabilitySet);

            capabilitySets.Add(pointerCapabilitySet);
            #endregion Populating Pointer Capability Set

            #region Populating Input Capability Set
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
            inputCapabilitySet.keyboardLayout = ConstValue.LOCALE_ENGLISH_UNITED_STATES;
            inputCapabilitySet.keyboardType = TS_INPUT_CAPABILITYSET_keyboardType_Values.V4;
            inputCapabilitySet.keyboardSubType = 0;
            inputCapabilitySet.keyboardFunctionKey = ConstValue.KEYBOARD_FUNCTION_KEY_NUMBER_DEFAULT;
            inputCapabilitySet.imeFileName = string.Empty;
            inputCapabilitySet.lengthCapability = (ushort)(
                                                    TypeMarshal.ToBytes(inputCapabilitySet).Length -
                                                    2 * (inputCapabilitySet.imeFileName.Length + 1) + // length of (inputCapabilitySet.imeFileName + null terminator) in bytes[]
                                                    ConstValue.INPUT_CAP_IME_FILENAME_SIZE);
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
            glyphCacheCapabilitySet.GlyphCache = new TS_CACHE_DEFINITION[ConstValue.CLYPH_CACHE_CAP_CLYPH_CACHE_NUM];
            glyphCacheCapabilitySet.GlyphCache[0].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[0].CacheMaximumCellSize = ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_4;
            glyphCacheCapabilitySet.GlyphCache[1].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[1].CacheMaximumCellSize = ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_4;
            glyphCacheCapabilitySet.GlyphCache[2].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[2].CacheMaximumCellSize = ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_8;
            glyphCacheCapabilitySet.GlyphCache[3].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[3].CacheMaximumCellSize = ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_4;
            glyphCacheCapabilitySet.GlyphCache[4].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[4].CacheMaximumCellSize = ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_16;
            glyphCacheCapabilitySet.GlyphCache[5].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[5].CacheMaximumCellSize = ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_32;
            glyphCacheCapabilitySet.GlyphCache[6].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[6].CacheMaximumCellSize = ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_64;
            glyphCacheCapabilitySet.GlyphCache[7].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[7].CacheMaximumCellSize =
                ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_128;
            glyphCacheCapabilitySet.GlyphCache[8].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254;
            glyphCacheCapabilitySet.GlyphCache[8].CacheMaximumCellSize =
                ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_256;
            glyphCacheCapabilitySet.GlyphCache[9].CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_64;
            glyphCacheCapabilitySet.GlyphCache[9].CacheMaximumCellSize =
                ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_256;

            glyphCacheCapabilitySet.FragCache = new TS_CACHE_DEFINITION();
            glyphCacheCapabilitySet.FragCache.CacheEntries = ConstValue.CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_256;
            glyphCacheCapabilitySet.FragCache.CacheMaximumCellSize = ConstValue.CLYPH_CACHE_CAP_CACHE_CELL_SIZE_256;
            glyphCacheCapabilitySet.GlyphSupportLevel = GlyphSupportLevel_Values.GLYPH_SUPPORT_ENCODE;
            glyphCacheCapabilitySet.pad2octets = 0;
            glyphCacheCapabilitySet.lengthCapability = (ushort)(sizeof(ushort)
                                                     * ConstValue.CLYPH_CACHE_CAP_USHORT_COUNT);

            capabilitySets.Add(glyphCacheCapabilitySet);
            #endregion Populating Glyph Cache Capability Set

            #region Populating Offscreen Bitmap Cache Capability Set
            TS_OFFSCREEN_CAPABILITYSET offscreenCapabilitySet = new TS_OFFSCREEN_CAPABILITYSET();
            offscreenCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_OFFSCREENCACHE;
            offscreenCapabilitySet.offscreenSupportLevel = offscreenSupportLevel_Values.TRUE;
            offscreenCapabilitySet.offscreenCacheSize = ConstValue.OFFSCREEN_CAP_MAX_CACHE_SIZE;
            offscreenCapabilitySet.offscreenCacheEntries = ConstValue.OFFSCREEN_CAP_CACHE_ENTRY_NUM;
            offscreenCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(offscreenCapabilitySet);

            capabilitySets.Add(offscreenCapabilitySet);
            #endregion Populating Offscreen Bitmap Cache Capability Set

            #region Populating Virtual Channel Capability Set
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
            controlCapabilitySet.controlInterest = ConstValue.CONTROLPRIORITY_NEVER;
            controlCapabilitySet.detachInterest = ConstValue.CONTROLPRIORITY_NEVER;
            controlCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(controlCapabilitySet);

            capabilitySets.Add(controlCapabilitySet);
            #endregion Populating Control Capability Set

            #region Populating Windows Capability Set
            TS_WINDOWACTIVATION_CAPABILITYSET windowsCapabilitySet = new TS_WINDOWACTIVATION_CAPABILITYSET();
            windowsCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_WINDOW;
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
            fontCapabilitySet.fontSupportFlags = ConstValue.FONTSUPPORT_FONTLIST;
            fontCapabilitySet.pad2octets = 0;
            fontCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(fontCapabilitySet);

            capabilitySets.Add(fontCapabilitySet);
            #endregion Populating Font Capability Set

            #region Populating Multifragment Update Capability Set
            TS_MULTIFRAGMENTUPDATE_CAPABILITYSET multiFragmentCapabilitySet =
                new TS_MULTIFRAGMENTUPDATE_CAPABILITYSET();
            multiFragmentCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_MULTIFRAGMENTUPDATE;
            multiFragmentCapabilitySet.MaxRequestSize = ConstValue.MULTIFRAGMENT_CAP_MAX_REQUEST_SIZE;
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
            #endregion fill capabilities

            return capabilitySets;
        }


        /// <summary>
        /// Update session key according to section 5.3.7 Session Key Updates.
        /// </summary>
        public void UpdateSessionKey()
        {
            context.UpdateSessionKey();

            // inform the event has happened and the blocked receive thread can continue
            updateSessionKeyEvent.Set();
        }


        /// <summary>
        /// Check whether the number of the encrypted packets is equal PduCountofUpdateSessionKey.
        /// If so, generate an UpdateSessionKeyPdu.
        /// </summary>
        private void CheckEncryptionCount()
        {
            // If the pdu encryption count have reached the count to update session key,
            // then put message UpdateSessionKeyPdu to queue.
            if (context.EncryptionCount == context.PduCountofUpdateSessionKey)
            {
                UpdateSessionKeyPdu pdu = new UpdateSessionKeyPdu();
                TransportEvent packetEvent = new TransportEvent(EventType.ReceivedPacket, null, pdu);
                transportStack.AddEvent(packetEvent);
            }
        }


        /// <summary>
        /// Check whether the number of the decrypted packets is equal PduCountofUpdateSessionKey.
        /// If so, generate an UpdateSessionKeyPdu.
        /// </summary>
        internal void CheckDecryptionCount()
        {
            // If the pdu decryption count reached the count to update session key,
            // then put message UpdateSessionKeyPdu to queue.
            if (context.DecryptionCount == context.PduCountofUpdateSessionKey)
            {
                UpdateSessionKeyPdu pdu = new UpdateSessionKeyPdu();
                TransportEvent packetEvent = new TransportEvent(EventType.ReceivedPacket, null, pdu);
                transportStack.AddEvent(packetEvent);

                // If the DecryptionCount increased to PduCountofUpdateSessionKey, that means
                // the received thread has to wait for user to trigger update session key event.
                updateSessionKeyEvent.WaitOne(ConstValue.UPDATE_SESSION_KEY_TIMEOUT, false);
                updateSessionKeyEvent.Reset();
            }
        }

        /// <summary>
        /// Create a Standard or Enhanced RDP Security Layer transport.
        /// </summary>
        /// <param name="protocol">Specify which protocol is used.</param>
        private void CreateTransport(EncryptedProtocol protocol)
        {
            if (protocol == EncryptedProtocol.DirectCredSsp)
            {
                if (clientStream.GetType() == typeof(NetworkStream))
                {
                    // the server name should start with "TERMSRV/"
                    string targetSPN = ConstValue.CREDSSP_SERVER_NAME_PREFIX + serverName;
                    clientStream = new CredSspStream(clientStream,
                                                     serverDomain,
                                                     targetSPN,
                                                     logonName,
                                                     logonPassword);
                    ((CredSspStream)clientStream).Authenticate();
                }
            }
            else
            {
                // It is only a simple TCP transport.
                // Set non-blocking mode.

                tcpClient.Client.Blocking = false;
                tcpClient.Client.ReceiveTimeout = SOCKET_RECEIVE_TIMEOUT;
            }

            // create a transport config
            transportConfig = new StreamConfig();
            transportConfig.Type = StackTransportType.Stream;
            transportConfig.Role = Role.Client;
            transportConfig.BufferSize = transportBufferSize;
            transportConfig.Stream = clientStream;

#if UT
            transportStack = new TransportStackMock(context, decoder.DecodePacketCallback);            
#else
            transportStack = new RdpbcgrClientTransportStack(transportConfig, decoder.DecodePacketCallback);
#endif
        }


        /// <summary>
        /// Update transport to an Negotiation-Based Security-Enhanced transport.
        /// </summary>
        internal void UpdateTransport()
        {
            if (encryptedProtocol == EncryptedProtocol.NegotiationTls)
            {
                if (clientStream.GetType() == typeof(NetworkStream))
                {
                    clientStream = new SslStream(
                        clientStream,
                        false,
                        new RemoteCertificateValidationCallback(ValidateServerCertificate),
                        null
                        );
                    transportConfig.Stream = clientStream;
                    transportStack.UpdateConfig(transportConfig, () =>
                    {
                        // Restore to blocking mode.
                        tcpClient.Client.Blocking = true;
                        tcpClient.Client.ReceiveTimeout = 0;

                        ((SslStream)clientStream).AuthenticateAsClient(serverName, null, TlsVersion, false);
                    });
                }
            }
            else if (encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                if (clientStream.GetType() == typeof(NetworkStream))
                {
                    // the server name should start with "TERMSRV/"
                    string target = serverName;
                    if (!serverName.StartsWith(ConstValue.CREDSSP_SERVER_NAME_PREFIX))
                    {
                        target = ConstValue.CREDSSP_SERVER_NAME_PREFIX + serverName;
                    }

                    clientStream = new CredSspStream(clientStream,
                                                     serverDomain,
                                                     target,
                                                     logonName,
                                                     logonPassword);
                    transportConfig.Stream = clientStream;
                    transportStack.UpdateConfig(transportConfig, () =>
                    {
                        // Restore to blocking mode.
                        tcpClient.Client.Blocking = true;
                        tcpClient.Client.ReceiveTimeout = 0;

                        ((CredSspStream)clientStream).Authenticate();

                        if (Context.ServerSelectedProtocol == (uint)selectedProtocols_Values.PROTOCOL_HYBRID_EX)
                        {
                            // Expect Early User Authorization Result PDU from SUT.
                            Context.IsExpectingEarlyUserAuthorizationResultPDU = true;
                        }
                    });
                }
            }
            // else do nothing
        }


        #region SSL methods
        /// <summary>
        /// This method is used for SSL authentication.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="certificate">Not used.</param>
        /// <param name="chain">Not used.</param>
        /// <param name="sslPolicyErrors">Specify if error occurs.</param>
        /// <returns>true: the server certificate is valid, 
        /// false: the server certificate is invalid.</returns>
        private static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //{
            //    return true;
            //}

            //return false;
        }
        #endregion SSL methods

        #region SVC Manager control

        /// <summary>
        /// Start Static virtual channel manager
        /// After started, this manager will process SVC data automatically, 
        /// and transfer SVC data to high level protocols by using Received event
        /// </summary>
        public void StartSVCManager()
        {
            if (this.context.SVCManager != null)
            {
                if (!this.context.SVCManager.IsRunning)
                {
                    this.context.SVCManager.Start();
                }
            }
        }

        /// <summary>
        /// Stop Static virtual channel manager
        /// </summary>
        public void StopSVCManager()
        {
            if (this.context.SVCManager != null)
            {
                if (this.context.SVCManager.IsRunning)
                {
                    this.context.SVCManager.Stop();
                }
            }
        }

        #endregion SVC Manager control

        #endregion help methods

        #region connection operations
        /// <summary>
        /// Build a RDPBCGR connection, includes all 23 connection messages.
        /// Support both Standard RDP Security and Enhanced Security.
        /// </summary>
        /// <param name="virtualChannels">The virtual channels user wants to join. This argument can be null.
        /// If it is null, then no channel will be joined.</param>
        /// <param name="alternateShell">Variable-length path to the executable file of an alternate shell.
        /// This argument can be null. If it is null, then the size field will be 0, the content field will
        /// be the same with TD.</param>
        /// <param name="workingDir">Variable-length directory that contains the executable file specified in the 
        /// AlternateShell field or any related files (the length in bytes is given by the cbWorkingDir field).
        /// This argument can be null. If it is null, then the size field will be 0, the content field will
        /// be the same with TD.</param>
        /// <param name="timeout">Timeout of completing the connection.</param>
        /// <param name="confirmCapabilitySets">The capability sets specified in Client Confirm Active PDU.
        /// This argument can be null. If it is null, then no capability set will be specified.
        /// This argument could be got from method CreateCapabilitySets.</param>
        /// <exception>TimeoutException, InvalidCastException or Socket exceptions.</exception>
        public void RdpbcgrConnect(CHANNEL_DEF[] virtualChannels,
                                   string alternateShell,
                                   string workingDir,
                                   TimeSpan timeout,
                                   Collection<ITsCapsSet> confirmCapabilitySets)
        {
            #region clear the configuration
            context.ClearAll();
            #endregion clear the configuration

            #region default parameters
            requestedProtocols_Values requestProtocols;
            if (encryptedProtocol == EncryptedProtocol.NegotiationTls)
            {
                requestProtocols = requestedProtocols_Values.PROTOCOL_SSL_FLAG;
            }
            else if (encryptedProtocol == EncryptedProtocol.DirectCredSsp
                || encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                requestProtocols = requestedProtocols_Values.PROTOCOL_HYBRID_FLAG
                                 | requestedProtocols_Values.PROTOCOL_SSL_FLAG;
            }
            else
            {
                requestProtocols = requestedProtocols_Values.PROTOCOL_RDP_FLAG;
            }

            UInt32 clientBuild = ConstValue.CLIENT_BUILD_DEFAULT;
            string clientDigProductId = System.Guid.NewGuid().ToString();
            bool isReconnect = false;

            flags_Values flags = flags_Values.INFO_MOUSE
                               | flags_Values.INFO_DISABLECTRLALTDEL
                               | flags_Values.INFO_UNICODE
                               | flags_Values.INFO_MAXIMIZESHELL
                               | flags_Values.INFO_ENABLEWINDOWSKEY
                               | flags_Values.INFO_FORCE_ENCRYPTED_CS_PDU
                               | flags_Values.INFO_LOGONNOTIFY
                               | flags_Values.INFO_AUTOLOGON;

            encryptionMethod_Values encryptionMethod = encryptionMethod_Values._40BIT_ENCRYPTION_FLAG
                                                     | encryptionMethod_Values._56BIT_ENCRYPTION_FLAG
                                                     | encryptionMethod_Values._128BIT_ENCRYPTION_FLAG
                                                     | encryptionMethod_Values.FIPS_ENCRYPTION_FLAG;
            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;
            #endregion default parameters

            #region Connection Initiation
            Client_X_224_Connection_Request_Pdu x224ConnectReqPdu = CreateX224ConnectionRequestPdu(requestProtocols);
            SendPdu(x224ConnectReqPdu);
            Server_X_224_Connection_Confirm_Pdu x224ConnectConfirmPdu =
                (Server_X_224_Connection_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Connection Initiation

            #region Basic Settings Exchange
            Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mcsInitialPdu =
                CreateMCSConnectInitialPduWithGCCConferenceCreateRequestPdu
                (Dns.GetHostName(), clientBuild, clientDigProductId, encryptionMethod, virtualChannels);
            SendPdu(mcsInitialPdu);
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectRspPdu =
                (Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response)
                ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Basic Settings Exchange

            #region Channel Connection
            Client_MCS_Erect_Domain_Request erectDomainPdu = CreateMCSErectDomainRequestPdu();
            SendPdu(erectDomainPdu);
            Client_MCS_Attach_User_Request attachUserRequestPdu = CreateMCSAttachUserRequestPdu();
            SendPdu(attachUserRequestPdu);
            Server_MCS_Attach_User_Confirm_Pdu attachConfirmPdu =
                (Server_MCS_Attach_User_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #region channel join
            Client_MCS_Channel_Join_Request userChannelJoin = CreateMCSChannelJoinRequestPdu(context.UserChannelId);
            SendPdu(userChannelJoin);
            Server_MCS_Channel_Join_Confirm_Pdu joinConfirm =
                (Server_MCS_Channel_Join_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Client_MCS_Channel_Join_Request ioChannelJoin = CreateMCSChannelJoinRequestPdu(context.IOChannelId);
            SendPdu(ioChannelJoin);
            joinConfirm = (Server_MCS_Channel_Join_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            ushort[] virtualChannelIds = context.VirtualChannelIdStore;
            if (virtualChannelIds != null)
            {
                foreach (ushort channelId in virtualChannelIds)
                {
                    Client_MCS_Channel_Join_Request mcsChannelJoin = CreateMCSChannelJoinRequestPdu(channelId);
                    SendPdu(mcsChannelJoin);
                    joinConfirm = (Server_MCS_Channel_Join_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
                }
            }
            #endregion channel join
            #endregion Channel Connection

            #region RDP Security Commencement
            // for enhanced security, don't send Client_Security_Exchange_Pdu
            if (requestProtocols == requestedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                byte[] clientRandom = RdpbcgrUtility.GenerateRandom(ConstValue.CLIENT_RANDOM_SIZE);
                Client_Security_Exchange_Pdu securtiyExchangePdu = CreateSecurityExchangePdu(clientRandom);
                SendPdu(securtiyExchangePdu);
            }
            #endregion RDP Security Commencement

            #region Secure Settings Exchange
            Client_Info_Pdu clientInfoPdu = CreateClientInfoPdu(flags,
                                                                serverDomain,
                                                                logonName,
                                                                logonPassword,
                                                                clientIPAddress,
                                                                alternateShell,
                                                                workingDir,
                                                                isReconnect);
            SendPdu(clientInfoPdu);
            #endregion Secure Settings Exchange

            #region Licensing
            Server_License_Error_Pdu_Valid_Client licenseErrorPdu =
                (Server_License_Error_Pdu_Valid_Client)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Licensing

            #region Capabilities Exchange
            Server_Demand_Active_Pdu demandActivePdu =
                (Server_Demand_Active_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            Client_Confirm_Active_Pdu confirmActivePdu = CreateConfirmActivePdu(confirmCapabilitySets);
            SendPdu(confirmActivePdu);
            #endregion Capabilities Exchange

            #region Connection Finalization
            Client_Synchronize_Pdu clientSynchrnizePdu = CreateSynchronizePdu();
            SendPdu(clientSynchrnizePdu);

            Client_Control_Pdu_Cooperate controlCoopPdu = CreateControlCooperatePdu();
            SendPdu(controlCoopPdu);

            Client_Control_Pdu_Request_Control controlRequest = CreateControlRequestPdu();
            SendPdu(controlRequest);

            Client_Persistent_Key_List_Pdu persistentKeyPdu = CreatePersistentKeyListPdu();
            SendPdu(persistentKeyPdu);

            Client_Font_List_Pdu fontListPdu = CreateFontListPdu();
            SendPdu(fontListPdu);

            Server_Synchronize_Pdu serverSynchronizePdu =
                (Server_Synchronize_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Control_Pdu_Cooperate serverControlCoopPdu =
                (Server_Control_Pdu_Cooperate)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Control_Pdu_Granted_Control serverControlGrantedPdu =
                (Server_Control_Pdu_Granted_Control)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Font_Map_Pdu serverFontPdu = (Server_Font_Map_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Connection Finalization
        }


        /// <summary>
        /// Build a RDPBCGR connection, includes all 23 connection messages.
        /// Support both Standard RDP Security and Enhanced Security.
        /// </summary>
        /// <param name="virtualChannels">The virtual channels user wants to join. This argument can be null.
        /// If it is null, then no channel will be joined.</param>
        /// <param name="alternateShell">Variable-length path to the executable file of an alternate shell.
        /// This argument can be null. If it is null, then the size field will be 0, the content field will
        /// be the same with TD.</param>
        /// <param name="workingDir">Variable-length directory that contains the executable file specified in the 
        /// AlternateShell field or any related files (the length in bytes is given by the cbWorkingDir field).
        /// This argument can be null. If it is null, then the size field will be 0, the content field will
        /// be the same with TD.</param>
        /// <param name="timeout">Timeout of completing the connection.</param>
        /// <exception>TimeoutException, InvalidCastException or Socket exceptions.</exception>
        public void RdpbcgrConnect(CHANNEL_DEF[] virtualChannels,
                                   string alternateShell,
                                   string workingDir,
                                   TimeSpan timeout)
        {
            Collection<ITsCapsSet> capabilitySets = CreateCapabilitySets();

            RdpbcgrConnect(virtualChannels, alternateShell, workingDir, timeout, capabilitySets);
        }


        /// <summary>
        /// Build a RDPBCGR connection, includes all 23 connection messages.
        /// Support both Standard RDP Security and Enhanced Security.
        /// </summary>
        /// <param name="virtualChannelNames">The names of the virtual channels user wants to join.
        /// This argument can be null. when this argument is null, no channel will be joined.</param>
        /// <param name="alternateShell">Variable-length path to the executable file of an alternate shell.
        /// This argument can be null. If it is null, then the size field will be 0, the content field will
        /// be the same with TD.</param>
        /// <param name="workingDir">Variable-length directory that contains the executable file specified in the 
        /// AlternateShell field or any related files (the length in bytes is given by the cbWorkingDir field).
        /// This argument can be null. If it is null, then the size field will be 0, the content field will
        /// be the same with TD.</param>
        /// <param name="totalTimeout">Timeout of completing the connection.</param>
        /// <exception>TimeoutException, InvalidCastException or Socket exceptions.</exception>
        public void RdpbcgrConnect(string[] virtualChannelNames,
                                   string alternateShell,
                                   string workingDir,
                                   TimeSpan totalTimeout)
        {
            // Fill channelDefArray
            CHANNEL_DEF[] channelDefArray = null;
            if (virtualChannelNames != null)
            {
                channelDefArray = new CHANNEL_DEF[virtualChannelNames.Length];
                for (int i = 0; i < virtualChannelNames.Length; i++)
                {
                    channelDefArray[i].options = Channel_Options.COMPRESS_RDP
                                               | Channel_Options.INITIALIZED;
                    channelDefArray[i].name = virtualChannelNames[i];
                }
            }

            RdpbcgrConnect(channelDefArray, alternateShell, workingDir, totalTimeout);
        }


        /// <summary>
        /// Do RdpbcgrConnect again with reconnect cookie. This method only performs RDPBCGR messages transport.
        /// Closing or reconnecting TCP connection is not included in this method.
        /// This method should be called after a normal connection and receiving 
        /// a Save Session Info PDU that contains the reconnect cookie.
        /// Please close the former connection before this call.
        /// </summary>
        /// <param name="timeout">Timeout of completing reconnection.</param>
        /// <exception>TimeoutException, InvalidCastException or Socket exceptions.</exception>
        public void RdpbcgrReconnect(TimeSpan timeout)
        {
            #region clear the configuration
            requestedProtocols_Values requestedProtocol = context.RequestedProtocol;
            string clientName = context.ClientName;
            uint clientBuild = context.ClientBuild;
            string clientDigProductId = context.ClientDigProductId;
            encryptionMethod_Values encryptionMethod = (encryptionMethod_Values)context.RdpEncryptionMethod;
            CHANNEL_DEF[] virtualChannelDefines = context.VirtualChannelDefines;
            Collection<ITsCapsSet> confirmCapabilitySets = context.ConfirmCapabilitySets;
            context.ClearForReconnect();
            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;
            #endregion clear the configuration

            #region Connection Initiation
            Client_X_224_Connection_Request_Pdu x224ConnectReqPdu = CreateX224ConnectionRequestPdu(requestedProtocol);
            SendPdu(x224ConnectReqPdu);
            Server_X_224_Connection_Confirm_Pdu x224ConnectConfirmPdu =
                (Server_X_224_Connection_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Connection Initiation

            #region Basic Settings Exchange
            Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mcsInitialPdu =
                CreateMCSConnectInitialPduWithGCCConferenceCreateRequestPdu(clientName,
                                                                            clientBuild,
                                                                            clientDigProductId,
                                                                            encryptionMethod,
                                                                            virtualChannelDefines);
            SendPdu(mcsInitialPdu);
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectRspPdu =
                (Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response)
                ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Basic Settings Exchange

            #region Channel Connection
            Client_MCS_Erect_Domain_Request erectDomainPdu = CreateMCSErectDomainRequestPdu();
            SendPdu(erectDomainPdu);
            Client_MCS_Attach_User_Request attachUserRequestPdu = CreateMCSAttachUserRequestPdu();
            SendPdu(attachUserRequestPdu);
            Server_MCS_Attach_User_Confirm_Pdu attachConfirmPdu =
                (Server_MCS_Attach_User_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            #region channel join
            Client_MCS_Channel_Join_Request userChannelJoin = CreateMCSChannelJoinRequestPdu(context.UserChannelId);
            SendPdu(userChannelJoin);
            Server_MCS_Channel_Join_Confirm_Pdu joinConfirm =
                (Server_MCS_Channel_Join_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Client_MCS_Channel_Join_Request ioChannelJoin = CreateMCSChannelJoinRequestPdu(context.IOChannelId);
            SendPdu(ioChannelJoin);
            joinConfirm = (Server_MCS_Channel_Join_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            ushort[] virtualChannelIds = context.VirtualChannelIdStore;
            if (virtualChannelIds != null)
            {
                foreach (ushort channelId in virtualChannelIds)
                {
                    Client_MCS_Channel_Join_Request mcsChannelJoin = CreateMCSChannelJoinRequestPdu(channelId);
                    SendPdu(mcsChannelJoin);
                    joinConfirm = (Server_MCS_Channel_Join_Confirm_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
                }
            }
            #endregion channel join
            #endregion Channel Connection

            #region RDP Security Commencement
            // for enhanced security, don't send Client_Security_Exchange_Pdu
            if (requestedProtocol == requestedProtocols_Values.PROTOCOL_RDP_FLAG)
            {
                byte[] clientRandom = RdpbcgrUtility.GenerateRandom(ConstValue.CLIENT_RANDOM_SIZE);
                Client_Security_Exchange_Pdu securtiyExchangePdu = CreateSecurityExchangePdu(clientRandom);
                SendPdu(securtiyExchangePdu);
            }
            #endregion RDP Security Commencement

            #region Secure Settings Exchange
            // for reconnect, the user name, password and etc. are not required any more
            Client_Info_Pdu clientInfoPdu = CreateClientInfoPdu((flags_Values)0,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                true);
            SendPdu(clientInfoPdu);
            #endregion Secure Settings Exchange

            #region Licensing
            Server_License_Error_Pdu_Valid_Client licenseErrorPdu =
                (Server_License_Error_Pdu_Valid_Client)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Licensing

            #region Capabilities Exchange
            Server_Demand_Active_Pdu demandActivePdu =
                (Server_Demand_Active_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            Client_Confirm_Active_Pdu confirmActivePdu = CreateConfirmActivePdu(confirmCapabilitySets);
            SendPdu(confirmActivePdu);
            #endregion Capabilities Exchange

            #region Connection Finalization
            Client_Synchronize_Pdu clientSynchrnizePdu = CreateSynchronizePdu();
            SendPdu(clientSynchrnizePdu);

            Client_Control_Pdu_Cooperate controlCoopPdu = CreateControlCooperatePdu();
            SendPdu(controlCoopPdu);

            Client_Control_Pdu_Request_Control controlRequest = CreateControlRequestPdu();
            SendPdu(controlRequest);

            Client_Persistent_Key_List_Pdu persistentKeyPdu = CreatePersistentKeyListPdu();
            SendPdu(persistentKeyPdu);

            Client_Font_List_Pdu fontListPdu = CreateFontListPdu();
            SendPdu(fontListPdu);

            Server_Synchronize_Pdu serverSynchronizePdu =
                (Server_Synchronize_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Control_Pdu_Cooperate serverControlCoopPdu =
                (Server_Control_Pdu_Cooperate)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Control_Pdu_Granted_Control serverControlGrantedPdu =
                (Server_Control_Pdu_Granted_Control)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Font_Map_Pdu serverFontPdu = (Server_Font_Map_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Connection Finalization
        }


        /// <summary>
        /// This method is called after receiving Deactivate All PDU, then reactivate.
        /// This method should be called after calling method Connect.
        /// </summary>
        /// <param name="timeout">Timeout of completing reactivation.</param>
        /// <exception>TimeoutException, InvalidCastException or Socket exceptions.</exception>
        public void Reactivate(TimeSpan timeout)
        {
            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;

            #region Capabilities Exchange
            Server_Demand_Active_Pdu demandActivePdu =
                (Server_Demand_Active_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            Collection<ITsCapsSet> capabilitySets = context.ConfirmCapabilitySets;
            Client_Confirm_Active_Pdu confirmActivePdu;
            if (capabilitySets == null)
            {
                confirmActivePdu = CreateConfirmActivePdu();
            }
            else
            {
                confirmActivePdu = CreateConfirmActivePdu(capabilitySets);
            }
            SendPdu(confirmActivePdu);
            #endregion Capabilities Exchange

            #region Connection Finalization
            Client_Synchronize_Pdu clientSynchrnizePdu = CreateSynchronizePdu();
            SendPdu(clientSynchrnizePdu);

            Client_Control_Pdu_Cooperate controlCoopPdu = CreateControlCooperatePdu();
            SendPdu(controlCoopPdu);

            Client_Control_Pdu_Request_Control controlRequest = CreateControlRequestPdu();
            SendPdu(controlRequest);

            Client_Font_List_Pdu fontListPdu = CreateFontListPdu();
            SendPdu(fontListPdu);

            Server_Synchronize_Pdu serverSynchronizePdu =
                (Server_Synchronize_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Control_Pdu_Cooperate serverControlCoopPdu =
                (Server_Control_Pdu_Cooperate)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Control_Pdu_Granted_Control serverControlGrantedPdu =
                (Server_Control_Pdu_Granted_Control)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Font_Map_Pdu serverFontPdu = (Server_Font_Map_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            #endregion Connection Finalization
        }


        /// <summary>
        /// Initiate a client-side disconnect by closing the RDP application.
        /// After this method, user should call Disconnect to close the TCP connection.
        /// This method only supports "If a logged-on user account is associated with the session".
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <exception>TimeoutException, InvalidCastException or Socket exceptions.</exception>
        public void RdpbcgrDisconnect(TimeSpan timeout)
        {
            Client_Shutdown_Request_Pdu sutdownPdu = CreateShutdownRequestPdu();
            SendPdu(sutdownPdu);

            // What we cared is only Server_Shutdown_Request_Denied_PDU
            // So ignore all other types
            List<Type> pduFilter = new List<Type>();
            pduFilter.Add(typeof(TS_MONITOR_LAYOUT_PDU));
            pduFilter.Add(typeof(Server_Save_Session_Info_Pdu));
            pduFilter.Add(typeof(Server_Play_Sound_Pdu));
            pduFilter.Add(typeof(TS_FP_UPDATE_PDU));
            pduFilter.Add(typeof(SlowPathOutputPdu));
            pduFilter.Add(typeof(Server_Set_Keyboard_IME_Status_Pdu));
            pduFilter.Add(typeof(Server_Set_Keyboard_Indicators_Pdu));
            pduFilter.Add(typeof(Server_Set_Error_Info_Pdu));
            pduFilter.Add(typeof(Server_Status_Info_Pdu));
            pduFilter.Add(typeof(Server_Auto_Reconnect_Status_Pdu));
            pduFilter.Add(typeof(Server_Deactivate_All_Pdu));
            pduFilter.Add(typeof(Virtual_Channel_RAW_Pdu));

            ClearFilter();
            AddFilter(pduFilter.ToArray());

            Server_Shutdown_Request_Denied_Pdu shutdownDeniedPdu =
                (Server_Shutdown_Request_Denied_Pdu)ExpectPdu(timeout);
            MCS_Disconnect_Provider_Ultimatum_Pdu mcsDisconnectPdu =
                CreateMCSDisconnectProviderUltimatumPdu(ConstValue.RN_USER_REQUESTED);
            SendPdu(mcsDisconnectPdu);
        }


        /// <summary>
        /// Make a TCP connection to perform RDPBCGR messages transport.
        /// </summary>
        /// <param name="protocol">Specify whether Standard RDP Security or Enhanced RDP Security will be used. 
        /// If select Enhanced RDP Security, specify Negotiation-Based Approach or Direct Approach will be 
        /// used.</param>
        public void Connect(EncryptedProtocol protocol)
        {
            encryptedProtocol = protocol;
#if !UT
            tcpClient = new TcpClient(serverName.ParseIPAddress().ToString(), serverPort); //Initialize and synchronous connect
            clientStream = tcpClient.GetStream();
#endif
            // create a type of transport
            CreateTransport(protocol);
            transportStack.Connect();
            transportStack.PacketFilter = new PacketFilter();

            context.LocalIdentity = tcpClient.Client.LocalEndPoint;
            context.RemoteIdentity = tcpClient.Client.RemoteEndPoint;

            disconnected = false;
        }


        /// <summary>
        /// Close the TCP connection.
        /// </summary>
        public void Disconnect()
        {
            if (clientStream != null)
            {
                clientStream.Close();
            }
            if (transportStack != null)
            {
                transportStack.Disconnect();
                transportStack.Dispose();
                transportStack = null;
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
        #endregion connection operations


        #region Config API
        /// <summary>
        /// The filter is based on type of the received message. if some types  
        /// of response message is added into the filter, all received packets
        /// of these types will be filtered in transport. Therefore, it is used 
        /// to filter some types of responses by TSD.
        /// </summary>
        /// <param name="types">The types of response packet to be filtered.</param>
        public void AddFilter(Type[] types)
        {
            transportStack.PacketFilter.AddFilters(types);
        }


        /// <summary>
        /// Clean all the filters in transport.
        /// </summary>
        public void ClearFilter()
        {
            transportStack.PacketFilter.ClearFilters();
        }
        #endregion Config API


        #region packet API
        #region connection
        /// <summary>
        /// Create an X224 Connection Request PDU.
        /// User can set special value in the PDU other than the default after calling this method.
        /// Then call SendPdu to send the packet.
        /// </summary>
        /// <param name="requestedProtocols">Specify the requested protocol of RDP_NEG_REQ.</param>
        /// <returns>X224 Connection Request PDU.</returns>
        public Client_X_224_Connection_Request_Pdu CreateX224ConnectionRequestPdu(
            requestedProtocols_Values requestedProtocols)
        {
            Client_X_224_Connection_Request_Pdu x224 = new Client_X_224_Connection_Request_Pdu(this.context);
            int tpktLength = Marshal.SizeOf(typeof(TpktHeader))
                           + Marshal.SizeOf(typeof(X224Crq))
                           + sizeof(byte)
                           + sizeof(byte)
                           + sizeof(ushort)
                           + sizeof(uint);

            RdpbcgrUtility.FillTpktHeader(ref x224.tpktHeader, (ushort)tpktLength);

            x224.x224Crq.lengthIndicator = (byte)(tpktLength - Marshal.SizeOf(x224.tpktHeader)
                                         - Marshal.SizeOf(x224.x224Crq.lengthIndicator));
            x224.x224Crq.typeCredit = ConstValue.X224_CONNECTION_REQUEST_TYPE;
            x224.x224Crq.destRef = 0;
            x224.x224Crq.srcRef = 0;
            x224.x224Crq.classOptions = 0;

            x224.routingToken = null;

            RDP_NEG_REQ pdu = new RDP_NEG_REQ();
            pdu.type = type_Values.V1;
            pdu.flags = RDP_NEG_REQ_flags_Values.V1;
            pdu.length = length_Values.V1;
            pdu.requestedProtocols = requestedProtocols;
            x224.rdpNegData = pdu;
            return x224;
        }


        /// <summary>
        /// Create MCS Connect Initial PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// If user do not want to present some fields of TS_UD_CS_CORE according to TD, then set them null.
        /// After set this value, do remember to update the length field of the header for clientCoreData.
        /// Other changes in clientNetworkData, clientClusterData or clientMonitorData need update 
        /// the corresponding header length too.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// <param name="clientName">The name of the client computer. This argument can be null.
        /// If it is null, then the size field will be 0, the content field will be the same with TD.</param>
        /// <param name="clientBuild">The build number of the client.</param>
        /// <param name="clientDigProductId">Contains a value that uniquely identifies the client. 
        /// This argument can be null. If it is null, then the size field will be 0, the content field will be the
        /// same with TD.</param>
        /// <param name="encryptionMethod">Cryptographic methods supported by the client and used in conjunction 
        /// with Standard RDP Security.</param>
        /// <param name="virtualChannels">The static virtual channels to be requested with their option flag.
        /// This argument can be null. If it is null, then no channel will join.</param>
        /// <returns>MCS Connect Initial PDU.</returns>
        public Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request
            CreateMCSConnectInitialPduWithGCCConferenceCreateRequestPdu(string clientName,
                                                                        UInt32 clientBuild,
                                                                        string clientDigProductId,
                                                                        encryptionMethod_Values encryptionMethod,
                                                                        CHANNEL_DEF[] virtualChannels,
                                                                        bool supportRDPTransportReliable = false,
                                                                        bool supportRDPTransportLossy = false,
                                                                        bool isMonitorDataPresent = false)
        {
            Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mcsInitialPdu
                = new Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request(this.context);

            #region Filling clientCoreData structure TS_UD_CS_CORE
            TS_UD_CS_CORE clientCoreData = new TS_UD_CS_CORE();
            int coreDataSize = 0;
            clientCoreData.header.type = TS_UD_HEADER_type_Values.CS_CORE;
            coreDataSize += Marshal.SizeOf(clientCoreData.header);
            clientCoreData.version = TS_UD_CS_CORE_version_Values.V2;
            coreDataSize += sizeof(uint);
            clientCoreData.desktopWidth = ConstValue.DESKTOP_WIDTH_DEFAULT;
            coreDataSize += Marshal.SizeOf(clientCoreData.desktopWidth);
            clientCoreData.desktopHeight = ConstValue.DESKTOP_HEIGHT_DEFAULT;
            coreDataSize += Marshal.SizeOf(clientCoreData.desktopHeight);
            clientCoreData.colorDepth = colorDepth_Values.RNS_UD_COLOR_8BPP;
            coreDataSize += sizeof(ushort);
            clientCoreData.SASSequence = ConstValue.RNS_UD_SAS_DEL;
            coreDataSize += Marshal.SizeOf(clientCoreData.SASSequence);
            clientCoreData.keyboardLayout = ConstValue.LOCALE_ENGLISH_UNITED_STATES;
            coreDataSize += Marshal.SizeOf(clientCoreData.keyboardLayout);
            clientCoreData.clientBuild = clientBuild;
            coreDataSize += Marshal.SizeOf(clientCoreData.clientBuild);
            clientCoreData.clientName = clientName;
            coreDataSize += ConstValue.CLIENT_CORE_DATA_CLIENT_NAME_SIZE;
            clientCoreData.keyboardType = keyboardType_Values.V4;
            coreDataSize += sizeof(uint);
            clientCoreData.keyboardSubType = 0;
            coreDataSize += Marshal.SizeOf(clientCoreData.keyboardSubType);
            clientCoreData.keyboardFunctionKey = ConstValue.KEYBOARD_FUNCTION_KEY_NUMBER_DEFAULT;
            coreDataSize += Marshal.SizeOf(clientCoreData.keyboardFunctionKey);
            clientCoreData.imeFileName = null;
            coreDataSize += ConstValue.CLIENT_CORE_DATA_IME_FILE_NAME_SIZE;
            clientCoreData.postBeta2ColorDepth = new UInt16Class((ushort)postBeta2ColorDepth_Values.RNS_UD_COLOR_8BPP);
            coreDataSize += sizeof(ushort);
            clientCoreData.clientProductId = new UInt16Class((ushort)ConstValue.CLIENT_PRODUCT_ID_DEFAULT);
            coreDataSize += sizeof(ushort);
            clientCoreData.serialNumber = new UInt32Class(0);
            coreDataSize += sizeof(UInt32);
            clientCoreData.highColorDepth = new UInt16Class((ushort)highColorDepth_Values.V5);
            coreDataSize += sizeof(ushort);
            clientCoreData.supportedColorDepths = new UInt16Class((ushort)(
                                                  supportedColorDepths_Values.RNS_UD_15BPP_SUPPORT
                                                | supportedColorDepths_Values.RNS_UD_16BPP_SUPPORT
                                                | supportedColorDepths_Values.RNS_UD_24BPP_SUPPORT
                                                | supportedColorDepths_Values.RNS_UD_32BPP_SUPPORT));
            coreDataSize += sizeof(ushort);
            clientCoreData.earlyCapabilityFlags =
                new UInt16Class((ushort)(earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_ERRINFO_PDU | earlyCapabilityFlags_Values.RNS_UD_CS_VALID_CONNECTION_TYPE | earlyCapabilityFlags_Values.RNS_UD_CS_WANT_32BPP_SESSION | earlyCapabilityFlags_Values.RNS_UD_CS_SUPPORT_STATUSINFO_PDU));
            coreDataSize += sizeof(ushort);
            clientCoreData.clientDigProductId = clientDigProductId;
            coreDataSize += ConstValue.CLIENT_CORE_DATA_CLIENT_DIG_PRODUCT_ID_SIZE;
            clientCoreData.connnectionType = new ByteClass((byte)ConnnectionType.CONNECTION_TYPE_LAN);
            coreDataSize += sizeof(byte);
            clientCoreData.pad1octets = new ByteClass(0);
            coreDataSize += sizeof(byte);
            clientCoreData.serverSelectedProtocol = new UInt32Class((uint)context.ServerSelectedProtocol);
            coreDataSize += sizeof(UInt32);
            clientCoreData.desktopPhysicalWidth = new UInt32Class(271);
            coreDataSize += sizeof(UInt32);
            clientCoreData.desktopPhysicalHeight = new UInt32Class(203);
            coreDataSize += sizeof(UInt32);
            clientCoreData.desktopOrientation = (TS_UD_CS_CORE_desktopOrientation_values)0;
            coreDataSize += sizeof(UInt16);
            clientCoreData.desktopScaleFactor = new UInt32Class(100);
            coreDataSize += sizeof(UInt32);
            clientCoreData.deviceScaleFactor = new UInt32Class(100);
            coreDataSize += sizeof(UInt32);
            clientCoreData.header.length = (ushort)coreDataSize;
            #endregion Filling clientCoreData structure TS_UD_CS_CORE

            #region Filling clientSecurityData TS_UD_CS_SEC
            TS_UD_CS_SEC clientSecurityData = new TS_UD_CS_SEC();
            clientSecurityData.header.type = TS_UD_HEADER_type_Values.CS_SECURITY;
            clientSecurityData.encryptionMethods = encryptionMethod;
            clientSecurityData.extEncryptionMethods = 0;
            clientSecurityData.header.length = RdpbcgrClient.TS_UD_CS_SEC_SecurityDataSize;
            #endregion Filling clientSecurityData TS_UD_CS_SEC

            #region Filling clientNetworkData TS_UD_CS_NET
            TS_UD_CS_NET clientNetworkData = new TS_UD_CS_NET();
            clientNetworkData.header.type = TS_UD_HEADER_type_Values.CS_NET;
            if (virtualChannels != null)
            {
                clientNetworkData.channelCount = (uint)virtualChannels.Length;
                if (virtualChannels.Length > 0)
                {
                    clientNetworkData.channelDefArray = new List<CHANNEL_DEF>();
                    clientNetworkData.channelDefArray.AddRange(virtualChannels);
                }
            }

            int networkDataSize = Marshal.SizeOf(clientNetworkData.header)
                                + Marshal.SizeOf(clientNetworkData.channelCount)
                                + (ConstValue.CHANNEL_DEF_NAME_SIZE + sizeof(uint)) * (int)clientNetworkData.channelCount;
            clientNetworkData.header.length = (ushort)networkDataSize;
            #endregion Filling clientNetworkData TS_UD_CS_NET

            #region Filling Client Monitor Data TS_UD_CS_MONITOR
            TS_UD_CS_MONITOR clientMonitorData = new TS_UD_CS_MONITOR();
            clientMonitorData.header.type = TS_UD_HEADER_type_Values.CS_MONITOR;
            clientMonitorData.Flags = 0;
            clientMonitorData.monitorCount = 1;
            Collection<TS_MONITOR_DEF> defArray = new Collection<TS_MONITOR_DEF>();
            TS_MONITOR_DEF monitorDef = new TS_MONITOR_DEF();
            monitorDef.left = ConstValue.PRIMARY_MONITOR_DEF_LEFT;
            monitorDef.top = ConstValue.PRIMARY_MONITOR_DEF_TOP;
            monitorDef.right = ConstValue.PRIMARY_MONITOR_DEF_RIGHT;
            monitorDef.bottom = ConstValue.PRIMARY_MONITOR_DEF_BOTTOM;
            monitorDef.flags = Flags_TS_MONITOR_DEF.TS_MONITOR_PRIMARY;
            defArray.Add(monitorDef);
            clientMonitorData.monitorDefArray = defArray;
            int monitorDataSize = Marshal.SizeOf(clientMonitorData.header)
                                + Marshal.SizeOf(clientMonitorData.Flags)
                                + Marshal.SizeOf(clientMonitorData.monitorCount)
                                + (int)clientMonitorData.monitorCount * 20;
            clientMonitorData.header.length = (ushort)monitorDataSize;
            #endregion Filling Client Monitor Data TS_UD_CS_MONITOR

            #region Filling clientClusterData TS_UD_CS_CLUSTER
            TS_UD_CS_CLUSTER clientClusterData = new TS_UD_CS_CLUSTER();
            clientClusterData.header.type = TS_UD_HEADER_type_Values.CS_CLUSTER;
            clientClusterData.Flags = (Flags_Values)0;
            clientClusterData.RedirectedSessionID = 0;
            int clusterDataSize = Marshal.SizeOf(clientClusterData.header)
                                + sizeof(uint)
                                + Marshal.SizeOf(clientClusterData.RedirectedSessionID);
            clientClusterData.header.length = (ushort)clusterDataSize;
            #endregion Filling clientClusterData TS_UD_CS_CLUSTER

            #region Filling clientMessageChannelData TS_UD_CS_MCS_MSGCHANNEL
            TS_UD_CS_MCS_MSGCHANNEL clientMessageChannelData = new TS_UD_CS_MCS_MSGCHANNEL();
            clientMessageChannelData.flags = 0;
            clientMessageChannelData.header.type = TS_UD_HEADER_type_Values.CS_MCS_MSGCHANNEL;
            clientMessageChannelData.header.length = 8;
            #endregion Filling clientMessageChannelData TS_UD_CS_MCS_MSGCHANNEL

            #region Filling clientMultitransportChannelData TS_UD_CS_MULTITRANSPORT
            TS_UD_CS_MULTITRANSPORT clientMultitransportChannelData = new TS_UD_CS_MULTITRANSPORT();
            clientMultitransportChannelData.flags = MULTITRANSPORT_TYPE_FLAGS.None;
            if (supportRDPTransportReliable)
            {
                clientMultitransportChannelData.flags |= MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR;
                clientMultitransportChannelData.flags |= MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDP_PREFERRED;
            }
            if (supportRDPTransportLossy)
            {
                clientMultitransportChannelData.flags |= MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL;
                clientMultitransportChannelData.flags |= MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDP_PREFERRED;
            }
            clientMultitransportChannelData.header.type = TS_UD_HEADER_type_Values.CS_MULTITRANSPORT;
            clientMultitransportChannelData.header.length = 8;
            #endregion Filling clientMultitransportChannelData TS_UD_CS_MULTITRANSPORT

            #region Filling ConnectGCC
            ConnectGCC gccPdu = new ConnectGCC();
            gccPdu.conferenceName = ConstValue.GCC_CONFERENCE_NAME_DEFAULT;
            gccPdu.lockedConference = false;
            gccPdu.listedConference = false;
            gccPdu.conductibleConference = false;
            gccPdu.terminationMethod = 0;
            gccPdu.clientCoreData = clientCoreData;
            gccPdu.clientSecurityData = clientSecurityData;
            gccPdu.clientNetworkData = clientNetworkData;
            if (isMonitorDataPresent)
            {
                gccPdu.clientMonitorData = clientMonitorData;
            }
            gccPdu.clientClusterData = clientClusterData;
            gccPdu.clientMultitransportChannelData = clientMultitransportChannelData;
            gccPdu.clientMessageChannelData = clientMessageChannelData;

            #endregion Filling ConnectGCC

            #region Filling MCSConnectInitial
            mcsInitialPdu.mcsCi = new MCSConnectInitial();
            mcsInitialPdu.mcsCi.calledDomainSelector = ConstValue.MCS_CALLED_DOMAIN_SELECTOR_DEFAULT;
            mcsInitialPdu.mcsCi.callingDomainSelector = ConstValue.MCS_CALLING_DOMAIN_SELECTOR_DEFAULT;
            mcsInitialPdu.mcsCi.upwardFlag = true;

            mcsInitialPdu.mcsCi.targetParameters.maxChannelIds = ConstValue.TARGET_PARAMETERS_MAX_CHANNEL_IDS;
            mcsInitialPdu.mcsCi.targetParameters.maxUserIds = ConstValue.TARGET_PARAMETERS_MAX_USER_IDS;
            mcsInitialPdu.mcsCi.targetParameters.maxTokenIds = ConstValue.TARGET_PARAMETERS_MAX_TOKEN_IDS;
            mcsInitialPdu.mcsCi.targetParameters.numPriorities = ConstValue.TARGET_PARAMETERS_NUM_PRIORITIES;
            mcsInitialPdu.mcsCi.targetParameters.minThroughput = ConstValue.TARGET_PARAMETERS_MIN_THROUGHPUT;
            mcsInitialPdu.mcsCi.targetParameters.maxHeight = ConstValue.TARGET_PARAMETERS_MAX_HEIGHT;
            mcsInitialPdu.mcsCi.targetParameters.maxMcsPduSize = ConstValue.TARGET_PARAMETERS_MAX_MCS_PDU_SIZE;
            mcsInitialPdu.mcsCi.targetParameters.protocolVersion = ConstValue.TARGET_PARAMETERS_PROTOCOL_VERSION;

            mcsInitialPdu.mcsCi.minimumParameters.maxChannelIds = ConstValue.MINIMUM_PARAMETERS_MAX_CHANNEL_IDS;
            mcsInitialPdu.mcsCi.minimumParameters.maxUserIds = ConstValue.MINIMUM_PARAMETERS_MAX_USER_IDS;
            mcsInitialPdu.mcsCi.minimumParameters.maxTokenIds = ConstValue.MINIMUM_PARAMETERS_MAX_TOKEN_IDS;
            mcsInitialPdu.mcsCi.minimumParameters.numPriorities = ConstValue.MINIMUM_PARAMETERS_NUM_PRIORITIES;
            mcsInitialPdu.mcsCi.minimumParameters.minThroughput = ConstValue.MINIMUM_PARAMETERS_MIN_THROUGHPUT;
            mcsInitialPdu.mcsCi.minimumParameters.maxHeight = ConstValue.MINIMUM_PARAMETERS_MAX_HEIGHT;
            mcsInitialPdu.mcsCi.minimumParameters.maxMcsPduSize = ConstValue.MINIMUM_PARAMETERS_MAX_MCS_PDU_SIZE;
            mcsInitialPdu.mcsCi.minimumParameters.protocolVersion = ConstValue.MINIMUM_PARAMETERS_PROTOCOL_VERSION;

            mcsInitialPdu.mcsCi.maximumParameters.maxChannelIds = ConstValue.MAXIMUM_PARAMETERS_MAX_CHANNEL_IDS;
            mcsInitialPdu.mcsCi.maximumParameters.maxUserIds = ConstValue.MAXIMUM_PARAMETERS_MAX_USER_IDS;
            mcsInitialPdu.mcsCi.maximumParameters.maxTokenIds = ConstValue.MAXIMUM_PARAMETERS_MAX_TOKEN_IDS;
            mcsInitialPdu.mcsCi.maximumParameters.numPriorities = ConstValue.MAXIMUM_PARAMETERS_NUM_PRIORITIES;
            mcsInitialPdu.mcsCi.maximumParameters.minThroughput = ConstValue.MAXIMUM_PARAMETERS_MIN_THROUGHPUT;
            mcsInitialPdu.mcsCi.maximumParameters.maxHeight = ConstValue.MAXIMUM_PARAMETERS_MAX_HEIGHT;
            mcsInitialPdu.mcsCi.maximumParameters.maxMcsPduSize = ConstValue.MAXIMUM_PARAMETERS_MAX_MCS_PDU_SIZE;
            mcsInitialPdu.mcsCi.maximumParameters.protocolVersion = ConstValue.MAXIMUM_PARAMETERS_PROTOCOL_VERSION;

            mcsInitialPdu.mcsCi.gccPdu = gccPdu;
            #endregion Filling MCSConnectInitial

            #region Filling mcsInitialPdu
            RdpbcgrUtility.FillTpktHeader(ref mcsInitialPdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref mcsInitialPdu.x224Data);
            #endregion Filling mcsInitialPdu


            return mcsInitialPdu;
        }


        /// <summary>
        /// Create MCS Connect Initial PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// If user do not want to present some fields of TS_UD_CS_CORE according to TD, then set them null.
        /// After set this value, do remember to update the length field of the header for clientCoreData.
        /// Other changes in clientNetworkData, clientClusterData or clientMonitorData need update 
        /// the corresponding header length too.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// <param name="clientName">Name of the client computer. This argument can be null.
        /// If it is null, then the size field will be 0, the content field will be the same with TD.</param>
        /// <param name="clientBuild">The build number of the client.</param>
        /// <param name="clientDigProductId">Contains a value that uniquely identifies the client. 
        /// This argument can be null. If it is null, then the size field will be 0, the content field will be the
        /// same with TD.</param>
        /// <param name="encryptionMethod">Cryptographic methods supported by the client and used in conjunction 
        /// with Standard RDP Security.</param>
        /// <param name="virtualChannelNames">The names of the static virtual channels to be requested.
        /// This argument can be null. If it is null, then no channel will join.</param>
        /// <returns>MCS Connect Initial PDU.</returns>
        public Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request
            CreateMCSConnectInitialPduWithGCCConferenceCreateRequestPdu(string clientName,
                                                                        UInt32 clientBuild,
                                                                        string clientDigProductId,
                                                                        encryptionMethod_Values encryptionMethod,
                                                                        string[] virtualChannelNames,
                                                                        bool supportRDPTransportReliable = false,
                                                                        bool supportRDPTransportLossy = false,
                                                                        bool isMonitorDataPresent = false)
        {
            CHANNEL_DEF[] channels = null;
            if (virtualChannelNames != null)
            {
                channels = new CHANNEL_DEF[virtualChannelNames.Length];
                for (int i = 0; i < virtualChannelNames.Length; ++i)
                {
                    channels[i].name = virtualChannelNames[i];
                    channels[i].options = Channel_Options.COMPRESS_RDP
                                        | Channel_Options.ENCRYPT_RDP
                                        | Channel_Options.INITIALIZED;
                }
            }

            return CreateMCSConnectInitialPduWithGCCConferenceCreateRequestPdu(clientName,
                                                                               clientBuild,
                                                                               clientDigProductId,
                                                                               encryptionMethod,
                                                                               channels,
                                                                               supportRDPTransportReliable,
                                                                               supportRDPTransportLossy,
                                                                               isMonitorDataPresent);
        }


        /// <summary>
        /// Create MCS Erect Domain Request PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// <returns>MCS Erect Domain Request PDU.</returns>
        public Client_MCS_Erect_Domain_Request CreateMCSErectDomainRequestPdu()
        {
            Client_MCS_Erect_Domain_Request erectPdu = new Client_MCS_Erect_Domain_Request(context);
            RdpbcgrUtility.FillTpktHeader(ref erectPdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref erectPdu.x224Data);
            erectPdu.subHeight = 1;
            erectPdu.subInterval = 1;
            return erectPdu;
        }


        /// <summary>
        /// Create MCS Attach User Request PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// <returns>MCS Attach User Request PDU.</returns>
        public Client_MCS_Attach_User_Request CreateMCSAttachUserRequestPdu()
        {
            Client_MCS_Attach_User_Request attachPdu = new Client_MCS_Attach_User_Request(context);
            RdpbcgrUtility.FillTpktHeader(ref attachPdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref attachPdu.x224Data);
            return attachPdu;
        }


        /// <summary>
        /// Create MCS Channel Join Request PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// <param name="channelId">The channel Id to be joined. This value can be got by getting property
        /// VirtualChannels.</param>
        /// <returns>MCS Channel Join Request PDU.</returns>
        public Client_MCS_Channel_Join_Request CreateMCSChannelJoinRequestPdu(long channelId)
        {
            Client_MCS_Channel_Join_Request joinPdu = new Client_MCS_Channel_Join_Request(context);
            RdpbcgrUtility.FillTpktHeader(ref joinPdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref joinPdu.x224Data);
            joinPdu.mcsChannelId = channelId;
            joinPdu.userChannelId = context.UserChannelId;
            return joinPdu;
        }


        /// <summary>
        /// Create Security Exchange PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// The length of securityExchangePduData will be calculated automatically when encode the PDU to bytes.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving MCS Connect Response PDU with GCC Conference Create Response.
        /// </summary>
        /// <param name="clientRandom">A 32-byte random value to generate session key. This argument can be null.
        /// If it is null, then the content field will be null.</param>
        /// <returns>Security Exchange PDU.</returns>
        public Client_Security_Exchange_Pdu CreateSecurityExchangePdu(byte[] clientRandom)
        {
            Client_Security_Exchange_Pdu exchangePdu = new Client_Security_Exchange_Pdu(context);
            RdpbcgrUtility.FillTpktHeader(ref exchangePdu.commonHeader.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref exchangePdu.commonHeader.x224Data);
            exchangePdu.commonHeader.securityHeader = new TS_SECURITY_HEADER();
            exchangePdu.commonHeader.securityHeader.flags = TS_SECURITY_HEADER_flags_Values.SEC_EXCHANGE_PKT
                                                          | TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_ENCRYPT_SC;
            exchangePdu.commonHeader.securityHeader.flagsHi = 0;
            exchangePdu.commonHeader.initiator = (ushort)context.UserChannelId;
            exchangePdu.commonHeader.channelId = (ushort)context.IOChannelId;
            exchangePdu.securityExchangePduData.length = 0;
            exchangePdu.securityExchangePduData.clientRandom = clientRandom;
            return exchangePdu;
        }


        /// <summary>
        /// Create Client Info PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving MCS Connect Response PDU with GCC Conference Create Response.
        /// </summary>
        /// <param name="flags">Option flags in TS_INFO_PACKET.</param>
        /// <param name="domain">The logon domain of the user. This argument can be null.
        /// If it is null, then the size field will be 0, the content field will be the same with TD.</param>
        /// <param name="userName">The logon user name of the user. This argument can be null.
        /// If it is null, then the size field will be 0, the content field will be the same with TD.</param>
        /// <param name="password">The logon password of the user. This argument can be null.
        /// If it is null, then the size field will be 0, the content field will be the same with TD.</param>
        /// <param name="clientAddress">The client IP address. This argument can be null.
        /// If it is null, then the size field will be 0, the content field will be the same with TD.</param>
        /// <param name="alternateShell">Variable-length path to the executable file of an alternate shell.
        /// This argument can be null. If it is null, then the size field will be 0, the content field will
        /// be the same with TD.</param>
        /// <param name="workingDir">Variable-length directory that contains the executable file specified in the 
        /// AlternateShell field or any related files (the length in bytes is given by the cbWorkingDir field).
        /// This argument can be null. If it is null, then the size field will be 0, the content field will
        /// be the same with TD.</param>
        /// <param name="isReconnect">Specify if this operation is auto-reconnection.</param>
        /// <returns>Client Info PDU.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public Client_Info_Pdu CreateClientInfoPdu(flags_Values flags,
                                                   string domain,
                                                   string userName,
                                                   string password,
                                                   string clientAddress,
                                                   string alternateShell,
                                                   string workingDir,
                                                   bool isReconnect)
        {
            Client_Info_Pdu clientInfoPdu = new Client_Info_Pdu(context);
            if ((context.RequestedProtocol & requestedProtocols_Values.PROTOCOL_SSL_FLAG)
                == requestedProtocols_Values.PROTOCOL_SSL_FLAG)
            {
                // in this situation, the PDU should have a basic security header 
                RdpbcgrUtility.FillTpktHeader(ref clientInfoPdu.commonHeader.tpktHeader, 0);
                RdpbcgrUtility.FillX224Data(ref clientInfoPdu.commonHeader.x224Data);

                clientInfoPdu.commonHeader.securityHeader = new TS_SECURITY_HEADER();
                clientInfoPdu.commonHeader.securityHeader.flags = TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT;
                clientInfoPdu.commonHeader.securityHeader.flagsHi = 0;
                clientInfoPdu.commonHeader.initiator = (ushort)context.UserChannelId;
                clientInfoPdu.commonHeader.channelId = (ushort)context.IOChannelId;
            }
            else
            {
                RdpbcgrUtility.FillCommonHeader(context, ref clientInfoPdu.commonHeader,
                                         TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT);
                // Support unencryption for CS messages
                if (clientInfoPdu.commonHeader.securityHeader == null)
                {
                    clientInfoPdu.commonHeader.securityHeader = new TS_SECURITY_HEADER();
                    clientInfoPdu.commonHeader.securityHeader.flags = TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT;
                    clientInfoPdu.commonHeader.securityHeader.flagsHi = 0;
                }
            }

            #region infoPacket
            clientInfoPdu.infoPacket = new TS_INFO_PACKET();
            clientInfoPdu.infoPacket.extraInfo = new TS_EXTENDED_INFO_PACKET();
            clientInfoPdu.infoPacket.CodePage = ConstValue.LOCALE_ENGLISH_UNITED_STATES;
            clientInfoPdu.infoPacket.flags = flags;

            if ((flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE)
            {
                UnicodeEncoding converter = new UnicodeEncoding();
                clientInfoPdu.infoPacket.cbDomain =
                    (ushort)converter.GetByteCount((domain == null) ? string.Empty : domain);
                clientInfoPdu.infoPacket.cbUserName =
                    (ushort)converter.GetByteCount((userName == null) ? string.Empty : userName);
                clientInfoPdu.infoPacket.cbPassword =
                    (ushort)converter.GetByteCount((password == null) ? string.Empty : password);
                clientInfoPdu.infoPacket.cbAlternateShell =
                    (ushort)converter.GetByteCount((alternateShell == null) ? string.Empty : alternateShell);
                clientInfoPdu.infoPacket.cbWorkingDir =
                    (ushort)converter.GetByteCount((workingDir == null) ? string.Empty : workingDir);

                // add null-terminator size for these fields
                clientInfoPdu.infoPacket.extraInfo.cbClientAddress =
                    (ushort)(converter.GetByteCount((clientAddress == null) ? string.Empty : clientAddress) + 2);
                clientInfoPdu.infoPacket.extraInfo.cbClientDir = (ushort)(converter.GetByteCount(string.Empty) + 2);
            }
            else
            {
                ASCIIEncoding converter = new ASCIIEncoding();
                clientInfoPdu.infoPacket.cbDomain =
                    (ushort)converter.GetByteCount((domain == null) ? string.Empty : domain);
                clientInfoPdu.infoPacket.cbUserName =
                    (ushort)converter.GetByteCount((userName == null) ? string.Empty : userName);
                clientInfoPdu.infoPacket.cbPassword =
                    (ushort)converter.GetByteCount((password == null) ? string.Empty : password);
                clientInfoPdu.infoPacket.cbAlternateShell =
                    (ushort)converter.GetByteCount((alternateShell == null) ? string.Empty : alternateShell);
                clientInfoPdu.infoPacket.cbWorkingDir =
                    (ushort)converter.GetByteCount((workingDir == null) ? string.Empty : workingDir);

                // add null-terminator size for these fields
                clientInfoPdu.infoPacket.extraInfo.cbClientAddress =
                    (ushort)(converter.GetByteCount((clientAddress == null) ? string.Empty : clientAddress) + 1);
                clientInfoPdu.infoPacket.extraInfo.cbClientDir = (ushort)(converter.GetByteCount(string.Empty) + 1);
            }

            clientInfoPdu.infoPacket.Domain = domain;
            clientInfoPdu.infoPacket.UserName = userName;
            clientInfoPdu.infoPacket.Password = password;
            clientInfoPdu.infoPacket.AlternateShell = alternateShell;
            clientInfoPdu.infoPacket.WorkingDir = workingDir;

            #region extraInfo
            clientInfoPdu.infoPacket.extraInfo.clientAddressFamily = clientAddressFamily_Values.V1;
            clientInfoPdu.infoPacket.extraInfo.clientAddress = clientAddress;
            clientInfoPdu.infoPacket.extraInfo.clientDir = string.Empty;

            #region time field
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.Bias = ConstValue.TIME_ZONE_BIAS_DEFAULT;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardName =
                ConstValue.TIME_ZONE_STANDARD_NAME_DEFAULT;

            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardDate.wYear = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardDate.wMonth = wMonth_Values.V10;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardDate.wDayOfWeek = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardDate.wDay = wDay_Values.V5;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardDate.wHour = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardDate.wMinute = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardDate.wSecond = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardDate.wMilliseconds = 0;

            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.StandardBias = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightName =
                ConstValue.TIME_ZONE_DAYLIGHT_NAME_DEFAULT;

            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightDate.wYear = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightDate.wMonth = wMonth_Values.V4;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightDate.wDayOfWeek = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightDate.wDay = wDay_Values.V1;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightDate.wHour = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightDate.wMinute = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightDate.wSecond = 0;
            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightDate.wMilliseconds = 0;

            clientInfoPdu.infoPacket.extraInfo.clientTimeZone.DaylightBias =
                ConstValue.TIME_ZONE_DAYLIGHT_BIAS_DEFAULT;
            #endregion time field

            clientInfoPdu.infoPacket.extraInfo.clientSessionId = 0;
            clientInfoPdu.infoPacket.extraInfo.performanceFlags =
                performanceFlags_Values.PERF_DISABLE_FULLWINDOWDRAG | performanceFlags_Values.PERF_DISABLE_THEMING | performanceFlags_Values.PERF_ENABLE_DESKTOP_COMPOSITION;

            if (!isReconnect)   // if this connection is not reconnect, then set the cookie to null
            {
                clientInfoPdu.infoPacket.extraInfo.cbAutoReconnectLen = 0;
                clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie = null;
            }
            else                // this connection is reconnect, then use the cookie to do authentication
            {
                clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie = new ARC_CS_PRIVATE_PACKET();
                clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.cbLen =
                    ARC_CS_PRIVATE_PACKET_cbLen_Values.V1;
                clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.Version =
                    ARC_CS_PRIVATE_PACKET_Version_Values.AUTO_RECONNECT_VERSION_1;
                clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.LogonId = context.LogonId;

                byte[] verifier = new byte[1];
                if (context.ArcRandomBits != null)
                {
                    // Suppress "CA5350:MD5CannotBeUsed" message since MD5 is used according protocol definition of MS-RDPBCGR 
                    HMACMD5 hmac = new HMACMD5(context.ArcRandomBits);

                    byte[] clientRandom = null;
                    if (context.RequestedProtocol != requestedProtocols_Values.PROTOCOL_RDP_FLAG)
                    {
                        clientRandom = new byte[ConstValue.RECONNECT_CLIENT_RANDOM_LENGTH];
                    }
                    else
                    {
                        clientRandom = context.ClientRandomNumber;
                    }

                    if (clientRandom != null)
                    {
                        verifier = hmac.ComputeHash(clientRandom);
                    }
                }
                clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.SecurityVerifier = verifier;
                clientInfoPdu.infoPacket.extraInfo.cbAutoReconnectLen =
                    (ushort)(Marshal.SizeOf((uint)clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.cbLen)
                    + Marshal.SizeOf((uint)clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.Version)
                    + Marshal.SizeOf(clientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.LogonId)
                    + verifier.Length);
            }

            clientInfoPdu.infoPacket.extraInfo.reserved1 = new UInt16Class(0);
            clientInfoPdu.infoPacket.extraInfo.reserved2 = new UInt16Class(0);
            #endregion extraInfo
            #endregion infoPacket

            context.IsWaitingLicenseErrorPdu = true;

            return clientInfoPdu;
        }


        /// <summary>
        /// Create Confirm Active PDU with given capability sets.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <param name="capabilitySets">The capability sets want to send. 
        /// This argument could be got from method CreateCapabilitySets.
        /// This argument can be null. If it is null, then no capability set will be specified.</param>
        /// <returns>Confirm Active PDU.</returns>
        public Client_Confirm_Active_Pdu CreateConfirmActivePdu(Collection<ITsCapsSet> capabilitySets)
        {
            Client_Confirm_Active_Pdu confirmActivePdu = new Client_Confirm_Active_Pdu(context);
            RdpbcgrUtility.FillCommonHeader(context, ref confirmActivePdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            TS_CONFIRM_ACTIVE_PDU confirmActivePduData = new TS_CONFIRM_ACTIVE_PDU();
            if (capabilitySets == null)
            {
                confirmActivePduData.capabilitySets = new Collection<ITsCapsSet>();
            }
            else
            {
                confirmActivePduData.capabilitySets = capabilitySets;
            }

            int capabilitiesLength = 0;
            foreach (ITsCapsSet capability in confirmActivePduData.capabilitySets)
            {
                capabilitiesLength += capability.ToBytes().Length;
            }

            confirmActivePduData.lengthCombinedCapabilities = (ushort)(sizeof(ushort)
                                                            + sizeof(ushort)
                                                            + capabilitiesLength);
            confirmActivePduData.shareId = context.ShareId;
            confirmActivePduData.originatorId = originatorId_Values.V1;
            confirmActivePduData.lengthSourceDescriptor = (ushort)ConstValue.CONFIRM_SOURCE_DESCRIPTOR.Length;
            confirmActivePduData.sourceDescriptor = ConstValue.CONFIRM_SOURCE_DESCRIPTOR;
            confirmActivePduData.numberCapabilities = (ushort)confirmActivePduData.capabilitySets.Count;
            confirmActivePduData.pad2Octets = 0;

            int totalLength = Marshal.SizeOf(confirmActivePduData.shareControlHeader)
                            + sizeof(uint)
                            + sizeof(ushort)
                            + sizeof(ushort)
                            + sizeof(ushort)
                            + confirmActivePduData.sourceDescriptor.Length
                            + confirmActivePduData.lengthCombinedCapabilities;
            RdpbcgrUtility.FillShareControlHeader(ref confirmActivePduData.shareControlHeader,
                                           (ushort)totalLength,
                                           ShareControlHeaderType.PDUTYPE_CONFIRMACTIVEPDU,
                                           (ushort)context.UserChannelId);

            confirmActivePdu.confirmActivePduData = confirmActivePduData;
            return confirmActivePdu;
        }


        /// <summary>
        /// Create Confirm Active PDU with the default capability sets.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Confirm Active PDU.</returns>
        public Client_Confirm_Active_Pdu CreateConfirmActivePdu()
        {
            Collection<ITsCapsSet> capabilitySets = CreateCapabilitySets();
            return CreateConfirmActivePdu(capabilitySets);
        }


        /// <summary>
        /// Create Synchronize PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Synchronize PDU.</returns>
        public Client_Synchronize_Pdu CreateSynchronizePdu()
        {
            Client_Synchronize_Pdu synchronizePdu = new Client_Synchronize_Pdu(context);
            RdpbcgrUtility.FillCommonHeader(context, ref synchronizePdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            TS_SYNCHRONIZE_PDU synchronizePduData = new TS_SYNCHRONIZE_PDU();
            synchronizePduData.messageType = messageType_Values.V1;
            synchronizePduData.targetUser = (ushort)context.ServerChannelId;
            RdpbcgrUtility.FillShareDataHeader(ref synchronizePduData.shareDataHeader,
                (ushort)(Marshal.SizeOf(synchronizePduData) - Marshal.SizeOf(synchronizePduData.shareDataHeader)),
                context,
                streamId_Values.STREAM_LOW,
                pduType2_Values.PDUTYPE2_SYNCHRONIZE,
                0,
                0);

            synchronizePdu.synchronizePduData = synchronizePduData;
            return synchronizePdu;
        }


        /// <summary>
        /// Create Control PDU Cooperate.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Control PDU Cooperate.</returns>
        public Client_Control_Pdu_Cooperate CreateControlCooperatePdu()
        {
            Client_Control_Pdu_Cooperate controlCooperatePdu = new Client_Control_Pdu_Cooperate(context);
            RdpbcgrUtility.FillCommonHeader(context, ref controlCooperatePdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            TS_CONTROL_PDU controlPduData = new TS_CONTROL_PDU();
            controlPduData.action = action_Values.CTRLACTION_COOPERATE;
            controlPduData.grantId = 0;
            controlPduData.controlId = 0;
            RdpbcgrUtility.FillShareDataHeader(ref controlPduData.shareDataHeader,
                (ushort)(Marshal.SizeOf(controlPduData) - Marshal.SizeOf(controlPduData.shareDataHeader)),
                context,
                streamId_Values.STREAM_LOW,
                pduType2_Values.PDUTYPE2_CONTROL,
                0,
                0);

            controlCooperatePdu.controlPduData = controlPduData;
            return controlCooperatePdu;
        }


        /// <summary>
        /// Create Request Control PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Request Control PDU.</returns>
        public Client_Control_Pdu_Request_Control CreateControlRequestPdu()
        {
            Client_Control_Pdu_Request_Control requestControlPdu = new Client_Control_Pdu_Request_Control(context);
            RdpbcgrUtility.FillCommonHeader(context, ref requestControlPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            TS_CONTROL_PDU controlPduData = new TS_CONTROL_PDU();
            controlPduData.action = action_Values.CTRLACTION_REQUEST_CONTROL;
            controlPduData.grantId = 0;
            controlPduData.controlId = 0;
            RdpbcgrUtility.FillShareDataHeader(ref controlPduData.shareDataHeader,
                (ushort)(Marshal.SizeOf(controlPduData) - Marshal.SizeOf(controlPduData.shareDataHeader)),
                context,
                streamId_Values.STREAM_LOW,
                pduType2_Values.PDUTYPE2_CONTROL,
                0,
                0);

            requestControlPdu.controlPduData = controlPduData;
            return requestControlPdu;
        }


        /// <summary>
        /// Create Persistent Key List PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Persistent Key List PDU.</returns>
        public Client_Persistent_Key_List_Pdu CreatePersistentKeyListPdu()
        {
            Client_Persistent_Key_List_Pdu persistentKeyListPdu = new Client_Persistent_Key_List_Pdu(context);
            RdpbcgrUtility.FillCommonHeader(context, ref persistentKeyListPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            TS_BITMAPCACHE_PERSISTENT_LIST_PDU persistentKeyListPduData = new TS_BITMAPCACHE_PERSISTENT_LIST_PDU();
            persistentKeyListPduData.numEntriesCache0 = 0;
            persistentKeyListPduData.numEntriesCache1 = 0;
            persistentKeyListPduData.numEntriesCache2 = (ushort)ConstValue.PERSISTENT_KEY_LIST_ENTRY_DEFAULT.Length;
            persistentKeyListPduData.numEntriesCache3 = 0;
            persistentKeyListPduData.numEntriesCache4 = 0;

            persistentKeyListPduData.totalEntriesCache0 = 0;
            persistentKeyListPduData.totalEntriesCache1 = 0;
            persistentKeyListPduData.totalEntriesCache2 = (ushort)ConstValue.PERSISTENT_KEY_LIST_ENTRY_DEFAULT.Length;
            persistentKeyListPduData.totalEntriesCache3 = 0;
            persistentKeyListPduData.totalEntriesCache4 = 0;

            persistentKeyListPduData.bBitMask = bBitMask_Values.PERSIST_FIRST_PDU | bBitMask_Values.PERSIST_LAST_PDU;
            persistentKeyListPduData.Pad2 = 0;
            persistentKeyListPduData.Pad3 = 0;
            persistentKeyListPduData.entries = new List<TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY>();
            persistentKeyListPduData.entries.AddRange(ConstValue.PERSISTENT_KEY_LIST_ENTRY_DEFAULT);

            int persistentKeyPacketLength = Marshal.SizeOf(persistentKeyListPduData.numEntriesCache0)
                                          + Marshal.SizeOf(persistentKeyListPduData.numEntriesCache1)
                                          + Marshal.SizeOf(persistentKeyListPduData.numEntriesCache2)
                                          + Marshal.SizeOf(persistentKeyListPduData.numEntriesCache3)
                                          + Marshal.SizeOf(persistentKeyListPduData.numEntriesCache4)
                                          + Marshal.SizeOf(persistentKeyListPduData.totalEntriesCache0)
                                          + Marshal.SizeOf(persistentKeyListPduData.totalEntriesCache1)
                                          + Marshal.SizeOf(persistentKeyListPduData.totalEntriesCache2)
                                          + Marshal.SizeOf(persistentKeyListPduData.totalEntriesCache3)
                                          + Marshal.SizeOf(persistentKeyListPduData.totalEntriesCache4)
                                          + sizeof(byte)
                                          + Marshal.SizeOf(persistentKeyListPduData.Pad2)
                                          + Marshal.SizeOf(persistentKeyListPduData.Pad3)
                                          + persistentKeyListPduData.numEntriesCache0
                                          * Marshal.SizeOf(ConstValue.PERSISTENT_KEY_LIST_ENTRY_DEFAULT[0])
                                          + persistentKeyListPduData.numEntriesCache1
                                          * Marshal.SizeOf(ConstValue.PERSISTENT_KEY_LIST_ENTRY_DEFAULT[0])
                                          + persistentKeyListPduData.numEntriesCache2
                                          * Marshal.SizeOf(ConstValue.PERSISTENT_KEY_LIST_ENTRY_DEFAULT[0])
                                          + persistentKeyListPduData.numEntriesCache3
                                          * Marshal.SizeOf(ConstValue.PERSISTENT_KEY_LIST_ENTRY_DEFAULT[0])
                                          + persistentKeyListPduData.numEntriesCache4
                                          * Marshal.SizeOf(ConstValue.PERSISTENT_KEY_LIST_ENTRY_DEFAULT[0]);

            RdpbcgrUtility.FillShareDataHeader(ref persistentKeyListPduData.shareDataHeader,
                (ushort)persistentKeyPacketLength,
                context,
                streamId_Values.STREAM_LOW,
                pduType2_Values.PDUTYPE2_BITMAPCACHE_PERSISTENT_LIST,
                0,
                0);

            persistentKeyListPdu.persistentKeyListPduData = persistentKeyListPduData;
            return persistentKeyListPdu;
        }


        /// <summary>
        /// Create Font List PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Font List PDU.</returns>
        public Client_Font_List_Pdu CreateFontListPdu()
        {
            Client_Font_List_Pdu fontListPdu = new Client_Font_List_Pdu(context);
            RdpbcgrUtility.FillCommonHeader(context, ref fontListPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            TS_FONT_LIST_PDU fontListPduData = new TS_FONT_LIST_PDU();
            fontListPduData.numberFonts = 0;
            fontListPduData.totalNumFonts = 0;
            fontListPduData.listFlags = ConstValue.FONTLIST_FIRST | ConstValue.FONTLIST_LAST;
            fontListPduData.entrySize = ConstValue.FONTLIST_ENTRY_SIZE;

            int clientFontListPacketLength = Marshal.SizeOf(fontListPduData.numberFonts)
                                             + Marshal.SizeOf(fontListPduData.totalNumFonts)
                                             + Marshal.SizeOf(fontListPduData.listFlags)
                                             + Marshal.SizeOf(fontListPduData.entrySize);

            RdpbcgrUtility.FillShareDataHeader(ref fontListPduData.shareDataHeader,
                (ushort)clientFontListPacketLength,
                context,
                streamId_Values.STREAM_LOW,
                pduType2_Values.PDUTYPE2_FONTLIST,
                0,
                0);

            fontListPdu.fontListPduData = fontListPduData;
            return fontListPdu;
        }
        #endregion connection


        #region disconnection
        /// <summary>
        /// Create Shutdown Request PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <returns>Shutdown Request PDU.</returns>
        public Client_Shutdown_Request_Pdu CreateShutdownRequestPdu()
        {
            Client_Shutdown_Request_Pdu ShutdownRequestPdu = new Client_Shutdown_Request_Pdu(context);
            TS_SHUTDOWN_REQ_PDU ShutdownRequestData = new TS_SHUTDOWN_REQ_PDU();

            RdpbcgrUtility.FillCommonHeader(context, ref ShutdownRequestPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            RdpbcgrUtility.FillShareDataHeader(ref ShutdownRequestData.shareDataHeader,
                                        0,
                                        context,
                                        streamId_Values.STREAM_LOW,
                                        pduType2_Values.PDUTYPE2_SHUTDOWN_REQUEST,
                                        0,
                                        0);
            ShutdownRequestPdu.shutdownRequestPduData = ShutdownRequestData;

            return ShutdownRequestPdu;
        }


        /// <summary>
        /// Create MCS Disconnect Provider Ultimatum PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <param name="reason">The reason to be disconnected.</param>
        /// <returns>MCS Disconnect Provider Ultimatum PDU.</returns>
        public MCS_Disconnect_Provider_Ultimatum_Pdu CreateMCSDisconnectProviderUltimatumPdu(int reason)
        {
            MCS_Disconnect_Provider_Ultimatum_Pdu mcsDisconnectProviderUltimatumPdu =
              new MCS_Disconnect_Provider_Ultimatum_Pdu(context);
            mcsDisconnectProviderUltimatumPdu.disconnectProvider =
                new DisconnectProviderUltimatum(new Reason(reason));
            RdpbcgrUtility.FillTpktHeader(ref mcsDisconnectProviderUltimatumPdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref mcsDisconnectProviderUltimatumPdu.x224Data);

            return mcsDisconnectProviderUltimatumPdu;
        }
        #endregion disconnection


        #region Keyboard and Mouse Input
        /// <summary>
        /// Create Slow Path InputEvent PDU. This PDU includes 5 event, 
        /// INPUT_EVENT_SYNC, INPUT_EVENT_SCANCODE, INPUT_EVENT_UNICODE, INPUT_EVENT_MOUSE and
        /// INPUT_EVENT_MOUSEX for each.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <returns>Slow Path InputEvent PDU.</returns>
        public TS_INPUT_PDU CreateSlowPathInputEventPDU()
        {
            TS_INPUT_PDU slowpathInputPdu = new TS_INPUT_PDU(context);
            RdpbcgrUtility.FillCommonHeader(context, ref slowpathInputPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            slowpathInputPdu.slowPathInputEvents = new Collection<TS_INPUT_EVENT>();
            slowpathInputPdu.numberEvents = ConstValue.NUMBER_EVENTS;
            slowpathInputPdu.pad2Octets = 0;

            #region Fill in slowPathInputEvents field
            int eventFieldLength = Marshal.SizeOf(slowpathInputPdu.numberEvents)
                                 + Marshal.SizeOf(slowpathInputPdu.pad2Octets);

            TS_INPUT_EVENT inputEvent = new TS_INPUT_EVENT();
            int inputEventheaderlength = Marshal.SizeOf(inputEvent.eventTime)
                                       + Marshal.SizeOf((ushort)inputEvent.messageType);

            TS_KEYBOARD_EVENT keyboardEvent = new TS_KEYBOARD_EVENT();
            keyboardEvent.keyboardFlags = keyboardFlags_Values.KBDFLAGS_RELEASE;
            keyboardEvent.keyCode = ConstValue.KEY_CODE;
            keyboardEvent.pad2Octets = 0;
            inputEvent.eventTime = 0;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SCANCODE;
            inputEvent.slowPathInputData = keyboardEvent;
            slowpathInputPdu.slowPathInputEvents.Add(inputEvent);
            eventFieldLength += (inputEventheaderlength + Marshal.SizeOf(keyboardEvent));

            TS_SYNC_EVENT synchronizeEvent = new TS_SYNC_EVENT();
            synchronizeEvent.pad2Octets = 0;
            synchronizeEvent.toggleFlags = toggleFlags_Values.TS_SYNC_NUM_LOCK;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SYNC;
            inputEvent.slowPathInputData = synchronizeEvent;
            slowpathInputPdu.slowPathInputEvents.Add(inputEvent);
            eventFieldLength += (inputEventheaderlength + Marshal.SizeOf(synchronizeEvent));

            TS_UNICODE_KEYBOARD_EVENT unicodeEvent = new TS_UNICODE_KEYBOARD_EVENT();
            unicodeEvent.keyboardFlags = keyboardFlags_Values.None;
            unicodeEvent.unicodeCode = ConstValue.UNICODE_CODE;
            unicodeEvent.pad2Octets = 0;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNICODE;
            inputEvent.slowPathInputData = unicodeEvent;
            slowpathInputPdu.slowPathInputEvents.Add(inputEvent);
            eventFieldLength += (inputEventheaderlength + Marshal.SizeOf(unicodeEvent));

            TS_POINTER_EVENT mouseEvent = new TS_POINTER_EVENT();
            mouseEvent.pointerFlags = pointerFlags_Values.PTRFLAGS_WHEEL;
            mouseEvent.xPos = ConstValue.X_POS;
            mouseEvent.yPos = ConstValue.Y_POS;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSE;
            inputEvent.slowPathInputData = mouseEvent;
            slowpathInputPdu.slowPathInputEvents.Add(inputEvent);
            eventFieldLength += (inputEventheaderlength + Marshal.SizeOf(mouseEvent));

            TS_POINTERX_EVENT exmouseEvent = new TS_POINTERX_EVENT();
            exmouseEvent.pointerFlags = TS_POINTERX_EVENT_pointerFlags_Values.PTRXFLAGS_BUTTON1;
            exmouseEvent.xPos = ConstValue.X_POS;
            exmouseEvent.yPos = ConstValue.Y_POS;
            inputEvent.messageType = TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSEX;
            inputEvent.slowPathInputData = exmouseEvent;
            slowpathInputPdu.slowPathInputEvents.Add(inputEvent);
            eventFieldLength += (inputEventheaderlength + Marshal.SizeOf(exmouseEvent));
            #endregion Fill in slowPathInputEvents field

            RdpbcgrUtility.FillShareDataHeader(ref slowpathInputPdu.shareDataHeader,
                                        (ushort)eventFieldLength,
                                        context,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_INPUT,
                                        0,
                                        0);

            return slowpathInputPdu;
        }

        /// <summary>
        /// Create Slow Path InputEvent PDU. 
        /// </summary>
        /// <param name="inputEvents">Array of input events</param>
        /// <returns>Slow Path InputEvent PDU.</returns>
        public TS_INPUT_PDU CreateSlowPathInputEventPDU(TS_INPUT_EVENT[] inputEvents)
        {
            if (inputEvents == null || inputEvents.Length == 0)
            {
                return CreateSlowPathInputEventPDU();
            }

            TS_INPUT_PDU slowpathInputPdu = new TS_INPUT_PDU(context);
            RdpbcgrUtility.FillCommonHeader(context, ref slowpathInputPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            slowpathInputPdu.slowPathInputEvents = new Collection<TS_INPUT_EVENT>();
            slowpathInputPdu.numberEvents = (ushort)inputEvents.Length;
            slowpathInputPdu.pad2Octets = 0;

            int eventFieldLength = Marshal.SizeOf(slowpathInputPdu.numberEvents)
                                 + Marshal.SizeOf(slowpathInputPdu.pad2Octets);
            foreach (TS_INPUT_EVENT inputEvent in inputEvents)
            {
                slowpathInputPdu.slowPathInputEvents.Add(inputEvent);
                eventFieldLength += (Marshal.SizeOf(inputEvent.eventTime) + Marshal.SizeOf((ushort)inputEvent.messageType) + Marshal.SizeOf(inputEvent.slowPathInputData));
            }

            RdpbcgrUtility.FillShareDataHeader(ref slowpathInputPdu.shareDataHeader,
                                        (ushort)eventFieldLength,
                                        context,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_INPUT,
                                        0,
                                        0);

            return slowpathInputPdu;
        }

        /// <summary>
        /// Create Fast Path Input Event PDU. This PDU includes 6 event, 
        /// TS_FP_KEYBOARD_EVENT, TS_FP_UNICODE_KEYBOARD_EVENT, TS_FP_POINTER_EVENT,
        /// TS_FP_POINTERX_EVENT, TS_FP_SYNC_EVENT and TS_FP_QOETIMESTAMP_EVENT for each.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length1 and length2 of TS_FP_INPUT_PDU and padlen of fipsInformation 0,
        /// and they will be calculated automatically when encode the PDU to bytes.
        /// If the they are not 0, then they will keep the value user set, and will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <returns>Fast Path Input Event PDU.</returns>
        public TS_FP_INPUT_PDU CreateFastPathInputEventPDU()
        {
            TS_FP_INPUT_PDU fastpathInputPdu = new TS_FP_INPUT_PDU(context);
            fastpathInputPdu.fpInputEvents = new Collection<TS_FP_INPUT_EVENT>();
            TS_FP_INPUT_EVENT fpInputEvent = new TS_FP_INPUT_EVENT();

            fastpathInputPdu.fpInputHeader = new nested_TS_FP_INPUT_PDU_fpInputHeader(actionCode_Values.FASTPATH_INPUT_ACTION_FASTPATH, ConstValue.FP_NUMBER_EVENTS, encryptionFlags_Values.None);

            if (context.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                fastpathInputPdu.fpInputHeader.flags |= encryptionFlags_Values.FASTPATH_INPUT_ENCRYPTED;
            }

            fastpathInputPdu.dataSignature = null;
            fastpathInputPdu.length1 = 0;
            fastpathInputPdu.length2 = 0;
            fastpathInputPdu.numberEvents = ConstValue.FP_NUMBER_EVENTS;

            if (context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                fastpathInputPdu.fipsInformation.length = TS_FP_FIPS_INFO_length_Values.V1;
                fastpathInputPdu.fipsInformation.version = ConstValue.TSFIPS_VERSION1;
                fastpathInputPdu.fipsInformation.padlen = 0;
            }

            #region Fill in TS_FP_INPUT_EVENT
            // All the events below have a field eventFlagsAndCode which consist of eventFlags and eventCode.
            // eventFlags (5 bits): lower 5 bits. The flags specific to the input event.
            // eventCode (3 bits): higher 3 bits. The type code of the input event.
            TS_FP_SYNC_EVENT synchronizeEvent = new TS_FP_SYNC_EVENT();
            fpInputEvent.eventHeader.eventFlagsAndCode = (byte)(((int)eventCode_Values.FASTPATH_INPUT_EVENT_SYNC << 5)
                | ((int)TS_FP_SYNC_EVENT_Eventflags.FASTPATH_INPUT_SYNC_NUM_LOCK & 0x1F));
            fpInputEvent.eventData = synchronizeEvent;
            fastpathInputPdu.fpInputEvents.Add(fpInputEvent);

            TS_FP_KEYBOARD_EVENT keyboardEvent = new TS_FP_KEYBOARD_EVENT();
            keyboardEvent.keyCode = ConstValue.FP_KEY_CODE;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)(((int)eventCode_Values.FASTPATH_INPUT_EVENT_SCANCODE << 5)
                | ((int)TS_FP_KEYBOARD_EVENT_Eventflags.FASTPATH_INPUT_KBDFLAGS_EXTENDED & 0x1F));
            fpInputEvent.eventData = keyboardEvent;
            fastpathInputPdu.fpInputEvents.Add(fpInputEvent);

            TS_FP_UNICODE_KEYBOARD_EVENT unicodeEvent = new TS_FP_UNICODE_KEYBOARD_EVENT();
            unicodeEvent.unicodeCode = ConstValue.FP_UNICODE_CODE;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)((int)eventCode_Values.FASTPATH_INPUT_EVENT_UNICODE << 5);
            fpInputEvent.eventData = unicodeEvent;
            fastpathInputPdu.fpInputEvents.Add(fpInputEvent);

            TS_FP_POINTER_EVENT mouseEvent = new TS_FP_POINTER_EVENT();
            mouseEvent.pointerFlags = (ushort)pointerFlags_Values.PTRFLAGS_WHEEL;
            mouseEvent.xPos = (ushort)ConstValue.X_POS;
            mouseEvent.yPos = (ushort)ConstValue.Y_POS;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)((int)eventCode_Values.FASTPATH_INPUT_EVENT_MOUSE << 5);
            fpInputEvent.eventData = mouseEvent;
            fastpathInputPdu.fpInputEvents.Add(fpInputEvent);

            TS_FP_POINTERX_EVENT extendedMouseEvent = new TS_FP_POINTERX_EVENT();
            extendedMouseEvent.pointerFlags = (ushort)TS_POINTERX_EVENT_pointerFlags_Values.PTRXFLAGS_BUTTON1;
            extendedMouseEvent.xPos = (ushort)ConstValue.X_POS;
            extendedMouseEvent.yPos = (ushort)ConstValue.Y_POS;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)((int)eventCode_Values.FASTPATH_INPUT_EVENT_MOUSEX << 5);
            fpInputEvent.eventData = extendedMouseEvent;
            fastpathInputPdu.fpInputEvents.Add(fpInputEvent);

            TS_FP_QOETIMESTAMP_EVENT qoeTimestampEvent = new TS_FP_QOETIMESTAMP_EVENT();
            qoeTimestampEvent.timestamp = (uint)DateTime.Now.Millisecond;
            fpInputEvent.eventHeader.eventFlagsAndCode =
                (byte)((int)eventCode_Values.FASTPATH_INPUT_EVENT_QOE_TIMESTAMP << 5);
            fpInputEvent.eventData = qoeTimestampEvent;
            fastpathInputPdu.fpInputEvents.Add(fpInputEvent);
            #endregion Fill in TS_FP_INPUT_EVENT

            return fastpathInputPdu;
        }

        /// <summary>
        /// Create Fast Path Input Event PDU. 
        /// </summary>
        /// <param name="inputEvents">Input Event</param>
        /// <returns>Fast Path Input Event PDU.</returns>
        public TS_FP_INPUT_PDU CreateFastPathInputEventPDU(TS_FP_INPUT_EVENT[] inputEvents)
        {
            if (inputEvents == null || inputEvents.Length == 0)
            {
                return CreateFastPathInputEventPDU();
            }
            TS_FP_INPUT_PDU fastpathInputPdu = new TS_FP_INPUT_PDU(context);
            fastpathInputPdu.fpInputEvents = new Collection<TS_FP_INPUT_EVENT>();
            byte eventNum = 0;
            if (inputEvents.Length <= 15)
            {
                eventNum = (byte)inputEvents.Length;
            }

            fastpathInputPdu.fpInputHeader = new nested_TS_FP_INPUT_PDU_fpInputHeader(actionCode_Values.FASTPATH_INPUT_ACTION_FASTPATH, eventNum, encryptionFlags_Values.None);

            if (context.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                fastpathInputPdu.fpInputHeader.flags |= encryptionFlags_Values.FASTPATH_INPUT_ENCRYPTED;
            }

            fastpathInputPdu.dataSignature = null;
            // length1 and length2 will be filled when encoding
            fastpathInputPdu.length1 = 0;
            fastpathInputPdu.length2 = 0;
            fastpathInputPdu.numberEvents = (byte)inputEvents.Length;

            if (context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                fastpathInputPdu.fipsInformation.length = TS_FP_FIPS_INFO_length_Values.V1;
                fastpathInputPdu.fipsInformation.version = ConstValue.TSFIPS_VERSION1;
                fastpathInputPdu.fipsInformation.padlen = 0;
            }

            foreach (TS_FP_INPUT_EVENT input in inputEvents)
            {
                fastpathInputPdu.fpInputEvents.Add(input);
            }

            return fastpathInputPdu;
        }
        #endregion Keyboard and Mouse Input


        #region Controlling Server Graphics Output
        /// <summary>
        /// Create Refresh Rect PDU. This PDU includes some Rectangles by default.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <returns>Refresh Rect PDU.</returns>
        public Client_Refresh_Rect_Pdu CreateRefreshRectPdu(TS_RECTANGLE16[] rects = null)
        {
            if (rects == null || rects.Length == 0)
            {
                rects = ConstValue.AREASTOFRESH_DEFAULT;
            }
            Client_Refresh_Rect_Pdu refreshRectPdu = new Client_Refresh_Rect_Pdu(context);
            TS_REFRESH_RECT_PDU refreshRectPduData = new TS_REFRESH_RECT_PDU();

            RdpbcgrUtility.FillCommonHeader(context, ref refreshRectPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            refreshRectPduData.pad3Octects = ConstValue.PAD_3_OCTECTS;
            refreshRectPduData.numberOfAreas = (byte)rects.Length;
            refreshRectPduData.areasToRefresh = new Collection<TS_RECTANGLE16>();

            foreach (TS_RECTANGLE16 rect in rects)
            {
                refreshRectPduData.areasToRefresh.Add(rect);
            }

            int refreshRectPacketLength = Marshal.SizeOf(refreshRectPduData.numberOfAreas)
                                          + refreshRectPduData.pad3Octects.Length
                                          + refreshRectPduData.numberOfAreas
                                          * Marshal.SizeOf(refreshRectPduData.areasToRefresh[0]);

            RdpbcgrUtility.FillShareDataHeader(ref refreshRectPduData.shareDataHeader,
                                        (ushort)refreshRectPacketLength,
                                        context,
                                        streamId_Values.STREAM_LOW,
                                        pduType2_Values.PDUTYPE2_REFRESH_RECT,
                                        0,
                                        0);
            refreshRectPdu.refreshRectPduData = refreshRectPduData;

            return refreshRectPdu;
        }


        /// <summary>
        /// Create Suppress Output PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <param name="allowDisplayUpdates">Indicates whether the client wants to receive display updates 
        /// from the server.</param>
        /// <param name="rect">An Inclusive Rectangle (section 2.2.11.1) which contains the coordinates of the desktop rectangle.</param>
        /// <returns>Suppress Output PDU.</returns>
        public Client_Suppress_Output_Pdu CreateSuppressOutputPdu(
            AllowDisplayUpdates_SUPPRESS_OUTPUT allowDisplayUpdates, TS_RECTANGLE16? rect = null)
        {
            if (rect == null)
            {
                rect = ConstValue.DESKTOP_RECT_DEFAULT;
            }
            Client_Suppress_Output_Pdu suppressOutputPdu = new Client_Suppress_Output_Pdu(this.context);
            TS_SUPPRESS_OUTPUT_PDU suppressOutputPduData = new TS_SUPPRESS_OUTPUT_PDU();

            RdpbcgrUtility.FillCommonHeader(context, ref suppressOutputPdu.commonHeader,
                                    TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                    | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            suppressOutputPduData.allowDisplayUpdates = allowDisplayUpdates;
            suppressOutputPduData.pad3Octects = ConstValue.PAD_3_OCTECTS;

            int suppressOutputPacketLength = Marshal.SizeOf((byte)suppressOutputPduData.allowDisplayUpdates)
                                           + suppressOutputPduData.pad3Octects.Length;

            if (suppressOutputPduData.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.ALLOW_DISPLAY_UPDATES)
            {
                suppressOutputPduData.desktopRect = rect.Value;
                suppressOutputPacketLength += Marshal.SizeOf(suppressOutputPduData.desktopRect);
            }

            RdpbcgrUtility.FillShareDataHeader(ref suppressOutputPduData.shareDataHeader,
                                        (ushort)suppressOutputPacketLength,
                                        context,
                                        streamId_Values.STREAM_LOW,
                                        pduType2_Values.PDUTYPE2_SUPPRESS_OUTPUT,
                                        0,
                                        0);

            suppressOutputPdu.suppressOutputPduData = suppressOutputPduData;

            return suppressOutputPdu;
        }
        #endregion Controlling Server Graphics Output


        #region Virtual Channel
        /// <summary>
        /// Create Virtual Channel Raw PDU. The argument virtualChannelData is considered as 
        /// having been processed beyond this method, so it will not be processed again.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <param name="channelId">The channel Id of the virtual channel.</param>
        /// <param name="length">The total length in bytes of the uncompressed channel data.</param>
        /// <param name="flags">The flags of the virtual channel to indicate some compression feature.</param>
        /// <param name="virtualChannelData">The binary data to be send as virtual data. This argument can be null.
        /// If it is null, then the content field will be null.</param>
        /// <returns>Virtual Channel Raw PDU.</returns>
        public Virtual_Channel_RAW_Pdu CreateRawVirtualChannelPDU(long channelId,
                                                                  uint length,
                                                                  CHANNEL_PDU_HEADER_flags_Values flags,
                                                                  byte[] virtualChannelData)
        {
            Virtual_Channel_RAW_Pdu channelPdu = new Virtual_Channel_RAW_Pdu(context);
            RdpbcgrUtility.FillCommonHeader(ref channelPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO,
                                     context,
                                     channelId);
            channelPdu.virtualChannelData = virtualChannelData;
            channelPdu.channelPduHeader.flags = flags;
            channelPdu.channelPduHeader.length = length;

            return channelPdu;
        }


        /// <summary>
        /// Create an array of Virtual Channel Raw PDU. The argument virtualChannelData will be split into 
        /// several PDUs according to chunk size negotiated before and compressed before sending.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Then call SendPDU to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <param name="channelId">The channel Id of the virtual channel. 
        /// If the channel id is invalid, then the return value is null.</param>
        /// <param name="virtualChannelData">The virtual channel data to be splitted. This argument can be null.
        /// If it is null, then no Virtual Channel Raw PDU will be generated.</param>
        /// <returns>The split Virtual Channel Raw PDUs.</returns>
        public Collection<Virtual_Channel_RAW_Pdu> CreateCompleteVirtualChannelPdu(long channelId,
                                                                                   byte[] virtualChannelData)
        {
            Virtual_Channel_Complete_Pdu completePdu = new Virtual_Channel_Complete_Pdu(context);
            completePdu.channelId = channelId;
            completePdu.virtualChannelData = virtualChannelData;
            completePdu.SplitToChunks();

            return completePdu.rawPdus;
        }
        #endregion Virtual Channel


        #region RDPELE PDU
        /// <summary>
        /// Create a RdpelePdu. 
        /// This type of PDU is especially used for RDPELE which is based on RDPBCGR.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// <param name="rdpeleData">The RDPELE message excludes RDPBCGR header. 
        /// That means this argument is composed of fields preamble and LicensingMessage defined in RDPELE.</param>
        /// <returns>A RdpelePdu.</returns>
        public RdpelePdu CreateRdpelePdu(byte[] rdpeleData)
        {
            RdpelePdu dataPdu = new RdpelePdu(context);
            RdpbcgrUtility.FillCommonHeader(context, ref dataPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT);

            dataPdu.rdpeleData = rdpeleData;

            return dataPdu;
        }
        #endregion RDPELE PDU

        #region Others

        /// <summary>
        /// Encode NETWORK_DETECTION_RESPONSE
        /// </summary>
        /// <param name="networkDetectionResponse"></param>
        /// <param name="isSubHeader"></param>
        /// <returns></returns>
        public static byte[] EncodeNetworkDetectionResponse(NETWORK_DETECTION_RESPONSE networkDetectionResponse, bool isSubHeader = false)
        {
            List<byte> respDataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeNetworkDetectionResponse(respDataBuffer, networkDetectionResponse, isSubHeader);
            return RdpbcgrUtility.ToBytes(respDataBuffer);
        }

        /// <summary>
        /// Create Client Auto-Detect Response PDU
        /// </summary>
        /// <param name="autodetectRspPduData">A variable-length field that contains auto-detect response data</param>
        /// <returns></returns>
        public Client_Auto_Detect_Response_PDU CreateAutoDetectResponsePdu(NETWORK_DETECTION_RESPONSE autodetectRspPduData)
        {
            if (context.MessageChannelId == null)
            {
                throw new InvalidOperationException("MCS message channel should be created.");
            }
            Client_Auto_Detect_Response_PDU pdu = new Client_Auto_Detect_Response_PDU(context);
            RdpbcgrUtility.FillCommonHeader(ref pdu.commonHeader, TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_RSP, context, context.MessageChannelId.Value);
            pdu.autodetectRspPduData = autodetectRspPduData;
            return pdu;
        }

        /// <summary>
        /// Create Client Initiate Multitransport Response PDU
        /// </summary>
        /// <param name="requestId">Request ID of associated Initiate Multitransport Request PDU</param>
        /// <returns></returns>
        public Client_Initiate_Multitransport_Response_PDU CreateInitiateMultitransportResponsePdu(uint requestId)
        {
            Client_Initiate_Multitransport_Response_PDU pdu = new Client_Initiate_Multitransport_Response_PDU(context);
            pdu.requestId = requestId;
            pdu.hrResponse = HrResponse_Value.E_ABORT;

            RdpbcgrUtility.FillCommonHeader(context, ref pdu.commonHeader,
                                    TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                    | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO);

            return pdu;
        }
        #endregion Others

        #endregion packet API


        #region raw API
        /// <summary>
        /// Encode a PDU to a binary stream. Then send the stream.
        /// The pdu could be got by calling method Create***PDU.
        /// 
        /// Notice: Virtual_Channel_Complete_PDU could not be sent. 
        /// It should be split into several Virtual_Channel_RAW_PDUs to send.
        /// </summary>
        /// <param name="pdu">A specified type of a PDU. This argument can not be null.
        /// If it is null, ArgumentNullException will be thrown.</param>
        /// <exception>Throw ArgumentNullException or FormatException if there is an encoding error.</exception>
        public void SendPdu(StackPacket pdu)
        {
            if (pdu == null)
            {
                throw new ArgumentNullException("pdu");
            }

            context.UpdateContext(pdu);

            transportStack.SendPacket(pdu);
            CheckEncryptionCount();
        }

        public void SendBytes(byte[] pktBytes)
        {
            if (pktBytes == null)
            {
                throw new ArgumentNullException("pdu");
            }

            transportStack.SendBytes(pktBytes);
            CheckEncryptionCount();
        }

        private StackPacket ExpectPdu(TimeSpan timeout, Func<StackPacket, bool> filter)
        {
            if (timeout.TotalMilliseconds < 0)
            {
                return null;
            }

            // Return the packet in the buffer, which is unprocessed packet.
            StackPacket packet = this.context.GetPacketFromBuffer(filter);

            if (packet != null)
            {
                return packet;
            }

            if (disconnected)
            {
                // No more packet would be available since disconnected.
                return null;
            }

            TransportEvent eventPacket = null;

            TimeSpan leftTime = timeout;
            DateTime endTime = DateTime.Now + timeout;

            while (leftTime.TotalMilliseconds > 0)
            {
                eventPacket = transportStack.ExpectTransportEvent(leftTime);
                packet = (StackPacket)eventPacket.EventObject;

                if (eventPacket.EventType == EventType.Disconnected)
                {
                    disconnected = true;
                    break;
                }
                else if (eventPacket.EventType == EventType.Exception)
                {
                    packet = new ErrorPdu(eventPacket.EventObject as Exception);
                }

                if (packet is ErrorPdu)
                {
                    // Receive thread has ended due to an exception of error PDU.
                    disconnected = true;
                }

                if (packet is Server_X_224_Connection_Confirm_Pdu)
                {
                    // Negotiation-based security-enhanced Connection
                    UpdateTransport();
                }

                if (isAutoReactivate && (packet is Server_Deactivate_All_Pdu))
                {
                    Reactivate(timeout);
                }

                if (filter(packet))
                {
                    // Return the packet if it is requested.
                    return packet;
                }
                else
                {
                    // Add the packet to buffer if it is not requested.
                    context.AddPacketToBuffer(packet);
                }

                if (disconnected)
                {
                    // No more packets from receive thread.
                    break;
                }

                leftTime = endTime - DateTime.Now;
            }

            return null;
        }


        /// <summary>
        /// Expect to receive a PDU of any type except virtual channel PDU from the remote host.
        /// To receive virtual channel PDU, please use method ExpectChannelPDU.
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception>TimeoutException.</exception>
        public StackPacket ExpectPdu(TimeSpan timeout)
        {
            var result = ExpectPdu(timeout, packet => true);

            return result;
        }

        /// <summary>
        /// Expect to receive a virtual channel PDU. Other types' PDUs will be filtered except an error occurs.
        /// If so MCS_Disconnect_Provider_Ultimatum_PDU or ErrorPdu will be returned according to the error.</summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <returns>The channel PDU.</returns>
        /// <exception>TimeoutException.</exception>
        public StackPacket ExpectChannelPdu(TimeSpan timeout)
        {
            var result = ExpectPdu(timeout, packet =>
            {
                if (packet is MCS_Disconnect_Provider_Ultimatum_Pdu
                         || packet is ErrorPdu
                         || packet is Virtual_Channel_RAW_Server_Pdu)                       // some error occurs
                {
                    return true;
                }
                return false;
            });

            return result;
        }

        /// <summary>
        /// Expect to receive a virtual channel PDU. Other types' PDUs will be filtered except an error occurs.
        /// If so MCS_Disconnect_Provider_Ultimatum_PDU or ErrorPdu will be returned according to the error.
        /// This method is especially used for expecting Virtual_Channel_RAW_PDU or Virtual_Channel_Complete_PDU.
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled and
        /// Virtual_Channel_RAW_PDU will be returned. Otherwise, the PDU will be reassembled and
        /// Virtual_Channel_Complete_PDU will be returned.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception>TimeoutException or InvalidCastException.</exception>
        public StackPacket ExpectChannelPdu(TimeSpan timeout, bool isRawPdu)
        {
            StackPacket packet = null;

            if (!isRawPdu)   // reassemble the virtual channel data
            {
                TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;
                Virtual_Channel_Complete_Server_Pdu pdu = null;
                while (pdu == null)
                {
                    packet = ExpectChannelPdu(totalTime - DateTime.Now.TimeOfDay);

                    if (packet is MCS_Disconnect_Provider_Ultimatum_Pdu || packet is ErrorPdu)
                    {
                        // some error occurs
                        break;
                    }

                    pdu = context.ReassembleChunkData((Virtual_Channel_RAW_Server_Pdu)packet);
                }

                packet = pdu;
            }
            else             // raw PDU, so return it directly
            {
                packet = ExpectChannelPdu(timeout);
            }

            return packet;
        }


        /// <summary>
        /// Expect to receive a virtual channel PDU from a given channel. Other types' PDUs will be filtered except
        /// an error occurs. If so MCS_Disconnect_Provider_Ultimatum_PDU or ErrorPdu will be returned according to 
        /// the error. This method is especially used for expecting Virtual_Channel_RAW_Pdu or 
        /// Virtual_Channel_Complete_Pdu.
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled and
        /// Virtual_Channel_RAW_PDU will be returned. Otherwise, the PDU will be reassembled and
        /// Virtual_Channel_Complete_PDU will be returned.</param>
        /// <param name="channelId">Specify the given channel.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception>TimeoutException or InvalidCastException.</exception>
        public StackPacket ExpectChannelPdu(TimeSpan timeout, bool isRawPdu, ushort channelId)
        {
            StackPacket packet = null;
            Virtual_Channel_RAW_Server_Pdu rawPdu = null;
            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;

            if (!isRawPdu)   // reassemble the virtual channel data
            {
                Virtual_Channel_Complete_Server_Pdu pdu = null;
                while (pdu == null)
                {
                    packet = ExpectChannelPdu(totalTime - DateTime.Now.TimeOfDay);

                    if (packet is MCS_Disconnect_Provider_Ultimatum_Pdu || packet is ErrorPdu)
                    {
                        // some error occurs
                        return packet;
                    }

                    rawPdu = (Virtual_Channel_RAW_Server_Pdu)packet;

                    if (rawPdu.commonHeader.channelId != channelId)
                    {
                        // discard the pdu which is belong to other channels
                        continue;
                    }

                    pdu = context.ReassembleChunkData(rawPdu);
                }

                packet = pdu;
            }
            else             // raw PDU, so return it directly
            {
                while (true)
                {
                    packet = ExpectChannelPdu(totalTime - DateTime.Now.TimeOfDay);

                    if (packet is MCS_Disconnect_Provider_Ultimatum_Pdu || packet is ErrorPdu)
                    {
                        // some error occurs
                        return packet;
                    }

                    rawPdu = (Virtual_Channel_RAW_Server_Pdu)packet;

                    if (rawPdu.commonHeader.channelId == channelId)
                    {
                        // return the pdu with the right channel Id
                        break;
                    }
                }

                packet = rawPdu;
            }

            return packet;
        }

        /// <summary>
        /// Expect disconnection event from SUT.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <exception>TimeoutException.</exception>

        public void ExpectDisconnect(TimeSpan timeout)
        {
            transportStack.ExpectDisconnect(timeout);
        }
        #endregion raw API


        #region IDisposable
        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                    if (transportStack != null)
                    {
                        transportStack.Dispose();
                        transportStack = null;
                    }

                    if (clientStream != null)
                    {
                        clientStream.Close();
                        clientStream.Dispose();
                        clientStream = null;
                    }

                    if (updateSessionKeyEvent != null)
                    {
                        updateSessionKeyEvent.Close();
                        updateSessionKeyEvent = null;
                    }

                    if (context != null)
                    {
                        context.Dispose();
                        context = null;
                    }
                }

                //Note disposing has been done.
                disposed = true;
            }
        }


        /// <summary>
        /// Destruct this instance.
        /// </summary>
        ~RdpbcgrClient()
        {
            Dispose(false);
        }
        #endregion
    }
}
