// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Quic2;
using System.Net.Security;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    internal class QuicServerTransport : IQuicServer, IVisitorGetAnyData, IVisitorGetAnyBytes
    {
        #region Fields

        private DataSequence sequence;

        private DecodePacketCallback decoder;

        private QuicServerConfig socketConfig;

        private SyncFilterQueue<TransportEvent> eventQueue;

        private Dictionary<IPEndPoint, QuicServerListener> listeners;

        private Dictionary<IPEndPoint, QuicServerConnection> connections;

        private SyncFilterQueue<IPEndPointStackPacket> packetCache;

        private bool started;

        private SslApplicationProtocol ApplicationProtocol = new("smb");

        private const int MAX_BIDIRECTIONAL_STREAMS = 100;
        private const int MAX_UNIRECTIONAL_STREAMS = 100;

        #endregion

        #region Properties

        public QuicServerConfig SocketConfig
        {
            get
            {
                ValidateServerTransport();

                return this.socketConfig;
            }
        }

        public DecodePacketCallback Decoder
        {
            get
            {
                ValidateServerTransport();

                return this.decoder;
            }
        }

        public DataSequence Sequence
        {
            get
            {
                ValidateServerTransport();

                return this.sequence;
            }
        }

        #endregion

        #region Constructor

        public QuicServerTransport(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
        {
            if (transportConfig == null)
            {
                throw new ArgumentNullException(nameof(transportConfig));
            }

            if (decodePacketCallback == null)
            {
                throw new ArgumentNullException(nameof(decodePacketCallback));
            }

            this.UpdateConfig(transportConfig);

            this.decoder = decodePacketCallback;

            this.listeners = new Dictionary<IPEndPoint, QuicServerListener>();

            this.connections = new Dictionary<IPEndPoint, QuicServerConnection>();

            this.eventQueue = new SyncFilterQueue<TransportEvent>();

            this.sequence = new DataSequence();

            this.packetCache = new SyncFilterQueue<IPEndPointStackPacket>();
        }

        #endregion

        #region IStartable Members

        public void Start()
        {
            ValidateServerTransport();

            IPAddress localAddress = this.socketConfig.LocalIpAddress;

            IPEndPoint endpoint = new IPEndPoint(localAddress, this.socketConfig.LocalIpPort);

            this.Start(endpoint);
        }

        public void Start(object localEndPoint)
        {
            ValidateServerTransport();

            if (localEndPoint == null)
            {
                throw new ArgumentNullException(nameof(localEndPoint));
            }

            IPEndPoint requiredLocalEP = localEndPoint as IPEndPoint;

            if (requiredLocalEP == null)
            {
                var config = new SocketTransportConfig()
                {
                    LocalIpAddress = this.socketConfig.LocalIpAddress,
                    LocalIpPort = this.socketConfig.LocalIpPort,
                    RemoteIpAddress = this.socketConfig.RemoteIpAddress,
                    RemoteIpPort = this.socketConfig.RemoteIpPort,
                };
                requiredLocalEP = Utility.GetEndPointByPort(config, localEndPoint);
            }

            if (this.listeners.ContainsKey(requiredLocalEP))
            {
                throw new InvalidOperationException("the specified endpoint has been started.");
            }

            bool isLspHooked;

            bool isBlocking;

            // get the replaced endpoint from LSP.
            IPEndPoint actualListenedLocalEP = LspConsole.Instance.GetReplacedEndPoint(
                this.socketConfig.Type, requiredLocalEP, out isLspHooked, out isBlocking);

            QuicListener serverListener = new QuicListener(GetListenerOptions(this.socketConfig.ServerCertificate));
            QuicServerListener listener =
                new QuicServerListener(serverListener, this, isLspHooked);

            listener.Start(isBlocking, requiredLocalEP);

            this.listeners[requiredLocalEP] = listener;

            this.started = true;
        }

        public void Stop()
        {
            ValidateServerTransportState();

            if (this.listeners.Count == 0)
            {
                return;
            }

            IPEndPoint[] endpoints = new IPEndPoint[this.listeners.Count];

            this.listeners.Keys.CopyTo(endpoints, 0);

            foreach(IPEndPoint endpoint in endpoints)
            {
                this.Stop(endpoint);
            }

            this.listeners.Clear();
        }

        public void Stop(object localEndPoint)
        {
            ValidateServerTransportState();

            if (localEndPoint == null)
            {
                throw new ArgumentNullException(nameof(localEndPoint));
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

            QuicServerListener listener = this.listeners[endpoint];

            if (listener == null)
            {
                throw new InvalidOperationException("The listener specified by endpoint is null!");
            }

            listener.Stop();

            this.listeners.Remove(endpoint);
        }

        #endregion

        #region IAcceptable Members

        public object ExpectConnect(TimeSpan timeout)
        {
            ValidateServerTransportState();

            TransportEvent transportEvent = Utility.Dequeue(this.eventQueue, this.sequence, timeout, new TransportFilter().FilterConnected);

            if (transportEvent == null)
            {
                throw new InvalidOperationException("Invalid connected event.");
            }

            IPEndPoint endpoint = transportEvent.EndPoint as IPEndPoint;

            if (endpoint == null)
            {
                throw new InvalidOperationException("Invalid endpoint of connected quic connection");
            }

            return endpoint;
        }

        public void ExpectDisconnect(TimeSpan timeout, object remoteEndPoint)
        {
            ValidateServerTransportState();

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException(nameof(remoteEndPoint));
            }

            IPEndPoint endpoint = remoteEndPoint as IPEndPoint;

            if (endpoint == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint", nameof(remoteEndPoint));
            }

            Utility.Dequeue(this.eventQueue, this.sequence, timeout, new TransportFilter().FilterEndPointDisconnected);
        }

        public void Disconnect(object remoteEndPoint)
        {
            ValidateServerTransportState();

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException(nameof(remoteEndPoint));
            }

            IPEndPoint endpoint = remoteEndPoint as IPEndPoint;

            if (endpoint == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint", nameof(remoteEndPoint));
            }

            if (!this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("The connection specified by endpoint cannot be found!");
            }

            QuicServerConnection connection = this.connections[endpoint];

            if (connection == null)
            {
                throw new InvalidOperationException("The connection specified by endpoint is null!");
            }

            connection.Stop();

            this.connections.Remove(endpoint);

            this.sequence.Remove(endpoint);

            if (connection.IsLspHooked)
            {
                LspConsole.Instance.Disconnect(connection.LocalEndPoint, endpoint, this.socketConfig.Type);
            }

            connection.Dispose();
        }

        #endregion

        #region IDisconnectable Members

        public void Disconnect()
        {
            ValidateServerTransportState();

            if (this.connections.Count <= 0)
            {
                return;
            }

            IPEndPoint[] endpoints = new IPEndPoint[this.connections.Count];

            this.connections.Keys.CopyTo(endpoints, 0);

            foreach (IPEndPoint endpoint in endpoints)
            {
                this.Disconnect(endpoint);
            }

            this.connections.Clear();
        }

        public object ExpectDisconnect(TimeSpan timeout)
        {
            ValidateServerTransportState();

            TransportEvent transportEvent = Utility.Dequeue(
                this.eventQueue, this.sequence, timeout, new TransportFilter().FilterDisconnected);

            if (transportEvent == null)
            {
                throw new InvalidOperationException("invalid connected event");
            }

            IPEndPoint endpoint = transportEvent.EndPoint as IPEndPoint;

            if (endpoint == null)
            {
                throw new InvalidOperationException("the disconnected client endpoint is invalid");
            }

            return endpoint;
        }

        #endregion

        #region ISpecialTarget Members

        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, object remoteEndPoint)
        {
            ValidateServerTransportState();

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            IPEndPoint endpoint = remoteEndPoint as IPEndPoint;
            if (endpoint == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint.", "remoteEndPoint");
            }

            if (!this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the connection specified by endpoint cannot be found!");
            }

            QuicServerConnection connection = this.connections[endpoint];

            if (connection == null)
            {
                throw new InvalidOperationException("the connection specified by endpoint is null!");
            }

            byte[] data = connection.ExpectBytes(timeout, maxCount);

            this.sequence.Consume(connection, data.Length);

            return data;
        }

        public StackPacket ExpectPacket(TimeSpan timeout, object remoteEndPoint)
        {
            ValidateServerTransportState();

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            IPEndPoint endpoint = remoteEndPoint as IPEndPoint;
            if (endpoint == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint.", "remoteEndPoint");
            }

            if (!this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the connection specified by endpoint cannot be found!");
            }

            // if there are packets in cache, check cache first.
            IPEndPointStackPacket packet = Utility.GetOne<IPEndPointStackPacket>(
                this.packetCache, new TransportFilter(endpoint).FilterIPEndPoint);

            if (packet != null)
            {
                return packet.Packet;
            }

            QuicServerConnection connection = this.connections[endpoint];

            if (connection == null)
            {
                throw new InvalidOperationException("the connection specified by endpoint is null!");
            }

            // decode from specified client.
            int consumedLength;
            StackPacket[] packets = connection.ExpectPacket(timeout, out consumedLength);

            // remove the sequence information.
            this.sequence.Consume(connection, consumedLength);

            if (packets == null)
            {
                return null;
            }

            foreach (StackPacket item in packets)
            {
                this.packetCache.Enqueue(
                    new IPEndPointStackPacket(item, endpoint, connection.LocalEndPoint as IPEndPoint));
            }

            return this.ExpectPacket(timeout, remoteEndPoint);
        }

        #endregion

        public byte[] GetBytes(int maxCount, object source, out object remoteEndPoint, out object localEndPoint)
        {
            throw new NotImplementedException();
        }

        public void VisitorGetEndPoint(object owner, out object remoteEndPoint, out object localEndPoint)
        {
            throw new NotImplementedException();
        }

        public StackPacket VisitorDecodePackets(object owner, object remoteEndPoint, object localEndPoint, out int consumedLength)
        {
            throw new NotImplementedException();
        }

        

        public void SendPacket(object remoteEndPoint, StackPacket packet)
        {
            ValidateServerTransportState();

            this.SendBytes(remoteEndPoint, packet.ToBytes());
        }

        public void SendBytes(object remoteEndPoint, byte[] message)
        {
            ValidateServerTransportState();

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            IPEndPoint endpoint = remoteEndPoint as IPEndPoint;
            if (endpoint == null)
            {
                throw new ArgumentException("the specified endpoint is not an IPEndPoint.", "remoteEndPoint");
            }

            if (!this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the connection specified by endpoint cannot be found!");
            }

            QuicServerConnection connection = this.connections[endpoint];

            if (connection == null)
            {
                throw new InvalidOperationException("the connection specified by endpoint is null!");
            }

            connection.SendBytes(message);
        }

        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint)
        {
            ValidateServerTransportState();

            object localEndPoint;

            return this.ExpectBytes(timeout, maxCount, out remoteEndPoint, out localEndPoint);
        }

        public StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint)
        {
            throw new NotImplementedException();
        }

        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint, out object localEndPoint)
        {
            ValidateServerTransportState();

            return GetAnyBytesVisitor.Visit(
                this, this.sequence, timeout, maxCount, out remoteEndPoint, out localEndPoint);
        }

        public StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint, out object localEndPoint)
        {
            ValidateServerTransportState();

            TransportEvent transportEvent = this.GetDataFromAnyClient(timeout, true);

            remoteEndPoint = transportEvent.RemoteEndPoint;
            localEndPoint = transportEvent.LocalEndPoint;

            return transportEvent.EventObject as StackPacket;
        }

        public bool IsDataAvailable
        {
            get
            {
                ValidateServerTransport();

                return this.sequence.DataLength > 0 || this.packetCache.Count > 0;
            }
        }


        public void AddEvent(TransportEvent transportEvent)
        {
            ValidateServerTransport();

            if (transportEvent == null)
            {
                throw new ArgumentNullException("transportEvent");
            }

            if (transportEvent.EventType == EventType.Disconnected)
            {
                if (transportEvent.RemoteEndPoint == null)
                {
                    throw new InvalidOperationException("the RemoteEndPoint of disconnected event is null");
                }

                IPEndPoint endpoint = transportEvent.RemoteEndPoint as IPEndPoint;
                if (endpoint == null)
                {
                    throw new InvalidOperationException("the specified endpoint is not an IPEndPoint.");
                }

                if (!this.connections.ContainsKey(endpoint))
                {
                    throw new InvalidOperationException("the connection specified by endpoint cannot be found!");
                }

                QuicServerConnection connection = this.connections[endpoint];

                if (connection == null)
                {
                    throw new InvalidOperationException("the connection specified by endpoint is null!");
                }

                // if the disconnected event is triggered by server, skip it.
                if (connection.IsServerStopped)
                {
                    return;
                }
            }

            Utility.Enqueue(this.eventQueue, this.sequence, transportEvent);
        }

        public void UpdateConfig(TransportConfig config)
        {
            ValidateServerTransport();

            QuicServerConfig socketTransportConfig = config as QuicServerConfig;

            if (socketTransportConfig == null)
            {
                throw new ArgumentException("transportConfig must be QuicServerConfig", "config");
            }

            this.socketConfig = socketTransportConfig;
        }

        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            ValidateServerTransportState();

            while (true)
            {
                // decode packet
                TransportEvent packet = this.GetDataFromAnyClient(timeout, false);

                if (packet != null)
                {
                    return packet;
                }
            }
        }

        #region IDisposable Members

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    disposed = true;
                }
            }
        }

        ~QuicServerTransport()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        private void ValidateServerTransport()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(QuicServerTransport));
            }
        }

        private void ValidateServerStarted()
        {
            if (!this.started)
            {
                throw new InvalidOperationException("Server is not started, must invoke Start() first.");
            }
        }

        private void ValidateServerTransportState()
        {
            ValidateServerTransport();

            ValidateServerStarted();
        }

        private QuicListenerOptions GetListenerOptions(X509Certificate2 serverCertificate)
        {
            QuicListenerOptions quicListenerOptions = new QuicListenerOptions()
            {
                ListenEndPoint = new IPEndPoint(this.socketConfig.LocalIpAddress, 0),
                ServerAuthenticationOptions = new SslServerAuthenticationOptions()
                {
                    ApplicationProtocols = new List<SslApplicationProtocol>() { ApplicationProtocol },
                    ServerCertificate = serverCertificate
                }
            };

            quicListenerOptions.MaxBidirectionalStreams = MAX_BIDIRECTIONAL_STREAMS;
            quicListenerOptions.MaxUnidirectionalStreams = MAX_UNIRECTIONAL_STREAMS;

            return quicListenerOptions;
        }

        internal QuicServerConnection AcceptClient(QuicConnection clientConnection, IPEndPoint lspHookedLocal, bool isLspHooked)
        {
            if (clientConnection == null)
            {
                throw new ArgumentNullException(nameof(clientConnection));
            }

            QuicServerConnection connection = new QuicServerConnection(clientConnection, this, lspHookedLocal, isLspHooked);

            IPEndPoint endpoint = connection.RemoteEndPoint;

            if (endpoint == null)
            {
                throw new InvalidOperationException("Invalid endpoint of connected quic connection");
            }

            if (this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the client specified by endpoint exists");
            }

            this.connections.Add(endpoint, connection);

            return connection;
        }

        private TransportEvent GetDataFromAnyClient(TimeSpan timeout, bool skipEvent)
        {
            IPEndPointStackPacket packet = Utility.GetOne<IPEndPointStackPacket>(this.packetCache, null);
            if (packet != null)
            {
                return new TransportEvent(
                    EventType.ReceivedPacket, packet.RemoteEndPoint, packet.LocalEndPoint, packet.Packet);
            }

            return GetAnyDataVisitor.Visit(this, this.eventQueue, this.sequence, timeout, skipEvent);
        }

        #endregion
    }
}
