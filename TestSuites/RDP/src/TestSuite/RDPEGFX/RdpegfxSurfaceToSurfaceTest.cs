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
        [Description("This test case is used to test intra-SurfacetoSurface copy command.")]
        public void RDPEGFX_SurfaceToSurface_PositiveTest_IntraSurfaceCopy()
        {
            uint fid;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { RdpegfxTestUtility.copySrcRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Fill a rectangle with red color and copy it to another position of the surface
            RDPGFX_POINT16[] destPts = {RdpegfxTestUtility.imgPos2};
            fid = this.rdpegfxAdapter.IntraSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, destPts);

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
        [Description("This test case is used to test inter-SurfacetoSurface copy command.")]
        public void RDPEGFX_SurfaceToSurface_PositiveTest_InterSurfaceCopy()
        {
            uint fid;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface, then fill it with blue color 
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos2, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf2.Id);

            RDPGFX_RECT16 fillSurfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            RDPGFX_RECT16[] fillRects2 = { fillSurfRect2 };
            fid = this.rdpegfxAdapter.SolidFillSurface(surf2, RdpegfxTestUtility.fillColorBlue, fillRects2);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);


            RDPGFX_POINT16[] destPts = { RdpegfxTestUtility.imgPos2 };   // Relative to destination surface 
            fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, RdpegfxTestUtility.fillColorRed, surf2, destPts);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, new RDPGFX_RECT16[] { surfRect, surfRect2 });

            // Delete the surfaces
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
            this.rdpegfxAdapter.DeleteSurface(surf2.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf2.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify whether RDP client can process correctly when receive a RDPGFX_SURFACE_TO_SURFACE_PDU message with source RECT whose specific borders are overlapped with surface.")]
        public void RDPEGFX_SurfaceToSurface_PositiveTest_SrcRectBorderOverlapSurface()
        {
            uint fid;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface, then fill it with blue color 
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos2, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf2, "Surface {0} is created", surf2.Id);

            RDPGFX_RECT16 fillSurfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            RDPGFX_RECT16[] fillRects2 = { fillSurfRect2 };
            fid = this.rdpegfxAdapter.SolidFillSurface(surf2, RdpegfxTestUtility.fillColorBlue, fillRects2);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Generate Source Rect to overlap its border with surface
            RDPGFX_RECT16 srcRect = new RDPGFX_RECT16();
            srcRect.left = (ushort)(RdpegfxTestUtility.surfWidth - RdpegfxTestUtility.smallWidth);
            srcRect.top = (ushort)(RdpegfxTestUtility.surfHeight - RdpegfxTestUtility.smallHeight);
            srcRect.right = RdpegfxTestUtility.surfWidth;
            srcRect.bottom = RdpegfxTestUtility.surfHeight;
            RDPGFX_POINT16[] destPts = { RdpegfxTestUtility.imgPos2 };   // Relative to destination surface 
            fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, srcRect, RdpegfxTestUtility.fillColorRed, surf2, destPts);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Send RDPGFX_SURFACE_TO_SURFACE_PDU message in frame {0}, specific borders of source rect are overlapped with surface.", fid);
            
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, new RDPGFX_RECT16[] { surfRect, surfRect2 });

            // Delete the surfaces
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
            this.rdpegfxAdapter.DeleteSurface(surf2.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf2.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify whether RDP client can process correctly when receive a RDPGFX_SURFACE_TO_SURFACE_PDU message with destRects whose specific borders are overlapped with surface.")]
        public void RDPEGFX_SurfaceToSurface_PositiveTest_DestRectsBorderOverlapSurface()
        {
            uint fid;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface, then fill it with blue color 
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos2, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf2.Id);

            RDPGFX_RECT16 fillSurfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            RDPGFX_RECT16[] fillRects2 = { fillSurfRect2 };
            fid = this.rdpegfxAdapter.SolidFillSurface(surf2, RdpegfxTestUtility.fillColorBlue, fillRects2);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Generate destPts array
            List<RDPGFX_POINT16> destPtsList = new List<RDPGFX_POINT16>();
            // Top-right corner
            int x = RdpegfxTestUtility.surfWidth2 -RdpegfxTestUtility.copySrcRect.Width;
            int y = 0;
            destPtsList.Add(new RDPGFX_POINT16((ushort)x, (ushort)y));

            // Bottom-right corner
            x = RdpegfxTestUtility.surfWidth2 - RdpegfxTestUtility.copySrcRect.Width;
            y = RdpegfxTestUtility.surfHeight2 - RdpegfxTestUtility.copySrcRect.Height;
            destPtsList.Add(new RDPGFX_POINT16((ushort)x, (ushort)y));

            // Bottom-left corner
            x = 0;
            y = RdpegfxTestUtility.surfHeight2 - RdpegfxTestUtility.copySrcRect.Height;
            destPtsList.Add(new RDPGFX_POINT16((ushort)x, (ushort)y));

            fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, RdpegfxTestUtility.fillColorRed, surf2, destPtsList.ToArray());
            this.TestSite.Log.Add(LogEntryKind.Debug, "Send RDPGFX_SURFACE_TO_SURFACE_PDU message in frame {0}, specific borders of dest rects are overlapped with surface.", fid);
            
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, new RDPGFX_RECT16[] { surfRect, surfRect2 });

            // Delete the surfaces
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
            this.rdpegfxAdapter.DeleteSurface(surf2.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf2.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify whether RDP client can process correctly when receive a RDPGFX_SURFACE_TO_SURFACE_PDU message with destRects partially overlapped with each other.")]
        public void RDPEGFX_SurfaceToSurface_PositiveTest_DestRectsOverlapped()
        {
            uint fid;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface, then fill it with blue color 
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos2, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf2.Id);

            RDPGFX_RECT16 fillSurfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            RDPGFX_RECT16[] fillRects2 = { fillSurfRect2 };
            fid = this.rdpegfxAdapter.SolidFillSurface(surf2, RdpegfxTestUtility.fillColorBlue, fillRects2);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame {0}, dest rects are overlapped each other", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Generate destPts array
            List<RDPGFX_POINT16> destPtsList = new List<RDPGFX_POINT16>();
            // First point
            int x = 0;
            int y = 0;
            destPtsList.Add(new RDPGFX_POINT16((ushort)x, (ushort)y));

            // Second Point, the rect ovelap partial of the first one
            x = RdpegfxTestUtility.copySrcRect.Width / 2;
            y = RdpegfxTestUtility.copySrcRect.Height / 2;
            destPtsList.Add(new RDPGFX_POINT16((ushort)x, (ushort)y));                       

            fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, RdpegfxTestUtility.fillColorRed, surf2, destPtsList.ToArray());
            this.TestSite.Log.Add(LogEntryKind.Debug, "Send RDPGFX_SURFACE_TO_SURFACE_PDU message in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, new RDPGFX_RECT16[] { surfRect, surfRect2 });

            // Delete the surfaces
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
            this.rdpegfxAdapter.DeleteSurface(surf2.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf2.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify whether RDP client can process correctly when receive a RDPGFX_SURFACE_TO_SURFACE_PDU message whose dest rect is partially overlapped with src rect.")]
        public void RDPEGFX_SurfaceToSurface_PositiveTest_DestRectOverlapSourceRect()
        {
            uint fid;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.smallWidth, RdpegfxTestUtility.smallHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Solid fill first area of surface by Green in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            fillSurfRect = new RDPGFX_RECT16(RdpegfxTestUtility.smallWidth, RdpegfxTestUtility.smallHeight, (ushort)(RdpegfxTestUtility.smallWidth*2), (ushort)(RdpegfxTestUtility.smallHeight*2));
            fillRects = new RDPGFX_RECT16[] { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorRed, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Solid fill first area of surface by Red in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Fill a rectangle with red color and copy it to another position of the surface
            RDPGFX_POINT16[] destPts = { new RDPGFX_POINT16(RdpegfxTestUtility.smallWidth, RdpegfxTestUtility.smallHeight) };
            RDPGFX_RECT16 srcRect = new RDPGFX_RECT16(0, 0, (ushort)(RdpegfxTestUtility.smallWidth * 2), (ushort)(RdpegfxTestUtility.smallHeight * 2));
            fid = this.rdpegfxAdapter.IntraSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, destPts);

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
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Attempt to copy bitmap from inexistent source surface to destination surface")]
        public void RDPEGFX_SurfaceToSurface_Negative_InterSurfaceCopy_InexistentSrc()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos2, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf2.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_InexistentSrc);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_InexistentSrc);
            
            try
            {
                // Trigger client to copy bitmap from inexistent source to destination surface
                RDPGFX_POINT16[] destPts = { RdpegfxTestUtility.imgPos2 };
                this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to copy bitmap from inexistent source to destination surface.");
                fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, RdpegfxTestUtility.fillColorRed, surf2, destPts);
                
                RDPClientTryDropConnection("copy bitmap from inexistent source to destination surface");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "SUT should terminate the connection, or deny the request, or ignore the request to solid fill to inexistent surface instead of throw out an exception: {0}.", ex.Message);
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Attempt to copy bitmap from source surface to inexistent destination surface")]
        public void RDPEGFX_SurfaceToSurface_Negative_InterSurfaceCopy_InexistentDest()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos2, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf2.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_InexistentDest);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_InexistentDest);
                        
            try
            {
                // Trigger client to copy bitmap from source to inexistent destination surface
                RDPGFX_POINT16[] destPts = { RdpegfxTestUtility.imgPos2 };
                this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to copy bitmap from source to inexistent destination surface.");
                fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, RdpegfxTestUtility.fillColorRed, surf2, destPts);
                
                RDPClientTryDropConnection("copy bitmap from source to inexistent destination surface");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "SUT should terminate the connection, or deny the request, or ignore the request to solid fill to inexistent surface instead of throw out an exception: {0}.", ex.Message);
            }
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Attempt to copy bitmap with boundary out of source surface to destination surface")]
        public void RDPEGFX_SurfaceToSurface_Negative_InterSurfaceCopy_SrcOutOfBoundary()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf2.Id);
                        
            try
            {
                // Trigger client to copy bitmap with boundary out of source surface to destination surface
                RDPGFX_POINT16[] destPts = { RdpegfxTestUtility.imgPos2 };   // Relative to destination surface 
                this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to copy bitmap with boundary out of source surface to destination surface.");
                fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect2, RdpegfxTestUtility.fillColorRed, surf2, destPts);

                RDPClientTryDropConnection("copy bitmap with boundary out of source surface to destination surface");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "SUT should terminate the connection, or deny the request, or ignore the request to solid fill to inexistent surface instead of throw out an exception: {0}.", ex.Message);
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Attempt to copy bitmap of source surface to illegal position outside destination surface")]
        public void RDPEGFX_SurfaceToSurface_Negative_InterSurfaceCopy_DestOutOfBoundary()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf2.Id);
                        
            try
            {
                // Trigger client to copy bitmap of source surface to illegal position outside destination surface
                RDPGFX_POINT16[] destPts = { RdpegfxTestUtility.imgPos4 };   // imgPos4 is a position outside destination surface
                this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to copy bitmap of source surface to illegal position outside destination surface.");
                fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, RdpegfxTestUtility.fillColorRed, surf2, destPts);

                RDPClientTryDropConnection("copy bitmap of source surface to illegal position outside destination surface");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "SUT should terminate the connection, or deny the request, or ignore the request to solid fill to inexistent surface instead of throw out an exception: {0}.", ex.Message);
            }
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Check if the client can handle the situation where value of destPtsCount and the length of destPts doesn't match")]
        public void RDPEGFX_SurfaceToSurface_Negative_DestPtsMismatch()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create & output source surface, then fill it with green color
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Create & output destination surface
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth3, RdpegfxTestUtility.surfHeight3);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            Site.Assert.IsNotNull(surf, "Surface {0} is created", surf2.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_DestPtsCount_Mismatch);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_DestPtsCount_Mismatch);
            
            try
            {
                // Send a SurfaceToSurface PDU to client with value of destPtsCount and the length of destPts doesn't match 
                RDPGFX_POINT16[] destPts = { RdpegfxTestUtility.imgPos2 };   // Relative to destination surface 
                this.TestSite.Log.Add(LogEntryKind.Comment, "Send a SurfaceToSurface PDU to client with value of destPtsCount and the length of destPts doesn't match.");
                fid = this.rdpegfxAdapter.InterSurfaceCopy(surf, RdpegfxTestUtility.copySrcRect, RdpegfxTestUtility.fillColorRed, surf2, destPts);
                                
                RDPClientTryDropConnection("a SurfaceToSurface PDU to client with value of destPtsCount and the length of destPts doesn't match");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "SUT should terminate the connection, or deny the request, or ignore the request to solid fill to inexistent surface instead of throw out an exception: {0}.", ex.Message);
            }
        }
    }
}
