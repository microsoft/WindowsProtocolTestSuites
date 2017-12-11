// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public class RdpbcgrServerConfig
    {
        public EncryptedProtocol encryptedProtocol = EncryptedProtocol.Rdp;

        #region Server X.224 Connection Confirm PDU

        public bool isExtendedClientDataSupported = true;
        public selectedProtocols_Values selectedProtocol = selectedProtocols_Values.PROTOCOL_RDP_FLAG;

        #endregion

        #region Server MCS Connect Response PDU

        //public RdpVersion rdpVersionValue = RdpVersion.Version70;
        public TS_UD_SC_CORE_version_Values rdpServerVersion = TS_UD_SC_CORE_version_Values.V2;

        #region Server Security Data
        public EncryptionMethods encryptionMethod = EncryptionMethods.ENCRYPTION_METHOD_128BIT;
        public EncryptionLevel encryptionLevel = EncryptionLevel.ENCRYPTION_LEVEL_LOW;
        public SERVER_CERTIFICATE_dwVersion_Values certChainVersion = SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1;
        public bool autoGenerateCertificate = true;
        public int dwKeysize = 2048; //2048, 1024, 512
        public byte[] serverPrivateExponent;
        public byte[] serverPublicExponent;
        public byte[] severModulus;
        public string certificatePath;
        public string certificatePassword;

        #endregion

        public CompressionType Compress = CompressionType.PACKET_COMPR_TYPE_NONE;
        public int DisconnectReason = (int)Reason.rn_provider_initiated;
        public ushort ColorDepth = (ushort)postBeta2ColorDepth_Values.RNS_UD_COLOR_8BPP;

        #endregion

        #region Capbility Settings
        public ServerCapabilitySetting CapabilitySetting = new ServerCapabilitySetting();

        #endregion

        public RdpbcgrServerConfig()
        {
        }
    }

    public class ServerCapabilitySetting
    {
        #region General Capability Set settings
        /// <summary>
        /// Indicates support for auto-reconnection.
        /// </summary>
        public bool GeneralCapSet_ExtraFlags_AutoReconnect = true;
        public bool GeneralCapSet_RefreshRectSupport = true;
        public bool GeneralCapSet_SuppressOutputSupport = true;
        public ushort DesktopWidth = 800;
        public ushort DesktopHeight = 600;

        #endregion

        #region Input Capability Set settings
        /// <summary>
        /// Indicates support for using scancodes in the Keyboard Event notifications (see sections 2.2.8.1.1.3.1.1.1 and 2.2.8.1.2.2.1).
        /// </summary>
        public bool InputCapSet_InputFlags_ScanCodes = true;

        /// <summary>
        /// Indicates support for Extended Mouse Event notifications (see sections 2.2.8.1.1.3.1.1.4 and 2.2.8.1.2.2.4).
        /// </summary>
        public bool InputCapSet_InputFlags_MouseX = true;

        /// <summary>
        /// Advertised by RDP 5.0 and 5.1 servers. RDP 5.2, 6.0, 6.1, and 7.0 servers advertise the INPUT_FLAG_FASTPATH_INPUT2 flag 
        /// to indicate support for fast-path input.
        /// </summary>
        public bool InputCapSet_InputFlags_FPInput = true;

        /// <summary>
        /// Indicates support for Unicode Keyboard Event notifications (see sections 2.2.8.1.1.3.1.1.2 and 2.2.8.1.2.2.2).
        /// </summary>
        public bool InputCapSet_InputFlags_Unicode = true;

        /// <summary>
        /// Advertised by RDP 5.2, 6.0, 6.1, and 7.0 servers. Clients that do not support this flag will not be able 
        /// to use fast-path input when connecting to RDP 5.2, 6.0, 6.1, and 7.0 servers.
        /// </summary>
        public bool InputCapSet_InputFlags_FPInput2;
        #endregion

        #region Virtual Channel Capability Set settings
        /// <summary>
        /// Indicates to the client that virtual channel compression is supported by the server for client-to-server traffic.
        /// </summary>
        public bool VCCapSet_CS_CompressionSupport = true;

        /// <summary>
        /// Present VCChunkSize field in Virtual Channel Capability Set or not.
        /// </summary>
        public bool VCCapSet_SC_VCChunkSizePresent = true;
        #endregion

        /// <summary>
        /// Indicate if server will carry Bitmap Cache Host Support Capability Set.
        /// </summary>
        public bool BitmapCacheHostSupportCapabilitySet = true;

        /// <summary>
        /// Indicate if server will carry Share Capability Set.
        /// </summary>
        public bool ShareCapabilitySet = true;

        /// <summary>
        /// Indicate if server will carry Font Capability Set.
        /// </summary>
        public bool FontCapabilitySet = true;

        /// <summary>
        /// Indicate if server will carry Multifragment Update Capability Set.
        /// </summary>
        public bool MultifragmentUpdateCapabilitySet = true;
        public uint MultifragmentUpdateCapabilitySet_maxRequestSize = 38055;

        /// <summary>
        /// Indicate if server will carry Large Pointer Capability Set.
        /// </summary>
        public bool LargePointerCapabilitySet = true;

        /// <summary>
        /// Indicate if server will carry Desktop Composition Capability Set.
        /// </summary>
        public bool DesktopCompositionCapabilitySet = true;

        /// <summary>
        /// Indicate if server will carry Surface Commands Capability Set. 
        /// </summary>
        public bool SurfaceCommandsCapabilitySet = true;
        public CmdFlags_Values SurfaceCommandsCapabilitySet_cmdFlag = CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS;

        /// <summary>
        /// Indicate if server will carry Bitmap Codecs Capability Set.
        /// </summary>
        public bool BitmapCodecsCapabilitySet = true;
        public bool BitmapCodecsCapabilitySet_NSCodec = true;
        public bool BitmapCodecsCapabilitySet_RemoteFx;

        /// <summary>
        /// Indicate if server will carry Frame Acknowledge Capability Set.
        /// </summary>
        public bool FrameAcknowledgeCapabilitySet;
        public uint FrameAcknowledgeCapabilitySet_maxUnacknowledgedFrameCount;
    }
}
