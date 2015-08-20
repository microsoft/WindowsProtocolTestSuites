// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// netbios server connection represents the connection between server and client.<para/>
    /// start a thread to accept data from netbios client.
    /// </summary>
    internal class NetbiosServerConnection : IDisposable, IVisitorNetbiosReceiveLoop
    {
        #region Fields

        /// <summary>
        /// an int value that specifies the session id of connection.
        /// </summary>
        private int localEndPoint;

        /// <summary>
        /// an int value that specifies the session id of connection.
        /// </summary>
        private int remoteEndPoint;

        /// <summary>
        /// NetbiosServerTransport object that specifies the netbios server transport.
        /// </summary>
        private NetbiosServerTransport server;

        /// <summary>
        /// a NetbiosTransport object that specifies the owner of client.
        /// </summary>
        private NetbiosTransport transport;

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
        /// a bool value that indicates that whether the server is stopped.
        /// </summary>
        private bool isServerStopped;

        #endregion

        #region Properties

        /// <summary>
        /// get an int value that specifies the remote server session id.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public int RemoteEndPoint
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("NetbiosServerConnection");
                }

                return this.remoteEndPoint;
            }
        }


        /// <summary>
        /// get an int value that specifies the local ncb number.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public int LocalEndPoint
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("NetbiosServerConnection");
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
                    throw new ObjectDisposedException("NetbiosServerConnection");
                }

                return this.isServerStopped;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="localEndPoint">
        /// an int value that specifies the session id of connection.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an int value that specifies the session id of connection.
        /// </param>
        /// <param name="server">
        /// NetbiosServerTransport object that specifies the netbios server transport.
        /// </param>
        /// <param name="transport">
        /// a NetbiosTransport object that specifies the owner of client.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when the server is null!
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when the transport is null!
        /// </exception>
        public NetbiosServerConnection(
            int localEndPoint, int remoteEndPoint, NetbiosServerTransport server, NetbiosTransport transport)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            if (transport == null)
            {
                throw new ArgumentNullException("transport");
            }

            this.localEndPoint = localEndPoint;
            this.remoteEndPoint = remoteEndPoint;
            this.server = server;
            this.transport = transport;

            this.thread = new ThreadManager(NetbiosServerConnectionReceiveLoop, Unblock);
            this.buffer = new BytesBuffer();
        }

        #endregion

        #region Methods

        /// <summary>
        /// start the receive thread to received data from client.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public void Start()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerConnection");
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
                throw new ObjectDisposedException("NetbiosServerConnection");
            }

            this.isServerStopped = true;

            this.thread.Stop();
        }


        /// <summary>
        /// Send an arbitrary message over the transport.<para/>
        /// the underlayer transport must be NetbiosClient, Stream or NetbiosClient.
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
        public void SendBytes(byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("NetbiosServerConnection");
            }

            this.transport.Send(this.remoteEndPoint, message);
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
                throw new ObjectDisposedException("NetbiosServerConnection");
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
                throw new ObjectDisposedException("NetbiosServerConnection");
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
                    if (this.transport != null)
                    {
                        // the netbios transport may throw exception, donot arise exception.
                        Utility.SafelyDisconnectNetbiosConnection(this.transport, this.remoteEndPoint);
                        this.transport = null;
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
        ~NetbiosServerConnection()
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
                throw new ObjectDisposedException("NetbiosServerConnection");
            }

            this.server.Sequence.Add(this, ArrayUtility.SubArray<byte>(data, 0), this.buffer);
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void NetbiosServerConnectionReceiveLoop()
        {
            try
            {
                // donot arise any exception in the work thread.
                Utility.SafelyOperation(this.NetbiosServerConnectionReceiveLoopImp);
            }
            finally
            {
                // this operation is safe, it will never arise exception.
                // because in this function, the thread is not end, so the buffer is not disposed.
                this.buffer.Close();
            }
        }


        /// <summary>
        /// the method to receive message from server.
        /// </summary>
        private void NetbiosServerConnectionReceiveLoopImp()
        {
            NetbiosReceiveLoopVisitor.Visit(this, this.server, this.transport, this.localEndPoint, this.remoteEndPoint, this.thread);
        }


        /// <summary>
        /// unblock the received thread.
        /// </summary>
        private void Unblock()
        {
            // the netbios transport may throw exception, donot arise exception.
            Utility.SafelyDisconnectNetbiosConnection(this.transport, this.remoteEndPoint);
        }

        #endregion
    }
}
