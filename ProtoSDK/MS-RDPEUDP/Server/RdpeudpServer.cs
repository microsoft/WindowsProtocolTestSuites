// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Transport;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
// using Microsoft.Protocols.TestTools.ExtendedLogging;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    /// <summary>
    /// Structure of packet information.
    /// </summary>
    public class StackPacketInfo
    {
        /// <summary>
        /// Identity of local endpoint
        /// </summary>
        public object LocalEndPoint;
        /// <summary>
        /// Identity of remote endpoint
        /// </summary>
        public object RemoteEndPoint;

        /// <summary>
        /// The packet received
        /// </summary>
        public StackPacket Packet;

        public StackPacketInfo(object localEp, object remoteEp, StackPacket pkt)
        {
            LocalEndPoint = localEp;
            RemoteEndPoint = remoteEp;
            Packet = pkt;
        }
    }

    /// <summary>
    /// RDPEUDP Server
    /// </summary>
    public class RdpeudpServer : IDisposable
    {
        #region Private variables

        // Local Endpoint
        private IPEndPoint localEndPoint;

        // UDP Transport stack
        private TransportStack udpTransport;

        // Buffer for unprocessed packet
        private List<StackPacketInfo> unprocessedPacketBuffer;

        // Dictionary of server sockets created by this server
        private Dictionary<IPEndPoint, RdpeudpServerSocket> serverSocketDic;

        // Thread handle for receiving thread
        private Thread receiveThread;

        // Cancellation token source for receiving thread.
        private CancellationTokenSource receiveThreadCancellationTokenSource;

        // Indicating the client is started successfully.
        private bool started;

        #endregion Private variables

        #region Properties
        /// <summary>
        /// This value decides whether the RdpeudpServerSocket is autohandle
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
        /// Constructor
        /// </summary>
        /// <param name="localEp"></param>
        public RdpeudpServer(IPEndPoint localEp, bool autoHandle = true)
        {
            this.localEndPoint = localEp;
            this.AutoHandle = autoHandle;
            this.unprocessedPacketBuffer = new List<StackPacketInfo>();
            this.serverSocketDic = new Dictionary<IPEndPoint, RdpeudpServerSocket>();

            UdpServerConfig config = new UdpServerConfig(localEp);
            config.Role = Transport.Role.Server;
            udpTransport = new TransportStack(config, RdpeudpBasePacket.DecodePacketCallback);
        }
        #endregion Constructor

        /// <summary>
        /// Start this RDPEUDP Server
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
        /// Stop this RDPEUDP Server
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
            foreach (IPEndPoint ep in serverSocketDic.Keys)
            {
                serverSocketDic[ep].Close();
            }

            serverSocketDic.Clear();

            udpTransport.Stop();

            unprocessedPacketBuffer.Clear();

            started = false;
        }

        public RdpeudpServerSocket Accept(IPAddress remoteIP, TransportMode mode, TimeSpan timeout)
        {
            DateTime endTime = DateTime.Now + timeout;
            RdpeudpServerSocket serverSocket = this.CreateSocket(remoteIP, mode, timeout);
            if (serverSocket == null)
            {
                return null;
            }

            if (serverSocket.ExpectConnect(endTime - DateTime.Now))
            {
                return serverSocket;
            }
            else
            {
                serverSocketDic.Remove(serverSocket.RemoteEndPoint);
            }

            return null;
        }

        /// <summary>
        /// Create a RdpeudpServerSocket 
        /// The socket can only be created if the RdpeudpServer received corresponding SYN Packet
        /// </summary>
        /// <param name="remoteIP">IP adress</param>
        /// <param name="mode">connection mode</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RdpeudpServerSocket CreateSocket(IPAddress remoteIP, TransportMode mode, TimeSpan timeout)
        {
            IPEndPoint remoteEndPoint;
            RdpeudpPacket synPacket = ExpectSynPacket(remoteIP, mode, timeout, out remoteEndPoint);
            if (synPacket == null)
            {
                return null;
            }

            RdpeudpServerSocket serverSock = new RdpeudpServerSocket(mode, remoteEndPoint, AutoHandle, PacketSender, this);
            serverSock.ReceivePacket(synPacket);

            serverSocketDic[remoteEndPoint] = serverSock;
            return serverSock;
        }

        /// <summary>
        /// Expect a SYN Packet which is from specific remoteIP using specific connection mode
        /// </summary>
        /// <param name="remoteIP">IP address of remote endpoint</param>
        /// <param name="mode">connection mode</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public RdpeudpPacket ExpectSynPacket(IPAddress remoteIP, TransportMode mode, TimeSpan timeout, out IPEndPoint remoteEndPoint)
        {
            remoteEndPoint = null;
            DateTime endtime = DateTime.Now + timeout;
            RDPUDP_FLAG expectFlag = RDPUDP_FLAG.RDPUDP_FLAG_SYN;
            if (mode == TransportMode.Lossy)
            {
                expectFlag |= RDPUDP_FLAG.RDPUDP_FLAG_SYNLOSSY;
            }
            while (DateTime.Now < endtime)
            {
                lock (this.unprocessedPacketBuffer)
                {
                    for (int i = 0; i < unprocessedPacketBuffer.Count; i++)
                    {
                        StackPacketInfo spInfo = unprocessedPacketBuffer[i];
                        remoteEndPoint = spInfo.RemoteEndPoint as IPEndPoint;
                        if (remoteEndPoint.Address.Equals(remoteIP))
                        {
                            RdpeudpPacket eudpPacket = new RdpeudpPacket();
                            if (PduMarshaler.Unmarshal(spInfo.Packet.ToBytes(), eudpPacket, false))
                            {
                                if (eudpPacket.FecHeader.uFlags.HasFlag(expectFlag))
                                {
                                    unprocessedPacketBuffer.RemoveAt(i);
                                    return eudpPacket;
                                }
                            }
                        }
                    }
                }
                // If not receive a Packet, wait a while 
                Thread.Sleep(RdpeudpSocketConfig.ReceivingInterval);
            }
            return null;
        }

        /// <summary>
        /// Remove a Socket from server
        /// </summary>
        /// <param name="socket"></param>
        public void RemoveSocket(RdpeudpServerSocket socket)
        {
            if (serverSocketDic != null && socket != null)
            {
                if (serverSocketDic.ContainsKey(socket.RemoteEndPoint))
                {
                    serverSocketDic.Remove(socket.RemoteEndPoint);
                }
            }
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

        #region Private Method

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
                        receivedPacket = udpTransport.ExpectPacket(timeout, localEndPoint, out remoteEndpoint);

                        if (serverSocketDic.ContainsKey(remoteEndpoint as IPEndPoint))
                        {
                            serverSocketDic[remoteEndpoint as IPEndPoint].ReceivePacket(receivedPacket);
                        }
                        else // If the packet belong to no RDPEUDP socket, try to Accept as a new RDPEUDP socket.
                        {
                            StackPacketInfo packetinfo = new StackPacketInfo(localEndPoint, remoteEndpoint, receivedPacket);
                            lock (this.unprocessedPacketBuffer)
                            {
                                unprocessedPacketBuffer.Add(packetinfo);
                            }

                            // ETW Provider Dump Message
                            // byte[] packetBytes = receivedPacket.ToBytes();
                            // string messageName = "RDPEUDP:ReceivedPDU";
                            // ExtendedLogger.DumpMessage(messageName, RdpeudpSocket.DumpLevel_LayerTLS, typeof(RdpeudpPacket).Name, packetBytes);
                        }
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
            if (started)
            {
                udpTransport.SendPacket(localEndPoint, remoteEP, packet);
            }
        }
        #endregion Private Method
    }
}
