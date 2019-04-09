// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestSuites.Rdpeudp
{    
    public partial class RdpeudpTestSuite : RdpTestClassBase
    {
        #region BVT Test Cases

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the RDP client will send an ACK to acknowledge the received package")]
        public void S2_DataTransfer_ClientReceiveData()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };
            foreach (TransportMode transportMode in transportModeArray)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", transportMode);
                this.EstablishUDPConnection(transportMode, waitTime, true);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send the first RDPEUDP Source packet");
                RdpeudpPacket packet = this.GetFirstValidUdpPacket(transportMode);
                this.SendPacket(transportMode, packet);

                #region Create Expect Ack Vectors

                List<AckVector> expectedAckVectors = new List<AckVector>();                
                AckVector ackVector = new AckVector();
                ackVector.State = VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED;
                ackVector.Length = (byte)(packet.sourceHeader.Value.snSourceStart - getSnInitialSequenceNumber(transportMode) - 1);
                expectedAckVectors.Add(ackVector);

                #endregion Create Expect Ack Vectors

                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect RDP client to send an ACK packet to acknowledge the receipt");
                RdpeudpPacket ackpacket = WaitForACKPacket(transportMode, waitTime, expectedAckVectors.ToArray());
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK to acknowledge the receipt of source packet. Transport mode is {0}", transportMode);
                
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the RDP client will send an ACK to acknowledge the package loss when detect a package loss in a reliable connection.")]
        public void S2_DataTransfer_AcknowledgeTest_AcknowlegeLossyPackage()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };

            foreach (TransportMode transportMode in transportModeArray)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", transportMode);
                this.EstablishUDPConnection(transportMode, waitTime, true);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", transportMode);
                this.EstablishRdpemtConnection(transportMode, waitTime);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send one RDPUDP packet.");
                this.SendNextValidUdpPacket(transportMode);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send the second RDPUDP packet, don't really send it, it is as a lost packet");
                RdpeudpPacket losspacket = this.GetNextValidUdpPacket(transportMode);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send the third and the forth RDPUDP packet.");
                this.SendNextValidUdpPacket(transportMode);
                this.SendNextValidUdpPacket(transportMode);
                
                #region Create Expect Ack Vectors
                
                List<AckVector> expectedAckVectors = new List<AckVector>();
                
                // All packet before the lost one are received.
                AckVector ackVector = new AckVector();
                ackVector.State = VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED;
                ackVector.Length = (byte)(losspacket.sourceHeader.Value.snSourceStart - getSnInitialSequenceNumber(transportMode) - 2);
                expectedAckVectors.Add(ackVector);

                // One packet lost.
                ackVector = new AckVector();
                ackVector.State = VECTOR_ELEMENT_STATE.DATAGRAM_NOT_YET_RECEIVED;
                ackVector.Length = 0;
                expectedAckVectors.Add(ackVector);

                // Two packet received.
                ackVector = new AckVector();
                ackVector.State = VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED;
                ackVector.Length = 1;
                expectedAckVectors.Add(ackVector);

                #endregion Create Expect Ack Vectors

                // Expect an ACK packet with expected acknowledge information.
                this.TestSite.Log.Add(LogEntryKind.Comment, "expect an ACK packet with expected acknowledge information.");
                RdpeudpPacket ackpacket = WaitForACKPacket(transportMode, waitTime, expectedAckVectors.ToArray());
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK to acknowledge the receipt of source packets correctly, transport mode is {0}.", transportMode);
                
                this.TestSite.Log.Add(LogEntryKind.Comment, "Send the second RDPUDP packet, which is lost before");
                this.SendPacket(transportMode, losspacket);

                #region Create Expect Ack Vectors

                expectedAckVectors.Clear();
                // All packet before the lost one are received.
                ackVector = new AckVector();
                ackVector.State = VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED;
                ackVector.Length = (byte)(losspacket.sourceHeader.Value.snSourceStart - getSnInitialSequenceNumber(transportMode)-2);  

                // The lost one and the next two packed are received too.
                ackVector.Length += 3;
                expectedAckVectors.Add(ackVector);

                #endregion

                // Expect an ACK packet with acknowledge all packets recieved.
                this.TestSite.Log.Add(LogEntryKind.Comment, "expect an ACK packet with acknowledge all packets received.");
                ackpacket = WaitForACKPacket(transportMode, waitTime, expectedAckVectors.ToArray());
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK to acknowledge the receipt of source packets correctly, transport mode is {0}.", transportMode);
                
            }
        }


        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the RDP client support congestion control as a receiver.")]
        public void S2_DataTransfer_CongestionControlTest_ClientReceiveData()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };

            foreach (TransportMode transportMode in transportModeArray)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", transportMode);
                this.EstablishUDPConnection(transportMode, waitTime, true);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", transportMode);
                this.EstablishRdpemtConnection(transportMode, waitTime);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send one RDPUDP packet.");
                this.SendNextValidUdpPacket(transportMode);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send the second RDPUDP packet, don't really send it, it is as a lost packet");
                RdpeudpPacket losspacket = this.GetNextValidUdpPacket(transportMode);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send another three RDPUDP packet.");
                this.SendNextValidUdpPacket(transportMode);
                this.SendNextValidUdpPacket(transportMode);
                this.SendNextValidUdpPacket(transportMode);

                // Expect an ACK packet with RDPUDP_FLAG_SYN flag.
                this.TestSite.Log.Add(LogEntryKind.Comment, "expect an ACK packet with RDPUDP_FLAG_SYN flag.");
                RdpeudpPacket ackpacket = WaitForACKPacket(transportMode, waitTime, null, RDPUDP_FLAG.RDPUDP_FLAG_CN, 0);
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK packet with RDPUDP_FLAG_SYN flag, transport mode is {0}", transportMode);
                
                RdpeudpPacket packet = this.GetNextValidUdpPacket(transportMode);
                packet.fecHeader.uFlags = packet.fecHeader.uFlags | RDPUDP_FLAG.RDPUDP_FLAG_CWR;
                this.SendPacket(transportMode, packet);

                // Expect an ACK packet without RDPUDP_FLAG_SYN flag.
                this.TestSite.Log.Add(LogEntryKind.Comment, "expect an ACK packet without RDPUDP_FLAG_SYN flag.");
                ackpacket = WaitForACKPacket(transportMode, waitTime, null, 0, RDPUDP_FLAG.RDPUDP_FLAG_CN);
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK packet without RDPUDP_FLAG_SYN flag, transport mode is {0}", transportMode);
                
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the TLS handshake process on the reliable RDP-UDP connection.")]
        public void S2_DataTransfer_SecurityChannelCreation_ReliableConnection()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a reliable UDP connection.");
            this.EstablishUDPConnection(TransportMode.Reliable, waitTime, true, true);

            // Set the autoHandle to true, then can be used for create security channel.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Start TLS handshake.");
            
            String certFile = this.Site.Properties["CertificatePath"];
            String certPwd = this.Site.Properties["CertificatePassword"];
            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);
            rdpemtServerR = new RdpemtServer(rdpeudpSocketR, cert, false);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Wait for a RDP_TUNNEL_CREATEREQUEST message from client after security channel creation");  
            RDP_TUNNEL_CREATEREQUEST createReq = rdpemtServerR.ExpectTunnelCreateRequest(waitTime);

            Site.Assert.IsNotNull(createReq, "Client should send a RDP_TUNNEL_CREATEREQUEST message after security channel creation.");
            
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the DTLS handshake process on the lossy RDP-UDP connection.")]
        public void S2_DataTransfer_SecurityChannelCreation_LossyConnection()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Lossy UDP connection.");
            this.EstablishUDPConnection(TransportMode.Lossy, waitTime, true);

            // Set the autoHandle to true, then can be used for create security channel.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Start DTLS handshake.");
            this.rdpeudpSocketL.AutoHandle = true;

            String certFile = this.Site.Properties["CertificatePath"];
            String certPwd = this.Site.Properties["CertificatePassword"];
            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);
            rdpemtServerL = new RdpemtServer(rdpeudpSocketL, cert, false);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Wait for a RDP_TUNNEL_CREATEREQUEST message from client after security channel creation");            
            
            RDP_TUNNEL_CREATEREQUEST createReq = rdpemtServerL.ExpectTunnelCreateRequest(waitTime);
            Site.Assert.IsNotNull(createReq, "Client should send a RDP_TUNNEL_CREATEREQUEST message after security channel creation.");
        }

        #endregion BVT Test Cases

        #region Normal Test Cases


        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the RDP client can retransmit source package in the reliable RDP-UDP connection if not receiving ACK acknowledged for a specified time.")]
        public void S2_DataTransfer_RetransmitTest_ClientRetransmit()
        {
            // Record the sequenceNumber of the lost source packet.
            uint sequenceNumberForLossPacket = 0;
            ushort receiveWindowSize = 0;

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a reliable UDP connection.");
            this.EstablishUDPConnection(TransportMode.Reliable, waitTime, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send the first RDPEUDP Source packet, but not acknowledge the receipt of the first source packet from client.");
            RdpeudpPacket packet = this.GetFirstValidUdpPacket(TransportMode.Reliable);

            #region Change packet.ackVectorHeader To Not Acknowledge The Receipt

            RDPUDP_ACK_VECTOR_HEADER ackVectorHeader = new RDPUDP_ACK_VECTOR_HEADER();
            ackVectorHeader.AckVectorElement = null;
            ackVectorHeader.uAckVectorSize = 0;
            packet.ackVectorHeader = ackVectorHeader;
            sequenceNumberForLossPacket = packet.fecHeader.snSourceAck;
            receiveWindowSize = packet.fecHeader.uReceiveWindowSize;
            packet.fecHeader.snSourceAck--;            

            #endregion Change packet.ackVectorHeader To Not Acknowledge The Receipt                        

            this.SendPacket(TransportMode.Reliable, packet);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Wait for 200 ms so as to fire the retransmit timer.");
            Thread.Sleep(this.RetransmitTimer);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Wait for the client to resend the lost packet.");
            RdpeudpPacket receivedPacket = this.WaitForSourcePacket(TransportMode.Reliable, waitTime, sequenceNumberForLossPacket);
            Site.Assert.IsNotNull(receivedPacket, "Client should resend the packet if not receiving an ACK for a specified time.");
            
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the RDP client will send ACK correctly when sequence number is wrapped around")]
        public void S2_DataTransfer_SequenceNumberWrapAround()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };
            foreach (TransportMode transportMode in transportModeArray)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", transportMode);
                initSequenceNumber = uint.MaxValue - 3;
                this.EstablishUDPConnection(transportMode, waitTime, true);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", transportMode);
                this.EstablishRdpemtConnection(transportMode, waitTime);

                if (getSourcePacketSequenceNumber(transportMode) > getSnInitialSequenceNumber(transportMode))
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Not wrap around yet, Send one RDPUDP packet.");
                    this.SendNextValidUdpPacket(transportMode);
                }

                this.TestSite.Log.Add(LogEntryKind.Comment, "Already wrap around, Send three RDPUDP packet again.");
                this.SendNextValidUdpPacket(transportMode);
                this.SendNextValidUdpPacket(transportMode);
                this.SendNextValidUdpPacket(transportMode);
                
                #region Create Expect Ack Vectors

                List<AckVector> expectedAckVectors = new List<AckVector>();
                AckVector ackVector = new AckVector();
                ackVector.State = VECTOR_ELEMENT_STATE.DATAGRAM_RECEIVED;
                ackVector.Length = (byte)(getSourcePacketSequenceNumber(transportMode) + (uint.MaxValue- getSnInitialSequenceNumber(transportMode)));
                expectedAckVectors.Add(ackVector);

                #endregion Create Expect Ack Vectors

                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect RDP client to send an ACK packet to acknowledge the receipt correctly");
                RdpeudpPacket ackpacket = WaitForACKPacket(transportMode, waitTime, expectedAckVectors.ToArray());
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK to correctly acknowledge the receipt of source packet. Transport mode is {0}", transportMode);

            }
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the RDP client will add RDPUDP_FLAG_ACKDELAYED flag in uFlags field of ACK packet if client delayed the ack")]
        public void S2_DataTransfer_ClientAckDelay()
        {
            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };
            foreach (TransportMode transportMode in transportModeArray)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", transportMode);
                initSequenceNumber = uint.MaxValue - 3;
                this.EstablishUDPConnection(transportMode, waitTime, true);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", transportMode);
                this.EstablishRdpemtConnection(transportMode, waitTime);
                
                this.TestSite.Log.Add(LogEntryKind.Comment, "Send three RDPUDP packets, wait {0} ms between each send");
                Thread.Sleep(DelayedACKTimer);
                this.SendNextValidUdpPacket(transportMode);
                Thread.Sleep(DelayedACKTimer);
                this.SendNextValidUdpPacket(transportMode);
                Thread.Sleep(DelayedACKTimer);
                this.SendNextValidUdpPacket(transportMode);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect RDP client to send an ACK packet with flag: RDPUDP_FLAG_ACKDELAYED");
                RdpeudpPacket ackpacket = WaitForACKPacket(transportMode, waitTime, null, RDPUDP_FLAG.RDPUDP_FLAG_ACKDELAYED);
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK with RDPUDP_FLAG_ACKDELAYED flag. Transport mode is {0}", transportMode);

            }
        }

        #endregion Normal Test Cases

    }
}