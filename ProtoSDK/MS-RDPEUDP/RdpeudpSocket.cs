// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Protocols.TestTools.ExtendedLogging;

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
            retransmitTimes = 0;
            Acknowledged = false;
            estimatedRTT = false;
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
        public int retransmitTimes;

        /// <summary>
        /// Whether this packet has been used to estimate RTT
        /// one packet only can be used to estimate RTT once
        /// </summary>
        public bool estimatedRTT;
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

    #endregion Type Definitions

    #region Delegates definitions
    /// <summary>
    /// Delegation to receive packets from the specified remote endpoint.
    /// </summary>
    /// <param name="remoteEP">The specified remote endpoint.</param>
    /// <param name="packet">Packet need to sent.</param>
    public delegate void RdpeudpSocketSender(IPEndPoint remoteEP, StackPacket packet);

    public delegate void ReceiveData(byte[] data);
    #endregion Delegates definitions

    public class RdpeudpSocket : IDisposable
    {
        #region Private variables
        // Used to make sure only one thread call this.ReceivePacket
        private readonly Object receiveLock = new object();
        // Lock to protect CurSnCoded and CurSnSource
        private readonly Object sequenceNumberLock = new object();
        // Block send method if the new source packet is not in send window
        private AutoResetEvent sendWindowLock = new AutoResetEvent(false);
        // Method used to send packet
        private RdpeudpSocketSender packetSender;

        private TimeSpan RTT;
        private IPEndPoint remoteEndPoint;

        private bool connected;
        private bool autoHandle;

        protected List<RdpeudpPacket> unProcessedPacketBuffer = new List<RdpeudpPacket>();

        // Cache for sending source packets, only for reliable connection, packet will be cached after sent but not acknowledged received
        private Dictionary<uint, OutPacketState> outPacketDic = new Dictionary<uint, OutPacketState>();
        // Cache for received source packets.
        private Dictionary<uint, InPacketState> inPacketDic = new Dictionary<uint, InPacketState>();

        // Variables used to update OutSnAckOfAcksSeqNum
        // Sequence number with which the packet send an AckOfAckVector
        private uint seqNumofPacketWithAckOfAckVector;
        // new OutSnAckOfAcksSeqNum
        private uint? newOutSnAckOfAcksSeqNum = null;
        private readonly Object updateAckOfAckLock = new object();

        #region Timers

        // Timer used to manage retransmit
        private Timer retransmitTimer;

        // Timer used to manage keep alive
        private Timer keepAliveTimer;

        // Timer used to manage delay ack
        private Timer delayACKTimer;
        #endregion Timers

        // Below two variables are used for keep alive timer
        private DateTime LastSendDiagramTime;
        private DateTime LastReceiveDiagramTime;

        // Number of received packets not ack yet, this value only be changed in Lock(inPacketDic) block so as to sync
        private int sourceNumNotAcked = 0;
        private DateTime ReceiveTimeForFirstNotACKSource;

        public const DumpLevel DumpLevel_LayerTLS = (DumpLevel)10;

        #endregion Private variables

        #region Properties

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
                    if (connected && autoHandle)
                    {
                        // Timer will start when connect established
                        this.InitTimers();
                    }
                }
            }
        }

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
                        if (connected)
                        {
                            InitTimers();
                        }
                    }
                    else
                    {
                        // If autoHandle changed from true to false, stop timer
                        DisposeTimers();
                    }
                }
            }
        }

        public uint SnInitialSequenceNumber { get; set; }

        public uUdpVer_Values HighestVersion { get; set; }

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
        public uint InSnAckOfAcksSeqNum { get; set; }
        // Start position of ACK vector received, which is used to analyze received Ack vector
        public uint OutSnAckOfAcksSeqNum { get; set; }

        // Window size that Remote endpoint advised
        public ushort URemoteAdvisedWindowSize { get; set; }
        // Send Window Size, which is the also the congestion window size, 
        // There is space in the receiver-advertised window for this datagram and the Congestion Control logic permits transmission of a datagram.
        public ushort USendWindowSize { get; set; }

        // Receive windows Size
        public ushort UReceiveWindowSize { get; set; }

        // Transport Mode: Reliable or Lossy
        public TransportMode TransMode { get; set; }
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
        public RdpeudpSocket(TransportMode mode, IPEndPoint remoteEp, bool autohandle, RdpeudpSocketSender sender)
        {
            this.TransMode = mode;
            this.SocketConfig = new RdpeudpSocketConfig();
            this.AutoHandle = autohandle;
            this.connected = false;
            remoteEndPoint = remoteEp;

            // Initial highestAckNumber as 0 because every hihestAckNumber will compare with Sequence Number of coming packet, and set as the bigger one.
            SnSourceAck = 0;

            URemoteAdvisedWindowSize = SocketConfig.initialWindowSize;
            USendWindowSize = SocketConfig.initialWindowSize;
            UReceiveWindowSize = SocketConfig.initialWindowSize;
            UUpStreamMtu = SocketConfig.initialStreamMtu;
            UDownStreamMtu = SocketConfig.initialStreamMtu;

            OutSnAckOfAcksSeqNum = SocketConfig.initialAcksPosition;
            InSnAckOfAcksSeqNum = SocketConfig.initialAcksPosition;

            ReceiveWindowStartPosition = SocketConfig.initialAcksPosition;
            RTT = new TimeSpan(0, 0, 0, 0, this.SocketConfig.DelayAckTime);

            packetSender = sender;
        }

        #endregion Constructor

        #region Methods for Auto-handle Socket

        /// <summary>
        /// Send Data from this specified UDP transport
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <returns>Return true if send success</returns>
        public bool Send(byte[] data)
        {
            if (!Connected) return false;
            List<byte> dataList = new List<byte>(data);
            int payloadLength = 0;
            byte[] packetData;
            RdpeudpPacket packet;
            do
            {
                packet = new RdpeudpPacket();                  // Fill in the common header.
                packet.fecHeader.snSourceAck = SnSourceAck;
                packet.fecHeader.uReceiveWindowSize = UReceiveWindowSize;
                packet.fecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_DATA | RDPUDP_FLAG.RDPUDP_FLAG_ACK;
                packet.ackVectorHeader = CreateAckVectorHeader();

                packet.sourceHeader = CreateSourcePayloadHeader();  // Generate SourceHeader.

                packetData = PduMarshaler.Marshal(packet, false);      // Measure the header lenght to figure out payload length.
                payloadLength = Math.Min(UUpStreamMtu - packetData.Length, dataList.Count);

                packet.payload = new byte[payloadLength];
                dataList.CopyTo(0, packet.payload, 0, payloadLength);   // Copy the data in to packet payload.

                string str = System.Text.Encoding.ASCII.GetString(packet.payload);
                SendPacket(packet);
                dataList.RemoveRange(0, payloadLength);

            } while (dataList.Count > 0);

            return true;
        }

        /// <summary>
        /// An Event will be called when receive Data bytes from this specified UDP transport
        /// </summary>
        public event ReceiveData Received;

        #endregion Methods for Auto-handle Socket

        #region Methods used only when AutoHandle is false

        /// <summary>
        /// Send a RDPEUDP Packet
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public bool SendPacket(RdpeudpPacket packet)
        {
            if (packet.sourceHeader.HasValue && AutoHandle)
            {                                 // Deal with window area.

                OutPacketState packetState = new OutPacketState();
                packetState.Packet = packet;
                packetState.Acknowledged = false;


                DateTime endTime = DateTime.Now + this.SocketConfig.Timeout;
                uint sendWindowsEndPos = SendWindowStartPosition + USendWindowSize;

                while (DateTime.Now < endTime)
                {
                    //If source sequence number is in send slide window, send the packet. Otherwise, wait a sendingInterval
                    if (!IsInSendWindow(packet.sourceHeader.Value.snSourceStart))
                    {
                        sendWindowLock.WaitOne(this.SocketConfig.sendingInterval);
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
                lock (outPacketDic) outPacketDic[packet.sourceHeader.Value.snSourceStart] = packetState;

                // Add RDPUDP_ACK_OF_ACKVECTOR_HEADER Structure if necessary
                UpdateOutSnAckOfAcksSeqNum(packet);

            }

            byte[] data = PduMarshaler.Marshal(packet, false);
            SendBytesByUDP(data);

            // Update Last send time, which is used for keep alive timer
            LastSendDiagramTime = DateTime.Now;

            return true;
        }

        /// <summary>
        /// Send bytes via UDP transport
        /// </summary>
        /// <param name="data"></param>
        public void SendBytesByUDP(byte[] data)
        {
            StackPacket stackPacket = new RdpeudpBasePacket(data);
            packetSender(remoteEndPoint, stackPacket);

            // ETW Provider Dump Message
            string messageName = "RDPEUDP:SentPDU";
            ExtendedLogger.DumpMessage(messageName, DumpLevel_LayerTLS, typeof(RdpeudpPacket).Name, data);
        }

        public RdpeudpPacket ExpectPacket(TimeSpan timeout)
        {
            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                lock (unProcessedPacketBuffer)
                {
                    if (unProcessedPacketBuffer.Count > 0)
                    {
                        RdpeudpPacket eudpPacket = unProcessedPacketBuffer[0];
                        unProcessedPacketBuffer.RemoveAt(0);
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
        public RdpeudpPacket ExpectACKPacket(TimeSpan timeout)
        {
            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                lock (unProcessedPacketBuffer)
                {
                    for (int i = 0; i < unProcessedPacketBuffer.Count; i++)
                    {
                        RdpeudpPacket eudpPacket = unProcessedPacketBuffer[i];
                        if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK))
                        {
                            if (!Connected || (eudpPacket.ackVectorHeader.HasValue && eudpPacket.ackVectorHeader.Value.uAckVectorSize > 0))
                            {
                                unProcessedPacketBuffer.RemoveAt(i);
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

            RdpeudpPacket packet = new RdpeudpPacket();                  // Fill in the common header.
            packet.fecHeader.snSourceAck = SnSourceAck;
            packet.fecHeader.uReceiveWindowSize = UReceiveWindowSize;
            packet.fecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_DATA | RDPUDP_FLAG.RDPUDP_FLAG_ACK;
            packet.ackVectorHeader = CreateAckVectorHeader();

            packet.sourceHeader = CreateSourcePayloadHeader();
            packet.payload = data;
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
                ackVectorHeader.AckVectorElement = null;
                ackVectorHeader.Padding = null;
            }
            else
            {
                lock (inPacketDic)
                {
                    // Generate an ACK header.
                    List<AckVector> ackVectorElements = new List<AckVector>();
                    VECTOR_ELEMENT_STATE currentState = inPacketDic.ContainsKey(InSnAckOfAcksSeqNum) ? VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED : VECTOR_ELEMENT_STATE.DATAGRAM_NOT_YET_RECEIVED;
                    AckVector ackVectorElement = new AckVector();
                    ackVectorElement.State = currentState;
                    ackVectorElement.Length = 0;

                    // RLE encoding.
                    for (uint i = InSnAckOfAcksSeqNum + 1; i <= SnSourceAck; i++)
                    {
                        currentState = inPacketDic.ContainsKey(i) ? VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED : VECTOR_ELEMENT_STATE.DATAGRAM_NOT_YET_RECEIVED;

                        // 0x40 is the max length of state length, of which I believe sould be indicate from soket config or types config not hard code here.
                        if (currentState != ackVectorElement.State || ackVectorElement.Length >= 0x40)
                        {
                            // If current state differs from last state, assign a new ack vector.
                            ackVectorElements.Add(ackVectorElement);
                            ackVectorElement = new AckVector();
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
                    ackVectorHeader.AckVectorElement = ackVectorElements.ToArray();

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

            Monitor.Enter(sequenceNumberLock);
            sourceHeader.snCoded = ++CurSnCoded;
            sourceHeader.snSourceStart = ++CurSnSource;
            Monitor.Exit(sequenceNumberLock);

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

            Monitor.Enter(sequenceNumberLock);
            fecPayloadHeader.snCoded = ++CurSnCoded;
            Monitor.Exit(sequenceNumberLock);

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
            RDPUDP_SYNDATA_PAYLOAD SynData = new RDPUDP_SYNDATA_PAYLOAD();
            if (initialSequenceNumber == null)
            {
                Random random = new Random();
                SnInitialSequenceNumber = (uint)random.Next();
            }
            else
            {
                SnInitialSequenceNumber = initialSequenceNumber.Value;
            }
            SynData.snInitialSequenceNumber = SnInitialSequenceNumber;
            SynData.uUpStreamMtu = UUpStreamMtu;
            SynData.uDownStreamMtu = UDownStreamMtu;
            CurSnCoded = SynData.snInitialSequenceNumber;
            CurSnSource = SynData.snInitialSequenceNumber;
            SendWindowStartPosition = SynData.snInitialSequenceNumber + 1;

            return SynData;
        }

        /// <summary>
        /// Create RDPUDP_SYNDATAEX_PAYLOAD Structure
        /// </summary>
        /// <returns></returns>
        public RDPUDP_SYNDATAEX_PAYLOAD CreateSynExData(uUdpVer_Values version)
        {
            RDPUDP_SYNDATAEX_PAYLOAD SynExData = new RDPUDP_SYNDATAEX_PAYLOAD();
            SynExData.uSynExFlags = uSynExFlags_Values.RDPUDP_VERSION_INFO_VALID;

            //The uUdpVer field MUST be set to the highest RDP-UDP protocol version supported by both endpoints
            if ((version & uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_2) != 0)
            {
                SynExData.uUdpVer = uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_2;
            }
            else
            {
                SynExData.uUdpVer = uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_1;
            }

            return SynExData;
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
            if (!connected ||
                packet.sourceHeader == null) return false;

            if (!outPacketDic.ContainsKey(packet.sourceHeader.Value.snSourceStart))
            {
                return false;
            }

            if (outPacketDic[packet.sourceHeader.Value.snSourceStart].retransmitTimes >= this.SocketConfig.retransmitLimit)
            {
                //If a datagram has been retransmitted five times without a response, the sender terminates the connection
                this.Close();
            }
            RDPUDP_SOURCE_PAYLOAD_HEADER sourceHeader = packet.sourceHeader.Value;
            Monitor.Enter(sequenceNumberLock);
            sourceHeader.snCoded = ++CurSnCoded;
            Monitor.Exit(sequenceNumberLock);
            packet.sourceHeader = sourceHeader;

            byte[] data = PduMarshaler.Marshal(packet, false);
            SendBytesByUDP(data);

            lock (outPacketDic)         // Deal with outPacketDic and retransmit packet.
            {
                if (outPacketDic.ContainsKey(packet.sourceHeader.Value.snSourceStart))
                {
                    outPacketDic[packet.sourceHeader.Value.snSourceStart].retransmitTimes++;
                    outPacketDic[packet.sourceHeader.Value.snSourceStart].SendTime = DateTime.Now;
                }
            }
            return true;
        }

        /// <summary>
        /// Send an ACK Datagrams
        /// </summary>
        /// <returns></returns>
        public bool SendAcKPacket(bool delayACK = false)
        {
            if (!connected) return false;
            RdpeudpPacket AckPacket = new RdpeudpPacket();
            AckPacket.fecHeader.snSourceAck = SnSourceAck;
            AckPacket.fecHeader.uReceiveWindowSize = UReceiveWindowSize;
            AckPacket.fecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_ACK;

            if (delayACK)
            {
                AckPacket.fecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_ACK | RDPUDP_FLAG.RDPUDP_FLAG_ACKDELAYED;
            }

            AckPacket.ackVectorHeader = CreateAckVectorHeader();

            SendPacket(AckPacket);

            return true;
        }

        /// <summary>
        /// Method used to process a received packet
        /// </summary>
        /// <param name="packet"></param>
        public void ReceivePacket(StackPacket packet)
        {
            // Transfer packet to 
            RdpeudpPacket eudpPacket = new RdpeudpPacket();
            byte[] packetBytes = packet.ToBytes();
            if (!PduMarshaler.Unmarshal(packetBytes, eudpPacket, false))
            {
                return;
            }

            // ETW Provider Dump Message
            string messageName = "RDPEUDP:ReceivedPDU";
            ExtendedLogger.DumpMessage(messageName, DumpLevel_LayerTLS, eudpPacket.GetType().Name, packetBytes);

            ReceivePacket(eudpPacket);
        }

        /// <summary>
        /// Method used to process a received packet
        /// </summary>
        /// <param name="packet"></param>
        public void ReceivePacket(RdpeudpPacket eudpPacket)
        {
            // Update last receive time, which is used for keep alive timer
            LastReceiveDiagramTime = DateTime.Now;

            Monitor.Enter(receiveLock);

            if (!connected || !AutoHandle)
            {
                // If connection haven't been setuped, or Auto handle is false, this packet will not be processed automatically by this method. 
                // It will buffer it for others to use it.
                lock (unProcessedPacketBuffer) unProcessedPacketBuffer.Add(eudpPacket);
            }
            else
            {
                // Process the received packet
                // In case the advised window size updated
                URemoteAdvisedWindowSize = eudpPacket.fecHeader.uReceiveWindowSize;
                // this value should update when receiving ACK if congestion algorithm is implemented
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
            Monitor.Exit(receiveLock);

        }
        #endregion Methods used only when AutoHandle is false

        #region Other Public Methods

        /// <summary>
        /// Close connection of this socket
        /// </summary>
        public void Close()
        {
            connected = false;
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
        #endregion Other Public Methods

        #region Private/Internal methods

        /// <summary>
        /// Process RDPUDP_ACK_VECTOR_HEADER if the packet contains a RDPUDP_ACK_VECTOR_HEADER
        /// </summary>
        /// <param name="eudpPacket"></param>
        private void ProcessAckVectorHeader(RdpeudpPacket eudpPacket)
        {
            // Update OutSnAckOfAcksSeqNum if necessary
            UpdateOutSnAckOfAcksSeqNum(eudpPacket.fecHeader.snSourceAck);

            if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK) && !eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYN))
            {
                //Contains ack, analyze ack value, update outPacketDic, caculate RTT, and move send window
                if (eudpPacket.ackVectorHeader.Value.uAckVectorSize > 0)    // Deal with ack vector.
                {
                    uint currentposition = OutSnAckOfAcksSeqNum;
                    lock (outPacketDic)
                    {
                        foreach (AckVector AckVectorElement in eudpPacket.ackVectorHeader.Value.AckVectorElement)
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

                        // If this packet is not a delay ack, calculate RTT, only the last acknowleged source packet is used to caculate RTT
                        if (outPacketDic.ContainsKey(currentposition - 1))
                        {
                            if (!eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACKDELAYED)
                                && !outPacketDic[currentposition - 1].estimatedRTT) // Also make sure this packet has not been used to estimate RTT. If the packet has been used to estimated RTT, this packet may be a resent ACK for keep alive
                            {
                                RTT = new TimeSpan(RTT.Ticks / 8 * 7 + (DateTime.Now - outPacketDic[currentposition - 1].SendTime).Ticks / 8);  // Count the RTT.
                                outPacketDic[currentposition - 1].estimatedRTT = true;
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
            if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA) && !eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_FEC))
            {
                lock (inPacketDic)
                {
                    if (IsInReceiveWindow(eudpPacket.sourceHeader.Value.snSourceStart))
                    {
                        SnSourceAck = Math.Max(SnSourceAck, eudpPacket.sourceHeader.Value.snSourceStart);
                        InPacketState inPacketState = new InPacketState();
                        inPacketState.Packet = eudpPacket;
                        inPacketDic[eudpPacket.sourceHeader.Value.snSourceStart] = inPacketState;

                        UpdateReceiveWindow();
                    }

                    // Increase received source packet numbers not be ack
                    if (sourceNumNotAcked == 0)
                    {
                        ReceiveTimeForFirstNotACKSource = DateTime.Now;
                    }
                    sourceNumNotAcked++;
                }
                // Send ACK diagram if necessary.
                AckPacketReceived();
            }
        }

        /// <summary>
        /// Process RDPUDP_ACK_OF_ACKVECTOR_HEADER Structure if the packet have
        /// </summary>
        /// <param name="eudpPacket"></param>
        public void ProcessAckOfAckVectorHeader(RdpeudpPacket eudpPacket)
        {
            if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK_OF_ACKS))
            {
                InSnAckOfAcksSeqNum = eudpPacket.ackOfAckVector.Value.snAckOfAcksSeqNum;
            }
        }

        /// <summary>
        /// Process RDPUDP_FEC_PAYLOAD_HEADER Structure and FEC Payload
        /// </summary>
        /// <param name="eudpPacket"></param>
        public void ProcessFECPayloadData(RdpeudpPacket eudpPacket)
        {
            if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_FEC))
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
            if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYNEX) && eudpPacket.SynDataEx != null)
            {
                if (eudpPacket.SynDataEx.Value.uUdpVer.HasFlag(uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_2))
                {
                    HighestVersion = uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_2;
                    return;
                }
            }
            HighestVersion = uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_1;
        }


        /// <summary>
        /// Process a Syn Packet
        /// </summary>
        /// <param name="eudpPacket"></param>
        public void ProcessSynPacket(RdpeudpPacket eudpPacket)
        {
            if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYN)
                || eudpPacket.fecHeader.uFlags.Equals(RDPUDP_FLAG.RDPUDP_FLAG_SYN | RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY)) // Make sure this packet is a SYN Packet
            {
                UDownStreamMtu = (ushort)(Math.Min(Math.Min(eudpPacket.SynData.Value.uUpStreamMtu, UDownStreamMtu), (ushort)1232));
                UUpStreamMtu = (ushort)(Math.Min(Math.Min(eudpPacket.SynData.Value.uDownStreamMtu, UUpStreamMtu), (ushort)1232));
                USendWindowSize = eudpPacket.fecHeader.uReceiveWindowSize;
                SnSourceAck = eudpPacket.SynData.Value.snInitialSequenceNumber;

                InSnAckOfAcksSeqNum = eudpPacket.SynData.Value.snInitialSequenceNumber + 1;
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
                    Received(inPacketDic[ReceiveWindowStartPosition].Packet.payload);
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
                                Received(inPacketDic[ReceiveWindowStartPosition].Packet.payload);
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

            // For lossy connection, Lossy connection mark a packet as lost if received 3 ack of packets after it, so should check states from SendWindowStartPosition to CurSnSource
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
        /// Used during sending source packet, Add RDPUDP_ACK_OF_ACKVECTOR_HEADER structure into the packet to update OutSnAckOfAcksSeqNum 
        /// </summary>
        /// <param name="eudpPacket">Packet to be sent</param>
        private void UpdateOutSnAckOfAcksSeqNum(RdpeudpPacket eudpPacket)
        {
            if (newOutSnAckOfAcksSeqNum == null
                && SendWindowStartPosition - 1 - OutSnAckOfAcksSeqNum > this.SocketConfig.changeSnAckOfAcksSeqNumInterval)
            {
                Monitor.Enter(updateAckOfAckLock);
                if (newOutSnAckOfAcksSeqNum == null
                    && SendWindowStartPosition - 1 - OutSnAckOfAcksSeqNum > this.SocketConfig.changeSnAckOfAcksSeqNumInterval)
                {
                    newOutSnAckOfAcksSeqNum = SendWindowStartPosition - 1;
                    RDPUDP_ACK_OF_ACKVECTOR_HEADER ackOfAckVector = new RDPUDP_ACK_OF_ACKVECTOR_HEADER();
                    ackOfAckVector.snAckOfAcksSeqNum = newOutSnAckOfAcksSeqNum.Value;
                    eudpPacket.ackOfAckVector = ackOfAckVector;
                    eudpPacket.fecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_ACK_OF_ACKS | eudpPacket.fecHeader.uFlags;
                    seqNumofPacketWithAckOfAckVector = eudpPacket.sourceHeader.Value.snSourceStart;
                }
                Monitor.Exit(updateAckOfAckLock);
            }
        }

        /// <summary>
        /// Used during receiving packet, if snSourceAck of received packet is larger than seqNumofPacketWithAckOfAckVector
        /// Update the OutSnAckOfAcksSeqNum to new one
        /// </summary>
        /// <param name="receivedSnSourceAck">snSourceAck of received packet</param>
        private void UpdateOutSnAckOfAcksSeqNum(uint receivedSnSourceAck)
        {
            if (newOutSnAckOfAcksSeqNum != null
                && receivedSnSourceAck >= seqNumofPacketWithAckOfAckVector)
            {
                Monitor.Enter(updateAckOfAckLock);
                if (newOutSnAckOfAcksSeqNum != null
                    && receivedSnSourceAck >= seqNumofPacketWithAckOfAckVector)
                {
                    OutSnAckOfAcksSeqNum = newOutSnAckOfAcksSeqNum.Value;
                    newOutSnAckOfAcksSeqNum = null;
                }
                Monitor.Exit(updateAckOfAckLock);
            }
        }

        /// <summary>
        /// Send Ack packet if necessary.
        /// </summary>
        private void AckPacketReceived()
        {
            if (!Connected || !AutoHandle)
            {
                return;
            }

            if (this.sourceNumNotAcked >= this.SocketConfig.AckSourcePacketsNumber)
            {
                this.SendAcKPacket();
            }

            TimeSpan delayAckDuration = SocketConfig.DelayAckDuration_V1;
            if (HighestVersion == uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_2)
            {
                TimeSpan half = new TimeSpan(RTT.Ticks / 2);
                // RDPUDP_PROTOCOL_VERSION_2: the delayed ACK time-out is 50 ms or half the RTT, whichever is longer, up to a maximum of 200 ms.
                delayAckDuration = SocketConfig.DelayAckDuration_V2 > half ? SocketConfig.DelayAckDuration_V2 : half;
                delayAckDuration = delayAckDuration > SocketConfig.DelayAckDuration_Max ? SocketConfig.DelayAckDuration_Max : delayAckDuration;
            }

            if (this.sourceNumNotAcked > 0 &&
                ReceiveTimeForFirstNotACKSource + delayAckDuration <= DateTime.Now)
            {
                // Send ACK diagram, and set RDPUDP_FLAG_ACKDELAYED flag 
                this.SendAcKPacket(true);
            }
        }

        #region Methods for Timer
        /// <summary>
        /// Create Timers for RDPEUDP socket
        /// </summary>
        private void InitTimers()
        {
            retransmitTimer = new Timer(ManageRetransmit, null, 0, this.SocketConfig.retransmitTimerInterval);
            keepAliveTimer = new Timer(ManageKeepLive, null, 0, this.SocketConfig.keepAliveTimerInterval);
            delayACKTimer = new Timer(ManageDelayAck, null, 0, this.SocketConfig.delayAckInterval);
        }

        /// <summary>
        /// Dispose Times of RDPEUDP socket
        /// </summary>
        private void DisposeTimers()
        {
            if (retransmitTimer != null)
            {
                retransmitTimer.Dispose();
            }
            if (keepAliveTimer != null)
            {
                keepAliveTimer.Dispose();
            }
            if (delayACKTimer != null)
            {
                delayACKTimer.Dispose();
            }
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
            TimeSpan _retransmitDuration = SocketConfig.RetransmitDuration_V1;
            if (HighestVersion == uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_2)
            {
                _retransmitDuration = SocketConfig.RetransmitDuration_V2;
            }

            TimeSpan retransmitDuration = ((RTT + RTT) > _retransmitDuration) ? (RTT + RTT) : _retransmitDuration;
            // Packets may need to retransmit must in send window
            uint curpos = SendWindowStartPosition;
            while (outPacketDic.ContainsKey(curpos)
                && (outPacketDic[curpos].retransmitTimes > 0 || outPacketDic[curpos].SendTime + retransmitDuration <= DateTime.Now))
            {
                if (outPacketDic[curpos].SendTime + retransmitDuration <= DateTime.Now)
                {
                    RetransmitPacket(outPacketDic[curpos].Packet);
                }
                curpos++;
            }
        }

        /// <summary>
        /// Function used to manage keep alive timer
        /// </summary>
        /// <param name="state"></param>
        private void ManageKeepLive(object state)
        {
            if (!Connected)
            {
                return;
            }

            if (LastReceiveDiagramTime + this.SocketConfig.LostConnectionTimeOut <= DateTime.Now)
            {
                // If the sender does not receive any ACK from the receiver after 65 seconds, the connection is terminated.
                this.Close();
            }

            if (LastSendDiagramTime + this.SocketConfig.KeepaliveDuration <= DateTime.Now)
            {
                // Send an ACK packet to keep alive
                this.SendAcKPacket();
            }
        }

        /// <summary>
        /// Function used to manage Delay ACK timer
        /// </summary>
        /// <param name="state"></param>
        private void ManageDelayAck(object state)
        {
            if (!Connected)
            {
                return;
            }

            AckPacketReceived();
        }

        #endregion Methods for Timer
        #endregion Private/Internal methods
    }
}
