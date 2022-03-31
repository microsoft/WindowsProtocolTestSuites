// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private int delayedACKTimer = 200;

        private int retransmitTimer = 200;

        private uint? initSequenceNumber = null;

        private RDPUDP_PROTOCOL_VERSION? clientUUdpVer = null;

        private RDPUDP_VERSION_INFO? clientRdpudpVersionInfoValidFlag = null;

        private const TransportMode rdpeudp2TransportMode = TransportMode.Reliable;

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

        #region Test Cleanup       
        protected override void TestCleanup()
        {
            base.TestCleanup();

            //Reset the client status to avoid dirty data for the next test case.
            this.clientUUdpVer = null;
            this.clientRdpudpVersionInfoValidFlag = null;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");

            TriggerClientDisconnectAll();

            rdpemtServerL?.Dispose();
            rdpemtServerR?.Dispose();

            rdpeudpServer?.Stop();
            rdpeudpSocketR?.Close();
            rdpeudpSocketL?.Close();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter?.StopRDPListening();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Get the initial sequence number of the source packet
        /// </summary>
        /// <param name="udpTransportMode">The transport mode: reliable or lossy</param>
        /// <returns>The initial sequence number of the source packet</returns>
        private uint GetSnInitialSequenceNumber(TransportMode udpTransportMode)
        {
            if (udpTransportMode == TransportMode.Reliable)
            {
                return rdpeudpSocketR.SnInitialSequenceNumber;
            }
            return rdpeudpSocketL.SnInitialSequenceNumber;
        }

        private uint GetSourcePacketSequenceNumber(TransportMode udpTransportMode)
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
        private void StartRDPConnection(bool enableRdpeudp2 = false)
        {
            // Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);


            #region Trigger Client To Connect
            // Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            TriggerClientRDPConnect(transportProtocol);
            #endregion

            #region RDPBCGR Connection

            // Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            // Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            if (enableRdpeudp2)
            {
                this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, TS_UD_SC_CORE_version_Values.V9, MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR);
            }
            else
            {
                this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion, MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL | MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECR);
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn);

            #endregion
        }

        /// <summary>
        /// Send SYN and ACK packet.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: Reliable or Lossy</param>
        /// <param name="invalidType">invalid type</param>
        public void SendSynAndAckPacket(TransportMode udpTransportMode, SynAndAck_InvalidType invalidType, uint? initSequenceNumber = null, RDPUDP_PROTOCOL_VERSION? uUdpVer = null)
        {
            RdpeudpServerSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            if (invalidType == SynAndAck_InvalidType.None)
            {
                // If invalid type is None, send the packet directly.
                rdpeudpSocket.SendSynAndAckPacket(initSequenceNumber, uUdpVer);
                return;
            }

            // Create the SYN and ACK packet first.
            RdpeudpPacket SynAndAckPacket = CreateInvalidSynAndAckPacket(udpTransportMode, invalidType, initSequenceNumber);

            rdpeudpSocket.SendPacket(SynAndAckPacket);
        }


        /// <summary>
        /// Establish a UDP connection.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: Reliable or Lossy.</param>
        /// <param name="timeout">Wait time.</param>
        /// <param name="verifyPacket">Whether verify the received packet.</param>
        /// <returns>The accepted socket.</returns>
        private RdpeudpSocket EstablishUDPConnection(TransportMode udpTransportMode, TimeSpan timeout, bool verifyPacket = false, bool autoHanlde = false, RDPUDP_PROTOCOL_VERSION? uUdpVer = null)
        {
            // Start UDP listening.
            if (rdpeudpServer == null)
            {
                rdpeudpServer = new RdpeudpServer((IPEndPoint)this.rdpbcgrAdapter.SessionContext.LocalIdentity, autoHanlde);

                rdpeudpServer.UnhandledExceptionReceived += (ex) =>
                {
                    Site.Log.Add(LogEntryKind.Debug, $"Unhandled exception from RdpeudpServer: {ex}");
                };
            }

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
            RdpeudpServerSocket rdpeudpSocket = rdpeudpServer.CreateSocket(((IPEndPoint)this.rdpbcgrAdapter.SessionContext.Identity).Address, udpTransportMode, timeout);
            if (rdpeudpSocket == null)
            {
                this.Site.Assert.Fail("Failed to create a UDP socket for the Client : {0}", ((IPEndPoint)this.rdpbcgrAdapter.SessionContext.Identity).Address);
            }

            if (udpTransportMode == TransportMode.Reliable)
            {
                this.rdpeudpSocketR = rdpeudpSocket;
            }
            else
            {
                this.rdpeudpSocketL = rdpeudpSocket;
            }

            // Expect a SYN packet.
            RdpeudpPacket synPacket = rdpeudpSocket.ExpectSynPacket(timeout);
            if (synPacket == null)
            {
                this.Site.Assert.Fail("Time out when waiting for the SYN packet");
            }

            // Verify the SYN packet.
            if (verifyPacket)
            {
                VerifySynPacket(synPacket, udpTransportMode);
            }

            // Send a SYN and ACK packet.
            if (this.clientRdpudpVersionInfoValidFlag == RDPUDP_VERSION_INFO.RDPUDP_VERSION_INFO_VALID)
            {
                //Section 3.1.5.1.3: The uUdpVer field MUST be set to the highest RDP-UDP protocol version supported by both endpoints. 
                uUdpVer = uUdpVer > this.clientUUdpVer ? this.clientUUdpVer : uUdpVer;
                SendSynAndAckPacket(udpTransportMode, SynAndAck_InvalidType.None, initSequenceNumber, uUdpVer);
            }
            else if (this.clientRdpudpVersionInfoValidFlag == RDPUDP_VERSION_INFO.None)
            {
                //Section 3.1.5.1.3: The highest version supported by both endpoints, which is RDPUDP_PROTOCOL_VERSION_1 if either this packet or the SYN packet does not specify a version, is the version that MUST be used by both endpoints.
                SendSynAndAckPacket(udpTransportMode, SynAndAck_InvalidType.None, initSequenceNumber, RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_1);
            }
            else
            {
                //Section 3.1.5.1.3: The RDPUDP_SYNEX_PAYLOAD structure (section 2.2.2.9) SHOULD only be present if it is also present in the received SYN packet.
                SendSynAndAckPacket(udpTransportMode, SynAndAck_InvalidType.None, initSequenceNumber, null);
            }

            if (!rdpeudpSocket.UpgradedToRdpeudp2)
            {
                // Expect an ACK packet or ACK and Source Packet.
                RdpeudpPacket ackPacket = rdpeudpSocket.ExpectAckPacket(timeout);
                if (ackPacket == null)
                {
                    this.Site.Assert.Fail("Time out when waiting for the ACK packet to response the ACK and SYN packet");
                }

                // Verify the ACK packet.
                if (verifyPacket)
                {
                    VerifyAckPacket(ackPacket);
                }

                // If the packet is an ACK and Source Packet, add the source packet to the un-processed packet.
                if (ackPacket.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA))
                {
                    byte[] bytes = PduMarshaler.Marshal(ackPacket, false);
                    RdpeudpBasePacket stackpacket = new RdpeudpBasePacket(bytes);
                    rdpeudpSocket.ReceivePacket(stackpacket);
                }
            }

            rdpeudpSocket.Connected = true;

            return rdpeudpSocket;
        }

        /// <summary>
        /// Used to establish a RDPEMT connection.
        /// </summary>
        /// <param name="udpTransportMode">Transport Mode: Reliable or Lossy.</param>
        /// <param name="timeout">Wait time.</param>
        /// <returns>true,false.</returns>
        private bool EstablishRdpemtConnection(TransportMode udpTransportMode, TimeSpan timeout)
        {
            bool pass = true;
            RdpeudpServerSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            if (!rdpeudpSocket.AutoHandle)
            {
                rdpeudpSocket.AutoHandle = true;
            }

            String certFile;
            PtfPropUtility.GetPtfPropertyValue(Site, "CertificatePath", out certFile);

            String certPwd;
            PtfPropUtility.GetPtfPropertyValue(Site, "CertificatePassword", out certPwd);

            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);

            RdpemtServer rdpemtServer = new RdpemtServer(rdpeudpSocket, cert);

            uint receivedRequestId;
            byte[] receivedSecurityCookie;
            if (!rdpemtServer.ExpectConnect(waitTime, out receivedRequestId, out receivedSecurityCookie))
            {
                pass = false;
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

            if (!pass)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection failed, stop rdpeudpServer and close socket connection and retry.", udpTransportMode);
                rdpeudpServer.Stop();
                rdpeudpServer = null;
                if (udpTransportMode == TransportMode.Reliable)
                {
                    rdpeudpSocketR.Close();
                    rdpeudpSocketR = null;
                }
                else
                {
                    rdpeudpSocketL.Close();
                    rdpeudpSocketL = null;
                }
            }

            return pass;
        }

        private int GetMaximumPayloadSizeForSourcePacket(int upStreamMtu)
        {
            // Create a fake empty RDPEUDP ACK+SOURCE packet.
            var packet = new RdpeudpPacket();

            packet.FecHeader.snSourceAck = 0;
            packet.FecHeader.uReceiveWindowSize = 0;
            packet.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_DATA | RDPUDP_FLAG.RDPUDP_FLAG_ACK;

            var ackVectorHeader = new RDPUDP_ACK_VECTOR_HEADER();
            ackVectorHeader.uAckVectorSize = 0;
            ackVectorHeader.AckVector = null;
            ackVectorHeader.Padding = null;
            packet.AckVectorHeader = ackVectorHeader;

            var sourceHeader = new RDPUDP_SOURCE_PAYLOAD_HEADER();
            sourceHeader.snCoded = 0;
            sourceHeader.snSourceStart = 0;
            packet.SourceHeader = sourceHeader;

            var size = PduMarshaler.Marshal(packet).Length;

            // Maximum payload size = upstream MTU - empty ACK+SOURCE header size.
            return upStreamMtu - size;
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
            String certFile;
            PtfPropUtility.GetPtfPropertyValue(Site, "CertificatePath", out certFile);

            String certPwd;
            PtfPropUtility.GetPtfPropertyValue(Site, "CertificatePassword", out certPwd);

            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);

            if (udpTransportMode == TransportMode.Reliable)
            {
                RdpeudpTLSChannel secChannel = new RdpeudpTLSChannel(rdpeudpSocketR);
                secChannel.AuthenticateAsServer(cert);
                RdpeudpPacket packet = rdpeudpSocketR.ExpectPacket(waitTime);
                if (packet.Payload != null)
                {
                    rdpeudpSocketR.ProcessSourceData(packet); // Process Source Data to make sure ACK Vector created next is correct
                    secChannel.ReceiveBytes(packet.Payload);
                }
                dataToSent = secChannel.GetDataToSent(waitTime);

                // Make sure this test packet does not exceed upstream MTU.
                int maxPayloadsize = GetMaximumPayloadSizeForSourcePacket(rdpeudpSocketR.UUpStreamMtu);

                dataToSent = dataToSent.Take(maxPayloadsize).ToArray();

                firstPacket = rdpeudpSocketR.CreateSourcePacket(dataToSent);
            }
            else
            {
                RdpeudpDTLSChannel secChannel = new RdpeudpDTLSChannel(rdpeudpSocketL);
                secChannel.AuthenticateAsServer(cert);
                RdpeudpPacket packet = rdpeudpSocketL.ExpectPacket(waitTime);
                if (packet.Payload != null)
                {
                    rdpeudpSocketL.ProcessSourceData(packet); // Process Source Data to make sure ACK Vector created next is correct
                    secChannel.ReceiveBytes(packet.Payload);
                }
                dataToSent = secChannel.GetDataToSent(waitTime);

                // Make sure this test packet does not exceed upstream MTU.
                int maxPayloadsize = GetMaximumPayloadSizeForSourcePacket(rdpeudpSocketL.UUpStreamMtu);

                dataToSent = dataToSent.Take(maxPayloadsize).ToArray();

                firstPacket = rdpeudpSocketL.CreateSourcePacket(dataToSent);
            }

            return firstPacket;
        }

        /// <summary>
        /// Get the next valid RDPEUDP packet.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: Reliable or Lossy.</param>
        /// <param name="data">Data to be sent.</param>
        /// <returns>The next valid RDPEUDP packet.</returns>
        private RdpeudpPacket GetNextValidUdpPacket(TransportMode udpTransportMode, byte[] data = null)
        {
            /*This function is used to get a valid rdpeudp packet.
            * Using rdpeudpSocket.LossPacket flag to control whether the socket send the packet.
            * First set rdpeudpSocket.LossPacket to true and send a tunnal Data, the socket will store the next packet(RDPEUDP socket which contains the encrypted tunnel data) and doesn't send it.
            * Then get the stored packet and return it.
            */
            RdpeudpSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            var encryptedData = GetEncryptedData(udpTransportMode, data);
            RdpeudpPacket packet = rdpeudpSocket.CreateSourcePacket(encryptedData);

            return packet;
        }

        /// <summary>
        /// Get the next valid RDPEUDP2 packet.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        /// <returns>The next valid RDPEUDP2 packet.</returns>
        private Rdpeudp2Packet GetNextValidUdp2Packet(byte[] data = null)
        {
            var encryptedData = GetEncryptedData(rdpeudp2TransportMode, data);
            var packet = rdpeudpSocketR.Rdpeudp2Handler.CreateDataPacket(encryptedData);

            return packet;
        }

        /// <summary>
        /// Get the an odd list of valid sequenced RDPEUDP2 packet.
        /// </summary>
        /// <param name="dataList">List of data to be sent with the order.</param>
        /// <returns>The a list of valid sequenced RDPEUDP2 packets.</returns>
        private List<Rdpeudp2Packet> GetNextValidUdp2PacketList(List<byte[]> dataList)
        {
            var packetList = new List<Rdpeudp2Packet>();

            foreach (byte[] b in dataList)
            {
                var encryptedData = GetEncryptedData(rdpeudp2TransportMode, b);
                var packet = rdpeudpSocketR.Rdpeudp2Handler.CreateDataPacket(encryptedData);

                packetList.Add(packet);
            }

            return packetList;
        }

        /// <summary>
        /// Get the encrypted data from the TLS tunnel established with the RpdemtServer.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: Reliable or Lossy.</param>
        /// <param name="data">Data to be sent.</param>
        /// <returns>The data encrypted with the associated TLS context.</returns>
        private byte[] GetEncryptedData(TransportMode udpTransportMode, byte[] data = null)
        {
            RdpemtServer rdpemtServer = rdpemtServerR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpemtServer = rdpemtServerL;
            }

            if (data == null)
                data = new byte[1000];
            RDP_TUNNEL_DATA tunnelData = rdpemtServer.CreateTunnelDataPdu(data, null);

            byte[] plainData = PduMarshaler.Marshal(tunnelData);
            byte[] encryptedData = null;

            if (udpTransportMode == TransportMode.Reliable)
            {
                RdpeudpTLSChannel secChannel = rdpemtServer.SecureChannel as RdpeudpTLSChannel;
                encryptedData = secChannel.Encrypt(plainData);
            }
            else
            {
                RdpeudpDTLSChannel secChannel = rdpemtServer.SecureChannel as RdpeudpDTLSChannel;
                List<byte[]> encryptDataList = secChannel.Encrypt(plainData);
                if (encryptDataList != null && encryptDataList.Count > 0)
                {
                    encryptedData = encryptDataList[0];
                }
            }

            return encryptedData;
        }

        /// <summary>
        /// Send a UDP packet.
        /// </summary>
        /// <param name="udpTransportMode">Transport mode: Reliable or lossy.</param>
        /// <param name="packet">The packet to be sent.</param>
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
        /// Send a UDP packet.
        /// </summary>
        /// <param name="packet">The packet to be sent.</param>
        private void SendPacket(Rdpeudp2Packet packet)
        {
            rdpeudpSocketR.SendPacket(packet);
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
        private bool CompareAckVectors(AckVectorElement[] vector1, AckVectorElement[] vector2)
        {
            int posForVector1 = 0;
            int posForVector2 = 0;
            int length1 = vector1[posForVector1].Length + 1;
            int length2 = vector2[posForVector2].Length + 1;
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
                    length1 = vector1[posForVector1].Length + 1;
                    length2 = vector2[posForVector2].Length + 1;
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
                if (packet != null && packet.FecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA))
                {
                    if (sequnceNumber == 0 || packet.SourceHeader.Value.snSourceStart == sequnceNumber)
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
        private RdpeudpPacket WaitForAckPacket(TransportMode udpTransportMode, TimeSpan timeout, AckVectorElement[] expectAckVectors = null, RDPUDP_FLAG hasFlag = 0, RDPUDP_FLAG notHasFlag = 0)
        {
            RdpeudpSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            DateTime endTime = DateTime.Now + timeout;

            while (DateTime.Now < endTime)
            {
                RdpeudpPacket ackPacket = rdpeudpSocket.ExpectAckPacket(endTime - DateTime.Now);
                if (ackPacket != null)
                {
                    if (expectAckVectors != null)
                    {
                        if (!(ackPacket.AckVectorHeader.HasValue && CompareAckVectors(ackPacket.AckVectorHeader.Value.AckVector, expectAckVectors)))
                        {
                            continue;
                        }
                    }
                    if (hasFlag != 0)
                    {
                        if ((ackPacket.FecHeader.uFlags & hasFlag) != hasFlag)
                        {
                            continue;
                        }
                    }
                    if (notHasFlag != 0)
                    {
                        if ((ackPacket.FecHeader.uFlags & notHasFlag) != 0)
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
        private RdpeudpPacket CreateInvalidSynAndAckPacket(TransportMode udpTransportMode, SynAndAck_InvalidType invalidType, uint? initSequenceNumber = null)
        {
            RdpeudpServerSocket rdpeudpSocket = rdpeudpSocketR;
            if (udpTransportMode == TransportMode.Lossy)
            {
                rdpeudpSocket = rdpeudpSocketL;
            }

            // Create the SYN and ACK packet.
            RdpeudpPacket SynAndAckPacket = new RdpeudpPacket();
            SynAndAckPacket.FecHeader.snSourceAck = rdpeudpSocket.SnSourceAck;
            SynAndAckPacket.FecHeader.uReceiveWindowSize = rdpeudpSocket.UReceiveWindowSize;
            SynAndAckPacket.FecHeader.uFlags = RDPUDP_FLAG.RDPUDP_FLAG_SYN | RDPUDP_FLAG.RDPUDP_FLAG_ACK;

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
        private void VerifySynPacket(RdpeudpPacket synPacket, TransportMode udpTransportMode)
        {
            if (synPacket == null)
            {
                this.Site.Assert.Fail("The SYN Packet should not be null!");
            }

            if (synPacket.FecHeader.snSourceAck != uint.MaxValue)
            {
                this.Site.Assert.Fail("The snSourceAck variable MUST be set to -1 (max value of uint)!");
            }

            if ((synPacket.FecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_SYN) == 0)
            {
                this.Site.Assert.Fail("The RDPUDP_FLAG_SYN flag MUST be set in SYN packet!");
            }

            if (udpTransportMode == TransportMode.Reliable)
            {
                if ((synPacket.FecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY) == RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY)
                {
                    this.Site.Assert.Fail("The RDPUDP_FLAG_SYNLOSSY flag MUST not be set when choose reliable UDP connection!");
                }
            }
            else
            {
                if ((synPacket.FecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY) == 0)
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

            // The RDPUDP_FLAG_SYNEX flag and RDPUDP_SYNDATAEX_PAYLOAD structure should appear at the same time.
            if ((synPacket.SynDataEx == null) ^ ((synPacket.FecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_SYNEX) == 0))
            {
                this.Site.Assert.Fail("Section 3.1.5.1.1: The RDPUDP_FLAG_SYNEX flag MUST be set only when the RDPUDP_SYNDATAEX_PAYLOAD structure is included. Section 3.1.5.1.1: The RDPUDP_SYNEX_PAYLOAD structure MUST be appended to the UDP datagram if the RDPUDP_FLAG_SYNEX flag is set in uFlags.");
            }

            //Section 3.1.5.1.1: Not appending RDPUDP_SYNDATAEX_PAYLOAD structure implies that RDPUDP_PROTOCOL_VERSION_1 is the highest protocol version supported. 
            if (synPacket.SynDataEx == null)
            {
                this.clientUUdpVer = RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_1;
                this.clientRdpudpVersionInfoValidFlag = null;
            }
            else
            {
                this.clientUUdpVer = synPacket.SynDataEx.Value.uUdpVer;
                this.clientRdpudpVersionInfoValidFlag = synPacket.SynDataEx.Value.uSynExFlags;

                //Section 3.1.5.1.1: The RDPUDP_VERSION_INFO_VALID flag MUST be set only if the structure contains a valid RDP-UDP protocol version.
                if (synPacket.SynDataEx.Value.uSynExFlags.HasFlag(RDPUDP_VERSION_INFO.RDPUDP_VERSION_INFO_VALID) && !Enum.IsDefined(synPacket.SynDataEx.Value.uUdpVer))
                {
                    this.Site.Assert.Fail("Section 3.1.5.1.1: The RDPUDP_VERSION_INFO_VALID flag MUST be set only if the structure contains a valid RDP-UDP protocol version");
                }
            }
        }

        /// <summary>
        /// Verify an ACK Packet.
        /// </summary>
        /// <param name="ackPacket">The ACK packet.</param>
        private void VerifyAckPacket(RdpeudpPacket ackPacket)
        {
            if (ackPacket == null)
            {
                this.Site.Assert.Fail("The ACK Packet should not be null!");
            }

            if ((ackPacket.FecHeader.uFlags & RDPUDP_FLAG.RDPUDP_FLAG_ACK) == 0)
            {
                this.Site.Assert.Fail("The RDPUDP_FLAG_ACK flag MUST be set in ACK packet!");
            }
        }

        #endregion

        private void CheckPlatformCompatibility(ref TransportMode[] transportModes)
        {
            var applicableTransportModes = transportModes.AsEnumerable();

            // Check lossy transport mode, which is currently only supported on Windows.
            if (applicableTransportModes.Any(transportMode => transportMode == TransportMode.Lossy))
            {
                if (!OperatingSystem.IsWindows())
                {
                    BaseTestSite.Log.Add(LogEntryKind.Comment, "The lossy transport mode is only supported on Windows.");

                    applicableTransportModes = applicableTransportModes.Where(transportMode => transportMode != TransportMode.Lossy);
                }
            }

            if (applicableTransportModes.Count() == 0)
            {
                BaseTestSite.Assume.Inconclusive("No transport mode is applicable to the running operating system.");
            }

            transportModes = applicableTransportModes.ToArray();
        }
    }
}
