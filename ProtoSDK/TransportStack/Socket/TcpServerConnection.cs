// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// tcp server connection represents the connection between server and client.<para/>
    /// start a thread to accept data from tcp client.
    /// </summary>
    internal class TcpServerConnection : IDisposable, IVisitorTcpReceiveLoop
    {
        #region Fields

        /// <summary>
        /// a Stream object that specifies the underlayer transport stream.<para/>
        /// if DirectTcp, it's the stream of TcpClient.GetStream().<para/>
        /// if Tcp over Ssl, it's SslStream.
        /// </summary>
        private Stream stream;

        /// <summary>
        /// a TcpServerTransport that represents the owner of listener.
        /// </summary>
        private TcpServerTransport server;

        /// <summary>
        /// a ThreadManager object that specifies the thread to receive data from client.
        /// </summary>
        private ThreadManager thread;

        /// <summary>
        /// a BytesBuffer object that specifies the buffer of client.<para/>
        /// never be null util disposed.
        /// </summary>
        private BytesBuffer buffer;

        /// <summary>
        /// an IPEndPoint int object that specifies the remote endpoint.
        /// </summary>
        private IPEndPoint remoteEndPoint;

        /// <summary>
        /// an IPEndPoint object that specifies the local endpoint.
        /// </summary>
        private IPEndPoint localEndPoint;

        /// <summary>
        /// a bool value that indicates that whether the server is stopped.
        /// </summary>
        private bool isServerStopped;

        /// <summary>
        /// a bool value that indicates whether lsp hooked the transport.
        /// </summary>
        private bool lspHooked;

        /// <summary>
        /// if a connection is lsp hooked, in intercept mode, there will be an associated forwarder channel 
        /// </summary>
        private TcpServerConnection associatedForwarderChannel;

        /// <summary>
        /// a bool value indicating whether this is a forwarder channel
        /// </summary>
        private bool isAForwarderChannel;

        #endregion

        #region Properties

        /// <summary>
        /// get a Stream object that specifies the underlayer transport stream.<para/>
        /// if DirectTcp, it's the stream of TcpClient.GetStream().<para/>
        /// if Tcp over Ssl, it's SslStream.
        /// </summary>
        public Stream Stream
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TcpServerConnection");
                }

                return this.stream;
            }
        }


        /// <summary>
        /// an IPEndPoint int object that specifies the remote endpoint.
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TcpServerConnection");
                }

                return this.remoteEndPoint;
            }
        }


        /// <summary>
        /// an IPEndPoint object that specifies the local endpoint.
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TcpServerConnection");
                }

                return this.localEndPoint;
            }
        }


        /// <summary>
        /// get a bool value that indicates that whether the server is stopped.
        /// </summary>
        public bool IsServerStopped
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TcpServerConnection");
                }

                return this.isServerStopped;
            }
        }


        /// <summary>
        /// get a bool value that indicates whether lsp hooked the transport.<para/>
        /// if the port required to listen at is already listened, LSP will hook it and IsLspHooked is true.<para/>
        /// if the port is available, LSP is disabled and IsLspHooked is false.
        /// </summary>
        internal bool IsLspHooked
        {
            get
            {
                return this.lspHooked;
            }
        }


        /// <summary>
        /// Get and set the forwarder channel associated with this channel
        /// </summary>
        internal TcpServerConnection AssociatedForwarderChannel
        {
            set
            {
                this.associatedForwarderChannel = value;
            }
        }


        /// <summary>
        /// A boolean indicating whether this is a forwarder channel
        /// </summary>
        internal bool IsAForwarderChannel
        {
            get
            {
                return this.isAForwarderChannel;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tcpClient">
        /// a TcpClient object that specifies the tcp client.
        /// </param>
        /// <param name="tcpServerTransport">
        /// a TcpServerTransport that represents the owner of listener.
        /// </param>
        /// <param name="lspHookedLocalEP">
        /// an IPEndPoint object that specifies the local endpoint.<para/>
        /// if LSP hooked, return the required local endpoint.<para/>
        /// otherwise, return the actual listened local endpoint.
        /// </param>
        /// <param name="isLspHooked">
        /// a bool value that indicates whether lsp hooked the transport.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when the tcpClient is null!
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when the tcpServerTransport is null!
        /// </exception>
        public TcpServerConnection(
            TcpClient tcpClient, TcpServerTransport tcpServerTransport, IPEndPoint lspHookedLocalEP, bool isLspHooked)
        {
            if (tcpClient == null)
            {
                throw new ArgumentNullException("tcpClient");
            }

            if (tcpServerTransport == null)
            {
                throw new ArgumentNullException("tcpServerTransport");
            }

            this.isAForwarderChannel = false;
            this.lspHooked = isLspHooked;
            this.stream = tcpClient.GetStream();
            this.server = tcpServerTransport;
            this.thread = new ThreadManager(TcpServerConnectionReceiveLoop, Unblock);
            this.buffer = new BytesBuffer();

            if (isLspHooked)
            {
                IPEndPoint destinationEndpoint;
                this.localEndPoint = lspHookedLocalEP;
                this.remoteEndPoint = LspConsole.Instance.RetrieveRemoteEndPoint(tcpClient.Client, out destinationEndpoint);

                //if equals, it means this is a forwarder channel.
                if (remoteEndPoint.Equals(destinationEndpoint))
                {
                    isAForwarderChannel = true;
                }
            }
            else
            {
                this.localEndPoint = tcpClient.Client.LocalEndPoint as IPEndPoint;
                this.remoteEndPoint = tcpClient.Client.RemoteEndPoint as IPEndPoint;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// start the received thread to received data from client.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Start()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerConnection");
            }

            // if using ssl.
            if (this.server.SslProvider != null)
            {
                SslStream sslStream = new SslStream(this.stream);

                if (this.server.SslProvider.EnabledSslProtocols == SslProtocols.None)
                {
                    sslStream.AuthenticateAsServer(this.server.SslProvider.Certificate);
                }
                else
                {
                    sslStream.AuthenticateAsServer(
                        this.server.SslProvider.Certificate, this.server.SslProvider.ClientCertificateRequired,
                        this.server.SslProvider.EnabledSslProtocols, false);
                }

                this.stream = sslStream;
            }

            this.thread.Start();

            this.isServerStopped = false;
        }


        /// <summary>
        /// stop the receive thread to prepare to disconnect it.
        /// </summary>
        public void Stop()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerConnection");
            }

            this.isServerStopped = true;

            this.thread.Stop();

            if (associatedForwarderChannel != null)
            {
                this.associatedForwarderChannel.Stop();
            }
        }


        /// <summary>
        /// Send an arbitrary message over the transport.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="message">
        /// a bytes array that contains the data to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when trying to send bytes on a readonly channel
        /// </exception>
        public void SendBytes(byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerConnection");
            }

            if (isAForwarderChannel)
            {
                throw new InvalidOperationException("This is a readonly channel");
            }

            this.stream.Write(message, 0, message.Length);
        }


        /// <summary>
        /// to receive bytes from connection.
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
        public byte[] ExpectBytes(TimeSpan timeout, int maxCount)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerConnection");
            }

            return ExpectBytesVisitor.Visit(this.buffer, timeout, maxCount);
        }


        /// <summary>
        /// expect packet from transport.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="consumedLength">
        /// return an int value that specifies the consumed length.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public StackPacket[] ExpectPacket(TimeSpan timeout, out int consumedLength)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerConnection");
            }

            return ExpectMultiPacketsVisitor.Visit(
                this.buffer, this.server.Decoder, this.remoteEndPoint, timeout, out consumedLength);
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
                    if (this.stream != null)
                    {
                        this.stream.Close();
                        this.stream = null;
                    }
                    if (this.buffer != null)
                    {
                        this.buffer.Dispose();
                        this.buffer = null;
                    }

                    if (associatedForwarderChannel != null)
                    {
                        this.associatedForwarderChannel.Dispose();
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
        ~TcpServerConnection()
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
                throw new ObjectDisposedException("TcpServerConnection");
            }

            this.server.Sequence.Add(this, data, this.buffer);
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
                throw new ObjectDisposedException("TcpServerConnection");
            }

            if (!isAForwarderChannel)
            {
                return new TransportEvent(type, this.remoteEndPoint, this.localEndPoint, detail);
            }
            //for forwarder channel, swap the endpoints
            else
            {
                return new TransportEvent(type, this.localEndPoint, this.remoteEndPoint, detail);
            }
        }


        /// <summary>
        /// received data from client.
        /// </summary>
        private void TcpServerConnectionReceiveLoop()
        {
            try
            {
                // donot arise any exception in the work thread.
                Utility.SafelyOperation(this.TcpServerConnectionReceiveLoopImp);
            }
            finally
            {
                // this operation is safe, it will never arise exception.
                // because in this function, the thread is not end, so the buffer is not disposed.
                buffer.Close();
            }
        }


        /// <summary>
        /// received data from client.
        /// </summary>
        private void TcpServerConnectionReceiveLoopImp()
        {
            TcpReceiveLoopVisitor.Visit(this, this.server, this.stream, this.thread, this.server.SocketConfig.BufferSize);
        }


        /// <summary>
        /// unblock the received thread.
        /// </summary>
        private void Unblock()
        {
            if (this.stream != null)
            {
                this.stream.Close();
            }
        }

        #endregion
    }
}
