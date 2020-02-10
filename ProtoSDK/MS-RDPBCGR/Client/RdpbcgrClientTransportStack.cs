// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    public class RdpbcgrClientTransportStack
    {
        #region Fields

        /// <summary>
        /// the identity of stream.
        /// </summary>
        private static int endpointIdentity = 1;

        /// <summary>
        /// the identity of stream.
        /// </summary>
        private static int EndpointIdentity
        {
            get
            {
                return endpointIdentity++;
            }
        }

        /// <summary>
        /// a StreamConfig object that contains the config.
        /// </summary>
        private StreamConfig streamConfig;

        /// <summary>
        /// a Stream object that specifies the underlayer transport.<para/>
        /// when connect to server, it will be constructed; <para/>
        /// when disconnect or dispose, it will be close and set to null.
        /// </summary>
        private Stream stream;

        /// <summary>
        /// a ThreadManager object that contains the received thread.<para/>
        /// when connect to server, it will be constructed;<para/>
        /// when disconnect or dispose, it will be stop and set to null.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// Used to stop the thread
        /// </summary>
        private bool stopThread = false;

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
        /// buffer for received data
        /// </summary>
        private List<byte> buffer;

        /// <summary>
        /// an int value that specifies the local endpoint.
        /// </summary>
        private int localEndPoint;

        /// <summary>
        /// a PacketFilter that is used to filter the packet.
        /// </summary>
        public PacketFilter PacketFilter;

        /// <summary>
        /// A boolean to avoid double disconnect.
        /// </summary>
        private bool disconnected = false;

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
        /// thrown when transportConfig is not StreamConfig
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when transportConfig is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when decodePacketCallback is null.
        /// </exception>
        public RdpbcgrClientTransportStack(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
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
            buffer = new List<byte>();
            this.eventQueue = new SyncFilterQueue<TransportEvent>();
            this.packetCache = new SyncFilterQueue<StackPacket>();

            this.decoder = decodePacketCallback;
        }

        #endregion

        #region IConnectable Members

        /// <summary>
        /// connect to remote endpoint.<para/>
        /// the underlayer transport must be TcpClient, UdpClient or NetbiosClient.
        /// </summary>
        /// <returns>
        /// the remote endpoint of the connection.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when tcp client is connected to server.
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
                throw new ObjectDisposedException("StreamTransport");
            }

            if (this.thread != null)
            {
                throw new InvalidOperationException("stream client is connected.");
            }

            this.eventQueue.Clear();
            this.packetCache.Clear();

            this.localEndPoint = RdpbcgrClientTransportStack.EndpointIdentity;

            this.stream = this.streamConfig.Stream;

            this.thread = new Thread(StreamReceiveLoop);
            this.thread.IsBackground = true;
            this.thread.Name = "StreamReceiveLoop() of RdpbcgrClientTransportStack";
            this.stopThread = false;
            this.thread.Start();

            return localEndPoint;

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
                    throw new ObjectDisposedException("StreamTransport");
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
                throw new ObjectDisposedException("StreamTransport");
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
        /// <param name="auth">An action which authenticates with SUT.</param>
        /// <exception cref="ArgumentException">
        /// thrown when transportConfig is not StreamTransport
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when config is null.
        /// </exception>
        public void UpdateConfig(TransportConfig config, Action auth = null)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("StreamTransport");
            }

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            buffer = new List<byte>();
            StreamConfig theStreamConfig = config as StreamConfig;

            if (theStreamConfig == null)
            {
                throw new ArgumentException("transportConfig must be StreamConfig", "config");
            }

            this.streamConfig = theStreamConfig;

            // if the decoder is null, the instance has not initialized
            // do not terminate the thread and return directly.
            if (this.decoder == null || this.packetCache == null)
            {
                return;
            }

            #region update the underlayer stream.

            // abort the blocked receive thread.
            EndThread();

            // Authenticate with SUT.
            if (auth != null)
            {
                auth();
            }

            // reconnect to received data from new stream.
            // if the stream is null, just stop the receiver of stream.
            if (this.streamConfig.Stream != null)
            {
                this.Connect();
            }

            #endregion
        }

        private void EndThread()
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    stopThread = true;
                }

                thread.Join();

                thread = null;
            }
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
        /// thrown when stream client is not connected, must invoke Connect() first.
        /// </exception>
        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("StreamTransport");
            }

            if (this.thread == null)
            {
                throw new InvalidOperationException(
                    "stream client is not connected, must invoke Connect() first.");
            }

            // decode packet
            StackPacket packet = this.ExpectPacket(timeout);

            // if packet is decoded, return it.
            if (packet != null)
            {
                return new TransportEvent(
                    EventType.ReceivedPacket, null, this.localEndPoint, packet);
            }

            // if no packet, and there is event coming, return event.
            // set timeout to zero, must not wait for event coming.
            return eventQueue.Dequeue(new TimeSpan());
        }

        #endregion

        #region IDisconnectable Members

        /// <summary>
        /// disconnect from remote host.<para/>
        /// the underlayer transport must be TcpClient, NetbiosClient, TcpServer or NetbiosServer.<para/>
        /// client side will disconnect the connection to server.<para/>
        /// server side will disconnect all client connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when stream client is not connected, must invoke Connect() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the received thread does not initialize.
        /// </exception>
        public void Disconnect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("StreamTransport");
            }

            if (disconnected)
            {
                return;
            }

            disconnected = true;

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "stream client is not connected, must invoke Connect() first.");
            }

            if (this.thread == null)
            {
                throw new InvalidOperationException("the received thread does not initialize.");
            }

            this.stream.Close();

            EndThread();

            this.stream.Dispose();
            this.stream = null;
        }


        /// <summary>
        /// expect the server to disconnect<para/>
        /// the underlayer transport must be TcpClient, NetbiosClient, TcpServer or NetbiosServer.<para/>
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
        /// thrown when stream client is not connected, must invoke Connect() first.
        /// </exception>
        public object ExpectDisconnect(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("StreamTransport");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "stream client is not connected, must invoke Connect() first.");
            }

            this.eventQueue.Dequeue(timeout, FilterDisconnected);

            return this.localEndPoint;
        }

        #endregion

        #region ISource Members

        /// <summary>
        /// Send a packet over the transport.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when packet is null
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when stream client is not connected, must invoke Connect() first.
        /// </exception>
        public void SendPacket(StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("StreamTransport");
            }

            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            this.SendBytes(packet.ToBytes());
        }


        /// <summary>
        /// Send an arbitrary message over the transport.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="message">
        /// a byte array that contains the data to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when stream client is not connected, must invoke Connect() first.
        /// </exception>
        public void SendBytes(byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("StreamTransport");
            }

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "stream client is not connected, must invoke Connect() first.");
            }

            this.stream.Write(message, 0, message.Length);
        }

        /// <summary>
        /// expect packet from transport.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when stream client is not connected, must invoke Connect() first.
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("StreamTransport");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "stream client is not connected, must invoke Connect() first.");
            }


            StackPacket packet = packetCache.Dequeue(timeout);
            return packet;

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
                    // Disconnect with SUT.
                    Disconnect();

                    // Free managed resources & other reference types:
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
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~RdpbcgrClientTransportStack()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void StreamReceiveLoop()
        {
            try
            {
                this.StreamReceiveLoopImp();
            }
            catch (Exception ex)
            {
                // Log unhandled exception by adding an exception event.
                this.AddEvent(new TransportEvent(EventType.Exception, null, this.localEndPoint, ex));
            }
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void StreamReceiveLoopImp()
        {
            byte[] data = new byte[this.streamConfig.BufferSize];

            NetworkStream networkStream = this.stream as NetworkStream;

            while (!this.stopThread)
            {
                int receivedLength = 0;

                // read event
                try
                {
                    // received data from server.
                    receivedLength = this.stream.Read(data, 0, data.Length);

                    // if the server close the stream, return.
                    if (receivedLength == 0)
                    {
                        this.AddEvent(new TransportEvent(
                            EventType.Disconnected, null, this.localEndPoint, null));

                        break;
                    }

                    this.buffer.AddRange(ArrayUtility.SubArray<byte>(data, 0, receivedLength));

                    int expectedLength = 0;

                    int consumedLength = 0;

                    bool errorPduReceived = false;

                    while (buffer.Count > 0)
                    {
                        StackPacket[] packets = decoder(localEndPoint, buffer.ToArray(), out consumedLength, out expectedLength);

                        if (packets == null)
                        {
                            break;
                        }

                        foreach (StackPacket packet in packets)
                        {
                            packetCache.Enqueue(packet);
                        }

                        if (packets.Any(packet => packet is ErrorPdu))
                        {
                            errorPduReceived = true;
                        }

                        if (consumedLength > 0)
                        {
                            buffer.RemoveRange(0, consumedLength);
                        }
                    }

                    if (errorPduReceived)
                    {
                        // Exit since ErrorPdu is received from decoder. 
                        break;
                    }
                }
                catch (Exception ex)
                {
                    bool handled = false;

                    bool exit = false;

                    if (ex is IOException ioException)
                    {
                        if (ioException.InnerException is SocketException socketException)
                        {
                            if (socketException.SocketErrorCode == SocketError.ConnectionReset)
                            {
                                // Add the disconnected transport event, if connection is reset by SUT.
                                this.AddEvent(new TransportEvent(EventType.Disconnected, null, this.localEndPoint, null));

                                handled = true;

                                exit = true;
                            }

                            if (socketException.SocketErrorCode == SocketError.WouldBlock)
                            {
                                // No data was received within receive timeout.
                                handled = true;
                            }
                        }
                    }

                    if (!handled)
                    {
                        // Throw to outside handler.
                        throw;
                    }

                    if (exit)
                    {
                        break;
                    }
                }
            }
        }


        private bool FilterDisconnected(TransportEvent obj)
        {
            if (obj != null && obj.EventType == EventType.Disconnected)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
