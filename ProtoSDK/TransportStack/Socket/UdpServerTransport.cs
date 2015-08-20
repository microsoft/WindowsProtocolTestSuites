// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the udp server transport.<para/>
    /// when decode packet from received bytes, drop the received bytes after decode it.<para/>
    /// even though the consumed length is smaller that the bytes length.
    /// </summary>
    internal class UdpServerTransport : IUdpServer
    {
        #region Fields

        /// <summary>
        /// a SocketTransportConfig object that contains the config.
        /// </summary>
        private SocketTransportConfig socketConfig;

        /// <summary>
        /// a DecodePacketCallback delegate that is used to decode the packet from bytes.
        /// </summary>
        private DecodePacketCallback decoder;

        /// <summary>
        /// a SyncFilterQueue&lt;TransportEvent&gt; object that contains the event,
        /// such as disconnected and exception.<para/>
        /// clear event queue when connect to server. never be null.
        /// </summary>
        private SyncFilterQueue<TransportEvent> eventQueue;

        /// <summary>
        /// a Dictionary&lt;IPEndPoint, UdpServerListener&gt; that contains the listeners.<para/>
        /// initialize at constructor, clear at stop. never be null util disposed. maybe empty.
        /// </summary>
        private Dictionary<IPEndPoint, UdpServerListener> listeners;

        /// <summary>
        /// a SyncFilterQueue&lt;UdpReceivedBytes&gt; object that contains the data,
        /// such as disconnected and exception.<para/>
        /// clear event queue when connect to server. never be null.
        /// </summary>
        private SyncFilterQueue<UdpReceivedBytes> buffer;

        /// <summary>
        /// a PacketCache to store the arrived packet.<para/>
        /// clear packet list when connect to server. never be null.
        /// </summary>
        private SyncFilterQueue<IPEndPointStackPacket> packetCache;

        /// <summary>
        /// a bool value that indicates whether server is started.
        /// </summary>
        private bool started;

        #endregion

        #region Properties

        /// <summary>
        /// get a SyncFilterQueue&lt;UdpReceivedBytes&gt; object that contains the data,
        /// such as disconnected and exception.<para/>
        /// clear event queue when connect to server. never be null.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public SyncFilterQueue<UdpReceivedBytes> Buffer
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("UdpServerTransport");
                }

                return this.buffer;
            }
        }

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
        public UdpServerTransport(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
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

            this.listeners = new Dictionary<IPEndPoint, UdpServerListener>();
            this.eventQueue = new SyncFilterQueue<TransportEvent>();
            this.packetCache = new SyncFilterQueue<IPEndPointStackPacket>();
            this.buffer = new SyncFilterQueue<UdpReceivedBytes>();

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
                throw new ObjectDisposedException("UdpServerTransport");
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
        /// thrown when the specified endpoint has been started.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when localEndPoint is an int port, but the LocalIpAddress is not configured.
        /// </exception>
        public void Start(object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerTransport");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("localEndPoint");
            }

            IPEndPoint requiredLocalEP = localEndPoint as IPEndPoint;
            if (requiredLocalEP == null)
            {
                requiredLocalEP = Utility.GetEndPointByPort(this.socketConfig, localEndPoint);
            }

            if (this.listeners.ContainsKey(requiredLocalEP))
            {
                throw new InvalidOperationException("the specified endpoint has been started.");
            }

            bool isLspHooked;
            bool isBlocking;
            StackTransportType type = this.socketConfig.Type;

            // get the replaced endpoint from LSP.
            IPEndPoint actualListenedLocalEP =
                LspConsole.Instance.GetReplacedEndPoint(type, requiredLocalEP, out isLspHooked, out isBlocking);

            UdpClient udpClient = new UdpClient(actualListenedLocalEP);
            UdpServerListener listener = new UdpServerListener(udpClient, this, isLspHooked);

            // if LSP is enabled, intercept the traffic
            if (isLspHooked)
            {
                LspConsole.Instance.InterceptTraffic(
                    type, isBlocking, requiredLocalEP, udpClient.Client.LocalEndPoint as IPEndPoint);
            }

            listener.Start(requiredLocalEP);
            this.listeners[requiredLocalEP] = listener;

            this.started = true;
        }


        /// <summary>
        /// stop all listener of server.<para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        public void Stop()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            // get all endpoint to stop.
            IPEndPoint[] endpoints = new IPEndPoint[this.listeners.Count];

            this.listeners.Keys.CopyTo(endpoints, 0);

            // stop the specified endpoint.
            foreach (IPEndPoint endpoint in endpoints)
            {
                this.Stop(endpoint);
            }

            this.listeners.Clear();
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
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an IPEndPoint/port.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the udp client is not connected to server, must invoke Start() first.
        /// </exception>
        public void Stop(object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("localEndPoint");
            }

            IPEndPoint endpoint = localEndPoint as IPEndPoint;
            if (endpoint == null)
            {
                Utility.StopTransportByPort(this, this.listeners.Keys, null, localEndPoint);
                return;
            }

            if (!this.listeners.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the listener specified by endpoint cannot be found!");
            }

            UdpServerListener listener = this.listeners[endpoint];

            if (listener == null)
            {
                throw new InvalidOperationException("the listener specified by endpoint is null!");
            }

            if (listener.IsLspHooked)
            {
                LspConsole.Instance.UnblockTraffic(listener.LspHookedLocalEP, this.socketConfig.Type);
            }

            listener.Stop();

            this.listeners.Remove(endpoint);
        }

        #endregion

        #region ITargetReceive Members

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
        /// an object that indicates the remote endpoint received data from.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received data at.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp server is not start, must invoke Start() first.
        /// </exception>
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint, out object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (this.listeners.Count == 0)
            {
                throw new InvalidOperationException("udp server is not start, must invoke Start() first.");
            }

            return this.GetUdpBytes(null, timeout, false, out remoteEndPoint, out localEndPoint).Packet;
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer, UdpClient or UdpServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.<para/>
        /// never return null, if no packets, throw exception.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp server is not start, must invoke Start() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the item in data sequence is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client in data sequence is disposed.
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint, out object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (this.listeners.Count == 0)
            {
                throw new InvalidOperationException("udp server is not start, must invoke Start() first.");
            }

            return this.GetFilteredPacket(null, timeout, false, out remoteEndPoint, out localEndPoint);
        }

        #endregion

        #region ITargetSend Members

        /// <summary>
        /// Send a packet to a special remote host.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the local endpoint to send bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send packet.
        /// </param>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp server is not start, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an IPEndPoint.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp server is not start, must invoke Start() first.
        /// </exception>
        public void SendPacket(object localEndPoint, object remoteEndPoint, StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (this.listeners.Count == 0)
            {
                throw new InvalidOperationException("udp server is not start, must invoke Start() first.");
            }

            this.SendBytes(localEndPoint, remoteEndPoint, packet.ToBytes());
        }


        /// <summary>
        /// Send arbitrary message to a special remote host.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the local endpoint to send bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send bytes.
        /// </param>
        /// <param name="message">
        /// a bytes array that contains the bytes to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp server is not start, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an IPEndPoint.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer udp transport specified by endpoint is invalid
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the listener on the specified localEndPoint does not exists,
        /// please invoke Start(localEndPoint) to start it first.
        /// </exception>
        public void SendBytes(object localEndPoint, object remoteEndPoint, byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            if (this.listeners.Count == 0)
            {
                throw new InvalidOperationException("udp server is not start, must invoke Start() first.");
            }

            IPEndPoint remoteEP = remoteEndPoint as IPEndPoint;
            if (remoteEP == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint.", "remoteEndPoint");
            }

            IPEndPoint localEP = localEndPoint as IPEndPoint;
            if (localEP == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint.", "localEndPoint");
            }

            if (!this.listeners.ContainsKey(localEP))
            {
                throw new InvalidOperationException(
                    "the listener on the specified localEndPoint does not exists,"
                    + " please invoke Start(localEndPoint) to start it first.");
            }

            UdpServerListener listener = this.listeners[localEP];

            if (listener == null || listener.Listener == null)
            {
                throw new InvalidOperationException("the underlayer udp transport specified by endpoint is invalid");
            }

            IPEndPoint mappedEndPoint = null;
            if (listener.IsLspHooked)
            {
                //replace the real client endpoint with lsp dll endpoint
                mappedEndPoint =
                    LspConsole.Instance.GetMappedIPEndPoint(
                        (IPEndPoint)listener.Listener.Client.LocalEndPoint, remoteEP, StackTransportType.Udp);

                if (mappedEndPoint != null)
                {
                    //add udp header
                    message = ArrayUtility.ConcatenateArrays<byte>(Utility.CreateUdpHeader(remoteEP), message);
                }
            }

            listener.Listener.Send(message, message.Length, mappedEndPoint == null ? remoteEP : mappedEndPoint);
        }

        #endregion

        #region ISpecialTarget Members

        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the local listening endpoint to received bytes.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.<para/>
        /// return the whole received bytes, even though the maxCount is smaller that the bytes length.
        /// </param>
        /// <param name="remoteEndPoint">
        /// return an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the remote listening endpoint to received bytes.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp server is not start, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an IPEndPoint.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client specified by endpoint cannot be found!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client specified by endpoint is null!
        /// </exception>
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, object localEndPoint, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            if (this.listeners.Count == 0)
            {
                throw new InvalidOperationException("udp server is not start, must invoke Start() first.");
            }

            IPEndPoint endpoint = localEndPoint as IPEndPoint;
            if (endpoint == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint.", "remoteEndPoint");
            }

            object localEP = null;

            return this.GetUdpBytes(
                new TransportFilter(endpoint).FilterUdpPacketOrBytes, timeout, false, out remoteEndPoint, out localEP).Packet;
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the local listening endpoint to received bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// return an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the remote listening endpoint to received bytes.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp server is not start, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an IPEndPoint.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client specified by endpoint cannot be found!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client specified by endpoint is null!
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout, object localEndPoint, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            if (this.listeners.Count == 0)
            {
                throw new InvalidOperationException("udp server is not start, must invoke Start() first.");
            }

            IPEndPoint endpoint = localEndPoint as IPEndPoint;
            if (endpoint == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint.", "remoteEndPoint");
            }

            object localEP = null;

            return this.GetFilteredPacket(endpoint, timeout, false, out remoteEndPoint, out localEP);
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
                    throw new ObjectDisposedException("UdpServerTransport");
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
                throw new ObjectDisposedException("UdpServerTransport");
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
                throw new ObjectDisposedException("UdpServerTransport");
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
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            try
            {
                object localEP = null;
                object remoteEP = null;

                // decode packet
                StackPacket packet = this.GetFilteredPacket(null, timeout, false, out remoteEP, out localEP);

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
        /// If disposing equals true, Managed and unmanaged resources are disposed. if false, Only unmanaged resources 
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
                    if (this.listeners != null)
                    {
                        // stop listener if exists.
                        if (this.listeners.Count > 0)
                        {
                            this.Stop();
                        }
                        this.listeners = null;
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
        ~UdpServerTransport()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer, UdpClient or UdpServer.
        /// </summary>
        /// <param name="filter">
        /// a Filter delegate that specifies the endpoint to receive data.<para/>
        /// if null, receive data from any endpoint.
        /// </param>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="removeEvent">
        /// a bool value that indicates whether need to remove the event from buffer.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        /// <returns>
        /// a UdpReceivedBytes object that specifies the received udp packet.<para/>
        /// never return null, if no packets, throw exception.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the received udp packet is invalid
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when exception arrived when expect bytes from udp client.
        /// </exception>
        private UdpReceivedBytes GetUdpBytes(
            Filter<UdpReceivedBytes> filter, TimeSpan timeout, bool removeEvent,
            out object remoteEndPoint, out object localEndPoint)
        {
            UdpReceivedBytes bytes = null;

            if (filter == null)
            {
                bytes = this.buffer.Dequeue(timeout);
            }
            else
            {
                bytes = this.buffer.Dequeue(timeout, filter);
            }

            if (bytes == null)
            {
                throw new InvalidOperationException("the received udp packet is invalid");
            }

            remoteEndPoint = bytes.RemoteEndPoint;
            localEndPoint = bytes.LocalEndPoint;

            // exception event arrived
            if (bytes.Packet == null)
            {
                if (!removeEvent)
                {
                    this.buffer.Enqueue(bytes);
                }

                InvalidOperationException exc =
                        new InvalidOperationException("exception arrived when expect bytes from udp client.");

                // identify this exception.
                exc.Data.Add(exc.GetType().Name, true);

                throw exc;
            }

            return bytes;
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer, UdpClient or UdpServer.
        /// </summary>
        /// <param name="endpoint">
        /// an IPEndPoint object that specifies the endpoint to get packet.
        /// </param>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="removeEvent">
        /// a bool value that indicates whether need to remove the event from buffer.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.<para/>
        /// never return null, if no packets, throw exception.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when udp server failed to decode udp packet
        /// </exception>
        private StackPacket GetFilteredPacket(
            IPEndPoint endpoint, TimeSpan timeout, bool removeEvent,
            out object remoteEndPoint, out object localEndPoint)
        {
            // generate the filter.
            Filter<IPEndPointStackPacket> packetFilter = null;
            Filter<UdpReceivedBytes> bytesFilter = null;
            if (endpoint != null)
            {
                packetFilter = new TransportFilter(endpoint).FilterUdpPacketOrBytes;
                bytesFilter = new TransportFilter(endpoint).FilterUdpPacketOrBytes;
            }

            // get the packet in packet list.
            IPEndPointStackPacket packet = Utility.GetOne<IPEndPointStackPacket>(this.packetCache, packetFilter);
            if (packet != null)
            {
                remoteEndPoint = packet.RemoteEndPoint;
                localEndPoint = packet.LocalEndPoint;

                return packet.Packet;
            }

            UdpReceivedBytes bytes = this.GetUdpBytes(
                bytesFilter, timeout, removeEvent, out remoteEndPoint, out localEndPoint);

            // decode packets using data in buffer.
            int consumedLength = 0;
            int expectedLength = 0;

            StackPacket[] packets = this.decoder(remoteEndPoint, bytes.Packet, out consumedLength, out expectedLength);

            // if no packet, drop the recieved data and continue.
            if (packets == null || packets.Length == 0)
            {
                throw new InvalidOperationException("udp server failed to decode udp packet");
            }

            // if packet arrived, add to packet list, and return the first.
            foreach (StackPacket item in packets)
            {
                this.packetCache.Enqueue(
                    new IPEndPointStackPacket(item, bytes.RemoteEndPoint, bytes.LocalEndPoint));
            }

            // set timeout to zero. when packet is decoded, must not wait.
            return GetFilteredPacket(endpoint, new TimeSpan(), removeEvent, out remoteEndPoint, out localEndPoint);
        }

        #endregion
    }
}
