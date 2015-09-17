// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    #region Enumerations
    /// <summary>
    /// Lsp message type enumerations
    /// </summary>
    public enum LspMessageType
    {
        /// <summary>
        /// Lsp InterceptionRequest
        /// </summary>
        InterceptionRequest,
        /// <summary>
        /// Lsp InterceptionResponse
        /// </summary>
        InterceptionResponse,
        /// <summary>
        /// Lsp BlockRequest
        /// </summary>
        BlockRequest,
        /// <summary>
        /// Lsp BlockResponse
        /// </summary>
        BlockResponse,
        /// <summary>
        /// Lsp UnblockRequest
        /// </summary>
        UnblockRequest,
        /// <summary>
        /// Lsp UnblockResponse
        /// </summary>
        UnblockResponse,
        /// <summary>
        /// Lsp RegistrationRequest
        /// </summary>
        RegistrationRequest,
        /// <summary>
        /// Lsp UnregistrationRequest
        /// </summary>
        UnregistrationRequest,
        /// <summary>
        /// Lsp UnregistrationResponse
        /// </summary>
        RetrieveEndPointRequest,
        /// <summary>
        /// Lsp RetrieveEndPointResponse
        /// </summary>
        RetrieveEndPointResponse,
    }
    #endregion

    /// <summary>
    /// A structure contains both protocol type and an IPEndPoint
    /// </summary>
    public struct ProtocolEndPoint
    {
        /// <summary>
        /// Protocol Type
        /// </summary>
        public StackTransportType protocolType;

        /// <summary>
        /// EndPoint
        /// </summary>
        public IPEndPoint endPoint;

        /// <summary>
        /// operator == overload
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        /// <returns>true if equal, else false</returns>
        public static bool operator ==(ProtocolEndPoint left, ProtocolEndPoint right)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(left, right))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }

            // Return true if the fields match
            if (IPAddress.Equals(left.endPoint.Address, right.endPoint.Address)
                && left.endPoint.Port == right.endPoint.Port
                && left.protocolType == right.protocolType)
                return true;
            else
                return false;
        }

        /// <summary>
        /// operator != overload
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right value</param>
        /// <returns>true if equal, else false</returns>
        public static bool operator !=(ProtocolEndPoint left, ProtocolEndPoint right)
        {
            return !(left == right);
        }

        /// <summary>
        /// equals
        /// </summary>
        /// <param name="obj">object to compared</param>
        /// <returns>true if equal, else false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is ProtocolEndPoint))
                return false;
            else
                return this.Equals((ProtocolEndPoint)obj);
        }

        /// <summary>
        /// equals
        /// </summary>
        /// <param name="pep">ProtocolEndPoint object to compared</param>
        /// <returns>true if equal, else false</returns>
        public bool Equals(ProtocolEndPoint pep)
        {
            if (base.Equals(pep)
                && IPAddress.Equals(this.endPoint.Address, pep.endPoint.Address)
                && this.endPoint.Port == pep.endPoint.Port
                && this.protocolType == pep.protocolType)
                return true;
            else
                return false;
        }

        /// <summary>
        /// override GetHashCode
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            if (this.endPoint != null)
                return base.GetHashCode() ^ this.protocolType.GetHashCode() ^ this.endPoint.GetHashCode();
            else
                return base.GetHashCode();
        }
    }

    /// <summary>
    /// A structure contains message header
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspMessageHeader
    {
        /// <summary>
        /// Message type
        /// </summary>
        public LspMessageType messageType;

        /// <summary>
        /// Message length
        /// </summary>
        public int messageLen;
    }

    /// <summary>
    /// A structure contains ip family, address and port
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    public struct IPEndPointMsg
    {
        /// <summary>
        /// Addresss family
        /// </summary>
        public int af;

        /// <summary>
        /// IP address in dot format
        /// </summary>
        [StringAttribute(StringEncoding.ASCII)]
        [StaticSize(AddressSize, StaticSizeMode.Elements)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = AddressSize)]
        public byte[] addr;

        /// <summary>
        /// IP port
        /// </summary>
        public ushort port;

        /// <summary>
        /// Padding
        /// </summary>
        public ushort padding;

        //ip address size, not included when marshal the struct
        const int AddressSize = 48;

        /// <summary>
        /// //transform IPEndPoint to IPEndPointMsg
        /// </summary>
        /// <param name="endpoint">IPEndPoint</param>
        public IPEndPointMsg(IPEndPoint endpoint)
        {
            if (endpoint == null)
            {
                this.af = (int)IPAddress.Loopback.AddressFamily;
                this.addr = new byte[AddressSize];
                this.port = 0;
                this.padding = 0;
                return;
            }

            this.af = (int)endpoint.AddressFamily;
            this.addr = new byte[AddressSize];
            byte[] endpointAddr = Encoding.ASCII.GetBytes(endpoint.Address.ToString());
            Array.Copy(endpointAddr, 0, this.addr, 0, endpointAddr.Length);
            this.port = (ushort)endpoint.Port;
            this.padding = 0;
        }


        /// <summary>
        /// transform IPEndPointMsg to IPEndPoint
        /// </summary>
        /// <returns>IPEndPoint</returns>
        public IPEndPoint ToIPEndPoint()
        {
            if (this.addr == null)
            {
                //fields uninitialized
                return null;
            }

            string s = new string(Encoding.ASCII.GetChars(this.addr));
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\0')
                {
                    s = new string(Encoding.ASCII.GetChars(this.addr), 0, i);
                    break;
                }
            }
            try
            {
                IPAddress endpointAddr = IPAddress.Parse(s);
                IPEndPoint endpoint = new IPEndPoint(endpointAddr, this.port);
                return endpoint;
            }
            catch (FormatException)
            {
            }

            return null;
        }
    }

    #region message definitions
    /// <summary>
    /// Base class of all Lsp messages.
    /// </summary>
    public abstract class LspMessage
    {
        /// <summary>
        /// type of the message.
        /// </summary>
        protected LspMessageType messageType;


        /// <summary>
        /// Message type enum
        /// </summary>
        public LspMessageType MessageType
        {
            get
            {
                return this.messageType;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Message type</param>
        protected LspMessage(LspMessageType type)
        {
            this.messageType = type;
        }


        /// <summary>
        /// Encode the Lsp message into byte array.
        /// </summary>
        /// <returns>Encoded byte array.</returns>
        public virtual byte[] Encode()
        {
            return null;
        }


        /// <summary>
        /// Decode byte array into Lsp message header
        /// </summary>
        /// <param name="rawData">byte array</param>
        /// <param name="header">message header</param>
        /// <returns>0 if it's of right format, otherwise -1</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static int DecodeMessageHeader(byte[] rawData, out LspMessageHeader header)
        {
            header = new LspMessageHeader();
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspMessageHeader)))
            {
                header.messageType = (LspMessageType)(-1);
                return -1;
            }

            header = TypeMarshal.ToStruct<LspMessageHeader>(rawData);
            return 0;
        }


        /// <summary>
        /// Receive data on a socket
        /// return until specific size of data is received or error occurs
        /// </summary>
        /// <param name="s">socket to receive data</param>
        /// <param name="messageSize">size of bytes to receive</param>
        /// <param name="buffer">message buffer</param>
        /// <returns>size of data received</returns>
        public static int ReceiveWholeMessage(Socket s, int messageSize, out byte[] buffer)
        {
            if (s == null || messageSize <= 0)
            {
                buffer = null;
                return -1;
            }

            int recvLen = 0;
            buffer = new byte[messageSize];
            while (recvLen < messageSize)
            {
                int ret = s.Receive(buffer, recvLen, messageSize - recvLen, SocketFlags.None);
                if (ret <= 0)
                    return recvLen;
                else
                    recvLen += ret;
            }

            return recvLen;
        }
    }
    
    #endregion
}