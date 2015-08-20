// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the transport for Tcp Client.<para/>
    /// it is reusable, supports connect, disconnect, and then connect mode.
    /// </summary>
    internal class TcpClientTransport : ITcpClient, IVisitorTcpReceiveLoop
    {
        #region Fields

        /// <summary>
        /// a SocketTransportConfig object that contains the config.
        /// </summary>
        private SocketTransportConfig socketConfig;

        /// <summary>
        /// a Stream object that specifies the underlayer transport stream.<para/>
        /// if DirectTcp, it's the stream of TcpClient.GetStream().<para/>
        /// if Tcp over Ssl, it's SslStream.
        /// </summary>
        private Stream stream;

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
        /// a ClientSslProvider object that presents the ssl security provider.<para/>
        /// initialize it when invoke SslStartup.<para/>
        /// when dispose and disconnect, close it and set to null.
        /// </summary>
        private ClientSslProvider sslProvider;

        /// <summary>
        /// an EndPoint int object that specifies the remote endpoint.
        /// </summary>
        private EndPoint remoteEndPoint;

        /// <summary>
        /// an EndPoint object that specifies the local endpoint.
        /// </summary>
        private EndPoint localEndPoint;
        
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
        public TcpClientTransport(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
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
        /// <exception cref="InvalidOperationException">
        /// thrown when the remote ip address is null.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public object Connect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream != null)
            {
                throw new InvalidOperationException("tcp client is connected to server.");
            }

            if (this.thread != null)
            {
                throw new InvalidOperationException("the received thread does not cleanup.");
            }

            if (this.socketConfig.RemoteIpAddress == null)
            {
                throw new InvalidOperationException("the remote ip address is null.");
            }

            this.buffer = new BytesBuffer();

            this.eventQueue.Clear();
            this.packetCache.Clear();

            TcpClient tcpClient;

            if (this.socketConfig.LocalIpAddress != null)
            {
                tcpClient = new TcpClient(new IPEndPoint(this.socketConfig.LocalIpAddress, 0));
            }
            else
            {
                tcpClient = new TcpClient(this.socketConfig.RemoteIpAddress.AddressFamily);
            }

            // The Timeout be set to TimeSpan.Zero if it is not initialized.
            if (this.socketConfig.Timeout != TimeSpan.Zero)
            {
                IAsyncResult result = tcpClient.BeginConnect(this.socketConfig.RemoteIpAddress, this.socketConfig.RemoteIpPort, new AsyncCallback(TcpConnectCallback), tcpClient);
                result.AsyncWaitHandle.WaitOne(this.socketConfig.Timeout, true);
                if (!tcpClient.Connected)
                {                             
                    throw new TimeoutException(string.Format(CultureInfo.InvariantCulture, "Failed to connect server {0}:{1} within {2} milliseconds.", this.socketConfig.RemoteIpAddress,this.socketConfig.RemoteIpPort, this.socketConfig.Timeout.TotalMilliseconds));
                }
            }
            else
            {
                tcpClient.Connect(new IPEndPoint(this.socketConfig.RemoteIpAddress, this.socketConfig.RemoteIpPort));
            }
            this.localEndPoint = tcpClient.Client.LocalEndPoint;
            this.remoteEndPoint = tcpClient.Client.RemoteEndPoint;
            this.stream = tcpClient.GetStream();

            if (this.sslProvider != null)
            {
                SslStream sslStream = new SslStream(this.stream);

                if (this.sslProvider.Certificate == null)
                {
                    sslStream.AuthenticateAsClient(this.sslProvider.TargetHost);
                }
                else
                {
                    sslStream.AuthenticateAsClient(this.sslProvider.TargetHost,
                        new X509CertificateCollection(new X509Certificate[] { this.sslProvider.Certificate }),
                        this.sslProvider.EnabledSslProtocols, false);
                }

                this.stream = sslStream;
            }

            this.thread = new ThreadManager(TcpClientReceiveLoop, UnblockReceiveThread);
            this.thread.Start();

            return this.localEndPoint;
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
                    throw new ObjectDisposedException("TcpClientTransport");
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
                throw new ObjectDisposedException("TcpClientTransport");
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
                throw new ObjectDisposedException("TcpClientTransport");
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
        /// thrown when tcp client is not connected to server, must invoke Connect() first.
        /// </exception>
        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "tcp client is not connected to server, must invoke Connect() first.");
            }

            // decode packet
            StackPacket packet = this.ExpectPacket(timeout);

            // if packet is decoded, return it.
            if (packet != null)
            {
                return new TransportEvent(
                    EventType.ReceivedPacket, this.localEndPoint, this.remoteEndPoint, this.localEndPoint, packet);
            }

            // eventQueue == null which should only happens in dispose during disconnect
            // Temporally adding wait loop would avoid calling on the null object
            while (eventQueue == null)
            {
                System.Threading.Thread.Sleep(100);
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
        /// thrown when tcp client is not connected to server, must invoke Connect() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the received thread does not initialize.
        /// </exception>
        public void Disconnect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "tcp client is not connected to server, must invoke Connect() first.");
            }

            if (this.thread == null)
            {
                throw new InvalidOperationException("the received thread does not initialize.");
            }

            this.thread.Dispose();
            this.thread = null;

            this.stream.Close();
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
        /// thrown when tcp client is not connected to server, must invoke Connect() first.
        /// </exception>
        public object ExpectDisconnect(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "tcp client is not connected to server, must invoke Connect() first.");
            }

            this.eventQueue.Dequeue(timeout, new TransportFilter().FilterDisconnected);

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
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when tcp client is not connected to server, must invoke Connect() first.
        /// </exception>
        public void SendPacket(StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            this.SendBytes(packet.ToBytes());
        }


        /// <summary>
        /// Send an arbitrary message over the transport.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="message">
        /// a bytes array that contains the data to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when tcp client is not connected to server, must invoke Connect() first.
        /// </exception>
        public void SendBytes(byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "tcp client is not connected to server, must invoke Connect() first.");
            }

            this.stream.Write(message, 0, message.Length);
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
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
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when tcp client is not connected to server, must invoke Connect() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection is closed, there is no data anymore.
        /// </exception>
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "tcp client is not connected to server, must invoke Connect() first.");
            }

            return ExpectBytesVisitor.Visit(this.buffer, timeout, maxCount);
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
        /// thrown when tcp client is not connected to server, must invoke Connect() first.
        /// </exception>
        public StackPacket ExpectPacket(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream == null)
            {
                throw new InvalidOperationException(
                    "tcp client is not connected to server, must invoke Connect() first.");
            }

            return ExpectSinglePacketVisitor.Visit(
                this.buffer, this.decoder, this.localEndPoint, timeout, this.packetCache);
        }

        #endregion

        #region ISslClient Members

        /// <summary>
        /// complete the SSL/TLS authenticate<para/>
        /// which is used to start the SSL/TLS handshake with the server,<para/>
        /// verify the certificate from server,<para/>
        /// establish the SSL/TLS security.<para/>
        /// the transport must be TcpClient.
        /// </summary>
        /// <param name="targetHost">
        /// a string that indicates the name of the server that shares the SSL/TLS.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when client has connected to server, must startup ssl before Connect().
        /// </exception>
        public void Startup(string targetHost)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream != null)
            {
                throw new InvalidOperationException(
                    "client has connected to server, must startup ssl before Connect().");
            }

            this.sslProvider = new ClientSslProvider(targetHost);
        }

        /// <summary>
        /// client authenticate over SSL/TLS with the server.<para/>
        /// the transport must be TcpClient.
        /// </summary>
        /// <param name="targetHost">
        /// a string that indicates the name of the server that shares the SSL/TLS.
        /// </param>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the client.
        /// </param>
        /// <param name="enabledSslProtocols">
        /// The SslProtocols value that represents the protocol used for authentication.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when client has connected to server, must startup ssl before Connect().
        /// </exception>
        public void Startup(string targetHost, X509Certificate certificate, SslProtocols enabledSslProtocols)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            if (this.stream != null)
            {
                throw new InvalidOperationException(
                    "client has connected to server, must startup ssl before Connect().");
            }

            this.sslProvider = new ClientSslProvider(targetHost, certificate, enabledSslProtocols);
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
                    if (this.stream != null)
                    {
                        this.stream.Close();
                        this.stream = null;
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
        ~TcpClientTransport()
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
                throw new ObjectDisposedException("TcpClientTransport");
            }

            this.buffer.Add(data);
        }


        /// <summary>
        /// create a transport event.<para/>
        /// TcpClient uses localEndPoint as EndPoint.<para/>
        /// while TcpServer uses remoteEndPointas EndPoint.
        /// </summary>
        /// <param name="type">
        /// an EventType enum that specifies the event type.
        /// </param>
        /// <param name="detail">
        /// an object that contains the event object.
        /// </param>
        /// <returns>
        /// a TransportEvent object that specifies the created event.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public TransportEvent VisitorCreateTransportEvent(EventType type, object detail)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpClientTransport");
            }

            return new TransportEvent(type, this.localEndPoint, this.remoteEndPoint, this.localEndPoint, detail);
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void TcpClientReceiveLoop()
        {
            try
            {
                // donot arise any exception in the work thread.
                Utility.SafelyOperation(this.TcpClientReceiveLoopImp);
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
        private void TcpClientReceiveLoopImp()
        {
            TcpReceiveLoopVisitor.Visit(this, this, this.stream, this.thread, this.socketConfig.BufferSize);
        }


        /// <summary>
        /// the method to unblock the received thread.
        /// </summary>
        private void UnblockReceiveThread()
        {
            if (this.stream != null)
            {
                this.stream.Close();
            }
        }

        /// <summary>
        /// This call back is used to handle the completion of the prior asynchronous BeginConnect() call.
        /// </summary>
        /// <param name="asyncResult">Async connection result.</param>
        private static void TcpConnectCallback(IAsyncResult asyncResult)
        {
            TcpClient tcpClient = (TcpClient)asyncResult.AsyncState;
            if (tcpClient != null)
            {
                try
                {
                    tcpClient.EndConnect(asyncResult);
                }
                catch
                {
                    // Swallow all exceptions thrown in a callback function.
                }
            }
        }
        #endregion
    }
}
