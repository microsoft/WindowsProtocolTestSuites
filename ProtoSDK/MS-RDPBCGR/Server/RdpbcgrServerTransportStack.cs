// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestTools.ExtendedLogging;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Decode stack packet from the received message bytes.
    /// </summary>
    /// <param name="endPoint">the endpoint from which the message bytes are received.</param>
    /// <param name="messageBytes">the received message bytes to be decoded.</param>
    /// <param name="consumedLength">the length of message bytes consumed by decoder.</param>
    /// <param name="expectedLength">the length of message bytes the decoder expects to receive.</param>
    /// <returns>the stack packets decoded from the received message bytes.</returns>
    public delegate StackPacket[] DecodePacketCallback(
        object endPoint,
        byte[] messageBytes,
        out int consumedLength,
        out int expectedLength);


    /// <summary>
    /// When enhanced security is used, a new ssl stream should be used to send and receive data.
    /// </summary>
    public enum SecurityStreamType
    {
        None,
        Ssl,
        CredSsp
    }


    /// <summary>
    /// the status of transport returned from receiving method.
    /// </summary>
    public enum ReceiveStatus : int
    {
        /// <summary>
        /// All transport should use this value to notify the caller that the transport has received
        /// valid data from the remote host.
        /// </summary>
        Success,

        /// <summary>
        /// All transport should use this value to notify the caller that the transport is disconnected
        /// by the remost host.
        /// </summary>
        Disconnected,
    }


    /// <summary>
    /// Stores the configurable parameters used by transport.
    /// </summary>
    public class RdpcbgrServerTransportConfig
    {
        #region Field members
        private int maxConnections;
        private SecurityStreamType streamType;
        private int bufferSize;
        private IPAddress localIpAddress;
        private int localIpPort;
        private IPAddress remoteIpAddress;
        private int remoteIpPort;
        #endregion


        #region Properties
        /// <summary>
        /// The size of buffer used for receiving data.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this.bufferSize;
            }
            set
            {
                this.bufferSize = value;
            }
        }


        /// <summary>
        /// The max number of connections.
        /// </summary>
        public int MaxConnections
        {
            get
            {
                return maxConnections;
            }
            set
            {
                maxConnections = value;
            }
        }


        /// <summary>
        /// Ssl stream or normal network stream.
        /// </summary>
        public SecurityStreamType StreamType
        {
            get
            {
                return streamType;
            }
            set
            {
                streamType = value;
            }
        }


        /// <summary>
        /// The local IPAddress.
        /// </summary>
        public IPAddress LocalIpAddress
        {
            get
            {
                return this.localIpAddress;
            }
            set
            {
                this.localIpAddress = value;
            }
        }


        /// <summary>
        /// The local IP port.
        /// </summary>
        public int LocalIpPort
        {
            get
            {
                return this.localIpPort;
            }
            set
            {
                this.localIpPort = value;
            }
        }


        /// <summary>
        /// The remote IPAddress.
        /// </summary>
        public IPAddress RemoteIpAddress
        {
            get
            {
                return this.remoteIpAddress;
            }
            set
            {
                this.remoteIpAddress = value;
            }
        }


        /// <summary>
        /// The remote IP port.
        /// </summary>
        public int RemoteIpPort
        {
            get
            {
                return this.remoteIpPort;
            }
            set
            {
                this.remoteIpPort = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor. Initialize member variables.
        /// </summary>
        /// <param name="sType">The type of security stream.</param>
        /// <param name="ip">The server ip.</param>
        /// <param name="port">The service port.</param>
        public RdpcbgrServerTransportConfig(
            SecurityStreamType sType,
            IPAddress ip,
            int port)
        {
            int ti = ConstValue.MAXCONNECTIONS;
            maxConnections = ti;
            streamType = sType;
            bufferSize = ConstValue.BUFFERSIZE;
            this.localIpAddress = ip;
            this.localIpPort = port;
        }
        #endregion
    }

    /// <summary>
    /// Ugly Code.
    /// ETW Stream wrapping NetworkStream for ETW Provider to dump message
    /// </summary>
    class ETWStream : Stream
    {
        Stream stream;

        List<byte> sentBuffer;
        int sentBufferIndex;
        List<byte> receivedBuffer;
        int receivedBufferIndex;
        const int lengthHigh = 3;
        const int lengthLow = 4;
        const int headerLength = 5;

        public ETWStream(Stream stream)
        {
            this.stream = stream;
            sentBuffer = new List<byte>();
            receivedBuffer = new List<byte>();
            sentBufferIndex = 0;
            receivedBufferIndex = 0;
        }

        public override bool CanRead
        {
            get { return stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return stream.CanWrite; }
        }

        public override void Flush()
        {
            stream.Flush();
        }

        public override long Length
        {
            get { return stream.Length; }
        }

        public override long Position
        {
            get
            {
                return stream.Position;
            }
            set
            {
                stream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int readcount = stream.Read(buffer, offset, count);

            // ETW Provider Dump Message and Reassembly
            string messageName = "RDPBCGR:TLSReceived";
            byte[] readBytes = new byte[readcount];
            Array.Copy(buffer, offset, readBytes, 0, readcount);

            receivedBuffer.AddRange(readBytes);
            if (receivedBufferIndex + lengthHigh < receivedBuffer.Count && receivedBufferIndex + lengthLow < receivedBuffer.Count)
            {
                int length = (receivedBuffer[receivedBufferIndex + lengthHigh] << 8) + receivedBuffer[receivedBufferIndex + lengthLow];
                while (receivedBufferIndex + headerLength + length < receivedBuffer.Count)
                {
                    receivedBufferIndex = receivedBufferIndex + headerLength + length;
                    length = (receivedBuffer[receivedBufferIndex + lengthHigh] << 8) + receivedBuffer[receivedBufferIndex + lengthLow];
                }
                if (receivedBufferIndex + headerLength + length == receivedBuffer.Count)
                {
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_LayerTLS, "TLS Received Data", receivedBuffer.ToArray());
                    receivedBuffer.Clear();
                    receivedBufferIndex = 0;
                }
            }

            return readcount;

        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            stream.Write(buffer, offset, count);

            // ETW Provider Dump Message and Reassembly
            string messageName = "RDPBCGR:TLSSent";
            byte[] sentBytes = new byte[count];
            Array.Copy(buffer, offset, sentBytes, 0, count);

            sentBuffer.AddRange(sentBytes);
            if (sentBufferIndex + lengthHigh < sentBuffer.Count && sentBufferIndex + lengthLow < sentBuffer.Count)
            {
                int length = (sentBuffer[sentBufferIndex + lengthHigh] << 8) + sentBuffer[sentBufferIndex + lengthLow];
                while (sentBufferIndex + headerLength + length < sentBuffer.Count)
                {
                    sentBufferIndex = sentBufferIndex + headerLength + length;
                    length = (sentBuffer[sentBufferIndex + lengthHigh] << 8) + sentBuffer[sentBufferIndex + lengthLow];
                }
                if (sentBufferIndex + headerLength + length == sentBuffer.Count)
                {
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_LayerTLS, "TLS Sent Data", sentBuffer.ToArray());
                    sentBuffer.Clear();
                    sentBufferIndex = 0;
                }
            }
        }
    }


    /// <summary>
    /// Contains fields and methods to provide tcp connection and relative functions.
    /// </summary>
    internal class RdpbcgrServerTransportStack : IDisposable
    {
        #region Field members
        private SecurityStreamType streamType;
        private bool disposed;
        private RdpcbgrServerTransportConfig config;
        private DecodePacketCallback decoder;
        private QueueManager packetQueue;
        private Dictionary<Socket, RdpbcgrReceiveThread> receivingStreams;
        private Socket listenSock;
        private Thread acceptThread;
        private bool isTcpServerTransportStarted;
        private volatile bool exitLoop;
        private RdpbcgrServer rdpbcgrServer;
        //private string certName;
        private X509Certificate2 cert;
        #endregion


        #region Properties
        /// <summary>
        /// The packetQueue.
        /// </summary>
        public QueueManager PacketQueue
        {
            get
            {
                return packetQueue;
            }
        }


        /// <summary>
        /// The receivingStreams.
        /// </summary>
        public ReadOnlyDictionary<Socket, RdpbcgrReceiveThread> ReceivingStreams
        {
            get
            {
                return new ReadOnlyDictionary<Socket, RdpbcgrReceiveThread>(receivingStreams);
            }
        }
        #endregion


        #region Constructor and Dispose
        /// <summary>
        ///  Constructor. Initialize member variables.
        /// </summary>
        /// <param name="transportConfig">Provides the transport parameters.</param>
        /// <param name="decodePacketCallback">Callback of decoding packet.</param>
        public RdpbcgrServerTransportStack(
            RdpbcgrServer rdpbcgrServer,
            RdpcbgrServerTransportConfig transportConfig,
            DecodePacketCallback decodePacketCallback)
        {
            this.rdpbcgrServer = rdpbcgrServer;
            this.config = transportConfig;
            if (this.config == null)
            {
                throw new System.InvalidCastException("TcpServerTransport needs SocketTransportConfig.");
            }
            
            this.decoder = decodePacketCallback;
            this.packetQueue = new QueueManager();
            this.listenSock = new Socket(transportConfig.LocalIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.streamType = config.StreamType;
            IPEndPoint endPoint = new IPEndPoint(config.LocalIpAddress, config.LocalIpPort);
            this.listenSock.Bind(endPoint);
            this.listenSock.Listen(config.MaxConnections);
            this.acceptThread = new Thread(new ThreadStart(AcceptLoop));
            this.receivingStreams = new Dictionary<Socket, RdpbcgrReceiveThread>();
        }


        /// <summary>
        ///  Constructor. Initialize member variables.
        /// </summary>
        /// <param name="transportConfig">Provides the transport parameters.</param>
        /// <param name="decodePacketCallback">Callback of decoding packet.</param>
        /// <param name="certificate">X509 certificate.</param>
        public RdpbcgrServerTransportStack(
            RdpbcgrServer rdpbcgrServer,
            RdpcbgrServerTransportConfig transportConfig,
            DecodePacketCallback decodePacketCallback,
            X509Certificate2 certificate)
        {
            this.rdpbcgrServer = rdpbcgrServer;
            this.config = transportConfig;
            if (this.config == null)
            {
                throw new System.InvalidCastException("TcpServerTransport needs SocketTransportConfig.");
            }

            this.decoder = decodePacketCallback;
            this.packetQueue = new QueueManager();
            this.listenSock = new Socket(transportConfig.LocalIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.streamType = transportConfig.StreamType;
            IPEndPoint endPoint = new IPEndPoint(config.LocalIpAddress, config.LocalIpPort);
            this.listenSock.Bind(endPoint);
            this.listenSock.Listen(config.MaxConnections);
            this.acceptThread = new Thread(new ThreadStart(AcceptLoop));
            this.receivingStreams = new Dictionary<Socket, RdpbcgrReceiveThread>();
            this.cert = certificate;
        }

        
        /// <summary>
        /// Close specific end point stream.
        /// </summary>
        public void CloseStream(object endPoint)
        {
            lock (this.receivingStreams)
            {
                if (this.receivingStreams != null)
                {
                    foreach (KeyValuePair<Socket, RdpbcgrReceiveThread> kvp in this.receivingStreams)
                    {
                        if (kvp.Key.RemoteEndPoint == endPoint)
                        {
                            this.receivingStreams.Remove(kvp.Key);
                            kvp.Key.Close();
                            kvp.Value.Dispose();
                            break;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            //Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                exitLoop = true;
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    if (this.listenSock != null)
                    {
                        this.listenSock.Close();
                        this.listenSock = null;
                    }

                    lock (this.receivingStreams)
                    {
                        if (this.receivingStreams != null)
                        {
                            foreach (KeyValuePair<Socket, RdpbcgrReceiveThread> kvp in this.receivingStreams)
                            {
                                kvp.Key.Close();
                                kvp.Value.Dispose();
                            }
                            this.receivingStreams = null;
                        }
                    }

                    if (this.packetQueue != null)
                    {
                        this.packetQueue.Dispose();
                        this.packetQueue = null;
                    }

                    if (this.acceptThread != null && this.acceptThread.ThreadState != ThreadState.Unstarted)
                    {
                        this.acceptThread.Join();
                    }
                }

                this.disposed = true;
            }
        }


        /// <summary>
        /// This destructor will get called only from the finalization queue.
        /// </summary>
        ~RdpbcgrServerTransportStack()
        {
            this.Dispose(false);
        }
        #endregion


        #region User interface
        /// <summary>
        /// Start the server.
        /// </summary>
        public void Start()
        {
            this.acceptThread.Start();
            this.isTcpServerTransportStarted = true;
        }


        /// <summary>
        /// Disconnect from all remote hosts.
        /// </summary>
        public void Disconnect()
        {
            lock (this.receivingStreams)
            {
                foreach (KeyValuePair<Socket, RdpbcgrReceiveThread> kvp in this.receivingStreams)
                {
                    kvp.Value.Abort();
                    kvp.Key.Close();
                }
                this.receivingStreams.Clear();
            }
        }


        /// <summary>
        /// Disconnect from the remote host according to the given endPoint.
        /// </summary>
        /// <param name="endPoint">Endpoint to be disconnected.</param>
        public void Disconnect(object endPoint)
        {
            IPEndPoint ipEndPoint = endPoint as IPEndPoint;
            if (ipEndPoint == null)
            {
                throw new ArgumentException("The endPoint is not an IPEndPoint.", "endPoint");
            }

            lock (this.receivingStreams)
            {
                Socket socket = null;

                foreach (Socket sock in this.receivingStreams.Keys)
                {
                    IPEndPoint endPointTmp = (IPEndPoint)sock.RemoteEndPoint;
                    if (ipEndPoint.Address.ToString().CompareTo(endPointTmp.Address.ToString()) == 0 && (ipEndPoint.Port == endPointTmp.Port))
                    {
                        socket = sock;
                    }
                }

                this.receivingStreams[socket].Abort();
                socket.Close();
                this.receivingStreams.Remove(socket);
            }
        }


        /// <summary>
        /// Send a packet to a special remote host.
        /// </summary>
        /// <param name="endPoint">The remote host to which the packet will be sent.</param>
        /// <param name="packet">The packet to be sent.</param>
        public void SendPacket(object endPoint, StackPacket packet)
        {
            ValidServerHasStarted();

            IPEndPoint ipEndPoint = endPoint as IPEndPoint;
            if (ipEndPoint == null)
            {
                throw new ArgumentException("The endPoint is not an IPEndPoint.", "endPoint");
            }

            // If we use raw bytes to construct the packet, we send the raw bytes out directly, 
            // otherwise we convert the packet to bytes and send it out.
            byte[] writeBytes = (packet.PacketBytes == null) ? packet.ToBytes() : packet.PacketBytes;
            Stream stream = GetStream(ipEndPoint.Address, ipEndPoint.Port);

            if (stream == null)
            {
                throw new InvalidOperationException("The endPoint is not in the connect list.");
            }

            stream.Write(writeBytes, 0, writeBytes.Length);
        }


        /// <summary>
        /// Send arbitrary message to a special remote host.
        /// </summary>
        /// <param name="endpoint">The remote host to which the packet will be sent.</param>
        /// <param name="message">The message to be sent.</param>
        public void SendBytes(object endpoint, byte[] message)
        {
            IPEndPoint ipEndPoint = endpoint as IPEndPoint;
            Stream stream = GetStream(ipEndPoint.Address, ipEndPoint.Port);
            stream.Write(message, 0, message.Length);
        }


        /// <summary>
        /// Add transport event to transport, TSD can invoke ExpectTransportEvent to get it.
        /// </summary>
        /// <param name="transportEvent">
        /// a TransportEvent object that contains the event to add to the queue
        /// </param>
        public void AddEvent(TransportEvent transportEvent)
        {
            this.packetQueue.AddObject(transportEvent);
        }


        /// <summary>
        /// Expect transport event from transport.
        /// if event arrived and packet data buffer is empty, return event directly.
        /// decode packet from packet data buffer, return packet if arrived, otherwise, return event.
        /// </summary>
        /// <param name="timeout">
        /// TimeSpan struct that specifies the timeout of waiting for a  packet or event from the transport.
        /// </param>
        /// <returns>
        /// TransportEvent object that contains the expected event from transport.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the under layer transport is null! the config for TransportStack is invalid.
        /// </exception>
        public TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            TransportEvent eventPacket = this.packetQueue.GetObject(ref timeout) as TransportEvent;

            if (eventPacket == null)
            {
                throw new InvalidOperationException("Invalid object are cached in the queue.");
            }

            if (eventPacket.EventType == EventType.ReceivedPacket)
            {
                StackPacket packet = eventPacket.EventObject as StackPacket;
            }
            else if (eventPacket.EventType == EventType.Exception)
            {
                throw new InvalidOperationException(
                    "There's an exception thrown when receiving a packet.",
                    (Exception)eventPacket.EventObject);
            }

            return eventPacket;       
        }


        /// <summary>
        /// Update the config of transport at runtime.
        /// </summary>
        /// <param name="type">The type of transport stream.</param>
        internal void UpdateConfig(SecurityStreamType type)
        {
            foreach (Socket sock in this.receivingStreams.Keys)
            {
                if (receivingStreams[sock].ReceiveStream is SslStream || receivingStreams[sock].ReceiveStream is RdpbcgrServerCredSspStream)
                {
                    //Skip the connections which already were updated to SSL or CredSSP.
                    continue;
                }
                else
                {
                    NetworkStream netStream = (NetworkStream)receivingStreams[sock].ReceiveStream;

                    if (type == SecurityStreamType.Ssl)
                    {
                        SslStream sslStream = new SslStream(new ETWStream(netStream));
                        ((SslStream)sslStream).AuthenticateAsServer(this.cert);
                        receivingStreams[sock].ReceiveStream = sslStream;
                    }

                    else if (type == SecurityStreamType.CredSsp)
                    {
                        string targetSPN = ConstValue.CREDSSP_SERVER_NAME_PREFIX + config.LocalIpAddress;
                        RdpbcgrServerCredSspStream credSspStream = new RdpbcgrServerCredSspStream(new ETWStream(netStream), targetSPN);
                        receivingStreams[sock].ReceiveStream = credSspStream;
                        credSspStream.Authenticate(cert);
                    }
                }
            }
        }


        /// <summary>
        /// Close the transport and release resources.
        /// </summary>
        public void Release()
        {
            this.Dispose();
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Creates a new Socket for a newly created connection and  a new thread to receive packet in the loop.
        /// </summary>
        private void AcceptLoop()
        {
            while (!exitLoop)
            {
                if (this.receivingStreams.Count >= config.MaxConnections)
                {
                    // not listen untill the current connections are less than the max value.
                    // the interval to query is 1 seconds:
                    Thread.Sleep(1000);
                    continue;
                }

                Socket socket = null;
                try
                {
                    socket = this.listenSock.Accept();
                }
                catch (SocketException)
                {
                    exitLoop = true;
                    continue;
                }

                TransportEvent connectEvent;

                Stream receiveStream = null;
                Stream baseStream = new NetworkStream(socket);
                switch (streamType)
                {
                    case SecurityStreamType.None:
                        receiveStream = baseStream;
                        break;
                    case SecurityStreamType.Ssl:
                        receiveStream = new SslStream(
                            new ETWStream(
                            baseStream),
                            false
                            );
                        ((SslStream)receiveStream).AuthenticateAsServer(cert);
                        break;
                    case SecurityStreamType.CredSsp:
                        string targetSPN = ConstValue.CREDSSP_SERVER_NAME_PREFIX + config.LocalIpAddress;
                        RdpbcgrServerCredSspStream credSspStream = new RdpbcgrServerCredSspStream(new ETWStream(baseStream), targetSPN);
                        credSspStream.Authenticate(cert);
                        receiveStream = credSspStream;
                        break;
                    default:
                        receiveStream = baseStream;
                        break;
                }

                RdpbcgrReceiveThread receiveThread = new RdpbcgrReceiveThread(
                    socket.RemoteEndPoint,
                    this.packetQueue,
                    this.decoder,
                    receiveStream,
                    this.config.BufferSize,
                    this.rdpbcgrServer);
                
                connectEvent = new TransportEvent(EventType.Connected, socket.RemoteEndPoint, null);

                RdpbcgrServerSessionContext session = new RdpbcgrServerSessionContext();
                session.Identity = connectEvent.EndPoint;
                session.Server = this.rdpbcgrServer;
                session.LocalIdentity = socket.LocalEndPoint;
                session.IsClientToServerEncrypted = this.rdpbcgrServer.IsClientToServerEncrypted;
                this.rdpbcgrServer.ServerContext.AddSession(session);

                this.packetQueue.AddObject(connectEvent);

                lock (this.receivingStreams)
                {
                    this.receivingStreams.Add(socket, receiveThread);
                }

                receiveThread.Start();
            }
        }


        /// <summary>
        /// Add the given stack packets to the QueueManager object.
        /// </summary>
        /// <param name="endPoint">The endpoint of the packets</param> 
        /// <param name="packets">Decoded packets</param>
        private void AddPacketToQueueManager(object endPoint, StackPacket[] packets)
        {
            if (packets == null)
            {
                return;
            }

            foreach (StackPacket packet in packets)
            {
                TransportEvent packetEvent = new TransportEvent(EventType.ReceivedPacket, endPoint, packet);
                this.packetQueue.AddObject(packetEvent);
            }
        }


        /// <summary>
        /// Confirm the server is started
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Thrown when transport is not started.
        /// </exception>
        private void ValidServerHasStarted()
        {
            if (!this.isTcpServerTransportStarted)
            {
                throw new InvalidOperationException(
                    "the server is not started! please call Start() to start the server first.");
            }
        }


        /// <summary>
        /// Get an existed stream object in the socket list through the ip address and port.
        /// </summary>
        /// <param name="address">The IP address of the remote host.</param>
        /// <param name="port">The port of the remote host.</param>
        /// <returns>
        /// Return the existed stream if it exists in the remoteSockets list.
        /// Otherwise return null.
        /// </returns>
        private Stream GetStream(IPAddress address, int port)
        {
            foreach (Socket sock in this.receivingStreams.Keys)
            {
                IPEndPoint endPoint = (IPEndPoint)sock.RemoteEndPoint;
                if ((address == endPoint.Address) && (port == endPoint.Port))
                {
                    return receivingStreams[sock].ReceiveStream;
                }
            }
            return null;
        }


        /// <summary>
        /// Get an existed thread object in the socket list through the ip address and port.
        /// </summary>
        /// <param name="address">The IP address of the remote host.</param>
        /// <param name="port">The port of the remote host.</param>
        /// <returns>
        /// Return the existed stream if it exists in the remoteSockets list.
        /// Otherwise return null.
        /// </returns>
        public RdpbcgrReceiveThread GetThread(object endPoint)
        {
            foreach (Socket sock in this.receivingStreams.Keys)
            {
                if (endPoint == sock.RemoteEndPoint)
                {
                    return receivingStreams[sock];
                }
            }
            return null;
        }
        #endregion
    }


    /// <summary>
    /// RdpbcgrReceiveThread is a new thread class used to receive data.
    /// </summary>
    internal sealed class RdpbcgrReceiveThread : IDisposable
    {
        #region Field members
        private bool disposed;
        private QueueManager packetQueue;
        private DecodePacketCallback decoder;
        private object endPointIdentity;
        private Thread receivingThread;
        private Stream receiveStream;
        private int maxBufferSize;
        private volatile bool exitLoop;
        // Give RdpbcgrReceiveThread object a reference of RdpbcgrServer, so that it can access necessary variable
        private RdpbcgrServer rdpbcgrServer;
        #endregion


        #region Properties
        /// <summary>
        /// The receiving thread.
        /// </summary>
        public Thread ReceivingThread
        {
            get
            {
                return receivingThread;
            }
        }


        /// <summary>
        /// The receive stream responded for data receiving.
        /// </summary>
        public Stream ReceiveStream
        {
            get
            {
                return receiveStream;
            }
            set
            {
                receiveStream = value;
            }
        }
        #endregion


        #region Constructor and Dispose
        /// <summary>
        /// Create a new thread to receive the data from remote host.
        /// </summary>
        /// <param name="packetQueueManager">Store all event packets generated in receiving loop.</param>
        /// <param name="decodePacketCallback">Callback of packet decode.</param>
        /// <param name="stream">The stream used to receive data.</param>
        /// <param name="bufferSize">Buffer size used in receiving data.</param>
        public RdpbcgrReceiveThread(
            object endPoint,
            QueueManager packetQueueManager,
            DecodePacketCallback decodePacketCallback,
            Stream stream,
            int bufferSize,
            RdpbcgrServer rdpbcgrServer)
        {
            if (packetQueueManager == null)
            {
                throw new ArgumentNullException("packetQueueManager");
            }

            if (decodePacketCallback == null)
            {
                throw new ArgumentNullException("decodePacketCallback");
            }

            // Initialize variable.
            this.endPointIdentity = endPoint;
            this.packetQueue = packetQueueManager;
            this.decoder = decodePacketCallback;
            this.receiveStream = stream;
            this.maxBufferSize = bufferSize;
            this.rdpbcgrServer = rdpbcgrServer;
            this.receivingThread = new Thread((new ThreadStart(ReceiveLoop)));
        }


        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            //Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.receivingThread != null)
                    {
                        this.Abort();
                        this.receivingThread = null;
                    }
                }

                this.disposed = true;
            }
        }


        /// <summary>
        /// This destructor will get called only from the finalization queue.
        /// </summary>
        ~RdpbcgrReceiveThread()
        {
            this.Dispose(false);
        }
        #endregion


        #region User interface
        /// <summary>
        /// Start the receiving thread.
        /// </summary>
        public void Start()
        {
            this.receivingThread.Start();
        }


        /// <summary>
        /// Abort the receiving thread.
        /// </summary>
        public void Abort()
        {
            this.exitLoop = true;
            this.receiveStream.Close();
            if (this.receivingThread.ThreadState != ThreadState.Unstarted)
            {
                this.receivingThread.Abort();
                this.receivingThread.Join();
            }
        }
        

        /// <summary>
        /// Receive data, decode Packet and add them to QueueManager in the loop.
        /// </summary>
        private void ReceiveLoop()
        {
            StackPacket[] packets = null;
            ReceiveStatus receiveStatus = ReceiveStatus.Success;
            //object endPoint = null;
            int bytesRecv = 0;
            int leftCount = 0;
            int expectedLength = this.maxBufferSize;
            byte[] receivedCaches = new byte[0];

            while (!exitLoop)
            {
                if (expectedLength <= 0)
                {
                    expectedLength = this.maxBufferSize;
                }

                byte[] receivingBuffer = new byte[expectedLength];
                try
                {
                    bytesRecv = this.receiveStream.Read(receivingBuffer, 0, receivingBuffer.Length);

                    if (bytesRecv == 0)
                    {
                        receiveStatus = ReceiveStatus.Disconnected;
                    }
                    else
                    {
                        receiveStatus = ReceiveStatus.Success;
                    }
                }
                catch (System.IO.IOException)
                {
                    // If this is an IOException, treat it as a disconnection.
                    if (!exitLoop)
                    {
                        if (this.packetQueue != null)
                        {
                            TransportEvent exceptionEvent = new TransportEvent(EventType.Disconnected, this.endPointIdentity, null);
                            this.packetQueue.AddObject(exceptionEvent);
                            break;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!exitLoop)
                    {
                        if (this.packetQueue != null)
                        {
                            TransportEvent exceptionEvent = new TransportEvent(EventType.Exception, this.endPointIdentity, e);
                            this.packetQueue.AddObject(exceptionEvent);
                            break;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                if (receiveStatus == ReceiveStatus.Success)
                {
                    byte[] data = new byte[bytesRecv + receivedCaches.Length];
                    Array.Copy(receivedCaches, data, receivedCaches.Length);
                    Array.Copy(receivingBuffer, 0, data, receivedCaches.Length, bytesRecv);
                    while (true)
                    {
                        int consumedLength;

                        try 
                        {
                            packets = this.decoder(this.endPointIdentity, data, out consumedLength, out expectedLength);
                        }
                        catch (Exception e)
                        {
                            TransportEvent exceptionEvent = new TransportEvent(EventType.Exception, this.endPointIdentity, e);
                            this.packetQueue.AddObject(exceptionEvent);

                            // If decoder throw exception, we think decoder will throw exception again when it decode
                            // subsequent received data So here we terminate the receive thread.
                            return;
                        }

                        if (consumedLength > 0)
                        {
                            //add packet to queue
                            if (packets != null && packets.Length > 0)
                            {
                                AddPacketToQueueManager(this.endPointIdentity, packets);                                
                                bytesRecv = 0;
                                foreach (StackPacket pdu in packets)
                                {
                                    if (pdu.GetType() == typeof(Client_X_224_Connection_Request_Pdu))
                                    {
                                        // Block the thread if received a Client X224 Connection Request PDU
                                        // the main thread will resume the thread after it send X224 Connection confirm PDU and other necessary process, such as TLS Handshake
                                        rdpbcgrServer.ReceiveThreadControlEvent.WaitOne();
                                    }
                                }
                            }

                            //check if continue the decoding
                            leftCount = data.Length - consumedLength;
                            if (leftCount <= 0)
                            {
                                receivedCaches = new byte[0];
                                data = new byte[0];
                                break;
                            }
                            else
                            {
                                // Update caches contents to the bytes which is not consumed
                                receivedCaches = new byte[leftCount];
                                Array.Copy(data, consumedLength, receivedCaches, 0, leftCount);
                                data = new byte[receivedCaches.Length];
                                Array.Copy(receivedCaches, data, data.Length);
                            }
                        }
                        else
                        {
                            //if no data consumed, it means the left data cannot be decoded separately, so cache it and jump out.
                            receivedCaches = new byte[data.Length];
                            Array.Copy(data, receivedCaches, receivedCaches.Length);
                            break;
                        }
                    }
                }
                else if (receiveStatus == ReceiveStatus.Disconnected)
                {
                    if (this.packetQueue != null)
                    {
                        TransportEvent disconnectEvent = new TransportEvent(EventType.Disconnected, this.endPointIdentity, null);
                        this.packetQueue.AddObject(disconnectEvent);
                        break;
                    }
                    else
                    {
                        throw new InvalidOperationException("The transport is disconnected by remote host.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unknown status returned from receiving method.");
                }
            }
        }


        /// <summary>
        /// Add the given stack packets to the QueueManager object if packet type not in filters or Customize filter.
        /// </summary>
        /// <param name="endPoint">the endpoint of the packets</param>
        /// <param name="packets">decoded packets</param>
        private void AddPacketToQueueManager(object endPoint, StackPacket[] packets)
        {
            if (packets == null)
            {
                return;
            }

            foreach (StackPacket packet in packets)
            {
                TransportEvent packetEvent = new TransportEvent(EventType.ReceivedPacket, endPoint, packet);
                this.packetQueue.AddObject(packetEvent);
            }
        }
        #endregion
    }
}
