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
        [Description("This test case is used to test SurfacetoCache, and CacheToSurface command.")]
        public void RDPEGFX_CacheManagement_PositiveTest()
        {
            RDPEGFX_CacheManagement(DynamicVC_TransportType.RDP_TCP, false);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test SurfacetoCache, and CacheToSurface command over reliable UDP transport using Soft Sync Negotiation.")]
        public void RDPEGFX_CacheManagement_PositiveTest_OverUDP_Reliable_SoftSync()
        {
            RDPEGFX_CacheManagement(DynamicVC_TransportType.RDP_UDP_Reliable, true);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to test SurfacetoCache, and CacheToSurface command over lossy UDP transport using Soft Sync Negotiation.")]
        public void RDPEGFX_CacheManagement_PositiveTest_OverUDP_Lossy_SoftSync()
        {
            RDPEGFX_CacheManagement(DynamicVC_TransportType.RDP_UDP_Lossy, true);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process cache correctly when receiving RDPGFX_SURFACE_TO_CACHE_PDU with the max cacheSlot number.")]
        public void RDPEGFX_CacheManagement_PositiveTest_SurfaceToCache_MaxCacheSlot()
        {
            ushort maxCacheSlot = RdpegfxTestUtility.maxCacheSlot;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            if (this.isSmallCache)
            {
                maxCacheSlot = RdpegfxTestUtility.maxCacheSlotForSmallCache;
            }

            // Create a surface 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Surface.");
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);
                        
            List<RDPGFX_POINT16> destPointList = new List<RDPGFX_POINT16>();
            RDPGFX_POINT16 pos = new RDPGFX_POINT16(RdpegfxTestUtility.cacheRect.right, RdpegfxTestUtility.cacheRect.bottom);
            destPointList.Add(pos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Copy a rect to cache; and copy the cached rect to surface, using max cacheSlot: {0}.", maxCacheSlot);
            uint fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, RdpegfxTestUtility.cacheRect, RdpegfxTestUtility.cacheKey, destPointList.ToArray(), maxCacheSlot, RdpegfxTestUtility.fillColorRed);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled by cached bitmap in frame: {0}, use max cacheSlot number {1}.", fid, maxCacheSlot);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface after wait 3 seconds.
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process cache correctly when its cache reached max size.")]
        public void RDPEGFX_CacheManagement_PositiveTest_SurfaceToCache_MaxCacheSize()
        {
            int maxCacheSize = RdpegfxTestUtility.maxCacheSize;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            if (this.isSmallCache)
            {
                maxCacheSize = RdpegfxTestUtility.maxCacheSizeForSmallCache;
            }
            // Create a surface 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Surface.");
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.largeSurfWidth, RdpegfxTestUtility.largeSurfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Add Image to cache, 1M for each image
            this.TestSite.Log.Add(LogEntryKind.Comment, "Copy rects of surface to cache, each rect is 1M, there are {0} slot in total to reach max size.", maxCacheSize);
            for (ushort slot = 1; slot <= maxCacheSize; slot++)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Copy a rect of surface to cache, slot is {0}.", slot);
                uint fid = this.rdpegfxAdapter.CacheSurface(surf, RdpegfxTestUtility.largeCacheRect, RdpegfxTestUtility.cacheKey, slot, RdpegfxTestUtility.fillColorRed);
                this.rdpegfxAdapter.ExpectFrameAck(fid);
            }
            
            // Delete the surface after wait 3 seconds.
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process correctly when receiving RDPGFX_SURFACE_TO_CACHE_PDU whose srcRect specific borders overlapped with surface.")]
        public void RDPEGFX_CacheManagement_PositiveTest_SurfaceToCache_SrcRectBorderOverlapSurface()
        {
            ushort maxCacheSlot = RdpegfxTestUtility.maxCacheSlot;

            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            if (this.isSmallCache)
            {
                maxCacheSlot = RdpegfxTestUtility.maxCacheSlotForSmallCache;
            }
            
            // Create a surface 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Surface.");
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Create srcRect, in the bottom-right corner of surface
            RDPGFX_RECT16 srcRect = new RDPGFX_RECT16();
            srcRect.left = (ushort)(RdpegfxTestUtility.surfWidth - RdpegfxTestUtility.smallWidth);
            srcRect.top = (ushort)(RdpegfxTestUtility.surfHeight - RdpegfxTestUtility.smallHeight);
            srcRect.right = RdpegfxTestUtility.surfWidth;
            srcRect.bottom = RdpegfxTestUtility.surfHeight;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Copy a rect to cache; and copy the cached rect to surface.");
            RDPGFX_POINT16[] destPoints = new RDPGFX_POINT16[] { RdpegfxTestUtility.imgPos };
            uint fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, srcRect, RdpegfxTestUtility.cacheKey, destPoints, null, RdpegfxTestUtility.fillColorRed);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled by cached bitmap in frame: {0}.", fid, maxCacheSlot);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface after wait 3 seconds.
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process cache correctly when receiving RDPGFX_SURFACE_TO_CACHE_PDU with a used cacheSlot number.")]
        public void RDPEGFX_CacheManagement_PositiveTest_SurfaceToCache_UpdateCache()
        {
            ushort cacheSlot = 1;
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();
            
            // Create a surface 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Surface.");
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Cache a rect of Surface to cache.");
            uint fid = this.rdpegfxAdapter.CacheSurface(surf, RdpegfxTestUtility.cacheRect, RdpegfxTestUtility.cacheKey, cacheSlot, RdpegfxTestUtility.fillColorRed);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Cache a rect in surface, slot is {0}.", cacheSlot);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Cache a rect of Surface to cache with same cacheSlot.");
            fid = this.rdpegfxAdapter.CacheSurface(surf, RdpegfxTestUtility.cacheRect, RdpegfxTestUtility.cacheKey, cacheSlot, RdpegfxTestUtility.fillColorGreen);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Cache a rect in surface, slot is {0}.", cacheSlot);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Fill the Surface with cached slot.");
            RDPGFX_POINT16[] destPoints = new RDPGFX_POINT16[] { new RDPGFX_POINT16(RdpegfxTestUtility.cacheRect.right, RdpegfxTestUtility.cacheRect.bottom) };
            fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, cacheSlot, destPoints);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface after wait 3 seconds.
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process cache correctly when receiving RDPGFX_CACHE_TO_SURFACE_PDU whose dest rects specific borders overlapped with surface.")]
        public void RDPEGFX_CacheManagement_PositiveTest_CacheToSurface_DestRectsBorderOverlapSurface()
        {
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create a surface 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Surface.");
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            ushort cacheW = (ushort)(RdpegfxTestUtility.cacheRect.right - RdpegfxTestUtility.cacheRect.left);
            ushort cacheH = (ushort)(RdpegfxTestUtility.cacheRect.bottom - RdpegfxTestUtility.cacheRect.top);

            List<RDPGFX_POINT16> destPointList = new List<RDPGFX_POINT16>();
            RDPGFX_POINT16 pos = new RDPGFX_POINT16(
                (ushort)(RdpegfxTestUtility.surfWidth - cacheW),
                (ushort)(RdpegfxTestUtility.surfHeight - cacheH));
            destPointList.Add(pos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Copy a rect to cache; and copy the cached rect to surface.");
            uint fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, RdpegfxTestUtility.cacheRect, RdpegfxTestUtility.cacheKey, destPointList.ToArray(), null, RdpegfxTestUtility.fillColorRed);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled by cached bitmap in frame: {0}.", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface after wait 3 seconds.
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process cache correctly when receiving RDPGFX_CACHE_TO_SURFACE_PDU whose dest rects partially overlapped with each other.")]
        public void RDPEGFX_CacheManagement_PositiveTest_CacheToSurface_DestRectsOverlapped()
        {
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create a surface 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Surface.");
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill a rect to green color
            this.TestSite.Log.Add(LogEntryKind.Comment, "Send solid fill request to client to fill a rect to green color.");
            RDPGFX_RECT16 fillSurfRect = new RDPGFX_RECT16(0, 0, RdpegfxTestUtility.smallWidth, RdpegfxTestUtility.smallHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorGreen, fillRects);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Send solid fill request to client to fill a rect to red color
            this.TestSite.Log.Add(LogEntryKind.Comment, "Send solid fill request to client to fill a rect to red color.");
            RDPGFX_RECT16 fillSurfRect2 = new RDPGFX_RECT16(RdpegfxTestUtility.smallWidth, RdpegfxTestUtility.smallHeight, (ushort)(RdpegfxTestUtility.smallWidth * 2), (ushort)(RdpegfxTestUtility.smallHeight * 2));
            RDPGFX_RECT16[] fillRects2 = { fillSurfRect2 };  // Relative to surface
            fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.fillColorRed, fillRects2);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            RDPGFX_RECT16 srcRect = new RDPGFX_RECT16(0, 0, (ushort)(RdpegfxTestUtility.smallWidth * 2), (ushort)(RdpegfxTestUtility.smallHeight * 2));
            List<RDPGFX_POINT16> destPointList = new List<RDPGFX_POINT16>();
            RDPGFX_POINT16 pos = new RDPGFX_POINT16((ushort)(RdpegfxTestUtility.smallWidth * 2), (ushort)(RdpegfxTestUtility.smallHeight * 2));
            destPointList.Add(pos);
            pos = new RDPGFX_POINT16((ushort)(RdpegfxTestUtility.smallWidth * 3), (ushort)(RdpegfxTestUtility.smallHeight * 3));
            destPointList.Add(pos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Copy a rect to cache; and copy the cached rect to surface.");
            fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, RdpegfxTestUtility.cacheRect, RdpegfxTestUtility.cacheKey, destPointList.ToArray(), null, null);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled by cached bitmap in frame: {0}.", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            RDPGFX_RECT16 verifyRect = new RDPGFX_RECT16(
                (ushort)(RdpegfxTestUtility.smallWidth * 2), (ushort)(RdpegfxTestUtility.smallHeight * 2), 
                (ushort)(RdpegfxTestUtility.smallWidth * 5), (ushort)(RdpegfxTestUtility.smallHeight * 5));
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface after wait 3 seconds.
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]
        [Description("This test case is used to verify RDP client can process cache correctly when receiving RDPGFX_EVICT_CACHE_ENTRY_PDU to delete a slot.")]
        public void RDPEGFX_CacheManagement_PositiveTest_EvictCache_DeleteCacheSlot()
        {
            ushort cacheSlot = 1;
            // Init for capability exchange
            RDPEGFX_CapabilityExchange();

            // Create a surface 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Surface.");
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);


            this.TestSite.Log.Add(LogEntryKind.Comment, "Fill a rect on surface and cache this rect.");
            uint fid = this.rdpegfxAdapter.CacheSurface(surf, RdpegfxTestUtility.cacheRect, RdpegfxTestUtility.cacheKey, cacheSlot, RdpegfxTestUtility.fillColorRed);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Evict the cache slot.");
            fid = this.rdpegfxAdapter.EvictCachEntry(cacheSlot);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            List<RDPGFX_POINT16> destPointList = new List<RDPGFX_POINT16>();
            RDPGFX_POINT16 pos = new RDPGFX_POINT16(RdpegfxTestUtility.cacheRect.right, RdpegfxTestUtility.cacheRect.bottom);
            destPointList.Add(pos);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Copy image from deleted slot.");
            fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, cacheSlot, destPointList.ToArray());

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to drop the connection");
            bool bDisconnected = this.rdpbcgrAdapter.WaitForDisconnection(waitTime);
            this.TestSite.Assert.IsTrue(bDisconnected, "RDP client should terminate the connection when invalid message received.");
        
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("The server attempts to allocate cache slots which exceeds size upper limitation for default cache flag")]
        public void RDPEGFX_CacheManagement_Negative_Default_ExceedMaxCacheSize()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Debug, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending capability confirm with default capability flag to client.");
            // Set capability flag to default, then the max cache size is 100MB
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);

            // Create & output a surface  
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.largeSurfWidth, RdpegfxTestUtility.largeSurfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.CacheManagement_Default_ExceedMaxCacheSize);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.CacheManagement_Default_ExceedMaxCacheSize);

            // Send message to trigger client to allocate cache slots with cache size exceed the max value 100MB
            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to allocate cache slots with cache size exceed the max value 100MB");
            this.rdpegfxAdapter.CacheSurface(surf, RdpegfxTestUtility.largeCacheRect, RdpegfxTestUtility.cacheKey, null, RdpegfxTestUtility.fillColorRed);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to drop the connection");
            bool bDisconnected = this.rdpbcgrAdapter.WaitForDisconnection(waitTime);
            this.TestSite.Assert.IsTrue(bDisconnected, "RDP client should terminate the connection when invalid message received.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Check if client can use an inexistent surface as source for cache successfully")]
        public void RDPEGFX_CacheManagement_Negative_SurfaceToCache_InexistentSurface()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create a surface & output a surface
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.largeSurfWidth, RdpegfxTestUtility.largeSurfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.CacheManagement_SurfaceToCache_InexistentSurface);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.CacheManagement_SurfaceToCache_InexistentSurface);

            // Send message to trigger client to allocate cache slots with cache size exceed the max value 100MB
            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to use an inexistent surface as source for cache");
            this.rdpegfxAdapter.CacheSurface(surf, RdpegfxTestUtility.largeCacheRect, RdpegfxTestUtility.cacheKey, null, RdpegfxTestUtility.fillColorRed);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to drop the connection");
            bool bDisconnected = this.rdpbcgrAdapter.WaitForDisconnection(waitTime);
            this.TestSite.Assert.IsTrue(bDisconnected, "RDP client should terminate the connection when invalid message received.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Check if client can copy cached bitmap data to inexistent surface")]
        public void RDPEGFX_CacheManagement_Negative_CacheToSurface_InexistentSurface()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create a surface & output a surface
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.largeSurfWidth, RdpegfxTestUtility.largeSurfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.CacheManagement_CacheToSurface_InexistentSurface);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.CacheManagement_CacheToSurface_InexistentSurface);

            // Build mutliple cache to surface messages to cover the surface by cacheRect 
            ushort cacheW = (ushort)(RdpegfxTestUtility.largeCacheRect.right - RdpegfxTestUtility.largeCacheRect.left);
            ushort cacheH = (ushort)(RdpegfxTestUtility.largeCacheRect.bottom - RdpegfxTestUtility.largeCacheRect.top);

            ushort currRectTop = 0;

            // DestPointList is a list of positions in destination surface where the bitmap cache will copy to
            List<RDPGFX_POINT16> destPointList = new List<RDPGFX_POINT16>();
            while (currRectTop < surf.Height)
            {
                ushort currRectLeft = 0;
                while (currRectLeft < surf.Width)
                {
                    RDPGFX_POINT16 pos = new RDPGFX_POINT16(currRectLeft, currRectTop);
                    destPointList.Add(pos);
                    currRectLeft += cacheW;
                }
                currRectTop += cacheH;
            }

            // Trigger the client to copy cached bitmap data to inexistent surface
            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger the client to copy cached bitmap data to inexistent surface.");
            uint fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, RdpegfxTestUtility.largeCacheRect, RdpegfxTestUtility.cacheKey, destPointList.ToArray(), null, RdpegfxTestUtility.fillColorRed);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to drop the connection");
            bool bDisconnected = this.rdpbcgrAdapter.WaitForDisconnection(waitTime);
            this.TestSite.Assert.IsTrue(bDisconnected, "RDP client should terminate the connection when invalid message received.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Check if client can copy cached bitmap from an inexistent cache slot to destination surface")]
        public void RDPEGFX_CacheManagement_Negative_CacheToSurface_InexistentCacheSlot()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create a surface & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.largeSurfWidth, RdpegfxTestUtility.largeSurfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.CacheManagement_CacheToSurface_InexistentCacheSlot);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.CacheManagement_CacheToSurface_InexistentCacheSlot);

            // Build mutliple cache to surface messages to cover the surface by cacheRect 
            ushort cacheW = (ushort)(RdpegfxTestUtility.largeCacheRect.right - RdpegfxTestUtility.largeCacheRect.left);
            ushort cacheH = (ushort)(RdpegfxTestUtility.largeCacheRect.bottom - RdpegfxTestUtility.largeCacheRect.top);

            ushort currRectTop = 0;

            // DestPointList is a list of positions in destination surface where the bitmap cache will copy to
            List<RDPGFX_POINT16> destPointList = new List<RDPGFX_POINT16>();
            while (currRectTop < surf.Height)
            {
                ushort currRectLeft = 0;
                while (currRectLeft < surf.Width)
                {
                    RDPGFX_POINT16 pos = new RDPGFX_POINT16(currRectLeft, currRectTop);
                    destPointList.Add(pos);
                    currRectLeft += cacheW;
                }
                currRectTop += cacheH;
            }

            // Trigger the client to copy cached bitmap from an inexsitent cache slot to destination surface
            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger the client to copy cached bitmap from an inexistent cache slot to destination surface.");
            uint fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, RdpegfxTestUtility.largeCacheRect, RdpegfxTestUtility.cacheKey, destPointList.ToArray(), null, RdpegfxTestUtility.fillColorRed);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to drop the connection");
            bool bDisconnected = this.rdpbcgrAdapter.WaitForDisconnection(waitTime);
            this.TestSite.Assert.IsTrue(bDisconnected, "RDP client should terminate the connection when invalid message received.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Non-BVT")]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEGFX")]        
        [Description("Check if client can handle a request of deleting an inexistent cache slot")]
        public void RDPEGFX_CacheManagement_Negative_Delete_InexistentCacheSlot()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Do capability exchange.");
            RDPEGFX_CapabilityExchange();

            // Create a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.largeSurfWidth, RdpegfxTestUtility.largeSurfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Set the test type to {0}.", RdpegfxNegativeTypes.CacheManagement_Delete_InexistentCacheSlot);
            this.rdpegfxAdapter.SetTestType(RdpegfxNegativeTypes.CacheManagement_Delete_InexistentCacheSlot);

            // Trigger the client to delete an inexistent cache slot
            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger the client to delete an inexistent cache slot.");
            this.rdpegfxAdapter.CacheSurface(surf, RdpegfxTestUtility.largeCacheRect, RdpegfxTestUtility.cacheKey, null, RdpegfxTestUtility.fillColorRed);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect SUT to drop the connection");
            bool bDisconnected = this.rdpbcgrAdapter.WaitForDisconnection(waitTime);
            this.TestSite.Assert.IsTrue(bDisconnected, "RDP client should terminate the connection when invalid message received.");
        }

        private void RDPEGFX_CacheManagement(DynamicVC_TransportType transport, bool isSoftSync)
        {
            // Check if SUT supports Soft Sync.
            if(isSoftSync)
                this.TestSite.Assert.IsTrue(isClientSupportSoftSync, "SUT should support Soft-Sync.");

            // Init for capability exchange
            RDPEGFX_CapabilityExchange(transport, isSoftSync);

            // Create a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(RdpegfxTestUtility.surfPos, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_ARGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Build muliple cache to surface messages to cover the surface by cacheRect 
            ushort cacheW = (ushort)(RdpegfxTestUtility.cacheRect.right - RdpegfxTestUtility.cacheRect.left);
            ushort cacheH = (ushort)(RdpegfxTestUtility.cacheRect.bottom - RdpegfxTestUtility.cacheRect.top);

            ushort currRectTop = 0;

            List<RDPGFX_POINT16> destPointList = new List<RDPGFX_POINT16>();
            while (currRectTop < surf.Height)
            {
                ushort currRectLeft = 0;
                while (currRectLeft < surf.Width)
                {
                    RDPGFX_POINT16 pos = new RDPGFX_POINT16(currRectLeft, currRectTop);
                    destPointList.Add(pos);
                    currRectLeft += cacheW;
                }
                currRectTop += cacheH;
            }

            uint fid = this.rdpegfxAdapter.FillSurfaceByCachedBitmap(surf, RdpegfxTestUtility.cacheRect, RdpegfxTestUtility.cacheKey, destPointList.ToArray(), null, RdpegfxTestUtility.fillColorRed);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled by cached bitmap in frame: {0}", fid);

            this.rdpegfxAdapter.ExpectFrameAck(fid);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            this.VerifySUTDisplay(false, surfRect);

            // Delete the surface after wait 3 seconds.
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }
    }
}
