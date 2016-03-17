// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    public class RdpedycClient : IDisposable
    {
        #region Variables

        private TimeSpan waitInterval = new TimeSpan(0, 0, 0, 0, 100);

        private RdpbcgrClient rdpbcgrClient = null;
        private RdpbcgrClientContext clientSessionContext = null;

        private Dictionary<DynamicVC_TransportType, IDVCTransport> transportDic;

        private List<UnprocessedDVCPDUInfo> unprocessedDVCPacketBuffer;

        private Dictionary<UInt32, DynamicVirtualChannel> channelDicbyId;

        private PduBuilder pduBuilder;

        private bool autoCreateChannel;
        private Dictionary<string, ReceiveData> callBackMethodsDic;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="context"></param>
        /// <param name="autoCreateChannel"></param>
        /// <param name="callBackMethodsDic"></param>
        public RdpedycClient(RdpbcgrClient client, RdpbcgrClientContext context, bool autoCreateChannel = true, Dictionary<string, ReceiveData> callBackMethodsDic = null)
        {
            this.rdpbcgrClient = client;
            this.clientSessionContext = context;
            transportDic = new Dictionary<DynamicVC_TransportType, IDVCTransport>();
            unprocessedDVCPacketBuffer = new List<UnprocessedDVCPDUInfo>();
            Rdpbcgr_DVCClientTransport transport = new Rdpbcgr_DVCClientTransport(context);

            channelDicbyId = new Dictionary<uint, DynamicVirtualChannel>();

            this.autoCreateChannel = autoCreateChannel;
            this.callBackMethodsDic = callBackMethodsDic;

            pduBuilder = new PduBuilder();

            transport.Received += ProcessPacketFromTCP;
            transportDic.Add(DynamicVC_TransportType.RDP_TCP, transport);
            
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

            Rdpemt_DVCClientTransport transport = new Rdpemt_DVCClientTransport(clientSessionContext, transportType);

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
        /// Exchange DVC capability, negotiate the version 
        /// </summary>
        /// <param name="version"></param>
        public void ExchangeCapabilities(TimeSpan timeout)
        {
            DynamicVCPDU pdu = this.ExpectDVCCapRequestPDU(timeout);
            if (pdu == null)
            {
                throw new System.IO.IOException("Cannot receive a DVC Capabilities Request PDU!");
            }
            DYNVC_CAPS_Version version = DYNVC_CAPS_Version.VERSION3;
            if(pdu is CapsVer1ReqDvcPdu)
            {
                version = DYNVC_CAPS_Version.VERSION1;
            }
            else if(pdu is CapsVer2ReqDvcPdu)
            {
                version = DYNVC_CAPS_Version.VERSION2;
            }

            this.SendDVCCapResponsePDU(version);
            
        }
        
        /// <summary>
        /// Expect to create a SVC
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="channelName">Channel Name</param>
        /// <param name="transportType">Transport Type</param>
        /// <returns></returns>
        public DynamicVirtualChannel ExpectChannel(TimeSpan timeout, string channelName, DynamicVC_TransportType transportType, ReceiveData receiveCallBack = null)
        {
            if (autoCreateChannel)
            {
                throw new InvalidOperationException("Cannot establish a DVC manually if autoCreateChannel is true!");
            }

            if (!transportDic.ContainsKey(transportType))
            {
                throw new InvalidOperationException("Not create DVC transport:" + transportType);
            }
            

            CreateReqDvcPdu createReq = this.ExpectDVCCreateRequestPDU(timeout, channelName, transportType);
            if (createReq == null)
            {
                throw new System.IO.IOException("Creation of channel: " + channelName + " failed, cannot receive a Create Request PDU");
            }

            DynamicVirtualChannel channel = new DynamicVirtualChannel(createReq.ChannelId, channelName, (ushort)createReq.HeaderBits.Sp, transportDic[transportType]);
            if (receiveCallBack != null)
            {
                // Add event method here can make sure processing the first DVC data packet
                channel.Received += receiveCallBack;
            }
            else
            {
                if (callBackMethodsDic != null && callBackMethodsDic.ContainsKey(channelName))
                {
                    channel.Received += callBackMethodsDic[channelName];
                }
            }
            
            channelDicbyId.Add(createReq.ChannelId, channel);

            this.SendDVCCreateResponsePDU(createReq.ChannelId, 0, transportType);

            return channel;

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
            foreach (DynamicVC_TransportType type in transportDic.Keys)
            {
                transportDic[type].Dispose();
            }
            channelDicbyId = null;
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Expect a DVC Capabilities Request PDU
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="transportType">Transport type</param>
        /// <returns></returns>
        private DynamicVCPDU ExpectDVCCapRequestPDU(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            DateTime endTime = DateTime.Now + timeout;
            while (DateTime.Now < endTime)
            {
                if (unprocessedDVCPacketBuffer.Count > 0)
                {
                    lock (unprocessedDVCPacketBuffer)
                    {
                        if (unprocessedDVCPacketBuffer.Count > 0)
                        {
                            for (int i = 0; i < unprocessedDVCPacketBuffer.Count; i++)
                            {
                                if (transportType == unprocessedDVCPacketBuffer[i].TransportType
                                    && (unprocessedDVCPacketBuffer[i].PDU is CapsVer1ReqDvcPdu
                                        || unprocessedDVCPacketBuffer[i].PDU is CapsVer2ReqDvcPdu
                                        || unprocessedDVCPacketBuffer[i].PDU is CapsVer3ReqDvcPdu))
                                {
                                    DynamicVCPDU capResp = unprocessedDVCPacketBuffer[i].PDU;
                                    unprocessedDVCPacketBuffer.RemoveAt(i);
                                    return capResp;
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(this.waitInterval);
            }
            return null;
        }

        /// <summary>
        /// Send a DVC Capabilities Response PDU
        /// </summary>
        /// <param name="version">Version</param>
        /// <param name="transportType">Transport Type</param>
        private void SendDVCCapResponsePDU(DYNVC_CAPS_Version version, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            CapsRespDvcPdu capResp = pduBuilder.CreateCapsRespPdu((ushort)version);
            this.Send(capResp, transportType);
        }

        /// <summary>
        /// Expect a Create Request PDU with specific channel name and using specific transport
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="channelName">Channel name</param>
        /// <param name="transportType">Transport type</param>
        /// <returns></returns>
        private CreateReqDvcPdu ExpectDVCCreateRequestPDU(TimeSpan timeout, string channelName, DynamicVC_TransportType transportType)
        {
            {
                DateTime endTime = DateTime.Now + timeout;
                while (DateTime.Now < endTime)
                {
                    if (unprocessedDVCPacketBuffer.Count > 0)
                    {
                        lock (unprocessedDVCPacketBuffer)
                        {
                            if (unprocessedDVCPacketBuffer.Count > 0)
                            {
                                for (int i = 0; i < unprocessedDVCPacketBuffer.Count; i++)
                                {
                                    if (transportType == unprocessedDVCPacketBuffer[i].TransportType
                                        && unprocessedDVCPacketBuffer[i].PDU is CreateReqDvcPdu
                                        && (unprocessedDVCPacketBuffer[i].PDU as CreateReqDvcPdu).ChannelName == channelName)
                                    {
                                        CreateReqDvcPdu pdu = unprocessedDVCPacketBuffer[i].PDU as CreateReqDvcPdu;
                                        unprocessedDVCPacketBuffer.RemoveAt(i);
                                        return pdu;
                                    }
                                }
                            }
                        }
                    }

                    Thread.Sleep(this.waitInterval);
                }
                return null;
            }
        }

        /// <summary>
        /// Expect a Create Request PDU
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="transportType">out parameter: indicate which transport received this packet</param>
        /// <returns></returns>
        private CreateReqDvcPdu ExpectDVCCreateRequestPDU(TimeSpan timeout, out DynamicVC_TransportType transportType)
        {
            transportType = DynamicVC_TransportType.RDP_TCP;

            {
                DateTime endTime = DateTime.Now + timeout;
                while (DateTime.Now < endTime)
                {
                    if (unprocessedDVCPacketBuffer.Count > 0)
                    {
                        lock (unprocessedDVCPacketBuffer)
                        {
                            if (unprocessedDVCPacketBuffer.Count > 0)
                            {
                                for (int i = 0; i < unprocessedDVCPacketBuffer.Count; i++)
                                {
                                    if (unprocessedDVCPacketBuffer[i].PDU is CreateReqDvcPdu)
                                    {
                                        CreateReqDvcPdu pdu = unprocessedDVCPacketBuffer[i].PDU as CreateReqDvcPdu;
                                        transportType = unprocessedDVCPacketBuffer[i].TransportType;
                                        unprocessedDVCPacketBuffer.RemoveAt(i);
                                        return pdu;
                                    }
                                }
                            }
                        }
                    }

                    Thread.Sleep(this.waitInterval);
                }
                return null;
            }
        }

        /// <summary>
        /// Send a DVC Create Response PDU
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="creationStatus"></param>
        /// <param name="transportType"></param>
        private void SendDVCCreateResponsePDU(uint channelId, int creationStatus, DynamicVC_TransportType transportType)
        {
            CreateRespDvcPdu createResp = pduBuilder.CreateCreateRespDvcPdu(channelId, creationStatus);
            this.Send(createResp, transportType);

        }

        /// <summary>
        /// Establish a DVC after received a Create Request PDU
        /// </summary>
        /// <param name="createReq"></param>
        /// <param name="transportType"></param>
        /// <param name="receiveCallBack"></param>
        private void EstablishChannel(CreateReqDvcPdu createReq, DynamicVC_TransportType transportType, ReceiveData receiveCallBack = null)
        {
            if (channelDicbyId.ContainsKey(createReq.ChannelId))
            {
                throw new InvalidOperationException("Cannot establish the DVC, since a channel with same channelId have been established. Channel ID is " + createReq.ChannelId);
            }

            DynamicVirtualChannel channel = new DynamicVirtualChannel(createReq.ChannelId, createReq.ChannelName, (ushort)createReq.HeaderBits.Sp, transportDic[transportType]);
            if (receiveCallBack != null)
            {
                // Add event method here can make sure processing the first DVC data packet
                channel.Received += receiveCallBack;
            }
            else
            {
                if (callBackMethodsDic != null && callBackMethodsDic.ContainsKey(createReq.ChannelName))
                {
                    channel.Received += callBackMethodsDic[createReq.ChannelName];
                }
            }

            channelDicbyId.Add(createReq.ChannelId, channel);

            this.SendDVCCreateResponsePDU(createReq.ChannelId, 0, transportType);

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
                        if (unprocessedDVCPacketBuffer.Count > 0)
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
                if (autoCreateChannel)
                {
                    if (pdu is CreateReqDvcPdu)
                    {
                        this.EstablishChannel(pdu as CreateReqDvcPdu, transportType);
                        return;
                    }
                    else if (pdu is CloseDvcPdu)
                    {
                        this.CloseChannel(pdu as CloseDvcPdu);
                        return;
                    }
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
}
