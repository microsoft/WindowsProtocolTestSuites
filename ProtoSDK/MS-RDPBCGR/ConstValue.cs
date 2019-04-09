// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Define const values used in this project.
    /// </summary>
    public static class ConstValue
    {
        #region internal member
        #region TransportStack
        internal const Int32 MAXCONNECTIONS = (Int32)100;
        internal const int BUFFERSIZE = 5000;
        internal const int PORT = 3389;
        #endregion


        #region X224 Connection Confirm
        internal const ushort SOURCE_REFERENCE = 0x3412;
        #endregion


        #region MCSConnect Initial
        /// <summary>
        /// The size of field clientName of 2.2.1.3.2 Client Core Data (TS_UD_CS_CORE)
        /// </summary>
        internal const int CLIENT_CORE_DATA_CLIENT_NAME_SIZE = 32;

        /// <summary>
        /// The size of field imeFileName of 2.2.1.3.2 Client Core Data (TS_UD_CS_CORE)
        /// </summary>
        internal const int CLIENT_CORE_DATA_IME_FILE_NAME_SIZE = 64;

        /// <summary>
        /// The size of field clientDigProductId of 2.2.1.3.2 Client Core Data (TS_UD_CS_CORE)
        /// </summary>
        internal const int CLIENT_CORE_DATA_CLIENT_DIG_PRODUCT_ID_SIZE = 64;

        /// <summary>
        /// The size of field name of 2.2.1.3.4.1 Channel Definition Structure (CHANNEL_DEF)
        /// </summary>
        internal const int CHANNEL_DEF_NAME_SIZE = 8;

        /// <summary>
        /// The default build number of the client.
        /// </summary>
        internal const uint CLIENT_BUILD_DEFAULT = 0x0A28;

        /// <summary>
        /// The version of TPKT header.
        /// </summary>
        internal const byte TPKT_HEADER_VERSION = 0x03;

        /// <summary>
        /// The length of data type credit of x224 header.
        /// </summary>
        internal const byte X224_DATA_TYPE_LENGTH = 0x02;

        /// <summary>
        /// The type credit of x224 header.
        /// </summary>
        internal const byte X224_CONNECTION_REQUEST_TYPE = 0xE0;

        /// <summary>
        /// The type credit of x224 header.
        /// </summary>
        internal const byte X224_DATA_TYPE = 0xF0;

        /// <summary>
        /// The eot field of data type credit of x224 header.
        /// </summary>
        internal const byte X224_DATA_TYPE_EOT = 0x80;

        /// <summary>
        /// The default value of desktop width.
        /// </summary>
        internal const ushort DESKTOP_WIDTH_DEFAULT = 0x0400;

        /// <summary>
        /// The default value of desktop height.
        /// </summary>
        internal const ushort DESKTOP_HEIGHT_DEFAULT = 0x0300;

        /// <summary>
        /// Used by SASSequence field of TS_UD_CS_CORE
        /// </summary>
        internal const ushort RNS_UD_SAS_DEL = 0xAA03;

        /// <summary>
        /// Used by SASSequence field of TS_UD_CS_CORE
        /// </summary>
        internal const ushort LOCALE_ENGLISH_UNITED_STATES = 0x0409;

        /// <summary>
        /// The default number of function keys on the keyboard.
        /// </summary>
        internal const uint KEYBOARD_FUNCTION_KEY_NUMBER_DEFAULT = 0x0C;

        /// <summary>
        /// The default value of the client product ID.
        /// </summary>
        internal const ushort CLIENT_PRODUCT_ID_DEFAULT = 0x01;

        #region targetParameters
        /// <summary>
        /// The maxChannelIds field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long TARGET_PARAMETERS_MAX_CHANNEL_IDS = 34;

        /// <summary>
        /// The maxUserIds field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long TARGET_PARAMETERS_MAX_USER_IDS = 2;

        /// <summary>
        /// The maxTokenIds field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long TARGET_PARAMETERS_MAX_TOKEN_IDS = 0;

        /// <summary>
        /// The numPriorities field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long TARGET_PARAMETERS_NUM_PRIORITIES = 1;

        /// <summary>
        /// The minThroughput field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long TARGET_PARAMETERS_MIN_THROUGHPUT = 0;

        /// <summary>
        /// The maxHeight field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long TARGET_PARAMETERS_MAX_HEIGHT = 1;

        /// <summary>
        /// The maxMCSPDUsize field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long TARGET_PARAMETERS_MAX_MCS_PDU_SIZE = 65535;

        /// <summary>
        /// The protocolVersion field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long TARGET_PARAMETERS_PROTOCOL_VERSION = 2;
        #endregion targetParameters

        #region minimumParameters
        /// <summary>
        /// The maxChannelIds field of minimumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MINIMUM_PARAMETERS_MAX_CHANNEL_IDS = 1;

        /// <summary>
        /// The maxUserIds field of minimumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MINIMUM_PARAMETERS_MAX_USER_IDS = 1;

        /// <summary>
        /// The maxTokenIds field of minimumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MINIMUM_PARAMETERS_MAX_TOKEN_IDS = 1;

        /// <summary>
        /// The numPriorities field of minimumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MINIMUM_PARAMETERS_NUM_PRIORITIES = 1;

        /// <summary>
        /// The minThroughput field of minimumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MINIMUM_PARAMETERS_MIN_THROUGHPUT = 0;

        /// <summary>
        /// The maxHeight field of minimumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MINIMUM_PARAMETERS_MAX_HEIGHT = 1;

        /// <summary>
        /// The maxMCSPDUsize field of minimumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MINIMUM_PARAMETERS_MAX_MCS_PDU_SIZE = 1056;

        /// <summary>
        /// The protocolVersion field of minimumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MINIMUM_PARAMETERS_PROTOCOL_VERSION = 2;
        #endregion minimumParameters

        #region maximumParameters
        /// <summary>
        /// The maxChannelIds field of maximumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MAXIMUM_PARAMETERS_MAX_CHANNEL_IDS = 65535;

        /// <summary>
        /// The maxUserIds field of maximumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MAXIMUM_PARAMETERS_MAX_USER_IDS = 65535;

        /// <summary>
        /// The maxTokenIds field of maximumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MAXIMUM_PARAMETERS_MAX_TOKEN_IDS = 65535;

        /// <summary>
        /// The numPriorities field of maximumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MAXIMUM_PARAMETERS_NUM_PRIORITIES = 1;

        /// <summary>
        /// The minThroughput field of maximumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MAXIMUM_PARAMETERS_MIN_THROUGHPUT = 0;

        /// <summary>
        /// The maxHeight field of maximumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MAXIMUM_PARAMETERS_MAX_HEIGHT = 1;

        /// <summary>
        /// The maxMCSPDUsize field of maximumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MAXIMUM_PARAMETERS_MAX_MCS_PDU_SIZE = 65535;

        /// <summary>
        /// The protocolVersion field of maximumParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long MAXIMUM_PARAMETERS_PROTOCOL_VERSION = 2;
        #endregion maximumParameters

        /// <summary>
        /// The client-to-server H.221 non-standard key which MUST be embedded at
        /// the userData field of the GCC Conference Create Request.
        /// </summary>
        internal const string H221_NON_STANDARD_KEY = "Duca";

        /// <summary>
        /// The default value of GCC Conference create request field conferenceName.
        /// </summary>
        internal const string GCC_CONFERENCE_NAME_DEFAULT = "1";

        /// <summary>
        /// The default value of MCS Connect initial field calledDomainSelector.
        /// </summary>
        internal static readonly byte[] MCS_CALLED_DOMAIN_SELECTOR_DEFAULT = new byte[] { 0x01 };

        /// <summary>
        /// The default value of MCS Connect initial field callingDomainSelector.
        /// </summary>
        internal static readonly byte[] MCS_CALLING_DOMAIN_SELECTOR_DEFAULT = new byte[] { 0x01 };

        /// <summary>
        /// The attribute ID of MCS Connect Initial PDU.
        /// </summary>
        internal static readonly int[] MCS_ATTRIBUTE_ID = new int[] { 0, 20, 124, 0, 1 };
        #endregion MCSConnect Initial


        #region MCSConnect Response
        #region Public const value
        public const UInt16 IO_CHANNEL_ID = 1003;
        public const UInt16 MCS_MESSAGE_CHANNEL_ID = 1008;
        public const UInt16 ID_MAX = 65535;
        public const UInt16 ID_LessMax = 65534;
        public const UInt16 ID_ONE = 1;
        public const UInt16 ID_ZERO = 0;
        #endregion
        internal const int X224_CONNECTION_COMFIRM_TYPE = 0xd0;
        internal const int MCS_RESPONSE_RESULT = 0;
        internal const int MCS_RESPONSE_CALLED_CONNECTED_ID = 0;
        internal const ushort GCC_RESPONSE_NODEID = 31219;
        internal const byte GCC_RESPONSE_TAG = 1;
        internal const byte GCC_RESPONSE_RESULT = 0;
        internal const string H221_KEY = "McDn";
        internal const uint CHANNEL_ID_SIZE = 2;
        internal const int PAD_SIZE = 2;
        internal const UInt16 USER_CHANNEL_ID = 1007;
        internal const UInt16 SERVER_CHANNEL_ID = 1002;
        internal const UInt16 RDPDR_CHANNEL_ID = 1004;
        internal const UInt16 CLIPRDR_CHANNEL_ID = 1005;
        internal const UInt16 RDPSND_CHANNEL_ID = 1006;
        internal const UInt16 RDP_STORED_ID = 1009;
        internal const UInt16 MAX_CHANNLE_ID = 2000;

        /// <summary>
        /// The maxChannelIds field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long DOMAIN_PARAMETERS_MAX_CHANNEL_IDS = 34;

        /// <summary>
        /// The maxUserIds field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long DOMAIN_PARAMETERS_MAX_USER_IDS = 3;

        /// <summary>
        /// The maxTokenIds field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long DOMAIN_PARAMETERS_MAX_TOKEN_IDS = 0;

        /// <summary>
        /// The numPriorities field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long DOMAIN_PARAMETERS_NUM_PRIORITIES = 1;

        /// <summary>
        /// The minThroughput field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long DOMAIN_PARAMETERS_MIN_THROUGHPUT = 0;

        /// <summary>
        /// The maxHeight field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long DOMAIN_PARAMETERS_MAX_HEIGHT = 1;

        /// <summary>
        /// The maxMCSPDUsize field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long DOMAIN_PARAMETERS_MAX_MCS_PDU_SIZE = 65528;

        /// <summary>
        /// The protocolVersion field of targetParameters in MCS Connect Initial PDU.
        /// </summary>
        internal const long DOMAIN_PARAMETERS_PROTOCOL_VERSION = 2;


        /// <summary>
        /// the default value of the left in MONITOR_DEF
        /// </summary>
        internal const uint PRIMARY_MONITOR_DEF_LEFT = 0;

        /// <summary>
        /// the default value of the right in MONITOR_DEF
        /// </summary>
        internal const uint PRIMARY_MONITOR_DEF_RIGHT = DESKTOP_WIDTH_DEFAULT - 1;

        /// <summary>
        /// the default value of the top in MONITOR_DEF
        /// </summary>
        internal const uint PRIMARY_MONITOR_DEF_TOP = 0;

        /// <summary>
        /// the default value of the bottom in MONITOR_DEF
        /// </summary>
        internal const uint PRIMARY_MONITOR_DEF_BOTTOM = DESKTOP_HEIGHT_DEFAULT - 1;

        #endregion


        #region Erect Domain Request
        /// <summary>
        /// The subHeight field of the MCS Erect Domain Request PDU.
        /// </summary>
        internal const int ERECT_DOMAIN_REQUEST_SUBHEIGHT = 0x01;

        /// <summary>
        /// The subInterval field of the MCS Erect Domain Request PDU.
        /// </summary>
        internal const int ERECT_DOMAIN_REQUEST_SUBINTERVAL = 0x01;

        internal const int ATTACH_USER_CONFIRM_RESULT = 0;

        #endregion Erect Domain Request


        #region Server Attach User Confirm PDU
        internal const UInt16 USER_ID = 1007;
        #endregion


        #region Security Exchange PDU
        /// <summary>
        /// The DataPriority field of SendDataRequest.
        /// </summary>
        internal const int SEND_DATA_REQUEST_PRIORITY = 1;

        /// <summary>
        /// The size of the client random number.
        /// </summary>
        internal const int CLIENT_RANDOM_SIZE = 32;

        internal const int SERVER_RANDOM_SIZE = 32;

        /// <summary>
        /// The Segmentation field of SendDataRequest.
        /// </summary>
        internal static readonly Segmentation SEND_DATA_REQUEST_SEGMENTATION
            = new Segmentation(new bool[] { true, true });
        #endregion Security Exchange PDU


        #region Client Info PDU
        /// <summary>
        /// The size of StandardName field of 2.2.1.11.1.1.1.1 Time Zone Information (TS_TIME_ZONE_INFORMATION).
        /// </summary>
        internal const int TIME_ZONE_STANDARD_NAME_SIZE = 64;

        /// <summary>
        /// The size of DaylightName field of 2.2.1.11.1.1.1.1 Time Zone Information (TS_TIME_ZONE_INFORMATION).
        /// </summary>
        internal const int TIME_ZONE_DAYLIGHT_NAME_SIZE = 64;

        /// <summary>
        /// The Bias field of 2.2.1.11.1.1.1.1 Time Zone Information (TS_TIME_ZONE_INFORMATION).
        /// </summary>
        internal const int TIME_ZONE_BIAS_DEFAULT = 480;

        /// <summary>
        /// The StandardName field of 2.2.1.11.1.1.1.1 Time Zone Information (TS_TIME_ZONE_INFORMATION).
        /// </summary>
        internal const string TIME_ZONE_STANDARD_NAME_DEFAULT = "Pacific Standard Time";

        /// <summary>
        /// The DaylightName field of 2.2.1.11.1.1.1.1 Time Zone Information (TS_TIME_ZONE_INFORMATION).
        /// </summary>
        internal const string TIME_ZONE_DAYLIGHT_NAME_DEFAULT = "Pacific Daylight Time";

        /// <summary>
        /// The DaylightBias field of 2.2.1.11.1.1.1.1 Time Zone Information (TS_TIME_ZONE_INFORMATION).
        /// </summary>
        internal const int TIME_ZONE_DAYLIGHT_BIAS_DEFAULT = -60;

        /// <summary>
        /// The version of the FIPS Header.
        /// </summary>
        internal const byte TSFIPS_VERSION1 = 0x01;

        /// <summary>
        /// When Enhanced RDP Security is in effect, the client random is assumed to be an array of 16 zero bytes.
        /// </summary>
        internal const int RECONNECT_CLIENT_RANDOM_LENGTH = 32;
        #endregion Client Info PDU


        #region Server Licensing Error PDU
        internal const int LICENSING_PACKET_SIZE = 0x10;
        internal const int LICENSING_BINARY_BLOB_LENGTH = 0;
        #endregion


        #region Confirm Active PDU
        /// <summary>
        /// The preferredBitsPerPixel field of 2.2.7.1.2 Bitmap Capability Set (TS_BITMAP_CAPABILITYSET).
        /// </summary>
        internal const ushort BITMAP_CAP_BITS_PER_PIXEL_DEFAULT = 24;

        /// <summary>
        /// Indicates the client support this feature in 2.2.7.1.2 Bitmap Capability Set (TS_BITMAP_CAPABILITYSET).
        /// </summary>
        internal const ushort BITMAP_CAP_SUPPORT_FEATURE = 0x0001;

        /// <summary>
        /// The terminalDescriptor field of 2.2.7.1.3 Order Capability Set (TS_ORDER_CAPABILITYSET).
        /// </summary>
        internal const int ORDER_CAP_TERMINAL_DESCRIPTOR = 16;

        /// <summary>
        /// The desktopSaveXGranularity field of 2.2.7.1.3 Order Capability Set (TS_ORDER_CAPABILITYSET).
        /// </summary>
        internal const ushort ORDER_CAP_DESKTOP_X = 1;

        /// <summary>
        /// The desktopSaveYGranularity field of 2.2.7.1.3 Order Capability Set (TS_ORDER_CAPABILITYSET).
        /// </summary>
        internal const ushort ORDER_CAP_DESKTOP_Y = 20;

        /// <summary>
        /// The desktopSaveSize field of 2.2.7.1.3 Order Capability Set (TS_ORDER_CAPABILITYSET).
        /// </summary>
        internal const uint ORDER_CAP_DESKTOP_SIZE_DEFAULT = 230400;

        /// <summary>
        /// The number of ushort type in 2.2.7.1.3 Order Capability Set (TS_ORDER_CAPABILITYSET).
        /// </summary>
        internal const uint ORDER_CAP_USHORT_COUNT = 14;

        /// <summary>
        /// The number of ushort type in 2.2.7.1.3 Order Capability Set (TS_ORDER_CAPABILITYSET).
        /// </summary>
        internal const uint ORDER_CAP_UINT_COUNT = 3;

        /// <summary>
        /// The maximumOrderLevel field of 2.2.7.1.3 Order Capability Set (TS_ORDER_CAPABILITYSET).
        /// </summary>
        internal const ushort ORD_LEVEL_1_ORDERS = 1;

        /// <summary>
        /// The orderSupport field of 2.2.7.1.3 Order Capability Set (TS_ORDER_CAPABILITYSET).
        /// </summary>
        internal static readonly byte[] ORDER_CAP_ORDER_SUPPORT_DEFAULT =
            new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x01,
                         0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x01,
                         0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00,
                         0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00};

        internal static readonly byte[] TERMINALDESCRIPTOR =
            new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// The NumCellCaches field of 2.2.7.1.4.2 Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2).
        /// </summary>
        internal const byte BITMAP_CACHE_NUM_CELL_DEFAULT = 3;

        /// <summary>
        /// The NumCellCaches field of 2.2.7.1.4.2 Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2).
        /// </summary>
        internal const ushort BITMAP_CACHE_CELL_NUM = 5;

        /// <summary>
        /// The BitmapCache1CellInfo field of 2.2.7.1.4.2 Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2).
        /// </summary>
        internal const uint BITMAP_CACHE_CELL1_VALUE = 120;

        /// <summary>
        /// The BitmapCache2CellInfo field of 2.2.7.1.4.2 Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2).
        /// </summary>
        internal const uint BITMAP_CACHE_CELL2_VALUE = 120;

        /// <summary>
        /// The BitmapCache3CellInfo field of 2.2.7.1.4.2 Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2).
        /// </summary>
        internal const uint BITMAP_CACHE_CELL3_VALUE = 2555;

        /// <summary>
        /// The Pad3 field of 2.2.7.1.4.2 Revision 2 (TS_BITMAPCACHE_CAPABILITYSET_REV2).
        /// </summary>
        internal static readonly byte[] BITMAP_CACHE_PAD3 =
            new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// The controlInterest field of 2.2.7.2.2 Control Capability Set (TS_CONTROL_CAPABILITYSET).
        /// </summary>
        internal const ushort CONTROLPRIORITY_NEVER = 0x0002;

        /// <summary>
        /// The colorPointerCacheSize field of 2.2.7.1.5 Pointer Capability Set (TS_POINTER_CAPABILITYSET).
        /// </summary>
        internal const ushort POINTER_CAP_COLOR_SIZE_DEFAULT = 20;

        /// <summary>
        /// The pointerCacheSize field of 2.2.7.1.5 Pointer Capability Set (TS_POINTER_CAPABILITYSET).
        /// </summary>
        internal const ushort POINTER_CAP_POINTER_SIZE_DEFAULT = 21;

        /// <summary>
        /// The size of imeFileName field of 2.2.7.1.6 Input Capability Set (TS_INPUT_CAPABILITYSET).
        /// </summary>
        internal const uint INPUT_CAP_IME_FILENAME_SIZE = 64;

        /// <summary>
        /// The number of GlyphCache field of 2.2.7.1.8 Glyph Cache Capability Set (TS_GLYPHCACHE_CAPABILITYSET).
        /// </summary>
        internal const uint CLYPH_CACHE_CAP_CLYPH_CACHE_NUM = 10;

        /// <summary>
        /// The value of CacheEntries field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_256 = 256;

        /// <summary>
        /// The value of CacheEntries field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_254 = 254;

        /// <summary>
        /// The value of CacheEntries field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_ENTRY_NUM_64 = 64;

        /// <summary>
        /// The value of CacheMaximumCellSize field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_CELL_SIZE_4 = 4;

        /// <summary>
        /// The value of CacheMaximumCellSize field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_CELL_SIZE_8 = 8;

        /// <summary>
        /// The value of CacheMaximumCellSize field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_CELL_SIZE_16 = 16;

        /// <summary>
        /// The value of CacheMaximumCellSize field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_CELL_SIZE_32 = 32;

        /// <summary>
        /// The value of CacheMaximumCellSize field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_CELL_SIZE_64 = 64;

        /// <summary>
        /// The value of CacheMaximumCellSize field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_CELL_SIZE_128 = 128;

        /// <summary>
        /// The value of CacheMaximumCellSize field of 2.2.7.1.8.1 Cache Definition (TS_CACHE_DEFINITION).
        /// </summary>
        internal const ushort CLYPH_CACHE_CAP_CACHE_CELL_SIZE_256 = 256;

        /// <summary>
        /// The number of ushort type in 2.2.7.1.8 Glyph Cache Capability Set (TS_GLYPHCACHE_CAPABILITYSET).
        /// </summary>
        internal const uint CLYPH_CACHE_CAP_USHORT_COUNT = 26;

        /// <summary>
        /// The max value of offscreenCacheSize field of 2.2.7.1.9 Offscreen Bitmap Cache Capability Set.
        /// </summary>
        internal const ushort OFFSCREEN_CAP_MAX_CACHE_SIZE = 7680;

        /// <summary>
        /// The value of offscreenCacheEntries field of 2.2.7.1.9 Offscreen Bitmap Cache Capability Set.
        /// </summary>
        internal const ushort OFFSCREEN_CAP_CACHE_ENTRY_NUM = 64;

        /// <summary>
        /// The default chunk size defined in section 2.2.6.1 Virtual Channel PDU
        /// </summary>
        internal const int CHANNEL_CHUNK_LENGTH_DEFAULT = 1600;

        /// <summary>
        /// The value of fontSupportFlags field of section 2.2.7.2.5 Font Capability Set (TS_FONT_CAPABILITYSET).
        /// </summary>
        internal const ushort FONTSUPPORT_FONTLIST = 0x0001;

        /// <summary>
        /// The value of MaxRequestSize field of section 2.2.7.2.6 Multifragment Update 
        /// Capability Set (TS_MULTIFRAGMENTUPDATE_CAPABILITYSET).
        /// </summary>
        internal const uint MULTIFRAGMENT_CAP_MAX_REQUEST_SIZE = 0x00005008;

        /// <summary>
        /// The sourceDescriptor field of 2.2.1.13.2.1 Confirm Active PDU Data (TS_CONFIRM_ACTIVE_PDU).
        /// </summary>
        internal static readonly byte[] CONFIRM_SOURCE_DESCRIPTOR =
            new byte[] { 0x4D, 0x53, 0x54, 0x53, 0x43, 0x00 };//'M', 'S', 'T', 'S', 'C', '\0'

        internal static readonly byte[] DEMAND_SOURCE_DESCRIPTOR =
            new byte[] { 0x52, 0x44, 0x50, 0x00 }; //'R', 'D', 'P', '\0' 

        #endregion Confirm Active PDU


        #region Demand Active PDU
        internal const uint SHAREID = 66538;
        internal const ushort SHAREID_LOW = 0x0100;
        internal static readonly byte[] SOURCE_DESCRIPTOR = new byte[] { 0x00 };
        #endregion


        #region Auto Reconnect Status PDU
        internal const uint ARC_STATUS = 0;
        #endregion


        #region Save Session Info PDU
        internal const uint SAVE_SESSION_INFO_INFOTYPE_LOGON = 0;
        internal const uint SAVE_SESSION_INFO_INFOTYPE_LOGON_LONG = 0x00000001;
        internal const uint SAVE_SESSION_INFO_INFOTYPE_LOGON_PLAINNOTIFY = 0x00000002;
        internal const uint SAVE_SESSION_INFO_INFOTYPE_LOGON_EXTENDED_INF = 0x00000003;
        #endregion


        #region Persistent Key List PDU
        /// <summary>
        /// The default value of entries field of 2.2.1.17.1 Persistent Key List PDU Data 
        /// (TS_BITMAPCACHE_PERSISTENT_LIST_PDU).
        /// </summary>
        internal static readonly TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY[] PERSISTENT_KEY_LIST_ENTRY_DEFAULT =
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY[] {
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x1651, 0x2948),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x9c89, 0xa9cd),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xbdb7, 0x6db4),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xaf64, 0xc3bc),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xfdfa, 0xba6e),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x06de, 0x3a56),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xb654, 0x9ebf),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x98c0, 0xe963),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x3550, 0xdb0e),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x2d18, 0x4430),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x2d18, 0x4430),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xb601, 0x5110),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x596c, 0x2919),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xa7e7, 0x0446),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xdd4a, 0xbb81),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x3c4e, 0x7248),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x4a4b, 0x4832),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xf256, 0xc981),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x543d, 0x9d86),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x360a, 0x63f8),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x4b41, 0xa8ec),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xb4b6, 0x43a6),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xa2ff, 0x3c29),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0xb080, 0x6e0d),
            new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(0x3154, 0x8aaa), };
        #endregion Persistent Key List PDU


        #region Font List PDU
        /// <summary>
        /// The value of listFlags field of section 2.2.1.18.1 Font List PDU Data (TS_FONT_LIST_PDU).
        /// </summary>
        internal const ushort FONTLIST_FIRST = 0x0001;

        /// <summary>
        /// The value of listFlags field of section 2.2.1.18.1 Font List PDU Data (TS_FONT_LIST_PDU).
        /// </summary>
        internal const ushort FONTLIST_LAST = 0x0002;

        /// <summary>
        /// The value of entrySize field of section 2.2.1.18.1 Font List PDU Data (TS_FONT_LIST_PDU).
        /// </summary>
        internal const ushort FONTLIST_ENTRY_SIZE = 0x0032;
        #endregion Font List PDU


        #region Font Map PDU
        internal const ushort FONTMAP_FIRST = 0x0001;
        internal const ushort FONTMAP_LAST = 0x0002;
        internal const ushort FONTMAP_ENTRY_SIZE = 0x0004;
        #endregion


        #region Keyboard Status PDU
        internal const ushort UNITID = 0;
        internal const ushort LED_FLAGS = 0x0002;


        internal const uint IME_OPEN = 0;
        internal const uint IME_CONV_MODE = 0;
        #endregion


        #region Slow-Path Input Event
        /// <summary>
        /// The value of numberEvents field of section 2.2.8.1.1.3.1 Slow-Path Input Event(TS_INPUT_PDU_DATA).
        /// </summary>
        internal const byte NUMBER_EVENTS = 0x05;

        /// <summary>
        /// The value of keycode field of section 2.2.8.1.1.3.1.1.1 Keyboard Event(TS_KEYBOARD_EVENT).
        /// </summary>
        internal const ushort KEY_CODE = 0x1f;

        /// <summary>
        /// The value of unicodeCode field of section 2.2.8.1.1.3.1.1.2 Unicode Keyboard Event
        /// (TS_UNICODE_KEYBOARD_EVENT).
        /// </summary>
        internal const ushort UNICODE_CODE = 0x0020;

        /// <summary>
        /// The value of x_pos field of section 2.2.8.1.1.3.1.1.3 Mouse Event (TS_POINT_EVENT).
        /// </summary>
        internal const ushort X_POS = 0x150;

        /// <summary>
        /// The value of y_pos field of section 2.2.8.1.1.3.1.1.3 Mouse Event (TS_POINT_EVENT).
        /// </summary>
        internal const ushort Y_POS = 0x200;
        #endregion TS Input PDU


        #region Fast-Path Input Event
        /// <summary>
        /// The value of numberEvents field of section 2.2.8.1.2 Fast-Path Input Event(TS_FP_INPUT_PDU).
        /// TS_FP_KEYBOARD_EVENT
        /// TS_FP_UNICODE_KEYBOARD_EVENT
        /// TS_FP_POINTER_EVENT
        /// TS_FP_POINTERX_EVENT
        /// TS_FP_SYNC_EVENT
        /// TS_FP_QOETIMESTAMP_EVENT
        /// </summary>
        internal const byte FP_NUMBER_EVENTS = 0x06;

        /// <summary>
        /// The value of keyboardEvent.keycode field of section 2.8.2.1.2.2.1 
        /// Fast-Path Keyboard Event(TS_FP_KEYBOARD_EVENT).
        /// </summary>
        internal const byte FP_KEY_CODE = 0x1F;

        /// <summary>
        /// The value of unicodeEvent.unicodecode field of section 2.8.2.1.2.2.2 
        /// Fast-Path Unicode Keyboard Event(TS_FP_UNICODE_KEYBOARD_EVENT).
        /// </summary>
        internal const byte FP_UNICODE_CODE = 0x20;
        #endregion Fast-Path Input Event


        #region Basic Output PDU
        internal const uint BITS_PER_PIXEL_32 = 32;
        #endregion


        #region Slow-Path Output
        internal const UInt32 NUMBER_COLORS = 256;
        internal const byte PALETTE_ENTRY_RED = 1;
        internal const byte PALETTE_ENTRY_GREEN = 10;
        internal const byte PALETTE_ENTRY_BLUE = 100;

        internal const UInt16 NUMBER_RECT = 2;
        internal const UInt16 DEST_LEFT = 10;
        internal const UInt16 DEST_TOP = 10;
        internal const UInt16 DEST_RIGHT = 200;
        internal const UInt16 DEST_BOTTOM = 200;
        internal const UInt16 WIDTH = 191;
        internal const UInt16 HEIGHT = 191;
        internal const UInt16 BITSPERPIXEL = 16;

        internal const UInt16 CB_COMP_FIRST_ROW_SIZE = 0;

        internal const UInt16 PTR_X_POS = 50;
        internal const UInt16 PTR_Y_POS = 50;

        internal const UInt32 SYSPTR_NULL = 0;
        internal const UInt32 SYSPTR_DEFAULT = 0x00007f00;

        internal const UInt32 DURATION = 2000;
        internal const UInt32 FREQUENCY = 2;

        internal const UInt16 CACHE_INDEX = 0;
        internal const UInt16 HOTSPOT_X_POS = 400;
        internal const UInt16 HOTSPOT_Y_POS = 300;
        internal const UInt16 PTR_HEIGHT = 32;
        internal const UInt16 LEN_AND_MASK = 2;
        internal const UInt16 LEN_XOR_MASK = 22;

        internal const UInt16 XOR_BPP = 8;

        internal const ushort BITMAP_DATA_SIZE_DEFAULT = 26;
        internal const ushort COLOR_PTR_UPDATE_SIZE_DEFAULT = 39;
        internal const ushort CACHED_PTR_ATTRIBUTE_SIZE_DEFAULT = 2;
        internal const byte BPP = 16;
        internal const ushort SURF_CMDS_SIZE_DEFAULT = 20;

        #endregion


        #region Refresh Rect PDU
        /// <summary>
        /// The pad3Octects of TS_REFRESH_RECT_PDU of 2.2.11.2.1 Client Refresh Rect PDU Data (TS_REFRESH_RECT_PDU)
        /// </summary>       
        internal static readonly byte[] PAD_3_OCTECTS = new byte[] { 0, 0, 0 };

        /// <summary>
        /// The default value of areasToRefresh field of 2.2.11.2.1 Client Refresh Rect PDU Data (TS_REFRESH_RECT_PDU)
        /// </summary>       
        internal static readonly TS_RECTANGLE16[] AREASTOFRESH_DEFAULT =
             new TS_RECTANGLE16[] { new TS_RECTANGLE16(0, 586, 591, 1023) };
        #endregion Refresh Rect PDU


        #region Suppress Output PDU
        /// <summary>
        /// The default value of desktopRect field of 2.2.11.3.1 Suppress Output PDU data (TS_SUPPRESS_OUTPUT_PDU)
        /// </summary>       
        internal static readonly TS_RECTANGLE16 DESKTOP_RECT_DEFAULT = new TS_RECTANGLE16(100, 140, 1015, 14);
        #endregion Suppress Output PDU


        #region Server Redirection PDU
        internal const UInt16 REDIRECTION_FLAG = 0x400;
        internal const int REDIRECTION_PACKET_PART_SIZE = 60;
        #endregion


        #region Disconnect
        /// <summary>
        /// The reason code of rn-user-requested (3) reason codes for MCS Disconnect Provider Ultimatum PDU.
        /// </summary>
        internal const ushort RN_USER_REQUESTED = 3;
        #endregion Disconnect


        #region Encryption
        /// <summary>
        /// Pad1 = 0x36 repeated 40 times to give 320 bits.
        /// </summary>
        internal static readonly byte[] NON_FIPS_PAD1 = new byte[] { 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36,
                                                                     0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36,
                                                                     0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36,
                                                                     0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36,
                                                                     0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36, 0x36,};

        /// <summary>
        /// Pad2 = 0x5C repeated 48 times to give 384 bits.
        /// </summary>
        internal static readonly byte[] NON_FIPS_PAD2 = new byte[] { 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C,
                                                                     0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C,
                                                                     0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C,
                                                                     0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C,
                                                                     0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C,
                                                                     0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C, 0x5C,};

        /// <summary>
        /// The salt value of 40-bit keys.
        /// </summary>
        internal static readonly byte[] NON_FIPS_SALT_40BIT = new byte[] { 0xD1, 0x26, 0x9E };

        /// <summary>
        /// The salt value of 56-bit keys.
        /// </summary>
        internal static readonly byte[] NON_FIPS_SALT_56BIT = new byte[] { 0xD1 };

        /// <summary>
        /// PreMasterHash('A'), section 5.3.5.1 Non-FIPS
        /// </summary>
        internal const string NON_FIPS_MASTER_SECRET_A = "A";

        /// <summary>
        /// PreMasterHash('BB'), section 5.3.5.1 Non-FIPS
        /// </summary>
        internal const string NON_FIPS_MASTER_SECRET_BB = "BB";

        /// <summary>
        /// PreMasterHash('CCC'), section 5.3.5.1 Non-FIPS
        /// </summary>
        internal const string NON_FIPS_MASTER_SECRET_CCC = "CCC";

        /// <summary>
        /// PreMasterHash('X'), section 5.3.5.1 Non-FIPS
        /// </summary>
        internal const string NON_FIPS_MASTER_SECRET_X = "X";

        /// <summary>
        /// PreMasterHash('YY'), section 5.3.5.1 Non-FIPS
        /// </summary>
        internal const string NON_FIPS_MASTER_SECRET_YY = "YY";

        /// <summary>
        /// PreMasterHash('ZZZ'), section 5.3.5.1 Non-FIPS
        /// </summary>
        internal const string NON_FIPS_MASTER_SECRET_ZZZ = "ZZZ";

        /// <summary>
        /// The salt value of 56-bit keys.
        /// </summary>
        internal static readonly byte[] TRPLE_DES_IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// The encryption and the decryption keys are updated after 4,096 packets have been sent or received.
        /// </summary>
        internal const int PDU_COUNT_TO_UPDATE_SESSION_KEY = 4096;

        /// <summary>
        /// The pad number for triple des.
        /// </summary>
        internal const int TRIPLE_DES_PAD = 8;

        /// <summary>
        /// The size of signature for validating proprietary certificate.
        /// </summary>
        internal const int PROPRIETARY_CERTIFICATE_SIGNATURE_SIZE = 63;

        /// <summary>
        /// The public exponent of Terminal Services asymmetric key used for 
        /// signing Proprietary Certificates with the RSA algorithm.
        /// Section 5.3.3.1.1 Terminal Services Signing Key.
        /// </summary>
        internal static readonly byte[] PROPRIETARY_CERTIFICATE_EXPONENT = new byte[] { 0x5b, 0x7b, 0x88, 0xc0 };

        /// <summary>
        /// The modulus of Terminal Services asymmetric key used for 
        /// signing Proprietary Certificates with the RSA algorithm.
        /// Section 5.3.3.1.1 Terminal Services Signing Key.
        /// </summary>
        internal static readonly byte[] PROPRIETARY_CERTIFICATE_MODULUS = new byte[] {
                               0x3d, 0x3a, 0x5e, 0xbd, 0x72, 0x43, 0x3e, 0xc9,
                               0x4d, 0xbb, 0xc1, 0x1e, 0x4a, 0xba, 0x5f, 0xcb,
                               0x3e, 0x88, 0x20, 0x87, 0xef, 0xf5, 0xc1, 0xe2,
                               0xd7, 0xb7, 0x6b, 0x9a, 0xf2, 0x52, 0x45, 0x95,
                               0xce, 0x63, 0x65, 0x6b, 0x58, 0x3a, 0xfe, 0xef,
                               0x7c, 0xe7, 0xbf, 0xfe, 0x3d, 0xf6, 0x5c, 0x7d,
                               0x6c, 0x5e, 0x06, 0x09, 0x1a, 0xf5, 0x61, 0xbb,
                               0x20, 0x93, 0x09, 0x5f, 0x05, 0x6d, 0xea, 0x87 };

        /// <summary>
        /// Indicate that the timeout wait for updating session key.
        /// </summary>
        internal static readonly TimeSpan UPDATE_SESSION_KEY_TIMEOUT = new TimeSpan(0, 0, 1);

        /// <summary>
        /// The size of client random values
        /// </summary>
        internal const int CLIENT_RANDOM_NUMBER_SIZE = 32;

        #endregion Encryption


        #region MpInt parameters
        /// <summary>
        /// How many bits a byte has.
        /// </summary>
        internal const int BITS_PER_BYTE = 8;
        #endregion MpInt parameters


        #region Transport properties
        /// <summary>
        /// Indicate the buffer size of the transport.
        /// </summary>
        internal const int TRANSPORT_BUFFER_SIZE = 20480;
        internal const int TRANSPORT_MAX_CONNECTIONS = 100;

        /// <summary>
        /// The frefix of server name for CredSSP.
        /// </summary>
        internal const string CREDSSP_SERVER_NAME_PREFIX = "TERMSRV/";

        /// <summary>
        /// The frefix of server name for CredSSP.
        /// </summary>
        internal const string TIMEOUT_EXCEPTION = "It has been timeout when receiving packets.";
        #endregion Transport properties


        #region Constants in RdpbcgrDecoder
        #region Constants: Specific Field Lengths for Parsers
        /// <summary>
        /// (576 bytes) TS_PLAIN_NOTIFY pad length
        /// </summary>
        internal const UInt16 TS_PLAIN_NOTIFY_PAD_LENGTH = 576;

        /// <summary>
        /// (570 bytes) TS_LOGON_INFO_EXTENDED pad length
        /// </summary>
        internal const UInt16 TS_LOGON_INFO_EXTENDED_PAD_LENGTH = 570;

        /// <summary>
        /// (558 bytes) TS_LOGON_INFO_VERSION_2 pad length
        /// </summary>
        internal const UInt16 TS_LOGON_INFO_VERSION_2_PAD_LENGTH = 558;

        /// <summary>
        /// (512 bytes) TS_LOGON_INFO user name length
        /// </summary>
        internal const UInt16 TS_LOGON_INFO_USER_NAME_LENGTH = 512;

        /// <summary>
        /// (52 bytes) TS_LOGON_INFO domain length
        /// </summary>
        internal const UInt16 TS_LOGON_INFO_DOMAIN_LENGTH = 52;

        /// <summary>
        /// (8 bytes) TS_FP_UPDATE_PDU data signature length
        /// </summary>
        internal const UInt16 TS_FP_UPDATE_PDU_DATA_SIGNATURE_LENGTH = 8;

        /// <summary>
        /// (8 bytes) TS_SECURITY_HEADER data signature length
        /// </summary>
        internal const UInt16 TS_SECURITY_HEADER_DATA_SIGNATURE_LENGTH = 8;

        /// <summary>
        /// (10 bytes) TS_GLYPHCACHE_CAPABILITYSET glyphcache length
        /// </summary>
        internal const UInt16 TS_GLYPHCACHE_CAPABILITYSET_GLYPHCACHE_LENGTH = 10;

        /// <summary>
        /// (64 bytes) TS_INPUT_CAPABILITYSET ime file name length
        /// </summary>
        internal const UInt16 TS_INPUT_CAPABILITYSET_IME_FILE_NAME_LENGTH = 64;

        /// <summary>
        /// (12 bytes) TS_BITMAPCACHE_CAPABILITYSET_REV2 pad3 length
        /// </summary>
        internal const UInt16 TS_BITMAPCACHE_CAPABILITYSET_REV2_PAD3_LENGTH = 12;

        /// <summary>
        /// (32 bytes) TS_ORDER_CAPABILITYSET order support length
        /// </summary>
        internal const UInt16 TS_ORDER_CAPABILITYSET_ORDER_SUPPORT_LENGTH = 32;

        /// <summary>
        /// (16 bytes) ARC_SC_PRIVATE_PACKET arc random bits length
        /// </summary>
        internal const UInt16 ARC_SC_PRIVATE_PACKET_ARC_RANDOM_BITS_LENGTH = 16;

        /// <summary>
        /// (16 bytes) TS_ORDER_CAPABILITYSET terminal descriptor
        /// </summary>
        internal const UInt16 TS_ORDER_CAPABILITYSET_TERMINAL_DESCRIPTOR_LENGTH = 16;

        /// <summary>
        /// (4 bytes) Slow-Path Pointer Update Attribute Data Length for "System Data"
        /// </summary>
        internal const UInt16 TS_PTRMSGTYPE_SYSTEM_DATA_LENGTH = 4;

        /// <summary>
        /// (4 bytes) Slow-Path Pointer Update Attribute Data Length for "Position Data"
        /// </summary>
        internal const UInt16 TS_PTRMSGTYPE_POSITION_DATA_LENGTH = 4;

        /// <summary>
        /// (2 bytes) Slow-Path Pointer Update Attribute Data Length for "Cached Data"
        /// </summary>
        internal const UInt16 TS_PTRMSGTYPE_CACHED_DATA_LENGTH = 2;

        /// <summary>
        /// (7 bytes) TPKT Header and X224 Data Length
        /// </summary>
        internal const UInt16 TPKT_HEADER_AND_X224_DATA_LENGTH = 7;

        /// <summary>
        /// (8 bytes) Gcc Data's offset (in McsConnectResponse User Data)
        /// </summary>
        internal const UInt16 GCC_DATA_OFFSET = 8;

        internal const UInt16 GCC_CCI_DATA_OFFSET = 9;
        #endregion Constants: Specific Field Lengths for Parsers

        #region Constants: PDU type indicators for Decoder Switches
        /// <summary>
        /// Slow-Path PDU indicator's position in raw PDU data
        /// </summary>
        internal const UInt32 SLOW_PATH_PDU_INDICATOR_INDEX = 0x0;

        /// <summary>
        /// Slow-Path PDU indicator's value
        /// </summary>
        internal const byte SLOW_PATH_PDU_INDICATOR_VALUE = 0x03;

        /// <summary>
        /// X224 TPDU type indicator's position in raw PDU data
        /// </summary>
        internal const UInt32 X224_TPDU_TYPE_INDICATOR_INDEX = 0x05;

        /// <summary>
        /// MCS Connect Response PDU indicator's position in raw PDU data
        /// </summary>
        internal const UInt32 MCS_CONNECT_RESPONSE_PDU_INDICATOR_INDEX = 0x07;

        /// <summary>
        /// MCS Connect Response PDU indicator's value
        /// </summary>
        internal const byte MCS_CONNECT_RESPONSE_PDU_INDICATOR_VALUE = 0x7f;

        internal const UInt32 MCS_CONNECT_INITIAL_PDU_INDICATOR_INDEX = 0x07;
        internal const byte MCS_CONNECT_INITIAL_PDU_INDICATOR_VALUE = 0x7f;
        #endregion Constants: PDU type indicators for Decoder Switches

        #region Constants: MCS Domain PDU Elem Names (from AsnCodec)
        internal const string MCS_DOMAIN_PDU_NAME_ERECT_DOMAIN_REQUEST = "erectDomainRequest";
        internal const string MCS_DOMAIN_PDU_NAME_ATTACH_USER_REQUEST = "attachUserRequest";
        internal const string MCS_DOMAIN_PDU_NAME_CHANNEL_JOIN_REQUEST = "channelJoinRequest";
        internal const string MCS_DOMAIN_PDU_NAME_SEND_DATA_REQUEST = "sendDataRequest";

        /// <summary>
        /// MCS Domain PDU Elem Name for Channel Join Confirm
        /// </summary>
        internal const string MCS_DOMAIN_PDU_NAME_CHANNEL_JOIN_CONFIRM = "channelJoinConfirm";

        /// <summary>
        /// MCS Domain PDU Elem Name for Send Data Indication
        /// </summary>
        internal const string MCS_DOMAIN_PDU_NAME_SEND_DATA_INDICATION = "sendDataIndication";

        /// <summary>
        /// MCS Domain PDU Elem Name for Attach User Confirm
        /// </summary>
        internal const string MCS_DOMAIN_PDU_NAME_ATTACH_USER_CONFIRM = "attachUserConfirm";

        /// <summary>
        /// MCS Domain PDU Elem Name for Disconnect Provider Ultimatum
        /// </summary>
        internal const string MCS_DOMAIN_PDU_NAME_DISCONNECT_PROVIDER_ULTIMATUM = "disconnectProviderUltimatum";
        #endregion Constants: MCS Domain PDU Elem Names (from AsnCodec)

        #region Constants: Values for Bit Operations
        /// <summary>
        /// (1001) Filter Value to parse Channel Initiator
        /// </summary>
        internal const UInt16 CHANNEL_INITIATOR_FILTER = (UInt16)1001;

        /// <summary>
        /// (0x7f) Filter Value to check the "Most Significant Bit" in byte
        /// </summary>
        internal const byte MOST_SIGNIFICANT_BIT_FILTER = 0x7f;
        #endregion Constants: Values for Bit Operations

        #region Constants: Error Messages
        internal const string ERROR_MESSAGE_DATA_LENGTH_EXCEEDED = "[Decode Error] data length exceeded exception.";
        internal const string ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT =
            "[Decode Error] data length inconsistent with expectation.";
        internal const string ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE = "[Decode Error] data index out of range.";
        internal const string ERROR_MESSAGE_DATA_NULL_REF = "[Decode Error] data is of null-referenced.";
        internal const string ERROR_MESSAGE_ENUM_UNRECOGNIZED = "[Decode Error] unrecognized enum value.";
        internal const string ERROR_MESSAGE_DECRYPTION_FAILED = "[Decode Error] data decryption failed.";
        internal const string ERROR_MESSAGE_NOT_SUPPORTED_X509_CERTIFICATE =
            "[Decode Error] X509 certificate is not supported.";
        internal const string ERROR_MESSAGE_UNRECOGNIZED_PDU = "[Decode Error] PDU unrecognized!";
        internal const string ERROR_MESSAGE_INVALID_GUID = "[Decode Error] Invalid GUID!";
        internal const string ERROR_MESSAGE_INVALID_UNICODE_STRING = "[Decode Error] Invalid Unicode string!";
        #endregion Constants: Error Messages
        #endregion Constants in RdpbcgrDecoder
        #endregion internal member

        public const int TEST_PORT = 3389;
    }
}
