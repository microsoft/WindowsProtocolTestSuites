// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdprfx;

namespace Microsoft.Protocols.TestSuites.Rdpegfx
{
    public class RdpegfxAdapter : ManagedAdapterBase, IRdpegfxAdapter
    {
        #region Private Variables


        List<RdpegfxPdu> rdpegfxPdusToSent;
        TimeSpan waitTime;
        IRdpbcgrAdapter bcgrAdapter;
        RdpegfxServer egfxServer;
        EGFXRdpSegmentedPdu segPdu;
        SurfaceManager surfManager;
        bool frAckSuspend = false;
        RdpegfxNegativeTypes currentTestType;

        // For RFX codec.
        private RdprfxServer rdprfxServer;
        private List<byte> pendingRfxBuffer;
        private object syncLocker;

        #endregion

        #region Private Methods       

        /// <summary>
        /// Add RDPEGFX pdu into buffer
        /// </summary>
        /// <param name="pdu"></param>
        private void AddPdusToBuffer(RdpegfxPdu pdu)
        {
            lock (rdpegfxPdusToSent)
            {
                rdpegfxPdusToSent.Add(pdu);
            }
        }

        /// <summary>
        /// Encode the RDPEGFX pdus in send buffer
        /// </summary>
        /// <returns></returns>
        private byte[] EncodePdusToSent()
        {
            if (rdpegfxPdusToSent != null && rdpegfxPdusToSent.Count > 0)
            {
                List<byte> byteList = new List<byte>();
                lock (rdpegfxPdusToSent)
                {
                    foreach (RdpegfxServerPdu pdu in rdpegfxPdusToSent)
                    {
                        byteList.AddRange(PduMarshaler.Marshal(pdu));
                    }
                    rdpegfxPdusToSent.Clear();
                }
                return byteList.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Method to pack server data into segment header and send it
        /// </summary>
        private void PackAndSendServerPdu()
        {
            byte[] frameData = EncodePdusToSent();
            if (frameData != null)
            {
                SendRdpegfxFrameInSegment(frameData);
            }
        }


        /// <summary>
        /// Method to make a Capability Confirm PDU.
        /// </summary>
        /// <param name="capFlag">The valid rdpgfx_capset_version8 flag.</param>
        /// <param name="version">version of the capability</param>
        void MakeCapabilityConfirmPdu(CapsFlags capFlag, CapsVersions version)
        {
            RDPGFX_CAPS_CONFIRM capsConfirm = egfxServer.CreateCapabilityConfirmPdu(capFlag, version);

            if (currentTestType == RdpegfxNegativeTypes.Capability_Incorrect_Version)
                capsConfirm.capsSet.version += 1;  // Set to an invalid value 1 more than 0x00080004.
            else if (currentTestType == RdpegfxNegativeTypes.Capability_Incorrect_CapsDatalength)
                capsConfirm.capsSet.capsDataLength = 0x01;  // Set to an invalid value 0x01.
            else if (currentTestType == RdpegfxNegativeTypes.SurfaceToScreen_Incorrect_PduLengthInHeader)
                capsConfirm.Header.pduLength = 0x00;        // Set to an invalid value 0x00.
            else if (currentTestType == RdpegfxNegativeTypes.Capability_InvalidCapFlag)
                capsConfirm.capsSet.capsData = BitConverter.GetBytes((uint)(0x03));  // 0x03 is an invalid cap flag.

            AddPdusToBuffer(capsConfirm);
        }

        /// <summary>
        /// Method to make a Cache Offer Reply PDU.
        /// </summary>
        /// <param name="cacheEntries">The cache entries(slot) to be sent to client </param>
        void MakeCacheImportReplyPdu(RDPGFX_CACHE_ENTRY_METADATA[] cacheEntries)
        {
            RDPGFX_CACHE_IMPORT_REPLY impReply = egfxServer.CreateCacheImportReplyPdu();
            impReply.importedEntriesCount = (ushort)cacheEntries.Length;

            for (ushort i = 1; i <= cacheEntries.Length; i++)
            {
                impReply.cacheSlotsList.Add(i);
            }

            AddPdusToBuffer(impReply);
        }

        /// <summary>
        /// Method to make a Reset Graphics Pdu.
        /// </summary>
        /// <param name="w"> Width of virtual desktop </param>
        /// <param name="h"> Height of virtual desktop </param>
        /// <param name="monitorCount">Count of Monitors.</param>
        void MakeResetGraphicsPdu(uint w, uint h, uint monitorCount)
        {
            // Reset size of virtual desktop.
            RDPGFX_RESET_GRAPHICS resetGraphics = egfxServer.CreateResetGraphicsPdu(w, h, monitorCount);
            AddPdusToBuffer(resetGraphics);
        }

        /// <summary>
        /// Method to make a start frame Pdu.
        /// </summary>
        uint MakeStartFramePdu(uint? fid = null)
        {
            if (fid == null)
            {
                fid = egfxServer.GetFrameId();
            }
            RDPGFX_START_FRAME startFrame = egfxServer.CreateStartFramePdu(fid.Value);
            AddPdusToBuffer(startFrame);

            return fid.Value;
        }

        /// <summary>
        /// Method to make an end frame Pdu.
        /// </summary>
        /// <param name="fid">This is used to indicate frame id.</param>
        void MakeEndFramePdu(uint fid)
        {
            RDPGFX_END_FRAME endFrame = egfxServer.CreateEndFramePdu(fid);
            if (currentTestType == RdpegfxNegativeTypes.SurfaceManagement_FrameIdMismatch)
            {
                endFrame.frameId = endFrame.frameId + 1;   // Make the end frame id mismatch with its corrresponding start frame id.
            }

            AddPdusToBuffer(endFrame);
        }

        /// <summary>
        /// Method to make a Create Surface Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="w">This is used to indicate width of surface.</param>
        /// <param name="h">This is used to indicate height of surface.</param>
        /// <param name="pix">This is used to indicate pixel format of surface.</param>
        void MakeCreateSurfacePdu(ushort sid, ushort w, ushort h, PixelFormat pix)
        {
            RDPGFX_CREATE_SURFACE createSurf = egfxServer.CreateCreateSurfacePdu(sid, w, h, pix);
            if (currentTestType == RdpegfxNegativeTypes.SurfaceManagement_CreateDuplicatedSurface)
                createSurf.surfaceId--;  // Set the surface's id to an duplicated value used by an existed surface
            else if (currentTestType == RdpegfxNegativeTypes.SurfaceToScreen_IncorrectPduCmdId)
                createSurf.Header.cmdId = (PacketTypeValues)0x00;  // 0x00 is an invalid cmdId#region Message Fields

            AddPdusToBuffer(createSurf);
            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.CreateSurface(sid, w, h, pix);
            }
        }

        /// <summary>
        /// Method to make a map Surface to output Pdu
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="x">This is used to indicate x-coordinate of the upper-left corner of the surface.</param>
        /// <param name="y">This is used to indicate y-coordinate of the upper-left corner of the surface.</param>
        void MakeMapSurfaceToOutputPdu(ushort sid, uint x, uint y)
        {
            RDPGFX_MAPSURFACE_TO_OUTPUT surf2Output = egfxServer.CreateMapSurfaceToOutputPdu(sid, x, y);
            // Change pdu based testtype. 
            if (currentTestType == RdpegfxNegativeTypes.SurfaceManagement_MapInexistentSurfaceToOutput)
                surf2Output.surfaceId = 0xffff;  // Set the surface id to an inexsit surface's id 0xffff

            AddPdusToBuffer(surf2Output);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.MapSurfaceToOutput(sid, x, y);
            }
        }
        /// <summary>
        /// Method to make a map Surface to scaled output Pdu
        /// </summary>
        /// <param name="sid">surface ID</param>
        /// <param name="x">the top-left corner of the surface</param>
        /// <param name="y">map the upper-left corner of the surface</param>
        /// <param name="width">the width of the target surface</param>
        /// <param name="height">the height of the target surface</param>
        void MakeMapSurfaceToScaledOutputPdu(ushort sid, uint x, uint y, uint width, uint height)
        {
            RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU surf2ScaledOutput = egfxServer.CreateMapSurfaceToScaledOutputPdu(sid, x, y, width, height);
            
            // Change pdu based testtype. 
            if (currentTestType == RdpegfxNegativeTypes.SurfaceManagement_MapInexistentSurfaceToOutput)
                surf2ScaledOutput.surfaceId = 0xffff;  // Set the surface id to an inexsit surface's id 0xffff

            AddPdusToBuffer(surf2ScaledOutput);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.MapSurfaceToScaledOutput(sid, x, y, width, height);
            }
        }

