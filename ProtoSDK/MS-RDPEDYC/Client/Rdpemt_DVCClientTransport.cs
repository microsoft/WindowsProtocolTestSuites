// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    public class Rdpemt_DVCClientTransport : IDVCTransport
    {
        #region Variables

        private Multitransport_Protocol_value transportProtocol;

        private TimeSpan timeout = new TimeSpan(0, 0, 20);

        private int portR = 65011;
        private int portL = 65012;

        private RdpbcgrClientContext clientSessionContext = null;
        private RdpemtClient rdpemtClient = null;
        private RdpeudpClient rdpeudpClient = null;

        private ClientDecodingPduBuilder decoder;
        private PduBuilder pduBuilder;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Transport Type
        /// </summary>
        public DynamicVC_TransportType TransportType
        {
            get
            {
                if (transportProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
                {
                    return DynamicVC_TransportType.RDP_UDP_Lossy;
                }
                return DynamicVC_TransportType.RDP_UDP_Reliable;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Construcotr
        /// </summary>
        /// <param name="clientSessionContext"></param>
        /// <param name="transportType"></param>
        public Rdpemt_DVCClientTransport(RdpbcgrClientContext clientSessionContext, DynamicVC_TransportType transportType)
        {
            this.clientSessionContext = clientSessionContext;
            this.transportProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR;
            if (transportType == DynamicVC_TransportType.RDP_UDP_Lossy)
            {
                this.transportProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL;
            }


            EstablishTransportConnection();


            decoder = new ClientDecodingPduBuilder();
            pduBuilder = new PduBuilder();
        }
        #endregion 

        #region Public Methods

        /// <summary>
        /// Send a Dynamic virtual channel PDU
        /// </summary>
        /// <param name="pdu"></param>
        public void Send(DynamicVCPDU pdu)
        {
            rdpemtClient.Send(pduBuilder.ToRawData(pdu));
        }

        /// <summary>
        /// Event called when receive a Dynamic Virtual channel PDU
        /// </summary>
        public event ReceivePacket Received;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (rdpemtClient != null)
            {
                rdpemtClient.Dispose();
            }
            if (rdpeudpClient != null)
            {
                rdpeudpClient.Dispose();
            }
        }

        #endregion Public Methods

        #region Private methods

        /// <summary>
        /// Process bytes received 
        /// </summary>
        /// <param name="data"></param>
        private void ReceivedBytes(byte[] data)
        {
            DynamicVCPDU pdu = decoder.ToPdu(data);
            if (Received != null)
            {
                Received(pdu);
            }
        }

        /// <summary>
        /// Establish a MultiTransport Connection
        /// </summary>
        public void EstablishTransportConnection()
        {
            TransportMode udpTransportMode = TransportMode.Reliable;
            uint requestId = clientSessionContext.RequestIdReliable;
            byte[] cookie = clientSessionContext.CookieReliable;
            int port = portR;
            if (transportProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                udpTransportMode = TransportMode.Lossy;
                requestId = clientSessionContext.RequestIdLossy;
                cookie = clientSessionContext.CookieLossy;
                port = portL;
            }

            if(cookie == null)
            {
                // Not receive a Server Initiate Multitransport Request PDU
                throw new InvalidOperationException("Cannot establish the connection, since the corresponding Server Initiate Multitransport Request PDU wasn't received!");
            }

            IPEndPoint localEndpoint = new IPEndPoint(((IPEndPoint)clientSessionContext.LocalIdentity).Address, port);
            rdpeudpClient = new RdpeudpClient(localEndpoint, (IPEndPoint)clientSessionContext.RemoteIdentity, udpTransportMode);
            if (!rdpeudpClient.Running)
            {
                rdpeudpClient.Start();
            }
            rdpeudpClient.Connect(timeout);

            rdpemtClient = new RdpemtClient(rdpeudpClient.Socket, ((IPEndPoint)clientSessionContext.RemoteIdentity).Address.ToString(), false);
            rdpemtClient.Received += ReceivedBytes;

            rdpemtClient.Connect(requestId, cookie, timeout);
            
        }

        #endregion Private Methods
    }
}
