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
    /// LspRetrieveEndPointRequest message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspRetrieveEndPointRequestMsg
    {
        /// <summary>
        /// Header
        /// </summary>
        public LspMessageHeader header;

        /// <summary>
        /// Transport type
        /// </summary>
        public int sessionId;

        /// <summary>
        /// Client IPEndPoint
        /// </summary>
        public IPEndPointMsg lspClientEndPoint;
    }

    /// <summary>
    /// LspRetrieveEndPointRequest, sent by LspConsole.
    /// </summary>
    public class LspRetrieveEndPointRequest : LspMessage
    {
        private LspSession session;
        private IPEndPoint lspClientEndPoint;

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
        /// Property of LspClient EndPoint.
        /// </summary>
        public IPEndPoint LspClientEndPoint
        {
            get
            {
                return this.lspClientEndPoint;
            }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="session">LspSession</param>
        /// <param name="lspClientEndPoint">IPEndPoint of LspClient. </param>
        public LspRetrieveEndPointRequest(LspSession session, IPEndPoint lspClientEndPoint)
            : base(LspMessageType.RetrieveEndPointRequest)
        {
            this.session = session;
            this.lspClientEndPoint = lspClientEndPoint;
        }


        /// <summary>
        /// Encode the Lsp message into byte array.
        /// </summary>
        /// <returns>Encoded byte array.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public override byte[] Encode()
        {
            LspRetrieveEndPointRequestMsg msg;
            msg.header.messageType = LspMessageType.RetrieveEndPointRequest;
            msg.header.messageLen = Marshal.SizeOf(typeof(LspRetrieveEndPointRequestMsg));
            msg.sessionId = this.session.SessionId;
            msg.lspClientEndPoint = new IPEndPointMsg(this.lspClientEndPoint);

            return TypeMarshal.ToBytes<LspRetrieveEndPointRequestMsg>(msg);
        }


        /// <summary>
        /// Decode the raw data into the strong type LspMessage.
        /// </summary>
        /// <param name="rawData">Raw data(byte array) to be decoded.</param>
        /// <returns>Strong type LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspRetrieveEndPointRequest Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspRetrieveEndPointRequestMsg)))
            {
                return null;
            }

            LspRetrieveEndPointRequestMsg msg = TypeMarshal.ToStruct<LspRetrieveEndPointRequestMsg>(rawData);
            IPEndPoint clientEndPoint = msg.lspClientEndPoint.ToIPEndPoint();
            if (clientEndPoint != null)
            {
                return new LspRetrieveEndPointRequest(new LspSession(msg.sessionId), clientEndPoint);
            }
            else
            {
                return null;
            }
        }

    }
}
