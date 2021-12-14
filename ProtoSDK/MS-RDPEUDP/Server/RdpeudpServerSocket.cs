// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2;
using System;
using System.Net;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    public class RdpeudpServerSocket : RdpeudpSocket
    {
        private RdpeudpServer rdpeudpServer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mode">Reliable or Lossy.</param>
        /// <param name="remoteEp">Remote endpoint.</param>
        /// <param name="autoHandle">Auto Handle or not.</param>
        /// <param name="sender">Sender Used to send packet</param>
        /// <param name="rdpeudpServer">RDPEUDP Server</param>
        public RdpeudpServerSocket(TransportMode mode, IPEndPoint remoteEp, bool autohandle, RdpeudpSocketSender sender, RdpeudpServer rdpeudpServer)
            : base(mode, remoteEp, autohandle, sender)
        {
            this.rdpeudpServer = rdpeudpServer;
        }

        /// <summary>
        /// Expect a RDPEUDP Connection
        /// </summary>
        /// <param name="mode">mode of the connection</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool ExpectConnect(TimeSpan timeout)
        {
            DateTime endtime = DateTime.Now + timeout;
            RdpeudpPacket eudpPacket;
            eudpPacket = ExpectSynPacket(timeout);
            // Verify SYN Packet
            if (eudpPacket == null)
            {
                return false;
            }

            SendSynAndAckPacket();

            if (!upgradedToRdpedup2)
            {
                eudpPacket = ExpectAckPacket(endtime - DateTime.Now);
                // Verify the ACK Packet
                if (eudpPacket == null || eudpPacket.FecHeader.snSourceAck != SnInitialSequenceNumber)
                {
                    return false;
                }

                // If the ACK packet contains source data, process it again after connected is true
                if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA))
                {
                    ReceivePacket(eudpPacket);
                }
            }

            // Connection has been established
            Connected = true;

            return true;
        }
        /// <summary>
        /// Send a SYN and ACK diagram
        /// </summary>
        /// <param name="initSequenceNumber">Specify an initial sequence number</param>
        /// <returns></returns>
        public bool SendSynAndAckPacket(uint? initSequenceNumber = null, RDPUDP_PROTOCOL_VERSION? version = null)
        {
            if (Connected)
            {
                return false;
            }

            // Lock the ReceivePacket method if it is transferring to RDPEUDP2 to avoid processing RDPEUDP2 packets with the RDPEUDP logic.
            lock (receiveLock)
            {
                RdpeudpPacket synAndAckPacket = new RdpeudpPacket();
                synAndAckPacket.FecHeader.snSourceAck = SnSourceAck;
                synAndAckPacket.FecHeader.uReceiveWindowSize = UReceiveWindowSize;
                synAndAckPacket.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_SYN | RDPUDP_FLAG.RDPUDP_FLAG_ACK;
                if (version.HasValue)
                {
                    synAndAckPacket.FecHeader.uFlags |= RDPUDP_FLAG.RDPUDP_FLAG_SYNEX;
                    synAndAckPacket.SynDataEx = CreateSynExData((RDPUDP_PROTOCOL_VERSION)version);
                }
                synAndAckPacket.SynData = CreateSynData(initSequenceNumber);
                SendPacket(synAndAckPacket);
                // Set the OutSnResetSeqNum value, number from which the receive thread decoding the state of the send packet.
                OutSnResetSeqNum = synAndAckPacket.SynData.Value.snInitialSequenceNumber + 1;

                if (version.HasValue && version.Value.HasFlag(RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3))
                {
                    upgradedToRdpedup2 = true;
                    DisposeTimers();
                    rdpeudp2Handler = new Rdpeudp2ProtocolHandler(AutoHandle, SendBytesByUdp, Close, ReceiveDataOnHigherLayer);
                    rdpeudp2Handler.SocketConfig.RetransmitDuration = RTT * 2;
                    rdpeudp2Handler.SocketConfig.DelayedAckTimeoutInMs = (int)(RTT.TotalMilliseconds / 2);
                }
            }

            return true;
        }

        /// <summary>
        /// Expect a Syn Packet
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RdpeudpPacket ExpectSynPacket(TimeSpan timeout)
        {
            if (Connected) return null;
            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                lock (unprocessedPacketBuffer)
                {
                    for (int i = 0; i < unprocessedPacketBuffer.Count; i++)
                    {
                        RdpeudpPacket eudpPacket = unprocessedPacketBuffer[i];
                        RDPUDP_FLAG expectFlag = RDPUDP_FLAG.RDPUDP_FLAG_SYN;
                        if (this.TransMode == TransportMode.Lossy)
                        {
                            expectFlag |= RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY;
                        }
                        if (eudpPacket.FecHeader.uFlags.HasFlag(expectFlag))
                        {
                            unprocessedPacketBuffer.RemoveAt(i);

                            // Analyse the SYN packet.
                            ProcessSynPacket(eudpPacket);

                            return eudpPacket;
                        }
                    }

                }
                // If not receive a Packet, wait a while 
                Thread.Sleep(RdpeudpSocketConfig.ReceivingInterval);
            }
            return null;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
            rdpeudpServer.RemoveSocket(this);
        }
    }
}
