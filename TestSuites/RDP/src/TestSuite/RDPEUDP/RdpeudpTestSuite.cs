// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System.Security.Cryptography.X509Certificates;


namespace Microsoft.Protocols.TestSuites.Rdpeudp
{
    public enum SynAndAck_InvalidType
    {
        /// <summary>
        /// Valid Type.
        /// </summary>
        None,

        /// <summary>
        /// uUpStreamMtu is larger than 1232.
        /// </summary>
        LargerUpStreamMtu,

        /// <summary>
        /// uUpStreamMtu is smaller than 1132.
        /// </summary>
        SamllerUpStreamMtu,

        /// <summary>
        /// uDownStreamMtu is larger than 1232.
        /// </summary>
        LargerDownStreamMtu,

        /// <summary>
        /// uDownStreamMtu is smaller than 1132.
        /// </summary>
        SamllerDownStreamMtu,
    }

    public enum SourcePacket_InvalidType
    {
        /// <summary>
        /// Valid Type.
        /// </summary>
        None,

        /// <summary>
        /// The Source Payload of Source packet is larger than 1232.
        /// </summary>
        LargerSourcePayload,
        
    }

    [TestClass]
    public partial class RdpeudpTestSuite : RdpTestClassBase
    {

        #region Variables

        private RdpeudpServer rdpeudpServer;
        private RdpeudpServerSocket rdpeudpSocketR;
        private RdpeudpServerSocket rdpeudpSocketL;

        private RdpemtServer rdpemtServerR;
        private RdpemtServer rdpemtServerL;
                
        private uint multitransportRequestId = 0;

        private int DelayedACKTimer = 200;

        private int RetransmitTimer = 200;

        private uint? initSequenceNumber = null;

        #endregion

        #region Class Initialization And Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            RdpTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            RdpTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization And Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            this.rdpbcgrAdapter.TurnVerificationOff(!bVerifyRdpbcgrMessage);
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll();
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);

            if (rdpemtServerL != null)
                rdpemtServerL.Dispose();

            if (rdpemtServerR != null)
                rdpemtServerR.Dispose();
            if (rdpeudpServer != null && rdpeudpServer.Running)
                rdpeudpServer.Stop();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter.StopRDPListening();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Get the initial sequence number of the source packet
        /// </summary>
        /// <param name="udpTransportMode">The transport mode: reliable or lossy</param>
        /// <returns>The initial sequence number of the source packet</returns>
        private uint getSnInitialSequenceNumber(TransportMode udpTransportMode)
        {
            if (udpTransportMode == TransportMode.Reliable)
            {
                return rdpeudpSocketR.SnInitialSequenceNumber;
            }
            return rdpeudpSocketL.SnInitialSequenceNumber;
        }

        private uint getSourcePacketSequenceNumber(TransportMode udpTransportMode)
        {
            if (udpTransportMode == TransportMode.Reliable)
            {
                return rdpeudpSocketR.CurSnSource;
            }
            return rdpeudpSocketL.CurSnSource;
        }

        /// <summary>
        /// Start RDP connection.
        /// </summary>
        private void StartRDPConnection()
        {

            // Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);


            #region Trigger Client To Connect
            // Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region RDPBCGR Connection

            // Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);
            

            // Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion
        }

        /// <summary>
        /// Send SYN and ACK packet.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: Reliable or Lossy</param>
        /// <param name="invalidType">invalid type</param>
        public void SendSynAndAckPacket(TransportMode udpTransportMode, SynAndAck_InvalidType invalidType, uint? initSequenceNumber = null)
        {
            RdpeudpServerSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            if (invalidType == SynAndAck_InvalidType.None)
            {
                // If invalid type is None, send the packet directly.
                rdpeudpSocket.SendSynAndAckPacket(initSequenceNumber);
                return;
            }

            // Create the SYN and ACK packet first.
            RdpeudpPacket SynAndAckPacket = CreateInvalidSynAndACKPacket(udpTransportMode, invalidType, initSequenceNumber);

            rdpeudpSocket.SendPacket(SynAndAckPacket);
        }

        
        /// <summary>
        /// Establish a UDP connection.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: Reliable or Lossy.</param>
        /// <param name="timeout">Wait time.</param>
        /// <param name="verifyPacket">Whether verify the received packet.</param>
        /// <returns>The accepted socket.</returns>
        private RdpeudpSocket EstablishUDPConnection(TransportMode udpTransportMode, TimeSpan timeout, bool verifyPacket = false, bool autoHanlde = false)
        {
            // Start UDP listening.
            if(rdpeudpServer == null)
                rdpeudpServer = new RdpeudpServer((IPEndPoint)this.rdpbcgrAdapter.SessionContext.LocalIdentity, autoHanlde);
            if(!rdpeudpServer.Running)
                rdpeudpServer.Start();

            // Send a Server Initiate Multitransport Request PDU.
            byte[] securityCookie = new byte[16];
            Random rnd = new Random();
            rnd.NextBytes(securityCookie);
            Multitransport_Protocol_value requestedProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                requestedProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL;
            }
            this.rdpbcgrAdapter.SendServerInitiateMultitransportRequestPDU(++this.multitransportRequestId, requestedProtocol, securityCookie);

