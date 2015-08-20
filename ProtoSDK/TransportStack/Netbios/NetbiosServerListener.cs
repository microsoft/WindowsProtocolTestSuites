// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Sockets;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// netbios server listener represents the listener of server<para/>
    /// start a thread to accept connect from NetbiosListener.
    /// </summary>
    internal class NetbiosServerListener : IDisposable
    {
        #region Fields

        /// <summary>
        /// a string value that specifies the local netbios name to listen at.
        /// </summary>
        private string netbiosName;

        /// <summary>
        /// a NetbiosServerTransport that represents the owner of listener.
        /// </summary>
        private NetbiosServerTransport server;

        /// <summary>
        /// a NetbiosTransport object that specifies the underlayer transport of netbios.<para/>
        /// initialize at start. set to null at dispose.
        /// </summary>
        private NetbiosTransport transport;

        /// <summary>
        /// a ThreadManager object that specifies the thread to accept connection from client.
        /// </summary>
        private ThreadManager thread;

        /// <summary>
        /// an int value that specifies the local endpoint of netbios server.
        /// </summary>
        private int localEndPoint;

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="localNetbiosName">
        /// a string value that specifies the local netbios name to listen at.
        /// </param>
        /// <param name="serverTransport">
        /// a NetbiosServerTransport that represents the owner of listener.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when localNetbiosName is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when serverTransport is null.
        /// </exception>
        public NetbiosServerListener(string localNetbiosName, NetbiosServerTransport serverTransport)
        {
            if (localNetbiosName == null)
            {
                throw new ArgumentNullException("localNetbiosName");
            }

            if (serverTransport == null)
            {
                throw new ArgumentNullException("serverTransport");
            }

            this.netbiosName = localNetbiosName;
            this.server = serverTransport;
        }

        #endregion

        #region Methods

        /// <summary>
        /// start the listener and then start a thread to accept connection from client.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the listener on the endpoint has been started.
        /// </exception>
        public void Start()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerListener");
            }

            if (this.thread != null)
            {
                throw new InvalidOperationException("the listener on the endpoint has been started.");
            }

            if (this.transport == null)
            {
                this.transport = new NetbiosTransport(
                    this.netbiosName, this.server.NetbiosConfig.AdapterIndex,
                    (ushort)this.server.NetbiosConfig.BufferSize, (byte)this.server.NetbiosConfig.MaxSessions,
                    (byte)this.server.NetbiosConfig.MaxNames);
            }

            this.localEndPoint = this.transport.NcbNum;

            this.thread = new ThreadManager(AcceptLoop, Unblock);
            this.thread.Start();
        }


        /// <summary>
        /// stop the listener: stop the listening thread.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Stop()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerListener");
            }

            if (this.thread == null)
            {
                return;
            }

            this.thread.Stop();
            this.thread = null;
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
                    if (this.transport != null)
                    {
                        this.transport.Dispose();
                        this.transport = null;
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
        ~NetbiosServerListener()
        {
            Dispose(false);
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
                    int client = this.transport.Listen();

                    NetbiosServerConnection connection = this.server.AcceptClient(localEndPoint, client, transport);

                    this.server.AddEvent(new TransportEvent(EventType.Connected, client, this.localEndPoint, this));

                    connection.Start();
                }
                // user stopped the listener.
                catch (InvalidOperationException)
                {
                }
                catch (Exception ex)
                {
                    this.server.AddEvent(new TransportEvent(EventType.Exception, null, ex));
                }
            }
        }


        /// <summary>
        /// unblock the accept thread.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void Unblock()
        {
            if (this.transport == null)
            {
            }

            // donot arise any exception
            try
            {
                this.transport.CancelListen();
            }
            catch (Exception ex)
            {
                this.server.AddEvent(new TransportEvent(EventType.Exception, null, ex));
            }
        }

        #endregion
    }
}
