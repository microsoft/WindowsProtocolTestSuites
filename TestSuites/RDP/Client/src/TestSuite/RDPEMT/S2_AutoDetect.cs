// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{

    public partial class RdpemtTestSuite : RdpTestClassBase
    {
        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]        
        [Description("Verify the RDP client can response to round-trip measurement operations initiated by the RTT Measure Request correctly.")]
        public void S2_AutoDetect_RTTMeasure()
        {
            ushort sequenceNumber = 1;

            TransportMode[] transportModeList = new TransportMode[] { TransportMode.Reliable, TransportMode.Lossy };

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            foreach (TransportMode mode in transportModeList)
            {
                Multitransport_Protocol_value protocolValue = (mode == TransportMode.Reliable)?Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR:Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL;

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", mode);
                this.EstablishUDPConnection(mode, waitTime);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", mode);
                this.EstablishRdpemtConnection(mode, waitTime, true);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send a RTT Measure Request");
                this.SendTunnelDataPdu_RTTMeasureRequest(protocolValue, ++sequenceNumber);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect a RTT Measure Response");
                this.WaitForAndCheckTunnelDataPdu_RTTResponse(protocolValue, sequenceNumber, timeout);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Network Characteristics Result");
                this.SendTunnelDataPdu_NetcharResult(protocolValue, sequenceNumber);
            }
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]        
        [Description("Verify the RDP client can complete the bandwidth measure auto detection in a reliable UDP transport.")]
        public void S2_AutoDetect_ReliableBandwidthMeasure()
        {
            ushort sequenceNumber = 1;

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", TransportMode.Reliable);
            this.EstablishUDPConnection(TransportMode.Reliable, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", TransportMode.Reliable);
            this.EstablishRdpemtConnection(TransportMode.Reliable, waitTime, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Bandwidth Measure Start");
            this.SendTunnelDataPdu_BWStart(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR, sequenceNumber);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send some random data");
            this.SendRandomTunnelData(TransportMode.Reliable);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Bandwidth Measure Stop");
            this.SendTunnelDataPdu_BWStop(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR, sequenceNumber);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect a Bandwidth Measure Result");
            this.WaitForAndCheckTunnelDataPdu_BWResult(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR, sequenceNumber, timeout);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Network Characteristics Result");
            this.SendTunnelDataPdu_NetcharResult(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR, sequenceNumber);

        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]        
        [Description("Verify the RDP client can complete the bandwidth measure auto detection in a lossy UDP transport.")]
        public void S2_AutoDetect_LossyBandwidthMeasure()
        {
            ushort sequenceNumber = 2;

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", TransportMode.Lossy);
            this.EstablishUDPConnection(TransportMode.Lossy, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", TransportMode.Lossy);
            this.EstablishRdpemtConnection(TransportMode.Lossy, waitTime, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Bandwidth Measure Start");
            this.SendTunnelDataPdu_BWStart(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL, sequenceNumber);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send some random data");
            this.SendRandomTunnelData(TransportMode.Lossy);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Bandwidth Measure Stop");
            this.SendTunnelDataPdu_BWStop(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL, sequenceNumber);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect a Bandwidth Measure Result");
            this.WaitForAndCheckTunnelDataPdu_BWResult(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL, sequenceNumber, timeout);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Network Characteristics Result");
            this.SendTunnelDataPdu_NetcharResult(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL, sequenceNumber);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]        
        [Description("Verify the RDP client doesn't respond Bandwidth detection request in a lossy UDP transport if the sequenceNumber field in the Bandwidth Measure Stop structure is not the same as that in the Bandwidth Measure Start structure.")]
        public void S2_AutoDetect_NegtiveLossyBandwidthMeasure()
        {
            ushort sequenceNumber = 2;

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", TransportMode.Lossy);
            this.EstablishUDPConnection(TransportMode.Lossy, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", TransportMode.Lossy);
            this.EstablishRdpemtConnection(TransportMode.Lossy, waitTime, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Bandwidth Measure Start");
            this.SendTunnelDataPdu_BWStart(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL, sequenceNumber);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send some random data");
            this.SendRandomTunnelData(TransportMode.Lossy);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send a Bandwidth Measure Stop");
            this.SendTunnelDataPdu_BWStop(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL, (ushort)(sequenceNumber+1000));

            this.TestSite.Log.Add(LogEntryKind.Comment, "Try to wait for a Bandwidth Measure Result");
            this.WaitForAndCheckTunnelDataPdu_BWResult(Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL, (ushort)(sequenceNumber + 1000), timeout, true);
        }
    }
}
