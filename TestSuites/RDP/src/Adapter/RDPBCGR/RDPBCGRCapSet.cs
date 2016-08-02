// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System.Collections.Generic;


namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public class RdpbcgrCapSet
    {

        #region Methods to convert cap set structure to byte array.
        //copy the functions from sdk as this func is internal
        //as some types are not implemented in SDK, we have to temporarily copy this funcs here
        //for future, we should add miss types to sdk and remove this region

        /// <summary>
        /// Encode a structure to a byte list.
        /// </summary>
        /// <param name="buffer">The buffer list to contain the structure. 
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        /// <param name="structure">The structure to be added to buffer list.
        /// This argument cannot be null. It may throw ArgumentNullException if it is null.</param>
        internal static void EncodeStructure(List<byte> buffer, object structure)
        {
            byte[] structBuffer = StructToBytes(structure);
            buffer.AddRange(structBuffer);
        }
        /// <summary>
        /// Method to covert struct to byte[]
        /// </summary>
        /// <param name="structp">The struct prepare to covert</param>
        /// <returns>the got byte array converted from struct</returns>
        internal static byte[] StructToBytes(object structp)
        {
            IntPtr ptr = IntPtr.Zero;
            byte[] buffer = null;

            try
            {
                int size = Marshal.SizeOf(structp.GetType());
                ptr = Marshal.AllocHGlobal(size);
                buffer = new byte[size];
                Marshal.StructureToPtr(structp, ptr, false);
                Marshal.Copy(ptr, buffer, 0, size);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            return buffer;
        }
        #endregion "SDK Subset"

        private Collection<ITsCapsSet> m_arrCapSet = new Collection<ITsCapsSet>();

        /// <summary>
        /// Capability Sets collection
        /// </summary>
        public Collection<ITsCapsSet> CapabilitySets
        {
            get { return m_arrCapSet; }
            set { m_arrCapSet = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RdpbcgrCapSet()
        {
        }

        /// <summary>
        /// Construct a default cap sets collection which contains all possible cap sets.
        /// </summary>
        public void SetToDefault()
        {
            m_arrCapSet.Add(CreateGeneralCapSet(false, true, true, true, true, true, true, osMajorType_Values.OSMAJORTYPE_WINDOWS, osMinorType_Values.OSMINORTYPE_WINDOWS_NT));
            m_arrCapSet.Add(CreateBitmapCapSet(800, 600, desktopResizeFlag_Values.TRUE, true, true, true));
            m_arrCapSet.Add(CreateOrderCapSet(new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x01, 
                         0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 
                         0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 
                         0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00}, true, true, true, false, true, true, true));
            m_arrCapSet.Add(CreatePointerCapSet(colorPointerFlag_Values.TRUE, 25, 25));
            m_arrCapSet.Add(CreateInputCapSet(true, true, false, true, true));
            m_arrCapSet.Add(CreateVirtualChannelCapSet(true, true));
            m_arrCapSet.Add(CreateTSWindowCapSet(2, 3, 12));
            m_arrCapSet.Add(CreateShareCapSet());
            m_arrCapSet.Add(CreateTSRailCapSet(true, true));
            m_arrCapSet.Add(CreateBitmapCacheHostSupportCapSet());
            m_arrCapSet.Add(CreateDesktopCompositionCapSet(CompDeskSupportLevel_Values.COMPDESK_SUPPORTED));
            m_arrCapSet.Add(CreateMultiFragmentUpdateCapSet(20488));
            m_arrCapSet.Add(CreateFontCapSet());
            m_arrCapSet.Add(CreateColorTableCapSet());
        }

        /// <summary>
        /// Fill the cap set collection with the input settings.
        /// </summary>
        /// <param name="capSetting">the input capability setting.</param>
        public void SetFromConfig(ServerCapabilitySetting capSetting)
        {
            m_arrCapSet.Add(CreateGeneralCapSet(true, true, true, capSetting.GeneralCapSet_ExtraFlags_AutoReconnect, true, capSetting.GeneralCapSet_RefreshRectSupport, capSetting.GeneralCapSet_SuppressOutputSupport, osMajorType_Values.OSMAJORTYPE_WINDOWS, osMinorType_Values.OSMINORTYPE_WINDOWS_NT));
            m_arrCapSet.Add(CreateBitmapCapSet(capSetting.DesktopWidth, capSetting.DesktopHeight, desktopResizeFlag_Values.TRUE, true, true, true));
            m_arrCapSet.Add(CreateOrderCapSet(new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x01, 
                         0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 
                         0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 
                         0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00}, true, true, true, false, true, true, true));
            m_arrCapSet.Add(CreatePointerCapSet(colorPointerFlag_Values.TRUE, 25, 25));
            m_arrCapSet.Add(CreateInputCapSet(
                capSetting.InputCapSet_InputFlags_ScanCodes,
                capSetting.InputCapSet_InputFlags_MouseX,
                capSetting.InputCapSet_InputFlags_FPInput,
                capSetting.InputCapSet_InputFlags_Unicode,
                capSetting.InputCapSet_InputFlags_FPInput2));
            m_arrCapSet.Add(CreateVirtualChannelCapSet(capSetting.VCCapSet_CS_CompressionSupport, capSetting.VCCapSet_SC_VCChunkSizePresent));
            m_arrCapSet.Add(CreateTSWindowCapSet(2, 3, 12));

            if (capSetting.ShareCapabilitySet)
            {
                m_arrCapSet.Add(CreateShareCapSet());
            }

            m_arrCapSet.Add(CreateTSRailCapSet(true, true));

            if (capSetting.BitmapCacheHostSupportCapabilitySet)
            {
                m_arrCapSet.Add(CreateBitmapCacheHostSupportCapSet());
            }

            if (capSetting.DesktopCompositionCapabilitySet)
            {
                m_arrCapSet.Add(CreateDesktopCompositionCapSet(CompDeskSupportLevel_Values.COMPDESK_SUPPORTED));
            }

            if (capSetting.MultifragmentUpdateCapabilitySet)
            {
                m_arrCapSet.Add(CreateMultiFragmentUpdateCapSet(capSetting.MultifragmentUpdateCapabilitySet_maxRequestSize));
            }

            if (capSetting.FontCapabilitySet)
            {
                m_arrCapSet.Add(CreateFontCapSet());
            }

            if (capSetting.LargePointerCapabilitySet)
            {
                m_arrCapSet.Add(CreateLargePointerCapSet(largePointerSupportFlags_Values.LARGE_POINTER_FLAG_96x96));
            }

            if (capSetting.FrameAcknowledgeCapabilitySet)
            {
                m_arrCapSet.Add(CreateFrameAcknowledgeCapSet(capSetting.FrameAcknowledgeCapabilitySet_maxUnacknowledgedFrameCount));
            }

            if (capSetting.SurfaceCommandsCapabilitySet)
            {
                m_arrCapSet.Add(CreateSurfaceCmdCapSet(capSetting.SurfaceCommandsCapabilitySet_cmdFlag));
            }

            if (capSetting.BitmapCodecsCapabilitySet)
            {
                m_arrCapSet.Add(CreateBitmapCodecsCapSet(capSetting.BitmapCodecsCapabilitySet_NSCodec, capSetting.BitmapCodecsCapabilitySet_RemoteFx));
            }

            m_arrCapSet.Add(CreateColorTableCapSet());
        }

        /// <summary>
        /// Search a cap set with type value.
        /// </summary>
        /// <param name="capType">Type of the cap set</param>
        /// <returns>The cap set with the specified type; if not find, return null;</returns>
        public ITsCapsSet FindCapSet(capabilitySetType_Values capType)
        {
            foreach (ITsCapsSet cap in m_arrCapSet)
            {
                if (cap.GetCapabilityType() == capType)
                    return cap;
            }

            return null;
        }

        #region "Mandatory Cap Sets"
        /// <summary>
        /// Create a TS_GENERAL_CAPABILITYSET type Capability, 2.2.7.1.1   
        /// </summary>
        /// <param name="fpOutput">Advertiser supports fast-path output.</param>
        /// <param name="noBitmapCompHdr">Advertiser supports excluding the 8-byte Compressed Data Header (section 2.2.9.1.1.3.1.2.3) 
        /// from the Bitmap Data (section 2.2.9.1.1.3.1.2.2) structure 
        /// or the Cache Bitmap (Revision 2) Secondary Drawing Order ([MS-RDPEGDI] section 2.2.2.2.1.2.3).</param>
        /// <param name="longCreds">Advertiser supports long-length credentials for the user name, password, 
        /// or domain name in the Save Session Info PDU (section 2.2.10.1).</param>
        /// <param name="autoReconnect">Advertiser supports auto-reconnection (section 5.5).</param>
        /// <param name="mac">Advertiser supports salted MAC generation (see section 5.3.6.1.1).</param>
        /// <param name="refreshRect">Server supports Refresh Rect PDU or not</param>
        /// <param name="suppressOutput">Server supports Suppress Output PDU or not</param>
        /// <param name="osMajorType">The type of platform</param>
        /// <param name="osMinorType">The version of the platform specified in the osMajorType field</param>
        /// <returns>TS_GENERAL_CAPABILITYSET type Capability</returns>
        public static TS_GENERAL_CAPABILITYSET CreateGeneralCapSet(
            bool fpOutput,
            bool noBitmapCompHdr,
            bool longCreds,
            bool autoReconnect,
            bool mac,
            bool refreshRect,
            bool suppressOutput,
            osMajorType_Values osMajorType,
            osMinorType_Values osMinorType
            )
        {
            TS_GENERAL_CAPABILITYSET generalCapabilitySet = new TS_GENERAL_CAPABILITYSET();

            generalCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_GENERAL;
            generalCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(generalCapabilitySet);
            generalCapabilitySet.osMajorType = osMajorType;
            generalCapabilitySet.osMinorType = osMinorType;
            generalCapabilitySet.protocolVersion = protocolVersion_Values.V1;
            generalCapabilitySet.pad2octetsA = 0;
            generalCapabilitySet.generalCompressionTypes = generalCompressionTypes_Values.V1;

            if (fpOutput)
                generalCapabilitySet.extraFlags |= extraFlags_Values.FASTPATH_OUTPUT_SUPPORTED;
            if (noBitmapCompHdr)
                generalCapabilitySet.extraFlags |= extraFlags_Values.NO_BITMAP_COMPRESSION_HDR;
            if (longCreds)
                generalCapabilitySet.extraFlags |= extraFlags_Values.LONG_CREDENTIALS_SUPPORTED;
            if (autoReconnect)
                generalCapabilitySet.extraFlags |= extraFlags_Values.AUTORECONNECT_SUPPORTED;
            if (mac)
                generalCapabilitySet.extraFlags |= extraFlags_Values.ENC_SALTED_CHECKSUM;

            generalCapabilitySet.updateCapabilityFlag = updateCapabilityFlag_Values.V1;
            generalCapabilitySet.remoteUnshareFlag = remoteUnshareFlag_Values.V1;
            generalCapabilitySet.generalCompressionLevel = generalCompressionLevel_Values.V1;

            generalCapabilitySet.refreshRectSupport = refreshRect ? refreshRectSupport_Values.TRUE : refreshRectSupport_Values.FALSE;
            generalCapabilitySet.suppressOutputSupport = suppressOutput ? suppressOutputSupport_Values.TRUE : suppressOutputSupport_Values.FALSE;

            generalCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(generalCapabilitySet);

            return generalCapabilitySet;
        }

        /// <summary>
        /// Create a TS_BITMAP_CAPABILITYSET type Capability, 2.2.7.1.2    
        /// </summary>
        /// <param name="desktopWidth">The width of the desktop in the session.</param>
        /// <param name="desktopHeight">The height of the desktop in the session.</param>
        /// <param name="desktopResizeFlag">Desktop resizing is supported or not</param>
        /// <param name="colorSubsampling">Indicates support for chroma subsampling when compressing 32 bpp bitmaps 
        /// ([MS-RDPEGDI] section 3.1.9.1.3).</param>
        /// <param name="dyColorFidelity">Indicates support for lossy compression of 32 bpp bitmaps by reducing color-fidelity
        /// on a per-pixel basis ([MS-RDPEGDI] section 3.1.9.1.4).</param>
        /// <param name="skipAlpha">Indicates that the client supports the removal of the alpha-channel when compressing 
        /// 32 bpp bitmaps. In this case the alpha is assumed to be 0xFF, meaning the bitmap is opaque.</param>
        /// <returns>TS_BITMAP_CAPABILITYSET type Capability</returns>
        public static TS_BITMAP_CAPABILITYSET CreateBitmapCapSet(ushort desktopWidth, ushort desktopHeight,
            desktopResizeFlag_Values desktopResizeFlag,
            bool colorSubsampling,
            bool dyColorFidelity,
            bool skipAlpha)
        {
            TS_BITMAP_CAPABILITYSET bitmapCapabilitySet = new TS_BITMAP_CAPABILITYSET();
            bitmapCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_BITMAP;
            bitmapCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(bitmapCapabilitySet);
            //A 16-bit, unsigned integer. The server MUST set this field to the color depth of the session, 
            //while the client SHOULD set this field to the color depth requested in the Client Core Data (section 2.2.1.3.2).
            bitmapCapabilitySet.preferredBitsPerPixel = 32;
            //A 16-bit, unsigned integer. Indicates whether the client can receive 1 bpp. 
            //This field is ignored and SHOULD be set to TRUE (0x0001).
            bitmapCapabilitySet.receive1BitPerPixel = 1;
            //A 16-bit, unsigned integer. Indicates whether the client can receive 4 bpp. 
            //This field is ignored and SHOULD be set to TRUE (0x0001).
            bitmapCapabilitySet.receive4BitsPerPixel = 1;
            //A 16-bit, unsigned integer. Indicates whether the client can receive 8 bpp. 
            //This field is ignored and SHOULD be set to TRUE (0x0001).
            bitmapCapabilitySet.receive8BitsPerPixel = 1;
            bitmapCapabilitySet.desktopWidth = desktopWidth;
            bitmapCapabilitySet.desktopHeight = desktopHeight;
            bitmapCapabilitySet.pad2octets = 0;
            bitmapCapabilitySet.desktopResizeFlag = desktopResizeFlag;
            //A 16-bit, unsigned integer. Indicates whether bitmap compression is supported. 
            //This field MUST be set to TRUE (0x0001) because support for compressed bitmaps is required for a connection to proceed.
            bitmapCapabilitySet.bitmapCompressionFlag = 1;
            //An 8-bit, unsigned integer. Client support for 16 bpp color modes. 
            //This field is ignored and SHOULD be set to 0.
            bitmapCapabilitySet.highColorFlags = 0;
            if (colorSubsampling)
                bitmapCapabilitySet.drawingFlags |= drawingFlags_Values.DRAW_ALLOW_COLOR_SUBSAMPLING;
            if (dyColorFidelity)
                bitmapCapabilitySet.drawingFlags |= drawingFlags_Values.DRAW_ALLOW_DYNAMIC_COLOR_FIDELITY;
            if (skipAlpha)
                bitmapCapabilitySet.drawingFlags |= drawingFlags_Values.DRAW_ALLOW_SKIP_ALPHA;

            bitmapCapabilitySet.drawingFlags |= (drawingFlags_Values)0x10;
            //A 16-bit, unsigned integer. Indicates whether the use of multiple bitmap rectangles is supported in the Bitmap Update (section 2.2.9.1.1.3.1.2). 
            //This field MUST be set to TRUE (0x0001) because multiple rectangle support is required for a connection to proceed.
            bitmapCapabilitySet.multipleRectangleSupport = 1;
            bitmapCapabilitySet.pad2octetsB = 0;
            return bitmapCapabilitySet;
        }

        /// <summary>
        /// Create a TS_ORDER_CAPABILITYSET type Capability, 2.2.7.1.3   
        /// Problem: contain hardCode as SDK bug(miss member)
        /// </summary>
        /// <param name="orderSupport">An array of 32 bytes indicating support for various primary drawing orders. 
        /// The indices of this array are the negotiation indices for the primary orders specified in [MS-RDPEGDI] section 2.2.2.2.1.1.2.</param>
        /// <param name="negoOrdSupport">Indicates support for specifying supported drawing orders in the orderSupport field. 
        /// This flag MUST be set.</param>
        /// <param name="zbSDELT">Indicates support for the TS_ZERO_BOUNDS_DELTAS (0x20) flag (see [MS-RDPEGDI] 
        /// section 2.2.2.2.1.1.2). The client MUST set this flag.</param>
        /// <param name="colorIdx">Indicates support for sending color indices (not RGB values) in orders.</param>
        /// <param name="spBrush">Indicates that this party can receive only solid and pattern brushes.</param>
        /// <param name="ordFlagsExFlags">Indicates that the orderSupportExFlags field contains valid data.</param>
        /// <param name="cbRev3">The Cache Bitmap (Revision 3) Secondary Drawing Order ([MS-RDPEGDI] section 2.2.2.2.1.2.8) 
        /// is supported.</param>
        /// <param name="afmSupport">The Frame Marker Alternate Secondary Drawing Order ([MS-RDPEGDI] section 2.2.2.2.1.3.7)
        /// is supported.</param>
        /// <returns>TS_ORDER_CAPABILITYSET type Capability</returns>
        public static TS_ORDER_CAPABILITYSET CreateOrderCapSet(byte[] orderSupport, bool negoOrdSupport, bool zbSDELT, bool colorIdx, bool spBrush, bool ordFlagsExFlags,
            bool cbRev3, bool afmSupport)
        {
            TS_ORDER_CAPABILITYSET orderCapabilitySet = new TS_ORDER_CAPABILITYSET();

            orderCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_ORDER;
            //A 16-element array of 8-bit, unsigned integers. Terminal descriptor. 
            //This field is ignored and SHOULD be set to all zeros.
            orderCapabilitySet.terminalDescriptor = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            orderCapabilitySet.pad4octetsA = 1000000;
            //A 16-bit, unsigned integer. X granularity used in conjunction with the SaveBitmap Primary Drawing Order (see [MS-RDPEGDI] section 2.2.2.2.1.1.2.12). 
            //This value is ignored and assumed to be 1.
            orderCapabilitySet.desktopSaveXGranularity = 1;
            //A 16-bit, unsigned integer. Y granularity used in conjunction with the SaveBitmap Primary Drawing Order (see [MS-RDPEGDI] section 2.2.2.2.1.1.2.12). 
            //This value is ignored and assumed to be 20.
            orderCapabilitySet.desktopSaveYGranularity = 20;
            orderCapabilitySet.pad2octetsA = 0;
            //A 16-bit, unsigned integer. Maximum order level. This value is ignored and SHOULD be set to ORD_LEVEL_1_ORDERS (1).
            orderCapabilitySet.maximumOrderLevel = 1;
            //A 16-bit, unsigned integer. Number of fonts. This value is ignored and SHOULD be set to 0.
            orderCapabilitySet.numberFonts = 0;
            if (negoOrdSupport)
                orderCapabilitySet.orderFlags |= orderFlags_Values.NEGOTIATEORDERSUPPORT;
            if (zbSDELT)
                orderCapabilitySet.orderFlags |= orderFlags_Values.ZEROBOUNDSDELTASSUPPORT;
            if (colorIdx)
                orderCapabilitySet.orderFlags |= orderFlags_Values.COLORINDEXSUPPORT;
            if (spBrush)
                orderCapabilitySet.orderFlags |= orderFlags_Values.SOLIDPATTERNBRUSHONLY;
            if (ordFlagsExFlags)
                orderCapabilitySet.orderFlags |= (orderFlags_Values)0x0080;//<hardcode>

            orderCapabilitySet.orderSupport = orderSupport;

            //A 16-bit, unsigned integer. Values in this field MUST be ignored.<?>
            orderCapabilitySet.textFlags = 1697;

            if (cbRev3)
                orderCapabilitySet.orderSupportExFlags |= orderSupportExFlags_values.ORDERFLAGS_EX_CACHE_BITMAP_REV3_SUPPORT;
            if (afmSupport)
                orderCapabilitySet.orderSupportExFlags |= orderSupportExFlags_values.ORDERFLAGS_EX_ALTSEC_FRAME_MARKER_SUPPORT;

            orderCapabilitySet.pad4octetsB = 1000000;
            //A 32-bit, unsigned integer. The maximum usable size of bitmap space for bitmap packing in the SaveBitmap Primary Drawing Order (see [MS-RDPEGDI] section 2.2.2.2.1.1.2.12). 
            //This field is ignored by the client and assumed to be 230400 bytes (480 * 480).<?>
            orderCapabilitySet.desktopSaveSize = 1000000;
            orderCapabilitySet.pad2octetsC = 1;
            orderCapabilitySet.pad2octetsD = 0;
            //A 16-bit, unsigned integer. ANSI code page descriptor being used by the client (for a list of code pages, see [MSDN-CP]). 
            //This field is ignored by the client and SHOULD be set to 0 by the server.
            orderCapabilitySet.textANSICodePage = 0;
            orderCapabilitySet.pad2octetsE = 0;
            orderCapabilitySet.lengthCapability = (ushort)(sizeof(ushort) * 14
                                                + sizeof(uint) * 3
                                                + orderCapabilitySet.terminalDescriptor.Length
                                                + orderCapabilitySet.orderSupport.Length);
            return orderCapabilitySet;
        }

        /// <summary>
        /// Create a TS_POINTER_CAPABILITYSET type Capability, 2.2.7.1.5    
        /// </summary>
        /// <param name="colorPointer">false:Monochrome mouse cursors are supported. true:Color mouse cursors are supported.</param>
        /// <param name="colorPointerCacheSize">A 16-bit, unsigned integer. The number of available slots in the 
        /// 24 bpp color pointer cache used to store data received in the Color Pointer Update (section 2.2.9.1.1.4.4).</param>
        /// <param name="pointerCacheSize">A 16-bit, unsigned integer. The number of available slots in the 
        /// pointer cache used to store pointer data of arbitrary bit depth received in the New Pointer Update 
        /// (section 2.2.9.1.1.4.5). If the value contained in this field is zero or the Pointer Capability 
        /// Set sent from the client does not include this field, the server will not use the New Pointer Update.</param>
        /// <returns>TS_POINTER_CAPABILITYSET type Capability</returns>
        public static TS_POINTER_CAPABILITYSET CreatePointerCapSet(colorPointerFlag_Values colorPointerFlag, ushort colorPointerCacheSize, ushort pointerCacheSize)
        {
            TS_POINTER_CAPABILITYSET pointerCapabilitySet = new TS_POINTER_CAPABILITYSET();
            pointerCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_POINTER;

            pointerCapabilitySet.colorPointerFlag = colorPointerFlag;

            pointerCapabilitySet.colorPointerCacheSize = colorPointerCacheSize;
            pointerCapabilitySet.pointerCacheSize = pointerCacheSize;
            pointerCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(pointerCapabilitySet);
            return pointerCapabilitySet;
        }

        /// <summary>
        /// Create a TS_INPUT_CAPABILITYSET type Capability, 2.2.7.1.6
        /// Problem: not implement client-end parameters, and we can modify this func in the future
        /// </summary>
        /// <param name="scanCodes">Indicates support for using scancodes in the Keyboard Event notifications 
        /// (see sections 2.2.8.1.1.3.1.1.1 and 2.2.8.1.2.2.1).</param>
        /// <param name="mouseX">Indicates support for Extended Mouse Event notifications (see sections 2.2.8.1.1.3.1.1.4 
        /// and 2.2.8.1.2.2.4).</param>
        /// <param name="fpInput">Advertised by RDP 5.0 and 5.1 servers. RDP 5.2, 6.0, 6.1, and 7.0 servers advertise the
        /// INPUT_FLAG_FASTPATH_INPUT2 flag to indicate support for fast-path input.</param>
        /// <param name="unicode">Indicates support for Unicode Keyboard Event notifications (see sections 2.2.8.1.1.3.1.1.2
        /// and 2.2.8.1.2.2.2).</param>
        /// <param name="fpInput2">Advertised by RDP 5.2, 6.0, 6.1, and 7.0 servers. Clients that do not support 
        /// this flag will not be able to use fast-path input when connecting to RDP 5.2, 6.0, 6.1, and 7.0 servers.</param>
        /// <returns>TS_INPUT_CAPABILITYSET type Capability</returns>
        public static TS_INPUT_CAPABILITYSET CreateInputCapSet(bool scanCodes, bool mouseX, bool fpInput, bool unicode, bool fpInput2)
        {
            TS_INPUT_CAPABILITYSET inputCapabilitySet = new TS_INPUT_CAPABILITYSET();
            inputCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_INPUT;

            if (scanCodes)
                inputCapabilitySet.inputFlags |= inputFlags_Values.INPUT_FLAG_SCANCODES;
            if (mouseX)
                inputCapabilitySet.inputFlags |= inputFlags_Values.INPUT_FLAG_MOUSEX;
            if (fpInput)
                inputCapabilitySet.inputFlags |= inputFlags_Values.INPUT_FLAG_FASTPATH_INPUT;
            if (unicode)
                inputCapabilitySet.inputFlags |= inputFlags_Values.INPUT_FLAG_UNICODE;
            if (fpInput2)
                inputCapabilitySet.inputFlags |= inputFlags_Values.INPUT_FLAG_FASTPATH_INPUT2;

            inputCapabilitySet.pad2octetsA = 0;

            //Only set by Client
            inputCapabilitySet.keyboardLayout = 0;
            //Only set by Client
            inputCapabilitySet.keyboardType = TS_INPUT_CAPABILITYSET_keyboardType_Values.None;
            //Only set by Client
            inputCapabilitySet.keyboardSubType = 0;
            //Only set by Client
            inputCapabilitySet.keyboardFunctionKey = 0;
            //Only set by Client
            inputCapabilitySet.imeFileName = "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-" +
                    "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-" +
                    "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-" +
                    "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00";
            inputCapabilitySet.lengthCapability = (ushort)(24 + 64); // the other fields(except imeFileName field) totoal length is 24
            return inputCapabilitySet;
        }

        /// <summary>
        /// Create a TS_VIRTUALCHANNEL_CAPABILITYSET type Capability, 2.2.7.1.10
        /// </summary>
        /// <param name="supportCompression"></param>
        /// <param name="presentChunkSize"></param>
        public static TS_VIRTUALCHANNEL_CAPABILITYSET CreateVirtualChannelCapSet(bool supportCompression, bool presentChunkSize)
        {
            TS_VIRTUALCHANNEL_CAPABILITYSET virtualChannelSet = new TS_VIRTUALCHANNEL_CAPABILITYSET();
            virtualChannelSet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL;

            if (supportCompression)
            {
                virtualChannelSet.flags = TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_COMPR_CS_8K;
            }
            else
            {
                virtualChannelSet.flags = TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_NO_COMPR;
            }

            if (presentChunkSize)
            {
                virtualChannelSet.VCChunkSize = 1600;//CHANNEL_CHUNK_LENGTH 
                virtualChannelSet.lengthCapability = 12;
            }
            else
            {
                virtualChannelSet.lengthCapability = 8;//it should be 8, SDK bug, VCChunkSize is optional.
                virtualChannelSet.VCChunkSize = 0;
            }
            return virtualChannelSet;
        }



        public void AddBitmapCacheCapSet()
        {

        }

        public void AddBrushCapSet()
        {

        }

        public void AddGlyphCacheCapSet()
        {
        }

        public void AddOffscreenBitmapCacheCapSet()
        {
        }

        public void AddSoundCapSet()
        {
            TS_SOUND_CAPABILITYSET soundCapSet = new TS_SOUND_CAPABILITYSET();

            soundCapSet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_SOUND;
            soundCapSet.lengthCapability = (ushort)Marshal.SizeOf(soundCapSet);
            soundCapSet.soundFlags = soundFlags_Values.SOUND_BEEPS_FLAG;
            m_arrCapSet.Add(soundCapSet);
        }

        #endregion "Mandatory Cap Sets"

        #region "Optional Cap Sets"

        /// <summary>
        /// Create a TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET type Capability, 2.2.7.2.1   
        /// </summary>
        /// <returns>TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET type Capability</returns>
        public static TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET CreateBitmapCacheHostSupportCapSet()
        {
            TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET bitmapHostsupprot =
                new TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET();
            bitmapHostsupprot.capabilitySetType = capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_HOSTSUPPORT;
            bitmapHostsupprot.cacheVersion = cacheVersion_Values.V1;
            bitmapHostsupprot.pad1 = 0;
            bitmapHostsupprot.pad2 = 0;
            bitmapHostsupprot.lengthCapability = (ushort)Marshal.SizeOf(bitmapHostsupprot);
            return bitmapHostsupprot;
        }

        public void AddControlCapSet()
        {
            TS_CONTROL_CAPABILITYSET control = new TS_CONTROL_CAPABILITYSET();
            control.capabilitySetType = capabilitySetType_Values.CAPSTYPE_CONTROL;
            control.lengthCapability = (ushort)Marshal.SizeOf(control);
            control.controlFlags = 0;
            control.remoteDetachFlag = 0;
            control.controlInterest = 2;
            control.detachInterest = 2;

            m_arrCapSet.Add(control);
        }

        public void AddWindowActivationCapSet()
        {
        }

        /// <summary>
        /// Create a TS_SHARE_CAPABILITYSET type Capability, 2.2.7.2.4
        /// </summary>
        /// <returns>TS_SHARE_CAPABILITYSET type Capability</returns>
        public static TS_SHARE_CAPABILITYSET CreateShareCapSet()
        {
            TS_SHARE_CAPABILITYSET shareCapabilitySet = new TS_SHARE_CAPABILITYSET();
            shareCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_SHARE;
            //A 16-bit, unsigned integer. This field SHOULD be set to 0 by the client and to the server channel ID by the server (0x03EA).
            shareCapabilitySet.nodeId = 1002;
            shareCapabilitySet.pad2octets = 0;
            shareCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(shareCapabilitySet);
            return shareCapabilitySet;
        }

        /// <summary>
        /// Create a TS_FONT_CAPABILITYSET type Capability, 2.2.7.2.5  
        /// </summary>
        /// <returns>TS_FONT_CAPABILITYSET type Capability</returns>
        public static TS_FONT_CAPABILITYSET CreateFontCapSet()
        {
            TS_FONT_CAPABILITYSET fontCapabilitySet = new TS_FONT_CAPABILITYSET();
            fontCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_FONT;
            //A 16-bit, unsigned integer. The font support options. This field SHOULD be set to FONTSUPPORT_FONTLIST (0x0001).
            fontCapabilitySet.fontSupportFlags = 0x0001;
            fontCapabilitySet.pad2octets = 0;
            fontCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(fontCapabilitySet);

            return fontCapabilitySet;
        }

        /// <summary>
        /// Create a TS_MULTIFRAGMENTUPDATE_CAPABILITYSET type Capability, 2.2.7.2.6   
        /// </summary>
        /// <returns>TS_MULTIFRAGMENTUPDATE_CAPABILITYSET type Capability</returns>
        public static TS_MULTIFRAGMENTUPDATE_CAPABILITYSET CreateMultiFragmentUpdateCapSet(UInt32 maxRequestSize)
        {
            TS_MULTIFRAGMENTUPDATE_CAPABILITYSET multiFragmentCapabilitySet = new TS_MULTIFRAGMENTUPDATE_CAPABILITYSET();
            multiFragmentCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_MULTIFRAGMENTUPDATE;
            multiFragmentCapabilitySet.MaxRequestSize = maxRequestSize;
            multiFragmentCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(multiFragmentCapabilitySet);

            return multiFragmentCapabilitySet;
        }

        /// <summary>
        /// Create a TS_LARGE_POINTER_CAPABILITYSET type Capability, 2.2.7.2.7   
        /// <param name="largePointerSupportFlags">96-pixel by 96-pixel mouse pointer shapes are supported or not.</param>
        /// </summary>
        /// <returns>TS_LARGE_POINTER_CAPABILITYSET type Capability</returns>
        public static TS_LARGE_POINTER_CAPABILITYSET CreateLargePointerCapSet(largePointerSupportFlags_Values largePointerSupportFlags)
        {
            TS_LARGE_POINTER_CAPABILITYSET large = new TS_LARGE_POINTER_CAPABILITYSET();
            large.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_LARGE_POINTER;
            large.lengthCapability = (ushort)Marshal.SizeOf(large);
            large.largePointerSupportFlags = largePointerSupportFlags;
            return large;
        }
        /// <summary>
        /// Create a TS_COMPDESK_CAPABILITYSET type Capability, 2.2.7.2.8   
        /// </summary>
        /// <param name="compDeskSupportLevel">Desktop composition services are supported or not.</param>
        /// <returns>TS_COMPDESK_CAPABILITYSET type Capability</returns>
        public static TS_COMPDESK_CAPABILITYSET CreateDesktopCompositionCapSet(CompDeskSupportLevel_Values compDeskSupportLevel)
        {
            TS_COMPDESK_CAPABILITYSET desktopCapabilitySet = new TS_COMPDESK_CAPABILITYSET();

            desktopCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_COMPDESK;
            desktopCapabilitySet.CompDeskSupportLevel = compDeskSupportLevel;
            desktopCapabilitySet.lengthCapability = sizeof(ushort) + sizeof(ushort) + sizeof(ushort);

            return desktopCapabilitySet;
        }

        /// <summary>
        /// Create a TS_COMPDESK_CAPABILITYSET type Capability, 2.2.7.2.9 
        /// Problem: hardcode as SDK bug (miss member)
        /// </summary>
        /// <param name="setSurfBits">The Set Surface Bits Command (section 2.2.9.2.1) is supported or not.</param>
        /// <param name="frameMarker">The Frame Marker Command (section 2.2.9.2.3) is supported or not.</param>
        /// <param name="streamSurfBits">The Stream Surface Bits Command (section 2.2.9.2.2) is supported or not.</param>
        /// <returns>TS_SURFCMDS_CAPABILITYSET type Capability</returns>
        public static TS_SURFCMDS_CAPABILITYSET CreateSurfaceCmdCapSet(CmdFlags_Values cmdFlags)
        {
            TS_SURFCMDS_CAPABILITYSET surfCmds = new TS_SURFCMDS_CAPABILITYSET();

            surfCmds.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_SURFACE_COMMANDS;
            surfCmds.lengthCapability = (ushort)Marshal.SizeOf(surfCmds);
            surfCmds.cmdFlags = cmdFlags;

            return surfCmds;
        }

        private static TS_BITMAPCODEC CreateTS_BITMAPCODEC_NSCodec()
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
            bitmapCodec.codecID = 0;
            bitmapCodec.codecPropertiesLength = 3;
            bitmapCodec.codecProperties = new byte[3];
            bitmapCodec.codecProperties[0] = 0x01; //fAllowDynamicFidelity 
            bitmapCodec.codecProperties[1] = 0x01; //fAllowSubsampling
            bitmapCodec.codecProperties[2] = 0x03; //colorLossLevel 
            return bitmapCodec;
        }

        private static TS_BITMAPCODEC CreateTS_BITMAPCODEC_RemoteFX()
        {
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
            bitmapCodec.codecID = 0;
            bitmapCodec.codecPropertiesLength = 4;
            bitmapCodec.codecProperties = new byte[4];//TS_RFX_SRVR_CAPS_CONTAINER, reserved, 4 bytes, all zero.
            return bitmapCodec;
        }

        /// <summary>
        /// Create a TS_BITMAPCODECS_CAPABILITYSET type Capability, 2.2.7.2.10   
        /// Problem: lengthCapability may be wrong, as different from previous value
        /// </summary>
        ///<param name="isNSCodecPresent">Indicates if present NS codec.</param>
        ///<param name="isRemoteFxCodecPresent">Indicates if present RemoteFX codec.</param>
        /// <returns>TS_BITMAPCODECS_CAPABILITYSET type Capability</returns>
        public static TS_BITMAPCODECS_CAPABILITYSET CreateBitmapCodecsCapSet(bool isNSCodecPresent, bool isRemoteFxCodecPresent)
        {
            TS_BITMAPCODEC[] codecArr;
            if (isNSCodecPresent && isRemoteFxCodecPresent)
            {
                codecArr = new TS_BITMAPCODEC[2];
                codecArr[0] = CreateTS_BITMAPCODEC_NSCodec();
                codecArr[1] = CreateTS_BITMAPCODEC_RemoteFX();
            }
            else if (isRemoteFxCodecPresent)
            {
                codecArr = new TS_BITMAPCODEC[1];
                codecArr[0] = CreateTS_BITMAPCODEC_RemoteFX();
            }
            else
            {
                codecArr = new TS_BITMAPCODEC[1];
                codecArr[0] = CreateTS_BITMAPCODEC_NSCodec();
            }

            TS_BITMAPCODECS_CAPABILITYSET codecCapSet = new TS_BITMAPCODECS_CAPABILITYSET();
            codecCapSet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS;
            codecCapSet.supportedBitmapCodecs.bitmapCodecCount = (byte)codecArr.Length;
            codecCapSet.supportedBitmapCodecs.bitmapCodecArray = codecArr;

            //<needcheck>
            // capabilitySetType (2 bytes) + lengthCapability (2 bytes) + bitmapCodecCount (1 byte)
            codecCapSet.lengthCapability = (ushort)(2 + 2 + 1);
            for (int index = 0; index < codecCapSet.supportedBitmapCodecs.bitmapCodecCount; ++index)
            {
                //codecGUID (16 bytes) + codecID (1 byte) + codecPropertiesLength (2 bytes) + codecProperties (variable)
                codecCapSet.lengthCapability += (ushort)(16 + 1 + 2 + codecCapSet.supportedBitmapCodecs.bitmapCodecArray[index].codecPropertiesLength);
            }

            return codecCapSet;
        }

        public static TS_FRAME_ACKNOWLEDGE_CAPABILITYSET CreateFrameAcknowledgeCapSet(uint maxUnacknowledgedFrameCount)
        {
            TS_FRAME_ACKNOWLEDGE_CAPABILITYSET capSet = new TS_FRAME_ACKNOWLEDGE_CAPABILITYSET();
            capSet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_FRAME_ACKNOWLEDGE;
            capSet.lengthCapability = 8;
            capSet.maxUnacknowledgedFrameCount = maxUnacknowledgedFrameCount;
            return capSet;
        }

        #endregion "Optional Cap Sets"

        #region "MISC Cap Sets"
        /// <summary>
        /// Create a TS_WINDOW_CAPABILITYSET type Capability, 2.2.1.1.2 in RDPERP
        /// Problem: field not implemented in SDK
        /// Problem: contain hardCode as SDK bug(miss member)
        /// </summary>
        /// <param name="wndSupportLevel">0x00000000:The client or server is not capable of supporting Windowing Alternate 
        /// Secondary Drawing Orders.0x00000001: The client or server is capable of supporting Windowing Alternate Secondary
        /// Drawing Orders. 0x00000002: The client or server is capable of supporting Windowing Alternate Secondary Drawing 
        /// Orders and the following flags: WINDOW_ORDER_FIELD_CLIENTAREASIZE WINDOW_ORDER_FIELD_RPCONTENTWINDOW_ORDER_FIELD_ROOTPARENT</param>
        /// <param name="numIconCaches">An unsigned 8-bit integer. The number of icon caches requested by the server 
        /// (Demand Active PDU) or supported by the client (Confirm Active PDU).The server maintains an icon cache and refers 
        /// to it to avoid sending duplicate icon information (see section 2.2.1.3.1.2.3). The client also maintains an icon 
        /// cache and refers to it when the server sends across a Cached Icon Window Information Order. </param>
        /// <param name="numIconCacheEntries">An unsigned 16-bit integer. The number of entries within each icon cache 
        /// requested by the server (Demand Active PDU) or supported by the client (Confirm Active PDU).The server maintains 
        /// an icon cache and refers to it to avoid sending duplicate icon information (see section 2.2.1.3.1.2.3). 
        /// The client also maintains an icon cache and refers to it when the server sends across a Cached Icon Window Information Order.</param>
        /// <returns>TS_WINDOW_CAPABILITYSET type Capability</returns>
        public static TS_WINDOW_CAPABILITYSET CreateTSWindowCapSet(UInt32 wndSupportLevel, byte numIconCaches, ushort numIconCacheEntries)
        {
            TS_WINDOW_CAPABILITYSET winSet = new TS_WINDOW_CAPABILITYSET();
            winSet.rawData = new byte[] { 24, 0, 11, 0, 0, 0, 0, 0, 3, 12, 0 };
            //winSet.rawData = new byte[] { 24, 0, 11, 0, 2, 0, 0, 0, 3, 12, 0 };
            //CapabilitySetType (2 bytes)
            //LengthCapability (2 bytes)
            //WndSupportLevel (4 bytes)
            if (wndSupportLevel == 1)
                winSet.rawData[4] = 0x01;
            if (wndSupportLevel == 2)
                winSet.rawData[4] = 0x02;
            //NumIconCaches (1 byte)
            winSet.rawData[8] = numIconCaches;
            //NumIconCacheEntries (2 bytes)
            winSet.rawData[9] = (byte)(numIconCacheEntries & 0xff);
            winSet.rawData[10] = (byte)(numIconCacheEntries >> 8);
            return winSet;
        }

        /// <summary>
        /// Create a TS_DRAWGRIDPLUS_CAPABILITYSET type Capability, 2.2.1.3 in MS-RDPEGDI
        /// Problem: field not implemented in SDK
        /// </summary>
        /// <param name="gdiSupport">0x00000000:GDI+ 1.1 is not supported 0x00000001: GDI+ 1.1 is supported</param>
        /// <param name="version ">A 32-bit, unsigned integer. The build number of the underlying GDI+ 1.1 subsystem. 
        /// Only the client-to-server instance of the GDI+ Capability Set MUST contain a valid value for this field.</param>
        /// <param name="crpSupport">A 32-bit, unsigned integer. The level of support for the caching of GDI+ 1.1 
        /// rendering primitives. </param>
        /// <param name="gdipGraphicsCacheEntries">Only the client-to-server instance of the GDI+ Capability Set MUST 
        /// contain a valid value for this field. A 16-bit, unsigned integer. The total number of entries allowed in the 
        /// GDI+ Graphics cache. The maximum allowed value is 10 entries.</param>
        /// <param name="gdipBrushCacheEntries">Only the client-to-server instance of the GDI+ Capability Set MUST 
        /// contain a valid value for this field. A 16-bit, unsigned integer. The total number of entries allowed 
        /// in the GDI+ Brush cache. The maximum allowed value is 5 entries.</param>
        /// <param name="gdipPenCacheEntries">Only the client-to-server instance of the GDI+ Capability Set MUST 
        /// contain a valid value for this field. A 16-bit, unsigned integer. The total number of entries allowed 
        /// in the GDI+ Pen cache. The maximum allowed value is 5 entries.</param>
        /// <param name="gdipImageCacheEntries">Only the client-to-server instance of the GDI+ Capability Set MUST 
        /// contain a valid value for this field. A 16-bit, unsigned integer. The total number of entries allowed in the 
        /// GDI+ Image cache. The maximum allowed value is 10 entries.</param>
        /// <param name="gdipImageAttributesCacheEntries">Only the client-to-server instance of the GDI+ Capability 
        /// Set MUST contain a valid value for this field. A 16-bit, unsigned integer. The total number of entries 
        /// allowed in the GDI+ Image Attributes cache. The maximum allowed value is 2 entries.</param>
        /// <param name="gdipGraphicsCacheChunkSize">Only the client-to-server instance of the GDI+ Capability Set 
        /// MUST contain a valid value for this field.A 16-bit, unsigned integer. The maximum size in bytes of a GDI+ 
        /// Graphics cache entry. The maximum allowed value is 512 bytes.</param>
        /// <param name="gdipObjectBrushCacheChunkSize">Only the client-to-server instance of the GDI+ Capability Set
        /// MUST contain a valid value for this field.A 16-bit, unsigned integer. The maximum size in bytes of a GDI+ 
        /// Brush cache entry. The maximum allowed value is 2,048 bytes.</param>
        /// <param name="gdipObjectPenCacheChunkSize">Only the client-to-server instance of the GDI+ Capability Set 
        /// MUST contain a valid value for this field. A 16-bit, unsigned integer. The maximum size in bytes of a GDI+ 
        /// Pen cache entry. The maximum allowed value is 1,024 bytes.</param>
        /// <param name="gdipObjectImageAttributesCacheChunkSize">Only the client-to-server instance of the GDI+ Capability 
        /// Set MUST contain a valid value for this field. ):  A 16-bit, unsigned integer. The maximum size in bytes of a 
        /// GDI+ Image Attributes cache entry. The maximum allowed value is 64 bytes.</param>
        /// <param name="gdipObjectImageCacheChunkSize">Only the client-to-server instance of the GDI+ Capability Set 
        /// MUST contain a valid value for this field.A 16-bit, unsigned integer. The maximum size in bytes of a chunk 
        /// in the GDI+ Image cache. The maximum allowed value is 4,096 bytes.</param>
        /// <param name="gdipObjectImageCacheTotalSize">Only the client-to-server instance of the GDI+ Capability Set 
        /// MUST contain a valid value for this field.A 16-bit, unsigned integer. The total number of chunks in the GDI+ 
        /// Image cache. The maximum allowed value is 256 chunks.</param>
        /// <param name="gdipObjectImageCacheMaxSize">Only the client-to-server instance of the GDI+ Capability Set MUST 
        /// contain a valid value for this field.A 16-bit, unsigned integer. The total number of chunks that can be used
        /// by an entry in the GDI+ Image cache. The maximum allowed value is 128 chunks.</param>
        /// <returns>TS_DRAWGRIDPLUS_CAPABILITYSET type Capability</returns>
        public static TS_DRAWGRIDPLUS_CAPABILITYSET CreateDrawGdiPlusCapSet(UInt32 gdiSupport, UInt32 version, UInt32 crpSupport,
            ushort gdipGraphicsCacheEntries, ushort gdipBrushCacheEntries, ushort gdipPenCacheEntries, ushort gdipImageCacheEntries, ushort gdipImageAttributesCacheEntries,
            ushort gdipGraphicsCacheChunkSize, ushort gdipObjectBrushCacheChunkSize, ushort gdipObjectPenCacheChunkSize, ushort gdipObjectImageAttributesCacheChunkSize,
            ushort gdipObjectImageCacheChunkSize, ushort gdipObjectImageCacheTotalSize, ushort gdipObjectImageCacheMaxSize)
        {
            TS_DRAWGRIDPLUS_CAPABILITYSET gridplusSet = new TS_DRAWGRIDPLUS_CAPABILITYSET();

            capabilitySetType_Values capabilitySetType = capabilitySetType_Values.CAPSTYPE_DRAWGDIPLUS;
            ushort lengthCapability = 40;

            List<byte> encodeBuffer = new List<byte>();
            EncodeStructure(encodeBuffer, (ushort)capabilitySetType);
            EncodeStructure(encodeBuffer, lengthCapability);
            EncodeStructure(encodeBuffer, gdiSupport);
            EncodeStructure(encodeBuffer, version);
            EncodeStructure(encodeBuffer, crpSupport);
            //GdipCacheEntries, Only set by Client
            EncodeStructure(encodeBuffer, gdipGraphicsCacheEntries);
            EncodeStructure(encodeBuffer, gdipBrushCacheEntries);
            EncodeStructure(encodeBuffer, gdipPenCacheEntries);
            EncodeStructure(encodeBuffer, gdipImageCacheEntries);
            EncodeStructure(encodeBuffer, gdipImageAttributesCacheEntries);
            //GdipCacheChunkSize, Only set by Client
            EncodeStructure(encodeBuffer, gdipGraphicsCacheChunkSize);
            EncodeStructure(encodeBuffer, gdipObjectBrushCacheChunkSize);
            EncodeStructure(encodeBuffer, gdipObjectPenCacheChunkSize);
            EncodeStructure(encodeBuffer, gdipObjectImageAttributesCacheChunkSize);
            //GdipImageCacheProperties, Only set by Client
            EncodeStructure(encodeBuffer, gdipObjectImageCacheChunkSize);
            EncodeStructure(encodeBuffer, gdipObjectImageCacheTotalSize);
            EncodeStructure(encodeBuffer, gdipObjectImageCacheMaxSize);

            gridplusSet.rawData = encodeBuffer.ToArray();
            return gridplusSet;
        }

        /// <summary>
        /// Create a TS_COLORCACHE_CAPABILITYSET type Capability, 2.2.1.1 in RDPGDI
        /// Problem: but in TD TS_COLORCACHE_CAPABILITYSET name TS_COLORTABLE_CAPABILITYSET
        /// Problem: field not implemented in SDK
        /// </summary>
        /// <returns>TS_COLORCACHE_CAPABILITYSET type Capability</returns>
        public TS_COLORCACHE_CAPABILITYSET CreateColorTableCapSet()
        {
            TS_COLORCACHE_CAPABILITYSET colorSet = new TS_COLORCACHE_CAPABILITYSET();
            //rawData[4:5]colorTableCacheSize (2 bytes):  A 16-bit, unsigned integer. 
            //The number of entries in the color table cache (each entry stores a color table). 
            //This value MUST be ignored during capability exchange and is assumed to be 0x0006.
            colorSet.rawData = new byte[] { 10, 0, 8, 0, 6, 0, 0, 0 };
            return colorSet;
        }

        /// <summary>
        /// Create a TS_RAIL_CAPABILITYSET type Capability, 2.2.1.1.2 in RDPERP
        /// Problem: field not implemented in SDK
        /// </summary>
        /// <param name="programSupport">Set to 1 if the client/server is capable of supporting Remote Programs; 
        /// set to 0 otherwise.</param>
        /// <param name="dlbSupport">Set to 1 if the client/server is capable of supporting Docked Language Bar for 
        /// Remote Programs; set to 0 otherwise. This flag MUST be set to 0 if TS_RAIL_LEVEL_SUPPORTED is 0.</param>
        /// <returns>TS_RAIL_CAPABILITYSET type Capability</returns>
        public TS_RAIL_CAPABILITYSET CreateTSRailCapSet(bool programSupport, bool dlbSupport)
        {
            TS_RAIL_CAPABILITYSET railSet = new TS_RAIL_CAPABILITYSET();
            railSet.rawData = new byte[] { 23, 0, 8, 0, 3, 0, 0, 0 };//<err>
            //railSet.rawData = new byte[] { 23, 0, 8, 0, 0, 0, 0, 0 };
            //CapabilitySetType (2 bytes)
            //LengthCapability (2 bytes)
            //RailSupportLevel (4 bytes)
            if (programSupport)//<err>
                railSet.rawData[4] |= 0x80;//TS_RAIL_LEVEL_SUPPORTED
            if (dlbSupport)
                railSet.rawData[4] |= 0x40;//TS_RAIL_LEVEL_DOCKED_LANGBAR_SUPPORTED
            return railSet;
        }

        #endregion "

    }
}
