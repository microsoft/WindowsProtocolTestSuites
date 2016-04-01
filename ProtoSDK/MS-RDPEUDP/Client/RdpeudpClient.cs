// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    /// <summary>
    /// RDPEUDP Client
    /// </summary>
    public class RdpeudpClient : IDisposable
    {
        #region Private variables

        // Local Endpoint
        private IPEndPoint localEndPoint;

        // Remote Endpoint
        private IPEndPoint remoteEndpoint;
        
        // UDP Transport stack
        private TransportStack udpTransport;

        // Transport mode
        private TransportMode transMode;

        // Whether the client the running, this mean the server can receive and send normally
        private bool running = false;

        // Thread handle for receiving thread
        private Thread receiveThread;

        // Socket of this client. RDPEUDP client only have one socket
        private RdpeudpClientSocket socket;

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
        public bool AutoHandle{get; set;}

        /// <summary>
        /// Whether the RdpeudpClient is running
        /// </summary>
        public bool Running
        {
            get
            {
                return running;
            }
        }

        #endregion Properties


        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="localEp"></param>
        public RdpeudpClient(IPEndPoint localEp, IPEndPoint remoteEp, TransportMode mode, bool autoHandle = true)
        {
            this.localEndPoint = localEp;
            this.remoteEndpoint = remoteEp;
            this.AutoHandle = autoHandle;
            this.transMode = mode;

            UdpClientConfig config = new UdpClientConfig(localEp.Port, remoteEp);
            udpTransport = new TransportStack(config, RdpeudpBasePacket.DecodePacketCallback);

            socket = new RdpeudpClientSocket(transMode, remoteEp, autoHandle, packetsender);
        }
        #endregion Constructor

        /// <summary>
        /// Start RDPEUDP Client
        /// </summary>
        public void Start()
        {
            udpTransport.Start();
            running = true;
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.Start();
        }        
      
        /// <summary>
        /// Stop RDPEUDP Client
        /// </summary>
        public void Stop()
        {
            running = false;
            if (receiveThread.IsAlive)
            {
                receiveThread.Abort();
                receiveThread.Join();
            }
            socket.Close();
            udpTransport.Stop();
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
            if (running)
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
            TimeSpan timeout;
            object remoteEndpoint;
            StackPacket receivedPacket;
            timeout = new TimeSpan(0, 0, 0, 0, 100); // 100 milliseconds.
            while (running)
            {                   // An infinite loop to receive packet from transport stack.
                try
                {
                    receivedPacket = udpTransport.ExpectPacket(timeout, out remoteEndpoint);
                    socket.ReceivePacket(receivedPacket);
                }
                catch (TimeoutException)
                { }
                Thread.Sleep(RdpeudpSocketConfig.ReceivingInterval);
            }
        }

        /// <summary>
        /// Method provided to socket, which can use it to send packet
        /// </summary>
        /// <param name="remoteEP">Remote endpoint</param>
        /// <param name="packet"></param>
        private void packetsender(IPEndPoint remoteEP, StackPacket packet)
        {
            udpTransport.SendPacket(remoteEP, packet);
        }

        #endregion Private methods
    }
}
