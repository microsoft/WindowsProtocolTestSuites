// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the endpoint data stores data from endpoint<para/>
    /// it stores a payload, local endpoint and the remote endpoint.<para/>
    /// it's used by all transport: Tcp, Udp and Netbios.
    /// </summary>
    /// <typeparam name="DataType">
    /// the type of stored data. for tcp/netbios, it's StackPacket. for udp, it's bytes array.
    /// </typeparam>
    /// <typeparam name="EndPointType">
    /// the type of endpoint.  for tcp/udp, it's IPEndPoint. for netbios, it's int.
    /// </typeparam>
    internal class EndPointData<EndPointType, DataType>
    {
        #region Fields

        /// <summary>
        /// a DataType that specifies the packet from client.
        /// </summary>
        private DataType packet;

        /// <summary>
        /// an EndPointType object that specifies the remote endpoint of packet.
        /// </summary>
        private EndPointType remoteEndPoint;

        /// <summary>
        /// an EndPointType object that specifies the local endpoint of packet.
        /// </summary>
        private EndPointType localEndPoint;

        #endregion

        #region Properties

        /// <summary>
        /// get an EndPointType object that specifies the remote endpoint of packet.
        /// </summary>
        public EndPointType RemoteEndPoint
        {
            get
            {
                return this.remoteEndPoint;
            }
        }


        /// <summary>
        /// get an EndPointType object that specifies the local endpoint of packet.
        /// </summary>
        public EndPointType LocalEndPoint
        {
            get
            {
                return this.localEndPoint;
            }
        }


        /// <summary>
        /// get a DataType that specifies the packet from client.
        /// </summary>
        public DataType Packet
        {
            get
            {
                return this.packet;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="stackPacket">
        /// a DataType that specifies the packet from client.
        /// </param>
        /// <param name="remoteEP">
        /// an EndPointType object that specifies the remote endpoint of packet.
        /// </param>
        /// <param name="localEP">
        /// an EndPointType object that specifies the local endpoint of packet.
        /// </param>
        public EndPointData(DataType stackPacket, EndPointType remoteEP, EndPointType localEP)
        {
            this.packet = stackPacket;
            this.remoteEndPoint = remoteEP;
            this.localEndPoint = localEP;
        }

        #endregion
    }

    /// <summary>
    /// this class specifies the udp client received bytes data from udp server.<para/>
    /// which contains the endpoint and bytes data.<para/>
    /// UdpClient will use it.
    /// </summary>
    internal class UdpReceivedBytes : EndPointData<IPEndPoint, byte[]>
    {
        #region Constructos

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">
        /// a bytes array that contains the data of Udp.
        /// </param>
        /// <param name="remoteEP">
        /// an IPEndPoint object that specifies the remote endpoint of data.
        /// </param>
        /// <param name="localEP">
        /// an IPEndPoint object that specifies the local endpoint of data.
        /// </param>
        public UdpReceivedBytes(byte[] data, IPEndPoint remoteEP, IPEndPoint localEP)
            : base(data, remoteEP, localEP)
        {
        }

        #endregion
    }

    /// <summary>
    /// this class specifies the packet that stores in the packet cache,<para/>
    /// when decoder return more than one packet, the packets are stored in the packet cache.<para/>
    /// NetbiosServer will use it.
    /// </summary>
    internal class NetbiosServerStackPacket : EndPointData<int, StackPacket>
    {
        #region Constructors

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="stackPacket">
        /// a StackPacket that specifies the packet from client.
        /// </param>
        /// <param name="remoteEP">
        /// an int value that specifies the remote endpoint of packet.
        /// </param>
        /// <param name="localEP">
        /// an int value that specifies the local endpoint of packet.
        /// </param>
        public NetbiosServerStackPacket(StackPacket stackPacket, int remoteEP, int localEP)
            : base(stackPacket, remoteEP, localEP)
        {
        }

        #endregion
    }

    /// <summary>
    /// this class specifies the packet that stores in the packet cache,<para/>
    /// when decoder return more than one packet, the packets are stored in the packet cache.<para/>
    /// TcpServer, UdpServer, UdpClient will use it.
    /// </summary>
    internal class IPEndPointStackPacket : EndPointData<IPEndPoint, StackPacket>
    {
        #region Constructors

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="stackPacket">
        /// a StackPacket that specifies the packet from client.
        /// </param>
        /// <param name="remoteEP">
        /// an IPEndPoint object that specifies the remote endpoint of packet.
        /// </param>
        /// <param name="localEP">
        /// an IPEndPoint object that specifies the local endpoint of packet.
        /// </param>
        public IPEndPointStackPacket(StackPacket stackPacket, IPEndPoint remoteEP, IPEndPoint localEP)
            : base(stackPacket, remoteEP, localEP)
        {
        }

        #endregion
    }
}