            // Create a UDP socket.
            RdpeudpServerSocket rdpudpSocket = rdpeudpServer.CreateSocket(((IPEndPoint)this.rdpbcgrAdapter.SessionContext.Identity).Address, udpTransportMode, timeout);
            if (rdpudpSocket == null)
            {
                this.Site.Assert.Fail("Failed to create a UDP socket for the Client : {0}", ((IPEndPoint)this.rdpbcgrAdapter.SessionContext.Identity).Address);
            }

            if (udpTransportMode == TransportMode.Reliable)
            {
                this.rdpeudpSocketR = rdpudpSocket;
            }
            else
            {
                this.rdpeudpSocketL = rdpudpSocket;
            }

            // Expect a SYN packet.
            RdpeudpPacket synPacket = rdpudpSocket.ExpectSynPacket(timeout);
            if (synPacket == null)
            {
                this.Site.Assert.Fail("Time out when waiting for the SYN packet");
            }

            // Verify the SYN packet.
            if (verifyPacket)
            {
                VerifySYNPacket(synPacket, udpTransportMode);
            }

            // Send a SYN and ACK packet.
            SendSynAndAckPacket(udpTransportMode, SynAndAck_InvalidType.None, initSequenceNumber);
            

            // Expect an ACK packet or ACK and Source Packet.
            RdpeudpPacket ackPacket = rdpudpSocket.ExpectACKPacket(timeout);
            if (ackPacket == null)
            {
                this.Site.Assert.Fail("Time out when waiting for the ACK packet to response the ACK and SYN packet");
            }
            
            // Verify the ACK packet.
            if (verifyPacket)
            {
                VerifyACKPacket(ackPacket);
            }            

