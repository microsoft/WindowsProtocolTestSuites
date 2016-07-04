// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdprfx
{
    public partial class RdprfxAdapter
    {
        /// <summary>
        /// Method to verify client capabilities.
        /// </summary>
        private void VerifyClientCapabilities()
        {
            Site.Assert.IsTrue(is_Client_Multifragment_Update_CapabilitySet_Received, 
                "The client MUST send the Multifragment Update Capability Set ([MS-RDPBCGR], section 2.2.7.2.6).");

            Site.Log.Add(LogEntryKind.Debug, 
                "The MaxRequestSize field in the client-to-server Multifragment Update Capability Set is {0}",
                client_Multifragment_Update_CapabilitySet.MaxRequestSize);
            Site.Log.Add(LogEntryKind.Debug,
                "The MaxRequestSize field in the server-to-client Multifragment Update Capability Set is {0}",
                s2cMaxRequestSize);
            Site.Assert.IsTrue(client_Multifragment_Update_CapabilitySet.MaxRequestSize >= s2cMaxRequestSize, 
                "The MaxRequestSize field in the client-to-server Multifragment Update Capability Set MUST be set to a value greater than or equal to the value in the MaxRequestSize field of the server-to-client Multifragment Update Capability Set.");

            Site.Assert.IsTrue(is_Client_Large_Pointer_Capability_Set_Received, 
                "The client MUST send the Large Pointer Capability Set ([MS-RDPBCGR] section 2.2.7.2.7).");
            Site.Assert.AreEqual<largePointerSupportFlags_Values>(largePointerSupportFlags_Values.LARGE_POINTER_FLAG_96x96, client_Large_Pointer_Capability_Set.largePointerSupportFlags & largePointerSupportFlags_Values.LARGE_POINTER_FLAG_96x96,
                "The LARGE_POINTER_FLAG_96x96 (0x00000001) MUST be present in the largePointerSupportFlags field.");

            if (is_Client_Revision2_Bitmap_Cache_Capability_Set_Received)
            {
                Site.Assert.AreEqual<CacheFlags_Values>(CacheFlags_Values.ALLOW_CACHE_WAITING_LIST_FLAG, client_Revision2_Bitmap_Cache_Capability_Set.CacheFlags & CacheFlags_Values.ALLOW_CACHE_WAITING_LIST_FLAG,
                    "If the Revision 2 Bitmap Cache Capability Set ([MS-RDPBCGR] section 2.2.7.1.4.2) is sent by the client, then the ALLOW_CACHE_WAITING_LIST_FLAG (0x0002) MUST be present in the CacheFlags field.");
            }

            if (is_TS_FRAME_ACKNOWLEDGE_CAPABILITYSET_Received)
            {
                Site.Log.Add(LogEntryKind.Debug, "The maxUnacknowledgedFrameCount field of TS_FRAME_ACKNOWLEDGE_CAPABILITYSET is {0}",
                    clientTS_FRAME_ACKNOWLEDGE_CAPABILITYSET.maxUnacknowledgedFrameCount);
            }

            Site.Assert.IsTrue(is_Client_Surface_Commands_Capability_Set_Received, 
                "Support for this surface command MUST be advertised in the Surface Commands Capability Set ([MS-RDPBCGR] section 2.2.7.2.9).");
            Site.Assert.IsTrue(clientupportStreamSurfaceBits, 
                "A RemoteFX client MUST support the Stream Surface Bits Surface Command ([MS-RDPBCGR] section 2.2.9.2.2).");

            Site.Assert.IsTrue(is_Client_Bitmap_Codecs_Capability_Set_Received, "Client must advertise the support of Bitmap Codecs Capability Set.");
            Site.Assert.IsTrue(is_TS_RFX_CLNT_CAPS_CONTAINER_Received, "Client should include the TS_RFX_CLNT_CAPS_CONTAINER in Bitmap Codecs Capability Set.");
        }
        
        /// <summary>
        /// Verify supportedColorDepths of Client Core data
        /// </summary>
        private void VerifyColorDepths()
        {
            this.Site.Assert.IsNotNull(this.supportedColorDepths, "The client must support a color depth of 32 bits per pixel. ([MS-RDPBCGR] section 1.5)");

            Boolean support32Bits = (this.supportedColorDepths.actualData >= 8);//If RNS_UD_32BPP_SUPPORT (0x0008) flag is set, supportedColorDepths should >=8
            this.Site.Assert.IsTrue(support32Bits, "The client must support a color depth of 32 bits per pixel. ([MS-RDPBCGR] section 1.5)");            
        }
        
        /// <summary>
        /// Verify TS_FRAME_ACKNOWLEDGE_PDU
        /// </summary>
        /// <param name="ackPdu">The TS_FRAME_ACKNOWLEDGE_PDU to be verified</param>
        private void VerifyTS_FRAME_ACKNOWLEDGE_PDU(TS_FRAME_ACKNOWLEDGE_PDU ackPdu)
        {
            this.Site.Log.Add(LogEntryKind.Comment, "The frameID of TS_FRAME_ACKNOWLEDGE_PDU is {0}", ackPdu.frameID);
            if (ackPdu.frameID == 0xFFFFFFFF)
            {
                this.Site.Assert.IsTrue(ackPdu.frameID == 0xFFFFFFFF, "[TS_FRAME_ACKNOWLEDGE_PDU] If frameID has the value 0xFFFFFFFF, the server SHOULD assume that all in-flight frames have been acknowledged.");
            }
            else
            {
                this.Site.Assert.AreEqual<uint>(this.frameMakerFrameId, ackPdu.frameID, "[TS_FRAME_ACKNOWLEDGE_PDU][frameID] This field specifies the 32-bit identifier of the frame that was sent to the client using a Frame Marker Command and is being acknowledged as delivered");
            }
        }

          
    }
}
