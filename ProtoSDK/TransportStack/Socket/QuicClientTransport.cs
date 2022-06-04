// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Quic;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    internal class QuicClientTransport : IQuicClient, IVisitorQuicReceiveLoop
    {
        #region Fields

        private SocketTransportConfig socketConfig;

        private QuicConnection connection;

        private QuicStream stream;

        private ThreadManager thread;

        private BytesBuffer buffer;

        private SyncFilterQueue<TransportEvent> eventQueue;

        private DecodePacketCallback decoder;

        private SyncFilterQueue<StackPacket> packetCache;

        private EndPoint remoteEndPoint;

        private EndPoint localEndPoint;

        private SslApplicationProtocol ApplicationProtocol = new("smb");

        #endregion

        #region Constructor

        public QuicClientTransport(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
        {
            if (transportConfig == null)
            {
                throw new ArgumentNullException(nameof(transportConfig));
            }

            if (decodePacketCallback == null)
            {
                throw new ArgumentNullException(nameof(decodePacketCallback));
            }

            this.UpdateConfig(transportConfig);
            this.buffer = new BytesBuffer();
            this.eventQueue = new SyncFilterQueue<TransportEvent>();
            this.packetCache = new SyncFilterQueue<StackPacket>();
            this.decoder = decodePacketCallback;
            this.remoteEndPoint = new IPEndPoint(this.socketConfig.RemoteIpAddress, this.socketConfig.RemoteIpPort);
        }

        #endregion

        public bool IsDataAvailable
        {
            get
            {
                ValidateClientTransport();

                return this.packetCache.Count > 0 || this.buffer.Length > 0;
            }
        }

        public void AddEvent(TransportEvent transportEvent)
        {
            ValidateClientTransport();

            if (transportEvent == null)
            {
                throw new ArgumentNullException("transportEvent");
            }

            this.eventQueue.Enqueue(transportEvent);
        }

        public object Connect()
        {
            ValidateClientTransport();

            if (this.stream != null)
            {
                throw new InvalidOperationException("A quic stream is already open. A server connection already exists.");
            }

            if (this.thread != null)
            {
                throw new InvalidOperationException("The received thread does not cleanup.");
            }

            if (this.socketConfig.RemoteIpAddress == null)
            {
                throw new InvalidOperationException("The remote ip address is null.");
            }

            this.buffer = new BytesBuffer();

            this.eventQueue.Clear();
            this.packetCache.Clear();

            IPEndPoint requiredLocalEP = this.localEndPoint as IPEndPoint;

            if (requiredLocalEP == null)
            {
                if (this.socketConfig.LocalIpAddress == null)
                {
                    this.socketConfig.LocalIpAddress = IPAddress.Any;
                }
                requiredLocalEP = Utility.GetEndPointByPort(this.socketConfig, localEndPoint);
            }

            this.socketConfig.LocalIpAddress = requiredLocalEP.Address;

            this.socketConfig.LocalIpPort = requiredLocalEP.Port;

            SendConnectionRequest();

            this.stream = connection.OpenBidirectionalStream();

            this.thread = new ThreadManager(QuicClientReceiveLoop);

            this.thread.Start();

            this.localEndPoint = connection.LocalEndPoint;

            this.remoteEndPoint = connection.RemoteEndPoint;

            return this.localEndPoint;
        }

        public void Disconnect()
        {
            ValidateClientTransportState();

            if (this.thread == null)
            {
                throw new InvalidOperationException("the received thread does not initialize.");
            }
            this.stream.Shutdown();
            this.stream = null;

            this.thread.Dispose();
            this.thread = null;
        }

        public byte[] ExpectBytes(TimeSpan timeout, int maxCount)
        {
            ValidateClientTransportState();

            return ExpectBytesVisitor.Visit(this.buffer, timeout, maxCount);
        }

        public object ExpectDisconnect(TimeSpan timeout)
        {
            ValidateClientTransportState();

            this.eventQueue.Dequeue(timeout, new TransportFilter().FilterDisconnected);

            return this.localEndPoint;
        }

        public StackPacket ExpectPacket(TimeSpan timeout)
        {
            ValidateClientTransportState();

            return ExpectSinglePacketVisitor.Visit(
                this.buffer, this.decoder, this.localEndPoint, timeout, this.packetCache);
        }

        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            ValidateClientTransportState();

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

            return eventQueue.Dequeue(new TimeSpan());
        }


        public void SendPacket(StackPacket packet)
        {
            ValidateClientTransport();

            this.SendBytes(packet.ToBytes());
        }

        public void SendBytes(byte[] message)
        {
            ValidateClientTransport();

            try
            {
                if (!this.connection.Connected)
                {
                    throw new QuicConnectionAbortedException("We lost the connection", 410L);
                }

                if (this.stream == null)
                {
                    throw new InvalidOperationException("Can not send message when stream is closed.");
                }
                this.stream.WriteAsync(message, 0, message.Length).Wait();
            }
            catch(QuicException qex)
            {
                //Swallow exception
            }
        }

        public void UpdateConfig(TransportConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("transportConfig");
            }

            SocketTransportConfig socketTransportConfig = config as SocketTransportConfig;

            if (socketTransportConfig == null)
            {
                throw new ArgumentException("transportConfig must be SocketTransportConfig", "config");
            }

            this.socketConfig = socketTransportConfig;
        }

        public void VisitorAddReceivedData(byte[] data)
        {
            ValidateClientTransport();

            this.buffer.Add(data);
        }

        public TransportEvent VisitorCreateTransportEvent(EventType type, object detail)
        {
            ValidateClientTransport();

            return new TransportEvent(type, this.localEndPoint, this.remoteEndPoint, this.localEndPoint, detail);
        }

        #region IDisposable Members

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (disposing)
                {
                    this.disposed = true;
                }
            }
        }

        ~QuicClientTransport()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        private void ValidateClientTransport()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(QuicClientTransport));
            }
        }

        private void ValidateStream()
        {
            if (this.stream == null)
            {
                throw new InvalidOperationException("Client is not connected to server, must first invoke connect() first");
            }
        }

        private void ValidateClientTransportState()
        {
            ValidateClientTransport();

            ValidateStream();
        }

        private SslClientAuthenticationOptions GetSslClientAuthenticationOptions()
        {
            return new SslClientAuthenticationOptions()
            {
                ApplicationProtocols = new List<SslApplicationProtocol>() { ApplicationProtocol },
                TargetHost = this.socketConfig.TargetName,
            };
        }

        private QuicConnection CreateQuicConnection()
        {
            //Using DNS EndPoint instead of IP because SmbOverQuic certificate is issued using DNS name
            EndPoint targetEndPoint = new DnsEndPoint(this.socketConfig.TargetName, this.socketConfig.RemoteIpPort);
            return new QuicConnection(targetEndPoint, GetSslClientAuthenticationOptions());
        }

        private void SendConnectionRequest()
        {
            connection = this.CreateQuicConnection();

            IAsyncResult result = connection.BeginConnect(new AsyncCallback(QuicConnectionCallback), connection);

            result.AsyncWaitHandle.WaitOne(this.socketConfig.Timeout, true);

            if (!connection.Connected)
            {
                throw new TimeoutException(string.Format(CultureInfo.InvariantCulture, "Failed to connect to server {0}:{1} within {2} milliseconds.", this.socketConfig.RemoteIpAddress, this.socketConfig.RemoteIpPort, this.socketConfig.Timeout.TotalMilliseconds));
            }
        }

        private void QuicConnectionCallback(IAsyncResult asyncResult)
        {
            QuicConnection quicConnection = (QuicConnection)asyncResult.AsyncState;
            if (quicConnection.Connected)
            {
                try
                {
                    quicConnection.EndConnect(asyncResult);
                }
                catch
                {
                    // Swallow all exceptions thrown in a callback function.
                }
            }
        }

        private void QuicClientReceiveLoop()
        {
            try
            {
                // donot arise any exception in the work thread.
                Utility.SafelyOperation(this.QuicClientReceiveLoopImp);
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
        private void QuicClientReceiveLoopImp()
        {
            QuicReceiveLoopVisitor.Visit(this, this, this.stream, this.thread, this.socketConfig.BufferSize);
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

        #endregion
    }
}
