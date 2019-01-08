// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.ExtendedLogging;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{   

    /// <summary>
    /// The RdpegfxServer is used to transform multiple EGFX server PDUs into byte stream.
    /// </summary>
    public class RdpegfxServer
    {
        #region Variables

        const string RdpegfxGraphicChannelName = "Microsoft::Windows::RDS::Graphics";
        
        /// <summary>
        /// The next available codec context ID, only used in wire_to_surface_pdu_2.
        /// </summary>
        private uint maxCodecContextId;

        /// <summary>
        /// The used codec context ID list. 
        /// </summary>
        private List<uint> codecContextIdList; 

        /// <summary>
        /// The next available frame ID.
        /// </summary>
        private uint maxFId;

        /// <summary>
        /// The next available sequence number, only used in clearcodec.
        /// </summary>
        private byte maxSeqNum;

        private List<RdpegfxPdu> receivedList;

        /// <summary>
        /// Instance of RDPEDYC srver
        /// </summary>
        private RdpedycServer rdpedycServer;

        private DynamicVirtualChannel rdpegfxDVC;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RdpegfxServer(RdpedycServer rdpedycServer)
        {
            this.rdpedycServer = rdpedycServer;
            codecContextIdList = new List<uint>();
            receivedList = new List<RdpegfxPdu>();
            maxCodecContextId = 0;
            maxFId = 0;
            maxSeqNum = 0;
        }

        #endregion Constructor

        /// <summary>
        /// Create dynamic virtual channel.
        /// </summary>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <param name="timeout">Timeout</param>
        /// <param name="channelId">ChannelId of this DVC</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool CreateRdpegfxDvc(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP, uint? channelId = null)
        {

            const ushort priority = 0;
            try
            {
                rdpegfxDVC = rdpedycServer.CreateChannel(timeout, priority, RdpegfxGraphicChannelName, transportType, OnDataReceived, channelId);
            }
            catch
            {
            }
            if (rdpegfxDVC != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get next frame id.
        /// </summary>
        public uint GetFrameId()
        {
            return maxFId++;
        }

        /// <summary>
        /// Get next available codec context id.
        /// </summary>
        public uint GetCodecContextId()
        {
            codecContextIdList.Add(maxCodecContextId);
            return maxCodecContextId++;
        }

        /// <summary>
        /// Get a used codec context id.
        /// </summary>
        public bool GetUsedCodecContextId(ref uint contextId)
        {
            if (codecContextIdList.Count > 0)
            {
                uint[] contextIdArr = codecContextIdList.ToArray();
                contextId = contextIdArr[0];
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove a codec context id from used list.
        /// </summary>
        public void DeleteUsedCodecContextId(uint contextId)
        {
            codecContextIdList.Remove(contextId);
        }

        /// <summary>
        /// Get next clear codec sequence id.
        /// </summary>
        public byte Get_ClearCodecBitmapStream_SeqNum()
        {
            return maxSeqNum++;
        }

        #region Create Methods

        /// <summary>
        /// Create a Capability Confirm PDU.
        /// </summary>
        /// <param name="capFlag">The valid rdpgfx_capset_version8 flag.</param>
        /// <param name="version">version of the capability</param>
        public RDPGFX_CAPS_CONFIRM CreateCapabilityConfirmPdu(CapsFlags capFlag, CapsVersions version)
        {
            RDPGFX_CAPS_CONFIRM capsConfirm = new RDPGFX_CAPS_CONFIRM(capFlag, version);
            return capsConfirm;
        }

        /// <summary>
        /// Create a Cache Offer Reply PDU.
        /// </summary>
        public RDPGFX_CACHE_IMPORT_REPLY CreateCacheImportReplyPdu()
        {
            RDPGFX_CACHE_IMPORT_REPLY cacheImportReply = new RDPGFX_CACHE_IMPORT_REPLY();
            return cacheImportReply;
        }

        /// <summary>
        /// Create a Reset Graphics Pdu.
        /// </summary>
        /// <param name="w">Width of virtual desktop.</param>
        /// <param name="h">Height of virtual desktop.</param>
        /// <param name="monitorCount">Count of Monitors.</param>
        public RDPGFX_RESET_GRAPHICS CreateResetGraphicsPdu(uint w, uint h, uint monitorCount)
        {
            // Reset size of virtual desktop.
            RDPGFX_RESET_GRAPHICS resetGraphics = new RDPGFX_RESET_GRAPHICS(w, h, monitorCount);
            return resetGraphics;
        }

        /// <summary>
        /// Create a start frame Pdu.
        /// </summary>
        /// <param name="fid">This is used to indicate frame id.</param>
        public RDPGFX_START_FRAME CreateStartFramePdu(uint fid)
        {
            RDPGFX_START_FRAME startFrame = new RDPGFX_START_FRAME(fid);
            return startFrame;
        }

        /// <summary>
        /// Create an end frame Pdu.
        /// </summary>
        /// <param name="fid">This is used to indicate frame id.</param>
        public RDPGFX_END_FRAME CreateEndFramePdu(uint fid)
        {
            RDPGFX_END_FRAME endFrame = new RDPGFX_END_FRAME(fid);
            return endFrame;
        }

        /// <summary>
        /// Create a Create Surface Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="w">This is used to indicate width of surface.</param>
        /// <param name="h">This is used to indicate height of surface.</param>
        /// <param name="pix">This is used to indicate pixel format of surface.</param>
        public RDPGFX_CREATE_SURFACE CreateCreateSurfacePdu(ushort sid, ushort w, ushort h, PixelFormat pix)
        {
            RDPGFX_CREATE_SURFACE createSurf = new RDPGFX_CREATE_SURFACE(sid, w, h, pix);
            return createSurf;
        }

        /// <summary>
        /// Method to make a map Surface to output Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="x">This is used to indicate x-coordinate of the upper-left corner of the surface.</param>
        /// <param name="y">This is used to indicate y-coordinate of the upper-left corner of the surface.</param>
        public RDPGFX_MAPSURFACE_TO_OUTPUT CreateMapSurfaceToOutputPdu(ushort sid, uint x, uint y)
        {
            RDPGFX_MAPSURFACE_TO_OUTPUT surf2Output = new RDPGFX_MAPSURFACE_TO_OUTPUT(sid, x, y);
            return surf2Output;

        }

        /// <summary>
        /// Method to make a map Surface to scaled output Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="x">This is used to indicate x-coordinate of the upper-left corner of the surface.</param>
        /// <param name="y">This is used to indicate y-coordinate of the upper-left corner of the surface.</param>
        /// <param name="w">This is used to indicate targetWidth of the surface.</param>
        /// <param name="h">This is used to indicate targeHeight of the surface.</param>
        public RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU CreateMapSurfaceToScaledOutputPdu(ushort sid, uint x, uint y, uint w, uint h)
        {
            RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU surf2ScaledOutput = new RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU(sid, x, y, w, h);
            return surf2ScaledOutput;

        }

        /// <summary>
        /// Method to make a map Surface to Window Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="wid">This is used to indicate window id.</param>
        /// <param name="w">This is used to indicate mapped width.</param>
        /// <param name="h">This is used to indicate mapped height.</param>
        public RDPGFX_MAPSURFACE_TO_WINDOW CreateMapSurfaceToWindowPdu(ushort sid, ulong wid, uint w, uint h)
        {
            RDPGFX_MAPSURFACE_TO_WINDOW surf2Window = new RDPGFX_MAPSURFACE_TO_WINDOW(sid, wid, w, h);
            return surf2Window;
        }

        /// <summary>
        /// Method to make a map Surface to scaled output Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="wid">This is used to indicate the window id.</param>
        /// <param name="mappedWidth">This is used to indicate mappedWidth of the surface.</param>
        /// <param name="mappedHeight">This is used to indicate mappedHeight of the surface.</param>
        /// <param name="targetdWidth">This is used to indicate targetWidth of the window.</param>
        /// <param name="targetdHeight">This is used to indicate targeHeight of the window.</param>
        public RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU CreateMapSurfaceToScaledWindowPdu(ushort sid, ulong wid, uint mappedWidth, uint mappedHeight, uint targetWidth, uint targetHeight)
        {
            RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU surf2ScaledWindow = new RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU(sid, wid, mappedWidth, mappedHeight,targetWidth,targetHeight);
            return surf2ScaledWindow;
        }

        /// <summary>
        /// Create a solid fill Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id to be filled.</param>
        /// <param name="pixel">This is used to indicate color to fill.</param>
        /// <param name="rects">This is to specify rectangle areas in surface. </param>
        public RDPGFX_SOLIDFILL CreateSolidFillPdu(ushort sid, RDPGFX_COLOR32 color, RDPGFX_RECT16[] rects)
        {
            RDPGFX_SOLIDFILL solidFill = new RDPGFX_SOLIDFILL(sid, color);
            if (rects != null)
            {
                for (int i = 0; i < rects.Length; i++)
                    solidFill.addFillRect(rects[i]);
            }

            return solidFill;
        }

        /// <summary>
        /// Create a surface to cache Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="key">This is used to indicate a key to associate with the bitmap cache entry.</param>
        /// <param name="slot">This is used to indicate the index of the bitmap cache entry in which 
        /// the source bitmap data is stored.</param>
        /// <param name="rect">This is used to indicate rectangle that bounds the source bitmap.</param>
        public RDPGFX_SURFACE_TO_CACHE CreateSurfaceToCachePdu(ushort sid, ulong key, ushort slot, RDPGFX_RECT16 rect)
        {
            RDPGFX_SURFACE_TO_CACHE surfaceToCache = new RDPGFX_SURFACE_TO_CACHE(sid, key, slot, rect);
            return surfaceToCache;
        }

        /// <summary>
        /// Create a cache to surface Pdu.
        /// </summary>
        /// <param name="slot">This is used to indicate a cache slot.</param>
        /// <param name="sid">This is used to indicate the destination surface id.</param>
        /// <param name="destPoints">This is used to specify destination points of source rectangle bitmap to be copied. </param>
        public RDPGFX_CACHE_TO_SURFACE CreateCacheToSurfacePdu(ushort slot, ushort sid, RDPGFX_POINT16[] destPoints)
        {
            RDPGFX_CACHE_TO_SURFACE cacheToSurface = new RDPGFX_CACHE_TO_SURFACE(slot, sid);
            if (destPoints != null)
            {
                for (ushort i = 0; i < destPoints.Length; i++)
                {
                    cacheToSurface.AddDestPosition(destPoints[i]);
                }
            }
            return cacheToSurface;
        }

        /// <summary>
        /// Create a surface to surface Pdu.
        /// </summary>
        /// <param name="srcSID">This is used to indicate source surface id.</param>
        /// <param name="destSID">This is used to indicate destination surface id.</param>
        /// <param name="srcRect">This is used to indicate source rectangle bitmap area to be copied.</param>
        /// <param name="destPoints">This is used to specify destination points of source rectangle bitmap to be copied. </param>
        public RDPGFX_SURFACE_TO_SURFACE CreateSurfaceToSurfacePdu(ushort srcSID, ushort destSID, RDPGFX_RECT16 srcRect, RDPGFX_POINT16[] destPoints)
        {
            RDPGFX_SURFACE_TO_SURFACE surfToSurf = new RDPGFX_SURFACE_TO_SURFACE(srcSID, destSID, srcRect);
            if (destPoints != null)
            {
                for (int i = 0; i < destPoints.Length; i++)
                {
                    surfToSurf.AddDestPosition(destPoints[i]);
                }
            }
            return surfToSurf;
        }

        /// <summary>
        /// Create a delete surface Pdu.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        public RDPGFX_DELETE_SURFACE CreateDeleteSurfacePdu(ushort sid)
        {
            RDPGFX_DELETE_SURFACE delSurface = new RDPGFX_DELETE_SURFACE(sid);
            return delSurface;
        }

        /// <summary>
        /// Create a wire to surface Pdu1.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="cId">This is used to indicate the codecId.</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">This is used to indicate border of bitmap on target surface.</param>
        /// <param name="bmLen">This is used to indicate the length of bitmap data.</param>
        /// <param name="bmData">This is used to indicate the bitmap data encoded by cId codec.</param>
        public RDPGFX_WIRE_TO_SURFACE_PDU_1 CreateWireToSurfacePdu1(ushort sId, CodecType cId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, byte[] bmData)
        {
            RDPGFX_WIRE_TO_SURFACE_PDU_1 wireToSurf1 = new RDPGFX_WIRE_TO_SURFACE_PDU_1(sId, cId, pixFormat, bmRect, bmData);
            return wireToSurf1;
        }

        /// <summary>
        /// Create a wire to surface Pdu2.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmLen">This is used to indicate the length of bitmap data.</param>
        /// <param name="bmData">This is used to indicate the bitmap data encoded by cId codec.</param>
        public RDPGFX_WIRE_TO_SURFACE_PDU_2 CreateWireToSurfacePdu2(ushort sId, PixelFormat pixFormat, byte[] bmData)
        {
            uint codecCtxId = this.GetCodecContextId();
            RDPGFX_WIRE_TO_SURFACE_PDU_2 wire2surf = new RDPGFX_WIRE_TO_SURFACE_PDU_2(sId, codecCtxId, pixFormat, bmData);
            return wire2surf;
        }

        /// <summary>
        /// Create a delete encoding context pdu.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="codecCtxId">This is used to indicate the codecContextId.</param>
        public RDPGFX_DELETE_ENCODING_CONTEXT CreateRfxProgressiveCodexContextDeletionPdu(ushort sId, uint codecCtxId)
        {
            this.DeleteUsedCodecContextId(codecCtxId);
            RDPGFX_DELETE_ENCODING_CONTEXT deleteContext = new RDPGFX_DELETE_ENCODING_CONTEXT(sId, codecCtxId);
            return deleteContext;
        }

        #endregion Create Methods

        /// <summary>
        /// Reset all variables in the class.
        /// </summary>
        public void Reset()
        {
            codecContextIdList.Clear();

            maxCodecContextId = 0;
            maxFId = 0;
            maxSeqNum = 0;
        }

        #region Send Methods

        /// <summary>
        /// Method to sent a packet via RDPEGFX DVC to client.
        /// </summary>
        /// <param name="Data">The packet data to be sent to client.</param>
        public void Send(byte[] data)
        {
            if (this.rdpegfxDVC != null)
            {
                rdpegfxDVC.Send(data);
            }
        }

        #endregion Send Methods

        #region Receive Methods

        /// <summary>
        /// Method to expect a RdpegfxPdu.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public T ExpectRdpegfxPdu<T>(TimeSpan timeout) where T : RdpegfxPdu
        {
            DateTime endTime = DateTime.Now + timeout;

            while (endTime > DateTime.Now)
            {
                if (receivedList.Count > 0)
                {
                    lock (receivedList)
                    {
                        foreach (RdpegfxPdu pdu in receivedList)
                        {
                            T recv = pdu as T;
                            if (recv != null)
                            {
                                receivedList.Remove(pdu);
                                return recv;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
            return null;
        }
        
        #endregion Receive Methods

        #region Private methods

        /// <summary>
        /// The callback method to receive data from transport layer.
        /// </summary>
        private void OnDataReceived(byte[] data, uint channelId)
        {
            lock (receivedList)
            {
                RdpegfxPdu pdu = new RdpegfxPdu();
                bool fSucceed = false;
                bool fResult = PduMarshaler.Unmarshal(data, pdu);
                if (fResult)
                {
                    byte[] pduData = new byte[pdu.Header.pduLength];
                    Array.Copy(data, pduData, pduData.Length);
                    if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CAPSADVERTISE)
                    {
                        RDPGFX_CAPS_ADVERTISE recv = new RDPGFX_CAPS_ADVERTISE();
                        fSucceed = AddToReceivedList(pduData, recv);

                    }
                    else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_FRAMEACKNOWLEDGE)
                    {
                        RDPGFX_FRAME_ACK recv = new RDPGFX_FRAME_ACK();
                        fSucceed = AddToReceivedList(pduData, recv);
                    }
                    else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CACHEIMPORTOFFER)
                    {
                        RDPGFX_CACHE_IMPORT_OFFER recv = new RDPGFX_CACHE_IMPORT_OFFER();
                        fSucceed = AddToReceivedList(pduData, recv);
                    }

                }
                if (!fResult || !fSucceed)
                {
                    RdpegfxUnkownPdu unkown = new RdpegfxUnkownPdu();
                    PduMarshaler.Unmarshal(data, unkown);
                    receivedList.Add(unkown);
                }
            }
        }

        /// <summary>
        /// The AddToReceivedList method to add receive message into receive list.
        /// </summary>
        private bool AddToReceivedList(byte[] pduData, RdpegfxPdu recv)
        {
            bool fSucceed = false;
            try
            {
                fSucceed = PduMarshaler.Unmarshal(pduData, recv);
                receivedList.Add(recv);
            }
            catch (PDUDecodeException decodeExceptioin)
            {
                RdpegfxUnkownPdu unkown = new RdpegfxUnkownPdu();
                fSucceed = PduMarshaler.Unmarshal(decodeExceptioin.DecodingData, unkown);
                receivedList.Add(unkown);
            }
            return fSucceed;
        }

        #endregion Private methods
    }

    #region SEGMENTED PDU for server messages
    /// <summary>
    /// The RDP Sever byte stream Must be encapsulated as RdpSegmentPdu with/without compression.
    /// </summary>
    public class EGFXRdpSegmentedPdu
    {
        #region Variables
        private RDP_SEGMENTED_DATA segHeader;

        public byte compressFlag;
        public const byte PACKET_COMPR_TYPE_RDP8 = 0x04;
        public const byte PACKET_COMPRESSED = 0x20;
        #endregion

        #region private variable
        List<RDP_SEGMENTED_DATA> segHeadList;  // List of segmented server data, excluding segment header.
        List<byte[]> segPduList;   // List of segment PDU, include segment header.
        private RdpegfxNegativeTypes currentTestType;  // Indicate the current test type
        private uint segmentPartSize;  // The pure data size in a single RDP8_BULK_ENCODED_DATA structure
        private const uint SEGMENT_PART_SIZE = 65535;  // Maximum number of uncompressed bytes in a single segment is 65535
        #endregion private variable

        /// <summary>
        /// Contructor. 
        /// </summary>
        /// <param name="descType"> Indicates if a single or multipart segment PDU.</param>
        /// <param name="compFlag">Indicates the data is compressed and the compress type.</param>
        public EGFXRdpSegmentedPdu(byte compFlag)
        {
            // segHeader.descType = descType;
            compressFlag = compFlag;
            // default number of uncompressed bytes in a single segment is 65535.
            segmentPartSize = SEGMENT_PART_SIZE;

            segHeadList = new List<RDP_SEGMENTED_DATA>();
            segPduList = new List<byte[]>();

            segHeader = new RDP_SEGMENTED_DATA();

        }

        // public void AttachServerPdu(RdpegfxServer svrPdu)
        public void SegmentAndCompressFrame(byte[] rawSvrData)
        {

            // byte[] rawSvrData = PduMarshaler.Marshal(svrPdu);
            // Set description type based on data length
            if (rawSvrData.Length <= segmentPartSize)    
            {
                segHeader.descriptor = DescriptorTypes.SINGLE;
                segHeader.bulkData = new RDP8_BULK_ENCODED_DATA();
                if (currentTestType == RdpegfxNegativeTypes.Segmentation_Uncompressed_WithSegmentHeader)
                    compressFlag = EGFXRdpSegmentedPdu.PACKET_COMPR_TYPE_RDP8;
                else if (currentTestType == RdpegfxNegativeTypes.RDP8Compression_InvalidCompressPDU)
                    compressFlag = EGFXRdpSegmentedPdu.PACKET_COMPR_TYPE_RDP8 | EGFXRdpSegmentedPdu.PACKET_COMPRESSED;

                segHeader.bulkData.header = compressFlag;

                // RDP 8.0 compression here. 
                if (compressFlag == (EGFXRdpSegmentedPdu.PACKET_COMPR_TYPE_RDP8 | EGFXRdpSegmentedPdu.PACKET_COMPRESSED))
                {
                    CompressFactory cpf = new CompressFactory();
                    byte[] compressedData = cpf.Compress(rawSvrData);
                    segHeader.bulkData.data = compressedData;

                    // ETW Provider Dump message
                    string messageName = "RDPEGFX:DecompressedData";
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, "RDPEGFX decompressed data", rawSvrData);
                }
                else
                {
                    segHeader.bulkData.data = rawSvrData;
                }

                if (currentTestType == RdpegfxNegativeTypes.RDP8Compression_InvalidCompressPDU)
                {
                    segHeader.bulkData.data = rawSvrData;
                }

                segHeadList.Add(segHeader);
            }
            else
            {
                segHeader.descriptor = DescriptorTypes.MULTIPART;
                segHeader.uncompressedSize = (uint)(rawSvrData.Length);
                int totalLength = rawSvrData.Length;
                if (totalLength % segmentPartSize == 0)
                {
                    segHeader.segmentCount = (ushort)(totalLength / segmentPartSize);
                }
                else
                {
                    segHeader.segmentCount = (ushort)(totalLength / segmentPartSize + 1);
                }

                segHeader.segmentArray = new RDP_DATA_SEGMENT[segHeader.segmentCount];
                uint baseIndex = 0;
                uint cnt = 0;

                while (cnt < segHeader.segmentCount)
                {
                    Byte[] rawPartData;
                    if (cnt + 1 < segHeader.segmentCount)
                    {
                        rawPartData = new Byte[segmentPartSize];
                        Array.Copy(rawSvrData, baseIndex, rawPartData, 0, segmentPartSize);
                    }
                    else // Last segment.
                    {
                        rawPartData = new Byte[(uint)totalLength - baseIndex];
                        Array.Copy(rawSvrData, baseIndex, rawPartData, 0, (uint)totalLength - baseIndex);
                    }

                    segHeader.segmentArray[cnt] = new RDP_DATA_SEGMENT();

                    if (compressFlag == (EGFXRdpSegmentedPdu.PACKET_COMPR_TYPE_RDP8 | EGFXRdpSegmentedPdu.PACKET_COMPRESSED))
                    {
                        CompressFactory cpf = new CompressFactory();
                        byte[] compressData = cpf.Compress(rawPartData);
                        segHeader.segmentArray[cnt].bulkData = new RDP8_BULK_ENCODED_DATA();
                        segHeader.segmentArray[cnt].bulkData.header = compressFlag;
                        segHeader.segmentArray[cnt].bulkData.data = compressData;
                        segHeader.segmentArray[cnt].size = (uint)(segHeader.segmentArray[cnt].bulkData.data.Length + 1);

                        // ETW Provider Dump message
                        string messageName = "RDPEGFX:DecompressedData";
                        ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, "RDPEGFX decompressed data", rawPartData);
                    }
                    else
                    {
                        segHeader.segmentArray[cnt].bulkData = new RDP8_BULK_ENCODED_DATA();
                        segHeader.segmentArray[cnt].bulkData.header = compressFlag;
                        segHeader.segmentArray[cnt].bulkData.data = rawPartData;
                        segHeader.segmentArray[cnt].size = (uint)(segHeader.segmentArray[cnt].bulkData.data.Length + 1);
                    }

                    baseIndex += segmentPartSize;
                    cnt++;
                }

                // Add segmented data into segHeadList.
                segHeadList.Add(segHeader);
            }
            
        }


        public List<byte[]> EncodeSegPdu()
        {
            foreach (RDP_SEGMENTED_DATA segHead in segHeadList)
            {
                List<byte> dataBuffer = new List<byte>();
                if (segHead.descriptor == DescriptorTypes.SINGLE)
                {
                    if (currentTestType != RdpegfxNegativeTypes.Segmentation_Compressed_NoSegemntHeader &&
                        currentTestType != RdpegfxNegativeTypes.Segmentation_Uncompressed_NoSegmentHeader)   // For this two test types, not add the segment header to the databuffer.
                    {
                        if (currentTestType == RdpegfxNegativeTypes.Segmentation_Incorrect_SegmentDescriptor)
                        {
                            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>((byte)(0)));   // Set the descriptor to an invalid value 0x00 other than 0xE0.
                        }
                        else
                        {
                            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>((byte)(segHead.descriptor)));
                        }
                    }
                    if (currentTestType == RdpegfxNegativeTypes.Segmentation_SingleSegment_WithSegmentCount)
                    {
                        dataBuffer.AddRange(BitConverter.GetBytes((ushort)1));    // Add an invalid additional segmentCount field with value 0x0001.
                    }
                    if (currentTestType == RdpegfxNegativeTypes.Segmentation_SingleSegment_WithUncompessedSize)
                    {
                        dataBuffer.AddRange(BitConverter.GetBytes(0x000000ff));   // Add an invalid additional uncompressedSize field with value 0x000000ff. 
                    }
                    if (currentTestType == RdpegfxNegativeTypes.Segmentation_SingleSegment_WithSegmentArray)
                    {
                        dataBuffer.AddRange(BitConverter.GetBytes(segHead.bulkData.data.Length + 1));  // Add an invalid additional size field with value of the RDP8_BULK_ENCODED_DATA structure's size. 
                    }

                    if (currentTestType == RdpegfxNegativeTypes.RDP8Compression_IncorrectCompressFlag)
                    {
                        segHead.bulkData.header = EGFXRdpSegmentedPdu.PACKET_COMPR_TYPE_RDP8 | 0x2;  // 0x02 is an invalid compress flag.
                    }
                    else if (currentTestType == RdpegfxNegativeTypes.RDP8Compression_IncorrectCompressType)
                    {
                        segHead.bulkData.header = 0x40;  // 0x40 is an invalid compress flag.
                    }

                    dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(segHead.bulkData.header));
                    dataBuffer.AddRange(segHead.bulkData.data);
                }
                else
                {
                    if (currentTestType != RdpegfxNegativeTypes.Segmentation_Compressed_NoSegemntHeader)   // For this test type, not add the segment header to the databuffer.
                    {
                        dataBuffer.AddRange(TypeMarshal.ToBytes<byte>((byte)(segHeader.descriptor)));
                        if (currentTestType != RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutSegmentCount)   // For this test type, not add the segmentCount field to the databuffer.
                        {
                            dataBuffer.AddRange(BitConverter.GetBytes(segHeader.segmentCount));
                        }
                        if (currentTestType != RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutUncompressedSize)  // For this test type, not add the uncompressedSize field to the databuffer.
                        {
                            dataBuffer.AddRange(BitConverter.GetBytes(segHeader.uncompressedSize));
                        }
                    }

                    if (currentTestType != RdpegfxNegativeTypes.Segmentation_MultiSegments_WithoutSegmentArray)   // For this test type, not add the segmentArray field to the databuffer.
                    {
                        for (int i = 0; i < segHeader.segmentCount; ++i)
                        {
                            dataBuffer.AddRange(BitConverter.GetBytes(segHead.segmentArray[i].size));   // Increase the size by 1 for the header of the RDP8_BULK_ENCODED_DATA structure.
                            dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(segHead.segmentArray[i].bulkData.header));
                            dataBuffer.AddRange(segHead.segmentArray[i].bulkData.data);
                        }
                    }

                }

                segPduList.Add(dataBuffer.ToArray());
            }

            return segPduList;
        }

        public void ClearSegments()
        {
            segHeadList.Clear();
            segPduList.Clear();
            
        }

        /// <summary>
        /// Set the type of current test.
        /// </summary>
        /// <param name="testType">The test type.</param>
        public void SetTestType(RdpegfxNegativeTypes testType)
        {
            currentTestType = testType;
        }

        /// <summary>
        /// Set the current segment part size.
        /// </summary>
        /// <param name="partSize">The part size.</param>
        public void SetSegmentPartSize(uint partSize)
        {
            segmentPartSize = partSize;
        }

        public void Reset()
        {
            currentTestType = RdpegfxNegativeTypes.None;
            ClearSegments();
        }
        
    }

    #endregion
}
