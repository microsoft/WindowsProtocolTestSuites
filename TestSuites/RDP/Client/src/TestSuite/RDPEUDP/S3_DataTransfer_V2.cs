// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
        [Description("Verify that the final OnWire version of the RDP2 Packet sent is evaluated to a specific output as shared in Section 4.4.6 of the RDPEUDP2 document")]
        public void S3_DataTransfer_V2_DataPacket()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "In the RDP-UDP connection, test suite prepare one RDPUDP2 Packet with the packet values given in section 4.4 of the RDPEUDP2 document.");
            var nextUdpPacket = this.GetNextValidUdp2Packet();
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The DataSeqNum of next valid RDPEUDP2 packet is {nextUdpPacket.DataHeader.Value.DataSeqNum}.");
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The ACK payload of next valid RDPEUDP2 packet is to acknowledge {nextUdpPacket.ACK.Value.SeqNum}.");

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
        [Description("Verify that the final OnWire version of the RDP2 Packet sent is evaluated to a specific output as shared in Section 4.4.6 of the RDPEUDP2 document")]
        public void S3_DataTransfer_V2_DataPacket_Ack_AckVec()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "In the RDP-UDP connection, test suite prepare one RDPUDP2 Packet with the packet values given in section 4.4 of the RDPEUDP2 document.");
            var nextUdpPacket = this.GetNextInvalidUdp2Packet();
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The DataSeqNum of next valid RDPEUDP2 packet is {nextUdpPacket.DataHeader.Value.DataSeqNum}.");
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The ACK payload of next valid RDPEUDP2 packet is to acknowledge {nextUdpPacket.ACK.Value.SeqNum}.");

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
        [Description("Verify that the final OnWire version of the RDP2 Packet sent is evaluated to a specific output as shared in Section 4.4.6 of the RDPEUDP2 document")]
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "In the RDP-UDP connection, test suite prepare one RDPUDP2 Packet with the packet values given in section 4.4 of the RDPEUDP2 document.");
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
        [Description("Verify that the final OnWire version of the RDP2 Packet sent is evaluated to a specific output as shared in Section 4.4.6 of the RDPEUDP2 document")]
        public void S3_DataTransfer_V2_DataPacketWithPacketLoss()
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

            this.TestSite.Log.Add(LogEntryKind.Comment, "In the RDP-UDP connection, test suite prepare one RDPUDP2 Packet with the packet values given in section 4.4 of the RDPEUDP2 document.");
            var dataList = new Dictionary<int, byte[]>()
            {
                { 1, null },
                { 2, null },
                { 3, null }
            };

            var nextUdpPacket = this.GetNextValidUdp2PacketList(dataList);

            //this.TestSite.Log.Add(LogEntryKind.Debug, $"The DataSeqNum of next valid RDPEUDP2 packet is {nextUdpPacket.DataHeader.Value.DataSeqNum}.");
            //this.TestSite.Log.Add(LogEntryKind.Debug, $"The ACK payload of next valid RDPEUDP2 packet is to acknowledge {nextUdpPacket.ACK.Value.SeqNum}.");

            this.SendPacket(nextUdpPacket.GetValueOrDefault(1));
            this.SendPacket(nextUdpPacket.GetValueOrDefault(3));

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect a RDPUDP2 Packet to acknowledge the receipt of all RDPUDP2 Packets.");
            var ackPacket = rdpeudpSocketR.Rdpeudp2Handler.ExpectAckVecPacket(this.waitTime);
            var dataSeqNum = ackPacket.ACK.Value.SeqNum;
            var delayedAcksNum = ackPacket.ACK.Value.numDelayedAcks;
            this.TestSite.Log.Add(LogEntryKind.Debug, $"The recvied ACK packet is to acknowledge DataSeqNum {dataSeqNum} and previous {delayedAcksNum} DataSeqNums.");
            Site.Assert.IsNotNull(ackPacket, "Client should send an ACK to acknowledge the receipt of data packet. Transport mode is {0}.", rdpeudp2TransportMode);
        }
    }
}