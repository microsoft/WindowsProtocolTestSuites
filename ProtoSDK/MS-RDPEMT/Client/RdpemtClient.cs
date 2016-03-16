// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    public class RdpemtClient : RdpemtTransport
    {
        #region Private variables

        // variables for Auto-Detect
        private DateTime bandwidthMeasureStartTime;
        private UInt32 bandwidthMeasurePayloadByteCount;
        private UInt32 timeDelta;

        private bool detectBandwidth;

        private ushort bwStopSequenceNumber;

        #endregion Private variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="secChannel">secure channel</param>
        /// <param name="autoHandle">Whether it is an auto handle connection</param>
        public RdpemtClient(ISecureChannel secChannel, bool autoHandle = true)
            :base(secChannel, autoHandle)
        {
            bandwidthMeasureStartTime = DateTime.Now;
            bandwidthMeasurePayloadByteCount = 0;
            detectBandwidth = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="targetHost"></param>
        /// <param name="autoHandle"></param>
        public RdpemtClient(RdpeudpSocket socket, string targetHost, bool autoHandle = true)
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
                secChannel.AuthenticateAsClient(targetHost);
                this.secureChannel = secChannel;
            }
            else
            {
                RdpeudpDTLSChannel secChannel = new RdpeudpDTLSChannel(socket);
                secChannel.Received += ReceiveBytes;
                secChannel.AuthenticateAsClient(targetHost);
                this.secureChannel = secChannel;
            }

            bandwidthMeasureStartTime = DateTime.Now;
            bandwidthMeasurePayloadByteCount = 0;
            detectBandwidth = false;
        }
        #endregion Constructor

        #region Properties

        public bool IsMeasuringBandwidth
        {
            get
            {
                return detectBandwidth;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Establish a connection to server.
        /// </summary>
        /// <param name="requestId">The RequestID to be set in Tunnel Create Request PDU. </param>
        /// <param name="securityCooke">The SecurityCookie to be set in Tunnel Create Request PDU.</param>
        public bool Connect(uint requestId, byte[] securityCookie, TimeSpan timeout)
        {
            // Send a Tunnel Create Request.
            RDP_TUNNEL_CREATEREQUEST request = this.CreateTunnelCreateRequest(requestId, securityCookie);
            SendRdpemtPacket(request);

            // Expect a Tunnel Create Response.
            RDP_TUNNEL_CREATERESPONSE response = this.ExpectTunnelCreateResponse(timeout);

            if (null == response || response.HrResponse != HrResponse_S_OK)
            {
                return false;
            }
            else
            {
                connected = true;
                return true;
            }
        }

        /// <summary>
        /// Create a RDP_TUNNEL_CREATEREQUEST
        /// </summary>
        /// <param name="reqestId">A 32-bit unsigned integer that contains the request ID included in the Initiate Multitransport Request PDU ([MS-RDPBCGR] section 2.2.15.1) that was sent over the main RDP connection.</param>
        /// <param name="securityCookie">A 16-byte element array of 8-bit unsigned integers that contains the security cookie included in the Initiate Multitransport Request PDU that was sent over the main RDP connection.</param>
        /// <returns>A RDP_TUNNEL_CREATEREQUEST instance.</returns>
        public RDP_TUNNEL_CREATEREQUEST CreateTunnelCreateRequest(uint reqestId, byte[] securityCookie)
        {
            RDP_TUNNEL_CREATEREQUEST createRes = new RDP_TUNNEL_CREATEREQUEST();
            createRes.TunnelHeader = new RDP_TUNNEL_HEADER();
            createRes.TunnelHeader.Action = RDP_TUNNEL_ACTION_Values.RDPTUNNEL_ACTION_CREATEREQUEST;
            createRes.TunnelHeader.Flags = 0;
            createRes.TunnelHeader.PayloadLength = 24;
            createRes.TunnelHeader.HeaderLength = 4;
            createRes.TunnelHeader.SubHeaders = null;
            createRes.RequestID = reqestId;
            createRes.Reserved = 0;
            if (16 == securityCookie.Length)
            {
                createRes.SecurityCookie = new byte[securityCookie.Length];
                Array.Copy(securityCookie, createRes.SecurityCookie, securityCookie.Length);
                return createRes;
            }
            else
            {
                return null;
            }
        }
                
        /// <summary>
        /// Expect a RDP_TUNNEL_CREATERESPONSE structure
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RDP_TUNNEL_CREATERESPONSE ExpectTunnelCreateResponse(TimeSpan timeout)
        {
            if (Connected)
            {
                return null;
            }

            DateTime endTime = DateTime.Now + timeout;
            RDP_TUNNEL_CREATERESPONSE createResp = null;
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
                                if (receiveBuffer[i] is RDP_TUNNEL_CREATERESPONSE)
                                {
                                    createResp = receiveBuffer[i] as RDP_TUNNEL_CREATERESPONSE;
                                    receiveBuffer.RemoveAt(i);
                                    return createResp;
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
            if (pdu == null || pdu.TunnelHeader == null)
            {
                return;
            }

            if (detectBandwidth)
            {
                // if detect device, count bytes
                bandwidthMeasurePayloadByteCount += ((uint)pdu.TunnelHeader.PayloadLength + pdu.TunnelHeader.HeaderLength);
            }

            RDP_TUNNEL_SUBHEADER[] RDPTunnelSubHeaders = pdu.TunnelHeader.SubHeaders;
            if (RDPTunnelSubHeaders != null)
            {
                foreach (RDP_TUNNEL_SUBHEADER RDPTunnelSubHeader in RDPTunnelSubHeaders)
                {
                    if (RDPTunnelSubHeader.SubHeaderType == RDP_TUNNEL_SUBHEADER_TYPE_Values.TYPE_ID_AUTODETECT_REQUEST)
                    {
                        uint sequenceNumber = (uint)(RDPTunnelSubHeader.SubHeaderData[1] << 8) + RDPTunnelSubHeader.SubHeaderData[0];
                        uint responseType = (uint)(RDPTunnelSubHeader.SubHeaderData[3] << 8) + RDPTunnelSubHeader.SubHeaderData[2];
                        if (responseType == (uint)AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP ||
                            responseType == (uint)AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP)
                        {
                            // Start bandwidth detect
                            bandwidthMeasureStartTime = DateTime.Now;
                            bandwidthMeasurePayloadByteCount = 0;
                            detectBandwidth = true;
                        }
                        else if (responseType == (uint)AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP ||
                            responseType == (uint)AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP)
                        {
                            detectBandwidth = false;
                            TimeSpan duration =  DateTime.Now - bandwidthMeasureStartTime;
                            this.timeDelta = (uint)duration.TotalMilliseconds;
                            bwStopSequenceNumber = (ushort)sequenceNumber;
                            // Send RDP_BW_RESULTS
                            if (AutoHandle)
                            {
                                byte[] subHeader = GenerateSubHander_BWResult();
                                List<byte[]> subHeaders = new List<byte[]>();
                                subHeaders.Add(subHeader);
                                RDP_TUNNEL_DATA tunnelData = CreateTunnelDataPdu(null, subHeaders, RDP_TUNNEL_SUBHEADER_TYPE_Values.TYPE_ID_AUTODETECT_RESPONSE);
                                this.SendRdpemtPacket(tunnelData);
                            }                           
                            
                        }
                        else if (responseType == (uint)AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT ||
                            responseType == (uint)AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT)
                        {
                            // Update bandwidth
                            RDP_NETCHAR_RESULT ncRes = RdpemtUtility.ParseRdpNetCharResult(RDPTunnelSubHeader.SubHeaderData);
                            this.bandwidth = ncRes.bandwidth;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Generate sub header Data of BW result
        /// </summary>
        /// <param name="sequenceNumber"></param>
        /// <returns></returns>
        public byte[] GenerateSubHander_BWResult()
        {
            RDP_BW_RESULTS bwresult = RdpbcgrUtility.GenerateBandwidthMeasureResults(AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT, bwStopSequenceNumber, timeDelta, bandwidthMeasurePayloadByteCount);
            byte[] subHeaderbytes = RdpbcgrClient.EncodeNetworkDetectionResponse(bwresult, true);
            return subHeaderbytes;
        }

        #endregion Public Methods
    }
}
