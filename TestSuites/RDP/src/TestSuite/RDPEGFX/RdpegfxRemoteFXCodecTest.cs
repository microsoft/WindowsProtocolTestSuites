// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdprfx;

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
        [Description("This test case is used to test RemoteFX codec bitmap(entropy method: RLGR1) transition via RDPEGFX.")]
        public void RDPEGFX_RemoteFXCodec_PositiveTest_ImageMode_RLGR1()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX encoded bitmap Data Messages encoded by RLGR1 entropy method to client.");
            Dictionary<uint, byte[]> frDict = this.rdpegfxAdapter.RemoteFXCodecEncode(image_64X64, OperationalMode.ImageMode, EntropyAlgorithm.CLW_ENTROPY_RLGR1,
                                                                        RdpegfxTestUtility.imgPos, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            fid = this.SendRfxCodecEcodedData(frDict);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test RemoteFX codec bitmap(entropy method: RLGR3) transition via RDPEGFX.")]
        public void RDPEGFX_RemoteFXCodec_PositiveTest_ImageMode_RLGR3()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX encoded bitmap Data Messages encoded by RLGR1 entropy method to client.");
            Dictionary<uint, byte[]> frDict = this.rdpegfxAdapter.RemoteFXCodecEncode(image_64X64, OperationalMode.ImageMode, EntropyAlgorithm.CLW_ENTROPY_RLGR3,
                                                                        RdpegfxTestUtility.imgPos, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            fid = this.SendRfxCodecEcodedData(frDict);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test RemoteFX codec video(entropy method: RLGR1) transition via RDPEGFX.")]
        public void RDPEGFX_RemoteFXCodec_PositiveTest_VideoMode_RLGR1()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX encoded video Data Messages encoded by RLGR1 entropy method to client.");
            Dictionary<uint, byte[]> frDict = this.rdpegfxAdapter.RemoteFXCodecEncode(imageForVideoMode, OperationalMode.VideoMode, EntropyAlgorithm.CLW_ENTROPY_RLGR1,
                                                                        RdpegfxTestUtility.imgPos, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            fid = this.SendRfxCodecEcodedData(frDict);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test RemoteFX codec video(entropy method: RLGR3) transition via RDPEGFX.")]
        public void RDPEGFX_RemoteFXCodec_PositiveTest_VideoMode_RLGR3()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX encoded video Data Messages encoded by RLGR3 entropy method to client.");
            Dictionary<uint, byte[]> frDict = this.rdpegfxAdapter.RemoteFXCodecEncode(imageForVideoMode, OperationalMode.VideoMode, EntropyAlgorithm.CLW_ENTROPY_RLGR3,
                                                                        RdpegfxTestUtility.imgPos, surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            fid = this.SendRfxCodecEcodedData(frDict);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }
    }
}
