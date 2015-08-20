// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the udp client transport.<para/>
    /// when decode packet from received bytes, drop the received bytes after decode it.<para/>
    /// even though the consumed length is smaller that the bytes length.
    /// </summary>
    internal class UdpClientTransport : IUdpClient, IVisitorUdpReceiveLoop
    {
        #region Fields

        /// <summary>
        /// a SocketTransportConfig object that contains the config.
        /// </summary>
        private SocketTransportConfig socketConfig;

        /// <summary>
        /// a UdpClient object that specifies the underlayer transport.<para/>
        /// when start, it will be constructed; <para/>
        /// when stop or dispose, it will be close and set to null.
        /// </summary>
        private UdpClient udpClient;

        /// <summary>
        /// a ThreadManager object that contains the received thread.<para/>
        /// when construct, it started;<para/>
        /// when dispose, it will be stop and set to null.
        /// </summary>
        private ThreadManager thread;

        /// <summary>
        /// a SyncFilterQueue&lt;TransportEvent&gt; object that contains the event,
        /// such as disconnected and exception.<para/>
        /// clear event queue when connect to server. never be null.
        /// </summary>
        private SyncFilterQueue<TransportEvent> eventQueue;

        /// <summary>
        /// a SyncFilterQueue&lt;UdpClientReceivedBytes&gt; object that contains the data,
        /// such as disconnected and exception.<para/>
        /// clear event queue when connect to server. never be null.
        /// </summary>
        private SyncFilterQueue<UdpReceivedBytes> buffer;

        /// <summary>
        /// a DecodePacketCallback delegate that is used to decode the packet from bytes.
        /// </summary>
        private DecodePacketCallback decoder;

        /// <summary>
        /// a list to store the arrived packet.<para/>
        /// clear packet list when connect to server. never be null.
        /// </summary>
        private SyncFilterQueue<IPEndPointStackPacket> packetCache;

        /// <summary>
        /// a bool value that indicates whether lsp hooked the transport.
        /// </summary>
        private bool lspHooked;

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="transportConfig">
        /// a TransportConfig object that contains the config.
        /// </param>
        /// <param name="decodePacketCallback">
        /// a DecodePacketCallback delegate that is used to decode the packet from bytes.
        /// </param>
        /// <exception cref="ArgumentException">
        /// thrown when transportConfig is not SocketTransportConfig
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when transportConfig is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when decodePacketCallback is null.
        /// </exception>
        public UdpClientTransport(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
        {
            if (transportConfig == null)
            {
                throw new ArgumentNullException("transportConfig");
            }

            if (decodePacketCallback == null)
            {
                throw new ArgumentNullException("decodePacketCallback");
            }

            this.UpdateConfig(transportConfig);

            this.eventQueue = new SyncFilterQueue<TransportEvent>();
            this.buffer = new SyncFilterQueue<UdpReceivedBytes>();
            this.packetCache = new SyncFilterQueue<IPEndPointStackPacket>();

            this.decoder = decodePacketCallback;
        }

        #endregion

        #region IStartable Members

        /// <summary>
        /// to start the transport. <para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.<para/>
        /// if config did not specify the address, sdk will specify an endpoint.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the specified endpoint in config has been started.
        /// </exception>
        public void Start()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.socketConfig.LocalIpAddress == null)
            {
                this.socketConfig.LocalIpAddress = IPAddress.Any;
            }

            this.Start(new IPEndPoint(this.socketConfig.LocalIpAddress, this.socketConfig.LocalIpPort));
        }


        /// <summary>
        /// start at the specified endpoint<para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the listener.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when localEndPoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when localEndPoint is not an IPEndPoint/port.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the udp client is started.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the received thread does not cleanup.
        /// </exception>
        public void Start(object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("localEndPoint");
            }

            if (this.udpClient != null)
            {
                throw new InvalidOperationException("udp client is started.");
            }

            if (this.thread != null)
            {
                throw new InvalidOperationException("the received thread does not cleanup.");
            }

            IPEndPoint requiredLocalEP = localEndPoint as IPEndPoint;
            if (requiredLocalEP == null)
            {
                if (this.socketConfig.LocalIpAddress == null)
                {
                    this.socketConfig.LocalIpAddress = IPAddress.Any;
                }
                requiredLocalEP = Utility.GetEndPointByPort(this.socketConfig, localEndPoint);
            }

            this.socketConfig.LocalIpAddress = requiredLocalEP.Address;
            this.socketConfig.LocalIpPort = requiredLocalEP.Port;

            this.eventQueue.Clear();
            this.packetCache.Clear();
            this.buffer.Clear();

            bool isLspHooked;
            bool isBlocking;
            StackTransportType type = this.socketConfig.Type;

            // get the replaced endpoint from LSP.
            IPEndPoint actualListenedLocalEP =
                LspConsole.Instance.GetReplacedEndPoint(type, requiredLocalEP, out isLspHooked, out isBlocking);

            // store the lsp state.
            this.lspHooked = isLspHooked;

            this.udpClient = new UdpClient(actualListenedLocalEP);

            if (socketConfig.LocalIpPort == 0)
            {
                //If set local Port to 0, a free port will be selected
                socketConfig.LocalIpPort = (udpClient.Client.LocalEndPoint as IPEndPoint).Port;
            }

            // if LSP is enabled, intercept the traffic
            if (isLspHooked)
            {
                LspConsole.Instance.InterceptTraffic(type, isBlocking, requiredLocalEP, (IPEndPoint)this.udpClient.Client.LocalEndPoint);
            }

            this.thread = new ThreadManager(this.UdpClientReceiveLoop, this.UnblockReceiveThread);
            this.thread.Start();
        }


        /// <summary>
        /// stop all listener of server.<para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the udp client is not connected to server, must invoke Start() first.
        /// </exception>
        public void Stop()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException(
                    "udp client is not connected to server, must invoke Start() first.");
            }

            this.Stop(new IPEndPoint(this.socketConfig.LocalIpAddress, this.socketConfig.LocalIpPort));
        }


        /// <summary>
        /// stop the specified listener.<para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the listener.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an IPEndPoint.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the udp client is not connected to server, must invoke Start() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when failed to stop the endpoint, it is not started.
        /// </exception>
        public void Stop(object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException(
                    "udp client is not connected to server, must invoke Start() first.");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("localEndPoint");
            }

            IPEndPoint localEP = localEndPoint as IPEndPoint;
            if (localEP == null)
            {
                Utility.StopTransportByPort(this, null, this.socketConfig.LocalIpAddress, localEndPoint);
                return;
            }

            if (this.lspHooked)
            {
                localEP = this.udpClient.Client.LocalEndPoint as IPEndPoint;

                LspConsole.Instance.UnblockTraffic(localEP, this.socketConfig.Type);
            }

            if (!localEP.Equals(this.udpClient.Client.LocalEndPoint))
            {
                throw new InvalidOperationException("failed to stop the endpoint, it is not started.");
            }

            this.thread.Stop();
            this.thread = null;

            this.udpClient.Close();
            this.udpClient = null;
        }

        #endregion

        #region ITarget Members

        /// <summary>
        /// Send a packet to a special remote host.<para/>
        /// the transport must be a TcpServer, UdpClient or UdpServer.
        /// </summary>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send packet.
        /// </param>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an IPEndPoint.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp client is not start, must invoke Start() first.
        /// </exception>
        public void SendPacket(object remoteEndPoint, StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException("udp client is not start, must invoke Start() first.");
            }

            this.SendBytes(remoteEndPoint, packet.ToBytes());
        }


        /// <summary>
        /// Send arbitrary message to a special remote host.<para/>
        /// the transport must be a TcpServer, UdpClient or UdpServer.
        /// </summary>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send bytes.
        /// </param>
        /// <param name="message">
        /// a bytes array that contains the bytes to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an IPEndPoint.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp client is not start, must invoke Start() first.
        /// </exception>
        public void SendBytes(object remoteEndPoint, byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException("udp client is not start, must invoke Start() first.");
            }

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            IPEndPoint remoteEP = remoteEndPoint as IPEndPoint;
            if (remoteEP == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint.", "remoteEndPoint");
            }

            if (this.lspHooked)
            {
                //replace the real client endpoint with lsp dll endpoint
                IPEndPoint mappedEndPoint = LspConsole.Instance.GetMappedIPEndPoint(
                        (IPEndPoint)this.udpClient.Client.LocalEndPoint, remoteEP, StackTransportType.Udp);

                if (mappedEndPoint != null)
                {
                    remoteEP = mappedEndPoint;

                    //add udp header
                    message = ArrayUtility.ConcatenateArrays<byte>(Utility.CreateUdpHeader(remoteEP), message);
                }
            }

            this.udpClient.Send(message, message.Length, remoteEP);
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be a TcpServer, UdpClient or UdpServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that indicates the connection received data from.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp client is not start, must invoke Start() first.
        /// </exception>
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException("udp client is not start, must invoke Start() first.");
            }

            UdpReceivedBytes bytes = this.buffer.Dequeue(timeout);

            remoteEndPoint = bytes.RemoteEndPoint;

            // exception event arrived
            if (bytes.Packet == null)
            {
                // do not remove it.
                this.buffer.Enqueue(bytes);

                throw new InvalidOperationException("exception arrived when expect bytes from udp.");
            }

            return bytes.Packet;
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer, UdpClient or UdpServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the connection expected packet.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp client is not start, must invoke Start() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp client failed to decode udp packet
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException("udp client is not start, must invoke Start() first.");
            }

            return this.GetPacket(timeout, false, out remoteEndPoint);
        }

        #endregion

        #region ISourceSend Members

        /// <summary>
        /// Send a packet over the transport.<para/>
        /// the underlayer transport must be TcpClient, UdpClient, Stream or NetbiosClient.<para/>
        /// the UdpClient will send data to the remote endpoint that stored in config.
        /// </summary>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp client is not start, must invoke Start() first.
        /// </exception>
        public void SendPacket(StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException("udp client is not start, must invoke Start() first.");
            }

            this.SendBytes(packet.ToBytes());
        }


        /// <summary>
        /// Send an arbitrary message over the transport.<para/>
        /// the underlayer transport must be TcpClient, UdpClient, Stream or NetbiosClient.<para/>
        /// the UdpClient will send data to the remote endpoint that stored in config.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp client is not start, must invoke Start() first.
        /// </exception>
        public void SendBytes(byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException("udp client is not start, must invoke Start() first.");
            }

            this.SendBytes(
                new IPEndPoint(this.socketConfig.RemoteIpAddress, this.socketConfig.RemoteIpPort), message);
        }

        #endregion

        #region ITransport Members

        /// <summary>
        /// get a bool value that indicates whether there is data received from transport.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public bool IsDataAvailable
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("UdpClientTransport");
                }

                return this.packetCache.Count > 0 || this.buffer.Count > 0;
            }
        }


        /// <summary>
        /// add transport event to transport, TSD can invoke ExpectTransportEvent to get it.
        /// </summary>
        /// <param name="transportEvent">
        /// a TransportEvent object that contains the event to add to the queue
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when transportEvent is null.
        /// </exception>
        public void AddEvent(TransportEvent transportEvent)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (transportEvent == null)
            {
                throw new ArgumentNullException("transportEvent");
            }

            this.eventQueue.Enqueue(transportEvent);

            this.buffer.Enqueue(new UdpReceivedBytes(null,
                transportEvent.RemoteEndPoint as IPEndPoint, transportEvent.LocalEndPoint as IPEndPoint));
        }


        /// <summary>
        /// to update the config of transport at runtime.
        /// </summary>
        /// <param name="config">
        /// a TransportConfig object that contains the config to update
        /// </param>
        /// <exception cref="ArgumentException">
        /// thrown when transportConfig is not SocketTransportConfig
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when config is null.
        /// </exception>
        public void UpdateConfig(TransportConfig config)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            SocketTransportConfig socketTransportConfig = config as SocketTransportConfig;

            if (socketTransportConfig == null)
            {
                throw new ArgumentException("transportConfig must be SocketTransportConfig", "config");
            }

            this.socketConfig = socketTransportConfig;
        }


        /// <summary>
        /// expect transport event from transport.<para/>
        /// if event arrived and packet data buffer is empty, return event directly.<para/>
        /// decode packet from packet data buffer, return packet if arrived, otherwise, return event.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan struct that specifies the timeout of waiting for a packet or event from the transport.
        /// </param>
        /// <returns>
        /// a TransportEvent object that contains the expected event from transport.
        /// </returns>
        /// <exception cref="TimeoutException">
        /// thrown when timeout to wait for packet coming.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// thrown when timeout to wait for event coming.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the udp client is not connected to server, must invoke Start() first.
        /// </exception>
        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            if (this.udpClient == null)
            {
                throw new InvalidOperationException("udp client is not start, must invoke Start() first.");
            }

            try
            {
                object localEP = this.LspHookedLocalEP;
                object remoteEP = null;

                // decode packet
                StackPacket packet = this.GetPacket(timeout, true, out remoteEP);

                // if packet is decoded, return it.
                return new TransportEvent(EventType.ReceivedPacket, remoteEP, localEP, packet);
            }
            catch (InvalidOperationException exc)
            {
                if (exc.Data.Count == 0)
                {
                    throw;
                }
            }

            // if no packet, and there is event coming, return event.
            // set timeout to zero, must not wait for event coming.
            return eventQueue.Dequeue(new TimeSpan());
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Release the managed and unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources. 
        /// </summary>
        /// <param name = "disposing">
        /// If disposing equals true, managed and unmanaged resources are disposed. if false, Only unmanaged resources 
        /// can be disposed. 
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.thread != null)
                    {
                        this.thread.Dispose();
                        this.thread = null;
                    }
                    if (this.udpClient != null)
                    {
                        this.udpClient.Close();
                        this.udpClient = null;
                    }
                    if (this.eventQueue != null)
                    {
                        // the SyncFilterQueue may throw exception, donot arise exception.
                        this.eventQueue.Dispose();
                        this.eventQueue = null;
                    }
                    if (this.packetCache != null)
                    {
                        this.packetCache.Dispose();
                        this.packetCache = null;
                    }
                    if (this.buffer != null)
                    {
                        // the SyncFilterQueue may throw exception, donot arise exception.
                        this.buffer.Dispose();
                        this.buffer = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~UdpClientTransport()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// get packet from packet cache or decode from buffer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout
        /// </param>
        /// <param name="removeEvent">
        /// a bool value that indicates whether need to remove the event from buffer.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint.
        /// </param>
        /// <returns>
        /// a StackPacket object that contains the decoded packet.
        /// </returns>
        private StackPacket GetPacket(TimeSpan timeout, bool removeEvent, out object remoteEndPoint)
        {
            // get the packet in packet list.
            IPEndPointStackPacket packet = Utility.GetOne<IPEndPointStackPacket>(this.packetCache, null);
            if (packet != null)
            {
                remoteEndPoint = packet.RemoteEndPoint;

                return packet.Packet;
            }

            UdpReceivedBytes bytes = this.buffer.Dequeue(timeout);

            remoteEndPoint = bytes.RemoteEndPoint;

            // exception event arrived
            if (bytes.Packet == null)
            {
                if (!removeEvent)
                {
                    this.buffer.Enqueue(bytes);
                }

                InvalidOperationException exc =
                    new InvalidOperationException("exception arrived when expect packet from udp.");

                // identify this exception.
                exc.Data.Add(exc.GetType().Name, true);

                throw exc;
            }

            // decode packets using data in buffer.
            int consumedLength = 0;
            int expectedLength = 0;

            StackPacket[] packets = this.decoder(remoteEndPoint, bytes.Packet, out consumedLength, out expectedLength);

            // if no packet, drop the recieved data and continue.
            if (packets == null || packets.Length == 0)
            {
                throw new InvalidOperationException("udp client failed to decode udp packet");
            }

            // if packet arrived, add to packet list, and return the first.
            foreach (StackPacket item in packets)
            {
                this.packetCache.Enqueue(
                    new IPEndPointStackPacket(item, bytes.RemoteEndPoint, bytes.LocalEndPoint));
            }

            // set timeout to zero. when packet is decoded, must not wait.
            return GetPacket(new TimeSpan(), removeEvent, out remoteEndPoint);
        }


        /// <summary>
        /// an IPEndPoint object that specifies the local endpoint.<para/>
        /// if LSP hooked, return the required local endpoint.<para/>
        /// otherwise, return the actual listened local endpoint.
        /// </summary>
        public IPEndPoint LspHookedLocalEP
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("UdpClientTransport");
                }

                return new IPEndPoint(this.socketConfig.LocalIpAddress, this.socketConfig.LocalIpPort);
            }
        }


        /// <summary>
        /// add the received data to buffer.
        /// </summary>
        /// <param name="data">
        /// a bytes array that contains the received data.
        /// </param>
        /// <param name="localEP">
        /// an IPEndPoint object that specifies the local endpoint.
        /// </param>
        /// <param name="remoteEP">
        /// an IPEndPoint object that specifies the remote endpoint.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void VisitorAddReceivedData(byte[] data, IPEndPoint localEP, IPEndPoint remoteEP)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpClientTransport");
            }

            this.buffer.Enqueue(new UdpReceivedBytes(data, remoteEP, localEP));
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void UdpClientReceiveLoop()
        {
            // donot arise any exception in the work thread.
            Utility.SafelyOperation(this.UdpClientReceiveLoopImp);
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void UdpClientReceiveLoopImp()
        {
            UdpReceiveLoopVisitor.Visit(this, this, this.udpClient, this.thread, this.lspHooked);
        }


        /// <summary>
        /// the method to unblock the received thread.
        /// </summary>
        private void UnblockReceiveThread()
        {
            this.udpClient.Close();
        }

        #endregion
    }
}
