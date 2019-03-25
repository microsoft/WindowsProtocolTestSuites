// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.ExtendedLogging;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    /// <summary>
    /// Class used to save unprocessed Packet
    /// </summary>
    public class UnprocessedDVCPDUInfo
    {
        public DynamicVC_TransportType TransportType;

        public DynamicVCPDU PDU;

        public UnprocessedDVCPDUInfo(DynamicVCPDU pdu, DynamicVC_TransportType type)
        {
            this.PDU = pdu;
            this.TransportType = type;
        }
    }

    public class RdpedycServer : IDisposable
    {
        #region Variables

        private TimeSpan waitInterval = new TimeSpan(0, 0, 0, 0, 100);

        private RdpbcgrServer rdpbcgrServer;
        private RdpbcgrServerSessionContext sessionContext;

        private Dictionary<DynamicVC_TransportType, IDVCTransport> transportDic;

        private List<UnprocessedDVCPDUInfo> unprocessedDVCPacketBuffer;

        private Dictionary<UInt32, DynamicVirtualChannel> channelDicbyId;

        public PduBuilder pduBuilder;

        private bool autoCloseChannel;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server"></param>
        /// <param name="context"></param>
        public RdpedycServer(RdpbcgrServer server, RdpbcgrServerSessionContext context, bool autoCloseChannel = true)
        {
            this.rdpbcgrServer = server;
            this.sessionContext = context;
            transportDic = new Dictionary<DynamicVC_TransportType, IDVCTransport>();
            unprocessedDVCPacketBuffer = new List<UnprocessedDVCPDUInfo>();
            Rdpbcgr_DVCServerTransport transport = new Rdpbcgr_DVCServerTransport(context);

            channelDicbyId = new Dictionary<uint, DynamicVirtualChannel>();

            pduBuilder = new PduBuilder();

            transport.Received += ProcessPacketFromTCP;
            transportDic.Add(DynamicVC_TransportType.RDP_TCP, transport);

            this.autoCloseChannel = autoCloseChannel;

        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Create a multitransport, RDP_UDP_reliable or RDP_UDP_Lossy
        /// </summary>
        /// <param name="transportType">Type of the transport, reliable or lossy</param>
        public void CreateMultipleTransport(DynamicVC_TransportType transportType)
        {
            if (transportDic.ContainsKey(transportType))
            {
                throw new InvalidOperationException("The multiple transport have already been created:" + transportType);
            }

            Rdpemt_DVCServerTransport transport = new Rdpemt_DVCServerTransport(rdpbcgrServer, sessionContext, transportType);

            if (transportType == DynamicVC_TransportType.RDP_UDP_Reliable)
            {
                transport.Received += ProcessPacketFromUDPR;
            }
            else
            {
                transport.Received += ProcessPacketFromUDPL;
            }
            transportDic.Add(transportType, transport);
        }

        /// <summary>
        /// Create multiple transports, both reliable and lossy
        /// </summary>
        public void CreateMultipleTransports()
        {
            CreateMultipleTransport(DynamicVC_TransportType.RDP_UDP_Reliable);
            CreateMultipleTransport(DynamicVC_TransportType.RDP_UDP_Lossy);
        }

        /// <summary>
        /// Whether a multitransport has been created
        /// </summary>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public bool IsMultipleTransportCreated(DynamicVC_TransportType transportType)
        {
            if (transportDic.ContainsKey(transportType))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Exchange DVC capability, negotiate the version 
        /// </summary>
        /// <param name="version"></param>
        public ushort ExchangeCapabilities(TimeSpan timeout, DYNVC_CAPS_Version version = DYNVC_CAPS_Version.VERSION3)
        {
            this.SendDVCCapRequestPDU(version);
            CapsRespDvcPdu capResp = this.ExpectDVCCapResponsePDU(timeout);
            if (capResp == null)
            {
                throw new System.IO.IOException("Cannot receive a DVC Capabilities Response PDU!");
            }
            return capResp.Version;
        }

        /// <summary>
        /// Create a dynamic virtual channel 
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="channelName">Channel name</param>
        /// <param name="transportType">Transport type</param>
        /// <param name="receiveCallBack">Callback method called when received data</param>
        /// <returns>DVC created</returns>
        public DynamicVirtualChannel CreateChannel(TimeSpan timeout, ushort priority, string channelName, DynamicVC_TransportType transportType, ReceiveData receiveCallBack = null, uint? channelId = null)
        {
            if (!transportDic.ContainsKey(transportType))
            {
                throw new InvalidOperationException("Not create DVC transport:" + transportType);
            }

            if (channelId == null)
                channelId = DynamicVirtualChannel.NewChannelId();
            DynamicVirtualChannel channel = new DynamicVirtualChannel((UInt32)channelId, channelName, priority, transportDic[transportType]);

            if (receiveCallBack != null)
            {
                // Add event method here can make sure processing the first DVC data packet
                channel.Received += receiveCallBack;
            }

            channelDicbyId.Add((UInt32)channelId, channel);

            this.SendDVCCreateRequestPDU(priority, (UInt32)channelId, channelName, transportType);
            CreateRespDvcPdu createResp = this.ExpectDVCCreateResponsePDU(timeout, (UInt32)channelId, transportType);
            if (createResp == null)
            {
                throw new System.IO.IOException("Creation of channel: " + channelName + " failed, cannot receive a Create Response PDU");
            }
            if (createResp.CreationStatus < 0)
            {
                //Create failed
                throw new System.IO.IOException("Creation of DVC failed with error code: " + createResp.CreationStatus + ", channel name is " + channelName);
            }

            return channel;
        }

        /// <summary>
        /// Send Soft Sync request and wait for Soft Sync response.
        /// </summary>
        /// <param name="timeout">wait time</param>
        /// <param name="flags">specifies the contents of this PDU</param>
        /// <param name="numberOfTunnels">tunnel numbers</param>
        /// <param name="channelList">DYNVC_SOFT_SYNC_CHANNEL_LISTs</param>
        public void SoftSyncNegotiate(TimeSpan timeout, SoftSyncReqFlags_Value flags, ushort numberOfTunnels, SoftSyncChannelList[] channelList = null)
        {
            this.SendSoftSyncRequestPDU(flags, numberOfTunnels, channelList);
            SoftSyncResDvcPdu pdu = this.ExpectSoftSyncResponsePDU(timeout);
            if (pdu == null)
            {
                throw new System.IO.IOException("Cannot receive a Soft Sync Response PDU!");
            }
        }

        /// <summary>
        /// Send Soft Sync requst.
        /// </summary>
        /// <param name="timeout">wait time</param>
        /// <param name="channelListDic">TunnelType value & Channel Ids</param>
        public void SoftSyncNegotiate(TimeSpan timeout, Dictionary<TunnelType_Value, List<uint>> channelListDic = null)
        {
            SoftSyncReqFlags_Value flags = SoftSyncReqFlags_Value.SOFT_SYNC_TCP_FLUSHED;
            if (channelListDic != null)
            {
                flags |= SoftSyncReqFlags_Value.SOFT_SYNC_CHANNEL_LIST_PRESENT;
            }

            List<SoftSyncChannelList> channelList = new List<SoftSyncChannelList>();
            int numberOfTunnels = 0;
            if (channelListDic != null)
            {
                foreach (KeyValuePair<TunnelType_Value, List<uint>> kvp in channelListDic)
                {
                    SoftSyncChannelList channel = new SoftSyncChannelList(kvp.Key, (ushort)kvp.Value.Count, kvp.Value);
                    channelList.Add(channel);
                }
                numberOfTunnels = channelListDic.Count;
            }

            SoftSyncNegotiate(timeout, flags, (ushort)numberOfTunnels, channelList.ToArray());
        }

        public void SendDataCompressedReqPdu(uint channelId, byte[] data)
        {
            DataCompressedDvcPdu pdu = pduBuilder.CreateDataCompressedReqPdu(channelId, data);
            Send(pdu, DynamicVC_TransportType.RDP_UDP_Reliable);
        }
        public void SendFirstCompressedDataPdu(uint channelId, byte[] data)
        {
            //According to section 3.1.5.1.4 of MS-RDPEDYC,
            //If the total uncompressed length of the message exceeds 1,590 bytes, 
            //the DYNVC_DATA_FIRST_COMPRESSED (section 2.2.3.3) PDU is sent as the first data PDU, 
            //followed by DYNVC_DATA_COMPRESSED (section 2.2.3.4) PDUs until all the data has been sent.
            if (data.Length <= ConstLength.MAX_UNCOMPRESSED_DATA_LENGTH)
            {
                byte[] compressedData = pduBuilder.CompressDataToRdp8BulkEncodedData(data, PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_LITE | PACKET_COMPR_FLAG.PACKET_COMPRESSED);
                DataFirstCompressedDvcPdu firstCompressedPdu = new DataFirstCompressedDvcPdu(channelId, (uint)data.Length, compressedData);
                firstCompressedPdu.GetNonDataSize();
                Send(firstCompressedPdu, DynamicVC_TransportType.RDP_UDP_Reliable);
            }
            else
            {
                //Cmd:4 bits, Len: 2 bits, cbChid:2 bits, ChannelId: 8 bit, Length: no more than 1600, so it it 16 bits. Totally, 4 bytes
                // Descriptor is 1 byte, Header is 1 byte
                //So the max length of the data should be 1600 (Max Chunk Length)-6
                byte[] uncompressedData = new byte[ConstLength.MAX_FIRST_COMPRESSED_DATA_LENGTH];
                Array.Copy(data, uncompressedData, ConstLength.MAX_FIRST_COMPRESSED_DATA_LENGTH);
                byte[] compressedData = pduBuilder.CompressDataToRdp8BulkEncodedData(uncompressedData, PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_LITE | PACKET_COMPR_FLAG.PACKET_COMPRESSED);

                DataFirstCompressedDvcPdu firstCompressedPdu = new DataFirstCompressedDvcPdu(channelId, (uint)data.Length, compressedData);
                firstCompressedPdu.GetNonDataSize();
                Send(firstCompressedPdu, DynamicVC_TransportType.RDP_UDP_Reliable);

                int leftBytes = uncompressedData.Length - (int)ConstLength.MAX_FIRST_COMPRESSED_DATA_LENGTH;
                int followingMsgCount = 0;

                if (leftBytes > 0)
                {
                    int followingLen = data.Length - (int)ConstLength.MAX_FIRST_COMPRESSED_DATA_LENGTH;
                    followingMsgCount = (followingLen / (int)ConstLength.MAX_COMPRESSED_DATA_LENGTH);
                    followingMsgCount = (followingLen % (int)ConstLength.MAX_COMPRESSED_DATA_LENGTH == 0) ? followingMsgCount : ++followingMsgCount;
                    for (int i = 0; i < followingMsgCount; i++)
                    {
                        if (i != followingMsgCount)
                        {
                            byte[] followingUnCompressedData = new byte[ConstLength.MAX_COMPRESSED_DATA_LENGTH];
                            Array.Copy(data, i * ConstLength.MAX_COMPRESSED_DATA_LENGTH + ConstLength.MAX_FIRST_COMPRESSED_DATA_LENGTH, followingUnCompressedData, 0, ConstLength.MAX_COMPRESSED_DATA_LENGTH);
                            byte[] followingCompressedData = pduBuilder.CompressDataToRdp8BulkEncodedData(followingUnCompressedData, PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_LITE | PACKET_COMPR_FLAG.PACKET_COMPRESSED);

                            DynamicVCPDU followingCompressedPDU = pduBuilder.CreateDataCompressedReqPdu(channelId, followingCompressedData);
                            Send(followingCompressedPDU, DynamicVC_TransportType.RDP_UDP_Reliable);
                        }
                        else //Last message
                        {
                            byte[] lastUnCompressedData = new byte[data.Length - i * ConstLength.MAX_COMPRESSED_DATA_LENGTH];
                            Array.Copy(data, i * ConstLength.MAX_COMPRESSED_DATA_LENGTH + ConstLength.MAX_FIRST_COMPRESSED_DATA_LENGTH, lastUnCompressedData, 0, followingLen - i * ConstLength.MAX_COMPRESSED_DATA_LENGTH);
                            byte[] lastCompressedData = pduBuilder.CompressDataToRdp8BulkEncodedData(lastUnCompressedData, PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_LITE | PACKET_COMPR_FLAG.PACKET_COMPRESSED);

                            DynamicVCPDU followingPdu = pduBuilder.CreateDataCompressedReqPdu(channelId, lastCompressedData);
                            Send(followingPdu, DynamicVC_TransportType.RDP_UDP_Reliable);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Close a DVC
        /// </summary>
        /// <param name="channelId">Channel Id</param>
        public void CloseChannel(UInt16 channelId)
        {
            if (!channelDicbyId.ContainsKey(channelId))
            {
                throw new InvalidOperationException("The channel has not been created, channel id:" + channelId);
            }
            DynamicVirtualChannel channel = channelDicbyId[channelId];

            // Send DVC close PDU
            this.SendDVCClosePDU(channelId, channel.TransportType);

            // Remove the channel from dictionary
            channelDicbyId.Remove(channelId);
        }

        /// <summary>
        /// Send a PDU using a specific transport
        /// </summary>
        /// <param name="pdu"></param>
        /// <param name="transportType"></param>
        public void Send(DynamicVCPDU pdu, DynamicVC_TransportType transportType)
        {
            if (!transportDic.ContainsKey(transportType))
            {
                throw new InvalidOperationException("Not create DVC transport:" + transportType);
            }
            transportDic[transportType].Send(pdu);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            //Try to close all opening Dynamic Virtual Channels before disconect SUT.
            if (channelDicbyId != null)
            {
                uint[] channelIds = new uint[channelDicbyId.Keys.Count];
                channelDicbyId.Keys.CopyTo(channelIds, 0);
                for (int i = 0; i < channelIds.Length; i++)
                {
                    try
                    {
                        //Will not expect DVC close response, since TD mentions the SUT may return DVC close response.
                        //[MS-RDPEDYC] Section 3.2.5.2: When a DVC client manager receives a DYNVC_CLOSE (section 2.2.4) PDU, the client MAY respond with a DYNVC_CLOSE (section 2.2.4) PDU specifying the ChannelId.
                        this.SendDVCClosePDU(channelIds[i], channelDicbyId[channelIds[i]].TransportType);
                    }
                    catch
                    {
                        //The RDP connection may be lost already, then the DVC close PDU cannot be received successfully.
                        //Ignore the DVC close reponse failure here and continue.
                    }
                }
            }

            foreach (DynamicVC_TransportType type in transportDic.Keys)
            {
                transportDic[type].Dispose();
            }
            channelDicbyId = null;
        }

        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Send DYNVC_SOFT_SYNC_REQUEST PDU.
        /// </summary>
        private void SendSoftSyncRequestPDU(SoftSyncReqFlags_Value flags, ushort numberOfTunnels = 0, SoftSyncChannelList[] channelList = null, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            SoftSyncReqDvcPDU pdu = pduBuilder.CreateSoftSyncReqPdu(flags, numberOfTunnels, channelList);
            this.Send(pdu, transportType);
        }

        /// <summary>
        /// Expect a DYNVC_SOFT_SYNC_RESPONSE PDU.
        /// </summary>
        private SoftSyncResDvcPdu ExpectSoftSyncResponsePDU(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            DateTime endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                if (unprocessedDVCPacketBuffer.Count > 0)
                {
                    lock (unprocessedDVCPacketBuffer)
                    {
                        for (int i = 0; i < unprocessedDVCPacketBuffer.Count; i++)
                        {
                            if (transportType == unprocessedDVCPacketBuffer[i].TransportType
                                && unprocessedDVCPacketBuffer[i].PDU is SoftSyncResDvcPdu)
                            {
                                SoftSyncResDvcPdu capResp = unprocessedDVCPacketBuffer[i].PDU as SoftSyncResDvcPdu;
                                unprocessedDVCPacketBuffer.RemoveAt(i);
                                return capResp;
                            }
                        }
                    }
                }

                Thread.Sleep(this.waitInterval);
            }
            return null;
        }

        /// <summary>
        /// Send DVC Capabilities Request PDU
        /// </summary>
        /// <param name="version">Version of Cap Request</param>
        /// <param name="transportType">transport Type</param>
        private void SendDVCCapRequestPDU(DYNVC_CAPS_Version version,
            DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            DynamicVCPDU capReq = null;
            if (version == DYNVC_CAPS_Version.VERSION1)
            {
                capReq = pduBuilder.CreateCapsV1ReqPdu();
            }
            else if (version == DYNVC_CAPS_Version.VERSION2)
            {
                capReq = pduBuilder.CreateCapsV2ReqPdu();
            }
            else
            {
                capReq = pduBuilder.CreateCapsV3ReqPdu();
            }

            this.Send(capReq, transportType);
        }

        /// <summary>
        /// Expect a DVC Capabilities Response PDU 
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="transportType">Transport type</param>
        /// <returns></returns>
        private CapsRespDvcPdu ExpectDVCCapResponsePDU(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            DateTime endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                if (unprocessedDVCPacketBuffer.Count > 0)
                {
                    lock (unprocessedDVCPacketBuffer)
                    {
                        for (int i = 0; i < unprocessedDVCPacketBuffer.Count; i++)
                        {
                            if (transportType == unprocessedDVCPacketBuffer[i].TransportType
                                && unprocessedDVCPacketBuffer[i].PDU is CapsRespDvcPdu)
                            {
                                CapsRespDvcPdu capResp = unprocessedDVCPacketBuffer[i].PDU as CapsRespDvcPdu;
                                unprocessedDVCPacketBuffer.RemoveAt(i);
                                return capResp;
                            }
                        }
                    }
                }

                Thread.Sleep(this.waitInterval);
            }
            return null;
        }

        /// <summary>
        /// Send a Create Request PDU
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="channelId">Channel ID</param>
        /// <param name="channelName">Channel Name</param>
        /// <param name="transportType">Transport type</param>
        private void SendDVCCreateRequestPDU(ushort priority, uint channelId, string channelName, DynamicVC_TransportType transportType)
        {
            CreateReqDvcPdu pdu = pduBuilder.CreateCreateReqDvcPdu(priority, channelId, channelName);
            this.Send(pdu, transportType);
        }

        /// <summary>
        /// Expect a DVC Create Response PDU 
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="channelId">Channel Id</param>
        /// <param name="transportType">Transport type</param>
        /// <returns></returns>
        private CreateRespDvcPdu ExpectDVCCreateResponsePDU(TimeSpan timeout, uint channelId, DynamicVC_TransportType transportType)
        {
            DateTime endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                if (unprocessedDVCPacketBuffer.Count > 0)
                {
                    lock (unprocessedDVCPacketBuffer)
                    {
                        for (int i = 0; i < unprocessedDVCPacketBuffer.Count; i++)
                        {
                            if (transportType == unprocessedDVCPacketBuffer[i].TransportType
                                && unprocessedDVCPacketBuffer[i].PDU is CreateRespDvcPdu
                                && (unprocessedDVCPacketBuffer[i].PDU as CreateRespDvcPdu).ChannelId == channelId)
                            {
                                CreateRespDvcPdu pdu = unprocessedDVCPacketBuffer[i].PDU as CreateRespDvcPdu;
                                unprocessedDVCPacketBuffer.RemoveAt(i);
                                return pdu;
                            }
                        }
                    }
                }

                Thread.Sleep(this.waitInterval);
            }
            return null;
        }

        /// <summary>
        /// Close a channel after received a Close DVC PDU
        /// </summary>
        /// <param name="pdu"></param>
        private void CloseChannel(CloseDvcPdu pdu)
        {
            if (!channelDicbyId.ContainsKey(pdu.ChannelId))
            {
                throw new InvalidOperationException("The channel has not been created, channel id:" + pdu.ChannelId);
            }
            DynamicVirtualChannel channel = channelDicbyId[pdu.ChannelId];

            channelDicbyId.Remove(channel.ChannelId);
            channel.IsActive = false;
        }

        /// <summary>
        /// Send DVC Close PDU
        /// </summary>
        /// <param name="channelId">Channel Id</param>
        /// <param name="transportType">Transport Type</param>
        private void SendDVCClosePDU(uint channelId, DynamicVC_TransportType transportType)
        {
            CloseDvcPdu pdu = pduBuilder.CreateCloseDvcPdu(channelId);
            this.Send(pdu, transportType);
        }

        /// <summary>
        /// Expect a DVC Close PDU
        /// </summary>
        /// <param name="timeout">Time out</param>
        /// <param name="channelId">Channel ID</param>
        /// <param name="transportType">Transport Type</param>
        /// <returns></returns>
        private CloseDvcPdu ExpectDVCClosePDU(TimeSpan timeout, uint channelId, DynamicVC_TransportType transportType)
        {
            DateTime endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                if (unprocessedDVCPacketBuffer.Count > 0)
                {
                    lock (unprocessedDVCPacketBuffer)
                    {
                        for (int i = 0; i < unprocessedDVCPacketBuffer.Count; i++)
                        {
                            if (transportType == unprocessedDVCPacketBuffer[i].TransportType
                                && unprocessedDVCPacketBuffer[i].PDU is CloseDvcPdu
                                && (unprocessedDVCPacketBuffer[i].PDU as CloseDvcPdu).ChannelId == channelId)
                            {
                                CloseDvcPdu pdu = unprocessedDVCPacketBuffer[i].PDU as CloseDvcPdu;
                                unprocessedDVCPacketBuffer.RemoveAt(i);
                                return pdu;
                            }
                        }
                    }
                }

                Thread.Sleep(this.waitInterval);
            }
            return null;
        }

        /// <summary>
        /// Process DVC packet, but don't process data packet
        /// Data packet will be processed by corresponding Dynamic virtual channel
        /// </summary>
        /// <param name="pdu">DVC packet</param>
        /// <param name="transportType">Transport type</param>
        private void ProcessPacket(DynamicVCPDU pdu, DynamicVC_TransportType transportType)
        {
            if (pdu is DataDvcBasePdu)
            {
                DataDvcBasePdu dataPdu = pdu as DataDvcBasePdu;
                if (channelDicbyId.ContainsKey(dataPdu.ChannelId))
                {
                    channelDicbyId[dataPdu.ChannelId].ProcessPacket(dataPdu);
                }
            }
            else
            {
                if (this.autoCloseChannel && (pdu is CloseDvcPdu))
                {
                    this.CloseChannel(pdu as CloseDvcPdu);
                    return;
                }

                lock (unprocessedDVCPacketBuffer)
                {
                    unprocessedDVCPacketBuffer.Add(new UnprocessedDVCPDUInfo(pdu, transportType));
                }
            }
        }

        /// <summary>
        /// Process Packet from TCP transport
        /// </summary>
        /// <param name="pdu"></param>
        private void ProcessPacketFromTCP(DynamicVCPDU pdu)
        {
            ProcessPacket(pdu, DynamicVC_TransportType.RDP_TCP);
        }

        /// <summary>
        /// Process Packet from reliable UDP transport
        /// </summary>
        /// <param name="pdu"></param>
        private void ProcessPacketFromUDPR(DynamicVCPDU pdu)
        {
            ProcessPacket(pdu, DynamicVC_TransportType.RDP_UDP_Reliable);
        }

        /// <summary>
        /// Process packet from lossy UDP transport
        /// </summary>
        /// <param name="pdu"></param>
        private void ProcessPacketFromUDPL(DynamicVCPDU pdu)
        {
            ProcessPacket(pdu, DynamicVC_TransportType.RDP_UDP_Lossy);
        }

        #endregion Private Methods
    }

    #region Data Packaging

    /// <summary>
    /// The RDP_SEGMENTED_DATA structure is used to wrap one or more RDP_DATA_SEGMENT (section 2.2.5.2) structures.
    /// Each segment contains data that has been encoded using RDP 8.0 Bulk Compression techniques (section 3.1.9.1).
    /// </summary>
    public class RDP_SEGMENTED_DATA : BasePDU
    {
        #region Message Fields

        /// <summary>
        /// An 8-bit unsigned integer that specifies whether the RDP_SEGMENTED_DATA structure wraps a single segment or multiple segments
        /// </summary>
        public DescriptorTypes descriptor;

        /// <summary>
        /// An optional 16-bit unsigned integer that specifies the number of elements in the segmentArray field.
        /// </summary>
        public ushort segmentCount;

        /// <summary>
        /// An optional 32-bit unsigned integer that specifies the size, in bytes, 
        /// of the data present in the segmentArray field once it has been reassembled and decompressed.
        /// </summary>
        public uint uncompressedSize;

        /// <summary>
        /// An optional variable-length RDP8_BULK_ENCODED_DATA structure (section 2.2.5.3).
        /// </summary>
        public RDP8_BULK_ENCODED_DATA bulkData;

        /// <summary>
        /// An optional variable-length array of RDP_DATA_SEGMENT structures.
        /// </summary>
        public RDP_DATA_SEGMENT[] segmentArray;

        #endregion

        #region Methods

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte((byte)descriptor);
            if (descriptor == DescriptorTypes.SINGLE)
            {
                if (bulkData != null)
                {
                    marshaler.WriteByte(bulkData.header);
                    marshaler.WriteBytes(bulkData.data);
                }
            }
            else
            {
                marshaler.WriteUInt16(segmentCount);
                marshaler.WriteUInt32(uncompressedSize);
                if (segmentArray != null && segmentArray.Length > 0)
                {
                    foreach (RDP_DATA_SEGMENT dataSeg in segmentArray)
                    {
                        marshaler.WriteUInt32(dataSeg.size);
                        if (dataSeg.bulkData != null)
                        {
                            marshaler.WriteByte(bulkData.header);
                            marshaler.WriteBytes(bulkData.data);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.descriptor = (DescriptorTypes)marshaler.ReadByte();

                if (this.descriptor == DescriptorTypes.SINGLE)
                {
                    this.bulkData = new RDP8_BULK_ENCODED_DATA();
                    this.bulkData.header = marshaler.ReadByte();
                    this.bulkData.data = marshaler.ReadToEnd();
                }
                else
                {
                    this.segmentCount = marshaler.ReadUInt16();
                    this.uncompressedSize = marshaler.ReadUInt32();
                    if (this.segmentCount > 0)
                    {
                        this.segmentArray = new RDP_DATA_SEGMENT[this.segmentCount];
                        for (int i = 0; i < this.segmentCount; i++)
                        {
                            this.segmentArray[i].size = marshaler.ReadUInt32();
                            if (this.segmentArray[i].size > 0)
                            {
                                this.segmentArray[i].bulkData = new RDP8_BULK_ENCODED_DATA();
                                this.segmentArray[i].bulkData.header = marshaler.ReadByte();
                                this.segmentArray[i].bulkData.data = marshaler.ReadBytes((int)this.segmentArray[i].size - 1);
                            }
                        }
                    }
                }

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        #endregion
    }

    /// <summary>
    /// The RDP_DATA_SEGMENT structure contains data that has been encoded using RDP 8.0 Bulk Compression techniques (section 3.1.9.1).
    /// </summary>
    public class RDP_DATA_SEGMENT
    {
        /// <summary>
        /// A 32-bit unsigned integer that specifies the size, in bytes, of the bulkData field
        /// </summary>
        public uint size;

        /// <summary>
        /// A variable-length RDP8_BULK_ENCODED_DATA structure (section 2.2.5.3).
        /// </summary>
        public RDP8_BULK_ENCODED_DATA bulkData;
    }

    /// <summary>
    /// The RDP8_BULK_ENCODED_DATA structure contains a header byte 
    /// and data that has been encoded using RDP 8.0 Bulk Compression techniques (section 3.1.9.1).
    /// </summary>
    public class RDP8_BULK_ENCODED_DATA
    {
        /// <summary>
        /// An 8-bit, unsigned integer that specifies the compression type and flags.
        /// </summary>
        public byte header;

        /// <summary>
        /// ): A variable-length array of bytes that contains data encoded using RDP 8.0 Bulk Compression techniques. 
        /// If the PACKET_COMPRESSED (0x20) flag is specified in the header field, then the data is compressed.
        /// </summary>
        public byte[] data;
    }


    #endregion

    /// <summary>
    /// The RDP Sever byte stream Must be encapsulated as RdpSegmentPdu with/without compression.
    /// </summary>
    public class RdpSegmentedPdu
    {
        #region Variables
        public RDP_SEGMENTED_DATA segHeader;
        public byte compressFlag;
        public byte segPduCompressedFlagSettings;

        public List<RDP_SEGMENTED_DATA> segHeadList;  // List of segmented server data, excluding segment header.
        public List<byte[]> segPduList;   // List of segment PDU, include segment header.        
        public uint segmentPartSize;  // The pure data size in a single RDP8_BULK_ENCODED_DATA structure
        #endregion variables

        /// <summary>
        /// Contructor. 
        /// </summary>
        /// <param name="descType"> Indicates if a single or multipart segment PDU.</param>
        /// <param name="compFlag">Indicates the data is compressed and the compress type.</param>
        public RdpSegmentedPdu(byte compFlag, uint segSize)
        {
            compressFlag = compFlag;
            segmentPartSize = segSize;

            segHeadList = new List<RDP_SEGMENTED_DATA>();
            segPduList = new List<byte[]>();

            segHeader = new RDP_SEGMENTED_DATA();
        }

        public void ClearSegments()
        {
            segHeadList.Clear();
            segPduList.Clear();

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
            ClearSegments();
        }

        public void SegmentAndCompressFrame(byte[] rawSvrData, byte compressFlag, uint segmentPartSize)
        {

            // Set descriptor type based on data length
            if (rawSvrData.Length <= segmentPartSize)
            {
                segHeader.descriptor = DescriptorTypes.SINGLE;
                segHeader.bulkData = new RDP8_BULK_ENCODED_DATA();

                segHeader.bulkData.header = compressFlag;

                // RDP 8.0 compression here. 
                if (compressFlag == ((byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8 | (byte)PACKET_COMPR_FLAG.PACKET_COMPRESSED))
                {
                    CompressFactory cpf = new CompressFactory();
                    byte[] compressedData = cpf.Compress(rawSvrData);
                    segHeader.bulkData.data = compressedData;

                    // ETW Provider Dump message
                    string messageName = "DecompressedData";
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, "Decompressed data", rawSvrData);
                }
                else
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
                    if (compressFlag == ((byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8 | (byte)PACKET_COMPR_FLAG.PACKET_COMPRESSED))
                    {
                        CompressFactory cpf = new CompressFactory();
                        byte[] compressData = cpf.Compress(rawPartData);
                        segHeader.segmentArray[cnt].bulkData = new RDP8_BULK_ENCODED_DATA();
                        segHeader.segmentArray[cnt].bulkData.header = compressFlag;
                        segHeader.segmentArray[cnt].bulkData.data = compressData;
                        segHeader.segmentArray[cnt].size = (uint)(segHeader.segmentArray[cnt].bulkData.data.Length + 1);

                        // ETW Provider Dump message
                        string messageName = "DecompressedData";
                        ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, "Decompressed data", rawPartData);
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
                    segHead.bulkData.header = (byte)PACKET_COMPR_FLAG.PACKET_COMPR_TYPE_RDP8 | (byte)PACKET_COMPR_FLAG.PACKET_COMPRESSED;
                    dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(segHead.bulkData.header));
                    dataBuffer.AddRange(segHead.bulkData.data);
                }
                else
                {
                    for (int i = 0; i < segHeader.segmentCount; ++i)
                    {
                        dataBuffer.AddRange(BitConverter.GetBytes(segHead.segmentArray[i].size));   // Increase the size by 1 for the header of the RDP8_BULK_ENCODED_DATA structure.
                        dataBuffer.AddRange(TypeMarshal.ToBytes<byte>(segHead.segmentArray[i].bulkData.header));
                        dataBuffer.AddRange(segHead.segmentArray[i].bulkData.data);
                    }
                }

                segPduList.Add(dataBuffer.ToArray());
            }

            return segPduList;
        }
    }

}
