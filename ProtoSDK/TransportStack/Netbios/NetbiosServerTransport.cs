// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the netbios server transport, support features of netbios server.<para/>
    /// support listening on multiple ports.<para/>
    /// support receive/send message with specified client.
    /// </summary>
    internal class NetbiosServerTransport : INetbiosServer, IVisitorGetAnyData, IVisitorGetAnyBytes
    {
        #region Fields

        /// <summary>
        /// a NetbiosTransportConfig object that contains the config for netbios server.
        /// </summary>
        private NetbiosTransportConfig netbiosConfig;

        /// <summary>
        /// a DecodePacketCallback delegate that specifies the decoder.
        /// </summary>
        private DecodePacketCallback decoder;

        /// <summary>
        /// a Dictionary&lt;string, NetbiosServerListener&gt; that specifies the listener of netbios server.<para/>
        /// initialize at constructor. never be null util disposed. maybe empty.
        /// </summary>
        private Dictionary<string, NetbiosServerListener> listeners;

        /// <summary>
        /// a Dictionary&lt;int, NetbiosServerConnection&gt; that contains the netbios connections.<para/>
        /// initialize at constructor, clear at disconnect/expect disconnect. never be null util disposed. maybe empty.
        /// </summary>
        private Dictionary<int, NetbiosServerConnection> connections;

        /// <summary>
        /// a SyncFilterQueue&lt;TransportEvent&gt; that contains the event.<para/>
        /// initialize at constructor. never be null util disposed. maybe empty.
        /// </summary>
        private SyncFilterQueue<TransportEvent> eventQueue;

        /// <summary>
        /// a DataSequence object that manages the sequence of received data.<para/>
        /// initialize at constructor. never be null util disposed.
        /// </summary>
        private DataSequence sequence;

        /// <summary>
        /// a PacketCache&lt;NetbiosServerStackPacket&gt; that stores the packet from client.<para/>
        /// initialize at constructor.never be null.
        /// </summary>
        private SyncFilterQueue<NetbiosServerStackPacket> packetCache;

        /// <summary>
        /// a bool value that indicates whether server is started.
        /// </summary>
        private bool started;

        #endregion

        #region Properties

        /// <summary>
        /// get a DataSequence object that manages the sequence of received data.<para/>
        /// initialize at constructor. never be null.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public DataSequence Sequence
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("NetbiosServerTransport");
                }

                return this.sequence;
            }
        }


        /// <summary>
        /// get a DecodePacketCallback delegate that specifies the decoder.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public DecodePacketCallback Decoder
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("NetbiosServerTransport");
                }

                return this.decoder;
            }
        }


        /// <summary>
        /// get a NetbiosTransportConfig object that contains the config for netbios server.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public NetbiosTransportConfig NetbiosConfig
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("NetbiosServerTransport");
                }

                return this.netbiosConfig;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// consturctor.
        /// </summary>
        /// <param name="transportConfig">
        /// a TransportConfig object that contains the config.
        /// </param>
        /// <param name="decodePacketCallback">
        /// a DecodePacketCallback delegate that is used to decode the packet from bytes.
        /// </param>
        /// <exception cref="ArgumentException">
        /// thrown when transportConfig is not NetbiosTransportConfig
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when transportConfig is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when decodePacketCallback is null.
        /// </exception>
        public NetbiosServerTransport(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
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

            this.decoder = decodePacketCallback;
            this.connections = new Dictionary<int, NetbiosServerConnection>();
            this.eventQueue = new SyncFilterQueue<TransportEvent>();
            this.sequence = new DataSequence();
            this.packetCache = new SyncFilterQueue<NetbiosServerStackPacket>();
            this.listeners = new Dictionary<string, NetbiosServerListener>();
        }

        #endregion

        #region IStartable Members

        /// <summary>
        /// to start the transport. <para/>
        /// the underlayer transport must be NetbiosServer, UdpServer or NetbiosServer.<para/>
        /// if config did not specify the local netbios name, return directly.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the local netbios name in config is null.
        /// </exception>
        public void Start()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            // if config did not specify the local netbios name, return directly.
            if (this.netbiosConfig.LocalNetbiosName == null)
            {
                throw new InvalidOperationException("the local netbios name in config is null");
            }

            this.Start(this.netbiosConfig.LocalNetbiosName);
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
        /// thrown when localEndPoint is not a string.
        /// </exception>
        public void Start(object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerTransport");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("localEndPoint");
            }

            string endpoint = localEndPoint as string;
            if (endpoint == null)
            {
                throw new ArgumentException("localEndPoint is not a string.", "localEndPoint");
            }

            NetbiosServerListener listener = null;

            if (this.listeners.ContainsKey(endpoint))
            {
                listener = this.listeners[endpoint];
            }
            else
            {
                listener = new NetbiosServerListener(endpoint, this);
                this.listeners[endpoint] = listener;
            }

            listener.Start();

            this.started = true;
        }


        /// <summary>
        /// stop all listener of server.<para/>
        /// the underlayer transport must be NetbiosServer, UdpServer or NetbiosServer.
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
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (this.listeners.Count <= 0)
            {
                return;
            }

            // get all endpoint to stop.
            string[] endpoints = new string[this.listeners.Count];

            this.listeners.Keys.CopyTo(endpoints, 0);

            // stop the specified endpoint.
            foreach (string endpoint in endpoints)
            {
                this.Stop(endpoint);
            }
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
        /// thrown when the specified endpoint is not a string.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the listener specified by endpoint cannot be found.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the listener specified by endpoint is null.
        /// </exception>
        public void Stop(object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("localEndPoint");
            }

            string endpoint = localEndPoint as string;
            if (endpoint == null)
            {
                throw new ArgumentException("the specified endpoint is not a string.", "localEndPoint");
            }

            if (!this.listeners.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the listener specified by endpoint cannot be found!");
            }

            NetbiosServerListener listener = this.listeners[endpoint];

            if (listener == null)
            {
                throw new InvalidOperationException("the listener specified by endpoint is null!");
            }

            listener.Stop();
        }

        #endregion

        #region IAcceptable Members

        /// <summary>
        /// expect connection from client.<para/>
        /// the underlayer transport must be NetbiosServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout.
        /// </param>
        /// <returns>
        /// an object that contains the connection.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when invalid connected event.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when received endpoint is not an int value
        /// </exception>
        public object ExpectConnect(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            TransportEvent transportEvent = Utility.Dequeue(
                this.eventQueue, this.sequence, timeout, new TransportFilter().FilterConnected);

            if (transportEvent == null)
            {
                throw new InvalidOperationException("invalid connected event");
            }

            if (!(transportEvent.EndPoint is int))
            {
                throw new InvalidOperationException("received endpoint is not an int value");
            }

            return (int)transportEvent.EndPoint;
        }


        /// <summary>
        /// expect the server to disconnect<para/>
        /// the underlayer transport must be NetbiosServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies which endpoint is expected to be disconnected.
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
        /// thrown when remote endpoint is not an int value
        /// </exception>
        public void ExpectDisconnect(TimeSpan timeout, object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            if (!(remoteEndPoint is int))
            {
                throw new ArgumentException("remote endpoint is not an int value", "remoteEndPoint");
            }

            Utility.Dequeue(this.eventQueue, this.sequence,
                timeout, new TransportFilter(remoteEndPoint).FilterEndPointDisconnected);
        }


        /// <summary>
        /// disconnect from a special remote host.<para/>
        /// the underlayer transport must be NetbiosServer or NetbiosServer.
        /// </summary>
        /// <param name="remoteEndPoint">
        /// an object that specifies the endpoint to be disconnected.
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
        /// thrown when remote endpoint is not an int value
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client specified by endpoint cannot be found!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint is null!
        /// </exception>
        public void Disconnect(object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            if (!(remoteEndPoint is int))
            {
                throw new ArgumentException("remote endpoint is not an int value", "remoteEndPoint");
            }

            int endpoint = (int)remoteEndPoint;

            if (!this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the client specified by endpoint cannot be found!");
            }

            NetbiosServerConnection connection = this.connections[endpoint];

            if (connection == null)
            {
                throw new InvalidOperationException("the connection specified by endpoint is null!");
            }

            connection.Stop();

            this.connections.Remove(endpoint);

            this.sequence.Remove(connection);

            connection.Dispose();
        }

        #endregion

        #region IDisconnectable Members

        /// <summary>
        /// disconnect from remote host.<para/>
        /// the underlayer transport must be NetbiosClient, NetbiosClient, NetbiosServer or NetbiosServer.<para/>
        /// client side will disconnect the connection to server.<para/>
        /// server side will disconnect all client connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        public void Disconnect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (this.connections.Count <= 0)
            {
                return;
            }

            // get all endpoint to disconnect.
            int[] endpoints = new int[this.connections.Count];

            this.connections.Keys.CopyTo(endpoints, 0);

            // stop the specified endpoint.
            foreach (int endpoint in endpoints)
            {
                this.Disconnect(endpoint);
            }

            this.connections.Clear();
        }


        /// <summary>
        /// expect the server to disconnect<para/>
        /// the underlayer transport must be NetbiosClient, NetbiosClient, NetbiosServer or NetbiosServer.<para/>
        /// client side will expect the disconnection from server.<para/>
        /// server side will expect the disconnection from any client.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <returns>
        /// return an object that is disconnected. client return null.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when invalid connected event.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when received endpoint is not an int value
        /// </exception>
        public object ExpectDisconnect(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            TransportEvent transportEvent = Utility.Dequeue(
                this.eventQueue, this.sequence, timeout, new TransportFilter().FilterDisconnected);

            if (transportEvent == null)
            {
                throw new InvalidOperationException("invalid connected event");
            }

            if (!(transportEvent.EndPoint is int))
            {
                throw new InvalidOperationException("received endpoint is not an int value");
            }

            return (int)transportEvent.EndPoint;
        }

        #endregion

        #region ISpecialTarget Members

        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be NetbiosServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that indicates the connection to received data from.
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
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when remote endpoint is not an int value
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint cannot be found!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint is null!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection is closed, there is no data anymore.
        /// </exception>
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            if (!(remoteEndPoint is int))
            {
                throw new ArgumentException("remote endpoint is not an int value", "remoteEndPoint");
            }

            int endpoint = (int)remoteEndPoint;

            if (!this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the connection specified by endpoint cannot be found!");
            }

            NetbiosServerConnection connection = this.connections[endpoint];

            if (connection == null)
            {
                throw new InvalidOperationException("the connection specified by endpoint is null!");
            }

            byte[] data = connection.ExpectBytes(timeout, maxCount);

            this.sequence.Consume(connection, data.Length);

            return data;
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be NetbiosServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the connection to expect packet.
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
        /// <exception cref="ArgumentException">
        /// thrown when the specified endpoint is not an int value.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint cannot be found!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint is null!
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout, object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            if (!(remoteEndPoint is int))
            {
                throw new ArgumentException("remote endpoint is not an int value", "remoteEndPoint");
            }

            int endpoint = (int)remoteEndPoint;

            if (!this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the connection specified by endpoint cannot be found!");
            }

            // if there are packets in cache, check cache first.
            NetbiosServerStackPacket packet = Utility.GetOne<NetbiosServerStackPacket>(
                this.packetCache, new TransportFilter(endpoint).FilterNetbiosEndPoint);

            if (packet != null)
            {
                return packet.Packet;
            }

            NetbiosServerConnection connection = this.connections[endpoint];

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
                    new NetbiosServerStackPacket(item, connection.RemoteEndPoint, connection.LocalEndPoint));
            }

            return this.ExpectPacket(timeout, remoteEndPoint);
        }

        #endregion

        #region ITarget Members

        /// <summary>
        /// Send a packet to a special remote host.<para/>
        /// the transport must be a NetbiosServer, UdpClient or UdpServer.
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
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when remote endpoint is not an int value
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client specified by endpoint cannot be found!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client specified by endpoint is null!
        /// </exception>
        public void SendPacket(object remoteEndPoint, StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            this.SendBytes(remoteEndPoint, packet.ToBytes());
        }


        /// <summary>
        /// Send arbitrary message to a special remote host.<para/>
        /// the transport must be a NetbiosServer, UdpClient or UdpServer.
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
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when remoteEndPoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when remote endpoint is not an int value
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint cannot be found!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint is null!
        /// </exception>
        public void SendBytes(object remoteEndPoint, byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            if (remoteEndPoint == null)
            {
                throw new ArgumentNullException("remoteEndPoint");
            }

            if (!(remoteEndPoint is int))
            {
                throw new ArgumentException("remote endpoint is not an int value", "remoteEndPoint");
            }

            int endpoint = (int)remoteEndPoint;

            if (!this.connections.ContainsKey(endpoint))
            {
                throw new InvalidOperationException("the connection specified by endpoint cannot be found!");
            }

            NetbiosServerConnection connection = this.connections[endpoint];

            if (connection == null)
            {
                throw new InvalidOperationException("the connection specified by endpoint is null!");
            }

            connection.SendBytes(message);
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be a NetbiosServer, UdpClient or UdpServer.
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
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            object localEndPoint;

            return this.ExpectBytes(timeout, maxCount, out remoteEndPoint, out localEndPoint);
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be a NetbiosServer, UdpClient or UdpServer.
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
        /// thrown when the item in data sequence is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection in data sequence is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when invalid data received from client.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when received an invalid data sequence item.
        /// </exception>
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint, out object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            return GetAnyBytesVisitor.Visit(
                this, this.sequence, timeout, maxCount, out remoteEndPoint, out localEndPoint);
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a NetbiosServer, UdpClient or UdpServer.
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
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            object localEndPoint;

            return ExpectPacket(timeout, out remoteEndPoint, out localEndPoint);
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a NetbiosServer, UdpClient or UdpServer.
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
        /// thrown when the item in data sequence is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client in data sequence is disposed.
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint, out object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            TransportEvent transportEvent = this.GetPacketFromAnyClient(timeout, true);

            remoteEndPoint = transportEvent.RemoteEndPoint;
            localEndPoint = transportEvent.LocalEndPoint;

            return transportEvent.EventObject as StackPacket;
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
                    throw new ObjectDisposedException("NetbiosServerTransport");
                }

                return this.sequence.DataLength > 0 || this.packetCache.Count > 0;
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
        /// thrown when transportEvent is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the RemoteEndPoint of disconnected event is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the specified endpoint is not an int value.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint cannot be found!
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection specified by endpoint is null!
        /// </exception>
        public void AddEvent(TransportEvent transportEvent)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

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

                if (!(transportEvent.RemoteEndPoint is int))
                {
                    throw new InvalidOperationException("received endpoint is not an int value");
                }

                int endpoint = (int)transportEvent.RemoteEndPoint;

                if (!this.connections.ContainsKey(endpoint))
                {
                    throw new InvalidOperationException("the connection specified by endpoint cannot be found!");
                }

                NetbiosServerConnection connection = this.connections[endpoint];

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


        /// <summary>
        /// to update the config of transport at runtime.
        /// </summary>
        /// <param name="config">
        /// a TransportConfig object that contains the config to update
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when transportConfig is not NetbiosTransportConfig.
        /// </exception>
        public void UpdateConfig(TransportConfig config)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            NetbiosTransportConfig netbiosTransportConfig = config as NetbiosTransportConfig;

            if (netbiosTransportConfig == null)
            {
                throw new ArgumentException("transportConfig must be NetbiosTransportConfig", "config");
            }

            this.netbiosConfig = netbiosTransportConfig;
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
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the server is not started, must invoke Start() first.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// thrown when decode packet timeout and no event arrived.
        /// </exception>
        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            if (!this.started)
            {
                throw new InvalidOperationException("server is not started, must invoke Start() first.");
            }

            while (true)
            {
                // decode packet
                TransportEvent packet = this.GetPacketFromAnyClient(timeout, false);

                if (packet != null)
                {
                    return packet;
                }
            }
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
                    // because the client uses listener as transport,
                    // so must stop listener, then dispose client, at last dispose listener.
                    if (this.connections != null)
                    {
                        // stop to accept clients, if exists listener.
                        if (this.listeners.Count > 0)
                        {
                            this.Stop();
                        }
                        // disconnect and dispose all clients, if exists.
                        if (this.connections.Count > 0)
                        {
                            this.Disconnect();
                        }
                        this.connections = null;
                    }
                    // dispose listeners after diposed all clients.
                    if (this.listeners != null)
                    {
                        // dispose all listener if exists.
                        foreach (NetbiosServerListener listener in this.listeners.Values)
                        {
                            listener.Dispose();
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
                    if (this.sequence != null)
                    {
                        this.sequence.Dispose();
                        this.sequence = null;
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
        ~NetbiosServerTransport()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// accept the connected NetbiosClient.
        /// </summary>
        /// <param name="localEP">
        /// an int value that specifies the local listening netbios session id.
        /// </param>
        /// <param name="remoteEP">
        /// an int value that specifies the connected netbios client session id.
        /// </param>
        /// <param name="transport">
        /// a NetbiosTransport object that specifies the owner of client.
        /// </param>
        /// <returns>
        /// a NetbiosServerConnection that specifies the connection.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when invalid connected netbios client.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when invalid endpoint of connected netbios client
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client specified by endpoint exists
        /// </exception>
        internal NetbiosServerConnection AcceptClient(int localEP, int remoteEP, NetbiosTransport transport)
        {
            if (this.connections.ContainsKey(remoteEP))
            {
                throw new InvalidOperationException("the client specified by endpoint exists");
            }

            NetbiosServerConnection connection = new NetbiosServerConnection(localEP, remoteEP, this, transport);

            this.connections.Add(remoteEP, connection);

            return connection;
        }


        /// <summary>
        /// get the endpoint from the security sequence item owner.<para/>
        /// this delegate is used by TcpServer and NetbiosServer that manages the data sequence.
        /// </summary>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of bytes to get.
        /// </param>
        /// <param name="source">
        /// an object that specifies the source of item.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        public byte[] GetBytes(int maxCount, object source, out object remoteEndPoint, out object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            NetbiosServerConnection connection = source as NetbiosServerConnection;

            if (connection == null)
            {
                throw new InvalidOperationException("the item in data sequence is invalid");
            }

            if (connection.RemoteEndPoint < 0)
            {
                throw new InvalidOperationException("the connection in data sequence is disposed");
            }

            remoteEndPoint = connection.RemoteEndPoint;
            localEndPoint = connection.LocalEndPoint;

            // set timeout to zero, if data is recieved, must not wait.
            return connection.ExpectBytes(new TimeSpan(), maxCount);
        }


        /// <summary>
        /// get the endpoint from the security sequence item owner.<para/>
        /// this delegate is used by TcpServer and NetbiosServer that manages the data sequence.
        /// </summary>
        /// <param name="owner">
        /// an object that specifies the owner of item.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void VisitorGetEndPoint(object owner, out object remoteEndPoint, out object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            NetbiosServerConnection connection = owner as NetbiosServerConnection;

            if (connection == null)
            {
                throw new InvalidOperationException("the item in data sequence is invalid");
            }

            if (connection.RemoteEndPoint < 0)
            {
                throw new InvalidOperationException("the connection in data sequence is disposed");
            }

            remoteEndPoint = connection.RemoteEndPoint;
            localEndPoint = connection.LocalEndPoint;
        }


        /// <summary>
        /// decode packet from the security sequence item owner<para/>
        /// this delegate is used by TcpServer and NetbiosServer that manages the data sequence.
        /// </summary>
        /// <param name="owner">
        /// an object that specifies the owner of item.
        /// </param>
        /// <param name="consumedLength">
        /// an int value that specifies the consumed length of decoder.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        /// <returns>
        /// the decoded packet.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public StackPacket VisitorDecodePackets(
            object owner, object remoteEndPoint, object localEndPoint, out int consumedLength)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerTransport");
            }

            NetbiosServerConnection connection = owner as NetbiosServerConnection;

            // set timeout to zero, must not wait for more data.
            // if timeout, process next.
            StackPacket[] packets = connection.ExpectPacket(new TimeSpan(), out consumedLength);

            if (packets == null)
            {
                return null;
            }

            // more than one packet.
            foreach (StackPacket packet in packets)
            {
                this.packetCache.Enqueue(
                    new NetbiosServerStackPacket(packet, connection.RemoteEndPoint, connection.LocalEndPoint));
            }

            return Utility.GetOne<NetbiosServerStackPacket>(this.packetCache, null).Packet;
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a NetbiosServer, UdpClient or UdpServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="skipEvent">
        /// a bool value that specifies whether skip the event.<para/>
        /// if true, just wait for packet coming; otherwise, both data and event will return.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.<para/>
        /// if all buffer is closed in this while, and required to return if all buffer is closed, return null.<para/>
        /// otherwise never return null, if no packets coming in timespan, throw exception.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the item in data sequence is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the client in data sequence is disposed.
        /// </exception>
        private TransportEvent GetPacketFromAnyClient(TimeSpan timeout, bool skipEvent)
        {
            NetbiosServerStackPacket packet = Utility.GetOne<NetbiosServerStackPacket>(this.packetCache, null);

            if(packet != null)
            {
                return new TransportEvent(
                    EventType.ReceivedPacket, packet.RemoteEndPoint, packet.LocalEndPoint, packet.Packet);
            }

            return GetAnyDataVisitor.Visit(this, this.eventQueue, this.sequence, timeout, skipEvent);
        }

        #endregion
    }
}
