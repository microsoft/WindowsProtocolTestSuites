// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{
    public partial class RdpemtTestSuite : RdpTestClassBase
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]        
        [Description("Verify the RDP client can set up a RDPEMT connection upon a reliable RDP-UDP connection.")]
        public void S1_Connection_Initialization_InitialReliableConnection()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", TransportMode.Reliable);
            this.EstablishUDPConnection(TransportMode.Reliable, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", TransportMode.Reliable);
            this.EstablishRdpemtConnection(TransportMode.Reliable, waitTime, true);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]        
        [Description("Verify the RDP client can set up a RDPEMT connection upon a Lossy RDP-UDP connection.")]
        public void S1_Connection_Initialization_InitialLossyConnection()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", TransportMode.Lossy);
            this.EstablishUDPConnection(TransportMode.Lossy, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} RDPEMT connection.", TransportMode.Lossy);
            this.EstablishRdpemtConnection(TransportMode.Lossy, waitTime, true);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]        
        [Description("Verify the RDP client will drop the RDP-UDP reliable connection if RDP-TCP connection uses RDP encryption method.")]
        public void S1_Connection_Initialization_NegativeTest_InitialReliableConnection_RDPEncryption()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection, used RDP encryption");
            StartRDPConnection(true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", TransportMode.Reliable);
            this.EstablishUDPConnection(TransportMode.Reliable, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start TLS handshake.");
            String certFile = this.Site.Properties["CertificatePath"];
            String certPwd = this.Site.Properties["CertificatePassword"];
            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);
            RdpeudpTLSChannel sChannel = new RdpeudpTLSChannel(rdpeudpSocketR);
            sChannel.AuthenticateAsServer(cert);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect for Client Initiate Multitransport Error PDU to indicate Client drop RDP-UDP connection");
            this.rdpbcgrAdapter.WaitForPacket<Client_Initiate_Multitransport_Response_PDU>(waitTime);

            if(requestIdList.Count == 1)
                VerifyClientInitiateMultitransportResponsePDU(rdpbcgrAdapter.SessionContext.ClientInitiateMultitransportResponsePDU, requestIdList[0]);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEMT")]        
        [Description("Verify the RDP client will drop the RDP-UDP lossy connection if RDP-TCP connection uses RDP encryption method.")]
        public void S1_Connection_Initialization_NegativeTest_InitialLossyConnection_RDPEncryption()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection, used RDP encryption");
            StartRDPConnection(true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a {0} UDP connection.", TransportMode.Lossy);
            this.EstablishUDPConnection(TransportMode.Lossy, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start DTLS handshake.");
            String certFile = this.Site.Properties["CertificatePath"];
            String certPwd = this.Site.Properties["CertificatePassword"];
            X509Certificate2 cert = new X509Certificate2(certFile, certPwd);
            RdpeudpDTLSChannel sChannel = new RdpeudpDTLSChannel(rdpeudpSocketL);
            sChannel.AuthenticateAsServer(cert);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect for Client Initiate Multitransport Error PDU to indicate Client drop RDP-UDP connection");
            this.rdpbcgrAdapter.WaitForPacket<Client_Initiate_Multitransport_Response_PDU>(waitTime);

            if (requestIdList.Count == 1)
                VerifyClientInitiateMultitransportResponsePDU(rdpbcgrAdapter.SessionContext.ClientInitiateMultitransportResponsePDU, requestIdList[0]);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEMT")]
        [Description("Verify the RDP client can handle soft sync connection using RDP-UDP-L.")]
        public void S1_Connection_SoftSync_Lossy()
        {
            this.TestSite.Assert.IsTrue(isClientSupportSoftSync, "SUT should support Soft-Sync.");
            StartSoftSyncConnection(TransportMode.Lossy);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEMT")]
        [Description("Verify the RDP client can handle soft sync connection using RDP-UDP-R.")]
        public void S1_Connection_SoftSync_Reliable()
        {
            this.TestSite.Assert.IsTrue(isClientSupportSoftSync, "SUT should support Soft-Sync.");
            StartSoftSyncConnection(TransportMode.Reliable);
        }
            
    }
}
