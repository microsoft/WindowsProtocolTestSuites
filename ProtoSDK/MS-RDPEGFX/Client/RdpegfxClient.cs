// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    public class RdpegfxClient
    {
        #region Variables
        const string RdpegfxGraphicChannelName = "Microsoft::Windows::RDS::Graphics";

        // Instance of RDPEDYC client
        private RdpedycClient rdpedycClient;

        // Dynamic virtual channel of RDPEGFX
        private DynamicVirtualChannel RdpegfxDVC;

        // Buffer of received packets
        private List<RdpegfxPdu> receivedList;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycClient"></param>
        public RdpegfxClient(RdpedycClient rdpedycClient)
        {
            this.rdpedycClient = rdpedycClient;
            receivedList = new List<RdpegfxPdu>();
        }

        #endregion Constructor

        /// <summary>
        /// Wait for creation of dynamic virtual channel for RDPEGFX
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public bool WaitForRdpegfxDvcCreation(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            try
            {
                RdpegfxDVC = rdpedycClient.ExpectChannel(timeout, RdpegfxGraphicChannelName, transportType);
            }
            catch
            {
            }
            if (RdpegfxDVC != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clear ReceiveList 
        /// </summary>
        public void ClearReceivedList()
        {
            if (this.receivedList != null)
            {
                this.receivedList.Clear();
            }
        }

        #region Send/Receive Methods

        /// <summary>
        /// Send a Pdu
        /// </summary>
        /// <param name="pdu"></param>
        public void SendRdpegfxPdu(RdpegfxPdu pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);
            if (RdpegfxDVC == null)
            {
                throw new InvalidOperationException("DVC instance of RDPEGFX is null, Dynamic virtual channel must be created before sending data.");
            }
            RdpegfxDVC.Send(data);
        }

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
                            T response = pdu as T;
                            if (response != null)
                            {
                                receivedList.Remove(pdu);
                                return response;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
            return null;
        }

        #endregion Send/Receive Methods

        #region Private Methods

        /// <summary>
        /// The callback method to receive data from transport layer.
        /// </summary>
        private void OnDataReceived(byte[] data, uint channelID)
        {
            RdpegfxPdu[] receivedPdus = RdpegfxClientDecoder.Decode(data);
            if (receivedPdus != null)
            {
                lock (receivedList)
                {
                    receivedList.AddRange(receivedPdus);
                }
            }
        }

        #endregion Private Methods
    }

    public class RdpegfxClientDecoder
    {
        private static CompressFactory Compressor = new CompressFactory();
        /// <summary>
        /// Decode segment data to an array of server Pdus
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RdpegfxPdu[] Decode(byte[] data)
        {
            RDP_SEGMENTED_DATA segData = new RDP_SEGMENTED_DATA();
            bool fResult = PduMarshaler.Unmarshal(data, segData);
            if (fResult)
            {
                if (segData.descriptor == DescriptorTypes.SINGLE)
                {
                    byte[] rawData = Compressor.Decompress(segData.bulkData.data, segData.bulkData.header); ;


                    return DecodePdus(rawData);
                }
                else
                {
                    List<byte> dataList = new List<byte>();
                    byte[] rawData = null;
                    RDP_DATA_SEGMENT[] dataSegs = segData.segmentArray;
                    for (int i = 0; i < dataSegs.Length; i++)
                    {
                        RDP8_BULK_ENCODED_DATA bulkData = dataSegs[i].bulkData;
                        rawData = Compressor.Decompress(bulkData.data, bulkData.header);
                        dataList.AddRange(rawData);
                    }

                    if (segData.uncompressedSize == dataList.Count)
                    {
                        return DecodePdus(dataList.ToArray());
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Decode multiple PDUs
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RdpegfxPdu[] DecodePdus(byte[] data)
        {
            List<RdpegfxPdu> pduList = new List<RdpegfxPdu>();
            while (data != null && data.Length > 0)
            {
                RdpegfxPdu receivedPdu = DecodeSinglePdu(data);
                if (receivedPdu is RdpegfxUnkownPdu)
                {
                    return pduList.ToArray();
                }
                else
                {
                    int consumedLen = (int)receivedPdu.Header.pduLength;
                    int resLen = data.Length - consumedLen;
                    if (resLen == 0)
                    {
                        data = null;
                    }
                    else
                    {
                        byte[] updatedData = new byte[resLen];
                        Array.Copy(data, consumedLen, updatedData, 0, resLen);
                        data = updatedData;
                    }
                    pduList.Add(receivedPdu);
                }

            }

            return pduList.ToArray();
        }

        /// <summary>
        /// Decode one PDU
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RdpegfxPdu DecodeSinglePdu(byte[] data)
        {

            RdpegfxPdu pdu = new RdpegfxPdu();
            bool fResult = PduMarshaler.Unmarshal(data, pdu);
            if (fResult)
            {
                RdpegfxServerPdu receivedPdu = null;
                if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_WIRETOSURFACE_1)
                {
                    receivedPdu = new RDPGFX_WIRE_TO_SURFACE_PDU_1();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_WIRETOSURFACE_2)
                {
                    receivedPdu = new RDPGFX_WIRE_TO_SURFACE_PDU_2();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_DELETEENCODINGCONTEXT)
                {
                    receivedPdu = new RDPGFX_DELETE_ENCODING_CONTEXT();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_SOLIDFILL)
                {
                    receivedPdu = new RDPGFX_SOLIDFILL();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_SURFACETOSURFACE)
                {
                    receivedPdu = new RDPGFX_SURFACE_TO_SURFACE();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_SURFACETOCACHE)
                {
                    receivedPdu = new RDPGFX_SURFACE_TO_CACHE();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CACHETOSURFACE)
                {
                    receivedPdu = new RDPGFX_CACHE_TO_SURFACE();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_EVICTCACHEENTRY)
                {
                    receivedPdu = new RDPGFX_EVICT_CACHE_ENTRY();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CREATESURFACE)
                {
                    receivedPdu = new RDPGFX_CREATE_SURFACE();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_DELETESURFACE)
                {
                    receivedPdu = new RDPGFX_DELETE_SURFACE();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_STARTFRAME)
                {
                    receivedPdu = new RDPGFX_START_FRAME();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_ENDFRAME)
                {
                    receivedPdu = new RDPGFX_END_FRAME();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_RESETGRAPHICS)
                {
                    receivedPdu = new RDPGFX_RESET_GRAPHICS();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOOUTPUT)
                {
                    receivedPdu = new RDPGFX_MAPSURFACE_TO_OUTPUT();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CACHEIMPORTREPLY)
                {
                    receivedPdu = new RDPGFX_CACHE_IMPORT_REPLY();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CAPSCONFIRM)
                {
                    receivedPdu = new RDPGFX_CAPS_CONFIRM();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOWINDOW)
                {
                    receivedPdu = new RDPGFX_MAP_SURFACE_TO_WINDOW();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_QOEFRAMEACKNOWLEDGE)
                {
                    receivedPdu = new RDPGFX_QOE_FRAME_ACKNOWLEDGE_PDU();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOSCALEDOUTPUT)
                {
                    receivedPdu = new RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU();
                }
                else if (pdu.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOSCALEDWINDOW)
                {
                    receivedPdu = new RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU();
                }

                if (receivedPdu != null && PduMarshaler.Unmarshal(data, receivedPdu))
                {
                    return receivedPdu;
                }
            }
            RdpegfxUnkownPdu unkown = new RdpegfxUnkownPdu();
            PduMarshaler.Unmarshal(data, unkown);
            return unkown;
        }

    }
}
