// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// tcp server listener represents the listener of server<para/>
    /// start a thread to accept connect from TcpListener.
    /// </summary>
    internal class UdpServerListener : IDisposable, IVisitorUdpReceiveLoop
    {
        #region Fields

        /// <summary>
        /// a UdpClient that specifies the listener.<para/>
        /// never be null util disposed.
        /// </summary>
        private UdpClient listener;

        /// <summary>
        /// a UdpServerTransport that represents the owner of listener.
        /// </summary>
        private UdpServerTransport server;

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

        #region Properties

        /// <summary>
        /// a UdpClient that specifies the listener.<para/>
        /// never be null util disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public UdpClient Listener
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("UdpServerListener");
                }

                return this.listener;
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

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="udpListener">
        /// a UdpClient that specifies the listener.
        /// </param>
        /// <param name="udpServerTransport">
        /// a UdpServerTransport that represents the owner of listener.
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
        public UdpServerListener(UdpClient udpListener, UdpServerTransport udpServerTransport, bool isLspHooked)
        {
            if (udpListener == null)
            {
                throw new ArgumentNullException("udpListener");
            }

            if (udpServerTransport == null)
            {
                throw new ArgumentNullException("udpServerTransport");
            }

            this.lspHooked = isLspHooked;
            this.listener = udpListener;
            this.server = udpServerTransport;
            this.thread = new ThreadManager(UdpServerListenerReceiveLoop, Unblock);
        }

        #endregion

        #region Methods

        /// <summary>
        /// start the listener and then start a thread to accept connection from client.
        /// </summary>
        /// <param name="requiredLocalEP">
        /// an IPEndPoint object that specifies the replaced endpoint.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Start(IPEndPoint requiredLocalEP)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerListener");
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
                throw new ObjectDisposedException("UdpServerListener");
            }

            this.Dispose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// an IPEndPoint object that specifies the local endpoint.<para/>
        /// if LSP hooked, return the required local endpoint.<para/>
        /// otherwise, return the actual listened local endpoint.
        /// </summary>
        public IPEndPoint LspHookedLocalEP
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("UdpServerListener");
                }

                return this.lspHookedLocalEP;
            }
        }


        /// <summary>
        /// add the received data to buffer.
        /// </summary>
        /// <param name="data">
        /// a bytes array that contains the received data.
        /// </param>
        /// <param name="localEP">
        /// an IPEndPoint object that specifies the local endpoint.
        /// </param>
        /// <param name="remoteEP">
        /// an IPEndPoint object that specifies the remote endpoint.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void VisitorAddReceivedData(byte[] data, IPEndPoint localEP, IPEndPoint remoteEP)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UdpServerListener");
            }

            this.server.Buffer.Enqueue(new UdpReceivedBytes(data, remoteEP, localEP));
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        private void UdpServerListenerReceiveLoop()
        {
            // donot arise any exception in the work thread.
            Utility.SafelyOperation(this.UdpServerListenerReceiveLoopImp);
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        private void UdpServerListenerReceiveLoopImp()
        {
            UdpReceiveLoopVisitor.Visit(
                this, this.server, this.listener, this.thread, this.lspHooked);
        }


        /// <summary>
        /// unblock the accept thread.
        /// </summary>
        private void Unblock()
        {
            this.listener.Close();
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
                        this.listener.Close();
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
        ~UdpServerListener()
        {
            Dispose(false);
        }

        #endregion
    }
}
