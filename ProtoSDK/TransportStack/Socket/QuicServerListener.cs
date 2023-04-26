// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Quic;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    internal class QuicServerListener : IDisposable
    {
        #region Fields

        private QuicListener listener;

        private QuicServerTransport server;

        private ThreadManager thread;

        private IPEndPoint lspHookedLocalEP;

        private bool lspHooked;

        private const int intervalForAcceptLoop = 1;

        #endregion

        #region Constructor

        public QuicServerListener(QuicListener quicListener, QuicServerTransport quicServerTransport, bool isLspHooked)
        {
            if (quicListener == null)
            {
                throw new ArgumentNullException(nameof(quicListener));
            }

            if (quicServerTransport == null)
            {
                throw new ArgumentNullException(nameof(quicServerTransport));
            }

            this.lspHooked = isLspHooked;

            this.listener = quicListener;

            this.server = quicServerTransport;

            this.thread = new ThreadManager(AcceptLoop, Unblock);
        }

        #endregion

        #region Methods

        public void Start(bool isBlocking, IPEndPoint requiredLocalEP)
        {
            ValidateListenerDisposed();

            QuicConnection serverConnection = AsyncUtil.RunSync<QuicConnection>(()
                => this.listener.AcceptConnectionAsync().AsTask());

            QuicStream serverStream = AsyncUtil.RunSync<QuicStream>(()
                => serverConnection.AcceptInboundStreamAsync().AsTask());

            if (this.lspHooked)
            {
                IPEndPoint actualListenedLocalEP = this.listener.LocalEndPoint as IPEndPoint;

                LspConsole.Instance.InterceptTraffic(this.server.SocketConfig.Type, isBlocking, requiredLocalEP, actualListenedLocalEP);
            }

            this.lspHookedLocalEP = requiredLocalEP;

            this.thread.Start();
        }

        public void Stop()
        {
            ValidateListenerDisposed();

            this.Dispose();

            if (this.lspHooked)
            {
                LspConsole.Instance.UnblockTraffic(this.lspHookedLocalEP, this.server.SocketConfig.Type);
            }
        }

        #endregion

        #region Private Methods

        private void ValidateListenerDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(QuicServerListener));
            }
        }

        private void AcceptLoop()
        {
            while(!this.thread.ShouldExit)
            {
                try
                {
                    QuicConnection clientConnection = AsyncUtil.RunSync<QuicConnection>(() 
                        => this.listener.AcceptConnectionAsync().AsTask());

                    QuicServerConnection connection =
                        this.server.AcceptClient(clientConnection, this.lspHookedLocalEP, this.lspHooked);

                    this.server.AddEvent(connection.VisitorCreateTransportEvent(EventType.Connected, clientConnection));

                    connection.Start();
                }
                catch(QuicException qex)
                {
                    this.server.AddEvent(new TransportEvent(EventType.Exception, null, qex));
                }
                catch(Exception ex)
                {
                    this.server.AddEvent(new TransportEvent(EventType.Exception, null, ex));
                }
            }
        }

        private void Unblock()
        {
            this.listener.DisposeAsync();
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
                    disposed = true;
                }
            }
        }

        ~QuicServerListener()
        {
            Dispose(false);
        }

        #endregion
    }
}
