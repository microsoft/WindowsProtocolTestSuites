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

        private PduBuilder pduBuilder;

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
            if(transportDic.ContainsKey(transportType))
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
        public void ExchangeCapabilities(TimeSpan timeout, DYNVC_CAPS_Version version = DYNVC_CAPS_Version.VERSION3)
        {
            this.SendDVCCapRequestPDU(version);
            CapsRespDvcPdu capResp = this.ExpectDVCCapResponsePDU(timeout);
            if (capResp == null)
            {
                throw new System.IO.IOException("Cannot receive a DVC Capabilities Response PDU!");
            }
        }

        /// <summary>
        /// Create a dynamic virtual channel 
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <param name="channelName">Channel name</param>
        /// <param name="transportType">Transport type</param>
        /// <param name="receiveCallBack">Callback method called when received data</param>
        /// <returns>DVC created</returns>
        public DynamicVirtualChannel CreateChannel(TimeSpan timeout, ushort priority, string channelName, DynamicVC_TransportType transportType, ReceiveData receiveCallBack = null)
        {
            if(!transportDic.ContainsKey(transportType))
            {
                throw new InvalidOperationException("Not create DVC transport:" + transportType);
            }

            UInt32 channelId = DynamicVirtualChannel.NewChannelId();
            DynamicVirtualChannel channel = new DynamicVirtualChannel(channelId, channelName, priority, transportDic[transportType]);
            
            if (receiveCallBack != null)
            {
                // Add event method here can make sure processing the first DVC data packet
                channel.Received += receiveCallBack;
            }

            channelDicbyId.Add(channelId, channel);

            this.SendDVCCreateRequestPDU(priority, channelId, channelName, transportType);
            CreateRespDvcPdu createResp = this.ExpectDVCCreateResponsePDU(timeout, channelId, transportType);
            if (createResp == null)
            {
                throw new System.IO.IOException("Creation of channel: " + channelName +" failed, cannot receive a Create Response PDU");
            }
            if (createResp.CreationStatus < 0)
            {
                //Create failed
                throw new System.IO.IOException("Creation of DVC failed with error code: " + createResp.CreationStatus +", channel name is " + channelName);
            }

            return channel;
        }

        /// <summary>
        /// Close a DVC
        /// </summary>
        /// <param name="channelId">Channel Id</param>
        public void CloseChannel(UInt16 channelId)
        {
            if(!channelDicbyId.ContainsKey(channelId))
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
                        if (unprocessedDVCPacketBuffer.Count > 0)
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
                        if (unprocessedDVCPacketBuffer.Count > 0)
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
}
