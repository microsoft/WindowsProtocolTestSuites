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
        [Description("This test case is used to test ClearCodec(L1&L2) bitmap transition via RDPEGFX.")]
        public void RDPEGFX_ClearCodec_PositiveTest_ResidualBandLayer()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data (L1&L2) Messages to client.");
            // Set clearcodec residual layer and band layer bitmap 
            Image bgImage;
            Bitmap ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight, out bgImage);
            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, ccBandImage);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)bgImage.Width, (ushort)bgImage.Height); 
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_CACHE_RESET, 0,
                                                        ccRect, bgImage, bandBmpDict, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

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
        [Description("This test case is used to test ClearCodec(L1&L3) bitmap transition via RDPEGFX.")]
        public void RDPEGFX_ClearCodec_PositiveTest_ResidualSubcodecLayer()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data(L1&L3) Messages to client.");
            // Set clearcodec residual layer and band layer bitmap 
            Image bgImage;
            BMP_INFO bmp_info = new BMP_INFO();
            bmp_info.scID = CLEARCODEC_SUBCODEC_ID.SUBCODEC_RLEX;
            bmp_info.bmp = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccSubcodecWidth, RdpegfxTestUtility.ccSubcodecHeight, out bgImage);

            Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecBmpDict = new Dictionary<RDPGFX_POINT16, BMP_INFO>();
            subcodecBmpDict.Add(RdpegfxTestUtility.imgPos, bmp_info);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)bgImage.Width, (ushort)bgImage.Height);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        ccRect, bgImage, null, subcodecBmpDict);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test ClearCodec(L1&L2) bitmap transition without resetting vbar cache via RDPEGFX.")]
        public void RDPEGFX_ClearCodec_PositiveTest_NotResetVBarCache()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data Messages to client.");
            // Set clearcodec residual layer and band layer bitmap
            Image bgImage;
            Bitmap ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight, out bgImage);
            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, ccBandImage);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)bgImage.Width, (ushort)bgImage.Height);
            // Send encoded clearcodec bitmap data in w2s_1 PDU, without v-bar cache reset.
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        ccRect, bgImage, bandBmpDict, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test ClearCodec multiple band bitmap transition via RDPEGFX.")]
        public void RDPEGFX_ClearCodec_PositiveTest_MultipleBands()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap(multiple bands) Messages to client.");
            // Init 2 clearcodec band bitmaps
            Image bgImage;
            Image ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight, out bgImage);
            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, (Bitmap)ccBandImage);
            bandBmpDict.Add(RdpegfxTestUtility.imgPos2, (Bitmap)ccBandImage);
            // Set clearcode bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)bgImage.Width, (ushort)bgImage.Height);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_CACHE_RESET, 0,
                                                        ccRect, (Bitmap)bgImage, bandBmpDict, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if vbar cache in previous PDU can be used in second PDU.")]
        public void RDPEGFX_ClearCodec_PositiveTest_MultipleFramesPduWithCache()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending first ClearCodec Encoded Bitmap Data Messages to client to init v-bar cache.");
            Image bgImage;
            Image ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight, out bgImage);

            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            // Set clearcodec band and position (relative to Clearcodec bitmap) 
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, (Bitmap)ccBandImage);
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)ccBandImage.Width, (ushort)ccBandImage.Height);
            // Send encoded clearcodec bitmap data in w2s_1 PDU, and reseting v-bar cache on client.
            // This message will set v-bar/short v-bar cache on client.
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_CACHE_RESET, 0,
                                                        ccRect, null, bandBmpDict, null);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending same band which is ClearCodec Encoded by v-bar cache to client.");
            bandBmpDict.Clear();
            // Set same clearcodec band and position(relative to Clearcodec bitmap) 
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, (Bitmap)ccBandImage);
            // Set clearcodec bitmap rectangle to different position, relative to surface
            RDPGFX_RECT16 ccRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos2, (ushort)ccBandImage.Width, (ushort)ccBandImage.Height);
            // Send encoded clearcodec bitmap data in w2s_1 PDU, and without reseting v-bar cache on client, the same band will be totally encoded 
            // by v-bar/short v-bar cache, which is created in first clearcodec message.
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        ccRect2, null, bandBmpDict, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test ClearCodec multiple subcodec bitmap transition via RDPEGFX.")]
        public void RDPEGFX_ClearCodec_PositiveTest_MultipleSubCodecArea()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data Messages(with multiple subcodec bitmap) to client.");
            // Set 2 clearcodec subcodec layer bitmap
            Image bgImage;
            BMP_INFO bmp_info = new BMP_INFO();
            bmp_info.scID = CLEARCODEC_SUBCODEC_ID.SUBCODEC_RLEX;
            bmp_info.bmp = (Bitmap)RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccSubcodecWidth, RdpegfxTestUtility.ccSubcodecHeight, out bgImage);
            Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecBmpDict = new Dictionary<RDPGFX_POINT16, BMP_INFO>();
            subcodecBmpDict.Add(RdpegfxTestUtility.imgPos, bmp_info);
            subcodecBmpDict.Add(RdpegfxTestUtility.imgPos2, bmp_info);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)bgImage.Width,(ushort)bgImage.Height);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        ccRect, (Bitmap)bgImage, null, subcodecBmpDict);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test ClearCodec uncompressed subcodec bitmap transition via RDPEGFX.")]
        public void RDPEGFX_ClearCodec_PositiveTest_SubCodecUncompressed()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data Messages(with raw format in subcodec layer) to client.");
            // Set clearcodec residual layer and subcodec layer bitmap
            Image bgImage;
            BMP_INFO bmp_info = new BMP_INFO();
            bmp_info.scID = CLEARCODEC_SUBCODEC_ID.SUBCODEC_RAW;  // Set subcodec layer bitmap to be encoded in raw format
            bmp_info.bmp = (Bitmap)RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccSubcodecWidth, RdpegfxTestUtility.ccSubcodecHeight, out bgImage);

            Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecBmpDict = new Dictionary<RDPGFX_POINT16, BMP_INFO>();
            subcodecBmpDict.Add(RdpegfxTestUtility.imgPos, bmp_info);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)bgImage.Width, (ushort)bgImage.Height);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        ccRect, (Bitmap)bgImage, null, subcodecBmpDict);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if ClearCodec stream with glyph index can be handled by the client.")]
        public void RDPEGFX_ClearCodec_PositiveTest_GlyphIndexEnabled()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a glyph(with glyph_index flag set) which is ClearCodec Encoded Messages to client.");
            // Init a glyph bitmap
            Image bgImage;
            Image ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight, out bgImage);

            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, (Bitmap)ccBandImage);
            // Set glyph bitmap rectangle, relative to surface
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            // Set GLYPH_INDEX flag
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX;
            // Sending a glyph to client with a valid glyph_index number in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect, null, bandBmpDict, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if client can cache max size(1024) glyph in clearcodec stream.")]
        public void RDPEGFX_ClearCodec_PositiveTest_MaxGlyphSize()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a Glyph with max(1024) square pixels size in ClearCodec Encoded Bitmap Messages to client.");
            // Init a glyph with size of 1024 square pixels
            Image bgImage;
            ushort w = RdpegfxTestUtility.ccGlyphWidth;
            ushort h = (ushort)(RdpegfxTestUtility.ccMaxGlyphSize / w);
            Image ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                w, h, out bgImage);
            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, (Bitmap)ccBandImage);
            // Set glyph bitmap rectangle, relative to surface
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos,w, h);
            // Sending a glyph to client without glyph_index in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE,
                                                        0, glyphRect, null, bandBmpDict, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if client can support max(4000) ClearCodec glyph slot.")]
        public void RDPEGFX_ClearCodec_PositiveTest_MaxNumGlyphSlot()
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


            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending max(4000) Glyphs in multiple ClearCodec Encoded Bitmap Data Messages to client.");
            // Init a small glyph with width 1, height 2, the glyphes in the same row will be sent in same w2s_1 PDU.
            ushort width = RdpegfxTestUtility.ccSmallGlyphWidth;
            ushort height = RdpegfxTestUtility.ccSmallGlyphHeight;
            Image ccGlyph = RdpegfxTestUtility.DrawImage(width, height, Color.Violet);
            // Init start position and rectangle of small glyph in same raw.
            RDPGFX_POINT16 startPos = RdpegfxTestUtility.imgPos;

            // Get the glyph number in a row, these glyphs will be sent in same w2s_1 PDU.
            ushort glyphNumInBatch = (ushort)(surf.Width - (startPos.x + width) + 1);
            ushort startGlyphIdx = 0;
            // Get the full row number for 4000 glyphs.
            ushort count = (ushort)(RdpegfxTestUtility.ccMaxGlyphIndexNum / glyphNumInBatch);
            ushort tailNum = (ushort)(RdpegfxTestUtility.ccMaxGlyphIndexNum % glyphNumInBatch);

            // Send the whole row glyphs in a w2s_1 frame
            for (ushort i = 0; i < count; i++)
            {
                fid = this.rdpegfxAdapter.SendClearCodecGlyphInBatch(surf, startGlyphIdx, startPos, glyphNumInBatch, ccGlyph);   
                this.rdpegfxAdapter.ExpectFrameAck(fid);             
                
                // Move to next row, reset start position
                startGlyphIdx += glyphNumInBatch;
                startPos.y += (ushort)(height+1);
            }

            // Send the left number glyphs in a rdpegfx frame
            if(tailNum != 0)
            {
                fid = this.rdpegfxAdapter.SendClearCodecGlyphInBatch(surf, startGlyphIdx, startPos, tailNum, ccGlyph);   
                this.rdpegfxAdapter.ExpectFrameAck(fid);
            }
            // Reaching here means each glyph message has a frame acknowledge received, test case pass

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if client can use ClearCodec glyph Cache in same rect area.")]
        public void RDPEGFX_ClearCodec_PositiveTest_GlyphHitEnabled_SameRect()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a glyph in ClearCodec Encoded Bitmap Data Messages to client and instruct client to cache it.");
            Image bgImage;
            Image ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight, out bgImage);
            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, (Bitmap)ccBandImage);
            // Set glyph bitmap rectangle, relative to surface
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            // Set glyph_index use flag
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX;
            // Sending glyph to client
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect, null, bandBmpDict, null);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);
            
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending same glyph in ClearCodec Encoded Bitmap Data Messages with glyph index only to client.");
            // Set same glyph bitmap to new rectangle, relative to surface
            RDPGFX_RECT16 glyphRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos2, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            // Set glyph_hit use flag
            ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX | ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_HIT;
            // Sending same glyph to client by using client cache
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect2, null, null, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test ClearCodec glyph Cache can be used in a Rectangle with same size, but different width/height.")]
        public void RDPEGFX_ClearCodec_PositiveTest_GlyphHitEnabled_DiffRect()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a glyph in ClearCodec Encoded Bitmap Data Messages to client and instruct client to cache it..");
            // Init a 50 * 20 size glyph
            Image bgImage;
            Image ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight, out bgImage);
            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, (Bitmap)ccBandImage);

            // Set glyph bitmap rectangle, relative to surface
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX;
            // Sending a glyph to client
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect, null, bandBmpDict, null);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data Messages to instruct client to use the cached glyph into a rectangle with different width/height .");
            // Create a new glyph rectangle with width and height are exchanged.
            RDPGFX_RECT16 glyphRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos2, RdpegfxTestUtility.ccBandHeight, RdpegfxTestUtility.ccBandWidth);
            // Set glyph_hit flag enabled
            ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX | ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_HIT;
            // Send w2s_1 PDU to instruct client to fill cached glyph into another glyph rectangle
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect2, null, null, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);
            
            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if RDP client can accept a ClearCodec Encoded bitmap with max(32766) bitmap width.")]
        public void RDPEGFX_ClearCodec_PositiveTest_MaxWidthBitmap()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size.");
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos3, RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages with max bitmap width(32766 pixels) to client.");
            // Init a bitmap with width is 32766
            Bitmap maxBitmap = RdpegfxTestUtility.DrawImage(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight, Color.Azure);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight);        
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            // Encode the bitmap into residual layer and Send encoded clearcodec bitmap data in w2s_1 PDU             
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, ccRect, maxBitmap, null, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);
            
            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if RDP client can accept a ClearCodec Encoded bitmap with max bitmap height(32766).")]
        public void RDPEGFX_ClearCodec_PositiveTest_MaxHeightBitmap()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size.");
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.desktopWidth, RdpegfxTestUtility.MaxBmpHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos3, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.MaxBmpHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages with max bitmap height(32766 pixels) to client.");
            // Init a bitmap with height is 32766
            Bitmap maxBitmap = RdpegfxTestUtility.DrawImage(RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.MaxBmpHeight, Color.Azure);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.MaxBmpHeight);       
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            // Encode the bitmap into residual layer and Send encoded clearcodec bitmap data in w2s_1 PDU          
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, ccRect, maxBitmap, null, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if the client accepts a ClearCodec stream with last RunLengthFactor that is zero in residual layer.")]
        public void RDPEGFX_ClearCodec_PositiveTest_Residual_ZeroRunLengthFactor()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Messages with last residual RLF is zero to client.");
            // Init a residual bitmap
            Bitmap resBitmap = RdpegfxTestUtility.DrawImage(RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight, Color.Azure);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);         
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            // Set test type with ClearCodec_Residual_ZeroRunLengthFactor
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.ClearCodec_Residual_ZeroRunLengthFactor);  // RLF data: 0x00
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, ccRect, resBitmap, null, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test ClearCodec bitmap transition with max(52 pixels) band height.")]
        public void RDPEGFX_ClearCodec_PositiveTest_MaxBandHeight()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Band(height is 52 pixels) Data Messages to client.");
            // Init a band with height is 52 pixels
            Image bgImage;
            Bitmap ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccMaxBandHeight, out bgImage);
            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, ccBandImage);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)ccBandImage.Width, (ushort)ccBandImage.Height);
            // Send the encoded band bitmap data to client in in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        ccRect, null, bandBmpDict, null);
            // Test case pass if frame acknowledge is received.
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Delete the surface for clean up
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if client can accept a RLEX encoded stream with max color in palette.")]
        public void RDPEGFX_ClearCodec_PositiveTest_Subcodec_MaxPlatteColor()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Subcodec layer Bitmap(max colors in palette) Messages to client.");
            // Init a subcodec bitmap with max(127) type of color
            BMP_INFO bmp_info = new BMP_INFO();
            bmp_info.scID = CLEARCODEC_SUBCODEC_ID.SUBCODEC_RLEX;
            bmp_info.bmp = RdpegfxTestUtility.DrawGradientImage(RdpegfxTestUtility.ccSubcodecMaxWidth, RdpegfxTestUtility.ccSubcodecHeight, Color.Red);
            Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecBmpDict = new Dictionary<RDPGFX_POINT16, BMP_INFO>();
            subcodecBmpDict.Add(RdpegfxTestUtility.imgPos, bmp_info);
            // Set clearcodec subcodec layer bitmap rectangle, relative to surface
            RDPGFX_RECT16 scRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccSubcodecMaxWidth, RdpegfxTestUtility.ccSubcodecHeight);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        scRect, null, null, subcodecBmpDict);
            // Test case pass if frame acknowledge is received.
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
        [Description("This test case is used to test if RDP client can accept a ClearCodec Encoded bitmap with width larger than max width(32766).")]
        public void RDPEGFX_ClearCodec_NegativeTest_TooBigWidthBitmap()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size.");
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos3, RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap(width > 32766) Messages to client.");
            // Init a too big width(32767) bitmap to client
            Bitmap maxBitmap = RdpegfxTestUtility.DrawImage((ushort)(RdpegfxTestUtility.MaxBmpWidth + 1), RdpegfxTestUtility.surfHeight, Color.Azure);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)(RdpegfxTestUtility.MaxBmpWidth+1), RdpegfxTestUtility.surfHeight);          
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, ccRect, maxBitmap, null, null);
            // Test case pass if RDP connection is stopped due to too wide bitmap are encoded in clearcodec stream.
            RDPClientTryDropConnection("a ClearCodec Encoded bitmap with width larger than 32766");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if RDP client can accept a ClearCodec Encoded bitmap with height larger than max height(32766).")]
        public void RDPEGFX_ClearCodec_NegativeTest_TooBigHeightBitmap()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size.");
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.desktopWidth, RdpegfxTestUtility.MaxBmpHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos3, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.MaxBmpHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap(height> 32766) Messages to client.");
            // Init too big height(32767) bitmap
            Bitmap maxBitmap = RdpegfxTestUtility.DrawImage(RdpegfxTestUtility.surfWidth, (ushort)(RdpegfxTestUtility.MaxBmpHeight + 1), Color.Azure);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, (ushort)(RdpegfxTestUtility.MaxBmpHeight+1));          
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            // Send encoded clearcodec bitmap data in w2s_1 PDU           
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, fillSurfRect, maxBitmap, null, null);
            // Test case pass if RDP connection is stopped due to too high bitmap are encoded in clearcodec stream.
            RDPClientTryDropConnection("a ClearCodec Encoded bitmap with height larger than 32766");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if a glyph larger than max number(1024) square pixels can be handled by the client.")]
        public void RDPEGFX_ClearCodec_NegativeTest_TooBigGlyph()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encoded Glyph with larger than max number(1024) square pixels to client.");
            // Init a glyph with 1025 (41*25) square pixels
            Image bgImage;
            Image ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth2, RdpegfxTestUtility.ccBandHeight2, out bgImage);

            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, (Bitmap)ccBandImage);
            // Set glyph bitmap rectangle, relative to surface
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth2, RdpegfxTestUtility.ccBandHeight2);
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX;
            // Send encoded clearcodec glyph data in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect, null, bandBmpDict, null);
            // Test case pass if RDP connection is stopped due to too large size bitmap are encoded in clearcodec stream.
            RDPClientTryDropConnection("a ClearCodec Encoded bitmap with a glyph larger than 1024 square pixels");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if a clearcodec stream with glyph_hit flag set only can be handled by the client.")]
        public void RDPEGFX_ClearCodec_NegativeTest_GlyphHitFlagSet_GlyphIndexFlagNotSet()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Clearcodec Encoded Messages with Glyph_Hit flag set without glyph_index flag set to client.");
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            // Set glyph_hit flag enabled, but glyph_index flag disabled
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_HIT;
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, glyphRect, null, null, null);
            // Test case pass if RDP connection is stopped due to glyph_hit flag is enabled, but glyph_index flag is disabled in clearcodec stream.
            RDPClientTryDropConnection("a ClearCodec stream with glyphhit only flag set");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if a clearcodec stream with glyph_hit and glyph_index flag set but glyph index is not used can be handled by the client.")]
        public void RDPEGFX_ClearCodec_NegativeTest_UnusedGlyphIndex()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data Messages with unused glyph index to client.");
            // Init clearcodec glyph rectangle and glyph flag
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_HIT | ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX;
            // Send encoded clearcodec bitmap data with unused glyph index set in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect, null, null, null);
            // Test case pass if RDP connection is stopped due to unused glyph index are encoded in clearcodec stream.
            RDPClientTryDropConnection("a ClearCodec stream with glyph_hit and glyph_index flag set but glyph index is unused");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if a clearcodec stream with glyph_hit and glyph_index flag set but glyph index is out of range(0, 3999) can be handled by the client.")]
        public void RDPEGFX_ClearCodec_NegativeTest_TooBigGlyphIndex()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data Messages with too big glyph index(0xffff) to client.");
            // Init clearcodec glyph rectangle and glyph flag
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_HIT | ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX;
            // Set glyph index to 4000(valid range is 0 ~ 3999)
            ushort ccGlyphIndex = RdpegfxTestUtility.ccMaxGlyphIndexNum;
            // Send encoded clearcodec bitmap data with too big glyph index set in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        ccGlyphIndex, glyphRect, null, null, null);
            // Test case pass if RDP connection is stopped due to too big glyph index are encoded in clearcodec stream.
            RDPClientTryDropConnection("a ClearCodec stream with glyph_hit and glyph_index flag set but glyph index is out of range(0, 3999)");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if client can support too much(4001) ClearCodec glyph slot.")]
        public void RDPEGFX_ClearCodec_NegativeTest_TooMuchGlyphSlot()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Data Messages to client to allocate more than max glyph slot(4000).");
            // Init a small glyph with width is 1, height is 2
            ushort width = RdpegfxTestUtility.ccSmallGlyphWidth;
            ushort height = RdpegfxTestUtility.ccSmallGlyphHeight;
            Image ccGlyph = RdpegfxTestUtility.DrawImage(width, height, Color.Violet);

            // Set start position of a glyph row
            RDPGFX_POINT16 startPos = RdpegfxTestUtility.imgPos;
            // Calculate glyph number in a row, these glyphs will be sent in same w2s_1 PDU.
            ushort glyphNumInBatch = (ushort)(surf.Width - (startPos.x + width) + 1);
            ushort startGlyphIdx = 0;
            // Set glyph number to 4001, then get full row glyph number, 
            ushort count = (ushort)((RdpegfxTestUtility.ccMaxGlyphIndexNum +1)/ glyphNumInBatch);
            ushort tailNum = (ushort)((RdpegfxTestUtility.ccMaxGlyphIndexNum + 1) % glyphNumInBatch);

            // Send a row of glyph in one w2s_1 frame
            for (ushort i = 0; i < count; i++)
            {
                fid = this.rdpegfxAdapter.SendClearCodecGlyphInBatch(surf, startGlyphIdx, startPos, glyphNumInBatch, ccGlyph);
                this.rdpegfxAdapter.ExpectFrameAck(fid);

                startGlyphIdx += glyphNumInBatch;
                startPos.y += (ushort)(height + 1);
            }

            // Send the left number glyph in a rdpegfx frame
            if (tailNum != 0)
            {
                fid = this.rdpegfxAdapter.SendClearCodecGlyphInBatch(surf, startGlyphIdx, startPos, tailNum, ccGlyph);
            }

            // Test case pass if RDP connection is stopped due to client failed to allocate 4001 glyph slot in cache.
            RDPClientTryDropConnection("a ClearCodec stream with more than max ClearCodec glyph slot(4000)");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if the client accepts a ClearCodec stream with graph index hit flag set but composite payload exists.")]
        public void RDPEGFX_ClearCodec_NegativeTest_GlyphIndexHitSet_CompositePayloadExists()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages to client and instruct client to cache it.");
            // Init a bitmap to be clearcodec encoded
            Image bgImage;
            Image ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight, out bgImage);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 glyphRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            // Set glyph index flag enable to instruct client cache the bitmap
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX;
            // Send encoded clearcodec bitmap data with glyph cache is 999 in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect, ccBandImage, null, null);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages with glyph hit flag is set but composite payload exists to client.");
            // Set new clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 glyphRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos2, RdpegfxTestUtility.ccBandWidth, RdpegfxTestUtility.ccBandHeight);
            // Set glyph hit flag enabled to instruct client to use glyph cache
            ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX | ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_HIT;
            // Set test type to instruct clearcodec encoder to include both glyph hit flag and compsite payload in w2s_1 PDU
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.ClearCodec_GraphHitFlagSet_CompositePayloadExist);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        RdpegfxTestUtility.ccGlyphIndex, glyphRect2, bgImage, null, null);
            // Test case pass if RDP connection is stopped due to both glyph hit flag and compsite payload exist in clearcodec stream
            RDPClientTryDropConnection("a ClearCodec stream with graph index hit flag set but composite payload exists");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if the client accepts a ClearCodec stream with redundant RunLengthFactor2 in residual layer.")]
        public void RDPEGFX_ClearCodec_NegativeTest_RedundantRunLengthFactor2()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages with residual RLF1 less than 255 but RLF2 exists to client.");
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)testData.ClearCodecImage.Width, 
                                                                    (ushort)testData.ClearCodecImage.Height);
            // Set test type to instruct clearcodec encoder to generate a redundant RLF2
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.ClearCodec_Residual_RedundantRunLengthFactor2);  // RLF Data: 0xf0 00 40 
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, ccRect, testData.ClearCodecImage, null, null);
            // Test case pass if RDP connection is stopped due to redundant RLF2 in clearcodec stream
            RDPClientTryDropConnection("a ClearCodec stream with redundant RunLengthFactor2 or 3 in residual layer");
            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if the client accepts a ClearCodec stream with RunLengthFactor1 is 0xff but RunLengthFactor2 is absent in residual layer.")]
        public void RDPEGFX_ClearCodec_NegativeTest_AbsentRunLengthFactor2()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages with RLF1 is 0xff but RLF2 is absent in residual layer to client.");
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)testData.ClearCodecImage.Width,
                                                                    (ushort)testData.ClearCodecImage.Height);
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            // Set test type to instruct clearcodec encoder to remove RLF2 part
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.ClearCodec_Residual_AbsentRunLengthFactor2);  // RLF data: 0xff 
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, ccRect, testData.ClearCodecImage, null, null);
            // Test case pass if RDP connection is stopped due to RLF1 is 0xff but RLF2 is absent in clearcodec stream
            RDPClientTryDropConnection("a ClearCodec stream with absent RunLengthFactor2 in residual layer");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if the client accepts a ClearCodec stream with RunLengthFactor2 is less than 0xffff but RunLengthFactor3 exist in residual layer.")]
        public void RDPEGFX_ClearCodec_NegativeTest_RedundantRunLengthFactor3()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size.");
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos3, RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages to client. In this stream, the RunLengthFactor2 is less than 0xffff and the RunLengthFactor3 exists in the residual layer.");
            // Init a residual layer bitmap for clearcodec encoding
            Bitmap resBitmap = RdpegfxTestUtility.DrawImage(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight, Color.Azure);
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)resBitmap.Width, (ushort)resBitmap.Height);
            // Set test type to instruct clearcodec encoder to add redundant RLF3
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.ClearCodec_Residual_RedundantRunLengthFactor3);  // RLF data: 0xff f0 f0 00 ff 3f 10
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, ccRect, resBitmap, null, null);
            // Test case pass if RDP connection is stopped due to redundant RLF3 in clearcodec stream
            RDPClientTryDropConnection("a ClearCodec stream with redundant RunLengthFactor3 in residual layer");
            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if the client accepts a ClearCodec stream with absent RunLengthFactor3 in residual layer.")]
        public void RDPEGFX_ClearCodec_NegativeTest_AbsentRunLengthFactor3()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size.");
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos3, RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages with  RLF2 is 0xffff but RLF3 is absent in residual layer to client.");
            // Init a residual layer bitmap for clearcodec encoding
            Bitmap resBitmap = RdpegfxTestUtility.DrawImage(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.surfHeight, Color.Azure);
            byte ccFlag = ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE;
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)resBitmap.Width, (ushort)resBitmap.Height);
            // Set test type to instruct clearcodec encoder to remove RLF3 part
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.ClearCodec_Residual_AbsentRunLengthFactor3);  // RLF data: 0xff ff ff 
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ccFlag,
                                                        0, ccRect, resBitmap, null, null);
            // Test case pass if RDP connection is stopped due to absent RLF3 in clearcodec stream
            RDPClientTryDropConnection("a ClearCodec stream with absent RunLengthFactor3 in residual layer");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test ClearCodec bitmap transition with larger than 52 pixels band height.")]
        public void RDPEGFX_ClearCodec_NegativeTest_TooBigBandHeight()
        {
            uint fid;

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
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Band Messages with 53 pixels height to client.");
            // Init a band with height is 53 pixels            
            Image bgImage;
            Bitmap ccBandImage = RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccBandWidth,
                                                                        (ushort)(RdpegfxTestUtility.ccMaxBandHeight+1), out bgImage);
            RDPGFX_RECT16 ccRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)ccBandImage.Width, (ushort)ccBandImage.Height);

            Dictionary<RDPGFX_POINT16, Bitmap> bandBmpDict = new Dictionary<RDPGFX_POINT16, Bitmap>();
            bandBmpDict.Add(RdpegfxTestUtility.imgPos, ccBandImage);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        ccRect, null, bandBmpDict, null);
            // Test case pass if RDP connection is stopped due to bandBmpDict height is too big(>52 pixels) 
            RDPClientTryDropConnection("a ClearCodec stream with band height larger than 52 pixels");
        }
                
        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if the client accepts a ClearCodec stream with RLEX encoded bytes number larger than 3 * width * height.")]
        public void RDPEGFX_ClearCodec_NegativeTest_SubCodecRLEX_EncodedBytesTooMuch()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending ClearCodec Encoded Bitmap Messages with RLEX encoded bytes number larger than 3 * width * height to client.");
            // Set a subcodec bitmap with each pixel color is unique for clearcodec RLEX encoding
            BMP_INFO bmp_info = new BMP_INFO();
            bmp_info.scID = CLEARCODEC_SUBCODEC_ID.SUBCODEC_RLEX;
            bmp_info.bmp = (Bitmap)RdpegfxTestUtility.DrawImageWithUniqueColor(RdpegfxTestUtility.ccSubcodecWidth, RdpegfxTestUtility.ccSubcodecHeight, Color.Red);                                        
            Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecBmpDict = new Dictionary<RDPGFX_POINT16, BMP_INFO>();
            subcodecBmpDict.Add(RdpegfxTestUtility.imgPos, bmp_info);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 scRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.ccSubcodecWidth, RdpegfxTestUtility.ccSubcodecHeight);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        scRect, null, null, subcodecBmpDict);
            // Test case pass if RDP connection is stopped due to RLEX encoded bytes is larger than 3 * width * height in subcodec layer.
            RDPClientTryDropConnection("a ClearCodec stream with RLEX encoded bytes larger than 3 * width * height in subcodec layer");            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("This test case is used to test if the client accepts a ClearCodec stream with incorrect palette count in subcodec layer.")]
        public void RDPEGFX_ClearCodec_NegativeTest_SubCodecRLEX_IncorrectPlatteCount()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Encode Header/Data Messages to client.");
            // Init a subcodec bitmap for clearcodec RLEX encoding
            Image bgImage;
            BMP_INFO bmp_info = new BMP_INFO();
            bmp_info.scID = CLEARCODEC_SUBCODEC_ID.SUBCODEC_RLEX;
            bmp_info.bmp = (Bitmap)RdpegfxTestUtility.captureFromImage(testData.ClearCodecImage, RdpegfxTestUtility.imgPos,
                                                RdpegfxTestUtility.ccSubcodecWidth, RdpegfxTestUtility.ccSubcodecHeight, out bgImage);
            Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecBmpDict = new Dictionary<RDPGFX_POINT16, BMP_INFO>();
            subcodecBmpDict.Add(RdpegfxTestUtility.imgPos, bmp_info);
            // Set clearcodec bitmap rectangle, relative to surface
            RDPGFX_RECT16 scRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            // test type to instruct clearcodec encoder to add correct Palette Count with 1 in RLEX encoding
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.ClearCodec_Band_Subcodec_IncorrectPaletteCount);
            // Send encoded clearcodec bitmap data in w2s_1 PDU
            fid = this.rdpegfxAdapter.SendImageWithClearCodec(surf.Id, PixelFormat.PIXEL_FORMAT_XRGB_8888, ClearCodec_BitmapStream.CLEARCODEC_FLAG_NONE, 0,
                                                        fillSurfRect, null, null, subcodecBmpDict);
            // Test case pass if RDP connection is stopped due to incorrect Palette Count in RLEX encoding
            RDPClientTryDropConnection("a ClearCodec stream with incorrect palette count in subcodec layer");            
        }
    }
}
