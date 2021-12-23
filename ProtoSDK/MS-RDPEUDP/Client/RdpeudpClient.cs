// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Transport;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    /// <summary>
    /// RDPEUDP Client
    /// </summary>
    public class RdpeudpClient : IDisposable
    {
        #region Private variables

        // Local Endpoint.
        private IPEndPoint localEndPoint;

        // Remote Endpoint.
        private IPEndPoint remoteEndPoint;

        // UDP Transport stack.
        private TransportStack udpTransport;

        // Transport mode
        private TransportMode transMode;

        // Thread handle for receiving thread.
        private Thread receiveThread;

        // Cancellation token source for receiving thread.
        private CancellationTokenSource receiveThreadCancellationTokenSource;

        // Socket of this client. RDPEUDP client only have one socket.
        private RdpeudpClientSocket socket;

        // Indicating the client is started successfully.
        private bool started;

        // The SHA-256 hash of the data that was transmitted from the server to the client in the securityCookie field of the Initiate Multitransport Request PDU.
        private byte[] cookieHash;

        #endregion Private variables

        #region Properties

        /// <summary>
        /// Get RdpeudpClientSocket in the RDPEUDP Client
        /// </summary>
        public RdpeudpClientSocket Socket
        {
            get
            {
                return socket;
            }
        }

        /// <summary>
        /// Whether socket in RDPEUDP client is auto handled
        /// </summary>
        public bool AutoHandle { get; set; }

        #endregion Properties

        #region Event
        /// <summary>
        /// Event for unhandled exception received.
        /// </summary>
        public event UnhandledExceptionReceivedDelegate UnhandledExceptionReceived;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize a new RdpeudpClient instance.
        /// </summary>
        /// <param name="localEp">Local endpoint.</param>
        /// <param name="remoteEp">Remote endpoint.</param>
        /// <param name="mode">Transport mode.</param>
        /// <param name="autoHandle">Auto handle transport.</param>
        /// <param name="securityCookie">The securityCookie field of the Initiate Multitransport Request PDU</param>
        public RdpeudpClient(IPEndPoint localEp, IPEndPoint remoteEp, TransportMode mode, bool autoHandle = true, byte[] securityCookie = null)
        {
            this.localEndPoint = localEp;
            this.remoteEndPoint = remoteEp;
            this.AutoHandle = autoHandle;
            this.transMode = mode;

            UdpClientConfig config = new UdpClientConfig(localEp.Port, remoteEp);
            udpTransport = new TransportStack(config, RdpeudpBasePacket.DecodePacketCallback);

            if (securityCookie == null)
            {
                cookieHash = new byte[32];
            }
            else
            {
                cookieHash = SHA256.HashData(securityCookie);
            }

            socket = new RdpeudpClientSocket(transMode, remoteEp, autoHandle, PacketSender, cookieHash);
        }
        #endregion Constructor

        /// <summary>
        /// Start RDPEUDP Client
        /// </summary>
        public void Start()
        {
            if (started)
            {
                return;
            }

            udpTransport.Start();

            receiveThread = new Thread(ReceiveLoop);

            receiveThreadCancellationTokenSource = new CancellationTokenSource();

            receiveThread.Start();

            started = true;
        }

        /// <summary>
        /// Stop RDPEUDP Client
        /// </summary>
        public void Stop()
        {
            if (!started)
            {
                return;
            }

            receiveThreadCancellationTokenSource.Cancel();

            if (receiveThread.IsAlive)
            {
                receiveThread.Join();
            }
            socket.Close();
            udpTransport.Stop();

            started = false;
        }

        /// <summary>
        /// Connect to the Server on Remote EndPoint 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Connect(TimeSpan timeout)
        {
            return socket.Connect(timeout);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (started)
            {
                this.Stop();
            }
        }

        #region Private methods

        /// <summary>
        /// Receive Loop
        /// </summary>
        private void ReceiveLoop()
        {
            try
            {
                TimeSpan timeout;
                object remoteEndpoint;
                StackPacket receivedPacket;
                timeout = new TimeSpan(0, 0, 0, 0, 100); // 100 milliseconds.

                // Check whether cancellation is requested before entering each receive loop.
                while (!receiveThreadCancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        receivedPacket = udpTransport.ExpectPacket(timeout, out remoteEndpoint);
                        socket.ReceivePacket(receivedPacket);
                    }
                    catch (TimeoutException)
                    { }
                }
            }
            catch (Exception ex)
            {
                UnhandledExceptionReceived?.Invoke(ex);
            }
        }

        /// <summary>
        /// Method provided to socket, which can use it to send packet
        /// </summary>
        /// <param name="remoteEP">Remote endpoint</param>
        /// <param name="packet"></param>
        private void PacketSender(IPEndPoint remoteEP, StackPacket packet)
        {
            udpTransport.SendPacket(remoteEP, packet);
        }

        #endregion Private methods
    }
}
