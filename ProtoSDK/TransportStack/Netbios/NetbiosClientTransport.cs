// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the transport for Netbios Client.<para/>
    /// it is reusable, supports connect, disconnect, and then connect mode.
    /// </summary>
    internal class NetbiosClientTransport : INetbiosClient, IVisitorNetbiosReceiveLoop
    {
        #region Fields

        /// <summary>
        /// a NetbiosTransportConfig object that contains the config.
        /// </summary>
        private NetbiosTransportConfig netbiosConfig;

        /// <summary>
        /// a NetbiosTransport object that specifies the underlayer transport.<para/>
        /// when connect to server, it will be constructed; <para/>
        /// when disconnect or dispose, it will be close and set to null.
        /// </summary>
        private NetbiosTransport transport;

        /// <summary>
        /// a ThreadManager object that contains the received thread.<para/>
        /// when connect to server, it will be constructed;<para/>
        /// when disconnect or dispose, it will be stop and set to null.
        /// </summary>
        private ThreadManager thread;

        /// <summary>
        /// a BytesBuffer object that stores the data received from server.<para/>
        /// initialize at constructor/connect to server, set to null when dispose.
        /// </summary>
        private BytesBuffer buffer;

        /// <summary>
        /// a SyncFilterQueue&lt;TransportEvent&gt; object that contains the event,
        /// such as disconnected and exception.<para/>
        /// clear event queue when connect to server. never be null.
        /// </summary>
        private SyncFilterQueue<TransportEvent> eventQueue;

        /// <summary>
        /// a DecodePacketCallback delegate that is used to decode the packet from bytes.
        /// </summary>
        private DecodePacketCallback decoder;

        /// <summary>
        /// a PacketCache to store the arrived packet.<para/>
        /// clear packet list when connect to server. never be null.
        /// </summary>
        private SyncFilterQueue<StackPacket> packetCache;

        /// <summary>
        /// an int value that specifies the session id of netbios client.<para/>
        /// this session id is used as the local endpoint.
        /// </summary>
        private int remoteEndPoint;

        /// <summary>
        /// an int value that specifies the local endpoint.
        /// </summary>
        private int localEndPoint;

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
        /// thrown when transportConfig is not NetbiosTransportConfig
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when transportConfig is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when decodePacketCallback is null.
        /// </exception>
        public NetbiosClientTransport(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
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

            this.buffer = new BytesBuffer();
            this.eventQueue = new SyncFilterQueue<TransportEvent>();
            this.packetCache = new SyncFilterQueue<StackPacket>();

            this.decoder = decodePacketCallback;
        }

        #endregion

        #region IConnectable Members

        /// <summary>
        /// connect to remote endpoint.<para/>
        /// the underlayer transport must be NetbiosTransport, UdpClient or NetbiosClient.
        /// </summary>
        /// <returns>
        /// the remote endpoint of the connection.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when netbios client is connected to server.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the received thread does not cleanup.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public object Connect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (this.transport != null)
            {
                throw new InvalidOperationException("netbios client is connected to server.");
            }

            if (this.thread != null)
            {
                throw new InvalidOperationException("the received thread does not cleanup.");
            }

            this.buffer = new BytesBuffer();

            this.eventQueue.Clear();
            this.packetCache.Clear();

            this.transport = new NetbiosTransport(
                this.netbiosConfig.LocalNetbiosName, this.netbiosConfig.AdapterIndex, (ushort)this.netbiosConfig.BufferSize,
                (byte)this.netbiosConfig.MaxSessions, (byte)this.netbiosConfig.MaxNames);

            this.remoteEndPoint = this.transport.Connect(this.netbiosConfig.RemoteNetbiosName);
            this.localEndPoint = this.transport.NcbNum;

            this.thread = new ThreadManager(NetbiosClientReceiveLoop, UnblockReceiveThread);
            this.thread.Start();

            return this.remoteEndPoint;
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
                    throw new ObjectDisposedException("NetbiosClientTransport");
                }

                return this.packetCache.Count > 0 || this.buffer.Length > 0;
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
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (transportEvent == null)
            {
                throw new ArgumentNullException("transportEvent");
            }

            this.eventQueue.Enqueue(transportEvent);
        }


        /// <summary>
        /// to update the config of transport at runtime.
        /// </summary>
        /// <param name="config">
        /// a TransportConfig object that contains the config to update
        /// </param>
        /// <exception cref="ArgumentException">
        /// thrown when transportConfig is not NetbiosTransportConfig
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
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            NetbiosTransportConfig netbiosTransportConfig = config as NetbiosTransportConfig;

            if (netbiosTransportConfig == null)
            {
                throw new ArgumentException("config must be NetbiosTransportConfig", "config");
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
        /// thrown when netbios client is not connected to server, must invoke Connect() first.
        /// </exception>
        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "netbios client is not connected to server, must invoke Connect() first.");
            }

            // decode packet
            StackPacket packet = this.ExpectPacket(timeout);

            // if packet is decoded, return it.
            if (packet != null)
            {
                return new TransportEvent(
                    EventType.ReceivedPacket, this.remoteEndPoint, this.localEndPoint, packet);
            }

            // if no packet, and there is event coming, return event.
            // set timeout to zero, must not wait for event coming.
            return eventQueue.Dequeue(new TimeSpan());
        }

        #endregion

        #region IDisconnectable Members

        /// <summary>
        /// disconnect from remote host.<para/>
        /// the underlayer transport must be NetbiosTransport, NetbiosClient, NetbiosServer or NetbiosServer.<para/>
        /// client side will disconnect the connection to server.<para/>
        /// server side will disconnect all client connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when netbios client is not connected to server, must invoke Connect() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the received thread does not initialize.
        /// </exception>
        public void Disconnect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "netbios client is not connected to server, must invoke Connect() first.");
            }

            if (this.thread == null)
            {
                throw new InvalidOperationException("the received thread does not initialize.");
            }

            this.transport.Disconnect(this.remoteEndPoint);

            this.thread.Dispose();
            this.thread = null;

            this.transport.Dispose();
            this.transport = null;
        }


        /// <summary>
        /// expect the server to disconnect<para/>
        /// the underlayer transport must be NetbiosTransport, NetbiosClient, NetbiosServer or NetbiosServer.<para/>
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
        /// thrown when netbios client is not connected to server, must invoke Connect() first.
        /// </exception>
        public object ExpectDisconnect(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "netbios client is not connected to server, must invoke Connect() first.");
            }

            this.eventQueue.Dequeue(timeout, new TransportFilter().FilterDisconnected);

            return this.remoteEndPoint;
        }

        #endregion

        #region ISource Members

        /// <summary>
        /// Send a packet over the transport.<para/>
        /// the underlayer transport must be NetbiosTransport, Stream or NetbiosClient.
        /// </summary>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when packet is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not NetbiosTransport, Stream and NetbiosClient.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when netbios client is not connected to server, must invoke Connect() first.
        /// </exception>
        public void SendPacket(StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            this.SendBytes(packet.ToBytes());
        }


        /// <summary>
        /// Send an arbitrary message over the transport.<para/>
        /// the underlayer transport must be NetbiosTransport, Stream or NetbiosClient.
        /// </summary>
        /// <param name="message">
        /// a bytes array that contains the data to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not NetbiosTransport, Stream and NetbiosClient.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when netbios client is not connected to server, must invoke Connect() first.
        /// </exception>
        public void SendBytes(byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "netbios client is not connected to server, must invoke Connect() first.");
            }

            this.transport.Send(this.remoteEndPoint, message);
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the underlayer transport must be NetbiosTransport, Stream or NetbiosClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not NetbiosTransport, Stream and NetbiosClient.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when netbios client is not connected to server, must invoke Connect() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection is closed, there is no data anymore.
        /// </exception>
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "netbios client is not connected to server, must invoke Connect() first.");
            }

            return ExpectBytesVisitor.Visit(this.buffer, timeout, maxCount);
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the underlayer transport must be NetbiosTransport, Stream or NetbiosClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not NetbiosTransport, Stream and NetbiosClient.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when netbios client is not connected to server, must invoke Connect() first.
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "netbios client is not connected to server, must invoke Connect() first.");
            }

            return ExpectSinglePacketVisitor.Visit(
               this.buffer, this.decoder, this.remoteEndPoint, timeout, this.packetCache);
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
                    if (this.thread != null)
                    {
                        this.thread.Dispose();
                        this.thread = null;
                    }
                    if (this.transport != null)
                    {
                        // the netbios transport may throw exception, donot arise exception.
                        Utility.SafelyDisconnectNetbiosConnection(this.transport, this.remoteEndPoint);
                        this.transport.Dispose();
                        this.transport = null;
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
        ~NetbiosClientTransport()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// add the received data to buffer.
        /// </summary>
        /// <param name="data">
        /// a bytes array that contains the received data.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void VisitorAddReceivedData(byte[] data)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosClientTransport");
            }

            this.buffer.Add(data);
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void NetbiosClientReceiveLoop()
        {
            try
            {
                // donot arise any exception in the work thread.
                Utility.SafelyOperation(this.NetbiosClientReceiveLoopImp);
            }
            finally
            {
                // this operation is safe, it will never arise exception.
                // because in this function, the thread is not end, so the buffer is not disposed.
                buffer.Close();
            }
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void NetbiosClientReceiveLoopImp()
        {
            NetbiosReceiveLoopVisitor.Visit(this, this, this.transport, this.localEndPoint, this.remoteEndPoint, this.thread);
        }


        /// <summary>
        /// the method to unblock the received thread.
        /// </summary>
        private void UnblockReceiveThread()
        {
            // the netbios transport may throw exception, donot arise exception.
            Utility.SafelyDisconnectNetbiosConnection(this.transport, this.remoteEndPoint);
        }

        #endregion
    }
}
