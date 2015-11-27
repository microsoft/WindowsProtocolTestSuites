// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib.Types;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    public class KpasswdClient : KerberosRole
    {
        #region members

        private TransportStack kdcTransport;
        private Type expectedPduType;

        protected string kdcAddress;
        protected int kdcPort;
        protected TransportType transportType;

        #endregion members

        /// <summary>
        /// Contains all the important state variables in the context.
        /// </summary>
        public override KerberosContext Context
        {
            get;
            set;
        }

        /// <summary>
        /// A boolean value indicating whether KDC Proxy is used.
        /// </summary>
        public bool UseProxy
        {
            get;
            set;
        }

        /// <summary>
        /// KDC Proxy client used to send and receive proxy messages.
        /// It is used when UseProxy is TRUE.
        /// </summary>
        public KKDCPClient ProxyClient
        {
            get;
            set;
        }

        /// <summary>
        /// Create a Kpassword client instance
        /// </summary>
        /// <param name="kdcAddress">The IP address of the KDC.</param>
        /// <param name="kdcPort">The port of the KDC.</param>
        /// <param name="transportType">Whether the transport is TCP or UDP transport.</param>
        public KpasswdClient(string kdcAddress, int kdcPort, TransportType transportType)
        {
            this.Context = new KerberosContext();
            this.kdcAddress = kdcAddress;
            this.kdcPort = kdcPort;
            this.transportType = transportType;
        }

        #region Connection with KDC
        /// <summary>
        /// Set up the TCP/UDP transport connection with KDC.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the connection type is neither TCP nor UDP</exception>
        public virtual void Connect()
        {
            SocketTransportConfig transportConfig = new SocketTransportConfig();
            transportConfig.Role = Role.Client;
            transportConfig.MaxConnections = 1;
            transportConfig.BufferSize = KerberosConstValue.TRANSPORT_BUFFER_SIZE;
            transportConfig.RemoteIpPort = kdcPort;
            transportConfig.RemoteIpAddress = IPAddress.Parse(kdcAddress);

            // For UDP bind
            if (transportConfig.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                transportConfig.LocalIpAddress = IPAddress.Any;
            }
            else if (transportConfig.RemoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                transportConfig.LocalIpAddress = IPAddress.IPv6Any;
            }

            if (transportType == TransportType.TCP)
            {
                transportConfig.Type = StackTransportType.Tcp;
            }
            else if (transportType == TransportType.UDP)
            {
                transportConfig.Type = StackTransportType.Udp;
            }
            else
            {
                throw new ArgumentException("ConnectionType can only be TCP or UDP.");
            }

            kdcTransport = new TransportStack(transportConfig, DecodePacketCallback);
            if (transportType == TransportType.TCP)
            {
                kdcTransport.Connect();
            }
            else
            {
                kdcTransport.Start();
            }
        }

        public virtual void DisConnect()
        {
            if (this.transportType == TransportType.TCP)
                kdcTransport.Disconnect();
        }
        #endregion

        #region Transport methods
        /// <summary>
        /// Encode a PDU to a binary stream. Then send the stream.
        /// </summary>
        /// <param name="pdu">A specified type of a PDU. This argument cannot be null.
        /// If it is null, ArgumentNullException will be thrown.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        public void SendPdu(KerberosPdu pdu)
        {
            if (pdu == null)
            {
                throw new ArgumentNullException("pdu");
            }

            if (UseProxy)
            {
                Debug.Assert(ProxyClient != null, "Proxy Client should be set when using proxy.");

                //temporarily change the tranpsort type to TCP, since all proxy messages are TCP format.
                var oriTransportType = Context.TransportType;
                Context.TransportType = TransportType.TCP;
                KDCProxyMessage message = ProxyClient.MakeProxyMessage(pdu);
                //send proxy message
                ProxyClient.SendProxyRequest(message);
                //restore the original transport type
                Context.TransportType = oriTransportType;
            }
            else
            {
                this.Connect();
                kdcTransport.SendPacket(pdu);
            }
        }

        /// <summary>
        /// Expect to receive a PDU of any type from the remote host.
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="System.TimeoutException">Thrown when the timeout parameter is negative.</exception>
        public KerberosPdu ExpectPdu(TimeSpan timeout, Type pduType = null)
        {
            this.expectedPduType = pduType;
            KerberosPdu pdu = null;
            if (UseProxy)
            {
                Debug.Assert(ProxyClient != null, "Proxy Client should be set when using proxy.");
                int consumedLength = 0;
                int expectedLength = 0;
                if (ProxyClient.Error == KKDCPError.STATUS_SUCCESS)
                {
                    KDCProxyMessage message = ProxyClient.GetProxyResponse();
                    //temporarily change the tranpsort type to TCP, since all proxy messages are TCP format.
                    var oriTransportType = Context.TransportType;
                    Context.TransportType = TransportType.TCP;
                    //get proxy message
                    pdu = getExpectedPduFromBytes(message.Message.kerb_message.ByteArrayValue, out consumedLength, out expectedLength);
                    //restore the original transport type
                    Context.TransportType = oriTransportType;
                }
            }
            else
            {
                if (timeout.TotalMilliseconds < 0)
                {
                    throw new TimeoutException(KerberosConstValue.TIMEOUT_EXCEPTION);
                }

                this.expectedPduType = pduType;

                TransportEvent eventPacket = kdcTransport.ExpectTransportEvent(timeout);
                pdu = (KerberosPdu)eventPacket.EventObject;
                this.DisConnect();
            }
            return pdu;
        }
        #endregion

        /// <summary>
        /// Decode Kpassword PDUs from received message bytes
        /// </summary>
        /// <param name="endPoint">An endpoint from which the message bytes are received</param>
        /// <param name="receivedBytes">The received bytes to be decoded</param>
        /// <param name="consumedLength">Length of message bytes consumed by decoder</param>
        /// <param name="expectedLength">Length of message bytes the decoder expects to receive</param>
        /// <returns>The decoded Kpassword PDUs</returns>
        /// <exception cref="System.FormatException">thrown when a kpassword message type is unsupported</exception>
        public override KerberosPdu[] DecodePacketCallback(object endPoint,
                                                byte[] receivedBytes,
                                                out int consumedLength,
                                                out int expectedLength)
        {
            KerberosPdu pdu = getExpectedPduFromBytes(receivedBytes, out consumedLength, out expectedLength);
            // insufficient data, need to receive more
            if (pdu == null)
            {
                return null;
            }
            return new KerberosPdu[] { pdu };
        }

                //Get the expected Kerberos PDU from byte array
        private KerberosPdu getExpectedPduFromBytes(
            byte[] receivedBytes,
            out int consumedLength,
            out int expectedLength)
        {
            // initialize lengths
            consumedLength = 0;
            expectedLength = 0;

            if (null == receivedBytes || 0 == receivedBytes.Length)
            {
                return null;
            }

            // TCP has a 4 bytes length header, while UDP has not
            byte[] pduBytes = receivedBytes;

            if ((this.Context.TransportType == TransportType.TCP))
            {
                // insufficient data, needs to receive more
                if (receivedBytes.Length < sizeof(int))
                {
                    return null;
                }

                // get pdu data length
                byte[] lengthBytes = ArrayUtility.SubArray(receivedBytes, 0, sizeof(int));
                Array.Reverse(lengthBytes);
                int pduLength = BitConverter.ToInt32(lengthBytes, 0);

                // insufficient data, needs to receive more
                expectedLength = sizeof(int) + pduLength;
                if (receivedBytes.Length < expectedLength)
                {
                    return null;
                }

                // remove length header from pdu bytes
                pduBytes = ArrayUtility.SubArray<byte>(receivedBytes, sizeof(int), pduLength);
            }
            else
            {
                // UDP has no length header
                expectedLength = pduBytes.Length;
            }

            consumedLength = expectedLength;
            KerberosPdu pdu = null;

            //Get AP data in message to judge the kpassword type
            byte[] apLengthBytes = ArrayUtility.SubArray<byte>(pduBytes, 2 * sizeof(ushort), sizeof(ushort));
            Array.Reverse(apLengthBytes);
            ushort apLength = BitConverter.ToUInt16(apLengthBytes, 0);

            //If the length is zero, then the last field contains a KRB-ERROR message instead of a KRB-PRIV message.
            if (apLength == 0)
            {
                pdu = new KerberosKrbError();
                pdu.FromBytes(ArrayUtility.SubArray<byte>(pduBytes, 3 * sizeof(ushort)));
                return pdu;
            }

            // get message type
            // (the lower 5 bits indicates its message type)
            byte[] apBytes = ArrayUtility.SubArray<byte>(pduBytes, 3 * sizeof(ushort), apLength);
            MsgType kpassMsgType = (MsgType)(apBytes[0] & 0x1f);
            if (kpassMsgType == MsgType.KRB_AP_REQ)
            {
                pdu = new KpasswordRequest();
            }
            else
            {
                pdu = new KpasswordResponse();
                pdu.FromBytes(pduBytes);
            }

            return pdu;
        }

    }
}
