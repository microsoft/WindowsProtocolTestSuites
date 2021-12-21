// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
// using Microsoft.Protocols.TestTools.ExtendedLogging;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{

    #region Type Definitions

    /// <summary>
    /// Indicate the transport mode of RDPEUDP.
    /// </summary>
    public enum TransportMode
    {
        Reliable,
        Lossy
    }

    /// <summary>
    /// Indicate the state of sending source packet.
    /// </summary>
    public class OutPacketState
    {
        public OutPacketState()
        {
            RetransmitTimes = 0;
            Acknowledged = false;
            EstimatedRTT = false;
        }

        /// <summary>
        /// Send Time.
        /// </summary>
        public DateTime SendTime;

        /// <summary>
        /// Indicate if this packet acknowledged by remote endpoint.
        /// </summary>
        public bool Acknowledged;

        /// <summary>
        /// The sending packet.
        /// </summary>
        public RdpeudpPacket Packet;

        /// <summary>
        /// Times that this packet have been retransmitted.
        /// </summary>
        public int RetransmitTimes;

        /// <summary>
        /// Whether this packet has been used to estimate RTT
        /// one packet only can be used to estimate RTT once
        /// </summary>
        public bool EstimatedRTT;
    }

    /// <summary>
    /// Indicate the state of received source packet.
    /// </summary>
    public class InPacketState
    {
        /// <summary>
        /// The received packet.
        /// </summary>
        public RdpeudpPacket Packet;
    }

    #endregion

    #region Delegates definitions

    /// <summary>
    /// Delegation to receive packets from the specified remote endpoint.
    /// </summary>
    /// <param name="remoteEP">The specified remote endpoint.</param>
    /// <param name="packet">Packet need to sent.</param>
    public delegate void RdpeudpSocketSender(IPEndPoint remoteEP, StackPacket packet);

    /// <summary>
    /// Delegation to invoke the receive data method of higher layer protocols.
    /// </summary>
    /// <param name="data">The data should be received by higher layer protocols.</param>
    public delegate void ReceiveData(byte[] data);

    /// <summary>
    /// The delegate used to handle disconnection.
    /// </summary>
    public delegate void DisconnectedHandler();

    #endregion

    public partial class RdpeudpSocket : IDisposable
    {
        #region Private variables

        #region Locks

        // Used to make sure only one thread call this.ReceivePacket
        protected readonly object receiveLock = new object();

        // Lock to protect CurSnCoded and CurSnSource
        private readonly object sequenceNumberLock = new object();

        // Block send method if the new source packet is not in send window
        private AutoResetEvent sendWindowLock = new AutoResetEvent(false);

        // Lock to update OutSnResetSeqNum
        private readonly object updateOutSnResetSeqNumLock = new object();

        #endregion

        // Method used to send packet
        private RdpeudpSocketSender packetSender;

        protected TimeSpan RTT;

        protected List<RdpeudpPacket> unprocessedPacketBuffer = new List<RdpeudpPacket>();

        // Cache for sending source packets, only for reliable connection, packet will be cached after sent but not acknowledged received
        private Dictionary<uint, OutPacketState> outPacketDic = new Dictionary<uint, OutPacketState>();
        // Cache for received source packets.
        private Dictionary<uint, InPacketState> inPacketDic = new Dictionary<uint, InPacketState>();

        // Variables used to update OutSnResetSeqNum
        // Sequence number with which the packet send an AckOfAckVector
        private uint seqNumOfPacketWithAckOfAckVector;
        // new OutSnResetSeqNum
        private uint? newOutSnResetSeqNum = null;

        #region Timers

        // Timer used to manage retransmit
        private Timer retransmitTimer;

        // Timer used to manage keep alive
        private Timer keepaliveTimer;

        // Timer used to manage delayed ack
        private Timer delayedAckTimer;

        #endregion

        // Below two variables are used for keep alive timer
        private DateTime latestDatagramSentAt;
        private DateTime latestDatagramReceivedAt;

        // Number of received packets not ack yet, this value only be changed in Lock(inPacketDic) block so as to sync
        private int sourceNumNotAcked = 0;
        private DateTime receiveTimeForFirstNotACKSource;

        // public const DumpLevel DumpLevel_LayerTLS = (DumpLevel)10;

        #endregion Private variables

        #region Properties

        private bool connected;

        /// <summary>
        /// Whether the socket is connected with remote endpoint
        /// </summary>
        public bool Connected
        {
            get
            {
                return connected;
            }
            set
            {
                if (connected != value)
                {
                    connected = value;
                    if (connected && autoHandle && (!upgradedToRdpedup2))
                    {
                        // Timer will start when connect established
                        this.InitTimers();
                    }
                }
            }
        }

        private IPEndPoint remoteEndPoint;

        /// <summary>
        /// Identity of Remote EndPoint
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return remoteEndPoint;
            }
        }

        private bool autoHandle;

        /// <summary>
        /// Whether the RDPEUDP Socket is autoHandle
        /// </summary>
        public bool AutoHandle
        {
            get
            {
                return autoHandle;
            }
            set
            {
                if (autoHandle != value)
                {
                    autoHandle = value;
                    if (autoHandle)
                    {
                        if (upgradedToRdpedup2)
                        {
                            rdpeudp2Handler.AutoHandle = true;
                            return;
                        }

                        if (connected)
                        {
                            InitTimers();
                        }
                    }
                    else
                    {
                        if (upgradedToRdpedup2)
                        {
                            rdpeudp2Handler.AutoHandle = false;
                            return;
                        }

                        // If autoHandle changed from true to false, stop timer
                        DisposeTimers();
                    }
                }
            }
        }

        public uint SnInitialSequenceNumber { get; set; }

        public RDPUDP_PROTOCOL_VERSION HighestVersion { get; set; }

        // Used when send packet, current sequence number of Code Packet
        public uint CurSnCoded { get; set; }
        // Used when send packet, current sequence number of Source packet
        public uint CurSnSource { get; set; }

        // Used when receive packet, start Position of Receive window, all datagrams with a smaller sequence number have been received
        // packet not fall in the receive window will be ignored
        public uint ReceiveWindowStartPosition { get; set; }

        // Used when send packet, start Position of send window, all datagrams with a smaller sequence number have been sent and acknowleged received or lossy (same as CumAcked)
        // packet not fall in the send window will not be sent until until it fall in the window 
        public uint SendWindowStartPosition { get; set; }

        // The maximum size for a datagram that can be generated by the endpoint
        public ushort UUpStreamMtu { get; set; }
        // The maximum size of the maximum transmission unit (MTU) that the endpoint can accept
        public ushort UDownStreamMtu { get; set; }

        // used for send packet, Value used for SnSourceAck in RDPUDP_FEC_HEADER Structure 
        // this value is the highest sequence number of source data received
        public uint SnSourceAck { get; set; }
        // Start position of ACK vector to sent, which is used to create Ack vector
        public uint InSnResetSeqNum { get; set; }
        // Start position of ACK vector received, which is used to analyze received Ack vector
        public uint OutSnResetSeqNum { get; set; }

        // Window size that Remote endpoint advised
        public ushort URemoteAdvisedWindowSize { get; set; }
        // Send Window Size, which is the also the congestion window size, 
        // There is space in the receiver-advertised window for this datagram and the Congestion Control logic permits transmission of a datagram.
        public ushort USendWindowSize { get; set; }

        // Receive windows Size
        public ushort UReceiveWindowSize { get; set; }

        // Transport Mode: Reliable or Lossy
        public TransportMode TransMode { get; set; }

        // Socket config for RDPEUDP transport, some of the values will be retained when transferring to RDPEUDP2 transport 
        public RdpeudpSocketConfig SocketConfig { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mode">The mode which indicate this connection if reliable or lossy.</param>
        /// <param name="remoteEp">The remote endpoint.</param>
        /// <param name="autoHandle">Decide if receiver will auto handle incoming packets.</param>
        /// <param name="sender">Sender Used to send packet</param>
        public RdpeudpSocket(TransportMode mode, IPEndPoint remoteEp, bool autoHandle, RdpeudpSocketSender sender)
        {
            this.TransMode = mode;
            this.SocketConfig = new RdpeudpSocketConfig();
            this.AutoHandle = autoHandle;
            this.connected = false;
            remoteEndPoint = remoteEp;

            // Initial highestAckNumber as 0 because every hihestAckNumber will compare with Sequence Number of coming packet, and set as the bigger one.
            SnSourceAck = 0;

            URemoteAdvisedWindowSize = SocketConfig.InitialWindowSize;
            USendWindowSize = SocketConfig.InitialWindowSize;
            UReceiveWindowSize = SocketConfig.InitialWindowSize;
            UUpStreamMtu = SocketConfig.InitialStreamMtu;
            UDownStreamMtu = SocketConfig.InitialStreamMtu;

            OutSnResetSeqNum = SocketConfig.InitialAckPosition;
            InSnResetSeqNum = SocketConfig.InitialAckPosition;

            ReceiveWindowStartPosition = SocketConfig.InitialAckPosition;
            RTT = new TimeSpan(0, 0, 0, 0, this.SocketConfig.DelayedAckTime);

            packetSender = sender;
        }

        #endregion

        #region Methods for Auto-handle Socket

        /// <summary>
        /// Send Data from this specified UDP transport
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <returns>Return true if send success</returns>
        public bool Send(byte[] data)
        {
            if (!Connected)
            {
                return false;
            }

            if (upgradedToRdpedup2)
            {
                return rdpeudp2Handler.Send(data);
            }

            List<byte> dataList = new List<byte>(data);
            byte[] packetData;
            RdpeudpPacket packet;
            do
            {
                packet = new RdpeudpPacket(); // Fill in the common header.
                packet.FecHeader.snSourceAck = SnSourceAck;
                packet.FecHeader.uReceiveWindowSize = UReceiveWindowSize;
                packet.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_DATA | RDPUDP_FLAG.RDPUDP_FLAG_ACK;
                packet.AckVectorHeader = CreateAckVectorHeader();

                packet.SourceHeader = CreateSourcePayloadHeader(); // Generate SourceHeader.

                packetData = PduMarshaler.Marshal(packet, false); // Measure the header lenght to figure out payload length.
                int payloadLength = Math.Min(UUpStreamMtu - packetData.Length, dataList.Count);

                packet.Payload = new byte[payloadLength];
                dataList.CopyTo(0, packet.Payload, 0, payloadLength); // Copy the data in to packet payload.

                SendPacket(packet);
                dataList.RemoveRange(0, payloadLength);

            } while (dataList.Count > 0);

            return true;
        }

        /// <summary>
        /// An Event will be called when receive Data bytes from this specified UDP transport
        /// </summary>
        public event ReceiveData Received;

        /// <summary>
        /// An event triggered when the connection is closed.
        /// </summary>
        public event DisconnectedHandler Disconnected;

        #endregion

        #region Methods used only when AutoHandle is false

        /// <summary>
        /// Send a RDPEUDP Packet
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public bool SendPacket(RdpeudpPacket packet)
        {
            if (packet.SourceHeader.HasValue && AutoHandle)
            {
                // Deal with window area.
                OutPacketState packetState = new OutPacketState();
                packetState.Packet = packet;
                packetState.Acknowledged = false;

                DateTime endTime = DateTime.Now + this.SocketConfig.Timeout;
                uint sendWindowEndPos = SendWindowStartPosition + USendWindowSize;

                while (DateTime.Now < endTime)
                {
                    //If source sequence number is in send slide window, send the packet. Otherwise, wait a sendingInterval
                    if (!IsInSendWindow(packet.SourceHeader.Value.snSourceStart))
                    {
                        sendWindowLock.WaitOne(this.SocketConfig.SendingInterval);
                    }
                    else
                    {
                        break;
                    }
                }

                if (DateTime.Now > endTime)
                {
                    // Time out.
                    return false;
                }

                packetState.SendTime = DateTime.Now;
                lock (outPacketDic)
                {
                    outPacketDic[packet.SourceHeader.Value.snSourceStart] = packetState;
                }

                // Add RDPUDP_ACK_OF_ACKVECTOR_HEADER Structure if necessary
                UpdateOutSnResetSeqNum(packet);
            }

            byte[] data = PduMarshaler.Marshal(packet, false);
            SendBytesByUdp(data);

            // Update Last send time, which is used for keep alive timer
            latestDatagramSentAt = DateTime.Now;

            return true;
        }

        /// <summary>
        /// Send bytes via UDP transport
        /// </summary>
        /// <param name="data"></param>
        public void SendBytesByUdp(byte[] data)
        {
            StackPacket stackPacket = new RdpeudpBasePacket(data);
            packetSender(remoteEndPoint, stackPacket);

            // ETW Provider Dump Message
            // string messageName = "RDPEUDP:SentPDU";
            // ExtendedLogger.DumpMessage(messageName, DumpLevel_LayerTLS, typeof(RdpeudpPacket).Name, data);
        }

        public RdpeudpPacket ExpectPacket(TimeSpan timeout)
        {
            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                lock (unprocessedPacketBuffer)
                {
                    if (unprocessedPacketBuffer.Count > 0)
                    {
                        RdpeudpPacket eudpPacket = unprocessedPacketBuffer[0];
                        unprocessedPacketBuffer.RemoveAt(0);
                        return eudpPacket;
                    }
                }
                // If not receive a Packet, wait a while 
                Thread.Sleep(RdpeudpSocketConfig.ReceivingInterval);
            }
            return null;
        }

        /// <summary>
        /// Expect an ACK packet
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RdpeudpPacket ExpectAckPacket(TimeSpan timeout)
        {
            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                lock (unprocessedPacketBuffer)
                {
                    for (int i = 0; i < unprocessedPacketBuffer.Count; i++)
                    {
                        RdpeudpPacket eudpPacket = unprocessedPacketBuffer[i];
                        if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK))
                        {
                            if (!Connected || (eudpPacket.AckVectorHeader.HasValue && eudpPacket.AckVectorHeader.Value.uAckVectorSize > 0))
                            {
                                unprocessedPacketBuffer.RemoveAt(i);
                                return eudpPacket;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Create Source Packet from byte data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public RdpeudpPacket CreateSourcePacket(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            RdpeudpPacket packet = new RdpeudpPacket(); // Fill in the common header.
            packet.FecHeader.snSourceAck = SnSourceAck;
            packet.FecHeader.uReceiveWindowSize = UReceiveWindowSize;
            packet.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_DATA | RDPUDP_FLAG.RDPUDP_FLAG_ACK;
            packet.AckVectorHeader = CreateAckVectorHeader();

            packet.SourceHeader = CreateSourcePayloadHeader();
            packet.Payload = data;
            return packet;
        }

        /// <summary>
        /// Create RDPUDP_ACK_VECTOR_HEADER Structure for packet sending
        /// </summary>
        /// <returns></returns>
        public RDPUDP_ACK_VECTOR_HEADER CreateAckVectorHeader()
        {
            RDPUDP_ACK_VECTOR_HEADER ackVectorHeader = new RDPUDP_ACK_VECTOR_HEADER();
            if (inPacketDic.Count == 0)
            {
                // Not receive any source packet
                ackVectorHeader.uAckVectorSize = 0;
                ackVectorHeader.AckVector = null;
                ackVectorHeader.Padding = null;
            }
            else
            {
                lock (inPacketDic)
                {
                    // Generate an ACK header.
                    List<AckVectorElement> ackVectorElements = new List<AckVectorElement>();
                    VECTOR_ELEMENT_STATE currentState = inPacketDic.ContainsKey(InSnResetSeqNum) ? VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED : VECTOR_ELEMENT_STATE.DATAGRAM_NOT_YET_RECEIVED;
                    AckVectorElement ackVectorElement = new AckVectorElement();
                    ackVectorElement.State = currentState;
                    ackVectorElement.Length = 0;

                    // RLE encoding.
                    for (uint i = InSnResetSeqNum + 1; i <= SnSourceAck; i++)
                    {
                        currentState = inPacketDic.ContainsKey(i) ? VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED : VECTOR_ELEMENT_STATE.DATAGRAM_NOT_YET_RECEIVED;

                        // 0x40 is the max length of state length, of which I believe sould be indicate from soket config or types config not hard code here.
                        if (currentState != ackVectorElement.State || ackVectorElement.Length >= 0x40)
                        {
                            // If current state differs from last state, assign a new ack vector.
                            ackVectorElements.Add(ackVectorElement);
                            ackVectorElement = new AckVectorElement();
                            ackVectorElement.State = currentState;
                            ackVectorElement.Length = 0;
                        }
                        else
                        {
                            // Else count++.
                            ackVectorElement.Length++;
                        }
                    }
                    ackVectorElements.Add(ackVectorElement);
                    ackVectorHeader.uAckVectorSize = (ushort)ackVectorElements.Count;
                    ackVectorHeader.AckVector = ackVectorElements.ToArray();

                    // ACK Vector created for all received packet, set number of received packets for ACK to 0
                    sourceNumNotAcked = 0;
                }
            }

            return ackVectorHeader;
        }

        /// <summary>
        /// Create a RDPUDP_SOURCE_PAYLOAD_HEADER Structure
        /// </summary>
        /// <returns></returns>
        public RDPUDP_SOURCE_PAYLOAD_HEADER CreateSourcePayloadHeader()
        {
            RDPUDP_SOURCE_PAYLOAD_HEADER sourceHeader = new RDPUDP_SOURCE_PAYLOAD_HEADER();

            lock (sequenceNumberLock)
            {
                sourceHeader.snCoded = ++CurSnCoded;
                sourceHeader.snSourceStart = ++CurSnSource;
            }

            return sourceHeader;
        }

        /// <summary>
        /// Create RDPUDP_FEC_PAYLOAD_HEADER Structure
        /// </summary>
        /// <param name="snSourceStart"></param>
        /// <param name="uSourceRange"></param>
        /// <param name="uFecIndex"></param>
        /// <returns></returns>
        public RDPUDP_FEC_PAYLOAD_HEADER CreateFECPayloadHeader(uint snSourceStart, byte uSourceRange, byte uFecIndex)
        {
            RDPUDP_FEC_PAYLOAD_HEADER fecPayloadHeader = new RDPUDP_FEC_PAYLOAD_HEADER();

            lock (sequenceNumberLock)
            {
                fecPayloadHeader.snCoded = ++CurSnCoded;
            }

            fecPayloadHeader.snSourceStart = snSourceStart;
            fecPayloadHeader.uRange = uSourceRange;
            fecPayloadHeader.uFecIndex = uFecIndex;
            fecPayloadHeader.uPadding = 0x00;

            return fecPayloadHeader;
        }

        /// <summary>
        /// Create RDPUDP_SYNDATA_PAYLOAD Structure
        /// </summary>
        /// <param name="initialSequenceNumber"></param>
        /// <returns></returns>
        public RDPUDP_SYNDATA_PAYLOAD CreateSynData(uint? initialSequenceNumber = null)
        {
            RDPUDP_SYNDATA_PAYLOAD synData = new RDPUDP_SYNDATA_PAYLOAD();
            if (initialSequenceNumber == null)
            {
                Random random = new Random();
                SnInitialSequenceNumber = (uint)random.Next();
            }
            else
            {
                SnInitialSequenceNumber = initialSequenceNumber.Value;
            }
            synData.snInitialSequenceNumber = SnInitialSequenceNumber;
            synData.uUpStreamMtu = UUpStreamMtu;
            synData.uDownStreamMtu = UDownStreamMtu;
            CurSnCoded = synData.snInitialSequenceNumber;
            CurSnSource = synData.snInitialSequenceNumber;
            SendWindowStartPosition = synData.snInitialSequenceNumber + 1;

            return synData;
        }

        /// <summary>
        /// Create RDPUDP_SYNDATAEX_PAYLOAD Structure
        /// </summary>
        /// <returns></returns>
        public RDPUDP_SYNDATAEX_PAYLOAD CreateSynExData(RDPUDP_PROTOCOL_VERSION version)
        {
            RDPUDP_SYNDATAEX_PAYLOAD synExData = new RDPUDP_SYNDATAEX_PAYLOAD();
            synExData.uSynExFlags = RDPUDP_VERSION_INFO.RDPUDP_VERSION_INFO_VALID;

            //The uUdpVer field MUST be set to the highest RDP-UDP protocol version supported by both endpoints
            if (version >= RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3)
            {
                synExData.uUdpVer = RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3;
            }
            else if (version >= RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_2)
            {
                synExData.uUdpVer = RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_2;
            }
            else
            {
                synExData.uUdpVer = RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_1;
            }

            return synExData;
        }

        public byte[] CreateFECPayload(RdpeudpPacket[] sourcePackets, out byte uFecIndex)
        {
            throw new NotImplementedException();
        }

        public byte[] FECRecover(RdpeudpPacket[] sourcePackets, byte uFecIndex, byte[] fecData, uint targetIndex)
        {
            throw new NotImplementedException();
        }

        public bool RetransmitPacket(RdpeudpPacket packet)
        {
            if (!connected || packet.SourceHeader == null)
            {
                return false;
            }

            if (!outPacketDic.ContainsKey(packet.SourceHeader.Value.snSourceStart))
            {
                return false;
            }

            if (outPacketDic[packet.SourceHeader.Value.snSourceStart].RetransmitTimes >= this.SocketConfig.RetransmitLimit)
            {
                //If a datagram has been retransmitted five times without a response, the sender terminates the connection
                this.Close();
            }
            RDPUDP_SOURCE_PAYLOAD_HEADER sourceHeader = packet.SourceHeader.Value;
            lock (sequenceNumberLock)
            {
                sourceHeader.snCoded = ++CurSnCoded;
            }
            packet.SourceHeader = sourceHeader;

            byte[] data = PduMarshaler.Marshal(packet, false);
            SendBytesByUdp(data);

            // Deal with outPacketDic and retransmit packet.
            if (outPacketDic.ContainsKey(packet.SourceHeader.Value.snSourceStart))
            {
                outPacketDic[packet.SourceHeader.Value.snSourceStart].RetransmitTimes++;
                outPacketDic[packet.SourceHeader.Value.snSourceStart].SendTime = DateTime.Now;
            }
            return true;
        }

        /// <summary>
        /// Send an ACK Datagrams
        /// </summary>
        /// <returns></returns>
        public bool SendAckPacket(bool delayACK = false)
        {
            if (!connected)
            {
                return false;
            }

            RdpeudpPacket AckPacket = new RdpeudpPacket();
            AckPacket.FecHeader.snSourceAck = SnSourceAck;
            AckPacket.FecHeader.uReceiveWindowSize = UReceiveWindowSize;
            AckPacket.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_ACK;

            if (delayACK)
            {
                AckPacket.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_ACK | RDPUDP_FLAG.RDPUDP_FLAG_ACKDELAYED;
            }

            AckPacket.AckVectorHeader = CreateAckVectorHeader();

            SendPacket(AckPacket);

            return true;
        }

        /// <summary>
        /// Method used to process a received packet
        /// </summary>
        /// <param name="packet"></param>
        public void ReceivePacket(StackPacket packet)
        {
            lock (receiveLock)
            {
                if (upgradedToRdpedup2)
                {
                    if (!connected || rdpeudp2Handler == null)
                    {
                        rdpeudp2UpgradeBuffer.Add(packet);
                        return;
                    }

                    if (rdpeudp2UpgradeBuffer.Any())
                    {
                        foreach (var p in rdpeudp2UpgradeBuffer)
                        {
                            rdpeudp2Handler.ReceivePacket(p);
                        }

                        rdpeudp2UpgradeBuffer.Clear();
                    }

                    rdpeudp2Handler.ReceivePacket(packet);
                    return;
                }
            }

            // Transfer packet to 
            RdpeudpPacket eudpPacket = new RdpeudpPacket();
            byte[] packetBytes = packet.ToBytes();
            if (!PduMarshaler.Unmarshal(packetBytes, eudpPacket, false))
            {
                return;
            }

            // ETW Provider Dump Message
            // string messageName = "RDPEUDP:ReceivedPDU";
            // ExtendedLogger.DumpMessage(messageName, DumpLevel_LayerTLS, eudpPacket.GetType().Name, packetBytes);

            ReceivePacket(eudpPacket);
        }

        /// <summary>
        /// Method used to process a received packet
        /// </summary>
        /// <param name="packet"></param>
        public void ReceivePacket(RdpeudpPacket eudpPacket)
        {
            // Update last receive time, which is used for keep alive timer
            latestDatagramReceivedAt = DateTime.Now;

            lock (receiveLock)
            {
                if (!connected || !AutoHandle)
                {
                    // If connection haven't been setuped, or Auto handle is false, this packet will not be processed automatically by this method. 
                    // It will buffer it for others to use it.
                    lock (unprocessedPacketBuffer)
                    {
                        unprocessedPacketBuffer.Add(eudpPacket);
                    }
                }
                else
                {
                    // Process the received packet
                    // In case the advised window size was updated
                    URemoteAdvisedWindowSize = eudpPacket.FecHeader.uReceiveWindowSize;
                    // This value should be updated when receiving ACK if congestion algorithm is implemented
                    USendWindowSize = URemoteAdvisedWindowSize;
                    // Process Ack Vector Header if the packet contained
                    ProcessAckVectorHeader(eudpPacket);
                    // Process Source Data if the packet contained
                    ProcessSourceData(eudpPacket);
                    // Process FEC Payload data if the packet contained
                    ProcessFECPayloadData(eudpPacket);
                    // Process RDPUDP_ACK_OF_ACKVECTOR_HEADER Structure if the packet contained
                    ProcessAckOfAckVectorHeader(eudpPacket);
                    // Process RDPUPD_SYNDATAEX_PAYLOAD
                    ProcessSynDataExPayload(eudpPacket);

                    //TODO: Congestion control function
                }
            }
        }

        #endregion

        #region Other Public Methods

        /// <summary>
        /// Close connection of this socket
        /// </summary>
        public void Close()
        {
            connected = false;

            Rdpeudp2Handler?.Close();

            Disconnected?.Invoke();

            DisposeTimers();
        }

        /// <summary>
        /// Dispose this socket
        /// </summary>
        public void Dispose()
        {
            if (connected)
            {
                Close();
            }
        }
        #endregion

        #region Private/Internal methods

        /// <summary>
        /// Process RDPUDP_ACK_VECTOR_HEADER if the packet contains a RDPUDP_ACK_VECTOR_HEADER
        /// </summary>
        /// <param name="eudpPacket"></param>
        private void ProcessAckVectorHeader(RdpeudpPacket eudpPacket)
        {
            // Update OutSnResetSeqNum if necessary
            UpdateOutSnResetSeqNum(eudpPacket.FecHeader.snSourceAck);

            if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK) && !eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYN))
            {
                //Contains ack, analyze ack value, update outPacketDic, caculate RTT, and move send window
                if (eudpPacket.AckVectorHeader.Value.uAckVectorSize > 0)    // Deal with ack vector.
                {
                    uint currentposition = OutSnResetSeqNum;
                    lock (outPacketDic)
                    {
                        foreach (AckVectorElement AckVectorElement in eudpPacket.AckVectorHeader.Value.AckVector)
                        {
                            if (AckVectorElement.State == VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED)
                            {
                                // update outPacketDic only if this vector element is to ack received
                                for (byte i = 0; i < AckVectorElement.Length + 1; i++, currentposition++)
                                {
                                    if (outPacketDic.ContainsKey(currentposition))
                                    {
                                        outPacketDic[currentposition].Acknowledged = true;
                                    }
                                }
                            }
                            else
                            {
                                currentposition = currentposition + AckVectorElement.Length + 1;
                            }
                        }

                        // If this packet is not a delayed ack, calculate RTT, only the last acknowleged source packet is used to caculate RTT
                        if (outPacketDic.ContainsKey(currentposition - 1))
                        {
                            if (!eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACKDELAYED)
                                && !outPacketDic[currentposition - 1].EstimatedRTT) // Also make sure this packet has not been used to estimate RTT. If the packet has been used to estimated RTT, this packet may be a resent ACK for keep alive
                            {
                                RTT = new TimeSpan(RTT.Ticks / 8 * 7 + (DateTime.Now - outPacketDic[currentposition - 1].SendTime).Ticks / 8);  // Count the RTT.
                                outPacketDic[currentposition - 1].EstimatedRTT = true;
                            }
                        }

                        // Update send window, this method will update outPacketDic
                        UpdateSendWindow();
                    }
                    sendWindowLock.Set();
                }
            }
        }

        /// <summary>
        /// Process Source payload if the packet has source payload data
        /// </summary>
        /// <param name="eudpPacket"></param>
        public void ProcessSourceData(RdpeudpPacket eudpPacket)
        {
            if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA) && !eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_FEC))
            {
                lock (inPacketDic)
                {
                    if (IsInReceiveWindow(eudpPacket.SourceHeader.Value.snSourceStart))
                    {
                        SnSourceAck = Math.Max(SnSourceAck, eudpPacket.SourceHeader.Value.snSourceStart);
                        InPacketState inPacketState = new InPacketState();
                        inPacketState.Packet = eudpPacket;
                        inPacketDic[eudpPacket.SourceHeader.Value.snSourceStart] = inPacketState;

                        UpdateReceiveWindow();
                    }

                    // Increase received source packet numbers not be ack
                    if (sourceNumNotAcked == 0)
                    {
                        receiveTimeForFirstNotACKSource = DateTime.Now;
                    }
                    sourceNumNotAcked++;
                }
                // Send ACK diagram if necessary.
                SendAckPacketBack();
            }
        }

        /// <summary>
        /// Process RDPUDP_ACK_OF_ACKVECTOR_HEADER Structure if the packet have
        /// </summary>
        /// <param name="eudpPacket"></param>
        public void ProcessAckOfAckVectorHeader(RdpeudpPacket eudpPacket)
        {
            if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK_OF_ACKS))
            {
                InSnResetSeqNum = eudpPacket.AckOfAckVector.Value.snResetSeqNum;
            }
        }

        /// <summary>
        /// Process RDPUDP_FEC_PAYLOAD_HEADER Structure and FEC Payload
        /// </summary>
        /// <param name="eudpPacket"></param>
        public void ProcessFECPayloadData(RdpeudpPacket eudpPacket)
        {
            if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_FEC))
            {
                // TODO: process FEC payload, calculate lost packet with FEC data
                // then use UpdateReceiveWindow function to update window and process received data
            }
        }

        /// <summary>
        /// The highest version supported by both endpoints, which is RDPUDP_PROTOCOL_VERSION_1 if either this packet or the SYN packet does not specify a version, 
        /// is the version that MUST be used by both endpoints.
        /// </summary>
        public void ProcessSynDataExPayload(RdpeudpPacket eudpPacket)
        {
            if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYNEX) && eudpPacket.SynDataEx != null)
            {
                if (eudpPacket.SynDataEx.Value.uUdpVer.HasFlag(RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3))
                {
                    HighestVersion = RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3;
                    return;
                }
                else if (eudpPacket.SynDataEx.Value.uUdpVer.HasFlag(RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_2))
                {
                    HighestVersion = RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_2;
                    return;
                }
            }
            HighestVersion = RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_1;
        }


        /// <summary>
        /// Process a Syn Packet
        /// </summary>
        /// <param name="eudpPacket"></param>
        public void ProcessSynPacket(RdpeudpPacket eudpPacket)
        {
            if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYN)
                || eudpPacket.FecHeader.uFlags.Equals(RDPUDP_FLAG.RDPUDP_FLAG_SYN | RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY)) // Make sure this packet is a SYN Packet
            {
                UDownStreamMtu = (ushort)(Math.Min(Math.Min(eudpPacket.SynData.Value.uUpStreamMtu, UDownStreamMtu), (ushort)1232));
                UUpStreamMtu = (ushort)(Math.Min(Math.Min(eudpPacket.SynData.Value.uDownStreamMtu, UUpStreamMtu), (ushort)1232));
                USendWindowSize = eudpPacket.FecHeader.uReceiveWindowSize;
                SnSourceAck = eudpPacket.SynData.Value.snInitialSequenceNumber;

                InSnResetSeqNum = eudpPacket.SynData.Value.snInitialSequenceNumber + 1;
                ReceiveWindowStartPosition = eudpPacket.SynData.Value.snInitialSequenceNumber + 1;
            }
        }

        /// <summary>
        /// Set a source packet have been received
        /// This method is only used when autohandle is false, this can make sure create correct Ack header when autohandle is false
        /// </summary>
        /// <param name="snSourceStart"></param>
        public void MarkSourcePacketReceived(uint snSourceStart)
        {
            if (!AutoHandle)
            {
                this.inPacketDic[snSourceStart] = null;
            }
        }

        /// <summary>
        /// Update Receive window, process and remove received packets 
        /// </summary>
        /// <param name="MaxReceivedSeqNum"></param>
        private void UpdateReceiveWindow()
        {

            while (inPacketDic.ContainsKey(ReceiveWindowStartPosition))
            {
                // process the packet
                if (Received != null)
                {
                    Received(inPacketDic[ReceiveWindowStartPosition].Packet.Payload);
                }
                // Set the packet to null, but not remove it so as to create ACK vector 
                inPacketDic[ReceiveWindowStartPosition].Packet = null;
                ReceiveWindowStartPosition++;
            }

            // For lossy connection, Lossy connection mark a packet as lost if received 3 ack of packets after it, Go through the status from ReceiveWindowStartPosition to SnSourceAck (Max source sequence number received)
            if (TransMode == TransportMode.Lossy &&
                IsInReceiveWindow(SnSourceAck)) // If all received sources have been processed, SnSourceAck should not in receive window
            {
                int count = this.SocketConfig.MarkLostPacketsNumber;
                uint curPos = SnSourceAck;

                while (count > 0 && IsInReceiveWindow(curPos))
                {
                    if (inPacketDic.ContainsKey(curPos))
                    {
                        count--;
                    }
                    curPos--;
                }

                if (IsInReceiveWindow(curPos))
                {
                    while (inPacketDic.ContainsKey(curPos))
                    {
                        curPos++;
                    }

                    while (ReceiveWindowStartPosition != curPos)
                    {
                        if (inPacketDic.ContainsKey(ReceiveWindowStartPosition))
                        {
                            if (Received != null)
                            {
                                Received(inPacketDic[ReceiveWindowStartPosition].Packet.Payload);
                            }
                            // Set the packet to null, but not remove it so as to create ACK vector 
                            inPacketDic[ReceiveWindowStartPosition].Packet = null;
                        }
                        ReceiveWindowStartPosition++;
                    }
                }
            }
        }
        /// <summary>
        /// Update Send Window, remove acknowledged packet from outPacketDic
        /// </summary>
        /// <param name="maxAckSeqNum"></param>
        private void UpdateSendWindow()
        {
            while (outPacketDic.ContainsKey(SendWindowStartPosition) && (outPacketDic[SendWindowStartPosition].Acknowledged))
            {
                outPacketDic.Remove(SendWindowStartPosition); // Client has receive the packet, Remove it from outPacketDic.
                SendWindowStartPosition++;
            }

            // For lossy connection, Lossy connection mark a packet as lost if received 3 ack of packets after it, so should check states from SendeWindowStartPosition to CurSnSource
            if (TransMode == TransportMode.Lossy &&
                IsInSendWindow(CurSnSource)) // If all sent sources have been Acknowledged, CurSnSource should not in send window
            {
                int count = this.SocketConfig.MarkLostPacketsNumber;
                uint curPos = CurSnSource;
                while (count > 0 && IsInSendWindow(curPos))
                {
                    if (outPacketDic.ContainsKey(curPos) && (outPacketDic[curPos].Acknowledged))
                    {
                        count--;
                    }
                    curPos--;
                }
                if (IsInSendWindow(curPos))
                {
                    while (outPacketDic.ContainsKey(curPos) && (outPacketDic[curPos].Acknowledged))
                    {
                        curPos++;
                    }
                    while (SendWindowStartPosition != curPos)
                    {
                        if (outPacketDic.ContainsKey(SendWindowStartPosition) && (outPacketDic[SendWindowStartPosition].Acknowledged))
                        {
                            outPacketDic.Remove(SendWindowStartPosition);
                        }
                        SendWindowStartPosition++;
                    }
                }
            }
        }

        /// <summary>
        /// Verify whether a sequence number is in receive window
        /// </summary>
        /// <param name="snSourceStart">sequence number of a received source packet</param>
        /// <returns></returns>
        private bool IsInReceiveWindow(uint snSourceStart)
        {
            uint inslideWindowEndPos = ReceiveWindowStartPosition + UReceiveWindowSize;
            if (inslideWindowEndPos > ReceiveWindowStartPosition
                && (snSourceStart >= ReceiveWindowStartPosition && snSourceStart < inslideWindowEndPos))
            {
                return true;
            }
            if (inslideWindowEndPos < ReceiveWindowStartPosition
                && (snSourceStart >= ReceiveWindowStartPosition || snSourceStart < inslideWindowEndPos))    //if the Receive window is wrapped around
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verify whether a sequence number is in send window
        /// </summary>
        /// <param name="snSourceStart">sequence number of source packet for sent</param>
        /// <returns></returns>
        private bool IsInSendWindow(uint snSourceStart)
        {
            uint outSlideWindowEndPos = SendWindowStartPosition + USendWindowSize;
            if (outSlideWindowEndPos > SendWindowStartPosition
                && (snSourceStart >= SendWindowStartPosition && snSourceStart < outSlideWindowEndPos))
            {
                return true;
            }
            if (outSlideWindowEndPos < SendWindowStartPosition
                && (snSourceStart >= SendWindowStartPosition || snSourceStart < outSlideWindowEndPos))    //if the Send window is wrapped around
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Used during sending source packet, Add RDPUDP_ACK_OF_ACKVECTOR_HEADER structure into the packet to update OutSnResetSeqNum
        /// </summary>
        /// <param name="eudpPacket">Packet to be sent</param>
        private void UpdateOutSnResetSeqNum(RdpeudpPacket eudpPacket)
        {
            if (newOutSnResetSeqNum == null
                && SendWindowStartPosition - 1 - OutSnResetSeqNum > this.SocketConfig.ChangeSnResetSeqNumInterval)
            {
                lock (updateOutSnResetSeqNumLock)
                {
                    if (newOutSnResetSeqNum == null
                        && SendWindowStartPosition - 1 - OutSnResetSeqNum > this.SocketConfig.ChangeSnResetSeqNumInterval)
                    {
                        newOutSnResetSeqNum = SendWindowStartPosition - 1;
                        RDPUDP_ACK_OF_ACKVECTOR_HEADER ackOfAckVector = new RDPUDP_ACK_OF_ACKVECTOR_HEADER();
                        ackOfAckVector.snResetSeqNum = newOutSnResetSeqNum.Value;
                        eudpPacket.AckOfAckVector = ackOfAckVector;
                        eudpPacket.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_ACK_OF_ACKS | eudpPacket.FecHeader.uFlags;
                        seqNumOfPacketWithAckOfAckVector = eudpPacket.SourceHeader.Value.snSourceStart;
                    }
                }
            }
        }

        /// <summary>
        /// Used during receiving packet, if snSourceAck of received packet is larger than seqNumofPacketWithAckOfAckVector
        /// Update the OutSnResetSeqNum to new one
        /// </summary>
        /// <param name="receivedSnSourceAck">snSourceAck of received packet</param>
        private void UpdateOutSnResetSeqNum(uint receivedSnSourceAck)
        {
            if (newOutSnResetSeqNum != null
                && receivedSnSourceAck >= seqNumOfPacketWithAckOfAckVector)
            {
                lock (updateOutSnResetSeqNumLock)
                {
                    if (newOutSnResetSeqNum != null
                        && receivedSnSourceAck >= seqNumOfPacketWithAckOfAckVector)
                    {
                        OutSnResetSeqNum = newOutSnResetSeqNum.Value;
                        newOutSnResetSeqNum = null;
                    }
                }
            }
        }

        /// <summary>
        /// Send Ack packet back to the sender if necessary.
        /// </summary>
        private void SendAckPacketBack()
        {
            if (!Connected || !AutoHandle)
            {
                return;
            }

            if (this.sourceNumNotAcked >= this.SocketConfig.AckSourcePacketsNumber)
            {
                this.SendAckPacket();
            }

            TimeSpan delayedAckDuration = SocketConfig.DelayedAckDurationV1;
            if (HighestVersion == RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_2)
            {
                TimeSpan half = new TimeSpan(RTT.Ticks / 2);
                // RDPUDP_PROTOCOL_VERSION_2: the delayed ACK time-out is 50 ms or half the RTT, whichever is longer, up to a maximum of 200 ms.
                delayedAckDuration = SocketConfig.DelayedAckDurationV2 > half ? SocketConfig.DelayedAckDurationV2 : half;
                delayedAckDuration = delayedAckDuration > SocketConfig.DelayedAckDurationMax ? SocketConfig.DelayedAckDurationMax : delayedAckDuration;
            }

            if (this.sourceNumNotAcked > 0 &&
                receiveTimeForFirstNotACKSource + delayedAckDuration <= DateTime.Now)
            {
                // Send ACK diagram, and set RDPUDP_FLAG_ACKDELAYED flag 
                this.SendAckPacket(true);
            }
        }

        #region Methods for Timers

        /// <summary>
        /// Create Timers for RDPEUDP socket
        /// </summary>
        private void InitTimers()
        {
            // Wrap handlers in try-catch blocks to ignore exceptions thrown in handler threads.
            retransmitTimer = new Timer((object state) => { try { ManageRetransmit(state); } catch { } }, null, 0, this.SocketConfig.RetransmitTimerInterval);
            keepaliveTimer = new Timer((object state) => { try { ManageKeepalive(state); } catch { } }, null, 0, this.SocketConfig.KeepaliveTimerInterval);
            delayedAckTimer = new Timer((object state) => { try { ManageDelayedAck(state); } catch { } }, null, 0, this.SocketConfig.DelayedAckInterval);
        }

        /// <summary>
        /// Dispose Timers of RDPEUDP socket
        /// </summary>
        protected void DisposeTimers()
        {
            retransmitTimer?.Dispose();
            keepaliveTimer?.Dispose();
            delayedAckTimer?.Dispose();
        }

        /// <summary>
        /// Function used to manage retransmit periodically
        /// </summary>
        /// <param name="state"></param>
        private void ManageRetransmit(object state)
        {
            if (!Connected)
            {
                return;
            }

            if (TransMode == TransportMode.Lossy)
            {
                // No need to retransmit packet if connection is lossy
                return;
            }

            // This timer MUST fire at the minimum retransmit time-out or twice the RTT, whichever is longer, after the datagram is first transmitted
            // RDPUDP_PROTOCOL_VERSION_1: the minimum retransmit time-out is 500 ms.
            // RDPUDP_PROTOCOL_VERSION_2: the minimum retransmit time-out is 300 ms.
            TimeSpan _retransmitDuration = SocketConfig.RetransmitDurationV1;
            if (HighestVersion == RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_2)
            {
                _retransmitDuration = SocketConfig.RetransmitDurationV2;
            }

            TimeSpan retransmitDuration = ((RTT + RTT) > _retransmitDuration) ? (RTT + RTT) : _retransmitDuration;
            // Packets may need to retransmit must in send window
            lock (outPacketDic)
            {
                uint curpos = SendWindowStartPosition;
                while (outPacketDic.ContainsKey(curpos)
                    && (outPacketDic[curpos].RetransmitTimes > 0 || outPacketDic[curpos].SendTime + retransmitDuration <= DateTime.Now))
                {
                    if (outPacketDic[curpos].SendTime + retransmitDuration <= DateTime.Now)
                    {
                        RetransmitPacket(outPacketDic[curpos].Packet);
                    }
                    curpos++;
                }
            }
        }

        /// <summary>
        /// Function used to manage keep alive timer
        /// </summary>
        /// <param name="state"></param>
        private void ManageKeepalive(object state)
        {
            if (!Connected)
            {
                return;
            }

            if (latestDatagramReceivedAt + this.SocketConfig.LostConnectionTimeout <= DateTime.Now)
            {
                // If the sender does not receive any ACK from the receiver after 65 seconds, the connection is terminated.
                this.Close();
            }

            if (latestDatagramSentAt + this.SocketConfig.KeepaliveDuration <= DateTime.Now)
            {
                // Send an ACK packet to keep alive
                this.SendAckPacket();
            }
        }

        /// <summary>
        /// Function used to manage Delayed ACK timer
        /// </summary>
        /// <param name="state"></param>
        private void ManageDelayedAck(object state)
        {
            if (!Connected)
            {
                return;
            }

            SendAckPacketBack();
        }

        #endregion

        #endregion
    }
}
