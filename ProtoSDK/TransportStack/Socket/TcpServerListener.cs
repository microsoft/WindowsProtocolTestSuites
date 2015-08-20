// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// tcp server listener represents the listener of server<para/>
    /// start a thread to accept connect from TcpListener.
    /// </summary>
    internal class TcpServerListener : IDisposable
    {
        #region Fields

        /// <summary>
        /// a TcpListener that specifies the listener.<para/>
        /// never be null util disposed.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// a TcpServerTransport that represents the owner of listener.
        /// </summary>
        private TcpServerTransport server;

        /// <summary>
        /// a ThreadManager object that specifies the thread to accept connection from client.
        /// </summary>
        private ThreadManager thread;

        /// <summary>
        /// an IPEndPoint object that specifies the local endpoint.<para/>
        /// if LSP hooked, return the required local endpoint.<para/>
        /// otherwise, return the actual listened local endpoint.
        /// </summary>
        private IPEndPoint lspHookedLocalEP;

        /// <summary>
        /// a bool value that indicates whether lsp hooked the transport.
        /// </summary>
        private bool lspHooked;

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tcpListener">
        /// a TcpListener that specifies the listener.
        /// </param>
        /// <param name="tcpServerTransport">
        /// a TcpServerTransport that represents the owner of listener.
        /// </param>
        /// <param name="isLspHooked">
        /// a bool value that indicates whether lsp hooked the transport.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when tcpListener is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when tcpServerTransport is null.
        /// </exception>
        public TcpServerListener(TcpListener tcpListener, TcpServerTransport tcpServerTransport, bool isLspHooked)
        {
            if (tcpListener == null)
            {
                throw new ArgumentNullException("tcpListener");
            }

            if (tcpServerTransport == null)
            {
                throw new ArgumentNullException("tcpServerTransport");
            }

            this.lspHooked = isLspHooked;
            this.listener = tcpListener;
            this.server = tcpServerTransport;
            this.thread = new ThreadManager(AcceptLoop, Unblock);
        }

        #endregion

        #region Methods

        /// <summary>
        /// start the listener and then start a thread to accept connection from client.
        /// </summary>
        /// <param name="isBlocking">
        /// a bool value that specifies whether LSP is block mode.<para/>
        /// it's used to pass to InterceptTraffic.
        /// </param>
        /// <param name="requiredLocalEP">
        /// an IPEndPoint object that specifies the local endpoint for server to listen at.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Start(bool isBlocking, IPEndPoint requiredLocalEP)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerListener");
            }

            this.listener.Start();

            // if LSP is enabled, intercept the traffic
            if (this.lspHooked)
            {
                // the actual local endpoint for server to listen at.
                IPEndPoint actualListenedLocalEP = this.listener.Server.LocalEndPoint as IPEndPoint;

                LspConsole.Instance.InterceptTraffic(
                    this.server.SocketConfig.Type, isBlocking, requiredLocalEP, actualListenedLocalEP);
            }

            this.lspHookedLocalEP = requiredLocalEP;

            this.thread.Start();
        }


        /// <summary>
        /// close the listener, it will dispose the listener.
        /// </summary>
        public void Stop()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TcpServerListener");
            }

            this.Dispose();

            if (this.lspHooked)
            {
                LspConsole.Instance.UnblockTraffic(this.lspHookedLocalEP, this.server.SocketConfig.Type);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// the accept thread to accept connection from client.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void AcceptLoop()
        {
            while (!this.thread.ShouldExit)
            {
                try
                {
                    TcpClient client = this.listener.AcceptTcpClient();

                    TcpServerConnection connection =
                        this.server.AcceptClient(client, this.lspHookedLocalEP, this.lspHooked);

                    this.server.AddEvent(connection.VisitorCreateTransportEvent(EventType.Connected, client));

                    connection.Start();
                }
                // user stopped the listener.
                catch (SocketException)
                {
                }
                // user is dispoing the listener.
                catch (Exception ex)
                {
                    this.server.AddEvent(new TransportEvent(EventType.Exception, null, ex));
                }
            }
        }


        /// <summary>
        /// unblock the accept thread.
        /// </summary>
        private void Unblock()
        {
            this.listener.Stop();
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
                    if (this.listener != null)
                    {
                        this.listener.Stop();
                        this.listener = null;
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
        ~TcpServerListener()
        {
            Dispose(false);
        }

        #endregion
    }
}
