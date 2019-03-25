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
        [Description("This test case is used to test create surface, map to output, solid fill, and delete surface command.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_MapAndFill()
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
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)(RdpegfxTestUtility.surfWidth), RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
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
        [Description("This test case is used to test create surface, solid fill, map to output, and delete surface command.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_FillAndMap()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface.");
            // Create a surface 
            Surface surf = this.rdpegfxAdapter.CreateSurface(RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            this.TestSite.Log.Add(LogEntryKind.Comment, "Fill the surface to solid green color.");
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Map surface to Output
            this.TestSite.Log.Add(LogEntryKind.Comment, "Map surface to output.");
            fid = this.rdpegfxAdapter.MapSurfaceToOutput(surf.Id, RdpegfxTestUtility.surfPos.x, RdpegfxTestUtility.surfPos.y);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP10.6")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test create surface, solid fill, map to output, update scaled output and delete surface command.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_ScaledOutput()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface.");
            // Create a surface 
            Surface surf = this.rdpegfxAdapter.CreateSurface(RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            this.TestSite.Log.Add(LogEntryKind.Comment, "Fill the surface to solid green color.");
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);
            
            // The output will be scaled as the new targetWidth and targetHeight fields specified
            this.TestSite.Log.Add(LogEntryKind.Comment, "Scale the output to bigger size.");
            fid = this.rdpegfxAdapter.ScaledOutput(surf.Id, RdpegfxTestUtility.surfPos4.x, RdpegfxTestUtility.surfPos4.y, RdpegfxTestUtility.largeSurfWidth, RdpegfxTestUtility.largeSurfHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);
            
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
        [Description("This test case is used to verify whether RDP client can process correctly when fill color to an overlapped surface.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_FillOverlappedSurface()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create another surface which overlap partial area of the first surface.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos4, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf2, "Surface {0} is created", surf2.Id);

            // Send solid fill request to client to fill surface with green color
            this.TestSite.Log.Add(LogEntryKind.Comment, "Fill the first surface with solid green color.");
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)(RdpegfxTestUtility.surfWidth), RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            RDPGFX_RECT16 verifyRect = new RDPGFX_RECT16(surfRect.left, surfRect.top, surfRect2.right, surfRect2.bottom);
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
        [Description("This test case is used to verify RDP client can create a surface with max surfaceId: UINT16.max.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_CreateSurface_MaxSurfaceId()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888, ushort.MaxValue);
            this.TestSite.Assert.IsNotNull(surf, "Surface with max ID: {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color

            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)(RdpegfxTestUtility.surfWidth), RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
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
        [Description("This test case is used to verify RDP client can create a surface which just cover the full RDP window.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_CreateSurface_FullWindow()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            ushort desktopWidth = this.rdpbcgrAdapter.SessionContext.DesktopWidth;
            ushort desktopHeight = this.rdpbcgrAdapter.SessionContext.DesktopHeight;
            if (desktopWidth == 0 || desktopHeight == 0)
            {
                this.Site.Assume.Fail("Cannot get Desktop width or height from TS_UD_CS_CORE structure in connection establish phase.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = new RDPGFX_RECT16(0, 0, desktopWidth, desktopHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface with max ID: {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = new RDPGFX_RECT16(0, 0, desktopWidth, desktopHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);

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
        [Description("This test case is used to verify RDP client can create a surface which have max width.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_CreateSurface_MaxWidth()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size, width is {0}, height is {1}.", RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color, surface width is {0}, height is {1}.", RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = new RDPGFX_RECT16(0, 0, RdpegfxTestUtility.MaxBmpWidth, (ushort)RdpegfxTestUtility.desktopHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface with max ID: {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = new RDPGFX_RECT16(0, 0, RdpegfxTestUtility.MaxBmpWidth, (ushort)RdpegfxTestUtility.desktopHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Don't verify the output since the output window is too large 32766*32766
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
        [Description("This test case is used to verify RDP client can process correctly when receiving a RDPGFX_MAP_SURFACE_TO_OUTPUT_PDU to map a surface whose specific borders are overlapped with RDP window.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_MapSurface_SurfaceBorderOverlapWindow()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            ushort desktopWidth = this.rdpbcgrAdapter.SessionContext.DesktopWidth;
            ushort desktopHeight = this.rdpbcgrAdapter.SessionContext.DesktopHeight;
            if (desktopWidth <= RdpegfxTestUtility.surfWidth || desktopHeight <= RdpegfxTestUtility.surfHeight)
            {
                this.Site.Assume.Fail("The Desktop width or height are not applicable for this test case, desktop width must larger than {0}, and desktop height must larger than {1}.", RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = new RDPGFX_RECT16(
                (ushort)(desktopWidth - RdpegfxTestUtility.surfWidth),
                (ushort)(desktopHeight - RdpegfxTestUtility.surfHeight),
                desktopWidth,
                desktopHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface with max ID: {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = new RDPGFX_RECT16(0, 0, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
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
        [Description("This test case is used to verify whether RDP client can delete a surface successfully.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_DeleteSurface()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface.");
            // Create & output a surface             
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf1 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf1, "Surface {0} is created", surf1.Id);

            //Recreate the second surface with same surfaceID as surf1.
            //The operation will fail. Since the client already has a surface with same surfaceId.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create the second surface with same surfaceId {0}. Expect the operation will fail.", surf1.Id);
            try
            {
                Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888, surf1.Id);
                this.TestSite.Assert.IsNull(surf2, "The second surface cannot be created successfully since the surface ID is duplicated.");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "The second surface: {0} cannot be created successfully with error message: {1}", surf1.Id, ex.Message);
            }

            // Delete the first surface
            this.TestSite.Log.Add(LogEntryKind.Comment, "Delete the first surface with ID: {0}.", surf1.Id);
            this.rdpegfxAdapter.DeleteSurface(surf1.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf1.Id);

            //Verify the surface is deleted successuflly by creating a third surface with the same surface ID.
            //Recreate the third surface with same surfaceID as surf1.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create the third surface with same surfaceId {0} after the first surface is deleted.", surf1.Id);
            Surface surf3 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888, surf1.Id);
            this.TestSite.Assert.IsNotNull(surf3, "The third surface {0} is created successfully.", surf1.Id);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can reuse surface Id after a surface is deleted.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_ReuseSurfaceId()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Delete the surface
            this.TestSite.Log.Add(LogEntryKind.Comment, "Delete the surface {0}.", surf.Id);
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a new surface and use the same surface Id {0}.", surf.Id);
            Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888, surf.Id);
            this.TestSite.Assert.IsNotNull(surf2, "Surface {0} is created", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process correctly when receiving a RDPGFX_START_FRAME_PDU with maximum frameId.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_StartFrame_MaxFrameId()
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
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, (ushort)(RdpegfxTestUtility.surfWidth), RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects, uint.MaxValue);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color, use max frameId: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
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
        [Description("This test case is used to verify whether RDP client can process correctly when receive a RDPGFX_SOLIDFILL_PDU with many fillRects.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_SolidFill_ManyFillRects()
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

            // Generate many fillRects on the surface
            List<RDPGFX_RECT16> rectList = new List<RDPGFX_RECT16>();
            for (ushort y = 0; y < RdpegfxTestUtility.surfHeight; y += 2)
            {
                for (ushort x = 0; x < RdpegfxTestUtility.surfWidth; x += 2)
                {
                    RDPGFX_RECT16 fillSurfRect = new RDPGFX_RECT16(x, y, (ushort)(x + 1), (ushort)(y + 1));
                    rectList.Add(fillSurfRect);
                }
            }

            // Send solid fill request to client to fill surface with green color
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, rectList.ToArray());
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}, {1} fillrects in total.", fid, rectList.Count);

            // Expect the client to send a frame acknowledge pdu
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
        [Description("This test case is used to verify whether RDP client can process correctly when receive a RDPGFX_SOLIDFILL_PDU with fillRects whose specific borders are overlapped with surface.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_SolidFill_FillRectsBorderOverlapSurface()
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

            // Generate fillrects in the corner
            List<RDPGFX_RECT16> rectList = new List<RDPGFX_RECT16>();
            // Top-right corner
            RDPGFX_RECT16 fillSurfRect = new RDPGFX_RECT16(
                (ushort)(RdpegfxTestUtility.surfWidth - RdpegfxTestUtility.smallWidth),
                0,
                RdpegfxTestUtility.surfWidth,
                RdpegfxTestUtility.smallHeight);
            rectList.Add(fillSurfRect);
            // bottom-right corner
            fillSurfRect = new RDPGFX_RECT16(
                (ushort)(RdpegfxTestUtility.surfWidth - RdpegfxTestUtility.smallWidth),
                (ushort)(RdpegfxTestUtility.surfHeight - RdpegfxTestUtility.smallHeight),
                RdpegfxTestUtility.surfWidth,
                RdpegfxTestUtility.surfHeight);
            rectList.Add(fillSurfRect);
            // bottom-left corner
            fillSurfRect = new RDPGFX_RECT16(
                0,
                (ushort)(RdpegfxTestUtility.surfHeight - RdpegfxTestUtility.smallHeight),
                RdpegfxTestUtility.smallWidth,
                RdpegfxTestUtility.surfHeight);
            rectList.Add(fillSurfRect);

            // Send solid fill request to client to fill surface with green color            
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, rectList.ToArray());
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
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
        [Description("This test case is used to verify whether RDP client can process correctly when receive a RDPGFX_SOLIDFILL_PDU with fillRects overlapped partially.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_SolidFill_MultiFillRectsOverlapped()
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

            // Generate fillrects in the corner
            List<RDPGFX_RECT16> rectList = new List<RDPGFX_RECT16>();
            // First Rect
            RDPGFX_RECT16 fillSurfRect = new RDPGFX_RECT16(
                0,
                0,
                RdpegfxTestUtility.smallWidth,
                RdpegfxTestUtility.smallHeight);
            rectList.Add(fillSurfRect);
            // Second Rect, overlap partial area of the first rect
            fillSurfRect = new RDPGFX_RECT16(
                (ushort)(RdpegfxTestUtility.smallWidth / 2),
                (ushort)(RdpegfxTestUtility.smallHeight / 2),
                (ushort)(RdpegfxTestUtility.smallWidth / 2 + RdpegfxTestUtility.smallWidth),
                (ushort)(RdpegfxTestUtility.smallHeight / 2 + RdpegfxTestUtility.smallHeight));
            rectList.Add(fillSurfRect);

            // Send solid fill request to client to fill surface with green color            
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, rectList.ToArray());
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
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
        [Description("This test case is used to verify RDP client can process correctly when receiving a RDPGFX_RESET_GRAPHICS_PDU whose width and height are max value(32766).")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_ResetGraphic_MaxHeighWidth()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size, width is {0}, height is {1}.", RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.MaxBmpWidth, RdpegfxTestUtility.desktopHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size, width is {0}, height is {1}.", RdpegfxTestUtility.desktopWidth, RdpegfxTestUtility.MaxBmpHeight);
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.desktopWidth, RdpegfxTestUtility.MaxBmpHeight);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process correctly when receiving a RDPGFX_RESET_GRAPHICS_PDU whose monitorCount is max value(16).")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_ResetGraphic_MaxMonitorCount()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Reset virtual desktop size, width is {0}, height is {1}, monitor count is {2}.", RdpegfxTestUtility.desktopWidth, RdpegfxTestUtility.desktopHeight, RdpegfxTestUtility.maxMonitorCount);
            // Reset graphics of virtual desktop
            fid = this.rdpegfxAdapter.ResetGraphics(RdpegfxTestUtility.desktopWidth, RdpegfxTestUtility.desktopHeight, RdpegfxTestUtility.maxMonitorCount);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test create multiple surfaces and overlapped.")]
        public void RDPEGFX_SurfaceToScreen_PositiveTest_MultiSurfaceOverlap()
        {
            uint fid;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            RDPGFX_RECT16 verifyRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);

            // Init Data for test
            RDPGFX_POINT16[] positions = new RDPGFX_POINT16[4];
            positions[0] = new RDPGFX_POINT16(RdpegfxTestUtility.surfPos.x, RdpegfxTestUtility.surfPos.y);
            positions[1] = new RDPGFX_POINT16((ushort)(RdpegfxTestUtility.surfPos.x + RdpegfxTestUtility.surfWidth / 2), RdpegfxTestUtility.surfPos.y);
            positions[2] = new RDPGFX_POINT16(RdpegfxTestUtility.surfPos.x, (ushort)(RdpegfxTestUtility.surfPos.y + RdpegfxTestUtility.surfHeight / 2));
            positions[3] = new RDPGFX_POINT16(RdpegfxTestUtility.surfPos.x, RdpegfxTestUtility.surfPos.y);

            ushort[] widths = new ushort[4];
            widths[0] = RdpegfxTestUtility.surfWidth;
            widths[1] = (ushort)(RdpegfxTestUtility.surfWidth / 2);
            widths[2] = RdpegfxTestUtility.surfWidth;
            widths[3] = (ushort)(RdpegfxTestUtility.surfWidth / 2);

            ushort[] heights = new ushort[4];
            heights[0] = (ushort)(RdpegfxTestUtility.surfHeight / 2);
            heights[1] = RdpegfxTestUtility.surfHeight;
            heights[2] = (ushort)(RdpegfxTestUtility.surfHeight / 2);
            heights[3] = RdpegfxTestUtility.surfHeight;

            Color[] colors = new Color[4];
            colors[0] = Color.Green;
            colors[1] = Color.Blue;
            colors[2] = Color.Red;
            colors[3] = Color.Yellow;

            Surface[] surfaces = new Surface[4];
            // Test the overlap
            for (int i = 0; i < positions.Length; i++)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with color: {0}.", colors[i]);
                // Create & output a surface 
                RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(positions[i], widths[i], heights[i]);
                surfaces[i] = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
                this.TestSite.Assert.IsNotNull(surfaces[i], "Surface {0} is created", surfaces[i].Id);

                // Send solid fill request to client to fill surface with green color
                RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, widths[i], heights[i]);
                RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
                fid = this.rdpegfxAdapter.SolidFillSurface(surfaces[i], RdpegfxTestUtility.ToRdpgfx_Color32(colors[i]), fillRects);
                this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);

                // Expect the client to send a frame acknowledge pdu
                // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
                this.rdpegfxAdapter.ExpectFrameAck(fid);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
                this.VerifySUTDisplay(false, verifyRect);
            }
            // Delete the surface
            for (int i = 0; i < surfaces.Length; i++)
            {
                if (surfaces[i] != null)
                {
                    this.rdpegfxAdapter.DeleteSurface(surfaces[i].Id);
                    this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surfaces[i].Id);
                }
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("Create a new surface with surfaceId is duplicated with another surface")]
        public void RDPEGFX_SurfaceToScreen_Negative_CreateDuplicateSurface()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create the first surface and map the surface to output");
            // Create & output the first surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.SurfaceManagement_CreateDuplicatedSurface);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.SurfaceManagement_CreateDuplicatedSurface);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create the second surface with same surface id as the first surface and map the surface to output");
            // Create & ouput the second surface with a duplicated surface id
            RDPGFX_RECT16 surfRect2 = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth2, RdpegfxTestUtility.surfHeight2);

            try
            {
                Surface surf2 = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect2, PixelFormat.PIXEL_FORMAT_XRGB_8888);
                this.Site.Assert.IsNotNull(surf2, "Surface {0} is created", surf2.Id);
                RDPClientTryDropConnection("create duplicated surface");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.CheckFailed, "SUT should terminate the connection, or deny the request, or ignore the request to create duplicated surface instead of throw out an exception: {0}.", ex.Message);                
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("Attempt to delete a surface, which has inexistent surfaceId")]
        public void RDPEGFX_SurfaceToScreen_Negative_DeleteInexistentSurface()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
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
            this.TestSite.Log.Add(LogEntryKind.Comment, "Surface is filled with solid color in frame: {0}", fid);

            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Delete the surface
            this.TestSite.Log.Add(LogEntryKind.Comment, "Delete surface {0}", surf.Id);
            this.rdpegfxAdapter.DeleteSurface(surf.Id);

            try
            {
                // Delete an inexistent surface
                this.TestSite.Log.Add(LogEntryKind.Comment, "Try to delete an inexistent surface");
                this.rdpegfxAdapter.DeleteSurface(surf.Id);

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
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("Attempt to map bitmap of an inexistent surface to output")]
        public void RDPEGFX_SurfaceToScreen_Negative_MapInexistentSurfaceToOutput()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.SurfaceManagement_MapInexistentSurfaceToOutput);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.SurfaceManagement_MapInexistentSurfaceToOutput);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and map an inexistent surface to output");            
            
            try
            {
                // Create a surface and output an inexisent surface to output
                RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
                Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
                this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

                RDPClientTryDropConnection("create a surface and map an inexistent surface to output");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "SUT should terminate the connection, or deny the request, or ignore the request to map inexistent surface to output instead of throw out an exception: {0}.", ex.Message);
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("Attempt to fill solid to an inexistent surface")]
        public void RDPEGFX_SurfaceToScreen_Negative_SolidFillToInexistentSurface()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.SurfaceManagement_SolidFill_ToInexistentSurface);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.SurfaceManagement_SolidFill_ToInexistentSurface);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill an inexistent surface with green color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.imgPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
                      
            try
            {
                //Solid fill to an inexistent surface(surfaceID: 0xffff)
                uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
                this.TestSite.Log.Add(LogEntryKind.Comment, "Solid color to fill an inexistent surface in frame: {0}", fid);

                //Expect the RDP client handle the negative request by dropping the connection as Windows does, or deny the request or ignore the request.
                RDPClientTryDropConnection("solid color to fill an inexistent surface");
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.CheckFailed, "SUT should terminate the connection, or deny the request, or ignore the request to create duplicated surface instead of throw out an exception: {0}.", ex.Message);
            }          
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("Check if client can handle a message with incorrect pdu length in RDPGFX_HEADER")]
        public void RDPEGFX_SurfaceToScreen_Negative_IncorrectPduLengthInHeader()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            this.TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.SurfaceToScreen_Incorrect_PduLengthInHeader);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.SurfaceToScreen_Incorrect_PduLengthInHeader);
            
            try
            {
                // Set first capset in capability advertise request, if no capdata in request, use default flag.
                CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
                if (capsAdv.capsSetCount > 0)
                    capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
                this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm with incorrect pdu length in RDPGFX_HEADER to client.");
                this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

                RDPClientTryDropConnection("capability confirm with incorrect pdu length in RDPGFX_HEADER ");                
            }
            catch (Exception ex)
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "SUT should terminate the connection, or deny the request, or ignore the request to solid fill to inexistent surface instead of throw out an exception: {0}.", ex.Message);
            }
        }

    
    }
}
