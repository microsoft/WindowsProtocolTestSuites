// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools.ExtendedLogging;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;
using Microsoft.Protocols.TestTools.StackSdk.Security;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// RDPBCGR server, receives client PDUs and sends server PDUs. 
    /// Meanwhile acts as a base protocol for upper layer protocol, such as MS-RA, MS-RDPEGDI, and etc.
    /// </summary>
    public class RdpbcgrServer : IDisposable
    {
        #region Field members
        //A manager context to manage all RDPBCGR session context
        private RdpbcgrServerContext serverContext;

        private int serverPort;

        // The selected security protocol
        private EncryptedProtocol encryptedProtocol;

        // This member indicates UpdateSessionKey has completed
        private ManualResetEvent updateSessionKeyEvent;

#if UT
        private TransportStackServerMock transportStack;
#else
        // When normal security
        private TransportStack transportStack;
#endif

        // When enhanced security, use new defined transport stack
        private RdpbcgrServerTransportStack directedTransportStack;

        //private string certName;
        private X509Certificate2 cert;
        private bool disposed;

        //Store the unconsumed disconnect TransportEvent which received from high layer transport.
        private List<TransportEvent> disconnectTransportEventCache;

        private TS_SECURITY_HEADER_flags_Values defaultSecurityHeaderFlag = TS_SECURITY_HEADER_flags_Values.None;

        private bool isClientToServerEncrypted;

        private List<string> warnings;

        /// <summary>
        /// This event is used to sync main thread and Receive thread.
        /// It is used to block receive thread when the main thread want to make TLS handshake with RDP client.
        /// If not block the receive thread, the receive thread will read bytes from network stream, which should be read by main thread for TLS handshake  
        /// </summary>
        public AutoResetEvent ReceiveThreadControlEvent = new AutoResetEvent(false);

        #endregion member


        #region Constructor
        /// <summary>
        /// Perform all operations of the RDPBCGR protocol server side.
        /// </summary>
        /// <param name="port">Specify the port of the server.</param>
        /// <param name="encryptedProtocol">Specify whether Standard RDP Security or Enhanced RDP Security will be used. 
        /// If select Enhanced RDP Security, specify Negotiation-Based Approach or Direct Approach will be 
        /// used.</param>
        public RdpbcgrServer(
            int port,
            EncryptedProtocol encryptedProtocol)
        {
            this.serverPort = port;
            this.encryptedProtocol = encryptedProtocol;
            this.serverContext = new RdpbcgrServerContext();
            updateSessionKeyEvent = new ManualResetEvent(false);
            disconnectTransportEventCache = new List<TransportEvent>();
            warnings = new List<string>();
        }


        /// <summary>
        /// Perform all operations of the RDPBCGR protocol server side.
        /// </summary>
        /// <param name="port">Specify the port of the server.</param>
        /// <param name="encryptedProtocol">Specify whether Standard RDP Security or Enhanced RDP Security will be used. 
        /// <param name="cert">Certificate provided by user.</param>
        /// If select Enhanced RDP Security, specify Negotiation-Based Approach or Direct Approach will be 
        /// used.</param>
        public RdpbcgrServer(
            int port,
            EncryptedProtocol encryptedProtocol,
            X509Certificate2 cert,
            bool IsClientToServerEncrypted = true)
        {
            this.serverPort = port;
            this.encryptedProtocol = encryptedProtocol;
            this.serverContext = new RdpbcgrServerContext();
            this.cert = cert;
            this.isClientToServerEncrypted = IsClientToServerEncrypted;
            updateSessionKeyEvent = new ManualResetEvent(false);
            disconnectTransportEventCache = new List<TransportEvent>();
        }
        #endregion constructor


        #region Properties
        /// <summary>
        /// Used for enhanced security.
        /// </summary>
        internal RdpbcgrServerTransportStack DirectedTransportStack
        {
            get
            {
                return directedTransportStack;
            }

            set
            {
                directedTransportStack = value;
            }
        }

        public X509Certificate2 AuthCertificate
        {
            get
            {
                return cert;
            }
        }
        /// <summary>
        /// The server context manager contains all needed server context information.
        /// </summary>
        public RdpbcgrServerContext ServerContext
        {
            get
            {
                return serverContext;
            }
        }


        /// <summary>
        /// Which security mechanism is used
        /// </summary> 
        public EncryptedProtocol EncryptedProtocol
        {
            get
            {
                return encryptedProtocol;
            }

            set
            {
                this.encryptedProtocol = value;
            }
        }

        /// <summary>
        /// Whether the client-to-server message is encrypted
        /// </summary> 
        public bool IsClientToServerEncrypted
        {
            get
            {
                return isClientToServerEncrypted;
            }

            set
            {
                this.isClientToServerEncrypted = value;
            }
        }


        /// <summary> 
        /// Server tcp port
        /// </summary>
        public int ServerPort
        {
            get
            {
                return serverPort;
            }
        }
        #endregion


        #region User interface
        #region Start and Stop
        /// <summary>
        /// Start the RDPBCGR server, create tcp transport according to the security used, 
        /// the address and local port of server are assigned with default value.
        /// </summary>
        public virtual void Start(IPAddress address)
        {
            CreateTransportStack(address);

            if (this.encryptedProtocol == EncryptedProtocol.DirectCredSsp ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationTls ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                this.directedTransportStack.Start();
            }

            else
            {
                this.transportStack.Start();
            }
        }
        #endregion


        #region Connect
        /// <summary>
        /// Expect a client connection event.
        /// </summary>
        /// <param name="timeout">Time for waiting response.</param>
        /// <returns>A new session context.</returns>
        /// <exception cref="InvalidOperationException"> 
        /// An error occurred when an unwanted object is received in transport.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public virtual RdpbcgrServerSessionContext ExpectConnect(TimeSpan timeout)
        {
            DateTime curTime = DateTime.Now;
            DateTime expiredTime = curTime + timeout;
            TimeSpan leftTime = timeout;

            if (this.transportStack == null && this.directedTransportStack == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            // There may be already other endpoints which connected to the server, 
            // so uses loop to exclude the unexpected transport events like exception type.
            while (curTime < expiredTime)
            {
                leftTime = expiredTime - curTime;
                TransportEvent transportEvent = ExpectTransportEvent(leftTime);
                curTime = DateTime.Now;

                if (this.encryptedProtocol == Rdpbcgr.EncryptedProtocol.Rdp)
                {
                    RdpbcgrServerSessionContext session = new RdpbcgrServerSessionContext();
                    session.Identity = transportEvent.EndPoint;
                    session.LocalIdentity = transportEvent.LocalEndPoint;
                    session.Server = this;
                    session.IsClientToServerEncrypted = this.IsClientToServerEncrypted;
                    this.serverContext.AddSession(session);
                    return session;
                }
                else if (transportEvent.EventType == EventType.Connected)
                {
                    return this.serverContext.LookupSession(transportEvent.EndPoint);
                }
                else
                {
                    Thread.Sleep(100);//Wait for next transport event.
                }
            }

            //if not received, throw timeout exception
            throw new TimeoutException(ConstValue.TIMEOUT_EXCEPTION);
        }


        /// <summary>
        /// Expect to disconnect from the specific client.
        /// </summary>
        /// <param name="sessionContext">The specific client sessionContext expected to disconnect with.</param>
        /// <param name="timeout">Time for expecting a disconnect.</param>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public virtual void ExpectDisconnect(
            RdpbcgrServerSessionContext sessionContext,
            TimeSpan timeout)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (this.transportStack == null && this.directedTransportStack == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            lock (disconnectTransportEventCache)
            {
                //Serach from the unconsumed list firstly.
                if (disconnectTransportEventCache.Count > 0)
                {
                    foreach (TransportEvent tsEvent in disconnectTransportEventCache)
                    {
                        if (tsEvent.EventType == EventType.Disconnected)
                        {
                            RdpbcgrServerSessionContext session = this.serverContext.LookupSession(tsEvent.EndPoint);

                            if (session == sessionContext)
                            {
                                this.serverContext.RemoveSession(session);
                                this.disconnectTransportEventCache.Remove(tsEvent);
                                return;
                            }
                        }
                    }
                }


                TransportEvent transportEvent = ExpectTransportEvent(timeout);

                if (transportEvent.EventType == EventType.Disconnected)
                {
                    RdpbcgrServerSessionContext session = this.serverContext.LookupSession(transportEvent.EndPoint);

                    if (session != sessionContext)
                    {
                        throw new InvalidOperationException("Unwanted disconnection received in transport.");
                    }
                    else
                    {
                        this.serverContext.RemoveSession(session);
                    }
                }
                else if (transportEvent.EventType == EventType.Exception)
                {
                    throw transportEvent.EventObject as Exception;
                }
                else
                {
                    throw new InvalidOperationException("Unknown object received in transport.");
                }
            }
        }


        /// <summary>
        /// Expect to disconnect from the specific client.
        /// </summary>
        /// <param name="sessionContext">The client sessionContext which sends disconnect.</param>
        /// <param name="timeout">Time for expecting a disconnect.</param>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public virtual void ExpectDisconnect(
            TimeSpan timeout,
            out RdpbcgrServerSessionContext sessionContext)
        {
            if (this.transportStack == null && this.directedTransportStack == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            TransportEvent transportEvent = ExpectTransportEvent(timeout);

            if (transportEvent.EventType == EventType.Disconnected)
            {
                sessionContext = this.serverContext.LookupSession(transportEvent.EndPoint);
                this.serverContext.RemoveSession(sessionContext);
            }
            else if (transportEvent.EventType == EventType.Exception)
            {
                throw transportEvent.EventObject as Exception;
            }
            else
            {
                throw new InvalidOperationException("Unknown object received in transport.");
            }
        }


        /// <summary>
        /// Disconnect the specific client connection.
        /// </summary>
        /// <param name="sessionContext">The session context server uses to communicate with client.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when The session has not been established.</exception>
        public virtual void Disconnect(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("The session to disconnect is NULL!");
            }

            if (this.serverContext.LookupSession(sessionContext.Identity) == null)
            {
                throw new InvalidOperationException("The session is not started!");
            }

            if (this.EncryptedProtocol == EncryptedProtocol.DirectCredSsp ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationTls ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                this.directedTransportStack.Disconnect(sessionContext.Identity);
                this.directedTransportStack.CloseStream(sessionContext.Identity);
            }
            else
            {
                this.transportStack.Disconnect(sessionContext.Identity);
            }

            this.serverContext.RemoveSession(sessionContext);
        }


        /// <summary>
        /// Disconnect all the client connection.
        /// </summary>
        public virtual void Disconnect()
        {
            while (this.serverContext.SessionContexts.Count > 0)
            {
                this.Disconnect(serverContext.SessionContexts[0]);
            }

            if (this.transportStack != null)
            {
                this.transportStack.Disconnect();
            }

            if (this.directedTransportStack != null)
            {
                this.directedTransportStack.Disconnect();
            }
        }
        #endregion


        #region Send data
        /// <summary>
        /// Encode a non-virtual channel PDU to a binary stream. Then send the stream.
        /// The pdu could be got by calling method Create***Pdu.
        /// </summary>
        /// <param name="sessionContext">The specific client connection server uses to communicate with.</param>
        /// <param name="pdu">A specified type of a PDU. This argument can not be null.
        /// If it is null, ArgumentNullException will be thrown.</param>
        /// <exception cref="ArgumentNullException">An error occurred when there is no argument.</exception>
        /// <exception cref="FormatException">An error occurred when there is an encoding error.</exception>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        public void SendPdu(
            RdpbcgrServerSessionContext sessionContext,
            RdpbcgrServerPdu pdu)
        {
            if (pdu == null)
            {
                throw new ArgumentNullException("pdu");
            }

            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (this.transportStack == null && this.directedTransportStack == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            sessionContext.UpdateContext(pdu);

            if (this.encryptedProtocol == EncryptedProtocol.DirectCredSsp)
            {
                this.directedTransportStack.SendPacket(sessionContext.Identity, pdu);
            }
            else if (this.encryptedProtocol == EncryptedProtocol.NegotiationTls ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                this.directedTransportStack.SendPacket(sessionContext.Identity, pdu);
                if (pdu.GetType() == typeof(Server_X_224_Connection_Confirm_Pdu))
                {
                    // If send a X224 Connection COnfirm Pdu, then should make TLS handshake
                    UpdateTransport();
                }
            }
            else
            {
                this.transportStack.SendPacket(sessionContext.Identity, pdu);
            }


            if (pdu.GetType() == typeof(Server_X_224_Connection_Confirm_Pdu) ||
                pdu.GetType() == typeof(Server_X_224_Negotiate_Failure_Pdu))
            {
                // Receive thread will be blocked by ReceiveThreadControlEvent after receiving Client X224 Connection Request.
                // If send a X224 Connection Confirm Pdu or X224 Negotiate Failure PDU, should signal the event to resume receive thread.
                ReceiveThreadControlEvent.Set();
            }

            CheckEncryptionCount(sessionContext);
        }


        /// <summary>
        /// Encode a virtual channel PDU to a binary stream. Then send the stream.
        /// The pdu could be got by calling method Create***VirtualChannelPDU.
        /// </summary>
        /// <param name="sessionContext">The specific client connection server uses to communicate with.</param>
        /// <param name="channelId">The channel id to send data.</param>
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled and
        /// Virtual_Channel_RAW_Pdu will be returned. Otherwise, the PDU will be reassembled and
        /// Virtual_Channel_Complete_Pdu will be returned.</param>   
        /// <param name="virtualChannelPdu">A specified type of virtual channel PDU.</param>
        /// <exception cref="ArgumentNullException">An error occured when there is no argument.</exception>
        /// <exception cref="FormatException">An error occured when there is an encoding error.</exception>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        public void SendVirtualChannelPdu(
            RdpbcgrServerSessionContext sessionContext,
            UInt16 channelId,
            bool isRawPdu,
            RdpbcgrServerPdu virtualChannelPdu)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (virtualChannelPdu == null)
            {
                throw new ArgumentNullException("virtualChannelPdu");
            }

            if (!isRawPdu)
            {
                Virtual_Channel_Complete_Server_Pdu tmpPdu = (Virtual_Channel_Complete_Server_Pdu)virtualChannelPdu;
                for (int i = 0; i < tmpPdu.rawPdus.Count; ++i)
                {
                    SendPdu(sessionContext, virtualChannelPdu);
                }
            }
            else
            {
                SendPdu(sessionContext, virtualChannelPdu);
            }
        }
        #endregion


        #region Expect data
        /// <summary>
        /// Expect to receive a non virtual channel PDU from the remote host for a specific session.
        /// </summary>
        /// <param name="sessionContext">The specific client connection server uses to communicate with.</param>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public StackPacket ExpectPdu(
            RdpbcgrServerSessionContext sessionContext,
            TimeSpan timeout)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (timeout.TotalMilliseconds < 0)
            {
                return null;
            }

            // Get pdu from buffer of this RDP session
            StackPacket pdu = sessionContext.GetPacketFromBuffer();
            if (pdu != null)
            {
                return pdu;
            }

            TransportEvent transportEvent = null;
            DateTime endtime = DateTime.Now + timeout;
            lock (disconnectTransportEventCache)
            {
                while (DateTime.Now < endtime)
                {
                    transportEvent = ExpectTransportEvent(endtime - DateTime.Now);

                    if (transportEvent.EventType == EventType.Disconnected)
                    {

                        this.disconnectTransportEventCache.Add(transportEvent);

                    }
                    else
                    {
                        pdu = (StackPacket)transportEvent.EventObject;
                        if (transportEvent.EndPoint == sessionContext.Identity)
                        {
                            return pdu;
                        }
                        else
                        {
                            // Add the pdu into packet buffer of corresponding RDP session
                            RdpbcgrServerSessionContext context = this.serverContext.LookupSession(transportEvent.EndPoint);
                            if (context != null)
                            {
                                context.AddPacketToBuffer(pdu);
                            }
                        }
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Expect to receive a non virtual channel PDU from the remote host.
        /// Meanwhile, extract the respect sessionContext.
        /// </summary>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <param name="sessionContext">The client connection which sends PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public StackPacket ExpectPdu(
            TimeSpan timeout,
            out RdpbcgrServerSessionContext sessionContext)
        {
            if (timeout.TotalMilliseconds < 0)
            {
                sessionContext = null;
                return null;
            }

            TransportEvent transportEvent = null;

            transportEvent = ExpectTransportEvent(timeout);

            if (transportEvent.EventType == EventType.ReceivedPacket)
            {
                sessionContext = this.serverContext.LookupSession(transportEvent.EndPoint);
                if (sessionContext == null)
                {
                    throw new InvalidOperationException(
                        "The session has not been established, please call ExpectConnect first.");
                }
                return transportEvent.EventObject as StackPacket;
            }
            else if (transportEvent.EventType == EventType.Exception
                || transportEvent.EventType == EventType.Disconnected)
            {
                throw transportEvent.EventObject as Exception;
            }
            else
            {
                throw new InvalidOperationException("Unknown object received from transport.");
            }
        }

        /// <summary>
        /// Expect to receive a virtual channel PDU. Other types' PDUs will be filtered except an error occurs.
        /// If so MCS_Disconnect_Provider_Ultimatum_PDU or ErrorPdu will be returned according to the error.</summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <returns>The channel PDU.</returns>
        /// <exception>TimeoutException.</exception>
        public RdpbcgrClientPdu ExpectChannelPdu(RdpbcgrServerSessionContext sessionContext, TimeSpan timeout)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (timeout.TotalMilliseconds < 0)
            {
                return null;
            }

            // Get pdu from buffer of this RDP session
            StackPacket pdu = sessionContext.GetPacketFromBuffer(true);
            if (pdu != null)
            {
                return (RdpbcgrClientPdu)pdu;
            }

            TransportEvent transportEvent = null;
            DateTime endtime = DateTime.Now + timeout;
            lock (disconnectTransportEventCache)
            {
                while (DateTime.Now < endtime)
                {
                    transportEvent = ExpectTransportEvent(endtime - DateTime.Now);

                    if (transportEvent.EventType == EventType.Disconnected)
                    {

                        this.disconnectTransportEventCache.Add(transportEvent);

                    }
                    else
                    {
                        pdu = (StackPacket)transportEvent.EventObject;
                        if (transportEvent.EndPoint == sessionContext.Identity)
                        {
                            if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu
                                        || pdu is ErrorPdu
                                        || pdu is Virtual_Channel_RAW_Pdu)
                            {
                                return (RdpbcgrClientPdu)pdu;
                            }
                            else
                            {
                                sessionContext.AddPacketToBuffer(pdu);
                            }
                        }
                        else
                        {
                            // Add the pdu into packet buffer of corresponding RDP session
                            RdpbcgrServerSessionContext context = this.serverContext.LookupSession(transportEvent.EndPoint);
                            if (context != null)
                            {
                                context.AddPacketToBuffer(pdu);
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Expect to receive a virtual channel PDU. This method is especially used for expecting
        /// Virtual_Channel_RAW_PDU or Virtual_Channel_Complete_Pdu.
        /// </summary>
        /// <param name="sessionContext">The specific client connection server uses to communicate with.</param>
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled and
        /// Virtual_Channel_RAW_Pdu will be returned. Otherwise, the PDU will be reassembled and the first 
        /// reassembled from some channel Virtual_Channel_Complete_Pdu will be returned.</param>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <returns>The first expected Virtual Channel PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public RdpbcgrClientPdu ExpectVirtualChannelPdu(
            RdpbcgrServerSessionContext sessionContext,
            bool isRawPdu,
            TimeSpan timeout)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            RdpbcgrClientPdu pdu = null;

            // reassemble the virtual channel data
            if (!isRawPdu)
            {
                TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;
                Virtual_Channel_Complete_Pdu completePdu = null;
                while (completePdu == null)
                {
                    pdu = ExpectChannelPdu(sessionContext, totalTime - DateTime.Now.TimeOfDay);

                    if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                    {
                        // some error occurs
                        return pdu;
                    }

                    completePdu = sessionContext.ReassembleChunkData((Virtual_Channel_RAW_Pdu)pdu);
                }

                pdu = completePdu;
            }
            // raw PDU, so return it directly
            else
            {
                pdu = ExpectChannelPdu(sessionContext, timeout);
            }

            return pdu;
        }


        /// <summary>
        /// Expect to receive a virtual channel PDU. This method is especially used for 
        /// expecting Virtual_Channel_RAW_PDU or Virtual_Channel_Complete_Pdu.Meanwhile, extract the respect sessionContext.
        /// </summary>
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled,
        /// Virtual_Channel_RAW_Pdu will be returned. Otherwise, the PDU will be reassembled and the first 
        /// reassembled from some channel Virtual_Channel_Complete_Pdu will be returned.</param>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <param name="sessionContext">The client connection which sends PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public RdpbcgrClientPdu ExpectVirtualChannelPdu(
            bool isRawPdu,
            TimeSpan timeout,
            out RdpbcgrServerSessionContext sessionContext)
        {
            RdpbcgrClientPdu pdu = null;

            // reassemble the virtual channel data
            if (!isRawPdu)
            {
                TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;
                Virtual_Channel_Complete_Pdu completePdu = null;
                sessionContext = null;
                while (completePdu == null)
                {
                    pdu = ExpectChannelPdu(totalTime - DateTime.Now.TimeOfDay, out sessionContext);

                    if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                    {
                        // some error occurs
                        return pdu;
                    }

                    completePdu = sessionContext.ReassembleChunkData((Virtual_Channel_RAW_Pdu)pdu);
                }

                pdu = completePdu;
            }
            // raw PDU, so return it directly
            else
            {
                pdu = ExpectChannelPdu(timeout, out sessionContext);
            }

            return pdu;
        }


        /// <summary>
        /// Expect to receive a virtual channel PDU from a given channel. This method is especially used 
        /// for expecting Virtual_Channel_RAW_Pdu or Virtual_Channel_Complete_Pdu.
        /// </summary>
        /// <param name="sessionContext">The specific client connection server uses to communicate with.</param>
        /// <param name="channelId">Specify the channel.</param>
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled and
        /// Virtual_Channel_RAW_Pdu will be returned. Otherwise, the PDU will be reassembled and
        /// Virtual_Channel_Complete_Pdu will be returned.</param>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session or channel has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public RdpbcgrClientPdu ExpectVirtualChannelPdu(
            RdpbcgrServerSessionContext sessionContext,
            ushort channelId,
            bool isRawPdu,
            TimeSpan timeout
            )
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            RdpbcgrClientPdu pdu = null;
            Virtual_Channel_RAW_Pdu rawPdu = null;
            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;

            // reassemble the virtual channel data
            if (!isRawPdu)
            {
                Virtual_Channel_Complete_Pdu completePdu = null;
                while (completePdu == null)
                {
                    pdu = ExpectChannelPdu(sessionContext, totalTime - DateTime.Now.TimeOfDay);

                    if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                    {
                        // some error occurs
                        return pdu;
                    }

                    rawPdu = (Virtual_Channel_RAW_Pdu)pdu;

                    if (rawPdu.commonHeader.channelId != channelId)
                    {
                        // discard the pdu which is belong to other channels
                        continue;
                    }

                    completePdu = sessionContext.ReassembleChunkData(rawPdu);
                }

                pdu = completePdu;

                // ETW Provider Dump Message
                if (sessionContext.RdpEncryptionMethod != EncryptionMethods.ENCRYPTION_METHOD_NONE)
                {
                    string messageName = "RDPEDYC:ReceivedPDU";
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer1, pdu.GetType().Name, completePdu.virtualChannelData);
                }

            }
            // raw PDU, so return it directly
            else
            {
                while (true)
                {
                    pdu = ExpectChannelPdu(sessionContext, totalTime - DateTime.Now.TimeOfDay);

                    if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                    {
                        // some error occurs
                        return pdu;
                    }

                    rawPdu = (Virtual_Channel_RAW_Pdu)pdu;

                    if (rawPdu.commonHeader.channelId == channelId)
                    {
                        // return the pdu with the right channel Id
                        break;
                    }
                }

                pdu = rawPdu;

                // ETW Provider Dump Message
                if (sessionContext.ClientEncryptionMethod != EncryptionMethods.ENCRYPTION_METHOD_NONE)
                {
                    string messageName = "RDPEDYC:ReceivedPDU";
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer1, pdu.GetType().Name, rawPdu.virtualChannelData);
                }
            }



            return pdu;
        }


        /// <summary>
        /// Expect to receive a virtual channel PDU from a given channel.This method is especially used for expecting Virtual_Channel_RAW_Pdu or 
        /// Virtual_Channel_Complete_Pdu. Meanwhile, extract the respect sessionContext.
        /// </summary>
        /// <param name="channelId">Specify the given channel.</param>
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled and
        /// Virtual_Channel_RAW_Pdu will be returned. Otherwise, the PDU will be reassembled and
        /// Virtual_Channel_Complete_Pdu will be returned.</param>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <param name="sessionContext">The client connection which sends PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session or channel has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public RdpbcgrClientPdu ExpectVirtualChannelPdu(
            ushort channelId,
            bool isRawPdu,
            TimeSpan timeout,
            out RdpbcgrServerSessionContext sessionContext)
        {
            RdpbcgrClientPdu pdu = null;
            Virtual_Channel_RAW_Pdu rawPdu = null;
            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;
            sessionContext = null;

            // reassemble the virtual channel data
            if (!isRawPdu)
            {
                Virtual_Channel_Complete_Pdu completePdu = null;

                while (completePdu == null)
                {
                    pdu = ExpectChannelPdu(totalTime - DateTime.Now.TimeOfDay, out sessionContext);

                    if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                    {
                        // some error occurs
                        return pdu;
                    }

                    rawPdu = (Virtual_Channel_RAW_Pdu)pdu;

                    if (rawPdu.commonHeader.channelId != channelId)
                    {
                        // discard the pdu which is belong to other channels
                        continue;
                    }

                    completePdu = sessionContext.ReassembleChunkData(rawPdu);
                }

                pdu = completePdu;
            }
            // raw PDU, so return it directly
            else
            {
                while (true)
                {
                    pdu = ExpectChannelPdu(totalTime - DateTime.Now.TimeOfDay, out sessionContext);

                    if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                    {
                        // some error occurs
                        return pdu;
                    }

                    rawPdu = (Virtual_Channel_RAW_Pdu)pdu;

                    if (rawPdu.commonHeader.channelId == channelId)
                    {
                        // return the pdu with the right channel Id
                        break;
                    }
                }

                pdu = rawPdu;
            }

            return pdu;
        }


        /// <summary>
        /// Expect to receive a virtual channel PDU from a given channel. This method is especially used for expecting Virtual_Channel_RAW_PDU or 
        /// Virtual_Channel_Complete_PDU. Meanwhile, extract the respect channel id.
        /// </summary>
        /// <param name="sessionContext">The specific client connection server uses to communicate with.</param> 
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled and
        /// Virtual_Channel_RAW_PDU will be returned. Otherwise, the PDU will be reassembled and
        /// Virtual_Channel_Complete_PDU will be returned.</param>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <param name="channelId">The channel which sends PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public RdpbcgrClientPdu ExpectVirtualChannelPdu(
            RdpbcgrServerSessionContext sessionContext,
            bool isRawPdu,
            TimeSpan timeout,
            out ushort channelId)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            RdpbcgrClientPdu pdu = null;
            Virtual_Channel_RAW_Pdu rawPdu = null;
            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;

            // reassemble the virtual channel data
            if (!isRawPdu)
            {
                Virtual_Channel_Complete_Pdu completePdu = null;

                while (completePdu == null)
                {
                    pdu = ExpectChannelPdu(sessionContext, totalTime - DateTime.Now.TimeOfDay);

                    if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                    {
                        // some error occurs
                        channelId = 0;
                        return pdu;
                    }

                    rawPdu = (Virtual_Channel_RAW_Pdu)pdu;

                    completePdu = sessionContext.ReassembleChunkData(rawPdu);
                }

                channelId = rawPdu.commonHeader.channelId;
                pdu = completePdu;
            }
            // raw PDU, so return it directly
            else
            {
                pdu = ExpectChannelPdu(sessionContext, totalTime - DateTime.Now.TimeOfDay);

                if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                {
                    // some error occurs
                    channelId = 0;
                    return pdu;
                }

                rawPdu = (Virtual_Channel_RAW_Pdu)pdu;
                channelId = rawPdu.commonHeader.channelId;
                pdu = rawPdu;
            }

            return pdu;
        }


        /// <summary>
        /// Expect to receive a virtual channel PDU from a given channel. This method is especially used for expecting Virtual_Channel_RAW_PDU or 
        /// Virtual_Channel_Complete_PDU. Meanwhile, extract the respect channel ID and sessionContext.
        /// </summary>
        /// <param name="isRawPdu">Specify if expect a raw PDU. If true, the PDU will not be reassembled and
        /// Virtual_Channel_RAW_PDU will be returned. Otherwise, the PDU will be reassembled and
        /// Virtual_Channel_Complete_PDU will be returned.</param>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <param name="sessionContext">The client connection which sends PDU.</param>
        /// <param name="channelId">The channel which sends PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when The session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public RdpbcgrClientPdu ExpectVirtualChannelPdu(
            bool isRawPdu,
            TimeSpan timeout,
            out RdpbcgrServerSessionContext sessionContext,
            out ushort channelId)
        {
            RdpbcgrClientPdu pdu = null;
            Virtual_Channel_RAW_Pdu rawPdu = null;
            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;
            sessionContext = null;

            // reassemble the virtual channel data
            if (!isRawPdu)
            {
                Virtual_Channel_Complete_Pdu completePdu = null;

                while (completePdu == null)
                {
                    pdu = ExpectChannelPdu(totalTime - DateTime.Now.TimeOfDay, out sessionContext);

                    if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                    {
                        // some error occurs
                        channelId = 0;
                        return pdu;
                    }

                    rawPdu = (Virtual_Channel_RAW_Pdu)pdu;
                    completePdu = sessionContext.ReassembleChunkData(rawPdu);
                }

                channelId = rawPdu.commonHeader.channelId;
                pdu = completePdu;
            }
            // raw PDU, so return it directly
            else
            {
                pdu = ExpectChannelPdu(totalTime - DateTime.Now.TimeOfDay, out sessionContext);

                if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu || pdu is ErrorPdu)
                {
                    // some error occurs
                    channelId = 0;
                    return pdu;
                }

                rawPdu = (Virtual_Channel_RAW_Pdu)pdu;
                channelId = rawPdu.commonHeader.channelId;
                pdu = rawPdu;
            }

            return pdu;
        }
        #endregion
        #endregion 


        #region Create PDU
        #region Connection
        /// <summary>
        /// Create an X224 Connection Request PDU.
        /// User can set special value in the PDU other than the default after calling this method.
        /// Then call SendPdu to send the packet.
        /// </summary>
        /// <param name="sessionContext">The specific client connection server uses to communicate with.</param>
        /// <returns>X224 Connection Confirm PDU.</returns>
        /// <exception cref="InvalidOperationException">client requested encryptionProtocols conflict with 
        /// encryptionMethodsSupport and encryptionLevelSupport, 
        /// should call CreateX224NegotiateFailurePdu </exception>
        public Server_X_224_Connection_Confirm_Pdu CreateX224ConnectionConfirmPdu(
            RdpbcgrServerSessionContext sessionContext,
            selectedProtocols_Values selectedProtocols,
            RDP_NEG_RSP_flags_Values flags = RDP_NEG_RSP_flags_Values.None)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_X_224_Connection_Confirm_Pdu x224 = new Server_X_224_Connection_Confirm_Pdu(sessionContext);

            int tpktLength = Marshal.SizeOf(typeof(TpktHeader))
                           + Marshal.SizeOf(typeof(X224Crq))
                           + sizeof(byte)
                           + sizeof(byte)
                           + sizeof(ushort)
                           + sizeof(uint);
            RdpbcgrUtility.FillTpktHeader(ref x224.tpktHeader, (ushort)tpktLength);
            x224.x224Ccf.lengthIndicator = (byte)(tpktLength - Marshal.SizeOf(x224.tpktHeader)
                                         - Marshal.SizeOf(x224.x224Ccf.lengthIndicator));
            x224.x224Ccf.typeCredit = ConstValue.X224_CONNECTION_COMFIRM_TYPE;
            x224.x224Ccf.destRef = 0;
            x224.x224Ccf.srcRef = ConstValue.SOURCE_REFERENCE;
            x224.x224Ccf.classOptions = 0;

            RDP_NEG_RSP pdu = new RDP_NEG_RSP();
            pdu.type = RDP_NEG_RSP_type_Values.V1;
            pdu.flags = flags;
            pdu.length = RDP_NEG_RSP_length_Values.V1;
            pdu.selectedProtocol = selectedProtocols;

            x224.rdpNegData = pdu;

            return x224;
        }


        /// <summary>
        /// Create an X224 Negotiate Failure PDU.
        /// User can set special value in the PDU other than the default after calling this method.
        /// Then call SendPdu to send the packet.
        /// </summary>
        /// <param name="sessionContext">The specific client connection server uses to communicate with.</param>
        /// <returns>X224 Negotiate Failure PDU.</returns>
        /// <exception>client requested encryptionProtocols don't conflict with 
        /// encryptionMethodsSupport and encryptionLevelSupport, 
        /// should call CreateX224ConnectionConfirmPdu. </exception>
        public Server_X_224_Negotiate_Failure_Pdu CreateX224NegotiateFailurePdu(
            RdpbcgrServerSessionContext sessionContext,
            failureCode_Values failureReason)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_X_224_Negotiate_Failure_Pdu x224 = new Server_X_224_Negotiate_Failure_Pdu(sessionContext);
            int tpktLength = Marshal.SizeOf(typeof(TpktHeader))
                           + Marshal.SizeOf(typeof(X224Crq))
                           + sizeof(byte)
                           + sizeof(byte)
                           + sizeof(ushort)
                           + sizeof(uint);
            RdpbcgrUtility.FillTpktHeader(ref x224.tpktHeader, (ushort)tpktLength);
            x224.x224Ccf.lengthIndicator = (byte)(tpktLength - Marshal.SizeOf(x224.tpktHeader)
                                         - Marshal.SizeOf(x224.x224Ccf.lengthIndicator));
            x224.x224Ccf.typeCredit = 0xD0;
            x224.x224Ccf.destRef = 0;
            x224.x224Ccf.srcRef = ConstValue.SOURCE_REFERENCE;
            x224.x224Ccf.classOptions = 0;

            RDP_NEG_FAILURE pdu = new RDP_NEG_FAILURE();
            pdu.type = RDP_NEG_FAILURE_type_Values.V1;
            pdu.flags = RDP_NEG_FAILURE_flags_Values.V1;
            pdu.length = RDP_NEG_FAILURE_length_Values.V1;
            pdu.failureCode = failureReason;

            x224.rdpNegFailure = pdu;

            return x224;
        }


        /// <summary>
        /// Create MCS Connect Response PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// If user do not want to present some fields of TS_UD_CS_CORE according to TD, then set them null.
        /// After set this value, do remember update the length field of the header for clientCoreData.
        /// Other changes in clientNetworkData, clientClusterData or clientMonitorData need update 
        /// the corresponding header length too.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// </summary>
        /// <param name="sessionContext">Server session context</param>
        /// <param name="encryptionMethod">Cryptographic methods supported by the client and used in conjunction 
        /// with Standard RDP Security.</param>
        /// <param name="encryptionLevel">Encryption Level</param>
        /// <param name="serverCertificate">Certificate of server</param>
        /// <param name="serverCerLen">Length of server certificate</param>
        /// <param name="multiTransportTypeFlags">Flags of Multitransport Channel Data</param>
        /// <param name="hasEarlyCapabilityFlags">Indicates the existing of the earlyCapabilityFlags</param>
        /// <param name="earlyCapabilityFlagsValue">The value of earlyCapabilityFlags</param>
        /// <param name="mcsChannelId_Net">MCSChannelId value for Server Network Data</param>
        /// <param name="mcsChannelId_MSGChannel">MCSChannelId value for Server Message Channel Data</param>
        /// <returns>MCS Connect Initial PDU.</returns>
        public Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response
            CreateMCSConnectResponsePduWithGCCConferenceCreateResponsePdu(
            RdpbcgrServerSessionContext sessionContext,
            EncryptionMethods encryptionMethods,
            EncryptionLevel encryptionLevel,
            SERVER_CERTIFICATE serverCertificate,
            int serverCerLen,
            MULTITRANSPORT_TYPE_FLAGS multiTransportTypeFlags = MULTITRANSPORT_TYPE_FLAGS.None,
            bool hasEarlyCapabilityFlags = false,
            SC_earlyCapabilityFlags_Values earlyCapabilityFlagsValue = SC_earlyCapabilityFlags_Values.RNS_UD_SC_EDGE_ACTIONS_SUPPORTED,
            UInt16 mcsChannelId_Net = ConstValue.IO_CHANNEL_ID,
            UInt16 mcsChannelId_MSGChannel = ConstValue.MCS_MESSAGE_CHANNEL_ID)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (serverCertificate == null && this.encryptedProtocol == EncryptedProtocol.Rdp)
            {
                throw new ArgumentNullException("serverCertificate");
            }

            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsResponsePdu
                = new Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response(sessionContext);

            #region filling serverCoreData structure TS_UD_SC_CORE

            TS_UD_SC_CORE serverCoreData = new TS_UD_SC_CORE();
            int coreDataSize = 0;
            serverCoreData.header.type = TS_UD_HEADER_type_Values.SC_CORE;
            coreDataSize += Marshal.SizeOf(serverCoreData.header);
            serverCoreData.version = TS_UD_SC_CORE_version_Values.V2;
            coreDataSize += sizeof(uint);
            serverCoreData.clientRequestedProtocols = sessionContext.ClientRequestedProtocol;
            coreDataSize += sizeof(uint);

            if (hasEarlyCapabilityFlags)
            {
                serverCoreData.earlyCapabilityFlags = earlyCapabilityFlagsValue;
                coreDataSize += sizeof(uint);
            }

            serverCoreData.header.length = (ushort)coreDataSize;
            #endregion

            #region filling serverSecurityData TS_UD_SC_SEC1

            TS_UD_SC_SEC1 serverSecurityData = new TS_UD_SC_SEC1();
            serverSecurityData.header.type = TS_UD_HEADER_type_Values.SC_SECURITY;
            serverSecurityData.encryptionMethod = encryptionMethods;
            serverSecurityData.encryptionLevel = encryptionLevel;


            if (encryptionMethods == EncryptionMethods.ENCRYPTION_METHOD_NONE &&
                encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                serverSecurityData.serverRandom = null;
                serverSecurityData.serverRandomLen = new UInt32Class(0);
                serverSecurityData.serverCertificate = null;
                serverSecurityData.serverCertLen = new UInt32Class((uint)0);
            }
            else
            {
                serverSecurityData.serverRandom = RdpbcgrUtility.GenerateRandom(ConstValue.SERVER_RANDOM_SIZE);
                serverSecurityData.serverRandom = new byte[] {
                    0x10 ,0x11 ,0x77 ,0x20 ,0x30 ,0x61 ,0x0a ,0x12 ,0xe4 ,0x34 ,0xa1 ,0x1e ,0xf2 ,0xc3 ,0x9f ,0x31,
                    0x7d ,0xa4 ,0x5f ,0x01 ,0x89 ,0x34 ,0x96 ,0xe0 ,0xff ,0x11 ,0x08 ,0x69 ,0x7f ,0x1a ,0xc3 ,0xd2
                };
                serverSecurityData.serverRandomLen = new UInt32Class((uint)serverSecurityData.serverRandom.Length);
                serverSecurityData.serverCertificate = serverCertificate;
                serverSecurityData.serverCertLen = new UInt32Class((uint)serverCerLen);
            }



            int securityDataSize;
            if (serverSecurityData.serverCertificate == null ||
                serverSecurityData.serverRandomLen == null ||
                serverSecurityData.serverCertLen == null)
            {
                securityDataSize = Marshal.SizeOf((ushort)serverSecurityData.header.type)
                                     + Marshal.SizeOf(serverSecurityData.header.length)
                                     + Marshal.SizeOf((uint)serverSecurityData.encryptionMethod)
                                     + Marshal.SizeOf((uint)serverSecurityData.encryptionLevel);
            }
            else
            {
                securityDataSize = Marshal.SizeOf((ushort)serverSecurityData.header.type)
                                     + Marshal.SizeOf(serverSecurityData.header.length)
                                     + Marshal.SizeOf((uint)serverSecurityData.encryptionMethod)
                                     + Marshal.SizeOf((uint)serverSecurityData.encryptionLevel)
                                     + Marshal.SizeOf(serverSecurityData.serverRandomLen.actualData)
                                     + Marshal.SizeOf(serverSecurityData.serverCertLen.actualData)
                                     + (int)serverSecurityData.serverRandomLen.actualData
                                     + (int)serverSecurityData.serverCertLen.actualData;
            }

            serverSecurityData.header.length = (ushort)securityDataSize;

            #endregion

            #region Filling serverNetworkData TS_UD_CS_NET
            TS_UD_SC_NET serverNetworkData = new TS_UD_SC_NET();
            serverNetworkData.header.type = TS_UD_HEADER_type_Values.SC_NET;
            serverNetworkData.MCSChannelId = mcsChannelId_Net;

            if (sessionContext.VirtualChannelDefines != null)
            {
                serverNetworkData.channelCount = (ushort)sessionContext.VirtualChannelDefines.Length;
                serverNetworkData.channelIdArray = new ushort[serverNetworkData.channelCount];

                for (int i = 0; i < serverNetworkData.channelCount; ++i)
                {
                    serverNetworkData.channelIdArray[i] = sessionContext.VirtualChannelIdFactory.Dequeue();
                }
            }

            if (!RdpbcgrUtility.IsEven(serverNetworkData.channelCount))
            {
                serverNetworkData.Pad = new byte[2];
            }
            else
            {
                serverNetworkData.Pad = null;
            }

            int networkDataSize = Marshal.SizeOf(serverNetworkData.header)
                                + Marshal.SizeOf(serverNetworkData.MCSChannelId)
                                + Marshal.SizeOf(serverNetworkData.channelCount)
                                + (int)(ConstValue.CHANNEL_ID_SIZE * (ushort)serverNetworkData.channelCount);
            if (serverNetworkData.Pad != null)
            {
                networkDataSize += (int)serverNetworkData.Pad.Length;
            }
            serverNetworkData.header.length = (ushort)networkDataSize;
            #endregion       

            #region Filling serverMessageChannelData TS_UD_SC_MCS_MSGCHANNEL

            TS_UD_SC_MCS_MSGCHANNEL serverMessageChannelData = new TS_UD_SC_MCS_MSGCHANNEL();
            serverMessageChannelData.header.type = TS_UD_HEADER_type_Values.SC_MCS_MSGCHANNEL;
            serverMessageChannelData.MCSChannelID = mcsChannelId_MSGChannel;
            serverMessageChannelData.header.length = 6;

            #endregion

            #region Filling serverMultitransportChannelData TS_UD_SC_MULTITRANSPORT

            TS_UD_SC_MULTITRANSPORT serverMultitransportChannelData = new TS_UD_SC_MULTITRANSPORT();
            serverMultitransportChannelData.header.type = TS_UD_HEADER_type_Values.SC_MULTITRANSPORT;
            serverMultitransportChannelData.flags = multiTransportTypeFlags;
            serverMultitransportChannelData.header.length = 8;

            #endregion

            #region Filling connectGCC
            mcsResponsePdu.mcsCrsp.gccPdu.serverCoreData = serverCoreData;
            mcsResponsePdu.mcsCrsp.gccPdu.serverNetworkData = serverNetworkData;
            mcsResponsePdu.mcsCrsp.gccPdu.serverSecurityData = serverSecurityData;
            mcsResponsePdu.mcsCrsp.gccPdu.nodeID = ConstValue.GCC_RESPONSE_NODEID;
            mcsResponsePdu.mcsCrsp.gccPdu.tag = ConstValue.GCC_RESPONSE_TAG;
            mcsResponsePdu.mcsCrsp.gccPdu.ccrResult = ConstValue.GCC_RESPONSE_RESULT;
            mcsResponsePdu.mcsCrsp.gccPdu.H221Key = ConstValue.H221_KEY;
            if (sessionContext.IsClientMessageChannelDataRecieved)
                mcsResponsePdu.mcsCrsp.gccPdu.serverMessageChannelData = serverMessageChannelData;
            if (sessionContext.IsClientMultitransportChannelDataRecieved)
                mcsResponsePdu.mcsCrsp.gccPdu.serverMultitransportChannelData = serverMultitransportChannelData;
            #endregion

            #region Filling mcsConnectInitial
            mcsResponsePdu.mcsCrsp.result = ConstValue.MCS_RESPONSE_RESULT;
            mcsResponsePdu.mcsCrsp.calledConnectId = ConstValue.MCS_RESPONSE_CALLED_CONNECTED_ID;
            mcsResponsePdu.mcsCrsp.domainParameters.maxChannelIds = ConstValue.DOMAIN_PARAMETERS_MAX_CHANNEL_IDS;
            mcsResponsePdu.mcsCrsp.domainParameters.maxUserIds = ConstValue.DOMAIN_PARAMETERS_MAX_USER_IDS;
            mcsResponsePdu.mcsCrsp.domainParameters.maxTokenIds = ConstValue.DOMAIN_PARAMETERS_MAX_TOKEN_IDS;
            mcsResponsePdu.mcsCrsp.domainParameters.numPriorities = ConstValue.DOMAIN_PARAMETERS_NUM_PRIORITIES;
            mcsResponsePdu.mcsCrsp.domainParameters.minThroughput = ConstValue.DOMAIN_PARAMETERS_MIN_THROUGHPUT;
            mcsResponsePdu.mcsCrsp.domainParameters.maxHeight = ConstValue.DOMAIN_PARAMETERS_MAX_HEIGHT;
            mcsResponsePdu.mcsCrsp.domainParameters.maxMcsPduSize = ConstValue.DOMAIN_PARAMETERS_MAX_MCS_PDU_SIZE;
            mcsResponsePdu.mcsCrsp.domainParameters.protocolVersion = ConstValue.DOMAIN_PARAMETERS_PROTOCOL_VERSION;
            #endregion 

            #region Filling mcsInitialPdu
            RdpbcgrUtility.FillTpktHeader(ref mcsResponsePdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref mcsResponsePdu.x224Data);
            #endregion 

            return mcsResponsePdu;
        }


        /// <summary>
        /// Create MCS Erect Domain Request PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// <returns>MCS Erect Domain Request PDU.</returns>
        public Server_MCS_Attach_User_Confirm_Pdu CreateMCSAttachUserConfirmPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_MCS_Attach_User_Confirm_Pdu aucPdu = new Server_MCS_Attach_User_Confirm_Pdu(sessionContext);
            AttachUserConfirm mcsAucf = new AttachUserConfirm(new Result(0), new UserId(ConstValue.USER_ID));

            aucPdu.attachUserConfirm = mcsAucf;
            RdpbcgrUtility.FillTpktHeader(ref aucPdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref aucPdu.x224Data);

            return aucPdu;
        }


        /// <summary>
        /// Create MCS Channel Join Request PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// <param name="channelId">The channel Id to be joined. This value can be got by getting property
        /// VirtualChannels.</param>
        /// <returns>MCS Channel Join Request PDU.</returns>
        public Server_MCS_Channel_Join_Confirm_Pdu CreateMCSChannelJoinConfirmPdu(
            RdpbcgrServerSessionContext sessionContext,
            long channelID)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_MCS_Channel_Join_Confirm_Pdu cjcPdu = new Server_MCS_Channel_Join_Confirm_Pdu(sessionContext);
            ChannelJoinConfirm mcsCjc = new ChannelJoinConfirm(
                new Result(0),
                new UserId(sessionContext.UserChannelId),
                new ChannelId(channelID),
                new ChannelId(channelID));
            cjcPdu.channelJoinConfirm = mcsCjc;
            RdpbcgrUtility.FillTpktHeader(ref cjcPdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref cjcPdu.x224Data);

            return cjcPdu;
        }


        /// <summary>
        /// Create License Error Message PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPDU to send the packet.
        /// </summary>
        /// 
        /// <returns>License Error Message PDU.</returns>
        public Server_License_Error_Pdu_Valid_Client CreateLicenseErrorMessage(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_License_Error_Pdu_Valid_Client licenseErrorPdu = new Server_License_Error_Pdu_Valid_Client(sessionContext);
            RdpbcgrUtility.FillCommonHeader(
                sessionContext,
                ref licenseErrorPdu.commonHeader,
                TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT
                | TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_ENCRYPT_CS);

            //There's always security header for License Pdu
            if (sessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
            {
                licenseErrorPdu.commonHeader.securityHeader = new TS_SECURITY_HEADER();
                licenseErrorPdu.commonHeader.securityHeader.flags =
                    TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT
                    | TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_ENCRYPT_CS;
                licenseErrorPdu.commonHeader.securityHeader.flagsHi = 0;
            }
            LICENSE_PREAMBLE preamblePduData = new LICENSE_PREAMBLE();
            preamblePduData.bMsgType = bMsgType_Values.ERROR_ALERT;
            preamblePduData.bVersion = bVersion_Values.PREAMBLE_VERSION_3_0;
            preamblePduData.wMsgSize = ConstValue.LICENSING_PACKET_SIZE;

            LICENSE_ERROR_MESSAGE errorMessagePduData = new LICENSE_ERROR_MESSAGE();
            errorMessagePduData.dwErrorCode = dwErrorCode_Values.STATUS_VALID_CLIENT;
            errorMessagePduData.dwStateTransition = dwStateTransition_Values.ST_NO_TRANSITION;
            errorMessagePduData.bbErrorInfo.wBlobType = wBlobType_Values.BB_ERROR_BLOB;
            errorMessagePduData.bbErrorInfo.wBlobLen = ConstValue.LICENSING_BINARY_BLOB_LENGTH;

            licenseErrorPdu.preamble = preamblePduData;
            licenseErrorPdu.validClientMessage = errorMessagePduData;

            return licenseErrorPdu;
        }


        /// <summary>
        /// Create Confirm Active PDU with the default capability sets.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Confirm Active PDU.</returns>
        public Server_Demand_Active_Pdu CreateDemandActivePdu(
            RdpbcgrServerSessionContext sessionContext,
            Collection<ITsCapsSet> capabilitiesSet)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Demand_Active_Pdu demandActivePdu = new Server_Demand_Active_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext,
                                     ref demandActivePdu.commonHeader,
                                     defaultSecurityHeaderFlag
                                     );
            demandActivePdu.commonHeader.initiator = ConstValue.SERVER_CHANNEL_ID;
            TS_DEMAND_ACTIVE_PDU demandActivePduData = new TS_DEMAND_ACTIVE_PDU();

            if (capabilitiesSet == null)
            {
                demandActivePduData.capabilitySets = new Collection<ITsCapsSet>();
            }
            else
            {
                demandActivePduData.capabilitySets = capabilitiesSet;
            }

            int capabilitiesLength = 0;

            foreach (ITsCapsSet capability in demandActivePduData.capabilitySets)
            {
                capabilitiesLength += capability.ToBytes().Length;
            }

            demandActivePduData.shareId = ConstValue.SHAREID;
            demandActivePduData.lengthSourceDescriptor = (ushort)ConstValue.DEMAND_SOURCE_DESCRIPTOR.Length;
            demandActivePduData.lengthCombinedCapabilities = (ushort)(sizeof(ushort)
                                                            + sizeof(ushort)
                                                            + capabilitiesLength);
            demandActivePduData.sourceDescriptor = ConstValue.DEMAND_SOURCE_DESCRIPTOR;
            demandActivePduData.numberCapabilities = (ushort)demandActivePduData.capabilitySets.Count;
            demandActivePduData.pad2Octets = 0;
            demandActivePduData.sessionId = 0;

            int totalLength = Marshal.SizeOf(demandActivePduData.shareControlHeader)
                            + sizeof(uint)
                            + sizeof(ushort)
                            + sizeof(ushort)
                            + sizeof(uint)
                            + demandActivePduData.lengthSourceDescriptor
                            + demandActivePduData.lengthCombinedCapabilities;

            RdpbcgrUtility.FillShareControlHeader(ref demandActivePduData.shareControlHeader,
                                           (ushort)totalLength,
                                           ShareControlHeaderType.PDUTYPE_DEMANDACTIVEPDU,
                                           (ushort)sessionContext.ServerChannelId);

            demandActivePdu.demandActivePduData = demandActivePduData;

            return demandActivePdu;
        }


        /// <summary>
        /// Create Confirm Active PDU with the default capability sets.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Confirm Active PDU.</returns>
        public Server_Demand_Active_Pdu CreateDemandActivePdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Collection<ITsCapsSet> capabilitySets = CreateCapabilitySets();

            return CreateDemandActivePdu(sessionContext, capabilitySets);
        }


        /// <summary>
        /// Create Synchronize PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Synchronize PDU.</returns>
        public Server_Synchronize_Pdu CreateSynchronizePdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Synchronize_Pdu synchronizePdu = new Server_Synchronize_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext, ref synchronizePdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            TS_SYNCHRONIZE_PDU synchronizePduData = new TS_SYNCHRONIZE_PDU();
            synchronizePduData.messageType = messageType_Values.V1;
            synchronizePduData.targetUser = (ushort)sessionContext.IOChannelId;

            RdpbcgrUtility.FillShareDataHeader(ref synchronizePduData.shareDataHeader,
                (ushort)(Marshal.SizeOf((ushort)synchronizePduData.messageType) + Marshal.SizeOf(synchronizePduData.targetUser)),
                sessionContext,
                streamId_Values.STREAM_UNDEFINED,
                pduType2_Values.PDUTYPE2_SYNCHRONIZE,
                0,
                0);

            synchronizePdu.synchronizePduData = synchronizePduData;

            return synchronizePdu;
        }


        /// <summary>
        /// Create Control PDU Cooperate.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Control PDU Cooperate.</returns>
        public Server_Control_Pdu CreateControlCooperatePdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Control_Pdu controlCooperatePdu = new Server_Control_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext, ref controlCooperatePdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            TS_CONTROL_PDU controlPduData = new TS_CONTROL_PDU();
            controlPduData.action = action_Values.CTRLACTION_COOPERATE;
            controlPduData.grantId = 0;
            controlPduData.controlId = 0;
            RdpbcgrUtility.FillShareDataHeader(ref controlPduData.shareDataHeader,
                (ushort)(Marshal.SizeOf(controlPduData) - Marshal.SizeOf(controlPduData.shareDataHeader)),
                sessionContext,
                streamId_Values.STREAM_MED,
                pduType2_Values.PDUTYPE2_CONTROL,
                0,
                0);

            controlCooperatePdu.controlPduData = controlPduData;

            return controlCooperatePdu;
        }


        /// <summary>
        /// Create Request Control PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Request Control PDU.</returns>
        public Server_Control_Pdu CreateControlGrantedPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Control_Pdu controlGrantedPdu = new Server_Control_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext, ref controlGrantedPdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            TS_CONTROL_PDU controlPduData = new TS_CONTROL_PDU();
            controlPduData.action = action_Values.CTRLACTION_GRANTED_CONTROL;
            controlPduData.grantId = sessionContext.UserChannelId;
            controlPduData.controlId = (uint)sessionContext.ServerChannelId;
            RdpbcgrUtility.FillShareDataHeader(ref controlPduData.shareDataHeader,
                (ushort)(Marshal.SizeOf(controlPduData) - Marshal.SizeOf(controlPduData.shareDataHeader)),
                sessionContext,
                streamId_Values.STREAM_MED,
                pduType2_Values.PDUTYPE2_CONTROL,
                0,
                0);

            controlGrantedPdu.controlPduData = controlPduData;

            return controlGrantedPdu;
        }


        /// <summary>
        /// Create Font List PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after receiving Demand Active PDU.
        /// </summary>
        /// <returns>Font List PDU.</returns>
        public Server_Font_Map_Pdu CreateFontMapPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Font_Map_Pdu fontMapPdu = new Server_Font_Map_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext, ref fontMapPdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            TS_FONT_MAP_PDU fontMapPduData = new TS_FONT_MAP_PDU();
            fontMapPduData.numberEntries = 0;
            fontMapPduData.totalNumEntries = 0;
            fontMapPduData.mapFlags = ConstValue.FONTMAP_FIRST | ConstValue.FONTMAP_LAST;
            fontMapPduData.entrySize = ConstValue.FONTMAP_ENTRY_SIZE;

            int clientFontMapPacketLength = Marshal.SizeOf(fontMapPduData.numberEntries)
                                             + Marshal.SizeOf(fontMapPduData.totalNumEntries)
                                             + Marshal.SizeOf(fontMapPduData.mapFlags)
                                             + Marshal.SizeOf(fontMapPduData.entrySize);

            RdpbcgrUtility.FillShareDataHeader(ref fontMapPduData.shareDataHeader,
                (ushort)clientFontMapPacketLength,
                sessionContext,
                streamId_Values.STREAM_MED,
                pduType2_Values.PDUTYPE2_FONTMAP,
                0,
                0);

            fontMapPdu.fontMapPduData = fontMapPduData;

            return fontMapPdu;
        }

        #endregion


        #region Deactivation reactivation

        /// <summary>
        /// Create deactive all PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <returns>Deactive All PDU.</returns>
        public Server_Deactivate_All_Pdu CreateDeactivateAllPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Deactivate_All_Pdu deactivateAllPdu = new Server_Deactivate_All_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext,
                ref deactivateAllPdu.commonHeader,
                defaultSecurityHeaderFlag
                );

            TS_DEACTIVATE_ALL_PDU deactivateAllPduData = new TS_DEACTIVATE_ALL_PDU();
            deactivateAllPduData.shareId = (uint)(sessionContext.ServerChannelId << 16 | ConstValue.SHAREID_LOW);
            deactivateAllPduData.lengthSourceDescriptor = 1;
            deactivateAllPduData.sourceDescriptor = ConstValue.SOURCE_DESCRIPTOR;

            int totalLength = Marshal.SizeOf(deactivateAllPdu.deactivateAllPduData.shareControlHeader)
                                             + Marshal.SizeOf(deactivateAllPduData.shareId)
                                             + Marshal.SizeOf(deactivateAllPduData.lengthSourceDescriptor)
                                             + deactivateAllPduData.lengthSourceDescriptor;
            RdpbcgrUtility.FillShareControlHeader(
                ref deactivateAllPduData.shareControlHeader,
                (ushort)totalLength,
                ShareControlHeaderType.PDUTYPE_DEACTIVATEALLPDU,
                (ushort)sessionContext.ServerChannelId);

            deactivateAllPdu.deactivateAllPduData = deactivateAllPduData;
            return deactivateAllPdu;
        }
        #endregion 


        #region Auto reconnect
        /// <summary>
        /// Create auto reconnect status PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <returns>auto reconnect status PDU.</returns>
        public Server_Auto_Reconnect_Status_Pdu CreateAutoReconnectStatusPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Auto_Reconnect_Status_Pdu autoReconnectStatusPdu = new Server_Auto_Reconnect_Status_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext, ref autoReconnectStatusPdu.commonHeader,
                                     defaultSecurityHeaderFlag
                                     );

            TS_AUTORECONNECT_STATUS_PDU autoReconnectStatusPduData = new TS_AUTORECONNECT_STATUS_PDU();
            autoReconnectStatusPduData.arcStatus = ConstValue.ARC_STATUS;
            RdpbcgrUtility.FillShareDataHeader(ref autoReconnectStatusPduData.shareDataHeader,
                (ushort)(Marshal.SizeOf(autoReconnectStatusPduData.arcStatus)),
                sessionContext,
                streamId_Values.STREAM_MED,
                pduType2_Values.PDUTYPE2_ARC_STATUS_PDU,
                0,
                0);

            autoReconnectStatusPdu.arcStatusPduData = autoReconnectStatusPduData;
            return autoReconnectStatusPdu;
        }
        #endregion


        #region Disconnection
        /// <summary>
        /// Create Shutdown Request PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <returns>Shutdown Request PDU.</returns>
        public Server_Shutdown_Request_Denied_Pdu CreateShutdownRequestDeniedPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Shutdown_Request_Denied_Pdu shutdownDeniedPdu = new Server_Shutdown_Request_Denied_Pdu(sessionContext);
            TS_SHUTDOWN_DENIED_PDU shutdownDeniedPduData = new TS_SHUTDOWN_DENIED_PDU();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref shutdownDeniedPdu.commonHeader,
                                     defaultSecurityHeaderFlag
                                     );

            RdpbcgrUtility.FillShareDataHeader(ref shutdownDeniedPduData.shareDataHeader,
                                        0,
                                        sessionContext,
                                        streamId_Values.STREAM_LOW,
                                        pduType2_Values.PDUTYPE2_SHUTDOWN_DENIED,
                                        0,
                                        0);
            shutdownDeniedPdu.shutdownRequestDeniedPduData = shutdownDeniedPduData;

            return shutdownDeniedPdu;
        }


        /// <summary>
        /// Create MCS Disconnect Provider Ultimatum PDU.
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <param name="reason">The reason to be disconnected.</param>
        /// <returns>MCS Disconnect Provider Ultimatum PDU.</returns>
        public MCS_Disconnect_Provider_Ultimatum_Server_Pdu CreateMCSDisconnectProviderUltimatumPdu(RdpbcgrServerSessionContext sessionContext, int reason)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            MCS_Disconnect_Provider_Ultimatum_Server_Pdu mcsDisconnectProviderUltimatumPdu =
              new MCS_Disconnect_Provider_Ultimatum_Server_Pdu(sessionContext);
            mcsDisconnectProviderUltimatumPdu.disconnectProvider =
                new DisconnectProviderUltimatum(new Reason(reason));
            RdpbcgrUtility.FillTpktHeader(ref mcsDisconnectProviderUltimatumPdu.tpktHeader, 0);
            RdpbcgrUtility.FillX224Data(ref mcsDisconnectProviderUltimatumPdu.x224Data);

            return mcsDisconnectProviderUltimatumPdu;
        }
        #endregion


        #region Error reporting and status update
        /// <summary>
        /// Create set error info PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="errorInfo">Expressed what error happens.</param>
        /// <returns>set error info PDU.</returns>
        public Server_Set_Error_Info_Pdu CreateSetErrorInfoPdu(
            RdpbcgrServerSessionContext sessionContext,
            errorInfo_Values errorInfo)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Set_Error_Info_Pdu setErrorInfoPdu = new Server_Set_Error_Info_Pdu(sessionContext);
            TS_SET_ERROR_INFO_PDU setErrorInfoPduData = new TS_SET_ERROR_INFO_PDU();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref setErrorInfoPdu.commonHeader,
                                     defaultSecurityHeaderFlag
                                     );

            setErrorInfoPduData.errorInfo = errorInfo;
            int payloadLen = Marshal.SizeOf((uint)setErrorInfoPduData.errorInfo);

            RdpbcgrUtility.FillShareDataHeader(ref setErrorInfoPduData.shareDataHeader,
                                        (ushort)payloadLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_SET_ERROR_INFO_PDU,
                                        0,
                                        0);
            setErrorInfoPdu.errorInfoPduData = setErrorInfoPduData;

            return setErrorInfoPdu;
        }


        /// <summary>
        /// Create status info PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="statusCode">Expressed the status of server.</param>
        /// <returns>status info PDU.</returns>
        public Server_Status_Info_Pdu CreateStatusInfoPdu(
            RdpbcgrServerSessionContext sessionContext,
            StatusCode_Values statusCode)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Status_Info_Pdu statusInfoPdu = new Server_Status_Info_Pdu(sessionContext);

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref statusInfoPdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            int payloadLen = Marshal.SizeOf((uint)statusInfoPdu.statusCode);

            RdpbcgrUtility.FillShareDataHeader(ref statusInfoPdu.shareDataHeader,
                                        (ushort)payloadLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_STATUS_INFO_PDU,
                                        0,
                                        0);
            statusInfoPdu.statusCode = statusCode;

            return statusInfoPdu;
        }
        #endregion 


        #region Keyboard and mouse input
        /// <summary>
        /// Create Slow Path InputEvent PDU. This PDU includes 5 event, 
        /// INPUT_EVENT_SYNC, INPUT_EVENT_SCANCODE, INPUT_EVENT_UNICODE, INPUT_EVENT_MOUSE and
        /// INPUT_EVENT_MOUSEX for each.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length of tpktHeader 0, and it will be calculated automatically when encode the PDU to bytes.
        /// If the length isn't 0, then it will keep the value user set, and it will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <returns>Slow Path InputEvent PDU.</returns>
        public Server_Set_Keyboard_Indicators_Pdu CreateSetKeyboardIndicatorsPdu(
            RdpbcgrServerSessionContext sessionContext,
            LedFlags_Values ledFlagsValues)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Set_Keyboard_Indicators_Pdu keyboardIndicatorsPdu =
                new Server_Set_Keyboard_Indicators_Pdu(sessionContext);
            TS_SET_KEYBOARD_INDICATORS_PDU pduData = new TS_SET_KEYBOARD_INDICATORS_PDU();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref keyboardIndicatorsPdu.commonHeader,
                                     defaultSecurityHeaderFlag);
            pduData.UnitId = ConstValue.UNITID;
            pduData.LedFlags = (LedFlags_Values)ConstValue.LED_FLAGS;
            int pduDataLen = Marshal.SizeOf(pduData.UnitId) + Marshal.SizeOf((ushort)pduData.LedFlags);
            RdpbcgrUtility.FillShareDataHeader(ref pduData.shareDataHeader,
                                        (ushort)pduDataLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_SET_KEYBOARD_INDICATORS,
                                        0,
                                        0);
            keyboardIndicatorsPdu.setKeyBdIndicatorsPduData = pduData;

            return keyboardIndicatorsPdu;
        }


        /// <summary>
        /// Create Fast Path Input Event PDU. This PDU includes 5 event, 
        /// FASTPATH_INPUT_EVENT_SCANCODE, FASTPATH_INPUT_EVENT_MOUSE, FASTPATH_INPUT_EVENT_MOUSEX,
        /// FASTPATH_INPUT_EVENT_SYNC and FASTPATH_INPUT_EVENT_UNICODE for each.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Keep the length1 and length2 of TS_FP_INPUT_PDU and padlen of fipsInformation 0,
        /// and they will be calculated automatically when encode the PDU to bytes.
        /// If the they are not 0, then they will keep the value user set, and will not be calculated again.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <returns>Fast Path Input Event PDU.</returns>
        public Server_Set_Keyboard_IME_Status_Pdu CreateSetKeyboardIMEStatusPdu(
            RdpbcgrServerSessionContext sessionContext,
            ImeState_Values ImeOpen,
            ImeConvMode_Values ImeConvMode)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Set_Keyboard_IME_Status_Pdu keyboardImePdu = new Server_Set_Keyboard_IME_Status_Pdu(sessionContext);
            TS_SET_KEYBOARD_IME_STATUS_PDU pduData = new TS_SET_KEYBOARD_IME_STATUS_PDU();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref keyboardImePdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            pduData.UnitId = ConstValue.UNITID;
            pduData.ImeState = (uint)ImeOpen;
            pduData.ImeConvMode = (uint)ImeConvMode;
            int pduDataLen = Marshal.SizeOf(pduData.UnitId) +
                             Marshal.SizeOf(pduData.ImeState) +
                             Marshal.SizeOf(pduData.ImeConvMode);

            RdpbcgrUtility.FillShareDataHeader(ref pduData.shareDataHeader,
                                        (ushort)pduDataLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_SET_KEYBOARD_IME_STATUS,
                                        0,
                                        0);
            keyboardImePdu.setKeyBdImeStatusPduData = pduData;

            return keyboardImePdu;
        }
        #endregion 


        #region Basic output
        /// <summary>
        /// Create slow path update PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <returns>slow path update PDU.</returns>
        public SlowPathOutputPdu CreateSlowPathUpdataPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            SlowPathOutputPdu pdu = new SlowPathOutputPdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext, ref pdu.commonHeader,
                                     defaultSecurityHeaderFlag);
            List<RdpbcgrSlowPathUpdatePdu> listPdu = new List<RdpbcgrSlowPathUpdatePdu>();

