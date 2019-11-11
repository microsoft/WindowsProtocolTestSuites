// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{
    public partial class RdpemtAdapter : ManagedAdapterBase
    {
        private Dictionary<Multitransport_Protocol_value, Server_Initiate_Multitransport_Request_PDU> serverInitiateMultitransportRequestPDUs = new Dictionary<Multitransport_Protocol_value, Server_Initiate_Multitransport_Request_PDU>();
        private Dictionary<Multitransport_Protocol_value, RdpeudpClient> rdpeudpClients = new Dictionary<Multitransport_Protocol_value, RdpeudpClient>();
        private Dictionary<Multitransport_Protocol_value, RdpemtClient> rdpemtClients = new Dictionary<Multitransport_Protocol_value, RdpemtClient>();
        private TestConfig testConfig;
        public RdpemtAdapter(TestConfig testConfig)
        {
            this.testConfig = testConfig;
        }

        /// <summary>
        /// Reset the Adapter
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            Disconnect();
        }

        /// <summary>
        /// Disconnect with server.
        /// </summary>
        public void Disconnect()
        {
            serverInitiateMultitransportRequestPDUs.Clear();

            foreach (var rdpemtClient in rdpemtClients.Values)
            {
                rdpemtClient.Dispose();
            }
            rdpemtClients.Clear();


            foreach (var rdpeudpClient in rdpeudpClients.Values)
            {
                rdpeudpClient.Stop();
                rdpeudpClient.Dispose();
            }
            rdpeudpClients.Clear();
        }



        /// <summary>
        /// Start multitransport connect with server.
        /// </summary>
        /// <param name="serverInitiateMultitransportRequestPDUs">The Server Intiate Multitransport Request PDUs sent by server.</param>

        public void StartMultitransportConnect(Server_Initiate_Multitransport_Request_PDU[] requests)
        {
            foreach (var request in requests)
            {
                TransportMode mode;

                switch (request.requestedProtocol)
                {
                    case Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR:
                        mode = TransportMode.Reliable;
                        break;

                    case Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL:
                        mode = TransportMode.Lossy;
                        break;

                    default:
                        throw new InvalidOperationException("Unexpected requestedProtocol!");
                }

                serverInitiateMultitransportRequestPDUs[request.requestedProtocol] = request;

                var rdpeudpClient = new RdpeudpClient(new IPEndPoint(testConfig.localAddress.ParseIPAddress(), 0), new IPEndPoint(testConfig.serverName.ParseIPAddress(), testConfig.serverPort), mode, true);

                rdpeudpClients[request.requestedProtocol] = rdpeudpClient;

                rdpeudpClient.Start();

                rdpeudpClient.Connect(testConfig.timeout);


                var rdpemtClient = new RdpemtClient(rdpeudpClient.Socket, testConfig.serverName, false);

                rdpemtClient.PDUReceived += VerifyPDU;

                rdpemtClients[request.requestedProtocol] = rdpemtClient;
            }
        }

        private void VerifyPDU(RdpemtBasePDU pdu)
        {
            VerifyTunnelHeader(pdu.TunnelHeader);

            switch (pdu.TunnelHeader.Action)
            {
                case RDP_TUNNEL_ACTION_Values.RDPTUNNEL_ACTION_CREATERESPONSE:
                    Site.Assert.IsInstanceOfType(pdu, typeof(RDP_TUNNEL_CREATERESPONSE), "When Action is RDPTUNNEL_ACTION_CREATEREQUEST, PDU type should be RDP_TUNNEL_CREATERESPONSE.");
                    break;

                case RDP_TUNNEL_ACTION_Values.RDPTUNNEL_ACTION_DATA:
                    Site.Assert.IsInstanceOfType(pdu, typeof(RDP_TUNNEL_DATA), "When Action is RDPTUNNEL_ACTION_CREATEREQUEST, PDU type should be RDP_TUNNEL_DATA.");
                    break;

                default:
                    Site.Assert.Fail("Unexpected Action value {0} from server.", pdu.TunnelHeader.Action);
                    break;
            }
        }

        private void VerifyTunnelHeader(RDP_TUNNEL_HEADER header)
        {
            Site.Assert.AreEqual(0, header.Flags, "Flags MUST be set to zero.");

            if (header.HeaderLength > 4)
            {
                Site.Assert.IsNotNull(header.SubHeaders, "If HeaderLength is larger than 4 bytes, then the SubHeaders field MUST be present.");
            }

            if (header.SubHeaders != null)
            {
                foreach (var subHeader in header.SubHeaders)
                {
                    Site.Assert.IsTrue(subHeader.SubHeaderLength >= 2, "This SubHeaderLength MUST be a minimum of 0x2 bytes since the SubHeaderLength and SubHeaderType fields are an implicit part of the header.");
                }
            }
        }

        /// <summary>
        /// Send Tunnel Create Request PDU to server.
        /// </summary>
        /// <param name="negativeType">The negative type want to test.</param>
        /// <param name="requestedProtocol">The request multitransport protocol.</param>
        public void SendTunnelCreateRequest(NegativeType negativeType, Multitransport_Protocol_value requestedProtocol)
        {
            var serverInitiateMultitransportRequestPDU = serverInitiateMultitransportRequestPDUs[requestedProtocol];
            uint requestId = serverInitiateMultitransportRequestPDU.requestId;
            var securityCookie = serverInitiateMultitransportRequestPDU.securityCookie;

            bool modifyRequestID = false;
            bool modifySecurityCookie = false;

            switch (negativeType)
            {
                case NegativeType.TunnelCreateRequest_InvalidRequestID:
                    modifyRequestID = true;
                    break;

                case NegativeType.TunnelCreateRequest_InvalidSecurityCookie:
                    modifySecurityCookie = true;
                    break;

                case NegativeType.TunnelCreateRequest_InvalidRequestIDAndSecurityCookie:
                    modifyRequestID = true;
                    modifySecurityCookie = true;
                    break;
            }

            if (modifyRequestID)
            {
                requestId = (uint)RequestID_Values.Invalid;
            }

            if (modifySecurityCookie)
            {
                securityCookie = securityCookie.Select(x => (byte)~x).ToArray();
            }

            var createTunnelCreateRequestPDU = rdpemtClients[requestedProtocol].CreateTunnelCreateRequest(requestId, securityCookie);

            rdpemtClients[requestedProtocol].SendRdpemtPacket(createTunnelCreateRequestPDU);
        }

        /// <summary>
        /// Expect the Tunnel Create Reponse PDU from server.
        /// </summary>
        /// <param name="requestedProtocol">The requested multitransport protocol.</param>
        /// <returns>The Tunnel Create Response PDU sent by server.</returns>
        public RDP_TUNNEL_CREATERESPONSE ExpectTunnelCreateResponse(Multitransport_Protocol_value requestedProtocol)
        {
            return rdpemtClients[requestedProtocol].ExpectTunnelCreateResponse(testConfig.timeout);
        }
    }
}
