// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.Rdpeudp
{
    public partial class RdpeudpTestSuite : RdpTestClassBase
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.6")]
        [TestCategory("RDPEUDP2")]
        [TestCategory("BasicRequirement")]
        [TestCategory("BasicFeature")]
        [Description("Verify that the RDPUDP2 Packet sent and an acknowlegdement is received as shared in the Sending Data Packets Without Packet Loss or Reordering section of the RDPEUDP2 document")]
        public void S3_DataTransfer_V2_ClientReceiveData()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection...");
            StartRDPConnection(enableRdpeudp2: true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish the RDPEMT connection.");
            DoUntilSucceed(() =>
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", rdpeudp2TransportMode);
                this.EstablishUDPConnection(rdpeudp2TransportMode, waitTime, verifyPacket: true, uUdpVer: RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", rdpeudp2TransportMode);
                return this.EstablishRdpemtConnection(rdpeudp2TransportMode, waitTime);
            },
            this.waitTime * 5,
            TimeSpan.FromSeconds(0.5),
            "RDPEMT tunnel creation failed");

            this.TestSite.Log.Add(LogEntryKind.Comment, "In the RDP-UDP connection, test suite prepare one RDPUDP2 Packet with the packet values given in the Data Example Sending Data Packet piggybacked with ACK section of the RDPEUDP2 document.");
            var nextUdpPacket = this.GetNextValidUdp2Packet();
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The DataSeqNum of next valid RDPEUDP2 packet is {nextUdpPacket.DataHeader.Value.DataSeqNum}.");
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The ACK payload of next valid RDPEUDP2 packet is to acknowledge {nextUdpPacket.ACK.Value.SeqNum}.");

            this.SendPacket(nextUdpPacket);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect a RDPUDP2 Packet to acknowledge the receipt of the RDPUDP2 Packet and check if the Sequence Number is equal to the ACK packet Sequence Number.");
            var ackPacket = rdpeudpSocketR.Rdpeudp2Handler.ExpectAckPacket(this.waitTime, nextUdpPacket.DataHeader.Value.DataSeqNum);
            var dataSeqNum = ackPacket.ACK.Value.SeqNum;

            this.TestSite.Log.Add(LogEntryKind.Debug, $"The recvied ACK packet is to acknowledge DataSeqNum {dataSeqNum}.");
            Site.Assert.IsNotNull(ackPacket, "Client should send an ACK to acknowledge the receipt of data packet. Transport mode is {0}.", rdpeudp2TransportMode);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.6")]
        [TestCategory("RDPEUDP2")]
        [TestCategory("BasicRequirement")]
        [TestCategory("BasicFeature")]
        [Description("Verify that the final OnWire version of the RDPUDP2 Packet sent is evaluated to a specific output as shared in the Data Example Sending Data Packet piggybacked with ACK section of the RDPEUDP2 document")]
        public void S3_DataTransfer_V2_DataPacketWithAck()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection...");
            StartRDPConnection(enableRdpeudp2: true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish the RDPEMT connection.");
            DoUntilSucceed(() =>
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", rdpeudp2TransportMode);
                this.EstablishUDPConnection(rdpeudp2TransportMode, waitTime, verifyPacket: true, uUdpVer: RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", rdpeudp2TransportMode);
                return this.EstablishRdpemtConnection(rdpeudp2TransportMode, waitTime);
            },
            this.waitTime * 5,
            TimeSpan.FromSeconds(0.5),
            "RDPEMT tunnel creation failed");

            this.TestSite.Log.Add(LogEntryKind.Comment, "In the RDP-UDP connection, test suite prepare one RDPUDP2 Packet with the packet values given in the Data Example Sending Data Packet piggybacked with ACK section of the RDPEUDP2 document.");
            var nextUdpPacket = this.GetNextValidUdp2Packet();
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The DataSeqNum of next valid RDPEUDP2 packet is {nextUdpPacket.DataHeader.Value.DataSeqNum}.");
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The ACK payload of next valid RDPEUDP2 packet is to acknowledge {nextUdpPacket.ACK.Value.SeqNum}.");

            this.TestSite.Log.Add(LogEntryKind.Comment, $"Attach an AckOfAcks payload with the SeqNum {nextUdpPacket.DataHeader.Value.DataSeqNum} to infer the receiver to update the receiver window lower bound.");
            var aoaPaylaod = new AckOfAcksPayload();
            aoaPaylaod.AckOfAcksSeqNum = nextUdpPacket.DataHeader.Value.DataSeqNum;
            nextUdpPacket.Header.Flags |= Rdpeudp2PacketHeaderFlags.AOA;
            nextUdpPacket.AckOfAcks = aoaPaylaod;

            this.SendPacket(nextUdpPacket);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect a RDPUDP2 Packet to acknowledge the receipt of all RDPUDP2 Packets.");
            var ackPacket = rdpeudpSocketR.Rdpeudp2Handler.ExpectAckPacket(this.waitTime, nextUdpPacket.DataHeader.Value.DataSeqNum);
            var dataSeqNum = ackPacket.ACK.Value.SeqNum;
            var delayedAcksNum = ackPacket.ACK.Value.numDelayedAcks;
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The recvied ACK packet is to acknowledge DataSeqNum {dataSeqNum} and previous {delayedAcksNum} DataSeqNums.");
            Site.Assert.IsNotNull(ackPacket, "Client should send an ACK to acknowledge the receipt of data packet. Transport mode is {0}.", rdpeudp2TransportMode);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP10.6")]
        [TestCategory("RDPEUDP2")]
        [TestCategory("BasicRequirement")]
        [TestCategory("BasicFeature")]
        [Description("Verify behaviour of client when missing packet is detected as shared in the Sending Data Packets with One Packet Lost in the Middle section of the RDPEUDP2 document")]
        public void S3_DataTransfer_V2_AcknowledgeLostPacket()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection...");
            StartRDPConnection(enableRdpeudp2: true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Establish the RDPEMT connection.");
            DoUntilSucceed(() =>
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", rdpeudp2TransportMode);
                this.EstablishUDPConnection(rdpeudp2TransportMode, waitTime, verifyPacket: true, uUdpVer: RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_3);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", rdpeudp2TransportMode);
                return this.EstablishRdpemtConnection(rdpeudp2TransportMode, waitTime);
            },
            this.waitTime * 5,
            TimeSpan.FromSeconds(0.5),
            "RDPEMT tunnel creation failed");

            this.TestSite.Log.Add(LogEntryKind.Comment, "In the RDP-UDP connection, test suite prepares multiple RDPUDP2 Packets according to the creation instructions in the On the Sender when sending the packet section of the RDPEUDP2 document.");
            var dataList = new List<byte[]>()
            {
                {GetByteData()},
                {GetByteData()},
                {GetByteData()}
            };

            var nextUdpPackets = this.GetNextValidUdp2PacketList(dataList);

            //Send first and third packet to simulate missing second packet.
            this.TestSite.Log.Add(LogEntryKind.Comment, "According to section 4.3.1, only the first and the last packet according to the data sequence is sent to the reciever. Allowing the receiver to assume the middle packet is lost.");
            this.SendPacket(nextUdpPackets[0]);
            this.SendPacket(nextUdpPackets[2]);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect a RDPUDP2 Receiver to send in an acknowledgement vector, showing that an RDPUDP2 Packet is missing. Sending Data Packets with One Packet Lost in the Middle Section");
            var ackVECPacket = rdpeudpSocketR.Rdpeudp2Handler.ExpectAckVecPacket(this.waitTime);

            var BaseSeqNum = ackVECPacket.ACKVEC.Value.BaseSeqNum;

            Site.Assert.IsNotNull(ackVECPacket, "Client should send an ACKVEC packet to acknowledge the presence of a lost packet. Transport mode is {0}.", rdpeudp2TransportMode);
            Site.Assert.AreEqual<ushort>(nextUdpPackets[1].DataHeader.Value.DataSeqNum, BaseSeqNum, $"The DataSeqNum({nextUdpPackets[1].DataHeader.Value.DataSeqNum}) of the missing packet must be equal to the BaseSeqNum({BaseSeqNum}) of the Acknowledgement Vector, showing the sequence number of the missing packet. Section 4.3.2, number 3");

            //Send second packet and confirm acknowledgement receipt
            this.SendPacket(nextUdpPackets[1]);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect an acknowledge packet showing the receipt of missing RDPUDP2 Packet.");
            var ackPacket = rdpeudpSocketR.Rdpeudp2Handler.ExpectAckPacket(this.waitTime, nextUdpPackets[1].DataHeader.Value.DataSeqNum);
            var dataSeqNum = ackPacket.ACK.Value.SeqNum;
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The recvied ACK packet is to acknowledge DataSeqNum {dataSeqNum}.");
            Site.Assert.IsNotNull(ackPacket, "Client should send an ACK to acknowledge the receipt of data packet. Transport mode is {0}.", rdpeudp2TransportMode);
        }

        private byte[] GetByteData()
        {
            byte[] bytes = new byte[1000];
            bytes = Enumerable.Repeat((byte)0x20, 1000).ToArray();

            return bytes;
        }
    }
}