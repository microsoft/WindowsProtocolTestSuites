// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    public delegate void PDUReceived(RdpemtBasePDU pdu);

    public delegate void ReceiveData(byte[] data);

    public abstract class RdpemtTransport : IDisposable
    {
        #region Private/Protected Methods


        private bool autoHandle;

        private RdpemtDecoder decoder;

        /// <summary>
        /// Secure channel which is encrypted by using TLS/DTLS 
        /// </summary>
        protected ISecureChannel secureChannel;

        protected bool connected;

        /// <summary>
        /// Buffer for received data
        /// </summary>
        protected List<BasePDU> receiveBuffer;

        /// <summary>
        /// Sleep interval when waiting pdu
        /// </summary>
        protected const int waitInterval = 50;

        protected const uint HrResponse_S_OK = 0;

        protected uint bandwidth = 0;

        #endregion Private/Protected Methods

        #region Properties

        /// <summary>
        /// Is the connection is an auto handle connection
        /// </summary>
        public bool AutoHandle
        {
            get
            {
                return autoHandle;
            }
        }

        /// <summary>
        /// Whether the connection established
        /// </summary>
        public bool Connected
        {
            get
            {
                return connected;
            }
        }

        /// <summary>
        /// Bandwidth of this connection, will be set a value after network auto-detection
        /// </summary>
        public uint Bandwidth
        {
            get
            {
                return bandwidth;
            }
        }

        /// <summary>
        /// lower secure channel
        /// </summary>
        public ISecureChannel SecureChannel
        {
            get
            {
                return secureChannel;
            }
        }
        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="secChannel"></param>
        /// <param name="autoHandle"></param>
        public RdpemtTransport(ISecureChannel secChannel, bool autoHandle = true)
        {
            this.autoHandle = autoHandle;
            if (secChannel != null)
            {
                this.secureChannel = secChannel;
                this.secureChannel.Received += ReceiveBytes;
            }

            receiveBuffer = new List<BasePDU>();
            decoder = new RdpemtDecoder();
        }

        public RdpemtTransport(bool autoHandle = true)
        {
            this.autoHandle = autoHandle;
            receiveBuffer = new List<BasePDU>();
            decoder = new RdpemtDecoder();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            RDP_TUNNEL_DATA tunnelData = this.CreateTunnelDataPdu(data, null, RDP_TUNNEL_SUBHEADER_TYPE_Values.TYPE_ID_AUTODETECT_REQUEST);
            this.SendRdpemtPacket(tunnelData);
        }

        public event PDUReceived PDUReceived;

        public event ReceiveData Received;


        /// <summary>
        /// Process bytes received from lower layer
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveBytes(byte[] data)
        {
            RdpemtBasePDU[] pdus = decoder.Decode(data);
            if (pdus != null && pdus.Length > 0)
            {
                if (!AutoHandle)
                {
                    lock (receiveBuffer)
                    {
                        receiveBuffer.AddRange(pdus);
                    }
                }
                else
                {
                    foreach (RdpemtBasePDU pdu in pdus)
                    {
                        ProcessSubHeaders(pdu);

                        if (pdu is RDP_TUNNEL_DATA)
                        {
                            ProcessTunnelData(pdu as RDP_TUNNEL_DATA);
                        }
                        else
                        {
                            receiveBuffer.Add(pdu);
                        }
                    }
                }

                foreach (var pdu in pdus)
                {
                    if (PDUReceived != null)
                    {
                        PDUReceived(pdu);
                    }
                }
            }
        }

        /// <summary>
        /// Send Tunnel Data
        /// </summary>
        /// <param name="tunnelData">Tunnel Data to sent</param>
        public void SendRdpemtPacket(RdpemtBasePDU rdpemtPacket)
        {
            byte[] encodedData = PduMarshaler.Marshal(rdpemtPacket);
            this.secureChannel.Send(encodedData);
        }

        /// <summary>
        /// Expect a Tunnel Data from receive buffer
        /// This method only can be used when autohandle is false
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RDP_TUNNEL_DATA ExpectTunnelData(TimeSpan timeout)
        {
            if (!Connected || AutoHandle)
            {
                return null;
            }

            DateTime endTime = DateTime.Now + timeout;
            RDP_TUNNEL_DATA tunnalData = null;
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
                                if (receiveBuffer[i] is RDP_TUNNEL_DATA)
                                {
                                    tunnalData = receiveBuffer[i] as RDP_TUNNEL_DATA;
                                    receiveBuffer.RemoveAt(i);
                                    return tunnalData;
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
        /// Create Tunnel Data PDU
        /// </summary>
        /// <param name="Payload"></param>
        /// <param name="SubheaderList"></param>
        /// <returns></returns>
        public RDP_TUNNEL_DATA CreateTunnelDataPdu(byte[] Payload, List<byte[]> SubheaderList = null,
            RDP_TUNNEL_SUBHEADER_TYPE_Values subHeaderType = RDP_TUNNEL_SUBHEADER_TYPE_Values.TYPE_ID_AUTODETECT_REQUEST)
        {
            RDP_TUNNEL_DATA tunnelData = new RDP_TUNNEL_DATA();
            tunnelData.TunnelHeader = new RDP_TUNNEL_HEADER();
            tunnelData.TunnelHeader.Action = RDP_TUNNEL_ACTION_Values.RDPTUNNEL_ACTION_DATA;
            tunnelData.TunnelHeader.Flags = 0;
            tunnelData.TunnelHeader.HeaderLength = 4;

            if (SubheaderList != null && SubheaderList.Count > 0)
            {
                tunnelData.TunnelHeader.SubHeaders = new RDP_TUNNEL_SUBHEADER[SubheaderList.Count];
                for (int i = 0; i < SubheaderList.Count; i++)
                {
                    tunnelData.TunnelHeader.SubHeaders[i] = new RDP_TUNNEL_SUBHEADER();
                    tunnelData.TunnelHeader.SubHeaders[i].SubHeaderLength = (byte)(2 + SubheaderList[i].Length);
                    tunnelData.TunnelHeader.SubHeaders[i].SubHeaderType = subHeaderType;
                    tunnelData.TunnelHeader.SubHeaders[i].SubHeaderData = new byte[SubheaderList[i].Length];
                    Array.Copy(SubheaderList[i], tunnelData.TunnelHeader.SubHeaders[i].SubHeaderData, SubheaderList[i].Length);
                    tunnelData.TunnelHeader.HeaderLength += tunnelData.TunnelHeader.SubHeaders[i].SubHeaderLength;
                }
            }
            else
            {
                tunnelData.TunnelHeader.SubHeaders = null;
            }

            if (Payload != null)
            {
                tunnelData.TunnelHeader.PayloadLength = (ushort)Payload.Length;
                tunnelData.HigherLayerData = new byte[Payload.Length];
                Array.Copy(Payload, tunnelData.HigherLayerData, Payload.Length);

            }
            else
            {
                tunnelData.TunnelHeader.PayloadLength = 0;
                tunnelData.HigherLayerData = null;
            }
            return tunnelData;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.secureChannel != null)
            {
                this.secureChannel.Dispose();
            }
        }

        #endregion Public Methods

        #region abstract methods

        /// <summary>
        /// abstract method to deal with auto detect
        /// The process is quick different between server and client 
        /// </summary>
        /// <param name="pdu"></param>
        public abstract void ProcessSubHeaders(RdpemtBasePDU pdu);


        #endregion abstract methods

        #region Private Methods

        /// <summary>
        /// Process Tunnel Data pdu
        /// </summary>
        /// <param name="pdu"></param>
        private void ProcessTunnelData(RDP_TUNNEL_DATA pdu)
        {
            if (pdu.HigherLayerData != null && pdu.HigherLayerData.Length > 0)
            {
                if (Received != null)
                {
                    Received(pdu.HigherLayerData);
                }
            }
        }

        #endregion Private Methods
    }
}