            // If the packet is an ACK and Source Packet, add the source packet to the un-processed packet.
            if (ackPacket.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA))
            {
                byte[] bytes = PduMarshaler.Marshal(ackPacket, false);
                RdpeudpBasePacket stackpacket = new RdpeudpBasePacket(bytes);
                rdpudpSocket.ReceivePacket(stackpacket);
            }

            rdpudpSocket.Connected = true;

            return rdpudpSocket;
        }

        /// <summary>
        /// Used to establish a RDPEMT connection.
        /// </summary>
        /// <param name="udpTransportMode">Transport Mode: Reliable or Lossy.</param>
        /// <param name="timeout">Wait time.</param>
        private void EstablishRdpemtConnection(TransportMode udpTransportMode, TimeSpan timeout)
        {
            RdpeudpServerSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            if (!rdpeudpSocket.AutoHandle)
            {
                rdpeudpSocket.AutoHandle = true;
            }

            String certFile = this.Site.Properties["CertificatePath"];
            String certPwd = this.Site.Properties["CertificatePassword"];
            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);


            RdpemtServer rdpemtServer = new RdpemtServer(rdpeudpSocket, cert);

            uint receivedRequestId;
            byte[] receivedSecurityCookie;
            if (!rdpemtServer.ExpectConnect(waitTime, out receivedRequestId, out receivedSecurityCookie))
            {
                Site.Assert.Fail("RDPEMT tunnel creation failed");
            }

            rdpeudpSocket.AutoHandle = false;

            if (udpTransportMode == TransportMode.Reliable)
            {
                rdpemtServerR = rdpemtServer;
            }
            else
            {
                rdpemtServerL = rdpemtServer;
            }
            
           
        }

        /// <summary>
        /// Get the First valid UDP Source Packet.
        /// </summary>
        /// <param name="udpTransportMode"></param>
        /// <returns></returns>
        private RdpeudpPacket GetFirstValidUdpPacket(TransportMode udpTransportMode)
        {
            byte[] dataToSent = null;
            RdpeudpPacket firstPacket = null;
            String certFile = this.Site.Properties["CertificatePath"];
            String certPwd = this.Site.Properties["CertificatePassword"];
            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);

            if (udpTransportMode == TransportMode.Reliable)
            {
                RdpeudpTLSChannel secChannel = new RdpeudpTLSChannel(rdpeudpSocketR);
                secChannel.AuthenticateAsServer(cert);
                RdpeudpPacket packet =  rdpeudpSocketR.ExpectPacket(waitTime);
                if (packet.payload != null)
                {
                    rdpeudpSocketR.ProcessSourceData(packet); // Process Source Data to makesure ACK Vector created next is correct
                    secChannel.ReceiveBytes(packet.payload);
                }
                dataToSent = secChannel.GetDataToSent(waitTime);
                firstPacket = rdpeudpSocketR.CreateSourcePacket(dataToSent);
            }
            else
            {
                RdpeudpDTLSChannel secChannel = new RdpeudpDTLSChannel(rdpeudpSocketL);
                secChannel.AuthenticateAsServer(cert);
                RdpeudpPacket packet = rdpeudpSocketL.ExpectPacket(waitTime);
                if (packet.payload != null)
                {
                    rdpeudpSocketL.ProcessSourceData(packet); // Process Source Data to makesure ACK Vector created next is correct
                    secChannel.ReceiveBytes(packet.payload);
                }
                dataToSent = secChannel.GetDataToSent(waitTime);
                firstPacket = rdpeudpSocketL.CreateSourcePacket(dataToSent);
            }
            
            return firstPacket;

        }

        /// <summary>
        /// Get the next valid rdpeudp packet.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: reliable or Lossy.</param>
        /// <returns>The next valid rdpeudp packet.</returns>
        private RdpeudpPacket GetNextValidUdpPacket(TransportMode udpTransportMode, byte[] data = null)
        {
            /*This function is used to get a valid rdpeudp packet.
             * Using rdpeudpSocket.LossPacket flag to control whether the socket send the packet.
             * First set rdpeudpSocket.LossPacket to true and send a tunnal Data, the socket will store the next packet(RDPEUDP socket which contains the encrypted tunnel data) and doesn't send it.
             * Then get the stored packet and return it.
             */
            RdpemtServer rdpemtServer = rdpemtServerR;
            RdpeudpSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpemtServer = rdpemtServerL;
                rdpeudpSocket = rdpeudpSocketL;
            }

            if (data == null)
                data = new byte[1000];
            RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(data, null);

            byte[] unEncryptData = PduMarshaler.Marshal(tunnelData);
            byte[] encryptData = null;

            if (udpTransportMode == TransportMode.Reliable)
            {
                RdpeudpTLSChannel secChannel = rdpemtServer.SecureChannel as RdpeudpTLSChannel;
                encryptData = secChannel.Encrypt(unEncryptData);
            }
            else
            {
                RdpeudpDTLSChannel secChannel = rdpemtServer.SecureChannel as RdpeudpDTLSChannel;
                List<byte[]> encryptDataList = secChannel.Encrypt(unEncryptData);
                if (encryptDataList != null && encryptDataList.Count > 0)
                {
                    encryptData = encryptDataList[0];
                }
            }

            RdpeudpPacket packet = rdpeudpSocket.CreateSourcePacket(encryptData);

            return packet;
            
        }

        /// <summary>
        /// Send a udp packet.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: reliable or lossy.</param>
        /// <param name="packet">The packet to send.</param>
        private void SendPacket(TransportMode udpTransportMode, RdpeudpPacket packet)
        {
            RdpeudpSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {                
                rdpeudpSocket = rdpeudpSocketL;
            }

            rdpeudpSocket.SendPacket(packet);
        }

        /// <summary>
        /// Get a valid RDPEUDP packet and send it.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: reliable or lossy.</param>
        private void SendNextValidUdpPacket(TransportMode udpTransportMode, byte[] data = null)
        {
            RdpemtServer rdpemtServer = rdpemtServerR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpemtServer = rdpemtServerL;
            }

            if (data == null)
                data = new byte[1000];
            RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(data, null);
            rdpemtServer.SendRdpemtPacket(tunnelData);            
        }

        /// <summary>
        /// Send an invalid UDP source Packet.
        /// </summary>
        /// <param name="udpTransportMode"></param>
        /// <param name="invalidType"></param>
        private void SendInvalidUdpSourcePacket(TransportMode udpTransportMode, SourcePacket_InvalidType invalidType)
        {            
            RdpeudpSocket rdpeudpSocket = rdpeudpSocketR;
            RdpemtServer rdpemtServer = rdpemtServerR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
                rdpemtServer = rdpemtServerL;
            }

            if (invalidType == SourcePacket_InvalidType.LargerSourcePayload)
            {
                // Change UpStreamMtu of RDPEUDP Socket, so that large data can be sent
                ushort upstreamMtu = rdpeudpSocket.UUpStreamMtu;
                rdpeudpSocket.UUpStreamMtu = 2000;

                byte[] data = new byte[1600];
                RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(data, null);
                rdpemtServer.SendRdpemtPacket(tunnelData);

                // Change UpStreamMtu to correct value
                rdpeudpSocket.UUpStreamMtu = upstreamMtu;
            }
        }
        /// <summary>
        /// Compare two AckVectors.
        /// </summary>
        /// <param name="vector1">Ack Vector 1.</param>
        /// <param name="vector2">Ack Vector 2.</param>
        /// <returns></returns>
        private bool CompareAckVectors(AckVector[] vector1, AckVector[] vector2)
        {
            int posForVector1 = 0;
            int posForVector2 = 0;
            int length1 = vector1[posForVector1].Length+1;
            int length2 = vector2[posForVector2].Length+1;
            while (posForVector1 < vector1.Length && posForVector2 < vector2.Length)
            {
                if (vector1[posForVector1].State != vector2[posForVector2].State)
                {
                    return false;
                }

                if (length1 == length2)
                {
                    posForVector1++;
                    posForVector2++;
                    if (!(posForVector1 < vector1.Length && posForVector2 < vector2.Length))
                        break;
                    length1 = vector1[posForVector1].Length+1;
                    length2 = vector2[posForVector2].Length+1;
                }
                else if (length1 > length2)
                {
                    length1 -= length2;
                    posForVector2++;
                    if (!(posForVector1 < vector1.Length && posForVector2 < vector2.Length))
                        break;
                    length2 = vector2[posForVector2].Length + 1;
                }
                else
                {
                    length2 -= length1;
                    posForVector1++;
                    if (!(posForVector1 < vector1.Length && posForVector2 < vector2.Length))
                        break;
                    length1 = vector1[posForVector1].Length + 1;
                }
            }
            if (posForVector1 < vector1.Length || posForVector2 < vector2.Length)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Expect for a Source Packet.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: reliable or lossy.</param>
        /// <param name="timeout">Wait time</param>
        /// <returns></returns>
        private RdpeudpPacket WaitForSourcePacket(TransportMode udpTransportMode, TimeSpan timeout, uint sequnceNumber = 0)
        {
            RdpeudpSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            DateTime endTime = DateTime.Now + timeout;

            while (DateTime.Now < endTime)
            {
                RdpeudpPacket packet = rdpeudpSocket.ExpectPacket(endTime - DateTime.Now);
                if (packet!=null && packet.fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA))
                {
                    if (sequnceNumber == 0||packet.sourceHeader.Value.snSourceStart == sequnceNumber)
                    {
                        return packet;
                    }
                }                
            }

            return null;
        }

        /// <summary>
        /// Wait for an ACK packet which meets certain conditions.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: reliable or lossy.</param>
        /// <param name="timeout">Wait time.</param>
        /// <param name="expectAckVectors">Expected ack vectors.</param>
        /// <param name="hasFlag">Flags, which the ACK packet must contain.</param>
        /// <param name="notHasFlag">Flags, which the ACK packet must no contain.</param>
        /// <returns></returns>
        private RdpeudpPacket WaitForACKPacket(TransportMode udpTransportMode, TimeSpan timeout, AckVector[] expectAckVectors = null, RDPUDP_FLAG hasFlag = 0, RDPUDP_FLAG notHasFlag = 0) 
        {
            RdpeudpSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            DateTime endTime = DateTime.Now + timeout;

            while (DateTime.Now < endTime)
            {
                RdpeudpPacket ackPacket = rdpeudpSocket.ExpectACKPacket(endTime - DateTime.Now);
                if (ackPacket!=null)
                {
                    if (expectAckVectors != null)
                    {
                        if (!(ackPacket.ackVectorHeader.HasValue && CompareAckVectors(ackPacket.ackVectorHeader.Value.AckVectorElement, expectAckVectors)))
                        {
                            continue;
                        }
                    }
                    if (hasFlag != 0)
                    {
                        if ((ackPacket.fecHeader.uFlags & hasFlag) != hasFlag)
                        {
                            continue;
                        }
                    }
                    if (notHasFlag != 0)
                    {
                        if ((ackPacket.fecHeader.uFlags & notHasFlag) != 0)
                        {
                            continue;
                        }
                    }
                    return ackPacket;
                }
            }

            return null;
        }

        /// <summary>
        /// Create invalid SYN and ACK Packet
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: reliable or lossy</param>
        /// <param name="invalidType">Invalid type</param>
        /// <param name="initSequenceNumber">init sequence</param>
        /// <returns></returns>
        private RdpeudpPacket CreateInvalidSynAndACKPacket(TransportMode udpTransportMode, SynAndAck_InvalidType invalidType, uint? initSequenceNumber = null)
        {
            RdpeudpServerSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            // Create the SYN and ACK packet.
            RdpeudpPacket SynAndAckPacket = new RdpeudpPacket();
            SynAndAckPacket.fecHeader.snSourceAck = rdpeudpSocket.SnSourceAck;
            SynAndAckPacket.fecHeader.uReceiveWindowSize = rdpeudpSocket.UReceiveWindowSize;
            SynAndAckPacket.fecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_SYN | RDPUDP_FLAG.RDPUDP_FLAG_ACK;


            RDPUDP_SYNDATA_PAYLOAD SynPayload = rdpeudpSocket.CreateSynData(initSequenceNumber);

            switch (invalidType)
            {
                case SynAndAck_InvalidType.LargerUpStreamMtu:
                    SynPayload.uUpStreamMtu = 1232 + 1;
                    break;
                case SynAndAck_InvalidType.SamllerUpStreamMtu:
                    SynPayload.uUpStreamMtu = 1132 - 1;
                    break;
                case SynAndAck_InvalidType.LargerDownStreamMtu:
                    SynPayload.uDownStreamMtu = 1232 + 1;
                    break;
                case SynAndAck_InvalidType.SamllerDownStreamMtu:
                    SynPayload.uDownStreamMtu = 1132 - 1;
                    break;
            }

            SynAndAckPacket.SynData = SynPayload;

            return SynAndAckPacket;
        }
        #endregion

        #region Packet Verification

        /// <summary>
        /// Verify SYN packet.
        /// </summary>
        /// <param name="synPacket">The SYN packet.</param>
        /// <param name="udpTransportMode">Transport mode: reliable or lossy.</param>
        private void VerifySYNPacket(RdpeudpPacket synPacket, TransportMode udpTransportMode)
        {
            if (synPacket == null)
            {
                this.Site.Assert.Fail("The SYN Packet should not be null!");
            }
            
            if (synPacket.fecHeader.snSourceAck != uint.MaxValue)
            {
                this.Site.Assert.Fail("The snSourceAck variable MUST be set to -1 (max value of uint)!");
            }

            if((synPacket.fecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_SYN) == 0)
            {
                this.Site.Assert.Fail("The RDPUDP_FLAG_SYN flag MUST be set in SYN packet!");
            }
            
            if (udpTransportMode == TransportMode.Reliable)
            {
                if ((synPacket.fecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY) == RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY)
                {
                    this.Site.Assert.Fail("The RDPUDP_FLAG_SYNLOSSY flag MUST not be set when choose reliable UDP connection!");
                }
            }
            else
            {
                if ((synPacket.fecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY) == 0)
                {
                    this.Site.Assert.Fail("The RDPUDP_FLAG_SYNLOSSY flag MUST be set When choose lossy UDP connection!");
                }
            }

            if (synPacket.SynData == null)
            {
                this.Site.Assert.Fail("The SYN Packet should contain a RDPUDP_SYNDATA_PAYLOAD structure!");
            }

            if (synPacket.SynData.Value.uUpStreamMtu > 1232 || synPacket.SynData.Value.uUpStreamMtu < 1132)
            {
                this.Site.Assert.Fail("The uUpStreamMtu field MUST be set to a value in the range of 1132 to 1232.");
            }

            if (synPacket.SynData.Value.uDownStreamMtu > 1232 || synPacket.SynData.Value.uDownStreamMtu < 1132)
            {
                this.Site.Assert.Fail("The uDownStreamMtu field MUST be set to a value in the range of 1132 to 1232.");
            }
            
        }

        /// <summary>
        /// Verify an ACK Packet.
        /// </summary>
        /// <param name="ackPacket">The ACK packet.</param>
        private void VerifyACKPacket(RdpeudpPacket ackPacket)
        {
            if (ackPacket == null)
            {
                this.Site.Assert.Fail("The ACK Packet should not be null!");
            }
            
            if ((ackPacket.fecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_ACK) == 0)
            {
                this.Site.Assert.Fail("The RDPUDP_FLAG_ACK flag MUST be set in ACK packet!");
            }
                       
        }
              
        #endregion
    }
}
