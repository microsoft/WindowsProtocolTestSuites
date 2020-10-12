// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    /// <summary>
    /// Creates capability set collect to fill Client Confirm Active PDU
    /// </summary>
    public static class RdpbcgrCapSet
    {
        /// <summary>
        /// Create a surface commands capability set.
        /// </summary>
        /// <param name="supportSurfaceCommands">Indicating whether surface commands are supported or not.</param>
        /// <returns>The created surface commands capability set.</returns>
        public static TS_SURFCMDS_CAPABILITYSET CreateSurfaceCommandsCapabilitySet(bool supportSurfaceCommands)
        {
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

            return surfCmdsCapSet;
        }

        /// <summary>
        /// Create a bitmap codecs capability set.
        /// </summary>
        /// <param name="supportRemoteFXCodec">Indicating whether remoteFX codec is supported or not.</param>
        /// <returns>The created bitmap codecs capability set.</returns>
        public static TS_BITMAPCODECS_CAPABILITYSET CreateBitmapCodecsCapabilitySet(bool supportRemoteFXCodec)
        {
            TS_BITMAPCODECS_CAPABILITYSET codecsCapSet = new TS_BITMAPCODECS_CAPABILITYSET();
            codecsCapSet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS;
            codecsCapSet.supportedBitmapCodecs = new TS_BITMAPCODECS();
            if (supportRemoteFXCodec)
            {
                codecsCapSet.supportedBitmapCodecs.bitmapCodecCount = 3;
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray = new TS_BITMAPCODEC[3];
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray[0] = CreateTS_BITMAPCODEC_NSCodec();
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray[1] = CreateTS_BITMAPCODEC_RemoteFX();
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray[2] = CreateTS_BITMAPCODEC_Image_RemoteFX();
            }
            else
            {
                codecsCapSet.supportedBitmapCodecs.bitmapCodecCount = 1;
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray = new TS_BITMAPCODEC[1];
                codecsCapSet.supportedBitmapCodecs.bitmapCodecArray[0] = CreateTS_BITMAPCODEC_NSCodec();
            }
            codecsCapSet.lengthCapability = (ushort)(sizeof(ushort) + sizeof(ushort) + sizeof(byte));
            foreach (TS_BITMAPCODEC codec in codecsCapSet.supportedBitmapCodecs.bitmapCodecArray)
            {
                codecsCapSet.lengthCapability += (ushort)(19 + codec.codecPropertiesLength);
            }

            return codecsCapSet;
        }

        #region private methods
        /// <summary>
        /// Creates Bitmap Codec structure which contains a NSCodec Capability Set
        /// </summary>
        /// <returns></returns>
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
        private static TS_BITMAPCODEC CreateTS_BITMAPCODEC_RemoteFX()
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
        private static TS_BITMAPCODEC CreateTS_BITMAPCODEC_Image_RemoteFX()
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
