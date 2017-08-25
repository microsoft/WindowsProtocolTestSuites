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
        [Description("This test case is used to verify whether RDP client can accept a RDPGFX_WIRE_TO_SURFACE_PDU_1 message with an uncompressed image.")]
        public void RDPEGFX_WireToSurface_PositiveTest()
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
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}.", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "The bitmap is sent to the client for frame {0}.", fid);
            byte compFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8;
            Image bgImage;
            Image compImage = RdpegfxTestUtility.captureFromImage(image_64X64, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.smallWidth, RdpegfxTestUtility.smallHeight, out bgImage);
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, RdpegfxTestUtility.imgPos.x, RdpegfxTestUtility.imgPos.y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify whether RDP client can process correctly when accept a RDPGFX_WIRE_TO_SURFACE_PDU_1 message with an uncompressed image whose specific borders are overlapped with surface.")]
        public void RDPEGFX_WireToSurface_PositiveTest_ImageBorderOverlapSurface()
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
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}.", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            Image bgImage;
            Image compImage = RdpegfxTestUtility.captureFromImage(image_64X64, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.smallWidth, RdpegfxTestUtility.smallHeight, out bgImage);
            byte compFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8;

            // Top-right corner
            this.TestSite.Log.Add(LogEntryKind.Comment, "The bitmap is sent to the client by frame {0}. whose top and right borders are overlapped with surface.", fid);
            int y = 0;
            int x = RdpegfxTestUtility.surfWidth - RdpegfxTestUtility.smallWidth;
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, (ushort)x, (ushort)y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Bottom-right corner
            this.TestSite.Log.Add(LogEntryKind.Comment, "The bitmap is sent to the client by frame {0}. whose bottom and right borders are overlapped with surface", fid);
            y = RdpegfxTestUtility.surfHeight - RdpegfxTestUtility.smallHeight;
            x = RdpegfxTestUtility.surfWidth - RdpegfxTestUtility.smallWidth;
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, (ushort)x, (ushort)y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Bottom-left corner
            this.TestSite.Log.Add(LogEntryKind.Comment, "The bitmap is sent to the client by frame {0}. whose bottom and left borders are overlapped with surface", fid);
            y = RdpegfxTestUtility.surfHeight - RdpegfxTestUtility.smallHeight;
            x = 0;
            fid = this.rdpegfxAdapter.SendUncompressedImage(compImage, (ushort)x, (ushort)y, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, compFlag, RdpegfxTestUtility.segmentPartSize);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }
    }
}