        /// <summary>
        /// Method to make a solid fill Pdu
        /// </summary>
        /// <param name="sid">This is used to indicate surface id to be filled.</param>
        /// <param name="pixel">This is used to indicate color to fill.</param>
        /// <param name="rects">This is to specify rectangle areas in surface </param>
        void MakeSolidFillPdu(ushort sid, RDPGFX_COLOR32 color, RDPGFX_RECT16[] rects)
        {
            RDPGFX_SOLIDFILL solidFill = egfxServer.CreateSolidFillPdu(sid, color, rects);
            // Change pdu based testtype. 
            if (currentTestType == RdpegfxNegativeTypes.SurfaceManagement_SolidFill_ToInexistentSurface)
                solidFill.surfaceId = 0xffff;   // Set the surface id to an inexsit surface's id 0xffff

            AddPdusToBuffer(solidFill);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.SolidFill(sid, color, rects);
            }
        }

        /// <summary>
        /// Method to make a surface to cache Pdu
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="key">This is used to indicate a key to associate with the bitmap cache entry.</param>
        /// <param name="slot">This is used to indicate the index of the bitmap cache entry in which 
        /// the source bitmap data is stored.</param>
        /// <param name="rect">This is used to indicate rectangle that bounds the source bitmap.</param>
        void MakeSurfaceToCachePdu(ushort sid, ulong key, ushort slot, RDPGFX_RECT16 rect)
        {
            RDPGFX_SURFACE_TO_CACHE surfaceToCache = egfxServer.CreateSurfaceToCachePdu(sid, key, slot, rect);
            AddPdusToBuffer(surfaceToCache);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.SurfaceToCache(sid, key, slot, rect);
            }
        }

        /// <summary>
        /// Method to make a cache to surface Pdu
        /// </summary>
        /// <param name="slot">This is used to indicate a cache slot.</param>
        /// <param name="sid">This is used to indicate the destination surface id.</param>
        /// <param name="destPoints">This is used to specify destination points of source rectangle bitmap to be copied </param>
        void MakeCacheToSurfacePdu(ushort slot, ushort sid, RDPGFX_POINT16[] destPoints)
        {
            RDPGFX_CACHE_TO_SURFACE cacheToSurface = egfxServer.CreateCacheToSurfacePdu(slot, sid, destPoints);
            AddPdusToBuffer(cacheToSurface);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.CacheToSurface(slot, sid, destPoints);
            }
        }

        /// <summary>
        /// Method to make an evict cache Pdu.
        /// </summary>
        /// <param name="slot">This is used to indicate a cache slot.</param>
        void MakeEvictCacheEntryPdu(ushort slot)
        {
            RDPGFX_EVICT_CACHE_ENTRY evictCacheEntry = new RDPGFX_EVICT_CACHE_ENTRY(slot);
            AddPdusToBuffer(evictCacheEntry);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.EvictCacheEntry(slot);
            }
        }

        /// <summary>
        /// Method to make a surface to surface Pdu.
        /// </summary>
        /// <param name="srcSID">This is used to indicate source surface id.</param>
        /// <param name="destSID">This is used to indicate destination surface id.</param>
        /// <param name="srcRect">This is used to indicate source rectangle bitmap area to be copied.</param>
        /// <param name="destPoints">This is used to specify destination points of source rectangle bitmap to be copied </param>
        void MakeSurfaceToSurfacePdu(ushort srcSID, ushort destSID, RDPGFX_RECT16 srcRect, RDPGFX_POINT16[] destPoints)
        {
            RDPGFX_SURFACE_TO_SURFACE surfToSurf = egfxServer.CreateSurfaceToSurfacePdu(srcSID, destSID, srcRect, destPoints);
            // Change pdu based testtype
            if (currentTestType == RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_InexistentSrc)
            {
                surfToSurf.surfaceIdSrc = 0xffff; // 0xffff is an inexistent surface id.
            }
            else if (currentTestType == RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_InexistentDest)
            {
                surfToSurf.surfaceIdDest = 0xffff; // 0xffff is an inexistent surface id.                
            }
            else if (currentTestType == RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_DestPtsCount_Mismatch)
            {
                surfToSurf.destPtsCount += 1; // Make the value of destPts and the length of destPts mismatch.
            }

            AddPdusToBuffer(surfToSurf);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.SurfaceToSurface(srcSID, destSID, srcRect, destPoints);
            }
        }

        /// <summary>
        /// Method to make a delete surface Pdu
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        void MakeDeleteSurfacePdu(ushort sid)
        {
            RDPGFX_DELETE_SURFACE delSurface = egfxServer.CreateDeleteSurfacePdu(sid);
            AddPdusToBuffer(delSurface);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.DeleteSurface(sid);
            }
        }

        /// <summary>
        /// Method to make a wire to surface Pdu1 
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="cId">This is used to indicate the codecId.</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">This is used to indicate border of bitmap on target surface.</param>
        /// <param name="bmLen">This is used to indicate the length of bitmap data.</param>
        /// <param name="bmData">This is used to indicate the bitmap data encoded by cId codec.</param>
        void MakeWireToSurfacePdu1(ushort sId, CodecType cId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, byte[] bmData)
        {
            RDPGFX_WIRE_TO_SURFACE_PDU_1 wireToSurf1 = egfxServer.CreateWireToSurfacePdu1(sId, cId, pixFormat, bmRect, bmData);
            AddPdusToBuffer(wireToSurf1);
        }

        /// <summary>
        /// Method to make a wire to surface Pdu2
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmLen">This is used to indicate the length of bitmap data.</param>
        /// <param name="bmData">This is used to indicate the bitmap data encoded by cId codec.</param>
        void MakeWireToSurfacePdu2(ushort sId, PixelFormat pixFormat, byte[] bmData)
        {
            RDPGFX_WIRE_TO_SURFACE_PDU_2 wire2surf = egfxServer.CreateWireToSurfacePdu2(sId, pixFormat, bmData);
            AddPdusToBuffer(wire2surf);
        }

        /// <summary>
        /// Method to make a delete encoding context pdu
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="codecCtxId">This is used to indicate the codecContextId.</param>
        void MakeRfxProgressiveCodexContextDeletionPdu(ushort sId, uint codecCtxId)
        {
            RDPGFX_DELETE_ENCODING_CONTEXT deleteContexPdu = egfxServer.CreateRfxProgressiveCodexContextDeletionPdu(sId, codecCtxId);
            AddPdusToBuffer(deleteContexPdu);
        }

        #endregion private method

        #region IAdapter Members

        /// <summary>
        /// Method to do test suite initialization.
        /// </summary>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            rdpegfxPdusToSent = new List<RdpegfxPdu>();

            // Set no rdp8.0 compression temperarily.
            byte compFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8;
            segPdu = new EGFXRdpSegmentedPdu(compFlag);

            surfManager = new SurfaceManager();

            // For RFX codec compatible.
            rdprfxServer = new RdprfxServer();
            pendingRfxBuffer = new List<byte>();
            syncLocker = new object();

            #region WaitTime
            string strWaitTime = Site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                waitTime = new TimeSpan(0, 0, waitSeconds);
            }
            else
            {
                waitTime = new TimeSpan(0, 0, 20);
            }
            #endregion

        }

        /// <summary>
        /// Method to reset the test suite status to initialization.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            segPdu.compressFlag = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8;

            rdpegfxPdusToSent.Clear();
            currentTestType = RdpegfxNegativeTypes.None;

            segPdu.Reset();
            surfManager.Reset();

            ClearCodecBandEncoder.GetInstance().ResetVBarStorage();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Do Something.
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IRdpegfxAdapter Members

        /// <summary>
        /// Attach a RdpbcgrAdapter object
        /// </summary>
        /// <param name="rdpbcgrAdapter">RDPBCGR adapter</param>
        public void AttachRdpbcgrAdapter(IRdpbcgrAdapter rdpbcgrAdapter)
        {
            this.bcgrAdapter = rdpbcgrAdapter;
        }

        /// <summary>
        /// Initialize this protocol with create graphic DVC channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC server instance</param>
        /// <param name="transportType">Transport type</param>
        /// <returns>True if client supports this protocol; otherwise, return false.</returns>
        public bool ProtocolInitialize(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            if (!rdpedycServer.IsMultipleTransportCreated(transportType))
            {
                rdpedycServer.CreateMultipleTransport(transportType);
            }

            return CreateEGFXDvc(rdpedycServer, transportType);
        }

        /// <summary>
        /// Create graphic DVC channel.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC server instance</param>
        /// <param name="transportType">Transport type</param>
        /// <param name="channelId">Channel Id</param>
        /// <returns>True if success; otherwise, return false.</returns>
        public bool CreateEGFXDvc(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType, uint? channelId = null)
        {
            this.egfxServer = new RdpegfxServer(rdpedycServer);

            bool success = false;

            try
            {
                success = egfxServer.CreateRdpegfxDvc(waitTime, transportType, channelId);
            }
            catch (Exception e)
            {
                Site.Log.Add(LogEntryKind.Comment, "Exception occurred when creating RDPEGFX channel: {0}.", e.Message);
            }

            return success;
        }

        /// <summary>
        /// Set the type of current test.
        /// </summary>
        /// <param name="testType">The test type.</param>
        public void SetTestType(RdpegfxNegativeTypes testType)
        {
            currentTestType = testType;
            this.segPdu.SetTestType(testType);
        }

        /// <summary>
        /// Verify the common header of RDPEGFX messages.
        /// </summary>
        /// <param name="pdu"> The received RdpegfxPdu message. </param>
        /// <returns> True if validation pass, otherwise false. </returns>
        bool RDPEGFX_ValidateHeader(RdpegfxPdu pdu)
        {
            Site.Assert.AreEqual(0, pdu.Header.flags,
                    "Graphics command flags Must be set to Zero(Section 2.2.2.5). Received flags: {0}", pdu.Header.flags);

            return true;
        }

        /// <summary>
        /// Verify if received RDPGFX_CAPS_ADVERTISE message has correct data.
        /// </summary>
        /// <param name="adv"> The received RDPGFX_CAPS_ADVERTISE message.</param>
        /// <returns> True if validation pass, otherwise false. </returns>
        bool Validate(RDPGFX_CAPS_ADVERTISE adv)
        {
            if (!RDPEGFX_ValidateHeader(adv)) return false;

            // Check PDU length.
            Site.Assert.AreEqual(adv.Header.pduLength, adv.pduLen,
                "The actual length ({0}) of Capability Advertise PDU is expected to be same with pduLength({1}) in header!", adv.pduLen, adv.Header.pduLength);

            // Check capability version, capLen, and flags.
            for (ushort index = 0; index < adv.capsSetCount; index++)
            {
                uint capsFlag;
                bool validFlag;

                switch (adv.capsSets[index].version)
                {
                    case CapsVersions.RDPGFX_CAPVERSION_8:
                        Site.Assert.AreEqual((uint)4, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.1), Received capsDataLength: {1} in capset[{2}]!",
                            4, adv.capsSets[index].capsDataLength, index);

                        capsFlag = BitConverter.ToUInt32(adv.capsSets[index].capsData, 0);
                        validFlag = (capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_THINCLIENT
                                            );
                        Site.Assert.IsTrue(validFlag, "Unknown capability flags {0} (Section 2.2.3.1).", capsFlag);
                        break;
                    case CapsVersions.RDPGFX_CAPVERSION_81:
                        Site.Assert.AreEqual((uint)4, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.2), Received capsDataLength: {1} in capset[{2}]!",
                            4, adv.capsSets[index].capsDataLength, index);

                        capsFlag = BitConverter.ToUInt32(adv.capsSets[index].capsData, 0);
                        validFlag = (capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_THINCLIENT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC420_ENABLED) ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC420_ENABLED | CapsFlags.RDPGFX_CAPS_FLAG_THINCLIENT)
                                            );
                        Site.Assert.IsTrue(validFlag, "Unknown capability flags {0} (Section 2.2.3.2).", capsFlag);
                        break;
                    case CapsVersions.RDPGFX_CAPVERSION_10:
                        Site.Assert.AreEqual((uint)4, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.3), Received capsDataLength: {1} in capset[{2}]!",
                            4, adv.capsSets[index].capsDataLength, index);

                        capsFlag = BitConverter.ToUInt32(adv.capsSets[index].capsData, 0);
                        validFlag = (capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED)
                                            );
                        Site.Assert.IsTrue(validFlag, "Unknown capability flags {0} (Section 2.2.3.3).", capsFlag);
                        break;
                    case CapsVersions.RDPGFX_CAPVERSION_101:
                        Site.Assert.AreEqual((uint)16, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.4), Received capsDataLength: {1} in capset[{2}]!",
                            16, adv.capsSets[index].capsDataLength, index);

                        byte[] allZero = new byte[] {
                            0, 0, 0, 0,
                            0, 0, 0, 0,
                            0, 0, 0, 0,
                            0, 0, 0, 0
                        };
                        bool reservedIsAllZero = adv.capsSets[index].capsData.SequenceEqual(allZero);
                        Site.Assert.IsTrue(reservedIsAllZero, "The reserved field MUST be set to all zero (Section 2.2.3.4).");
                        break;
                    case CapsVersions.RDPGFX_CAPVERSION_102:
                        Site.Assert.AreEqual((uint)4, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.5), Received capsDataLength: {1} in capset[{2}]!",
                            4, adv.capsSets[index].capsDataLength, index);

                        capsFlag = BitConverter.ToUInt32(adv.capsSets[index].capsData, 0);
                        validFlag = (capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED)
                                            );
                        Site.Assert.IsTrue(validFlag, "Unknown capability flags {0} (Section 2.2.3.5).", capsFlag);
                        break;
                    case CapsVersions.RDPGFX_CAPVERSION_103:
                        Site.Assert.AreEqual((uint)4, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.6), Received capsDataLength: {1} in capset[{2}]!",
                            4, adv.capsSets[index].capsDataLength, index);

                        capsFlag = BitConverter.ToUInt32(adv.capsSets[index].capsData, 0);
                        validFlag = (capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_THINCLIENT
                                            );
                        Site.Assert.IsTrue(validFlag, "Unknown capability flags {0} (Section 2.2.3.6).", capsFlag);
                        break;
                    case CapsVersions.RDPGFX_CAPVERSION_104:
                        Site.Assert.AreEqual((uint)4, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.7), Received capsDataLength: {1} in capset[{2}]!",
                            4, adv.capsSets[index].capsDataLength, index);

                        capsFlag = BitConverter.ToUInt32(adv.capsSets[index].capsData, 0);
                        validFlag = (capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_THINCLIENT ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC_THINCLIENT) ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED)
                                            );
                        Site.Assert.IsTrue(validFlag, "Unknown capability flags {0} (Section 2.2.3.7).", capsFlag);
                        break;
                    case CapsVersions.RDPGFX_CAPVERSION_105:
                        Site.Assert.AreEqual((uint)4, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.8), Received capsDataLength: {1} in capset[{2}]!",
                            4, adv.capsSets[index].capsDataLength, index);

                        capsFlag = BitConverter.ToUInt32(adv.capsSets[index].capsData, 0);
                        validFlag = (capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_THINCLIENT ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC_THINCLIENT) ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED)
                                            );
                        Site.Assert.IsTrue(validFlag, "Unknown capability flags {0} (Section 2.2.3.8).", capsFlag);
                        break;
                    case CapsVersions.RDPGFX_CAPVERSION_106:
                        Site.Assert.AreEqual((uint)4, adv.capsSets[index].capsDataLength,
                            "Data Length of RDPEGFX capability set MUST be set to {0} (Section 2.2.3.9), Received capsDataLength: {1} in capset[{2}]!",
                            4, adv.capsSets[index].capsDataLength, index);

                        capsFlag = BitConverter.ToUInt32(adv.capsSets[index].capsData, 0);
                        validFlag = (capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED ||
                                            capsFlag == (uint)CapsFlags.RDPGFX_CAPS_FLAG_AVC_THINCLIENT ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC_THINCLIENT) ||
                                            capsFlag == (uint)(CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE | CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED)
                                            );
                        Site.Assert.IsTrue(validFlag, "Unknown capability flags {0} (Section 2.2.3.9).", capsFlag);
                        break;
                    default:
                        Site.Assert.Fail("The version of RDPEGFX capability set MUST be set to : {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7} or {8}. Received version: {9} in capset[{10}]",
                            CapsVersions.RDPGFX_CAPVERSION_8,
                            CapsVersions.RDPGFX_CAPVERSION_81, 
                            CapsVersions.RDPGFX_CAPVERSION_10, 
                            CapsVersions.RDPGFX_CAPVERSION_101,
                            CapsVersions.RDPGFX_CAPVERSION_102, 
                            CapsVersions.RDPGFX_CAPVERSION_103, 
                            CapsVersions.RDPGFX_CAPVERSION_104, 
                            CapsVersions.RDPGFX_CAPVERSION_105, 
                            CapsVersions.RDPGFX_CAPVERSION_106, 
                            adv.capsSets[index].version, 
                            index);
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Method to expect a Capability Advertise from client.
        /// </summary>
        /// <returns> Received capsAdv message if not NULL </returns>
        public RDPGFX_CAPS_ADVERTISE ExpectCapabilityAdvertise()
        {
            DateTime beginTime = DateTime.Now;
            DateTime endTime = beginTime + waitTime;
            RDPGFX_CAPS_ADVERTISE capsAdv = null;
            while (capsAdv == null && DateTime.Now < endTime)
            {
                // System.Threading.Thread.Sleep(100);
                capsAdv = egfxServer.ExpectRdpegfxPdu<RDPGFX_CAPS_ADVERTISE>(waitTime);
            }

            // If validation fails, return NULL.
            if ((capsAdv == null) || !Validate(capsAdv))
                return null;
            return capsAdv;
        }

        /// <summary>
        /// Method to expect a CACHE_IMPORT_OFFER_PDU from client.
        /// </summary>
        /// <returns> Received cache import offer message if not NULL </returns>
        public RDPGFX_CACHE_IMPORT_OFFER ExpectCacheImportOffer()
        {
            DateTime beginTime = DateTime.Now;
            DateTime endTime = beginTime + waitTime;
            RDPGFX_CACHE_IMPORT_OFFER cacheImportOffer = null;
            while (cacheImportOffer == null && DateTime.Now < endTime)
            {
                cacheImportOffer = egfxServer.ExpectRdpegfxPdu<RDPGFX_CACHE_IMPORT_OFFER>(waitTime);
            }

            // If validation fails, return NULL.
            if ((cacheImportOffer == null) || !Validate(cacheImportOffer))
                return null;
            return cacheImportOffer;
        }

        /// <summary>
        /// Method to send a Capability Confirm to client.
        /// </summary> 
        /// <param name="capFlag">The valid rdpgfx_capset_version8 flag.</param>
        /// <param name="version">version of the capability</param>
        public void SendCapabilityConfirm(CapsFlags capFlag, CapsVersions version = CapsVersions.RDPGFX_CAPVERSION_8)
        {
            MakeCapabilityConfirmPdu(capFlag, version);
            PackAndSendServerPdu();
        }

        /// <summary>
        /// Method to send a CACHE_IMPORT_REPLY pdu to client.
        /// </summary>
        /// <param name="cacheEntries">Identify a collection of bitmap cache present on the client.</param>
        public void SendCacheImportReply(RDPGFX_CACHE_ENTRY_METADATA[] cacheEntries)
        {
            MakeCacheImportReplyPdu(cacheEntries);
            PackAndSendServerPdu();
        }

        /// <summary>
        /// Method to instruct client to create a surface, and output it to client screen.
        /// </summary>
        /// <param name="w"> Width of virtual desktop </param>
        /// <param name="h"> Height of virtual desktop </param>
        /// <param name="monitorCount">Count of Monitors.</param>
        /// <returns> Frame Id </returns>
        public uint ResetGraphics(uint w, uint h, uint monitorCount = 1)
        {
            // Reset size of virtual desktop
            MakeResetGraphicsPdu(w, h, monitorCount);
            uint fid = MakeStartFramePdu();
            MakeEndFramePdu(fid);

            // Send out the packet
            PackAndSendServerPdu();

            return fid;
        }

        /// <summary>
        /// Method to instruct client to create a surface, and output it to client screen.
        /// </summary>
        /// <param name="rect"> surface rectangle </param>
        /// <param name="pixFormat"> pixel Format to fill surface </param>
        /// <param name="surfaceId">Specify a Surface ID</param>
        /// <returns> The created surface </returns>
        public Surface CreateAndOutputSurface(RDPGFX_RECT16 rect, PixelFormat pixFormat, ushort? surfaceId = null)
        {
            ushort w = (ushort)(rect.right - rect.left);
            ushort h = (ushort)(rect.bottom - rect.top);

            // Create a surface and output it to screen
            Surface surface = surfManager.CreateSurface(w, h, surfaceId);
            if (null == surface)
            {
                Site.Log.Add(LogEntryKind.Warning, "Attempt to create too much(>255) surfaces!");
                return null;
            }

            MakeCreateSurfacePdu(surface.Id, w, h, pixFormat);
            MakeMapSurfaceToOutputPdu(surface.Id, rect.left, rect.top);

            PackAndSendServerPdu();

            return surface;
        }

        /// <summary>
        /// Method to instruct client to create a surface
        /// </summary>
        /// <param name="width">Width of the surface</param>
        /// <param name="height">Height of the surface</param>
        /// <param name="pixFormat">pixel Format to fill surface</param>
        /// <param name="surfaceId">Specify a Surface ID</param>
        /// <returns>The created surface</returns>
        public Surface CreateSurface(ushort width, ushort height, PixelFormat pixFormat, ushort? surfaceId = null)
        {
            // Create a surface and output it to screen
            Surface surface = surfManager.CreateSurface(width, height, surfaceId);
            MakeCreateSurfacePdu(surface.Id, width, height, pixFormat);
            PackAndSendServerPdu();

            return surface;
        }


        /// <summary>
        /// Method to instruct client to map a surface to output
        /// </summary>
        /// <param name="surfaceId">Surface Id</param>
        /// <param name="outputOriginX">x-coordinate of the map point</param>
        /// <param name="outputOriginY">Y-coordinate of the map point</param>
        public uint MapSurfaceToOutput(ushort surfaceId, uint outputOriginX, uint outputOriginY)
        {
            MakeMapSurfaceToOutputPdu(surfaceId, outputOriginX, outputOriginY);
            uint fid = MakeStartFramePdu();
            MakeEndFramePdu(fid);
            PackAndSendServerPdu();
            return fid;
        }

        /// <summary>
        /// Method to instruct scale output
        /// </summary>
        /// <param name="surfaceId">surface ID</param>
        /// <param name="outputOriginX">x-coordinate of the scaled output point</param>
        /// <param name="outputOriginY">y-coordinate of the scaled output point</param>
        /// <param name="targetWidth">target width of the scaled output</param>
        /// <param name="targetHeight">target height of the scaled output</param>
        /// <returns></returns>
        public uint ScaledOutput(ushort surfaceId, uint outputOriginX, uint outputOriginY, uint targetWidth, uint targetHeight)
        {
            MakeMapSurfaceToScaledOutputPdu(surfaceId, outputOriginX, outputOriginY, targetWidth, targetHeight);
            uint fid = MakeStartFramePdu();
            MakeEndFramePdu(fid);
            PackAndSendServerPdu();
            return fid;
        }

        /// <summary>
        /// Method to solidfill a surface with color
        /// </summary>
        /// <param name="surf">The surface to be filled.</param>
        /// <param name="color">The color to fill the surface.</param>
        /// <param name="rects">The rectangles to be filled in the surface.</param>
        /// <param name="frameId">Specify the frame Id.</param>
        /// <returns> Frame Id </returns>
        public uint SolidFillSurface(Surface surf, RDPGFX_COLOR32 color, RDPGFX_RECT16[] rects, uint? frameId = null)
        {
            uint fid = MakeStartFramePdu(frameId);
            MakeSolidFillPdu(surf.Id, color, rects);
            MakeEndFramePdu(fid);

            PackAndSendServerPdu();
            return fid;
        }

        /// <summary>
        /// Method to send nested frames
        /// </summary>
        /// <param name="surf">The surface that the frames belong to</param>
        /// <param name="colors">The color to fill the surface</param>
        /// <param name="rects">The rectangles to be filled in the surface</param>
        public void SendNestedFrames(Surface surf, RDPGFX_COLOR32[] colors, RDPGFX_RECT16[] rects)
        {
            uint fid1 = MakeStartFramePdu();
            uint fid2 = MakeStartFramePdu();
            MakeSolidFillPdu(surf.Id, colors[0], new RDPGFX_RECT16[] { rects[0] });
            MakeSolidFillPdu(surf.Id, colors[1], new RDPGFX_RECT16[] { rects[1] });
            MakeEndFramePdu(fid2);
            MakeEndFramePdu(fid1);

            PackAndSendServerPdu();
            //ExpectFrameAck(fid2);
            //ExpectFrameAck(fid1);
        }

        /// <summary>
        /// Method to implement SurfaceToCache functionality
        /// </summary>
        /// <param name="surf">The surface to be filled.</param>
        /// <param name="cacheRect">The rectangle to be cached on the surface.</param>
        /// <param name="cacheKey">The cacheKey of rectangle bitmap data on client.</param>
        /// <param name="cacheSlot">Specify a cacheslot</param>
        /// <param name="fillColor">The color that rectangle to be filled.</param>
        public uint CacheSurface(Surface surf, RDPGFX_RECT16 cacheRect, ulong cacheKey, ushort? cacheSlot, RDPGFX_COLOR32? fillColor = null)
        {
            uint fid = MakeStartFramePdu();

            if (fillColor != null)
            {
                // Send solid fill request to client to fill cacheRect of surface with color.
                RDPGFX_RECT16[] rects = { cacheRect };
                MakeSolidFillPdu(surf.Id, fillColor.Value, rects);
            }

            if (cacheSlot == null)
            {
                cacheSlot = 1;
            }

            if (currentTestType == RdpegfxNegativeTypes.CacheManagement_Default_ExceedMaxCacheSlot)
            {
                // 25600 is the max cache slots number for default cache size.
                for (ushort index = 1; index <= 25601; ++index)
                {
                    cacheSlot = index;
                    MakeSurfaceToCachePdu(surf.Id, cacheKey, cacheSlot.Value, cacheRect);
                    if (index % 100 == 0)
                        PackAndSendServerPdu();
                }
            }
            else if (currentTestType == RdpegfxNegativeTypes.CacheManagement_SmallCache_ExceedMaxCacheSlot)
            {
                // 12800 is the max cache slots number for small cache size.
                for (ushort index = 1; index <= 12801; ++index)
                {
                    cacheSlot = index;
                    MakeSurfaceToCachePdu(surf.Id, cacheKey, cacheSlot.Value, cacheRect);
                    if (index % 100 == 0)
                        PackAndSendServerPdu();
                }
            }
            else if (currentTestType == RdpegfxNegativeTypes.CacheManagement_ThinCient_ExceedMaxCacheslot)
            {
                // 4096 is the max cache slots number for thin client cache size.
                for (ushort index = 1; index <= 4097; ++index)
                {
                    cacheSlot = index;
                    MakeSurfaceToCachePdu(surf.Id, cacheKey, cacheSlot.Value, cacheRect);
                    if (index % 100 == 0)
                        PackAndSendServerPdu();
                }
            }
            else if (currentTestType == RdpegfxNegativeTypes.CacheManagement_Default_ExceedMaxCacheSize)
            {
                // Every cache slot has the size of 1MB here,
                // 100MB is the max cache size for default flag, cache 101 MB here which exceeds max cache size.
                for (ushort index = 1; index <= 101; ++index)
                {
                    cacheSlot = index;
                    MakeSurfaceToCachePdu(surf.Id, cacheKey, cacheSlot.Value, cacheRect);
                }
            }
            else if (currentTestType == RdpegfxNegativeTypes.CacheManagement_SmallCache_ExceedMaxCacheSize)
            {
                // Every cache slot has the size of 1MB here,
                // 50MB is the max cache size for SmallCache flag, cache 51 MB here which exceeds max cache size.
                for (ushort index = 1; index <= 51; ++index)
                {
                    cacheSlot = index;
                    MakeSurfaceToCachePdu(surf.Id, cacheKey, cacheSlot.Value, cacheRect);
                }
            }
            else if (currentTestType == RdpegfxNegativeTypes.CacheManagement_ThinClient_ExceedMaxCacheSize)
            {
                // Every cache slot has the size of 1MB here,
                // 16MB is the max cache size for SmallCache flag, cache 17 MB here which exceeds max cache size.
                for (ushort index = 1; index <= 17; ++index)
                {
                    cacheSlot = index;
                    MakeSurfaceToCachePdu(surf.Id, cacheKey, cacheSlot.Value, cacheRect);
                }
            }
            else
            {
                MakeSurfaceToCachePdu(surf.Id, cacheKey, cacheSlot.Value, cacheRect);
            }

            if (currentTestType == RdpegfxNegativeTypes.CacheManagement_Delete_InexistentCacheSlot)
            {
                // Delete an inexistent cache slot.
                MakeEvictCacheEntryPdu(0xfefe);    // 0xfefe is an inexistent cache slot.
            }

            MakeEndFramePdu(fid);
            PackAndSendServerPdu();
            return fid;
        }

        /// <summary>
        /// Method to implement CacheToSurface functionality.
        /// </summary>
        /// <param name="surf">The surface to be filled.</param>
        /// <param name="cacheSlot">Cache slot of bitmap</param>
        /// <param name="destPoints">This is used to specify destination points of source rectangle bitmap to be copied</param>
        /// <returns>Frame Id</returns>
        public uint FillSurfaceByCachedBitmap(Surface surf, ushort cacheSlot, RDPGFX_POINT16[] destPoints)
        {
            uint fid = MakeStartFramePdu();
            MakeCacheToSurfacePdu(cacheSlot, surf.Id, destPoints);

            MakeEndFramePdu(fid);

            PackAndSendServerPdu();
            return fid;
        }

        /// <summary>
        /// Method to implement SurfaceToCache and CacheToSurface functionality.
        /// </summary>
        /// <param name="surf">The surface to be filled.</param>
        /// <param name="cacheRect">The rectangle to be cached on the surface.</param>
        /// <param name="cacheKey">The cacheKey of rectangle bitmap data on client.</param>
        /// <param name="destPoints">This is used to specify destination points of source rectangle bitmap to be copied </param>
        /// <param name="cacheSlot">Specify a cacheslot</param>
        /// <param name="fillColor">The color that rectangle to be filled.</param>
        /// <returns> Frame Id </returns>
        public uint FillSurfaceByCachedBitmap(Surface surf, RDPGFX_RECT16 cacheRect, ulong cacheKey, RDPGFX_POINT16[] destPoints, ushort? cacheSlot, RDPGFX_COLOR32? fillColor = null)
        {
            uint fid = MakeStartFramePdu();

            if (fillColor != null)
            {
                // Send solid fill request to client to fill cacheRect of surface with color
                RDPGFX_RECT16[] rects = { cacheRect };
                MakeSolidFillPdu(surf.Id, fillColor.Value, rects);
            }

            // Copy a rectangle bitmap data to cache and move it from cache to other positions of surface.
            if (cacheSlot == null)
            {
                cacheSlot = 1;  //set slot# to 1 
            }
            MakeSurfaceToCachePdu(surf.Id, cacheKey, cacheSlot.Value, cacheRect);

            if (currentTestType == RdpegfxNegativeTypes.CacheManagement_CacheToSurface_InexistentCacheSlot)
            {
                cacheSlot = (ushort)(cacheSlot.Value + 1);
            }
            ushort sid = surf.Id;
            if (currentTestType == RdpegfxNegativeTypes.CacheManagement_CacheToSurface_InexistentSurface)
            {
                sid++;
            }
            MakeCacheToSurfacePdu(cacheSlot.Value, sid, destPoints);

            MakeEndFramePdu(fid);

            PackAndSendServerPdu();
            return fid;
        }

        /// <summary>
        /// Method to implement evictCacheEntry functionality
        /// </summary>
        /// <param name="cacheSlot">Cache slot</param>
        /// <returns>Frame Id</returns>
        public uint EvictCachEntry(ushort cacheSlot)
        {
            uint fid = MakeStartFramePdu();

            MakeEvictCacheEntryPdu(cacheSlot);
            MakeEndFramePdu(fid);

            PackAndSendServerPdu();
            return fid;
        }

        /// <summary>
        /// Method to copy bitmap of a rectangle in surface to other position
        /// </summary>
        /// <param name="surf">The source surface where the rectangle to be copied.</param>
        /// <param name="srcRect">The rectangle to be copied.</param>
        /// <param name="destPos">The position array that rectangle is copied to.</param>
        /// <returns> Frame Id </returns>
        public uint IntraSurfaceCopy(Surface surf, RDPGFX_RECT16 srcRect, RDPGFX_POINT16[] destPos)
        {
            uint fid = MakeStartFramePdu();
            MakeSurfaceToSurfacePdu(surf.Id, surf.Id, srcRect, destPos);
            MakeEndFramePdu(fid);

            PackAndSendServerPdu();

            return fid;
        }

        /// <summary>
        /// Method to copy bitmap of a rectangle in a surface to other position in another surface
        /// </summary>
        /// <param name="surf">The source surface where the rectangle to be copied.</param>
        /// <param name="srcRect">The rectangle to be copied.</param>
        /// <param name="fillColor">The color of rectangle to be filled.</param>
        /// <param name="surfDest">The destination surface where the rectangle is copied to.</param>
        /// <param name="destPos">The position array that rectangle is copied to.</param>
        /// <returns> Frame Id </returns>
        public uint InterSurfaceCopy(Surface surfSrc, RDPGFX_RECT16 srcRect, RDPGFX_COLOR32 fillColor, Surface surfDest, RDPGFX_POINT16[] destPos)
        {
            uint fid = MakeStartFramePdu();
            RDPGFX_RECT16[] rects = { srcRect };
            MakeSolidFillPdu(surfSrc.Id, fillColor, rects);
            MakeSurfaceToSurfacePdu(surfSrc.Id, surfDest.Id, srcRect, destPos);
            if (currentTestType != RdpegfxNegativeTypes.SurfaceManagement_InterSurfaceCopy_DestPtsCount_Mismatch)
            {
                MakeEndFramePdu(fid);
            }

            PackAndSendServerPdu();

            return fid;
        }

        /// <summary>
        /// Method to Delete surface.
        /// </summary>
        /// <param name="sid">The ID of surface to be deleted.</param>
        public void DeleteSurface(ushort sid)
        {
            MakeDeleteSurfacePdu(sid);
            PackAndSendServerPdu();
            surfManager.DeleteSurface(sid);
        }

        /// <summary>
        /// Verify if received RDPGFX_FRAME_ACK message has correct data.
        /// </summary>
        /// <param name="ack"> The received RDPGFX_FRAME_ACK message</param>
        /// <returns> True if validation pass, otherwise false </returns>
        public bool Validate(RDPGFX_FRAME_ACK ack, uint expectedFrameId)
        {

            if (!RDPEGFX_ValidateHeader(ack)) return false;

            // Check PDU length
            Site.Assert.AreEqual(ack.Header.pduLength, ack.pduLen,
                "The actual length ({0}) of FrameAck PDU is different from pduLength({1}) in header!", ack.pduLen, ack.Header.pduLength);

            if (ack.queueDepth == RDPGFX_FRAME_ACK.SUSPEND_FRAME_ACKNOWLEDGE)
            {
                this.frAckSuspend = true;
            }
            else
            {
                this.frAckSuspend = false;
                /* temply diasabled, the debug result shows queueDepth increased based on queueDepth in last RDP connection,
                 * so we receive queueDepth as 555, 571, ... in different test round.
                // Check the received frame id
                bool validFrameId = (ack.frameId + ack.queueDepth <= expectedFrameId);
                Site.Assert.IsTrue(validFrameId,
                        "The received frameId: {0},  ack.queueDepth: {1}, expected frameId: {2} ",
                        ack.frameId, ack.queueDepth, expectedFrameId);
                */
            }

            return true;
        }

        /// <summary>
        /// Method to expect a Frame Acknowledge from client.
        /// </summary>
        public void ExpectFrameAck(uint fid)
        {
            TimeSpan timeout = waitTime;
            DateTime endTime = DateTime.Now + timeout;
            if (frAckSuspend)
            {
                // Frame ack suspend, check new frame ack in 200 ms to see if suspend is disabled.  
                // This can make sure process of frame ack packet even if suspend is enabled
                timeout = new TimeSpan(0, 0, 0, 0, 200);
                endTime = DateTime.Now;
            }

            RDPGFX_FRAME_ACK frameAck = null;
            do
            {
                frameAck = egfxServer.ExpectRdpegfxPdu<RDPGFX_FRAME_ACK>(timeout);

                if (frameAck != null)
                {
                    // Expected message is received, validate it
                    Validate(frameAck, fid);

                    // If frameack is not suspend mode, drop frame if frame id is less than expected fid.
                    if (!frAckSuspend && frameAck.frameId < fid)
                    {
                        frameAck = null;
                    }
                }
            } while (frameAck == null && (DateTime.Now < endTime));

            if (!frAckSuspend)
            {
                Site.Assert.IsNotNull(frameAck, "RDPGFX_FRAME_ACKNOWLEDGE (frameId: {0}) is received.", fid);
            }
        }

        /// <summary>
        /// Verify if received RDPGFX_CACHE_IMPORT_OFFER message has correct data.
        /// </summary>
        /// <param name="impOffer"> The received RDPGFX_CACHE_IMPORT_OFFER message</param>
        /// <returns> True if validation pass, otherwise false </returns>
        bool Validate(RDPGFX_CACHE_IMPORT_OFFER impOffer)
        {
            if (!RDPEGFX_ValidateHeader(impOffer)) return false;

            // Check PDU length
            Site.Assert.AreEqual(impOffer.Header.pduLength, impOffer.pduLen,
                "The actual length ({0}) of Capability Advertise PDU is is expected to be same with pduLength({1}) in header!", impOffer.pduLen, impOffer.Header.pduLength);

            return true;
        }

        /// <summary>
        /// Add a Rdprfx message data into byte list.
        /// </summary>
        private void AddToPendingList(IMarshalable rfxMessage)
        {
            lock (syncLocker)
            {
                byte[] data = rfxMessage.ToBytes();
                pendingRfxBuffer.AddRange(data);
            }
        }

        /// <summary>
        /// Pack a Rdprfx message for a tile image.
        /// </summary>
        /// <param name="index"> Frame index in frame begin block.</param>
        /// <param name="opMode"> Indicate Operational Mode.</param>
        /// <param name="entropy"> Indicate Entropy Algorithm.</param>
        /// <param name="tileImage"> The image to be encoded as RFX codec.</param>
        /// <return> A byte array of image encoded as RFX codec.</return>
        private byte[] PackRfxTileImage(uint index, OperationalMode opMode, EntropyAlgorithm entropy, System.Drawing.Image tileImage)
        {
            lock (syncLocker)
            {
                pendingRfxBuffer.Clear();
            }

            if (index == 0 || opMode == OperationalMode.ImageMode)
            {
                TS_RFX_SYNC rfxSync = rdprfxServer.CreateTsRfxSync();
                AddToPendingList(rfxSync);

                TS_RFX_CODEC_VERSIONS rfxVersions = rdprfxServer.CreateTsRfxCodecVersions();
                AddToPendingList(rfxVersions);

                TS_RFX_CHANNELS rfxChannels = rdprfxServer.CreateTsRfxChannels();
                AddToPendingList(rfxChannels);

                TS_RFX_CONTEXT rfxContext = rdprfxServer.CreateTsRfxContext(opMode, entropy);
                AddToPendingList(rfxContext);
            }

            TS_RFX_FRAME_BEGIN rfxBegin = rdprfxServer.CreateTsRfxFrameBegin(index);
            AddToPendingList(rfxBegin);

            TS_RFX_REGION rfxRegion = rdprfxServer.CreateTsRfxRegion();
            AddToPendingList(rfxRegion);

            TS_RFX_TILESET rfxTileSet = rdprfxServer.CreateTsRfxTileSet(opMode, entropy, tileImage);
            AddToPendingList(rfxTileSet);

            TS_RFX_FRAME_END rfxEnd = rdprfxServer.CreateTsRfxFrameEnd();
            AddToPendingList(rfxEnd);

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.SetRemoteFXRegion(rfxRegion);
                this.bcgrAdapter.SimulatedScreen.SetRemoteFXTileSet(rfxTileSet, entropy);
            }

            lock (syncLocker)
            {
                return pendingRfxBuffer.ToArray();
            }

        }


        /// <summary>
        /// Method to pack rdpegfx frame into segment header and send it
        /// </summary>
        /// <param name="frameData"> The frame data to be sent </param>
        public void SendRdpegfxFrameInSegment(byte[] frameData)
        {
            this.segPdu.SegmentAndCompressFrame(frameData);
            List<byte[]> segPdus = segPdu.EncodeSegPdu();

            foreach (byte[] data in segPdus)
                this.egfxServer.Send(data);

            segPdu.ClearSegments();
        }

        /// <summary>
        /// Encode a bitmap data by RemoteFX codec.
        /// </summary>
        /// <param name="image"> The bitmap image to be sent </param>
        /// <param name="opMode"> Indicate Operational Mode.</param>
        /// <param name="entropy"> Indicate Entropy Algorithm.</param>
        /// <param name="imgPos"> The top-left position of bitmap image relative to surface</param>
        /// <param name="sId"> The surface Id that bitmap image is sent to </param>
        /// <returns> A dictionary with frameId and byte stream frame pair </returns>
        public Dictionary<uint, byte[]> RemoteFXCodecEncode(System.Drawing.Image image, OperationalMode opMode, EntropyAlgorithm entropy,
                                            RDPGFX_POINT16 imgPos, ushort sId, PixelFormat pixFormat)
        {
            if (image == null) return null;

            Dictionary<uint, byte[]> frDict = new Dictionary<uint, byte[]>();   // Save encoded frames for one tile

            TileImage[] tileImageArr = RdprfxTileUtils.SplitToTileImage(image, RdprfxServer.TileSize, RdprfxServer.TileSize);

            for (uint index = 0; index < tileImageArr.Length; index++)
            {
                byte[] tileBitmap = this.PackRfxTileImage(index, opMode, entropy, tileImageArr[index].image);
                ushort tileLeft = (ushort)(imgPos.x + tileImageArr[index].x);
                ushort tileTop = (ushort)(imgPos.y + tileImageArr[index].y);
                RDPGFX_RECT16 tileRect = new RDPGFX_RECT16(tileLeft, tileTop,
                                                            (ushort)(tileLeft + tileImageArr[index].image.Width),
                                                            (ushort)(tileTop + tileImageArr[index].image.Height));

                uint fid = MakeStartFramePdu();
                MakeWireToSurfacePdu1(sId, CodecType.RDPGFX_CODECID_CAVIDEO, pixFormat, tileRect, tileBitmap);
                MakeEndFramePdu(fid);

                if (this.bcgrAdapter.SimulatedScreen != null)
                {
                    this.bcgrAdapter.SimulatedScreen.RenderRemoteFXTile(sId, tileRect);
                }

                frDict.Add(fid, EncodePdusToSent());
            }

            return frDict;
        }

        /// <summary>
        /// Send RFX Progressive codec Pdu without image data to client.
        /// </summary>
        /// <param name="sId"> The surface Id that bitmap image is sent to </param>
        /// <param name="pixFormat">The pixel format to draw surface.</param>
        /// <param name="hasSync">Indicates if sync block exists in FRX Progressive bitmap stream.</param>
        /// <param name="hasContext">Indicates if context block exists in FRX Progressive bitmap stream.</param>
        /// <param name="bSubDiff">Indicates if sub-diffing with last frame of this surface</param>
        /// <returns> Frame Id </returns>
        public uint SendRfxProgressiveCodecPduWithoutImage(ushort sId, PixelFormat pixFormat, bool hasSync, bool hasContext, bool bSubDiff)
        {
            uint fid = MakeStartFramePdu();
            RdpegfxRfxProgCodecBlockManagerDecorator blockMngr = new RdpegfxRfxProgCodecBlockManagerDecorator(currentTestType);

            // No image in w2s_2 pdu, encoding rfx progressive data block without region and tile blocks
            byte[] blockdata = blockMngr.PackRfxProgCodecDataBlock(hasSync, hasContext, bSubDiff);
            MakeWireToSurfacePdu2(sId, pixFormat, blockdata);
            MakeEndFramePdu(fid);
            PackAndSendServerPdu();
            return fid;
        }

        private List<Dictionary<TileIndex, EncodedTile>> ConvertTileDictToLayer(Dictionary<TileIndex, EncodedTile[]> tileDict)
        {
            List<Dictionary<TileIndex, EncodedTile>> layerTileList = new List<Dictionary<TileIndex, EncodedTile>>();

            byte tileLayerNum = 0;

            // Get layer number
            foreach (KeyValuePair<TileIndex, EncodedTile[]> tile in tileDict)
            {
                tileLayerNum = (byte)tile.Value.Length;
                break;
            }

            // Convert into layer tile 
            for (int i = 0; i < tileLayerNum; i++)
            {
                Dictionary<TileIndex, EncodedTile> layerTileDict = new Dictionary<TileIndex, EncodedTile>();

                foreach (KeyValuePair<TileIndex, EncodedTile[]> tile in tileDict)
                {
                    layerTileDict.Add(tile.Key, tile.Value[i]);
                }
                layerTileList.Add(layerTileDict);
            }

            return layerTileList;
        }

        /// <summary>
        /// Encode bitmap data by RFX Progressive codec (one tile in one rfx_progressive_datablock frame).
        /// </summary>
        /// <param name="image"> The bitmap image to be sent </param>
        /// <param name="surf"> The surface that bitmap image is sent to </param>
        /// <param name="pixFormat">The pixel format to draw surface.</param>
        /// <param name="hasSync">Indicates if sync block exists in FRX Progressive bitmap stream.</param>
        /// <param name="hasContext">Indicates if context block exists in FRX Progressive bitmap stream.</param>
        /// <param name="quality">The target encoded quality.</param>
        /// <param name="bProg">Indicates if encode progressively</param>
        /// <param name="bSubDiff">Indicates if sub-diffing with last frame of this surface</param>
        /// <param name="bReduceExtrapolate">Indicates if use Reduce Extrapolate method in DWT step.</param>
        /// <returns> A list of layer byte stream, each layer is built by a dictionary with frameId and byte stream frame pair </returns>
        public List<Dictionary<uint, byte[]>> RfxProgressiveCodecEncode(Surface surf, Image image, PixelFormat pixFormat, bool hasSync, bool hasContext,
                                                    ImageQuality_Values quality, bool bProg, bool bSubDiff, bool bReduceExtrapolate)
        {
            bool multipleTileInRegion = true;
            List<Dictionary<TileIndex, EncodedTile>> layerTileList = new List<Dictionary<TileIndex, EncodedTile>>();

            if (image == null) return null;

            uint fid = 0;
            List<Dictionary<uint, byte[]>> layerDataList = new List<Dictionary<uint, byte[]>>();  // To save different layer data encoded by RFX Prog Codec.            
            RdpegfxRfxProgCodecBlockManagerDecorator blockMngr = new RdpegfxRfxProgCodecBlockManagerDecorator(currentTestType);

            surf.UpdateFromBitmap((System.Drawing.Bitmap)image);
            Dictionary<TileIndex, EncodedTile[]> tileDict = surf.ProgressiveEncode(quality, bProg, bSubDiff, bReduceExtrapolate, false);
            if (multipleTileInRegion)
            {
                layerTileList = ConvertTileDictToLayer(tileDict);
            }

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.RenderProgressiveCodec(surf.Id, tileDict, (ushort)image.Width, (ushort)image.Height);
            }

            if (bProg)  // Progressive codec is enabled
            {

                for (int i = 0; i < layerTileList.Count; i++)
                {
                    Dictionary<uint, byte[]> tileFrameDict = new Dictionary<uint, byte[]>();

                    fid = MakeStartFramePdu();
                    RFXProgCodecBlockType blockType = i == 0 ? RFXProgCodecBlockType.WBT_TILE_PROGRESSIVE_FIRST : RFXProgCodecBlockType.WBT_TILE_PROGRESSIVE_UPGRADE;
                    byte[] tileUpgradeData = blockMngr.PackRfxProgCodecDataBlock(hasSync, hasContext, bSubDiff, bReduceExtrapolate, layerTileList[i], blockType);
                    MakeWireToSurfacePdu2(surf.Id, pixFormat, tileUpgradeData);

                    MakeEndFramePdu(fid);

                    // Save frame into encoded tile first frame Dictionary                    
                    tileFrameDict.Add(fid, EncodePdusToSent());

                    // Add tile first frames into layer data list
                    layerDataList.Add(tileFrameDict);
                }
            }
            else  // Non-progressive encoding(i.e. tile_simple)
            {

                Dictionary<uint, byte[]> tileSimpleFrameDict = new Dictionary<uint, byte[]>();
                fid = MakeStartFramePdu();

                foreach (Dictionary<TileIndex, EncodedTile> layerTileDict in layerTileList)
                {
                    byte[] tileUpgradeData = blockMngr.PackRfxProgCodecDataBlock(hasSync, hasContext, bSubDiff, bReduceExtrapolate, layerTileDict, RFXProgCodecBlockType.WBT_TILE_SIMPLE);
                    MakeWireToSurfacePdu2(surf.Id, pixFormat, tileUpgradeData);
                }

                MakeEndFramePdu(fid);

                // Save frame into encoded tile simple frame Dictionary                    
                tileSimpleFrameDict.Add(fid, EncodePdusToSent());

                // Add tile first frames into layer data list
                layerDataList.Add(tileSimpleFrameDict);
            }
            return layerDataList;
        }

        /// <summary>
        /// Get a used RfxProgssiveCodec Context Id.
        /// </summary>
        /// <param name="contextId">A used context Id if exists.</param>
        /// <returns> True if codec context Id exists. </returns>
        public bool GetUsedRfxProgssiveCodecContextId(ref uint contextId)
        {
            return egfxServer.GetUsedCodecContextId(ref contextId);
        }

        /// <summary>
        /// Send a RDPEGFX_DELETE_ENCODING_CONTEXT_PDU to client.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="contextId">the context Id to be deleted.</param>
        /// <returns> Frame Id. </returns>
        public uint DeleteRfxProgssiveCodecContextId(ushort sId, uint contextId)
        {
            uint fid = MakeStartFramePdu();
            MakeRfxProgressiveCodexContextDeletionPdu(sId, contextId);
            MakeEndFramePdu(fid);
            PackAndSendServerPdu();
            return fid;
        }

        /// <summary>
        /// Send bitmap data in clearCodec encoding method.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="ccFlag">This is used to indicate the clearcodec stream flags.</param>
        /// <param name="graphIdx">This is used to indicate the index of graph to be put in client graph cache.</param>
        /// <param name="bmRect">The rectangle of whole Image, which will be sent in clearcodec, relative to the surface </param>
        /// <param name="residualImage"> The residual layer image to be sent </param>
        /// <param name="bands"> The dictionary of band layer image and position </param>
        /// <param name="subcodecs"> The dictionary of subcodec layer image, subcodecID and position </param>
        /// <returns> Frame Id </returns>
        public uint SendImageWithClearCodec(ushort sId, PixelFormat pixFormat, byte ccFlag, ushort graphIdx, RDPGFX_RECT16 bmRect, Image residualBmp,
                            Dictionary<RDPGFX_POINT16, Bitmap> bands, Dictionary<RDPGFX_POINT16, BMP_INFO> subcodecs)
        {
            uint fid = MakeStartFramePdu();

            ClearCodec_BitmapStream ccStream = new ClearCodec_BitmapStream(ccFlag, graphIdx);
            if ((ushort)currentTestType >= 500 && (ushort)currentTestType <= 599)
            {
                // Clearcodec negative test
                ccStream.SetTestType(currentTestType);
            }

            ccStream.LoadResidualBitmap((Bitmap)residualBmp);
            if (bands != null)
            {
                foreach (KeyValuePair<RDPGFX_POINT16, Bitmap> band in bands)
                {
                    ccStream.LoadBandBitmap(band.Value, band.Key);
                }
            }
            if (subcodecs != null)
            {
                foreach (KeyValuePair<RDPGFX_POINT16, BMP_INFO> scBmp in subcodecs)
                {
                    ccStream.LoadSubcodecBitmap(scBmp.Value.bmp, scBmp.Key, scBmp.Value.scID);
                }
            }
            ccStream.seqNumber = egfxServer.Get_ClearCodecBitmapStream_SeqNum();
            MakeWireToSurfacePdu1(sId, CodecType.RDPGFX_CODECID_CLEARCODEC, pixFormat, bmRect, ccStream.Encode());
            MakeEndFramePdu(fid);

            PackAndSendServerPdu();

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.RenderClearCodecImage(sId, pixFormat, ccFlag, graphIdx, bmRect, residualBmp, bands, subcodecs);
            }

            return fid;
        }

        /// <summary>
        /// Send bitmap data in H264 AVC420 codec
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">The rectangle of whole Image</param>
        /// <param name="numRects">Number of rects</param>
        /// <param name="regionRects">Rect list</param>
        /// <param name="quantQualityVals">Quality list</param>
        /// <param name="avc420EncodedBitstream">encoded H264 AVC420 data stream</param>
        /// <param name="baseImage">Base Image used to verify output</param>
        /// <returns></returns>
        public uint SendImageWithH264AVC420Codec(ushort sId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, uint numRects, RDPGFX_RECT16[] regionRects, RDPGFX_AVC420_QUANT_QUALITY[] quantQualityVals,
                          byte[] avc420EncodedBitstream, Image baseImage)
        {
            uint fid = MakeStartFramePdu();

            RFX_AVC420_METABLOCK avc420MetaData = new RFX_AVC420_METABLOCK();
            avc420MetaData.numRegionRects = numRects;
            avc420MetaData.regionRects = regionRects;
            avc420MetaData.quantQualityVals = quantQualityVals;

            RFX_AVC420_BITMAP_STREAM avc420Stream = new RFX_AVC420_BITMAP_STREAM(avc420MetaData, avc420EncodedBitstream);
            MakeWireToSurfacePdu1(sId, CodecType.RDPGFX_CODECID_AVC420, pixFormat, bmRect, avc420Stream.Encode());
            MakeEndFramePdu(fid);

            PackAndSendServerPdu();

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                if (baseImage == null)
                {
                    Site.Assume.Inconclusive("Cannot verify the output since base image is not found, check the existance and format of BaseImage element in test data file.");
                }
                this.bcgrAdapter.SimulatedScreen.RenderUncompressedImage(sId, baseImage, bmRect.left, bmRect.top);
            }

            return fid;
        }

        /// <summary>
        /// Send bitmap data in H264 AVC420 codec
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">The rectangle of whole Image</param>
        /// <param name="avc420BitmapStream">A RFX_AVC420_BITMAP_STREAM structure for encoded information</param>
        /// <param name="baseImage">Base Image used to verify output</param>
        /// <returns></returns>
        public uint SendImageWithH264AVC420Codec(ushort sId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, RFX_AVC420_BITMAP_STREAM avc420BitmapStream, Image baseImage)
        {
            uint fid = MakeStartFramePdu();
            MakeWireToSurfacePdu1(sId, CodecType.RDPGFX_CODECID_AVC420, pixFormat, bmRect, avc420BitmapStream.Encode());
            MakeEndFramePdu(fid);
            PackAndSendServerPdu();

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                if (baseImage == null)
                {
                    Site.Assume.Inconclusive("Cannot verify the output since base image is not found, check the existance and format of BaseImage element in test data file.");
                }
                this.bcgrAdapter.SimulatedScreen.RenderUncompressedImage(sId, baseImage, bmRect.left, bmRect.top);
            }

            return fid;
        }
        /// <summary>
        /// Send bitmap data in H264 AVC444 codec
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">The rectangle of whole Image</param>
        /// <param name="lcValue">Code specifies how data is encoded in the avc420EncodedBitstream1 and avc420EncodedBitstream2 fields</param>
        /// <param name="stream1NumRects">Number of rects of avc420EncodedBitstream1</param>
        /// <param name="steam1RegionRects">Rect list of avc420EncodedBitstream1</param>
        /// <param name="stream1QuantQualityVals">Quality list of avc420EncodedBitstream1</param>
        /// <param name="avc420EncodedBitstream1">encoded H264 AVC420 data stream of avc420EncodedBitstream1</param>
        /// <param name="stream2NumRects">Number of rects of avc420EncodedBitstream2</param>
        /// <param name="steam2RegionRects">Rect list of avc420EncodedBitstream2</param>
        /// <param name="stream2QuantQualityVals">Quality list of avc420EncodedBitstream2</param>
        /// <param name="avc420EncodedBitstream2">encoded H264 AVC420 data stream of avc420EncodedBitstream2</param>
        /// <param name="baseImage">Base Image used to verify output</param>
        /// <returns></returns>
        public uint SendImageWithH264AVC444Codec(ushort sId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, AVC444LCValue lcValue,
            uint stream1NumRects, RDPGFX_RECT16[] steam1RegionRects, RDPGFX_AVC420_QUANT_QUALITY[] stream1QuantQualityVals, byte[] avc420EncodedBitstream1,
            uint stream2NumRects, RDPGFX_RECT16[] steam2RegionRects, RDPGFX_AVC420_QUANT_QUALITY[] stream2QuantQualityVals, byte[] avc420EncodedBitstream2,
            Image baseImage)
        {
            uint fid = MakeStartFramePdu();
            RFX_AVC420_METABLOCK avc420MetaData = new RFX_AVC420_METABLOCK();
            avc420MetaData.numRegionRects = stream1NumRects;
            avc420MetaData.regionRects = steam1RegionRects;
            avc420MetaData.quantQualityVals = stream1QuantQualityVals;
            RFX_AVC420_BITMAP_STREAM avc420Stream1 = new RFX_AVC420_BITMAP_STREAM(avc420MetaData, avc420EncodedBitstream1);

            RFX_AVC420_BITMAP_STREAM avc420Stream2 = null;
            if (stream2NumRects != 0 && avc420EncodedBitstream2 != null)
            {
                avc420MetaData = new RFX_AVC420_METABLOCK();
                avc420MetaData.numRegionRects = stream2NumRects;
                avc420MetaData.regionRects = steam2RegionRects;
                avc420MetaData.quantQualityVals = stream2QuantQualityVals;
                avc420Stream2 = new RFX_AVC420_BITMAP_STREAM(avc420MetaData, avc420EncodedBitstream2);
            }

            RFX_AVC444_BITMAP_STREAM avc444Stream = new RFX_AVC444_BITMAP_STREAM(lcValue, avc420Stream1, avc420Stream2);
            MakeWireToSurfacePdu1(sId, CodecType.RDPGFX_CODECID_AVC444, pixFormat, bmRect, avc444Stream.Encode());
            MakeEndFramePdu(fid);

            PackAndSendServerPdu();

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                if (baseImage == null)
                {
                    Site.Assume.Inconclusive("Cannot verify the output since base image is not found, check the existance and format of BaseImage element in test data file.");
                }
                this.bcgrAdapter.SimulatedScreen.RenderUncompressedImage(sId, baseImage, bmRect.left, bmRect.top);
            }

            return fid;
        }

        /// <summary>
        /// Send bitmap data in H264 AVC444 codec
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">The rectangle of whole Image</param>
        /// <param name="codec">Codec type.</param>
        /// <param name="avc444BitmapStream">An IRFX_AVC444_BITMAP_STREAM interface for encoded information</param>
        /// <param name="baseImage">Base Image used to verify output</param>
        /// <returns></returns>
        public uint SendImageWithH264AVC444Codec(ushort sId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, CodecType codec, IRFX_AVC444_BITMAP_STREAM avc444BitmapStream,
            Image baseImage)
        {
            bool checkType = (codec == CodecType.RDPGFX_CODECID_AVC444 && avc444BitmapStream is RFX_AVC444_BITMAP_STREAM)
                || (codec == CodecType.RDPGFX_CODECID_AVC444v2 && avc444BitmapStream is RFX_AVC444V2_BITMAP_STREAM);

            if (!checkType)
            {
                Site.Assume.Fail("The codec type and bitmap stream type is inconsistent with codec:{0} and avc444BitmapStream:{1}.", codec, avc444BitmapStream.GetType().Name);
            }

            uint fid = MakeStartFramePdu();
            MakeWireToSurfacePdu1(sId, codec, pixFormat, bmRect, avc444BitmapStream.Encode());
            MakeEndFramePdu(fid);
            PackAndSendServerPdu();

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                if (baseImage == null)
                {
                    Site.Assume.Inconclusive("Cannot verify the output since base image is not found, check the existance and format of BaseImage element in test data file.");
                }
                this.bcgrAdapter.SimulatedScreen.RenderUncompressedImage(sId, baseImage, bmRect.left, bmRect.top);
            }

            return fid;
        }

        /// <summary>
        /// Send clearcodec encoded glyph in batch (make sure glyphnum + startGlyphRect.right is not bigger than surf.width)
        /// </summary>
        /// <param name="surf">This is used to indicate the target surface id.</param>
        /// <param name="startGlyphIdx">This is used to indicate the start index of graph batch to be put in client graph cache.</param>
        /// <param name="startGlyphPos">The start position of glyph batch, which will be sent in clearcodec, relative to the surface. </param>
        /// <param name="glyphNum"> The glyph number in batch. </param>
        /// <param name="glyph"> The residual layer image to be sent. </param>
        /// <returns> Frame Id </returns>
        public uint SendClearCodecGlyphInBatch(Surface surf, ushort startGlyphIdx, RDPGFX_POINT16 startGlyphPos, ushort glyphNum, Image glyph)
        {
            uint fid = MakeStartFramePdu();

            ushort glyphIdx = startGlyphIdx;
            RDPGFX_RECT16 glyphRect = new RDPGFX_RECT16(startGlyphPos.x, startGlyphPos.y,
                                            (ushort)(startGlyphPos.x + glyph.Width),
                                            (ushort)(startGlyphPos.y + glyph.Height));

            // Pack multiple w2s_1 PDU into a frame.
            for (ushort i = 0; i < glyphNum; i++)
            {
                ClearCodec_BitmapStream ccStream = new ClearCodec_BitmapStream(ClearCodec_BitmapStream.CLEARCODEC_FLAG_GLYPH_INDEX, glyphIdx);
                ccStream.LoadResidualBitmap((Bitmap)glyph);
                ccStream.seqNumber = egfxServer.Get_ClearCodecBitmapStream_SeqNum();
                MakeWireToSurfacePdu1(surf.Id, CodecType.RDPGFX_CODECID_CLEARCODEC, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                        glyphRect, ccStream.Encode());
                glyphIdx++;
                glyphRect.left++;
                glyphRect.right++;
            }

            MakeEndFramePdu(fid);

            PackAndSendServerPdu();

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.RenderClearCodecBatch(surf.Id, startGlyphIdx, startGlyphPos, glyphNum, glyph);
            }

            return fid;
        }
        /// <summary>
        /// Send a uncompressed bitmap data, the data segment(s) can be compressed or uncompressed according to parameter.
        /// </summary>
        /// <param name="image"> The bitmap image to be sent </param>
        /// <param name="destLeft"> The x-coordination of bitmap image top-left position  </param>
        /// <param name="destTop"> The y-coordination of bitmap image top-left position </param>
        /// <param name="sId"> The surface Id that bitmap image is sent to </param>
        /// <param name="pixFormat"> The pixel format of bitmap image </param>
        /// <param name="compFlag"> The flag indicates whether the bitmap is compressed </param>
        /// <param name="partSize"> The size of pure data in a single RDP8_BULK_ENCODED_DATA structure </param>
        /// <returns> Frame Id </returns>
        public uint SendUncompressedImage(System.Drawing.Image image, ushort destLeft, ushort destTop,
                                            ushort sId, PixelFormat pixFormat, byte compFlag, uint partSize)
        {
            if (image == null)
            {
                Site.Log.Add(LogEntryKind.Debug, "[In iRdprfxAdapter.SendImageToClient Method] The image to be send is null.");
                return 0;
            }

            uint fid = MakeStartFramePdu();

            RDPGFX_RECT16 imageRect = new RDPGFX_RECT16(destLeft, destTop,
                                                            (ushort)(destLeft + image.Width),
                                                            (ushort)(destTop + image.Height));
            byte[] imageData = ImageToByteArray(image);

            MakeWireToSurfacePdu1(sId, CodecType.RDPGFX_CODECID_UNCOMPRESSED, pixFormat, imageRect, imageData);

            MakeEndFramePdu(fid);

            this.segPdu.compressFlag = compFlag;
            this.segPdu.SetSegmentPartSize(partSize);
            PackAndSendServerPdu();

            if (this.bcgrAdapter.SimulatedScreen != null)
            {
                this.bcgrAdapter.SimulatedScreen.RenderUncompressedImage(sId, image, destLeft, destTop);
            }

            return fid;
        }

        /// <summary>
        /// Convert an bitmap to an byte array.
        /// </summary>
        /// <param name="img"> The source bitmap </param>
        private byte[] ImageToByteArray(System.Drawing.Image img)
        {
            Bitmap bmpImg = new Bitmap(img);
            int width = bmpImg.Width;
            int height = bmpImg.Height;
            int right = width - 1;
            int bottom = height - 1;


            // System.Drawing.Imaging.BitmapData bmpData = bmpImg.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            System.Drawing.Imaging.BitmapData bmpData = bmpImg.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            int stride = bmpData.Stride;
            byte[] RGB = new byte[height * stride];

            unsafe
            {
                unsafe
                {
                    byte* cursor = (byte*)bmpData.Scan0.ToPointer();
                    int curIdx = 0;
                    for (int y = 0; y <= bottom; ++y)
                    {
                        for (int x = 0; x <= right; ++x)
                        {
                            RGB[curIdx++] = cursor[y * stride + 4 * x + 0];
                            RGB[curIdx++] = cursor[y * stride + 4 * x + 1];
                            RGB[curIdx++] = cursor[y * stride + 4 * x + 2];
                            RGB[curIdx++] = cursor[y * stride + 4 * x + 3];
                        }
                    }

                }
                bmpImg.UnlockBits(bmpData);
            }
            return RGB;
        }
        #endregion
    }
}
