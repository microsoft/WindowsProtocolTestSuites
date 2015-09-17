// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the facade of transport that provides interfaces to user.<para/>
    /// supported transport type: Tcp, Udp, Netbios and Stream.<para/>
    /// supported features:<para/>
    /// * basic transport as bytes or packet.<para/>
    /// * tcp server listening on multiple ports.<para/>
    /// * tcp over ssl.<para/>
    /// * netbios server listening on multiple names/network-adapter.<para/>
    /// * udp server listening on multiple ports.<para/>
    /// * LSP.
    /// </summary>
    public class TransportStack : IDisposable
    {
        #region Fields

        /// <summary>
        /// an ITransprot interfaces that specifies the underlayer transport services.
        /// </summary>
        private ITransport transport;

        /// <summary>
        /// a PacketFilter that is used to filter the packet.
        /// </summary>
        private PacketFilter packetFilter;

        #endregion

        #region Properties

        /// <summary>
        /// get/set a Packet filter. This filter is used in the call of ExpectPacket. 
        /// Packets in types of the filter will be dropped.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public virtual PacketFilter PacketFilter
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TransportStack");
                }

                return this.packetFilter;
            }
            set
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TransportStack");
                }

                this.packetFilter = value;
            }
        }


        /// <summary>
        /// get/set a ConnectionFilter filter. This filter is used when receiving connection. 
        /// Packets from the connections of the filter will be dropped.<para/>
        /// it's not implemented yet.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="NotImplementedException">
        /// thrown when get/set the ConnectionFilter, it's NotImplemented yet.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public virtual ConnectionFilter ConnectionFilter
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TransportStack");
                }

                throw new NotImplementedException();
            }
            set
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TransportStack");
                }

                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// get a bool value that indicates whether there is data received from transport.<para/>
        /// donot include the event such as Exception Connected and Disconnected.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public virtual bool IsDataAvailable
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("TransportStack");
                }

                return this.transport.IsDataAvailable;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// the default constructor, do nothing.<para/>
        /// the mock class can invoke this constructor.
        /// </summary>
        protected TransportStack()
        {
        }


        /// <summary>
        /// Constructor. To create the instance of transport specified in the transportConfig.
        /// </summary>
        /// <param name="transportConfig">
        /// a TransportConfig object that specifies the transport type and parameters.
        /// </param>
        /// <param name="decodePacketCallback">
        /// a DecodePacketCallback delegate that is used to decode the packet from bytes.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// thrown when StackTransportType is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when Role is invalid, for transport type is Tcp, it must be Server or Client.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when Role is invalid, for transport type is Netbios, it must be Server or Client.
        /// </exception>
        public TransportStack(TransportConfig transportConfig, DecodePacketCallback decodePacketCallback)
        {
            switch (transportConfig.Type)
            {
                case StackTransportType.Tcp:

                    if (transportConfig.Role == Role.Client)
                    {
                        this.transport = new TcpClientTransport(transportConfig, decodePacketCallback);
                    }
                    else if (transportConfig.Role == Role.Server)
                    {
                        this.transport = new TcpServerTransport(transportConfig, decodePacketCallback);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Role is invalid, for transport type is Tcp, it must be Server or Client.");
                    }

                    break;

                case StackTransportType.Netbios:

                    if (transportConfig.Role == Role.Client)
                    {
                        this.transport = new NetbiosClientTransport(transportConfig, decodePacketCallback);
                    }
                    else if (transportConfig.Role == Role.Server)
                    {
                        this.transport = new NetbiosServerTransport(transportConfig, decodePacketCallback);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "Role is invalid, for transport type is Netbios, it must be Server or Client.");
                    }

                    break;

                case StackTransportType.Stream:

                    this.transport = new StreamTransport(transportConfig, decodePacketCallback);

                    break;

                case StackTransportType.Udp:

                    if (transportConfig.Role == Role.Server)
                    {
                        this.transport = new UdpServerTransport(transportConfig, decodePacketCallback);
                    }
                    // all other role are equals to Client.
                    else
                    {
                        this.transport = new UdpClientTransport(transportConfig, decodePacketCallback);
                    }

                    break;

                default:
                    throw new InvalidOperationException("StackTransportType is invalid.");
            }
        }

        #endregion

        #region Methods

        #region IConnectable

        /// <summary>
        /// connect to remote endpoint.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.<para/>
        /// the return value must be:<para/>
        /// if Tcp client, it's an IPEndPoint object that specifies the local client endpoint.<para/>
        /// if Netbios, it's an int value that specifies the remote server session id.<para/>
        /// if Stream, it's an int value that specifies the local identity of stream.
        /// </summary>
        /// <returns>
        /// the endpoint of the connection.<para/>
        /// if Tcp client, it's an IPEndPoint object that specifies the local client endpoint.<para/>
        /// if Netbios, it's an int value that specifies the remote server session id.<para/>
        /// if Stream, it's an int value that specifies the local identity of stream.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, Stream and NetbiosClient
        /// </exception>
        public virtual object Connect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IConnectable connectable = this.transport as IConnectable;

            if (connectable == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpClient, Stream and NetbiosClient");
            }

            return connectable.Connect();
        }

        #endregion

        #region IDisconnectable

        /// <summary>
        /// disconnect from remote host.<para/>
        /// the underlayer transport must be TcpClient, Stream, NetbiosClient, TcpServer or NetbiosServer.<para/>
        /// client side will disconnect the connection to server.<para/>
        /// server side will disconnect all client connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, Stream, NetbiosClient, TcpServer and NetbiosServer.
        /// </exception>
        public virtual void Disconnect()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IDisconnectable disconnectable = this.transport as IDisconnectable;

            if (disconnectable == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpClient, Stream, NetbiosClient, TcpServer and NetbiosServer.");
            }

            disconnectable.Disconnect();
        }


        /// <summary>
        /// expect the server to disconnect<para/>
        /// the underlayer transport must be TcpClient, Stream, NetbiosClient, TcpServer or NetbiosServer.<para/>
        /// client side will expect the disconnection from server.<para/>
        /// server side will expect the disconnection from any client.<para/>
        /// the return value must be:<para/>
        /// if Tcp server, return an IPEndPoint object that specifies the remote client endpoint<para/>
        /// if Tcp client, return an IPEndPoint object that specifies the local endpoint.<para/>
        /// if Netbios server, return an int value that specifies the remote client session id.<para/>
        /// if Netbios client, return an int value that specifies the remote server session id.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <returns>
        /// return an object that is disconnected.<para/>
        /// if Tcp server, return an IPEndPoint object that specifies the remote client endpoint<para/>
        /// if Tcp client, return an IPEndPoint object that specifies the local endpoint.<para/>
        /// if Netbios server, return an int value that specifies the remote client session id.<para/>
        /// if Netbios client, return an int value that specifies the remote server session id.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, Stream, NetbiosClient, TcpServer and NetbiosServer.
        /// </exception>
        public virtual object ExpectDisconnect(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IDisconnectable disconnectable = this.transport as IDisconnectable;

            if (disconnectable == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpClient, Stream, NetbiosClient, TcpServer and NetbiosServer.");
            }

            return disconnectable.ExpectDisconnect(timeout);
        }

        #endregion

        #region IStartable

        /// <summary>
        /// to start the transport. <para/>
        /// the underlayer transport must be TcpServer, UdpClient, UdpServer or NetbiosServer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, UdpClient, UdpServer and NetbiosServer.
        /// </exception>
        public virtual void Start()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IStartable startable = this.transport as IStartable;

            if (startable == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, UdpClient, UdpServer and NetbiosServer.");
            }

            startable.Start();
        }


        /// <summary>
        /// start at the specified endpoint<para/>
        /// the underlayer transport must be TcpServer, UdpClient, UdpServer or NetbiosServer.
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the listener.<para/>
        /// if Tcp/Udp, it's an IPEndPoint object that specifies the local listening endpoint.<para/>
        /// if Tcp/Udp, it can be an int value that specifies the local listening port; and the
        /// address is specified in config.<para/>
        /// if Netbios, it's an string value that specifies the localNetbiosServerName to start.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, UdpClient, UdpServer and NetbiosServer.
        /// </exception>
        public virtual void Start(object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IStartable startable = this.transport as IStartable;

            if (startable == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, UdpClient, UdpServer and NetbiosServer.");
            }

            startable.Start(localEndPoint);
        }


        /// <summary>
        /// stop all listener of server.<para/>
        /// the underlayer transport must be TcpServer, UdpClient, UdpServer or NetbiosServer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, UdpClient, UdpServer and NetbiosServer.
        /// </exception>
        public virtual void Stop()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IStartable startable = this.transport as IStartable;

            if (startable == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, UdpClient, UdpServer and NetbiosServer.");
            }

            startable.Stop();
        }


        /// <summary>
        /// stop the specified listener.<para/>
        /// the underlayer transport must be TcpServer, UdpClient, UdpServer or NetbiosServer.
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the listener.<para/>
        /// if Tcp/Udp, it's an IPEndPoint object that specifies the local listening endpoint.<para/>
        /// if Tcp/Udp, it can be an int value that specifies the local listening port.
        /// all endpoint that on the port will be stopped.<para/>
        /// if Netbios, it's an string value that specifies the localNetbiosServerName to stop.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, UdpClient, UdpServer and NetbiosServer.
        /// </exception>
        public virtual void Stop(object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IStartable startable = this.transport as IStartable;

            if (startable == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, UdpClient, UdpServer and NetbiosServer.");
            }

            startable.Stop(localEndPoint);
        }

        #endregion

        #region IAcceptable

        /// <summary>
        /// expect connection from client.<para/>
        /// the underlayer transport must be TcpServer or NetbiosServer.<para/>
        /// the return value must be:<para/>
        /// if Tcp, return an IPEndPoint that specifies the client endpoint.<para/>
        /// if Netbios, return an int value that specifies the client session id.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout.
        /// </param>
        /// <returns>
        /// an object that contains the connection.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        public virtual object ExpectConnect(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IAcceptable acceptable = this.transport as IAcceptable;

            if (acceptable == null)
            {
                throw new InvalidOperationException("the underlayer transport is not TcpServer and NetbiosServer.");
            }

            return acceptable.ExpectConnect(timeout);
        }


        /// <summary>
        /// expect the server to disconnect<para/>
        /// the underlayer transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies which endpoint is expected to be disconnected.<para/>
        /// if Tcp, it's an IPEndPoint that specifies the client endpoint.<para/>
        /// if Netbios, it's an int value that specifies the remote client session id.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        public virtual void ExpectDisconnect(TimeSpan timeout, object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IAcceptable acceptable = this.transport as IAcceptable;

            if (acceptable == null)
            {
                throw new InvalidOperationException("the underlayer transport is not TcpServer and NetbiosServer.");
            }

            acceptable.ExpectDisconnect(timeout, remoteEndPoint);
        }


        /// <summary>
        /// disconnect from a special remote host.<para/>
        /// the underlayer transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="remoteEndPoint">
        /// an object that specifies the endpoint to be disconnected.<para/>
        /// if Tcp, it's an IPEndPoint that specifies the client endpoint.<para/>
        /// if Netbios, it's an int value that specifies the remote client session id.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        public virtual void Disconnect(object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            IAcceptable acceptable = this.transport as IAcceptable;

            if (acceptable == null)
            {
                throw new InvalidOperationException("the underlayer transport is not TcpServer and NetbiosServer.");
            }

            acceptable.Disconnect(remoteEndPoint);
        }

        #endregion

        #region ISourceSend

        /// <summary>
        /// Send a packet over the transport.<para/>
        /// the underlayer transport must be TcpClient, UdpClient, Stream or NetbiosClient.<para/>
        /// the UdpClient will send data to the remote endpoint that stored in config.
        /// </summary>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when packet is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, UdpClient, Stream and NetbiosClient.
        /// </exception>
        public virtual void SendPacket(StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            ISourceSend source = this.transport as ISourceSend;

            if (source == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpClient, UdpClient, Stream and NetbiosClient.");
            }

            if (packet.PacketBytes != null)
            {
                source.SendBytes(packet.PacketBytes);
            }
            else
            {
                source.SendPacket(packet);
            }
        }


        /// <summary>
        /// Send an arbitrary message over the transport.<para/>
        /// the underlayer transport must be TcpClient, UdpClient, Stream or NetbiosClient.<para/>
        /// the UdpClient will send data to the remote endpoint that stored in config.
        /// </summary>
        /// <param name="message">
        /// a bytes array that contains the data to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, UdpClient, Stream and NetbiosClient.
        /// </exception>
        public virtual void SendBytes(byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            ISourceSend source = this.transport as ISourceSend;

            if (source == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpClient, UdpClient, Stream and NetbiosClient.");
            }

            source.SendBytes(message);
        }

        #endregion

        #region ISourceReceive

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
        /// thrown when the underlayer transport is not TcpClient, Stream and NetbiosClient.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        public virtual byte[] ExpectBytes(TimeSpan timeout, int maxCount)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ISourceReceive source = this.transport as ISourceReceive;

            if (source == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpClient, Stream and NetbiosClient.");
            }

            if (maxCount < 0)
            {
                throw new ArgumentException("max count must not be negative", "maxCount");
            }

            return source.ExpectBytes(timeout, maxCount);
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
        /// thrown when the underlayer transport is not TcpClient, Stream and NetbiosClient.
        /// </exception>
        public virtual StackPacket ExpectPacket(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ISourceReceive source = this.transport as ISourceReceive;

            if (source == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpClient, Stream and NetbiosClient.");
            }

            return source.ExpectPacket(timeout);
        }

        #endregion

        #region ITarget

        /// <summary>
        /// Send a packet to a special remote host.<para/>
        /// the transport must be a TcpServer, NetbiosServer or UdpClient.
        /// </summary>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send packet.<para/>
        /// if Tcp/Udp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's an int value that specifies the remote client session id.
        /// </param>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when packet is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, NetbiosServer and UdpClient.
        /// </exception>
        public virtual void SendPacket(object remoteEndPoint, StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            ITarget target = this.transport as ITarget;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, NetbiosServer and UdpClient.");
            }

            target.SendPacket(remoteEndPoint, packet);
        }


        /// <summary>
        /// Send arbitrary message to a special remote host.<para/>
        /// the transport must be a TcpServer, NetbiosServer or UdpClient.
        /// </summary>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send bytes.<para/>
        /// if Tcp/Udp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's an int value that specifies the remote client session id.
        /// </param>
        /// <param name="message">
        /// a bytes array that contains the bytes to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, NetbiosServer and UdpClient.
        /// </exception>
        public virtual void SendBytes(object remoteEndPoint, byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            ITarget target = this.transport as ITarget;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, NetbiosServer and UdpClient.");
            }

            target.SendBytes(remoteEndPoint, message);
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be a TcpServer, NetbiosServer or UdpClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.<para/>
        /// if Udp, return the whole received bytes, even though the maxCount is smaller that the bytes length.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that indicates the connection received data from.<para/>
        /// if Tcp/Udp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's an int value that specifies the remote client session id.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, NetbiosServer and UdpClient.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public virtual byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ITarget target = this.transport as ITarget;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, NetbiosServer and UdpClient.");
            }

            if (maxCount < 0)
            {
                throw new ArgumentException("max count must not be negative", "maxCount");
            }

            return target.ExpectBytes(timeout, maxCount, out remoteEndPoint);
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer, NetbiosServer or UdpClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the connection expected packet.<para/>
        /// if Tcp/Udp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's an int value that specifies the remote client session id.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, NetbiosServer and UdpClient.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public virtual StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ITarget target = this.transport as ITarget;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, NetbiosServer and UdpClient.");
            }

            return target.ExpectPacket(timeout, out remoteEndPoint);
        }

        #endregion

        #region ITargetReceive

        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer, NetbiosServer or UdpServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.<para/>
        /// if Tcp/Udp, it's an IPEndPoint object.<para/>
        /// if Netbios, it's an int value that specifies the remote netbios client session id.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.<para/>
        /// if Tcp/Udp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's the ncb_num that specifies the number for the local network name.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, NetbiosServer and UdpServer.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public virtual StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint, out object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ITargetReceive target = this.transport as ITargetReceive;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, NetbiosServer and UdpServer.");
            }

            return target.ExpectPacket(timeout, out remoteEndPoint, out localEndPoint);
        }


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be a TcpServer, NetbiosServer or UdpServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.<para/>
        /// if Udp, return the whole received bytes, even though the maxCount is smaller that the bytes length.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.<para/>
        /// if Tcp/Udp, it's an IPEndPoint object.<para/>
        /// if Netbios, it's an int value that specifies the remote netbios session id.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.<para/>
        /// if Tcp/Udp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's int(0) that specifies the server netbios session id.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, NetbiosServer and UdpServer.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public virtual byte[] ExpectBytes(
            TimeSpan timeout, int maxCount, out object remoteEndPoint, out object localEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ITargetReceive target = this.transport as ITargetReceive;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer, NetbiosServer and UdpServer.");
            }

            if (maxCount < 0)
            {
                throw new ArgumentException("max count must not be negative", "maxCount");
            }

            return target.ExpectBytes(timeout, maxCount, out remoteEndPoint, out localEndPoint);
        }

        #endregion

        #region ITargetSend

        /// <summary>
        /// Send a packet to a special remote host.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the local endpoint to send bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send packet.
        /// </param>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when packet is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not UdpServer.
        /// </exception>
        public virtual void SendPacket(object localEndPoint, object remoteEndPoint, StackPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            ITargetSend target = this.transport as ITargetSend;

            if (target == null)
            {
                throw new InvalidOperationException("the underlayer transport is not UdpServer.");
            }

            target.SendPacket(localEndPoint, remoteEndPoint, packet);
        }


        /// <summary>
        /// Send arbitrary message to a special remote host.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the local endpoint to send bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send bytes.
        /// </param>
        /// <param name="message">
        /// a bytes array that contains the bytes to send to target.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not UdpServer.
        /// </exception>
        public virtual void SendBytes(object localEndPoint, object remoteEndPoint, byte[] message)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            ITargetSend target = this.transport as ITargetSend;

            if (target == null)
            {
                throw new InvalidOperationException("the underlayer transport is not UdpServer.");
            }

            target.SendBytes(localEndPoint, remoteEndPoint, message);
        }

        #endregion

        #region ISpecialTarget

        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that indicates the connection to received data from.<para/>
        /// if Tcp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's an int value that specifies the client session id.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        public virtual byte[] ExpectBytes(TimeSpan timeout, int maxCount, object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ISpecialTarget target = this.transport as ISpecialTarget;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer and NetbiosServer.");
            }

            if (maxCount < 0)
            {
                throw new ArgumentException("max count must not be negative", "maxCount");
            }

            return target.ExpectBytes(timeout, maxCount, remoteEndPoint);
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the connection to expect packet.<para/>
        /// if Tcp, it's an IPEndPoint that specifies the target endpoint.<para/>
        /// if Netbios, it's an int value that specifies the client session id.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        public virtual StackPacket ExpectPacket(TimeSpan timeout, object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ISpecialTarget target = this.transport as ISpecialTarget;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is not TcpServer and NetbiosServer.");
            }

            return target.ExpectPacket(timeout, remoteEndPoint);
        }

        #endregion

        #region ILocalTarget

        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the local listening endpoint to received bytes.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.<para/>
        /// return the whole received bytes, even though the maxCount is smaller that the bytes length.
        /// </param>
        /// <param name="remoteEndPoint">
        /// return an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the remote listening endpoint to received bytes.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not UdpServer
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public virtual byte[] ExpectBytes(
            TimeSpan timeout, int maxCount, object localEndPoint, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ILocalTarget target = this.transport as ILocalTarget;

            if (target == null)
            {
                throw new InvalidOperationException("the underlayer transport is not UdpServer.");
            }

            if (maxCount < 0)
            {
                throw new ArgumentException("max count must not be negative", "maxCount");
            }

            return target.ExpectBytes(timeout, maxCount, localEndPoint, out remoteEndPoint);
        }


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the local listening endpoint to received bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// return an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the remote listening endpoint to received bytes.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not UdpServer
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public virtual StackPacket ExpectPacket(TimeSpan timeout, object localEndPoint, out object remoteEndPoint)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            ILocalTarget target = this.transport as ILocalTarget;

            if (target == null)
            {
                throw new InvalidOperationException("the underlayer transport is not UdpServer.");
            }

            return target.ExpectPacket(timeout, localEndPoint, out remoteEndPoint);
        }

        #endregion

        #region ISsl Client and Server

        /// <summary>
        /// startup the ssl security provider.<para/>
        /// must invoke it before connect to server.<para/>
        /// the underlayer transport must be TcpClient.
        /// </summary>
        /// <param name="targetHost">
        /// a string that indicates the name of the server that shares the SSL/TLS.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when targetHost is null.
        /// </exception>
        public virtual void SslStartupAsClient(string targetHost)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (targetHost == null)
            {
                throw new ArgumentNullException("targetHost");
            }

            ISslClient client = this.transport as ISslClient;

            if (client == null)
            {
                throw new InvalidOperationException("the underlayer transport is not TcpClient.");
            }

            client.Startup(targetHost);
        }


        /// <summary>
        /// startup the ssl security provider.<para/>
        /// must invoke it before connect to server.<para/>
        /// the underlayer transport must be TcpClient.
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
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when targetHost is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient.
        /// </exception>
        public virtual void SslStartupAsClient(
            string targetHost, X509Certificate certificate, SslProtocols enabledSslProtocols)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (targetHost == null)
            {
                throw new ArgumentNullException("targetHost");
            }

            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            ISslClient client = this.transport as ISslClient;

            if (client == null)
            {
                throw new InvalidOperationException("the underlayer transport is not TcpClient.");
            }

            client.Startup(targetHost, certificate, enabledSslProtocols);
        }


        /// <summary>
        /// startup the ssl security provider.<para/>
        /// must invoke it before start server.<para/>
        /// the underlayer transport must be TcpServer.
        /// </summary>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the server.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient.
        /// </exception>
        public virtual void SslStartupAsServer(X509Certificate certificate)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            ISslServer server = this.transport as ISslServer;

            if (server == null)
            {
                throw new InvalidOperationException("the underlayer transport is not TcpServer.");
            }

            server.Startup(certificate);
        }


        /// <summary>
        /// startup the ssl security provider.<para/>
        /// must invoke it before start server.<para/>
        /// the underlayer transport must be TcpServer.
        /// </summary>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the server.
        /// </param>
        /// <param name="clientCertificateRequired">
        /// A Boolean value that specifies whether the client must supply a certificate for authentication.
        /// </param>
        /// <param name="enabledSslProtocols">
        /// The SslProtocols value that represents the protocol used for authentication.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient.
        /// </exception>
        public virtual void SslStartupAsServer(
            X509Certificate certificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            ISslServer server = this.transport as ISslServer;

            if (server == null)
            {
                throw new InvalidOperationException("the underlayer transport is not TcpServer.");
            }

            server.Startup(certificate, clientCertificateRequired, enabledSslProtocols);
        }

        #endregion

        #region ITransport

        /// <summary>
        /// add transport event to transport, TSD can invoke ExpectTransportEvent to get it.
        /// </summary>
        /// <param name="transportEvent">
        /// a TransportEvent object that contains the event to add to the queue
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is null! the config for TransportStack is invalid.
        /// </exception>
        public virtual void AddEvent(TransportEvent transportEvent)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is null! the config for TransportStack is invalid.");
            }

            this.transport.AddEvent(transportEvent);
        }


        /// <summary>
        /// to update the config of transport at runtime.
        /// </summary>
        /// <param name="config">
        /// a TransportConfig object that contains the config to update
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is null! the config for TransportStack is invalid.
        /// </exception>
        public virtual void UpdateConfig(TransportConfig config)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is null! the config for TransportStack is invalid.");
            }

            this.transport.UpdateConfig(config);
        }


        /// <summary>
        /// expect transport event from transport.<para/>
        /// invoke ExpectConnect() to get the connected client,<para/>
        /// invoke ExpectDisconnect() to get the dis-connected client,<para/>
        /// invoke ExpectPacket/Bytes() to get data from client.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan struct that specifies the timeout of waiting for a packet or event from the transport.
        /// </param>
        /// <returns>
        /// a TransportEvent object that contains the expected event from transport.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is null! the config for TransportStack is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when Invalid object are cached in the queue.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when underlayer transport throw exception.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// throw when timeout to expect transport event.
        /// </exception>
        public virtual TransportEvent ExpectTransportEvent(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("TransportStack");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "the underlayer transport is null! the config for TransportStack is invalid.");
            }

            DateTime endTime = DateTime.Now + timeout;
            TimeSpan currentTimeout = timeout;

            while (true)
            {
                if (currentTimeout.Ticks < 0)
                {
                    throw new TimeoutException("TransportStack timeout to expect transport event.");
                }

                TransportEvent eventPacket = this.transport.ExpectTransportEvent(currentTimeout);

                // update the timeout.
                currentTimeout = endTime - DateTime.Now;

                if (eventPacket == null)
                {
                    throw new InvalidOperationException("Invalid object are cached in the queue.");
                }

                // Use PacketFilter if ReceivedPacket:
                // if the type of received packet is in types of the PacketFilter or the CustomizeFilter returns true,
                // this packet will be dropped.
                if (eventPacket.EventType == EventType.ReceivedPacket)
                {
                    StackPacket packet = eventPacket.EventObject as StackPacket;

                    if (this.packetFilter != null
                        && (this.packetFilter.FilterPacket(packet) || this.packetFilter.CustomizeFilter(packet)))
                    {
                        continue;
                    }
                }
                else if (eventPacket.EventType == EventType.Exception)
                {
                    throw new InvalidOperationException(
                        "There's an exception thrown when receiving a packet.",
                        (Exception)eventPacket.EventObject);
                }

                return eventPacket;
            }
        }

        #endregion

        #endregion

        #region Lsp Configurations

        /// <summary>
        /// Intercept the ip traffic in a designated address. 
        /// Sdk is listening to the same IP/port to the intercepted address. 
        /// </summary>
        /// <param name="transportType">TCP or UDP . </param>
        /// <param name="interceptedEndPoint">The intercepted IP/Port of the windows service . </param>
        /// <exception cref="NotSupportedException">Thrown when the transport doesn't support this 
        /// operation</exception>
        static public void InterceptTraffic(StackTransportType transportType, IPEndPoint interceptedEndPoint)
        {
            LspConsole.Instance.InterceptTraffic(transportType, false, interceptedEndPoint);
        }


        /// <summary>
        /// Change a hooked endpoint from intercepted mode to blocking mode
        /// </summary>
        /// <param name="transportType">Tcp or Udp</param>
        /// <param name="localServerEndpoint">the lsp hooked server endpoint</param>
        [SecurityPermission(SecurityAction.Demand)]
        static public void ChangeToBlockingMode(StackTransportType transportType, IPEndPoint localServerEndpoint)
        {
            LspConsole.Instance.ChangeToBlockingMode(localServerEndpoint, transportType);
        }


        /// <summary>
        /// Block the traffic on the specific endpoint.
        /// </summary>
        /// <param name="transportType">Tcp or Udp </param>
        /// <param name="interceptedEndPoint">Endpoint to intercept</param>
        static public void BlockTraffic(StackTransportType transportType, IPEndPoint interceptedEndPoint)
        {
            LspConsole.Instance.InterceptTraffic(transportType, true, interceptedEndPoint);
        }


        /// <summary>
        /// Exit intercepted or blocking mode on the specific endpoint.
        /// </summary>
        /// <param name="transportType">Tcp or Udp</param>
        /// <param name="localServerEndpoint">the lsp hooked server endpoint</param>
        [SecurityPermission(SecurityAction.Demand)]
        static public void UnblockTraffic(StackTransportType transportType, IPEndPoint localServerEndpoint)
        {
            LspConsole.Instance.UnblockTraffic(localServerEndpoint, transportType);
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
        ~TransportStack()
        {
            Dispose(false);
        }

        #endregion
    }
}
