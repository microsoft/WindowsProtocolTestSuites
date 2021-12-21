// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2;
using System;
using System.Net;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    public class RdpeudpClientSocket : RdpeudpSocket
    {
        private byte[] cookieHash;

        /// <summary>
        /// Initialize a new RdpeudpClientSocket instance.
        /// </summary>
        /// <param name="mode">Reliable or Lossy.</param>
        /// <param name="remoteEp">Remote endpoint.</param>
        /// <param name="autoHandle">Auto Handle or not.</param>
        /// <param name="sender">Sender Used to send packet</param>
        /// <param name="cookieHash">the SHA-256 hash of the data that was transmitted from the server to the client in the securityCookie field of the Initiate Multitransport Request PDU.</param>
        public RdpeudpClientSocket(TransportMode mode, IPEndPoint remoteEp, bool autoHandle, RdpeudpSocketSender sender, byte[] cookieHash)
            : base(mode, remoteEp, autoHandle, sender)
        {
            this.cookieHash = cookieHash;
        }

        /// <summary>
        /// Establish Connection
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Connect(TimeSpan timeout)
        {
            SendSynPacket();
            RdpeudpPacket eudpPacket = ExpectSynAndAckPacket(timeout);
            if (eudpPacket == null)
            {
                return false;
            }

            Connected = true;

            if (!upgradedToRdpedup2)
            {
                this.SendAckPacket();
            }

            return true;
        }

        /// <summary>
        /// Send SYN Packet to init a RDPEUDP Connection
        /// </summary>
        /// <param name="initSequenceNumber">Specify a snInitialSequenceNumber</param>
        /// <returns></returns>
        public bool SendSynPacket(uint? initSequenceNumber = null, RDPUDP_PROTOCOL_VERSION? version = null)
        {
            if (Connected)
            {
                return false;
            }

            RdpeudpPacket synPacket = new RdpeudpPacket();
            synPacket.FecHeader.snSourceAck = UInt32.MaxValue;
            synPacket.FecHeader.uReceiveWindowSize = UReceiveWindowSize;
            synPacket.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_SYN;
            if (this.TransMode == TransportMode.Lossy)
            {
                synPacket.FecHeader.uFlags |= RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY;
            }
            synPacket.SynData = CreateSynData(initSequenceNumber);
            if (version.HasValue)
            {
                synPacket.FecHeader.uFlags |= RDPUDP_FLAG.RDPUDP_FLAG_SYNEX;
                var synDataEx = CreateSynExData((RDPUDP_PROTOCOL_VERSION)version);
                if (version.Value == RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3)
                {
                    synDataEx.cookieHash = cookieHash;
                }
                synPacket.SynDataEx = synDataEx;
            }
            SendPacket(synPacket);
            // Set the OutSnResetSeqNum value, number from which the receive thread decoding the state of the send packet.
            OutSnResetSeqNum = synPacket.SynData.Value.snInitialSequenceNumber + 1;
            return true;
        }

        /// <summary>
        /// Expect a SYN and ACK Packet
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RdpeudpPacket ExpectSynAndAckPacket(TimeSpan timeout)
        {
            if (Connected)
            {
                return null;
            }

            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                // Lock the ReceivePacket method if it is transferring to RDPEUDP2 to avoid processing RDPEUDP2 packets with the RDPEUDP logic.
                lock (receiveLock)
                {
                    for (int i = 0; i < unprocessedPacketBuffer.Count; i++)
                    {
                        RdpeudpPacket eudpPacket = unprocessedPacketBuffer[i];
                        if (eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYN)
                            && eudpPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK))
                        {
                            unprocessedPacketBuffer.RemoveAt(i);

                            if (eudpPacket.SynDataEx.HasValue)
                            {
                                if (eudpPacket.SynDataEx.Value.uUdpVer == RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3)
                                {
                                    upgradedToRdpedup2 = true;
                                    DisposeTimers();
                                    rdpeudp2Handler = new Rdpeudp2ProtocolHandler(AutoHandle, SendBytesByUdp, Close, ReceiveDataOnHigherLayer);
                                    rdpeudp2Handler.SocketConfig.RetransmitDuration = RTT * 2;
                                    rdpeudp2Handler.SocketConfig.DelayedAckTimeoutInMs = (int)(RTT.TotalMilliseconds / 2);
                                    return eudpPacket;
                                }
                            }

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
    }
}
