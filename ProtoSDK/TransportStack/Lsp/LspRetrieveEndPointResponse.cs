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
    /// LspRetrieveEndPointResponse message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspRetrieveEndPointResponseMsg
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
        /// Client IPEndPoint
        /// </summary>
        public IPEndPointMsg remoteClientEndPoint;

        /// <summary>
        /// Destination IPEndPoint
        /// </summary>
        public IPEndPointMsg destinationEndPoint;
    }

    /// <summary>
    /// LspRetrieveEndPointResponse, sent by LspManagementClient
    /// </summary>
    public class LspRetrieveEndPointResponse : LspMessage
    {
        private LspSession session;
        private int status;
        private IPEndPoint remoteClientEndPoint;
        private IPEndPoint destinationEndPoint;

        /// <summary>
        /// Property of IPEndPoint of remote client.
        /// </summary>
        public IPEndPoint RemoteClientEndPoint
        {
            get
            {
                return this.remoteClientEndPoint;
            }
        }

        /// <summary>
        /// Property of destination of a connection
        /// </summary>
        public IPEndPoint DestinationEndPoint
        {
            get
            {
                return this.destinationEndPoint;
            }
        }


        /// <summary>
        /// Property of LspSession.
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
        /// Constructor.
        /// </summary>
        /// <param name="session">LspSession. </param>
        /// <param name="status">Status of response. </param>
        /// <param name="endPoint">source endpoint. </param>
        /// <param name="destEndPoint">destination endpoint. </param>
        public LspRetrieveEndPointResponse(LspSession session, int status, IPEndPoint endPoint, IPEndPoint destEndPoint)
            : base(LspMessageType.RetrieveEndPointResponse)
        {
            this.session = session;
            this.status = status;
            this.remoteClientEndPoint = endPoint;
            this.destinationEndPoint = destEndPoint;
        }


        /// <summary>
        /// Encode the Lsp message into byte array.
        /// </summary>
        /// <returns>Encoded byte array.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public override byte[] Encode()
        {
            LspRetrieveEndPointResponseMsg msg;
            msg.header.messageType = LspMessageType.RetrieveEndPointResponse;
            msg.header.messageLen = Marshal.SizeOf(typeof(LspRetrieveEndPointResponseMsg));
            msg.sessionId = this.session.SessionId;
            msg.status = this.status;
            msg.remoteClientEndPoint = new IPEndPointMsg(this.remoteClientEndPoint);
            msg.destinationEndPoint = new IPEndPointMsg(this.destinationEndPoint);

            return TypeMarshal.ToBytes<LspRetrieveEndPointResponseMsg>(msg);
        }


        /// <summary>
        /// Decode the raw data into the strong type LspMessage.
        /// </summary>
        /// <param name="rawData">Raw data(byte array) to be decoded.</param>
        /// <returns>Strong type LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspRetrieveEndPointResponse Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspRetrieveEndPointResponseMsg)))
            {
                return null;
            }

            LspRetrieveEndPointResponseMsg msg = TypeMarshal.ToStruct<LspRetrieveEndPointResponseMsg>(rawData);
            IPEndPoint clientEndPoint = msg.remoteClientEndPoint.ToIPEndPoint();
            IPEndPoint destEndPoint = msg.destinationEndPoint.ToIPEndPoint();
            if (clientEndPoint != null && destEndPoint != null)
            {
                return new LspRetrieveEndPointResponse(
                    new LspSession(msg.sessionId),
                    msg.status,
                    clientEndPoint,
                    destEndPoint);
            }
            else
            {
                return null;
            }
        }

    }
}
