// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;

namespace Microsoft.Protocols.TestSuites.Rdpemt
{
    public interface IRdpemtAdapter : IAdapter
    {
        /// <summary>
        /// Start multitransport connect with server.
        /// </summary>
        /// <param name="serverInitiateMultitransportRequestPDUs">The Server Intiate Multitransport Request PDUs sent by server.</param>
        void StartMultitransportConnect(Server_Initiate_Multitransport_Request_PDU[] serverInitiateMultitransportRequestPDUs);

        /// <summary>
        /// Send Tunnel Create Request PDU to server.
        /// </summary>
        /// <param name="negativeType">The negative type want to test.</param>
        /// <param name="requestedProtocol">The request multitransport protocol.</param>
        void SendTunnelCreateRequest(NegativeType negativeType, Multitransport_Protocol_value requestedProtocol);

        /// <summary>
        /// Expect the Tunnel Create Reponse PDU from server.
        /// </summary>
        /// <param name="requestedProtocol">The requested multitransport protocol.</param>
        /// <returns>The Tunnel Create Response PDU sent by server.</returns>
        RDP_TUNNEL_CREATERESPONSE ExpectTunnelCreateResponse(Multitransport_Protocol_value requestedProtocol);

        /// <summary>
        /// Disconnect with server.
        /// </summary>
        void Disconnect();
    }
}