#if UT
            TS_UPDATE_SYNC updateSync = new TS_UPDATE_SYNC();
            int syncLen = 0;
            updateSync.updateType = (ushort)updateType_Values.UPDATETYPE_SYNCHRONIZE;
            updateSync.pad2Octets = 12654;
            syncLen = Marshal.SizeOf((ushort)updateSync.updateType)
                      + Marshal.SizeOf(updateSync.pad2Octets);
            RdpbcgrUtility.FillShareDataHeader(ref updateSync.shareDataHeader,
                                               (ushort)syncLen,
                                               sessionContext,
                                               streamId_Values.STREAM_MED,
                                               pduType2_Values.PDUTYPE2_UPDATE,
                                               0,
                                               0);
            updateSync.shareDataHeader.pad1 = 203;
            updateSync.shareDataHeader.shareId = 66538;
            updateSync.shareDataHeader.shareControlHeader.pduSource = 1002;

            listPdu.Add(updateSync);

            TS_UPDATE_ORDERS updateOrders = new TS_UPDATE_ORDERS();
            int ordersLen = 0;
            updateOrders.updateType = updateType_Values.UPDATETYPE_ORDERS;
            updateOrders.pad2OctetA = 0;
            updateOrders.numberOrders = 2;
            updateOrders.pad2OctetsB = 25617;
            updateOrders.orderData = new byte[] {3, 9, 0, 32, 12, 5, 16, 1, 64, 10, 255, 255, 12, 132,
            0, 0, 0, 0, 0, 0, 0, 0, 25, 13, 56, 1, 16, 1, 204, 255, 127};
            ordersLen = Marshal.SizeOf((ushort)updateOrders.updateType)
                        + Marshal.SizeOf(updateOrders.pad2OctetA)
                        + Marshal.SizeOf(updateOrders.numberOrders)
                        + Marshal.SizeOf(updateOrders.pad2OctetsB)
                        + updateOrders.orderData.Length;
            RdpbcgrUtility.FillShareDataHeader(ref updateOrders.shareDataHeader,
                                               (ushort)ordersLen,
                                               sessionContext,
                                               streamId_Values.STREAM_MED,
                                               pduType2_Values.PDUTYPE2_UPDATE,
                                               0,
                                               0);
            updateOrders.shareDataHeader.pad1 = 120;
            updateOrders.shareDataHeader.shareId = 66538;
            updateOrders.shareDataHeader.shareControlHeader.pduSource = 1002;
            listPdu.Add(updateOrders);

            TS_UPDATE_SYNC updateSync2 = new TS_UPDATE_SYNC();
            int syncLen2 = 0;
            updateSync2.updateType = (ushort)updateType_Values.UPDATETYPE_SYNCHRONIZE;
            updateSync2.pad2Octets = 8293;
            syncLen2 = Marshal.SizeOf((ushort)updateSync2.updateType)
                      + Marshal.SizeOf(updateSync2.pad2Octets);
            RdpbcgrUtility.FillShareDataHeader(ref updateSync2.shareDataHeader,
                                               (ushort)syncLen2,
                                               sessionContext,
                                               streamId_Values.STREAM_MED,
                                               pduType2_Values.PDUTYPE2_UPDATE,
                                               0,
                                               0);
            updateSync2.shareDataHeader.pad1 = 67;
            updateSync2.shareDataHeader.shareId = 66538;
            updateSync2.shareDataHeader.shareControlHeader.pduSource = 1002;

            listPdu.Add(updateSync2);

            TS_POINTER_PDU ptrPdu = new TS_POINTER_PDU();
            int ptrLen = 0;
            ptrPdu.messageType = (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_SYSTEM;
            ptrPdu.pad2Octets = 25931;
            ptrPdu.pointerAttributeData = new byte[] { 0, 127, 0, 0 };
            ptrLen = Marshal.SizeOf((ushort)ptrPdu.messageType)
                     + Marshal.SizeOf(ptrPdu.pad2Octets)
                     + 4;
            RdpbcgrUtility.FillShareDataHeader(ref ptrPdu.shareDataHeader,
                                        (ushort)ptrLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_POINTER,
                                        0,
                                        0);
            ptrPdu.shareDataHeader.pad1 = 67;
            ptrPdu.shareDataHeader.shareId = 66538;
            ptrPdu.shareDataHeader.shareControlHeader.pduSource = 1002;
            listPdu.Add(ptrPdu);
#else

            TS_UPDATE_ORDERS updateOrders = new TS_UPDATE_ORDERS();
            int ordersLen = 0;
            updateOrders.updateType = updateType_Values.UPDATETYPE_ORDERS;
            updateOrders.pad2OctetA = 0;
            updateOrders.numberOrders = 2;
            updateOrders.pad2OctetsB = 25617;
            updateOrders.orderData = new byte[] {3, 9, 0, 32, 12, 5, 16, 1, 64, 10, 255, 255, 12, 132,
            0, 0, 0, 0, 0, 0, 0, 0, 25, 13, 56, 1, 16, 1, 204, 255, 127};
            ordersLen = Marshal.SizeOf((ushort)updateOrders.updateType)
                        + Marshal.SizeOf(updateOrders.pad2OctetA)
                        + Marshal.SizeOf(updateOrders.numberOrders)
                        + Marshal.SizeOf(updateOrders.pad2OctetsB)
                        + updateOrders.orderData.Length;
            RdpbcgrUtility.FillShareDataHeader(ref updateOrders.shareDataHeader,
                                               (ushort)ordersLen,
                                               sessionContext,
                                               streamId_Values.STREAM_MED,
                                               pduType2_Values.PDUTYPE2_UPDATE,
                                               0,
                                               0);
            updateOrders.shareDataHeader.pad1 = 120;
            updateOrders.shareDataHeader.shareId = 66538;
            updateOrders.shareDataHeader.shareControlHeader.pduSource = 1002;
            listPdu.Add(updateOrders);

            TS_UPDATE_SURFCMDS updateSurfCmds = new TS_UPDATE_SURFCMDS();
            int surfCmdsLen = 0;
            updateSurfCmds.shareDataHeader.compressedLength = 0;
            updateSurfCmds.shareDataHeader.compressedType = compressedType_Values.None;
            updateSurfCmds.shareDataHeader.pad1 = 0;
            updateSurfCmds.shareDataHeader.pduType2 = pduType2_Values.None;
            updateSurfCmds.shareDataHeader.shareControlHeader.pduSource = 0;
            updateSurfCmds.shareDataHeader.shareControlHeader.pduType.typeAndVersionLow = 0;
            updateSurfCmds.shareDataHeader.shareControlHeader.pduType.versionHigh = 0;
            updateSurfCmds.shareDataHeader.shareId = 0;
            updateSurfCmds.shareDataHeader.streamId = streamId_Values.STREAM_HI;
            updateSurfCmds.shareDataHeader.uncompressedLength = 0;
            updateSurfCmds.surfaceCommands = new TS_SURFCMD[0];
            updateSurfCmds.updateType = updateType_Values.UPDATETYPE_SURFCMDS;
            RdpbcgrUtility.FillShareDataHeader(ref updateSurfCmds.shareDataHeader,
                                        (ushort)surfCmdsLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_UPDATE,
                                        0,
                                        0);
            listPdu.Add(updateSurfCmds);

            TS_UPDATE_PALETTE updatePalette = new TS_UPDATE_PALETTE();
            int paletteLength = 0;
            TS_PALETTE_ENTRY paletteEntry = new TS_PALETTE_ENTRY();
            paletteEntry.red = ConstValue.PALETTE_ENTRY_RED;
            paletteEntry.green = ConstValue.PALETTE_ENTRY_GREEN;
            paletteEntry.blue = ConstValue.PALETTE_ENTRY_BLUE;
            updatePalette.paletteData.updateType = updateType_Values.UPDATETYPE_PALETTE;
            updatePalette.paletteData.pad2Octets = 0;
            updatePalette.paletteData.numberColors = ConstValue.NUMBER_COLORS;
            updatePalette.paletteData.paletteEntries = new TS_PALETTE_ENTRY[ConstValue.NUMBER_COLORS];

            for (int i = 0; i < updatePalette.paletteData.paletteEntries.Length; i++)
            {
                updatePalette.paletteData.paletteEntries[i] = paletteEntry;
            }
            RdpbcgrUtility.FillShareDataHeader(ref updatePalette.shareDataHeader,
                                        (ushort)paletteLength,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_UPDATE,
                                        0,
                                        0);
            listPdu.Add(updatePalette);

            TS_UPDATE_BITMAP updateBitmap = new TS_UPDATE_BITMAP();
            int bitmapLength = 0;
            updateBitmap.bitmapData.updateType = (ushort)updateType_Values.UPDATETYPE_BITMAP;
            updateBitmap.bitmapData.numberRectangles = ConstValue.NUMBER_RECT;
            updateBitmap.bitmapData.rectangles = new TS_BITMAP_DATA[ConstValue.NUMBER_RECT];
            TS_BITMAP_DATA bitmapData = new TS_BITMAP_DATA();
            bitmapData.destLeft = ConstValue.DEST_LEFT;
            bitmapData.destTop = ConstValue.DEST_TOP;
            bitmapData.destRight = ConstValue.DEST_RIGHT;
            bitmapData.destBottom = ConstValue.DEST_BOTTOM;
            bitmapData.width = ConstValue.WIDTH;
            bitmapData.height = ConstValue.HEIGHT;
            bitmapData.bitsPerPixel = ConstValue.BITSPERPIXEL;
            bitmapData.Flags = TS_BITMAP_DATA_Flags_Values.BITMAP_COMPRESSION;
            bitmapData.bitmapLength = 8;
            bitmapData.bitmapComprHdr.cbCompFirstRowSize = ConstValue.CB_COMP_FIRST_ROW_SIZE;
            bitmapData.bitmapComprHdr.cbCompMainBodySize = 0;
            bitmapData.bitmapComprHdr.cbScanWidth = 0;
            bitmapData.bitmapComprHdr.cbUncompressedSize = 0;
            bitmapData.bitmapDataStream = null;

            updateBitmap.bitmapData.rectangles[0] = bitmapData;

            bitmapLength += ConstValue.BITMAP_DATA_SIZE_DEFAULT + bitmapData.bitmapLength;

            bitmapData.destLeft = ConstValue.DEST_LEFT;
            bitmapData.destTop = ConstValue.DEST_TOP;
            bitmapData.destRight = ConstValue.DEST_RIGHT;
            bitmapData.destBottom = ConstValue.DEST_BOTTOM;
            bitmapData.width = ConstValue.WIDTH;
            bitmapData.height = ConstValue.HEIGHT;
            bitmapData.bitsPerPixel = ConstValue.BITSPERPIXEL;
            bitmapData.Flags = TS_BITMAP_DATA_Flags_Values.NO_BITMAP_COMPRESSION_HDR;
            bitmapData.bitmapLength = 0;
            bitmapData.bitmapComprHdr.cbCompFirstRowSize = ConstValue.CB_COMP_FIRST_ROW_SIZE;
            bitmapData.bitmapComprHdr.cbCompMainBodySize = 0;
            bitmapData.bitmapComprHdr.cbScanWidth = 0;
            bitmapData.bitmapComprHdr.cbUncompressedSize = 0;
            bitmapData.bitmapDataStream = null;

            updateBitmap.bitmapData.rectangles[1] = bitmapData;

            bitmapLength += ConstValue.BITMAP_DATA_SIZE_DEFAULT;

            bitmapLength += (int)(Marshal.SizeOf(updateBitmap.bitmapData.updateType)
                                  + Marshal.SizeOf(updateBitmap.bitmapData.numberRectangles));

            RdpbcgrUtility.FillShareDataHeader(ref updateBitmap.shareDataHeader,
                                        (ushort)bitmapLength,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_UPDATE,
                                        0,
                                        0);

            listPdu.Add(updateBitmap);


            TS_UPDATE_SYNC updateSync = new TS_UPDATE_SYNC();
            updateSync.updateType = (ushort)updateType_Values.UPDATETYPE_SYNCHRONIZE;
            updateSync.pad2Octets = 0;
            int syncLength = (int)(Marshal.SizeOf(updateSync.updateType)
                                    + Marshal.SizeOf(updateSync.pad2Octets));
            RdpbcgrUtility.FillShareDataHeader(ref updateSync.shareDataHeader,
                                        (ushort)syncLength,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_UPDATE,
                                        0,
                                        0);

            listPdu.Add(updateSync);

            TS_POINTER_PDU updatePtr1 = new TS_POINTER_PDU();
            int ptr1Length = 0;
            TS_POINTERPOSATTRIBUTE updatePtrPos = new TS_POINTERPOSATTRIBUTE();
            updatePtrPos.position.xPos = ConstValue.PTR_X_POS;
            updatePtrPos.position.yPos = ConstValue.PTR_Y_POS;
            updatePtr1.messageType = (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_POSITION;
            updatePtr1.pad2Octets = 0;
            updatePtr1.pointerAttributeData = updatePtrPos;

            ptr1Length += (int)(Marshal.SizeOf(updatePtr1.messageType)
                                    + Marshal.SizeOf(updatePtr1.pad2Octets)
                                    + Marshal.SizeOf(updatePtrPos.position.xPos)
                                    + Marshal.SizeOf(updatePtrPos.position.yPos));

            RdpbcgrUtility.FillShareDataHeader(ref updatePtr1.shareDataHeader,
                                        (ushort)ptr1Length,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_POINTER,
                                        0,
                                        0);

            listPdu.Add(updatePtr1);

            TS_POINTER_PDU updatePtr2 = new TS_POINTER_PDU();
            int ptr2Length = 0;
            TS_SYSTEMPOINTERATTRIBUTE updateSysPtr = new TS_SYSTEMPOINTERATTRIBUTE();
            updateSysPtr.systemPointerType = ConstValue.SYSPTR_DEFAULT;
            updatePtr2.messageType = (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_SYSTEM;
            updatePtr2.pad2Octets = 0;
            updatePtr2.pointerAttributeData = updateSysPtr;

            ptr2Length += (int)(Marshal.SizeOf(updatePtr2.messageType)
                                    + Marshal.SizeOf(updatePtr2.pad2Octets)
                                    + Marshal.SizeOf(updateSysPtr.systemPointerType));

            RdpbcgrUtility.FillShareDataHeader(ref updatePtr2.shareDataHeader,
                                        (ushort)ptr2Length,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_POINTER,
                                        0,
                                        0);

            listPdu.Add(updatePtr2);

            TS_POINTER_PDU updatePtr3 = new TS_POINTER_PDU();
            int ptr3Length = 0;
            TS_COLORPOINTERATTRIBUTE updateColPtr = new TS_COLORPOINTERATTRIBUTE();
            updateColPtr.cacheIndex = ConstValue.CACHE_INDEX;
            updateColPtr.hotSpot.xPos = ConstValue.HOTSPOT_X_POS;
            updateColPtr.hotSpot.yPos = ConstValue.HOTSPOT_Y_POS;
            updateColPtr.height = ConstValue.PTR_HEIGHT;
            updateColPtr.lengthAndMask = ConstValue.LEN_AND_MASK;
            updateColPtr.lengthXorMask = ConstValue.LEN_XOR_MASK;
            updateColPtr.xorMaskData = new byte[updateColPtr.lengthXorMask];
            updateColPtr.andMaskData = new byte[updateColPtr.lengthAndMask];
            updateColPtr.pad = 0;
            updatePtr3.messageType = (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_SYSTEM;
            updatePtr3.pad2Octets = 0;
            updatePtr3.pointerAttributeData = updateColPtr;

            ptr3Length += (int)(Marshal.SizeOf(updatePtr3.messageType)
                                    + Marshal.SizeOf(updatePtr3.pad2Octets)
                                    + ConstValue.COLOR_PTR_UPDATE_SIZE_DEFAULT);

            RdpbcgrUtility.FillShareDataHeader(ref updatePtr3.shareDataHeader,
                                        (ushort)ptr3Length,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_POINTER,
                                        0,
                                        0);

            listPdu.Add(updatePtr3);

            TS_POINTER_PDU updatePtr4 = new TS_POINTER_PDU();
            int ptr4Length = 0;
            TS_POINTERATTRIBUTE updatePtr = new TS_POINTERATTRIBUTE();
            updatePtr.xorBpp = ConstValue.XOR_BPP;
            updatePtr.colorPtrAttr.cacheIndex = ConstValue.CACHE_INDEX;
            updatePtr.colorPtrAttr.hotSpot.xPos = ConstValue.HOTSPOT_X_POS;
            updatePtr.colorPtrAttr.hotSpot.yPos = ConstValue.HOTSPOT_Y_POS;
            updatePtr.colorPtrAttr.height = ConstValue.PTR_HEIGHT;
            updatePtr.colorPtrAttr.lengthAndMask = ConstValue.LEN_AND_MASK;
            updatePtr.colorPtrAttr.lengthXorMask = ConstValue.LEN_XOR_MASK;
            updatePtr.colorPtrAttr.xorMaskData = new byte[updateColPtr.lengthXorMask];
            updatePtr.colorPtrAttr.andMaskData = new byte[updateColPtr.lengthAndMask];
            updatePtr.colorPtrAttr.pad = 0;
            updatePtr4.messageType = (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_SYSTEM;
            updatePtr4.pad2Octets = 0;
            updatePtr4.pointerAttributeData = updatePtr;

            ptr4Length += (int)(Marshal.SizeOf(updatePtr4.messageType)
                                    + Marshal.SizeOf(updatePtr4.pad2Octets)
                                    + ConstValue.COLOR_PTR_UPDATE_SIZE_DEFAULT
                                    + 2);

            RdpbcgrUtility.FillShareDataHeader(ref updatePtr4.shareDataHeader,
                                        (ushort)ptr4Length,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_POINTER,
                                        0,
                                        0);

            listPdu.Add(updatePtr4);

            TS_POINTER_PDU updatePtr5 = new TS_POINTER_PDU();
            int ptr5Length = 0;
            TS_CACHEDPOINTERATTRIBUTE updateCacPtr = new TS_CACHEDPOINTERATTRIBUTE();
            updateCacPtr.cacheIndex = ConstValue.CACHE_INDEX;
            updatePtr5.messageType = (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_SYSTEM;
            updatePtr5.pad2Octets = 0;
            updatePtr5.pointerAttributeData = updateCacPtr;

            RdpbcgrUtility.FillShareDataHeader(ref updatePtr5.shareDataHeader,
                                        (ushort)ptr5Length,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_POINTER,
                                        0,
                                        0);

            ptr5Length += (int)(Marshal.SizeOf(updatePtr5.messageType)
                                    + Marshal.SizeOf(updatePtr5.pad2Octets)
                                    + Marshal.SizeOf(updateCacPtr.cacheIndex));

            listPdu.Add(updatePtr5);
#endif

            RdpbcgrSlowPathUpdatePdu[] arrayPdu = new RdpbcgrSlowPathUpdatePdu[listPdu.Count];
            for (int i = 0; i < arrayPdu.Length; i++)
            {
                arrayPdu[i] = listPdu[i];
            }

            pdu.slowPathUpdates = arrayPdu;

            return pdu;
        }


        /// <summary>
        /// Create play sound PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <returns>play sound PDU.</returns>
        public Server_Play_Sound_Pdu CreatePlaySoundPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Play_Sound_Pdu playSoundPdu = new Server_Play_Sound_Pdu(sessionContext);
            TS_PLAY_SOUND_PDU_DATA pduData = new TS_PLAY_SOUND_PDU_DATA();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref playSoundPdu.commonHeader,
                                     defaultSecurityHeaderFlag);
            pduData.duration = ConstValue.DURATION;
            pduData.frequency = ConstValue.FREQUENCY;

            int pduDataLen = Marshal.SizeOf(pduData.duration) + Marshal.SizeOf(pduData.frequency);

            RdpbcgrUtility.FillShareDataHeader(ref pduData.shareDataHeader,
                                        (ushort)pduDataLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_PLAY_SOUND,
                                        0,
                                        0);

            playSoundPdu.playSoundPduData = pduData;

            return playSoundPdu;
        }


        /// <summary>
        /// Create fast path update PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <returns>fast path update PDU.</returns>
        public TS_FP_UPDATE_PDU CreateFastPathUpdatePdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            TS_FP_UPDATE_PDU fastpathOutputPdu = new TS_FP_UPDATE_PDU(sessionContext);
            List<TS_FP_UPDATE> outputUpdateList = new List<TS_FP_UPDATE>();
            TS_FP_UPDATE outputUpdate = new TS_FP_UPDATE();

            fastpathOutputPdu.fpOutputHeader =
                (byte)(((int)nested_TS_FP_UPDATE_PDU_fpOutputHeader_actionCode_Values.FASTPATH_OUTPUT_ACTION_FASTPATH & 0x03)
                | ((int)((int)reserved_Values.V1 & 0x0f) << 2));

            if (sessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                // encryptionFlags (2 bits): A higher 2-bit field containing the flags 
                // that describe the cryptographic parameters of the PDU.
                fastpathOutputPdu.fpOutputHeader |=
                    (byte)((int)encryptionFlagsChgd_Values.FASTPATH_OUTPUT_ENCRYPTED << 6);
            }

            fastpathOutputPdu.dataSignature = null;
            fastpathOutputPdu.length1 = 0;
            fastpathOutputPdu.length2 = 0;

            if (sessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                fastpathOutputPdu.fipsInformation.length = TS_FP_FIPS_INFO_length_Values.V1;
                fastpathOutputPdu.fipsInformation.version = ConstValue.TSFIPS_VERSION1;
                fastpathOutputPdu.fipsInformation.padlen = 0;
            }

            int totalSize = 0;

            #region fill in TS_FP_UPDATE

            TS_FP_UPDATE_ORDERS updateOrders = new TS_FP_UPDATE_ORDERS();
            updateOrders.updateHeader = 0;
            updateOrders.compressionFlags = compressedType_Values.None;
            updateOrders.updateOrders = new byte[] { 0 };
            updateOrders.size = (ushort)(Marshal.SizeOf(updateOrders.updateHeader)
                + Marshal.SizeOf((ushort)updateOrders.compressionFlags)
                + updateOrders.updateOrders.Length
                + updateOrders.updateOrders.Length);
            outputUpdateList.Add(updateOrders);
            totalSize += updateOrders.size;

            TS_FP_UPDATE_PALETTE paletteUpdate = new TS_FP_UPDATE_PALETTE();

            paletteUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_PALETTE & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            paletteUpdate.compressionFlags = compressedType_Values.None;
            paletteUpdate.paletteUpdateData.updateType = updateType_Values.UPDATETYPE_PALETTE;
            paletteUpdate.paletteUpdateData.numberColors = ConstValue.NUMBER_COLORS;
            paletteUpdate.paletteUpdateData.pad2Octets = 0;

            TS_PALETTE_ENTRY paletteEntry = new TS_PALETTE_ENTRY();
            paletteEntry.red = ConstValue.PALETTE_ENTRY_RED;
            paletteEntry.blue = ConstValue.PALETTE_ENTRY_BLUE;
            paletteEntry.green = ConstValue.PALETTE_ENTRY_GREEN;

            paletteUpdate.paletteUpdateData.paletteEntries = new TS_PALETTE_ENTRY[ConstValue.NUMBER_COLORS];
            for (int i = 0; i < paletteUpdate.paletteUpdateData.paletteEntries.Length; i++)
            {
                paletteUpdate.paletteUpdateData.paletteEntries[i] = paletteEntry;
            }


            paletteUpdate.size = (ushort)(Marshal.SizeOf((ushort)paletteUpdate.paletteUpdateData.updateType)
                                + Marshal.SizeOf(paletteUpdate.paletteUpdateData.pad2Octets)
                                + Marshal.SizeOf(paletteUpdate.paletteUpdateData.numberColors)
                                + 3);

            outputUpdateList.Add(paletteUpdate);
            totalSize += paletteUpdate.size;

            TS_FP_UPDATE_BITMAP bitmapUpdate = new TS_FP_UPDATE_BITMAP();

            bitmapUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_BITMAP & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            bitmapUpdate.compressionFlags = compressedType_Values.None;
            bitmapUpdate.bitmapUpdateData.updateType = (ushort)updateType_Values.UPDATETYPE_BITMAP;
            bitmapUpdate.bitmapUpdateData.numberRectangles = 1;
            TS_BITMAP_DATA bitmapData = new TS_BITMAP_DATA();
            bitmapData.destLeft = ConstValue.DEST_LEFT;
            bitmapData.destTop = ConstValue.DEST_TOP;
            bitmapData.destRight = ConstValue.DEST_RIGHT;
            bitmapData.destBottom = ConstValue.DEST_BOTTOM;
            bitmapData.width = ConstValue.DESKTOP_WIDTH_DEFAULT;
            bitmapData.height = ConstValue.DESKTOP_HEIGHT_DEFAULT;
            bitmapData.bitsPerPixel = ConstValue.BITSPERPIXEL;
            bitmapData.Flags = TS_BITMAP_DATA_Flags_Values.NO_BITMAP_COMPRESSION_HDR;
            bitmapData.bitmapComprHdr.cbUncompressedSize = 0;
            bitmapData.bitmapComprHdr.cbCompFirstRowSize = cbCompFirstRowSize_Values.V1;
            bitmapData.bitmapComprHdr.cbCompMainBodySize = 0;
            bitmapData.bitmapComprHdr.cbScanWidth = 0;
            bitmapData.bitmapDataStream = null;
            bitmapData.bitmapLength = 26;
            bitmapUpdate.bitmapUpdateData.rectangles = new TS_BITMAP_DATA[1];
            bitmapUpdate.bitmapUpdateData.rectangles[0] = bitmapData;

            bitmapUpdate.size = (ushort)(Marshal.SizeOf((ushort)bitmapUpdate.bitmapUpdateData.updateType)
                                + Marshal.SizeOf(bitmapUpdate.bitmapUpdateData.numberRectangles)
                                + ConstValue.BITMAP_DATA_SIZE_DEFAULT);

            outputUpdateList.Add(bitmapUpdate);
            totalSize += bitmapUpdate.size;

            TS_FP_UPDATE_SYNCHRONIZE syncUpdate = new TS_FP_UPDATE_SYNCHRONIZE();

            syncUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_SYNCHRONIZE & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            syncUpdate.compressionFlags = compressedType_Values.None;

            syncUpdate.size = 0;

            outputUpdateList.Add(syncUpdate);

            TS_FP_POINTERPOSATTRIBUTE ptrPosUpdate = new TS_FP_POINTERPOSATTRIBUTE();

            ptrPosUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_PTR_POSITION & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            ptrPosUpdate.compressionFlags = compressedType_Values.None;

            ptrPosUpdate.pointerPositionUpdateData.position.xPos = ConstValue.X_POS;
            ptrPosUpdate.pointerPositionUpdateData.position.yPos = ConstValue.Y_POS;

            ptrPosUpdate.size = (ushort)(Marshal.SizeOf(ptrPosUpdate.pointerPositionUpdateData.position.xPos)
                                + Marshal.SizeOf(ptrPosUpdate.pointerPositionUpdateData.position.xPos));

            outputUpdateList.Add(ptrPosUpdate);
            totalSize += ptrPosUpdate.size;

            TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE ptrHiddenUpdate = new TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE();

            // integrate update type, fragment sequencing and compression usage indication in to 1 byte update header
            ptrHiddenUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_PTR_NULL & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            ptrHiddenUpdate.compressionFlags = compressedType_Values.None;

            ptrHiddenUpdate.size = 0;

            outputUpdateList.Add(ptrHiddenUpdate);

            TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE ptrDefaultUpdate = new TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE();

            ptrDefaultUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_PTR_DEFAULT & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            ptrDefaultUpdate.compressionFlags = compressedType_Values.None;

            ptrDefaultUpdate.size = 0;

            outputUpdateList.Add(ptrDefaultUpdate);

            TS_FP_COLORPOINTERATTRIBUTE colorPtrUpdate = new TS_FP_COLORPOINTERATTRIBUTE();

            colorPtrUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_PTR_POSITION & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            colorPtrUpdate.compressionFlags = compressedType_Values.None;

            colorPtrUpdate.colorPointerUpdateData.cacheIndex = ConstValue.CACHE_INDEX;
            colorPtrUpdate.colorPointerUpdateData.hotSpot.xPos = ConstValue.HOTSPOT_X_POS;
            colorPtrUpdate.colorPointerUpdateData.hotSpot.yPos = ConstValue.HOTSPOT_Y_POS;
            colorPtrUpdate.colorPointerUpdateData.height = ConstValue.PTR_HEIGHT;
            colorPtrUpdate.colorPointerUpdateData.lengthAndMask = ConstValue.LEN_AND_MASK;
            colorPtrUpdate.colorPointerUpdateData.lengthXorMask = ConstValue.LEN_XOR_MASK;
            colorPtrUpdate.colorPointerUpdateData.xorMaskData = new byte[ConstValue.LEN_XOR_MASK];
            colorPtrUpdate.colorPointerUpdateData.andMaskData = new byte[ConstValue.LEN_AND_MASK];
            colorPtrUpdate.colorPointerUpdateData.pad = 0;

            colorPtrUpdate.size = (ushort)ConstValue.COLOR_PTR_UPDATE_SIZE_DEFAULT;

            outputUpdateList.Add(colorPtrUpdate);
            totalSize += colorPtrUpdate.size;

            TS_FP_POINTERATTRIBUTE newPtrUpdate = new TS_FP_POINTERATTRIBUTE();

            newPtrUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_POINTER & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            newPtrUpdate.compressionFlags = compressedType_Values.None;

            newPtrUpdate.newPointerUpdateData.xorBpp = ConstValue.XOR_BPP;
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.cacheIndex = ConstValue.CACHE_INDEX;
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.hotSpot.xPos = ConstValue.HOTSPOT_X_POS;
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.hotSpot.yPos = ConstValue.HOTSPOT_Y_POS;
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.height = ConstValue.PTR_HEIGHT;
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.lengthAndMask = ConstValue.LEN_AND_MASK;
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.lengthXorMask = ConstValue.LEN_XOR_MASK;
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.xorMaskData = new byte[ConstValue.LEN_XOR_MASK];
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.andMaskData = new byte[ConstValue.LEN_AND_MASK];
            newPtrUpdate.newPointerUpdateData.colorPtrAttr.pad = 0;

            newPtrUpdate.size = (ushort)(ConstValue.COLOR_PTR_UPDATE_SIZE_DEFAULT + 2);

            outputUpdateList.Add(newPtrUpdate);
            totalSize += newPtrUpdate.size;

            TS_FP_CACHEDPOINTERATTRIBUTE cachedPtrUpdate = new TS_FP_CACHEDPOINTERATTRIBUTE();

            cachedPtrUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_CACHED & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            cachedPtrUpdate.compressionFlags = compressedType_Values.None;

            cachedPtrUpdate.cachedPointerUpdateData.cacheIndex = ConstValue.CACHE_INDEX;

            newPtrUpdate.size = (ushort)(ConstValue.CACHED_PTR_ATTRIBUTE_SIZE_DEFAULT);

            outputUpdateList.Add(newPtrUpdate);
            totalSize += newPtrUpdate.size;

            TS_FP_SURFCMDS surfCmdsUpdate = new TS_FP_SURFCMDS();

            surfCmdsUpdate.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_SURFCMDS & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
            surfCmdsUpdate.compressionFlags = compressedType_Values.None;
            TS_SURFCMD_SET_SURF_BITS surfBitsCmd = new TS_SURFCMD_SET_SURF_BITS();
            surfBitsCmd.cmdType = cmdType_Values.CMDTYPE_SET_SURFACE_BITS;
            surfBitsCmd.destLeft = ConstValue.DEST_LEFT;
            surfBitsCmd.destTop = ConstValue.DEST_TOP;
            surfBitsCmd.destRight = ConstValue.DEST_RIGHT;
            surfBitsCmd.destBottom = ConstValue.DEST_BOTTOM;
            surfBitsCmd.bitmapData.bpp = ConstValue.BPP;
            // TODO: flags equals TSBitmapDataExFlags_Values.TSBitmapDataExFlags_Values
            surfBitsCmd.bitmapData.flags = TSBitmapDataExFlags_Values.None;
            surfBitsCmd.bitmapData.reserved = 0;
            surfBitsCmd.bitmapData.codecID = 0;
            surfBitsCmd.bitmapData.width = 0;
            surfBitsCmd.bitmapData.height = 0;
            surfBitsCmd.bitmapData.width = 0;
            surfBitsCmd.bitmapData.height = 0;
            surfBitsCmd.bitmapData.bitmapDataLength = 0;
            surfBitsCmd.bitmapData.bitmapData = null;
            surfCmdsUpdate.surfaceCommands = new TS_SURFCMD[1];
            surfCmdsUpdate.surfaceCommands[0] = surfBitsCmd;

            surfCmdsUpdate.size = (ushort)(ConstValue.SURF_CMDS_SIZE_DEFAULT);

            outputUpdateList.Add(surfCmdsUpdate);
            totalSize += surfCmdsUpdate.size;
            #endregion

            fastpathOutputPdu.fpOutputUpdates = new TS_FP_UPDATE[outputUpdateList.Count];

            for (int i = 0; i < outputUpdateList.Count; ++i)
            {
                fastpathOutputPdu.fpOutputUpdates[i] = outputUpdateList[i];
            }

            fastpathOutputPdu.length1 = (byte)(totalSize >> 7);
            fastpathOutputPdu.length2 = (byte)(totalSize & 127);

            return fastpathOutputPdu;
        }

        /// <summary>
        /// Create fast path update PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="updates">An array of Fast-Path Update structures to be processed by the client.</param>
        /// <returns>fast path update PDU.</returns>
        public TS_FP_UPDATE_PDU CreateFastPathUpdatePdu(RdpbcgrServerSessionContext sessionContext, TS_FP_UPDATE[] udpates)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            TS_FP_UPDATE_PDU fastpathOutputPdu = new TS_FP_UPDATE_PDU(sessionContext);

            fastpathOutputPdu.fpOutputHeader =
                (byte)(((int)nested_TS_FP_UPDATE_PDU_fpOutputHeader_actionCode_Values.FASTPATH_OUTPUT_ACTION_FASTPATH & 0x03)
                | ((int)((int)reserved_Values.V1 & 0x0f) << 2));

            if (sessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && sessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW)
            {
                // encryptionFlags (2 bits): A higher 2-bit field containing the flags 
                // that describe the cryptographic parameters of the PDU.
                fastpathOutputPdu.fpOutputHeader |=
                    (byte)((int)encryptionFlagsChgd_Values.FASTPATH_OUTPUT_ENCRYPTED << 6);
            }

            fastpathOutputPdu.dataSignature = null;
            fastpathOutputPdu.length1 = 0;
            fastpathOutputPdu.length2 = 0;

            if (sessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                fastpathOutputPdu.fipsInformation.length = TS_FP_FIPS_INFO_length_Values.V1;
                fastpathOutputPdu.fipsInformation.version = ConstValue.TSFIPS_VERSION1;
                fastpathOutputPdu.fipsInformation.padlen = 0;
            }

            fastpathOutputPdu.fpOutputUpdates = udpates;

            return fastpathOutputPdu;
        }
        #endregion


        #region Logon notification
        /// <summary>
        /// Create save session info PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="infoData">The logon info PDU sent to client.</param>
        /// <returns>save session info PDU.</returns>
        public Server_Save_Session_Info_Pdu CreateSaveSessionInfoPdu(
            RdpbcgrServerSessionContext sessionContext,
            TS_LOGON_INFO infoData)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Save_Session_Info_Pdu saveSessionInfoPdu = new Server_Save_Session_Info_Pdu(sessionContext);
            TS_SAVE_SESSION_INFO_PDU_DATA saveSessionInfoPduData = new TS_SAVE_SESSION_INFO_PDU_DATA();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref saveSessionInfoPdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            saveSessionInfoPduData.infoType = infoType_Values.INFOTYPE_LOGON;
            TS_LOGON_INFO logonInfoData = infoData;
            saveSessionInfoPduData.infoData = logonInfoData;
            int totalLen = Marshal.SizeOf((uint)saveSessionInfoPduData.infoType)
                           + Marshal.SizeOf(infoData.SessionId)
                           + Marshal.SizeOf(infoData.cbUserName)
                           + Marshal.SizeOf(infoData.cbDomain)
                           + ConstValue.TS_LOGON_INFO_DOMAIN_LENGTH
                           + ConstValue.TS_LOGON_INFO_USER_NAME_LENGTH
                           ;

            RdpbcgrUtility.FillShareDataHeader(ref saveSessionInfoPduData.shareDataHeader,
                                        (ushort)totalLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_SAVE_SESSION_INFO,
                                        0,
                                        0);
            saveSessionInfoPdu.saveSessionInfoPduData = saveSessionInfoPduData;

            return saveSessionInfoPdu;
        }


        /// <summary>
        /// Create save session info PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="infoData">The logon info PDU sent to client.</param>
        /// <returns>save session info PDU.</returns>
        public Server_Save_Session_Info_Pdu CreateSaveSessionInfoPdu(
            RdpbcgrServerSessionContext sessionContext,
            TS_LOGON_INFO_VERSION_2 infoData)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Save_Session_Info_Pdu saveSessionInfoPdu = new Server_Save_Session_Info_Pdu(sessionContext);
            TS_SAVE_SESSION_INFO_PDU_DATA saveSessionInfoPduData = new TS_SAVE_SESSION_INFO_PDU_DATA();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref saveSessionInfoPdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            saveSessionInfoPduData.infoType = infoType_Values.INFOTYPE_LOGON_LONG;
            saveSessionInfoPduData.infoData = infoData;

            int totalLen = Marshal.SizeOf((uint)saveSessionInfoPduData.infoType)
                           + Marshal.SizeOf(infoData.SessionId)
                           + Marshal.SizeOf(infoData.cbUserName)
                           + Marshal.SizeOf(infoData.cbDomain)
                           + (int)infoData.cbUserName
                           + (int)infoData.cbDomain
                           + Marshal.SizeOf((ushort)infoData.Version)
                           + Marshal.SizeOf(infoData.Size)
                           + infoData.Pad.Length
                           ;

            RdpbcgrUtility.FillShareDataHeader(ref saveSessionInfoPduData.shareDataHeader,
                                        (ushort)totalLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_SAVE_SESSION_INFO,
                                        0,
                                        0);
            saveSessionInfoPdu.saveSessionInfoPduData = saveSessionInfoPduData;

            return saveSessionInfoPdu;
        }


        /// <summary>
        /// Create save session info PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="infoData">The logon info PDU sent to client.</param>
        /// <returns>save session info PDU.</returns>
        public Server_Save_Session_Info_Pdu CreateSaveSessionInfoPdu(
            RdpbcgrServerSessionContext sessionContext,
            TS_PLAIN_NOTIFY infoData)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Save_Session_Info_Pdu saveSessionInfoPdu = new Server_Save_Session_Info_Pdu(sessionContext);
            TS_SAVE_SESSION_INFO_PDU_DATA saveSessionInfoPduData = new TS_SAVE_SESSION_INFO_PDU_DATA();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref saveSessionInfoPdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            saveSessionInfoPduData.infoType = infoType_Values.INFOTYPE_LOGON_PLAINNOTIFY;
            saveSessionInfoPduData.infoData = infoData;
            int totalLen = Marshal.SizeOf((uint)saveSessionInfoPduData.infoType)
                           + infoData.Pad.Length
                           ;

            RdpbcgrUtility.FillShareDataHeader(ref saveSessionInfoPduData.shareDataHeader,
                                        (ushort)totalLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_SAVE_SESSION_INFO,
                                        0,
                                        0);
            saveSessionInfoPdu.saveSessionInfoPduData = saveSessionInfoPduData;

            return saveSessionInfoPdu;
        }


        /// <summary>
        /// Create save session info PDU sent with reconnect info.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="infoData">The reconnect info sent to client.</param>
        /// <returns>save session info PDU.</returns>
        public Server_Save_Session_Info_Pdu CreateSaveSessionInfoPdu(
            RdpbcgrServerSessionContext sessionContext,
            TS_LOGON_INFO_EXTENDED infoData)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Save_Session_Info_Pdu saveSessionInfoPdu = new Server_Save_Session_Info_Pdu(sessionContext);
            TS_SAVE_SESSION_INFO_PDU_DATA saveSessionInfoPduData = new TS_SAVE_SESSION_INFO_PDU_DATA();

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref saveSessionInfoPdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            saveSessionInfoPduData.infoType = infoType_Values.INFOTYPE_LOGON_EXTENDED_INF;
            saveSessionInfoPduData.infoData = infoData;

            int totalLen = Marshal.SizeOf((uint)saveSessionInfoPduData.infoType)
                           + Marshal.SizeOf(infoData.Length)
                           + Marshal.SizeOf((uint)infoData.FieldsPresent)
                           + infoData.Pad.Length;

            for (int i = 0; i < infoData.LogonFields.Length; i++)
            {
                if (infoData.LogonFields[i].FieldData.GetType() == typeof(TS_LOGON_ERRORS_INFO))
                {
                    totalLen += Marshal.SizeOf(infoData.LogonFields[i].cbFieldData)
                                + Marshal.SizeOf((uint)((TS_LOGON_ERRORS_INFO)infoData.LogonFields[i].FieldData).ErrorNotificationType)
                                + Marshal.SizeOf(((TS_LOGON_ERRORS_INFO)infoData.LogonFields[i].FieldData).ErrorNotificationData);
                }
                else if (infoData.LogonFields[i].FieldData.GetType() == typeof(ARC_SC_PRIVATE_PACKET))
                {
                    totalLen += Marshal.SizeOf(infoData.LogonFields[i].cbFieldData)
                                + ((ARC_SC_PRIVATE_PACKET)infoData.LogonFields[i].FieldData).ArcRandomBits.Length
                                + Marshal.SizeOf((uint)((ARC_SC_PRIVATE_PACKET)infoData.LogonFields[i].FieldData).cbLen)
                                + Marshal.SizeOf(((ARC_SC_PRIVATE_PACKET)infoData.LogonFields[i].FieldData).LogonId)
                                + Marshal.SizeOf((uint)((ARC_SC_PRIVATE_PACKET)infoData.LogonFields[i].FieldData).Version);
                }
            }

            RdpbcgrUtility.FillShareDataHeader(ref saveSessionInfoPduData.shareDataHeader,
                                        (ushort)totalLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_SAVE_SESSION_INFO,
                                        0,
                                        0);
            saveSessionInfoPdu.saveSessionInfoPduData = saveSessionInfoPduData;

            return saveSessionInfoPdu;
        }
        #endregion Logon Notification


        #region Display update notification
        /// <summary>
        /// Create monitor layout PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <returns>Monitor layout PDU.</returns>
        public TS_MONITOR_LAYOUT_PDU CreateMonitorLayoutPdu(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            TS_MONITOR_LAYOUT_PDU monitorLayoutPdu = new TS_MONITOR_LAYOUT_PDU(sessionContext);

            RdpbcgrUtility.FillCommonHeader(sessionContext, ref monitorLayoutPdu.commonHeader,
                                     defaultSecurityHeaderFlag);

            monitorLayoutPdu.monitorCount = 1;
            monitorLayoutPdu.monitorDefArray = new TS_MONITOR_DEF[1];
            monitorLayoutPdu.monitorDefArray[0].bottom = 1;
            monitorLayoutPdu.monitorDefArray[0].flags = (Flags_TS_MONITOR_DEF)16777216;
            monitorLayoutPdu.monitorDefArray[0].left = 0;
            monitorLayoutPdu.monitorDefArray[0].right = 1;
            monitorLayoutPdu.monitorDefArray[0].top = 0;

            int monitorDefLen = Marshal.SizeOf(monitorLayoutPdu.monitorDefArray[0].bottom)
                + Marshal.SizeOf((uint)monitorLayoutPdu.monitorDefArray[0].flags)
                + Marshal.SizeOf(monitorLayoutPdu.monitorDefArray[0].left)
                + Marshal.SizeOf(monitorLayoutPdu.monitorDefArray[0].right)
                + Marshal.SizeOf(monitorLayoutPdu.monitorDefArray[0].top);
            int payloadLen = Marshal.SizeOf(monitorLayoutPdu.monitorCount)
                + (int)(monitorLayoutPdu.monitorCount * monitorDefLen);

            RdpbcgrUtility.FillShareDataHeader(ref monitorLayoutPdu.shareDataHeader,
                                        (ushort)payloadLen,
                                        sessionContext,
                                        streamId_Values.STREAM_MED,
                                        pduType2_Values.PDUTYPE2_MONITOR_LAYOUT_PDU,
                                        0,
                                        0);

            return monitorLayoutPdu;
        }
        #endregion


        #region Virtual channel
        /// <summary>
        /// Create Virtual Channel Raw PDU. The argument virtualChannelData is considered as 
        /// having been processed beyond this method, so it will not be processed again.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <param name="channelId">The channel Id of the virtual channel.</param>
        /// <param name="length">The total length in bytes of the uncompressed channel data.</param>
        /// <param name="flags">The flags of the virtual channel to indicate some compression feature.</param>
        /// <param name="virtualChannelData">The binary data to be send as virtual data. This argument can be null.
        /// If it is null, then the content field will be null.</param>
        /// <returns>Virtual Channel Raw PDU.</returns>
        public Virtual_Channel_RAW_Server_Pdu CreateRawVirtualChannelPdu(
            RdpbcgrServerSessionContext sessionContext,
            UInt16 channelId,
            uint length,
            CHANNEL_PDU_HEADER_flags_Values flags,
            byte[] virtualChannelData)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Virtual_Channel_RAW_Server_Pdu channelPdu = new Virtual_Channel_RAW_Server_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(ref channelPdu.commonHeader,
                                     defaultSecurityHeaderFlag,
                                     sessionContext,
                                     channelId);
            channelPdu.virtualChannelData = virtualChannelData;
            channelPdu.channelPduHeader.flags = flags;
            channelPdu.channelPduHeader.length = length;

            return channelPdu;
        }


        /// <summary>
        /// Create an array of Virtual Channel Raw PDU. The argument virtualChannelData will be split into 
        /// several PDUs according to chunk size negotiated before and compressed before sending.
        /// 
        /// The default fields value of the PDU is determined by previous messages.
        /// User can set special value in the PDU other than the default after calling this method.
        /// 
        /// Then call SendPdu to send the packet.
        /// This method should be called after performing connection.
        /// </summary>
        /// <param name="channelId">The channel Id of the virtual channel. 
        /// If the channel id is invalid, then the return value is null.</param>
        /// <param name="virtualChannelData">The virtual channel data to be splitted. This argument can be null.
        /// If it is null, then no Virtual Channel Raw PDU will be generated.</param>
        /// <returns>The split Virtual Channel Raw PDUs.</returns>
        public Virtual_Channel_Complete_Server_Pdu CreateCompleteVirtualChannelPdu(
            RdpbcgrServerSessionContext sessionContext,
            UInt16 channelId,
            byte[] virtualChannelData)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Virtual_Channel_Complete_Server_Pdu completePdu = new Virtual_Channel_Complete_Server_Pdu(sessionContext);
            completePdu.channelId = channelId;
            completePdu.virtualChannelData = virtualChannelData;
            completePdu.SplitToChunks();

            return completePdu;
        }
        #endregion 


        #region Server redirection
        /// <summary>
        /// Create standard redirection PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="redirectionPacket">The redirection info in the PDU.</param>
        /// <returns>Standard redirection PDU.</returns>
        public Server_Redirection_Pdu CreateStandardRedirectionPdu(
            RdpbcgrServerSessionContext sessionContext,
            RDP_SERVER_REDIRECTION_PACKET redirectionPacket)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Server_Redirection_Pdu redirectionPdu = new Server_Redirection_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeaderForServerRedirectionPDU(sessionContext, ref redirectionPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_REDIRECTION_PKT);

            redirectionPdu.serverRedirectionPdu = redirectionPacket;

            return redirectionPdu;
        }


        /// <summary>
        /// Create enhanced redirection PDU.
        /// </summary>
        /// <param name="sessionContext">The session context creating the PDU.</param>
        /// <param name="redirectionPacket">The redirection info in the PDU.</param>
        /// <returns>Enhanced redirection PDU.</returns>
        public Enhanced_Security_Server_Redirection_Pdu CreateEnhancedRedirectionPdu(
            RdpbcgrServerSessionContext sessionContext,
            RDP_SERVER_REDIRECTION_PACKET redirectionPacket)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            Enhanced_Security_Server_Redirection_Pdu redirectionPdu = new Enhanced_Security_Server_Redirection_Pdu(sessionContext);
            RdpbcgrUtility.FillCommonHeader(sessionContext, ref redirectionPdu.commonHeader,
                                     defaultSecurityHeaderFlag);
            redirectionPdu.pad = 0;
            redirectionPdu.pad1Octet = new byte[1];

            int totalLength = Marshal.SizeOf(redirectionPdu.shareControlHeader)
                + Marshal.SizeOf(redirectionPdu.pad)
                + redirectionPacket.Length
                + sizeof(byte);
            RdpbcgrUtility.FillShareControlHeader(ref redirectionPdu.shareControlHeader,
                                           (ushort)totalLength,
                                           ShareControlHeaderType.PDUTYPE_SERVER_REDIR_PKT,
                                           (ushort)sessionContext.UserChannelId);
            redirectionPdu.serverRedirectionPdu = redirectionPacket;

            return redirectionPdu;
        }
        #endregion

        #region Auto-Detect
        /// <summary>
        /// Create Server Auto-Detect Request PDU. The message can be used to detect network during connection phase.
        /// </summary>
        /// <param name="sessionContext"></param>
        /// <param name="autoDetectReqData"></param>
        /// <returns></returns>
        public Server_Auto_Detect_Request_PDU CreateServerAutoDetectRequestPDU(RdpbcgrServerSessionContext sessionContext, NETWORK_DETECTION_REQUEST autoDetectReqData)
        {
            Server_Auto_Detect_Request_PDU autoDetectRequestPdu = new Server_Auto_Detect_Request_PDU(sessionContext);
            RdpbcgrUtility.FillCommonHeader(ref autoDetectRequestPdu.commonHeader, defaultSecurityHeaderFlag | TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_REQ, sessionContext, (long)sessionContext.McsMsgChannelId);
            autoDetectRequestPdu.autoDetectReqData = autoDetectReqData;

            return autoDetectRequestPdu;
        }
        #endregion

        #region Optional Multitransport Bootstrapping

        /// <summary>
        /// Create a Server Initiate Multitransport Request PDU
        /// </summary>
        /// <param name="sessionContext"></param>
        /// <param name="requestId"></param>
        /// <param name="requestedProtocol"></param>
        /// <param name="securityCookie"></param>
        /// <returns></returns>
        public Server_Initiate_Multitransport_Request_PDU CreateServerInitiateMultitransportRequestPDU(RdpbcgrServerSessionContext sessionContext, uint requestId, Multitransport_Protocol_value requestedProtocol, byte[] securityCookie)
        {
            Server_Initiate_Multitransport_Request_PDU requestPDU = new Server_Initiate_Multitransport_Request_PDU(sessionContext);
            RdpbcgrUtility.FillCommonHeader(ref requestPDU.commonHeader, defaultSecurityHeaderFlag | TS_SECURITY_HEADER_flags_Values.SEC_TRANSPORT_REQ, sessionContext, (long)sessionContext.McsMsgChannelId);
            requestPDU.requestId = requestId;
            requestPDU.requestedProtocol = requestedProtocol;
            requestPDU.securityCookie = securityCookie;
            requestPDU.reserved = 0;

            return requestPDU;
        }

        #endregion Optional Multitransport Bootstrapping

        #region Connection Health Monitoring

        /// <summary>
        /// Create a Server Heartbeat PDU
        /// </summary>
        /// <param name="sessionContext"></param>
        /// <param name="period"></param>
        /// <param name="warningCount"></param>
        /// <param name="reconnectCount"></param>
        /// <returns></returns>
        public Server_Heartbeat_PDU CreateServerHeartbeatPDU(RdpbcgrServerSessionContext sessionContext, byte period, byte warningCount, byte reconnectCount)
        {
            Server_Heartbeat_PDU heartbeatPDU = new Server_Heartbeat_PDU(sessionContext);
            RdpbcgrUtility.FillCommonHeader(ref heartbeatPDU.commonHeader, defaultSecurityHeaderFlag | TS_SECURITY_HEADER_flags_Values.SEC_HEARTBEAT, sessionContext, (long)sessionContext.McsMsgChannelId);
            heartbeatPDU.reserved = 0;
            heartbeatPDU.period = period;
            heartbeatPDU.count1 = warningCount;
            heartbeatPDU.count2 = reconnectCount;

            return heartbeatPDU;
        }

        #endregion Connection Health Monitoring

        #endregion


        #region Connection sequence
        /// <summary>
        /// Expect a RDPBCGR connection sequence, including connection initiation, basic exchange, channel connection, 
        /// RDP security commencement, security settings exchange, licensing, capabilities exchange and connection finalization.
        /// Support both Standard RDP Security and Enhanced Security.
        /// </summary>
        /// <param name="sessionContext">The session context server uses to communicate with client.</param>
        /// <param name="timeout">Time for completing the connection.</param>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when the session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public void ExpectConnectSequence(RdpbcgrServerSessionContext sessionContext, TimeSpan timeout)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            #region default parameters

            selectedProtocols_Values selectedProtocols;
            if (encryptedProtocol == EncryptedProtocol.NegotiationTls)
            {
                selectedProtocols = selectedProtocols_Values.PROTOCOL_SSL_FLAG;
            }
            else if (encryptedProtocol == EncryptedProtocol.DirectCredSsp
                || encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                selectedProtocols = selectedProtocols_Values.PROTOCOL_HYBRID_FLAG
                                 | selectedProtocols_Values.PROTOCOL_SSL_FLAG;
            }
            else
            {
                selectedProtocols = selectedProtocols_Values.PROTOCOL_RDP_FLAG;
            }

            EncryptionMethods encryptionMethod = EncryptionMethods.ENCRYPTION_METHOD_128BIT;

            EncryptionLevel encyptionLevel = EncryptionLevel.ENCRYPTION_LEVEL_LOW;

            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;
            #endregion 

            #region Connection initiation
            Client_X_224_Connection_Request_Pdu x224ConnectRequestPdu =
                (Client_X_224_Connection_Request_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_X_224_Connection_Confirm_Pdu x224ConnectConfirmPdu =
                CreateX224ConnectionConfirmPdu(sessionContext, selectedProtocols);
            SendPdu(sessionContext, x224ConnectConfirmPdu);
            #endregion

            #region Basic settings exchange
            Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request mcsInitialPdu =
                (Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request)
                ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response mcsConnectRspPdu =
                CreateMCSConnectResponsePduWithGCCConferenceCreateResponsePdu(
                    sessionContext, encryptionMethod, encyptionLevel, GenerateCertificate(), 376);  //8888
            SendPdu(sessionContext, mcsConnectRspPdu);
            #endregion 

            #region Channel connection
            Client_MCS_Erect_Domain_Request erectDomainPdu =
                (Client_MCS_Erect_Domain_Request)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            Client_MCS_Attach_User_Request attachUserRequestPdu =
                (Client_MCS_Attach_User_Request)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            Server_MCS_Attach_User_Confirm_Pdu attachConfirmPdu =
                CreateMCSAttachUserConfirmPdu(sessionContext);
            SendPdu(sessionContext, attachConfirmPdu);
            Client_MCS_Channel_Join_Request userChannelJoin =
    (Client_MCS_Channel_Join_Request)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            Server_MCS_Channel_Join_Confirm_Pdu joinConfirm =
                CreateMCSChannelJoinConfirmPdu(sessionContext, sessionContext.UserChannelId);
            SendPdu(sessionContext, joinConfirm);
            userChannelJoin = (Client_MCS_Channel_Join_Request)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
            joinConfirm = CreateMCSChannelJoinConfirmPdu(sessionContext, sessionContext.IOChannelId);
            SendPdu(sessionContext, joinConfirm);

            List<ushort> idList = new List<ushort>();
            if (sessionContext.VirtualChannelIdStore != null)
            {
                idList.AddRange(sessionContext.VirtualChannelIdStore);
                for (int i = 0; i < sessionContext.VirtualChannelIdStore.Length; ++i)
                {
                    Client_MCS_Channel_Join_Request mcsChannelJoin =
                        (Client_MCS_Channel_Join_Request)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);
                    if (idList.Contains((ushort)mcsChannelJoin.mcsChannelId))
                    {
                        joinConfirm = CreateMCSChannelJoinConfirmPdu(sessionContext, mcsChannelJoin.mcsChannelId);
                        SendPdu(sessionContext, joinConfirm);
                    }
                    else
                    {
                        throw new ArgumentException("The virtual channel id is not negotiated");
                    }
                }
            }
            #endregion
        }


        /// <summary>
        /// Expect a new RdpbcgrConnect again with user reconnect cookie. 
        /// This method only performs RDPBCGR messages transport.
        /// Closing or reconnecting TCP connection is not included in this method.
        /// This method should be called after a normal connection .
        /// Please close the former connection before this call.
        /// </summary>
        /// <param name="sessionContext">The session context server uses to communicate with client.</param>
        /// <param name="timeout">Time for completing reconnection.</param>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when the session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public void ExpectReconnectSequence(out RdpbcgrServerSessionContext sessionContext, TimeSpan timeout)
        {
            #region waiting for rdp connection
            EncryptionMethods encryptionMethod = EncryptionMethods.ENCRYPTION_METHOD_NONE;
            EncryptionLevel encryptionLevel = EncryptionLevel.ENCRYPTION_LEVEL_NONE;
            RdpbcgrServerPdu response;
            sessionContext = ExpectConnect(timeout);
            #endregion

            #region connection initiation
            //expect x.224 request from client
            StackPacket rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_X_224_Connection_Request_Pdu))
            {
                throw new InvalidCastException();
            }

            //send response
            response = CreateX224ConnectionConfirmPdu(
                sessionContext,
                selectedProtocols_Values.PROTOCOL_SSL_FLAG);
            SendPdu(sessionContext, response);
            #endregion

            #region basic setting exchange
            //expect MCS connect initial Pdu
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request))
            {
                throw new InvalidCastException();
            }

            //send response
            response =
                CreateMCSConnectResponsePduWithGCCConferenceCreateResponsePdu(
                sessionContext,
                encryptionMethod,
                encryptionLevel,
                null,
                0);
            SendPdu(sessionContext, response);
            #endregion

            #region channel connection
            //expect MCS Erect Domain Request Pdu
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_MCS_Erect_Domain_Request))
            {
                throw new InvalidCastException();
            }

            //expect Attach User Request Pdu
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_MCS_Attach_User_Request))
            {
                throw new InvalidCastException();
            }

            //send response
            response = CreateMCSAttachUserConfirmPdu(sessionContext);
            SendPdu(sessionContext, response);
            #endregion

            #region channel join connection
            int channelNum = 2;
            if (sessionContext.VirtualChannelIdStore != null)
            {
                channelNum += sessionContext.VirtualChannelIdStore.Length;
            }
            for (int i = 0; i < channelNum; i++)
            {
                while (true)
                {
                    rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
                    if (rdpbcgrPacket is Client_MCS_Channel_Join_Request)
                    {
                        break;
                    }
                }
                Client_MCS_Channel_Join_Request channelJoinRequest = rdpbcgrPacket as Client_MCS_Channel_Join_Request;
                response = CreateMCSChannelJoinConfirmPdu(
                    sessionContext,
                    channelJoinRequest.mcsChannelId);
                SendPdu(sessionContext, response);
            }
            #endregion

            #region security setting exchange
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_Info_Pdu))
            {
                throw new InvalidCastException();
            }
            #endregion

            #region licensing
            Server_License_Error_Pdu_Valid_Client licenseErrorPdu = CreateLicenseErrorMessage(
                sessionContext);
            SendPdu(sessionContext, licenseErrorPdu);
            #endregion

            #region capabilities exchange
            //send Demand Active Pdu to client
            Server_Demand_Active_Pdu demandActivePdu = CreateDemandActivePdu(sessionContext, null);
            SendPdu(sessionContext, demandActivePdu);

            //expect Confirm Active Pdu
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_Confirm_Active_Pdu))
            {
                throw new InvalidCastException();
            }
            #endregion

            #region connection finalization
            //expect Synchronize Pdu
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_Synchronize_Pdu))
            {
                throw new InvalidCastException();
            }

            //expect Control Pdu - Cooperate
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_Control_Pdu_Cooperate))
            {
                throw new InvalidCastException();
            }

            //expect Control Pdu - Request Control
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);
            if (!(rdpbcgrPacket is Client_Control_Pdu_Request_Control))
            {
                throw new InvalidCastException();
            }

            //expect Font List Pdu
            rdpbcgrPacket = ExpectPdu(sessionContext, timeout);

            //send Synchronize Pdu
            Server_Synchronize_Pdu synchronizePdu = CreateSynchronizePdu(sessionContext);
            SendPdu(sessionContext, synchronizePdu);

            //send Control Pdu - Cooperate
            Server_Control_Pdu controlCooperatePdu = CreateControlCooperatePdu(
                sessionContext);
            SendPdu(sessionContext, controlCooperatePdu);

            //send Control Pdu - Granted Control
            Server_Control_Pdu controlGrantedPdu = CreateControlGrantedPdu(
                sessionContext);
            SendPdu(sessionContext, controlGrantedPdu);

            //send Font Map Pdu
            Server_Font_Map_Pdu fontMapPdu = CreateFontMapPdu(sessionContext);
            SendPdu(sessionContext, fontMapPdu);
            #endregion
        }


        /// <summary>
        /// This method is called after sending Deactivate All PDU, then expect reactivate.
        /// This method should be called after calling method Connect.
        /// </summary>
        /// <param name="sessionContext">The session context server uses to communicate with client.</param>
        /// <param name="timeout">Time for completing reactivation.</param>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when the session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public void ExpectReactivateSequence(RdpbcgrServerSessionContext sessionContext, TimeSpan timeout)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            TimeSpan totalTime = timeout + DateTime.Now.TimeOfDay;

            #region capabilities exchange

            Server_Demand_Active_Pdu demandActivePdu = CreateDemandActivePdu(sessionContext);
            SendPdu(sessionContext, demandActivePdu);
            Client_Confirm_Active_Pdu confirmActivePdu =
                (Client_Confirm_Active_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            #endregion capabilities exchange

            #region connection finalization

            Client_Synchronize_Pdu clientSynchrnizePdu =
                (Client_Synchronize_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Client_Control_Pdu_Cooperate controlCoopPdu =
                (Client_Control_Pdu_Cooperate)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Client_Control_Pdu_Request_Control controlRequest =
                (Client_Control_Pdu_Request_Control)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Client_Font_List_Pdu fontListPdu =
                (Client_Font_List_Pdu)ExpectPdu(totalTime - DateTime.Now.TimeOfDay);

            Server_Synchronize_Pdu serverSynchronizePdu = CreateSynchronizePdu(sessionContext);
            SendPdu(sessionContext, serverSynchronizePdu);

            Server_Control_Pdu serverControlCoopPdu = CreateControlCooperatePdu(sessionContext);
            SendPdu(sessionContext, serverControlCoopPdu);

            Server_Control_Pdu serverControlGrantedPdu = CreateControlGrantedPdu(sessionContext);
            SendPdu(sessionContext, serverControlCoopPdu);

            Server_Font_Map_Pdu serverFontPdu = CreateFontMapPdu(sessionContext);
            SendPdu(sessionContext, serverControlCoopPdu);

            #endregion connection finalization
        }


        /// <summary>
        /// Initiate a server-side disconnect by closing the RDP application.
        /// After this method, user should call Disconnect to close the TCP connection.
        /// This method only supports "If a logged-on user account is associated with the session".
        /// </summary>
        /// <param name="sessionContext">The session context server uses to communicate with client.</param>
        /// <param name="isLogon">Whether there's a user loging on</param>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <exception cref="InvalidOperationException">
        /// An error occurred when the session has not been established.</exception>
        /// <exception cref="TimeoutException">An error occurred when time out.</exception>
        public void ExpectDisconnectSequence(
            RdpbcgrServerSessionContext sessionContext,
            bool isLogon,
            TimeSpan timeout)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            StackPacket rdpbcgrPacket;

            while (true)
            {
                rdpbcgrPacket = ExpectPdu(sessionContext, timeout);

                if (rdpbcgrPacket == null)
                {
                    throw new InvalidOperationException();
                }

                if (rdpbcgrPacket is Client_Shutdown_Request_Pdu)
                {
                    break;
                }
            }

            //response shutdown denied
            Server_Shutdown_Request_Denied_Pdu shutdownDeniedPdu =
                CreateShutdownRequestDeniedPdu(sessionContext);
            SendPdu(sessionContext, shutdownDeniedPdu);

            //expect client MCS Disconnect Provider Ultimatum Pdu
            while (true)
            {
                rdpbcgrPacket = ExpectPdu(sessionContext, timeout);

                if (rdpbcgrPacket == null)
                {
                    throw new InvalidOperationException();
                }

                if (rdpbcgrPacket is MCS_Disconnect_Provider_Ultimatum_Pdu)
                {
                    break;
                }
            }
        }

        #endregion

        #region SVC Manager control

        /// <summary>
        /// Start Static virtual channel manager of a specific RDP session
        /// After started, this manager will process SVC data automatically, 
        /// and transfer SVC data to high level protocols by using Received event
        /// </summary>
        /// <param name="sessionContext">Session context</param>
        public void StartSVCManager(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext.SVCManager != null)
            {
                if (!sessionContext.SVCManager.IsRunning)
                {
                    sessionContext.SVCManager.Start();
                }
            }
        }

        /// <summary>
        /// Stop Static virtual channel manager of a specific RDP session
        /// </summary>
        /// <param name="sessionContext">Session context</param>
        public void StopSVCManager(RdpbcgrServerSessionContext sessionContext)
        {
            if (sessionContext.SVCManager != null)
            {
                if (sessionContext.SVCManager.IsRunning)
                {
                    sessionContext.SVCManager.Stop();
                }
            }
        }

        #endregion SVC Manager control

        #region other

        /// <summary>
        /// Parse a byte array to a NETWORK_DETECTION_RESPONSE structure
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public NETWORK_DETECTION_RESPONSE ParseNetworkDetectionResponse(byte[] data, bool isSubHeader = false)
        {
            RdpbcgrServerDecoder decoder = new RdpbcgrServerDecoder(this);
            int currentIndex = 0;
            return decoder.ParseNetworkDetectionResponse(data, ref currentIndex, isSubHeader);
        }

        /// <summary>
        /// Encode a NETWORK_DETECTION_REQUEST to byte array
        /// </summary>
        /// <param name="networkDetectionRequest"></param>
        /// <param name="isSubHeader"></param>
        /// <returns></returns>
        public byte[] EncodeNetworkDetectionRequest(NETWORK_DETECTION_REQUEST networkDetectionRequest, bool isSubHeader = false)
        {
            List<byte> reqDataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeNetworkDetectionRequest(reqDataBuffer, networkDetectionRequest, isSubHeader);
            return RdpbcgrUtility.ToBytes(reqDataBuffer);
        }

        #endregion

        #region RDSTLS
        public RDSTLS_CapabilitiesPDU CreateRDSTLSCapabilityPDU(RdpbcgrServerSessionContext sessionContext)
        {
            var pdu = new RDSTLS_CapabilitiesPDU();

            // fill common header
            var header = new RDSTLS_CommonHeader();
            header.Version = RDSTLS_VersionEnum.RDSTLS_VERSION_1;
            header.PduType = RDSTLS_PduTypeEnum.RDSTLS_TYPE_CAPABILITIES;
            header.DataType = RDSTLS_DataTypeEnum.RDSTLS_DATA_CAPABILITIES;
            pdu.Header = header;

            pdu.SupportedVersions = RDSTLS_VersionEnum.RDSTLS_VERSION_1;

            return pdu;
        }

        public RDSTLS_AuthenticationResponsePDU CreateRDSTLSAuthenticationResponsePDU(RdpbcgrServerSessionContext sessionContext)
        {
            var pdu = new RDSTLS_AuthenticationResponsePDU();

            // fill common header
            var header = new RDSTLS_CommonHeader();
            header.Version = RDSTLS_VersionEnum.RDSTLS_VERSION_1;
            header.PduType = RDSTLS_PduTypeEnum.RDSTLS_TYPE_AUTHRSP;
            header.DataType = RDSTLS_DataTypeEnum.RDSTLS_DATA_RESULT_CODE;
            pdu.Header = header;

            pdu.ResultCode = RDSTLS_ResultCodeEnum.RDSTLS_RESULT_SUCCESS;

            return pdu;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Expect to receive a non virtual channel PDU.
        /// </summary>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <returns>The expected PDU.</returns>
        private StackPacket ExpectPdu(TimeSpan timeout)
        {
            if (timeout.TotalMilliseconds < 0)
            {
                return null;
            }

            TransportEvent eventPacket = null;
            StackPacket pdu = null;

            eventPacket = ExpectTransportEvent(timeout);
            pdu = (StackPacket)eventPacket.EventObject;

            return pdu;
        }


        /// <summary>
        /// Use transport stack according security mechanism to expect transport event.
        /// </summary>
        /// <param name="timeout">Time for expecting transport event.</param>
        /// <returns></returns>
        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            if (timeout.TotalMilliseconds < 0)
            {
                return null;
            }

            TransportEvent transportEvent = null;
            if (this.EncryptedProtocol == EncryptedProtocol.DirectCredSsp ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationTls ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                transportEvent = this.directedTransportStack.ExpectTransportEvent(timeout);
            }
            else
            {
                transportEvent = this.transportStack.ExpectTransportEvent(timeout);
            }

            return transportEvent;
        }

        /// <summary>
        /// Create a new session with the specified TransportEvent
        /// </summary>
        /// <param name="transEvt">The specified TransportEvent</param>
        /// <returns>New session instance</returns>
        public RdpbcgrServerSessionContext NewSession(TransportEvent transEvt)
        {
            if (this.encryptedProtocol == Rdpbcgr.EncryptedProtocol.Rdp)
            {
                RdpbcgrServerSessionContext session = new RdpbcgrServerSessionContext();
                session.Identity = transEvt.EndPoint;
                session.LocalIdentity = transEvt.LocalEndPoint;
                session.Server = this;
                session.IsClientToServerEncrypted = this.IsClientToServerEncrypted;
                this.serverContext.AddSession(session);
                return session;
            }
            else if (transEvt.EventType == EventType.Connected)
            {
                RdpbcgrServerSessionContext session = this.serverContext.LookupSession(transEvt.EndPoint);
                if (session != null)
                {
                    return session;
                }
            }
            return null;
        }

        /// <summary>
        /// Create default capability sets.
        /// User can add, remove or update the capability sets with the return value.
        /// </summary>
        /// <returns>The capability sets created.</returns>
        public Collection<ITsCapsSet> CreateCapabilitySets()
        {
            Collection<ITsCapsSet> capabilitySets = new Collection<ITsCapsSet>();

            #region fill capabilities

            #region Populating Share Capability Set
            TS_SHARE_CAPABILITYSET shareCapabilitySet = new TS_SHARE_CAPABILITYSET();
            shareCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_SHARE;
            shareCapabilitySet.nodeId = 1002;
            shareCapabilitySet.pad2octets = 0;
            shareCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(shareCapabilitySet);

            capabilitySets.Add(shareCapabilitySet);
            #endregion Populating Share Capability Set

            #region Populating general Capability Set
            TS_GENERAL_CAPABILITYSET generalCapabilitySet = new TS_GENERAL_CAPABILITYSET();
            generalCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_GENERAL;
            generalCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(generalCapabilitySet);
            generalCapabilitySet.osMajorType = osMajorType_Values.OSMAJORTYPE_WINDOWS;
            generalCapabilitySet.osMinorType = osMinorType_Values.OSMINORTYPE_WINDOWS_NT;
            generalCapabilitySet.protocolVersion = protocolVersion_Values.V1;
            generalCapabilitySet.pad2octetsA = 0;
            generalCapabilitySet.generalCompressionTypes = generalCompressionTypes_Values.V1;
            generalCapabilitySet.extraFlags = extraFlags_Values.NO_BITMAP_COMPRESSION_HDR
                                            | extraFlags_Values.ENC_SALTED_CHECKSUM
                                            | extraFlags_Values.AUTORECONNECT_SUPPORTED
                                            | extraFlags_Values.LONG_CREDENTIALS_SUPPORTED
                                            | extraFlags_Values.FASTPATH_OUTPUT_SUPPORTED;
            generalCapabilitySet.updateCapabilityFlag = updateCapabilityFlag_Values.V1;
            generalCapabilitySet.remoteUnshareFlag = remoteUnshareFlag_Values.V1;
            generalCapabilitySet.generalCompressionLevel = generalCompressionLevel_Values.V1;
            generalCapabilitySet.refreshRectSupport = refreshRectSupport_Values.TRUE;
            generalCapabilitySet.suppressOutputSupport = suppressOutputSupport_Values.TRUE;
            generalCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(generalCapabilitySet);

            capabilitySets.Add(generalCapabilitySet);
            #endregion  Populating general Capability Set

            #region Populating Virtual Channel Set
            TS_VIRTUALCHANNEL_CAPABILITYSET virtualChannelSet = new TS_VIRTUALCHANNEL_CAPABILITYSET();
            virtualChannelSet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL;
            virtualChannelSet.flags = TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values.VCCAPS_COMPR_CS_8K;
            virtualChannelSet.VCChunkSize = 1600;
            virtualChannelSet.lengthCapability = 12;

            capabilitySets.Add(virtualChannelSet);
            #endregion

            #region Populating Draw Grid Plus Set
            TS_DRAWGRIDPLUS_CAPABILITYSET drawSet = new TS_DRAWGRIDPLUS_CAPABILITYSET();
            drawSet.rawData = new byte[]
            {
                22, 0, 40, 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0,
                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            capabilitySets.Add(drawSet);
            #endregion

            #region Populating Font Capability Set
            TS_FONT_CAPABILITYSET fontCapabilitySet = new TS_FONT_CAPABILITYSET();
            fontCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_FONT;
            fontCapabilitySet.fontSupportFlags = ConstValue.FONTSUPPORT_FONTLIST;
            fontCapabilitySet.pad2octets = 0;
            fontCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(fontCapabilitySet);

            capabilitySets.Add(fontCapabilitySet);
            #endregion Populating Font Capability Set

            #region Populating Bitmap Capability Set
            TS_BITMAP_CAPABILITYSET bitmapCapabilitySet = new TS_BITMAP_CAPABILITYSET();
            bitmapCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_BITMAP;
            bitmapCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(bitmapCapabilitySet);
            bitmapCapabilitySet.preferredBitsPerPixel = 16;
            bitmapCapabilitySet.receive1BitPerPixel = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.receive4BitsPerPixel = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.receive8BitsPerPixel = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.desktopWidth = ConstValue.DESKTOP_WIDTH_DEFAULT;
            bitmapCapabilitySet.desktopHeight = ConstValue.DESKTOP_HEIGHT_DEFAULT;
            bitmapCapabilitySet.pad2octets = 0;
            bitmapCapabilitySet.desktopResizeFlag = desktopResizeFlag_Values.TRUE;
            bitmapCapabilitySet.bitmapCompressionFlag = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.highColorFlags = 0;
            bitmapCapabilitySet.drawingFlags = drawingFlags_Values.DRAW_ALLOW_COLOR_SUBSAMPLING
                                             | drawingFlags_Values.DRAW_ALLOW_DYNAMIC_COLOR_FIDELITY
                                             | drawingFlags_Values.DRAW_ALLOW_SKIP_ALPHA;
            bitmapCapabilitySet.multipleRectangleSupport = ConstValue.BITMAP_CAP_SUPPORT_FEATURE;
            bitmapCapabilitySet.pad2octetsB = 0;

            capabilitySets.Add(bitmapCapabilitySet);
            #endregion Populating Bitmap Capability Set

            #region Populating Order Capability Set
            TS_ORDER_CAPABILITYSET orderCapabilitySet = new TS_ORDER_CAPABILITYSET();
            orderCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_ORDER;
            orderCapabilitySet.terminalDescriptor = ConstValue.TERMINALDESCRIPTOR;
            orderCapabilitySet.pad4octetsA = 1000000;
            orderCapabilitySet.desktopSaveXGranularity = ConstValue.ORDER_CAP_DESKTOP_X;
            orderCapabilitySet.desktopSaveYGranularity = ConstValue.ORDER_CAP_DESKTOP_Y;
            orderCapabilitySet.pad2octetsA = 0;
            orderCapabilitySet.maximumOrderLevel = ConstValue.ORD_LEVEL_1_ORDERS;
            orderCapabilitySet.numberFonts = 0;
            orderCapabilitySet.orderFlags = (orderFlags_Values)170;
            orderCapabilitySet.orderSupport =
                new byte[] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x01,
                         0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
                         0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00,
                         0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00};
            orderCapabilitySet.textFlags = 1697;
            orderCapabilitySet.orderSupportExFlags =
                orderSupportExFlags_values.ORDERFLAGS_EX_CACHE_BITMAP_REV3_SUPPORT
                | orderSupportExFlags_values.ORDERFLAGS_EX_ALTSEC_FRAME_MARKER_SUPPORT;
            orderCapabilitySet.pad4octetsB = 1000000;
            orderCapabilitySet.desktopSaveSize = 1000000;
            orderCapabilitySet.pad2octetsC = 1;
            orderCapabilitySet.pad2octetsD = 0;
            orderCapabilitySet.textANSICodePage = 0;
            orderCapabilitySet.pad2octetsE = 0;
            orderCapabilitySet.lengthCapability = (ushort)(sizeof(ushort) * ConstValue.ORDER_CAP_USHORT_COUNT
                                                + sizeof(uint) * ConstValue.ORDER_CAP_UINT_COUNT
                                                + orderCapabilitySet.terminalDescriptor.Length
                                                + orderCapabilitySet.orderSupport.Length);

            capabilitySets.Add(orderCapabilitySet);
            #endregion Populating Order Capability Set

            #region Color Cache Set
            TS_COLORCACHE_CAPABILITYSET colorSet = new TS_COLORCACHE_CAPABILITYSET();
            colorSet.rawData = new byte[] { 10, 0, 8, 0, 6, 0, 0, 0 };

            capabilitySets.Add(colorSet);
            #endregion

            #region Populating Bitmap Cache Host Support Capability Set
            TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET bitmapHostsupprot =
                new TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET();
            bitmapHostsupprot.capabilitySetType = capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_HOSTSUPPORT;
            bitmapHostsupprot.cacheVersion = cacheVersion_Values.V1;
            bitmapHostsupprot.pad1 = 0;
            bitmapHostsupprot.pad2 = 0;
            bitmapHostsupprot.lengthCapability = (ushort)Marshal.SizeOf(bitmapHostsupprot);

            capabilitySets.Add(bitmapHostsupprot);
            #endregion Populating Bitmap Cache Host Support Capability Set

            #region Populating Pointer Capability Set
            TS_POINTER_CAPABILITYSET pointerCapabilitySet = new TS_POINTER_CAPABILITYSET();
            pointerCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_POINTER;
            pointerCapabilitySet.colorPointerFlag = colorPointerFlag_Values.TRUE;
            pointerCapabilitySet.colorPointerCacheSize = 25;
            pointerCapabilitySet.pointerCacheSize = 25;
            pointerCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(pointerCapabilitySet);

            capabilitySets.Add(pointerCapabilitySet);
            #endregion Populating Pointer Capability Set

            #region Populating Input Capability Set
            TS_INPUT_CAPABILITYSET inputCapabilitySet = new TS_INPUT_CAPABILITYSET();
            inputCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSTYPE_INPUT;
            inputCapabilitySet.inputFlags = (inputFlags_Values)117;
            inputCapabilitySet.pad2octetsA = 0;
            inputCapabilitySet.keyboardLayout = 0;
            inputCapabilitySet.keyboardType = TS_INPUT_CAPABILITYSET_keyboardType_Values.None;
            inputCapabilitySet.keyboardSubType = 0;
            inputCapabilitySet.keyboardFunctionKey = 0;
            byte[] arr = new byte[64];
            inputCapabilitySet.imeFileName = "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-" +
                    "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-" +
                    "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-" +
                    "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00";
            inputCapabilitySet.lengthCapability = (ushort)(24 + ConstValue.INPUT_CAP_IME_FILENAME_SIZE); // the other fields(except imeFileName field) totoal length is 24

            capabilitySets.Add(inputCapabilitySet);
            #endregion Populating Input Capability Set

            #region TS Rail Set
            TS_RAIL_CAPABILITYSET railSet = new TS_RAIL_CAPABILITYSET();
            railSet.rawData = new byte[] { 23, 0, 8, 0, 3, 0, 0, 0 };

            capabilitySets.Add(railSet);
            #endregion

            #region Ts Window Set
            TS_WINDOW_CAPABILITYSET winSet = new TS_WINDOW_CAPABILITYSET();
            winSet.rawData = new byte[] { 24, 0, 11, 0, 2, 0, 0, 0, 3, 12, 0 };

            capabilitySets.Add(winSet);
            #endregion

            #region Populating Desktop Composition Capability Set
            TS_COMPDESK_CAPABILITYSET desktopCapabilitySet = new TS_COMPDESK_CAPABILITYSET();
            desktopCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_COMPDESK;
            desktopCapabilitySet.CompDeskSupportLevel = CompDeskSupportLevel_Values.COMPDESK_SUPPORTED;
            desktopCapabilitySet.lengthCapability = sizeof(ushort) + sizeof(ushort) + sizeof(ushort);

            capabilitySets.Add(desktopCapabilitySet);
            #endregion Populating Desktop Composition Capability Set

            #region Populating Multifragment Update Capability Set
            TS_MULTIFRAGMENTUPDATE_CAPABILITYSET multiFragmentCapabilitySet =
                new TS_MULTIFRAGMENTUPDATE_CAPABILITYSET();
            multiFragmentCapabilitySet.capabilitySetType = capabilitySetType_Values.CAPSETTYPE_MULTIFRAGMENTUPDATE;
            multiFragmentCapabilitySet.MaxRequestSize = 20488;
            multiFragmentCapabilitySet.lengthCapability = (ushort)Marshal.SizeOf(multiFragmentCapabilitySet);

            capabilitySets.Add(multiFragmentCapabilitySet);
            #endregion Populating Multifragment Update Capability Set

            #endregion 

            return capabilitySets;
        }


        /// <summary>
        /// Update session key according to section 5.3.7 Session Key Updates.
        /// </summary>
        /// /// <exception cref="InvalidOperationException">
        /// An error occurred when the session has not been established.</exception>
        private void UpdateSessionKey(RdpbcgrServerSessionContext sessionContext)
        {
            sessionContext.UpdateSessionKey();

            updateSessionKeyEvent.Set();
        }


        /// <summary>
        /// Check whether the number of the encrypted packets is equal to PduCountofUpdateSessionKey.
        /// If so, generate an UpdateSessionKeyPdu.
        /// </summary>
        private void CheckEncryptionCount(RdpbcgrServerSessionContext sessionContext)
        {
            // If the pdu encryption count have reached the count to update session key,
            // then put message UpdateSessionKeyPdu to queue.
            if (sessionContext.EncryptionCount == sessionContext.PduCountofUpdateSessionKey)
            {
                UpdateSessionKeyPdu pdu = new UpdateSessionKeyPdu();
                TransportEvent packetEvent = new TransportEvent(EventType.ReceivedPacket, sessionContext.Identity, pdu);

                if (this.EncryptedProtocol == EncryptedProtocol.DirectCredSsp ||
                    this.encryptedProtocol == EncryptedProtocol.NegotiationTls ||
                    this.encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
                {
                    this.directedTransportStack.AddEvent(packetEvent);
                }
                else
                {
                    this.transportStack.AddEvent(packetEvent);
                }
            }
        }


        /// <summary>
        /// Check whether the number of the decrypted packets is equal to PduCountofUpdateSessionKey.
        /// If so, generate an UpdateSessionKeyPdu.
        /// </summary>
        internal void CheckDecryptionCount(RdpbcgrServerSessionContext sessionContext)
        {
            // If the pdu decryption count reached the count to update session key,
            // then put message UpdateSessionKeyPdu to queue.
            if (sessionContext.DecryptionCount == sessionContext.PduCountofUpdateSessionKey)
            {
                UpdateSessionKeyPdu pdu = new UpdateSessionKeyPdu();
                TransportEvent packetEvent = new TransportEvent(EventType.ReceivedPacket, sessionContext.Identity, pdu);

                if (this.EncryptedProtocol == EncryptedProtocol.DirectCredSsp ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationTls ||
                this.encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
                {
                    this.directedTransportStack.AddEvent(packetEvent);
                }
                else
                {
                    this.transportStack.AddEvent(packetEvent);
                }

                // If the DecryptionCount increased to PduCountofUpdateSessionKey, that means
                // the received thread has to wait for user to trigger update session key event.
                if (updateSessionKeyEvent.WaitOne(ConstValue.UPDATE_SESSION_KEY_TIMEOUT, false))
                {
                    updateSessionKeyEvent.Reset();
                }
                else
                {
                    throw new TimeoutException("RdpbcgrServer:CheckDecryptionCount: failed to update session key");
                }
            }
        }

        /// <summary>
        /// Expect to receive a virtual channel PDU from the remote host from some specific session.
        /// </summary>
        /// <param name="timeout">Time for receiving PDU.</param>
        /// <param name="sessionContext">The client connection from which server receives PDU.</param>
        /// <returns>The expected PDU.</returns>
        private RdpbcgrClientPdu ExpectChannelPdu(TimeSpan timeout, out RdpbcgrServerSessionContext session)
        {
            RdpbcgrClientPdu pdu = null;
            DateTime startTime = DateTime.Now;

            while (true)
            {
                // The remain time to expect
                TimeSpan leftTime = timeout - (DateTime.Now - startTime);
                pdu = (RdpbcgrClientPdu)ExpectPdu(leftTime, out session);

                if (pdu is MCS_Disconnect_Provider_Ultimatum_Pdu
                    || pdu is ErrorPdu
                    || pdu is Virtual_Channel_RAW_Pdu)                       // some error occurs
                {
                    break;
                }
            }

            return pdu;
        }


        /// <summary>
        /// Generates a certificate.
        /// </summary>
        /// <returns>The certiface.</returns>
        public SERVER_CERTIFICATE GenerateCertificate()
        {
            PROPRIETARYSERVERCERTIFICATE proprietaryCert = new PROPRIETARYSERVERCERTIFICATE();
            proprietaryCert.dwKeyAlgId = dwKeyAlgId_Values.V1;
            proprietaryCert.dwSigAlgId = dwSigAlgId_Values.V1;
            proprietaryCert.wPublicKeyBlobType = wPublicKeyBlobType_Values.V1;
            proprietaryCert.wSignatureBlobType = wSignatureBlobType_Values.V1;
            RSA_PUBLIC_KEY publicKey = new RSA_PUBLIC_KEY();
            publicKey.magic = magic_Values.V1;
            publicKey.pubExp = 0x00010001; //65537
            publicKey.bitlen = 0x00000800; //2048
            publicKey.datalen = 0x000000ff; //255
            publicKey.keylen = 0x00000108; //264
            //264 bytes
            publicKey.modulus = new byte[] {
                0xDB, 0x29, 0x5E, 0x20, 0xCD, 0x3F, 0xA7, 0xC4, 0xD0, 0xD0,
                0x53, 0xAC, 0x02, 0xA3, 0x8F, 0x63, 0x63, 0x15, 0xAD, 0xDC,
                0x99, 0xC7, 0x26, 0xCC, 0x9E, 0xE9, 0x8B, 0x89, 0xF1, 0x8C,
                0xC5, 0xD1, 0x77, 0x19, 0x62, 0xB6, 0x48, 0xC8, 0x5A, 0xF4,
                0xC4, 0x01, 0xEF, 0xC2, 0x1B, 0x0A, 0x46, 0x87, 0x74, 0xCA,
                0x49, 0xAD, 0x12, 0xBC, 0x8E, 0xE3, 0x58, 0xA0, 0xE2, 0xA8,
                0xCC, 0xDC, 0x85, 0x16, 0x76, 0x30, 0xAA, 0x68, 0x80, 0x5A,
                0xBB, 0xB1, 0x6F, 0xC3, 0xC7, 0xFB, 0xB3, 0x02, 0x19, 0xEE,
                0x86, 0x0B, 0x79, 0xE4, 0xEF, 0x3A, 0x39, 0x94, 0xC1, 0x5F,
                0x79, 0x3B, 0xE9, 0x3A, 0xCA, 0x16, 0xD0, 0x3E, 0x06, 0x4F,
                0x51, 0xF9, 0xD8, 0x82, 0xA8, 0x2A, 0xE1, 0x30, 0x1F, 0x17,
                0xDF, 0x50, 0xDC, 0xFC, 0xC2, 0x02, 0xF8, 0x16, 0x04, 0xE5,
                0x39, 0x3D, 0xDC, 0x6E, 0xE0, 0x11, 0x4B, 0xD8, 0x8E, 0xC8,
                0x9D, 0xE3, 0x8B, 0x85, 0xC6, 0xD8, 0xE2, 0xDB, 0x50, 0xD6,
                0x5C, 0x31, 0x84, 0x86, 0xFA, 0x64, 0x0A, 0xB1, 0x0B, 0xBA,
                0xD9, 0x87, 0x05, 0x13, 0xAF, 0x9A, 0xBE, 0x8B, 0x7A, 0xDF,
                0x2F, 0x8B, 0xFA, 0xC9, 0xA6, 0xB4, 0xE6, 0x92, 0xE0, 0xDE,
                0x8F, 0x0A, 0x12, 0x1B, 0x93, 0x0D, 0xAB, 0xEB, 0xC3, 0x20,
                0x0B, 0x49, 0x25, 0x09, 0xBC, 0x98, 0x70, 0xBA, 0x6E, 0xED,
                0xF2, 0x69, 0x60, 0x49, 0x51, 0x01, 0xD8, 0xA4, 0x06, 0x71,
                0xC8, 0x4A, 0xEE, 0xF7, 0xF8, 0xED, 0x91, 0xCE, 0x06, 0xEA,
                0xC3, 0x7C, 0xAD, 0xF9, 0xE8, 0xE1, 0xD8, 0x36, 0xC9, 0xE4,
                0x67, 0x81, 0xA9, 0xCE, 0x48, 0xE9, 0x49, 0x73, 0xD6, 0x7B,
                0x06, 0xEB, 0x5F, 0x90, 0xBB, 0x22, 0xF0, 0x1B, 0x98, 0xA6,
                0xA1, 0x7A, 0xDB, 0x8A, 0x06, 0x4F, 0xB8, 0xD6, 0xEF, 0x2B,
                0x74, 0x1D, 0x31, 0x3C, 0xF6, 0xBC, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00};

            proprietaryCert.PublicKeyBlob = publicKey;
            proprietaryCert.wPublicKeyBlobLen = 0x011c; //284
            proprietaryCert.SignatureBlob = new byte[] {
                0xC3, 0x04, 0xCB, 0xC1, 0x81, 0x7B, 0x36, 0x34, 0x4A, 0x25,
                0x2B, 0xEC, 0x3A, 0x3E, 0xC8, 0x45, 0xE1, 0x4D, 0x1D, 0x3E,
                0x56, 0xDA, 0x3B, 0xA9, 0x2F, 0x59, 0x1E, 0xB5, 0x0F, 0x1D,
                0x7C, 0x73, 0x24, 0x40, 0x38, 0x82, 0x79, 0x14, 0x48, 0x5A,
                0xAF, 0xDB, 0xC0, 0x2C, 0x3E, 0xF7, 0xD0, 0x17, 0x79, 0x75,
                0xE0, 0xEF, 0x38, 0xF5, 0xC5, 0xC3, 0x2A, 0x73, 0x35, 0x3D,
                0x51, 0xD6, 0x7E, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00};
            proprietaryCert.wSignatureBlobLen = 0x0048; //72  

            //create server certificate from proprietary certificate 
            SERVER_CERTIFICATE serverCert = new SERVER_CERTIFICATE();
            serverCert.dwVersion = SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1;
            serverCert.certData = proprietaryCert;

            return serverCert;
        }

        /// <summary>
        /// Generates a certificate.
        /// </summary>
        /// <param name="dwkeySize">the size of the key to use in bits</param>
        /// <param name="updateToContext">if set the modulus, private exponent, public exponent to serverContext</param>
        /// <returns>The certiface.</returns>
        public SERVER_CERTIFICATE GenerateCertificate(int dwkeySize, out byte[] priviateExp, out byte[] publicExp, out byte[] modulus)
        {
            RSACryptoServiceProvider pk = new RSACryptoServiceProvider(dwkeySize);
            RSAParameters rsaParams = pk.ExportParameters(true);

            priviateExp = RdpbcgrUtility.CloneByteArray(rsaParams.D);
            Array.Reverse(priviateExp);
            publicExp = RdpbcgrUtility.CloneByteArray(rsaParams.Exponent);
            Array.Reverse(publicExp);
            if (publicExp.Length < 4)
            {
                Array.Resize<byte>(ref publicExp, 4);
            }
            modulus = RdpbcgrUtility.CloneByteArray(rsaParams.Modulus);
            Array.Reverse(modulus);

            PROPRIETARYSERVERCERTIFICATE proprietaryCert = new PROPRIETARYSERVERCERTIFICATE();
            proprietaryCert.dwKeyAlgId = dwKeyAlgId_Values.V1;
            proprietaryCert.dwSigAlgId = dwSigAlgId_Values.V1;
            proprietaryCert.wPublicKeyBlobType = wPublicKeyBlobType_Values.V1;
            proprietaryCert.wSignatureBlobType = wSignatureBlobType_Values.V1;
            RSA_PUBLIC_KEY publicKey = new RSA_PUBLIC_KEY();
            publicKey.magic = magic_Values.V1;

            //compute the public exponent to uint value 
            byte[] pubExp = RdpbcgrUtility.CloneByteArray(rsaParams.Exponent);
            uint pubExpValue = 0;
            for (int i = 0; i < pubExp.Length; i++)
            {
                pubExpValue = pubExpValue * 16 * 16 + pubExp[i];
            }
            publicKey.pubExp = pubExpValue;
            publicKey.modulus = RdpbcgrUtility.CloneByteArray(rsaParams.Modulus);
            Array.Reverse(publicKey.modulus);
            Array.Resize<byte>(ref publicKey.modulus, publicKey.modulus.Length + 8);
            publicKey.bitlen = (uint)(rsaParams.Modulus.Length * 8);
            publicKey.datalen = (uint)(rsaParams.Modulus.Length - 1);
            publicKey.keylen = (uint)(publicKey.modulus.Length); //264
                                                                 //264 bytes


            proprietaryCert.PublicKeyBlob = publicKey;
            proprietaryCert.wPublicKeyBlobLen = (ushort)(20 + publicKey.keylen);
            proprietaryCert.SignatureBlob = RdpbcgrUtility.SignProprietaryCertificate(proprietaryCert);
            Array.Resize<byte>(ref proprietaryCert.SignatureBlob, ConstValue.PROPRIETARY_CERTIFICATE_SIGNATURE_SIZE + 9); // 72
            proprietaryCert.wSignatureBlobLen = (ushort)(proprietaryCert.SignatureBlob.Length);

            //create server certificate from proprietary certificate 
            SERVER_CERTIFICATE serverCert = new SERVER_CERTIFICATE();
            serverCert.dwVersion = SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1;
            serverCert.certData = proprietaryCert;

            return serverCert;
        }

        /// <summary>
        /// Create transport stack according to security mechanism.
        /// </summary>
        internal void CreateTransportStack(IPAddress address)
        {
            RdpbcgrServerDecoder decoder = new RdpbcgrServerDecoder(this);
#if UT
            this.transportStack = new TransportStackServerMock(
                serverContext, decoder.DecodePacketCallback);
#else  
            if (this.encryptedProtocol == EncryptedProtocol.DirectCredSsp)
            {
                RdpcbgrServerTransportConfig config = new RdpcbgrServerTransportConfig(
                    SecurityStreamType.CredSsp,
                    address,
                    this.serverPort);
                this.directedTransportStack = new RdpbcgrServerTransportStack(this, config, decoder.DecodePacketCallback, cert);
            }
            else if (this.encryptedProtocol == EncryptedProtocol.NegotiationTls)
            {
                RdpcbgrServerTransportConfig config = new RdpcbgrServerTransportConfig(
                    SecurityStreamType.None,
                    address,
                    this.serverPort);
                this.directedTransportStack = new RdpbcgrServerTransportStack(this, config, decoder.DecodePacketCallback, cert);
            }
            else if (this.encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
            {
                RdpcbgrServerTransportConfig config = new RdpcbgrServerTransportConfig(
                    SecurityStreamType.None,
                    address,
                    this.serverPort);
                this.directedTransportStack = new RdpbcgrServerTransportStack(this, config, decoder.DecodePacketCallback, cert);
            }
            else
            {
                SocketTransportConfig config = new SocketTransportConfig();

                IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());

                config.LocalIpAddress = address;
                config.Type = StackTransportType.Tcp;
                config.Role = Role.Server;
                config.LocalIpPort = serverPort;
                config.MaxConnections = ConstValue.TRANSPORT_MAX_CONNECTIONS;
                config.BufferSize = ConstValue.TRANSPORT_BUFFER_SIZE;
                this.transportStack = new TransportStack(config, decoder.DecodePacketCallback);
            }
#endif
        }


        /// <summary>
        /// Update transport to an Negotiation-Based Security-Enhanced transport.
        /// </summary>
        internal void UpdateTransport()
        {
            try
            {
                if (encryptedProtocol == EncryptedProtocol.NegotiationTls)
                {
                    this.directedTransportStack.UpdateConfig(SecurityStreamType.Ssl);
                }
                else if (encryptedProtocol == EncryptedProtocol.NegotiationCredSsp)
                {
                    this.directedTransportStack.UpdateConfig(SecurityStreamType.CredSsp);
                }
            }
            catch (IOException)
            {
                // IOException may occured in some negative cases
            }

            return;
        }
        #endregion


        #region IDisposable
        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                    if (transportStack != null)
                    {
                        transportStack.Dispose();
                        transportStack = null;
                    }

                    if (directedTransportStack != null)
                    {
                        directedTransportStack.Dispose();
                        directedTransportStack = null;
                    }

                    if (updateSessionKeyEvent != null)
                    {
                        updateSessionKeyEvent.Close();
                        updateSessionKeyEvent = null;
                    }

                    if (serverContext != null)
                    {
                        serverContext = null;
                    }

                    disconnectTransportEventCache.Clear();
                }

                //Note disposing has been done.
                disposed = true;
            }
        }


        /// <summary>
        /// Destruct this instance.
        /// </summary>
        ~RdpbcgrServer()
        {
            Dispose(false);
        }
        #endregion

        #region Logging

        /// <summary>
        /// Method which used to add a warning
        /// </summary>
        internal void AddWarning(string formatStr, params object[] args)
        {
            string wrnStr = string.Format(formatStr, args);
            lock (this.warnings)
            {
                warnings.Add(wrnStr);
            }
        }

        /// <summary>
        /// Get the warnings
        /// </summary>
        public string[] Warnings
        {
            get
            {
                lock (warnings)
                {
                    string[] ws = warnings.ToArray();
                    warnings.Clear();
                    return ws;
                }
            }
        }

        #endregion
    }
}


