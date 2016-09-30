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
            
            eudpPacket = ExpectACKPacket(endtime - DateTime.Now);
            // Verify the ACK Packet
            if (eudpPacket == null || eudpPacket.fecHeader.snSourceAck != SnInitialSequenceNumber)
            {
                return false;
            }
                        
            // If the ACK packet contains source data, process it again after connected is true
            if (eudpPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA))
            {
                ReceivePacket(eudpPacket);
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
        public bool SendSynAndAckPacket(uint? initSequenceNumber = null, uUdpVer_Values? version = null)
        {
            if (Connected) return false;
            RdpeudpPacket SynAndAckPacket = new RdpeudpPacket();
            SynAndAckPacket.fecHeader.snSourceAck = SnSourceAck;
            SynAndAckPacket.fecHeader.uReceiveWindowSize = UReceiveWindowSize;
            SynAndAckPacket.fecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_SYN | RDPUDP_FLAG.RDPUDP_FLAG_ACK;
            if(version != null)
            {
                SynAndAckPacket.fecHeader.uFlags |= RDPUDP_FLAG.RDPUDP_FLAG_SYNEX;
                SynAndAckPacket.SynDataEx = CreateSynExData((uUdpVer_Values)version);                
            }
            SynAndAckPacket.SynData = CreateSynData(initSequenceNumber);
            SendPacket(SynAndAckPacket);
            // Set the OutSnAckOfAcksSeqNum value, number from which the receive thread decoding the state of the send packet.
            OutSnAckOfAcksSeqNum = SynAndAckPacket.SynData.Value.snInitialSequenceNumber + 1;
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
                lock (unProcessedPacketBuffer)
                {
                    for (int i=0; i < unProcessedPacketBuffer.Count; i++)
                    {
                        RdpeudpPacket eudpPacket = unProcessedPacketBuffer[i];
                        RDPUDP_FLAG expectFlag = RDPUDP_FLAG.RDPUDP_FLAG_SYN;
                        if (this.TransMode == TransportMode.Lossy)
                        {
                            expectFlag |= RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY;
                        }
                        if (eudpPacket.fecHeader.uFlags.HasFlag(expectFlag))
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
