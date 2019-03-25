// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    public class Rdpemt_DVCServerTransport : IDVCTransport
    {
        #region Variables

        private Multitransport_Protocol_value transportProtocol;

        private TimeSpan timeout = new TimeSpan(0, 0, 20);

        private RdpbcgrServer rdpbcgrServer = null;
        private RdpbcgrServerSessionContext serverSessionContext = null;
        private RdpemtServer rdpemtServer = null;
        private RdpeudpServer rdpeudpServer = null;
        private static readonly object rdpeudpServerLock = new object();
        private static uint multitransportId = 0;
        private RdpeudpSocket rdpeudpSocket = null;

        private ServerDecodingPduBuilder decoder;
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
        /// Constructor
        /// </summary>
        /// <param name="connectedRdpServer"></param>
        /// <param name="context"></param>
        /// <param name="transportType"></param>
        public Rdpemt_DVCServerTransport(RdpbcgrServer connectedRdpServer, RdpbcgrServerSessionContext context, DynamicVC_TransportType transportType)
        {
            this.serverSessionContext = context;
            this.rdpbcgrServer = connectedRdpServer;
            this.transportProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECR;
            if (transportType == DynamicVC_TransportType.RDP_UDP_Lossy)
            {
                this.transportProtocol = Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL;
            }

            if (!serverSessionContext.IsClientMultitransportChannelDataRecieved)
            {
                throw new NotSupportedException("This RDP connection doesn't support multiple transport!");
            }

            try
            {
                EstablishTransportConnection();
            }
            catch (Exception)
            {
                // Ensure resource can be released properly if exception occurred in Constructor 
                Dispose();
                // Not suppress the exception, transfer the error to test result. 
                throw;
            }

            decoder = new ServerDecodingPduBuilder();
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
            rdpemtServer.Send(pduBuilder.ToRawData(pdu));
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
            if (rdpemtServer != null)
            {
                rdpemtServer.Dispose();
            }
            if (rdpeudpServer != null)
            {
                rdpeudpServer.Dispose();
            }
            if (rdpeudpSocket != null && rdpeudpSocket.Connected)
            {
                rdpeudpSocket.Close();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Process bytes Received
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
        private void EstablishTransportConnection()
        {
            // Send the Server Initial multitransport
            byte[] securityCookie = new byte[16];
            Random rnd = new Random();
            rnd.NextBytes(securityCookie);

            Server_Initiate_Multitransport_Request_PDU requestPDU = rdpbcgrServer.CreateServerInitiateMultitransportRequestPDU(serverSessionContext, ++multitransportId, transportProtocol, securityCookie);
            rdpbcgrServer.SendPdu(serverSessionContext, requestPDU);

            //Create RDP-UDP Connection
            CreateRdpeudpServer(this.serverSessionContext);
            TransportMode transMode = TransportMode.Reliable;
            if (transportProtocol == Multitransport_Protocol_value.INITITATE_REQUEST_PROTOCOL_UDPFECL)
            {
                transMode = TransportMode.Lossy;
            }

            rdpeudpSocket = rdpeudpServer.Accept(((IPEndPoint)serverSessionContext.Identity).Address, transMode, timeout);
            if (rdpeudpSocket == null)
            {
                if (rdpeudpServer != null && rdpeudpServer.Running)
                    rdpeudpServer.Dispose();

                throw new NotSupportedException("RDPEMT Server create rdpedupSocket failed.");
            }
            rdpemtServer = new RdpemtServer(rdpeudpSocket, rdpbcgrServer.AuthCertificate, true);
            rdpemtServer.Received += ReceivedBytes;

            uint receivedRequestId;
            byte[] receivedCookie;
            if (!rdpemtServer.ExpectConnect(timeout, out receivedRequestId, out receivedCookie))
            {
                throw new ProtocolViolationException("RDPEMT Server Expect Connection failed");
            }
            if (receivedRequestId != multitransportId || receivedCookie == null || receivedCookie.Length != 16)
            {
                throw new ProtocolViolationException("RDPEMT Server received a connection with un-expected request id or Cookie is null (or cookie's length is not 16)!");
            }

            for (int i = 0; i < receivedCookie.Length; i++)
            {
                if (receivedCookie[i] != securityCookie[i])
                {
                    throw new ProtocolViolationException("RDPEMT Server received a connection with un-correct cookie!");
                }
            }

        }

        /// <summary>
        /// Only on RDPEUDP Server can exist
        /// </summary>
        /// <param name="context"></param>
        private void CreateRdpeudpServer(RdpbcgrServerSessionContext context)
        {
            if (rdpeudpServer == null)
            {
                rdpeudpServer = new RdpeudpServer((IPEndPoint)context.LocalIdentity);
            }
            if (!rdpeudpServer.Running)
                rdpeudpServer.Start();
        }

        #endregion Priate Methods
    }
}
