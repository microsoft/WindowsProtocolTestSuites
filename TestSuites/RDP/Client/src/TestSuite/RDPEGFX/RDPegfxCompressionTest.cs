// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpegfx
{
    public partial class RdpegfxTestSuite : RdpTestClassBase
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test Compression segment of bitmap transition via RDPEGFX.")]
        public void RDPEGFX_Compression_PositiveTest()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "The bitmap is sent to the client for frame {0} using RDP 8.0 compression technology.", fid); 
            byte compFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8 | (byte)PACKET_COMPR_FLAG.PACKET_COMPRESSED;
            // byte compFlag = RdpSegmentedPdu.PACKET_COMPR_TYPE_RDP8;
            Image bgImage;
            Image compImage = RdpegfxTestUtility.captureFromImage(image_64X64, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.smallWidth, RdpegfxTestUtility.smallHeight, out bgImage);
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, RdpegfxTestUtility.imgPos.x, RdpegfxTestUtility.imgPos.y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);
            this.rdpegfxAdapter.ExpectFrameAck(fid);
            
            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with compress flag is not 0x20.")]
        public void RDPEGFX_Compression_Negative_IncorrectCompressFlag()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.RDP8Compression_IncorrectCompressFlag);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RDP8Compression_IncorrectCompressFlag);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send CreateSurface message and MapSurfaceToOutPut message with incorrect compress flag to client.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPClientTryDropConnection("CreateSurface message and MapSurfaceToOutPut message with incorrect compress flag");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with compress type is not 0x04.")]
        public void RDPEGFX_Compression_Negative_IncorrectCompressType()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.RDP8Compression_IncorrectCompressType);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RDP8Compression_IncorrectCompressType);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send CreateSurface message and MapSurfaceToOutPut message with incorrect compress type to client.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPClientTryDropConnection("CreateSurface message and MapSurfaceToOutPut message with incorrect compress type");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to check if client can handle a message with incorrect compressed pdu.")]
        public void RDPEGFX_Compression_Negative_InvalidCompressPdu()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.RDP8Compression_InvalidCompressPDU);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RDP8Compression_InvalidCompressPDU);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send CreateSurface message and MapSurfaceToOutPut message with compress flag and uncompressed payload to client.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPClientTryDropConnection("CreateSurface message and MapSurfaceToOutPut message with compress flag and uncompressed payload");            
        }
    }
}
