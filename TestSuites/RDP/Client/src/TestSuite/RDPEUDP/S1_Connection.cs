// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;
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
        [Description("Verify the RDP client can initiate a reliable RDP-UDP (RDP-UDP-R) connection and a lossy RDP-UDP (RDP-UDP-L) connection.")]
        public void S1_Connection_Initialization_InitialUDPConnection()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a reliable UDP connection.");
            this.EstablishUDPConnection(TransportMode.Reliable, waitTime, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Lossy UDP connection.");
            this.EstablishUDPConnection(TransportMode.Lossy, waitTime, true);
        }
        #endregion BVT Test Cases

        #region Normal Test Cases
        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]
        [Description("Verify the RDP client can initiate a reliable RDP-UDP (RDP-UDP-R) connection and a lossy RDP-UDP (RDP-UDP-L) connection. Server supports only RDPUDP_PROTOCOL_VERSION_1.")]
        public void S1_Connection_Initialization_InitialUDPConnection_UUDPVer1()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a reliable UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_1.");
            this.EstablishUDPConnection(TransportMode.Reliable, waitTime, true, false, uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_1);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Lossy UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_1.");
            this.EstablishUDPConnection(TransportMode.Lossy, waitTime, true, false, uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_1);

            Site.Log.Add(LogEntryKind.Debug, "Client accept the server's UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_1.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEUDP")]
        [Description("Verify the RDP client can initiate a reliable RDP-UDP (RDP-UDP-R) connection and a lossy RDP-UDP (RDP-UDP-L) connection. Server supports highest version RDPUDP_PROTOCOL_VERSION_2.")]
        public void S1_Connection_Initialization_InitialUDPConnection_UUDPVer2()
        {                 
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a reliable UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_2.");
            this.EstablishUDPConnection(TransportMode.Reliable, waitTime, true, false, uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_1 | uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_2);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a Lossy UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_2.");
            this.EstablishUDPConnection(TransportMode.Lossy, waitTime, true, false, uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_1 | uUdpVer_Values.RDPUDP_PROTOCOL_VERSION_2);

            Site.Log.Add(LogEntryKind.Debug, "Client accept the server's UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_2.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]        
        [Description("Verify the RDP client will resend the ACK packet to keep alive.")]
        public void S1_Connection_Keepalive_ClientSendKeepAlive()
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

                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect RDP client to send an ACK packet to keep alive or acknowledge the receipt");
                RdpeudpPacket ackpacket = WaitForACKPacket(transportMode, waitTime);
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK to keep alive or acknowledge the receipt of source packet. Transport mode is {0}", transportMode);
                

                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect RDP client to resend the ACK packet to keep alive");
                ackpacket = WaitForACKPacket(transportMode, waitTime);
                Site.Assert.IsNotNull(ackpacket, "Client should resend the ACK packet to keep alive. Transport mode is {0}", transportMode);                
            }
        }
        

        #endregion Normal Test Cases

    }
}