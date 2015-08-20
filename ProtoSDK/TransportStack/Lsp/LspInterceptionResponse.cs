// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// LspInterceptionResponse message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspInterceptionResponseMsg
    {
        /// <summary>
        /// Header
        /// </summary>
        public LspMessageHeader header;

        /// <summary>
        /// Session id
        /// </summary>
        public int sessionId;

        /// <summary>
        /// Status
        /// </summary>
        public int status;

        /// <summary>
        /// Transport type
        /// </summary>
        public StackTransportType transportType;

        /// <summary>
        /// Intercepted IPEndPoint
        /// </summary>
        public IPEndPointMsg interceptedEndPoint;
    }

    /// <summary>
    /// LspInterceptionResponse, sent by LspManagementClient.
    /// </summary>
    public class LspInterceptionResponse : LspMessage
    {
        private LspSession session;
        private ProtocolEndPoint interceptedEndPoint;
        private int status;

        /// <summary>
        /// LspSession ID
        /// </summary>
        public LspSession Session
        {
            get
            {
                return this.session;
            }
        }


        /// <summary>
        /// Property of response status.
        /// </summary>
        public int Status
        {
            get
            {
                return this.status;
            }
        }


        /// <summary>
        /// Intercepted Windows service IPEndPoint
        /// </summary>
        public ProtocolEndPoint InterceptedEndPoint
        {
            get
            {
                return this.interceptedEndPoint;
            }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="session">LspSession ID. </param>
        /// <param name="status">Status of response. </param>
        /// <param name="endPoint">endpoint of the intercept response</param>
        public LspInterceptionResponse(LspSession session, int status, ProtocolEndPoint endPoint)
            : base(LspMessageType.InterceptionResponse)
        {
            this.session = session;
            this.status = status;
            this.interceptedEndPoint = endPoint;
        }


        /// <summary>
        /// Encode the Lsp message into byte array.
        /// </summary>
        /// <returns>Encoded byte array.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public override byte[] Encode()
        {
            LspInterceptionResponseMsg msg;
            msg.header.messageType = LspMessageType.InterceptionResponse;
            msg.header.messageLen = Marshal.SizeOf(typeof(LspInterceptionResponseMsg));
            msg.sessionId = this.session.SessionId;
            msg.status = this.status;
            msg.transportType = this.interceptedEndPoint.protocolType;
            msg.interceptedEndPoint = new IPEndPointMsg(this.interceptedEndPoint.endPoint);

            return TypeMarshal.ToBytes<LspInterceptionResponseMsg>(msg);
        }


        /// <summary>
        /// Decode the received raw data to strong-typed LspMessage.
        /// </summary>
        /// <param name="rawData">Received raw data. </param>
        /// <returns>Decoded strong-typed LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspInterceptionResponse Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspInterceptionResponseMsg)))
            {
                return null;
            }

            LspInterceptionResponseMsg msg = TypeMarshal.ToStruct<LspInterceptionResponseMsg>(rawData);
            ProtocolEndPoint endpoint;
            endpoint.protocolType = msg.transportType;
            endpoint.endPoint = msg.interceptedEndPoint.ToIPEndPoint();
            if (endpoint.endPoint != null)
            {
                return new LspInterceptionResponse(new LspSession(msg.sessionId), msg.status, endpoint);
            }
            else
            {
                return null;
            }
        }
    }
}
