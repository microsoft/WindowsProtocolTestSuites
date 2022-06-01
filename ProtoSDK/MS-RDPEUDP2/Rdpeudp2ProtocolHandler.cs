// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2
{
    /// <summary>
    /// The state of <see cref="ReceiverBufferNode"/>.
    /// </summary>
    public enum ReceiverBufferNodeState : byte
    {
        /// <summary>
        ///  The packet is received.
        /// </summary>
        Received,

        /// <summary>
        /// The packet is not received yet.
        /// Specific to the implementation, it is the state for a gap node.
        /// </summary>
        Pending
    }

    /// <summary>
    /// The element of the receiver buffer.
    /// </summary>
    public class ReceiverBufferNode
    {
        /// <summary>
        /// The Rdpeudp2Packet associated to this node.
        /// </summary>
        public Rdpeudp2Packet Packet;

        /// <summary>
        /// The state of this node.
        /// </summary>
        public ReceiverBufferNodeState State;

        /// <summary>
        /// The timestamp in milliseconds of when the packet is received obtained from the stopwatch in the protocol handler.
        /// </summary>
        public long ReceivedAt;

        /// <summary>
        /// The data sequence number of the packet.
        /// </summary>
        public ushort DataSeqNum;

        /// <summary>
        /// The channel sequence number of the packet.
        /// </summary>
        public ushort ChannelSeqNum;
    }

    /// <summary>
    /// The state of <see cref="SenderBufferNode"/>.
    /// </summary>
    public enum SenderBufferNodeState : byte
    {
        /// <summary>
        /// The packet is sent and not ACKed yet.
        /// </summary>
        Pending,

        /// <summary>
        /// The packet is sent and ACKed.
        /// </summary>
        Received,

        /// <summary>
        /// The packet is detected as lost by the packet loss detection logic.
        /// Lost packets will be retransmitted by the retransmit timer.
        /// </summary>
        Lost
    }

    /// <summary>
    /// The element of the sender buffer.
    /// </summary>
    public class SenderBufferNode
    {
        /// <summary>
        /// The Rdpeudp2Packet associated to this node.
        /// </summary>
        public Rdpeudp2Packet Packet;

        /// <summary>
        /// The state of this node.
        /// </summary>
        public SenderBufferNodeState State;

        /// <summary>
        /// The timestamp in milliseconds of when the packet is sent obtained from the stopwatch in the protocol handler.
        /// </summary>
        public long SentAt;

        /// <summary>
        /// The data sequence number of the packet.
        /// </summary>
        public ushort DataSeqNum;

        /// <summary>
        /// The channel sequence number of the packet.
        /// </summary>
        public ushort ChannelSeqNum;
    }

    /// <summary>
    /// The handler to process RPDEUDP2 packets, it must be initialized via the transport transition from RDPEUDP to RDPEUDP2.
    /// </summary>
    public class Rdpeudp2ProtocolHandler : IDisposable
    {
        #region Private variables

        #region Locks

        // Lock to protect sender window sequence numbers.
        private readonly object sequenceNumberLock = new object();

        // Lock to update sender AckOfAcksSeqNum.
        private readonly object updateSenderAOASeqNumLock = new object();

        // Lock to update receiver AckOfAcksSeqNum.
        private readonly object updateReceiverAOASeqNumLock = new object();

        // Block send method if the new source packet is not in send window.
        private readonly AutoResetEvent senderWindowLock = new AutoResetEvent(false);

        #endregion

        #region Callbacks

        // Method used to send packet.
        private Action<byte[]> sendBytes;

        // Method used to close the connection.
        private Action closeConnection;

        // Method used to send bytes to higher layer.
        private Action<byte[]> sendBytesToHigherLayer;

        #endregion

        #region Protocol handling related variables

        // Buffer used for manual packet processing.
        protected List<Rdpeudp2Packet> unprocessedPacketBuffer = new List<Rdpeudp2Packet>();
        protected Dictionary<uint, long> receivedAtBuffer = new Dictionary<uint, long>();

        // Buffer used as the receiver window.
        protected SortedList<uint, ReceiverBufferNode> receiverBuffer = new SortedList<uint, ReceiverBufferNode>();
        protected SortedList<uint, ReceiverBufferNode> processedPackets = new SortedList<uint, ReceiverBufferNode>();
        protected SortedList<uint, byte[]> receivedDataBuffer = new SortedList<uint, byte[]>();
        private bool isFirstDataPacketReceived = false;
        private ushort currentReceiverAckOfAcksSeqNum = 0;
        private ushort currentReceiverChannelSeqNum = 1;

        // Buffer used as the sender window.
        protected SortedList<uint, SenderBufferNode> senderBuffer = new SortedList<uint, SenderBufferNode>();
        private bool shouldSendAckOfAcksPayload = false;
        private ushort currentSenderAckOfAcksSeqNum = 0;
        private ushort currentSenderDataSeqNum = 1;
        private ushort currentSenderChannelSeqNum = 1;

        // Clock used for calculate timestamps.
        private Stopwatch stopwatch = new Stopwatch();

        #endregion

        #region Timers

        // Timer used to manage retransmit.
        private Timer retransmitTimer;

        // Timer used to manage keep alive.
        private Timer keepaliveTimer;

        // Timer used to manage delayed ACK.
        private Timer delayedAckTimer;

        #endregion

        // Below two variables are used for the keep alive timer.
        private DateTime latestDatagramSentAt;
        private DateTime latestDatagramReceivedAt = DateTime.Now;

        #endregion Private variables

        #region Properties

        private bool autoHandle = false;

        /// <summary>
        /// Whether the RDPEUDP2 Socket is auto-handle.
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
                        InitTimers();
                    }
                    else
                    {
                        // If autoHandle changed from true to false, stop timers.
                        DisposeTimers();
                    }
                }
            }
        }

        /// <summary>
        /// The Rdpeudp2SocketConfig instance for the current connection.
        /// </summary>
        public Rdpeudp2SocketConfig SocketConfig { get; init; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initialize a new Rdpeudp2ProtocolHandler instance.
        /// </summary>
        /// <param name="autoHandle">Decide if receiver will auto handle incoming packets.</param>
        /// <param name="sender">Sender used to send packets.</param>
        /// <param name="close">Method used to close the conenction.</param>
        /// <param name="received">Method used to send bytes to higher layer.</param>
        public Rdpeudp2ProtocolHandler(bool autoHandle, Action<byte[]> sender, Action close, Action<byte[]> received)
        {
            SocketConfig = new Rdpeudp2SocketConfig();
            AutoHandle = autoHandle;

            stopwatch.Restart();

            sendBytes = sender;
            closeConnection = close;
            sendBytesToHigherLayer = received;
        }

        #endregion

        #region Methods used when auto hanle is set to true

        /// <summary>
        /// Send data via this specified UDP transport.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        /// <returns>Return true if the data was successfully sent.</returns>
        public bool Send(byte[] data)
        {
            Rdpeudp2Packet packet;
            var originalDataLength = data.Length;
            int consumedLength = 0;

            do
            {
                packet = new Rdpeudp2Packet();
                packet.Header.Flags = Rdpeudp2PacketHeaderFlags.DATA;
                packet.Header.LogWindowSize = SocketConfig.InitialLogWindowSize;

                var ack = CreateAckPayload();
                if (ack.HasValue)
                {
                    packet.Header.Flags |= Rdpeudp2PacketHeaderFlags.ACK;
                    packet.ACK = ack;
                }

                UpdateAckOfAcksPayload(packet);

                var packetBytes = packet.ToBytes();
                int dataBodyLength = Math.Min(SocketConfig.InitialMTU - packetBytes.Length - 2 - 2, data.Length);

                lock (sequenceNumberLock)
                {
                    var dataHeader = new DataHeaderPayload();
                    dataHeader.DataSeqNum = currentSenderDataSeqNum++;

                    var dataBody = new DataBodyPayload();
                    dataBody.ChannelSeqNum = currentSenderChannelSeqNum++;
                    dataBody.Data = data[0..dataBodyLength];

                    packet.DataHeader = dataHeader;
                    packet.DataBody = dataBody;
                }

                SendPacket(packet);

                consumedLength += dataBodyLength;
                if (consumedLength >= originalDataLength)
                {
                    break;
                }

                data = data[dataBodyLength..];
            } while (true);

            return true;
        }

        #endregion

        #region Methods used for manual packet processing

        /// <summary>
        /// Send a RDPEUDP2 Packet.
        /// </summary>
        /// <param name="packet">The packet to be sent.</param>
        /// <returns>Return true if the packet was successfully sent.</returns>
        public bool SendPacket(Rdpeudp2Packet packet)
        {
            if (packet.DataHeader.HasValue && packet.DataBody.HasValue && AutoHandle)
            {
                // Deal with window area.
                SenderBufferNode node = new SenderBufferNode();
                node.Packet = packet;
                node.State = SenderBufferNodeState.Pending;
                node.DataSeqNum = packet.DataHeader.Value.DataSeqNum;
                node.ChannelSeqNum = packet.DataBody.Value.ChannelSeqNum;

                DateTime endTime = DateTime.Now + this.SocketConfig.Timeout;
                while (DateTime.Now < endTime)
                {
                    // If the sender window is full, wait a SendingInterval.
                    if (senderBuffer.Count >= (1 << SocketConfig.InitialLogWindowSize))
                    {
                        senderWindowLock.WaitOne(this.SocketConfig.SendingInterval);
                    }
                    else
                    {
                        // Otherwise, send the packet.
                        break;
                    }
                }

                if (DateTime.Now > endTime)
                {
                    // Time out.
                    return false;
                }

                node.SentAt = stopwatch.ElapsedMilliseconds;
                lock (senderBuffer)
                {
                    senderBuffer.Add(node.DataSeqNum, node);
                }
            }

            byte[] data = packet.ToBytes();
            SendBytesByUdp(data);

            // Update time for the latest datagram sent, which is used for keep alive timer
            latestDatagramSentAt = DateTime.Now;

            return true;
        }

        /// <summary>
        /// Send bytes via UDP transport.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        public void SendBytesByUdp(byte[] data)
        {
            sendBytes(data);
        }

        /// <summary>
        /// Expect a RDPEUDP2 packet.
        /// </summary>
        /// <param name="timeout">The timeout for waiting the incoming packet.</param>
        /// <param name="filter">The filter to filter expected packet.</param>
        /// <returns>The expected RDPEUDP2 packet.</returns>
        public Rdpeudp2Packet ExpectPacket(TimeSpan timeout, Func<Rdpeudp2Packet, bool> filter)
        {
            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                lock (unprocessedPacketBuffer)
                {
                    for (int i = 0; i < unprocessedPacketBuffer.Count; i++)
                    {
                        Rdpeudp2Packet eudp2Packet = unprocessedPacketBuffer[i];
                        if (filter(eudp2Packet))
                        {
                            unprocessedPacketBuffer.RemoveAt(i);
                            return eudp2Packet;
                        }
                    }
                }

                // If not received a packet, wait a while.
                Thread.Sleep(Rdpeudp2SocketConfig.ReceivingInterval);
            }

            return null;
        }

        /// <summary>
        /// Expect a RDPEUDP2 packet.
        /// </summary>
        /// <param name="timeout">The timeout for waiting the incoming packet.</param>
        /// <returns>The expected RDPEUDP2 packet.</returns>
        public Rdpeudp2Packet ExpectPacket(TimeSpan timeout)
        {
            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                lock (unprocessedPacketBuffer)
                {
                    if (unprocessedPacketBuffer.Count > 0)
                    {
                        var eudp2Packet = unprocessedPacketBuffer[0];
                        unprocessedPacketBuffer.RemoveAt(0);
                        return eudp2Packet;
                    }
                }

                // If not received a packet, wait a while.
                Thread.Sleep(Rdpeudp2SocketConfig.ReceivingInterval);
            }

            return null;
        }

        /// <summary>
        /// Expect an ACK packet.
        /// </summary>
        /// <param name="timeout">The timeout for waiting the incoming packet.</param>
        /// <param name="ackedSeqNum">The data sequence number to be ACKed by the expected packet.</param>
        /// <returns>The expected ACK packet.</returns>
        public Rdpeudp2Packet ExpectAckPacket(TimeSpan timeout, uint? ackedSeqNum = null)
        {
            return ExpectPacket(timeout, ackedSeqNum.HasValue ? IsAckedExpectedSeqNum(ackedSeqNum.Value) : IsAckPacket);

            static bool IsAckPacket(Rdpeudp2Packet packet)
            {
                return packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.ACK) && packet.ACK.HasValue;
            }

            static Func<Rdpeudp2Packet, bool> IsAckedExpectedSeqNum(uint seqNum) => (Rdpeudp2Packet packet) =>
            {
                return IsAckPacket(packet) && packet.ACK.Value.GetAckedSeq().Contains(seqNum);
            };
        }

        /// <summary>
        /// Expect an ACKVEC packet.
        /// </summary>
        /// <param name="timeout">The timeout for waiting the incoming packet.</param>
        /// <returns>The expected ACKVEC packet.</returns>
        public Rdpeudp2Packet ExpectAckVecPacket(TimeSpan timeout)
        {
            return ExpectPacket(timeout, packet => packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.ACKVEC) && packet.ACKVEC.HasValue);
        }

        /// <summary>
        /// Expect a Data packet.
        /// </summary>
        /// <param name="timeout">The timeout for waiting the incoming packet.</param>
        /// <returns>The expected DATA packet.</returns>
        public Rdpeudp2Packet ExpectDataPacket(TimeSpan timeout)
        {
            return ExpectPacket(timeout, packet => packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.DATA) && packet.DataHeader.HasValue && packet.DataBody.HasValue);
        }

        /// <summary>
        /// Create a Data Packet from byte data.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        /// <param name="isDummyPacket">Whether to create a dummy packet.</param>
        /// <returns>The data packet packet to be sent.</returns>
        public Rdpeudp2Packet CreateDataPacket(byte[] data, bool isDummyPacket = false)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            var packet = new Rdpeudp2Packet(isDummyPacket);
            packet.Header.Flags = Rdpeudp2PacketHeaderFlags.DATA;
            packet.Header.LogWindowSize = SocketConfig.InitialLogWindowSize;

            var ack = CreateAckPayload();
            if (ack.HasValue)
            {
                packet.Header.Flags |= Rdpeudp2PacketHeaderFlags.ACK;
                packet.ACK = ack;
            }

            lock (sequenceNumberLock)
            {
                var dataHeader = new DataHeaderPayload();
                dataHeader.DataSeqNum = currentSenderDataSeqNum++;

                var dataBody = new DataBodyPayload();
                dataBody.ChannelSeqNum = isDummyPacket ? (ushort)0 : currentSenderChannelSeqNum++;
                dataBody.Data = data;

                packet.DataHeader = dataHeader;
                packet.DataBody = dataBody;
            }

            return packet;
        }

        /// <summary>
        /// Create an ACK payloads acording to current receiver buffer.
        /// </summary>
        /// <returns>The ACK payload can be sent currently.</returns>
        public AcknowledgementPayload? CreateAckPayload()
        {
            if (!AutoHandle)
            {
                var ack = CreateAckPayloadFromUnprocessedPacketBuffer();
                if (ack.HasValue)
                {
                    return ack;
                }
            }

            if (processedPackets.Count == 0)
            {
                return null;
            }

            var acks = CreateAckPayloads(1);
            if (acks.Count < 1)
            {
                return null;
            }

            return acks[0];
        }

        /// <summary>
        /// Create an ACK payloads acording to current unprocessed packet buffer.
        /// </summary>
        /// <returns>The ACK payload can be sent currently.</returns>
        private AcknowledgementPayload? CreateAckPayloadFromUnprocessedPacketBuffer()
        {
            var dataPacket = ExpectDataPacket(SocketConfig.Timeout);

            if (dataPacket == null)
            {
                return null;
            }

            var dataHeader = dataPacket.DataHeader.Value;
            var receivedAt = receivedAtBuffer[dataHeader.DataSeqNum];

            var ack = new AcknowledgementPayload();
            ack.SeqNum = dataHeader.DataSeqNum;
            ack.receivedTS = (uint)(receivedAt / 4);
            ack.sendAckTimeGap = (byte)Math.Min(stopwatch.ElapsedMilliseconds - receivedAt, 255);
            ack.numDelayedAcks = 0;
            ack.delayAckTimeScale = 0;
            ack.delayAckTimeAdditions = new byte[0];

            receivedAtBuffer.Remove(dataHeader.DataSeqNum);

            return ack;
        }

        /// <summary>
        /// Create several ACK payloads acording to current receiver buffer.
        /// </summary>
        /// <param name="maxCount">The maximum number of ACK payloads returned.</param>
        /// <param name="delayAck">Whether to send multiple ACKs in a packet.</param>
        /// <returns>The ACK payloads can be sent currently.</returns>
        private List<AcknowledgementPayload> CreateAckPayloads(int maxCount, bool delayAck = false)
        {
            maxCount = Math.Min(maxCount, 16);
            var acks = new List<AcknowledgementPayload>();

            lock (processedPackets)
            {
                var keys = processedPackets.Keys.ToList();
                foreach (var key in keys)
                {
                    if (key < currentReceiverAckOfAcksSeqNum)
                    {
                        processedPackets.Remove(key);
                        continue;
                    }

                    var node = processedPackets[key];
                    if (node.State == ReceiverBufferNodeState.Received)
                    {
                        var ack = new AcknowledgementPayload();
                        ack.SeqNum = node.DataSeqNum;
                        ack.receivedTS = (uint)(node.ReceivedAt / 4);
                        ack.sendAckTimeGap = (byte)Math.Min(stopwatch.ElapsedMilliseconds - node.ReceivedAt, 255);
                        ack.numDelayedAcks = 0;
                        ack.delayAckTimeScale = 0;
                        ack.delayAckTimeAdditions = new byte[0];

                        processedPackets.Remove(key);

                        acks.Add(ack);
                        if (acks.Count == maxCount)
                        {
                            break;
                        }
                    }
                }
            }

            if (acks.Count > 1 && delayAck)
            {
                var delayedAck = acks.Last();

                var maxGapValue = (delayedAck.receivedTS * 4 + delayedAck.sendAckTimeGap) - (acks.First().receivedTS * 4);
                var scale = DetermineDelayAckTimeScale(maxGapValue);

                delayedAck.numDelayedAcks = (byte)(acks.Count - 1);
                delayedAck.delayAckTimeScale = scale;
                delayedAck.delayAckTimeAdditions = new byte[acks.Count - 1];
                for (var i = 0; i < acks.Count - 1; i++)
                {
                    var addtion = ((delayedAck.receivedTS * 4 + delayedAck.sendAckTimeGap) - (acks[acks.Count - 2 - i].receivedTS * 4)) / (1 << scale);
                    delayedAck.delayAckTimeAdditions[i] = (byte)Math.Min(addtion, 255);
                }

                return new List<AcknowledgementPayload>(new AcknowledgementPayload[] { delayedAck });
            }

            return acks;

            static byte DetermineDelayAckTimeScale(uint gapValue)
            {
                byte i = 0;
                for (; i <= 15; i++)
                {
                    // All elements in delayAckTimeAdditions fit between 0 and 255.
                    if ((gapValue / (1 << i)) <= 255)
                    {
                        break;
                    }
                }

                return i;
            }
        }

        /// <summary>
        /// Retransmit the lost packet.
        /// </summary>
        /// <param name="packet">The lost packet to be retransmitted.</param>
        /// <returns>Return true if the packet was successfully restransmmitted.</returns>
        public bool RetransmitPacket(Rdpeudp2Packet packet)
        {
            if (packet.IsDummyPacket)
            {
                return false;
            }

            if (!packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.DATA) || !packet.DataHeader.HasValue || !packet.DataBody.HasValue)
            {
                return false;
            }

            var dataHeader = packet.DataHeader.Value;

            lock (senderBuffer)
            {
                if (!senderBuffer.ContainsKey(dataHeader.DataSeqNum))
                {
                    return false;
                }

                senderBuffer.Remove(dataHeader.DataSeqNum);
            }

            lock (sequenceNumberLock)
            {
                var newDataHeader = dataHeader;
                newDataHeader.DataSeqNum = currentSenderDataSeqNum++;
                packet.DataHeader = newDataHeader;
            }

            var ack = CreateAckPayload();
            if (ack.HasValue)
            {
                packet.Header.Flags |= Rdpeudp2PacketHeaderFlags.ACK;
                packet.ACK = ack;
            }
            else
            {
                packet.Header.Flags &= ~Rdpeudp2PacketHeaderFlags.ACK;
                packet.ACK = null;
            }

            SendPacket(packet);

            return true;
        }

        /// <summary>
        /// Send an ACK packet.
        /// </summary>
        /// <returns>Return true if the packet was successfully sent.</returns>
        public bool SendAckPacket()
        {
            if (!processedPackets.Any())
            {
                return false;
            }

            var ackPacket = new Rdpeudp2Packet();
            ackPacket.Header.Flags = Rdpeudp2PacketHeaderFlags.ACK;
            ackPacket.Header.LogWindowSize = SocketConfig.InitialLogWindowSize;
            ackPacket.ACK = CreateAckPayload();

            if (!ackPacket.ACK.HasValue)
            {
                return false;
            }

            SendPacket(ackPacket);

            return true;
        }

        /// <summary>
        /// Send a dummy data packet to keep alive.
        /// </summary>
        /// <returns>Return true if the packet was successfully sent.</returns>
        public bool SendKeepalivePacket()
        {
            var keepalivePacket = CreateDataPacket(new byte[100], true);

            UpdateAckOfAcksPayload(keepalivePacket);

            SendPacket(keepalivePacket);

            return true;
        }

        /// <summary>
        /// Method used to process a received packet.
        /// </summary>
        /// <param name="packet">The packet to be processed.</param>
        public void ReceivePacket(StackPacket packet)
        {
            var eudp2Packet = new Rdpeudp2Packet(packet.ToBytes(), out _);

            ReceivePacket(eudp2Packet);
        }

        /// <summary>
        /// Method used to process a received packet.
        /// </summary>
        /// <param name="eudp2Packet">The packet to be processed.</param>
        public void ReceivePacket(Rdpeudp2Packet eudp2Packet)
        {
            // Update time for the latest datagram received, which is used for keep alive timer.
            latestDatagramReceivedAt = DateTime.Now;

            if (!AutoHandle)
            {
                // If AutoHandle is false, this packet will not be processed automatically by this method. 
                // We will buffer it for others to use it.
                lock (unprocessedPacketBuffer)
                {
                    unprocessedPacketBuffer.Add(eudp2Packet);

                    if (eudp2Packet.DataHeader.HasValue)
                    {
                        receivedAtBuffer[eudp2Packet.DataHeader.Value.DataSeqNum] = stopwatch.ElapsedMilliseconds;
                    }
                }
            }
            else
            {
                // Process the received packet.
                // In case the advised window size was updated.
                SocketConfig.InitialLogWindowSize = eudp2Packet.Header.LogWindowSize;

                // Process Ack payload.
                ProcessAckPayload(eudp2Packet);

                // Process AckVec payload.
                ProcessAckVecPayload(eudp2Packet);

                // Process DataHeader and DataBody payloads.
                ProcessDataHeaderAndDataBodyPayloads(eudp2Packet);

                // Process AOA payload.
                ProcessAckOfAcksPayload(eudp2Packet);

                // Process OverheadSize payload.
                ProcessOverheadSizePayload(eudp2Packet);

                // Process DelayAckInfo payload.
                ProcessDelayAckInfoPayload(eudp2Packet);
            }
        }

        #endregion

        #region Packet processing methods

        public void ProcessOverheadSizePayload(Rdpeudp2Packet eudp2Packet)
        {
            if (eudp2Packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.OVERHEADSIZE) && eudp2Packet.OverheadSize.HasValue)
            {
                // TODO: Investigate when and how to use the OverheadSize value.
                SocketConfig.OverheadSize = eudp2Packet.OverheadSize.Value.OverheadSize;
            }
        }

        public void ProcessDelayAckInfoPayload(Rdpeudp2Packet eudp2Packet)
        {
            if (eudp2Packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.DELAYACKINFO) && eudp2Packet.DelayAckInfo.HasValue)
            {
                SocketConfig.MaxDelayedAcks = eudp2Packet.DelayAckInfo.Value.MaxDelayedAcks;
                SocketConfig.DelayedAckTimeoutInMs = eudp2Packet.DelayAckInfo.Value.DelayAckTimeoutInMs;
            }
        }

        public void ProcessAckOfAcksPayload(Rdpeudp2Packet eudp2Packet)
        {
            if (!eudp2Packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.AOA) || !eudp2Packet.AckOfAcks.HasValue)
            {
                return;
            }

            var aoaSeqNum = eudp2Packet.AckOfAcks.Value.AckOfAcksSeqNum;

            lock (updateReceiverAOASeqNumLock)
            {
                currentReceiverAckOfAcksSeqNum = aoaSeqNum;
            }

            lock (receiverBuffer)
            {
                foreach (var node in receiverBuffer)
                {
                    if (node.Key < aoaSeqNum)
                    {
                        node.Value.State = ReceiverBufferNodeState.Received;
                    }
                }

                // Update receiver buffer to fit current receiver window.
                UpdateReceiverBuffer();

                // Send higher layer data.
                SendBytesToHigherLayer();
            }
        }

        public void ProcessAckPayload(Rdpeudp2Packet eudp2Packet)
        {
            if (!eudp2Packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.ACK) || !eudp2Packet.ACK.HasValue)
            {
                return;
            }

            lock (senderBuffer)
            {
                var ackedSeq = eudp2Packet.ACK.Value.GetAckedSeq();
                foreach (var ackedSeqNum in ackedSeq)
                {
                    if (senderBuffer.ContainsKey(ackedSeqNum))
                    {
                        senderBuffer[ackedSeqNum].State = SenderBufferNodeState.Received;
                    }

                    if (ackedSeqNum >= currentSenderAckOfAcksSeqNum)
                    {
                        shouldSendAckOfAcksPayload = false;
                    }
                }

                UpdateSenderBuffer();
            }
        }

        public void ProcessAckVecPayload(Rdpeudp2Packet eudp2Packet)
        {
            if (!eudp2Packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.ACKVEC) || !eudp2Packet.ACKVEC.HasValue)
            {
                return;
            }

            lock (senderBuffer)
            {
                var ackeqSeq = eudp2Packet.ACKVEC.Value.GetAckedSeq();
                foreach (var ackedSeqNum in ackeqSeq)
                {
                    if (senderBuffer.ContainsKey(ackedSeqNum))
                    {
                        senderBuffer[ackedSeqNum].State = SenderBufferNodeState.Received;
                    }

                    if (ackedSeqNum >= currentSenderAckOfAcksSeqNum)
                    {
                        shouldSendAckOfAcksPayload = false;
                    }

                    UpdateSenderBuffer();
                }
            }
        }

        public void ProcessDataHeaderAndDataBodyPayloads(Rdpeudp2Packet eudp2Packet)
        {
            if (!eudp2Packet.Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.DATA) || !eudp2Packet.DataHeader.HasValue || !eudp2Packet.DataBody.HasValue)
            {
                return;
            }

            var dataHeader = eudp2Packet.DataHeader.Value;
            var dataBody = eudp2Packet.DataBody.Value;

            lock (receiverBuffer)
            {
                if (receiverBuffer.ContainsKey(dataHeader.DataSeqNum))
                {
                    receiverBuffer[dataHeader.DataSeqNum].Packet = eudp2Packet;
                    receiverBuffer[dataHeader.DataSeqNum].ReceivedAt = stopwatch.ElapsedMilliseconds;
                    receiverBuffer[dataHeader.DataSeqNum].State = ReceiverBufferNodeState.Received;
                    receiverBuffer[dataHeader.DataSeqNum].ChannelSeqNum = dataBody.ChannelSeqNum;
                }
                else
                {
                    var newNode = new ReceiverBufferNode();
                    newNode.Packet = eudp2Packet;
                    newNode.ReceivedAt = stopwatch.ElapsedMilliseconds;
                    newNode.State = ReceiverBufferNodeState.Received;
                    newNode.DataSeqNum = dataHeader.DataSeqNum;
                    newNode.ChannelSeqNum = dataBody.ChannelSeqNum;

                    receiverBuffer.Add(dataHeader.DataSeqNum, newNode);
                }

                // Add pending nodes to the receiver buffer.
                MakeReceiverWindowGaps();

                // Update receiver buffer to fit current receiver window.
                UpdateReceiverBuffer();

                // Send higher layer data.
                SendBytesToHigherLayer();
            }
        }

        private void MakeReceiverWindowGaps()
        {
            lock (receiverBuffer)
            {
                if (receiverBuffer.Count == 0)
                {
                    return;
                }

                // TODO: Update receiver window status by sending an ACKVEC packet back.
                var gaps = GetGaps(receiverBuffer);
                foreach (var gap in gaps)
                {
                    var gapNode = new ReceiverBufferNode();
                    gapNode.DataSeqNum = (ushort)gap;
                    gapNode.State = ReceiverBufferNodeState.Pending;

                    receiverBuffer.Add(gap, gapNode);
                }
            }
        }

        private void MakeReceivedDataGaps()
        {
            lock (receivedDataBuffer)
            {
                if (receivedDataBuffer.Count == 0)
                {
                    return;
                }

                var gaps = GetGaps(receivedDataBuffer);
                foreach (var gap in gaps)
                {
                    receivedDataBuffer.Add(gap, null);
                }
            }
        }

        private HashSet<uint> GetGaps<T>(SortedList<uint, T> buffer)
        {
            var keys = buffer.Keys.ToList();
            var (start, end) = (keys.First(), keys.Last());
            var seq = Enumerable.Range((int)start, (int)(end - start + 1)).Select(i => (uint)i);
            var gaps = new HashSet<uint>(seq);
            gaps.ExceptWith(keys);

            return gaps;
        }

        private void SendAckPacketsBack(bool sendImmediately = false)
        {
            lock (processedPackets)
            {
                if (processedPackets.Any() && !sendImmediately)
                {
                    var firstNode = processedPackets.First();
                    var timeoutReached = stopwatch.ElapsedMilliseconds - firstNode.Value.ReceivedAt >= SocketConfig.DelayedAckTimeoutInMs;
                    var maxCountReached = processedPackets.Count >= 1 + SocketConfig.MaxDelayedAcks;

                    if (!(timeoutReached || maxCountReached))
                    {
                        return;
                    }
                }

                while (processedPackets.Any())
                {
                    var acks = CreateAckPayloads(1 + SocketConfig.MaxDelayedAcks, delayAck: true);
                    foreach (var ack in acks)
                    {
                        var packet = new Rdpeudp2Packet();
                        packet.Header.Flags = Rdpeudp2PacketHeaderFlags.ACK;
                        packet.Header.LogWindowSize = SocketConfig.InitialLogWindowSize;
                        packet.ACK = ack;

                        SendPacket(packet);
                    }
                }
            }
        }

        private void SendBytesToHigherLayer()
        {
            lock (receivedDataBuffer)
            {
                if (receivedDataBuffer.Count == 0)
                {
                    return;
                }

                while (receivedDataBuffer.ContainsKey(currentReceiverChannelSeqNum))
                {
                    sendBytesToHigherLayer(receivedDataBuffer[currentReceiverChannelSeqNum]);
                    receivedDataBuffer.Remove(currentReceiverChannelSeqNum);

                    currentReceiverChannelSeqNum++;
                }
            }
        }

        /// <summary>
        /// Update receiver buffer, process and remove received packets.
        /// </summary>
        private void UpdateReceiverBuffer()
        {
            lock (receiverBuffer)
            {
                if (receiverBuffer.Count == 0)
                {
                    return;
                }

                var firstNode = receiverBuffer.First();
                if (firstNode.Value.State == ReceiverBufferNodeState.Pending)
                {
                    return;
                }

                var keys = receiverBuffer.Keys.ToList();
                foreach (var key in keys)
                {
                    var node = receiverBuffer[key];
                    if (node.State == ReceiverBufferNodeState.Received)
                    {
                        var packet = node.Packet;
                        if ((packet != null) && !packet.IsDummyPacket)
                        {
                            UpdateReceivedDataBuffer(packet);
                        }

                        receiverBuffer.Remove(key);

                        lock (processedPackets)
                        {
                            processedPackets[key] = node;

                            if (!packet.IsDummyPacket)
                            {
                                SendAckPacketsBack(sendImmediately: true);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            void UpdateReceivedDataBuffer(Rdpeudp2Packet packet)
            {
                lock (receivedDataBuffer)
                {
                    var dataBody = packet.DataBody.Value;
                    if (!isFirstDataPacketReceived)
                    {
                        isFirstDataPacketReceived = true;
                        currentReceiverChannelSeqNum = dataBody.ChannelSeqNum;
                    }
                    else
                    {
                        if (dataBody.ChannelSeqNum < currentReceiverChannelSeqNum)
                        {
                            // Ignore retransmitted packet that its data were previously forwarded.
                            return;
                        }
                    }

                    receivedDataBuffer[dataBody.ChannelSeqNum] = dataBody.Data;

                    // Add pending nodes to the received data buffer.
                    MakeReceivedDataGaps();
                }
            }
        }

        /// <summary>
        /// Update sender buffer, remove acknowledged packets.
        /// </summary>
        private void UpdateSenderBuffer()
        {
            lock (senderBuffer)
            {
                if (senderBuffer.Count == 0)
                {
                    return;
                }

                var keys = senderBuffer.Keys.ToList();
                var hasLostPackets = false;

                foreach (var key in keys)
                {
                    if (senderBuffer[key].State == SenderBufferNodeState.Received)
                    {
                        senderBuffer.Remove(key);
                    }
                    else if (senderBuffer[key].State == SenderBufferNodeState.Pending)
                    {
                        // TODO: Update loss detection logic to fit the Windows client behaviors.
                        if (stopwatch.ElapsedMilliseconds - senderBuffer[key].SentAt >= (SocketConfig.DelayedAckTimeoutInMs + SocketConfig.RetransmitDuration.TotalMilliseconds))
                        {
                            hasLostPackets = true;
                            senderBuffer[key].State = SenderBufferNodeState.Lost;
                        }
                        else
                        {
                            if (hasLostPackets)
                            {
                                lock (updateSenderAOASeqNumLock)
                                {
                                    currentSenderAckOfAcksSeqNum = senderBuffer[key].DataSeqNum;
                                    shouldSendAckOfAcksPayload = true;
                                }
                            }

                            break;
                        }
                    }
                }

                senderWindowLock.Set();
            }
        }

        /// <summary>
        /// Used during sending data packet, Add AckOfAcks payload to it if possible.
        /// </summary>
        /// <param name="eudp2Packet">Packet to be sent</param>
        private void UpdateAckOfAcksPayload(Rdpeudp2Packet eudp2Packet)
        {
            lock (updateSenderAOASeqNumLock)
            {
                if (!shouldSendAckOfAcksPayload)
                {
                    return;
                }

                var aoaPayload = new AckOfAcksPayload();
                aoaPayload.AckOfAcksSeqNum = currentSenderAckOfAcksSeqNum;

                eudp2Packet.Header.Flags |= Rdpeudp2PacketHeaderFlags.AOA;
                eudp2Packet.AckOfAcks = aoaPayload;
            }
        }

        #region Other public methods

        /// <summary>
        /// Stop this handler.
        /// </summary>
        public void Close()
        {
            DisposeTimers();
        }

        /// <summary>
        /// Dispose this handler.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        #region Methods for timers

        /// <summary>
        /// Create Timers for RDPEUDP2 socket.
        /// </summary>
        private void InitTimers()
        {
            // Wrap handlers in try-catch blocks to ignore exceptions thrown in handler threads.
            retransmitTimer = new Timer((object state) => { try { ManageRetransmit(state); } catch { } }, null, 0, this.SocketConfig.RetransmitTimerInterval);
            keepaliveTimer = new Timer((object state) => { try { ManageKeepalive(state); } catch { } }, null, 0, this.SocketConfig.KeepaliveTimerInterval);
            delayedAckTimer = new Timer((object state) => { try { ManageDelayedAck(state); } catch { } }, null, 0, this.SocketConfig.DelayedAckInterval);
        }

        /// <summary>
        /// Dispose timers of RDPEUDP2 handler.
        /// </summary>
        private void DisposeTimers()
        {
            retransmitTimer?.Dispose();
            keepaliveTimer?.Dispose();
            delayedAckTimer?.Dispose();
        }

        /// <summary>
        /// Function used to manage retransmit periodically.
        /// </summary>
        private void ManageRetransmit(object _)
        {
            if (!AutoHandle)
            {
                return;
            }

            lock (senderBuffer)
            {
                UpdateSenderBuffer();

                if (senderBuffer.Count == 0)
                {
                    return;
                }

                var firstNode = senderBuffer.First();
                if (firstNode.Value.State != SenderBufferNodeState.Lost)
                {
                    return;
                }

                var keys = senderBuffer.Keys.ToList();
                foreach (var key in keys)
                {
                    if (senderBuffer[key].State == SenderBufferNodeState.Lost)
                    {
                        RetransmitPacket(senderBuffer[key].Packet);
                        senderBuffer.Remove(key);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Function used to manage keep alive timer.
        /// </summary>
        private void ManageKeepalive(object _)
        {
            if (!AutoHandle)
            {
                return;
            }

            if (latestDatagramReceivedAt + this.SocketConfig.LostConnectionTimeout <= DateTime.Now)
            {
                closeConnection();
                return;
            }

            if (latestDatagramSentAt + this.SocketConfig.KeepaliveDuration <= DateTime.Now)
            {
                // Send a packet to keep alive.
                SendKeepalivePacket();
            }
        }

        /// <summary>
        /// Function used to manage Delayed ACK timer.
        /// </summary>
        private void ManageDelayedAck(object _)
        {
            if (!AutoHandle)
            {
                return;
            }

            SendAckPacketsBack();
        }

        #endregion

        #endregion
    }
}
