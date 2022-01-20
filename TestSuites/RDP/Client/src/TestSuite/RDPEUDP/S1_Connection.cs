// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        [TestCategory("BasicRequirement")]
        [TestCategory("BasicFeature")]
        [Description("Verify the RDP client can initiate a reliable RDP-UDP (RDP-UDP-R) connection and a lossy RDP-UDP (RDP-UDP-L) connection.")]
        public void S1_Connection_Initialization_InitialUDPConnection()
        {
            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };

            CheckPlatformCompatibility(ref transportModeArray);

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            foreach (var transportMode in transportModeArray)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, $"Create a {transportMode} UDP connection.");

                this.EstablishUDPConnection(transportMode, waitTime, true);
            }
        }
        #endregion BVT Test Cases

        #region Normal Test Cases
        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]
        [TestCategory("BasicRequirement")]
        [TestCategory("BasicFeature")]
        [Description("Verify the RDP client can initiate a reliable RDP-UDP (RDP-UDP-R) connection and a lossy RDP-UDP (RDP-UDP-L) connection. Server supports only RDPUDP_PROTOCOL_VERSION_1.")]
        public void S1_Connection_Initialization_InitialUDPConnection_UUDPVer1()
        {
            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };

            CheckPlatformCompatibility(ref transportModeArray);

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            foreach (var transportMode in transportModeArray)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, $"Create a {transportMode} UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_1.");

                this.EstablishUDPConnection(transportMode, waitTime, true, false, RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_1);

                Site.Log.Add(LogEntryKind.Debug, $"Client accept the server's {transportMode} UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_1.");
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEUDP")]
        [TestCategory("BasicRequirement")]
        [TestCategory("BasicFeature")]
        [Description("Verify the RDP client can initiate a reliable RDP-UDP (RDP-UDP-R) connection and a lossy RDP-UDP (RDP-UDP-L) connection. Server supports highest version RDPUDP_PROTOCOL_VERSION_2.")]
        public void S1_Connection_Initialization_InitialUDPConnection_UUDPVer2()
        {
            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };

            CheckPlatformCompatibility(ref transportModeArray);

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            foreach (var transportMode in transportModeArray)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, $"Create a {transportMode} UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_2.");

                this.EstablishUDPConnection(transportMode, waitTime, true, false, RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_1 | RDPUDP_PROTOCOL_VERSION.RDPUDP_PROTOCOL_VERSION_2);

                Site.Log.Add(LogEntryKind.Debug, $"Client accept the server's {transportMode} UDP connection with uUdpVer RDPUDP_PROTOCOL_VERSION_2.");
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEUDP")]
        [TestCategory("BasicRequirement")]
        [TestCategory("BasicFeature")]
        [Description("Verify the RDP client will resend the ACK packet to keep alive.")]
        public void S1_Connection_Keepalive_ClientSendKeepAlive()
        {
            TransportMode[] transportModeArray = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };

            CheckPlatformCompatibility(ref transportModeArray);

            CheckSecurityProtocolForMultitransport();

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            foreach (TransportMode transportMode in transportModeArray)
            {
                DoUntilSucceed(() =>
                {
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", transportMode);
                    this.EstablishUDPConnection(transportMode, waitTime, true);

                    this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", transportMode);
                    return this.EstablishRdpemtConnection(transportMode, waitTime);
                },
                this.waitTime * 5,
                TimeSpan.FromSeconds(0.5),
                "RDPEMT tunnel creation failed");

                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect RDP client to send an ACK packet to keep alive or acknowledge the receipt");
                RdpeudpPacket ackpacket = WaitForAckPacket(transportMode, waitTime);
                Site.Assert.IsNotNull(ackpacket, "Client should send an ACK to keep alive or acknowledge the receipt of source packet. Transport mode is {0}", transportMode);


                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect RDP client to resend the ACK packet to keep alive");
                ackpacket = WaitForAckPacket(transportMode, waitTime);
                Site.Assert.IsNotNull(ackpacket, "Client should resend the ACK packet to keep alive. Transport mode is {0}", transportMode);
            }
        }


        #endregion Normal Test Cases

    }
}