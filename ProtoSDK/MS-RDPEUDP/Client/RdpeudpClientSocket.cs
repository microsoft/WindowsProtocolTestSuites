// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    public class RdpeudpClientSocket : RdpeudpSocket
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mode">Reliable or Lossy.</param>
        /// <param name="remoteEp">Remote endpoint.</param>
        /// <param name="autoHandle">Auto Handle or not.</param>
        /// <param name="sender">Sender Used to send packet</param>
        public RdpeudpClientSocket(TransportMode mode, IPEndPoint remoteEp, bool autohandle, RdpeudpSocketSender sender)
            : base(mode, remoteEp, autohandle, sender)
        {
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
            this.SendAcKPacket();

            return true;
        }

        /// <summary>
        /// Send SYN Packet to init a RDPEUDP Connection
        /// </summary>
        /// <param name="initSequenceNumber">Specify a snInitialSequenceNumber</param>
        /// <returns></returns>
        public bool SendSynPacket(uint? initSequenceNumber = null)
        {
            if (Connected) return false;
            RdpeudpPacket SynAndAckPacket = new RdpeudpPacket();
            SynAndAckPacket.fecHeader.snSourceAck = UInt32.MaxValue;
            SynAndAckPacket.fecHeader.uReceiveWindowSize = UReceiveWindowSize;
            SynAndAckPacket.fecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_SYN;
            if (this.TransMode == TransportMode.Lossy)
            {
                SynAndAckPacket.fecHeader.uFlags |= RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY;
            }
            SynAndAckPacket.SynData = CreateSynData(initSequenceNumber);
            SendPacket(SynAndAckPacket);
            // Set the OutSnAckOfAcksSeqNum value, number from which the receive thread decoding the state of the send packet.
            OutSnAckOfAcksSeqNum = SynAndAckPacket.SynData.Value.snInitialSequenceNumber + 1;
            return true;
        }

        /// <summary>
        /// Expect a SYN and ACK Packet
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RdpeudpPacket ExpectSynAndAckPacket(TimeSpan timeout)
        {
            if (Connected) return null;
            DateTime endtime = DateTime.Now + timeout;
            while (DateTime.Now < endtime)
            {
                lock (unProcessedPacketBuffer)
                {
                    for (int i = 0; i < unProcessedPacketBuffer.Count; i++)
                    {
                        RdpeudpPacket eudpPacket = unProcessedPacketBuffer[i];                        
                        if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYN)
                            && eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK))
                        {
                            unProcessedPacketBuffer.RemoveAt(i);

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
