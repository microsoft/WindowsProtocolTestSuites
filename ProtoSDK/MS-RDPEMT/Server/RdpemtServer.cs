// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    public class RdpemtServer : RdpemtTransport
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="secChannel">secure channel</param>
        /// <param name="autoHandle">Whether it is an auto handle connection</param>
        public RdpemtServer(ISecureChannel secChannel, bool autoHandle = true)
            :base(secChannel, autoHandle)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="cert"></param>
        /// <param name="autoHandle"></param>
        public RdpemtServer(RdpeudpSocket socket, X509Certificate2 cert, bool autoHandle = true)
            :base(autoHandle)
        {
            if (!socket.AutoHandle)
            {
                throw new NotSupportedException("To Create RDPEMT Server, RDPEUDP Socket must be auto handle.");
            }

            if (socket.TransMode == TransportMode.Reliable)
            {
                RdpeudpTLSChannel secChannel = new RdpeudpTLSChannel(socket);
                secChannel.Received += ReceiveBytes;
                secChannel.AuthenticateAsServer(cert);
                this.secureChannel = secChannel;
            }
            else
            {
                RdpeudpDTLSChannel secChannel = new RdpeudpDTLSChannel(socket);
                secChannel.Received += ReceiveBytes;
                secChannel.AuthenticateAsServer(cert);
                this.secureChannel = secChannel;
            }

        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Expect a connection request from client.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="requestId">The requestID should be set in Tunnel Create Request.</param>
        /// <param name="securityCooke">The SecurityCookie should be set in Tunnel Create Request.</param>
        public bool ExpectConnect(TimeSpan timeout, out uint requestId, out byte[] securityCooke)
        {
            // Expect a Tunnel Create Request.
            RDP_TUNNEL_CREATEREQUEST createReq = this.ExpectTunnelCreateRequest(timeout);
            if (createReq == null)
            {
                requestId = 0;
                securityCooke = null;
                return false;
            }
            requestId = createReq.RequestID;
            securityCooke = createReq.SecurityCookie;

            // Respond a Tunnel Create Response. 
            RDP_TUNNEL_CREATERESPONSE createRes = this.CreateTunnelCreateResponse(HrResponse_S_OK);
            this.SendRdpemtPacket(createRes);
            connected = true;

            return true;
        }

        /// <summary>
        /// Create a RDP_TUNNEL_CREATERESPONSE pdu
        /// </summary>
        /// <param name="hrRes"></param>
        /// <returns></returns>
        public RDP_TUNNEL_CREATERESPONSE CreateTunnelCreateResponse(uint hrRes)
        {
            RDP_TUNNEL_CREATERESPONSE createRes = new RDP_TUNNEL_CREATERESPONSE();
            createRes.TunnelHeader = new RDP_TUNNEL_HEADER();
            createRes.TunnelHeader.Action = RDP_TUNNEL_ACTION_Values.RDPTUNNEL_ACTION_CREATERESPONSE;
            createRes.TunnelHeader.Flags = 0;
            createRes.TunnelHeader.PayloadLength = 4;
            createRes.TunnelHeader.HeaderLength = 4;
            createRes.TunnelHeader.SubHeaders = null;
            createRes.HrResponse = hrRes;
            return createRes;
        }

        /// <summary>
        /// Expect to receive a RDP_TUNNEL_CREATEREQUEST
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RDP_TUNNEL_CREATEREQUEST ExpectTunnelCreateRequest(TimeSpan timeout)
        {
            if (Connected)
            {
                return null;
            }

            DateTime endTime = DateTime.Now + timeout;
            RDP_TUNNEL_CREATEREQUEST createReq = null;
            while (DateTime.Now < endTime)
            {
                if (receiveBuffer.Count > 0)
                {
                    lock (receiveBuffer)
                    {
                        if (receiveBuffer.Count > 0)
                        {
                            for (int i = 0; i < receiveBuffer.Count; i++)
                            {
                                if (receiveBuffer[i] is RDP_TUNNEL_CREATEREQUEST)
                                {
                                    createReq = receiveBuffer[i] as RDP_TUNNEL_CREATEREQUEST;
                                    receiveBuffer.RemoveAt(i);
                                    return createReq;
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(waitInterval);
            }

            return null;
        }
        
        /// <summary>
        /// Process Auto Detect feature
        /// </summary>
        /// <param name="pdu"></param>
        public override void ProcessSubHeaders(RdpemtBasePDU pdu)
        {
            RDP_TUNNEL_SUBHEADER[] RDPTunnelSubHeaders = pdu.TunnelHeader.SubHeaders;
            if (RDPTunnelSubHeaders != null)
            {
                foreach (RDP_TUNNEL_SUBHEADER RDPTunnelSubHeader in RDPTunnelSubHeaders)
                {
                    if (RDPTunnelSubHeader.SubHeaderType == RDP_TUNNEL_SUBHEADER_TYPE_Values.TYPE_ID_AUTODETECT_RESPONSE)
                    {
                        uint responseType = (uint)(RDPTunnelSubHeader.SubHeaderData[3] << 8) + RDPTunnelSubHeader.SubHeaderData[2];
                        if (responseType == (uint)AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT)
                        {
                            RDP_BW_RESULTS bwRes = RdpemtUtility.ParseRdpBWResults(RDPTunnelSubHeader.SubHeaderData);
                            this.bandwidth = (bwRes.byteCount * 8) / bwRes.timeDelta;
                        }
                    }
                }
            }
            
        }

        #endregion Public Methods
    }
}
