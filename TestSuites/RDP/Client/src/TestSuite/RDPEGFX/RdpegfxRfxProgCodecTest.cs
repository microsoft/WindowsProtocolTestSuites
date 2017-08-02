// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;

using System.Collections.Generic;
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
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream which is progressively encoded.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_ProgressiveEncoding_Default()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages(Progressive Encoding) to client. to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, ImageQuality_Values.Midium, true, false, false);
            
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream which is non-progressively encoded.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_NonProgressiveEncoding_Default()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages(Non-Progressive Encoding) to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream which is progressively encoded with tile diff.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_ProgressiveEncodingWithTileDiff()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending first bitmap Data Messages, progressively encoded by RemoteFX Progressive Codec, to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Midium, true, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Get second bitmap by adding a diagonal to first bitmap");
            // Add a diagonal to the test image and send it again, the image data should be sent in tile diff method.
            Bitmap newBitmap = RdpegfxTestUtility.drawDiagonal(testData.RfxProgCodecImage);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending second bitmap Data Messages, progressively encoded by RemoteFX Progressive Codec(tile diff enabled), to client.");
            List<Dictionary<uint, byte[]>> layerDataList2 = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, newBitmap, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Midium, true, true, true);
            fid = SendRfxProgCodecEcodedData(layerDataList2);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream which is non-progressively encoded with tile diff.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_NonProgressiveEncodingWithTileDiff()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending first bitmap Data Messages, non-progressively encoded by RemoteFX Progressive Codec, to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Midium, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Get second bitmap by adding a diagonal to first bitmap");
            // Add a diagonal to the test image and send it again, the image data should be sent in tile diff method.
            Bitmap newBitmap = RdpegfxTestUtility.drawDiagonal(testData.RfxProgCodecImage);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending second bitmap Data Messages, non-progressively encoded by RemoteFX Progressive Codec(tile diff enabled), to client.");
            List<Dictionary<uint, byte[]>> layerDataList2 = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, newBitmap, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Midium, false, true, true);
            fid = SendRfxProgCodecEcodedData(layerDataList2);
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
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream when surface size is not tile aligned.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_ProgressiveEncoding_SurfaceNotTileAligned()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface(width: {0}, height {1}) and fill it with green color.",
                                RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages(Progressive Encoding) to client.");
            Bitmap surfImage = RdpegfxTestUtility.DrawSurfImage(surf, Color.Green, testData.RfxProgCodecImage, RdpegfxTestUtility.imgPos3);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, surfImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, true, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
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
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream when surface size is not tile aligned.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_NonProgressiveEncoding_SurfaceNotTileAligned()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface(width: {0}, height {1}) and fill it with green color.",
                                RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages(Non-Progressive Encoding) to client.");
            Bitmap surfImage = RdpegfxTestUtility.DrawSurfImage(surf, Color.Green, testData.RfxProgCodecImage, RdpegfxTestUtility.imgPos3);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, surfImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
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
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream with sync block.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_WithSyncBlock()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data (with sync block) Messages to client.");
            fid = this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, true, true, false);
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
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream without context block.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_WithoutContextBlock()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data (without context block) Messages to client.");
            fid = this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, false, false);
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
        [Description("This test case is used to test if the client accepts second RFX Progressive codec stream (progressively encoded) with SubBand_Diffing disabled in context block.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_ProgressiveEncoding_SubBandDiffDisabled()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending first bitmap Data Messages, progressively encoded by RemoteFX Progressive Codec, to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, true, true, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Get second bitmap by adding a diagonal to first bitmap");
            // Add a diagonal to the test image and send it again, the image data should be sent in tile diff method.
            Bitmap newBitmap = RdpegfxTestUtility.drawDiagonal(testData.RfxProgCodecImage);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending second bitmap Data Messages, progressively encoded by RemoteFX Progressive Codec(SubBand_Diffing disabled), to client.");
            List<Dictionary<uint, byte[]>> layerDataList2 = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, true, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList2);
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
        [Description("This test case is used to test if the client accepts second RFX Progressive codec stream (non-progressively encoded) with SubBand_Diffing disabled in context block.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_NonProgressiveEncoding_SubBandDiffDisabled()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending first bitmap Data Messages, non-progressively encoded by RemoteFX Progressive Codec, to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, true, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(true, surfRect);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Get second bitmap by adding a diagonal to first bitmap");
            // Add a diagonal to the test image and send it again, the image data should be sent in tile diff method.
            Bitmap newBitmap = RdpegfxTestUtility.drawDiagonal(testData.RfxProgCodecImage);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending second bitmap Data Messages, non-progressively encoded by RemoteFX Progressive Codec(SubBand_Diffing disabled), to client.");
            List<Dictionary<uint, byte[]>> layerDataList2 = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList2);
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
        [Description("This test case is used to test if the client accepts RFX Progressive codec stream (progressively encoded) with DWT doesn't use Reduce Extrapolate method.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_ProgressiveEncoding_ReduceExtrapolateDisabled()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending bitmap Data Messages, progressively encoded by RemoteFX Progressive Codec (DWT doesn't use Reduce Extrapolate method), to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, true, false, false);
            fid = SendRfxProgCodecEcodedData(layerDataList);
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
        [Description("This test case is used to test if the client accepts second RFX Progressive codec stream (non-progressively encoded) with DWT doesn't use Reduce Extrapolate method.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_NonProgressiveEncoding_ReduceExtrapolateDisabled()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending bitmap Data Messages, non-progressively encoded by RemoteFX Progressive Codec(DWT doesn't use Reduce Extrapolate method), to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, false);
            fid = SendRfxProgCodecEcodedData(layerDataList);
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
        [Description("This test case is used to test if the client can delete a RFX Progressive codec stream.")]
        public void RDPEGFX_RfxProgressiveCodec_PositiveTest_DeleteContext()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Encoded Bitmap Data Messages to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, true, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending request to delete a RemoteFX Progressive Codec Context from client.");
            // Get a used context id 
            uint contextId = RdpegfxTestUtility.maxRfxProgCodecContextId;
            bool result = this.rdpegfxAdapter.GetUsedRfxProgssiveCodecContextId(ref contextId);
            this.TestSite.Assert.IsTrue(result, "failed to get a used Rfx Progressive Codec Context to delete");

            // Delete context
            fid = this.rdpegfxAdapter.DeleteRfxProgssiveCodecContextId(surf.Id, contextId);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream which is sent to a non-exist surface.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_InexistSurface()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Delete the surface (id: {0}) on client.", surf.Id);
            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages to the deleted surface (id: {0}) on client.", surf.Id);
                       
            try
            {
                // Delete context from inexistent surface
                this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

                //Expect the RDP client handle the negative request by dropping the connection as Windows does, or deny the request or ignore the request.
                RDPClientTryDropConnection("delete inexistent surface");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.CheckFailed, "SUT should terminate the connection, or deny the request, or ignore the request to create duplicated surface instead of throw out an exception: {0}.", ex.Message);
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with sync block len is not 12.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_SyncBlock_IncorrectBlockLen()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with sync block length is not 12 to client.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with sync block len is not 12
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_SyncBlock_IncorrectLen);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, true, true, false);

            // Test case pass if RDP connection is stopped due to inexist surface id in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with sync block length is not 12");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with duplicated frame begin block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_DuplicatedFrameBeginBlock()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with duplicated frame begin block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with duplicated frame begin block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_DuplicatedFrameBeginBlock);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

            // Test case pass if RDP connection is stopped due to duplicated frame begin block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with duplicated frame begin block");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with missing frame begin block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_MissedFrameBeginBlock()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with missing frame begin block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with missing frame begin block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_MissedFrameBeginBlock);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

            // Test case pass if RDP connection is stopped due to missed frame begin block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream without frame begin block");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with Nested frame block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_NestedFrameBlock()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with Nested frame block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with Nested frame block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_NestedFrameBlock);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

            // Test case pass if RDP connection is stopped due to Nested frame block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with Nested frame block");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with incorrect length in frame begin block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_FrameBeginBlock_IncorrectLength()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect length in frame begin block");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect length in frame begin block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_FrameBeginBlock_IncorrectLen);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

            // Test case pass if RDP connection is stopped due to incorrect length in frame begin block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect length in frame begin block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with missing frame end block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_MissedFrameEndBlock()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with missing frame end block");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with missing frame end block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_MissedFrameEndBlock);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

            // Test case pass if RDP connection is stopped due to missed frame end block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream without frame end block.");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with incorrect length in frame end block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_FrameEndBlockIncorrectLength()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect length in frame end block");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect length in frame end block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_FrameEndBlock_IncorrectLength);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

            // Test case pass if RDP connection is stopped due to incorrect length in frame end block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect length in frame end block");
            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with incorrect length in context block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_ContextBlock_IncorrectLength()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect length in context block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect length in context block
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_ContextBlock_IncorrectLen);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

            // Test case pass if RDP connection is stopped due to incorrect length in context block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect length in context block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with incorrect tile size in context block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_ContextBlock_IncorrectTileSize()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect tile size  in context block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect tile size(0x41)  in context block
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_ContextBlock_IncorrectTileSize);
            this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);

            // Test case pass if RDP connection is stopped due to incorrect tile size in context block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect tile size  in context block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with region block is before frame begin block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_BeforeFrameBeginBlock()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with region block is before frame begin block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with region block is before frame begin block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_BeforeFrameBeginBlock);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to region block is before frame begin block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with region block is before frame begin block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with incorrect block length of region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_IncorrectLen()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect block length of region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect block length of region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectLen);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to incorrect block length of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect block length of region block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with incorrect tile size in region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_IncorrectTileSize()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect tile size in region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect tile size in region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectTileSize);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to incorrect tile size of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect tile size in region block");
            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if the client can accept a RFX Progressive codec stream with incorrect Rects Number in region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_IncorrectRectsNumber()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect Rects Number in region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect Rects Number in region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectRectsNumber);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to incorrect Rects Number of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect Rects Number in region block");
            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can accept a RFX Progressive codec stream with zero Rects Number in region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_ZeroRectsNumber()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with zero Rects Number in region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect Rects Number in region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_ZeroRectsNumber);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to zero Rects Number of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with 0 Rects Number in region block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can accept a RFX Progressive codec stream with incorrect Quant Number in region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_IncorrectQuantNumber()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect Quant Number in region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect Quant Number in region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectQuantNumber);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to incorrect Quant Number of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect Quant  Number in region block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can accept a RFX Progressive codec stream with zero Quant Number in region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_ZeroQuantNumber()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with zero Quant Number in region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with zero Quant Number in region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_ZeroQuantNumber);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to zero Quant Number of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with 0 Quant  Number in region block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can accept a RFX Progressive codec stream with incorrect ProgQuant Number in region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_IncorrectProgQuantNumber()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect ProgQuant Number in region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect ProgQuant Number in region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectProgQuantNumber);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Midium, true, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to incorrect ProgQuant Number of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect ProgQuant Number in region block");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can accept a RFX Progressive codec stream with incorrect tile block number in region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_IncorrectTileBlockNumber()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with incorrect tile block number in region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with incorrect tile block number in region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_IncorrectTileBlockNumber);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to incorrect tile block number of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with incorrect tile block in region block");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can accept a RFX Progressive codec stream with invalid tile block type in region block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_InvalidTileBlockType()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with invalid tile block type in region block.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with invalid tile block type (0xffff) in region block.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlock_InvalidTileBlockType);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to iinvalid tile block type of region block in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with invalid tile block type in region block");
            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can accept a RFX Progressive codec stream with mismatched region block number as that in frame begin block.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlockNumberMismatch()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages with mismatched region block number.");
            // Set test type to instruct egfx adapter to encode Rfx Progressive Codec stream with mismatched region block number.
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.RfxProgCodec_RegionBlockNumberMismatch);
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList, false);

            // Test case pass if RDP connection is stopped due to mismatched region block number in Rfx Progressive Codec stream 
            RDPClientTryDropConnection("a Rfx Progressive stream with mismatched region block number");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can accept a RFX Progressive codec stream to delete non-exist context on client.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_RegionBlock_DeleteInexistContext()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, testData.RfxProgCodecImage, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            uint contextId = RdpegfxTestUtility.maxRfxProgCodecContextId;
            bool result = this.rdpegfxAdapter.GetUsedRfxProgssiveCodecContextId(ref contextId);
            this.TestSite.Assert.IsTrue(result, "failed to get a used Rfx Progressive Codec Context to delete");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages to delete a context (id: {0}) on client.", contextId);
            // Delete context
            fid = this.rdpegfxAdapter.DeleteRfxProgssiveCodecContextId(surf.Id, contextId);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages to delete same context(id: {0}) , which does not exist on client.", contextId);
            try
            {
                // Attempt to delete the same context again
                fid = this.rdpegfxAdapter.DeleteRfxProgssiveCodecContextId(surf.Id, contextId);

                //Expect the RDP client handle the negative request by dropping the connection as Windows does, or deny the request or ignore the request.
                RDPClientTryDropConnection("delete context from an inexistent surface");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.CheckFailed, "SUT should terminate the connection, or deny the request, or ignore the request to create duplicated surface instead of throw out an exception: {0}.", ex.Message);
            }         
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test if client can delete Rfx Progressive codec context from a non-exist surface.")]
        public void RDPEGFX_RfxProgressiveCodec_NegativeTest_DeleteContextFromInexistSurface()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages to surface (id: {0}) on client.", surf.Id);
            fid = this.rdpegfxAdapter.SendRfxProgressiveCodecPduWithoutImage(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, false, true, false);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Get Rfx progressive codec context Id
            uint contextId = RdpegfxTestUtility.maxRfxProgCodecContextId;
            bool result = this.rdpegfxAdapter.GetUsedRfxProgssiveCodecContextId(ref contextId);
            this.TestSite.Assert.IsTrue(result, "failed to get a used Rfx Progressive Codec Context to delete");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Delete the surface (id: {0}) on client.", surf.Id);
            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending RemoteFX Progressive Codec Data Messages to delete a context from surface(id: {0}) on client.", surf.Id);
            try
            {
                // Delete context from inexist surface
                fid = this.rdpegfxAdapter.DeleteRfxProgssiveCodecContextId(surf.Id, contextId);

                //Expect the RDP client handle the negative request by dropping the connection as Windows does, or deny the request or ignore the request.
                RDPClientTryDropConnection("allocate cache slots with cache size exceeds the max value 100MB");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.CheckFailed, "SUT should terminate the connection, or deny the request, or ignore the request to create duplicated surface instead of throw out an exception: {0}.", ex.Message);
            }          
        }
    }
}
