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
    /// LspUnblockResponse message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspUnblockResponseMsg
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
    }

    /// <summary>
    /// LspUnblockResponse, sent by LspManagementClient
    /// </summary>
    public class LspUnblockResponse : LspMessage
    {
        private int status;
        private LspSession session;

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
        public LspUnblockResponse(LspSession session, int status)
            : base(LspMessageType.UnblockResponse)
        {
            this.session = session;
            this.status = status;
        }


        /// <summary>
        /// Encode the Lsp message into byte array.
        /// </summary>
        /// <returns>Encoded byte array.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public override byte[] Encode()
        {
            LspUnblockResponseMsg msg;
            msg.header.messageType = LspMessageType.UnblockResponse;
            msg.header.messageLen = Marshal.SizeOf(typeof(LspUnblockResponseMsg));
            msg.sessionId = this.session.SessionId;
            msg.status = this.status;

            return TypeMarshal.ToBytes<LspUnblockResponseMsg>(msg);
        }


        /// <summary>
        /// Decode the raw data into the strong type LspMessage.
        /// </summary>
        /// <param name="rawData">Raw data(byte array) to be decoded.</param>
        /// <returns>Strong type LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspUnblockResponse Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspUnblockResponseMsg)))
            {
                return null;
            }

            LspUnblockResponseMsg msg = TypeMarshal.ToStruct<LspUnblockResponseMsg>(rawData);
            return new LspUnblockResponse(new LspSession(msg.sessionId), msg.status);
        }
    }
}
