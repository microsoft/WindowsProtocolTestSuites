// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Quic;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    internal class QuicServerConnection : IVisitorQuicReceiveLoop, IDisposable
    {
        #region Fields

        private QuicConnection connection;

        private QuicStream stream;

        private QuicServerTransport server;

        private ThreadManager thread;

        private BytesBuffer buffer;

        private IPEndPoint remoteEndPoint;

        private IPEndPoint localEndPoint;

        private bool isServerStopped;

        private bool lspHooked;

        #endregion

        #region Properties

        public QuicStream Stream
        {
            get
            {
                ValidateConnectionDisposed();

                return this.stream;
            }
        }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                ValidateConnectionDisposed();

                return this.remoteEndPoint;
            }
        }

        public IPEndPoint LocalEndPoint
        {
            get
            {
                ValidateConnectionDisposed();

                return this.localEndPoint;
            }
        }

        public bool IsServerStopped
        {
            get
            {
                ValidateConnectionDisposed();

                return this.isServerStopped;
            }
        }

        public bool IsLspHooked
        {
            get
            {
                return this.lspHooked;
            }
        }

        #endregion

        #region Constructor

        public QuicServerConnection(
            QuicConnection quicConnection,
            QuicServerTransport quicServerTransport,
            IPEndPoint lspHookedLocalEP,
            bool isLspHooked)
        {
            if (quicConnection == null)
            {
                throw new ArgumentNullException(nameof(quicConnection));
            }

            if (quicServerTransport == null)
            {
                throw new ArgumentNullException(nameof(quicServerTransport));
            }

            this.server = quicServerTransport;

            this.connection = quicConnection;

            this.stream = quicConnection.AcceptInboundStreamAsync().GetAwaiter().GetResult();

            this.thread = new ThreadManager(QuicServerConnectionReceiveLoop, Unblock);

            this.buffer = new BytesBuffer();

            this.localEndPoint = quicConnection.LocalEndPoint as IPEndPoint;

            this.remoteEndPoint = quicConnection.RemoteEndPoint as IPEndPoint;
        }

        #endregion

        #region Methods

        public void Start()
        {
            ValidateConnectionDisposed();

            this.thread.Start();

            this.isServerStopped = false;
        }

        public void Stop()
        {
            ValidateConnectionDisposed();

            this.isServerStopped = true;

            this.thread.Stop();
        }

        public void SendBytes(byte[] message)
        {
            ValidateConnectionDisposed();

            this.stream.Write(message, 0, message.Length);
        }

        public byte[] ExpectBytes(TimeSpan timeout, int maxCount)
        {
            ValidateConnectionDisposed();

            return ExpectBytesVisitor.Visit(this.buffer, timeout, maxCount);
        }

        public StackPacket[] ExpectPacket(TimeSpan timeout, out int consumedLength)
        {
            ValidateConnectionDisposed();

            return ExpectMultiPacketsVisitor.Visit(
                this.buffer, this.server.Decoder, this.remoteEndPoint, timeout, out consumedLength);
        }

        public void VisitorAddReceivedData(byte[] data)
        {
            ValidateConnectionDisposed();

            this.server.Sequence.Add(this, data, this.buffer);
        }

        public TransportEvent VisitorCreateTransportEvent(EventType type, object detail)
        {
            ValidateConnectionDisposed();

            return new TransportEvent(type, this.remoteEndPoint, this.localEndPoint, detail);
        }

        #endregion

        #region Private Methods

        private void ValidateConnectionDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(QuicServerConnection));
            }
        }

        private void QuicServerConnectionReceiveLoop()
        {
            try
            {
                Utility.SafelyOperation(this.QuicServerConnectionReceiveLoopImp);
            }
            finally
            {
                buffer.Close();
            }
        }

        private void QuicServerConnectionReceiveLoopImp()
        {
            QuicReceiveLoopVisitor.Visit(this, this.server, this.stream, this.thread, this.server.SocketConfig.BufferSize);
        }

        private void Unblock()
        {
            if (this.stream != null)
            {
                this.stream.Close();
            }
        }

        #endregion

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
                    this.disposed = true;
                }
            }
        }

        ~QuicServerConnection()
        {
            Dispose(false);
        }

        #endregion
    }
}
